<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCategoryStock
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
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.grpMain = New System.Windows.Forms.GroupBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.rbtExem = New System.Windows.Forms.RadioButton()
        Me.rbtAll = New System.Windows.Forms.RadioButton()
        Me.rbtMfg = New System.Windows.Forms.RadioButton()
        Me.rbtTrading = New System.Windows.Forms.RadioButton()
        Me.chkalloy = New System.Windows.Forms.CheckBox()
        Me.chkWithApproval = New System.Windows.Forms.CheckBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox()
        Me.Label = New System.Windows.Forms.Label()
        Me.chkCmbCompany = New BrighttechPack.CheckedComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.rbtMetalDateWise = New System.Windows.Forms.RadioButton()
        Me.rbtStkType = New System.Windows.Forms.RadioButton()
        Me.rbtSummary = New System.Windows.Forms.RadioButton()
        Me.rbtDateWise = New System.Windows.Forms.RadioButton()
        Me.rbtMonthWise = New System.Windows.Forms.RadioButton()
        Me.rbtTranNoWise = New System.Windows.Forms.RadioButton()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.rbtCarat = New System.Windows.Forms.RadioButton()
        Me.rbtGeneral = New System.Windows.Forms.RadioButton()
        Me.rbtGram = New System.Windows.Forms.RadioButton()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.rbtGrsNetwt = New System.Windows.Forms.RadioButton()
        Me.rbtPurewt = New System.Windows.Forms.RadioButton()
        Me.rbtGrsWt = New System.Windows.Forms.RadioButton()
        Me.rbtNetWt = New System.Windows.Forms.RadioButton()
        Me.cmbMetalType = New System.Windows.Forms.ComboBox()
        Me.cmbCatName = New System.Windows.Forms.ComboBox()
        Me.cmbMetal = New System.Windows.Forms.ComboBox()
        Me.chkWithValue = New System.Windows.Forms.CheckBox()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.chkWithOtherIssue = New System.Windows.Forms.CheckBox()
        Me.chkWithStuddedStone = New System.Windows.Forms.CheckBox()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pnlView = New System.Windows.Forms.Panel()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ReziseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlHeading = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.grpMain.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pnlView.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.pnlHeading.SuspendLayout()
        Me.SuspendLayout()
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
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'grpMain
        '
        Me.grpMain.Controls.Add(Me.Label10)
        Me.grpMain.Controls.Add(Me.Panel4)
        Me.grpMain.Controls.Add(Me.chkalloy)
        Me.grpMain.Controls.Add(Me.chkWithApproval)
        Me.grpMain.Controls.Add(Me.btnSave)
        Me.grpMain.Controls.Add(Me.chkCmbCostCentre)
        Me.grpMain.Controls.Add(Me.Label)
        Me.grpMain.Controls.Add(Me.chkCmbCompany)
        Me.grpMain.Controls.Add(Me.Label9)
        Me.grpMain.Controls.Add(Me.dtpTo)
        Me.grpMain.Controls.Add(Me.dtpFrom)
        Me.grpMain.Controls.Add(Me.btnPrint)
        Me.grpMain.Controls.Add(Me.btnExport)
        Me.grpMain.Controls.Add(Me.Panel3)
        Me.grpMain.Controls.Add(Me.Panel2)
        Me.grpMain.Controls.Add(Me.Panel1)
        Me.grpMain.Controls.Add(Me.cmbMetalType)
        Me.grpMain.Controls.Add(Me.cmbCatName)
        Me.grpMain.Controls.Add(Me.cmbMetal)
        Me.grpMain.Controls.Add(Me.chkWithValue)
        Me.grpMain.Controls.Add(Me.btnExit)
        Me.grpMain.Controls.Add(Me.btnNew)
        Me.grpMain.Controls.Add(Me.Label6)
        Me.grpMain.Controls.Add(Me.chkWithOtherIssue)
        Me.grpMain.Controls.Add(Me.chkWithStuddedStone)
        Me.grpMain.Controls.Add(Me.btnView_Search)
        Me.grpMain.Controls.Add(Me.Label7)
        Me.grpMain.Controls.Add(Me.Label4)
        Me.grpMain.Controls.Add(Me.Label3)
        Me.grpMain.Controls.Add(Me.Label8)
        Me.grpMain.Controls.Add(Me.Label2)
        Me.grpMain.Controls.Add(Me.Label5)
        Me.grpMain.Controls.Add(Me.Label1)
        Me.grpMain.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpMain.Location = New System.Drawing.Point(0, 0)
        Me.grpMain.Name = "grpMain"
        Me.grpMain.Size = New System.Drawing.Size(1028, 181)
        Me.grpMain.TabIndex = 0
        Me.grpMain.TabStop = False
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(5, 159)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(115, 21)
        Me.Label10.TabIndex = 14
        Me.Label10.Text = "Stock Type"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.rbtExem)
        Me.Panel4.Controls.Add(Me.rbtAll)
        Me.Panel4.Controls.Add(Me.rbtMfg)
        Me.Panel4.Controls.Add(Me.rbtTrading)
        Me.Panel4.Location = New System.Drawing.Point(123, 160)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(335, 21)
        Me.Panel4.TabIndex = 15
        '
        'rbtExem
        '
        Me.rbtExem.AutoSize = True
        Me.rbtExem.Location = New System.Drawing.Point(198, 2)
        Me.rbtExem.Name = "rbtExem"
        Me.rbtExem.Size = New System.Drawing.Size(82, 17)
        Me.rbtExem.TabIndex = 2
        Me.rbtExem.Text = "Exempted"
        Me.rbtExem.UseVisualStyleBackColor = True
        '
        'rbtAll
        '
        Me.rbtAll.AutoSize = True
        Me.rbtAll.Checked = True
        Me.rbtAll.Location = New System.Drawing.Point(291, 2)
        Me.rbtAll.Name = "rbtAll"
        Me.rbtAll.Size = New System.Drawing.Size(39, 17)
        Me.rbtAll.TabIndex = 3
        Me.rbtAll.TabStop = True
        Me.rbtAll.Text = "All"
        Me.rbtAll.UseVisualStyleBackColor = True
        '
        'rbtMfg
        '
        Me.rbtMfg.AutoSize = True
        Me.rbtMfg.Location = New System.Drawing.Point(4, 2)
        Me.rbtMfg.Name = "rbtMfg"
        Me.rbtMfg.Size = New System.Drawing.Size(105, 17)
        Me.rbtMfg.TabIndex = 0
        Me.rbtMfg.Text = "Manufacturing"
        Me.rbtMfg.UseVisualStyleBackColor = True
        '
        'rbtTrading
        '
        Me.rbtTrading.AutoSize = True
        Me.rbtTrading.Location = New System.Drawing.Point(109, 2)
        Me.rbtTrading.Name = "rbtTrading"
        Me.rbtTrading.Size = New System.Drawing.Size(67, 17)
        Me.rbtTrading.TabIndex = 1
        Me.rbtTrading.Text = "Trading"
        Me.rbtTrading.UseVisualStyleBackColor = True
        '
        'chkalloy
        '
        Me.chkalloy.AutoSize = True
        Me.chkalloy.Location = New System.Drawing.Point(860, 87)
        Me.chkalloy.Name = "chkalloy"
        Me.chkalloy.Size = New System.Drawing.Size(83, 17)
        Me.chkalloy.TabIndex = 26
        Me.chkalloy.Text = "With Alloy"
        Me.chkalloy.UseVisualStyleBackColor = True
        '
        'chkWithApproval
        '
        Me.chkWithApproval.AutoSize = True
        Me.chkWithApproval.Checked = True
        Me.chkWithApproval.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkWithApproval.Location = New System.Drawing.Point(698, 87)
        Me.chkWithApproval.Name = "chkWithApproval"
        Me.chkWithApproval.Size = New System.Drawing.Size(156, 17)
        Me.chkWithApproval.TabIndex = 25
        Me.chkWithApproval.Text = "With Counter Approval"
        Me.chkWithApproval.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(890, 108)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 32
        Me.btnSave.Text = "&Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(127, 61)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(222, 22)
        Me.chkCmbCostCentre.TabIndex = 7
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(6, 64)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(76, 13)
        Me.Label.TabIndex = 6
        Me.Label.Text = "Cost Centre"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbCompany
        '
        Me.chkCmbCompany.CheckOnClick = True
        Me.chkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCompany.DropDownHeight = 3
        Me.chkCmbCompany.FormattingEnabled = True
        Me.chkCmbCompany.IntegralHeight = False
        Me.chkCmbCompany.Location = New System.Drawing.Point(127, 10)
        Me.chkCmbCompany.Name = "chkCmbCompany"
        Me.chkCmbCompany.Size = New System.Drawing.Size(321, 22)
        Me.chkCmbCompany.TabIndex = 1
        Me.chkCmbCompany.ValueSeparator = ", "
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 14)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(62, 13)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Company"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(261, 36)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(88, 21)
        Me.dtpTo.TabIndex = 5
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(127, 36)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(88, 21)
        Me.dtpFrom.TabIndex = 3
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(783, 108)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 31
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(571, 108)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 29
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.rbtMetalDateWise)
        Me.Panel3.Controls.Add(Me.rbtStkType)
        Me.Panel3.Controls.Add(Me.rbtSummary)
        Me.Panel3.Controls.Add(Me.rbtDateWise)
        Me.Panel3.Controls.Add(Me.rbtMonthWise)
        Me.Panel3.Controls.Add(Me.rbtTranNoWise)
        Me.Panel3.Location = New System.Drawing.Point(445, 59)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(577, 22)
        Me.Panel3.TabIndex = 21
        '
        'rbtMetalDateWise
        '
        Me.rbtMetalDateWise.AutoSize = True
        Me.rbtMetalDateWise.Location = New System.Drawing.Point(461, 2)
        Me.rbtMetalDateWise.Name = "rbtMetalDateWise"
        Me.rbtMetalDateWise.Size = New System.Drawing.Size(87, 17)
        Me.rbtMetalDateWise.TabIndex = 5
        Me.rbtMetalDateWise.TabStop = True
        Me.rbtMetalDateWise.Text = "Metal/Date"
        Me.rbtMetalDateWise.UseVisualStyleBackColor = True
        '
        'rbtStkType
        '
        Me.rbtStkType.AutoSize = True
        Me.rbtStkType.Location = New System.Drawing.Point(372, 3)
        Me.rbtStkType.Name = "rbtStkType"
        Me.rbtStkType.Size = New System.Drawing.Size(84, 17)
        Me.rbtStkType.TabIndex = 4
        Me.rbtStkType.TabStop = True
        Me.rbtStkType.Text = "StockType"
        Me.rbtStkType.UseVisualStyleBackColor = True
        '
        'rbtSummary
        '
        Me.rbtSummary.AutoSize = True
        Me.rbtSummary.Location = New System.Drawing.Point(0, 3)
        Me.rbtSummary.Name = "rbtSummary"
        Me.rbtSummary.Size = New System.Drawing.Size(81, 17)
        Me.rbtSummary.TabIndex = 0
        Me.rbtSummary.TabStop = True
        Me.rbtSummary.Text = "Summary"
        Me.rbtSummary.UseVisualStyleBackColor = True
        '
        'rbtDateWise
        '
        Me.rbtDateWise.AutoSize = True
        Me.rbtDateWise.Location = New System.Drawing.Point(183, 3)
        Me.rbtDateWise.Name = "rbtDateWise"
        Me.rbtDateWise.Size = New System.Drawing.Size(83, 17)
        Me.rbtDateWise.TabIndex = 2
        Me.rbtDateWise.TabStop = True
        Me.rbtDateWise.Text = "Date Wise"
        Me.rbtDateWise.UseVisualStyleBackColor = True
        '
        'rbtMonthWise
        '
        Me.rbtMonthWise.AutoSize = True
        Me.rbtMonthWise.Location = New System.Drawing.Point(87, 3)
        Me.rbtMonthWise.Name = "rbtMonthWise"
        Me.rbtMonthWise.Size = New System.Drawing.Size(90, 17)
        Me.rbtMonthWise.TabIndex = 1
        Me.rbtMonthWise.TabStop = True
        Me.rbtMonthWise.Text = "Month Wise"
        Me.rbtMonthWise.UseVisualStyleBackColor = True
        '
        'rbtTranNoWise
        '
        Me.rbtTranNoWise.AutoSize = True
        Me.rbtTranNoWise.Location = New System.Drawing.Point(270, 3)
        Me.rbtTranNoWise.Name = "rbtTranNoWise"
        Me.rbtTranNoWise.Size = New System.Drawing.Size(96, 17)
        Me.rbtTranNoWise.TabIndex = 3
        Me.rbtTranNoWise.TabStop = True
        Me.rbtTranNoWise.Text = "TranNo Wise"
        Me.rbtTranNoWise.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rbtCarat)
        Me.Panel2.Controls.Add(Me.rbtGeneral)
        Me.Panel2.Controls.Add(Me.rbtGram)
        Me.Panel2.Location = New System.Drawing.Point(451, 34)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(202, 22)
        Me.Panel2.TabIndex = 19
        '
        'rbtCarat
        '
        Me.rbtCarat.AutoSize = True
        Me.rbtCarat.Location = New System.Drawing.Point(142, 3)
        Me.rbtCarat.Name = "rbtCarat"
        Me.rbtCarat.Size = New System.Drawing.Size(57, 17)
        Me.rbtCarat.TabIndex = 2
        Me.rbtCarat.TabStop = True
        Me.rbtCarat.Text = "Carat"
        Me.rbtCarat.UseVisualStyleBackColor = True
        '
        'rbtGeneral
        '
        Me.rbtGeneral.AutoSize = True
        Me.rbtGeneral.Location = New System.Drawing.Point(3, 2)
        Me.rbtGeneral.Name = "rbtGeneral"
        Me.rbtGeneral.Size = New System.Drawing.Size(70, 17)
        Me.rbtGeneral.TabIndex = 0
        Me.rbtGeneral.TabStop = True
        Me.rbtGeneral.Text = "General"
        Me.rbtGeneral.UseVisualStyleBackColor = True
        '
        'rbtGram
        '
        Me.rbtGram.AutoSize = True
        Me.rbtGram.Location = New System.Drawing.Point(79, 3)
        Me.rbtGram.Name = "rbtGram"
        Me.rbtGram.Size = New System.Drawing.Size(57, 17)
        Me.rbtGram.TabIndex = 1
        Me.rbtGram.TabStop = True
        Me.rbtGram.Text = "Gram"
        Me.rbtGram.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbtGrsNetwt)
        Me.Panel1.Controls.Add(Me.rbtPurewt)
        Me.Panel1.Controls.Add(Me.rbtGrsWt)
        Me.Panel1.Controls.Add(Me.rbtNetWt)
        Me.Panel1.Location = New System.Drawing.Point(127, 138)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(411, 22)
        Me.Panel1.TabIndex = 13
        '
        'rbtGrsNetwt
        '
        Me.rbtGrsNetwt.AutoSize = True
        Me.rbtGrsNetwt.Location = New System.Drawing.Point(287, 3)
        Me.rbtGrsNetwt.Name = "rbtGrsNetwt"
        Me.rbtGrsNetwt.Size = New System.Drawing.Size(106, 17)
        Me.rbtGrsNetwt.TabIndex = 3
        Me.rbtGrsNetwt.TabStop = True
        Me.rbtGrsNetwt.Text = "GrsNet Weight"
        Me.rbtGrsNetwt.UseVisualStyleBackColor = True
        '
        'rbtPurewt
        '
        Me.rbtPurewt.AutoSize = True
        Me.rbtPurewt.Location = New System.Drawing.Point(194, 3)
        Me.rbtPurewt.Name = "rbtPurewt"
        Me.rbtPurewt.Size = New System.Drawing.Size(93, 17)
        Me.rbtPurewt.TabIndex = 0
        Me.rbtPurewt.TabStop = True
        Me.rbtPurewt.Text = "Pure Weight"
        Me.rbtPurewt.UseVisualStyleBackColor = True
        '
        'rbtGrsWt
        '
        Me.rbtGrsWt.AutoSize = True
        Me.rbtGrsWt.Location = New System.Drawing.Point(0, 3)
        Me.rbtGrsWt.Name = "rbtGrsWt"
        Me.rbtGrsWt.Size = New System.Drawing.Size(100, 17)
        Me.rbtGrsWt.TabIndex = 1
        Me.rbtGrsWt.TabStop = True
        Me.rbtGrsWt.Text = "Gross Weight"
        Me.rbtGrsWt.UseVisualStyleBackColor = True
        '
        'rbtNetWt
        '
        Me.rbtNetWt.AutoSize = True
        Me.rbtNetWt.Location = New System.Drawing.Point(105, 3)
        Me.rbtNetWt.Name = "rbtNetWt"
        Me.rbtNetWt.Size = New System.Drawing.Size(86, 17)
        Me.rbtNetWt.TabIndex = 2
        Me.rbtNetWt.TabStop = True
        Me.rbtNetWt.Text = "Net Weight"
        Me.rbtNetWt.UseVisualStyleBackColor = True
        '
        'cmbMetalType
        '
        Me.cmbMetalType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMetalType.FormattingEnabled = True
        Me.cmbMetalType.Location = New System.Drawing.Point(127, 113)
        Me.cmbMetalType.Name = "cmbMetalType"
        Me.cmbMetalType.Size = New System.Drawing.Size(222, 21)
        Me.cmbMetalType.TabIndex = 11
        '
        'cmbCatName
        '
        Me.cmbCatName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCatName.FormattingEnabled = True
        Me.cmbCatName.Location = New System.Drawing.Point(519, 10)
        Me.cmbCatName.Name = "cmbCatName"
        Me.cmbCatName.Size = New System.Drawing.Size(390, 21)
        Me.cmbCatName.TabIndex = 17
        '
        'cmbMetal
        '
        Me.cmbMetal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(127, 88)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(222, 21)
        Me.cmbMetal.TabIndex = 9
        '
        'chkWithValue
        '
        Me.chkWithValue.AutoSize = True
        Me.chkWithValue.Location = New System.Drawing.Point(362, 87)
        Me.chkWithValue.Name = "chkWithValue"
        Me.chkWithValue.Size = New System.Drawing.Size(86, 17)
        Me.chkWithValue.TabIndex = 22
        Me.chkWithValue.Text = "With Value"
        Me.chkWithValue.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(677, 108)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 30
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(465, 108)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 28
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(224, 35)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(25, 21)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "To"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkWithOtherIssue
        '
        Me.chkWithOtherIssue.AutoSize = True
        Me.chkWithOtherIssue.Location = New System.Drawing.Point(454, 87)
        Me.chkWithOtherIssue.Name = "chkWithOtherIssue"
        Me.chkWithOtherIssue.Size = New System.Drawing.Size(93, 17)
        Me.chkWithOtherIssue.TabIndex = 23
        Me.chkWithOtherIssue.Text = "Other Issue"
        Me.chkWithOtherIssue.UseVisualStyleBackColor = True
        '
        'chkWithStuddedStone
        '
        Me.chkWithStuddedStone.AutoSize = True
        Me.chkWithStuddedStone.Location = New System.Drawing.Point(553, 87)
        Me.chkWithStuddedStone.Name = "chkWithStuddedStone"
        Me.chkWithStuddedStone.Size = New System.Drawing.Size(139, 17)
        Me.chkWithStuddedStone.TabIndex = 24
        Me.chkWithStuddedStone.Text = "With Studded Stone"
        Me.chkWithStuddedStone.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(359, 108)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 27
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(356, 60)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(82, 21)
        Me.Label7.TabIndex = 20
        Me.Label7.Text = "Group By"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(363, 35)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 21)
        Me.Label4.TabIndex = 18
        Me.Label4.Text = "Stone Unit"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(6, 112)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(115, 21)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Metal Type"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(456, 9)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(82, 21)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "Category"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(6, 86)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(115, 21)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Metal Combination"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(6, 35)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(115, 21)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Date From"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(5, 138)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(115, 21)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "Weight Based On"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnlView
        '
        Me.pnlView.Controls.Add(Me.gridView)
        Me.pnlView.Controls.Add(Me.pnlHeading)
        Me.pnlView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlView.Location = New System.Drawing.Point(0, 181)
        Me.pnlView.Name = "pnlView"
        Me.pnlView.Size = New System.Drawing.Size(1028, 415)
        Me.pnlView.TabIndex = 3
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.ContextMenuStrip = Me.ContextMenuStrip2
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 31)
        Me.gridView.Name = "gridView"
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.gridView.Size = New System.Drawing.Size(1028, 384)
        Me.gridView.TabIndex = 0
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ReziseToolStripMenuItem})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(107, 26)
        '
        'ReziseToolStripMenuItem
        '
        Me.ReziseToolStripMenuItem.CheckOnClick = True
        Me.ReziseToolStripMenuItem.Name = "ReziseToolStripMenuItem"
        Me.ReziseToolStripMenuItem.Size = New System.Drawing.Size(106, 22)
        Me.ReziseToolStripMenuItem.Text = "Rezise"
        '
        'pnlHeading
        '
        Me.pnlHeading.Controls.Add(Me.lblTitle)
        Me.pnlHeading.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeading.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeading.Name = "pnlHeading"
        Me.pnlHeading.Size = New System.Drawing.Size(1028, 31)
        Me.pnlHeading.TabIndex = 1
        '
        'lblTitle
        '
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1028, 31)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Label8"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmCategoryStock
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 596)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlView)
        Me.Controls.Add(Me.grpMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmCategoryStock"
        Me.Text = "Category Stock"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.grpMain.ResumeLayout(False)
        Me.grpMain.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlView.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip2.ResumeLayout(False)
        Me.pnlHeading.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpMain As System.Windows.Forms.GroupBox
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents chkWithStuddedStone As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithOtherIssue As System.Windows.Forms.CheckBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkWithValue As System.Windows.Forms.CheckBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents rbtTranNoWise As System.Windows.Forms.RadioButton
    Friend WithEvents rbtMonthWise As System.Windows.Forms.RadioButton
    Friend WithEvents rbtDateWise As System.Windows.Forms.RadioButton
    Friend WithEvents rbtSummary As System.Windows.Forms.RadioButton
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cmbMetal As System.Windows.Forms.ComboBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rbtCarat As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGeneral As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGram As System.Windows.Forms.RadioButton
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rbtGrsWt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtNetWt As System.Windows.Forms.RadioButton
    Friend WithEvents cmbMetalType As System.Windows.Forms.ComboBox
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents pnlView As System.Windows.Forms.Panel
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents pnlHeading As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents cmbCatName As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkCmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Label As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents chkWithApproval As System.Windows.Forms.CheckBox
    Friend WithEvents rbtPurewt As System.Windows.Forms.RadioButton
    Friend WithEvents chkalloy As System.Windows.Forms.CheckBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents rbtExem As System.Windows.Forms.RadioButton
    Friend WithEvents rbtAll As System.Windows.Forms.RadioButton
    Friend WithEvents rbtMfg As System.Windows.Forms.RadioButton
    Friend WithEvents rbtTrading As System.Windows.Forms.RadioButton
    Friend WithEvents rbtStkType As System.Windows.Forms.RadioButton
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ReziseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents rbtMetalDateWise As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGrsNetwt As RadioButton
End Class
