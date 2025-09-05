<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCancelReport
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
        Me.lblDateFrom = New System.Windows.Forms.Label()
        Me.lblDateTo = New System.Windows.Forms.Label()
        Me.lblOrderBy = New System.Windows.Forms.Label()
        Me.lblCostCentre = New System.Windows.Forms.Label()
        Me.cmbCostCentre = New System.Windows.Forms.ComboBox()
        Me.grbControls = New System.Windows.Forms.GroupBox()
        Me.cmbBillType = New System.Windows.Forms.ComboBox()
        Me.lblBillType = New System.Windows.Forms.Label()
        Me.ChkBillSummary = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkCompanySelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstCompany = New System.Windows.Forms.CheckedListBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.grpOrderBy = New System.Windows.Forms.GroupBox()
        Me.rbtBillType = New System.Windows.Forms.RadioButton()
        Me.rbtBillDate = New System.Windows.Forms.RadioButton()
        Me.rbtBillNo = New System.Windows.Forms.RadioButton()
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.GrpType = New System.Windows.Forms.GroupBox()
        Me.RdbAccounts = New System.Windows.Forms.RadioButton()
        Me.RbdBilling = New System.Windows.Forms.RadioButton()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlTitle = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.grbControls.SuspendLayout()
        Me.grpOrderBy.SuspendLayout()
        Me.GrpType.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.pnlTitle.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblDateFrom
        '
        Me.lblDateFrom.AutoSize = True
        Me.lblDateFrom.Location = New System.Drawing.Point(10, 21)
        Me.lblDateFrom.Name = "lblDateFrom"
        Me.lblDateFrom.Size = New System.Drawing.Size(67, 13)
        Me.lblDateFrom.TabIndex = 0
        Me.lblDateFrom.Text = "Date From"
        '
        'lblDateTo
        '
        Me.lblDateTo.AutoSize = True
        Me.lblDateTo.Location = New System.Drawing.Point(184, 21)
        Me.lblDateTo.Name = "lblDateTo"
        Me.lblDateTo.Size = New System.Drawing.Size(20, 13)
        Me.lblDateTo.TabIndex = 2
        Me.lblDateTo.Text = "To"
        '
        'lblOrderBy
        '
        Me.lblOrderBy.AutoSize = True
        Me.lblOrderBy.Location = New System.Drawing.Point(304, 71)
        Me.lblOrderBy.Name = "lblOrderBy"
        Me.lblOrderBy.Size = New System.Drawing.Size(59, 13)
        Me.lblOrderBy.TabIndex = 11
        Me.lblOrderBy.Text = "Order By"
        '
        'lblCostCentre
        '
        Me.lblCostCentre.AutoSize = True
        Me.lblCostCentre.Location = New System.Drawing.Point(307, 21)
        Me.lblCostCentre.Name = "lblCostCentre"
        Me.lblCostCentre.Size = New System.Drawing.Size(72, 13)
        Me.lblCostCentre.TabIndex = 4
        Me.lblCostCentre.Text = "CostCentre"
        '
        'cmbCostCentre
        '
        Me.cmbCostCentre.FormattingEnabled = True
        Me.cmbCostCentre.Location = New System.Drawing.Point(385, 17)
        Me.cmbCostCentre.Name = "cmbCostCentre"
        Me.cmbCostCentre.Size = New System.Drawing.Size(213, 21)
        Me.cmbCostCentre.TabIndex = 5
        '
        'grbControls
        '
        Me.grbControls.Controls.Add(Me.cmbBillType)
        Me.grbControls.Controls.Add(Me.lblBillType)
        Me.grbControls.Controls.Add(Me.ChkBillSummary)
        Me.grbControls.Controls.Add(Me.Label1)
        Me.grbControls.Controls.Add(Me.chkCompanySelectAll)
        Me.grbControls.Controls.Add(Me.chkLstCompany)
        Me.grbControls.Controls.Add(Me.dtpTo)
        Me.grbControls.Controls.Add(Me.grpOrderBy)
        Me.grbControls.Controls.Add(Me.dtpFrom)
        Me.grbControls.Controls.Add(Me.btnView_Search)
        Me.grbControls.Controls.Add(Me.btnNew)
        Me.grbControls.Controls.Add(Me.btnExport)
        Me.grbControls.Controls.Add(Me.btnPrint)
        Me.grbControls.Controls.Add(Me.btnExit)
        Me.grbControls.Controls.Add(Me.cmbCostCentre)
        Me.grbControls.Controls.Add(Me.lblCostCentre)
        Me.grbControls.Controls.Add(Me.lblOrderBy)
        Me.grbControls.Controls.Add(Me.lblDateFrom)
        Me.grbControls.Controls.Add(Me.lblDateTo)
        Me.grbControls.Controls.Add(Me.GrpType)
        Me.grbControls.Dock = System.Windows.Forms.DockStyle.Top
        Me.grbControls.Location = New System.Drawing.Point(0, 0)
        Me.grbControls.Name = "grbControls"
        Me.grbControls.Size = New System.Drawing.Size(1019, 148)
        Me.grbControls.TabIndex = 0
        Me.grbControls.TabStop = False
        '
        'cmbBillType
        '
        Me.cmbBillType.FormattingEnabled = True
        Me.cmbBillType.Location = New System.Drawing.Point(775, 17)
        Me.cmbBillType.Name = "cmbBillType"
        Me.cmbBillType.Size = New System.Drawing.Size(163, 21)
        Me.cmbBillType.TabIndex = 8
        Me.cmbBillType.Visible = False
        '
        'lblBillType
        '
        Me.lblBillType.AutoSize = True
        Me.lblBillType.Location = New System.Drawing.Point(718, 21)
        Me.lblBillType.Name = "lblBillType"
        Me.lblBillType.Size = New System.Drawing.Size(51, 13)
        Me.lblBillType.TabIndex = 7
        Me.lblBillType.Text = "BillType"
        Me.lblBillType.Visible = False
        '
        'ChkBillSummary
        '
        Me.ChkBillSummary.AutoSize = True
        Me.ChkBillSummary.Location = New System.Drawing.Point(609, 19)
        Me.ChkBillSummary.Name = "ChkBillSummary"
        Me.ChkBillSummary.Size = New System.Drawing.Size(103, 17)
        Me.ChkBillSummary.TabIndex = 6
        Me.ChkBillSummary.Text = "Bill Summary"
        Me.ChkBillSummary.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(606, 70)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "Type"
        '
        'chkCompanySelectAll
        '
        Me.chkCompanySelectAll.AutoSize = True
        Me.chkCompanySelectAll.Location = New System.Drawing.Point(15, 51)
        Me.chkCompanySelectAll.Name = "chkCompanySelectAll"
        Me.chkCompanySelectAll.Size = New System.Drawing.Size(81, 17)
        Me.chkCompanySelectAll.TabIndex = 9
        Me.chkCompanySelectAll.Text = "Company"
        Me.chkCompanySelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCompany
        '
        Me.chkLstCompany.FormattingEnabled = True
        Me.chkLstCompany.Location = New System.Drawing.Point(12, 71)
        Me.chkLstCompany.Name = "chkLstCompany"
        Me.chkLstCompany.Size = New System.Drawing.Size(289, 68)
        Me.chkLstCompany.TabIndex = 10
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(212, 17)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(89, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'grpOrderBy
        '
        Me.grpOrderBy.Controls.Add(Me.rbtBillType)
        Me.grpOrderBy.Controls.Add(Me.rbtBillDate)
        Me.grpOrderBy.Controls.Add(Me.rbtBillNo)
        Me.grpOrderBy.Location = New System.Drawing.Point(369, 55)
        Me.grpOrderBy.Name = "grpOrderBy"
        Me.grpOrderBy.Size = New System.Drawing.Size(219, 38)
        Me.grpOrderBy.TabIndex = 12
        Me.grpOrderBy.TabStop = False
        '
        'rbtBillType
        '
        Me.rbtBillType.AutoSize = True
        Me.rbtBillType.Location = New System.Drawing.Point(6, 15)
        Me.rbtBillType.Name = "rbtBillType"
        Me.rbtBillType.Size = New System.Drawing.Size(69, 17)
        Me.rbtBillType.TabIndex = 0
        Me.rbtBillType.TabStop = True
        Me.rbtBillType.Text = "BillType"
        Me.rbtBillType.UseVisualStyleBackColor = True
        '
        'rbtBillDate
        '
        Me.rbtBillDate.AutoSize = True
        Me.rbtBillDate.Location = New System.Drawing.Point(145, 15)
        Me.rbtBillDate.Name = "rbtBillDate"
        Me.rbtBillDate.Size = New System.Drawing.Size(69, 17)
        Me.rbtBillDate.TabIndex = 2
        Me.rbtBillDate.TabStop = True
        Me.rbtBillDate.Text = "BillDate"
        Me.rbtBillDate.UseVisualStyleBackColor = True
        '
        'rbtBillNo
        '
        Me.rbtBillNo.AutoSize = True
        Me.rbtBillNo.Location = New System.Drawing.Point(82, 15)
        Me.rbtBillNo.Name = "rbtBillNo"
        Me.rbtBillNo.Size = New System.Drawing.Size(57, 17)
        Me.rbtBillNo.TabIndex = 1
        Me.rbtBillNo.TabStop = True
        Me.rbtBillNo.Text = "BillNo"
        Me.rbtBillNo.UseVisualStyleBackColor = True
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(88, 17)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(89, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(307, 109)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 15
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(415, 109)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 16
        Me.btnNew.Text = "&New[F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(523, 109)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 17
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(631, 109)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 18
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(739, 109)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 19
        Me.btnExit.Text = "&Exit[F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'GrpType
        '
        Me.GrpType.Controls.Add(Me.RdbAccounts)
        Me.GrpType.Controls.Add(Me.RbdBilling)
        Me.GrpType.Location = New System.Drawing.Point(671, 55)
        Me.GrpType.Name = "GrpType"
        Me.GrpType.Size = New System.Drawing.Size(168, 37)
        Me.GrpType.TabIndex = 14
        Me.GrpType.TabStop = False
        '
        'RdbAccounts
        '
        Me.RdbAccounts.AutoSize = True
        Me.RdbAccounts.Location = New System.Drawing.Point(71, 14)
        Me.RdbAccounts.Name = "RdbAccounts"
        Me.RdbAccounts.Size = New System.Drawing.Size(76, 17)
        Me.RdbAccounts.TabIndex = 1
        Me.RdbAccounts.TabStop = True
        Me.RdbAccounts.Text = "Accounts"
        Me.RdbAccounts.UseVisualStyleBackColor = True
        '
        'RbdBilling
        '
        Me.RbdBilling.AutoSize = True
        Me.RbdBilling.Checked = True
        Me.RbdBilling.Location = New System.Drawing.Point(6, 14)
        Me.RbdBilling.Name = "RbdBilling"
        Me.RbdBilling.Size = New System.Drawing.Size(59, 17)
        Me.RbdBilling.TabIndex = 0
        Me.RbdBilling.TabStop = True
        Me.RbdBilling.Text = "Billing"
        Me.RbdBilling.UseVisualStyleBackColor = True
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.AllowUserToResizeRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 23)
        Me.gridView.MultiSelect = False
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(1019, 461)
        Me.gridView.StandardTab = True
        Me.gridView.TabIndex = 1
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
        'pnlTitle
        '
        Me.pnlTitle.Controls.Add(Me.gridView)
        Me.pnlTitle.Controls.Add(Me.lblTitle)
        Me.pnlTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlTitle.Location = New System.Drawing.Point(0, 148)
        Me.pnlTitle.Name = "pnlTitle"
        Me.pnlTitle.Size = New System.Drawing.Size(1019, 484)
        Me.pnlTitle.TabIndex = 2
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1019, 23)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "TITLE"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmCancelReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1019, 632)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlTitle)
        Me.Controls.Add(Me.grbControls)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "frmCancelReport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmCancelReport"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grbControls.ResumeLayout(False)
        Me.grbControls.PerformLayout()
        Me.grpOrderBy.ResumeLayout(False)
        Me.grpOrderBy.PerformLayout()
        Me.GrpType.ResumeLayout(False)
        Me.GrpType.PerformLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.pnlTitle.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblDateFrom As System.Windows.Forms.Label
    Friend WithEvents lblDateTo As System.Windows.Forms.Label
    Friend WithEvents lblOrderBy As System.Windows.Forms.Label
    Friend WithEvents lblCostCentre As System.Windows.Forms.Label
    Friend WithEvents cmbCostCentre As System.Windows.Forms.ComboBox
    Friend WithEvents grbControls As System.Windows.Forms.GroupBox
    Friend WithEvents grpOrderBy As System.Windows.Forms.GroupBox
    Friend WithEvents rbtBillDate As System.Windows.Forms.RadioButton
    Friend WithEvents rbtBillNo As System.Windows.Forms.RadioButton
    Friend WithEvents rbtBillType As System.Windows.Forms.RadioButton
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlTitle As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkCompanySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCompany As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GrpType As System.Windows.Forms.GroupBox
    Friend WithEvents RdbAccounts As System.Windows.Forms.RadioButton
    Friend WithEvents RbdBilling As System.Windows.Forms.RadioButton
    Friend WithEvents ChkBillSummary As CheckBox
    Friend WithEvents cmbBillType As ComboBox
    Friend WithEvents lblBillType As Label
End Class
