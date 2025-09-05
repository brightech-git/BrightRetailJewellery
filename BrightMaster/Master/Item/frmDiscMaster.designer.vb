<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDiscMaster
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.chkLstCostcentre = New System.Windows.Forms.CheckedListBox
        Me.cmbState = New System.Windows.Forms.ComboBox
        Me.txtStuddedDiamondRs_AMT = New System.Windows.Forms.TextBox
        Me.txtStuddedStonesRs_AMT = New System.Windows.Forms.TextBox
        Me.cmbWithWastage = New System.Windows.Forms.ComboBox
        Me.cmbBasedOn = New System.Windows.Forms.ComboBox
        Me.pnlControls = New System.Windows.Forms.Panel
        Me.cmbType = New System.Windows.Forms.ComboBox
        Me.Label19 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.cmbSubItem = New System.Windows.Forms.ComboBox
        Me.cmbMetalName_Man = New System.Windows.Forms.ComboBox
        Me.cmbItemName_Man = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmbDiscountGroup = New System.Windows.Forms.ComboBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.Label8 = New System.Windows.Forms.Label
        Me.btnOpen = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtBoardRate_Amt = New System.Windows.Forms.TextBox
        Me.txtStuddedDiamond_Per = New System.Windows.Forms.TextBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label21 = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.txtMakingCharge_Amt = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtStuddedStones_Per = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.txtWastage_Per = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtMaking_Per = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.txtOnFinal_Amt = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtOnFinalAmt_Per = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.cmbAfterTax = New System.Windows.Forms.ComboBox
        Me.cmbActive = New System.Windows.Forms.ComboBox
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
        Me.btnBack = New System.Windows.Forms.Button
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.pnlControls.SuspendLayout()
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
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox1.Controls.Add(Me.chkLstCostcentre)
        Me.GroupBox1.Controls.Add(Me.Label16)
        Me.GroupBox1.Controls.Add(Me.Label17)
        Me.GroupBox1.Controls.Add(Me.cmbState)
        Me.GroupBox1.Controls.Add(Me.cmbBasedOn)
        Me.GroupBox1.Controls.Add(Me.txtStuddedDiamondRs_AMT)
        Me.GroupBox1.Controls.Add(Me.cmbWithWastage)
        Me.GroupBox1.Controls.Add(Me.txtStuddedStonesRs_AMT)
        Me.GroupBox1.Controls.Add(Me.pnlControls)
        Me.GroupBox1.Controls.Add(Me.btnExit)
        Me.GroupBox1.Controls.Add(Me.btnNew)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.btnOpen)
        Me.GroupBox1.Controls.Add(Me.btnSave)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.txtBoardRate_Amt)
        Me.GroupBox1.Controls.Add(Me.txtStuddedDiamond_Per)
        Me.GroupBox1.Controls.Add(Me.Label18)
        Me.GroupBox1.Controls.Add(Me.Label21)
        Me.GroupBox1.Controls.Add(Me.Label20)
        Me.GroupBox1.Controls.Add(Me.Label15)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.txtMakingCharge_Amt)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.txtStuddedStones_Per)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.txtWastage_Per)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.txtMaking_Per)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.txtOnFinal_Amt)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.txtOnFinalAmt_Per)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.Label14)
        Me.GroupBox1.Controls.Add(Me.cmbAfterTax)
        Me.GroupBox1.Controls.Add(Me.cmbActive)
        Me.GroupBox1.Location = New System.Drawing.Point(287, 36)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(477, 508)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'chkLstCostcentre
        '
        Me.chkLstCostcentre.FormattingEnabled = True
        Me.chkLstCostcentre.Location = New System.Drawing.Point(143, 346)
        Me.chkLstCostcentre.Name = "chkLstCostcentre"
        Me.chkLstCostcentre.Size = New System.Drawing.Size(321, 116)
        Me.chkLstCostcentre.TabIndex = 32
        '
        'cmbState
        '
        Me.cmbState.FormattingEnabled = True
        Me.cmbState.Location = New System.Drawing.Point(143, 318)
        Me.cmbState.Name = "cmbState"
        Me.cmbState.Size = New System.Drawing.Size(323, 21)
        Me.cmbState.TabIndex = 30
        '
        'txtStuddedDiamondRs_AMT
        '
        Me.txtStuddedDiamondRs_AMT.Location = New System.Drawing.Point(370, 210)
        Me.txtStuddedDiamondRs_AMT.Name = "txtStuddedDiamondRs_AMT"
        Me.txtStuddedDiamondRs_AMT.Size = New System.Drawing.Size(96, 21)
        Me.txtStuddedDiamondRs_AMT.TabIndex = 22
        '
        'txtStuddedStonesRs_AMT
        '
        Me.txtStuddedStonesRs_AMT.Location = New System.Drawing.Point(370, 155)
        Me.txtStuddedStonesRs_AMT.Name = "txtStuddedStonesRs_AMT"
        Me.txtStuddedStonesRs_AMT.Size = New System.Drawing.Size(96, 21)
        Me.txtStuddedStonesRs_AMT.TabIndex = 18
        '
        'cmbWithWastage
        '
        Me.cmbWithWastage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbWithWastage.FormattingEnabled = True
        Me.cmbWithWastage.Location = New System.Drawing.Point(143, 290)
        Me.cmbWithWastage.Name = "cmbWithWastage"
        Me.cmbWithWastage.Size = New System.Drawing.Size(96, 21)
        Me.cmbWithWastage.TabIndex = 14
        '
        'cmbBasedOn
        '
        Me.cmbBasedOn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBasedOn.FormattingEnabled = True
        Me.cmbBasedOn.Location = New System.Drawing.Point(143, 263)
        Me.cmbBasedOn.Name = "cmbBasedOn"
        Me.cmbBasedOn.Size = New System.Drawing.Size(96, 21)
        Me.cmbBasedOn.TabIndex = 12
        '
        'pnlControls
        '
        Me.pnlControls.BackColor = System.Drawing.Color.Transparent
        Me.pnlControls.Controls.Add(Me.cmbType)
        Me.pnlControls.Controls.Add(Me.Label19)
        Me.pnlControls.Controls.Add(Me.Label3)
        Me.pnlControls.Controls.Add(Me.Label1)
        Me.pnlControls.Controls.Add(Me.Label4)
        Me.pnlControls.Controls.Add(Me.cmbSubItem)
        Me.pnlControls.Controls.Add(Me.cmbMetalName_Man)
        Me.pnlControls.Controls.Add(Me.cmbItemName_Man)
        Me.pnlControls.Controls.Add(Me.Label2)
        Me.pnlControls.Controls.Add(Me.cmbDiscountGroup)
        Me.pnlControls.Location = New System.Drawing.Point(7, 20)
        Me.pnlControls.Name = "pnlControls"
        Me.pnlControls.Size = New System.Drawing.Size(465, 104)
        Me.pnlControls.TabIndex = 0
        '
        'cmbType
        '
        Me.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbType.FormattingEnabled = True
        Me.cmbType.Location = New System.Drawing.Point(136, 1)
        Me.cmbType.Name = "cmbType"
        Me.cmbType.Size = New System.Drawing.Size(96, 21)
        Me.cmbType.TabIndex = 1
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(3, 5)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(35, 13)
        Me.Label19.TabIndex = 0
        Me.Label19.Text = "Type"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(4, 59)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(34, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Item"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(238, 5)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(95, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Discount Group"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(4, 86)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(60, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Sub Item"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbSubItem
        '
        Me.cmbSubItem.FormattingEnabled = True
        Me.cmbSubItem.Location = New System.Drawing.Point(136, 82)
        Me.cmbSubItem.Name = "cmbSubItem"
        Me.cmbSubItem.Size = New System.Drawing.Size(323, 21)
        Me.cmbSubItem.TabIndex = 9
        '
        'cmbMetalName_Man
        '
        Me.cmbMetalName_Man.FormattingEnabled = True
        Me.cmbMetalName_Man.Location = New System.Drawing.Point(136, 28)
        Me.cmbMetalName_Man.Name = "cmbMetalName_Man"
        Me.cmbMetalName_Man.Size = New System.Drawing.Size(323, 21)
        Me.cmbMetalName_Man.TabIndex = 5
        '
        'cmbItemName_Man
        '
        Me.cmbItemName_Man.FormattingEnabled = True
        Me.cmbItemName_Man.Location = New System.Drawing.Point(136, 55)
        Me.cmbItemName_Man.Name = "cmbItemName_Man"
        Me.cmbItemName_Man.Size = New System.Drawing.Size(323, 21)
        Me.cmbItemName_Man.TabIndex = 7
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(4, 32)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(74, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Metal Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbDiscountGroup
        '
        Me.cmbDiscountGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDiscountGroup.FormattingEnabled = True
        Me.cmbDiscountGroup.Location = New System.Drawing.Point(336, 1)
        Me.cmbDiscountGroup.Name = "cmbDiscountGroup"
        Me.cmbDiscountGroup.Size = New System.Drawing.Size(123, 21)
        Me.cmbDiscountGroup.TabIndex = 3
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(367, 470)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 36
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(259, 470)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 35
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(11, 241)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(127, 13)
        Me.Label8.TabIndex = 9
        Me.Label8.Text = "Making Charge/ Grm"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(151, 470)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 34
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(43, 470)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 33
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(11, 214)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(83, 13)
        Me.Label9.TabIndex = 7
        Me.Label9.Text = "On Making %"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtBoardRate_Amt
        '
        Me.txtBoardRate_Amt.Location = New System.Drawing.Point(370, 237)
        Me.txtBoardRate_Amt.MaxLength = 10
        Me.txtBoardRate_Amt.Name = "txtBoardRate_Amt"
        Me.txtBoardRate_Amt.Size = New System.Drawing.Size(96, 21)
        Me.txtBoardRate_Amt.TabIndex = 24
        '
        'txtStuddedDiamond_Per
        '
        Me.txtStuddedDiamond_Per.Location = New System.Drawing.Point(370, 183)
        Me.txtStuddedDiamond_Per.MaxLength = 10
        Me.txtStuddedDiamond_Per.Name = "txtStuddedDiamond_Per"
        Me.txtStuddedDiamond_Per.Size = New System.Drawing.Size(96, 21)
        Me.txtStuddedDiamond_Per.TabIndex = 20
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(243, 214)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(127, 13)
        Me.Label18.TabIndex = 21
        Me.Label18.Text = "Studded Diamond Rs"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(11, 350)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(76, 13)
        Me.Label21.TabIndex = 31
        Me.Label21.Text = "Cost Centre"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(11, 324)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(37, 13)
        Me.Label20.TabIndex = 29
        Me.Label20.Text = "State"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(243, 159)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(115, 13)
        Me.Label15.TabIndex = 17
        Me.Label15.Text = "Studded Stones Rs"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(243, 132)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(113, 13)
        Me.Label10.TabIndex = 15
        Me.Label10.Text = "Studded Stones %"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtMakingCharge_Amt
        '
        Me.txtMakingCharge_Amt.Location = New System.Drawing.Point(143, 237)
        Me.txtMakingCharge_Amt.MaxLength = 10
        Me.txtMakingCharge_Amt.Name = "txtMakingCharge_Amt"
        Me.txtMakingCharge_Amt.Size = New System.Drawing.Size(96, 21)
        Me.txtMakingCharge_Amt.TabIndex = 10
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(11, 133)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(96, 13)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "On Final Amt %"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtStuddedStones_Per
        '
        Me.txtStuddedStones_Per.Location = New System.Drawing.Point(370, 128)
        Me.txtStuddedStones_Per.MaxLength = 10
        Me.txtStuddedStones_Per.Name = "txtStuddedStones_Per"
        Me.txtStuddedStones_Per.Size = New System.Drawing.Size(96, 21)
        Me.txtStuddedStones_Per.TabIndex = 16
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(243, 187)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(125, 13)
        Me.Label11.TabIndex = 19
        Me.Label11.Text = "Studded Diamond %"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtWastage_Per
        '
        Me.txtWastage_Per.Location = New System.Drawing.Point(143, 183)
        Me.txtWastage_Per.MaxLength = 10
        Me.txtWastage_Per.Name = "txtWastage_Per"
        Me.txtWastage_Per.Size = New System.Drawing.Size(96, 21)
        Me.txtWastage_Per.TabIndex = 6
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(11, 160)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(80, 13)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "On Final Amt"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtMaking_Per
        '
        Me.txtMaking_Per.Location = New System.Drawing.Point(143, 210)
        Me.txtMaking_Per.MaxLength = 10
        Me.txtMaking_Per.Name = "txtMaking_Per"
        Me.txtMaking_Per.Size = New System.Drawing.Size(96, 21)
        Me.txtMaking_Per.TabIndex = 8
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(243, 241)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(119, 13)
        Me.Label12.TabIndex = 23
        Me.Label12.Text = "Disc On Board Rate"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtOnFinal_Amt
        '
        Me.txtOnFinal_Amt.Location = New System.Drawing.Point(143, 156)
        Me.txtOnFinal_Amt.MaxLength = 10
        Me.txtOnFinal_Amt.Name = "txtOnFinal_Amt"
        Me.txtOnFinal_Amt.Size = New System.Drawing.Size(96, 21)
        Me.txtOnFinal_Amt.TabIndex = 4
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(11, 187)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(92, 13)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "On Wastage %"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtOnFinalAmt_Per
        '
        Me.txtOnFinalAmt_Per.Location = New System.Drawing.Point(143, 129)
        Me.txtOnFinalAmt_Per.MaxLength = 10
        Me.txtOnFinalAmt_Per.Name = "txtOnFinalAmt_Per"
        Me.txtOnFinalAmt_Per.Size = New System.Drawing.Size(96, 21)
        Me.txtOnFinalAmt_Per.TabIndex = 2
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(243, 267)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(60, 13)
        Me.Label13.TabIndex = 25
        Me.Label13.Text = "After Tax"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(11, 294)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(85, 13)
        Me.Label17.TabIndex = 13
        Me.Label17.Text = "With Wastage"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(11, 267)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(62, 13)
        Me.Label16.TabIndex = 11
        Me.Label16.Text = "Based On"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(243, 294)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(42, 13)
        Me.Label14.TabIndex = 27
        Me.Label14.Text = "Active"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbAfterTax
        '
        Me.cmbAfterTax.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbAfterTax.FormattingEnabled = True
        Me.cmbAfterTax.Location = New System.Drawing.Point(370, 263)
        Me.cmbAfterTax.Name = "cmbAfterTax"
        Me.cmbAfterTax.Size = New System.Drawing.Size(96, 21)
        Me.cmbAfterTax.TabIndex = 26
        '
        'cmbActive
        '
        Me.cmbActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbActive.FormattingEnabled = True
        Me.cmbActive.Location = New System.Drawing.Point(370, 290)
        Me.cmbActive.Name = "cmbActive"
        Me.cmbActive.Size = New System.Drawing.Size(96, 21)
        Me.cmbActive.TabIndex = 28
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
        Me.lblStatus.Location = New System.Drawing.Point(6, 559)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(103, 13)
        Me.lblStatus.TabIndex = 25
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
        Me.tabMain.Size = New System.Drawing.Size(1028, 616)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.GroupBox1)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 22)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(1020, 590)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
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
        Me.tabView.Size = New System.Drawing.Size(1020, 590)
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
        'frmDiscMaster
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 616)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmDiscMaster"
        Me.Text = "Discount Master"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.pnlControls.ResumeLayout(False)
        Me.pnlControls.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.tabView.ResumeLayout(False)
        Me.tabView.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbDiscountGroup As System.Windows.Forms.ComboBox
    Friend WithEvents cmbMetalName_Man As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents txtBoardRate_Amt As System.Windows.Forms.TextBox
    Friend WithEvents txtStuddedDiamond_Per As System.Windows.Forms.TextBox
    Friend WithEvents txtMakingCharge_Amt As System.Windows.Forms.TextBox
    Friend WithEvents txtStuddedStones_Per As System.Windows.Forms.TextBox
    Friend WithEvents txtWastage_Per As System.Windows.Forms.TextBox
    Friend WithEvents txtMaking_Per As System.Windows.Forms.TextBox
    Friend WithEvents txtOnFinal_Amt As System.Windows.Forms.TextBox
    Friend WithEvents txtOnFinalAmt_Per As System.Windows.Forms.TextBox
    Friend WithEvents cmbSubItem As System.Windows.Forms.ComboBox
    Friend WithEvents cmbItemName_Man As System.Windows.Forms.ComboBox
    Friend WithEvents cmbActive As System.Windows.Forms.ComboBox
    Friend WithEvents cmbAfterTax As System.Windows.Forms.ComboBox
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlControls As System.Windows.Forms.Panel
    Friend WithEvents cmbWithWastage As System.Windows.Forms.ComboBox
    Friend WithEvents cmbBasedOn As System.Windows.Forms.ComboBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents txtStuddedStonesRs_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtStuddedDiamondRs_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents cmbType As System.Windows.Forms.ComboBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents chkLstCostcentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents cmbState As System.Windows.Forms.ComboBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
End Class
