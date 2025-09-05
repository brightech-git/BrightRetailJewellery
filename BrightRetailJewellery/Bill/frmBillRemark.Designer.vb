<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBillRemark
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.grpRemark = New CodeVendor.Controls.Grouper
        Me.lblReason = New System.Windows.Forms.Label
        Me.cmbReason = New System.Windows.Forms.ComboBox
        Me.cmbRemark1_OWN = New System.Windows.Forms.ComboBox
        Me.cmbRemark2_OWN = New System.Windows.Forms.ComboBox
        Me.grpRemark.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(9, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Remark"
        '
        'grpRemark
        '
        Me.grpRemark.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpRemark.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpRemark.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpRemark.BorderColor = System.Drawing.Color.Transparent
        Me.grpRemark.BorderThickness = 1.0!
        Me.grpRemark.Controls.Add(Me.lblReason)
        Me.grpRemark.Controls.Add(Me.cmbReason)
        Me.grpRemark.Controls.Add(Me.cmbRemark1_OWN)
        Me.grpRemark.Controls.Add(Me.cmbRemark2_OWN)
        Me.grpRemark.Controls.Add(Me.Label1)
        Me.grpRemark.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpRemark.GroupImage = Nothing
        Me.grpRemark.GroupTitle = ""
        Me.grpRemark.Location = New System.Drawing.Point(3, -6)
        Me.grpRemark.Name = "grpRemark"
        Me.grpRemark.Padding = New System.Windows.Forms.Padding(20)
        Me.grpRemark.PaintGroupBox = False
        Me.grpRemark.RoundCorners = 10
        Me.grpRemark.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpRemark.ShadowControl = False
        Me.grpRemark.ShadowThickness = 3
        Me.grpRemark.Size = New System.Drawing.Size(472, 96)
        Me.grpRemark.TabIndex = 2
        '
        'lblReason
        '
        Me.lblReason.AutoSize = True
        Me.lblReason.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReason.Location = New System.Drawing.Point(195, 15)
        Me.lblReason.Name = "lblReason"
        Me.lblReason.Size = New System.Drawing.Size(61, 16)
        Me.lblReason.TabIndex = 1
        Me.lblReason.Text = "Reason"
        Me.lblReason.Visible = False
        '
        'cmbReason
        '
        Me.cmbReason.FormattingEnabled = True
        Me.cmbReason.Location = New System.Drawing.Point(264, 12)
        Me.cmbReason.Name = "cmbReason"
        Me.cmbReason.Size = New System.Drawing.Size(201, 21)
        Me.cmbReason.TabIndex = 2
        Me.cmbReason.Visible = False
        '
        'cmbRemark1_OWN
        '
        Me.cmbRemark1_OWN.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Bold)
        Me.cmbRemark1_OWN.FormattingEnabled = True
        Me.cmbRemark1_OWN.Location = New System.Drawing.Point(9, 39)
        Me.cmbRemark1_OWN.MaxLength = 50
        Me.cmbRemark1_OWN.Name = "cmbRemark1_OWN"
        Me.cmbRemark1_OWN.Size = New System.Drawing.Size(456, 26)
        Me.cmbRemark1_OWN.TabIndex = 3
        '
        'cmbRemark2_OWN
        '
        Me.cmbRemark2_OWN.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Bold)
        Me.cmbRemark2_OWN.FormattingEnabled = True
        Me.cmbRemark2_OWN.Location = New System.Drawing.Point(9, 64)
        Me.cmbRemark2_OWN.MaxLength = 50
        Me.cmbRemark2_OWN.Name = "cmbRemark2_OWN"
        Me.cmbRemark2_OWN.Size = New System.Drawing.Size(456, 26)
        Me.cmbRemark2_OWN.TabIndex = 4
        '
        'frmBillRemark
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(480, 92)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpRemark)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmBillRemark"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Bill Remark"
        Me.grpRemark.ResumeLayout(False)
        Me.grpRemark.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents grpRemark As CodeVendor.Controls.Grouper
    Friend WithEvents cmbRemark1_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents cmbRemark2_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents lblReason As System.Windows.Forms.Label
    Friend WithEvents cmbReason As System.Windows.Forms.ComboBox
End Class
