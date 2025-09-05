<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCashTransactionReport
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
        Me.lblNodeId = New System.Windows.Forms.Label()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.lblCashTransactioType = New System.Windows.Forms.Label()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.cmbCashTransactionType = New System.Windows.Forms.ComboBox()
        Me.grbControls = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.rbtNormal = New System.Windows.Forms.RadioButton()
        Me.rbtDetail = New System.Windows.Forms.RadioButton()
        Me.rbtSummary = New System.Windows.Forms.RadioButton()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmbMCashTransactionType = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblRemark = New System.Windows.Forms.Label()
        Me.txtRemark = New System.Windows.Forms.TextBox()
        Me.chkCmbAcchead = New BrighttechPack.CheckedComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.chkCompanySelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstCompany = New System.Windows.Forms.CheckedListBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.txtNodeId = New System.Windows.Forms.TextBox()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FindToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.gridView_OWN = New System.Windows.Forms.DataGridView()
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlGrid = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.chkCmbCostcentre = New BrighttechPack.CheckedComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.grbControls.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmbGridShortCut.SuspendLayout()
        Me.pnlGrid.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblDateFrom
        '
        Me.lblDateFrom.AutoSize = True
        Me.lblDateFrom.Location = New System.Drawing.Point(12, 17)
        Me.lblDateFrom.Name = "lblDateFrom"
        Me.lblDateFrom.Size = New System.Drawing.Size(67, 13)
        Me.lblDateFrom.TabIndex = 0
        Me.lblDateFrom.Text = "Date From"
        '
        'lblNodeId
        '
        Me.lblNodeId.AutoSize = True
        Me.lblNodeId.Location = New System.Drawing.Point(733, 16)
        Me.lblNodeId.Name = "lblNodeId"
        Me.lblNodeId.Size = New System.Drawing.Size(54, 13)
        Me.lblNodeId.TabIndex = 16
        Me.lblNodeId.Text = "Node ID"
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(181, 17)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(20, 13)
        Me.lblTo.TabIndex = 2
        Me.lblTo.Text = "To"
        '
        'lblCashTransactioType
        '
        Me.lblCashTransactioType.AutoSize = True
        Me.lblCashTransactioType.Location = New System.Drawing.Point(307, 43)
        Me.lblCashTransactioType.Name = "lblCashTransactioType"
        Me.lblCashTransactioType.Size = New System.Drawing.Size(128, 13)
        Me.lblCashTransactioType.TabIndex = 10
        Me.lblCashTransactioType.Text = "CashTransactionType"
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(304, 117)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 27)
        Me.btnView_Search.TabIndex = 19
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(412, 117)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 27)
        Me.btnNew.TabIndex = 20
        Me.btnNew.Text = "&New[F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(520, 117)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 27)
        Me.btnExport.TabIndex = 21
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(628, 117)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 27)
        Me.btnPrint.TabIndex = 22
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(736, 117)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 27)
        Me.btnExit.TabIndex = 23
        Me.btnExit.Text = "&Exit[F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'cmbCashTransactionType
        '
        Me.cmbCashTransactionType.FormattingEnabled = True
        Me.cmbCashTransactionType.Location = New System.Drawing.Point(441, 38)
        Me.cmbCashTransactionType.Name = "cmbCashTransactionType"
        Me.cmbCashTransactionType.Size = New System.Drawing.Size(288, 21)
        Me.cmbCashTransactionType.TabIndex = 11
        '
        'grbControls
        '
        Me.grbControls.Controls.Add(Me.Label6)
        Me.grbControls.Controls.Add(Me.chkCmbCostcentre)
        Me.grbControls.Controls.Add(Me.Label4)
        Me.grbControls.Controls.Add(Me.Label3)
        Me.grbControls.Controls.Add(Me.GroupBox1)
        Me.grbControls.Controls.Add(Me.Label2)
        Me.grbControls.Controls.Add(Me.cmbMCashTransactionType)
        Me.grbControls.Controls.Add(Me.Label1)
        Me.grbControls.Controls.Add(Me.lblRemark)
        Me.grbControls.Controls.Add(Me.txtRemark)
        Me.grbControls.Controls.Add(Me.chkCmbAcchead)
        Me.grbControls.Controls.Add(Me.Label5)
        Me.grbControls.Controls.Add(Me.chkCompanySelectAll)
        Me.grbControls.Controls.Add(Me.chkLstCompany)
        Me.grbControls.Controls.Add(Me.dtpTo)
        Me.grbControls.Controls.Add(Me.dtpFrom)
        Me.grbControls.Controls.Add(Me.txtNodeId)
        Me.grbControls.Controls.Add(Me.btnView_Search)
        Me.grbControls.Controls.Add(Me.lblNodeId)
        Me.grbControls.Controls.Add(Me.lblTo)
        Me.grbControls.Controls.Add(Me.lblCashTransactioType)
        Me.grbControls.Controls.Add(Me.cmbCashTransactionType)
        Me.grbControls.Controls.Add(Me.btnExport)
        Me.grbControls.Controls.Add(Me.lblDateFrom)
        Me.grbControls.Controls.Add(Me.btnNew)
        Me.grbControls.Controls.Add(Me.btnPrint)
        Me.grbControls.Controls.Add(Me.btnExit)
        Me.grbControls.Dock = System.Windows.Forms.DockStyle.Top
        Me.grbControls.Location = New System.Drawing.Point(0, 0)
        Me.grbControls.Name = "grbControls"
        Me.grbControls.Size = New System.Drawing.Size(1098, 149)
        Me.grbControls.TabIndex = 0
        Me.grbControls.TabStop = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label4.Location = New System.Drawing.Point(850, 124)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(242, 13)
        Me.Label4.TabIndex = 26
        Me.Label4.Text = "* Hit L / C For Ledger / Contra Summary"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label3.Location = New System.Drawing.Point(850, 90)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(157, 13)
        Me.Label3.TabIndex = 24
        Me.Label3.Text = "* Hit [Ctrl + F] For Search"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbtNormal)
        Me.GroupBox1.Controls.Add(Me.rbtDetail)
        Me.GroupBox1.Controls.Add(Me.rbtSummary)
        Me.GroupBox1.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.GroupBox1.Location = New System.Drawing.Point(733, 38)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(274, 41)
        Me.GroupBox1.TabIndex = 18
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Format"
        '
        'rbtNormal
        '
        Me.rbtNormal.AutoSize = True
        Me.rbtNormal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rbtNormal.Location = New System.Drawing.Point(6, 15)
        Me.rbtNormal.Name = "rbtNormal"
        Me.rbtNormal.Size = New System.Drawing.Size(66, 17)
        Me.rbtNormal.TabIndex = 0
        Me.rbtNormal.TabStop = True
        Me.rbtNormal.Text = "Normal"
        Me.rbtNormal.UseVisualStyleBackColor = True
        '
        'rbtDetail
        '
        Me.rbtDetail.AutoSize = True
        Me.rbtDetail.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rbtDetail.Location = New System.Drawing.Point(195, 15)
        Me.rbtDetail.Name = "rbtDetail"
        Me.rbtDetail.Size = New System.Drawing.Size(72, 17)
        Me.rbtDetail.TabIndex = 2
        Me.rbtDetail.TabStop = True
        Me.rbtDetail.Text = "Detailed"
        Me.rbtDetail.UseVisualStyleBackColor = True
        '
        'rbtSummary
        '
        Me.rbtSummary.AutoSize = True
        Me.rbtSummary.ForeColor = System.Drawing.SystemColors.ControlText
        Me.rbtSummary.Location = New System.Drawing.Point(93, 15)
        Me.rbtSummary.Name = "rbtSummary"
        Me.rbtSummary.Size = New System.Drawing.Size(81, 17)
        Me.rbtSummary.TabIndex = 1
        Me.rbtSummary.TabStop = True
        Me.rbtSummary.Text = "Summary"
        Me.rbtSummary.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(307, 17)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(125, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "MainTransactionType"
        '
        'cmbMCashTransactionType
        '
        Me.cmbMCashTransactionType.FormattingEnabled = True
        Me.cmbMCashTransactionType.Location = New System.Drawing.Point(441, 13)
        Me.cmbMCashTransactionType.Name = "cmbMCashTransactionType"
        Me.cmbMCashTransactionType.Size = New System.Drawing.Size(288, 21)
        Me.cmbMCashTransactionType.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label1.Location = New System.Drawing.Point(850, 107)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(225, 13)
        Me.Label1.TabIndex = 25
        Me.Label1.Text = "* Hit M For Main Tran Month Summary"
        '
        'lblRemark
        '
        Me.lblRemark.AutoSize = True
        Me.lblRemark.Location = New System.Drawing.Point(307, 95)
        Me.lblRemark.Name = "lblRemark"
        Me.lblRemark.Size = New System.Drawing.Size(56, 13)
        Me.lblRemark.TabIndex = 14
        Me.lblRemark.Text = "Remark "
        '
        'txtRemark
        '
        Me.txtRemark.Location = New System.Drawing.Point(441, 89)
        Me.txtRemark.Name = "txtRemark"
        Me.txtRemark.Size = New System.Drawing.Size(288, 21)
        Me.txtRemark.TabIndex = 15
        '
        'chkCmbAcchead
        '
        Me.chkCmbAcchead.CheckOnClick = True
        Me.chkCmbAcchead.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbAcchead.DropDownHeight = 1
        Me.chkCmbAcchead.FormattingEnabled = True
        Me.chkCmbAcchead.IntegralHeight = False
        Me.chkCmbAcchead.Location = New System.Drawing.Point(441, 63)
        Me.chkCmbAcchead.Name = "chkCmbAcchead"
        Me.chkCmbAcchead.Size = New System.Drawing.Size(288, 22)
        Me.chkCmbAcchead.TabIndex = 13
        Me.chkCmbAcchead.ValueSeparator = ", "
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(307, 69)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(85, 13)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Account Head"
        '
        'chkCompanySelectAll
        '
        Me.chkCompanySelectAll.AutoSize = True
        Me.chkCompanySelectAll.Location = New System.Drawing.Point(18, 39)
        Me.chkCompanySelectAll.Name = "chkCompanySelectAll"
        Me.chkCompanySelectAll.Size = New System.Drawing.Size(81, 17)
        Me.chkCompanySelectAll.TabIndex = 4
        Me.chkCompanySelectAll.Text = "Company"
        Me.chkCompanySelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCompany
        '
        Me.chkLstCompany.FormattingEnabled = True
        Me.chkLstCompany.Location = New System.Drawing.Point(15, 59)
        Me.chkLstCompany.Name = "chkLstCompany"
        Me.chkLstCompany.Size = New System.Drawing.Size(284, 52)
        Me.chkLstCompany.TabIndex = 5
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(206, 13)
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
        Me.dtpFrom.Location = New System.Drawing.Point(82, 13)
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
        'txtNodeId
        '
        Me.txtNodeId.Location = New System.Drawing.Point(793, 13)
        Me.txtNodeId.Name = "txtNodeId"
        Me.txtNodeId.Size = New System.Drawing.Size(90, 21)
        Me.txtNodeId.TabIndex = 17
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem, Me.FindToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(138, 70)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(137, 22)
        Me.NewToolStripMenuItem.Text = "&New"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(137, 22)
        Me.ExitToolStripMenuItem.Text = "&Exit"
        '
        'FindToolStripMenuItem
        '
        Me.FindToolStripMenuItem.Name = "FindToolStripMenuItem"
        Me.FindToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.FindToolStripMenuItem.Size = New System.Drawing.Size(137, 22)
        Me.FindToolStripMenuItem.Text = "Find"
        '
        'gridView_OWN
        '
        Me.gridView_OWN.AllowUserToAddRows = False
        Me.gridView_OWN.AllowUserToDeleteRows = False
        Me.gridView_OWN.AllowUserToResizeRows = False
        Me.gridView_OWN.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView_OWN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView_OWN.ContextMenuStrip = Me.cmbGridShortCut
        Me.gridView_OWN.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView_OWN.Location = New System.Drawing.Point(0, 19)
        Me.gridView_OWN.MultiSelect = False
        Me.gridView_OWN.Name = "gridView_OWN"
        Me.gridView_OWN.ReadOnly = True
        Me.gridView_OWN.RowHeadersVisible = False
        Me.gridView_OWN.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView_OWN.Size = New System.Drawing.Size(1098, 464)
        Me.gridView_OWN.StandardTab = True
        Me.gridView_OWN.TabIndex = 1
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
        'pnlGrid
        '
        Me.pnlGrid.Controls.Add(Me.gridView_OWN)
        Me.pnlGrid.Controls.Add(Me.lblTitle)
        Me.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlGrid.Location = New System.Drawing.Point(0, 149)
        Me.pnlGrid.Name = "pnlGrid"
        Me.pnlGrid.Size = New System.Drawing.Size(1098, 483)
        Me.pnlGrid.TabIndex = 2
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1098, 19)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "TITLE"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'chkCmbCostcentre
        '
        Me.chkCmbCostcentre.CheckOnClick = True
        Me.chkCmbCostcentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostcentre.DropDownHeight = 1
        Me.chkCmbCostcentre.FormattingEnabled = True
        Me.chkCmbCostcentre.IntegralHeight = False
        Me.chkCmbCostcentre.Location = New System.Drawing.Point(90, 121)
        Me.chkCmbCostcentre.Name = "chkCmbCostcentre"
        Me.chkCmbCostcentre.Size = New System.Drawing.Size(210, 22)
        Me.chkCmbCostcentre.TabIndex = 7
        Me.chkCmbCostcentre.ValueSeparator = ", "
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 124)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(72, 13)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "CostCentre"
        '
        'frmCashTransactionReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1098, 632)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlGrid)
        Me.Controls.Add(Me.grbControls)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "frmCashTransactionReport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "CashTransactionReport"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grbControls.ResumeLayout(False)
        Me.grbControls.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        CType(Me.gridView_OWN, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmbGridShortCut.ResumeLayout(False)
        Me.pnlGrid.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblDateFrom As System.Windows.Forms.Label
    Friend WithEvents lblNodeId As System.Windows.Forms.Label
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents lblCashTransactioType As System.Windows.Forms.Label
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents cmbCashTransactionType As System.Windows.Forms.ComboBox
    Friend WithEvents grbControls As System.Windows.Forms.GroupBox
    Friend WithEvents txtNodeId As System.Windows.Forms.TextBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gridView_OWN As System.Windows.Forms.DataGridView
    Friend WithEvents pnlGrid As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkCompanySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCompany As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkCmbAcchead As BrighttechPack.CheckedComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FindToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblRemark As System.Windows.Forms.Label
    Friend WithEvents txtRemark As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbMCashTransactionType As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbtDetail As System.Windows.Forms.RadioButton
    Friend WithEvents rbtSummary As System.Windows.Forms.RadioButton
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents rbtNormal As System.Windows.Forms.RadioButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label6 As Label
    Friend WithEvents chkCmbCostcentre As BrighttechPack.CheckedComboBox
End Class
