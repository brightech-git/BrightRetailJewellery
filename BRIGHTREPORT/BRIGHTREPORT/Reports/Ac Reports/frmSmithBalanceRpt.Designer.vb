<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSmithBalanceRpt
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
        Me.pnlContainer = New System.Windows.Forms.Panel
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.chkCostCentreSelectAll = New System.Windows.Forms.CheckBox
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox
        Me.chkMetalSelectAll = New System.Windows.Forms.CheckBox
        Me.chkLstMetal = New System.Windows.Forms.CheckedListBox
        Me.chkGrstWt = New System.Windows.Forms.CheckBox
        Me.chknetwt = New System.Windows.Forms.CheckBox
        Me.chkpurwt = New System.Windows.Forms.CheckBox
        Me.chkCompanySelectAll = New System.Windows.Forms.CheckBox
        Me.chkLstCompany = New System.Windows.Forms.CheckedListBox
        Me.pnlContainer.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlContainer
        '
        Me.pnlContainer.Controls.Add(Me.chkCompanySelectAll)
        Me.pnlContainer.Controls.Add(Me.chkpurwt)
        Me.pnlContainer.Controls.Add(Me.chknetwt)
        Me.pnlContainer.Controls.Add(Me.chkLstCompany)
        Me.pnlContainer.Controls.Add(Me.chkGrstWt)
        Me.pnlContainer.Controls.Add(Me.dtpTo)
        Me.pnlContainer.Controls.Add(Me.dtpFrom)
        Me.pnlContainer.Controls.Add(Me.Label4)
        Me.pnlContainer.Controls.Add(Me.Label5)
        Me.pnlContainer.Controls.Add(Me.Label1)
        Me.pnlContainer.Controls.Add(Me.btnExit)
        Me.pnlContainer.Controls.Add(Me.btnSearch)
        Me.pnlContainer.Controls.Add(Me.btnNew)
        Me.pnlContainer.Controls.Add(Me.chkCostCentreSelectAll)
        Me.pnlContainer.Controls.Add(Me.chkLstCostCentre)
        Me.pnlContainer.Controls.Add(Me.chkMetalSelectAll)
        Me.pnlContainer.Controls.Add(Me.chkLstMetal)
        Me.pnlContainer.Location = New System.Drawing.Point(175, 57)
        Me.pnlContainer.Name = "pnlContainer"
        Me.pnlContainer.Size = New System.Drawing.Size(437, 389)
        Me.pnlContainer.TabIndex = 1
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(221, 46)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 20)
        Me.dtpTo.TabIndex = 5
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(87, 46)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(93, 20)
        Me.dtpFrom.TabIndex = 3
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(191, 49)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(20, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "To"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(19, 310)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(54, 13)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Based On"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(17, 49)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Date From"
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(232, 346)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 18
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(20, 346)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 16
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(126, 346)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 17
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'chkCostCentreSelectAll
        '
        Me.chkCostCentreSelectAll.AutoSize = True
        Me.chkCostCentreSelectAll.Location = New System.Drawing.Point(22, 186)
        Me.chkCostCentreSelectAll.Name = "chkCostCentreSelectAll"
        Me.chkCostCentreSelectAll.Size = New System.Drawing.Size(81, 17)
        Me.chkCostCentreSelectAll.TabIndex = 8
        Me.chkCostCentreSelectAll.Text = "Cost Centre"
        Me.chkCostCentreSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(19, 205)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(201, 64)
        Me.chkLstCostCentre.TabIndex = 9
        '
        'chkMetalSelectAll
        '
        Me.chkMetalSelectAll.AutoSize = True
        Me.chkMetalSelectAll.Location = New System.Drawing.Point(230, 186)
        Me.chkMetalSelectAll.Name = "chkMetalSelectAll"
        Me.chkMetalSelectAll.Size = New System.Drawing.Size(52, 17)
        Me.chkMetalSelectAll.TabIndex = 10
        Me.chkMetalSelectAll.Text = "Metal"
        Me.chkMetalSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstMetal
        '
        Me.chkLstMetal.FormattingEnabled = True
        Me.chkLstMetal.Location = New System.Drawing.Point(227, 205)
        Me.chkLstMetal.Name = "chkLstMetal"
        Me.chkLstMetal.Size = New System.Drawing.Size(199, 64)
        Me.chkLstMetal.TabIndex = 11
        '
        'chkGrstWt
        '
        Me.chkGrstWt.AutoSize = True
        Me.chkGrstWt.Checked = True
        Me.chkGrstWt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkGrstWt.Location = New System.Drawing.Point(99, 309)
        Me.chkGrstWt.Name = "chkGrstWt"
        Me.chkGrstWt.Size = New System.Drawing.Size(87, 17)
        Me.chkGrstWt.TabIndex = 13
        Me.chkGrstWt.Text = "GrossWeight"
        Me.chkGrstWt.UseVisualStyleBackColor = True
        '
        'chknetwt
        '
        Me.chknetwt.AutoSize = True
        Me.chknetwt.Location = New System.Drawing.Point(192, 309)
        Me.chknetwt.Name = "chknetwt"
        Me.chknetwt.Size = New System.Drawing.Size(80, 17)
        Me.chknetwt.TabIndex = 14
        Me.chknetwt.Text = "Net Weight"
        Me.chknetwt.UseVisualStyleBackColor = True
        '
        'chkpurwt
        '
        Me.chkpurwt.AutoSize = True
        Me.chkpurwt.Location = New System.Drawing.Point(278, 310)
        Me.chkpurwt.Name = "chkpurwt"
        Me.chkpurwt.Size = New System.Drawing.Size(85, 17)
        Me.chkpurwt.TabIndex = 15
        Me.chkpurwt.Text = "Pure Weight"
        Me.chkpurwt.UseVisualStyleBackColor = True
        '
        'chkCompanySelectAll
        '
        Me.chkCompanySelectAll.AutoSize = True
        Me.chkCompanySelectAll.Location = New System.Drawing.Point(25, 84)
        Me.chkCompanySelectAll.Name = "chkCompanySelectAll"
        Me.chkCompanySelectAll.Size = New System.Drawing.Size(70, 17)
        Me.chkCompanySelectAll.TabIndex = 6
        Me.chkCompanySelectAll.Text = "Company"
        Me.chkCompanySelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCompany
        '
        Me.chkLstCompany.FormattingEnabled = True
        Me.chkLstCompany.Location = New System.Drawing.Point(22, 105)
        Me.chkLstCompany.Name = "chkLstCompany"
        Me.chkLstCompany.Size = New System.Drawing.Size(292, 64)
        Me.chkLstCompany.TabIndex = 7
        '
        'frmSmithBalanceRpt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(787, 503)
        Me.Controls.Add(Me.pnlContainer)
        Me.KeyPreview = True
        Me.Name = "frmSmithBalanceRpt"
        Me.Text = "frmSmithBalanceRpt"
        Me.pnlContainer.ResumeLayout(False)
        Me.pnlContainer.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlContainer As System.Windows.Forms.Panel
    Friend WithEvents chkpurwt As System.Windows.Forms.CheckBox
    Friend WithEvents chknetwt As System.Windows.Forms.CheckBox
    Friend WithEvents chkGrstWt As System.Windows.Forms.CheckBox
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents chkCostCentreSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkMetalSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstMetal As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkCompanySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCompany As System.Windows.Forms.CheckedListBox
End Class
