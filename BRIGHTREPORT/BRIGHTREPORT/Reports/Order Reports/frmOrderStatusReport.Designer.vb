<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmOrderStatusReport
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
        Me.grpContainer = New System.Windows.Forms.GroupBox()
        Me.cmbStatus = New System.Windows.Forms.ComboBox()
        Me.cmbSmith = New System.Windows.Forms.ComboBox()
        Me.chkStatusMultiSelect = New System.Windows.Forms.CheckBox()
        Me.chkSmithMultiSelect = New System.Windows.Forms.CheckBox()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.rbtBothStatus = New System.Windows.Forms.RadioButton()
        Me.rbtDeliveryedStatus = New System.Windows.Forms.RadioButton()
        Me.rbtPendingStatus = New System.Windows.Forms.RadioButton()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.rbtBothRate = New System.Windows.Forms.RadioButton()
        Me.rbtDeliveryRate = New System.Windows.Forms.RadioButton()
        Me.rbtCurrentRate = New System.Windows.Forms.RadioButton()
        Me.chkCmbDealer = New BrighttechPack.CheckedComboBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.rbtSmith = New System.Windows.Forms.RadioButton()
        Me.rbtWithOutGrpBy = New System.Windows.Forms.RadioButton()
        Me.rbtCounter = New System.Windows.Forms.RadioButton()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.rbtEmpName = New System.Windows.Forms.RadioButton()
        Me.cmbOrderBy = New System.Windows.Forms.ComboBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.ChkCmbStatus = New BrighttechPack.CheckedComboBox()
        Me.chkcountercmb = New BrighttechPack.CheckedComboBox()
        Me.chkCmbDesigner = New BrighttechPack.CheckedComboBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.rbtTypeAll = New System.Windows.Forms.RadioButton()
        Me.rbtTypeCompany = New System.Windows.Forms.RadioButton()
        Me.rbtTypeCustomer = New System.Windows.Forms.RadioButton()
        Me.chkCmbItem = New BrighttechPack.CheckedComboBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.rbtbooking = New System.Windows.Forms.RadioButton()
        Me.rbtOrder = New System.Windows.Forms.RadioButton()
        Me.rbtBoth = New System.Windows.Forms.RadioButton()
        Me.rbtRepair = New System.Windows.Forms.RadioButton()
        Me.chkCmbMetal = New BrighttechPack.CheckedComboBox()
        Me.txtCustomerName = New System.Windows.Forms.TextBox()
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbEmpName = New System.Windows.Forms.ComboBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.chkAsOnDate = New System.Windows.Forms.CheckBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.rbtOrderDate = New System.Windows.Forms.RadioButton()
        Me.rbtDueDate = New System.Windows.Forms.RadioButton()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabView = New System.Windows.Forms.TabPage()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.pnlfooter = New System.Windows.Forms.Panel()
        Me.lblRecSmith = New System.Windows.Forms.Label()
        Me.lblRec = New System.Windows.Forms.Label()
        Me.lblPendingWithUs = New System.Windows.Forms.Label()
        Me.lblPending = New System.Windows.Forms.Label()
        Me.lblDeliver = New System.Windows.Forms.Label()
        Me.lblIsstoSmith = New System.Windows.Forms.Label()
        Me.lblIss = New System.Windows.Forms.Label()
        Me.lblDelivered = New System.Windows.Forms.Label()
        Me.lblCancelled = New System.Windows.Forms.Label()
        Me.lblCancel = New System.Windows.Forms.Label()
        Me.lblApproval = New System.Windows.Forms.Label()
        Me.lblAppIss = New System.Windows.Forms.Label()
        Me.lblReadyforDel = New System.Windows.Forms.Label()
        Me.lblReady = New System.Windows.Forms.Label()
        Me.btnSendSms = New System.Windows.Forms.Button()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.Panel7 = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.TabGeneral = New System.Windows.Forms.TabPage()
        Me.chkWithSummary = New System.Windows.Forms.CheckBox()
        Me.grpContainer.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.Panel5.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlfooter.SuspendLayout()
        Me.Panel7.SuspendLayout()
        Me.TabGeneral.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpContainer
        '
        Me.grpContainer.Controls.Add(Me.chkWithSummary)
        Me.grpContainer.Controls.Add(Me.cmbStatus)
        Me.grpContainer.Controls.Add(Me.cmbSmith)
        Me.grpContainer.Controls.Add(Me.chkStatusMultiSelect)
        Me.grpContainer.Controls.Add(Me.chkSmithMultiSelect)
        Me.grpContainer.Controls.Add(Me.Panel6)
        Me.grpContainer.Controls.Add(Me.Panel4)
        Me.grpContainer.Controls.Add(Me.chkCmbDealer)
        Me.grpContainer.Controls.Add(Me.Label14)
        Me.grpContainer.Controls.Add(Me.Panel2)
        Me.grpContainer.Controls.Add(Me.cmbOrderBy)
        Me.grpContainer.Controls.Add(Me.Label11)
        Me.grpContainer.Controls.Add(Me.ChkCmbStatus)
        Me.grpContainer.Controls.Add(Me.chkcountercmb)
        Me.grpContainer.Controls.Add(Me.chkCmbDesigner)
        Me.grpContainer.Controls.Add(Me.Panel1)
        Me.grpContainer.Controls.Add(Me.chkCmbItem)
        Me.grpContainer.Controls.Add(Me.Panel3)
        Me.grpContainer.Controls.Add(Me.chkCmbMetal)
        Me.grpContainer.Controls.Add(Me.txtCustomerName)
        Me.grpContainer.Controls.Add(Me.chkCmbCostCentre)
        Me.grpContainer.Controls.Add(Me.Label6)
        Me.grpContainer.Controls.Add(Me.Label7)
        Me.grpContainer.Controls.Add(Me.Label1)
        Me.grpContainer.Controls.Add(Me.cmbEmpName)
        Me.grpContainer.Controls.Add(Me.Label13)
        Me.grpContainer.Controls.Add(Me.chkAsOnDate)
        Me.grpContainer.Controls.Add(Me.Label10)
        Me.grpContainer.Controls.Add(Me.Label9)
        Me.grpContainer.Controls.Add(Me.Label8)
        Me.grpContainer.Controls.Add(Me.Label3)
        Me.grpContainer.Controls.Add(Me.Label5)
        Me.grpContainer.Controls.Add(Me.rbtOrderDate)
        Me.grpContainer.Controls.Add(Me.rbtDueDate)
        Me.grpContainer.Controls.Add(Me.dtpTo)
        Me.grpContainer.Controls.Add(Me.dtpFrom)
        Me.grpContainer.Controls.Add(Me.btnSearch)
        Me.grpContainer.Controls.Add(Me.btnExit)
        Me.grpContainer.Controls.Add(Me.btnNew)
        Me.grpContainer.Controls.Add(Me.Label2)
        Me.grpContainer.Location = New System.Drawing.Point(241, 15)
        Me.grpContainer.Name = "grpContainer"
        Me.grpContainer.Size = New System.Drawing.Size(440, 573)
        Me.grpContainer.TabIndex = 0
        Me.grpContainer.TabStop = False
        '
        'cmbStatus
        '
        Me.cmbStatus.FormattingEnabled = True
        Me.cmbStatus.Location = New System.Drawing.Point(108, 367)
        Me.cmbStatus.Name = "cmbStatus"
        Me.cmbStatus.Size = New System.Drawing.Size(290, 21)
        Me.cmbStatus.TabIndex = 2
        '
        'cmbSmith
        '
        Me.cmbSmith.FormattingEnabled = True
        Me.cmbSmith.Location = New System.Drawing.Point(108, 338)
        Me.cmbSmith.Name = "cmbSmith"
        Me.cmbSmith.Size = New System.Drawing.Size(290, 21)
        Me.cmbSmith.TabIndex = 1
        '
        'chkStatusMultiSelect
        '
        Me.chkStatusMultiSelect.AutoSize = True
        Me.chkStatusMultiSelect.Location = New System.Drawing.Point(20, 394)
        Me.chkStatusMultiSelect.Name = "chkStatusMultiSelect"
        Me.chkStatusMultiSelect.Size = New System.Drawing.Size(87, 17)
        Me.chkStatusMultiSelect.TabIndex = 36
        Me.chkStatusMultiSelect.Text = "MultiSelect"
        Me.chkStatusMultiSelect.UseVisualStyleBackColor = True
        '
        'chkSmithMultiSelect
        '
        Me.chkSmithMultiSelect.AutoSize = True
        Me.chkSmithMultiSelect.Location = New System.Drawing.Point(18, 355)
        Me.chkSmithMultiSelect.Name = "chkSmithMultiSelect"
        Me.chkSmithMultiSelect.Size = New System.Drawing.Size(87, 17)
        Me.chkSmithMultiSelect.TabIndex = 35
        Me.chkSmithMultiSelect.Text = "MultiSelect"
        Me.chkSmithMultiSelect.UseVisualStyleBackColor = True
        '
        'Panel6
        '
        Me.Panel6.Controls.Add(Me.rbtBothStatus)
        Me.Panel6.Controls.Add(Me.rbtDeliveryedStatus)
        Me.Panel6.Controls.Add(Me.rbtPendingStatus)
        Me.Panel6.Location = New System.Drawing.Point(107, 394)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(315, 30)
        Me.Panel6.TabIndex = 34
        '
        'rbtBothStatus
        '
        Me.rbtBothStatus.AutoSize = True
        Me.rbtBothStatus.Checked = True
        Me.rbtBothStatus.Location = New System.Drawing.Point(6, 10)
        Me.rbtBothStatus.Name = "rbtBothStatus"
        Me.rbtBothStatus.Size = New System.Drawing.Size(51, 17)
        Me.rbtBothStatus.TabIndex = 0
        Me.rbtBothStatus.TabStop = True
        Me.rbtBothStatus.Text = "Both"
        Me.rbtBothStatus.UseVisualStyleBackColor = True
        '
        'rbtDeliveryedStatus
        '
        Me.rbtDeliveryedStatus.AutoSize = True
        Me.rbtDeliveryedStatus.Location = New System.Drawing.Point(155, 10)
        Me.rbtDeliveryedStatus.Name = "rbtDeliveryedStatus"
        Me.rbtDeliveryedStatus.Size = New System.Drawing.Size(87, 17)
        Me.rbtDeliveryedStatus.TabIndex = 2
        Me.rbtDeliveryedStatus.Text = "Deliveryed"
        Me.rbtDeliveryedStatus.UseVisualStyleBackColor = True
        '
        'rbtPendingStatus
        '
        Me.rbtPendingStatus.AutoSize = True
        Me.rbtPendingStatus.Location = New System.Drawing.Point(67, 10)
        Me.rbtPendingStatus.Name = "rbtPendingStatus"
        Me.rbtPendingStatus.Size = New System.Drawing.Size(70, 17)
        Me.rbtPendingStatus.TabIndex = 1
        Me.rbtPendingStatus.Text = "Pending"
        Me.rbtPendingStatus.UseVisualStyleBackColor = True
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.rbtBothRate)
        Me.Panel4.Controls.Add(Me.rbtDeliveryRate)
        Me.Panel4.Controls.Add(Me.rbtCurrentRate)
        Me.Panel4.Location = New System.Drawing.Point(108, 428)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(315, 30)
        Me.Panel4.TabIndex = 27
        '
        'rbtBothRate
        '
        Me.rbtBothRate.AutoSize = True
        Me.rbtBothRate.Checked = True
        Me.rbtBothRate.Location = New System.Drawing.Point(6, 10)
        Me.rbtBothRate.Name = "rbtBothRate"
        Me.rbtBothRate.Size = New System.Drawing.Size(51, 17)
        Me.rbtBothRate.TabIndex = 0
        Me.rbtBothRate.TabStop = True
        Me.rbtBothRate.Text = "Both"
        Me.rbtBothRate.UseVisualStyleBackColor = True
        '
        'rbtDeliveryRate
        '
        Me.rbtDeliveryRate.AutoSize = True
        Me.rbtDeliveryRate.Location = New System.Drawing.Point(171, 10)
        Me.rbtDeliveryRate.Name = "rbtDeliveryRate"
        Me.rbtDeliveryRate.Size = New System.Drawing.Size(103, 17)
        Me.rbtDeliveryRate.TabIndex = 2
        Me.rbtDeliveryRate.Text = "Delivery Rate"
        Me.rbtDeliveryRate.UseVisualStyleBackColor = True
        '
        'rbtCurrentRate
        '
        Me.rbtCurrentRate.AutoSize = True
        Me.rbtCurrentRate.Location = New System.Drawing.Point(66, 10)
        Me.rbtCurrentRate.Name = "rbtCurrentRate"
        Me.rbtCurrentRate.Size = New System.Drawing.Size(99, 17)
        Me.rbtCurrentRate.TabIndex = 1
        Me.rbtCurrentRate.Text = "Current Rate"
        Me.rbtCurrentRate.UseVisualStyleBackColor = True
        '
        'chkCmbDealer
        '
        Me.chkCmbDealer.CheckOnClick = True
        Me.chkCmbDealer.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbDealer.DropDownHeight = 1
        Me.chkCmbDealer.FormattingEnabled = True
        Me.chkCmbDealer.IntegralHeight = False
        Me.chkCmbDealer.Location = New System.Drawing.Point(108, 337)
        Me.chkCmbDealer.Name = "chkCmbDealer"
        Me.chkCmbDealer.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbDealer.TabIndex = 24
        Me.chkCmbDealer.ValueSeparator = ", "
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(18, 338)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(83, 13)
        Me.Label14.TabIndex = 23
        Me.Label14.Text = "Smith/Dealer"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rbtSmith)
        Me.Panel2.Controls.Add(Me.rbtWithOutGrpBy)
        Me.Panel2.Controls.Add(Me.rbtCounter)
        Me.Panel2.Controls.Add(Me.Label12)
        Me.Panel2.Controls.Add(Me.rbtEmpName)
        Me.Panel2.Location = New System.Drawing.Point(6, 465)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(416, 30)
        Me.Panel2.TabIndex = 28
        '
        'rbtSmith
        '
        Me.rbtSmith.AutoSize = True
        Me.rbtSmith.Location = New System.Drawing.Point(333, 6)
        Me.rbtSmith.Name = "rbtSmith"
        Me.rbtSmith.Size = New System.Drawing.Size(58, 17)
        Me.rbtSmith.TabIndex = 4
        Me.rbtSmith.Text = "Smith"
        Me.rbtSmith.UseVisualStyleBackColor = True
        '
        'rbtWithOutGrpBy
        '
        Me.rbtWithOutGrpBy.AutoSize = True
        Me.rbtWithOutGrpBy.Checked = True
        Me.rbtWithOutGrpBy.Location = New System.Drawing.Point(108, 6)
        Me.rbtWithOutGrpBy.Name = "rbtWithOutGrpBy"
        Me.rbtWithOutGrpBy.Size = New System.Drawing.Size(54, 17)
        Me.rbtWithOutGrpBy.TabIndex = 1
        Me.rbtWithOutGrpBy.TabStop = True
        Me.rbtWithOutGrpBy.Text = "None"
        Me.rbtWithOutGrpBy.UseVisualStyleBackColor = True
        '
        'rbtCounter
        '
        Me.rbtCounter.AutoSize = True
        Me.rbtCounter.Location = New System.Drawing.Point(256, 6)
        Me.rbtCounter.Name = "rbtCounter"
        Me.rbtCounter.Size = New System.Drawing.Size(71, 17)
        Me.rbtCounter.TabIndex = 3
        Me.rbtCounter.Text = "Counter"
        Me.rbtCounter.UseVisualStyleBackColor = True
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(9, 8)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(61, 13)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "Group By"
        '
        'rbtEmpName
        '
        Me.rbtEmpName.AutoSize = True
        Me.rbtEmpName.Location = New System.Drawing.Point(168, 6)
        Me.rbtEmpName.Name = "rbtEmpName"
        Me.rbtEmpName.Size = New System.Drawing.Size(83, 17)
        Me.rbtEmpName.TabIndex = 2
        Me.rbtEmpName.Text = "EmpName"
        Me.rbtEmpName.UseVisualStyleBackColor = True
        '
        'cmbOrderBy
        '
        Me.cmbOrderBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOrderBy.FormattingEnabled = True
        Me.cmbOrderBy.Location = New System.Drawing.Point(108, 502)
        Me.cmbOrderBy.Name = "cmbOrderBy"
        Me.cmbOrderBy.Size = New System.Drawing.Size(187, 21)
        Me.cmbOrderBy.TabIndex = 30
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(17, 505)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(59, 13)
        Me.Label11.TabIndex = 29
        Me.Label11.Text = "Order By"
        '
        'ChkCmbStatus
        '
        Me.ChkCmbStatus.CheckOnClick = True
        Me.ChkCmbStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.ChkCmbStatus.DropDownHeight = 1
        Me.ChkCmbStatus.FormattingEnabled = True
        Me.ChkCmbStatus.IntegralHeight = False
        Me.ChkCmbStatus.Location = New System.Drawing.Point(108, 366)
        Me.ChkCmbStatus.Name = "ChkCmbStatus"
        Me.ChkCmbStatus.Size = New System.Drawing.Size(290, 22)
        Me.ChkCmbStatus.TabIndex = 26
        Me.ChkCmbStatus.ValueSeparator = ", "
        '
        'chkcountercmb
        '
        Me.chkcountercmb.CheckOnClick = True
        Me.chkcountercmb.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcountercmb.DropDownHeight = 1
        Me.chkcountercmb.FormattingEnabled = True
        Me.chkcountercmb.IntegralHeight = False
        Me.chkcountercmb.Location = New System.Drawing.Point(108, 279)
        Me.chkcountercmb.Name = "chkcountercmb"
        Me.chkcountercmb.Size = New System.Drawing.Size(290, 22)
        Me.chkcountercmb.TabIndex = 20
        Me.chkcountercmb.ValueSeparator = ", "
        '
        'chkCmbDesigner
        '
        Me.chkCmbDesigner.CheckOnClick = True
        Me.chkCmbDesigner.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbDesigner.DropDownHeight = 1
        Me.chkCmbDesigner.FormattingEnabled = True
        Me.chkCmbDesigner.IntegralHeight = False
        Me.chkCmbDesigner.Location = New System.Drawing.Point(108, 221)
        Me.chkCmbDesigner.Name = "chkCmbDesigner"
        Me.chkCmbDesigner.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbDesigner.TabIndex = 16
        Me.chkCmbDesigner.ValueSeparator = ", "
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.rbtTypeAll)
        Me.Panel1.Controls.Add(Me.rbtTypeCompany)
        Me.Panel1.Controls.Add(Me.rbtTypeCustomer)
        Me.Panel1.Location = New System.Drawing.Point(6, 104)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(428, 22)
        Me.Panel1.TabIndex = 8
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(11, 3)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(34, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Type"
        '
        'rbtTypeAll
        '
        Me.rbtTypeAll.AutoSize = True
        Me.rbtTypeAll.Location = New System.Drawing.Point(103, 1)
        Me.rbtTypeAll.Name = "rbtTypeAll"
        Me.rbtTypeAll.Size = New System.Drawing.Size(39, 17)
        Me.rbtTypeAll.TabIndex = 1
        Me.rbtTypeAll.TabStop = True
        Me.rbtTypeAll.Text = "All"
        Me.rbtTypeAll.UseVisualStyleBackColor = True
        '
        'rbtTypeCompany
        '
        Me.rbtTypeCompany.AutoSize = True
        Me.rbtTypeCompany.Location = New System.Drawing.Point(162, 1)
        Me.rbtTypeCompany.Name = "rbtTypeCompany"
        Me.rbtTypeCompany.Size = New System.Drawing.Size(80, 17)
        Me.rbtTypeCompany.TabIndex = 2
        Me.rbtTypeCompany.TabStop = True
        Me.rbtTypeCompany.Text = "Company"
        Me.rbtTypeCompany.UseVisualStyleBackColor = True
        '
        'rbtTypeCustomer
        '
        Me.rbtTypeCustomer.AutoSize = True
        Me.rbtTypeCustomer.Location = New System.Drawing.Point(253, 1)
        Me.rbtTypeCustomer.Name = "rbtTypeCustomer"
        Me.rbtTypeCustomer.Size = New System.Drawing.Size(81, 17)
        Me.rbtTypeCustomer.TabIndex = 3
        Me.rbtTypeCustomer.TabStop = True
        Me.rbtTypeCustomer.Text = "Customer"
        Me.rbtTypeCustomer.UseVisualStyleBackColor = True
        '
        'chkCmbItem
        '
        Me.chkCmbItem.CheckOnClick = True
        Me.chkCmbItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbItem.DropDownHeight = 1
        Me.chkCmbItem.FormattingEnabled = True
        Me.chkCmbItem.IntegralHeight = False
        Me.chkCmbItem.Location = New System.Drawing.Point(108, 191)
        Me.chkCmbItem.Name = "chkCmbItem"
        Me.chkCmbItem.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbItem.TabIndex = 14
        Me.chkCmbItem.ValueSeparator = ", "
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.rbtbooking)
        Me.Panel3.Controls.Add(Me.rbtOrder)
        Me.Panel3.Controls.Add(Me.rbtBoth)
        Me.Panel3.Controls.Add(Me.rbtRepair)
        Me.Panel3.Location = New System.Drawing.Point(7, 20)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(428, 22)
        Me.Panel3.TabIndex = 0
        '
        'rbtbooking
        '
        Me.rbtbooking.AutoSize = True
        Me.rbtbooking.Location = New System.Drawing.Point(294, 3)
        Me.rbtbooking.Name = "rbtbooking"
        Me.rbtbooking.Size = New System.Drawing.Size(68, 17)
        Me.rbtbooking.TabIndex = 3
        Me.rbtbooking.TabStop = True
        Me.rbtbooking.Text = "Booked"
        Me.rbtbooking.UseVisualStyleBackColor = True
        '
        'rbtOrder
        '
        Me.rbtOrder.AutoSize = True
        Me.rbtOrder.Location = New System.Drawing.Point(162, 2)
        Me.rbtOrder.Name = "rbtOrder"
        Me.rbtOrder.Size = New System.Drawing.Size(58, 17)
        Me.rbtOrder.TabIndex = 1
        Me.rbtOrder.TabStop = True
        Me.rbtOrder.Text = "Order"
        Me.rbtOrder.UseVisualStyleBackColor = True
        '
        'rbtBoth
        '
        Me.rbtBoth.AutoSize = True
        Me.rbtBoth.Checked = True
        Me.rbtBoth.Location = New System.Drawing.Point(101, 2)
        Me.rbtBoth.Name = "rbtBoth"
        Me.rbtBoth.Size = New System.Drawing.Size(51, 17)
        Me.rbtBoth.TabIndex = 0
        Me.rbtBoth.TabStop = True
        Me.rbtBoth.Text = "Both"
        Me.rbtBoth.UseVisualStyleBackColor = True
        '
        'rbtRepair
        '
        Me.rbtRepair.AutoSize = True
        Me.rbtRepair.Location = New System.Drawing.Point(226, 2)
        Me.rbtRepair.Name = "rbtRepair"
        Me.rbtRepair.Size = New System.Drawing.Size(62, 17)
        Me.rbtRepair.TabIndex = 2
        Me.rbtRepair.TabStop = True
        Me.rbtRepair.Text = "Repair"
        Me.rbtRepair.UseVisualStyleBackColor = True
        '
        'chkCmbMetal
        '
        Me.chkCmbMetal.CheckOnClick = True
        Me.chkCmbMetal.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbMetal.DropDownHeight = 1
        Me.chkCmbMetal.FormattingEnabled = True
        Me.chkCmbMetal.IntegralHeight = False
        Me.chkCmbMetal.Location = New System.Drawing.Point(108, 162)
        Me.chkCmbMetal.Name = "chkCmbMetal"
        Me.chkCmbMetal.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbMetal.TabIndex = 12
        Me.chkCmbMetal.ValueSeparator = ", "
        '
        'txtCustomerName
        '
        Me.txtCustomerName.Location = New System.Drawing.Point(108, 309)
        Me.txtCustomerName.Name = "txtCustomerName"
        Me.txtCustomerName.Size = New System.Drawing.Size(290, 21)
        Me.txtCustomerName.TabIndex = 22
        Me.txtCustomerName.Tag = "19"
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(108, 133)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbCostCentre.TabIndex = 10
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(18, 375)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(43, 13)
        Me.Label6.TabIndex = 25
        Me.Label6.Text = "Status"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(17, 313)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(63, 13)
        Me.Label7.TabIndex = 21
        Me.Label7.Text = "Customer"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(17, 255)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 13)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Employee"
        '
        'cmbEmpName
        '
        Me.cmbEmpName.FormattingEnabled = True
        Me.cmbEmpName.Location = New System.Drawing.Point(108, 251)
        Me.cmbEmpName.Name = "cmbEmpName"
        Me.cmbEmpName.Size = New System.Drawing.Size(290, 21)
        Me.cmbEmpName.TabIndex = 18
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(17, 284)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(53, 13)
        Me.Label13.TabIndex = 19
        Me.Label13.Text = "Counter"
        '
        'chkAsOnDate
        '
        Me.chkAsOnDate.AutoSize = True
        Me.chkAsOnDate.Location = New System.Drawing.Point(18, 54)
        Me.chkAsOnDate.Name = "chkAsOnDate"
        Me.chkAsOnDate.Size = New System.Drawing.Size(86, 17)
        Me.chkAsOnDate.TabIndex = 1
        Me.chkAsOnDate.Text = "Date From"
        Me.chkAsOnDate.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(17, 226)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(58, 13)
        Me.Label10.TabIndex = 15
        Me.Label10.Text = "Designer"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(17, 196)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(34, 13)
        Me.Label9.TabIndex = 13
        Me.Label9.Text = "Item"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(17, 167)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(37, 13)
        Me.Label8.TabIndex = 11
        Me.Label8.Text = "Metal"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(17, 138)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Cost Centre"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(17, 82)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(62, 13)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "Based On"
        '
        'rbtOrderDate
        '
        Me.rbtOrderDate.AutoSize = True
        Me.rbtOrderDate.Location = New System.Drawing.Point(108, 80)
        Me.rbtOrderDate.Name = "rbtOrderDate"
        Me.rbtOrderDate.Size = New System.Drawing.Size(89, 17)
        Me.rbtOrderDate.TabIndex = 6
        Me.rbtOrderDate.Text = "Order Date"
        Me.rbtOrderDate.UseVisualStyleBackColor = True
        '
        'rbtDueDate
        '
        Me.rbtDueDate.AutoSize = True
        Me.rbtDueDate.Location = New System.Drawing.Point(259, 80)
        Me.rbtDueDate.Name = "rbtDueDate"
        Me.rbtDueDate.Size = New System.Drawing.Size(79, 17)
        Me.rbtDueDate.TabIndex = 7
        Me.rbtDueDate.Text = "Due Date"
        Me.rbtDueDate.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(243, 51)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpTo.TabIndex = 4
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(109, 51)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(93, 21)
        Me.dtpFrom.TabIndex = 2
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(110, 529)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 31
        Me.btnSearch.Tag = "21"
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(322, 529)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 33
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(216, 529)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 32
        Me.btnNew.Tag = "22"
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(208, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(20, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "To"
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
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Controls.Add(Me.TabGeneral)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1022, 640)
        Me.tabMain.TabIndex = 0
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.Panel5)
        Me.tabView.Controls.Add(Me.Panel7)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1014, 614)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.gridView)
        Me.Panel5.Controls.Add(Me.pnlfooter)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel5.Location = New System.Drawing.Point(3, 32)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(1008, 579)
        Me.Panel5.TabIndex = 1
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 0)
        Me.gridView.Name = "gridView"
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(1008, 512)
        Me.gridView.TabIndex = 0
        '
        'pnlfooter
        '
        Me.pnlfooter.BackColor = System.Drawing.SystemColors.Menu
        Me.pnlfooter.Controls.Add(Me.lblRecSmith)
        Me.pnlfooter.Controls.Add(Me.lblRec)
        Me.pnlfooter.Controls.Add(Me.lblPendingWithUs)
        Me.pnlfooter.Controls.Add(Me.lblPending)
        Me.pnlfooter.Controls.Add(Me.lblDeliver)
        Me.pnlfooter.Controls.Add(Me.lblIsstoSmith)
        Me.pnlfooter.Controls.Add(Me.lblIss)
        Me.pnlfooter.Controls.Add(Me.lblDelivered)
        Me.pnlfooter.Controls.Add(Me.lblCancelled)
        Me.pnlfooter.Controls.Add(Me.lblCancel)
        Me.pnlfooter.Controls.Add(Me.lblApproval)
        Me.pnlfooter.Controls.Add(Me.lblAppIss)
        Me.pnlfooter.Controls.Add(Me.lblReadyforDel)
        Me.pnlfooter.Controls.Add(Me.lblReady)
        Me.pnlfooter.Controls.Add(Me.btnSendSms)
        Me.pnlfooter.Controls.Add(Me.btnBack)
        Me.pnlfooter.Controls.Add(Me.btnPrint)
        Me.pnlfooter.Controls.Add(Me.btnExport)
        Me.pnlfooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlfooter.Location = New System.Drawing.Point(0, 512)
        Me.pnlfooter.Name = "pnlfooter"
        Me.pnlfooter.Size = New System.Drawing.Size(1008, 67)
        Me.pnlfooter.TabIndex = 2
        '
        'lblRecSmith
        '
        Me.lblRecSmith.AutoSize = True
        Me.lblRecSmith.BackColor = System.Drawing.Color.Transparent
        Me.lblRecSmith.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecSmith.ForeColor = System.Drawing.Color.Blue
        Me.lblRecSmith.Location = New System.Drawing.Point(450, 14)
        Me.lblRecSmith.Name = "lblRecSmith"
        Me.lblRecSmith.Size = New System.Drawing.Size(115, 13)
        Me.lblRecSmith.TabIndex = 56
        Me.lblRecSmith.Text = "REC FROM SMITH"
        '
        'lblRec
        '
        Me.lblRec.BackColor = System.Drawing.Color.RosyBrown
        Me.lblRec.Location = New System.Drawing.Point(427, 12)
        Me.lblRec.Name = "lblRec"
        Me.lblRec.Size = New System.Drawing.Size(18, 17)
        Me.lblRec.TabIndex = 55
        '
        'lblPendingWithUs
        '
        Me.lblPendingWithUs.AutoSize = True
        Me.lblPendingWithUs.BackColor = System.Drawing.Color.Transparent
        Me.lblPendingWithUs.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPendingWithUs.ForeColor = System.Drawing.Color.Blue
        Me.lblPendingWithUs.Location = New System.Drawing.Point(309, 39)
        Me.lblPendingWithUs.Name = "lblPendingWithUs"
        Me.lblPendingWithUs.Size = New System.Drawing.Size(125, 13)
        Me.lblPendingWithUs.TabIndex = 54
        Me.lblPendingWithUs.Text = "PENDING WITH US"
        '
        'lblPending
        '
        Me.lblPending.BackColor = System.Drawing.Color.Wheat
        Me.lblPending.Location = New System.Drawing.Point(285, 38)
        Me.lblPending.Name = "lblPending"
        Me.lblPending.Size = New System.Drawing.Size(18, 17)
        Me.lblPending.TabIndex = 53
        '
        'lblDeliver
        '
        Me.lblDeliver.BackColor = System.Drawing.Color.LightGreen
        Me.lblDeliver.Location = New System.Drawing.Point(176, 38)
        Me.lblDeliver.Name = "lblDeliver"
        Me.lblDeliver.Size = New System.Drawing.Size(18, 17)
        Me.lblDeliver.TabIndex = 52
        '
        'lblIsstoSmith
        '
        Me.lblIsstoSmith.AutoSize = True
        Me.lblIsstoSmith.BackColor = System.Drawing.Color.Transparent
        Me.lblIsstoSmith.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIsstoSmith.ForeColor = System.Drawing.Color.Blue
        Me.lblIsstoSmith.Location = New System.Drawing.Point(309, 12)
        Me.lblIsstoSmith.Name = "lblIsstoSmith"
        Me.lblIsstoSmith.Size = New System.Drawing.Size(112, 13)
        Me.lblIsstoSmith.TabIndex = 51
        Me.lblIsstoSmith.Text = "ISSUE TO SMITH"
        '
        'lblIss
        '
        Me.lblIss.BackColor = System.Drawing.Color.LightPink
        Me.lblIss.Location = New System.Drawing.Point(285, 12)
        Me.lblIss.Name = "lblIss"
        Me.lblIss.Size = New System.Drawing.Size(18, 17)
        Me.lblIss.TabIndex = 50
        '
        'lblDelivered
        '
        Me.lblDelivered.AutoSize = True
        Me.lblDelivered.BackColor = System.Drawing.Color.Transparent
        Me.lblDelivered.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDelivered.ForeColor = System.Drawing.Color.Blue
        Me.lblDelivered.Location = New System.Drawing.Point(198, 39)
        Me.lblDelivered.Name = "lblDelivered"
        Me.lblDelivered.Size = New System.Drawing.Size(78, 13)
        Me.lblDelivered.TabIndex = 49
        Me.lblDelivered.Text = "DELIVERED"
        '
        'lblCancelled
        '
        Me.lblCancelled.AutoSize = True
        Me.lblCancelled.BackColor = System.Drawing.Color.Transparent
        Me.lblCancelled.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCancelled.ForeColor = System.Drawing.Color.Blue
        Me.lblCancelled.Location = New System.Drawing.Point(198, 14)
        Me.lblCancelled.Name = "lblCancelled"
        Me.lblCancelled.Size = New System.Drawing.Size(80, 13)
        Me.lblCancelled.TabIndex = 48
        Me.lblCancelled.Text = "CANCELLED"
        '
        'lblCancel
        '
        Me.lblCancel.BackColor = System.Drawing.Color.Red
        Me.lblCancel.Location = New System.Drawing.Point(176, 12)
        Me.lblCancel.Name = "lblCancel"
        Me.lblCancel.Size = New System.Drawing.Size(18, 17)
        Me.lblCancel.TabIndex = 47
        '
        'lblApproval
        '
        Me.lblApproval.AutoSize = True
        Me.lblApproval.BackColor = System.Drawing.Color.Transparent
        Me.lblApproval.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblApproval.ForeColor = System.Drawing.Color.Blue
        Me.lblApproval.Location = New System.Drawing.Point(29, 40)
        Me.lblApproval.Name = "lblApproval"
        Me.lblApproval.Size = New System.Drawing.Size(116, 13)
        Me.lblApproval.TabIndex = 46
        Me.lblApproval.Text = "APPROVAL ISSUE"
        '
        'lblAppIss
        '
        Me.lblAppIss.BackColor = System.Drawing.Color.LightSkyBlue
        Me.lblAppIss.Location = New System.Drawing.Point(8, 38)
        Me.lblAppIss.Name = "lblAppIss"
        Me.lblAppIss.Size = New System.Drawing.Size(18, 17)
        Me.lblAppIss.TabIndex = 45
        '
        'lblReadyforDel
        '
        Me.lblReadyforDel.AutoSize = True
        Me.lblReadyforDel.BackColor = System.Drawing.Color.Transparent
        Me.lblReadyforDel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReadyforDel.ForeColor = System.Drawing.Color.Blue
        Me.lblReadyforDel.Location = New System.Drawing.Point(29, 14)
        Me.lblReadyforDel.Name = "lblReadyforDel"
        Me.lblReadyforDel.Size = New System.Drawing.Size(144, 13)
        Me.lblReadyforDel.TabIndex = 44
        Me.lblReadyforDel.Text = "READY FOR DELIVERY"
        '
        'lblReady
        '
        Me.lblReady.BackColor = System.Drawing.Color.Orange
        Me.lblReady.Location = New System.Drawing.Point(8, 12)
        Me.lblReady.Name = "lblReady"
        Me.lblReady.Size = New System.Drawing.Size(18, 17)
        Me.lblReady.TabIndex = 43
        '
        'btnSendSms
        '
        Me.btnSendSms.Enabled = False
        Me.btnSendSms.Location = New System.Drawing.Point(890, 15)
        Me.btnSendSms.Name = "btnSendSms"
        Me.btnSendSms.Size = New System.Drawing.Size(100, 30)
        Me.btnSendSms.TabIndex = 6
        Me.btnSendSms.Text = "SendSms"
        Me.btnSendSms.UseVisualStyleBackColor = True
        Me.btnSendSms.Visible = False
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(571, 15)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 5
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(784, 15)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 3
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(678, 15)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 4
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'Panel7
        '
        Me.Panel7.Controls.Add(Me.lblTitle)
        Me.Panel7.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel7.Location = New System.Drawing.Point(3, 3)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(1008, 29)
        Me.Panel7.TabIndex = 3
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1008, 16)
        Me.lblTitle.TabIndex = 1
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TabGeneral
        '
        Me.TabGeneral.Controls.Add(Me.grpContainer)
        Me.TabGeneral.Location = New System.Drawing.Point(4, 22)
        Me.TabGeneral.Name = "TabGeneral"
        Me.TabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.TabGeneral.Size = New System.Drawing.Size(1014, 614)
        Me.TabGeneral.TabIndex = 0
        Me.TabGeneral.Text = "General"
        Me.TabGeneral.UseVisualStyleBackColor = True
        '
        'chkWithSummary
        '
        Me.chkWithSummary.AutoSize = True
        Me.chkWithSummary.Location = New System.Drawing.Point(301, 504)
        Me.chkWithSummary.Name = "chkWithSummary"
        Me.chkWithSummary.Size = New System.Drawing.Size(111, 17)
        Me.chkWithSummary.TabIndex = 37
        Me.chkWithSummary.Text = "With Summary"
        Me.chkWithSummary.UseVisualStyleBackColor = True
        '
        'frmOrderStatusReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1022, 640)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmOrderStatusReport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Order/Repair Status Report"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpContainer.ResumeLayout(False)
        Me.grpContainer.PerformLayout()
        Me.Panel6.ResumeLayout(False)
        Me.Panel6.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabView.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlfooter.ResumeLayout(False)
        Me.pnlfooter.PerformLayout()
        Me.Panel7.ResumeLayout(False)
        Me.TabGeneral.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents rbtTypeCustomer As System.Windows.Forms.RadioButton
    Friend WithEvents rbtTypeCompany As System.Windows.Forms.RadioButton
    Friend WithEvents rbtTypeAll As System.Windows.Forms.RadioButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents TabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkAsOnDate As System.Windows.Forms.CheckBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents rbtOrderDate As System.Windows.Forms.RadioButton
    Friend WithEvents rbtDueDate As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbEmpName As System.Windows.Forms.ComboBox
    Friend WithEvents txtCustomerName As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents rbtOrder As System.Windows.Forms.RadioButton
    Friend WithEvents rbtBoth As System.Windows.Forms.RadioButton
    Friend WithEvents rbtRepair As System.Windows.Forms.RadioButton
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbMetal As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbItem As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbDesigner As BrighttechPack.CheckedComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ChkCmbStatus As BrighttechPack.CheckedComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbOrderBy As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rbtCounter As System.Windows.Forms.RadioButton
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents rbtEmpName As System.Windows.Forms.RadioButton
    Friend WithEvents rbtWithOutGrpBy As System.Windows.Forms.RadioButton
    Friend WithEvents chkcountercmb As BrighttechPack.CheckedComboBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents chkCmbDealer As BrighttechPack.CheckedComboBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents rbtSmith As System.Windows.Forms.RadioButton
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents rbtBothRate As System.Windows.Forms.RadioButton
    Friend WithEvents rbtDeliveryRate As System.Windows.Forms.RadioButton
    Friend WithEvents rbtCurrentRate As System.Windows.Forms.RadioButton
    Friend WithEvents rbtbooking As System.Windows.Forms.RadioButton
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents pnlfooter As System.Windows.Forms.Panel
    Friend WithEvents btnSendSms As System.Windows.Forms.Button
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents Panel7 As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblApproval As System.Windows.Forms.Label
    Friend WithEvents lblAppIss As System.Windows.Forms.Label
    Friend WithEvents lblReadyforDel As System.Windows.Forms.Label
    Friend WithEvents lblReady As System.Windows.Forms.Label
    Friend WithEvents lblCancel As System.Windows.Forms.Label
    Friend WithEvents lblCancelled As System.Windows.Forms.Label
    Friend WithEvents lblRecSmith As System.Windows.Forms.Label
    Friend WithEvents lblRec As System.Windows.Forms.Label
    Friend WithEvents lblPendingWithUs As System.Windows.Forms.Label
    Friend WithEvents lblPending As System.Windows.Forms.Label
    Friend WithEvents lblDeliver As System.Windows.Forms.Label
    Friend WithEvents lblIsstoSmith As System.Windows.Forms.Label
    Friend WithEvents lblIss As System.Windows.Forms.Label
    Friend WithEvents lblDelivered As System.Windows.Forms.Label
    Friend WithEvents Panel6 As Panel
    Friend WithEvents rbtBothStatus As RadioButton
    Friend WithEvents rbtDeliveryedStatus As RadioButton
    Friend WithEvents rbtPendingStatus As RadioButton
    Friend WithEvents chkStatusMultiSelect As CheckBox
    Friend WithEvents chkSmithMultiSelect As CheckBox
    Friend WithEvents cmbStatus As ComboBox
    Friend WithEvents cmbSmith As ComboBox
    Friend WithEvents chkWithSummary As CheckBox
End Class
