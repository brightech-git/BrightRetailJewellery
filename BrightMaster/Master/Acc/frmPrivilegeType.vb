Imports System.Data.OleDb
Public Class frmPrivilegeType
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim flagUpdateId As String = Nothing
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Private Sub frmPrivilegeType_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub frmPrivilegeType_KeyPress(sender As Object, e As KeyPressEventArgs) Handles MyBase.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub LoadGrid()
        strSql = "SELECT TYPEID,TYPENAME FROM " & cnAdminDb & "..PRIVILEGETYPE ORDER BY TYPEID"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtGrid As New DataTable
        da.Fill(dtGrid)
        gridView.DataSource = dtGrid
        'gridView.Columns("TYPEID").Visible = False
        gridView.Columns("TYPEID").HeaderText = "PRIVILEGETYPE ID"
        gridView.Columns("TYPENAME").HeaderText = "PRIVILEGETYPE NAME"
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        gridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        If objGPack.Validator_Check(Me) Then Exit Sub
        If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..PRIVILEGETYPE WHERE TYPENAME = '" & txtPrivilegeTypeName_MAN.Text & "' AND TYPEID <> '" & flagUpdateId & "'").Length > 0 Then
            MsgBox("Already exist", MsgBoxStyle.Information)
            txtPrivilegeTypeName_MAN.Focus()
            Exit Sub
        End If
        If flagUpdateId <> Nothing Then 'UPDATE
            strSql = "UPDATE " & cnAdminDb & "..PRIVILEGETYPE"
            strSql += " SET TYPENAME = '" & txtPrivilegeTypeName_MAN.Text & "'"
            strSql += " WHERE TYPEID = '" & flagUpdateId & "'"
            ExecQuery(SyncMode.Master, strSql, cn)
            MsgBox("Successfully Updated")
            btnNew_Click(Me, New EventArgs)
            Exit Sub
        End If
        Dim cnt As Integer = objGPack.GetSqlValue("SELECT COUNT(*)+1 FROM " & cnAdminDb & "..PRIVILEGETYPE")
        Dim discId As String = Nothing
genDis:

        If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..PRIVILEGETYPE WHERE TYPEID = '" & discId & "'").Length > 0 Then
            cnt += 1
            GoTo genDis
        End If
        strSql = " INSERT INTO " & cnAdminDb & "..PRIVILEGETYPE"
        strSql += " (TYPENAME)VALUES"
        strSql += " ('" & txtPrivilegeTypeName_MAN.Text & "')"
        ExecQuery(SyncMode.Master, strSql, cn)
        MsgBox("Successfully Inserted")
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub btnOpen_Click(sender As Object, e As EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        LoadGrid()
        If gridView.Rows.Count > 0 Then
            gridView.Focus()
        End If
    End Sub
    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        txtPrivilegeTypeName_MAN.Clear()
        flagUpdateId = Nothing
        LoadGrid()
        txtPrivilegeTypeName_MAN.Focus()
    End Sub
    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub gridView_KeyDown(sender As Object, e As KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then e.Handled = True
    End Sub
    Private Sub gridView_KeyPress(sender As Object, e As KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If gridView.RowCount > 0 Then
                gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells(0)
                txtPrivilegeTypeName_MAN.Text = gridView.CurrentRow.Cells("TYPENAME").Value
                flagUpdateId = gridView.CurrentRow.Cells("TYPEID").Value
                txtPrivilegeTypeName_MAN.Focus()
            End If
        End If
    End Sub
    Private Sub txtPrivilegeTypeName_MAN_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPrivilegeTypeName_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtPrivilegeTypeName_MAN.Text = "" Then Exit Sub
            If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..PRIVILEGETYPE WHERE TYPENAME = '" & txtPrivilegeTypeName_MAN.Text & "' AND TYPEID <> '" & flagUpdateId & "'").Length > 0 Then
                MsgBox("Already exist", MsgBoxStyle.Information)
                txtPrivilegeTypeName_MAN.Focus()
            End If
        End If
    End Sub
    Private Sub txtPrivilegeTypeName_MAN_LostFocus(sender As Object, e As EventArgs) Handles txtPrivilegeTypeName_MAN.LostFocus
        If txtPrivilegeTypeName_MAN.Text = "" Then Exit Sub
        If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..PRIVILEGETYPE WHERE TYPENAME = '" & txtPrivilegeTypeName_MAN.Text & "' AND TYPEID <> '" & flagUpdateId & "'").Length > 0 Then
            MsgBox("Already exist", MsgBoxStyle.Information)
            txtPrivilegeTypeName_MAN.Focus()
        End If
    End Sub
End Class