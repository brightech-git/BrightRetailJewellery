<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPurchaseVATReportPer
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
        Me.ChkGrndtot = New System.Windows.Forms.CheckBox
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.chkED = New System.Windows.Forms.CheckBox
        Me.GrpContainer = New System.Windows.Forms.GroupBox
        Me.chkInclVat = New System.Windows.Forms.CheckBox
        Me.Label = New System.Windows.Forms.Label
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.chkCmbCompany = New BrighttechPack.CheckedComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.rbtTranDate = New System.Windows.Forms.RadioButton
        Me.rbtTranNo = New System.Windows.Forms.RadioButton
        Me.rbtMonth = New System.Windows.Forms.RadioButton
        Me.rbtCategoryWise = New System.Windows.Forms.RadioButton
        Me.cmbMetal = New System.Windows.Forms.ComboBox
        Me.chkOutstaion = New System.Windows.Forms.CheckBox
        Me.chkLocal = New System.Windows.Forms.CheckBox
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblDateFrom = New System.Windows.Forms.Label
        Me.lblDateTo = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Label5 = New System.Windows.Forms.Label
        Me.chkcmbTranType = New BrighttechPack.CheckedComboBox
        Me.GrpContainer.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ChkGrndtot
        '
        Me.ChkGrndtot.AutoSize = True
        Me.ChkGrndtot.Checked = True
        Me.ChkGrndtot.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkGrndtot.Location = New System.Drawing.Point(114, 236)
        Me.ChkGrndtot.Name = "ChkGrndtot"
        Me.ChkGrndtot.Size = New System.Drawing.Size(82, 17)
        Me.ChkGrndtot.TabIndex = 27
        Me.ChkGrndtot.Text = "Grand Total"
        Me.ChkGrndtot.UseVisualStyleBackColor = True
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "&Exit"
        '
        'chkED
        '
        Me.chkED.AutoSize = True
        Me.chkED.Location = New System.Drawing.Point(202, 236)
        Me.chkED.Name = "chkED"
        Me.chkED.Size = New System.Drawing.Size(66, 17)
        Me.chkED.TabIndex = 26
        Me.chkED.Text = "With ED"
        Me.chkED.UseVisualStyleBackColor = True
        Me.chkED.Visible = False
        '
        'GrpContainer
        '
        Me.GrpContainer.Controls.Add(Me.ChkGrndtot)
        Me.GrpContainer.Controls.Add(Me.chkED)
        Me.GrpContainer.Controls.Add(Me.chkInclVat)
        Me.GrpContainer.Controls.Add(Me.Label)
        Me.GrpContainer.Controls.Add(Me.chkCmbCostCentre)
        Me.GrpContainer.Controls.Add(Me.chkCmbCompany)
        Me.GrpContainer.Controls.Add(Me.Label4)
        Me.GrpContainer.Controls.Add(Me.Label2)
        Me.GrpContainer.Controls.Add(Me.rbtTranDate)
        Me.GrpContainer.Controls.Add(Me.rbtTranNo)
        Me.GrpContainer.Controls.Add(Me.rbtMonth)
        Me.GrpContainer.Controls.Add(Me.rbtCategoryWise)
        Me.GrpContainer.Controls.Add(Me.cmbMetal)
        Me.GrpContainer.Controls.Add(Me.chkOutstaion)
        Me.GrpContainer.Controls.Add(Me.chkLocal)
        Me.GrpContainer.Controls.Add(Me.dtpTo)
        Me.GrpContainer.Controls.Add(Me.dtpFrom)
        Me.GrpContainer.Controls.Add(Me.Label1)
        Me.GrpContainer.Controls.Add(Me.lblDateFrom)
        Me.GrpContainer.Controls.Add(Me.lblDateTo)
        Me.GrpContainer.Controls.Add(Me.btnExit)
        Me.GrpContainer.Controls.Add(Me.btnSearch)
        Me.GrpContainer.Controls.Add(Me.btnNew)
        Me.GrpContainer.Location = New System.Drawing.Point(199, 159)
        Me.GrpContainer.Name = "GrpContainer"
        Me.GrpContainer.Size = New System.Drawing.Size(438, 318)
        Me.GrpContainer.TabIndex = 10
        Me.GrpContainer.TabStop = False
        '
        'chkInclVat
        '
        Me.chkInclVat.AutoSize = True
        Me.chkInclVat.Checked = True
        Me.chkInclVat.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkInclVat.Location = New System.Drawing.Point(264, 172)
        Me.chkInclVat.Name = "chkInclVat"
        Me.chkInclVat.Size = New System.Drawing.Size(137, 17)
        Me.chkInclVat.TabIndex = 25
        Me.chkInclVat.Text = "Include Vat (Outstation)"
        Me.chkInclVat.UseVisualStyleBackColor = True
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(15, 139)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(62, 13)
        Me.Label.TabIndex = 10
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
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(114, 134)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(290, 21)
        Me.chkCmbCostCentre.TabIndex = 11
        Me.chkCmbCostCentre.ValueSeparator = ","
        '
        'chkCmbCompany
        '
        Me.chkCmbCompany.CheckOnClick = True
        Me.chkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCompany.DropDownHeight = 1
        Me.chkCmbCompany.FormattingEnabled = True
        Me.chkCmbCompany.IntegralHeight = False
        Me.chkCmbCompany.Location = New System.Drawing.Point(114, 94)
        Me.chkCmbCompany.Name = "chkCmbCompany"
        Me.chkCmbCompany.Size = New System.Drawing.Size(290, 21)
        Me.chkCmbCompany.TabIndex = 7
        Me.chkCmbCompany.ValueSeparator = ", "
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(15, 103)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(51, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Company"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 201)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(51, 13)
        Me.Label2.TabIndex = 17
        Me.Label2.Text = "Group By"
        '
        'rbtTranDate
        '
        Me.rbtTranDate.AutoSize = True
        Me.rbtTranDate.Location = New System.Drawing.Point(343, 201)
        Me.rbtTranDate.Name = "rbtTranDate"
        Me.rbtTranDate.Size = New System.Drawing.Size(70, 17)
        Me.rbtTranDate.TabIndex = 21
        Me.rbtTranDate.Text = "TranDate"
        Me.rbtTranDate.UseVisualStyleBackColor = True
        '
        'rbtTranNo
        '
        Me.rbtTranNo.AutoSize = True
        Me.rbtTranNo.Checked = True
        Me.rbtTranNo.Location = New System.Drawing.Point(110, 201)
        Me.rbtTranNo.Name = "rbtTranNo"
        Me.rbtTranNo.Size = New System.Drawing.Size(61, 17)
        Me.rbtTranNo.TabIndex = 18
        Me.rbtTranNo.TabStop = True
        Me.rbtTranNo.Text = "TranNo"
        Me.rbtTranNo.UseVisualStyleBackColor = True
        '
        'rbtMonth
        '
        Me.rbtMonth.AutoSize = True
        Me.rbtMonth.Location = New System.Drawing.Point(278, 201)
        Me.rbtMonth.Name = "rbtMonth"
        Me.rbtMonth.Size = New System.Drawing.Size(55, 17)
        Me.rbtMonth.TabIndex = 20
        Me.rbtMonth.Text = "Month"
        Me.rbtMonth.UseVisualStyleBackColor = True
        '
        'rbtCategoryWise
        '
        Me.rbtCategoryWise.AutoSize = True
        Me.rbtCategoryWise.Location = New System.Drawing.Point(194, 201)
        Me.rbtCategoryWise.Name = "rbtCategoryWise"
        Me.rbtCategoryWise.Size = New System.Drawing.Size(67, 17)
        Me.rbtCategoryWise.TabIndex = 19
        Me.rbtCategoryWise.Text = "Category"
        Me.rbtCategoryWise.UseVisualStyleBackColor = True
        '
        'cmbMetal
        '
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(114, 57)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(215, 21)
        Me.cmbMetal.TabIndex = 5
        '
        'chkOutstaion
        '
        Me.chkOutstaion.AutoSize = True
        Me.chkOutstaion.Checked = True
        Me.chkOutstaion.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkOutstaion.Location = New System.Drawing.Point(174, 172)
        Me.chkOutstaion.Name = "chkOutstaion"
        Me.chkOutstaion.Size = New System.Drawing.Size(74, 17)
        Me.chkOutstaion.TabIndex = 16
        Me.chkOutstaion.Text = "Outstation"
        Me.chkOutstaion.UseVisualStyleBackColor = True
        '
        'chkLocal
        '
        Me.chkLocal.AutoSize = True
        Me.chkLocal.Checked = True
        Me.chkLocal.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkLocal.Location = New System.Drawing.Point(110, 172)
        Me.chkLocal.Name = "chkLocal"
        Me.chkLocal.Size = New System.Drawing.Size(52, 17)
        Me.chkLocal.TabIndex = 15
        Me.chkLocal.Text = "Local"
        Me.chkLocal.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(238, 20)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 20)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(114, 20)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(93, 20)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 60)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(33, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Metal"
        '
        'lblDateFrom
        '
        Me.lblDateFrom.AutoSize = True
        Me.lblDateFrom.Location = New System.Drawing.Point(15, 24)
        Me.lblDateFrom.Name = "lblDateFrom"
        Me.lblDateFrom.Size = New System.Drawing.Size(53, 13)
        Me.lblDateFrom.TabIndex = 0
        Me.lblDateFrom.Text = "DateFrom"
        '
        'lblDateTo
        '
        Me.lblDateTo.AutoSize = True
        Me.lblDateTo.Location = New System.Drawing.Point(212, 24)
        Me.lblDateTo.Name = "lblDateTo"
        Me.lblDateTo.Size = New System.Drawing.Size(20, 13)
        Me.lblDateTo.TabIndex = 2
        Me.lblDateTo.Text = "To"
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(320, 273)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 24
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(108, 273)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 22
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(214, 273)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 23
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.NewToolStripMenuItem.Text = "&New"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 48)
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(580, 380)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(49, 13)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Trantype"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label5.Visible = False
        '
        'chkcmbTranType
        '
        Me.chkcmbTranType.CheckOnClick = True
        Me.chkcmbTranType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbTranType.DropDownHeight = 1
        Me.chkcmbTranType.FormattingEnabled = True
        Me.chkcmbTranType.IntegralHeight = False
        Me.chkcmbTranType.Location = New System.Drawing.Point(583, 355)
        Me.chkcmbTranType.Name = "chkcmbTranType"
        Me.chkcmbTranType.Size = New System.Drawing.Size(290, 21)
        Me.chkcmbTranType.TabIndex = 12
        Me.chkcmbTranType.ValueSeparator = ", "
        '
        'frmPurchaseVATReportPer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1030, 756)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.GrpContainer)
        Me.Controls.Add(Me.chkcmbTranType)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmPurchaseVATReportPer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Purchase VAT Report New"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GrpContainer.ResumeLayout(False)
        Me.GrpContainer.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ChkGrndtot As System.Windows.Forms.CheckBox
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkED As System.Windows.Forms.CheckBox
    Friend WithEvents GrpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents chkInclVat As System.Windows.Forms.CheckBox
    Friend WithEvents Label As System.Windows.Forms.Label
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents rbtTranDate As System.Windows.Forms.RadioButton
    Friend WithEvents rbtTranNo As System.Windows.Forms.RadioButton
    Friend WithEvents rbtMonth As System.Windows.Forms.RadioButton
    Friend WithEvents rbtCategoryWise As System.Windows.Forms.RadioButton
    Friend WithEvents cmbMetal As System.Windows.Forms.ComboBox
    Friend WithEvents chkOutstaion As System.Windows.Forms.CheckBox
    Friend WithEvents chkLocal As System.Windows.Forms.CheckBox
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblDateFrom As System.Windows.Forms.Label
    Friend WithEvents lblDateTo As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents chkcmbTranType As BrighttechPack.CheckedComboBox
End Class
