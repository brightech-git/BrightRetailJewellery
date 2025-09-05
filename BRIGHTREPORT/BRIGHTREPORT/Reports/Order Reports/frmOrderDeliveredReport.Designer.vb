<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOrderDeliveredReport
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
        Me.grpContainer = New System.Windows.Forms.GroupBox
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.rbtformat2 = New System.Windows.Forms.RadioButton
        Me.rbtformat = New System.Windows.Forms.RadioButton
        Me.panelORtype = New System.Windows.Forms.Panel
        Me.rbtbooked = New System.Windows.Forms.RadioButton
        Me.rbtRepair = New System.Windows.Forms.RadioButton
        Me.rbtOrder = New System.Windows.Forms.RadioButton
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.rbtBothRate = New System.Windows.Forms.RadioButton
        Me.rbtDeliveryRate = New System.Windows.Forms.RadioButton
        Me.rbtCurrentRate = New System.Windows.Forms.RadioButton
        Me.chkCmbDealer = New BrighttechPack.CheckedComboBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.rbtSmith = New System.Windows.Forms.RadioButton
        Me.rbtWithOutGrpBy = New System.Windows.Forms.RadioButton
        Me.rbtCounter = New System.Windows.Forms.RadioButton
        Me.Label12 = New System.Windows.Forms.Label
        Me.rbtEmpName = New System.Windows.Forms.RadioButton
        Me.cmbOrderBy = New System.Windows.Forms.ComboBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.chkcountercmb = New BrighttechPack.CheckedComboBox
        Me.chkCmbDesigner = New BrighttechPack.CheckedComboBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label4 = New System.Windows.Forms.Label
        Me.rbtTypeAll = New System.Windows.Forms.RadioButton
        Me.rbtTypeCompany = New System.Windows.Forms.RadioButton
        Me.rbtTypeCustomer = New System.Windows.Forms.RadioButton
        Me.chkCmbItem = New BrighttechPack.CheckedComboBox
        Me.chkCmbMetal = New BrighttechPack.CheckedComboBox
        Me.txtCustomerName = New System.Windows.Forms.TextBox
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmbEmpName = New System.Windows.Forms.ComboBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.chkAsOnDate = New System.Windows.Forms.CheckBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.rbtOrderDate = New System.Windows.Forms.RadioButton
        Me.rbtDueDate = New System.Windows.Forms.RadioButton
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabView = New System.Windows.Forms.TabPage
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.TabGeneral = New System.Windows.Forms.TabPage
        Me.grpContainer.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.panelORtype.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabView.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabGeneral.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpContainer
        '
        Me.grpContainer.Controls.Add(Me.Panel3)
        Me.grpContainer.Controls.Add(Me.panelORtype)
        Me.grpContainer.Controls.Add(Me.Panel4)
        Me.grpContainer.Controls.Add(Me.chkCmbDealer)
        Me.grpContainer.Controls.Add(Me.Label14)
        Me.grpContainer.Controls.Add(Me.Panel2)
        Me.grpContainer.Controls.Add(Me.cmbOrderBy)
        Me.grpContainer.Controls.Add(Me.Label11)
        Me.grpContainer.Controls.Add(Me.chkcountercmb)
        Me.grpContainer.Controls.Add(Me.chkCmbDesigner)
        Me.grpContainer.Controls.Add(Me.Panel1)
        Me.grpContainer.Controls.Add(Me.chkCmbItem)
        Me.grpContainer.Controls.Add(Me.chkCmbMetal)
        Me.grpContainer.Controls.Add(Me.txtCustomerName)
        Me.grpContainer.Controls.Add(Me.chkCmbCostCentre)
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
        Me.grpContainer.Location = New System.Drawing.Point(238, 35)
        Me.grpContainer.Name = "grpContainer"
        Me.grpContainer.Size = New System.Drawing.Size(440, 527)
        Me.grpContainer.TabIndex = 0
        Me.grpContainer.TabStop = False
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.rbtformat2)
        Me.Panel3.Controls.Add(Me.rbtformat)
        Me.Panel3.Location = New System.Drawing.Point(109, 448)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(179, 30)
        Me.Panel3.TabIndex = 30
        '
        'rbtformat2
        '
        Me.rbtformat2.AutoSize = True
        Me.rbtformat2.Location = New System.Drawing.Point(94, 6)
        Me.rbtformat2.Name = "rbtformat2"
        Me.rbtformat2.Size = New System.Drawing.Size(76, 17)
        Me.rbtformat2.TabIndex = 33
        Me.rbtformat2.Text = "Format 2"
        Me.rbtformat2.UseVisualStyleBackColor = True
        '
        'rbtformat
        '
        Me.rbtformat.AutoSize = True
        Me.rbtformat.Checked = True
        Me.rbtformat.Location = New System.Drawing.Point(5, 7)
        Me.rbtformat.Name = "rbtformat"
        Me.rbtformat.Size = New System.Drawing.Size(76, 17)
        Me.rbtformat.TabIndex = 29
        Me.rbtformat.TabStop = True
        Me.rbtformat.Text = "Format 1" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.rbtformat.UseVisualStyleBackColor = True
        '
        'panelORtype
        '
        Me.panelORtype.Controls.Add(Me.rbtbooked)
        Me.panelORtype.Controls.Add(Me.rbtRepair)
        Me.panelORtype.Controls.Add(Me.rbtOrder)
        Me.panelORtype.Location = New System.Drawing.Point(6, 41)
        Me.panelORtype.Name = "panelORtype"
        Me.panelORtype.Size = New System.Drawing.Size(428, 23)
        Me.panelORtype.TabIndex = 4
        '
        'rbtbooked
        '
        Me.rbtbooked.AutoSize = True
        Me.rbtbooked.Location = New System.Drawing.Point(273, 3)
        Me.rbtbooked.Name = "rbtbooked"
        Me.rbtbooked.Size = New System.Drawing.Size(68, 17)
        Me.rbtbooked.TabIndex = 2
        Me.rbtbooked.Text = "Booked"
        Me.rbtbooked.UseVisualStyleBackColor = True
        '
        'rbtRepair
        '
        Me.rbtRepair.AutoSize = True
        Me.rbtRepair.Location = New System.Drawing.Point(197, 2)
        Me.rbtRepair.Name = "rbtRepair"
        Me.rbtRepair.Size = New System.Drawing.Size(62, 17)
        Me.rbtRepair.TabIndex = 1
        Me.rbtRepair.Text = "Repair"
        Me.rbtRepair.UseVisualStyleBackColor = True
        '
        'rbtOrder
        '
        Me.rbtOrder.AutoSize = True
        Me.rbtOrder.Checked = True
        Me.rbtOrder.Location = New System.Drawing.Point(103, 3)
        Me.rbtOrder.Name = "rbtOrder"
        Me.rbtOrder.Size = New System.Drawing.Size(58, 17)
        Me.rbtOrder.TabIndex = 0
        Me.rbtOrder.TabStop = True
        Me.rbtOrder.Text = "Order"
        Me.rbtOrder.UseVisualStyleBackColor = True
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.rbtBothRate)
        Me.Panel4.Controls.Add(Me.rbtDeliveryRate)
        Me.Panel4.Controls.Add(Me.rbtCurrentRate)
        Me.Panel4.Location = New System.Drawing.Point(108, 357)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(315, 30)
        Me.Panel4.TabIndex = 25
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
        Me.chkCmbDealer.Location = New System.Drawing.Point(108, 329)
        Me.chkCmbDealer.Name = "chkCmbDealer"
        Me.chkCmbDealer.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbDealer.TabIndex = 24
        Me.chkCmbDealer.ValueSeparator = ", "
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(18, 332)
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
        Me.Panel2.Location = New System.Drawing.Point(6, 391)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(416, 30)
        Me.Panel2.TabIndex = 26
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
        Me.cmbOrderBy.Location = New System.Drawing.Point(108, 427)
        Me.cmbOrderBy.Name = "cmbOrderBy"
        Me.cmbOrderBy.Size = New System.Drawing.Size(290, 21)
        Me.cmbOrderBy.TabIndex = 28
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(17, 430)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(59, 13)
        Me.Label11.TabIndex = 27
        Me.Label11.Text = "Order By"
        '
        'chkcountercmb
        '
        Me.chkcountercmb.CheckOnClick = True
        Me.chkcountercmb.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcountercmb.DropDownHeight = 1
        Me.chkcountercmb.FormattingEnabled = True
        Me.chkcountercmb.IntegralHeight = False
        Me.chkcountercmb.Location = New System.Drawing.Point(108, 266)
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
        Me.chkCmbDesigner.Location = New System.Drawing.Point(108, 207)
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
        Me.Panel1.Location = New System.Drawing.Point(6, 91)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(428, 22)
        Me.Panel1.TabIndex = 8
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(11, 3)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(35, 13)
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
        Me.chkCmbItem.Location = New System.Drawing.Point(108, 177)
        Me.chkCmbItem.Name = "chkCmbItem"
        Me.chkCmbItem.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbItem.TabIndex = 14
        Me.chkCmbItem.ValueSeparator = ", "
        '
        'chkCmbMetal
        '
        Me.chkCmbMetal.CheckOnClick = True
        Me.chkCmbMetal.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbMetal.DropDownHeight = 1
        Me.chkCmbMetal.FormattingEnabled = True
        Me.chkCmbMetal.IntegralHeight = False
        Me.chkCmbMetal.Location = New System.Drawing.Point(108, 148)
        Me.chkCmbMetal.Name = "chkCmbMetal"
        Me.chkCmbMetal.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbMetal.TabIndex = 12
        Me.chkCmbMetal.ValueSeparator = ", "
        '
        'txtCustomerName
        '
        Me.txtCustomerName.Location = New System.Drawing.Point(108, 296)
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
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(108, 120)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbCostCentre.TabIndex = 10
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(17, 300)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(63, 13)
        Me.Label7.TabIndex = 21
        Me.Label7.Text = "Customer"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(17, 242)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 13)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Employee"
        '
        'cmbEmpName
        '
        Me.cmbEmpName.FormattingEnabled = True
        Me.cmbEmpName.Location = New System.Drawing.Point(108, 238)
        Me.cmbEmpName.Name = "cmbEmpName"
        Me.cmbEmpName.Size = New System.Drawing.Size(290, 21)
        Me.cmbEmpName.TabIndex = 18
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(17, 271)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(53, 13)
        Me.Label13.TabIndex = 19
        Me.Label13.Text = "Counter"
        '
        'chkAsOnDate
        '
        Me.chkAsOnDate.AutoSize = True
        Me.chkAsOnDate.Location = New System.Drawing.Point(18, 19)
        Me.chkAsOnDate.Name = "chkAsOnDate"
        Me.chkAsOnDate.Size = New System.Drawing.Size(86, 17)
        Me.chkAsOnDate.TabIndex = 0
        Me.chkAsOnDate.Text = "Date From"
        Me.chkAsOnDate.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(17, 212)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(58, 13)
        Me.Label10.TabIndex = 15
        Me.Label10.Text = "Designer"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(17, 182)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(34, 13)
        Me.Label9.TabIndex = 13
        Me.Label9.Text = "Item"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(17, 153)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(37, 13)
        Me.Label8.TabIndex = 11
        Me.Label8.Text = "Metal"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(17, 125)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Cost Centre"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(17, 67)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(62, 13)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "Based On"
        '
        'rbtOrderDate
        '
        Me.rbtOrderDate.AutoSize = True
        Me.rbtOrderDate.Location = New System.Drawing.Point(108, 65)
        Me.rbtOrderDate.Name = "rbtOrderDate"
        Me.rbtOrderDate.Size = New System.Drawing.Size(89, 17)
        Me.rbtOrderDate.TabIndex = 6
        Me.rbtOrderDate.Text = "Order Date"
        Me.rbtOrderDate.UseVisualStyleBackColor = True
        '
        'rbtDueDate
        '
        Me.rbtDueDate.AutoSize = True
        Me.rbtDueDate.Location = New System.Drawing.Point(203, 65)
        Me.rbtDueDate.Name = "rbtDueDate"
        Me.rbtDueDate.Size = New System.Drawing.Size(79, 17)
        Me.rbtDueDate.TabIndex = 7
        Me.rbtDueDate.Text = "Due Date"
        Me.rbtDueDate.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(243, 16)
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
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(109, 16)
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
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(105, 479)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 31
        Me.btnSearch.Tag = "21"
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(317, 479)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 33
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(211, 479)
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
        Me.Label2.Location = New System.Drawing.Point(208, 20)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(21, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
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
        Me.tabView.Controls.Add(Me.gridView)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1014, 614)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Location = New System.Drawing.Point(31, 30)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.Size = New System.Drawing.Size(957, 459)
        Me.gridView.TabIndex = 0
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
        'frmOrderDeliveredReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1022, 640)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmOrderDeliveredReport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Order\Repair Delivered Report"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpContainer.ResumeLayout(False)
        Me.grpContainer.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.panelORtype.ResumeLayout(False)
        Me.panelORtype.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabView.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbMetal As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbItem As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbDesigner As BrighttechPack.CheckedComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
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
    Friend WithEvents panelORtype As System.Windows.Forms.Panel
    Friend WithEvents rbtRepair As System.Windows.Forms.RadioButton
    Friend WithEvents rbtOrder As System.Windows.Forms.RadioButton
    Friend WithEvents rbtbooked As System.Windows.Forms.RadioButton
    Friend WithEvents rbtformat2 As System.Windows.Forms.RadioButton
    Friend WithEvents rbtformat As System.Windows.Forms.RadioButton
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
End Class
