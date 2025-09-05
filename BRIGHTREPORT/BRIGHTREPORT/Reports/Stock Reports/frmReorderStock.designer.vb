<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReorderStock
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
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.grpControls = New System.Windows.Forms.GroupBox()
        Me.chkWithCaption = New System.Windows.Forms.CheckBox()
        Me.lblWR = New System.Windows.Forms.Label()
        Me.txtRangeTo = New System.Windows.Forms.TextBox()
        Me.txtRangeFrom = New System.Windows.Forms.TextBox()
        Me.chkCmbCompany = New BrighttechPack.CheckedComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.chkCmbSubItem = New BrighttechPack.CheckedComboBox()
        Me.ChkOrder = New System.Windows.Forms.CheckBox()
        Me.cmbCostCentre = New BrighttechPack.CheckedComboBox()
        Me.cmbCounter = New BrighttechPack.CheckedComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbMetal = New BrighttechPack.CheckedComboBox()
        Me.chkCmbItem = New BrighttechPack.CheckedComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.rdbWeight = New System.Windows.Forms.RadioButton()
        Me.rdbRate = New System.Windows.Forms.RadioButton()
        Me.chkDetailedOthers = New System.Windows.Forms.CheckBox()
        Me.chkSize = New System.Windows.Forms.CheckBox()
        Me.cmbSize = New System.Windows.Forms.ComboBox()
        Me.dtpAsOnDate = New BrighttechPack.DatePicker(Me.components)
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.rbtBoth = New System.Windows.Forms.RadioButton()
        Me.rbtShort = New System.Windows.Forms.RadioButton()
        Me.rbtExcess = New System.Windows.Forms.RadioButton()
        Me.rbtSales = New System.Windows.Forms.RadioButton()
        Me.rbtStock = New System.Windows.Forms.RadioButton()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbSubItemName = New System.Windows.Forms.ComboBox()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.pnlView = New System.Windows.Forms.Panel()
        Me.pnlgrid = New System.Windows.Forms.Panel()
        Me.pnlTagView = New System.Windows.Forms.Panel()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.gridviewTag = New System.Windows.Forms.DataGridView()
        Me.pnlFooter = New System.Windows.Forms.Panel()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.pnlMain = New System.Windows.Forms.Panel()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabGen = New System.Windows.Forms.TabPage()
        Me.tabView = New System.Windows.Forms.TabPage()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.grpControls.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlView.SuspendLayout()
        Me.pnlgrid.SuspendLayout()
        Me.pnlTagView.SuspendLayout()
        CType(Me.gridviewTag, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlFooter.SuspendLayout()
        Me.pnlMain.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGen.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(51, 407)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 26
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'grpControls
        '
        Me.grpControls.Controls.Add(Me.chkWithCaption)
        Me.grpControls.Controls.Add(Me.lblWR)
        Me.grpControls.Controls.Add(Me.txtRangeTo)
        Me.grpControls.Controls.Add(Me.txtRangeFrom)
        Me.grpControls.Controls.Add(Me.chkCmbCompany)
        Me.grpControls.Controls.Add(Me.Label8)
        Me.grpControls.Controls.Add(Me.chkCmbSubItem)
        Me.grpControls.Controls.Add(Me.ChkOrder)
        Me.grpControls.Controls.Add(Me.cmbCostCentre)
        Me.grpControls.Controls.Add(Me.cmbCounter)
        Me.grpControls.Controls.Add(Me.Label6)
        Me.grpControls.Controls.Add(Me.cmbMetal)
        Me.grpControls.Controls.Add(Me.chkCmbItem)
        Me.grpControls.Controls.Add(Me.Label4)
        Me.grpControls.Controls.Add(Me.Panel2)
        Me.grpControls.Controls.Add(Me.chkDetailedOthers)
        Me.grpControls.Controls.Add(Me.chkSize)
        Me.grpControls.Controls.Add(Me.cmbSize)
        Me.grpControls.Controls.Add(Me.dtpAsOnDate)
        Me.grpControls.Controls.Add(Me.btnNew)
        Me.grpControls.Controls.Add(Me.btnExit)
        Me.grpControls.Controls.Add(Me.Panel1)
        Me.grpControls.Controls.Add(Me.rbtSales)
        Me.grpControls.Controls.Add(Me.rbtStock)
        Me.grpControls.Controls.Add(Me.Label5)
        Me.grpControls.Controls.Add(Me.Label3)
        Me.grpControls.Controls.Add(Me.Label2)
        Me.grpControls.Controls.Add(Me.Label1)
        Me.grpControls.Controls.Add(Me.btnView_Search)
        Me.grpControls.Location = New System.Drawing.Point(314, 103)
        Me.grpControls.Name = "grpControls"
        Me.grpControls.Size = New System.Drawing.Size(414, 448)
        Me.grpControls.TabIndex = 0
        Me.grpControls.TabStop = False
        '
        'chkWithCaption
        '
        Me.chkWithCaption.AutoSize = True
        Me.chkWithCaption.Location = New System.Drawing.Point(268, 380)
        Me.chkWithCaption.Name = "chkWithCaption"
        Me.chkWithCaption.Size = New System.Drawing.Size(99, 17)
        Me.chkWithCaption.TabIndex = 25
        Me.chkWithCaption.Text = "With Caption"
        Me.chkWithCaption.UseVisualStyleBackColor = True
        '
        'lblWR
        '
        Me.lblWR.Location = New System.Drawing.Point(231, 352)
        Me.lblWR.Name = "lblWR"
        Me.lblWR.Size = New System.Drawing.Size(21, 21)
        Me.lblWR.TabIndex = 21
        Me.lblWR.Text = "To"
        Me.lblWR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtRangeTo
        '
        Me.txtRangeTo.Location = New System.Drawing.Point(256, 352)
        Me.txtRangeTo.Name = "txtRangeTo"
        Me.txtRangeTo.Size = New System.Drawing.Size(90, 21)
        Me.txtRangeTo.TabIndex = 22
        Me.txtRangeTo.Text = "0"
        Me.txtRangeTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtRangeFrom
        '
        Me.txtRangeFrom.Location = New System.Drawing.Point(135, 352)
        Me.txtRangeFrom.Name = "txtRangeFrom"
        Me.txtRangeFrom.Size = New System.Drawing.Size(90, 21)
        Me.txtRangeFrom.TabIndex = 20
        Me.txtRangeFrom.Text = "0"
        Me.txtRangeFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'chkCmbCompany
        '
        Me.chkCmbCompany.CheckOnClick = True
        Me.chkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCompany.DropDownHeight = 1
        Me.chkCmbCompany.FormattingEnabled = True
        Me.chkCmbCompany.IntegralHeight = False
        Me.chkCmbCompany.Location = New System.Drawing.Point(138, 25)
        Me.chkCmbCompany.Name = "chkCmbCompany"
        Me.chkCmbCompany.Size = New System.Drawing.Size(204, 22)
        Me.chkCmbCompany.TabIndex = 1
        Me.chkCmbCompany.ValueSeparator = ", "
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(20, 29)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(62, 13)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Company"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbSubItem
        '
        Me.chkCmbSubItem.CheckOnClick = True
        Me.chkCmbSubItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbSubItem.DropDownHeight = 1
        Me.chkCmbSubItem.FormattingEnabled = True
        Me.chkCmbSubItem.IntegralHeight = False
        Me.chkCmbSubItem.Location = New System.Drawing.Point(138, 148)
        Me.chkCmbSubItem.Name = "chkCmbSubItem"
        Me.chkCmbSubItem.Size = New System.Drawing.Size(204, 22)
        Me.chkCmbSubItem.TabIndex = 9
        Me.chkCmbSubItem.ValueSeparator = ", "
        '
        'ChkOrder
        '
        Me.ChkOrder.AutoSize = True
        Me.ChkOrder.Checked = True
        Me.ChkOrder.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkOrder.Location = New System.Drawing.Point(173, 380)
        Me.ChkOrder.Name = "ChkOrder"
        Me.ChkOrder.Size = New System.Drawing.Size(88, 17)
        Me.ChkOrder.TabIndex = 24
        Me.ChkOrder.Text = "With Order"
        Me.ChkOrder.UseVisualStyleBackColor = True
        '
        'cmbCostCentre
        '
        Me.cmbCostCentre.CheckOnClick = True
        Me.cmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbCostCentre.DropDownHeight = 1
        Me.cmbCostCentre.FormattingEnabled = True
        Me.cmbCostCentre.IntegralHeight = False
        Me.cmbCostCentre.Location = New System.Drawing.Point(138, 178)
        Me.cmbCostCentre.Name = "cmbCostCentre"
        Me.cmbCostCentre.Size = New System.Drawing.Size(204, 22)
        Me.cmbCostCentre.TabIndex = 11
        Me.cmbCostCentre.ValueSeparator = ", "
        '
        'cmbCounter
        '
        Me.cmbCounter.CheckOnClick = True
        Me.cmbCounter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbCounter.DropDownHeight = 1
        Me.cmbCounter.FormattingEnabled = True
        Me.cmbCounter.IntegralHeight = False
        Me.cmbCounter.Location = New System.Drawing.Point(138, 55)
        Me.cmbCounter.Name = "cmbCounter"
        Me.cmbCounter.Size = New System.Drawing.Size(204, 22)
        Me.cmbCounter.TabIndex = 3
        Me.cmbCounter.ValueSeparator = ", "
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(20, 56)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(58, 21)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Counter"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbMetal
        '
        Me.cmbMetal.CheckOnClick = True
        Me.cmbMetal.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbMetal.DropDownHeight = 1
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.IntegralHeight = False
        Me.cmbMetal.Location = New System.Drawing.Point(138, 88)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(204, 22)
        Me.cmbMetal.TabIndex = 5
        Me.cmbMetal.ValueSeparator = ", "
        '
        'chkCmbItem
        '
        Me.chkCmbItem.CheckOnClick = True
        Me.chkCmbItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbItem.DropDownHeight = 1
        Me.chkCmbItem.FormattingEnabled = True
        Me.chkCmbItem.IntegralHeight = False
        Me.chkCmbItem.Location = New System.Drawing.Point(138, 118)
        Me.chkCmbItem.Name = "chkCmbItem"
        Me.chkCmbItem.Size = New System.Drawing.Size(204, 22)
        Me.chkCmbItem.TabIndex = 7
        Me.chkCmbItem.ValueSeparator = ", "
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(20, 87)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(58, 21)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Metal"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rdbWeight)
        Me.Panel2.Controls.Add(Me.rdbRate)
        Me.Panel2.Location = New System.Drawing.Point(135, 325)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(211, 22)
        Me.Panel2.TabIndex = 19
        '
        'rdbWeight
        '
        Me.rdbWeight.AutoSize = True
        Me.rdbWeight.Checked = True
        Me.rdbWeight.Location = New System.Drawing.Point(3, 3)
        Me.rdbWeight.Name = "rdbWeight"
        Me.rdbWeight.Size = New System.Drawing.Size(103, 17)
        Me.rdbWeight.TabIndex = 1
        Me.rdbWeight.TabStop = True
        Me.rdbWeight.Text = "Weight Range" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.rdbWeight.UseVisualStyleBackColor = True
        '
        'rdbRate
        '
        Me.rdbRate.AutoSize = True
        Me.rdbRate.Location = New System.Drawing.Point(113, 3)
        Me.rdbRate.Name = "rdbRate"
        Me.rdbRate.Size = New System.Drawing.Size(91, 17)
        Me.rdbRate.TabIndex = 0
        Me.rdbRate.Text = "Rate Range"
        Me.rdbRate.UseVisualStyleBackColor = True
        '
        'chkDetailedOthers
        '
        Me.chkDetailedOthers.AutoSize = True
        Me.chkDetailedOthers.Location = New System.Drawing.Point(51, 380)
        Me.chkDetailedOthers.Name = "chkDetailedOthers"
        Me.chkDetailedOthers.Size = New System.Drawing.Size(115, 17)
        Me.chkDetailedOthers.TabIndex = 23
        Me.chkDetailedOthers.Text = "Detailed Others"
        Me.chkDetailedOthers.UseVisualStyleBackColor = True
        '
        'chkSize
        '
        Me.chkSize.AutoSize = True
        Me.chkSize.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkSize.Location = New System.Drawing.Point(20, 215)
        Me.chkSize.Name = "chkSize"
        Me.chkSize.Size = New System.Drawing.Size(50, 17)
        Me.chkSize.TabIndex = 12
        Me.chkSize.Text = "Size"
        Me.chkSize.UseVisualStyleBackColor = True
        '
        'cmbSize
        '
        Me.cmbSize.FormattingEnabled = True
        Me.cmbSize.Location = New System.Drawing.Point(138, 211)
        Me.cmbSize.Name = "cmbSize"
        Me.cmbSize.Size = New System.Drawing.Size(204, 21)
        Me.cmbSize.TabIndex = 13
        '
        'dtpAsOnDate
        '
        Me.dtpAsOnDate.Location = New System.Drawing.Point(138, 241)
        Me.dtpAsOnDate.Mask = "##/##/####"
        Me.dtpAsOnDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpAsOnDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpAsOnDate.Name = "dtpAsOnDate"
        Me.dtpAsOnDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpAsOnDate.Size = New System.Drawing.Size(93, 21)
        Me.dtpAsOnDate.TabIndex = 15
        Me.dtpAsOnDate.Text = "07/03/9998"
        Me.dtpAsOnDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(159, 407)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 27
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(267, 407)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 28
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbtBoth)
        Me.Panel1.Controls.Add(Me.rbtShort)
        Me.Panel1.Controls.Add(Me.rbtExcess)
        Me.Panel1.Location = New System.Drawing.Point(135, 297)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(211, 22)
        Me.Panel1.TabIndex = 18
        '
        'rbtBoth
        '
        Me.rbtBoth.AutoSize = True
        Me.rbtBoth.Checked = True
        Me.rbtBoth.Location = New System.Drawing.Point(3, 3)
        Me.rbtBoth.Name = "rbtBoth"
        Me.rbtBoth.Size = New System.Drawing.Size(51, 17)
        Me.rbtBoth.TabIndex = 0
        Me.rbtBoth.TabStop = True
        Me.rbtBoth.Text = "Both"
        Me.rbtBoth.UseVisualStyleBackColor = True
        '
        'rbtShort
        '
        Me.rbtShort.AutoSize = True
        Me.rbtShort.Location = New System.Drawing.Point(70, 3)
        Me.rbtShort.Name = "rbtShort"
        Me.rbtShort.Size = New System.Drawing.Size(56, 17)
        Me.rbtShort.TabIndex = 1
        Me.rbtShort.Text = "Short"
        Me.rbtShort.UseVisualStyleBackColor = True
        '
        'rbtExcess
        '
        Me.rbtExcess.AutoSize = True
        Me.rbtExcess.Location = New System.Drawing.Point(132, 3)
        Me.rbtExcess.Name = "rbtExcess"
        Me.rbtExcess.Size = New System.Drawing.Size(64, 17)
        Me.rbtExcess.TabIndex = 2
        Me.rbtExcess.Text = "Excess"
        Me.rbtExcess.UseVisualStyleBackColor = True
        '
        'rbtSales
        '
        Me.rbtSales.AutoSize = True
        Me.rbtSales.Location = New System.Drawing.Point(201, 274)
        Me.rbtSales.Name = "rbtSales"
        Me.rbtSales.Size = New System.Drawing.Size(56, 17)
        Me.rbtSales.TabIndex = 17
        Me.rbtSales.Text = "Sales"
        Me.rbtSales.UseVisualStyleBackColor = True
        Me.rbtSales.Visible = False
        '
        'rbtStock
        '
        Me.rbtStock.AutoSize = True
        Me.rbtStock.Checked = True
        Me.rbtStock.Location = New System.Drawing.Point(138, 274)
        Me.rbtStock.Name = "rbtStock"
        Me.rbtStock.Size = New System.Drawing.Size(57, 17)
        Me.rbtStock.TabIndex = 16
        Me.rbtStock.TabStop = True
        Me.rbtStock.Text = "Stock"
        Me.rbtStock.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(20, 240)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(112, 21)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "As on Date"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(20, 179)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(112, 21)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(20, 148)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(112, 21)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Sub Item "
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(20, 117)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(112, 21)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Item Name"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbSubItemName
        '
        Me.cmbSubItemName.FormattingEnabled = True
        Me.cmbSubItemName.Location = New System.Drawing.Point(744, 244)
        Me.cmbSubItemName.Name = "cmbSubItemName"
        Me.cmbSubItemName.Size = New System.Drawing.Size(204, 21)
        Me.cmbSubItemName.TabIndex = 7
        Me.cmbSubItemName.Visible = False
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 0)
        Me.gridView.MultiSelect = False
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 18
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(1008, 529)
        Me.gridView.TabIndex = 0
        '
        'pnlView
        '
        Me.pnlView.Controls.Add(Me.pnlgrid)
        Me.pnlView.Controls.Add(Me.pnlFooter)
        Me.pnlView.Controls.Add(Me.lblTitle)
        Me.pnlView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlView.Location = New System.Drawing.Point(3, 3)
        Me.pnlView.Name = "pnlView"
        Me.pnlView.Size = New System.Drawing.Size(1008, 608)
        Me.pnlView.TabIndex = 3
        '
        'pnlgrid
        '
        Me.pnlgrid.Controls.Add(Me.pnlTagView)
        Me.pnlgrid.Controls.Add(Me.gridView)
        Me.pnlgrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlgrid.Location = New System.Drawing.Point(0, 31)
        Me.pnlgrid.Name = "pnlgrid"
        Me.pnlgrid.Size = New System.Drawing.Size(1008, 529)
        Me.pnlgrid.TabIndex = 3
        '
        'pnlTagView
        '
        Me.pnlTagView.Controls.Add(Me.btnOk)
        Me.pnlTagView.Controls.Add(Me.gridviewTag)
        Me.pnlTagView.Location = New System.Drawing.Point(197, 81)
        Me.pnlTagView.Name = "pnlTagView"
        Me.pnlTagView.Size = New System.Drawing.Size(573, 382)
        Me.pnlTagView.TabIndex = 1
        Me.pnlTagView.Visible = False
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(487, 343)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 31)
        Me.btnOk.TabIndex = 1
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'gridviewTag
        '
        Me.gridviewTag.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridviewTag.Location = New System.Drawing.Point(3, 3)
        Me.gridviewTag.Name = "gridviewTag"
        Me.gridviewTag.Size = New System.Drawing.Size(567, 333)
        Me.gridviewTag.TabIndex = 0
        '
        'pnlFooter
        '
        Me.pnlFooter.Controls.Add(Me.btnBack)
        Me.pnlFooter.Controls.Add(Me.btnExport)
        Me.pnlFooter.Controls.Add(Me.btnPrint)
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Location = New System.Drawing.Point(0, 560)
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Size = New System.Drawing.Size(1008, 48)
        Me.pnlFooter.TabIndex = 2
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(610, 3)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 2
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(716, 3)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 1
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(822, 3)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 0
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1008, 31)
        Me.lblTitle.TabIndex = 1
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlMain
        '
        Me.pnlMain.Controls.Add(Me.tabMain)
        Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlMain.Location = New System.Drawing.Point(0, 0)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(1022, 640)
        Me.pnlMain.TabIndex = 0
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGen)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1022, 640)
        Me.tabMain.TabIndex = 0
        '
        'tabGen
        '
        Me.tabGen.Controls.Add(Me.grpControls)
        Me.tabGen.Controls.Add(Me.cmbSubItemName)
        Me.tabGen.Location = New System.Drawing.Point(4, 22)
        Me.tabGen.Name = "tabGen"
        Me.tabGen.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGen.Size = New System.Drawing.Size(1014, 614)
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
        Me.tabView.Size = New System.Drawing.Size(1014, 614)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
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
        'frmReorderStock
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1022, 640)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmReorderStock"
        Me.Text = "Reorder Stock"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpControls.ResumeLayout(False)
        Me.grpControls.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlView.ResumeLayout(False)
        Me.pnlgrid.ResumeLayout(False)
        Me.pnlTagView.ResumeLayout(False)
        CType(Me.gridviewTag, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlFooter.ResumeLayout(False)
        Me.pnlMain.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGen.ResumeLayout(False)
        Me.tabView.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents grpControls As System.Windows.Forms.GroupBox
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents pnlView As System.Windows.Forms.Panel
    Friend WithEvents pnlMain As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbSubItemName As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rbtBoth As System.Windows.Forms.RadioButton
    Friend WithEvents rbtShort As System.Windows.Forms.RadioButton
    Friend WithEvents rbtExcess As System.Windows.Forms.RadioButton
    Friend WithEvents rbtSales As System.Windows.Forms.RadioButton
    Friend WithEvents rbtStock As System.Windows.Forms.RadioButton
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGen As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents pnlgrid As System.Windows.Forms.Panel
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents dtpAsOnDate As BrighttechPack.DatePicker
    Friend WithEvents cmbSize As System.Windows.Forms.ComboBox
    Friend WithEvents chkSize As System.Windows.Forms.CheckBox
    Friend WithEvents chkDetailedOthers As System.Windows.Forms.CheckBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rdbWeight As System.Windows.Forms.RadioButton
    Friend WithEvents rdbRate As System.Windows.Forms.RadioButton
    Friend WithEvents pnlTagView As System.Windows.Forms.Panel
    Friend WithEvents gridviewTag As System.Windows.Forms.DataGridView
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents chkCmbItem As BrighttechPack.CheckedComboBox
    Friend WithEvents cmbMetal As BrighttechPack.CheckedComboBox
    Friend WithEvents cmbCounter As BrighttechPack.CheckedComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents ChkOrder As System.Windows.Forms.CheckBox
    Friend WithEvents chkCmbSubItem As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtRangeTo As System.Windows.Forms.TextBox
    Friend WithEvents txtRangeFrom As System.Windows.Forms.TextBox
    Friend WithEvents lblWR As System.Windows.Forms.Label
    Friend WithEvents chkWithCaption As CheckBox
End Class
