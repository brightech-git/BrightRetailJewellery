<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOrEstEdit
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
        Me.lblWastagePer = New System.Windows.Forms.Label
        Me.grpWastageMc = New CodeVendor.Controls.Grouper
        Me.txtEstimate_Num = New System.Windows.Forms.TextBox
        Me.grpWastageMc.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblWastagePer
        '
        Me.lblWastagePer.AutoSize = True
        Me.lblWastagePer.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWastagePer.Location = New System.Drawing.Point(45, 22)
        Me.lblWastagePer.Name = "lblWastagePer"
        Me.lblWastagePer.Size = New System.Drawing.Size(90, 14)
        Me.lblWastagePer.TabIndex = 2
        Me.lblWastagePer.Text = "Estimate No "
        Me.lblWastagePer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'grpWastageMc
        '
        Me.grpWastageMc.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpWastageMc.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpWastageMc.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpWastageMc.BorderColor = System.Drawing.Color.Transparent
        Me.grpWastageMc.BorderThickness = 1.0!
        Me.grpWastageMc.Controls.Add(Me.lblWastagePer)
        Me.grpWastageMc.Controls.Add(Me.txtEstimate_Num)
        Me.grpWastageMc.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpWastageMc.GroupImage = Nothing
        Me.grpWastageMc.GroupTitle = ""
        Me.grpWastageMc.Location = New System.Drawing.Point(5, -6)
        Me.grpWastageMc.Name = "grpWastageMc"
        Me.grpWastageMc.Padding = New System.Windows.Forms.Padding(20)
        Me.grpWastageMc.PaintGroupBox = False
        Me.grpWastageMc.RoundCorners = 10
        Me.grpWastageMc.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpWastageMc.ShadowControl = False
        Me.grpWastageMc.ShadowThickness = 3
        Me.grpWastageMc.Size = New System.Drawing.Size(356, 56)
        Me.grpWastageMc.TabIndex = 0
        '
        'txtEstimate_Num
        '
        Me.txtEstimate_Num.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEstimate_Num.Location = New System.Drawing.Point(152, 20)
        Me.txtEstimate_Num.Name = "txtEstimate_Num"
        Me.txtEstimate_Num.Size = New System.Drawing.Size(138, 22)
        Me.txtEstimate_Num.TabIndex = 3
        '
        'frmOrEstEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(370, 52)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpWastageMc)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmOrEstEdit"
        Me.Text = "Order Estimate "
        Me.grpWastageMc.ResumeLayout(False)
        Me.grpWastageMc.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblWastagePer As System.Windows.Forms.Label
    Friend WithEvents grpWastageMc As CodeVendor.Controls.Grouper
    Friend WithEvents txtEstimate_Num As System.Windows.Forms.TextBox
End Class
