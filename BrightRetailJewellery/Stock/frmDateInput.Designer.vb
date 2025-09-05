<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDateInput
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
        Me.grpWastageMc = New CodeVendor.Controls.Grouper
        Me.dtpBillDatef = New BrighttechPack.DatePicker(Me.components)
        Me.Label1 = New System.Windows.Forms.Label
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
        Me.grpWastageMc.Controls.Add(Me.Label1)
        Me.grpWastageMc.Controls.Add(Me.dtpBillDatef)
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
        Me.grpWastageMc.Size = New System.Drawing.Size(167, 102)
        Me.grpWastageMc.TabIndex = 0
        '
        'dtpBillDatef
        '
        Me.dtpBillDatef.Location = New System.Drawing.Point(32, 71)
        Me.dtpBillDatef.Mask = "##/##/####"
        Me.dtpBillDatef.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpBillDatef.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpBillDatef.Name = "dtpBillDatef"
        Me.dtpBillDatef.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpBillDatef.Size = New System.Drawing.Size(103, 21)
        Me.dtpBillDatef.TabIndex = 16
        Me.dtpBillDatef.Text = "06/03/9998"
        Me.dtpBillDatef.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(50, 36)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Date"
        '
        'frmDateInput
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(173, 99)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpWastageMc)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmDateInput"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Enter the date to validate"
        Me.grpWastageMc.ResumeLayout(False)
        Me.grpWastageMc.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpWastageMc As CodeVendor.Controls.Grouper
    Friend WithEvents dtpBillDatef As BrighttechPack.DatePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
