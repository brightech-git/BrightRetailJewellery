<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmSaleReportWastMc
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
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlContainer = New System.Windows.Forms.Panel()
        Me.chkcmbItemCtr = New BrighttechPack.CheckedComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.chkCategorySelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstCategory = New System.Windows.Forms.CheckedListBox()
        Me.ChkInclude = New System.Windows.Forms.CheckBox()
        Me.ChklstboxInclude = New System.Windows.Forms.CheckedListBox()
        Me.chkBasedOnPartlyWeight = New System.Windows.Forms.CheckBox()
        Me.chkWithDiffRate = New System.Windows.Forms.CheckBox()
        Me.chkSpecificFormat = New System.Windows.Forms.CheckBox()
        Me.chkcmbCashCtr = New BrighttechPack.CheckedComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.ChkHighLight = New System.Windows.Forms.CheckBox()
        Me.chkstnamt = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.rbtTranNo = New System.Windows.Forms.RadioButton()
        Me.rbtItemId = New System.Windows.Forms.RadioButton()
        Me.ChkGrpbyEmp = New System.Windows.Forms.CheckBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtWper = New System.Windows.Forms.TextBox()
        Me.chkCompanySelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstCompany = New System.Windows.Forms.CheckedListBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.chkHomeSales = New System.Windows.Forms.CheckBox()
        Me.rbtBoth = New System.Windows.Forms.RadioButton()
        Me.rbtNonTag = New System.Windows.Forms.RadioButton()
        Me.rbtTag = New System.Windows.Forms.RadioButton()
        Me.chkCostCentreSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox()
        Me.chkMetalSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstMetal = New System.Windows.Forms.CheckedListBox()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.pnlContainer.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(193, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(20, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(19, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date From"
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(24, 425)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 34
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(233, 425)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 36
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(130, 425)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 35
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
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
        'pnlContainer
        '
        Me.pnlContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlContainer.Controls.Add(Me.chkcmbItemCtr)
        Me.pnlContainer.Controls.Add(Me.Label7)
        Me.pnlContainer.Controls.Add(Me.chkCategorySelectAll)
        Me.pnlContainer.Controls.Add(Me.chkLstCategory)
        Me.pnlContainer.Controls.Add(Me.ChkInclude)
        Me.pnlContainer.Controls.Add(Me.ChklstboxInclude)
        Me.pnlContainer.Controls.Add(Me.chkBasedOnPartlyWeight)
        Me.pnlContainer.Controls.Add(Me.chkWithDiffRate)
        Me.pnlContainer.Controls.Add(Me.chkSpecificFormat)
        Me.pnlContainer.Controls.Add(Me.chkcmbCashCtr)
        Me.pnlContainer.Controls.Add(Me.Label6)
        Me.pnlContainer.Controls.Add(Me.ChkHighLight)
        Me.pnlContainer.Controls.Add(Me.chkstnamt)
        Me.pnlContainer.Controls.Add(Me.Label5)
        Me.pnlContainer.Controls.Add(Me.rbtTranNo)
        Me.pnlContainer.Controls.Add(Me.rbtItemId)
        Me.pnlContainer.Controls.Add(Me.ChkGrpbyEmp)
        Me.pnlContainer.Controls.Add(Me.Label4)
        Me.pnlContainer.Controls.Add(Me.Label3)
        Me.pnlContainer.Controls.Add(Me.txtWper)
        Me.pnlContainer.Controls.Add(Me.chkCompanySelectAll)
        Me.pnlContainer.Controls.Add(Me.chkLstCompany)
        Me.pnlContainer.Controls.Add(Me.dtpTo)
        Me.pnlContainer.Controls.Add(Me.dtpFrom)
        Me.pnlContainer.Controls.Add(Me.chkHomeSales)
        Me.pnlContainer.Controls.Add(Me.rbtBoth)
        Me.pnlContainer.Controls.Add(Me.rbtNonTag)
        Me.pnlContainer.Controls.Add(Me.rbtTag)
        Me.pnlContainer.Controls.Add(Me.chkCostCentreSelectAll)
        Me.pnlContainer.Controls.Add(Me.chkLstCostCentre)
        Me.pnlContainer.Controls.Add(Me.chkMetalSelectAll)
        Me.pnlContainer.Controls.Add(Me.chkLstMetal)
        Me.pnlContainer.Controls.Add(Me.Label1)
        Me.pnlContainer.Controls.Add(Me.btnSearch)
        Me.pnlContainer.Controls.Add(Me.btnExit)
        Me.pnlContainer.Controls.Add(Me.btnNew)
        Me.pnlContainer.Controls.Add(Me.Label2)
        Me.pnlContainer.Location = New System.Drawing.Point(103, 12)
        Me.pnlContainer.Name = "pnlContainer"
        Me.pnlContainer.Size = New System.Drawing.Size(442, 464)
        Me.pnlContainer.TabIndex = 0
        '
        'chkcmbItemCtr
        '
        Me.chkcmbItemCtr.CheckOnClick = True
        Me.chkcmbItemCtr.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbItemCtr.DropDownHeight = 1
        Me.chkcmbItemCtr.FormattingEnabled = True
        Me.chkcmbItemCtr.IntegralHeight = False
        Me.chkcmbItemCtr.Location = New System.Drawing.Point(114, 225)
        Me.chkcmbItemCtr.Name = "chkcmbItemCtr"
        Me.chkcmbItemCtr.Size = New System.Drawing.Size(232, 22)
        Me.chkcmbItemCtr.TabIndex = 13
        Me.chkcmbItemCtr.ValueSeparator = ", "
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(19, 230)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(84, 13)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "Item Counter"
        '
        'chkCategorySelectAll
        '
        Me.chkCategorySelectAll.AutoSize = True
        Me.chkCategorySelectAll.Location = New System.Drawing.Point(225, 129)
        Me.chkCategorySelectAll.Name = "chkCategorySelectAll"
        Me.chkCategorySelectAll.Size = New System.Drawing.Size(79, 17)
        Me.chkCategorySelectAll.TabIndex = 10
        Me.chkCategorySelectAll.Text = "Category"
        Me.chkCategorySelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCategory
        '
        Me.chkLstCategory.FormattingEnabled = True
        Me.chkLstCategory.Location = New System.Drawing.Point(222, 150)
        Me.chkLstCategory.Name = "chkLstCategory"
        Me.chkLstCategory.Size = New System.Drawing.Size(199, 68)
        Me.chkLstCategory.TabIndex = 11
        '
        'ChkInclude
        '
        Me.ChkInclude.AutoSize = True
        Me.ChkInclude.Checked = True
        Me.ChkInclude.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkInclude.Location = New System.Drawing.Point(22, 277)
        Me.ChkInclude.Name = "ChkInclude"
        Me.ChkInclude.Size = New System.Drawing.Size(68, 17)
        Me.ChkInclude.TabIndex = 16
        Me.ChkInclude.Text = "Include"
        Me.ChkInclude.UseVisualStyleBackColor = True
        '
        'ChklstboxInclude
        '
        Me.ChklstboxInclude.FormattingEnabled = True
        Me.ChklstboxInclude.Location = New System.Drawing.Point(114, 275)
        Me.ChklstboxInclude.Name = "ChklstboxInclude"
        Me.ChklstboxInclude.Size = New System.Drawing.Size(232, 20)
        Me.ChklstboxInclude.TabIndex = 17
        '
        'chkBasedOnPartlyWeight
        '
        Me.chkBasedOnPartlyWeight.AutoSize = True
        Me.chkBasedOnPartlyWeight.Location = New System.Drawing.Point(259, 377)
        Me.chkBasedOnPartlyWeight.Name = "chkBasedOnPartlyWeight"
        Me.chkBasedOnPartlyWeight.Size = New System.Drawing.Size(164, 17)
        Me.chkBasedOnPartlyWeight.TabIndex = 30
        Me.chkBasedOnPartlyWeight.Text = "Sale Va (Partly Sale Wt)"
        Me.chkBasedOnPartlyWeight.UseVisualStyleBackColor = True
        '
        'chkWithDiffRate
        '
        Me.chkWithDiffRate.AutoSize = True
        Me.chkWithDiffRate.Location = New System.Drawing.Point(156, 377)
        Me.chkWithDiffRate.Name = "chkWithDiffRate"
        Me.chkWithDiffRate.Size = New System.Drawing.Size(105, 17)
        Me.chkWithDiffRate.TabIndex = 29
        Me.chkWithDiffRate.Text = "With Diff Rate"
        Me.chkWithDiffRate.UseVisualStyleBackColor = True
        '
        'chkSpecificFormat
        '
        Me.chkSpecificFormat.AutoSize = True
        Me.chkSpecificFormat.Location = New System.Drawing.Point(25, 377)
        Me.chkSpecificFormat.Name = "chkSpecificFormat"
        Me.chkSpecificFormat.Size = New System.Drawing.Size(114, 17)
        Me.chkSpecificFormat.TabIndex = 28
        Me.chkSpecificFormat.Text = "Specific Format"
        Me.chkSpecificFormat.UseVisualStyleBackColor = True
        '
        'chkcmbCashCtr
        '
        Me.chkcmbCashCtr.CheckOnClick = True
        Me.chkcmbCashCtr.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbCashCtr.DropDownHeight = 1
        Me.chkcmbCashCtr.FormattingEnabled = True
        Me.chkcmbCashCtr.IntegralHeight = False
        Me.chkcmbCashCtr.Location = New System.Drawing.Point(114, 250)
        Me.chkcmbCashCtr.Name = "chkcmbCashCtr"
        Me.chkcmbCashCtr.Size = New System.Drawing.Size(232, 22)
        Me.chkcmbCashCtr.TabIndex = 15
        Me.chkcmbCashCtr.ValueSeparator = ", "
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(22, 255)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(86, 13)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "Cash Counter"
        '
        'ChkHighLight
        '
        Me.ChkHighLight.AutoSize = True
        Me.ChkHighLight.Location = New System.Drawing.Point(259, 351)
        Me.ChkHighLight.Name = "ChkHighLight"
        Me.ChkHighLight.Size = New System.Drawing.Size(174, 17)
        Me.ChkHighLight.TabIndex = 27
        Me.ChkHighLight.Text = "HighLight Chit Adjustment"
        Me.ChkHighLight.UseVisualStyleBackColor = True
        '
        'chkstnamt
        '
        Me.chkstnamt.AutoSize = True
        Me.chkstnamt.Location = New System.Drawing.Point(25, 351)
        Me.chkstnamt.Name = "chkstnamt"
        Me.chkstnamt.Size = New System.Drawing.Size(136, 17)
        Me.chkstnamt.TabIndex = 26
        Me.chkstnamt.Text = "With Stone Amount"
        Me.chkstnamt.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(22, 329)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(59, 13)
        Me.Label5.TabIndex = 22
        Me.Label5.Text = "Order By"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'rbtTranNo
        '
        Me.rbtTranNo.AutoSize = True
        Me.rbtTranNo.Location = New System.Drawing.Point(156, 327)
        Me.rbtTranNo.Name = "rbtTranNo"
        Me.rbtTranNo.Size = New System.Drawing.Size(69, 17)
        Me.rbtTranNo.TabIndex = 24
        Me.rbtTranNo.TabStop = True
        Me.rbtTranNo.Text = "Tran No"
        Me.rbtTranNo.UseVisualStyleBackColor = True
        '
        'rbtItemId
        '
        Me.rbtItemId.AutoSize = True
        Me.rbtItemId.Location = New System.Drawing.Point(87, 327)
        Me.rbtItemId.Name = "rbtItemId"
        Me.rbtItemId.Size = New System.Drawing.Size(68, 17)
        Me.rbtItemId.TabIndex = 23
        Me.rbtItemId.TabStop = True
        Me.rbtItemId.Text = "Item Id"
        Me.rbtItemId.UseVisualStyleBackColor = True
        '
        'ChkGrpbyEmp
        '
        Me.ChkGrpbyEmp.AutoSize = True
        Me.ChkGrpbyEmp.Checked = True
        Me.ChkGrpbyEmp.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkGrpbyEmp.Location = New System.Drawing.Point(259, 327)
        Me.ChkGrpbyEmp.Name = "ChkGrpbyEmp"
        Me.ChkGrpbyEmp.Size = New System.Drawing.Size(139, 17)
        Me.ChkGrpbyEmp.TabIndex = 25
        Me.ChkGrpbyEmp.Text = "Group by Employee"
        Me.ChkGrpbyEmp.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(213, 403)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(63, 13)
        Me.Label4.TabIndex = 33
        Me.Label4.Text = " &&  Above"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(21, 403)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(104, 13)
        Me.Label3.TabIndex = 31
        Me.Label3.Text = "Less Wastage % "
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtWper
        '
        Me.txtWper.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtWper.Location = New System.Drawing.Point(129, 399)
        Me.txtWper.MaxLength = 3
        Me.txtWper.Name = "txtWper"
        Me.txtWper.Size = New System.Drawing.Size(79, 21)
        Me.txtWper.TabIndex = 32
        Me.txtWper.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'chkCompanySelectAll
        '
        Me.chkCompanySelectAll.AutoSize = True
        Me.chkCompanySelectAll.Location = New System.Drawing.Point(22, 36)
        Me.chkCompanySelectAll.Name = "chkCompanySelectAll"
        Me.chkCompanySelectAll.Size = New System.Drawing.Size(81, 17)
        Me.chkCompanySelectAll.TabIndex = 4
        Me.chkCompanySelectAll.Text = "Company"
        Me.chkCompanySelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCompany
        '
        Me.chkLstCompany.FormattingEnabled = True
        Me.chkLstCompany.Location = New System.Drawing.Point(19, 57)
        Me.chkLstCompany.Name = "chkLstCompany"
        Me.chkLstCompany.Size = New System.Drawing.Size(201, 68)
        Me.chkLstCompany.TabIndex = 5
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(228, 15)
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
        Me.dtpFrom.Location = New System.Drawing.Point(94, 15)
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
        'chkHomeSales
        '
        Me.chkHomeSales.AutoSize = True
        Me.chkHomeSales.Location = New System.Drawing.Point(259, 303)
        Me.chkHomeSales.Name = "chkHomeSales"
        Me.chkHomeSales.Size = New System.Drawing.Size(123, 17)
        Me.chkHomeSales.TabIndex = 21
        Me.chkHomeSales.Text = "With Home Sales"
        Me.chkHomeSales.UseVisualStyleBackColor = True
        '
        'rbtBoth
        '
        Me.rbtBoth.AutoSize = True
        Me.rbtBoth.Location = New System.Drawing.Point(156, 303)
        Me.rbtBoth.Name = "rbtBoth"
        Me.rbtBoth.Size = New System.Drawing.Size(51, 17)
        Me.rbtBoth.TabIndex = 20
        Me.rbtBoth.TabStop = True
        Me.rbtBoth.Text = "Both"
        Me.rbtBoth.UseVisualStyleBackColor = True
        '
        'rbtNonTag
        '
        Me.rbtNonTag.AutoSize = True
        Me.rbtNonTag.Location = New System.Drawing.Point(87, 303)
        Me.rbtNonTag.Name = "rbtNonTag"
        Me.rbtNonTag.Size = New System.Drawing.Size(71, 17)
        Me.rbtNonTag.TabIndex = 19
        Me.rbtNonTag.TabStop = True
        Me.rbtNonTag.Text = "Non Tag"
        Me.rbtNonTag.UseVisualStyleBackColor = True
        '
        'rbtTag
        '
        Me.rbtTag.AutoSize = True
        Me.rbtTag.Location = New System.Drawing.Point(25, 303)
        Me.rbtTag.Name = "rbtTag"
        Me.rbtTag.Size = New System.Drawing.Size(45, 17)
        Me.rbtTag.TabIndex = 18
        Me.rbtTag.TabStop = True
        Me.rbtTag.Text = "Tag"
        Me.rbtTag.UseVisualStyleBackColor = True
        '
        'chkCostCentreSelectAll
        '
        Me.chkCostCentreSelectAll.AutoSize = True
        Me.chkCostCentreSelectAll.Location = New System.Drawing.Point(228, 36)
        Me.chkCostCentreSelectAll.Name = "chkCostCentreSelectAll"
        Me.chkCostCentreSelectAll.Size = New System.Drawing.Size(95, 17)
        Me.chkCostCentreSelectAll.TabIndex = 6
        Me.chkCostCentreSelectAll.Text = "Cost Centre"
        Me.chkCostCentreSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(225, 57)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(201, 68)
        Me.chkLstCostCentre.TabIndex = 7
        '
        'chkMetalSelectAll
        '
        Me.chkMetalSelectAll.AutoSize = True
        Me.chkMetalSelectAll.Location = New System.Drawing.Point(22, 129)
        Me.chkMetalSelectAll.Name = "chkMetalSelectAll"
        Me.chkMetalSelectAll.Size = New System.Drawing.Size(56, 17)
        Me.chkMetalSelectAll.TabIndex = 8
        Me.chkMetalSelectAll.Text = "Metal"
        Me.chkMetalSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstMetal
        '
        Me.chkLstMetal.FormattingEnabled = True
        Me.chkLstMetal.Location = New System.Drawing.Point(19, 150)
        Me.chkLstMetal.Name = "chkLstMetal"
        Me.chkLstMetal.Size = New System.Drawing.Size(199, 68)
        Me.chkLstMetal.TabIndex = 9
        '
        'frmSaleReportWastMc
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(673, 488)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmSaleReportWastMc"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Sales Wastage & Mc Analysis Report "
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.pnlContainer.ResumeLayout(False)
        Me.pnlContainer.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlContainer As System.Windows.Forms.Panel
    Friend WithEvents chkCostCentreSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkMetalSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstMetal As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkHomeSales As System.Windows.Forms.CheckBox
    Friend WithEvents rbtBoth As System.Windows.Forms.RadioButton
    Friend WithEvents rbtNonTag As System.Windows.Forms.RadioButton
    Friend WithEvents rbtTag As System.Windows.Forms.RadioButton
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkCompanySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCompany As System.Windows.Forms.CheckedListBox
    Friend WithEvents txtWper As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ChkGrpbyEmp As System.Windows.Forms.CheckBox
    Friend WithEvents rbtTranNo As System.Windows.Forms.RadioButton
    Friend WithEvents rbtItemId As System.Windows.Forms.RadioButton
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents chkstnamt As System.Windows.Forms.CheckBox
    Friend WithEvents ChkHighLight As System.Windows.Forms.CheckBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents chkcmbCashCtr As BrighttechPack.CheckedComboBox
    Friend WithEvents chkSpecificFormat As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithDiffRate As CheckBox
    Friend WithEvents chkBasedOnPartlyWeight As CheckBox
    Friend WithEvents ChklstboxInclude As CheckedListBox
    Friend WithEvents ChkInclude As CheckBox
    Friend WithEvents chkCategorySelectAll As CheckBox
    Friend WithEvents chkLstCategory As CheckedListBox
    Friend WithEvents chkcmbItemCtr As BrighttechPack.CheckedComboBox
    Friend WithEvents Label7 As Label
End Class
