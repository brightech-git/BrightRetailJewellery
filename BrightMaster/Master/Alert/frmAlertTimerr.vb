Imports System.Data.OleDb
Public Class frmAlertTimerr
    Dim _cmd As OleDbCommand
    Dim _da As OleDbDataAdapter
    Dim strSql As String
    Dim updFlag As Boolean

    Private Sub CallGrid()
        strSql = " SELECT ALERTID,"
        strSql += " A.GROUPID,G.GROUPNAME,ALERT_DESC,ALERT_PROC,ALERT_TIME,CASE WHEN A.ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END AS ACTIVE"
        strSql += " FROM " & cnAdminDb & "..ALERTTIMER A INNER JOIN " & cnAdminDb & "..ALERTGROUP AS G ON A.GROUPID=G.GROUPID"
        Dim _dt As New DataTable
        _da = New OleDbDataAdapter(strSql, cn)
        _da.Fill(_dt)
        gridView.DataSource = _dt
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If objGPack.Validator_Check(Me) Then Exit Sub
        If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ALERTTIMER WHERE ALERT_PROC= '" & txtProcName.Text & "' AND GROUPID <> " & Val(txtRoleId.Text) & "").Length > 0 Then
            MsgBox("Role Name Already Exist", MsgBoxStyle.Information)
            txtTime.Focus()
            Exit Sub
        End If

        Dim CostId As String = Nothing
        strSql = "Select groupid from " & cnAdminDb & "..ALERTGROUP where GroupName = '" & cmbCostCentre.Text & "'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            CostId = dt.Rows(0).Item("gROUPID")
        Else
            CostId = ""
        End If
        If updFlag Then
            strSql = " UPDATE " & cnAdminDb & "..ALERTTIMER SET"
            strSql += " GROUPID= " & CostId
            strSql += ", ALERT_TIME= '" & txtTime.Text & "'" 'GROUPNAME
            strSql += ", ALERT_DESC = '" & txtAlertName.Text & "'"
            strSql += ", ALERT_PROC = '" & Me.txtProcName.Text & "'"
            strSql += " ,ACTIVE = " & IIf(Mid(cmbActive.Text, 1, 1) = "Y", 1, 0)
            strSql += " ,USERID = " & userId & ""
            strSql += " ,UPDATED = '" & GetEntryDate(GetServerDate) & "'"
            strSql += " ,UPTIME = '" & GetServerTime() & "'"
            strSql += " WHERE GROUPID = " & Val(txtRoleId.Text) & ""
            ExecQuery(SyncMode.Master, strSql, cn)
            MsgBox("Updated..")
        Else
            strSql = " INSERT INTO " & cnAdminDb & "..ALERTIMER"
            strSql += " (ALERTID,GROUPID,ALERT_TIMER,ALERT_DESC,ALERT_PROC,GROUPMAILS,ACTIVE,USERID,UPDATED,UPTIME)VALUES"
            strSql += "(" & Val(txtRoleId.Text)
            strSql += "," & CostId & ""
            strSql += " ,'" & txtTime.Text & "'" 'GROUPNAME
            strSql += " ,'" & txtAlertName.Text & "'" 'GROUPNAME
            strSql += " ,'" & txtProcName.Text & "'" 'GROUPNAME
            strSql += " ," & IIf(Mid(cmbActive.Text, 1, 1) = "Y", 1, 0)
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
        txtRoleId.Text = Val(objGPack.GetSqlValue("SELECT ISNULL(MAX(Alertid),0)+ 1 FROM " & cnAdminDb & "..ALERTtimer"))
        tabMain.SelectedTab = tabGeneral
        updFlag = False
        strSql = " Select Groupname from " & cnAdminDb & "..Alertgroup order by GroupName"
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

        strSql = " Select Groupname from " & cnAdminDb & "..Alertgroup order by Groupname"
        objGPack.FillCombo(strSql, cmbCostCentre)
        cmbCostCentre.Enabled = True

        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub txtRoleId_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRoleId.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtUserName_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtTime.Text = "" Then Exit Sub
            If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..ALERTTIMER WHERE ALERT_PROC = '" & txtProcName.Text & "' AND alertid <> " & Val(txtRoleId.Text) & "").Length > 0 Then
                MsgBox("Procedure Name Already Exist", MsgBoxStyle.Information)
                txtTime.Focus()
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
                txtRoleId.Text = .Cells("ALERTID").Value.ToString
                txtTime.Text = .Cells("alert_time").Value.ToString
                Me.txtProcName.Text = .Cells("Alert_Proc").Value.ToString
                Me.txtAlertName.Text = .Cells("Alert_DESC").Value.ToString
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
        Dim GROUPID As String = gridView.CurrentRow.Cells("ALERTID").Value.ToString
        If DeleteItem(SyncMode.Master, list, "DELETE FROM " & cnAdminDb & "..ALERTTIMER WHERE ALERTID = '" & GROUPID & "'", GROUPID, "ALERTGROUP") Then
            CallGrid()
            gridView.Focus()
        End If
    End Sub

    Private Sub txtRoleName_MAN_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
End Class