Imports System.Data.OleDb
Public Class frmAlertGrpMaster
    Dim _cmd As OleDbCommand
    Dim _da As OleDbDataAdapter
    Dim strSql As String
    Dim updFlag As Boolean

    Private Sub CallGrid()
        strSql = " SELECT "
        strSql += " GROUPID,GROUPNAME,GROUPMOBILES,GROUPMAILS,CASE WHEN ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END AS ACTIVE"
        strSql += " FROM " & cnAdminDb & "..ALERTGROUP"
        Dim _dt As New DataTable
        _da = New OleDbDataAdapter(strSql, cn)
        _da.Fill(_dt)
        gridView.DataSource = _dt
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If objGPack.Validator_Check(Me) Then Exit Sub
        If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ALERTGROUP WHERE GROUPNAME = '" & txtRoleName_MAN.Text & "' AND GROUPID <> " & Val(txtRoleId.Text) & "").Length > 0 Then
            MsgBox("Role Name Already Exist", MsgBoxStyle.Information)
            txtRoleName_MAN.Focus()
            Exit Sub
        End If

        Dim CostId As String = Nothing
        strSql = "Select CostId from " & cnAdminDb & "..CostCentre where CostName = '" & cmbCostCentre.Text & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            CostId = dt.Rows(0).Item("CostId")
        Else
            CostId = ""
        End If
        If updFlag Then
            strSql = " UPDATE " & cnAdminDb & "..ALERTGROUP SET"
            strSql += " COSTID= '" & CostId & "'" 'GROUPNAME
            strSql += ", GROUPNAME = '" & txtRoleName_MAN.Text & "'" 'GROUPNAME
            strSql += ", GROUPMOBILES = '" & txtMobileNos.Text & "'"
            strSql += ", GROUPMAILS = '" & Me.txtMails.Text & "'"
            strSql += " ,ACTIVE = '" & Mid(cmbActive.Text, 1, 1) & "'" 'ACTIVE
            strSql += " ,USERID = " & userId & ""
            strSql += " ,UPDATED = '" & GetEntryDate(GetServerDate) & "'"
            strSql += " ,UPTIME = '" & GetServerTime() & "'"
            strSql += " WHERE GROUPID = " & Val(txtRoleId.Text) & ""
            ExecQuery(SyncMode.Master, strSql, cn)
            MsgBox("Updated..")
        Else
            strSql = " INSERT INTO " & cnAdminDb & "..ALERTGROUP"
            strSql += " (COSTID,GROUPID,GROUPNAME,GROUPMOBILES,GROUPMAILS,ACTIVE,USERID,UPDATED,UPTIME)VALUES"

            strSql += " ('" & CostId & "'"
            strSql += "," & Val(txtRoleId.Text) & "" 'GROUPID
            strSql += " ,'" & txtRoleName_MAN.Text & "'" 'GROUPNAME
            strSql += " ,'" & txtMobileNos.Text & "'" 'GROUPNAME
            strSql += " ,'" & txtMails.Text & "'" 'GROUPNAME
            strSql += " ,'" & Mid(cmbActive.Text, 1, 1) & "'" 'ACTIVE
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
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        tabMain.SelectedTab = tabView
        If gridView.RowCount > 0 Then
            gridView.Focus()
        Else
            btnBack.Focus()
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        txtRoleId.Text = Val(objGPack.GetSqlValue("SELECT ISNULL(MAX(GROUPID),0)+ 1 FROM " & cnAdminDb & "..ALERTGROUP"))
        tabMain.SelectedTab = tabGeneral
        updFlag = False
        strSql = " Select 'All' as CostName Union All Select Costname from " & cnAdminDb & "..CostCentre order by CostName"
        objGPack.FillCombo(strSql, cmbCostCentre)
        cmbActive.Text = "YES"
        CallGrid()
        txtRoleId.Focus()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub frmALERTGROUP_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                btnBack_Click(Me, New EventArgs)
            End If
        End If
    End Sub

    Private Sub frmALERTGROUP_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmALERTGRpmaster_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        If GetAdmindbSoftValue("COSTCENTRE").ToUpper = "Y" Then
            strSql = " Select CostName from " & cnAdminDb & "..CostCentre order by CostName"
            objGPack.FillCombo(strSql, cmbCostCentre)
            cmbCostCentre.Enabled = True
        Else
            cmbCostCentre.Enabled = False
        End If
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub txtRoleId_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRoleId.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtUserName_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRoleName_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtRoleName_MAN.Text = "" Then Exit Sub
            If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ALERTGROUP WHERE GROUPNAME = '" & txtRoleName_MAN.Text & "' AND GROUPID <> " & Val(txtRoleId.Text) & "").Length > 0 Then
                MsgBox("Group Name Already Exist", MsgBoxStyle.Information)
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
            gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells("GROUPID")
            With gridView.CurrentRow
                cmbActive.Text = .Cells("ACTIVE").Value.ToString
                txtRoleId.Text = .Cells("GROUPID").Value.ToString
                txtRoleName_MAN.Text = .Cells("GROUPNAME").Value.ToString
                Me.txtMails.Text = .Cells("GROUPMAILS").Value.ToString
                Me.txtMobileNos.Text = .Cells("GROUPMOBILES").Value.ToString
                updFlag = True
                btnBack_Click(Me, New EventArgs)
            End With
        ElseIf UCase(e.KeyChar) = "D" Then
            btnDelete_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        If Not gridView.RowCount > 0 Then Exit Sub
        Dim list As New List(Of String)
        list.Add("GROUPID")
        Dim GROUPID As String = gridView.CurrentRow.Cells("GROUPID").Value.ToString
        If DeleteItem(SyncMode.Master, list, "DELETE FROM " & cnAdminDb & "..ALERTGROUP WHERE GROUPID = '" & GROUPID & "'", GROUPID, "ALERTGROUP") Then
            CallGrid()
            gridView.Focus()
        End If
    End Sub
End Class