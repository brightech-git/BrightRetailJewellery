<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAlloycharge
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
        Me.txtAlloy_Wet = New System.Windows.Forms.TextBox
        Me.lblWastagePer = New System.Windows.Forms.Label
        Me.txtAlloyRate_AMT = New System.Windows.Forms.TextBox
        Me.grpMelt = New CodeVendor.Controls.Grouper
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtAlloy_AMT = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.grpMelt.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtAlloy_Wet
        '
        Me.txtAlloy_Wet.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAlloy_Wet.Location = New System.Drawing.Point(111, 17)
        Me.txtAlloy_Wet.Name = "txtAlloy_Wet"
        Me.txtAlloy_Wet.Size = New System.Drawing.Size(62, 22)
        Me.txtAlloy_Wet.TabIndex = 1
        '
        'lblWastagePer
        '
        Me.lblWastagePer.AutoSize = True
        Me.lblWastagePer.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWastagePer.Location = New System.Drawing.Point(14, 20)
        Me.lblWastagePer.Name = "lblWastagePer"
        Me.lblWastagePer.Size = New System.Drawing.Size(91, 14)
        Me.lblWastagePer.TabIndex = 0
        Me.lblWastagePer.Text = "Alloy Weight"
        '
        'txtAlloyRate_AMT
        '
        Me.txtAlloyRate_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAlloyRate_AMT.Location = New System.Drawing.Point(238, 17)
        Me.txtAlloyRate_AMT.Name = "txtAlloyRate_AMT"
        Me.txtAlloyRate_AMT.Size = New System.Drawing.Size(90, 22)
        Me.txtAlloyRate_AMT.TabIndex = 3
        '
        'grpMelt
        '
        Me.grpMelt.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpMelt.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpMelt.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpMelt.BorderColor = System.Drawing.Color.Transparent
        Me.grpMelt.BorderThickness = 1.0!
        Me.grpMelt.Controls.Add(Me.Label2)
        Me.grpMelt.Controls.Add(Me.txtAlloy_AMT)
        Me.grpMelt.Controls.Add(Me.Label1)
        Me.grpMelt.Controls.Add(Me.lblWastagePer)
        Me.grpMelt.Controls.Add(Me.txtAlloyRate_AMT)
        Me.grpMelt.Controls.Add(Me.txtAlloy_Wet)
        Me.grpMelt.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpMelt.GroupImage = Nothing
        Me.grpMelt.GroupTitle = ""
        Me.grpMelt.Location = New System.Drawing.Point(5, -6)
        Me.grpMelt.Name = "grpMelt"
        Me.grpMelt.Padding = New System.Windows.Forms.Padding(20)
        Me.grpMelt.PaintGroupBox = False
        Me.grpMelt.RoundCorners = 10
        Me.grpMelt.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpMelt.ShadowControl = False
        Me.grpMelt.ShadowThickness = 3
        Me.grpMelt.Size = New System.Drawing.Size(532, 53)
        Me.grpMelt.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(345, 21)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(57, 14)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Amount"
        '
        'txtAlloy_AMT
        '
        Me.txtAlloy_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAlloy_AMT.Location = New System.Drawing.Point(419, 18)
        Me.txtAlloy_AMT.Name = "txtAlloy_AMT"
        Me.txtAlloy_AMT.Size = New System.Drawing.Size(90, 22)
        Me.txtAlloy_AMT.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(179, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 14)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "@ Rate"
        '
        'frmAlloycharge
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(543, 51)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpMelt)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmAlloycharge"
        Me.Text = "Alloy Calculation"
        Me.grpMelt.ResumeLayout(False)
        Me.grpMelt.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtAlloy_Wet As System.Windows.Forms.TextBox
    Friend WithEvents lblWastagePer As System.Windows.Forms.Label
    Friend WithEvents txtAlloyRate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents grpMelt As CodeVendor.Controls.Grouper
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtAlloy_AMT As System.Windows.Forms.TextBox
End Class
