<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUserMaster
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtUserId = New System.Windows.Forms.TextBox
        Me.btnSave = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtUserName_MAN = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtPassword_MAN = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtReTypePassword = New System.Windows.Forms.TextBox
        Me.btnOpen = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.GrpGeneral = New System.Windows.Forms.GroupBox
        Me.btnUserImage = New System.Windows.Forms.Button
        Me.PnlChangePwd = New System.Windows.Forms.Panel
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtChangePwd_NUM = New System.Windows.Forms.TextBox
        Me.picbxUserImage = New System.Windows.Forms.PictureBox
        Me.chkCentralizedLogin = New System.Windows.Forms.CheckBox
        Me.PnlAut = New System.Windows.Forms.Panel
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.cmbActive = New System.Windows.Forms.ComboBox
        Me.txtAuthPassword = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.PnlBottom = New System.Windows.Forms.Panel
        Me.tabView = New System.Windows.Forms.TabPage
        Me.Label8 = New System.Windows.Forms.Label
        Me.cmbCostcentrev = New System.Windows.Forms.ComboBox
        Me.btnBack = New System.Windows.Forms.Button
        Me.btnDelete = New System.Windows.Forms.Button
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.GrpGeneral.SuspendLayout()
        Me.PnlChangePwd.SuspendLayout()
        CType(Me.picbxUserImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlAut.SuspendLayout()
        Me.PnlBottom.SuspendLayout()
        Me.tabView.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(1, 43)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Cost Centre"
        '
        'txtUserId
        '
        Me.txtUserId.Location = New System.Drawing.Point(161, 23)
        Me.txtUserId.Name = "txtUserId"
        Me.txtUserId.Size = New System.Drawing.Size(91, 21)
        Me.txtUserId.TabIndex = 1
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(0, -1)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 0
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(35, 31)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(49, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "User Id"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(35, 63)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(70, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "User Name"
        '
        'txtUserName_MAN
        '
        Me.txtUserName_MAN.Location = New System.Drawing.Point(161, 55)
        Me.txtUserName_MAN.MaxLength = 50
        Me.txtUserName_MAN.Name = "txtUserName_MAN"
        Me.txtUserName_MAN.Size = New System.Drawing.Size(232, 21)
        Me.txtUserName_MAN.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(35, 95)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(61, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Password"
        '
        'txtPassword_MAN
        '
        Me.txtPassword_MAN.Location = New System.Drawing.Point(162, 87)
        Me.txtPassword_MAN.MaxLength = 20
        Me.txtPassword_MAN.Name = "txtPassword_MAN"
        Me.txtPassword_MAN.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword_MAN.Size = New System.Drawing.Size(232, 21)
        Me.txtPassword_MAN.TabIndex = 5
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(1, 7)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(109, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Re type Password"
        '
        'txtReTypePassword
        '
        Me.txtReTypePassword.Location = New System.Drawing.Point(129, 3)
        Me.txtReTypePassword.MaxLength = 20
        Me.txtReTypePassword.Name = "txtReTypePassword"
        Me.txtReTypePassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtReTypePassword.Size = New System.Drawing.Size(232, 21)
        Me.txtReTypePassword.TabIndex = 1
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(117, -1)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 1
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(234, -1)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 2
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(351, -1)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 3
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.NewToolStripMenuItem, Me.OpenToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(123, 92)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.NewToolStripMenuItem.Text = "New"
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGeneral)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(588, 347)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.GrpGeneral)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 22)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(580, 321)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'GrpGeneral
        '
        Me.GrpGeneral.Controls.Add(Me.btnUserImage)
        Me.GrpGeneral.Controls.Add(Me.txtUserId)
        Me.GrpGeneral.Controls.Add(Me.PnlChangePwd)
        Me.GrpGeneral.Controls.Add(Me.txtUserName_MAN)
        Me.GrpGeneral.Controls.Add(Me.txtPassword_MAN)
        Me.GrpGeneral.Controls.Add(Me.picbxUserImage)
        Me.GrpGeneral.Controls.Add(Me.chkCentralizedLogin)
        Me.GrpGeneral.Controls.Add(Me.Label4)
        Me.GrpGeneral.Controls.Add(Me.Label3)
        Me.GrpGeneral.Controls.Add(Me.Label2)
        Me.GrpGeneral.Controls.Add(Me.PnlAut)
        Me.GrpGeneral.Controls.Add(Me.PnlBottom)
        Me.GrpGeneral.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GrpGeneral.Location = New System.Drawing.Point(3, 3)
        Me.GrpGeneral.Name = "GrpGeneral"
        Me.GrpGeneral.Size = New System.Drawing.Size(574, 315)
        Me.GrpGeneral.TabIndex = 0
        Me.GrpGeneral.TabStop = False
        '
        'btnUserImage
        '
        Me.btnUserImage.Location = New System.Drawing.Point(400, 113)
        Me.btnUserImage.Name = "btnUserImage"
        Me.btnUserImage.Size = New System.Drawing.Size(100, 30)
        Me.btnUserImage.TabIndex = 10
        Me.btnUserImage.Text = "Select Image"
        Me.btnUserImage.UseVisualStyleBackColor = True
        '
        'PnlChangePwd
        '
        Me.PnlChangePwd.Controls.Add(Me.Label10)
        Me.PnlChangePwd.Controls.Add(Me.Label9)
        Me.PnlChangePwd.Controls.Add(Me.txtChangePwd_NUM)
        Me.PnlChangePwd.Location = New System.Drawing.Point(162, 116)
        Me.PnlChangePwd.Name = "PnlChangePwd"
        Me.PnlChangePwd.Size = New System.Drawing.Size(145, 24)
        Me.PnlChangePwd.TabIndex = 6
        Me.PnlChangePwd.Visible = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(3, 5)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(59, 13)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "Expire In"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(101, 5)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(36, 13)
        Me.Label9.TabIndex = 2
        Me.Label9.Text = "Days"
        '
        'txtChangePwd_NUM
        '
        Me.txtChangePwd_NUM.Location = New System.Drawing.Point(71, 2)
        Me.txtChangePwd_NUM.Name = "txtChangePwd_NUM"
        Me.txtChangePwd_NUM.Size = New System.Drawing.Size(26, 21)
        Me.txtChangePwd_NUM.TabIndex = 1
        '
        'picbxUserImage
        '
        Me.picbxUserImage.ErrorImage = Nothing
        Me.picbxUserImage.Image = Global.BrighttechMaster.My.Resources.Resources.noimagenew
        Me.picbxUserImage.Location = New System.Drawing.Point(400, 13)
        Me.picbxUserImage.Name = "picbxUserImage"
        Me.picbxUserImage.Size = New System.Drawing.Size(100, 97)
        Me.picbxUserImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picbxUserImage.TabIndex = 20
        Me.picbxUserImage.TabStop = False
        '
        'chkCentralizedLogin
        '
        Me.chkCentralizedLogin.AutoSize = True
        Me.chkCentralizedLogin.Location = New System.Drawing.Point(401, 153)
        Me.chkCentralizedLogin.Name = "chkCentralizedLogin"
        Me.chkCentralizedLogin.Size = New System.Drawing.Size(125, 17)
        Me.chkCentralizedLogin.TabIndex = 8
        Me.chkCentralizedLogin.Text = "Centralized Login"
        Me.chkCentralizedLogin.UseVisualStyleBackColor = True
        '
        'PnlAut
        '
        Me.PnlAut.Controls.Add(Me.chkCmbCostCentre)
        Me.PnlAut.Controls.Add(Me.Label1)
        Me.PnlAut.Controls.Add(Me.Label6)
        Me.PnlAut.Controls.Add(Me.cmbActive)
        Me.PnlAut.Controls.Add(Me.txtAuthPassword)
        Me.PnlAut.Controls.Add(Me.Label5)
        Me.PnlAut.Controls.Add(Me.txtReTypePassword)
        Me.PnlAut.Controls.Add(Me.Label7)
        Me.PnlAut.Location = New System.Drawing.Point(33, 143)
        Me.PnlAut.Name = "PnlAut"
        Me.PnlAut.Size = New System.Drawing.Size(365, 123)
        Me.PnlAut.TabIndex = 7
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(129, 35)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(232, 22)
        Me.chkCmbCostCentre.TabIndex = 3
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(1, 107)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(42, 13)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Active"
        '
        'cmbActive
        '
        Me.cmbActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbActive.FormattingEnabled = True
        Me.cmbActive.Location = New System.Drawing.Point(129, 100)
        Me.cmbActive.Name = "cmbActive"
        Me.cmbActive.Size = New System.Drawing.Size(91, 21)
        Me.cmbActive.TabIndex = 7
        '
        'txtAuthPassword
        '
        Me.txtAuthPassword.Location = New System.Drawing.Point(129, 68)
        Me.txtAuthPassword.MaxLength = 20
        Me.txtAuthPassword.Name = "txtAuthPassword"
        Me.txtAuthPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtAuthPassword.Size = New System.Drawing.Size(232, 21)
        Me.txtAuthPassword.TabIndex = 5
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(1, 75)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(99, 13)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Auth.  Password"
        '
        'PnlBottom
        '
        Me.PnlBottom.Controls.Add(Me.btnNew)
        Me.PnlBottom.Controls.Add(Me.btnExit)
        Me.PnlBottom.Controls.Add(Me.btnOpen)
        Me.PnlBottom.Controls.Add(Me.btnSave)
        Me.PnlBottom.Location = New System.Drawing.Point(32, 273)
        Me.PnlBottom.Name = "PnlBottom"
        Me.PnlBottom.Size = New System.Drawing.Size(465, 30)
        Me.PnlBottom.TabIndex = 9
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.Label8)
        Me.tabView.Controls.Add(Me.cmbCostcentrev)
        Me.tabView.Controls.Add(Me.btnBack)
        Me.tabView.Controls.Add(Me.btnDelete)
        Me.tabView.Controls.Add(Me.gridView)
        Me.tabView.Controls.Add(Me.Panel1)
        Me.tabView.Controls.Add(Me.Panel2)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(580, 321)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(216, 17)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(76, 13)
        Me.Label8.TabIndex = 10
        Me.Label8.Text = "Cost Centre"
        '
        'cmbCostcentrev
        '
        Me.cmbCostcentrev.FormattingEnabled = True
        Me.cmbCostcentrev.Location = New System.Drawing.Point(298, 17)
        Me.cmbCostcentrev.Name = "cmbCostcentrev"
        Me.cmbCostcentrev.Size = New System.Drawing.Size(221, 21)
        Me.cmbCostcentrev.TabIndex = 11
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(114, 11)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 1
        Me.btnBack.Text = "Back"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(8, 11)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnDelete.TabIndex = 1
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(3, 45)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(574, 273)
        Me.gridView.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 45)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(574, 273)
        Me.Panel1.TabIndex = 12
        '
        'Panel2
        '
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(3, 3)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(574, 42)
        Me.Panel2.TabIndex = 0
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "Select Image"
        Me.OpenFileDialog1.Filter = "JPEG(*.jpg)|*.jpg|Bitmap(*.bmp)|*.bmp|GIF(*.gif)|*.gif"
        '
        'frmUserMaster
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(588, 347)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmUserMaster"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "UserMaster"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.GrpGeneral.ResumeLayout(False)
        Me.GrpGeneral.PerformLayout()
        Me.PnlChangePwd.ResumeLayout(False)
        Me.PnlChangePwd.PerformLayout()
        CType(Me.picbxUserImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlAut.ResumeLayout(False)
        Me.PnlAut.PerformLayout()
        Me.PnlBottom.ResumeLayout(False)
        Me.tabView.ResumeLayout(False)
        Me.tabView.PerformLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtUserId As System.Windows.Forms.TextBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtUserName_MAN As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtPassword_MAN As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtReTypePassword As System.Windows.Forms.TextBox
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents cmbActive As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents chkCentralizedLogin As System.Windows.Forms.CheckBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtAuthPassword As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cmbCostcentrev As System.Windows.Forms.ComboBox
    Friend WithEvents PnlChangePwd As System.Windows.Forms.Panel
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtChangePwd_NUM As System.Windows.Forms.TextBox
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents picbxUserImage As System.Windows.Forms.PictureBox
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents btnUserImage As System.Windows.Forms.Button
    Friend WithEvents GrpGeneral As System.Windows.Forms.GroupBox
    Friend WithEvents PnlAut As System.Windows.Forms.Panel
    Friend WithEvents PnlBottom As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
End Class
