Imports System.IO
Imports System.Xml.Serialization
Public Class FRM_MAILCONFIGURATION
    Private FileName As String = Application.StartupPath & "\AppData\CONFIG_MAIL.XML"
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
Initialize:
        If File.Exists(FileName) Then
            Dim objStreamReader As New StreamReader(FileName)
            Dim obj As New CLS_MAILCONFIGURATION
            Dim x As New XmlSerializer(GetType(CLS_MAILCONFIGURATION))
            obj = x.Deserialize(objStreamReader)
            objStreamReader.Close()
            txtHostName.Text = obj.pSmtpHostName
            txtPortNo.Text = obj.pSmtpPort
            chkEmailSSL.Checked = obj.pEnableSSL
            txtFromEmailId.Text = obj.pFromMailId
            txtFromMailPwd.Text = obj.pFromMailPwd
            txtToMailIds.Text = obj.pToMailId
            txtBody.Text = obj.pBody
            chkEmailCellColor.Checked = obj.pCellColor
            chkEmailHeaderForAllpages.Checked = obj.pHeaderForAllPages
            chkEmailFittoPageWid.Checked = obj.pFitToPageWidth
            chkEMailLandscape.Checked = obj.pLandscape
            chkEmailBorder.Checked = obj.pWithBorder
            chkEmailAttachment.Checked = obj.pAttachementView
        Else
            Dim objStreamWriter As New StreamWriter(FileName)
            Dim x As New XmlSerializer(GetType(CLS_MAILCONFIGURATION))
            x.Serialize(objStreamWriter, New CLS_MAILCONFIGURATION)
            objStreamWriter.Close()
            GoTo Initialize
        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim obj As New CLS_MAILCONFIGURATION
        obj.pSmtpHostName = txtHostName.Text
        obj.pSmtpPort = Val(txtPortNo.Text)
        obj.pEnableSSL = chkEmailSSL.Checked
        obj.pFromMailId = txtFromEmailId.Text
        obj.pFromMailPwd = txtFromMailPwd.Text
        obj.pToMailId = txtToMailIds.Text
        obj.pBody = txtBody.Text
        obj.pCellColor = chkEmailCellColor.Checked
        obj.pHeaderForAllPages = chkEmailHeaderForAllpages.Checked
        obj.pFitToPageWidth = chkEmailFittoPageWid.Checked
        obj.pLandscape = chkEMailLandscape.Checked
        obj.pWithBorder = chkEmailBorder.Checked
        obj.pAttachementView = chkEmailAttachment.Checked

        Dim objStreamWriter As New StreamWriter(FileName)
        Dim x As New XmlSerializer(GetType(CLS_MAILCONFIGURATION))
        x.Serialize(objStreamWriter, obj)
        objStreamWriter.Close()
        Me.Close()
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub FRM_MAILCONFIGURATION_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        ElseIf e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtPortNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPortNo.KeyPress
        Dim allowedChars As String = "0123456789"
        If allowedChars.IndexOf(e.KeyChar) = -1 Then
            ' Invalid Character
            e.Handled = True
        End If
    End Sub

    Private Sub txt_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtToMailIds.KeyPress, txtBody.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            e.Handled = True
        End If
    End Sub

    Private Sub FRM_MAILCONFIGURATION_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class