<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSetItemWastMc
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
        Me.txtSetId_NUM = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtSetMc_AMT = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtSetWastage_WET = New System.Windows.Forms.TextBox
        Me.Label49 = New System.Windows.Forms.Label
        Me.txtSetMcPerGrm_AMT = New System.Windows.Forms.TextBox
        Me.txtSetWastagePer_Per = New System.Windows.Forms.TextBox
        Me.grpWastageMc.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblWastagePer
        '
        Me.lblWastagePer.AutoSize = True
        Me.lblWastagePer.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWastagePer.Location = New System.Drawing.Point(14, 25)
        Me.lblWastagePer.Name = "lblWastagePer"
        Me.lblWastagePer.Size = New System.Drawing.Size(84, 14)
        Me.lblWastagePer.TabIndex = 0
        Me.lblWastagePer.Text = "Wastage %"
        Me.lblWastagePer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'grpWastageMc
        '
        Me.grpWastageMc.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpWastageMc.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpWastageMc.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpWastageMc.BorderColor = System.Drawing.Color.Transparent
        Me.grpWastageMc.BorderThickness = 1.0!
        Me.grpWastageMc.Controls.Add(Me.txtSetId_NUM)
        Me.grpWastageMc.Controls.Add(Me.Label1)
        Me.grpWastageMc.Controls.Add(Me.txtSetMc_AMT)
        Me.grpWastageMc.Controls.Add(Me.Label9)
        Me.grpWastageMc.Controls.Add(Me.Label8)
        Me.grpWastageMc.Controls.Add(Me.txtSetWastage_WET)
        Me.grpWastageMc.Controls.Add(Me.lblWastagePer)
        Me.grpWastageMc.Controls.Add(Me.Label49)
        Me.grpWastageMc.Controls.Add(Me.txtSetMcPerGrm_AMT)
        Me.grpWastageMc.Controls.Add(Me.txtSetWastagePer_Per)
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
        Me.grpWastageMc.Size = New System.Drawing.Size(389, 115)
        Me.grpWastageMc.TabIndex = 0
        '
        'txtSetId_NUM
        '
        Me.txtSetId_NUM.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSetId_NUM.Location = New System.Drawing.Point(101, 85)
        Me.txtSetId_NUM.MaxLength = 12
        Me.txtSetId_NUM.Name = "txtSetId_NUM"
        Me.txtSetId_NUM.Size = New System.Drawing.Size(79, 22)
        Me.txtSetId_NUM.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(14, 88)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(50, 15)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Set ID"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtSetMc_AMT
        '
        Me.txtSetMc_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSetMc_AMT.Location = New System.Drawing.Point(258, 54)
        Me.txtSetMc_AMT.MaxLength = 12
        Me.txtSetMc_AMT.Name = "txtSetMc_AMT"
        Me.txtSetMc_AMT.Size = New System.Drawing.Size(108, 22)
        Me.txtSetMc_AMT.TabIndex = 7
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(186, 57)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(73, 15)
        Me.Label9.TabIndex = 6
        Me.Label9.Text = "Mc"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(186, 25)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(66, 15)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "Wastage"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtSetWastage_WET
        '
        Me.txtSetWastage_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSetWastage_WET.Location = New System.Drawing.Point(258, 22)
        Me.txtSetWastage_WET.MaxLength = 10
        Me.txtSetWastage_WET.Name = "txtSetWastage_WET"
        Me.txtSetWastage_WET.Size = New System.Drawing.Size(108, 22)
        Me.txtSetWastage_WET.TabIndex = 3
        '
        'Label49
        '
        Me.Label49.AutoSize = True
        Me.Label49.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label49.Location = New System.Drawing.Point(14, 57)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(84, 14)
        Me.Label49.TabIndex = 4
        Me.Label49.Text = "Mc Per Grm"
        Me.Label49.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtSetMcPerGrm_AMT
        '
        Me.txtSetMcPerGrm_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSetMcPerGrm_AMT.Location = New System.Drawing.Point(101, 54)
        Me.txtSetMcPerGrm_AMT.Name = "txtSetMcPerGrm_AMT"
        Me.txtSetMcPerGrm_AMT.Size = New System.Drawing.Size(79, 22)
        Me.txtSetMcPerGrm_AMT.TabIndex = 5
        '
        'txtSetWastagePer_Per
        '
        Me.txtSetWastagePer_Per.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSetWastagePer_Per.Location = New System.Drawing.Point(101, 22)
        Me.txtSetWastagePer_Per.Name = "txtSetWastagePer_Per"
        Me.txtSetWastagePer_Per.Size = New System.Drawing.Size(79, 22)
        Me.txtSetWastagePer_Per.TabIndex = 1
        '
        'frmSetItemWastMc
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(399, 112)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpWastageMc)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmSetItemWastMc"
        Me.Text = "Set Item Wastage/Mc"
        Me.grpWastageMc.ResumeLayout(False)
        Me.grpWastageMc.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblWastagePer As System.Windows.Forms.Label
    Friend WithEvents grpWastageMc As CodeVendor.Controls.Grouper
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents txtSetMcPerGrm_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtSetWastagePer_Per As System.Windows.Forms.TextBox
    Friend WithEvents txtSetMc_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtSetWastage_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtSetId_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
