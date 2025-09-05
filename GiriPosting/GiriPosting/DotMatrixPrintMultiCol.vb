Public Class DotMatrixPrintMultiCol
    Public dgv As New DataGridView
    Public pageRowCount As Integer
    Public AvailableColumns As List(Of String)
    Public noOfSplit As Integer
    Public dtGridView As New DataTable
    Public AutoFitColumns As Boolean



    Public Sub New(ByRef dgv As DataGridView, ByVal noOfSplit As Integer, ByVal pageRowCount As Integer, ByVal AvailableColumns As List(Of String), ByVal AutoFitColumns As Boolean)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Me.dgv = dgv
        Me.pageRowCount = pageRowCount
        Me.AvailableColumns = AvailableColumns
        Me.noOfSplit = noOfSplit
        Me.AutoFitColumns = AutoFitColumns

        ' Add any initialization after the InitializeComponent() call.
        If AutoFitColumns And Not dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells Then
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        Else
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        End If
        gridView.Font = dgv.Font
        gridView.DefaultCellStyle = dgv.DefaultCellStyle
        gridView.AutoSizeRowsMode = dgv.AutoSizeRowsMode
        gridView.AlternatingRowsDefaultCellStyle = dgv.AlternatingRowsDefaultCellStyle
        gridView.ColumnHeadersDefaultCellStyle = dgv.ColumnHeadersDefaultCellStyle
        gridView.ColumnHeadersHeight = dgv.ColumnHeadersHeight
        gridView.ColumnHeadersHeightSizeMode = dgv.ColumnHeadersHeightSizeMode
        gridView.ColumnHeadersVisible = dgv.ColumnHeadersVisible
        gridView.RowHeadersVisible = False
        For i As Integer = 0 To noOfSplit - 1
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                If Not ((dgvCol.Name.ToUpper = "COLHEAD" Or dgvCol.Name.ToUpper = "PAGEBREAK") And noOfSplit = 1) Then
                    If Not AvailableColumns.Contains(dgvCol.Name) Then Continue For
                End If
                Dim dtCol As New DataColumn
                dtCol.ColumnName = dgvCol.Name & "~" & i.ToString
                dtCol.DataType = dgvCol.ValueType
                dtCol.Caption = dgvCol.HeaderText
                dtGridView.Columns.Add(dtCol)
            Next
        Next
        gridView.DataSource = dtGridView
        dtGridView.AcceptChanges()
        For Each gridCol As DataGridViewColumn In gridView.Columns
            Dim sp() As String = gridCol.Name.Split("~")
            Dim colName As String = sp(0)
            If Not AutoFitColumns Then
                'gridCol.MinimumWidth = dgv.Columns(colName).Width
                gridCol.Width = dgv.Columns(colName).Width
            End If
            gridCol.CellTemplate = dgv.Columns(colName).CellTemplate
            gridCol.DefaultCellStyle = dgv.Columns(colName).DefaultCellStyle
            gridCol.HeaderText = dgv.Columns(colName).HeaderText
        Next
        'If AutoFitColumns And dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells Then
        '    If Not noOfSplit > 1 Then 'normal posting
        '        Dim rwIndex As Integer = 0
        '        For Each dgvRow As DataGridViewRow In dgv.Rows
        '            dtGridView.Rows.Add()
        '            For Each dgvCell As DataGridViewCell In dgvRow.Cells
        '                If Not ((dgvCell.OwningColumn.Name.ToUpper = "COLHEAD" Or dgvCell.OwningColumn.Name.ToUpper = "PAGEBREAK") And noOfSplit = 1) Then
        '                    If Not AvailableColumns.Contains(dgvCell.OwningColumn.Name) Then Continue For
        '                End If
        '                With gridView.Rows(rwIndex).Cells(dgvCell.OwningColumn.Name + "~0")
        '                    If dgvRow.Cells(dgvCell.OwningColumn.Name).Visible Then
        '                        dgv.CurrentCell = dgvRow.Cells(dgvCell.OwningColumn.Name)
        '                    End If
        '                    .Value = dgvCell.Value
        '                    .Style.BackColor = dgvCell.InheritedStyle.BackColor
        '                    .Style.Font = dgvCell.InheritedStyle.Font
        '                End With
        '            Next
        '            rwIndex += 1
        '        Next
        '    End If
        'End If
        Me.Hide()
    End Sub

    Public Function GetGridView() As DataGridView
        Return gridView
    End Function

    Private Sub frmPrintMultiCol_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If AutoFitColumns And dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells Then
            For Each gridCol As DataGridViewColumn In gridView.Columns
                Dim sp() As String = gridCol.Name.Split("~")
                Dim colName As String = sp(0)
                gridCol.Width = dgv.Columns(colName).Width
            Next
            Exit Sub
        End If
        'If noOfSplit > 1 Then
        Dim rwCount As Integer = 0
        Dim spRwIndex As Integer = 0
        Dim lbCount As Integer = 0
        Dim rwIndex As Integer = 0

        For Each dgvRow As DataGridViewRow In dgv.Rows
            If rwCount = pageRowCount Then
                rwCount = 0
                lbCount += 1
                If lbCount <= noOfSplit - 1 Then
                    rwIndex = (spRwIndex * pageRowCount)
                Else
                    spRwIndex += 1
                    rwIndex = (spRwIndex * pageRowCount)
                    lbCount = 0
                End If
            End If
            If lbCount = 0 Then dtGridView.Rows.Add()
            For Each dgvCell As DataGridViewCell In dgvRow.Cells
                If Not ((dgvCell.OwningColumn.Name.ToUpper = "COLHEAD" Or dgvCell.OwningColumn.Name.ToUpper = "PAGEBREAK") And noOfSplit = 1) Then
                    If Not AvailableColumns.Contains(dgvCell.OwningColumn.Name) Then Continue For
                End If
                With gridView.Rows(rwIndex).Cells(dgvCell.OwningColumn.Name + "~" + lbCount.ToString)
                    If dgvRow.Cells(dgvCell.OwningColumn.Name).Visible Then
                        dgv.CurrentCell = dgvRow.Cells(dgvCell.OwningColumn.Name)
                    End If
                    .Value = dgvCell.Value
                    .Style.BackColor = dgvCell.InheritedStyle.BackColor
                    .Style.Font = dgvCell.InheritedStyle.Font
                End With
            Next
            rwIndex += 1
            rwCount += 1
        Next
        'End If

        'If Not noOfSplit > 1 Then 'normal posting
        '    Dim rwIndex As Integer = 0
        '    For Each dgvRow As DataGridViewRow In dgv.Rows
        '        dtGridView.Rows.Add()
        '        For Each dgvCell As DataGridViewCell In dgvRow.Cells
        '            If Not ((dgvCell.OwningColumn.Name.ToUpper = "COLHEAD" Or dgvCell.OwningColumn.Name.ToUpper = "PAGEBREAK") And noOfSplit = 1) Then
        '                If Not AvailableColumns.Contains(dgvCell.OwningColumn.Name) Then Continue For
        '            End If
        '            With dtGridView.Rows(rwIndex)
        '                .Item(dgvCell.OwningColumn.Name + "~0") = dgvCell.Value
        '            End With
        '            'With gridView.Rows(rwIndex).Cells(dgvCell.OwningColumn.Name + "~0")
        '            '    If dgvRow.Cells(dgvCell.OwningColumn.Name).Visible Then
        '            '        dgv.CurrentCell = dgvRow.Cells(dgvCell.OwningColumn.Name)
        '            '    End If
        '            '    .Value = dgvCell.Value
        '            '    .Style.BackColor = dgvCell.InheritedStyle.BackColor
        '            '    .Style.Font = dgvCell.InheritedStyle.Font
        '            'End With
        '        Next
        '        rwIndex += 1
        '    Next
        'End If





        'rwCount = 0
        'spRwIndex = 0
        'lbCount = 0
        'rwIndex = 0
        'For Each dgvRow As DataGridViewRow In dgv.Rows
        '    If rwCount = pageRowCount Then
        '        rwCount = 0
        '        lbCount += 1
        '        If lbCount <= noOfSplit - 1 Then
        '            rwIndex = (spRwIndex * pageRowCount)
        '        Else
        '            spRwIndex += 1
        '            rwIndex = (spRwIndex * pageRowCount)
        '            lbCount = 0
        '        End If
        '    End If
        '    For Each dgvCell As DataGridViewCell In dgvRow.Cells
        '        If Not ((dgvCell.OwningColumn.Name.ToUpper = "COLHEAD" Or dgvCell.OwningColumn.Name.ToUpper = "PAGEBREAK") And noOfSplit = 1) Then
        'If Not AvailableColumns.Contains(dgvCell.OwningColumn.Name) Then Continue For
        '        End If
        '        With gridView.Rows(rwIndex).Cells(dgvCell.OwningColumn.Name + "~" + lbCount.ToString)
        '            If dgvRow.Cells(dgvCell.OwningColumn.Name).Visible Then
        '                dgv.CurrentCell = dgvRow.Cells(dgvCell.OwningColumn.Name)
        '            End If
        '            .Style.BackColor = dgvCell.InheritedStyle.BackColor
        '            .Style.Font = dgvCell.InheritedStyle.Font
        '        End With
        '    Next
        '    rwIndex += 1
        '    rwCount += 1
        'Next
    End Sub
End Class