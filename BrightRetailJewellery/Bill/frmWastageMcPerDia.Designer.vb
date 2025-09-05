<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWastageMcPerDia
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
        Me.txtSAWastagePer_Per = New System.Windows.Forms.TextBox
        Me.lblWastagePer = New System.Windows.Forms.Label
        Me.lblMcPerGrm = New System.Windows.Forms.Label
        Me.txtSAMcPerGrm_AMT = New System.Windows.Forms.TextBox
        Me.grpWastageMc = New CodeVendor.Controls.Grouper
        Me.grpWastageMc.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtSAWastagePer_Per
        '
        Me.txtSAWastagePer_Per.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSAWastagePer_Per.Location = New System.Drawing.Point(101, 26)
        Me.txtSAWastagePer_Per.Name = "txtSAWastagePer_Per"
        Me.txtSAWastagePer_Per.Size = New System.Drawing.Size(92, 22)
        Me.txtSAWastagePer_Per.TabIndex = 1
        '
        'lblWastagePer
        '
        Me.lblWastagePer.AutoSize = True
        Me.lblWastagePer.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWastagePer.Location = New System.Drawing.Point(14, 30)
        Me.lblWastagePer.Name = "lblWastagePer"
        Me.lblWastagePer.Size = New System.Drawing.Size(84, 14)
        Me.lblWastagePer.TabIndex = 0
        Me.lblWastagePer.Text = "Wastage %"
        '
        'lblMcPerGrm
        '
        Me.lblMcPerGrm.AutoSize = True
        Me.lblMcPerGrm.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMcPerGrm.Location = New System.Drawing.Point(199, 30)
        Me.lblMcPerGrm.Name = "lblMcPerGrm"
        Me.lblMcPerGrm.Size = New System.Drawing.Size(84, 14)
        Me.lblMcPerGrm.TabIndex = 2
        Me.lblMcPerGrm.Text = "Mc Per Grm"
        '
        'txtSAMcPerGrm_AMT
        '
        Me.txtSAMcPerGrm_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSAMcPerGrm_AMT.Location = New System.Drawing.Point(285, 26)
        Me.txtSAMcPerGrm_AMT.Name = "txtSAMcPerGrm_AMT"
        Me.txtSAMcPerGrm_AMT.Size = New System.Drawing.Size(90, 22)
        Me.txtSAMcPerGrm_AMT.TabIndex = 3
        '
        'grpWastageMc
        '
        Me.grpWastageMc.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpWastageMc.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpWastageMc.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpWastageMc.BorderColor = System.Drawing.Color.Transparent
        Me.grpWastageMc.BorderThickness = 1.0!
        Me.grpWastageMc.Controls.Add(Me.lblWastagePer)
        Me.grpWastageMc.Controls.Add(Me.lblMcPerGrm)
        Me.grpWastageMc.Controls.Add(Me.txtSAMcPerGrm_AMT)
        Me.grpWastageMc.Controls.Add(Me.txtSAWastagePer_Per)
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
        Me.grpWastageMc.Size = New System.Drawing.Size(389, 61)
        Me.grpWastageMc.TabIndex = 0
        '
        'frmWastageMcPerDia
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(399, 60)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpWastageMc)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmWastageMcPerDia"
        Me.Text = "Wastage Mc Percentage"
        Me.grpWastageMc.ResumeLayout(False)
        Me.grpWastageMc.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtSAWastagePer_Per As System.Windows.Forms.TextBox
    Friend WithEvents lblWastagePer As System.Windows.Forms.Label
    Friend WithEvents lblMcPerGrm As System.Windows.Forms.Label
    Friend WithEvents txtSAMcPerGrm_AMT As System.Windows.Forms.TextBox
    Friend WithEvents grpWastageMc As CodeVendor.Controls.Grouper
End Class
