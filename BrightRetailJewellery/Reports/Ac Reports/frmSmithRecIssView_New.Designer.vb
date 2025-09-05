<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSmithRecIssView_New
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
        Me.pnlContainer = New System.Windows.Forms.Panel()
        Me.ChkCmbStuddedSubitem = New BrighttechPack.CheckedComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.chkLstStuddedItem = New System.Windows.Forms.CheckedListBox()
        Me.chkStuddedOnly = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.rbtAcAll = New System.Windows.Forms.RadioButton()
        Me.rbtOthers = New System.Windows.Forms.RadioButton()
        Me.rbtDealer = New System.Windows.Forms.RadioButton()
        Me.rbtSmith = New System.Windows.Forms.RadioButton()
        Me.CmbSmithname = New System.Windows.Forms.ComboBox()
        Me.ChkMultiSmith = New System.Windows.Forms.CheckBox()
        Me.rbtBoth = New System.Windows.Forms.RadioButton()
        Me.chkcmbTranType = New BrighttechPack.CheckedComboBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.chkSmith = New System.Windows.Forms.CheckBox()
        Me.chkLstSmith = New System.Windows.Forms.CheckedListBox()
        Me.rbtReceipt = New System.Windows.Forms.RadioButton()
        Me.rbtIssue = New System.Windows.Forms.RadioButton()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkCostCentreSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox()
        Me.chkLstCategory = New System.Windows.Forms.CheckedListBox()
        Me.chkMetalSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkCategorySelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstMetal = New System.Windows.Forms.CheckedListBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.pnlOrder = New System.Windows.Forms.Panel()
        Me.rbtTranNo = New System.Windows.Forms.RadioButton()
        Me.rbtDateNO = New System.Windows.Forms.RadioButton()
        Me.Chktran = New System.Windows.Forms.CheckBox()
        Me.chkCancel = New System.Windows.Forms.CheckBox()
        Me.PnlMark = New System.Windows.Forms.Panel()
        Me.rbtsummary = New System.Windows.Forms.RadioButton()
        Me.rbtDetailed = New System.Windows.Forms.RadioButton()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.ChkGrpCategory = New System.Windows.Forms.CheckBox()
        Me.ChkApproval = New System.Windows.Forms.CheckBox()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.chkStuddedItemChkall = New System.Windows.Forms.CheckBox()
        Me.pnlContainer.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pnlOrder.SuspendLayout()
        Me.PnlMark.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlContainer
        '
        Me.pnlContainer.Controls.Add(Me.chkStuddedItemChkall)
        Me.pnlContainer.Controls.Add(Me.ChkCmbStuddedSubitem)
        Me.pnlContainer.Controls.Add(Me.Label4)
        Me.pnlContainer.Controls.Add(Me.chkLstStuddedItem)
        Me.pnlContainer.Controls.Add(Me.chkStuddedOnly)
        Me.pnlContainer.Controls.Add(Me.GroupBox1)
        Me.pnlContainer.Controls.Add(Me.CmbSmithname)
        Me.pnlContainer.Controls.Add(Me.ChkMultiSmith)
        Me.pnlContainer.Controls.Add(Me.rbtBoth)
        Me.pnlContainer.Controls.Add(Me.chkcmbTranType)
        Me.pnlContainer.Controls.Add(Me.dtpTo)
        Me.pnlContainer.Controls.Add(Me.dtpFrom)
        Me.pnlContainer.Controls.Add(Me.chkSmith)
        Me.pnlContainer.Controls.Add(Me.chkLstSmith)
        Me.pnlContainer.Controls.Add(Me.rbtReceipt)
        Me.pnlContainer.Controls.Add(Me.rbtIssue)
        Me.pnlContainer.Controls.Add(Me.Label2)
        Me.pnlContainer.Controls.Add(Me.Label3)
        Me.pnlContainer.Controls.Add(Me.Label1)
        Me.pnlContainer.Controls.Add(Me.chkCostCentreSelectAll)
        Me.pnlContainer.Controls.Add(Me.chkLstCostCentre)
        Me.pnlContainer.Controls.Add(Me.chkLstCategory)
        Me.pnlContainer.Controls.Add(Me.chkMetalSelectAll)
        Me.pnlContainer.Controls.Add(Me.chkCategorySelectAll)
        Me.pnlContainer.Controls.Add(Me.chkLstMetal)
        Me.pnlContainer.Controls.Add(Me.Panel1)
        Me.pnlContainer.Location = New System.Drawing.Point(181, 12)
        Me.pnlContainer.Name = "pnlContainer"
        Me.pnlContainer.Size = New System.Drawing.Size(431, 568)
        Me.pnlContainer.TabIndex = 0
        '
        'ChkCmbStuddedSubitem
        '
        Me.ChkCmbStuddedSubitem.CheckOnClick = True
        Me.ChkCmbStuddedSubitem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.ChkCmbStuddedSubitem.DropDownHeight = 1
        Me.ChkCmbStuddedSubitem.FormattingEnabled = True
        Me.ChkCmbStuddedSubitem.IntegralHeight = False
        Me.ChkCmbStuddedSubitem.Location = New System.Drawing.Point(131, 268)
        Me.ChkCmbStuddedSubitem.Name = "ChkCmbStuddedSubitem"
        Me.ChkCmbStuddedSubitem.Size = New System.Drawing.Size(289, 22)
        Me.ChkCmbStuddedSubitem.TabIndex = 18
        Me.ChkCmbStuddedSubitem.ValueSeparator = ", "
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(18, 271)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(107, 13)
        Me.Label4.TabIndex = 17
        Me.Label4.Text = "Studded SubItem"
        '
        'chkLstStuddedItem
        '
        Me.chkLstStuddedItem.FormattingEnabled = True
        Me.chkLstStuddedItem.Location = New System.Drawing.Point(220, 194)
        Me.chkLstStuddedItem.Name = "chkLstStuddedItem"
        Me.chkLstStuddedItem.Size = New System.Drawing.Size(201, 68)
        Me.chkLstStuddedItem.TabIndex = 16
        '
        'chkStuddedOnly
        '
        Me.chkStuddedOnly.AutoSize = True
        Me.chkStuddedOnly.Location = New System.Drawing.Point(223, 177)
        Me.chkStuddedOnly.Name = "chkStuddedOnly"
        Me.chkStuddedOnly.Size = New System.Drawing.Size(103, 17)
        Me.chkStuddedOnly.TabIndex = 15
        Me.chkStuddedOnly.Text = "Studded Only"
        Me.chkStuddedOnly.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbtAcAll)
        Me.GroupBox1.Controls.Add(Me.rbtOthers)
        Me.GroupBox1.Controls.Add(Me.rbtDealer)
        Me.GroupBox1.Controls.Add(Me.rbtSmith)
        Me.GroupBox1.Location = New System.Drawing.Point(15, 291)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(398, 36)
        Me.GroupBox1.TabIndex = 19
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Account Type"
        '
        'rbtAcAll
        '
        Me.rbtAcAll.AutoSize = True
        Me.rbtAcAll.Checked = True
        Me.rbtAcAll.Location = New System.Drawing.Point(40, 14)
        Me.rbtAcAll.Name = "rbtAcAll"
        Me.rbtAcAll.Size = New System.Drawing.Size(39, 17)
        Me.rbtAcAll.TabIndex = 0
        Me.rbtAcAll.TabStop = True
        Me.rbtAcAll.Text = "All"
        Me.rbtAcAll.UseVisualStyleBackColor = True
        '
        'rbtOthers
        '
        Me.rbtOthers.AutoSize = True
        Me.rbtOthers.Location = New System.Drawing.Point(314, 14)
        Me.rbtOthers.Name = "rbtOthers"
        Me.rbtOthers.Size = New System.Drawing.Size(63, 17)
        Me.rbtOthers.TabIndex = 3
        Me.rbtOthers.TabStop = True
        Me.rbtOthers.Text = "Others"
        Me.rbtOthers.UseVisualStyleBackColor = True
        '
        'rbtDealer
        '
        Me.rbtDealer.AutoSize = True
        Me.rbtDealer.Location = New System.Drawing.Point(213, 14)
        Me.rbtDealer.Name = "rbtDealer"
        Me.rbtDealer.Size = New System.Drawing.Size(63, 17)
        Me.rbtDealer.TabIndex = 2
        Me.rbtDealer.TabStop = True
        Me.rbtDealer.Text = "Dealer"
        Me.rbtDealer.UseVisualStyleBackColor = True
        '
        'rbtSmith
        '
        Me.rbtSmith.AutoSize = True
        Me.rbtSmith.Location = New System.Drawing.Point(117, 14)
        Me.rbtSmith.Name = "rbtSmith"
        Me.rbtSmith.Size = New System.Drawing.Size(58, 17)
        Me.rbtSmith.TabIndex = 1
        Me.rbtSmith.TabStop = True
        Me.rbtSmith.Text = "Smith"
        Me.rbtSmith.UseVisualStyleBackColor = True
        '
        'CmbSmithname
        '
        Me.CmbSmithname.FormattingEnabled = True
        Me.CmbSmithname.Location = New System.Drawing.Point(18, 369)
        Me.CmbSmithname.Name = "CmbSmithname"
        Me.CmbSmithname.Size = New System.Drawing.Size(401, 21)
        Me.CmbSmithname.TabIndex = 23
        Me.CmbSmithname.Visible = False
        '
        'ChkMultiSmith
        '
        Me.ChkMultiSmith.AutoSize = True
        Me.ChkMultiSmith.Checked = True
        Me.ChkMultiSmith.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkMultiSmith.Location = New System.Drawing.Point(120, 332)
        Me.ChkMultiSmith.Name = "ChkMultiSmith"
        Me.ChkMultiSmith.Size = New System.Drawing.Size(87, 17)
        Me.ChkMultiSmith.TabIndex = 21
        Me.ChkMultiSmith.Text = "MultiSelect"
        Me.ChkMultiSmith.UseVisualStyleBackColor = True
        '
        'rbtBoth
        '
        Me.rbtBoth.AutoSize = True
        Me.rbtBoth.Location = New System.Drawing.Point(233, 8)
        Me.rbtBoth.Name = "rbtBoth"
        Me.rbtBoth.Size = New System.Drawing.Size(51, 17)
        Me.rbtBoth.TabIndex = 2
        Me.rbtBoth.TabStop = True
        Me.rbtBoth.Text = "Both"
        Me.rbtBoth.UseVisualStyleBackColor = True
        '
        'chkcmbTranType
        '
        Me.chkcmbTranType.CheckOnClick = True
        Me.chkcmbTranType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbTranType.DropDownHeight = 1
        Me.chkcmbTranType.FormattingEnabled = True
        Me.chkcmbTranType.IntegralHeight = False
        Me.chkcmbTranType.Location = New System.Drawing.Point(82, 28)
        Me.chkcmbTranType.Name = "chkcmbTranType"
        Me.chkcmbTranType.Size = New System.Drawing.Size(227, 22)
        Me.chkcmbTranType.TabIndex = 4
        Me.chkcmbTranType.ValueSeparator = ", "
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(216, 56)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpTo.TabIndex = 8
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(82, 56)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(93, 21)
        Me.dtpFrom.TabIndex = 6
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'chkSmith
        '
        Me.chkSmith.AutoSize = True
        Me.chkSmith.Location = New System.Drawing.Point(18, 332)
        Me.chkSmith.Name = "chkSmith"
        Me.chkSmith.Size = New System.Drawing.Size(40, 17)
        Me.chkSmith.TabIndex = 20
        Me.chkSmith.Text = "All"
        Me.chkSmith.UseVisualStyleBackColor = True
        '
        'chkLstSmith
        '
        Me.chkLstSmith.FormattingEnabled = True
        Me.chkLstSmith.Location = New System.Drawing.Point(15, 352)
        Me.chkLstSmith.Name = "chkLstSmith"
        Me.chkLstSmith.Size = New System.Drawing.Size(407, 68)
        Me.chkLstSmith.TabIndex = 22
        '
        'rbtReceipt
        '
        Me.rbtReceipt.AutoSize = True
        Me.rbtReceipt.Location = New System.Drawing.Point(152, 8)
        Me.rbtReceipt.Name = "rbtReceipt"
        Me.rbtReceipt.Size = New System.Drawing.Size(67, 17)
        Me.rbtReceipt.TabIndex = 1
        Me.rbtReceipt.TabStop = True
        Me.rbtReceipt.Text = "Receipt"
        Me.rbtReceipt.UseVisualStyleBackColor = True
        '
        'rbtIssue
        '
        Me.rbtIssue.AutoSize = True
        Me.rbtIssue.Location = New System.Drawing.Point(82, 8)
        Me.rbtIssue.Name = "rbtIssue"
        Me.rbtIssue.Size = New System.Drawing.Size(56, 17)
        Me.rbtIssue.TabIndex = 0
        Me.rbtIssue.TabStop = True
        Me.rbtIssue.Text = "Issue"
        Me.rbtIssue.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(14, 31)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Tran Type"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(185, 60)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(20, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "To"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 59)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Date From"
        '
        'chkCostCentreSelectAll
        '
        Me.chkCostCentreSelectAll.AutoSize = True
        Me.chkCostCentreSelectAll.Location = New System.Drawing.Point(15, 84)
        Me.chkCostCentreSelectAll.Name = "chkCostCentreSelectAll"
        Me.chkCostCentreSelectAll.Size = New System.Drawing.Size(95, 17)
        Me.chkCostCentreSelectAll.TabIndex = 9
        Me.chkCostCentreSelectAll.Text = "Cost Centre"
        Me.chkCostCentreSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(12, 103)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(201, 68)
        Me.chkLstCostCentre.TabIndex = 11
        '
        'chkLstCategory
        '
        Me.chkLstCategory.FormattingEnabled = True
        Me.chkLstCategory.Location = New System.Drawing.Point(12, 194)
        Me.chkLstCategory.Name = "chkLstCategory"
        Me.chkLstCategory.Size = New System.Drawing.Size(201, 68)
        Me.chkLstCategory.TabIndex = 14
        '
        'chkMetalSelectAll
        '
        Me.chkMetalSelectAll.AutoSize = True
        Me.chkMetalSelectAll.Location = New System.Drawing.Point(223, 84)
        Me.chkMetalSelectAll.Name = "chkMetalSelectAll"
        Me.chkMetalSelectAll.Size = New System.Drawing.Size(56, 17)
        Me.chkMetalSelectAll.TabIndex = 10
        Me.chkMetalSelectAll.Text = "Metal"
        Me.chkMetalSelectAll.UseVisualStyleBackColor = True
        '
        'chkCategorySelectAll
        '
        Me.chkCategorySelectAll.AutoSize = True
        Me.chkCategorySelectAll.Location = New System.Drawing.Point(15, 177)
        Me.chkCategorySelectAll.Name = "chkCategorySelectAll"
        Me.chkCategorySelectAll.Size = New System.Drawing.Size(79, 17)
        Me.chkCategorySelectAll.TabIndex = 13
        Me.chkCategorySelectAll.Text = "Category"
        Me.chkCategorySelectAll.UseVisualStyleBackColor = True
        '
        'chkLstMetal
        '
        Me.chkLstMetal.FormattingEnabled = True
        Me.chkLstMetal.Location = New System.Drawing.Point(220, 103)
        Me.chkLstMetal.Name = "chkLstMetal"
        Me.chkLstMetal.Size = New System.Drawing.Size(199, 68)
        Me.chkLstMetal.TabIndex = 12
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.pnlOrder)
        Me.Panel1.Controls.Add(Me.Chktran)
        Me.Panel1.Controls.Add(Me.chkCancel)
        Me.Panel1.Controls.Add(Me.PnlMark)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.btnSearch)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.ChkGrpCategory)
        Me.Panel1.Controls.Add(Me.ChkApproval)
        Me.Panel1.Location = New System.Drawing.Point(18, 427)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(401, 138)
        Me.Panel1.TabIndex = 24
        '
        'pnlOrder
        '
        Me.pnlOrder.Controls.Add(Me.rbtTranNo)
        Me.pnlOrder.Controls.Add(Me.rbtDateNO)
        Me.pnlOrder.Location = New System.Drawing.Point(14, 75)
        Me.pnlOrder.Name = "pnlOrder"
        Me.pnlOrder.Size = New System.Drawing.Size(363, 22)
        Me.pnlOrder.TabIndex = 5
        '
        'rbtTranNo
        '
        Me.rbtTranNo.AutoSize = True
        Me.rbtTranNo.Location = New System.Drawing.Point(187, 4)
        Me.rbtTranNo.Name = "rbtTranNo"
        Me.rbtTranNo.Size = New System.Drawing.Size(123, 17)
        Me.rbtTranNo.TabIndex = 1
        Me.rbtTranNo.Text = "Order By TranNO"
        Me.rbtTranNo.UseVisualStyleBackColor = True
        '
        'rbtDateNO
        '
        Me.rbtDateNO.AutoSize = True
        Me.rbtDateNO.Checked = True
        Me.rbtDateNO.Location = New System.Drawing.Point(7, 4)
        Me.rbtDateNO.Name = "rbtDateNO"
        Me.rbtDateNO.Size = New System.Drawing.Size(133, 17)
        Me.rbtDateNO.TabIndex = 0
        Me.rbtDateNO.TabStop = True
        Me.rbtDateNO.Text = "Order By TranDate"
        Me.rbtDateNO.UseVisualStyleBackColor = True
        '
        'Chktran
        '
        Me.Chktran.AutoSize = True
        Me.Chktran.Enabled = False
        Me.Chktran.Location = New System.Drawing.Point(200, 38)
        Me.Chktran.Name = "Chktran"
        Me.Chktran.Size = New System.Drawing.Size(154, 17)
        Me.Chktran.TabIndex = 2
        Me.Chktran.Text = "With Tranno Summary"
        Me.Chktran.UseVisualStyleBackColor = True
        '
        'chkCancel
        '
        Me.chkCancel.AutoSize = True
        Me.chkCancel.Location = New System.Drawing.Point(17, 38)
        Me.chkCancel.Name = "chkCancel"
        Me.chkCancel.Size = New System.Drawing.Size(94, 17)
        Me.chkCancel.TabIndex = 1
        Me.chkCancel.Text = "With Cancel"
        Me.chkCancel.UseVisualStyleBackColor = True
        '
        'PnlMark
        '
        Me.PnlMark.Controls.Add(Me.rbtsummary)
        Me.PnlMark.Controls.Add(Me.rbtDetailed)
        Me.PnlMark.Location = New System.Drawing.Point(14, 6)
        Me.PnlMark.Name = "PnlMark"
        Me.PnlMark.Size = New System.Drawing.Size(203, 22)
        Me.PnlMark.TabIndex = 0
        '
        'rbtsummary
        '
        Me.rbtsummary.AutoSize = True
        Me.rbtsummary.Location = New System.Drawing.Point(108, 4)
        Me.rbtsummary.Name = "rbtsummary"
        Me.rbtsummary.Size = New System.Drawing.Size(81, 17)
        Me.rbtsummary.TabIndex = 1
        Me.rbtsummary.Text = "Summary"
        Me.rbtsummary.UseVisualStyleBackColor = True
        '
        'rbtDetailed
        '
        Me.rbtDetailed.AutoSize = True
        Me.rbtDetailed.Checked = True
        Me.rbtDetailed.Location = New System.Drawing.Point(7, 4)
        Me.rbtDetailed.Name = "rbtDetailed"
        Me.rbtDetailed.Size = New System.Drawing.Size(72, 17)
        Me.rbtDetailed.TabIndex = 0
        Me.rbtDetailed.TabStop = True
        Me.rbtDetailed.Text = "Detailed"
        Me.rbtDetailed.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(117, 103)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 7
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(11, 103)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 6
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(223, 103)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 8
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'ChkGrpCategory
        '
        Me.ChkGrpCategory.AutoSize = True
        Me.ChkGrpCategory.Location = New System.Drawing.Point(200, 61)
        Me.ChkGrpCategory.Name = "ChkGrpCategory"
        Me.ChkGrpCategory.Size = New System.Drawing.Size(137, 17)
        Me.ChkGrpCategory.TabIndex = 4
        Me.ChkGrpCategory.Text = "Group By Category"
        Me.ChkGrpCategory.UseVisualStyleBackColor = True
        '
        'ChkApproval
        '
        Me.ChkApproval.AutoSize = True
        Me.ChkApproval.Enabled = False
        Me.ChkApproval.Location = New System.Drawing.Point(17, 61)
        Me.ChkApproval.Name = "ChkApproval"
        Me.ChkApproval.Size = New System.Drawing.Size(175, 17)
        Me.ChkApproval.TabIndex = 3
        Me.ChkApproval.Text = "With Approval Transaction"
        Me.ChkApproval.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'chkStuddedItemChkall
        '
        Me.chkStuddedItemChkall.AutoSize = True
        Me.chkStuddedItemChkall.Location = New System.Drawing.Point(328, 177)
        Me.chkStuddedItemChkall.Name = "chkStuddedItemChkall"
        Me.chkStuddedItemChkall.Size = New System.Drawing.Size(80, 17)
        Me.chkStuddedItemChkall.TabIndex = 25
        Me.chkStuddedItemChkall.Text = "Check All"
        Me.chkStuddedItemChkall.UseVisualStyleBackColor = True
        '
        'frmSmithRecIssView_New
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(805, 581)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.pnlContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmSmithRecIssView_New"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Smith Issue Receipt View"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlContainer.ResumeLayout(False)
        Me.pnlContainer.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlOrder.ResumeLayout(False)
        Me.pnlOrder.PerformLayout()
        Me.PnlMark.ResumeLayout(False)
        Me.PnlMark.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlContainer As System.Windows.Forms.Panel
    Friend WithEvents rbtReceipt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtIssue As System.Windows.Forms.RadioButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkCostCentreSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkLstCategory As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkMetalSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkCategorySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstMetal As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkSmith As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstSmith As System.Windows.Forms.CheckedListBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkcmbTranType As BrighttechPack.CheckedComboBox
    Friend WithEvents PnlMark As System.Windows.Forms.Panel
    Friend WithEvents rbtsummary As System.Windows.Forms.RadioButton
    Friend WithEvents rbtDetailed As System.Windows.Forms.RadioButton
    Friend WithEvents chkCancel As System.Windows.Forms.CheckBox
    Friend WithEvents ChkApproval As System.Windows.Forms.CheckBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents rbtBoth As System.Windows.Forms.RadioButton
    Friend WithEvents ChkGrpCategory As System.Windows.Forms.CheckBox
    Friend WithEvents ChkMultiSmith As System.Windows.Forms.CheckBox
    Friend WithEvents CmbSmithname As System.Windows.Forms.ComboBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Chktran As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbtAcAll As System.Windows.Forms.RadioButton
    Friend WithEvents rbtOthers As System.Windows.Forms.RadioButton
    Friend WithEvents rbtDealer As System.Windows.Forms.RadioButton
    Friend WithEvents rbtSmith As System.Windows.Forms.RadioButton
    Friend WithEvents pnlOrder As System.Windows.Forms.Panel
    Friend WithEvents rbtTranNo As System.Windows.Forms.RadioButton
    Friend WithEvents rbtDateNO As System.Windows.Forms.RadioButton
    Friend WithEvents chkLstStuddedItem As CheckedListBox
    Friend WithEvents chkStuddedOnly As CheckBox
    Friend WithEvents Label4 As Label
    Friend WithEvents ChkCmbStuddedSubitem As BrighttechPack.CheckedComboBox
    Friend WithEvents chkStuddedItemChkall As CheckBox
End Class
