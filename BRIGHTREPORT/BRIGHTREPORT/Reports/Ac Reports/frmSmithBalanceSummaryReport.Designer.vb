<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmSmithBalanceSummaryReport
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
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.chkCostCentreSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstCategory = New System.Windows.Forms.CheckedListBox()
        Me.chkCategorySelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstMetal = New System.Windows.Forms.CheckedListBox()
        Me.chkMetalSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.pnlContainer = New System.Windows.Forms.Panel()
        Me.chkMIMRApproval = New System.Windows.Forms.CheckBox()
        Me.chkDiaWtConvert = New System.Windows.Forms.CheckBox()
        Me.ChkWithDirectPur = New System.Windows.Forms.CheckBox()
        Me.chkSales = New System.Windows.Forms.CheckBox()
        Me.chkGroupByAcType = New System.Windows.Forms.CheckBox()
        Me.chkWithItemDet = New System.Windows.Forms.CheckBox()
        Me.chkStnWtConv = New System.Windows.Forms.CheckBox()
        Me.chkWithAppAmt = New System.Windows.Forms.CheckBox()
        Me.ChkwithWast = New System.Windows.Forms.CheckBox()
        Me.ChkPureWt = New System.Windows.Forms.CheckBox()
        Me.ChkNetWt = New System.Windows.Forms.CheckBox()
        Me.ChkGrossWt = New System.Windows.Forms.CheckBox()
        Me.chkWithSpecicFormat = New System.Windows.Forms.CheckBox()
        Me.cmbactive = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ChkRelatedtran = New System.Windows.Forms.CheckBox()
        Me.CmbAcname = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ChkApproval = New System.Windows.Forms.CheckBox()
        Me.PnlMark = New System.Windows.Forms.Panel()
        Me.rbtLocal = New System.Windows.Forms.RadioButton()
        Me.rbtOutstation = New System.Windows.Forms.RadioButton()
        Me.rbtBothMU = New System.Windows.Forms.RadioButton()
        Me.chkAmtbal = New System.Windows.Forms.CheckBox()
        Me.dtpTodate = New BrighttechPack.DatePicker(Me.components)
        Me.chkWithAbs = New System.Windows.Forms.CheckBox()
        Me.chkCmbTranType = New BrighttechPack.CheckedComboBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.chkCustomer = New System.Windows.Forms.CheckBox()
        Me.chkInternal = New System.Windows.Forms.CheckBox()
        Me.chkOthers = New System.Windows.Forms.CheckBox()
        Me.chkSmith = New System.Windows.Forms.CheckBox()
        Me.chkDealer = New System.Windows.Forms.CheckBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.chkWithNillBalance = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.dtpAsOnDate = New BrighttechPack.DatePicker(Me.components)
        Me.lblTo = New System.Windows.Forms.Label()
        Me.lblFrom = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlContainer.SuspendLayout()
        Me.PnlMark.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(61, 459)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 36
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'chkCostCentreSelectAll
        '
        Me.chkCostCentreSelectAll.AutoSize = True
        Me.chkCostCentreSelectAll.Location = New System.Drawing.Point(20, 56)
        Me.chkCostCentreSelectAll.Name = "chkCostCentreSelectAll"
        Me.chkCostCentreSelectAll.Size = New System.Drawing.Size(95, 17)
        Me.chkCostCentreSelectAll.TabIndex = 7
        Me.chkCostCentreSelectAll.Text = "Cost Centre"
        Me.chkCostCentreSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCategory
        '
        Me.chkLstCategory.FormattingEnabled = True
        Me.chkLstCategory.Location = New System.Drawing.Point(17, 162)
        Me.chkLstCategory.Name = "chkLstCategory"
        Me.chkLstCategory.Size = New System.Drawing.Size(413, 84)
        Me.chkLstCategory.TabIndex = 12
        '
        'chkCategorySelectAll
        '
        Me.chkCategorySelectAll.AutoSize = True
        Me.chkCategorySelectAll.Location = New System.Drawing.Point(20, 145)
        Me.chkCategorySelectAll.Name = "chkCategorySelectAll"
        Me.chkCategorySelectAll.Size = New System.Drawing.Size(79, 17)
        Me.chkCategorySelectAll.TabIndex = 11
        Me.chkCategorySelectAll.Text = "Category"
        Me.chkCategorySelectAll.UseVisualStyleBackColor = True
        '
        'chkLstMetal
        '
        Me.chkLstMetal.FormattingEnabled = True
        Me.chkLstMetal.Location = New System.Drawing.Point(225, 75)
        Me.chkLstMetal.Name = "chkLstMetal"
        Me.chkLstMetal.Size = New System.Drawing.Size(205, 68)
        Me.chkLstMetal.TabIndex = 10
        '
        'chkMetalSelectAll
        '
        Me.chkMetalSelectAll.AutoSize = True
        Me.chkMetalSelectAll.Location = New System.Drawing.Point(228, 56)
        Me.chkMetalSelectAll.Name = "chkMetalSelectAll"
        Me.chkMetalSelectAll.Size = New System.Drawing.Size(56, 17)
        Me.chkMetalSelectAll.TabIndex = 9
        Me.chkMetalSelectAll.Text = "Metal"
        Me.chkMetalSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(17, 75)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(201, 68)
        Me.chkLstCostCentre.TabIndex = 8
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(273, 459)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 38
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(167, 459)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 37
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'pnlContainer
        '
        Me.pnlContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlContainer.Controls.Add(Me.chkMIMRApproval)
        Me.pnlContainer.Controls.Add(Me.chkDiaWtConvert)
        Me.pnlContainer.Controls.Add(Me.ChkWithDirectPur)
        Me.pnlContainer.Controls.Add(Me.chkSales)
        Me.pnlContainer.Controls.Add(Me.chkGroupByAcType)
        Me.pnlContainer.Controls.Add(Me.chkWithItemDet)
        Me.pnlContainer.Controls.Add(Me.chkStnWtConv)
        Me.pnlContainer.Controls.Add(Me.chkWithAppAmt)
        Me.pnlContainer.Controls.Add(Me.ChkwithWast)
        Me.pnlContainer.Controls.Add(Me.ChkPureWt)
        Me.pnlContainer.Controls.Add(Me.ChkNetWt)
        Me.pnlContainer.Controls.Add(Me.ChkGrossWt)
        Me.pnlContainer.Controls.Add(Me.chkWithSpecicFormat)
        Me.pnlContainer.Controls.Add(Me.cmbactive)
        Me.pnlContainer.Controls.Add(Me.Label2)
        Me.pnlContainer.Controls.Add(Me.ChkRelatedtran)
        Me.pnlContainer.Controls.Add(Me.CmbAcname)
        Me.pnlContainer.Controls.Add(Me.Label1)
        Me.pnlContainer.Controls.Add(Me.ChkApproval)
        Me.pnlContainer.Controls.Add(Me.PnlMark)
        Me.pnlContainer.Controls.Add(Me.chkAmtbal)
        Me.pnlContainer.Controls.Add(Me.dtpTodate)
        Me.pnlContainer.Controls.Add(Me.chkWithAbs)
        Me.pnlContainer.Controls.Add(Me.chkCmbTranType)
        Me.pnlContainer.Controls.Add(Me.Panel1)
        Me.pnlContainer.Controls.Add(Me.chkWithNillBalance)
        Me.pnlContainer.Controls.Add(Me.Label5)
        Me.pnlContainer.Controls.Add(Me.dtpAsOnDate)
        Me.pnlContainer.Controls.Add(Me.lblTo)
        Me.pnlContainer.Controls.Add(Me.lblFrom)
        Me.pnlContainer.Controls.Add(Me.btnExit)
        Me.pnlContainer.Controls.Add(Me.btnSearch)
        Me.pnlContainer.Controls.Add(Me.btnNew)
        Me.pnlContainer.Controls.Add(Me.chkCostCentreSelectAll)
        Me.pnlContainer.Controls.Add(Me.chkLstCostCentre)
        Me.pnlContainer.Controls.Add(Me.chkLstCategory)
        Me.pnlContainer.Controls.Add(Me.chkMetalSelectAll)
        Me.pnlContainer.Controls.Add(Me.chkCategorySelectAll)
        Me.pnlContainer.Controls.Add(Me.chkLstMetal)
        Me.pnlContainer.Location = New System.Drawing.Point(122, 12)
        Me.pnlContainer.Name = "pnlContainer"
        Me.pnlContainer.Size = New System.Drawing.Size(479, 501)
        Me.pnlContainer.TabIndex = 0
        '
        'chkMIMRApproval
        '
        Me.chkMIMRApproval.AutoSize = True
        Me.chkMIMRApproval.Location = New System.Drawing.Point(17, 406)
        Me.chkMIMRApproval.Name = "chkMIMRApproval"
        Me.chkMIMRApproval.Size = New System.Drawing.Size(141, 17)
        Me.chkMIMRApproval.TabIndex = 33
        Me.chkMIMRApproval.Text = "With MIMR Approval"
        Me.chkMIMRApproval.UseVisualStyleBackColor = True
        '
        'chkDiaWtConvert
        '
        Me.chkDiaWtConvert.AutoSize = True
        Me.chkDiaWtConvert.Location = New System.Drawing.Point(327, 386)
        Me.chkDiaWtConvert.Name = "chkDiaWtConvert"
        Me.chkDiaWtConvert.Size = New System.Drawing.Size(146, 17)
        Me.chkDiaWtConvert.TabIndex = 32
        Me.chkDiaWtConvert.Text = "Diamond Wt Convert"
        Me.chkDiaWtConvert.UseVisualStyleBackColor = True
        '
        'ChkWithDirectPur
        '
        Me.ChkWithDirectPur.AutoSize = True
        Me.ChkWithDirectPur.Location = New System.Drawing.Point(178, 406)
        Me.ChkWithDirectPur.Name = "ChkWithDirectPur"
        Me.ChkWithDirectPur.Size = New System.Drawing.Size(145, 17)
        Me.ChkWithDirectPur.TabIndex = 34
        Me.ChkWithDirectPur.Text = "With Direct Purchase"
        Me.ChkWithDirectPur.UseVisualStyleBackColor = True
        '
        'chkSales
        '
        Me.chkSales.AutoSize = True
        Me.chkSales.Location = New System.Drawing.Point(178, 385)
        Me.chkSales.Name = "chkSales"
        Me.chkSales.Size = New System.Drawing.Size(141, 17)
        Me.chkSales.TabIndex = 31
        Me.chkSales.Text = "With Sales\SalesRtn"
        Me.chkSales.UseVisualStyleBackColor = True
        '
        'chkGroupByAcType
        '
        Me.chkGroupByAcType.AutoSize = True
        Me.chkGroupByAcType.Location = New System.Drawing.Point(240, 276)
        Me.chkGroupByAcType.Name = "chkGroupByAcType"
        Me.chkGroupByAcType.Size = New System.Drawing.Size(123, 17)
        Me.chkGroupByAcType.TabIndex = 16
        Me.chkGroupByAcType.Text = "Group By Actype"
        Me.chkGroupByAcType.UseVisualStyleBackColor = True
        '
        'chkWithItemDet
        '
        Me.chkWithItemDet.AutoSize = True
        Me.chkWithItemDet.Location = New System.Drawing.Point(368, 365)
        Me.chkWithItemDet.Name = "chkWithItemDet"
        Me.chkWithItemDet.Size = New System.Drawing.Size(90, 17)
        Me.chkWithItemDet.TabIndex = 29
        Me.chkWithItemDet.Text = "Item Detail"
        Me.chkWithItemDet.UseVisualStyleBackColor = True
        '
        'chkStnWtConv
        '
        Me.chkStnWtConv.AutoSize = True
        Me.chkStnWtConv.Location = New System.Drawing.Point(327, 344)
        Me.chkStnWtConv.Name = "chkStnWtConv"
        Me.chkStnWtConv.Size = New System.Drawing.Size(128, 17)
        Me.chkStnWtConv.TabIndex = 26
        Me.chkStnWtConv.Text = "Stone Wt Convert"
        Me.chkStnWtConv.UseVisualStyleBackColor = True
        '
        'chkWithAppAmt
        '
        Me.chkWithAppAmt.AutoSize = True
        Me.chkWithAppAmt.Location = New System.Drawing.Point(17, 386)
        Me.chkWithAppAmt.Name = "chkWithAppAmt"
        Me.chkWithAppAmt.Size = New System.Drawing.Size(155, 17)
        Me.chkWithAppAmt.TabIndex = 30
        Me.chkWithAppAmt.Text = "With Approval Balance"
        Me.chkWithAppAmt.UseVisualStyleBackColor = True
        '
        'ChkwithWast
        '
        Me.ChkwithWast.AutoSize = True
        Me.ChkwithWast.Location = New System.Drawing.Point(327, 324)
        Me.ChkwithWast.Name = "ChkwithWast"
        Me.ChkwithWast.Size = New System.Drawing.Size(103, 17)
        Me.ChkwithWast.TabIndex = 23
        Me.ChkwithWast.Text = "With Wastage"
        Me.ChkwithWast.UseVisualStyleBackColor = True
        '
        'ChkPureWt
        '
        Me.ChkPureWt.AutoSize = True
        Me.ChkPureWt.Location = New System.Drawing.Point(253, 324)
        Me.ChkPureWt.Name = "ChkPureWt"
        Me.ChkPureWt.Size = New System.Drawing.Size(71, 17)
        Me.ChkPureWt.TabIndex = 22
        Me.ChkPureWt.Text = "Pure Wt"
        Me.ChkPureWt.UseVisualStyleBackColor = True
        '
        'ChkNetWt
        '
        Me.ChkNetWt.AutoSize = True
        Me.ChkNetWt.Location = New System.Drawing.Point(178, 324)
        Me.ChkNetWt.Name = "ChkNetWt"
        Me.ChkNetWt.Size = New System.Drawing.Size(64, 17)
        Me.ChkNetWt.TabIndex = 21
        Me.ChkNetWt.Text = "Net Wt"
        Me.ChkNetWt.UseVisualStyleBackColor = True
        '
        'ChkGrossWt
        '
        Me.ChkGrossWt.AutoSize = True
        Me.ChkGrossWt.Location = New System.Drawing.Point(101, 324)
        Me.ChkGrossWt.Name = "ChkGrossWt"
        Me.ChkGrossWt.Size = New System.Drawing.Size(65, 17)
        Me.ChkGrossWt.TabIndex = 20
        Me.ChkGrossWt.Text = "Grs Wt"
        Me.ChkGrossWt.UseVisualStyleBackColor = True
        '
        'chkWithSpecicFormat
        '
        Me.chkWithSpecicFormat.AutoSize = True
        Me.chkWithSpecicFormat.Location = New System.Drawing.Point(281, 33)
        Me.chkWithSpecicFormat.Name = "chkWithSpecicFormat"
        Me.chkWithSpecicFormat.Size = New System.Drawing.Size(143, 17)
        Me.chkWithSpecicFormat.TabIndex = 6
        Me.chkWithSpecicFormat.Text = "With Specific Format"
        Me.chkWithSpecicFormat.UseVisualStyleBackColor = True
        '
        'cmbactive
        '
        Me.cmbactive.FormattingEnabled = True
        Me.cmbactive.Location = New System.Drawing.Point(100, 274)
        Me.cmbactive.Name = "cmbactive"
        Me.cmbactive.Size = New System.Drawing.Size(134, 21)
        Me.cmbactive.TabIndex = 15
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 277)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(73, 13)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Active Type"
        '
        'ChkRelatedtran
        '
        Me.ChkRelatedtran.AutoSize = True
        Me.ChkRelatedtran.Location = New System.Drawing.Point(178, 365)
        Me.ChkRelatedtran.Name = "ChkRelatedtran"
        Me.ChkRelatedtran.Size = New System.Drawing.Size(168, 17)
        Me.ChkRelatedtran.TabIndex = 28
        Me.ChkRelatedtran.Text = "Related Transaction Only"
        Me.ChkRelatedtran.UseVisualStyleBackColor = True
        Me.ChkRelatedtran.Visible = False
        '
        'CmbAcname
        '
        Me.CmbAcname.FormattingEnabled = True
        Me.CmbAcname.Location = New System.Drawing.Point(100, 297)
        Me.CmbAcname.Name = "CmbAcname"
        Me.CmbAcname.Size = New System.Drawing.Size(278, 21)
        Me.CmbAcname.TabIndex = 18
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 301)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(54, 13)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "AcName"
        '
        'ChkApproval
        '
        Me.ChkApproval.AutoSize = True
        Me.ChkApproval.Location = New System.Drawing.Point(17, 365)
        Me.ChkApproval.Name = "ChkApproval"
        Me.ChkApproval.Size = New System.Drawing.Size(156, 17)
        Me.ChkApproval.TabIndex = 27
        Me.ChkApproval.Text = "With Counter Approval"
        Me.ChkApproval.UseVisualStyleBackColor = True
        '
        'PnlMark
        '
        Me.PnlMark.Controls.Add(Me.rbtLocal)
        Me.PnlMark.Controls.Add(Me.rbtOutstation)
        Me.PnlMark.Controls.Add(Me.rbtBothMU)
        Me.PnlMark.Location = New System.Drawing.Point(96, 427)
        Me.PnlMark.Name = "PnlMark"
        Me.PnlMark.Size = New System.Drawing.Size(228, 26)
        Me.PnlMark.TabIndex = 35
        '
        'rbtLocal
        '
        Me.rbtLocal.AutoSize = True
        Me.rbtLocal.Location = New System.Drawing.Point(73, 3)
        Me.rbtLocal.Name = "rbtLocal"
        Me.rbtLocal.Size = New System.Drawing.Size(54, 17)
        Me.rbtLocal.TabIndex = 1
        Me.rbtLocal.Text = "Local"
        Me.rbtLocal.UseVisualStyleBackColor = True
        '
        'rbtOutstation
        '
        Me.rbtOutstation.AutoSize = True
        Me.rbtOutstation.Location = New System.Drawing.Point(143, 3)
        Me.rbtOutstation.Name = "rbtOutstation"
        Me.rbtOutstation.Size = New System.Drawing.Size(83, 17)
        Me.rbtOutstation.TabIndex = 2
        Me.rbtOutstation.Text = "Outstation"
        Me.rbtOutstation.UseVisualStyleBackColor = True
        '
        'rbtBothMU
        '
        Me.rbtBothMU.AutoSize = True
        Me.rbtBothMU.Checked = True
        Me.rbtBothMU.Location = New System.Drawing.Point(6, 3)
        Me.rbtBothMU.Name = "rbtBothMU"
        Me.rbtBothMU.Size = New System.Drawing.Size(51, 17)
        Me.rbtBothMU.TabIndex = 0
        Me.rbtBothMU.TabStop = True
        Me.rbtBothMU.Text = "Both"
        Me.rbtBothMU.UseVisualStyleBackColor = True
        '
        'chkAmtbal
        '
        Me.chkAmtbal.AutoSize = True
        Me.chkAmtbal.Location = New System.Drawing.Point(178, 344)
        Me.chkAmtbal.Name = "chkAmtbal"
        Me.chkAmtbal.Size = New System.Drawing.Size(148, 17)
        Me.chkAmtbal.TabIndex = 25
        Me.chkAmtbal.Text = "With Amount Balance"
        Me.chkAmtbal.UseVisualStyleBackColor = True
        '
        'dtpTodate
        '
        Me.dtpTodate.Location = New System.Drawing.Point(292, 7)
        Me.dtpTodate.Mask = "##/##/####"
        Me.dtpTodate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTodate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTodate.Name = "dtpTodate"
        Me.dtpTodate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTodate.Size = New System.Drawing.Size(93, 21)
        Me.dtpTodate.TabIndex = 3
        Me.dtpTodate.Text = "07/03/9998"
        Me.dtpTodate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        Me.dtpTodate.Visible = False
        '
        'chkWithAbs
        '
        Me.chkWithAbs.Location = New System.Drawing.Point(20, 9)
        Me.chkWithAbs.Name = "chkWithAbs"
        Me.chkWithAbs.Size = New System.Drawing.Size(95, 17)
        Me.chkWithAbs.TabIndex = 0
        Me.chkWithAbs.Text = "As On Date"
        Me.chkWithAbs.UseVisualStyleBackColor = True
        '
        'chkCmbTranType
        '
        Me.chkCmbTranType.CheckOnClick = True
        Me.chkCmbTranType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbTranType.DropDownHeight = 1
        Me.chkCmbTranType.FormattingEnabled = True
        Me.chkCmbTranType.IntegralHeight = False
        Me.chkCmbTranType.Location = New System.Drawing.Point(100, 30)
        Me.chkCmbTranType.Name = "chkCmbTranType"
        Me.chkCmbTranType.Size = New System.Drawing.Size(164, 22)
        Me.chkCmbTranType.TabIndex = 5
        Me.chkCmbTranType.ValueSeparator = ", "
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.chkCustomer)
        Me.Panel1.Controls.Add(Me.chkInternal)
        Me.Panel1.Controls.Add(Me.chkOthers)
        Me.Panel1.Controls.Add(Me.chkSmith)
        Me.Panel1.Controls.Add(Me.chkDealer)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Location = New System.Drawing.Point(17, 248)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(413, 23)
        Me.Panel1.TabIndex = 13
        '
        'chkCustomer
        '
        Me.chkCustomer.AutoSize = True
        Me.chkCustomer.Location = New System.Drawing.Point(259, 4)
        Me.chkCustomer.Name = "chkCustomer"
        Me.chkCustomer.Size = New System.Drawing.Size(82, 17)
        Me.chkCustomer.TabIndex = 4
        Me.chkCustomer.Text = "Customer"
        Me.chkCustomer.UseVisualStyleBackColor = True
        '
        'chkInternal
        '
        Me.chkInternal.AutoSize = True
        Me.chkInternal.Location = New System.Drawing.Point(184, 4)
        Me.chkInternal.Name = "chkInternal"
        Me.chkInternal.Size = New System.Drawing.Size(71, 17)
        Me.chkInternal.TabIndex = 3
        Me.chkInternal.Text = "Internal"
        Me.chkInternal.UseVisualStyleBackColor = True
        '
        'chkOthers
        '
        Me.chkOthers.AutoSize = True
        Me.chkOthers.Location = New System.Drawing.Point(345, 4)
        Me.chkOthers.Name = "chkOthers"
        Me.chkOthers.Size = New System.Drawing.Size(64, 17)
        Me.chkOthers.TabIndex = 5
        Me.chkOthers.Text = "Others"
        Me.chkOthers.UseVisualStyleBackColor = True
        '
        'chkSmith
        '
        Me.chkSmith.AutoSize = True
        Me.chkSmith.Checked = True
        Me.chkSmith.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSmith.Location = New System.Drawing.Point(53, 4)
        Me.chkSmith.Name = "chkSmith"
        Me.chkSmith.Size = New System.Drawing.Size(59, 17)
        Me.chkSmith.TabIndex = 1
        Me.chkSmith.Text = "Smith"
        Me.chkSmith.UseVisualStyleBackColor = True
        '
        'chkDealer
        '
        Me.chkDealer.AutoSize = True
        Me.chkDealer.Checked = True
        Me.chkDealer.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDealer.Location = New System.Drawing.Point(116, 4)
        Me.chkDealer.Name = "chkDealer"
        Me.chkDealer.Size = New System.Drawing.Size(64, 17)
        Me.chkDealer.TabIndex = 2
        Me.chkDealer.Text = "Dealer"
        Me.chkDealer.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 5)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(46, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Actype"
        '
        'chkWithNillBalance
        '
        Me.chkWithNillBalance.AutoSize = True
        Me.chkWithNillBalance.Location = New System.Drawing.Point(17, 344)
        Me.chkWithNillBalance.Name = "chkWithNillBalance"
        Me.chkWithNillBalance.Size = New System.Drawing.Size(121, 17)
        Me.chkWithNillBalance.TabIndex = 24
        Me.chkWithNillBalance.Text = "With Nill Balance"
        Me.chkWithNillBalance.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(14, 326)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(62, 13)
        Me.Label5.TabIndex = 19
        Me.Label5.Text = "Based On"
        '
        'dtpAsOnDate
        '
        Me.dtpAsOnDate.Location = New System.Drawing.Point(121, 7)
        Me.dtpAsOnDate.Mask = "##/##/####"
        Me.dtpAsOnDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpAsOnDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpAsOnDate.Name = "dtpAsOnDate"
        Me.dtpAsOnDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpAsOnDate.Size = New System.Drawing.Size(93, 21)
        Me.dtpAsOnDate.TabIndex = 1
        Me.dtpAsOnDate.Text = "07/03/9998"
        Me.dtpAsOnDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(17, 35)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(63, 13)
        Me.lblTo.TabIndex = 4
        Me.lblTo.Text = "Tran Type"
        '
        'lblFrom
        '
        Me.lblFrom.AutoSize = True
        Me.lblFrom.Location = New System.Drawing.Point(222, 11)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(64, 13)
        Me.lblFrom.TabIndex = 2
        Me.lblFrom.Text = "AsOnDate"
        Me.lblFrom.Visible = False
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
        'frmSmithBalanceSummaryReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(789, 525)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmSmithBalanceSummaryReport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Smith Balance Summary"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlContainer.ResumeLayout(False)
        Me.pnlContainer.PerformLayout()
        Me.PnlMark.ResumeLayout(False)
        Me.PnlMark.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents chkCostCentreSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCategory As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkCategorySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstMetal As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkMetalSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents pnlContainer As System.Windows.Forms.Panel
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents dtpAsOnDate As BrighttechPack.DatePicker
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents chkWithNillBalance As System.Windows.Forms.CheckBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents chkSmith As System.Windows.Forms.CheckBox
    Friend WithEvents chkDealer As System.Windows.Forms.CheckBox
    Friend WithEvents chkOthers As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents chkInternal As System.Windows.Forms.CheckBox
    Friend WithEvents chkCmbTranType As BrighttechPack.CheckedComboBox
    Friend WithEvents dtpTodate As BrighttechPack.DatePicker
    Friend WithEvents chkWithAbs As System.Windows.Forms.CheckBox
    Friend WithEvents lblFrom As System.Windows.Forms.Label
    Friend WithEvents chkAmtbal As System.Windows.Forms.CheckBox
    Friend WithEvents PnlMark As System.Windows.Forms.Panel
    Friend WithEvents rbtLocal As System.Windows.Forms.RadioButton
    Friend WithEvents rbtOutstation As System.Windows.Forms.RadioButton
    Friend WithEvents rbtBothMU As System.Windows.Forms.RadioButton
    Friend WithEvents ChkApproval As System.Windows.Forms.CheckBox
    Friend WithEvents chkCustomer As System.Windows.Forms.CheckBox
    Friend WithEvents CmbAcname As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ChkRelatedtran As System.Windows.Forms.CheckBox
    Friend WithEvents cmbactive As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkWithSpecicFormat As System.Windows.Forms.CheckBox
    Friend WithEvents ChkGrossWt As System.Windows.Forms.CheckBox
    Friend WithEvents ChkNetWt As System.Windows.Forms.CheckBox
    Friend WithEvents ChkPureWt As System.Windows.Forms.CheckBox
    Friend WithEvents ChkwithWast As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithAppAmt As System.Windows.Forms.CheckBox
    Friend WithEvents chkStnWtConv As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithItemDet As System.Windows.Forms.CheckBox
    Friend WithEvents chkGroupByAcType As System.Windows.Forms.CheckBox
    Friend WithEvents chkSales As System.Windows.Forms.CheckBox
    Friend WithEvents ChkWithDirectPur As CheckBox
    Friend WithEvents chkDiaWtConvert As CheckBox
    Friend WithEvents chkMIMRApproval As CheckBox
End Class
