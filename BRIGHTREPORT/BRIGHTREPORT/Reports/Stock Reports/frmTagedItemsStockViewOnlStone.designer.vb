<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRMTAGEDITEMSSTOCKVIEWONLSTONE
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.pnlMain = New System.Windows.Forms.Panel()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabGeneral = New System.Windows.Forms.TabPage()
        Me.grpFiltration = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chkcmbItemName = New BrighttechPack.CheckedComboBox()
        Me.chkcmbSubItemName = New BrighttechPack.CheckedComboBox()
        Me.ChkWithSubItem = New System.Windows.Forms.CheckBox()
        Me.dateTo = New System.Windows.Forms.Label()
        Me.dtpTo_OWN = New BrighttechPack.DatePicker(Me.components)
        Me.chkdate = New System.Windows.Forms.CheckBox()
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox()
        Me.LstOrderby = New System.Windows.Forms.ListBox()
        Me.chkCmbCompany = New BrighttechPack.CheckedComboBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.dtpAsOnDate_OWN = New BrighttechPack.DatePicker(Me.components)
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cmbGroup = New System.Windows.Forms.ComboBox()
        Me.cmbCostCenter = New System.Windows.Forms.ComboBox()
        Me.cmbStockType = New System.Windows.Forms.ComboBox()
        Me.CmbGroupBy = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.chkCmbCategory = New BrighttechPack.CheckedComboBox()
        Me.chkApproval = New System.Windows.Forms.CheckBox()
        Me.txtItemCode_NUM = New System.Windows.Forms.TextBox()
        Me.cmbCounterName = New BrighttechPack.CheckedComboBox()
        Me.txtItemName = New System.Windows.Forms.TextBox()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbSubItemName = New BrighttechPack.CheckedComboBox()
        Me.cmbSize = New System.Windows.Forms.ComboBox()
        Me.chkCmbMetal = New BrighttechPack.CheckedComboBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.cmbItemType = New System.Windows.Forms.ComboBox()
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
        Me.tabGeneral.Controls.Add(Me.cmbStockType)
        Me.tabGeneral.Controls.Add(Me.CmbGroupBy)
        Me.tabGeneral.Controls.Add(Me.Label6)
        Me.tabGeneral.Controls.Add(Me.chkCmbCategory)
        Me.tabGeneral.Controls.Add(Me.chkApproval)
        Me.tabGeneral.Controls.Add(Me.txtItemCode_NUM)
        Me.tabGeneral.Controls.Add(Me.cmbCounterName)
        Me.tabGeneral.Controls.Add(Me.txtItemName)
        Me.tabGeneral.Controls.Add(Me.Label30)
        Me.tabGeneral.Controls.Add(Me.Label1)
        Me.tabGeneral.Controls.Add(Me.Label5)
        Me.tabGeneral.Controls.Add(Me.cmbSubItemName)
        Me.tabGeneral.Controls.Add(Me.cmbSize)
        Me.tabGeneral.Controls.Add(Me.chkCmbMetal)
        Me.tabGeneral.Controls.Add(Me.Label21)
        Me.tabGeneral.Controls.Add(Me.Label7)
        Me.tabGeneral.Controls.Add(Me.Label8)
        Me.tabGeneral.Controls.Add(Me.Label17)
        Me.tabGeneral.Controls.Add(Me.cmbItemType)
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
        Me.grpFiltration.Controls.Add(Me.Label3)
        Me.grpFiltration.Controls.Add(Me.Label2)
        Me.grpFiltration.Controls.Add(Me.chkcmbItemName)
        Me.grpFiltration.Controls.Add(Me.chkcmbSubItemName)
        Me.grpFiltration.Controls.Add(Me.ChkWithSubItem)
        Me.grpFiltration.Controls.Add(Me.dateTo)
        Me.grpFiltration.Controls.Add(Me.dtpTo_OWN)
        Me.grpFiltration.Controls.Add(Me.chkdate)
        Me.grpFiltration.Controls.Add(Me.chkCmbCostCentre)
        Me.grpFiltration.Controls.Add(Me.LstOrderby)
        Me.grpFiltration.Controls.Add(Me.chkCmbCompany)
        Me.grpFiltration.Controls.Add(Me.Label22)
        Me.grpFiltration.Controls.Add(Me.dtpAsOnDate_OWN)
        Me.grpFiltration.Controls.Add(Me.btnNew)
        Me.grpFiltration.Controls.Add(Me.btnExit)
        Me.grpFiltration.Controls.Add(Me.btnView_Search)
        Me.grpFiltration.Controls.Add(Me.Label9)
        Me.grpFiltration.Location = New System.Drawing.Point(272, 166)
        Me.grpFiltration.Name = "grpFiltration"
        Me.grpFiltration.Size = New System.Drawing.Size(469, 299)
        Me.grpFiltration.TabIndex = 0
        Me.grpFiltration.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(66, 83)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(97, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Sub Item Name"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(66, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(71, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Item Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkcmbItemName
        '
        Me.chkcmbItemName.CheckOnClick = True
        Me.chkcmbItemName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbItemName.DropDownHeight = 1
        Me.chkcmbItemName.FormattingEnabled = True
        Me.chkcmbItemName.IntegralHeight = False
        Me.chkcmbItemName.Location = New System.Drawing.Point(171, 52)
        Me.chkcmbItemName.Name = "chkcmbItemName"
        Me.chkcmbItemName.Size = New System.Drawing.Size(224, 22)
        Me.chkcmbItemName.TabIndex = 1
        Me.chkcmbItemName.ValueSeparator = ", "
        '
        'chkcmbSubItemName
        '
        Me.chkcmbSubItemName.CheckOnClick = True
        Me.chkcmbSubItemName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbSubItemName.DropDownHeight = 1
        Me.chkcmbSubItemName.FormattingEnabled = True
        Me.chkcmbSubItemName.IntegralHeight = False
        Me.chkcmbSubItemName.Location = New System.Drawing.Point(171, 80)
        Me.chkcmbSubItemName.Name = "chkcmbSubItemName"
        Me.chkcmbSubItemName.Size = New System.Drawing.Size(224, 22)
        Me.chkcmbSubItemName.TabIndex = 3
        Me.chkcmbSubItemName.ValueSeparator = ", "
        '
        'ChkWithSubItem
        '
        Me.ChkWithSubItem.AutoSize = True
        Me.ChkWithSubItem.Location = New System.Drawing.Point(171, 190)
        Me.ChkWithSubItem.Name = "ChkWithSubItem"
        Me.ChkWithSubItem.Size = New System.Drawing.Size(101, 17)
        Me.ChkWithSubItem.TabIndex = 12
        Me.ChkWithSubItem.Text = "Sep SubItem"
        Me.ChkWithSubItem.UseVisualStyleBackColor = True
        '
        'dateTo
        '
        Me.dateTo.AutoSize = True
        Me.dateTo.Location = New System.Drawing.Point(269, 166)
        Me.dateTo.Name = "dateTo"
        Me.dateTo.Size = New System.Drawing.Size(21, 13)
        Me.dateTo.TabIndex = 10
        Me.dateTo.Text = "To"
        Me.dateTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.dateTo.Visible = False
        '
        'dtpTo_OWN
        '
        Me.dtpTo_OWN.Location = New System.Drawing.Point(299, 163)
        Me.dtpTo_OWN.Mask = "##/##/####"
        Me.dtpTo_OWN.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo_OWN.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo_OWN.Name = "dtpTo_OWN"
        Me.dtpTo_OWN.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo_OWN.Size = New System.Drawing.Size(83, 21)
        Me.dtpTo_OWN.TabIndex = 11
        Me.dtpTo_OWN.Text = "07/03/9998"
        Me.dtpTo_OWN.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        Me.dtpTo_OWN.Visible = False
        '
        'chkdate
        '
        Me.chkdate.AutoSize = True
        Me.chkdate.Location = New System.Drawing.Point(66, 167)
        Me.chkdate.Name = "chkdate"
        Me.chkdate.Size = New System.Drawing.Size(83, 17)
        Me.chkdate.TabIndex = 8
        Me.chkdate.Text = "AsOnDate"
        Me.chkdate.UseVisualStyleBackColor = True
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(171, 108)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(224, 22)
        Me.chkCmbCostCentre.TabIndex = 5
        Me.chkCmbCostCentre.ValueSeparator = ", "
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
        Me.chkCmbCompany.Location = New System.Drawing.Point(171, 135)
        Me.chkCmbCompany.Name = "chkCmbCompany"
        Me.chkCmbCompany.Size = New System.Drawing.Size(224, 22)
        Me.chkCmbCompany.TabIndex = 7
        Me.chkCmbCompany.ValueSeparator = ", "
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(66, 140)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(62, 13)
        Me.Label22.TabIndex = 6
        Me.Label22.Text = "Company"
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpAsOnDate_OWN
        '
        Me.dtpAsOnDate_OWN.Location = New System.Drawing.Point(171, 163)
        Me.dtpAsOnDate_OWN.Mask = "##/##/####"
        Me.dtpAsOnDate_OWN.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpAsOnDate_OWN.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpAsOnDate_OWN.Name = "dtpAsOnDate_OWN"
        Me.dtpAsOnDate_OWN.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpAsOnDate_OWN.Size = New System.Drawing.Size(83, 21)
        Me.dtpAsOnDate_OWN.TabIndex = 9
        Me.dtpAsOnDate_OWN.Text = "07/03/9998"
        Me.dtpAsOnDate_OWN.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(181, 217)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 14
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(291, 217)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 15
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(68, 217)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 13
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(66, 113)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(76, 13)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "Cost Centre"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
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
        'cmbStockType
        '
        Me.cmbStockType.FormattingEnabled = True
        Me.cmbStockType.Location = New System.Drawing.Point(109, 273)
        Me.cmbStockType.Name = "cmbStockType"
        Me.cmbStockType.Size = New System.Drawing.Size(224, 21)
        Me.cmbStockType.TabIndex = 12
        Me.cmbStockType.Visible = False
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
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(16, 281)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(71, 13)
        Me.Label6.TabIndex = 29
        Me.Label6.Text = "Stock Type"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label6.Visible = False
        '
        'chkCmbCategory
        '
        Me.chkCmbCategory.CheckOnClick = True
        Me.chkCmbCategory.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCmbCategory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCategory.DropDownHeight = 1
        Me.chkCmbCategory.FormattingEnabled = True
        Me.chkCmbCategory.IntegralHeight = False
        Me.chkCmbCategory.Location = New System.Drawing.Point(106, 122)
        Me.chkCmbCategory.Name = "chkCmbCategory"
        Me.chkCmbCategory.Size = New System.Drawing.Size(224, 22)
        Me.chkCmbCategory.TabIndex = 1
        Me.chkCmbCategory.ValueSeparator = ", "
        Me.chkCmbCategory.Visible = False
        '
        'chkApproval
        '
        Me.chkApproval.AutoSize = True
        Me.chkApproval.Checked = True
        Me.chkApproval.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkApproval.Location = New System.Drawing.Point(63, 320)
        Me.chkApproval.Name = "chkApproval"
        Me.chkApproval.Size = New System.Drawing.Size(106, 17)
        Me.chkApproval.TabIndex = 13
        Me.chkApproval.Text = "With Approval"
        Me.chkApproval.UseVisualStyleBackColor = True
        Me.chkApproval.Visible = False
        '
        'txtItemCode_NUM
        '
        Me.txtItemCode_NUM.BackColor = System.Drawing.SystemColors.Window
        Me.txtItemCode_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtItemCode_NUM.Location = New System.Drawing.Point(106, 144)
        Me.txtItemCode_NUM.MaxLength = 8
        Me.txtItemCode_NUM.Name = "txtItemCode_NUM"
        Me.txtItemCode_NUM.Size = New System.Drawing.Size(93, 21)
        Me.txtItemCode_NUM.TabIndex = 2
        Me.txtItemCode_NUM.Visible = False
        '
        'cmbCounterName
        '
        Me.cmbCounterName.CheckOnClick = True
        Me.cmbCounterName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbCounterName.DropDownHeight = 1
        Me.cmbCounterName.FormattingEnabled = True
        Me.cmbCounterName.IntegralHeight = False
        Me.cmbCounterName.Location = New System.Drawing.Point(109, 185)
        Me.cmbCounterName.Name = "cmbCounterName"
        Me.cmbCounterName.Size = New System.Drawing.Size(224, 22)
        Me.cmbCounterName.TabIndex = 7
        Me.cmbCounterName.ValueSeparator = ", "
        Me.cmbCounterName.Visible = False
        '
        'txtItemName
        '
        Me.txtItemName.BackColor = System.Drawing.SystemColors.Window
        Me.txtItemName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtItemName.Enabled = False
        Me.txtItemName.Location = New System.Drawing.Point(204, 145)
        Me.txtItemName.Name = "txtItemName"
        Me.txtItemName.Size = New System.Drawing.Size(126, 21)
        Me.txtItemName.TabIndex = 3
        Me.txtItemName.Visible = False
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Location = New System.Drawing.Point(13, 127)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(60, 13)
        Me.Label30.TabIndex = 2
        Me.Label30.Text = "Category"
        Me.Label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label30.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 148)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Item Code"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label1.Visible = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(13, 170)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(93, 13)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "SubItem Name"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label5.Visible = False
        '
        'cmbSubItemName
        '
        Me.cmbSubItemName.CheckOnClick = True
        Me.cmbSubItemName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbSubItemName.DropDownHeight = 1
        Me.cmbSubItemName.FormattingEnabled = True
        Me.cmbSubItemName.IntegralHeight = False
        Me.cmbSubItemName.Location = New System.Drawing.Point(106, 165)
        Me.cmbSubItemName.Name = "cmbSubItemName"
        Me.cmbSubItemName.Size = New System.Drawing.Size(224, 22)
        Me.cmbSubItemName.TabIndex = 4
        Me.cmbSubItemName.ValueSeparator = ", "
        Me.cmbSubItemName.Visible = False
        '
        'cmbSize
        '
        Me.cmbSize.FormattingEnabled = True
        Me.cmbSize.Location = New System.Drawing.Point(234, 207)
        Me.cmbSize.Name = "cmbSize"
        Me.cmbSize.Size = New System.Drawing.Size(99, 21)
        Me.cmbSize.TabIndex = 9
        Me.cmbSize.Visible = False
        '
        'chkCmbMetal
        '
        Me.chkCmbMetal.CheckOnClick = True
        Me.chkCmbMetal.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkCmbMetal.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbMetal.DropDownHeight = 1
        Me.chkCmbMetal.FormattingEnabled = True
        Me.chkCmbMetal.IntegralHeight = False
        Me.chkCmbMetal.Location = New System.Drawing.Point(106, 101)
        Me.chkCmbMetal.Name = "chkCmbMetal"
        Me.chkCmbMetal.Size = New System.Drawing.Size(224, 22)
        Me.chkCmbMetal.TabIndex = 0
        Me.chkCmbMetal.ValueSeparator = ", "
        Me.chkCmbMetal.Visible = False
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(13, 105)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(37, 13)
        Me.Label21.TabIndex = 0
        Me.Label21.Text = "Metal"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label21.Visible = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(16, 190)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(90, 13)
        Me.Label7.TabIndex = 19
        Me.Label7.Text = "Counter Name"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label7.Visible = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(16, 211)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(66, 13)
        Me.Label8.TabIndex = 21
        Me.Label8.Text = "Item Type"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label8.Visible = False
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(202, 211)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(31, 13)
        Me.Label17.TabIndex = 23
        Me.Label17.Text = "Size"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label17.Visible = False
        '
        'cmbItemType
        '
        Me.cmbItemType.FormattingEnabled = True
        Me.cmbItemType.Location = New System.Drawing.Point(109, 207)
        Me.cmbItemType.Name = "cmbItemType"
        Me.cmbItemType.Size = New System.Drawing.Size(90, 21)
        Me.cmbItemType.TabIndex = 8
        Me.cmbItemType.Visible = False
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
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.gridTotalView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
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
        'FRMTAGEDITEMSSTOCKVIEWONLSTONE
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
        Me.Name = "FRMTAGEDITEMSSTOCKVIEWONLSTONE"
        Me.Text = "Item Stock View"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlMain.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.tabGeneral.PerformLayout()
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
    Friend WithEvents dtpAsOnDate_OWN As BrighttechPack.DatePicker
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
    Friend WithEvents dtpTo_OWN As BrighttechPack.DatePicker
    Friend WithEvents dateTo As System.Windows.Forms.Label
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChkWithSubItem As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkcmbItemName As BrighttechPack.CheckedComboBox
    Friend WithEvents chkcmbSubItemName As BrighttechPack.CheckedComboBox

End Class
