<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmStockSaleValueCheckRpt
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
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbGroupBy = New System.Windows.Forms.ComboBox()
        Me.chkcmbItemCtr = New BrighttechPack.CheckedComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtTagNo = New System.Windows.Forms.TextBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.btnExit = New System.Windows.Forms.Button()
        Me.chkHomeSalesView = New System.Windows.Forms.CheckBox()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.pnlOrderBy = New System.Windows.Forms.Panel()
        Me.rdbItemWise = New System.Windows.Forms.RadioButton()
        Me.rdbBillNoWise = New System.Windows.Forms.RadioButton()
        Me.lblOrderBy = New System.Windows.Forms.Label()
        Me.pnlDisplay = New System.Windows.Forms.Panel()
        Me.pnlDifference = New System.Windows.Forms.Panel()
        Me.lblGreaterThan = New System.Windows.Forms.Label()
        Me.lblPercentage = New System.Windows.Forms.Label()
        Me.txtDifference = New System.Windows.Forms.TextBox()
        Me.chkDifference = New System.Windows.Forms.CheckBox()
        Me.chkMinValue = New System.Windows.Forms.CheckBox()
        Me.cmbMetalName = New System.Windows.Forms.ComboBox()
        Me.cmbCostCentre = New System.Windows.Forms.ComboBox()
        Me.lblMetalName = New System.Windows.Forms.Label()
        Me.lblCostCentre = New System.Windows.Forms.Label()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.lblDateFrom = New System.Windows.Forms.Label()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.pnlGrid = New System.Windows.Forms.Panel()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabGen = New System.Windows.Forms.TabPage()
        Me.tabView = New System.Windows.Forms.TabPage()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.pnlTitle = New System.Windows.Forms.Panel()
        Me.grpControls.SuspendLayout()
        Me.pnlOrderBy.SuspendLayout()
        Me.pnlDisplay.SuspendLayout()
        Me.pnlDifference.SuspendLayout()
        Me.pnlGrid.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGen.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pnlTitle.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpControls
        '
        Me.grpControls.Controls.Add(Me.Label2)
        Me.grpControls.Controls.Add(Me.cmbGroupBy)
        Me.grpControls.Controls.Add(Me.chkcmbItemCtr)
        Me.grpControls.Controls.Add(Me.Label7)
        Me.grpControls.Controls.Add(Me.Label1)
        Me.grpControls.Controls.Add(Me.txtTagNo)
        Me.grpControls.Controls.Add(Me.dtpTo)
        Me.grpControls.Controls.Add(Me.dtpFrom)
        Me.grpControls.Controls.Add(Me.btnExit)
        Me.grpControls.Controls.Add(Me.chkHomeSalesView)
        Me.grpControls.Controls.Add(Me.btnNew)
        Me.grpControls.Controls.Add(Me.btnView_Search)
        Me.grpControls.Controls.Add(Me.pnlOrderBy)
        Me.grpControls.Controls.Add(Me.pnlDisplay)
        Me.grpControls.Controls.Add(Me.cmbMetalName)
        Me.grpControls.Controls.Add(Me.cmbCostCentre)
        Me.grpControls.Controls.Add(Me.lblMetalName)
        Me.grpControls.Controls.Add(Me.lblCostCentre)
        Me.grpControls.Controls.Add(Me.lblTo)
        Me.grpControls.Controls.Add(Me.lblDateFrom)
        Me.grpControls.Location = New System.Drawing.Point(300, 64)
        Me.grpControls.Name = "grpControls"
        Me.grpControls.Size = New System.Drawing.Size(411, 351)
        Me.grpControls.TabIndex = 0
        Me.grpControls.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 248)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(61, 13)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Group By"
        '
        'cmbGroupBy
        '
        Me.cmbGroupBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbGroupBy.FormattingEnabled = True
        Me.cmbGroupBy.Location = New System.Drawing.Point(98, 244)
        Me.cmbGroupBy.Name = "cmbGroupBy"
        Me.cmbGroupBy.Size = New System.Drawing.Size(297, 21)
        Me.cmbGroupBy.TabIndex = 15
        '
        'chkcmbItemCtr
        '
        Me.chkcmbItemCtr.CheckOnClick = True
        Me.chkcmbItemCtr.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbItemCtr.DropDownHeight = 1
        Me.chkcmbItemCtr.FormattingEnabled = True
        Me.chkcmbItemCtr.IntegralHeight = False
        Me.chkcmbItemCtr.Location = New System.Drawing.Point(98, 219)
        Me.chkcmbItemCtr.Name = "chkcmbItemCtr"
        Me.chkcmbItemCtr.Size = New System.Drawing.Size(297, 22)
        Me.chkcmbItemCtr.TabIndex = 13
        Me.chkcmbItemCtr.ValueSeparator = ", "
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(13, 224)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(84, 13)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "Item Counter"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 198)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(42, 13)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "TagNo"
        '
        'txtTagNo
        '
        Me.txtTagNo.Location = New System.Drawing.Point(98, 194)
        Me.txtTagNo.Name = "txtTagNo"
        Me.txtTagNo.Size = New System.Drawing.Size(297, 21)
        Me.txtTagNo.TabIndex = 11
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(273, 48)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(90, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(148, 48)
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
        Me.btnExit.Location = New System.Drawing.Point(294, 298)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(90, 30)
        Me.btnExit.TabIndex = 19
        Me.btnExit.Text = "&Exit[F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'chkHomeSalesView
        '
        Me.chkHomeSalesView.AutoSize = True
        Me.chkHomeSalesView.Location = New System.Drawing.Point(98, 171)
        Me.chkHomeSalesView.Name = "chkHomeSalesView"
        Me.chkHomeSalesView.Size = New System.Drawing.Size(117, 17)
        Me.chkHomeSalesView.TabIndex = 9
        Me.chkHomeSalesView.Text = "HomeSalesView"
        Me.chkHomeSalesView.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(196, 298)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(90, 30)
        Me.btnNew.TabIndex = 18
        Me.btnNew.Text = "&New[F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(98, 298)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(90, 30)
        Me.btnView_Search.TabIndex = 17
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'pnlOrderBy
        '
        Me.pnlOrderBy.Controls.Add(Me.rdbItemWise)
        Me.pnlOrderBy.Controls.Add(Me.rdbBillNoWise)
        Me.pnlOrderBy.Controls.Add(Me.lblOrderBy)
        Me.pnlOrderBy.Location = New System.Drawing.Point(98, 271)
        Me.pnlOrderBy.Name = "pnlOrderBy"
        Me.pnlOrderBy.Size = New System.Drawing.Size(297, 21)
        Me.pnlOrderBy.TabIndex = 16
        '
        'rdbItemWise
        '
        Me.rdbItemWise.AutoSize = True
        Me.rdbItemWise.Location = New System.Drawing.Point(196, 2)
        Me.rdbItemWise.Name = "rdbItemWise"
        Me.rdbItemWise.Size = New System.Drawing.Size(79, 17)
        Me.rdbItemWise.TabIndex = 2
        Me.rdbItemWise.TabStop = True
        Me.rdbItemWise.Text = "ItemWise"
        Me.rdbItemWise.UseVisualStyleBackColor = True
        '
        'rdbBillNoWise
        '
        Me.rdbBillNoWise.AutoSize = True
        Me.rdbBillNoWise.Location = New System.Drawing.Point(89, 2)
        Me.rdbBillNoWise.Name = "rdbBillNoWise"
        Me.rdbBillNoWise.Size = New System.Drawing.Size(84, 17)
        Me.rdbBillNoWise.TabIndex = 1
        Me.rdbBillNoWise.TabStop = True
        Me.rdbBillNoWise.Text = "BillNoWise"
        Me.rdbBillNoWise.UseVisualStyleBackColor = True
        '
        'lblOrderBy
        '
        Me.lblOrderBy.AutoSize = True
        Me.lblOrderBy.Location = New System.Drawing.Point(9, 4)
        Me.lblOrderBy.Name = "lblOrderBy"
        Me.lblOrderBy.Size = New System.Drawing.Size(55, 13)
        Me.lblOrderBy.TabIndex = 0
        Me.lblOrderBy.Text = "OrderBy"
        '
        'pnlDisplay
        '
        Me.pnlDisplay.Controls.Add(Me.pnlDifference)
        Me.pnlDisplay.Controls.Add(Me.chkMinValue)
        Me.pnlDisplay.Location = New System.Drawing.Point(98, 128)
        Me.pnlDisplay.Name = "pnlDisplay"
        Me.pnlDisplay.Size = New System.Drawing.Size(297, 37)
        Me.pnlDisplay.TabIndex = 8
        '
        'pnlDifference
        '
        Me.pnlDifference.Controls.Add(Me.lblGreaterThan)
        Me.pnlDifference.Controls.Add(Me.lblPercentage)
        Me.pnlDifference.Controls.Add(Me.txtDifference)
        Me.pnlDifference.Controls.Add(Me.chkDifference)
        Me.pnlDifference.Location = New System.Drawing.Point(3, 7)
        Me.pnlDifference.Name = "pnlDifference"
        Me.pnlDifference.Size = New System.Drawing.Size(197, 25)
        Me.pnlDifference.TabIndex = 0
        '
        'lblGreaterThan
        '
        Me.lblGreaterThan.AutoSize = True
        Me.lblGreaterThan.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGreaterThan.Location = New System.Drawing.Point(91, 4)
        Me.lblGreaterThan.Name = "lblGreaterThan"
        Me.lblGreaterThan.Size = New System.Drawing.Size(25, 13)
        Me.lblGreaterThan.TabIndex = 1
        Me.lblGreaterThan.Text = ">="
        Me.lblGreaterThan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPercentage
        '
        Me.lblPercentage.AutoSize = True
        Me.lblPercentage.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPercentage.Location = New System.Drawing.Point(170, 5)
        Me.lblPercentage.Name = "lblPercentage"
        Me.lblPercentage.Size = New System.Drawing.Size(21, 13)
        Me.lblPercentage.TabIndex = 2
        Me.lblPercentage.Text = "%"
        Me.lblPercentage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtDifference
        '
        Me.txtDifference.Location = New System.Drawing.Point(122, 1)
        Me.txtDifference.MaxLength = 6
        Me.txtDifference.Name = "txtDifference"
        Me.txtDifference.Size = New System.Drawing.Size(46, 21)
        Me.txtDifference.TabIndex = 2
        Me.txtDifference.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'chkDifference
        '
        Me.chkDifference.AutoSize = True
        Me.chkDifference.Location = New System.Drawing.Point(3, 3)
        Me.chkDifference.Name = "chkDifference"
        Me.chkDifference.Size = New System.Drawing.Size(89, 17)
        Me.chkDifference.TabIndex = 0
        Me.chkDifference.Text = "Difference "
        Me.chkDifference.UseVisualStyleBackColor = True
        '
        'chkMinValue
        '
        Me.chkMinValue.AutoSize = True
        Me.chkMinValue.Location = New System.Drawing.Point(206, 10)
        Me.chkMinValue.Name = "chkMinValue"
        Me.chkMinValue.Size = New System.Drawing.Size(76, 17)
        Me.chkMinValue.TabIndex = 0
        Me.chkMinValue.Text = "MinValue"
        Me.chkMinValue.UseVisualStyleBackColor = True
        '
        'cmbMetalName
        '
        Me.cmbMetalName.FormattingEnabled = True
        Me.cmbMetalName.Location = New System.Drawing.Point(98, 101)
        Me.cmbMetalName.Name = "cmbMetalName"
        Me.cmbMetalName.Size = New System.Drawing.Size(297, 21)
        Me.cmbMetalName.TabIndex = 7
        '
        'cmbCostCentre
        '
        Me.cmbCostCentre.FormattingEnabled = True
        Me.cmbCostCentre.Location = New System.Drawing.Point(98, 74)
        Me.cmbCostCentre.Name = "cmbCostCentre"
        Me.cmbCostCentre.Size = New System.Drawing.Size(297, 21)
        Me.cmbCostCentre.TabIndex = 5
        '
        'lblMetalName
        '
        Me.lblMetalName.AutoSize = True
        Me.lblMetalName.Location = New System.Drawing.Point(13, 105)
        Me.lblMetalName.Name = "lblMetalName"
        Me.lblMetalName.Size = New System.Drawing.Size(70, 13)
        Me.lblMetalName.TabIndex = 6
        Me.lblMetalName.Text = "MetalName"
        '
        'lblCostCentre
        '
        Me.lblCostCentre.AutoSize = True
        Me.lblCostCentre.Location = New System.Drawing.Point(13, 78)
        Me.lblCostCentre.Name = "lblCostCentre"
        Me.lblCostCentre.Size = New System.Drawing.Size(72, 13)
        Me.lblCostCentre.TabIndex = 4
        Me.lblCostCentre.Text = "CostCentre"
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(247, 52)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(20, 13)
        Me.lblTo.TabIndex = 2
        Me.lblTo.Text = "To"
        '
        'lblDateFrom
        '
        Me.lblDateFrom.AutoSize = True
        Me.lblDateFrom.Location = New System.Drawing.Point(98, 52)
        Me.lblDateFrom.Name = "lblDateFrom"
        Me.lblDateFrom.Size = New System.Drawing.Size(36, 13)
        Me.lblDateFrom.TabIndex = 0
        Me.lblDateFrom.Text = "From"
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(796, 4)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 9
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(684, 4)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 8
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'pnlGrid
        '
        Me.pnlGrid.Controls.Add(Me.gridView)
        Me.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlGrid.Location = New System.Drawing.Point(3, 23)
        Me.pnlGrid.Name = "pnlGrid"
        Me.pnlGrid.Size = New System.Drawing.Size(1005, 416)
        Me.pnlGrid.TabIndex = 1
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
        Me.gridView.Size = New System.Drawing.Size(1005, 416)
        Me.gridView.StandardTab = True
        Me.gridView.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1005, 20)
        Me.lblTitle.TabIndex = 1
        Me.lblTitle.Text = "TITLE"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
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
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGen)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1019, 505)
        Me.tabMain.TabIndex = 0
        '
        'tabGen
        '
        Me.tabGen.Controls.Add(Me.grpControls)
        Me.tabGen.Location = New System.Drawing.Point(4, 22)
        Me.tabGen.Name = "tabGen"
        Me.tabGen.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGen.Size = New System.Drawing.Size(1011, 479)
        Me.tabGen.TabIndex = 0
        Me.tabGen.Text = "Gen"
        Me.tabGen.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.pnlGrid)
        Me.tabView.Controls.Add(Me.Panel1)
        Me.tabView.Controls.Add(Me.pnlTitle)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1011, 479)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnBack)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(3, 439)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1005, 37)
        Me.Panel1.TabIndex = 1
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(572, 4)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 10
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'pnlTitle
        '
        Me.pnlTitle.Controls.Add(Me.lblTitle)
        Me.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTitle.Location = New System.Drawing.Point(3, 3)
        Me.pnlTitle.Name = "pnlTitle"
        Me.pnlTitle.Size = New System.Drawing.Size(1005, 20)
        Me.pnlTitle.TabIndex = 0
        '
        'FrmStockSaleValueCheckRpt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(1019, 505)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "FrmStockSaleValueCheckRpt"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "FrmStockSaleValueCheck"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpControls.ResumeLayout(False)
        Me.grpControls.PerformLayout()
        Me.pnlOrderBy.ResumeLayout(False)
        Me.pnlOrderBy.PerformLayout()
        Me.pnlDisplay.ResumeLayout(False)
        Me.pnlDisplay.PerformLayout()
        Me.pnlDifference.ResumeLayout(False)
        Me.pnlDifference.PerformLayout()
        Me.pnlGrid.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGen.ResumeLayout(False)
        Me.tabView.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.pnlTitle.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpControls As System.Windows.Forms.GroupBox
    Friend WithEvents pnlDisplay As System.Windows.Forms.Panel
    Friend WithEvents pnlOrderBy As System.Windows.Forms.Panel
    Friend WithEvents txtDifference As System.Windows.Forms.TextBox
    Friend WithEvents cmbMetalName As System.Windows.Forms.ComboBox
    Friend WithEvents cmbCostCentre As System.Windows.Forms.ComboBox
    Friend WithEvents lblMetalName As System.Windows.Forms.Label
    Friend WithEvents lblCostCentre As System.Windows.Forms.Label
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents lblDateFrom As System.Windows.Forms.Label
    Friend WithEvents rdbItemWise As System.Windows.Forms.RadioButton
    Friend WithEvents rdbBillNoWise As System.Windows.Forms.RadioButton
    Friend WithEvents lblOrderBy As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents pnlGrid As System.Windows.Forms.Panel
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkHomeSalesView As System.Windows.Forms.CheckBox
    Friend WithEvents chkMinValue As System.Windows.Forms.CheckBox
    Friend WithEvents chkDifference As System.Windows.Forms.CheckBox
    Friend WithEvents pnlDifference As System.Windows.Forms.Panel
    Friend WithEvents lblGreaterThan As System.Windows.Forms.Label
    Friend WithEvents lblPercentage As System.Windows.Forms.Label
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGen As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents pnlTitle As System.Windows.Forms.Panel
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents Label1 As Label
    Friend WithEvents txtTagNo As TextBox
    Friend WithEvents chkcmbItemCtr As BrighttechPack.CheckedComboBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents cmbGroupBy As ComboBox
End Class
