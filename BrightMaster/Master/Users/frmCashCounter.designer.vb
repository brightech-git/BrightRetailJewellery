<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCashCounter
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
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.btnExit = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.btnNew = New System.Windows.Forms.Button
        Me.Label9 = New System.Windows.Forms.Label
        Me.btnOpen = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.btnSave = New System.Windows.Forms.Button
        Me.Label10 = New System.Windows.Forms.Label
        Me.cmbSepBillNo = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.cmbAmountLock = New System.Windows.Forms.ComboBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.cmbRateTax = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.cmbManualBillNo = New System.Windows.Forms.ComboBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.cmbCostCentre = New System.Windows.Forms.ComboBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.txtDiscountAmt_AMT = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtStatus = New System.Windows.Forms.TextBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.txtSalesBillNo_NUM = New System.Windows.Forms.TextBox
        Me.txtSalesSerialNo_NUM = New System.Windows.Forms.TextBox
        Me.txtDiscountPer_PER = New System.Windows.Forms.TextBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.txtPurchaseBillNo_NUM = New System.Windows.Forms.TextBox
        Me.txtPurchaseSerialNo_NUM = New System.Windows.Forms.TextBox
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.txtCounterId_NUM_MAN = New System.Windows.Forms.TextBox
        Me.txtCounterName = New System.Windows.Forms.TextBox
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnDelete = New System.Windows.Forms.Button
        Me.lblStatus = New System.Windows.Forms.Label
        Me.cmbCashAc = New System.Windows.Forms.ComboBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.txtNodeID = New System.Windows.Forms.TextBox
        Me.cmbCounterType = New System.Windows.Forms.ComboBox
        Me.Label19 = New System.Windows.Forms.Label
        Me.cmbActive = New System.Windows.Forms.ComboBox
        Me.Label20 = New System.Windows.Forms.Label
        Me.chkCmbItemCounter = New BrighttechPack.CheckedComboBox
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(15, 181)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(872, 288)
        Me.gridView.TabIndex = 37
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(692, 145)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 35
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Location = New System.Drawing.Point(12, 130)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(61, 13)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Password"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(592, 145)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 34
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Location = New System.Drawing.Point(12, 158)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(87, 13)
        Me.Label9.TabIndex = 16
        Me.Label9.Text = "Manual Bill No"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(492, 145)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 33
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Location = New System.Drawing.Point(178, 17)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(90, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Counter Name"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(392, 145)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 32
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Location = New System.Drawing.Point(732, 76)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(69, 13)
        Me.Label10.TabIndex = 26
        Me.Label10.Text = "Sep Bill No"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label10.Visible = False
        '
        'cmbSepBillNo
        '
        Me.cmbSepBillNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSepBillNo.FormattingEnabled = True
        Me.cmbSepBillNo.Items.AddRange(New Object() {"YES", "NO"})
        Me.cmbSepBillNo.Location = New System.Drawing.Point(818, 72)
        Me.cmbSepBillNo.Name = "cmbSepBillNo"
        Me.cmbSepBillNo.Size = New System.Drawing.Size(69, 21)
        Me.cmbSepBillNo.TabIndex = 27
        Me.cmbSepBillNo.Visible = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Location = New System.Drawing.Point(409, 50)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(43, 13)
        Me.Label6.TabIndex = 18
        Me.Label6.Text = "Status"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label6.Visible = False
        '
        'cmbAmountLock
        '
        Me.cmbAmountLock.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbAmountLock.FormattingEnabled = True
        Me.cmbAmountLock.Items.AddRange(New Object() {"YES", "NO"})
        Me.cmbAmountLock.Location = New System.Drawing.Point(818, 97)
        Me.cmbAmountLock.Name = "cmbAmountLock"
        Me.cmbAmountLock.Size = New System.Drawing.Size(69, 21)
        Me.cmbAmountLock.TabIndex = 29
        Me.cmbAmountLock.Visible = False
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Location = New System.Drawing.Point(732, 122)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(58, 13)
        Me.Label11.TabIndex = 30
        Me.Label11.Text = "Rate Tax"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label11.Visible = False
        '
        'cmbRateTax
        '
        Me.cmbRateTax.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbRateTax.FormattingEnabled = True
        Me.cmbRateTax.Items.AddRange(New Object() {"YES", "NO"})
        Me.cmbRateTax.Location = New System.Drawing.Point(818, 118)
        Me.cmbRateTax.Name = "cmbRateTax"
        Me.cmbRateTax.Size = New System.Drawing.Size(69, 21)
        Me.cmbRateTax.TabIndex = 31
        Me.cmbRateTax.Visible = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Location = New System.Drawing.Point(12, 74)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "CostCentre"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbManualBillNo
        '
        Me.cmbManualBillNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbManualBillNo.FormattingEnabled = True
        Me.cmbManualBillNo.Items.AddRange(New Object() {"YES", "NO"})
        Me.cmbManualBillNo.Location = New System.Drawing.Point(118, 154)
        Me.cmbManualBillNo.Name = "cmbManualBillNo"
        Me.cmbManualBillNo.Size = New System.Drawing.Size(69, 21)
        Me.cmbManualBillNo.TabIndex = 17
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Location = New System.Drawing.Point(537, 50)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(79, 13)
        Me.Label7.TabIndex = 20
        Me.Label7.Text = "Discount Per"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label7.Visible = False
        '
        'cmbCostCentre
        '
        Me.cmbCostCentre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCostCentre.FormattingEnabled = True
        Me.cmbCostCentre.Location = New System.Drawing.Point(118, 70)
        Me.cmbCostCentre.Name = "cmbCostCentre"
        Me.cmbCostCentre.Size = New System.Drawing.Size(211, 21)
        Me.cmbCostCentre.TabIndex = 9
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.BackColor = System.Drawing.Color.Transparent
        Me.Label13.Location = New System.Drawing.Point(732, 101)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(81, 13)
        Me.Label13.TabIndex = 28
        Me.Label13.Text = "Amount Lock"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label13.Visible = False
        '
        'txtDiscountAmt_AMT
        '
        Me.txtDiscountAmt_AMT.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDiscountAmt_AMT.Location = New System.Drawing.Point(824, 45)
        Me.txtDiscountAmt_AMT.MaxLength = 8
        Me.txtDiscountAmt_AMT.Name = "txtDiscountAmt_AMT"
        Me.txtDiscountAmt_AMT.Size = New System.Drawing.Size(69, 21)
        Me.txtDiscountAmt_AMT.TabIndex = 23
        Me.txtDiscountAmt_AMT.Visible = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Location = New System.Drawing.Point(731, 49)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(83, 13)
        Me.Label8.TabIndex = 22
        Me.Label8.Text = "Discount Amt"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label8.Visible = False
        '
        'txtStatus
        '
        Me.txtStatus.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtStatus.Location = New System.Drawing.Point(466, 46)
        Me.txtStatus.MaxLength = 7
        Me.txtStatus.Name = "txtStatus"
        Me.txtStatus.Size = New System.Drawing.Size(69, 21)
        Me.txtStatus.TabIndex = 19
        Me.txtStatus.Text = "1234567"
        Me.txtStatus.Visible = False
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.txtSalesBillNo_NUM)
        Me.GroupBox1.Controls.Add(Me.txtSalesSerialNo_NUM)
        Me.GroupBox1.Location = New System.Drawing.Point(433, 70)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(151, 69)
        Me.GroupBox1.TabIndex = 24
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Sales"
        Me.GroupBox1.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(59, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Serial No"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(6, 45)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(43, 13)
        Me.Label12.TabIndex = 2
        Me.Label12.Text = "Bill No"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtSalesBillNo_NUM
        '
        Me.txtSalesBillNo_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSalesBillNo_NUM.Location = New System.Drawing.Point(73, 41)
        Me.txtSalesBillNo_NUM.Name = "txtSalesBillNo_NUM"
        Me.txtSalesBillNo_NUM.Size = New System.Drawing.Size(69, 21)
        Me.txtSalesBillNo_NUM.TabIndex = 3
        '
        'txtSalesSerialNo_NUM
        '
        Me.txtSalesSerialNo_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSalesSerialNo_NUM.Location = New System.Drawing.Point(73, 15)
        Me.txtSalesSerialNo_NUM.Name = "txtSalesSerialNo_NUM"
        Me.txtSalesSerialNo_NUM.Size = New System.Drawing.Size(69, 21)
        Me.txtSalesSerialNo_NUM.TabIndex = 1
        '
        'txtDiscountPer_PER
        '
        Me.txtDiscountPer_PER.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDiscountPer_PER.Location = New System.Drawing.Point(643, 46)
        Me.txtDiscountPer_PER.MaxLength = 8
        Me.txtDiscountPer_PER.Name = "txtDiscountPer_PER"
        Me.txtDiscountPer_PER.Size = New System.Drawing.Size(69, 21)
        Me.txtDiscountPer_PER.TabIndex = 21
        Me.txtDiscountPer_PER.Text = "12345678"
        Me.txtDiscountPer_PER.Visible = False
        '
        'GroupBox2
        '
        Me.GroupBox2.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox2.Controls.Add(Me.Label14)
        Me.GroupBox2.Controls.Add(Me.Label15)
        Me.GroupBox2.Controls.Add(Me.txtPurchaseBillNo_NUM)
        Me.GroupBox2.Controls.Add(Me.txtPurchaseSerialNo_NUM)
        Me.GroupBox2.Location = New System.Drawing.Point(583, 70)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(152, 69)
        Me.GroupBox2.TabIndex = 25
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Purchase"
        Me.GroupBox2.Visible = False
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(6, 19)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(59, 13)
        Me.Label14.TabIndex = 0
        Me.Label14.Text = "Serial No"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(6, 45)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(43, 13)
        Me.Label15.TabIndex = 2
        Me.Label15.Text = "Bill No"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtPurchaseBillNo_NUM
        '
        Me.txtPurchaseBillNo_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtPurchaseBillNo_NUM.Location = New System.Drawing.Point(73, 41)
        Me.txtPurchaseBillNo_NUM.Name = "txtPurchaseBillNo_NUM"
        Me.txtPurchaseBillNo_NUM.Size = New System.Drawing.Size(69, 21)
        Me.txtPurchaseBillNo_NUM.TabIndex = 3
        '
        'txtPurchaseSerialNo_NUM
        '
        Me.txtPurchaseSerialNo_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtPurchaseSerialNo_NUM.Location = New System.Drawing.Point(73, 15)
        Me.txtPurchaseSerialNo_NUM.Name = "txtPurchaseSerialNo_NUM"
        Me.txtPurchaseSerialNo_NUM.Size = New System.Drawing.Size(69, 21)
        Me.txtPurchaseSerialNo_NUM.TabIndex = 1
        '
        'txtPassword
        '
        Me.txtPassword.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtPassword.Location = New System.Drawing.Point(118, 126)
        Me.txtPassword.MaxLength = 6
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(69, 21)
        Me.txtPassword.TabIndex = 13
        '
        'txtCounterId_NUM_MAN
        '
        Me.txtCounterId_NUM_MAN.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCounterId_NUM_MAN.Location = New System.Drawing.Point(118, 14)
        Me.txtCounterId_NUM_MAN.MaxLength = 5
        Me.txtCounterId_NUM_MAN.Name = "txtCounterId_NUM_MAN"
        Me.txtCounterId_NUM_MAN.Size = New System.Drawing.Size(54, 21)
        Me.txtCounterId_NUM_MAN.TabIndex = 1
        Me.txtCounterId_NUM_MAN.Text = "12345"
        '
        'txtCounterName
        '
        Me.txtCounterName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCounterName.Location = New System.Drawing.Point(284, 13)
        Me.txtCounterName.MaxLength = 20
        Me.txtCounterName.Name = "txtCounterName"
        Me.txtCounterName.Size = New System.Drawing.Size(182, 21)
        Me.txtCounterName.TabIndex = 3
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
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Location = New System.Drawing.Point(12, 18)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(69, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Counter Id"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(792, 145)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnDelete.TabIndex = 36
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(12, 470)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(103, 13)
        Me.lblStatus.TabIndex = 38
        Me.lblStatus.Text = "*Hit Enter to Edit"
        Me.lblStatus.Visible = False
        '
        'cmbCashAc
        '
        Me.cmbCashAc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCashAc.FormattingEnabled = True
        Me.cmbCashAc.Location = New System.Drawing.Point(118, 43)
        Me.cmbCashAc.Name = "cmbCashAc"
        Me.cmbCashAc.Size = New System.Drawing.Size(348, 21)
        Me.cmbCashAc.TabIndex = 4
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.BackColor = System.Drawing.Color.Transparent
        Me.Label16.Location = New System.Drawing.Point(12, 46)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(62, 13)
        Me.Label16.TabIndex = 6
        Me.Label16.Text = "Cash A/C"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.BackColor = System.Drawing.Color.Transparent
        Me.Label17.Location = New System.Drawing.Point(12, 102)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(84, 13)
        Me.Label17.TabIndex = 10
        Me.Label17.Text = "Item Counter"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.BackColor = System.Drawing.Color.Transparent
        Me.Label18.Location = New System.Drawing.Point(190, 130)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(55, 13)
        Me.Label18.TabIndex = 14
        Me.Label18.Text = "Node-ID"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label18.Visible = False
        '
        'txtNodeID
        '
        Me.txtNodeID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNodeID.Location = New System.Drawing.Point(244, 126)
        Me.txtNodeID.MaxLength = 6
        Me.txtNodeID.Name = "txtNodeID"
        Me.txtNodeID.Size = New System.Drawing.Size(85, 21)
        Me.txtNodeID.TabIndex = 15
        Me.txtNodeID.Visible = False
        '
        'cmbCounterType
        '
        Me.cmbCounterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCounterType.FormattingEnabled = True
        Me.cmbCounterType.Items.AddRange(New Object() {"", "COLLECTION", "BILLING"})
        Me.cmbCounterType.Location = New System.Drawing.Point(563, 14)
        Me.cmbCounterType.Name = "cmbCounterType"
        Me.cmbCounterType.Size = New System.Drawing.Size(105, 21)
        Me.cmbCounterType.TabIndex = 5
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.BackColor = System.Drawing.Color.Transparent
        Me.Label19.Location = New System.Drawing.Point(472, 16)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(85, 13)
        Me.Label19.TabIndex = 4
        Me.Label19.Text = "Counter Type"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbActive
        '
        Me.cmbActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbActive.FormattingEnabled = True
        Me.cmbActive.Location = New System.Drawing.Point(244, 154)
        Me.cmbActive.Name = "cmbActive"
        Me.cmbActive.Size = New System.Drawing.Size(85, 21)
        Me.cmbActive.TabIndex = 19
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(191, 158)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(42, 13)
        Me.Label20.TabIndex = 18
        Me.Label20.Text = "Active"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbItemCounter
        '
        Me.chkCmbItemCounter.CheckOnClick = True
        Me.chkCmbItemCounter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbItemCounter.DropDownHeight = 1
        Me.chkCmbItemCounter.FormattingEnabled = True
        Me.chkCmbItemCounter.IntegralHeight = False
        Me.chkCmbItemCounter.Location = New System.Drawing.Point(118, 98)
        Me.chkCmbItemCounter.Name = "chkCmbItemCounter"
        Me.chkCmbItemCounter.Size = New System.Drawing.Size(211, 22)
        Me.chkCmbItemCounter.TabIndex = 11
        Me.chkCmbItemCounter.ValueSeparator = ", "
        '
        'frmCashCounter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(902, 492)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.cmbActive)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.txtNodeID)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.cmbCounterType)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.chkCmbItemCounter)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.gridView)
        Me.Controls.Add(Me.cmbCashAc)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.txtCounterName)
        Me.Controls.Add(Me.btnNew)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtCounterId_NUM_MAN)
        Me.Controls.Add(Me.btnOpen)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.cmbSepBillNo)
        Me.Controls.Add(Me.txtDiscountPer_PER)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtStatus)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.cmbCostCentre)
        Me.Controls.Add(Me.cmbAmountLock)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.txtDiscountAmt_AMT)
        Me.Controls.Add(Me.cmbRateTax)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.cmbManualBillNo)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmCashCounter"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CashCounter"
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cmbCostCentre As System.Windows.Forms.ComboBox
    Friend WithEvents txtDiscountAmt_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtStatus As System.Windows.Forms.TextBox
    Friend WithEvents txtDiscountPer_PER As System.Windows.Forms.TextBox
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtCounterName As System.Windows.Forms.TextBox
    Friend WithEvents txtCounterId_NUM_MAN As System.Windows.Forms.TextBox
    Friend WithEvents txtSalesBillNo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtSalesSerialNo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents cmbSepBillNo As System.Windows.Forms.ComboBox
    Friend WithEvents cmbAmountLock As System.Windows.Forms.ComboBox
    Friend WithEvents cmbRateTax As System.Windows.Forms.ComboBox
    Friend WithEvents cmbManualBillNo As System.Windows.Forms.ComboBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtPurchaseBillNo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtPurchaseSerialNo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents cmbCashAc As System.Windows.Forms.ComboBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents chkCmbItemCounter As BrighttechPack.CheckedComboBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents txtNodeID As System.Windows.Forms.TextBox
    Friend WithEvents cmbCounterType As System.Windows.Forms.ComboBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents cmbActive As System.Windows.Forms.ComboBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
End Class
