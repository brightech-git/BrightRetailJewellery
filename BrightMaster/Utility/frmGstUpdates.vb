Imports System.Data.OleDb
Public Class frmGstUpdates
    Dim StrSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dt As New DataTable
    Dim GSTNOFORMAT As String = GetAdmindbSoftValue("GSTNOFORMAT", "")
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub frmGstUpdates_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            If gridItemView.Focused Then Exit Sub
            If gridSubItemView.Focused Then Exit Sub
            If gridAcheadView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmGstUpdates_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        funcItemLoad()
        funcSubItemLoad()
        funcAcheadLoad()
        StrSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST ORDER BY ITEMNAME"
        objGPack.FillCombo(StrSql, cmbOpenItemName, , False)
        If cmbOpenItemName.Items.Count > 0 Then
            cmbOpenItemName.Text = "ALL"
        End If
        StrSql = " SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP ORDER BY ACGRPNAME"
        objGPack.FillCombo(StrSql, cmbOpenGroupName, , False)
        If cmbOpenGroupName.Items.Count > 0 Then
            cmbOpenGroupName.Text = "ALL"
        End If
        tabMain.SelectedTab = tabItem
        gridItemView.Select()
        SendKeys.Send("{TAB}")
    End Sub
    Private Sub funcItemLoad()
        gridItemView.DataSource = Nothing
        StrSql = "SELECT ITEMID,ITEMNAME AS ITEM,HSN "
        StrSql += " FROM " & cnAdminDb & "..ITEMMAST "
        StrSql += " ORDER BY ITEMID"
        da = New OleDbDataAdapter(StrSql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            With gridItemView
                .DataSource = dt
                For i As Integer = 0 To .ColumnCount - 1
                    .Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
                Next
                .SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
                .Columns("ITEMID").Visible = False
                .Columns("ITEM").ReadOnly = True
                .Columns("ITEM").Width = 250
                .Columns("HSN").Width = 150
                .Columns("HSN").ReadOnly = False
            End With
        End If
    End Sub
    Private Sub funcSubItemLoad(Optional ByVal ItemName As String = "")
        gridSubItemView.DataSource = Nothing
        StrSql = "SELECT ITEMID,SUBITEMID,SUBITEMNAME AS SUBITEM,HSN "
        StrSql += " FROM " & cnAdminDb & "..SUBITEMMAST WHERE 1=1"
        If ItemName <> "ALL" And ItemName <> "" Then
            StrSql += " AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & ItemName & "' )"
        End If
        StrSql += " ORDER BY ITEMID,SUBITEMID"
        da = New OleDbDataAdapter(StrSql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            With gridSubItemView
                .DataSource = dt
                For i As Integer = 0 To .ColumnCount - 1
                    .Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
                Next
                .SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
                .Columns("ITEMID").Visible = False
                .Columns("SUBITEMID").Visible = False
                .Columns("SUBITEM").ReadOnly = True
                .Columns("SUBITEM").Width = 260
                .Columns("HSN").Width = 150
                .Columns("HSN").ReadOnly = False
            End With
        End If
    End Sub
    Private Sub funcAcheadLoad(Optional ByVal AcGrp As String = "", Optional ByVal Name As String = "")
        gridAcheadView.DataSource = Nothing
        StrSql = "SELECT ACCODE,ACNAME,GSTNO "
        StrSql += " FROM " & cnAdminDb & "..ACHEAD WHERE 1=1"
        If AcGrp <> "ALL" And AcGrp <> "" Then
            StrSql += " AND ACGRPCODE=(SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPNAME='" & AcGrp & "' )"
        End If
        If Name <> "" Then
            StrSql += " AND ACNAME LIKE '%" & Name & "%'"
        End If
        StrSql += " ORDER BY ACNAME"
        da = New OleDbDataAdapter(StrSql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            With gridAcheadView
                .DataSource = dt
                For i As Integer = 0 To .ColumnCount - 1
                    .Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
                Next
                .SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
                .Columns("ACCODE").Visible = False
                .Columns("ACNAME").ReadOnly = True
                .Columns("GSTNO").ReadOnly = False
                .Columns("ACNAME").Width = 260
                .Columns("GSTNO").Width = 150
            End With
        End If
    End Sub

    Private Sub gridItemView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridItemView.CellEndEdit
        If Not gridItemView.RowCount > 0 Then Exit Sub
        If gridItemView.CurrentRow Is Nothing Then Exit Sub
        Dim UpFlag As Boolean = False
        StrSql = " UPDATE " & cnAdminDb & "..ITEMMAST SET"
        If Me.gridItemView.CurrentCell.ColumnIndex = gridItemView.Columns("HSN").Index Then
            StrSql += " HSN='" & gridItemView.CurrentRow.Cells("HSN").Value.ToString & "'"
            UpFlag = True
        End If
        StrSql += " WHERE ITEMID = '" & gridItemView.CurrentRow.Cells("ITEMID").Value.ToString & "'"
        If UpFlag = True Then ExecQuery(SyncMode.Master, StrSql, cn, tran)
    End Sub

    Private Sub gridSubItemView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridSubItemView.CellEndEdit
        If Not gridSubItemView.RowCount > 0 Then Exit Sub
        If gridSubItemView.CurrentRow Is Nothing Then Exit Sub
        Dim UpFlag As Boolean = False
        StrSql = " UPDATE " & cnAdminDb & "..SUBITEMMAST SET"
        If Me.gridSubItemView.CurrentCell.ColumnIndex = gridSubItemView.Columns("HSN").Index Then
            StrSql += " HSN='" & gridSubItemView.CurrentRow.Cells("HSN").Value.ToString & "'"
            UpFlag = True
        End If
        StrSql += " WHERE ITEMID = '" & gridSubItemView.CurrentRow.Cells("ITEMID").Value.ToString & "'"
        StrSql += " AND SUBITEMID = '" & gridSubItemView.CurrentRow.Cells("SUBITEMID").Value.ToString & "'"
        If UpFlag = True Then ExecQuery(SyncMode.Master, StrSql, cn, tran)
    End Sub

    Private Sub gridAcheadView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridAcheadView.CellEndEdit
        If Not gridAcheadView.RowCount > 0 Then Exit Sub
        If gridAcheadView.CurrentRow Is Nothing Then Exit Sub
        Dim UpFlag As Boolean = False
        StrSql = " UPDATE " & cnAdminDb & "..ACHEAD SET"
        If Me.gridAcheadView.CurrentCell.ColumnIndex = gridAcheadView.Columns("GSTNO").Index Then
            StrSql += " GSTNO='" & gridAcheadView.CurrentRow.Cells("GSTNO").Value.ToString & "'"
            If Not formatchkGST(GSTNOFORMAT, gridAcheadView.CurrentRow.Cells("GSTNO").Value.ToString) Then
                MsgBox("GST No format(" & GSTNOFORMAT & ")should not match", MsgBoxStyle.Information)
                gridAcheadView.CurrentRow.Cells("GSTNO").Value = DBNull.Value
                Exit Sub
            End If
            UpFlag = True
        End If
        StrSql += " WHERE ACCODE = '" & gridAcheadView.CurrentRow.Cells("ACCODE").Value.ToString & "'"
        If UpFlag = True Then ExecQuery(SyncMode.Master, StrSql, cn, tran)
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click, RefreshToolStripMenuItem.Click
        funcItemLoad()
        funcSubItemLoad(cmbOpenItemName.Text)
        funcAcheadLoad(cmbOpenGroupName.Text, txtSearch.Text)
        If tabMain.SelectedTab.Name = tabItem.Name Then
            gridItemView.Select()
            SendKeys.Send("{TAB}")
        ElseIf tabMain.SelectedTab.Name = tabSubItem.Name Then
            gridSubItemView.Select()
            SendKeys.Send("{TAB}")
        Else
            gridAcheadView.Select()
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        funcSubItemLoad(cmbOpenItemName.Text)
        gridSubItemView.Select()
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub btnOpenSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenSearch.Click
        funcAcheadLoad(cmbOpenGroupName.Text, txtSearch.Text)
        gridAcheadView.Select()
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub gridItemView_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridItemView.CellEnter
        Select Case gridItemView.Columns(e.ColumnIndex).Name
            Case "HSN"
                lblHelp.Text = "* Type HSN No and give and Enter."
            Case Else
                lblHelp.Text = ""
        End Select
    End Sub

    Private Sub gridSubItemView_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridSubItemView.CellEnter
        Select Case gridSubItemView.Columns(e.ColumnIndex).Name
            Case "HSN"
                lblHelp.Text = "* Type HSN No and give and Enter."
            Case Else
                lblHelp.Text = ""
        End Select
    End Sub

    Private Sub gridAcheadView_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridAcheadView.CellEnter
        Select Case gridAcheadView.Columns(e.ColumnIndex).Name
            Case "GSTNO"
                lblHelp.Text = "* Type Gst No and give and Enter."
            Case Else
                lblHelp.Text = ""
        End Select
    End Sub
End Class