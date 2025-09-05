<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ItemWiseStockFlow
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
        Me.ChkWithApproval = New System.Windows.Forms.CheckBox
        Me.ChkwithCumStk = New System.Windows.Forms.CheckBox
        Me.chkWithBeads = New System.Windows.Forms.CheckBox
        Me.CHKCOUNTER = New BrighttechPack.CheckedComboBox
        Me.chkAllCounter = New System.Windows.Forms.CheckBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.chkWithSubitem = New System.Windows.Forms.CheckBox
        Me.chkNonTag = New System.Windows.Forms.CheckBox
        Me.chkTag = New System.Windows.Forms.CheckBox
        Me.rbtOrderItemId = New System.Windows.Forms.RadioButton
        Me.rbtOrderItemName = New System.Windows.Forms.RadioButton
        Me.cmbMetal = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.chkCmbCompany = New BrighttechPack.CheckedComboBox
        Me.chkCmbItemName = New BrighttechPack.CheckedComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.chkAsOnDate = New System.Windows.Forms.CheckBox
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.GrpContainer.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GrpContainer
        '
        Me.GrpContainer.Controls.Add(Me.ChkWithApproval)
        Me.GrpContainer.Controls.Add(Me.ChkwithCumStk)
        Me.GrpContainer.Controls.Add(Me.chkWithBeads)
        Me.GrpContainer.Controls.Add(Me.CHKCOUNTER)
        Me.GrpContainer.Controls.Add(Me.chkAllCounter)
        Me.GrpContainer.Controls.Add(Me.Label7)
        Me.GrpContainer.Controls.Add(Me.chkWithSubitem)
        Me.GrpContainer.Controls.Add(Me.chkNonTag)
        Me.GrpContainer.Controls.Add(Me.chkTag)
        Me.GrpContainer.Controls.Add(Me.rbtOrderItemId)
        Me.GrpContainer.Controls.Add(Me.rbtOrderItemName)
        Me.GrpContainer.Controls.Add(Me.cmbMetal)
        Me.GrpContainer.Controls.Add(Me.Label2)
        Me.GrpContainer.Controls.Add(Me.chkCmbCompany)
        Me.GrpContainer.Controls.Add(Me.chkCmbItemName)
        Me.GrpContainer.Controls.Add(Me.Label5)
        Me.GrpContainer.Controls.Add(Me.Label)
        Me.GrpContainer.Controls.Add(Me.Label4)
        Me.GrpContainer.Controls.Add(Me.Label3)
        Me.GrpContainer.Controls.Add(Me.chkCmbCostCentre)
        Me.GrpContainer.Controls.Add(Me.Label1)
        Me.GrpContainer.Controls.Add(Me.chkAsOnDate)
        Me.GrpContainer.Controls.Add(Me.dtpTo)
        Me.GrpContainer.Controls.Add(Me.dtpFrom)
        Me.GrpContainer.Controls.Add(Me.btnExit)
        Me.GrpContainer.Controls.Add(Me.btnSearch)
        Me.GrpContainer.Controls.Add(Me.btnNew)
        Me.GrpContainer.Location = New System.Drawing.Point(24, 12)
        Me.GrpContainer.Name = "GrpContainer"
        Me.GrpContainer.Size = New System.Drawing.Size(491, 390)
        Me.GrpContainer.TabIndex = 0
        Me.GrpContainer.TabStop = False
        '
        'ChkWithApproval
        '
        Me.ChkWithApproval.AutoSize = True
        Me.ChkWithApproval.Location = New System.Drawing.Point(297, 290)
        Me.ChkWithApproval.Name = "ChkWithApproval"
        Me.ChkWithApproval.Size = New System.Drawing.Size(139, 17)
        Me.ChkWithApproval.TabIndex = 23
        Me.ChkWithApproval.Text = "Stock with approval"
        Me.ChkWithApproval.UseVisualStyleBackColor = True
        '
        'ChkwithCumStk
        '
        Me.ChkwithCumStk.AutoSize = True
        Me.ChkwithCumStk.Location = New System.Drawing.Point(130, 290)
        Me.ChkwithCumStk.Name = "ChkwithCumStk"
        Me.ChkwithCumStk.Size = New System.Drawing.Size(156, 17)
        Me.ChkwithCumStk.TabIndex = 22
        Me.ChkwithCumStk.Text = "With Cumulative Stock"
        Me.ChkwithCumStk.UseVisualStyleBackColor = True
        '
        'chkWithBeads
        '
        Me.chkWithBeads.AutoSize = True
        Me.chkWithBeads.Location = New System.Drawing.Point(298, 267)
        Me.chkWithBeads.Name = "chkWithBeads"
        Me.chkWithBeads.Size = New System.Drawing.Size(90, 17)
        Me.chkWithBeads.TabIndex = 21
        Me.chkWithBeads.Text = "With Beads"
        Me.chkWithBeads.UseVisualStyleBackColor = True
        '
        'CHKCOUNTER
        '
        Me.CHKCOUNTER.CheckOnClick = True
        Me.CHKCOUNTER.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.CHKCOUNTER.DropDownHeight = 1
        Me.CHKCOUNTER.FormattingEnabled = True
        Me.CHKCOUNTER.IntegralHeight = False
        Me.CHKCOUNTER.Location = New System.Drawing.Point(130, 207)
        Me.CHKCOUNTER.Name = "CHKCOUNTER"
        Me.CHKCOUNTER.Size = New System.Drawing.Size(234, 22)
        Me.CHKCOUNTER.TabIndex = 14
        Me.CHKCOUNTER.ValueSeparator = ", "
        Me.CHKCOUNTER.Visible = False
        '
        'chkAllCounter
        '
        Me.chkAllCounter.AutoSize = True
        Me.chkAllCounter.Location = New System.Drawing.Point(109, 212)
        Me.chkAllCounter.Name = "chkAllCounter"
        Me.chkAllCounter.Size = New System.Drawing.Size(15, 14)
        Me.chkAllCounter.TabIndex = 13
        Me.chkAllCounter.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(25, 208)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(99, 21)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "Item Counter"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkWithSubitem
        '
        Me.chkWithSubitem.AutoSize = True
        Me.chkWithSubitem.Location = New System.Drawing.Point(298, 237)
        Me.chkWithSubitem.Name = "chkWithSubitem"
        Me.chkWithSubitem.Size = New System.Drawing.Size(104, 17)
        Me.chkWithSubitem.TabIndex = 18
        Me.chkWithSubitem.Text = "With SubItem"
        Me.chkWithSubitem.UseVisualStyleBackColor = True
        '
        'chkNonTag
        '
        Me.chkNonTag.AutoSize = True
        Me.chkNonTag.Location = New System.Drawing.Point(183, 267)
        Me.chkNonTag.Name = "chkNonTag"
        Me.chkNonTag.Size = New System.Drawing.Size(69, 17)
        Me.chkNonTag.TabIndex = 20
        Me.chkNonTag.Text = "NonTag"
        Me.chkNonTag.UseVisualStyleBackColor = True
        '
        'chkTag
        '
        Me.chkTag.AutoSize = True
        Me.chkTag.Location = New System.Drawing.Point(130, 267)
        Me.chkTag.Name = "chkTag"
        Me.chkTag.Size = New System.Drawing.Size(47, 17)
        Me.chkTag.TabIndex = 19
        Me.chkTag.Text = "Tag"
        Me.chkTag.UseVisualStyleBackColor = True
        '
        'rbtOrderItemId
        '
        Me.rbtOrderItemId.AutoSize = True
        Me.rbtOrderItemId.Location = New System.Drawing.Point(129, 235)
        Me.rbtOrderItemId.Name = "rbtOrderItemId"
        Me.rbtOrderItemId.Size = New System.Drawing.Size(68, 17)
        Me.rbtOrderItemId.TabIndex = 16
        Me.rbtOrderItemId.TabStop = True
        Me.rbtOrderItemId.Text = "Item Id"
        Me.rbtOrderItemId.UseVisualStyleBackColor = True
        '
        'rbtOrderItemName
        '
        Me.rbtOrderItemName.AutoSize = True
        Me.rbtOrderItemName.Location = New System.Drawing.Point(203, 235)
        Me.rbtOrderItemName.Name = "rbtOrderItemName"
        Me.rbtOrderItemName.Size = New System.Drawing.Size(89, 17)
        Me.rbtOrderItemName.TabIndex = 17
        Me.rbtOrderItemName.TabStop = True
        Me.rbtOrderItemName.Text = "Item Name"
        Me.rbtOrderItemName.UseVisualStyleBackColor = True
        '
        'cmbMetal
        '
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(130, 66)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(234, 21)
        Me.cmbMetal.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(23, 141)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Company"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbCompany
        '
        Me.chkCmbCompany.CheckOnClick = True
        Me.chkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCompany.DropDownHeight = 1
        Me.chkCmbCompany.FormattingEnabled = True
        Me.chkCmbCompany.IntegralHeight = False
        Me.chkCmbCompany.Location = New System.Drawing.Point(130, 136)
        Me.chkCmbCompany.Name = "chkCmbCompany"
        Me.chkCmbCompany.Size = New System.Drawing.Size(323, 22)
        Me.chkCmbCompany.TabIndex = 9
        Me.chkCmbCompany.ValueSeparator = ", "
        '
        'chkCmbItemName
        '
        Me.chkCmbItemName.CheckOnClick = True
        Me.chkCmbItemName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbItemName.DropDownHeight = 1
        Me.chkCmbItemName.FormattingEnabled = True
        Me.chkCmbItemName.IntegralHeight = False
        Me.chkCmbItemName.Location = New System.Drawing.Point(130, 103)
        Me.chkCmbItemName.Name = "chkCmbItemName"
        Me.chkCmbItemName.Size = New System.Drawing.Size(234, 22)
        Me.chkCmbItemName.TabIndex = 7
        Me.chkCmbItemName.ValueSeparator = ", "
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(59, 232)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(59, 13)
        Me.Label5.TabIndex = 15
        Me.Label5.Text = "Order By"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(23, 176)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(76, 13)
        Me.Label.TabIndex = 10
        Me.Label.Text = "Cost Centre"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(23, 69)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(70, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "MetalName"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(23, 108)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "ItemName"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(130, 171)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(234, 22)
        Me.chkCmbCostCentre.TabIndex = 11
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(219, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(21, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "To"
        '
        'chkAsOnDate
        '
        Me.chkAsOnDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAsOnDate.Location = New System.Drawing.Point(23, 31)
        Me.chkAsOnDate.Name = "chkAsOnDate"
        Me.chkAsOnDate.Size = New System.Drawing.Size(95, 17)
        Me.chkAsOnDate.TabIndex = 0
        Me.chkAsOnDate.Text = "Date From"
        Me.chkAsOnDate.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(246, 29)
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
        Me.dtpFrom.Location = New System.Drawing.Point(130, 29)
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
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(264, 328)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 26
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(52, 328)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 24
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(158, 328)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 25
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.NewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 48)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ItemWiseStockFlow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(556, 438)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.GrpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.Name = "ItemWiseStockFlow"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ItemWiseStockFlow"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GrpContainer.ResumeLayout(False)
        Me.GrpContainer.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GrpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents chkCmbItemName As BrighttechPack.CheckedComboBox
    Friend WithEvents Label As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkAsOnDate As System.Windows.Forms.CheckBox
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkCmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents cmbMetal As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents rbtOrderItemName As System.Windows.Forms.RadioButton
    Friend WithEvents rbtOrderItemId As System.Windows.Forms.RadioButton
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents chkNonTag As System.Windows.Forms.CheckBox
    Friend WithEvents chkTag As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithSubitem As System.Windows.Forms.CheckBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents chkAllCounter As System.Windows.Forms.CheckBox
    Friend WithEvents CHKCOUNTER As BrighttechPack.CheckedComboBox
    Friend WithEvents chkWithBeads As System.Windows.Forms.CheckBox
    Friend WithEvents ChkwithCumStk As System.Windows.Forms.CheckBox
    Friend WithEvents ChkWithApproval As System.Windows.Forms.CheckBox
End Class
