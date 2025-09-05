Imports System.Data.OleDb
Public Class MaterialIssRecRateChangeBox

#Region " User Define Function "
    Dim strsql As String = ""
    Public cmbTrantype As String = ""
    Public cmbAcname As String = ""
    Public ItemId As Integer = 0
    Public Subitemid As Integer = 0
    Dim chkPwdid As Integer = 0
    Dim Optionid As Integer = 0
    Dim pwd As String = ""
    Dim SMS_OTP_RATECHANGE_MIMR_MOBILENO As String = GetAdmindbSoftValue("SMS_OTP_RATECHANGE_MIMR_MOBILENO", "").ToString
#End Region

#Region " Constructor"
    Private Sub funO()
        Dim Sqlqry As String = "SELECT OPTIONID FROM " & cnAdminDb & "..PRJPWDOPTION WHERE OPTIONNAME ='MIMR_RATECHANGE' AND ACTIVE = 'Y'"
        Optionid = Val("" & objGPack.GetSqlValue(Sqlqry, , , Nothing))
        chkPwdid = objGPack.GetSqlValue("SELECT PWDID FROM " & cnAdminDb & "..PWDMASTER WHERE PWDOPTIONID = " & Optionid & " AND  PWDUSERID = " & userId & " AND ISNULL(PWDSTATUS,'')<>'C' ", , Nothing)
        pwd = objGPack.GetSqlValue("SELECT PASSWORD FROM " & cnAdminDb & "..PWDMASTER WHERE PWDID = " & chkPwdid & " and ISNULL(PWDSTATUS,'') <> 'C'", , Nothing)
        pwd = BrighttechPack.Methods.Decrypt(pwd)
    End Sub
    Public Sub funradiobuttonFixed(ByVal _bool As Boolean)
        If UCase(cmbTrantype) = "PURCHASE" Then
            rbtFixed.Visible = _bool
            rbtUnFixed.Visible = _bool
        End If
    End Sub
    Public Sub New()
        InitializeComponent()
        ''funO()
        If UCase(cmbTrantype) = "PURCHASE" Then
            txtNewRate_RATE.ReadOnly = False
            funradiobuttonFixed(True)
        Else
            txtNewRate_RATE.ReadOnly = True
            funradiobuttonFixed(False)
        End If
    End Sub
#End Region

