<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOtherbranchadvanceadjustment
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
        Me.grbControls = New System.Windows.Forms.GroupBox
        Me.cmbcompany = New System.Windows.Forms.ComboBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.chkScheme = New System.Windows.Forms.CheckBox
        Me.chkadvance = New System.Windows.Forms.CheckBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.lblCostCentre = New System.Windows.Forms.Label
        Me.cmbCostCentre = New System.Windows.Forms.ComboBox
        Me.lblDateTo = New System.Windows.Forms.Label
        Me.lblDateFrom = New System.Windows.Forms.Label
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.pnlGrid = New System.Windows.Forms.Panel
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmbType = New System.Windows.Forms.ComboBox
        Me.lblRunto = New System.Windows.Forms.Label
        Me.txtOldRunNo = New System.Windows.Forms.TextBox
        Me.txtNewRunNo = New System.Windows.Forms.TextBox
        Me.lblRunNo = New System.Windows.Forms.Label
        Me.grbRunNoFocus = New System.Windows.Forms.GroupBox
        Me.gridRunNoFocus = New System.Windows.Forms.DataGridView
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.lblTitle = New System.Windows.Forms.Label
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabGen = New System.Windows.Forms.TabPage
        Me.rbtDetailWise = New System.Windows.Forms.RadioButton
        Me.tabView = New System.Windows.Forms.TabPage
        Me.pnlView = New System.Windows.Forms.Panel
        Me.pnlFooter = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnBack = New System.Windows.Forms.Button
        Me.pnlTitle = New System.Windows.Forms.Panel
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.grbControls.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.pnlGrid.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
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
        Me.grbControls.Controls.Add(Me.cmbcompany)
        Me.grbControls.Controls.Add(Me.GroupBox2)
        Me.grbControls.Controls.Add(Me.Label4)
        Me.grbControls.Controls.Add(Me.Label3)
        Me.grbControls.Controls.Add(Me.dtpTo)
        Me.grbControls.Controls.Add(Me.dtpFrom)
        Me.grbControls.Controls.Add(Me.btnExit)
        Me.grbControls.Controls.Add(Me.btnNew)
        Me.grbControls.Controls.Add(Me.btnView_Search)
        Me.grbControls.Controls.Add(Me.lblCostCentre)
        Me.grbControls.Controls.Add(Me.cmbCostCentre)
        Me.grbControls.Controls.Add(Me.lblDateTo)
        Me.grbControls.Controls.Add(Me.lblDateFrom)
        Me.grbControls.Location = New System.Drawing.Point(320, 129)
        Me.grbControls.Name = "grbControls"
        Me.grbControls.Size = New System.Drawing.Size(339, 251)
        Me.grbControls.TabIndex = 0
        Me.grbControls.TabStop = False
        '
        'cmbcompany
        '
        Me.cmbcompany.FormattingEnabled = True
        Me.cmbcompany.Location = New System.Drawing.Point(99, 75)
        Me.cmbcompany.Name = "cmbcompany"
        Me.cmbcompany.Size = New System.Drawing.Size(227, 21)
        Me.cmbcompany.TabIndex = 5
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.chkScheme)
        Me.GroupBox2.Controls.Add(Me.chkadvance)
        Me.GroupBox2.Location = New System.Drawing.Point(99, 161)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(227, 35)
        Me.GroupBox2.TabIndex = 9
        Me.GroupBox2.TabStop = False
        '
        'chkScheme
        '
        Me.chkScheme.AutoSize = True
        Me.chkScheme.Location = New System.Drawing.Point(102, 12)
        Me.chkScheme.Name = "chkScheme"
        Me.chkScheme.Size = New System.Drawing.Size(72, 17)
        Me.chkScheme.TabIndex = 1
        Me.chkScheme.Text = "Scheme"
        Me.chkScheme.UseVisualStyleBackColor = True
        '
        'chkadvance
        '
        Me.chkadvance.AutoSize = True
        Me.chkadvance.Location = New System.Drawing.Point(18, 12)
        Me.chkadvance.Name = "chkadvance"
        Me.chkadvance.Size = New System.Drawing.Size(75, 17)
        Me.chkadvance.TabIndex = 0
        Me.chkadvance.Text = "Advance"
        Me.chkadvance.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(18, 173)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(65, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Tran Type"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(18, 75)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(62, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Company"
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(230, 211)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(90, 30)
        Me.btnExit.TabIndex = 12
        Me.btnExit.Text = "&Exit[F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(125, 211)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(90, 30)
        Me.btnNew.TabIndex = 11
        Me.btnNew.Text = "&New[F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(21, 211)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(90, 30)
        Me.btnView_Search.TabIndex = 10
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'lblCostCentre
        '
        Me.lblCostCentre.AutoSize = True
        Me.lblCostCentre.Location = New System.Drawing.Point(17, 122)
        Me.lblCostCentre.Name = "lblCostCentre"
        Me.lblCostCentre.Size = New System.Drawing.Size(76, 13)
        Me.lblCostCentre.TabIndex = 6
        Me.lblCostCentre.Text = "Cost Centre"
        '
        'cmbCostCentre
        '
        Me.cmbCostCentre.FormattingEnabled = True
        Me.cmbCostCentre.Location = New System.Drawing.Point(99, 118)
        Me.cmbCostCentre.Name = "cmbCostCentre"
        Me.cmbCostCentre.Size = New System.Drawing.Size(227, 21)
        Me.cmbCostCentre.TabIndex = 7
        '
        'lblDateTo
        '
        Me.lblDateTo.AutoSize = True
        Me.lblDateTo.Location = New System.Drawing.Point(198, 24)
        Me.lblDateTo.Name = "lblDateTo"
        Me.lblDateTo.Size = New System.Drawing.Size(21, 13)
        Me.lblDateTo.TabIndex = 2
        Me.lblDateTo.Text = "To"
        '
        'lblDateFrom
        '
        Me.lblDateFrom.AutoSize = True
        Me.lblDateFrom.Location = New System.Drawing.Point(13, 24)
        Me.lblDateFrom.Name = "lblDateFrom"
        Me.lblDateFrom.Size = New System.Drawing.Size(67, 13)
        Me.lblDateFrom.TabIndex = 0
        Me.lblDateFrom.Text = "Date From"
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(661, 7)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 14
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(552, 7)
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
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.NewToolStripMenuItem.Text = "&New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "&Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'pnlGrid
        '
        Me.pnlGrid.Controls.Add(Me.GroupBox1)
        Me.pnlGrid.Controls.Add(Me.grbRunNoFocus)
        Me.pnlGrid.Controls.Add(Me.gridView)
        Me.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlGrid.Location = New System.Drawing.Point(0, 20)
        Me.pnlGrid.Name = "pnlGrid"
        Me.pnlGrid.Size = New System.Drawing.Size(1005, 540)
        Me.pnlGrid.TabIndex = 25
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.cmbType)
        Me.GroupBox1.Controls.Add(Me.lblRunto)
        Me.GroupBox1.Controls.Add(Me.txtOldRunNo)
        Me.GroupBox1.Controls.Add(Me.txtNewRunNo)
        Me.GroupBox1.Controls.Add(Me.lblRunNo)
        Me.GroupBox1.Location = New System.Drawing.Point(68, 434)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(650, 56)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "To Change the Ref No and Ref Type"
        Me.GroupBox1.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(457, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(35, 13)
        Me.Label1.TabIndex = 30
        Me.Label1.Text = "Type"
        Me.Label1.Visible = False
        '
        'cmbType
        '
        Me.cmbType.FormattingEnabled = True
        Me.cmbType.Items.AddRange(New Object() {"Credit", "Advance"})
        Me.cmbType.Location = New System.Drawing.Point(516, 23)
        Me.cmbType.Name = "cmbType"
        Me.cmbType.Size = New System.Drawing.Size(106, 21)
        Me.cmbType.TabIndex = 29
        Me.cmbType.Visible = False
        '
        'lblRunto
        '
        Me.lblRunto.AutoSize = True
        Me.lblRunto.Location = New System.Drawing.Point(222, 26)
        Me.lblRunto.Name = "lblRunto"
        Me.lblRunto.Size = New System.Drawing.Size(81, 13)
        Me.lblRunto.TabIndex = 28
        Me.lblRunto.Text = "New Ref. No."
        '
        'txtOldRunNo
        '
        Me.txtOldRunNo.Enabled = False
        Me.txtOldRunNo.Location = New System.Drawing.Point(95, 21)
        Me.txtOldRunNo.Name = "txtOldRunNo"
        Me.txtOldRunNo.Size = New System.Drawing.Size(112, 21)
        Me.txtOldRunNo.TabIndex = 27
        '
        'txtNewRunNo
        '
        Me.txtNewRunNo.Location = New System.Drawing.Point(319, 23)
        Me.txtNewRunNo.Name = "txtNewRunNo"
        Me.txtNewRunNo.Size = New System.Drawing.Size(122, 21)
        Me.txtNewRunNo.TabIndex = 26
        '
        'lblRunNo
        '
        Me.lblRunNo.AutoSize = True
        Me.lblRunNo.Location = New System.Drawing.Point(6, 26)
        Me.lblRunNo.Name = "lblRunNo"
        Me.lblRunNo.Size = New System.Drawing.Size(76, 13)
        Me.lblRunNo.TabIndex = 25
        Me.lblRunNo.Text = "Old Ref. No."
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
        Me.gridView.AllowUserToResizeRows = False
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
        Me.cmbGridShortCut.Size = New System.Drawing.Size(143, 26)
        '
        'ResizeToolStripMenuItem
        '
        Me.ResizeToolStripMenuItem.CheckOnClick = True
        Me.ResizeToolStripMenuItem.Name = "ResizeToolStripMenuItem"
        Me.ResizeToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
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
        Me.tabGen.Controls.Add(Me.rbtDetailWise)
        Me.tabGen.Controls.Add(Me.grbControls)
        Me.tabGen.Location = New System.Drawing.Point(4, 22)
        Me.tabGen.Name = "tabGen"
        Me.tabGen.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGen.Size = New System.Drawing.Size(1011, 606)
        Me.tabGen.TabIndex = 0
        Me.tabGen.Text = "Gen"
        Me.tabGen.UseVisualStyleBackColor = True
        '
        'rbtDetailWise
        '
        Me.rbtDetailWise.AutoSize = True
        Me.rbtDetailWise.Location = New System.Drawing.Point(8, 6)
        Me.rbtDetailWise.Name = "rbtDetailWise"
        Me.rbtDetailWise.Size = New System.Drawing.Size(85, 17)
        Me.rbtDetailWise.TabIndex = 1
        Me.rbtDetailWise.TabStop = True
        Me.rbtDetailWise.Text = "DetailWise"
        Me.rbtDetailWise.UseVisualStyleBackColor = True
        Me.rbtDetailWise.Visible = False
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
        Me.pnlFooter.Controls.Add(Me.Label2)
        Me.pnlFooter.Controls.Add(Me.btnBack)
        Me.pnlFooter.Controls.Add(Me.btnExport)
        Me.pnlFooter.Controls.Add(Me.btnPrint)
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Location = New System.Drawing.Point(0, 560)
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Size = New System.Drawing.Size(1005, 40)
        Me.pnlFooter.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(47, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(94, 13)
        Me.Label2.TabIndex = 26
        Me.Label2.Text = "Press E for Edit"
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
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(233, 20)
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
        Me.dtpFrom.Location = New System.Drawing.Point(99, 20)
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
        'frmOtherbranchadvanceadjustment
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
        Me.Name = "frmOtherbranchadvanceadjustment"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "CustomerOutstanding"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grbControls.ResumeLayout(False)
        Me.grbControls.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.pnlGrid.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
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
        Me.pnlFooter.PerformLayout()
        Me.pnlTitle.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grbControls As System.Windows.Forms.GroupBox
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents lblDateTo As System.Windows.Forms.Label
    Friend WithEvents lblDateFrom As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblCostCentre As System.Windows.Forms.Label
    Friend WithEvents cmbCostCentre As System.Windows.Forms.ComboBox
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
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbType As System.Windows.Forms.ComboBox
    Friend WithEvents lblRunto As System.Windows.Forms.Label
    Friend WithEvents txtOldRunNo As System.Windows.Forms.TextBox
    Friend WithEvents txtNewRunNo As System.Windows.Forms.TextBox
    Friend WithEvents lblRunNo As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents chkScheme As System.Windows.Forms.CheckBox
    Friend WithEvents chkadvance As System.Windows.Forms.CheckBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmbcompany As System.Windows.Forms.ComboBox
    Friend WithEvents rbtDetailWise As System.Windows.Forms.RadioButton
End Class
