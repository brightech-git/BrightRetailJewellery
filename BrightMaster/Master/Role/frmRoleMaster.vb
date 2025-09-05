Imports System.Data.OleDb
Public Class frmRoleMaster
    Dim _cmd As OleDbCommand
    Dim _da As OleDbDataAdapter
    Dim strSql As String
    Dim updFlag As Boolean

    Private Sub CallGrid()
        strSql = " SELECT "
        strSql += " ROLEID,ROLENAME,CASE WHEN ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END AS ACTIVE"
        strSql += " ,CASE WHEN PWDACCESS = 'Y' THEN 'YES' ELSE 'NO' END AS PWDACCESS "
        strSql += " ,CASE WHEN ADMINACCESS = 'Y' THEN 'YES' ELSE 'NO' END AS ADMINACCESS "
        strSql += " FROM " & cnAdminDb & "..ROLEMASTER"
        Dim _dt As New DataTable
        _da = New OleDbDataAdapter(strSql, cn)
        _da.Fill(_dt)
        gridView.DataSource = _dt
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        If PnlAdminAcc.Visible = False Then gridView.Columns("ADMINACCESS").Visible = False
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If objGPack.Validator_Check(Me) Then Exit Sub
        If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ROLEMASTER WHERE ROLENAME = '" & txtRoleName_MAN.Text & "' AND ROLEID <> " & Val(txtRoleId.Text) & "").Length > 0 Then
            MsgBox("Role Name Already Exist", MsgBoxStyle.Information)
            txtRoleName_MAN.Focus()
            Exit Sub
        End If
        If updFlag Then
            strSql = " UPDATE " & cnAdminDb & "..ROLEMASTER SET"
            strSql += " ROLENAME = '" & txtRoleName_MAN.Text & "'" 'ROLENAME
            strSql += " ,ACTIVE = '" & Mid(cmbActive.Text, 1, 1) & "'" 'ACTIVE
            strSql += " ,PWDACCESS= '" & Mid(cmbPwdAcc.Text, 1, 1) & "'" 'PWDACCESS
            If PnlAdminAcc.Visible = True Then
                strSql += " ,ADMINACCESS= '" & Mid(CmbAdminAcc.Text, 1, 1) & "'" 'ADMINACCESS
            End If
            strSql += " ,USERID = " & userId & ""
            strSql += " ,UPDATED = '" & GetEntryDate(GetServerDate) & "'"
            strSql += " ,UPTIME = '" & GetServerTime() & "'"
            strSql += " WHERE ROLEID = " & Val(txtRoleId.Text) & ""
            ExecQuery(SyncMode.Master, strSql, cn)
            MsgBox("Updated..")
        Else
            strSql = " INSERT INTO " & cnAdminDb & "..ROLEMASTER"
            strSql += " (ROLEID,ROLENAME,ACTIVE,PWDACCESS"
            If PnlAdminAcc.Visible = True Then
                strSql += " ,ADMINACCESS"
            End If
            strSql += " ,USERID,UPDATED,UPTIME)VALUES"
            strSql += " ("
            strSql += " " & Val(txtRoleId.Text) & "" 'ROLEID
            strSql += " ,'" & txtRoleName_MAN.Text & "'" 'ROLENAME
            strSql += " ,'" & Mid(cmbActive.Text, 1, 1) & "'" 'ACTIVE
            strSql += " ,'" & Mid(cmbPwdAcc.Text, 1, 1) & "'" 'PWDACCESS
            If PnlAdminAcc.Visible = True Then
                strSql += " ,'" & Mid(CmbAdminAcc.Text, 1, 1) & "'" 'ADMINACCESS
            End If
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
        txtRoleId.Text = Val(objGPack.GetSqlValue("SELECT ISNULL(MAX(ROLEID),0)+ 1 FROM " & cnAdminDb & "..ROLEMASTER"))
        tabMain.SelectedTab = tabGeneral
        updFlag = False
        If GetAdmindbSoftValue("ADMINACCESS", "N") <> "Y" Then PnlAdminAcc.Visible = False : CmbAdminAcc.Text = "NO"
        cmbActive.Text = "YES"
        CallGrid()
        txtRoleId.Focus()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub frmRoleMaster_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                btnBack_Click(Me, New EventArgs)
            End If
        End If
    End Sub

    Private Sub frmRoleMaster_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub RoleMaster_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        cmbPwdAcc.Items.Add("YES")
        cmbPwdAcc.Items.Add("NO")
        cmbPwdAcc.Text = "TES"
        CmbAdminAcc.Items.Add("YES")
        CmbAdminAcc.Items.Add("NO")
        CmbAdminAcc.Text = "NO"
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub txtRoleId_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRoleId.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtUserName_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRoleName_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtRoleName_MAN.Text = "" Then Exit Sub
            If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ROLEMASTER WHERE ROLENAME = '" & txtRoleName_MAN.Text & "' AND ROLEID <> " & Val(txtRoleId.Text) & "").Length > 0 Then
                MsgBox("Role Name Already Exist", MsgBoxStyle.Information)
                txtRoleName_MAN.Focus()
            End If
        End If
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        txtRoleId.Focus()
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
            gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells("ROLEID")
            With gridView.CurrentRow
                cmbPwdAcc.Text = .Cells("PWDACCESS").Value.ToString
                CmbAdminAcc.Text = .Cells("ADMINACCESS").Value.ToString
                cmbActive.Text = .Cells("ACTIVE").Value.ToString
                txtRoleId.Text = .Cells("ROLEID").Value.ToString
                txtRoleName_MAN.Text = .Cells("ROLENAME").Value.ToString
                updFlag = True
                btnBack_Click(Me, New EventArgs)
            End With
        ElseIf UCase(e.KeyChar) = "D" Then
            btnDelete_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        If Not gridView.RowCount > 0 Then Exit Sub
        Dim list As New List(Of String)
        list.Add("ROLEID")
        Dim ROLEID As String = gridView.CurrentRow.Cells("ROLEID").Value.ToString
        If DeleteItem(SyncMode.Master, list, "DELETE FROM " & cnAdminDb & "..ROLEMASTER WHERE ROLEID = '" & ROLEID & "'", ROLEID, "ROLEMASTER") Then
            CallGrid()
            gridView.Focus()
        End If
    End Sub
End Class