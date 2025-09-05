<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class VAMARGIN
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
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.grpContainer = New System.Windows.Forms.GroupBox
        Me.txtStnRate_PER = New System.Windows.Forms.TextBox
        Me.txtPreRate_PER = New System.Windows.Forms.TextBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.txtDiaRate_PER = New System.Windows.Forms.TextBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.cmbCostCentre = New System.Windows.Forms.ComboBox
        Me.lblCostCentre = New System.Windows.Forms.Label
        Me.txtStnRate_AMT = New System.Windows.Forms.TextBox
        Me.txtPreRate_AMT = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.txtWastage_WET = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtMarginName = New System.Windows.Forms.TextBox
        Me.txtMc_AMT = New System.Windows.Forms.TextBox
        Me.txtWastPer_PER = New System.Windows.Forms.TextBox
        Me.txtDisplayOrder_NUM = New System.Windows.Forms.TextBox
        Me.txtDiaRate_AMT = New System.Windows.Forms.TextBox
        Me.txtFixedValue_AMT = New System.Windows.Forms.TextBox
        Me.txtMcGrm_AMT = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.cmbDisplay = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmbGetPwd = New System.Windows.Forms.ComboBox
        Me.cmbMetalName = New System.Windows.Forms.ComboBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnOpen = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.tabView = New System.Windows.Forms.TabPage
        Me.grpContainer2 = New System.Windows.Forms.GroupBox
        Me.btnBack = New System.Windows.Forms.Button
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.txtMc_PER = New System.Windows.Forms.TextBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.grpContainer.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.grpContainer2.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGeneral)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(985, 536)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.grpContainer)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 22)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(977, 510)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'grpContainer
        '
        Me.grpContainer.Controls.Add(Me.txtMc_PER)
        Me.grpContainer.Controls.Add(Me.Label17)
        Me.grpContainer.Controls.Add(Me.txtStnRate_PER)
        Me.grpContainer.Controls.Add(Me.txtPreRate_PER)
        Me.grpContainer.Controls.Add(Me.Label14)
        Me.grpContainer.Controls.Add(Me.Label15)
        Me.grpContainer.Controls.Add(Me.txtDiaRate_PER)
        Me.grpContainer.Controls.Add(Me.Label16)
        Me.grpContainer.Controls.Add(Me.cmbCostCentre)
        Me.grpContainer.Controls.Add(Me.lblCostCentre)
        Me.grpContainer.Controls.Add(Me.txtStnRate_AMT)
        Me.grpContainer.Controls.Add(Me.txtPreRate_AMT)
        Me.grpContainer.Controls.Add(Me.Label12)
        Me.grpContainer.Controls.Add(Me.Label13)
        Me.grpContainer.Controls.Add(Me.txtWastage_WET)
        Me.grpContainer.Controls.Add(Me.Label1)
        Me.grpContainer.Controls.Add(Me.txtMarginName)
        Me.grpContainer.Controls.Add(Me.txtMc_AMT)
        Me.grpContainer.Controls.Add(Me.txtWastPer_PER)
        Me.grpContainer.Controls.Add(Me.txtDisplayOrder_NUM)
        Me.grpContainer.Controls.Add(Me.txtDiaRate_AMT)
        Me.grpContainer.Controls.Add(Me.txtFixedValue_AMT)
        Me.grpContainer.Controls.Add(Me.txtMcGrm_AMT)
        Me.grpContainer.Controls.Add(Me.Label6)
        Me.grpContainer.Controls.Add(Me.Label9)
        Me.grpContainer.Controls.Add(Me.Label10)
        Me.grpContainer.Controls.Add(Me.Label8)
        Me.grpContainer.Controls.Add(Me.Label11)
        Me.grpContainer.Controls.Add(Me.Label7)
        Me.grpContainer.Controls.Add(Me.Label5)
        Me.grpContainer.Controls.Add(Me.Label4)
        Me.grpContainer.Controls.Add(Me.Label3)
        Me.grpContainer.Controls.Add(Me.cmbDisplay)
        Me.grpContainer.Controls.Add(Me.Label2)
        Me.grpContainer.Controls.Add(Me.cmbGetPwd)
        Me.grpContainer.Controls.Add(Me.cmbMetalName)
        Me.grpContainer.Controls.Add(Me.btnExit)
        Me.grpContainer.Controls.Add(Me.btnNew)
        Me.grpContainer.Controls.Add(Me.btnOpen)
        Me.grpContainer.Controls.Add(Me.btnSave)
        Me.grpContainer.Location = New System.Drawing.Point(218, 18)
        Me.grpContainer.Name = "grpContainer"
        Me.grpContainer.Size = New System.Drawing.Size(450, 469)
        Me.grpContainer.TabIndex = 0
        Me.grpContainer.TabStop = False
        '
        'txtStnRate_PER
        '
        Me.txtStnRate_PER.Location = New System.Drawing.Point(269, 272)
        Me.txtStnRate_PER.Name = "txtStnRate_PER"
        Me.txtStnRate_PER.Size = New System.Drawing.Size(77, 21)
        Me.txtStnRate_PER.TabIndex = 25
        '
        'txtPreRate_PER
        '
        Me.txtPreRate_PER.Location = New System.Drawing.Point(269, 309)
        Me.txtPreRate_PER.Name = "txtPreRate_PER"
        Me.txtPreRate_PER.Size = New System.Drawing.Size(77, 21)
        Me.txtPreRate_PER.TabIndex = 29
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(192, 276)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(72, 13)
        Me.Label14.TabIndex = 24
        Me.Label14.Text = "Stn Rate %"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(192, 313)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(72, 13)
        Me.Label15.TabIndex = 28
        Me.Label15.Text = "Pre Rate %"
        '
        'txtDiaRate_PER
        '
        Me.txtDiaRate_PER.Location = New System.Drawing.Point(269, 235)
        Me.txtDiaRate_PER.Name = "txtDiaRate_PER"
        Me.txtDiaRate_PER.Size = New System.Drawing.Size(77, 21)
        Me.txtDiaRate_PER.TabIndex = 21
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(192, 239)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(72, 13)
        Me.Label16.TabIndex = 20
        Me.Label16.Text = "Dia Rate %"
        '
        'cmbCostCentre
        '
        Me.cmbCostCentre.FormattingEnabled = True
        Me.cmbCostCentre.Location = New System.Drawing.Point(109, 20)
        Me.cmbCostCentre.Name = "cmbCostCentre"
        Me.cmbCostCentre.Size = New System.Drawing.Size(232, 21)
        Me.cmbCostCentre.TabIndex = 1
        '
        'lblCostCentre
        '
        Me.lblCostCentre.AutoSize = True
        Me.lblCostCentre.Location = New System.Drawing.Point(10, 24)
        Me.lblCostCentre.Name = "lblCostCentre"
        Me.lblCostCentre.Size = New System.Drawing.Size(76, 13)
        Me.lblCostCentre.TabIndex = 0
        Me.lblCostCentre.Text = "Cost Centre"
        '
        'txtStnRate_AMT
        '
        Me.txtStnRate_AMT.Location = New System.Drawing.Point(109, 273)
        Me.txtStnRate_AMT.Name = "txtStnRate_AMT"
        Me.txtStnRate_AMT.Size = New System.Drawing.Size(77, 21)
        Me.txtStnRate_AMT.TabIndex = 23
        '
        'txtPreRate_AMT
        '
        Me.txtPreRate_AMT.Location = New System.Drawing.Point(109, 310)
        Me.txtPreRate_AMT.Name = "txtPreRate_AMT"
        Me.txtPreRate_AMT.Size = New System.Drawing.Size(77, 21)
        Me.txtPreRate_AMT.TabIndex = 27
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(11, 277)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(56, 13)
        Me.Label12.TabIndex = 22
        Me.Label12.Text = "Stn Rate"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(11, 314)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(56, 13)
        Me.Label13.TabIndex = 26
        Me.Label13.Text = "Pre Rate"
        '
        'txtWastage_WET
        '
        Me.txtWastage_WET.Location = New System.Drawing.Point(269, 125)
        Me.txtWastage_WET.Name = "txtWastage_WET"
        Me.txtWastage_WET.Size = New System.Drawing.Size(77, 21)
        Me.txtWastage_WET.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 92)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Margin Name"
        '
        'txtMarginName
        '
        Me.txtMarginName.Location = New System.Drawing.Point(109, 88)
        Me.txtMarginName.MaxLength = 5
        Me.txtMarginName.Name = "txtMarginName"
        Me.txtMarginName.Size = New System.Drawing.Size(77, 21)
        Me.txtMarginName.TabIndex = 5
        '
        'txtMc_AMT
        '
        Me.txtMc_AMT.Location = New System.Drawing.Point(269, 162)
        Me.txtMc_AMT.Name = "txtMc_AMT"
        Me.txtMc_AMT.Size = New System.Drawing.Size(77, 21)
        Me.txtMc_AMT.TabIndex = 13
        '
        'txtWastPer_PER
        '
        Me.txtWastPer_PER.Location = New System.Drawing.Point(109, 125)
        Me.txtWastPer_PER.Name = "txtWastPer_PER"
        Me.txtWastPer_PER.Size = New System.Drawing.Size(77, 21)
        Me.txtWastPer_PER.TabIndex = 7
        '
        'txtDisplayOrder_NUM
        '
        Me.txtDisplayOrder_NUM.Location = New System.Drawing.Point(109, 347)
        Me.txtDisplayOrder_NUM.Name = "txtDisplayOrder_NUM"
        Me.txtDisplayOrder_NUM.Size = New System.Drawing.Size(77, 21)
        Me.txtDisplayOrder_NUM.TabIndex = 31
        '
        'txtDiaRate_AMT
        '
        Me.txtDiaRate_AMT.Location = New System.Drawing.Point(109, 236)
        Me.txtDiaRate_AMT.Name = "txtDiaRate_AMT"
        Me.txtDiaRate_AMT.Size = New System.Drawing.Size(77, 21)
        Me.txtDiaRate_AMT.TabIndex = 19
        '
        'txtFixedValue_AMT
        '
        Me.txtFixedValue_AMT.Location = New System.Drawing.Point(109, 199)
        Me.txtFixedValue_AMT.Name = "txtFixedValue_AMT"
        Me.txtFixedValue_AMT.Size = New System.Drawing.Size(77, 21)
        Me.txtFixedValue_AMT.TabIndex = 15
        '
        'txtMcGrm_AMT
        '
        Me.txtMcGrm_AMT.Location = New System.Drawing.Point(109, 162)
        Me.txtMcGrm_AMT.Name = "txtMcGrm_AMT"
        Me.txtMcGrm_AMT.Size = New System.Drawing.Size(77, 21)
        Me.txtMcGrm_AMT.TabIndex = 11
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(188, 166)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(22, 13)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Mc"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(11, 351)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(86, 13)
        Me.Label9.TabIndex = 30
        Me.Label9.Text = "Display Order"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(203, 388)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(49, 13)
        Me.Label10.TabIndex = 34
        Me.Label10.Text = "Display"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(11, 388)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(85, 13)
        Me.Label8.TabIndex = 32
        Me.Label8.Text = "Get Password"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(11, 240)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(56, 13)
        Me.Label11.TabIndex = 18
        Me.Label11.Text = "Dia Rate"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(11, 203)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(73, 13)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "Fixed Value"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(11, 166)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(52, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Mc/Grm"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(188, 129)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(56, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Wastage"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(11, 129)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(51, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Wast %"
        '
        'cmbDisplay
        '
        Me.cmbDisplay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDisplay.FormattingEnabled = True
        Me.cmbDisplay.Location = New System.Drawing.Point(254, 384)
        Me.cmbDisplay.Name = "cmbDisplay"
        Me.cmbDisplay.Size = New System.Drawing.Size(77, 21)
        Me.cmbDisplay.TabIndex = 35
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(11, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(37, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Metal"
        '
        'cmbGetPwd
        '
        Me.cmbGetPwd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbGetPwd.FormattingEnabled = True
        Me.cmbGetPwd.Location = New System.Drawing.Point(109, 384)
        Me.cmbGetPwd.Name = "cmbGetPwd"
        Me.cmbGetPwd.Size = New System.Drawing.Size(77, 21)
        Me.cmbGetPwd.TabIndex = 33
        '
        'cmbMetalName
        '
        Me.cmbMetalName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMetalName.FormattingEnabled = True
        Me.cmbMetalName.Location = New System.Drawing.Point(109, 51)
        Me.cmbMetalName.Name = "cmbMetalName"
        Me.cmbMetalName.Size = New System.Drawing.Size(175, 21)
        Me.cmbMetalName.TabIndex = 3
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(335, 424)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 39
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(228, 424)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 38
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(120, 424)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 37
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(14, 424)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 36
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.grpContainer2)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(977, 510)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'grpContainer2
        '
        Me.grpContainer2.Controls.Add(Me.btnBack)
        Me.grpContainer2.Controls.Add(Me.gridView)
        Me.grpContainer2.Location = New System.Drawing.Point(9, 21)
        Me.grpContainer2.Name = "grpContainer2"
        Me.grpContainer2.Size = New System.Drawing.Size(955, 481)
        Me.grpContainer2.TabIndex = 2
        Me.grpContainer2.TabStop = False
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(6, 20)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 0
        Me.btnBack.Text = "&Back"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(6, 54)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(941, 420)
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
        'txtMc_PER
        '
        Me.txtMc_PER.Location = New System.Drawing.Point(269, 200)
        Me.txtMc_PER.Name = "txtMc_PER"
        Me.txtMc_PER.Size = New System.Drawing.Size(77, 21)
        Me.txtMc_PER.TabIndex = 17
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(188, 204)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(74, 13)
        Me.Label17.TabIndex = 16
        Me.Label17.Text = "Mc Value %"
        '
        'VAMARGIN
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(985, 536)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "VAMARGIN"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "VAMARGIN"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.grpContainer.ResumeLayout(False)
        Me.grpContainer.PerformLayout()
        Me.tabView.ResumeLayout(False)
        Me.grpContainer2.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents grpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtMarginName As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbMetalName As System.Windows.Forms.ComboBox
    Friend WithEvents txtWastage_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtMc_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtWastPer_PER As System.Windows.Forms.TextBox
    Friend WithEvents txtMcGrm_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtDisplayOrder_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtFixedValue_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmbGetPwd As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents cmbDisplay As System.Windows.Forms.ComboBox
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents txtDiaRate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents grpContainer2 As System.Windows.Forms.GroupBox
    Friend WithEvents txtStnRate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtPreRate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents cmbCostCentre As System.Windows.Forms.ComboBox
    Friend WithEvents lblCostCentre As System.Windows.Forms.Label
    Friend WithEvents txtStnRate_PER As System.Windows.Forms.TextBox
    Friend WithEvents txtPreRate_PER As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtDiaRate_PER As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents txtMc_PER As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
End Class
