<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCustomerOutstanding
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
        Me.grbControls = New System.Windows.Forms.GroupBox()
        Me.ChKWithGst = New System.Windows.Forms.CheckBox()
        Me.ChkWithWt = New System.Windows.Forms.CheckBox()
        Me.chkgrbcc = New System.Windows.Forms.CheckBox()
        Me.ChkWithAddr = New System.Windows.Forms.CheckBox()
        Me.ChkWithSlNo = New System.Windows.Forms.CheckBox()
        Me.ChkWithRemark = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.optAllCode = New System.Windows.Forms.RadioButton()
        Me.rbtDrs = New System.Windows.Forms.RadioButton()
        Me.chkformat = New System.Windows.Forms.CheckBox()
        Me.chkCreditRpt = New System.Windows.Forms.CheckBox()
        Me.chkGroupByArea = New System.Windows.Forms.CheckBox()
        Me.grpOutput = New System.Windows.Forms.GroupBox()
        Me.rbtDetailWise = New System.Windows.Forms.RadioButton()
        Me.rbtSummaryWise = New System.Windows.Forms.RadioButton()
        Me.lblOutPut = New System.Windows.Forms.Label()
        Me.chkBasedOnAccode = New System.Windows.Forms.CheckBox()
        Me.chkCompanySelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstCompany = New System.Windows.Forms.CheckedListBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.lblOuderBy = New System.Windows.Forms.Label()
        Me.lblDisplay = New System.Windows.Forms.Label()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.grpDisplay = New System.Windows.Forms.GroupBox()
        Me.rbtReceived = New System.Windows.Forms.RadioButton()
        Me.rbtPending = New System.Windows.Forms.RadioButton()
        Me.rbtAll = New System.Windows.Forms.RadioButton()
        Me.rbtClosed = New System.Windows.Forms.RadioButton()
        Me.grpOutStanding = New System.Windows.Forms.GroupBox()
        Me.chkGiftVoucher = New System.Windows.Forms.CheckBox()
        Me.chkAdvance = New System.Windows.Forms.CheckBox()
        Me.chkCreditPurchase = New System.Windows.Forms.CheckBox()
        Me.chkToBe = New System.Windows.Forms.CheckBox()
        Me.chkCredit = New System.Windows.Forms.CheckBox()
        Me.chkOrder = New System.Windows.Forms.CheckBox()
        Me.chkGeneral = New System.Windows.Forms.CheckBox()
        Me.grpOrderBy = New System.Windows.Forms.GroupBox()
        Me.rbtOName = New System.Windows.Forms.RadioButton()
        Me.rbtORunNo = New System.Windows.Forms.RadioButton()
        Me.rbtOBillDate = New System.Windows.Forms.RadioButton()
        Me.txtNodeId = New System.Windows.Forms.TextBox()
        Me.lblNodeId = New System.Windows.Forms.Label()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.lblCostCentre = New System.Windows.Forms.Label()
        Me.cmbCostCentre = New System.Windows.Forms.ComboBox()
        Me.lblFilterCaption = New System.Windows.Forms.Label()
        Me.lblFilterBy = New System.Windows.Forms.Label()
        Me.lblDateTo = New System.Windows.Forms.Label()
        Me.lblDateFrom = New System.Windows.Forms.Label()
        Me.cmbFilterBy = New System.Windows.Forms.ComboBox()
        Me.txtFilterCaption = New System.Windows.Forms.TextBox()
        Me.lblGroupBy = New System.Windows.Forms.Label()
        Me.grpGroupBy = New System.Windows.Forms.GroupBox()
        Me.rbtGName = New System.Windows.Forms.RadioButton()
        Me.rbtGCode = New System.Windows.Forms.RadioButton()
        Me.rbtGRunNo = New System.Windows.Forms.RadioButton()
        Me.chkAll = New System.Windows.Forms.CheckBox()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlGrid = New System.Windows.Forms.Panel()
        Me.grbRunNoFocus = New System.Windows.Forms.GroupBox()
        Me.gridRunNoFocus = New System.Windows.Forms.DataGridView()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabGen = New System.Windows.Forms.TabPage()
        Me.tabView = New System.Windows.Forms.TabPage()
        Me.pnlView = New System.Windows.Forms.Panel()
        Me.pnlFooter = New System.Windows.Forms.Panel()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.pnlTitle = New System.Windows.Forms.Panel()
        Me.grbControls.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.grpOutput.SuspendLayout()
        Me.grpDisplay.SuspendLayout()
        Me.grpOutStanding.SuspendLayout()
        Me.grpOrderBy.SuspendLayout()
        Me.grpGroupBy.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.pnlGrid.SuspendLayout()
        Me.grbRunNoFocus.SuspendLayout()
        CType(Me.gridRunNoFocus, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmbGridShortCut.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGen.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.pnlView.SuspendLayout()
        Me.pnlFooter.SuspendLayout()
        Me.pnlTitle.SuspendLayout()
        Me.SuspendLayout()
        '
        'grbControls
        '
        Me.grbControls.Controls.Add(Me.ChKWithGst)
        Me.grbControls.Controls.Add(Me.ChkWithWt)
        Me.grbControls.Controls.Add(Me.chkgrbcc)
        Me.grbControls.Controls.Add(Me.ChkWithAddr)
        Me.grbControls.Controls.Add(Me.ChkWithSlNo)
        Me.grbControls.Controls.Add(Me.ChkWithRemark)
        Me.grbControls.Controls.Add(Me.GroupBox1)
        Me.grbControls.Controls.Add(Me.chkformat)
        Me.grbControls.Controls.Add(Me.chkCreditRpt)
        Me.grbControls.Controls.Add(Me.chkGroupByArea)
        Me.grbControls.Controls.Add(Me.grpOutput)
        Me.grbControls.Controls.Add(Me.lblOutPut)
        Me.grbControls.Controls.Add(Me.chkBasedOnAccode)
        Me.grbControls.Controls.Add(Me.chkCompanySelectAll)
        Me.grbControls.Controls.Add(Me.chkLstCompany)
        Me.grbControls.Controls.Add(Me.dtpTo)
        Me.grbControls.Controls.Add(Me.dtpFrom)
        Me.grbControls.Controls.Add(Me.lblOuderBy)
        Me.grbControls.Controls.Add(Me.lblDisplay)
        Me.grbControls.Controls.Add(Me.btnExit)
        Me.grbControls.Controls.Add(Me.grpDisplay)
        Me.grbControls.Controls.Add(Me.grpOutStanding)
        Me.grbControls.Controls.Add(Me.grpOrderBy)
        Me.grbControls.Controls.Add(Me.txtNodeId)
        Me.grbControls.Controls.Add(Me.lblNodeId)
        Me.grbControls.Controls.Add(Me.btnNew)
        Me.grbControls.Controls.Add(Me.btnView_Search)
        Me.grbControls.Controls.Add(Me.lblCostCentre)
        Me.grbControls.Controls.Add(Me.cmbCostCentre)
        Me.grbControls.Controls.Add(Me.lblFilterCaption)
        Me.grbControls.Controls.Add(Me.lblFilterBy)
        Me.grbControls.Controls.Add(Me.lblDateTo)
        Me.grbControls.Controls.Add(Me.lblDateFrom)
        Me.grbControls.Controls.Add(Me.cmbFilterBy)
        Me.grbControls.Controls.Add(Me.txtFilterCaption)
        Me.grbControls.Location = New System.Drawing.Point(314, 13)
        Me.grbControls.Name = "grbControls"
        Me.grbControls.Size = New System.Drawing.Size(403, 580)
        Me.grbControls.TabIndex = 0
        Me.grbControls.TabStop = False
        '
        'ChKWithGst
        '
        Me.ChKWithGst.AutoSize = True
        Me.ChKWithGst.Location = New System.Drawing.Point(102, 454)
        Me.ChKWithGst.Name = "ChKWithGst"
        Me.ChKWithGst.Size = New System.Drawing.Size(116, 17)
        Me.ChKWithGst.TabIndex = 29
        Me.ChKWithGst.Text = "With GST Detail"
        Me.ChKWithGst.UseVisualStyleBackColor = True
        '
        'ChkWithWt
        '
        Me.ChkWithWt.AutoSize = True
        Me.ChkWithWt.Location = New System.Drawing.Point(102, 433)
        Me.ChkWithWt.Name = "ChkWithWt"
        Me.ChkWithWt.Size = New System.Drawing.Size(117, 17)
        Me.ChkWithWt.TabIndex = 27
        Me.ChkWithWt.Text = "With Weight Det"
        Me.ChkWithWt.UseVisualStyleBackColor = True
        '
        'chkgrbcc
        '
        Me.chkgrbcc.AutoSize = True
        Me.chkgrbcc.Location = New System.Drawing.Point(225, 433)
        Me.chkgrbcc.Name = "chkgrbcc"
        Me.chkgrbcc.Size = New System.Drawing.Size(145, 17)
        Me.chkgrbcc.TabIndex = 28
        Me.chkgrbcc.Text = "Group by Costcentre"
        Me.chkgrbcc.UseVisualStyleBackColor = True
        '
        'ChkWithAddr
        '
        Me.ChkWithAddr.AutoSize = True
        Me.ChkWithAddr.Location = New System.Drawing.Point(225, 412)
        Me.ChkWithAddr.Name = "ChkWithAddr"
        Me.ChkWithAddr.Size = New System.Drawing.Size(101, 17)
        Me.ChkWithAddr.TabIndex = 26
        Me.ChkWithAddr.Text = "With Address"
        Me.ChkWithAddr.UseVisualStyleBackColor = True
        '
        'ChkWithSlNo
        '
        Me.ChkWithSlNo.AutoSize = True
        Me.ChkWithSlNo.Location = New System.Drawing.Point(102, 412)
        Me.ChkWithSlNo.Name = "ChkWithSlNo"
        Me.ChkWithSlNo.Size = New System.Drawing.Size(103, 17)
        Me.ChkWithSlNo.TabIndex = 25
        Me.ChkWithSlNo.Text = "With SerialNo"
        Me.ChkWithSlNo.UseVisualStyleBackColor = True
        '
        'ChkWithRemark
        '
        Me.ChkWithRemark.AutoSize = True
        Me.ChkWithRemark.Location = New System.Drawing.Point(225, 391)
        Me.ChkWithRemark.Name = "ChkWithRemark"
        Me.ChkWithRemark.Size = New System.Drawing.Size(106, 17)
        Me.ChkWithRemark.TabIndex = 24
        Me.ChkWithRemark.Text = "With Remarks"
        Me.ChkWithRemark.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.optAllCode)
        Me.GroupBox1.Controls.Add(Me.rbtDrs)
        Me.GroupBox1.Location = New System.Drawing.Point(94, 513)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(220, 28)
        Me.GroupBox1.TabIndex = 31
        Me.GroupBox1.TabStop = False
        '
        'optAllCode
        '
        Me.optAllCode.AutoSize = True
        Me.optAllCode.Location = New System.Drawing.Point(6, 9)
        Me.optAllCode.Name = "optAllCode"
        Me.optAllCode.Size = New System.Drawing.Size(73, 17)
        Me.optAllCode.TabIndex = 0
        Me.optAllCode.TabStop = True
        Me.optAllCode.Text = "All Code"
        Me.optAllCode.UseVisualStyleBackColor = True
        '
        'rbtDrs
        '
        Me.rbtDrs.AutoSize = True
        Me.rbtDrs.Location = New System.Drawing.Point(91, 9)
        Me.rbtDrs.Name = "rbtDrs"
        Me.rbtDrs.Size = New System.Drawing.Size(129, 17)
        Me.rbtDrs.TabIndex = 1
        Me.rbtDrs.TabStop = True
        Me.rbtDrs.Text = "Drs/Advance Only"
        Me.rbtDrs.UseVisualStyleBackColor = True
        '
        'chkformat
        '
        Me.chkformat.AutoSize = True
        Me.chkformat.Location = New System.Drawing.Point(225, 367)
        Me.chkformat.Name = "chkformat"
        Me.chkformat.Size = New System.Drawing.Size(73, 17)
        Me.chkformat.TabIndex = 22
        Me.chkformat.Text = "Format1"
        Me.chkformat.UseVisualStyleBackColor = True
        '
        'chkCreditRpt
        '
        Me.chkCreditRpt.AutoSize = True
        Me.chkCreditRpt.Location = New System.Drawing.Point(164, 17)
        Me.chkCreditRpt.Name = "chkCreditRpt"
        Me.chkCreditRpt.Size = New System.Drawing.Size(103, 17)
        Me.chkCreditRpt.TabIndex = 1
        Me.chkCreditRpt.Text = "Credit Report"
        Me.chkCreditRpt.UseVisualStyleBackColor = True
        '
        'chkGroupByArea
        '
        Me.chkGroupByArea.AutoSize = True
        Me.chkGroupByArea.Location = New System.Drawing.Point(102, 391)
        Me.chkGroupByArea.Name = "chkGroupByArea"
        Me.chkGroupByArea.Size = New System.Drawing.Size(110, 17)
        Me.chkGroupByArea.TabIndex = 23
        Me.chkGroupByArea.Text = "Group by Area"
        Me.chkGroupByArea.UseVisualStyleBackColor = True
        '
        'grpOutput
        '
        Me.grpOutput.Controls.Add(Me.rbtDetailWise)
        Me.grpOutput.Controls.Add(Me.rbtSummaryWise)
        Me.grpOutput.Location = New System.Drawing.Point(94, 34)
        Me.grpOutput.Name = "grpOutput"
        Me.grpOutput.Size = New System.Drawing.Size(213, 34)
        Me.grpOutput.TabIndex = 3
        Me.grpOutput.TabStop = False
        '
        'rbtDetailWise
        '
        Me.rbtDetailWise.AutoSize = True
        Me.rbtDetailWise.Location = New System.Drawing.Point(6, 14)
        Me.rbtDetailWise.Name = "rbtDetailWise"
        Me.rbtDetailWise.Size = New System.Drawing.Size(85, 17)
        Me.rbtDetailWise.TabIndex = 0
        Me.rbtDetailWise.TabStop = True
        Me.rbtDetailWise.Text = "DetailWise"
        Me.rbtDetailWise.UseVisualStyleBackColor = True
        '
        'rbtSummaryWise
        '
        Me.rbtSummaryWise.AutoSize = True
        Me.rbtSummaryWise.Location = New System.Drawing.Point(97, 14)
        Me.rbtSummaryWise.Name = "rbtSummaryWise"
        Me.rbtSummaryWise.Size = New System.Drawing.Size(108, 17)
        Me.rbtSummaryWise.TabIndex = 1
        Me.rbtSummaryWise.TabStop = True
        Me.rbtSummaryWise.Text = "SummaryWise"
        Me.rbtSummaryWise.UseVisualStyleBackColor = True
        '
        'lblOutPut
        '
        Me.lblOutPut.AutoSize = True
        Me.lblOutPut.Location = New System.Drawing.Point(12, 50)
        Me.lblOutPut.Name = "lblOutPut"
        Me.lblOutPut.Size = New System.Drawing.Size(45, 13)
        Me.lblOutPut.TabIndex = 2
        Me.lblOutPut.Text = "Output"
        '
        'chkBasedOnAccode
        '
        Me.chkBasedOnAccode.AutoSize = True
        Me.chkBasedOnAccode.Location = New System.Drawing.Point(32, 17)
        Me.chkBasedOnAccode.Name = "chkBasedOnAccode"
        Me.chkBasedOnAccode.Size = New System.Drawing.Size(126, 17)
        Me.chkBasedOnAccode.TabIndex = 0
        Me.chkBasedOnAccode.Text = "Based On Accode"
        Me.chkBasedOnAccode.UseVisualStyleBackColor = True
        '
        'chkCompanySelectAll
        '
        Me.chkCompanySelectAll.AutoSize = True
        Me.chkCompanySelectAll.Location = New System.Drawing.Point(15, 101)
        Me.chkCompanySelectAll.Name = "chkCompanySelectAll"
        Me.chkCompanySelectAll.Size = New System.Drawing.Size(81, 17)
        Me.chkCompanySelectAll.TabIndex = 8
        Me.chkCompanySelectAll.Text = "Company"
        Me.chkCompanySelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCompany
        '
        Me.chkLstCompany.FormattingEnabled = True
        Me.chkLstCompany.Location = New System.Drawing.Point(12, 121)
        Me.chkLstCompany.Name = "chkLstCompany"
        Me.chkLstCompany.Size = New System.Drawing.Size(310, 84)
        Me.chkLstCompany.TabIndex = 9
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(225, 74)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(81, 21)
        Me.dtpTo.TabIndex = 7
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(95, 74)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(81, 21)
        Me.dtpFrom.TabIndex = 5
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'lblOuderBy
        '
        Me.lblOuderBy.AutoSize = True
        Me.lblOuderBy.Location = New System.Drawing.Point(13, 254)
        Me.lblOuderBy.Name = "lblOuderBy"
        Me.lblOuderBy.Size = New System.Drawing.Size(59, 13)
        Me.lblOuderBy.TabIndex = 12
        Me.lblOuderBy.Text = "Order By"
        '
        'lblDisplay
        '
        Me.lblDisplay.AutoSize = True
        Me.lblDisplay.Location = New System.Drawing.Point(13, 216)
        Me.lblDisplay.Name = "lblDisplay"
        Me.lblDisplay.Size = New System.Drawing.Size(49, 13)
        Me.lblDisplay.TabIndex = 10
        Me.lblDisplay.Text = "Display"
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(263, 545)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(90, 30)
        Me.btnExit.TabIndex = 34
        Me.btnExit.Text = "&Exit[F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'grpDisplay
        '
        Me.grpDisplay.Controls.Add(Me.rbtReceived)
        Me.grpDisplay.Controls.Add(Me.rbtPending)
        Me.grpDisplay.Controls.Add(Me.rbtAll)
        Me.grpDisplay.Controls.Add(Me.rbtClosed)
        Me.grpDisplay.Location = New System.Drawing.Point(102, 202)
        Me.grpDisplay.Name = "grpDisplay"
        Me.grpDisplay.Size = New System.Drawing.Size(281, 35)
        Me.grpDisplay.TabIndex = 11
        Me.grpDisplay.TabStop = False
        '
        'rbtReceived
        '
        Me.rbtReceived.AutoSize = True
        Me.rbtReceived.Location = New System.Drawing.Point(141, 12)
        Me.rbtReceived.Name = "rbtReceived"
        Me.rbtReceived.Size = New System.Drawing.Size(77, 17)
        Me.rbtReceived.TabIndex = 3
        Me.rbtReceived.TabStop = True
        Me.rbtReceived.Text = "Received"
        Me.rbtReceived.UseVisualStyleBackColor = True
        '
        'rbtPending
        '
        Me.rbtPending.AutoSize = True
        Me.rbtPending.Location = New System.Drawing.Point(6, 11)
        Me.rbtPending.Name = "rbtPending"
        Me.rbtPending.Size = New System.Drawing.Size(70, 17)
        Me.rbtPending.TabIndex = 0
        Me.rbtPending.TabStop = True
        Me.rbtPending.Text = "Pending"
        Me.rbtPending.UseVisualStyleBackColor = True
        '
        'rbtAll
        '
        Me.rbtAll.AutoSize = True
        Me.rbtAll.Location = New System.Drawing.Point(221, 12)
        Me.rbtAll.Name = "rbtAll"
        Me.rbtAll.Size = New System.Drawing.Size(39, 17)
        Me.rbtAll.TabIndex = 2
        Me.rbtAll.TabStop = True
        Me.rbtAll.Text = "All"
        Me.rbtAll.UseVisualStyleBackColor = True
        '
        'rbtClosed
        '
        Me.rbtClosed.AutoSize = True
        Me.rbtClosed.Location = New System.Drawing.Point(77, 11)
        Me.rbtClosed.Name = "rbtClosed"
        Me.rbtClosed.Size = New System.Drawing.Size(64, 17)
        Me.rbtClosed.TabIndex = 1
        Me.rbtClosed.TabStop = True
        Me.rbtClosed.Text = "Closed"
        Me.rbtClosed.UseVisualStyleBackColor = True
        '
        'grpOutStanding
        '
        Me.grpOutStanding.Controls.Add(Me.chkGiftVoucher)
        Me.grpOutStanding.Controls.Add(Me.chkAdvance)
        Me.grpOutStanding.Controls.Add(Me.chkCreditPurchase)
        Me.grpOutStanding.Controls.Add(Me.chkToBe)
        Me.grpOutStanding.Controls.Add(Me.chkCredit)
        Me.grpOutStanding.Controls.Add(Me.chkOrder)
        Me.grpOutStanding.Controls.Add(Me.chkGeneral)
        Me.grpOutStanding.Location = New System.Drawing.Point(16, 464)
        Me.grpOutStanding.Name = "grpOutStanding"
        Me.grpOutStanding.Size = New System.Drawing.Size(367, 48)
        Me.grpOutStanding.TabIndex = 30
        Me.grpOutStanding.TabStop = False
        '
        'chkGiftVoucher
        '
        Me.chkGiftVoucher.AutoSize = True
        Me.chkGiftVoucher.Location = New System.Drawing.Point(5, 29)
        Me.chkGiftVoucher.Name = "chkGiftVoucher"
        Me.chkGiftVoucher.Size = New System.Drawing.Size(81, 17)
        Me.chkGiftVoucher.TabIndex = 5
        Me.chkGiftVoucher.Text = "GVoucher"
        Me.chkGiftVoucher.UseVisualStyleBackColor = True
        '
        'chkAdvance
        '
        Me.chkAdvance.AutoSize = True
        Me.chkAdvance.Location = New System.Drawing.Point(5, 11)
        Me.chkAdvance.Name = "chkAdvance"
        Me.chkAdvance.Size = New System.Drawing.Size(75, 17)
        Me.chkAdvance.TabIndex = 0
        Me.chkAdvance.Text = "Advance"
        Me.chkAdvance.UseVisualStyleBackColor = True
        '
        'chkCreditPurchase
        '
        Me.chkCreditPurchase.AutoSize = True
        Me.chkCreditPurchase.Location = New System.Drawing.Point(208, 11)
        Me.chkCreditPurchase.Name = "chkCreditPurchase"
        Me.chkCreditPurchase.Size = New System.Drawing.Size(80, 17)
        Me.chkCreditPurchase.TabIndex = 3
        Me.chkCreditPurchase.Text = "Cr Purch."
        Me.chkCreditPurchase.UseVisualStyleBackColor = False
        '
        'chkToBe
        '
        Me.chkToBe.AutoSize = True
        Me.chkToBe.Location = New System.Drawing.Point(87, 11)
        Me.chkToBe.Name = "chkToBe"
        Me.chkToBe.Size = New System.Drawing.Size(48, 17)
        Me.chkToBe.TabIndex = 1
        Me.chkToBe.Text = "JND"
        Me.chkToBe.UseVisualStyleBackColor = False
        '
        'chkCredit
        '
        Me.chkCredit.AutoSize = True
        Me.chkCredit.Location = New System.Drawing.Point(295, 11)
        Me.chkCredit.Name = "chkCredit"
        Me.chkCredit.Size = New System.Drawing.Size(61, 17)
        Me.chkCredit.TabIndex = 4
        Me.chkCredit.Text = "Credit"
        Me.chkCredit.UseVisualStyleBackColor = False
        '
        'chkOrder
        '
        Me.chkOrder.AutoSize = True
        Me.chkOrder.Location = New System.Drawing.Point(142, 11)
        Me.chkOrder.Name = "chkOrder"
        Me.chkOrder.Size = New System.Drawing.Size(59, 17)
        Me.chkOrder.TabIndex = 2
        Me.chkOrder.Text = "Order"
        Me.chkOrder.UseVisualStyleBackColor = True
        '
        'chkGeneral
        '
        Me.chkGeneral.AutoSize = True
        Me.chkGeneral.Location = New System.Drawing.Point(87, 29)
        Me.chkGeneral.Name = "chkGeneral"
        Me.chkGeneral.Size = New System.Drawing.Size(58, 17)
        Me.chkGeneral.TabIndex = 6
        Me.chkGeneral.Text = "Other"
        Me.chkGeneral.UseVisualStyleBackColor = True
        '
        'grpOrderBy
        '
        Me.grpOrderBy.Controls.Add(Me.rbtOName)
        Me.grpOrderBy.Controls.Add(Me.rbtORunNo)
        Me.grpOrderBy.Controls.Add(Me.rbtOBillDate)
        Me.grpOrderBy.Location = New System.Drawing.Point(102, 240)
        Me.grpOrderBy.Name = "grpOrderBy"
        Me.grpOrderBy.Size = New System.Drawing.Size(213, 35)
        Me.grpOrderBy.TabIndex = 13
        Me.grpOrderBy.TabStop = False
        '
        'rbtOName
        '
        Me.rbtOName.AutoSize = True
        Me.rbtOName.Location = New System.Drawing.Point(147, 12)
        Me.rbtOName.Name = "rbtOName"
        Me.rbtOName.Size = New System.Drawing.Size(58, 17)
        Me.rbtOName.TabIndex = 2
        Me.rbtOName.TabStop = True
        Me.rbtOName.Text = "Name"
        Me.rbtOName.UseVisualStyleBackColor = True
        '
        'rbtORunNo
        '
        Me.rbtORunNo.AutoSize = True
        Me.rbtORunNo.Location = New System.Drawing.Point(6, 11)
        Me.rbtORunNo.Name = "rbtORunNo"
        Me.rbtORunNo.Size = New System.Drawing.Size(65, 17)
        Me.rbtORunNo.TabIndex = 0
        Me.rbtORunNo.TabStop = True
        Me.rbtORunNo.Text = "TranNo"
        Me.rbtORunNo.UseVisualStyleBackColor = True
        '
        'rbtOBillDate
        '
        Me.rbtOBillDate.AutoSize = True
        Me.rbtOBillDate.Location = New System.Drawing.Point(77, 12)
        Me.rbtOBillDate.Name = "rbtOBillDate"
        Me.rbtOBillDate.Size = New System.Drawing.Size(69, 17)
        Me.rbtOBillDate.TabIndex = 1
        Me.rbtOBillDate.TabStop = True
        Me.rbtOBillDate.Text = "BillDate"
        Me.rbtOBillDate.UseVisualStyleBackColor = True
        '
        'txtNodeId
        '
        Me.txtNodeId.Location = New System.Drawing.Point(102, 365)
        Me.txtNodeId.Name = "txtNodeId"
        Me.txtNodeId.Size = New System.Drawing.Size(91, 21)
        Me.txtNodeId.TabIndex = 21
        '
        'lblNodeId
        '
        Me.lblNodeId.AutoSize = True
        Me.lblNodeId.Location = New System.Drawing.Point(13, 368)
        Me.lblNodeId.Name = "lblNodeId"
        Me.lblNodeId.Size = New System.Drawing.Size(48, 13)
        Me.lblNodeId.TabIndex = 20
        Me.lblNodeId.Text = "NodeId"
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(158, 545)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(90, 30)
        Me.btnNew.TabIndex = 33
        Me.btnNew.Text = "&New[F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(54, 545)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(90, 30)
        Me.btnView_Search.TabIndex = 32
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'lblCostCentre
        '
        Me.lblCostCentre.AutoSize = True
        Me.lblCostCentre.Location = New System.Drawing.Point(13, 285)
        Me.lblCostCentre.Name = "lblCostCentre"
        Me.lblCostCentre.Size = New System.Drawing.Size(76, 13)
        Me.lblCostCentre.TabIndex = 14
        Me.lblCostCentre.Text = "Cost Centre"
        '
        'cmbCostCentre
        '
        Me.cmbCostCentre.FormattingEnabled = True
        Me.cmbCostCentre.Location = New System.Drawing.Point(102, 281)
        Me.cmbCostCentre.Name = "cmbCostCentre"
        Me.cmbCostCentre.Size = New System.Drawing.Size(213, 21)
        Me.cmbCostCentre.TabIndex = 15
        '
        'lblFilterCaption
        '
        Me.lblFilterCaption.AutoSize = True
        Me.lblFilterCaption.Location = New System.Drawing.Point(13, 340)
        Me.lblFilterCaption.Name = "lblFilterCaption"
        Me.lblFilterCaption.Size = New System.Drawing.Size(83, 13)
        Me.lblFilterCaption.TabIndex = 18
        Me.lblFilterCaption.Text = "Filter Caption"
        '
        'lblFilterBy
        '
        Me.lblFilterBy.AutoSize = True
        Me.lblFilterBy.Location = New System.Drawing.Point(13, 314)
        Me.lblFilterBy.Name = "lblFilterBy"
        Me.lblFilterBy.Size = New System.Drawing.Size(54, 13)
        Me.lblFilterBy.TabIndex = 16
        Me.lblFilterBy.Text = "Filter By"
        '
        'lblDateTo
        '
        Me.lblDateTo.AutoSize = True
        Me.lblDateTo.Location = New System.Drawing.Point(190, 78)
        Me.lblDateTo.Name = "lblDateTo"
        Me.lblDateTo.Size = New System.Drawing.Size(20, 13)
        Me.lblDateTo.TabIndex = 6
        Me.lblDateTo.Text = "To"
        '
        'lblDateFrom
        '
        Me.lblDateFrom.AutoSize = True
        Me.lblDateFrom.Location = New System.Drawing.Point(9, 78)
        Me.lblDateFrom.Name = "lblDateFrom"
        Me.lblDateFrom.Size = New System.Drawing.Size(67, 13)
        Me.lblDateFrom.TabIndex = 4
        Me.lblDateFrom.Text = "Date From"
        '
        'cmbFilterBy
        '
        Me.cmbFilterBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFilterBy.FormattingEnabled = True
        Me.cmbFilterBy.Location = New System.Drawing.Point(102, 310)
        Me.cmbFilterBy.Name = "cmbFilterBy"
        Me.cmbFilterBy.Size = New System.Drawing.Size(213, 21)
        Me.cmbFilterBy.TabIndex = 17
        '
        'txtFilterCaption
        '
        Me.txtFilterCaption.Location = New System.Drawing.Point(102, 336)
        Me.txtFilterCaption.Name = "txtFilterCaption"
        Me.txtFilterCaption.Size = New System.Drawing.Size(213, 21)
        Me.txtFilterCaption.TabIndex = 19
        '
        'lblGroupBy
        '
        Me.lblGroupBy.AutoSize = True
        Me.lblGroupBy.Location = New System.Drawing.Point(79, 125)
        Me.lblGroupBy.Name = "lblGroupBy"
        Me.lblGroupBy.Size = New System.Drawing.Size(61, 13)
        Me.lblGroupBy.TabIndex = 10
        Me.lblGroupBy.Text = "Group By"
        Me.lblGroupBy.Visible = False
        '
        'grpGroupBy
        '
        Me.grpGroupBy.Controls.Add(Me.rbtGName)
        Me.grpGroupBy.Controls.Add(Me.rbtGCode)
        Me.grpGroupBy.Controls.Add(Me.rbtGRunNo)
        Me.grpGroupBy.Location = New System.Drawing.Point(82, 141)
        Me.grpGroupBy.Name = "grpGroupBy"
        Me.grpGroupBy.Size = New System.Drawing.Size(213, 35)
        Me.grpGroupBy.TabIndex = 11
        Me.grpGroupBy.TabStop = False
        Me.grpGroupBy.Visible = False
        '
        'rbtGName
        '
        Me.rbtGName.AutoSize = True
        Me.rbtGName.Location = New System.Drawing.Point(147, 11)
        Me.rbtGName.Name = "rbtGName"
        Me.rbtGName.Size = New System.Drawing.Size(58, 17)
        Me.rbtGName.TabIndex = 2
        Me.rbtGName.TabStop = True
        Me.rbtGName.Text = "Name"
        Me.rbtGName.UseVisualStyleBackColor = True
        '
        'rbtGCode
        '
        Me.rbtGCode.AutoSize = True
        Me.rbtGCode.Location = New System.Drawing.Point(77, 11)
        Me.rbtGCode.Name = "rbtGCode"
        Me.rbtGCode.Size = New System.Drawing.Size(55, 17)
        Me.rbtGCode.TabIndex = 1
        Me.rbtGCode.TabStop = True
        Me.rbtGCode.Text = "Code"
        Me.rbtGCode.UseVisualStyleBackColor = True
        '
        'rbtGRunNo
        '
        Me.rbtGRunNo.AutoSize = True
        Me.rbtGRunNo.Location = New System.Drawing.Point(6, 11)
        Me.rbtGRunNo.Name = "rbtGRunNo"
        Me.rbtGRunNo.Size = New System.Drawing.Size(62, 17)
        Me.rbtGRunNo.TabIndex = 0
        Me.rbtGRunNo.TabStop = True
        Me.rbtGRunNo.Text = "RunNo"
        Me.rbtGRunNo.UseVisualStyleBackColor = True
        '
        'chkAll
        '
        Me.chkAll.AutoSize = True
        Me.chkAll.Location = New System.Drawing.Point(82, 190)
        Me.chkAll.Name = "chkAll"
        Me.chkAll.Size = New System.Drawing.Size(40, 17)
        Me.chkAll.TabIndex = 0
        Me.chkAll.Text = "All"
        Me.chkAll.UseVisualStyleBackColor = False
        Me.chkAll.Visible = False
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(659, 7)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 14
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(551, 7)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 13
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
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
        'pnlGrid
        '
        Me.pnlGrid.Controls.Add(Me.grbRunNoFocus)
        Me.pnlGrid.Controls.Add(Me.gridView)
        Me.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlGrid.Location = New System.Drawing.Point(0, 20)
        Me.pnlGrid.Name = "pnlGrid"
        Me.pnlGrid.Size = New System.Drawing.Size(1005, 540)
        Me.pnlGrid.TabIndex = 25
        '
        'grbRunNoFocus
        '
        Me.grbRunNoFocus.Controls.Add(Me.gridRunNoFocus)
        Me.grbRunNoFocus.Location = New System.Drawing.Point(145, 131)
        Me.grbRunNoFocus.Name = "grbRunNoFocus"
        Me.grbRunNoFocus.Size = New System.Drawing.Size(616, 209)
        Me.grbRunNoFocus.TabIndex = 3
        Me.grbRunNoFocus.TabStop = False
        Me.grbRunNoFocus.Visible = False
        '
        'gridRunNoFocus
        '
        Me.gridRunNoFocus.AllowUserToAddRows = False
        Me.gridRunNoFocus.AllowUserToDeleteRows = False
        Me.gridRunNoFocus.AllowUserToResizeRows = False
        Me.gridRunNoFocus.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridRunNoFocus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridRunNoFocus.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridRunNoFocus.Location = New System.Drawing.Point(3, 17)
        Me.gridRunNoFocus.MultiSelect = False
        Me.gridRunNoFocus.Name = "gridRunNoFocus"
        Me.gridRunNoFocus.ReadOnly = True
        Me.gridRunNoFocus.RowHeadersVisible = False
        Me.gridRunNoFocus.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridRunNoFocus.Size = New System.Drawing.Size(610, 189)
        Me.gridRunNoFocus.StandardTab = True
        Me.gridRunNoFocus.TabIndex = 2
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.ContextMenuStrip = Me.cmbGridShortCut
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 0)
        Me.gridView.MultiSelect = False
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(1005, 540)
        Me.gridView.StandardTab = True
        Me.gridView.TabIndex = 1
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
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1005, 20)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "TITLE"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGen)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1019, 632)
        Me.tabMain.TabIndex = 0
        '
        'tabGen
        '
        Me.tabGen.Controls.Add(Me.grbControls)
        Me.tabGen.Controls.Add(Me.lblGroupBy)
        Me.tabGen.Controls.Add(Me.chkAll)
        Me.tabGen.Controls.Add(Me.grpGroupBy)
        Me.tabGen.Location = New System.Drawing.Point(4, 22)
        Me.tabGen.Name = "tabGen"
        Me.tabGen.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGen.Size = New System.Drawing.Size(1011, 606)
        Me.tabGen.TabIndex = 0
        Me.tabGen.Text = "Gen"
        Me.tabGen.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.pnlView)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1011, 606)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'pnlView
        '
        Me.pnlView.Controls.Add(Me.pnlGrid)
        Me.pnlView.Controls.Add(Me.pnlFooter)
        Me.pnlView.Controls.Add(Me.pnlTitle)
        Me.pnlView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlView.Location = New System.Drawing.Point(3, 3)
        Me.pnlView.Name = "pnlView"
        Me.pnlView.Size = New System.Drawing.Size(1005, 600)
        Me.pnlView.TabIndex = 1
        '
        'pnlFooter
        '
        Me.pnlFooter.Controls.Add(Me.btnBack)
        Me.pnlFooter.Controls.Add(Me.btnExport)
        Me.pnlFooter.Controls.Add(Me.btnPrint)
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Location = New System.Drawing.Point(0, 560)
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Size = New System.Drawing.Size(1005, 40)
        Me.pnlFooter.TabIndex = 1
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(443, 7)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 15
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'pnlTitle
        '
        Me.pnlTitle.Controls.Add(Me.lblTitle)
        Me.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTitle.Location = New System.Drawing.Point(0, 0)
        Me.pnlTitle.Name = "pnlTitle"
        Me.pnlTitle.Size = New System.Drawing.Size(1005, 20)
        Me.pnlTitle.TabIndex = 0
        '
        'frmCustomerOutstanding
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(1019, 632)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "frmCustomerOutstanding"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "CustomerOutstanding"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grbControls.ResumeLayout(False)
        Me.grbControls.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.grpOutput.ResumeLayout(False)
        Me.grpOutput.PerformLayout()
        Me.grpDisplay.ResumeLayout(False)
        Me.grpDisplay.PerformLayout()
        Me.grpOutStanding.ResumeLayout(False)
        Me.grpOutStanding.PerformLayout()
        Me.grpOrderBy.ResumeLayout(False)
        Me.grpOrderBy.PerformLayout()
        Me.grpGroupBy.ResumeLayout(False)
        Me.grpGroupBy.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.pnlGrid.ResumeLayout(False)
        Me.grbRunNoFocus.ResumeLayout(False)
        CType(Me.gridRunNoFocus, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmbGridShortCut.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGen.ResumeLayout(False)
        Me.tabGen.PerformLayout()
        Me.tabView.ResumeLayout(False)
        Me.pnlView.ResumeLayout(False)
        Me.pnlFooter.ResumeLayout(False)
        Me.pnlTitle.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grbControls As System.Windows.Forms.GroupBox
    Friend WithEvents cmbFilterBy As System.Windows.Forms.ComboBox
    Friend WithEvents txtFilterCaption As System.Windows.Forms.TextBox
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents lblFilterCaption As System.Windows.Forms.Label
    Friend WithEvents lblDateTo As System.Windows.Forms.Label
    Friend WithEvents lblDateFrom As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents lblFilterBy As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents grpOutStanding As System.Windows.Forms.GroupBox
    Friend WithEvents chkGeneral As System.Windows.Forms.CheckBox
    Friend WithEvents chkOrder As System.Windows.Forms.CheckBox
    Friend WithEvents chkToBe As System.Windows.Forms.CheckBox
    Friend WithEvents chkCredit As System.Windows.Forms.CheckBox
    Friend WithEvents chkAll As System.Windows.Forms.CheckBox
    Friend WithEvents grpOutput As System.Windows.Forms.GroupBox
    Friend WithEvents rbtSummaryWise As System.Windows.Forms.RadioButton
    Friend WithEvents rbtDetailWise As System.Windows.Forms.RadioButton
    Friend WithEvents grpDisplay As System.Windows.Forms.GroupBox
    Friend WithEvents rbtPending As System.Windows.Forms.RadioButton
    Friend WithEvents rbtAll As System.Windows.Forms.RadioButton
    Friend WithEvents rbtClosed As System.Windows.Forms.RadioButton
    Friend WithEvents lblCostCentre As System.Windows.Forms.Label
    Friend WithEvents cmbCostCentre As System.Windows.Forms.ComboBox
    Friend WithEvents txtNodeId As System.Windows.Forms.TextBox
    Friend WithEvents lblNodeId As System.Windows.Forms.Label
    Friend WithEvents grpOrderBy As System.Windows.Forms.GroupBox
    Friend WithEvents rbtOName As System.Windows.Forms.RadioButton
    Friend WithEvents rbtORunNo As System.Windows.Forms.RadioButton
    Friend WithEvents rbtOBillDate As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGCode As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGName As System.Windows.Forms.RadioButton
    Friend WithEvents grpGroupBy As System.Windows.Forms.GroupBox
    Friend WithEvents rbtGRunNo As System.Windows.Forms.RadioButton
    Friend WithEvents lblGroupBy As System.Windows.Forms.Label
    Friend WithEvents lblOuderBy As System.Windows.Forms.Label
    Friend WithEvents lblDisplay As System.Windows.Forms.Label
    Friend WithEvents lblOutPut As System.Windows.Forms.Label
    Friend WithEvents pnlGrid As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents grbRunNoFocus As System.Windows.Forms.GroupBox
    Friend WithEvents gridRunNoFocus As System.Windows.Forms.DataGridView
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGen As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents pnlView As System.Windows.Forms.Panel
    Friend WithEvents pnlTitle As System.Windows.Forms.Panel
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkCompanySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCompany As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkBasedOnAccode As System.Windows.Forms.CheckBox
    Friend WithEvents chkGroupByArea As System.Windows.Forms.CheckBox
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkCreditRpt As System.Windows.Forms.CheckBox
    Friend WithEvents chkformat As System.Windows.Forms.CheckBox
    Friend WithEvents rbtReceived As System.Windows.Forms.RadioButton
    Friend WithEvents chkCreditPurchase As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents optAllCode As System.Windows.Forms.RadioButton
    Friend WithEvents rbtDrs As System.Windows.Forms.RadioButton
    Friend WithEvents chkAdvance As System.Windows.Forms.CheckBox
    Friend WithEvents ChkWithRemark As System.Windows.Forms.CheckBox
    Friend WithEvents ChkWithAddr As System.Windows.Forms.CheckBox
    Friend WithEvents ChkWithSlNo As System.Windows.Forms.CheckBox
    Friend WithEvents chkgrbcc As System.Windows.Forms.CheckBox
    Friend WithEvents ChkWithWt As System.Windows.Forms.CheckBox
    Friend WithEvents ChKWithGst As System.Windows.Forms.CheckBox
    Friend WithEvents chkGiftVoucher As CheckBox
End Class
