<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCompanyMast
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
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabGen = New System.Windows.Forms.TabPage
        Me.pnlInputControls = New System.Windows.Forms.Panel
        Me.txtGstNo = New System.Windows.Forms.TextBox
        Me.Label28 = New System.Windows.Forms.Label
        Me.CmbState = New System.Windows.Forms.ComboBox
        Me.Label27 = New System.Windows.Forms.Label
        Me.chkcmbcostcentre = New BrighttechPack.CheckedComboBox
        Me.lblCostName = New System.Windows.Forms.Label
        Me.txtTanno = New System.Windows.Forms.TextBox
        Me.Label25 = New System.Windows.Forms.Label
        Me.cmbActive = New System.Windows.Forms.ComboBox
        Me.Label23 = New System.Windows.Forms.Label
        Me.Label21 = New System.Windows.Forms.Label
        Me.cmbShortKey = New System.Windows.Forms.ComboBox
        Me.txtAddress4 = New System.Windows.Forms.TextBox
        Me.txtAddress3 = New System.Windows.Forms.TextBox
        Me.txtAddress1 = New System.Windows.Forms.TextBox
        Me.txtAddress2 = New System.Windows.Forms.TextBox
        Me.txtDisplayOrder = New System.Windows.Forms.TextBox
        Me.txtTdsNo = New System.Windows.Forms.TextBox
        Me.txtPanNo = New System.Windows.Forms.TextBox
        Me.txtTinNo = New System.Windows.Forms.TextBox
        Me.txtAreaCode = New System.Windows.Forms.TextBox
        Me.txtCstNo = New System.Windows.Forms.TextBox
        Me.txtLocalTaxNo = New System.Windows.Forms.TextBox
        Me.txtEmail = New System.Windows.Forms.TextBox
        Me.txtPhone = New System.Windows.Forms.TextBox
        Me.txtCompanyName = New System.Windows.Forms.TextBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnOpen = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.btnSave = New System.Windows.Forms.Button
        Me.txtCompanyId = New System.Windows.Forms.TextBox
        Me.Label24 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.tabView = New System.Windows.Forms.TabPage
        Me.pnlFooter = New System.Windows.Forms.Panel
        Me.btnBack = New System.Windows.Forms.Button
        Me.lblStatus = New System.Windows.Forms.Label
        Me.btnDelete = New System.Windows.Forms.Button
        Me.pnlViewDet = New System.Windows.Forms.Panel
        Me.detGSTNO = New System.Windows.Forms.TextBox
        Me.Label30 = New System.Windows.Forms.Label
        Me.detState = New System.Windows.Forms.TextBox
        Me.Label29 = New System.Windows.Forms.Label
        Me.detTANNo = New System.Windows.Forms.TextBox
        Me.Label26 = New System.Windows.Forms.Label
        Me.detAddress4 = New System.Windows.Forms.TextBox
        Me.detAddress3 = New System.Windows.Forms.TextBox
        Me.detAddress2 = New System.Windows.Forms.TextBox
        Me.detAddress1 = New System.Windows.Forms.TextBox
        Me.detLocalTaxNo = New System.Windows.Forms.TextBox
        Me.detAreaCode = New System.Windows.Forms.TextBox
        Me.detTDSNo = New System.Windows.Forms.TextBox
        Me.detPANNo = New System.Windows.Forms.TextBox
        Me.detTINNo = New System.Windows.Forms.TextBox
        Me.detCSTNo = New System.Windows.Forms.TextBox
        Me.detEmail = New System.Windows.Forms.TextBox
        Me.detPhone = New System.Windows.Forms.TextBox
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label22 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.tabMain.SuspendLayout()
        Me.tabGen.SuspendLayout()
        Me.pnlInputControls.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.pnlFooter.SuspendLayout()
        Me.pnlViewDet.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGen)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(671, 535)
        Me.tabMain.TabIndex = 0
        '
        'tabGen
        '
        Me.tabGen.Controls.Add(Me.pnlInputControls)
        Me.tabGen.Location = New System.Drawing.Point(4, 22)
        Me.tabGen.Name = "tabGen"
        Me.tabGen.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGen.Size = New System.Drawing.Size(663, 509)
        Me.tabGen.TabIndex = 0
        Me.tabGen.Text = "Gen"
        Me.tabGen.UseVisualStyleBackColor = True
        '
        'pnlInputControls
        '
        Me.pnlInputControls.Controls.Add(Me.txtGstNo)
        Me.pnlInputControls.Controls.Add(Me.Label28)
        Me.pnlInputControls.Controls.Add(Me.CmbState)
        Me.pnlInputControls.Controls.Add(Me.Label27)
        Me.pnlInputControls.Controls.Add(Me.chkcmbcostcentre)
        Me.pnlInputControls.Controls.Add(Me.lblCostName)
        Me.pnlInputControls.Controls.Add(Me.txtTanno)
        Me.pnlInputControls.Controls.Add(Me.Label25)
        Me.pnlInputControls.Controls.Add(Me.cmbActive)
        Me.pnlInputControls.Controls.Add(Me.Label23)
        Me.pnlInputControls.Controls.Add(Me.Label21)
        Me.pnlInputControls.Controls.Add(Me.cmbShortKey)
        Me.pnlInputControls.Controls.Add(Me.txtAddress4)
        Me.pnlInputControls.Controls.Add(Me.txtAddress3)
        Me.pnlInputControls.Controls.Add(Me.txtAddress1)
        Me.pnlInputControls.Controls.Add(Me.txtAddress2)
        Me.pnlInputControls.Controls.Add(Me.txtDisplayOrder)
        Me.pnlInputControls.Controls.Add(Me.txtTdsNo)
        Me.pnlInputControls.Controls.Add(Me.txtPanNo)
        Me.pnlInputControls.Controls.Add(Me.txtTinNo)
        Me.pnlInputControls.Controls.Add(Me.txtAreaCode)
        Me.pnlInputControls.Controls.Add(Me.txtCstNo)
        Me.pnlInputControls.Controls.Add(Me.txtLocalTaxNo)
        Me.pnlInputControls.Controls.Add(Me.txtEmail)
        Me.pnlInputControls.Controls.Add(Me.txtPhone)
        Me.pnlInputControls.Controls.Add(Me.txtCompanyName)
        Me.pnlInputControls.Controls.Add(Me.btnExit)
        Me.pnlInputControls.Controls.Add(Me.btnOpen)
        Me.pnlInputControls.Controls.Add(Me.btnNew)
        Me.pnlInputControls.Controls.Add(Me.btnSave)
        Me.pnlInputControls.Controls.Add(Me.txtCompanyId)
        Me.pnlInputControls.Controls.Add(Me.Label24)
        Me.pnlInputControls.Controls.Add(Me.Label12)
        Me.pnlInputControls.Controls.Add(Me.Label11)
        Me.pnlInputControls.Controls.Add(Me.Label10)
        Me.pnlInputControls.Controls.Add(Me.Label9)
        Me.pnlInputControls.Controls.Add(Me.Label8)
        Me.pnlInputControls.Controls.Add(Me.Label7)
        Me.pnlInputControls.Controls.Add(Me.Label6)
        Me.pnlInputControls.Controls.Add(Me.Label5)
        Me.pnlInputControls.Controls.Add(Me.Label4)
        Me.pnlInputControls.Controls.Add(Me.Label3)
        Me.pnlInputControls.Controls.Add(Me.Label2)
        Me.pnlInputControls.Controls.Add(Me.Label1)
        Me.pnlInputControls.Location = New System.Drawing.Point(17, 18)
        Me.pnlInputControls.Name = "pnlInputControls"
        Me.pnlInputControls.Size = New System.Drawing.Size(628, 483)
        Me.pnlInputControls.TabIndex = 0
        '
        'txtGstNo
        '
        Me.txtGstNo.Location = New System.Drawing.Point(399, 391)
        Me.txtGstNo.MaxLength = 30
        Me.txtGstNo.Name = "txtGstNo"
        Me.txtGstNo.Size = New System.Drawing.Size(204, 21)
        Me.txtGstNo.TabIndex = 34
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Location = New System.Drawing.Point(330, 393)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(44, 13)
        Me.Label28.TabIndex = 33
        Me.Label28.Text = "GSTIN"
        '
        'CmbState
        '
        Me.CmbState.FormattingEnabled = True
        Me.CmbState.Items.AddRange(New Object() {"Yes", "No"})
        Me.CmbState.Location = New System.Drawing.Point(127, 218)
        Me.CmbState.Name = "CmbState"
        Me.CmbState.Size = New System.Drawing.Size(204, 21)
        Me.CmbState.TabIndex = 12
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Location = New System.Drawing.Point(27, 222)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(37, 13)
        Me.Label27.TabIndex = 11
        Me.Label27.Text = "State"
        '
        'chkcmbcostcentre
        '
        Me.chkcmbcostcentre.CheckOnClick = True
        Me.chkcmbcostcentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbcostcentre.DropDownHeight = 1
        Me.chkcmbcostcentre.FormattingEnabled = True
        Me.chkcmbcostcentre.IntegralHeight = False
        Me.chkcmbcostcentre.Location = New System.Drawing.Point(127, 73)
        Me.chkcmbcostcentre.Name = "chkcmbcostcentre"
        Me.chkcmbcostcentre.Size = New System.Drawing.Size(476, 22)
        Me.chkcmbcostcentre.TabIndex = 5
        Me.chkcmbcostcentre.ValueSeparator = ", "
        '
        'lblCostName
        '
        Me.lblCostName.AutoSize = True
        Me.lblCostName.Location = New System.Drawing.Point(27, 81)
        Me.lblCostName.Name = "lblCostName"
        Me.lblCostName.Size = New System.Drawing.Size(70, 13)
        Me.lblCostName.TabIndex = 4
        Me.lblCostName.Text = "Cost Name"
        '
        'txtTanno
        '
        Me.txtTanno.Location = New System.Drawing.Point(127, 391)
        Me.txtTanno.MaxLength = 30
        Me.txtTanno.Name = "txtTanno"
        Me.txtTanno.Size = New System.Drawing.Size(204, 21)
        Me.txtTanno.TabIndex = 32
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(27, 393)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(49, 13)
        Me.Label25.TabIndex = 31
        Me.Label25.Text = "TAN No"
        '
        'cmbActive
        '
        Me.cmbActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbActive.FormattingEnabled = True
        Me.cmbActive.Items.AddRange(New Object() {"Yes", "No"})
        Me.cmbActive.Location = New System.Drawing.Point(398, 418)
        Me.cmbActive.Name = "cmbActive"
        Me.cmbActive.Size = New System.Drawing.Size(70, 21)
        Me.cmbActive.TabIndex = 39
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(124, 422)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(47, 13)
        Me.Label23.TabIndex = 36
        Me.Label23.Text = "Ctrl + "
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(27, 421)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(81, 13)
        Me.Label21.TabIndex = 35
        Me.Label21.Text = "Shortcut Key"
        '
        'cmbShortKey
        '
        Me.cmbShortKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbShortKey.FormattingEnabled = True
        Me.cmbShortKey.Items.AddRange(New Object() {"", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"})
        Me.cmbShortKey.Location = New System.Drawing.Point(177, 418)
        Me.cmbShortKey.Name = "cmbShortKey"
        Me.cmbShortKey.Size = New System.Drawing.Size(70, 21)
        Me.cmbShortKey.TabIndex = 37
        '
        'txtAddress4
        '
        Me.txtAddress4.Location = New System.Drawing.Point(127, 190)
        Me.txtAddress4.MaxLength = 50
        Me.txtAddress4.Name = "txtAddress4"
        Me.txtAddress4.Size = New System.Drawing.Size(475, 21)
        Me.txtAddress4.TabIndex = 10
        '
        'txtAddress3
        '
        Me.txtAddress3.Location = New System.Drawing.Point(127, 161)
        Me.txtAddress3.MaxLength = 50
        Me.txtAddress3.Name = "txtAddress3"
        Me.txtAddress3.Size = New System.Drawing.Size(475, 21)
        Me.txtAddress3.TabIndex = 9
        '
        'txtAddress1
        '
        Me.txtAddress1.Location = New System.Drawing.Point(127, 103)
        Me.txtAddress1.MaxLength = 50
        Me.txtAddress1.Name = "txtAddress1"
        Me.txtAddress1.Size = New System.Drawing.Size(475, 21)
        Me.txtAddress1.TabIndex = 7
        '
        'txtAddress2
        '
        Me.txtAddress2.Location = New System.Drawing.Point(127, 132)
        Me.txtAddress2.MaxLength = 50
        Me.txtAddress2.Name = "txtAddress2"
        Me.txtAddress2.Size = New System.Drawing.Size(475, 21)
        Me.txtAddress2.TabIndex = 8
        '
        'txtDisplayOrder
        '
        Me.txtDisplayOrder.Location = New System.Drawing.Point(398, 362)
        Me.txtDisplayOrder.MaxLength = 3
        Me.txtDisplayOrder.Name = "txtDisplayOrder"
        Me.txtDisplayOrder.Size = New System.Drawing.Size(204, 21)
        Me.txtDisplayOrder.TabIndex = 30
        '
        'txtTdsNo
        '
        Me.txtTdsNo.Location = New System.Drawing.Point(127, 362)
        Me.txtTdsNo.MaxLength = 30
        Me.txtTdsNo.Name = "txtTdsNo"
        Me.txtTdsNo.Size = New System.Drawing.Size(204, 21)
        Me.txtTdsNo.TabIndex = 28
        '
        'txtPanNo
        '
        Me.txtPanNo.Location = New System.Drawing.Point(398, 333)
        Me.txtPanNo.MaxLength = 30
        Me.txtPanNo.Name = "txtPanNo"
        Me.txtPanNo.Size = New System.Drawing.Size(204, 21)
        Me.txtPanNo.TabIndex = 26
        '
        'txtTinNo
        '
        Me.txtTinNo.Location = New System.Drawing.Point(127, 333)
        Me.txtTinNo.MaxLength = 30
        Me.txtTinNo.Name = "txtTinNo"
        Me.txtTinNo.Size = New System.Drawing.Size(204, 21)
        Me.txtTinNo.TabIndex = 24
        '
        'txtAreaCode
        '
        Me.txtAreaCode.Location = New System.Drawing.Point(127, 246)
        Me.txtAreaCode.MaxLength = 10
        Me.txtAreaCode.Name = "txtAreaCode"
        Me.txtAreaCode.Size = New System.Drawing.Size(204, 21)
        Me.txtAreaCode.TabIndex = 14
        '
        'txtCstNo
        '
        Me.txtCstNo.Location = New System.Drawing.Point(398, 304)
        Me.txtCstNo.MaxLength = 30
        Me.txtCstNo.Name = "txtCstNo"
        Me.txtCstNo.Size = New System.Drawing.Size(204, 21)
        Me.txtCstNo.TabIndex = 22
        '
        'txtLocalTaxNo
        '
        Me.txtLocalTaxNo.Location = New System.Drawing.Point(127, 304)
        Me.txtLocalTaxNo.MaxLength = 30
        Me.txtLocalTaxNo.Name = "txtLocalTaxNo"
        Me.txtLocalTaxNo.Size = New System.Drawing.Size(204, 21)
        Me.txtLocalTaxNo.TabIndex = 20
        '
        'txtEmail
        '
        Me.txtEmail.Location = New System.Drawing.Point(127, 275)
        Me.txtEmail.MaxLength = 50
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(475, 21)
        Me.txtEmail.TabIndex = 18
        '
        'txtPhone
        '
        Me.txtPhone.Location = New System.Drawing.Point(398, 246)
        Me.txtPhone.MaxLength = 30
        Me.txtPhone.Name = "txtPhone"
        Me.txtPhone.Size = New System.Drawing.Size(204, 21)
        Me.txtPhone.TabIndex = 16
        '
        'txtCompanyName
        '
        Me.txtCompanyName.Location = New System.Drawing.Point(127, 44)
        Me.txtCompanyName.MaxLength = 50
        Me.txtCompanyName.Name = "txtCompanyName"
        Me.txtCompanyName.Size = New System.Drawing.Size(475, 21)
        Me.txtCompanyName.TabIndex = 3
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(445, 445)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 43
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(233, 445)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 41
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.ContextMenuStrip = Me.ContextMenuStrip1
        Me.btnNew.Location = New System.Drawing.Point(339, 445)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 42
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.OpenToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(123, 92)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        Me.OpenToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExToolStripMenuItem
        '
        Me.ExToolStripMenuItem.Name = "ExToolStripMenuItem"
        Me.ExToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.ExToolStripMenuItem.Text = "Exit"
        Me.ExToolStripMenuItem.Visible = False
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(127, 445)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 40
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'txtCompanyId
        '
        Me.txtCompanyId.Location = New System.Drawing.Point(127, 15)
        Me.txtCompanyId.MaxLength = 3
        Me.txtCompanyId.Name = "txtCompanyId"
        Me.txtCompanyId.Size = New System.Drawing.Size(60, 21)
        Me.txtCompanyId.TabIndex = 1
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(330, 422)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(42, 13)
        Me.Label24.TabIndex = 38
        Me.Label24.Text = "Active"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(330, 369)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(69, 13)
        Me.Label12.TabIndex = 29
        Me.Label12.Text = "Disp Order"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(27, 365)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(50, 13)
        Me.Label11.TabIndex = 27
        Me.Label11.Text = "TDS No"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(330, 340)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(49, 13)
        Me.Label10.TabIndex = 25
        Me.Label10.Text = "PAN No"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(27, 337)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(46, 13)
        Me.Label9.TabIndex = 23
        Me.Label9.Text = "TIN No"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(27, 253)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(68, 13)
        Me.Label8.TabIndex = 13
        Me.Label8.Text = "Area Code"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(330, 311)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(50, 13)
        Me.Label7.TabIndex = 21
        Me.Label7.Text = "CST No"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(27, 309)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(80, 13)
        Me.Label6.TabIndex = 19
        Me.Label6.Text = "Local Tax No"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(27, 281)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(36, 13)
        Me.Label5.TabIndex = 17
        Me.Label5.Text = "EMail"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(337, 253)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(42, 13)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "Phone"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(27, 112)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Address"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(27, 50)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(99, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Company Name"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(27, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Company ID"
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.pnlFooter)
        Me.tabView.Controls.Add(Me.pnlViewDet)
        Me.tabView.Controls.Add(Me.gridView)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(663, 509)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'pnlFooter
        '
        Me.pnlFooter.Controls.Add(Me.btnBack)
        Me.pnlFooter.Controls.Add(Me.lblStatus)
        Me.pnlFooter.Controls.Add(Me.btnDelete)
        Me.pnlFooter.Location = New System.Drawing.Point(8, 464)
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Size = New System.Drawing.Size(647, 35)
        Me.pnlFooter.TabIndex = 2
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(396, 3)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 2
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(3, 12)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(103, 13)
        Me.lblStatus.TabIndex = 1
        Me.lblStatus.Text = "*Hit Enter to Edit"
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(501, 3)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnDelete.TabIndex = 0
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'pnlViewDet
        '
        Me.pnlViewDet.Controls.Add(Me.detGSTNO)
        Me.pnlViewDet.Controls.Add(Me.Label30)
        Me.pnlViewDet.Controls.Add(Me.detState)
        Me.pnlViewDet.Controls.Add(Me.Label29)
        Me.pnlViewDet.Controls.Add(Me.detTANNo)
        Me.pnlViewDet.Controls.Add(Me.Label26)
        Me.pnlViewDet.Controls.Add(Me.detAddress4)
        Me.pnlViewDet.Controls.Add(Me.detAddress3)
        Me.pnlViewDet.Controls.Add(Me.detAddress2)
        Me.pnlViewDet.Controls.Add(Me.detAddress1)
        Me.pnlViewDet.Controls.Add(Me.detLocalTaxNo)
        Me.pnlViewDet.Controls.Add(Me.detAreaCode)
        Me.pnlViewDet.Controls.Add(Me.detTDSNo)
        Me.pnlViewDet.Controls.Add(Me.detPANNo)
        Me.pnlViewDet.Controls.Add(Me.detTINNo)
        Me.pnlViewDet.Controls.Add(Me.detCSTNo)
        Me.pnlViewDet.Controls.Add(Me.detEmail)
        Me.pnlViewDet.Controls.Add(Me.detPhone)
        Me.pnlViewDet.Controls.Add(Me.Label20)
        Me.pnlViewDet.Controls.Add(Me.Label19)
        Me.pnlViewDet.Controls.Add(Me.Label18)
        Me.pnlViewDet.Controls.Add(Me.Label17)
        Me.pnlViewDet.Controls.Add(Me.Label16)
        Me.pnlViewDet.Controls.Add(Me.Label15)
        Me.pnlViewDet.Controls.Add(Me.Label14)
        Me.pnlViewDet.Controls.Add(Me.Label22)
        Me.pnlViewDet.Controls.Add(Me.Label13)
        Me.pnlViewDet.Location = New System.Drawing.Point(8, 275)
        Me.pnlViewDet.Name = "pnlViewDet"
        Me.pnlViewDet.Size = New System.Drawing.Size(647, 184)
        Me.pnlViewDet.TabIndex = 1
        '
        'detGSTNO
        '
        Me.detGSTNO.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detGSTNO.Location = New System.Drawing.Point(414, 160)
        Me.detGSTNO.Name = "detGSTNO"
        Me.detGSTNO.Size = New System.Drawing.Size(221, 14)
        Me.detGSTNO.TabIndex = 8
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Location = New System.Drawing.Point(329, 165)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(50, 13)
        Me.Label30.TabIndex = 7
        Me.Label30.Text = "GST No"
        '
        'detState
        '
        Me.detState.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detState.Location = New System.Drawing.Point(64, 100)
        Me.detState.Name = "detState"
        Me.detState.Size = New System.Drawing.Size(255, 14)
        Me.detState.TabIndex = 6
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Location = New System.Drawing.Point(5, 105)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(37, 13)
        Me.Label29.TabIndex = 5
        Me.Label29.Text = "State"
        '
        'detTANNo
        '
        Me.detTANNo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detTANNo.Location = New System.Drawing.Point(414, 138)
        Me.detTANNo.Name = "detTANNo"
        Me.detTANNo.Size = New System.Drawing.Size(221, 14)
        Me.detTANNo.TabIndex = 4
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(329, 142)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(49, 13)
        Me.Label26.TabIndex = 3
        Me.Label26.Text = "TAN No"
        '
        'detAddress4
        '
        Me.detAddress4.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detAddress4.Location = New System.Drawing.Point(64, 78)
        Me.detAddress4.Name = "detAddress4"
        Me.detAddress4.Size = New System.Drawing.Size(255, 14)
        Me.detAddress4.TabIndex = 2
        '
        'detAddress3
        '
        Me.detAddress3.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detAddress3.Location = New System.Drawing.Point(64, 56)
        Me.detAddress3.Name = "detAddress3"
        Me.detAddress3.Size = New System.Drawing.Size(255, 14)
        Me.detAddress3.TabIndex = 2
        '
        'detAddress2
        '
        Me.detAddress2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detAddress2.Location = New System.Drawing.Point(64, 34)
        Me.detAddress2.Name = "detAddress2"
        Me.detAddress2.Size = New System.Drawing.Size(255, 14)
        Me.detAddress2.TabIndex = 2
        '
        'detAddress1
        '
        Me.detAddress1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detAddress1.Location = New System.Drawing.Point(64, 12)
        Me.detAddress1.Name = "detAddress1"
        Me.detAddress1.Size = New System.Drawing.Size(255, 14)
        Me.detAddress1.TabIndex = 2
        '
        'detLocalTaxNo
        '
        Me.detLocalTaxNo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detLocalTaxNo.Location = New System.Drawing.Point(414, 29)
        Me.detLocalTaxNo.Name = "detLocalTaxNo"
        Me.detLocalTaxNo.Size = New System.Drawing.Size(221, 14)
        Me.detLocalTaxNo.TabIndex = 1
        '
        'detAreaCode
        '
        Me.detAreaCode.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detAreaCode.Location = New System.Drawing.Point(414, 7)
        Me.detAreaCode.Name = "detAreaCode"
        Me.detAreaCode.Size = New System.Drawing.Size(221, 14)
        Me.detAreaCode.TabIndex = 1
        '
        'detTDSNo
        '
        Me.detTDSNo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detTDSNo.Location = New System.Drawing.Point(414, 117)
        Me.detTDSNo.Name = "detTDSNo"
        Me.detTDSNo.Size = New System.Drawing.Size(221, 14)
        Me.detTDSNo.TabIndex = 1
        '
        'detPANNo
        '
        Me.detPANNo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detPANNo.Location = New System.Drawing.Point(414, 95)
        Me.detPANNo.Name = "detPANNo"
        Me.detPANNo.Size = New System.Drawing.Size(221, 14)
        Me.detPANNo.TabIndex = 1
        '
        'detTINNo
        '
        Me.detTINNo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detTINNo.Location = New System.Drawing.Point(414, 73)
        Me.detTINNo.Name = "detTINNo"
        Me.detTINNo.Size = New System.Drawing.Size(221, 14)
        Me.detTINNo.TabIndex = 1
        '
        'detCSTNo
        '
        Me.detCSTNo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detCSTNo.Location = New System.Drawing.Point(414, 51)
        Me.detCSTNo.Name = "detCSTNo"
        Me.detCSTNo.Size = New System.Drawing.Size(221, 14)
        Me.detCSTNo.TabIndex = 1
        '
        'detEmail
        '
        Me.detEmail.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detEmail.Location = New System.Drawing.Point(64, 145)
        Me.detEmail.Name = "detEmail"
        Me.detEmail.Size = New System.Drawing.Size(255, 14)
        Me.detEmail.TabIndex = 1
        '
        'detPhone
        '
        Me.detPhone.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detPhone.Location = New System.Drawing.Point(64, 123)
        Me.detPhone.Name = "detPhone"
        Me.detPhone.Size = New System.Drawing.Size(255, 14)
        Me.detPhone.TabIndex = 1
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(329, 120)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(50, 13)
        Me.Label20.TabIndex = 0
        Me.Label20.Text = "TDS No"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(329, 98)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(49, 13)
        Me.Label19.TabIndex = 0
        Me.Label19.Text = "PAN No"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(329, 77)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(46, 13)
        Me.Label18.TabIndex = 0
        Me.Label18.Text = "TIN No"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(329, 54)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(50, 13)
        Me.Label17.TabIndex = 0
        Me.Label17.Text = "CST No"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(329, 31)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(80, 13)
        Me.Label16.TabIndex = 0
        Me.Label16.Text = "Local Tax No"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(329, 8)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(68, 13)
        Me.Label15.TabIndex = 0
        Me.Label15.Text = "Area Code"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(5, 147)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(38, 13)
        Me.Label14.TabIndex = 0
        Me.Label14.Text = "Email"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(5, 13)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(53, 13)
        Me.Label22.TabIndex = 0
        Me.Label22.Text = "Address"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(5, 125)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(42, 13)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "Phone"
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.AllowUserToResizeRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(6, 8)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(651, 262)
        Me.gridView.TabIndex = 0
        '
        'frmCompanyMast
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(671, 535)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmCompanyMast"
        Me.Text = "Company Creation"
        Me.tabMain.ResumeLayout(False)
        Me.tabGen.ResumeLayout(False)
        Me.pnlInputControls.ResumeLayout(False)
        Me.pnlInputControls.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabView.ResumeLayout(False)
        Me.pnlFooter.ResumeLayout(False)
        Me.pnlFooter.PerformLayout()
        Me.pnlViewDet.ResumeLayout(False)
        Me.pnlViewDet.PerformLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGen As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents pnlInputControls As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtCompanyId As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents txtCompanyName As System.Windows.Forms.TextBox
    Friend WithEvents txtAddress4 As System.Windows.Forms.TextBox
    Friend WithEvents txtAddress3 As System.Windows.Forms.TextBox
    Friend WithEvents txtAddress2 As System.Windows.Forms.TextBox
    Friend WithEvents txtDisplayOrder As System.Windows.Forms.TextBox
    Friend WithEvents txtTdsNo As System.Windows.Forms.TextBox
    Friend WithEvents txtPanNo As System.Windows.Forms.TextBox
    Friend WithEvents txtTinNo As System.Windows.Forms.TextBox
    Friend WithEvents txtAreaCode As System.Windows.Forms.TextBox
    Friend WithEvents txtCstNo As System.Windows.Forms.TextBox
    Friend WithEvents txtLocalTaxNo As System.Windows.Forms.TextBox
    Friend WithEvents txtEmail As System.Windows.Forms.TextBox
    Friend WithEvents txtPhone As System.Windows.Forms.TextBox
    Friend WithEvents txtAddress1 As System.Windows.Forms.TextBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents pnlViewDet As System.Windows.Forms.Panel
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents detPhone As System.Windows.Forms.TextBox
    Friend WithEvents detLocalTaxNo As System.Windows.Forms.TextBox
    Friend WithEvents detAreaCode As System.Windows.Forms.TextBox
    Friend WithEvents detEmail As System.Windows.Forms.TextBox
    Friend WithEvents detTDSNo As System.Windows.Forms.TextBox
    Friend WithEvents detPANNo As System.Windows.Forms.TextBox
    Friend WithEvents detTINNo As System.Windows.Forms.TextBox
    Friend WithEvents detCSTNo As System.Windows.Forms.TextBox
    Friend WithEvents detAddress4 As System.Windows.Forms.TextBox
    Friend WithEvents detAddress3 As System.Windows.Forms.TextBox
    Friend WithEvents detAddress2 As System.Windows.Forms.TextBox
    Friend WithEvents detAddress1 As System.Windows.Forms.TextBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents cmbShortKey As System.Windows.Forms.ComboBox
    Friend WithEvents cmbActive As System.Windows.Forms.ComboBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents txtTanno As System.Windows.Forms.TextBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents detTANNo As System.Windows.Forms.TextBox
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents lblCostName As System.Windows.Forms.Label
    Friend WithEvents chkcmbcostcentre As BrighttechPack.CheckedComboBox
    Friend WithEvents CmbState As System.Windows.Forms.ComboBox
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents txtGstNo As System.Windows.Forms.TextBox
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents detState As System.Windows.Forms.TextBox
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents detGSTNO As System.Windows.Forms.TextBox
    Friend WithEvents Label30 As System.Windows.Forms.Label
End Class
