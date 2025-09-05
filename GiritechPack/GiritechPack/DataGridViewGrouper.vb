Public Class DataGridViewGrouper
    Private Enum HighlightProperty
        Title = 0
        SubTotal = 1
        GrandTotal = 2
    End Enum

    Private RowNum As Integer
    Private DtSource As DataTable
    Private Dgv As DataGridView
    Private DtDestination As DataTable
    Private ColName_Particular As String = Nothing
    Private ColName_ReplaceWithParticular As String = Nothing
    Private Columns_Group As New List(Of String)
    Private Columns_Sum As New List(Of String)

    Private Group1_DataGridViewCellStyle As New DataGridViewCellStyle
    Private Group2_DataGridViewCellStyle As New DataGridViewCellStyle
    Private Group3_DataGridViewCellStyle As New DataGridViewCellStyle
    Private Group4_DataGridViewCellStyle As New DataGridViewCellStyle
    Private GroupGrand_DataGridViewCellStyle As New DataGridViewCellStyle
    Public Summary_View As Boolean
    Public group_Visible As String
    Public Sub New(ByRef Dgv As DataGridView, ByVal DtSource As DataTable)
        If DtSource Is Nothing Then Exit Sub
        Me.Dgv = Dgv
        Me.DtSource = DtSource
        DtDestination = New DataTable
        Me.DtDestination = DtSource.Clone

        Group1_DataGridViewCellStyle.BackColor = Color.LightBlue
        Group1_DataGridViewCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        Group1_DataGridViewCellStyle.ForeColor = Color.Blue

        Group2_DataGridViewCellStyle.BackColor = Color.Beige
        Group2_DataGridViewCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        Group2_DataGridViewCellStyle.ForeColor = Color.Red

        Group3_DataGridViewCellStyle.BackColor = Color.LightGreen
        Group3_DataGridViewCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        Group3_DataGridViewCellStyle.ForeColor = Color.Green

        Group4_DataGridViewCellStyle.BackColor = Color.MistyRose
        Group4_DataGridViewCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        Group4_DataGridViewCellStyle.ForeColor = Color.Black

        GroupGrand_DataGridViewCellStyle.BackColor = Color.LightGoldenrodYellow
        GroupGrand_DataGridViewCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        GroupGrand_DataGridViewCellStyle.ForeColor = Color.Black
    End Sub

    Private IdentityColName As String
    Public Property pIdentityColName() As String
        Get
            Return IdentityColName
        End Get
        Set(ByVal value As String)
            IdentityColName = value
        End Set
    End Property

    Public Property pGroup1_DataGridViewCellStyle() As DataGridViewCellStyle
        Get
            Return Group1_DataGridViewCellStyle
        End Get
        Set(ByVal value As DataGridViewCellStyle)
            Group1_DataGridViewCellStyle = value
        End Set
    End Property

    Public Property pGroup2_DataGridViewCellStyle() As DataGridViewCellStyle
        Get
            Return Group2_DataGridViewCellStyle
        End Get
        Set(ByVal value As DataGridViewCellStyle)
            Group2_DataGridViewCellStyle = value
        End Set
    End Property
    Public Property pGroup3_DataGridViewCellStyle() As DataGridViewCellStyle
        Get
            Return Group3_DataGridViewCellStyle
        End Get
        Set(ByVal value As DataGridViewCellStyle)
            Group3_DataGridViewCellStyle = value
        End Set
    End Property

    Public Property pColName_Particular() As String
        Get
            Return ColName_Particular
        End Get
        Set(ByVal value As String)
            ColName_Particular = value
        End Set
    End Property

    Private Columns_Sort As String
    Public Property pColumns_Sort() As String
        Get
            Return Columns_Sort
        End Get
        Set(ByVal value As String)
            Columns_Sort = value
        End Set
    End Property


    Public Property pColumns_Group() As List(Of String)
        Get
            Return Columns_Group
        End Get
        Set(ByVal value As List(Of String))
            Columns_Group = value
        End Set
    End Property
    Private Filter_Empty_Groups As New List(Of String)
    Public Property p_Filter_Empty_Groups() As List(Of String)
        Get
            Return Filter_Empty_Groups
        End Get
        Set(ByVal value As List(Of String))
            Filter_Empty_Groups = value
        End Set
    End Property

    Private Columns_Sum_FilterString As String = String.Empty
    Public Property pColumns_Sum_FilterString() As String
        Get
            Return Columns_Sum_FilterString
        End Get
        Set(ByVal value As String)
            Columns_Sum_FilterString = value
        End Set
    End Property

    Public Property pColumns_Sum() As List(Of String)
        Get
            Return Columns_Sum
        End Get
        Set(ByVal value As List(Of String))
            Columns_Sum = value
        End Set
    End Property

    Private Columns_Count As New List(Of String)
    Public Property pColumns_Count() As List(Of String)
        Get
            Return Columns_Count
        End Get
        Set(ByVal value As List(Of String))
            Columns_Count = value
        End Set
    End Property

    Public Property pColName_ReplaceWithParticular() As String
        Get
            Return ColName_ReplaceWithParticular
        End Get
        Set(ByVal value As String)
            ColName_ReplaceWithParticular = value
        End Set
    End Property
    Private IssSort As Boolean = True
    Public Property pIssSort() As Boolean
        Get
            Return IssSort
        End Get
        Set(ByVal value As Boolean)
            IssSort = value
        End Set
    End Property
    Private GrandTotal As Boolean = True
    Public Property pGrandTotal() As Boolean
        Get
            Return GrandTotal
        End Get
        Set(ByVal value As Boolean)
            GrandTotal = value
        End Set
    End Property

    Public Sub GroupDgv()
        If DtSource Is Nothing Then Exit Sub
        Dgv.DataSource = Nothing
        RowNum = 1 'Initializing
        'T
        If Summary_View = True Then

        End If
        If pColumns_Group.Count > 0 Then
            If Not DtDestination.Columns.Contains("COLHEAD") Then
                DtDestination.Columns.Add("COLHEAD", GetType(String))
                DtDestination.Columns("COLHEAD").SetOrdinal(DtDestination.Columns.Count - 1)
            End If
            Dgv.DataSource = DtDestination
            'T
            'If Summary_View = False Then
            GroupSource(DtSource.Copy, pColumns_Group.Item(0))
            'End If

            For Each GCol As String In pColumns_Group
                Dgv.Columns(GCol).Visible = False
            Next
            Dgv.Columns(pColName_ReplaceWithParticular).Visible = False
        Else
            If Not DtDestination.Columns.Contains("COLHEAD") Then
                DtDestination.Columns.Add("COLHEAD", GetType(String))
                DtDestination.Columns("COLHEAD").SetOrdinal(DtDestination.Columns.Count - 1)
            End If
            Dgv.DataSource = DtDestination
            For Each RowInsert As DataRow In DtSource.Select(Nothing, pColumns_Sort)
                If pIdentityColName <> Nothing Then RowInsert.Item(pIdentityColName) = RowNum
                DtDestination.ImportRow(RowInsert)
                RowNum += 1
            Next
        End If
        Dgv.Columns("COLHEAD").Visible = False
        ''Inserting Grouper Total
        If pColumns_Sum.Count > 0 And pGrandTotal Then
            Dim Row As DataRow = DtDestination.NewRow
            Row(pColName_Particular) = "GRAND TOTAL"
            Row("COLHEAD") = "G"
            For Each SumColName As String In pColumns_Sum
                Row(SumColName) = DtSource.Compute("SUM([" & SumColName & "])", pColumns_Sum_FilterString)
            Next
            DtDestination.Rows.Add(Row)
            Dgv.Rows(Dgv.RowCount - 1).DefaultCellStyle = GrouperRowStyle("GRANDTOTAL", HighlightProperty.GrandTotal)
        End If
        'T
        'If Summary_View = False Then
        If pColumns_Count.Count > 0 Then
            Dim Row As DataRow = DtDestination.NewRow
            For Each s As String In pColumns_Count
                Row(pColName_Particular) = "NO OF " & s
                Row("COLHEAD") = "G"
                Row(s) = DtSource.Compute("COUNT([" & s & "])", String.Empty)
                DtDestination.Rows.Add(Row)
                Dgv.Rows(Dgv.RowCount - 1).DefaultCellStyle = GrouperRowStyle("GRANDTOTAL", HighlightProperty.GrandTotal)
            Next
        End If
        'End If

        For Each DgvCol As DataGridViewColumn In Dgv.Columns
            DgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub

    Private Sub GroupSource(ByVal DtDataSource As DataTable, ByVal CurrentGrouperName As String)
        Dim FiltStr As String = String.Empty
        Dim Row As DataRow = Nothing
        Dim NextGrouperIndex As Integer = -1
        Dim DvFilter As DataView
        Dim DtFilter As DataTable
        Dim DtDistint As New DataTable
        Dim StrFilter As String = Nothing
        If pIssSort Then
            DtDataSource.DefaultView.Sort = CurrentGrouperName
        Else
            '        DtDataSource.DefaultView.Sort = CurrentGrouperName
        End If
        If pIssSort Then
            DtDistint = DtDataSource.DefaultView.ToTable(True, CurrentGrouperName)
        Else
            DtDistint = DtDataSource.DefaultView.ToTable(True, CurrentGrouperName)
        End If
        For Each RoDist As DataRow In DtDistint.Rows
            If Filter_Empty_Groups.Contains(CurrentGrouperName) Then
                If RoDist(CurrentGrouperName).ToString.Trim = "" Then
                    GoTo NextToTitleInstertion
                End If
            End If
            ''Inserting Title
            If Summary_View = False Then
                Row = DtDestination.NewRow
                Row(pColName_Particular) = GrouperSpace(CurrentGrouperName) & RoDist(CurrentGrouperName).ToString
                Row("COLHEAD") = GrouperColHead(CurrentGrouperName, HighlightProperty.Title)
                DtDestination.Rows.Add(Row)
                Dgv.Rows(Dgv.RowCount - 1).Cells(pColName_Particular).Style.BackColor = GrouperRowStyle(CurrentGrouperName, HighlightProperty.Title).BackColor
                Dgv.Rows(Dgv.RowCount - 1).DefaultCellStyle.Font = GrouperRowStyle(CurrentGrouperName, HighlightProperty.Title).Font
                Dgv.Rows(Dgv.RowCount - 1).DefaultCellStyle.ForeColor = GrouperRowStyle(CurrentGrouperName, HighlightProperty.Title).ForeColor
            End If
