<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmItemWiseSalesED
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
        Me.pnlFitration = New System.Windows.Forms.Panel
        Me.cmbMetal = New System.Windows.Forms.ComboBox
        Me.grp1 = New System.Windows.Forms.GroupBox
        Me.dtpRec_OWN = New BrighttechPack.DatePicker(Me.components)
        Me.Label4 = New System.Windows.Forms.Label
        Me.chkPurdetail = New System.Windows.Forms.CheckBox
        Me.chkDesigner = New System.Windows.Forms.CheckBox
        Me.chkCounter = New System.Windows.Forms.CheckBox
        Me.chkcmbMetal = New BrighttechPack.CheckedComboBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.txtBillnoFrom_NUM = New System.Windows.Forms.TextBox
        Me.txtBillNoTo_NUM = New System.Windows.Forms.TextBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.chkcmbcostcentre = New BrighttechPack.CheckedComboBox
        Me.Label22 = New System.Windows.Forms.Label
        Me.chkWithOrderRepair = New System.Windows.Forms.CheckBox
        Me.chkWithMiscIssue = New System.Windows.Forms.CheckBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.rbtName = New System.Windows.Forms.RadioButton
        Me.rbtOrderId = New System.Windows.Forms.RadioButton
        Me.chkCompanySelectAll = New System.Windows.Forms.CheckBox
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.chkLstCompany = New System.Windows.Forms.CheckedListBox
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.chkLstNodeId = New System.Windows.Forms.CheckedListBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnNew = New System.Windows.Forms.Button
        Me.Label8 = New System.Windows.Forms.Label
        Me.chkLstSubItem = New System.Windows.Forms.CheckedListBox
        Me.lblCtrTo = New System.Windows.Forms.Label
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.Label6 = New System.Windows.Forms.Label
        Me.cmbOrderBy = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtCtrIdFrom_NUM = New System.Windows.Forms.TextBox
        Me.txtCtrTo_NUM = New System.Windows.Forms.TextBox
        Me.txtItemIdTo_NUM = New System.Windows.Forms.TextBox
        Me.txtItemIdFrom_NUM = New System.Windows.Forms.TextBox
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.rbtStktypeAll = New System.Windows.Forms.RadioButton
        Me.rbtManufacturing = New System.Windows.Forms.RadioButton
        Me.rbtTrading = New System.Windows.Forms.RadioButton
        Me.chkCmbCounter = New BrighttechPack.CheckedComboBox
        Me.cmbDesigner = New System.Windows.Forms.ComboBox
        Me.chkcmbDesginer = New BrighttechPack.CheckedComboBox
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.pnlGrid = New System.Windows.Forms.Panel
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.pnlfooter = New System.Windows.Forms.Panel
        Me.btnBack = New System.Windows.Forms.Button
        Me.pnlHeading = New System.Windows.Forms.Panel
        Me.lblHeading = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tabmain = New System.Windows.Forms.TabControl
        Me.tabGen = New System.Windows.Forms.TabPage
        Me.tabView = New System.Windows.Forms.TabPage
        Me.pnlFitration.SuspendLayout()
        Me.grp1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.pnlGrid.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmbGridShortCut.SuspendLayout()
        Me.pnlfooter.SuspendLayout()
        Me.pnlHeading.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabmain.SuspendLayout()
        Me.tabGen.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlFitration
        '
        Me.pnlFitration.Controls.Add(Me.cmbMetal)
        Me.pnlFitration.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlFitration.Location = New System.Drawing.Point(3, 3)
        Me.pnlFitration.Name = "pnlFitration"
        Me.pnlFitration.Size = New System.Drawing.Size(1014, 584)
        Me.pnlFitration.TabIndex = 0
        '
        'cmbMetal
        '
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(22, 144)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(227, 21)
        Me.cmbMetal.TabIndex = 0
        Me.cmbMetal.Visible = False
        '
        'grp1
        '
        Me.grp1.Controls.Add(Me.dtpRec_OWN)
        Me.grp1.Controls.Add(Me.Label4)
        Me.grp1.Controls.Add(Me.chkPurdetail)
        Me.grp1.Controls.Add(Me.chkDesigner)
        Me.grp1.Controls.Add(Me.chkCounter)
        Me.grp1.Controls.Add(Me.chkcmbMetal)
        Me.grp1.Controls.Add(Me.Label15)
        Me.grp1.Controls.Add(Me.txtBillnoFrom_NUM)
        Me.grp1.Controls.Add(Me.txtBillNoTo_NUM)
        Me.grp1.Controls.Add(Me.Label16)
        Me.grp1.Controls.Add(Me.chkcmbcostcentre)
        Me.grp1.Controls.Add(Me.Label22)
        Me.grp1.Controls.Add(Me.chkWithOrderRepair)
        Me.grp1.Controls.Add(Me.chkWithMiscIssue)
        Me.grp1.Controls.Add(Me.Panel2)
        Me.grp1.Controls.Add(Me.chkCompanySelectAll)
        Me.grp1.Controls.Add(Me.dtpTo)
        Me.grp1.Controls.Add(Me.chkLstCompany)
        Me.grp1.Controls.Add(Me.dtpFrom)
        Me.grp1.Controls.Add(Me.chkLstNodeId)
        Me.grp1.Controls.Add(Me.Label5)
        Me.grp1.Controls.Add(Me.btnExit)
        Me.grp1.Controls.Add(Me.Label1)
        Me.grp1.Controls.Add(Me.btnNew)
        Me.grp1.Controls.Add(Me.Label8)
        Me.grp1.Controls.Add(Me.chkLstSubItem)
        Me.grp1.Controls.Add(Me.lblCtrTo)
        Me.grp1.Controls.Add(Me.btnView_Search)
        Me.grp1.Controls.Add(Me.Label6)
        Me.grp1.Controls.Add(Me.cmbOrderBy)
        Me.grp1.Controls.Add(Me.Label2)
        Me.grp1.Controls.Add(Me.Label3)
        Me.grp1.Controls.Add(Me.Label12)
        Me.grp1.Controls.Add(Me.Label13)
        Me.grp1.Controls.Add(Me.Label7)
        Me.grp1.Controls.Add(Me.txtCtrIdFrom_NUM)
        Me.grp1.Controls.Add(Me.txtCtrTo_NUM)
        Me.grp1.Controls.Add(Me.txtItemIdTo_NUM)
        Me.grp1.Controls.Add(Me.txtItemIdFrom_NUM)
        Me.grp1.Controls.Add(Me.Panel3)
        Me.grp1.Controls.Add(Me.chkCmbCounter)
        Me.grp1.Controls.Add(Me.cmbDesigner)
        Me.grp1.Controls.Add(Me.chkcmbDesginer)
        Me.grp1.Location = New System.Drawing.Point(269, -1)
        Me.grp1.Name = "grp1"
        Me.grp1.Size = New System.Drawing.Size(389, 578)
        Me.grp1.TabIndex = 1
        Me.grp1.TabStop = False
        '
        'dtpRec_OWN
        '
        Me.dtpRec_OWN.Location = New System.Drawing.Point(120, 513)
        Me.dtpRec_OWN.Mask = "##/##/####"
        Me.dtpRec_OWN.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpRec_OWN.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpRec_OWN.Name = "dtpRec_OWN"
        Me.dtpRec_OWN.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpRec_OWN.Size = New System.Drawing.Size(93, 21)
        Me.dtpRec_OWN.TabIndex = 37
        Me.dtpRec_OWN.Text = "07/03/9998"
        Me.dtpRec_OWN.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(36, 516)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(81, 13)
        Me.Label4.TabIndex = 36
        Me.Label4.Text = "Rec Date >="
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkPurdetail
        '
        Me.chkPurdetail.AutoSize = True
        Me.chkPurdetail.Checked = True
        Me.chkPurdetail.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPurdetail.Location = New System.Drawing.Point(120, 493)
        Me.chkPurdetail.Name = "chkPurdetail"
        Me.chkPurdetail.Size = New System.Drawing.Size(144, 17)
        Me.chkPurdetail.TabIndex = 35
        Me.chkPurdetail.Text = "With Purchase Detail"
        Me.chkPurdetail.UseVisualStyleBackColor = True
        '
        'chkDesigner
        '
        Me.chkDesigner.AutoSize = True
        Me.chkDesigner.Checked = True
        Me.chkDesigner.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDesigner.Location = New System.Drawing.Point(36, 369)
        Me.chkDesigner.Name = "chkDesigner"
        Me.chkDesigner.Size = New System.Drawing.Size(77, 17)
        Me.chkDesigner.TabIndex = 26
        Me.chkDesigner.Text = "Designer"
        Me.chkDesigner.UseVisualStyleBackColor = True
        '
        'chkCounter
        '
        Me.chkCounter.AutoSize = True
        Me.chkCounter.Location = New System.Drawing.Point(10, 317)
        Me.chkCounter.Name = "chkCounter"
        Me.chkCounter.Size = New System.Drawing.Size(105, 17)
        Me.chkCounter.TabIndex = 18
        Me.chkCounter.Text = "Counter From"
        Me.chkCounter.UseVisualStyleBackColor = True
        '
        'chkcmbMetal
        '
        Me.chkcmbMetal.CheckOnClick = True
        Me.chkcmbMetal.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbMetal.DropDownHeight = 1
        Me.chkcmbMetal.FormattingEnabled = True
        Me.chkcmbMetal.IntegralHeight = False
        Me.chkcmbMetal.Location = New System.Drawing.Point(120, 143)
        Me.chkcmbMetal.Name = "chkcmbMetal"
        Me.chkcmbMetal.Size = New System.Drawing.Size(226, 22)
        Me.chkcmbMetal.TabIndex = 9
        Me.chkcmbMetal.ValueSeparator = ", "
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(209, 343)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(21, 13)
        Me.Label15.TabIndex = 24
        Me.Label15.Text = "To"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtBillnoFrom_NUM
        '
        Me.txtBillnoFrom_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtBillnoFrom_NUM.Location = New System.Drawing.Point(120, 340)
        Me.txtBillnoFrom_NUM.Name = "txtBillnoFrom_NUM"
        Me.txtBillnoFrom_NUM.Size = New System.Drawing.Size(85, 21)
        Me.txtBillnoFrom_NUM.TabIndex = 23
        '
        'txtBillNoTo_NUM
        '
        Me.txtBillNoTo_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtBillNoTo_NUM.Location = New System.Drawing.Point(252, 340)
        Me.txtBillNoTo_NUM.Name = "txtBillNoTo_NUM"
        Me.txtBillNoTo_NUM.Size = New System.Drawing.Size(95, 21)
        Me.txtBillNoTo_NUM.TabIndex = 25
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(33, 343)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(57, 13)
        Me.Label16.TabIndex = 22
        Me.Label16.Text = "Bill From"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkcmbcostcentre
        '
        Me.chkcmbcostcentre.CheckOnClick = True
        Me.chkcmbcostcentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbcostcentre.DropDownHeight = 1
        Me.chkcmbcostcentre.FormattingEnabled = True
        Me.chkcmbcostcentre.IntegralHeight = False
        Me.chkcmbcostcentre.Location = New System.Drawing.Point(120, 117)
        Me.chkcmbcostcentre.Name = "chkcmbcostcentre"
        Me.chkcmbcostcentre.Size = New System.Drawing.Size(226, 22)
        Me.chkcmbcostcentre.TabIndex = 7
        Me.chkcmbcostcentre.ValueSeparator = ", "
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(36, 117)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(69, 13)
        Me.Label22.TabIndex = 6
        Me.Label22.Text = "Costcentre"
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkWithOrderRepair
        '
        Me.chkWithOrderRepair.AutoSize = True
        Me.chkWithOrderRepair.Location = New System.Drawing.Point(242, 442)
        Me.chkWithOrderRepair.Name = "chkWithOrderRepair"
        Me.chkWithOrderRepair.Size = New System.Drawing.Size(141, 17)
        Me.chkWithOrderRepair.TabIndex = 33
        Me.chkWithOrderRepair.Text = "With Order && Repair"
        Me.chkWithOrderRepair.UseVisualStyleBackColor = True
        '
        'chkWithMiscIssue
        '
        Me.chkWithMiscIssue.AutoSize = True
        Me.chkWithMiscIssue.Location = New System.Drawing.Point(121, 443)
        Me.chkWithMiscIssue.Name = "chkWithMiscIssue"
        Me.chkWithMiscIssue.Size = New System.Drawing.Size(114, 17)
        Me.chkWithMiscIssue.TabIndex = 32
        Me.chkWithMiscIssue.Text = "With Misc Issue"
        Me.chkWithMiscIssue.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rbtName)
        Me.Panel2.Controls.Add(Me.rbtOrderId)
        Me.Panel2.Location = New System.Drawing.Point(120, 415)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(115, 22)
        Me.Panel2.TabIndex = 31
        '
        'rbtName
        '
        Me.rbtName.AutoSize = True
        Me.rbtName.Location = New System.Drawing.Point(47, 1)
        Me.rbtName.Name = "rbtName"
        Me.rbtName.Size = New System.Drawing.Size(58, 17)
        Me.rbtName.TabIndex = 1
        Me.rbtName.Text = "Name"
        Me.rbtName.UseVisualStyleBackColor = True
        '
        'rbtOrderId
        '
        Me.rbtOrderId.AutoSize = True
        Me.rbtOrderId.Checked = True
        Me.rbtOrderId.Location = New System.Drawing.Point(5, 1)
        Me.rbtOrderId.Name = "rbtOrderId"
        Me.rbtOrderId.Size = New System.Drawing.Size(37, 17)
        Me.rbtOrderId.TabIndex = 0
        Me.rbtOrderId.TabStop = True
        Me.rbtOrderId.Text = "Id"
        Me.rbtOrderId.UseVisualStyleBackColor = True
        '
        'chkCompanySelectAll
        '
        Me.chkCompanySelectAll.AutoSize = True
        Me.chkCompanySelectAll.Location = New System.Drawing.Point(39, 43)
        Me.chkCompanySelectAll.Name = "chkCompanySelectAll"
        Me.chkCompanySelectAll.Size = New System.Drawing.Size(81, 17)
        Me.chkCompanySelectAll.TabIndex = 4
        Me.chkCompanySelectAll.Text = "Company"
        Me.chkCompanySelectAll.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(251, 18)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'chkLstCompany
        '
        Me.chkLstCompany.FormattingEnabled = True
        Me.chkLstCompany.Location = New System.Drawing.Point(36, 62)
        Me.chkLstCompany.Name = "chkLstCompany"
        Me.chkLstCompany.Size = New System.Drawing.Size(310, 52)
        Me.chkLstCompany.TabIndex = 5
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(101, 18)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(93, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'chkLstNodeId
        '
        Me.chkLstNodeId.FormattingEnabled = True
        Me.chkLstNodeId.Location = New System.Drawing.Point(205, 211)
        Me.chkLstNodeId.Name = "chkLstNodeId"
        Me.chkLstNodeId.Size = New System.Drawing.Size(141, 100)
        Me.chkLstNodeId.TabIndex = 17
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(33, 21)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(67, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "From Date"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(246, 546)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 40
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(202, 195)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(36, 13)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Node"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(141, 546)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 39
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(209, 172)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(21, 13)
        Me.Label8.TabIndex = 12
        Me.Label8.Text = "To"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkLstSubItem
        '
        Me.chkLstSubItem.FormattingEnabled = True
        Me.chkLstSubItem.Location = New System.Drawing.Point(36, 211)
        Me.chkLstSubItem.Name = "chkLstSubItem"
        Me.chkLstSubItem.Size = New System.Drawing.Size(140, 100)
        Me.chkLstSubItem.TabIndex = 15
        '
        'lblCtrTo
        '
        Me.lblCtrTo.AutoSize = True
        Me.lblCtrTo.Location = New System.Drawing.Point(209, 318)
        Me.lblCtrTo.Name = "lblCtrTo"
        Me.lblCtrTo.Size = New System.Drawing.Size(21, 13)
        Me.lblCtrTo.TabIndex = 20
        Me.lblCtrTo.Text = "To"
        Me.lblCtrTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(36, 546)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 38
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(202, 21)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(52, 13)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "To Date"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbOrderBy
        '
        Me.cmbOrderBy.FormattingEnabled = True
        Me.cmbOrderBy.Location = New System.Drawing.Point(120, 390)
        Me.cmbOrderBy.Name = "cmbOrderBy"
        Me.cmbOrderBy.Size = New System.Drawing.Size(227, 21)
        Me.cmbOrderBy.TabIndex = 29
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(36, 172)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(83, 13)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Item Id From"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(33, 195)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 13)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "Sub Item"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(36, 146)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(37, 13)
        Me.Label12.TabIndex = 8
        Me.Label12.Text = "Metal"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(34, 414)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(59, 13)
        Me.Label13.TabIndex = 30
        Me.Label13.Text = "Order By"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(33, 393)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(61, 13)
        Me.Label7.TabIndex = 28
        Me.Label7.Text = "Group By"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtCtrIdFrom_NUM
        '
        Me.txtCtrIdFrom_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCtrIdFrom_NUM.Location = New System.Drawing.Point(120, 315)
        Me.txtCtrIdFrom_NUM.Name = "txtCtrIdFrom_NUM"
        Me.txtCtrIdFrom_NUM.Size = New System.Drawing.Size(85, 21)
        Me.txtCtrIdFrom_NUM.TabIndex = 19
        '
        'txtCtrTo_NUM
        '
        Me.txtCtrTo_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCtrTo_NUM.Location = New System.Drawing.Point(252, 315)
        Me.txtCtrTo_NUM.Name = "txtCtrTo_NUM"
        Me.txtCtrTo_NUM.Size = New System.Drawing.Size(95, 21)
        Me.txtCtrTo_NUM.TabIndex = 21
        '
        'txtItemIdTo_NUM
        '
        Me.txtItemIdTo_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtItemIdTo_NUM.Location = New System.Drawing.Point(252, 169)
        Me.txtItemIdTo_NUM.Name = "txtItemIdTo_NUM"
        Me.txtItemIdTo_NUM.Size = New System.Drawing.Size(95, 21)
        Me.txtItemIdTo_NUM.TabIndex = 13
        '
        'txtItemIdFrom_NUM
        '
        Me.txtItemIdFrom_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtItemIdFrom_NUM.Location = New System.Drawing.Point(120, 168)
        Me.txtItemIdFrom_NUM.Name = "txtItemIdFrom_NUM"
        Me.txtItemIdFrom_NUM.Size = New System.Drawing.Size(85, 21)
        Me.txtItemIdFrom_NUM.TabIndex = 11
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.rbtStktypeAll)
        Me.Panel3.Controls.Add(Me.rbtManufacturing)
        Me.Panel3.Controls.Add(Me.rbtTrading)
        Me.Panel3.Location = New System.Drawing.Point(120, 466)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(229, 21)
        Me.Panel3.TabIndex = 34
        '
        'rbtStktypeAll
        '
        Me.rbtStktypeAll.AutoSize = True
        Me.rbtStktypeAll.Location = New System.Drawing.Point(180, 2)
        Me.rbtStktypeAll.Name = "rbtStktypeAll"
        Me.rbtStktypeAll.Size = New System.Drawing.Size(39, 17)
        Me.rbtStktypeAll.TabIndex = 0
        Me.rbtStktypeAll.Text = "All"
        Me.rbtStktypeAll.UseVisualStyleBackColor = True
        '
        'rbtManufacturing
        '
        Me.rbtManufacturing.AutoSize = True
        Me.rbtManufacturing.Location = New System.Drawing.Point(77, 2)
        Me.rbtManufacturing.Name = "rbtManufacturing"
        Me.rbtManufacturing.Size = New System.Drawing.Size(105, 17)
        Me.rbtManufacturing.TabIndex = 2
        Me.rbtManufacturing.Text = "Manufacturing"
        Me.rbtManufacturing.UseVisualStyleBackColor = True
        '
        'rbtTrading
        '
        Me.rbtTrading.AutoSize = True
        Me.rbtTrading.Location = New System.Drawing.Point(3, 2)
        Me.rbtTrading.Name = "rbtTrading"
        Me.rbtTrading.Size = New System.Drawing.Size(68, 17)
        Me.rbtTrading.TabIndex = 1
        Me.rbtTrading.Text = "Trading"
        Me.rbtTrading.UseVisualStyleBackColor = True
        '
        'chkCmbCounter
        '
        Me.chkCmbCounter.CheckOnClick = True
        Me.chkCmbCounter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCounter.DropDownHeight = 1
        Me.chkCmbCounter.FormattingEnabled = True
        Me.chkCmbCounter.IntegralHeight = False
        Me.chkCmbCounter.Location = New System.Drawing.Point(120, 315)
        Me.chkCmbCounter.Name = "chkCmbCounter"
        Me.chkCmbCounter.Size = New System.Drawing.Size(226, 22)
        Me.chkCmbCounter.TabIndex = 22
        Me.chkCmbCounter.ValueSeparator = ", "
        Me.chkCmbCounter.Visible = False
        '
        'cmbDesigner
        '
        Me.cmbDesigner.FormattingEnabled = True
        Me.cmbDesigner.Location = New System.Drawing.Point(120, 364)
        Me.cmbDesigner.Name = "cmbDesigner"
        Me.cmbDesigner.Size = New System.Drawing.Size(227, 21)
        Me.cmbDesigner.TabIndex = 27
        '
        'chkcmbDesginer
        '
        Me.chkcmbDesginer.CheckOnClick = True
        Me.chkcmbDesginer.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbDesginer.DropDownHeight = 1
        Me.chkcmbDesginer.FormattingEnabled = True
        Me.chkcmbDesginer.IntegralHeight = False
        Me.chkcmbDesginer.Location = New System.Drawing.Point(122, 362)
        Me.chkcmbDesginer.Name = "chkcmbDesginer"
        Me.chkcmbDesginer.Size = New System.Drawing.Size(227, 22)
        Me.chkcmbDesginer.TabIndex = 44
        Me.chkcmbDesginer.ValueSeparator = ", "
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(594, 6)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 20
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(702, 6)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 20
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'pnlGrid
        '
        Me.pnlGrid.Controls.Add(Me.Panel1)
        Me.pnlGrid.Controls.Add(Me.pnlfooter)
        Me.pnlGrid.Controls.Add(Me.pnlHeading)
        Me.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlGrid.Location = New System.Drawing.Point(3, 3)
        Me.pnlGrid.Name = "pnlGrid"
        Me.pnlGrid.Size = New System.Drawing.Size(1014, 584)
        Me.pnlGrid.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.gridView)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 30)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1014, 516)
        Me.Panel1.TabIndex = 0
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.ContextMenuStrip = Me.cmbGridShortCut
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 0)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 18
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(1014, 516)
        Me.gridView.TabIndex = 0
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
        'pnlfooter
        '
        Me.pnlfooter.Controls.Add(Me.btnBack)
        Me.pnlfooter.Controls.Add(Me.btnPrint)
        Me.pnlfooter.Controls.Add(Me.btnExport)
        Me.pnlfooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlfooter.Location = New System.Drawing.Point(0, 546)
        Me.pnlfooter.Name = "pnlfooter"
        Me.pnlfooter.Size = New System.Drawing.Size(1014, 38)
        Me.pnlfooter.TabIndex = 1
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(486, 6)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 21
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'pnlHeading
        '
        Me.pnlHeading.Controls.Add(Me.lblHeading)
        Me.pnlHeading.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeading.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeading.Name = "pnlHeading"
        Me.pnlHeading.Size = New System.Drawing.Size(1014, 30)
        Me.pnlHeading.TabIndex = 1
        '
        'lblHeading
        '
        Me.lblHeading.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblHeading.Location = New System.Drawing.Point(0, 0)
        Me.lblHeading.Name = "lblHeading"
        Me.lblHeading.Size = New System.Drawing.Size(1014, 30)
        Me.lblHeading.TabIndex = 0
        Me.lblHeading.Text = "Label10"
        Me.lblHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
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
        'tabmain
        '
        Me.tabmain.Controls.Add(Me.tabGen)
        Me.tabmain.Controls.Add(Me.tabView)
        Me.tabmain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabmain.Location = New System.Drawing.Point(0, 0)
        Me.tabmain.Name = "tabmain"
        Me.tabmain.SelectedIndex = 0
        Me.tabmain.Size = New System.Drawing.Size(1028, 616)
        Me.tabmain.TabIndex = 0
        '
        'tabGen
        '
        Me.tabGen.Controls.Add(Me.grp1)
        Me.tabGen.Controls.Add(Me.pnlFitration)
        Me.tabGen.Location = New System.Drawing.Point(4, 22)
        Me.tabGen.Name = "tabGen"
        Me.tabGen.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGen.Size = New System.Drawing.Size(1020, 590)
        Me.tabGen.TabIndex = 0
        Me.tabGen.Text = "Gen"
        Me.tabGen.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.pnlGrid)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1020, 590)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'frmItemWiseSalesED
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 616)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabmain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmItemWiseSalesED"
        Me.Text = "Item Wise Excise Duty Report"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlFitration.ResumeLayout(False)
        Me.grp1.ResumeLayout(False)
        Me.grp1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.pnlGrid.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmbGridShortCut.ResumeLayout(False)
        Me.pnlfooter.ResumeLayout(False)
        Me.pnlHeading.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabmain.ResumeLayout(False)
        Me.tabGen.ResumeLayout(False)
        Me.tabView.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlFitration As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents pnlGrid As System.Windows.Forms.Panel
    Friend WithEvents txtCtrIdFrom_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtItemIdFrom_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents txtCtrTo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtItemIdTo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents cmbOrderBy As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lblCtrTo As System.Windows.Forms.Label
    Friend WithEvents chkLstSubItem As System.Windows.Forms.CheckedListBox
    Friend WithEvents pnlHeading As System.Windows.Forms.Panel
    Friend WithEvents lblHeading As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents tabmain As System.Windows.Forms.TabControl
    Friend WithEvents tabGen As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents pnlfooter As System.Windows.Forms.Panel
    Friend WithEvents grp1 As System.Windows.Forms.GroupBox
    Friend WithEvents chkLstNodeId As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkCompanySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCompany As System.Windows.Forms.CheckedListBox
    Friend WithEvents cmbMetal As System.Windows.Forms.ComboBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents rbtManufacturing As System.Windows.Forms.RadioButton
    Friend WithEvents rbtTrading As System.Windows.Forms.RadioButton
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rbtName As System.Windows.Forms.RadioButton
    Friend WithEvents rbtOrderId As System.Windows.Forms.RadioButton
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents chkWithMiscIssue As System.Windows.Forms.CheckBox
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbDesigner As System.Windows.Forms.ComboBox
    Friend WithEvents chkWithOrderRepair As System.Windows.Forms.CheckBox
    Friend WithEvents chkcmbcostcentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtBillnoFrom_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtBillNoTo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents chkcmbMetal As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbCounter As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCounter As System.Windows.Forms.CheckBox
    Friend WithEvents chkDesigner As System.Windows.Forms.CheckBox
    Friend WithEvents chkcmbDesginer As BrighttechPack.CheckedComboBox
    Friend WithEvents chkPurdetail As System.Windows.Forms.CheckBox
    Friend WithEvents rbtStktypeAll As System.Windows.Forms.RadioButton
    Friend WithEvents dtpRec_OWN As BrighttechPack.DatePicker
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
