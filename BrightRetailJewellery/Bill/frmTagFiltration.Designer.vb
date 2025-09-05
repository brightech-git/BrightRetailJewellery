<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTagFiltration
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
        Me.grpTagFiltration = New CodeVendor.Controls.Grouper
        Me.lblWeight = New System.Windows.Forms.Label
        Me.Label49 = New System.Windows.Forms.Label
        Me.txtWeightTo = New System.Windows.Forms.TextBox
        Me.txtWeightFrom = New System.Windows.Forms.TextBox
        Me.grpTagFiltration.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpTagFiltration
        '
        Me.grpTagFiltration.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpTagFiltration.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpTagFiltration.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpTagFiltration.BorderColor = System.Drawing.Color.Transparent
        Me.grpTagFiltration.BorderThickness = 1.0!
        Me.grpTagFiltration.Controls.Add(Me.lblWeight)
        Me.grpTagFiltration.Controls.Add(Me.Label49)
        Me.grpTagFiltration.Controls.Add(Me.txtWeightTo)
        Me.grpTagFiltration.Controls.Add(Me.txtWeightFrom)
        Me.grpTagFiltration.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpTagFiltration.GroupImage = Nothing
        Me.grpTagFiltration.GroupTitle = ""
        Me.grpTagFiltration.Location = New System.Drawing.Point(3, -4)
        Me.grpTagFiltration.Name = "grpTagFiltration"
        Me.grpTagFiltration.Padding = New System.Windows.Forms.Padding(20)
        Me.grpTagFiltration.PaintGroupBox = False
        Me.grpTagFiltration.RoundCorners = 10
        Me.grpTagFiltration.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpTagFiltration.ShadowControl = False
        Me.grpTagFiltration.ShadowThickness = 3
        Me.grpTagFiltration.Size = New System.Drawing.Size(345, 48)
        Me.grpTagFiltration.TabIndex = 0
        '
        'lblWeight
        '
        Me.lblWeight.AutoSize = True
        Me.lblWeight.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWeight.Location = New System.Drawing.Point(10, 20)
        Me.lblWeight.Name = "lblWeight"
        Me.lblWeight.Size = New System.Drawing.Size(92, 14)
        Me.lblWeight.TabIndex = 0
        Me.lblWeight.Text = "Weight From"
        '
        'Label49
        '
        Me.Label49.AutoSize = True
        Me.Label49.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label49.Location = New System.Drawing.Point(215, 21)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(23, 14)
        Me.Label49.TabIndex = 2
        Me.Label49.Text = "To"
        '
        'txtWeightTo
        '
        Me.txtWeightTo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWeightTo.Location = New System.Drawing.Point(242, 18)
        Me.txtWeightTo.Name = "txtWeightTo"
        Me.txtWeightTo.Size = New System.Drawing.Size(92, 22)
        Me.txtWeightTo.TabIndex = 3
        '
        'txtWeightFrom
        '
        Me.txtWeightFrom.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWeightFrom.Location = New System.Drawing.Point(118, 17)
        Me.txtWeightFrom.Name = "txtWeightFrom"
        Me.txtWeightFrom.Size = New System.Drawing.Size(92, 22)
        Me.txtWeightFrom.TabIndex = 1
        '
        'frmTagFiltration
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(353, 50)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpTagFiltration)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmTagFiltration"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Tag Weight Filtration "
        Me.grpTagFiltration.ResumeLayout(False)
        Me.grpTagFiltration.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpTagFiltration As CodeVendor.Controls.Grouper
    Friend WithEvents lblWeight As System.Windows.Forms.Label
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents txtWeightTo As System.Windows.Forms.TextBox
    Friend WithEvents txtWeightFrom As System.Windows.Forms.TextBox
End Class
