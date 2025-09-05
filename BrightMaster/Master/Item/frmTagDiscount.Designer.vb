<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTagDiscount
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
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.pnlControls = New System.Windows.Forms.Panel
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.txtLessWastPer_PER = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label23 = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.chkLstCostcentre = New System.Windows.Forms.CheckedListBox
        Me.btnNew = New System.Windows.Forms.Button
        Me.Label15 = New System.Windows.Forms.Label
        Me.btnOpen = New System.Windows.Forms.Button
        Me.cmbState = New System.Windows.Forms.ComboBox
        Me.btnSave = New System.Windows.Forms.Button
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label21 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.txtStuddedDiamond_Per = New System.Windows.Forms.TextBox
        Me.cmbActive = New System.Windows.Forms.ComboBox
        Me.Label20 = New System.Windows.Forms.Label
        Me.txtMakingCharge_Amt = New System.Windows.Forms.TextBox
        Me.txtBoardRate_Amt = New System.Windows.Forms.TextBox
        Me.cmbAfterTax = New System.Windows.Forms.ComboBox
        Me.txtStuddedStones_Per = New System.Windows.Forms.TextBox
        Me.cmbBasedOn = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.txtStuddedDiamondRs_AMT = New System.Windows.Forms.TextBox
        Me.txtWastage_Per = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.cmbWithWastage = New System.Windows.Forms.ComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtOnFinalAmt_Per = New System.Windows.Forms.TextBox
        Me.txtMaking_Per = New System.Windows.Forms.TextBox
        Me.txtStuddedStonesRs_AMT = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtOnFinal_Amt = New System.Windows.Forms.TextBox
        Me.pnlItemGroup = New System.Windows.Forms.Panel
        Me.Label3 = New System.Windows.Forms.Label
        Me.cmbItemName_Man = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.cmbSubItem = New System.Windows.Forms.ComboBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmbMetalName_Man = New System.Windows.Forms.ComboBox
        Me.tabView = New System.Windows.Forms.TabPage
        Me.btnBack = New System.Windows.Forms.Button
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.lblStatus = New System.Windows.Forms.Label
        Me.btnDelete = New System.Windows.Forms.Button
        Me.Label24 = New System.Windows.Forms.Label
        Me.Label25 = New System.Windows.Forms.Label
        Me.txtRangefrom_NUM = New System.Windows.Forms.TextBox
        Me.txtRangeTo_NUM = New System.Windows.Forms.TextBox
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.pnlControls.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.pnlItemGroup.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.tabView.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
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
        Me.tabMain.Controls.Add(Me.tabGeneral)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1022, 640)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.pnlControls)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 22)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(1014, 614)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'pnlControls
        '
        Me.pnlControls.BackColor = System.Drawing.Color.Transparent
        Me.pnlControls.Controls.Add(Me.Panel4)
        Me.pnlControls.Controls.Add(Me.pnlItemGroup)
        Me.pnlControls.Controls.Add(Me.Panel1)
        Me.pnlControls.Location = New System.Drawing.Point(258, 22)
        Me.pnlControls.Name = "pnlControls"
        Me.pnlControls.Size = New System.Drawing.Size(465, 580)
        Me.pnlControls.TabIndex = 0
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.txtRangeTo_NUM)
        Me.Panel4.Controls.Add(Me.txtRangefrom_NUM)
        Me.Panel4.Controls.Add(Me.Label25)
        Me.Panel4.Controls.Add(Me.Label24)
        Me.Panel4.Controls.Add(Me.txtLessWastPer_PER)
        Me.Panel4.Controls.Add(Me.Label5)
        Me.Panel4.Controls.Add(Me.Label23)
        Me.Panel4.Controls.Add(Me.btnExit)
        Me.Panel4.Controls.Add(Me.chkLstCostcentre)
        Me.Panel4.Controls.Add(Me.btnNew)
        Me.Panel4.Controls.Add(Me.Label15)
        Me.Panel4.Controls.Add(Me.btnOpen)
        Me.Panel4.Controls.Add(Me.cmbState)
        Me.Panel4.Controls.Add(Me.btnSave)
        Me.Panel4.Controls.Add(Me.Label16)
        Me.Panel4.Controls.Add(Me.Label18)
        Me.Panel4.Controls.Add(Me.Label10)
        Me.Panel4.Controls.Add(Me.Label21)
        Me.Panel4.Controls.Add(Me.Label17)
        Me.Panel4.Controls.Add(Me.txtStuddedDiamond_Per)
        Me.Panel4.Controls.Add(Me.cmbActive)
        Me.Panel4.Controls.Add(Me.Label20)
        Me.Panel4.Controls.Add(Me.txtMakingCharge_Amt)
        Me.Panel4.Controls.Add(Me.txtBoardRate_Amt)
        Me.Panel4.Controls.Add(Me.cmbAfterTax)
        Me.Panel4.Controls.Add(Me.txtStuddedStones_Per)
        Me.Panel4.Controls.Add(Me.cmbBasedOn)
        Me.Panel4.Controls.Add(Me.Label9)
        Me.Panel4.Controls.Add(Me.Label14)
        Me.Panel4.Controls.Add(Me.Label11)
        Me.Panel4.Controls.Add(Me.txtStuddedDiamondRs_AMT)
        Me.Panel4.Controls.Add(Me.txtWastage_Per)
        Me.Panel4.Controls.Add(Me.Label13)
        Me.Panel4.Controls.Add(Me.Label6)
        Me.Panel4.Controls.Add(Me.cmbWithWastage)
        Me.Panel4.Controls.Add(Me.Label8)
        Me.Panel4.Controls.Add(Me.txtOnFinalAmt_Per)
        Me.Panel4.Controls.Add(Me.txtMaking_Per)
        Me.Panel4.Controls.Add(Me.txtStuddedStonesRs_AMT)
        Me.Panel4.Controls.Add(Me.Label12)
        Me.Panel4.Controls.Add(Me.Label7)
        Me.Panel4.Controls.Add(Me.txtOnFinal_Amt)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel4.Location = New System.Drawing.Point(0, 78)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(465, 502)
        Me.Panel4.TabIndex = 3
        '
        'txtLessWastPer_PER
        '
        Me.txtLessWastPer_PER.Location = New System.Drawing.Point(136, 126)
        Me.txtLessWastPer_PER.Name = "txtLessWastPer_PER"
        Me.txtLessWastPer_PER.Size = New System.Drawing.Size(96, 21)
        Me.txtLessWastPer_PER.TabIndex = 13
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(3, 34)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(96, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "On Final Amt %"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(3, 132)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(115, 13)
        Me.Label23.TabIndex = 12
        Me.Label23.Text = "Less Wast% On Wt"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(358, 319)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 41
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'chkLstCostcentre
        '
        Me.chkLstCostcentre.FormattingEnabled = True
        Me.chkLstCostcentre.Location = New System.Drawing.Point(136, 246)
        Me.chkLstCostcentre.Name = "chkLstCostcentre"
        Me.chkLstCostcentre.Size = New System.Drawing.Size(322, 68)
        Me.chkLstCostcentre.TabIndex = 37
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(250, 319)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 40
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(235, 58)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(115, 13)
        Me.Label15.TabIndex = 22
        Me.Label15.Text = "Studded Stones Rs"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(142, 319)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 39
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'cmbState
        '
        Me.cmbState.FormattingEnabled = True
        Me.cmbState.Location = New System.Drawing.Point(136, 222)
        Me.cmbState.Name = "cmbState"
        Me.cmbState.Size = New System.Drawing.Size(322, 21)
        Me.cmbState.TabIndex = 35
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(34, 319)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 38
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(3, 178)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(62, 13)
        Me.Label16.TabIndex = 16
        Me.Label16.Text = "Based On"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(235, 106)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(127, 13)
        Me.Label18.TabIndex = 26
        Me.Label18.Text = "Studded Diamond Rs"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(235, 34)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(113, 13)
        Me.Label10.TabIndex = 20
        Me.Label10.Text = "Studded Stones %"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(3, 246)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(76, 13)
        Me.Label21.TabIndex = 36
        Me.Label21.Text = "Cost Centre"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(3, 202)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(85, 13)
        Me.Label17.TabIndex = 18
        Me.Label17.Text = "With Wastage"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtStuddedDiamond_Per
        '
        Me.txtStuddedDiamond_Per.Location = New System.Drawing.Point(362, 78)
        Me.txtStuddedDiamond_Per.MaxLength = 10
        Me.txtStuddedDiamond_Per.Name = "txtStuddedDiamond_Per"
        Me.txtStuddedDiamond_Per.Size = New System.Drawing.Size(96, 21)
        Me.txtStuddedDiamond_Per.TabIndex = 25
        '
        'cmbActive
        '
        Me.cmbActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbActive.FormattingEnabled = True
        Me.cmbActive.Location = New System.Drawing.Point(362, 198)
        Me.cmbActive.Name = "cmbActive"
        Me.cmbActive.Size = New System.Drawing.Size(96, 21)
        Me.cmbActive.TabIndex = 33
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(3, 228)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(37, 13)
        Me.Label20.TabIndex = 34
        Me.Label20.Text = "State"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtMakingCharge_Amt
        '
        Me.txtMakingCharge_Amt.Location = New System.Drawing.Point(136, 150)
        Me.txtMakingCharge_Amt.MaxLength = 10
        Me.txtMakingCharge_Amt.Name = "txtMakingCharge_Amt"
        Me.txtMakingCharge_Amt.Size = New System.Drawing.Size(96, 21)
        Me.txtMakingCharge_Amt.TabIndex = 15
        '
        'txtBoardRate_Amt
        '
        Me.txtBoardRate_Amt.Location = New System.Drawing.Point(362, 150)
        Me.txtBoardRate_Amt.MaxLength = 10
        Me.txtBoardRate_Amt.Name = "txtBoardRate_Amt"
        Me.txtBoardRate_Amt.Size = New System.Drawing.Size(96, 21)
        Me.txtBoardRate_Amt.TabIndex = 29
        '
        'cmbAfterTax
        '
        Me.cmbAfterTax.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbAfterTax.FormattingEnabled = True
        Me.cmbAfterTax.Location = New System.Drawing.Point(362, 174)
        Me.cmbAfterTax.Name = "cmbAfterTax"
        Me.cmbAfterTax.Size = New System.Drawing.Size(96, 21)
        Me.cmbAfterTax.TabIndex = 31
        '
        'txtStuddedStones_Per
        '
        Me.txtStuddedStones_Per.Location = New System.Drawing.Point(362, 30)
        Me.txtStuddedStones_Per.MaxLength = 10
        Me.txtStuddedStones_Per.Name = "txtStuddedStones_Per"
        Me.txtStuddedStones_Per.Size = New System.Drawing.Size(96, 21)
        Me.txtStuddedStones_Per.TabIndex = 21
        '
        'cmbBasedOn
        '
        Me.cmbBasedOn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBasedOn.FormattingEnabled = True
        Me.cmbBasedOn.Location = New System.Drawing.Point(136, 174)
        Me.cmbBasedOn.Name = "cmbBasedOn"
        Me.cmbBasedOn.Size = New System.Drawing.Size(96, 21)
        Me.cmbBasedOn.TabIndex = 17
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(3, 106)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(83, 13)
        Me.Label9.TabIndex = 10
        Me.Label9.Text = "On Making %"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(235, 202)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(42, 13)
        Me.Label14.TabIndex = 32
        Me.Label14.Text = "Active"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(235, 82)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(125, 13)
        Me.Label11.TabIndex = 24
        Me.Label11.Text = "Studded Diamond %"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtStuddedDiamondRs_AMT
        '
        Me.txtStuddedDiamondRs_AMT.Location = New System.Drawing.Point(362, 102)
        Me.txtStuddedDiamondRs_AMT.Name = "txtStuddedDiamondRs_AMT"
        Me.txtStuddedDiamondRs_AMT.Size = New System.Drawing.Size(96, 21)
        Me.txtStuddedDiamondRs_AMT.TabIndex = 27
        '
        'txtWastage_Per
        '
        Me.txtWastage_Per.Location = New System.Drawing.Point(136, 78)
        Me.txtWastage_Per.MaxLength = 10
        Me.txtWastage_Per.Name = "txtWastage_Per"
        Me.txtWastage_Per.Size = New System.Drawing.Size(96, 21)
        Me.txtWastage_Per.TabIndex = 9
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(235, 178)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(60, 13)
        Me.Label13.TabIndex = 30
        Me.Label13.Text = "After Tax"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(3, 58)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(80, 13)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "On Final Amt"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbWithWastage
        '
        Me.cmbWithWastage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbWithWastage.FormattingEnabled = True
        Me.cmbWithWastage.Location = New System.Drawing.Point(136, 198)
        Me.cmbWithWastage.Name = "cmbWithWastage"
        Me.cmbWithWastage.Size = New System.Drawing.Size(96, 21)
        Me.cmbWithWastage.TabIndex = 19
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(3, 154)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(127, 13)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "Making Charge/ Grm"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtOnFinalAmt_Per
        '
        Me.txtOnFinalAmt_Per.Location = New System.Drawing.Point(136, 30)
        Me.txtOnFinalAmt_Per.MaxLength = 10
        Me.txtOnFinalAmt_Per.Name = "txtOnFinalAmt_Per"
        Me.txtOnFinalAmt_Per.Size = New System.Drawing.Size(96, 21)
        Me.txtOnFinalAmt_Per.TabIndex = 5
        '
        'txtMaking_Per
        '
        Me.txtMaking_Per.Location = New System.Drawing.Point(136, 102)
        Me.txtMaking_Per.MaxLength = 10
        Me.txtMaking_Per.Name = "txtMaking_Per"
        Me.txtMaking_Per.Size = New System.Drawing.Size(96, 21)
        Me.txtMaking_Per.TabIndex = 11
        '
        'txtStuddedStonesRs_AMT
        '
        Me.txtStuddedStonesRs_AMT.Location = New System.Drawing.Point(362, 54)
        Me.txtStuddedStonesRs_AMT.Name = "txtStuddedStonesRs_AMT"
        Me.txtStuddedStonesRs_AMT.Size = New System.Drawing.Size(96, 21)
        Me.txtStuddedStonesRs_AMT.TabIndex = 23
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(235, 154)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(119, 13)
        Me.Label12.TabIndex = 28
        Me.Label12.Text = "Disc On Board Rate"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(3, 82)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(92, 13)
        Me.Label7.TabIndex = 8
        Me.Label7.Text = "On Wastage %"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtOnFinal_Amt
        '
        Me.txtOnFinal_Amt.Location = New System.Drawing.Point(136, 54)
        Me.txtOnFinal_Amt.MaxLength = 10
        Me.txtOnFinal_Amt.Name = "txtOnFinal_Amt"
        Me.txtOnFinal_Amt.Size = New System.Drawing.Size(96, 21)
        Me.txtOnFinal_Amt.TabIndex = 7
        '
        'pnlItemGroup
        '
        Me.pnlItemGroup.Controls.Add(Me.Label3)
        Me.pnlItemGroup.Controls.Add(Me.cmbItemName_Man)
        Me.pnlItemGroup.Controls.Add(Me.Label4)
        Me.pnlItemGroup.Controls.Add(Me.cmbSubItem)
        Me.pnlItemGroup.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlItemGroup.Location = New System.Drawing.Point(0, 29)
        Me.pnlItemGroup.Name = "pnlItemGroup"
        Me.pnlItemGroup.Size = New System.Drawing.Size(465, 49)
        Me.pnlItemGroup.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 6)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(34, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Item"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbItemName_Man
        '
        Me.cmbItemName_Man.FormattingEnabled = True
        Me.cmbItemName_Man.Location = New System.Drawing.Point(136, 2)
        Me.cmbItemName_Man.Name = "cmbItemName_Man"
        Me.cmbItemName_Man.Size = New System.Drawing.Size(323, 21)
        Me.cmbItemName_Man.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(3, 30)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(60, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Sub Item"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbSubItem
        '
        Me.cmbSubItem.FormattingEnabled = True
        Me.cmbSubItem.Location = New System.Drawing.Point(136, 26)
        Me.cmbSubItem.Name = "cmbSubItem"
        Me.cmbSubItem.Size = New System.Drawing.Size(323, 21)
        Me.cmbSubItem.TabIndex = 3
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.cmbMetalName_Man)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(465, 29)
        Me.Panel1.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 7)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(74, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Metal Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbMetalName_Man
        '
        Me.cmbMetalName_Man.FormattingEnabled = True
        Me.cmbMetalName_Man.Location = New System.Drawing.Point(136, 3)
        Me.cmbMetalName_Man.Name = "cmbMetalName_Man"
        Me.cmbMetalName_Man.Size = New System.Drawing.Size(322, 21)
        Me.cmbMetalName_Man.TabIndex = 5
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.btnBack)
        Me.tabView.Controls.Add(Me.gridView)
        Me.tabView.Controls.Add(Me.lblStatus)
        Me.tabView.Controls.Add(Me.btnDelete)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1014, 614)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(113, 8)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 34
        Me.btnBack.Text = "Back"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(5, 44)
        Me.gridView.MultiSelect = False
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 20
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(1007, 512)
        Me.gridView.TabIndex = 2
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(6, 559)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(103, 13)
        Me.lblStatus.TabIndex = 25
        Me.lblStatus.Text = "*Hit Enter to Edit"
        Me.lblStatus.Visible = False
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(7, 8)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnDelete.TabIndex = 33
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(3, 10)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(76, 13)
        Me.Label24.TabIndex = 0
        Me.Label24.Text = "Range From"
        Me.Label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(235, 10)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(61, 13)
        Me.Label25.TabIndex = 2
        Me.Label25.Text = "Range To"
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtRangefrom_NUM
        '
        Me.txtRangefrom_NUM.Location = New System.Drawing.Point(136, 4)
        Me.txtRangefrom_NUM.Name = "txtRangefrom_NUM"
        Me.txtRangefrom_NUM.Size = New System.Drawing.Size(96, 21)
        Me.txtRangefrom_NUM.TabIndex = 1
        '
        'txtRangeTo_NUM
        '
        Me.txtRangeTo_NUM.Location = New System.Drawing.Point(362, 4)
        Me.txtRangeTo_NUM.Name = "txtRangeTo_NUM"
        Me.txtRangeTo_NUM.Size = New System.Drawing.Size(96, 21)
        Me.txtRangeTo_NUM.TabIndex = 3
        '
        'frmTagDiscount
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1022, 640)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmTagDiscount"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TagDiscount"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.pnlControls.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.pnlItemGroup.ResumeLayout(False)
        Me.pnlItemGroup.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.tabView.ResumeLayout(False)
        Me.tabView.PerformLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents chkLstCostcentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents pnlControls As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmbSubItem As System.Windows.Forms.ComboBox
    Friend WithEvents cmbMetalName_Man As System.Windows.Forms.ComboBox
    Friend WithEvents cmbItemName_Man As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents cmbActive As System.Windows.Forms.ComboBox
    Friend WithEvents cmbState As System.Windows.Forms.ComboBox
    Friend WithEvents cmbAfterTax As System.Windows.Forms.ComboBox
    Friend WithEvents cmbBasedOn As System.Windows.Forms.ComboBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtStuddedDiamondRs_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents cmbWithWastage As System.Windows.Forms.ComboBox
    Friend WithEvents txtOnFinalAmt_Per As System.Windows.Forms.TextBox
    Friend WithEvents txtStuddedStonesRs_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtOnFinal_Amt As System.Windows.Forms.TextBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents txtMaking_Per As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents txtWastage_Per As System.Windows.Forms.TextBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtStuddedStones_Per As System.Windows.Forms.TextBox
    Friend WithEvents txtBoardRate_Amt As System.Windows.Forms.TextBox
    Friend WithEvents txtMakingCharge_Amt As System.Windows.Forms.TextBox
    Friend WithEvents txtStuddedDiamond_Per As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents pnlItemGroup As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents txtLessWastPer_PER As System.Windows.Forms.TextBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents txtRangeTo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtRangefrom_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
End Class
