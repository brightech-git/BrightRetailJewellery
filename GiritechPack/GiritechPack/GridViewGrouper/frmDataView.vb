Public Class frmDataView
    Dim dtGridView As New DataTable
    Dim cMenu As New ContextMenuStrip
    Dim sumColumn As New List(Of String)
    Dim defaultGroupColumns As New List(Of String)
    Dim notVisibleCols As New List(Of String)


    Public Sub New(ByVal dtDataSource As DataTable, _
        Optional ByVal notVisibleCols As List(Of String) = Nothing, _
        Optional ByVal defaultGroupColumns As List(Of String) = Nothing, _
        Optional ByVal defaultSumColumns As List(Of String) = Nothing)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        notVisibleCols = Me.notVisibleCols
        defaultGroupColumns = Me.defaultGroupColumns
        defaultSumColumns = Me.sumColumn
        dtGridView = dtDataSource
        dgv.RowHeadersVisible = False
        dgv.RowTemplate.Resizable = DataGridViewTriState.False
    End Sub

    Private Sub dgvTStripMenu(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim cnt As Integer = 0
        For Each mm As ToolStripMenuItem In cMenu.Items
            If mm.CheckState = CheckState.Checked Then
                cnt += 1
            End If
        Next
        If cnt > 2 Then
            For Each mm As ToolStripMenuItem In cMenu.Items
                mm.CheckState = CheckState.Unchecked
            Next
            CType(sender, ToolStripMenuItem).CheckState = CheckState.Checked
        End If
        Dim lstColumn As New List(Of String)
        For Each mm As ToolStripMenuItem In cMenu.Items
            If mm.CheckState = CheckState.Checked Then
                lstColumn.Add(mm.Text)
            End If
        Next
        dgv.DataSource = Nothing
        Dim dt As New DataTable
        dt = dtGridView.Copy
        If cnt = 0 Then
            dgv.DataSource = dt
            GridViewGrouper.StyleGridColumns(dgv, notVisibleCols)
        Else
            dgv.DataSource = dt
            GridViewGrouper.GridView_Grouper(dgv, notVisibleCols, lstColumn, sumColumn)
        End If
    End Sub

    Private Sub frmDataView_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dt As New DataTable
        dt = dtGridView.Copy
        dgv.DataSource = dt
        GridViewGrouper.StyleGridColumns(dgv, notVisibleCols)

        cMenu.Items.Clear()
        For Each col As DataGridViewColumn In dgv.Columns
            col.SortMode = DataGridViewColumnSortMode.NotSortable
            Dim menu As New ToolStripMenuItem
            menu.Name = col.Name
            menu.Text = col.HeaderText
            menu.CheckOnClick = True
            AddHandler menu.Click, AddressOf dgvTStripMenu
            cMenu.Items.Add(menu)
        Next
        If defaultGroupColumns.Count > 0 Then
            For Each str As String In defaultGroupColumns
                cMenu.Items.Contains(New ToolStripMenuItem(str))
            Next
            dgvTStripMenu(Me, New EventArgs)
        End If
        dgv.ContextMenuStrip = cMenu
    End Sub

    Private Sub dgv_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgv.ColumnHeaderMouseClick
        Select Case dgv.Columns(e.ColumnIndex).ValueType.FullName
            Case GetType(Int16).FullName, GetType(Int32).FullName, GetType(Int64).FullName, GetType(Integer).FullName, GetType(Decimal).FullName, GetType(Double).FullName
                If sumColumn.Contains(dgv.Columns(e.ColumnIndex).Name) Then
                    sumColumn.Remove(dgv.Columns(e.ColumnIndex).Name)
                Else
                    sumColumn.Add(dgv.Columns(e.ColumnIndex).Name)
                End If
                dgvTStripMenu(Me, New EventArgs)
        End Select
    End Sub

    Private Sub tStripExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripExit.Click
        Me.Close()
    End Sub

    'Private Sub tStripPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripPrint.Click
    '    tStripPrint.Enabled = False
    '    PrintDataGridView.Print_DataGridView(dgv)
    '    tStripPrint.Enabled = True
    'End Sub

    'Private Sub tStripExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tStripExcel.Click
    '    tStripExcel.Enabled = False
    '    Excel.Post(dgv, "")
    '    tStripExcel.Enabled = True
    'End Sub
End Class