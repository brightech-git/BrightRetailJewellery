Imports System.Data.OleDb
Public Class frmPasswordOption
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dtmenu, dt As New DataTable
    Dim strSql As String
    Dim updFlag As Boolean
    Dim upopid As Integer = 0
    Private Sub frmPasswordOption_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))
        funnew()
    End Sub
    Function funnew()
        strSql = "SELECT MENUID,MENUTEXT FROM " & cnAdminDb & "..PRJMENUS"
        da = New OleDbDataAdapter(strSql, cn)
        dtmenu = New DataTable()
        da.Fill(dtmenu)
        If dtmenu.Rows.Count > 0 Then
            cmbmenuid_OWN.DataSource = Nothing
            cmbmenuid_OWN.DataSource = dtmenu
            cmbmenuid_OWN.DisplayMember = "MENUTEXT"
            cmbmenuid_OWN.ValueMember = "MENUID"
            cmbmenuid_OWN.SelectedIndex = 0
        End If
        txtoptionname.Text = ""
        chkalt.Checked = False
        chkshft.Checked = False
        cmbshortkeys.SelectedIndex = 0
        cmboptiontype.SelectedIndex = 0
        cmbActive.SelectedIndex = 0
        CmbOtpType.SelectedIndex = 0
        cmbmenuid_OWN.Enabled = True
        cmbmenuid_OWN.Focus()
        updFlag = False
        txtoptionname.ReadOnly = False
    End Function
    Function search()
        strSql = " SELECT MENUID,OPTIONID,OPTIONNAME"
        strSql += " ,CASE WHEN OPTIONTYPE='OP' THEN 'OneTime' "
        strSql += " WHEN OPTIONTYPE='DP' THEN 'Daily' "
        strSql += " WHEN OPTIONTYPE='WP' THEN 'Weekly' "
        strSql += " WHEN OPTIONTYPE='MP' THEN 'Monthly' "
        strSql += " WHEN OPTIONTYPE='YP' THEN 'Yearly' END AS OPTIONTYPE"
        strSql += " ,CASE WHEN ISNULL(ACTIVE,'N')='Y' THEN 'Yes' ELSE 'No' END ACTIVE"
        strSql += " ,CASE WHEN ISNULL(PWDTYPE,'D')='D' THEN 'DB' "
        strSql += " WHEN PWDTYPE='S' THEN 'SMS' "
        strSql += " WHEN PWDTYPE='O' THEN 'OPTIONAL' "
        strSql += " ELSE 'DB' END PWDTYPE"
        strSql += " ,MOBILENO"
        strSql += " FROM " & cnAdminDb & "..PRJPWDOPTION"
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtgrid As New DataTable
        da.Fill(dtgrid)
        If dtgrid.Rows.Count > 0 Then
            gridView.DataSource = Nothing
            gridView.DataSource = dtgrid
            With gridView
                .Columns("MENUID").Visible = False
                .Columns("OPTIONID").HeaderText = "ID"
                .Columns("OPTIONNAME").HeaderText = "NAME"
                .Columns("OPTIONTYPE").HeaderText = "TYPE"
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                .Invalidate()
                For Each dgvCol As DataGridViewColumn In .Columns
                    dgvCol.Width = dgvCol.Width
                Next
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End With
            tabMain.SelectedTab = tabView
            gridView.Focus()
        Else
            gridView.DataSource = Nothing
            MsgBox("Records not found.", MsgBoxStyle.Information)
        End If
    End Function
    Function Save()
        If txtoptionname.Text = "" Then
            MsgBox("Optionname should not Empty.", MsgBoxStyle.Information)
            txtoptionname.Focus()
            txtoptionname.SelectAll()
            Exit Function
        End If
        Dim OPID As Int64
        Dim OPKEY As String = ""
        Dim optype As String = ""
        If cmboptiontype.Text.ToString() = "OneTime" Then
            optype = "OP"
        ElseIf cmboptiontype.Text.ToString() = "Daily" Then
            optype = "DP"
        ElseIf cmboptiontype.Text.ToString() = "Weekly" Then
            optype = "WP"
        ElseIf cmboptiontype.Text.ToString() = "Monthly" Then
            optype = "MP"
        ElseIf cmboptiontype.Text.ToString() = "Yearly" Then
            optype = "YP"
        End If
        strSql = "SELECT ISNULL(MAX(OPTIONID),0) FROM " & cnAdminDb & "..PRJPWDOPTION"
        OPKEY = ""
        OPID = Val(GetSqlValue(cn, strSql).ToString()) + 1
        If chkalt.Checked Then
            OPKEY = "ctrl+"
        ElseIf chkshft.Checked Then
            OPKEY += "shift+"
        End If
        If cmbshortkeys.Text <> "" Then
            OPKEY += cmbshortkeys.Text.ToString()
        End If
        Dim Type As String = "D"
        If CmbOtpType.Text = "SMS" Then
            Type = "S"
        ElseIf CmbOtpType.Text = "OPTIONAL" Then
            Type = "O"
        Else
            Type = "D"
        End If
        If Not updFlag Then
            strSql = "INSERT INTO " & cnAdminDb & "..PRJPWDOPTION(OPTIONID,MENUID,OPTIONNAME,OPTIONKEY,OPTIONTYPE,ACTIVE,PWDTYPE,MOBILENO)"
            strSql += vbCrLf + " VALUES( "
            strSql += vbCrLf + "" & OPID & ""
            strSql += vbCrLf + ",'" & cmbmenuid_OWN.SelectedValue.ToString() & "'"
            strSql += vbCrLf + ",'" & txtoptionname.Text.ToString() & "'"
            strSql += vbCrLf + ",'" & OPKEY & "'"
            strSql += vbCrLf + ",'" & optype & "'"
            strSql += vbCrLf + ",'" & IIf(cmbActive.Text.ToString() = "Yes", "Y", "N") & "'"
            strSql += vbCrLf + ",'" & Type & "'"
            strSql += vbCrLf + ",'" & txtMobileNo.Text.ToString & "')"
            ExecQuery(SyncMode.Master, strSql, cn)
            'cmd = New OleDbCommand(strSql, cn)
            'cmd.ExecuteNonQuery()
            MsgBox("Records added successfully.", MsgBoxStyle.Information)
        Else
            strSql = "UPDATE " & cnAdminDb & "..PRJPWDOPTION SET"
            strSql += vbCrLf + " OPTIONNAME='" & txtoptionname.Text.ToString() & "'"
            strSql += vbCrLf + ",OPTIONKEY='" & OPKEY & "'"
            strSql += vbCrLf + ",OPTIONTYPE='" & optype & "'"
            strSql += vbCrLf + ",ACTIVE='" & IIf(cmbActive.Text.ToString = "Yes", "Y", "N") & "'"
            strSql += vbCrLf + ",PWDTYPE='" & Type & "'"
            strSql += vbCrLf + ",MOBILENO='" & txtMobileNo.Text & "'"
            strSql += vbCrLf + " WHERE OPTIONID=" & upopid & " "
            If cmbmenuid_OWN.Text <> "" Then
                strSql += vbCrLf + " AND MENUID='" & cmbmenuid_OWN.SelectedValue.ToString & "'"
            End If
            ExecQuery(SyncMode.Master, strSql, cn)
            'cmd = New OleDbCommand(strSql, cn)
            'cmd.ExecuteNonQuery()
            MsgBox("Records updated successfully.", MsgBoxStyle.Information)
        End If
        funnew()
    End Function

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        funnew()
    End Sub

    Private Sub frmPasswordOption_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyCode = Keys.Escape Then
            tabMain.SelectedTab = tabGeneral
            funnew()
        End If
    End Sub

    Private Sub txtoptionname_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtoptionname.Leave
        If updFlag = False Then
            strSql = "SELECT OPTIONNAME FROM " & cnAdminDb & "..PRJPWDOPTION WHERE OPTIONNAME='" & txtoptionname.Text.ToString & "'"
            strSql += vbCrLf + " AND MENUID='" & cmbmenuid_OWN.SelectedValue.ToString() & "' "
            da = New OleDbDataAdapter(strSql, cn)
            dt = New DataTable()
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                MsgBox("This Optionname is already exists belong to this menuid.", MsgBoxStyle.Information)
                txtoptionname.Focus()
                txtoptionname.SelectAll()
            End If
        End If
        If txtoptionname.Text = "" Then
            MsgBox("Optionname should not Empty.", MsgBoxStyle.Information)
            txtoptionname.Focus()
            txtoptionname.SelectAll()
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If _SyncTo <> "" Then MsgBox("Master Entry cannot allow at Location", MsgBoxStyle.Information) : Exit Sub
        Save()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, OpenToolStripMenuItem.Click
        search()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        funnew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
            If _SyncTo <> "" Then MsgBox("Master entry cannot allow at location", MsgBoxStyle.Information) : Exit Sub
            If gridView.Rows.Count > 0 Then
                With gridView
                    upopid = Val(.CurrentRow.Cells("OPTIONID").Value.ToString)
                    cmbActive.Text = .CurrentRow.Cells("ACTIVE").Value.ToString
                    cmbmenuid_OWN.Text = ""
                    cmboptiontype.Text = .CurrentRow.Cells("OPTIONTYPE").Value.ToString
                    txtoptionname.Text = .CurrentRow.Cells("OPTIONNAME").Value.ToString
                    CmbOtpType.Text = .CurrentRow.Cells("PWDTYPE").Value.ToString
                    txtMobileNo.Text = .CurrentRow.Cells("MOBILENO").Value.ToString
                    tabMain.SelectedTab = tabGeneral
                    txtoptionname.Focus()
                    txtoptionname.ReadOnly = True
                    updFlag = True
                    If cmbmenuid_OWN.Text = "" Then cmbmenuid_OWN.Enabled = False : txtoptionname.Focus() Else cmbmenuid_OWN.Focus()
                    'Dim mactive As String = ""
                    'If .CurrentRow.Cells("ACTIVE").Value.ToString = "Y" Then
                    '    mactive = "N"
                    'Else
                    '    mactive = "Y"
                    'End If
                    'strSql = "UPDATE " & cnAdminDb & "..PRJPWDOPTION SET"
                    'strSql += vbCrLf + "ACTIVE='" & mactive & "'"
                    'strSql += vbCrLf + " WHERE OPTIONID=" & upopid
                    'ExecQuery(SyncMode.Master, strSql, cn)
                    '.CurrentRow.Cells("ACTIVE").Value = mactive
                End With
            End If
        End If
    End Sub

    Private Sub CmbOtpType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbOtpType.SelectedIndexChanged
        If CmbOtpType.Text = "DB" Then
            PnlMobile.Visible = False
        Else
            PnlMobile.Visible = True
        End If
    End Sub
End Class