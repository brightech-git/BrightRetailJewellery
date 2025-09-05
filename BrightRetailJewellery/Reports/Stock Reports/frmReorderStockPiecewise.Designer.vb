<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReorderStockPiecewise
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
        Me.grpContainer = New System.Windows.Forms.GroupBox
        Me.chkItemLot = New System.Windows.Forms.CheckBox
        Me.rdbAll = New System.Windows.Forms.RadioButton
        Me.rdbStockWithoutSale = New System.Windows.Forms.RadioButton
        Me.rdbLessMinPcsStk = New System.Windows.Forms.RadioButton
        Me.rbtnilStockSales = New System.Windows.Forms.RadioButton
        Me.cmbSubItemGrp = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtNoofDays = New System.Windows.Forms.TextBox
        Me.chkSubitemAll = New System.Windows.Forms.CheckBox
        Me.chkLstSubitem = New System.Windows.Forms.CheckedListBox
        Me.dtpAsOnDate = New BrighttechPack.DatePicker(Me.components)
        Me.chkLstItem = New System.Windows.Forms.CheckedListBox
        Me.chkItemSelectAll = New System.Windows.Forms.CheckBox
        Me.chkLstDesigner = New System.Windows.Forms.CheckedListBox
        Me.chkItemCounterSelectAll = New System.Windows.Forms.CheckBox
        Me.chkDesignerSelectAll = New System.Windows.Forms.CheckBox
        Me.chkMetalSelectAll = New System.Windows.Forms.CheckBox
        Me.chkLstMetal = New System.Windows.Forms.CheckedListBox
        Me.chkLstItemCounter = New System.Windows.Forms.CheckedListBox
        Me.lblAsonDate = New System.Windows.Forms.Label
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.Tabmain = New System.Windows.Forms.TabControl
        Me.tabgeneral = New System.Windows.Forms.TabPage
        Me.tabview = New System.Windows.Forms.TabPage
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.GridView = New System.Windows.Forms.DataGridView
        Me.gridViewDetail = New System.Windows.Forms.DataGridView
        Me.gridViewHeader = New System.Windows.Forms.DataGridView
        Me.lbltitle = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.btnBack = New System.Windows.Forms.Button
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.EditToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.escapeToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem
        Me.SearchSubItemToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.UpdateMasterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.grpContainer.SuspendLayout()
        Me.Tabmain.SuspendLayout()
        Me.tabgeneral.SuspendLayout()
        Me.tabview.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridViewDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridViewHeader, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpContainer
        '
        Me.grpContainer.Controls.Add(Me.chkItemLot)
        Me.grpContainer.Controls.Add(Me.rdbAll)
        Me.grpContainer.Controls.Add(Me.rdbStockWithoutSale)
        Me.grpContainer.Controls.Add(Me.rdbLessMinPcsStk)
        Me.grpContainer.Controls.Add(Me.rbtnilStockSales)
        Me.grpContainer.Controls.Add(Me.cmbSubItemGrp)
        Me.grpContainer.Controls.Add(Me.Label2)
        Me.grpContainer.Controls.Add(Me.Label1)
        Me.grpContainer.Controls.Add(Me.txtNoofDays)
        Me.grpContainer.Controls.Add(Me.chkSubitemAll)
        Me.grpContainer.Controls.Add(Me.chkLstSubitem)
        Me.grpContainer.Controls.Add(Me.dtpAsOnDate)
        Me.grpContainer.Controls.Add(Me.chkLstItem)
        Me.grpContainer.Controls.Add(Me.chkItemSelectAll)
        Me.grpContainer.Controls.Add(Me.chkLstDesigner)
        Me.grpContainer.Controls.Add(Me.chkItemCounterSelectAll)
        Me.grpContainer.Controls.Add(Me.chkDesignerSelectAll)
        Me.grpContainer.Controls.Add(Me.chkMetalSelectAll)
        Me.grpContainer.Controls.Add(Me.chkLstMetal)
        Me.grpContainer.Controls.Add(Me.chkLstItemCounter)
        Me.grpContainer.Controls.Add(Me.lblAsonDate)
        Me.grpContainer.Controls.Add(Me.btnSearch)
        Me.grpContainer.Controls.Add(Me.btnExit)
        Me.grpContainer.Controls.Add(Me.btnNew)
        Me.grpContainer.Location = New System.Drawing.Point(294, 4)
        Me.grpContainer.Name = "grpContainer"
        Me.grpContainer.Size = New System.Drawing.Size(541, 581)
        Me.grpContainer.TabIndex = 0
        Me.grpContainer.TabStop = False
        '
        'chkItemLot
        '
        Me.chkItemLot.AutoSize = True
        Me.chkItemLot.Checked = True
        Me.chkItemLot.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkItemLot.Location = New System.Drawing.Point(14, 456)
        Me.chkItemLot.Name = "chkItemLot"
        Me.chkItemLot.Size = New System.Drawing.Size(99, 17)
        Me.chkItemLot.TabIndex = 14
        Me.chkItemLot.Text = "With LotView"
        Me.chkItemLot.UseVisualStyleBackColor = True
        '
        'rdbAll
        '
        Me.rdbAll.AutoSize = True
        Me.rdbAll.Checked = True
        Me.rdbAll.Location = New System.Drawing.Point(258, 502)
        Me.rdbAll.Name = "rdbAll"
        Me.rdbAll.Size = New System.Drawing.Size(39, 17)
        Me.rdbAll.TabIndex = 18
        Me.rdbAll.TabStop = True
        Me.rdbAll.Text = "All"
        Me.rdbAll.UseVisualStyleBackColor = True
        '
        'rdbStockWithoutSale
        '
        Me.rdbStockWithoutSale.AutoSize = True
        Me.rdbStockWithoutSale.Location = New System.Drawing.Point(14, 502)
        Me.rdbStockWithoutSale.Name = "rdbStockWithoutSale"
        Me.rdbStockWithoutSale.Size = New System.Drawing.Size(225, 17)
        Me.rdbStockWithoutSale.TabIndex = 17
        Me.rdbStockWithoutSale.Text = "Stock Without Sales Current Month"
        Me.rdbStockWithoutSale.UseVisualStyleBackColor = True
        '
        'rdbLessMinPcsStk
        '
        Me.rdbLessMinPcsStk.AutoSize = True
        Me.rdbLessMinPcsStk.Location = New System.Drawing.Point(14, 479)
        Me.rdbLessMinPcsStk.Name = "rdbLessMinPcsStk"
        Me.rdbLessMinPcsStk.Size = New System.Drawing.Size(161, 17)
        Me.rdbLessMinPcsStk.TabIndex = 15
        Me.rdbLessMinPcsStk.Text = "Less than Min.Pcs Stock"
        Me.rdbLessMinPcsStk.UseVisualStyleBackColor = True
        '
        'rbtnilStockSales
        '
        Me.rbtnilStockSales.AutoSize = True
        Me.rbtnilStockSales.Location = New System.Drawing.Point(258, 479)
        Me.rbtnilStockSales.Name = "rbtnilStockSales"
        Me.rbtnilStockSales.Size = New System.Drawing.Size(186, 17)
        Me.rbtnilStockSales.TabIndex = 16
        Me.rbtnilStockSales.Text = "Nil Stock in Same day Sales"
        Me.rbtnilStockSales.UseVisualStyleBackColor = True
        '
        'cmbSubItemGrp
        '
        Me.cmbSubItemGrp.FormattingEnabled = True
        Me.cmbSubItemGrp.Location = New System.Drawing.Point(119, 430)
        Me.cmbSubItemGrp.Name = "cmbSubItemGrp"
        Me.cmbSubItemGrp.Size = New System.Drawing.Size(369, 21)
        Me.cmbSubItemGrp.TabIndex = 13
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(14, 430)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(99, 21)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Sub Item Group "
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(258, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 13)
        Me.Label1.TabIndex = 19
        Me.Label1.Text = "No of Days"
        '
        'txtNoofDays
        '
        Me.txtNoofDays.Location = New System.Drawing.Point(334, 14)
        Me.txtNoofDays.Name = "txtNoofDays"
        Me.txtNoofDays.Size = New System.Drawing.Size(155, 21)
        Me.txtNoofDays.TabIndex = 20
        '
        'chkSubitemAll
        '
        Me.chkSubitemAll.AutoSize = True
        Me.chkSubitemAll.Location = New System.Drawing.Point(14, 305)
        Me.chkSubitemAll.Name = "chkSubitemAll"
        Me.chkSubitemAll.Size = New System.Drawing.Size(79, 17)
        Me.chkSubitemAll.TabIndex = 8
        Me.chkSubitemAll.Text = "Sub Item"
        Me.chkSubitemAll.UseVisualStyleBackColor = True
        '
        'chkLstSubitem
        '
        Me.chkLstSubitem.CheckOnClick = True
        Me.chkLstSubitem.FormattingEnabled = True
        Me.chkLstSubitem.Location = New System.Drawing.Point(14, 323)
        Me.chkLstSubitem.Name = "chkLstSubitem"
        Me.chkLstSubitem.Size = New System.Drawing.Size(231, 100)
        Me.chkLstSubitem.TabIndex = 9
        '
        'dtpAsOnDate
        '
        Me.dtpAsOnDate.Location = New System.Drawing.Point(98, 13)
        Me.dtpAsOnDate.Mask = "##/##/####"
        Me.dtpAsOnDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpAsOnDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpAsOnDate.Name = "dtpAsOnDate"
        Me.dtpAsOnDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpAsOnDate.Size = New System.Drawing.Size(93, 21)
        Me.dtpAsOnDate.TabIndex = 1
        Me.dtpAsOnDate.Text = "07/03/9998"
        Me.dtpAsOnDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'chkLstItem
        '
        Me.chkLstItem.CheckOnClick = True
        Me.chkLstItem.FormattingEnabled = True
        Me.chkLstItem.Location = New System.Drawing.Point(258, 185)
        Me.chkLstItem.Name = "chkLstItem"
        Me.chkLstItem.Size = New System.Drawing.Size(234, 116)
        Me.chkLstItem.TabIndex = 7
        '
        'chkItemSelectAll
        '
        Me.chkItemSelectAll.AutoSize = True
        Me.chkItemSelectAll.Location = New System.Drawing.Point(258, 168)
        Me.chkItemSelectAll.Name = "chkItemSelectAll"
        Me.chkItemSelectAll.Size = New System.Drawing.Size(53, 17)
        Me.chkItemSelectAll.TabIndex = 6
        Me.chkItemSelectAll.Text = "Item"
        Me.chkItemSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstDesigner
        '
        Me.chkLstDesigner.FormattingEnabled = True
        Me.chkLstDesigner.Location = New System.Drawing.Point(14, 63)
        Me.chkLstDesigner.MultiColumn = True
        Me.chkLstDesigner.Name = "chkLstDesigner"
        Me.chkLstDesigner.Size = New System.Drawing.Size(478, 100)
        Me.chkLstDesigner.TabIndex = 3
        '
        'chkItemCounterSelectAll
        '
        Me.chkItemCounterSelectAll.AutoSize = True
        Me.chkItemCounterSelectAll.Location = New System.Drawing.Point(258, 305)
        Me.chkItemCounterSelectAll.Name = "chkItemCounterSelectAll"
        Me.chkItemCounterSelectAll.Size = New System.Drawing.Size(103, 17)
        Me.chkItemCounterSelectAll.TabIndex = 10
        Me.chkItemCounterSelectAll.Text = "Item Counter"
        Me.chkItemCounterSelectAll.UseVisualStyleBackColor = True
        '
        'chkDesignerSelectAll
        '
        Me.chkDesignerSelectAll.AutoSize = True
        Me.chkDesignerSelectAll.Location = New System.Drawing.Point(14, 42)
        Me.chkDesignerSelectAll.Name = "chkDesignerSelectAll"
        Me.chkDesignerSelectAll.Size = New System.Drawing.Size(77, 17)
        Me.chkDesignerSelectAll.TabIndex = 2
        Me.chkDesignerSelectAll.Text = "Designer"
        Me.chkDesignerSelectAll.UseVisualStyleBackColor = True
        '
        'chkMetalSelectAll
        '
        Me.chkMetalSelectAll.AutoSize = True
        Me.chkMetalSelectAll.Location = New System.Drawing.Point(14, 167)
        Me.chkMetalSelectAll.Name = "chkMetalSelectAll"
        Me.chkMetalSelectAll.Size = New System.Drawing.Size(56, 17)
        Me.chkMetalSelectAll.TabIndex = 4
        Me.chkMetalSelectAll.Text = "Metal"
        Me.chkMetalSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstMetal
        '
        Me.chkLstMetal.CheckOnClick = True
        Me.chkLstMetal.FormattingEnabled = True
        Me.chkLstMetal.Location = New System.Drawing.Point(14, 185)
        Me.chkLstMetal.Name = "chkLstMetal"
        Me.chkLstMetal.Size = New System.Drawing.Size(231, 116)
        Me.chkLstMetal.TabIndex = 5
        '
        'chkLstItemCounter
        '
        Me.chkLstItemCounter.CheckOnClick = True
        Me.chkLstItemCounter.FormattingEnabled = True
        Me.chkLstItemCounter.Location = New System.Drawing.Point(258, 323)
        Me.chkLstItemCounter.Name = "chkLstItemCounter"
        Me.chkLstItemCounter.Size = New System.Drawing.Size(231, 100)
        Me.chkLstItemCounter.TabIndex = 11
        '
        'lblAsonDate
        '
        Me.lblAsonDate.AutoSize = True
        Me.lblAsonDate.Location = New System.Drawing.Point(10, 17)
        Me.lblAsonDate.Name = "lblAsonDate"
        Me.lblAsonDate.Size = New System.Drawing.Size(72, 13)
        Me.lblAsonDate.TabIndex = 0
        Me.lblAsonDate.Text = "As On Date"
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(64, 530)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(117, 30)
        Me.btnSearch.TabIndex = 21
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(311, 530)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(117, 30)
        Me.btnExit.TabIndex = 23
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(187, 530)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(117, 30)
        Me.btnNew.TabIndex = 22
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Tabmain
        '
        Me.Tabmain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.Tabmain.Controls.Add(Me.tabgeneral)
        Me.Tabmain.Controls.Add(Me.tabview)
        Me.Tabmain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Tabmain.Location = New System.Drawing.Point(0, 0)
        Me.Tabmain.Name = "Tabmain"
        Me.Tabmain.SelectedIndex = 0
        Me.Tabmain.Size = New System.Drawing.Size(1080, 674)
        Me.Tabmain.TabIndex = 0
        '
        'tabgeneral
        '
        Me.tabgeneral.Controls.Add(Me.grpContainer)
        Me.tabgeneral.Location = New System.Drawing.Point(4, 25)
        Me.tabgeneral.Name = "tabgeneral"
        Me.tabgeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabgeneral.Size = New System.Drawing.Size(1072, 645)
        Me.tabgeneral.TabIndex = 0
        Me.tabgeneral.Text = "tabgeneral"
        Me.tabgeneral.UseVisualStyleBackColor = True
        '
        'tabview
        '
        Me.tabview.Controls.Add(Me.Panel2)
        Me.tabview.Location = New System.Drawing.Point(4, 25)
        Me.tabview.Name = "tabview"
        Me.tabview.Padding = New System.Windows.Forms.Padding(3)
        Me.tabview.Size = New System.Drawing.Size(1072, 645)
        Me.tabview.TabIndex = 1
        Me.tabview.Text = "tabview"
        Me.tabview.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.Label6)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.GridView)
        Me.Panel2.Controls.Add(Me.gridViewDetail)
        Me.Panel2.Controls.Add(Me.gridViewHeader)
        Me.Panel2.Controls.Add(Me.lbltitle)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.btnBack)
        Me.Panel2.Controls.Add(Me.btnPrint)
        Me.Panel2.Controls.Add(Me.btnExport)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 3)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1066, 639)
        Me.Panel2.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Red
        Me.Label5.Location = New System.Drawing.Point(552, 557)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(154, 15)
        Me.Label5.TabIndex = 25
        Me.Label5.Text = "Ctrl+U - Update Master"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Red
        Me.Label6.Location = New System.Drawing.Point(353, 557)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(147, 15)
        Me.Label6.TabIndex = 24
        Me.Label6.Text = "Ctrl+F - Find SubItem "
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Red
        Me.Label4.Location = New System.Drawing.Point(163, 557)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(147, 15)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "Ctrl+S-SubItem Group"
        '
        'GridView
        '
        Me.GridView.AllowUserToAddRows = False
        Me.GridView.AllowUserToDeleteRows = False
        Me.GridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridView.Location = New System.Drawing.Point(7, 74)
        Me.GridView.Name = "GridView"
        Me.GridView.ReadOnly = True
        Me.GridView.Size = New System.Drawing.Size(1053, 410)
        Me.GridView.TabIndex = 12
        '
        'gridViewDetail
        '
        Me.gridViewDetail.AllowUserToAddRows = False
        Me.gridViewDetail.AllowUserToDeleteRows = False
        Me.gridViewDetail.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gridViewDetail.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridViewDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewDetail.Location = New System.Drawing.Point(7, 465)
        Me.gridViewDetail.Name = "gridViewDetail"
        Me.gridViewDetail.ReadOnly = True
        Me.gridViewDetail.Size = New System.Drawing.Size(1053, 110)
        Me.gridViewDetail.TabIndex = 10
        '
        'gridViewHeader
        '
        Me.gridViewHeader.AllowUserToAddRows = False
        Me.gridViewHeader.AllowUserToDeleteRows = False
        Me.gridViewHeader.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridViewHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewHeader.Enabled = False
        Me.gridViewHeader.Location = New System.Drawing.Point(7, 444)
        Me.gridViewHeader.Name = "gridViewHeader"
        Me.gridViewHeader.ReadOnly = True
        Me.gridViewHeader.RowHeadersVisible = False
        Me.gridViewHeader.Size = New System.Drawing.Size(1161, 19)
        Me.gridViewHeader.TabIndex = 11
        Me.gridViewHeader.Visible = False
        '
        'lbltitle
        '
        Me.lbltitle.AutoSize = True
        Me.lbltitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbltitle.Location = New System.Drawing.Point(498, 4)
        Me.lbltitle.Name = "lbltitle"
        Me.lbltitle.Size = New System.Drawing.Size(51, 15)
        Me.lbltitle.TabIndex = 0
        Me.lbltitle.Text = "Label2"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Red
        Me.Label3.Location = New System.Drawing.Point(10, 557)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(119, 15)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Ctrl+E-Auto Order"
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(776, 550)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(117, 30)
        Me.btnBack.TabIndex = 8
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(1028, 550)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(117, 30)
        Me.btnPrint.TabIndex = 7
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(902, 550)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(117, 30)
        Me.btnExport.TabIndex = 6
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditToolStripMenuItem1, Me.escapeToolStripMenuItem2, Me.SaveToolStripMenuItem3, Me.SearchSubItemToolStripMenuItem, Me.UpdateMasterToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(248, 114)
        '
        'EditToolStripMenuItem1
        '
        Me.EditToolStripMenuItem1.Name = "EditToolStripMenuItem1"
        Me.EditToolStripMenuItem1.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.EditToolStripMenuItem1.Size = New System.Drawing.Size(247, 22)
        Me.EditToolStripMenuItem1.Text = "EditToolStripMenuItem1"
        '
        'escapeToolStripMenuItem2
        '
        Me.escapeToolStripMenuItem2.Name = "escapeToolStripMenuItem2"
        Me.escapeToolStripMenuItem2.Size = New System.Drawing.Size(247, 22)
        Me.escapeToolStripMenuItem2.Text = "ToolStripMenuItem2"
        '
        'SaveToolStripMenuItem3
        '
        Me.SaveToolStripMenuItem3.Name = "SaveToolStripMenuItem3"
        Me.SaveToolStripMenuItem3.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.SaveToolStripMenuItem3.Size = New System.Drawing.Size(247, 22)
        Me.SaveToolStripMenuItem3.Text = "SaveToolStripMenuItem3"
        '
        'SearchSubItemToolStripMenuItem
        '
        Me.SearchSubItemToolStripMenuItem.Name = "SearchSubItemToolStripMenuItem"
        Me.SearchSubItemToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.SearchSubItemToolStripMenuItem.Size = New System.Drawing.Size(247, 22)
        Me.SearchSubItemToolStripMenuItem.Text = "SearchSubItem"
        '
        'UpdateMasterToolStripMenuItem
        '
        Me.UpdateMasterToolStripMenuItem.Name = "UpdateMasterToolStripMenuItem"
        Me.UpdateMasterToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.U), System.Windows.Forms.Keys)
        Me.UpdateMasterToolStripMenuItem.Size = New System.Drawing.Size(247, 22)
        Me.UpdateMasterToolStripMenuItem.Text = "UpdateMaster"
        '
        'frmReorderStockPiecewise
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1080, 674)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.Tabmain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "frmReorderStockPiecewise"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "PieceWise Reorder Stock"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpContainer.ResumeLayout(False)
        Me.grpContainer.PerformLayout()
        Me.Tabmain.ResumeLayout(False)
        Me.tabgeneral.ResumeLayout(False)
        Me.tabview.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridViewDetail, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridViewHeader, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents dtpAsOnDate As BrighttechPack.DatePicker
    Friend WithEvents chkLstItem As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkItemSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstDesigner As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkItemCounterSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkDesignerSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkMetalSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstMetal As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkLstItemCounter As System.Windows.Forms.CheckedListBox
    Friend WithEvents lblAsonDate As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents chkSubitemAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstSubitem As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtNoofDays As System.Windows.Forms.TextBox
    Friend WithEvents Tabmain As System.Windows.Forms.TabControl
    Friend WithEvents tabgeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabview As System.Windows.Forms.TabPage
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents lbltitle As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents EditToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents escapeToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents gridViewDetail As System.Windows.Forms.DataGridView
    Friend WithEvents gridViewHeader As System.Windows.Forms.DataGridView
    Friend WithEvents cmbSubItemGrp As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents rdbLessMinPcsStk As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnilStockSales As System.Windows.Forms.RadioButton
    Friend WithEvents rdbStockWithoutSale As System.Windows.Forms.RadioButton
    Friend WithEvents rdbAll As System.Windows.Forms.RadioButton
    Friend WithEvents GridView As System.Windows.Forms.DataGridView
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents SearchSubItemToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UpdateMasterToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents chkItemLot As System.Windows.Forms.CheckBox
End Class
