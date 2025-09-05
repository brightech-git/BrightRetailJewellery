<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSign
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
        Me.components = New System.ComponentModel.Container
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtDbserver = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtDbPath = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.cmbLogintype = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtSqluser = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtSqlPwd = New System.Windows.Forms.TextBox
        Me.btnOk = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtSmsUrl = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtMailId = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtMailPwd = New System.Windows.Forms.TextBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label11 = New System.Windows.Forms.Label
        Me.txtSmsOptinUrl = New System.Windows.Forms.TextBox
        Me.ChkSSL = New System.Windows.Forms.CheckBox
        Me.txtSmtpPort = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.txtHostName = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Label12 = New System.Windows.Forms.Label
        Me.txtWhatsappUrl = New System.Windows.Forms.TextBox
        Me.txtWhatsappOptinUrl = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(3, 15)
        Me.Label3.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(104, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Database Server"
        '
        'txtDbserver
        '
        Me.txtDbserver.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDbserver.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDbserver.Location = New System.Drawing.Point(184, 11)
        Me.txtDbserver.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.txtDbserver.MaxLength = 50
        Me.txtDbserver.Name = "txtDbserver"
        Me.txtDbserver.Size = New System.Drawing.Size(254, 21)
        Me.txtDbserver.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.txtDbserver, "Server Name")
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(3, 41)
        Me.Label4.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(90, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Database Path"
        '
        'txtDbPath
        '
        Me.txtDbPath.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDbPath.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDbPath.Location = New System.Drawing.Point(184, 37)
        Me.txtDbPath.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.txtDbPath.MaxLength = 50
        Me.txtDbPath.Name = "txtDbPath"
        Me.txtDbPath.Size = New System.Drawing.Size(254, 21)
        Me.txtDbPath.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.txtDbPath, "Where to Store the Data")
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(3, 67)
        Me.Label5.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(146, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "SQL Authentication Type"
        '
        'cmbLogintype
        '
        Me.cmbLogintype.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbLogintype.FormattingEnabled = True
        Me.cmbLogintype.Items.AddRange(New Object() {"SQL MODE", "WINDOWS MODE"})
        Me.cmbLogintype.Location = New System.Drawing.Point(184, 63)
        Me.cmbLogintype.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.cmbLogintype.Name = "cmbLogintype"
        Me.cmbLogintype.Size = New System.Drawing.Size(254, 21)
        Me.cmbLogintype.TabIndex = 5
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(3, 93)
        Me.Label6.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(97, 13)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "SQL User Name"
        '
        'txtSqluser
        '
        Me.txtSqluser.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSqluser.Location = New System.Drawing.Point(184, 89)
        Me.txtSqluser.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.txtSqluser.MaxLength = 50
        Me.txtSqluser.Name = "txtSqluser"
        Me.txtSqluser.Size = New System.Drawing.Size(254, 21)
        Me.txtSqluser.TabIndex = 7
        Me.ToolTip1.SetToolTip(Me.txtSqluser, "Sql Server User Name")
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(3, 119)
        Me.Label7.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(118, 13)
        Me.Label7.TabIndex = 8
        Me.Label7.Text = "SQL User Password"
        '
        'txtSqlPwd
        '
        Me.txtSqlPwd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSqlPwd.Location = New System.Drawing.Point(184, 115)
        Me.txtSqlPwd.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.txtSqlPwd.MaxLength = 50
        Me.txtSqlPwd.Name = "txtSqlPwd"
        Me.txtSqlPwd.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtSqlPwd.Size = New System.Drawing.Size(254, 21)
        Me.txtSqlPwd.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.txtSqlPwd, "Sql Server Password")
        '
        'btnOk
        '
        Me.btnOk.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOk.Location = New System.Drawing.Point(184, 354)
        Me.btnOk.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(86, 26)
        Me.btnOk.TabIndex = 23
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(278, 354)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(86, 26)
        Me.btnCancel.TabIndex = 24
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(3, 145)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "SMS URL"
        '
        'txtSmsUrl
        '
        Me.txtSmsUrl.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSmsUrl.Location = New System.Drawing.Point(184, 141)
        Me.txtSmsUrl.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.txtSmsUrl.MaxLength = 250
        Me.txtSmsUrl.Name = "txtSmsUrl"
        Me.txtSmsUrl.Size = New System.Drawing.Size(254, 21)
        Me.txtSmsUrl.TabIndex = 11
        Me.txtSmsUrl.Text = "http://api-alerts.solutionsinfini.com/v3/?method=sms&api_key=A35c0caa7e6269e6655e" & _
            "8dc7c1bec74e4&to=<SMSTO>&sender=<SENDERID>&message=<SMSMSG>"
        Me.ToolTip1.SetToolTip(Me.txtSmsUrl, "Url to Send Sms")
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(3, 197)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(78, 13)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "From Mail Id"
        '
        'txtMailId
        '
        Me.txtMailId.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMailId.Location = New System.Drawing.Point(184, 193)
        Me.txtMailId.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.txtMailId.MaxLength = 50
        Me.txtMailId.Name = "txtMailId"
        Me.txtMailId.Size = New System.Drawing.Size(254, 21)
        Me.txtMailId.TabIndex = 15
        Me.ToolTip1.SetToolTip(Me.txtMailId, "From Mail Id ")
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(3, 223)
        Me.Label8.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(120, 13)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "From Mail Password"
        '
        'txtMailPwd
        '
        Me.txtMailPwd.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMailPwd.Location = New System.Drawing.Point(184, 219)
        Me.txtMailPwd.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.txtMailPwd.MaxLength = 50
        Me.txtMailPwd.Name = "txtMailPwd"
        Me.txtMailPwd.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtMailPwd.Size = New System.Drawing.Size(254, 21)
        Me.txtMailPwd.TabIndex = 17
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.txtWhatsappOptinUrl)
        Me.Panel1.Controls.Add(Me.Label13)
        Me.Panel1.Controls.Add(Me.txtWhatsappUrl)
        Me.Panel1.Controls.Add(Me.Label12)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.txtSmsOptinUrl)
        Me.Panel1.Controls.Add(Me.ChkSSL)
        Me.Panel1.Controls.Add(Me.txtSmtpPort)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.txtHostName)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.btnOk)
        Me.Panel1.Controls.Add(Me.txtDbserver)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.txtDbPath)
        Me.Panel1.Controls.Add(Me.txtMailPwd)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.txtMailId)
        Me.Panel1.Controls.Add(Me.cmbLogintype)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.txtSqluser)
        Me.Panel1.Controls.Add(Me.txtSmsUrl)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.btnCancel)
        Me.Panel1.Controls.Add(Me.txtSqlPwd)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Location = New System.Drawing.Point(14, 6)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(450, 385)
        Me.Panel1.TabIndex = 0
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(3, 171)
        Me.Label11.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(92, 13)
        Me.Label11.TabIndex = 12
        Me.Label11.Text = "SMS Optin URL"
        '
        'txtSmsOptinUrl
        '
        Me.txtSmsOptinUrl.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSmsOptinUrl.Location = New System.Drawing.Point(184, 167)
        Me.txtSmsOptinUrl.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.txtSmsOptinUrl.MaxLength = 250
        Me.txtSmsOptinUrl.Name = "txtSmsOptinUrl"
        Me.txtSmsOptinUrl.Size = New System.Drawing.Size(254, 21)
        Me.txtSmsOptinUrl.TabIndex = 13
        Me.ToolTip1.SetToolTip(Me.txtSmsOptinUrl, "Url to Register the Customer Mobile no to Send Sms from Sender")
        '
        'ChkSSL
        '
        Me.ChkSSL.AutoSize = True
        Me.ChkSSL.Location = New System.Drawing.Point(314, 273)
        Me.ChkSSL.Name = "ChkSSL"
        Me.ChkSSL.Size = New System.Drawing.Size(90, 17)
        Me.ChkSSL.TabIndex = 22
        Me.ChkSSL.Text = "Enable SSL"
        Me.ChkSSL.UseVisualStyleBackColor = True
        '
        'txtSmtpPort
        '
        Me.txtSmtpPort.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSmtpPort.Location = New System.Drawing.Point(184, 271)
        Me.txtSmtpPort.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.txtSmtpPort.MaxLength = 50
        Me.txtSmtpPort.Name = "txtSmtpPort"
        Me.txtSmtpPort.Size = New System.Drawing.Size(125, 21)
        Me.txtSmtpPort.TabIndex = 21
        Me.txtSmtpPort.Text = "587"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(3, 275)
        Me.Label10.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(79, 13)
        Me.Label10.TabIndex = 20
        Me.Label10.Text = "Smtp PortNo"
        '
        'txtHostName
        '
        Me.txtHostName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHostName.Location = New System.Drawing.Point(184, 245)
        Me.txtHostName.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.txtHostName.MaxLength = 50
        Me.txtHostName.Name = "txtHostName"
        Me.txtHostName.Size = New System.Drawing.Size(254, 21)
        Me.txtHostName.TabIndex = 19
        Me.txtHostName.Text = "smtp.gmail.com"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(3, 249)
        Me.Label9.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(99, 13)
        Me.Label9.TabIndex = 18
        Me.Label9.Text = "Smtp HostName"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(3, 301)
        Me.Label12.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(89, 13)
        Me.Label12.TabIndex = 25
        Me.Label12.Text = "Whatsapp URL"
        '
        'txtWhatsappUrl
        '
        Me.txtWhatsappUrl.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWhatsappUrl.Location = New System.Drawing.Point(184, 297)
        Me.txtWhatsappUrl.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.txtWhatsappUrl.MaxLength = 250
        Me.txtWhatsappUrl.Name = "txtWhatsappUrl"
        Me.txtWhatsappUrl.Size = New System.Drawing.Size(254, 21)
        Me.txtWhatsappUrl.TabIndex = 26
        Me.txtWhatsappUrl.Text = "http://wapi.msgpedia.com/wapp/api/send?apikey=<APIKEY>&mobile=<SMSTO>&msg=<SMSMSG" & _
            ">"
        Me.ToolTip1.SetToolTip(Me.txtWhatsappUrl, "http://wapi.msgpedia.com/wapp/api/send?apikey=<APIKEY>&mobile=<SMSTO>&msg=<SMSMSG" & _
                ">")
        '
        'txtWhatsappOptinUrl
        '
        Me.txtWhatsappOptinUrl.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWhatsappOptinUrl.Location = New System.Drawing.Point(184, 323)
        Me.txtWhatsappOptinUrl.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.txtWhatsappOptinUrl.MaxLength = 250
        Me.txtWhatsappOptinUrl.Name = "txtWhatsappOptinUrl"
        Me.txtWhatsappOptinUrl.Size = New System.Drawing.Size(254, 21)
        Me.txtWhatsappOptinUrl.TabIndex = 28
        Me.ToolTip1.SetToolTip(Me.txtWhatsappOptinUrl, "http://wapi.msgpedia.com/wapp/api/send?apikey=<APIKEY>&mobile=<SMSTO>&msg=<SMSMSG" & _
                ">")
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(3, 327)
        Me.Label13.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(123, 13)
        Me.Label13.TabIndex = 27
        Me.Label13.Text = "Whatsapp Optin URL"
        '
        'frmSign
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(481, 403)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSign"
        Me.ShowIcon = False
        Me.Text = "Installation Signature"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtDbserver As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtDbPath As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cmbLogintype As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtSqluser As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtSqlPwd As System.Windows.Forms.TextBox
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtSmsUrl As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtMailId As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtMailPwd As System.Windows.Forms.TextBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ChkSSL As System.Windows.Forms.CheckBox
    Friend WithEvents txtSmtpPort As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtHostName As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtSmsOptinUrl As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents txtWhatsappUrl As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtWhatsappOptinUrl As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
End Class
