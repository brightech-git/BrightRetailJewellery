Imports System.Data.OleDb
Public Class frmSyncCostcentre
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim costId As String = Nothing 'for update
    Dim Sync_Local As Boolean = IIf(GetAdmindbSoftValue("SYNC-LOCAL", "N") = "N", False, True)
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        strSql = " SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST ORDER BY STATENAME"
        objGPack.FillCombo(strSql, cmbState)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        costId = Nothing
        If Sync_Local Then GrpLocalDb.Enabled = True
        CallGrid()
        txtDbId.Text = cnCompanyId
        cmbActive.SelectedIndex = 0
        txtCostId.Focus()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        If gridView.RowCount > 0 Then
            gridView.Focus()
        End If
    End Sub

    Private Sub Add()
        strSql = " INSERT INTO " & cnAdminDb & "..SYNCCOSTCENTRE(COSTID,STATEID,EMAILID,FTPID,PASSWORD,COMPID,MAIN,MANUAL,ACTIVE"
        If Sync_Local Then
            strSql += " ,WEBDBNAME,WEBDBTBLPREFIX,WEBDBUSERNAME"
        End If
        strSql += " )"
        strSql += " VALUES"
        strSql += " ("
        strSql += " '" & txtCostId.Text & "'"
        strSql += " ,'" & objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME = '" & cmbState.Text & "'") & "'"
        strSql += " ,'" & txtEmailId.Text & "'"
        strSql += " ,'" & txtFtpId.Text & "'"
        strSql += " ,'" & BrighttechPack.Methods.Encrypt(txtPassword.Text) & "'"
        strSql += " ,'" & txtDbId.Text & "'"
        strSql += " ,'" & IIf(chkMain.Checked, "Y", "N") & "'"
        strSql += " ,'" & IIf(chkManual.Checked, "Y", "N") & "'"
        strSql += " ,'" & Mid(cmbActive.Text, 1, 1) & "'"
        If Sync_Local Then
            strSql += " ,'" & txtWebDbname.Text & "'"
            strSql += " ,'" & txtWebTblPrefix.Text & "'"
            strSql += " ,'" & txtWebDbUser.Text & "'"
        End If
        strSql += " )"
        ExecQuery(SyncMode.Master, strSql, cn)
    End Sub

    Private Sub Updat()
        Dim pwd As String = BrighttechPack.Methods.Encrypt(txtPassword.Text)
        strSql = " UPDATE " & cnAdminDb & "..SYNCCOSTCENTRE"
        strSql += " SET"
        strSql += " COSTID = '" & txtCostId.Text & "'"
        strSql += " ,STATEID = '" & objGPack.GetSqlValue("SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME = '" & cmbState.Text & "'") & "'"
        strSql += " ,EMAILID = '" & txtEmailId.Text & "'"
        strSql += " ,FTPID = '" & txtFtpId.Text & "'"
        strSql += " ,PASSWORD = '" & BrighttechPack.Methods.Encrypt(txtPassword.Text) & "'"
        strSql += " ,COMPID = '" & txtDbId.Text & "'"
        strSql += " ,MAIN = '" & IIf(chkMain.Checked, "Y", "N") & "'"
        strSql += " ,MANUAL = '" & IIf(chkManual.Checked, "Y", "N") & "'"
        strSql += " ,ACTIVE = '" & Mid(cmbActive.Text, 1, 1) & "'"
        If Sync_Local Then
            strSql += " ,WEBDBNAME = '" & txtWebDbname.Text & "'"
            strSql += " ,WEBDBTBLPREFIX = '" & txtWebTblPrefix.Text & "'"
            strSql += " ,WEBDBUSERNAME = '" & txtWebDbUser.Text & "'"
        End If
        strSql += " WHERE COSTID = '" & costId & "'"
        ExecQuery(SyncMode.Master, strSql, cn)
    End Sub

    Private Function checkCostCentre() As Boolean
        strSql = " SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE "
        strSql += " WHERE COSTID = '" & txtCostId.Text & "' "
        strSql += " AND COSTID <> '" & costId & "'"
        If objGPack.GetSqlValue(strSql).Length > 0 Then
            MsgBox("Already Exist this costcentre sync info", MsgBoxStyle.Information)
            Return False
        End If
        Return True
    End Function

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If txtCostId.Text = "" Then
            MsgBox("CostId should not empty", MsgBoxStyle.Information)
            txtCostId.Focus()
            Exit Sub
        End If
        If checkCostCentre() = False Then
            Exit Sub
        End If
        If txtDbId.Text = "" Then
            MsgBox("CompId should not empty", MsgBoxStyle.Information)
            Exit Sub
        End If
        If txtEmailId.Text = "" And txtFtpId.Text = "" And Not chkManual.Checked Then
            MsgBox("Email Id should not empty", MsgBoxStyle.Information)
            txtEmailId.Focus()
            Exit Sub
        End If
        strSql = " SELECT COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE "
        strSql += " WHERE MAIN = 'Y'"
        strSql += " AND COSTID <> '" & txtCostId.Text & "'"
        strSql += " AND COSTID <> '" & costId & "'"
        If chkMain.Checked Then
            If objGPack.GetSqlValue(strSql).Length > 0 Then
                MsgBox("Only one main server should allow", MsgBoxStyle.Information)
                chkMain.Focus()
                Exit Sub
            End If
        End If
        If costId = Nothing Then
            Add()
            MsgBox("Inserted..")
        Else
            Updat()
            MsgBox("Updated..")
        End If
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub CallGrid()
        strSql = " SELECT COSTID,ISNULL(APPVERSION,'') AS VERSION"
        strSql += " ,(SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID = S.STATEID)AS STATE"
        strSql += " ,EMAILID,FTPID,PASSWORD,COMPID,MAIN,MANUAL"
        strSql += " ,CASE WHEN ISNULL(ACTIVE,'')<>'N' THEN 'YES' ELSE 'NO' END ACTIVE "
        strSql += " ,WEBDBNAME,WEBDBTBLPREFIX,WEBDBUSERNAME"
        strSql += "  FROM " & cnAdminDb & "..SYNCCOSTCENTRE as S"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dt As New DataTable
        da.Fill(dt)
        With gridView
            .DataSource = dt
            .Columns("COSTID").Width = 60
            .Columns("VERSION").Width = 75
            .Columns("STATE").Width = 200
            .Columns("EMAILID").Width = 250
            .Columns("FTPID").Width = 100
            .Columns("PASSWORD").Width = 100
            .Columns("COMPID").Width = 60
            .Columns("MAIN").Width = 40
            .Columns("MANUAL").Width = 40
            .Columns("ACTIVE").Width = 60
            .Columns("WEBDBNAME").Width = 75
            .Columns("WEBDBTBLPREFIX").Width = 75
            .Columns("WEBDBUSERNAME").Width = 75
        End With
    End Sub

    Private Sub frmSyncCostcentre_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSyncCostcentre_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtEmailId.CharacterCasing = CharacterCasing.Normal
        txtFtpId.CharacterCasing = CharacterCasing.Normal
        txtPassword.CharacterCasing = CharacterCasing.Normal
        cmbActive.Items.Clear()
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub txtEmailId_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtEmailId.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            checkEmailId(txtEmailId)
        End If
    End Sub

    Private Sub txtPassword_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPassword.GotFocus
        If txtEmailId.Text = "" And txtFtpId.Text = "" Then
            txtPassword.Clear()
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.GotFocus
        lblStatus.Visible = True
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridView.Rows.Count > 0 Then
                gridView.CurrentCell = gridView.CurrentRow.Cells(0)
            End If
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If Not gridView.RowCount > 0 Then Exit Sub
            gridView.CurrentCell = gridView.CurrentRow.Cells(0)
            With gridView.CurrentRow
                txtCostId.Text = .Cells("COSTID").Value.ToString
                cmbState.Text = .Cells("STATE").Value.ToString
                txtDbId.Text = .Cells("COMPID").Value.ToString
                txtEmailId.Text = .Cells("EMAILID").Value.ToString
                txtFtpId.Text = .Cells("FTPID").Value.ToString
                txtPassword.Text = BrighttechPack.Methods.Decrypt(.Cells("PASSWORD").Value.ToString)
                chkMain.Checked = IIf(.Cells("MAIN").Value.ToString = "Y", True, False)
                costId = .Cells("COSTID").Value.ToString
                chkManual.Checked = IIf(.Cells("MANUAL").Value.ToString = "Y", True, False)
                cmbActive.Text = .Cells("ACTIVE").Value.ToString
                txtWebDbname.Text = .Cells("WEBDBNAME").Value.ToString
                txtWebTblPrefix.Text = .Cells("WEBDBTBLPREFIX").Value.ToString
                txtWebDbUser.Text = .Cells("WEBDBUSERNAME").Value.ToString
                txtCostId.Focus()
            End With
        End If
    End Sub

    Private Sub cmbCostName_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then
            checkCostCentre()
        End If
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        btnSave_Click(Me, New EventArgs)
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        btnOpen_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView.LostFocus
        lblStatus.Visible = False
    End Sub
End Class