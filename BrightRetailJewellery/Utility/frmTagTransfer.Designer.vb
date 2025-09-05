<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTagTransfer
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
        Me.txtTagNo = New System.Windows.Forms.TextBox()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FileSystemWatcher1 = New System.IO.FileSystemWatcher()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FindToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.chkRecDate = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbCostCentre_MAN = New System.Windows.Forms.ComboBox()
        Me.btnTransfer = New System.Windows.Forms.Button()
        Me.txtItemId = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.chkCheckByScan = New System.Windows.Forms.CheckBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.txtLotNo_NUM = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.chkWithTransferStk = New System.Windows.Forms.CheckBox()
        Me.CmbStockType = New System.Windows.Forms.ComboBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.CmbDesigner = New System.Windows.Forms.ComboBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtSubItemID = New System.Windows.Forms.TextBox()
        Me.chkSelect = New System.Windows.Forms.CheckBox()
        Me.lblAcname = New System.Windows.Forms.Label()
        Me.CmbAcname = New System.Windows.Forms.ComboBox()
        Me.cmbSubitem = New System.Windows.Forms.ComboBox()
        Me.lblSubitemid = New System.Windows.Forms.Label()
        Me.txtRemark2 = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtRemark1 = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.ChkNontag = New System.Windows.Forms.CheckBox()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cmbSearchKey = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtEstNo = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.chkCounter = New System.Windows.Forms.CheckBox()
        Me.cmbCounter_OWN = New BrighttechPack.CheckedComboBox()
        Me.cmbMetal = New BrighttechPack.CheckedComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbTagsCostName_MAN = New System.Windows.Forms.ComboBox()
        Me.dtpDate = New BrighttechPack.DatePicker(Me.components)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmbGridShortCut.SuspendLayout()
        CType(Me.FileSystemWatcher1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtTagNo
        '
        Me.txtTagNo.Location = New System.Drawing.Point(585, 106)
        Me.txtTagNo.Margin = New System.Windows.Forms.Padding(4)
        Me.txtTagNo.Name = "txtTagNo"
        Me.txtTagNo.Size = New System.Drawing.Size(116, 24)
        Me.txtTagNo.TabIndex = 20
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 240)
        Me.gridView.Margin = New System.Windows.Forms.Padding(4)
        Me.gridView.Name = "gridView"
        Me.gridView.Size = New System.Drawing.Size(1168, 410)
        Me.gridView.TabIndex = 1
        'AddHandler Me.gridView.CellContentClick, AddressOf Me.gridView_CellChecked
        '
        'cmbGridShortCut
        '
        Me.cmbGridShortCut.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResizeToolStripMenuItem})
        Me.cmbGridShortCut.Name = "ContextMenuStrip2"
        Me.cmbGridShortCut.Size = New System.Drawing.Size(157, 28)
        '
        'ResizeToolStripMenuItem
        '
        Me.ResizeToolStripMenuItem.CheckOnClick = True
        Me.ResizeToolStripMenuItem.Name = "ResizeToolStripMenuItem"
        Me.ResizeToolStripMenuItem.Size = New System.Drawing.Size(156, 24)
        Me.ResizeToolStripMenuItem.Text = "Auto Resize"
        '
        'FileSystemWatcher1
        '
        Me.FileSystemWatcher1.EnableRaisingEvents = True
        Me.FileSystemWatcher1.Filter = "*.*"
        Me.FileSystemWatcher1.SynchronizingObject = Me
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(469, 112)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 17)
        Me.Label2.TabIndex = 19
        Me.Label2.Text = "Tag No"
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(402, 199)
        Me.btnNew.Margin = New System.Windows.Forms.Padding(4)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(125, 38)
        Me.btnNew.TabIndex = 41
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(800, 199)
        Me.btnExit.Margin = New System.Windows.Forms.Padding(4)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(125, 38)
        Me.btnExit.TabIndex = 44
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem, Me.FindToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(156, 76)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(155, 24)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(155, 24)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'FindToolStripMenuItem
        '
        Me.FindToolStripMenuItem.Name = "FindToolStripMenuItem"
        Me.FindToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.FindToolStripMenuItem.Size = New System.Drawing.Size(155, 24)
        Me.FindToolStripMenuItem.Text = "Find"
        Me.FindToolStripMenuItem.Visible = False
        '
        'chkRecDate
        '
        Me.chkRecDate.AutoSize = True
        Me.chkRecDate.Location = New System.Drawing.Point(11, 78)
        Me.chkRecDate.Margin = New System.Windows.Forms.Padding(4)
        Me.chkRecDate.Name = "chkRecDate"
        Me.chkRecDate.Size = New System.Drawing.Size(93, 21)
        Me.chkRecDate.TabIndex = 9
        Me.chkRecDate.Text = "Rec Date"
        Me.chkRecDate.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 19)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(87, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Transfer To"
        '
        'cmbCostCentre_MAN
        '
        Me.cmbCostCentre_MAN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCostCentre_MAN.FormattingEnabled = True
        Me.cmbCostCentre_MAN.Location = New System.Drawing.Point(139, 15)
        Me.cmbCostCentre_MAN.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbCostCentre_MAN.Name = "cmbCostCentre_MAN"
        Me.cmbCostCentre_MAN.Size = New System.Drawing.Size(315, 25)
        Me.cmbCostCentre_MAN.TabIndex = 1
        '
        'btnTransfer
        '
        Me.btnTransfer.Location = New System.Drawing.Point(270, 199)
        Me.btnTransfer.Margin = New System.Windows.Forms.Padding(4)
        Me.btnTransfer.Name = "btnTransfer"
        Me.btnTransfer.Size = New System.Drawing.Size(125, 38)
        Me.btnTransfer.TabIndex = 40
        Me.btnTransfer.Text = "&Transfer"
        Me.btnTransfer.UseVisualStyleBackColor = True
        '
        'txtItemId
        '
        Me.txtItemId.Location = New System.Drawing.Point(139, 106)
        Me.txtItemId.Margin = New System.Windows.Forms.Padding(4)
        Me.txtItemId.Name = "txtItemId"
        Me.txtItemId.Size = New System.Drawing.Size(116, 24)
        Me.txtItemId.TabIndex = 16
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(11, 112)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(59, 17)
        Me.Label3.TabIndex = 15
        Me.Label3.Text = "Item Id"
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(668, 199)
        Me.btnPrint.Margin = New System.Windows.Forms.Padding(4)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(125, 38)
        Me.btnPrint.TabIndex = 43
        Me.btnPrint.Text = "Print [P]"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(535, 199)
        Me.btnExport.Margin = New System.Windows.Forms.Padding(4)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(125, 38)
        Me.btnExport.TabIndex = 42
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(469, 82)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(109, 17)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "Counter Name"
        '
        'chkCheckByScan
        '
        Me.chkCheckByScan.AutoSize = True
        Me.chkCheckByScan.Location = New System.Drawing.Point(921, 20)
        Me.chkCheckByScan.Margin = New System.Windows.Forms.Padding(4)
        Me.chkCheckByScan.Name = "chkCheckByScan"
        Me.chkCheckByScan.Size = New System.Drawing.Size(133, 21)
        Me.chkCheckByScan.TabIndex = 4
        Me.chkCheckByScan.Text = "Check by Scan"
        Me.chkCheckByScan.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(139, 199)
        Me.btnSearch.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(125, 38)
        Me.btnSearch.TabIndex = 39
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'txtLotNo_NUM
        '
        Me.txtLotNo_NUM.Location = New System.Drawing.Point(344, 75)
        Me.txtLotNo_NUM.Margin = New System.Windows.Forms.Padding(4)
        Me.txtLotNo_NUM.Name = "txtLotNo_NUM"
        Me.txtLotNo_NUM.Size = New System.Drawing.Size(116, 24)
        Me.txtLotNo_NUM.TabIndex = 12
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(260, 82)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(55, 17)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Lot No"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.chkWithTransferStk)
        Me.Panel1.Controls.Add(Me.CmbStockType)
        Me.Panel1.Controls.Add(Me.Label14)
        Me.Panel1.Controls.Add(Me.CmbDesigner)
        Me.Panel1.Controls.Add(Me.Label13)
        Me.Panel1.Controls.Add(Me.Label12)
        Me.Panel1.Controls.Add(Me.txtSubItemID)
        Me.Panel1.Controls.Add(Me.chkSelect)
        Me.Panel1.Controls.Add(Me.lblAcname)
        Me.Panel1.Controls.Add(Me.CmbAcname)
        Me.Panel1.Controls.Add(Me.cmbSubitem)
        Me.Panel1.Controls.Add(Me.lblSubitemid)
        Me.Panel1.Controls.Add(Me.txtRemark2)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.txtRemark1)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.ChkNontag)
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
        Me.Panel1.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1168, 240)
        Me.Panel1.TabIndex = 0
        '
        'chkWithTransferStk
        '
        Me.chkWithTransferStk.AutoSize = True
        Me.chkWithTransferStk.Checked = True
        Me.chkWithTransferStk.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkWithTransferStk.Location = New System.Drawing.Point(585, 171)
        Me.chkWithTransferStk.Margin = New System.Windows.Forms.Padding(4)
        Me.chkWithTransferStk.Name = "chkWithTransferStk"
        Me.chkWithTransferStk.Size = New System.Drawing.Size(171, 21)
        Me.chkWithTransferStk.TabIndex = 26
        Me.chkWithTransferStk.Text = "With Transfer Stock"
        Me.chkWithTransferStk.UseVisualStyleBackColor = True
        '
        'CmbStockType
        '
        Me.CmbStockType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbStockType.FormattingEnabled = True
        Me.CmbStockType.Items.AddRange(New Object() {"ALL", "TRADING", "MANUFACTURING", "EXEMPTED"})
        Me.CmbStockType.Location = New System.Drawing.Point(139, 168)
        Me.CmbStockType.Margin = New System.Windows.Forms.Padding(4)
        Me.CmbStockType.Name = "CmbStockType"
        Me.CmbStockType.Size = New System.Drawing.Size(315, 25)
        Me.CmbStockType.TabIndex = 38
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(11, 174)
        Me.Label14.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(86, 17)
        Me.Label14.TabIndex = 37
        Me.Label14.Text = "Stock Type"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CmbDesigner
        '
        Me.CmbDesigner.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbDesigner.FormattingEnabled = True
        Me.CmbDesigner.Location = New System.Drawing.Point(139, 138)
        Me.CmbDesigner.Margin = New System.Windows.Forms.Padding(4)
        Me.CmbDesigner.Name = "CmbDesigner"
        Me.CmbDesigner.Size = New System.Drawing.Size(315, 25)
        Me.CmbDesigner.TabIndex = 22
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(11, 144)
        Me.Label13.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(70, 17)
        Me.Label13.TabIndex = 21
        Me.Label13.Text = "Designer"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(260, 112)
        Me.Label12.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(82, 17)
        Me.Label12.TabIndex = 17
        Me.Label12.Text = "SubItemId"
        '
        'txtSubItemID
        '
        Me.txtSubItemID.Location = New System.Drawing.Point(344, 106)
        Me.txtSubItemID.Margin = New System.Windows.Forms.Padding(4)
        Me.txtSubItemID.Name = "txtSubItemID"
        Me.txtSubItemID.Size = New System.Drawing.Size(116, 24)
        Me.txtSubItemID.TabIndex = 18
        '
        'chkSelect
        '
        Me.chkSelect.AutoSize = True
        Me.chkSelect.Location = New System.Drawing.Point(15, 211)
        Me.chkSelect.Margin = New System.Windows.Forms.Padding(4)
        Me.chkSelect.Name = "chkSelect"
        Me.chkSelect.Size = New System.Drawing.Size(72, 21)
        Me.chkSelect.TabIndex = 45
        Me.chkSelect.Text = "Select"
        Me.chkSelect.UseVisualStyleBackColor = True
        AddHandler Me.chkSelect.CheckedChanged, AddressOf Me.chkSelect_CheckedChanged
        '
        'lblAcname
        '
        Me.lblAcname.AutoSize = True
        Me.lblAcname.Location = New System.Drawing.Point(469, 19)
        Me.lblAcname.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblAcname.Name = "lblAcname"
        Me.lblAcname.Size = New System.Drawing.Size(63, 17)
        Me.lblAcname.TabIndex = 2
        Me.lblAcname.Text = "Acname"
        '
        'CmbAcname
        '
        Me.CmbAcname.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbAcname.FormattingEnabled = True
        Me.CmbAcname.Location = New System.Drawing.Point(585, 15)
        Me.CmbAcname.Margin = New System.Windows.Forms.Padding(4)
        Me.CmbAcname.Name = "CmbAcname"
        Me.CmbAcname.Size = New System.Drawing.Size(335, 25)
        Me.CmbAcname.TabIndex = 3
        '
        'cmbSubitem
        '
        Me.cmbSubitem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSubitem.FormattingEnabled = True
        Me.cmbSubitem.Location = New System.Drawing.Point(934, 206)
        Me.cmbSubitem.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbSubitem.Name = "cmbSubitem"
        Me.cmbSubitem.Size = New System.Drawing.Size(238, 25)
        Me.cmbSubitem.TabIndex = 36
        '
        'lblSubitemid
        '
        Me.lblSubitemid.AutoSize = True
        Me.lblSubitemid.Location = New System.Drawing.Point(934, 179)
        Me.lblSubitemid.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSubitemid.Name = "lblSubitemid"
        Me.lblSubitemid.Size = New System.Drawing.Size(92, 17)
        Me.lblSubitemid.TabIndex = 35
        Me.lblSubitemid.Text = "Sub Item Id"
        '
        'txtRemark2
        '
        Me.txtRemark2.Location = New System.Drawing.Point(924, 138)
        Me.txtRemark2.Margin = New System.Windows.Forms.Padding(4)
        Me.txtRemark2.MaxLength = 50
        Me.txtRemark2.Name = "txtRemark2"
        Me.txtRemark2.Size = New System.Drawing.Size(238, 24)
        Me.txtRemark2.TabIndex = 34
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(828, 144)
        Me.Label11.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(70, 17)
        Me.Label11.TabIndex = 33
        Me.Label11.Text = "Remark2"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtRemark1
        '
        Me.txtRemark1.Location = New System.Drawing.Point(924, 106)
        Me.txtRemark1.Margin = New System.Windows.Forms.Padding(4)
        Me.txtRemark1.MaxLength = 50
        Me.txtRemark1.Name = "txtRemark1"
        Me.txtRemark1.Size = New System.Drawing.Size(238, 24)
        Me.txtRemark1.TabIndex = 32
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(828, 110)
        Me.Label10.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(70, 17)
        Me.Label10.TabIndex = 31
        Me.Label10.Text = "Remark1"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ChkNontag
        '
        Me.ChkNontag.AutoSize = True
        Me.ChkNontag.Location = New System.Drawing.Point(472, 171)
        Me.ChkNontag.Margin = New System.Windows.Forms.Padding(4)
        Me.ChkNontag.Name = "ChkNontag"
        Me.ChkNontag.Size = New System.Drawing.Size(86, 21)
        Me.ChkNontag.TabIndex = 25
        Me.ChkNontag.Text = "Non tag"
        Me.ChkNontag.UseVisualStyleBackColor = True
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(924, 75)
        Me.txtSearch.Margin = New System.Windows.Forms.Padding(4)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(238, 24)
        Me.txtSearch.TabIndex = 30
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(828, 81)
        Me.Label9.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(91, 17)
        Me.Label9.TabIndex = 29
        Me.Label9.Text = "Search Text"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbSearchKey
        '
        Me.cmbSearchKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSearchKey.FormattingEnabled = True
        Me.cmbSearchKey.Location = New System.Drawing.Point(924, 45)
        Me.cmbSearchKey.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbSearchKey.Name = "cmbSearchKey"
        Me.cmbSearchKey.Size = New System.Drawing.Size(238, 25)
        Me.cmbSearchKey.TabIndex = 28
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(828, 50)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(87, 17)
        Me.Label8.TabIndex = 27
        Me.Label8.Text = "Search Key"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtEstNo
        '
        Me.txtEstNo.Location = New System.Drawing.Point(585, 138)
        Me.txtEstNo.Margin = New System.Windows.Forms.Padding(4)
        Me.txtEstNo.Name = "txtEstNo"
        Me.txtEstNo.Size = New System.Drawing.Size(116, 24)
        Me.txtEstNo.TabIndex = 24
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(469, 142)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(106, 17)
        Me.Label7.TabIndex = 23
        Me.Label7.Text = "Estimation No"
        '
        'chkCounter
        '
        Me.chkCounter.AutoSize = True
        Me.chkCounter.Location = New System.Drawing.Point(469, 45)
        Me.chkCounter.Margin = New System.Windows.Forms.Padding(4)
        Me.chkCounter.Name = "chkCounter"
        Me.chkCounter.Size = New System.Drawing.Size(96, 21)
        Me.chkCounter.TabIndex = 7
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
        Me.cmbCounter_OWN.Location = New System.Drawing.Point(585, 74)
        Me.cmbCounter_OWN.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbCounter_OWN.Name = "cmbCounter_OWN"
        Me.cmbCounter_OWN.Size = New System.Drawing.Size(238, 25)
        Me.cmbCounter_OWN.TabIndex = 14
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
        Me.cmbMetal.Location = New System.Drawing.Point(585, 44)
        Me.cmbMetal.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(238, 25)
        Me.cmbMetal.TabIndex = 8
        Me.cmbMetal.ValueSeparator = ", "
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(11, 50)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(121, 17)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Tag's CostName"
        '
        'cmbTagsCostName_MAN
        '
        Me.cmbTagsCostName_MAN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTagsCostName_MAN.FormattingEnabled = True
        Me.cmbTagsCostName_MAN.Location = New System.Drawing.Point(139, 45)
        Me.cmbTagsCostName_MAN.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbTagsCostName_MAN.Name = "cmbTagsCostName_MAN"
        Me.cmbTagsCostName_MAN.Size = New System.Drawing.Size(315, 25)
        Me.cmbTagsCostName_MAN.TabIndex = 6
        '
        'dtpDate
        '
        Me.dtpDate.Location = New System.Drawing.Point(139, 75)
        Me.dtpDate.Margin = New System.Windows.Forms.Padding(4)
        Me.dtpDate.Mask = "##/##/####"
        Me.dtpDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpDate.Size = New System.Drawing.Size(116, 24)
        Me.dtpDate.TabIndex = 10
        Me.dtpDate.Text = "07/03/9998"
        Me.dtpDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'frmTagTransfer
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(120.0!, 120.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1168, 650)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.gridView)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4)
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
    Friend WithEvents ChkNontag As System.Windows.Forms.CheckBox
    Friend WithEvents txtRemark2 As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtRemark1 As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents lblSubitemid As System.Windows.Forms.Label
    Friend WithEvents cmbSubitem As System.Windows.Forms.ComboBox
    Friend WithEvents lblAcname As System.Windows.Forms.Label
    Friend WithEvents CmbAcname As System.Windows.Forms.ComboBox
    Friend WithEvents chkSelect As System.Windows.Forms.CheckBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtSubItemID As System.Windows.Forms.TextBox
    Friend WithEvents CmbDesigner As System.Windows.Forms.ComboBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents CmbStockType As System.Windows.Forms.ComboBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents FindToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkWithTransferStk As CheckBox
End Class
