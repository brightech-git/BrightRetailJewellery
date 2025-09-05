<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStockCheckWithRFID
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.pnlMain = New System.Windows.Forms.Panel()
        Me.pnlgrid = New System.Windows.Forms.Panel()
        Me.gridFullView = New System.Windows.Forms.DataGridView()
        Me.gridLastView = New System.Windows.Forms.DataGridView()
        Me.pnlTop = New System.Windows.Forms.Panel()
        Me.lblCount = New System.Windows.Forms.Label()
        Me.btnConnect = New System.Windows.Forms.Button()
        Me.btnScan = New System.Windows.Forms.Button()
        Me.cmbRange = New System.Windows.Forms.ComboBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.cmbItemType = New System.Windows.Forms.ComboBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.CmbMetal = New System.Windows.Forms.ComboBox()
        Me.chkCmbCounter = New BrighttechPack.CheckedComboBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.CmbCompany = New System.Windows.Forms.ComboBox()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnImportdata = New System.Windows.Forms.Button()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.cmbSubItem = New System.Windows.Forms.ComboBox()
        Me.cmbCostCentre_MAN = New System.Windows.Forms.ComboBox()
        Me.dtpAsOnDate = New BrighttechPack.DatePicker(Me.components)
        Me.btnNew = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtTagNo = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbItemName = New System.Windows.Forms.ComboBox()
        Me.txtTrayNo = New System.Windows.Forms.TextBox()
        Me.cmbDesignerName = New System.Windows.Forms.ComboBox()
        Me.rbtMarked = New System.Windows.Forms.RadioButton()
        Me.rbtPending = New System.Windows.Forms.RadioButton()
        Me.chkAsonDate = New System.Windows.Forms.CheckBox()
        Me.rbtAll = New System.Windows.Forms.RadioButton()
        Me.chkWithApproval = New System.Windows.Forms.CheckBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.picTagImage = New System.Windows.Forms.PictureBox()
        Me.pnlStoneTotal = New System.Windows.Forms.Panel()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtStPcs = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtDiaPcs = New System.Windows.Forms.TextBox()
        Me.txtPreWt = New System.Windows.Forms.TextBox()
        Me.txtPrePcs = New System.Windows.Forms.TextBox()
        Me.txtDiaWt = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtStWt = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.gridStone = New System.Windows.Forms.DataGridView()
        Me.lstTotal = New System.Windows.Forms.ListView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.pnlMain.SuspendLayout()
        Me.pnlgrid.SuspendLayout()
        CType(Me.gridFullView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridLastView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlTop.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.picTagImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlStoneTotal.SuspendLayout()
        CType(Me.gridStone, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlMain
        '
        Me.pnlMain.Controls.Add(Me.pnlgrid)
        Me.pnlMain.Controls.Add(Me.pnlTop)
        Me.pnlMain.Controls.Add(Me.Panel2)
        Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlMain.Location = New System.Drawing.Point(0, 0)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(1011, 526)
        Me.pnlMain.TabIndex = 0
        '
        'pnlgrid
        '
        Me.pnlgrid.Controls.Add(Me.gridFullView)
        Me.pnlgrid.Controls.Add(Me.gridLastView)
        Me.pnlgrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlgrid.Location = New System.Drawing.Point(0, 168)
        Me.pnlgrid.Name = "pnlgrid"
        Me.pnlgrid.Size = New System.Drawing.Size(1011, 213)
        Me.pnlgrid.TabIndex = 1
        '
        'gridFullView
        '
        Me.gridFullView.AllowUserToAddRows = False
        Me.gridFullView.AllowUserToDeleteRows = False
        Me.gridFullView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridFullView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.gridFullView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridFullView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridFullView.Location = New System.Drawing.Point(0, 26)
        Me.gridFullView.MultiSelect = False
        Me.gridFullView.Name = "gridFullView"
        Me.gridFullView.ReadOnly = True
        Me.gridFullView.RowHeadersVisible = False
        Me.gridFullView.RowTemplate.Height = 17
        Me.gridFullView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridFullView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridFullView.Size = New System.Drawing.Size(1011, 187)
        Me.gridFullView.TabIndex = 1
        '
        'gridLastView
        '
        Me.gridLastView.AllowUserToAddRows = False
        Me.gridLastView.AllowUserToDeleteRows = False
        Me.gridLastView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridLastView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.gridLastView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridLastView.ColumnHeadersVisible = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ControlLightLight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridLastView.DefaultCellStyle = DataGridViewCellStyle1
        Me.gridLastView.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridLastView.Enabled = False
        Me.gridLastView.Location = New System.Drawing.Point(0, 0)
        Me.gridLastView.MultiSelect = False
        Me.gridLastView.Name = "gridLastView"
        Me.gridLastView.ReadOnly = True
        Me.gridLastView.RowHeadersVisible = False
        Me.gridLastView.RowTemplate.Height = 17
        Me.gridLastView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridLastView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridLastView.Size = New System.Drawing.Size(1011, 26)
        Me.gridLastView.TabIndex = 0
        '
        'pnlTop
        '
        Me.pnlTop.Controls.Add(Me.lblCount)
        Me.pnlTop.Controls.Add(Me.btnConnect)
        Me.pnlTop.Controls.Add(Me.btnScan)
        Me.pnlTop.Controls.Add(Me.cmbRange)
        Me.pnlTop.Controls.Add(Me.Label16)
        Me.pnlTop.Controls.Add(Me.cmbItemType)
        Me.pnlTop.Controls.Add(Me.Label15)
        Me.pnlTop.Controls.Add(Me.CmbMetal)
        Me.pnlTop.Controls.Add(Me.chkCmbCounter)
        Me.pnlTop.Controls.Add(Me.Label14)
        Me.pnlTop.Controls.Add(Me.CmbCompany)
        Me.pnlTop.Controls.Add(Me.btnPrint)
        Me.pnlTop.Controls.Add(Me.btnExport)
        Me.pnlTop.Controls.Add(Me.btnImportdata)
        Me.pnlTop.Controls.Add(Me.Label13)
        Me.pnlTop.Controls.Add(Me.cmbSubItem)
        Me.pnlTop.Controls.Add(Me.cmbCostCentre_MAN)
        Me.pnlTop.Controls.Add(Me.dtpAsOnDate)
        Me.pnlTop.Controls.Add(Me.btnNew)
        Me.pnlTop.Controls.Add(Me.Label1)
        Me.pnlTop.Controls.Add(Me.btnExit)
        Me.pnlTop.Controls.Add(Me.Label7)
        Me.pnlTop.Controls.Add(Me.Label2)
        Me.pnlTop.Controls.Add(Me.btnView_Search)
        Me.pnlTop.Controls.Add(Me.Label4)
        Me.pnlTop.Controls.Add(Me.Label6)
        Me.pnlTop.Controls.Add(Me.txtTagNo)
        Me.pnlTop.Controls.Add(Me.Label3)
        Me.pnlTop.Controls.Add(Me.cmbItemName)
        Me.pnlTop.Controls.Add(Me.txtTrayNo)
        Me.pnlTop.Controls.Add(Me.cmbDesignerName)
        Me.pnlTop.Controls.Add(Me.rbtMarked)
        Me.pnlTop.Controls.Add(Me.rbtPending)
        Me.pnlTop.Controls.Add(Me.chkAsonDate)
        Me.pnlTop.Controls.Add(Me.rbtAll)
        Me.pnlTop.Controls.Add(Me.chkWithApproval)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(1011, 168)
        Me.pnlTop.TabIndex = 0
        '
        'lblCount
        '
        Me.lblCount.AutoSize = True
        Me.lblCount.Location = New System.Drawing.Point(804, 104)
        Me.lblCount.Name = "lblCount"
        Me.lblCount.Size = New System.Drawing.Size(100, 13)
        Me.lblCount.TabIndex = 39
        Me.lblCount.Text = "Scaned Count : "
        '
        'btnConnect
        '
        Me.btnConnect.Location = New System.Drawing.Point(353, 130)
        Me.btnConnect.Name = "btnConnect"
        Me.btnConnect.Size = New System.Drawing.Size(90, 30)
        Me.btnConnect.TabIndex = 38
        Me.btnConnect.Text = "Connect"
        Me.btnConnect.UseVisualStyleBackColor = True
        '
        'btnScan
        '
        Me.btnScan.Location = New System.Drawing.Point(445, 130)
        Me.btnScan.Name = "btnScan"
        Me.btnScan.Size = New System.Drawing.Size(90, 30)
        Me.btnScan.TabIndex = 37
        Me.btnScan.Text = "Start Scan"
        Me.btnScan.UseVisualStyleBackColor = True
        '
        'cmbRange
        '
        Me.cmbRange.FormattingEnabled = True
        Me.cmbRange.Location = New System.Drawing.Point(619, 71)
        Me.cmbRange.Name = "cmbRange"
        Me.cmbRange.Size = New System.Drawing.Size(192, 21)
        Me.cmbRange.TabIndex = 17
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(547, 46)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(65, 13)
        Me.Label16.TabIndex = 14
        Me.Label16.Text = "Item Type"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbItemType
        '
        Me.cmbItemType.FormattingEnabled = True
        Me.cmbItemType.Location = New System.Drawing.Point(619, 44)
        Me.cmbItemType.Name = "cmbItemType"
        Me.cmbItemType.Size = New System.Drawing.Size(192, 21)
        Me.cmbItemType.TabIndex = 15
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(12, 70)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(70, 13)
        Me.Label15.TabIndex = 4
        Me.Label15.Text = "MetalName"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CmbMetal
        '
        Me.CmbMetal.FormattingEnabled = True
        Me.CmbMetal.Location = New System.Drawing.Point(84, 67)
        Me.CmbMetal.Name = "CmbMetal"
        Me.CmbMetal.Size = New System.Drawing.Size(192, 21)
        Me.CmbMetal.TabIndex = 5
        '
        'chkCmbCounter
        '
        Me.chkCmbCounter.CheckOnClick = True
        Me.chkCmbCounter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCounter.DropDownHeight = 1
        Me.chkCmbCounter.FormattingEnabled = True
        Me.chkCmbCounter.IntegralHeight = False
        Me.chkCmbCounter.Location = New System.Drawing.Point(349, 66)
        Me.chkCmbCounter.Name = "chkCmbCounter"
        Me.chkCmbCounter.Size = New System.Drawing.Size(192, 22)
        Me.chkCmbCounter.TabIndex = 11
        Me.chkCmbCounter.ValueSeparator = ", "
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(12, 18)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(62, 13)
        Me.Label14.TabIndex = 0
        Me.Label14.Text = "Company"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CmbCompany
        '
        Me.CmbCompany.FormattingEnabled = True
        Me.CmbCompany.Location = New System.Drawing.Point(84, 16)
        Me.CmbCompany.Name = "CmbCompany"
        Me.CmbCompany.Size = New System.Drawing.Size(192, 21)
        Me.CmbCompany.TabIndex = 1
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(807, 129)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(91, 30)
        Me.btnPrint.TabIndex = 34
        Me.btnPrint.Text = "Print [P]"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(718, 129)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(90, 30)
        Me.btnExport.TabIndex = 33
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnImportdata
        '
        Me.btnImportdata.Location = New System.Drawing.Point(558, 93)
        Me.btnImportdata.Name = "btnImportdata"
        Me.btnImportdata.Size = New System.Drawing.Size(207, 30)
        Me.btnImportdata.TabIndex = 36
        Me.btnImportdata.Text = "&Import Checked Data"
        Me.btnImportdata.UseVisualStyleBackColor = True
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(287, 44)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(60, 13)
        Me.Label13.TabIndex = 8
        Me.Label13.Text = "Sub Item"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbSubItem
        '
        Me.cmbSubItem.FormattingEnabled = True
        Me.cmbSubItem.Location = New System.Drawing.Point(349, 42)
        Me.cmbSubItem.Name = "cmbSubItem"
        Me.cmbSubItem.Size = New System.Drawing.Size(192, 21)
        Me.cmbSubItem.TabIndex = 9
        '
        'cmbCostCentre_MAN
        '
        Me.cmbCostCentre_MAN.FormattingEnabled = True
        Me.cmbCostCentre_MAN.Location = New System.Drawing.Point(84, 42)
        Me.cmbCostCentre_MAN.Name = "cmbCostCentre_MAN"
        Me.cmbCostCentre_MAN.Size = New System.Drawing.Size(192, 21)
        Me.cmbCostCentre_MAN.TabIndex = 3
        '
        'dtpAsOnDate
        '
        Me.dtpAsOnDate.Location = New System.Drawing.Point(103, 98)
        Me.dtpAsOnDate.Mask = "##/##/####"
        Me.dtpAsOnDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpAsOnDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpAsOnDate.Name = "dtpAsOnDate"
        Me.dtpAsOnDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpAsOnDate.Size = New System.Drawing.Size(96, 21)
        Me.dtpAsOnDate.TabIndex = 19
        Me.dtpAsOnDate.Text = "07/03/9998"
        Me.dtpAsOnDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(624, 130)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(94, 30)
        Me.btnNew.TabIndex = 32
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(287, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Item"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(898, 129)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 35
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(12, 44)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(70, 13)
        Me.Label7.TabIndex = 2
        Me.Label7.Text = "Cost Name"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(547, 18)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Designer"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(535, 130)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(90, 30)
        Me.btnView_Search.TabIndex = 31
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(11, 138)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(51, 13)
        Me.Label4.TabIndex = 25
        Me.Label4.Text = "Tray No"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(212, 138)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(42, 13)
        Me.Label6.TabIndex = 29
        Me.Label6.Text = "TagNo"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtTagNo
        '
        Me.txtTagNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTagNo.Location = New System.Drawing.Point(261, 134)
        Me.txtTagNo.MaxLength = 20
        Me.txtTagNo.Name = "txtTagNo"
        Me.txtTagNo.Size = New System.Drawing.Size(86, 21)
        Me.txtTagNo.TabIndex = 30
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(287, 70)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Counter"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbItemName
        '
        Me.cmbItemName.FormattingEnabled = True
        Me.cmbItemName.Location = New System.Drawing.Point(349, 16)
        Me.cmbItemName.Name = "cmbItemName"
        Me.cmbItemName.Size = New System.Drawing.Size(192, 21)
        Me.cmbItemName.TabIndex = 7
        '
        'txtTrayNo
        '
        Me.txtTrayNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTrayNo.Location = New System.Drawing.Point(103, 134)
        Me.txtTrayNo.MaxLength = 7
        Me.txtTrayNo.Name = "txtTrayNo"
        Me.txtTrayNo.Size = New System.Drawing.Size(96, 21)
        Me.txtTrayNo.TabIndex = 26
        '
        'cmbDesignerName
        '
        Me.cmbDesignerName.FormattingEnabled = True
        Me.cmbDesignerName.Location = New System.Drawing.Point(619, 16)
        Me.cmbDesignerName.Name = "cmbDesignerName"
        Me.cmbDesignerName.Size = New System.Drawing.Size(192, 21)
        Me.cmbDesignerName.TabIndex = 13
        '
        'rbtMarked
        '
        Me.rbtMarked.AutoSize = True
        Me.rbtMarked.Location = New System.Drawing.Point(478, 102)
        Me.rbtMarked.Name = "rbtMarked"
        Me.rbtMarked.Size = New System.Drawing.Size(67, 17)
        Me.rbtMarked.TabIndex = 24
        Me.rbtMarked.Text = "Marked"
        Me.rbtMarked.UseVisualStyleBackColor = True
        '
        'rbtPending
        '
        Me.rbtPending.AutoSize = True
        Me.rbtPending.Location = New System.Drawing.Point(392, 102)
        Me.rbtPending.Name = "rbtPending"
        Me.rbtPending.Size = New System.Drawing.Size(70, 17)
        Me.rbtPending.TabIndex = 23
        Me.rbtPending.Text = "Pending"
        Me.rbtPending.UseVisualStyleBackColor = True
        '
        'chkAsonDate
        '
        Me.chkAsonDate.AutoSize = True
        Me.chkAsonDate.Checked = True
        Me.chkAsonDate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAsonDate.Location = New System.Drawing.Point(14, 102)
        Me.chkAsonDate.Name = "chkAsonDate"
        Me.chkAsonDate.Size = New System.Drawing.Size(83, 17)
        Me.chkAsonDate.TabIndex = 18
        Me.chkAsonDate.Text = "AsOnDate"
        Me.chkAsonDate.UseVisualStyleBackColor = True
        '
        'rbtAll
        '
        Me.rbtAll.AutoSize = True
        Me.rbtAll.Checked = True
        Me.rbtAll.Location = New System.Drawing.Point(337, 102)
        Me.rbtAll.Name = "rbtAll"
        Me.rbtAll.Size = New System.Drawing.Size(39, 17)
        Me.rbtAll.TabIndex = 22
        Me.rbtAll.TabStop = True
        Me.rbtAll.Text = "All"
        Me.rbtAll.UseVisualStyleBackColor = True
        '
        'chkWithApproval
        '
        Me.chkWithApproval.AutoSize = True
        Me.chkWithApproval.Checked = True
        Me.chkWithApproval.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkWithApproval.Location = New System.Drawing.Point(215, 102)
        Me.chkWithApproval.Name = "chkWithApproval"
        Me.chkWithApproval.Size = New System.Drawing.Size(106, 17)
        Me.chkWithApproval.TabIndex = 21
        Me.chkWithApproval.Text = "With Approval"
        Me.chkWithApproval.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.picTagImage)
        Me.Panel2.Controls.Add(Me.pnlStoneTotal)
        Me.Panel2.Controls.Add(Me.gridStone)
        Me.Panel2.Controls.Add(Me.lstTotal)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 381)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1011, 145)
        Me.Panel2.TabIndex = 0
        '
        'picTagImage
        '
        Me.picTagImage.Location = New System.Drawing.Point(642, 5)
        Me.picTagImage.Name = "picTagImage"
        Me.picTagImage.Size = New System.Drawing.Size(149, 136)
        Me.picTagImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picTagImage.TabIndex = 9
        Me.picTagImage.TabStop = False
        '
        'pnlStoneTotal
        '
        Me.pnlStoneTotal.Controls.Add(Me.Label10)
        Me.pnlStoneTotal.Controls.Add(Me.txtStPcs)
        Me.pnlStoneTotal.Controls.Add(Me.Label9)
        Me.pnlStoneTotal.Controls.Add(Me.Label8)
        Me.pnlStoneTotal.Controls.Add(Me.txtDiaPcs)
        Me.pnlStoneTotal.Controls.Add(Me.txtPreWt)
        Me.pnlStoneTotal.Controls.Add(Me.txtPrePcs)
        Me.pnlStoneTotal.Controls.Add(Me.txtDiaWt)
        Me.pnlStoneTotal.Controls.Add(Me.Label11)
        Me.pnlStoneTotal.Controls.Add(Me.txtStWt)
        Me.pnlStoneTotal.Controls.Add(Me.Label12)
        Me.pnlStoneTotal.Location = New System.Drawing.Point(396, 61)
        Me.pnlStoneTotal.Name = "pnlStoneTotal"
        Me.pnlStoneTotal.Size = New System.Drawing.Size(196, 77)
        Me.pnlStoneTotal.TabIndex = 8
        '
        'Label10
        '
        Me.Label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label10.Location = New System.Drawing.Point(2, 19)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(61, 17)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "Stone"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtStPcs
        '
        Me.txtStPcs.Enabled = False
        Me.txtStPcs.Location = New System.Drawing.Point(66, 19)
        Me.txtStPcs.Multiline = True
        Me.txtStPcs.Name = "txtStPcs"
        Me.txtStPcs.Size = New System.Drawing.Size(58, 18)
        Me.txtStPcs.TabIndex = 5
        '
        'Label9
        '
        Me.Label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label9.Location = New System.Drawing.Point(125, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(66, 17)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "GRS WT"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label8.Location = New System.Drawing.Point(66, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(58, 17)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "PIECES"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtDiaPcs
        '
        Me.txtDiaPcs.Enabled = False
        Me.txtDiaPcs.Location = New System.Drawing.Point(66, 38)
        Me.txtDiaPcs.Multiline = True
        Me.txtDiaPcs.Name = "txtDiaPcs"
        Me.txtDiaPcs.Size = New System.Drawing.Size(58, 18)
        Me.txtDiaPcs.TabIndex = 5
        '
        'txtPreWt
        '
        Me.txtPreWt.Enabled = False
        Me.txtPreWt.Location = New System.Drawing.Point(125, 57)
        Me.txtPreWt.Multiline = True
        Me.txtPreWt.Name = "txtPreWt"
        Me.txtPreWt.Size = New System.Drawing.Size(66, 18)
        Me.txtPreWt.TabIndex = 5
        '
        'txtPrePcs
        '
        Me.txtPrePcs.Enabled = False
        Me.txtPrePcs.Location = New System.Drawing.Point(66, 57)
        Me.txtPrePcs.Multiline = True
        Me.txtPrePcs.Name = "txtPrePcs"
        Me.txtPrePcs.Size = New System.Drawing.Size(58, 18)
        Me.txtPrePcs.TabIndex = 5
        '
        'txtDiaWt
        '
        Me.txtDiaWt.Enabled = False
        Me.txtDiaWt.Location = New System.Drawing.Point(125, 38)
        Me.txtDiaWt.Multiline = True
        Me.txtDiaWt.Name = "txtDiaWt"
        Me.txtDiaWt.Size = New System.Drawing.Size(66, 18)
        Me.txtDiaWt.TabIndex = 5
        '
        'Label11
        '
        Me.Label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label11.Location = New System.Drawing.Point(2, 38)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(61, 17)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "Diamond"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtStWt
        '
        Me.txtStWt.Enabled = False
        Me.txtStWt.Location = New System.Drawing.Point(125, 19)
        Me.txtStWt.Multiline = True
        Me.txtStWt.Name = "txtStWt"
        Me.txtStWt.Size = New System.Drawing.Size(66, 18)
        Me.txtStWt.TabIndex = 5
        '
        'Label12
        '
        Me.Label12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label12.Location = New System.Drawing.Point(2, 57)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(61, 17)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "Precious"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gridStone
        '
        Me.gridStone.AllowUserToAddRows = False
        Me.gridStone.AllowUserToDeleteRows = False
        Me.gridStone.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridStone.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridStone.DefaultCellStyle = DataGridViewCellStyle2
        Me.gridStone.Location = New System.Drawing.Point(0, 54)
        Me.gridStone.MultiSelect = False
        Me.gridStone.Name = "gridStone"
        Me.gridStone.ReadOnly = True
        Me.gridStone.RowHeadersVisible = False
        Me.gridStone.RowTemplate.Height = 17
        Me.gridStone.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridStone.Size = New System.Drawing.Size(392, 84)
        Me.gridStone.TabIndex = 1
        '
        'lstTotal
        '
        Me.lstTotal.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.lstTotal.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lstTotal.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstTotal.GridLines = True
        Me.lstTotal.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lstTotal.HideSelection = False
        Me.lstTotal.Location = New System.Drawing.Point(1, 0)
        Me.lstTotal.MultiSelect = False
        Me.lstTotal.Name = "lstTotal"
        Me.lstTotal.Size = New System.Drawing.Size(592, 51)
        Me.lstTotal.TabIndex = 0
        Me.lstTotal.UseCompatibleStateImageBehavior = False
        Me.lstTotal.View = System.Windows.Forms.View.Details
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
        '
        'Timer1
        '
        '
        'frmStockCheckWithRFID
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1011, 526)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmStockCheckWithRFID"
        Me.Text = "StockCheckLoading"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlMain.ResumeLayout(False)
        Me.pnlgrid.ResumeLayout(False)
        CType(Me.gridFullView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridLastView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        CType(Me.picTagImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlStoneTotal.ResumeLayout(False)
        Me.pnlStoneTotal.PerformLayout()
        CType(Me.gridStone, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlMain As System.Windows.Forms.Panel
    Friend WithEvents chkAsonDate As System.Windows.Forms.CheckBox
    Friend WithEvents cmbDesignerName As System.Windows.Forms.ComboBox
    Friend WithEvents cmbItemName As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents rbtMarked As System.Windows.Forms.RadioButton
    Friend WithEvents rbtPending As System.Windows.Forms.RadioButton
    Friend WithEvents rbtAll As System.Windows.Forms.RadioButton
    Friend WithEvents chkWithApproval As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtTagNo As System.Windows.Forms.TextBox
    Friend WithEvents txtTrayNo As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents pnlgrid As System.Windows.Forms.Panel
    Friend WithEvents gridFullView As System.Windows.Forms.DataGridView
    Friend WithEvents gridLastView As System.Windows.Forms.DataGridView
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents lstTotal As System.Windows.Forms.ListView
    Friend WithEvents gridStone As System.Windows.Forms.DataGridView
    Friend WithEvents pnlStoneTotal As System.Windows.Forms.Panel
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtStPcs As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtDiaPcs As System.Windows.Forms.TextBox
    Friend WithEvents txtPreWt As System.Windows.Forms.TextBox
    Friend WithEvents txtPrePcs As System.Windows.Forms.TextBox
    Friend WithEvents txtDiaWt As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtStWt As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents picTagImage As System.Windows.Forms.PictureBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents dtpAsOnDate As BrighttechPack.DatePicker
    Friend WithEvents cmbCostCentre_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents cmbSubItem As System.Windows.Forms.ComboBox
    Friend WithEvents btnImportdata As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents CmbCompany As System.Windows.Forms.ComboBox
    Friend WithEvents chkCmbCounter As BrighttechPack.CheckedComboBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents CmbMetal As System.Windows.Forms.ComboBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents cmbItemType As System.Windows.Forms.ComboBox
    Friend WithEvents cmbRange As System.Windows.Forms.ComboBox
    Friend WithEvents btnScan As Button
    Friend WithEvents btnConnect As Button
    Friend WithEvents lblCount As Label
    Friend WithEvents Timer1 As Timer
End Class
