<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmApprovalSummary
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
        Me.lblTo = New System.Windows.Forms.Label
        Me.cmbMetal = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.grpContainer = New System.Windows.Forms.Panel
        Me.txtPartyname = New System.Windows.Forms.ComboBox
        Me.chkWithDia = New System.Windows.Forms.CheckBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label = New System.Windows.Forms.Label
        Me.chkAsOnDate = New System.Windows.Forms.CheckBox
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.chkWithRunNo = New System.Windows.Forms.CheckBox
        Me.rbtItem = New System.Windows.Forms.RadioButton
        Me.rbtCounter = New System.Windows.Forms.RadioButton
        Me.rbtCustomer = New System.Windows.Forms.RadioButton
        Me.rbtEmployee = New System.Windows.Forms.RadioButton
        Me.Label7 = New System.Windows.Forms.Label
        Me.cmbSupplier = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.cmbPurity = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.cmbSubProduct = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.cmbProduct = New System.Windows.Forms.ComboBox
        Me.chkDetailedView = New System.Windows.Forms.CheckBox
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.chkwithval = New System.Windows.Forms.CheckBox
        Me.grpContainer.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(200, 10)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(21, 13)
        Me.lblTo.TabIndex = 2
        Me.lblTo.Text = "To"
        '
        'cmbMetal
        '
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(103, 62)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(287, 21)
        Me.cmbMetal.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 66)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(37, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Metal"
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(95, 311)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 22
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(201, 311)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 23
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(310, 311)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 24
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'grpContainer
        '
        Me.grpContainer.Controls.Add(Me.chkwithval)
        Me.grpContainer.Controls.Add(Me.txtPartyname)
        Me.grpContainer.Controls.Add(Me.chkWithDia)
        Me.grpContainer.Controls.Add(Me.Label1)
        Me.grpContainer.Controls.Add(Me.Label)
        Me.grpContainer.Controls.Add(Me.chkAsOnDate)
        Me.grpContainer.Controls.Add(Me.chkCmbCostCentre)
        Me.grpContainer.Controls.Add(Me.GroupBox1)
        Me.grpContainer.Controls.Add(Me.Label7)
        Me.grpContainer.Controls.Add(Me.cmbSupplier)
        Me.grpContainer.Controls.Add(Me.Label9)
        Me.grpContainer.Controls.Add(Me.cmbPurity)
        Me.grpContainer.Controls.Add(Me.Label6)
        Me.grpContainer.Controls.Add(Me.cmbSubProduct)
        Me.grpContainer.Controls.Add(Me.Label5)
        Me.grpContainer.Controls.Add(Me.cmbProduct)
        Me.grpContainer.Controls.Add(Me.chkDetailedView)
        Me.grpContainer.Controls.Add(Me.dtpTo)
        Me.grpContainer.Controls.Add(Me.dtpFrom)
        Me.grpContainer.Controls.Add(Me.Label3)
        Me.grpContainer.Controls.Add(Me.btnExit)
        Me.grpContainer.Controls.Add(Me.lblTo)
        Me.grpContainer.Controls.Add(Me.btnNew)
        Me.grpContainer.Controls.Add(Me.cmbMetal)
        Me.grpContainer.Controls.Add(Me.btnSearch)
        Me.grpContainer.Location = New System.Drawing.Point(192, 150)
        Me.grpContainer.Name = "grpContainer"
        Me.grpContainer.Size = New System.Drawing.Size(436, 355)
        Me.grpContainer.TabIndex = 0
        '
        'txtPartyname
        '
        Me.txtPartyname.FormattingEnabled = True
        Me.txtPartyname.Location = New System.Drawing.Point(103, 142)
        Me.txtPartyname.Name = "txtPartyname"
        Me.txtPartyname.Size = New System.Drawing.Size(287, 21)
        Me.txtPartyname.TabIndex = 12
        '
        'chkWithDia
        '
        Me.chkWithDia.AutoSize = True
        Me.chkWithDia.Location = New System.Drawing.Point(218, 288)
        Me.chkWithDia.Name = "chkWithDia"
        Me.chkWithDia.Size = New System.Drawing.Size(74, 17)
        Me.chkWithDia.TabIndex = 20
        Me.chkWithDia.Text = "With Dia"
        Me.chkWithDia.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 145)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(74, 13)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "Party Name"
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(3, 37)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(76, 13)
        Me.Label.TabIndex = 4
        Me.Label.Text = "Cost Centre"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkAsOnDate
        '
        Me.chkAsOnDate.AutoSize = True
        Me.chkAsOnDate.Location = New System.Drawing.Point(6, 9)
        Me.chkAsOnDate.Name = "chkAsOnDate"
        Me.chkAsOnDate.Size = New System.Drawing.Size(91, 17)
        Me.chkAsOnDate.TabIndex = 0
        Me.chkAsOnDate.Text = "As On Date"
        Me.chkAsOnDate.UseVisualStyleBackColor = True
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(103, 34)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(287, 22)
        Me.chkCmbCostCentre.TabIndex = 5
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkWithRunNo)
        Me.GroupBox1.Controls.Add(Me.rbtItem)
        Me.GroupBox1.Controls.Add(Me.rbtCounter)
        Me.GroupBox1.Controls.Add(Me.rbtCustomer)
        Me.GroupBox1.Controls.Add(Me.rbtEmployee)
        Me.GroupBox1.Location = New System.Drawing.Point(95, 219)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(303, 63)
        Me.GroupBox1.TabIndex = 18
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Group By"
        '
        'chkWithRunNo
        '
        Me.chkWithRunNo.AutoSize = True
        Me.chkWithRunNo.Location = New System.Drawing.Point(10, 41)
        Me.chkWithRunNo.Name = "chkWithRunNo"
        Me.chkWithRunNo.Size = New System.Drawing.Size(92, 17)
        Me.chkWithRunNo.TabIndex = 4
        Me.chkWithRunNo.Text = "With RunNo"
        Me.chkWithRunNo.UseVisualStyleBackColor = True
        '
        'rbtItem
        '
        Me.rbtItem.AutoSize = True
        Me.rbtItem.Location = New System.Drawing.Point(10, 18)
        Me.rbtItem.Name = "rbtItem"
        Me.rbtItem.Size = New System.Drawing.Size(52, 17)
        Me.rbtItem.TabIndex = 0
        Me.rbtItem.TabStop = True
        Me.rbtItem.Text = "Item"
        Me.rbtItem.UseVisualStyleBackColor = True
        '
        'rbtCounter
        '
        Me.rbtCounter.AutoSize = True
        Me.rbtCounter.Location = New System.Drawing.Point(62, 18)
        Me.rbtCounter.Name = "rbtCounter"
        Me.rbtCounter.Size = New System.Drawing.Size(71, 17)
        Me.rbtCounter.TabIndex = 1
        Me.rbtCounter.TabStop = True
        Me.rbtCounter.Text = "Counter"
        Me.rbtCounter.UseVisualStyleBackColor = True
        '
        'rbtCustomer
        '
        Me.rbtCustomer.AutoSize = True
        Me.rbtCustomer.Location = New System.Drawing.Point(215, 19)
        Me.rbtCustomer.Name = "rbtCustomer"
        Me.rbtCustomer.Size = New System.Drawing.Size(81, 17)
        Me.rbtCustomer.TabIndex = 3
        Me.rbtCustomer.TabStop = True
        Me.rbtCustomer.Text = "Customer"
        Me.rbtCustomer.UseVisualStyleBackColor = True
        '
        'rbtEmployee
        '
        Me.rbtEmployee.AutoSize = True
        Me.rbtEmployee.Location = New System.Drawing.Point(133, 19)
        Me.rbtEmployee.Name = "rbtEmployee"
        Me.rbtEmployee.Size = New System.Drawing.Size(81, 17)
        Me.rbtEmployee.TabIndex = 2
        Me.rbtEmployee.TabStop = True
        Me.rbtEmployee.Text = "Employee"
        Me.rbtEmployee.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(3, 201)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(58, 13)
        Me.Label7.TabIndex = 16
        Me.Label7.Text = "Designer"
        '
        'cmbSupplier
        '
        Me.cmbSupplier.FormattingEnabled = True
        Me.cmbSupplier.Location = New System.Drawing.Point(103, 197)
        Me.cmbSupplier.Name = "cmbSupplier"
        Me.cmbSupplier.Size = New System.Drawing.Size(287, 21)
        Me.cmbSupplier.TabIndex = 17
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(3, 173)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(40, 13)
        Me.Label9.TabIndex = 14
        Me.Label9.Text = "Purity"
        '
        'cmbPurity
        '
        Me.cmbPurity.FormattingEnabled = True
        Me.cmbPurity.Location = New System.Drawing.Point(103, 169)
        Me.cmbPurity.Name = "cmbPurity"
        Me.cmbPurity.Size = New System.Drawing.Size(287, 21)
        Me.cmbPurity.TabIndex = 15
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(3, 118)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(56, 13)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "SubItem"
        '
        'cmbSubProduct
        '
        Me.cmbSubProduct.FormattingEnabled = True
        Me.cmbSubProduct.Location = New System.Drawing.Point(103, 115)
        Me.cmbSubProduct.Name = "cmbSubProduct"
        Me.cmbSubProduct.Size = New System.Drawing.Size(287, 21)
        Me.cmbSubProduct.TabIndex = 11
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(3, 92)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(34, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Item"
        '
        'cmbProduct
        '
        Me.cmbProduct.FormattingEnabled = True
        Me.cmbProduct.Location = New System.Drawing.Point(103, 88)
        Me.cmbProduct.Name = "cmbProduct"
        Me.cmbProduct.Size = New System.Drawing.Size(287, 21)
        Me.cmbProduct.TabIndex = 9
        '
        'chkDetailedView
        '
        Me.chkDetailedView.AutoSize = True
        Me.chkDetailedView.Location = New System.Drawing.Point(105, 288)
        Me.chkDetailedView.Name = "chkDetailedView"
        Me.chkDetailedView.Size = New System.Drawing.Size(104, 17)
        Me.chkDetailedView.TabIndex = 19
        Me.chkDetailedView.Text = "Detailed View"
        Me.chkDetailedView.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(233, 7)
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
        Me.dtpFrom.Location = New System.Drawing.Point(103, 7)
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
        'chkwithval
        '
        Me.chkwithval.AutoSize = True
        Me.chkwithval.Location = New System.Drawing.Point(298, 288)
        Me.chkwithval.Name = "chkwithval"
        Me.chkwithval.Size = New System.Drawing.Size(87, 17)
        Me.chkwithval.TabIndex = 21
        Me.chkwithval.Text = "With Value"
        Me.chkwithval.UseVisualStyleBackColor = True
        '
        'frmApprovalSummary
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1009, 618)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.grpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmApprovalSummary"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Approval Summary"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpContainer.ResumeLayout(False)
        Me.grpContainer.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents cmbMetal As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents grpContainer As System.Windows.Forms.Panel
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents chkDetailedView As System.Windows.Forms.CheckBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cmbSupplier As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents cmbPurity As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbSubProduct As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cmbProduct As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbtItem As System.Windows.Forms.RadioButton
    Friend WithEvents rbtCounter As System.Windows.Forms.RadioButton
    Friend WithEvents rbtCustomer As System.Windows.Forms.RadioButton
    Friend WithEvents rbtEmployee As System.Windows.Forms.RadioButton
    Friend WithEvents chkAsOnDate As System.Windows.Forms.CheckBox
    Friend WithEvents Label As System.Windows.Forms.Label
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkWithRunNo As System.Windows.Forms.CheckBox
    Friend WithEvents chkWithDia As System.Windows.Forms.CheckBox
    Friend WithEvents txtPartyname As System.Windows.Forms.ComboBox
    Friend WithEvents chkwithval As System.Windows.Forms.CheckBox
End Class
