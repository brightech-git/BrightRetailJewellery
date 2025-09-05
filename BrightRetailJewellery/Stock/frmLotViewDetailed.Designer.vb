<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLotViewDetailed
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
        Me.grpContainer = New System.Windows.Forms.GroupBox()
        Me.cmbUserName = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.chkLstCompany = New System.Windows.Forms.CheckedListBox()
        Me.cmbItemName = New System.Windows.Forms.ComboBox()
        Me.chkCompanySelectAll = New System.Windows.Forms.CheckBox()
        Me.cmbDesigner = New System.Windows.Forms.ComboBox()
        Me.chkMultiDesigner = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtLotNoTo_NUM = New System.Windows.Forms.TextBox()
        Me.ChkSalVal = New System.Windows.Forms.CheckBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.rbtLTLot = New System.Windows.Forms.RadioButton()
        Me.rbtLTReceipt = New System.Windows.Forms.RadioButton()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.rbtLTBoth = New System.Windows.Forms.RadioButton()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtLotNo_NUM = New System.Windows.Forms.TextBox()
        Me.chkOrderByLotno = New System.Windows.Forms.CheckBox()
        Me.chkOrderByItemId = New System.Windows.Forms.CheckBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.rbtGroupLot = New System.Windows.Forms.RadioButton()
        Me.rbtGroupDesigner = New System.Windows.Forms.RadioButton()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.rbtGroupNone = New System.Windows.Forms.RadioButton()
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.chkLstCategory = New System.Windows.Forms.CheckedListBox()
        Me.rbtDetailed = New System.Windows.Forms.RadioButton()
        Me.rbtSummary = New System.Windows.Forms.RadioButton()
        Me.chkCategorySelectAll = New System.Windows.Forms.CheckBox()
        Me.chkItemCounterSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox()
        Me.chkDesignerSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkMetalSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstMetal = New System.Windows.Forms.CheckedListBox()
        Me.chkLstItemCounter = New System.Windows.Forms.CheckedListBox()
        Me.chkCostCentreSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstDesigner = New System.Windows.Forms.CheckedListBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.grpContainer.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpContainer
        '
        Me.grpContainer.Controls.Add(Me.cmbUserName)
        Me.grpContainer.Controls.Add(Me.Label7)
        Me.grpContainer.Controls.Add(Me.chkLstCompany)
        Me.grpContainer.Controls.Add(Me.cmbItemName)
        Me.grpContainer.Controls.Add(Me.chkCompanySelectAll)
        Me.grpContainer.Controls.Add(Me.cmbDesigner)
        Me.grpContainer.Controls.Add(Me.chkMultiDesigner)
        Me.grpContainer.Controls.Add(Me.Label5)
        Me.grpContainer.Controls.Add(Me.Label6)
        Me.grpContainer.Controls.Add(Me.txtLotNoTo_NUM)
        Me.grpContainer.Controls.Add(Me.ChkSalVal)
        Me.grpContainer.Controls.Add(Me.Panel2)
        Me.grpContainer.Controls.Add(Me.Label10)
        Me.grpContainer.Controls.Add(Me.txtLotNo_NUM)
        Me.grpContainer.Controls.Add(Me.chkOrderByLotno)
        Me.grpContainer.Controls.Add(Me.chkOrderByItemId)
        Me.grpContainer.Controls.Add(Me.dtpTo)
        Me.grpContainer.Controls.Add(Me.Panel1)
        Me.grpContainer.Controls.Add(Me.dtpFrom)
        Me.grpContainer.Controls.Add(Me.chkLstCategory)
        Me.grpContainer.Controls.Add(Me.rbtDetailed)
        Me.grpContainer.Controls.Add(Me.rbtSummary)
        Me.grpContainer.Controls.Add(Me.chkCategorySelectAll)
        Me.grpContainer.Controls.Add(Me.chkItemCounterSelectAll)
        Me.grpContainer.Controls.Add(Me.chkLstCostCentre)
        Me.grpContainer.Controls.Add(Me.chkDesignerSelectAll)
        Me.grpContainer.Controls.Add(Me.chkMetalSelectAll)
        Me.grpContainer.Controls.Add(Me.chkLstMetal)
        Me.grpContainer.Controls.Add(Me.chkLstItemCounter)
        Me.grpContainer.Controls.Add(Me.chkCostCentreSelectAll)
        Me.grpContainer.Controls.Add(Me.chkLstDesigner)
        Me.grpContainer.Controls.Add(Me.Label2)
        Me.grpContainer.Controls.Add(Me.Label1)
        Me.grpContainer.Controls.Add(Me.btnSearch)
        Me.grpContainer.Controls.Add(Me.btnExit)
        Me.grpContainer.Controls.Add(Me.btnNew)
        Me.grpContainer.Location = New System.Drawing.Point(238, 13)
        Me.grpContainer.Name = "grpContainer"
        Me.grpContainer.Size = New System.Drawing.Size(430, 566)
        Me.grpContainer.TabIndex = 0
        Me.grpContainer.TabStop = False
        '
        'cmbUserName
        '
        Me.cmbUserName.FormattingEnabled = True
        Me.cmbUserName.Location = New System.Drawing.Point(124, 375)
        Me.cmbUserName.Name = "cmbUserName"
        Me.cmbUserName.Size = New System.Drawing.Size(294, 21)
        Me.cmbUserName.TabIndex = 21
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(9, 375)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(100, 21)
        Me.Label7.TabIndex = 20
        Me.Label7.Text = "UserName"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkLstCompany
        '
        Me.chkLstCompany.FormattingEnabled = True
        Me.chkLstCompany.Location = New System.Drawing.Point(114, 36)
        Me.chkLstCompany.Name = "chkLstCompany"
        Me.chkLstCompany.Size = New System.Drawing.Size(302, 52)
        Me.chkLstCompany.TabIndex = 5
        '
        'cmbItemName
        '
        Me.cmbItemName.FormattingEnabled = True
        Me.cmbItemName.Location = New System.Drawing.Point(122, 252)
        Me.cmbItemName.Name = "cmbItemName"
        Me.cmbItemName.Size = New System.Drawing.Size(294, 21)
        Me.cmbItemName.TabIndex = 13
        '
        'chkCompanySelectAll
        '
        Me.chkCompanySelectAll.AutoSize = True
        Me.chkCompanySelectAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkCompanySelectAll.Location = New System.Drawing.Point(9, 54)
        Me.chkCompanySelectAll.Name = "chkCompanySelectAll"
        Me.chkCompanySelectAll.Size = New System.Drawing.Size(81, 17)
        Me.chkCompanySelectAll.TabIndex = 4
        Me.chkCompanySelectAll.Text = "Company"
        Me.chkCompanySelectAll.UseVisualStyleBackColor = True
        '
        'cmbDesigner
        '
        Me.cmbDesigner.FormattingEnabled = True
        Me.cmbDesigner.Location = New System.Drawing.Point(123, 279)
        Me.cmbDesigner.Name = "cmbDesigner"
        Me.cmbDesigner.Size = New System.Drawing.Size(293, 21)
        Me.cmbDesigner.TabIndex = 15
        Me.cmbDesigner.Visible = False
        '
        'chkMultiDesigner
        '
        Me.chkMultiDesigner.AutoSize = True
        Me.chkMultiDesigner.Checked = True
        Me.chkMultiDesigner.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMultiDesigner.Location = New System.Drawing.Point(9, 279)
        Me.chkMultiDesigner.Name = "chkMultiDesigner"
        Me.chkMultiDesigner.Size = New System.Drawing.Size(91, 17)
        Me.chkMultiDesigner.TabIndex = 14
        Me.chkMultiDesigner.Text = "Multi Select"
        Me.chkMultiDesigner.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(231, 402)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(20, 13)
        Me.Label5.TabIndex = 24
        Me.Label5.Text = "To"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(9, 252)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(100, 21)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "ItemName"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtLotNoTo_NUM
        '
        Me.txtLotNoTo_NUM.BackColor = System.Drawing.SystemColors.Window
        Me.txtLotNoTo_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtLotNoTo_NUM.Location = New System.Drawing.Point(276, 398)
        Me.txtLotNoTo_NUM.MaxLength = 8
        Me.txtLotNoTo_NUM.Name = "txtLotNoTo_NUM"
        Me.txtLotNoTo_NUM.Size = New System.Drawing.Size(96, 21)
        Me.txtLotNoTo_NUM.TabIndex = 25
        '
        'ChkSalVal
        '
        Me.ChkSalVal.AutoSize = True
        Me.ChkSalVal.Location = New System.Drawing.Point(37, 503)
        Me.ChkSalVal.Name = "ChkSalVal"
        Me.ChkSalVal.Size = New System.Drawing.Size(104, 17)
        Me.ChkSalVal.TabIndex = 30
        Me.ChkSalVal.Text = "With Salvalue"
        Me.ChkSalVal.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rbtLTLot)
        Me.Panel2.Controls.Add(Me.rbtLTReceipt)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.rbtLTBoth)
        Me.Panel2.Location = New System.Drawing.Point(12, 444)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(404, 23)
        Me.Panel2.TabIndex = 28
        '
        'rbtLTLot
        '
        Me.rbtLTLot.AutoSize = True
        Me.rbtLTLot.Location = New System.Drawing.Point(291, 5)
        Me.rbtLTLot.Name = "rbtLTLot"
        Me.rbtLTLot.Size = New System.Drawing.Size(110, 17)
        Me.rbtLTLot.TabIndex = 3
        Me.rbtLTLot.TabStop = True
        Me.rbtLTLot.Text = "Direct Lot Only"
        Me.rbtLTLot.UseVisualStyleBackColor = True
        '
        'rbtLTReceipt
        '
        Me.rbtLTReceipt.AutoSize = True
        Me.rbtLTReceipt.Location = New System.Drawing.Point(162, 5)
        Me.rbtLTReceipt.Name = "rbtLTReceipt"
        Me.rbtLTReceipt.Size = New System.Drawing.Size(130, 17)
        Me.rbtLTReceipt.TabIndex = 2
        Me.rbtLTReceipt.TabStop = True
        Me.rbtLTReceipt.Text = "From Receipt Only"
        Me.rbtLTReceipt.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(3, 6)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(55, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Lot Type"
        '
        'rbtLTBoth
        '
        Me.rbtLTBoth.AutoSize = True
        Me.rbtLTBoth.Checked = True
        Me.rbtLTBoth.Location = New System.Drawing.Point(102, 5)
        Me.rbtLTBoth.Name = "rbtLTBoth"
        Me.rbtLTBoth.Size = New System.Drawing.Size(51, 17)
        Me.rbtLTBoth.TabIndex = 1
        Me.rbtLTBoth.TabStop = True
        Me.rbtLTBoth.Text = "Both"
        Me.rbtLTBoth.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(9, 402)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(76, 13)
        Me.Label10.TabIndex = 22
        Me.Label10.Text = "Lot No From"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtLotNo_NUM
        '
        Me.txtLotNo_NUM.BackColor = System.Drawing.SystemColors.Window
        Me.txtLotNo_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtLotNo_NUM.Location = New System.Drawing.Point(124, 398)
        Me.txtLotNo_NUM.MaxLength = 8
        Me.txtLotNo_NUM.Name = "txtLotNo_NUM"
        Me.txtLotNo_NUM.Size = New System.Drawing.Size(96, 21)
        Me.txtLotNo_NUM.TabIndex = 23
        '
        'chkOrderByLotno
        '
        Me.chkOrderByLotno.AutoSize = True
        Me.chkOrderByLotno.Location = New System.Drawing.Point(276, 503)
        Me.chkOrderByLotno.Name = "chkOrderByLotno"
        Me.chkOrderByLotno.Size = New System.Drawing.Size(118, 17)
        Me.chkOrderByLotno.TabIndex = 32
        Me.chkOrderByLotno.Text = "Order By Lot No"
        Me.chkOrderByLotno.UseVisualStyleBackColor = True
        '
        'chkOrderByItemId
        '
        Me.chkOrderByItemId.AutoSize = True
        Me.chkOrderByItemId.Location = New System.Drawing.Point(151, 503)
        Me.chkOrderByItemId.Name = "chkOrderByItemId"
        Me.chkOrderByItemId.Size = New System.Drawing.Size(119, 17)
        Me.chkOrderByItemId.TabIndex = 31
        Me.chkOrderByItemId.Text = "Order By Itemid"
        Me.chkOrderByItemId.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(217, 13)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(95, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbtGroupLot)
        Me.Panel1.Controls.Add(Me.rbtGroupDesigner)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.rbtGroupNone)
        Me.Panel1.Location = New System.Drawing.Point(12, 474)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(404, 23)
        Me.Panel1.TabIndex = 29
        '
        'rbtGroupLot
        '
        Me.rbtGroupLot.AutoSize = True
        Me.rbtGroupLot.Location = New System.Drawing.Point(244, 3)
        Me.rbtGroupLot.Name = "rbtGroupLot"
        Me.rbtGroupLot.Size = New System.Drawing.Size(61, 17)
        Me.rbtGroupLot.TabIndex = 3
        Me.rbtGroupLot.TabStop = True
        Me.rbtGroupLot.Text = "Lot No"
        Me.rbtGroupLot.UseVisualStyleBackColor = True
        '
        'rbtGroupDesigner
        '
        Me.rbtGroupDesigner.AutoSize = True
        Me.rbtGroupDesigner.Location = New System.Drawing.Point(162, 3)
        Me.rbtGroupDesigner.Name = "rbtGroupDesigner"
        Me.rbtGroupDesigner.Size = New System.Drawing.Size(76, 17)
        Me.rbtGroupDesigner.TabIndex = 2
        Me.rbtGroupDesigner.TabStop = True
        Me.rbtGroupDesigner.Text = "Designer"
        Me.rbtGroupDesigner.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 6)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Group by"
        '
        'rbtGroupNone
        '
        Me.rbtGroupNone.AutoSize = True
        Me.rbtGroupNone.Location = New System.Drawing.Point(102, 3)
        Me.rbtGroupNone.Name = "rbtGroupNone"
        Me.rbtGroupNone.Size = New System.Drawing.Size(54, 17)
        Me.rbtGroupNone.TabIndex = 1
        Me.rbtGroupNone.TabStop = True
        Me.rbtGroupNone.Text = "None"
        Me.rbtGroupNone.UseVisualStyleBackColor = True
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(82, 13)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(95, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'chkLstCategory
        '
        Me.chkLstCategory.FormattingEnabled = True
        Me.chkLstCategory.Location = New System.Drawing.Point(9, 180)
        Me.chkLstCategory.Name = "chkLstCategory"
        Me.chkLstCategory.Size = New System.Drawing.Size(407, 68)
        Me.chkLstCategory.TabIndex = 11
        '
        'rbtDetailed
        '
        Me.rbtDetailed.AutoSize = True
        Me.rbtDetailed.Checked = True
        Me.rbtDetailed.Location = New System.Drawing.Point(124, 423)
        Me.rbtDetailed.Name = "rbtDetailed"
        Me.rbtDetailed.Size = New System.Drawing.Size(72, 17)
        Me.rbtDetailed.TabIndex = 26
        Me.rbtDetailed.TabStop = True
        Me.rbtDetailed.Text = "Detailed"
        Me.rbtDetailed.UseVisualStyleBackColor = True
        '
        'rbtSummary
        '
        Me.rbtSummary.AutoSize = True
        Me.rbtSummary.Location = New System.Drawing.Point(231, 423)
        Me.rbtSummary.Name = "rbtSummary"
        Me.rbtSummary.Size = New System.Drawing.Size(81, 17)
        Me.rbtSummary.TabIndex = 27
        Me.rbtSummary.Text = "Summary"
        Me.rbtSummary.UseVisualStyleBackColor = True
        '
        'chkCategorySelectAll
        '
        Me.chkCategorySelectAll.AutoSize = True
        Me.chkCategorySelectAll.Location = New System.Drawing.Point(9, 163)
        Me.chkCategorySelectAll.Name = "chkCategorySelectAll"
        Me.chkCategorySelectAll.Size = New System.Drawing.Size(79, 17)
        Me.chkCategorySelectAll.TabIndex = 10
        Me.chkCategorySelectAll.Text = "Category"
        Me.chkCategorySelectAll.UseVisualStyleBackColor = True
        '
        'chkItemCounterSelectAll
        '
        Me.chkItemCounterSelectAll.AutoSize = True
        Me.chkItemCounterSelectAll.Location = New System.Drawing.Point(9, 347)
        Me.chkItemCounterSelectAll.Name = "chkItemCounterSelectAll"
        Me.chkItemCounterSelectAll.Size = New System.Drawing.Size(103, 17)
        Me.chkItemCounterSelectAll.TabIndex = 18
        Me.chkItemCounterSelectAll.Text = "Item Counter"
        Me.chkItemCounterSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(12, 108)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(201, 52)
        Me.chkLstCostCentre.TabIndex = 7
        '
        'chkDesignerSelectAll
        '
        Me.chkDesignerSelectAll.AutoSize = True
        Me.chkDesignerSelectAll.Location = New System.Drawing.Point(9, 314)
        Me.chkDesignerSelectAll.Name = "chkDesignerSelectAll"
        Me.chkDesignerSelectAll.Size = New System.Drawing.Size(77, 17)
        Me.chkDesignerSelectAll.TabIndex = 16
        Me.chkDesignerSelectAll.Text = "Designer"
        Me.chkDesignerSelectAll.UseVisualStyleBackColor = True
        '
        'chkMetalSelectAll
        '
        Me.chkMetalSelectAll.AutoSize = True
        Me.chkMetalSelectAll.Location = New System.Drawing.Point(217, 89)
        Me.chkMetalSelectAll.Name = "chkMetalSelectAll"
        Me.chkMetalSelectAll.Size = New System.Drawing.Size(56, 17)
        Me.chkMetalSelectAll.TabIndex = 8
        Me.chkMetalSelectAll.Text = "Metal"
        Me.chkMetalSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstMetal
        '
        Me.chkLstMetal.FormattingEnabled = True
        Me.chkLstMetal.Location = New System.Drawing.Point(217, 108)
        Me.chkLstMetal.Name = "chkLstMetal"
        Me.chkLstMetal.Size = New System.Drawing.Size(199, 52)
        Me.chkLstMetal.TabIndex = 9
        '
        'chkLstItemCounter
        '
        Me.chkLstItemCounter.FormattingEnabled = True
        Me.chkLstItemCounter.Location = New System.Drawing.Point(124, 337)
        Me.chkLstItemCounter.Name = "chkLstItemCounter"
        Me.chkLstItemCounter.Size = New System.Drawing.Size(292, 36)
        Me.chkLstItemCounter.TabIndex = 19
        '
        'chkCostCentreSelectAll
        '
        Me.chkCostCentreSelectAll.AutoSize = True
        Me.chkCostCentreSelectAll.Location = New System.Drawing.Point(9, 89)
        Me.chkCostCentreSelectAll.Name = "chkCostCentreSelectAll"
        Me.chkCostCentreSelectAll.Size = New System.Drawing.Size(95, 17)
        Me.chkCostCentreSelectAll.TabIndex = 6
        Me.chkCostCentreSelectAll.Text = "Cost Centre"
        Me.chkCostCentreSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstDesigner
        '
        Me.chkLstDesigner.FormattingEnabled = True
        Me.chkLstDesigner.Location = New System.Drawing.Point(122, 279)
        Me.chkLstDesigner.Name = "chkLstDesigner"
        Me.chkLstDesigner.Size = New System.Drawing.Size(294, 52)
        Me.chkLstDesigner.TabIndex = 17
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(187, 17)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(20, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date From"
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(54, 526)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 33
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(266, 526)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 35
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(160, 526)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 34
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(119, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'frmLotViewDetailed
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(868, 591)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.grpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmLotViewDetailed"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Lot View"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpContainer.ResumeLayout(False)
        Me.grpContainer.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents grpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents rbtSummary As System.Windows.Forms.RadioButton
    Friend WithEvents rbtDetailed As System.Windows.Forms.RadioButton
    Friend WithEvents chkLstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkLstMetal As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkLstItemCounter As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkLstDesigner As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkItemCounterSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkDesignerSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkCostCentreSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkMetalSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkLstCategory As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkCategorySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents rbtGroupDesigner As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGroupNone As System.Windows.Forms.RadioButton
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkOrderByItemId As System.Windows.Forms.CheckBox
    Friend WithEvents chkOrderByLotno As System.Windows.Forms.CheckBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtLotNo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rbtLTLot As System.Windows.Forms.RadioButton
    Friend WithEvents rbtLTReceipt As System.Windows.Forms.RadioButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents rbtLTBoth As System.Windows.Forms.RadioButton
    Friend WithEvents ChkSalVal As System.Windows.Forms.CheckBox
    Friend WithEvents rbtGroupLot As System.Windows.Forms.RadioButton
    Friend WithEvents Label5 As Label
    Friend WithEvents txtLotNoTo_NUM As TextBox
    Friend WithEvents chkMultiDesigner As CheckBox
    Friend WithEvents cmbDesigner As ComboBox
    Friend WithEvents cmbItemName As ComboBox
    Friend WithEvents Label6 As Label
    Friend WithEvents chkLstCompany As CheckedListBox
    Friend WithEvents chkCompanySelectAll As CheckBox
    Friend WithEvents cmbUserName As ComboBox
    Friend WithEvents Label7 As Label
End Class
