<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmProcessTypeSelection
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.grpWastageMc = New CodeVendor.Controls.Grouper()
        Me.Label62 = New System.Windows.Forms.Label()
        Me.cmbProcessType = New System.Windows.Forms.ComboBox()
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
        Me.grpWastageMc.Controls.Add(Me.Label62)
        Me.grpWastageMc.Controls.Add(Me.cmbProcessType)
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
        Me.grpWastageMc.Size = New System.Drawing.Size(262, 52)
        Me.grpWastageMc.TabIndex = 0
        '
        'Label62
        '
        Me.Label62.AutoSize = True
        Me.Label62.Location = New System.Drawing.Point(8, 22)
        Me.Label62.Name = "Label62"
        Me.Label62.Size = New System.Drawing.Size(51, 13)
        Me.Label62.TabIndex = 4
        Me.Label62.Text = "Process"
        '
        'cmbProcessType
        '
        Me.cmbProcessType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbProcessType.FormattingEnabled = True
        Me.cmbProcessType.Location = New System.Drawing.Point(83, 18)
        Me.cmbProcessType.Name = "cmbProcessType"
        Me.cmbProcessType.Size = New System.Drawing.Size(163, 21)
        Me.cmbProcessType.TabIndex = 5
        '
        'frmProcessTypeSelection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(264, 46)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpWastageMc)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmProcessTypeSelection"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ProcessType Selection"
        Me.grpWastageMc.ResumeLayout(False)
        Me.grpWastageMc.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpWastageMc As CodeVendor.Controls.Grouper
    Friend WithEvents Label62 As Label
    Friend WithEvents cmbProcessType As ComboBox
End Class
