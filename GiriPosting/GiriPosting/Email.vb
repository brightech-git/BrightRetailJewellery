Imports System.Net.Mail
Public Class Email
#Region "Properties"
    Private SmtpHostName As String = "smtp.gmail.com"
    Public Property pSmtpHostName() As String
        Get
            Return SmtpHostName
        End Get
        Set(ByVal value As String)
            SmtpHostName = value
        End Set
    End Property
    Private SmtpPort As Integer = 587
    Public Property pSmtpPort() As Integer
        Get
            Return SmtpPort
        End Get
        Set(ByVal value As Integer)
            SmtpPort = value
        End Set
    End Property
    Private FromMailId As String = "Sample@gmail.com"
    Public Property pFromMailId() As String
        Get
            Return FromMailId
        End Get
        Set(ByVal value As String)
            FromMailId = value
        End Set
    End Property
    Private FromMailPwd As String = "welcome"
    Public Property pFromMailPwd() As String
        Get
            Return FromMailPwd
        End Get
        Set(ByVal value As String)
            FromMailPwd = value
        End Set
    End Property
    Private CcMailId As String
    Public Property pCcMailId() As String
        Get
            Return CcMailId
        End Get
        Set(ByVal value As String)
            CcMailId = value
        End Set
    End Property
    Private ToMailId As String
    Public Property pToMailId() As String
        Get
            Return ToMailId
        End Get
        Set(ByVal value As String)
            ToMailId = value
        End Set
    End Property
    Private Subject As String = Nothing
    Public Property pSubject() As String
        Get
            Return Subject
        End Get
        Set(ByVal value As String)
            Subject = value
        End Set
    End Property
    Private Body As String = Nothing
    Public Property pBody() As String
        Get
            Return Body
        End Get
        Set(ByVal value As String)
            Body = value
        End Set
    End Property
    Private isBodyHtml As Boolean = False
    Public Property pIsBodyHtml() As Boolean
        Get
            Return isBodyHtml
        End Get
        Set(ByVal value As Boolean)
            isBodyHtml = value
        End Set
    End Property
    Private AttachFilePath As String = Nothing
    Public Property pAttachFilePath() As String
        Get
            Return AttachFilePath
        End Get
        Set(ByVal value As String)
            AttachFilePath = value
        End Set
    End Property
    Private DGV As DataGridView = Nothing
    Public Property pDGV() As DataGridView
        Get
            Return DGV
        End Get
        Set(ByVal Value As DataGridView)
            DGV = Value
        End Set
    End Property
#End Region
#Region "Methods"
    Private Function DgvToHtmlTag(ByVal Grid As DataGridView) As String
        Dim hTag As String = ""
        hTag += " <html>"
        hTag += " <title></title>"
        hTag += " <body>"
        hTag += " <table>"
        hTag += " <tr>"
        hTag += " <td>Gold</td>"
        hTag += " <td>102520.00</td>"
        hTag += " <td>Silver</td>"
        hTag += " <td>10252.00</td>"
        hTag += " </tr>"
        hTag += " </table>"
        hTag += " </body>"
        hTag += " </html>"
        Return hTag
    End Function
    Public Function Send(Optional ByVal pSmtpPort As Integer = 587, Optional ByVal pSmtpHostName As String = "smtp.gmail.com", Optional ByVal SmtpSSL As Boolean = True) As Boolean
        Try
            Dim mail As New MailMessage()
            Dim SmtpServer As New SmtpClient()
            SmtpServer.Credentials = New Net.NetworkCredential(pFromMailId, pFromMailPwd)
            SmtpServer.Port = pSmtpPort
            SmtpServer.Host = pSmtpHostName
            SmtpServer.EnableSsl = SmtpSSL

            mail = New MailMessage()
            mail.From = New MailAddress(pFromMailId, pFromMailId, System.Text.Encoding.UTF8)
            mail.To.Add(pToMailId)
            If Not pCcMailId Is Nothing And pCcMailId <> "" Then mail.CC.Add(pCcMailId)
            Try
                mail.Subject = pSubject
            Catch ex As Exception
                mail.Subject = ""
            End Try
            mail.SubjectEncoding = System.Text.Encoding.UTF8
            If DGV IsNot Nothing Then
                mail.Body = DgvToHtmlTag(pDGV)
            Else
                mail.Body = pBody
            End If
            mail.BodyEncoding = System.Text.Encoding.UTF8
            mail.Priority = MailPriority.High
            mail.IsBodyHtml = pIsBodyHtml
            If pAttachFilePath <> "" Then
                If IO.File.Exists(pAttachFilePath) Then mail.Attachments.Add(New Attachment(pAttachFilePath))
            End If
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
            SmtpServer.Send(mail)
            mail.Dispose()
            If IO.File.Exists(pAttachFilePath) Then IO.File.Delete(pAttachFilePath)
            Return True
        Catch ex As Exception
            MsgBox("Mail Sending Failure" + vbCrLf + ex.Message + vbCrLf + ex.StackTrace)
            Return False
        End Try
    End Function
#End Region
End Class
