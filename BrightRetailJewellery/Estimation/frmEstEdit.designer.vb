<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEstEdit
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
        Me.optDupl = New System.Windows.Forms.RadioButton
        Me.optRevise = New System.Windows.Forms.RadioButton
        Me.txtEstimate_Num = New System.Windows.Forms.TextBox
        Me.grpWastageMc.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblWastagePer
        '
        Me.lblWastagePer.AutoSize = True
        Me.lblWastagePer.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWastagePer.Location = New System.Drawing.Point(45, 59)
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
        Me.grpWastageMc.Controls.Add(Me.optDupl)
        Me.grpWastageMc.Controls.Add(Me.optRevise)
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
        Me.grpWastageMc.Size = New System.Drawing.Size(389, 91)
        Me.grpWastageMc.TabIndex = 0
        '
        'optDupl
        '
        Me.optDupl.AutoSize = True
        Me.optDupl.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optDupl.Location = New System.Drawing.Point(156, 18)
        Me.optDupl.Name = "optDupl"
        Me.optDupl.Size = New System.Drawing.Size(94, 20)
        Me.optDupl.TabIndex = 1
        Me.optDupl.TabStop = True
        Me.optDupl.Text = "Duplicate"
        Me.optDupl.UseVisualStyleBackColor = True
        '
        'optRevise
        '
        Me.optRevise.AutoSize = True
        Me.optRevise.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optRevise.Location = New System.Drawing.Point(69, 18)
        Me.optRevise.Name = "optRevise"
        Me.optRevise.Size = New System.Drawing.Size(54, 20)
        Me.optRevise.TabIndex = 0
        Me.optRevise.TabStop = True
        Me.optRevise.Text = "Edit"
        Me.optRevise.UseVisualStyleBackColor = True
        '
        'txtEstimate_Num
        '
        Me.txtEstimate_Num.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEstimate_Num.Location = New System.Drawing.Point(152, 56)
        Me.txtEstimate_Num.Name = "txtEstimate_Num"
        Me.txtEstimate_Num.Size = New System.Drawing.Size(138, 22)
        Me.txtEstimate_Num.TabIndex = 3
        '
        'frmEstEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(399, 90)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpWastageMc)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmEstEdit"
        Me.Text = "Edit/Duplicate Estimate"
        Me.grpWastageMc.ResumeLayout(False)
        Me.grpWastageMc.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblWastagePer As System.Windows.Forms.Label
    Friend WithEvents grpWastageMc As CodeVendor.Controls.Grouper
    Friend WithEvents txtEstimate_Num As System.Windows.Forms.TextBox
    Friend WithEvents optDupl As System.Windows.Forms.RadioButton
    Friend WithEvents optRevise As System.Windows.Forms.RadioButton
End Class
