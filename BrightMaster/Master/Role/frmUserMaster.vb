Imports System.Data.OleDb
Imports System.IO
Public Class frmUserMaster
    Dim dtCostCentre As New DataTable
    Dim cmd As OleDbCommand
    Dim _da As OleDbDataAdapter
    Dim tran As OleDbTransaction
    Dim strSql As String
    Dim updFlag As Boolean
    Dim chitMaindb As String = GetAdmindbSoftValue("CHITDBPREFIX", , tran) + "SAVINGS"
    Dim _ChangePwd As Boolean = IIf(GetAdmindbSoftValue("USERPWDCHANGE", , tran).ToUpper.ToString = "Y", True, False)
    Dim UserImgbyte As Byte() = Nothing

    Private Sub CallGrid(Optional ByVal costid As String = Nothing)
        strSql = " SELECT "
        strSql += " DISTINCT (SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = U.COSTID)AS COSTNAME"
        strSql += " ,U.USERID,USERNAME,PWD,AUTHPWD,CASE WHEN ACTIVE = 'Y' THEN 'YES' ELSE 'NO' END AS ACTIVE,CENTLOGIN,PWDCHANGE,ISNULL(L.LOGINSTATUS,'') ISLOGGED,USERCOSTID"
        strSql += " FROM " & cnAdminDb & "..USERMASTER AS U"
        strSql += " LEFT JOIN " & cnAdminDb & "..LOGINDETAIL AS L ON L.USERID=U.USERID"
        If Not (costid Is Nothing) Then strSql += " WHERE COSTID ='" & costid & "'"
        Dim _dt As New DataTable
        _da = New OleDbDataAdapter(strSql, cn)
        _da.Fill(_dt)
        gridView.DataSource = _dt
        gridView.Columns("PWD").Visible = False
        gridView.Columns("PWDCHANGE").Visible = False
        gridView.Columns("USERCOSTID").Visible = False
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    Private Sub ChitUserMaster()

    End Sub
    Private Function encdecpassword(ByVal password As String) As String
        Dim pwdlen As Integer = Len(password) + 1
        Dim encdecpwd As String = "@@@@@("
        For iii As Integer = 1 To Len(password)
            encdecpwd = encdecpwd & Mid(password, iii, 1) & Mid(password, pwdlen - iii, 1)
        Next
        encdecpwd = encdecpwd + ")@@@@@"
        Return encdecpwd
    End Function
    Private Function InsertQry(ByVal CostId As String, Optional ByVal syncrecord As Boolean = False, Optional ByVal usercostid As String = Nothing) As String
        Dim Qry As String = ""
        'If syncrecord Then Qry = "@@@###"

        'Dim costids As String = "ALL"
        'If chkCmbCostCentre.Text <> "ALL" Then
        '    costids = GetSelectedCostId(chkCmbCostCentre, True)
        'End If

        'Newly Add Column USERIMAGE
        Qry += " INSERT INTO " & cnAdminDb & "..USERMASTER"
        Qry += " (COSTID,USERID,USERNAME,PWD,AUTHPWD,ACTIVE,UPUSERID,UPDATED,UPTIME,CENTLOGIN,PWDCHANGE,PWDUPDATED,USERCOSTID,USERIMAGE)VALUES"
        Qry += " ("
        Qry += " '" & CostId & "'" 'COSTID
        Qry += " ," & Val(txtUserId.Text) & "" 'USEID
        Qry += " ,'" & txtUserName_MAN.Text & "'" 'USERNAME
        If syncrecord Then
            Qry += " ,'" & encdecpassword(txtPassword_MAN.Text) & "'"
            Qry += " ,'" & encdecpassword(txtAuthPassword.Text) & "'"
        Else
            Qry += " ,'" & BrighttechPack.Methods.Encrypt(txtPassword_MAN.Text) & "'" 'PASSWORD
            Qry += " ,'" & BrighttechPack.Methods.Encrypt(txtAuthPassword.Text) & "'" 'PASSWORD
        End If
        Qry += " ,'" & Mid(cmbActive.Text, 1, 1) & "'" 'ACTIVE
        Qry += " ," & userId & ""
        Qry += " ,'" & GetEntryDate(GetServerDate(tran), tran) & "'"
        Qry += " ,'" & GetServerTime(tran) & "'"
        Qry += " ,'" & IIf(chkCentralizedLogin.Checked, "Y", "N") & "'"
        If Val(txtChangePwd_NUM.Text) <> 0 Then
            Qry += " ," & Val(txtChangePwd_NUM.Text) & "" ''PWDCHANGE
        Else
            Qry += " ,NULL"
        End If
        Qry += " ,'" & GetEntryDate(GetServerDate(tran), tran) & "'" ''PWDUPDATED
        Qry += " ,'" & usercostid & "'" ''usercostid
        'Newly
        Qry += " , ''" ''USERIMAGE
        Qry += " )"
        If chitMaindb <> "" Then
            Qry += " INSERT INTO " & chitMaindb & "..USERMASTER"
            Qry += " (COSTID,USERID,USERNAME,PWD,ACTIVE,CENTLOGIN,USERCOSTID)VALUES"
            Qry += " ("
            Qry += " '" & CostId & "'" 'COSTID
            Qry += " ," & Val(txtUserId.Text) & "" 'USEID
            Qry += " ,'" & txtUserName_MAN.Text & "'" 'USERNAME
            If syncrecord Then
                Qry += " ,'" & encdecpassword(txtPassword_MAN.Text) & "'"
            Else
                Qry += " ,'" & BrighttechPack.Methods.Encrypt(txtPassword_MAN.Text) & "'" 'PASSWORD
            End If
            Qry += " ,'" & Mid(cmbActive.Text, 1, 1) & "'" 'ACTIVE
            Qry += " ,'" & IIf(chkCentralizedLogin.Checked, "Y", "N") & "'"
            Qry += " ,'" & usercostid & "'" ''usercostid
            Qry += " )"
        End If
        Return Qry
    End Function

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click, SaveToolStripMenuItem.Click
        If _SyncTo <> "" Then
            MsgBox("Master entry cannot allow at location", MsgBoxStyle.Information)
            Exit Sub
        End If
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Save) Then Exit Sub
        If objGPack.Validator_Check(Me) Then Exit Sub
        If txtPassword_MAN.Text <> txtReTypePassword.Text Then
            MsgBox("Password mismatch", MsgBoxStyle.Information)
            txtReTypePassword.Focus()
            Exit Sub
        End If
        If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME = '" & txtUserName_MAN.Text & "' AND USERID <> " & Val(txtUserId.Text) & "").Length > 0 Then
            MsgBox("User Name Already Exist", MsgBoxStyle.Information)
            txtUserName_MAN.Focus()
            Exit Sub
        End If
        Dim DelQry As String = Nothing
        If updFlag Then
            DelQry = " DELETE FROM " & cnAdminDb & "..USERMASTER WHERE USERID = '" & txtUserId.Text & "'"
            If chitMaindb <> "" Then
                DelQry += " DELETE FROM " & chitMaindb & "..USERMASTER WHERE USERID = '" & txtUserId.Text & "'"
            End If
        End If
        Dim chkCostId As String = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME")
        If chkCostId = "ALL" Then
            strSql = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE ISNULL(ACTIVE,'Y')<>'N'"
            Dim dtCost As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCost)
            If dtCost.Rows.Count > 0 Then
                chkCostId = ""
                For i As Integer = 0 To dtCost.Rows.Count - 1
                    chkCostId = chkCostId + dtCost.Rows(i).Item("COSTID").ToString + ","
                Next
                If chkCostId <> "" Then chkCostId = Mid(chkCostId, 1, Len(chkCostId) - 1)
            End If
        End If
        '   Dim costId As String = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostcentre_MAN.Text & "'")
        Dim costId As String
        Dim costidarray() As String = chkCostId.Split(",")
        Try
            tran = cn.BeginTransaction
            strSql = DelQry + InsertQry(cnCostId, False, chkCostId)
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
            If chkCentralizedLogin.Checked Then
                For Each Ro As DataRow In dtCostCentre.Rows
                    If Ro.Item("COSTID").ToString.ToUpper = cnCostId.ToUpper Then Continue For
                    strSql = DelQry + InsertQry(Ro.Item("COSTID").ToString, , Ro.Item("COSTID").ToString)
                    Exec(strSql.Replace("'", "''"), cn, Ro.Item("COSTID").ToString, OpenFileDialog1.FileName.ToString, tran, "" & cnAdminDb & ":USERMASTER:USERIMAGE:USERID:" & Val(txtUserId.Text), , UserImgbyte)
                Next
            Else
                For ii As Integer = 0 To costidarray.Length - 1
                    costId = costidarray(ii).ToString
                    If costId = cnCostId Then Continue For
                    strSql = DelQry + InsertQry(costId, False, chkCostId)
                    Exec(strSql.Replace("'", "''"), cn, costId, OpenFileDialog1.FileName.ToString, tran, "" & cnAdminDb & ":USERMASTER:USERIMAGE:USERID:" & Val(txtUserId.Text), , UserImgbyte)
                Next
            End If

            If OpenFileDialog1.FileName.ToString <> "" Then
                Dim ImageTemp As Byte() = ConvertImageFiletoBytes(OpenFileDialog1.FileName.ToString)
                If Not ImageTemp Is Nothing Then
                    If ImageTemp.Length > 0 Then
                        strSql = "UPDATE " & cnAdminDb & "..USERMASTER SET USERIMAGE = ? "
                        strSql += " WHERE  USERID = " & txtUserId.Text & ""
                        cmd = New OleDbCommand(strSql, cn, tran)
                        cmd.Parameters.AddWithValue("@USERIMAGE", ImageTemp)
                        cmd.ExecuteNonQuery()
                        If chitMaindb <> "" Then
                            strSql = "UPDATE " & chitMaindb & "..USERMASTER SET USERIMAGE = ? "
                            strSql += " WHERE  USERID = " & txtUserId.Text & ""
                            cmd = New OleDbCommand(strSql, cn, tran)
                            cmd.Parameters.AddWithValue("@USERIMAGE", ImageTemp)
                            cmd.ExecuteNonQuery()
                        End If
                    End If
                End If
            End If
            If Not UserImgbyte Is Nothing Then
                If UserImgbyte.Length > 0 Then
                    strSql = "UPDATE " & cnAdminDb & "..USERMASTER SET USERIMAGE = ? "
                    strSql += " WHERE  USERID = " & txtUserId.Text & ""
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.Parameters.AddWithValue("@USERIMAGE", UserImgbyte)
                    cmd.ExecuteNonQuery()
                    If chitMaindb <> "" Then
                        strSql = "UPDATE " & chitMaindb & "..USERMASTER SET USERIMAGE = ? "
                        strSql += " WHERE  USERID = " & txtUserId.Text & ""
                        cmd = New OleDbCommand(strSql, cn, tran)
                        cmd.Parameters.AddWithValue("@USERIMAGE", UserImgbyte)
                        cmd.ExecuteNonQuery()
                    End If
                End If
            End If
            tran.Commit()
            tran = Nothing
            UserImgbyte = Nothing
            OpenFileDialog1.FileName = ""
            If updFlag = True Then
                MsgBox("Updated..")
            Else
                MsgBox("Saved..")
                End If
            btnNew_Click(Me, New EventArgs)
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information)
        End Try
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
        'Dim cost As String = cmbCostcentre_MAN.Text
        objGPack.TextClear(Me)
        'Newly Add
        picbxUserImage.Image = My.Resources.noimagenew
        'cmbCostcentre_MAN.Text = cost
        txtUserId.Text = Val(objGPack.GetSqlValue("SELECT ISNULL(MAX(USERID),0)+ 1 FROM " & cnAdminDb & "..USERMASTER WHERE USERID <> 999"))
        tabMain.SelectedTab = tabGeneral
        updFlag = False
        cmbActive.Text = "YES"
        chkCentralizedLogin.Checked = False
        CallGrid()
        For i As Integer = 0 To chkCmbCostCentre.Items.Count - 1
            chkCmbCostCentre.SetItemCheckState(i, CheckState.Unchecked)
        Next
        txtUserId.Focus()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub frmUserMaster_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If tabMain.SelectedTab.Name = tabView.Name Then
                btnBack_Click(Me, New EventArgs)
            End If
        End If
    End Sub

    Private Sub frmUserMaster_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub UserMaster_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGeneral.Left, Me.tabGeneral.Top, Me.tabGeneral.Width, Me.tabGeneral.Height))

        txtPassword_MAN.CharacterCasing = CharacterCasing.Normal
        txtReTypePassword.CharacterCasing = CharacterCasing.Normal
        txtAuthPassword.CharacterCasing = CharacterCasing.Normal
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
            strSql += " UNION ALL"
            strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
            strSql += " ORDER BY RESULT,COSTNAME"
            dtCostCentre = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCostCentre)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))

            objGPack.FillCombo(strSql, cmbCostcentrev, , False)
            cmbCostcentrev.Enabled = True

        Else
            chkCmbCostCentre.Enabled = False
            chkCmbCostCentre.Enabled = False
        End If
        cmbActive.Items.Clear()
        cmbActive.Items.Add("YES")
        cmbActive.Items.Add("NO")
        If Not objGPack.GetSqlValue("SELECT NAME FROM SYSDATABASES WHERE NAME = '" & chitMaindb & "'", , , tran).Length > 0 Then
            chitMaindb = ""
        End If
        strSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID <> '" & cnCostId & "'"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        If _ChangePwd Then
            PnlChangePwd.Visible = True
        Else
            PnlChangePwd.Visible = False
            PnlAut.Location = New Drawing.Point(PnlAut.Location.X, PnlAut.Location.Y - PnlChangePwd.Height)
            PnlBottom.Location = New Drawing.Point(PnlBottom.Location.X, PnlBottom.Location.Y - PnlChangePwd.Height)
        End If
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub txtUserId_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtUserId.GotFocus
        'SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtUserName_MAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtUserName_MAN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtUserName_MAN.Text = "" Then Exit Sub
            If objGPack.GetSqlValue("SELECT 'CHECK' FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME = '" & txtUserName_MAN.Text & "' AND USERID <> " & Val(txtUserId.Text) & "").Length > 0 Then
                MsgBox("User Name Already Exist", MsgBoxStyle.Information)
                txtUserName_MAN.Focus()
            End If
        End If
    End Sub

    Private Sub txtReTypePassword_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtReTypePassword.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtPassword_MAN.Text <> txtReTypePassword.Text Then
                MsgBox("Password mismatch", MsgBoxStyle.Information)
                txtReTypePassword.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGeneral
        txtUserId.Focus()
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress

        ' UPDATE IMAGE
        Dim tempbyte As Byte() = Nothing
        Dim dt_image As New DataTable
        Dim imgMemoryStream As MemoryStream = New MemoryStream()

        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
            If Not gridView.RowCount > 0 Then Exit Sub
            gridView.CurrentCell = gridView.Rows(gridView.CurrentRow.Index).Cells("USERID")
            With gridView.CurrentRow
                'cmbCostcentre.Text = .Cells("COSTNAME").Value.ToString
                txtUserId.Text = .Cells("USERID").Value.ToString
                txtUserName_MAN.Text = .Cells("USERNAME").Value.ToString
                txtPassword_MAN.Text = BrighttechPack.Methods.Decrypt(.Cells("PWD").Value.ToString)
                txtReTypePassword.Text = BrighttechPack.Methods.Decrypt(.Cells("PWD").Value.ToString)
                txtAuthPassword.Text = BrighttechPack.Methods.Decrypt(.Cells("AUTHPWD").Value.ToString)
                cmbActive.Text = .Cells("ACTIVE").Value.ToString
                chkCentralizedLogin.Checked = IIf(.Cells("CENTLOGIN").Value.ToString = "Y", True, False)
                txtChangePwd_NUM.Text = .Cells("PWDCHANGE").Value.ToString

                'View Image Newly add
                strSql = "SELECT USERIMAGE FROM " & cnAdminDb & "..USERMASTER WHERE USERID= " & txtUserId.Text & ""
                'strSql = "SELECT PHOTO_PATH AS USERIMAGE FROM " & cnAdminDb & "..NEWMEMBERPERSONALINFO WHERE TRANID='188350'"
                da = New OleDbDataAdapter(strSql, cn)
                dt_image = New DataTable
                da.Fill(dt_image)

                If (IsDBNull(dt_image.Rows(0).Item("USERIMAGE")) = True) Then
                    picbxUserImage.Image = My.Resources.noimagenew
                Else
                    tempbyte = dt_image.Rows(0).Item("USERIMAGE")
                    UserImgbyte = dt_image.Rows(0).Item("USERIMAGE")
                    Dim check As Integer = tempbyte.Length
                    If check > 0 Then
                        imgMemoryStream = New MemoryStream(tempbyte)
                        picbxUserImage.Image = Drawing.Image.FromStream(imgMemoryStream)
                        picbxUserImage.SizeMode = PictureBoxSizeMode.StretchImage
                    Else
                        picbxUserImage.Image = My.Resources.noimagenew
                    End If
                End If


                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                updFlag = True

                For i As Integer = 0 To chkCmbCostCentre.Items.Count - 1
                    chkCmbCostCentre.SetItemCheckState(i, CheckState.Unchecked)
                Next
                If .Cells("USERCOSTID").Value.ToString <> "" Then
                    Dim dtusercostid As New DataTable
                    strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID IN ('" & Replace(.Cells("USERCOSTID").Value.ToString, ",", "','") & "')"
                    da = New OleDbDataAdapter(strSql, cn)
                    da.Fill(dtusercostid)
                    If dtusercostid.Rows.Count > 0 Then
                        For iij As Integer = 0 To dtusercostid.Rows.Count - 1
                            If chkCmbCostCentre.Items.Contains(dtusercostid.Rows(iij).Item("COSTNAME").ToString) Then
                                Dim INDX As Integer = chkCmbCostCentre.Items.IndexOf(dtusercostid.Rows(iij).Item("COSTNAME").ToString)
                                chkCmbCostCentre.SetItemCheckState(INDX, CheckState.Checked)
                            End If
                        Next
                    End If
                End If
                btnBack_Click(Me, New EventArgs)
            End With
        ElseIf UCase(e.KeyChar) = "D" Then
            btnDelete_Click(Me, New EventArgs)
        ElseIf UCase(e.KeyChar) = "C" Then
            If gridView.CurrentRow.Cells("ISLOGGED").Value.ToString <> "" Then
                Dim Strsql As String = "delete from " & cnAdminDb & "..LOGINDETAIL WHERE USERID = " & Val(gridView.CurrentRow.Cells("USERID").Value.ToString) & ""
                cmd = New OleDbCommand(Strsql, cn)
                cmd.ExecuteNonQuery()
                CallGrid()
                gridView.Focus()
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Delete) Then Exit Sub
        If Not gridView.RowCount > 0 Then Exit Sub
        Dim list As New List(Of String)
        list.Add("USERID")
        Dim userId As String = gridView.CurrentRow.Cells("USERID").Value.ToString
        If DeleteItem(SyncMode.Master, list, "DELETE FROM " & cnAdminDb & "..USERMASTER WHERE USERID = '" & userId & "' AND USERID <> 999", userId, "USERMASTER") Then
            Dim chitMaindb As String = GetAdmindbSoftValue("CHITDBPREFIX", , tran) + "SAVINGS"
            If objGPack.GetSqlValue("SELECT NAME FROM SYSDATABASES WHERE NAME = '" & chitMaindb & "'", , , tran).Length > 0 Then
                DeleteItem(SyncMode.Master, list, "DELETE FROM " & chitMaindb & "..USERMASTER WHERE USERID = '" & userId & "' AND USERID <> 999", userId, "USERMASTER")
            End If
            CallGrid()
            gridView.Focus()
        End If
    End Sub

    Private Sub txtUserId_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtUserId.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If updFlag = True Then Exit Sub
            If objGPack.GetSqlValue("SELECT 1 FROM " & cnAdminDb & "..USERMASTER WHERE USERID ='" & Val(txtUserId.Text) & "'").Length > 0 Then
                MsgBox("UserId already exists", MsgBoxStyle.Information)
                txtUserId.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub cmbCostcentrev_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbCostcentrev.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim costId As String
            If cmbCostcentrev.Text <> "" And cmbCostcentrev.Text <> "ALL" Then
                costId = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostcentrev.Text & "'")
            Else
                costid = Nothing
            End If
            CallGrid(costId)
        End If
    End Sub

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Private Sub btnUserImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUserImage.Click
        If OpenFileDialog1.ShowDialog <> Windows.Forms.DialogResult.Cancel Then
            picbxUserImage.Image = Image.FromFile(OpenFileDialog1.FileName)
        End If
        SendKeys.Send("{TAB}")
    End Sub

#Region "USER Define Function"
    Public Function ConvertImageFiletoBytes(ByVal ImageFilePath As String) As Byte()
        Dim _tempByte() As Byte = Nothing
        If String.IsNullOrEmpty(ImageFilePath) = True Then
            Throw New ArgumentNullException("Image File Name Cannot be Null or Empty", "ImageFilePath")
            Return Nothing
        End If
        Try
            Dim _fileInfo As New IO.FileInfo(ImageFilePath)
            Dim _NumBytes As Long = _fileInfo.Length
            Dim _FStream As New IO.FileStream(ImageFilePath, IO.FileMode.Open, IO.FileAccess.Read)
            Dim _BinaryReader As New IO.BinaryReader(_FStream)
            _tempByte = _BinaryReader.ReadBytes(Convert.ToInt32(_NumBytes))
            _fileInfo = Nothing
            _NumBytes = 0
            _FStream.Close()
            _FStream.Dispose()
            _BinaryReader.Close()
            Return _tempByte
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
#End Region
End Class