NextToTitleInstertion:
            DvFilter = New DataView
            DvFilter = DtDataSource.DefaultView
            If RoDist(CurrentGrouperName).ToString = "" Then
                StrFilter = CurrentGrouperName & " = '" & RoDist(CurrentGrouperName).ToString & "' OR " & CurrentGrouperName & " IS NULL "
            Else
                StrFilter = CurrentGrouperName & " = '" & RoDist(CurrentGrouperName).ToString & "'"
            End If
            DvFilter.RowFilter = StrFilter
            DtFilter = New DataTable
            DtFilter = DvFilter.ToTable
            DtFilter.AcceptChanges()

            ''Check whether Next Grouper Available
            NextGrouperIndex = NextGrouper(CurrentGrouperName)
            If NextGrouperIndex = -1 Then
                ''There's no next group
                For Each RowInsert As DataRow In DtFilter.Select(StrFilter, CurrentGrouperName & IIf(pColumns_Sort <> "" And Summary_View = False, "," & pColumns_Sort, ""))
                    If DtFilter.Columns(pColName_ReplaceWithParticular).DataType.Name = GetType(DateTime).Name Then
                        If RowInsert.Item(pColName_ReplaceWithParticular).ToString <> "" Then
                            RowInsert.Item(pColName_Particular) = GrouperSpace(CurrentGrouperName) & "  " & Format(RowInsert.Item(pColName_ReplaceWithParticular), "dd/MM/yyyy")
                        End If
                    Else
                        RowInsert.Item(pColName_Particular) = GrouperSpace(CurrentGrouperName) & "  " & RowInsert.Item(pColName_ReplaceWithParticular)
                    End If
                    If pIdentityColName <> Nothing Then RowInsert.Item(pIdentityColName) = RowNum
                    DtDestination.ImportRow(RowInsert)
                    RowNum += 1
                Next
                If Filter_Empty_Groups.Contains(CurrentGrouperName) Then
                    If RoDist(CurrentGrouperName).ToString.Trim = "" Then
                        GoTo NextToGroupTotalInstertion
                    End If
                End If
                If Summary_View = False Then
                    ''Inserting Grouper Total
                    If pColumns_Sum.Count > 0 Then
                        Row = DtDestination.NewRow
                        Row(pColName_Particular) = GrouperSpace(CurrentGrouperName) & RoDist(CurrentGrouperName).ToString & " TOTAL"
                        Row("COLHEAD") = GrouperColHead(CurrentGrouperName, HighlightProperty.SubTotal)
                        For Each SumColName As String In pColumns_Sum
                            FiltStr = pColumns_Sum_FilterString
                            If RoDist(CurrentGrouperName).ToString = "" Then
                                If FiltStr <> "" Then FiltStr += " AND "
                                FiltStr += " (" & CurrentGrouperName & " = '' OR " & CurrentGrouperName & " IS NULL) "
                                'Else
                                '    FiltStr = String.Empty
                            End If
                            Row(SumColName) = DtFilter.Compute("SUM([" & SumColName & "])", FiltStr)
                        Next
                        DtDestination.Rows.Add(Row)
                        'Dgv.Rows(Dgv.RowCount - 1).DefaultCellStyle = GrouperRowStyle(CurrentGrouperName)
                        'Dgv.Rows(Dgv.RowCount - 1).Cells(pColName_Particular).Style.BackColor = GrouperRowStyle(CurrentGrouperName, HighlightProperty.SubTotal).BackColor
                        Dgv.Rows(Dgv.RowCount - 1).DefaultCellStyle.Font = GrouperRowStyle(CurrentGrouperName, HighlightProperty.SubTotal).Font
                        Dgv.Rows(Dgv.RowCount - 1).DefaultCellStyle.ForeColor = GrouperRowStyle(CurrentGrouperName, HighlightProperty.SubTotal).ForeColor
                    End If
                End If

