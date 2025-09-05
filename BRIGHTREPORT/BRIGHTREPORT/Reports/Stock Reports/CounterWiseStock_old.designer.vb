<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CounterWiseStock_old
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.chkSeperateTransferCol = New System.Windows.Forms.CheckBox
        Me.chkOrderbyId = New System.Windows.Forms.CheckBox
        Me.chkOnlyApproval = New System.Windows.Forms.CheckBox
        Me.chkNetWt = New System.Windows.Forms.CheckBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.rbtWithClosing = New System.Windows.Forms.RadioButton
        Me.rbtWithOpening = New System.Windows.Forms.RadioButton
        Me.rbtWithBoth = New System.Windows.Forms.RadioButton
        Me.chkTransactionOnly = New System.Windows.Forms.CheckBox
        Me.chkCompanySelectAll = New System.Windows.Forms.CheckBox
        Me.chkLstCompany = New System.Windows.Forms.CheckedListBox
        Me.pnlDetailedSummary = New System.Windows.Forms.Panel
        Me.rbtSummary = New System.Windows.Forms.RadioButton
        Me.rbtDetailed = New System.Windows.Forms.RadioButton
        Me.chkAllCostCentre = New System.Windows.Forms.CheckBox
        Me.chkAllItemType = New System.Windows.Forms.CheckBox
        Me.chkAllCounter = New System.Windows.Forms.CheckBox
        Me.dtpAsOnDate = New BrighttechPack.DatePicker(Me.components)
        Me.chkOnlyTag = New System.Windows.Forms.CheckBox
        Me.chkWithSubItem = New System.Windows.Forms.CheckBox
        Me.chkWithApproval = New System.Windows.Forms.CheckBox
        Me.cmbItemName = New System.Windows.Forms.ComboBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.rbtNonTag = New System.Windows.Forms.RadioButton
        Me.rbtTag = New System.Windows.Forms.RadioButton
        Me.rbtBoth = New System.Windows.Forms.RadioButton
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox
        Me.chkLstItemType = New System.Windows.Forms.CheckedListBox
        Me.chkLstCounter = New System.Windows.Forms.CheckedListBox
        Me.chkGrsWt = New System.Windows.Forms.CheckBox
        Me.cmbMetal = New System.Windows.Forms.ComboBox
        Me.cmbCategory = New System.Windows.Forms.ComboBox
        Me.cmbDesigner = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.label10 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.AsOnDate = New System.Windows.Forms.Label
        Me.btnNew = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.chkAll = New System.Windows.Forms.CheckBox
        Me.rbtOrder = New System.Windows.Forms.RadioButton
        Me.chkWithStudded = New System.Windows.Forms.CheckBox
        Me.chkWithNegativeStock = New System.Windows.Forms.CheckBox
        Me.rbtRegular = New System.Windows.Forms.RadioButton
        Me.rbtAll = New System.Windows.Forms.RadioButton
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
        Me.GroupBox1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pnlDetailedSummary.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
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
        Me.tabGen.Controls.Add(Me.chkAll)
        Me.tabGen.Controls.Add(Me.rbtOrder)
        Me.tabGen.Controls.Add(Me.chkWithStudded)
        Me.tabGen.Controls.Add(Me.chkWithNegativeStock)
        Me.tabGen.Controls.Add(Me.rbtRegular)
        Me.tabGen.Controls.Add(Me.rbtAll)
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
        Me.pnlGroupFilter.Controls.Add(Me.GroupBox1)
        Me.pnlGroupFilter.Controls.Add(Me.btnNew)
        Me.pnlGroupFilter.Controls.Add(Me.btnExit)
        Me.pnlGroupFilter.Controls.Add(Me.btnView_Search)
        Me.pnlGroupFilter.Location = New System.Drawing.Point(276, -11)
        Me.pnlGroupFilter.Name = "pnlGroupFilter"
        Me.pnlGroupFilter.Size = New System.Drawing.Size(439, 594)
        Me.pnlGroupFilter.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkSeperateTransferCol)
        Me.GroupBox1.Controls.Add(Me.chkOrderbyId)
        Me.GroupBox1.Controls.Add(Me.chkOnlyApproval)
        Me.GroupBox1.Controls.Add(Me.chkNetWt)
        Me.GroupBox1.Controls.Add(Me.Panel1)
        Me.GroupBox1.Controls.Add(Me.chkTransactionOnly)
        Me.GroupBox1.Controls.Add(Me.chkCompanySelectAll)
        Me.GroupBox1.Controls.Add(Me.chkLstCompany)
        Me.GroupBox1.Controls.Add(Me.pnlDetailedSummary)
        Me.GroupBox1.Controls.Add(Me.chkAllCostCentre)
        Me.GroupBox1.Controls.Add(Me.chkAllItemType)
        Me.GroupBox1.Controls.Add(Me.chkAllCounter)
        Me.GroupBox1.Controls.Add(Me.dtpAsOnDate)
        Me.GroupBox1.Controls.Add(Me.chkOnlyTag)
        Me.GroupBox1.Controls.Add(Me.chkWithSubItem)
        Me.GroupBox1.Controls.Add(Me.chkWithApproval)
        Me.GroupBox1.Controls.Add(Me.cmbItemName)
        Me.GroupBox1.Controls.Add(Me.Panel2)
        Me.GroupBox1.Controls.Add(Me.chkLstCostCentre)
        Me.GroupBox1.Controls.Add(Me.chkLstItemType)
        Me.GroupBox1.Controls.Add(Me.chkLstCounter)
        Me.GroupBox1.Controls.Add(Me.chkGrsWt)
        Me.GroupBox1.Controls.Add(Me.cmbMetal)
        Me.GroupBox1.Controls.Add(Me.cmbCategory)
        Me.GroupBox1.Controls.Add(Me.cmbDesigner)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.label10)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.AsOnDate)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(422, 536)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'chkSeperateTransferCol
        '
        Me.chkSeperateTransferCol.AutoSize = True
        Me.chkSeperateTransferCol.Checked = True
        Me.chkSeperateTransferCol.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSeperateTransferCol.Location = New System.Drawing.Point(242, 482)
        Me.chkSeperateTransferCol.Name = "chkSeperateTransferCol"
        Me.chkSeperateTransferCol.Size = New System.Drawing.Size(178, 17)
        Me.chkSeperateTransferCol.TabIndex = 29
        Me.chkSeperateTransferCol.Text = "Seperate Transfer Column"
        Me.chkSeperateTransferCol.UseVisualStyleBackColor = True
        '
        'chkOrderbyId
        '
        Me.chkOrderbyId.AutoSize = True
        Me.chkOrderbyId.Location = New System.Drawing.Point(242, 517)
        Me.chkOrderbyId.Name = "chkOrderbyId"
        Me.chkOrderbyId.Size = New System.Drawing.Size(93, 17)
        Me.chkOrderbyId.TabIndex = 33
        Me.chkOrderbyId.Text = "Order by Id"
        Me.chkOrderbyId.UseVisualStyleBackColor = True
        '
        'chkOnlyApproval
        '
        Me.chkOnlyApproval.AutoSize = True
        Me.chkOnlyApproval.Location = New System.Drawing.Point(242, 499)
        Me.chkOnlyApproval.Name = "chkOnlyApproval"
        Me.chkOnlyApproval.Size = New System.Drawing.Size(107, 17)
        Me.chkOnlyApproval.TabIndex = 31
        Me.chkOnlyApproval.Text = "Only Approval"
        Me.chkOnlyApproval.UseVisualStyleBackColor = True
        '
        'chkNetWt
        '
        Me.chkNetWt.AutoSize = True
        Me.chkNetWt.Location = New System.Drawing.Point(242, 464)
        Me.chkNetWt.Name = "chkNetWt"
        Me.chkNetWt.Size = New System.Drawing.Size(64, 17)
        Me.chkNetWt.TabIndex = 27
        Me.chkNetWt.Text = "Net Wt"
        Me.chkNetWt.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbtWithClosing)
        Me.Panel1.Controls.Add(Me.rbtWithOpening)
        Me.Panel1.Controls.Add(Me.rbtWithBoth)
        Me.Panel1.Location = New System.Drawing.Point(122, 437)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(226, 23)
        Me.Panel1.TabIndex = 24
        '
        'rbtWithClosing
        '
        Me.rbtWithClosing.AutoSize = True
        Me.rbtWithClosing.Location = New System.Drawing.Point(154, 3)
        Me.rbtWithClosing.Name = "rbtWithClosing"
        Me.rbtWithClosing.Size = New System.Drawing.Size(67, 17)
        Me.rbtWithClosing.TabIndex = 2
        Me.rbtWithClosing.Text = "Closing"
        Me.rbtWithClosing.UseVisualStyleBackColor = True
        '
        'rbtWithOpening
        '
        Me.rbtWithOpening.AutoSize = True
        Me.rbtWithOpening.Location = New System.Drawing.Point(73, 3)
        Me.rbtWithOpening.Name = "rbtWithOpening"
        Me.rbtWithOpening.Size = New System.Drawing.Size(72, 17)
        Me.rbtWithOpening.TabIndex = 1
        Me.rbtWithOpening.Text = "Opening"
        Me.rbtWithOpening.UseVisualStyleBackColor = True
        '
        'rbtWithBoth
        '
        Me.rbtWithBoth.AutoSize = True
        Me.rbtWithBoth.Checked = True
        Me.rbtWithBoth.Location = New System.Drawing.Point(3, 3)
        Me.rbtWithBoth.Name = "rbtWithBoth"
        Me.rbtWithBoth.Size = New System.Drawing.Size(51, 17)
        Me.rbtWithBoth.TabIndex = 0
        Me.rbtWithBoth.TabStop = True
        Me.rbtWithBoth.Text = "Both"
        Me.rbtWithBoth.UseVisualStyleBackColor = True
        '
        'chkTransactionOnly
        '
        Me.chkTransactionOnly.AutoSize = True
        Me.chkTransactionOnly.Location = New System.Drawing.Point(122, 517)
        Me.chkTransactionOnly.Name = "chkTransactionOnly"
        Me.chkTransactionOnly.Size = New System.Drawing.Size(122, 17)
        Me.chkTransactionOnly.TabIndex = 32
        Me.chkTransactionOnly.Text = "Transaction Only"
        Me.chkTransactionOnly.UseVisualStyleBackColor = True
        '
        'chkCompanySelectAll
        '
        Me.chkCompanySelectAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkCompanySelectAll.Location = New System.Drawing.Point(6, 152)
        Me.chkCompanySelectAll.Name = "chkCompanySelectAll"
        Me.chkCompanySelectAll.Size = New System.Drawing.Size(110, 21)
        Me.chkCompanySelectAll.TabIndex = 10
        Me.chkCompanySelectAll.Text = "Company"
        Me.chkCompanySelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCompany
        '
        Me.chkLstCompany.FormattingEnabled = True
        Me.chkLstCompany.Location = New System.Drawing.Point(122, 152)
        Me.chkLstCompany.Name = "chkLstCompany"
        Me.chkLstCompany.Size = New System.Drawing.Size(290, 36)
        Me.chkLstCompany.TabIndex = 11
        '
        'pnlDetailedSummary
        '
        Me.pnlDetailedSummary.Controls.Add(Me.rbtSummary)
        Me.pnlDetailedSummary.Controls.Add(Me.rbtDetailed)
        Me.pnlDetailedSummary.Location = New System.Drawing.Point(122, 375)
        Me.pnlDetailedSummary.Name = "pnlDetailedSummary"
        Me.pnlDetailedSummary.Size = New System.Drawing.Size(290, 22)
        Me.pnlDetailedSummary.TabIndex = 21
        '
        'rbtSummary
        '
        Me.rbtSummary.AutoSize = True
        Me.rbtSummary.Location = New System.Drawing.Point(81, 3)
        Me.rbtSummary.Name = "rbtSummary"
        Me.rbtSummary.Size = New System.Drawing.Size(81, 17)
        Me.rbtSummary.TabIndex = 1
        Me.rbtSummary.TabStop = True
        Me.rbtSummary.Text = "Summary"
        Me.rbtSummary.UseVisualStyleBackColor = True
        '
        'rbtDetailed
        '
        Me.rbtDetailed.AutoSize = True
        Me.rbtDetailed.Location = New System.Drawing.Point(3, 3)
        Me.rbtDetailed.Name = "rbtDetailed"
        Me.rbtDetailed.Size = New System.Drawing.Size(72, 17)
        Me.rbtDetailed.TabIndex = 0
        Me.rbtDetailed.TabStop = True
        Me.rbtDetailed.Text = "Detailed"
        Me.rbtDetailed.UseVisualStyleBackColor = True
        '
        'chkAllCostCentre
        '
        Me.chkAllCostCentre.AutoSize = True
        Me.chkAllCostCentre.Location = New System.Drawing.Point(101, 319)
        Me.chkAllCostCentre.Name = "chkAllCostCentre"
        Me.chkAllCostCentre.Size = New System.Drawing.Size(15, 14)
        Me.chkAllCostCentre.TabIndex = 19
        Me.chkAllCostCentre.UseVisualStyleBackColor = True
        '
        'chkAllItemType
        '
        Me.chkAllItemType.AutoSize = True
        Me.chkAllItemType.Location = New System.Drawing.Point(101, 261)
        Me.chkAllItemType.Name = "chkAllItemType"
        Me.chkAllItemType.Size = New System.Drawing.Size(15, 14)
        Me.chkAllItemType.TabIndex = 16
        Me.chkAllItemType.UseVisualStyleBackColor = True
        '
        'chkAllCounter
        '
        Me.chkAllCounter.AutoSize = True
        Me.chkAllCounter.Location = New System.Drawing.Point(101, 204)
        Me.chkAllCounter.Name = "chkAllCounter"
        Me.chkAllCounter.Size = New System.Drawing.Size(15, 14)
        Me.chkAllCounter.TabIndex = 13
        Me.chkAllCounter.UseVisualStyleBackColor = True
        '
        'dtpAsOnDate
        '
        Me.dtpAsOnDate.Location = New System.Drawing.Point(122, 98)
        Me.dtpAsOnDate.Mask = "##/##/####"
        Me.dtpAsOnDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpAsOnDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpAsOnDate.Name = "dtpAsOnDate"
        Me.dtpAsOnDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpAsOnDate.Size = New System.Drawing.Size(93, 21)
        Me.dtpAsOnDate.TabIndex = 7
        Me.dtpAsOnDate.Text = "07/03/9998"
        Me.dtpAsOnDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'chkOnlyTag
        '
        Me.chkOnlyTag.AutoSize = True
        Me.chkOnlyTag.Location = New System.Drawing.Point(306, 547)
        Me.chkOnlyTag.Name = "chkOnlyTag"
        Me.chkOnlyTag.Size = New System.Drawing.Size(77, 17)
        Me.chkOnlyTag.TabIndex = 27
        Me.chkOnlyTag.Text = "Only Tag"
        Me.chkOnlyTag.UseVisualStyleBackColor = True
        Me.chkOnlyTag.Visible = False
        '
        'chkWithSubItem
        '
        Me.chkWithSubItem.AutoSize = True
        Me.chkWithSubItem.Location = New System.Drawing.Point(122, 482)
        Me.chkWithSubItem.Name = "chkWithSubItem"
        Me.chkWithSubItem.Size = New System.Drawing.Size(104, 17)
        Me.chkWithSubItem.TabIndex = 28
        Me.chkWithSubItem.Text = "With SubItem"
        Me.chkWithSubItem.UseVisualStyleBackColor = True
        '
        'chkWithApproval
        '
        Me.chkWithApproval.AutoSize = True
        Me.chkWithApproval.Checked = True
        Me.chkWithApproval.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkWithApproval.Location = New System.Drawing.Point(122, 499)
        Me.chkWithApproval.Name = "chkWithApproval"
        Me.chkWithApproval.Size = New System.Drawing.Size(106, 17)
        Me.chkWithApproval.TabIndex = 30
        Me.chkWithApproval.Text = "With Approval"
        Me.chkWithApproval.UseVisualStyleBackColor = True
        '
        'cmbItemName
        '
        Me.cmbItemName.FormattingEnabled = True
        Me.cmbItemName.Location = New System.Drawing.Point(122, 71)
        Me.cmbItemName.Name = "cmbItemName"
        Me.cmbItemName.Size = New System.Drawing.Size(290, 21)
        Me.cmbItemName.TabIndex = 5
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rbtNonTag)
        Me.Panel2.Controls.Add(Me.rbtTag)
        Me.Panel2.Controls.Add(Me.rbtBoth)
        Me.Panel2.Location = New System.Drawing.Point(122, 406)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(290, 28)
        Me.Panel2.TabIndex = 23
        '
        'rbtNonTag
        '
        Me.rbtNonTag.AutoSize = True
        Me.rbtNonTag.Location = New System.Drawing.Point(154, 6)
        Me.rbtNonTag.Name = "rbtNonTag"
        Me.rbtNonTag.Size = New System.Drawing.Size(72, 17)
        Me.rbtNonTag.TabIndex = 2
        Me.rbtNonTag.Text = "Non Tag"
        Me.rbtNonTag.UseVisualStyleBackColor = True
        '
        'rbtTag
        '
        Me.rbtTag.AutoSize = True
        Me.rbtTag.Location = New System.Drawing.Point(73, 6)
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
        Me.rbtBoth.Location = New System.Drawing.Point(3, 6)
        Me.rbtBoth.Name = "rbtBoth"
        Me.rbtBoth.Size = New System.Drawing.Size(51, 17)
        Me.rbtBoth.TabIndex = 0
        Me.rbtBoth.TabStop = True
        Me.rbtBoth.Text = "Both"
        Me.rbtBoth.UseVisualStyleBackColor = True
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(122, 317)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(290, 52)
        Me.chkLstCostCentre.TabIndex = 20
        '
        'chkLstItemType
        '
        Me.chkLstItemType.FormattingEnabled = True
        Me.chkLstItemType.Location = New System.Drawing.Point(122, 259)
        Me.chkLstItemType.Name = "chkLstItemType"
        Me.chkLstItemType.Size = New System.Drawing.Size(290, 52)
        Me.chkLstItemType.TabIndex = 17
        '
        'chkLstCounter
        '
        Me.chkLstCounter.FormattingEnabled = True
        Me.chkLstCounter.Location = New System.Drawing.Point(122, 201)
        Me.chkLstCounter.Name = "chkLstCounter"
        Me.chkLstCounter.Size = New System.Drawing.Size(290, 52)
        Me.chkLstCounter.TabIndex = 14
        '
        'chkGrsWt
        '
        Me.chkGrsWt.AutoSize = True
        Me.chkGrsWt.Checked = True
        Me.chkGrsWt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkGrsWt.Location = New System.Drawing.Point(122, 466)
        Me.chkGrsWt.Name = "chkGrsWt"
        Me.chkGrsWt.Size = New System.Drawing.Size(78, 17)
        Me.chkGrsWt.TabIndex = 26
        Me.chkGrsWt.Text = "Gross Wt"
        Me.chkGrsWt.UseVisualStyleBackColor = True
        '
        'cmbMetal
        '
        Me.cmbMetal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(122, 17)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(290, 21)
        Me.cmbMetal.TabIndex = 1
        '
        'cmbCategory
        '
        Me.cmbCategory.FormattingEnabled = True
        Me.cmbCategory.Location = New System.Drawing.Point(122, 44)
        Me.cmbCategory.Name = "cmbCategory"
        Me.cmbCategory.Size = New System.Drawing.Size(290, 21)
        Me.cmbCategory.TabIndex = 3
        '
        'cmbDesigner
        '
        Me.cmbDesigner.FormattingEnabled = True
        Me.cmbDesigner.Location = New System.Drawing.Point(122, 125)
        Me.cmbDesigner.Name = "cmbDesigner"
        Me.cmbDesigner.Size = New System.Drawing.Size(290, 21)
        Me.cmbDesigner.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(6, 464)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 21)
        Me.Label1.TabIndex = 25
        Me.Label1.Text = "Piece(s) With"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'label10
        '
        Me.label10.Location = New System.Drawing.Point(6, 17)
        Me.label10.Name = "label10"
        Me.label10.Size = New System.Drawing.Size(100, 21)
        Me.label10.TabIndex = 0
        Me.label10.Text = "Metal"
        Me.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(6, 71)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 21)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "ItemName"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(6, 44)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 21)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Category"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label
        '
        Me.Label.Location = New System.Drawing.Point(6, 317)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(100, 21)
        Me.Label.TabIndex = 18
        Me.Label.Text = "Cost Centre"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(6, 259)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(100, 21)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "Item Type"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(6, 201)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(100, 21)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "Counter Name"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(6, 413)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(100, 21)
        Me.Label5.TabIndex = 22
        Me.Label5.Text = "Selection Type"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(6, 125)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(100, 21)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "Designer"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'AsOnDate
        '
        Me.AsOnDate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AsOnDate.Location = New System.Drawing.Point(6, 98)
        Me.AsOnDate.Name = "AsOnDate"
        Me.AsOnDate.Size = New System.Drawing.Size(100, 21)
        Me.AsOnDate.TabIndex = 6
        Me.AsOnDate.Text = "AsOnDate"
        Me.AsOnDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnNew
        '
        Me.btnNew.ContextMenuStrip = Me.ContextMenuStrip1
        Me.btnNew.Location = New System.Drawing.Point(114, 546)
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
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(220, 546)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 3
        Me.btnExit.Text = "&Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(8, 546)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 1
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'chkAll
        '
        Me.chkAll.AutoSize = True
        Me.chkAll.Location = New System.Drawing.Point(733, 545)
        Me.chkAll.Name = "chkAll"
        Me.chkAll.Size = New System.Drawing.Size(146, 17)
        Me.chkAll.TabIndex = 28
        Me.chkAll.Text = "All [With Zero Stock]"
        Me.chkAll.UseVisualStyleBackColor = True
        Me.chkAll.Visible = False
        '
        'rbtOrder
        '
        Me.rbtOrder.AutoSize = True
        Me.rbtOrder.Location = New System.Drawing.Point(959, 555)
        Me.rbtOrder.Name = "rbtOrder"
        Me.rbtOrder.Size = New System.Drawing.Size(58, 17)
        Me.rbtOrder.TabIndex = 40
        Me.rbtOrder.Text = "Order"
        Me.rbtOrder.UseVisualStyleBackColor = True
        Me.rbtOrder.Visible = False
        '
        'chkWithStudded
        '
        Me.chkWithStudded.AutoSize = True
        Me.chkWithStudded.Location = New System.Drawing.Point(733, 571)
        Me.chkWithStudded.Name = "chkWithStudded"
        Me.chkWithStudded.Size = New System.Drawing.Size(145, 17)
        Me.chkWithStudded.TabIndex = 31
        Me.chkWithStudded.Text = "With Studded Details"
        Me.chkWithStudded.UseVisualStyleBackColor = True
        Me.chkWithStudded.Visible = False
        '
        'chkWithNegativeStock
        '
        Me.chkWithNegativeStock.AutoSize = True
        Me.chkWithNegativeStock.Checked = True
        Me.chkWithNegativeStock.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkWithNegativeStock.Location = New System.Drawing.Point(887, 545)
        Me.chkWithNegativeStock.Name = "chkWithNegativeStock"
        Me.chkWithNegativeStock.Size = New System.Drawing.Size(111, 17)
        Me.chkWithNegativeStock.TabIndex = 29
        Me.chkWithNegativeStock.Text = "With -Ve Stock"
        Me.chkWithNegativeStock.UseVisualStyleBackColor = True
        Me.chkWithNegativeStock.Visible = False
        '
        'rbtRegular
        '
        Me.rbtRegular.AutoSize = True
        Me.rbtRegular.Location = New System.Drawing.Point(889, 555)
        Me.rbtRegular.Name = "rbtRegular"
        Me.rbtRegular.Size = New System.Drawing.Size(69, 17)
        Me.rbtRegular.TabIndex = 39
        Me.rbtRegular.Text = "Regular"
        Me.rbtRegular.UseVisualStyleBackColor = True
        Me.rbtRegular.Visible = False
        '
        'rbtAll
        '
        Me.rbtAll.AutoSize = True
        Me.rbtAll.Checked = True
        Me.rbtAll.Location = New System.Drawing.Point(850, 555)
        Me.rbtAll.Name = "rbtAll"
        Me.rbtAll.Size = New System.Drawing.Size(39, 17)
        Me.rbtAll.TabIndex = 38
        Me.rbtAll.TabStop = True
        Me.rbtAll.Text = "All"
        Me.rbtAll.UseVisualStyleBackColor = True
        Me.rbtAll.Visible = False
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
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
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
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        'CounterWiseStock
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
        Me.Name = "CounterWiseStock_old"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Counter Wise Stock"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.tabMain.ResumeLayout(False)
        Me.tabGen.ResumeLayout(False)
        Me.tabGen.PerformLayout()
        Me.pnlGroupFilter.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlDetailedSummary.ResumeLayout(False)
        Me.pnlDetailedSummary.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
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
    Friend WithEvents cmbDesigner As System.Windows.Forms.ComboBox
    Friend WithEvents Label As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents AsOnDate As System.Windows.Forms.Label
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents chkNetWt As System.Windows.Forms.CheckBox
    Friend WithEvents chkGrsWt As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkAll As System.Windows.Forms.CheckBox
    Friend WithEvents cmbMetal As System.Windows.Forms.ComboBox
    Friend WithEvents cmbCategory As System.Windows.Forms.ComboBox
    Friend WithEvents label10 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkLstCounter As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkLstItemType As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkLstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rbtNonTag As System.Windows.Forms.RadioButton
    Friend WithEvents rbtTag As System.Windows.Forms.RadioButton
    Friend WithEvents rbtBoth As System.Windows.Forms.RadioButton
    Friend WithEvents chkWithStudded As System.Windows.Forms.CheckBox
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
    Friend WithEvents cmbItemName As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents chkOnlyApproval As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithApproval As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithSubItem As System.Windows.Forms.CheckBox
    Friend WithEvents chkOnlyTag As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithNegativeStock As System.Windows.Forms.CheckBox
    Friend WithEvents dtpAsOnDate As BrighttechPack.DatePicker
    Friend WithEvents chkAllCostCentre As System.Windows.Forms.CheckBox
    Friend WithEvents chkAllItemType As System.Windows.Forms.CheckBox
    Friend WithEvents chkAllCounter As System.Windows.Forms.CheckBox
    Friend WithEvents chkOrderbyId As System.Windows.Forms.CheckBox
    Friend WithEvents pnlDetailedSummary As System.Windows.Forms.Panel
    Friend WithEvents rbtSummary As System.Windows.Forms.RadioButton
    Friend WithEvents rbtDetailed As System.Windows.Forms.RadioButton
    Friend WithEvents rbtOrder As System.Windows.Forms.RadioButton
    Friend WithEvents rbtRegular As System.Windows.Forms.RadioButton
    Friend WithEvents rbtAll As System.Windows.Forms.RadioButton
    Friend WithEvents chkCompanySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCompany As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkSeperateTransferCol As System.Windows.Forms.CheckBox
    Friend WithEvents chkTransactionOnly As System.Windows.Forms.CheckBox
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rbtWithClosing As System.Windows.Forms.RadioButton
    Friend WithEvents rbtWithOpening As System.Windows.Forms.RadioButton
    Friend WithEvents rbtWithBoth As System.Windows.Forms.RadioButton
End Class
