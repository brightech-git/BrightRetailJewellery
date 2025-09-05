<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmApprovalIssRecPen
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
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.cmbItem = New System.Windows.Forms.ComboBox()
        Me.cmbMetal = New System.Windows.Forms.ComboBox()
        Me.rbtReceipt = New System.Windows.Forms.RadioButton()
        Me.rbtIssue = New System.Windows.Forms.RadioButton()
        Me.lblDateTo = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlGrid = New System.Windows.Forms.Panel()
        Me.pnlTitle = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.rbtPending = New System.Windows.Forms.RadioButton()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabGen = New System.Windows.Forms.TabPage()
        Me.grpControls = New System.Windows.Forms.GroupBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cmbDesigner = New System.Windows.Forms.ComboBox()
        Me.chkCmbNodeId = New BrighttechPack.CheckedComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cmbItemCounter_OWN = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtItemId_NUM = New System.Windows.Forms.TextBox()
        Me.ChkCmbCompany = New BrighttechPack.CheckedComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ChkImage = New System.Windows.Forms.CheckBox()
        Me.chkAsOnDate = New System.Windows.Forms.CheckBox()
        Me.txtPartyname = New System.Windows.Forms.ComboBox()
        Me.cmbOrderBy = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label = New System.Windows.Forms.Label()
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox()
        Me.txtApprovalNo = New System.Windows.Forms.TextBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Label6 = New System.Windows.Forms.Label()
        Me.tabView = New System.Windows.Forms.TabPage()
        Me.pnlFooter = New System.Windows.Forms.Panel()
        Me.btnBack = New System.Windows.Forms.Button()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.pnlGrid.SuspendLayout()
        Me.pnlTitle.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGen.SuspendLayout()
        Me.grpControls.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.pnlFooter.SuspendLayout()
        Me.SuspendLayout()
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 0)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(1004, 544)
        Me.gridView.TabIndex = 17
        '
        'cmbItem
        '
        Me.cmbItem.FormattingEnabled = True
        Me.cmbItem.Location = New System.Drawing.Point(124, 161)
        Me.cmbItem.Name = "cmbItem"
        Me.cmbItem.Size = New System.Drawing.Size(194, 21)
        Me.cmbItem.TabIndex = 15
        '
        'cmbMetal
        '
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(91, 133)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(227, 21)
        Me.cmbMetal.TabIndex = 12
        '
        'rbtReceipt
        '
        Me.rbtReceipt.AutoSize = True
        Me.rbtReceipt.Location = New System.Drawing.Point(167, 20)
        Me.rbtReceipt.Name = "rbtReceipt"
        Me.rbtReceipt.Size = New System.Drawing.Size(67, 17)
        Me.rbtReceipt.TabIndex = 1
        Me.rbtReceipt.TabStop = True
        Me.rbtReceipt.Text = "&Receipt"
        Me.rbtReceipt.UseVisualStyleBackColor = True
        '
        'rbtIssue
        '
        Me.rbtIssue.AutoSize = True
        Me.rbtIssue.Location = New System.Drawing.Point(236, 20)
        Me.rbtIssue.Name = "rbtIssue"
        Me.rbtIssue.Size = New System.Drawing.Size(56, 17)
        Me.rbtIssue.TabIndex = 2
        Me.rbtIssue.TabStop = True
        Me.rbtIssue.Text = "&Issue"
        Me.rbtIssue.UseVisualStyleBackColor = True
        '
        'lblDateTo
        '
        Me.lblDateTo.AutoSize = True
        Me.lblDateTo.Location = New System.Drawing.Point(202, 51)
        Me.lblDateTo.Name = "lblDateTo"
        Me.lblDateTo.Size = New System.Drawing.Size(20, 13)
        Me.lblDateTo.TabIndex = 5
        Me.lblDateTo.Text = "To"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 165)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(34, 13)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Item"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 137)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(37, 13)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Metal"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 278)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(77, 13)
        Me.Label5.TabIndex = 22
        Me.Label5.Text = "Approval No"
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(11, 382)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 29
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(117, 382)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 30
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(223, 382)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 31
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(634, 3)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 15
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(750, 3)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 16
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
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
        'pnlGrid
        '
        Me.pnlGrid.Controls.Add(Me.gridView)
        Me.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlGrid.Location = New System.Drawing.Point(3, 27)
        Me.pnlGrid.Name = "pnlGrid"
        Me.pnlGrid.Size = New System.Drawing.Size(1004, 544)
        Me.pnlGrid.TabIndex = 18
        '
        'pnlTitle
        '
        Me.pnlTitle.Controls.Add(Me.lblTitle)
        Me.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTitle.Location = New System.Drawing.Point(3, 3)
        Me.pnlTitle.Name = "pnlTitle"
        Me.pnlTitle.Size = New System.Drawing.Size(1004, 24)
        Me.pnlTitle.TabIndex = 18
        '
        'lblTitle
        '
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1004, 24)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'rbtPending
        '
        Me.rbtPending.AutoSize = True
        Me.rbtPending.Location = New System.Drawing.Point(91, 20)
        Me.rbtPending.Name = "rbtPending"
        Me.rbtPending.Size = New System.Drawing.Size(70, 17)
        Me.rbtPending.TabIndex = 0
        Me.rbtPending.TabStop = True
        Me.rbtPending.Text = "&Pending"
        Me.rbtPending.UseVisualStyleBackColor = True
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGen)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1018, 635)
        Me.tabMain.TabIndex = 0
        '
        'tabGen
        '
        Me.tabGen.Controls.Add(Me.grpControls)
        Me.tabGen.Location = New System.Drawing.Point(4, 22)
        Me.tabGen.Name = "tabGen"
        Me.tabGen.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGen.Size = New System.Drawing.Size(1010, 609)
        Me.tabGen.TabIndex = 0
        Me.tabGen.Text = "Gen"
        Me.tabGen.UseVisualStyleBackColor = True
        '
        'grpControls
        '
        Me.grpControls.Controls.Add(Me.Label9)
        Me.grpControls.Controls.Add(Me.cmbDesigner)
        Me.grpControls.Controls.Add(Me.chkCmbNodeId)
        Me.grpControls.Controls.Add(Me.Label8)
        Me.grpControls.Controls.Add(Me.cmbItemCounter_OWN)
        Me.grpControls.Controls.Add(Me.Label2)
        Me.grpControls.Controls.Add(Me.txtItemId_NUM)
        Me.grpControls.Controls.Add(Me.ChkCmbCompany)
        Me.grpControls.Controls.Add(Me.Label1)
        Me.grpControls.Controls.Add(Me.ChkImage)
        Me.grpControls.Controls.Add(Me.chkAsOnDate)
        Me.grpControls.Controls.Add(Me.txtPartyname)
        Me.grpControls.Controls.Add(Me.cmbOrderBy)
        Me.grpControls.Controls.Add(Me.rbtPending)
        Me.grpControls.Controls.Add(Me.rbtReceipt)
        Me.grpControls.Controls.Add(Me.Label7)
        Me.grpControls.Controls.Add(Me.rbtIssue)
        Me.grpControls.Controls.Add(Me.Label)
        Me.grpControls.Controls.Add(Me.chkCmbCostCentre)
        Me.grpControls.Controls.Add(Me.txtApprovalNo)
        Me.grpControls.Controls.Add(Me.dtpTo)
        Me.grpControls.Controls.Add(Me.Label5)
        Me.grpControls.Controls.Add(Me.dtpFrom)
        Me.grpControls.Controls.Add(Me.btnNew)
        Me.grpControls.Controls.Add(Me.btnView_Search)
        Me.grpControls.Controls.Add(Me.btnExit)
        Me.grpControls.Controls.Add(Me.Label4)
        Me.grpControls.Controls.Add(Me.cmbItem)
        Me.grpControls.Controls.Add(Me.cmbMetal)
        Me.grpControls.Controls.Add(Me.Label6)
        Me.grpControls.Controls.Add(Me.lblDateTo)
        Me.grpControls.Controls.Add(Me.Label3)
        Me.grpControls.Location = New System.Drawing.Point(320, 124)
        Me.grpControls.Name = "grpControls"
        Me.grpControls.Size = New System.Drawing.Size(331, 433)
        Me.grpControls.TabIndex = 0
        Me.grpControls.TabStop = False
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(12, 305)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(58, 13)
        Me.Label9.TabIndex = 24
        Me.Label9.Text = "Designer"
        '
        'cmbDesigner
        '
        Me.cmbDesigner.FormattingEnabled = True
        Me.cmbDesigner.Location = New System.Drawing.Point(91, 302)
        Me.cmbDesigner.Name = "cmbDesigner"
        Me.cmbDesigner.Size = New System.Drawing.Size(227, 21)
        Me.cmbDesigner.TabIndex = 25
        '
        'chkCmbNodeId
        '
        Me.chkCmbNodeId.CheckOnClick = True
        Me.chkCmbNodeId.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbNodeId.DropDownHeight = 1
        Me.chkCmbNodeId.FormattingEnabled = True
        Me.chkCmbNodeId.IntegralHeight = False
        Me.chkCmbNodeId.Location = New System.Drawing.Point(91, 245)
        Me.chkCmbNodeId.Name = "chkCmbNodeId"
        Me.chkCmbNodeId.Size = New System.Drawing.Size(227, 22)
        Me.chkCmbNodeId.TabIndex = 21
        Me.chkCmbNodeId.ValueSeparator = ", "
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(12, 250)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(52, 13)
        Me.Label8.TabIndex = 20
        Me.Label8.Text = "Node Id"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbItemCounter_OWN
        '
        Me.cmbItemCounter_OWN.FormattingEnabled = True
        Me.cmbItemCounter_OWN.Location = New System.Drawing.Point(91, 189)
        Me.cmbItemCounter_OWN.Name = "cmbItemCounter_OWN"
        Me.cmbItemCounter_OWN.Size = New System.Drawing.Size(227, 21)
        Me.cmbItemCounter_OWN.TabIndex = 17
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 193)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 13)
        Me.Label2.TabIndex = 16
        Me.Label2.Text = "Counter"
        '
        'txtItemId_NUM
        '
        Me.txtItemId_NUM.Location = New System.Drawing.Point(91, 161)
        Me.txtItemId_NUM.Name = "txtItemId_NUM"
        Me.txtItemId_NUM.Size = New System.Drawing.Size(29, 21)
        Me.txtItemId_NUM.TabIndex = 14
        '
        'ChkCmbCompany
        '
        Me.ChkCmbCompany.CheckOnClick = True
        Me.ChkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.ChkCmbCompany.DropDownHeight = 1
        Me.ChkCmbCompany.FormattingEnabled = True
        Me.ChkCmbCompany.IntegralHeight = False
        Me.ChkCmbCompany.Location = New System.Drawing.Point(91, 75)
        Me.ChkCmbCompany.Name = "ChkCmbCompany"
        Me.ChkCmbCompany.Size = New System.Drawing.Size(227, 22)
        Me.ChkCmbCompany.TabIndex = 8
        Me.ChkCmbCompany.ValueSeparator = ", "
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 80)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(62, 13)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Company"
        '
        'ChkImage
        '
        Me.ChkImage.AutoSize = True
        Me.ChkImage.Location = New System.Drawing.Point(91, 355)
        Me.ChkImage.Name = "ChkImage"
        Me.ChkImage.Size = New System.Drawing.Size(92, 17)
        Me.ChkImage.TabIndex = 28
        Me.ChkImage.Text = "With Image"
        Me.ChkImage.UseVisualStyleBackColor = True
        '
        'chkAsOnDate
        '
        Me.chkAsOnDate.AutoSize = True
        Me.chkAsOnDate.Checked = True
        Me.chkAsOnDate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAsOnDate.Location = New System.Drawing.Point(12, 49)
        Me.chkAsOnDate.Name = "chkAsOnDate"
        Me.chkAsOnDate.Size = New System.Drawing.Size(91, 17)
        Me.chkAsOnDate.TabIndex = 3
        Me.chkAsOnDate.Text = "As On Date"
        Me.chkAsOnDate.UseVisualStyleBackColor = True
        '
        'txtPartyname
        '
        Me.txtPartyname.FormattingEnabled = True
        Me.txtPartyname.Location = New System.Drawing.Point(91, 217)
        Me.txtPartyname.Name = "txtPartyname"
        Me.txtPartyname.Size = New System.Drawing.Size(227, 21)
        Me.txtPartyname.TabIndex = 19
        '
        'cmbOrderBy
        '
        Me.cmbOrderBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOrderBy.FormattingEnabled = True
        Me.cmbOrderBy.Location = New System.Drawing.Point(91, 330)
        Me.cmbOrderBy.Name = "cmbOrderBy"
        Me.cmbOrderBy.Size = New System.Drawing.Size(227, 21)
        Me.cmbOrderBy.TabIndex = 27
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(12, 334)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(61, 13)
        Me.Label7.TabIndex = 26
        Me.Label7.Text = "Group By"
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(12, 109)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(76, 13)
        Me.Label.TabIndex = 9
        Me.Label.Text = "Cost Centre"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(91, 104)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(227, 22)
        Me.chkCmbCostCentre.TabIndex = 10
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'txtApprovalNo
        '
        Me.txtApprovalNo.Location = New System.Drawing.Point(91, 274)
        Me.txtApprovalNo.Name = "txtApprovalNo"
        Me.txtApprovalNo.Size = New System.Drawing.Size(93, 21)
        Me.txtApprovalNo.TabIndex = 23
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(238, 47)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(80, 21)
        Me.dtpTo.TabIndex = 6
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(107, 47)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(81, 21)
        Me.dtpFrom.TabIndex = 4
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 221)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(74, 13)
        Me.Label6.TabIndex = 18
        Me.Label6.Text = "Party Name"
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.pnlGrid)
        Me.tabView.Controls.Add(Me.pnlFooter)
        Me.tabView.Controls.Add(Me.pnlTitle)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1010, 609)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'pnlFooter
        '
        Me.pnlFooter.Controls.Add(Me.btnBack)
        Me.pnlFooter.Controls.Add(Me.btnExport)
        Me.pnlFooter.Controls.Add(Me.btnPrint)
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Location = New System.Drawing.Point(3, 571)
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Size = New System.Drawing.Size(1004, 35)
        Me.pnlFooter.TabIndex = 0
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(518, 3)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 17
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'frmApprovalIssRecPen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1018, 635)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmApprovalIssRecPen"
        Me.Text = "Approval"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.pnlGrid.ResumeLayout(False)
        Me.pnlTitle.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGen.ResumeLayout(False)
        Me.grpControls.ResumeLayout(False)
        Me.grpControls.PerformLayout()
        Me.tabView.ResumeLayout(False)
        Me.pnlFooter.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents cmbItem As System.Windows.Forms.ComboBox
    Friend WithEvents cmbMetal As System.Windows.Forms.ComboBox
    Friend WithEvents rbtReceipt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtIssue As System.Windows.Forms.RadioButton
    Friend WithEvents lblDateTo As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlGrid As System.Windows.Forms.Panel
    Friend WithEvents pnlTitle As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents rbtPending As System.Windows.Forms.RadioButton
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGen As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents grpControls As System.Windows.Forms.GroupBox
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents txtApprovalNo As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Label As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cmbOrderBy As System.Windows.Forms.ComboBox
    Friend WithEvents txtPartyname As System.Windows.Forms.ComboBox
    Friend WithEvents chkAsOnDate As System.Windows.Forms.CheckBox
    Friend WithEvents ChkImage As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ChkCmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents txtItemId_NUM As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents cmbItemCounter_OWN As ComboBox
    Friend WithEvents chkCmbNodeId As BrighttechPack.CheckedComboBox
    Friend WithEvents Label8 As Label
    Friend WithEvents cmbDesigner As ComboBox
    Friend WithEvents Label9 As Label
End Class