NextToGroupTotalInstertion:
            Else
                GroupSource(DtFilter, pColumns_Group.Item(NextGrouperIndex))
                If Filter_Empty_Groups.Contains(CurrentGrouperName) And DtFilter.Rows.Count > 1 Then
                    If RoDist(CurrentGrouperName).ToString = "" Then
                        GoTo NextToGroupTotalInstertion2
                    End If
                End If
                If Summary_View = False Then
                    ''Inserting Grouper Total
                    If pColumns_Sum.Count > 0 Then
                        Row = DtDestination.NewRow
                        Row(pColName_Particular) = GrouperSpace(CurrentGrouperName) & RoDist(CurrentGrouperName).ToString & " TOTAL"
                        Row("COLHEAD") = GrouperColHead(CurrentGrouperName, HighlightProperty.SubTotal)
                        For Each SumColName As String In pColumns_Sum
                            FiltStr = pColumns_Sum_FilterString
                            If RoDist(CurrentGrouperName).ToString = "" Then
                                If FiltStr <> "" Then FiltStr += " AND "
                                FiltStr += " (" & CurrentGrouperName & " = '' OR " & CurrentGrouperName & " IS NULL) "
                            End If
                            Row(SumColName) = DtFilter.Compute("SUM([" & SumColName & "])", FiltStr)
                        Next
                        DtDestination.Rows.Add(Row)
                        'Dgv.Rows(Dgv.RowCount - 1).DefaultCellStyle = GrouperRowStyle(CurrentGrouperName)
                        'Dgv.Rows(Dgv.RowCount - 1).Cells(pColName_Particular).Style.BackColor = GrouperRowStyle(CurrentGrouperName, HighlightProperty.SubTotal).BackColor
                        Dgv.Rows(Dgv.RowCount - 1).DefaultCellStyle.Font = GrouperRowStyle(CurrentGrouperName, HighlightProperty.SubTotal).Font
                        Dgv.Rows(Dgv.RowCount - 1).DefaultCellStyle.ForeColor = GrouperRowStyle(CurrentGrouperName, HighlightProperty.SubTotal).ForeColor
                    End If
                End If

