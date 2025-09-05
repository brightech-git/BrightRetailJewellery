<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCategoryStockNew
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
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.grpMain = New System.Windows.Forms.GroupBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.rbtnParty = New System.Windows.Forms.RadioButton
        Me.rbtnTranno = New System.Windows.Forms.RadioButton
        Me.chkWithApproval = New System.Windows.Forms.CheckBox
        Me.btnSave = New System.Windows.Forms.Button
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.Label = New System.Windows.Forms.Label
        Me.chkCmbCompany = New BrighttechPack.CheckedComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.rbtCarat = New System.Windows.Forms.RadioButton
        Me.rbtGeneral = New System.Windows.Forms.RadioButton
        Me.rbtGram = New System.Windows.Forms.RadioButton
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.rbtPurewt = New System.Windows.Forms.RadioButton
        Me.rbtGrsWt = New System.Windows.Forms.RadioButton
        Me.rbtNetWt = New System.Windows.Forms.RadioButton
        Me.cmbMetalType = New System.Windows.Forms.ComboBox
        Me.cmbCatName = New System.Windows.Forms.ComboBox
        Me.cmbMetal = New System.Windows.Forms.ComboBox
        Me.chkWithValue = New System.Windows.Forms.CheckBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.Label6 = New System.Windows.Forms.Label
        Me.chkWithOtherIssue = New System.Windows.Forms.CheckBox
        Me.chkWithStuddedStone = New System.Windows.Forms.CheckBox
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.pnlView = New System.Windows.Forms.Panel
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AutoResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.pnlHeading = New System.Windows.Forms.Panel
        Me.lblTitle = New System.Windows.Forms.Label
        Me.ContextMenuStrip1.SuspendLayout()
        Me.grpMain.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pnlView.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.pnlHeading.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'grpMain
        '
        Me.grpMain.Controls.Add(Me.Label7)
        Me.grpMain.Controls.Add(Me.GroupBox1)
        Me.grpMain.Controls.Add(Me.chkWithApproval)
        Me.grpMain.Controls.Add(Me.btnSave)
        Me.grpMain.Controls.Add(Me.chkCmbCostCentre)
        Me.grpMain.Controls.Add(Me.Label)
        Me.grpMain.Controls.Add(Me.chkCmbCompany)
        Me.grpMain.Controls.Add(Me.Label9)
        Me.grpMain.Controls.Add(Me.dtpTo)
        Me.grpMain.Controls.Add(Me.dtpFrom)
        Me.grpMain.Controls.Add(Me.btnPrint)
        Me.grpMain.Controls.Add(Me.btnExport)
        Me.grpMain.Controls.Add(Me.Panel2)
        Me.grpMain.Controls.Add(Me.Panel1)
        Me.grpMain.Controls.Add(Me.cmbMetalType)
        Me.grpMain.Controls.Add(Me.cmbCatName)
        Me.grpMain.Controls.Add(Me.cmbMetal)
        Me.grpMain.Controls.Add(Me.chkWithValue)
        Me.grpMain.Controls.Add(Me.btnExit)
        Me.grpMain.Controls.Add(Me.btnNew)
        Me.grpMain.Controls.Add(Me.Label6)
        Me.grpMain.Controls.Add(Me.chkWithOtherIssue)
        Me.grpMain.Controls.Add(Me.chkWithStuddedStone)
        Me.grpMain.Controls.Add(Me.btnView_Search)
        Me.grpMain.Controls.Add(Me.Label4)
        Me.grpMain.Controls.Add(Me.Label3)
        Me.grpMain.Controls.Add(Me.Label8)
        Me.grpMain.Controls.Add(Me.Label2)
        Me.grpMain.Controls.Add(Me.Label5)
        Me.grpMain.Controls.Add(Me.Label1)
        Me.grpMain.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpMain.Location = New System.Drawing.Point(0, 0)
        Me.grpMain.Name = "grpMain"
        Me.grpMain.Size = New System.Drawing.Size(1028, 179)
        Me.grpMain.TabIndex = 0
        Me.grpMain.TabStop = False
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(355, 116)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(115, 21)
        Me.Label7.TabIndex = 22
        Me.Label7.Text = "Order By"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbtnParty)
        Me.GroupBox1.Controls.Add(Me.rbtnTranno)
        Me.GroupBox1.Location = New System.Drawing.Point(480, 107)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(222, 33)
        Me.GroupBox1.TabIndex = 23
        Me.GroupBox1.TabStop = False
        '
        'rbtnParty
        '
        Me.rbtnParty.AutoSize = True
        Me.rbtnParty.Location = New System.Drawing.Point(100, 13)
        Me.rbtnParty.Name = "rbtnParty"
        Me.rbtnParty.Size = New System.Drawing.Size(92, 17)
        Me.rbtnParty.TabIndex = 1
        Me.rbtnParty.TabStop = True
        Me.rbtnParty.Text = "Party Name"
        Me.rbtnParty.UseVisualStyleBackColor = True
        '
        'rbtnTranno
        '
        Me.rbtnTranno.AutoSize = True
        Me.rbtnTranno.Location = New System.Drawing.Point(6, 13)
        Me.rbtnTranno.Name = "rbtnTranno"
        Me.rbtnTranno.Size = New System.Drawing.Size(66, 17)
        Me.rbtnTranno.TabIndex = 0
        Me.rbtnTranno.TabStop = True
        Me.rbtnTranno.Text = "TranNo"
        Me.rbtnTranno.UseVisualStyleBackColor = True
        '
        'chkWithApproval
        '
        Me.chkWithApproval.AutoSize = True
        Me.chkWithApproval.Checked = True
        Me.chkWithApproval.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkWithApproval.Location = New System.Drawing.Point(818, 90)
        Me.chkWithApproval.Name = "chkWithApproval"
        Me.chkWithApproval.Size = New System.Drawing.Size(106, 17)
        Me.chkWithApproval.TabIndex = 21
        Me.chkWithApproval.Text = "With Approval"
        Me.chkWithApproval.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(651, 143)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 30)
        Me.btnSave.TabIndex = 29
        Me.btnSave.Text = "&Save"
        Me.btnSave.UseVisualStyleBackColor = True
        Me.btnSave.Visible = False
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(121, 61)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(222, 22)
        Me.chkCmbCostCentre.TabIndex = 7
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(6, 64)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(76, 13)
        Me.Label.TabIndex = 6
        Me.Label.Text = "Cost Centre"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbCompany
        '
        Me.chkCmbCompany.CheckOnClick = True
        Me.chkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCompany.DropDownHeight = 1
        Me.chkCmbCompany.FormattingEnabled = True
        Me.chkCmbCompany.IntegralHeight = False
        Me.chkCmbCompany.Location = New System.Drawing.Point(121, 35)
        Me.chkCmbCompany.Name = "chkCmbCompany"
        Me.chkCmbCompany.Size = New System.Drawing.Size(222, 22)
        Me.chkCmbCompany.TabIndex = 5
        Me.chkCmbCompany.ValueSeparator = ", "
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 39)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(62, 13)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "Company"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(255, 10)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(88, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(121, 10)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(88, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(545, 143)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 28
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(333, 143)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 26
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rbtCarat)
        Me.Panel2.Controls.Add(Me.rbtGeneral)
        Me.Panel2.Controls.Add(Me.rbtGram)
        Me.Panel2.Location = New System.Drawing.Point(480, 61)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(202, 22)
        Me.Panel2.TabIndex = 17
        '
        'rbtCarat
        '
        Me.rbtCarat.AutoSize = True
        Me.rbtCarat.Location = New System.Drawing.Point(142, 3)
        Me.rbtCarat.Name = "rbtCarat"
        Me.rbtCarat.Size = New System.Drawing.Size(57, 17)
        Me.rbtCarat.TabIndex = 2
        Me.rbtCarat.TabStop = True
        Me.rbtCarat.Text = "Carat"
        Me.rbtCarat.UseVisualStyleBackColor = True
        '
        'rbtGeneral
        '
        Me.rbtGeneral.AutoSize = True
        Me.rbtGeneral.Location = New System.Drawing.Point(3, 2)
        Me.rbtGeneral.Name = "rbtGeneral"
        Me.rbtGeneral.Size = New System.Drawing.Size(70, 17)
        Me.rbtGeneral.TabIndex = 0
        Me.rbtGeneral.TabStop = True
        Me.rbtGeneral.Text = "General"
        Me.rbtGeneral.UseVisualStyleBackColor = True
        '
        'rbtGram
        '
        Me.rbtGram.AutoSize = True
        Me.rbtGram.Location = New System.Drawing.Point(79, 3)
        Me.rbtGram.Name = "rbtGram"
        Me.rbtGram.Size = New System.Drawing.Size(57, 17)
        Me.rbtGram.TabIndex = 1
        Me.rbtGram.TabStop = True
        Me.rbtGram.Text = "Gram"
        Me.rbtGram.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbtPurewt)
        Me.Panel1.Controls.Add(Me.rbtGrsWt)
        Me.Panel1.Controls.Add(Me.rbtNetWt)
        Me.Panel1.Location = New System.Drawing.Point(480, 10)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(316, 22)
        Me.Panel1.TabIndex = 13
        '
        'rbtPurewt
        '
        Me.rbtPurewt.AutoSize = True
        Me.rbtPurewt.Location = New System.Drawing.Point(211, 3)
        Me.rbtPurewt.Name = "rbtPurewt"
        Me.rbtPurewt.Size = New System.Drawing.Size(94, 17)
        Me.rbtPurewt.TabIndex = 2
        Me.rbtPurewt.TabStop = True
        Me.rbtPurewt.Text = "Pure Weight"
        Me.rbtPurewt.UseVisualStyleBackColor = True
        '
        'rbtGrsWt
        '
        Me.rbtGrsWt.AutoSize = True
        Me.rbtGrsWt.Location = New System.Drawing.Point(11, 3)
        Me.rbtGrsWt.Name = "rbtGrsWt"
        Me.rbtGrsWt.Size = New System.Drawing.Size(101, 17)
        Me.rbtGrsWt.TabIndex = 0
        Me.rbtGrsWt.TabStop = True
        Me.rbtGrsWt.Text = "Gross Weight"
        Me.rbtGrsWt.UseVisualStyleBackColor = True
        '
        'rbtNetWt
        '
        Me.rbtNetWt.AutoSize = True
        Me.rbtNetWt.Location = New System.Drawing.Point(118, 3)
        Me.rbtNetWt.Name = "rbtNetWt"
        Me.rbtNetWt.Size = New System.Drawing.Size(87, 17)
        Me.rbtNetWt.TabIndex = 1
        Me.rbtNetWt.TabStop = True
        Me.rbtNetWt.Text = "Net Weight"
        Me.rbtNetWt.UseVisualStyleBackColor = True
        '
        'cmbMetalType
        '
        Me.cmbMetalType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMetalType.FormattingEnabled = True
        Me.cmbMetalType.Location = New System.Drawing.Point(121, 113)
        Me.cmbMetalType.Name = "cmbMetalType"
        Me.cmbMetalType.Size = New System.Drawing.Size(222, 21)
        Me.cmbMetalType.TabIndex = 11
        '
        'cmbCatName
        '
        Me.cmbCatName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCatName.FormattingEnabled = True
        Me.cmbCatName.Location = New System.Drawing.Point(480, 37)
        Me.cmbCatName.Name = "cmbCatName"
        Me.cmbCatName.Size = New System.Drawing.Size(386, 21)
        Me.cmbCatName.TabIndex = 15
        '
        'cmbMetal
        '
        Me.cmbMetal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(121, 88)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(222, 21)
        Me.cmbMetal.TabIndex = 9
        '
        'chkWithValue
        '
        Me.chkWithValue.AutoSize = True
        Me.chkWithValue.Location = New System.Drawing.Point(480, 90)
        Me.chkWithValue.Name = "chkWithValue"
        Me.chkWithValue.Size = New System.Drawing.Size(87, 17)
        Me.chkWithValue.TabIndex = 18
        Me.chkWithValue.Text = "With Value"
        Me.chkWithValue.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(439, 143)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 27
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(227, 143)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 25
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(216, 9)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(25, 21)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "To"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkWithOtherIssue
        '
        Me.chkWithOtherIssue.AutoSize = True
        Me.chkWithOtherIssue.Location = New System.Drawing.Point(574, 90)
        Me.chkWithOtherIssue.Name = "chkWithOtherIssue"
        Me.chkWithOtherIssue.Size = New System.Drawing.Size(93, 17)
        Me.chkWithOtherIssue.TabIndex = 19
        Me.chkWithOtherIssue.Text = "Other Issue"
        Me.chkWithOtherIssue.UseVisualStyleBackColor = True
        '
        'chkWithStuddedStone
        '
        Me.chkWithStuddedStone.AutoSize = True
        Me.chkWithStuddedStone.Location = New System.Drawing.Point(673, 90)
        Me.chkWithStuddedStone.Name = "chkWithStuddedStone"
        Me.chkWithStuddedStone.Size = New System.Drawing.Size(139, 17)
        Me.chkWithStuddedStone.TabIndex = 20
        Me.chkWithStuddedStone.Text = "With Studded Stone"
        Me.chkWithStuddedStone.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(121, 143)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 24
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(355, 62)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 21)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "Stone Unit"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(6, 112)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(115, 21)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Metal Type"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(355, 36)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(82, 21)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "Category"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(6, 86)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(115, 21)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Metal Combination"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(6, 9)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(115, 21)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Date From"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(355, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(115, 21)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "Weight Based On"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnlView
        '
        Me.pnlView.Controls.Add(Me.gridView)
        Me.pnlView.Controls.Add(Me.pnlHeading)
        Me.pnlView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlView.Location = New System.Drawing.Point(0, 179)
        Me.pnlView.Name = "pnlView"
        Me.pnlView.Size = New System.Drawing.Size(1028, 417)
        Me.pnlView.TabIndex = 3
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.ContextMenuStrip = Me.ContextMenuStrip2
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 31)
        Me.gridView.Name = "gridView"
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.gridView.Size = New System.Drawing.Size(1028, 386)
        Me.gridView.TabIndex = 0
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AutoResizeToolStripMenuItem})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(133, 26)
        '
        'AutoResizeToolStripMenuItem
        '
        Me.AutoResizeToolStripMenuItem.CheckOnClick = True
        Me.AutoResizeToolStripMenuItem.Name = "AutoResizeToolStripMenuItem"
        Me.AutoResizeToolStripMenuItem.Size = New System.Drawing.Size(132, 22)
        Me.AutoResizeToolStripMenuItem.Text = "AutoResize"
        '
        'pnlHeading
        '
        Me.pnlHeading.Controls.Add(Me.lblTitle)
        Me.pnlHeading.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeading.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeading.Name = "pnlHeading"
        Me.pnlHeading.Size = New System.Drawing.Size(1028, 31)
        Me.pnlHeading.TabIndex = 1
        '
        'lblTitle
        '
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1028, 31)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Label8"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmCategoryStockNew
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 596)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlView)
        Me.Controls.Add(Me.grpMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmCategoryStockNew"
        Me.Text = "Category Stock"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.grpMain.ResumeLayout(False)
        Me.grpMain.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlView.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip2.ResumeLayout(False)
        Me.pnlHeading.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpMain As System.Windows.Forms.GroupBox
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents chkWithStuddedStone As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithOtherIssue As System.Windows.Forms.CheckBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkWithValue As System.Windows.Forms.CheckBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbMetal As System.Windows.Forms.ComboBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rbtCarat As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGeneral As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGram As System.Windows.Forms.RadioButton
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rbtGrsWt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtNetWt As System.Windows.Forms.RadioButton
    Friend WithEvents cmbMetalType As System.Windows.Forms.ComboBox
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents pnlView As System.Windows.Forms.Panel
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents pnlHeading As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents cmbCatName As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkCmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Label As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents chkWithApproval As System.Windows.Forms.CheckBox
    Friend WithEvents rbtPurewt As System.Windows.Forms.RadioButton
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AutoResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbtnParty As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnTranno As System.Windows.Forms.RadioButton
    Friend WithEvents Label7 As System.Windows.Forms.Label
End Class
