<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCounterwiseStockDetail
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
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.chkAll = New System.Windows.Forms.CheckBox
        Me.rbtOrder = New System.Windows.Forms.RadioButton
        Me.chkWithStudded = New System.Windows.Forms.CheckBox
        Me.chkWithNegativeStock = New System.Windows.Forms.CheckBox
        Me.rbtRegular = New System.Windows.Forms.RadioButton
        Me.rbtAll = New System.Windows.Forms.RadioButton
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.cmbCounter = New System.Windows.Forms.ComboBox
        Me.chkAsOnDate = New System.Windows.Forms.CheckBox
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.lblTo = New System.Windows.Forms.Label
        Me.chkCompanyAll = New System.Windows.Forms.CheckBox
        Me.chkListCmpny = New System.Windows.Forms.CheckedListBox
        Me.CheckBox10 = New System.Windows.Forms.CheckBox
        Me.cmbItem = New System.Windows.Forms.ComboBox
        Me.cmbMetalName = New System.Windows.Forms.ComboBox
        Me.cmbCat = New System.Windows.Forms.ComboBox
        Me.cmbDesig = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.butnNew = New System.Windows.Forms.Button
        Me.butnExit = New System.Windows.Forms.Button
        Me.btnView = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.pnlGroupFilter.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pnlDetailedSummary.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
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
        Me.chkSeperateTransferCol.Size = New System.Drawing.Size(149, 17)
        Me.chkSeperateTransferCol.TabIndex = 29
        Me.chkSeperateTransferCol.Text = "Seperate Transfer Column"
        Me.chkSeperateTransferCol.UseVisualStyleBackColor = True
        '
        'chkOrderbyId
        '
        Me.chkOrderbyId.AutoSize = True
        Me.chkOrderbyId.Location = New System.Drawing.Point(242, 517)
        Me.chkOrderbyId.Name = "chkOrderbyId"
        Me.chkOrderbyId.Size = New System.Drawing.Size(78, 17)
        Me.chkOrderbyId.TabIndex = 33
        Me.chkOrderbyId.Text = "Order by Id"
        Me.chkOrderbyId.UseVisualStyleBackColor = True
        '
        'chkOnlyApproval
        '
        Me.chkOnlyApproval.AutoSize = True
        Me.chkOnlyApproval.Location = New System.Drawing.Point(242, 499)
        Me.chkOnlyApproval.Name = "chkOnlyApproval"
        Me.chkOnlyApproval.Size = New System.Drawing.Size(92, 17)
        Me.chkOnlyApproval.TabIndex = 31
        Me.chkOnlyApproval.Text = "Only Approval"
        Me.chkOnlyApproval.UseVisualStyleBackColor = True
        '
        'chkNetWt
        '
        Me.chkNetWt.AutoSize = True
        Me.chkNetWt.Location = New System.Drawing.Point(242, 464)
        Me.chkNetWt.Name = "chkNetWt"
        Me.chkNetWt.Size = New System.Drawing.Size(60, 17)
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
        Me.rbtWithClosing.Size = New System.Drawing.Size(59, 17)
        Me.rbtWithClosing.TabIndex = 2
        Me.rbtWithClosing.Text = "Closing"
        Me.rbtWithClosing.UseVisualStyleBackColor = True
        '
        'rbtWithOpening
        '
        Me.rbtWithOpening.AutoSize = True
        Me.rbtWithOpening.Location = New System.Drawing.Point(73, 3)
        Me.rbtWithOpening.Name = "rbtWithOpening"
        Me.rbtWithOpening.Size = New System.Drawing.Size(65, 17)
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
        Me.rbtWithBoth.Size = New System.Drawing.Size(47, 17)
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
        Me.chkTransactionOnly.Size = New System.Drawing.Size(106, 17)
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
        Me.chkLstCompany.Size = New System.Drawing.Size(290, 34)
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
        Me.rbtSummary.Size = New System.Drawing.Size(68, 17)
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
        Me.rbtDetailed.Size = New System.Drawing.Size(64, 17)
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
        Me.dtpAsOnDate.Size = New System.Drawing.Size(93, 20)
        Me.dtpAsOnDate.TabIndex = 7
        Me.dtpAsOnDate.Text = "07/03/9998"
        Me.dtpAsOnDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'chkOnlyTag
        '
        Me.chkOnlyTag.AutoSize = True
        Me.chkOnlyTag.Location = New System.Drawing.Point(306, 547)
        Me.chkOnlyTag.Name = "chkOnlyTag"
        Me.chkOnlyTag.Size = New System.Drawing.Size(69, 17)
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
        Me.chkWithSubItem.Size = New System.Drawing.Size(90, 17)
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
        Me.chkWithApproval.Size = New System.Drawing.Size(93, 17)
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
        Me.rbtNonTag.Size = New System.Drawing.Size(67, 17)
        Me.rbtNonTag.TabIndex = 2
        Me.rbtNonTag.Text = "Non Tag"
        Me.rbtNonTag.UseVisualStyleBackColor = True
        '
        'rbtTag
        '
        Me.rbtTag.AutoSize = True
        Me.rbtTag.Location = New System.Drawing.Point(73, 6)
        Me.rbtTag.Name = "rbtTag"
        Me.rbtTag.Size = New System.Drawing.Size(44, 17)
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
        Me.rbtBoth.Size = New System.Drawing.Size(47, 17)
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
        Me.chkLstCostCentre.Size = New System.Drawing.Size(290, 49)
        Me.chkLstCostCentre.TabIndex = 20
        '
        'chkLstItemType
        '
        Me.chkLstItemType.FormattingEnabled = True
        Me.chkLstItemType.Location = New System.Drawing.Point(122, 259)
        Me.chkLstItemType.Name = "chkLstItemType"
        Me.chkLstItemType.Size = New System.Drawing.Size(290, 49)
        Me.chkLstItemType.TabIndex = 17
        '
        'chkLstCounter
        '
        Me.chkLstCounter.FormattingEnabled = True
        Me.chkLstCounter.Location = New System.Drawing.Point(122, 201)
        Me.chkLstCounter.Name = "chkLstCounter"
        Me.chkLstCounter.Size = New System.Drawing.Size(290, 49)
        Me.chkLstCounter.TabIndex = 14
        '
        'chkGrsWt
        '
        Me.chkGrsWt.AutoSize = True
        Me.chkGrsWt.Checked = True
        Me.chkGrsWt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkGrsWt.Location = New System.Drawing.Point(122, 466)
        Me.chkGrsWt.Name = "chkGrsWt"
        Me.chkGrsWt.Size = New System.Drawing.Size(70, 17)
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
        Me.btnNew.Location = New System.Drawing.Point(114, 546)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 2
        Me.btnNew.Text = "&New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
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
        Me.chkAll.Size = New System.Drawing.Size(124, 17)
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
        Me.rbtOrder.Size = New System.Drawing.Size(51, 17)
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
        Me.chkWithStudded.Size = New System.Drawing.Size(126, 17)
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
        Me.chkWithNegativeStock.Size = New System.Drawing.Size(98, 17)
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
        Me.rbtRegular.Size = New System.Drawing.Size(62, 17)
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
        Me.rbtAll.Size = New System.Drawing.Size(36, 17)
        Me.rbtAll.TabIndex = 38
        Me.rbtAll.TabStop = True
        Me.rbtAll.Text = "All"
        Me.rbtAll.UseVisualStyleBackColor = True
        Me.rbtAll.Visible = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.cmbCounter)
        Me.GroupBox2.Controls.Add(Me.chkAsOnDate)
        Me.GroupBox2.Controls.Add(Me.dtpTo)
        Me.GroupBox2.Controls.Add(Me.dtpFrom)
        Me.GroupBox2.Controls.Add(Me.lblTo)
        Me.GroupBox2.Controls.Add(Me.chkCompanyAll)
        Me.GroupBox2.Controls.Add(Me.chkListCmpny)
        Me.GroupBox2.Controls.Add(Me.CheckBox10)
        Me.GroupBox2.Controls.Add(Me.cmbItem)
        Me.GroupBox2.Controls.Add(Me.cmbMetalName)
        Me.GroupBox2.Controls.Add(Me.cmbCat)
        Me.GroupBox2.Controls.Add(Me.cmbDesig)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.Label11)
        Me.GroupBox2.Controls.Add(Me.Label12)
        Me.GroupBox2.Controls.Add(Me.Label15)
        Me.GroupBox2.Controls.Add(Me.Label17)
        Me.GroupBox2.Location = New System.Drawing.Point(285, 141)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(422, 308)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        '
        'cmbCounter
        '
        Me.cmbCounter.FormattingEnabled = True
        Me.cmbCounter.Location = New System.Drawing.Point(122, 257)
        Me.cmbCounter.Name = "cmbCounter"
        Me.cmbCounter.Size = New System.Drawing.Size(290, 21)
        Me.cmbCounter.TabIndex = 15
        '
        'chkAsOnDate
        '
        Me.chkAsOnDate.AutoSize = True
        Me.chkAsOnDate.Checked = True
        Me.chkAsOnDate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAsOnDate.Location = New System.Drawing.Point(9, 103)
        Me.chkAsOnDate.Name = "chkAsOnDate"
        Me.chkAsOnDate.Size = New System.Drawing.Size(81, 17)
        Me.chkAsOnDate.TabIndex = 6
        Me.chkAsOnDate.Text = "As On Date"
        Me.chkAsOnDate.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(248, 99)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 20)
        Me.dtpTo.TabIndex = 9
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(122, 99)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(93, 20)
        Me.dtpFrom.TabIndex = 7
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(221, 105)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(20, 13)
        Me.lblTo.TabIndex = 8
        Me.lblTo.Text = "To"
        Me.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCompanyAll
        '
        Me.chkCompanyAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkCompanyAll.Location = New System.Drawing.Point(6, 152)
        Me.chkCompanyAll.Name = "chkCompanyAll"
        Me.chkCompanyAll.Size = New System.Drawing.Size(110, 21)
        Me.chkCompanyAll.TabIndex = 12
        Me.chkCompanyAll.Text = "Company"
        Me.chkCompanyAll.UseVisualStyleBackColor = True
        '
        'chkListCmpny
        '
        Me.chkListCmpny.FormattingEnabled = True
        Me.chkListCmpny.Location = New System.Drawing.Point(122, 152)
        Me.chkListCmpny.Name = "chkListCmpny"
        Me.chkListCmpny.Size = New System.Drawing.Size(290, 94)
        Me.chkListCmpny.TabIndex = 13
        '
        'CheckBox10
        '
        Me.CheckBox10.AutoSize = True
        Me.CheckBox10.Location = New System.Drawing.Point(306, 547)
        Me.CheckBox10.Name = "CheckBox10"
        Me.CheckBox10.Size = New System.Drawing.Size(69, 17)
        Me.CheckBox10.TabIndex = 27
        Me.CheckBox10.Text = "Only Tag"
        Me.CheckBox10.UseVisualStyleBackColor = True
        Me.CheckBox10.Visible = False
        '
        'cmbItem
        '
        Me.cmbItem.FormattingEnabled = True
        Me.cmbItem.Location = New System.Drawing.Point(122, 71)
        Me.cmbItem.Name = "cmbItem"
        Me.cmbItem.Size = New System.Drawing.Size(290, 21)
        Me.cmbItem.TabIndex = 5
        '
        'cmbMetalName
        '
        Me.cmbMetalName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMetalName.FormattingEnabled = True
        Me.cmbMetalName.Location = New System.Drawing.Point(122, 17)
        Me.cmbMetalName.Name = "cmbMetalName"
        Me.cmbMetalName.Size = New System.Drawing.Size(290, 21)
        Me.cmbMetalName.TabIndex = 1
        '
        'cmbCat
        '
        Me.cmbCat.FormattingEnabled = True
        Me.cmbCat.Location = New System.Drawing.Point(122, 44)
        Me.cmbCat.Name = "cmbCat"
        Me.cmbCat.Size = New System.Drawing.Size(290, 21)
        Me.cmbCat.TabIndex = 3
        '
        'cmbDesig
        '
        Me.cmbDesig.FormattingEnabled = True
        Me.cmbDesig.Location = New System.Drawing.Point(122, 125)
        Me.cmbDesig.Name = "cmbDesig"
        Me.cmbDesig.Size = New System.Drawing.Size(290, 21)
        Me.cmbDesig.TabIndex = 11
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(6, 17)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(100, 21)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Metal"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(6, 71)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(100, 21)
        Me.Label11.TabIndex = 4
        Me.Label11.Text = "ItemName"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(6, 44)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(100, 21)
        Me.Label12.TabIndex = 2
        Me.Label12.Text = "Category"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label15
        '
        Me.Label15.Location = New System.Drawing.Point(6, 255)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(100, 21)
        Me.Label15.TabIndex = 14
        Me.Label15.Text = "Counter Name"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label17
        '
        Me.Label17.Location = New System.Drawing.Point(6, 125)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(100, 21)
        Me.Label17.TabIndex = 10
        Me.Label17.Text = "Designer"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'butnNew
        '
        Me.butnNew.Location = New System.Drawing.Point(451, 540)
        Me.butnNew.Name = "butnNew"
        Me.butnNew.Size = New System.Drawing.Size(100, 30)
        Me.butnNew.TabIndex = 2
        Me.butnNew.Text = "&New [F3]"
        Me.butnNew.UseVisualStyleBackColor = True
        '
        'butnExit
        '
        Me.butnExit.Location = New System.Drawing.Point(557, 540)
        Me.butnExit.Name = "butnExit"
        Me.butnExit.Size = New System.Drawing.Size(100, 30)
        Me.butnExit.TabIndex = 3
        Me.butnExit.Text = "&Exit [F12]"
        Me.butnExit.UseVisualStyleBackColor = True
        '
        'btnView
        '
        Me.btnView.Location = New System.Drawing.Point(345, 540)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(100, 30)
        Me.btnView.TabIndex = 1
        Me.btnView.Text = "&View"
        Me.btnView.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.NewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(153, 70)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'frmCounterwiseStockDetail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(992, 616)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.butnNew)
        Me.Controls.Add(Me.butnExit)
        Me.Controls.Add(Me.btnView)
        Me.KeyPreview = True
        Me.Name = "frmCounterwiseStockDetail"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CounterWise Stock Detail"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlGroupFilter.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlDetailedSummary.ResumeLayout(False)
        Me.pnlDetailedSummary.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlGroupFilter As System.Windows.Forms.Panel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents chkSeperateTransferCol As System.Windows.Forms.CheckBox
    Friend WithEvents chkOrderbyId As System.Windows.Forms.CheckBox
    Friend WithEvents chkOnlyApproval As System.Windows.Forms.CheckBox
    Friend WithEvents chkNetWt As System.Windows.Forms.CheckBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rbtWithClosing As System.Windows.Forms.RadioButton
    Friend WithEvents rbtWithOpening As System.Windows.Forms.RadioButton
    Friend WithEvents rbtWithBoth As System.Windows.Forms.RadioButton
    Friend WithEvents chkTransactionOnly As System.Windows.Forms.CheckBox
    Friend WithEvents chkCompanySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCompany As System.Windows.Forms.CheckedListBox
    Friend WithEvents pnlDetailedSummary As System.Windows.Forms.Panel
    Friend WithEvents rbtSummary As System.Windows.Forms.RadioButton
    Friend WithEvents rbtDetailed As System.Windows.Forms.RadioButton
    Friend WithEvents chkAllCostCentre As System.Windows.Forms.CheckBox
    Friend WithEvents chkAllItemType As System.Windows.Forms.CheckBox
    Friend WithEvents chkAllCounter As System.Windows.Forms.CheckBox
    Friend WithEvents dtpAsOnDate As BrighttechPack.DatePicker
    Friend WithEvents chkOnlyTag As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithSubItem As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithApproval As System.Windows.Forms.CheckBox
    Friend WithEvents cmbItemName As System.Windows.Forms.ComboBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rbtNonTag As System.Windows.Forms.RadioButton
    Friend WithEvents rbtTag As System.Windows.Forms.RadioButton
    Friend WithEvents rbtBoth As System.Windows.Forms.RadioButton
    Friend WithEvents chkLstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkLstItemType As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkLstCounter As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkGrsWt As System.Windows.Forms.CheckBox
    Friend WithEvents cmbMetal As System.Windows.Forms.ComboBox
    Friend WithEvents cmbCategory As System.Windows.Forms.ComboBox
    Friend WithEvents cmbDesigner As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents label10 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents AsOnDate As System.Windows.Forms.Label
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents chkAll As System.Windows.Forms.CheckBox
    Friend WithEvents rbtOrder As System.Windows.Forms.RadioButton
    Friend WithEvents chkWithStudded As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithNegativeStock As System.Windows.Forms.CheckBox
    Friend WithEvents rbtRegular As System.Windows.Forms.RadioButton
    Friend WithEvents rbtAll As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents chkCompanyAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkListCmpny As System.Windows.Forms.CheckedListBox
    Friend WithEvents CheckBox10 As System.Windows.Forms.CheckBox
    Friend WithEvents cmbItem As System.Windows.Forms.ComboBox
    Friend WithEvents cmbMetalName As System.Windows.Forms.ComboBox
    Friend WithEvents cmbCat As System.Windows.Forms.ComboBox
    Friend WithEvents cmbDesig As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents butnNew As System.Windows.Forms.Button
    Friend WithEvents butnExit As System.Windows.Forms.Button
    Friend WithEvents btnView As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkAsOnDate As System.Windows.Forms.CheckBox
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents cmbCounter As System.Windows.Forms.ComboBox
End Class
