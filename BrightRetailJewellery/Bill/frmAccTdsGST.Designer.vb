<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAccTdsGST
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
        Me.grpWastageMc = New CodeVendor.Controls.Grouper()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbTdsCategory_OWN = New System.Windows.Forms.ComboBox()
        Me.Label54 = New System.Windows.Forms.Label()
        Me.txtTdsPer_PER = New System.Windows.Forms.TextBox()
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
        Me.grpWastageMc.Controls.Add(Me.cmbTdsCategory_OWN)
        Me.grpWastageMc.Controls.Add(Me.Label54)
        Me.grpWastageMc.Controls.Add(Me.txtTdsPer_PER)
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
        Me.grpWastageMc.Size = New System.Drawing.Size(660, 95)
        Me.grpWastageMc.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(10, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(94, 14)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Tds Category"
        '
        'cmbTdsCategory_OWN
        '
        Me.cmbTdsCategory_OWN.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbTdsCategory_OWN.FormattingEnabled = True
        Me.cmbTdsCategory_OWN.Location = New System.Drawing.Point(140, 26)
        Me.cmbTdsCategory_OWN.Name = "cmbTdsCategory_OWN"
        Me.cmbTdsCategory_OWN.Size = New System.Drawing.Size(497, 22)
        Me.cmbTdsCategory_OWN.TabIndex = 3
        '
        'Label54
        '
        Me.Label54.AutoSize = True
        Me.Label54.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label54.Location = New System.Drawing.Point(10, 55)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(49, 14)
        Me.Label54.TabIndex = 10
        Me.Label54.Text = "Tds %"
        '
        'txtTdsPer_PER
        '
        Me.txtTdsPer_PER.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTdsPer_PER.Location = New System.Drawing.Point(140, 52)
        Me.txtTdsPer_PER.Name = "txtTdsPer_PER"
        Me.txtTdsPer_PER.Size = New System.Drawing.Size(90, 22)
        Me.txtTdsPer_PER.TabIndex = 11
        '
        'frmAccTdsGST
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(667, 97)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpWastageMc)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmAccTdsGST"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Tds"
        Me.grpWastageMc.ResumeLayout(False)
        Me.grpWastageMc.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpWastageMc As CodeVendor.Controls.Grouper
    Friend WithEvents Label54 As System.Windows.Forms.Label
    Friend WithEvents txtTdsPer_PER As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbTdsCategory_OWN As System.Windows.Forms.ComboBox
End Class
