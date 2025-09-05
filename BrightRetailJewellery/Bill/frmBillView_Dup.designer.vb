<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBillView_Dup
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.pnlGrid = New System.Windows.Forms.Panel
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.gridFullView = New System.Windows.Forms.DataGridView
        Me.gridHead = New System.Windows.Forms.DataGridView
        Me.gridTot = New System.Windows.Forms.DataGridView
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.gridMiscDetails = New System.Windows.Forms.DataGridView
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.txtAddress_OWN = New System.Windows.Forms.TextBox
        Me.Panel6 = New System.Windows.Forms.Panel
        Me.Label13 = New System.Windows.Forms.Label
        Me.lblPackMaterial = New System.Windows.Forms.Label
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.lblDuplicateBill = New System.Windows.Forms.Label
        Me.lblAddreessEdit = New System.Windows.Forms.Label
        Me.lblEditPayment = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
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
        Me.cmbEntryType = New System.Windows.Forms.ComboBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.cmbCostCentre = New System.Windows.Forms.ComboBox
        Me.cmbMetalName = New System.Windows.Forms.ComboBox
        Me.txtItemId = New System.Windows.Forms.TextBox
        Me.txtBillNo = New System.Windows.Forms.TextBox
        Me.txtSystemId = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
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
        Me.btnView_Search.Location = New System.Drawing.Point(0, 360)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(97, 30)
        Me.btnView_Search.TabIndex = 22
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
        Me.pnlGrid.Size = New System.Drawing.Size(741, 528)
        Me.pnlGrid.TabIndex = 1
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.gridFullView)
        Me.Panel3.Controls.Add(Me.gridTot)
        Me.Panel3.Controls.Add(Me.gridHead)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(737, 250)
        Me.Panel3.TabIndex = 2
        '
        'gridFullView
        '
        Me.gridFullView.AllowUserToAddRows = False
        Me.gridFullView.AllowUserToDeleteRows = False
        Me.gridFullView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridFullView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.NullValue = Nothing
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.gridFullView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.gridFullView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.NullValue = Nothing
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridFullView.DefaultCellStyle = DataGridViewCellStyle2
        Me.gridFullView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridFullView.Location = New System.Drawing.Point(0, 19)
        Me.gridFullView.MultiSelect = False
        Me.gridFullView.Name = "gridFullView"
        Me.gridFullView.ReadOnly = True
        Me.gridFullView.RowHeadersVisible = False
        Me.gridFullView.RowTemplate.Height = 20
        Me.gridFullView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridFullView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridFullView.Size = New System.Drawing.Size(737, 231)
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
        Me.gridHead.Size = New System.Drawing.Size(737, 19)
        Me.gridHead.StandardTab = True
        Me.gridHead.TabIndex = 0
        '
        'gridTot
        '
        Me.gridTot.AllowUserToAddRows = False
        Me.gridTot.AllowUserToDeleteRows = False
        Me.gridTot.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.gridTot.BackgroundColor = System.Drawing.SystemColors.ControlDark
        Me.gridTot.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridTot.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridTot.ColumnHeadersVisible = False
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.LightBlue
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.HotTrack
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.LightBlue
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HotTrack
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridTot.DefaultCellStyle = DataGridViewCellStyle3
        Me.gridTot.Location = New System.Drawing.Point(464, 163)
        Me.gridTot.MultiSelect = False
        Me.gridTot.Name = "gridTot"
        Me.gridTot.ReadOnly = True
        Me.gridTot.RowHeadersVisible = False
        Me.gridTot.RowTemplate.Height = 18
        Me.gridTot.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridTot.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.gridTot.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridTot.Size = New System.Drawing.Size(103, 87)
        Me.gridTot.TabIndex = 0
        Me.gridTot.Visible = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.gridMiscDetails)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 250)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(737, 274)
        Me.Panel1.TabIndex = 1
        '
        'gridMiscDetails
        '
        Me.gridMiscDetails.AllowUserToAddRows = False
        Me.gridMiscDetails.AllowUserToDeleteRows = False
        Me.gridMiscDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.gridMiscDetails.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridMiscDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridMiscDetails.ColumnHeadersVisible = False
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridMiscDetails.DefaultCellStyle = DataGridViewCellStyle4
        Me.gridMiscDetails.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridMiscDetails.Location = New System.Drawing.Point(0, 0)
        Me.gridMiscDetails.MultiSelect = False
        Me.gridMiscDetails.Name = "gridMiscDetails"
        Me.gridMiscDetails.ReadOnly = True
        Me.gridMiscDetails.RowHeadersVisible = False
        Me.gridMiscDetails.RowTemplate.Height = 18
        Me.gridMiscDetails.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridMiscDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridMiscDetails.Size = New System.Drawing.Size(567, 274)
        Me.gridMiscDetails.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel2.Controls.Add(Me.txtAddress_OWN)
        Me.Panel2.Controls.Add(Me.Panel6)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel2.Location = New System.Drawing.Point(567, 0)
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
        Me.txtAddress_OWN.Size = New System.Drawing.Size(166, 144)
        Me.txtAddress_OWN.TabIndex = 0
        '
        'Panel6
        '
        Me.Panel6.BackColor = System.Drawing.SystemColors.Window
        Me.Panel6.Controls.Add(Me.Label13)
        Me.Panel6.Controls.Add(Me.lblPackMaterial)
        Me.Panel6.Controls.Add(Me.TextBox2)
        Me.Panel6.Controls.Add(Me.TextBox1)
        Me.Panel6.Controls.Add(Me.Label3)
        Me.Panel6.Controls.Add(Me.lblDuplicateBill)
        Me.Panel6.Controls.Add(Me.lblAddreessEdit)
        Me.Panel6.Controls.Add(Me.lblEditPayment)
        Me.Panel6.Controls.Add(Me.Label1)
        Me.Panel6.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel6.Location = New System.Drawing.Point(0, 144)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(166, 126)
        Me.Panel6.TabIndex = 1
        '
        'Label13
        '
        Me.Label13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(42, 72)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(121, 15)
        Me.Label13.TabIndex = 4
        Me.Label13.Text = "[O] Others"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblPackMaterial
        '
        Me.lblPackMaterial.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPackMaterial.Location = New System.Drawing.Point(42, 38)
        Me.lblPackMaterial.Name = "lblPackMaterial"
        Me.lblPackMaterial.Size = New System.Drawing.Size(121, 15)
        Me.lblPackMaterial.TabIndex = 3
        Me.lblPackMaterial.Text = "[M] Pack Material"
        Me.lblPackMaterial.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextBox2
        '
        Me.TextBox2.BackColor = System.Drawing.Color.Red
        Me.TextBox2.Enabled = False
        Me.TextBox2.Location = New System.Drawing.Point(6, 89)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(30, 15)
        Me.TextBox2.TabIndex = 2
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.Color.LightGreen
        Me.TextBox1.Enabled = False
        Me.TextBox1.Location = New System.Drawing.Point(6, 107)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(30, 15)
        Me.TextBox1.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(42, 105)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(121, 15)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Partly Sale"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblDuplicateBill
        '
        Me.lblDuplicateBill.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDuplicateBill.Location = New System.Drawing.Point(42, 55)
        Me.lblDuplicateBill.Name = "lblDuplicateBill"
        Me.lblDuplicateBill.Size = New System.Drawing.Size(121, 15)
        Me.lblDuplicateBill.TabIndex = 0
        Me.lblDuplicateBill.Text = "[D] Duplicate Bill"
        Me.lblDuplicateBill.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblAddreessEdit
        '
        Me.lblAddreessEdit.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddreessEdit.Location = New System.Drawing.Point(42, 21)
        Me.lblAddreessEdit.Name = "lblAddreessEdit"
        Me.lblAddreessEdit.Size = New System.Drawing.Size(121, 15)
        Me.lblAddreessEdit.TabIndex = 0
        Me.lblAddreessEdit.Text = "[A] Edit AddressInfo"
        Me.lblAddreessEdit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblEditPayment
        '
        Me.lblEditPayment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEditPayment.Location = New System.Drawing.Point(42, 4)
        Me.lblEditPayment.Name = "lblEditPayment"
        Me.lblEditPayment.Size = New System.Drawing.Size(121, 15)
        Me.lblEditPayment.TabIndex = 0
        Me.lblEditPayment.Text = "[E] Edit Payment"
        Me.lblEditPayment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(42, 88)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(121, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "[C] Cancel"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 26)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.AccessibleDescription = ""
        Me.ExitToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1})
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.AccessibleDescription = "~AUT"
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(183, 22)
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
        Me.Panel7.Controls.Add(Me.cmbEntryType)
        Me.Panel7.Controls.Add(Me.btnExit)
        Me.Panel7.Controls.Add(Me.cmbCostCentre)
        Me.Panel7.Controls.Add(Me.cmbMetalName)
        Me.Panel7.Controls.Add(Me.txtItemId)
        Me.Panel7.Controls.Add(Me.txtBillNo)
        Me.Panel7.Controls.Add(Me.txtSystemId)
        Me.Panel7.Controls.Add(Me.Label10)
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
        Me.Panel7.Size = New System.Drawing.Size(193, 528)
        Me.Panel7.TabIndex = 0
        '
        'cmbUserName
        '
        Me.cmbUserName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cmbUserName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbUserName.FormattingEnabled = True
        Me.cmbUserName.Location = New System.Drawing.Point(8, 312)
        Me.cmbUserName.Name = "cmbUserName"
        Me.cmbUserName.Size = New System.Drawing.Size(178, 21)
        Me.cmbUserName.TabIndex = 20
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.Color.Transparent
        Me.Label12.Location = New System.Drawing.Point(8, 296)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(66, 13)
        Me.Label12.TabIndex = 19
        Me.Label12.Text = "UserName"
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(193, 13)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "TITLE"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblTitle.Visible = False
        '
        'BtnAdvanced
        '
        Me.BtnAdvanced.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.BtnAdvanced.Location = New System.Drawing.Point(48, 336)
        Me.BtnAdvanced.Name = "BtnAdvanced"
        Me.BtnAdvanced.Size = New System.Drawing.Size(97, 23)
        Me.BtnAdvanced.TabIndex = 21
        Me.BtnAdvanced.Text = "Advanced"
        Me.BtnAdvanced.UseVisualStyleBackColor = True
        '
        'cmbCashCounter
        '
        Me.cmbCashCounter.FormattingEnabled = True
        Me.cmbCashCounter.Location = New System.Drawing.Point(8, 272)
        Me.cmbCashCounter.Name = "cmbCashCounter"
        Me.cmbCashCounter.Size = New System.Drawing.Size(178, 21)
        Me.cmbCashCounter.TabIndex = 18
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(8, 255)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(86, 13)
        Me.Label11.TabIndex = 17
        Me.Label11.Text = "Cash Counter"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtCustName
        '
        Me.txtCustName.Location = New System.Drawing.Point(8, 230)
        Me.txtCustName.Name = "txtCustName"
        Me.txtCustName.Size = New System.Drawing.Size(177, 21)
        Me.txtCustName.TabIndex = 16
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 214)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 13)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "Customer Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(11, 393)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(171, 127)
        Me.PictureBox1.TabIndex = 18
        Me.PictureBox1.TabStop = False
        '
        'dtpBillDate
        '
        Me.dtpBillDate.Location = New System.Drawing.Point(83, 15)
        Me.dtpBillDate.Mask = "##/##/####"
        Me.dtpBillDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpBillDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpBillDate.Name = "dtpBillDate"
        Me.dtpBillDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpBillDate.Size = New System.Drawing.Size(103, 21)
        Me.dtpBillDate.TabIndex = 2
        Me.dtpBillDate.Text = "06/03/9998"
        Me.dtpBillDate.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'cmbEntryType
        '
        Me.cmbEntryType.FormattingEnabled = True
        Me.cmbEntryType.Items.AddRange(New Object() {"ALL", "SALES", "PURCHASE", "SALES & PURCHASE", "PAYMENTS", "RECEIPTS", "MISC ISSUE", "SALES RETURN", "APPROVAL ISSUE", "APPROVAL RECEIPT", "ORDER DELIVERY", "ORDER BOOKING", "REPAIR DELIVERY", "REPAIR BOOKING", "GIFT VOUCHER"})
        Me.cmbEntryType.Location = New System.Drawing.Point(8, 189)
        Me.cmbEntryType.Name = "cmbEntryType"
        Me.cmbEntryType.Size = New System.Drawing.Size(178, 21)
        Me.cmbEntryType.TabIndex = 14
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(95, 360)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(97, 30)
        Me.btnExit.TabIndex = 25
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'cmbCostCentre
        '
        Me.cmbCostCentre.FormattingEnabled = True
        Me.cmbCostCentre.Location = New System.Drawing.Point(8, 144)
        Me.cmbCostCentre.Name = "cmbCostCentre"
        Me.cmbCostCentre.Size = New System.Drawing.Size(178, 21)
        Me.cmbCostCentre.TabIndex = 12
        '
        'cmbMetalName
        '
        Me.cmbMetalName.FormattingEnabled = True
        Me.cmbMetalName.Location = New System.Drawing.Point(83, 105)
        Me.cmbMetalName.Name = "cmbMetalName"
        Me.cmbMetalName.Size = New System.Drawing.Size(103, 21)
        Me.cmbMetalName.TabIndex = 10
        '
        'txtItemId
        '
        Me.txtItemId.Location = New System.Drawing.Point(83, 82)
        Me.txtItemId.Name = "txtItemId"
        Me.txtItemId.Size = New System.Drawing.Size(103, 21)
        Me.txtItemId.TabIndex = 8
        '
        'txtBillNo
        '
        Me.txtBillNo.Location = New System.Drawing.Point(83, 59)
        Me.txtBillNo.Name = "txtBillNo"
        Me.txtBillNo.Size = New System.Drawing.Size(103, 21)
        Me.txtBillNo.TabIndex = 6
        '
        'txtSystemId
        '
        Me.txtSystemId.Location = New System.Drawing.Point(83, 37)
        Me.txtSystemId.Name = "txtSystemId"
        Me.txtSystemId.Size = New System.Drawing.Size(103, 21)
        Me.txtSystemId.TabIndex = 4
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(8, 171)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(69, 13)
        Me.Label10.TabIndex = 13
        Me.Label10.Text = "Entry Type"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(8, 128)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(76, 13)
        Me.Label9.TabIndex = 11
        Me.Label9.Text = "Cost Centre"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(8, 108)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(37, 13)
        Me.Label8.TabIndex = 9
        Me.Label8.Text = "Metal"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(8, 86)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(50, 13)
        Me.Label6.TabIndex = 7
        Me.Label6.Text = "Item Id"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(8, 63)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(43, 13)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "Bill No"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(8, 19)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(55, 13)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "Bill Date"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(8, 41)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(62, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "SystemId"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'frmBillView_Dup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(934, 528)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.pnlGrid)
        Me.Controls.Add(Me.Panel7)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.KeyPreview = True
        Me.Name = "frmBillView_Dup"
        Me.Text = "BillView"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlGrid.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        CType(Me.gridFullView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridHead, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridTot, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        CType(Me.gridMiscDetails, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel6.ResumeLayout(False)
        Me.Panel6.PerformLayout()
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
    Friend WithEvents txtAddress_OWN As System.Windows.Forms.TextBox
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
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
    Friend WithEvents cmbEntryType As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents lblDuplicateBill As System.Windows.Forms.Label
    Friend WithEvents lblAddreessEdit As System.Windows.Forms.Label
    Friend WithEvents dtpBillDate As BrighttechPack.DatePicker
    Friend WithEvents lblPackMaterial As System.Windows.Forms.Label
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
    Friend WithEvents Label13 As System.Windows.Forms.Label
End Class
