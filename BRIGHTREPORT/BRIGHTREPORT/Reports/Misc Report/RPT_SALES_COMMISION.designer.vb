<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RPT_SALES_COMMISION
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
        Me.GrpContainer = New System.Windows.Forms.GroupBox()
        Me.btnEmptyComm = New System.Windows.Forms.Button()
        Me.pnlSumType = New System.Windows.Forms.Panel()
        Me.cmbViewBy = New System.Windows.Forms.ComboBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.chkStoneDetails = New System.Windows.Forms.CheckBox()
        Me.chkshowroomincentive = New System.Windows.Forms.CheckBox()
        Me.chkcmbCounter = New BrighttechPack.CheckedComboBox()
        Me.pnlrpttype = New System.Windows.Forms.Panel()
        Me.rbtDetailed = New System.Windows.Forms.RadioButton()
        Me.rbtSummary = New System.Windows.Forms.RadioButton()
        Me.chkdesc = New System.Windows.Forms.CheckBox()
        Me.chkmnthwise = New System.Windows.Forms.CheckBox()
        Me.pnlComm = New System.Windows.Forms.Panel()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtCommPercentage_AMT = New System.Windows.Forms.TextBox()
        Me.chkCmbCompany = New BrighttechPack.CheckedComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.chkcmbsubitem = New BrighttechPack.CheckedComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.chkWithTagno = New System.Windows.Forms.CheckBox()
        Me.Metal = New System.Windows.Forms.Label()
        Me.chkcmbmetal = New BrighttechPack.CheckedComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.chkcmbitemname = New BrighttechPack.CheckedComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.chkcmbemployee = New BrighttechPack.CheckedComboBox()
        Me.grppannel = New System.Windows.Forms.Panel()
        Me.rbtAll = New System.Windows.Forms.RadioButton()
        Me.rbtMetal = New System.Windows.Forms.RadioButton()
        Me.rbtproduct = New System.Windows.Forms.RadioButton()
        Me.rbtGrpEmployee = New System.Windows.Forms.RadioButton()
        Me.rbtGrpCounter = New System.Windows.Forms.RadioButton()
        Me.chkWithEmptyCommision = New System.Windows.Forms.CheckBox()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label = New System.Windows.Forms.Label()
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.GrpContainer.SuspendLayout()
        Me.pnlSumType.SuspendLayout()
        Me.pnlrpttype.SuspendLayout()
        Me.pnlComm.SuspendLayout()
        Me.grppannel.SuspendLayout()
        Me.SuspendLayout()
        '
        'GrpContainer
        '
        Me.GrpContainer.Controls.Add(Me.btnEmptyComm)
        Me.GrpContainer.Controls.Add(Me.pnlSumType)
        Me.GrpContainer.Controls.Add(Me.Label11)
        Me.GrpContainer.Controls.Add(Me.Label9)
        Me.GrpContainer.Controls.Add(Me.chkStoneDetails)
        Me.GrpContainer.Controls.Add(Me.chkshowroomincentive)
        Me.GrpContainer.Controls.Add(Me.chkcmbCounter)
        Me.GrpContainer.Controls.Add(Me.pnlrpttype)
        Me.GrpContainer.Controls.Add(Me.chkmnthwise)
        Me.GrpContainer.Controls.Add(Me.pnlComm)
        Me.GrpContainer.Controls.Add(Me.chkCmbCompany)
        Me.GrpContainer.Controls.Add(Me.Label8)
        Me.GrpContainer.Controls.Add(Me.Label7)
        Me.GrpContainer.Controls.Add(Me.chkcmbsubitem)
        Me.GrpContainer.Controls.Add(Me.Label6)
        Me.GrpContainer.Controls.Add(Me.chkWithTagno)
        Me.GrpContainer.Controls.Add(Me.Metal)
        Me.GrpContainer.Controls.Add(Me.chkcmbmetal)
        Me.GrpContainer.Controls.Add(Me.Label5)
        Me.GrpContainer.Controls.Add(Me.chkcmbitemname)
        Me.GrpContainer.Controls.Add(Me.Label4)
        Me.GrpContainer.Controls.Add(Me.chkcmbemployee)
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
        Me.GrpContainer.Location = New System.Drawing.Point(210, 28)
        Me.GrpContainer.Name = "GrpContainer"
        Me.GrpContainer.Size = New System.Drawing.Size(504, 485)
        Me.GrpContainer.TabIndex = 0
        Me.GrpContainer.TabStop = False
        '
        'btnEmptyComm
        '
        Me.btnEmptyComm.Location = New System.Drawing.Point(360, 445)
        Me.btnEmptyComm.Name = "btnEmptyComm"
        Me.btnEmptyComm.Size = New System.Drawing.Size(100, 30)
        Me.btnEmptyComm.TabIndex = 33
        Me.btnEmptyComm.Text = "Empty Comm"
        Me.btnEmptyComm.UseVisualStyleBackColor = True
        '
        'pnlSumType
        '
        Me.pnlSumType.Controls.Add(Me.cmbViewBy)
        Me.pnlSumType.Location = New System.Drawing.Point(117, 326)
        Me.pnlSumType.Name = "pnlSumType"
        Me.pnlSumType.Size = New System.Drawing.Size(273, 27)
        Me.pnlSumType.TabIndex = 23
        '
        'cmbViewBy
        '
        Me.cmbViewBy.FormattingEnabled = True
        Me.cmbViewBy.Location = New System.Drawing.Point(4, 3)
        Me.cmbViewBy.Name = "cmbViewBy"
        Me.cmbViewBy.Size = New System.Drawing.Size(234, 21)
        Me.cmbViewBy.TabIndex = 0
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(19, 328)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(94, 13)
        Me.Label11.TabIndex = 22
        Me.Label11.Text = "Summary Type"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(19, 151)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(84, 13)
        Me.Label9.TabIndex = 10
        Me.Label9.Text = "Item Counter"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkStoneDetails
        '
        Me.chkStoneDetails.AutoSize = True
        Me.chkStoneDetails.Location = New System.Drawing.Point(184, 385)
        Me.chkStoneDetails.Name = "chkStoneDetails"
        Me.chkStoneDetails.Size = New System.Drawing.Size(149, 17)
        Me.chkStoneDetails.TabIndex = 28
        Me.chkStoneDetails.Text = "Sep Col Stone details"
        Me.chkStoneDetails.UseVisualStyleBackColor = True
        '
        'chkshowroomincentive
        '
        Me.chkshowroomincentive.AutoSize = True
        Me.chkshowroomincentive.Location = New System.Drawing.Point(26, 385)
        Me.chkshowroomincentive.Name = "chkshowroomincentive"
        Me.chkshowroomincentive.Size = New System.Drawing.Size(151, 17)
        Me.chkshowroomincentive.TabIndex = 27
        Me.chkshowroomincentive.Text = "Show Room Incentive"
        Me.chkshowroomincentive.UseVisualStyleBackColor = True
        '
        'chkcmbCounter
        '
        Me.chkcmbCounter.CheckOnClick = True
        Me.chkcmbCounter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbCounter.DropDownHeight = 1
        Me.chkcmbCounter.FormattingEnabled = True
        Me.chkcmbCounter.IntegralHeight = False
        Me.chkcmbCounter.Location = New System.Drawing.Point(121, 146)
        Me.chkcmbCounter.Name = "chkcmbCounter"
        Me.chkcmbCounter.Size = New System.Drawing.Size(234, 22)
        Me.chkcmbCounter.TabIndex = 11
        Me.chkcmbCounter.ValueSeparator = ", "
        '
        'pnlrpttype
        '
        Me.pnlrpttype.Controls.Add(Me.rbtDetailed)
        Me.pnlrpttype.Controls.Add(Me.rbtSummary)
        Me.pnlrpttype.Controls.Add(Me.chkdesc)
        Me.pnlrpttype.Location = New System.Drawing.Point(121, 297)
        Me.pnlrpttype.Name = "pnlrpttype"
        Me.pnlrpttype.Size = New System.Drawing.Size(270, 23)
        Me.pnlrpttype.TabIndex = 21
        '
        'rbtDetailed
        '
        Me.rbtDetailed.AutoSize = True
        Me.rbtDetailed.Checked = True
        Me.rbtDetailed.Location = New System.Drawing.Point(86, 3)
        Me.rbtDetailed.Name = "rbtDetailed"
        Me.rbtDetailed.Size = New System.Drawing.Size(72, 17)
        Me.rbtDetailed.TabIndex = 1
        Me.rbtDetailed.TabStop = True
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
        'chkdesc
        '
        Me.chkdesc.AutoSize = True
        Me.chkdesc.Location = New System.Drawing.Point(158, 4)
        Me.chkdesc.Name = "chkdesc"
        Me.chkdesc.Size = New System.Drawing.Size(91, 17)
        Me.chkdesc.TabIndex = 2
        Me.chkdesc.Text = "Desc Order"
        Me.chkdesc.UseVisualStyleBackColor = True
        '
        'chkmnthwise
        '
        Me.chkmnthwise.AutoSize = True
        Me.chkmnthwise.Location = New System.Drawing.Point(305, 362)
        Me.chkmnthwise.Name = "chkmnthwise"
        Me.chkmnthwise.Size = New System.Drawing.Size(85, 17)
        Me.chkmnthwise.TabIndex = 26
        Me.chkmnthwise.Text = "Monthwise"
        Me.chkmnthwise.UseVisualStyleBackColor = True
        '
        'pnlComm
        '
        Me.pnlComm.Controls.Add(Me.Label10)
        Me.pnlComm.Controls.Add(Me.txtCommPercentage_AMT)
        Me.pnlComm.Location = New System.Drawing.Point(31, 406)
        Me.pnlComm.Name = "pnlComm"
        Me.pnlComm.Size = New System.Drawing.Size(361, 33)
        Me.pnlComm.TabIndex = 29
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
        'chkCmbCompany
        '
        Me.chkCmbCompany.CheckOnClick = True
        Me.chkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCompany.DropDownHeight = 1
        Me.chkCmbCompany.FormattingEnabled = True
        Me.chkCmbCompany.IntegralHeight = False
        Me.chkCmbCompany.Location = New System.Drawing.Point(121, 59)
        Me.chkCmbCompany.Name = "chkCmbCompany"
        Me.chkCmbCompany.Size = New System.Drawing.Size(234, 22)
        Me.chkCmbCompany.TabIndex = 5
        Me.chkCmbCompany.ValueSeparator = ", "
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(19, 64)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(62, 13)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "Company"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(19, 209)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(97, 13)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "Sub Item Name"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkcmbsubitem
        '
        Me.chkcmbsubitem.CheckOnClick = True
        Me.chkcmbsubitem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbsubitem.DropDownHeight = 1
        Me.chkcmbsubitem.FormattingEnabled = True
        Me.chkcmbsubitem.IntegralHeight = False
        Me.chkcmbsubitem.Location = New System.Drawing.Point(121, 204)
        Me.chkcmbsubitem.Name = "chkcmbsubitem"
        Me.chkcmbsubitem.Size = New System.Drawing.Size(234, 22)
        Me.chkcmbsubitem.TabIndex = 15
        Me.chkcmbsubitem.ValueSeparator = ", "
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(19, 297)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(76, 13)
        Me.Label6.TabIndex = 20
        Me.Label6.Text = "Report Type"
        '
        'chkWithTagno
        '
        Me.chkWithTagno.AutoSize = True
        Me.chkWithTagno.Enabled = False
        Me.chkWithTagno.Location = New System.Drawing.Point(184, 362)
        Me.chkWithTagno.Name = "chkWithTagno"
        Me.chkWithTagno.Size = New System.Drawing.Size(120, 17)
        Me.chkWithTagno.TabIndex = 25
        Me.chkWithTagno.Text = "With Total Tagno"
        Me.chkWithTagno.UseVisualStyleBackColor = True
        '
        'Metal
        '
        Me.Metal.AutoSize = True
        Me.Metal.Location = New System.Drawing.Point(19, 122)
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
        Me.chkcmbmetal.Location = New System.Drawing.Point(121, 117)
        Me.chkcmbmetal.Name = "chkcmbmetal"
        Me.chkcmbmetal.Size = New System.Drawing.Size(234, 22)
        Me.chkcmbmetal.TabIndex = 9
        Me.chkcmbmetal.ValueSeparator = ", "
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(19, 180)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(71, 13)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Item Name"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkcmbitemname
        '
        Me.chkcmbitemname.CheckOnClick = True
        Me.chkcmbitemname.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbitemname.DropDownHeight = 1
        Me.chkcmbitemname.FormattingEnabled = True
        Me.chkcmbitemname.IntegralHeight = False
        Me.chkcmbitemname.Location = New System.Drawing.Point(121, 175)
        Me.chkcmbitemname.Name = "chkcmbitemname"
        Me.chkcmbitemname.Size = New System.Drawing.Size(234, 22)
        Me.chkcmbitemname.TabIndex = 13
        Me.chkcmbitemname.ValueSeparator = ", "
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(19, 238)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(69, 13)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "Emp Name"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkcmbemployee
        '
        Me.chkcmbemployee.CheckOnClick = True
        Me.chkcmbemployee.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbemployee.DropDownHeight = 1
        Me.chkcmbemployee.FormattingEnabled = True
        Me.chkcmbemployee.IntegralHeight = False
        Me.chkcmbemployee.Location = New System.Drawing.Point(121, 233)
        Me.chkcmbemployee.Name = "chkcmbemployee"
        Me.chkcmbemployee.Size = New System.Drawing.Size(234, 22)
        Me.chkcmbemployee.TabIndex = 17
        Me.chkcmbemployee.ValueSeparator = ", "
        '
        'grppannel
        '
        Me.grppannel.Controls.Add(Me.rbtAll)
        Me.grppannel.Controls.Add(Me.rbtMetal)
        Me.grppannel.Controls.Add(Me.rbtproduct)
        Me.grppannel.Controls.Add(Me.rbtGrpEmployee)
        Me.grppannel.Controls.Add(Me.rbtGrpCounter)
        Me.grppannel.Location = New System.Drawing.Point(121, 263)
        Me.grppannel.Name = "grppannel"
        Me.grppannel.Size = New System.Drawing.Size(365, 28)
        Me.grppannel.TabIndex = 19
        '
        'rbtAll
        '
        Me.rbtAll.AutoSize = True
        Me.rbtAll.Location = New System.Drawing.Point(6, 5)
        Me.rbtAll.Name = "rbtAll"
        Me.rbtAll.Size = New System.Drawing.Size(39, 17)
        Me.rbtAll.TabIndex = 0
        Me.rbtAll.TabStop = True
        Me.rbtAll.Text = "All"
        Me.rbtAll.UseVisualStyleBackColor = True
        '
        'rbtMetal
        '
        Me.rbtMetal.AutoSize = True
        Me.rbtMetal.Location = New System.Drawing.Point(285, 5)
        Me.rbtMetal.Name = "rbtMetal"
        Me.rbtMetal.Size = New System.Drawing.Size(55, 17)
        Me.rbtMetal.TabIndex = 4
        Me.rbtMetal.TabStop = True
        Me.rbtMetal.Text = "Metal"
        Me.rbtMetal.UseVisualStyleBackColor = True
        '
        'rbtproduct
        '
        Me.rbtproduct.AutoSize = True
        Me.rbtproduct.Location = New System.Drawing.Point(136, 5)
        Me.rbtproduct.Name = "rbtproduct"
        Me.rbtproduct.Size = New System.Drawing.Size(68, 17)
        Me.rbtproduct.TabIndex = 2
        Me.rbtproduct.TabStop = True
        Me.rbtproduct.Text = "Product"
        Me.rbtproduct.UseVisualStyleBackColor = True
        '
        'rbtGrpEmployee
        '
        Me.rbtGrpEmployee.AutoSize = True
        Me.rbtGrpEmployee.Checked = True
        Me.rbtGrpEmployee.Location = New System.Drawing.Point(50, 5)
        Me.rbtGrpEmployee.Name = "rbtGrpEmployee"
        Me.rbtGrpEmployee.Size = New System.Drawing.Size(81, 17)
        Me.rbtGrpEmployee.TabIndex = 1
        Me.rbtGrpEmployee.TabStop = True
        Me.rbtGrpEmployee.Text = "Employee"
        Me.rbtGrpEmployee.UseVisualStyleBackColor = True
        '
        'rbtGrpCounter
        '
        Me.rbtGrpCounter.AutoSize = True
        Me.rbtGrpCounter.Location = New System.Drawing.Point(209, 5)
        Me.rbtGrpCounter.Name = "rbtGrpCounter"
        Me.rbtGrpCounter.Size = New System.Drawing.Size(71, 17)
        Me.rbtGrpCounter.TabIndex = 3
        Me.rbtGrpCounter.TabStop = True
        Me.rbtGrpCounter.Text = "Counter"
        Me.rbtGrpCounter.UseVisualStyleBackColor = True
        '
        'chkWithEmptyCommision
        '
        Me.chkWithEmptyCommision.AutoSize = True
        Me.chkWithEmptyCommision.Location = New System.Drawing.Point(26, 362)
        Me.chkWithEmptyCommision.Name = "chkWithEmptyCommision"
        Me.chkWithEmptyCommision.Size = New System.Drawing.Size(159, 17)
        Me.chkWithEmptyCommision.TabIndex = 24
        Me.chkWithEmptyCommision.Text = "With Empty Commision"
        Me.chkWithEmptyCommision.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(255, 445)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 32
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(45, 445)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 30
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(150, 445)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 31
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(19, 35)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(67, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Date From"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(19, 264)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 13)
        Me.Label3.TabIndex = 18
        Me.Label3.Text = "Group By"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(19, 93)
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
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(121, 88)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(234, 22)
        Me.chkCmbCostCentre.TabIndex = 7
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(210, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(20, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "To"
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(237, 31)
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
        Me.dtpFrom.Location = New System.Drawing.Point(121, 31)
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
        'RPT_SALES_COMMISION
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(881, 530)
        Me.ControlBox = False
        Me.Controls.Add(Me.GrpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "RPT_SALES_COMMISION"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "RPT_SALES_COMMISION"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GrpContainer.ResumeLayout(False)
        Me.GrpContainer.PerformLayout()
        Me.pnlSumType.ResumeLayout(False)
        Me.pnlrpttype.ResumeLayout(False)
        Me.pnlrpttype.PerformLayout()
        Me.pnlComm.ResumeLayout(False)
        Me.pnlComm.PerformLayout()
        Me.grppannel.ResumeLayout(False)
        Me.grppannel.PerformLayout()
        Me.ResumeLayout(False)

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
    Friend WithEvents chkStoneDetails As System.Windows.Forms.CheckBox
    Friend WithEvents rbtAll As System.Windows.Forms.RadioButton
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents chkcmbCounter As BrighttechPack.CheckedComboBox
    Friend WithEvents pnlSumType As System.Windows.Forms.Panel
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents cmbViewBy As System.Windows.Forms.ComboBox
    Friend WithEvents btnEmptyComm As System.Windows.Forms.Button
End Class
