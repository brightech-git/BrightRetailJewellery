<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBillToJnd
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
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.pnlGrid = New System.Windows.Forms.Panel
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.gridFullView = New System.Windows.Forms.DataGridView
        Me.gridHead = New System.Windows.Forms.DataGridView
        Me.pnlTotGrid = New System.Windows.Forms.Panel
        Me.gridTot = New System.Windows.Forms.DataGridView
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.gridMiscDetails = New System.Windows.Forms.DataGridView
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.txtAddress_OWN = New System.Windows.Forms.TextBox
        Me.Panel6 = New System.Windows.Forms.Panel
        Me.lblEditPayment = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.Panel7 = New System.Windows.Forms.Panel
        Me.cmbUserName = New System.Windows.Forms.ComboBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.lblTitle = New System.Windows.Forms.Label
        Me.BtnAdvanced = New System.Windows.Forms.Button
        Me.cmbCashCounter = New System.Windows.Forms.ComboBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.txtCustName = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.dtpBillDate = New BrighttechPack.DatePicker(Me.components)
        Me.btnExit = New System.Windows.Forms.Button
        Me.cmbCostCentre = New System.Windows.Forms.ComboBox
        Me.cmbMetalName = New System.Windows.Forms.ComboBox
        Me.txtItemId = New System.Windows.Forms.TextBox
        Me.txtBillNo = New System.Windows.Forms.TextBox
        Me.txtSystemId = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.pnlGrid.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.gridFullView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridHead, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlTotGrid.SuspendLayout()
        CType(Me.gridTot, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.gridMiscDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel7.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnView_Search
        '
        Me.btnView_Search.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnView_Search.Location = New System.Drawing.Point(0, 372)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(97, 30)
        Me.btnView_Search.TabIndex = 19
        Me.btnView_Search.Text = "&Search"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'pnlGrid
        '
        Me.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlGrid.Controls.Add(Me.Panel3)
        Me.pnlGrid.Controls.Add(Me.Panel1)
        Me.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlGrid.Location = New System.Drawing.Point(193, 0)
        Me.pnlGrid.Name = "pnlGrid"
        Me.pnlGrid.Size = New System.Drawing.Size(835, 626)
        Me.pnlGrid.TabIndex = 1
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.gridFullView)
        Me.Panel3.Controls.Add(Me.gridHead)
        Me.Panel3.Controls.Add(Me.pnlTotGrid)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(831, 348)
        Me.Panel3.TabIndex = 2
        '
        'gridFullView
        '
        Me.gridFullView.AllowUserToAddRows = False
        Me.gridFullView.AllowUserToDeleteRows = False
        Me.gridFullView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridFullView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle13.NullValue = Nothing
        DataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.gridFullView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle13
        Me.gridFullView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle14.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle14.NullValue = Nothing
        DataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridFullView.DefaultCellStyle = DataGridViewCellStyle14
        Me.gridFullView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridFullView.Location = New System.Drawing.Point(0, 19)
        Me.gridFullView.MultiSelect = False
        Me.gridFullView.Name = "gridFullView"
        Me.gridFullView.ReadOnly = True
        Me.gridFullView.RowHeadersVisible = False
        Me.gridFullView.RowTemplate.Height = 20
        Me.gridFullView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridFullView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridFullView.Size = New System.Drawing.Size(831, 223)
        Me.gridFullView.TabIndex = 1
        '
        'gridHead
        '
        Me.gridHead.AllowUserToAddRows = False
        Me.gridHead.AllowUserToDeleteRows = False
        Me.gridHead.AllowUserToResizeColumns = False
        Me.gridHead.AllowUserToResizeRows = False
        Me.gridHead.BackgroundColor = System.Drawing.SystemColors.ButtonShadow
        Me.gridHead.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridHead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridHead.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridHead.Location = New System.Drawing.Point(0, 0)
        Me.gridHead.MultiSelect = False
        Me.gridHead.Name = "gridHead"
        Me.gridHead.ReadOnly = True
        Me.gridHead.RowHeadersVisible = False
        Me.gridHead.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridHead.Size = New System.Drawing.Size(831, 19)
        Me.gridHead.StandardTab = True
        Me.gridHead.TabIndex = 4
        '
        'pnlTotGrid
        '
        Me.pnlTotGrid.Controls.Add(Me.gridTot)
        Me.pnlTotGrid.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlTotGrid.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlTotGrid.Location = New System.Drawing.Point(0, 242)
        Me.pnlTotGrid.Name = "pnlTotGrid"
        Me.pnlTotGrid.Size = New System.Drawing.Size(831, 106)
        Me.pnlTotGrid.TabIndex = 2
        Me.pnlTotGrid.Visible = False
        '
        'gridTot
        '
        Me.gridTot.AllowUserToAddRows = False
        Me.gridTot.AllowUserToDeleteRows = False
        Me.gridTot.BackgroundColor = System.Drawing.SystemColors.ControlDark
        Me.gridTot.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridTot.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridTot.ColumnHeadersVisible = False
        DataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle15.BackColor = System.Drawing.Color.LightBlue
        DataGridViewCellStyle15.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.HotTrack
        DataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.LightBlue
        DataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HotTrack
        DataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridTot.DefaultCellStyle = DataGridViewCellStyle15
        Me.gridTot.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridTot.Location = New System.Drawing.Point(0, 0)
        Me.gridTot.MultiSelect = False
        Me.gridTot.Name = "gridTot"
        Me.gridTot.ReadOnly = True
        Me.gridTot.RowHeadersVisible = False
        Me.gridTot.RowTemplate.Height = 18
        Me.gridTot.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridTot.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.gridTot.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridTot.Size = New System.Drawing.Size(831, 106)
        Me.gridTot.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.gridMiscDetails)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 348)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(831, 274)
        Me.Panel1.TabIndex = 1
        '
        'gridMiscDetails
        '
        Me.gridMiscDetails.AllowUserToAddRows = False
        Me.gridMiscDetails.AllowUserToDeleteRows = False
        Me.gridMiscDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridMiscDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridMiscDetails.ColumnHeadersVisible = False
        DataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle16.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridMiscDetails.DefaultCellStyle = DataGridViewCellStyle16
        Me.gridMiscDetails.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridMiscDetails.Location = New System.Drawing.Point(0, 0)
        Me.gridMiscDetails.MultiSelect = False
        Me.gridMiscDetails.Name = "gridMiscDetails"
        Me.gridMiscDetails.ReadOnly = True
        Me.gridMiscDetails.RowHeadersVisible = False
        Me.gridMiscDetails.RowTemplate.Height = 18
        Me.gridMiscDetails.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridMiscDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridMiscDetails.Size = New System.Drawing.Size(661, 274)
        Me.gridMiscDetails.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel2.Controls.Add(Me.txtAddress_OWN)
        Me.Panel2.Controls.Add(Me.Panel6)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel2.Location = New System.Drawing.Point(661, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(170, 274)
        Me.Panel2.TabIndex = 1
        '
        'txtAddress_OWN
        '
        Me.txtAddress_OWN.BackColor = System.Drawing.SystemColors.Window
        Me.txtAddress_OWN.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtAddress_OWN.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtAddress_OWN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress_OWN.ForeColor = System.Drawing.Color.Blue
        Me.txtAddress_OWN.Location = New System.Drawing.Point(0, 0)
        Me.txtAddress_OWN.Multiline = True
        Me.txtAddress_OWN.Name = "txtAddress_OWN"
        Me.txtAddress_OWN.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtAddress_OWN.Size = New System.Drawing.Size(166, 211)
        Me.txtAddress_OWN.TabIndex = 0
        '
        'Panel6
        '
        Me.Panel6.BackColor = System.Drawing.SystemColors.Window
        Me.Panel6.Controls.Add(Me.lblEditPayment)
        Me.Panel6.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel6.Location = New System.Drawing.Point(0, 211)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(166, 59)
        Me.Panel6.TabIndex = 1
        '
        'lblEditPayment
        '
        Me.lblEditPayment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEditPayment.Location = New System.Drawing.Point(4, 3)
        Me.lblEditPayment.Name = "lblEditPayment"
        Me.lblEditPayment.Size = New System.Drawing.Size(160, 15)
        Me.lblEditPayment.TabIndex = 0
        Me.lblEditPayment.Text = "[Space] Change Tobe"
        Me.lblEditPayment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 26)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.AccessibleDescription = ""
        Me.ExitToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1})
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.AccessibleDescription = "~AUT"
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(181, 22)
        Me.ToolStripMenuItem1.Text = "ToolStripMenuItem1"
        '
        'Panel7
        '
        Me.Panel7.Controls.Add(Me.cmbUserName)
        Me.Panel7.Controls.Add(Me.Label12)
        Me.Panel7.Controls.Add(Me.lblTitle)
        Me.Panel7.Controls.Add(Me.BtnAdvanced)
        Me.Panel7.Controls.Add(Me.cmbCashCounter)
        Me.Panel7.Controls.Add(Me.Label11)
        Me.Panel7.Controls.Add(Me.txtCustName)
        Me.Panel7.Controls.Add(Me.Label2)
        Me.Panel7.Controls.Add(Me.PictureBox1)
        Me.Panel7.Controls.Add(Me.dtpBillDate)
        Me.Panel7.Controls.Add(Me.btnExit)
        Me.Panel7.Controls.Add(Me.cmbCostCentre)
        Me.Panel7.Controls.Add(Me.cmbMetalName)
        Me.Panel7.Controls.Add(Me.txtItemId)
        Me.Panel7.Controls.Add(Me.txtBillNo)
        Me.Panel7.Controls.Add(Me.txtSystemId)
        Me.Panel7.Controls.Add(Me.Label9)
        Me.Panel7.Controls.Add(Me.Label8)
        Me.Panel7.Controls.Add(Me.btnView_Search)
        Me.Panel7.Controls.Add(Me.Label6)
        Me.Panel7.Controls.Add(Me.Label5)
        Me.Panel7.Controls.Add(Me.Label7)
        Me.Panel7.Controls.Add(Me.Label4)
        Me.Panel7.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel7.Location = New System.Drawing.Point(0, 0)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(193, 626)
        Me.Panel7.TabIndex = 0
        '
        'cmbUserName
        '
        Me.cmbUserName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbUserName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbUserName.FormattingEnabled = True
        Me.cmbUserName.Location = New System.Drawing.Point(8, 316)
        Me.cmbUserName.Name = "cmbUserName"
        Me.cmbUserName.Size = New System.Drawing.Size(178, 21)
        Me.cmbUserName.TabIndex = 27
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.Color.Transparent
        Me.Label12.Location = New System.Drawing.Point(9, 300)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(110, 13)
        Me.Label12.TabIndex = 26
        Me.Label12.Text = "UserName"
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(193, 13)
        Me.lblTitle.TabIndex = 24
        Me.lblTitle.Text = "TITLE"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblTitle.Visible = False
        '
        'BtnAdvanced
        '
        Me.BtnAdvanced.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.BtnAdvanced.Location = New System.Drawing.Point(50, 343)
        Me.BtnAdvanced.Name = "BtnAdvanced"
        Me.BtnAdvanced.Size = New System.Drawing.Size(97, 23)
        Me.BtnAdvanced.TabIndex = 25
        Me.BtnAdvanced.Text = "Advanced"
        Me.BtnAdvanced.UseVisualStyleBackColor = True
        '
        'cmbCashCounter
        '
        Me.cmbCashCounter.FormattingEnabled = True
        Me.cmbCashCounter.Location = New System.Drawing.Point(8, 272)
        Me.cmbCashCounter.Name = "cmbCashCounter"
        Me.cmbCashCounter.Size = New System.Drawing.Size(178, 21)
        Me.cmbCashCounter.TabIndex = 17
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(8, 256)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(86, 13)
        Me.Label11.TabIndex = 16
        Me.Label11.Text = "Cash Counter"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtCustName
        '
        Me.txtCustName.Location = New System.Drawing.Point(9, 232)
        Me.txtCustName.Name = "txtCustName"
        Me.txtCustName.Size = New System.Drawing.Size(177, 21)
        Me.txtCustName.TabIndex = 15
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 216)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 13)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Customer Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(12, 408)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(159, 181)
        Me.PictureBox1.TabIndex = 18
        Me.PictureBox1.TabStop = False
        '
        'dtpBillDate
        '
        Me.dtpBillDate.Location = New System.Drawing.Point(83, 16)
        Me.dtpBillDate.Mask = "##/##/####"
        Me.dtpBillDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpBillDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpBillDate.Name = "dtpBillDate"
        Me.dtpBillDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpBillDate.Size = New System.Drawing.Size(103, 21)
        Me.dtpBillDate.TabIndex = 1
        Me.dtpBillDate.Text = "06/03/9998"
        Me.dtpBillDate.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(96, 372)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(97, 30)
        Me.btnExit.TabIndex = 22
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'cmbCostCentre
        '
        Me.cmbCostCentre.FormattingEnabled = True
        Me.cmbCostCentre.Location = New System.Drawing.Point(9, 182)
        Me.cmbCostCentre.Name = "cmbCostCentre"
        Me.cmbCostCentre.Size = New System.Drawing.Size(178, 21)
        Me.cmbCostCentre.TabIndex = 11
        '
        'cmbMetalName
        '
        Me.cmbMetalName.FormattingEnabled = True
        Me.cmbMetalName.Location = New System.Drawing.Point(9, 142)
        Me.cmbMetalName.Name = "cmbMetalName"
        Me.cmbMetalName.Size = New System.Drawing.Size(177, 21)
        Me.cmbMetalName.TabIndex = 9
        '
        'txtItemId
        '
        Me.txtItemId.Location = New System.Drawing.Point(83, 102)
        Me.txtItemId.Name = "txtItemId"
        Me.txtItemId.Size = New System.Drawing.Size(103, 21)
        Me.txtItemId.TabIndex = 7
        '
        'txtBillNo
        '
        Me.txtBillNo.Location = New System.Drawing.Point(83, 72)
        Me.txtBillNo.Name = "txtBillNo"
        Me.txtBillNo.Size = New System.Drawing.Size(103, 21)
        Me.txtBillNo.TabIndex = 5
        '
        'txtSystemId
        '
        Me.txtSystemId.Location = New System.Drawing.Point(83, 43)
        Me.txtSystemId.Name = "txtSystemId"
        Me.txtSystemId.Size = New System.Drawing.Size(103, 21)
        Me.txtSystemId.TabIndex = 3
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(9, 166)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(76, 13)
        Me.Label9.TabIndex = 10
        Me.Label9.Text = "Cost Centre"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(9, 126)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(37, 13)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "Metal"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(9, 105)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(50, 13)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Item Id"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(9, 78)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(43, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Bill No"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(9, 19)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(55, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Bill Date"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(9, 50)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(62, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "SystemId"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'frmBillToJnt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 626)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.pnlGrid)
        Me.Controls.Add(Me.Panel7)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmBillToJnt"
        Me.Text = "BillToJnt"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlGrid.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        CType(Me.gridFullView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridHead, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTotGrid.ResumeLayout(False)
        CType(Me.gridTot, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        CType(Me.gridMiscDetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel6.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel7.ResumeLayout(False)
        Me.Panel7.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents pnlGrid As System.Windows.Forms.Panel
    Friend WithEvents gridMiscDetails As System.Windows.Forms.DataGridView
    Friend WithEvents gridFullView As System.Windows.Forms.DataGridView
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents pnlTotGrid As System.Windows.Forms.Panel
    Friend WithEvents txtAddress_OWN As System.Windows.Forms.TextBox
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Friend WithEvents Panel7 As System.Windows.Forms.Panel
    Friend WithEvents cmbMetalName As System.Windows.Forms.ComboBox
    Friend WithEvents txtItemId As System.Windows.Forms.TextBox
    Friend WithEvents txtBillNo As System.Windows.Forms.TextBox
    Friend WithEvents txtSystemId As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmbCostCentre As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents gridTot As System.Windows.Forms.DataGridView
    Friend WithEvents gridHead As System.Windows.Forms.DataGridView
    Friend WithEvents lblEditPayment As System.Windows.Forms.Label
    Friend WithEvents dtpBillDate As BrighttechPack.DatePicker
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents txtCustName As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbCashCounter As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents BtnAdvanced As System.Windows.Forms.Button
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbUserName As System.Windows.Forms.ComboBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
End Class
