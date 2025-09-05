<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm4CReport
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
        Me.pnlMain = New System.Windows.Forms.Panel
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.grpFiltration = New System.Windows.Forms.GroupBox
        Me.cmbsubitem = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.CmbItem = New System.Windows.Forms.ComboBox
        Me.Label34 = New System.Windows.Forms.Label
        Me.Label35 = New System.Windows.Forms.Label
        Me.txtCaratTo = New System.Windows.Forms.TextBox
        Me.txtCaratFrom = New System.Windows.Forms.TextBox
        Me.Label32 = New System.Windows.Forms.Label
        Me.Label33 = New System.Windows.Forms.Label
        Me.txtPriceTo = New System.Windows.Forms.TextBox
        Me.txtPriceFrom = New System.Windows.Forms.TextBox
        Me.CmbSettype = New System.Windows.Forms.ComboBox
        Me.CmbShape = New System.Windows.Forms.ComboBox
        Me.CmbColor = New System.Windows.Forms.ComboBox
        Me.CmbClarity = New System.Windows.Forms.ComboBox
        Me.CmbCut = New System.Windows.Forms.ComboBox
        Me.Label30 = New System.Windows.Forms.Label
        Me.Label29 = New System.Windows.Forms.Label
        Me.Label28 = New System.Windows.Forms.Label
        Me.Label27 = New System.Windows.Forms.Label
        Me.Lable22 = New System.Windows.Forms.Label
        Me.Label21 = New System.Windows.Forms.Label
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.cmbCostCenter = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.rbtStock = New System.Windows.Forms.RadioButton
        Me.rbtSales = New System.Windows.Forms.RadioButton
        Me.tabView = New System.Windows.Forms.TabPage
        Me.pnlTotalGridView = New System.Windows.Forms.Panel
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AutoResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.lblReportTitle = New System.Windows.Forms.Label
        Me.pnlSummary = New System.Windows.Forms.Panel
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.btnBack = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.pnlMain.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.grpFiltration.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.pnlTotalGridView.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.pnlSummary.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlMain
        '
        Me.pnlMain.Controls.Add(Me.tabMain)
        Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlMain.Location = New System.Drawing.Point(0, 0)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(1022, 706)
        Me.pnlMain.TabIndex = 0
        '
        'tabMain
        '
        Me.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tabMain.Controls.Add(Me.tabGeneral)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.ItemSize = New System.Drawing.Size(10, 5)
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1022, 706)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.grpFiltration)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 9)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(1014, 693)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'grpFiltration
        '
        Me.grpFiltration.Controls.Add(Me.cmbsubitem)
        Me.grpFiltration.Controls.Add(Me.Label1)
        Me.grpFiltration.Controls.Add(Me.btnView_Search)
        Me.grpFiltration.Controls.Add(Me.CmbItem)
        Me.grpFiltration.Controls.Add(Me.Label34)
        Me.grpFiltration.Controls.Add(Me.Label35)
        Me.grpFiltration.Controls.Add(Me.txtCaratTo)
        Me.grpFiltration.Controls.Add(Me.txtCaratFrom)
        Me.grpFiltration.Controls.Add(Me.Label32)
        Me.grpFiltration.Controls.Add(Me.Label33)
        Me.grpFiltration.Controls.Add(Me.txtPriceTo)
        Me.grpFiltration.Controls.Add(Me.txtPriceFrom)
        Me.grpFiltration.Controls.Add(Me.CmbSettype)
        Me.grpFiltration.Controls.Add(Me.CmbShape)
        Me.grpFiltration.Controls.Add(Me.CmbColor)
        Me.grpFiltration.Controls.Add(Me.CmbClarity)
        Me.grpFiltration.Controls.Add(Me.CmbCut)
        Me.grpFiltration.Controls.Add(Me.Label30)
        Me.grpFiltration.Controls.Add(Me.Label29)
        Me.grpFiltration.Controls.Add(Me.Label28)
        Me.grpFiltration.Controls.Add(Me.Label27)
        Me.grpFiltration.Controls.Add(Me.Lable22)
        Me.grpFiltration.Controls.Add(Me.Label21)
        Me.grpFiltration.Controls.Add(Me.btnNew)
        Me.grpFiltration.Controls.Add(Me.btnExit)
        Me.grpFiltration.Controls.Add(Me.cmbCostCenter)
        Me.grpFiltration.Controls.Add(Me.Label9)
        Me.grpFiltration.Controls.Add(Me.Panel1)
        Me.grpFiltration.Location = New System.Drawing.Point(339, 112)
        Me.grpFiltration.Name = "grpFiltration"
        Me.grpFiltration.Size = New System.Drawing.Size(389, 375)
        Me.grpFiltration.TabIndex = 0
        Me.grpFiltration.TabStop = False
        '
        'cmbsubitem
        '
        Me.cmbsubitem.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbsubitem.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbsubitem.Enabled = False
        Me.cmbsubitem.FormattingEnabled = True
        Me.cmbsubitem.Location = New System.Drawing.Point(125, 52)
        Me.cmbsubitem.Name = "cmbsubitem"
        Me.cmbsubitem.Size = New System.Drawing.Size(229, 21)
        Me.cmbsubitem.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(29, 54)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "SubItem"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(37, 307)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 25
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'CmbItem
        '
        Me.CmbItem.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.CmbItem.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.CmbItem.FormattingEnabled = True
        Me.CmbItem.Location = New System.Drawing.Point(125, 28)
        Me.CmbItem.Name = "CmbItem"
        Me.CmbItem.Size = New System.Drawing.Size(229, 21)
        Me.CmbItem.TabIndex = 1
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.Location = New System.Drawing.Point(224, 230)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(21, 13)
        Me.Label34.TabIndex = 18
        Me.Label34.Text = "To"
        Me.Label34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.Location = New System.Drawing.Point(29, 229)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(72, 13)
        Me.Label35.TabIndex = 16
        Me.Label35.Text = "Carat From"
        Me.Label35.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtCaratTo
        '
        Me.txtCaratTo.BackColor = System.Drawing.SystemColors.Window
        Me.txtCaratTo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCaratTo.Location = New System.Drawing.Point(259, 226)
        Me.txtCaratTo.MaxLength = 8
        Me.txtCaratTo.Name = "txtCaratTo"
        Me.txtCaratTo.Size = New System.Drawing.Size(96, 21)
        Me.txtCaratTo.TabIndex = 19
        '
        'txtCaratFrom
        '
        Me.txtCaratFrom.BackColor = System.Drawing.SystemColors.Window
        Me.txtCaratFrom.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCaratFrom.Location = New System.Drawing.Point(126, 226)
        Me.txtCaratFrom.MaxLength = 8
        Me.txtCaratFrom.Name = "txtCaratFrom"
        Me.txtCaratFrom.Size = New System.Drawing.Size(96, 21)
        Me.txtCaratFrom.TabIndex = 17
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Location = New System.Drawing.Point(224, 255)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(21, 13)
        Me.Label32.TabIndex = 22
        Me.Label32.Text = "To"
        Me.Label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Location = New System.Drawing.Point(29, 254)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(68, 13)
        Me.Label33.TabIndex = 20
        Me.Label33.Text = "Price From"
        Me.Label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtPriceTo
        '
        Me.txtPriceTo.BackColor = System.Drawing.SystemColors.Window
        Me.txtPriceTo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtPriceTo.Location = New System.Drawing.Point(259, 251)
        Me.txtPriceTo.MaxLength = 8
        Me.txtPriceTo.Name = "txtPriceTo"
        Me.txtPriceTo.Size = New System.Drawing.Size(96, 21)
        Me.txtPriceTo.TabIndex = 23
        '
        'txtPriceFrom
        '
        Me.txtPriceFrom.BackColor = System.Drawing.SystemColors.Window
        Me.txtPriceFrom.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtPriceFrom.Location = New System.Drawing.Point(126, 251)
        Me.txtPriceFrom.MaxLength = 8
        Me.txtPriceFrom.Name = "txtPriceFrom"
        Me.txtPriceFrom.Size = New System.Drawing.Size(96, 21)
        Me.txtPriceFrom.TabIndex = 21
        '
        'CmbSettype
        '
        Me.CmbSettype.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.CmbSettype.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.CmbSettype.FormattingEnabled = True
        Me.CmbSettype.Location = New System.Drawing.Point(125, 196)
        Me.CmbSettype.Name = "CmbSettype"
        Me.CmbSettype.Size = New System.Drawing.Size(229, 21)
        Me.CmbSettype.TabIndex = 15
        '
        'CmbShape
        '
        Me.CmbShape.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.CmbShape.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.CmbShape.FormattingEnabled = True
        Me.CmbShape.Location = New System.Drawing.Point(125, 172)
        Me.CmbShape.Name = "CmbShape"
        Me.CmbShape.Size = New System.Drawing.Size(229, 21)
        Me.CmbShape.TabIndex = 13
        '
        'CmbColor
        '
        Me.CmbColor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.CmbColor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.CmbColor.FormattingEnabled = True
        Me.CmbColor.Location = New System.Drawing.Point(125, 148)
        Me.CmbColor.Name = "CmbColor"
        Me.CmbColor.Size = New System.Drawing.Size(229, 21)
        Me.CmbColor.TabIndex = 11
        '
        'CmbClarity
        '
        Me.CmbClarity.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.CmbClarity.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.CmbClarity.FormattingEnabled = True
        Me.CmbClarity.Location = New System.Drawing.Point(125, 124)
        Me.CmbClarity.Name = "CmbClarity"
        Me.CmbClarity.Size = New System.Drawing.Size(229, 21)
        Me.CmbClarity.TabIndex = 9
        '
        'CmbCut
        '
        Me.CmbCut.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.CmbCut.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.CmbCut.FormattingEnabled = True
        Me.CmbCut.Location = New System.Drawing.Point(125, 100)
        Me.CmbCut.Name = "CmbCut"
        Me.CmbCut.Size = New System.Drawing.Size(229, 21)
        Me.CmbCut.TabIndex = 7
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Location = New System.Drawing.Point(29, 179)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(76, 13)
        Me.Label30.TabIndex = 12
        Me.Label30.Text = "StoneShape"
        Me.Label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Location = New System.Drawing.Point(29, 204)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(87, 13)
        Me.Label29.TabIndex = 14
        Me.Label29.Text = "StoneSetType"
        Me.Label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Location = New System.Drawing.Point(29, 154)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(71, 13)
        Me.Label28.TabIndex = 10
        Me.Label28.Text = "StoneColor"
        Me.Label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Location = New System.Drawing.Point(29, 129)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(78, 13)
        Me.Label27.TabIndex = 8
        Me.Label27.Text = "StoneClarity"
        Me.Label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Lable22
        '
        Me.Lable22.AutoSize = True
        Me.Lable22.Location = New System.Drawing.Point(29, 104)
        Me.Lable22.Name = "Lable22"
        Me.Lable22.Size = New System.Drawing.Size(60, 13)
        Me.Lable22.TabIndex = 6
        Me.Lable22.Text = "StoneCut"
        Me.Lable22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(29, 29)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(34, 13)
        Me.Label21.TabIndex = 0
        Me.Label21.Text = "Item"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(148, 307)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 26
        Me.btnNew.TabStop = False
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(259, 307)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 27
        Me.btnExit.TabStop = False
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'cmbCostCenter
        '
        Me.cmbCostCenter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbCostCenter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbCostCenter.FormattingEnabled = True
        Me.cmbCostCenter.Location = New System.Drawing.Point(125, 76)
        Me.cmbCostCenter.Name = "cmbCostCenter"
        Me.cmbCostCenter.Size = New System.Drawing.Size(229, 21)
        Me.cmbCostCenter.TabIndex = 5
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(29, 79)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(76, 13)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "Cost Centre"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbtStock)
        Me.Panel1.Controls.Add(Me.rbtSales)
        Me.Panel1.Location = New System.Drawing.Point(122, 281)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(138, 18)
        Me.Panel1.TabIndex = 24
        '
        'rbtStock
        '
        Me.rbtStock.AutoSize = True
        Me.rbtStock.Location = New System.Drawing.Point(4, -1)
        Me.rbtStock.Name = "rbtStock"
        Me.rbtStock.Size = New System.Drawing.Size(57, 17)
        Me.rbtStock.TabIndex = 0
        Me.rbtStock.TabStop = True
        Me.rbtStock.Text = "Stock"
        Me.rbtStock.UseVisualStyleBackColor = True
        '
        'rbtSales
        '
        Me.rbtSales.AutoSize = True
        Me.rbtSales.Location = New System.Drawing.Point(70, -1)
        Me.rbtSales.Name = "rbtSales"
        Me.rbtSales.Size = New System.Drawing.Size(56, 17)
        Me.rbtSales.TabIndex = 1
        Me.rbtSales.TabStop = True
        Me.rbtSales.Text = "Sales"
        Me.rbtSales.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.pnlTotalGridView)
        Me.tabView.Location = New System.Drawing.Point(4, 9)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1014, 693)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'pnlTotalGridView
        '
        Me.pnlTotalGridView.Controls.Add(Me.gridView)
        Me.pnlTotalGridView.Controls.Add(Me.lblReportTitle)
        Me.pnlTotalGridView.Controls.Add(Me.pnlSummary)
        Me.pnlTotalGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlTotalGridView.Location = New System.Drawing.Point(3, 3)
        Me.pnlTotalGridView.Name = "pnlTotalGridView"
        Me.pnlTotalGridView.Size = New System.Drawing.Size(1008, 687)
        Me.pnlTotalGridView.TabIndex = 6
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.AllowUserToResizeRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.ContextMenuStrip = Me.ContextMenuStrip2
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 34)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.gridView.Size = New System.Drawing.Size(1008, 611)
        Me.gridView.TabIndex = 3
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AutoResizeToolStripMenuItem})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(136, 26)
        '
        'AutoResizeToolStripMenuItem
        '
        Me.AutoResizeToolStripMenuItem.CheckOnClick = True
        Me.AutoResizeToolStripMenuItem.Name = "AutoResizeToolStripMenuItem"
        Me.AutoResizeToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.AutoResizeToolStripMenuItem.Text = "Auto Resize"
        '
        'lblReportTitle
        '
        Me.lblReportTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblReportTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReportTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblReportTitle.Name = "lblReportTitle"
        Me.lblReportTitle.Size = New System.Drawing.Size(1008, 34)
        Me.lblReportTitle.TabIndex = 0
        Me.lblReportTitle.Text = "Label5"
        Me.lblReportTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblReportTitle.Visible = False
        '
        'pnlSummary
        '
        Me.pnlSummary.BackColor = System.Drawing.SystemColors.Control
        Me.pnlSummary.Controls.Add(Me.btnPrint)
        Me.pnlSummary.Controls.Add(Me.btnExport)
        Me.pnlSummary.Controls.Add(Me.btnBack)
        Me.pnlSummary.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlSummary.Location = New System.Drawing.Point(0, 645)
        Me.pnlSummary.Name = "pnlSummary"
        Me.pnlSummary.Size = New System.Drawing.Size(1008, 42)
        Me.pnlSummary.TabIndex = 2
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(856, 6)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(99, 29)
        Me.btnPrint.TabIndex = 4
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(751, 6)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(99, 29)
        Me.btnExport.TabIndex = 4
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(646, 6)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(99, 29)
        Me.btnBack.TabIndex = 3
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
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
        'ToolTip1
        '
        Me.ToolTip1.AutomaticDelay = 1000
        Me.ToolTip1.IsBalloon = True
        Me.ToolTip1.ShowAlways = True
        '
        'frm4CReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1022, 706)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frm4CReport"
        Me.Text = "Item Stock View"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlMain.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.grpFiltration.ResumeLayout(False)
        Me.grpFiltration.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.tabView.ResumeLayout(False)
        Me.pnlTotalGridView.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip2.ResumeLayout(False)
        Me.pnlSummary.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlMain As System.Windows.Forms.Panel
    Friend WithEvents pnlTotalGridView As System.Windows.Forms.Panel
    Friend WithEvents lblReportTitle As System.Windows.Forms.Label
    Friend WithEvents pnlSummary As System.Windows.Forms.Panel
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents grpFiltration As System.Windows.Forms.GroupBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents cmbCostCenter As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents Lable22 As System.Windows.Forms.Label
    Friend WithEvents CmbSettype As System.Windows.Forms.ComboBox
    Friend WithEvents CmbShape As System.Windows.Forms.ComboBox
    Friend WithEvents CmbColor As System.Windows.Forms.ComboBox
    Friend WithEvents CmbClarity As System.Windows.Forms.ComboBox
    Friend WithEvents CmbCut As System.Windows.Forms.ComboBox
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents txtCaratTo As System.Windows.Forms.TextBox
    Friend WithEvents txtCaratFrom As System.Windows.Forms.TextBox
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents txtPriceTo As System.Windows.Forms.TextBox
    Friend WithEvents txtPriceFrom As System.Windows.Forms.TextBox
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents CmbItem As System.Windows.Forms.ComboBox
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rbtStock As System.Windows.Forms.RadioButton
    Friend WithEvents rbtSales As System.Windows.Forms.RadioButton
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AutoResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbsubitem As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
