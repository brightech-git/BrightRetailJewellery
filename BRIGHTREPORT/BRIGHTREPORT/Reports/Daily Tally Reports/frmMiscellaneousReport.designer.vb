<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMiscellaneousReport
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
        Me.cmbMetalName = New System.Windows.Forms.ComboBox()
        Me.cmbItemName = New System.Windows.Forms.ComboBox()
        Me.cmbCategory = New System.Windows.Forms.ComboBox()
        Me.lblMetalName = New System.Windows.Forms.Label()
        Me.lblNodeId = New System.Windows.Forms.Label()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.lblDateFrom = New System.Windows.Forms.Label()
        Me.lblCostCentre = New System.Windows.Forms.Label()
        Me.lblItemName = New System.Windows.Forms.Label()
        Me.lblCategory = New System.Windows.Forms.Label()
        Me.chkStuddedStone = New System.Windows.Forms.CheckBox()
        Me.chkSubItem = New System.Windows.Forms.CheckBox()
        Me.GrbControls = New System.Windows.Forms.GroupBox()
        Me.chkLstItemCounter = New System.Windows.Forms.CheckedListBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.ChkPurDet = New System.Windows.Forms.CheckBox()
        Me.chkCmbStkType = New BrighttechPack.CheckedComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.chkGroupCounter = New System.Windows.Forms.CheckBox()
        Me.ChkCmbDesigner = New BrighttechPack.CheckedComboBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.rbtOrder = New System.Windows.Forms.RadioButton()
        Me.rbtBoth = New System.Windows.Forms.RadioButton()
        Me.rbtNormal = New System.Windows.Forms.RadioButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkitemwisesubtot = New System.Windows.Forms.CheckBox()
        Me.chksepcol = New System.Windows.Forms.CheckBox()
        Me.chkGroupItem = New System.Windows.Forms.CheckBox()
        Me.chkLstCompany = New System.Windows.Forms.CheckedListBox()
        Me.chkCompanySelectAll = New System.Windows.Forms.CheckBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.chkLstNodeId = New System.Windows.Forms.CheckedListBox()
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox()
        Me.grbSummaryWise = New System.Windows.Forms.GroupBox()
        Me.rbtMetalNameWise = New System.Windows.Forms.RadioButton()
        Me.rbtBillDateWise = New System.Windows.Forms.RadioButton()
        Me.rbtItemNameWise = New System.Windows.Forms.RadioButton()
        Me.rbtCategoryWise = New System.Windows.Forms.RadioButton()
        Me.rbtBillNoWise = New System.Windows.Forms.RadioButton()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.rbtSummaryWise = New System.Windows.Forms.RadioButton()
        Me.rbtDetailWise = New System.Windows.Forms.RadioButton()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlGrid = New System.Windows.Forms.Panel()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabGen = New System.Windows.Forms.TabPage()
        Me.tabView = New System.Windows.Forms.TabPage()
        Me.pnlView = New System.Windows.Forms.Panel()
        Me.pnlTitle = New System.Windows.Forms.Panel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.GrbControls.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.grbSummaryWise.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.pnlGrid.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabMain.SuspendLayout()
        Me.tabGen.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.pnlView.SuspendLayout()
        Me.pnlTitle.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmbMetalName
        '
        Me.cmbMetalName.FormattingEnabled = True
        Me.cmbMetalName.Location = New System.Drawing.Point(98, 38)
        Me.cmbMetalName.Name = "cmbMetalName"
        Me.cmbMetalName.Size = New System.Drawing.Size(289, 21)
        Me.cmbMetalName.TabIndex = 5
        '
        'cmbItemName
        '
        Me.cmbItemName.FormattingEnabled = True
        Me.cmbItemName.Location = New System.Drawing.Point(98, 111)
        Me.cmbItemName.Name = "cmbItemName"
        Me.cmbItemName.Size = New System.Drawing.Size(289, 21)
        Me.cmbItemName.TabIndex = 11
        '
        'cmbCategory
        '
        Me.cmbCategory.FormattingEnabled = True
        Me.cmbCategory.Location = New System.Drawing.Point(98, 87)
        Me.cmbCategory.Name = "cmbCategory"
        Me.cmbCategory.Size = New System.Drawing.Size(289, 21)
        Me.cmbCategory.TabIndex = 9
        '
        'lblMetalName
        '
        Me.lblMetalName.AutoSize = True
        Me.lblMetalName.Location = New System.Drawing.Point(12, 42)
        Me.lblMetalName.Name = "lblMetalName"
        Me.lblMetalName.Size = New System.Drawing.Size(70, 13)
        Me.lblMetalName.TabIndex = 4
        Me.lblMetalName.Text = "MetalName"
        '
        'lblNodeId
        '
        Me.lblNodeId.AutoSize = True
        Me.lblNodeId.Location = New System.Drawing.Point(17, 245)
        Me.lblNodeId.Name = "lblNodeId"
        Me.lblNodeId.Size = New System.Drawing.Size(54, 13)
        Me.lblNodeId.TabIndex = 16
        Me.lblNodeId.Text = "Node ID"
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(202, 18)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(20, 13)
        Me.lblTo.TabIndex = 2
        Me.lblTo.Text = "To"
        '
        'lblDateFrom
        '
        Me.lblDateFrom.AutoSize = True
        Me.lblDateFrom.Location = New System.Drawing.Point(12, 18)
        Me.lblDateFrom.Name = "lblDateFrom"
        Me.lblDateFrom.Size = New System.Drawing.Size(67, 13)
        Me.lblDateFrom.TabIndex = 0
        Me.lblDateFrom.Text = "Date From"
        '
        'lblCostCentre
        '
        Me.lblCostCentre.AutoSize = True
        Me.lblCostCentre.Location = New System.Drawing.Point(17, 190)
        Me.lblCostCentre.Name = "lblCostCentre"
        Me.lblCostCentre.Size = New System.Drawing.Size(72, 13)
        Me.lblCostCentre.TabIndex = 14
        Me.lblCostCentre.Text = "CostCentre"
        '
        'lblItemName
        '
        Me.lblItemName.AutoSize = True
        Me.lblItemName.Location = New System.Drawing.Point(12, 115)
        Me.lblItemName.Name = "lblItemName"
        Me.lblItemName.Size = New System.Drawing.Size(67, 13)
        Me.lblItemName.TabIndex = 10
        Me.lblItemName.Text = "ItemName"
        '
        'lblCategory
        '
        Me.lblCategory.AutoSize = True
        Me.lblCategory.Location = New System.Drawing.Point(12, 91)
        Me.lblCategory.Name = "lblCategory"
        Me.lblCategory.Size = New System.Drawing.Size(60, 13)
        Me.lblCategory.TabIndex = 8
        Me.lblCategory.Text = "Category"
        '
        'chkStuddedStone
        '
        Me.chkStuddedStone.AutoSize = True
        Me.chkStuddedStone.Location = New System.Drawing.Point(55, 432)
        Me.chkStuddedStone.Name = "chkStuddedStone"
        Me.chkStuddedStone.Size = New System.Drawing.Size(106, 17)
        Me.chkStuddedStone.TabIndex = 24
        Me.chkStuddedStone.Text = "StuddedStone"
        Me.chkStuddedStone.UseVisualStyleBackColor = True
        '
        'chkSubItem
        '
        Me.chkSubItem.AutoSize = True
        Me.chkSubItem.Location = New System.Drawing.Point(178, 432)
        Me.chkSubItem.Name = "chkSubItem"
        Me.chkSubItem.Size = New System.Drawing.Size(75, 17)
        Me.chkSubItem.TabIndex = 25
        Me.chkSubItem.Text = "SubItem"
        Me.chkSubItem.UseVisualStyleBackColor = True
        '
        'GrbControls
        '
        Me.GrbControls.Controls.Add(Me.chkLstItemCounter)
        Me.GrbControls.Controls.Add(Me.Label6)
        Me.GrbControls.Controls.Add(Me.ChkPurDet)
        Me.GrbControls.Controls.Add(Me.chkCmbStkType)
        Me.GrbControls.Controls.Add(Me.Label9)
        Me.GrbControls.Controls.Add(Me.chkGroupCounter)
        Me.GrbControls.Controls.Add(Me.ChkCmbDesigner)
        Me.GrbControls.Controls.Add(Me.Panel2)
        Me.GrbControls.Controls.Add(Me.Label1)
        Me.GrbControls.Controls.Add(Me.chkitemwisesubtot)
        Me.GrbControls.Controls.Add(Me.chksepcol)
        Me.GrbControls.Controls.Add(Me.chkGroupItem)
        Me.GrbControls.Controls.Add(Me.chkLstCompany)
        Me.GrbControls.Controls.Add(Me.chkCompanySelectAll)
        Me.GrbControls.Controls.Add(Me.dtpTo)
        Me.GrbControls.Controls.Add(Me.dtpFrom)
        Me.GrbControls.Controls.Add(Me.chkLstNodeId)
        Me.GrbControls.Controls.Add(Me.chkLstCostCentre)
        Me.GrbControls.Controls.Add(Me.grbSummaryWise)
        Me.GrbControls.Controls.Add(Me.btnExit)
        Me.GrbControls.Controls.Add(Me.btnNew)
        Me.GrbControls.Controls.Add(Me.btnView_Search)
        Me.GrbControls.Controls.Add(Me.chkSubItem)
        Me.GrbControls.Controls.Add(Me.cmbMetalName)
        Me.GrbControls.Controls.Add(Me.chkStuddedStone)
        Me.GrbControls.Controls.Add(Me.cmbItemName)
        Me.GrbControls.Controls.Add(Me.cmbCategory)
        Me.GrbControls.Controls.Add(Me.lblMetalName)
        Me.GrbControls.Controls.Add(Me.lblNodeId)
        Me.GrbControls.Controls.Add(Me.lblTo)
        Me.GrbControls.Controls.Add(Me.lblDateFrom)
        Me.GrbControls.Controls.Add(Me.lblCostCentre)
        Me.GrbControls.Controls.Add(Me.lblCategory)
        Me.GrbControls.Controls.Add(Me.lblItemName)
        Me.GrbControls.Controls.Add(Me.Panel3)
        Me.GrbControls.Location = New System.Drawing.Point(275, 4)
        Me.GrbControls.Name = "GrbControls"
        Me.GrbControls.Size = New System.Drawing.Size(424, 593)
        Me.GrbControls.TabIndex = 0
        Me.GrbControls.TabStop = False
        '
        'chkLstItemCounter
        '
        Me.chkLstItemCounter.FormattingEnabled = True
        Me.chkLstItemCounter.Location = New System.Drawing.Point(99, 300)
        Me.chkLstItemCounter.Name = "chkLstItemCounter"
        Me.chkLstItemCounter.Size = New System.Drawing.Size(289, 52)
        Me.chkLstItemCounter.TabIndex = 19
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(17, 300)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(84, 13)
        Me.Label6.TabIndex = 18
        Me.Label6.Text = "Item Counter"
        '
        'ChkPurDet
        '
        Me.ChkPurDet.AutoSize = True
        Me.ChkPurDet.Location = New System.Drawing.Point(178, 472)
        Me.ChkPurDet.Name = "ChkPurDet"
        Me.ChkPurDet.Size = New System.Drawing.Size(144, 17)
        Me.ChkPurDet.TabIndex = 30
        Me.ChkPurDet.Text = "With Purchase Detail"
        Me.ChkPurDet.UseVisualStyleBackColor = True
        '
        'chkCmbStkType
        '
        Me.chkCmbStkType.CheckOnClick = True
        Me.chkCmbStkType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbStkType.DropDownHeight = 1
        Me.chkCmbStkType.FormattingEnabled = True
        Me.chkCmbStkType.IntegralHeight = False
        Me.chkCmbStkType.Location = New System.Drawing.Point(98, 355)
        Me.chkCmbStkType.Name = "chkCmbStkType"
        Me.chkCmbStkType.Size = New System.Drawing.Size(289, 22)
        Me.chkCmbStkType.TabIndex = 21
        Me.chkCmbStkType.ValueSeparator = ", "
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(17, 360)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(70, 13)
        Me.Label9.TabIndex = 20
        Me.Label9.Text = "Stock Type"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkGroupCounter
        '
        Me.chkGroupCounter.AutoSize = True
        Me.chkGroupCounter.Location = New System.Drawing.Point(178, 452)
        Me.chkGroupCounter.Name = "chkGroupCounter"
        Me.chkGroupCounter.Size = New System.Drawing.Size(130, 17)
        Me.chkGroupCounter.TabIndex = 28
        Me.chkGroupCounter.Text = "Group By Counter"
        Me.chkGroupCounter.UseVisualStyleBackColor = True
        '
        'ChkCmbDesigner
        '
        Me.ChkCmbDesigner.CheckOnClick = True
        Me.ChkCmbDesigner.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.ChkCmbDesigner.DropDownHeight = 1
        Me.ChkCmbDesigner.FormattingEnabled = True
        Me.ChkCmbDesigner.IntegralHeight = False
        Me.ChkCmbDesigner.Location = New System.Drawing.Point(98, 62)
        Me.ChkCmbDesigner.Name = "ChkCmbDesigner"
        Me.ChkCmbDesigner.Size = New System.Drawing.Size(289, 22)
        Me.ChkCmbDesigner.TabIndex = 7
        Me.ChkCmbDesigner.ValueSeparator = ", "
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rbtOrder)
        Me.Panel2.Controls.Add(Me.rbtBoth)
        Me.Panel2.Controls.Add(Me.rbtNormal)
        Me.Panel2.Location = New System.Drawing.Point(98, 406)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(289, 23)
        Me.Panel2.TabIndex = 23
        '
        'rbtOrder
        '
        Me.rbtOrder.AutoSize = True
        Me.rbtOrder.Location = New System.Drawing.Point(167, 4)
        Me.rbtOrder.Name = "rbtOrder"
        Me.rbtOrder.Size = New System.Drawing.Size(58, 17)
        Me.rbtOrder.TabIndex = 2
        Me.rbtOrder.Text = "Order"
        Me.rbtOrder.UseVisualStyleBackColor = True
        '
        'rbtBoth
        '
        Me.rbtBoth.AutoSize = True
        Me.rbtBoth.Checked = True
        Me.rbtBoth.Location = New System.Drawing.Point(6, 5)
        Me.rbtBoth.Name = "rbtBoth"
        Me.rbtBoth.Size = New System.Drawing.Size(51, 17)
        Me.rbtBoth.TabIndex = 0
        Me.rbtBoth.TabStop = True
        Me.rbtBoth.Text = "Both"
        Me.rbtBoth.UseVisualStyleBackColor = True
        '
        'rbtNormal
        '
        Me.rbtNormal.AutoSize = True
        Me.rbtNormal.Location = New System.Drawing.Point(86, 4)
        Me.rbtNormal.Name = "rbtNormal"
        Me.rbtNormal.Size = New System.Drawing.Size(66, 17)
        Me.rbtNormal.TabIndex = 1
        Me.rbtNormal.Text = "Normal"
        Me.rbtNormal.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 67)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Designer"
        '
        'chkitemwisesubtot
        '
        Me.chkitemwisesubtot.AutoSize = True
        Me.chkitemwisesubtot.Location = New System.Drawing.Point(55, 472)
        Me.chkitemwisesubtot.Name = "chkitemwisesubtot"
        Me.chkitemwisesubtot.Size = New System.Drawing.Size(117, 17)
        Me.chkitemwisesubtot.TabIndex = 29
        Me.chkitemwisesubtot.Text = "BillWise Sub Tot"
        Me.chkitemwisesubtot.UseVisualStyleBackColor = True
        '
        'chksepcol
        '
        Me.chksepcol.AutoSize = True
        Me.chksepcol.Checked = True
        Me.chksepcol.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chksepcol.Location = New System.Drawing.Point(269, 432)
        Me.chksepcol.Name = "chksepcol"
        Me.chksepcol.Size = New System.Drawing.Size(118, 17)
        Me.chksepcol.TabIndex = 26
        Me.chksepcol.Text = "Sep Col Stn/Dia"
        Me.chksepcol.UseVisualStyleBackColor = True
        '
        'chkGroupItem
        '
        Me.chkGroupItem.AutoSize = True
        Me.chkGroupItem.Location = New System.Drawing.Point(55, 452)
        Me.chkGroupItem.Name = "chkGroupItem"
        Me.chkGroupItem.Size = New System.Drawing.Size(111, 17)
        Me.chkGroupItem.TabIndex = 27
        Me.chkGroupItem.Text = "Group By Item"
        Me.chkGroupItem.UseVisualStyleBackColor = True
        '
        'chkLstCompany
        '
        Me.chkLstCompany.FormattingEnabled = True
        Me.chkLstCompany.Location = New System.Drawing.Point(98, 135)
        Me.chkLstCompany.Name = "chkLstCompany"
        Me.chkLstCompany.Size = New System.Drawing.Size(289, 52)
        Me.chkLstCompany.TabIndex = 13
        '
        'chkCompanySelectAll
        '
        Me.chkCompanySelectAll.AutoSize = True
        Me.chkCompanySelectAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkCompanySelectAll.Location = New System.Drawing.Point(12, 135)
        Me.chkCompanySelectAll.Name = "chkCompanySelectAll"
        Me.chkCompanySelectAll.Size = New System.Drawing.Size(81, 17)
        Me.chkCompanySelectAll.TabIndex = 12
        Me.chkCompanySelectAll.Text = "Company"
        Me.chkCompanySelectAll.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(232, 14)
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
        Me.dtpFrom.Location = New System.Drawing.Point(98, 14)
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
        'chkLstNodeId
        '
        Me.chkLstNodeId.FormattingEnabled = True
        Me.chkLstNodeId.Location = New System.Drawing.Point(98, 245)
        Me.chkLstNodeId.Name = "chkLstNodeId"
        Me.chkLstNodeId.Size = New System.Drawing.Size(289, 52)
        Me.chkLstNodeId.TabIndex = 17
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(98, 190)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(289, 52)
        Me.chkLstCostCentre.TabIndex = 15
        '
        'grbSummaryWise
        '
        Me.grbSummaryWise.Controls.Add(Me.rbtMetalNameWise)
        Me.grbSummaryWise.Controls.Add(Me.rbtBillDateWise)
        Me.grbSummaryWise.Controls.Add(Me.rbtItemNameWise)
        Me.grbSummaryWise.Controls.Add(Me.rbtCategoryWise)
        Me.grbSummaryWise.Controls.Add(Me.rbtBillNoWise)
        Me.grbSummaryWise.Location = New System.Drawing.Point(20, 483)
        Me.grbSummaryWise.Name = "grbSummaryWise"
        Me.grbSummaryWise.Size = New System.Drawing.Size(365, 55)
        Me.grbSummaryWise.TabIndex = 31
        Me.grbSummaryWise.TabStop = False
        '
        'rbtMetalNameWise
        '
        Me.rbtMetalNameWise.AutoSize = True
        Me.rbtMetalNameWise.Location = New System.Drawing.Point(121, 34)
        Me.rbtMetalNameWise.Name = "rbtMetalNameWise"
        Me.rbtMetalNameWise.Size = New System.Drawing.Size(115, 17)
        Me.rbtMetalNameWise.TabIndex = 4
        Me.rbtMetalNameWise.TabStop = True
        Me.rbtMetalNameWise.Text = "MetalNameWise"
        Me.rbtMetalNameWise.UseVisualStyleBackColor = True
        '
        'rbtBillDateWise
        '
        Me.rbtBillDateWise.AutoSize = True
        Me.rbtBillDateWise.Location = New System.Drawing.Point(121, 11)
        Me.rbtBillDateWise.Name = "rbtBillDateWise"
        Me.rbtBillDateWise.Size = New System.Drawing.Size(100, 17)
        Me.rbtBillDateWise.TabIndex = 1
        Me.rbtBillDateWise.TabStop = True
        Me.rbtBillDateWise.Text = "BillDate Wise"
        Me.rbtBillDateWise.UseVisualStyleBackColor = True
        '
        'rbtItemNameWise
        '
        Me.rbtItemNameWise.AutoSize = True
        Me.rbtItemNameWise.Location = New System.Drawing.Point(6, 34)
        Me.rbtItemNameWise.Name = "rbtItemNameWise"
        Me.rbtItemNameWise.Size = New System.Drawing.Size(112, 17)
        Me.rbtItemNameWise.TabIndex = 3
        Me.rbtItemNameWise.TabStop = True
        Me.rbtItemNameWise.Text = "ItemNameWise"
        Me.rbtItemNameWise.UseVisualStyleBackColor = True
        '
        'rbtCategoryWise
        '
        Me.rbtCategoryWise.AutoSize = True
        Me.rbtCategoryWise.Location = New System.Drawing.Point(232, 11)
        Me.rbtCategoryWise.Name = "rbtCategoryWise"
        Me.rbtCategoryWise.Size = New System.Drawing.Size(109, 17)
        Me.rbtCategoryWise.TabIndex = 2
        Me.rbtCategoryWise.TabStop = True
        Me.rbtCategoryWise.Text = "Category Wise"
        Me.rbtCategoryWise.UseVisualStyleBackColor = True
        '
        'rbtBillNoWise
        '
        Me.rbtBillNoWise.AutoSize = True
        Me.rbtBillNoWise.Location = New System.Drawing.Point(6, 11)
        Me.rbtBillNoWise.Name = "rbtBillNoWise"
        Me.rbtBillNoWise.Size = New System.Drawing.Size(88, 17)
        Me.rbtBillNoWise.TabIndex = 0
        Me.rbtBillNoWise.TabStop = True
        Me.rbtBillNoWise.Text = "BillNo Wise"
        Me.rbtBillNoWise.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(273, 539)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 34
        Me.btnExit.Text = "&Exit[F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(167, 539)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 33
        Me.btnNew.Text = "&New[F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(61, 539)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 32
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.rbtSummaryWise)
        Me.Panel3.Controls.Add(Me.rbtDetailWise)
        Me.Panel3.Location = New System.Drawing.Point(98, 380)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(200, 23)
        Me.Panel3.TabIndex = 22
        '
        'rbtSummaryWise
        '
        Me.rbtSummaryWise.AutoSize = True
        Me.rbtSummaryWise.Location = New System.Drawing.Point(85, 3)
        Me.rbtSummaryWise.Name = "rbtSummaryWise"
        Me.rbtSummaryWise.Size = New System.Drawing.Size(108, 17)
        Me.rbtSummaryWise.TabIndex = 1
        Me.rbtSummaryWise.TabStop = True
        Me.rbtSummaryWise.Text = "SummaryWise"
        Me.rbtSummaryWise.UseVisualStyleBackColor = True
        '
        'rbtDetailWise
        '
        Me.rbtDetailWise.AutoSize = True
        Me.rbtDetailWise.Location = New System.Drawing.Point(4, 4)
        Me.rbtDetailWise.Name = "rbtDetailWise"
        Me.rbtDetailWise.Size = New System.Drawing.Size(85, 17)
        Me.rbtDetailWise.TabIndex = 0
        Me.rbtDetailWise.TabStop = True
        Me.rbtDetailWise.Text = "DetailWise"
        Me.rbtDetailWise.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(775, 2)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 15
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(668, 2)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 14
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
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
        Me.NewToolStripMenuItem.Text = "&New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.ExitToolStripMenuItem.Text = "&Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'pnlGrid
        '
        Me.pnlGrid.Controls.Add(Me.gridView)
        Me.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlGrid.Location = New System.Drawing.Point(0, 29)
        Me.pnlGrid.Name = "pnlGrid"
        Me.pnlGrid.Size = New System.Drawing.Size(1005, 539)
        Me.pnlGrid.TabIndex = 2
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.AllowUserToResizeRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 0)
        Me.gridView.MultiSelect = False
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(1005, 539)
        Me.gridView.StandardTab = True
        Me.gridView.TabIndex = 1
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1005, 29)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "TITLE"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGen)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1019, 632)
        Me.tabMain.TabIndex = 0
        '
        'tabGen
        '
        Me.tabGen.Controls.Add(Me.GrbControls)
        Me.tabGen.Location = New System.Drawing.Point(4, 22)
        Me.tabGen.Name = "tabGen"
        Me.tabGen.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGen.Size = New System.Drawing.Size(1011, 606)
        Me.tabGen.TabIndex = 0
        Me.tabGen.Text = "Gen"
        Me.tabGen.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.pnlView)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1011, 606)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'pnlView
        '
        Me.pnlView.Controls.Add(Me.pnlGrid)
        Me.pnlView.Controls.Add(Me.pnlTitle)
        Me.pnlView.Controls.Add(Me.Panel1)
        Me.pnlView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlView.Location = New System.Drawing.Point(3, 3)
        Me.pnlView.Name = "pnlView"
        Me.pnlView.Size = New System.Drawing.Size(1005, 600)
        Me.pnlView.TabIndex = 0
        '
        'pnlTitle
        '
        Me.pnlTitle.Controls.Add(Me.lblTitle)
        Me.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTitle.Location = New System.Drawing.Point(0, 0)
        Me.pnlTitle.Name = "pnlTitle"
        Me.pnlTitle.Size = New System.Drawing.Size(1005, 29)
        Me.pnlTitle.TabIndex = 1
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnBack)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 568)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1005, 32)
        Me.Panel1.TabIndex = 0
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(561, 2)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 16
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'frmMiscellaneousReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1019, 632)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "frmMiscellaneousReport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "MiscellaneousReport"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GrbControls.ResumeLayout(False)
        Me.GrbControls.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.grbSummaryWise.ResumeLayout(False)
        Me.grbSummaryWise.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.pnlGrid.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabMain.ResumeLayout(False)
        Me.tabGen.ResumeLayout(False)
        Me.tabView.ResumeLayout(False)
        Me.pnlView.ResumeLayout(False)
        Me.pnlTitle.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cmbMetalName As System.Windows.Forms.ComboBox
    Friend WithEvents cmbItemName As System.Windows.Forms.ComboBox
    Friend WithEvents cmbCategory As System.Windows.Forms.ComboBox
    Friend WithEvents lblMetalName As System.Windows.Forms.Label
    Friend WithEvents lblNodeId As System.Windows.Forms.Label
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents lblDateFrom As System.Windows.Forms.Label
    Friend WithEvents lblCostCentre As System.Windows.Forms.Label
    Friend WithEvents lblItemName As System.Windows.Forms.Label
    Friend WithEvents lblCategory As System.Windows.Forms.Label
    Friend WithEvents chkStuddedStone As System.Windows.Forms.CheckBox
    Friend WithEvents chkSubItem As System.Windows.Forms.CheckBox
    Friend WithEvents GrbControls As System.Windows.Forms.GroupBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents rbtSummaryWise As System.Windows.Forms.RadioButton
    Friend WithEvents rbtDetailWise As System.Windows.Forms.RadioButton
    Friend WithEvents rbtCategoryWise As System.Windows.Forms.RadioButton
    Friend WithEvents rbtBillDateWise As System.Windows.Forms.RadioButton
    Friend WithEvents rbtBillNoWise As System.Windows.Forms.RadioButton
    Friend WithEvents rbtItemNameWise As System.Windows.Forms.RadioButton
    Friend WithEvents grbSummaryWise As System.Windows.Forms.GroupBox
    Friend WithEvents pnlGrid As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGen As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents pnlView As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents pnlTitle As System.Windows.Forms.Panel
    Friend WithEvents chkLstNodeId As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkLstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkLstCompany As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkCompanySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rbtOrder As System.Windows.Forms.RadioButton
    Friend WithEvents rbtBoth As System.Windows.Forms.RadioButton
    Friend WithEvents rbtNormal As System.Windows.Forms.RadioButton
    Friend WithEvents chkGroupItem As System.Windows.Forms.CheckBox
    Friend WithEvents chksepcol As System.Windows.Forms.CheckBox
    Friend WithEvents chkitemwisesubtot As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ChkCmbDesigner As BrighttechPack.CheckedComboBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents chkGroupCounter As System.Windows.Forms.CheckBox
    Friend WithEvents chkCmbStkType As BrighttechPack.CheckedComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents ChkPurDet As System.Windows.Forms.CheckBox
    Friend WithEvents rbtMetalNameWise As RadioButton
    Friend WithEvents Label6 As Label
    Friend WithEvents chkLstItemCounter As CheckedListBox
End Class
