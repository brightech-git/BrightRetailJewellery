<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOpeningWeightEntry
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
        Me.grpMain = New System.Windows.Forms.GroupBox
        Me.Label20 = New System.Windows.Forms.Label
        Me.txtAppPureWeight_WET = New System.Windows.Forms.TextBox
        Me.txtAppNetWeight_WET = New System.Windows.Forms.TextBox
        Me.txtAppGrsWeight_WET = New System.Windows.Forms.TextBox
        Me.Label21 = New System.Windows.Forms.Label
        Me.Label22 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.pnlStone = New System.Windows.Forms.Panel
        Me.rbtCarat = New System.Windows.Forms.RadioButton
        Me.rbtGram = New System.Windows.Forms.RadioButton
        Me.Label17 = New System.Windows.Forms.Label
        Me.cmbCostCentre_MAN = New System.Windows.Forms.ComboBox
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.rbtPiece = New System.Windows.Forms.RadioButton
        Me.rbtWeight = New System.Windows.Forms.RadioButton
        Me.pnlIssueReceipt = New System.Windows.Forms.Panel
        Me.rbtIssue = New System.Windows.Forms.RadioButton
        Me.rbtReceipt = New System.Windows.Forms.RadioButton
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.rbtCompany = New System.Windows.Forms.RadioButton
        Me.rbtSmith = New System.Windows.Forms.RadioButton
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnNew = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.btnOpen = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.btnSave = New System.Windows.Forms.Button
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtRemark = New System.Windows.Forms.TextBox
        Me.txtAmount_AMT = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtRate_AMT = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtPureWeight_WET = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtNetWeight_WET = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.txtGrsWeight_WET = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.txtPcs_NUM = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.cmbCategory_MAN = New System.Windows.Forms.ComboBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.cmbSubItem_MAN = New System.Windows.Forms.ComboBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.cmbItem_MAN = New System.Windows.Forms.ComboBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.lblAmount = New System.Windows.Forms.Label
        Me.cmbMetal_MAN = New System.Windows.Forms.ComboBox
        Me.cmbSupplier_MAN = New System.Windows.Forms.ComboBox
        Me.tabView = New System.Windows.Forms.TabPage
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.btnExport = New System.Windows.Forms.Button
        Me.btnPrint = New System.Windows.Forms.Button
        Me.Label19 = New System.Windows.Forms.Label
        Me.cmbOpenCostCentre = New System.Windows.Forms.ComboBox
        Me.btnDelete = New System.Windows.Forms.Button
        Me.Label15 = New System.Windows.Forms.Label
        Me.rbtOpenCompany = New System.Windows.Forms.RadioButton
        Me.rbtOpenSmith = New System.Windows.Forms.RadioButton
        Me.btnSearch = New System.Windows.Forms.Button
        Me.cmbOpenMetal = New System.Windows.Forms.ComboBox
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.grpMain.SuspendLayout()
        Me.pnlStone.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.pnlIssueReceipt.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabMain
        '
        Me.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tabMain.Controls.Add(Me.tabGeneral)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1028, 606)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.grpMain)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 25)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(1020, 577)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'grpMain
        '
        Me.grpMain.Controls.Add(Me.Label20)
        Me.grpMain.Controls.Add(Me.txtAppPureWeight_WET)
        Me.grpMain.Controls.Add(Me.txtAppNetWeight_WET)
        Me.grpMain.Controls.Add(Me.txtAppGrsWeight_WET)
        Me.grpMain.Controls.Add(Me.Label21)
        Me.grpMain.Controls.Add(Me.Label22)
        Me.grpMain.Controls.Add(Me.Label18)
        Me.grpMain.Controls.Add(Me.pnlStone)
        Me.grpMain.Controls.Add(Me.cmbCostCentre_MAN)
        Me.grpMain.Controls.Add(Me.Panel5)
        Me.grpMain.Controls.Add(Me.pnlIssueReceipt)
        Me.grpMain.Controls.Add(Me.Panel3)
        Me.grpMain.Controls.Add(Me.Label1)
        Me.grpMain.Controls.Add(Me.btnExit)
        Me.grpMain.Controls.Add(Me.Label2)
        Me.grpMain.Controls.Add(Me.btnNew)
        Me.grpMain.Controls.Add(Me.Label3)
        Me.grpMain.Controls.Add(Me.btnOpen)
        Me.grpMain.Controls.Add(Me.Label4)
        Me.grpMain.Controls.Add(Me.btnSave)
        Me.grpMain.Controls.Add(Me.Label8)
        Me.grpMain.Controls.Add(Me.txtRemark)
        Me.grpMain.Controls.Add(Me.txtAmount_AMT)
        Me.grpMain.Controls.Add(Me.Label6)
        Me.grpMain.Controls.Add(Me.txtRate_AMT)
        Me.grpMain.Controls.Add(Me.Label9)
        Me.grpMain.Controls.Add(Me.txtPureWeight_WET)
        Me.grpMain.Controls.Add(Me.Label5)
        Me.grpMain.Controls.Add(Me.txtNetWeight_WET)
        Me.grpMain.Controls.Add(Me.Label12)
        Me.grpMain.Controls.Add(Me.txtGrsWeight_WET)
        Me.grpMain.Controls.Add(Me.Label10)
        Me.grpMain.Controls.Add(Me.txtPcs_NUM)
        Me.grpMain.Controls.Add(Me.Label13)
        Me.grpMain.Controls.Add(Me.cmbCategory_MAN)
        Me.grpMain.Controls.Add(Me.Label7)
        Me.grpMain.Controls.Add(Me.cmbSubItem_MAN)
        Me.grpMain.Controls.Add(Me.Label11)
        Me.grpMain.Controls.Add(Me.cmbItem_MAN)
        Me.grpMain.Controls.Add(Me.Label16)
        Me.grpMain.Controls.Add(Me.lblAmount)
        Me.grpMain.Controls.Add(Me.cmbMetal_MAN)
        Me.grpMain.Controls.Add(Me.cmbSupplier_MAN)
        Me.grpMain.Location = New System.Drawing.Point(243, 9)
        Me.grpMain.Name = "grpMain"
        Me.grpMain.Size = New System.Drawing.Size(575, 556)
        Me.grpMain.TabIndex = 0
        Me.grpMain.TabStop = False
        '
        'Label20
        '
        Me.Label20.Location = New System.Drawing.Point(213, 378)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(100, 21)
        Me.Label20.TabIndex = 28
        Me.Label20.Text = "Approval Net Wt"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtAppPureWeight_WET
        '
        Me.txtAppPureWeight_WET.Location = New System.Drawing.Point(127, 404)
        Me.txtAppPureWeight_WET.MaxLength = 10
        Me.txtAppPureWeight_WET.Name = "txtAppPureWeight_WET"
        Me.txtAppPureWeight_WET.Size = New System.Drawing.Size(80, 21)
        Me.txtAppPureWeight_WET.TabIndex = 31
        Me.txtAppPureWeight_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAppNetWeight_WET
        '
        Me.txtAppNetWeight_WET.Location = New System.Drawing.Point(319, 377)
        Me.txtAppNetWeight_WET.MaxLength = 10
        Me.txtAppNetWeight_WET.Name = "txtAppNetWeight_WET"
        Me.txtAppNetWeight_WET.Size = New System.Drawing.Size(80, 21)
        Me.txtAppNetWeight_WET.TabIndex = 29
        Me.txtAppNetWeight_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAppGrsWeight_WET
        '
        Me.txtAppGrsWeight_WET.Location = New System.Drawing.Point(127, 378)
        Me.txtAppGrsWeight_WET.MaxLength = 10
        Me.txtAppGrsWeight_WET.Name = "txtAppGrsWeight_WET"
        Me.txtAppGrsWeight_WET.Size = New System.Drawing.Size(80, 21)
        Me.txtAppGrsWeight_WET.TabIndex = 27
        Me.txtAppGrsWeight_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label21
        '
        Me.Label21.Location = New System.Drawing.Point(12, 377)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(109, 21)
        Me.Label21.TabIndex = 26
        Me.Label21.Text = "Approval Grs Wt"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label22
        '
        Me.Label22.Location = New System.Drawing.Point(9, 404)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(112, 21)
        Me.Label22.TabIndex = 30
        Me.Label22.Text = "Approval Pure Wt"
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label18
        '
        Me.Label18.Location = New System.Drawing.Point(21, 22)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(83, 21)
        Me.Label18.TabIndex = 0
        Me.Label18.Text = "Cost Centre"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnlStone
        '
        Me.pnlStone.Controls.Add(Me.rbtCarat)
        Me.pnlStone.Controls.Add(Me.rbtGram)
        Me.pnlStone.Controls.Add(Me.Label17)
        Me.pnlStone.Location = New System.Drawing.Point(294, 434)
        Me.pnlStone.Name = "pnlStone"
        Me.pnlStone.Size = New System.Drawing.Size(267, 27)
        Me.pnlStone.TabIndex = 34
        '
        'rbtCarat
        '
        Me.rbtCarat.AutoSize = True
        Me.rbtCarat.Location = New System.Drawing.Point(82, 4)
        Me.rbtCarat.Name = "rbtCarat"
        Me.rbtCarat.Size = New System.Drawing.Size(57, 17)
        Me.rbtCarat.TabIndex = 1
        Me.rbtCarat.TabStop = True
        Me.rbtCarat.Text = "Carat"
        Me.rbtCarat.UseVisualStyleBackColor = True
        '
        'rbtGram
        '
        Me.rbtGram.AutoSize = True
        Me.rbtGram.Location = New System.Drawing.Point(144, 4)
        Me.rbtGram.Name = "rbtGram"
        Me.rbtGram.Size = New System.Drawing.Size(57, 17)
        Me.rbtGram.TabIndex = 2
        Me.rbtGram.TabStop = True
        Me.rbtGram.Text = "Gram"
        Me.rbtGram.UseVisualStyleBackColor = True
        '
        'Label17
        '
        Me.Label17.Location = New System.Drawing.Point(3, 2)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(73, 21)
        Me.Label17.TabIndex = 0
        Me.Label17.Text = "Stone Unit"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbCostCentre_MAN
        '
        Me.cmbCostCentre_MAN.FormattingEnabled = True
        Me.cmbCostCentre_MAN.Location = New System.Drawing.Point(127, 22)
        Me.cmbCostCentre_MAN.Name = "cmbCostCentre_MAN"
        Me.cmbCostCentre_MAN.Size = New System.Drawing.Size(171, 21)
        Me.cmbCostCentre_MAN.TabIndex = 1
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.rbtPiece)
        Me.Panel5.Controls.Add(Me.rbtWeight)
        Me.Panel5.Location = New System.Drawing.Point(127, 434)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(161, 27)
        Me.Panel5.TabIndex = 33
        '
        'rbtPiece
        '
        Me.rbtPiece.AutoSize = True
        Me.rbtPiece.Location = New System.Drawing.Point(0, 4)
        Me.rbtPiece.Name = "rbtPiece"
        Me.rbtPiece.Size = New System.Drawing.Size(55, 17)
        Me.rbtPiece.TabIndex = 0
        Me.rbtPiece.TabStop = True
        Me.rbtPiece.Text = "Piece"
        Me.rbtPiece.UseVisualStyleBackColor = True
        '
        'rbtWeight
        '
        Me.rbtWeight.AutoSize = True
        Me.rbtWeight.Location = New System.Drawing.Point(62, 4)
        Me.rbtWeight.Name = "rbtWeight"
        Me.rbtWeight.Size = New System.Drawing.Size(64, 17)
        Me.rbtWeight.TabIndex = 1
        Me.rbtWeight.TabStop = True
        Me.rbtWeight.Text = "Weight"
        Me.rbtWeight.UseVisualStyleBackColor = True
        '
        'pnlIssueReceipt
        '
        Me.pnlIssueReceipt.Controls.Add(Me.rbtIssue)
        Me.pnlIssueReceipt.Controls.Add(Me.rbtReceipt)
        Me.pnlIssueReceipt.Location = New System.Drawing.Point(127, 116)
        Me.pnlIssueReceipt.Name = "pnlIssueReceipt"
        Me.pnlIssueReceipt.Size = New System.Drawing.Size(161, 27)
        Me.pnlIssueReceipt.TabIndex = 7
        '
        'rbtIssue
        '
        Me.rbtIssue.AutoSize = True
        Me.rbtIssue.Location = New System.Drawing.Point(3, 6)
        Me.rbtIssue.Name = "rbtIssue"
        Me.rbtIssue.Size = New System.Drawing.Size(56, 17)
        Me.rbtIssue.TabIndex = 0
        Me.rbtIssue.TabStop = True
        Me.rbtIssue.Text = "Issue"
        Me.rbtIssue.UseVisualStyleBackColor = True
        '
        'rbtReceipt
        '
        Me.rbtReceipt.AutoSize = True
        Me.rbtReceipt.Location = New System.Drawing.Point(65, 6)
        Me.rbtReceipt.Name = "rbtReceipt"
        Me.rbtReceipt.Size = New System.Drawing.Size(67, 17)
        Me.rbtReceipt.TabIndex = 1
        Me.rbtReceipt.TabStop = True
        Me.rbtReceipt.Text = "Receipt"
        Me.rbtReceipt.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.rbtCompany)
        Me.Panel3.Controls.Add(Me.rbtSmith)
        Me.Panel3.Location = New System.Drawing.Point(127, 52)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(259, 27)
        Me.Panel3.TabIndex = 3
        '
        'rbtCompany
        '
        Me.rbtCompany.AutoSize = True
        Me.rbtCompany.Location = New System.Drawing.Point(3, 7)
        Me.rbtCompany.Name = "rbtCompany"
        Me.rbtCompany.Size = New System.Drawing.Size(80, 17)
        Me.rbtCompany.TabIndex = 0
        Me.rbtCompany.TabStop = True
        Me.rbtCompany.Text = "Company"
        Me.rbtCompany.UseVisualStyleBackColor = True
        '
        'rbtSmith
        '
        Me.rbtSmith.AutoSize = True
        Me.rbtSmith.Location = New System.Drawing.Point(89, 7)
        Me.rbtSmith.Name = "rbtSmith"
        Me.rbtSmith.Size = New System.Drawing.Size(105, 17)
        Me.rbtSmith.TabIndex = 1
        Me.rbtSmith.TabStop = True
        Me.rbtSmith.Text = "Smith/ Dealer"
        Me.rbtSmith.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(21, 58)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 21)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Stock Type"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(445, 519)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 42
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(21, 89)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 21)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Smith/ Dealer"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(339, 519)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 41
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(21, 122)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 21)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Tran Type"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(233, 519)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 30)
        Me.btnOpen.TabIndex = 40
        Me.btnOpen.Text = "Open [F2]"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(21, 156)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(100, 21)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Metal"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(127, 519)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 39
        Me.btnSave.Text = "Save [F1]"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(21, 290)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(100, 21)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "Pcs"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtRemark
        '
        Me.txtRemark.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtRemark.Location = New System.Drawing.Point(127, 492)
        Me.txtRemark.MaxLength = 50
        Me.txtRemark.Name = "txtRemark"
        Me.txtRemark.Size = New System.Drawing.Size(428, 21)
        Me.txtRemark.TabIndex = 38
        '
        'txtAmount_AMT
        '
        Me.txtAmount_AMT.Location = New System.Drawing.Point(127, 466)
        Me.txtAmount_AMT.MaxLength = 17
        Me.txtAmount_AMT.Name = "txtAmount_AMT"
        Me.txtAmount_AMT.Size = New System.Drawing.Size(103, 21)
        Me.txtAmount_AMT.TabIndex = 36
        Me.txtAmount_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(21, 225)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(100, 21)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Item"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtRate_AMT
        '
        Me.txtRate_AMT.Location = New System.Drawing.Point(319, 349)
        Me.txtRate_AMT.MaxLength = 7
        Me.txtRate_AMT.Name = "txtRate_AMT"
        Me.txtRate_AMT.Size = New System.Drawing.Size(80, 21)
        Me.txtRate_AMT.TabIndex = 25
        Me.txtRate_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(213, 320)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(100, 21)
        Me.Label9.TabIndex = 20
        Me.Label9.Text = "Net Weight"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtPureWeight_WET
        '
        Me.txtPureWeight_WET.Location = New System.Drawing.Point(127, 351)
        Me.txtPureWeight_WET.MaxLength = 10
        Me.txtPureWeight_WET.Name = "txtPureWeight_WET"
        Me.txtPureWeight_WET.Size = New System.Drawing.Size(80, 21)
        Me.txtPureWeight_WET.TabIndex = 23
        Me.txtPureWeight_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(21, 190)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(100, 21)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Category"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtNetWeight_WET
        '
        Me.txtNetWeight_WET.Location = New System.Drawing.Point(319, 319)
        Me.txtNetWeight_WET.MaxLength = 10
        Me.txtNetWeight_WET.Name = "txtNetWeight_WET"
        Me.txtNetWeight_WET.Size = New System.Drawing.Size(80, 21)
        Me.txtNetWeight_WET.TabIndex = 21
        Me.txtNetWeight_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(21, 434)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(100, 21)
        Me.Label12.TabIndex = 32
        Me.Label12.Text = "Calc Mode"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtGrsWeight_WET
        '
        Me.txtGrsWeight_WET.Location = New System.Drawing.Point(127, 320)
        Me.txtGrsWeight_WET.MaxLength = 10
        Me.txtGrsWeight_WET.Name = "txtGrsWeight_WET"
        Me.txtGrsWeight_WET.Size = New System.Drawing.Size(80, 21)
        Me.txtGrsWeight_WET.TabIndex = 19
        Me.txtGrsWeight_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(21, 319)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(100, 21)
        Me.Label10.TabIndex = 18
        Me.Label10.Text = "Grs Weight"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtPcs_NUM
        '
        Me.txtPcs_NUM.Location = New System.Drawing.Point(127, 290)
        Me.txtPcs_NUM.MaxLength = 6
        Me.txtPcs_NUM.Name = "txtPcs_NUM"
        Me.txtPcs_NUM.Size = New System.Drawing.Size(80, 21)
        Me.txtPcs_NUM.TabIndex = 17
        Me.txtPcs_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(213, 350)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(100, 21)
        Me.Label13.TabIndex = 24
        Me.Label13.Text = "Rate"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbCategory_MAN
        '
        Me.cmbCategory_MAN.FormattingEnabled = True
        Me.cmbCategory_MAN.Location = New System.Drawing.Point(127, 190)
        Me.cmbCategory_MAN.Name = "cmbCategory_MAN"
        Me.cmbCategory_MAN.Size = New System.Drawing.Size(428, 21)
        Me.cmbCategory_MAN.TabIndex = 11
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(21, 259)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(100, 21)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "Sub Item"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbSubItem_MAN
        '
        Me.cmbSubItem_MAN.FormattingEnabled = True
        Me.cmbSubItem_MAN.Location = New System.Drawing.Point(127, 259)
        Me.cmbSubItem_MAN.Name = "cmbSubItem_MAN"
        Me.cmbSubItem_MAN.Size = New System.Drawing.Size(272, 21)
        Me.cmbSubItem_MAN.TabIndex = 15
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(21, 351)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(100, 21)
        Me.Label11.TabIndex = 22
        Me.Label11.Text = "Pure Wt"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbItem_MAN
        '
        Me.cmbItem_MAN.FormattingEnabled = True
        Me.cmbItem_MAN.Location = New System.Drawing.Point(127, 225)
        Me.cmbItem_MAN.Name = "cmbItem_MAN"
        Me.cmbItem_MAN.Size = New System.Drawing.Size(272, 21)
        Me.cmbItem_MAN.TabIndex = 13
        '
        'Label16
        '
        Me.Label16.Location = New System.Drawing.Point(21, 492)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(100, 21)
        Me.Label16.TabIndex = 37
        Me.Label16.Text = "Remark"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblAmount
        '
        Me.lblAmount.AutoSize = True
        Me.lblAmount.Location = New System.Drawing.Point(21, 468)
        Me.lblAmount.Name = "lblAmount"
        Me.lblAmount.Size = New System.Drawing.Size(51, 13)
        Me.lblAmount.TabIndex = 35
        Me.lblAmount.Text = "Amount"
        Me.lblAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbMetal_MAN
        '
        Me.cmbMetal_MAN.FormattingEnabled = True
        Me.cmbMetal_MAN.Location = New System.Drawing.Point(127, 156)
        Me.cmbMetal_MAN.Name = "cmbMetal_MAN"
        Me.cmbMetal_MAN.Size = New System.Drawing.Size(272, 21)
        Me.cmbMetal_MAN.TabIndex = 9
        '
        'cmbSupplier_MAN
        '
        Me.cmbSupplier_MAN.FormattingEnabled = True
        Me.cmbSupplier_MAN.Location = New System.Drawing.Point(127, 89)
        Me.cmbSupplier_MAN.Name = "cmbSupplier_MAN"
        Me.cmbSupplier_MAN.Size = New System.Drawing.Size(428, 21)
        Me.cmbSupplier_MAN.TabIndex = 5
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.Panel2)
        Me.tabView.Controls.Add(Me.Panel1)
        Me.tabView.Location = New System.Drawing.Point(4, 25)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1020, 577)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.gridView)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 78)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1014, 496)
        Me.Panel2.TabIndex = 7
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 0)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 20
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(1014, 496)
        Me.gridView.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.Label19)
        Me.Panel1.Controls.Add(Me.cmbOpenCostCentre)
        Me.Panel1.Controls.Add(Me.btnDelete)
        Me.Panel1.Controls.Add(Me.Label15)
        Me.Panel1.Controls.Add(Me.rbtOpenCompany)
        Me.Panel1.Controls.Add(Me.rbtOpenSmith)
        Me.Panel1.Controls.Add(Me.btnSearch)
        Me.Panel1.Controls.Add(Me.cmbOpenMetal)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1014, 75)
        Me.Panel1.TabIndex = 0
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(696, 15)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 12
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(802, 15)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 13
        Me.btnPrint.Text = "Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(5, 19)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(76, 13)
        Me.Label19.TabIndex = 0
        Me.Label19.Text = "Cost Centre"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbOpenCostCentre
        '
        Me.cmbOpenCostCentre.FormattingEnabled = True
        Me.cmbOpenCostCentre.Location = New System.Drawing.Point(87, 15)
        Me.cmbOpenCostCentre.Name = "cmbOpenCostCentre"
        Me.cmbOpenCostCentre.Size = New System.Drawing.Size(171, 21)
        Me.cmbOpenCostCentre.TabIndex = 1
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(908, 15)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 30)
        Me.btnDelete.TabIndex = 7
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(269, 19)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(37, 13)
        Me.Label15.TabIndex = 2
        Me.Label15.Text = "Metal"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'rbtOpenCompany
        '
        Me.rbtOpenCompany.AutoSize = True
        Me.rbtOpenCompany.Location = New System.Drawing.Point(312, 52)
        Me.rbtOpenCompany.Name = "rbtOpenCompany"
        Me.rbtOpenCompany.Size = New System.Drawing.Size(80, 17)
        Me.rbtOpenCompany.TabIndex = 4
        Me.rbtOpenCompany.TabStop = True
        Me.rbtOpenCompany.Text = "Company"
        Me.rbtOpenCompany.UseVisualStyleBackColor = True
        '
        'rbtOpenSmith
        '
        Me.rbtOpenSmith.AutoSize = True
        Me.rbtOpenSmith.Location = New System.Drawing.Point(462, 52)
        Me.rbtOpenSmith.Name = "rbtOpenSmith"
        Me.rbtOpenSmith.Size = New System.Drawing.Size(81, 17)
        Me.rbtOpenSmith.TabIndex = 5
        Me.rbtOpenSmith.TabStop = True
        Me.rbtOpenSmith.Text = "Customer"
        Me.rbtOpenSmith.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(590, 15)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 6
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'cmbOpenMetal
        '
        Me.cmbOpenMetal.FormattingEnabled = True
        Me.cmbOpenMetal.Location = New System.Drawing.Point(312, 15)
        Me.cmbOpenMetal.Name = "cmbOpenMetal"
        Me.cmbOpenMetal.Size = New System.Drawing.Size(272, 21)
        Me.cmbOpenMetal.TabIndex = 3
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
        'frmOpeningWeightEntry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 606)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmOpeningWeightEntry"
        Me.Text = "Opening Weight Entry"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.grpMain.ResumeLayout(False)
        Me.grpMain.PerformLayout()
        Me.pnlStone.ResumeLayout(False)
        Me.pnlStone.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.pnlIssueReceipt.ResumeLayout(False)
        Me.pnlIssueReceipt.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.tabView.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtRate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents cmbCategory_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSubItem_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents cmbItem_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents cmbMetal_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSupplier_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents rbtWeight As System.Windows.Forms.RadioButton
    Friend WithEvents rbtReceipt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtSmith As System.Windows.Forms.RadioButton
    Friend WithEvents rbtPiece As System.Windows.Forms.RadioButton
    Friend WithEvents rbtIssue As System.Windows.Forms.RadioButton
    Friend WithEvents rbtCompany As System.Windows.Forms.RadioButton
    Friend WithEvents lblAmount As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents grpMain As System.Windows.Forms.GroupBox
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents cmbOpenMetal As System.Windows.Forms.ComboBox
    Friend WithEvents rbtOpenSmith As System.Windows.Forms.RadioButton
    Friend WithEvents rbtOpenCompany As System.Windows.Forms.RadioButton
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents pnlIssueReceipt As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents txtRemark As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents pnlStone As System.Windows.Forms.Panel
    Friend WithEvents rbtCarat As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGram As System.Windows.Forms.RadioButton
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents cmbCostCentre_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents cmbOpenCostCentre As System.Windows.Forms.ComboBox
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtPureWeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtNetWeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtGrsWeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtPcs_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents txtAppPureWeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtAppNetWeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtAppGrsWeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
End Class
