<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmPendingTrsCorp
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
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
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.chkSalesReturn = New System.Windows.Forms.CheckBox()
        Me.chkPartlySale = New System.Windows.Forms.CheckBox()
        Me.txtTranNo_MAN = New System.Windows.Forms.TextBox()
        Me.chkMiscIssue = New System.Windows.Forms.CheckBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.chkPurchase = New System.Windows.Forms.CheckBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.CmbCategory = New System.Windows.Forms.ComboBox()
        Me.btnOpen = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbOpenMetal = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.page2 = New System.Windows.Forms.TabPage()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.dGridView = New System.Windows.Forms.DataGridView()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
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
        Me.lblHelp = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tabMain.SuspendLayout()
        Me.page1.SuspendLayout()
        Me.grpGrid.SuspendLayout()
        CType(Me.gridViewtotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
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
        Me.tabMain.Size = New System.Drawing.Size(1021, 591)
        Me.tabMain.TabIndex = 0
        '
        'page1
        '
        Me.page1.Controls.Add(Me.grpGrid)
        Me.page1.Controls.Add(Me.Panel2)
        Me.page1.Location = New System.Drawing.Point(4, 22)
        Me.page1.Name = "page1"
        Me.page1.Padding = New System.Windows.Forms.Padding(3)
        Me.page1.Size = New System.Drawing.Size(1013, 565)
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
        Me.grpGrid.Controls.Add(Me.gridView)
        Me.grpGrid.Location = New System.Drawing.Point(4, 127)
        Me.grpGrid.Name = "grpGrid"
        Me.grpGrid.Size = New System.Drawing.Size(1010, 434)
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
        Me.gridViewtotal.Location = New System.Drawing.Point(3, 320)
        Me.gridViewtotal.MultiSelect = False
        Me.gridViewtotal.Name = "gridViewtotal"
        Me.gridViewtotal.RowHeadersVisible = False
        Me.gridViewtotal.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridViewtotal.Size = New System.Drawing.Size(1004, 111)
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
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.gridView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridView.Location = New System.Drawing.Point(3, 17)
        Me.gridView.MultiSelect = False
        Me.gridView.Name = "gridView"
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.White
        Me.gridView.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.gridView.Size = New System.Drawing.Size(1004, 298)
        Me.gridView.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Controls.Add(Me.lblHelp)
        Me.Panel2.Controls.Add(Me.chkSalesReturn)
        Me.Panel2.Controls.Add(Me.chkPartlySale)
        Me.Panel2.Controls.Add(Me.txtTranNo_MAN)
        Me.Panel2.Controls.Add(Me.chkMiscIssue)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.chkPurchase)
        Me.Panel2.Controls.Add(Me.Label12)
        Me.Panel2.Controls.Add(Me.CmbCategory)
        Me.Panel2.Controls.Add(Me.btnOpen)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.cmbOpenMetal)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.btnExit)
        Me.Panel2.Controls.Add(Me.btnNew)
        Me.Panel2.Controls.Add(Me.btnLoad)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(3, 3)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1007, 120)
        Me.Panel2.TabIndex = 0
        '
        'chkSalesReturn
        '
        Me.chkSalesReturn.AutoSize = True
        Me.chkSalesReturn.Location = New System.Drawing.Point(498, 13)
        Me.chkSalesReturn.Name = "chkSalesReturn"
        Me.chkSalesReturn.Size = New System.Drawing.Size(99, 17)
        Me.chkSalesReturn.TabIndex = 8
        Me.chkSalesReturn.Text = "Sales Return"
        Me.chkSalesReturn.UseVisualStyleBackColor = True
        '
        'chkPartlySale
        '
        Me.chkPartlySale.AutoSize = True
        Me.chkPartlySale.Location = New System.Drawing.Point(498, 36)
        Me.chkPartlySale.Name = "chkPartlySale"
        Me.chkPartlySale.Size = New System.Drawing.Size(88, 17)
        Me.chkPartlySale.TabIndex = 10
        Me.chkPartlySale.Text = "Partly Sale"
        Me.chkPartlySale.UseVisualStyleBackColor = True
        '
        'txtTranNo_MAN
        '
        Me.txtTranNo_MAN.Location = New System.Drawing.Point(91, 4)
        Me.txtTranNo_MAN.Name = "txtTranNo_MAN"
        Me.txtTranNo_MAN.Size = New System.Drawing.Size(100, 21)
        Me.txtTranNo_MAN.TabIndex = 1
        Me.txtTranNo_MAN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'chkMiscIssue
        '
        Me.chkMiscIssue.AutoSize = True
        Me.chkMiscIssue.Location = New System.Drawing.Point(405, 36)
        Me.chkMiscIssue.Name = "chkMiscIssue"
        Me.chkMiscIssue.Size = New System.Drawing.Size(85, 17)
        Me.chkMiscIssue.TabIndex = 9
        Me.chkMiscIssue.Text = "Misc Issue"
        Me.chkMiscIssue.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(11, 8)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(73, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Transfer No"
        '
        'chkPurchase
        '
        Me.chkPurchase.AutoSize = True
        Me.chkPurchase.Location = New System.Drawing.Point(405, 13)
        Me.chkPurchase.Name = "chkPurchase"
        Me.chkPurchase.Size = New System.Drawing.Size(78, 17)
        Me.chkPurchase.TabIndex = 7
        Me.chkPurchase.Text = "Purchase"
        Me.chkPurchase.UseVisualStyleBackColor = True
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(12, 58)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(60, 13)
        Me.Label12.TabIndex = 4
        Me.Label12.Text = "Category"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CmbCategory
        '
        Me.CmbCategory.FormattingEnabled = True
        Me.CmbCategory.Location = New System.Drawing.Point(91, 54)
        Me.CmbCategory.Name = "CmbCategory"
        Me.CmbCategory.Size = New System.Drawing.Size(222, 21)
        Me.CmbCategory.TabIndex = 5
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(289, 78)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 13
        Me.btnOpen.Text = "Open[F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 32)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(74, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Metal Name"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbOpenMetal
        '
        Me.cmbOpenMetal.FormattingEnabled = True
        Me.cmbOpenMetal.Location = New System.Drawing.Point(91, 29)
        Me.cmbOpenMetal.Name = "cmbOpenMetal"
        Me.cmbOpenMetal.Size = New System.Drawing.Size(222, 21)
        Me.cmbOpenMetal.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(320, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(83, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Pending Type"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(389, 78)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 15
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(189, 78)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 12
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnLoad
        '
        Me.btnLoad.Location = New System.Drawing.Point(89, 78)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(100, 30)
        Me.btnLoad.TabIndex = 11
        Me.btnLoad.Text = "&Load"
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'page2
        '
        Me.page2.Controls.Add(Me.Panel4)
        Me.page2.Controls.Add(Me.Panel3)
        Me.page2.Location = New System.Drawing.Point(4, 22)
        Me.page2.Name = "page2"
        Me.page2.Padding = New System.Windows.Forms.Padding(3)
        Me.page2.Size = New System.Drawing.Size(1013, 565)
        Me.page2.TabIndex = 1
        Me.page2.Text = "View"
        Me.page2.UseVisualStyleBackColor = True
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.dGridView)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel4.Location = New System.Drawing.Point(3, 156)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(1007, 422)
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
        Me.dGridView.Size = New System.Drawing.Size(1007, 422)
        Me.dGridView.TabIndex = 1
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.lblTitle)
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
        Me.Panel3.Size = New System.Drawing.Size(1007, 153)
        Me.Panel3.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 130)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1007, 23)
        Me.lblTitle.TabIndex = 16
        Me.lblTitle.Text = "TITLE"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(429, 95)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 15
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(323, 95)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 14
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(217, 95)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 13
        Me.btnBack.Text = "Back"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(108, 95)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 12
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
        Me.dtpToView.TabIndex = 2
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
        Me.Label7.Location = New System.Drawing.Point(3, 11)
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
        Me.Label9.Location = New System.Drawing.Point(4, 62)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(85, 21)
        Me.Label9.TabIndex = 10
        Me.Label9.Text = "Metal"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbOpenMetalView
        '
        Me.cmbOpenMetalView.FormattingEnabled = True
        Me.cmbOpenMetalView.Location = New System.Drawing.Point(110, 66)
        Me.cmbOpenMetalView.Name = "cmbOpenMetalView"
        Me.cmbOpenMetalView.Size = New System.Drawing.Size(222, 21)
        Me.cmbOpenMetalView.TabIndex = 11
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(3, 35)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(100, 21)
        Me.Label10.TabIndex = 8
        Me.Label10.Text = "Cost Centre"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbCostCenterName
        '
        Me.cmbCostCenterName.FormattingEnabled = True
        Me.cmbCostCenterName.Location = New System.Drawing.Point(110, 35)
        Me.cmbCostCenterName.Name = "cmbCostCenterName"
        Me.cmbCostCenterName.Size = New System.Drawing.Size(222, 21)
        Me.cmbCostCenterName.TabIndex = 9
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(341, 4)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(100, 21)
        Me.Label11.TabIndex = 3
        Me.Label11.Text = "Pending Type"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkPurchaseView
        '
        Me.chkPurchaseView.AutoSize = True
        Me.chkPurchaseView.Location = New System.Drawing.Point(462, 8)
        Me.chkPurchaseView.Name = "chkPurchaseView"
        Me.chkPurchaseView.Size = New System.Drawing.Size(78, 17)
        Me.chkPurchaseView.TabIndex = 4
        Me.chkPurchaseView.Text = "Purchase"
        Me.chkPurchaseView.UseVisualStyleBackColor = True
        '
        'chkSalesReturnView
        '
        Me.chkSalesReturnView.AutoSize = True
        Me.chkSalesReturnView.Location = New System.Drawing.Point(555, 8)
        Me.chkSalesReturnView.Name = "chkSalesReturnView"
        Me.chkSalesReturnView.Size = New System.Drawing.Size(99, 17)
        Me.chkSalesReturnView.TabIndex = 5
        Me.chkSalesReturnView.Text = "Sales Return"
        Me.chkSalesReturnView.UseVisualStyleBackColor = True
        '
        'chkMiscIssueView
        '
        Me.chkMiscIssueView.AutoSize = True
        Me.chkMiscIssueView.Enabled = False
        Me.chkMiscIssueView.Location = New System.Drawing.Point(462, 31)
        Me.chkMiscIssueView.Name = "chkMiscIssueView"
        Me.chkMiscIssueView.Size = New System.Drawing.Size(85, 17)
        Me.chkMiscIssueView.TabIndex = 6
        Me.chkMiscIssueView.Text = "Misc Issue"
        Me.chkMiscIssueView.UseVisualStyleBackColor = True
        '
        'chkPartysaleView
        '
        Me.chkPartysaleView.AutoSize = True
        Me.chkPartysaleView.Location = New System.Drawing.Point(555, 31)
        Me.chkPartysaleView.Name = "chkPartysaleView"
        Me.chkPartysaleView.Size = New System.Drawing.Size(88, 17)
        Me.chkPartysaleView.TabIndex = 7
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
        Me.Panel1.Size = New System.Drawing.Size(1021, 642)
        Me.Panel1.TabIndex = 9
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.btnGenerate)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.GroupBox2.Location = New System.Drawing.Point(0, 591)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(1021, 51)
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
        Me.btnGenerate.Location = New System.Drawing.Point(36, 15)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(100, 30)
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
        'lblHelp
        '
        Me.lblHelp.AutoSize = True
        Me.lblHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHelp.ForeColor = System.Drawing.Color.Red
        Me.lblHelp.Location = New System.Drawing.Point(839, 14)
        Me.lblHelp.Name = "lblHelp"
        Me.lblHelp.Size = New System.Drawing.Size(117, 13)
        Me.lblHelp.TabIndex = 23
        Me.lblHelp.Text = "Press X for Excel"
        Me.lblHelp.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(839, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(113, 13)
        Me.Label1.TabIndex = 24
        Me.Label1.Text = "Press P for Print"
        Me.Label1.Visible = False
        '
        'frmPendingTrsCorp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1021, 642)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmPendingTrsCorp"
        Me.Text = "Transfer"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.tabMain.ResumeLayout(False)
        Me.page1.ResumeLayout(False)
        Me.grpGrid.ResumeLayout(False)
        Me.grpGrid.PerformLayout()
        CType(Me.gridViewtotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
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
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cmbOpenMetal As System.Windows.Forms.ComboBox
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
    Friend WithEvents txtTranNo_MAN As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents Label1 As Label
    Friend WithEvents lblHelp As Label
End Class
