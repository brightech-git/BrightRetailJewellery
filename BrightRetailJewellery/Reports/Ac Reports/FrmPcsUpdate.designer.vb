<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmPcsUpdate
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
        Me.grpDetail = New CodeVendor.Controls.Grouper
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtPCS_NUM = New System.Windows.Forms.TextBox
        Me.grpDetail.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpDetail
        '
        Me.grpDetail.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpDetail.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpDetail.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpDetail.BorderColor = System.Drawing.Color.Transparent
        Me.grpDetail.BorderThickness = 1.0!
        Me.grpDetail.Controls.Add(Me.txtPCS_NUM)
        Me.grpDetail.Controls.Add(Me.Label1)
        Me.grpDetail.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpDetail.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpDetail.GroupImage = Nothing
        Me.grpDetail.GroupTitle = ""
        Me.grpDetail.Location = New System.Drawing.Point(0, 0)
        Me.grpDetail.Name = "grpDetail"
        Me.grpDetail.Padding = New System.Windows.Forms.Padding(20)
        Me.grpDetail.PaintGroupBox = False
        Me.grpDetail.RoundCorners = 10
        Me.grpDetail.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpDetail.ShadowControl = False
        Me.grpDetail.ShadowThickness = 3
        Me.grpDetail.Size = New System.Drawing.Size(239, 127)
        Me.grpDetail.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(24, 57)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(31, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "PCS"
        '
        'txtPCS_NUM
        '
        Me.txtPCS_NUM.Location = New System.Drawing.Point(82, 54)
        Me.txtPCS_NUM.Name = "txtPCS_NUM"
        Me.txtPCS_NUM.Size = New System.Drawing.Size(111, 21)
        Me.txtPCS_NUM.TabIndex = 1
        '
        'FrmPcsUpdate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(239, 127)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpDetail)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmPcsUpdate"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "PCS UPDATE"
        Me.grpDetail.ResumeLayout(False)
        Me.grpDetail.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpDetail As CodeVendor.Controls.Grouper
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtPCS_NUM As System.Windows.Forms.TextBox
End Class
