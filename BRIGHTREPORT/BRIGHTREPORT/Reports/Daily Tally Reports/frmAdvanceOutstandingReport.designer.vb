<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAdvanceOutstandingReport
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
        Me.pnlFitration = New System.Windows.Forms.Panel
        Me.grp1 = New System.Windows.Forms.GroupBox
        Me.chkAcname = New System.Windows.Forms.CheckBox
        Me.txtAcName = New System.Windows.Forms.TextBox
        Me.chkcmbAcGrp = New BrighttechPack.CheckedComboBox
        Me.chkCmbAcName = New BrighttechPack.CheckedComboBox
        Me.Dgvsearch = New System.Windows.Forms.DataGridView
        Me.Label4 = New System.Windows.Forms.Label
        Me.cmbAccountType = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblCostCentre = New System.Windows.Forms.Label
        Me.cmbCostCentre = New System.Windows.Forms.ComboBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.chkPurReturn = New System.Windows.Forms.CheckBox
        Me.chkJandD = New System.Windows.Forms.CheckBox
        Me.chkCredit = New System.Windows.Forms.CheckBox
        Me.chkOrder = New System.Windows.Forms.CheckBox
        Me.chkOther = New System.Windows.Forms.CheckBox
        Me.chkAdvance = New System.Windows.Forms.CheckBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.rbtSummary = New System.Windows.Forms.RadioButton
        Me.rbtDetailed = New System.Windows.Forms.RadioButton
        Me.chkCompanySelectAll = New System.Windows.Forms.CheckBox
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.chkLstCompany = New System.Windows.Forms.CheckedListBox
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Label5 = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.Label6 = New System.Windows.Forms.Label
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.pnlGrid = New System.Windows.Forms.Panel
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.GridView2 = New System.Windows.Forms.DataGridView
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.FindSearchToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.pnlfooter = New System.Windows.Forms.Panel
        Me.btnSendSms = New System.Windows.Forms.Button
        Me.btnBack = New System.Windows.Forms.Button
        Me.pnlHeading = New System.Windows.Forms.Panel
        Me.lblHeading = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tabmain = New System.Windows.Forms.TabControl
        Me.tabGen = New System.Windows.Forms.TabPage
        Me.tabView = New System.Windows.Forms.TabPage
        Me.pnlFitration.SuspendLayout()
        Me.grp1.SuspendLayout()
        CType(Me.Dgvsearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.pnlGrid.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmbGridShortCut.SuspendLayout()
        Me.pnlfooter.SuspendLayout()
        Me.pnlHeading.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabmain.SuspendLayout()
        Me.tabGen.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlFitration
        '
        Me.pnlFitration.Controls.Add(Me.grp1)
        Me.pnlFitration.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlFitration.Location = New System.Drawing.Point(3, 3)
        Me.pnlFitration.Name = "pnlFitration"
        Me.pnlFitration.Size = New System.Drawing.Size(1014, 581)
        Me.pnlFitration.TabIndex = 0
        '
        'grp1
        '
        Me.grp1.Controls.Add(Me.chkAcname)
        Me.grp1.Controls.Add(Me.txtAcName)
        Me.grp1.Controls.Add(Me.chkcmbAcGrp)
        Me.grp1.Controls.Add(Me.chkCmbAcName)
        Me.grp1.Controls.Add(Me.Dgvsearch)
        Me.grp1.Controls.Add(Me.Label4)
        Me.grp1.Controls.Add(Me.cmbAccountType)
        Me.grp1.Controls.Add(Me.Label1)
        Me.grp1.Controls.Add(Me.lblCostCentre)
        Me.grp1.Controls.Add(Me.cmbCostCentre)
        Me.grp1.Controls.Add(Me.GroupBox2)
        Me.grp1.Controls.Add(Me.GroupBox1)
        Me.grp1.Controls.Add(Me.chkCompanySelectAll)
        Me.grp1.Controls.Add(Me.dtpTo)
        Me.grp1.Controls.Add(Me.chkLstCompany)
        Me.grp1.Controls.Add(Me.dtpFrom)
        Me.grp1.Controls.Add(Me.Label5)
        Me.grp1.Controls.Add(Me.btnExit)
        Me.grp1.Controls.Add(Me.btnNew)
        Me.grp1.Controls.Add(Me.btnView_Search)
        Me.grp1.Controls.Add(Me.Label6)
        Me.grp1.Location = New System.Drawing.Point(274, 3)
        Me.grp1.Name = "grp1"
        Me.grp1.Size = New System.Drawing.Size(450, 511)
        Me.grp1.TabIndex = 0
        Me.grp1.TabStop = False
        '
        'chkAcname
        '
        Me.chkAcname.AutoSize = True
        Me.chkAcname.Checked = True
        Me.chkAcname.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAcname.Location = New System.Drawing.Point(25, 299)
        Me.chkAcname.Name = "chkAcname"
        Me.chkAcname.Size = New System.Drawing.Size(77, 17)
        Me.chkAcname.TabIndex = 12
        Me.chkAcname.Text = "Ac Name"
        Me.chkAcname.UseVisualStyleBackColor = True
        '
        'txtAcName
        '
        Me.txtAcName.Location = New System.Drawing.Point(108, 295)
        Me.txtAcName.Name = "txtAcName"
        Me.txtAcName.Size = New System.Drawing.Size(294, 21)
        Me.txtAcName.TabIndex = 13
        Me.txtAcName.Visible = False
        '
        'chkcmbAcGrp
        '
        Me.chkcmbAcGrp.CheckOnClick = True
        Me.chkcmbAcGrp.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbAcGrp.DropDownHeight = 1
        Me.chkcmbAcGrp.FormattingEnabled = True
        Me.chkcmbAcGrp.IntegralHeight = False
        Me.chkcmbAcGrp.Location = New System.Drawing.Point(107, 257)
        Me.chkcmbAcGrp.Name = "chkcmbAcGrp"
        Me.chkcmbAcGrp.Size = New System.Drawing.Size(295, 22)
        Me.chkcmbAcGrp.TabIndex = 11
        Me.chkcmbAcGrp.ValueSeparator = ", "
        '
        'chkCmbAcName
        '
        Me.chkCmbAcName.CheckOnClick = True
        Me.chkCmbAcName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbAcName.DropDownHeight = 1
        Me.chkCmbAcName.FormattingEnabled = True
        Me.chkCmbAcName.IntegralHeight = False
        Me.chkCmbAcName.Location = New System.Drawing.Point(108, 294)
        Me.chkCmbAcName.Name = "chkCmbAcName"
        Me.chkCmbAcName.Size = New System.Drawing.Size(294, 22)
        Me.chkCmbAcName.TabIndex = 13
        Me.chkCmbAcName.ValueSeparator = ", "
        '
        'Dgvsearch
        '
        Me.Dgvsearch.AllowUserToAddRows = False
        Me.Dgvsearch.AllowUserToDeleteRows = False
        Me.Dgvsearch.AllowUserToResizeRows = False
        Me.Dgvsearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Dgvsearch.Location = New System.Drawing.Point(107, 495)
        Me.Dgvsearch.Name = "Dgvsearch"
        Me.Dgvsearch.Size = New System.Drawing.Size(295, 10)
        Me.Dgvsearch.TabIndex = 19
        Me.Dgvsearch.Visible = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(31, 260)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(60, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Ac Group"
        '
        'cmbAccountType
        '
        Me.cmbAccountType.FormattingEnabled = True
        Me.cmbAccountType.Location = New System.Drawing.Point(108, 223)
        Me.cmbAccountType.Name = "cmbAccountType"
        Me.cmbAccountType.Size = New System.Drawing.Size(294, 21)
        Me.cmbAccountType.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(31, 223)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 13)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Ac Type"
        '
        'lblCostCentre
        '
        Me.lblCostCentre.AutoSize = True
        Me.lblCostCentre.Location = New System.Drawing.Point(30, 186)
        Me.lblCostCentre.Name = "lblCostCentre"
        Me.lblCostCentre.Size = New System.Drawing.Size(76, 13)
        Me.lblCostCentre.TabIndex = 6
        Me.lblCostCentre.Text = "Cost Centre"
        '
        'cmbCostCentre
        '
        Me.cmbCostCentre.FormattingEnabled = True
        Me.cmbCostCentre.Location = New System.Drawing.Point(107, 182)
        Me.cmbCostCentre.Name = "cmbCostCentre"
        Me.cmbCostCentre.Size = New System.Drawing.Size(293, 21)
        Me.cmbCostCentre.TabIndex = 7
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.chkPurReturn)
        Me.GroupBox2.Controls.Add(Me.chkJandD)
        Me.GroupBox2.Controls.Add(Me.chkCredit)
        Me.GroupBox2.Controls.Add(Me.chkOrder)
        Me.GroupBox2.Controls.Add(Me.chkOther)
        Me.GroupBox2.Controls.Add(Me.chkAdvance)
        Me.GroupBox2.Location = New System.Drawing.Point(108, 322)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(291, 70)
        Me.GroupBox2.TabIndex = 14
        Me.GroupBox2.TabStop = False
        '
        'chkPurReturn
        '
        Me.chkPurReturn.AutoSize = True
        Me.chkPurReturn.Location = New System.Drawing.Point(171, 42)
        Me.chkPurReturn.Name = "chkPurReturn"
        Me.chkPurReturn.Size = New System.Drawing.Size(96, 17)
        Me.chkPurReturn.TabIndex = 5
        Me.chkPurReturn.Text = "Cr Purchase"
        Me.chkPurReturn.UseVisualStyleBackColor = True
        '
        'chkJandD
        '
        Me.chkJandD.AutoSize = True
        Me.chkJandD.Location = New System.Drawing.Point(83, 42)
        Me.chkJandD.Name = "chkJandD"
        Me.chkJandD.Size = New System.Drawing.Size(48, 17)
        Me.chkJandD.TabIndex = 4
        Me.chkJandD.Text = "JND"
        Me.chkJandD.UseVisualStyleBackColor = True
        '
        'chkCredit
        '
        Me.chkCredit.AutoSize = True
        Me.chkCredit.Location = New System.Drawing.Point(9, 42)
        Me.chkCredit.Name = "chkCredit"
        Me.chkCredit.Size = New System.Drawing.Size(61, 17)
        Me.chkCredit.TabIndex = 3
        Me.chkCredit.Text = "Credit"
        Me.chkCredit.UseVisualStyleBackColor = True
        '
        'chkOrder
        '
        Me.chkOrder.AutoSize = True
        Me.chkOrder.Location = New System.Drawing.Point(171, 19)
        Me.chkOrder.Name = "chkOrder"
        Me.chkOrder.Size = New System.Drawing.Size(59, 17)
        Me.chkOrder.TabIndex = 2
        Me.chkOrder.Text = "Order"
        Me.chkOrder.UseVisualStyleBackColor = True
        '
        'chkOther
        '
        Me.chkOther.AutoSize = True
        Me.chkOther.Location = New System.Drawing.Point(83, 19)
        Me.chkOther.Name = "chkOther"
        Me.chkOther.Size = New System.Drawing.Size(58, 17)
        Me.chkOther.TabIndex = 1
        Me.chkOther.Text = "Other"
        Me.chkOther.UseVisualStyleBackColor = True
        '
        'chkAdvance
        '
        Me.chkAdvance.AutoSize = True
        Me.chkAdvance.Location = New System.Drawing.Point(9, 19)
        Me.chkAdvance.Name = "chkAdvance"
        Me.chkAdvance.Size = New System.Drawing.Size(75, 17)
        Me.chkAdvance.TabIndex = 0
        Me.chkAdvance.Text = "Advance"
        Me.chkAdvance.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbtSummary)
        Me.GroupBox1.Controls.Add(Me.rbtDetailed)
        Me.GroupBox1.Location = New System.Drawing.Point(108, 392)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(291, 33)
        Me.GroupBox1.TabIndex = 15
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Visible = False
        '
        'rbtSummary
        '
        Me.rbtSummary.AutoSize = True
        Me.rbtSummary.Location = New System.Drawing.Point(110, 11)
        Me.rbtSummary.Name = "rbtSummary"
        Me.rbtSummary.Size = New System.Drawing.Size(81, 17)
        Me.rbtSummary.TabIndex = 1
        Me.rbtSummary.Text = "Summary"
        Me.rbtSummary.UseVisualStyleBackColor = True
        '
        'rbtDetailed
        '
        Me.rbtDetailed.AutoSize = True
        Me.rbtDetailed.Checked = True
        Me.rbtDetailed.Location = New System.Drawing.Point(15, 11)
        Me.rbtDetailed.Name = "rbtDetailed"
        Me.rbtDetailed.Size = New System.Drawing.Size(72, 17)
        Me.rbtDetailed.TabIndex = 0
        Me.rbtDetailed.TabStop = True
        Me.rbtDetailed.Text = "Detailed"
        Me.rbtDetailed.UseVisualStyleBackColor = True
        '
        'chkCompanySelectAll
        '
        Me.chkCompanySelectAll.AutoSize = True
        Me.chkCompanySelectAll.Location = New System.Drawing.Point(39, 73)
        Me.chkCompanySelectAll.Name = "chkCompanySelectAll"
        Me.chkCompanySelectAll.Size = New System.Drawing.Size(81, 17)
        Me.chkCompanySelectAll.TabIndex = 4
        Me.chkCompanySelectAll.Text = "Company"
        Me.chkCompanySelectAll.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(304, 39)
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
        'chkLstCompany
        '
        Me.chkLstCompany.FormattingEnabled = True
        Me.chkLstCompany.Location = New System.Drawing.Point(36, 92)
        Me.chkLstCompany.Name = "chkLstCompany"
        Me.chkLstCompany.Size = New System.Drawing.Size(364, 84)
        Me.chkLstCompany.TabIndex = 5
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(103, 39)
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
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(35, 42)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(67, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "From Date"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(237, 447)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 18
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(132, 447)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 17
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(27, 447)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 16
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(230, 42)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(52, 13)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "To Date"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(593, 6)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 20
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(700, 6)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 20
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'pnlGrid
        '
        Me.pnlGrid.Controls.Add(Me.Panel1)
        Me.pnlGrid.Controls.Add(Me.pnlfooter)
        Me.pnlGrid.Controls.Add(Me.pnlHeading)
        Me.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlGrid.Location = New System.Drawing.Point(3, 3)
        Me.pnlGrid.Name = "pnlGrid"
        Me.pnlGrid.Size = New System.Drawing.Size(1014, 581)
        Me.pnlGrid.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.GridView2)
        Me.Panel1.Controls.Add(Me.gridView)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 30)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1014, 513)
        Me.Panel1.TabIndex = 0
        '
        'GridView2
        '
        Me.GridView2.AllowUserToAddRows = False
        Me.GridView2.AllowUserToDeleteRows = False
        Me.GridView2.AllowUserToOrderColumns = True
        Me.GridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridView2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridView2.Location = New System.Drawing.Point(0, 0)
        Me.GridView2.Name = "GridView2"
        Me.GridView2.RowHeadersVisible = False
        Me.GridView2.Size = New System.Drawing.Size(1014, 513)
        Me.GridView2.TabIndex = 1
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
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 18
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(1014, 513)
        Me.gridView.TabIndex = 0
        '
        'cmbGridShortCut
        '
        Me.cmbGridShortCut.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResizeToolStripMenuItem, Me.FindSearchToolStripMenuItem})
        Me.cmbGridShortCut.Name = "ContextMenuStrip2"
        Me.cmbGridShortCut.Size = New System.Drawing.Size(143, 48)
        '
        'ResizeToolStripMenuItem
        '
        Me.ResizeToolStripMenuItem.CheckOnClick = True
        Me.ResizeToolStripMenuItem.Name = "ResizeToolStripMenuItem"
        Me.ResizeToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.ResizeToolStripMenuItem.Text = "Auto Resize"
        '
        'FindSearchToolStripMenuItem
        '
        Me.FindSearchToolStripMenuItem.Name = "FindSearchToolStripMenuItem"
        Me.FindSearchToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.FindSearchToolStripMenuItem.Text = "find search"
        '
        'pnlfooter
        '
        Me.pnlfooter.Controls.Add(Me.btnSendSms)
        Me.pnlfooter.Controls.Add(Me.btnBack)
        Me.pnlfooter.Controls.Add(Me.btnPrint)
        Me.pnlfooter.Controls.Add(Me.btnExport)
        Me.pnlfooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlfooter.Location = New System.Drawing.Point(0, 543)
        Me.pnlfooter.Name = "pnlfooter"
        Me.pnlfooter.Size = New System.Drawing.Size(1014, 38)
        Me.pnlfooter.TabIndex = 1
        '
        'btnSendSms
        '
        Me.btnSendSms.Enabled = False
        Me.btnSendSms.Location = New System.Drawing.Point(807, 6)
        Me.btnSendSms.Name = "btnSendSms"
        Me.btnSendSms.Size = New System.Drawing.Size(100, 30)
        Me.btnSendSms.TabIndex = 22
        Me.btnSendSms.Text = "SendSms"
        Me.btnSendSms.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(486, 6)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 21
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'pnlHeading
        '
        Me.pnlHeading.Controls.Add(Me.lblHeading)
        Me.pnlHeading.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeading.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeading.Name = "pnlHeading"
        Me.pnlHeading.Size = New System.Drawing.Size(1014, 30)
        Me.pnlHeading.TabIndex = 1
        '
        'lblHeading
        '
        Me.lblHeading.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblHeading.Location = New System.Drawing.Point(0, 0)
        Me.lblHeading.Name = "lblHeading"
        Me.lblHeading.Size = New System.Drawing.Size(1014, 30)
        Me.lblHeading.TabIndex = 0
        Me.lblHeading.Text = "Label10"
        Me.lblHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
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
        'tabmain
        '
        Me.tabmain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tabmain.Controls.Add(Me.tabGen)
        Me.tabmain.Controls.Add(Me.tabView)
        Me.tabmain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabmain.Location = New System.Drawing.Point(0, 0)
        Me.tabmain.Name = "tabmain"
        Me.tabmain.SelectedIndex = 0
        Me.tabmain.Size = New System.Drawing.Size(1028, 616)
        Me.tabmain.TabIndex = 0
        '
        'tabGen
        '
        Me.tabGen.Controls.Add(Me.pnlFitration)
        Me.tabGen.Location = New System.Drawing.Point(4, 25)
        Me.tabGen.Name = "tabGen"
        Me.tabGen.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGen.Size = New System.Drawing.Size(1020, 587)
        Me.tabGen.TabIndex = 0
        Me.tabGen.Text = "Gen"
        Me.tabGen.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.pnlGrid)
        Me.tabView.Location = New System.Drawing.Point(4, 25)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1020, 587)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'frmAdvanceOutstandingReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 616)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabmain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmAdvanceOutstandingReport"
        Me.Text = "Advance Outstanding Report"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlFitration.ResumeLayout(False)
        Me.grp1.ResumeLayout(False)
        Me.grp1.PerformLayout()
        CType(Me.Dgvsearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.pnlGrid.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmbGridShortCut.ResumeLayout(False)
        Me.pnlfooter.ResumeLayout(False)
        Me.pnlHeading.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabmain.ResumeLayout(False)
        Me.tabGen.ResumeLayout(False)
        Me.tabView.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlFitration As System.Windows.Forms.Panel
    Friend WithEvents pnlGrid As System.Windows.Forms.Panel
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents pnlHeading As System.Windows.Forms.Panel
    Friend WithEvents lblHeading As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents tabmain As System.Windows.Forms.TabControl
    Friend WithEvents tabGen As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents pnlfooter As System.Windows.Forms.Panel
    Friend WithEvents grp1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkCompanySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCompany As System.Windows.Forms.CheckedListBox
    Friend WithEvents rbtSummary As System.Windows.Forms.RadioButton
    Friend WithEvents rbtDetailed As System.Windows.Forms.RadioButton
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents chkJandD As System.Windows.Forms.CheckBox
    Friend WithEvents chkCredit As System.Windows.Forms.CheckBox
    Friend WithEvents chkOrder As System.Windows.Forms.CheckBox
    Friend WithEvents chkOther As System.Windows.Forms.CheckBox
    Friend WithEvents chkAdvance As System.Windows.Forms.CheckBox
    Friend WithEvents lblCostCentre As System.Windows.Forms.Label
    Friend WithEvents cmbCostCentre As System.Windows.Forms.ComboBox
    Friend WithEvents GridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents chkCmbAcName As BrighttechPack.CheckedComboBox
    Friend WithEvents cmbAccountType As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkcmbAcGrp As BrighttechPack.CheckedComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents chkPurReturn As System.Windows.Forms.CheckBox
    Friend WithEvents Dgvsearch As System.Windows.Forms.DataGridView
    Friend WithEvents FindSearchToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtAcName As System.Windows.Forms.TextBox
    Friend WithEvents chkAcname As System.Windows.Forms.CheckBox
    Friend WithEvents btnSendSms As System.Windows.Forms.Button
End Class