NextToGroupTotalInstertion2:
            End If
        Next
    End Sub

    Private Function GrouperRowStyle(ByVal CurrentGrouper As String, ByVal StyleFor As HighlightProperty) As DataGridViewCellStyle
        Dim DgvCellStyle As New DataGridViewCellStyle
        If pColumns_Group.IndexOf(CurrentGrouper) = 0 Then
            DgvCellStyle = Group1_DataGridViewCellStyle
        ElseIf pColumns_Group.IndexOf(CurrentGrouper) = 1 Then
            DgvCellStyle = Group2_DataGridViewCellStyle
        ElseIf pColumns_Group.IndexOf(CurrentGrouper) = 2 Then
            DgvCellStyle = Group3_DataGridViewCellStyle
        Else
            ''Grand Total
            DgvCellStyle = GroupGrand_DataGridViewCellStyle
        End If
        Return DgvCellStyle
    End Function
    Private Function GrouperColHead(ByVal CurrentGrouper As String, ByVal HeadFor As HighlightProperty) As String
        Dim RetStr As String = ""
        Dim T As String = "T"
        Dim S As String = "S"
        If pColumns_Group.IndexOf(CurrentGrouper) > 0 Then
            T = T & pColumns_Group.IndexOf(CurrentGrouper).ToString
            S = S & pColumns_Group.IndexOf(CurrentGrouper).ToString
        End If
        Select Case HeadFor
            Case HighlightProperty.Title
                RetStr = T
            Case HighlightProperty.SubTotal
                RetStr = S
        End Select
        Return RetStr
    End Function

    Private Function GrouperSpace(ByVal CurrentGrouper As String) As String
        Dim Str As String = ""
        For cnt As Integer = 0 To pColumns_Group.IndexOf(CurrentGrouper) - 1
            Str += "  "
        Next
        Return Str
    End Function

    Private Function NextGrouper(ByVal CurrentGrouper As String) As Integer
        Dim NextIndex As Integer = pColumns_Group.IndexOf(CurrentGrouper) + 1
        If Not NextIndex <= pColumns_Group.Count - 1 Then
            NextIndex = -1
        End If
        Return NextIndex
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
