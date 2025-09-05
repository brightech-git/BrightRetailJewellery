Public Class PrintLabel
    'Dim dtGridView As New DataTable
    'Dim noOfLables As Integer = 2
    'Dim row As DataRow = Nothing
    'Dim dt As New DataTable
#Region "Old Style"
    'Dim dtGridView As New DataTable
    'Dim pageRowCount As Integer = 5
    'Dim row As DataRow = Nothing
    'Dim dt As New DataTable
    'noOfLables = lblCount
    'Try
    '    dt = grid.DataSource
    '    Dim col As DataColumn = Nothing
    '    Dim lbCount As Integer
    '    With dtGridView.Columns
    '        For lbCount = 0 To noOfLables - 1
    '            For cnt As Integer = 0 To dt.Columns.Count - 1
    '                If Not grid.Columns(cnt).Visible Then Continue For
    '                If Not SelectedColumns.Contains(grid.Columns(cnt).HeaderText) Then Continue For
    '                col = New DataColumn
    '                col.ColumnName = dt.Columns(cnt).ColumnName.ToString + "~" + (lbCount).ToString
    '                col.DataType = dt.Columns(cnt).DataType
    '                col.Caption = grid.Columns(cnt).HeaderText
    '                dtGridView.Columns.Add(col)
    '            Next
    '            'If lbCount <> noOfLables Then
    '            '    col = New DataColumn
    '            '    col.ColumnName = "Sep_" + lbCount.ToString
    '            '    'col.Caption = dt.Columns(cnt).Caption
    '            '    dtGridView.Columns.Add(col)
    '            'End If
    '        Next
    '    End With

    '    Dim rwCount As Integer = 0
    '    Dim spRwIndex As Integer = 0
    '    lbCount = 0
    '    Dim rwIndex As Integer = 0
    '    For i As Integer = 0 To dt.Rows.Count - 1
    '        If rwCount = pageRowCount Then
    '            rwCount = 0
    '            lbCount += 1
    '            If lbCount <= noOfLables - 1 Then
    '                rwIndex = (spRwIndex * pageRowCount) ''+ IIf(spRwIndex <> 0, spRwIndex, 0)
    '            Else
    '                spRwIndex += 1
    '                rwIndex = (spRwIndex * pageRowCount) '' + spRwIndex
    '                lbCount = 0
    '                'dtGridView.Rows.Add()
    '            End If
    '        End If
    '        If lbCount = 0 Then
    '            dtGridView.Rows.Add()
    '        End If
    '        For j As Integer = 0 To dt.Columns.Count - 1
    '            If Not grid.Columns(j).Visible Then Continue For
    '            If Not SelectedColumns.Contains(grid.Columns(j).HeaderText) Then Continue For
    '            dtGridView.Rows(rwIndex).Item(dt.Columns(j).ColumnName.ToString + "~" + lbCount.ToString) = _
    '            dt.Rows(i).Item(dt.Columns(j).ColumnName.ToString)
    '        Next
    '        rwIndex += 1
    '        rwCount += 1
    '    Next
    '    gridView.DataSource = dtGridView
    '    gridView.Font = grid.Font
    '    gridView.DefaultCellStyle = grid.DefaultCellStyle
    '    gridView.AlternatingRowsDefaultCellStyle = grid.AlternatingRowsDefaultCellStyle
    '    gridView.ColumnHeadersDefaultCellStyle = grid.ColumnHeadersDefaultCellStyle
    '    gridView.ColumnHeadersHeight = grid.ColumnHeadersHeight
    '    gridView.ColumnHeadersHeightSizeMode = grid.ColumnHeadersHeightSizeMode
    '    gridView.ColumnHeadersVisible = grid.ColumnHeadersVisible
    '    For cnt As Integer = 0 To gridView.ColumnCount - 1
    '        With gridView.Columns(cnt)
    '            .HeaderText = dtGridView.Columns(cnt).Caption
    '            Dim s() As String
    '            s = dtGridView.Columns(cnt).ColumnName.Split("~")
    '            .Width = grid.Columns(s(0)).Width
    '            .DefaultCellStyle = grid.Columns(s(0)).DefaultCellStyle
    '        End With
    '    Next
    'Catch ex As Exception
    '    MsgBox(ex.Message)
    '    MsgBox(ex.StackTrace)
    'End Try
