Imports System.Data.OleDb
Public Class frmUserRole
    Dim _Cmd As OleDbCommand
    Dim _Da As OleDbDataAdapter
    Dim strSql As String

    Private Sub CallGrid()
        strSql = " SELECT "
        strSql += " (SELECT top 1 ROLENAME FROM " & cnAdminDb & "..ROLEMASTER WHERE ROLEID = U.ROLEID)AS ROLENAME"
        strSql += " ,(SELECT top 1 USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = U.USERID)AS USERNAME"
        strSql += " FROM " & cnadmindb & "..USERROLE AS U"
        Dim _dt As New DataTable
        _da = New OleDbDataAdapter(strSql, cn)
        _da.Fill(_dt)
        gridView.DataSource = _dt
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If objGPack.Validator_Check(Me) Then Exit Sub
        Dim UpdFlag As Boolean = False
        Dim _RoleId As Integer = Val(objGPack.GetSqlValue("SELECT ROLEID FROM " & cnAdminDb & "..ROLEMASTER WHERE ROLENAME = '" & cmbRoleName_MAN.Text & "'"))
        Dim _UserId As Integer = Val(objGPack.GetSqlValue("SELECT USERID FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME = '" & cmbUserName_MAN.Text & "'"))
        If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..USERROLE WHERE USERID = " & _UserId & "").Length > 0 Then
            strSql = " UPDATE " & cnAdminDb & "..USERROLE SET"
            strSql += " ROLEID = " & _RoleId & ""
            strSql += " WHERE USERID = " & _UserId & ""
            ExecQuery(SyncMode.Master, strSql, cn)
            MsgBox("Updated..")
        Else
            strSql = " INSERT INTO " & cnAdminDb & "..USERROLE"
            strSql += " (ROLEID,USERID,UPUSERID,UPDATED,UPTIME)VALUES"
            strSql += " ("
            strSql += " " & _RoleId & "," & _UserId & ""
            strSql += " ," & userId & ""
            strSql += " ,'" & GetEntryDate(GetServerDate) & "'"
            strSql += " ,'" & GetServerTime() & "'"
            strSql += " )"
            ExecQuery(SyncMode.Master, strSql, cn)
            MsgBox("Saved..")
        End If
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        tabMain.SelectedTab = tabView
        If gridView.RowCount > 0 Then
            gridView.Focus()
        Else
            btnBack.Focus()
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        tabMain.SelectedTab = tabGeneral
        CallGrid()
        cmbUserName_MAN.Select()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub frmUserRole_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                btnBack_Click(Me, New EventArgs)
            End If
        End If
    End Sub

    Private Sub frmUserRole_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmUserRole_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))

        strSql = " SELECT ROLENAME FROM " & cnAdminDb & "..ROLEMASTER WHERE ACTIVE = 'Y'"
        strSql += " ORDER BY ROLENAME"
        objGPack.FillCombo(strSql, cmbRoleName_MAN, , False)

        strSql = " SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER ORDER BY USERNAME"
        objGPack.FillCombo(strSql, cmbUserName_MAN, , False)

        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        cmbUserName_MAN.Focus()
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If Not gridView.RowCount > 0 Then Exit Sub
            gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells("USERNAME")
            With gridView.CurrentRow
                cmbUserName_MAN.Text = .Cells("USERNAME").Value.ToString
                cmbRoleName_MAN.Text = .Cells("ROLENAME").Value.ToString
                btnBack_Click(Me, New EventArgs)
            End With
        End If
    End Sub

    Private Sub cmbUserName_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbUserName_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim _UserId As Integer = Val(objGPack.GetSqlValue("SELECT USERID FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME = '" & cmbUserName_MAN.Text & "'"))
            Dim _RoleId As Integer = Val(objGPack.GetSqlValue("SELECT ROLEID FROM " & cnAdminDb & "..USERROLE WHERE USERID = " & _UserId & "", , "-1"))
            If _RoleId > 0 Then
                cmbRoleName_MAN.Text = objGPack.GetSqlValue("SELECT ROLENAME FROM " & cnAdminDb & "..ROLEMASTER WHERE ROLEID = " & _RoleId & "")
            Else
                cmbRoleName_MAN.Text = ""
            End If
        End If
    End Sub
End Class