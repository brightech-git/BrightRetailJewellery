<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCreditCard
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCreditCard))
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlChit = New System.Windows.Forms.Panel()
        Me.chkAutoPost = New System.Windows.Forms.CheckBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cmbCompany = New System.Windows.Forms.ComboBox()
        Me.cmbScheme = New System.Windows.Forms.ComboBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.txtCurrency = New System.Windows.Forms.TextBox()
        Me.txtSurcharge = New System.Windows.Forms.TextBox()
        Me.txtCommision = New System.Windows.Forms.TextBox()
        Me.txtShortName = New System.Windows.Forms.TextBox()
        Me.txtCardName__Man = New System.Windows.Forms.TextBox()
        Me.cmbDefaultAcCode_OWN = New System.Windows.Forms.ComboBox()
        Me.cmbCardType = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.grpField = New System.Windows.Forms.GroupBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.cmbActive = New System.Windows.Forms.ComboBox()
        Me.pnlButtons = New System.Windows.Forms.Panel()
        Me.btnOpen = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.pnl1 = New System.Windows.Forms.Panel()
        Me.pnl2 = New System.Windows.Forms.Panel()
        Me.chkOtherAc = New System.Windows.Forms.CheckBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtDispOrder = New System.Windows.Forms.TextBox()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabGeneral = New System.Windows.Forms.TabPage()
        Me.tabView = New System.Windows.Forms.TabPage()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.grpInfo = New System.Windows.Forms.GroupBox()
        Me.DetDisp = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.DetActive = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.detSchemeName = New System.Windows.Forms.TextBox()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.detCompanyName = New System.Windows.Forms.TextBox()
        Me.detDeductAcc = New System.Windows.Forms.TextBox()
        Me.detCurrency = New System.Windows.Forms.TextBox()
        Me.detBonusAcc = New System.Windows.Forms.TextBox()
        Me.detDefaultAcc = New System.Windows.Forms.TextBox()
        Me.detPrizeAcc = New System.Windows.Forms.TextBox()
        Me.detSurcharge = New System.Windows.Forms.TextBox()
        Me.detGiftAcc = New System.Windows.Forms.TextBox()
        Me.detCommision = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.Label45 = New System.Windows.Forms.Label()
        Me.Label77 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label78 = New System.Windows.Forms.Label()
        Me.Label79 = New System.Windows.Forms.Label()
        Me.Label73 = New System.Windows.Forms.Label()
        Me.Label74 = New System.Windows.Forms.Label()
        Me.Label75 = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.pnlChit.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpField.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pnlButtons.SuspendLayout()
        Me.pnl1.SuspendLayout()
        Me.pnl2.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.grpInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.OpenToolStripMenuItem, Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
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
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'pnlChit
        '
        Me.pnlChit.Controls.Add(Me.chkAutoPost)
        Me.pnlChit.Controls.Add(Me.Label9)
        Me.pnlChit.Controls.Add(Me.Label8)
        Me.pnlChit.Controls.Add(Me.cmbCompany)
        Me.pnlChit.Controls.Add(Me.cmbScheme)
        Me.pnlChit.Location = New System.Drawing.Point(21, 322)
        Me.pnlChit.Name = "pnlChit"
        Me.pnlChit.Size = New System.Drawing.Size(481, 60)
        Me.pnlChit.TabIndex = 5
        '
        'chkAutoPost
        '
        Me.chkAutoPost.AutoSize = True
        Me.chkAutoPost.Location = New System.Drawing.Point(336, 34)
        Me.chkAutoPost.Name = "chkAutoPost"
        Me.chkAutoPost.Size = New System.Drawing.Size(80, 17)
        Me.chkAutoPost.TabIndex = 1
        Me.chkAutoPost.Text = "Auto Post"
        Me.chkAutoPost.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(8, 10)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(62, 13)
        Me.Label9.TabIndex = 2
        Me.Label9.Text = "Company"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(8, 35)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(53, 13)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "Scheme"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbCompany
        '
        Me.cmbCompany.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCompany.FormattingEnabled = True
        Me.cmbCompany.Location = New System.Drawing.Point(109, 6)
        Me.cmbCompany.Name = "cmbCompany"
        Me.cmbCompany.Size = New System.Drawing.Size(224, 21)
        Me.cmbCompany.TabIndex = 3
        '
        'cmbScheme
        '
        Me.cmbScheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbScheme.FormattingEnabled = True
        Me.cmbScheme.Location = New System.Drawing.Point(109, 31)
        Me.cmbScheme.Name = "cmbScheme"
        Me.cmbScheme.Size = New System.Drawing.Size(224, 21)
        Me.cmbScheme.TabIndex = 0
        '
        'btnSave
        '
        Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(3, 4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 0
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'txtCurrency
        '
        Me.txtCurrency.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCurrency.Location = New System.Drawing.Point(109, 57)
        Me.txtCurrency.MaxLength = 3
        Me.txtCurrency.Name = "txtCurrency"
        Me.txtCurrency.Size = New System.Drawing.Size(43, 21)
        Me.txtCurrency.TabIndex = 8
        Me.txtCurrency.Text = "999"
        '
        'txtSurcharge
        '
        Me.txtSurcharge.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSurcharge.Location = New System.Drawing.Point(262, 7)
        Me.txtSurcharge.MaxLength = 5
        Me.txtSurcharge.Name = "txtSurcharge"
        Me.txtSurcharge.Size = New System.Drawing.Size(71, 21)
        Me.txtSurcharge.TabIndex = 3
        '
        'txtCommision
        '
        Me.txtCommision.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCommision.Location = New System.Drawing.Point(109, 7)
        Me.txtCommision.MaxLength = 5
        Me.txtCommision.Name = "txtCommision"
        Me.txtCommision.Size = New System.Drawing.Size(84, 21)
        Me.txtCommision.TabIndex = 1
        '
        'txtShortName
        '
        Me.txtShortName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtShortName.Location = New System.Drawing.Point(109, 56)
        Me.txtShortName.MaxLength = 10
        Me.txtShortName.Name = "txtShortName"
        Me.txtShortName.Size = New System.Drawing.Size(84, 21)
        Me.txtShortName.TabIndex = 5
        Me.txtShortName.Text = "1234567890"
        '
        'txtCardName__Man
        '
        Me.txtCardName__Man.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCardName__Man.Location = New System.Drawing.Point(109, 31)
        Me.txtCardName__Man.MaxLength = 30
        Me.txtCardName__Man.Name = "txtCardName__Man"
        Me.txtCardName__Man.Size = New System.Drawing.Size(224, 21)
        Me.txtCardName__Man.TabIndex = 3
        Me.txtCardName__Man.Text = "123456789012345678901234567890"
        '
        'cmbDefaultAcCode_OWN
        '
        Me.cmbDefaultAcCode_OWN.FormattingEnabled = True
        Me.cmbDefaultAcCode_OWN.Location = New System.Drawing.Point(109, 32)
        Me.cmbDefaultAcCode_OWN.Name = "cmbDefaultAcCode_OWN"
        Me.cmbDefaultAcCode_OWN.Size = New System.Drawing.Size(310, 21)
        Me.cmbDefaultAcCode_OWN.TabIndex = 6
        '
        'cmbCardType
        '
        Me.cmbCardType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCardType.FormattingEnabled = True
        Me.cmbCardType.Location = New System.Drawing.Point(109, 6)
        Me.cmbCardType.Name = "cmbCardType"
        Me.cmbCardType.Size = New System.Drawing.Size(131, 21)
        Me.cmbCardType.TabIndex = 1
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(8, 60)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(60, 13)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "Currency"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(8, 35)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(99, 13)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Default AcName"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(195, 10)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(66, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Surcharge"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(8, 11)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(71, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Commision"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 60)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(75, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Short Name"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 35)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Card Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Card Type"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(3, 44)
        Me.gridView.MultiSelect = False
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 20
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(587, 300)
        Me.gridView.TabIndex = 19
        '
        'grpField
        '
        Me.grpField.BackColor = System.Drawing.Color.Transparent
        Me.grpField.Controls.Add(Me.Panel1)
        Me.grpField.Controls.Add(Me.pnlButtons)
        Me.grpField.Controls.Add(Me.pnlChit)
        Me.grpField.Controls.Add(Me.pnl1)
        Me.grpField.Controls.Add(Me.pnl2)
        Me.grpField.Location = New System.Drawing.Point(30, 20)
        Me.grpField.Name = "grpField"
        Me.grpField.Size = New System.Drawing.Size(542, 422)
        Me.grpField.TabIndex = 0
        Me.grpField.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label24)
        Me.Panel1.Controls.Add(Me.cmbActive)
        Me.Panel1.Location = New System.Drawing.Point(21, 240)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(481, 29)
        Me.Panel1.TabIndex = 8
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(8, 8)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(42, 13)
        Me.Label24.TabIndex = 2
        Me.Label24.Text = "Active"
        '
        'cmbActive
        '
        Me.cmbActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbActive.FormattingEnabled = True
        Me.cmbActive.Items.AddRange(New Object() {"Yes", "No"})
        Me.cmbActive.Location = New System.Drawing.Point(107, 4)
        Me.cmbActive.Name = "cmbActive"
        Me.cmbActive.Size = New System.Drawing.Size(70, 21)
        Me.cmbActive.TabIndex = 3
        '
        'pnlButtons
        '
        Me.pnlButtons.Controls.Add(Me.btnSave)
        Me.pnlButtons.Controls.Add(Me.btnOpen)
        Me.pnlButtons.Controls.Add(Me.btnNew)
        Me.pnlButtons.Controls.Add(Me.btnExit)
        Me.pnlButtons.Location = New System.Drawing.Point(21, 276)
        Me.pnlButtons.Name = "pnlButtons"
        Me.pnlButtons.Size = New System.Drawing.Size(481, 38)
        Me.pnlButtons.TabIndex = 4
        '
        'btnOpen
        '
        Me.btnOpen.Image = CType(resources.GetObject("btnOpen.Image"), System.Drawing.Image)
        Me.btnOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOpen.Location = New System.Drawing.Point(109, 3)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 1
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Image = CType(resources.GetObject("btnNew.Image"), System.Drawing.Image)
        Me.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNew.Location = New System.Drawing.Point(215, 3)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 2
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Image = CType(resources.GetObject("btnExit.Image"), System.Drawing.Image)
        Me.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExit.Location = New System.Drawing.Point(321, 3)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 3
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'pnl1
        '
        Me.pnl1.Controls.Add(Me.txtShortName)
        Me.pnl1.Controls.Add(Me.Label3)
        Me.pnl1.Controls.Add(Me.Label1)
        Me.pnl1.Controls.Add(Me.txtCardName__Man)
        Me.pnl1.Controls.Add(Me.cmbCardType)
        Me.pnl1.Controls.Add(Me.Label2)
        Me.pnl1.Location = New System.Drawing.Point(21, 44)
        Me.pnl1.Name = "pnl1"
        Me.pnl1.Size = New System.Drawing.Size(481, 84)
        Me.pnl1.TabIndex = 0
        '
        'pnl2
        '
        Me.pnl2.Controls.Add(Me.chkOtherAc)
        Me.pnl2.Controls.Add(Me.Label12)
        Me.pnl2.Controls.Add(Me.txtDispOrder)
        Me.pnl2.Controls.Add(Me.Label4)
        Me.pnl2.Controls.Add(Me.txtSurcharge)
        Me.pnl2.Controls.Add(Me.cmbDefaultAcCode_OWN)
        Me.pnl2.Controls.Add(Me.txtCommision)
        Me.pnl2.Controls.Add(Me.Label7)
        Me.pnl2.Controls.Add(Me.txtCurrency)
        Me.pnl2.Controls.Add(Me.Label6)
        Me.pnl2.Controls.Add(Me.Label5)
        Me.pnl2.Location = New System.Drawing.Point(21, 143)
        Me.pnl2.Name = "pnl2"
        Me.pnl2.Size = New System.Drawing.Size(481, 84)
        Me.pnl2.TabIndex = 1
        '
        'chkOtherAc
        '
        Me.chkOtherAc.AutoSize = True
        Me.chkOtherAc.Checked = True
        Me.chkOtherAc.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkOtherAc.Location = New System.Drawing.Point(339, 10)
        Me.chkOtherAc.Name = "chkOtherAc"
        Me.chkOtherAc.Size = New System.Drawing.Size(80, 17)
        Me.chkOtherAc.TabIndex = 4
        Me.chkOtherAc.Text = "Other Ac."
        Me.chkOtherAc.UseVisualStyleBackColor = True
        Me.chkOtherAc.Visible = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(195, 60)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(86, 13)
        Me.Label12.TabIndex = 9
        Me.Label12.Text = "Display Order"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtDispOrder
        '
        Me.txtDispOrder.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDispOrder.Location = New System.Drawing.Point(290, 55)
        Me.txtDispOrder.MaxLength = 3
        Me.txtDispOrder.Name = "txtDispOrder"
        Me.txtDispOrder.Size = New System.Drawing.Size(43, 21)
        Me.txtDispOrder.TabIndex = 10
        Me.txtDispOrder.Text = "999"
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
        Me.tabMain.Size = New System.Drawing.Size(601, 487)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.grpField)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 9)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(593, 474)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.gridView)
        Me.tabView.Controls.Add(Me.Panel4)
        Me.tabView.Controls.Add(Me.Panel3)
        Me.tabView.Location = New System.Drawing.Point(4, 9)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(593, 474)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.btnDelete)
        Me.Panel4.Controls.Add(Me.Label20)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel4.Location = New System.Drawing.Point(3, 3)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(587, 41)
        Me.Panel4.TabIndex = 26
        '
        'btnDelete
        '
        Me.btnDelete.Image = CType(resources.GetObject("btnDelete.Image"), System.Drawing.Image)
        Me.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDelete.Location = New System.Drawing.Point(11, 5)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnDelete.TabIndex = 22
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.ForeColor = System.Drawing.Color.Red
        Me.Label20.Location = New System.Drawing.Point(119, 14)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(153, 13)
        Me.Label20.TabIndex = 23
        Me.Label20.Text = "*Press Escape to Back"
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.grpInfo)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(3, 344)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(587, 127)
        Me.Panel3.TabIndex = 25
        '
        'grpInfo
        '
        Me.grpInfo.Controls.Add(Me.DetDisp)
        Me.grpInfo.Controls.Add(Me.Label14)
        Me.grpInfo.Controls.Add(Me.DetActive)
        Me.grpInfo.Controls.Add(Me.Label13)
        Me.grpInfo.Controls.Add(Me.detSchemeName)
        Me.grpInfo.Controls.Add(Me.lblStatus)
        Me.grpInfo.Controls.Add(Me.detCompanyName)
        Me.grpInfo.Controls.Add(Me.detDeductAcc)
        Me.grpInfo.Controls.Add(Me.detCurrency)
        Me.grpInfo.Controls.Add(Me.detBonusAcc)
        Me.grpInfo.Controls.Add(Me.detDefaultAcc)
        Me.grpInfo.Controls.Add(Me.detPrizeAcc)
        Me.grpInfo.Controls.Add(Me.detSurcharge)
        Me.grpInfo.Controls.Add(Me.detGiftAcc)
        Me.grpInfo.Controls.Add(Me.detCommision)
        Me.grpInfo.Controls.Add(Me.Label10)
        Me.grpInfo.Controls.Add(Me.Label42)
        Me.grpInfo.Controls.Add(Me.Label45)
        Me.grpInfo.Controls.Add(Me.Label77)
        Me.grpInfo.Controls.Add(Me.Label11)
        Me.grpInfo.Controls.Add(Me.Label78)
        Me.grpInfo.Controls.Add(Me.Label79)
        Me.grpInfo.Controls.Add(Me.Label73)
        Me.grpInfo.Controls.Add(Me.Label74)
        Me.grpInfo.Controls.Add(Me.Label75)
        Me.grpInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpInfo.Location = New System.Drawing.Point(0, 0)
        Me.grpInfo.Name = "grpInfo"
        Me.grpInfo.Size = New System.Drawing.Size(587, 127)
        Me.grpInfo.TabIndex = 21
        Me.grpInfo.TabStop = False
        '
        'DetDisp
        '
        Me.DetDisp.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DetDisp.Location = New System.Drawing.Point(548, 97)
        Me.DetDisp.Name = "DetDisp"
        Me.DetDisp.Size = New System.Drawing.Size(323, 14)
        Me.DetDisp.TabIndex = 24
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(446, 97)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(65, 13)
        Me.Label14.TabIndex = 23
        Me.Label14.Text = "DispOrder"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'DetActive
        '
        Me.DetActive.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.DetActive.Location = New System.Drawing.Point(107, 97)
        Me.DetActive.Name = "DetActive"
        Me.DetActive.Size = New System.Drawing.Size(323, 14)
        Me.DetActive.TabIndex = 22
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(5, 97)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(42, 13)
        Me.Label13.TabIndex = 21
        Me.Label13.Text = "Active"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'detSchemeName
        '
        Me.detSchemeName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detSchemeName.Location = New System.Drawing.Point(548, 49)
        Me.detSchemeName.Name = "detSchemeName"
        Me.detSchemeName.Size = New System.Drawing.Size(323, 14)
        Me.detSchemeName.TabIndex = 15
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(-1, 112)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(103, 13)
        Me.lblStatus.TabIndex = 20
        Me.lblStatus.Text = "*Hit Enter to Edit"
        Me.lblStatus.Visible = False
        '
        'detCompanyName
        '
        Me.detCompanyName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detCompanyName.Location = New System.Drawing.Point(548, 33)
        Me.detCompanyName.Name = "detCompanyName"
        Me.detCompanyName.Size = New System.Drawing.Size(323, 14)
        Me.detCompanyName.TabIndex = 13
        '
        'detDeductAcc
        '
        Me.detDeductAcc.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detDeductAcc.Location = New System.Drawing.Point(548, 17)
        Me.detDeductAcc.Name = "detDeductAcc"
        Me.detDeductAcc.Size = New System.Drawing.Size(323, 14)
        Me.detDeductAcc.TabIndex = 11
        '
        'detCurrency
        '
        Me.detCurrency.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detCurrency.Location = New System.Drawing.Point(107, 49)
        Me.detCurrency.Name = "detCurrency"
        Me.detCurrency.Size = New System.Drawing.Size(323, 14)
        Me.detCurrency.TabIndex = 5
        '
        'detBonusAcc
        '
        Me.detBonusAcc.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detBonusAcc.Location = New System.Drawing.Point(548, 82)
        Me.detBonusAcc.Name = "detBonusAcc"
        Me.detBonusAcc.Size = New System.Drawing.Size(323, 14)
        Me.detBonusAcc.TabIndex = 19
        '
        'detDefaultAcc
        '
        Me.detDefaultAcc.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detDefaultAcc.Location = New System.Drawing.Point(107, 65)
        Me.detDefaultAcc.Name = "detDefaultAcc"
        Me.detDefaultAcc.Size = New System.Drawing.Size(323, 14)
        Me.detDefaultAcc.TabIndex = 7
        '
        'detPrizeAcc
        '
        Me.detPrizeAcc.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detPrizeAcc.Location = New System.Drawing.Point(548, 66)
        Me.detPrizeAcc.Name = "detPrizeAcc"
        Me.detPrizeAcc.Size = New System.Drawing.Size(323, 14)
        Me.detPrizeAcc.TabIndex = 17
        '
        'detSurcharge
        '
        Me.detSurcharge.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detSurcharge.Location = New System.Drawing.Point(107, 33)
        Me.detSurcharge.Name = "detSurcharge"
        Me.detSurcharge.Size = New System.Drawing.Size(323, 14)
        Me.detSurcharge.TabIndex = 3
        '
        'detGiftAcc
        '
        Me.detGiftAcc.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detGiftAcc.Location = New System.Drawing.Point(107, 81)
        Me.detGiftAcc.Name = "detGiftAcc"
        Me.detGiftAcc.Size = New System.Drawing.Size(323, 14)
        Me.detGiftAcc.TabIndex = 9
        '
        'detCommision
        '
        Me.detCommision.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.detCommision.Location = New System.Drawing.Point(107, 17)
        Me.detCommision.Name = "detCommision"
        Me.detCommision.Size = New System.Drawing.Size(323, 14)
        Me.detCommision.TabIndex = 1
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(446, 49)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(90, 13)
        Me.Label10.TabIndex = 14
        Me.Label10.Text = "Scheme Name"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label42
        '
        Me.Label42.AutoSize = True
        Me.Label42.Location = New System.Drawing.Point(446, 33)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(99, 13)
        Me.Label42.TabIndex = 12
        Me.Label42.Text = "Company Name"
        Me.Label42.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label45
        '
        Me.Label45.AutoSize = True
        Me.Label45.Location = New System.Drawing.Point(446, 17)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(71, 13)
        Me.Label45.TabIndex = 10
        Me.Label45.Text = "Deduct Acc"
        Me.Label45.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label77
        '
        Me.Label77.AutoSize = True
        Me.Label77.Location = New System.Drawing.Point(5, 81)
        Me.Label77.Name = "Label77"
        Me.Label77.Size = New System.Drawing.Size(51, 13)
        Me.Label77.TabIndex = 8
        Me.Label77.Text = "Gift Acc"
        Me.Label77.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(5, 49)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(60, 13)
        Me.Label11.TabIndex = 4
        Me.Label11.Text = "Currency"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label78
        '
        Me.Label78.AutoSize = True
        Me.Label78.Location = New System.Drawing.Point(446, 82)
        Me.Label78.Name = "Label78"
        Me.Label78.Size = New System.Drawing.Size(66, 13)
        Me.Label78.TabIndex = 18
        Me.Label78.Text = "Bonus Acc"
        Me.Label78.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label79
        '
        Me.Label79.AutoSize = True
        Me.Label79.Location = New System.Drawing.Point(446, 66)
        Me.Label79.Name = "Label79"
        Me.Label79.Size = New System.Drawing.Size(59, 13)
        Me.Label79.TabIndex = 16
        Me.Label79.Text = "Prize Acc"
        Me.Label79.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label73
        '
        Me.Label73.AutoSize = True
        Me.Label73.Location = New System.Drawing.Point(5, 17)
        Me.Label73.Name = "Label73"
        Me.Label73.Size = New System.Drawing.Size(71, 13)
        Me.Label73.TabIndex = 0
        Me.Label73.Text = "Commision"
        Me.Label73.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label74
        '
        Me.Label74.AutoSize = True
        Me.Label74.Location = New System.Drawing.Point(5, 33)
        Me.Label74.Name = "Label74"
        Me.Label74.Size = New System.Drawing.Size(66, 13)
        Me.Label74.TabIndex = 2
        Me.Label74.Text = "Surcharge"
        Me.Label74.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label75
        '
        Me.Label75.AutoSize = True
        Me.Label75.Location = New System.Drawing.Point(5, 65)
        Me.Label75.Name = "Label75"
        Me.Label75.Size = New System.Drawing.Size(97, 13)
        Me.Label75.TabIndex = 6
        Me.Label75.Text = "Default Account"
        Me.Label75.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'frmCreditCard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(601, 487)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmCreditCard"
        Me.Text = "CreditCard"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.pnlChit.ResumeLayout(False)
        Me.pnlChit.PerformLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpField.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlButtons.ResumeLayout(False)
        Me.pnl1.ResumeLayout(False)
        Me.pnl1.PerformLayout()
        Me.pnl2.ResumeLayout(False)
        Me.pnl2.PerformLayout()
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.tabView.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.grpInfo.ResumeLayout(False)
        Me.grpInfo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtCardName__Man As System.Windows.Forms.TextBox
    Friend WithEvents cmbCardType As System.Windows.Forms.ComboBox
    Friend WithEvents txtCurrency As System.Windows.Forms.TextBox
    Friend WithEvents txtSurcharge As System.Windows.Forms.TextBox
    Friend WithEvents txtCommision As System.Windows.Forms.TextBox
    Friend WithEvents txtShortName As System.Windows.Forms.TextBox
    Friend WithEvents cmbDefaultAcCode_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents cmbScheme As System.Windows.Forms.ComboBox
    Friend WithEvents cmbCompany As System.Windows.Forms.ComboBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents pnlChit As System.Windows.Forms.Panel
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents grpField As System.Windows.Forms.GroupBox
    Friend WithEvents pnlButtons As System.Windows.Forms.Panel
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents pnl1 As System.Windows.Forms.Panel
    Friend WithEvents pnl2 As System.Windows.Forms.Panel
    Friend WithEvents grpInfo As System.Windows.Forms.GroupBox
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents detCompanyName As System.Windows.Forms.TextBox
    Friend WithEvents detDeductAcc As System.Windows.Forms.TextBox
    Friend WithEvents detBonusAcc As System.Windows.Forms.TextBox
    Friend WithEvents detDefaultAcc As System.Windows.Forms.TextBox
    Friend WithEvents detPrizeAcc As System.Windows.Forms.TextBox
    Friend WithEvents detSurcharge As System.Windows.Forms.TextBox
    Friend WithEvents detGiftAcc As System.Windows.Forms.TextBox
    Friend WithEvents detCommision As System.Windows.Forms.TextBox
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents Label77 As System.Windows.Forms.Label
    Friend WithEvents Label78 As System.Windows.Forms.Label
    Friend WithEvents Label79 As System.Windows.Forms.Label
    Friend WithEvents Label73 As System.Windows.Forms.Label
    Friend WithEvents Label74 As System.Windows.Forms.Label
    Friend WithEvents Label75 As System.Windows.Forms.Label
    Friend WithEvents detSchemeName As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents detCurrency As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents chkAutoPost As System.Windows.Forms.CheckBox
    Friend WithEvents cmbActive As System.Windows.Forms.ComboBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtDispOrder As System.Windows.Forms.TextBox
    Friend WithEvents chkOtherAc As System.Windows.Forms.CheckBox
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents DetActive As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents DetDisp As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
End Class
