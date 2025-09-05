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
        Me.txtCompid = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
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
        Me.SuspendLayout()
        '
        'txtCompid
        '
        Me.txtCompid.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCompid.Location = New System.Drawing.Point(157, 16)
        Me.txtCompid.MaxLength = 3
        Me.txtCompid.Name = "txtCompid"
        Me.txtCompid.Size = New System.Drawing.Size(79, 21)
        Me.txtCompid.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(32, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(78, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Company Id"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(32, 46)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(104, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Database Server"
        '
        'txtDbserver
        '
        Me.txtDbserver.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDbserver.Location = New System.Drawing.Point(157, 42)
        Me.txtDbserver.MaxLength = 50
        Me.txtDbserver.Name = "txtDbserver"
        Me.txtDbserver.Size = New System.Drawing.Size(293, 21)
        Me.txtDbserver.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(32, 72)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(90, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Database Path"
        '
        'txtDbPath
        '
        Me.txtDbPath.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDbPath.Location = New System.Drawing.Point(157, 68)
        Me.txtDbPath.MaxLength = 50
        Me.txtDbPath.Name = "txtDbPath"
        Me.txtDbPath.Size = New System.Drawing.Size(293, 21)
        Me.txtDbPath.TabIndex = 7
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(32, 98)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(92, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "SQL Auth Type"
        '
        'cmbLogintype
        '
        Me.cmbLogintype.FormattingEnabled = True
        Me.cmbLogintype.Items.AddRange(New Object() {"SQL MODE", "WINDOWS MODE"})
        Me.cmbLogintype.Location = New System.Drawing.Point(157, 94)
        Me.cmbLogintype.Name = "cmbLogintype"
        Me.cmbLogintype.Size = New System.Drawing.Size(293, 21)
        Me.cmbLogintype.TabIndex = 9
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(32, 124)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(97, 13)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "SQL User Name"
        '
        'txtSqluser
        '
        Me.txtSqluser.Location = New System.Drawing.Point(157, 120)
        Me.txtSqluser.MaxLength = 50
        Me.txtSqluser.Name = "txtSqluser"
        Me.txtSqluser.Size = New System.Drawing.Size(293, 21)
        Me.txtSqluser.TabIndex = 11
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(32, 149)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(118, 13)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "SQL User Password"
        '
        'txtSqlPwd
        '
        Me.txtSqlPwd.Location = New System.Drawing.Point(157, 146)
        Me.txtSqlPwd.MaxLength = 50
        Me.txtSqlPwd.Name = "txtSqlPwd"
        Me.txtSqlPwd.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtSqlPwd.Size = New System.Drawing.Size(293, 21)
        Me.txtSqlPwd.TabIndex = 13
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(157, 173)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(87, 28)
        Me.btnOk.TabIndex = 16
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(246, 173)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(87, 28)
        Me.btnCancel.TabIndex = 17
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'frmSign
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(486, 214)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtSqlPwd)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtSqluser)
        Me.Controls.Add(Me.cmbLogintype)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtDbPath)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtDbserver)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtCompid)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmSign"
        Me.Text = "Installation Signature"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtCompid As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
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
End Class
