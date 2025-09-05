Imports System.Data.OleDb
Public Class frmSyncMast
    Dim strSql As String
    Dim da As OleDbDataAdapter

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub frmSyncMast_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        strSql = " SELECT TABLENAME,SYNC FROM " & cnAdminDb & "..SYNCMAST"
        Dim dtGrid As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        gridView.DataSource = dtGrid
        gridView.Columns("TABLENAME").Width = 388
        gridView.Columns("TABLENAME").ReadOnly = True
        gridView.Columns("SYNC").Width = 50
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then
            gridView.Columns("SYNC").ReadOnly = True
        Else
            gridView.Columns("SYNC").ReadOnly = False
        End If
    End Sub

    Private Sub TextBox_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If UCase(e.KeyChar) = "Y" Or UCase(e.KeyChar = "N") Then
            e.KeyChar = UCase(e.KeyChar)
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub gridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEndEdit
        strSql = " UPDATE " & cnAdminDb & "..SYNCMAST "
        strSql += " SET SYNC = '" & gridView.CurrentRow.Cells("SYNC").Value.ToString & "'"
        strSql += " WHERE TABLENAME = '" & gridView.CurrentRow.Cells("TABLENAME").Value.ToString & "'"
        ExecQuery(SyncMode.Master, strSql, cn)
    End Sub

    Private Sub gridView_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles gridView.EditingControlShowing
        If Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("SYNC").Index And Not e.Control Is Nothing Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            '---add an event handler to the TextBox control---
            AddHandler tb.KeyPress, AddressOf TextBox_KeyPress
        End If
    End Sub
End Class