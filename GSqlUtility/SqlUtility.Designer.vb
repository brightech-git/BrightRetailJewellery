<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SqlUtility
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SqlUtility))
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.RbtnRecover = New System.Windows.Forms.RadioButton
        Me.RbtnRelease = New System.Windows.Forms.RadioButton
        Me.RbtnCollation = New System.Windows.Forms.RadioButton
        Me.btnRefreshServer = New System.Windows.Forms.Button
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.txtLoginName = New System.Windows.Forms.TextBox
        Me.cmbLoginType = New System.Windows.Forms.ComboBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.cmbServers = New System.Windows.Forms.ComboBox
        Me.rdbShrinkLogfile = New System.Windows.Forms.RadioButton
        Me.rdbDetach = New System.Windows.Forms.RadioButton
        Me.rdbAttach = New System.Windows.Forms.RadioButton
        Me.rdbRestore = New System.Windows.Forms.RadioButton
        Me.rdbBackUp = New System.Windows.Forms.RadioButton
        Me.tabShrink = New System.Windows.Forms.TabPage
        Me.ChkLstbShrinkDbName = New System.Windows.Forms.CheckedListBox
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.dudShrink = New System.Windows.Forms.DomainUpDown
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.tabAttach = New System.Windows.Forms.TabPage
        Me.tabctrlAttachDetach = New System.Windows.Forms.TabControl
        Me.tabpgParticular = New System.Windows.Forms.TabPage
        Me.PnlAttachGrid = New System.Windows.Forms.Panel
        Me.GridViewAttach = New System.Windows.Forms.DataGridView
        Me.txtAttachDatabaseName = New System.Windows.Forms.TextBox
        Me.txtAttachLdfLocation = New System.Windows.Forms.TextBox
        Me.txtAttachMdfLocation = New System.Windows.Forms.TextBox
        Me.btnAttachBrowseSource = New System.Windows.Forms.Button
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.txtAttachSourcePath = New System.Windows.Forms.TextBox
        Me.Label19 = New System.Windows.Forms.Label
        Me.tabpgAll = New System.Windows.Forms.TabPage
        Me.chkSelectAll = New System.Windows.Forms.CheckBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.dgvAttach = New System.Windows.Forms.DataGridView
        Me.Label2 = New System.Windows.Forms.Label
        Me.PictureBox4 = New System.Windows.Forms.PictureBox
        Me.tabDetach = New System.Windows.Forms.TabPage
        Me.tabctrlDetach = New System.Windows.Forms.TabControl
        Me.tabpgdparticular = New System.Windows.Forms.TabPage
        Me.chkcmbDetachDatabase = New BrighttechPack.CheckedComboBox
        Me.Label21 = New System.Windows.Forms.Label
        Me.cmbDetachDatabase = New System.Windows.Forms.ComboBox
        Me.Label23 = New System.Windows.Forms.Label
        Me.Label24 = New System.Windows.Forms.Label
        Me.tabpgdAll = New System.Windows.Forms.TabPage
        Me.chkdSelectAll = New System.Windows.Forms.CheckBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.dgvDetach = New System.Windows.Forms.DataGridView
        Me.PictureBox5 = New System.Windows.Forms.PictureBox
        Me.tabBackUp = New System.Windows.Forms.TabPage
        Me.chkBackUpIsDBSystemPath = New System.Windows.Forms.CheckBox
        Me.gridBackUpStatus = New System.Windows.Forms.DataGridView
        Me.pBackUpBar = New System.Windows.Forms.ProgressBar
        Me.btnBackUpBrowsePath = New System.Windows.Forms.Button
        Me.Label14 = New System.Windows.Forms.Label
        Me.txtBackUpPath = New System.Windows.Forms.TextBox
        Me.rbtdayname = New System.Windows.Forms.RadioButton
        Me.rbtBackUpDayWise = New System.Windows.Forms.RadioButton
        Me.PictureBox3 = New System.Windows.Forms.PictureBox
        Me.rbtBackUpTimeWise = New System.Windows.Forms.RadioButton
        Me.Label4 = New System.Windows.Forms.Label
        Me.chkLstBackUpDataBases = New System.Windows.Forms.CheckedListBox
        Me.tabRestore = New System.Windows.Forms.TabPage
        Me.Label22 = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.btnRestorePathBrowse = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtRestorePath = New System.Windows.Forms.TextBox
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar
        Me.gridRestoreFiles = New System.Windows.Forms.DataGridView
        Me.btnAddBackUpFiles = New System.Windows.Forms.Button
        Me.PictureBox6 = New System.Windows.Forms.PictureBox
        Me.Collation = New System.Windows.Forms.TabPage
        Me.Label28 = New System.Windows.Forms.Label
        Me.Label29 = New System.Windows.Forms.Label
        Me.Cmbcurcollation = New System.Windows.Forms.ComboBox
        Me.BtnUpdateCollaton = New System.Windows.Forms.Button
        Me.Label27 = New System.Windows.Forms.Label
        Me.CmbCollationName = New System.Windows.Forms.ComboBox
        Me.Label26 = New System.Windows.Forms.Label
        Me.CmbDataBaseName = New System.Windows.Forms.ComboBox
        Me.Label25 = New System.Windows.Forms.Label
        Me.PictureBox7 = New System.Windows.Forms.PictureBox
        Me.tabReleaseMemory = New System.Windows.Forms.TabPage
        Me.LblDelete = New System.Windows.Forms.Label
        Me.dtpSyncDate = New System.Windows.Forms.DateTimePicker
        Me.LblRDBaseName = New System.Windows.Forms.Label
        Me.CmbRDataBase = New System.Windows.Forms.ComboBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.chkDroptempdb = New System.Windows.Forms.RadioButton
        Me.ChkSyncData = New System.Windows.Forms.RadioButton
        Me.CkhRebuidDatabase = New System.Windows.Forms.RadioButton
        Me.chkIndex = New System.Windows.Forms.RadioButton
        Me.chkMemory = New System.Windows.Forms.RadioButton
        Me.Label30 = New System.Windows.Forms.Label
        Me.Label31 = New System.Windows.Forms.Label
        Me.PictureBox8 = New System.Windows.Forms.PictureBox
        Me.tabRecover = New System.Windows.Forms.TabPage
        Me.Label32 = New System.Windows.Forms.Label
        Me.CmbRcDatabase = New System.Windows.Forms.ComboBox
        Me.Label33 = New System.Windows.Forms.Label
        Me.Label34 = New System.Windows.Forms.Label
        Me.PictureBox9 = New System.Windows.Forms.PictureBox
        Me.tabView = New System.Windows.Forms.TabPage
        Me.GrpMain = New System.Windows.Forms.GroupBox
        Me.GridView = New System.Windows.Forms.DataGridView
        Me.GrpHead = New System.Windows.Forms.GroupBox
        Me.ChkAll = New System.Windows.Forms.CheckBox
        Me.btnUpdate = New System.Windows.Forms.Button
        Me.btnVBack = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnNext = New System.Windows.Forms.Button
        Me.btnBack = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabShrink.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabAttach.SuspendLayout()
        Me.tabctrlAttachDetach.SuspendLayout()
        Me.tabpgParticular.SuspendLayout()
        Me.PnlAttachGrid.SuspendLayout()
        CType(Me.GridViewAttach, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabpgAll.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.dgvAttach, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabDetach.SuspendLayout()
        Me.tabctrlDetach.SuspendLayout()
        Me.tabpgdparticular.SuspendLayout()
        Me.tabpgdAll.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.dgvDetach, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabBackUp.SuspendLayout()
        CType(Me.gridBackUpStatus, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabRestore.SuspendLayout()
        CType(Me.gridRestoreFiles, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Collation.SuspendLayout()
        CType(Me.PictureBox7, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabReleaseMemory.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.PictureBox8, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabRecover.SuspendLayout()
        CType(Me.PictureBox9, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabView.SuspendLayout()
        Me.GrpMain.SuspendLayout()
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GrpHead.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGeneral)
        Me.tabMain.Controls.Add(Me.tabShrink)
        Me.tabMain.Controls.Add(Me.tabAttach)
        Me.tabMain.Controls.Add(Me.tabDetach)
        Me.tabMain.Controls.Add(Me.tabBackUp)
        Me.tabMain.Controls.Add(Me.tabRestore)
        Me.tabMain.Controls.Add(Me.Collation)
        Me.tabMain.Controls.Add(Me.tabReleaseMemory)
        Me.tabMain.Controls.Add(Me.tabRecover)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.ItemSize = New System.Drawing.Size(405, 18)
        Me.tabMain.Location = New System.Drawing.Point(7, 6)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(448, 370)
        Me.tabMain.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.BackColor = System.Drawing.Color.White
        Me.tabGeneral.Controls.Add(Me.RbtnRecover)
        Me.tabGeneral.Controls.Add(Me.RbtnRelease)
        Me.tabGeneral.Controls.Add(Me.RbtnCollation)
        Me.tabGeneral.Controls.Add(Me.btnRefreshServer)
        Me.tabGeneral.Controls.Add(Me.PictureBox2)
        Me.tabGeneral.Controls.Add(Me.txtPassword)
        Me.tabGeneral.Controls.Add(Me.txtLoginName)
        Me.tabGeneral.Controls.Add(Me.cmbLoginType)
        Me.tabGeneral.Controls.Add(Me.Label11)
        Me.tabGeneral.Controls.Add(Me.Label10)
        Me.tabGeneral.Controls.Add(Me.Label9)
        Me.tabGeneral.Controls.Add(Me.Label8)
        Me.tabGeneral.Controls.Add(Me.cmbServers)
        Me.tabGeneral.Controls.Add(Me.rdbShrinkLogfile)
        Me.tabGeneral.Controls.Add(Me.rdbDetach)
        Me.tabGeneral.Controls.Add(Me.rdbAttach)
        Me.tabGeneral.Controls.Add(Me.rdbRestore)
        Me.tabGeneral.Controls.Add(Me.rdbBackUp)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 22)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(440, 344)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'RbtnRecover
        '
        Me.RbtnRecover.AutoSize = True
        Me.RbtnRecover.Location = New System.Drawing.Point(270, 266)
        Me.RbtnRecover.Name = "RbtnRecover"
        Me.RbtnRecover.Size = New System.Drawing.Size(130, 17)
        Me.RbtnRecover.TabIndex = 16
        Me.RbtnRecover.Text = "Recover Database" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.RbtnRecover.UseVisualStyleBackColor = True
        '
        'RbtnRelease
        '
        Me.RbtnRelease.AutoSize = True
        Me.RbtnRelease.Location = New System.Drawing.Point(270, 293)
        Me.RbtnRelease.Name = "RbtnRelease"
        Me.RbtnRelease.Size = New System.Drawing.Size(166, 17)
        Me.RbtnRelease.TabIndex = 15
        Me.RbtnRelease.Text = "Release Unused Memory"
        Me.RbtnRelease.UseVisualStyleBackColor = True
        '
        'RbtnCollation
        '
        Me.RbtnCollation.AutoSize = True
        Me.RbtnCollation.Location = New System.Drawing.Point(270, 239)
        Me.RbtnCollation.Name = "RbtnCollation"
        Me.RbtnCollation.Size = New System.Drawing.Size(123, 17)
        Me.RbtnCollation.TabIndex = 14
        Me.RbtnCollation.Text = "Change Collation"
        Me.RbtnCollation.UseVisualStyleBackColor = True
        '
        'btnRefreshServer
        '
        Me.btnRefreshServer.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRefreshServer.ForeColor = System.Drawing.Color.Red
        Me.btnRefreshServer.Location = New System.Drawing.Point(362, 77)
        Me.btnRefreshServer.Name = "btnRefreshServer"
        Me.btnRefreshServer.Size = New System.Drawing.Size(25, 23)
        Me.btnRefreshServer.TabIndex = 2
        Me.btnRefreshServer.Text = "?"
        Me.btnRefreshServer.UseVisualStyleBackColor = True
        '
        'PictureBox2
        '
        Me.PictureBox2.Dock = System.Windows.Forms.DockStyle.Left
        Me.PictureBox2.Image = Global.BrightUtility.My.Resources.Resources.ImportExport
        Me.PictureBox2.Location = New System.Drawing.Point(3, 3)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(149, 338)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox2.TabIndex = 10
        Me.PictureBox2.TabStop = False
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(234, 162)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(148, 21)
        Me.txtPassword.TabIndex = 8
        '
        'txtLoginName
        '
        Me.txtLoginName.Location = New System.Drawing.Point(234, 134)
        Me.txtLoginName.Name = "txtLoginName"
        Me.txtLoginName.Size = New System.Drawing.Size(148, 21)
        Me.txtLoginName.TabIndex = 6
        Me.txtLoginName.Text = "sa"
        '
        'cmbLoginType
        '
        Me.cmbLoginType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbLoginType.FormattingEnabled = True
        Me.cmbLoginType.Items.AddRange(New Object() {"Server", "Windows"})
        Me.cmbLoginType.Location = New System.Drawing.Point(234, 106)
        Me.cmbLoginType.Name = "cmbLoginType"
        Me.cmbLoginType.Size = New System.Drawing.Size(104, 21)
        Me.cmbLoginType.TabIndex = 4
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(158, 166)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(61, 13)
        Me.Label11.TabIndex = 7
        Me.Label11.Text = "Password"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(158, 138)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(74, 13)
        Me.Label10.TabIndex = 5
        Me.Label10.Text = "Login Name"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(158, 110)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(71, 13)
        Me.Label9.TabIndex = 3
        Me.Label9.Text = "Login Mode"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(158, 82)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(46, 13)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Server"
        '
        'cmbServers
        '
        Me.cmbServers.FormattingEnabled = True
        Me.cmbServers.Location = New System.Drawing.Point(234, 78)
        Me.cmbServers.Name = "cmbServers"
        Me.cmbServers.Size = New System.Drawing.Size(126, 21)
        Me.cmbServers.TabIndex = 1
        '
        'rdbShrinkLogfile
        '
        Me.rdbShrinkLogfile.AutoSize = True
        Me.rdbShrinkLogfile.Checked = True
        Me.rdbShrinkLogfile.Location = New System.Drawing.Point(270, 211)
        Me.rdbShrinkLogfile.Name = "rdbShrinkLogfile"
        Me.rdbShrinkLogfile.Size = New System.Drawing.Size(103, 17)
        Me.rdbShrinkLogfile.TabIndex = 13
        Me.rdbShrinkLogfile.TabStop = True
        Me.rdbShrinkLogfile.Text = "Shrink Logfile"
        Me.rdbShrinkLogfile.UseVisualStyleBackColor = True
        '
        'rdbDetach
        '
        Me.rdbDetach.AutoSize = True
        Me.rdbDetach.Location = New System.Drawing.Point(170, 239)
        Me.rdbDetach.Name = "rdbDetach"
        Me.rdbDetach.Size = New System.Drawing.Size(65, 17)
        Me.rdbDetach.TabIndex = 12
        Me.rdbDetach.Text = "Detach"
        Me.rdbDetach.UseVisualStyleBackColor = True
        '
        'rdbAttach
        '
        Me.rdbAttach.AutoSize = True
        Me.rdbAttach.Location = New System.Drawing.Point(170, 211)
        Me.rdbAttach.Name = "rdbAttach"
        Me.rdbAttach.Size = New System.Drawing.Size(61, 17)
        Me.rdbAttach.TabIndex = 11
        Me.rdbAttach.Text = "Attach"
        Me.rdbAttach.UseVisualStyleBackColor = True
        '
        'rdbRestore
        '
        Me.rdbRestore.AutoSize = True
        Me.rdbRestore.Location = New System.Drawing.Point(170, 266)
        Me.rdbRestore.Name = "rdbRestore"
        Me.rdbRestore.Size = New System.Drawing.Size(69, 17)
        Me.rdbRestore.TabIndex = 10
        Me.rdbRestore.Text = "Restore"
        Me.rdbRestore.UseVisualStyleBackColor = True
        '
        'rdbBackUp
        '
        Me.rdbBackUp.AutoSize = True
        Me.rdbBackUp.Location = New System.Drawing.Point(170, 293)
        Me.rdbBackUp.Name = "rdbBackUp"
        Me.rdbBackUp.Size = New System.Drawing.Size(72, 17)
        Me.rdbBackUp.TabIndex = 9
        Me.rdbBackUp.Text = "Back Up"
        Me.rdbBackUp.UseVisualStyleBackColor = True
        '
        'tabShrink
        '
        Me.tabShrink.BackColor = System.Drawing.Color.White
        Me.tabShrink.Controls.Add(Me.ChkLstbShrinkDbName)
        Me.tabShrink.Controls.Add(Me.PictureBox1)
        Me.tabShrink.Controls.Add(Me.Label13)
        Me.tabShrink.Controls.Add(Me.Label7)
        Me.tabShrink.Controls.Add(Me.Label6)
        Me.tabShrink.Controls.Add(Me.dudShrink)
        Me.tabShrink.Controls.Add(Me.Label12)
        Me.tabShrink.Controls.Add(Me.Label5)
        Me.tabShrink.Controls.Add(Me.Label1)
        Me.tabShrink.Location = New System.Drawing.Point(4, 22)
        Me.tabShrink.Name = "tabShrink"
        Me.tabShrink.Padding = New System.Windows.Forms.Padding(3)
        Me.tabShrink.Size = New System.Drawing.Size(440, 344)
        Me.tabShrink.TabIndex = 1
        Me.tabShrink.Text = "Shrink Logfile"
        Me.tabShrink.UseVisualStyleBackColor = True
        '
        'ChkLstbShrinkDbName
        '
        Me.ChkLstbShrinkDbName.FormattingEnabled = True
        Me.ChkLstbShrinkDbName.Location = New System.Drawing.Point(235, 182)
        Me.ChkLstbShrinkDbName.Name = "ChkLstbShrinkDbName"
        Me.ChkLstbShrinkDbName.Size = New System.Drawing.Size(137, 100)
        Me.ChkLstbShrinkDbName.TabIndex = 12
        '
        'PictureBox1
        '
        Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Left
        Me.PictureBox1.Image = Global.BrightUtility.My.Resources.Resources.ImportExport
        Me.PictureBox1.Location = New System.Drawing.Point(3, 3)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(149, 338)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 11
        Me.PictureBox1.TabStop = False
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(301, 295)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(64, 13)
        Me.Label13.TabIndex = 5
        Me.Label13.Text = "MB in size"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(158, 295)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(31, 13)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "Size"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(158, 203)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(61, 13)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Database"
        '
        'dudShrink
        '
        Me.dudShrink.Items.Add("2")
        Me.dudShrink.Items.Add("3")
        Me.dudShrink.Items.Add("4")
        Me.dudShrink.Items.Add("5")
        Me.dudShrink.Items.Add("6")
        Me.dudShrink.Items.Add("7")
        Me.dudShrink.Items.Add("8")
        Me.dudShrink.Items.Add("10")
        Me.dudShrink.Location = New System.Drawing.Point(235, 291)
        Me.dudShrink.Name = "dudShrink"
        Me.dudShrink.Size = New System.Drawing.Size(63, 21)
        Me.dudShrink.TabIndex = 4
        Me.dudShrink.Text = "2"
        Me.dudShrink.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(158, 137)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(214, 42)
        Me.Label12.TabIndex = 1
        Me.Label12.Text = "Back up the SQL Server database immediately after shrinking.  (highly recommended" & _
            ")."
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(158, 72)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(229, 45)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "This wizard helps you to shrink the transaction logs of the SQL server databases " & _
            "with specified size."
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(154, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(237, 43)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Welcome to the Transaction Log Shrink Wizard"
        '
        'tabAttach
        '
        Me.tabAttach.BackColor = System.Drawing.Color.White
        Me.tabAttach.Controls.Add(Me.tabctrlAttachDetach)
        Me.tabAttach.Controls.Add(Me.Label2)
        Me.tabAttach.Controls.Add(Me.PictureBox4)
        Me.tabAttach.Location = New System.Drawing.Point(4, 22)
        Me.tabAttach.Name = "tabAttach"
        Me.tabAttach.Padding = New System.Windows.Forms.Padding(3)
        Me.tabAttach.Size = New System.Drawing.Size(440, 344)
        Me.tabAttach.TabIndex = 2
        Me.tabAttach.Text = "Attach"
        Me.tabAttach.UseVisualStyleBackColor = True
        '
        'tabctrlAttachDetach
        '
        Me.tabctrlAttachDetach.Controls.Add(Me.tabpgParticular)
        Me.tabctrlAttachDetach.Controls.Add(Me.tabpgAll)
        Me.tabctrlAttachDetach.Location = New System.Drawing.Point(155, 4)
        Me.tabctrlAttachDetach.Name = "tabctrlAttachDetach"
        Me.tabctrlAttachDetach.SelectedIndex = 0
        Me.tabctrlAttachDetach.Size = New System.Drawing.Size(289, 344)
        Me.tabctrlAttachDetach.TabIndex = 22
        '
        'tabpgParticular
        '
        Me.tabpgParticular.Controls.Add(Me.PnlAttachGrid)
        Me.tabpgParticular.Controls.Add(Me.txtAttachDatabaseName)
        Me.tabpgParticular.Controls.Add(Me.txtAttachLdfLocation)
        Me.tabpgParticular.Controls.Add(Me.txtAttachMdfLocation)
        Me.tabpgParticular.Controls.Add(Me.btnAttachBrowseSource)
        Me.tabpgParticular.Controls.Add(Me.Label16)
        Me.tabpgParticular.Controls.Add(Me.Label17)
        Me.tabpgParticular.Controls.Add(Me.Label15)
        Me.tabpgParticular.Controls.Add(Me.txtAttachSourcePath)
        Me.tabpgParticular.Controls.Add(Me.Label19)
        Me.tabpgParticular.Location = New System.Drawing.Point(4, 22)
        Me.tabpgParticular.Name = "tabpgParticular"
        Me.tabpgParticular.Padding = New System.Windows.Forms.Padding(3)
        Me.tabpgParticular.Size = New System.Drawing.Size(281, 318)
        Me.tabpgParticular.TabIndex = 0
        Me.tabpgParticular.Text = "Particular"
        Me.tabpgParticular.UseVisualStyleBackColor = True
        '
        'PnlAttachGrid
        '
        Me.PnlAttachGrid.Controls.Add(Me.GridViewAttach)
        Me.PnlAttachGrid.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PnlAttachGrid.Location = New System.Drawing.Point(3, 226)
        Me.PnlAttachGrid.Name = "PnlAttachGrid"
        Me.PnlAttachGrid.Size = New System.Drawing.Size(275, 89)
        Me.PnlAttachGrid.TabIndex = 40
        Me.PnlAttachGrid.Visible = False
        '
        'GridViewAttach
        '
        Me.GridViewAttach.AllowUserToAddRows = False
        Me.GridViewAttach.AllowUserToDeleteRows = False
        Me.GridViewAttach.BackgroundColor = System.Drawing.SystemColors.Window
        Me.GridViewAttach.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridViewAttach.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridViewAttach.Location = New System.Drawing.Point(0, 0)
        Me.GridViewAttach.Name = "GridViewAttach"
        Me.GridViewAttach.ReadOnly = True
        Me.GridViewAttach.RowHeadersVisible = False
        Me.GridViewAttach.RowTemplate.Height = 18
        Me.GridViewAttach.Size = New System.Drawing.Size(275, 89)
        Me.GridViewAttach.TabIndex = 0
        '
        'txtAttachDatabaseName
        '
        Me.txtAttachDatabaseName.Location = New System.Drawing.Point(117, 103)
        Me.txtAttachDatabaseName.Name = "txtAttachDatabaseName"
        Me.txtAttachDatabaseName.Size = New System.Drawing.Size(128, 21)
        Me.txtAttachDatabaseName.TabIndex = 39
        '
        'txtAttachLdfLocation
        '
        Me.txtAttachLdfLocation.Location = New System.Drawing.Point(16, 185)
        Me.txtAttachLdfLocation.Name = "txtAttachLdfLocation"
        Me.txtAttachLdfLocation.Size = New System.Drawing.Size(229, 21)
        Me.txtAttachLdfLocation.TabIndex = 37
        '
        'txtAttachMdfLocation
        '
        Me.txtAttachMdfLocation.Location = New System.Drawing.Point(16, 145)
        Me.txtAttachMdfLocation.Name = "txtAttachMdfLocation"
        Me.txtAttachMdfLocation.Size = New System.Drawing.Size(229, 21)
        Me.txtAttachMdfLocation.TabIndex = 38
        '
        'btnAttachBrowseSource
        '
        Me.btnAttachBrowseSource.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAttachBrowseSource.ForeColor = System.Drawing.Color.Red
        Me.btnAttachBrowseSource.Location = New System.Drawing.Point(221, 64)
        Me.btnAttachBrowseSource.Name = "btnAttachBrowseSource"
        Me.btnAttachBrowseSource.Size = New System.Drawing.Size(28, 24)
        Me.btnAttachBrowseSource.TabIndex = 36
        Me.btnAttachBrowseSource.Text = ".."
        Me.btnAttachBrowseSource.UseVisualStyleBackColor = True
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(13, 169)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(79, 13)
        Me.Label16.TabIndex = 32
        Me.Label16.Text = "LDF Location"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(13, 106)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(98, 13)
        Me.Label17.TabIndex = 33
        Me.Label17.Text = "Database Name"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(14, 50)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(55, 13)
        Me.Label15.TabIndex = 34
        Me.Label15.Text = "File Path"
        '
        'txtAttachSourcePath
        '
        Me.txtAttachSourcePath.Location = New System.Drawing.Point(14, 66)
        Me.txtAttachSourcePath.Name = "txtAttachSourcePath"
        Me.txtAttachSourcePath.Size = New System.Drawing.Size(205, 21)
        Me.txtAttachSourcePath.TabIndex = 35
        '
        'Label19
        '
        Me.Label19.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(11, 3)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(237, 43)
        Me.Label19.TabIndex = 30
        Me.Label19.Text = "Welcome to the Database Attach Wizard"
        '
        'tabpgAll
        '
        Me.tabpgAll.Controls.Add(Me.chkSelectAll)
        Me.tabpgAll.Controls.Add(Me.Panel2)
        Me.tabpgAll.Location = New System.Drawing.Point(4, 22)
        Me.tabpgAll.Name = "tabpgAll"
        Me.tabpgAll.Padding = New System.Windows.Forms.Padding(3)
        Me.tabpgAll.Size = New System.Drawing.Size(281, 318)
        Me.tabpgAll.TabIndex = 1
        Me.tabpgAll.Text = "All"
        Me.tabpgAll.UseVisualStyleBackColor = True
        '
        'chkSelectAll
        '
        Me.chkSelectAll.AutoSize = True
        Me.chkSelectAll.Location = New System.Drawing.Point(3, 6)
        Me.chkSelectAll.Name = "chkSelectAll"
        Me.chkSelectAll.Size = New System.Drawing.Size(79, 17)
        Me.chkSelectAll.TabIndex = 7
        Me.chkSelectAll.Text = "Select All"
        Me.chkSelectAll.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.dgvAttach)
        Me.Panel2.Location = New System.Drawing.Point(3, 29)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(275, 252)
        Me.Panel2.TabIndex = 6
        '
        'dgvAttach
        '
        Me.dgvAttach.AllowUserToAddRows = False
        Me.dgvAttach.AllowUserToDeleteRows = False
        Me.dgvAttach.AllowUserToOrderColumns = True
        Me.dgvAttach.AllowUserToResizeColumns = False
        Me.dgvAttach.AllowUserToResizeRows = False
        Me.dgvAttach.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAttach.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvAttach.Location = New System.Drawing.Point(0, 0)
        Me.dgvAttach.Name = "dgvAttach"
        Me.dgvAttach.RowHeadersVisible = False
        Me.dgvAttach.Size = New System.Drawing.Size(275, 252)
        Me.dgvAttach.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(168, 191)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(82, 13)
        Me.Label2.TabIndex = 21
        Me.Label2.Text = "MDF Location"
        '
        'PictureBox4
        '
        Me.PictureBox4.Dock = System.Windows.Forms.DockStyle.Left
        Me.PictureBox4.Image = Global.BrightUtility.My.Resources.Resources.ImportExport
        Me.PictureBox4.Location = New System.Drawing.Point(3, 3)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(149, 338)
        Me.PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox4.TabIndex = 20
        Me.PictureBox4.TabStop = False
        '
        'tabDetach
        '
        Me.tabDetach.BackColor = System.Drawing.Color.White
        Me.tabDetach.Controls.Add(Me.tabctrlDetach)
        Me.tabDetach.Controls.Add(Me.PictureBox5)
        Me.tabDetach.Location = New System.Drawing.Point(4, 22)
        Me.tabDetach.Name = "tabDetach"
        Me.tabDetach.Padding = New System.Windows.Forms.Padding(3)
        Me.tabDetach.Size = New System.Drawing.Size(440, 344)
        Me.tabDetach.TabIndex = 3
        Me.tabDetach.Text = "Detach"
        Me.tabDetach.UseVisualStyleBackColor = True
        '
        'tabctrlDetach
        '
        Me.tabctrlDetach.Controls.Add(Me.tabpgdparticular)
        Me.tabctrlDetach.Controls.Add(Me.tabpgdAll)
        Me.tabctrlDetach.Location = New System.Drawing.Point(155, 6)
        Me.tabctrlDetach.Name = "tabctrlDetach"
        Me.tabctrlDetach.SelectedIndex = 0
        Me.tabctrlDetach.Size = New System.Drawing.Size(282, 332)
        Me.tabctrlDetach.TabIndex = 21
        '
        'tabpgdparticular
        '
        Me.tabpgdparticular.Controls.Add(Me.chkcmbDetachDatabase)
        Me.tabpgdparticular.Controls.Add(Me.Label21)
        Me.tabpgdparticular.Controls.Add(Me.cmbDetachDatabase)
        Me.tabpgdparticular.Controls.Add(Me.Label23)
        Me.tabpgdparticular.Controls.Add(Me.Label24)
        Me.tabpgdparticular.Location = New System.Drawing.Point(4, 22)
        Me.tabpgdparticular.Name = "tabpgdparticular"
        Me.tabpgdparticular.Padding = New System.Windows.Forms.Padding(3)
        Me.tabpgdparticular.Size = New System.Drawing.Size(274, 306)
        Me.tabpgdparticular.TabIndex = 0
        Me.tabpgdparticular.Text = "Particular"
        Me.tabpgdparticular.UseVisualStyleBackColor = True
        '
        'chkcmbDetachDatabase
        '
        Me.chkcmbDetachDatabase.CheckOnClick = True
        Me.chkcmbDetachDatabase.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbDetachDatabase.DropDownHeight = 1
        Me.chkcmbDetachDatabase.FormattingEnabled = True
        Me.chkcmbDetachDatabase.IntegralHeight = False
        Me.chkcmbDetachDatabase.Location = New System.Drawing.Point(75, 115)
        Me.chkcmbDetachDatabase.Name = "chkcmbDetachDatabase"
        Me.chkcmbDetachDatabase.Size = New System.Drawing.Size(164, 22)
        Me.chkcmbDetachDatabase.TabIndex = 26
        Me.chkcmbDetachDatabase.ValueSeparator = ", "
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(11, 114)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(61, 13)
        Me.Label21.TabIndex = 25
        Me.Label21.Text = "Database"
        '
        'cmbDetachDatabase
        '
        Me.cmbDetachDatabase.FormattingEnabled = True
        Me.cmbDetachDatabase.Location = New System.Drawing.Point(75, 155)
        Me.cmbDetachDatabase.Name = "cmbDetachDatabase"
        Me.cmbDetachDatabase.Size = New System.Drawing.Size(164, 21)
        Me.cmbDetachDatabase.TabIndex = 24
        Me.cmbDetachDatabase.Visible = False
        '
        'Label23
        '
        Me.Label23.Location = New System.Drawing.Point(10, 69)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(229, 45)
        Me.Label23.TabIndex = 23
        Me.Label23.Text = "This wizard helps you to detach the specific SQL server database."
        '
        'Label24
        '
        Me.Label24.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(6, 3)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(237, 43)
        Me.Label24.TabIndex = 22
        Me.Label24.Text = "Welcome to the database detach Wizard"
        '
        'tabpgdAll
        '
        Me.tabpgdAll.Controls.Add(Me.chkdSelectAll)
        Me.tabpgdAll.Controls.Add(Me.Panel1)
        Me.tabpgdAll.Location = New System.Drawing.Point(4, 22)
        Me.tabpgdAll.Name = "tabpgdAll"
        Me.tabpgdAll.Padding = New System.Windows.Forms.Padding(3)
        Me.tabpgdAll.Size = New System.Drawing.Size(274, 306)
        Me.tabpgdAll.TabIndex = 1
        Me.tabpgdAll.Text = "All"
        Me.tabpgdAll.UseVisualStyleBackColor = True
        '
        'chkdSelectAll
        '
        Me.chkdSelectAll.AutoSize = True
        Me.chkdSelectAll.Location = New System.Drawing.Point(6, 6)
        Me.chkdSelectAll.Name = "chkdSelectAll"
        Me.chkdSelectAll.Size = New System.Drawing.Size(79, 17)
        Me.chkdSelectAll.TabIndex = 9
        Me.chkdSelectAll.Text = "Select All"
        Me.chkdSelectAll.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.dgvDetach)
        Me.Panel1.Location = New System.Drawing.Point(6, 29)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(275, 271)
        Me.Panel1.TabIndex = 8
        '
        'dgvDetach
        '
        Me.dgvDetach.AllowUserToAddRows = False
        Me.dgvDetach.AllowUserToDeleteRows = False
        Me.dgvDetach.AllowUserToOrderColumns = True
        Me.dgvDetach.AllowUserToResizeColumns = False
        Me.dgvDetach.AllowUserToResizeRows = False
        Me.dgvDetach.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDetach.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvDetach.Location = New System.Drawing.Point(0, 0)
        Me.dgvDetach.Name = "dgvDetach"
        Me.dgvDetach.RowHeadersVisible = False
        Me.dgvDetach.Size = New System.Drawing.Size(275, 271)
        Me.dgvDetach.TabIndex = 0
        '
        'PictureBox5
        '
        Me.PictureBox5.Dock = System.Windows.Forms.DockStyle.Left
        Me.PictureBox5.Image = Global.BrightUtility.My.Resources.Resources.ImportExport
        Me.PictureBox5.Location = New System.Drawing.Point(3, 3)
        Me.PictureBox5.Name = "PictureBox5"
        Me.PictureBox5.Size = New System.Drawing.Size(149, 338)
        Me.PictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox5.TabIndex = 20
        Me.PictureBox5.TabStop = False
        '
        'tabBackUp
        '
        Me.tabBackUp.BackColor = System.Drawing.Color.White
        Me.tabBackUp.Controls.Add(Me.chkBackUpIsDBSystemPath)
        Me.tabBackUp.Controls.Add(Me.gridBackUpStatus)
        Me.tabBackUp.Controls.Add(Me.pBackUpBar)
        Me.tabBackUp.Controls.Add(Me.btnBackUpBrowsePath)
        Me.tabBackUp.Controls.Add(Me.Label14)
        Me.tabBackUp.Controls.Add(Me.txtBackUpPath)
        Me.tabBackUp.Controls.Add(Me.rbtdayname)
        Me.tabBackUp.Controls.Add(Me.rbtBackUpDayWise)
        Me.tabBackUp.Controls.Add(Me.PictureBox3)
        Me.tabBackUp.Controls.Add(Me.rbtBackUpTimeWise)
        Me.tabBackUp.Controls.Add(Me.Label4)
        Me.tabBackUp.Controls.Add(Me.chkLstBackUpDataBases)
        Me.tabBackUp.Location = New System.Drawing.Point(4, 22)
        Me.tabBackUp.Name = "tabBackUp"
        Me.tabBackUp.Padding = New System.Windows.Forms.Padding(3)
        Me.tabBackUp.Size = New System.Drawing.Size(440, 344)
        Me.tabBackUp.TabIndex = 4
        Me.tabBackUp.Text = "BackUp"
        Me.tabBackUp.UseVisualStyleBackColor = True
        '
        'chkBackUpIsDBSystemPath
        '
        Me.chkBackUpIsDBSystemPath.AutoSize = True
        Me.chkBackUpIsDBSystemPath.Checked = True
        Me.chkBackUpIsDBSystemPath.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkBackUpIsDBSystemPath.Location = New System.Drawing.Point(175, 233)
        Me.chkBackUpIsDBSystemPath.Name = "chkBackUpIsDBSystemPath"
        Me.chkBackUpIsDBSystemPath.Size = New System.Drawing.Size(230, 17)
        Me.chkBackUpIsDBSystemPath.TabIndex = 7
        Me.chkBackUpIsDBSystemPath.Text = "Is above path exists in DB System?"
        Me.chkBackUpIsDBSystemPath.UseVisualStyleBackColor = True
        '
        'gridBackUpStatus
        '
        Me.gridBackUpStatus.AllowUserToAddRows = False
        Me.gridBackUpStatus.AllowUserToDeleteRows = False
        Me.gridBackUpStatus.BackgroundColor = System.Drawing.SystemColors.Window
        Me.gridBackUpStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridBackUpStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridBackUpStatus.DefaultCellStyle = DataGridViewCellStyle1
        Me.gridBackUpStatus.GridColor = System.Drawing.Color.Gainsboro
        Me.gridBackUpStatus.Location = New System.Drawing.Point(173, 251)
        Me.gridBackUpStatus.Name = "gridBackUpStatus"
        Me.gridBackUpStatus.ReadOnly = True
        Me.gridBackUpStatus.RowHeadersVisible = False
        Me.gridBackUpStatus.RowTemplate.Height = 16
        Me.gridBackUpStatus.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridBackUpStatus.Size = New System.Drawing.Size(225, 63)
        Me.gridBackUpStatus.TabIndex = 8
        '
        'pBackUpBar
        '
        Me.pBackUpBar.Location = New System.Drawing.Point(173, 320)
        Me.pBackUpBar.Name = "pBackUpBar"
        Me.pBackUpBar.Size = New System.Drawing.Size(225, 13)
        Me.pBackUpBar.TabIndex = 9
        '
        'btnBackUpBrowsePath
        '
        Me.btnBackUpBrowsePath.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBackUpBrowsePath.ForeColor = System.Drawing.Color.Red
        Me.btnBackUpBrowsePath.Location = New System.Drawing.Point(367, 207)
        Me.btnBackUpBrowsePath.Name = "btnBackUpBrowsePath"
        Me.btnBackUpBrowsePath.Size = New System.Drawing.Size(28, 23)
        Me.btnBackUpBrowsePath.TabIndex = 6
        Me.btnBackUpBrowsePath.Text = ".."
        Me.btnBackUpBrowsePath.UseVisualStyleBackColor = True
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(173, 192)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(88, 13)
        Me.Label14.TabIndex = 4
        Me.Label14.Text = "BackUp Path :"
        '
        'txtBackUpPath
        '
        Me.txtBackUpPath.Location = New System.Drawing.Point(173, 208)
        Me.txtBackUpPath.Name = "txtBackUpPath"
        Me.txtBackUpPath.Size = New System.Drawing.Size(189, 21)
        Me.txtBackUpPath.TabIndex = 5
        '
        'rbtdayname
        '
        Me.rbtdayname.AutoSize = True
        Me.rbtdayname.Location = New System.Drawing.Point(323, 13)
        Me.rbtdayname.Name = "rbtdayname"
        Me.rbtdayname.Size = New System.Drawing.Size(108, 17)
        Me.rbtdayname.TabIndex = 1
        Me.rbtdayname.Text = "DayNameWise"
        Me.rbtdayname.UseVisualStyleBackColor = True
        '
        'rbtBackUpDayWise
        '
        Me.rbtBackUpDayWise.AutoSize = True
        Me.rbtBackUpDayWise.Location = New System.Drawing.Point(253, 13)
        Me.rbtBackUpDayWise.Name = "rbtBackUpDayWise"
        Me.rbtBackUpDayWise.Size = New System.Drawing.Size(75, 17)
        Me.rbtBackUpDayWise.TabIndex = 1
        Me.rbtBackUpDayWise.Text = "DayWise"
        Me.rbtBackUpDayWise.UseVisualStyleBackColor = True
        '
        'PictureBox3
        '
        Me.PictureBox3.Dock = System.Windows.Forms.DockStyle.Left
        Me.PictureBox3.Image = Global.BrightUtility.My.Resources.Resources.ImportExport
        Me.PictureBox3.Location = New System.Drawing.Point(3, 3)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(149, 338)
        Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox3.TabIndex = 12
        Me.PictureBox3.TabStop = False
        '
        'rbtBackUpTimeWise
        '
        Me.rbtBackUpTimeWise.AutoSize = True
        Me.rbtBackUpTimeWise.Checked = True
        Me.rbtBackUpTimeWise.Location = New System.Drawing.Point(173, 13)
        Me.rbtBackUpTimeWise.Name = "rbtBackUpTimeWise"
        Me.rbtBackUpTimeWise.Size = New System.Drawing.Size(84, 17)
        Me.rbtBackUpTimeWise.TabIndex = 0
        Me.rbtBackUpTimeWise.TabStop = True
        Me.rbtBackUpTimeWise.Text = "Time Wise"
        Me.rbtBackUpTimeWise.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(170, 39)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(76, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Databases :"
        '
        'chkLstBackUpDataBases
        '
        Me.chkLstBackUpDataBases.FormattingEnabled = True
        Me.chkLstBackUpDataBases.Location = New System.Drawing.Point(173, 55)
        Me.chkLstBackUpDataBases.Name = "chkLstBackUpDataBases"
        Me.chkLstBackUpDataBases.Size = New System.Drawing.Size(225, 132)
        Me.chkLstBackUpDataBases.TabIndex = 3
        '
        'tabRestore
        '
        Me.tabRestore.BackColor = System.Drawing.Color.White
        Me.tabRestore.Controls.Add(Me.Label22)
        Me.tabRestore.Controls.Add(Me.Label20)
        Me.tabRestore.Controls.Add(Me.btnRestorePathBrowse)
        Me.tabRestore.Controls.Add(Me.Label3)
        Me.tabRestore.Controls.Add(Me.txtRestorePath)
        Me.tabRestore.Controls.Add(Me.ProgressBar1)
        Me.tabRestore.Controls.Add(Me.gridRestoreFiles)
        Me.tabRestore.Controls.Add(Me.btnAddBackUpFiles)
        Me.tabRestore.Controls.Add(Me.PictureBox6)
        Me.tabRestore.Location = New System.Drawing.Point(4, 22)
        Me.tabRestore.Name = "tabRestore"
        Me.tabRestore.Padding = New System.Windows.Forms.Padding(3)
        Me.tabRestore.Size = New System.Drawing.Size(440, 344)
        Me.tabRestore.TabIndex = 5
        Me.tabRestore.Text = "Restore"
        Me.tabRestore.UseVisualStyleBackColor = True
        '
        'Label22
        '
        Me.Label22.Location = New System.Drawing.Point(174, 46)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(229, 45)
        Me.Label22.TabIndex = 24
        Me.Label22.Text = "This wizard helps you to restore of the SQL server databases with specified backu" &
            "p file."
        '
        'Label20
        '
        Me.Label20.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(174, 3)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(236, 43)
        Me.Label20.TabIndex = 23
        Me.Label20.Text = "Welcome to the Database Restore Wizard"
        '
        'btnRestorePathBrowse
        '
        Me.btnRestorePathBrowse.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRestorePathBrowse.ForeColor = System.Drawing.Color.Red
        Me.btnRestorePathBrowse.Location = New System.Drawing.Point(363, 285)
        Me.btnRestorePathBrowse.Name = "btnRestorePathBrowse"
        Me.btnRestorePathBrowse.Size = New System.Drawing.Size(28, 23)
        Me.btnRestorePathBrowse.TabIndex = 22
        Me.btnRestorePathBrowse.Text = ".."
        Me.btnRestorePathBrowse.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(169, 270)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(89, 13)
        Me.Label3.TabIndex = 20
        Me.Label3.Text = "Restore Path :"
        '
        'txtRestorePath
        '
        Me.txtRestorePath.Location = New System.Drawing.Point(169, 286)
        Me.txtRestorePath.Name = "txtRestorePath"
        Me.txtRestorePath.Size = New System.Drawing.Size(189, 21)
        Me.txtRestorePath.TabIndex = 21
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(169, 324)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(239, 13)
        Me.ProgressBar1.TabIndex = 19
        '
        'gridRestoreFiles
        '
        Me.gridRestoreFiles.AllowUserToAddRows = False
        Me.gridRestoreFiles.AllowUserToDeleteRows = False
        Me.gridRestoreFiles.BackgroundColor = System.Drawing.SystemColors.Window
        Me.gridRestoreFiles.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.gridRestoreFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridRestoreFiles.DefaultCellStyle = DataGridViewCellStyle2
        Me.gridRestoreFiles.GridColor = System.Drawing.Color.Gainsboro
        Me.gridRestoreFiles.Location = New System.Drawing.Point(169, 128)
        Me.gridRestoreFiles.Name = "gridRestoreFiles"
        Me.gridRestoreFiles.RowHeadersVisible = False
        Me.gridRestoreFiles.RowTemplate.Height = 16
        Me.gridRestoreFiles.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridRestoreFiles.Size = New System.Drawing.Size(239, 137)
        Me.gridRestoreFiles.TabIndex = 17
        '
        'btnAddBackUpFiles
        '
        Me.btnAddBackUpFiles.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddBackUpFiles.Location = New System.Drawing.Point(167, 100)
        Me.btnAddBackUpFiles.Name = "btnAddBackUpFiles"
        Me.btnAddBackUpFiles.Size = New System.Drawing.Size(145, 23)
        Me.btnAddBackUpFiles.TabIndex = 16
        Me.btnAddBackUpFiles.Text = "Add BackUp Files"
        Me.btnAddBackUpFiles.UseVisualStyleBackColor = True
        '
        'PictureBox6
        '
        Me.PictureBox6.Dock = System.Windows.Forms.DockStyle.Left
        Me.PictureBox6.Image = Global.BrightUtility.My.Resources.Resources.ImportExport
        Me.PictureBox6.Location = New System.Drawing.Point(3, 3)
        Me.PictureBox6.Name = "PictureBox6"
        Me.PictureBox6.Size = New System.Drawing.Size(149, 338)
        Me.PictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox6.TabIndex = 13
        Me.PictureBox6.TabStop = False
        '
        'Collation
        '
        Me.Collation.BackColor = System.Drawing.Color.White
        Me.Collation.Controls.Add(Me.Label28)
        Me.Collation.Controls.Add(Me.Label29)
        Me.Collation.Controls.Add(Me.Cmbcurcollation)
        Me.Collation.Controls.Add(Me.BtnUpdateCollaton)
        Me.Collation.Controls.Add(Me.Label27)
        Me.Collation.Controls.Add(Me.CmbCollationName)
        Me.Collation.Controls.Add(Me.Label26)
        Me.Collation.Controls.Add(Me.CmbDataBaseName)
        Me.Collation.Controls.Add(Me.Label25)
        Me.Collation.Controls.Add(Me.PictureBox7)
        Me.Collation.Location = New System.Drawing.Point(4, 22)
        Me.Collation.Name = "Collation"
        Me.Collation.Padding = New System.Windows.Forms.Padding(3)
        Me.Collation.Size = New System.Drawing.Size(440, 344)
        Me.Collation.TabIndex = 6
        Me.Collation.Text = "Collation"
        Me.Collation.UseVisualStyleBackColor = True
        '
        'Label28
        '
        Me.Label28.Location = New System.Drawing.Point(162, 59)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(259, 43)
        Me.Label28.TabIndex = 25
        Me.Label28.Text = "This wizard helps you to Alter collation Name  Particular Database to the SQL ser" &
            "ver."
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Location = New System.Drawing.Point(162, 159)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(142, 13)
        Me.Label29.TabIndex = 24
        Me.Label29.Text = "Current Collation Name"
        '
        'Cmbcurcollation
        '
        Me.Cmbcurcollation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.Cmbcurcollation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.Cmbcurcollation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cmbcurcollation.FormattingEnabled = True
        Me.Cmbcurcollation.Location = New System.Drawing.Point(158, 175)
        Me.Cmbcurcollation.Name = "Cmbcurcollation"
        Me.Cmbcurcollation.Size = New System.Drawing.Size(263, 21)
        Me.Cmbcurcollation.TabIndex = 23
        '
        'BtnUpdateCollaton
        '
        Me.BtnUpdateCollaton.Location = New System.Drawing.Point(158, 263)
        Me.BtnUpdateCollaton.Name = "BtnUpdateCollaton"
        Me.BtnUpdateCollaton.Size = New System.Drawing.Size(77, 25)
        Me.BtnUpdateCollaton.TabIndex = 3
        Me.BtnUpdateCollaton.Text = "Update"
        Me.BtnUpdateCollaton.UseVisualStyleBackColor = True
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Location = New System.Drawing.Point(161, 209)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(69, 13)
        Me.Label27.TabIndex = 19
        Me.Label27.Text = "Change To"
        '
        'CmbCollationName
        '
        Me.CmbCollationName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.CmbCollationName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.CmbCollationName.FormattingEnabled = True
        Me.CmbCollationName.Location = New System.Drawing.Point(158, 225)
        Me.CmbCollationName.Name = "CmbCollationName"
        Me.CmbCollationName.Size = New System.Drawing.Size(263, 21)
        Me.CmbCollationName.TabIndex = 2
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(161, 110)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(98, 13)
        Me.Label26.TabIndex = 6
        Me.Label26.Text = "Database Name"
        '
        'CmbDataBaseName
        '
        Me.CmbDataBaseName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.CmbDataBaseName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.CmbDataBaseName.FormattingEnabled = True
        Me.CmbDataBaseName.Location = New System.Drawing.Point(158, 126)
        Me.CmbDataBaseName.Name = "CmbDataBaseName"
        Me.CmbDataBaseName.Size = New System.Drawing.Size(171, 21)
        Me.CmbDataBaseName.TabIndex = 1
        '
        'Label25
        '
        Me.Label25.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label25.Location = New System.Drawing.Point(158, 3)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(263, 43)
        Me.Label25.TabIndex = 15
        Me.Label25.Text = "Welcome to the Transaction Alter Collation  Name Wizard"
        '
        'PictureBox7
        '
        Me.PictureBox7.Dock = System.Windows.Forms.DockStyle.Left
        Me.PictureBox7.Image = Global.BrightUtility.My.Resources.Resources.ImportExport
        Me.PictureBox7.Location = New System.Drawing.Point(3, 3)
        Me.PictureBox7.Name = "PictureBox7"
        Me.PictureBox7.Size = New System.Drawing.Size(149, 338)
        Me.PictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox7.TabIndex = 14
        Me.PictureBox7.TabStop = False
        '
        'tabReleaseMemory
        '
        Me.tabReleaseMemory.Controls.Add(Me.LblDelete)
        Me.tabReleaseMemory.Controls.Add(Me.dtpSyncDate)
        Me.tabReleaseMemory.Controls.Add(Me.LblRDBaseName)
        Me.tabReleaseMemory.Controls.Add(Me.CmbRDataBase)
        Me.tabReleaseMemory.Controls.Add(Me.GroupBox1)
        Me.tabReleaseMemory.Controls.Add(Me.Label30)
        Me.tabReleaseMemory.Controls.Add(Me.Label31)
        Me.tabReleaseMemory.Controls.Add(Me.PictureBox8)
        Me.tabReleaseMemory.Location = New System.Drawing.Point(4, 22)
        Me.tabReleaseMemory.Name = "tabReleaseMemory"
        Me.tabReleaseMemory.Size = New System.Drawing.Size(440, 344)
        Me.tabReleaseMemory.TabIndex = 7
        Me.tabReleaseMemory.Text = "ReleaseUnusedMemory"
        Me.tabReleaseMemory.UseVisualStyleBackColor = True
        '
        'LblDelete
        '
        Me.LblDelete.Location = New System.Drawing.Point(331, 267)
        Me.LblDelete.Name = "LblDelete"
        Me.LblDelete.Size = New System.Drawing.Size(102, 30)
        Me.LblDelete.TabIndex = 37
        Me.LblDelete.Text = "Select Date Before to Delete" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.LblDelete.Visible = False
        '
        'dtpSyncDate
        '
        Me.dtpSyncDate.CustomFormat = "dd-MM-yyyy"
        Me.dtpSyncDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpSyncDate.Location = New System.Drawing.Point(331, 302)
        Me.dtpSyncDate.Name = "dtpSyncDate"
        Me.dtpSyncDate.Size = New System.Drawing.Size(102, 21)
        Me.dtpSyncDate.TabIndex = 36
        Me.dtpSyncDate.Visible = False
        '
        'LblRDBaseName
        '
        Me.LblRDBaseName.AutoSize = True
        Me.LblRDBaseName.Location = New System.Drawing.Point(172, 149)
        Me.LblRDBaseName.Name = "LblRDBaseName"
        Me.LblRDBaseName.Size = New System.Drawing.Size(98, 13)
        Me.LblRDBaseName.TabIndex = 35
        Me.LblRDBaseName.Text = "Database Name"
        '
        'CmbRDataBase
        '
        Me.CmbRDataBase.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.CmbRDataBase.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.CmbRDataBase.FormattingEnabled = True
        Me.CmbRDataBase.Location = New System.Drawing.Point(169, 165)
        Me.CmbRDataBase.Name = "CmbRDataBase"
        Me.CmbRDataBase.Size = New System.Drawing.Size(153, 21)
        Me.CmbRDataBase.TabIndex = 34
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkDroptempdb)
        Me.GroupBox1.Controls.Add(Me.ChkSyncData)
        Me.GroupBox1.Controls.Add(Me.CkhRebuidDatabase)
        Me.GroupBox1.Controls.Add(Me.chkIndex)
        Me.GroupBox1.Controls.Add(Me.chkMemory)
        Me.GroupBox1.Location = New System.Drawing.Point(170, 191)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(155, 141)
        Me.GroupBox1.TabIndex = 29
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Select"
        '
        'chkDroptempdb
        '
        Me.chkDroptempdb.AutoSize = True
        Me.chkDroptempdb.Location = New System.Drawing.Point(14, 66)
        Me.chkDroptempdb.Name = "chkDroptempdb"
        Me.chkDroptempdb.Size = New System.Drawing.Size(133, 17)
        Me.chkDroptempdb.TabIndex = 40
        Me.chkDroptempdb.Text = "DropTempTableDB"
        Me.chkDroptempdb.UseVisualStyleBackColor = True
        '
        'ChkSyncData
        '
        Me.ChkSyncData.AutoSize = True
        Me.ChkSyncData.Location = New System.Drawing.Point(14, 111)
        Me.ChkSyncData.Name = "ChkSyncData"
        Me.ChkSyncData.Size = New System.Drawing.Size(121, 17)
        Me.ChkSyncData.TabIndex = 39
        Me.ChkSyncData.Text = "Delete SyncData"
        Me.ChkSyncData.UseVisualStyleBackColor = True
        '
        'CkhRebuidDatabase
        '
        Me.CkhRebuidDatabase.AutoSize = True
        Me.CkhRebuidDatabase.Location = New System.Drawing.Point(14, 89)
        Me.CkhRebuidDatabase.Name = "CkhRebuidDatabase"
        Me.CkhRebuidDatabase.Size = New System.Drawing.Size(125, 17)
        Me.CkhRebuidDatabase.TabIndex = 38
        Me.CkhRebuidDatabase.Text = "Rebuild Database"
        Me.CkhRebuidDatabase.UseVisualStyleBackColor = True
        '
        'chkIndex
        '
        Me.chkIndex.AutoSize = True
        Me.chkIndex.Location = New System.Drawing.Point(14, 20)
        Me.chkIndex.Name = "chkIndex"
        Me.chkIndex.Size = New System.Drawing.Size(104, 17)
        Me.chkIndex.TabIndex = 37
        Me.chkIndex.Text = "Rebuild Index" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.chkIndex.UseVisualStyleBackColor = True
        '
        'chkMemory
        '
        Me.chkMemory.AutoSize = True
        Me.chkMemory.Checked = True
        Me.chkMemory.Location = New System.Drawing.Point(14, 43)
        Me.chkMemory.Name = "chkMemory"
        Me.chkMemory.Size = New System.Drawing.Size(126, 17)
        Me.chkMemory.TabIndex = 36
        Me.chkMemory.TabStop = True
        Me.chkMemory.Text = "Drop TempTables"
        Me.chkMemory.UseVisualStyleBackColor = True
        '
        'Label30
        '
        Me.Label30.Location = New System.Drawing.Point(165, 63)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(259, 80)
        Me.Label30.TabIndex = 28
        Me.Label30.Text = "This wizard helps you to Release the Unused  Memory and Rebuilding the Index and " &
            "Rebuilding the Database." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Note:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "       Make Sure the Database is Not in Use." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'Label31
        '
        Me.Label31.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label31.Location = New System.Drawing.Point(161, 3)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(263, 57)
        Me.Label31.TabIndex = 27
        Me.Label31.Text = "Welcome to the Release Unused Memory and Rebuilding Index and Database " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Wizard"
        '
        'PictureBox8
        '
        Me.PictureBox8.Dock = System.Windows.Forms.DockStyle.Left
        Me.PictureBox8.Image = Global.BrightUtility.My.Resources.Resources.ImportExport
        Me.PictureBox8.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox8.Name = "PictureBox8"
        Me.PictureBox8.Size = New System.Drawing.Size(149, 344)
        Me.PictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox8.TabIndex = 26
        Me.PictureBox8.TabStop = False
        '
        'tabRecover
        '
        Me.tabRecover.Controls.Add(Me.Label32)
        Me.tabRecover.Controls.Add(Me.CmbRcDatabase)
        Me.tabRecover.Controls.Add(Me.Label33)
        Me.tabRecover.Controls.Add(Me.Label34)
        Me.tabRecover.Controls.Add(Me.PictureBox9)
        Me.tabRecover.Location = New System.Drawing.Point(4, 22)
        Me.tabRecover.Name = "tabRecover"
        Me.tabRecover.Size = New System.Drawing.Size(440, 344)
        Me.tabRecover.TabIndex = 8
        Me.tabRecover.Text = "RecoverDatabase"
        Me.tabRecover.UseVisualStyleBackColor = True
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Location = New System.Drawing.Point(203, 168)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(98, 13)
        Me.Label32.TabIndex = 40
        Me.Label32.Text = "Database Name"
        '
        'CmbRcDatabase
        '
        Me.CmbRcDatabase.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.CmbRcDatabase.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.CmbRcDatabase.FormattingEnabled = True
        Me.CmbRcDatabase.Location = New System.Drawing.Point(200, 184)
        Me.CmbRcDatabase.Name = "CmbRcDatabase"
        Me.CmbRcDatabase.Size = New System.Drawing.Size(153, 21)
        Me.CmbRcDatabase.TabIndex = 39
        '
        'Label33
        '
        Me.Label33.Location = New System.Drawing.Point(173, 68)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(259, 84)
        Me.Label33.TabIndex = 38
        Me.Label33.Text = "This wizard helps you to Recover the Suspect Database to Active Database." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Note" &
            ":" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "       Don't Forget to Run DBCreator after Recovered the Suspect DataBase." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'Label34
        '
        Me.Label34.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label34.Location = New System.Drawing.Point(169, 3)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(263, 57)
        Me.Label34.TabIndex = 37
        Me.Label34.Text = "Welcome to the Recover the Suspect Database Wizard." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'PictureBox9
        '
        Me.PictureBox9.Dock = System.Windows.Forms.DockStyle.Left
        Me.PictureBox9.Image = Global.BrightUtility.My.Resources.Resources.ImportExport
        Me.PictureBox9.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox9.Name = "PictureBox9"
        Me.PictureBox9.Size = New System.Drawing.Size(149, 344)
        Me.PictureBox9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox9.TabIndex = 36
        Me.PictureBox9.TabStop = False
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.GrpMain)
        Me.tabView.Controls.Add(Me.GrpHead)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Size = New System.Drawing.Size(440, 344)
        Me.tabView.TabIndex = 9
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'GrpMain
        '
        Me.GrpMain.Controls.Add(Me.GridView)
        Me.GrpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GrpMain.Location = New System.Drawing.Point(0, 44)
        Me.GrpMain.Name = "GrpMain"
        Me.GrpMain.Size = New System.Drawing.Size(440, 300)
        Me.GrpMain.TabIndex = 2
        Me.GrpMain.TabStop = False
        '
        'GridView
        '
        Me.GridView.AllowUserToAddRows = False
        Me.GridView.AllowUserToDeleteRows = False
        Me.GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridView.Location = New System.Drawing.Point(3, 17)
        Me.GridView.Name = "GridView"
        Me.GridView.RowHeadersVisible = False
        Me.GridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.GridView.Size = New System.Drawing.Size(434, 280)
        Me.GridView.TabIndex = 0
        '
        'GrpHead
        '
        Me.GrpHead.Controls.Add(Me.ChkAll)
        Me.GrpHead.Controls.Add(Me.btnUpdate)
        Me.GrpHead.Controls.Add(Me.btnVBack)
        Me.GrpHead.Dock = System.Windows.Forms.DockStyle.Top
        Me.GrpHead.Location = New System.Drawing.Point(0, 0)
        Me.GrpHead.Name = "GrpHead"
        Me.GrpHead.Size = New System.Drawing.Size(440, 44)
        Me.GrpHead.TabIndex = 1
        Me.GrpHead.TabStop = False
        '
        'ChkAll
        '
        Me.ChkAll.AutoSize = True
        Me.ChkAll.Checked = True
        Me.ChkAll.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkAll.Location = New System.Drawing.Point(8, 17)
        Me.ChkAll.Name = "ChkAll"
        Me.ChkAll.Size = New System.Drawing.Size(76, 17)
        Me.ChkAll.TabIndex = 0
        Me.ChkAll.Text = "CheckAll"
        Me.ChkAll.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(177, 10)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(86, 30)
        Me.btnUpdate.TabIndex = 5
        Me.btnUpdate.Text = "Delete"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnVBack
        '
        Me.btnVBack.Location = New System.Drawing.Point(88, 10)
        Me.btnVBack.Name = "btnVBack"
        Me.btnVBack.Size = New System.Drawing.Size(86, 30)
        Me.btnVBack.TabIndex = 4
        Me.btnVBack.Text = "Back"
        Me.btnVBack.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(280, 382)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(86, 30)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnNext
        '
        Me.btnNext.Location = New System.Drawing.Point(188, 382)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(86, 30)
        Me.btnNext.TabIndex = 2
        Me.btnNext.Text = "Next >"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(96, 382)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(86, 30)
        Me.btnBack.TabIndex = 1
        Me.btnBack.Text = "< Back"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'SqlUtility
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(462, 417)
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "SqlUtility"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "GSqlUtility Wizard"
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.tabGeneral.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabShrink.ResumeLayout(False)
        Me.tabShrink.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabAttach.ResumeLayout(False)
        Me.tabAttach.PerformLayout()
        Me.tabctrlAttachDetach.ResumeLayout(False)
        Me.tabpgParticular.ResumeLayout(False)
        Me.tabpgParticular.PerformLayout()
        Me.PnlAttachGrid.ResumeLayout(False)
        CType(Me.GridViewAttach, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabpgAll.ResumeLayout(False)
        Me.tabpgAll.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        CType(Me.dgvAttach, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabDetach.ResumeLayout(False)
        Me.tabctrlDetach.ResumeLayout(False)
        Me.tabpgdparticular.ResumeLayout(False)
        Me.tabpgdparticular.PerformLayout()
        Me.tabpgdAll.ResumeLayout(False)
        Me.tabpgdAll.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        CType(Me.dgvDetach, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabBackUp.ResumeLayout(False)
        Me.tabBackUp.PerformLayout()
        CType(Me.gridBackUpStatus, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabRestore.ResumeLayout(False)
        Me.tabRestore.PerformLayout()
        CType(Me.gridRestoreFiles, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Collation.ResumeLayout(False)
        Me.Collation.PerformLayout()
        CType(Me.PictureBox7, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabReleaseMemory.ResumeLayout(False)
        Me.tabReleaseMemory.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.PictureBox8, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabRecover.ResumeLayout(False)
        Me.tabRecover.PerformLayout()
        CType(Me.PictureBox9, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabView.ResumeLayout(False)
        Me.GrpMain.ResumeLayout(False)
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GrpHead.ResumeLayout(False)
        Me.GrpHead.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabShrink As System.Windows.Forms.TabPage
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents rdbShrinkLogfile As System.Windows.Forms.RadioButton
    Friend WithEvents rdbDetach As System.Windows.Forms.RadioButton
    Friend WithEvents rdbAttach As System.Windows.Forms.RadioButton
    Friend WithEvents rdbRestore As System.Windows.Forms.RadioButton
    Friend WithEvents rdbBackUp As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tabAttach As System.Windows.Forms.TabPage
    Friend WithEvents tabDetach As System.Windows.Forms.TabPage
    Friend WithEvents tabBackUp As System.Windows.Forms.TabPage
    Friend WithEvents tabRestore As System.Windows.Forms.TabPage
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents dudShrink As System.Windows.Forms.DomainUpDown
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cmbServers As System.Windows.Forms.ComboBox
    Friend WithEvents cmbLoginType As System.Windows.Forms.ComboBox
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtLoginName As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents btnRefreshServer As System.Windows.Forms.Button
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Public WithEvents txtBackUpPath As System.Windows.Forms.TextBox
    Friend WithEvents rbtBackUpDayWise As System.Windows.Forms.RadioButton
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents rbtBackUpTimeWise As System.Windows.Forms.RadioButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents chkLstBackUpDataBases As System.Windows.Forms.CheckedListBox
    Friend WithEvents btnBackUpBrowsePath As System.Windows.Forms.Button
    Friend WithEvents gridBackUpStatus As System.Windows.Forms.DataGridView
    Friend WithEvents pBackUpBar As System.Windows.Forms.ProgressBar
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents PictureBox5 As System.Windows.Forms.PictureBox
    Friend WithEvents btnAddBackUpFiles As System.Windows.Forms.Button
    Friend WithEvents PictureBox6 As System.Windows.Forms.PictureBox
    Friend WithEvents gridRestoreFiles As System.Windows.Forms.DataGridView
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents btnRestorePathBrowse As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents txtRestorePath As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents RbtnCollation As System.Windows.Forms.RadioButton
    Friend WithEvents Collation As System.Windows.Forms.TabPage
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents CmbCollationName As System.Windows.Forms.ComboBox
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents CmbDataBaseName As System.Windows.Forms.ComboBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents PictureBox7 As System.Windows.Forms.PictureBox
    Friend WithEvents BtnUpdateCollaton As System.Windows.Forms.Button
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Protected WithEvents Cmbcurcollation As System.Windows.Forms.ComboBox
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents chkBackUpIsDBSystemPath As System.Windows.Forms.CheckBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents rbtdayname As System.Windows.Forms.RadioButton
    Friend WithEvents RbtnRelease As System.Windows.Forms.RadioButton
    Friend WithEvents tabReleaseMemory As System.Windows.Forms.TabPage
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents PictureBox8 As System.Windows.Forms.PictureBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents LblRDBaseName As System.Windows.Forms.Label
    Friend WithEvents CmbRDataBase As System.Windows.Forms.ComboBox
    Friend WithEvents RbtnRecover As System.Windows.Forms.RadioButton
    Friend WithEvents tabRecover As System.Windows.Forms.TabPage
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents CmbRcDatabase As System.Windows.Forms.ComboBox
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents PictureBox9 As System.Windows.Forms.PictureBox
    Friend WithEvents dtpSyncDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents LblDelete As System.Windows.Forms.Label
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents GrpMain As System.Windows.Forms.GroupBox
    Friend WithEvents GrpHead As System.Windows.Forms.GroupBox
    Friend WithEvents GridView As System.Windows.Forms.DataGridView
    Friend WithEvents ChkAll As System.Windows.Forms.CheckBox
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents btnVBack As System.Windows.Forms.Button
    Friend WithEvents ChkSyncData As System.Windows.Forms.RadioButton
    Friend WithEvents CkhRebuidDatabase As System.Windows.Forms.RadioButton
    Friend WithEvents chkIndex As System.Windows.Forms.RadioButton
    Friend WithEvents chkMemory As System.Windows.Forms.RadioButton
    Friend WithEvents chkDroptempdb As System.Windows.Forms.RadioButton
    Friend WithEvents tabctrlAttachDetach As System.Windows.Forms.TabControl
    Friend WithEvents tabpgParticular As System.Windows.Forms.TabPage
    Friend WithEvents PnlAttachGrid As System.Windows.Forms.Panel
    Friend WithEvents GridViewAttach As System.Windows.Forms.DataGridView
    Friend WithEvents txtAttachDatabaseName As System.Windows.Forms.TextBox
    Friend WithEvents txtAttachLdfLocation As System.Windows.Forms.TextBox
    Friend WithEvents txtAttachMdfLocation As System.Windows.Forms.TextBox
    Friend WithEvents btnAttachBrowseSource As System.Windows.Forms.Button
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtAttachSourcePath As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents tabpgAll As System.Windows.Forms.TabPage
    Friend WithEvents chkSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents dgvAttach As System.Windows.Forms.DataGridView
    Friend WithEvents tabctrlDetach As System.Windows.Forms.TabControl
    Friend WithEvents tabpgdparticular As System.Windows.Forms.TabPage
    Friend WithEvents chkcmbDetachDatabase As BrighttechPack.CheckedComboBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents cmbDetachDatabase As System.Windows.Forms.ComboBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents tabpgdAll As System.Windows.Forms.TabPage
    Friend WithEvents chkdSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents dgvDetach As System.Windows.Forms.DataGridView
    Friend WithEvents ChkLstbShrinkDbName As System.Windows.Forms.CheckedListBox

End Class
