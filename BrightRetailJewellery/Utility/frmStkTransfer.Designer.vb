<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStkTransfer
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
        Me.txtTagNo = New System.Windows.Forms.TextBox
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.FileSystemWatcher1 = New System.IO.FileSystemWatcher
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.chkRecDate = New System.Windows.Forms.CheckBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmbCostCentre_MAN = New System.Windows.Forms.ComboBox
        Me.btnTransfer = New System.Windows.Forms.Button
        Me.dtpDate = New BrighttechPack.DatePicker(Me.components)
        Me.txtItemId = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.chkCheckByScan = New System.Windows.Forms.CheckBox
        Me.btnSearch = New System.Windows.Forms.Button
        Me.txtLotNo_NUM = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.grpNonTag = New System.Windows.Forms.GroupBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.txtSubItemName_OWN = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.cmbDesigner_OWN = New System.Windows.Forms.ComboBox
        Me.txtPcs_NUM = New System.Windows.Forms.TextBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.txtGrsWt_WET = New System.Windows.Forms.TextBox
        Me.txtNetWt_WET = New System.Windows.Forms.TextBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.txtOldPcs = New System.Windows.Forms.TextBox
        Me.txtOldGrsWt = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.txtOldNetWt = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.txtSearch = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.cmbSearchKey = New System.Windows.Forms.ComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtEstNo = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.chkCounter = New System.Windows.Forms.CheckBox
        Me.cmbCounter_OWN = New BrighttechPack.CheckedComboBox
        Me.cmbMetal = New BrighttechPack.CheckedComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.cmbTagsCostName_MAN = New System.Windows.Forms.ComboBox
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmbGridShortCut.SuspendLayout()
        CType(Me.FileSystemWatcher1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.grpNonTag.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtTagNo
        '
        Me.txtTagNo.Location = New System.Drawing.Point(275, 95)
        Me.txtTagNo.Name = "txtTagNo"
        Me.txtTagNo.Size = New System.Drawing.Size(94, 21)
        Me.txtTagNo.TabIndex = 16
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.ContextMenuStrip = Me.cmbGridShortCut
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 180)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(934, 340)
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
        'FileSystemWatcher1
        '
        Me.FileSystemWatcher1.EnableRaisingEvents = True
        Me.FileSystemWatcher1.SynchronizingObject = Me
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(218, 99)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 13)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "Tag No"
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(265, 147)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 25
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(583, 147)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 28
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
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
        'chkRecDate
        '
        Me.chkRecDate.AutoSize = True
        Me.chkRecDate.Location = New System.Drawing.Point(17, 70)
        Me.chkRecDate.Name = "chkRecDate"
        Me.chkRecDate.Size = New System.Drawing.Size(78, 17)
        Me.chkRecDate.TabIndex = 7
        Me.chkRecDate.Text = "Rec Date"
        Me.chkRecDate.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(73, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Transfer To"
        '
        'cmbCostCentre_MAN
        '
        Me.cmbCostCentre_MAN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCostCentre_MAN.FormattingEnabled = True
        Me.cmbCostCentre_MAN.Location = New System.Drawing.Point(116, 12)
        Me.cmbCostCentre_MAN.Name = "cmbCostCentre_MAN"
        Me.cmbCostCentre_MAN.Size = New System.Drawing.Size(253, 21)
        Me.cmbCostCentre_MAN.TabIndex = 1
        '
        'btnTransfer
        '
        Me.btnTransfer.Location = New System.Drawing.Point(159, 147)
        Me.btnTransfer.Name = "btnTransfer"
        Me.btnTransfer.Size = New System.Drawing.Size(100, 30)
        Me.btnTransfer.TabIndex = 24
        Me.btnTransfer.Text = "&Transfer"
        Me.btnTransfer.UseVisualStyleBackColor = True
        '
        'dtpDate
        '
        Me.dtpDate.Location = New System.Drawing.Point(116, 68)
        Me.dtpDate.Mask = "##/##/####"
        Me.dtpDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpDate.Size = New System.Drawing.Size(94, 21)
        Me.dtpDate.TabIndex = 8
        Me.dtpDate.Text = "07/03/9998"
        Me.dtpDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'txtItemId
        '
        Me.txtItemId.Location = New System.Drawing.Point(116, 95)
        Me.txtItemId.Name = "txtItemId"
        Me.txtItemId.Size = New System.Drawing.Size(94, 21)
        Me.txtItemId.TabIndex = 14
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(17, 99)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(50, 13)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Item Id"
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(477, 147)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 27
        Me.btnPrint.Text = "Print [P]"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(371, 147)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 26
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(372, 72)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(90, 13)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Counter Name"
        '
        'chkCheckByScan
        '
        Me.chkCheckByScan.AutoSize = True
        Me.chkCheckByScan.Location = New System.Drawing.Point(375, 14)
        Me.chkCheckByScan.Name = "chkCheckByScan"
        Me.chkCheckByScan.Size = New System.Drawing.Size(112, 17)
        Me.chkCheckByScan.TabIndex = 2
        Me.chkCheckByScan.Text = "Check by Scan"
        Me.chkCheckByScan.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(54, 147)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 23
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'txtLotNo_NUM
        '
        Me.txtLotNo_NUM.Location = New System.Drawing.Point(275, 68)
        Me.txtLotNo_NUM.Name = "txtLotNo_NUM"
        Me.txtLotNo_NUM.Size = New System.Drawing.Size(94, 21)
        Me.txtLotNo_NUM.TabIndex = 10
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(218, 72)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(43, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Lot No"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.grpNonTag)
        Me.Panel1.Controls.Add(Me.txtSearch)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.cmbSearchKey)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.txtEstNo)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.chkCounter)
        Me.Panel1.Controls.Add(Me.cmbCounter_OWN)
        Me.Panel1.Controls.Add(Me.cmbMetal)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.btnSearch)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.cmbTagsCostName_MAN)
        Me.Panel1.Controls.Add(Me.txtTagNo)
        Me.Panel1.Controls.Add(Me.chkCheckByScan)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.chkRecDate)
        Me.Panel1.Controls.Add(Me.dtpDate)
        Me.Panel1.Controls.Add(Me.btnTransfer)
        Me.Panel1.Controls.Add(Me.txtItemId)
        Me.Panel1.Controls.Add(Me.cmbCostCentre_MAN)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.txtLotNo_NUM)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(934, 180)
        Me.Panel1.TabIndex = 0
        '
        'grpNonTag
        '
        Me.grpNonTag.Controls.Add(Me.Label17)
        Me.grpNonTag.Controls.Add(Me.txtSubItemName_OWN)
        Me.grpNonTag.Controls.Add(Me.Label13)
        Me.grpNonTag.Controls.Add(Me.cmbDesigner_OWN)
        Me.grpNonTag.Controls.Add(Me.txtPcs_NUM)
        Me.grpNonTag.Controls.Add(Me.Label14)
        Me.grpNonTag.Controls.Add(Me.txtGrsWt_WET)
        Me.grpNonTag.Controls.Add(Me.txtNetWt_WET)
        Me.grpNonTag.Controls.Add(Me.Label15)
        Me.grpNonTag.Controls.Add(Me.Label16)
        Me.grpNonTag.Controls.Add(Me.Label10)
        Me.grpNonTag.Controls.Add(Me.txtOldPcs)
        Me.grpNonTag.Controls.Add(Me.txtOldGrsWt)
        Me.grpNonTag.Controls.Add(Me.Label11)
        Me.grpNonTag.Controls.Add(Me.txtOldNetWt)
        Me.grpNonTag.Controls.Add(Me.Label12)
        Me.grpNonTag.Location = New System.Drawing.Point(687, 0)
        Me.grpNonTag.Name = "grpNonTag"
        Me.grpNonTag.Size = New System.Drawing.Size(244, 180)
        Me.grpNonTag.TabIndex = 29
        Me.grpNonTag.TabStop = False
        Me.grpNonTag.Visible = False
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(6, 53)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(54, 13)
        Me.Label17.TabIndex = 30
        Me.Label17.Text = "Subitem"
        '
        'txtSubItemName_OWN
        '
        Me.txtSubItemName_OWN.Location = New System.Drawing.Point(82, 50)
        Me.txtSubItemName_OWN.Name = "txtSubItemName_OWN"
        Me.txtSubItemName_OWN.Size = New System.Drawing.Size(156, 21)
        Me.txtSubItemName_OWN.TabIndex = 29
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(6, 111)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(26, 13)
        Me.Label13.TabIndex = 21
        Me.Label13.Text = "Pcs"
        '
        'cmbDesigner_OWN
        '
        Me.cmbDesigner_OWN.FormattingEnabled = True
        Me.cmbDesigner_OWN.Location = New System.Drawing.Point(82, 76)
        Me.cmbDesigner_OWN.Name = "cmbDesigner_OWN"
        Me.cmbDesigner_OWN.Size = New System.Drawing.Size(156, 21)
        Me.cmbDesigner_OWN.TabIndex = 30
        '
        'txtPcs_NUM
        '
        Me.txtPcs_NUM.Location = New System.Drawing.Point(82, 103)
        Me.txtPcs_NUM.Name = "txtPcs_NUM"
        Me.txtPcs_NUM.Size = New System.Drawing.Size(93, 21)
        Me.txtPcs_NUM.TabIndex = 31
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(6, 134)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(70, 13)
        Me.Label14.TabIndex = 23
        Me.Label14.Text = "Grs Weight"
        '
        'txtGrsWt_WET
        '
        Me.txtGrsWt_WET.Location = New System.Drawing.Point(82, 130)
        Me.txtGrsWt_WET.Name = "txtGrsWt_WET"
        Me.txtGrsWt_WET.Size = New System.Drawing.Size(93, 21)
        Me.txtGrsWt_WET.TabIndex = 32
        '
        'txtNetWt_WET
        '
        Me.txtNetWt_WET.Location = New System.Drawing.Point(82, 153)
        Me.txtNetWt_WET.Name = "txtNetWt_WET"
        Me.txtNetWt_WET.Size = New System.Drawing.Size(93, 21)
        Me.txtNetWt_WET.TabIndex = 34
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(6, 79)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(58, 13)
        Me.Label15.TabIndex = 27
        Me.Label15.Text = "Designer"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(6, 161)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(69, 13)
        Me.Label16.TabIndex = 25
        Me.Label16.Text = "Net Weight"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(3, 8)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(26, 13)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "Pcs"
        '
        'txtOldPcs
        '
        Me.txtOldPcs.Location = New System.Drawing.Point(6, 23)
        Me.txtOldPcs.Name = "txtOldPcs"
        Me.txtOldPcs.Size = New System.Drawing.Size(38, 21)
        Me.txtOldPcs.TabIndex = 1
        Me.txtOldPcs.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtOldGrsWt
        '
        Me.txtOldGrsWt.Location = New System.Drawing.Point(49, 23)
        Me.txtOldGrsWt.Name = "txtOldGrsWt"
        Me.txtOldGrsWt.Size = New System.Drawing.Size(93, 21)
        Me.txtOldGrsWt.TabIndex = 3
        Me.txtOldGrsWt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(46, 8)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(70, 13)
        Me.Label11.TabIndex = 2
        Me.Label11.Text = "Grs Weight"
        '
        'txtOldNetWt
        '
        Me.txtOldNetWt.Location = New System.Drawing.Point(148, 23)
        Me.txtOldNetWt.Name = "txtOldNetWt"
        Me.txtOldNetWt.Size = New System.Drawing.Size(87, 21)
        Me.txtOldNetWt.TabIndex = 5
        Me.txtOldNetWt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(148, 8)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(69, 13)
        Me.Label12.TabIndex = 4
        Me.Label12.Text = "Net Weight"
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(462, 123)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(219, 21)
        Me.txtSearch.TabIndex = 22
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(377, 123)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(76, 13)
        Me.Label9.TabIndex = 21
        Me.Label9.Text = "Search Text"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbSearchKey
        '
        Me.cmbSearchKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSearchKey.FormattingEnabled = True
        Me.cmbSearchKey.Location = New System.Drawing.Point(116, 121)
        Me.cmbSearchKey.Name = "cmbSearchKey"
        Me.cmbSearchKey.Size = New System.Drawing.Size(253, 21)
        Me.cmbSearchKey.TabIndex = 20
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(17, 126)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(73, 13)
        Me.Label8.TabIndex = 19
        Me.Label8.Text = "Search Key"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtEstNo
        '
        Me.txtEstNo.Location = New System.Drawing.Point(462, 97)
        Me.txtEstNo.Name = "txtEstNo"
        Me.txtEstNo.Size = New System.Drawing.Size(94, 21)
        Me.txtEstNo.TabIndex = 18
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(372, 99)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(85, 13)
        Me.Label7.TabIndex = 17
        Me.Label7.Text = "Estimation No"
        '
        'chkCounter
        '
        Me.chkCounter.AutoSize = True
        Me.chkCounter.Location = New System.Drawing.Point(375, 41)
        Me.chkCounter.Name = "chkCounter"
        Me.chkCounter.Size = New System.Drawing.Size(81, 17)
        Me.chkCounter.TabIndex = 5
        Me.chkCounter.Text = "Metalwise"
        Me.chkCounter.UseVisualStyleBackColor = True
        '
        'cmbCounter_OWN
        '
        Me.cmbCounter_OWN.CheckOnClick = True
        Me.cmbCounter_OWN.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbCounter_OWN.DropDownHeight = 1
        Me.cmbCounter_OWN.FormattingEnabled = True
        Me.cmbCounter_OWN.IntegralHeight = False
        Me.cmbCounter_OWN.Location = New System.Drawing.Point(462, 68)
        Me.cmbCounter_OWN.Name = "cmbCounter_OWN"
        Me.cmbCounter_OWN.Size = New System.Drawing.Size(204, 22)
        Me.cmbCounter_OWN.TabIndex = 12
        Me.cmbCounter_OWN.ValueSeparator = ", "
        '
        'cmbMetal
        '
        Me.cmbMetal.CheckOnClick = True
        Me.cmbMetal.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbMetal.DropDownHeight = 1
        Me.cmbMetal.Enabled = False
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.IntegralHeight = False
        Me.cmbMetal.Location = New System.Drawing.Point(462, 39)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(204, 22)
        Me.cmbMetal.TabIndex = 6
        Me.cmbMetal.ValueSeparator = ", "
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(14, 43)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(100, 13)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "Tag's CostName"
        '
        'cmbTagsCostName_MAN
        '
        Me.cmbTagsCostName_MAN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTagsCostName_MAN.FormattingEnabled = True
        Me.cmbTagsCostName_MAN.Location = New System.Drawing.Point(116, 39)
        Me.cmbTagsCostName_MAN.Name = "cmbTagsCostName_MAN"
        Me.cmbTagsCostName_MAN.Size = New System.Drawing.Size(253, 21)
        Me.cmbTagsCostName_MAN.TabIndex = 4
        '
        'frmTagTransfer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(934, 520)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.gridView)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmTagTransfer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Tag Transfer"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmbGridShortCut.ResumeLayout(False)
        CType(Me.FileSystemWatcher1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.grpNonTag.ResumeLayout(False)
        Me.grpNonTag.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtTagNo As System.Windows.Forms.TextBox
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents FileSystemWatcher1 As System.IO.FileSystemWatcher
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkRecDate As System.Windows.Forms.CheckBox
    Friend WithEvents cmbCostCentre_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnTransfer As System.Windows.Forms.Button
    Friend WithEvents dtpDate As BrighttechPack.DatePicker
    Friend WithEvents txtItemId As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents chkCheckByScan As System.Windows.Forms.CheckBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents txtLotNo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbTagsCostName_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents cmbMetal As BrighttechPack.CheckedComboBox
    Friend WithEvents cmbCounter_OWN As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCounter As System.Windows.Forms.CheckBox
    Friend WithEvents txtEstNo As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cmbSearchKey As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents grpNonTag As System.Windows.Forms.GroupBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtOldPcs As System.Windows.Forms.TextBox
    Friend WithEvents txtOldGrsWt As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtOldNetWt As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents cmbDesigner_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents txtPcs_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtGrsWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtNetWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents txtSubItemName_OWN As System.Windows.Forms.TextBox
End Class
