<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMetalOrnamentDetailedStockView_New
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
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.grpContainer = New System.Windows.Forms.GroupBox
        Me.ChkPresious = New System.Windows.Forms.CheckBox
        Me.ChkStone = New System.Windows.Forms.CheckBox
        Me.ChkDia = New System.Windows.Forms.CheckBox
        Me.chkWithAlloy = New System.Windows.Forms.CheckBox
        Me.chkCategorywise = New System.Windows.Forms.CheckBox
        Me.chkOrderbyTranno = New System.Windows.Forms.CheckBox
        Me.chkCmbTransation = New BrighttechPack.CheckedComboBox
        Me.chkCmbItem = New BrighttechPack.CheckedComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.chkGs11 = New System.Windows.Forms.CheckBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.rbtBoth = New System.Windows.Forms.RadioButton
        Me.rbtWeight = New System.Windows.Forms.RadioButton
        Me.rbtPcs = New System.Windows.Forms.RadioButton
        Me.chkGs12 = New System.Windows.Forms.CheckBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.rdbBothwt = New System.Windows.Forms.RadioButton
        Me.rbtNetWt = New System.Windows.Forms.RadioButton
        Me.rbtGrsWeight = New System.Windows.Forms.RadioButton
        Me.Label5 = New System.Windows.Forms.Label
        Me.chkOthers = New System.Windows.Forms.CheckBox
        Me.cmbCategoryGroup = New System.Windows.Forms.ComboBox
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.chkLstCategory = New System.Windows.Forms.CheckedListBox
        Me.rbtTrandate = New System.Windows.Forms.RadioButton
        Me.rbtTranno = New System.Windows.Forms.RadioButton
        Me.chkCategorySelectAll = New System.Windows.Forms.CheckBox
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox
        Me.chkMetalSelectAll = New System.Windows.Forms.CheckBox
        Me.chkLstMetal = New System.Windows.Forms.CheckedListBox
        Me.chkCostCentreSelectAll = New System.Windows.Forms.CheckBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.Panelbody = New System.Windows.Forms.Panel
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.GridView = New System.Windows.Forms.DataGridView
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.GridViewHead = New System.Windows.Forms.DataGridView
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.btnPrint = New System.Windows.Forms.Button
        Me.BtnBack = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.Paneltitle = New System.Windows.Forms.Panel
        Me.lblTitle = New System.Windows.Forms.Label
        Me.lblTitleHead = New System.Windows.Forms.Label
        Me.ContextMenuStrip1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.grpContainer.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.Panelbody.SuspendLayout()
        Me.Panel4.SuspendLayout()
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmbGridShortCut.SuspendLayout()
        CType(Me.GridViewHead, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.Paneltitle.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
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
        Me.TabControl1.Size = New System.Drawing.Size(917, 608)
        Me.TabControl1.TabIndex = 1
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.grpContainer)
        Me.TabPage1.Location = New System.Drawing.Point(4, 25)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(909, 579)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "General"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'grpContainer
        '
        Me.grpContainer.Controls.Add(Me.ChkPresious)
        Me.grpContainer.Controls.Add(Me.ChkStone)
        Me.grpContainer.Controls.Add(Me.ChkDia)
        Me.grpContainer.Controls.Add(Me.chkWithAlloy)
        Me.grpContainer.Controls.Add(Me.chkCategorywise)
        Me.grpContainer.Controls.Add(Me.chkOrderbyTranno)
        Me.grpContainer.Controls.Add(Me.chkCmbTransation)
        Me.grpContainer.Controls.Add(Me.chkCmbItem)
        Me.grpContainer.Controls.Add(Me.Label9)
        Me.grpContainer.Controls.Add(Me.Label8)
        Me.grpContainer.Controls.Add(Me.chkGs11)
        Me.grpContainer.Controls.Add(Me.Panel2)
        Me.grpContainer.Controls.Add(Me.chkGs12)
        Me.grpContainer.Controls.Add(Me.Panel1)
        Me.grpContainer.Controls.Add(Me.chkOthers)
        Me.grpContainer.Controls.Add(Me.cmbCategoryGroup)
        Me.grpContainer.Controls.Add(Me.dtpTo)
        Me.grpContainer.Controls.Add(Me.dtpFrom)
        Me.grpContainer.Controls.Add(Me.chkLstCategory)
        Me.grpContainer.Controls.Add(Me.rbtTrandate)
        Me.grpContainer.Controls.Add(Me.rbtTranno)
        Me.grpContainer.Controls.Add(Me.chkCategorySelectAll)
        Me.grpContainer.Controls.Add(Me.chkLstCostCentre)
        Me.grpContainer.Controls.Add(Me.chkMetalSelectAll)
        Me.grpContainer.Controls.Add(Me.chkLstMetal)
        Me.grpContainer.Controls.Add(Me.chkCostCentreSelectAll)
        Me.grpContainer.Controls.Add(Me.Label2)
        Me.grpContainer.Controls.Add(Me.Label3)
        Me.grpContainer.Controls.Add(Me.Label4)
        Me.grpContainer.Controls.Add(Me.Label7)
        Me.grpContainer.Controls.Add(Me.Label6)
        Me.grpContainer.Controls.Add(Me.Label1)
        Me.grpContainer.Controls.Add(Me.btnSearch)
        Me.grpContainer.Controls.Add(Me.btnExit)
        Me.grpContainer.Controls.Add(Me.btnNew)
        Me.grpContainer.Location = New System.Drawing.Point(282, 8)
        Me.grpContainer.Name = "grpContainer"
        Me.grpContainer.Size = New System.Drawing.Size(444, 544)
        Me.grpContainer.TabIndex = 1
        Me.grpContainer.TabStop = False
        '
        'ChkPresious
        '
        Me.ChkPresious.AutoSize = True
        Me.ChkPresious.Location = New System.Drawing.Point(266, 445)
        Me.ChkPresious.Name = "ChkPresious"
        Me.ChkPresious.Size = New System.Drawing.Size(74, 17)
        Me.ChkPresious.TabIndex = 36
        Me.ChkPresious.Text = "Presious"
        Me.ChkPresious.UseVisualStyleBackColor = True
        '
        'ChkStone
        '
        Me.ChkStone.AutoSize = True
        Me.ChkStone.Location = New System.Drawing.Point(114, 445)
        Me.ChkStone.Name = "ChkStone"
        Me.ChkStone.Size = New System.Drawing.Size(59, 17)
        Me.ChkStone.TabIndex = 35
        Me.ChkStone.Text = "Stone"
        Me.ChkStone.UseVisualStyleBackColor = True
        '
        'ChkDia
        '
        Me.ChkDia.AutoSize = True
        Me.ChkDia.Location = New System.Drawing.Point(183, 445)
        Me.ChkDia.Name = "ChkDia"
        Me.ChkDia.Size = New System.Drawing.Size(77, 17)
        Me.ChkDia.TabIndex = 34
        Me.ChkDia.Text = "Diamond"
        Me.ChkDia.UseVisualStyleBackColor = True
        '
        'chkWithAlloy
        '
        Me.chkWithAlloy.AutoSize = True
        Me.chkWithAlloy.Location = New System.Drawing.Point(221, 468)
        Me.chkWithAlloy.Name = "chkWithAlloy"
        Me.chkWithAlloy.Size = New System.Drawing.Size(83, 17)
        Me.chkWithAlloy.TabIndex = 33
        Me.chkWithAlloy.Text = "With Alloy"
        Me.chkWithAlloy.UseVisualStyleBackColor = True
        '
        'chkCategorywise
        '
        Me.chkCategorywise.AutoSize = True
        Me.chkCategorywise.Location = New System.Drawing.Point(114, 468)
        Me.chkCategorywise.Name = "chkCategorywise"
        Me.chkCategorywise.Size = New System.Drawing.Size(106, 17)
        Me.chkCategorywise.TabIndex = 32
        Me.chkCategorywise.Text = "CategoryWise"
        Me.chkCategorywise.UseVisualStyleBackColor = True
        '
        'chkOrderbyTranno
        '
        Me.chkOrderbyTranno.AutoSize = True
        Me.chkOrderbyTranno.Location = New System.Drawing.Point(294, 386)
        Me.chkOrderbyTranno.Name = "chkOrderbyTranno"
        Me.chkOrderbyTranno.Size = New System.Drawing.Size(122, 17)
        Me.chkOrderbyTranno.TabIndex = 26
        Me.chkOrderbyTranno.Text = "Order by TranNo"
        Me.chkOrderbyTranno.UseVisualStyleBackColor = True
        '
        'chkCmbTransation
        '
        Me.chkCmbTransation.CheckOnClick = True
        Me.chkCmbTransation.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbTransation.DropDownHeight = 1
        Me.chkCmbTransation.FormattingEnabled = True
        Me.chkCmbTransation.IntegralHeight = False
        Me.chkCmbTransation.Location = New System.Drawing.Point(117, 357)
        Me.chkCmbTransation.Name = "chkCmbTransation"
        Me.chkCmbTransation.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbTransation.TabIndex = 20
        Me.chkCmbTransation.ValueSeparator = ", "
        '
        'chkCmbItem
        '
        Me.chkCmbItem.CheckOnClick = True
        Me.chkCmbItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbItem.DropDownHeight = 1
        Me.chkCmbItem.FormattingEnabled = True
        Me.chkCmbItem.IntegralHeight = False
        Me.chkCmbItem.Location = New System.Drawing.Point(117, 332)
        Me.chkCmbItem.Name = "chkCmbItem"
        Me.chkCmbItem.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbItem.TabIndex = 19
        Me.chkCmbItem.ValueSeparator = ", "
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(8, 366)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(73, 13)
        Me.Label9.TabIndex = 18
        Me.Label9.Text = "Transaction"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(8, 338)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(67, 13)
        Me.Label8.TabIndex = 18
        Me.Label8.Text = "ItemName"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkGs11
        '
        Me.chkGs11.AutoSize = True
        Me.chkGs11.Location = New System.Drawing.Point(118, 171)
        Me.chkGs11.Name = "chkGs11"
        Me.chkGs11.Size = New System.Drawing.Size(57, 17)
        Me.chkGs11.TabIndex = 11
        Me.chkGs11.Text = "GS11"
        Me.chkGs11.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rbtBoth)
        Me.Panel2.Controls.Add(Me.rbtWeight)
        Me.Panel2.Controls.Add(Me.rbtPcs)
        Me.Panel2.Location = New System.Drawing.Point(118, 137)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(186, 26)
        Me.Panel2.TabIndex = 9
        '
        'rbtBoth
        '
        Me.rbtBoth.AutoSize = True
        Me.rbtBoth.Location = New System.Drawing.Point(123, 4)
        Me.rbtBoth.Name = "rbtBoth"
        Me.rbtBoth.Size = New System.Drawing.Size(51, 17)
        Me.rbtBoth.TabIndex = 2
        Me.rbtBoth.TabStop = True
        Me.rbtBoth.Text = "Both"
        Me.rbtBoth.UseVisualStyleBackColor = True
        '
        'rbtWeight
        '
        Me.rbtWeight.AutoSize = True
        Me.rbtWeight.Location = New System.Drawing.Point(4, 3)
        Me.rbtWeight.Name = "rbtWeight"
        Me.rbtWeight.Size = New System.Drawing.Size(64, 17)
        Me.rbtWeight.TabIndex = 0
        Me.rbtWeight.TabStop = True
        Me.rbtWeight.Text = "Weight"
        Me.rbtWeight.UseVisualStyleBackColor = True
        '
        'rbtPcs
        '
        Me.rbtPcs.AutoSize = True
        Me.rbtPcs.Location = New System.Drawing.Point(76, 3)
        Me.rbtPcs.Name = "rbtPcs"
        Me.rbtPcs.Size = New System.Drawing.Size(44, 17)
        Me.rbtPcs.TabIndex = 1
        Me.rbtPcs.TabStop = True
        Me.rbtPcs.Text = "Pcs"
        Me.rbtPcs.UseVisualStyleBackColor = True
        '
        'chkGs12
        '
        Me.chkGs12.AutoSize = True
        Me.chkGs12.Location = New System.Drawing.Point(235, 171)
        Me.chkGs12.Name = "chkGs12"
        Me.chkGs12.Size = New System.Drawing.Size(57, 17)
        Me.chkGs12.TabIndex = 12
        Me.chkGs12.Text = "GS12"
        Me.chkGs12.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rdbBothwt)
        Me.Panel1.Controls.Add(Me.rbtNetWt)
        Me.Panel1.Controls.Add(Me.rbtGrsWeight)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Location = New System.Drawing.Point(4, 410)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(299, 29)
        Me.Panel1.TabIndex = 25
        '
        'rdbBothwt
        '
        Me.rdbBothwt.AutoSize = True
        Me.rdbBothwt.Checked = True
        Me.rdbBothwt.Location = New System.Drawing.Point(240, 9)
        Me.rdbBothwt.Name = "rdbBothwt"
        Me.rdbBothwt.Size = New System.Drawing.Size(51, 17)
        Me.rdbBothwt.TabIndex = 3
        Me.rdbBothwt.TabStop = True
        Me.rdbBothwt.Text = "Both"
        Me.rdbBothwt.UseVisualStyleBackColor = True
        '
        'rbtNetWt
        '
        Me.rbtNetWt.AutoSize = True
        Me.rbtNetWt.Location = New System.Drawing.Point(178, 9)
        Me.rbtNetWt.Name = "rbtNetWt"
        Me.rbtNetWt.Size = New System.Drawing.Size(59, 17)
        Me.rbtNetWt.TabIndex = 2
        Me.rbtNetWt.Text = "NetWt"
        Me.rbtNetWt.UseVisualStyleBackColor = True
        '
        'rbtGrsWeight
        '
        Me.rbtGrsWeight.AutoSize = True
        Me.rbtGrsWeight.Location = New System.Drawing.Point(112, 9)
        Me.rbtGrsWeight.Name = "rbtGrsWeight"
        Me.rbtGrsWeight.Size = New System.Drawing.Size(60, 17)
        Me.rbtGrsWeight.TabIndex = 1
        Me.rbtGrsWeight.Text = "GrsWt"
        Me.rbtGrsWeight.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(3, 8)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(78, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Wt based on"
        '
        'chkOthers
        '
        Me.chkOthers.AutoSize = True
        Me.chkOthers.Location = New System.Drawing.Point(344, 171)
        Me.chkOthers.Name = "chkOthers"
        Me.chkOthers.Size = New System.Drawing.Size(64, 17)
        Me.chkOthers.TabIndex = 13
        Me.chkOthers.Text = "Others"
        Me.chkOthers.UseVisualStyleBackColor = True
        '
        'cmbCategoryGroup
        '
        Me.cmbCategoryGroup.FormattingEnabled = True
        Me.cmbCategoryGroup.Location = New System.Drawing.Point(118, 195)
        Me.cmbCategoryGroup.Name = "cmbCategoryGroup"
        Me.cmbCategoryGroup.Size = New System.Drawing.Size(298, 21)
        Me.cmbCategoryGroup.TabIndex = 15
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(214, 15)
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
        Me.dtpFrom.Location = New System.Drawing.Point(80, 15)
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
        'chkLstCategory
        '
        Me.chkLstCategory.FormattingEnabled = True
        Me.chkLstCategory.Location = New System.Drawing.Point(8, 244)
        Me.chkLstCategory.Name = "chkLstCategory"
        Me.chkLstCategory.Size = New System.Drawing.Size(407, 84)
        Me.chkLstCategory.TabIndex = 17
        '
        'rbtTrandate
        '
        Me.rbtTrandate.AutoSize = True
        Me.rbtTrandate.Location = New System.Drawing.Point(116, 386)
        Me.rbtTrandate.Name = "rbtTrandate"
        Me.rbtTrandate.Size = New System.Drawing.Size(76, 17)
        Me.rbtTrandate.TabIndex = 21
        Me.rbtTrandate.TabStop = True
        Me.rbtTrandate.Text = "Trandate"
        Me.rbtTrandate.UseVisualStyleBackColor = True
        '
        'rbtTranno
        '
        Me.rbtTranno.AutoSize = True
        Me.rbtTranno.Location = New System.Drawing.Point(198, 386)
        Me.rbtTranno.Name = "rbtTranno"
        Me.rbtTranno.Size = New System.Drawing.Size(65, 17)
        Me.rbtTranno.TabIndex = 22
        Me.rbtTranno.TabStop = True
        Me.rbtTranno.Text = "Tranno"
        Me.rbtTranno.UseVisualStyleBackColor = True
        '
        'chkCategorySelectAll
        '
        Me.chkCategorySelectAll.AutoSize = True
        Me.chkCategorySelectAll.Location = New System.Drawing.Point(11, 227)
        Me.chkCategorySelectAll.Name = "chkCategorySelectAll"
        Me.chkCategorySelectAll.Size = New System.Drawing.Size(79, 17)
        Me.chkCategorySelectAll.TabIndex = 16
        Me.chkCategorySelectAll.Text = "Category"
        Me.chkCategorySelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(9, 59)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(201, 68)
        Me.chkLstCostCentre.TabIndex = 5
        '
        'chkMetalSelectAll
        '
        Me.chkMetalSelectAll.AutoSize = True
        Me.chkMetalSelectAll.Location = New System.Drawing.Point(220, 40)
        Me.chkMetalSelectAll.Name = "chkMetalSelectAll"
        Me.chkMetalSelectAll.Size = New System.Drawing.Size(56, 17)
        Me.chkMetalSelectAll.TabIndex = 6
        Me.chkMetalSelectAll.Text = "Metal"
        Me.chkMetalSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstMetal
        '
        Me.chkLstMetal.FormattingEnabled = True
        Me.chkLstMetal.Location = New System.Drawing.Point(217, 59)
        Me.chkLstMetal.Name = "chkLstMetal"
        Me.chkLstMetal.Size = New System.Drawing.Size(199, 68)
        Me.chkLstMetal.TabIndex = 7
        '
        'chkCostCentreSelectAll
        '
        Me.chkCostCentreSelectAll.AutoSize = True
        Me.chkCostCentreSelectAll.Location = New System.Drawing.Point(12, 40)
        Me.chkCostCentreSelectAll.Name = "chkCostCentreSelectAll"
        Me.chkCostCentreSelectAll.Size = New System.Drawing.Size(95, 17)
        Me.chkCostCentreSelectAll.TabIndex = 4
        Me.chkCostCentreSelectAll.Text = "Cost Centre"
        Me.chkCostCentreSelectAll.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(183, 18)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(21, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 388)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 13)
        Me.Label3.TabIndex = 20
        Me.Label3.Text = "Group By"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(9, 195)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(99, 13)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Category Group"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(9, 173)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(68, 13)
        Me.Label7.TabIndex = 10
        Me.Label7.Text = "Stock type"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(9, 141)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(49, 13)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "Display"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date From"
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(110, 491)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 27
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(322, 491)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 29
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(216, 491)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 28
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Panelbody)
        Me.TabPage2.Controls.Add(Me.Paneltitle)
        Me.TabPage2.Location = New System.Drawing.Point(4, 25)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(909, 579)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "View"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'Panelbody
        '
        Me.Panelbody.Controls.Add(Me.Panel4)
        Me.Panelbody.Controls.Add(Me.GridViewHead)
        Me.Panelbody.Controls.Add(Me.Panel3)
        Me.Panelbody.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panelbody.Location = New System.Drawing.Point(3, 65)
        Me.Panelbody.Name = "Panelbody"
        Me.Panelbody.Size = New System.Drawing.Size(903, 511)
        Me.Panelbody.TabIndex = 5
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.GridView)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel4.Location = New System.Drawing.Point(0, 16)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(903, 444)
        Me.Panel4.TabIndex = 4
        '
        'GridView
        '
        Me.GridView.AllowUserToAddRows = False
        Me.GridView.AllowUserToDeleteRows = False
        Me.GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridView.ContextMenuStrip = Me.cmbGridShortCut
        Me.GridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridView.Location = New System.Drawing.Point(0, 0)
        Me.GridView.Name = "GridView"
        Me.GridView.ReadOnly = True
        Me.GridView.Size = New System.Drawing.Size(903, 444)
        Me.GridView.TabIndex = 0
        '
        'cmbGridShortCut
        '
        Me.cmbGridShortCut.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResizeToolStripMenuItem})
        Me.cmbGridShortCut.Name = "ContextMenuStrip2"
        Me.cmbGridShortCut.Size = New System.Drawing.Size(143, 26)
        '
        'ResizeToolStripMenuItem
        '
        Me.ResizeToolStripMenuItem.CheckOnClick = True
        Me.ResizeToolStripMenuItem.Name = "ResizeToolStripMenuItem"
        Me.ResizeToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.ResizeToolStripMenuItem.Text = "Auto Resize"
        '
        'GridViewHead
        '
        Me.GridViewHead.AllowUserToDeleteRows = False
        Me.GridViewHead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridViewHead.Dock = System.Windows.Forms.DockStyle.Top
        Me.GridViewHead.Location = New System.Drawing.Point(0, 0)
        Me.GridViewHead.Name = "GridViewHead"
        Me.GridViewHead.ReadOnly = True
        Me.GridViewHead.Size = New System.Drawing.Size(903, 16)
        Me.GridViewHead.TabIndex = 1
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.btnPrint)
        Me.Panel3.Controls.Add(Me.BtnBack)
        Me.Panel3.Controls.Add(Me.btnExport)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(0, 460)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(903, 51)
        Me.Panel3.TabIndex = 3
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(618, 17)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 31
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'BtnBack
        '
        Me.BtnBack.Location = New System.Drawing.Point(424, 18)
        Me.BtnBack.Name = "BtnBack"
        Me.BtnBack.Size = New System.Drawing.Size(82, 30)
        Me.BtnBack.TabIndex = 0
        Me.BtnBack.Text = "Back [ESC]"
        Me.BtnBack.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(512, 17)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 30
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'Paneltitle
        '
        Me.Paneltitle.Controls.Add(Me.lblTitle)
        Me.Paneltitle.Controls.Add(Me.lblTitleHead)
        Me.Paneltitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.Paneltitle.Location = New System.Drawing.Point(3, 3)
        Me.Paneltitle.Name = "Paneltitle"
        Me.Paneltitle.Size = New System.Drawing.Size(903, 62)
        Me.Paneltitle.TabIndex = 4
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 35)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(903, 27)
        Me.lblTitle.TabIndex = 1
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTitleHead
        '
        Me.lblTitleHead.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitleHead.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitleHead.Location = New System.Drawing.Point(0, 0)
        Me.lblTitleHead.Name = "lblTitleHead"
        Me.lblTitleHead.Size = New System.Drawing.Size(903, 35)
        Me.lblTitleHead.TabIndex = 2
        Me.lblTitleHead.Text = "Title"
        Me.lblTitleHead.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmMetalOrnamentDetailedStockView_New
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(917, 608)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.TabControl1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmMetalOrnamentDetailedStockView_New"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Metal Ornament Stock View (Detailed)"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.grpContainer.ResumeLayout(False)
        Me.grpContainer.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.Panelbody.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmbGridShortCut.ResumeLayout(False)
        CType(Me.GridViewHead, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Paneltitle.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents grpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents ChkPresious As System.Windows.Forms.CheckBox
    Friend WithEvents ChkStone As System.Windows.Forms.CheckBox
    Friend WithEvents ChkDia As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithAlloy As System.Windows.Forms.CheckBox
    Friend WithEvents chkCategorywise As System.Windows.Forms.CheckBox
    Friend WithEvents chkOrderbyTranno As System.Windows.Forms.CheckBox
    Friend WithEvents chkCmbTransation As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbItem As BrighttechPack.CheckedComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents chkGs11 As System.Windows.Forms.CheckBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rbtBoth As System.Windows.Forms.RadioButton
    Friend WithEvents rbtWeight As System.Windows.Forms.RadioButton
    Friend WithEvents rbtPcs As System.Windows.Forms.RadioButton
    Friend WithEvents chkGs12 As System.Windows.Forms.CheckBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rdbBothwt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtNetWt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGrsWeight As System.Windows.Forms.RadioButton
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents chkOthers As System.Windows.Forms.CheckBox
    Friend WithEvents cmbCategoryGroup As System.Windows.Forms.ComboBox
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkLstCategory As System.Windows.Forms.CheckedListBox
    Friend WithEvents rbtTrandate As System.Windows.Forms.RadioButton
    Friend WithEvents rbtTranno As System.Windows.Forms.RadioButton
    Friend WithEvents chkCategorySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkMetalSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstMetal As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkCostCentreSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents BtnBack As System.Windows.Forms.Button
    Friend WithEvents Paneltitle As System.Windows.Forms.Panel
    Friend WithEvents lblTitleHead As System.Windows.Forms.Label
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents Panelbody As System.Windows.Forms.Panel
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GridView As System.Windows.Forms.DataGridView
    Friend WithEvents GridViewHead As System.Windows.Forms.DataGridView
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
End Class
