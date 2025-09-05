<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmGSTR3BNEW
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
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AutoReziseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlGridHead = New System.Windows.Forms.Panel()
        Me.gridViewHead = New System.Windows.Forms.DataGridView()
        Me.pnlHeading = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExtToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbMetal = New System.Windows.Forms.ComboBox()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Chkonlycancel = New System.Windows.Forms.CheckBox()
        Me.chkcancel = New System.Windows.Forms.CheckBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.rbtmulti = New System.Windows.Forms.RadioButton()
        Me.rbtsingle = New System.Windows.Forms.RadioButton()
        Me.Pnltype = New System.Windows.Forms.Panel()
        Me.rbtInput = New System.Windows.Forms.RadioButton()
        Me.rbtOutput = New System.Windows.Forms.RadioButton()
        Me.rbtBothMU = New System.Windows.Forms.RadioButton()
        Me.lblentrymulti = New System.Windows.Forms.Label()
        Me.ChkcmbFilter = New BrighttechPack.CheckedComboBox()
        Me.PnlSaleType = New System.Windows.Forms.Panel()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbType = New System.Windows.Forms.ComboBox()
        Me.cmbFilter = New System.Windows.Forms.ComboBox()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.PnlAdv = New System.Windows.Forms.Panel()
        Me.lblAdv = New System.Windows.Forms.Label()
        Me.dtpAdv_OWN = New BrighttechPack.DatePicker(Me.components)
        Me.Panel2.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.pnlGridHead.SuspendLayout()
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlHeading.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Pnltype.SuspendLayout()
        Me.PnlSaleType.SuspendLayout()
        Me.PnlAdv.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.gridView)
        Me.Panel2.Controls.Add(Me.pnlGridHead)
        Me.Panel2.Controls.Add(Me.pnlHeading)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 158)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1015, 458)
        Me.Panel2.TabIndex = 1
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.ContextMenuStrip = Me.ContextMenuStrip2
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 43)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 21
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.Size = New System.Drawing.Size(1015, 415)
        Me.gridView.TabIndex = 0
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AutoReziseToolStripMenuItem})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(133, 26)
        '
        'AutoReziseToolStripMenuItem
        '
        Me.AutoReziseToolStripMenuItem.CheckOnClick = True
        Me.AutoReziseToolStripMenuItem.Name = "AutoReziseToolStripMenuItem"
        Me.AutoReziseToolStripMenuItem.Size = New System.Drawing.Size(132, 22)
        Me.AutoReziseToolStripMenuItem.Text = "AutoRezise"
        '
        'pnlGridHead
        '
        Me.pnlGridHead.Controls.Add(Me.gridViewHead)
        Me.pnlGridHead.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlGridHead.Location = New System.Drawing.Point(0, 25)
        Me.pnlGridHead.Name = "pnlGridHead"
        Me.pnlGridHead.Size = New System.Drawing.Size(1015, 18)
        Me.pnlGridHead.TabIndex = 6
        '
        'gridViewHead
        '
        Me.gridViewHead.AllowUserToAddRows = False
        Me.gridViewHead.AllowUserToDeleteRows = False
        Me.gridViewHead.AllowUserToResizeColumns = False
        Me.gridViewHead.AllowUserToResizeRows = False
        Me.gridViewHead.BackgroundColor = System.Drawing.SystemColors.ControlDark
        Me.gridViewHead.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridViewHead.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.gridViewHead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewHead.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridViewHead.Location = New System.Drawing.Point(0, 0)
        Me.gridViewHead.Name = "gridViewHead"
        Me.gridViewHead.ReadOnly = True
        Me.gridViewHead.RowHeadersVisible = False
        Me.gridViewHead.Size = New System.Drawing.Size(1015, 18)
        Me.gridViewHead.TabIndex = 0
        '
        'pnlHeading
        '
        Me.pnlHeading.Controls.Add(Me.lblTitle)
        Me.pnlHeading.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeading.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeading.Name = "pnlHeading"
        Me.pnlHeading.Size = New System.Drawing.Size(1015, 25)
        Me.pnlHeading.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1015, 25)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExtToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(119, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExtToolStripMenuItem
        '
        Me.ExtToolStripMenuItem.Name = "ExtToolStripMenuItem"
        Me.ExtToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExtToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.ExtToolStripMenuItem.Text = "Exit"
        Me.ExtToolStripMenuItem.Visible = False
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(313, 112)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 16
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(514, 112)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 18
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(414, 112)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 17
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date From"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(7, 39)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(7, 65)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(37, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Metal"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(186, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(23, 21)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbMetal
        '
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(87, 60)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(222, 21)
        Me.cmbMetal.TabIndex = 7
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(614, 112)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 19
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(714, 111)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 21
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Chkonlycancel)
        Me.Panel1.Controls.Add(Me.chkcancel)
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Controls.Add(Me.Pnltype)
        Me.Panel1.Controls.Add(Me.lblentrymulti)
        Me.Panel1.Controls.Add(Me.ChkcmbFilter)
        Me.Panel1.Controls.Add(Me.PnlSaleType)
        Me.Panel1.Controls.Add(Me.cmbFilter)
        Me.Panel1.Controls.Add(Me.lblStatus)
        Me.Panel1.Controls.Add(Me.chkCmbCostCentre)
        Me.Panel1.Controls.Add(Me.dtpTo)
        Me.Panel1.Controls.Add(Me.dtpFrom)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.cmbMetal)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.btnView_Search)
        Me.Panel1.Controls.Add(Me.PnlAdv)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1015, 158)
        Me.Panel1.TabIndex = 0
        '
        'Chkonlycancel
        '
        Me.Chkonlycancel.AutoSize = True
        Me.Chkonlycancel.Location = New System.Drawing.Point(414, 92)
        Me.Chkonlycancel.Name = "Chkonlycancel"
        Me.Chkonlycancel.Size = New System.Drawing.Size(95, 17)
        Me.Chkonlycancel.TabIndex = 22
        Me.Chkonlycancel.Text = "Only Cancel"
        Me.Chkonlycancel.UseVisualStyleBackColor = True
        '
        'chkcancel
        '
        Me.chkcancel.AutoSize = True
        Me.chkcancel.Location = New System.Drawing.Point(315, 92)
        Me.chkcancel.Name = "chkcancel"
        Me.chkcancel.Size = New System.Drawing.Size(94, 17)
        Me.chkcancel.TabIndex = 9
        Me.chkcancel.Text = "With Cancel"
        Me.chkcancel.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.rbtmulti)
        Me.Panel3.Controls.Add(Me.rbtsingle)
        Me.Panel3.Location = New System.Drawing.Point(492, 36)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(141, 26)
        Me.Panel3.TabIndex = 15
        Me.Panel3.Visible = False
        '
        'rbtmulti
        '
        Me.rbtmulti.AutoSize = True
        Me.rbtmulti.Location = New System.Drawing.Point(74, 3)
        Me.rbtmulti.Name = "rbtmulti"
        Me.rbtmulti.Size = New System.Drawing.Size(51, 17)
        Me.rbtmulti.TabIndex = 21
        Me.rbtmulti.Text = "Multi"
        Me.rbtmulti.UseVisualStyleBackColor = True
        '
        'rbtsingle
        '
        Me.rbtsingle.AutoSize = True
        Me.rbtsingle.Location = New System.Drawing.Point(8, 3)
        Me.rbtsingle.Name = "rbtsingle"
        Me.rbtsingle.Size = New System.Drawing.Size(60, 17)
        Me.rbtsingle.TabIndex = 0
        Me.rbtsingle.Text = "Single"
        Me.rbtsingle.UseVisualStyleBackColor = True
        '
        'Pnltype
        '
        Me.Pnltype.Controls.Add(Me.rbtInput)
        Me.Pnltype.Controls.Add(Me.rbtOutput)
        Me.Pnltype.Controls.Add(Me.rbtBothMU)
        Me.Pnltype.Location = New System.Drawing.Point(87, 88)
        Me.Pnltype.Name = "Pnltype"
        Me.Pnltype.Size = New System.Drawing.Size(223, 26)
        Me.Pnltype.TabIndex = 8
        '
        'rbtInput
        '
        Me.rbtInput.AutoSize = True
        Me.rbtInput.Location = New System.Drawing.Point(73, 3)
        Me.rbtInput.Name = "rbtInput"
        Me.rbtInput.Size = New System.Drawing.Size(55, 17)
        Me.rbtInput.TabIndex = 1
        Me.rbtInput.Text = "Input"
        Me.rbtInput.UseVisualStyleBackColor = True
        '
        'rbtOutput
        '
        Me.rbtOutput.AutoSize = True
        Me.rbtOutput.Location = New System.Drawing.Point(143, 3)
        Me.rbtOutput.Name = "rbtOutput"
        Me.rbtOutput.Size = New System.Drawing.Size(63, 17)
        Me.rbtOutput.TabIndex = 2
        Me.rbtOutput.Text = "Output"
        Me.rbtOutput.UseVisualStyleBackColor = True
        '
        'rbtBothMU
        '
        Me.rbtBothMU.AutoSize = True
        Me.rbtBothMU.Checked = True
        Me.rbtBothMU.Location = New System.Drawing.Point(6, 3)
        Me.rbtBothMU.Name = "rbtBothMU"
        Me.rbtBothMU.Size = New System.Drawing.Size(51, 17)
        Me.rbtBothMU.TabIndex = 0
        Me.rbtBothMU.TabStop = True
        Me.rbtBothMU.Text = "Both"
        Me.rbtBothMU.UseVisualStyleBackColor = True
        '
        'lblentrymulti
        '
        Me.lblentrymulti.AutoSize = True
        Me.lblentrymulti.Location = New System.Drawing.Point(7, 121)
        Me.lblentrymulti.Name = "lblentrymulti"
        Me.lblentrymulti.Size = New System.Drawing.Size(68, 13)
        Me.lblentrymulti.TabIndex = 10
        Me.lblentrymulti.Text = "Entry Type"
        Me.lblentrymulti.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ChkcmbFilter
        '
        Me.ChkcmbFilter.CheckOnClick = True
        Me.ChkcmbFilter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.ChkcmbFilter.DropDownHeight = 1
        Me.ChkcmbFilter.FormattingEnabled = True
        Me.ChkcmbFilter.IntegralHeight = False
        Me.ChkcmbFilter.Location = New System.Drawing.Point(87, 118)
        Me.ChkcmbFilter.Name = "ChkcmbFilter"
        Me.ChkcmbFilter.Size = New System.Drawing.Size(222, 22)
        Me.ChkcmbFilter.TabIndex = 11
        Me.ChkcmbFilter.ValueSeparator = ", "
        '
        'PnlSaleType
        '
        Me.PnlSaleType.Controls.Add(Me.Label6)
        Me.PnlSaleType.Controls.Add(Me.cmbType)
        Me.PnlSaleType.Location = New System.Drawing.Point(313, 38)
        Me.PnlSaleType.Name = "PnlSaleType"
        Me.PnlSaleType.Size = New System.Drawing.Size(173, 24)
        Me.PnlSaleType.TabIndex = 13
        Me.PnlSaleType.Visible = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(7, 6)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(34, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Type"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbType
        '
        Me.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbType.FormattingEnabled = True
        Me.cmbType.Location = New System.Drawing.Point(45, 2)
        Me.cmbType.Name = "cmbType"
        Me.cmbType.Size = New System.Drawing.Size(121, 21)
        Me.cmbType.TabIndex = 1
        '
        'cmbFilter
        '
        Me.cmbFilter.FormattingEnabled = True
        Me.cmbFilter.Location = New System.Drawing.Point(578, 12)
        Me.cmbFilter.Name = "cmbFilter"
        Me.cmbFilter.Size = New System.Drawing.Size(55, 21)
        Me.cmbFilter.TabIndex = 14
        Me.cmbFilter.Visible = False
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.Location = New System.Drawing.Point(693, 100)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(0, 13)
        Me.lblStatus.TabIndex = 20
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(87, 34)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(222, 22)
        Me.chkCmbCostCentre.TabIndex = 5
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(216, 9)
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
        Me.dtpFrom.Location = New System.Drawing.Point(87, 9)
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
        'PnlAdv
        '
        Me.PnlAdv.Controls.Add(Me.lblAdv)
        Me.PnlAdv.Controls.Add(Me.dtpAdv_OWN)
        Me.PnlAdv.Location = New System.Drawing.Point(313, 9)
        Me.PnlAdv.Name = "PnlAdv"
        Me.PnlAdv.Size = New System.Drawing.Size(259, 24)
        Me.PnlAdv.TabIndex = 12
        Me.PnlAdv.Visible = False
        '
        'lblAdv
        '
        Me.lblAdv.AutoSize = True
        Me.lblAdv.Location = New System.Drawing.Point(4, 5)
        Me.lblAdv.Name = "lblAdv"
        Me.lblAdv.Size = New System.Drawing.Size(144, 13)
        Me.lblAdv.TabIndex = 0
        Me.lblAdv.Text = "Advance Received After"
        Me.lblAdv.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpAdv_OWN
        '
        Me.dtpAdv_OWN.Location = New System.Drawing.Point(157, 2)
        Me.dtpAdv_OWN.Mask = "##/##/####"
        Me.dtpAdv_OWN.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpAdv_OWN.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpAdv_OWN.Name = "dtpAdv_OWN"
        Me.dtpAdv_OWN.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpAdv_OWN.Size = New System.Drawing.Size(93, 21)
        Me.dtpAdv_OWN.TabIndex = 1
        Me.dtpAdv_OWN.Text = "07/03/9998"
        Me.dtpAdv_OWN.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'frmGSTR3BNEW
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1015, 616)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmGSTR3BNEW"
        Me.Text = "GST Report"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel2.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip2.ResumeLayout(False)
        Me.pnlGridHead.ResumeLayout(False)
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlHeading.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Pnltype.ResumeLayout(False)
        Me.Pnltype.PerformLayout()
        Me.PnlSaleType.ResumeLayout(False)
        Me.PnlSaleType.PerformLayout()
        Me.PnlAdv.ResumeLayout(False)
        Me.PnlAdv.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents pnlHeading As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExtToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbMetal As System.Windows.Forms.ComboBox
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AutoReziseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlGridHead As System.Windows.Forms.Panel
    Friend WithEvents gridViewHead As System.Windows.Forms.DataGridView
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents cmbFilter As System.Windows.Forms.ComboBox
    Friend WithEvents dtpAdv_OWN As BrighttechPack.DatePicker
    Friend WithEvents PnlAdv As System.Windows.Forms.Panel
    Friend WithEvents lblAdv As System.Windows.Forms.Label
    Friend WithEvents PnlSaleType As Panel
    Friend WithEvents Label6 As Label
    Friend WithEvents cmbType As ComboBox
    Friend WithEvents lblentrymulti As Label
    Friend WithEvents ChkcmbFilter As BrighttechPack.CheckedComboBox
    Friend WithEvents Pnltype As Panel
    Friend WithEvents rbtInput As RadioButton
    Friend WithEvents rbtOutput As RadioButton
    Friend WithEvents rbtBothMU As RadioButton
    Friend WithEvents Panel3 As Panel
    Friend WithEvents rbtsingle As RadioButton
    Friend WithEvents rbtmulti As RadioButton
    Friend WithEvents chkcancel As CheckBox
    Friend WithEvents Chkonlycancel As CheckBox
End Class
