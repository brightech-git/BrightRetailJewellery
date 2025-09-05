<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmReorderWeightBased
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
        Me.gridheader1 = New System.Windows.Forms.DataGridView()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExtToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.PanelRange = New System.Windows.Forms.Panel()
        Me.rbtRangeOut = New System.Windows.Forms.RadioButton()
        Me.rbtRangeAll = New System.Windows.Forms.RadioButton()
        Me.rbtRangeIn = New System.Windows.Forms.RadioButton()
        Me.lblRangeType = New System.Windows.Forms.Label()
        Me.chkWithCaption = New System.Windows.Forms.CheckBox()
        Me.chkEmptyStock = New System.Windows.Forms.CheckBox()
        Me.chkwithoutStockOnly = New System.Windows.Forms.CheckBox()
        Me.cmbItemName_OWN = New System.Windows.Forms.ComboBox()
        Me.cmbCostCentre_OWN = New System.Windows.Forms.ComboBox()
        Me.chkwithsalvalue = New System.Windows.Forms.CheckBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.rbthorizontal = New System.Windows.Forms.RadioButton()
        Me.rbtvertical = New System.Windows.Forms.RadioButton()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Itemname = New System.Windows.Forms.Label()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.rbtitem = New System.Windows.Forms.RadioButton()
        Me.rbtdesigner = New System.Windows.Forms.RadioButton()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.rbtboth = New System.Windows.Forms.RadioButton()
        Me.rbtNontag = New System.Windows.Forms.RadioButton()
        Me.rbttag = New System.Windows.Forms.RadioButton()
        Me.chkwithcum = New System.Windows.Forms.CheckBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Panel8 = New System.Windows.Forms.Panel()
        Me.gridview = New System.Windows.Forms.DataGridView()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.btnback = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.Panel7 = New System.Windows.Forms.Panel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.chkcmbrange = New BrighttechPack.CheckedComboBox()
        Me.chkcmbsutitem = New BrighttechPack.CheckedComboBox()
        Me.chkcmbitem = New BrighttechPack.CheckedComboBox()
        Me.chkcmbcompany = New BrighttechPack.CheckedComboBox()
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox()
        Me.chkcmbdesigner = New BrighttechPack.CheckedComboBox()
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.chkcmbmetal = New BrighttechPack.CheckedComboBox()
        CType(Me.gridheader1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.PanelRange.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.Panel8.SuspendLayout()
        CType(Me.gridview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        Me.Panel7.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'gridheader1
        '
        Me.gridheader1.AllowUserToAddRows = False
        Me.gridheader1.AllowUserToDeleteRows = False
        Me.gridheader1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridheader1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridheader1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridheader1.Enabled = False
        Me.gridheader1.Location = New System.Drawing.Point(0, 0)
        Me.gridheader1.Name = "gridheader1"
        Me.gridheader1.ReadOnly = True
        Me.gridheader1.RowHeadersVisible = False
        Me.gridheader1.RowTemplate.Height = 21
        Me.gridheader1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridheader1.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.gridheader1.Size = New System.Drawing.Size(1127, 25)
        Me.gridheader1.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.SystemColors.Control
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1133, 28)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExtToolStripMenuItem, Me.ResizeToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(133, 70)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(132, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExtToolStripMenuItem
        '
        Me.ExtToolStripMenuItem.Name = "ExtToolStripMenuItem"
        Me.ExtToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExtToolStripMenuItem.Size = New System.Drawing.Size(132, 22)
        Me.ExtToolStripMenuItem.Text = "Exit"
        Me.ExtToolStripMenuItem.Visible = False
        '
        'ResizeToolStripMenuItem
        '
        Me.ResizeToolStripMenuItem.Name = "ResizeToolStripMenuItem"
        Me.ResizeToolStripMenuItem.Size = New System.Drawing.Size(132, 22)
        Me.ResizeToolStripMenuItem.Text = "AutoResize"
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(46, 357)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 28
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(245, 357)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 30
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(145, 357)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 29
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(27, 52)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 21)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "As OnDate"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(27, 104)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(79, 21)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.Panel1.Controls.Add(Me.TabControl1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1147, 676)
        Me.Panel1.TabIndex = 0
        '
        'TabControl1
        '
        Me.TabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1147, 676)
        Me.TabControl1.TabIndex = 1
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Label11)
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        Me.TabPage1.Controls.Add(Me.chkcmbmetal)
        Me.TabPage1.Controls.Add(Me.Label4)
        Me.TabPage1.Controls.Add(Me.Label6)
        Me.TabPage1.Controls.Add(Me.Panel4)
        Me.TabPage1.Controls.Add(Me.chkwithcum)
        Me.TabPage1.Location = New System.Drawing.Point(4, 25)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(1139, 647)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "TabPage1"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(8, 171)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(277, 72)
        Me.Label11.TabIndex = 5
        Me.Label11.Text = "*Hint " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Cost Centre, Item Name checkedcombox" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Not View Reason" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "output Invalid" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label11.Visible = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.PanelRange)
        Me.GroupBox1.Controls.Add(Me.lblRangeType)
        Me.GroupBox1.Controls.Add(Me.chkWithCaption)
        Me.GroupBox1.Controls.Add(Me.chkEmptyStock)
        Me.GroupBox1.Controls.Add(Me.chkwithoutStockOnly)
        Me.GroupBox1.Controls.Add(Me.cmbItemName_OWN)
        Me.GroupBox1.Controls.Add(Me.cmbCostCentre_OWN)
        Me.GroupBox1.Controls.Add(Me.chkwithsalvalue)
        Me.GroupBox1.Controls.Add(Me.chkcmbrange)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.Panel2)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.chkcmbsutitem)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.Itemname)
        Me.GroupBox1.Controls.Add(Me.Panel5)
        Me.GroupBox1.Controls.Add(Me.btnNew)
        Me.GroupBox1.Controls.Add(Me.chkcmbitem)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.chkcmbcompany)
        Me.GroupBox1.Controls.Add(Me.btnExit)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.btnView_Search)
        Me.GroupBox1.Controls.Add(Me.chkCmbCostCentre)
        Me.GroupBox1.Controls.Add(Me.chkcmbdesigner)
        Me.GroupBox1.Controls.Add(Me.dtpFrom)
        Me.GroupBox1.Location = New System.Drawing.Point(339, 145)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(571, 417)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'PanelRange
        '
        Me.PanelRange.Controls.Add(Me.rbtRangeOut)
        Me.PanelRange.Controls.Add(Me.rbtRangeAll)
        Me.PanelRange.Controls.Add(Me.rbtRangeIn)
        Me.PanelRange.Location = New System.Drawing.Point(127, 279)
        Me.PanelRange.Name = "PanelRange"
        Me.PanelRange.Size = New System.Drawing.Size(218, 22)
        Me.PanelRange.TabIndex = 23
        Me.PanelRange.Visible = False
        '
        'rbtRangeOut
        '
        Me.rbtRangeOut.AutoSize = True
        Me.rbtRangeOut.Location = New System.Drawing.Point(134, 3)
        Me.rbtRangeOut.Name = "rbtRangeOut"
        Me.rbtRangeOut.Size = New System.Drawing.Size(45, 17)
        Me.rbtRangeOut.TabIndex = 2
        Me.rbtRangeOut.TabStop = True
        Me.rbtRangeOut.Text = "Out"
        Me.rbtRangeOut.UseVisualStyleBackColor = True
        '
        'rbtRangeAll
        '
        Me.rbtRangeAll.AutoSize = True
        Me.rbtRangeAll.Checked = True
        Me.rbtRangeAll.Location = New System.Drawing.Point(6, 2)
        Me.rbtRangeAll.Name = "rbtRangeAll"
        Me.rbtRangeAll.Size = New System.Drawing.Size(39, 17)
        Me.rbtRangeAll.TabIndex = 0
        Me.rbtRangeAll.TabStop = True
        Me.rbtRangeAll.Text = "All"
        Me.rbtRangeAll.UseVisualStyleBackColor = True
        '
        'rbtRangeIn
        '
        Me.rbtRangeIn.AutoSize = True
        Me.rbtRangeIn.Location = New System.Drawing.Point(71, 2)
        Me.rbtRangeIn.Name = "rbtRangeIn"
        Me.rbtRangeIn.Size = New System.Drawing.Size(37, 17)
        Me.rbtRangeIn.TabIndex = 1
        Me.rbtRangeIn.TabStop = True
        Me.rbtRangeIn.Text = "In"
        Me.rbtRangeIn.UseVisualStyleBackColor = True
        '
        'lblRangeType
        '
        Me.lblRangeType.Location = New System.Drawing.Point(27, 280)
        Me.lblRangeType.Name = "lblRangeType"
        Me.lblRangeType.Size = New System.Drawing.Size(97, 21)
        Me.lblRangeType.TabIndex = 22
        Me.lblRangeType.Text = "Range Type"
        Me.lblRangeType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblRangeType.Visible = False
        '
        'chkWithCaption
        '
        Me.chkWithCaption.AutoSize = True
        Me.chkWithCaption.Location = New System.Drawing.Point(216, 331)
        Me.chkWithCaption.Name = "chkWithCaption"
        Me.chkWithCaption.Size = New System.Drawing.Size(99, 17)
        Me.chkWithCaption.TabIndex = 27
        Me.chkWithCaption.Text = "With Caption"
        Me.chkWithCaption.UseVisualStyleBackColor = True
        Me.chkWithCaption.Visible = False
        '
        'chkEmptyStock
        '
        Me.chkEmptyStock.AutoSize = True
        Me.chkEmptyStock.Checked = True
        Me.chkEmptyStock.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkEmptyStock.Location = New System.Drawing.Point(50, 331)
        Me.chkEmptyStock.Name = "chkEmptyStock"
        Me.chkEmptyStock.Size = New System.Drawing.Size(147, 17)
        Me.chkEmptyStock.TabIndex = 26
        Me.chkEmptyStock.Text = "WithOut Empty Stock"
        Me.chkEmptyStock.UseVisualStyleBackColor = True
        '
        'chkwithoutStockOnly
        '
        Me.chkwithoutStockOnly.AutoSize = True
        Me.chkwithoutStockOnly.Checked = True
        Me.chkwithoutStockOnly.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkwithoutStockOnly.Location = New System.Drawing.Point(216, 308)
        Me.chkwithoutStockOnly.Name = "chkwithoutStockOnly"
        Me.chkwithoutStockOnly.Size = New System.Drawing.Size(107, 17)
        Me.chkwithoutStockOnly.TabIndex = 25
        Me.chkwithoutStockOnly.Text = "WithOut Stock"
        Me.chkwithoutStockOnly.UseVisualStyleBackColor = True
        '
        'cmbItemName_OWN
        '
        Me.cmbItemName_OWN.FormattingEnabled = True
        Me.cmbItemName_OWN.Location = New System.Drawing.Point(127, 129)
        Me.cmbItemName_OWN.Name = "cmbItemName_OWN"
        Me.cmbItemName_OWN.Size = New System.Drawing.Size(218, 21)
        Me.cmbItemName_OWN.TabIndex = 7
        '
        'cmbCostCentre_OWN
        '
        Me.cmbCostCentre_OWN.FormattingEnabled = True
        Me.cmbCostCentre_OWN.Location = New System.Drawing.Point(127, 104)
        Me.cmbCostCentre_OWN.Name = "cmbCostCentre_OWN"
        Me.cmbCostCentre_OWN.Size = New System.Drawing.Size(218, 21)
        Me.cmbCostCentre_OWN.TabIndex = 6
        '
        'chkwithsalvalue
        '
        Me.chkwithsalvalue.AutoSize = True
        Me.chkwithsalvalue.Location = New System.Drawing.Point(50, 308)
        Me.chkwithsalvalue.Name = "chkwithsalvalue"
        Me.chkwithsalvalue.Size = New System.Drawing.Size(104, 17)
        Me.chkwithsalvalue.TabIndex = 24
        Me.chkwithsalvalue.Text = "With Salvalue"
        Me.chkwithsalvalue.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(27, 203)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(97, 21)
        Me.Label10.TabIndex = 14
        Me.Label10.Text = "Range"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(27, 226)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(71, 21)
        Me.Label9.TabIndex = 18
        Me.Label9.Text = "Display"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rbthorizontal)
        Me.Panel2.Controls.Add(Me.rbtvertical)
        Me.Panel2.Location = New System.Drawing.Point(128, 229)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(217, 20)
        Me.Panel2.TabIndex = 19
        '
        'rbthorizontal
        '
        Me.rbthorizontal.AutoSize = True
        Me.rbthorizontal.Checked = True
        Me.rbthorizontal.Location = New System.Drawing.Point(4, 2)
        Me.rbthorizontal.Name = "rbthorizontal"
        Me.rbthorizontal.Size = New System.Drawing.Size(82, 17)
        Me.rbthorizontal.TabIndex = 0
        Me.rbthorizontal.TabStop = True
        Me.rbthorizontal.Text = "Horizontal"
        Me.rbthorizontal.UseVisualStyleBackColor = True
        '
        'rbtvertical
        '
        Me.rbtvertical.AutoSize = True
        Me.rbtvertical.Location = New System.Drawing.Point(88, 2)
        Me.rbtvertical.Name = "rbtvertical"
        Me.rbtvertical.Size = New System.Drawing.Size(67, 17)
        Me.rbtvertical.TabIndex = 1
        Me.rbtvertical.TabStop = True
        Me.rbtvertical.Text = "Vertical"
        Me.rbtvertical.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(27, 155)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(97, 21)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Subitem Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(27, 253)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(71, 21)
        Me.Label8.TabIndex = 20
        Me.Label8.Text = "Based On"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Itemname
        '
        Me.Itemname.Location = New System.Drawing.Point(27, 129)
        Me.Itemname.Name = "Itemname"
        Me.Itemname.Size = New System.Drawing.Size(79, 21)
        Me.Itemname.TabIndex = 8
        Me.Itemname.Text = "Item Name"
        Me.Itemname.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.rbtitem)
        Me.Panel5.Controls.Add(Me.rbtdesigner)
        Me.Panel5.Location = New System.Drawing.Point(128, 254)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(217, 22)
        Me.Panel5.TabIndex = 21
        '
        'rbtitem
        '
        Me.rbtitem.AutoSize = True
        Me.rbtitem.Checked = True
        Me.rbtitem.Location = New System.Drawing.Point(4, 2)
        Me.rbtitem.Name = "rbtitem"
        Me.rbtitem.Size = New System.Drawing.Size(77, 17)
        Me.rbtitem.TabIndex = 0
        Me.rbtitem.TabStop = True
        Me.rbtitem.Text = "Itemwise"
        Me.rbtitem.UseVisualStyleBackColor = True
        '
        'rbtdesigner
        '
        Me.rbtdesigner.AutoSize = True
        Me.rbtdesigner.Location = New System.Drawing.Point(86, 2)
        Me.rbtdesigner.Name = "rbtdesigner"
        Me.rbtdesigner.Size = New System.Drawing.Size(101, 17)
        Me.rbtdesigner.TabIndex = 1
        Me.rbtdesigner.TabStop = True
        Me.rbtdesigner.Text = "Designerwise"
        Me.rbtdesigner.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(27, 79)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(79, 21)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Company"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(27, 180)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(97, 21)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "Designer Name"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(8, 244)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(73, 21)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Metal"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label4.Visible = False
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(8, 275)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(83, 21)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "Stock"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label6.Visible = False
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.rbtboth)
        Me.Panel4.Controls.Add(Me.rbtNontag)
        Me.Panel4.Controls.Add(Me.rbttag)
        Me.Panel4.Location = New System.Drawing.Point(97, 271)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(218, 25)
        Me.Panel4.TabIndex = 4
        Me.Panel4.Visible = False
        '
        'rbtboth
        '
        Me.rbtboth.AutoSize = True
        Me.rbtboth.Location = New System.Drawing.Point(3, 1)
        Me.rbtboth.Name = "rbtboth"
        Me.rbtboth.Size = New System.Drawing.Size(51, 17)
        Me.rbtboth.TabIndex = 0
        Me.rbtboth.Text = "Both"
        Me.rbtboth.UseVisualStyleBackColor = True
        '
        'rbtNontag
        '
        Me.rbtNontag.AutoSize = True
        Me.rbtNontag.Location = New System.Drawing.Point(112, 3)
        Me.rbtNontag.Name = "rbtNontag"
        Me.rbtNontag.Size = New System.Drawing.Size(71, 17)
        Me.rbtNontag.TabIndex = 2
        Me.rbtNontag.Text = "Non Tag"
        Me.rbtNontag.UseVisualStyleBackColor = True
        '
        'rbttag
        '
        Me.rbttag.AutoSize = True
        Me.rbttag.Checked = True
        Me.rbttag.Location = New System.Drawing.Point(60, 2)
        Me.rbttag.Name = "rbttag"
        Me.rbttag.Size = New System.Drawing.Size(45, 17)
        Me.rbttag.TabIndex = 1
        Me.rbttag.TabStop = True
        Me.rbttag.Text = "Tag"
        Me.rbttag.UseVisualStyleBackColor = True
        '
        'chkwithcum
        '
        Me.chkwithcum.AutoSize = True
        Me.chkwithcum.Location = New System.Drawing.Point(97, 302)
        Me.chkwithcum.Name = "chkwithcum"
        Me.chkwithcum.Size = New System.Drawing.Size(120, 17)
        Me.chkwithcum.TabIndex = 24
        Me.chkwithcum.Text = "With Cumulative"
        Me.chkwithcum.UseVisualStyleBackColor = True
        Me.chkwithcum.Visible = False
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Panel6)
        Me.TabPage2.Controls.Add(Me.Panel3)
        Me.TabPage2.Location = New System.Drawing.Point(4, 25)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(1139, 647)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "TabPage2"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'Panel6
        '
        Me.Panel6.Controls.Add(Me.GroupBox2)
        Me.Panel6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel6.Location = New System.Drawing.Point(3, 31)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(1133, 613)
        Me.Panel6.TabIndex = 2
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Panel8)
        Me.GroupBox2.Controls.Add(Me.GroupBox3)
        Me.GroupBox2.Controls.Add(Me.Panel7)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(1133, 613)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        '
        'Panel8
        '
        Me.Panel8.Controls.Add(Me.gridview)
        Me.Panel8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel8.Location = New System.Drawing.Point(3, 42)
        Me.Panel8.Name = "Panel8"
        Me.Panel8.Size = New System.Drawing.Size(1127, 523)
        Me.Panel8.TabIndex = 28
        '
        'gridview
        '
        Me.gridview.AllowUserToAddRows = False
        Me.gridview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridview.Location = New System.Drawing.Point(0, 0)
        Me.gridview.Name = "gridview"
        Me.gridview.ReadOnly = True
        Me.gridview.RowHeadersVisible = False
        Me.gridview.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridview.Size = New System.Drawing.Size(1127, 523)
        Me.gridview.TabIndex = 2
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.btnback)
        Me.GroupBox3.Controls.Add(Me.btnExport)
        Me.GroupBox3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.GroupBox3.Location = New System.Drawing.Point(3, 565)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(1127, 45)
        Me.GroupBox3.TabIndex = 27
        Me.GroupBox3.TabStop = False
        '
        'btnback
        '
        Me.btnback.Location = New System.Drawing.Point(780, 11)
        Me.btnback.Name = "btnback"
        Me.btnback.Size = New System.Drawing.Size(100, 30)
        Me.btnback.TabIndex = 25
        Me.btnback.Text = "Back[Escape]"
        Me.btnback.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(886, 11)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 26
        Me.btnExport.Text = "Export[x]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'Panel7
        '
        Me.Panel7.Controls.Add(Me.gridheader1)
        Me.Panel7.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel7.Location = New System.Drawing.Point(3, 17)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(1127, 25)
        Me.Panel7.TabIndex = 1
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.lblTitle)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(3, 3)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1133, 28)
        Me.Panel3.TabIndex = 1
        '
        'chkcmbrange
        '
        Me.chkcmbrange.CheckOnClick = True
        Me.chkcmbrange.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbrange.DropDownHeight = 1
        Me.chkcmbrange.FormattingEnabled = True
        Me.chkcmbrange.IntegralHeight = False
        Me.chkcmbrange.Location = New System.Drawing.Point(127, 203)
        Me.chkcmbrange.Name = "chkcmbrange"
        Me.chkcmbrange.Size = New System.Drawing.Size(218, 22)
        Me.chkcmbrange.TabIndex = 15
        Me.chkcmbrange.ValueSeparator = ", "
        '
        'chkcmbsutitem
        '
        Me.chkcmbsutitem.CheckOnClick = True
        Me.chkcmbsutitem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbsutitem.DropDownHeight = 1
        Me.chkcmbsutitem.FormattingEnabled = True
        Me.chkcmbsutitem.IntegralHeight = False
        Me.chkcmbsutitem.Location = New System.Drawing.Point(127, 153)
        Me.chkcmbsutitem.Name = "chkcmbsutitem"
        Me.chkcmbsutitem.Size = New System.Drawing.Size(218, 22)
        Me.chkcmbsutitem.TabIndex = 11
        Me.chkcmbsutitem.ValueSeparator = ", "
        '
        'chkcmbitem
        '
        Me.chkcmbitem.CheckOnClick = True
        Me.chkcmbitem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbitem.DropDownHeight = 1
        Me.chkcmbitem.FormattingEnabled = True
        Me.chkcmbitem.IntegralHeight = False
        Me.chkcmbitem.Location = New System.Drawing.Point(353, 128)
        Me.chkcmbitem.Name = "chkcmbitem"
        Me.chkcmbitem.Size = New System.Drawing.Size(218, 22)
        Me.chkcmbitem.TabIndex = 9
        Me.chkcmbitem.ValueSeparator = ", "
        Me.chkcmbitem.Visible = False
        '
        'chkcmbcompany
        '
        Me.chkcmbcompany.CheckOnClick = True
        Me.chkcmbcompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbcompany.DropDownHeight = 1
        Me.chkcmbcompany.FormattingEnabled = True
        Me.chkcmbcompany.IntegralHeight = False
        Me.chkcmbcompany.Location = New System.Drawing.Point(127, 78)
        Me.chkcmbcompany.Name = "chkcmbcompany"
        Me.chkcmbcompany.Size = New System.Drawing.Size(218, 22)
        Me.chkcmbcompany.TabIndex = 5
        Me.chkcmbcompany.ValueSeparator = ", "
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(353, 103)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(218, 22)
        Me.chkCmbCostCentre.TabIndex = 7
        Me.chkCmbCostCentre.ValueSeparator = ", "
        Me.chkCmbCostCentre.Visible = False
        '
        'chkcmbdesigner
        '
        Me.chkcmbdesigner.CheckOnClick = True
        Me.chkcmbdesigner.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbdesigner.DropDownHeight = 1
        Me.chkcmbdesigner.FormattingEnabled = True
        Me.chkcmbdesigner.IntegralHeight = False
        Me.chkcmbdesigner.Location = New System.Drawing.Point(127, 178)
        Me.chkcmbdesigner.Name = "chkcmbdesigner"
        Me.chkcmbdesigner.Size = New System.Drawing.Size(218, 22)
        Me.chkcmbdesigner.TabIndex = 13
        Me.chkcmbdesigner.ValueSeparator = ", "
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(127, 54)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(96, 21)
        Me.dtpFrom.TabIndex = 3
        Me.dtpFrom.Text = "01/01/9998"
        Me.dtpFrom.Value = New Date(9998, 1, 1, 0, 0, 0, 0)
        '
        'chkcmbmetal
        '
        Me.chkcmbmetal.CheckOnClick = True
        Me.chkcmbmetal.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbmetal.DropDownHeight = 1
        Me.chkcmbmetal.FormattingEnabled = True
        Me.chkcmbmetal.IntegralHeight = False
        Me.chkcmbmetal.Location = New System.Drawing.Point(97, 243)
        Me.chkcmbmetal.Name = "chkcmbmetal"
        Me.chkcmbmetal.Size = New System.Drawing.Size(218, 22)
        Me.chkcmbmetal.TabIndex = 2
        Me.chkcmbmetal.ValueSeparator = ", "
        Me.chkcmbmetal.Visible = False
        '
        'frmReorderWeightBased
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1147, 676)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmReorderWeightBased"
        Me.Text = "REORDER STOCK"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.gridheader1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.PanelRange.ResumeLayout(False)
        Me.PanelRange.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.Panel6.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.Panel8.ResumeLayout(False)
        CType(Me.gridview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.Panel7.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExtToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents gridheader1 As System.Windows.Forms.DataGridView
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkcmbmetal As BrighttechPack.CheckedComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents chkcmbitem As BrighttechPack.CheckedComboBox
    Friend WithEvents Itemname As System.Windows.Forms.Label
    Friend WithEvents chkcmbcompany As BrighttechPack.CheckedComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents rbttag As System.Windows.Forms.RadioButton
    Friend WithEvents rbtNontag As System.Windows.Forms.RadioButton
    Friend WithEvents rbtboth As System.Windows.Forms.RadioButton
    Friend WithEvents chkwithcum As System.Windows.Forms.CheckBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents chkcmbdesigner As BrighttechPack.CheckedComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents rbtitem As System.Windows.Forms.RadioButton
    Friend WithEvents rbtdesigner As System.Windows.Forms.RadioButton
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkcmbsutitem As BrighttechPack.CheckedComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rbthorizontal As System.Windows.Forms.RadioButton
    Friend WithEvents rbtvertical As System.Windows.Forms.RadioButton
    Friend WithEvents chkcmbrange As BrighttechPack.CheckedComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Panel7 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents gridview As System.Windows.Forms.DataGridView
    Friend WithEvents btnback As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Panel8 As System.Windows.Forms.Panel
    Friend WithEvents chkwithsalvalue As System.Windows.Forms.CheckBox
    Friend WithEvents Label11 As Label
    Friend WithEvents cmbItemName_OWN As ComboBox
    Friend WithEvents cmbCostCentre_OWN As ComboBox
    Friend WithEvents chkwithoutStockOnly As CheckBox
    Friend WithEvents chkEmptyStock As CheckBox
    Friend WithEvents chkWithCaption As CheckBox
    Friend WithEvents lblRangeType As Label
    Friend WithEvents PanelRange As Panel
    Friend WithEvents rbtRangeAll As RadioButton
    Friend WithEvents rbtRangeIn As RadioButton
    Friend WithEvents rbtRangeOut As RadioButton
End Class
