<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInvdet
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
        Me.components = New System.ComponentModel.Container()
        Me.grpWastageMc = New CodeVendor.Controls.Grouper()
        Me.dtpInvDate = New BrighttechPack.DatePicker(Me.components)
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label49 = New System.Windows.Forms.Label()
        Me.txtInvno = New System.Windows.Forms.TextBox()
        Me.grpWastageMc.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpWastageMc
        '
        Me.grpWastageMc.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpWastageMc.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpWastageMc.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpWastageMc.BorderColor = System.Drawing.Color.Transparent
        Me.grpWastageMc.BorderThickness = 1.0!
        Me.grpWastageMc.Controls.Add(Me.dtpInvDate)
        Me.grpWastageMc.Controls.Add(Me.Label3)
        Me.grpWastageMc.Controls.Add(Me.Label49)
        Me.grpWastageMc.Controls.Add(Me.txtInvno)
        Me.grpWastageMc.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpWastageMc.GroupImage = Nothing
        Me.grpWastageMc.GroupTitle = ""
        Me.grpWastageMc.Location = New System.Drawing.Point(4, -5)
        Me.grpWastageMc.Name = "grpWastageMc"
        Me.grpWastageMc.Padding = New System.Windows.Forms.Padding(20)
        Me.grpWastageMc.PaintGroupBox = False
        Me.grpWastageMc.RoundCorners = 10
        Me.grpWastageMc.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpWastageMc.ShadowControl = False
        Me.grpWastageMc.ShadowThickness = 3
        Me.grpWastageMc.Size = New System.Drawing.Size(398, 50)
        Me.grpWastageMc.TabIndex = 0
        '
        'dtpInvDate
        '
        Me.dtpInvDate.Location = New System.Drawing.Point(277, 17)
        Me.dtpInvDate.Mask = "##/##/####"
        Me.dtpInvDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpInvDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpInvDate.Name = "dtpInvDate"
        Me.dtpInvDate.Seperator = Global.Microsoft.VisualBasic.ChrW(45)
        Me.dtpInvDate.Size = New System.Drawing.Size(114, 21)
        Me.dtpInvDate.TabIndex = 9
        Me.dtpInvDate.Text = "23-10-2013"
        Me.dtpInvDate.Value = New Date(2013, 10, 23, 0, 0, 0, 0)
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(196, 20)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(68, 14)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Inv. Date"
        '
        'Label49
        '
        Me.Label49.AutoSize = True
        Me.Label49.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label49.Location = New System.Drawing.Point(8, 20)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(59, 14)
        Me.Label49.TabIndex = 6
        Me.Label49.Text = "Inv. No."
        '
        'txtInvno
        '
        Me.txtInvno.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInvno.Location = New System.Drawing.Point(73, 17)
        Me.txtInvno.Name = "txtInvno"
        Me.txtInvno.Size = New System.Drawing.Size(92, 22)
        Me.txtInvno.TabIndex = 5
        '
        'frmInvdet
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(402, 52)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpWastageMc)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmInvdet"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Invoice details"
        Me.grpWastageMc.ResumeLayout(False)
        Me.grpWastageMc.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpWastageMc As CodeVendor.Controls.Grouper
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents txtInvno As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents dtpInvDate As BrighttechPack.DatePicker
End Class
