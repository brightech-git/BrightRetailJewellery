<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTransactionDetailed
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cmbCostCentre = New System.Windows.Forms.ComboBox()
        Me.grp1 = New System.Windows.Forms.GroupBox()
        Me.PnlFields = New System.Windows.Forms.Panel()
        Me.chkBillPrefix = New System.Windows.Forms.CheckBox()
        Me.ChkSchemeAdjust = New System.Windows.Forms.CheckBox()
        Me.rbtNormal = New System.Windows.Forms.RadioButton()
        Me.ChkTotal = New System.Windows.Forms.CheckBox()
        Me.chkLinkPurInfo = New System.Windows.Forms.CheckBox()
        Me.rbtGrm = New System.Windows.Forms.RadioButton()
        Me.chkLstNodeId = New System.Windows.Forms.CheckedListBox()
        Me.grpWeight = New System.Windows.Forms.GroupBox()
        Me.rbtBoth = New System.Windows.Forms.RadioButton()
        Me.rbtNetWt = New System.Windows.Forms.RadioButton()
        Me.rbtGrossWt = New System.Windows.Forms.RadioButton()
        Me.chkStnSepPost = New System.Windows.Forms.CheckBox()
        Me.chksalesordcredit = New System.Windows.Forms.CheckBox()
        Me.chkchitBreakup = New System.Windows.Forms.CheckBox()
        Me.ChkAddr = New System.Windows.Forms.CheckBox()
        Me.chkTranDateSum = New System.Windows.Forms.CheckBox()
        Me.ChkWithoutGrp = New System.Windows.Forms.CheckBox()
        Me.chkLstCompany = New System.Windows.Forms.CheckedListBox()
        Me.chkGvdetails = New System.Windows.Forms.CheckBox()
        Me.ChkMiscIssue = New System.Windows.Forms.CheckBox()
        Me.ChkCancel = New System.Windows.Forms.CheckBox()
        Me.chkWithPcs = New System.Windows.Forms.CheckBox()
        Me.chkctbwdet = New System.Windows.Forms.CheckBox()
        Me.chkWithCashOpen = New System.Windows.Forms.CheckBox()
        Me.chkCostCentreSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkCompanySelectAll = New System.Windows.Forms.CheckBox()
        Me.chkWithOthers = New System.Windows.Forms.CheckBox()
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox()
        Me.chkWithAccode = New System.Windows.Forms.CheckBox()
        Me.chkCashCounterSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkSystemIdSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstCashCounter = New System.Windows.Forms.CheckedListBox()
        Me.btnSave_OWN = New System.Windows.Forms.Button()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.pnlFooter = New System.Windows.Forms.Panel()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabGen = New System.Windows.Forms.TabPage()
        Me.tabView = New System.Windows.Forms.TabPage()
        Me.Panel1.SuspendLayout()
        Me.grp1.SuspendLayout()
        Me.PnlFields.SuspendLayout()
        Me.grpWeight.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmbGridShortCut.SuspendLayout()
        Me.pnlFooter.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGen.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.cmbCostCentre)
        Me.Panel1.Controls.Add(Me.grp1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1014, 584)
        Me.Panel1.TabIndex = 0
        '
        'cmbCostCentre
        '
        Me.cmbCostCentre.FormattingEnabled = True
        Me.cmbCostCentre.Location = New System.Drawing.Point(741, 172)
        Me.cmbCostCentre.Name = "cmbCostCentre"
        Me.cmbCostCentre.Size = New System.Drawing.Size(128, 21)
        Me.cmbCostCentre.TabIndex = 0
        Me.cmbCostCentre.Visible = False
        '
        'grp1
        '
        Me.grp1.Controls.Add(Me.PnlFields)
        Me.grp1.Controls.Add(Me.btnSave_OWN)
        Me.grp1.Controls.Add(Me.dtpTo)
        Me.grp1.Controls.Add(Me.dtpFrom)
        Me.grp1.Controls.Add(Me.btnNew)
        Me.grp1.Controls.Add(Me.btnView_Search)
        Me.grp1.Controls.Add(Me.btnExit)
        Me.grp1.Controls.Add(Me.Label2)
        Me.grp1.Controls.Add(Me.Label3)
        Me.grp1.Location = New System.Drawing.Point(275, 17)
        Me.grp1.Name = "grp1"
        Me.grp1.Size = New System.Drawing.Size(445, 562)
        Me.grp1.TabIndex = 0
        Me.grp1.TabStop = False
        '
        'PnlFields
        '
        Me.PnlFields.Controls.Add(Me.chkBillPrefix)
        Me.PnlFields.Controls.Add(Me.ChkSchemeAdjust)
        Me.PnlFields.Controls.Add(Me.rbtNormal)
        Me.PnlFields.Controls.Add(Me.ChkTotal)
        Me.PnlFields.Controls.Add(Me.chkLinkPurInfo)
        Me.PnlFields.Controls.Add(Me.rbtGrm)
        Me.PnlFields.Controls.Add(Me.chkLstNodeId)
        Me.PnlFields.Controls.Add(Me.grpWeight)
        Me.PnlFields.Controls.Add(Me.chkStnSepPost)
        Me.PnlFields.Controls.Add(Me.chksalesordcredit)
        Me.PnlFields.Controls.Add(Me.chkchitBreakup)
        Me.PnlFields.Controls.Add(Me.ChkAddr)
        Me.PnlFields.Controls.Add(Me.chkTranDateSum)
        Me.PnlFields.Controls.Add(Me.ChkWithoutGrp)
        Me.PnlFields.Controls.Add(Me.chkLstCompany)
        Me.PnlFields.Controls.Add(Me.chkGvdetails)
        Me.PnlFields.Controls.Add(Me.ChkMiscIssue)
        Me.PnlFields.Controls.Add(Me.ChkCancel)
        Me.PnlFields.Controls.Add(Me.chkWithPcs)
        Me.PnlFields.Controls.Add(Me.chkctbwdet)
        Me.PnlFields.Controls.Add(Me.chkWithCashOpen)
        Me.PnlFields.Controls.Add(Me.chkCostCentreSelectAll)
        Me.PnlFields.Controls.Add(Me.chkCompanySelectAll)
        Me.PnlFields.Controls.Add(Me.chkWithOthers)
        Me.PnlFields.Controls.Add(Me.chkLstCostCentre)
        Me.PnlFields.Controls.Add(Me.chkWithAccode)
        Me.PnlFields.Controls.Add(Me.chkCashCounterSelectAll)
        Me.PnlFields.Controls.Add(Me.chkSystemIdSelectAll)
        Me.PnlFields.Controls.Add(Me.chkLstCashCounter)
        Me.PnlFields.Location = New System.Drawing.Point(13, 46)
        Me.PnlFields.Name = "PnlFields"
        Me.PnlFields.Size = New System.Drawing.Size(417, 482)
        Me.PnlFields.TabIndex = 4
        '
        'chkBillPrefix
        '
        Me.chkBillPrefix.AutoSize = True
        Me.chkBillPrefix.Location = New System.Drawing.Point(300, 458)
        Me.chkBillPrefix.Name = "chkBillPrefix"
        Me.chkBillPrefix.Size = New System.Drawing.Size(109, 17)
        Me.chkBillPrefix.TabIndex = 28
        Me.chkBillPrefix.Text = "With Bill Prefix"
        Me.chkBillPrefix.UseVisualStyleBackColor = True
        '
        'ChkSchemeAdjust
        '
        Me.ChkSchemeAdjust.AutoSize = True
        Me.ChkSchemeAdjust.Location = New System.Drawing.Point(175, 458)
        Me.ChkSchemeAdjust.Name = "ChkSchemeAdjust"
        Me.ChkSchemeAdjust.Size = New System.Drawing.Size(118, 17)
        Me.ChkSchemeAdjust.TabIndex = 27
        Me.ChkSchemeAdjust.Text = "With Chit Adjust"
        Me.ChkSchemeAdjust.UseVisualStyleBackColor = True
        '
        'rbtNormal
        '
        Me.rbtNormal.AutoSize = True
        Me.rbtNormal.Location = New System.Drawing.Point(347, 305)
        Me.rbtNormal.Name = "rbtNormal"
        Me.rbtNormal.Size = New System.Drawing.Size(66, 17)
        Me.rbtNormal.TabIndex = 13
        Me.rbtNormal.TabStop = True
        Me.rbtNormal.Text = "Normal"
        Me.rbtNormal.UseVisualStyleBackColor = True
        '
        'ChkTotal
        '
        Me.ChkTotal.AutoSize = True
        Me.ChkTotal.Checked = True
        Me.ChkTotal.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkTotal.Location = New System.Drawing.Point(300, 374)
        Me.ChkTotal.Name = "ChkTotal"
        Me.ChkTotal.Size = New System.Drawing.Size(113, 17)
        Me.ChkTotal.TabIndex = 18
        Me.ChkTotal.Text = "With SalesTotal"
        Me.ChkTotal.UseVisualStyleBackColor = True
        '
        'chkLinkPurInfo
        '
        Me.chkLinkPurInfo.AutoSize = True
        Me.chkLinkPurInfo.Location = New System.Drawing.Point(204, 372)
        Me.chkLinkPurInfo.Name = "chkLinkPurInfo"
        Me.chkLinkPurInfo.Size = New System.Drawing.Size(99, 17)
        Me.chkLinkPurInfo.TabIndex = 17
        Me.chkLinkPurInfo.Text = "Link Pur Info"
        Me.chkLinkPurInfo.UseVisualStyleBackColor = True
        '
        'rbtGrm
        '
        Me.rbtGrm.AutoSize = True
        Me.rbtGrm.Location = New System.Drawing.Point(300, 305)
        Me.rbtGrm.Name = "rbtGrm"
        Me.rbtGrm.Size = New System.Drawing.Size(50, 17)
        Me.rbtGrm.TabIndex = 12
        Me.rbtGrm.TabStop = True
        Me.rbtGrm.Text = "Grm"
        Me.rbtGrm.UseVisualStyleBackColor = True
        '
        'chkLstNodeId
        '
        Me.chkLstNodeId.FormattingEnabled = True
        Me.chkLstNodeId.Location = New System.Drawing.Point(213, 132)
        Me.chkLstNodeId.Name = "chkLstNodeId"
        Me.chkLstNodeId.Size = New System.Drawing.Size(136, 116)
        Me.chkLstNodeId.TabIndex = 5
        '
        'grpWeight
        '
        Me.grpWeight.Controls.Add(Me.rbtBoth)
        Me.grpWeight.Controls.Add(Me.rbtNetWt)
        Me.grpWeight.Controls.Add(Me.rbtGrossWt)
        Me.grpWeight.Location = New System.Drawing.Point(203, 247)
        Me.grpWeight.Name = "grpWeight"
        Me.grpWeight.Size = New System.Drawing.Size(192, 31)
        Me.grpWeight.TabIndex = 8
        Me.grpWeight.TabStop = False
        '
        'rbtBoth
        '
        Me.rbtBoth.AutoSize = True
        Me.rbtBoth.Location = New System.Drawing.Point(143, 9)
        Me.rbtBoth.Name = "rbtBoth"
        Me.rbtBoth.Size = New System.Drawing.Size(51, 17)
        Me.rbtBoth.TabIndex = 10
        Me.rbtBoth.TabStop = True
        Me.rbtBoth.Text = "Both"
        Me.rbtBoth.UseVisualStyleBackColor = True
        '
        'rbtNetWt
        '
        Me.rbtNetWt.AutoSize = True
        Me.rbtNetWt.Location = New System.Drawing.Point(81, 9)
        Me.rbtNetWt.Name = "rbtNetWt"
        Me.rbtNetWt.Size = New System.Drawing.Size(59, 17)
        Me.rbtNetWt.TabIndex = 9
        Me.rbtNetWt.TabStop = True
        Me.rbtNetWt.Text = "NetWt"
        Me.rbtNetWt.UseVisualStyleBackColor = True
        '
        'rbtGrossWt
        '
        Me.rbtGrossWt.AutoSize = True
        Me.rbtGrossWt.Location = New System.Drawing.Point(5, 9)
        Me.rbtGrossWt.Name = "rbtGrossWt"
        Me.rbtGrossWt.Size = New System.Drawing.Size(73, 17)
        Me.rbtGrossWt.TabIndex = 8
        Me.rbtGrossWt.TabStop = True
        Me.rbtGrossWt.Text = "GrossWt"
        Me.rbtGrossWt.UseVisualStyleBackColor = True
        '
        'chkStnSepPost
        '
        Me.chkStnSepPost.AutoSize = True
        Me.chkStnSepPost.Location = New System.Drawing.Point(204, 306)
        Me.chkStnSepPost.Name = "chkStnSepPost"
        Me.chkStnSepPost.Size = New System.Drawing.Size(99, 17)
        Me.chkStnSepPost.TabIndex = 11
        Me.chkStnSepPost.Text = "Stn Sep Post"
        Me.chkStnSepPost.UseVisualStyleBackColor = True
        '
        'chksalesordcredit
        '
        Me.chksalesordcredit.AutoSize = True
        Me.chksalesordcredit.Location = New System.Drawing.Point(300, 328)
        Me.chksalesordcredit.Name = "chksalesordcredit"
        Me.chksalesordcredit.Size = New System.Drawing.Size(98, 17)
        Me.chksalesordcredit.TabIndex = 15
        Me.chksalesordcredit.Text = "Order Credit"
        Me.chksalesordcredit.UseVisualStyleBackColor = True
        '
        'chkchitBreakup
        '
        Me.chkchitBreakup.AutoSize = True
        Me.chkchitBreakup.Location = New System.Drawing.Point(204, 438)
        Me.chkchitBreakup.Name = "chkchitBreakup"
        Me.chkchitBreakup.Size = New System.Drawing.Size(130, 17)
        Me.chkchitBreakup.TabIndex = 25
        Me.chkchitBreakup.Text = "With Chit Breakup"
        Me.chkchitBreakup.UseVisualStyleBackColor = True
        '
        'ChkAddr
        '
        Me.ChkAddr.AutoSize = True
        Me.ChkAddr.Location = New System.Drawing.Point(31, 458)
        Me.ChkAddr.Name = "ChkAddr"
        Me.ChkAddr.Size = New System.Drawing.Size(124, 17)
        Me.ChkAddr.TabIndex = 26
        Me.ChkAddr.Text = "With AddressInfo"
        Me.ChkAddr.UseVisualStyleBackColor = True
        '
        'chkTranDateSum
        '
        Me.chkTranDateSum.AutoSize = True
        Me.chkTranDateSum.Location = New System.Drawing.Point(204, 416)
        Me.chkTranDateSum.Name = "chkTranDateSum"
        Me.chkTranDateSum.Size = New System.Drawing.Size(173, 17)
        Me.chkTranDateSum.TabIndex = 23
        Me.chkTranDateSum.Text = "With Date Wise Summary"
        Me.chkTranDateSum.UseVisualStyleBackColor = True
        '
        'ChkWithoutGrp
        '
        Me.ChkWithoutGrp.AutoSize = True
        Me.ChkWithoutGrp.Location = New System.Drawing.Point(31, 438)
        Me.ChkWithoutGrp.Name = "ChkWithoutGrp"
        Me.ChkWithoutGrp.Size = New System.Drawing.Size(125, 17)
        Me.ChkWithoutGrp.TabIndex = 24
        Me.ChkWithoutGrp.Text = "Without Grouping"
        Me.ChkWithoutGrp.UseVisualStyleBackColor = True
        '
        'chkLstCompany
        '
        Me.chkLstCompany.FormattingEnabled = True
        Me.chkLstCompany.Location = New System.Drawing.Point(27, 19)
        Me.chkLstCompany.Name = "chkLstCompany"
        Me.chkLstCompany.Size = New System.Drawing.Size(321, 84)
        Me.chkLstCompany.TabIndex = 1
        '
        'chkGvdetails
        '
        Me.chkGvdetails.AutoSize = True
        Me.chkGvdetails.Location = New System.Drawing.Point(31, 418)
        Me.chkGvdetails.Name = "chkGvdetails"
        Me.chkGvdetails.Size = New System.Drawing.Size(139, 17)
        Me.chkGvdetails.TabIndex = 22
        Me.chkGvdetails.Text = "Gift Voucher Details"
        Me.chkGvdetails.UseVisualStyleBackColor = True
        '
        'ChkMiscIssue
        '
        Me.ChkMiscIssue.AutoSize = True
        Me.ChkMiscIssue.Location = New System.Drawing.Point(300, 397)
        Me.ChkMiscIssue.Name = "ChkMiscIssue"
        Me.ChkMiscIssue.Size = New System.Drawing.Size(110, 17)
        Me.ChkMiscIssue.TabIndex = 21
        Me.ChkMiscIssue.Text = "With MiscIssue"
        Me.ChkMiscIssue.UseVisualStyleBackColor = True
        '
        'ChkCancel
        '
        Me.ChkCancel.AutoSize = True
        Me.ChkCancel.Location = New System.Drawing.Point(204, 394)
        Me.ChkCancel.Name = "ChkCancel"
        Me.ChkCancel.Size = New System.Drawing.Size(94, 17)
        Me.ChkCancel.TabIndex = 20
        Me.ChkCancel.Text = "With Cancel"
        Me.ChkCancel.UseVisualStyleBackColor = True
        '
        'chkWithPcs
        '
        Me.chkWithPcs.AutoSize = True
        Me.chkWithPcs.Location = New System.Drawing.Point(204, 284)
        Me.chkWithPcs.Name = "chkWithPcs"
        Me.chkWithPcs.Size = New System.Drawing.Size(91, 17)
        Me.chkWithPcs.TabIndex = 9
        Me.chkWithPcs.Text = "With Pieces"
        Me.chkWithPcs.UseVisualStyleBackColor = True
        '
        'chkctbwdet
        '
        Me.chkctbwdet.AutoSize = True
        Me.chkctbwdet.Location = New System.Drawing.Point(31, 398)
        Me.chkctbwdet.Name = "chkctbwdet"
        Me.chkctbwdet.Size = New System.Drawing.Size(175, 17)
        Me.chkctbwdet.TabIndex = 19
        Me.chkctbwdet.Text = "Cash to Bank With Details"
        Me.chkctbwdet.UseVisualStyleBackColor = True
        '
        'chkWithCashOpen
        '
        Me.chkWithCashOpen.AutoSize = True
        Me.chkWithCashOpen.Location = New System.Drawing.Point(204, 350)
        Me.chkWithCashOpen.Name = "chkWithCashOpen"
        Me.chkWithCashOpen.Size = New System.Drawing.Size(135, 17)
        Me.chkWithCashOpen.TabIndex = 16
        Me.chkWithCashOpen.Text = "With Cash Opening"
        Me.chkWithCashOpen.UseVisualStyleBackColor = True
        '
        'chkCostCentreSelectAll
        '
        Me.chkCostCentreSelectAll.AutoSize = True
        Me.chkCostCentreSelectAll.Location = New System.Drawing.Point(30, 109)
        Me.chkCostCentreSelectAll.Name = "chkCostCentreSelectAll"
        Me.chkCostCentreSelectAll.Size = New System.Drawing.Size(91, 17)
        Me.chkCostCentreSelectAll.TabIndex = 2
        Me.chkCostCentreSelectAll.Text = "CostCentre"
        Me.chkCostCentreSelectAll.UseVisualStyleBackColor = True
        '
        'chkCompanySelectAll
        '
        Me.chkCompanySelectAll.AutoSize = True
        Me.chkCompanySelectAll.Location = New System.Drawing.Point(30, 0)
        Me.chkCompanySelectAll.Name = "chkCompanySelectAll"
        Me.chkCompanySelectAll.Size = New System.Drawing.Size(81, 17)
        Me.chkCompanySelectAll.TabIndex = 0
        Me.chkCompanySelectAll.Text = "Company"
        Me.chkCompanySelectAll.UseVisualStyleBackColor = True
        '
        'chkWithOthers
        '
        Me.chkWithOthers.AutoSize = True
        Me.chkWithOthers.Location = New System.Drawing.Point(204, 328)
        Me.chkWithOthers.Name = "chkWithOthers"
        Me.chkWithOthers.Size = New System.Drawing.Size(93, 17)
        Me.chkWithOthers.TabIndex = 14
        Me.chkWithOthers.Text = "With Others"
        Me.chkWithOthers.UseVisualStyleBackColor = True
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(27, 132)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(166, 116)
        Me.chkLstCostCentre.TabIndex = 3
        '
        'chkWithAccode
        '
        Me.chkWithAccode.AutoSize = True
        Me.chkWithAccode.Location = New System.Drawing.Point(300, 284)
        Me.chkWithAccode.Name = "chkWithAccode"
        Me.chkWithAccode.Size = New System.Drawing.Size(96, 17)
        Me.chkWithAccode.TabIndex = 10
        Me.chkWithAccode.Text = "With Accode"
        Me.chkWithAccode.UseVisualStyleBackColor = True
        '
        'chkCashCounterSelectAll
        '
        Me.chkCashCounterSelectAll.AutoSize = True
        Me.chkCashCounterSelectAll.Location = New System.Drawing.Point(30, 255)
        Me.chkCashCounterSelectAll.Name = "chkCashCounterSelectAll"
        Me.chkCashCounterSelectAll.Size = New System.Drawing.Size(105, 17)
        Me.chkCashCounterSelectAll.TabIndex = 6
        Me.chkCashCounterSelectAll.Text = "Cash Counter"
        Me.chkCashCounterSelectAll.UseVisualStyleBackColor = True
        '
        'chkSystemIdSelectAll
        '
        Me.chkSystemIdSelectAll.AutoSize = True
        Me.chkSystemIdSelectAll.Location = New System.Drawing.Point(216, 109)
        Me.chkSystemIdSelectAll.Name = "chkSystemIdSelectAll"
        Me.chkSystemIdSelectAll.Size = New System.Drawing.Size(81, 17)
        Me.chkSystemIdSelectAll.TabIndex = 4
        Me.chkSystemIdSelectAll.Text = "SystemId"
        Me.chkSystemIdSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCashCounter
        '
        Me.chkLstCashCounter.FormattingEnabled = True
        Me.chkLstCashCounter.Location = New System.Drawing.Point(27, 278)
        Me.chkLstCashCounter.Name = "chkLstCashCounter"
        Me.chkLstCashCounter.Size = New System.Drawing.Size(163, 116)
        Me.chkLstCashCounter.TabIndex = 7
        '
        'btnSave_OWN
        '
        Me.btnSave_OWN.Location = New System.Drawing.Point(310, 529)
        Me.btnSave_OWN.Name = "btnSave_OWN"
        Me.btnSave_OWN.Size = New System.Drawing.Size(100, 30)
        Me.btnSave_OWN.TabIndex = 8
        Me.btnSave_OWN.Text = "Save Settings"
        Me.btnSave_OWN.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(268, 17)
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
        Me.dtpFrom.Location = New System.Drawing.Point(123, 17)
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
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(110, 529)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 6
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(10, 529)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 5
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(210, 529)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 7
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(41, 21)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(67, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Date From"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(231, 20)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(20, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "To"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(844, 5)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 17
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(738, 5)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 15
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.gridView)
        Me.Panel2.Controls.Add(Me.lblTitle)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 3)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1014, 543)
        Me.Panel2.TabIndex = 0
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.ContextMenuStrip = Me.cmbGridShortCut
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 57)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 18
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(1014, 486)
        Me.gridView.TabIndex = 0
        '
        'cmbGridShortCut
        '
        Me.cmbGridShortCut.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResizeToolStripMenuItem})
        Me.cmbGridShortCut.Name = "ContextMenuStrip2"
        Me.cmbGridShortCut.Size = New System.Drawing.Size(136, 26)
        '
        'ResizeToolStripMenuItem
        '
        Me.ResizeToolStripMenuItem.CheckOnClick = True
        Me.ResizeToolStripMenuItem.Name = "ResizeToolStripMenuItem"
        Me.ResizeToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.ResizeToolStripMenuItem.Text = "Auto Resize"
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1014, 57)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Label1"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlFooter
        '
        Me.pnlFooter.Controls.Add(Me.btnBack)
        Me.pnlFooter.Controls.Add(Me.btnExport)
        Me.pnlFooter.Controls.Add(Me.btnPrint)
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Location = New System.Drawing.Point(3, 546)
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Size = New System.Drawing.Size(1014, 41)
        Me.pnlFooter.TabIndex = 1
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(632, 5)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 18
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
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
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGen)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1028, 616)
        Me.tabMain.TabIndex = 0
        '
        'tabGen
        '
        Me.tabGen.Controls.Add(Me.Panel1)
        Me.tabGen.Location = New System.Drawing.Point(4, 22)
        Me.tabGen.Name = "tabGen"
        Me.tabGen.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGen.Size = New System.Drawing.Size(1020, 590)
        Me.tabGen.TabIndex = 0
        Me.tabGen.Text = "Gen"
        Me.tabGen.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.Panel2)
        Me.tabView.Controls.Add(Me.pnlFooter)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1020, 590)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'frmTransactionDetailed
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 616)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmTransactionDetailed"
        Me.Text = "Transaction Detailed"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel1.ResumeLayout(False)
        Me.grp1.ResumeLayout(False)
        Me.grp1.PerformLayout()
        Me.PnlFields.ResumeLayout(False)
        Me.PnlFields.PerformLayout()
        Me.grpWeight.ResumeLayout(False)
        Me.grpWeight.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmbGridShortCut.ResumeLayout(False)
        Me.pnlFooter.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGen.ResumeLayout(False)
        Me.tabView.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents chkWithPcs As System.Windows.Forms.CheckBox
    Friend WithEvents rbtBoth As System.Windows.Forms.RadioButton
    Friend WithEvents rbtNetWt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGrossWt As System.Windows.Forms.RadioButton
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGen As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents grp1 As System.Windows.Forms.GroupBox
    Friend WithEvents chkLstCashCounter As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkLstNodeId As System.Windows.Forms.CheckedListBox
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkCompanySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCompany As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkLstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkSystemIdSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkCashCounterSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkCostCentreSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents cmbCostCentre As System.Windows.Forms.ComboBox
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkWithAccode As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithOthers As System.Windows.Forms.CheckBox
    Friend WithEvents chkLinkPurInfo As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithCashOpen As System.Windows.Forms.CheckBox
    Friend WithEvents chkctbwdet As System.Windows.Forms.CheckBox
    Friend WithEvents ChkTotal As System.Windows.Forms.CheckBox
    Friend WithEvents ChkMiscIssue As System.Windows.Forms.CheckBox
    Friend WithEvents ChkCancel As System.Windows.Forms.CheckBox
    Friend WithEvents chkGvdetails As System.Windows.Forms.CheckBox
    Friend WithEvents ChkWithoutGrp As System.Windows.Forms.CheckBox
    Friend WithEvents PnlFields As System.Windows.Forms.Panel
    Friend WithEvents btnSave_OWN As System.Windows.Forms.Button
    Friend WithEvents chkTranDateSum As System.Windows.Forms.CheckBox
    Friend WithEvents ChkAddr As System.Windows.Forms.CheckBox
    Friend WithEvents chkchitBreakup As System.Windows.Forms.CheckBox
    Friend WithEvents chkStnSepPost As CheckBox
    Friend WithEvents chksalesordcredit As CheckBox
    Friend WithEvents rbtNormal As RadioButton
    Friend WithEvents rbtGrm As RadioButton
    Friend WithEvents grpWeight As GroupBox
    Friend WithEvents ChkSchemeAdjust As CheckBox
    Friend WithEvents chkBillPrefix As CheckBox
End Class
