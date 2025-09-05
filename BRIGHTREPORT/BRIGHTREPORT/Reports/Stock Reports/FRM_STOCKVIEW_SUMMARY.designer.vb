<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRM_STOCKVIEW_SUMMARY
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
        Me.grpContainer = New System.Windows.Forms.GroupBox()
        Me.ChkRepairDet = New System.Windows.Forms.CheckBox()
        Me.chkMIMRApproval = New System.Windows.Forms.CheckBox()
        Me.cmbstocktype = New System.Windows.Forms.ComboBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.chkWithAlloy = New System.Windows.Forms.CheckBox()
        Me.ChkWithApproval = New System.Windows.Forms.CheckBox()
        Me.chkCmbTransation = New BrighttechPack.CheckedComboBox()
        Me.chkCmbItem = New BrighttechPack.CheckedComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.rbtBoth = New System.Windows.Forms.RadioButton()
        Me.rbtWeight = New System.Windows.Forms.RadioButton()
        Me.rbtPcs = New System.Windows.Forms.RadioButton()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.rbtBothWt = New System.Windows.Forms.RadioButton()
        Me.rbtNetWt = New System.Windows.Forms.RadioButton()
        Me.rbtGrsWeight = New System.Windows.Forms.RadioButton()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbCategoryGroup = New System.Windows.Forms.ComboBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.chkLstCategory = New System.Windows.Forms.CheckedListBox()
        Me.chkCategorySelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox()
        Me.chkMetalSelectAll = New System.Windows.Forms.CheckBox()
        Me.chkLstMetal = New System.Windows.Forms.CheckedListBox()
        Me.chkCostCentreSelectAll = New System.Windows.Forms.CheckBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.grpContainer.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpContainer
        '
        Me.grpContainer.Controls.Add(Me.ChkRepairDet)
        Me.grpContainer.Controls.Add(Me.chkMIMRApproval)
        Me.grpContainer.Controls.Add(Me.cmbstocktype)
        Me.grpContainer.Controls.Add(Me.Label10)
        Me.grpContainer.Controls.Add(Me.chkWithAlloy)
        Me.grpContainer.Controls.Add(Me.ChkWithApproval)
        Me.grpContainer.Controls.Add(Me.chkCmbTransation)
        Me.grpContainer.Controls.Add(Me.chkCmbItem)
        Me.grpContainer.Controls.Add(Me.Label9)
        Me.grpContainer.Controls.Add(Me.Label8)
        Me.grpContainer.Controls.Add(Me.Panel2)
        Me.grpContainer.Controls.Add(Me.Panel1)
        Me.grpContainer.Controls.Add(Me.cmbCategoryGroup)
        Me.grpContainer.Controls.Add(Me.dtpTo)
        Me.grpContainer.Controls.Add(Me.dtpFrom)
        Me.grpContainer.Controls.Add(Me.chkLstCategory)
        Me.grpContainer.Controls.Add(Me.chkCategorySelectAll)
        Me.grpContainer.Controls.Add(Me.chkLstCostCentre)
        Me.grpContainer.Controls.Add(Me.chkMetalSelectAll)
        Me.grpContainer.Controls.Add(Me.chkLstMetal)
        Me.grpContainer.Controls.Add(Me.chkCostCentreSelectAll)
        Me.grpContainer.Controls.Add(Me.Label2)
        Me.grpContainer.Controls.Add(Me.Label4)
        Me.grpContainer.Controls.Add(Me.Label6)
        Me.grpContainer.Controls.Add(Me.Label1)
        Me.grpContainer.Controls.Add(Me.btnSearch)
        Me.grpContainer.Controls.Add(Me.btnExit)
        Me.grpContainer.Controls.Add(Me.btnNew)
        Me.grpContainer.Location = New System.Drawing.Point(243, 19)
        Me.grpContainer.Name = "grpContainer"
        Me.grpContainer.Size = New System.Drawing.Size(424, 528)
        Me.grpContainer.TabIndex = 0
        Me.grpContainer.TabStop = False
        '
        'ChkRepairDet
        '
        Me.ChkRepairDet.AutoSize = True
        Me.ChkRepairDet.Location = New System.Drawing.Point(8, 463)
        Me.ChkRepairDet.Name = "ChkRepairDet"
        Me.ChkRepairDet.Size = New System.Drawing.Size(92, 17)
        Me.ChkRepairDet.TabIndex = 24
        Me.ChkRepairDet.Text = "With Repair"
        Me.ChkRepairDet.UseVisualStyleBackColor = True
        '
        'chkMIMRApproval
        '
        Me.chkMIMRApproval.AutoSize = True
        Me.chkMIMRApproval.Checked = True
        Me.chkMIMRApproval.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMIMRApproval.Location = New System.Drawing.Point(174, 442)
        Me.chkMIMRApproval.Name = "chkMIMRApproval"
        Me.chkMIMRApproval.Size = New System.Drawing.Size(141, 17)
        Me.chkMIMRApproval.TabIndex = 22
        Me.chkMIMRApproval.Text = "With MIMR Approval"
        Me.chkMIMRApproval.UseVisualStyleBackColor = True
        '
        'cmbstocktype
        '
        Me.cmbstocktype.FormattingEnabled = True
        Me.cmbstocktype.Location = New System.Drawing.Point(118, 382)
        Me.cmbstocktype.Name = "cmbstocktype"
        Me.cmbstocktype.Size = New System.Drawing.Size(290, 21)
        Me.cmbstocktype.TabIndex = 19
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(9, 386)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(70, 13)
        Me.Label10.TabIndex = 18
        Me.Label10.Text = "Stock Type"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkWithAlloy
        '
        Me.chkWithAlloy.AutoSize = True
        Me.chkWithAlloy.Location = New System.Drawing.Point(324, 442)
        Me.chkWithAlloy.Name = "chkWithAlloy"
        Me.chkWithAlloy.Size = New System.Drawing.Size(83, 17)
        Me.chkWithAlloy.TabIndex = 23
        Me.chkWithAlloy.Text = "With Alloy"
        Me.chkWithAlloy.UseVisualStyleBackColor = True
        '
        'ChkWithApproval
        '
        Me.ChkWithApproval.AutoSize = True
        Me.ChkWithApproval.Checked = True
        Me.ChkWithApproval.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkWithApproval.Location = New System.Drawing.Point(9, 442)
        Me.ChkWithApproval.Name = "ChkWithApproval"
        Me.ChkWithApproval.Size = New System.Drawing.Size(156, 17)
        Me.ChkWithApproval.TabIndex = 21
        Me.ChkWithApproval.Text = "With Counter Approval"
        Me.ChkWithApproval.UseVisualStyleBackColor = True
        '
        'chkCmbTransation
        '
        Me.chkCmbTransation.CheckOnClick = True
        Me.chkCmbTransation.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbTransation.DropDownHeight = 1
        Me.chkCmbTransation.FormattingEnabled = True
        Me.chkCmbTransation.IntegralHeight = False
        Me.chkCmbTransation.Location = New System.Drawing.Point(118, 356)
        Me.chkCmbTransation.Name = "chkCmbTransation"
        Me.chkCmbTransation.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbTransation.TabIndex = 17
        Me.chkCmbTransation.ValueSeparator = ", "
        '
        'chkCmbItem
        '
        Me.chkCmbItem.CheckOnClick = True
        Me.chkCmbItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbItem.DropDownHeight = 1
        Me.chkCmbItem.FormattingEnabled = True
        Me.chkCmbItem.IntegralHeight = False
        Me.chkCmbItem.Location = New System.Drawing.Point(118, 331)
        Me.chkCmbItem.Name = "chkCmbItem"
        Me.chkCmbItem.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbItem.TabIndex = 15
        Me.chkCmbItem.ValueSeparator = ", "
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(9, 361)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(72, 13)
        Me.Label9.TabIndex = 16
        Me.Label9.Text = "Transaction"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(9, 336)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(67, 13)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "ItemName"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rbtBoth)
        Me.Panel2.Controls.Add(Me.rbtWeight)
        Me.Panel2.Controls.Add(Me.rbtPcs)
        Me.Panel2.Location = New System.Drawing.Point(118, 150)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(186, 26)
        Me.Panel2.TabIndex = 9
        '
        'rbtBoth
        '
        Me.rbtBoth.AutoSize = True
        Me.rbtBoth.Location = New System.Drawing.Point(124, 4)
        Me.rbtBoth.Name = "rbtBoth"
        Me.rbtBoth.Size = New System.Drawing.Size(51, 17)
        Me.rbtBoth.TabIndex = 2
        Me.rbtBoth.TabStop = True
        Me.rbtBoth.Text = "Both"
        Me.rbtBoth.UseVisualStyleBackColor = True
        '
        'rbtWeight
        '
        Me.rbtWeight.AutoSize = True
        Me.rbtWeight.Location = New System.Drawing.Point(5, 4)
        Me.rbtWeight.Name = "rbtWeight"
        Me.rbtWeight.Size = New System.Drawing.Size(63, 17)
        Me.rbtWeight.TabIndex = 0
        Me.rbtWeight.TabStop = True
        Me.rbtWeight.Text = "Weight"
        Me.rbtWeight.UseVisualStyleBackColor = True
        '
        'rbtPcs
        '
        Me.rbtPcs.AutoSize = True
        Me.rbtPcs.Location = New System.Drawing.Point(73, 4)
        Me.rbtPcs.Name = "rbtPcs"
        Me.rbtPcs.Size = New System.Drawing.Size(44, 17)
        Me.rbtPcs.TabIndex = 1
        Me.rbtPcs.TabStop = True
        Me.rbtPcs.Text = "Pcs"
        Me.rbtPcs.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbtBothWt)
        Me.Panel1.Controls.Add(Me.rbtNetWt)
        Me.Panel1.Controls.Add(Me.rbtGrsWeight)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Location = New System.Drawing.Point(5, 408)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(403, 29)
        Me.Panel1.TabIndex = 20
        '
        'rbtBothWt
        '
        Me.rbtBothWt.AutoSize = True
        Me.rbtBothWt.Location = New System.Drawing.Point(275, 6)
        Me.rbtBothWt.Name = "rbtBothWt"
        Me.rbtBothWt.Size = New System.Drawing.Size(51, 17)
        Me.rbtBothWt.TabIndex = 3
        Me.rbtBothWt.Text = "Both"
        Me.rbtBothWt.UseVisualStyleBackColor = True
        '
        'rbtNetWt
        '
        Me.rbtNetWt.AutoSize = True
        Me.rbtNetWt.Location = New System.Drawing.Point(196, 6)
        Me.rbtNetWt.Name = "rbtNetWt"
        Me.rbtNetWt.Size = New System.Drawing.Size(59, 17)
        Me.rbtNetWt.TabIndex = 2
        Me.rbtNetWt.Text = "NetWt"
        Me.rbtNetWt.UseVisualStyleBackColor = True
        '
        'rbtGrsWeight
        '
        Me.rbtGrsWeight.AutoSize = True
        Me.rbtGrsWeight.Checked = True
        Me.rbtGrsWeight.Location = New System.Drawing.Point(118, 6)
        Me.rbtGrsWeight.Name = "rbtGrsWeight"
        Me.rbtGrsWeight.Size = New System.Drawing.Size(60, 17)
        Me.rbtGrsWeight.TabIndex = 1
        Me.rbtGrsWeight.TabStop = True
        Me.rbtGrsWeight.Text = "GrsWt"
        Me.rbtGrsWeight.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(3, 8)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(78, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Wt based on"
        '
        'cmbCategoryGroup
        '
        Me.cmbCategoryGroup.FormattingEnabled = True
        Me.cmbCategoryGroup.Location = New System.Drawing.Point(118, 182)
        Me.cmbCategoryGroup.Name = "cmbCategoryGroup"
        Me.cmbCategoryGroup.Size = New System.Drawing.Size(298, 21)
        Me.cmbCategoryGroup.TabIndex = 11
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(214, 23)
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
        Me.dtpFrom.Location = New System.Drawing.Point(80, 23)
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
        'chkLstCategory
        '
        Me.chkLstCategory.FormattingEnabled = True
        Me.chkLstCategory.Location = New System.Drawing.Point(8, 240)
        Me.chkLstCategory.Name = "chkLstCategory"
        Me.chkLstCategory.Size = New System.Drawing.Size(407, 84)
        Me.chkLstCategory.TabIndex = 13
        '
        'chkCategorySelectAll
        '
        Me.chkCategorySelectAll.AutoSize = True
        Me.chkCategorySelectAll.Location = New System.Drawing.Point(11, 223)
        Me.chkCategorySelectAll.Name = "chkCategorySelectAll"
        Me.chkCategorySelectAll.Size = New System.Drawing.Size(79, 17)
        Me.chkCategorySelectAll.TabIndex = 12
        Me.chkCategorySelectAll.Text = "Category"
        Me.chkCategorySelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(9, 77)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(201, 68)
        Me.chkLstCostCentre.TabIndex = 5
        '
        'chkMetalSelectAll
        '
        Me.chkMetalSelectAll.AutoSize = True
        Me.chkMetalSelectAll.Location = New System.Drawing.Point(220, 58)
        Me.chkMetalSelectAll.Name = "chkMetalSelectAll"
        Me.chkMetalSelectAll.Size = New System.Drawing.Size(56, 17)
        Me.chkMetalSelectAll.TabIndex = 6
        Me.chkMetalSelectAll.Text = "Metal"
        Me.chkMetalSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstMetal
        '
        Me.chkLstMetal.FormattingEnabled = True
        Me.chkLstMetal.Location = New System.Drawing.Point(217, 77)
        Me.chkLstMetal.Name = "chkLstMetal"
        Me.chkLstMetal.Size = New System.Drawing.Size(199, 68)
        Me.chkLstMetal.TabIndex = 7
        '
        'chkCostCentreSelectAll
        '
        Me.chkCostCentreSelectAll.AutoSize = True
        Me.chkCostCentreSelectAll.Location = New System.Drawing.Point(12, 58)
        Me.chkCostCentreSelectAll.Name = "chkCostCentreSelectAll"
        Me.chkCostCentreSelectAll.Size = New System.Drawing.Size(95, 17)
        Me.chkCostCentreSelectAll.TabIndex = 4
        Me.chkCostCentreSelectAll.Text = "Cost Centre"
        Me.chkCostCentreSelectAll.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(183, 26)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(20, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(9, 182)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(99, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Category Group"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(9, 154)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(49, 13)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "Display"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date From"
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(54, 486)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 25
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(266, 486)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 27
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(160, 486)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 26
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
        'FRM_STOCKVIEW_SUMMARY
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(917, 572)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.grpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "FRM_STOCKVIEW_SUMMARY"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Metal Ornament Stock View (Summary)"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpContainer.ResumeLayout(False)
        Me.grpContainer.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents chkLstCategory As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkCategorySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkMetalSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstMetal As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkCostCentreSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents cmbCategoryGroup As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rbtNetWt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGrsWeight As System.Windows.Forms.RadioButton
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rbtPcs As System.Windows.Forms.RadioButton
    Friend WithEvents rbtBoth As System.Windows.Forms.RadioButton
    Friend WithEvents rbtWeight As System.Windows.Forms.RadioButton
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents chkCmbItem As BrighttechPack.CheckedComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents chkCmbTransation As BrighttechPack.CheckedComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents ChkWithApproval As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithAlloy As System.Windows.Forms.CheckBox
    Friend WithEvents rbtBothWt As System.Windows.Forms.RadioButton
    Friend WithEvents cmbstocktype As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents chkMIMRApproval As CheckBox
    Friend WithEvents ChkRepairDet As CheckBox
End Class
