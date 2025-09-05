Imports System.Net.Mail
Public Class SendMail
    Public Shared Function Send(ByVal smtpHost As String, ByVal smtpPort As Integer, ByVal ToMailid As List(Of String), ByVal subject As String _
    , ByVal body As String, ByVal filepath As String, ByVal Fromid As String _
    , ByVal Pwd As String, ByVal dispName As String, Optional ByVal DeleteFileAfterSend As Boolean = False, Optional ByVal ERRmsg As Boolean = True) As Boolean
        Try
            Dim mail As New MailMessage()
            Dim SmtpServer As New SmtpClient()
            SmtpServer.Credentials = New Net.NetworkCredential(Fromid, Pwd)
            SmtpServer.Port = smtpPort
            SmtpServer.Host = smtpHost
            SmtpServer.EnableSsl = True

            mail = New MailMessage()
            mail.From = New MailAddress(Fromid, dispName, System.Text.Encoding.UTF8)
            For Each s As String In ToMailid
                mail.To.Add(s)
            Next

            mail.Subject = subject
            mail.SubjectEncoding = System.Text.Encoding.UTF8
            mail.Body = body
            mail.BodyEncoding = System.Text.Encoding.UTF8
            mail.Priority = MailPriority.High
            mail.IsBodyHtml = False
            If filepath <> "" Then
                If IO.File.Exists(filepath) Then mail.Attachments.Add(New Attachment(filepath))
            End If
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
            SmtpServer.Send(mail)
            mail.Dispose()
            If IO.File.Exists(filepath) Then IO.File.Delete(filepath)
            Return True
        Catch ex As Exception
            If ERRmsg Then
                MsgBox(ex.Message)
            End If
            Return False
        End Try
    End Function
    Public Shared Function Send(ByVal smtpHost As String, ByVal smtpPort As Integer, ByVal ToMailid As String, ByVal subject As String _
, ByVal body As String, ByVal filepath As String, ByVal Fromid As String _
, ByVal Pwd As String, ByVal dispName As String, Optional ByVal DeleteFileAfterSend As Boolean = False, Optional ByVal ERRmsg As Boolean = True) As Boolean
        Try
            Dim mail As New MailMessage()
            Dim SmtpServer As New SmtpClient()
            SmtpServer.Credentials = New Net.NetworkCredential(Fromid, Pwd)
            SmtpServer.Port = smtpPort
            SmtpServer.Host = smtpHost
            SmtpServer.EnableSsl = True

            mail = New MailMessage()
            mail.From = New MailAddress(Fromid, dispName, System.Text.Encoding.UTF8)
            mail.To.Add(ToMailid)

            mail.Subject = subject
            mail.SubjectEncoding = System.Text.Encoding.UTF8
            mail.Body = body
            mail.BodyEncoding = System.Text.Encoding.UTF8
            mail.Priority = MailPriority.High
            mail.IsBodyHtml = False
            If filepath <> "" Then
                If IO.File.Exists(filepath) Then mail.Attachments.Add(New Attachment(filepath))
            End If
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
            SmtpServer.Send(mail)
            mail.Dispose()
            If IO.File.Exists(filepath) Then IO.File.Delete(filepath)
            Return True
        Catch ex As Exception
            If ERRmsg Then
                MsgBox(ex.Message)
            End If
            Return False
        End Try
    End Function

End Class
