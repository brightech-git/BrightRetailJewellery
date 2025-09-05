<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmTransferSumCostCentreWise
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GrpContainer = New System.Windows.Forms.GroupBox()
        Me.chktCostCentreSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstttCostCentre = New System.Windows.Forms.CheckedListBox()
        Me.txtCCostid = New System.Windows.Forms.TextBox()
        Me.txtCorpCCName = New System.Windows.Forms.TextBox()
        Me.cmbFromCostCenter = New System.Windows.Forms.ComboBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.cmbtoCostCenter = New System.Windows.Forms.ComboBox()
        Me.chkCounterWiseGroup = New System.Windows.Forms.CheckBox()
        Me.cmbDesigner = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cmbCategory = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.rbtTransfer = New System.Windows.Forms.RadioButton()
        Me.rbtLot = New System.Windows.Forms.RadioButton()
        Me.rbtAll = New System.Windows.Forms.RadioButton()
        Me.ChkSummary = New System.Windows.Forms.CheckBox()
        Me.ChkVaDetail = New System.Windows.Forms.CheckBox()
        Me.txtrefNo = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbMetal = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ChkActDate = New System.Windows.Forms.CheckBox()
        Me.CmbItemCounter = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.chkFCostCentreSelectAll = New System.Windows.Forms.CheckBox()
        Me.cmbOrderBy = New System.Windows.Forms.ComboBox()
        Me.chkItemWiseGroup = New System.Windows.Forms.CheckBox()
        Me.rbtReceipt = New System.Windows.Forms.RadioButton()
        Me.rbtIssue = New System.Windows.Forms.RadioButton()
        Me.chkCostCentreSelectAll = New System.Windows.Forms.CheckBox()
        Me.cmbItemName = New System.Windows.Forms.ComboBox()
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkAsOnDate = New System.Windows.Forms.CheckBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.GrpContainer.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.NewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(119, 48)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'GrpContainer
        '
        Me.GrpContainer.ContextMenuStrip = Me.ContextMenuStrip1
        Me.GrpContainer.Controls.Add(Me.chktCostCentreSelectAll)
        Me.GrpContainer.Controls.Add(Me.chkLstttCostCentre)
        Me.GrpContainer.Controls.Add(Me.txtCCostid)
        Me.GrpContainer.Controls.Add(Me.txtCorpCCName)
        Me.GrpContainer.Controls.Add(Me.cmbFromCostCenter)
        Me.GrpContainer.Controls.Add(Me.Label11)
        Me.GrpContainer.Controls.Add(Me.cmbtoCostCenter)
        Me.GrpContainer.Controls.Add(Me.chkCounterWiseGroup)
        Me.GrpContainer.Controls.Add(Me.cmbDesigner)
        Me.GrpContainer.Controls.Add(Me.Label9)
        Me.GrpContainer.Controls.Add(Me.cmbCategory)
        Me.GrpContainer.Controls.Add(Me.Label8)
        Me.GrpContainer.Controls.Add(Me.Label7)
        Me.GrpContainer.Controls.Add(Me.Panel1)
        Me.GrpContainer.Controls.Add(Me.ChkSummary)
        Me.GrpContainer.Controls.Add(Me.ChkVaDetail)
        Me.GrpContainer.Controls.Add(Me.txtrefNo)
        Me.GrpContainer.Controls.Add(Me.Label6)
        Me.GrpContainer.Controls.Add(Me.cmbMetal)
        Me.GrpContainer.Controls.Add(Me.Label5)
        Me.GrpContainer.Controls.Add(Me.ChkActDate)
        Me.GrpContainer.Controls.Add(Me.CmbItemCounter)
        Me.GrpContainer.Controls.Add(Me.Label4)
        Me.GrpContainer.Controls.Add(Me.chkFCostCentreSelectAll)
        Me.GrpContainer.Controls.Add(Me.cmbOrderBy)
        Me.GrpContainer.Controls.Add(Me.chkItemWiseGroup)
        Me.GrpContainer.Controls.Add(Me.rbtReceipt)
        Me.GrpContainer.Controls.Add(Me.rbtIssue)
        Me.GrpContainer.Controls.Add(Me.chkCostCentreSelectAll)
        Me.GrpContainer.Controls.Add(Me.cmbItemName)
        Me.GrpContainer.Controls.Add(Me.chkLstCostCentre)
        Me.GrpContainer.Controls.Add(Me.Label3)
        Me.GrpContainer.Controls.Add(Me.Label2)
        Me.GrpContainer.Controls.Add(Me.Label1)
        Me.GrpContainer.Controls.Add(Me.chkAsOnDate)
        Me.GrpContainer.Controls.Add(Me.dtpTo)
        Me.GrpContainer.Controls.Add(Me.dtpFrom)
        Me.GrpContainer.Controls.Add(Me.btnExit)
        Me.GrpContainer.Controls.Add(Me.btnSearch)
        Me.GrpContainer.Controls.Add(Me.btnNew)
        Me.GrpContainer.Location = New System.Drawing.Point(175, 227)
        Me.GrpContainer.Name = "GrpContainer"
        Me.GrpContainer.Size = New System.Drawing.Size(344, 261)
        Me.GrpContainer.TabIndex = 0
        Me.GrpContainer.TabStop = False
        '
        'chktCostCentreSelectAll
        '
        Me.chktCostCentreSelectAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chktCostCentreSelectAll.Location = New System.Drawing.Point(6, 84)
        Me.chktCostCentreSelectAll.Name = "chktCostCentreSelectAll"
        Me.chktCostCentreSelectAll.Size = New System.Drawing.Size(122, 17)
        Me.chktCostCentreSelectAll.TabIndex = 40
        Me.chktCostCentreSelectAll.Text = "To Cost Centre"
        Me.chktCostCentreSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstttCostCentre
        '
        Me.chkLstttCostCentre.FormattingEnabled = True
        Me.chkLstttCostCentre.Location = New System.Drawing.Point(133, 84)
        Me.chkLstttCostCentre.Name = "chkLstttCostCentre"
        Me.chkLstttCostCentre.Size = New System.Drawing.Size(201, 100)
        Me.chkLstttCostCentre.TabIndex = 41
        '
        'txtCCostid
        '
        Me.txtCCostid.Enabled = False
        Me.txtCCostid.Location = New System.Drawing.Point(294, 55)
        Me.txtCCostid.Name = "txtCCostid"
        Me.txtCCostid.Size = New System.Drawing.Size(36, 21)
        Me.txtCCostid.TabIndex = 39
        '
        'txtCorpCCName
        '
        Me.txtCorpCCName.Enabled = False
        Me.txtCorpCCName.Location = New System.Drawing.Point(132, 55)
        Me.txtCorpCCName.Name = "txtCorpCCName"
        Me.txtCorpCCName.Size = New System.Drawing.Size(155, 21)
        Me.txtCorpCCName.TabIndex = 38
        '
        'cmbFromCostCenter
        '
        Me.cmbFromCostCenter.FormattingEnabled = True
        Me.cmbFromCostCenter.Location = New System.Drawing.Point(122, 271)
        Me.cmbFromCostCenter.Name = "cmbFromCostCenter"
        Me.cmbFromCostCenter.Size = New System.Drawing.Size(202, 21)
        Me.cmbFromCostCenter.TabIndex = 17
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(9, 58)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(105, 13)
        Me.Label11.TabIndex = 16
        Me.Label11.Text = "From CostCenter"
        '
        'cmbtoCostCenter
        '
        Me.cmbtoCostCenter.FormattingEnabled = True
        Me.cmbtoCostCenter.Location = New System.Drawing.Point(138, 366)
        Me.cmbtoCostCenter.Name = "cmbtoCostCenter"
        Me.cmbtoCostCenter.Size = New System.Drawing.Size(202, 21)
        Me.cmbtoCostCenter.TabIndex = 19
        '
        'chkCounterWiseGroup
        '
        Me.chkCounterWiseGroup.AutoSize = True
        Me.chkCounterWiseGroup.Location = New System.Drawing.Point(210, 408)
        Me.chkCounterWiseGroup.Name = "chkCounterWiseGroup"
        Me.chkCounterWiseGroup.Size = New System.Drawing.Size(142, 17)
        Me.chkCounterWiseGroup.TabIndex = 27
        Me.chkCounterWiseGroup.Text = "Counter Wise Group"
        Me.chkCounterWiseGroup.UseVisualStyleBackColor = True
        '
        'cmbDesigner
        '
        Me.cmbDesigner.FormattingEnabled = True
        Me.cmbDesigner.Location = New System.Drawing.Point(120, 410)
        Me.cmbDesigner.Name = "cmbDesigner"
        Me.cmbDesigner.Size = New System.Drawing.Size(202, 21)
        Me.cmbDesigner.TabIndex = 15
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(10, 414)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(95, 13)
        Me.Label9.TabIndex = 14
        Me.Label9.Text = "Designer"
        '
        'cmbCategory
        '
        Me.cmbCategory.FormattingEnabled = True
        Me.cmbCategory.Location = New System.Drawing.Point(127, 261)
        Me.cmbCategory.Name = "cmbCategory"
        Me.cmbCategory.Size = New System.Drawing.Size(202, 21)
        Me.cmbCategory.TabIndex = 9
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(10, 348)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(95, 13)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "Category"
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(174, 412)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(95, 21)
        Me.Label7.TabIndex = 28
        Me.Label7.Text = "Type"
        Me.Label7.Visible = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbtTransfer)
        Me.Panel1.Controls.Add(Me.rbtLot)
        Me.Panel1.Controls.Add(Me.rbtAll)
        Me.Panel1.Location = New System.Drawing.Point(275, 407)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(201, 21)
        Me.Panel1.TabIndex = 29
        Me.Panel1.Visible = False
        '
        'rbtTransfer
        '
        Me.rbtTransfer.AutoSize = True
        Me.rbtTransfer.Location = New System.Drawing.Point(119, 3)
        Me.rbtTransfer.Name = "rbtTransfer"
        Me.rbtTransfer.Size = New System.Drawing.Size(72, 17)
        Me.rbtTransfer.TabIndex = 2
        Me.rbtTransfer.Text = "Transfer"
        Me.rbtTransfer.UseVisualStyleBackColor = True
        '
        'rbtLot
        '
        Me.rbtLot.AutoSize = True
        Me.rbtLot.Location = New System.Drawing.Point(62, 3)
        Me.rbtLot.Name = "rbtLot"
        Me.rbtLot.Size = New System.Drawing.Size(42, 17)
        Me.rbtLot.TabIndex = 1
        Me.rbtLot.Text = "Lot"
        Me.rbtLot.UseVisualStyleBackColor = True
        '
        'rbtAll
        '
        Me.rbtAll.AutoSize = True
        Me.rbtAll.Checked = True
        Me.rbtAll.Location = New System.Drawing.Point(3, 3)
        Me.rbtAll.Name = "rbtAll"
        Me.rbtAll.Size = New System.Drawing.Size(39, 17)
        Me.rbtAll.TabIndex = 0
        Me.rbtAll.TabStop = True
        Me.rbtAll.Text = "All"
        Me.rbtAll.UseVisualStyleBackColor = True
        '
        'ChkSummary
        '
        Me.ChkSummary.AutoSize = True
        Me.ChkSummary.Location = New System.Drawing.Point(7, 407)
        Me.ChkSummary.Name = "ChkSummary"
        Me.ChkSummary.Size = New System.Drawing.Size(82, 17)
        Me.ChkSummary.TabIndex = 25
        Me.ChkSummary.Text = "Summary"
        Me.ChkSummary.UseVisualStyleBackColor = True
        Me.ChkSummary.Visible = False
        '
        'ChkVaDetail
        '
        Me.ChkVaDetail.AutoSize = True
        Me.ChkVaDetail.Location = New System.Drawing.Point(364, 436)
        Me.ChkVaDetail.Name = "ChkVaDetail"
        Me.ChkVaDetail.Size = New System.Drawing.Size(125, 17)
        Me.ChkVaDetail.TabIndex = 37
        Me.ChkVaDetail.Text = "Include VA Detail"
        Me.ChkVaDetail.UseVisualStyleBackColor = True
        Me.ChkVaDetail.Visible = False
        '
        'txtrefNo
        '
        Me.txtrefNo.Location = New System.Drawing.Point(127, 361)
        Me.txtrefNo.Name = "txtrefNo"
        Me.txtrefNo.Size = New System.Drawing.Size(124, 21)
        Me.txtrefNo.TabIndex = 21
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(13, 388)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(96, 13)
        Me.Label6.TabIndex = 20
        Me.Label6.Text = "Ref No"
        '
        'cmbMetal
        '
        Me.cmbMetal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(120, 288)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(202, 21)
        Me.cmbMetal.TabIndex = 7
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(10, 326)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(95, 13)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Metal"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ChkActDate
        '
        Me.ChkActDate.AutoSize = True
        Me.ChkActDate.Location = New System.Drawing.Point(161, 436)
        Me.ChkActDate.Name = "ChkActDate"
        Me.ChkActDate.Size = New System.Drawing.Size(170, 17)
        Me.ChkActDate.TabIndex = 36
        Me.ChkActDate.Text = "Based on Actual RecDate"
        Me.ChkActDate.UseVisualStyleBackColor = True
        Me.ChkActDate.Visible = False
        '
        'CmbItemCounter
        '
        Me.CmbItemCounter.FormattingEnabled = True
        Me.CmbItemCounter.Location = New System.Drawing.Point(147, 361)
        Me.CmbItemCounter.Name = "CmbItemCounter"
        Me.CmbItemCounter.Size = New System.Drawing.Size(202, 21)
        Me.CmbItemCounter.TabIndex = 13
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(10, 392)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(95, 13)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "Item Counter"
        '
        'chkFCostCentreSelectAll
        '
        Me.chkFCostCentreSelectAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkFCostCentreSelectAll.Location = New System.Drawing.Point(39, 335)
        Me.chkFCostCentreSelectAll.Name = "chkFCostCentreSelectAll"
        Me.chkFCostCentreSelectAll.Size = New System.Drawing.Size(98, 21)
        Me.chkFCostCentreSelectAll.TabIndex = 30
        Me.chkFCostCentreSelectAll.Text = "From Centre"
        Me.chkFCostCentreSelectAll.UseVisualStyleBackColor = True
        Me.chkFCostCentreSelectAll.Visible = False
        '
        'cmbOrderBy
        '
        Me.cmbOrderBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOrderBy.FormattingEnabled = True
        Me.cmbOrderBy.Location = New System.Drawing.Point(275, 339)
        Me.cmbOrderBy.Name = "cmbOrderBy"
        Me.cmbOrderBy.Size = New System.Drawing.Size(129, 21)
        Me.cmbOrderBy.TabIndex = 33
        Me.cmbOrderBy.Visible = False
        '
        'chkItemWiseGroup
        '
        Me.chkItemWiseGroup.AutoSize = True
        Me.chkItemWiseGroup.Location = New System.Drawing.Point(90, 407)
        Me.chkItemWiseGroup.Name = "chkItemWiseGroup"
        Me.chkItemWiseGroup.Size = New System.Drawing.Size(123, 17)
        Me.chkItemWiseGroup.TabIndex = 26
        Me.chkItemWiseGroup.Text = "Item Wise Group"
        Me.chkItemWiseGroup.UseVisualStyleBackColor = True
        '
        'rbtReceipt
        '
        Me.rbtReceipt.AutoSize = True
        Me.rbtReceipt.Location = New System.Drawing.Point(18, 301)
        Me.rbtReceipt.Name = "rbtReceipt"
        Me.rbtReceipt.Size = New System.Drawing.Size(67, 17)
        Me.rbtReceipt.TabIndex = 5
        Me.rbtReceipt.Text = "Receipt"
        Me.rbtReceipt.UseVisualStyleBackColor = True
        '
        'rbtIssue
        '
        Me.rbtIssue.AutoSize = True
        Me.rbtIssue.Checked = True
        Me.rbtIssue.Location = New System.Drawing.Point(27, 369)
        Me.rbtIssue.Name = "rbtIssue"
        Me.rbtIssue.Size = New System.Drawing.Size(56, 17)
        Me.rbtIssue.TabIndex = 4
        Me.rbtIssue.TabStop = True
        Me.rbtIssue.Text = "Issue"
        Me.rbtIssue.UseVisualStyleBackColor = True
        '
        'chkCostCentreSelectAll
        '
        Me.chkCostCentreSelectAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkCostCentreSelectAll.Location = New System.Drawing.Point(183, 312)
        Me.chkCostCentreSelectAll.Name = "chkCostCentreSelectAll"
        Me.chkCostCentreSelectAll.Size = New System.Drawing.Size(98, 21)
        Me.chkCostCentreSelectAll.TabIndex = 35
        Me.chkCostCentreSelectAll.Text = "Cost Centre"
        Me.chkCostCentreSelectAll.UseVisualStyleBackColor = True
        Me.chkCostCentreSelectAll.Visible = False
        '
        'cmbItemName
        '
        Me.cmbItemName.FormattingEnabled = True
        Me.cmbItemName.Location = New System.Drawing.Point(16, 288)
        Me.cmbItemName.Name = "cmbItemName"
        Me.cmbItemName.Size = New System.Drawing.Size(202, 21)
        Me.cmbItemName.TabIndex = 11
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(49, 270)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(201, 20)
        Me.chkLstCostCentre.TabIndex = 34
        Me.chkLstCostCentre.Visible = False
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(171, 342)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(95, 21)
        Me.Label3.TabIndex = 31
        Me.Label3.Text = "Order By"
        Me.Label3.Visible = False
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(10, 370)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(95, 13)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Item"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(221, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(20, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "To"
        '
        'chkAsOnDate
        '
        Me.chkAsOnDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAsOnDate.Location = New System.Drawing.Point(6, 23)
        Me.chkAsOnDate.Name = "chkAsOnDate"
        Me.chkAsOnDate.Size = New System.Drawing.Size(120, 17)
        Me.chkAsOnDate.TabIndex = 0
        Me.chkAsOnDate.Text = "Date From"
        Me.chkAsOnDate.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(248, 21)
        Me.dtpTo.Mask = "##-##-####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(82, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "29/09/2010"
        Me.dtpTo.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(132, 21)
        Me.dtpFrom.Mask = "##-##-####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(82, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "29/09/2010"
        Me.dtpFrom.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(235, 204)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 24
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(23, 204)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 22
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(129, 204)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 23
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'frmTransferSumCostCentreWise
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(671, 595)
        Me.Controls.Add(Me.GrpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmTransferSumCostCentreWise"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = " TRANSFER ISSUE SUMMARY COSTCENTRE WISE"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.GrpContainer.ResumeLayout(False)
        Me.GrpContainer.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GrpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents chkCostCentreSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents cmbItemName As System.Windows.Forms.ComboBox
    Friend WithEvents chkLstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkAsOnDate As System.Windows.Forms.CheckBox
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents rbtReceipt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtIssue As System.Windows.Forms.RadioButton
    Friend WithEvents chkItemWiseGroup As System.Windows.Forms.CheckBox
    Friend WithEvents cmbOrderBy As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents chkFCostCentreSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents CmbItemCounter As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ChkActDate As System.Windows.Forms.CheckBox
    Friend WithEvents cmbMetal As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtrefNo As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ChkVaDetail As System.Windows.Forms.CheckBox
    Friend WithEvents ChkSummary As CheckBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents rbtTransfer As RadioButton
    Friend WithEvents rbtLot As RadioButton
    Friend WithEvents rbtAll As RadioButton
    Friend WithEvents cmbCategory As ComboBox
    Friend WithEvents Label8 As Label
    Friend WithEvents cmbDesigner As ComboBox
    Friend WithEvents Label9 As Label
    Friend WithEvents chkCounterWiseGroup As CheckBox
    Friend WithEvents cmbtoCostCenter As ComboBox
    Friend WithEvents cmbFromCostCenter As ComboBox
    Friend WithEvents Label11 As Label
    Friend WithEvents txtCorpCCName As TextBox
    Friend WithEvents txtCCostid As TextBox
    Friend WithEvents chktCostCentreSelectAll As CheckBox
    Friend WithEvents chkLstttCostCentre As CheckedListBox
End Class
