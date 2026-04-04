<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChitGiftVoucher
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
        Me.txtCHITCardRegNo_NUM = New System.Windows.Forms.TextBox()
        Me.cmbCHITtCardType_MAN = New System.Windows.Forms.ComboBox()
        Me.cmbGroupCode = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.lblBonusDeduction = New System.Windows.Forms.Label()
        Me.grpCHIT = New CodeVendor.Controls.Grouper()
        Me.grpCHIT.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtCHITCardRegNo_NUM
        '
        Me.txtCHITCardRegNo_NUM.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCHITCardRegNo_NUM.Location = New System.Drawing.Point(112, 101)
        Me.txtCHITCardRegNo_NUM.MaxLength = 12
        Me.txtCHITCardRegNo_NUM.Name = "txtCHITCardRegNo_NUM"
        Me.txtCHITCardRegNo_NUM.Size = New System.Drawing.Size(84, 22)
        Me.txtCHITCardRegNo_NUM.TabIndex = 14
        Me.txtCHITCardRegNo_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cmbCHITtCardType_MAN
        '
        Me.cmbCHITtCardType_MAN.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCHITtCardType_MAN.FormattingEnabled = True
        Me.cmbCHITtCardType_MAN.Location = New System.Drawing.Point(112, 30)
        Me.cmbCHITtCardType_MAN.Name = "cmbCHITtCardType_MAN"
        Me.cmbCHITtCardType_MAN.Size = New System.Drawing.Size(285, 22)
        Me.cmbCHITtCardType_MAN.TabIndex = 3
        '
        'cmbGroupCode
        '
        Me.cmbGroupCode.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbGroupCode.FormattingEnabled = True
        Me.cmbGroupCode.Location = New System.Drawing.Point(112, 61)
        Me.cmbGroupCode.Name = "cmbGroupCode"
        Me.cmbGroupCode.Size = New System.Drawing.Size(84, 22)
        Me.cmbGroupCode.TabIndex = 3
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(8, 34)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(59, 14)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "Scheme"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(8, 66)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(98, 14)
        Me.Label9.TabIndex = 2
        Me.Label9.Text = "GROUP CODE"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(11, 104)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(59, 14)
        Me.Label11.TabIndex = 2
        Me.Label11.Text = "REG NO"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblBonusDeduction
        '
        Me.lblBonusDeduction.AutoSize = True
        Me.lblBonusDeduction.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBonusDeduction.ForeColor = System.Drawing.Color.Red
        Me.lblBonusDeduction.Location = New System.Drawing.Point(3, 78)
        Me.lblBonusDeduction.Name = "lblBonusDeduction"
        Me.lblBonusDeduction.Size = New System.Drawing.Size(0, 13)
        Me.lblBonusDeduction.TabIndex = 40
        '
        'grpCHIT
        '
        Me.grpCHIT.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpCHIT.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpCHIT.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpCHIT.BorderColor = System.Drawing.Color.Transparent
        Me.grpCHIT.BorderThickness = 1.0!
        Me.grpCHIT.Controls.Add(Me.lblBonusDeduction)
        Me.grpCHIT.Controls.Add(Me.Label11)
        Me.grpCHIT.Controls.Add(Me.Label9)
        Me.grpCHIT.Controls.Add(Me.Label8)
        Me.grpCHIT.Controls.Add(Me.cmbGroupCode)
        Me.grpCHIT.Controls.Add(Me.cmbCHITtCardType_MAN)
        Me.grpCHIT.Controls.Add(Me.txtCHITCardRegNo_NUM)
        Me.grpCHIT.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpCHIT.GroupImage = Nothing
        Me.grpCHIT.GroupTitle = ""
        Me.grpCHIT.Location = New System.Drawing.Point(4, 3)
        Me.grpCHIT.Name = "grpCHIT"
        Me.grpCHIT.Padding = New System.Windows.Forms.Padding(20)
        Me.grpCHIT.PaintGroupBox = False
        Me.grpCHIT.RoundCorners = 10
        Me.grpCHIT.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpCHIT.ShadowControl = False
        Me.grpCHIT.ShadowThickness = 3
        Me.grpCHIT.Size = New System.Drawing.Size(412, 143)
        Me.grpCHIT.TabIndex = 0
        '
        'frmChitGiftVoucher
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(428, 158)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpCHIT)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmChitGiftVoucher"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gift Voucher"
        Me.grpCHIT.ResumeLayout(False)
        Me.grpCHIT.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents txtCHITCardRegNo_NUM As TextBox
    Friend WithEvents cmbCHITtCardType_MAN As ComboBox
    Friend WithEvents cmbGroupCode As ComboBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents lblBonusDeduction As Label
    Friend WithEvents grpCHIT As CodeVendor.Controls.Grouper
End Class
