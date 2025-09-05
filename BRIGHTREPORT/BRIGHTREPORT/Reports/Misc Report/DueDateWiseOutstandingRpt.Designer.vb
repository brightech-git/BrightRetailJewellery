<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DueDateWiseOutstandingRpt
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
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GrpContainer = New System.Windows.Forms.GroupBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.rbtBoth = New System.Windows.Forms.RadioButton()
        Me.rbtTobe = New System.Windows.Forms.RadioButton()
        Me.rbtCredit = New System.Windows.Forms.RadioButton()
        Me.chkCompanySelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox()
        Me.chkCostCentreSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstCompany = New System.Windows.Forms.CheckedListBox()
        Me.lblFilterCaption = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.rbtduedate = New System.Windows.Forms.RadioButton()
        Me.rbtArea = New System.Windows.Forms.RadioButton()
        Me.rbtOrderName = New System.Windows.Forms.RadioButton()
        Me.rbtOrderRunNo = New System.Windows.Forms.RadioButton()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.rbtOther = New System.Windows.Forms.RadioButton()
        Me.rbtDealerSmith = New System.Windows.Forms.RadioButton()
        Me.rbtCustomer = New System.Windows.Forms.RadioButton()
        Me.chkWithReceiptDetail = New System.Windows.Forms.CheckBox()
        Me.lblFilterBy = New System.Windows.Forms.Label()
        Me.txtFilterCaption = New System.Windows.Forms.TextBox()
        Me.cmbFilterBy = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkAsOnDate = New System.Windows.Forms.CheckBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.TabMain = New System.Windows.Forms.TabControl()
        Me.TabGen = New System.Windows.Forms.TabPage()
        Me.TabView = New System.Windows.Forms.TabPage()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.gridview = New System.Windows.Forms.DataGridView()
        Me.pnlfooter = New System.Windows.Forms.Panel()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.btnSendSms = New System.Windows.Forms.Button()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.pnlTop = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.GrpContainer.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.TabMain.SuspendLayout()
        Me.TabGen.SuspendLayout()
        Me.TabView.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.gridview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlfooter.SuspendLayout()
        Me.pnlTop.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.NewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 48)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'GrpContainer
        '
        Me.GrpContainer.ContextMenuStrip = Me.ContextMenuStrip1
        Me.GrpContainer.Controls.Add(Me.GroupBox3)
        Me.GrpContainer.Controls.Add(Me.chkCompanySelectAll)
        Me.GrpContainer.Controls.Add(Me.chkLstCostCentre)
        Me.GrpContainer.Controls.Add(Me.chkCostCentreSelectAll)
        Me.GrpContainer.Controls.Add(Me.chkLstCompany)
        Me.GrpContainer.Controls.Add(Me.lblFilterCaption)
        Me.GrpContainer.Controls.Add(Me.GroupBox2)
        Me.GrpContainer.Controls.Add(Me.GroupBox1)
        Me.GrpContainer.Controls.Add(Me.chkWithReceiptDetail)
        Me.GrpContainer.Controls.Add(Me.lblFilterBy)
        Me.GrpContainer.Controls.Add(Me.txtFilterCaption)
        Me.GrpContainer.Controls.Add(Me.cmbFilterBy)
        Me.GrpContainer.Controls.Add(Me.Label1)
        Me.GrpContainer.Controls.Add(Me.chkAsOnDate)
        Me.GrpContainer.Controls.Add(Me.dtpTo)
        Me.GrpContainer.Controls.Add(Me.dtpFrom)
        Me.GrpContainer.Controls.Add(Me.btnExit)
        Me.GrpContainer.Controls.Add(Me.btnSearch)
        Me.GrpContainer.Controls.Add(Me.btnNew)
        Me.GrpContainer.Location = New System.Drawing.Point(320, 76)
        Me.GrpContainer.Name = "GrpContainer"
        Me.GrpContainer.Size = New System.Drawing.Size(346, 441)
        Me.GrpContainer.TabIndex = 0
        Me.GrpContainer.TabStop = False
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.rbtBoth)
        Me.GroupBox3.Controls.Add(Me.rbtTobe)
        Me.GroupBox3.Controls.Add(Me.rbtCredit)
        Me.GroupBox3.Location = New System.Drawing.Point(18, 315)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(308, 36)
        Me.GroupBox3.TabIndex = 14
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = " "
        '
        'rbtBoth
        '
        Me.rbtBoth.AutoSize = True
        Me.rbtBoth.Location = New System.Drawing.Point(207, 11)
        Me.rbtBoth.Name = "rbtBoth"
        Me.rbtBoth.Size = New System.Drawing.Size(51, 17)
        Me.rbtBoth.TabIndex = 2
        Me.rbtBoth.Text = "Both"
        Me.rbtBoth.UseVisualStyleBackColor = True
        '
        'rbtTobe
        '
        Me.rbtTobe.AutoSize = True
        Me.rbtTobe.Location = New System.Drawing.Point(94, 10)
        Me.rbtTobe.Name = "rbtTobe"
        Me.rbtTobe.Size = New System.Drawing.Size(52, 17)
        Me.rbtTobe.TabIndex = 1
        Me.rbtTobe.Text = "Tobe"
        Me.rbtTobe.UseVisualStyleBackColor = True
        '
        'rbtCredit
        '
        Me.rbtCredit.AutoSize = True
        Me.rbtCredit.Checked = True
        Me.rbtCredit.Location = New System.Drawing.Point(6, 9)
        Me.rbtCredit.Name = "rbtCredit"
        Me.rbtCredit.Size = New System.Drawing.Size(60, 17)
        Me.rbtCredit.TabIndex = 0
        Me.rbtCredit.TabStop = True
        Me.rbtCredit.Text = "Credit"
        Me.rbtCredit.UseVisualStyleBackColor = True
        '
        'chkCompanySelectAll
        '
        Me.chkCompanySelectAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkCompanySelectAll.Location = New System.Drawing.Point(15, 48)
        Me.chkCompanySelectAll.Name = "chkCompanySelectAll"
        Me.chkCompanySelectAll.Size = New System.Drawing.Size(95, 17)
        Me.chkCompanySelectAll.TabIndex = 4
        Me.chkCompanySelectAll.Text = "Company"
        Me.chkCompanySelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(113, 122)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(213, 68)
        Me.chkLstCostCentre.TabIndex = 7
        '
        'chkCostCentreSelectAll
        '
        Me.chkCostCentreSelectAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkCostCentreSelectAll.Location = New System.Drawing.Point(15, 122)
        Me.chkCostCentreSelectAll.Name = "chkCostCentreSelectAll"
        Me.chkCostCentreSelectAll.Size = New System.Drawing.Size(95, 17)
        Me.chkCostCentreSelectAll.TabIndex = 6
        Me.chkCostCentreSelectAll.Text = "Cost Centre"
        Me.chkCostCentreSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCompany
        '
        Me.chkLstCompany.FormattingEnabled = True
        Me.chkLstCompany.Location = New System.Drawing.Point(113, 48)
        Me.chkLstCompany.Name = "chkLstCompany"
        Me.chkLstCompany.Size = New System.Drawing.Size(213, 68)
        Me.chkLstCompany.TabIndex = 5
        '
        'lblFilterCaption
        '
        Me.lblFilterCaption.AutoSize = True
        Me.lblFilterCaption.Location = New System.Drawing.Point(15, 258)
        Me.lblFilterCaption.Name = "lblFilterCaption"
        Me.lblFilterCaption.Size = New System.Drawing.Size(83, 13)
        Me.lblFilterCaption.TabIndex = 11
        Me.lblFilterCaption.Text = "Filter Caption"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.rbtduedate)
        Me.GroupBox2.Controls.Add(Me.rbtArea)
        Me.GroupBox2.Controls.Add(Me.rbtOrderName)
        Me.GroupBox2.Controls.Add(Me.rbtOrderRunNo)
        Me.GroupBox2.Location = New System.Drawing.Point(18, 357)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(308, 39)
        Me.GroupBox2.TabIndex = 15
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Order By"
        '
        'rbtduedate
        '
        Me.rbtduedate.AutoSize = True
        Me.rbtduedate.Location = New System.Drawing.Point(194, 14)
        Me.rbtduedate.Name = "rbtduedate"
        Me.rbtduedate.Size = New System.Drawing.Size(79, 17)
        Me.rbtduedate.TabIndex = 3
        Me.rbtduedate.Text = "Due Date"
        Me.rbtduedate.UseVisualStyleBackColor = True
        '
        'rbtArea
        '
        Me.rbtArea.AutoSize = True
        Me.rbtArea.Location = New System.Drawing.Point(133, 14)
        Me.rbtArea.Name = "rbtArea"
        Me.rbtArea.Size = New System.Drawing.Size(52, 17)
        Me.rbtArea.TabIndex = 2
        Me.rbtArea.Text = "Area"
        Me.rbtArea.UseVisualStyleBackColor = True
        '
        'rbtOrderName
        '
        Me.rbtOrderName.AutoSize = True
        Me.rbtOrderName.Checked = True
        Me.rbtOrderName.Location = New System.Drawing.Point(7, 14)
        Me.rbtOrderName.Name = "rbtOrderName"
        Me.rbtOrderName.Size = New System.Drawing.Size(58, 17)
        Me.rbtOrderName.TabIndex = 0
        Me.rbtOrderName.TabStop = True
        Me.rbtOrderName.Text = "Name"
        Me.rbtOrderName.UseVisualStyleBackColor = True
        '
        'rbtOrderRunNo
        '
        Me.rbtOrderRunNo.AutoSize = True
        Me.rbtOrderRunNo.Location = New System.Drawing.Point(65, 15)
        Me.rbtOrderRunNo.Name = "rbtOrderRunNo"
        Me.rbtOrderRunNo.Size = New System.Drawing.Size(62, 17)
        Me.rbtOrderRunNo.TabIndex = 1
        Me.rbtOrderRunNo.Text = "RunNo"
        Me.rbtOrderRunNo.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbtOther)
        Me.GroupBox1.Controls.Add(Me.rbtDealerSmith)
        Me.GroupBox1.Controls.Add(Me.rbtCustomer)
        Me.GroupBox1.Location = New System.Drawing.Point(18, 281)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(308, 36)
        Me.GroupBox1.TabIndex = 13
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = " "
        '
        'rbtOther
        '
        Me.rbtOther.AutoSize = True
        Me.rbtOther.Location = New System.Drawing.Point(207, 11)
        Me.rbtOther.Name = "rbtOther"
        Me.rbtOther.Size = New System.Drawing.Size(63, 17)
        Me.rbtOther.TabIndex = 2
        Me.rbtOther.Text = "Others"
        Me.rbtOther.UseVisualStyleBackColor = True
        '
        'rbtDealerSmith
        '
        Me.rbtDealerSmith.AutoSize = True
        Me.rbtDealerSmith.Location = New System.Drawing.Point(94, 10)
        Me.rbtDealerSmith.Name = "rbtDealerSmith"
        Me.rbtDealerSmith.Size = New System.Drawing.Size(112, 17)
        Me.rbtDealerSmith.TabIndex = 1
        Me.rbtDealerSmith.Text = "Dealer && Smith"
        Me.rbtDealerSmith.UseVisualStyleBackColor = True
        '
        'rbtCustomer
        '
        Me.rbtCustomer.AutoSize = True
        Me.rbtCustomer.Checked = True
        Me.rbtCustomer.Location = New System.Drawing.Point(6, 9)
        Me.rbtCustomer.Name = "rbtCustomer"
        Me.rbtCustomer.Size = New System.Drawing.Size(81, 17)
        Me.rbtCustomer.TabIndex = 0
        Me.rbtCustomer.TabStop = True
        Me.rbtCustomer.Text = "Customer"
        Me.rbtCustomer.UseVisualStyleBackColor = True
        '
        'chkWithReceiptDetail
        '
        Me.chkWithReceiptDetail.AutoSize = True
        Me.chkWithReceiptDetail.Location = New System.Drawing.Point(113, 202)
        Me.chkWithReceiptDetail.Name = "chkWithReceiptDetail"
        Me.chkWithReceiptDetail.Size = New System.Drawing.Size(140, 17)
        Me.chkWithReceiptDetail.TabIndex = 8
        Me.chkWithReceiptDetail.Text = "With Receipt Details"
        Me.chkWithReceiptDetail.UseVisualStyleBackColor = True
        '
        'lblFilterBy
        '
        Me.lblFilterBy.AutoSize = True
        Me.lblFilterBy.Location = New System.Drawing.Point(15, 231)
        Me.lblFilterBy.Name = "lblFilterBy"
        Me.lblFilterBy.Size = New System.Drawing.Size(54, 13)
        Me.lblFilterBy.TabIndex = 9
        Me.lblFilterBy.Text = "Filter By"
        '
        'txtFilterCaption
        '
        Me.txtFilterCaption.Location = New System.Drawing.Point(113, 254)
        Me.txtFilterCaption.Name = "txtFilterCaption"
        Me.txtFilterCaption.Size = New System.Drawing.Size(213, 21)
        Me.txtFilterCaption.TabIndex = 12
        '
        'cmbFilterBy
        '
        Me.cmbFilterBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFilterBy.FormattingEnabled = True
        Me.cmbFilterBy.Location = New System.Drawing.Point(113, 227)
        Me.cmbFilterBy.Name = "cmbFilterBy"
        Me.cmbFilterBy.Size = New System.Drawing.Size(213, 21)
        Me.cmbFilterBy.TabIndex = 10
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(208, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(20, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "To"
        '
        'chkAsOnDate
        '
        Me.chkAsOnDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAsOnDate.Location = New System.Drawing.Point(15, 20)
        Me.chkAsOnDate.Name = "chkAsOnDate"
        Me.chkAsOnDate.Size = New System.Drawing.Size(95, 17)
        Me.chkAsOnDate.TabIndex = 0
        Me.chkAsOnDate.Text = "Date From"
        Me.chkAsOnDate.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(235, 18)
        Me.dtpTo.Mask = "##-##-####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(82, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "29/09/2010"
        Me.dtpTo.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(119, 18)
        Me.dtpFrom.Mask = "##-##-####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(82, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "29/09/2010"
        Me.dtpFrom.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(226, 402)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 18
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(18, 402)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 16
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(122, 402)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 17
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'TabMain
        '
        Me.TabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.TabMain.Controls.Add(Me.TabGen)
        Me.TabMain.Controls.Add(Me.TabView)
        Me.TabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabMain.Location = New System.Drawing.Point(0, 0)
        Me.TabMain.Name = "TabMain"
        Me.TabMain.SelectedIndex = 0
        Me.TabMain.Size = New System.Drawing.Size(994, 622)
        Me.TabMain.TabIndex = 0
        '
        'TabGen
        '
        Me.TabGen.Controls.Add(Me.GrpContainer)
        Me.TabGen.Location = New System.Drawing.Point(4, 25)
        Me.TabGen.Name = "TabGen"
        Me.TabGen.Padding = New System.Windows.Forms.Padding(3)
        Me.TabGen.Size = New System.Drawing.Size(986, 593)
        Me.TabGen.TabIndex = 0
        Me.TabGen.Text = "Gen"
        Me.TabGen.UseVisualStyleBackColor = True
        '
        'TabView
        '
        Me.TabView.Controls.Add(Me.Panel1)
        Me.TabView.Controls.Add(Me.pnlTop)
        Me.TabView.Location = New System.Drawing.Point(4, 25)
        Me.TabView.Name = "TabView"
        Me.TabView.Padding = New System.Windows.Forms.Padding(3)
        Me.TabView.Size = New System.Drawing.Size(986, 593)
        Me.TabView.TabIndex = 1
        Me.TabView.Text = "View"
        Me.TabView.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.gridview)
        Me.Panel1.Controls.Add(Me.pnlfooter)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 31)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(980, 559)
        Me.Panel1.TabIndex = 1
        '
        'gridview
        '
        Me.gridview.AllowUserToAddRows = False
        Me.gridview.AllowUserToDeleteRows = False
        Me.gridview.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridview.Location = New System.Drawing.Point(0, 0)
        Me.gridview.MultiSelect = False
        Me.gridview.Name = "gridview"
        Me.gridview.RowHeadersVisible = False
        Me.gridview.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.gridview.RowTemplate.Height = 20
        Me.gridview.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridview.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridview.ShowCellToolTips = False
        Me.gridview.Size = New System.Drawing.Size(980, 517)
        Me.gridview.TabIndex = 6
        '
        'pnlfooter
        '
        Me.pnlfooter.Controls.Add(Me.Label14)
        Me.pnlfooter.Controls.Add(Me.btnSendSms)
        Me.pnlfooter.Controls.Add(Me.btnBack)
        Me.pnlfooter.Controls.Add(Me.btnPrint)
        Me.pnlfooter.Controls.Add(Me.btnExport)
        Me.pnlfooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlfooter.Location = New System.Drawing.Point(0, 517)
        Me.pnlfooter.Name = "pnlfooter"
        Me.pnlfooter.Size = New System.Drawing.Size(980, 42)
        Me.pnlfooter.TabIndex = 7
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.Red
        Me.Label14.Location = New System.Drawing.Point(24, 15)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(193, 14)
        Me.Label14.TabIndex = 4
        Me.Label14.Text = "Press U for Update DueDate"
        '
        'btnSendSms
        '
        Me.btnSendSms.Location = New System.Drawing.Point(618, 7)
        Me.btnSendSms.Name = "btnSendSms"
        Me.btnSendSms.Size = New System.Drawing.Size(100, 30)
        Me.btnSendSms.TabIndex = 2
        Me.btnSendSms.Text = "SendSms"
        Me.btnSendSms.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(300, 7)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 1
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(512, 7)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 0
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(406, 7)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 0
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'pnlTop
        '
        Me.pnlTop.Controls.Add(Me.lblTitle)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(3, 3)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(980, 28)
        Me.pnlTop.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(980, 28)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DueDateWiseOutstandingRpt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(994, 622)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.TabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "DueDateWiseOutstandingRpt"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "DueDateWiseOutstandingRpt"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.GrpContainer.ResumeLayout(False)
        Me.GrpContainer.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TabMain.ResumeLayout(False)
        Me.TabGen.ResumeLayout(False)
        Me.TabView.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        CType(Me.gridview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlfooter.ResumeLayout(False)
        Me.pnlfooter.PerformLayout()
        Me.pnlTop.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GrpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents chkCostCentreSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkAsOnDate As System.Windows.Forms.CheckBox
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents chkWithReceiptDetail As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents rbtduedate As System.Windows.Forms.RadioButton
    Friend WithEvents rbtOrderName As System.Windows.Forms.RadioButton
    Friend WithEvents rbtArea As System.Windows.Forms.RadioButton
    Friend WithEvents rbtOrderRunNo As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbtDealerSmith As System.Windows.Forms.RadioButton
    Friend WithEvents rbtCustomer As System.Windows.Forms.RadioButton
    Friend WithEvents rbtOther As System.Windows.Forms.RadioButton
    Friend WithEvents lblFilterCaption As System.Windows.Forms.Label
    Friend WithEvents lblFilterBy As System.Windows.Forms.Label
    Friend WithEvents txtFilterCaption As System.Windows.Forms.TextBox
    Friend WithEvents cmbFilterBy As System.Windows.Forms.ComboBox
    Friend WithEvents TabMain As System.Windows.Forms.TabControl
    Friend WithEvents TabGen As System.Windows.Forms.TabPage
    Friend WithEvents TabView As System.Windows.Forms.TabPage
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents gridview As System.Windows.Forms.DataGridView
    Friend WithEvents pnlfooter As System.Windows.Forms.Panel
    Friend WithEvents btnSendSms As System.Windows.Forms.Button
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents chkCompanySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCompany As System.Windows.Forms.CheckedListBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents rbtBoth As RadioButton
    Friend WithEvents rbtTobe As RadioButton
    Friend WithEvents rbtCredit As RadioButton
End Class
