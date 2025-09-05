<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSmithBalanceSummaryFormat3
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
        Me.btnSearch = New System.Windows.Forms.Button
        Me.chkCostCentreSelectAll = New System.Windows.Forms.CheckBox
        Me.chkLstCategory = New System.Windows.Forms.CheckedListBox
        Me.chkCategorySelectAll = New System.Windows.Forms.CheckBox
        Me.chkLstMetal = New System.Windows.Forms.CheckedListBox
        Me.chkMetalSelectAll = New System.Windows.Forms.CheckBox
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.pnlContainer = New System.Windows.Forms.Panel
        Me.Label4 = New System.Windows.Forms.Label
        Me.rbtNetWt = New System.Windows.Forms.RadioButton
        Me.rbtPureWt = New System.Windows.Forms.RadioButton
        Me.rbtGrsWt = New System.Windows.Forms.RadioButton
        Me.ChkwithWast = New System.Windows.Forms.CheckBox
        Me.dtpTodate = New BrighttechPack.DatePicker(Me.components)
        Me.cmbactive = New System.Windows.Forms.ComboBox
        Me.lblFrom = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.CmbAcname = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.ChkApproval = New System.Windows.Forms.CheckBox
        Me.PnlMark = New System.Windows.Forms.Panel
        Me.rbtLocal = New System.Windows.Forms.RadioButton
        Me.rbtOutstation = New System.Windows.Forms.RadioButton
        Me.rbtBothMU = New System.Windows.Forms.RadioButton
        Me.chkCmbTranType = New BrighttechPack.CheckedComboBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.chkCustomer = New System.Windows.Forms.CheckBox
        Me.chkInternal = New System.Windows.Forms.CheckBox
        Me.chkOthers = New System.Windows.Forms.CheckBox
        Me.chkSmith = New System.Windows.Forms.CheckBox
        Me.chkDealer = New System.Windows.Forms.CheckBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.chkWithNillBalance = New System.Windows.Forms.CheckBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.dtpAsOnDate = New BrighttechPack.DatePicker(Me.components)
        Me.lblTo = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.pnlContainer.SuspendLayout()
        Me.PnlMark.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(57, 419)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 27
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'chkCostCentreSelectAll
        '
        Me.chkCostCentreSelectAll.AutoSize = True
        Me.chkCostCentreSelectAll.Location = New System.Drawing.Point(20, 67)
        Me.chkCostCentreSelectAll.Name = "chkCostCentreSelectAll"
        Me.chkCostCentreSelectAll.Size = New System.Drawing.Size(95, 17)
        Me.chkCostCentreSelectAll.TabIndex = 6
        Me.chkCostCentreSelectAll.Text = "Cost Centre"
        Me.chkCostCentreSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCategory
        '
        Me.chkLstCategory.FormattingEnabled = True
        Me.chkLstCategory.Location = New System.Drawing.Point(17, 188)
        Me.chkLstCategory.Name = "chkLstCategory"
        Me.chkLstCategory.Size = New System.Drawing.Size(407, 84)
        Me.chkLstCategory.TabIndex = 11
        '
        'chkCategorySelectAll
        '
        Me.chkCategorySelectAll.AutoSize = True
        Me.chkCategorySelectAll.Location = New System.Drawing.Point(20, 171)
        Me.chkCategorySelectAll.Name = "chkCategorySelectAll"
        Me.chkCategorySelectAll.Size = New System.Drawing.Size(79, 17)
        Me.chkCategorySelectAll.TabIndex = 10
        Me.chkCategorySelectAll.Text = "Category"
        Me.chkCategorySelectAll.UseVisualStyleBackColor = True
        '
        'chkLstMetal
        '
        Me.chkLstMetal.FormattingEnabled = True
        Me.chkLstMetal.Location = New System.Drawing.Point(225, 84)
        Me.chkLstMetal.Name = "chkLstMetal"
        Me.chkLstMetal.Size = New System.Drawing.Size(199, 84)
        Me.chkLstMetal.TabIndex = 9
        '
        'chkMetalSelectAll
        '
        Me.chkMetalSelectAll.AutoSize = True
        Me.chkMetalSelectAll.Location = New System.Drawing.Point(228, 67)
        Me.chkMetalSelectAll.Name = "chkMetalSelectAll"
        Me.chkMetalSelectAll.Size = New System.Drawing.Size(56, 17)
        Me.chkMetalSelectAll.TabIndex = 8
        Me.chkMetalSelectAll.Text = "Metal"
        Me.chkMetalSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(17, 84)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(201, 84)
        Me.chkLstCostCentre.TabIndex = 7
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(269, 419)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 29
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(163, 419)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 28
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'pnlContainer
        '
        Me.pnlContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlContainer.Controls.Add(Me.Label4)
        Me.pnlContainer.Controls.Add(Me.rbtNetWt)
        Me.pnlContainer.Controls.Add(Me.rbtPureWt)
        Me.pnlContainer.Controls.Add(Me.rbtGrsWt)
        Me.pnlContainer.Controls.Add(Me.ChkwithWast)
        Me.pnlContainer.Controls.Add(Me.dtpTodate)
        Me.pnlContainer.Controls.Add(Me.cmbactive)
        Me.pnlContainer.Controls.Add(Me.lblFrom)
        Me.pnlContainer.Controls.Add(Me.Label2)
        Me.pnlContainer.Controls.Add(Me.CmbAcname)
        Me.pnlContainer.Controls.Add(Me.Label1)
        Me.pnlContainer.Controls.Add(Me.ChkApproval)
        Me.pnlContainer.Controls.Add(Me.PnlMark)
        Me.pnlContainer.Controls.Add(Me.chkCmbTranType)
        Me.pnlContainer.Controls.Add(Me.Panel1)
        Me.pnlContainer.Controls.Add(Me.chkWithNillBalance)
        Me.pnlContainer.Controls.Add(Me.Label5)
        Me.pnlContainer.Controls.Add(Me.dtpAsOnDate)
        Me.pnlContainer.Controls.Add(Me.lblTo)
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
        Me.pnlContainer.Size = New System.Drawing.Size(450, 461)
        Me.pnlContainer.TabIndex = 0
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(17, 16)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(67, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "From Date"
        '
        'rbtNetWt
        '
        Me.rbtNetWt.AutoSize = True
        Me.rbtNetWt.Location = New System.Drawing.Point(162, 351)
        Me.rbtNetWt.Name = "rbtNetWt"
        Me.rbtNetWt.Size = New System.Drawing.Size(63, 17)
        Me.rbtNetWt.TabIndex = 19
        Me.rbtNetWt.Text = "Net Wt"
        Me.rbtNetWt.UseVisualStyleBackColor = True
        '
        'rbtPureWt
        '
        Me.rbtPureWt.AutoSize = True
        Me.rbtPureWt.Location = New System.Drawing.Point(232, 351)
        Me.rbtPureWt.Name = "rbtPureWt"
        Me.rbtPureWt.Size = New System.Drawing.Size(70, 17)
        Me.rbtPureWt.TabIndex = 20
        Me.rbtPureWt.Text = "Pure Wt"
        Me.rbtPureWt.UseVisualStyleBackColor = True
        '
        'rbtGrsWt
        '
        Me.rbtGrsWt.AutoSize = True
        Me.rbtGrsWt.Checked = True
        Me.rbtGrsWt.Location = New System.Drawing.Point(91, 351)
        Me.rbtGrsWt.Name = "rbtGrsWt"
        Me.rbtGrsWt.Size = New System.Drawing.Size(64, 17)
        Me.rbtGrsWt.TabIndex = 18
        Me.rbtGrsWt.TabStop = True
        Me.rbtGrsWt.Text = "Grs Wt"
        Me.rbtGrsWt.UseVisualStyleBackColor = True
        '
        'ChkwithWast
        '
        Me.ChkwithWast.AutoSize = True
        Me.ChkwithWast.Location = New System.Drawing.Point(316, 351)
        Me.ChkwithWast.Name = "ChkwithWast"
        Me.ChkwithWast.Size = New System.Drawing.Size(104, 17)
        Me.ChkwithWast.TabIndex = 21
        Me.ChkwithWast.Text = "With Wastage"
        Me.ChkwithWast.UseVisualStyleBackColor = True
        '
        'dtpTodate
        '
        Me.dtpTodate.Location = New System.Drawing.Point(249, 13)
        Me.dtpTodate.Mask = "##/##/####"
        Me.dtpTodate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTodate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTodate.Name = "dtpTodate"
        Me.dtpTodate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTodate.Size = New System.Drawing.Size(93, 21)
        Me.dtpTodate.TabIndex = 3
        Me.dtpTodate.Text = "07/03/9998"
        Me.dtpTodate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'cmbactive
        '
        Me.cmbactive.FormattingEnabled = True
        Me.cmbactive.Location = New System.Drawing.Point(91, 302)
        Me.cmbactive.Name = "cmbactive"
        Me.cmbactive.Size = New System.Drawing.Size(134, 21)
        Me.cmbactive.TabIndex = 14
        '
        'lblFrom
        '
        Me.lblFrom.AutoSize = True
        Me.lblFrom.Location = New System.Drawing.Point(191, 16)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(52, 13)
        Me.lblFrom.TabIndex = 2
        Me.lblFrom.Text = "To Date"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 305)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(74, 13)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "Active Type"
        '
        'CmbAcname
        '
        Me.CmbAcname.FormattingEnabled = True
        Me.CmbAcname.Location = New System.Drawing.Point(91, 326)
        Me.CmbAcname.Name = "CmbAcname"
        Me.CmbAcname.Size = New System.Drawing.Size(278, 21)
        Me.CmbAcname.TabIndex = 16
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 326)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(54, 13)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "AcName"
        '
        'ChkApproval
        '
        Me.ChkApproval.AutoSize = True
        Me.ChkApproval.Location = New System.Drawing.Point(233, 395)
        Me.ChkApproval.Name = "ChkApproval"
        Me.ChkApproval.Size = New System.Drawing.Size(176, 17)
        Me.ChkApproval.TabIndex = 25
        Me.ChkApproval.Text = "With Approval Transaction"
        Me.ChkApproval.UseVisualStyleBackColor = True
        '
        'PnlMark
        '
        Me.PnlMark.Controls.Add(Me.rbtLocal)
        Me.PnlMark.Controls.Add(Me.rbtOutstation)
        Me.PnlMark.Controls.Add(Me.rbtBothMU)
        Me.PnlMark.Location = New System.Drawing.Point(85, 369)
        Me.PnlMark.Name = "PnlMark"
        Me.PnlMark.Size = New System.Drawing.Size(228, 22)
        Me.PnlMark.TabIndex = 22
        '
        'rbtLocal
        '
        Me.rbtLocal.AutoSize = True
        Me.rbtLocal.Location = New System.Drawing.Point(78, 3)
        Me.rbtLocal.Name = "rbtLocal"
        Me.rbtLocal.Size = New System.Drawing.Size(54, 17)
        Me.rbtLocal.TabIndex = 1
        Me.rbtLocal.Text = "Local"
        Me.rbtLocal.UseVisualStyleBackColor = True
        '
        'rbtOutstation
        '
        Me.rbtOutstation.AutoSize = True
        Me.rbtOutstation.Location = New System.Drawing.Point(148, 3)
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
        'chkCmbTranType
        '
        Me.chkCmbTranType.CheckOnClick = True
        Me.chkCmbTranType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbTranType.DropDownHeight = 1
        Me.chkCmbTranType.FormattingEnabled = True
        Me.chkCmbTranType.IntegralHeight = False
        Me.chkCmbTranType.Location = New System.Drawing.Point(94, 41)
        Me.chkCmbTranType.Name = "chkCmbTranType"
        Me.chkCmbTranType.Size = New System.Drawing.Size(248, 22)
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
        Me.Panel1.Location = New System.Drawing.Point(13, 276)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(426, 23)
        Me.Panel1.TabIndex = 12
        '
        'chkCustomer
        '
        Me.chkCustomer.AutoSize = True
        Me.chkCustomer.Location = New System.Drawing.Point(264, 4)
        Me.chkCustomer.Name = "chkCustomer"
        Me.chkCustomer.Size = New System.Drawing.Size(82, 17)
        Me.chkCustomer.TabIndex = 4
        Me.chkCustomer.Text = "Customer"
        Me.chkCustomer.UseVisualStyleBackColor = True
        '
        'chkInternal
        '
        Me.chkInternal.AutoSize = True
        Me.chkInternal.Location = New System.Drawing.Point(191, 4)
        Me.chkInternal.Name = "chkInternal"
        Me.chkInternal.Size = New System.Drawing.Size(71, 17)
        Me.chkInternal.TabIndex = 3
        Me.chkInternal.Text = "Internal"
        Me.chkInternal.UseVisualStyleBackColor = True
        '
        'chkOthers
        '
        Me.chkOthers.AutoSize = True
        Me.chkOthers.Location = New System.Drawing.Point(356, 4)
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
        Me.chkSmith.Location = New System.Drawing.Point(59, 4)
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
        Me.chkDealer.Location = New System.Drawing.Point(124, 4)
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
        Me.chkWithNillBalance.Location = New System.Drawing.Point(92, 395)
        Me.chkWithNillBalance.Name = "chkWithNillBalance"
        Me.chkWithNillBalance.Size = New System.Drawing.Size(121, 17)
        Me.chkWithNillBalance.TabIndex = 23
        Me.chkWithNillBalance.Text = "With Nill Balance"
        Me.chkWithNillBalance.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(14, 355)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(62, 13)
        Me.Label5.TabIndex = 17
        Me.Label5.Text = "Based On"
        '
        'dtpAsOnDate
        '
        Me.dtpAsOnDate.Location = New System.Drawing.Point(94, 13)
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
        Me.lblTo.Location = New System.Drawing.Point(17, 44)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(65, 13)
        Me.lblTo.TabIndex = 4
        Me.lblTo.Text = "Tran Type"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'frmSmithBalanceSummaryFormat3
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
        Me.Name = "frmSmithBalanceSummaryFormat3"
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
    Friend WithEvents lblFrom As System.Windows.Forms.Label
    Friend WithEvents PnlMark As System.Windows.Forms.Panel
    Friend WithEvents rbtLocal As System.Windows.Forms.RadioButton
    Friend WithEvents rbtOutstation As System.Windows.Forms.RadioButton
    Friend WithEvents rbtBothMU As System.Windows.Forms.RadioButton
    Friend WithEvents ChkApproval As System.Windows.Forms.CheckBox
    Friend WithEvents chkCustomer As System.Windows.Forms.CheckBox
    Friend WithEvents CmbAcname As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbactive As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ChkwithWast As System.Windows.Forms.CheckBox
    Friend WithEvents rbtNetWt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtPureWt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGrsWt As System.Windows.Forms.RadioButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
