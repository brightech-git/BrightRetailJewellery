<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSizewiseStockReport
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
        Me.grpControls = New System.Windows.Forms.GroupBox
        Me.chkWithApproval = New System.Windows.Forms.CheckBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.cmbMetal = New BrighttechPack.CheckedComboBox
        Me.chkAsOnDate = New System.Windows.Forms.CheckBox
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.lblTo = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.chkCmbDesigner = New BrighttechPack.CheckedComboBox
        Me.chkCmbItemType = New BrighttechPack.CheckedComboBox
        Me.chkCmbCompany = New BrighttechPack.CheckedComboBox
        Me.cmbCategory = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.chkCmbItem = New BrighttechPack.CheckedComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.cmbSize = New System.Windows.Forms.ComboBox
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.cmbCostCentre = New System.Windows.Forms.ComboBox
        Me.cmbSubItemName = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.grpControls.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpControls
        '
        Me.grpControls.Controls.Add(Me.chkWithApproval)
        Me.grpControls.Controls.Add(Me.Label5)
        Me.grpControls.Controls.Add(Me.cmbMetal)
        Me.grpControls.Controls.Add(Me.chkAsOnDate)
        Me.grpControls.Controls.Add(Me.dtpTo)
        Me.grpControls.Controls.Add(Me.dtpFrom)
        Me.grpControls.Controls.Add(Me.lblTo)
        Me.grpControls.Controls.Add(Me.Label10)
        Me.grpControls.Controls.Add(Me.chkCmbDesigner)
        Me.grpControls.Controls.Add(Me.chkCmbItemType)
        Me.grpControls.Controls.Add(Me.chkCmbCompany)
        Me.grpControls.Controls.Add(Me.cmbCategory)
        Me.grpControls.Controls.Add(Me.Label6)
        Me.grpControls.Controls.Add(Me.Label8)
        Me.grpControls.Controls.Add(Me.Label9)
        Me.grpControls.Controls.Add(Me.Label7)
        Me.grpControls.Controls.Add(Me.chkCmbItem)
        Me.grpControls.Controls.Add(Me.Label4)
        Me.grpControls.Controls.Add(Me.cmbSize)
        Me.grpControls.Controls.Add(Me.btnNew)
        Me.grpControls.Controls.Add(Me.btnExit)
        Me.grpControls.Controls.Add(Me.cmbCostCentre)
        Me.grpControls.Controls.Add(Me.cmbSubItemName)
        Me.grpControls.Controls.Add(Me.Label3)
        Me.grpControls.Controls.Add(Me.Label2)
        Me.grpControls.Controls.Add(Me.Label1)
        Me.grpControls.Controls.Add(Me.btnView_Search)
        Me.grpControls.Location = New System.Drawing.Point(296, 106)
        Me.grpControls.Name = "grpControls"
        Me.grpControls.Size = New System.Drawing.Size(414, 423)
        Me.grpControls.TabIndex = 0
        Me.grpControls.TabStop = False
        '
        'chkWithApproval
        '
        Me.chkWithApproval.AutoSize = True
        Me.chkWithApproval.Checked = True
        Me.chkWithApproval.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkWithApproval.Location = New System.Drawing.Point(125, 309)
        Me.chkWithApproval.Name = "chkWithApproval"
        Me.chkWithApproval.Size = New System.Drawing.Size(93, 17)
        Me.chkWithApproval.TabIndex = 23
        Me.chkWithApproval.Text = "With Approval"
        Me.chkWithApproval.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(8, 309)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(49, 13)
        Me.Label5.TabIndex = 22
        Me.Label5.Text = "Approval"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbMetal
        '
        Me.cmbMetal.CheckOnClick = True
        Me.cmbMetal.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbMetal.DropDownHeight = 1
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.IntegralHeight = False
        Me.cmbMetal.Location = New System.Drawing.Point(124, 43)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(204, 21)
        Me.cmbMetal.TabIndex = 5
        Me.cmbMetal.ValueSeparator = ", "
        '
        'chkAsOnDate
        '
        Me.chkAsOnDate.AutoSize = True
        Me.chkAsOnDate.Checked = True
        Me.chkAsOnDate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAsOnDate.Location = New System.Drawing.Point(9, 16)
        Me.chkAsOnDate.Name = "chkAsOnDate"
        Me.chkAsOnDate.Size = New System.Drawing.Size(81, 17)
        Me.chkAsOnDate.TabIndex = 0
        Me.chkAsOnDate.Text = "As On Date"
        Me.chkAsOnDate.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(250, 15)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(78, 20)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(124, 15)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(82, 20)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(212, 17)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(20, 13)
        Me.lblTo.TabIndex = 2
        Me.lblTo.Text = "To"
        Me.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(6, 158)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(112, 21)
        Me.Label10.TabIndex = 12
        Me.Label10.Text = "Size Name"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbDesigner
        '
        Me.chkCmbDesigner.CheckOnClick = True
        Me.chkCmbDesigner.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbDesigner.DropDownHeight = 1
        Me.chkCmbDesigner.FormattingEnabled = True
        Me.chkCmbDesigner.IntegralHeight = False
        Me.chkCmbDesigner.Location = New System.Drawing.Point(124, 218)
        Me.chkCmbDesigner.Name = "chkCmbDesigner"
        Me.chkCmbDesigner.Size = New System.Drawing.Size(204, 21)
        Me.chkCmbDesigner.TabIndex = 17
        Me.chkCmbDesigner.ValueSeparator = ", "
        '
        'chkCmbItemType
        '
        Me.chkCmbItemType.CheckOnClick = True
        Me.chkCmbItemType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbItemType.DropDownHeight = 1
        Me.chkCmbItemType.FormattingEnabled = True
        Me.chkCmbItemType.IntegralHeight = False
        Me.chkCmbItemType.Location = New System.Drawing.Point(124, 278)
        Me.chkCmbItemType.Name = "chkCmbItemType"
        Me.chkCmbItemType.Size = New System.Drawing.Size(204, 21)
        Me.chkCmbItemType.TabIndex = 21
        Me.chkCmbItemType.ValueSeparator = ", "
        '
        'chkCmbCompany
        '
        Me.chkCmbCompany.CheckOnClick = True
        Me.chkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCompany.DropDownHeight = 1
        Me.chkCmbCompany.FormattingEnabled = True
        Me.chkCmbCompany.IntegralHeight = False
        Me.chkCmbCompany.Location = New System.Drawing.Point(124, 249)
        Me.chkCmbCompany.Name = "chkCmbCompany"
        Me.chkCmbCompany.Size = New System.Drawing.Size(204, 21)
        Me.chkCmbCompany.TabIndex = 19
        Me.chkCmbCompany.ValueSeparator = ", "
        '
        'cmbCategory
        '
        Me.cmbCategory.FormattingEnabled = True
        Me.cmbCategory.Location = New System.Drawing.Point(124, 189)
        Me.cmbCategory.Name = "cmbCategory"
        Me.cmbCategory.Size = New System.Drawing.Size(204, 21)
        Me.cmbCategory.TabIndex = 15
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 192)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(49, 13)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "Category"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(8, 280)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(54, 13)
        Me.Label8.TabIndex = 20
        Me.Label8.Text = "Item Type"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(7, 251)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(51, 13)
        Me.Label9.TabIndex = 18
        Me.Label9.Text = "Company"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(7, 221)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(49, 13)
        Me.Label7.TabIndex = 16
        Me.Label7.Text = "Designer"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbItem
        '
        Me.chkCmbItem.CheckOnClick = True
        Me.chkCmbItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbItem.DropDownHeight = 1
        Me.chkCmbItem.FormattingEnabled = True
        Me.chkCmbItem.IntegralHeight = False
        Me.chkCmbItem.Location = New System.Drawing.Point(124, 73)
        Me.chkCmbItem.Name = "chkCmbItem"
        Me.chkCmbItem.Size = New System.Drawing.Size(204, 21)
        Me.chkCmbItem.TabIndex = 7
        Me.chkCmbItem.ValueSeparator = ", "
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(6, 42)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(58, 21)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Metal"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbSize
        '
        Me.cmbSize.FormattingEnabled = True
        Me.cmbSize.Location = New System.Drawing.Point(124, 159)
        Me.cmbSize.Name = "cmbSize"
        Me.cmbSize.Size = New System.Drawing.Size(204, 21)
        Me.cmbSize.TabIndex = 13
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(147, 352)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 25
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(255, 352)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 26
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'cmbCostCentre
        '
        Me.cmbCostCentre.FormattingEnabled = True
        Me.cmbCostCentre.Location = New System.Drawing.Point(124, 130)
        Me.cmbCostCentre.Name = "cmbCostCentre"
        Me.cmbCostCentre.Size = New System.Drawing.Size(204, 21)
        Me.cmbCostCentre.TabIndex = 11
        '
        'cmbSubItemName
        '
        Me.cmbSubItemName.FormattingEnabled = True
        Me.cmbSubItemName.Location = New System.Drawing.Point(124, 102)
        Me.cmbSubItemName.Name = "cmbSubItemName"
        Me.cmbSubItemName.Size = New System.Drawing.Size(204, 21)
        Me.cmbSubItemName.TabIndex = 9
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(6, 130)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(112, 21)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(6, 102)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(112, 21)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Sub Item "
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(6, 72)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(112, 21)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Item Name"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(39, 352)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 24
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(121, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(120, 22)
        Me.NewToolStripMenuItem.Text = "New "
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(120, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'frmSizewiseStockReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1020, 630)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.grpControls)
        Me.KeyPreview = True
        Me.Name = "frmSizewiseStockReport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Sizewise Stock Report"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpControls.ResumeLayout(False)
        Me.grpControls.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpControls As System.Windows.Forms.GroupBox
    Friend WithEvents chkCmbItem As BrighttechPack.CheckedComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmbSize As System.Windows.Forms.ComboBox
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents cmbCostCentre As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSubItemName As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents chkCmbDesigner As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbItemType As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents cmbCategory As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents chkAsOnDate As System.Windows.Forms.CheckBox
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents cmbMetal As BrighttechPack.CheckedComboBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkWithApproval As System.Windows.Forms.CheckBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
End Class