#End Region

    Public Sub New(ByVal lableStyle As Boolean, ByVal grid As DataGridView, ByVal noOfLables As Integer, ByVal SelectedColumns As List(Of String), ByVal pgLandscape As Boolean, ByVal rowHeight As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        If Not noOfLables > 0 Then noOfLables = 1
        ' Add any initialization after the InitializeComponent() call.
        gridView.Font = grid.Font
        gridView.DefaultCellStyle = grid.DefaultCellStyle
        gridView.AlternatingRowsDefaultCellStyle = grid.AlternatingRowsDefaultCellStyle
        gridView.ColumnHeadersDefaultCellStyle = grid.ColumnHeadersDefaultCellStyle
        gridView.ColumnHeadersHeight = grid.ColumnHeadersHeight
        gridView.ColumnHeadersHeightSizeMode = grid.ColumnHeadersHeightSizeMode
        gridView.ColumnHeadersVisible = grid.ColumnHeadersVisible
        'gridView.RowTemplate = grid.RowTemplate
        gridView.RowTemplate.Height = rowHeight
        If Not lableStyle Then
            gridView.DataSource = CType(grid.DataSource, DataTable)
            For cnt As Integer = 0 To grid.ColumnCount - 1
                If Not grid.Columns(cnt).Visible Then Continue For
                If Not SelectedColumns.Contains(grid.Columns(cnt).HeaderText) Then Continue For
                With gridView.Columns(cnt)
                    .Width = grid.Columns(cnt).Width
                    .DefaultCellStyle = grid.Columns(cnt).DefaultCellStyle
                    '.Name = grid.Columns(cnt).Name + "~" + (lbCount).ToString
                    .HeaderText = grid.Columns(cnt).HeaderText
                End With
            Next
            Exit Sub
        End If

        Dim dgCell As DataGridViewImageCell
        Dim dtGridView As New DataTable
        Dim pageRowCount As Integer = IIf(pgLandscape, (Math.Ceiling(595 / grid.RowTemplate.Height)), (Math.Ceiling(848 / grid.RowTemplate.Height)))

        Try
            Dim colText As DataGridViewTextBoxColumn = Nothing
            Dim colImage As DataGridViewImageColumn
            Dim colType As Object = Nothing
            Dim lbCount As Integer
            With gridView.Columns
                For lbCount = 0 To noOfLables - 1
                    For cnt As Integer = 0 To grid.Columns.Count - 1
                        If Not grid.Columns(cnt).Visible Then Continue For
                        If Not SelectedColumns.Contains(grid.Columns(cnt).HeaderText) Then Continue For
                        If grid.Columns(cnt).GetType Is GetType(DataGridViewTextBoxColumn) Then
                            colText = New DataGridViewTextBoxColumn
                            colText.Width = grid.Columns(cnt).Width
                            colText.DefaultCellStyle = grid.Columns(cnt).DefaultCellStyle
                            colText.Name = grid.Columns(cnt).Name + "~" + (lbCount).ToString
                            colText.HeaderText = grid.Columns(cnt).HeaderText
                            gridView.Columns.Add(colText)
                        ElseIf grid.Columns(cnt).GetType Is GetType(DataGridViewImageColumn) Then
                            colImage = New DataGridViewImageColumn
                            colImage.DefaultCellStyle.NullValue = My.Resources.emptyImage
                            colImage.ImageLayout = DataGridViewImageCellLayout.Stretch
                            colImage.Image = My.Resources.emptyImage

                            colImage.Width = grid.Columns(cnt).Width
                            colImage.DefaultCellStyle = grid.Columns(cnt).DefaultCellStyle
                            colImage.Name = grid.Columns(cnt).Name + "~" + (lbCount).ToString
                            colImage.HeaderText = grid.Columns(cnt).HeaderText
                            gridView.Columns.Add(colImage)
                        End If
                    Next
                Next
            End With

            Dim rwCount As Integer = 0
            Dim spRwIndex As Integer = 0
            lbCount = 0
            Dim rwIndex As Integer = 0
            For i As Integer = 0 To grid.RowCount - 1
                If rwCount = pageRowCount Then
                    rwCount = 0
                    lbCount += 1
                    If lbCount <= noOfLables - 1 Then
                        rwIndex = (spRwIndex * pageRowCount) ''+ IIf(spRwIndex <> 0, spRwIndex, 0)
                    Else
                        spRwIndex += 1
                        rwIndex = (spRwIndex * pageRowCount) '' + spRwIndex
                        lbCount = 0
                        'dtGridView.Rows.Add()
                    End If
                End If
                If lbCount = 0 Then
                    gridView.Rows.Add()
                End If
                For j As Integer = 0 To grid.ColumnCount - 1
                    If Not grid.Columns(j).Visible Then Continue For
                    If Not SelectedColumns.Contains(grid.Columns(j).HeaderText) Then Continue For
                    If grid.Columns(j).GetType Is GetType(DataGridViewTextBoxColumn) Then
                        gridView.Rows(rwIndex).Cells(grid.Columns(j).Name.ToString + "~" + lbCount.ToString).Value = _
                        grid.Rows(i).Cells(grid.Columns(j).Name.ToString).Value
                    ElseIf grid.Columns(j).GetType Is GetType(DataGridViewImageColumn) Then
                        dgCell = New DataGridViewImageCell
                        dgCell.ImageLayout = DataGridViewImageCellLayout.Stretch
                        dgCell.Value = grid.Rows(i).Cells(grid.Columns(j).Name.ToString).Value
                        gridView.Rows(rwIndex).Cells(grid.Columns(j).Name.ToString + "~" + lbCount.ToString).Value = dgCell.Value
                    End If
                Next
                rwIndex += 1
                rwCount += 1
            Next
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Sub
End Class