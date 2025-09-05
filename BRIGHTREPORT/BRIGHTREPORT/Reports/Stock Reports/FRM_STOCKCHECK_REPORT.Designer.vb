<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FRM_STOCKCHECK_REPORT
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
        Me.GrpContainer = New System.Windows.Forms.GroupBox()
        Me.txtItemCode_NUM = New System.Windows.Forms.TextBox()
        Me.chkWithDiffwt = New System.Windows.Forms.CheckBox()
        Me.btngenerate = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.RbtBoth = New System.Windows.Forms.RadioButton()
        Me.RbtUnchecked = New System.Windows.Forms.RadioButton()
        Me.RbtChecked = New System.Windows.Forms.RadioButton()
        Me.ChkSubItem = New System.Windows.Forms.CheckBox()
        Me.chkItem = New System.Windows.Forms.CheckBox()
        Me.rbtSummary = New System.Windows.Forms.RadioButton()
        Me.rbtDetailed = New System.Windows.Forms.RadioButton()
        Me.chkCmbCounter = New BrighttechPack.CheckedComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkCmbCompany = New BrighttechPack.CheckedComboBox()
        Me.chkCmbItemName = New BrighttechPack.CheckedComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.chkCmbDesigner = New BrighttechPack.CheckedComboBox()
        Me.Label = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.GrpContainer.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GrpContainer
        '
        Me.GrpContainer.Controls.Add(Me.txtItemCode_NUM)
        Me.GrpContainer.Controls.Add(Me.chkWithDiffwt)
        Me.GrpContainer.Controls.Add(Me.btngenerate)
        Me.GrpContainer.Controls.Add(Me.Panel1)
        Me.GrpContainer.Controls.Add(Me.ChkSubItem)
        Me.GrpContainer.Controls.Add(Me.chkItem)
        Me.GrpContainer.Controls.Add(Me.rbtSummary)
        Me.GrpContainer.Controls.Add(Me.rbtDetailed)
        Me.GrpContainer.Controls.Add(Me.chkCmbCounter)
        Me.GrpContainer.Controls.Add(Me.Label2)
        Me.GrpContainer.Controls.Add(Me.Label1)
        Me.GrpContainer.Controls.Add(Me.chkCmbCompany)
        Me.GrpContainer.Controls.Add(Me.chkCmbItemName)
        Me.GrpContainer.Controls.Add(Me.Label9)
        Me.GrpContainer.Controls.Add(Me.chkCmbDesigner)
        Me.GrpContainer.Controls.Add(Me.Label)
        Me.GrpContainer.Controls.Add(Me.Label3)
        Me.GrpContainer.Controls.Add(Me.Label6)
        Me.GrpContainer.Controls.Add(Me.chkCmbCostCentre)
        Me.GrpContainer.Controls.Add(Me.btnExit)
        Me.GrpContainer.Controls.Add(Me.btnSearch)
        Me.GrpContainer.Controls.Add(Me.btnNew)
        Me.GrpContainer.Location = New System.Drawing.Point(196, 102)
        Me.GrpContainer.Name = "GrpContainer"
        Me.GrpContainer.Size = New System.Drawing.Size(420, 308)
        Me.GrpContainer.TabIndex = 0
        Me.GrpContainer.TabStop = False
        '
        'txtItemCode_NUM
        '
        Me.txtItemCode_NUM.Location = New System.Drawing.Point(121, 83)
        Me.txtItemCode_NUM.Name = "txtItemCode_NUM"
        Me.txtItemCode_NUM.Size = New System.Drawing.Size(53, 21)
        Me.txtItemCode_NUM.TabIndex = 5
        '
        'chkWithDiffwt
        '
        Me.chkWithDiffwt.AutoSize = True
        Me.chkWithDiffwt.Enabled = False
        Me.chkWithDiffwt.Location = New System.Drawing.Point(290, 211)
        Me.chkWithDiffwt.Name = "chkWithDiffwt"
        Me.chkWithDiffwt.Size = New System.Drawing.Size(88, 17)
        Me.chkWithDiffwt.TabIndex = 14
        Me.chkWithDiffwt.Text = "With Diffwt"
        Me.chkWithDiffwt.UseVisualStyleBackColor = True
        '
        'btngenerate
        '
        Me.btngenerate.Location = New System.Drawing.Point(208, 267)
        Me.btngenerate.Name = "btngenerate"
        Me.btngenerate.Size = New System.Drawing.Size(100, 30)
        Me.btngenerate.TabIndex = 20
        Me.btngenerate.Text = "Generate"
        Me.btngenerate.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.RbtBoth)
        Me.Panel1.Controls.Add(Me.RbtUnchecked)
        Me.Panel1.Controls.Add(Me.RbtChecked)
        Me.Panel1.Location = New System.Drawing.Point(112, 173)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(270, 34)
        Me.Panel1.TabIndex = 11
        '
        'RbtBoth
        '
        Me.RbtBoth.AutoSize = True
        Me.RbtBoth.Checked = True
        Me.RbtBoth.Location = New System.Drawing.Point(10, 6)
        Me.RbtBoth.Name = "RbtBoth"
        Me.RbtBoth.Size = New System.Drawing.Size(51, 17)
        Me.RbtBoth.TabIndex = 0
        Me.RbtBoth.TabStop = True
        Me.RbtBoth.Text = "Both"
        Me.RbtBoth.UseVisualStyleBackColor = True
        '
        'RbtUnchecked
        '
        Me.RbtUnchecked.AutoSize = True
        Me.RbtUnchecked.Location = New System.Drawing.Point(178, 6)
        Me.RbtUnchecked.Name = "RbtUnchecked"
        Me.RbtUnchecked.Size = New System.Drawing.Size(90, 17)
        Me.RbtUnchecked.TabIndex = 2
        Me.RbtUnchecked.Text = "UnChecked"
        Me.RbtUnchecked.UseVisualStyleBackColor = True
        '
        'RbtChecked
        '
        Me.RbtChecked.AutoSize = True
        Me.RbtChecked.Location = New System.Drawing.Point(97, 6)
        Me.RbtChecked.Name = "RbtChecked"
        Me.RbtChecked.Size = New System.Drawing.Size(75, 17)
        Me.RbtChecked.TabIndex = 1
        Me.RbtChecked.Text = "Checked"
        Me.RbtChecked.UseVisualStyleBackColor = True
        '
        'ChkSubItem
        '
        Me.ChkSubItem.AutoSize = True
        Me.ChkSubItem.Checked = True
        Me.ChkSubItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkSubItem.Location = New System.Drawing.Point(178, 241)
        Me.ChkSubItem.Name = "ChkSubItem"
        Me.ChkSubItem.Size = New System.Drawing.Size(75, 17)
        Me.ChkSubItem.TabIndex = 17
        Me.ChkSubItem.Text = "SubItem"
        Me.ChkSubItem.UseVisualStyleBackColor = True
        '
        'chkItem
        '
        Me.chkItem.AutoSize = True
        Me.chkItem.Checked = True
        Me.chkItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkItem.Location = New System.Drawing.Point(121, 241)
        Me.chkItem.Name = "chkItem"
        Me.chkItem.Size = New System.Drawing.Size(53, 17)
        Me.chkItem.TabIndex = 16
        Me.chkItem.Text = "Item"
        Me.chkItem.UseVisualStyleBackColor = True
        '
        'rbtSummary
        '
        Me.rbtSummary.AutoSize = True
        Me.rbtSummary.Checked = True
        Me.rbtSummary.Location = New System.Drawing.Point(121, 211)
        Me.rbtSummary.Name = "rbtSummary"
        Me.rbtSummary.Size = New System.Drawing.Size(81, 17)
        Me.rbtSummary.TabIndex = 12
        Me.rbtSummary.TabStop = True
        Me.rbtSummary.Text = "Summary"
        Me.rbtSummary.UseVisualStyleBackColor = True
        '
        'rbtDetailed
        '
        Me.rbtDetailed.AutoSize = True
        Me.rbtDetailed.Location = New System.Drawing.Point(208, 211)
        Me.rbtDetailed.Name = "rbtDetailed"
        Me.rbtDetailed.Size = New System.Drawing.Size(72, 17)
        Me.rbtDetailed.TabIndex = 13
        Me.rbtDetailed.Text = "Detailed"
        Me.rbtDetailed.UseVisualStyleBackColor = True
        '
        'chkCmbCounter
        '
        Me.chkCmbCounter.CheckOnClick = True
        Me.chkCmbCounter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCounter.DropDownHeight = 1
        Me.chkCmbCounter.FormattingEnabled = True
        Me.chkCmbCounter.IntegralHeight = False
        Me.chkCmbCounter.Location = New System.Drawing.Point(121, 145)
        Me.chkCmbCounter.Name = "chkCmbCounter"
        Me.chkCmbCounter.Size = New System.Drawing.Size(234, 22)
        Me.chkCmbCounter.TabIndex = 10
        Me.chkCmbCounter.ValueSeparator = ", "
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(17, 242)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(61, 13)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "Group By"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(17, 150)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Counter"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbCompany
        '
        Me.chkCmbCompany.CheckOnClick = True
        Me.chkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCompany.DropDownHeight = 1
        Me.chkCmbCompany.FormattingEnabled = True
        Me.chkCmbCompany.IntegralHeight = False
        Me.chkCmbCompany.Location = New System.Drawing.Point(121, 19)
        Me.chkCmbCompany.Name = "chkCmbCompany"
        Me.chkCmbCompany.Size = New System.Drawing.Size(234, 22)
        Me.chkCmbCompany.TabIndex = 1
        Me.chkCmbCompany.ValueSeparator = ", "
        '
        'chkCmbItemName
        '
        Me.chkCmbItemName.CheckOnClick = True
        Me.chkCmbItemName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbItemName.DropDownHeight = 1
        Me.chkCmbItemName.FormattingEnabled = True
        Me.chkCmbItemName.IntegralHeight = False
        Me.chkCmbItemName.Location = New System.Drawing.Point(175, 82)
        Me.chkCmbItemName.Name = "chkCmbItemName"
        Me.chkCmbItemName.Size = New System.Drawing.Size(180, 22)
        Me.chkCmbItemName.TabIndex = 6
        Me.chkCmbItemName.ValueSeparator = ", "
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(17, 28)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(62, 13)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Company"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbDesigner
        '
        Me.chkCmbDesigner.CheckOnClick = True
        Me.chkCmbDesigner.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbDesigner.DropDownHeight = 1
        Me.chkCmbDesigner.FormattingEnabled = True
        Me.chkCmbDesigner.IntegralHeight = False
        Me.chkCmbDesigner.Location = New System.Drawing.Point(121, 113)
        Me.chkCmbDesigner.Name = "chkCmbDesigner"
        Me.chkCmbDesigner.Size = New System.Drawing.Size(234, 22)
        Me.chkCmbDesigner.TabIndex = 8
        Me.chkCmbDesigner.ValueSeparator = ", "
        '
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(17, 56)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(76, 13)
        Me.Label.TabIndex = 2
        Me.Label.Text = "Cost Centre"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(17, 87)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(88, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Item Id\Name"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(17, 118)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(58, 13)
        Me.Label6.TabIndex = 7
        Me.Label6.Text = "Designer"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(121, 51)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(234, 22)
        Me.chkCmbCostCentre.TabIndex = 3
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(310, 267)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 21
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(5, 267)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 18
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(107, 267)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 19
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'FRM_STOCKCHECK_REPORT
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(778, 484)
        Me.Controls.Add(Me.GrpContainer)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "FRM_STOCKCHECK_REPORT"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FRM_STOCKCHECK_REPORT"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.GrpContainer.ResumeLayout(False)
        Me.GrpContainer.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GrpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents chkCmbItemName As BrighttechPack.CheckedComboBox
    Friend WithEvents chkCmbDesigner As BrighttechPack.CheckedComboBox
    Friend WithEvents Label As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents chkCmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents chkCmbCounter As BrighttechPack.CheckedComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents rbtSummary As System.Windows.Forms.RadioButton
    Friend WithEvents rbtDetailed As System.Windows.Forms.RadioButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkItem As System.Windows.Forms.CheckBox
    Friend WithEvents ChkSubItem As System.Windows.Forms.CheckBox
    Friend WithEvents RbtBoth As System.Windows.Forms.RadioButton
    Friend WithEvents RbtChecked As System.Windows.Forms.RadioButton
    Friend WithEvents RbtUnchecked As System.Windows.Forms.RadioButton
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btngenerate As System.Windows.Forms.Button
    Friend WithEvents chkWithDiffwt As System.Windows.Forms.CheckBox
    Friend WithEvents txtItemCode_NUM As TextBox
End Class
