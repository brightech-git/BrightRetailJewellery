<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDailyAbstractTT
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
        Me.components = New System.ComponentModel.Container
        Me.chkCompanySelectAll = New System.Windows.Forms.CheckBox
        Me.chkLstCompany = New System.Windows.Forms.CheckedListBox
        Me.chkCatShortname = New System.Windows.Forms.CheckBox
        Me.gbxarrange = New System.Windows.Forms.GroupBox
        Me.rbtPartyName = New System.Windows.Forms.RadioButton
        Me.rbtAmount = New System.Windows.Forms.RadioButton
        Me.chkLstNodeId = New System.Windows.Forms.CheckedListBox
        Me.chkLstCashCounter = New System.Windows.Forms.CheckedListBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.chklstCostCentre = New System.Windows.Forms.CheckedListBox
        Me.chkNetwt = New System.Windows.Forms.CheckBox
        Me.chkGrswt = New System.Windows.Forms.CheckBox
        Me.chkPcs = New System.Windows.Forms.CheckBox
        Me.chkAverage = New System.Windows.Forms.CheckBox
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.rbtMetalwise = New System.Windows.Forms.RadioButton
        Me.rbtCategoryWise = New System.Windows.Forms.RadioButton
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lbltitle = New System.Windows.Forms.Label
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExtToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.GridViewHead = New System.Windows.Forms.DataGridView
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabGen = New System.Windows.Forms.TabPage
        Me.grpFiltration = New System.Windows.Forms.GroupBox
        Me.pnlUser = New System.Windows.Forms.Panel
        Me.cmbUser = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.rbtmetalwisegrp = New System.Windows.Forms.RadioButton
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.ChkMatIssue = New System.Windows.Forms.CheckBox
        Me.chkAdvwtdetail = New System.Windows.Forms.CheckBox
        Me.ChkMetalDisc = New System.Windows.Forms.CheckBox
        Me.Chk_FinDiscount = New System.Windows.Forms.CheckBox
        Me.chkAdvdueSummary = New System.Windows.Forms.CheckBox
        Me.chkstockdetail = New System.Windows.Forms.CheckBox
        Me.chkDetaledApproval = New System.Windows.Forms.CheckBox
        Me.txtSiPurRate_Amt = New System.Windows.Forms.TextBox
        Me.lblSiPurRate = New System.Windows.Forms.Label
        Me.chkwithcreditcommision = New System.Windows.Forms.CheckBox
        Me.chkDetaledRecPay = New System.Windows.Forms.CheckBox
        Me.chkWithVat = New System.Windows.Forms.CheckBox
        Me.chkSeperateBeeds = New System.Windows.Forms.CheckBox
        Me.chkChitInfo = New System.Windows.Forms.CheckBox
        Me.chkCashOpening = New System.Windows.Forms.CheckBox
        Me.chkCancelBills = New System.Windows.Forms.CheckBox
        Me.chkHomeSale = New System.Windows.Forms.CheckBox
        Me.chkMiscRecPaySummary = New System.Windows.Forms.CheckBox
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.tabView = New System.Windows.Forms.TabPage
        Me.pnlFooter = New System.Windows.Forms.Panel
        Me.btnDotMatrix = New System.Windows.Forms.Button
        Me.btnBack = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.btnPrint = New System.Windows.Forms.Button
        Me.gbxarrange.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmbGridShortCut.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        CType(Me.GridViewHead, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabMain.SuspendLayout()
        Me.tabGen.SuspendLayout()
        Me.grpFiltration.SuspendLayout()
        Me.pnlUser.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.pnlFooter.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkCompanySelectAll
        '
        Me.chkCompanySelectAll.AutoSize = True
        Me.chkCompanySelectAll.Location = New System.Drawing.Point(18, 34)
        Me.chkCompanySelectAll.Name = "chkCompanySelectAll"
        Me.chkCompanySelectAll.Size = New System.Drawing.Size(81, 17)
        Me.chkCompanySelectAll.TabIndex = 4
        Me.chkCompanySelectAll.Text = "Company"
        Me.chkCompanySelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCompany
        '
        Me.chkLstCompany.FormattingEnabled = True
        Me.chkLstCompany.Location = New System.Drawing.Point(15, 53)
        Me.chkLstCompany.Name = "chkLstCompany"
        Me.chkLstCompany.Size = New System.Drawing.Size(432, 68)
        Me.chkLstCompany.TabIndex = 5
        '
        'chkCatShortname
        '
        Me.chkCatShortname.AutoSize = True
        Me.chkCatShortname.Location = New System.Drawing.Point(125, 6)
        Me.chkCatShortname.Name = "chkCatShortname"
        Me.chkCatShortname.Size = New System.Drawing.Size(151, 17)
        Me.chkCatShortname.TabIndex = 1
        Me.chkCatShortname.Text = "Category Short Name"
        Me.chkCatShortname.UseVisualStyleBackColor = True
        '
        'gbxarrange
        '
        Me.gbxarrange.Controls.Add(Me.rbtPartyName)
        Me.gbxarrange.Controls.Add(Me.rbtAmount)
        Me.gbxarrange.Location = New System.Drawing.Point(249, 248)
        Me.gbxarrange.Name = "gbxarrange"
        Me.gbxarrange.Size = New System.Drawing.Size(185, 41)
        Me.gbxarrange.TabIndex = 14
        Me.gbxarrange.TabStop = False
        Me.gbxarrange.Text = "Amount Order By"
        '
        'rbtPartyName
        '
        Me.rbtPartyName.AutoSize = True
        Me.rbtPartyName.Location = New System.Drawing.Point(12, 18)
        Me.rbtPartyName.Name = "rbtPartyName"
        Me.rbtPartyName.Size = New System.Drawing.Size(88, 17)
        Me.rbtPartyName.TabIndex = 0
        Me.rbtPartyName.Text = "PartyName"
        Me.rbtPartyName.UseVisualStyleBackColor = True
        '
        'rbtAmount
        '
        Me.rbtAmount.AutoSize = True
        Me.rbtAmount.Checked = True
        Me.rbtAmount.Location = New System.Drawing.Point(106, 18)
        Me.rbtAmount.Name = "rbtAmount"
        Me.rbtAmount.Size = New System.Drawing.Size(69, 17)
        Me.rbtAmount.TabIndex = 1
        Me.rbtAmount.TabStop = True
        Me.rbtAmount.Text = "Amount"
        Me.rbtAmount.UseVisualStyleBackColor = True
        '
        'chkLstNodeId
        '
        Me.chkLstNodeId.FormattingEnabled = True
        Me.chkLstNodeId.Location = New System.Drawing.Point(307, 146)
        Me.chkLstNodeId.Name = "chkLstNodeId"
        Me.chkLstNodeId.Size = New System.Drawing.Size(140, 100)
        Me.chkLstNodeId.TabIndex = 11
        '
        'chkLstCashCounter
        '
        Me.chkLstCashCounter.FormattingEnabled = True
        Me.chkLstCashCounter.Location = New System.Drawing.Point(161, 146)
        Me.chkLstCashCounter.Name = "chkLstCashCounter"
        Me.chkLstCashCounter.Size = New System.Drawing.Size(140, 100)
        Me.chkLstCashCounter.TabIndex = 9
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(270, 530)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 20
        Me.btnExit.Text = "Exit[F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'chklstCostCentre
        '
        Me.chklstCostCentre.FormattingEnabled = True
        Me.chklstCostCentre.Location = New System.Drawing.Point(15, 146)
        Me.chklstCostCentre.Name = "chklstCostCentre"
        Me.chklstCostCentre.Size = New System.Drawing.Size(140, 100)
        Me.chklstCostCentre.TabIndex = 7
        '
        'chkNetwt
        '
        Me.chkNetwt.AutoSize = True
        Me.chkNetwt.Checked = True
        Me.chkNetwt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkNetwt.Enabled = False
        Me.chkNetwt.Location = New System.Drawing.Point(3, 44)
        Me.chkNetwt.Name = "chkNetwt"
        Me.chkNetwt.Size = New System.Drawing.Size(64, 17)
        Me.chkNetwt.TabIndex = 6
        Me.chkNetwt.Text = "Net Wt"
        Me.chkNetwt.UseVisualStyleBackColor = True
        '
        'chkGrswt
        '
        Me.chkGrswt.AutoSize = True
        Me.chkGrswt.Checked = True
        Me.chkGrswt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkGrswt.Enabled = False
        Me.chkGrswt.Location = New System.Drawing.Point(3, 26)
        Me.chkGrswt.Name = "chkGrswt"
        Me.chkGrswt.Size = New System.Drawing.Size(78, 17)
        Me.chkGrswt.TabIndex = 3
        Me.chkGrswt.Text = "Gross Wt"
        Me.chkGrswt.UseVisualStyleBackColor = True
        '
        'chkPcs
        '
        Me.chkPcs.AutoSize = True
        Me.chkPcs.Checked = True
        Me.chkPcs.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPcs.Location = New System.Drawing.Point(3, 6)
        Me.chkPcs.Name = "chkPcs"
        Me.chkPcs.Size = New System.Drawing.Size(62, 17)
        Me.chkPcs.TabIndex = 0
        Me.chkPcs.Text = "Pieces"
        Me.chkPcs.UseVisualStyleBackColor = True
        '
        'chkAverage
        '
        Me.chkAverage.AutoSize = True
        Me.chkAverage.Checked = True
        Me.chkAverage.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAverage.Location = New System.Drawing.Point(3, 64)
        Me.chkAverage.Name = "chkAverage"
        Me.chkAverage.Size = New System.Drawing.Size(104, 17)
        Me.chkAverage.TabIndex = 9
        Me.chkAverage.Text = "Rate Average"
        Me.chkAverage.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(164, 530)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 19
        Me.btnNew.Text = "New[F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(56, 530)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 18
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'rbtMetalwise
        '
        Me.rbtMetalwise.AutoSize = True
        Me.rbtMetalwise.Location = New System.Drawing.Point(141, 250)
        Me.rbtMetalwise.Name = "rbtMetalwise"
        Me.rbtMetalwise.Size = New System.Drawing.Size(82, 17)
        Me.rbtMetalwise.TabIndex = 13
        Me.rbtMetalwise.TabStop = True
        Me.rbtMetalwise.Text = "MetalWise"
        Me.rbtMetalwise.UseVisualStyleBackColor = True
        '
        'rbtCategoryWise
        '
        Me.rbtCategoryWise.AutoSize = True
        Me.rbtCategoryWise.Checked = True
        Me.rbtCategoryWise.Location = New System.Drawing.Point(20, 250)
        Me.rbtCategoryWise.Name = "rbtCategoryWise"
        Me.rbtCategoryWise.Size = New System.Drawing.Size(105, 17)
        Me.rbtCategoryWise.TabIndex = 12
        Me.rbtCategoryWise.TabStop = True
        Me.rbtCategoryWise.Text = "CategoryWise"
        Me.rbtCategoryWise.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(307, 125)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(59, 16)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Node Id"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(161, 125)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(97, 16)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Cash Counter"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(15, 125)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(87, 16)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Cost Center"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(262, 17)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(21, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(67, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date From"
        '
        'lbltitle
        '
        Me.lbltitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lbltitle.Location = New System.Drawing.Point(3, 3)
        Me.lbltitle.Name = "lbltitle"
        Me.lbltitle.Size = New System.Drawing.Size(1006, 24)
        Me.lbltitle.TabIndex = 6
        Me.lbltitle.Text = "TITLE"
        Me.lbltitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.ContextMenuStrip = Me.cmbGridShortCut
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(3, 45)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(1006, 488)
        Me.gridView.TabIndex = 1
        '
        'cmbGridShortCut
        '
        Me.cmbGridShortCut.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResizeToolStripMenuItem})
        Me.cmbGridShortCut.Name = "ContextMenuStrip2"
        Me.cmbGridShortCut.Size = New System.Drawing.Size(136, 26)
        '
        'ResizeToolStripMenuItem
        '
        Me.ResizeToolStripMenuItem.CheckOnClick = True
        Me.ResizeToolStripMenuItem.Name = "ResizeToolStripMenuItem"
        Me.ResizeToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.ResizeToolStripMenuItem.Text = "Auto Resize"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExtToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 49)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.Padding = New System.Windows.Forms.Padding(32, 2, 1, 1)
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(150, 23)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExtToolStripMenuItem
        '
        Me.ExtToolStripMenuItem.Name = "ExtToolStripMenuItem"
        Me.ExtToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExtToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExtToolStripMenuItem.Text = "Exit"
        Me.ExtToolStripMenuItem.Visible = False
        '
        'GridViewHead
        '
        Me.GridViewHead.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.GridViewHead.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.GridViewHead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridViewHead.Dock = System.Windows.Forms.DockStyle.Top
        Me.GridViewHead.Enabled = False
        Me.GridViewHead.Location = New System.Drawing.Point(3, 27)
        Me.GridViewHead.Name = "GridViewHead"
        Me.GridViewHead.Size = New System.Drawing.Size(1006, 18)
        Me.GridViewHead.TabIndex = 0
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGen)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1020, 598)
        Me.tabMain.TabIndex = 0
        '
        'tabGen
        '
        Me.tabGen.Controls.Add(Me.grpFiltration)
        Me.tabGen.Location = New System.Drawing.Point(4, 22)
        Me.tabGen.Name = "tabGen"
        Me.tabGen.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGen.Size = New System.Drawing.Size(1012, 572)
        Me.tabGen.TabIndex = 0
        Me.tabGen.Text = "General"
        Me.tabGen.UseVisualStyleBackColor = True
        '
        'grpFiltration
        '
        Me.grpFiltration.Controls.Add(Me.pnlUser)
        Me.grpFiltration.Controls.Add(Me.rbtmetalwisegrp)
        Me.grpFiltration.Controls.Add(Me.Panel1)
        Me.grpFiltration.Controls.Add(Me.gbxarrange)
        Me.grpFiltration.Controls.Add(Me.chkCompanySelectAll)
        Me.grpFiltration.Controls.Add(Me.Label1)
        Me.grpFiltration.Controls.Add(Me.dtpTo)
        Me.grpFiltration.Controls.Add(Me.Label2)
        Me.grpFiltration.Controls.Add(Me.chkLstCompany)
        Me.grpFiltration.Controls.Add(Me.Label3)
        Me.grpFiltration.Controls.Add(Me.dtpFrom)
        Me.grpFiltration.Controls.Add(Me.Label4)
        Me.grpFiltration.Controls.Add(Me.Label5)
        Me.grpFiltration.Controls.Add(Me.rbtCategoryWise)
        Me.grpFiltration.Controls.Add(Me.chkLstNodeId)
        Me.grpFiltration.Controls.Add(Me.rbtMetalwise)
        Me.grpFiltration.Controls.Add(Me.chkLstCashCounter)
        Me.grpFiltration.Controls.Add(Me.btnView_Search)
        Me.grpFiltration.Controls.Add(Me.btnExit)
        Me.grpFiltration.Controls.Add(Me.btnNew)
        Me.grpFiltration.Controls.Add(Me.chklstCostCentre)
        Me.grpFiltration.Location = New System.Drawing.Point(226, 0)
        Me.grpFiltration.Name = "grpFiltration"
        Me.grpFiltration.Size = New System.Drawing.Size(461, 569)
        Me.grpFiltration.TabIndex = 0
        Me.grpFiltration.TabStop = False
        '
        'pnlUser
        '
        Me.pnlUser.Controls.Add(Me.cmbUser)
        Me.pnlUser.Controls.Add(Me.Label6)
        Me.pnlUser.Location = New System.Drawing.Point(26, 294)
        Me.pnlUser.Name = "pnlUser"
        Me.pnlUser.Size = New System.Drawing.Size(269, 32)
        Me.pnlUser.TabIndex = 16
        '
        'cmbUser
        '
        Me.cmbUser.FormattingEnabled = True
        Me.cmbUser.Location = New System.Drawing.Point(66, 6)
        Me.cmbUser.Name = "cmbUser"
        Me.cmbUser.Size = New System.Drawing.Size(198, 21)
        Me.cmbUser.TabIndex = 1
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(11, 5)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(56, 21)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "User"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'rbtmetalwisegrp
        '
        Me.rbtmetalwisegrp.AutoSize = True
        Me.rbtmetalwisegrp.Location = New System.Drawing.Point(21, 276)
        Me.rbtmetalwisegrp.Name = "rbtmetalwisegrp"
        Me.rbtmetalwisegrp.Size = New System.Drawing.Size(140, 17)
        Me.rbtmetalwisegrp.TabIndex = 15
        Me.rbtmetalwisegrp.TabStop = True
        Me.rbtmetalwisegrp.Text = "MetalWise Group By"
        Me.rbtmetalwisegrp.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.ChkMatIssue)
        Me.Panel1.Controls.Add(Me.chkAdvwtdetail)
        Me.Panel1.Controls.Add(Me.ChkMetalDisc)
        Me.Panel1.Controls.Add(Me.Chk_FinDiscount)
        Me.Panel1.Controls.Add(Me.chkAdvdueSummary)
        Me.Panel1.Controls.Add(Me.chkstockdetail)
        Me.Panel1.Controls.Add(Me.chkDetaledApproval)
        Me.Panel1.Controls.Add(Me.txtSiPurRate_Amt)
        Me.Panel1.Controls.Add(Me.lblSiPurRate)
        Me.Panel1.Controls.Add(Me.chkwithcreditcommision)
        Me.Panel1.Controls.Add(Me.chkDetaledRecPay)
        Me.Panel1.Controls.Add(Me.chkWithVat)
        Me.Panel1.Controls.Add(Me.chkPcs)
        Me.Panel1.Controls.Add(Me.chkSeperateBeeds)
        Me.Panel1.Controls.Add(Me.chkGrswt)
        Me.Panel1.Controls.Add(Me.chkChitInfo)
        Me.Panel1.Controls.Add(Me.chkNetwt)
        Me.Panel1.Controls.Add(Me.chkCashOpening)
        Me.Panel1.Controls.Add(Me.chkAverage)
        Me.Panel1.Controls.Add(Me.chkCancelBills)
        Me.Panel1.Controls.Add(Me.chkCatShortname)
        Me.Panel1.Controls.Add(Me.chkHomeSale)
        Me.Panel1.Controls.Add(Me.chkMiscRecPaySummary)
        Me.Panel1.Location = New System.Drawing.Point(15, 327)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(432, 186)
        Me.Panel1.TabIndex = 17
        '
        'ChkMatIssue
        '
        Me.ChkMatIssue.AutoSize = True
        Me.ChkMatIssue.Location = New System.Drawing.Point(3, 161)
        Me.ChkMatIssue.Name = "ChkMatIssue"
        Me.ChkMatIssue.Size = New System.Drawing.Size(135, 17)
        Me.ChkMatIssue.TabIndex = 22
        Me.ChkMatIssue.Text = "With Material Issue"
        Me.ChkMatIssue.UseVisualStyleBackColor = True
        '
        'chkAdvwtdetail
        '
        Me.chkAdvwtdetail.AutoSize = True
        Me.chkAdvwtdetail.Checked = True
        Me.chkAdvwtdetail.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAdvwtdetail.Location = New System.Drawing.Point(282, 123)
        Me.chkAdvwtdetail.Name = "chkAdvwtdetail"
        Me.chkAdvwtdetail.Size = New System.Drawing.Size(122, 17)
        Me.chkAdvwtdetail.TabIndex = 21
        Me.chkAdvwtdetail.Text = "Detailed Adv Wt."
        Me.chkAdvwtdetail.UseVisualStyleBackColor = True
        '
        'ChkMetalDisc
        '
        Me.ChkMetalDisc.AutoSize = True
        Me.ChkMetalDisc.Location = New System.Drawing.Point(3, 123)
        Me.ChkMetalDisc.Name = "ChkMetalDisc"
        Me.ChkMetalDisc.Size = New System.Drawing.Size(158, 17)
        Me.ChkMetalDisc.TabIndex = 20
        Me.ChkMetalDisc.Text = "With Metal wise Disc %"
        Me.ChkMetalDisc.UseVisualStyleBackColor = True
        '
        'Chk_FinDiscount
        '
        Me.Chk_FinDiscount.AutoSize = True
        Me.Chk_FinDiscount.Location = New System.Drawing.Point(282, 104)
        Me.Chk_FinDiscount.Name = "Chk_FinDiscount"
        Me.Chk_FinDiscount.Size = New System.Drawing.Size(131, 17)
        Me.Chk_FinDiscount.TabIndex = 16
        Me.Chk_FinDiscount.Text = "With ItemDiscount"
        Me.Chk_FinDiscount.UseVisualStyleBackColor = True
        '
        'chkAdvdueSummary
        '
        Me.chkAdvdueSummary.AutoSize = True
        Me.chkAdvdueSummary.Location = New System.Drawing.Point(3, 141)
        Me.chkAdvdueSummary.Name = "chkAdvdueSummary"
        Me.chkAdvdueSummary.Size = New System.Drawing.Size(173, 17)
        Me.chkAdvdueSummary.TabIndex = 17
        Me.chkAdvdueSummary.Text = "With Adv - Due Summary"
        Me.chkAdvdueSummary.UseVisualStyleBackColor = True
        '
        'chkstockdetail
        '
        Me.chkstockdetail.AutoSize = True
        Me.chkstockdetail.Location = New System.Drawing.Point(125, 82)
        Me.chkstockdetail.Name = "chkstockdetail"
        Me.chkstockdetail.Size = New System.Drawing.Size(137, 17)
        Me.chkstockdetail.TabIndex = 13
        Me.chkstockdetail.Text = "With Stock Addition"
        Me.chkstockdetail.UseVisualStyleBackColor = True
        '
        'chkDetaledApproval
        '
        Me.chkDetaledApproval.AutoSize = True
        Me.chkDetaledApproval.Location = New System.Drawing.Point(282, 84)
        Me.chkDetaledApproval.Name = "chkDetaledApproval"
        Me.chkDetaledApproval.Size = New System.Drawing.Size(128, 17)
        Me.chkDetaledApproval.TabIndex = 14
        Me.chkDetaledApproval.Text = "Detailed Approval"
        Me.chkDetaledApproval.UseVisualStyleBackColor = True
        '
        'txtSiPurRate_Amt
        '
        Me.txtSiPurRate_Amt.Location = New System.Drawing.Point(396, 142)
        Me.txtSiPurRate_Amt.Name = "txtSiPurRate_Amt"
        Me.txtSiPurRate_Amt.Size = New System.Drawing.Size(31, 21)
        Me.txtSiPurRate_Amt.TabIndex = 19
        Me.txtSiPurRate_Amt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblSiPurRate
        '
        Me.lblSiPurRate.Location = New System.Drawing.Point(241, 143)
        Me.lblSiPurRate.Name = "lblSiPurRate"
        Me.lblSiPurRate.Size = New System.Drawing.Size(158, 20)
        Me.lblSiPurRate.TabIndex = 18
        Me.lblSiPurRate.Text = "Silver Purchase Rate Less"
        Me.lblSiPurRate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkwithcreditcommision
        '
        Me.chkwithcreditcommision.AutoSize = True
        Me.chkwithcreditcommision.Checked = True
        Me.chkwithcreditcommision.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkwithcreditcommision.Location = New System.Drawing.Point(3, 103)
        Me.chkwithcreditcommision.Name = "chkwithcreditcommision"
        Me.chkwithcreditcommision.Size = New System.Drawing.Size(196, 17)
        Me.chkwithcreditcommision.TabIndex = 15
        Me.chkwithcreditcommision.Text = "With Credit Card Commission"
        Me.chkwithcreditcommision.UseVisualStyleBackColor = True
        '
        'chkDetaledRecPay
        '
        Me.chkDetaledRecPay.AutoSize = True
        Me.chkDetaledRecPay.Checked = True
        Me.chkDetaledRecPay.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDetaledRecPay.Location = New System.Drawing.Point(3, 84)
        Me.chkDetaledRecPay.Name = "chkDetaledRecPay"
        Me.chkDetaledRecPay.Size = New System.Drawing.Size(119, 17)
        Me.chkDetaledRecPay.TabIndex = 12
        Me.chkDetaledRecPay.Text = "Detailed RecPay"
        Me.chkDetaledRecPay.UseVisualStyleBackColor = True
        '
        'chkWithVat
        '
        Me.chkWithVat.AutoSize = True
        Me.chkWithVat.Location = New System.Drawing.Point(282, 44)
        Me.chkWithVat.Name = "chkWithVat"
        Me.chkWithVat.Size = New System.Drawing.Size(78, 17)
        Me.chkWithVat.TabIndex = 8
        Me.chkWithVat.Text = "With VAT"
        Me.chkWithVat.UseVisualStyleBackColor = True
        '
        'chkSeperateBeeds
        '
        Me.chkSeperateBeeds.AutoSize = True
        Me.chkSeperateBeeds.Location = New System.Drawing.Point(282, 26)
        Me.chkSeperateBeeds.Name = "chkSeperateBeeds"
        Me.chkSeperateBeeds.Size = New System.Drawing.Size(117, 17)
        Me.chkSeperateBeeds.TabIndex = 5
        Me.chkSeperateBeeds.Text = "Seperate Beeds"
        Me.chkSeperateBeeds.UseVisualStyleBackColor = True
        '
        'chkChitInfo
        '
        Me.chkChitInfo.AutoSize = True
        Me.chkChitInfo.Location = New System.Drawing.Point(282, 6)
        Me.chkChitInfo.Name = "chkChitInfo"
        Me.chkChitInfo.Size = New System.Drawing.Size(101, 17)
        Me.chkChitInfo.TabIndex = 2
        Me.chkChitInfo.Text = "SCHEME Info"
        Me.chkChitInfo.UseVisualStyleBackColor = True
        '
        'chkCashOpening
        '
        Me.chkCashOpening.AutoSize = True
        Me.chkCashOpening.Location = New System.Drawing.Point(282, 64)
        Me.chkCashOpening.Name = "chkCashOpening"
        Me.chkCashOpening.Size = New System.Drawing.Size(135, 17)
        Me.chkCashOpening.TabIndex = 11
        Me.chkCashOpening.Text = "With Cash Opening"
        Me.chkCashOpening.UseVisualStyleBackColor = True
        '
        'chkCancelBills
        '
        Me.chkCancelBills.AutoSize = True
        Me.chkCancelBills.Location = New System.Drawing.Point(125, 64)
        Me.chkCancelBills.Name = "chkCancelBills"
        Me.chkCancelBills.Size = New System.Drawing.Size(123, 17)
        Me.chkCancelBills.TabIndex = 10
        Me.chkCancelBills.Text = "Cancel Bill Detail"
        Me.chkCancelBills.UseVisualStyleBackColor = True
        '
        'chkHomeSale
        '
        Me.chkHomeSale.AutoSize = True
        Me.chkHomeSale.Location = New System.Drawing.Point(125, 26)
        Me.chkHomeSale.Name = "chkHomeSale"
        Me.chkHomeSale.Size = New System.Drawing.Size(88, 17)
        Me.chkHomeSale.TabIndex = 4
        Me.chkHomeSale.Text = "Home Sale"
        Me.chkHomeSale.UseVisualStyleBackColor = True
        '
        'chkMiscRecPaySummary
        '
        Me.chkMiscRecPaySummary.AutoSize = True
        Me.chkMiscRecPaySummary.Location = New System.Drawing.Point(125, 44)
        Me.chkMiscRecPaySummary.Name = "chkMiscRecPaySummary"
        Me.chkMiscRecPaySummary.Size = New System.Drawing.Size(156, 17)
        Me.chkMiscRecPaySummary.TabIndex = 7
        Me.chkMiscRecPaySummary.Text = "Misc RecPay Summary"
        Me.chkMiscRecPaySummary.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(289, 13)
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
        Me.dtpFrom.Location = New System.Drawing.Point(155, 13)
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
        'tabView
        '
        Me.tabView.Controls.Add(Me.gridView)
        Me.tabView.Controls.Add(Me.GridViewHead)
        Me.tabView.Controls.Add(Me.pnlFooter)
        Me.tabView.Controls.Add(Me.lbltitle)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1012, 572)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'pnlFooter
        '
        Me.pnlFooter.Controls.Add(Me.btnDotMatrix)
        Me.pnlFooter.Controls.Add(Me.btnBack)
        Me.pnlFooter.Controls.Add(Me.btnExport)
        Me.pnlFooter.Controls.Add(Me.btnPrint)
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Location = New System.Drawing.Point(3, 533)
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Size = New System.Drawing.Size(1006, 36)
        Me.pnlFooter.TabIndex = 21
        '
        'btnDotMatrix
        '
        Me.btnDotMatrix.Location = New System.Drawing.Point(905, 3)
        Me.btnDotMatrix.Name = "btnDotMatrix"
        Me.btnDotMatrix.Size = New System.Drawing.Size(100, 30)
        Me.btnDotMatrix.TabIndex = 22
        Me.btnDotMatrix.Text = "Dot Matrix"
        Me.btnDotMatrix.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(605, 3)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 21
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(705, 3)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 19
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(805, 3)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 20
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'frmDailyAbstractTT
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1020, 598)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmDailyAbstractTT"
        Me.Text = "DAILY ABSTRACT REPORT"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.gbxarrange.ResumeLayout(False)
        Me.gbxarrange.PerformLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmbGridShortCut.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        CType(Me.GridViewHead, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabMain.ResumeLayout(False)
        Me.tabGen.ResumeLayout(False)
        Me.grpFiltration.ResumeLayout(False)
        Me.grpFiltration.PerformLayout()
        Me.pnlUser.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.tabView.ResumeLayout(False)
        Me.pnlFooter.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents rbtMetalwise As System.Windows.Forms.RadioButton
    Friend WithEvents rbtCategoryWise As System.Windows.Forms.RadioButton
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents lbltitle As System.Windows.Forms.Label
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExtToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkAverage As System.Windows.Forms.CheckBox
    Friend WithEvents GridViewHead As System.Windows.Forms.DataGridView
    Friend WithEvents chkGrswt As System.Windows.Forms.CheckBox
    Friend WithEvents chkPcs As System.Windows.Forms.CheckBox
    Friend WithEvents chkNetwt As System.Windows.Forms.CheckBox
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGen As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents chklstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents chkLstCashCounter As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents chkLstNodeId As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel
    Friend WithEvents rbtAmount As System.Windows.Forms.RadioButton
    Friend WithEvents rbtPartyName As System.Windows.Forms.RadioButton
    Friend WithEvents gbxarrange As System.Windows.Forms.GroupBox
    Friend WithEvents chkCatShortname As System.Windows.Forms.CheckBox
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkCompanySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCompany As System.Windows.Forms.CheckedListBox
    Friend WithEvents grpFiltration As System.Windows.Forms.GroupBox
    Friend WithEvents chkHomeSale As System.Windows.Forms.CheckBox
    Friend WithEvents chkMiscRecPaySummary As System.Windows.Forms.CheckBox
    Friend WithEvents chkCancelBills As System.Windows.Forms.CheckBox
    Friend WithEvents chkCashOpening As System.Windows.Forms.CheckBox
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkChitInfo As System.Windows.Forms.CheckBox
    Friend WithEvents chkSeperateBeeds As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithVat As System.Windows.Forms.CheckBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblSiPurRate As System.Windows.Forms.Label
    Friend WithEvents txtSiPurRate_Amt As System.Windows.Forms.TextBox
    Friend WithEvents btnDotMatrix As System.Windows.Forms.Button
    Friend WithEvents chkDetaledRecPay As System.Windows.Forms.CheckBox
    Friend WithEvents chkDetaledApproval As System.Windows.Forms.CheckBox
    Friend WithEvents rbtmetalwisegrp As System.Windows.Forms.RadioButton
    Friend WithEvents chkstockdetail As System.Windows.Forms.CheckBox
    Friend WithEvents chkwithcreditcommision As System.Windows.Forms.CheckBox
    Friend WithEvents chkAdvdueSummary As System.Windows.Forms.CheckBox
    Friend WithEvents Chk_FinDiscount As System.Windows.Forms.CheckBox
    Friend WithEvents pnlUser As System.Windows.Forms.Panel
    Friend WithEvents cmbUser As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ChkMetalDisc As System.Windows.Forms.CheckBox
    Friend WithEvents chkAdvwtdetail As System.Windows.Forms.CheckBox
    Friend WithEvents ChkMatIssue As System.Windows.Forms.CheckBox
End Class
