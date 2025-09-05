Imports System.Data.OleDb
Imports System.IO
Imports System.Text
Imports System.Net.Mail
Imports System.Globalization
Imports System.Net
Imports System.Web
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Web.UI.MobileControls.Adapters

Public Class frmAlertMail
    Public Cn As New OleDbConnection
    Public CnMast As New OleDbConnection
    Public Strsql As String
    Dim Cmd As New OleDbCommand
    Dim da As New OleDbDataAdapter
    Dim dt As New DataTable
    Dim MsgTitle As String = "AlertMail"
    Dim DbType, PassWord, dbPath, serverName As String
    Dim notify As NotifyIcon
    Dim ToMailId As String = ""
    Dim ToMobileNo As String = ""
    Dim Message As String = ""
    Dim flagSent As Boolean = False
    Private t2 As Threading.Thread = Nothing
    Dim tspan As TimeSpan
    Dim StartTime As Date
    Dim NextTime As Date
    Private _AlertSch As Integer = 30
    Dim SMSURL As String = ""
    Dim FromId As String = Nothing
    Dim MailServer As String = Nothing
    Dim MailPassword As String = Nothing
    Dim MailTag As String = Nothing
    Dim SmtpHostname As String = ""
    Dim SmtpPort As Long
    Dim SmtpSSL As Boolean = False
    Dim filePath As String
    Dim SmsWay As String
    Dim SmsData As Boolean = False
    Dim MailData As Boolean = False
    Dim OptinUrl As String = ""
    Dim WhatsappUrl As String = ""
    Dim WhatsappOptinUrl As String = ""

    Private Sub funcSendMail()
        If Cn.State = ConnectionState.Closed Then Cn.Open()
        Strsql = "SELECT * FROM MAILDATA WHERE (STATUS='N' OR ISNULL(STATUS,'')='') AND  (ISNULL(EXPIRYDATE,'')='' OR EXPIRYDATE >='" & Format(Today.Date, "yyyy-MM-dd") & "')  "
        Strsql += " AND ISNULL(MESSAGES,'')<>'' AND ISNULL(MAILID,'')<>''"
        Strsql += " ORDER BY SERNO"
        da = New OleDbDataAdapter(Strsql, Cn)
        dt = New DataTable
        da.Fill(dt)
        For I As Integer = 0 To dt.Rows.Count - 1
            Dim Id As Integer
            Dim Path As String = ""
            Id = dt.Rows(I).Item("SERNO")
            ToMailId = dt.Rows(I).Item("MAILID").ToString
            Message = dt.Rows(I).Item("MESSAGES").ToString
            MailTag = dt.Rows(I).Item("SUBJECT").ToString
            Path = dt.Rows(I).Item("FILEPATH").ToString
            If ToMailId = "" Or Message = "" Then Continue For
            If NEWMAILSEND(ToMailId, MailTag, Message, Path) = False Then Exit Sub
            If Cn.State = ConnectionState.Closed Then Cn.Open()
            Strsql = "UPDATE MAILDATA "
            Strsql += vbCrLf + "SET STATUS='Y'"
            Strsql += vbCrLf + ",SENTUPDATED='" & Format(Date.Parse(Date.Now), "yyyy-MM-dd HH:mm:ss") & "' "
            Strsql += vbCrLf + " WHERE SERNO = " & Id
            Cmd = New OleDbCommand(Strsql, Cn)
            Cmd.ExecuteNonQuery()
            LstViewAdd(Id, ToMailId, "Sent")
            LstView.Visible = True
            LstView.Refresh()
        Next
    End Sub
    Private Sub funcSendSms()
        If Cn.State = ConnectionState.Closed Then Cn.Open()
        Strsql = "SELECT * FROM SMSDATA WHERE (STATUS='N' OR ISNULL(STATUS,'')='') AND (ISNULL(EXPIRYDATE,'')='' OR EXPIRYDATE >='" & Format(Today.Date, "yyyy-MM-dd") & "') "
        Strsql += " AND ISNULL(MESSAGES,'')<>'' AND ISNULL(MOBILENO,'')<>''"
        Strsql += " AND STATUS NOT IN('E','I')"
        Strsql += " ORDER BY SERNO"
        da = New OleDbDataAdapter(Strsql, Cn)
        dt = New DataTable
        da.Fill(dt)
        For I As Integer = 0 To dt.Rows.Count - 1
            Dim Id As Integer
            Id = Val(dt.Rows(I).Item("SERNO").ToString)
            ToMobileNo = dt.Rows(I).Item("MOBILENO").ToString
            Message = dt.Rows(I).Item("MESSAGES").ToString
            If dt.Rows(I).Item("SMSTYPE").ToString = "W" Then
                If WhatsappSend(ToMobileNo, Message, Id) = False Then Exit Sub
            Else
                If NEWSMSSEND(ToMobileNo, Message, Id) = False Then Exit Sub
            End If

            If Cn.State = ConnectionState.Closed Then Cn.Open()
            Strsql = "UPDATE SMSDATA "
            Strsql += vbCrLf + "SET STATUS='Y'"
            Strsql += vbCrLf + ",SENTUPDATED='" & Format(Date.Parse(Date.Now), "yyyy-MM-dd HH:mm:ss") & "' "
            Strsql += vbCrLf + " WHERE SERNO = " & Id
            Cmd = New OleDbCommand(Strsql, Cn)
            Cmd.ExecuteNonQuery()
            LstViewAdd(Id, ToMobileNo, "Sent")
            LstView.Visible = True
            LstView.Refresh()
        Next
    End Sub
    Private Sub funcScheduleMsg()
        If SmsData Then funcSendSms()
        If MailData Then funcSendMail()
    End Sub
    Private Sub frmAutoMail_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IO.File.Exists(Application.StartupPath + "\SmsServer.ini") = False Then
            'Dim frm As New frmSign()
            frmSign.ShowDialog()
        End If
        If IO.File.Exists(Application.StartupPath + "\SmsServer.ini") = False Then End
        NotifyIcon1.Text = "Alert Management"
        NotifyIcon1.BalloonTipText = "Alert Management"
        Dim file As New FileStream(Application.StartupPath + "\SmsServer.ini", FileMode.Open)
        Dim fstream As New StreamReader(file)
        fstream.BaseStream.Seek(0, SeekOrigin.Begin)
        serverName = Mid(fstream.ReadLine, 21)
        DbType = UCase(Mid(fstream.ReadLine, 21))
        PassWord = Mid(fstream.ReadLine, 21).Trim
        If Trim(PassWord) <> "" Then
            PassWord = Decrypt(PassWord)
        End If
        dbPath = Mid(fstream.ReadLine, 21)
        SmsWay = Mid(fstream.ReadLine, 21)
        SMSURL = Mid(fstream.ReadLine, 21)
        FromId = Mid(fstream.ReadLine, 21)
        MailPassword = Mid(fstream.ReadLine, 21)
        If Trim(MailPassword) <> "" Then
            MailPassword = Decrypt(MailPassword)
        End If
        SmtpHostname = Mid(fstream.ReadLine, 21)
        SmtpPort = Mid(fstream.ReadLine, 21)
        SmtpSSL = IIf(Mid(fstream.ReadLine, 21).ToString.ToUpper = "Y", True, False)
        OptinUrl = Mid(fstream.ReadLine, 21)
        WhatsappUrl = Mid(fstream.ReadLine, 21)
        WhatsappOptinUrl = Mid(fstream.ReadLine, 21)
        fstream.Close()
        Dim dbSql As String
        If DbType = "W" Then
            Strsql = String.Format("Provider=SQLOLEDB.1;Integrated Security =SSPI;Persist Security Info=False;Initial Catalog=AkshayaSmsDb;Data Source={0}", serverName)
            dbSql = String.Format("Provider=SQLOLEDB.1;Integrated Security =SSPI;Persist Security Info=False;Initial Catalog=MASTER;Data Source={0}", serverName)
        ElseIf DbType = "S" Then
            Strsql = String.Format("Provider=SQLOLEDB.1;Persist Security Info=False;user id = sa;pwd=" & PassWord & ";Initial Catalog=AkshayaSmsDb;Data Source={0}", serverName)
            dbSql = String.Format("Provider=SQLOLEDB.1;Persist Security Info=False;user id = sa;pwd=" & PassWord & ";Initial Catalog=MASTER;Data Source={0}", serverName)
        ElseIf DbType <> "" Then
            Strsql = String.Format("Provider=SQLOLEDB.1;Persist Security Info=False;user id = " & DbType & ";pwd=" & PassWord & ";Initial Catalog=AkshayaSmsDb;Data Source={0}", serverName)
            dbSql = String.Format("Provider=SQLOLEDB.1;Persist Security Info=False;user id = " & DbType & ";pwd=" & PassWord & ";Initial Catalog=MASTER;Data Source={0}", serverName)
        Else
            Strsql = String.Format("Provider=SQLOLEDB.1;Persist Security Info=False;user id = sa;pwd=" & PassWord & ";Initial Catalog=AkshayaSmsDb;Data Source={0}", serverName)
            dbSql = String.Format("Provider=SQLOLEDB.1;Persist Security Info=False;user id = sa;pwd=" & PassWord & ";Initial Catalog=MASTER;Data Source={0}", serverName)
        End If
        CnMast = New OleDbConnection(dbSql)
        Cn = New OleDbConnection(Strsql)
        If CnMast.State = ConnectionState.Closed Then CnMast.Open()
        If DbChecker("AKSHAYASMSDB", CnMast) = 0 Then
            NotifyIcon1.Visible = True
            NotifyIcon1.Icon = My.Resources.email
            NotifyIcon1.ShowBalloonTip(5000, "Information", "AkshayaSmsDb Not Found", ToolTipIcon.Info)
            Exit Sub
        End If
        If CnMast.State = ConnectionState.Open Then CnMast.Close()
        If Cn.State = ConnectionState.Closed Then Cn.Open()
        If TblChecker("SMSDATA") = 1 Then SmsData = True
        If TblChecker("MAILDATA") = 1 Then MailData = True
        If MailData Then
            Strsql = "SELECT COUNT(*)CNT FROM AKSHAYASMSDB..SYSCOLUMNS WHERE NAME='FILEPATH' AND ID=OBJECT_ID('AKSHAYASMSDB..MAILDATA')"
            If GetSqlValue(Strsql, "CNT", 0) = 0 Then
                Strsql = "ALTER TABLE AKSHAYASMSDB..MAILDATA ADD FILEPATH VARCHAR(100)"
                Cmd = New OleDbCommand(Strsql, Cn)
                Cmd.ExecuteNonQuery()
            End If
        End If
        Strsql = "SELECT COUNT(*)CNT FROM AKSHAYASMSDB..SYSCOLUMNS WHERE NAME='SMSTYPE' AND ID=OBJECT_ID('AKSHAYASMSDB..SMSDATA')"
        If GetSqlValue(Strsql, "CNT", 0) = 0 Then
            Strsql = "ALTER TABLE AKSHAYASMSDB..SMSDATA ADD SMSTYPE VARCHAR(1)"
            Cmd = New OleDbCommand(Strsql, Cn)
            Cmd.ExecuteNonQuery()
        End If
        LstView.Clear()
        LstView.Columns.Add("ID", 50, HorizontalAlignment.Left)
        LstView.Columns.Add("TOID", 220, HorizontalAlignment.Left)
        LstView.Columns.Add("STATUS", 80, HorizontalAlignment.Left)
        Dim myBuildInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(Application.ExecutablePath)
        Dim Version As String = myBuildInfo.FileVersion
        Me.Text = "Alert Sms/Mail " & Space(10) & "v:" & Version
    End Sub
    Public Function GetSqlValue(ByVal qry As String, Optional ByVal field As String = Nothing, Optional ByVal defValue As String = "", Optional ByRef tran As OleDbTransaction = Nothing) As String
        Dim dt As New DataTable
        Dim da As New OleDbDataAdapter
        If tran Is Nothing Then
            da = New OleDbDataAdapter(qry, Cn)
            da.Fill(dt)
        Else
            Cmd = New OleDbCommand(qry, Cn, tran)
            da = New OleDbDataAdapter(Cmd)
            da.Fill(dt)
        End If
        If field <> "" Then
            If dt.Rows.Count > 0 Then Return dt.Rows(0).Item(field).ToString
        Else
            If dt.Rows.Count > 0 Then Return dt.Rows(0).Item(0).ToString
        End If
        Return defValue
    End Function
    Function DbChecker(ByVal dbname As String, ByVal Cn As OleDbConnection) As Integer
        Dim ds As New Data.DataSet
        ds.Clear()
        Strsql = " SELECT NAME FROM MASTER..SYSDATABASES WHERE NAME = '" & dbname & "'"
        Cmd = New OleDbCommand(Strsql, Cn) : Cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(Cmd)
        da.Fill(ds, "SYSDATABASES")
        If ds.Tables("SYSDATABASES").Rows.Count > 0 Then
            Return 1
        End If
        Return 0
    End Function
    Function TblChecker(ByVal TblName As String) As Integer
        Dim ds As New Data.DataSet
        ds.Clear()
        Strsql = " SELECT NAME FROM AKSHAYASMSDB..SYSOBJECTS WHERE NAME = '" & TblName & "'"
        Cmd = New OleDbCommand(Strsql, Cn) : Cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(Cmd)
        da.Fill(ds, "SYSOBJECTS")
        If ds.Tables("SYSOBJECTS").Rows.Count > 0 Then
            Return 1
        End If
        Return 0
    End Function
    Function LstViewAdd(ByVal name As String, ByVal desc As String, ByVal desc1 As String)
        Dim node As ListViewItem
        node = New ListViewItem(name)
        node.SubItems.Add(desc)
        node.SubItems.Add(desc1)
        LstView.Items.Add(node)
    End Function
    Public Shared Function Decrypt(ByVal Str_Pwd As String) As String
        Dim vv As String = ""
        Dim x As Integer = 0
        For k As Integer = 0 To Str_Pwd.Length - 1
            Dim vIn As Char = Str_Pwd.Substring(x, 1)
            Dim vOut As String = vIn.ToString(vIn)
            Dim Byte_Data As Byte   ' For Reading Data
            Byte_Data = Asc(vOut)
            Byte_Data = Byte_Data - 23
            Byte_Data = Not Byte_Data
            vv += Chr(Byte_Data)
            x += 1
        Next
        Return vv
    End Function
    Public Function CALLURL(ByVal address As String, ByVal Id As Integer) As Boolean
        Try
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls Or SecurityProtocolType.Ssl3 Or SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11
            Dim req As System.Net.WebRequest = System.Net.WebRequest.Create(address)
            'req.AuthenticationLevel = SecurityProtocolType.Tls Or SecurityProtocolType.Ssl3 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls12
            Dim resp As System.Net.WebResponse = req.GetResponse()
            Dim s As System.IO.Stream = resp.GetResponseStream()
            Dim sr As System.IO.StreamReader = New System.IO.StreamReader(s, System.Text.Encoding.ASCII)
            Dim info As String = sr.ReadToEnd()
            If info.Contains("Insufficient credits") Or info.Contains("Insufficient") Or info.Contains("Invalid") Then
                Strsql = "UPDATE SMSDATA "
                Strsql += vbCrLf + "SET STATUS='I'"
                Strsql += vbCrLf + ",SENTUPDATED='" & Format(Date.Parse(Date.Now), "yyyy-MM-dd HH:mm:ss") & "' "
                Strsql += vbCrLf + " WHERE SERNO = " & Id
                Cmd = New OleDbCommand(Strsql, Cn)
                Cmd.ExecuteNonQuery()
                If info.Contains("Invalid") Then
                    LstViewAdd(Id, ToMobileNo, "Invalid Template")
                Else
                    LstViewAdd(Id, ToMobileNo, "Insufficient credits")
                End If
                LstView.Visible = True
                LstView.Refresh()
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            NotifyIcon1.Visible = True
            NotifyIcon1.Icon = My.Resources.email
            NotifyIcon1.ShowBalloonTip(5000, "Information", ex.Message, ToolTipIcon.Info)
            Strsql = "UPDATE SMSDATA "
            Strsql += vbCrLf + "SET STATUS='E'"
            Strsql += vbCrLf + ",SENTUPDATED='" & Format(Date.Parse(Date.Now), "yyyy-MM-dd HH:mm:ss") & "' "
            Strsql += vbCrLf + " WHERE SERNO = " & Id
            Cmd = New OleDbCommand(Strsql, Cn)
            Cmd.ExecuteNonQuery()
            LstViewAdd(Id, ToMobileNo, "Exception Caught")
            LstView.Visible = True
            LstView.Refresh()
            Return False
        End Try
    End Function


    Public Function Callurl_New(ByVal address As String, ByVal Id As Integer) As Boolean

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls12 Or CType(3072, SecurityProtocolType)
        ServicePointManager.DefaultConnectionLimit = 4
        System.Net.ServicePointManager.Expect100Continue = False
        Dim wrGETURL As WebRequest
        wrGETURL = WebRequest.Create(address)
        Try
            Dim objStream As Stream
            objStream = wrGETURL.GetResponse().GetResponseStream()
            Dim srr As System.IO.StreamReader = New System.IO.StreamReader(objStream, System.Text.Encoding.ASCII)
            Dim info As String = srr.ReadToEnd()
            If info.Contains("Insufficient credits") Or info.Contains("Insufficient") Or info.Contains("Invalid") Then
                Strsql = "UPDATE SMSDATA "
                Strsql += vbCrLf + "SET STATUS='I'"
                Strsql += vbCrLf + ",SENTUPDATED='" & Format(Date.Parse(Date.Now), "yyyy-MM-dd HH:mm:ss") & "' "
                Strsql += vbCrLf + " WHERE SERNO = " & Id
                Cmd = New OleDbCommand(Strsql, Cn)
                Cmd.ExecuteNonQuery()
                If info.Contains("Invalid") Then
                    LstViewAdd(Id, ToMobileNo, "Invalid Template")
                Else
                    LstViewAdd(Id, ToMobileNo, "Insufficient credits")
                End If
                LstView.Visible = True
                LstView.Refresh()
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            NotifyIcon1.Visible = True
            NotifyIcon1.Icon = My.Resources.email
            NotifyIcon1.ShowBalloonTip(5000, "Information", ex.Message, ToolTipIcon.Info)
            Strsql = "UPDATE SMSDATA "
            Strsql += vbCrLf + "SET STATUS='E'"
            Strsql += vbCrLf + ",SENTUPDATED='" & Format(Date.Parse(Date.Now), "yyyy-MM-dd HH:mm:ss") & "' "
            Strsql += vbCrLf + " WHERE SERNO = " & Id
            Cmd = New OleDbCommand(Strsql, Cn)
            Cmd.ExecuteNonQuery()
            LstViewAdd(Id, ToMobileNo, "Exception Caught")
            LstView.Visible = True
            LstView.Refresh()
            Return False
        End Try
    End Function

    Function NEWSMSSEND(ByVal ToMobileNo As String, ByVal MESSAGE As String, ByVal Id As Integer) As Boolean
        Try
            Dim Url As String = ""
            Dim OpUrl As String = ""
            If SMSURL = "" Then
                NotifyIcon1.Visible = True
                NotifyIcon1.Icon = My.Resources.email
                NotifyIcon1.ShowBalloonTip(2000, "Information", "Please Check Sms Url", ToolTipIcon.Info)
                Return False
            End If
            If Not OptinUrl Is Nothing Then
                If OptinUrl.Trim <> "" Then
                    OpUrl = Replace(OptinUrl, "<SMSTO>", ToMobileNo)
                    OpUrl = Replace(OpUrl, "<DATEFORMAT>", Format(Date.Now, "MM-dd-yyyy") & "%2006:08:34")
                    Callurl_New(OpUrl, Id)
                End If
            End If
            Url = Replace(SMSURL, "<SMSTO>", ToMobileNo)
            Url = Replace(Url, "<SMSMSG>", MESSAGE)
            If Callurl_New(Url, Id) = False Then Return False
            Return True
        Catch ex As Exception
            NotifyIcon1.Visible = True
            NotifyIcon1.Icon = My.Resources.email
            NotifyIcon1.ShowBalloonTip(5000, "Information", ex.Message, ToolTipIcon.Info)
            Return False
        End Try
    End Function


    Function WhatsappSend(ByVal ToMobileNo As String, ByVal MESSAGE As String, ByVal Id As Integer) As Boolean
        Try
            Dim Url As String = ""
            Dim OpUrl As String = ""
            If WhatsappUrl = "" Then
                NotifyIcon1.Visible = True
                NotifyIcon1.Icon = My.Resources.email
                NotifyIcon1.ShowBalloonTip(2000, "Information", "Please Check Whatsapp Url", ToolTipIcon.Info)
                Return False
            End If
            If Not WhatsappOptinUrl Is Nothing Then
                If WhatsappOptinUrl.Trim <> "" Then
                    OpUrl = Replace(WhatsappOptinUrl, "<SMSTO>", ToMobileNo)
                    OpUrl = Replace(OpUrl, "<DATEFORMAT>", Format(Date.Now, "MM-dd-yyyy") & "%2006:08:34")
                    CALLURL(OpUrl, Id)
                End If
            End If
            Url = Replace(WhatsappUrl, "<SMSTO>", ToMobileNo)
            Url = Replace(Url, "<SMSMSG>", MESSAGE)
            If CALLURL(Url, Id) = False Then Return False
            Return True
        Catch ex As Exception
            NotifyIcon1.Visible = True
            NotifyIcon1.Icon = My.Resources.email
            NotifyIcon1.ShowBalloonTip(5000, "Information", ex.Message, ToolTipIcon.Info)
            Return False
        End Try
    End Function

    Function NEWMAILSEND(ByVal ToMail As String, ByVal Subject As String, ByVal MESSAGE As String, Optional ByVal Attachpath As String = "") As Boolean
        Dim obj As System.Web.Mail.SmtpMail
        Dim smtpServer As New SmtpClient()
        Dim mail As New MailMessage
        Dim Counter As Integer = 0
        Try
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
                ' smtpServer.EnableSsl = True
            ElseIf Trim(MailServer2) = "@hotmail.com" Then
                smtpServer.Port = 587
                smtpServer.Host = "smtp.live.com"
                smtpServer.EnableSsl = True
            Else
                smtpServer.Port = SmtpPort
                smtpServer.Host = SmtpHostname
                smtpServer.EnableSsl = SmtpSSL
            End If
            NotifyIcon1.Visible = True
            NotifyIcon1.Icon = My.Resources.email
            NotifyIcon1.Text = "Sending Mail to " & ToMail
            NotifyIcon1.ShowBalloonTip(2000, "Information", "Sending Mail to " & ToMail, ToolTipIcon.Info)
            mail.From = New MailAddress(FromId)
            mail.To.Add(New MailAddress(ToMail))
            mail.Subject = Subject
            Dim _IsHtml As Boolean = False
            If MESSAGE = "HTML" Then _IsHtml = True : MESSAGE = GenerateHtml()
            mail.Body = MESSAGE
            mail.IsBodyHtml = _IsHtml
            If Attachpath.Contains(",") Then
                Dim Path() As String = Attachpath.Split(",")
                For i As Integer = 0 To Path.Length - 1
                    If File.Exists(Path(i)) = True Then mail.Attachments.Add(New Attachment(Path(i)))
                Next
            Else
                If File.Exists(Attachpath) = True Then mail.Attachments.Add(New Attachment(Attachpath))
            End If
            smtpServer.Credentials = New Net.NetworkCredential(FromId.Trim.ToString, MailPassword.Trim.ToString)
            'smtpServer.Timeout = 400000
            smtpServer.Send(mail)
            Return True
        Catch ex As Exception
            NotifyIcon1.Visible = True
            NotifyIcon1.Icon = My.Resources.email
            NotifyIcon1.ShowBalloonTip(5000, "Information", ex.Message, ToolTipIcon.Info)
            Return False
        End Try
    End Function
    Private Function GenerateHtml() As String
        Dim Str As String
        Str = "<p></p>"
        Str += "<p><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;"">GREETINGS from </span></span><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;""><strong>GIRI TECHNOLOGIES PVT LTD</strong></span></span></p>"
        Str += "<p>&ldquo;<span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;""><strong>AKSHAYA GOLD&rdquo; - </strong></span></span><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;""> is one of the leading SOFTWARE in JEWELLERY RETAIL INDUSTRY.</span></span></p>"
        Str += "<ul>"
        Str += "<li>"
        Str += "<p><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;"">360+ satisfied customers</span></span></p>"
        Str += "</li>"
        Str += "<li>"
        Str += "<p><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;"">Inbuilt best business practices</span></span></p>"
        Str += "</li>"
        Str += "<li>"
        Str += "<p><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;"">Configurable for SINGLE / MULTIPLE stores </span></span></p>"
        Str += "</li>"
        Str += "<li>"
        Str += "<p><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;"">Latest technology solution </span></span></p>"
        Str += "<blockquote style=""margin: 0 0 0 40px; border: none; padding: 0px;"">"
        Str += "<p><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;"">- &nbsp; &nbsp;Mobile apps</span></span></p>"
        Str += "<p><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;"">- &nbsp; &nbsp;Web based estimate</span></span></p>"
        Str += "<p><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;"">- &nbsp; &nbsp;Web based catalogue</span></span></p>"
        Str += "<p><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;"">- &nbsp; &nbsp;SMS communication(s)</span></span></p>"
        Str += "<p><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;"">- &nbsp; &nbsp;OTP controlled modules</span></span></p>"
        Str += "<p><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;"">- &nbsp; &nbsp;Finger print device controls</span></span></p>"
        Str += "<p><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;"">- &nbsp; &nbsp;Cloud based solution</span></span></p>"
        Str += "<p><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;"">- &nbsp; &nbsp;RFID Integration</span></span></p>"
        Str += "</blockquote>"
        'Str += "<p>&nbsp;</p>"
        Str += "</li>"
        Str += "</ul>"
        'Str += "<p>&nbsp;</p>"
        Str += "<ul>"
        Str += "<li>"
        Str += "<p><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;"">Statutory Reports</span></span></p>"
        Str += "</li>"
        Str += "<li>"
        Str += "<p><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;"">BI Reports (Business Intelligence) </span></span></p>"
        Str += "</li>"
        Str += "<li>"
        Str += "<p><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;"">Loyalty Program configuration</span></span></p>"
        Str += "</li>"
        Str += "<li>"
        Str += "<p><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;"">Automatic Synchronisation between showrooms and Corporate Office</span></span></p>"
        Str += "</li>"
        Str += "</ul>"
        Str += "<p>&nbsp;</p>"
        Str += "<p><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;"">Software demo kindly contact </span></span></p>"
        Str += "<p><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;""><strong>M/S. GIRI TECHNOLOGIES PVT LTD</strong></span></span></p>"
        Str += "<p><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;""><strong>Chennai: </strong></span></span><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;"">8A, SAROJINI STREET,</span></span></p>"
        Str += "<p><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;"">T.NAGAR, CHENNAI &ndash; 600017</span></span></p>"
        Str += "<p><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;""><strong>Mumba</strong></span></span><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;"">i : Flat No 216,Om Shiv Matoshree</span></span></p>"
        Str += "<p><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;""> Co-op Hsg Soc Ltd, L.T Road</span></span></p>"
        Str += "<p><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;""> Near Dahisar Rlwy Station , Dahisar(west) Mumbai &ndash; 400068</span></span></p>"
        Str += "<p><span style=""font-family: 'Times New Roman', serif;""><span style=""font-size: medium;""> Mobile : Chennai : 9840538818,9940370046 Mumbai :08080502678</span></span></p>"
        Str += "<p>&nbsp;</p>"
        Return Str
    End Function

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If Cn.Database = "" Then Exit Sub
        funcScheduleMsg()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Application.Exit()
    End Sub

    Private Sub SettingsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingsToolStripMenuItem.Click
        If IO.File.Exists(Application.StartupPath + "\SmsServer.ini") = False Then Exit Sub
        Dim file As New FileStream(Application.StartupPath + "\SmsServer.ini", FileMode.Open)
        Dim fstream As New StreamReader(file)
        Dim SqlUser, Url, MailId, serverName, OptinUrl As String
        Dim MailPwd, HostName, Port, DbType, dbPath As String
        Dim Pwd As String
        Dim SSL As Boolean
        fstream.BaseStream.Seek(0, SeekOrigin.Begin)
        serverName = Mid(fstream.ReadLine, 21)
        DbType = IIf(UCase(Mid(fstream.ReadLine, 21)) = "S", "SQL MODE", "WINDOWS MODE")
        SqlUser = IIf(DbType = "SQL MODE", "Sa", "")
        Pwd = Mid(fstream.ReadLine, 21).Trim
        If Trim(Pwd) <> "" Then
            Pwd = Decrypt(Pwd)
        End If
        dbPath = Mid(fstream.ReadLine, 21)
        SmsWay = Mid(fstream.ReadLine, 21)
        Url = Mid(fstream.ReadLine, 21)
        MailId = Mid(fstream.ReadLine, 21)
        MailPwd = Mid(fstream.ReadLine, 21).Trim
        If Trim(MailPwd) <> "" Then
            MailPwd = Decrypt(MailPwd)
        End If
        HostName = Mid(fstream.ReadLine, 21)
        Port = Val(Mid(fstream.ReadLine, 21))
        SSL = IIf(Mid(fstream.ReadLine, 21).ToString.ToUpper = "Y", True, False)
        OptinUrl = Mid(fstream.ReadLine, 21)
        fstream.Close()
        Me.WindowState = FormWindowState.Minimized
        Dim frm As New frmSign(serverName, dbPath, Pwd, DbType, Url, MailId, MailPwd, HostName, Port, SSL, OptinUrl)
        '        frm = New frmSign(serverName, dbPath, Pwd, DbType, Url, MailId, MailPwd, HostName, Port, SSL)
        NotifyIcon1.Text = "Settings Updation."
        NotifyIcon1.BalloonTipText = "Settings Updation."
        NotifyIcon1.ShowBalloonTip(5000, "Information", "Settings Updation.", ToolTipIcon.Info)
        If frm.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Application.Restart()
        End If
    End Sub
End Class
