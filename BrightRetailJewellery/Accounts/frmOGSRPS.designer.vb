<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOGSRPS
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.grpStone = New System.Windows.Forms.GroupBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.txtCtrstkbalwt = New System.Windows.Forms.TextBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.txtCtrstkAddwt = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtCtrstkTotwt = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.TextBox4 = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.cmbSubItemName_Man = New System.Windows.Forms.ComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtPiece_Num_Man = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.txtItemName = New System.Windows.Forms.TextBox
        Me.txtNoOfTags_Num = New System.Windows.Forms.TextBox
        Me.txtItemCode_Num_Man = New System.Windows.Forms.TextBox
        Me.Label30 = New System.Windows.Forms.Label
        Me.cmbItemCounter_MAN = New System.Windows.Forms.ComboBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.txtGrossWeight_Wet = New System.Windows.Forms.TextBox
        Me.Label39 = New System.Windows.Forms.Label
        Me.CmbTableCode = New System.Windows.Forms.ComboBox
        Me.Label28 = New System.Windows.Forms.Label
        Me.txtNetWeight_Wet = New System.Windows.Forms.TextBox
        Me.cmbValueAdded = New System.Windows.Forms.ComboBox
        Me.Label27 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.cmbBulkLot = New System.Windows.Forms.ComboBox
        Me.Label20 = New System.Windows.Forms.Label
        Me.cmbMultipleTag = New System.Windows.Forms.ComboBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.txtStonePieces_Num = New System.Windows.Forms.TextBox
        Me.txtStoneWeight_Wet = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.cmbCostCentre_Man = New System.Windows.Forms.ComboBox
        Me.pnlGrid = New System.Windows.Forms.Panel
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.dtpTo = New GiritechPack.DatePicker(Me.components)
        Me.dtpFrom = New GiritechPack.DatePicker(Me.components)
        Me.Label29 = New System.Windows.Forms.Label
        Me.gridViewOpen = New System.Windows.Forms.DataGridView
        Me.Label34 = New System.Windows.Forms.Label
        Me.btnSearch = New System.Windows.Forms.Button
        Me.Label33 = New System.Windows.Forms.Label
        Me.txtOpenLotNo = New System.Windows.Forms.TextBox
        Me.Label32 = New System.Windows.Forms.Label
        Me.txtOpenItemName = New System.Windows.Forms.TextBox
        Me.cmbOpenDesignerName = New System.Windows.Forms.ComboBox
        Me.Label31 = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.pnlNonTag = New System.Windows.Forms.Panel
        Me.rbtPartsales = New System.Windows.Forms.RadioButton
        Me.rbtSaleReturn = New System.Windows.Forms.RadioButton
        Me.rbtPurchase = New System.Windows.Forms.RadioButton
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label21 = New System.Windows.Forms.Label
        Me.TextBox10 = New System.Windows.Forms.TextBox
        Me.txtNonTagitemid = New System.Windows.Forms.TextBox
        Me.cmbMetalName = New System.Windows.Forms.ComboBox
        Me.lblMetalName = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.txtStnwt_WET = New System.Windows.Forms.TextBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.txtMeltwt_WET = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtCtrStkwt_WET = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtTotalwt_WET = New System.Windows.Forms.TextBox
        Me.gridViewTotal = New System.Windows.Forms.DataGridView
        Me.dtpDate = New GiritechPack.DatePicker(Me.components)
        Me.btnAdd = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.tabView = New System.Windows.Forms.TabPage
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpStone.SuspendLayout()
        Me.pnlGrid.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.gridViewOpen, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.pnlNonTag.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.gridViewTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabView.SuspendLayout()
        Me.SuspendLayout()
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(16, 332)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(930, 218)
        Me.gridView.TabIndex = 49
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(209, 247)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(81, 30)
        Me.btnNew.TabIndex = 47
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(296, 247)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(74, 30)
        Me.btnExit.TabIndex = 48
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'grpStone
        '
        Me.grpStone.Controls.Add(Me.Label15)
        Me.grpStone.Controls.Add(Me.txtCtrstkbalwt)
        Me.grpStone.Controls.Add(Me.Label14)
        Me.grpStone.Controls.Add(Me.txtCtrstkAddwt)
        Me.grpStone.Controls.Add(Me.Label7)
        Me.grpStone.Controls.Add(Me.txtCtrstkTotwt)
        Me.grpStone.Controls.Add(Me.Label4)
        Me.grpStone.Controls.Add(Me.Label5)
        Me.grpStone.Controls.Add(Me.TextBox3)
        Me.grpStone.Controls.Add(Me.TextBox4)
        Me.grpStone.Controls.Add(Me.Label10)
        Me.grpStone.Controls.Add(Me.cmbSubItemName_Man)
        Me.grpStone.Controls.Add(Me.Label8)
        Me.grpStone.Controls.Add(Me.Label9)
        Me.grpStone.Controls.Add(Me.txtPiece_Num_Man)
        Me.grpStone.Controls.Add(Me.Label11)
        Me.grpStone.Controls.Add(Me.txtItemName)
        Me.grpStone.Controls.Add(Me.txtNoOfTags_Num)
        Me.grpStone.Controls.Add(Me.txtItemCode_Num_Man)
        Me.grpStone.Controls.Add(Me.Label30)
        Me.grpStone.Controls.Add(Me.cmbItemCounter_MAN)
        Me.grpStone.Controls.Add(Me.Label18)
        Me.grpStone.Controls.Add(Me.txtGrossWeight_Wet)
        Me.grpStone.Controls.Add(Me.Label39)
        Me.grpStone.Controls.Add(Me.CmbTableCode)
        Me.grpStone.Controls.Add(Me.Label28)
        Me.grpStone.Controls.Add(Me.txtNetWeight_Wet)
        Me.grpStone.Controls.Add(Me.cmbValueAdded)
        Me.grpStone.Controls.Add(Me.Label27)
        Me.grpStone.Controls.Add(Me.Label19)
        Me.grpStone.Controls.Add(Me.cmbBulkLot)
        Me.grpStone.Controls.Add(Me.Label20)
        Me.grpStone.Controls.Add(Me.cmbMultipleTag)
        Me.grpStone.Controls.Add(Me.Label13)
        Me.grpStone.Controls.Add(Me.Label12)
        Me.grpStone.Controls.Add(Me.txtStonePieces_Num)
        Me.grpStone.Controls.Add(Me.txtStoneWeight_Wet)
        Me.grpStone.Location = New System.Drawing.Point(398, 2)
        Me.grpStone.Name = "grpStone"
        Me.grpStone.Size = New System.Drawing.Size(363, 324)
        Me.grpStone.TabIndex = 25
        Me.grpStone.TabStop = False
        Me.grpStone.Visible = False
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(275, 19)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(71, 13)
        Me.Label15.TabIndex = 68
        Me.Label15.Text = "Balance Wt"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtCtrstkbalwt
        '
        Me.txtCtrstkbalwt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCtrstkbalwt.Location = New System.Drawing.Point(278, 32)
        Me.txtCtrstkbalwt.MaxLength = 13
        Me.txtCtrstkbalwt.Name = "txtCtrstkbalwt"
        Me.txtCtrstkbalwt.Size = New System.Drawing.Size(75, 21)
        Me.txtCtrstkbalwt.TabIndex = 69
        Me.txtCtrstkbalwt.Text = "123456789"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(179, 19)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(48, 13)
        Me.Label14.TabIndex = 66
        Me.Label14.Text = "Add Wt"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtCtrstkAddwt
        '
        Me.txtCtrstkAddwt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCtrstkAddwt.Location = New System.Drawing.Point(182, 32)
        Me.txtCtrstkAddwt.MaxLength = 13
        Me.txtCtrstkAddwt.Name = "txtCtrstkAddwt"
        Me.txtCtrstkAddwt.Size = New System.Drawing.Size(75, 21)
        Me.txtCtrstkAddwt.TabIndex = 67
        Me.txtCtrstkAddwt.Text = "123456789"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(89, 19)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(54, 13)
        Me.Label7.TabIndex = 64
        Me.Label7.Text = "Total Wt"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtCtrstkTotwt
        '
        Me.txtCtrstkTotwt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCtrstkTotwt.Location = New System.Drawing.Point(87, 32)
        Me.txtCtrstkTotwt.MaxLength = 13
        Me.txtCtrstkTotwt.Name = "txtCtrstkTotwt"
        Me.txtCtrstkTotwt.Size = New System.Drawing.Size(75, 21)
        Me.txtCtrstkTotwt.TabIndex = 65
        Me.txtCtrstkTotwt.Text = "123456789"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(31, 221)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 13)
        Me.Label4.TabIndex = 60
        Me.Label4.Text = "Dia. Pcs"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(186, 221)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(73, 13)
        Me.Label5.TabIndex = 62
        Me.Label5.Text = "Dia. Weight"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextBox3
        '
        Me.TextBox3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBox3.Location = New System.Drawing.Point(91, 219)
        Me.TextBox3.MaxLength = 8
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(83, 21)
        Me.TextBox3.TabIndex = 61
        Me.TextBox3.Text = "123456789"
        '
        'TextBox4
        '
        Me.TextBox4.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBox4.Location = New System.Drawing.Point(272, 220)
        Me.TextBox4.MaxLength = 13
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(80, 21)
        Me.TextBox4.TabIndex = 63
        Me.TextBox4.Text = "12345678.00"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(29, 117)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(55, 13)
        Me.Label10.TabIndex = 56
        Me.Label10.Text = "Piece(S)"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbSubItemName_Man
        '
        Me.cmbSubItemName_Man.FormattingEnabled = True
        Me.cmbSubItemName_Man.Location = New System.Drawing.Point(92, 86)
        Me.cmbSubItemName_Man.Name = "cmbSubItemName_Man"
        Me.cmbSubItemName_Man.Size = New System.Drawing.Size(260, 21)
        Me.cmbSubItemName_Man.TabIndex = 55
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(8, 62)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(68, 13)
        Me.Label8.TabIndex = 51
        Me.Label8.Text = "Item Code"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(2, 89)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(93, 13)
        Me.Label9.TabIndex = 54
        Me.Label9.Text = "SubItem Name"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtPiece_Num_Man
        '
        Me.txtPiece_Num_Man.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtPiece_Num_Man.Location = New System.Drawing.Point(92, 112)
        Me.txtPiece_Num_Man.MaxLength = 9
        Me.txtPiece_Num_Man.Name = "txtPiece_Num_Man"
        Me.txtPiece_Num_Man.Size = New System.Drawing.Size(72, 21)
        Me.txtPiece_Num_Man.TabIndex = 57
        Me.txtPiece_Num_Man.Text = "123456789"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(176, 115)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(70, 13)
        Me.Label11.TabIndex = 58
        Me.Label11.Text = "No Of Tags"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtItemName
        '
        Me.txtItemName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtItemName.Enabled = False
        Me.txtItemName.Location = New System.Drawing.Point(149, 59)
        Me.txtItemName.MaxLength = 50
        Me.txtItemName.Name = "txtItemName"
        Me.txtItemName.Size = New System.Drawing.Size(204, 21)
        Me.txtItemName.TabIndex = 53
        '
        'txtNoOfTags_Num
        '
        Me.txtNoOfTags_Num.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNoOfTags_Num.Location = New System.Drawing.Point(252, 112)
        Me.txtNoOfTags_Num.MaxLength = 9
        Me.txtNoOfTags_Num.Name = "txtNoOfTags_Num"
        Me.txtNoOfTags_Num.Size = New System.Drawing.Size(78, 21)
        Me.txtNoOfTags_Num.TabIndex = 59
        Me.txtNoOfTags_Num.Text = "123456789"
        '
        'txtItemCode_Num_Man
        '
        Me.txtItemCode_Num_Man.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtItemCode_Num_Man.Location = New System.Drawing.Point(92, 59)
        Me.txtItemCode_Num_Man.MaxLength = 8
        Me.txtItemCode_Num_Man.Name = "txtItemCode_Num_Man"
        Me.txtItemCode_Num_Man.Size = New System.Drawing.Size(53, 21)
        Me.txtItemCode_Num_Man.TabIndex = 52
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Location = New System.Drawing.Point(5, 300)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(84, 13)
        Me.Label30.TabIndex = 49
        Me.Label30.Text = "Item Counter"
        Me.Label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbItemCounter_MAN
        '
        Me.cmbItemCounter_MAN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbItemCounter_MAN.FormattingEnabled = True
        Me.cmbItemCounter_MAN.Location = New System.Drawing.Point(90, 295)
        Me.cmbItemCounter_MAN.Name = "cmbItemCounter_MAN"
        Me.cmbItemCounter_MAN.Size = New System.Drawing.Size(263, 21)
        Me.cmbItemCounter_MAN.TabIndex = 50
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(6, 147)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(83, 13)
        Me.Label18.TabIndex = 47
        Me.Label18.Text = "Gross Weight"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtGrossWeight_Wet
        '
        Me.txtGrossWeight_Wet.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtGrossWeight_Wet.Location = New System.Drawing.Point(92, 139)
        Me.txtGrossWeight_Wet.MaxLength = 13
        Me.txtGrossWeight_Wet.Name = "txtGrossWeight_Wet"
        Me.txtGrossWeight_Wet.Size = New System.Drawing.Size(121, 21)
        Me.txtGrossWeight_Wet.TabIndex = 48
        Me.txtGrossWeight_Wet.Text = "123456789"
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.Location = New System.Drawing.Point(191, 274)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(72, 13)
        Me.Label39.TabIndex = 45
        Me.Label39.Text = "Table Code"
        Me.Label39.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CmbTableCode
        '
        Me.CmbTableCode.FormattingEnabled = True
        Me.CmbTableCode.Location = New System.Drawing.Point(271, 271)
        Me.CmbTableCode.Name = "CmbTableCode"
        Me.CmbTableCode.Size = New System.Drawing.Size(83, 21)
        Me.CmbTableCode.TabIndex = 46
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Location = New System.Drawing.Point(5, 276)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(79, 13)
        Me.Label28.TabIndex = 43
        Me.Label28.Text = "Value Added"
        Me.Label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtNetWeight_Wet
        '
        Me.txtNetWeight_Wet.Location = New System.Drawing.Point(92, 166)
        Me.txtNetWeight_Wet.MaxLength = 13
        Me.txtNetWeight_Wet.Name = "txtNetWeight_Wet"
        Me.txtNetWeight_Wet.Size = New System.Drawing.Size(121, 21)
        Me.txtNetWeight_Wet.TabIndex = 42
        Me.txtNetWeight_Wet.Text = "12345678.00"
        '
        'cmbValueAdded
        '
        Me.cmbValueAdded.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbValueAdded.FormattingEnabled = True
        Me.cmbValueAdded.Location = New System.Drawing.Point(90, 271)
        Me.cmbValueAdded.Name = "cmbValueAdded"
        Me.cmbValueAdded.Size = New System.Drawing.Size(86, 21)
        Me.cmbValueAdded.TabIndex = 44
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Location = New System.Drawing.Point(20, 169)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(69, 13)
        Me.Label27.TabIndex = 41
        Me.Label27.Text = "Net Weight"
        Me.Label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(188, 248)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(75, 13)
        Me.Label19.TabIndex = 39
        Me.Label19.Text = "Multiple Tag"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbBulkLot
        '
        Me.cmbBulkLot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBulkLot.FormattingEnabled = True
        Me.cmbBulkLot.Location = New System.Drawing.Point(90, 245)
        Me.cmbBulkLot.Name = "cmbBulkLot"
        Me.cmbBulkLot.Size = New System.Drawing.Size(84, 21)
        Me.cmbBulkLot.TabIndex = 38
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(31, 248)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(53, 13)
        Me.Label20.TabIndex = 37
        Me.Label20.Text = "Bulk Lot"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbMultipleTag
        '
        Me.cmbMultipleTag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMultipleTag.FormattingEnabled = True
        Me.cmbMultipleTag.Location = New System.Drawing.Point(271, 244)
        Me.cmbMultipleTag.Name = "cmbMultipleTag"
        Me.cmbMultipleTag.Size = New System.Drawing.Size(84, 21)
        Me.cmbMultipleTag.TabIndex = 40
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(21, 196)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(63, 13)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "Stone Pcs"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(176, 196)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(83, 13)
        Me.Label12.TabIndex = 2
        Me.Label12.Text = "Stone Weight"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtStonePieces_Num
        '
        Me.txtStonePieces_Num.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtStonePieces_Num.Location = New System.Drawing.Point(92, 193)
        Me.txtStonePieces_Num.MaxLength = 8
        Me.txtStonePieces_Num.Name = "txtStonePieces_Num"
        Me.txtStonePieces_Num.Size = New System.Drawing.Size(82, 21)
        Me.txtStonePieces_Num.TabIndex = 1
        Me.txtStonePieces_Num.Text = "123456789"
        '
        'txtStoneWeight_Wet
        '
        Me.txtStoneWeight_Wet.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtStoneWeight_Wet.Location = New System.Drawing.Point(273, 194)
        Me.txtStoneWeight_Wet.MaxLength = 13
        Me.txtStoneWeight_Wet.Name = "txtStoneWeight_Wet"
        Me.txtStoneWeight_Wet.Size = New System.Drawing.Size(80, 21)
        Me.txtStoneWeight_Wet.TabIndex = 3
        Me.txtStoneWeight_Wet.Text = "12345678.00"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(263, 21)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Date"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(38, 52)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(76, 13)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "Cost Centre"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbCostCentre_Man
        '
        Me.cmbCostCentre_Man.FormattingEnabled = True
        Me.cmbCostCentre_Man.Location = New System.Drawing.Point(123, 49)
        Me.cmbCostCentre_Man.Name = "cmbCostCentre_Man"
        Me.cmbCostCentre_Man.Size = New System.Drawing.Size(269, 21)
        Me.cmbCostCentre_Man.TabIndex = 11
        '
        'pnlGrid
        '
        Me.pnlGrid.Controls.Add(Me.GroupBox2)
        Me.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlGrid.Location = New System.Drawing.Point(3, 3)
        Me.pnlGrid.Name = "pnlGrid"
        Me.pnlGrid.Size = New System.Drawing.Size(1008, 621)
        Me.pnlGrid.TabIndex = 1
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.dtpTo)
        Me.GroupBox2.Controls.Add(Me.dtpFrom)
        Me.GroupBox2.Controls.Add(Me.Label29)
        Me.GroupBox2.Controls.Add(Me.gridViewOpen)
        Me.GroupBox2.Controls.Add(Me.Label34)
        Me.GroupBox2.Controls.Add(Me.btnSearch)
        Me.GroupBox2.Controls.Add(Me.Label33)
        Me.GroupBox2.Controls.Add(Me.txtOpenLotNo)
        Me.GroupBox2.Controls.Add(Me.Label32)
        Me.GroupBox2.Controls.Add(Me.txtOpenItemName)
        Me.GroupBox2.Controls.Add(Me.cmbOpenDesignerName)
        Me.GroupBox2.Controls.Add(Me.Label31)
        Me.GroupBox2.Location = New System.Drawing.Point(17, 14)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(973, 589)
        Me.GroupBox2.TabIndex = 9
        Me.GroupBox2.TabStop = False
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(240, 82)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(91, 21)
        Me.dtpTo.TabIndex = 11
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(116, 82)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(91, 21)
        Me.dtpFrom.TabIndex = 7
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Location = New System.Drawing.Point(11, 23)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(39, 13)
        Me.Label29.TabIndex = 0
        Me.Label29.Text = "&LotNo"
        Me.Label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gridViewOpen
        '
        Me.gridViewOpen.AllowUserToAddRows = False
        Me.gridViewOpen.AllowUserToDeleteRows = False
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.gridViewOpen.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle2
        Me.gridViewOpen.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridViewOpen.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewOpen.Location = New System.Drawing.Point(14, 108)
        Me.gridViewOpen.MultiSelect = False
        Me.gridViewOpen.Name = "gridViewOpen"
        Me.gridViewOpen.ReadOnly = True
        Me.gridViewOpen.RowHeadersVisible = False
        Me.gridViewOpen.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridViewOpen.Size = New System.Drawing.Size(948, 472)
        Me.gridViewOpen.TabIndex = 1
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.Location = New System.Drawing.Point(213, 85)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(21, 13)
        Me.Label34.TabIndex = 8
        Me.Label34.Text = "To"
        Me.Label34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(351, 72)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 10
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Location = New System.Drawing.Point(11, 86)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(67, 13)
        Me.Label33.TabIndex = 6
        Me.Label33.Text = "Date From"
        Me.Label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtOpenLotNo
        '
        Me.txtOpenLotNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtOpenLotNo.Location = New System.Drawing.Point(117, 18)
        Me.txtOpenLotNo.Name = "txtOpenLotNo"
        Me.txtOpenLotNo.Size = New System.Drawing.Size(88, 21)
        Me.txtOpenLotNo.TabIndex = 1
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Location = New System.Drawing.Point(11, 55)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(71, 13)
        Me.Label32.TabIndex = 4
        Me.Label32.Text = "Item Name"
        Me.Label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtOpenItemName
        '
        Me.txtOpenItemName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtOpenItemName.Location = New System.Drawing.Point(116, 50)
        Me.txtOpenItemName.Name = "txtOpenItemName"
        Me.txtOpenItemName.Size = New System.Drawing.Size(156, 21)
        Me.txtOpenItemName.TabIndex = 5
        '
        'cmbOpenDesignerName
        '
        Me.cmbOpenDesignerName.FormattingEnabled = True
        Me.cmbOpenDesignerName.Location = New System.Drawing.Point(318, 16)
        Me.cmbOpenDesignerName.Name = "cmbOpenDesignerName"
        Me.cmbOpenDesignerName.Size = New System.Drawing.Size(219, 21)
        Me.cmbOpenDesignerName.TabIndex = 3
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Location = New System.Drawing.Point(213, 21)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(95, 13)
        Me.Label31.TabIndex = 2
        Me.Label31.Text = "Designer Name"
        Me.Label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
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
        Me.tabMain.Size = New System.Drawing.Size(1022, 640)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.pnlNonTag)
        Me.tabGeneral.Controls.Add(Me.GroupBox1)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 9)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(1014, 627)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'pnlNonTag
        '
        Me.pnlNonTag.Controls.Add(Me.rbtPartsales)
        Me.pnlNonTag.Controls.Add(Me.rbtSaleReturn)
        Me.pnlNonTag.Controls.Add(Me.rbtPurchase)
        Me.pnlNonTag.Enabled = False
        Me.pnlNonTag.Location = New System.Drawing.Point(33, 6)
        Me.pnlNonTag.Name = "pnlNonTag"
        Me.pnlNonTag.Size = New System.Drawing.Size(957, 30)
        Me.pnlNonTag.TabIndex = 48
        '
        'rbtPartsales
        '
        Me.rbtPartsales.AutoSize = True
        Me.rbtPartsales.Location = New System.Drawing.Point(691, 3)
        Me.rbtPartsales.Name = "rbtPartsales"
        Me.rbtPartsales.Size = New System.Drawing.Size(93, 17)
        Me.rbtPartsales.TabIndex = 2
        Me.rbtPartsales.TabStop = True
        Me.rbtPartsales.Text = "Partly Sales"
        Me.rbtPartsales.UseVisualStyleBackColor = True
        '
        'rbtSaleReturn
        '
        Me.rbtSaleReturn.AutoSize = True
        Me.rbtSaleReturn.Location = New System.Drawing.Point(319, 3)
        Me.rbtSaleReturn.Name = "rbtSaleReturn"
        Me.rbtSaleReturn.Size = New System.Drawing.Size(98, 17)
        Me.rbtSaleReturn.TabIndex = 1
        Me.rbtSaleReturn.TabStop = True
        Me.rbtSaleReturn.Text = "Sales Return"
        Me.rbtSaleReturn.UseVisualStyleBackColor = True
        '
        'rbtPurchase
        '
        Me.rbtPurchase.AutoSize = True
        Me.rbtPurchase.Location = New System.Drawing.Point(20, 3)
        Me.rbtPurchase.Name = "rbtPurchase"
        Me.rbtPurchase.Size = New System.Drawing.Size(81, 17)
        Me.rbtPurchase.TabIndex = 0
        Me.rbtPurchase.TabStop = True
        Me.rbtPurchase.Text = "Purchase "
        Me.rbtPurchase.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label21)
        Me.GroupBox1.Controls.Add(Me.TextBox10)
        Me.GroupBox1.Controls.Add(Me.txtNonTagitemid)
        Me.GroupBox1.Controls.Add(Me.cmbMetalName)
        Me.GroupBox1.Controls.Add(Me.lblMetalName)
        Me.GroupBox1.Controls.Add(Me.Label17)
        Me.GroupBox1.Controls.Add(Me.txtStnwt_WET)
        Me.GroupBox1.Controls.Add(Me.Label16)
        Me.GroupBox1.Controls.Add(Me.txtMeltwt_WET)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.txtCtrStkwt_WET)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.txtTotalwt_WET)
        Me.GroupBox1.Controls.Add(Me.gridViewTotal)
        Me.GroupBox1.Controls.Add(Me.dtpDate)
        Me.GroupBox1.Controls.Add(Me.btnAdd)
        Me.GroupBox1.Controls.Add(Me.btnSave)
        Me.GroupBox1.Controls.Add(Me.gridView)
        Me.GroupBox1.Controls.Add(Me.btnNew)
        Me.GroupBox1.Controls.Add(Me.cmbCostCentre_Man)
        Me.GroupBox1.Controls.Add(Me.btnExit)
        Me.GroupBox1.Controls.Add(Me.grpStone)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Location = New System.Drawing.Point(17, 30)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(973, 585)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(46, 85)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(40, 13)
        Me.Label21.TabIndex = 63
        Me.Label21.Text = "Items"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextBox10
        '
        Me.TextBox10.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBox10.Enabled = False
        Me.TextBox10.Location = New System.Drawing.Point(188, 82)
        Me.TextBox10.MaxLength = 50
        Me.TextBox10.Name = "TextBox10"
        Me.TextBox10.Size = New System.Drawing.Size(204, 21)
        Me.TextBox10.TabIndex = 62
        '
        'txtNonTagitemid
        '
        Me.txtNonTagitemid.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNonTagitemid.Location = New System.Drawing.Point(123, 82)
        Me.txtNonTagitemid.MaxLength = 8
        Me.txtNonTagitemid.Name = "txtNonTagitemid"
        Me.txtNonTagitemid.Size = New System.Drawing.Size(53, 21)
        Me.txtNonTagitemid.TabIndex = 61
        '
        'cmbMetalName
        '
        Me.cmbMetalName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbMetalName.FormattingEnabled = True
        Me.cmbMetalName.Location = New System.Drawing.Point(122, 17)
        Me.cmbMetalName.Name = "cmbMetalName"
        Me.cmbMetalName.Size = New System.Drawing.Size(135, 21)
        Me.cmbMetalName.TabIndex = 60
        '
        'lblMetalName
        '
        Me.lblMetalName.AutoSize = True
        Me.lblMetalName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMetalName.Location = New System.Drawing.Point(36, 25)
        Me.lblMetalName.Name = "lblMetalName"
        Me.lblMetalName.Size = New System.Drawing.Size(78, 13)
        Me.lblMetalName.TabIndex = 59
        Me.lblMetalName.Text = "Metal Name "
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(26, 217)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(91, 13)
        Me.Label17.TabIndex = 57
        Me.Label17.Text = "To Less Stone "
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtStnwt_WET
        '
        Me.txtStnwt_WET.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtStnwt_WET.Location = New System.Drawing.Point(122, 214)
        Me.txtStnwt_WET.MaxLength = 13
        Me.txtStnwt_WET.Name = "txtStnwt_WET"
        Me.txtStnwt_WET.Size = New System.Drawing.Size(121, 21)
        Me.txtStnwt_WET.TabIndex = 58
        Me.txtStnwt_WET.Text = "123456789"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(4, 184)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(113, 13)
        Me.Label16.TabIndex = 55
        Me.Label16.Text = "To Melting Process"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtMeltwt_WET
        '
        Me.txtMeltwt_WET.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMeltwt_WET.Location = New System.Drawing.Point(122, 181)
        Me.txtMeltwt_WET.MaxLength = 13
        Me.txtMeltwt_WET.Name = "txtMeltwt_WET"
        Me.txtMeltwt_WET.Size = New System.Drawing.Size(121, 21)
        Me.txtMeltwt_WET.TabIndex = 56
        Me.txtMeltwt_WET.Text = "123456789"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(7, 149)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(107, 13)
        Me.Label3.TabIndex = 53
        Me.Label3.Text = "To Counter Stock"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtCtrStkwt_WET
        '
        Me.txtCtrStkwt_WET.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCtrStkwt_WET.Location = New System.Drawing.Point(122, 146)
        Me.txtCtrStkwt_WET.MaxLength = 13
        Me.txtCtrStkwt_WET.Name = "txtCtrStkwt_WET"
        Me.txtCtrStkwt_WET.Size = New System.Drawing.Size(121, 21)
        Me.txtCtrStkwt_WET.TabIndex = 54
        Me.txtCtrStkwt_WET.Text = "123456789"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(36, 115)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(78, 13)
        Me.Label1.TabIndex = 51
        Me.Label1.Text = "Total Weight"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtTotalwt_WET
        '
        Me.txtTotalwt_WET.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTotalwt_WET.Location = New System.Drawing.Point(123, 112)
        Me.txtTotalwt_WET.MaxLength = 13
        Me.txtTotalwt_WET.Name = "txtTotalwt_WET"
        Me.txtTotalwt_WET.Size = New System.Drawing.Size(121, 21)
        Me.txtTotalwt_WET.TabIndex = 52
        Me.txtTotalwt_WET.Text = "123456789"
        '
        'gridViewTotal
        '
        Me.gridViewTotal.AllowUserToAddRows = False
        Me.gridViewTotal.AllowUserToDeleteRows = False
        Me.gridViewTotal.BackgroundColor = System.Drawing.Color.WhiteSmoke
        Me.gridViewTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewTotal.Location = New System.Drawing.Point(14, 556)
        Me.gridViewTotal.Name = "gridViewTotal"
        Me.gridViewTotal.RowHeadersVisible = False
        Me.gridViewTotal.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.gridViewTotal.Size = New System.Drawing.Size(932, 23)
        Me.gridViewTotal.TabIndex = 48
        '
        'dtpDate
        '
        Me.dtpDate.Location = New System.Drawing.Point(296, 18)
        Me.dtpDate.Mask = "##/##/####"
        Me.dtpDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpDate.Size = New System.Drawing.Size(96, 21)
        Me.dtpDate.TabIndex = 3
        Me.dtpDate.Text = "07/03/9998"
        Me.dtpDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(29, 247)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(85, 30)
        Me.btnAdd.TabIndex = 44
        Me.btnAdd.Text = "&Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.ImageKey = "(none)"
        Me.btnSave.Location = New System.Drawing.Point(120, 247)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(83, 30)
        Me.btnSave.TabIndex = 45
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.pnlGrid)
        Me.tabView.Location = New System.Drawing.Point(4, 9)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1014, 627)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'frmOGSRPS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1022, 640)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmOGSRPS"
        Me.Text = "Lot Entry"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpStone.ResumeLayout(False)
        Me.grpStone.PerformLayout()
        Me.pnlGrid.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.gridViewOpen, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.pnlNonTag.ResumeLayout(False)
        Me.pnlNonTag.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.gridViewTotal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabView.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents cmbCostCentre_Man As System.Windows.Forms.ComboBox
    Friend WithEvents txtStoneWeight_Wet As System.Windows.Forms.TextBox
    Friend WithEvents txtStonePieces_Num As System.Windows.Forms.TextBox
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents grpStone As System.Windows.Forms.GroupBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlGrid As System.Windows.Forms.Panel
    Friend WithEvents gridViewOpen As System.Windows.Forms.DataGridView
    Friend WithEvents cmbOpenDesignerName As System.Windows.Forms.ComboBox
    Friend WithEvents txtOpenItemName As System.Windows.Forms.TextBox
    Friend WithEvents txtOpenLotNo As System.Windows.Forms.TextBox
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents dtpDate As GiritechPack.DatePicker
    Friend WithEvents dtpTo As GiritechPack.DatePicker
    Friend WithEvents dtpFrom As GiritechPack.DatePicker
    Friend WithEvents gridViewTotal As System.Windows.Forms.DataGridView
    Friend WithEvents pnlNonTag As System.Windows.Forms.Panel
    Friend WithEvents rbtPartsales As System.Windows.Forms.RadioButton
    Friend WithEvents rbtSaleReturn As System.Windows.Forms.RadioButton
    Friend WithEvents rbtPurchase As System.Windows.Forms.RadioButton
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents txtGrossWeight_Wet As System.Windows.Forms.TextBox
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents CmbTableCode As System.Windows.Forms.ComboBox
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents txtNetWeight_Wet As System.Windows.Forms.TextBox
    Friend WithEvents cmbValueAdded As System.Windows.Forms.ComboBox
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents cmbBulkLot As System.Windows.Forms.ComboBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents cmbMultipleTag As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtCtrStkwt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtTotalwt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents cmbSubItemName_Man As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtPiece_Num_Man As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtItemName As System.Windows.Forms.TextBox
    Friend WithEvents txtNoOfTags_Num As System.Windows.Forms.TextBox
    Friend WithEvents txtItemCode_Num_Man As System.Windows.Forms.TextBox
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents cmbItemCounter_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtCtrstkTotwt As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtCtrstkbalwt As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtCtrstkAddwt As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents txtMeltwt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents txtStnwt_WET As System.Windows.Forms.TextBox
    Friend WithEvents cmbMetalName As System.Windows.Forms.ComboBox
    Friend WithEvents lblMetalName As System.Windows.Forms.Label
    Friend WithEvents TextBox10 As System.Windows.Forms.TextBox
    Friend WithEvents txtNonTagitemid As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
End Class
