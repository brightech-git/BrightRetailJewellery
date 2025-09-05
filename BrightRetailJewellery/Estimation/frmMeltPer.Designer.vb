<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMeltPer
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
        Me.txtPUMeltPer_Per = New System.Windows.Forms.TextBox
        Me.lblWastagePer = New System.Windows.Forms.Label
        Me.Label49 = New System.Windows.Forms.Label
        Me.txtPUMelt_WET = New System.Windows.Forms.TextBox
        Me.grpMelt = New CodeVendor.Controls.Grouper
        Me.grpMelt.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtPUMeltPer_Per
        '
        Me.txtPUMeltPer_Per.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUMeltPer_Per.Location = New System.Drawing.Point(101, 16)
        Me.txtPUMeltPer_Per.Name = "txtPUMeltPer_Per"
        Me.txtPUMeltPer_Per.Size = New System.Drawing.Size(62, 22)
        Me.txtPUMeltPer_Per.TabIndex = 1
        '
        'lblWastagePer
        '
        Me.lblWastagePer.AutoSize = True
        Me.lblWastagePer.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWastagePer.Location = New System.Drawing.Point(14, 20)
        Me.lblWastagePer.Name = "lblWastagePer"
        Me.lblWastagePer.Size = New System.Drawing.Size(74, 14)
        Me.lblWastagePer.TabIndex = 0
        Me.lblWastagePer.Text = "Melting %"
        '
        'Label49
        '
        Me.Label49.AutoSize = True
        Me.Label49.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label49.Location = New System.Drawing.Point(34, 46)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(54, 14)
        Me.Label49.TabIndex = 2
        Me.Label49.Text = "Weight"
        '
        'txtPUMelt_WET
        '
        Me.txtPUMelt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUMelt_WET.Location = New System.Drawing.Point(101, 42)
        Me.txtPUMelt_WET.Name = "txtPUMelt_WET"
        Me.txtPUMelt_WET.Size = New System.Drawing.Size(90, 22)
        Me.txtPUMelt_WET.TabIndex = 3
        '
        'grpMelt
        '
        Me.grpMelt.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpMelt.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpMelt.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpMelt.BorderColor = System.Drawing.Color.Transparent
        Me.grpMelt.BorderThickness = 1.0!
        Me.grpMelt.Controls.Add(Me.lblWastagePer)
        Me.grpMelt.Controls.Add(Me.Label49)
        Me.grpMelt.Controls.Add(Me.txtPUMelt_WET)
        Me.grpMelt.Controls.Add(Me.txtPUMeltPer_Per)
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
        Me.grpMelt.Size = New System.Drawing.Size(198, 74)
        Me.grpMelt.TabIndex = 0
        '
        'frmMeltPer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(205, 72)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpMelt)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmMeltPer"
        Me.Text = "Melting Weight"
        Me.grpMelt.ResumeLayout(False)
        Me.grpMelt.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtPUMeltPer_Per As System.Windows.Forms.TextBox
    Friend WithEvents lblWastagePer As System.Windows.Forms.Label
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents txtPUMelt_WET As System.Windows.Forms.TextBox
    Friend WithEvents grpMelt As CodeVendor.Controls.Grouper
End Class
