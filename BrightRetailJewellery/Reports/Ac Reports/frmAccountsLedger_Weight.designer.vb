<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAccountsLedger_Weight
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
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.grpFields = New System.Windows.Forms.GroupBox
        Me.ChkSummaryBalance = New System.Windows.Forms.CheckBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.chkCmbCompany = New BrighttechPack.CheckedComboBox
        Me.chkGrpTranNo = New System.Windows.Forms.CheckBox
        Me.chkOrderbyTranNo = New System.Windows.Forms.CheckBox
        Me.chkWithRunningBalance = New System.Windows.Forms.CheckBox
        Me.chkMultiSelect = New System.Windows.Forms.CheckBox
        Me.cmbAcName = New System.Windows.Forms.ComboBox
        Me.cmbAcGroup = New System.Windows.Forms.ComboBox
        Me.chkCmbAcName = New BrighttechPack.CheckedComboBox
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.chkCmbAcGroup = New BrighttechPack.CheckedComboBox
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnMore = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.btnNew = New System.Windows.Forms.Button
        Me.tabView = New System.Windows.Forms.TabPage
        Me.pnlGrid = New System.Windows.Forms.Panel
        Me.DataGridView2 = New System.Windows.Forms.DataGridView
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.grpReportOptions = New System.Windows.Forms.GroupBox
        Me.btnReport = New System.Windows.Forms.Button
        Me.chkWithPcsWeight = New System.Windows.Forms.CheckBox
        Me.chkWithNarraion = New System.Windows.Forms.CheckBox
        Me.grpSumOfSelectedRows = New System.Windows.Forms.GroupBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.txtSumCredit = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.txtSumDebit = New System.Windows.Forms.TextBox
        Me.grpBillView = New System.Windows.Forms.GroupBox
        Me.gridBillView = New System.Windows.Forms.DataGridView
        Me.gridView_OWN = New System.Windows.Forms.DataGridView
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.FindNextToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.FindSearchToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SumOfSelectedRowsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.pnlHeading = New System.Windows.Forms.Panel
        Me.lblTitle = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.pnlFooter = New System.Windows.Forms.Panel
        Me.btnChqPrint = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.btnAccountInfo = New System.Windows.Forms.Button
        Me.btnDuplicatePrint = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnContraSummary = New System.Windows.Forms.Button
        Me.btnEdit = New System.Windows.Forms.Button
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.btnSumOfSelectedRows = New System.Windows.Forms.Button
        Me.btnFindSearch = New System.Windows.Forms.Button
        Me.btnBack = New System.Windows.Forms.Button
        Me.gridSummary = New System.Windows.Forms.DataGridView
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.grpFields.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.pnlGrid.SuspendLayout()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpReportOptions.SuspendLayout()
        Me.grpSumOfSelectedRows.SuspendLayout()
        Me.grpBillView.SuspendLayout()
        CType(Me.gridBillView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.pnlHeading.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pnlFooter.SuspendLayout()
        CType(Me.gridSummary, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tabMain
        '
        Me.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tabMain.Controls.Add(Me.tabGeneral)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1022, 640)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.ContextMenuStrip = Me.ContextMenuStrip1
        Me.tabGeneral.Controls.Add(Me.grpFields)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 25)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(1014, 611)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.NewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 48)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'grpFields
        '
        Me.grpFields.Controls.Add(Me.ChkSummaryBalance)
        Me.grpFields.Controls.Add(Me.Label6)
        Me.grpFields.Controls.Add(Me.chkCmbCompany)
        Me.grpFields.Controls.Add(Me.chkGrpTranNo)
        Me.grpFields.Controls.Add(Me.chkOrderbyTranNo)
        Me.grpFields.Controls.Add(Me.chkWithRunningBalance)
        Me.grpFields.Controls.Add(Me.chkMultiSelect)
        Me.grpFields.Controls.Add(Me.cmbAcName)
        Me.grpFields.Controls.Add(Me.cmbAcGroup)
        Me.grpFields.Controls.Add(Me.chkCmbAcName)
        Me.grpFields.Controls.Add(Me.chkCmbCostCentre)
        Me.grpFields.Controls.Add(Me.chkCmbAcGroup)
        Me.grpFields.Controls.Add(Me.dtpTo)
        Me.grpFields.Controls.Add(Me.dtpFrom)
        Me.grpFields.Controls.Add(Me.Label3)
        Me.grpFields.Controls.Add(Me.Label4)
        Me.grpFields.Controls.Add(Me.Label2)
        Me.grpFields.Controls.Add(Me.btnView_Search)
        Me.grpFields.Controls.Add(Me.Label1)
        Me.grpFields.Controls.Add(Me.btnExit)
        Me.grpFields.Controls.Add(Me.btnMore)
        Me.grpFields.Controls.Add(Me.Label5)
        Me.grpFields.Controls.Add(Me.btnNew)
        Me.grpFields.Location = New System.Drawing.Point(247, 73)
        Me.grpFields.Name = "grpFields"
        Me.grpFields.Size = New System.Drawing.Size(560, 430)
        Me.grpFields.TabIndex = 0
        Me.grpFields.TabStop = False
        '
        'ChkSummaryBalance
        '
        Me.ChkSummaryBalance.AutoSize = True
        Me.ChkSummaryBalance.Location = New System.Drawing.Point(91, 313)
        Me.ChkSummaryBalance.Name = "ChkSummaryBalance"
        Me.ChkSummaryBalance.Size = New System.Drawing.Size(159, 17)
        Me.ChkSummaryBalance.TabIndex = 14
        Me.ChkSummaryBalance.Text = "Summary Ledger Only "
        Me.ChkSummaryBalance.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(16, 200)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(62, 13)
        Me.Label6.TabIndex = 7
        Me.Label6.Text = "Company"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbCompany
        '
        Me.chkCmbCompany.CheckOnClick = True
        Me.chkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCompany.DropDownHeight = 1
        Me.chkCmbCompany.FormattingEnabled = True
        Me.chkCmbCompany.IntegralHeight = False
        Me.chkCmbCompany.Location = New System.Drawing.Point(89, 197)
        Me.chkCmbCompany.Name = "chkCmbCompany"
        Me.chkCmbCompany.Size = New System.Drawing.Size(408, 22)
        Me.chkCmbCompany.TabIndex = 8
        Me.chkCmbCompany.ValueSeparator = ", "
        '
        'chkGrpTranNo
        '
        Me.chkGrpTranNo.AutoSize = True
        Me.chkGrpTranNo.Location = New System.Drawing.Point(375, 288)
        Me.chkGrpTranNo.Name = "chkGrpTranNo"
        Me.chkGrpTranNo.Size = New System.Drawing.Size(124, 17)
        Me.chkGrpTranNo.TabIndex = 13
        Me.chkGrpTranNo.Text = "Group by TranNo"
        Me.chkGrpTranNo.UseVisualStyleBackColor = True
        '
        'chkOrderbyTranNo
        '
        Me.chkOrderbyTranNo.AutoSize = True
        Me.chkOrderbyTranNo.Location = New System.Drawing.Point(247, 288)
        Me.chkOrderbyTranNo.Name = "chkOrderbyTranNo"
        Me.chkOrderbyTranNo.Size = New System.Drawing.Size(122, 17)
        Me.chkOrderbyTranNo.TabIndex = 12
        Me.chkOrderbyTranNo.Text = "Order by TranNo"
        Me.chkOrderbyTranNo.UseVisualStyleBackColor = True
        '
        'chkWithRunningBalance
        '
        Me.chkWithRunningBalance.AutoSize = True
        Me.chkWithRunningBalance.Location = New System.Drawing.Point(91, 288)
        Me.chkWithRunningBalance.Name = "chkWithRunningBalance"
        Me.chkWithRunningBalance.Size = New System.Drawing.Size(150, 17)
        Me.chkWithRunningBalance.TabIndex = 11
        Me.chkWithRunningBalance.Text = "With Running Balance"
        Me.chkWithRunningBalance.UseVisualStyleBackColor = True
        '
        'chkMultiSelect
        '
        Me.chkMultiSelect.AutoSize = True
        Me.chkMultiSelect.Checked = True
        Me.chkMultiSelect.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMultiSelect.Location = New System.Drawing.Point(93, 26)
        Me.chkMultiSelect.Name = "chkMultiSelect"
        Me.chkMultiSelect.Size = New System.Drawing.Size(91, 17)
        Me.chkMultiSelect.TabIndex = 0
        Me.chkMultiSelect.Text = "Multi Select"
        Me.chkMultiSelect.UseVisualStyleBackColor = True
        '
        'cmbAcName
        '
        Me.cmbAcName.FormattingEnabled = True
        Me.cmbAcName.Location = New System.Drawing.Point(89, 154)
        Me.cmbAcName.Name = "cmbAcName"
        Me.cmbAcName.Size = New System.Drawing.Size(408, 21)
        Me.cmbAcName.TabIndex = 6
        '
        'cmbAcGroup
        '
        Me.cmbAcGroup.FormattingEnabled = True
        Me.cmbAcGroup.Location = New System.Drawing.Point(89, 81)
        Me.cmbAcGroup.Name = "cmbAcGroup"
        Me.cmbAcGroup.Size = New System.Drawing.Size(408, 21)
        Me.cmbAcGroup.TabIndex = 3
        '
        'chkCmbAcName
        '
        Me.chkCmbAcName.CheckOnClick = True
        Me.chkCmbAcName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbAcName.DropDownHeight = 1
        Me.chkCmbAcName.FormattingEnabled = True
        Me.chkCmbAcName.IntegralHeight = False
        Me.chkCmbAcName.Location = New System.Drawing.Point(89, 126)
        Me.chkCmbAcName.Name = "chkCmbAcName"
        Me.chkCmbAcName.Size = New System.Drawing.Size(408, 22)
        Me.chkCmbAcName.TabIndex = 5
        Me.chkCmbAcName.ValueSeparator = ", "
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(89, 238)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(408, 22)
        Me.chkCmbCostCentre.TabIndex = 10
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'chkCmbAcGroup
        '
        Me.chkCmbAcGroup.CheckOnClick = True
        Me.chkCmbAcGroup.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbAcGroup.DropDownHeight = 1
        Me.chkCmbAcGroup.FormattingEnabled = True
        Me.chkCmbAcGroup.IntegralHeight = False
        Me.chkCmbAcGroup.Location = New System.Drawing.Point(89, 53)
        Me.chkCmbAcGroup.Name = "chkCmbAcGroup"
        Me.chkCmbAcGroup.Size = New System.Drawing.Size(408, 22)
        Me.chkCmbAcGroup.TabIndex = 2
        Me.chkCmbAcGroup.ValueSeparator = ", "
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(209, 343)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpTo.TabIndex = 18
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(89, 343)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(93, 21)
        Me.dtpFrom.TabIndex = 16
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(15, 241)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(16, 347)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(67, 13)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "Date From"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(15, 129)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(66, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "A/C Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(89, 385)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 20
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 56)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "A/C Group"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(315, 385)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 22
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnMore
        '
        Me.btnMore.Location = New System.Drawing.Point(316, 343)
        Me.btnMore.Name = "btnMore"
        Me.btnMore.Size = New System.Drawing.Size(100, 21)
        Me.btnMore.TabIndex = 19
        Me.btnMore.Text = "&More"
        Me.btnMore.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(188, 347)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(21, 13)
        Me.Label5.TabIndex = 17
        Me.Label5.Text = "To"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(202, 385)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 21
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.ContextMenuStrip = Me.ContextMenuStrip1
        Me.tabView.Controls.Add(Me.pnlGrid)
        Me.tabView.Controls.Add(Me.Panel1)
        Me.tabView.Location = New System.Drawing.Point(4, 25)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1014, 611)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'pnlGrid
        '
        Me.pnlGrid.Controls.Add(Me.DataGridView2)
        Me.pnlGrid.Controls.Add(Me.DataGridView1)
        Me.pnlGrid.Controls.Add(Me.grpReportOptions)
        Me.pnlGrid.Controls.Add(Me.grpSumOfSelectedRows)
        Me.pnlGrid.Controls.Add(Me.grpBillView)
        Me.pnlGrid.Controls.Add(Me.gridView_OWN)
        Me.pnlGrid.Controls.Add(Me.pnlHeading)
        Me.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlGrid.Location = New System.Drawing.Point(3, 3)
        Me.pnlGrid.Name = "pnlGrid"
        Me.pnlGrid.Size = New System.Drawing.Size(1008, 413)
        Me.pnlGrid.TabIndex = 1
        '
        'DataGridView2
        '
        Me.DataGridView2.AllowUserToAddRows = False
        Me.DataGridView2.AllowUserToDeleteRows = False
        Me.DataGridView2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataGridView2.ColumnHeadersHeight = 30
        Me.DataGridView2.Location = New System.Drawing.Point(36, 40)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.ReadOnly = True
        Me.DataGridView2.RowHeadersVisible = False
        Me.DataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView2.Size = New System.Drawing.Size(897, 381)
        Me.DataGridView2.TabIndex = 7
        Me.DataGridView2.Visible = False
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(36, 40)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(897, 381)
        Me.DataGridView1.TabIndex = 6
        Me.DataGridView1.Visible = False
        '
        'grpReportOptions
        '
        Me.grpReportOptions.Controls.Add(Me.btnReport)
        Me.grpReportOptions.Controls.Add(Me.chkWithPcsWeight)
        Me.grpReportOptions.Controls.Add(Me.chkWithNarraion)
        Me.grpReportOptions.Location = New System.Drawing.Point(288, 183)
        Me.grpReportOptions.Name = "grpReportOptions"
        Me.grpReportOptions.Size = New System.Drawing.Size(353, 56)
        Me.grpReportOptions.TabIndex = 0
        Me.grpReportOptions.TabStop = False
        Me.grpReportOptions.Text = "Report Options"
        '
        'btnReport
        '
        Me.btnReport.Location = New System.Drawing.Point(240, 19)
        Me.btnReport.Name = "btnReport"
        Me.btnReport.Size = New System.Drawing.Size(75, 25)
        Me.btnReport.TabIndex = 2
        Me.btnReport.Text = "Report"
        Me.btnReport.UseVisualStyleBackColor = True
        '
        'chkWithPcsWeight
        '
        Me.chkWithPcsWeight.AutoSize = True
        Me.chkWithPcsWeight.Location = New System.Drawing.Point(116, 25)
        Me.chkWithPcsWeight.Name = "chkWithPcsWeight"
        Me.chkWithPcsWeight.Size = New System.Drawing.Size(117, 17)
        Me.chkWithPcsWeight.TabIndex = 1
        Me.chkWithPcsWeight.Text = "With Pcs Weight"
        Me.chkWithPcsWeight.UseVisualStyleBackColor = True
        '
        'chkWithNarraion
        '
        Me.chkWithNarraion.AutoSize = True
        Me.chkWithNarraion.Location = New System.Drawing.Point(6, 25)
        Me.chkWithNarraion.Name = "chkWithNarraion"
        Me.chkWithNarraion.Size = New System.Drawing.Size(104, 17)
        Me.chkWithNarraion.TabIndex = 0
        Me.chkWithNarraion.Text = "With Narraion"
        Me.chkWithNarraion.UseVisualStyleBackColor = True
        '
        'grpSumOfSelectedRows
        '
        Me.grpSumOfSelectedRows.Controls.Add(Me.Label12)
        Me.grpSumOfSelectedRows.Controls.Add(Me.txtSumCredit)
        Me.grpSumOfSelectedRows.Controls.Add(Me.Label11)
        Me.grpSumOfSelectedRows.Controls.Add(Me.txtSumDebit)
        Me.grpSumOfSelectedRows.Location = New System.Drawing.Point(288, 127)
        Me.grpSumOfSelectedRows.Name = "grpSumOfSelectedRows"
        Me.grpSumOfSelectedRows.Size = New System.Drawing.Size(353, 50)
        Me.grpSumOfSelectedRows.TabIndex = 5
        Me.grpSumOfSelectedRows.TabStop = False
        Me.grpSumOfSelectedRows.Text = "Sum of Selected Rows"
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(178, 17)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(52, 21)
        Me.Label12.TabIndex = 2
        Me.Label12.Text = "Credit"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtSumCredit
        '
        Me.txtSumCredit.Enabled = False
        Me.txtSumCredit.Location = New System.Drawing.Point(236, 17)
        Me.txtSumCredit.Name = "txtSumCredit"
        Me.txtSumCredit.Size = New System.Drawing.Size(96, 21)
        Me.txtSumCredit.TabIndex = 3
        Me.txtSumCredit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(21, 17)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(52, 21)
        Me.Label11.TabIndex = 2
        Me.Label11.Text = "Debit"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtSumDebit
        '
        Me.txtSumDebit.Enabled = False
        Me.txtSumDebit.Location = New System.Drawing.Point(79, 17)
        Me.txtSumDebit.Name = "txtSumDebit"
        Me.txtSumDebit.Size = New System.Drawing.Size(96, 21)
        Me.txtSumDebit.TabIndex = 3
        Me.txtSumDebit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'grpBillView
        '
        Me.grpBillView.Controls.Add(Me.gridBillView)
        Me.grpBillView.Location = New System.Drawing.Point(786, 220)
        Me.grpBillView.Name = "grpBillView"
        Me.grpBillView.Size = New System.Drawing.Size(168, 201)
        Me.grpBillView.TabIndex = 4
        Me.grpBillView.TabStop = False
        Me.grpBillView.Text = "Bill View"
        '
        'gridBillView
        '
        Me.gridBillView.AllowUserToAddRows = False
        Me.gridBillView.AllowUserToDeleteRows = False
        Me.gridBillView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridBillView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridBillView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridBillView.Location = New System.Drawing.Point(3, 17)
        Me.gridBillView.Name = "gridBillView"
        Me.gridBillView.ReadOnly = True
        Me.gridBillView.RowHeadersVisible = False
        Me.gridBillView.RowTemplate.Height = 20
        Me.gridBillView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridBillView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridBillView.Size = New System.Drawing.Size(162, 181)
        Me.gridBillView.TabIndex = 0
        '
        'gridView_OWN
        '
        Me.gridView_OWN.AllowUserToAddRows = False
        Me.gridView_OWN.AllowUserToDeleteRows = False
        Me.gridView_OWN.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridView_OWN.ColumnHeadersHeight = 30
        Me.gridView_OWN.ContextMenuStrip = Me.ContextMenuStrip2
        Me.gridView_OWN.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView_OWN.Location = New System.Drawing.Point(0, 40)
        Me.gridView_OWN.Name = "gridView_OWN"
        Me.gridView_OWN.ReadOnly = True
        Me.gridView_OWN.RowHeadersVisible = False
        Me.gridView_OWN.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView_OWN.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView_OWN.Size = New System.Drawing.Size(1008, 373)
        Me.gridView_OWN.TabIndex = 0
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FindNextToolStripMenuItem, Me.FindSearchToolStripMenuItem, Me.SumOfSelectedRowsToolStripMenuItem})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(216, 70)
        '
        'FindNextToolStripMenuItem
        '
        Me.FindNextToolStripMenuItem.Name = "FindNextToolStripMenuItem"
        Me.FindNextToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.FindNextToolStripMenuItem.Size = New System.Drawing.Size(215, 22)
        Me.FindNextToolStripMenuItem.Text = "Find Next"
        Me.FindNextToolStripMenuItem.Visible = False
        '
        'FindSearchToolStripMenuItem
        '
        Me.FindSearchToolStripMenuItem.Name = "FindSearchToolStripMenuItem"
        Me.FindSearchToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.FindSearchToolStripMenuItem.Size = New System.Drawing.Size(215, 22)
        Me.FindSearchToolStripMenuItem.Text = "Find Search"
        Me.FindSearchToolStripMenuItem.Visible = False
        '
        'SumOfSelectedRowsToolStripMenuItem
        '
        Me.SumOfSelectedRowsToolStripMenuItem.Name = "SumOfSelectedRowsToolStripMenuItem"
        Me.SumOfSelectedRowsToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.SumOfSelectedRowsToolStripMenuItem.Size = New System.Drawing.Size(215, 22)
        Me.SumOfSelectedRowsToolStripMenuItem.Text = "SumOfSelected Rows"
        Me.SumOfSelectedRowsToolStripMenuItem.Visible = False
        '
        'pnlHeading
        '
        Me.pnlHeading.Controls.Add(Me.lblTitle)
        Me.pnlHeading.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeading.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeading.Name = "pnlHeading"
        Me.pnlHeading.Size = New System.Drawing.Size(1008, 40)
        Me.pnlHeading.TabIndex = 1
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1008, 40)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Label1"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.pnlFooter)
        Me.Panel1.Controls.Add(Me.gridSummary)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(3, 416)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1008, 192)
        Me.Panel1.TabIndex = 3
        '
        'pnlFooter
        '
        Me.pnlFooter.Controls.Add(Me.btnChqPrint)
        Me.pnlFooter.Controls.Add(Me.Button3)
        Me.pnlFooter.Controls.Add(Me.Button2)
        Me.pnlFooter.Controls.Add(Me.Button1)
        Me.pnlFooter.Controls.Add(Me.btnAccountInfo)
        Me.pnlFooter.Controls.Add(Me.btnDuplicatePrint)
        Me.pnlFooter.Controls.Add(Me.btnCancel)
        Me.pnlFooter.Controls.Add(Me.btnContraSummary)
        Me.pnlFooter.Controls.Add(Me.btnEdit)
        Me.pnlFooter.Controls.Add(Me.btnPrint)
        Me.pnlFooter.Controls.Add(Me.btnExport)
        Me.pnlFooter.Controls.Add(Me.btnSumOfSelectedRows)
        Me.pnlFooter.Controls.Add(Me.btnFindSearch)
        Me.pnlFooter.Controls.Add(Me.btnBack)
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Location = New System.Drawing.Point(0, 132)
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Size = New System.Drawing.Size(1008, 60)
        Me.pnlFooter.TabIndex = 1
        '
        'btnChqPrint
        '
        Me.btnChqPrint.Location = New System.Drawing.Point(791, 30)
        Me.btnChqPrint.Name = "btnChqPrint"
        Me.btnChqPrint.Size = New System.Drawing.Size(148, 28)
        Me.btnChqPrint.TabIndex = 21
        Me.btnChqPrint.Text = "Cheque Print [Ctrl +Q]"
        Me.btnChqPrint.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(685, 0)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(100, 30)
        Me.Button3.TabIndex = 20
        Me.Button3.Text = "Load"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(284, 31)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(157, 28)
        Me.Button2.TabIndex = 19
        Me.Button2.Text = "Ledger Print [L]"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(685, 29)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(100, 30)
        Me.Button1.TabIndex = 18
        Me.Button1.Text = "Exit [F12]"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'btnAccountInfo
        '
        Me.btnAccountInfo.Location = New System.Drawing.Point(5, 30)
        Me.btnAccountInfo.Name = "btnAccountInfo"
        Me.btnAccountInfo.Size = New System.Drawing.Size(145, 28)
        Me.btnAccountInfo.TabIndex = 6
        Me.btnAccountInfo.Text = "&Account Info"
        Me.btnAccountInfo.UseVisualStyleBackColor = True
        '
        'btnDuplicatePrint
        '
        Me.btnDuplicatePrint.Location = New System.Drawing.Point(791, 2)
        Me.btnDuplicatePrint.Name = "btnDuplicatePrint"
        Me.btnDuplicatePrint.Size = New System.Drawing.Size(148, 28)
        Me.btnDuplicatePrint.TabIndex = 6
        Me.btnDuplicatePrint.Text = "Dup Print [Ctrl + D]"
        Me.btnDuplicatePrint.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(573, 30)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(106, 28)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnContraSummary
        '
        Me.btnContraSummary.Location = New System.Drawing.Point(156, 31)
        Me.btnContraSummary.Name = "btnContraSummary"
        Me.btnContraSummary.Size = New System.Drawing.Size(122, 28)
        Me.btnContraSummary.TabIndex = 1
        Me.btnContraSummary.Text = "Contra Su&mmary"
        Me.btnContraSummary.UseVisualStyleBackColor = True
        '
        'btnEdit
        '
        Me.btnEdit.Location = New System.Drawing.Point(440, 30)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(127, 28)
        Me.btnEdit.TabIndex = 3
        Me.btnEdit.Text = "Edit [E]"
        Me.btnEdit.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(573, 2)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(106, 28)
        Me.btnPrint.TabIndex = 2
        Me.btnPrint.Text = "Print [P]"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(440, 2)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(127, 28)
        Me.btnExport.TabIndex = 2
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnSumOfSelectedRows
        '
        Me.btnSumOfSelectedRows.ContextMenuStrip = Me.ContextMenuStrip2
        Me.btnSumOfSelectedRows.Location = New System.Drawing.Point(284, 2)
        Me.btnSumOfSelectedRows.Name = "btnSumOfSelectedRows"
        Me.btnSumOfSelectedRows.Size = New System.Drawing.Size(157, 28)
        Me.btnSumOfSelectedRows.TabIndex = 1
        Me.btnSumOfSelectedRows.Text = "Sum of Rows [Ctrl + C]"
        Me.btnSumOfSelectedRows.UseVisualStyleBackColor = True
        '
        'btnFindSearch
        '
        Me.btnFindSearch.Location = New System.Drawing.Point(156, 3)
        Me.btnFindSearch.Name = "btnFindSearch"
        Me.btnFindSearch.Size = New System.Drawing.Size(122, 28)
        Me.btnFindSearch.TabIndex = 1
        Me.btnFindSearch.Text = "Find  [Ctrl + F]"
        Me.btnFindSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnFindSearch.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(5, 2)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(145, 28)
        Me.btnBack.TabIndex = 1
        Me.btnBack.Text = "Back [ESC]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'gridSummary
        '
        Me.gridSummary.AllowUserToAddRows = False
        Me.gridSummary.AllowUserToDeleteRows = False
        Me.gridSummary.BackgroundColor = System.Drawing.SystemColors.ControlDark
        Me.gridSummary.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridSummary.ColumnHeadersVisible = False
        Me.gridSummary.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridSummary.Enabled = False
        Me.gridSummary.Location = New System.Drawing.Point(0, 0)
        Me.gridSummary.Name = "gridSummary"
        Me.gridSummary.ReadOnly = True
        Me.gridSummary.RowHeadersVisible = False
        Me.gridSummary.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridSummary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridSummary.Size = New System.Drawing.Size(1008, 192)
        Me.gridSummary.TabIndex = 0
        '
        'frmAccountsLedger_Weight
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1022, 640)
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmAccountsLedger_Weight"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Accounts Ledger with Weight"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.grpFields.ResumeLayout(False)
        Me.grpFields.PerformLayout()
        Me.tabView.ResumeLayout(False)
        Me.pnlGrid.ResumeLayout(False)
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpReportOptions.ResumeLayout(False)
        Me.grpReportOptions.PerformLayout()
        Me.grpSumOfSelectedRows.ResumeLayout(False)
        Me.grpSumOfSelectedRows.PerformLayout()
        Me.grpBillView.ResumeLayout(False)
        CType(Me.gridBillView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip2.ResumeLayout(False)
        Me.pnlHeading.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.pnlFooter.ResumeLayout(False)
        CType(Me.gridSummary, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    '======================================================
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents gridView_OWN As System.Windows.Forms.DataGridView
    Friend WithEvents pnlGrid As System.Windows.Forms.Panel
    Friend WithEvents pnlHeading As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents grpBillView As System.Windows.Forms.GroupBox
    Friend WithEvents gridBillView As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents FindNextToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents grpSumOfSelectedRows As System.Windows.Forms.GroupBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtSumCredit As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtSumDebit As System.Windows.Forms.TextBox
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel
    Friend WithEvents grpReportOptions As System.Windows.Forms.GroupBox
    Friend WithEvents chkWithPcsWeight As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithNarraion As System.Windows.Forms.CheckBox
    Friend WithEvents btnReport As System.Windows.Forms.Button
    Friend WithEvents btnSumOfSelectedRows As System.Windows.Forms.Button
    Friend WithEvents btnFindSearch As System.Windows.Forms.Button
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents FindSearchToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SumOfSelectedRowsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnEdit As System.Windows.Forms.Button
    Friend WithEvents btnContraSummary As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents gridSummary As System.Windows.Forms.DataGridView
    Friend WithEvents btnDuplicatePrint As System.Windows.Forms.Button
    Friend WithEvents btnAccountInfo As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents btnChqPrint As System.Windows.Forms.Button
    Friend WithEvents grpFields As System.Windows.Forms.GroupBox
    Friend WithEvents chkCmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents chkGrpTranNo As System.Windows.Forms.CheckBox
    Friend WithEvents chkOrderbyTranNo As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithRunningBalance As System.Windows.Forms.CheckBox
    Friend WithEvents chkMultiSelect As System.Windows.Forms.CheckBox
    Friend WithEvents cmbAcName As System.Windows.Forms.ComboBox
    Friend WithEvents cmbAcGroup As System.Windows.Forms.ComboBox
    Friend WithEvents chkCmbAcName As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbAcGroup As BrighttechPack.CheckedComboBox
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnMore As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents ChkSummaryBalance As System.Windows.Forms.CheckBox
    'Private WithEvents AxMSFlexGrid1 As AxMSFlexGridLib.AxMSFlexGrid
    'Private WithEvents AxMSFlexGrid1 As AxMSFlexGridLib.AxMSFlexGrid
    'Private WithEvents AxMSFlexGrid1 As AxMSFlexGridLib.AxMSFlexGrid
    'Private WithEvents AxMSFlexGrid1 As AxMSFlexGridLib.AxMSFlexGrid
    'Private WithEvents AxMSFlexGrid1 As AxMSFlexGridLib.AxMSFlexGrid
    'Private WithEvents AxMSFlexGrid1 As AxMSFlexGridLib.AxMSFlexGrid
End Class
