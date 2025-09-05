<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSalesAbstract
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
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AutoReziseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.pnlHeading = New System.Windows.Forms.Panel
        Me.lblTitle = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExtToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.rbtSummary = New System.Windows.Forms.RadioButton
        Me.rbtMonth = New System.Windows.Forms.RadioButton
        Me.rbtDate = New System.Windows.Forms.RadioButton
        Me.rbtBillNo = New System.Windows.Forms.RadioButton
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmbMetal = New System.Windows.Forms.ComboBox
        Me.btnExport = New System.Windows.Forms.Button
        Me.chkWithSR = New System.Windows.Forms.CheckBox
        Me.chkPcs = New System.Windows.Forms.CheckBox
        Me.chkGrsWt = New System.Windows.Forms.CheckBox
        Me.chkNetWt = New System.Windows.Forms.CheckBox
        Me.btnPrint = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.rbtExem = New System.Windows.Forms.RadioButton
        Me.rbtAll = New System.Windows.Forms.RadioButton
        Me.rbtMfg = New System.Windows.Forms.RadioButton
        Me.rbtTrading = New System.Windows.Forms.RadioButton
        Me.chkTranno = New System.Windows.Forms.CheckBox
        Me.ChkSepPureMc = New System.Windows.Forms.CheckBox
        Me.chkMC = New System.Windows.Forms.CheckBox
        Me.btn_dPrint = New System.Windows.Forms.Button
        Me.chkwithstustone = New System.Windows.Forms.CheckBox
        Me.chkDiscount = New System.Windows.Forms.CheckBox
        Me.GBmore = New System.Windows.Forms.GroupBox
        Me.ChkBillPrefix = New System.Windows.Forms.CheckBox
        Me.ChkItem = New System.Windows.Forms.CheckBox
        Me.Chkstngrm = New System.Windows.Forms.CheckBox
        Me.ChkStnAmt = New System.Windows.Forms.CheckBox
        Me.ChkDiaAmt = New System.Windows.Forms.CheckBox
        Me.Chkecs = New System.Windows.Forms.CheckBox
        Me.Chkparticular = New System.Windows.Forms.CheckBox
        Me.ChkStnWt = New System.Windows.Forms.CheckBox
        Me.ChkDIAwt = New System.Windows.Forms.CheckBox
        Me.Chkmore = New System.Windows.Forms.CheckBox
        Me.chkEd = New System.Windows.Forms.CheckBox
        Me.chkVA = New System.Windows.Forms.CheckBox
        Me.cmbCategory = New BrighttechPack.CheckedComboBox
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Panel2.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.pnlHeading.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.GBmore.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.gridView)
        Me.Panel2.Controls.Add(Me.pnlHeading)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 146)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1028, 470)
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
        Me.gridView.Location = New System.Drawing.Point(0, 25)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 21
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.Size = New System.Drawing.Size(1028, 445)
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
        'pnlHeading
        '
        Me.pnlHeading.Controls.Add(Me.lblTitle)
        Me.pnlHeading.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeading.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeading.Name = "pnlHeading"
        Me.pnlHeading.Size = New System.Drawing.Size(1028, 25)
        Me.pnlHeading.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1028, 25)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExtToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExtToolStripMenuItem
        '
        Me.ExtToolStripMenuItem.Name = "ExtToolStripMenuItem"
        Me.ExtToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExtToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExtToolStripMenuItem.Text = "Exit"
        Me.ExtToolStripMenuItem.Visible = False
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(423, 111)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 27
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(723, 111)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 30
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(523, 111)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 28
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'rbtSummary
        '
        Me.rbtSummary.AutoSize = True
        Me.rbtSummary.Checked = True
        Me.rbtSummary.Location = New System.Drawing.Point(10, 97)
        Me.rbtSummary.Name = "rbtSummary"
        Me.rbtSummary.Size = New System.Drawing.Size(81, 17)
        Me.rbtSummary.TabIndex = 21
        Me.rbtSummary.TabStop = True
        Me.rbtSummary.Text = "Summary"
        Me.rbtSummary.UseVisualStyleBackColor = True
        '
        'rbtMonth
        '
        Me.rbtMonth.AutoSize = True
        Me.rbtMonth.Location = New System.Drawing.Point(91, 97)
        Me.rbtMonth.Name = "rbtMonth"
        Me.rbtMonth.Size = New System.Drawing.Size(90, 17)
        Me.rbtMonth.TabIndex = 22
        Me.rbtMonth.Text = "Month Wise"
        Me.rbtMonth.UseVisualStyleBackColor = True
        '
        'rbtDate
        '
        Me.rbtDate.AutoSize = True
        Me.rbtDate.Location = New System.Drawing.Point(181, 97)
        Me.rbtDate.Name = "rbtDate"
        Me.rbtDate.Size = New System.Drawing.Size(83, 17)
        Me.rbtDate.TabIndex = 23
        Me.rbtDate.Text = "Date Wise"
        Me.rbtDate.UseVisualStyleBackColor = True
        '
        'rbtBillNo
        '
        Me.rbtBillNo.AutoSize = True
        Me.rbtBillNo.Location = New System.Drawing.Point(264, 97)
        Me.rbtBillNo.Name = "rbtBillNo"
        Me.rbtBillNo.Size = New System.Drawing.Size(88, 17)
        Me.rbtBillNo.TabIndex = 24
        Me.rbtBillNo.Text = "BillNo Wise"
        Me.rbtBillNo.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(12, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 21)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date From"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(12, 40)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(79, 21)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(12, 64)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(79, 21)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "Metal"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(368, 72)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(60, 13)
        Me.Label5.TabIndex = 18
        Me.Label5.Text = "Category"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(186, 7)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(23, 21)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbMetal
        '
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(87, 68)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(274, 21)
        Me.cmbMetal.TabIndex = 17
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(623, 111)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 29
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'chkWithSR
        '
        Me.chkWithSR.AutoSize = True
        Me.chkWithSR.Location = New System.Drawing.Point(515, 10)
        Me.chkWithSR.Name = "chkWithSR"
        Me.chkWithSR.Size = New System.Drawing.Size(128, 17)
        Me.chkWithSR.TabIndex = 7
        Me.chkWithSR.Text = "&With Sales Return"
        Me.chkWithSR.UseVisualStyleBackColor = True
        '
        'chkPcs
        '
        Me.chkPcs.AutoSize = True
        Me.chkPcs.Location = New System.Drawing.Point(313, 10)
        Me.chkPcs.Name = "chkPcs"
        Me.chkPcs.Size = New System.Drawing.Size(62, 17)
        Me.chkPcs.TabIndex = 4
        Me.chkPcs.Text = "Pieces"
        Me.chkPcs.UseVisualStyleBackColor = True
        '
        'chkGrsWt
        '
        Me.chkGrsWt.AutoSize = True
        Me.chkGrsWt.Location = New System.Drawing.Point(380, 10)
        Me.chkGrsWt.Name = "chkGrsWt"
        Me.chkGrsWt.Size = New System.Drawing.Size(74, 17)
        Me.chkGrsWt.TabIndex = 5
        Me.chkGrsWt.Text = "GrossWt"
        Me.chkGrsWt.UseVisualStyleBackColor = True
        '
        'chkNetWt
        '
        Me.chkNetWt.AutoSize = True
        Me.chkNetWt.Location = New System.Drawing.Point(454, 10)
        Me.chkNetWt.Name = "chkNetWt"
        Me.chkNetWt.Size = New System.Drawing.Size(60, 17)
        Me.chkNetWt.TabIndex = 6
        Me.chkNetWt.Text = "NetWt"
        Me.chkNetWt.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(823, 111)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 31
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Controls.Add(Me.chkTranno)
        Me.Panel1.Controls.Add(Me.ChkSepPureMc)
        Me.Panel1.Controls.Add(Me.cmbCategory)
        Me.Panel1.Controls.Add(Me.chkMC)
        Me.Panel1.Controls.Add(Me.btn_dPrint)
        Me.Panel1.Controls.Add(Me.chkwithstustone)
        Me.Panel1.Controls.Add(Me.chkDiscount)
        Me.Panel1.Controls.Add(Me.GBmore)
        Me.Panel1.Controls.Add(Me.Chkmore)
        Me.Panel1.Controls.Add(Me.chkEd)
        Me.Panel1.Controls.Add(Me.chkVA)
        Me.Panel1.Controls.Add(Me.chkCmbCostCentre)
        Me.Panel1.Controls.Add(Me.dtpTo)
        Me.Panel1.Controls.Add(Me.dtpFrom)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.chkNetWt)
        Me.Panel1.Controls.Add(Me.chkGrsWt)
        Me.Panel1.Controls.Add(Me.chkPcs)
        Me.Panel1.Controls.Add(Me.chkWithSR)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.cmbMetal)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.rbtBillNo)
        Me.Panel1.Controls.Add(Me.rbtDate)
        Me.Panel1.Controls.Add(Me.rbtMonth)
        Me.Panel1.Controls.Add(Me.rbtSummary)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.btnView_Search)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1028, 146)
        Me.Panel1.TabIndex = 0
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.rbtExem)
        Me.Panel3.Controls.Add(Me.rbtAll)
        Me.Panel3.Controls.Add(Me.rbtMfg)
        Me.Panel3.Controls.Add(Me.rbtTrading)
        Me.Panel3.Location = New System.Drawing.Point(6, 122)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(411, 21)
        Me.Panel3.TabIndex = 26
        '
        'rbtExem
        '
        Me.rbtExem.AutoSize = True
        Me.rbtExem.Location = New System.Drawing.Point(195, 2)
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
        Me.rbtAll.Location = New System.Drawing.Point(286, 2)
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
        Me.rbtMfg.Location = New System.Drawing.Point(85, 2)
        Me.rbtMfg.Name = "rbtMfg"
        Me.rbtMfg.Size = New System.Drawing.Size(105, 17)
        Me.rbtMfg.TabIndex = 1
        Me.rbtMfg.Text = "Manufacturing"
        Me.rbtMfg.UseVisualStyleBackColor = True
        '
        'rbtTrading
        '
        Me.rbtTrading.AutoSize = True
        Me.rbtTrading.Location = New System.Drawing.Point(4, 2)
        Me.rbtTrading.Name = "rbtTrading"
        Me.rbtTrading.Size = New System.Drawing.Size(68, 17)
        Me.rbtTrading.TabIndex = 0
        Me.rbtTrading.Text = "Trading"
        Me.rbtTrading.UseVisualStyleBackColor = True
        '
        'chkTranno
        '
        Me.chkTranno.AutoSize = True
        Me.chkTranno.Location = New System.Drawing.Point(352, 98)
        Me.chkTranno.Name = "chkTranno"
        Me.chkTranno.Size = New System.Drawing.Size(67, 17)
        Me.chkTranno.TabIndex = 25
        Me.chkTranno.Text = "TranNo"
        Me.chkTranno.UseVisualStyleBackColor = True
        Me.chkTranno.Visible = False
        '
        'ChkSepPureMc
        '
        Me.ChkSepPureMc.AutoSize = True
        Me.ChkSepPureMc.Location = New System.Drawing.Point(750, 35)
        Me.ChkSepPureMc.Name = "ChkSepPureMc"
        Me.ChkSepPureMc.Size = New System.Drawing.Size(130, 17)
        Me.ChkSepPureMc.TabIndex = 15
        Me.ChkSepPureMc.Text = "Seperate Pure MC"
        Me.ChkSepPureMc.UseVisualStyleBackColor = True
        '
        'chkMC
        '
        Me.chkMC.AutoSize = True
        Me.chkMC.Location = New System.Drawing.Point(679, 35)
        Me.chkMC.Name = "chkMC"
        Me.chkMC.Size = New System.Drawing.Size(73, 17)
        Me.chkMC.TabIndex = 14
        Me.chkMC.Text = "With MC"
        Me.chkMC.UseVisualStyleBackColor = True
        '
        'btn_dPrint
        '
        Me.btn_dPrint.Location = New System.Drawing.Point(923, 111)
        Me.btn_dPrint.Name = "btn_dPrint"
        Me.btn_dPrint.Size = New System.Drawing.Size(100, 30)
        Me.btn_dPrint.TabIndex = 32
        Me.btn_dPrint.Text = "Detail Print"
        Me.btn_dPrint.UseVisualStyleBackColor = True
        Me.btn_dPrint.Visible = False
        '
        'chkwithstustone
        '
        Me.chkwithstustone.AutoSize = True
        Me.chkwithstustone.Location = New System.Drawing.Point(474, 35)
        Me.chkwithstustone.Name = "chkwithstustone"
        Me.chkwithstustone.Size = New System.Drawing.Size(132, 17)
        Me.chkwithstustone.TabIndex = 12
        Me.chkwithstustone.Text = "With Studed Stone"
        Me.chkwithstustone.UseVisualStyleBackColor = True
        '
        'chkDiscount
        '
        Me.chkDiscount.AutoSize = True
        Me.chkDiscount.Location = New System.Drawing.Point(371, 35)
        Me.chkDiscount.Name = "chkDiscount"
        Me.chkDiscount.Size = New System.Drawing.Size(104, 17)
        Me.chkDiscount.TabIndex = 11
        Me.chkDiscount.Text = "&With Discount"
        Me.chkDiscount.UseVisualStyleBackColor = True
        '
        'GBmore
        '
        Me.GBmore.Controls.Add(Me.ChkBillPrefix)
        Me.GBmore.Controls.Add(Me.ChkItem)
        Me.GBmore.Controls.Add(Me.Chkstngrm)
        Me.GBmore.Controls.Add(Me.ChkStnAmt)
        Me.GBmore.Controls.Add(Me.ChkDiaAmt)
        Me.GBmore.Controls.Add(Me.Chkecs)
        Me.GBmore.Controls.Add(Me.Chkparticular)
        Me.GBmore.Controls.Add(Me.ChkStnWt)
        Me.GBmore.Controls.Add(Me.ChkDIAwt)
        Me.GBmore.Location = New System.Drawing.Point(879, 1)
        Me.GBmore.Name = "GBmore"
        Me.GBmore.Size = New System.Drawing.Size(150, 103)
        Me.GBmore.TabIndex = 31
        Me.GBmore.TabStop = False
        Me.GBmore.Visible = False
        '
        'ChkBillPrefix
        '
        Me.ChkBillPrefix.AutoSize = True
        Me.ChkBillPrefix.Location = New System.Drawing.Point(6, 80)
        Me.ChkBillPrefix.Name = "ChkBillPrefix"
        Me.ChkBillPrefix.Size = New System.Drawing.Size(80, 17)
        Me.ChkBillPrefix.TabIndex = 8
        Me.ChkBillPrefix.Text = "Bill Prefix"
        Me.ChkBillPrefix.UseVisualStyleBackColor = True
        '
        'ChkItem
        '
        Me.ChkItem.AutoSize = True
        Me.ChkItem.Location = New System.Drawing.Point(83, 45)
        Me.ChkItem.Name = "ChkItem"
        Me.ChkItem.Size = New System.Drawing.Size(53, 17)
        Me.ChkItem.TabIndex = 7
        Me.ChkItem.Text = "Item"
        Me.ChkItem.UseVisualStyleBackColor = True
        '
        'Chkstngrm
        '
        Me.Chkstngrm.AutoSize = True
        Me.Chkstngrm.Checked = True
        Me.Chkstngrm.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Chkstngrm.Location = New System.Drawing.Point(83, 10)
        Me.Chkstngrm.Name = "Chkstngrm"
        Me.Chkstngrm.Size = New System.Drawing.Size(68, 17)
        Me.Chkstngrm.TabIndex = 6
        Me.Chkstngrm.Text = "Stngrm"
        Me.Chkstngrm.UseVisualStyleBackColor = True
        '
        'ChkStnAmt
        '
        Me.ChkStnAmt.AutoSize = True
        Me.ChkStnAmt.Checked = True
        Me.ChkStnAmt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkStnAmt.Location = New System.Drawing.Point(6, 29)
        Me.ChkStnAmt.Name = "ChkStnAmt"
        Me.ChkStnAmt.Size = New System.Drawing.Size(68, 17)
        Me.ChkStnAmt.TabIndex = 4
        Me.ChkStnAmt.Text = "StnAmt"
        Me.ChkStnAmt.UseVisualStyleBackColor = True
        '
        'ChkDiaAmt
        '
        Me.ChkDiaAmt.AutoSize = True
        Me.ChkDiaAmt.Checked = True
        Me.ChkDiaAmt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkDiaAmt.Location = New System.Drawing.Point(83, 27)
        Me.ChkDiaAmt.Name = "ChkDiaAmt"
        Me.ChkDiaAmt.Size = New System.Drawing.Size(68, 17)
        Me.ChkDiaAmt.TabIndex = 5
        Me.ChkDiaAmt.Text = "DiaAmt"
        Me.ChkDiaAmt.UseVisualStyleBackColor = True
        '
        'Chkecs
        '
        Me.Chkecs.AutoSize = True
        Me.Chkecs.Location = New System.Drawing.Point(83, 63)
        Me.Chkecs.Name = "Chkecs"
        Me.Chkecs.Size = New System.Drawing.Size(50, 17)
        Me.Chkecs.TabIndex = 3
        Me.Chkecs.Text = "ECS"
        Me.Chkecs.UseVisualStyleBackColor = True
        Me.Chkecs.Visible = False
        '
        'Chkparticular
        '
        Me.Chkparticular.AutoSize = True
        Me.Chkparticular.Checked = True
        Me.Chkparticular.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Chkparticular.Location = New System.Drawing.Point(6, 63)
        Me.Chkparticular.Name = "Chkparticular"
        Me.Chkparticular.Size = New System.Drawing.Size(80, 17)
        Me.Chkparticular.TabIndex = 2
        Me.Chkparticular.Text = "Particular"
        Me.Chkparticular.UseVisualStyleBackColor = True
        '
        'ChkStnWt
        '
        Me.ChkStnWt.AutoSize = True
        Me.ChkStnWt.Checked = True
        Me.ChkStnWt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkStnWt.Location = New System.Drawing.Point(6, 12)
        Me.ChkStnWt.Name = "ChkStnWt"
        Me.ChkStnWt.Size = New System.Drawing.Size(63, 17)
        Me.ChkStnWt.TabIndex = 0
        Me.ChkStnWt.Text = "StnCrt"
        Me.ChkStnWt.UseVisualStyleBackColor = True
        '
        'ChkDIAwt
        '
        Me.ChkDIAwt.AutoSize = True
        Me.ChkDIAwt.Checked = True
        Me.ChkDIAwt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkDIAwt.Location = New System.Drawing.Point(6, 46)
        Me.ChkDIAwt.Name = "ChkDIAwt"
        Me.ChkDIAwt.Size = New System.Drawing.Size(60, 17)
        Me.ChkDIAwt.TabIndex = 1
        Me.ChkDIAwt.Text = "DiaWt"
        Me.ChkDIAwt.UseVisualStyleBackColor = True
        '
        'Chkmore
        '
        Me.Chkmore.AutoSize = True
        Me.Chkmore.Location = New System.Drawing.Point(806, 70)
        Me.Chkmore.Name = "Chkmore"
        Me.Chkmore.Size = New System.Drawing.Size(54, 17)
        Me.Chkmore.TabIndex = 20
        Me.Chkmore.Text = "More"
        Me.Chkmore.UseVisualStyleBackColor = True
        '
        'chkEd
        '
        Me.chkEd.AutoSize = True
        Me.chkEd.Location = New System.Drawing.Point(610, 35)
        Me.chkEd.Name = "chkEd"
        Me.chkEd.Size = New System.Drawing.Size(71, 17)
        Me.chkEd.TabIndex = 13
        Me.chkEd.Text = "With ED"
        Me.chkEd.UseVisualStyleBackColor = True
        '
        'chkVA
        '
        Me.chkVA.AutoSize = True
        Me.chkVA.Location = New System.Drawing.Point(641, 10)
        Me.chkVA.Name = "chkVA"
        Me.chkVA.Size = New System.Drawing.Size(242, 17)
        Me.chkVA.TabIndex = 8
        Me.chkVA.Text = "With &Vertical Address && Payment Det."
        Me.chkVA.UseVisualStyleBackColor = True
        '
        'cmbCategory
        '
        Me.cmbCategory.CheckOnClick = True
        Me.cmbCategory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbCategory.DropDownHeight = 1
        Me.cmbCategory.FormattingEnabled = True
        Me.cmbCategory.IntegralHeight = False
        Me.cmbCategory.Location = New System.Drawing.Point(434, 67)
        Me.cmbCategory.Name = "cmbCategory"
        Me.cmbCategory.Size = New System.Drawing.Size(365, 22)
        Me.cmbCategory.TabIndex = 19
        Me.cmbCategory.ValueSeparator = ", "
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(87, 40)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(274, 22)
        Me.chkCmbCostCentre.TabIndex = 10
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
        'frmSalesAbstract
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 616)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmSalesAbstract"
        Me.Text = "SALES ABSTRACT REPORT"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel2.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip2.ResumeLayout(False)
        Me.pnlHeading.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.GBmore.ResumeLayout(False)
        Me.GBmore.PerformLayout()
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
    Friend WithEvents rbtSummary As System.Windows.Forms.RadioButton
    Friend WithEvents rbtMonth As System.Windows.Forms.RadioButton
    Friend WithEvents rbtDate As System.Windows.Forms.RadioButton
    Friend WithEvents rbtBillNo As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbMetal As System.Windows.Forms.ComboBox
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents chkWithSR As System.Windows.Forms.CheckBox
    Friend WithEvents chkPcs As System.Windows.Forms.CheckBox
    Friend WithEvents chkGrsWt As System.Windows.Forms.CheckBox
    Friend WithEvents chkNetWt As System.Windows.Forms.CheckBox
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents chkVA As System.Windows.Forms.CheckBox
    Friend WithEvents chkEd As System.Windows.Forms.CheckBox
    Friend WithEvents ChkStnWt As System.Windows.Forms.CheckBox
    Friend WithEvents ChkDIAwt As System.Windows.Forms.CheckBox
    Friend WithEvents GBmore As System.Windows.Forms.GroupBox
    Friend WithEvents Chkparticular As System.Windows.Forms.CheckBox
    Friend WithEvents Chkmore As System.Windows.Forms.CheckBox
    Friend WithEvents Chkecs As System.Windows.Forms.CheckBox
    Friend WithEvents chkDiscount As System.Windows.Forms.CheckBox
    Friend WithEvents btn_dPrint As System.Windows.Forms.Button
    Friend WithEvents chkwithstustone As System.Windows.Forms.CheckBox
    Friend WithEvents chkMC As System.Windows.Forms.CheckBox
    Friend WithEvents cmbCategory As BrighttechPack.CheckedComboBox
    Friend WithEvents ChkSepPureMc As System.Windows.Forms.CheckBox
    Friend WithEvents ChkStnAmt As System.Windows.Forms.CheckBox
    Friend WithEvents ChkDiaAmt As System.Windows.Forms.CheckBox
    Friend WithEvents Chkstngrm As System.Windows.Forms.CheckBox
    Friend WithEvents chkTranno As System.Windows.Forms.CheckBox
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AutoReziseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChkItem As System.Windows.Forms.CheckBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents rbtExem As System.Windows.Forms.RadioButton
    Friend WithEvents rbtAll As System.Windows.Forms.RadioButton
    Friend WithEvents rbtMfg As System.Windows.Forms.RadioButton
    Friend WithEvents rbtTrading As System.Windows.Forms.RadioButton
    Friend WithEvents ChkBillPrefix As System.Windows.Forms.CheckBox
End Class
