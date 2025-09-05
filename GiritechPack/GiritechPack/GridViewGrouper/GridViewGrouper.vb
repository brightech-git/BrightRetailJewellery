Public Class GridViewGrouper
    Shared dtGridView As New DataTable
    Shared dtResult As New DataTable
    Shared groupCol As New List(Of String)
    Shared dgv1 As New DataGridView
    Shared sumOfCol As List(Of String)

    Public Shared Sub Show(ByVal dtSource As DataTable, _
    Optional ByVal notVisibleCols As List(Of String) = Nothing, _
    Optional ByVal defaultGroupCols As List(Of String) = Nothing, _
    Optional ByVal defaultSumCols As List(Of String) = Nothing)
        Dim objDataView As New frmDataView(dtSource)
        objDataView.ShowDialog()
    End Sub

    Public Shared Sub GridView_Grouper(ByVal dgv As DataGridView, _
  ByVal notVisibleColumns As List(Of String), _
  ByVal groupColumns As List(Of String) _
  , Optional ByVal sumOfColumns As List(Of String) = Nothing, Optional ByVal sortOfColumns As String = "")
        dtGridView = dgv.DataSource
        groupCol = groupColumns

        Dim parCol As New DataColumn("PARCOL", GetType(String))
        If Not dtGridView.Columns.Contains("PARCOL") Then
            dtGridView.Columns.Add(parCol)
        End If

        dtResult = New DataTable
        dtResult.Columns.Add("PARCOL", GetType(String))
        For Each col As DataColumn In dtGridView.Copy.Columns
            If col.ColumnName = "PARCOL" Then Continue For
            Dim newCol As New DataColumn
            newCol.ColumnName = col.ColumnName
            newCol.DataType = col.DataType
            newCol.MaxLength = col.MaxLength
            dtResult.Columns.Add(newCol)
        Next
        dgv.DataSource = Nothing
        dgv.DataSource = dtResult
        dgv1 = dgv
        For Each str As String In groupColumns
            If Not sumOfColumns Is Nothing Then
                If sumOfColumns.Count > 0 Then
                    If sumOfColumns.Contains(str) Then sumOfColumns.Remove(str)
                End If
            End If
        Next

        sumOfCol = sumOfColumns

        Dim dtGroup As New DataTable
        dtGroup = dtGridView.DefaultView.ToTable(True, groupColumns.ToArray)
        Dim sort As String = Nothing
        For cnt As Integer = 0 To groupColumns.Count - 1
            sort += dtGridView.Columns(cnt).ColumnName + " ASC "
            If cnt <> groupColumns.Count - 1 Then
                sort += ","
            Else
                If sortOfColumns <> "" Then sort += "," + sortOfColumns
                sort += "," + dtGridView.Columns(cnt + 1).ColumnName + " ASC"
            End If
        Next
        dtGridView.DefaultView.Sort = sort

        If groupColumns.Count = 1 Then
            Group1()
        ElseIf groupColumns.Count = 2 Then
            Group2()
        End If

        dgv.Columns("PARCOL").DisplayIndex = 0
        dgv.Columns("PARCOL").HeaderText = "PARTICULAR"
        dgv.Columns("PARCOL").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        For Each dgvCol As DataGridViewColumn In dgv.Columns
            If groupColumns.Contains(dgvCol.Name) Then dgvCol.Visible = False
            dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
        Next
        StyleGridColumns(dgv, notVisibleColumns)
        For Each dgvCol As String In groupColumns
            dgv.Columns(dgvCol).Visible = False
        Next
        If Not groupColumns Is Nothing Then
            Dim index As Integer = 0
            For cnt As Integer = 0 To dgv.ColumnCount
                If dgv.Columns(cnt).Name = "PARCOL" Then Continue For
                If Not groupCol.Contains(dgv.Columns(cnt).Name) Then
                    index = cnt
                    dgv.Columns(index).Visible = False
                    Exit For
                End If
            Next
        End If
    End Sub

    Public Shared Sub StyleGridColumns(ByVal dgv As DataGridView, ByVal notVisibleCols As List(Of String))
        For Each dgvCol As DataGridViewColumn In dgv.Columns
            dgvCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            If Not notVisibleCols Is Nothing Then
                If notVisibleCols.Contains(dgvCol.Name) Then dgvCol.Visible = False
            End If
            Select Case dgvCol.ValueType.FullName
                Case GetType(DateTime).FullName
                    dgvCol.DefaultCellStyle.Format = "dd/MM/yyyy"
                Case GetType(Int16).FullName, GetType(Int32).FullName, GetType(Int64).FullName, GetType(Integer).FullName, _
                GetType(Decimal).FullName, GetType(Double).FullName
                    dgvCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End Select
        Next
    End Sub

    Private Shared Sub Group1()
        For Each roGroup As DataRow In dtGridView.DefaultView.ToTable(True, groupCol.ToArray).Rows
            ''Add Header
            AddHeader(roGroup(0))
            ''Add Row by Row
            Dim strFil As String = Nothing
            Select Case roGroup.Table.Columns(groupCol.Item(0)).DataType.FullName
                Case GetType(Decimal).FullName, GetType(Double).FullName, GetType(Int32).FullName, _
                GetType(Int64).FullName, GetType(Integer).FullName, GetType(Int16).FullName
                    strFil = "ISNULL(" + dtGridView.Columns(groupCol(0)).ColumnName + ",0)" + " = " & Val(roGroup(0).ToString) & ""
                Case Else
                    strFil = "ISNULL(" + dtGridView.Columns(groupCol(0)).ColumnName + ",'')" + " = '" & roGroup(0).ToString & "'"
            End Select
            Dim strSort As String = dtGridView.Columns(groupCol(0)).ColumnName + " ASC"
            AddRows(dtGridView.Select(strFil, strSort))
            ''Add Sub Total
            AddSubTotal(roGroup(0).ToString, strFil)
        Next
        ''Add Grand Total
        AddGrandTotal()
    End Sub


    Private Shared Sub Group2()
        Dim head1 As String = Nothing
        Dim lastTot As String = Nothing
        Dim dtGroup As New DataTable
        dtGroup = dtGridView.DefaultView.ToTable(True, groupCol.ToArray).DefaultView.ToTable.Copy
        For cnt As Integer = 0 To dtGroup.Rows.Count - 1
            With dtGroup.Rows(cnt)
                Dim head2 As String = Nothing
                If head1 <> .Item(0).ToString Then
                    ''Add Header
                    head1 = .Item(0).ToString
                    AddHeader(.Item(0))
                End If
                If head1 = .Item(0).ToString And head2 <> .Item(1).ToString Then
                    ''Add Header
                    head2 = .Item(1).ToString
                    AddHeader(.Item(1), 2)
                End If
                ''Add Row by Row
                Dim strFil As String = Nothing
                Dim strSort As String = Nothing
                For i As Integer = 0 To groupCol.Count - 1
                    Select Case dtGroup.Columns(groupCol.Item(i)).DataType.FullName
                        Case GetType(Decimal).FullName, GetType(Double).FullName, GetType(Int32).FullName, _
                        GetType(Int64).FullName, GetType(Integer).FullName, GetType(Int16).FullName
                            strFil += "ISNULL(" + dtGridView.Columns(groupCol(i)).ColumnName + ",0)" + " = " & Val(.Item(groupCol.Item(i)).ToString) & ""
                        Case Else
                            strFil += "ISNULL(" + dtGridView.Columns(groupCol(i)).ColumnName + ",'')" + " = '" & .Item(groupCol.Item(i)).ToString & "'"
                    End Select
                    'strFil += dtGridView.Columns(groupCol.Item(i)).ColumnName + " = '" & .Item(groupCol.Item(i)).ToString & "'"
                    strSort += dtGridView.Columns(groupCol.Item(i)).ColumnName + " ASC"
                    If i <> groupCol.Count - 1 Then
                        strFil += " AND "
                        strSort += " , "
                    End If
                Next
                AddRows(dtGridView.Select(strFil, strSort), 4)

                AddSubTotal(.Item(1), strFil, 2)
                If cnt <> dtGroup.Rows.Count - 1 Then
                    If dtGroup.Rows(cnt + 1).Item(0).ToString <> .Item(0).ToString Then
                        strFil = dtGridView.Columns(0).ColumnName + " = '" & .Item(0).ToString & "'"
                        AddSubTotal(.Item(0), strFil)
                    End If
                Else
                    strFil = dtGridView.Columns(0).ColumnName + " = '" & .Item(0).ToString & "'"
                    AddSubTotal(.Item(0), strFil, 2)
                End If
            End With
        Next
        ''Add Grand Total
        AddGrandTotal()
    End Sub
    Private Shared Sub AddRows(ByVal foundRows() As DataRow, Optional ByVal noOfHead As Integer = 0)
        For Each ro As DataRow In foundRows
            Dim index As Integer = 0
            For cnt As Integer = 0 To dtGridView.Columns.Count - 1
                If Not groupCol.Contains(dtGridView.Columns(cnt).ColumnName) Then
                    index = cnt
                    Exit For
                End If
            Next
            Dim obj As Object = ro(index)
            If dtGridView.Columns(index).DataType.FullName = GetType(DateTime).FullName Then
                obj = Format(obj, "dd/MM/yyyy")
            End If
            ro("PARCOL") = Space(noOfHead + 1) & obj
            dtResult.ImportRow(ro)
        Next
    End Sub

    Private Shared Sub AddHeader(ByVal strHead As Object, Optional ByVal noOfHead As Integer = 0)
        If TypeOf strHead Is DateTime Then
            strHead = Format(strHead, "dd/MM/yyyy")
        End If
        Dim roHeader As DataRow = dtResult.NewRow
        roHeader!PARCOL = Space(noOfHead + 1) & strHead
        dtResult.Rows.Add(roHeader)
        dgv1.Rows(dtResult.Rows.Count - 1).Cells("PARCOL").Style.BackColor = SystemColors.Control
    End Sub

    Private Shared Sub AddSubTotal(ByVal str As Object, ByVal strFilt As String, Optional ByVal noOfHead As Integer = 0)
        If Not sumOfCol Is Nothing Then
            If sumOfCol.Count > 0 Then
                Dim roTotal As DataRow = dtResult.NewRow
                roTotal!PARCOL = Space(noOfHead + 1) & IIf(TypeOf str Is DateTime, Format(str, "dd/MM/yyyy"), str)
                For Each sumColName As String In sumOfCol
                    If groupCol.Contains(sumColName) Then Continue For
                    roTotal(sumColName) = dtGridView.Compute("SUM(" & sumColName & ")", strFilt)
                Next
                dtResult.Rows.Add(roTotal)
                dgv1.Rows(dtResult.Rows.Count - 1).DefaultCellStyle.ForeColor = Color.Blue
            End If
        End If
    End Sub

    Private Shared Sub AddGrandTotal()
        If Not sumOfCol Is Nothing Then
            If sumOfCol.Count > 0 Then
                Dim roTotal As DataRow = dtResult.NewRow
                roTotal!PARCOL = " GRAND TOTAL"
                For Each sumColName As String In sumOfCol
                    roTotal(sumColName) = dtGridView.Compute("SUM(" & sumColName & ")", Nothing)
                Next
                dtResult.Rows.Add(roTotal)
                dgv1.Rows(dtResult.Rows.Count - 1).DefaultCellStyle.BackColor = SystemColors.Control
            End If
        End If
    End Sub
End Class
