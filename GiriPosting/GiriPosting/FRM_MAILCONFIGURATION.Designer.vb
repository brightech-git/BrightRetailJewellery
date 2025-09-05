<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRM_MAILCONFIGURATION
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtFromEmailId = New System.Windows.Forms.TextBox()
        Me.txtToMailIds = New System.Windows.Forms.TextBox()
        Me.txtFromMailPwd = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtHostName = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtPortNo = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtBody = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tabGeneral = New System.Windows.Forms.TabPage()
        Me.chkEmailSSL = New System.Windows.Forms.CheckBox()
        Me.chkEmailFittoPageWid = New System.Windows.Forms.CheckBox()
        Me.chkEmailHeaderForAllpages = New System.Windows.Forms.CheckBox()
        Me.chkEmailBorder = New System.Windows.Forms.CheckBox()
        Me.chkEmailAttachment = New System.Windows.Forms.CheckBox()
        Me.chkEmailCellColor = New System.Windows.Forms.CheckBox()
        Me.chkEMailLandscape = New System.Windows.Forms.CheckBox()
        Me.TabControl1.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtFromEmailId
        '
        Me.txtFromEmailId.Location = New System.Drawing.Point(115, 62)
        Me.txtFromEmailId.Name = "txtFromEmailId"
        Me.txtFromEmailId.Size = New System.Drawing.Size(265, 21)
        Me.txtFromEmailId.TabIndex = 6
        '
        'txtToMailIds
        '
        Me.txtToMailIds.Location = New System.Drawing.Point(115, 114)
        Me.txtToMailIds.Multiline = True
        Me.txtToMailIds.Name = "txtToMailIds"
        Me.txtToMailIds.Size = New System.Drawing.Size(265, 88)
        Me.txtToMailIds.TabIndex = 10
        '
        'txtFromMailPwd
        '
        Me.txtFromMailPwd.Location = New System.Drawing.Point(115, 88)
        Me.txtFromMailPwd.Name = "txtFromMailPwd"
        Me.txtFromMailPwd.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtFromMailPwd.Size = New System.Drawing.Size(265, 21)
        Me.txtFromMailPwd.TabIndex = 8
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 66)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(87, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "From Email Id"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 117)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(71, 13)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "To Email Id"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 92)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Password"
        '
        'txtHostName
        '
        Me.txtHostName.Location = New System.Drawing.Point(115, 10)
        Me.txtHostName.Name = "txtHostName"
        Me.txtHostName.Size = New System.Drawing.Size(197, 21)
        Me.txtHostName.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 14)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(103, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Smtp Host Name"
        '
        'txtPortNo
        '
        Me.txtPortNo.Location = New System.Drawing.Point(115, 36)
        Me.txtPortNo.Name = "txtPortNo"
        Me.txtPortNo.Size = New System.Drawing.Size(57, 21)
        Me.txtPortNo.TabIndex = 3
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 40)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(83, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Smtp Port No"
        '
        'txtBody
        '
        Me.txtBody.Location = New System.Drawing.Point(115, 208)
        Me.txtBody.Multiline = True
        Me.txtBody.Name = "txtBody"
        Me.txtBody.Size = New System.Drawing.Size(265, 52)
        Me.txtBody.TabIndex = 12
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 212)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(36, 13)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "Body"
        '
        'btnSave
        '
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(238, 361)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 27)
        Me.btnSave.TabIndex = 1
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(319, 361)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 27)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tabGeneral)
        Me.TabControl1.Location = New System.Drawing.Point(6, 5)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(395, 350)
        Me.TabControl1.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.chkEmailSSL)
        Me.tabGeneral.Controls.Add(Me.chkEmailFittoPageWid)
        Me.tabGeneral.Controls.Add(Me.chkEmailHeaderForAllpages)
        Me.tabGeneral.Controls.Add(Me.chkEmailBorder)
        Me.tabGeneral.Controls.Add(Me.chkEmailAttachment)
        Me.tabGeneral.Controls.Add(Me.chkEmailCellColor)
        Me.tabGeneral.Controls.Add(Me.chkEMailLandscape)
        Me.tabGeneral.Controls.Add(Me.Label4)
        Me.tabGeneral.Controls.Add(Me.txtBody)
        Me.tabGeneral.Controls.Add(Me.txtFromEmailId)
        Me.tabGeneral.Controls.Add(Me.Label3)
        Me.tabGeneral.Controls.Add(Me.Label5)
        Me.tabGeneral.Controls.Add(Me.txtHostName)
        Me.tabGeneral.Controls.Add(Me.txtFromMailPwd)
        Me.tabGeneral.Controls.Add(Me.Label2)
        Me.tabGeneral.Controls.Add(Me.txtToMailIds)
        Me.tabGeneral.Controls.Add(Me.Label6)
        Me.tabGeneral.Controls.Add(Me.txtPortNo)
        Me.tabGeneral.Controls.Add(Me.Label1)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 22)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(387, 324)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'chkEmailSSL
        '
        Me.chkEmailSSL.AutoSize = True
        Me.chkEmailSSL.Checked = True
        Me.chkEmailSSL.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkEmailSSL.Location = New System.Drawing.Point(178, 37)
        Me.chkEmailSSL.Name = "chkEmailSSL"
        Me.chkEmailSSL.Size = New System.Drawing.Size(90, 17)
        Me.chkEmailSSL.TabIndex = 4
        Me.chkEmailSSL.Text = "Enable SSL"
        Me.chkEmailSSL.UseVisualStyleBackColor = True
        '
        'chkEmailFittoPageWid
        '
        Me.chkEmailFittoPageWid.AutoSize = True
        Me.chkEmailFittoPageWid.Location = New System.Drawing.Point(115, 302)
        Me.chkEmailFittoPageWid.Name = "chkEmailFittoPageWid"
        Me.chkEmailFittoPageWid.Size = New System.Drawing.Size(120, 17)
        Me.chkEmailFittoPageWid.TabIndex = 15
        Me.chkEmailFittoPageWid.Text = "Fit to page width"
        Me.chkEmailFittoPageWid.UseVisualStyleBackColor = True
        '
        'chkEmailHeaderForAllpages
        '
        Me.chkEmailHeaderForAllpages.AutoSize = True
        Me.chkEmailHeaderForAllpages.Checked = True
        Me.chkEmailHeaderForAllpages.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkEmailHeaderForAllpages.Location = New System.Drawing.Point(115, 284)
        Me.chkEmailHeaderForAllpages.Name = "chkEmailHeaderForAllpages"
        Me.chkEmailHeaderForAllpages.Size = New System.Drawing.Size(143, 17)
        Me.chkEmailHeaderForAllpages.TabIndex = 14
        Me.chkEmailHeaderForAllpages.Text = "Header for All Pages"
        Me.chkEmailHeaderForAllpages.UseVisualStyleBackColor = True
        '
        'chkEmailBorder
        '
        Me.chkEmailBorder.AutoSize = True
        Me.chkEmailBorder.Location = New System.Drawing.Point(258, 284)
        Me.chkEmailBorder.Name = "chkEmailBorder"
        Me.chkEmailBorder.Size = New System.Drawing.Size(94, 17)
        Me.chkEmailBorder.TabIndex = 17
        Me.chkEmailBorder.Text = "With Border"
        Me.chkEmailBorder.UseVisualStyleBackColor = True
        '
        'chkEmailAttachment
        '
        Me.chkEmailAttachment.AutoSize = True
        Me.chkEmailAttachment.Checked = True
        Me.chkEmailAttachment.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkEmailAttachment.Enabled = False
        Me.chkEmailAttachment.Location = New System.Drawing.Point(258, 303)
        Me.chkEmailAttachment.Name = "chkEmailAttachment"
        Me.chkEmailAttachment.Size = New System.Drawing.Size(122, 17)
        Me.chkEmailAttachment.TabIndex = 18
        Me.chkEmailAttachment.Text = "Attachment View"
        Me.chkEmailAttachment.UseVisualStyleBackColor = True
        '
        'chkEmailCellColor
        '
        Me.chkEmailCellColor.AutoSize = True
        Me.chkEmailCellColor.Checked = True
        Me.chkEmailCellColor.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkEmailCellColor.Location = New System.Drawing.Point(115, 266)
        Me.chkEmailCellColor.Name = "chkEmailCellColor"
        Me.chkEmailCellColor.Size = New System.Drawing.Size(83, 17)
        Me.chkEmailCellColor.TabIndex = 13
        Me.chkEmailCellColor.Text = "Cell Color"
        Me.chkEmailCellColor.UseVisualStyleBackColor = True
        '
        'chkEMailLandscape
        '
        Me.chkEMailLandscape.AutoSize = True
        Me.chkEMailLandscape.Checked = True
        Me.chkEMailLandscape.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkEMailLandscape.Location = New System.Drawing.Point(258, 266)
        Me.chkEMailLandscape.Name = "chkEMailLandscape"
        Me.chkEMailLandscape.Size = New System.Drawing.Size(86, 17)
        Me.chkEMailLandscape.TabIndex = 16
        Me.chkEMailLandscape.Text = "Landscape"
        Me.chkEMailLandscape.UseVisualStyleBackColor = True
        '
        'FRM_MAILCONFIGURATION
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(406, 394)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnCancel)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "FRM_MAILCONFIGURATION"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "EMail Configuration"
        Me.TabControl1.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.tabGeneral.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtFromEmailId As System.Windows.Forms.TextBox
    Friend WithEvents txtToMailIds As System.Windows.Forms.TextBox
    Friend WithEvents txtFromMailPwd As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtHostName As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtPortNo As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtBody As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents chkEmailFittoPageWid As System.Windows.Forms.CheckBox
    Friend WithEvents chkEmailHeaderForAllpages As System.Windows.Forms.CheckBox
    Friend WithEvents chkEmailBorder As System.Windows.Forms.CheckBox
    Friend WithEvents chkEmailAttachment As System.Windows.Forms.CheckBox
    Friend WithEvents chkEmailCellColor As System.Windows.Forms.CheckBox
    Friend WithEvents chkEMailLandscape As System.Windows.Forms.CheckBox
    Friend WithEvents chkEmailSSL As System.Windows.Forms.CheckBox
End Class
