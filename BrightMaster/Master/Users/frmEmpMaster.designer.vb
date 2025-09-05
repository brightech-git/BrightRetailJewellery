<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEmpMaster
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
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.grpControl = New System.Windows.Forms.GroupBox
        Me.btnUserImage = New System.Windows.Forms.Button
        Me.chkCentralizedLogin = New System.Windows.Forms.CheckBox
        Me.picbxUserImage = New System.Windows.Forms.PictureBox
        Me.cmbCostcentre_MAN = New System.Windows.Forms.ComboBox
        Me.txtAliasName = New System.Windows.Forms.TextBox
        Me.pnlDiscDetails = New System.Windows.Forms.Panel
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.txtDiscount_Per = New System.Windows.Forms.TextBox
        Me.txtDiscount_Amt = New System.Windows.Forms.TextBox
        Me.dtpDoj = New BrighttechPack.DatePicker(Me.components)
        Me.chkDoj = New System.Windows.Forms.CheckBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.cmbIncentive = New System.Windows.Forms.ComboBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.txtSTAmount_Amt = New System.Windows.Forms.TextBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.txtSTWeight_Wet = New System.Windows.Forms.TextBox
        Me.txtSTPiece_Num = New System.Windows.Forms.TextBox
        Me.cmbActive = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtEmpId_Num_Man = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnOpen = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.cmbDiscAuthorize = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmbItemCounter = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.cmbDesignation_Man = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.txtAddress4 = New System.Windows.Forms.TextBox
        Me.txtAddress3 = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtAddress2 = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtAddress1 = New System.Windows.Forms.TextBox
        Me.txtEmpName__Man = New System.Windows.Forms.TextBox
        Me.btnDelete = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.lblStatus = New System.Windows.Forms.Label
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.tabView = New System.Windows.Forms.TabPage
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.btnBack = New System.Windows.Forms.Button
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmbGridShortCut.SuspendLayout()
        Me.grpControl.SuspendLayout()
        CType(Me.picbxUserImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlDiscDetails.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.SuspendLayout()
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.ContextMenuStrip = Me.cmbGridShortCut
        Me.gridView.Location = New System.Drawing.Point(11, 50)
        Me.gridView.MultiSelect = False
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(992, 541)
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
        'grpControl
        '
        Me.grpControl.BackColor = System.Drawing.Color.Transparent
        Me.grpControl.Controls.Add(Me.btnUserImage)
        Me.grpControl.Controls.Add(Me.chkCentralizedLogin)
        Me.grpControl.Controls.Add(Me.picbxUserImage)
        Me.grpControl.Controls.Add(Me.cmbCostcentre_MAN)
        Me.grpControl.Controls.Add(Me.txtAliasName)
        Me.grpControl.Controls.Add(Me.pnlDiscDetails)
        Me.grpControl.Controls.Add(Me.dtpDoj)
        Me.grpControl.Controls.Add(Me.chkDoj)
        Me.grpControl.Controls.Add(Me.GroupBox1)
        Me.grpControl.Controls.Add(Me.cmbActive)
        Me.grpControl.Controls.Add(Me.Label6)
        Me.grpControl.Controls.Add(Me.txtEmpId_Num_Man)
        Me.grpControl.Controls.Add(Me.Label1)
        Me.grpControl.Controls.Add(Me.btnExit)
        Me.grpControl.Controls.Add(Me.btnNew)
        Me.grpControl.Controls.Add(Me.btnOpen)
        Me.grpControl.Controls.Add(Me.btnSave)
        Me.grpControl.Controls.Add(Me.cmbDiscAuthorize)
        Me.grpControl.Controls.Add(Me.Label3)
        Me.grpControl.Controls.Add(Me.Label2)
        Me.grpControl.Controls.Add(Me.cmbItemCounter)
        Me.grpControl.Controls.Add(Me.Label9)
        Me.grpControl.Controls.Add(Me.cmbDesignation_Man)
        Me.grpControl.Controls.Add(Me.Label5)
        Me.grpControl.Controls.Add(Me.Label10)
        Me.grpControl.Controls.Add(Me.txtAddress4)
        Me.grpControl.Controls.Add(Me.txtAddress3)
        Me.grpControl.Controls.Add(Me.Label4)
        Me.grpControl.Controls.Add(Me.txtAddress2)
        Me.grpControl.Controls.Add(Me.Label8)
        Me.grpControl.Controls.Add(Me.txtAddress1)
        Me.grpControl.Controls.Add(Me.txtEmpName__Man)
        Me.grpControl.Location = New System.Drawing.Point(139, 30)
        Me.grpControl.Name = "grpControl"
        Me.grpControl.Size = New System.Drawing.Size(634, 574)
        Me.grpControl.TabIndex = 0
        Me.grpControl.TabStop = False
        '
        'btnUserImage
        '
        Me.btnUserImage.Location = New System.Drawing.Point(498, 130)
        Me.btnUserImage.Name = "btnUserImage"
        Me.btnUserImage.Size = New System.Drawing.Size(100, 30)
        Me.btnUserImage.TabIndex = 26
        Me.btnUserImage.Text = "Select Image"
        Me.btnUserImage.UseVisualStyleBackColor = True
        '
        'chkCentralizedLogin
        '
        Me.chkCentralizedLogin.AutoSize = True
        Me.chkCentralizedLogin.Location = New System.Drawing.Point(303, 16)
        Me.chkCentralizedLogin.Name = "chkCentralizedLogin"
        Me.chkCentralizedLogin.Size = New System.Drawing.Size(125, 17)
        Me.chkCentralizedLogin.TabIndex = 2
        Me.chkCentralizedLogin.Text = "Centralized Login"
        Me.chkCentralizedLogin.UseVisualStyleBackColor = True
        '
        'picbxUserImage
        '
        Me.picbxUserImage.ErrorImage = Nothing
        Me.picbxUserImage.Image = My.Resources.Resources.noimagenew
        Me.picbxUserImage.Location = New System.Drawing.Point(498, 20)
        Me.picbxUserImage.Name = "picbxUserImage"
        Me.picbxUserImage.Size = New System.Drawing.Size(100, 105)
        Me.picbxUserImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picbxUserImage.TabIndex = 25
        Me.picbxUserImage.TabStop = False
        '
        'cmbCostcentre_MAN
        '
        Me.cmbCostcentre_MAN.FormattingEnabled = True
        Me.cmbCostcentre_MAN.Location = New System.Drawing.Point(104, 14)
        Me.cmbCostcentre_MAN.Name = "cmbCostcentre_MAN"
        Me.cmbCostcentre_MAN.Size = New System.Drawing.Size(193, 21)
        Me.cmbCostcentre_MAN.TabIndex = 1
        '
        'txtAliasName
        '
        Me.txtAliasName.Location = New System.Drawing.Point(104, 112)
        Me.txtAliasName.MaxLength = 20
        Me.txtAliasName.Name = "txtAliasName"
        Me.txtAliasName.Size = New System.Drawing.Size(224, 21)
        Me.txtAliasName.TabIndex = 7
        '
        'pnlDiscDetails
        '
        Me.pnlDiscDetails.Controls.Add(Me.Label12)
        Me.pnlDiscDetails.Controls.Add(Me.Label11)
        Me.pnlDiscDetails.Controls.Add(Me.Label14)
        Me.pnlDiscDetails.Controls.Add(Me.txtPassword)
        Me.pnlDiscDetails.Controls.Add(Me.txtDiscount_Per)
        Me.pnlDiscDetails.Controls.Add(Me.txtDiscount_Amt)
        Me.pnlDiscDetails.Location = New System.Drawing.Point(353, 343)
        Me.pnlDiscDetails.Name = "pnlDiscDetails"
        Me.pnlDiscDetails.Size = New System.Drawing.Size(94, 55)
        Me.pnlDiscDetails.TabIndex = 15
        Me.pnlDiscDetails.Visible = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(4, 10)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(61, 13)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "Password"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(158, 33)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(51, 13)
        Me.Label11.TabIndex = 4
        Me.Label11.Text = "Amount"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(4, 33)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(72, 13)
        Me.Label14.TabIndex = 2
        Me.Label14.Text = "Discount %"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtPassword
        '
        Me.txtPassword.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtPassword.Location = New System.Drawing.Point(110, 2)
        Me.txtPassword.MaxLength = 10
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(203, 21)
        Me.txtPassword.TabIndex = 1
        '
        'txtDiscount_Per
        '
        Me.txtDiscount_Per.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDiscount_Per.Location = New System.Drawing.Point(110, 29)
        Me.txtDiscount_Per.MaxLength = 10
        Me.txtDiscount_Per.Name = "txtDiscount_Per"
        Me.txtDiscount_Per.Size = New System.Drawing.Size(42, 21)
        Me.txtDiscount_Per.TabIndex = 3
        '
        'txtDiscount_Amt
        '
        Me.txtDiscount_Amt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDiscount_Amt.Location = New System.Drawing.Point(215, 30)
        Me.txtDiscount_Amt.MaxLength = 10
        Me.txtDiscount_Amt.Name = "txtDiscount_Amt"
        Me.txtDiscount_Amt.Size = New System.Drawing.Size(98, 21)
        Me.txtDiscount_Amt.TabIndex = 5
        '
        'dtpDoj
        '
        Me.dtpDoj.Location = New System.Drawing.Point(104, 147)
        Me.dtpDoj.Mask = "##/##/####"
        Me.dtpDoj.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpDoj.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpDoj.Name = "dtpDoj"
        Me.dtpDoj.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpDoj.Size = New System.Drawing.Size(93, 21)
        Me.dtpDoj.TabIndex = 9
        Me.dtpDoj.Text = "07/03/9998"
        Me.dtpDoj.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'chkDoj
        '
        Me.chkDoj.AutoSize = True
        Me.chkDoj.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkDoj.Checked = True
        Me.chkDoj.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDoj.Location = New System.Drawing.Point(6, 149)
        Me.chkDoj.Name = "chkDoj"
        Me.chkDoj.Size = New System.Drawing.Size(46, 17)
        Me.chkDoj.TabIndex = 8
        Me.chkDoj.Text = "Doj"
        Me.chkDoj.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmbIncentive)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.Label15)
        Me.GroupBox1.Controls.Add(Me.txtSTAmount_Amt)
        Me.GroupBox1.Controls.Add(Me.Label17)
        Me.GroupBox1.Controls.Add(Me.Label16)
        Me.GroupBox1.Controls.Add(Me.txtSTWeight_Wet)
        Me.GroupBox1.Controls.Add(Me.txtSTPiece_Num)
        Me.GroupBox1.Location = New System.Drawing.Point(11, 410)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(328, 81)
        Me.GroupBox1.TabIndex = 23
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Sales Target"
        '
        'cmbIncentive
        '
        Me.cmbIncentive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbIncentive.FormattingEnabled = True
        Me.cmbIncentive.Location = New System.Drawing.Point(236, 50)
        Me.cmbIncentive.Name = "cmbIncentive"
        Me.cmbIncentive.Size = New System.Drawing.Size(83, 21)
        Me.cmbIncentive.TabIndex = 7
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(6, 21)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(37, 13)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "Piece"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(6, 53)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(46, 13)
        Me.Label15.TabIndex = 4
        Me.Label15.Text = "Weight"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtSTAmount_Amt
        '
        Me.txtSTAmount_Amt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSTAmount_Amt.Location = New System.Drawing.Point(236, 18)
        Me.txtSTAmount_Amt.MaxLength = 10
        Me.txtSTAmount_Amt.Name = "txtSTAmount_Amt"
        Me.txtSTAmount_Amt.Size = New System.Drawing.Size(83, 21)
        Me.txtSTAmount_Amt.TabIndex = 3
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(179, 53)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(60, 13)
        Me.Label17.TabIndex = 6
        Me.Label17.Text = "Incentive"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(179, 21)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(51, 13)
        Me.Label16.TabIndex = 2
        Me.Label16.Text = "Amount"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtSTWeight_Wet
        '
        Me.txtSTWeight_Wet.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSTWeight_Wet.Location = New System.Drawing.Point(95, 50)
        Me.txtSTWeight_Wet.MaxLength = 10
        Me.txtSTWeight_Wet.Name = "txtSTWeight_Wet"
        Me.txtSTWeight_Wet.Size = New System.Drawing.Size(83, 21)
        Me.txtSTWeight_Wet.TabIndex = 5
        '
        'txtSTPiece_Num
        '
        Me.txtSTPiece_Num.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSTPiece_Num.Location = New System.Drawing.Point(95, 18)
        Me.txtSTPiece_Num.MaxLength = 10
        Me.txtSTPiece_Num.Name = "txtSTPiece_Num"
        Me.txtSTPiece_Num.Size = New System.Drawing.Size(83, 21)
        Me.txtSTPiece_Num.TabIndex = 1
        '
        'cmbActive
        '
        Me.cmbActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbActive.FormattingEnabled = True
        Me.cmbActive.Location = New System.Drawing.Point(245, 371)
        Me.cmbActive.Name = "cmbActive"
        Me.cmbActive.Size = New System.Drawing.Size(83, 21)
        Me.cmbActive.TabIndex = 22
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(193, 375)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(42, 13)
        Me.Label6.TabIndex = 21
        Me.Label6.Text = "Active"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtEmpId_Num_Man
        '
        Me.txtEmpId_Num_Man.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtEmpId_Num_Man.Location = New System.Drawing.Point(104, 45)
        Me.txtEmpId_Num_Man.MaxLength = 7
        Me.txtEmpId_Num_Man.Name = "txtEmpId_Num_Man"
        Me.txtEmpId_Num_Man.Size = New System.Drawing.Size(98, 21)
        Me.txtEmpId_Num_Man.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 49)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Emp Id"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(422, 520)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 27
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(314, 520)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 26
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(206, 519)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 25
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(98, 519)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 24
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'cmbDiscAuthorize
        '
        Me.cmbDiscAuthorize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDiscAuthorize.FormattingEnabled = True
        Me.cmbDiscAuthorize.Location = New System.Drawing.Point(104, 371)
        Me.cmbDiscAuthorize.Name = "cmbDiscAuthorize"
        Me.cmbDiscAuthorize.Size = New System.Drawing.Size(83, 21)
        Me.cmbDiscAuthorize.TabIndex = 20
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 115)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(71, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Alias Name"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 81)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(69, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Emp Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbItemCounter
        '
        Me.cmbItemCounter.FormattingEnabled = True
        Me.cmbItemCounter.Location = New System.Drawing.Point(104, 339)
        Me.cmbItemCounter.Name = "cmbItemCounter"
        Me.cmbItemCounter.Size = New System.Drawing.Size(224, 21)
        Me.cmbItemCounter.TabIndex = 18
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 343)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(84, 13)
        Me.Label9.TabIndex = 17
        Me.Label9.Text = "Item Counter"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbDesignation_Man
        '
        Me.cmbDesignation_Man.FormattingEnabled = True
        Me.cmbDesignation_Man.Location = New System.Drawing.Point(104, 307)
        Me.cmbDesignation_Man.Name = "cmbDesignation_Man"
        Me.cmbDesignation_Man.Size = New System.Drawing.Size(224, 21)
        Me.cmbDesignation_Man.TabIndex = 16
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 17)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(76, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Cost Centre"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(6, 375)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(89, 13)
        Me.Label10.TabIndex = 19
        Me.Label10.Text = "Disc Authorize"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtAddress4
        '
        Me.txtAddress4.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAddress4.Location = New System.Drawing.Point(104, 275)
        Me.txtAddress4.MaxLength = 50
        Me.txtAddress4.Name = "txtAddress4"
        Me.txtAddress4.Size = New System.Drawing.Size(361, 21)
        Me.txtAddress4.TabIndex = 14
        '
        'txtAddress3
        '
        Me.txtAddress3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAddress3.Location = New System.Drawing.Point(104, 243)
        Me.txtAddress3.MaxLength = 50
        Me.txtAddress3.Name = "txtAddress3"
        Me.txtAddress3.Size = New System.Drawing.Size(361, 21)
        Me.txtAddress3.TabIndex = 13
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 183)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Address"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtAddress2
        '
        Me.txtAddress2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAddress2.Location = New System.Drawing.Point(104, 211)
        Me.txtAddress2.MaxLength = 50
        Me.txtAddress2.Name = "txtAddress2"
        Me.txtAddress2.Size = New System.Drawing.Size(361, 21)
        Me.txtAddress2.TabIndex = 12
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 311)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(74, 13)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "Designation"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtAddress1
        '
        Me.txtAddress1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAddress1.Location = New System.Drawing.Point(104, 179)
        Me.txtAddress1.MaxLength = 50
        Me.txtAddress1.Name = "txtAddress1"
        Me.txtAddress1.Size = New System.Drawing.Size(361, 21)
        Me.txtAddress1.TabIndex = 11
        '
        'txtEmpName__Man
        '
        Me.txtEmpName__Man.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtEmpName__Man.Location = New System.Drawing.Point(104, 77)
        Me.txtEmpName__Man.MaxLength = 50
        Me.txtEmpName__Man.Name = "txtEmpName__Man"
        Me.txtEmpName__Man.Size = New System.Drawing.Size(361, 21)
        Me.txtEmpName__Man.TabIndex = 5
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(11, 14)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnDelete.TabIndex = 25
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
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
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(8, 594)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(103, 13)
        Me.lblStatus.TabIndex = 26
        Me.lblStatus.Text = "*Hit Enter to Edit"
        Me.lblStatus.Visible = False
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGeneral)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1020, 638)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.grpControl)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 22)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(1012, 612)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.btnPrint)
        Me.tabView.Controls.Add(Me.btnExport)
        Me.tabView.Controls.Add(Me.btnBack)
        Me.tabView.Controls.Add(Me.btnDelete)
        Me.tabView.Controls.Add(Me.gridView)
        Me.tabView.Controls.Add(Me.lblStatus)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1012, 612)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(326, 15)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 29
        Me.btnPrint.Text = "Print [P]"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(222, 15)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 28
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(117, 14)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 27
        Me.btnBack.Text = "&Back"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "Select an Image"
        Me.OpenFileDialog1.Filter = "JPEG(*.jpg)|*.jpg|Bitmap(*.bmp)|*.bmp|GIF(*.gif)|*.gif"
        '
        'frmEmpMaster
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1020, 638)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmEmpMaster"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Employee Master"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmbGridShortCut.ResumeLayout(False)
        Me.grpControl.ResumeLayout(False)
        Me.grpControl.PerformLayout()
        CType(Me.picbxUserImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlDiscDetails.ResumeLayout(False)
        Me.pnlDiscDetails.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.tabView.ResumeLayout(False)
        Me.tabView.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents pnlDiscDetails As System.Windows.Forms.Panel
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtDiscount_Per As System.Windows.Forms.TextBox
    Friend WithEvents txtDiscount_Amt As System.Windows.Forms.TextBox
    Friend WithEvents cmbIncentive As System.Windows.Forms.ComboBox
    Friend WithEvents cmbDiscAuthorize As System.Windows.Forms.ComboBox
    Friend WithEvents cmbItemCounter As System.Windows.Forms.ComboBox
    Friend WithEvents cmbDesignation_Man As System.Windows.Forms.ComboBox
    Friend WithEvents txtAddress4 As System.Windows.Forms.TextBox
    Friend WithEvents txtAddress3 As System.Windows.Forms.TextBox
    Friend WithEvents txtAddress2 As System.Windows.Forms.TextBox
    Friend WithEvents txtAddress1 As System.Windows.Forms.TextBox
    Friend WithEvents txtSTAmount_Amt As System.Windows.Forms.TextBox
    Friend WithEvents txtSTWeight_Wet As System.Windows.Forms.TextBox
    Friend WithEvents txtSTPiece_Num As System.Windows.Forms.TextBox
    Friend WithEvents txtEmpName__Man As System.Windows.Forms.TextBox
    Friend WithEvents txtEmpId_Num_Man As System.Windows.Forms.TextBox
    Friend WithEvents grpControl As System.Windows.Forms.GroupBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbActive As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents chkDoj As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents dtpDoj As BrighttechPack.DatePicker
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtAliasName As System.Windows.Forms.TextBox
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbCostcentre_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents chkCentralizedLogin As System.Windows.Forms.CheckBox
    Friend WithEvents btnUserImage As System.Windows.Forms.Button
    Friend WithEvents picbxUserImage As System.Windows.Forms.PictureBox
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
End Class
