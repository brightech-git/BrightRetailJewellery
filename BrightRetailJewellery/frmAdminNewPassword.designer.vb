<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAdminNewPassword
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAdminNewPassword))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtNewPwd = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtRetypePwd = New System.Windows.Forms.TextBox()
        Me.lblAuthpwd = New System.Windows.Forms.Label()
        Me.txtAuthPassword = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.ForeColor = System.Drawing.SystemColors.Control
        Me.Label1.Location = New System.Drawing.Point(22, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(89, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "New Password"
        '
        'txtNewPwd
        '
        Me.txtNewPwd.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower
        Me.txtNewPwd.Location = New System.Drawing.Point(140, 6)
        Me.txtNewPwd.MaxLength = 10
        Me.txtNewPwd.Name = "txtNewPwd"
        Me.txtNewPwd.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtNewPwd.Size = New System.Drawing.Size(181, 21)
        Me.txtNewPwd.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.ForeColor = System.Drawing.SystemColors.ButtonFace
        Me.Label2.Location = New System.Drawing.Point(22, 37)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(105, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Retype Password"
        '
        'txtRetypePwd
        '
        Me.txtRetypePwd.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower
        Me.txtRetypePwd.Location = New System.Drawing.Point(140, 34)
        Me.txtRetypePwd.MaxLength = 10
        Me.txtRetypePwd.Name = "txtRetypePwd"
        Me.txtRetypePwd.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtRetypePwd.Size = New System.Drawing.Size(181, 21)
        Me.txtRetypePwd.TabIndex = 3
        '
        'lblAuthpwd
        '
        Me.lblAuthpwd.AutoSize = True
        Me.lblAuthpwd.BackColor = System.Drawing.Color.Transparent
        Me.lblAuthpwd.ForeColor = System.Drawing.SystemColors.ButtonFace
        Me.lblAuthpwd.Location = New System.Drawing.Point(22, 64)
        Me.lblAuthpwd.Name = "lblAuthpwd"
        Me.lblAuthpwd.Size = New System.Drawing.Size(99, 13)
        Me.lblAuthpwd.TabIndex = 4
        Me.lblAuthpwd.Text = "Auth.  Password"
        '
        'txtAuthPassword
        '
        Me.txtAuthPassword.Location = New System.Drawing.Point(140, 61)
        Me.txtAuthPassword.MaxLength = 20
        Me.txtAuthPassword.Name = "txtAuthPassword"
        Me.txtAuthPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtAuthPassword.Size = New System.Drawing.Size(181, 21)
        Me.txtAuthPassword.TabIndex = 5
        '
        'frmAdminNewPassword
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InfoText
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.ClientSize = New System.Drawing.Size(350, 93)
        Me.Controls.Add(Me.lblAuthpwd)
        Me.Controls.Add(Me.txtAuthPassword)
        Me.Controls.Add(Me.txtRetypePwd)
        Me.Controls.Add(Me.txtNewPwd)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.SystemColors.ButtonFace
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmAdminNewPassword"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "New Password"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtNewPwd As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtRetypePwd As System.Windows.Forms.TextBox
    Friend WithEvents lblAuthpwd As System.Windows.Forms.Label
    Friend WithEvents txtAuthPassword As System.Windows.Forms.TextBox
End Class
