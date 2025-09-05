<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPurchaseAbstract
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
        Me.rbtDate = New System.Windows.Forms.RadioButton
        Me.rbtMonth = New System.Windows.Forms.RadioButton
        Me.rbtSummary = New System.Windows.Forms.RadioButton
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.pnlHeading = New System.Windows.Forms.Panel
        Me.lblTitle = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExtToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.chkNetWt = New System.Windows.Forms.CheckBox
        Me.chkGrsWt = New System.Windows.Forms.CheckBox
        Me.chkPcs = New System.Windows.Forms.CheckBox
        Me.btnExport = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.ChkDateSep = New System.Windows.Forms.CheckBox
        Me.chkWithPurReturn = New System.Windows.Forms.CheckBox
        Me.cmbCategory = New BrighttechPack.CheckedComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.btn_dPrint = New System.Windows.Forms.Button
        Me.chkUrd = New System.Windows.Forms.CheckBox
        Me.chkRd = New System.Windows.Forms.CheckBox
        Me.GBmore = New System.Windows.Forms.GroupBox
        Me.ChkBillPrefix = New System.Windows.Forms.CheckBox
        Me.Chkstngrm = New System.Windows.Forms.CheckBox
        Me.ChkStnWt = New System.Windows.Forms.CheckBox
        Me.ChkStnAmt = New System.Windows.Forms.CheckBox
        Me.ChkDiaAmt = New System.Windows.Forms.CheckBox
        Me.Chkecs = New System.Windows.Forms.CheckBox
        Me.Chkparticular = New System.Windows.Forms.CheckBox
        Me.ChkDIAwt = New System.Windows.Forms.CheckBox
        Me.Chkmore = New System.Windows.Forms.CheckBox
        Me.chkEd = New System.Windows.Forms.CheckBox
        Me.chkVA = New System.Windows.Forms.CheckBox
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.btnPrint = New System.Windows.Forms.Button
        Me.cmbMetal = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.rbtBillNo = New System.Windows.Forms.RadioButton
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AutoReziseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Panel2.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlHeading.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GBmore.SuspendLayout()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'rbtDate
        '
        Me.rbtDate.AutoSize = True
        Me.rbtDate.Location = New System.Drawing.Point(193, 121)
        Me.rbtDate.Name = "rbtDate"
        Me.rbtDate.Size = New System.Drawing.Size(83, 17)
        Me.rbtDate.TabIndex = 16
        Me.rbtDate.Text = "Date Wise"
        Me.rbtDate.UseVisualStyleBackColor = True
        '
        'rbtMonth
        '
        Me.rbtMonth.AutoSize = True
        Me.rbtMonth.Location = New System.Drawing.Point(98, 121)
        Me.rbtMonth.Name = "rbtMonth"
        Me.rbtMonth.Size = New System.Drawing.Size(90, 17)
        Me.rbtMonth.TabIndex = 15
        Me.rbtMonth.Text = "Month Wise"
        Me.rbtMonth.UseVisualStyleBackColor = True
        '
        'rbtSummary
        '
        Me.rbtSummary.AutoSize = True
        Me.rbtSummary.Checked = True
        Me.rbtSummary.Location = New System.Drawing.Point(12, 121)
        Me.rbtSummary.Name = "rbtSummary"
        Me.rbtSummary.Size = New System.Drawing.Size(81, 17)
        Me.rbtSummary.TabIndex = 14
        Me.rbtSummary.TabStop = True
        Me.rbtSummary.Text = "Summary"
        Me.rbtSummary.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(479, 114)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 19
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(695, 114)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 21
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(371, 114)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 18
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.gridView)
        Me.Panel2.Controls.Add(Me.pnlHeading)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 152)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1020, 454)
        Me.Panel2.TabIndex = 3
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
        Me.gridView.RowTemplate.Height = 18
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(1020, 429)
        Me.gridView.TabIndex = 0
        '
        'pnlHeading
        '
        Me.pnlHeading.Controls.Add(Me.lblTitle)
        Me.pnlHeading.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeading.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeading.Name = "pnlHeading"
        Me.pnlHeading.Size = New System.Drawing.Size(1020, 25)
        Me.pnlHeading.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1020, 25)
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
        'chkNetWt
        '
        Me.chkNetWt.AutoSize = True
        Me.chkNetWt.Location = New System.Drawing.Point(479, 9)
        Me.chkNetWt.Name = "chkNetWt"
        Me.chkNetWt.Size = New System.Drawing.Size(60, 17)
        Me.chkNetWt.TabIndex = 6
        Me.chkNetWt.Text = "NetWt"
        Me.chkNetWt.UseVisualStyleBackColor = True
        '
        'chkGrsWt
        '
        Me.chkGrsWt.AutoSize = True
        Me.chkGrsWt.Location = New System.Drawing.Point(399, 9)
        Me.chkGrsWt.Name = "chkGrsWt"
        Me.chkGrsWt.Size = New System.Drawing.Size(74, 17)
        Me.chkGrsWt.TabIndex = 5
        Me.chkGrsWt.Text = "GrossWt"
        Me.chkGrsWt.UseVisualStyleBackColor = True
        '
        'chkPcs
        '
        Me.chkPcs.AutoSize = True
        Me.chkPcs.Location = New System.Drawing.Point(331, 9)
        Me.chkPcs.Name = "chkPcs"
        Me.chkPcs.Size = New System.Drawing.Size(62, 17)
        Me.chkPcs.TabIndex = 4
        Me.chkPcs.Text = "Pieces"
        Me.chkPcs.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(587, 114)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 20
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.ChkDateSep)
        Me.Panel1.Controls.Add(Me.chkWithPurReturn)
        Me.Panel1.Controls.Add(Me.cmbCategory)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.btn_dPrint)
        Me.Panel1.Controls.Add(Me.chkUrd)
        Me.Panel1.Controls.Add(Me.chkRd)
        Me.Panel1.Controls.Add(Me.GBmore)
        Me.Panel1.Controls.Add(Me.Chkmore)
        Me.Panel1.Controls.Add(Me.chkEd)
        Me.Panel1.Controls.Add(Me.chkVA)
        Me.Panel1.Controls.Add(Me.chkCmbCostCentre)
        Me.Panel1.Controls.Add(Me.dtpTo)
        Me.Panel1.Controls.Add(Me.dtpFrom)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.cmbMetal)
        Me.Panel1.Controls.Add(Me.chkNetWt)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.chkGrsWt)
        Me.Panel1.Controls.Add(Me.chkPcs)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.Label2)
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
        Me.Panel1.Size = New System.Drawing.Size(1020, 152)
        Me.Panel1.TabIndex = 1
        '
        'ChkDateSep
        '
        Me.ChkDateSep.AutoSize = True
        Me.ChkDateSep.Location = New System.Drawing.Point(479, 49)
        Me.ChkDateSep.Name = "ChkDateSep"
        Me.ChkDateSep.Size = New System.Drawing.Size(147, 17)
        Me.ChkDateSep.TabIndex = 31
        Me.ChkDateSep.Text = "Sep Column for Date"
        Me.ChkDateSep.UseVisualStyleBackColor = True
        '
        'chkWithPurReturn
        '
        Me.chkWithPurReturn.AutoSize = True
        Me.chkWithPurReturn.Location = New System.Drawing.Point(545, 8)
        Me.chkWithPurReturn.Name = "chkWithPurReturn"
        Me.chkWithPurReturn.Size = New System.Drawing.Size(149, 17)
        Me.chkWithPurReturn.TabIndex = 30
        Me.chkWithPurReturn.Text = "With Purchase Return"
        Me.chkWithPurReturn.UseVisualStyleBackColor = True
        '
        'cmbCategory
        '
        Me.cmbCategory.CheckOnClick = True
        Me.cmbCategory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbCategory.DropDownHeight = 1
        Me.cmbCategory.FormattingEnabled = True
        Me.cmbCategory.IntegralHeight = False
        Me.cmbCategory.Location = New System.Drawing.Point(399, 81)
        Me.cmbCategory.Name = "cmbCategory"
        Me.cmbCategory.Size = New System.Drawing.Size(383, 22)
        Me.cmbCategory.TabIndex = 29
        Me.cmbCategory.ValueSeparator = ", "
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(327, 86)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(60, 13)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Category"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btn_dPrint
        '
        Me.btn_dPrint.Location = New System.Drawing.Point(912, 114)
        Me.btn_dPrint.Name = "btn_dPrint"
        Me.btn_dPrint.Size = New System.Drawing.Size(100, 30)
        Me.btn_dPrint.TabIndex = 23
        Me.btn_dPrint.Text = "Detail Print"
        Me.btn_dPrint.UseVisualStyleBackColor = True
        Me.btn_dPrint.Visible = False
        '
        'chkUrd
        '
        Me.chkUrd.AutoSize = True
        Me.chkUrd.Checked = True
        Me.chkUrd.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkUrd.Location = New System.Drawing.Point(331, 49)
        Me.chkUrd.Name = "chkUrd"
        Me.chkUrd.Size = New System.Drawing.Size(51, 17)
        Me.chkUrd.TabIndex = 26
        Me.chkUrd.Text = "URD"
        Me.chkUrd.UseVisualStyleBackColor = True
        '
        'chkRd
        '
        Me.chkRd.AutoSize = True
        Me.chkRd.Location = New System.Drawing.Point(399, 49)
        Me.chkRd.Name = "chkRd"
        Me.chkRd.Size = New System.Drawing.Size(43, 17)
        Me.chkRd.TabIndex = 27
        Me.chkRd.Text = "RD"
        Me.chkRd.UseVisualStyleBackColor = True
        '
        'GBmore
        '
        Me.GBmore.Controls.Add(Me.ChkBillPrefix)
        Me.GBmore.Controls.Add(Me.Chkstngrm)
        Me.GBmore.Controls.Add(Me.ChkStnWt)
        Me.GBmore.Controls.Add(Me.ChkStnAmt)
        Me.GBmore.Controls.Add(Me.ChkDiaAmt)
        Me.GBmore.Controls.Add(Me.Chkecs)
        Me.GBmore.Controls.Add(Me.Chkparticular)
        Me.GBmore.Controls.Add(Me.ChkDIAwt)
        Me.GBmore.Location = New System.Drawing.Point(831, 20)
        Me.GBmore.Name = "GBmore"
        Me.GBmore.Size = New System.Drawing.Size(160, 92)
        Me.GBmore.TabIndex = 28
        Me.GBmore.TabStop = False
        Me.GBmore.Visible = False
        '
        'ChkBillPrefix
        '
        Me.ChkBillPrefix.AutoSize = True
        Me.ChkBillPrefix.Location = New System.Drawing.Point(6, 72)
        Me.ChkBillPrefix.Name = "ChkBillPrefix"
        Me.ChkBillPrefix.Size = New System.Drawing.Size(80, 17)
        Me.ChkBillPrefix.TabIndex = 34
        Me.ChkBillPrefix.Text = "Bill Prefix"
        Me.ChkBillPrefix.UseVisualStyleBackColor = True
        '
        'Chkstngrm
        '
        Me.Chkstngrm.AutoSize = True
        Me.Chkstngrm.Checked = True
        Me.Chkstngrm.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Chkstngrm.Location = New System.Drawing.Point(79, 12)
        Me.Chkstngrm.Name = "Chkstngrm"
        Me.Chkstngrm.Size = New System.Drawing.Size(68, 17)
        Me.Chkstngrm.TabIndex = 33
        Me.Chkstngrm.Text = "Stngrm"
        Me.Chkstngrm.UseVisualStyleBackColor = True
        '
        'ChkStnWt
        '
        Me.ChkStnWt.AutoSize = True
        Me.ChkStnWt.Checked = True
        Me.ChkStnWt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkStnWt.Location = New System.Drawing.Point(6, 10)
        Me.ChkStnWt.Name = "ChkStnWt"
        Me.ChkStnWt.Size = New System.Drawing.Size(63, 17)
        Me.ChkStnWt.TabIndex = 32
        Me.ChkStnWt.Text = "StnCrt"
        Me.ChkStnWt.UseVisualStyleBackColor = True
        '
        'ChkStnAmt
        '
        Me.ChkStnAmt.AutoSize = True
        Me.ChkStnAmt.Checked = True
        Me.ChkStnAmt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkStnAmt.Location = New System.Drawing.Point(79, 31)
        Me.ChkStnAmt.Name = "ChkStnAmt"
        Me.ChkStnAmt.Size = New System.Drawing.Size(68, 17)
        Me.ChkStnAmt.TabIndex = 30
        Me.ChkStnAmt.Text = "StnAmt"
        Me.ChkStnAmt.UseVisualStyleBackColor = True
        '
        'ChkDiaAmt
        '
        Me.ChkDiaAmt.AutoSize = True
        Me.ChkDiaAmt.Checked = True
        Me.ChkDiaAmt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkDiaAmt.Location = New System.Drawing.Point(79, 54)
        Me.ChkDiaAmt.Name = "ChkDiaAmt"
        Me.ChkDiaAmt.Size = New System.Drawing.Size(68, 17)
        Me.ChkDiaAmt.TabIndex = 31
        Me.ChkDiaAmt.Text = "DiaAmt"
        Me.ChkDiaAmt.UseVisualStyleBackColor = True
        '
        'Chkecs
        '
        Me.Chkecs.AutoSize = True
        Me.Chkecs.Checked = True
        Me.Chkecs.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Chkecs.Location = New System.Drawing.Point(6, 56)
        Me.Chkecs.Name = "Chkecs"
        Me.Chkecs.Size = New System.Drawing.Size(50, 17)
        Me.Chkecs.TabIndex = 29
        Me.Chkecs.Text = "ECS"
        Me.Chkecs.UseVisualStyleBackColor = True
        Me.Chkecs.Visible = False
        '
        'Chkparticular
        '
        Me.Chkparticular.AutoSize = True
        Me.Chkparticular.Checked = True
        Me.Chkparticular.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Chkparticular.Location = New System.Drawing.Point(6, 40)
        Me.Chkparticular.Name = "Chkparticular"
        Me.Chkparticular.Size = New System.Drawing.Size(80, 17)
        Me.Chkparticular.TabIndex = 28
        Me.Chkparticular.Text = "Particular"
        Me.Chkparticular.UseVisualStyleBackColor = True
        '
        'ChkDIAwt
        '
        Me.ChkDIAwt.AutoSize = True
        Me.ChkDIAwt.Checked = True
        Me.ChkDIAwt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkDIAwt.Location = New System.Drawing.Point(6, 24)
        Me.ChkDIAwt.Name = "ChkDIAwt"
        Me.ChkDIAwt.Size = New System.Drawing.Size(60, 17)
        Me.ChkDIAwt.TabIndex = 26
        Me.ChkDIAwt.Text = "DiaWt"
        Me.ChkDIAwt.UseVisualStyleBackColor = True
        '
        'Chkmore
        '
        Me.Chkmore.AutoSize = True
        Me.Chkmore.Location = New System.Drawing.Point(773, 35)
        Me.Chkmore.Name = "Chkmore"
        Me.Chkmore.Size = New System.Drawing.Size(54, 17)
        Me.Chkmore.TabIndex = 25
        Me.Chkmore.Text = "More"
        Me.Chkmore.UseVisualStyleBackColor = True
        '
        'chkEd
        '
        Me.chkEd.AutoSize = True
        Me.chkEd.Location = New System.Drawing.Point(696, 35)
        Me.chkEd.Name = "chkEd"
        Me.chkEd.Size = New System.Drawing.Size(71, 17)
        Me.chkEd.TabIndex = 24
        Me.chkEd.Text = "With ED"
        Me.chkEd.UseVisualStyleBackColor = True
        '
        'chkVA
        '
        Me.chkVA.AutoSize = True
        Me.chkVA.Location = New System.Drawing.Point(696, 8)
        Me.chkVA.Name = "chkVA"
        Me.chkVA.Size = New System.Drawing.Size(257, 17)
        Me.chkVA.TabIndex = 7
        Me.chkVA.Text = "With &Vertical Address && Payment Details"
        Me.chkVA.UseVisualStyleBackColor = True
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(94, 44)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(226, 22)
        Me.chkCmbCostCentre.TabIndex = 9
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(229, 8)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(91, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(95, 8)
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
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(803, 114)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 22
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'cmbMetal
        '
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(96, 86)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(224, 21)
        Me.cmbMetal.TabIndex = 11
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 90)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(37, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Metal"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(199, 7)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(23, 21)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 48)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(12, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 21)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date From"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'rbtBillNo
        '
        Me.rbtBillNo.AutoSize = True
        Me.rbtBillNo.Location = New System.Drawing.Point(281, 121)
        Me.rbtBillNo.Name = "rbtBillNo"
        Me.rbtBillNo.Size = New System.Drawing.Size(88, 17)
        Me.rbtBillNo.TabIndex = 17
        Me.rbtBillNo.Text = "BillNo Wise"
        Me.rbtBillNo.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AutoReziseToolStripMenuItem})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(153, 48)
        '
        'AutoReziseToolStripMenuItem
        '
        Me.AutoReziseToolStripMenuItem.CheckOnClick = True
        Me.AutoReziseToolStripMenuItem.Name = "AutoReziseToolStripMenuItem"
        Me.AutoReziseToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.AutoReziseToolStripMenuItem.Text = "AutoRezise"
        '
        'frmPurchaseAbstract
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1020, 606)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmPurchaseAbstract"
        Me.Text = "PURCHASE ABSTRACT REPORT"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel2.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlHeading.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GBmore.ResumeLayout(False)
        Me.GBmore.PerformLayout()
        Me.ContextMenuStrip2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents rbtDate As System.Windows.Forms.RadioButton
    Friend WithEvents rbtMonth As System.Windows.Forms.RadioButton
    Friend WithEvents rbtSummary As System.Windows.Forms.RadioButton
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents pnlHeading As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExtToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkNetWt As System.Windows.Forms.CheckBox
    Friend WithEvents chkGrsWt As System.Windows.Forms.CheckBox
    Friend WithEvents chkPcs As System.Windows.Forms.CheckBox
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents rbtBillNo As System.Windows.Forms.RadioButton
    Friend WithEvents cmbMetal As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents chkVA As System.Windows.Forms.CheckBox
    Friend WithEvents chkEd As System.Windows.Forms.CheckBox
    Friend WithEvents GBmore As System.Windows.Forms.GroupBox
    Friend WithEvents Chkecs As System.Windows.Forms.CheckBox
    Friend WithEvents Chkparticular As System.Windows.Forms.CheckBox
    Friend WithEvents ChkDIAwt As System.Windows.Forms.CheckBox
    Friend WithEvents Chkmore As System.Windows.Forms.CheckBox
    Friend WithEvents chkUrd As System.Windows.Forms.CheckBox
    Friend WithEvents chkRd As System.Windows.Forms.CheckBox
    Friend WithEvents btn_dPrint As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cmbCategory As BrighttechPack.CheckedComboBox
    Friend WithEvents chkWithPurReturn As System.Windows.Forms.CheckBox
    Friend WithEvents ChkStnAmt As System.Windows.Forms.CheckBox
    Friend WithEvents ChkDiaAmt As System.Windows.Forms.CheckBox
    Friend WithEvents Chkstngrm As System.Windows.Forms.CheckBox
    Friend WithEvents ChkStnWt As System.Windows.Forms.CheckBox
    Friend WithEvents ChkDateSep As System.Windows.Forms.CheckBox
    Friend WithEvents ChkBillPrefix As System.Windows.Forms.CheckBox
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AutoReziseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
