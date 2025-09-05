<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTagedItemsStockViewOnlProduct
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.pnlMain = New System.Windows.Forms.Panel()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabGeneral = New System.Windows.Forms.TabPage()
        Me.grpFiltration = New System.Windows.Forms.GroupBox()
        Me.dateTo = New System.Windows.Forms.Label()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.chkdate = New System.Windows.Forms.CheckBox()
        Me.cmbStockType = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox()
        Me.chkCmbCategory = New BrighttechPack.CheckedComboBox()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.cmbCounterName = New BrighttechPack.CheckedComboBox()
        Me.cmbSubItemName = New BrighttechPack.CheckedComboBox()
        Me.LstOrderby = New System.Windows.Forms.ListBox()
        Me.chkCmbCompany = New BrighttechPack.CheckedComboBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.chkCmbMetal = New BrighttechPack.CheckedComboBox()
        Me.chkApproval = New System.Windows.Forms.CheckBox()
        Me.cmbSize = New System.Windows.Forms.ComboBox()
        Me.dtpAsOnDate = New BrighttechPack.DatePicker(Me.components)
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.cmbItemType = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtItemName = New System.Windows.Forms.TextBox()
        Me.txtItemCode_NUM = New System.Windows.Forms.TextBox()
        Me.cmbGroup = New System.Windows.Forms.ComboBox()
        Me.cmbCostCenter = New System.Windows.Forms.ComboBox()
        Me.CmbGroupBy = New System.Windows.Forms.ComboBox()
        Me.tabView = New System.Windows.Forms.TabPage()
        Me.pnlTotalGridView = New System.Windows.Forms.Panel()
        Me.gridTotalView = New System.Windows.Forms.DataGridView()
        Me.lblReportTitle = New System.Windows.Forms.Label()
        Me.pnlSummary = New System.Windows.Forms.Panel()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtTagno = New System.Windows.Forms.TextBox()
        Me.pnlMain.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.grpFiltration.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.pnlTotalGridView.SuspendLayout()
        CType(Me.gridTotalView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlSummary.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.cmbGridShortCut.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlMain
        '
        Me.pnlMain.Controls.Add(Me.tabMain)
        Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlMain.Location = New System.Drawing.Point(0, 0)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(1020, 643)
        Me.pnlMain.TabIndex = 0
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
        Me.tabMain.Size = New System.Drawing.Size(1020, 643)
        Me.tabMain.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.grpFiltration)
        Me.tabGeneral.Controls.Add(Me.cmbGroup)
        Me.tabGeneral.Controls.Add(Me.cmbCostCenter)
        Me.tabGeneral.Controls.Add(Me.CmbGroupBy)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 9)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(1012, 630)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'grpFiltration
        '
        Me.grpFiltration.Controls.Add(Me.Label2)
        Me.grpFiltration.Controls.Add(Me.txtTagno)
        Me.grpFiltration.Controls.Add(Me.dateTo)
        Me.grpFiltration.Controls.Add(Me.dtpTo)
        Me.grpFiltration.Controls.Add(Me.chkdate)
        Me.grpFiltration.Controls.Add(Me.cmbStockType)
        Me.grpFiltration.Controls.Add(Me.Label6)
        Me.grpFiltration.Controls.Add(Me.chkCmbCostCentre)
        Me.grpFiltration.Controls.Add(Me.chkCmbCategory)
        Me.grpFiltration.Controls.Add(Me.Label30)
        Me.grpFiltration.Controls.Add(Me.cmbCounterName)
        Me.grpFiltration.Controls.Add(Me.cmbSubItemName)
        Me.grpFiltration.Controls.Add(Me.LstOrderby)
        Me.grpFiltration.Controls.Add(Me.chkCmbCompany)
        Me.grpFiltration.Controls.Add(Me.Label22)
        Me.grpFiltration.Controls.Add(Me.Label21)
        Me.grpFiltration.Controls.Add(Me.chkCmbMetal)
        Me.grpFiltration.Controls.Add(Me.chkApproval)
        Me.grpFiltration.Controls.Add(Me.cmbSize)
        Me.grpFiltration.Controls.Add(Me.dtpAsOnDate)
        Me.grpFiltration.Controls.Add(Me.btnNew)
        Me.grpFiltration.Controls.Add(Me.btnExit)
        Me.grpFiltration.Controls.Add(Me.btnView_Search)
        Me.grpFiltration.Controls.Add(Me.cmbItemType)
        Me.grpFiltration.Controls.Add(Me.Label9)
        Me.grpFiltration.Controls.Add(Me.Label17)
        Me.grpFiltration.Controls.Add(Me.Label8)
        Me.grpFiltration.Controls.Add(Me.Label7)
        Me.grpFiltration.Controls.Add(Me.Label5)
        Me.grpFiltration.Controls.Add(Me.Label1)
        Me.grpFiltration.Controls.Add(Me.txtItemName)
        Me.grpFiltration.Controls.Add(Me.txtItemCode_NUM)
        Me.grpFiltration.Location = New System.Drawing.Point(273, 76)
        Me.grpFiltration.Name = "grpFiltration"
        Me.grpFiltration.Size = New System.Drawing.Size(469, 495)
        Me.grpFiltration.TabIndex = 0
        Me.grpFiltration.TabStop = False
        '
        'dateTo
        '
        Me.dateTo.AutoSize = True
        Me.dateTo.Location = New System.Drawing.Point(255, 182)
        Me.dateTo.Name = "dateTo"
        Me.dateTo.Size = New System.Drawing.Size(20, 13)
        Me.dateTo.TabIndex = 11
        Me.dateTo.Text = "To"
        Me.dateTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.dateTo.Visible = False
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(285, 179)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(83, 21)
        Me.dtpTo.TabIndex = 12
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        Me.dtpTo.Visible = False
        '
        'chkdate
        '
        Me.chkdate.AutoSize = True
        Me.chkdate.Location = New System.Drawing.Point(71, 183)
        Me.chkdate.Name = "chkdate"
        Me.chkdate.Size = New System.Drawing.Size(83, 17)
        Me.chkdate.TabIndex = 9
        Me.chkdate.Text = "AsOnDate"
        Me.chkdate.UseVisualStyleBackColor = True
        '
        'cmbStockType
        '
        Me.cmbStockType.FormattingEnabled = True
        Me.cmbStockType.Location = New System.Drawing.Point(161, 325)
        Me.cmbStockType.Name = "cmbStockType"
        Me.cmbStockType.Size = New System.Drawing.Size(224, 21)
        Me.cmbStockType.TabIndex = 26
        Me.cmbStockType.Visible = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(68, 333)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(70, 13)
        Me.Label6.TabIndex = 25
        Me.Label6.Text = "Stock Type"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label6.Visible = False
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(163, 250)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(224, 22)
        Me.chkCmbCostCentre.TabIndex = 20
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'chkCmbCategory
        '
        Me.chkCmbCategory.CheckOnClick = True
        Me.chkCmbCategory.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCmbCategory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCategory.DropDownHeight = 1
        Me.chkCmbCategory.FormattingEnabled = True
        Me.chkCmbCategory.IntegralHeight = False
        Me.chkCmbCategory.Location = New System.Drawing.Point(161, 112)
        Me.chkCmbCategory.Name = "chkCmbCategory"
        Me.chkCmbCategory.Size = New System.Drawing.Size(224, 22)
        Me.chkCmbCategory.TabIndex = 3
        Me.chkCmbCategory.ValueSeparator = ", "
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Location = New System.Drawing.Point(68, 117)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(60, 13)
        Me.Label30.TabIndex = 2
        Me.Label30.Text = "Category"
        Me.Label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbCounterName
        '
        Me.cmbCounterName.CheckOnClick = True
        Me.cmbCounterName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbCounterName.DropDownHeight = 1
        Me.cmbCounterName.FormattingEnabled = True
        Me.cmbCounterName.IntegralHeight = False
        Me.cmbCounterName.Location = New System.Drawing.Point(163, 207)
        Me.cmbCounterName.Name = "cmbCounterName"
        Me.cmbCounterName.Size = New System.Drawing.Size(224, 22)
        Me.cmbCounterName.TabIndex = 14
        Me.cmbCounterName.ValueSeparator = ", "
        '
        'cmbSubItemName
        '
        Me.cmbSubItemName.CheckOnClick = True
        Me.cmbSubItemName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbSubItemName.DropDownHeight = 1
        Me.cmbSubItemName.FormattingEnabled = True
        Me.cmbSubItemName.IntegralHeight = False
        Me.cmbSubItemName.Location = New System.Drawing.Point(161, 155)
        Me.cmbSubItemName.Name = "cmbSubItemName"
        Me.cmbSubItemName.Size = New System.Drawing.Size(224, 22)
        Me.cmbSubItemName.TabIndex = 8
        Me.cmbSubItemName.ValueSeparator = ", "
        '
        'LstOrderby
        '
        Me.LstOrderby.Enabled = False
        Me.LstOrderby.FormattingEnabled = True
        Me.LstOrderby.Location = New System.Drawing.Point(476, 254)
        Me.LstOrderby.Name = "LstOrderby"
        Me.LstOrderby.Size = New System.Drawing.Size(89, 82)
        Me.LstOrderby.TabIndex = 1
        Me.LstOrderby.Visible = False
        '
        'chkCmbCompany
        '
        Me.chkCmbCompany.CheckOnClick = True
        Me.chkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCompany.DropDownHeight = 1
        Me.chkCmbCompany.FormattingEnabled = True
        Me.chkCmbCompany.IntegralHeight = False
        Me.chkCmbCompany.Location = New System.Drawing.Point(163, 272)
        Me.chkCmbCompany.Name = "chkCmbCompany"
        Me.chkCmbCompany.Size = New System.Drawing.Size(224, 22)
        Me.chkCmbCompany.TabIndex = 22
        Me.chkCmbCompany.ValueSeparator = ", "
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(70, 277)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(62, 13)
        Me.Label22.TabIndex = 21
        Me.Label22.Text = "Company"
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(68, 95)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(37, 13)
        Me.Label21.TabIndex = 0
        Me.Label21.Text = "Metal"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbMetal
        '
        Me.chkCmbMetal.CheckOnClick = True
        Me.chkCmbMetal.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCmbMetal.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbMetal.DropDownHeight = 1
        Me.chkCmbMetal.FormattingEnabled = True
        Me.chkCmbMetal.IntegralHeight = False
        Me.chkCmbMetal.Location = New System.Drawing.Point(161, 91)
        Me.chkCmbMetal.Name = "chkCmbMetal"
        Me.chkCmbMetal.Size = New System.Drawing.Size(224, 22)
        Me.chkCmbMetal.TabIndex = 1
        Me.chkCmbMetal.ValueSeparator = ", "
        '
        'chkApproval
        '
        Me.chkApproval.AutoSize = True
        Me.chkApproval.Checked = True
        Me.chkApproval.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkApproval.Location = New System.Drawing.Point(161, 352)
        Me.chkApproval.Name = "chkApproval"
        Me.chkApproval.Size = New System.Drawing.Size(106, 17)
        Me.chkApproval.TabIndex = 27
        Me.chkApproval.Text = "With Approval"
        Me.chkApproval.UseVisualStyleBackColor = True
        '
        'cmbSize
        '
        Me.cmbSize.FormattingEnabled = True
        Me.cmbSize.Location = New System.Drawing.Point(288, 229)
        Me.cmbSize.Name = "cmbSize"
        Me.cmbSize.Size = New System.Drawing.Size(99, 21)
        Me.cmbSize.TabIndex = 18
        '
        'dtpAsOnDate
        '
        Me.dtpAsOnDate.Location = New System.Drawing.Point(161, 179)
        Me.dtpAsOnDate.Mask = "##/##/####"
        Me.dtpAsOnDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpAsOnDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpAsOnDate.Name = "dtpAsOnDate"
        Me.dtpAsOnDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpAsOnDate.Size = New System.Drawing.Size(83, 21)
        Me.dtpAsOnDate.TabIndex = 10
        Me.dtpAsOnDate.Text = "07/03/9998"
        Me.dtpAsOnDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(182, 375)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 29
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(292, 375)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 30
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(69, 375)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 28
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'cmbItemType
        '
        Me.cmbItemType.FormattingEnabled = True
        Me.cmbItemType.Location = New System.Drawing.Point(163, 229)
        Me.cmbItemType.Name = "cmbItemType"
        Me.cmbItemType.Size = New System.Drawing.Size(90, 21)
        Me.cmbItemType.TabIndex = 16
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(70, 255)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(76, 13)
        Me.Label9.TabIndex = 19
        Me.Label9.Text = "Cost Centre"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(256, 233)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(31, 13)
        Me.Label17.TabIndex = 17
        Me.Label17.Text = "Size"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(70, 233)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(65, 13)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "Item Type"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(70, 212)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(90, 13)
        Me.Label7.TabIndex = 13
        Me.Label7.Text = "Counter Name"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(68, 160)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(93, 13)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "SubItem Name"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(68, 138)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Item Code"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtItemName
        '
        Me.txtItemName.BackColor = System.Drawing.SystemColors.Window
        Me.txtItemName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtItemName.Enabled = False
        Me.txtItemName.Location = New System.Drawing.Point(259, 135)
        Me.txtItemName.Name = "txtItemName"
        Me.txtItemName.Size = New System.Drawing.Size(126, 21)
        Me.txtItemName.TabIndex = 6
        '
        'txtItemCode_NUM
        '
        Me.txtItemCode_NUM.BackColor = System.Drawing.SystemColors.Window
        Me.txtItemCode_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtItemCode_NUM.Location = New System.Drawing.Point(161, 134)
        Me.txtItemCode_NUM.MaxLength = 8
        Me.txtItemCode_NUM.Name = "txtItemCode_NUM"
        Me.txtItemCode_NUM.Size = New System.Drawing.Size(93, 21)
        Me.txtItemCode_NUM.TabIndex = 5
        '
        'cmbGroup
        '
        Me.cmbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbGroup.FormattingEnabled = True
        Me.cmbGroup.Location = New System.Drawing.Point(723, 343)
        Me.cmbGroup.Name = "cmbGroup"
        Me.cmbGroup.Size = New System.Drawing.Size(224, 21)
        Me.cmbGroup.TabIndex = 37
        Me.cmbGroup.Visible = False
        '
        'cmbCostCenter
        '
        Me.cmbCostCenter.FormattingEnabled = True
        Me.cmbCostCenter.Location = New System.Drawing.Point(723, 286)
        Me.cmbCostCenter.Name = "cmbCostCenter"
        Me.cmbCostCenter.Size = New System.Drawing.Size(224, 21)
        Me.cmbCostCenter.TabIndex = 26
        Me.cmbCostCenter.Visible = False
        '
        'CmbGroupBy
        '
        Me.CmbGroupBy.FormattingEnabled = True
        Me.CmbGroupBy.Location = New System.Drawing.Point(723, 316)
        Me.CmbGroupBy.Name = "CmbGroupBy"
        Me.CmbGroupBy.Size = New System.Drawing.Size(224, 21)
        Me.CmbGroupBy.TabIndex = 52
        Me.CmbGroupBy.Visible = False
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.pnlTotalGridView)
        Me.tabView.Location = New System.Drawing.Point(4, 9)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1012, 630)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'pnlTotalGridView
        '
        Me.pnlTotalGridView.Controls.Add(Me.gridTotalView)
        Me.pnlTotalGridView.Controls.Add(Me.lblReportTitle)
        Me.pnlTotalGridView.Controls.Add(Me.pnlSummary)
        Me.pnlTotalGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlTotalGridView.Location = New System.Drawing.Point(3, 3)
        Me.pnlTotalGridView.Name = "pnlTotalGridView"
        Me.pnlTotalGridView.Size = New System.Drawing.Size(1006, 624)
        Me.pnlTotalGridView.TabIndex = 6
        '
        'gridTotalView
        '
        Me.gridTotalView.AllowUserToAddRows = False
        Me.gridTotalView.AllowUserToDeleteRows = False
        Me.gridTotalView.BorderStyle = System.Windows.Forms.BorderStyle.None
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.gridTotalView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.gridTotalView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridTotalView.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.gridTotalView.Location = New System.Drawing.Point(0, 58)
        Me.gridTotalView.MultiSelect = False
        Me.gridTotalView.Name = "gridTotalView"
        Me.gridTotalView.ReadOnly = True
        Me.gridTotalView.RowHeadersVisible = False
        Me.gridTotalView.RowHeadersWidth = 25
        Me.gridTotalView.RowTemplate.Height = 20
        Me.gridTotalView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridTotalView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridTotalView.ShowCellToolTips = False
        Me.gridTotalView.Size = New System.Drawing.Size(1006, 516)
        Me.gridTotalView.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.gridTotalView, "<S>StuddedDetails,<X>Excel Posting")
        '
        'lblReportTitle
        '
        Me.lblReportTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblReportTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReportTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblReportTitle.Name = "lblReportTitle"
        Me.lblReportTitle.Size = New System.Drawing.Size(1006, 43)
        Me.lblReportTitle.TabIndex = 0
        Me.lblReportTitle.Text = "Label5"
        Me.lblReportTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlSummary
        '
        Me.pnlSummary.BackColor = System.Drawing.SystemColors.Control
        Me.pnlSummary.Controls.Add(Me.btnPrint)
        Me.pnlSummary.Controls.Add(Me.btnExport)
        Me.pnlSummary.Controls.Add(Me.btnBack)
        Me.pnlSummary.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlSummary.Location = New System.Drawing.Point(0, 574)
        Me.pnlSummary.Name = "pnlSummary"
        Me.pnlSummary.Size = New System.Drawing.Size(1006, 50)
        Me.pnlSummary.TabIndex = 2
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(889, 12)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(99, 29)
        Me.btnPrint.TabIndex = 4
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(784, 12)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(99, 29)
        Me.btnExport.TabIndex = 4
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(679, 12)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(99, 29)
        Me.btnBack.TabIndex = 3
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.NewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 48)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ToolTip1
        '
        Me.ToolTip1.AutomaticDelay = 1000
        Me.ToolTip1.IsBalloon = True
        Me.ToolTip1.ShowAlways = True
        '
        'cmbGridShortCut
        '
        Me.cmbGridShortCut.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResizeToolStripMenuItem})
        Me.cmbGridShortCut.Name = "ContextMenuStrip2"
        Me.cmbGridShortCut.Size = New System.Drawing.Size(136, 26)
        '
        'ResizeToolStripMenuItem
        '
        Me.ResizeToolStripMenuItem.CheckOnClick = True
        Me.ResizeToolStripMenuItem.Name = "ResizeToolStripMenuItem"
        Me.ResizeToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.ResizeToolStripMenuItem.Text = "Auto Resize"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(78, 302)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(41, 13)
        Me.Label2.TabIndex = 23
        Me.Label2.Text = "Tagno"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtTagno
        '
        Me.txtTagno.BackColor = System.Drawing.SystemColors.Window
        Me.txtTagno.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTagno.Location = New System.Drawing.Point(162, 298)
        Me.txtTagno.MaxLength = 8
        Me.txtTagno.Name = "txtTagno"
        Me.txtTagno.Size = New System.Drawing.Size(157, 21)
        Me.txtTagno.TabIndex = 24
        '
        'frmTagedItemsStockViewOnlProduct
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1020, 643)
        Me.ContextMenuStrip = Me.cmbGridShortCut
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmTagedItemsStockViewOnlProduct"
        Me.Text = "Item Stock View"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlMain.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.grpFiltration.ResumeLayout(False)
        Me.grpFiltration.PerformLayout()
        Me.tabView.ResumeLayout(False)
        Me.pnlTotalGridView.ResumeLayout(False)
        CType(Me.gridTotalView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlSummary.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.cmbGridShortCut.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlMain As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtItemName As System.Windows.Forms.TextBox
    Friend WithEvents txtItemCode_NUM As System.Windows.Forms.TextBox
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents pnlTotalGridView As System.Windows.Forms.Panel
    Friend WithEvents gridTotalView As System.Windows.Forms.DataGridView
    Friend WithEvents lblReportTitle As System.Windows.Forms.Label
    Friend WithEvents pnlSummary As System.Windows.Forms.Panel
    Friend WithEvents grpFiltration As System.Windows.Forms.GroupBox
    Friend WithEvents cmbCostCenter As System.Windows.Forms.ComboBox
    Friend WithEvents cmbItemType As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents dtpAsOnDate As BrighttechPack.DatePicker
    Friend WithEvents cmbSize As System.Windows.Forms.ComboBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents cmbGroup As System.Windows.Forms.ComboBox
    Friend WithEvents chkApproval As System.Windows.Forms.CheckBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents chkCmbMetal As BrighttechPack.CheckedComboBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents chkCmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents checkedcomboboxitem As BrighttechPack.CheckedComboBox
    Friend WithEvents LstOrderby As System.Windows.Forms.ListBox
    Friend WithEvents cmbSubItemName As BrighttechPack.CheckedComboBox
    Friend WithEvents cmbCounterName As BrighttechPack.CheckedComboBox
    Friend WithEvents CmbGroupBy As System.Windows.Forms.ComboBox
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents chkCmbCategory As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents cmbStockType As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents chkdate As System.Windows.Forms.CheckBox
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dateTo As System.Windows.Forms.Label
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label2 As Label
    Friend WithEvents txtTagno As TextBox
End Class
