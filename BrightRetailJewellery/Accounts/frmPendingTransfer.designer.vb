<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPendingTransfer
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.page1 = New System.Windows.Forms.TabPage()
        Me.grpGrid = New System.Windows.Forms.GroupBox()
        Me.txtGrid = New System.Windows.Forms.TextBox()
        Me.gridViewtotal = New System.Windows.Forms.DataGridView()
        Me.cmbTransferTo = New System.Windows.Forms.ComboBox()
        Me.chkAll = New System.Windows.Forms.CheckBox()
        Me.gridView_OWN = New System.Windows.Forms.DataGridView()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.CmbAcname_MAN = New System.Windows.Forms.ComboBox()
        Me.lblAcname = New System.Windows.Forms.Label()
        Me.cmbEntryType_MAN = New System.Windows.Forms.ComboBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.cmbCompany_MAN = New System.Windows.Forms.ComboBox()
        Me.chkToType = New System.Windows.Forms.CheckBox()
        Me.chkPosting = New System.Windows.Forms.CheckBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.CmbCategory = New System.Windows.Forms.ComboBox()
        Me.btnOpen = New System.Windows.Forms.Button()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbOpenMetal = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbCostCentre_MAN = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.chkPurchase = New System.Windows.Forms.CheckBox()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.chkSalesReturn = New System.Windows.Forms.CheckBox()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.chkMiscIssue = New System.Windows.Forms.CheckBox()
        Me.chkPartlySale = New System.Windows.Forms.CheckBox()
        Me.grpPurchase = New System.Windows.Forms.GroupBox()
        Me.chkPurchaseOwn = New System.Windows.Forms.CheckBox()
        Me.chkPurchaseExchange = New System.Windows.Forms.CheckBox()
        Me.page2 = New System.Windows.Forms.TabPage()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.dGridView = New System.Windows.Forms.DataGridView()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.cmbOpenCompanyView = New System.Windows.Forms.ComboBox()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.dtpToView = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFromView = New BrighttechPack.DatePicker(Me.components)
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cmbOpenMetalView = New System.Windows.Forms.ComboBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cmbCostCenterName = New System.Windows.Forms.ComboBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.chkPurchaseView = New System.Windows.Forms.CheckBox()
        Me.chkSalesReturnView = New System.Windows.Forms.CheckBox()
        Me.chkMiscIssueView = New System.Windows.Forms.CheckBox()
        Me.chkPartysaleView = New System.Windows.Forms.CheckBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnGenerate = New System.Windows.Forms.Button()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnExcel = New System.Windows.Forms.Button()
        Me.tabMain.SuspendLayout()
        Me.page1.SuspendLayout()
        Me.grpGrid.SuspendLayout()
        CType(Me.gridViewtotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.grpPurchase.SuspendLayout()
        Me.page2.SuspendLayout()
        Me.Panel4.SuspendLayout()
        CType(Me.dGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.page1)
        Me.tabMain.Controls.Add(Me.page2)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1009, 591)
        Me.tabMain.TabIndex = 0
        '
        'page1
        '
        Me.page1.Controls.Add(Me.grpGrid)
        Me.page1.Controls.Add(Me.Panel2)
        Me.page1.Location = New System.Drawing.Point(4, 22)
        Me.page1.Name = "page1"
        Me.page1.Padding = New System.Windows.Forms.Padding(3)
        Me.page1.Size = New System.Drawing.Size(1001, 565)
        Me.page1.TabIndex = 0
        Me.page1.Text = "Gen"
        Me.page1.UseVisualStyleBackColor = True
        '
        'grpGrid
        '
        Me.grpGrid.Controls.Add(Me.txtGrid)
        Me.grpGrid.Controls.Add(Me.gridViewtotal)
        Me.grpGrid.Controls.Add(Me.cmbTransferTo)
        Me.grpGrid.Controls.Add(Me.chkAll)
        Me.grpGrid.Controls.Add(Me.gridView_OWN)
        Me.grpGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpGrid.Location = New System.Drawing.Point(3, 137)
        Me.grpGrid.Name = "grpGrid"
        Me.grpGrid.Size = New System.Drawing.Size(995, 425)
        Me.grpGrid.TabIndex = 1
        Me.grpGrid.TabStop = False
        '
        'txtGrid
        '
        Me.txtGrid.Location = New System.Drawing.Point(183, 137)
        Me.txtGrid.Name = "txtGrid"
        Me.txtGrid.Size = New System.Drawing.Size(100, 21)
        Me.txtGrid.TabIndex = 2
        Me.txtGrid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtGrid.Visible = False
        '
        'gridViewtotal
        '
        Me.gridViewtotal.AllowUserToAddRows = False
        Me.gridViewtotal.AllowUserToDeleteRows = False
        Me.gridViewtotal.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridViewtotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewtotal.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.gridViewtotal.Location = New System.Drawing.Point(3, 311)
        Me.gridViewtotal.MultiSelect = False
        Me.gridViewtotal.Name = "gridViewtotal"
        Me.gridViewtotal.RowHeadersVisible = False
        Me.gridViewtotal.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridViewtotal.Size = New System.Drawing.Size(989, 111)
        Me.gridViewtotal.TabIndex = 4
        '
        'cmbTransferTo
        '
        Me.cmbTransferTo.FormattingEnabled = True
        Me.cmbTransferTo.Location = New System.Drawing.Point(108, 245)
        Me.cmbTransferTo.Name = "cmbTransferTo"
        Me.cmbTransferTo.Size = New System.Drawing.Size(176, 21)
        Me.cmbTransferTo.TabIndex = 3
        Me.cmbTransferTo.Visible = False
        '
        'chkAll
        '
        Me.chkAll.AutoSize = True
        Me.chkAll.Location = New System.Drawing.Point(9, 40)
        Me.chkAll.Name = "chkAll"
        Me.chkAll.Size = New System.Drawing.Size(80, 17)
        Me.chkAll.TabIndex = 1
        Me.chkAll.Text = "Check All"
        Me.chkAll.UseVisualStyleBackColor = True
        '
        'gridView_OWN
        '
        Me.gridView_OWN.AllowUserToAddRows = False
        Me.gridView_OWN.AllowUserToDeleteRows = False
        Me.gridView_OWN.BorderStyle = System.Windows.Forms.BorderStyle.None
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.gridView_OWN.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.gridView_OWN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView_OWN.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridView_OWN.Location = New System.Drawing.Point(3, 17)
        Me.gridView_OWN.MultiSelect = False
        Me.gridView_OWN.Name = "gridView_OWN"
        Me.gridView_OWN.RowHeadersVisible = False
        Me.gridView_OWN.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.White
        Me.gridView_OWN.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black
        Me.gridView_OWN.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView_OWN.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.gridView_OWN.Size = New System.Drawing.Size(989, 249)
        Me.gridView_OWN.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.btnExcel)
        Me.Panel2.Controls.Add(Me.CmbAcname_MAN)
        Me.Panel2.Controls.Add(Me.lblAcname)
        Me.Panel2.Controls.Add(Me.cmbEntryType_MAN)
        Me.Panel2.Controls.Add(Me.Label13)
        Me.Panel2.Controls.Add(Me.cmbCompany_MAN)
        Me.Panel2.Controls.Add(Me.chkToType)
        Me.Panel2.Controls.Add(Me.chkPosting)
        Me.Panel2.Controls.Add(Me.Label12)
        Me.Panel2.Controls.Add(Me.CmbCategory)
        Me.Panel2.Controls.Add(Me.btnOpen)
        Me.Panel2.Controls.Add(Me.dtpTo)
        Me.Panel2.Controls.Add(Me.dtpFrom)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.Label6)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.cmbOpenMetal)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Controls.Add(Me.cmbCostCentre_MAN)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.btnExit)
        Me.Panel2.Controls.Add(Me.chkPurchase)
        Me.Panel2.Controls.Add(Me.btnNew)
        Me.Panel2.Controls.Add(Me.chkSalesReturn)
        Me.Panel2.Controls.Add(Me.btnLoad)
        Me.Panel2.Controls.Add(Me.chkMiscIssue)
        Me.Panel2.Controls.Add(Me.chkPartlySale)
        Me.Panel2.Controls.Add(Me.grpPurchase)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(3, 3)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(995, 134)
        Me.Panel2.TabIndex = 0
        '
        'CmbAcname_MAN
        '
        Me.CmbAcname_MAN.FormattingEnabled = True
        Me.CmbAcname_MAN.Location = New System.Drawing.Point(487, 71)
        Me.CmbAcname_MAN.Name = "CmbAcname_MAN"
        Me.CmbAcname_MAN.Size = New System.Drawing.Size(223, 21)
        Me.CmbAcname_MAN.TabIndex = 22
        Me.CmbAcname_MAN.Visible = False
        '
        'lblAcname
        '
        Me.lblAcname.Location = New System.Drawing.Point(366, 71)
        Me.lblAcname.Name = "lblAcname"
        Me.lblAcname.Size = New System.Drawing.Size(100, 21)
        Me.lblAcname.TabIndex = 21
        Me.lblAcname.Text = "Dealer Name"
        Me.lblAcname.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblAcname.Visible = False
        '
        'cmbEntryType_MAN
        '
        Me.cmbEntryType_MAN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbEntryType_MAN.FormattingEnabled = True
        Me.cmbEntryType_MAN.Location = New System.Drawing.Point(40, 100)
        Me.cmbEntryType_MAN.Name = "cmbEntryType_MAN"
        Me.cmbEntryType_MAN.Size = New System.Drawing.Size(92, 21)
        Me.cmbEntryType_MAN.TabIndex = 20
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(3, 27)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(100, 21)
        Me.Label13.TabIndex = 4
        Me.Label13.Text = "Company"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbCompany_MAN
        '
        Me.cmbCompany_MAN.FormattingEnabled = True
        Me.cmbCompany_MAN.Location = New System.Drawing.Point(136, 27)
        Me.cmbCompany_MAN.Name = "cmbCompany_MAN"
        Me.cmbCompany_MAN.Size = New System.Drawing.Size(222, 21)
        Me.cmbCompany_MAN.TabIndex = 5
        '
        'chkToType
        '
        Me.chkToType.AutoSize = True
        Me.chkToType.Location = New System.Drawing.Point(4, 102)
        Me.chkToType.Name = "chkToType"
        Me.chkToType.Size = New System.Drawing.Size(39, 17)
        Me.chkToType.TabIndex = 19
        Me.chkToType.Text = "To"
        Me.chkToType.UseVisualStyleBackColor = True
        '
        'chkPosting
        '
        Me.chkPosting.AutoSize = True
        Me.chkPosting.Location = New System.Drawing.Point(674, 29)
        Me.chkPosting.Name = "chkPosting"
        Me.chkPosting.Size = New System.Drawing.Size(67, 17)
        Me.chkPosting.TabIndex = 11
        Me.chkPosting.Text = "Posting"
        Me.chkPosting.UseVisualStyleBackColor = True
        Me.chkPosting.Visible = False
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(366, 49)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(100, 21)
        Me.Label12.TabIndex = 17
        Me.Label12.Text = "Category"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CmbCategory
        '
        Me.CmbCategory.FormattingEnabled = True
        Me.CmbCategory.Location = New System.Drawing.Point(487, 49)
        Me.CmbCategory.Name = "CmbCategory"
        Me.CmbCategory.Size = New System.Drawing.Size(222, 21)
        Me.CmbCategory.TabIndex = 18
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(370, 95)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 25
        Me.btnOpen.Text = "Open[F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(264, 5)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(94, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "06/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(136, 5)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(94, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "06/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(3, 9)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(67, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Date From"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(237, 9)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(20, 13)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "To"
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(4, 71)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(85, 21)
        Me.Label5.TabIndex = 15
        Me.Label5.Text = "Metal"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbOpenMetal
        '
        Me.cmbOpenMetal.FormattingEnabled = True
        Me.cmbOpenMetal.Location = New System.Drawing.Point(136, 71)
        Me.cmbOpenMetal.Name = "cmbOpenMetal"
        Me.cmbOpenMetal.Size = New System.Drawing.Size(222, 21)
        Me.cmbOpenMetal.TabIndex = 16
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(3, 49)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 21)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "Cost Centre"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbCostCentre_MAN
        '
        Me.cmbCostCentre_MAN.FormattingEnabled = True
        Me.cmbCostCentre_MAN.Location = New System.Drawing.Point(136, 49)
        Me.cmbCostCentre_MAN.Name = "cmbCostCentre_MAN"
        Me.cmbCostCentre_MAN.Size = New System.Drawing.Size(222, 21)
        Me.cmbCostCentre_MAN.TabIndex = 14
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(366, 5)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 21)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Pending Type"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(610, 95)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 26
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'chkPurchase
        '
        Me.chkPurchase.AutoSize = True
        Me.chkPurchase.Location = New System.Drawing.Point(487, 7)
        Me.chkPurchase.Name = "chkPurchase"
        Me.chkPurchase.Size = New System.Drawing.Size(78, 17)
        Me.chkPurchase.TabIndex = 7
        Me.chkPurchase.Text = "Purchase"
        Me.chkPurchase.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(253, 95)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 24
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'chkSalesReturn
        '
        Me.chkSalesReturn.AutoSize = True
        Me.chkSalesReturn.Location = New System.Drawing.Point(580, 7)
        Me.chkSalesReturn.Name = "chkSalesReturn"
        Me.chkSalesReturn.Size = New System.Drawing.Size(99, 17)
        Me.chkSalesReturn.TabIndex = 8
        Me.chkSalesReturn.Text = "Sales Return"
        Me.chkSalesReturn.UseVisualStyleBackColor = True
        '
        'btnLoad
        '
        Me.btnLoad.Location = New System.Drawing.Point(136, 95)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(100, 30)
        Me.btnLoad.TabIndex = 23
        Me.btnLoad.Text = "Load"
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'chkMiscIssue
        '
        Me.chkMiscIssue.AutoSize = True
        Me.chkMiscIssue.Location = New System.Drawing.Point(487, 29)
        Me.chkMiscIssue.Name = "chkMiscIssue"
        Me.chkMiscIssue.Size = New System.Drawing.Size(85, 17)
        Me.chkMiscIssue.TabIndex = 9
        Me.chkMiscIssue.Text = "Misc Issue"
        Me.chkMiscIssue.UseVisualStyleBackColor = True
        '
        'chkPartlySale
        '
        Me.chkPartlySale.AutoSize = True
        Me.chkPartlySale.Location = New System.Drawing.Point(580, 29)
        Me.chkPartlySale.Name = "chkPartlySale"
        Me.chkPartlySale.Size = New System.Drawing.Size(88, 17)
        Me.chkPartlySale.TabIndex = 10
        Me.chkPartlySale.Text = "Partly Sale"
        Me.chkPartlySale.UseVisualStyleBackColor = True
        '
        'grpPurchase
        '
        Me.grpPurchase.Controls.Add(Me.chkPurchaseOwn)
        Me.grpPurchase.Controls.Add(Me.chkPurchaseExchange)
        Me.grpPurchase.Location = New System.Drawing.Point(747, -3)
        Me.grpPurchase.Name = "grpPurchase"
        Me.grpPurchase.Size = New System.Drawing.Size(154, 41)
        Me.grpPurchase.TabIndex = 12
        Me.grpPurchase.TabStop = False
        '
        'chkPurchaseOwn
        '
        Me.chkPurchaseOwn.AutoSize = True
        Me.chkPurchaseOwn.Location = New System.Drawing.Point(9, 15)
        Me.chkPurchaseOwn.Name = "chkPurchaseOwn"
        Me.chkPurchaseOwn.Size = New System.Drawing.Size(51, 17)
        Me.chkPurchaseOwn.TabIndex = 0
        Me.chkPurchaseOwn.Text = "Own"
        Me.chkPurchaseOwn.UseVisualStyleBackColor = True
        '
        'chkPurchaseExchange
        '
        Me.chkPurchaseExchange.AutoSize = True
        Me.chkPurchaseExchange.Location = New System.Drawing.Point(69, 15)
        Me.chkPurchaseExchange.Name = "chkPurchaseExchange"
        Me.chkPurchaseExchange.Size = New System.Drawing.Size(64, 17)
        Me.chkPurchaseExchange.TabIndex = 1
        Me.chkPurchaseExchange.Text = "Others"
        Me.chkPurchaseExchange.UseVisualStyleBackColor = True
        '
        'page2
        '
        Me.page2.Controls.Add(Me.Panel4)
        Me.page2.Controls.Add(Me.Panel3)
        Me.page2.Location = New System.Drawing.Point(4, 22)
        Me.page2.Name = "page2"
        Me.page2.Padding = New System.Windows.Forms.Padding(3)
        Me.page2.Size = New System.Drawing.Size(1001, 565)
        Me.page2.TabIndex = 1
        Me.page2.Text = "View"
        Me.page2.UseVisualStyleBackColor = True
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.dGridView)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel4.Location = New System.Drawing.Point(3, 137)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(995, 422)
        Me.Panel4.TabIndex = 2
        '
        'dGridView
        '
        Me.dGridView.AllowUserToAddRows = False
        Me.dGridView.AllowUserToDeleteRows = False
        Me.dGridView.AllowUserToResizeColumns = False
        Me.dGridView.AllowUserToResizeRows = False
        Me.dGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dGridView.Location = New System.Drawing.Point(0, 0)
        Me.dGridView.Name = "dGridView"
        Me.dGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullColumnSelect
        Me.dGridView.Size = New System.Drawing.Size(995, 422)
        Me.dGridView.TabIndex = 0
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.Label14)
        Me.Panel3.Controls.Add(Me.cmbOpenCompanyView)
        Me.Panel3.Controls.Add(Me.btnPrint)
        Me.Panel3.Controls.Add(Me.btnExport)
        Me.Panel3.Controls.Add(Me.btnBack)
        Me.Panel3.Controls.Add(Me.btnSearch)
        Me.Panel3.Controls.Add(Me.dtpToView)
        Me.Panel3.Controls.Add(Me.dtpFromView)
        Me.Panel3.Controls.Add(Me.Label7)
        Me.Panel3.Controls.Add(Me.Label8)
        Me.Panel3.Controls.Add(Me.Label9)
        Me.Panel3.Controls.Add(Me.cmbOpenMetalView)
        Me.Panel3.Controls.Add(Me.Label10)
        Me.Panel3.Controls.Add(Me.cmbCostCenterName)
        Me.Panel3.Controls.Add(Me.Label11)
        Me.Panel3.Controls.Add(Me.chkPurchaseView)
        Me.Panel3.Controls.Add(Me.chkSalesReturnView)
        Me.Panel3.Controls.Add(Me.chkMiscIssueView)
        Me.Panel3.Controls.Add(Me.chkPartysaleView)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(3, 3)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(995, 134)
        Me.Panel3.TabIndex = 1
        '
        'Label14
        '
        Me.Label14.Location = New System.Drawing.Point(5, 28)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(100, 21)
        Me.Label14.TabIndex = 9
        Me.Label14.Text = "Company"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbOpenCompanyView
        '
        Me.cmbOpenCompanyView.FormattingEnabled = True
        Me.cmbOpenCompanyView.Location = New System.Drawing.Point(110, 28)
        Me.cmbOpenCompanyView.Name = "cmbOpenCompanyView"
        Me.cmbOpenCompanyView.Size = New System.Drawing.Size(222, 21)
        Me.cmbOpenCompanyView.TabIndex = 10
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(431, 100)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 18
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(324, 100)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 17
        Me.btnExport.Text = "&Export[X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(217, 100)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 16
        Me.btnBack.Text = "Back"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(110, 100)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 15
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'dtpToView
        '
        Me.dtpToView.Location = New System.Drawing.Point(238, 5)
        Me.dtpToView.Mask = "##/##/####"
        Me.dtpToView.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpToView.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpToView.Name = "dtpToView"
        Me.dtpToView.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpToView.Size = New System.Drawing.Size(94, 21)
        Me.dtpToView.TabIndex = 3
        Me.dtpToView.Text = "06/03/9998"
        Me.dtpToView.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'dtpFromView
        '
        Me.dtpFromView.Location = New System.Drawing.Point(110, 5)
        Me.dtpFromView.Mask = "##/##/####"
        Me.dtpFromView.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFromView.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFromView.Name = "dtpFromView"
        Me.dtpFromView.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFromView.Size = New System.Drawing.Size(94, 21)
        Me.dtpFromView.TabIndex = 1
        Me.dtpFromView.Text = "06/03/9998"
        Me.dtpFromView.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(3, 9)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(67, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Date From"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(210, 9)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(20, 13)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "To"
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(4, 74)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(85, 21)
        Me.Label9.TabIndex = 13
        Me.Label9.Text = "Metal"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbOpenMetalView
        '
        Me.cmbOpenMetalView.FormattingEnabled = True
        Me.cmbOpenMetalView.Location = New System.Drawing.Point(110, 74)
        Me.cmbOpenMetalView.Name = "cmbOpenMetalView"
        Me.cmbOpenMetalView.Size = New System.Drawing.Size(222, 21)
        Me.cmbOpenMetalView.TabIndex = 14
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(3, 51)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(100, 21)
        Me.Label10.TabIndex = 11
        Me.Label10.Text = "Cost Centre"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbCostCenterName
        '
        Me.cmbCostCenterName.FormattingEnabled = True
        Me.cmbCostCenterName.Location = New System.Drawing.Point(110, 51)
        Me.cmbCostCenterName.Name = "cmbCostCenterName"
        Me.cmbCostCenterName.Size = New System.Drawing.Size(222, 21)
        Me.cmbCostCenterName.TabIndex = 12
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(341, 4)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(100, 21)
        Me.Label11.TabIndex = 4
        Me.Label11.Text = "Pending Type"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkPurchaseView
        '
        Me.chkPurchaseView.AutoSize = True
        Me.chkPurchaseView.Location = New System.Drawing.Point(462, 8)
        Me.chkPurchaseView.Name = "chkPurchaseView"
        Me.chkPurchaseView.Size = New System.Drawing.Size(78, 17)
        Me.chkPurchaseView.TabIndex = 5
        Me.chkPurchaseView.Text = "Purchase"
        Me.chkPurchaseView.UseVisualStyleBackColor = True
        '
        'chkSalesReturnView
        '
        Me.chkSalesReturnView.AutoSize = True
        Me.chkSalesReturnView.Location = New System.Drawing.Point(555, 8)
        Me.chkSalesReturnView.Name = "chkSalesReturnView"
        Me.chkSalesReturnView.Size = New System.Drawing.Size(99, 17)
        Me.chkSalesReturnView.TabIndex = 6
        Me.chkSalesReturnView.Text = "Sales Return"
        Me.chkSalesReturnView.UseVisualStyleBackColor = True
        '
        'chkMiscIssueView
        '
        Me.chkMiscIssueView.AutoSize = True
        Me.chkMiscIssueView.Location = New System.Drawing.Point(462, 31)
        Me.chkMiscIssueView.Name = "chkMiscIssueView"
        Me.chkMiscIssueView.Size = New System.Drawing.Size(85, 17)
        Me.chkMiscIssueView.TabIndex = 7
        Me.chkMiscIssueView.Text = "Misc Issue"
        Me.chkMiscIssueView.UseVisualStyleBackColor = True
        '
        'chkPartysaleView
        '
        Me.chkPartysaleView.AutoSize = True
        Me.chkPartysaleView.Location = New System.Drawing.Point(555, 31)
        Me.chkPartysaleView.Name = "chkPartysaleView"
        Me.chkPartysaleView.Size = New System.Drawing.Size(88, 17)
        Me.chkPartysaleView.TabIndex = 8
        Me.chkPartysaleView.Text = "Partly Sale"
        Me.chkPartysaleView.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.tabMain)
        Me.Panel1.Controls.Add(Me.GroupBox2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1009, 642)
        Me.Panel1.TabIndex = 9
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.btnGenerate)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.GroupBox2.Location = New System.Drawing.Point(0, 591)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(1009, 51)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(238, 23)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(149, 13)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "P-Partly Weight  Transfer"
        '
        'btnGenerate
        '
        Me.btnGenerate.Location = New System.Drawing.Point(36, 17)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(85, 24)
        Me.btnGenerate.TabIndex = 0
        Me.btnGenerate.Text = "&Generate"
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem, Me.OpenToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(123, 70)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        '
        'btnExcel
        '
        Me.btnExcel.Location = New System.Drawing.Point(487, 95)
        Me.btnExcel.Name = "btnExcel"
        Me.btnExcel.Size = New System.Drawing.Size(100, 30)
        Me.btnExcel.TabIndex = 27
        Me.btnExcel.Text = "Excel"
        Me.btnExcel.UseVisualStyleBackColor = True
        '
        'frmPendingTransfer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1009, 642)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmPendingTransfer"
        Me.Text = "Transfer"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.tabMain.ResumeLayout(False)
        Me.page1.ResumeLayout(False)
        Me.grpGrid.ResumeLayout(False)
        Me.grpGrid.PerformLayout()
        CType(Me.gridViewtotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.grpPurchase.ResumeLayout(False)
        Me.grpPurchase.PerformLayout()
        Me.page2.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        CType(Me.dGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents page1 As System.Windows.Forms.TabPage
    Friend WithEvents page2 As System.Windows.Forms.TabPage
    Friend WithEvents grpGrid As System.Windows.Forms.GroupBox
    Friend WithEvents txtGrid As System.Windows.Forms.TextBox
    Friend WithEvents gridViewtotal As System.Windows.Forms.DataGridView
    Friend WithEvents cmbTransferTo As System.Windows.Forms.ComboBox
    Friend WithEvents chkAll As System.Windows.Forms.CheckBox
    Friend WithEvents gridView_OWN As System.Windows.Forms.DataGridView
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cmbOpenMetal As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbCostCentre_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents chkPurchase As System.Windows.Forms.CheckBox
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents chkSalesReturn As System.Windows.Forms.CheckBox
    Friend WithEvents btnLoad As System.Windows.Forms.Button
    Friend WithEvents chkMiscIssue As System.Windows.Forms.CheckBox
    Friend WithEvents chkPartlySale As System.Windows.Forms.CheckBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents dtpToView As BrighttechPack.DatePicker
    Friend WithEvents dtpFromView As BrighttechPack.DatePicker
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents cmbOpenMetalView As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents cmbCostCenterName As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents chkPurchaseView As System.Windows.Forms.CheckBox
    Friend WithEvents chkSalesReturnView As System.Windows.Forms.CheckBox
    Friend WithEvents chkMiscIssueView As System.Windows.Forms.CheckBox
    Friend WithEvents chkPartysaleView As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnGenerate As System.Windows.Forms.Button
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents dGridView As System.Windows.Forms.DataGridView
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents CmbCategory As System.Windows.Forms.ComboBox
    Friend WithEvents chkPosting As System.Windows.Forms.CheckBox
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents grpPurchase As GroupBox
    Friend WithEvents chkPurchaseOwn As CheckBox
    Friend WithEvents chkPurchaseExchange As CheckBox
    Friend WithEvents chkToType As CheckBox
    Friend WithEvents Label13 As Label
    Friend WithEvents cmbCompany_MAN As ComboBox
    Friend WithEvents Label14 As Label
    Friend WithEvents cmbOpenCompanyView As ComboBox
    Friend WithEvents cmbEntryType_MAN As ComboBox
    Friend WithEvents CmbAcname_MAN As ComboBox
    Friend WithEvents lblAcname As Label
    Friend WithEvents btnExcel As Button
End Class
