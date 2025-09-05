<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSmithRecIssView
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
        Me.chkcmbProcessType = New BrighttechPack.CheckedComboBox()
        Me.Label62 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.rbtAcAll = New System.Windows.Forms.RadioButton()
        Me.rbtOthers = New System.Windows.Forms.RadioButton()
        Me.rbtDealer = New System.Windows.Forms.RadioButton()
        Me.rbtSmith = New System.Windows.Forms.RadioButton()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.PnlCheckType = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.rbtIssRecUnMarked = New System.Windows.Forms.RadioButton()
        Me.rbtIssRecMarked = New System.Windows.Forms.RadioButton()
        Me.rbtIssRecBoth = New System.Windows.Forms.RadioButton()
        Me.chkRateCut = New System.Windows.Forms.CheckBox()
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
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlContainer.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.PnlCheckType.SuspendLayout()
        Me.pnlOrder.SuspendLayout()
        Me.PnlMark.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlContainer
        '
        Me.pnlContainer.Controls.Add(Me.chkcmbProcessType)
        Me.pnlContainer.Controls.Add(Me.Label62)
        Me.pnlContainer.Controls.Add(Me.GroupBox1)
        Me.pnlContainer.Controls.Add(Me.Panel1)
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
        Me.pnlContainer.Location = New System.Drawing.Point(181, 12)
        Me.pnlContainer.Name = "pnlContainer"
        Me.pnlContainer.Size = New System.Drawing.Size(431, 568)
        Me.pnlContainer.TabIndex = 0
        '
        'chkcmbProcessType
        '
        Me.chkcmbProcessType.CheckOnClick = True
        Me.chkcmbProcessType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbProcessType.DropDownHeight = 1
        Me.chkcmbProcessType.FormattingEnabled = True
        Me.chkcmbProcessType.IntegralHeight = False
        Me.chkcmbProcessType.Location = New System.Drawing.Point(82, 62)
        Me.chkcmbProcessType.Name = "chkcmbProcessType"
        Me.chkcmbProcessType.Size = New System.Drawing.Size(227, 22)
        Me.chkcmbProcessType.TabIndex = 6
        Me.chkcmbProcessType.ValueSeparator = ", "
        '
        'Label62
        '
        Me.Label62.AutoSize = True
        Me.Label62.Location = New System.Drawing.Point(12, 65)
        Me.Label62.Name = "Label62"
        Me.Label62.Size = New System.Drawing.Size(51, 13)
        Me.Label62.TabIndex = 5
        Me.Label62.Text = "Process"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbtAcAll)
        Me.GroupBox1.Controls.Add(Me.rbtOthers)
        Me.GroupBox1.Controls.Add(Me.rbtDealer)
        Me.GroupBox1.Controls.Add(Me.rbtSmith)
        Me.GroupBox1.Location = New System.Drawing.Point(15, 274)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(398, 36)
        Me.GroupBox1.TabIndex = 17
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
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.PnlCheckType)
        Me.Panel1.Controls.Add(Me.chkRateCut)
        Me.Panel1.Controls.Add(Me.pnlOrder)
        Me.Panel1.Controls.Add(Me.Chktran)
        Me.Panel1.Controls.Add(Me.chkCancel)
        Me.Panel1.Controls.Add(Me.PnlMark)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.btnSearch)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.ChkGrpCategory)
        Me.Panel1.Controls.Add(Me.ChkApproval)
        Me.Panel1.Location = New System.Drawing.Point(15, 408)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(401, 157)
        Me.Panel1.TabIndex = 22
        '
        'PnlCheckType
        '
        Me.PnlCheckType.Controls.Add(Me.Label4)
        Me.PnlCheckType.Controls.Add(Me.rbtIssRecUnMarked)
        Me.PnlCheckType.Controls.Add(Me.rbtIssRecMarked)
        Me.PnlCheckType.Controls.Add(Me.rbtIssRecBoth)
        Me.PnlCheckType.Location = New System.Drawing.Point(14, 94)
        Me.PnlCheckType.Name = "PnlCheckType"
        Me.PnlCheckType.Size = New System.Drawing.Size(363, 22)
        Me.PnlCheckType.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(4, 4)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(74, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Check Type"
        '
        'rbtIssRecUnMarked
        '
        Me.rbtIssRecUnMarked.AutoSize = True
        Me.rbtIssRecUnMarked.Location = New System.Drawing.Point(258, 2)
        Me.rbtIssRecUnMarked.Name = "rbtIssRecUnMarked"
        Me.rbtIssRecUnMarked.Size = New System.Drawing.Size(82, 17)
        Me.rbtIssRecUnMarked.TabIndex = 3
        Me.rbtIssRecUnMarked.Text = "UnMarked"
        Me.rbtIssRecUnMarked.UseVisualStyleBackColor = True
        '
        'rbtIssRecMarked
        '
        Me.rbtIssRecMarked.AutoSize = True
        Me.rbtIssRecMarked.Location = New System.Drawing.Point(168, 3)
        Me.rbtIssRecMarked.Name = "rbtIssRecMarked"
        Me.rbtIssRecMarked.Size = New System.Drawing.Size(67, 17)
        Me.rbtIssRecMarked.TabIndex = 2
        Me.rbtIssRecMarked.Text = "Marked"
        Me.rbtIssRecMarked.UseVisualStyleBackColor = True
        '
        'rbtIssRecBoth
        '
        Me.rbtIssRecBoth.AutoSize = True
        Me.rbtIssRecBoth.Checked = True
        Me.rbtIssRecBoth.Location = New System.Drawing.Point(94, 3)
        Me.rbtIssRecBoth.Name = "rbtIssRecBoth"
        Me.rbtIssRecBoth.Size = New System.Drawing.Size(51, 17)
        Me.rbtIssRecBoth.TabIndex = 1
        Me.rbtIssRecBoth.TabStop = True
        Me.rbtIssRecBoth.Text = "Both"
        Me.rbtIssRecBoth.UseVisualStyleBackColor = True
        '
        'chkRateCut
        '
        Me.chkRateCut.AutoSize = True
        Me.chkRateCut.Enabled = False
        Me.chkRateCut.Location = New System.Drawing.Point(273, 6)
        Me.chkRateCut.Name = "chkRateCut"
        Me.chkRateCut.Size = New System.Drawing.Size(104, 17)
        Me.chkRateCut.TabIndex = 1
        Me.chkRateCut.Text = "Rate Cut only"
        Me.chkRateCut.UseVisualStyleBackColor = True
        '
        'pnlOrder
        '
        Me.pnlOrder.Controls.Add(Me.rbtTranNo)
        Me.pnlOrder.Controls.Add(Me.rbtDateNO)
        Me.pnlOrder.Location = New System.Drawing.Point(14, 66)
        Me.pnlOrder.Name = "pnlOrder"
        Me.pnlOrder.Size = New System.Drawing.Size(363, 22)
        Me.pnlOrder.TabIndex = 6
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
        Me.Chktran.Location = New System.Drawing.Point(200, 26)
        Me.Chktran.Name = "Chktran"
        Me.Chktran.Size = New System.Drawing.Size(154, 17)
        Me.Chktran.TabIndex = 3
        Me.Chktran.Text = "With Tranno Summary"
        Me.Chktran.UseVisualStyleBackColor = True
        '
        'chkCancel
        '
        Me.chkCancel.AutoSize = True
        Me.chkCancel.Location = New System.Drawing.Point(14, 26)
        Me.chkCancel.Name = "chkCancel"
        Me.chkCancel.Size = New System.Drawing.Size(94, 17)
        Me.chkCancel.TabIndex = 2
        Me.chkCancel.Text = "With Cancel"
        Me.chkCancel.UseVisualStyleBackColor = True
        '
        'PnlMark
        '
        Me.PnlMark.Controls.Add(Me.rbtsummary)
        Me.PnlMark.Controls.Add(Me.rbtDetailed)
        Me.PnlMark.Location = New System.Drawing.Point(14, 3)
        Me.PnlMark.Name = "PnlMark"
        Me.PnlMark.Size = New System.Drawing.Size(249, 22)
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
        Me.btnNew.Location = New System.Drawing.Point(144, 119)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 9
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(38, 119)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 8
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(249, 119)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 10
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'ChkGrpCategory
        '
        Me.ChkGrpCategory.AutoSize = True
        Me.ChkGrpCategory.Location = New System.Drawing.Point(200, 49)
        Me.ChkGrpCategory.Name = "ChkGrpCategory"
        Me.ChkGrpCategory.Size = New System.Drawing.Size(137, 17)
        Me.ChkGrpCategory.TabIndex = 5
        Me.ChkGrpCategory.Text = "Group By Category"
        Me.ChkGrpCategory.UseVisualStyleBackColor = True
        '
        'ChkApproval
        '
        Me.ChkApproval.AutoSize = True
        Me.ChkApproval.Enabled = False
        Me.ChkApproval.Location = New System.Drawing.Point(14, 49)
        Me.ChkApproval.Name = "ChkApproval"
        Me.ChkApproval.Size = New System.Drawing.Size(175, 17)
        Me.ChkApproval.TabIndex = 4
        Me.ChkApproval.Text = "With Approval Transaction"
        Me.ChkApproval.UseVisualStyleBackColor = True
        '
        'CmbSmithname
        '
        Me.CmbSmithname.FormattingEnabled = True
        Me.CmbSmithname.Location = New System.Drawing.Point(15, 351)
        Me.CmbSmithname.Name = "CmbSmithname"
        Me.CmbSmithname.Size = New System.Drawing.Size(401, 21)
        Me.CmbSmithname.TabIndex = 21
        Me.CmbSmithname.Visible = False
        '
        'ChkMultiSmith
        '
        Me.ChkMultiSmith.AutoSize = True
        Me.ChkMultiSmith.Checked = True
        Me.ChkMultiSmith.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkMultiSmith.Location = New System.Drawing.Point(117, 314)
        Me.ChkMultiSmith.Name = "ChkMultiSmith"
        Me.ChkMultiSmith.Size = New System.Drawing.Size(87, 17)
        Me.ChkMultiSmith.TabIndex = 19
        Me.ChkMultiSmith.Text = "MultiSelect"
        Me.ChkMultiSmith.UseVisualStyleBackColor = True
        '
        'rbtBoth
        '
        Me.rbtBoth.AutoSize = True
        Me.rbtBoth.Location = New System.Drawing.Point(258, 12)
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
        Me.chkcmbTranType.Location = New System.Drawing.Point(82, 35)
        Me.chkcmbTranType.Name = "chkcmbTranType"
        Me.chkcmbTranType.Size = New System.Drawing.Size(227, 22)
        Me.chkcmbTranType.TabIndex = 4
        Me.chkcmbTranType.ValueSeparator = ", "
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(216, 90)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpTo.TabIndex = 10
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(82, 90)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(93, 21)
        Me.dtpFrom.TabIndex = 8
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'chkSmith
        '
        Me.chkSmith.AutoSize = True
        Me.chkSmith.Location = New System.Drawing.Point(15, 314)
        Me.chkSmith.Name = "chkSmith"
        Me.chkSmith.Size = New System.Drawing.Size(40, 17)
        Me.chkSmith.TabIndex = 18
        Me.chkSmith.Text = "All"
        Me.chkSmith.UseVisualStyleBackColor = True
        '
        'chkLstSmith
        '
        Me.chkLstSmith.FormattingEnabled = True
        Me.chkLstSmith.Location = New System.Drawing.Point(12, 334)
        Me.chkLstSmith.Name = "chkLstSmith"
        Me.chkLstSmith.Size = New System.Drawing.Size(407, 68)
        Me.chkLstSmith.TabIndex = 20
        '
        'rbtReceipt
        '
        Me.rbtReceipt.AutoSize = True
        Me.rbtReceipt.Location = New System.Drawing.Point(162, 12)
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
        Me.rbtIssue.Location = New System.Drawing.Point(82, 12)
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
        Me.Label2.Location = New System.Drawing.Point(12, 38)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Tran Type"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(185, 94)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(20, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "To"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 93)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Date From"
        '
        'chkCostCentreSelectAll
        '
        Me.chkCostCentreSelectAll.AutoSize = True
        Me.chkCostCentreSelectAll.Location = New System.Drawing.Point(15, 113)
        Me.chkCostCentreSelectAll.Name = "chkCostCentreSelectAll"
        Me.chkCostCentreSelectAll.Size = New System.Drawing.Size(95, 17)
        Me.chkCostCentreSelectAll.TabIndex = 11
        Me.chkCostCentreSelectAll.Text = "Cost Centre"
        Me.chkCostCentreSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(12, 132)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(201, 52)
        Me.chkLstCostCentre.TabIndex = 12
        '
        'chkLstCategory
        '
        Me.chkLstCategory.FormattingEnabled = True
        Me.chkLstCategory.Location = New System.Drawing.Point(12, 202)
        Me.chkLstCategory.Name = "chkLstCategory"
        Me.chkLstCategory.Size = New System.Drawing.Size(407, 68)
        Me.chkLstCategory.TabIndex = 16
        '
        'chkMetalSelectAll
        '
        Me.chkMetalSelectAll.AutoSize = True
        Me.chkMetalSelectAll.Location = New System.Drawing.Point(223, 113)
        Me.chkMetalSelectAll.Name = "chkMetalSelectAll"
        Me.chkMetalSelectAll.Size = New System.Drawing.Size(56, 17)
        Me.chkMetalSelectAll.TabIndex = 13
        Me.chkMetalSelectAll.Text = "Metal"
        Me.chkMetalSelectAll.UseVisualStyleBackColor = True
        '
        'chkCategorySelectAll
        '
        Me.chkCategorySelectAll.AutoSize = True
        Me.chkCategorySelectAll.Location = New System.Drawing.Point(15, 185)
        Me.chkCategorySelectAll.Name = "chkCategorySelectAll"
        Me.chkCategorySelectAll.Size = New System.Drawing.Size(79, 17)
        Me.chkCategorySelectAll.TabIndex = 15
        Me.chkCategorySelectAll.Text = "Category"
        Me.chkCategorySelectAll.UseVisualStyleBackColor = True
        '
        'chkLstMetal
        '
        Me.chkLstMetal.FormattingEnabled = True
        Me.chkLstMetal.Location = New System.Drawing.Point(223, 132)
        Me.chkLstMetal.Name = "chkLstMetal"
        Me.chkLstMetal.Size = New System.Drawing.Size(199, 52)
        Me.chkLstMetal.TabIndex = 14
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
        'frmSmithRecIssView
        '
        Me.AccessibleDescription = ""
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(765, 581)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.pnlContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmSmithRecIssView"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Smith Issue Receipt View"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlContainer.ResumeLayout(False)
        Me.pnlContainer.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.PnlCheckType.ResumeLayout(False)
        Me.PnlCheckType.PerformLayout()
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
    Friend WithEvents chkRateCut As CheckBox
    Friend WithEvents PnlCheckType As Panel
    Friend WithEvents rbtIssRecUnMarked As RadioButton
    Friend WithEvents rbtIssRecMarked As RadioButton
    Friend WithEvents rbtIssRecBoth As RadioButton
    Friend WithEvents Label4 As Label
    Friend WithEvents Label62 As Label
    Friend WithEvents chkcmbProcessType As BrighttechPack.CheckedComboBox
End Class
