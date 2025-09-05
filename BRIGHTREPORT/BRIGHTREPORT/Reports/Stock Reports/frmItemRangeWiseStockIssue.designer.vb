<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmItemRangeWiseStockIssue
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
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabGen = New System.Windows.Forms.TabPage()
        Me.pnlGroupFilter = New System.Windows.Forms.Panel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkZero = New System.Windows.Forms.CheckBox()
        Me.ChkIssueOnly = New System.Windows.Forms.CheckBox()
        Me.ChkTransPrint = New System.Windows.Forms.CheckBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.rbtDispWt = New System.Windows.Forms.RadioButton()
        Me.rbtDispPcs = New System.Windows.Forms.RadioButton()
        Me.rbtDispBoth = New System.Windows.Forms.RadioButton()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.rbtSubItem = New System.Windows.Forms.RadioButton()
        Me.rbtItem = New System.Windows.Forms.RadioButton()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.chkRangeOut = New System.Windows.Forms.CheckBox()
        Me.chkcmbrange = New BrighttechPack.CheckedComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.ChkItemMode = New BrighttechPack.CheckedComboBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.chkCmbDesigner = New BrighttechPack.CheckedComboBox()
        Me.chkCmbItem = New BrighttechPack.CheckedComboBox()
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox()
        Me.chkCmbMetal = New BrighttechPack.CheckedComboBox()
        Me.chkCmbItemType = New BrighttechPack.CheckedComboBox()
        Me.chkCmbCounter = New BrighttechPack.CheckedComboBox()
        Me.chkCmbCompany = New BrighttechPack.CheckedComboBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.rbtNonTag = New System.Windows.Forms.RadioButton()
        Me.rbtTag = New System.Windows.Forms.RadioButton()
        Me.rbtBoth = New System.Windows.Forms.RadioButton()
        Me.cmbCategory = New System.Windows.Forms.ComboBox()
        Me.label10 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.tabView = New System.Windows.Forms.TabPage()
        Me.gridviewDetail = New System.Windows.Forms.DataGridView()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.gridViewHead = New System.Windows.Forms.DataGridView()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.pnlfooter = New System.Windows.Forms.Panel()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.BtnGenerate = New System.Windows.Forms.Button()
        Me.btnPost = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.tabMain.SuspendLayout()
        Me.tabGen.SuspendLayout()
        Me.pnlGroupFilter.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.tabView.SuspendLayout()
        CType(Me.gridviewDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmbGridShortCut.SuspendLayout()
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlfooter.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabMain
        '
        Me.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tabMain.Controls.Add(Me.tabGen)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(941, 622)
        Me.tabMain.TabIndex = 0
        '
        'tabGen
        '
        Me.tabGen.Controls.Add(Me.pnlGroupFilter)
        Me.tabGen.Location = New System.Drawing.Point(4, 29)
        Me.tabGen.Name = "tabGen"
        Me.tabGen.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGen.Size = New System.Drawing.Size(933, 589)
        Me.tabGen.TabIndex = 0
        Me.tabGen.Text = "Gen"
        Me.tabGen.UseVisualStyleBackColor = True
        '
        'pnlGroupFilter
        '
        Me.pnlGroupFilter.Controls.Add(Me.GroupBox1)
        Me.pnlGroupFilter.Location = New System.Drawing.Point(151, 79)
        Me.pnlGroupFilter.Name = "pnlGroupFilter"
        Me.pnlGroupFilter.Size = New System.Drawing.Size(619, 428)
        Me.pnlGroupFilter.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkZero)
        Me.GroupBox1.Controls.Add(Me.ChkIssueOnly)
        Me.GroupBox1.Controls.Add(Me.ChkTransPrint)
        Me.GroupBox1.Controls.Add(Me.Label15)
        Me.GroupBox1.Controls.Add(Me.Panel3)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.Panel1)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.chkRangeOut)
        Me.GroupBox1.Controls.Add(Me.chkcmbrange)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.btnNew)
        Me.GroupBox1.Controls.Add(Me.btnExit)
        Me.GroupBox1.Controls.Add(Me.btnView_Search)
        Me.GroupBox1.Controls.Add(Me.ChkItemMode)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.chkCmbDesigner)
        Me.GroupBox1.Controls.Add(Me.chkCmbItem)
        Me.GroupBox1.Controls.Add(Me.chkCmbCostCentre)
        Me.GroupBox1.Controls.Add(Me.chkCmbMetal)
        Me.GroupBox1.Controls.Add(Me.chkCmbItemType)
        Me.GroupBox1.Controls.Add(Me.chkCmbCounter)
        Me.GroupBox1.Controls.Add(Me.chkCmbCompany)
        Me.GroupBox1.Controls.Add(Me.dtpTo)
        Me.GroupBox1.Controls.Add(Me.dtpFrom)
        Me.GroupBox1.Controls.Add(Me.Panel2)
        Me.GroupBox1.Controls.Add(Me.cmbCategory)
        Me.GroupBox1.Controls.Add(Me.label10)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.lblTo)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Location = New System.Drawing.Point(10, 11)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(610, 407)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'chkZero
        '
        Me.chkZero.AutoSize = True
        Me.chkZero.Location = New System.Drawing.Point(493, 340)
        Me.chkZero.Name = "chkZero"
        Me.chkZero.Size = New System.Drawing.Size(119, 21)
        Me.chkZero.TabIndex = 35
        Me.chkZero.Text = "Zero Closing"
        Me.chkZero.UseVisualStyleBackColor = True
        '
        'ChkIssueOnly
        '
        Me.ChkIssueOnly.AutoSize = True
        Me.ChkIssueOnly.Checked = True
        Me.ChkIssueOnly.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkIssueOnly.Location = New System.Drawing.Point(392, 340)
        Me.ChkIssueOnly.Name = "ChkIssueOnly"
        Me.ChkIssueOnly.Size = New System.Drawing.Size(104, 21)
        Me.ChkIssueOnly.TabIndex = 35
        Me.ChkIssueOnly.Text = "Issue Only"
        Me.ChkIssueOnly.UseVisualStyleBackColor = True
        '
        'ChkTransPrint
        '
        Me.ChkTransPrint.AutoSize = True
        Me.ChkTransPrint.Location = New System.Drawing.Point(392, 315)
        Me.ChkTransPrint.Name = "ChkTransPrint"
        Me.ChkTransPrint.Size = New System.Drawing.Size(117, 21)
        Me.ChkTransPrint.TabIndex = 27
        Me.ChkTransPrint.Text = "Receipt Only"
        Me.ChkTransPrint.UseVisualStyleBackColor = True
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(77, 113)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(82, 17)
        Me.Label15.TabIndex = 34
        Me.Label15.Text = "Date From"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.rbtDispWt)
        Me.Panel3.Controls.Add(Me.rbtDispPcs)
        Me.Panel3.Controls.Add(Me.rbtDispBoth)
        Me.Panel3.Location = New System.Drawing.Point(183, 338)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(203, 22)
        Me.Panel3.TabIndex = 30
        '
        'rbtDispWt
        '
        Me.rbtDispWt.AutoSize = True
        Me.rbtDispWt.Location = New System.Drawing.Point(126, 2)
        Me.rbtDispWt.Name = "rbtDispWt"
        Me.rbtDispWt.Size = New System.Drawing.Size(78, 21)
        Me.rbtDispWt.TabIndex = 2
        Me.rbtDispWt.Text = "Weight"
        Me.rbtDispWt.UseVisualStyleBackColor = True
        '
        'rbtDispPcs
        '
        Me.rbtDispPcs.AutoSize = True
        Me.rbtDispPcs.Location = New System.Drawing.Point(73, 2)
        Me.rbtDispPcs.Name = "rbtDispPcs"
        Me.rbtDispPcs.Size = New System.Drawing.Size(52, 21)
        Me.rbtDispPcs.TabIndex = 1
        Me.rbtDispPcs.Text = "Pcs"
        Me.rbtDispPcs.UseVisualStyleBackColor = True
        '
        'rbtDispBoth
        '
        Me.rbtDispBoth.AutoSize = True
        Me.rbtDispBoth.Checked = True
        Me.rbtDispBoth.Location = New System.Drawing.Point(3, 2)
        Me.rbtDispBoth.Name = "rbtDispBoth"
        Me.rbtDispBoth.Size = New System.Drawing.Size(63, 21)
        Me.rbtDispBoth.TabIndex = 0
        Me.rbtDispBoth.TabStop = True
        Me.rbtDispBoth.Text = "Both"
        Me.rbtDispBoth.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(77, 342)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(96, 17)
        Me.Label11.TabIndex = 29
        Me.Label11.Text = "Display Type"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbtSubItem)
        Me.Panel1.Controls.Add(Me.rbtItem)
        Me.Panel1.Location = New System.Drawing.Point(183, 313)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(203, 22)
        Me.Panel1.TabIndex = 28
        '
        'rbtSubItem
        '
        Me.rbtSubItem.AutoSize = True
        Me.rbtSubItem.Checked = True
        Me.rbtSubItem.Location = New System.Drawing.Point(73, 2)
        Me.rbtSubItem.Name = "rbtSubItem"
        Me.rbtSubItem.Size = New System.Drawing.Size(94, 21)
        Me.rbtSubItem.TabIndex = 1
        Me.rbtSubItem.TabStop = True
        Me.rbtSubItem.Text = "Sub Item"
        Me.rbtSubItem.UseVisualStyleBackColor = True
        '
        'rbtItem
        '
        Me.rbtItem.AutoSize = True
        Me.rbtItem.Location = New System.Drawing.Point(3, 2)
        Me.rbtItem.Name = "rbtItem"
        Me.rbtItem.Size = New System.Drawing.Size(61, 21)
        Me.rbtItem.TabIndex = 0
        Me.rbtItem.Text = "Item"
        Me.rbtItem.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(77, 317)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(94, 17)
        Me.Label4.TabIndex = 27
        Me.Label4.Text = "Report Type"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkRangeOut
        '
        Me.chkRangeOut.AutoSize = True
        Me.chkRangeOut.Location = New System.Drawing.Point(392, 290)
        Me.chkRangeOut.Name = "chkRangeOut"
        Me.chkRangeOut.Size = New System.Drawing.Size(126, 21)
        Me.chkRangeOut.TabIndex = 26
        Me.chkRangeOut.Text = "Out Of Range"
        Me.chkRangeOut.UseVisualStyleBackColor = True
        '
        'chkcmbrange
        '
        Me.chkcmbrange.CheckOnClick = True
        Me.chkcmbrange.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbrange.DropDownHeight = 1
        Me.chkcmbrange.FormattingEnabled = True
        Me.chkcmbrange.IntegralHeight = False
        Me.chkcmbrange.Location = New System.Drawing.Point(183, 259)
        Me.chkcmbrange.Name = "chkcmbrange"
        Me.chkcmbrange.Size = New System.Drawing.Size(290, 25)
        Me.chkcmbrange.TabIndex = 23
        Me.chkcmbrange.ValueSeparator = ", "
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(77, 264)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(52, 17)
        Me.Label1.TabIndex = 22
        Me.Label1.Text = "Range"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnNew
        '
        Me.btnNew.ContextMenuStrip = Me.ContextMenuStrip1
        Me.btnNew.Location = New System.Drawing.Point(289, 366)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 32
        Me.btnNew.Text = "&New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.NewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(135, 52)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(134, 24)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(134, 24)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(395, 366)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 33
        Me.btnExit.Text = "&Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(183, 366)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 31
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'ChkItemMode
        '
        Me.ChkItemMode.CheckOnClick = True
        Me.ChkItemMode.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.ChkItemMode.DropDownHeight = 1
        Me.ChkItemMode.FormattingEnabled = True
        Me.ChkItemMode.IntegralHeight = False
        Me.ChkItemMode.Location = New System.Drawing.Point(183, 60)
        Me.ChkItemMode.Name = "ChkItemMode"
        Me.ChkItemMode.Size = New System.Drawing.Size(290, 25)
        Me.ChkItemMode.TabIndex = 5
        Me.ChkItemMode.ValueSeparator = ", "
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(77, 65)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(82, 17)
        Me.Label12.TabIndex = 4
        Me.Label12.Text = "Item Mode"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbDesigner
        '
        Me.chkCmbDesigner.CheckOnClick = True
        Me.chkCmbDesigner.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbDesigner.DropDownHeight = 1
        Me.chkCmbDesigner.FormattingEnabled = True
        Me.chkCmbDesigner.IntegralHeight = False
        Me.chkCmbDesigner.Location = New System.Drawing.Point(183, 134)
        Me.chkCmbDesigner.Name = "chkCmbDesigner"
        Me.chkCmbDesigner.Size = New System.Drawing.Size(290, 25)
        Me.chkCmbDesigner.TabIndex = 13
        Me.chkCmbDesigner.ValueSeparator = ", "
        '
        'chkCmbItem
        '
        Me.chkCmbItem.CheckOnClick = True
        Me.chkCmbItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbItem.DropDownHeight = 1
        Me.chkCmbItem.FormattingEnabled = True
        Me.chkCmbItem.IntegralHeight = False
        Me.chkCmbItem.Location = New System.Drawing.Point(183, 85)
        Me.chkCmbItem.Name = "chkCmbItem"
        Me.chkCmbItem.Size = New System.Drawing.Size(290, 25)
        Me.chkCmbItem.TabIndex = 7
        Me.chkCmbItem.ValueSeparator = ", "
        AddHandler Me.chkCmbItem.Validated, AddressOf Me.chkCmbItem_Validated_1
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(183, 234)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(290, 25)
        Me.chkCmbCostCentre.TabIndex = 21
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'chkCmbMetal
        '
        Me.chkCmbMetal.CheckOnClick = True
        Me.chkCmbMetal.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbMetal.DropDownHeight = 1
        Me.chkCmbMetal.FormattingEnabled = True
        Me.chkCmbMetal.IntegralHeight = False
        Me.chkCmbMetal.Location = New System.Drawing.Point(183, 11)
        Me.chkCmbMetal.Name = "chkCmbMetal"
        Me.chkCmbMetal.Size = New System.Drawing.Size(290, 25)
        Me.chkCmbMetal.TabIndex = 1
        Me.chkCmbMetal.ValueSeparator = ", "
        '
        'chkCmbItemType
        '
        Me.chkCmbItemType.CheckOnClick = True
        Me.chkCmbItemType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbItemType.DropDownHeight = 1
        Me.chkCmbItemType.FormattingEnabled = True
        Me.chkCmbItemType.IntegralHeight = False
        Me.chkCmbItemType.Location = New System.Drawing.Point(183, 209)
        Me.chkCmbItemType.Name = "chkCmbItemType"
        Me.chkCmbItemType.Size = New System.Drawing.Size(290, 25)
        Me.chkCmbItemType.TabIndex = 19
        Me.chkCmbItemType.ValueSeparator = ", "
        '
        'chkCmbCounter
        '
        Me.chkCmbCounter.CheckOnClick = True
        Me.chkCmbCounter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCounter.DropDownHeight = 1
        Me.chkCmbCounter.FormattingEnabled = True
        Me.chkCmbCounter.IntegralHeight = False
        Me.chkCmbCounter.Location = New System.Drawing.Point(183, 184)
        Me.chkCmbCounter.Name = "chkCmbCounter"
        Me.chkCmbCounter.Size = New System.Drawing.Size(290, 25)
        Me.chkCmbCounter.TabIndex = 17
        Me.chkCmbCounter.ValueSeparator = ", "
        '
        'chkCmbCompany
        '
        Me.chkCmbCompany.CheckOnClick = True
        Me.chkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCompany.DropDownHeight = 1
        Me.chkCmbCompany.FormattingEnabled = True
        Me.chkCmbCompany.IntegralHeight = False
        Me.chkCmbCompany.Location = New System.Drawing.Point(183, 159)
        Me.chkCmbCompany.Name = "chkCmbCompany"
        Me.chkCmbCompany.Size = New System.Drawing.Size(290, 25)
        Me.chkCmbCompany.TabIndex = 15
        Me.chkCmbCompany.ValueSeparator = ", "
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(309, 110)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 24)
        Me.dtpTo.TabIndex = 11
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(183, 110)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(93, 24)
        Me.dtpFrom.TabIndex = 9
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rbtNonTag)
        Me.Panel2.Controls.Add(Me.rbtTag)
        Me.Panel2.Controls.Add(Me.rbtBoth)
        Me.Panel2.Location = New System.Drawing.Point(183, 287)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(203, 22)
        Me.Panel2.TabIndex = 25
        '
        'rbtNonTag
        '
        Me.rbtNonTag.AutoSize = True
        Me.rbtNonTag.Location = New System.Drawing.Point(126, 2)
        Me.rbtNonTag.Name = "rbtNonTag"
        Me.rbtNonTag.Size = New System.Drawing.Size(86, 21)
        Me.rbtNonTag.TabIndex = 2
        Me.rbtNonTag.Text = "Non Tag"
        Me.rbtNonTag.UseVisualStyleBackColor = True
        '
        'rbtTag
        '
        Me.rbtTag.AutoSize = True
        Me.rbtTag.Location = New System.Drawing.Point(73, 2)
        Me.rbtTag.Name = "rbtTag"
        Me.rbtTag.Size = New System.Drawing.Size(53, 21)
        Me.rbtTag.TabIndex = 1
        Me.rbtTag.Text = "Tag"
        Me.rbtTag.UseVisualStyleBackColor = True
        '
        'rbtBoth
        '
        Me.rbtBoth.AutoSize = True
        Me.rbtBoth.Checked = True
        Me.rbtBoth.Location = New System.Drawing.Point(3, 2)
        Me.rbtBoth.Name = "rbtBoth"
        Me.rbtBoth.Size = New System.Drawing.Size(63, 21)
        Me.rbtBoth.TabIndex = 0
        Me.rbtBoth.TabStop = True
        Me.rbtBoth.Text = "Both"
        Me.rbtBoth.UseVisualStyleBackColor = True
        '
        'cmbCategory
        '
        Me.cmbCategory.FormattingEnabled = True
        Me.cmbCategory.Location = New System.Drawing.Point(183, 36)
        Me.cmbCategory.Name = "cmbCategory"
        Me.cmbCategory.Size = New System.Drawing.Size(290, 25)
        Me.cmbCategory.TabIndex = 3
        '
        'label10
        '
        Me.label10.AutoSize = True
        Me.label10.Location = New System.Drawing.Point(77, 16)
        Me.label10.Name = "label10"
        Me.label10.Size = New System.Drawing.Size(44, 17)
        Me.label10.TabIndex = 0
        Me.label10.Text = "Metal"
        Me.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(77, 90)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(79, 17)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "ItemName"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(77, 40)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 17)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Category"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(77, 239)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(93, 17)
        Me.Label.TabIndex = 20
        Me.Label.Text = "Cost Centre"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(77, 214)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(78, 17)
        Me.Label8.TabIndex = 18
        Me.Label8.Text = "Item Type"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(77, 164)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(74, 17)
        Me.Label9.TabIndex = 14
        Me.Label9.Text = "Company"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(77, 189)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(109, 17)
        Me.Label7.TabIndex = 16
        Me.Label7.Text = "Counter Name"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(77, 291)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(109, 17)
        Me.Label5.TabIndex = 24
        Me.Label5.Text = "Selection Type"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(282, 115)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(24, 17)
        Me.lblTo.TabIndex = 10
        Me.lblTo.Text = "To"
        Me.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(77, 139)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(70, 17)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Designer"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.gridviewDetail)
        Me.tabView.Controls.Add(Me.gridView)
        Me.tabView.Controls.Add(Me.gridViewHead)
        Me.tabView.Controls.Add(Me.lblTitle)
        Me.tabView.Controls.Add(Me.pnlfooter)
        Me.tabView.Location = New System.Drawing.Point(4, 29)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(933, 589)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'gridviewDetail
        '
        Me.gridviewDetail.AllowUserToAddRows = False
        Me.gridviewDetail.AllowUserToDeleteRows = False
        Me.gridviewDetail.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridviewDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridviewDetail.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridviewDetail.Location = New System.Drawing.Point(3, 69)
        Me.gridviewDetail.MultiSelect = False
        Me.gridviewDetail.Name = "gridviewDetail"
        Me.gridviewDetail.RowHeadersVisible = False
        Me.gridviewDetail.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.gridviewDetail.RowTemplate.Height = 20
        Me.gridviewDetail.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridviewDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridviewDetail.ShowCellToolTips = False
        Me.gridviewDetail.Size = New System.Drawing.Size(927, 475)
        Me.gridviewDetail.TabIndex = 5
        Me.gridviewDetail.Visible = False
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridView.ContextMenuStrip = Me.cmbGridShortCut
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(3, 69)
        Me.gridView.MultiSelect = False
        Me.gridView.Name = "gridView"
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.gridView.RowTemplate.Height = 20
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.ShowCellToolTips = False
        Me.gridView.Size = New System.Drawing.Size(927, 475)
        Me.gridView.TabIndex = 1
        '
        'cmbGridShortCut
        '
        Me.cmbGridShortCut.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResizeToolStripMenuItem})
        Me.cmbGridShortCut.Name = "ContextMenuStrip2"
        Me.cmbGridShortCut.Size = New System.Drawing.Size(157, 28)
        '
        'ResizeToolStripMenuItem
        '
        Me.ResizeToolStripMenuItem.CheckOnClick = True
        Me.ResizeToolStripMenuItem.Name = "ResizeToolStripMenuItem"
        Me.ResizeToolStripMenuItem.Size = New System.Drawing.Size(156, 24)
        Me.ResizeToolStripMenuItem.Text = "Auto Resize"
        '
        'gridViewHead
        '
        Me.gridViewHead.AllowUserToAddRows = False
        Me.gridViewHead.AllowUserToDeleteRows = False
        Me.gridViewHead.BackgroundColor = System.Drawing.SystemColors.ControlDark
        Me.gridViewHead.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridViewHead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridViewHead.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridViewHead.Enabled = False
        Me.gridViewHead.Location = New System.Drawing.Point(3, 49)
        Me.gridViewHead.Name = "gridViewHead"
        Me.gridViewHead.ReadOnly = True
        Me.gridViewHead.RowHeadersVisible = False
        Me.gridViewHead.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.gridViewHead.Size = New System.Drawing.Size(927, 20)
        Me.gridViewHead.TabIndex = 3
        '
        'lblTitle
        '
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Location = New System.Drawing.Point(3, 3)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(927, 46)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Label3"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlfooter
        '
        Me.pnlfooter.Controls.Add(Me.Label14)
        Me.pnlfooter.Controls.Add(Me.Label13)
        Me.pnlfooter.Controls.Add(Me.btnBack)
        Me.pnlfooter.Controls.Add(Me.btnPrint)
        Me.pnlfooter.Controls.Add(Me.BtnGenerate)
        Me.pnlfooter.Controls.Add(Me.btnPost)
        Me.pnlfooter.Controls.Add(Me.btnExport)
        Me.pnlfooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlfooter.Location = New System.Drawing.Point(3, 544)
        Me.pnlfooter.Name = "pnlfooter"
        Me.pnlfooter.Size = New System.Drawing.Size(927, 42)
        Me.pnlfooter.TabIndex = 2
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.Red
        Me.Label14.Location = New System.Drawing.Point(5, 12)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(275, 18)
        Me.Label14.TabIndex = 3
        Me.Label14.Text = "All Metal Select for Stone Details"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.Red
        Me.Label13.Location = New System.Drawing.Point(229, 12)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(211, 18)
        Me.Label13.TabIndex = 2
        Me.Label13.Text = "Press T for Stone Details"
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(399, 3)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 1
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(609, 3)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 0
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'BtnGenerate
        '
        Me.BtnGenerate.Enabled = False
        Me.BtnGenerate.Location = New System.Drawing.Point(821, 3)
        Me.BtnGenerate.Name = "BtnGenerate"
        Me.BtnGenerate.Size = New System.Drawing.Size(100, 30)
        Me.BtnGenerate.TabIndex = 0
        Me.BtnGenerate.Text = "PO Generate"
        Me.BtnGenerate.UseVisualStyleBackColor = True
        '
        'btnPost
        '
        Me.btnPost.Enabled = False
        Me.btnPost.Location = New System.Drawing.Point(716, 3)
        Me.btnPost.Name = "btnPost"
        Me.btnPost.Size = New System.Drawing.Size(100, 30)
        Me.btnPost.TabIndex = 0
        Me.btnPost.Text = "Posting"
        Me.btnPost.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(503, 3)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 0
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'frmItemRangeWiseStockIssue
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(941, 622)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmItemRangeWiseStockIssue"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Item Range Wise Stock"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.tabMain.ResumeLayout(False)
        Me.tabGen.ResumeLayout(False)
        Me.pnlGroupFilter.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.tabView.ResumeLayout(False)
        CType(Me.gridviewDetail, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmbGridShortCut.ResumeLayout(False)
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlfooter.ResumeLayout(False)
        Me.pnlfooter.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlGroupFilter As System.Windows.Forms.Panel
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbCategory As System.Windows.Forms.ComboBox
    Friend WithEvents label10 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rbtNonTag As System.Windows.Forms.RadioButton
    Friend WithEvents rbtTag As System.Windows.Forms.RadioButton
    Friend WithEvents rbtBoth As System.Windows.Forms.RadioButton
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGen As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents gridViewHead As System.Windows.Forms.DataGridView
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlfooter As System.Windows.Forms.Panel
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkCmbMetal As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbItemType As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbCounter As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents chkCmbDesigner As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbItem As BrighttechPack.CheckedComboBox
    Friend WithEvents ChkItemMode As BrighttechPack.CheckedComboBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents gridviewDetail As System.Windows.Forms.DataGridView
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents chkcmbrange As BrighttechPack.CheckedComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkRangeOut As System.Windows.Forms.CheckBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rbtSubItem As System.Windows.Forms.RadioButton
    Friend WithEvents rbtItem As System.Windows.Forms.RadioButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents rbtDispWt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtDispPcs As System.Windows.Forms.RadioButton
    Friend WithEvents rbtDispBoth As System.Windows.Forms.RadioButton
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents ChkTransPrint As System.Windows.Forms.CheckBox
    Friend WithEvents ChkIssueOnly As CheckBox
    Friend WithEvents chkZero As CheckBox
    Friend WithEvents btnPost As Button
    Friend WithEvents BtnGenerate As Button
End Class
