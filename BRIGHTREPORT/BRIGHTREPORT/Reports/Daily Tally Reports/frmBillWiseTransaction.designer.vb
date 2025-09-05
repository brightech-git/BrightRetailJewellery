<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmBillWiseTransaction
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
        Me.lblAsOn = New System.Windows.Forms.Label()
        Me.lblCostCentre = New System.Windows.Forms.Label()
        Me.lblNodeId = New System.Windows.Forms.Label()
        Me.txtNodeId = New System.Windows.Forms.TextBox()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.chkItemDetail = New System.Windows.Forms.CheckBox()
        Me.grbcontrols = New System.Windows.Forms.GroupBox()
        Me.chkSpecificSummary = New System.Windows.Forms.CheckBox()
        Me.chkWithAppIssRec = New System.Windows.Forms.CheckBox()
        Me.chkTagType = New System.Windows.Forms.CheckBox()
        Me.cmbCashCounter = New System.Windows.Forms.ComboBox()
        Me.cmbMetal = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chkWithSubItem = New System.Windows.Forms.CheckBox()
        Me.ChkWithCatName = New System.Windows.Forms.CheckBox()
        Me.ChkWithRunno = New System.Windows.Forms.CheckBox()
        Me.ChkWithHSN = New System.Windows.Forms.CheckBox()
        Me.ChkWithTax = New System.Windows.Forms.CheckBox()
        Me.ChkSummaryView = New System.Windows.Forms.CheckBox()
        Me.chkCanelBill = New System.Windows.Forms.CheckBox()
        Me.ChkOrdAdvWt = New System.Windows.Forms.CheckBox()
        Me.ChkOrderRecipt = New System.Windows.Forms.CheckBox()
        Me.ChkBillTotal = New System.Windows.Forms.CheckBox()
        Me.rbtboth = New System.Windows.Forms.RadioButton()
        Me.rbtnetwght = New System.Windows.Forms.RadioButton()
        Me.rbtGrswght = New System.Windows.Forms.RadioButton()
        Me.chkCompanySelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstCompany = New System.Windows.Forms.CheckedListBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpDate = New BrighttechPack.DatePicker(Me.components)
        Me.cmbCostCentre = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.gridHeader = New System.Windows.Forms.DataGridView()
        Me.gridBillWiseTransaction = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ReziseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlGrid = New System.Windows.Forms.Panel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.grbcontrols.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        CType(Me.gridHeader, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridBillWiseTransaction, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.pnlGrid.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblAsOn
        '
        Me.lblAsOn.AutoSize = True
        Me.lblAsOn.Location = New System.Drawing.Point(7, 21)
        Me.lblAsOn.Name = "lblAsOn"
        Me.lblAsOn.Size = New System.Drawing.Size(34, 13)
        Me.lblAsOn.TabIndex = 0
        Me.lblAsOn.Text = "Date"
        '
        'lblCostCentre
        '
        Me.lblCostCentre.AutoSize = True
        Me.lblCostCentre.Location = New System.Drawing.Point(412, 21)
        Me.lblCostCentre.Name = "lblCostCentre"
        Me.lblCostCentre.Size = New System.Drawing.Size(76, 13)
        Me.lblCostCentre.TabIndex = 6
        Me.lblCostCentre.Text = "Cost Centre"
        '
        'lblNodeId
        '
        Me.lblNodeId.AutoSize = True
        Me.lblNodeId.Location = New System.Drawing.Point(251, 21)
        Me.lblNodeId.Name = "lblNodeId"
        Me.lblNodeId.Size = New System.Drawing.Size(54, 13)
        Me.lblNodeId.TabIndex = 4
        Me.lblNodeId.Text = "Node ID"
        '
        'txtNodeId
        '
        Me.txtNodeId.Location = New System.Drawing.Point(311, 17)
        Me.txtNodeId.Name = "txtNodeId"
        Me.txtNodeId.Size = New System.Drawing.Size(98, 21)
        Me.txtNodeId.TabIndex = 5
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(373, 104)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 31
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(479, 104)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 32
        Me.btnNew.Text = "&New[F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(585, 104)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 33
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(691, 104)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 34
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(797, 104)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 35
        Me.btnExit.Text = "&Exit[F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'chkItemDetail
        '
        Me.chkItemDetail.AutoSize = True
        Me.chkItemDetail.Location = New System.Drawing.Point(1209, 19)
        Me.chkItemDetail.Name = "chkItemDetail"
        Me.chkItemDetail.Size = New System.Drawing.Size(90, 17)
        Me.chkItemDetail.TabIndex = 12
        Me.chkItemDetail.Text = "Item Detail"
        Me.chkItemDetail.UseVisualStyleBackColor = True
        '
        'grbcontrols
        '
        Me.grbcontrols.Controls.Add(Me.chkSpecificSummary)
        Me.grbcontrols.Controls.Add(Me.chkWithAppIssRec)
        Me.grbcontrols.Controls.Add(Me.chkTagType)
        Me.grbcontrols.Controls.Add(Me.cmbCashCounter)
        Me.grbcontrols.Controls.Add(Me.cmbMetal)
        Me.grbcontrols.Controls.Add(Me.Label3)
        Me.grbcontrols.Controls.Add(Me.Label2)
        Me.grbcontrols.Controls.Add(Me.chkWithSubItem)
        Me.grbcontrols.Controls.Add(Me.ChkWithCatName)
        Me.grbcontrols.Controls.Add(Me.ChkWithRunno)
        Me.grbcontrols.Controls.Add(Me.ChkWithHSN)
        Me.grbcontrols.Controls.Add(Me.ChkWithTax)
        Me.grbcontrols.Controls.Add(Me.ChkSummaryView)
        Me.grbcontrols.Controls.Add(Me.chkCanelBill)
        Me.grbcontrols.Controls.Add(Me.ChkOrdAdvWt)
        Me.grbcontrols.Controls.Add(Me.ChkOrderRecipt)
        Me.grbcontrols.Controls.Add(Me.ChkBillTotal)
        Me.grbcontrols.Controls.Add(Me.rbtboth)
        Me.grbcontrols.Controls.Add(Me.rbtnetwght)
        Me.grbcontrols.Controls.Add(Me.rbtGrswght)
        Me.grbcontrols.Controls.Add(Me.chkCompanySelectAll)
        Me.grbcontrols.Controls.Add(Me.chkLstCompany)
        Me.grbcontrols.Controls.Add(Me.dtpTo)
        Me.grbcontrols.Controls.Add(Me.dtpDate)
        Me.grbcontrols.Controls.Add(Me.txtNodeId)
        Me.grbcontrols.Controls.Add(Me.btnExit)
        Me.grbcontrols.Controls.Add(Me.btnPrint)
        Me.grbcontrols.Controls.Add(Me.btnExport)
        Me.grbcontrols.Controls.Add(Me.btnNew)
        Me.grbcontrols.Controls.Add(Me.chkItemDetail)
        Me.grbcontrols.Controls.Add(Me.btnView_Search)
        Me.grbcontrols.Controls.Add(Me.cmbCostCentre)
        Me.grbcontrols.Controls.Add(Me.Label1)
        Me.grbcontrols.Controls.Add(Me.lblAsOn)
        Me.grbcontrols.Controls.Add(Me.lblCostCentre)
        Me.grbcontrols.Controls.Add(Me.lblNodeId)
        Me.grbcontrols.Dock = System.Windows.Forms.DockStyle.Top
        Me.grbcontrols.Location = New System.Drawing.Point(0, 0)
        Me.grbcontrols.Name = "grbcontrols"
        Me.grbcontrols.Size = New System.Drawing.Size(1331, 144)
        Me.grbcontrols.TabIndex = 0
        Me.grbcontrols.TabStop = False
        '
        'chkSpecificSummary
        '
        Me.chkSpecificSummary.AutoSize = True
        Me.chkSpecificSummary.Location = New System.Drawing.Point(1209, 78)
        Me.chkSpecificSummary.Name = "chkSpecificSummary"
        Me.chkSpecificSummary.Size = New System.Drawing.Size(101, 17)
        Me.chkSpecificSummary.TabIndex = 30
        Me.chkSpecificSummary.Text = "Specific View"
        Me.chkSpecificSummary.UseVisualStyleBackColor = True
        '
        'chkWithAppIssRec
        '
        Me.chkWithAppIssRec.AutoSize = True
        Me.chkWithAppIssRec.Location = New System.Drawing.Point(1090, 45)
        Me.chkWithAppIssRec.Name = "chkWithAppIssRec"
        Me.chkWithAppIssRec.Size = New System.Drawing.Size(106, 17)
        Me.chkWithAppIssRec.TabIndex = 20
        Me.chkWithAppIssRec.Text = "With Approval"
        Me.chkWithAppIssRec.UseVisualStyleBackColor = True
        '
        'chkTagType
        '
        Me.chkTagType.AutoSize = True
        Me.chkTagType.Location = New System.Drawing.Point(1090, 78)
        Me.chkTagType.Name = "chkTagType"
        Me.chkTagType.Size = New System.Drawing.Size(102, 17)
        Me.chkTagType.TabIndex = 29
        Me.chkTagType.Text = "With TagType"
        Me.chkTagType.UseVisualStyleBackColor = True
        '
        'cmbCashCounter
        '
        Me.cmbCashCounter.FormattingEnabled = True
        Me.cmbCashCounter.Location = New System.Drawing.Point(1008, 18)
        Me.cmbCashCounter.Name = "cmbCashCounter"
        Me.cmbCashCounter.Size = New System.Drawing.Size(195, 21)
        Me.cmbCashCounter.TabIndex = 11
        '
        'cmbMetal
        '
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(775, 19)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(135, 21)
        Me.cmbMetal.TabIndex = 9
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(916, 23)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(86, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Cash Counter"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(732, 21)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(37, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Metal"
        '
        'chkWithSubItem
        '
        Me.chkWithSubItem.AutoSize = True
        Me.chkWithSubItem.Location = New System.Drawing.Point(1209, 42)
        Me.chkWithSubItem.Name = "chkWithSubItem"
        Me.chkWithSubItem.Size = New System.Drawing.Size(104, 17)
        Me.chkWithSubItem.TabIndex = 21
        Me.chkWithSubItem.Text = "With SubItem"
        Me.chkWithSubItem.UseVisualStyleBackColor = True
        '
        'ChkWithCatName
        '
        Me.ChkWithCatName.AutoSize = True
        Me.ChkWithCatName.Location = New System.Drawing.Point(939, 78)
        Me.ChkWithCatName.Name = "ChkWithCatName"
        Me.ChkWithCatName.Size = New System.Drawing.Size(145, 17)
        Me.ChkWithCatName.TabIndex = 28
        Me.ChkWithCatName.Text = "With CatName Purity"
        Me.ChkWithCatName.UseVisualStyleBackColor = True
        '
        'ChkWithRunno
        '
        Me.ChkWithRunno.AutoSize = True
        Me.ChkWithRunno.Location = New System.Drawing.Point(982, 46)
        Me.ChkWithRunno.Name = "ChkWithRunno"
        Me.ChkWithRunno.Size = New System.Drawing.Size(91, 17)
        Me.ChkWithRunno.TabIndex = 19
        Me.ChkWithRunno.Text = "With Runno"
        Me.ChkWithRunno.UseVisualStyleBackColor = True
        Me.ChkWithRunno.Visible = False
        '
        'ChkWithHSN
        '
        Me.ChkWithHSN.AutoSize = True
        Me.ChkWithHSN.Location = New System.Drawing.Point(854, 78)
        Me.ChkWithHSN.Name = "ChkWithHSN"
        Me.ChkWithHSN.Size = New System.Drawing.Size(79, 17)
        Me.ChkWithHSN.TabIndex = 27
        Me.ChkWithHSN.Text = "With HSN"
        Me.ChkWithHSN.UseVisualStyleBackColor = True
        Me.ChkWithHSN.Visible = False
        '
        'ChkWithTax
        '
        Me.ChkWithTax.AutoSize = True
        Me.ChkWithTax.Location = New System.Drawing.Point(854, 46)
        Me.ChkWithTax.Name = "ChkWithTax"
        Me.ChkWithTax.Size = New System.Drawing.Size(122, 17)
        Me.ChkWithTax.TabIndex = 18
        Me.ChkWithTax.Text = "With GST Details"
        Me.ChkWithTax.UseVisualStyleBackColor = True
        Me.ChkWithTax.Visible = False
        '
        'ChkSummaryView
        '
        Me.ChkSummaryView.AutoSize = True
        Me.ChkSummaryView.Location = New System.Drawing.Point(734, 78)
        Me.ChkSummaryView.Name = "ChkSummaryView"
        Me.ChkSummaryView.Size = New System.Drawing.Size(113, 17)
        Me.ChkSummaryView.TabIndex = 26
        Me.ChkSummaryView.Text = "Summary View"
        Me.ChkSummaryView.UseVisualStyleBackColor = True
        '
        'chkCanelBill
        '
        Me.chkCanelBill.AutoSize = True
        Me.chkCanelBill.Location = New System.Drawing.Point(734, 46)
        Me.chkCanelBill.Name = "chkCanelBill"
        Me.chkCanelBill.Size = New System.Drawing.Size(115, 17)
        Me.chkCanelBill.TabIndex = 17
        Me.chkCanelBill.Text = "With Cancel Bill"
        Me.chkCanelBill.UseVisualStyleBackColor = True
        '
        'ChkOrdAdvWt
        '
        Me.ChkOrdAdvWt.AutoSize = True
        Me.ChkOrdAdvWt.Location = New System.Drawing.Point(599, 78)
        Me.ChkOrdAdvWt.Name = "ChkOrdAdvWt"
        Me.ChkOrdAdvWt.Size = New System.Drawing.Size(129, 17)
        Me.ChkOrdAdvWt.TabIndex = 25
        Me.ChkOrdAdvWt.Text = "With Order AdvWt"
        Me.ChkOrdAdvWt.UseVisualStyleBackColor = True
        '
        'ChkOrderRecipt
        '
        Me.ChkOrderRecipt.AutoSize = True
        Me.ChkOrderRecipt.Location = New System.Drawing.Point(599, 46)
        Me.ChkOrderRecipt.Name = "ChkOrderRecipt"
        Me.ChkOrderRecipt.Size = New System.Drawing.Size(134, 17)
        Me.ChkOrderRecipt.TabIndex = 16
        Me.ChkOrderRecipt.Text = "With Order Receipt"
        Me.ChkOrderRecipt.UseVisualStyleBackColor = True
        '
        'ChkBillTotal
        '
        Me.ChkBillTotal.AutoSize = True
        Me.ChkBillTotal.Location = New System.Drawing.Point(470, 46)
        Me.ChkBillTotal.Name = "ChkBillTotal"
        Me.ChkBillTotal.Size = New System.Drawing.Size(122, 17)
        Me.ChkBillTotal.TabIndex = 15
        Me.ChkBillTotal.Text = "Bill No wise Total"
        Me.ChkBillTotal.UseVisualStyleBackColor = True
        '
        'rbtboth
        '
        Me.rbtboth.AutoSize = True
        Me.rbtboth.Location = New System.Drawing.Point(516, 78)
        Me.rbtboth.Name = "rbtboth"
        Me.rbtboth.Size = New System.Drawing.Size(51, 17)
        Me.rbtboth.TabIndex = 24
        Me.rbtboth.TabStop = True
        Me.rbtboth.Text = "Both"
        Me.rbtboth.UseVisualStyleBackColor = True
        '
        'rbtnetwght
        '
        Me.rbtnetwght.AutoSize = True
        Me.rbtnetwght.Location = New System.Drawing.Point(447, 78)
        Me.rbtnetwght.Name = "rbtnetwght"
        Me.rbtnetwght.Size = New System.Drawing.Size(63, 17)
        Me.rbtnetwght.TabIndex = 23
        Me.rbtnetwght.TabStop = True
        Me.rbtnetwght.Text = "Net Wt"
        Me.rbtnetwght.UseVisualStyleBackColor = True
        '
        'rbtGrswght
        '
        Me.rbtGrswght.AutoSize = True
        Me.rbtGrswght.Location = New System.Drawing.Point(373, 78)
        Me.rbtGrswght.Name = "rbtGrswght"
        Me.rbtGrswght.Size = New System.Drawing.Size(68, 17)
        Me.rbtGrswght.TabIndex = 22
        Me.rbtGrswght.TabStop = True
        Me.rbtGrswght.Text = "Grs  Wt"
        Me.rbtGrswght.UseVisualStyleBackColor = True
        '
        'chkCompanySelectAll
        '
        Me.chkCompanySelectAll.AutoSize = True
        Me.chkCompanySelectAll.Location = New System.Drawing.Point(10, 46)
        Me.chkCompanySelectAll.Name = "chkCompanySelectAll"
        Me.chkCompanySelectAll.Size = New System.Drawing.Size(81, 17)
        Me.chkCompanySelectAll.TabIndex = 13
        Me.chkCompanySelectAll.Text = "Company"
        Me.chkCompanySelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCompany
        '
        Me.chkLstCompany.FormattingEnabled = True
        Me.chkLstCompany.Location = New System.Drawing.Point(7, 66)
        Me.chkLstCompany.Name = "chkLstCompany"
        Me.chkLstCompany.Size = New System.Drawing.Size(351, 68)
        Me.chkLstCompany.TabIndex = 14
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(165, 17)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(86, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpDate
        '
        Me.dtpDate.Location = New System.Drawing.Point(47, 17)
        Me.dtpDate.Mask = "##/##/####"
        Me.dtpDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpDate.Size = New System.Drawing.Size(86, 21)
        Me.dtpDate.TabIndex = 1
        Me.dtpDate.Text = "07/03/9998"
        Me.dtpDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'cmbCostCentre
        '
        Me.cmbCostCentre.FormattingEnabled = True
        Me.cmbCostCentre.Location = New System.Drawing.Point(494, 17)
        Me.cmbCostCentre.Name = "cmbCostCentre"
        Me.cmbCostCentre.Size = New System.Drawing.Size(232, 21)
        Me.cmbCostCentre.TabIndex = 7
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(139, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(20, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "To"
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
        'gridHeader
        '
        Me.gridHeader.AllowUserToAddRows = False
        Me.gridHeader.AllowUserToDeleteRows = False
        Me.gridHeader.AllowUserToResizeColumns = False
        Me.gridHeader.AllowUserToResizeRows = False
        Me.gridHeader.BackgroundColor = System.Drawing.SystemColors.ControlDark
        Me.gridHeader.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridHeader.Location = New System.Drawing.Point(0, 0)
        Me.gridHeader.Name = "gridHeader"
        Me.gridHeader.ReadOnly = True
        Me.gridHeader.RowHeadersVisible = False
        Me.gridHeader.Size = New System.Drawing.Size(1331, 16)
        Me.gridHeader.TabIndex = 0
        '
        'gridBillWiseTransaction
        '
        Me.gridBillWiseTransaction.AllowUserToAddRows = False
        Me.gridBillWiseTransaction.AllowUserToDeleteRows = False
        Me.gridBillWiseTransaction.AllowUserToResizeRows = False
        Me.gridBillWiseTransaction.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridBillWiseTransaction.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridBillWiseTransaction.ContextMenuStrip = Me.ContextMenuStrip2
        Me.gridBillWiseTransaction.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridBillWiseTransaction.Location = New System.Drawing.Point(0, 0)
        Me.gridBillWiseTransaction.MultiSelect = False
        Me.gridBillWiseTransaction.Name = "gridBillWiseTransaction"
        Me.gridBillWiseTransaction.ReadOnly = True
        Me.gridBillWiseTransaction.RowHeadersVisible = False
        Me.gridBillWiseTransaction.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridBillWiseTransaction.Size = New System.Drawing.Size(1331, 446)
        Me.gridBillWiseTransaction.StandardTab = True
        Me.gridBillWiseTransaction.TabIndex = 0
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ReziseToolStripMenuItem})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(107, 26)
        '
        'ReziseToolStripMenuItem
        '
        Me.ReziseToolStripMenuItem.CheckOnClick = True
        Me.ReziseToolStripMenuItem.Name = "ReziseToolStripMenuItem"
        Me.ReziseToolStripMenuItem.Size = New System.Drawing.Size(106, 22)
        Me.ReziseToolStripMenuItem.Text = "Rezise"
        '
        'pnlGrid
        '
        Me.pnlGrid.Controls.Add(Me.Panel3)
        Me.pnlGrid.Controls.Add(Me.Panel2)
        Me.pnlGrid.Controls.Add(Me.Panel1)
        Me.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlGrid.Location = New System.Drawing.Point(0, 144)
        Me.pnlGrid.Name = "pnlGrid"
        Me.pnlGrid.Size = New System.Drawing.Size(1331, 488)
        Me.pnlGrid.TabIndex = 3
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Panel3.Controls.Add(Me.gridBillWiseTransaction)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(0, 42)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1331, 446)
        Me.Panel3.TabIndex = 5
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Panel2.Controls.Add(Me.gridHeader)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 26)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1331, 16)
        Me.Panel2.TabIndex = 4
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Panel1.Controls.Add(Me.lblTitle)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1331, 26)
        Me.Panel1.TabIndex = 3
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1331, 16)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "TITLE"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmBillWiseTransaction
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1331, 632)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlGrid)
        Me.Controls.Add(Me.grbcontrols)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmBillWiseTransaction"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "BillWise Transaction"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grbcontrols.ResumeLayout(False)
        Me.grbcontrols.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        CType(Me.gridHeader, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridBillWiseTransaction, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip2.ResumeLayout(False)
        Me.pnlGrid.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblAsOn As System.Windows.Forms.Label
    Friend WithEvents lblCostCentre As System.Windows.Forms.Label
    Friend WithEvents lblNodeId As System.Windows.Forms.Label
    Friend WithEvents txtNodeId As System.Windows.Forms.TextBox
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents chkItemDetail As System.Windows.Forms.CheckBox
    Friend WithEvents grbcontrols As System.Windows.Forms.GroupBox
    Friend WithEvents cmbCostCentre As System.Windows.Forms.ComboBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gridHeader As System.Windows.Forms.DataGridView
    Friend WithEvents gridBillWiseTransaction As System.Windows.Forms.DataGridView
    Friend WithEvents pnlGrid As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents dtpDate As BrighttechPack.DatePicker
    Friend WithEvents chkCompanySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCompany As System.Windows.Forms.CheckedListBox
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents rbtboth As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnetwght As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGrswght As System.Windows.Forms.RadioButton
    Friend WithEvents ChkBillTotal As System.Windows.Forms.CheckBox
    Friend WithEvents ChkOrderRecipt As System.Windows.Forms.CheckBox
    Friend WithEvents ChkOrdAdvWt As System.Windows.Forms.CheckBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents chkCanelBill As System.Windows.Forms.CheckBox
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ReziseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChkSummaryView As System.Windows.Forms.CheckBox
    Friend WithEvents ChkWithHSN As System.Windows.Forms.CheckBox
    Friend WithEvents ChkWithTax As System.Windows.Forms.CheckBox
    Friend WithEvents ChkWithRunno As System.Windows.Forms.CheckBox
    Friend WithEvents ChkWithCatName As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithSubItem As System.Windows.Forms.CheckBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbCashCounter As System.Windows.Forms.ComboBox
    Friend WithEvents cmbMetal As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents chkTagType As CheckBox
    Friend WithEvents chkWithAppIssRec As CheckBox
    Friend WithEvents chkSpecificSummary As CheckBox
End Class
