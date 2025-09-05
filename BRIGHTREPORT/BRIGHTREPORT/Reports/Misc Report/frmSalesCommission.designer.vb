<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSalesCommission
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
        Me.GrpContainer = New System.Windows.Forms.GroupBox
        Me.chkLstEmployee = New System.Windows.Forms.CheckedListBox
        Me.chkEmpAll = New System.Windows.Forms.CheckBox
        Me.chkLstSubItem = New System.Windows.Forms.CheckedListBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.chkSubItemAll = New System.Windows.Forms.CheckBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.chkLstItem = New System.Windows.Forms.CheckedListBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.chkItemAll = New System.Windows.Forms.CheckBox
        Me.chkshowroomincentive = New System.Windows.Forms.CheckBox
        Me.pnlrpttype = New System.Windows.Forms.Panel
        Me.rbtDetailed = New System.Windows.Forms.RadioButton
        Me.rbtSummary = New System.Windows.Forms.RadioButton
        Me.chkCmbCompany = New BrighttechPack.CheckedComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Metal = New System.Windows.Forms.Label
        Me.chkcmbmetal = New BrighttechPack.CheckedComboBox
        Me.grppannel = New System.Windows.Forms.Panel
        Me.rbtMetal = New System.Windows.Forms.RadioButton
        Me.rbtproduct = New System.Windows.Forms.RadioButton
        Me.rbtGrpEmployee = New System.Windows.Forms.RadioButton
        Me.chkWithEmptyCommision = New System.Windows.Forms.CheckBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label = New System.Windows.Forms.Label
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.chkdesc = New System.Windows.Forms.CheckBox
        Me.chkmnthwise = New System.Windows.Forms.CheckBox
        Me.chkWithTagno = New System.Windows.Forms.CheckBox
        Me.rbtGrpCounter = New System.Windows.Forms.RadioButton
        Me.pnlComm = New System.Windows.Forms.Panel
        Me.Label10 = New System.Windows.Forms.Label
        Me.txtCommPercentage_AMT = New System.Windows.Forms.TextBox
        Me.chkcmbsubitem = New BrighttechPack.CheckedComboBox
        Me.chkcmbitemname = New BrighttechPack.CheckedComboBox
        Me.chkcmbemployee = New BrighttechPack.CheckedComboBox
        Me.rbtTarget = New System.Windows.Forms.RadioButton
        Me.GrpContainer.SuspendLayout()
        Me.pnlrpttype.SuspendLayout()
        Me.grppannel.SuspendLayout()
        Me.pnlComm.SuspendLayout()
        Me.SuspendLayout()
        '
        'GrpContainer
        '
        Me.GrpContainer.Controls.Add(Me.chkLstEmployee)
        Me.GrpContainer.Controls.Add(Me.chkEmpAll)
        Me.GrpContainer.Controls.Add(Me.chkLstSubItem)
        Me.GrpContainer.Controls.Add(Me.Label4)
        Me.GrpContainer.Controls.Add(Me.chkSubItemAll)
        Me.GrpContainer.Controls.Add(Me.Label7)
        Me.GrpContainer.Controls.Add(Me.chkLstItem)
        Me.GrpContainer.Controls.Add(Me.Label5)
        Me.GrpContainer.Controls.Add(Me.chkItemAll)
        Me.GrpContainer.Controls.Add(Me.chkshowroomincentive)
        Me.GrpContainer.Controls.Add(Me.pnlrpttype)
        Me.GrpContainer.Controls.Add(Me.chkCmbCompany)
        Me.GrpContainer.Controls.Add(Me.Label8)
        Me.GrpContainer.Controls.Add(Me.Label6)
        Me.GrpContainer.Controls.Add(Me.Metal)
        Me.GrpContainer.Controls.Add(Me.chkcmbmetal)
        Me.GrpContainer.Controls.Add(Me.grppannel)
        Me.GrpContainer.Controls.Add(Me.chkWithEmptyCommision)
        Me.GrpContainer.Controls.Add(Me.btnExit)
        Me.GrpContainer.Controls.Add(Me.btnSearch)
        Me.GrpContainer.Controls.Add(Me.btnNew)
        Me.GrpContainer.Controls.Add(Me.Label2)
        Me.GrpContainer.Controls.Add(Me.Label3)
        Me.GrpContainer.Controls.Add(Me.Label)
        Me.GrpContainer.Controls.Add(Me.chkCmbCostCentre)
        Me.GrpContainer.Controls.Add(Me.Label1)
        Me.GrpContainer.Controls.Add(Me.dtpTo)
        Me.GrpContainer.Controls.Add(Me.dtpFrom)
        Me.GrpContainer.Location = New System.Drawing.Point(263, 28)
        Me.GrpContainer.Name = "GrpContainer"
        Me.GrpContainer.Size = New System.Drawing.Size(425, 567)
        Me.GrpContainer.TabIndex = 0
        Me.GrpContainer.TabStop = False
        '
        'chkLstEmployee
        '
        Me.chkLstEmployee.FormattingEnabled = True
        Me.chkLstEmployee.Location = New System.Drawing.Point(116, 326)
        Me.chkLstEmployee.Name = "chkLstEmployee"
        Me.chkLstEmployee.Size = New System.Drawing.Size(237, 68)
        Me.chkLstEmployee.TabIndex = 18
        '
        'chkEmpAll
        '
        Me.chkEmpAll.AutoSize = True
        Me.chkEmpAll.Location = New System.Drawing.Point(119, 309)
        Me.chkEmpAll.Name = "chkEmpAll"
        Me.chkEmpAll.Size = New System.Drawing.Size(115, 17)
        Me.chkEmpAll.TabIndex = 17
        Me.chkEmpAll.Text = "EmployeeName"
        Me.chkEmpAll.UseVisualStyleBackColor = True
        '
        'chkLstSubItem
        '
        Me.chkLstSubItem.FormattingEnabled = True
        Me.chkLstSubItem.Location = New System.Drawing.Point(116, 237)
        Me.chkLstSubItem.Name = "chkLstSubItem"
        Me.chkLstSubItem.Size = New System.Drawing.Size(237, 68)
        Me.chkLstSubItem.TabIndex = 15
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(15, 326)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(69, 13)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "Emp Name"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkSubItemAll
        '
        Me.chkSubItemAll.AutoSize = True
        Me.chkSubItemAll.Location = New System.Drawing.Point(119, 220)
        Me.chkSubItemAll.Name = "chkSubItemAll"
        Me.chkSubItemAll.Size = New System.Drawing.Size(108, 17)
        Me.chkSubItemAll.TabIndex = 14
        Me.chkSubItemAll.Text = "SubItemName"
        Me.chkSubItemAll.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(15, 237)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(93, 13)
        Me.Label7.TabIndex = 13
        Me.Label7.Text = "Sub ItemName"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkLstItem
        '
        Me.chkLstItem.FormattingEnabled = True
        Me.chkLstItem.Location = New System.Drawing.Point(116, 148)
        Me.chkLstItem.Name = "chkLstItem"
        Me.chkLstItem.Size = New System.Drawing.Size(237, 68)
        Me.chkLstItem.TabIndex = 12
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(15, 148)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(71, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Item Name"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkItemAll
        '
        Me.chkItemAll.AutoSize = True
        Me.chkItemAll.Location = New System.Drawing.Point(119, 131)
        Me.chkItemAll.Name = "chkItemAll"
        Me.chkItemAll.Size = New System.Drawing.Size(86, 17)
        Me.chkItemAll.TabIndex = 11
        Me.chkItemAll.Text = "ItemName"
        Me.chkItemAll.UseVisualStyleBackColor = True
        '
        'chkshowroomincentive
        '
        Me.chkshowroomincentive.AutoSize = True
        Me.chkshowroomincentive.Location = New System.Drawing.Point(118, 491)
        Me.chkshowroomincentive.Name = "chkshowroomincentive"
        Me.chkshowroomincentive.Size = New System.Drawing.Size(143, 17)
        Me.chkshowroomincentive.TabIndex = 24
        Me.chkshowroomincentive.Text = "ShowRoomIncentive"
        Me.chkshowroomincentive.UseVisualStyleBackColor = True
        '
        'pnlrpttype
        '
        Me.pnlrpttype.Controls.Add(Me.rbtTarget)
        Me.pnlrpttype.Controls.Add(Me.rbtDetailed)
        Me.pnlrpttype.Controls.Add(Me.rbtSummary)
        Me.pnlrpttype.Location = New System.Drawing.Point(118, 435)
        Me.pnlrpttype.Name = "pnlrpttype"
        Me.pnlrpttype.Size = New System.Drawing.Size(273, 23)
        Me.pnlrpttype.TabIndex = 22
        '
        'rbtDetailed
        '
        Me.rbtDetailed.AutoSize = True
        Me.rbtDetailed.Checked = True
        Me.rbtDetailed.Location = New System.Drawing.Point(86, 1)
        Me.rbtDetailed.Name = "rbtDetailed"
        Me.rbtDetailed.Size = New System.Drawing.Size(72, 17)
        Me.rbtDetailed.TabIndex = 1
        Me.rbtDetailed.Text = "Detailed"
        Me.rbtDetailed.UseVisualStyleBackColor = True
        '
        'rbtSummary
        '
        Me.rbtSummary.AutoSize = True
        Me.rbtSummary.Location = New System.Drawing.Point(5, 1)
        Me.rbtSummary.Name = "rbtSummary"
        Me.rbtSummary.Size = New System.Drawing.Size(81, 17)
        Me.rbtSummary.TabIndex = 0
        Me.rbtSummary.Text = "Summary"
        Me.rbtSummary.UseVisualStyleBackColor = True
        '
        'chkCmbCompany
        '
        Me.chkCmbCompany.CheckOnClick = True
        Me.chkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCompany.DropDownHeight = 1
        Me.chkCmbCompany.FormattingEnabled = True
        Me.chkCmbCompany.IntegralHeight = False
        Me.chkCmbCompany.Location = New System.Drawing.Point(119, 50)
        Me.chkCmbCompany.Name = "chkCmbCompany"
        Me.chkCmbCompany.Size = New System.Drawing.Size(234, 22)
        Me.chkCmbCompany.TabIndex = 5
        Me.chkCmbCompany.ValueSeparator = ", "
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(15, 53)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(62, 13)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "Company"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(18, 440)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(77, 13)
        Me.Label6.TabIndex = 21
        Me.Label6.Text = "Report Type"
        '
        'Metal
        '
        Me.Metal.AutoSize = True
        Me.Metal.Location = New System.Drawing.Point(15, 109)
        Me.Metal.Name = "Metal"
        Me.Metal.Size = New System.Drawing.Size(37, 13)
        Me.Metal.TabIndex = 8
        Me.Metal.Text = "Metal"
        Me.Metal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkcmbmetal
        '
        Me.chkcmbmetal.CheckOnClick = True
        Me.chkcmbmetal.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbmetal.DropDownHeight = 1
        Me.chkcmbmetal.FormattingEnabled = True
        Me.chkcmbmetal.IntegralHeight = False
        Me.chkcmbmetal.Location = New System.Drawing.Point(119, 104)
        Me.chkcmbmetal.Name = "chkcmbmetal"
        Me.chkcmbmetal.Size = New System.Drawing.Size(234, 22)
        Me.chkcmbmetal.TabIndex = 9
        Me.chkcmbmetal.ValueSeparator = ", "
        '
        'grppannel
        '
        Me.grppannel.Controls.Add(Me.rbtMetal)
        Me.grppannel.Controls.Add(Me.rbtproduct)
        Me.grppannel.Controls.Add(Me.rbtGrpEmployee)
        Me.grppannel.Location = New System.Drawing.Point(119, 401)
        Me.grppannel.Name = "grppannel"
        Me.grppannel.Size = New System.Drawing.Size(272, 24)
        Me.grppannel.TabIndex = 20
        '
        'rbtMetal
        '
        Me.rbtMetal.AutoSize = True
        Me.rbtMetal.Location = New System.Drawing.Point(159, 4)
        Me.rbtMetal.Name = "rbtMetal"
        Me.rbtMetal.Size = New System.Drawing.Size(55, 17)
        Me.rbtMetal.TabIndex = 3
        Me.rbtMetal.Text = "Metal"
        Me.rbtMetal.UseVisualStyleBackColor = True
        '
        'rbtproduct
        '
        Me.rbtproduct.AutoSize = True
        Me.rbtproduct.Location = New System.Drawing.Point(85, 4)
        Me.rbtproduct.Name = "rbtproduct"
        Me.rbtproduct.Size = New System.Drawing.Size(68, 17)
        Me.rbtproduct.TabIndex = 1
        Me.rbtproduct.Text = "Product"
        Me.rbtproduct.UseVisualStyleBackColor = True
        '
        'rbtGrpEmployee
        '
        Me.rbtGrpEmployee.AutoSize = True
        Me.rbtGrpEmployee.Checked = True
        Me.rbtGrpEmployee.Location = New System.Drawing.Point(3, 3)
        Me.rbtGrpEmployee.Name = "rbtGrpEmployee"
        Me.rbtGrpEmployee.Size = New System.Drawing.Size(81, 17)
        Me.rbtGrpEmployee.TabIndex = 0
        Me.rbtGrpEmployee.TabStop = True
        Me.rbtGrpEmployee.Text = "Employee"
        Me.rbtGrpEmployee.UseVisualStyleBackColor = True
        '
        'chkWithEmptyCommision
        '
        Me.chkWithEmptyCommision.AutoSize = True
        Me.chkWithEmptyCommision.Location = New System.Drawing.Point(118, 468)
        Me.chkWithEmptyCommision.Name = "chkWithEmptyCommision"
        Me.chkWithEmptyCommision.Size = New System.Drawing.Size(159, 17)
        Me.chkWithEmptyCommision.TabIndex = 23
        Me.chkWithEmptyCommision.Text = "With Empty Commision"
        Me.chkWithEmptyCommision.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(262, 517)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 27
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(52, 517)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 25
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(156, 517)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 26
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(15, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(67, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Date From"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(17, 408)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 13)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "Group By"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(15, 82)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(76, 13)
        Me.Label.TabIndex = 6
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
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(119, 77)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(234, 22)
        Me.chkCmbCostCentre.TabIndex = 7
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(208, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(21, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "To"
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(235, 22)
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
        Me.dtpFrom.Location = New System.Drawing.Point(119, 22)
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
        'chkdesc
        '
        Me.chkdesc.AutoSize = True
        Me.chkdesc.Location = New System.Drawing.Point(67, 544)
        Me.chkdesc.Name = "chkdesc"
        Me.chkdesc.Size = New System.Drawing.Size(91, 17)
        Me.chkdesc.TabIndex = 2
        Me.chkdesc.Text = "Desc Order"
        Me.chkdesc.UseVisualStyleBackColor = True
        Me.chkdesc.Visible = False
        '
        'chkmnthwise
        '
        Me.chkmnthwise.AutoSize = True
        Me.chkmnthwise.Location = New System.Drawing.Point(67, 586)
        Me.chkmnthwise.Name = "chkmnthwise"
        Me.chkmnthwise.Size = New System.Drawing.Size(85, 17)
        Me.chkmnthwise.TabIndex = 25
        Me.chkmnthwise.Text = "Monthwise"
        Me.chkmnthwise.UseVisualStyleBackColor = True
        Me.chkmnthwise.Visible = False
        '
        'chkWithTagno
        '
        Me.chkWithTagno.AutoSize = True
        Me.chkWithTagno.Enabled = False
        Me.chkWithTagno.Location = New System.Drawing.Point(50, 521)
        Me.chkWithTagno.Name = "chkWithTagno"
        Me.chkWithTagno.Size = New System.Drawing.Size(122, 17)
        Me.chkWithTagno.TabIndex = 24
        Me.chkWithTagno.Text = "With Total Tagno"
        Me.chkWithTagno.UseVisualStyleBackColor = True
        Me.chkWithTagno.Visible = False
        '
        'rbtGrpCounter
        '
        Me.rbtGrpCounter.AutoSize = True
        Me.rbtGrpCounter.Location = New System.Drawing.Point(33, 504)
        Me.rbtGrpCounter.Name = "rbtGrpCounter"
        Me.rbtGrpCounter.Size = New System.Drawing.Size(71, 17)
        Me.rbtGrpCounter.TabIndex = 2
        Me.rbtGrpCounter.Text = "Counter"
        Me.rbtGrpCounter.UseVisualStyleBackColor = True
        Me.rbtGrpCounter.Visible = False
        '
        'pnlComm
        '
        Me.pnlComm.Controls.Add(Me.Label10)
        Me.pnlComm.Controls.Add(Me.txtCommPercentage_AMT)
        Me.pnlComm.Location = New System.Drawing.Point(43, 442)
        Me.pnlComm.Name = "pnlComm"
        Me.pnlComm.Size = New System.Drawing.Size(214, 33)
        Me.pnlComm.TabIndex = 27
        Me.pnlComm.Visible = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(4, 9)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(111, 13)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "Counter Comm %"
        '
        'txtCommPercentage_AMT
        '
        Me.txtCommPercentage_AMT.Location = New System.Drawing.Point(122, 5)
        Me.txtCommPercentage_AMT.Name = "txtCommPercentage_AMT"
        Me.txtCommPercentage_AMT.Size = New System.Drawing.Size(85, 21)
        Me.txtCommPercentage_AMT.TabIndex = 1
        '
        'chkcmbsubitem
        '
        Me.chkcmbsubitem.CheckOnClick = True
        Me.chkcmbsubitem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbsubitem.DropDownHeight = 1
        Me.chkcmbsubitem.FormattingEnabled = True
        Me.chkcmbsubitem.IntegralHeight = False
        Me.chkcmbsubitem.Location = New System.Drawing.Point(106, 341)
        Me.chkcmbsubitem.Name = "chkcmbsubitem"
        Me.chkcmbsubitem.Size = New System.Drawing.Size(151, 22)
        Me.chkcmbsubitem.TabIndex = 13
        Me.chkcmbsubitem.ValueSeparator = ", "
        Me.chkcmbsubitem.Visible = False
        '
        'chkcmbitemname
        '
        Me.chkcmbitemname.CheckOnClick = True
        Me.chkcmbitemname.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbitemname.DropDownHeight = 1
        Me.chkcmbitemname.FormattingEnabled = True
        Me.chkcmbitemname.IntegralHeight = False
        Me.chkcmbitemname.Location = New System.Drawing.Point(106, 311)
        Me.chkcmbitemname.Name = "chkcmbitemname"
        Me.chkcmbitemname.Size = New System.Drawing.Size(151, 22)
        Me.chkcmbitemname.TabIndex = 11
        Me.chkcmbitemname.ValueSeparator = ", "
        Me.chkcmbitemname.Visible = False
        '
        'chkcmbemployee
        '
        Me.chkcmbemployee.CheckOnClick = True
        Me.chkcmbemployee.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbemployee.DropDownHeight = 1
        Me.chkcmbemployee.FormattingEnabled = True
        Me.chkcmbemployee.IntegralHeight = False
        Me.chkcmbemployee.Location = New System.Drawing.Point(106, 369)
        Me.chkcmbemployee.Name = "chkcmbemployee"
        Me.chkcmbemployee.Size = New System.Drawing.Size(151, 22)
        Me.chkcmbemployee.TabIndex = 15
        Me.chkcmbemployee.ValueSeparator = ", "
        Me.chkcmbemployee.Visible = False
        '
        'rbtTarget
        '
        Me.rbtTarget.AutoSize = True
        Me.rbtTarget.Location = New System.Drawing.Point(159, 1)
        Me.rbtTarget.Name = "rbtTarget"
        Me.rbtTarget.Size = New System.Drawing.Size(115, 17)
        Me.rbtTarget.TabIndex = 2
        Me.rbtTarget.Text = "Target Vs Sales"
        Me.rbtTarget.UseVisualStyleBackColor = True
        '
        'RPT_SALES_COMMISION
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(881, 640)
        Me.ControlBox = False
        Me.Controls.Add(Me.GrpContainer)
        Me.Controls.Add(Me.chkcmbitemname)
        Me.Controls.Add(Me.chkdesc)
        Me.Controls.Add(Me.chkcmbemployee)
        Me.Controls.Add(Me.rbtGrpCounter)
        Me.Controls.Add(Me.chkcmbsubitem)
        Me.Controls.Add(Me.pnlComm)
        Me.Controls.Add(Me.chkmnthwise)
        Me.Controls.Add(Me.chkWithTagno)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmSalesCommission"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Sales Commission"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GrpContainer.ResumeLayout(False)
        Me.GrpContainer.PerformLayout()
        Me.pnlrpttype.ResumeLayout(False)
        Me.pnlrpttype.PerformLayout()
        Me.grppannel.ResumeLayout(False)
        Me.grppannel.PerformLayout()
        Me.pnlComm.ResumeLayout(False)
        Me.pnlComm.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GrpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents rbtDetailed As System.Windows.Forms.RadioButton
    Friend WithEvents rbtSummary As System.Windows.Forms.RadioButton
    Friend WithEvents Label As System.Windows.Forms.Label
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkWithEmptyCommision As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents chkcmbitemname As BrighttechPack.CheckedComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents chkcmbemployee As BrighttechPack.CheckedComboBox
    Friend WithEvents Metal As System.Windows.Forms.Label
    Friend WithEvents chkcmbmetal As BrighttechPack.CheckedComboBox
    Friend WithEvents chkWithTagno As System.Windows.Forms.CheckBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents grppannel As System.Windows.Forms.Panel
    Friend WithEvents rbtproduct As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGrpEmployee As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGrpCounter As System.Windows.Forms.RadioButton
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents chkcmbsubitem As BrighttechPack.CheckedComboBox
    Friend WithEvents chkdesc As System.Windows.Forms.CheckBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents chkCmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtCommPercentage_AMT As System.Windows.Forms.TextBox
    Friend WithEvents pnlComm As System.Windows.Forms.Panel
    Friend WithEvents rbtMetal As System.Windows.Forms.RadioButton
    Friend WithEvents chkmnthwise As System.Windows.Forms.CheckBox
    Friend WithEvents pnlrpttype As System.Windows.Forms.Panel
    Friend WithEvents chkshowroomincentive As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstItem As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkItemAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstEmployee As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkEmpAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstSubItem As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkSubItemAll As System.Windows.Forms.CheckBox
    Friend WithEvents rbtTarget As System.Windows.Forms.RadioButton
End Class
