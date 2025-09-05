Imports System.Data.OleDb
Public Class frmUserPassword
    Dim strSql As String
    Public userpwdid As Integer
    Dim OptId As Integer
    Dim pwd As String
    Dim Final_disc_val As String
    Dim Final_disc_per As String = ""
    Dim CancelBillNo As String = ""
    Dim MsgStr As String
    Dim MobileNo As String = ""
    Dim showPassword As Boolean = True
    Dim PWDCLOSETIME As Integer = Val(GetAdmindbSoftValue("PWDCLOSETIME", "").ToString)
    Dim cmd As OleDbCommand
    Dim _tran As OleDbTransaction = Nothing
    Private Sub frmUserPassword_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim drsms As DataRow = Nothing
        Dim mobile As String() = Nothing
        Dim Optionid As String = ""
        strSql = "SELECT OPTIONNAME,MOBILENO FROM " & cnAdminDb & "..PRJPWDOPTION WHERE OPTIONID ='" + OptId.ToString + "' AND ACTIVE = 'Y' AND ISNULL(PWDTYPE,'')='S'"
        drsms = GetSqlRow(strSql, cn)
        Dim Msg As String = ""
        If Not drsms Is Nothing Then
            Dim OptName As String = ""
            'If MobileNo Is Nothing Then
            '    MobileNo = drsms("MOBILENO").ToString
            'End If
            strSql = "SELECT TEMPLATE_DESC FROM " & cnAdminDb & "..SMSTEMPLATE WHERE TEMPLATE_NAME='" + drsms.Item("OPTIONNAME").ToString + "'"
            Dim TempMsg As String = GetSqlValue(cn, strSql).ToString
            If Not String.IsNullOrEmpty(TempMsg) And String.IsNullOrEmpty(MsgStr) Then MsgStr = TempMsg
            If MsgStr = Nothing Then
                If Final_disc_val <> "" Then
                    Msg = "Pls Share the OTP <OTP> for " + drsms.Item("OPTIONNAME").ToString + ". FINALAMOUNT : " + Final_disc_val + " . Initiated by " + cnUserName.ToString + ", " + cnCompanyName.ToString.Trim & "."
                Else
                    Msg = "Pls Share the OTP <OTP> for " + drsms.Item("OPTIONNAME").ToString + ". Initiated by " + cnUserName.ToString + ", " + cnCompanyName.ToString.Trim & "."
                End If
            Else
                Msg = MsgStr
            End If
            'userpwdid = funcGenPwd1(OptId.ToString, drsms("MOBILENO").ToString, Msg.Trim, True) '''''CHANGEd FOR CUSTOMER OTP
            If MobileNo Is Nothing Then
                MobileNo = ""
            End If
            If MobileNo.Length > 0 And MobileNo.Length = 10 Then
                MobileNo = MobileNo + IIf(drsms("MOBILENO").ToString <> "", "," + drsms("MOBILENO").ToString, "")
                userpwdid = funcGenPwd1(OptId.ToString, MobileNo.ToString.Trim, Msg.Trim, True, Final_disc_val, Final_disc_per, CancelBillNo.ToString)
            Else
                MobileNo = drsms("MOBILENO").ToString
                userpwdid = funcGenPwd1(OptId.ToString, drsms("MOBILENO").ToString, Msg.Trim, True, Final_disc_val, Final_disc_per, CancelBillNo.ToString)
            End If
            showPassword = False
        End If

        If cnUserName <> "" Then Me.Text = cnUserName.ToString.ToUpper & " PASSWORD"
        If PWDCLOSETIME > 0 Then
            strSql = " SELECT DATEDIFF(MINUTE,CAST(PWDTIME AS time),CAST (GETDATE() AS time)) PWDCLOSETIME FROM " & cnAdminDb & "..PWDMASTER "
            strSql += vbCrLf + " WHERE PWDSTATUS = 'N' AND PWDUSERID = " & userId & " AND PWDDATE <=GETDATE() AND PWDID = '" & userpwdid & "'"
            If Val(GetSqlValue(cn, strSql).ToString) > PWDCLOSETIME Then
                Try
                    _tran = Nothing
                    _tran = cn.BeginTransaction
                    strSql = " UPDATE " & cnAdminDb & "..PWDMASTER SET PWDSTATUS = 'E'"
                    strSql += vbCrLf + " , PWDCLSDATE = '" & Format(Now.Date, "yyyy-MM-dd") & "'"
                    strSql += vbCrLf + " , PWDCLSTIME = '" & Now.ToLongTimeString & "'"
                    strSql += vbCrLf + "  WHERE PWDSTATUS = 'N' AND PWDUSERID = " & userId & " "
                    strSql += vbCrLf + " AND PWDDATE <=GETDATE() AND PWDID = '" & userpwdid & "' "
                    cmd = New OleDbCommand(strSql, cn, _tran)
                    cmd.ExecuteNonQuery()
                    _tran.Commit()
                    _tran = Nothing
                Catch ex As Exception
                    If Not _tran Is Nothing Then
                        _tran.Rollback()
                        _tran = Nothing
                        MessageBox.Show(ex.ToString)
                        Exit Sub
                    Else
                        MessageBox.Show(ex.ToString)
                        Exit Sub
                    End If
                End Try
                Dim cnt As Integer = 0
                cnt = objGPack.GetSqlValue("SELECT COUNT(*) CNT FROM " & cnAdminDb & "..PWDMASTER WHERE PWDID = " & userpwdid & " AND ISNULL(PWDSTATUS,'')  NOT IN('C','E') ", , Nothing)
                If cnt = 0 Then
                    MsgBox("OTP Require", MsgBoxStyle.Information)
                    Me.Close()
                End If
            End If
        End If
        pwd = objGPack.GetSqlValue("SELECT PASSWORD FROM " & cnAdminDb & "..PWDMASTER WHERE PWDID = " & userpwdid & " AND ISNULL(PWDSTATUS,'')  NOT IN('C','E') ", , Nothing)
        txtPassword.CharacterCasing = CharacterCasing.Normal
        lblUsrPwd.Text = BrighttechPack.Methods.Decrypt(pwd)
        If showPassword = False Then
            lblUsrPwd.Visible = False
            lnkResend.Visible = True
            Me.Text = "One Time Password"
        End If
    End Sub

    Private Sub txtPassword_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPassword.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If userpwdid = Nothing Then
                MsgBox("Authorised Password Is Empty", MsgBoxStyle.Information)
                txtPassword.Focus()
                txtPassword.SelectAll()
                Me.Close()
                Exit Sub
            End If
            If BrighttechPack.Methods.Encrypt(txtPassword.Text) <> pwd Then
                MsgBox("Authorised Password Invalid", MsgBoxStyle.Information)
                txtPassword.Clear()
                txtPassword.Focus()
                txtPassword.SelectAll()
                Me.Close()
                Exit Sub
            End If
            Me.DialogResult = Windows.Forms.DialogResult.OK
            strSql = " UPDATE " & cnAdminDb & "..PWDMASTER SET PWDSTATUS='C',PWDCLSDATE= '" + GetServerDate() + "',PWDCLSTIME='" & GetServerTime() + "'"
            strSql += " WHERE PWDID=" & userpwdid
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, cnCostId)

            'Update  a set  FROM " & cnAdminDb & "..PWDMASTER a WHERE PWDID = " & userpwdId  
            'Me.Close()
        End If
    End Sub

    Public Sub New(ByVal pwdid As String, ByVal OptionId As Integer, Optional ByVal ShowPwd As Boolean = True, Optional ByVal Mobile As String = Nothing _
                   , Optional ByVal Msg As String = Nothing, Optional ByVal Fin_disc As String = Nothing, Optional ByVal Fin_disc_per As String = Nothing _
                   , Optional ByVal Billno As String = "")
        userpwdid = pwdid
        showPassword = ShowPwd
        MobileNo = Mobile
        OptId = OptionId
        MsgStr = Msg
        Final_disc_val = Fin_disc
        Final_disc_per = Fin_disc_per
        CancelBillNo = Billno
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub lnkResend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkResend.Click
        funcGenPwd(OptId, MobileNo, MsgStr, True)
        pwd = objGPack.GetSqlValue("SELECT PASSWORD FROM " & cnAdminDb & "..PWDMASTER WHERE PWDOPTIONID = " & OptId & " AND ISNULL(PWDSTATUS,'')  NOT IN('C','E') ", , Nothing)
    End Sub

    Private Sub frmUserPassword_ImeModeChanged(sender As Object, e As EventArgs) Handles Me.ImeModeChanged

    End Sub
End Class