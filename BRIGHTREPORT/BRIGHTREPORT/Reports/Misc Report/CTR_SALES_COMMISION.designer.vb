<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CTR_SALES_COMMISION
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
        Me.Label9 = New System.Windows.Forms.Label
        Me.chkcmbCounter = New BrighttechPack.CheckedComboBox
        Me.pnlrpttype = New System.Windows.Forms.Panel
        Me.rbtDetailed = New System.Windows.Forms.RadioButton
        Me.rbtSummary = New System.Windows.Forms.RadioButton
        Me.chkCmbCompany = New BrighttechPack.CheckedComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.chkcmbsubitem = New BrighttechPack.CheckedComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Metal = New System.Windows.Forms.Label
        Me.chkcmbmetal = New BrighttechPack.CheckedComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.chkcmbitemname = New BrighttechPack.CheckedComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.chkcmbemployee = New BrighttechPack.CheckedComboBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label = New System.Windows.Forms.Label
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.GrpContainer.SuspendLayout()
        Me.pnlrpttype.SuspendLayout()
        Me.SuspendLayout()
        '
        'GrpContainer
        '
        Me.GrpContainer.Controls.Add(Me.Label9)
        Me.GrpContainer.Controls.Add(Me.chkcmbCounter)
        Me.GrpContainer.Controls.Add(Me.pnlrpttype)
        Me.GrpContainer.Controls.Add(Me.chkCmbCompany)
        Me.GrpContainer.Controls.Add(Me.Label8)
        Me.GrpContainer.Controls.Add(Me.Label7)
        Me.GrpContainer.Controls.Add(Me.chkcmbsubitem)
        Me.GrpContainer.Controls.Add(Me.Label6)
        Me.GrpContainer.Controls.Add(Me.Metal)
        Me.GrpContainer.Controls.Add(Me.chkcmbmetal)
        Me.GrpContainer.Controls.Add(Me.Label5)
        Me.GrpContainer.Controls.Add(Me.chkcmbitemname)
        Me.GrpContainer.Controls.Add(Me.Label4)
        Me.GrpContainer.Controls.Add(Me.chkcmbemployee)
        Me.GrpContainer.Controls.Add(Me.btnExit)
        Me.GrpContainer.Controls.Add(Me.btnSearch)
        Me.GrpContainer.Controls.Add(Me.btnNew)
        Me.GrpContainer.Controls.Add(Me.Label2)
        Me.GrpContainer.Controls.Add(Me.Label)
        Me.GrpContainer.Controls.Add(Me.chkCmbCostCentre)
        Me.GrpContainer.Controls.Add(Me.Label1)
        Me.GrpContainer.Controls.Add(Me.dtpTo)
        Me.GrpContainer.Controls.Add(Me.dtpFrom)
        Me.GrpContainer.Location = New System.Drawing.Point(188, 23)
        Me.GrpContainer.Name = "GrpContainer"
        Me.GrpContainer.Size = New System.Drawing.Size(504, 485)
        Me.GrpContainer.TabIndex = 0
        Me.GrpContainer.TabStop = False
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(84, 217)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(84, 13)
        Me.Label9.TabIndex = 10
        Me.Label9.Text = "Item Counter"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkcmbCounter
        '
        Me.chkcmbCounter.CheckOnClick = True
        Me.chkcmbCounter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkcmbCounter.DropDownHeight = 1
        Me.chkcmbCounter.FormattingEnabled = True
        Me.chkcmbCounter.IntegralHeight = False
        Me.chkcmbCounter.Location = New System.Drawing.Point(186, 212)
        Me.chkcmbCounter.Name = "chkcmbCounter"
        Me.chkcmbCounter.Size = New System.Drawing.Size(234, 22)
        Me.chkcmbCounter.TabIndex = 11
        Me.chkcmbCounter.ValueSeparator = ", "
        '
        'pnlrpttype
        '
        Me.pnlrpttype.Controls.Add(Me.rbtDetailed)
        Me.pnlrpttype.Controls.Add(Me.rbtSummary)
        Me.pnlrpttype.Location = New System.Drawing.Point(186, 326)
        Me.pnlrpttype.Name = "pnlrpttype"
        Me.pnlrpttype.Size = New System.Drawing.Size(234, 23)
        Me.pnlrpttype.TabIndex = 21
        '
        'rbtDetailed
        '
        Me.rbtDetailed.AutoSize = True
        Me.rbtDetailed.Checked = True
        Me.rbtDetailed.Location = New System.Drawing.Point(126, 1)
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
        'chkCmbCompany
        '
        Me.chkCmbCompany.CheckOnClick = True
        Me.chkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCompany.DropDownHeight = 1
        Me.chkCmbCompany.FormattingEnabled = True
        Me.chkCmbCompany.IntegralHeight = False
        Me.chkCmbCompany.Location = New System.Drawing.Point(186, 125)
        Me.chkCmbCompany.Name = "chkCmbCompany"
        Me.chkCmbCompany.Size = New System.Drawing.Size(234, 22)
        Me.chkCmbCompany.TabIndex = 5
        Me.chkCmbCompany.ValueSeparator = ", "
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(84, 130)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(62, 13)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "Company"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(84, 275)
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
        Me.chkcmbsubitem.Location = New System.Drawing.Point(186, 270)
        Me.chkcmbsubitem.Name = "chkcmbsubitem"
        Me.chkcmbsubitem.Size = New System.Drawing.Size(234, 22)
        Me.chkcmbsubitem.TabIndex = 15
        Me.chkcmbsubitem.ValueSeparator = ", "
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(84, 326)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(77, 13)
        Me.Label6.TabIndex = 20
        Me.Label6.Text = "Report Type"
        '
        'Metal
        '
        Me.Metal.AutoSize = True
        Me.Metal.Location = New System.Drawing.Point(84, 188)
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
        Me.chkcmbmetal.Location = New System.Drawing.Point(186, 183)
        Me.chkcmbmetal.Name = "chkcmbmetal"
        Me.chkcmbmetal.Size = New System.Drawing.Size(234, 22)
        Me.chkcmbmetal.TabIndex = 9
        Me.chkcmbmetal.ValueSeparator = ", "
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(84, 246)
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
        Me.chkcmbitemname.Location = New System.Drawing.Point(186, 241)
        Me.chkcmbitemname.Name = "chkcmbitemname"
        Me.chkcmbitemname.Size = New System.Drawing.Size(234, 22)
        Me.chkcmbitemname.TabIndex = 13
        Me.chkcmbitemname.ValueSeparator = ", "
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(84, 304)
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
        Me.chkcmbemployee.Location = New System.Drawing.Point(186, 299)
        Me.chkcmbemployee.Name = "chkcmbemployee"
        Me.chkcmbemployee.Size = New System.Drawing.Size(234, 22)
        Me.chkcmbemployee.TabIndex = 17
        Me.chkcmbemployee.ValueSeparator = ", "
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(320, 358)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 32
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(110, 358)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 30
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(215, 358)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 31
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(84, 101)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(67, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Date From"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(84, 159)
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
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(186, 154)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(234, 22)
        Me.chkCmbCostCentre.TabIndex = 7
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(275, 100)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(21, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "To"
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(302, 97)
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
        Me.dtpFrom.Location = New System.Drawing.Point(186, 97)
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
        'CTR_SALES_COMMISION
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(881, 530)
        Me.ControlBox = False
        Me.Controls.Add(Me.GrpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "CTR_SALES_COMMISION"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "RPT_SALES_COMMISION"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GrpContainer.ResumeLayout(False)
        Me.GrpContainer.PerformLayout()
        Me.pnlrpttype.ResumeLayout(False)
        Me.pnlrpttype.PerformLayout()
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
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents chkcmbitemname As BrighttechPack.CheckedComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents chkcmbemployee As BrighttechPack.CheckedComboBox
    Friend WithEvents Metal As System.Windows.Forms.Label
    Friend WithEvents chkcmbmetal As BrighttechPack.CheckedComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents chkcmbsubitem As BrighttechPack.CheckedComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents chkCmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents pnlrpttype As System.Windows.Forms.Panel
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents chkcmbCounter As BrighttechPack.CheckedComboBox
End Class
