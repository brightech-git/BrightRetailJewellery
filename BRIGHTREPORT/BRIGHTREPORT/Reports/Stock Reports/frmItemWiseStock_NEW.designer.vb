<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmItemWiseStock_NEW
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
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabGen = New System.Windows.Forms.TabPage
        Me.pnlGroupFilter = New System.Windows.Forms.Panel
        Me.btnNew = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.chkLoseStSepCol = New System.Windows.Forms.CheckBox
        Me.chkWithCumulative = New System.Windows.Forms.CheckBox
        Me.chkSeperateColumnApproval = New System.Windows.Forms.CheckBox
        Me.chkOnlyApproval = New System.Windows.Forms.CheckBox
        Me.chkStyleNo = New System.Windows.Forms.CheckBox
        Me.chkWithValue = New System.Windows.Forms.CheckBox
        Me.chkWithRate = New System.Windows.Forms.CheckBox
        Me.chkCmbDesigner = New BrighttechPack.CheckedComboBox
        Me.chkCmbItem = New BrighttechPack.CheckedComboBox
        Me.chkAsOnDate = New System.Windows.Forms.CheckBox
        Me.ChkLstGroupBy = New System.Windows.Forms.CheckedListBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.pnlDisStnResult = New System.Windows.Forms.Panel
        Me.rbtDiaStnByColumn = New System.Windows.Forms.RadioButton
        Me.rbtDiaStnByRow = New System.Windows.Forms.RadioButton
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.chkCmbMetal = New BrighttechPack.CheckedComboBox
        Me.chkCmbItemType = New BrighttechPack.CheckedComboBox
        Me.chkWithSubItem = New System.Windows.Forms.CheckBox
        Me.chkCmbCounter = New BrighttechPack.CheckedComboBox
        Me.chkStone = New System.Windows.Forms.CheckBox
        Me.chkCmbCompany = New BrighttechPack.CheckedComboBox
        Me.chkOrderbyItemId = New System.Windows.Forms.CheckBox
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.chkWithNegativeStock = New System.Windows.Forms.CheckBox
        Me.chkOnlyTag = New System.Windows.Forms.CheckBox
        Me.chkWithApproval = New System.Windows.Forms.CheckBox
        Me.chkDiamond = New System.Windows.Forms.CheckBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.rbtNonTag = New System.Windows.Forms.RadioButton
        Me.rbtTag = New System.Windows.Forms.RadioButton
        Me.rbtBoth = New System.Windows.Forms.RadioButton
        Me.chkNetWt = New System.Windows.Forms.CheckBox
        Me.chkAll = New System.Windows.Forms.CheckBox
        Me.chkGrsWt = New System.Windows.Forms.CheckBox
        Me.rbtOrder = New System.Windows.Forms.RadioButton
        Me.rbtRegular = New System.Windows.Forms.RadioButton
        Me.rbtAll = New System.Windows.Forms.RadioButton
        Me.cmbCategory = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.label10 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.lblTo = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.tabView = New System.Windows.Forms.TabPage
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.gridViewHead = New System.Windows.Forms.DataGridView
        Me.lblTitle = New System.Windows.Forms.Label
        Me.pnlfooter = New System.Windows.Forms.Panel
        Me.btnBack = New System.Windows.Forms.Button
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.tabMain.SuspendLayout()
        Me.tabGen.SuspendLayout()
        Me.pnlGroupFilter.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.pnlDisStnResult.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.tabView.SuspendLayout()
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
        Me.tabMain.Size = New System.Drawing.Size(994, 626)
        Me.tabMain.TabIndex = 3
        '
        'tabGen
        '
        Me.tabGen.Controls.Add(Me.pnlGroupFilter)
        Me.tabGen.Location = New System.Drawing.Point(4, 25)
        Me.tabGen.Name = "tabGen"
        Me.tabGen.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGen.Size = New System.Drawing.Size(986, 597)
        Me.tabGen.TabIndex = 0
        Me.tabGen.Text = "Gen"
        Me.tabGen.UseVisualStyleBackColor = True
        '
        'pnlGroupFilter
        '
        Me.pnlGroupFilter.Controls.Add(Me.btnNew)
        Me.pnlGroupFilter.Controls.Add(Me.GroupBox1)
        Me.pnlGroupFilter.Controls.Add(Me.btnExit)
        Me.pnlGroupFilter.Controls.Add(Me.btnView_Search)
        Me.pnlGroupFilter.Location = New System.Drawing.Point(276, 5)
        Me.pnlGroupFilter.Name = "pnlGroupFilter"
        Me.pnlGroupFilter.Size = New System.Drawing.Size(535, 542)
        Me.pnlGroupFilter.TabIndex = 0
        '
        'btnNew
        '
        Me.btnNew.ContextMenuStrip = Me.ContextMenuStrip1
        Me.btnNew.Location = New System.Drawing.Point(232, 506)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 2
        Me.btnNew.Text = "&New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.NewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 48)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkLoseStSepCol)
        Me.GroupBox1.Controls.Add(Me.chkWithCumulative)
        Me.GroupBox1.Controls.Add(Me.chkSeperateColumnApproval)
        Me.GroupBox1.Controls.Add(Me.chkOnlyApproval)
        Me.GroupBox1.Controls.Add(Me.chkStyleNo)
        Me.GroupBox1.Controls.Add(Me.chkWithValue)
        Me.GroupBox1.Controls.Add(Me.chkWithRate)
        Me.GroupBox1.Controls.Add(Me.chkCmbDesigner)
        Me.GroupBox1.Controls.Add(Me.chkCmbItem)
        Me.GroupBox1.Controls.Add(Me.chkAsOnDate)
        Me.GroupBox1.Controls.Add(Me.ChkLstGroupBy)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.pnlDisStnResult)
        Me.GroupBox1.Controls.Add(Me.chkCmbCostCentre)
        Me.GroupBox1.Controls.Add(Me.chkCmbMetal)
        Me.GroupBox1.Controls.Add(Me.chkCmbItemType)
        Me.GroupBox1.Controls.Add(Me.chkWithSubItem)
        Me.GroupBox1.Controls.Add(Me.chkCmbCounter)
        Me.GroupBox1.Controls.Add(Me.chkStone)
        Me.GroupBox1.Controls.Add(Me.chkCmbCompany)
        Me.GroupBox1.Controls.Add(Me.chkOrderbyItemId)
        Me.GroupBox1.Controls.Add(Me.dtpTo)
        Me.GroupBox1.Controls.Add(Me.dtpFrom)
        Me.GroupBox1.Controls.Add(Me.chkWithNegativeStock)
        Me.GroupBox1.Controls.Add(Me.chkOnlyTag)
        Me.GroupBox1.Controls.Add(Me.chkWithApproval)
        Me.GroupBox1.Controls.Add(Me.chkDiamond)
        Me.GroupBox1.Controls.Add(Me.Panel2)
        Me.GroupBox1.Controls.Add(Me.chkNetWt)
        Me.GroupBox1.Controls.Add(Me.chkAll)
        Me.GroupBox1.Controls.Add(Me.chkGrsWt)
        Me.GroupBox1.Controls.Add(Me.rbtOrder)
        Me.GroupBox1.Controls.Add(Me.rbtRegular)
        Me.GroupBox1.Controls.Add(Me.rbtAll)
        Me.GroupBox1.Controls.Add(Me.cmbCategory)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.label10)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.lblTo)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(518, 497)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'chkLoseStSepCol
        '
        Me.chkLoseStSepCol.AutoSize = True
        Me.chkLoseStSepCol.Location = New System.Drawing.Point(297, 381)
        Me.chkLoseStSepCol.Name = "chkLoseStSepCol"
        Me.chkLoseStSepCol.Size = New System.Drawing.Size(190, 17)
        Me.chkLoseStSepCol.TabIndex = 36
        Me.chkLoseStSepCol.Text = "Loose Dia/Stn in Dia/Stn Col"
        Me.chkLoseStSepCol.UseVisualStyleBackColor = True
        '
        'chkWithCumulative
        '
        Me.chkWithCumulative.AutoSize = True
        Me.chkWithCumulative.Checked = True
        Me.chkWithCumulative.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkWithCumulative.Location = New System.Drawing.Point(252, 450)
        Me.chkWithCumulative.Name = "chkWithCumulative"
        Me.chkWithCumulative.Size = New System.Drawing.Size(156, 17)
        Me.chkWithCumulative.TabIndex = 43
        Me.chkWithCumulative.Text = "With Cumulative Stock"
        Me.chkWithCumulative.UseVisualStyleBackColor = True
        '
        'chkSeperateColumnApproval
        '
        Me.chkSeperateColumnApproval.AutoSize = True
        Me.chkSeperateColumnApproval.Location = New System.Drawing.Point(369, 427)
        Me.chkSeperateColumnApproval.Name = "chkSeperateColumnApproval"
        Me.chkSeperateColumnApproval.Size = New System.Drawing.Size(146, 17)
        Me.chkSeperateColumnApproval.TabIndex = 41
        Me.chkSeperateColumnApproval.Text = "Sep Col for Approval"
        Me.chkSeperateColumnApproval.UseVisualStyleBackColor = True
        '
        'chkOnlyApproval
        '
        Me.chkOnlyApproval.AutoSize = True
        Me.chkOnlyApproval.Location = New System.Drawing.Point(265, 427)
        Me.chkOnlyApproval.Name = "chkOnlyApproval"
        Me.chkOnlyApproval.Size = New System.Drawing.Size(107, 17)
        Me.chkOnlyApproval.TabIndex = 40
        Me.chkOnlyApproval.Text = "Approval Only"
        Me.chkOnlyApproval.UseVisualStyleBackColor = True
        '
        'chkStyleNo
        '
        Me.chkStyleNo.AutoSize = True
        Me.chkStyleNo.Location = New System.Drawing.Point(418, 313)
        Me.chkStyleNo.Name = "chkStyleNo"
        Me.chkStyleNo.Size = New System.Drawing.Size(70, 17)
        Me.chkStyleNo.TabIndex = 30
        Me.chkStyleNo.Text = "StyleNo"
        Me.chkStyleNo.UseVisualStyleBackColor = True
        '
        'chkWithValue
        '
        Me.chkWithValue.AutoSize = True
        Me.chkWithValue.Location = New System.Drawing.Point(276, 313)
        Me.chkWithValue.Name = "chkWithValue"
        Me.chkWithValue.Size = New System.Drawing.Size(82, 17)
        Me.chkWithValue.TabIndex = 28
        Me.chkWithValue.Text = "Salevalue"
        Me.chkWithValue.UseVisualStyleBackColor = True
        '
        'chkWithRate
        '
        Me.chkWithRate.AutoSize = True
        Me.chkWithRate.Location = New System.Drawing.Point(360, 313)
        Me.chkWithRate.Name = "chkWithRate"
        Me.chkWithRate.Size = New System.Drawing.Size(52, 17)
        Me.chkWithRate.TabIndex = 29
        Me.chkWithRate.Text = "Rate"
        Me.chkWithRate.UseVisualStyleBackColor = True
        '
        'chkCmbDesigner
        '
        Me.chkCmbDesigner.CheckOnClick = True
        Me.chkCmbDesigner.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbDesigner.DropDownHeight = 1
        Me.chkCmbDesigner.FormattingEnabled = True
        Me.chkCmbDesigner.IntegralHeight = False
        Me.chkCmbDesigner.Location = New System.Drawing.Point(122, 106)
        Me.chkCmbDesigner.Name = "chkCmbDesigner"
        Me.chkCmbDesigner.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbDesigner.TabIndex = 11
        Me.chkCmbDesigner.ValueSeparator = ", "
        '
        'chkCmbItem
        '
        Me.chkCmbItem.CheckOnClick = True
        Me.chkCmbItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbItem.DropDownHeight = 1
        Me.chkCmbItem.FormattingEnabled = True
        Me.chkCmbItem.IntegralHeight = False
        Me.chkCmbItem.Location = New System.Drawing.Point(122, 57)
        Me.chkCmbItem.Name = "chkCmbItem"
        Me.chkCmbItem.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbItem.TabIndex = 5
        Me.chkCmbItem.ValueSeparator = ", "
        '
        'chkAsOnDate
        '
        Me.chkAsOnDate.AutoSize = True
        Me.chkAsOnDate.Checked = True
        Me.chkAsOnDate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAsOnDate.Location = New System.Drawing.Point(9, 86)
        Me.chkAsOnDate.Name = "chkAsOnDate"
        Me.chkAsOnDate.Size = New System.Drawing.Size(91, 17)
        Me.chkAsOnDate.TabIndex = 6
        Me.chkAsOnDate.Text = "As On Date"
        Me.chkAsOnDate.UseVisualStyleBackColor = True
        '
        'ChkLstGroupBy
        '
        Me.ChkLstGroupBy.FormattingEnabled = True
        Me.ChkLstGroupBy.Items.AddRange(New Object() {"COSTCENTRE", "DESIGNER", "COUNTER"})
        Me.ChkLstGroupBy.Location = New System.Drawing.Point(122, 231)
        Me.ChkLstGroupBy.Name = "ChkLstGroupBy"
        Me.ChkLstGroupBy.Size = New System.Drawing.Size(135, 52)
        Me.ChkLstGroupBy.TabIndex = 21
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(6, 406)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(87, 13)
        Me.Label11.TabIndex = 37
        Me.Label11.Text = "Dis Stn Result"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnlDisStnResult
        '
        Me.pnlDisStnResult.Controls.Add(Me.rbtDiaStnByColumn)
        Me.pnlDisStnResult.Controls.Add(Me.rbtDiaStnByRow)
        Me.pnlDisStnResult.Location = New System.Drawing.Point(122, 404)
        Me.pnlDisStnResult.Name = "pnlDisStnResult"
        Me.pnlDisStnResult.Size = New System.Drawing.Size(184, 17)
        Me.pnlDisStnResult.TabIndex = 38
        '
        'rbtDiaStnByColumn
        '
        Me.rbtDiaStnByColumn.AutoSize = True
        Me.rbtDiaStnByColumn.Location = New System.Drawing.Point(72, 0)
        Me.rbtDiaStnByColumn.Name = "rbtDiaStnByColumn"
        Me.rbtDiaStnByColumn.Size = New System.Drawing.Size(88, 17)
        Me.rbtDiaStnByColumn.TabIndex = 1
        Me.rbtDiaStnByColumn.Text = "By Column"
        Me.rbtDiaStnByColumn.UseVisualStyleBackColor = True
        '
        'rbtDiaStnByRow
        '
        Me.rbtDiaStnByRow.AutoSize = True
        Me.rbtDiaStnByRow.Checked = True
        Me.rbtDiaStnByRow.Location = New System.Drawing.Point(3, 0)
        Me.rbtDiaStnByRow.Name = "rbtDiaStnByRow"
        Me.rbtDiaStnByRow.Size = New System.Drawing.Size(68, 17)
        Me.rbtDiaStnByRow.TabIndex = 0
        Me.rbtDiaStnByRow.TabStop = True
        Me.rbtDiaStnByRow.Text = "By Row"
        Me.rbtDiaStnByRow.UseVisualStyleBackColor = True
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(122, 206)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbCostCentre.TabIndex = 19
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'chkCmbMetal
        '
        Me.chkCmbMetal.CheckOnClick = True
        Me.chkCmbMetal.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbMetal.DropDownHeight = 1
        Me.chkCmbMetal.FormattingEnabled = True
        Me.chkCmbMetal.IntegralHeight = False
        Me.chkCmbMetal.Location = New System.Drawing.Point(122, 8)
        Me.chkCmbMetal.Name = "chkCmbMetal"
        Me.chkCmbMetal.Size = New System.Drawing.Size(290, 22)
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
        Me.chkCmbItemType.Location = New System.Drawing.Point(122, 181)
        Me.chkCmbItemType.Name = "chkCmbItemType"
        Me.chkCmbItemType.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbItemType.TabIndex = 17
        Me.chkCmbItemType.ValueSeparator = ", "
        '
        'chkWithSubItem
        '
        Me.chkWithSubItem.AutoSize = True
        Me.chkWithSubItem.Location = New System.Drawing.Point(233, 357)
        Me.chkWithSubItem.Name = "chkWithSubItem"
        Me.chkWithSubItem.Size = New System.Drawing.Size(104, 17)
        Me.chkWithSubItem.TabIndex = 33
        Me.chkWithSubItem.Text = "With SubItem"
        Me.chkWithSubItem.UseVisualStyleBackColor = True
        '
        'chkCmbCounter
        '
        Me.chkCmbCounter.CheckOnClick = True
        Me.chkCmbCounter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCounter.DropDownHeight = 1
        Me.chkCmbCounter.FormattingEnabled = True
        Me.chkCmbCounter.IntegralHeight = False
        Me.chkCmbCounter.Location = New System.Drawing.Point(122, 156)
        Me.chkCmbCounter.Name = "chkCmbCounter"
        Me.chkCmbCounter.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbCounter.TabIndex = 15
        Me.chkCmbCounter.ValueSeparator = ", "
        '
        'chkStone
        '
        Me.chkStone.AutoSize = True
        Me.chkStone.Checked = True
        Me.chkStone.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkStone.Location = New System.Drawing.Point(203, 381)
        Me.chkStone.Name = "chkStone"
        Me.chkStone.Size = New System.Drawing.Size(88, 17)
        Me.chkStone.TabIndex = 35
        Me.chkStone.Text = "With Stone"
        Me.chkStone.UseVisualStyleBackColor = True
        '
        'chkCmbCompany
        '
        Me.chkCmbCompany.CheckOnClick = True
        Me.chkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCompany.DropDownHeight = 1
        Me.chkCmbCompany.FormattingEnabled = True
        Me.chkCmbCompany.IntegralHeight = False
        Me.chkCmbCompany.Location = New System.Drawing.Point(122, 131)
        Me.chkCmbCompany.Name = "chkCmbCompany"
        Me.chkCmbCompany.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbCompany.TabIndex = 13
        Me.chkCmbCompany.ValueSeparator = ", "
        '
        'chkOrderbyItemId
        '
        Me.chkOrderbyItemId.AutoSize = True
        Me.chkOrderbyItemId.Checked = True
        Me.chkOrderbyItemId.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkOrderbyItemId.Location = New System.Drawing.Point(125, 450)
        Me.chkOrderbyItemId.Name = "chkOrderbyItemId"
        Me.chkOrderbyItemId.Size = New System.Drawing.Size(120, 17)
        Me.chkOrderbyItemId.TabIndex = 42
        Me.chkOrderbyItemId.Text = "Order by ItemId"
        Me.chkOrderbyItemId.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(248, 82)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpTo.TabIndex = 9
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(122, 82)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(93, 21)
        Me.dtpFrom.TabIndex = 7
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'chkWithNegativeStock
        '
        Me.chkWithNegativeStock.AutoSize = True
        Me.chkWithNegativeStock.Checked = True
        Me.chkWithNegativeStock.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkWithNegativeStock.Location = New System.Drawing.Point(125, 358)
        Me.chkWithNegativeStock.Name = "chkWithNegativeStock"
        Me.chkWithNegativeStock.Size = New System.Drawing.Size(111, 17)
        Me.chkWithNegativeStock.TabIndex = 32
        Me.chkWithNegativeStock.Text = "With -Ve Stock"
        Me.chkWithNegativeStock.UseVisualStyleBackColor = True
        '
        'chkOnlyTag
        '
        Me.chkOnlyTag.AutoSize = True
        Me.chkOnlyTag.Location = New System.Drawing.Point(353, 290)
        Me.chkOnlyTag.Name = "chkOnlyTag"
        Me.chkOnlyTag.Size = New System.Drawing.Size(77, 17)
        Me.chkOnlyTag.TabIndex = 24
        Me.chkOnlyTag.Text = "Only Tag"
        Me.chkOnlyTag.UseVisualStyleBackColor = True
        '
        'chkWithApproval
        '
        Me.chkWithApproval.AutoSize = True
        Me.chkWithApproval.Checked = True
        Me.chkWithApproval.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkWithApproval.Location = New System.Drawing.Point(125, 427)
        Me.chkWithApproval.Name = "chkWithApproval"
        Me.chkWithApproval.Size = New System.Drawing.Size(142, 17)
        Me.chkWithApproval.TabIndex = 39
        Me.chkWithApproval.Text = "Stock With Approval"
        Me.chkWithApproval.UseVisualStyleBackColor = True
        '
        'chkDiamond
        '
        Me.chkDiamond.AutoSize = True
        Me.chkDiamond.Checked = True
        Me.chkDiamond.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDiamond.Location = New System.Drawing.Point(125, 381)
        Me.chkDiamond.Name = "chkDiamond"
        Me.chkDiamond.Size = New System.Drawing.Size(74, 17)
        Me.chkDiamond.TabIndex = 34
        Me.chkDiamond.Text = "With Dia"
        Me.chkDiamond.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rbtNonTag)
        Me.Panel2.Controls.Add(Me.rbtTag)
        Me.Panel2.Controls.Add(Me.rbtBoth)
        Me.Panel2.Location = New System.Drawing.Point(122, 288)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(203, 22)
        Me.Panel2.TabIndex = 23
        '
        'rbtNonTag
        '
        Me.rbtNonTag.AutoSize = True
        Me.rbtNonTag.Location = New System.Drawing.Point(126, 2)
        Me.rbtNonTag.Name = "rbtNonTag"
        Me.rbtNonTag.Size = New System.Drawing.Size(72, 17)
        Me.rbtNonTag.TabIndex = 2
        Me.rbtNonTag.Text = "Non Tag"
        Me.rbtNonTag.UseVisualStyleBackColor = True
        '
        'rbtTag
        '
        Me.rbtTag.AutoSize = True
        Me.rbtTag.Location = New System.Drawing.Point(73, 2)
        Me.rbtTag.Name = "rbtTag"
        Me.rbtTag.Size = New System.Drawing.Size(46, 17)
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
        Me.rbtBoth.Size = New System.Drawing.Size(51, 17)
        Me.rbtBoth.TabIndex = 0
        Me.rbtBoth.TabStop = True
        Me.rbtBoth.Text = "Both"
        Me.rbtBoth.UseVisualStyleBackColor = True
        '
        'chkNetWt
        '
        Me.chkNetWt.AutoSize = True
        Me.chkNetWt.Location = New System.Drawing.Point(209, 314)
        Me.chkNetWt.Name = "chkNetWt"
        Me.chkNetWt.Size = New System.Drawing.Size(64, 17)
        Me.chkNetWt.TabIndex = 27
        Me.chkNetWt.Text = "Net Wt"
        Me.chkNetWt.UseVisualStyleBackColor = True
        '
        'chkAll
        '
        Me.chkAll.AutoSize = True
        Me.chkAll.Location = New System.Drawing.Point(125, 335)
        Me.chkAll.Name = "chkAll"
        Me.chkAll.Size = New System.Drawing.Size(146, 17)
        Me.chkAll.TabIndex = 31
        Me.chkAll.Text = "All [With Zero Stock]"
        Me.chkAll.UseVisualStyleBackColor = True
        '
        'chkGrsWt
        '
        Me.chkGrsWt.AutoSize = True
        Me.chkGrsWt.Checked = True
        Me.chkGrsWt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkGrsWt.Location = New System.Drawing.Point(125, 314)
        Me.chkGrsWt.Name = "chkGrsWt"
        Me.chkGrsWt.Size = New System.Drawing.Size(78, 17)
        Me.chkGrsWt.TabIndex = 26
        Me.chkGrsWt.Text = "Gross Wt"
        Me.chkGrsWt.UseVisualStyleBackColor = True
        '
        'rbtOrder
        '
        Me.rbtOrder.AutoSize = True
        Me.rbtOrder.Location = New System.Drawing.Point(234, 473)
        Me.rbtOrder.Name = "rbtOrder"
        Me.rbtOrder.Size = New System.Drawing.Size(58, 17)
        Me.rbtOrder.TabIndex = 46
        Me.rbtOrder.Text = "Order"
        Me.rbtOrder.UseVisualStyleBackColor = True
        '
        'rbtRegular
        '
        Me.rbtRegular.AutoSize = True
        Me.rbtRegular.Location = New System.Drawing.Point(164, 473)
        Me.rbtRegular.Name = "rbtRegular"
        Me.rbtRegular.Size = New System.Drawing.Size(69, 17)
        Me.rbtRegular.TabIndex = 45
        Me.rbtRegular.Text = "Regular"
        Me.rbtRegular.UseVisualStyleBackColor = True
        '
        'rbtAll
        '
        Me.rbtAll.AutoSize = True
        Me.rbtAll.Checked = True
        Me.rbtAll.Location = New System.Drawing.Point(125, 473)
        Me.rbtAll.Name = "rbtAll"
        Me.rbtAll.Size = New System.Drawing.Size(39, 17)
        Me.rbtAll.TabIndex = 44
        Me.rbtAll.TabStop = True
        Me.rbtAll.Text = "All"
        Me.rbtAll.UseVisualStyleBackColor = True
        '
        'cmbCategory
        '
        Me.cmbCategory.FormattingEnabled = True
        Me.cmbCategory.Location = New System.Drawing.Point(122, 33)
        Me.cmbCategory.Name = "cmbCategory"
        Me.cmbCategory.Size = New System.Drawing.Size(290, 21)
        Me.cmbCategory.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(6, 314)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 21)
        Me.Label1.TabIndex = 25
        Me.Label1.Text = "Piece(s) With"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'label10
        '
        Me.label10.AutoSize = True
        Me.label10.Location = New System.Drawing.Point(6, 15)
        Me.label10.Name = "label10"
        Me.label10.Size = New System.Drawing.Size(37, 13)
        Me.label10.TabIndex = 0
        Me.label10.Text = "Metal"
        Me.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 63)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "ItemName"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 39)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Category"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(6, 207)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(76, 13)
        Me.Label.TabIndex = 18
        Me.Label.Text = "Cost Centre"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 183)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(66, 13)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "Item Type"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 135)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(62, 13)
        Me.Label9.TabIndex = 12
        Me.Label9.Text = "Company"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 159)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(90, 13)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "Counter Name"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(6, 288)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(100, 21)
        Me.Label5.TabIndex = 22
        Me.Label5.Text = "Selection Type"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(221, 85)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(21, 13)
        Me.lblTo.TabIndex = 8
        Me.lblTo.Text = "To"
        Me.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 231)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(61, 13)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "Group By"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 111)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(58, 13)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "Designer"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(338, 506)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 3
        Me.btnExit.Text = "&Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(126, 506)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 1
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.gridView)
        Me.tabView.Controls.Add(Me.gridViewHead)
        Me.tabView.Controls.Add(Me.lblTitle)
        Me.tabView.Controls.Add(Me.pnlfooter)
        Me.tabView.Location = New System.Drawing.Point(4, 25)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(986, 597)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
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
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.gridView.RowTemplate.Height = 20
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.ShowCellToolTips = False
        Me.gridView.Size = New System.Drawing.Size(980, 483)
        Me.gridView.TabIndex = 1
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
        Me.gridViewHead.Size = New System.Drawing.Size(980, 20)
        Me.gridViewHead.TabIndex = 3
        '
        'lblTitle
        '
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Location = New System.Drawing.Point(3, 3)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(980, 46)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Label3"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlfooter
        '
        Me.pnlfooter.Controls.Add(Me.btnBack)
        Me.pnlfooter.Controls.Add(Me.btnPrint)
        Me.pnlfooter.Controls.Add(Me.btnExport)
        Me.pnlfooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlfooter.Location = New System.Drawing.Point(3, 552)
        Me.pnlfooter.Name = "pnlfooter"
        Me.pnlfooter.Size = New System.Drawing.Size(980, 42)
        Me.pnlfooter.TabIndex = 2
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(598, 3)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 1
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(814, 3)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 0
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(706, 3)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 0
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'frmItemWiseStock
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(994, 626)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmItemWiseStock_New"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Item Wise Stock"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.tabMain.ResumeLayout(False)
        Me.tabGen.ResumeLayout(False)
        Me.pnlGroupFilter.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.pnlDisStnResult.ResumeLayout(False)
        Me.pnlDisStnResult.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.tabView.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmbGridShortCut.ResumeLayout(False)
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlfooter.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlGroupFilter As System.Windows.Forms.Panel
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbtOrder As System.Windows.Forms.RadioButton
    Friend WithEvents rbtRegular As System.Windows.Forms.RadioButton
    Friend WithEvents rbtAll As System.Windows.Forms.RadioButton
    Friend WithEvents Label As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents chkNetWt As System.Windows.Forms.CheckBox
    Friend WithEvents chkGrsWt As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkAll As System.Windows.Forms.CheckBox
    Friend WithEvents cmbCategory As System.Windows.Forms.ComboBox
    Friend WithEvents label10 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rbtNonTag As System.Windows.Forms.RadioButton
    Friend WithEvents rbtTag As System.Windows.Forms.RadioButton
    Friend WithEvents rbtBoth As System.Windows.Forms.RadioButton
    Friend WithEvents chkDiamond As System.Windows.Forms.CheckBox
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
    Friend WithEvents chkWithApproval As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithSubItem As System.Windows.Forms.CheckBox
    Friend WithEvents chkOnlyTag As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithNegativeStock As System.Windows.Forms.CheckBox
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkOrderbyItemId As System.Windows.Forms.CheckBox
    Friend WithEvents chkStone As System.Windows.Forms.CheckBox
    Friend WithEvents chkCmbMetal As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbItemType As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbCounter As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents pnlDisStnResult As System.Windows.Forms.Panel
    Friend WithEvents rbtDiaStnByColumn As System.Windows.Forms.RadioButton
    Friend WithEvents rbtDiaStnByRow As System.Windows.Forms.RadioButton
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChkLstGroupBy As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkAsOnDate As System.Windows.Forms.CheckBox
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents chkCmbDesigner As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbItem As BrighttechPack.CheckedComboBox
    Friend WithEvents chkWithRate As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithValue As System.Windows.Forms.CheckBox
    Friend WithEvents chkStyleNo As System.Windows.Forms.CheckBox
    Friend WithEvents chkOnlyApproval As System.Windows.Forms.CheckBox
    Friend WithEvents chkSeperateColumnApproval As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithCumulative As System.Windows.Forms.CheckBox
    Friend WithEvents chkLoseStSepCol As System.Windows.Forms.CheckBox
End Class
