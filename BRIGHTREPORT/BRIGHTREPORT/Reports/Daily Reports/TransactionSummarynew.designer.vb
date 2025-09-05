<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TransactionSummarynew
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
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.cmbCostCentre = New System.Windows.Forms.ComboBox
        Me.grp1 = New System.Windows.Forms.GroupBox
        Me.btnSave_OWN = New System.Windows.Forms.Button
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.PnlFields = New System.Windows.Forms.Panel
        Me.chkWithCashOpen = New System.Windows.Forms.CheckBox
        Me.chkLstCompany = New System.Windows.Forms.CheckedListBox
        Me.chkSystemIdSelectAll = New System.Windows.Forms.CheckBox
        Me.rbtGrossWt = New System.Windows.Forms.RadioButton
        Me.chkLstCashCounter = New System.Windows.Forms.CheckedListBox
        Me.rbtNetWt = New System.Windows.Forms.RadioButton
        Me.chkCashCounterSelectAll = New System.Windows.Forms.CheckBox
        Me.rbtBoth = New System.Windows.Forms.RadioButton
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox
        Me.chkWithPcs = New System.Windows.Forms.CheckBox
        Me.chkCompanySelectAll = New System.Windows.Forms.CheckBox
        Me.chkLstNodeId = New System.Windows.Forms.CheckedListBox
        Me.chkCostCentreSelectAll = New System.Windows.Forms.CheckBox
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.pnlgridview = New System.Windows.Forms.Panel
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.pnlFooter = New System.Windows.Forms.Panel
        Me.btnBack = New System.Windows.Forms.Button
        Me.pnlHeading = New System.Windows.Forms.Panel
        Me.lblTitle = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabGen = New System.Windows.Forms.TabPage
        Me.tabView = New System.Windows.Forms.TabPage
        Me.Panel1.SuspendLayout()
        Me.grp1.SuspendLayout()
        Me.PnlFields.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.pnlgridview.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmbGridShortCut.SuspendLayout()
        Me.pnlFooter.SuspendLayout()
        Me.pnlHeading.SuspendLayout()
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
        Me.cmbCostCentre.TabIndex = 1
        Me.cmbCostCentre.Visible = False
        '
        'grp1
        '
        Me.grp1.Controls.Add(Me.btnSave_OWN)
        Me.grp1.Controls.Add(Me.dtpTo)
        Me.grp1.Controls.Add(Me.dtpFrom)
        Me.grp1.Controls.Add(Me.btnNew)
        Me.grp1.Controls.Add(Me.btnView_Search)
        Me.grp1.Controls.Add(Me.btnExit)
        Me.grp1.Controls.Add(Me.Label2)
        Me.grp1.Controls.Add(Me.Label3)
        Me.grp1.Controls.Add(Me.PnlFields)
        Me.grp1.Location = New System.Drawing.Point(275, 17)
        Me.grp1.Name = "grp1"
        Me.grp1.Size = New System.Drawing.Size(439, 530)
        Me.grp1.TabIndex = 0
        Me.grp1.TabStop = False
        '
        'btnSave_OWN
        '
        Me.btnSave_OWN.Location = New System.Drawing.Point(324, 489)
        Me.btnSave_OWN.Name = "btnSave_OWN"
        Me.btnSave_OWN.Size = New System.Drawing.Size(100, 30)
        Me.btnSave_OWN.TabIndex = 8
        Me.btnSave_OWN.Text = "Save Settings"
        Me.btnSave_OWN.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(285, 20)
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
        Me.dtpFrom.Location = New System.Drawing.Point(140, 20)
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
        Me.btnNew.Location = New System.Drawing.Point(120, 489)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 6
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(18, 489)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 5
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(222, 489)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 7
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(58, 24)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(67, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Date From"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(248, 23)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(21, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "To"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PnlFields
        '
        Me.PnlFields.Controls.Add(Me.chkWithCashOpen)
        Me.PnlFields.Controls.Add(Me.chkLstCompany)
        Me.PnlFields.Controls.Add(Me.chkSystemIdSelectAll)
        Me.PnlFields.Controls.Add(Me.rbtGrossWt)
        Me.PnlFields.Controls.Add(Me.chkLstCashCounter)
        Me.PnlFields.Controls.Add(Me.rbtNetWt)
        Me.PnlFields.Controls.Add(Me.chkCashCounterSelectAll)
        Me.PnlFields.Controls.Add(Me.rbtBoth)
        Me.PnlFields.Controls.Add(Me.chkLstCostCentre)
        Me.PnlFields.Controls.Add(Me.chkWithPcs)
        Me.PnlFields.Controls.Add(Me.chkCompanySelectAll)
        Me.PnlFields.Controls.Add(Me.chkLstNodeId)
        Me.PnlFields.Controls.Add(Me.chkCostCentreSelectAll)
        Me.PnlFields.Location = New System.Drawing.Point(47, 43)
        Me.PnlFields.Name = "PnlFields"
        Me.PnlFields.Size = New System.Drawing.Size(356, 439)
        Me.PnlFields.TabIndex = 4
        '
        'chkWithCashOpen
        '
        Me.chkWithCashOpen.AutoSize = True
        Me.chkWithCashOpen.Location = New System.Drawing.Point(204, 414)
        Me.chkWithCashOpen.Name = "chkWithCashOpen"
        Me.chkWithCashOpen.Size = New System.Drawing.Size(135, 17)
        Me.chkWithCashOpen.TabIndex = 12
        Me.chkWithCashOpen.Text = "With Cash Opening"
        Me.chkWithCashOpen.UseVisualStyleBackColor = True
        Me.chkWithCashOpen.Visible = False
        '
        'chkLstCompany
        '
        Me.chkLstCompany.FormattingEnabled = True
        Me.chkLstCompany.Location = New System.Drawing.Point(10, 28)
        Me.chkLstCompany.Name = "chkLstCompany"
        Me.chkLstCompany.Size = New System.Drawing.Size(321, 84)
        Me.chkLstCompany.TabIndex = 1
        '
        'chkSystemIdSelectAll
        '
        Me.chkSystemIdSelectAll.AutoSize = True
        Me.chkSystemIdSelectAll.Location = New System.Drawing.Point(199, 118)
        Me.chkSystemIdSelectAll.Name = "chkSystemIdSelectAll"
        Me.chkSystemIdSelectAll.Size = New System.Drawing.Size(81, 17)
        Me.chkSystemIdSelectAll.TabIndex = 3
        Me.chkSystemIdSelectAll.Text = "SystemId"
        Me.chkSystemIdSelectAll.UseVisualStyleBackColor = True
        '
        'rbtGrossWt
        '
        Me.rbtGrossWt.AutoSize = True
        Me.rbtGrossWt.Location = New System.Drawing.Point(204, 287)
        Me.rbtGrossWt.Name = "rbtGrossWt"
        Me.rbtGrossWt.Size = New System.Drawing.Size(73, 17)
        Me.rbtGrossWt.TabIndex = 8
        Me.rbtGrossWt.TabStop = True
        Me.rbtGrossWt.Text = "GrossWt"
        Me.rbtGrossWt.UseVisualStyleBackColor = True
        '
        'chkLstCashCounter
        '
        Me.chkLstCashCounter.FormattingEnabled = True
        Me.chkLstCashCounter.Location = New System.Drawing.Point(10, 287)
        Me.chkLstCashCounter.Name = "chkLstCashCounter"
        Me.chkLstCashCounter.Size = New System.Drawing.Size(163, 116)
        Me.chkLstCashCounter.TabIndex = 7
        '
        'rbtNetWt
        '
        Me.rbtNetWt.AutoSize = True
        Me.rbtNetWt.Location = New System.Drawing.Point(204, 320)
        Me.rbtNetWt.Name = "rbtNetWt"
        Me.rbtNetWt.Size = New System.Drawing.Size(59, 17)
        Me.rbtNetWt.TabIndex = 9
        Me.rbtNetWt.TabStop = True
        Me.rbtNetWt.Text = "NetWt"
        Me.rbtNetWt.UseVisualStyleBackColor = True
        '
        'chkCashCounterSelectAll
        '
        Me.chkCashCounterSelectAll.AutoSize = True
        Me.chkCashCounterSelectAll.Location = New System.Drawing.Point(13, 264)
        Me.chkCashCounterSelectAll.Name = "chkCashCounterSelectAll"
        Me.chkCashCounterSelectAll.Size = New System.Drawing.Size(105, 17)
        Me.chkCashCounterSelectAll.TabIndex = 6
        Me.chkCashCounterSelectAll.Text = "Cash Counter"
        Me.chkCashCounterSelectAll.UseVisualStyleBackColor = True
        '
        'rbtBoth
        '
        Me.rbtBoth.AutoSize = True
        Me.rbtBoth.Location = New System.Drawing.Point(204, 353)
        Me.rbtBoth.Name = "rbtBoth"
        Me.rbtBoth.Size = New System.Drawing.Size(51, 17)
        Me.rbtBoth.TabIndex = 10
        Me.rbtBoth.TabStop = True
        Me.rbtBoth.Text = "Both"
        Me.rbtBoth.UseVisualStyleBackColor = True
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(10, 141)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(166, 116)
        Me.chkLstCostCentre.TabIndex = 4
        '
        'chkWithPcs
        '
        Me.chkWithPcs.AutoSize = True
        Me.chkWithPcs.Location = New System.Drawing.Point(204, 386)
        Me.chkWithPcs.Name = "chkWithPcs"
        Me.chkWithPcs.Size = New System.Drawing.Size(91, 17)
        Me.chkWithPcs.TabIndex = 11
        Me.chkWithPcs.Text = "With Pieces"
        Me.chkWithPcs.UseVisualStyleBackColor = True
        Me.chkWithPcs.Visible = False
        '
        'chkCompanySelectAll
        '
        Me.chkCompanySelectAll.AutoSize = True
        Me.chkCompanySelectAll.Location = New System.Drawing.Point(13, 9)
        Me.chkCompanySelectAll.Name = "chkCompanySelectAll"
        Me.chkCompanySelectAll.Size = New System.Drawing.Size(81, 17)
        Me.chkCompanySelectAll.TabIndex = 0
        Me.chkCompanySelectAll.Text = "Company"
        Me.chkCompanySelectAll.UseVisualStyleBackColor = True
        '
        'chkLstNodeId
        '
        Me.chkLstNodeId.FormattingEnabled = True
        Me.chkLstNodeId.Location = New System.Drawing.Point(196, 141)
        Me.chkLstNodeId.Name = "chkLstNodeId"
        Me.chkLstNodeId.Size = New System.Drawing.Size(136, 116)
        Me.chkLstNodeId.TabIndex = 5
        '
        'chkCostCentreSelectAll
        '
        Me.chkCostCentreSelectAll.AutoSize = True
        Me.chkCostCentreSelectAll.Location = New System.Drawing.Point(13, 118)
        Me.chkCostCentreSelectAll.Name = "chkCostCentreSelectAll"
        Me.chkCostCentreSelectAll.Size = New System.Drawing.Size(91, 17)
        Me.chkCostCentreSelectAll.TabIndex = 2
        Me.chkCostCentreSelectAll.Text = "CostCentre"
        Me.chkCostCentreSelectAll.UseVisualStyleBackColor = True
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
        Me.Panel2.Controls.Add(Me.pnlgridview)
        Me.Panel2.Controls.Add(Me.pnlFooter)
        Me.Panel2.Controls.Add(Me.pnlHeading)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 3)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1014, 584)
        Me.Panel2.TabIndex = 0
        '
        'pnlgridview
        '
        Me.pnlgridview.Controls.Add(Me.gridView)
        Me.pnlgridview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlgridview.Location = New System.Drawing.Point(0, 38)
        Me.pnlgridview.Name = "pnlgridview"
        Me.pnlgridview.Size = New System.Drawing.Size(1014, 490)
        Me.pnlgridview.TabIndex = 2
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
        Me.gridView.Size = New System.Drawing.Size(1014, 490)
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
        'pnlFooter
        '
        Me.pnlFooter.Controls.Add(Me.btnBack)
        Me.pnlFooter.Controls.Add(Me.btnExport)
        Me.pnlFooter.Controls.Add(Me.btnPrint)
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Location = New System.Drawing.Point(0, 528)
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Size = New System.Drawing.Size(1014, 56)
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
        'pnlHeading
        '
        Me.pnlHeading.Controls.Add(Me.lblTitle)
        Me.pnlHeading.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeading.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeading.Name = "pnlHeading"
        Me.pnlHeading.Size = New System.Drawing.Size(1014, 38)
        Me.pnlHeading.TabIndex = 1
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1014, 38)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Label1"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
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
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
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
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1020, 590)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'TransactionSummarynew
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 616)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "TransactionSummarynew"
        Me.Text = "Transaction Detailed"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel1.ResumeLayout(False)
        Me.grp1.ResumeLayout(False)
        Me.grp1.PerformLayout()
        Me.PnlFields.ResumeLayout(False)
        Me.PnlFields.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.pnlgridview.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmbGridShortCut.ResumeLayout(False)
        Me.pnlFooter.ResumeLayout(False)
        Me.pnlHeading.ResumeLayout(False)
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
    Friend WithEvents pnlHeading As System.Windows.Forms.Panel
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
    Friend WithEvents pnlgridview As System.Windows.Forms.Panel
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
    Friend WithEvents chkWithCashOpen As System.Windows.Forms.CheckBox
    Friend WithEvents btnSave_OWN As System.Windows.Forms.Button
    Friend WithEvents PnlFields As System.Windows.Forms.Panel
End Class
