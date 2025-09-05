<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGiftVoucher
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
        Me.grpGiftVoucher = New CodeVendor.Controls.Grouper()
        Me.lblBal = New System.Windows.Forms.Label()
        Me.lblBalVal = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtRefNo = New System.Windows.Forms.TextBox()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.txtGiftRemark = New System.Windows.Forms.TextBox()
        Me.txtGiftRowIndex = New System.Windows.Forms.TextBox()
        Me.gridGiftVoucherTotal = New System.Windows.Forms.DataGridView()
        Me.gridGiftVoucher = New System.Windows.Forms.DataGridView()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.txtGiftAmount_AMT = New System.Windows.Forms.TextBox()
        Me.txtGiftUnit_NUM = New System.Windows.Forms.TextBox()
        Me.txtGiftDenomination_AMT = New System.Windows.Forms.TextBox()
        Me.cmbGiftVoucherType = New System.Windows.Forms.ComboBox()
        Me.grpGiftVoucher.SuspendLayout()
        CType(Me.gridGiftVoucherTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridGiftVoucher, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpGiftVoucher
        '
        Me.grpGiftVoucher.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpGiftVoucher.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpGiftVoucher.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpGiftVoucher.BorderColor = System.Drawing.Color.Transparent
        Me.grpGiftVoucher.BorderThickness = 1.0!
        Me.grpGiftVoucher.Controls.Add(Me.lblBal)
        Me.grpGiftVoucher.Controls.Add(Me.lblBalVal)
        Me.grpGiftVoucher.Controls.Add(Me.Label1)
        Me.grpGiftVoucher.Controls.Add(Me.txtRefNo)
        Me.grpGiftVoucher.Controls.Add(Me.Label36)
        Me.grpGiftVoucher.Controls.Add(Me.txtGiftRemark)
        Me.grpGiftVoucher.Controls.Add(Me.txtGiftRowIndex)
        Me.grpGiftVoucher.Controls.Add(Me.gridGiftVoucherTotal)
        Me.grpGiftVoucher.Controls.Add(Me.gridGiftVoucher)
        Me.grpGiftVoucher.Controls.Add(Me.Label35)
        Me.grpGiftVoucher.Controls.Add(Me.Label34)
        Me.grpGiftVoucher.Controls.Add(Me.Label33)
        Me.grpGiftVoucher.Controls.Add(Me.Label25)
        Me.grpGiftVoucher.Controls.Add(Me.txtGiftAmount_AMT)
        Me.grpGiftVoucher.Controls.Add(Me.txtGiftUnit_NUM)
        Me.grpGiftVoucher.Controls.Add(Me.txtGiftDenomination_AMT)
        Me.grpGiftVoucher.Controls.Add(Me.cmbGiftVoucherType)
        Me.grpGiftVoucher.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpGiftVoucher.GroupImage = Nothing
        Me.grpGiftVoucher.GroupTitle = ""
        Me.grpGiftVoucher.Location = New System.Drawing.Point(5, -5)
        Me.grpGiftVoucher.Name = "grpGiftVoucher"
        Me.grpGiftVoucher.Padding = New System.Windows.Forms.Padding(20)
        Me.grpGiftVoucher.PaintGroupBox = False
        Me.grpGiftVoucher.RoundCorners = 10
        Me.grpGiftVoucher.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpGiftVoucher.ShadowControl = False
        Me.grpGiftVoucher.ShadowThickness = 3
        Me.grpGiftVoucher.Size = New System.Drawing.Size(613, 259)
        Me.grpGiftVoucher.TabIndex = 1
        '
        'lblBal
        '
        Me.lblBal.Location = New System.Drawing.Point(330, 14)
        Me.lblBal.Name = "lblBal"
        Me.lblBal.Size = New System.Drawing.Size(109, 14)
        Me.lblBal.TabIndex = 15
        Me.lblBal.Text = "Bounz Point Bal -"
        Me.lblBal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblBal.Visible = False
        '
        'lblBalVal
        '
        Me.lblBalVal.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBalVal.ForeColor = System.Drawing.Color.Red
        Me.lblBalVal.Location = New System.Drawing.Point(445, 14)
        Me.lblBalVal.Name = "lblBalVal"
        Me.lblBalVal.Size = New System.Drawing.Size(145, 14)
        Me.lblBalVal.TabIndex = 14
        Me.lblBalVal.Text = "..."
        Me.lblBalVal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblBalVal.Visible = False
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(160, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(73, 14)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "Vou. No"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtRefNo
        '
        Me.txtRefNo.Location = New System.Drawing.Point(163, 52)
        Me.txtRefNo.MaxLength = 15
        Me.txtRefNo.Name = "txtRefNo"
        Me.txtRefNo.Size = New System.Drawing.Size(73, 21)
        Me.txtRefNo.TabIndex = 2
        '
        'Label36
        '
        Me.Label36.Location = New System.Drawing.Point(236, 35)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(124, 14)
        Me.Label36.TabIndex = 7
        Me.Label36.Text = "Remark"
        Me.Label36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtGiftRemark
        '
        Me.txtGiftRemark.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtGiftRemark.Location = New System.Drawing.Point(239, 52)
        Me.txtGiftRemark.MaxLength = 20
        Me.txtGiftRemark.Name = "txtGiftRemark"
        Me.txtGiftRemark.Size = New System.Drawing.Size(124, 21)
        Me.txtGiftRemark.TabIndex = 3
        Me.txtGiftRemark.Text = "12345678901234567890"
        '
        'txtGiftRowIndex
        '
        Me.txtGiftRowIndex.Location = New System.Drawing.Point(598, 27)
        Me.txtGiftRowIndex.Name = "txtGiftRowIndex"
        Me.txtGiftRowIndex.Size = New System.Drawing.Size(16, 21)
        Me.txtGiftRowIndex.TabIndex = 10
        Me.txtGiftRowIndex.Visible = False
        '
        'gridGiftVoucherTotal
        '
        Me.gridGiftVoucherTotal.AllowUserToAddRows = False
        Me.gridGiftVoucherTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridGiftVoucherTotal.Enabled = False
        Me.gridGiftVoucherTotal.Location = New System.Drawing.Point(7, 227)
        Me.gridGiftVoucherTotal.Name = "gridGiftVoucherTotal"
        Me.gridGiftVoucherTotal.ReadOnly = True
        Me.gridGiftVoucherTotal.Size = New System.Drawing.Size(596, 19)
        Me.gridGiftVoucherTotal.TabIndex = 12
        '
        'gridGiftVoucher
        '
        Me.gridGiftVoucher.AllowUserToAddRows = False
        Me.gridGiftVoucher.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridGiftVoucher.Location = New System.Drawing.Point(7, 74)
        Me.gridGiftVoucher.Name = "gridGiftVoucher"
        Me.gridGiftVoucher.ReadOnly = True
        Me.gridGiftVoucher.Size = New System.Drawing.Size(596, 153)
        Me.gridGiftVoucher.TabIndex = 11
        '
        'Label35
        '
        Me.Label35.Location = New System.Drawing.Point(502, 35)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(98, 14)
        Me.Label35.TabIndex = 10
        Me.Label35.Text = "Amount"
        Me.Label35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label34
        '
        Me.Label34.Location = New System.Drawing.Point(454, 35)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(42, 14)
        Me.Label34.TabIndex = 9
        Me.Label34.Text = "Unit"
        Me.Label34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label33
        '
        Me.Label33.Location = New System.Drawing.Point(362, 35)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(91, 14)
        Me.Label33.TabIndex = 8
        Me.Label33.Text = "Denomination"
        Me.Label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label25
        '
        Me.Label25.Location = New System.Drawing.Point(7, 35)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(160, 14)
        Me.Label25.TabIndex = 0
        Me.Label25.Text = "Voucher Type"
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtGiftAmount_AMT
        '
        Me.txtGiftAmount_AMT.Location = New System.Drawing.Point(505, 52)
        Me.txtGiftAmount_AMT.MaxLength = 12
        Me.txtGiftAmount_AMT.Name = "txtGiftAmount_AMT"
        Me.txtGiftAmount_AMT.Size = New System.Drawing.Size(98, 21)
        Me.txtGiftAmount_AMT.TabIndex = 6
        '
        'txtGiftUnit_NUM
        '
        Me.txtGiftUnit_NUM.Location = New System.Drawing.Point(440, 52)
        Me.txtGiftUnit_NUM.MaxLength = 8
        Me.txtGiftUnit_NUM.Name = "txtGiftUnit_NUM"
        Me.txtGiftUnit_NUM.Size = New System.Drawing.Size(64, 21)
        Me.txtGiftUnit_NUM.TabIndex = 5
        '
        'txtGiftDenomination_AMT
        '
        Me.txtGiftDenomination_AMT.Location = New System.Drawing.Point(365, 52)
        Me.txtGiftDenomination_AMT.MaxLength = 12
        Me.txtGiftDenomination_AMT.Name = "txtGiftDenomination_AMT"
        Me.txtGiftDenomination_AMT.Size = New System.Drawing.Size(74, 21)
        Me.txtGiftDenomination_AMT.TabIndex = 4
        '
        'cmbGiftVoucherType
        '
        Me.cmbGiftVoucherType.FormattingEnabled = True
        Me.cmbGiftVoucherType.Location = New System.Drawing.Point(7, 52)
        Me.cmbGiftVoucherType.Name = "cmbGiftVoucherType"
        Me.cmbGiftVoucherType.Size = New System.Drawing.Size(160, 21)
        Me.cmbGiftVoucherType.TabIndex = 1
        '
        'frmGiftVoucher
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(620, 255)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpGiftVoucher)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmGiftVoucher"
        Me.ShowInTaskbar = False
        Me.Text = "Gift Voucher"
        Me.grpGiftVoucher.ResumeLayout(False)
        Me.grpGiftVoucher.PerformLayout()
        CType(Me.gridGiftVoucherTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridGiftVoucher, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpGiftVoucher As CodeVendor.Controls.Grouper
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents txtGiftRemark As System.Windows.Forms.TextBox
    Friend WithEvents txtGiftRowIndex As System.Windows.Forms.TextBox
    Friend WithEvents gridGiftVoucherTotal As System.Windows.Forms.DataGridView
    Friend WithEvents gridGiftVoucher As System.Windows.Forms.DataGridView
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents txtGiftAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtGiftUnit_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtGiftDenomination_AMT As System.Windows.Forms.TextBox
    Friend WithEvents cmbGiftVoucherType As System.Windows.Forms.ComboBox
    Friend WithEvents txtRefNo As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblBal As Label
    Friend WithEvents lblBalVal As Label
End Class
