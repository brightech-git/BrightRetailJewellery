<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmItemSalesPerformance
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
        Me.grpControls = New System.Windows.Forms.GroupBox()
        Me.cmbGroupBy = New System.Windows.Forms.ComboBox()
        Me.cmbCompany = New GiritechPack.CheckedComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ChkOrderonly = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.rbtGRSWT = New System.Windows.Forms.RadioButton()
        Me.rbtNetWT = New System.Windows.Forms.RadioButton()
        Me.Txtwithdate = New System.Windows.Forms.TextBox()
        Me.WithDate = New System.Windows.Forms.CheckBox()
        Me.chkCmbCostCentre = New GiritechPack.CheckedComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.chkcmbItem = New GiritechPack.CheckedComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.rbtCounter = New System.Windows.Forms.RadioButton()
        Me.rbtNone = New System.Windows.Forms.RadioButton()
        Me.rbtMetal = New System.Windows.Forms.RadioButton()
        Me.dtpTo = New GiritechPack.DatePicker(Me.components)
        Me.dtpFrom = New GiritechPack.DatePicker(Me.components)
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.cmbMetal = New System.Windows.Forms.ComboBox()
        Me.lABEL60 = New System.Windows.Forms.Label()
        Me.lblDateTo = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblDateFrom = New System.Windows.Forms.Label()
        Me.pnlGrid = New System.Windows.Forms.Panel()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.gridViewHead = New System.Windows.Forms.DataGridView()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.CmbCashCounter = New System.Windows.Forms.ComboBox()
        Me.grpControls.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.pnlGrid.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpControls
        '
        Me.grpControls.Controls.Add(Me.CmbCashCounter)
        Me.grpControls.Controls.Add(Me.Label6)
        Me.grpControls.Controls.Add(Me.cmbGroupBy)
        Me.grpControls.Controls.Add(Me.cmbCompany)
        Me.grpControls.Controls.Add(Me.Label5)
        Me.grpControls.Controls.Add(Me.ChkOrderonly)
        Me.grpControls.Controls.Add(Me.GroupBox1)
        Me.grpControls.Controls.Add(Me.Txtwithdate)
        Me.grpControls.Controls.Add(Me.WithDate)
        Me.grpControls.Controls.Add(Me.chkCmbCostCentre)
        Me.grpControls.Controls.Add(Me.Label3)
        Me.grpControls.Controls.Add(Me.chkcmbItem)
        Me.grpControls.Controls.Add(Me.Label2)
        Me.grpControls.Controls.Add(Me.rbtCounter)
        Me.grpControls.Controls.Add(Me.rbtNone)
        Me.grpControls.Controls.Add(Me.rbtMetal)
        Me.grpControls.Controls.Add(Me.dtpTo)
        Me.grpControls.Controls.Add(Me.dtpFrom)
        Me.grpControls.Controls.Add(Me.btnExit)
        Me.grpControls.Controls.Add(Me.btnPrint)
        Me.grpControls.Controls.Add(Me.btnExport)
        Me.grpControls.Controls.Add(Me.btnNew)
        Me.grpControls.Controls.Add(Me.btnView_Search)
        Me.grpControls.Controls.Add(Me.cmbMetal)
        Me.grpControls.Controls.Add(Me.lABEL60)
        Me.grpControls.Controls.Add(Me.lblDateTo)
        Me.grpControls.Controls.Add(Me.Label1)
        Me.grpControls.Controls.Add(Me.lblDateFrom)
        Me.grpControls.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpControls.Location = New System.Drawing.Point(0, 0)
        Me.grpControls.Name = "grpControls"
        Me.grpControls.Size = New System.Drawing.Size(1019, 125)
        Me.grpControls.TabIndex = 1
        Me.grpControls.TabStop = False
        '
        'cmbGroupBy
        '
        Me.cmbGroupBy.FormattingEnabled = True
        Me.cmbGroupBy.Location = New System.Drawing.Point(704, 67)
        Me.cmbGroupBy.Name = "cmbGroupBy"
        Me.cmbGroupBy.Size = New System.Drawing.Size(209, 21)
        Me.cmbGroupBy.TabIndex = 17
        '
        'cmbCompany
        '
        Me.cmbCompany.CheckOnClick = True
        Me.cmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbCompany.DropDownHeight = 1
        Me.cmbCompany.FormattingEnabled = True
        Me.cmbCompany.IntegralHeight = False
        Me.cmbCompany.Location = New System.Drawing.Point(90, 39)
        Me.cmbCompany.Name = "cmbCompany"
        Me.cmbCompany.Size = New System.Drawing.Size(213, 22)
        Me.cmbCompany.TabIndex = 11
        Me.cmbCompany.ValueSeparator = ", "
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 43)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(62, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Company"
        '
        'ChkOrderonly
        '
        Me.ChkOrderonly.AutoSize = True
        Me.ChkOrderonly.Location = New System.Drawing.Point(920, 71)
        Me.ChkOrderonly.Name = "ChkOrderonly"
        Me.ChkOrderonly.Size = New System.Drawing.Size(89, 17)
        Me.ChkOrderonly.TabIndex = 18
        Me.ChkOrderonly.Text = "Order Only"
        Me.ChkOrderonly.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.rbtGRSWT)
        Me.GroupBox1.Controls.Add(Me.rbtNetWT)
        Me.GroupBox1.Location = New System.Drawing.Point(789, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(201, 38)
        Me.GroupBox1.TabIndex = 9
        Me.GroupBox1.TabStop = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 17)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(46, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Weight"
        '
        'rbtGRSWT
        '
        Me.rbtGRSWT.AutoSize = True
        Me.rbtGRSWT.Location = New System.Drawing.Point(58, 15)
        Me.rbtGRSWT.Name = "rbtGRSWT"
        Me.rbtGRSWT.Size = New System.Drawing.Size(67, 17)
        Me.rbtGRSWT.TabIndex = 1
        Me.rbtGRSWT.TabStop = True
        Me.rbtGRSWT.Text = "Grs WT"
        Me.rbtGRSWT.UseVisualStyleBackColor = True
        '
        'rbtNetWT
        '
        Me.rbtNetWT.AutoSize = True
        Me.rbtNetWT.Location = New System.Drawing.Point(131, 15)
        Me.rbtNetWT.Name = "rbtNetWT"
        Me.rbtNetWT.Size = New System.Drawing.Size(66, 17)
        Me.rbtNetWT.TabIndex = 2
        Me.rbtNetWT.TabStop = True
        Me.rbtNetWT.Text = "Net WT"
        Me.rbtNetWT.UseVisualStyleBackColor = True
        '
        'Txtwithdate
        '
        Me.Txtwithdate.Location = New System.Drawing.Point(752, 14)
        Me.Txtwithdate.Name = "Txtwithdate"
        Me.Txtwithdate.Size = New System.Drawing.Size(31, 21)
        Me.Txtwithdate.TabIndex = 8
        Me.Txtwithdate.Visible = False
        '
        'WithDate
        '
        Me.WithDate.AutoSize = True
        Me.WithDate.Location = New System.Drawing.Point(617, 16)
        Me.WithDate.Name = "WithDate"
        Me.WithDate.Size = New System.Drawing.Size(133, 17)
        Me.WithDate.TabIndex = 7
        Me.WithDate.Text = "With Date Opening"
        Me.WithDate.UseVisualStyleBackColor = True
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(704, 39)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(213, 22)
        Me.chkCmbCostCentre.TabIndex = 15
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(619, 38)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(79, 21)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkcmbItem
        '
        Me.chkcmbItem.CheckOnClick = True
        Me.chkcmbItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbItem.DropDownHeight = 1
        Me.chkcmbItem.FormattingEnabled = True
        Me.chkcmbItem.IntegralHeight = False
        Me.chkcmbItem.Location = New System.Drawing.Point(394, 37)
        Me.chkcmbItem.Name = "chkcmbItem"
        Me.chkcmbItem.Size = New System.Drawing.Size(213, 22)
        Me.chkcmbItem.TabIndex = 13
        Me.chkcmbItem.ValueSeparator = ", "
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(330, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Item"
        '
        'rbtCounter
        '
        Me.rbtCounter.AutoSize = True
        Me.rbtCounter.Location = New System.Drawing.Point(952, 80)
        Me.rbtCounter.Name = "rbtCounter"
        Me.rbtCounter.Size = New System.Drawing.Size(71, 17)
        Me.rbtCounter.TabIndex = 21
        Me.rbtCounter.TabStop = True
        Me.rbtCounter.Text = "Counter"
        Me.rbtCounter.UseVisualStyleBackColor = True
        Me.rbtCounter.Visible = False
        '
        'rbtNone
        '
        Me.rbtNone.AutoSize = True
        Me.rbtNone.Location = New System.Drawing.Point(953, 41)
        Me.rbtNone.Name = "rbtNone"
        Me.rbtNone.Size = New System.Drawing.Size(54, 17)
        Me.rbtNone.TabIndex = 19
        Me.rbtNone.TabStop = True
        Me.rbtNone.Text = "None"
        Me.rbtNone.UseVisualStyleBackColor = True
        Me.rbtNone.Visible = False
        '
        'rbtMetal
        '
        Me.rbtMetal.AutoSize = True
        Me.rbtMetal.Location = New System.Drawing.Point(952, 63)
        Me.rbtMetal.Name = "rbtMetal"
        Me.rbtMetal.Size = New System.Drawing.Size(55, 17)
        Me.rbtMetal.TabIndex = 20
        Me.rbtMetal.TabStop = True
        Me.rbtMetal.Text = "Metal"
        Me.rbtMetal.UseVisualStyleBackColor = True
        Me.rbtMetal.Visible = False
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(210, 13)
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
        Me.dtpFrom.Location = New System.Drawing.Point(90, 13)
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
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(511, 89)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 0
        Me.btnExit.Text = "&Exit[F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(403, 89)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 27
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(295, 89)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 26
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(187, 89)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 25
        Me.btnNew.Text = "&New[F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(79, 89)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 24
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'cmbMetal
        '
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(394, 13)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(213, 21)
        Me.cmbMetal.TabIndex = 5
        '
        'lABEL60
        '
        Me.lABEL60.AutoSize = True
        Me.lABEL60.Location = New System.Drawing.Point(329, 17)
        Me.lABEL60.Name = "lABEL60"
        Me.lABEL60.Size = New System.Drawing.Size(37, 13)
        Me.lABEL60.TabIndex = 4
        Me.lABEL60.Text = "Metal"
        '
        'lblDateTo
        '
        Me.lblDateTo.AutoSize = True
        Me.lblDateTo.Location = New System.Drawing.Point(186, 16)
        Me.lblDateTo.Name = "lblDateTo"
        Me.lblDateTo.Size = New System.Drawing.Size(21, 13)
        Me.lblDateTo.TabIndex = 2
        Me.lblDateTo.Text = "To"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(619, 71)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 13)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Group By"
        '
        'lblDateFrom
        '
        Me.lblDateFrom.AutoSize = True
        Me.lblDateFrom.Location = New System.Drawing.Point(6, 16)
        Me.lblDateFrom.Name = "lblDateFrom"
        Me.lblDateFrom.Size = New System.Drawing.Size(67, 13)
        Me.lblDateFrom.TabIndex = 0
        Me.lblDateFrom.Text = "Date From"
        '
        'pnlGrid
        '
        Me.pnlGrid.Controls.Add(Me.gridView)
        Me.pnlGrid.Controls.Add(Me.gridViewHead)
        Me.pnlGrid.Controls.Add(Me.lblTitle)
        Me.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlGrid.Location = New System.Drawing.Point(0, 125)
        Me.pnlGrid.Name = "pnlGrid"
        Me.pnlGrid.Size = New System.Drawing.Size(1019, 507)
        Me.pnlGrid.TabIndex = 1
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.AllowUserToResizeRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.ContextMenuStrip = Me.ContextMenuStrip1
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 38)
        Me.gridView.MultiSelect = False
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(1019, 469)
        Me.gridView.StandardTab = True
        Me.gridView.TabIndex = 0
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.NewToolStripMenuItem.Text = "&New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "&Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'gridViewHead
        '
        Me.gridViewHead.AllowUserToAddRows = False
        Me.gridViewHead.AllowUserToDeleteRows = False
        Me.gridViewHead.AllowUserToResizeColumns = False
        Me.gridViewHead.AllowUserToResizeRows = False
        Me.gridViewHead.BackgroundColor = System.Drawing.SystemColors.ControlDark
        Me.gridViewHead.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridViewHead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewHead.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridViewHead.Location = New System.Drawing.Point(0, 20)
        Me.gridViewHead.MultiSelect = False
        Me.gridViewHead.Name = "gridViewHead"
        Me.gridViewHead.ReadOnly = True
        Me.gridViewHead.RowHeadersVisible = False
        Me.gridViewHead.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridViewHead.Size = New System.Drawing.Size(1019, 18)
        Me.gridViewHead.StandardTab = True
        Me.gridViewHead.TabIndex = 1
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1019, 20)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "TITLE"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(7, 67)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(86, 13)
        Me.Label6.TabIndex = 22
        Me.Label6.Text = "Cash Counter"
        '
        'CmbCashCounter
        '
        Me.CmbCashCounter.FormattingEnabled = True
        Me.CmbCashCounter.Location = New System.Drawing.Point(90, 64)
        Me.CmbCashCounter.Name = "CmbCashCounter"
        Me.CmbCashCounter.Size = New System.Drawing.Size(213, 21)
        Me.CmbCashCounter.TabIndex = 23
        '
        'frmItemSalesPerformance
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1019, 632)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlGrid)
        Me.Controls.Add(Me.grpControls)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "frmItemSalesPerformance"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Item Sales Performance"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpControls.ResumeLayout(False)
        Me.grpControls.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.pnlGrid.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpControls As System.Windows.Forms.GroupBox
    Friend WithEvents lblDateTo As System.Windows.Forms.Label
    Friend WithEvents lblDateFrom As System.Windows.Forms.Label
    Friend WithEvents pnlGrid As System.Windows.Forms.Panel
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents gridViewHead As System.Windows.Forms.DataGridView
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents dtpTo As GiritechPack.DatePicker
    Friend WithEvents dtpFrom As GiritechPack.DatePicker
    Friend WithEvents rbtCounter As System.Windows.Forms.RadioButton
    Friend WithEvents rbtNone As System.Windows.Forms.RadioButton
    Friend WithEvents rbtMetal As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbMetal As System.Windows.Forms.ComboBox
    Friend WithEvents lABEL60 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkcmbItem As GiritechPack.CheckedComboBox
    Friend WithEvents chkCmbCostCentre As GiritechPack.CheckedComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents WithDate As System.Windows.Forms.CheckBox
    Friend WithEvents Txtwithdate As System.Windows.Forms.TextBox
    Friend WithEvents rbtGRSWT As System.Windows.Forms.RadioButton
    Friend WithEvents rbtNetWT As System.Windows.Forms.RadioButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ChkOrderonly As System.Windows.Forms.CheckBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cmbCompany As GiritechPack.CheckedComboBox
    Friend WithEvents cmbGroupBy As System.Windows.Forms.ComboBox
    Friend WithEvents CmbCashCounter As ComboBox
    Friend WithEvents Label6 As Label
End Class