#Region " User Defined Function"
    Public Sub textEvent(ByVal Rate As Boolean, ByVal passw As Boolean)
        txtNewRate_RATE.ReadOnly = Rate
        txtPassword.ReadOnly = passw
    End Sub

    Public Function RateChangeOTPMIMR() As Boolean
        'Dim pwdid As Integer = 0 : Dim pwdpass As Boolean = False
        'If Optionid = 0 Then pwdpass = False
        'If userId <> 0 Then
        '    If Optionid <> 0 Then
        '        If chkPwdid > 0 Then
        '            Closedotp()
        '            chkPwdid = 0
        '        End If
        '        If chkPwdid = 0 Then
        '            Dim shtNameTitle As String = ""
        '            If UCase(cmbTrantype) = "ISSUE" _
        '            Or UCase(cmbTrantype) = "PURCHASE RETURN" _
        '            Or UCase(cmbTrantype) = "INTERNAL TRANSFER" _
        '            Or UCase(cmbTrantype) = "APPROVAL ISSUE" _
        '            Or UCase(cmbTrantype) = "OTHER ISSUE" _
        '            Or UCase(cmbTrantype) = "PACKING" Then
        '                shtNameTitle = "MI"
        '            Else
        '                shtNameTitle = "MR"
        '            End If
        '            If SMS_OTP_RATECHANGE_MIMR_MOBILENO = "" Then MsgBox("OTP No. Should not empty", MsgBoxStyle.Information) : Exit Function
        '            pwdid = GetuserPwd(Optionid, cnCostId, userId)
        '            Dim otpnumber As String = setUserPwd(Optionid, 1, 4, "", "", "")
        '            Dim getMobile() As String = SMS_OTP_RATECHANGE_MIMR_MOBILENO.Split(",")
        '            Dim msgcontent As String = ""
        '            strsql = ""
        '            strsql = " SELECT TEMPLATE_DESC FROM " & cnAdminDb & "..SMSTEMPLATE "
        '            strsql += vbCrLf + " WHERE TEMPLATE_NAME = 'SMS_MIMR_RATECHANGE' and ACTIVE = 'Y'"
        '            msgcontent = GetSqlValue(cn, strsql)
        '            If msgcontent Is Nothing Then
        '                MsgBox("Sms Template is Empty")
        '                Exit Function
        '            End If
        '            msgcontent = msgcontent.Replace("<OTPNO>", otpnumber)
        '            msgcontent = msgcontent.Replace("<MIMR>", (UCase(shtNameTitle)))
        '            msgcontent = msgcontent.Replace("<BOARDRATE>", Format(Val(txtNewRate_RATE.Text), "0.00"))
        '            msgcontent = msgcontent.Replace("<ACNAME>", cmbAcname)
        '            msgcontent = msgcontent.Replace("<USERNAME>", cnUserName)
        '            msgcontent = msgcontent.Replace("<SYSTEMNAME>", Environment.MachineName)
        '            msgcontent = msgcontent.Replace("<DATE>", Format(Now.Date, "dd/MM/yyyy"))
        '            msgcontent = msgcontent.Replace("<TIME>", Date.Now.ToShortTimeString)
        '            msgcontent = msgcontent.Replace("<COSTNAME>", cnCostName)
        '            For i As Integer = 0 To getMobile.Length - 1
        '                SmsSend(msgcontent, getMobile(i))
        '            Next
        '            textEvent(True, False)
        '            btnSend.Enabled = False
        '            'MsgBox("OTP send successfully")
        '            funO()
        '            txtPassword.Focus()
        '            txtPassword.SelectAll()
        '        End If
        '    End If
        'End If
    End Function
#End Region

#Region " Form Load"
    Private Sub MaterialIssRecRateChangeBox_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        txtNewRateInclusive_RATE.Focus()
        txtNewRateInclusive_RATE.SelectAll()
    End Sub

    Private Sub MaterialIssRecRateChangeBox_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ''funO()
        txtNewRateInclusive_RATE.Focus()
        txtNewRateInclusive_RATE.SelectAll()
        strsql = ""
        strsql = ""
    End Sub
    Private Sub MaterialIssRecRateChangeBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{tab}")
        ElseIf e.KeyCode = Keys.Escape Then
            btnCancel_Click(Me, New System.EventArgs)
        End If
    End Sub
#End Region

#Region " Textbox Events"
    Private Sub txtNewRate_AMT_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNewRate_RATE.KeyDown
        If e.KeyCode = Keys.Enter Then
            'If Val(txtNewRate_AMT.Text) > 0 Then
            'End If
        End If
    End Sub
#End Region

#Region " User Define Function"
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If Val(txtNewRate_RATE.Text) > 0 Then
            If chkPwdid > 0 Then
                textEvent(True, False)
                If txtPassword.Text = pwd Then
                    strsql = " UPDATE " & cnAdminDb & "..PWDMASTER SET PWDSTATUS='C' "
                    strsql += " ,PWDCLSDATE= '" + GetServerDate() + "',PWDCLSTIME='" & GetServerTime() + "'"
                    strsql += " WHERE PWDID=" & chkPwdid
                    ExecQuery(SyncMode.Transaction, strsql, cn, tran, cnCostId)
                    DialogResult = Windows.Forms.DialogResult.OK
                    Me.Close()
                Else
                    MsgBox("Invalid Password", MsgBoxStyle.Information)
                    txtPassword.Focus()
                    txtPassword.SelectAll()
                End If
            End If
        End If
    End Sub

    Private Sub btnSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSend.Click
        If Val(txtNewRate_RATE.Text) > 0 Then
            If Val(txtNewRate_RATE.Text) > Val(txtNewRateInclusive_RATE.Text) Then
                MsgBox("Exceed rate not allowed", MsgBoxStyle.Information)
                txtNewRateInclusive_RATE.Focus()
                txtNewRateInclusive_RATE.SelectAll()
                Exit Sub
            End If
            DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
            'RateChangeOTPMIMR()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
