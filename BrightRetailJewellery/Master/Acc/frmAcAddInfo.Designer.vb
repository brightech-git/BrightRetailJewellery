<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAcAddInfo
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
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtSupName = New System.Windows.Forms.TextBox
        Me.txtGuardian = New System.Windows.Forms.TextBox
        Me.txtChild1 = New System.Windows.Forms.TextBox
        Me.txtChild2 = New System.Windows.Forms.TextBox
        Me.txtChild3 = New System.Windows.Forms.TextBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.CheckBox3 = New System.Windows.Forms.CheckBox
        Me.CheckBox2 = New System.Windows.Forms.CheckBox
        Me.Chkdb2 = New System.Windows.Forms.CheckBox
        Me.Chkdb1 = New System.Windows.Forms.CheckBox
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.CmbIssuedBy = New System.Windows.Forms.ComboBox
        Me.CmbEnteredBy = New System.Windows.Forms.ComboBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.txtOccupation = New System.Windows.Forms.TextBox
        Me.dtpC3DOB = New BrighttechPack.DatePicker(Me.components)
        Me.dtpDOB = New BrighttechPack.DatePicker(Me.components)
        Me.dtpC2DOB = New BrighttechPack.DatePicker(Me.components)
        Me.dtpC1DOB = New BrighttechPack.DatePicker(Me.components)
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(3, 24)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(96, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Guardian Name"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 55)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(66, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Sup Name"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 86)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(80, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Child Name1"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 117)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(80, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Child Name2"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(3, 148)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(80, 13)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Child Name3"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtSupName
        '
        Me.txtSupName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSupName.Location = New System.Drawing.Point(100, 52)
        Me.txtSupName.MaxLength = 55
        Me.txtSupName.Name = "txtSupName"
        Me.txtSupName.Size = New System.Drawing.Size(161, 21)
        Me.txtSupName.TabIndex = 3
        '
        'txtGuardian
        '
        Me.txtGuardian.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtGuardian.Location = New System.Drawing.Point(100, 21)
        Me.txtGuardian.MaxLength = 55
        Me.txtGuardian.Name = "txtGuardian"
        Me.txtGuardian.Size = New System.Drawing.Size(161, 21)
        Me.txtGuardian.TabIndex = 1
        '
        'txtChild1
        '
        Me.txtChild1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtChild1.Location = New System.Drawing.Point(100, 83)
        Me.txtChild1.MaxLength = 55
        Me.txtChild1.Name = "txtChild1"
        Me.txtChild1.Size = New System.Drawing.Size(161, 21)
        Me.txtChild1.TabIndex = 7
        '
        'txtChild2
        '
        Me.txtChild2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtChild2.Location = New System.Drawing.Point(100, 114)
        Me.txtChild2.MaxLength = 55
        Me.txtChild2.Name = "txtChild2"
        Me.txtChild2.Size = New System.Drawing.Size(161, 21)
        Me.txtChild2.TabIndex = 11
        '
        'txtChild3
        '
        Me.txtChild3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtChild3.Location = New System.Drawing.Point(100, 145)
        Me.txtChild3.MaxLength = 55
        Me.txtChild3.Name = "txtChild3"
        Me.txtChild3.Size = New System.Drawing.Size(161, 21)
        Me.txtChild3.TabIndex = 15
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.CheckBox3)
        Me.Panel1.Controls.Add(Me.CheckBox2)
        Me.Panel1.Controls.Add(Me.Chkdb2)
        Me.Panel1.Controls.Add(Me.Chkdb1)
        Me.Panel1.Controls.Add(Me.btnCancel)
        Me.Panel1.Controls.Add(Me.btnSave)
        Me.Panel1.Controls.Add(Me.CmbIssuedBy)
        Me.Panel1.Controls.Add(Me.CmbEnteredBy)
        Me.Panel1.Controls.Add(Me.Label12)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.txtOccupation)
        Me.Panel1.Controls.Add(Me.dtpC3DOB)
        Me.Panel1.Controls.Add(Me.dtpDOB)
        Me.Panel1.Controls.Add(Me.dtpC2DOB)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.dtpC1DOB)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.txtSupName)
        Me.Panel1.Controls.Add(Me.txtGuardian)
        Me.Panel1.Controls.Add(Me.txtChild3)
        Me.Panel1.Controls.Add(Me.txtChild1)
        Me.Panel1.Controls.Add(Me.txtChild2)
        Me.Panel1.Location = New System.Drawing.Point(12, 23)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(457, 308)
        Me.Panel1.TabIndex = 0
        '
        'CheckBox3
        '
        Me.CheckBox3.AutoSize = True
        Me.CheckBox3.Location = New System.Drawing.Point(266, 146)
        Me.CheckBox3.Name = "CheckBox3"
        Me.CheckBox3.Size = New System.Drawing.Size(52, 17)
        Me.CheckBox3.TabIndex = 16
        Me.CheckBox3.Text = "DOB"
        Me.CheckBox3.UseVisualStyleBackColor = True
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Location = New System.Drawing.Point(266, 116)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(52, 17)
        Me.CheckBox2.TabIndex = 12
        Me.CheckBox2.Text = "DOB"
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'Chkdb2
        '
        Me.Chkdb2.AutoSize = True
        Me.Chkdb2.Location = New System.Drawing.Point(266, 85)
        Me.Chkdb2.Name = "Chkdb2"
        Me.Chkdb2.Size = New System.Drawing.Size(52, 17)
        Me.Chkdb2.TabIndex = 8
        Me.Chkdb2.Text = "DOB"
        Me.Chkdb2.UseVisualStyleBackColor = True
        '
        'Chkdb1
        '
        Me.Chkdb1.AutoSize = True
        Me.Chkdb1.Location = New System.Drawing.Point(266, 53)
        Me.Chkdb1.Name = "Chkdb1"
        Me.Chkdb1.Size = New System.Drawing.Size(52, 17)
        Me.Chkdb1.TabIndex = 4
        Me.Chkdb1.Text = "DOB"
        Me.Chkdb1.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(201, 269)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 30)
        Me.btnCancel.TabIndex = 25
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(100, 269)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 24
        Me.btnSave.Text = "Ok"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'CmbIssuedBy
        '
        Me.CmbIssuedBy.FormattingEnabled = True
        Me.CmbIssuedBy.Location = New System.Drawing.Point(100, 238)
        Me.CmbIssuedBy.Name = "CmbIssuedBy"
        Me.CmbIssuedBy.Size = New System.Drawing.Size(161, 21)
        Me.CmbIssuedBy.TabIndex = 23
        '
        'CmbEnteredBy
        '
        Me.CmbEnteredBy.FormattingEnabled = True
        Me.CmbEnteredBy.Location = New System.Drawing.Point(100, 207)
        Me.CmbEnteredBy.Name = "CmbEnteredBy"
        Me.CmbEnteredBy.Size = New System.Drawing.Size(161, 21)
        Me.CmbEnteredBy.TabIndex = 21
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(3, 241)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(64, 13)
        Me.Label12.TabIndex = 22
        Me.Label12.Text = "Issued By"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(3, 210)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(70, 13)
        Me.Label11.TabIndex = 20
        Me.Label11.Text = "Entered By"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(3, 179)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(70, 13)
        Me.Label10.TabIndex = 18
        Me.Label10.Text = "Occupation"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtOccupation
        '
        Me.txtOccupation.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtOccupation.Location = New System.Drawing.Point(100, 176)
        Me.txtOccupation.MaxLength = 55
        Me.txtOccupation.Name = "txtOccupation"
        Me.txtOccupation.Size = New System.Drawing.Size(161, 21)
        Me.txtOccupation.TabIndex = 19
        '
        'dtpC3DOB
        '
        Me.dtpC3DOB.Enabled = False
        Me.dtpC3DOB.Location = New System.Drawing.Point(321, 145)
        Me.dtpC3DOB.Mask = "##/##/####"
        Me.dtpC3DOB.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpC3DOB.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpC3DOB.Name = "dtpC3DOB"
        Me.dtpC3DOB.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpC3DOB.Size = New System.Drawing.Size(88, 21)
        Me.dtpC3DOB.TabIndex = 17
        Me.dtpC3DOB.Text = "06/03/9998"
        Me.dtpC3DOB.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'dtpDOB
        '
        Me.dtpDOB.Enabled = False
        Me.dtpDOB.Location = New System.Drawing.Point(321, 52)
        Me.dtpDOB.Mask = "##/##/####"
        Me.dtpDOB.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpDOB.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpDOB.Name = "dtpDOB"
        Me.dtpDOB.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpDOB.Size = New System.Drawing.Size(88, 21)
        Me.dtpDOB.TabIndex = 5
        Me.dtpDOB.Text = "06/03/9998"
        Me.dtpDOB.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'dtpC2DOB
        '
        Me.dtpC2DOB.Enabled = False
        Me.dtpC2DOB.Location = New System.Drawing.Point(321, 114)
        Me.dtpC2DOB.Mask = "##/##/####"
        Me.dtpC2DOB.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpC2DOB.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpC2DOB.Name = "dtpC2DOB"
        Me.dtpC2DOB.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpC2DOB.Size = New System.Drawing.Size(88, 21)
        Me.dtpC2DOB.TabIndex = 13
        Me.dtpC2DOB.Text = "06/03/9998"
        Me.dtpC2DOB.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'dtpC1DOB
        '
        Me.dtpC1DOB.Enabled = False
        Me.dtpC1DOB.Location = New System.Drawing.Point(321, 83)
        Me.dtpC1DOB.Mask = "##/##/####"
        Me.dtpC1DOB.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpC1DOB.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpC1DOB.Name = "dtpC1DOB"
        Me.dtpC1DOB.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpC1DOB.Size = New System.Drawing.Size(88, 21)
        Me.dtpC1DOB.TabIndex = 9
        Me.dtpC1DOB.Text = "06/03/9998"
        Me.dtpC1DOB.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'frmAcAddInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(481, 343)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmAcAddInfo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Achead Other Info"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtSupName As System.Windows.Forms.TextBox
    Friend WithEvents txtGuardian As System.Windows.Forms.TextBox
    Friend WithEvents txtChild1 As System.Windows.Forms.TextBox
    Friend WithEvents txtChild2 As System.Windows.Forms.TextBox
    Friend WithEvents txtChild3 As System.Windows.Forms.TextBox
    Friend WithEvents dtpDOB As BrighttechPack.DatePicker
    Friend WithEvents dtpC1DOB As BrighttechPack.DatePicker
    Friend WithEvents dtpC2DOB As BrighttechPack.DatePicker
    Friend WithEvents dtpC3DOB As BrighttechPack.DatePicker
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtOccupation As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents CmbIssuedBy As System.Windows.Forms.ComboBox
    Friend WithEvents CmbEnteredBy As System.Windows.Forms.ComboBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents CheckBox3 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
    Friend WithEvents Chkdb2 As System.Windows.Forms.CheckBox
    Friend WithEvents Chkdb1 As System.Windows.Forms.CheckBox
End Class
