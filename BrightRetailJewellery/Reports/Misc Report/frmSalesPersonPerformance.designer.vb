<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSalesPersonPerformance
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
        Me.grpControls = New System.Windows.Forms.GroupBox()
        Me.cmdcashcounter = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.chkWithSavings = New System.Windows.Forms.CheckBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtTagNoDay_Num = New System.Windows.Forms.TextBox()
        Me.ChkWithAdv = New System.Windows.Forms.CheckBox()
        Me.chkLstItem = New System.Windows.Forms.CheckedListBox()
        Me.chkItemAll = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ChkWithRepair = New System.Windows.Forms.CheckBox()
        Me.ChkWithOrder = New System.Windows.Forms.CheckBox()
        Me.chkMonthWise = New System.Windows.Forms.CheckBox()
        Me.ChkDateWise = New System.Windows.Forms.CheckBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.rbtGRSWT = New System.Windows.Forms.RadioButton()
        Me.rbtNetWT = New System.Windows.Forms.RadioButton()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbCompany = New GiritechPack.CheckedComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbSalesPerson = New GiritechPack.CheckedComboBox()
        Me.ChkOrderonly = New System.Windows.Forms.CheckBox()
        Me.txtwithdate = New System.Windows.Forms.TextBox()
        Me.withdate = New System.Windows.Forms.CheckBox()
        Me.chkCmbCostCentre = New GiritechPack.CheckedComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtpTo = New GiritechPack.DatePicker(Me.components)
        Me.dtpFrom = New GiritechPack.DatePicker(Me.components)
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.cmbMetal = New System.Windows.Forms.ComboBox()
        Me.lABEL60 = New System.Windows.Forms.Label()
        Me.lblSalePerson = New System.Windows.Forms.Label()
        Me.lblDateTo = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblDateFrom = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.rdbCategory = New System.Windows.Forms.RadioButton()
        Me.rbtMetal = New System.Windows.Forms.RadioButton()
        Me.rbtNone = New System.Windows.Forms.RadioButton()
        Me.rbtCounter = New System.Windows.Forms.RadioButton()
        Me.pnlGrid = New System.Windows.Forms.Panel()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.gridViewHead = New System.Windows.Forms.DataGridView()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.grpControls.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pnlGrid.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip2.SuspendLayout()
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpControls
        '
        Me.grpControls.Controls.Add(Me.cmdcashcounter)
        Me.grpControls.Controls.Add(Me.Label7)
        Me.grpControls.Controls.Add(Me.chkWithSavings)
        Me.grpControls.Controls.Add(Me.Label6)
        Me.grpControls.Controls.Add(Me.txtTagNoDay_Num)
        Me.grpControls.Controls.Add(Me.ChkWithAdv)
        Me.grpControls.Controls.Add(Me.chkLstItem)
        Me.grpControls.Controls.Add(Me.chkItemAll)
        Me.grpControls.Controls.Add(Me.Label5)
        Me.grpControls.Controls.Add(Me.ChkWithRepair)
        Me.grpControls.Controls.Add(Me.ChkWithOrder)
        Me.grpControls.Controls.Add(Me.chkMonthWise)
        Me.grpControls.Controls.Add(Me.ChkDateWise)
        Me.grpControls.Controls.Add(Me.Panel2)
        Me.grpControls.Controls.Add(Me.Label4)
        Me.grpControls.Controls.Add(Me.cmbCompany)
        Me.grpControls.Controls.Add(Me.Label3)
        Me.grpControls.Controls.Add(Me.cmbSalesPerson)
        Me.grpControls.Controls.Add(Me.ChkOrderonly)
        Me.grpControls.Controls.Add(Me.txtwithdate)
        Me.grpControls.Controls.Add(Me.withdate)
        Me.grpControls.Controls.Add(Me.chkCmbCostCentre)
        Me.grpControls.Controls.Add(Me.Label2)
        Me.grpControls.Controls.Add(Me.dtpTo)
        Me.grpControls.Controls.Add(Me.dtpFrom)
        Me.grpControls.Controls.Add(Me.btnExit)
        Me.grpControls.Controls.Add(Me.btnPrint)
        Me.grpControls.Controls.Add(Me.btnExport)
        Me.grpControls.Controls.Add(Me.btnNew)
        Me.grpControls.Controls.Add(Me.btnView_Search)
        Me.grpControls.Controls.Add(Me.cmbMetal)
        Me.grpControls.Controls.Add(Me.lABEL60)
        Me.grpControls.Controls.Add(Me.lblSalePerson)
        Me.grpControls.Controls.Add(Me.lblDateTo)
        Me.grpControls.Controls.Add(Me.Label1)
        Me.grpControls.Controls.Add(Me.lblDateFrom)
        Me.grpControls.Controls.Add(Me.Panel1)
        Me.grpControls.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpControls.Location = New System.Drawing.Point(0, 0)
        Me.grpControls.Name = "grpControls"
        Me.grpControls.Size = New System.Drawing.Size(1019, 179)
        Me.grpControls.TabIndex = 0
        Me.grpControls.TabStop = False
        '
        'cmdcashcounter
        '
        Me.cmdcashcounter.FormattingEnabled = True
        Me.cmdcashcounter.Location = New System.Drawing.Point(94, 116)
        Me.cmdcashcounter.Name = "cmdcashcounter"
        Me.cmdcashcounter.Size = New System.Drawing.Size(213, 21)
        Me.cmdcashcounter.TabIndex = 10
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 120)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(86, 13)
        Me.Label7.TabIndex = 35
        Me.Label7.Text = "Cash Counter"
        '
        'chkWithSavings
        '
        Me.chkWithSavings.AutoSize = True
        Me.chkWithSavings.Location = New System.Drawing.Point(798, 65)
        Me.chkWithSavings.Name = "chkWithSavings"
        Me.chkWithSavings.Size = New System.Drawing.Size(100, 17)
        Me.chkWithSavings.TabIndex = 22
        Me.chkWithSavings.Text = "With Savings"
        Me.chkWithSavings.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(795, 141)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(107, 13)
        Me.Label6.TabIndex = 28
        Me.Label6.Text = "Tag Receipt Days"
        '
        'txtTagNoDay_Num
        '
        Me.txtTagNoDay_Num.Location = New System.Drawing.Point(907, 137)
        Me.txtTagNoDay_Num.Name = "txtTagNoDay_Num"
        Me.txtTagNoDay_Num.Size = New System.Drawing.Size(79, 21)
        Me.txtTagNoDay_Num.TabIndex = 29
        '
        'ChkWithAdv
        '
        Me.ChkWithAdv.AutoSize = True
        Me.ChkWithAdv.Location = New System.Drawing.Point(683, 140)
        Me.ChkWithAdv.Name = "ChkWithAdv"
        Me.ChkWithAdv.Size = New System.Drawing.Size(104, 17)
        Me.ChkWithAdv.TabIndex = 27
        Me.ChkWithAdv.Text = "With Advance"
        Me.ChkWithAdv.UseVisualStyleBackColor = True
        '
        'chkLstItem
        '
        Me.chkLstItem.FormattingEnabled = True
        Me.chkLstItem.Location = New System.Drawing.Point(398, 34)
        Me.chkLstItem.Name = "chkLstItem"
        Me.chkLstItem.Size = New System.Drawing.Size(275, 52)
        Me.chkLstItem.TabIndex = 12
        '
        'chkItemAll
        '
        Me.chkItemAll.AutoSize = True
        Me.chkItemAll.Location = New System.Drawing.Point(401, 17)
        Me.chkItemAll.Name = "chkItemAll"
        Me.chkItemAll.Size = New System.Drawing.Size(86, 17)
        Me.chkItemAll.TabIndex = 11
        Me.chkItemAll.Text = "ItemName"
        Me.chkItemAll.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(313, 16)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(75, 13)
        Me.Label5.TabIndex = 36
        Me.Label5.Text = "Item Name "
        '
        'ChkWithRepair
        '
        Me.ChkWithRepair.AutoSize = True
        Me.ChkWithRepair.Location = New System.Drawing.Point(798, 90)
        Me.ChkWithRepair.Name = "ChkWithRepair"
        Me.ChkWithRepair.Size = New System.Drawing.Size(92, 17)
        Me.ChkWithRepair.TabIndex = 24
        Me.ChkWithRepair.Text = "With Repair"
        Me.ChkWithRepair.UseVisualStyleBackColor = True
        '
        'ChkWithOrder
        '
        Me.ChkWithOrder.AutoSize = True
        Me.ChkWithOrder.Location = New System.Drawing.Point(683, 90)
        Me.ChkWithOrder.Name = "ChkWithOrder"
        Me.ChkWithOrder.Size = New System.Drawing.Size(88, 17)
        Me.ChkWithOrder.TabIndex = 23
        Me.ChkWithOrder.Text = "With Order"
        Me.ChkWithOrder.UseVisualStyleBackColor = True
        '
        'chkMonthWise
        '
        Me.chkMonthWise.AutoSize = True
        Me.chkMonthWise.Location = New System.Drawing.Point(798, 115)
        Me.chkMonthWise.Name = "chkMonthWise"
        Me.chkMonthWise.Size = New System.Drawing.Size(120, 17)
        Me.chkMonthWise.TabIndex = 26
        Me.chkMonthWise.Text = "With Month Wise"
        Me.chkMonthWise.UseVisualStyleBackColor = True
        '
        'ChkDateWise
        '
        Me.ChkDateWise.AutoSize = True
        Me.ChkDateWise.Location = New System.Drawing.Point(683, 115)
        Me.ChkDateWise.Name = "ChkDateWise"
        Me.ChkDateWise.Size = New System.Drawing.Size(113, 17)
        Me.ChkDateWise.TabIndex = 25
        Me.ChkDateWise.Text = "With Date Wise"
        Me.ChkDateWise.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rbtGRSWT)
        Me.Panel2.Controls.Add(Me.rbtNetWT)
        Me.Panel2.Location = New System.Drawing.Point(680, 9)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(177, 27)
        Me.Panel2.TabIndex = 18
        '
        'rbtGRSWT
        '
        Me.rbtGRSWT.AutoSize = True
        Me.rbtGRSWT.Location = New System.Drawing.Point(18, 5)
        Me.rbtGRSWT.Name = "rbtGRSWT"
        Me.rbtGRSWT.Size = New System.Drawing.Size(67, 17)
        Me.rbtGRSWT.TabIndex = 0
        Me.rbtGRSWT.TabStop = True
        Me.rbtGRSWT.Text = "Grs WT"
        Me.rbtGRSWT.UseVisualStyleBackColor = True
        '
        'rbtNetWT
        '
        Me.rbtNetWT.AutoSize = True
        Me.rbtNetWT.Location = New System.Drawing.Point(91, 5)
        Me.rbtNetWT.Name = "rbtNetWT"
        Me.rbtNetWT.Size = New System.Drawing.Size(66, 17)
        Me.rbtNetWT.TabIndex = 1
        Me.rbtNetWT.TabStop = True
        Me.rbtNetWT.Text = "Net WT"
        Me.rbtNetWT.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(628, 14)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(46, 13)
        Me.Label4.TabIndex = 17
        Me.Label4.Text = "Weight"
        '
        'cmbCompany
        '
        Me.cmbCompany.CheckOnClick = True
        Me.cmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbCompany.DropDownHeight = 1
        Me.cmbCompany.FormattingEnabled = True
        Me.cmbCompany.IntegralHeight = False
        Me.cmbCompany.Location = New System.Drawing.Point(94, 38)
        Me.cmbCompany.Name = "cmbCompany"
        Me.cmbCompany.Size = New System.Drawing.Size(213, 22)
        Me.cmbCompany.TabIndex = 5
        Me.cmbCompany.ValueSeparator = ", "
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 41)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(62, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Company"
        '
        'cmbSalesPerson
        '
        Me.cmbSalesPerson.CheckOnClick = True
        Me.cmbSalesPerson.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbSalesPerson.DropDownHeight = 1
        Me.cmbSalesPerson.FormattingEnabled = True
        Me.cmbSalesPerson.IntegralHeight = False
        Me.cmbSalesPerson.Location = New System.Drawing.Point(398, 89)
        Me.cmbSalesPerson.Name = "cmbSalesPerson"
        Me.cmbSalesPerson.Size = New System.Drawing.Size(275, 22)
        Me.cmbSalesPerson.TabIndex = 14
        Me.cmbSalesPerson.ValueSeparator = ", "
        '
        'ChkOrderonly
        '
        Me.ChkOrderonly.AutoSize = True
        Me.ChkOrderonly.Location = New System.Drawing.Point(683, 65)
        Me.ChkOrderonly.Name = "ChkOrderonly"
        Me.ChkOrderonly.Size = New System.Drawing.Size(89, 17)
        Me.ChkOrderonly.TabIndex = 21
        Me.ChkOrderonly.Text = "Order Only"
        Me.ChkOrderonly.UseVisualStyleBackColor = True
        '
        'txtwithdate
        '
        Me.txtwithdate.Location = New System.Drawing.Point(814, 37)
        Me.txtwithdate.Name = "txtwithdate"
        Me.txtwithdate.Size = New System.Drawing.Size(39, 21)
        Me.txtwithdate.TabIndex = 20
        Me.txtwithdate.Visible = False
        '
        'withdate
        '
        Me.withdate.AutoSize = True
        Me.withdate.Location = New System.Drawing.Point(683, 40)
        Me.withdate.Name = "withdate"
        Me.withdate.Size = New System.Drawing.Size(129, 17)
        Me.withdate.TabIndex = 19
        Me.withdate.Text = "With date opening"
        Me.withdate.UseVisualStyleBackColor = True
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(94, 64)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(213, 22)
        Me.chkCmbCostCentre.TabIndex = 7
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 68)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(76, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Cost Centre"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(214, 13)
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
        Me.dtpFrom.Location = New System.Drawing.Point(94, 13)
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
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(577, 145)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 34
        Me.btnExit.Text = "&Exit[F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(475, 145)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 33
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(372, 145)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 32
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(270, 145)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 31
        Me.btnNew.Text = "&New[F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(168, 145)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 30
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'cmbMetal
        '
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(94, 90)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(213, 21)
        Me.cmbMetal.TabIndex = 9
        '
        'lABEL60
        '
        Me.lABEL60.AutoSize = True
        Me.lABEL60.Location = New System.Drawing.Point(6, 94)
        Me.lABEL60.Name = "lABEL60"
        Me.lABEL60.Size = New System.Drawing.Size(37, 13)
        Me.lABEL60.TabIndex = 8
        Me.lABEL60.Text = "Metal"
        '
        'lblSalePerson
        '
        Me.lblSalePerson.AutoSize = True
        Me.lblSalePerson.Location = New System.Drawing.Point(313, 94)
        Me.lblSalePerson.Name = "lblSalePerson"
        Me.lblSalePerson.Size = New System.Drawing.Size(71, 13)
        Me.lblSalePerson.TabIndex = 13
        Me.lblSalePerson.Text = "SalePerson"
        '
        'lblDateTo
        '
        Me.lblDateTo.AutoSize = True
        Me.lblDateTo.Location = New System.Drawing.Point(190, 16)
        Me.lblDateTo.Name = "lblDateTo"
        Me.lblDateTo.Size = New System.Drawing.Size(21, 13)
        Me.lblDateTo.TabIndex = 2
        Me.lblDateTo.Text = "To"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(313, 120)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 13)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Group By"
        '
        'lblDateFrom
        '
        Me.lblDateFrom.AutoSize = True
        Me.lblDateFrom.Location = New System.Drawing.Point(6, 16)
        Me.lblDateFrom.Name = "lblDateFrom"
        Me.lblDateFrom.Size = New System.Drawing.Size(67, 13)
        Me.lblDateFrom.TabIndex = 0
        Me.lblDateFrom.Text = "Date From"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rdbCategory)
        Me.Panel1.Controls.Add(Me.rbtMetal)
        Me.Panel1.Controls.Add(Me.rbtNone)
        Me.Panel1.Controls.Add(Me.rbtCounter)
        Me.Panel1.Location = New System.Drawing.Point(398, 117)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(275, 24)
        Me.Panel1.TabIndex = 16
        '
        'rdbCategory
        '
        Me.rdbCategory.AutoSize = True
        Me.rdbCategory.Location = New System.Drawing.Point(196, 4)
        Me.rdbCategory.Name = "rdbCategory"
        Me.rdbCategory.Size = New System.Drawing.Size(78, 17)
        Me.rdbCategory.TabIndex = 3
        Me.rdbCategory.TabStop = True
        Me.rdbCategory.Text = "Category"
        Me.rdbCategory.UseVisualStyleBackColor = True
        '
        'rbtMetal
        '
        Me.rbtMetal.AutoSize = True
        Me.rbtMetal.Location = New System.Drawing.Point(67, 4)
        Me.rbtMetal.Name = "rbtMetal"
        Me.rbtMetal.Size = New System.Drawing.Size(55, 17)
        Me.rbtMetal.TabIndex = 1
        Me.rbtMetal.TabStop = True
        Me.rbtMetal.Text = "Metal"
        Me.rbtMetal.UseVisualStyleBackColor = True
        '
        'rbtNone
        '
        Me.rbtNone.AutoSize = True
        Me.rbtNone.Location = New System.Drawing.Point(6, 4)
        Me.rbtNone.Name = "rbtNone"
        Me.rbtNone.Size = New System.Drawing.Size(54, 17)
        Me.rbtNone.TabIndex = 0
        Me.rbtNone.TabStop = True
        Me.rbtNone.Text = "None"
        Me.rbtNone.UseVisualStyleBackColor = True
        '
        'rbtCounter
        '
        Me.rbtCounter.AutoSize = True
        Me.rbtCounter.Location = New System.Drawing.Point(128, 4)
        Me.rbtCounter.Name = "rbtCounter"
        Me.rbtCounter.Size = New System.Drawing.Size(71, 17)
        Me.rbtCounter.TabIndex = 2
        Me.rbtCounter.TabStop = True
        Me.rbtCounter.Text = "Counter"
        Me.rbtCounter.UseVisualStyleBackColor = True
        '
        'pnlGrid
        '
        Me.pnlGrid.Controls.Add(Me.gridView)
        Me.pnlGrid.Controls.Add(Me.gridViewHead)
        Me.pnlGrid.Controls.Add(Me.lblTitle)
        Me.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlGrid.Location = New System.Drawing.Point(0, 179)
        Me.pnlGrid.Name = "pnlGrid"
        Me.pnlGrid.Size = New System.Drawing.Size(1019, 453)
        Me.pnlGrid.TabIndex = 1
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.AllowUserToResizeRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.ContextMenuStrip = Me.ContextMenuStrip2
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 41)
        Me.gridView.MultiSelect = False
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(1019, 412)
        Me.gridView.StandardTab = True
        Me.gridView.TabIndex = 2
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResizeToolStripMenuItem})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(107, 26)
        '
        'ResizeToolStripMenuItem
        '
        Me.ResizeToolStripMenuItem.CheckOnClick = True
        Me.ResizeToolStripMenuItem.Name = "ResizeToolStripMenuItem"
        Me.ResizeToolStripMenuItem.Size = New System.Drawing.Size(106, 22)
        Me.ResizeToolStripMenuItem.Text = "Resize"
        '
        'gridViewHead
        '
        Me.gridViewHead.AllowUserToAddRows = False
        Me.gridViewHead.AllowUserToDeleteRows = False
        Me.gridViewHead.AllowUserToResizeColumns = False
        Me.gridViewHead.AllowUserToResizeRows = False
        Me.gridViewHead.BackgroundColor = System.Drawing.SystemColors.ControlDark
        Me.gridViewHead.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridViewHead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewHead.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridViewHead.Location = New System.Drawing.Point(0, 23)
        Me.gridViewHead.MultiSelect = False
        Me.gridViewHead.Name = "gridViewHead"
        Me.gridViewHead.ReadOnly = True
        Me.gridViewHead.RowHeadersVisible = False
        Me.gridViewHead.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridViewHead.Size = New System.Drawing.Size(1019, 18)
        Me.gridViewHead.StandardTab = True
        Me.gridViewHead.TabIndex = 1
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1019, 23)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "TITLE"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
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
        Me.NewToolStripMenuItem.Text = "&New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "&Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'frmSalesPersonPerformance
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1019, 632)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlGrid)
        Me.Controls.Add(Me.grpControls)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "frmSalesPersonPerformance"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "SalesPersonPerformance"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpControls.ResumeLayout(False)
        Me.grpControls.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlGrid.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip2.ResumeLayout(False)
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpControls As System.Windows.Forms.GroupBox
    Friend WithEvents lblSalePerson As System.Windows.Forms.Label
    Friend WithEvents lblDateTo As System.Windows.Forms.Label
    Friend WithEvents lblDateFrom As System.Windows.Forms.Label
    Friend WithEvents pnlGrid As System.Windows.Forms.Panel
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents gridViewHead As System.Windows.Forms.DataGridView
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents dtpTo As GiritechPack.DatePicker
    Friend WithEvents dtpFrom As GiritechPack.DatePicker
    Friend WithEvents rbtCounter As System.Windows.Forms.RadioButton
    Friend WithEvents rbtNone As System.Windows.Forms.RadioButton
    Friend WithEvents rbtMetal As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbMetal As System.Windows.Forms.ComboBox
    Friend WithEvents lABEL60 As System.Windows.Forms.Label
    Friend WithEvents chkCmbCostCentre As GiritechPack.CheckedComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtwithdate As System.Windows.Forms.TextBox
    Friend WithEvents withdate As System.Windows.Forms.CheckBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents rbtGRSWT As System.Windows.Forms.RadioButton
    Friend WithEvents rbtNetWT As System.Windows.Forms.RadioButton
    Friend WithEvents ChkOrderonly As System.Windows.Forms.CheckBox
    Friend WithEvents cmbSalesPerson As GiritechPack.CheckedComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbCompany As GiritechPack.CheckedComboBox
    Friend WithEvents rdbCategory As System.Windows.Forms.RadioButton
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ChkDateWise As System.Windows.Forms.CheckBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkMonthWise As System.Windows.Forms.CheckBox
    Friend WithEvents ChkWithRepair As System.Windows.Forms.CheckBox
    Friend WithEvents ChkWithOrder As System.Windows.Forms.CheckBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents chkLstItem As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkItemAll As System.Windows.Forms.CheckBox
    Friend WithEvents ChkWithAdv As System.Windows.Forms.CheckBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtTagNoDay_Num As TextBox
    Friend WithEvents chkWithSavings As CheckBox
    Friend WithEvents cmdcashcounter As ComboBox
    Friend WithEvents Label7 As Label
End Class
