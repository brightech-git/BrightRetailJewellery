<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReceiptPaymentDetailss
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
        Me.components = New System.ComponentModel.Container()
        Me.grpContrainer = New System.Windows.Forms.GroupBox()
        Me.chkWithPaymode = New System.Windows.Forms.CheckBox()
        Me.chkDis = New System.Windows.Forms.CheckBox()
        Me.chkCompanySelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstCompany = New System.Windows.Forms.CheckedListBox()
        Me.chkPayModeSelectAll = New System.Windows.Forms.CheckBox()
        Me.ChkLstPaymodetype = New System.Windows.Forms.CheckedListBox()
        Me.chkCostCentreSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox()
        Me.txtSystemId = New System.Windows.Forms.TextBox()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChkOtherReceipts = New System.Windows.Forms.CheckBox()
        Me.ChkPurchaseSalesReturn = New System.Windows.Forms.CheckBox()
        Me.ChkCustomerAdvance = New System.Windows.Forms.CheckBox()
        Me.ChkAdvanceRepay = New System.Windows.Forms.CheckBox()
        Me.ChkCreditSales = New System.Windows.Forms.CheckBox()
        Me.ChkFurtherAdvance = New System.Windows.Forms.CheckBox()
        Me.ChkOrderAdvance = New System.Windows.Forms.CheckBox()
        Me.ChkOtherPayment = New System.Windows.Forms.CheckBox()
        Me.CheckBox4 = New System.Windows.Forms.CheckBox()
        Me.chkRepairAdvance = New System.Windows.Forms.CheckBox()
        Me.grpContrainer.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpContrainer
        '
        Me.grpContrainer.Controls.Add(Me.chkWithPaymode)
        Me.grpContrainer.Controls.Add(Me.chkDis)
        Me.grpContrainer.Controls.Add(Me.chkCompanySelectAll)
        Me.grpContrainer.Controls.Add(Me.chkLstCompany)
        Me.grpContrainer.Controls.Add(Me.chkPayModeSelectAll)
        Me.grpContrainer.Controls.Add(Me.ChkLstPaymodetype)
        Me.grpContrainer.Controls.Add(Me.chkCostCentreSelectAll)
        Me.grpContrainer.Controls.Add(Me.chkLstCostCentre)
        Me.grpContrainer.Controls.Add(Me.txtSystemId)
        Me.grpContrainer.Controls.Add(Me.btnExit)
        Me.grpContrainer.Controls.Add(Me.btnSearch)
        Me.grpContrainer.Controls.Add(Me.btnNew)
        Me.grpContrainer.Controls.Add(Me.dtpTo)
        Me.grpContrainer.Controls.Add(Me.dtpFrom)
        Me.grpContrainer.Controls.Add(Me.Label3)
        Me.grpContrainer.Controls.Add(Me.Label2)
        Me.grpContrainer.Controls.Add(Me.Label1)
        Me.grpContrainer.Location = New System.Drawing.Point(101, 12)
        Me.grpContrainer.Name = "grpContrainer"
        Me.grpContrainer.Size = New System.Drawing.Size(433, 468)
        Me.grpContrainer.TabIndex = 0
        Me.grpContrainer.TabStop = False
        '
        'chkWithPaymode
        '
        Me.chkWithPaymode.AutoSize = True
        Me.chkWithPaymode.Location = New System.Drawing.Point(211, 413)
        Me.chkWithPaymode.Name = "chkWithPaymode"
        Me.chkWithPaymode.Size = New System.Drawing.Size(108, 17)
        Me.chkWithPaymode.TabIndex = 13
        Me.chkWithPaymode.Text = "With Paymode"
        Me.chkWithPaymode.UseVisualStyleBackColor = True
        '
        'chkDis
        '
        Me.chkDis.AutoSize = True
        Me.chkDis.Location = New System.Drawing.Point(91, 413)
        Me.chkDis.Name = "chkDis"
        Me.chkDis.Size = New System.Drawing.Size(104, 17)
        Me.chkDis.TabIndex = 12
        Me.chkDis.Text = "With Discount"
        Me.chkDis.UseVisualStyleBackColor = True
        '
        'chkCompanySelectAll
        '
        Me.chkCompanySelectAll.AutoSize = True
        Me.chkCompanySelectAll.Location = New System.Drawing.Point(23, 49)
        Me.chkCompanySelectAll.Name = "chkCompanySelectAll"
        Me.chkCompanySelectAll.Size = New System.Drawing.Size(81, 17)
        Me.chkCompanySelectAll.TabIndex = 4
        Me.chkCompanySelectAll.Text = "Company"
        Me.chkCompanySelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCompany
        '
        Me.chkLstCompany.FormattingEnabled = True
        Me.chkLstCompany.Location = New System.Drawing.Point(20, 68)
        Me.chkLstCompany.Name = "chkLstCompany"
        Me.chkLstCompany.Size = New System.Drawing.Size(299, 84)
        Me.chkLstCompany.TabIndex = 5
        '
        'chkPayModeSelectAll
        '
        Me.chkPayModeSelectAll.AutoSize = True
        Me.chkPayModeSelectAll.Location = New System.Drawing.Point(19, 159)
        Me.chkPayModeSelectAll.Name = "chkPayModeSelectAll"
        Me.chkPayModeSelectAll.Size = New System.Drawing.Size(108, 17)
        Me.chkPayModeSelectAll.TabIndex = 6
        Me.chkPayModeSelectAll.Text = "PayMode Type"
        Me.chkPayModeSelectAll.UseVisualStyleBackColor = True
        '
        'ChkLstPaymodetype
        '
        Me.ChkLstPaymodetype.FormattingEnabled = True
        Me.ChkLstPaymodetype.Location = New System.Drawing.Point(19, 182)
        Me.ChkLstPaymodetype.Name = "ChkLstPaymodetype"
        Me.ChkLstPaymodetype.Size = New System.Drawing.Size(297, 100)
        Me.ChkLstPaymodetype.TabIndex = 7
        '
        'chkCostCentreSelectAll
        '
        Me.chkCostCentreSelectAll.AutoSize = True
        Me.chkCostCentreSelectAll.Location = New System.Drawing.Point(24, 291)
        Me.chkCostCentreSelectAll.Name = "chkCostCentreSelectAll"
        Me.chkCostCentreSelectAll.Size = New System.Drawing.Size(95, 17)
        Me.chkCostCentreSelectAll.TabIndex = 8
        Me.chkCostCentreSelectAll.Text = "Cost Centre"
        Me.chkCostCentreSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(22, 315)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(297, 68)
        Me.chkLstCostCentre.TabIndex = 9
        '
        'txtSystemId
        '
        Me.txtSystemId.Location = New System.Drawing.Point(91, 387)
        Me.txtSystemId.MaxLength = 3
        Me.txtSystemId.Name = "txtSystemId"
        Me.txtSystemId.Size = New System.Drawing.Size(121, 21)
        Me.txtSystemId.TabIndex = 11
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(250, 433)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 16
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(38, 433)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 14
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(144, 433)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 15
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(223, 26)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(89, 26)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(93, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(192, 30)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(20, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "To"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(19, 391)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(66, 13)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "System Id"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(19, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date From"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(119, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'ChkOtherReceipts
        '
        Me.ChkOtherReceipts.AutoSize = True
        Me.ChkOtherReceipts.Location = New System.Drawing.Point(581, 147)
        Me.ChkOtherReceipts.Name = "ChkOtherReceipts"
        Me.ChkOtherReceipts.Size = New System.Drawing.Size(110, 17)
        Me.ChkOtherReceipts.TabIndex = 15
        Me.ChkOtherReceipts.Text = "Other Receipts"
        Me.ChkOtherReceipts.UseVisualStyleBackColor = True
        Me.ChkOtherReceipts.Visible = False
        '
        'ChkPurchaseSalesReturn
        '
        Me.ChkPurchaseSalesReturn.AutoSize = True
        Me.ChkPurchaseSalesReturn.Location = New System.Drawing.Point(581, 216)
        Me.ChkPurchaseSalesReturn.Name = "ChkPurchaseSalesReturn"
        Me.ChkPurchaseSalesReturn.Size = New System.Drawing.Size(164, 17)
        Me.ChkPurchaseSalesReturn.TabIndex = 16
        Me.ChkPurchaseSalesReturn.Text = "Purchase \ Sales Return"
        Me.ChkPurchaseSalesReturn.UseVisualStyleBackColor = True
        Me.ChkPurchaseSalesReturn.Visible = False
        '
        'ChkCustomerAdvance
        '
        Me.ChkCustomerAdvance.AutoSize = True
        Me.ChkCustomerAdvance.Location = New System.Drawing.Point(581, 78)
        Me.ChkCustomerAdvance.Name = "ChkCustomerAdvance"
        Me.ChkCustomerAdvance.Size = New System.Drawing.Size(131, 17)
        Me.ChkCustomerAdvance.TabIndex = 14
        Me.ChkCustomerAdvance.Text = "CustomerAdvance"
        Me.ChkCustomerAdvance.UseVisualStyleBackColor = True
        Me.ChkCustomerAdvance.Visible = False
        '
        'ChkAdvanceRepay
        '
        Me.ChkAdvanceRepay.AutoSize = True
        Me.ChkAdvanceRepay.Location = New System.Drawing.Point(581, 101)
        Me.ChkAdvanceRepay.Name = "ChkAdvanceRepay"
        Me.ChkAdvanceRepay.Size = New System.Drawing.Size(115, 17)
        Me.ChkAdvanceRepay.TabIndex = 17
        Me.ChkAdvanceRepay.Text = "Advance Repay"
        Me.ChkAdvanceRepay.UseVisualStyleBackColor = True
        Me.ChkAdvanceRepay.Visible = False
        '
        'ChkCreditSales
        '
        Me.ChkCreditSales.AutoSize = True
        Me.ChkCreditSales.Location = New System.Drawing.Point(581, 193)
        Me.ChkCreditSales.Name = "ChkCreditSales"
        Me.ChkCreditSales.Size = New System.Drawing.Size(92, 17)
        Me.ChkCreditSales.TabIndex = 13
        Me.ChkCreditSales.Text = "CreditSales"
        Me.ChkCreditSales.UseVisualStyleBackColor = True
        Me.ChkCreditSales.Visible = False
        '
        'ChkFurtherAdvance
        '
        Me.ChkFurtherAdvance.AutoSize = True
        Me.ChkFurtherAdvance.Location = New System.Drawing.Point(581, 55)
        Me.ChkFurtherAdvance.Name = "ChkFurtherAdvance"
        Me.ChkFurtherAdvance.Size = New System.Drawing.Size(120, 17)
        Me.ChkFurtherAdvance.TabIndex = 11
        Me.ChkFurtherAdvance.Text = "Further Advance"
        Me.ChkFurtherAdvance.UseVisualStyleBackColor = True
        Me.ChkFurtherAdvance.Visible = False
        '
        'ChkOrderAdvance
        '
        Me.ChkOrderAdvance.AutoSize = True
        Me.ChkOrderAdvance.Location = New System.Drawing.Point(581, 124)
        Me.ChkOrderAdvance.Name = "ChkOrderAdvance"
        Me.ChkOrderAdvance.Size = New System.Drawing.Size(112, 17)
        Me.ChkOrderAdvance.TabIndex = 12
        Me.ChkOrderAdvance.Text = "Order Advance"
        Me.ChkOrderAdvance.UseVisualStyleBackColor = True
        Me.ChkOrderAdvance.Visible = False
        '
        'ChkOtherPayment
        '
        Me.ChkOtherPayment.AutoSize = True
        Me.ChkOtherPayment.Location = New System.Drawing.Point(581, 170)
        Me.ChkOtherPayment.Name = "ChkOtherPayment"
        Me.ChkOtherPayment.Size = New System.Drawing.Size(112, 17)
        Me.ChkOtherPayment.TabIndex = 18
        Me.ChkOtherPayment.Text = "Other Payment"
        Me.ChkOtherPayment.UseVisualStyleBackColor = True
        Me.ChkOtherPayment.Visible = False
        '
        'CheckBox4
        '
        Me.CheckBox4.AutoSize = True
        Me.CheckBox4.Location = New System.Drawing.Point(581, 239)
        Me.CheckBox4.Name = "CheckBox4"
        Me.CheckBox4.Size = New System.Drawing.Size(99, 17)
        Me.CheckBox4.TabIndex = 19
        Me.CheckBox4.Text = "Order Repay"
        Me.CheckBox4.UseVisualStyleBackColor = True
        Me.CheckBox4.Visible = False
        '
        'chkRepairAdvance
        '
        Me.chkRepairAdvance.AutoSize = True
        Me.chkRepairAdvance.Location = New System.Drawing.Point(581, 262)
        Me.chkRepairAdvance.Name = "chkRepairAdvance"
        Me.chkRepairAdvance.Size = New System.Drawing.Size(116, 17)
        Me.chkRepairAdvance.TabIndex = 20
        Me.chkRepairAdvance.Text = "Repair Advance"
        Me.chkRepairAdvance.UseVisualStyleBackColor = True
        Me.chkRepairAdvance.Visible = False
        '
        'frmReceiptPaymentDetailss
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(632, 534)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.chkRepairAdvance)
        Me.Controls.Add(Me.grpContrainer)
        Me.Controls.Add(Me.ChkFurtherAdvance)
        Me.Controls.Add(Me.CheckBox4)
        Me.Controls.Add(Me.ChkOrderAdvance)
        Me.Controls.Add(Me.ChkOtherPayment)
        Me.Controls.Add(Me.ChkOtherReceipts)
        Me.Controls.Add(Me.ChkCreditSales)
        Me.Controls.Add(Me.ChkAdvanceRepay)
        Me.Controls.Add(Me.ChkCustomerAdvance)
        Me.Controls.Add(Me.ChkPurchaseSalesReturn)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmReceiptPaymentDetailss"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Receipt Payment Details"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpContrainer.ResumeLayout(False)
        Me.grpContrainer.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents grpContrainer As System.Windows.Forms.GroupBox
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkCostCentreSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtSystemId As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkLstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkPayModeSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents ChkLstPaymodetype As System.Windows.Forms.CheckedListBox
    Friend WithEvents ChkOtherReceipts As System.Windows.Forms.CheckBox
    Friend WithEvents ChkPurchaseSalesReturn As System.Windows.Forms.CheckBox
    Friend WithEvents ChkCustomerAdvance As System.Windows.Forms.CheckBox
    Friend WithEvents ChkAdvanceRepay As System.Windows.Forms.CheckBox
    Friend WithEvents ChkCreditSales As System.Windows.Forms.CheckBox
    Friend WithEvents ChkFurtherAdvance As System.Windows.Forms.CheckBox
    Friend WithEvents ChkOrderAdvance As System.Windows.Forms.CheckBox
    Friend WithEvents ChkOtherPayment As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox4 As System.Windows.Forms.CheckBox
    Friend WithEvents chkCompanySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCompany As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkDis As System.Windows.Forms.CheckBox
    Friend WithEvents chkRepairAdvance As CheckBox
    Friend WithEvents chkWithPaymode As CheckBox
End Class
