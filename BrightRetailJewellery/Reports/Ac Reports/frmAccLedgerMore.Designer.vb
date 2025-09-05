<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAccLedgerMore
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
        Me.txtFindAmount_AMT = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtFindNarration = New System.Windows.Forms.TextBox()
        Me.cmbFindAmount = New System.Windows.Forms.ComboBox()
        Me.cmbVoucherType = New System.Windows.Forms.ComboBox()
        Me.lstContra = New System.Windows.Forms.CheckedListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkContraAll = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkGstNo = New System.Windows.Forms.CheckBox()
        Me.chkPanNo = New System.Windows.Forms.CheckBox()
        Me.ChkBillPrefix = New System.Windows.Forms.CheckBox()
        Me.chkDiscAuthEmp = New System.Windows.Forms.CheckBox()
        Me.chkSubledger = New System.Windows.Forms.CheckBox()
        Me.rdbSepColumns = New System.Windows.Forms.RadioButton()
        Me.chkUserName = New System.Windows.Forms.CheckBox()
        Me.chkCostName = New System.Windows.Forms.CheckBox()
        Me.rdbWithParticular = New System.Windows.Forms.RadioButton()
        Me.chkBankName = New System.Windows.Forms.CheckBox()
        Me.chkChequeDate = New System.Windows.Forms.CheckBox()
        Me.chkRefDate = New System.Windows.Forms.CheckBox()
        Me.chkRemark2 = New System.Windows.Forms.CheckBox()
        Me.chkRefNo = New System.Windows.Forms.CheckBox()
        Me.chkRemark1 = New System.Windows.Forms.CheckBox()
        Me.chkChequeNo = New System.Windows.Forms.CheckBox()
        Me.chkEmp = New System.Windows.Forms.CheckBox()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtFindAmount_AMT
        '
        Me.txtFindAmount_AMT.Location = New System.Drawing.Point(233, 291)
        Me.txtFindAmount_AMT.Name = "txtFindAmount_AMT"
        Me.txtFindAmount_AMT.Size = New System.Drawing.Size(105, 21)
        Me.txtFindAmount_AMT.TabIndex = 7
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(11, 295)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(78, 13)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "Find Amount"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(11, 323)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(87, 13)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "Find Narration"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(10, 267)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(84, 13)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "Voucher Type"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtFindNarration
        '
        Me.txtFindNarration.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtFindNarration.Location = New System.Drawing.Point(117, 319)
        Me.txtFindNarration.Name = "txtFindNarration"
        Me.txtFindNarration.Size = New System.Drawing.Size(221, 21)
        Me.txtFindNarration.TabIndex = 9
        '
        'cmbFindAmount
        '
        Me.cmbFindAmount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFindAmount.FormattingEnabled = True
        Me.cmbFindAmount.Location = New System.Drawing.Point(117, 291)
        Me.cmbFindAmount.Name = "cmbFindAmount"
        Me.cmbFindAmount.Size = New System.Drawing.Size(110, 21)
        Me.cmbFindAmount.TabIndex = 6
        '
        'cmbVoucherType
        '
        Me.cmbVoucherType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbVoucherType.FormattingEnabled = True
        Me.cmbVoucherType.Location = New System.Drawing.Point(117, 263)
        Me.cmbVoucherType.Name = "cmbVoucherType"
        Me.cmbVoucherType.Size = New System.Drawing.Size(221, 21)
        Me.cmbVoucherType.TabIndex = 4
        '
        'lstContra
        '
        Me.lstContra.FormattingEnabled = True
        Me.lstContra.Location = New System.Drawing.Point(11, 42)
        Me.lstContra.Name = "lstContra"
        Me.lstContra.Size = New System.Drawing.Size(327, 212)
        Me.lstContra.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(104, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Contra Account :"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkContraAll
        '
        Me.chkContraAll.AutoSize = True
        Me.chkContraAll.Location = New System.Drawing.Point(14, 24)
        Me.chkContraAll.Name = "chkContraAll"
        Me.chkContraAll.Size = New System.Drawing.Size(79, 17)
        Me.chkContraAll.TabIndex = 1
        Me.chkContraAll.Text = "Select All"
        Me.chkContraAll.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkGstNo)
        Me.GroupBox1.Controls.Add(Me.chkPanNo)
        Me.GroupBox1.Controls.Add(Me.ChkBillPrefix)
        Me.GroupBox1.Controls.Add(Me.chkDiscAuthEmp)
        Me.GroupBox1.Controls.Add(Me.chkSubledger)
        Me.GroupBox1.Controls.Add(Me.rdbSepColumns)
        Me.GroupBox1.Controls.Add(Me.chkUserName)
        Me.GroupBox1.Controls.Add(Me.chkCostName)
        Me.GroupBox1.Controls.Add(Me.rdbWithParticular)
        Me.GroupBox1.Controls.Add(Me.chkBankName)
        Me.GroupBox1.Controls.Add(Me.chkChequeDate)
        Me.GroupBox1.Controls.Add(Me.chkRefDate)
        Me.GroupBox1.Controls.Add(Me.chkRemark2)
        Me.GroupBox1.Controls.Add(Me.chkRefNo)
        Me.GroupBox1.Controls.Add(Me.chkRemark1)
        Me.GroupBox1.Controls.Add(Me.chkChequeNo)
        Me.GroupBox1.Location = New System.Drawing.Point(13, 346)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(325, 153)
        Me.GroupBox1.TabIndex = 10
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Display"
        '
        'chkGstNo
        '
        Me.chkGstNo.AutoSize = True
        Me.chkGstNo.Location = New System.Drawing.Point(224, 136)
        Me.chkGstNo.Name = "chkGstNo"
        Me.chkGstNo.Size = New System.Drawing.Size(64, 17)
        Me.chkGstNo.TabIndex = 37
        Me.chkGstNo.Text = "Gst No"
        Me.chkGstNo.UseVisualStyleBackColor = True
        '
        'chkPanNo
        '
        Me.chkPanNo.AutoSize = True
        Me.chkPanNo.Location = New System.Drawing.Point(140, 136)
        Me.chkPanNo.Name = "chkPanNo"
        Me.chkPanNo.Size = New System.Drawing.Size(66, 17)
        Me.chkPanNo.TabIndex = 36
        Me.chkPanNo.Text = "Pan No"
        Me.chkPanNo.UseVisualStyleBackColor = True
        '
        'ChkBillPrefix
        '
        Me.ChkBillPrefix.AutoSize = True
        Me.ChkBillPrefix.Location = New System.Drawing.Point(224, 113)
        Me.ChkBillPrefix.Name = "ChkBillPrefix"
        Me.ChkBillPrefix.Size = New System.Drawing.Size(80, 17)
        Me.ChkBillPrefix.TabIndex = 35
        Me.ChkBillPrefix.Text = "Bill Prefix"
        Me.ChkBillPrefix.UseVisualStyleBackColor = True
        '
        'chkDiscAuthEmp
        '
        Me.chkDiscAuthEmp.AutoSize = True
        Me.chkDiscAuthEmp.Location = New System.Drawing.Point(25, 112)
        Me.chkDiscAuthEmp.Name = "chkDiscAuthEmp"
        Me.chkDiscAuthEmp.Size = New System.Drawing.Size(108, 17)
        Me.chkDiscAuthEmp.TabIndex = 12
        Me.chkDiscAuthEmp.Text = "Disc Authorize"
        Me.chkDiscAuthEmp.UseVisualStyleBackColor = True
        '
        'chkSubledger
        '
        Me.chkSubledger.AutoSize = True
        Me.chkSubledger.Location = New System.Drawing.Point(140, 113)
        Me.chkSubledger.Name = "chkSubledger"
        Me.chkSubledger.Size = New System.Drawing.Size(88, 17)
        Me.chkSubledger.TabIndex = 11
        Me.chkSubledger.Text = "Sub ledger"
        Me.chkSubledger.UseVisualStyleBackColor = True
        '
        'rdbSepColumns
        '
        Me.rdbSepColumns.AutoSize = True
        Me.rdbSepColumns.Location = New System.Drawing.Point(167, 18)
        Me.rdbSepColumns.Name = "rdbSepColumns"
        Me.rdbSepColumns.Size = New System.Drawing.Size(131, 17)
        Me.rdbSepColumns.TabIndex = 1
        Me.rdbSepColumns.Text = "Separate Columns"
        Me.rdbSepColumns.UseVisualStyleBackColor = True
        '
        'chkUserName
        '
        Me.chkUserName.AutoSize = True
        Me.chkUserName.Location = New System.Drawing.Point(224, 89)
        Me.chkUserName.Name = "chkUserName"
        Me.chkUserName.Size = New System.Drawing.Size(89, 17)
        Me.chkUserName.TabIndex = 10
        Me.chkUserName.Text = "User Name"
        Me.chkUserName.UseVisualStyleBackColor = True
        '
        'chkCostName
        '
        Me.chkCostName.AutoSize = True
        Me.chkCostName.Location = New System.Drawing.Point(140, 89)
        Me.chkCostName.Name = "chkCostName"
        Me.chkCostName.Size = New System.Drawing.Size(85, 17)
        Me.chkCostName.TabIndex = 7
        Me.chkCostName.Text = "CostName"
        Me.chkCostName.UseVisualStyleBackColor = True
        '
        'rdbWithParticular
        '
        Me.rdbWithParticular.AutoSize = True
        Me.rdbWithParticular.Checked = True
        Me.rdbWithParticular.Location = New System.Drawing.Point(53, 18)
        Me.rdbWithParticular.Name = "rdbWithParticular"
        Me.rdbWithParticular.Size = New System.Drawing.Size(108, 17)
        Me.rdbWithParticular.TabIndex = 0
        Me.rdbWithParticular.TabStop = True
        Me.rdbWithParticular.Text = "With Particular"
        Me.rdbWithParticular.UseVisualStyleBackColor = True
        '
        'chkBankName
        '
        Me.chkBankName.AutoSize = True
        Me.chkBankName.Location = New System.Drawing.Point(25, 89)
        Me.chkBankName.Name = "chkBankName"
        Me.chkBankName.Size = New System.Drawing.Size(92, 17)
        Me.chkBankName.TabIndex = 4
        Me.chkBankName.Text = "Bank Name"
        Me.chkBankName.UseVisualStyleBackColor = True
        '
        'chkChequeDate
        '
        Me.chkChequeDate.AutoSize = True
        Me.chkChequeDate.Location = New System.Drawing.Point(25, 66)
        Me.chkChequeDate.Name = "chkChequeDate"
        Me.chkChequeDate.Size = New System.Drawing.Size(101, 17)
        Me.chkChequeDate.TabIndex = 3
        Me.chkChequeDate.Text = "Cheque Date"
        Me.chkChequeDate.UseVisualStyleBackColor = True
        '
        'chkRefDate
        '
        Me.chkRefDate.AutoSize = True
        Me.chkRefDate.Location = New System.Drawing.Point(224, 66)
        Me.chkRefDate.Name = "chkRefDate"
        Me.chkRefDate.Size = New System.Drawing.Size(72, 17)
        Me.chkRefDate.TabIndex = 9
        Me.chkRefDate.Text = "RefDate"
        Me.chkRefDate.UseVisualStyleBackColor = True
        '
        'chkRemark2
        '
        Me.chkRemark2.AutoSize = True
        Me.chkRemark2.Location = New System.Drawing.Point(140, 66)
        Me.chkRemark2.Name = "chkRemark2"
        Me.chkRemark2.Size = New System.Drawing.Size(78, 17)
        Me.chkRemark2.TabIndex = 6
        Me.chkRemark2.Text = "Remark2"
        Me.chkRemark2.UseVisualStyleBackColor = True
        '
        'chkRefNo
        '
        Me.chkRefNo.AutoSize = True
        Me.chkRefNo.Location = New System.Drawing.Point(224, 43)
        Me.chkRefNo.Name = "chkRefNo"
        Me.chkRefNo.Size = New System.Drawing.Size(64, 17)
        Me.chkRefNo.TabIndex = 8
        Me.chkRefNo.Text = "Ref No"
        Me.chkRefNo.UseVisualStyleBackColor = True
        '
        'chkRemark1
        '
        Me.chkRemark1.AutoSize = True
        Me.chkRemark1.Location = New System.Drawing.Point(140, 43)
        Me.chkRemark1.Name = "chkRemark1"
        Me.chkRemark1.Size = New System.Drawing.Size(78, 17)
        Me.chkRemark1.TabIndex = 5
        Me.chkRemark1.Text = "Remark1"
        Me.chkRemark1.UseVisualStyleBackColor = True
        '
        'chkChequeNo
        '
        Me.chkChequeNo.AutoSize = True
        Me.chkChequeNo.Location = New System.Drawing.Point(25, 43)
        Me.chkChequeNo.Name = "chkChequeNo"
        Me.chkChequeNo.Size = New System.Drawing.Size(89, 17)
        Me.chkChequeNo.TabIndex = 2
        Me.chkChequeNo.Text = "Cheque No"
        Me.chkChequeNo.UseVisualStyleBackColor = True
        '
        'chkEmp
        '
        Me.chkEmp.AutoSize = True
        Me.chkEmp.Location = New System.Drawing.Point(38, 482)
        Me.chkEmp.Name = "chkEmp"
        Me.chkEmp.Size = New System.Drawing.Size(84, 17)
        Me.chkEmp.TabIndex = 36
        Me.chkEmp.Text = "EmpName"
        Me.chkEmp.UseVisualStyleBackColor = True
        '
        'frmAccLedgerMore
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(350, 498)
        Me.ControlBox = False
        Me.Controls.Add(Me.chkEmp)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.chkContraAll)
        Me.Controls.Add(Me.lstContra)
        Me.Controls.Add(Me.txtFindAmount_AMT)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtFindNarration)
        Me.Controls.Add(Me.cmbFindAmount)
        Me.Controls.Add(Me.cmbVoucherType)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmAccLedgerMore"
        Me.ShowInTaskbar = False
        Me.Text = "Ledger view more options"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtFindAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtFindNarration As System.Windows.Forms.TextBox
    Friend WithEvents cmbFindAmount As System.Windows.Forms.ComboBox
    Friend WithEvents cmbVoucherType As System.Windows.Forms.ComboBox
    Friend WithEvents lstContra As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkContraAll As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents chkBankName As System.Windows.Forms.CheckBox
    Friend WithEvents chkChequeDate As System.Windows.Forms.CheckBox
    Friend WithEvents chkRemark2 As System.Windows.Forms.CheckBox
    Friend WithEvents chkRemark1 As System.Windows.Forms.CheckBox
    Friend WithEvents chkChequeNo As System.Windows.Forms.CheckBox
    Friend WithEvents chkRefDate As System.Windows.Forms.CheckBox
    Friend WithEvents chkRefNo As System.Windows.Forms.CheckBox
    Friend WithEvents chkUserName As System.Windows.Forms.CheckBox
    Friend WithEvents chkCostName As System.Windows.Forms.CheckBox
    Friend WithEvents rdbSepColumns As System.Windows.Forms.RadioButton
    Friend WithEvents rdbWithParticular As System.Windows.Forms.RadioButton
    Friend WithEvents chkSubledger As System.Windows.Forms.CheckBox
    Friend WithEvents chkDiscAuthEmp As System.Windows.Forms.CheckBox
    Friend WithEvents ChkBillPrefix As System.Windows.Forms.CheckBox
    Friend WithEvents chkEmp As CheckBox
    Friend WithEvents chkGstNo As CheckBox
    Friend WithEvents chkPanNo As CheckBox
End Class
