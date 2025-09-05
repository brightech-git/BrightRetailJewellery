<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTax
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
        Me.btnExit = New System.Windows.Forms.Button
        Me.txtDisplayOrder_Num = New System.Windows.Forms.TextBox
        Me.btnNew = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnOpen = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.btnSave = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtSSurcharge_Per = New System.Windows.Forms.TextBox
        Me.txtSAdditionalSc_Per = New System.Windows.Forms.TextBox
        Me.txtSTax_Per = New System.Windows.Forms.TextBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtPAdditionalSc_Per = New System.Windows.Forms.TextBox
        Me.txtPSurcharge_Per = New System.Windows.Forms.TextBox
        Me.txtPTax_Per = New System.Windows.Forms.TextBox
        Me.txtTaxName__Man = New System.Windows.Forms.TextBox
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.tabView = New System.Windows.Forms.TabPage
        Me.grpInfo = New System.Windows.Forms.GroupBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.detPurAdlSc = New System.Windows.Forms.TextBox
        Me.detPurSc = New System.Windows.Forms.TextBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.detPurTax = New System.Windows.Forms.TextBox
        Me.detSalesSurcharge = New System.Windows.Forms.TextBox
        Me.detSalesTax = New System.Windows.Forms.TextBox
        Me.detSalesAdlSc = New System.Windows.Forms.TextBox
        Me.lblStatus = New System.Windows.Forms.Label
        Me.btnDelete = New System.Windows.Forms.Button
        Me.Label20 = New System.Windows.Forms.Label
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.grpInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(435, 271)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 15
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'txtDisplayOrder_Num
        '
        Me.txtDisplayOrder_Num.Location = New System.Drawing.Point(223, 108)
        Me.txtDisplayOrder_Num.MaxLength = 7
        Me.txtDisplayOrder_Num.Name = "txtDisplayOrder_Num"
        Me.txtDisplayOrder_Num.Size = New System.Drawing.Size(51, 21)
        Me.txtDisplayOrder_Num.TabIndex = 3
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(327, 271)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 14
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Location = New System.Drawing.Point(117, 79)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(65, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Tax Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(219, 271)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 13
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Location = New System.Drawing.Point(117, 109)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(86, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Display Order"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(111, 271)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 12
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.txtSSurcharge_Per)
        Me.GroupBox1.Controls.Add(Me.txtSAdditionalSc_Per)
        Me.GroupBox1.Controls.Add(Me.txtSTax_Per)
        Me.GroupBox1.Location = New System.Drawing.Point(111, 135)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(226, 115)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Sales"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 17)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(28, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Tax"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 49)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(69, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "SurCharge"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 85)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(81, 13)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Additional Sc"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtSSurcharge_Per
        '
        Me.txtSSurcharge_Per.Location = New System.Drawing.Point(112, 48)
        Me.txtSSurcharge_Per.MaxLength = 7
        Me.txtSSurcharge_Per.Name = "txtSSurcharge_Per"
        Me.txtSSurcharge_Per.Size = New System.Drawing.Size(100, 21)
        Me.txtSSurcharge_Per.TabIndex = 3
        '
        'txtSAdditionalSc_Per
        '
        Me.txtSAdditionalSc_Per.Location = New System.Drawing.Point(112, 81)
        Me.txtSAdditionalSc_Per.MaxLength = 7
        Me.txtSAdditionalSc_Per.Name = "txtSAdditionalSc_Per"
        Me.txtSAdditionalSc_Per.Size = New System.Drawing.Size(100, 21)
        Me.txtSAdditionalSc_Per.TabIndex = 5
        '
        'txtSTax_Per
        '
        Me.txtSTax_Per.Location = New System.Drawing.Point(112, 17)
        Me.txtSTax_Per.MaxLength = 7
        Me.txtSTax_Per.Name = "txtSTax_Per"
        Me.txtSTax_Per.Size = New System.Drawing.Size(100, 21)
        Me.txtSTax_Per.TabIndex = 1
        '
        'GroupBox2
        '
        Me.GroupBox2.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.txtPAdditionalSc_Per)
        Me.GroupBox2.Controls.Add(Me.txtPSurcharge_Per)
        Me.GroupBox2.Controls.Add(Me.txtPTax_Per)
        Me.GroupBox2.Location = New System.Drawing.Point(340, 135)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(226, 115)
        Me.GroupBox2.TabIndex = 5
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Purchase"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 22)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(28, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Tax"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 52)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(66, 13)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "Surcharge"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 85)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(81, 13)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "Additional Sc"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtPAdditionalSc_Per
        '
        Me.txtPAdditionalSc_Per.Location = New System.Drawing.Point(112, 81)
        Me.txtPAdditionalSc_Per.MaxLength = 7
        Me.txtPAdditionalSc_Per.Name = "txtPAdditionalSc_Per"
        Me.txtPAdditionalSc_Per.Size = New System.Drawing.Size(100, 21)
        Me.txtPAdditionalSc_Per.TabIndex = 5
        '
        'txtPSurcharge_Per
        '
        Me.txtPSurcharge_Per.Location = New System.Drawing.Point(112, 48)
        Me.txtPSurcharge_Per.MaxLength = 7
        Me.txtPSurcharge_Per.Name = "txtPSurcharge_Per"
        Me.txtPSurcharge_Per.Size = New System.Drawing.Size(100, 21)
        Me.txtPSurcharge_Per.TabIndex = 3
        '
        'txtPTax_Per
        '
        Me.txtPTax_Per.Location = New System.Drawing.Point(112, 18)
        Me.txtPTax_Per.MaxLength = 7
        Me.txtPTax_Per.Name = "txtPTax_Per"
        Me.txtPTax_Per.Size = New System.Drawing.Size(100, 21)
        Me.txtPTax_Per.TabIndex = 1
        '
        'txtTaxName__Man
        '
        Me.txtTaxName__Man.BackColor = System.Drawing.SystemColors.Window
        Me.txtTaxName__Man.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTaxName__Man.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTaxName__Man.Location = New System.Drawing.Point(223, 79)
        Me.txtTaxName__Man.MaxLength = 30
        Me.txtTaxName__Man.Name = "txtTaxName__Man"
        Me.txtTaxName__Man.Size = New System.Drawing.Size(212, 21)
        Me.txtTaxName__Man.TabIndex = 1
        Me.txtTaxName__Man.Text = "DDDDDDDDDD"
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(8, 35)
        Me.gridView.MultiSelect = False
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 20
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(650, 266)
        Me.gridView.TabIndex = 1
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
        'tabMain
        '
        Me.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tabMain.Controls.Add(Me.tabGeneral)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.ItemSize = New System.Drawing.Size(10, 5)
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(674, 421)
        Me.tabMain.TabIndex = 17
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.Label2)
        Me.tabGeneral.Controls.Add(Me.Label3)
        Me.tabGeneral.Controls.Add(Me.btnSave)
        Me.tabGeneral.Controls.Add(Me.txtTaxName__Man)
        Me.tabGeneral.Controls.Add(Me.btnExit)
        Me.tabGeneral.Controls.Add(Me.txtDisplayOrder_Num)
        Me.tabGeneral.Controls.Add(Me.btnNew)
        Me.tabGeneral.Controls.Add(Me.GroupBox1)
        Me.tabGeneral.Controls.Add(Me.btnOpen)
        Me.tabGeneral.Controls.Add(Me.GroupBox2)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 9)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(666, 408)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.Label20)
        Me.tabView.Controls.Add(Me.gridView)
        Me.tabView.Controls.Add(Me.grpInfo)
        Me.tabView.Controls.Add(Me.lblStatus)
        Me.tabView.Controls.Add(Me.btnDelete)
        Me.tabView.Location = New System.Drawing.Point(4, 9)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(666, 408)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'grpInfo
        '
        Me.grpInfo.Controls.Add(Me.Label15)
        Me.grpInfo.Controls.Add(Me.Label16)
        Me.grpInfo.Controls.Add(Me.Label1)
        Me.grpInfo.Controls.Add(Me.Label17)
        Me.grpInfo.Controls.Add(Me.Label13)
        Me.grpInfo.Controls.Add(Me.detPurAdlSc)
        Me.grpInfo.Controls.Add(Me.detPurSc)
        Me.grpInfo.Controls.Add(Me.Label14)
        Me.grpInfo.Controls.Add(Me.detPurTax)
        Me.grpInfo.Controls.Add(Me.detSalesSurcharge)
        Me.grpInfo.Controls.Add(Me.detSalesTax)
        Me.grpInfo.Controls.Add(Me.detSalesAdlSc)
        Me.grpInfo.Location = New System.Drawing.Point(8, 301)
        Me.grpInfo.Name = "grpInfo"
        Me.grpInfo.Size = New System.Drawing.Size(650, 70)
        Me.grpInfo.TabIndex = 3
        Me.grpInfo.TabStop = False
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(320, 16)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(51, 13)
        Me.Label15.TabIndex = 0
        Me.Label15.Text = "Pur Tax"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(320, 32)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(89, 13)
        Me.Label16.TabIndex = 2
        Me.Label16.Text = "Pur Surcharge"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(95, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Sales Tax"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(320, 48)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(104, 13)
        Me.Label17.TabIndex = 4
        Me.Label17.Text = "Pur Additional Sc"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(95, 33)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(101, 13)
        Me.Label13.TabIndex = 2
        Me.Label13.Text = "Sales Surcharge"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'detPurAdlSc
        '
        Me.detPurAdlSc.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detPurAdlSc.Location = New System.Drawing.Point(426, 47)
        Me.detPurAdlSc.MaxLength = 7
        Me.detPurAdlSc.Name = "detPurAdlSc"
        Me.detPurAdlSc.Size = New System.Drawing.Size(100, 14)
        Me.detPurAdlSc.TabIndex = 5
        '
        'detPurSc
        '
        Me.detPurSc.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detPurSc.Location = New System.Drawing.Point(426, 31)
        Me.detPurSc.MaxLength = 7
        Me.detPurSc.Name = "detPurSc"
        Me.detPurSc.Size = New System.Drawing.Size(100, 14)
        Me.detPurSc.TabIndex = 3
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(95, 49)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(116, 13)
        Me.Label14.TabIndex = 4
        Me.Label14.Text = "Sales Additional Sc"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'detPurTax
        '
        Me.detPurTax.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detPurTax.Location = New System.Drawing.Point(426, 15)
        Me.detPurTax.MaxLength = 7
        Me.detPurTax.Name = "detPurTax"
        Me.detPurTax.Size = New System.Drawing.Size(100, 14)
        Me.detPurTax.TabIndex = 1
        '
        'detSalesSurcharge
        '
        Me.detSalesSurcharge.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detSalesSurcharge.Location = New System.Drawing.Point(214, 33)
        Me.detSalesSurcharge.MaxLength = 7
        Me.detSalesSurcharge.Name = "detSalesSurcharge"
        Me.detSalesSurcharge.Size = New System.Drawing.Size(100, 14)
        Me.detSalesSurcharge.TabIndex = 3
        '
        'detSalesTax
        '
        Me.detSalesTax.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detSalesTax.Location = New System.Drawing.Point(214, 17)
        Me.detSalesTax.MaxLength = 7
        Me.detSalesTax.Name = "detSalesTax"
        Me.detSalesTax.Size = New System.Drawing.Size(100, 14)
        Me.detSalesTax.TabIndex = 1
        '
        'detSalesAdlSc
        '
        Me.detSalesAdlSc.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detSalesAdlSc.Location = New System.Drawing.Point(214, 49)
        Me.detSalesAdlSc.MaxLength = 7
        Me.detSalesAdlSc.Name = "detSalesAdlSc"
        Me.detSalesAdlSc.Size = New System.Drawing.Size(100, 14)
        Me.detSalesAdlSc.TabIndex = 5
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(8, 377)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(103, 13)
        Me.lblStatus.TabIndex = 24
        Me.lblStatus.Text = "*Hit Enter to Edit"
        Me.lblStatus.Visible = False
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(11, 3)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnDelete.TabIndex = 2
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.ForeColor = System.Drawing.Color.Red
        Me.Label20.Location = New System.Drawing.Point(117, 12)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(153, 13)
        Me.Label20.TabIndex = 25
        Me.Label20.Text = "*Press Escape to Back"
        '
        'frmTax
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(674, 421)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmTax"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Tax"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.tabGeneral.PerformLayout()
        Me.tabView.ResumeLayout(False)
        Me.tabView.PerformLayout()
        Me.grpInfo.ResumeLayout(False)
        Me.grpInfo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtTaxName__Man As System.Windows.Forms.TextBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtPSurcharge_Per As System.Windows.Forms.TextBox
    Friend WithEvents txtPTax_Per As System.Windows.Forms.TextBox
    Friend WithEvents txtSTax_Per As System.Windows.Forms.TextBox
    Friend WithEvents txtSAdditionalSc_Per As System.Windows.Forms.TextBox
    Friend WithEvents txtPAdditionalSc_Per As System.Windows.Forms.TextBox
    Friend WithEvents txtDisplayOrder_Num As System.Windows.Forms.TextBox
    Friend WithEvents txtSSurcharge_Per As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents grpInfo As System.Windows.Forms.GroupBox
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents detPurAdlSc As System.Windows.Forms.TextBox
    Friend WithEvents detPurSc As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents detPurTax As System.Windows.Forms.TextBox
    Friend WithEvents detSalesSurcharge As System.Windows.Forms.TextBox
    Friend WithEvents detSalesTax As System.Windows.Forms.TextBox
    Friend WithEvents detSalesAdlSc As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
End Class
