Imports System.Data.SqlClient
Imports System.IO
Imports System.Security.Principal
Imports System.Security.Permissions
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient

Public Class frmPortal
    Dim strsql As String = ""
    Dim c_cn As New SqlConnection
    Dim cmd As New SqlCommand
    Public CP_strKey As String = ""
    Public CP_COSTID As String = ""
    Public CP_COMPID As String = ""
    Public CP_COMPNAME As String = ""

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If txtCompid.Text = "" Then txtCompid.Focus() : Exit Sub
        If txtCompName.Text = "" Then txtCompName.Focus() : Exit Sub
        If txtMobile_NUM_MAN.Text = "" Then txtMobile_NUM_MAN.Focus() : Exit Sub
        If txtEmail.Text = "" Then txtEmail.Focus() : Exit Sub
        Try
            If CP_COMPID.ToString = "" Then
                CP_COMPID = BrighttechPack.Methods.Encrypt(txtCompid.Text.ToString)
            Else
                CP_COMPID = BrighttechPack.Methods.Encrypt(CP_COMPID)
            End If
            If CP_COMPNAME.ToString = "" Then
                CP_COMPNAME = BrighttechPack.Methods.Encrypt(txtCompName.Text.ToString)
            Else
                CP_COMPNAME = BrighttechPack.Methods.Encrypt(CP_COMPNAME)
            End If
            If CP_COSTID.ToString = "" Then
                CP_COSTID = BrighttechPack.Methods.Encrypt(cnCostId.ToString)
            Else
                CP_COSTID = BrighttechPack.Methods.Encrypt(CP_COSTID)
            End If

            If CP_COSTID = Nothing Then
                CP_COSTID = ""
            End If
            strsql = "SELECT 1 FROM PRJMASTER WHERE ID='" & CP_COMPID & "' AND NAME='" & CP_COMPNAME & "' AND COSTID='" & CP_COSTID.ToString & "'"
            Dim Da As SqlDataAdapter
            Dim DtTemp As New DataTable
            Da = New SqlDataAdapter(strsql, c_cn)
            Da.Fill(DtTemp)
            If DtTemp.Rows.Count > 0 Then
                MsgBox("Already Registered.")
                Exit Sub
            Else
                'strsql = "DECLARE @NO INT"
                'strsql += vbCrLf + "SET @NO = (SELECT CAST((RAND()*1000000) AS DECIMAL(6)))"
                'strsql += vbCrLf + "IF LEN(@NO) = 6"
                'strsql += vbCrLf + "SELECT @NO"
                'strsql += vbCrLf + "ELSE"
                'strsql += vbCrLf + "SET @NO = CONVERT(VARCHAR,(SELECT CAST((RAND()*10) AS DECIMAL(1))))+CONVERT(VARCHAR,@NO)"

                ''strsql = "SELECT "
                ''strsql += vbCrLf + "CONVERT(VARCHAR,ASCII('" & cnCompanyId.Substring(0, 1) & "')) + CONVERT(VARCHAR,ASCII('" & cnCompanyId.Substring(0, 1) & "')+18) "
                ''strsql += vbCrLf + "+ CONVERT(VARCHAR,ASCII('" & cnCompanyId.Substring(1, 1) & "')) + CONVERT(VARCHAR,ASCII('" & cnCompanyId.Substring(1, 1) & "')+18)"
                ''strsql += vbCrLf + "+ CONVERT(VARCHAR,ASCII('" & cnCompanyId.Substring(2, 1) & "')) + CONVERT(VARCHAR,ASCII('" & cnCompanyId.Substring(2, 1) & "')+18)"
                ''Dim strKey As String = BrighttechPack.Methods.Encrypt(objGPack.GetSqlValue(strsql, , ""))
                Dim strKey As String = BrighttechPack.Methods.Encrypt(CP_strKey.ToString)
                Dim strId As String = BrighttechPack.Methods.Encrypt(txtCompid.Text)
                Dim strName As String = BrighttechPack.Methods.Encrypt(txtCompName.Text)
                Dim strMobile As String = BrighttechPack.Methods.Encrypt(txtMobile_NUM_MAN.Text)
                Dim strEmail As String = BrighttechPack.Methods.Encrypt(txtEmail.Text)
                Dim strBranch As String = BrighttechPack.Methods.Encrypt(cnCostId.ToString)
                strsql = "INSERT INTO PRJMASTER(ID,NAME,MOBILE,EMAIL,ACCESSKEY,COSTID)"
                strsql += vbCrLf + "VALUES ('" & strId & "','" & strName & "','" & strMobile & "'"
                strsql += vbCrLf + ",'" & strEmail & "','" & strKey & "','" & strBranch & "')"
                cmd = New SqlCommand(strsql, c_cn)
                cmd.ExecuteNonQuery()
                MsgBox("Registered.")
                txtCompid.Text = ""
                txtCompName.Text = ""
                txtMobile_NUM_MAN.Text = ""
                txtEmail.Text = ""
                Me.Close()
                Exit Sub
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub frmSign_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtCompid.Focused And Len(txtCompid.Text) <> 3 Then Exit Sub
            If txtCompName.Focused And Len(txtCompName.Text) = 0 Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSign_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If txtCompid.Text = "" Then
            txtCompid.Text = cnCompanyId.ToString
            strsql = "SELECT TOP 1 COMPANYNAME FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID='" & cnCompanyId & "'"
            txtCompName.Text = objGPack.GetSqlValue(strsql).ToString
        End If
        txtMobile_NUM_MAN.Focus()
        c_cn = New SqlConnection("Data Source=103.133.215.2,1433;Initial Catalog=giritech1;User ID=giri1;Password=Giritech@123;")
        Try
            c_cn.Open()
        Catch ex As Exception
            MsgBox("Connection Problem" + vbCrLf + ex.Message)
            'Application.Exit()
            Exit Sub
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub txtCurrency_NUM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMobile_NUM_MAN.KeyPress
        Select Case e.KeyChar
            Case "+", "-", "0" To "9", ChrW(Keys.Back),
                ChrW(Keys.Enter), ChrW(Keys.Escape)
            Case Else
                e.Handled = True
                MsgBox("Digits only Allowed 0 to 9", MsgBoxStyle.Information)
                sender.Focus()
        End Select
    End Sub

    Public Function CustomerDetail_Portal(ByVal CompId As String, ByVal CompName As String, ByVal CostId As String) As DataTable
        c_cn = New SqlConnection("Data Source=103.133.215.2,1433;Initial Catalog=giritech1;User ID=giri1;Password=Giritech@123;")
        c_cn.Open()
        strsql = "SELECT MOBILE,EMAIL,ACCESSKEY,ISNULL(ACTIVE,'')ACTIVE FROM PRJMASTER WHERE ID='" & CompId & "' AND NAME='" & CompName & "' AND COSTID='" & CostId & "'"
        Dim Da As SqlDataAdapter
        Dim DtTemp As New DataTable
        Da = New SqlDataAdapter(strsql, c_cn)
        Da.Fill(DtTemp)
        Return DtTemp
    End Function

    Public Function Sendmail_Portal(ByVal strKey As String, ByVal MailId As String, ByVal Mobile As String) As Integer

        Dim Message As String = ""
        Dim smtpServer As New SmtpClient()
        Dim mail As New MailMessage
        Dim Counter As Integer = 0
        Dim FromId As String = Nothing
        Dim MailServer As String = Nothing
        Dim Password As String = Nothing
        If objGPack Is Nothing Then
            objGPack = New BrighttechPack.Methods(cn)
        End If
        Dim costcentre As String = objGPack.GetSqlValue("SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & cnCostId.ToString & "'")
        strsql = "SELECT TOP 1 COMPANYNAME FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID='" & cnCompanyId & "'"
        Dim CP_Company As String = objGPack.GetSqlValue(strsql).ToString

        Try
            'txtCompid.Text = cnCompanyId.ToString
            'strsql = "SELECT TOP 1 COMPANYNAME FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID='" & cnCompanyId & "'"
            'txtCompName.Text = objGPack.GetSqlValue(strsql).ToString
            'txtCompid.Focus()
            'c_cn = New SqlConnection("Data Source=103.133.215.2,1433;Initial Catalog=giritech1;User ID=giri1;Password=abcdef;")
            'c_cn.Open()
            'strsql = "SELECT MOBILE,EMAIL FROM PRJMASTER WHERE ID='" & txtCompid.Text.ToString & "' AND NAME='" & txtCompName.Text.ToString & "' AND COSTID='" & cnCostId.ToString & "'"
            'Dim Da As SqlDataAdapter
            'Dim DtTemp As New DataTable
            'Da = New SqlDataAdapter(strsql, c_cn)
            'Da.Fill(DtTemp)
            'If DtTemp.Rows.Count > 0 Then
            '    Return 1
            'Else
            '    Return 0
            'End If

            Message = strKey & " is your Access code for Database creation. [" & cnCompanyId.ToString & "-" & CP_Company.ToString & "-" & costcentre & "]"
            Dim strUrl As String = "http://alerts.sinfini.com/web2sms.php?username=giritech&password=akshaya@123&to=" & Mobile & "&sender=GIRITC&message=" & Message
            funcSMSSend(strUrl)
            strUrl = "http://alerts.sinfini.com/web2sms.php?username=giritech&password=akshaya@123&to=9600018255&sender=GIRITC&message=" & Message
            funcSMSSend(strUrl)

            FromId = "care.akshaya@gmail.com"
            Password = "desknote"
            'Password = BrighttechPack.Methods.Decrypt(Password)
            Dim MailServer1 As String = Nothing
            Dim MailServer2 As String = Nothing
            If FromId.Contains("@") = True Then
                Dim SplitMailServer() As String = Split(FromId, "@")
                If Not SplitMailServer Is Nothing Then
                    MailServer1 = SplitMailServer(0)
                    MailServer2 = Trim(SplitMailServer(1))
                    MailServer2 = "@" & MailServer2
                End If
            End If
            If Trim(MailServer2) = "@gmail.com" Then
                smtpServer.Host = "smtp.gmail.com"
                smtpServer.Port = 587
                smtpServer.EnableSsl = True
            ElseIf Trim(MailServer2) = "@yahoo.com" Then
                smtpServer.Port = 465
                smtpServer.Host = "smtp.mail.yahoo.com"
                smtpServer.EnableSsl = True
            ElseIf Trim(MailServer2) = "@yahoo.co.in" Then
                smtpServer.Port = 587
                smtpServer.Host = "smtp.mail.yahoo.com"
            ElseIf Trim(MailServer2) = "@hotmail.com" Then
                smtpServer.Port = 587
                smtpServer.Host = "smtp.live.com"
                smtpServer.EnableSsl = True
            Else
                smtpServer.Port = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-PORT'")
                smtpServer.Host = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-HOSTNAME'")
                smtpServer.EnableSsl = IIf(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-SSL'").ToString.ToUpper() = "Y", True, False)
            End If
            mail.From = New MailAddress(FromId)
            mail.To.Add(New MailAddress(MailId))
            mail.Subject = "AKSHAYA ACCESS CODE"
            mail.Body = Message
            mail.IsBodyHtml = True
            'If File.Exists(Attachpath) = True Then mail.Attachments.Add(New Attachment(Attachpath))
            smtpServer.Credentials = New Net.NetworkCredential(FromId.Trim.ToString, Password.Trim.ToString)
            smtpServer.Timeout = 400000
            smtpServer.Send(mail)
        Catch ex As Exception
            Return 0
        End Try
        Return 1

    End Function

    Private Function funcSMSSend(ByVal URL As String)
        Dim req As System.Net.WebRequest = System.Net.WebRequest.Create(URL)
        Dim resp As System.Net.WebResponse = req.GetResponse()
        Dim s As System.IO.Stream = resp.GetResponseStream()
        Dim sr As System.IO.StreamReader = New System.IO.StreamReader(s, System.Text.Encoding.ASCII)
        Dim info As String = sr.ReadToEnd()
    End Function

End Class