#End Region

#Region " Label Event"
    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblResend.LinkClicked
        If MsgBox("Do You Want Resend OTP", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Closedotp()
            Me.Close()
            textEvent(False, True)
        End If
    End Sub
    Private Sub Closedotp()
        strsql = " UPDATE " & cnAdminDb & "..PWDMASTER SET PWDSTATUS='C' "
        strsql += " ,PWDCLSDATE= '" + GetServerDate() + "' "
        strsql += " ,PWDCLSTIME='" & GetServerTime() + "'"
        strsql += " WHERE PWDID=" & chkPwdid
        ExecQuery(SyncMode.Transaction, strsql, cn, tran, cnCostId)
    End Sub

    Private Sub btnSend_GotFocus(sender As Object, e As EventArgs) Handles btnSend.GotFocus
        If Val(txtNewRate_RATE.Text) = 0 Then
            txtNewRate_RATE.Focus()
        End If
    End Sub

    Private Function RateChange(ByVal Trantype As String) As Double
        Dim Rate As Double = 0
        If Trantype = "PURCHASE" Then
            Rate = Math.Round((Val(txtNewRateInclusive_RATE.Text) / (100 + (Val(1.5) + Val(1.5) + 0))) * 100, 4)
        Else
            Rate = Math.Round((Val(txtNewRateInclusive_RATE.Text)), 4)
        End If
        Return Rate
    End Function

    Private Sub txtNewRateInclusive_AMT_KeyDown(sender As Object, e As KeyEventArgs) Handles txtNewRateInclusive_RATE.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtNewRate_RATE.Text = Format(RateChange(UCase(cmbTrantype)), "0.0000")
        End If
    End Sub

    Private Sub txtNewRateInclusive_AMT_Leave(sender As Object, e As EventArgs) Handles txtNewRateInclusive_RATE.Leave
        txtNewRate_RATE.Text = Format(RateChange(UCase(cmbTrantype)), "0.0000")
        If Val(txtNewRate_RATE.Text) = 0 Then
            txtNewRate_RATE.Text = ""
            txtNewRateInclusive_RATE.Focus()
            txtNewRateInclusive_RATE.SelectAll()
        End If
    End Sub

    Private Sub txtNewRateInclusive_AMT_TextChanged(sender As Object, e As EventArgs) Handles txtNewRateInclusive_RATE.TextChanged
        txtNewRate_RATE.Text = Format(RateChange(UCase(cmbTrantype)), "0.0000")
        If Val(txtNewRate_RATE.Text) = 0 Then
            txtNewRate_RATE.Text = ""
        End If
    End Sub

    Private Sub txtNewRate_AMT_GotFocus(sender As Object, e As EventArgs) Handles txtNewRate_RATE.GotFocus
        'txtNewRateInclusive_AMT.Focus()
        'txtNewRateInclusive_AMT.SelectAll()
    End Sub

    Private Sub txtNewRateInclusive_AMT_GotFocus(sender As Object, e As EventArgs) Handles txtNewRateInclusive_RATE.GotFocus
        'txtNewRateInclusive_AMT.SelectAll()
    End Sub

    Private Sub txtNewRate_AMT_Leave(sender As Object, e As EventArgs) Handles txtNewRate_RATE.Leave
        If UCase(cmbTrantype) = "PURCHASE" Then
            If Val(txtNewRateInclusive_RATE.Text) > Val(txtNewRate_RATE.Text) Then
                'MsgBox("Exceed rate not allowed", MsgBoxStyle.Information)
                'txtNewRate_AMT.Text = Format(RateChange(UCase(cmbTrantype)), "0.0000")
            End If
        End If

    End Sub
#End Region


End Class