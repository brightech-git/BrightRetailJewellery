<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmdocumentupdation
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
        Me.btnSave = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.btnNew = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.lblStatus = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.dtpdocdate = New BrighttechPack.DatePicker(Me.components)
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.txtremark = New System.Windows.Forms.TextBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.dtpdoctodate = New BrighttechPack.DatePicker(Me.components)
        Me.Label8 = New System.Windows.Forms.Label
        Me.cmbsentto = New System.Windows.Forms.ComboBox
        Me.btnopen = New System.Windows.Forms.Button
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtdocdes = New System.Windows.Forms.TextBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.cmbsentby = New System.Windows.Forms.ComboBox
        Me.cmbpreperedby = New System.Windows.Forms.ComboBox
        Me.cmbcheckedby = New System.Windows.Forms.ComboBox
        Me.cmbdoctype = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmbtocostid = New System.Windows.Forms.ComboBox
        Me.cmbdocdescription = New System.Windows.Forms.ComboBox
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.gridview = New System.Windows.Forms.DataGridView
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label11 = New System.Windows.Forms.Label
        Me.cmbvsentby = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmbvtocostid = New System.Windows.Forms.ComboBox
        Me.dtpfrmdate = New BrighttechPack.DatePicker(Me.components)
        Me.dtptodate = New BrighttechPack.DatePicker(Me.components)
        Me.btnexport = New System.Windows.Forms.Button
        Me.Label10 = New System.Windows.Forms.Label
        Me.btnback = New System.Windows.Forms.Button
        Me.btnview = New System.Windows.Forms.Button
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.cmbgrid_OWN = New System.Windows.Forms.ComboBox
        Me.gridviewR = New System.Windows.Forms.DataGridView
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.btnRexport = New System.Windows.Forms.Button
        Me.btnRexit = New System.Windows.Forms.Button
        Me.btnRview = New System.Windows.Forms.Button
        Me.Type = New System.Windows.Forms.Label
        Me.cmbRrpttype = New System.Windows.Forms.ComboBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.cmbRreceivedby = New System.Windows.Forms.ComboBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.cmbRfrmcostid = New System.Windows.Forms.ComboBox
        Me.pnldatefilt = New System.Windows.Forms.Panel
        Me.dtpRfrmdate = New BrighttechPack.DatePicker(Me.components)
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.dtpRTodate = New BrighttechPack.DatePicker(Me.components)
        Me.chkwithcancel = New System.Windows.Forms.CheckBox
        Me.ContextMenuStrip1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.gridview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me.gridviewR, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.pnldatefilt.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(4, 320)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 18
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 150)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(78, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Prepared By"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(11, 187)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(76, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Checked By"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(11, 80)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(87, 13)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "To Costcentre"
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(204, 320)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 20
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.OpenToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(131, 92)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        Me.OpenToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(29, 462)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(128, 13)
        Me.lblStatus.TabIndex = 0
        Me.lblStatus.Text = "Press Enter to Edit"
        Me.lblStatus.Visible = False
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(12, 221)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(52, 13)
        Me.Label20.TabIndex = 12
        Me.Label20.Text = "Sent By"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TabControl1
        '
        Me.TabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1020, 630)
        Me.TabControl1.TabIndex = 22
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        Me.TabPage1.Controls.Add(Me.cmbdoctype)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.cmbtocostid)
        Me.TabPage1.Controls.Add(Me.cmbdocdescription)
        Me.TabPage1.Location = New System.Drawing.Point(4, 25)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(1012, 601)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "TabPage1"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.dtpdocdate)
        Me.GroupBox1.Controls.Add(Me.chkCmbCostCentre)
        Me.GroupBox1.Controls.Add(Me.txtremark)
        Me.GroupBox1.Controls.Add(Me.Label17)
        Me.GroupBox1.Controls.Add(Me.Label16)
        Me.GroupBox1.Controls.Add(Me.dtpdoctodate)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.cmbsentto)
        Me.GroupBox1.Controls.Add(Me.btnopen)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.btnNew)
        Me.GroupBox1.Controls.Add(Me.btnSave)
        Me.GroupBox1.Controls.Add(Me.txtdocdes)
        Me.GroupBox1.Controls.Add(Me.btnExit)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label20)
        Me.GroupBox1.Controls.Add(Me.cmbsentby)
        Me.GroupBox1.Controls.Add(Me.cmbpreperedby)
        Me.GroupBox1.Controls.Add(Me.cmbcheckedby)
        Me.GroupBox1.Location = New System.Drawing.Point(297, 147)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(408, 369)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'dtpdocdate
        '
        Me.dtpdocdate.Location = New System.Drawing.Point(114, 42)
        Me.dtpdocdate.Mask = "##-##-####"
        Me.dtpdocdate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpdocdate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpdocdate.Name = "dtpdocdate"
        Me.dtpdocdate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpdocdate.Size = New System.Drawing.Size(104, 21)
        Me.dtpdocdate.TabIndex = 1
        Me.dtpdocdate.Text = "29/09/2010"
        Me.dtpdocdate.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(114, 77)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(279, 22)
        Me.chkCmbCostCentre.TabIndex = 5
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'txtremark
        '
        Me.txtremark.Location = New System.Drawing.Point(114, 289)
        Me.txtremark.Name = "txtremark"
        Me.txtremark.Size = New System.Drawing.Size(279, 21)
        Me.txtremark.TabIndex = 17
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(11, 292)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(52, 13)
        Me.Label17.TabIndex = 16
        Me.Label17.Text = "Remark"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(242, 45)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(21, 13)
        Me.Label16.TabIndex = 2
        Me.Label16.Text = "To"
        '
        'dtpdoctodate
        '
        Me.dtpdoctodate.Location = New System.Drawing.Point(289, 42)
        Me.dtpdoctodate.Mask = "##-##-####"
        Me.dtpdoctodate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpdoctodate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpdoctodate.Name = "dtpdoctodate"
        Me.dtpdoctodate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpdoctodate.Size = New System.Drawing.Size(104, 21)
        Me.dtpdoctodate.TabIndex = 3
        Me.dtpdoctodate.Text = "29/09/2010"
        Me.dtpdoctodate.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(12, 258)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(51, 13)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "Sent To"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbsentto
        '
        Me.cmbsentto.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cmbsentto.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbsentto.FormattingEnabled = True
        Me.cmbsentto.Location = New System.Drawing.Point(114, 256)
        Me.cmbsentto.Name = "cmbsentto"
        Me.cmbsentto.Size = New System.Drawing.Size(279, 21)
        Me.cmbsentto.TabIndex = 15
        '
        'btnopen
        '
        Me.btnopen.Location = New System.Drawing.Point(104, 320)
        Me.btnopen.Name = "btnopen"
        Me.btnopen.Size = New System.Drawing.Size(100, 30)
        Me.btnopen.TabIndex = 19
        Me.btnopen.Text = "Open [F2]"
        Me.btnopen.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(12, 45)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(96, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Document Date"
        '
        'txtdocdes
        '
        Me.txtdocdes.Location = New System.Drawing.Point(114, 112)
        Me.txtdocdes.Name = "txtdocdes"
        Me.txtdocdes.Size = New System.Drawing.Size(279, 21)
        Me.txtdocdes.TabIndex = 7
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(304, 320)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 21
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(11, 112)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(101, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Document Desc."
        '
        'cmbsentby
        '
        Me.cmbsentby.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cmbsentby.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbsentby.FormattingEnabled = True
        Me.cmbsentby.Location = New System.Drawing.Point(114, 221)
        Me.cmbsentby.Name = "cmbsentby"
        Me.cmbsentby.Size = New System.Drawing.Size(279, 21)
        Me.cmbsentby.TabIndex = 13
        '
        'cmbpreperedby
        '
        Me.cmbpreperedby.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cmbpreperedby.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbpreperedby.FormattingEnabled = True
        Me.cmbpreperedby.Location = New System.Drawing.Point(114, 147)
        Me.cmbpreperedby.Name = "cmbpreperedby"
        Me.cmbpreperedby.Size = New System.Drawing.Size(279, 21)
        Me.cmbpreperedby.TabIndex = 9
        '
        'cmbcheckedby
        '
        Me.cmbcheckedby.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cmbcheckedby.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbcheckedby.FormattingEnabled = True
        Me.cmbcheckedby.Location = New System.Drawing.Point(114, 183)
        Me.cmbcheckedby.Name = "cmbcheckedby"
        Me.cmbcheckedby.Size = New System.Drawing.Size(279, 21)
        Me.cmbcheckedby.TabIndex = 11
        '
        'cmbdoctype
        '
        Me.cmbdoctype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbdoctype.FormattingEnabled = True
        Me.cmbdoctype.Location = New System.Drawing.Point(124, 137)
        Me.cmbdoctype.Name = "cmbdoctype"
        Me.cmbdoctype.Size = New System.Drawing.Size(101, 21)
        Me.cmbdoctype.TabIndex = 3
        Me.cmbdoctype.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(21, 140)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(97, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Document Type"
        Me.Label1.Visible = False
        '
        'cmbtocostid
        '
        Me.cmbtocostid.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbtocostid.FormattingEnabled = True
        Me.cmbtocostid.Location = New System.Drawing.Point(12, 184)
        Me.cmbtocostid.Name = "cmbtocostid"
        Me.cmbtocostid.Size = New System.Drawing.Size(279, 21)
        Me.cmbtocostid.TabIndex = 1
        Me.cmbtocostid.Visible = False
        '
        'cmbdocdescription
        '
        Me.cmbdocdescription.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbdocdescription.FormattingEnabled = True
        Me.cmbdocdescription.Location = New System.Drawing.Point(12, 259)
        Me.cmbdocdescription.Name = "cmbdocdescription"
        Me.cmbdocdescription.Size = New System.Drawing.Size(279, 21)
        Me.cmbdocdescription.TabIndex = 2
        Me.cmbdocdescription.Visible = False
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.GroupBox2)
        Me.TabPage2.Controls.Add(Me.Panel1)
        Me.TabPage2.Location = New System.Drawing.Point(4, 25)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(1012, 601)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "TabPage2"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.gridview)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Location = New System.Drawing.Point(3, 98)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(1006, 500)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        '
        'gridview
        '
        Me.gridview.AllowUserToAddRows = False
        Me.gridview.AllowUserToDeleteRows = False
        Me.gridview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridview.Location = New System.Drawing.Point(3, 17)
        Me.gridview.Name = "gridview"
        Me.gridview.ReadOnly = True
        Me.gridview.RowHeadersVisible = False
        Me.gridview.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.ColumnHeaderSelect
        Me.gridview.Size = New System.Drawing.Size(1000, 480)
        Me.gridview.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.chkwithcancel)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.cmbvsentby)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.cmbvtocostid)
        Me.Panel1.Controls.Add(Me.dtpfrmdate)
        Me.Panel1.Controls.Add(Me.dtptodate)
        Me.Panel1.Controls.Add(Me.btnexport)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.btnback)
        Me.Panel1.Controls.Add(Me.btnview)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1006, 95)
        Me.Panel1.TabIndex = 0
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(8, 64)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(52, 13)
        Me.Label11.TabIndex = 6
        Me.Label11.Text = "Sent By"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbvsentby
        '
        Me.cmbvsentby.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbvsentby.FormattingEnabled = True
        Me.cmbvsentby.Location = New System.Drawing.Point(101, 64)
        Me.cmbvsentby.Name = "cmbvsentby"
        Me.cmbvsentby.Size = New System.Drawing.Size(283, 21)
        Me.cmbvsentby.TabIndex = 7
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(186, 8)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(21, 13)
        Me.Label9.TabIndex = 2
        Me.Label9.Text = "To"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 38)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(87, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "To Costcentre"
        '
        'cmbvtocostid
        '
        Me.cmbvtocostid.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbvtocostid.FormattingEnabled = True
        Me.cmbvtocostid.Location = New System.Drawing.Point(101, 35)
        Me.cmbvtocostid.Name = "cmbvtocostid"
        Me.cmbvtocostid.Size = New System.Drawing.Size(283, 21)
        Me.cmbvtocostid.TabIndex = 5
        '
        'dtpfrmdate
        '
        Me.dtpfrmdate.Location = New System.Drawing.Point(102, 5)
        Me.dtpfrmdate.Mask = "##-##-####"
        Me.dtpfrmdate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpfrmdate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpfrmdate.Name = "dtpfrmdate"
        Me.dtpfrmdate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpfrmdate.Size = New System.Drawing.Size(78, 21)
        Me.dtpfrmdate.TabIndex = 1
        Me.dtpfrmdate.Text = "29/09/2010"
        Me.dtpfrmdate.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'dtptodate
        '
        Me.dtptodate.Location = New System.Drawing.Point(213, 5)
        Me.dtptodate.Mask = "##-##-####"
        Me.dtptodate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtptodate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtptodate.Name = "dtptodate"
        Me.dtptodate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtptodate.Size = New System.Drawing.Size(78, 21)
        Me.dtptodate.TabIndex = 3
        Me.dtptodate.Text = "29/09/2010"
        Me.dtptodate.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'btnexport
        '
        Me.btnexport.Location = New System.Drawing.Point(485, 55)
        Me.btnexport.Name = "btnexport"
        Me.btnexport.Size = New System.Drawing.Size(100, 30)
        Me.btnexport.TabIndex = 9
        Me.btnexport.Text = "Export"
        Me.btnexport.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(9, 8)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(67, 13)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "From Date"
        '
        'btnback
        '
        Me.btnback.Location = New System.Drawing.Point(584, 55)
        Me.btnback.Name = "btnback"
        Me.btnback.Size = New System.Drawing.Size(100, 30)
        Me.btnback.TabIndex = 10
        Me.btnback.Text = "Back[Esc]"
        Me.btnback.UseVisualStyleBackColor = True
        '
        'btnview
        '
        Me.btnview.Location = New System.Drawing.Point(386, 55)
        Me.btnview.Name = "btnview"
        Me.btnview.Size = New System.Drawing.Size(100, 30)
        Me.btnview.TabIndex = 8
        Me.btnview.Text = "View"
        Me.btnview.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.GroupBox3)
        Me.TabPage3.Controls.Add(Me.Panel2)
        Me.TabPage3.Location = New System.Drawing.Point(4, 25)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(1012, 601)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "TabPage3"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.cmbgrid_OWN)
        Me.GroupBox3.Controls.Add(Me.gridviewR)
        Me.GroupBox3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox3.Location = New System.Drawing.Point(0, 133)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(1012, 468)
        Me.GroupBox3.TabIndex = 1
        Me.GroupBox3.TabStop = False
        '
        'cmbgrid_OWN
        '
        Me.cmbgrid_OWN.FormattingEnabled = True
        Me.cmbgrid_OWN.Location = New System.Drawing.Point(446, 212)
        Me.cmbgrid_OWN.Name = "cmbgrid_OWN"
        Me.cmbgrid_OWN.Size = New System.Drawing.Size(121, 21)
        Me.cmbgrid_OWN.TabIndex = 1
        '
        'gridviewR
        '
        Me.gridviewR.AllowUserToAddRows = False
        Me.gridviewR.AllowUserToDeleteRows = False
        Me.gridviewR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridviewR.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridviewR.Location = New System.Drawing.Point(3, 17)
        Me.gridviewR.Name = "gridviewR"
        Me.gridviewR.ReadOnly = True
        Me.gridviewR.Size = New System.Drawing.Size(1006, 448)
        Me.gridviewR.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.btnRexport)
        Me.Panel2.Controls.Add(Me.btnRexit)
        Me.Panel2.Controls.Add(Me.btnRview)
        Me.Panel2.Controls.Add(Me.Type)
        Me.Panel2.Controls.Add(Me.cmbRrpttype)
        Me.Panel2.Controls.Add(Me.Label12)
        Me.Panel2.Controls.Add(Me.cmbRreceivedby)
        Me.Panel2.Controls.Add(Me.Label14)
        Me.Panel2.Controls.Add(Me.cmbRfrmcostid)
        Me.Panel2.Controls.Add(Me.pnldatefilt)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1012, 133)
        Me.Panel2.TabIndex = 0
        '
        'btnRexport
        '
        Me.btnRexport.Location = New System.Drawing.Point(491, 97)
        Me.btnRexport.Name = "btnRexport"
        Me.btnRexport.Size = New System.Drawing.Size(100, 30)
        Me.btnRexport.TabIndex = 8
        Me.btnRexport.Text = "Export"
        Me.btnRexport.UseVisualStyleBackColor = True
        '
        'btnRexit
        '
        Me.btnRexit.Location = New System.Drawing.Point(590, 97)
        Me.btnRexit.Name = "btnRexit"
        Me.btnRexit.Size = New System.Drawing.Size(100, 30)
        Me.btnRexit.TabIndex = 9
        Me.btnRexit.Text = "Exit[F12]"
        Me.btnRexit.UseVisualStyleBackColor = True
        '
        'btnRview
        '
        Me.btnRview.Location = New System.Drawing.Point(392, 97)
        Me.btnRview.Name = "btnRview"
        Me.btnRview.Size = New System.Drawing.Size(100, 30)
        Me.btnRview.TabIndex = 7
        Me.btnRview.Text = "View"
        Me.btnRview.UseVisualStyleBackColor = True
        '
        'Type
        '
        Me.Type.AutoSize = True
        Me.Type.Location = New System.Drawing.Point(4, 17)
        Me.Type.Name = "Type"
        Me.Type.Size = New System.Drawing.Size(35, 13)
        Me.Type.TabIndex = 0
        Me.Type.Text = "Type"
        '
        'cmbRrpttype
        '
        Me.cmbRrpttype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbRrpttype.FormattingEnabled = True
        Me.cmbRrpttype.Items.AddRange(New Object() {"All", "Pending", "Received"})
        Me.cmbRrpttype.Location = New System.Drawing.Point(107, 14)
        Me.cmbRrpttype.Name = "cmbRrpttype"
        Me.cmbRrpttype.Size = New System.Drawing.Size(105, 21)
        Me.cmbRrpttype.TabIndex = 1
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(4, 104)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(78, 13)
        Me.Label12.TabIndex = 5
        Me.Label12.Text = "Received By"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbRreceivedby
        '
        Me.cmbRreceivedby.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbRreceivedby.FormattingEnabled = True
        Me.cmbRreceivedby.Location = New System.Drawing.Point(107, 104)
        Me.cmbRreceivedby.Name = "cmbRreceivedby"
        Me.cmbRreceivedby.Size = New System.Drawing.Size(279, 21)
        Me.cmbRreceivedby.TabIndex = 6
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(4, 78)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(102, 13)
        Me.Label14.TabIndex = 3
        Me.Label14.Text = "From Costcentre"
        '
        'cmbRfrmcostid
        '
        Me.cmbRfrmcostid.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbRfrmcostid.FormattingEnabled = True
        Me.cmbRfrmcostid.Location = New System.Drawing.Point(107, 75)
        Me.cmbRfrmcostid.Name = "cmbRfrmcostid"
        Me.cmbRfrmcostid.Size = New System.Drawing.Size(279, 21)
        Me.cmbRfrmcostid.TabIndex = 4
        '
        'pnldatefilt
        '
        Me.pnldatefilt.Controls.Add(Me.dtpRfrmdate)
        Me.pnldatefilt.Controls.Add(Me.Label15)
        Me.pnldatefilt.Controls.Add(Me.Label13)
        Me.pnldatefilt.Controls.Add(Me.dtpRTodate)
        Me.pnldatefilt.Location = New System.Drawing.Point(4, 41)
        Me.pnldatefilt.Name = "pnldatefilt"
        Me.pnldatefilt.Size = New System.Drawing.Size(345, 30)
        Me.pnldatefilt.TabIndex = 2
        '
        'dtpRfrmdate
        '
        Me.dtpRfrmdate.Location = New System.Drawing.Point(103, 3)
        Me.dtpRfrmdate.Mask = "##-##-####"
        Me.dtpRfrmdate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpRfrmdate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpRfrmdate.Name = "dtpRfrmdate"
        Me.dtpRfrmdate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpRfrmdate.Size = New System.Drawing.Size(78, 21)
        Me.dtpRfrmdate.TabIndex = 1
        Me.dtpRfrmdate.Text = "29/09/2010"
        Me.dtpRfrmdate.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(1, 6)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(67, 13)
        Me.Label15.TabIndex = 0
        Me.Label15.Text = "From Date"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(187, 6)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(21, 13)
        Me.Label13.TabIndex = 2
        Me.Label13.Text = "To"
        '
        'dtpRTodate
        '
        Me.dtpRTodate.Location = New System.Drawing.Point(214, 3)
        Me.dtpRTodate.Mask = "##-##-####"
        Me.dtpRTodate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpRTodate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpRTodate.Name = "dtpRTodate"
        Me.dtpRTodate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpRTodate.Size = New System.Drawing.Size(78, 21)
        Me.dtpRTodate.TabIndex = 3
        Me.dtpRTodate.Text = "29/09/2010"
        Me.dtpRTodate.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'chkwithcancel
        '
        Me.chkwithcancel.AutoSize = True
        Me.chkwithcancel.Location = New System.Drawing.Point(297, 8)
        Me.chkwithcancel.Name = "chkwithcancel"
        Me.chkwithcancel.Size = New System.Drawing.Size(88, 17)
        Me.chkwithcancel.TabIndex = 4
        Me.chkwithcancel.Text = "withCancel"
        Me.chkwithcancel.UseVisualStyleBackColor = True
        '
        'frmdocumentupdation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1020, 630)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.lblStatus)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "frmdocumentupdation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Document Maintanence"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.gridview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        CType(Me.gridviewR, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.pnldatefilt.ResumeLayout(False)
        Me.pnldatefilt.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents cmbsentby As System.Windows.Forms.ComboBox
    Friend WithEvents cmbcheckedby As System.Windows.Forms.ComboBox
    Friend WithEvents cmbpreperedby As System.Windows.Forms.ComboBox
    Friend WithEvents cmbtocostid As System.Windows.Forms.ComboBox
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents cmbdoctype As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbdocdescription As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtdocdes As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnopen As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cmbsentto As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnview As System.Windows.Forms.Button
    Friend WithEvents gridview As System.Windows.Forms.DataGridView
    Friend WithEvents btnback As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents btnexport As System.Windows.Forms.Button
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents cmbvsentby As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbvtocostid As System.Windows.Forms.ComboBox
    Friend WithEvents dtpfrmdate As BrighttechPack.DatePicker
    Friend WithEvents dtptodate As BrighttechPack.DatePicker
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents gridviewR As System.Windows.Forms.DataGridView
    Friend WithEvents pnldatefilt As System.Windows.Forms.Panel
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents cmbRreceivedby As System.Windows.Forms.ComboBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents dtpRfrmdate As BrighttechPack.DatePicker
    Friend WithEvents dtpRTodate As BrighttechPack.DatePicker
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Type As System.Windows.Forms.Label
    Friend WithEvents cmbRrpttype As System.Windows.Forms.ComboBox
    Friend WithEvents cmbgrid_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents btnRexport As System.Windows.Forms.Button
    Friend WithEvents btnRexit As System.Windows.Forms.Button
    Friend WithEvents btnRview As System.Windows.Forms.Button
    Friend WithEvents cmbRfrmcostid As System.Windows.Forms.ComboBox
    Friend WithEvents dtpdoctodate As BrighttechPack.DatePicker
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents txtremark As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents dtpdocdate As BrighttechPack.DatePicker
    Friend WithEvents chkwithcancel As System.Windows.Forms.CheckBox
End Class
