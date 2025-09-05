<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCostCentreWiseSales
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
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.chkCostCentreSelectAll = New System.Windows.Forms.CheckBox
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox
        Me.grpContainer = New System.Windows.Forms.GroupBox
        Me.chkCompanySelectAll = New System.Windows.Forms.CheckBox
        Me.chkLstCompany = New System.Windows.Forms.CheckedListBox
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.chkUserWiseSelectAll = New System.Windows.Forms.CheckBox
        Me.chkLstUserWise = New System.Windows.Forms.CheckedListBox
        Me.rbtGeneral = New System.Windows.Forms.RadioButton
        Me.chkWeightDetail = New System.Windows.Forms.CheckBox
        Me.ChkCreditDebit = New System.Windows.Forms.CheckBox
        Me.ChkSummary = New System.Windows.Forms.CheckBox
        Me.grpMore = New System.Windows.Forms.GroupBox
        Me.chkRefDate = New System.Windows.Forms.CheckBox
        Me.chkChequeNo = New System.Windows.Forms.CheckBox
        Me.chkRefNo = New System.Windows.Forms.CheckBox
        Me.chkChequeDate = New System.Windows.Forms.CheckBox
        Me.chkMore = New System.Windows.Forms.CheckBox
        Me.chkDetailed = New System.Windows.Forms.CheckBox
        Me.rbtTranNo = New System.Windows.Forms.RadioButton
        Me.rbtAcName = New System.Windows.Forms.RadioButton
        Me.chkOpening = New System.Windows.Forms.CheckBox
        Me.chkPageBreak = New System.Windows.Forms.CheckBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.grpContainer.SuspendLayout()
        Me.grpMore.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(228, 16)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(99, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(70, 16)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(102, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(190, 19)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(21, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "To"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(0, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date From"
        '
        'chkCostCentreSelectAll
        '
        Me.chkCostCentreSelectAll.AutoSize = True
        Me.chkCostCentreSelectAll.Location = New System.Drawing.Point(6, 144)
        Me.chkCostCentreSelectAll.Name = "chkCostCentreSelectAll"
        Me.chkCostCentreSelectAll.Size = New System.Drawing.Size(95, 17)
        Me.chkCostCentreSelectAll.TabIndex = 6
        Me.chkCostCentreSelectAll.Text = "Cost Centre"
        Me.chkCostCentreSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(3, 163)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(322, 84)
        Me.chkLstCostCentre.TabIndex = 7
        '
        'grpContainer
        '
        Me.grpContainer.Controls.Add(Me.chkCompanySelectAll)
        Me.grpContainer.Controls.Add(Me.chkLstCompany)
        Me.grpContainer.Controls.Add(Me.btnExit)
        Me.grpContainer.Controls.Add(Me.btnSearch)
        Me.grpContainer.Controls.Add(Me.btnNew)
        Me.grpContainer.Controls.Add(Me.Label1)
        Me.grpContainer.Controls.Add(Me.dtpTo)
        Me.grpContainer.Controls.Add(Me.chkLstCostCentre)
        Me.grpContainer.Controls.Add(Me.dtpFrom)
        Me.grpContainer.Controls.Add(Me.chkCostCentreSelectAll)
        Me.grpContainer.Controls.Add(Me.Label3)
        Me.grpContainer.Location = New System.Drawing.Point(301, 72)
        Me.grpContainer.Name = "grpContainer"
        Me.grpContainer.Size = New System.Drawing.Size(332, 309)
        Me.grpContainer.TabIndex = 0
        Me.grpContainer.TabStop = False
        '
        'chkCompanySelectAll
        '
        Me.chkCompanySelectAll.AutoSize = True
        Me.chkCompanySelectAll.Location = New System.Drawing.Point(7, 39)
        Me.chkCompanySelectAll.Name = "chkCompanySelectAll"
        Me.chkCompanySelectAll.Size = New System.Drawing.Size(81, 17)
        Me.chkCompanySelectAll.TabIndex = 4
        Me.chkCompanySelectAll.Text = "Company"
        Me.chkCompanySelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCompany
        '
        Me.chkLstCompany.FormattingEnabled = True
        Me.chkLstCompany.Location = New System.Drawing.Point(4, 58)
        Me.chkLstCompany.Name = "chkLstCompany"
        Me.chkLstCompany.Size = New System.Drawing.Size(321, 84)
        Me.chkLstCompany.TabIndex = 5
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(215, 263)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 24
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(3, 263)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 22
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(109, 263)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 23
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'chkUserWiseSelectAll
        '
        Me.chkUserWiseSelectAll.AutoSize = True
        Me.chkUserWiseSelectAll.Location = New System.Drawing.Point(7, 215)
        Me.chkUserWiseSelectAll.Name = "chkUserWiseSelectAll"
        Me.chkUserWiseSelectAll.Size = New System.Drawing.Size(83, 17)
        Me.chkUserWiseSelectAll.TabIndex = 8
        Me.chkUserWiseSelectAll.Text = "User Wise"
        Me.chkUserWiseSelectAll.UseVisualStyleBackColor = True
        Me.chkUserWiseSelectAll.Visible = False
        '
        'chkLstUserWise
        '
        Me.chkLstUserWise.FormattingEnabled = True
        Me.chkLstUserWise.Location = New System.Drawing.Point(4, 235)
        Me.chkLstUserWise.Name = "chkLstUserWise"
        Me.chkLstUserWise.Size = New System.Drawing.Size(209, 84)
        Me.chkLstUserWise.TabIndex = 9
        Me.chkLstUserWise.Visible = False
        '
        'rbtGeneral
        '
        Me.rbtGeneral.AutoSize = True
        Me.rbtGeneral.Location = New System.Drawing.Point(210, 364)
        Me.rbtGeneral.Name = "rbtGeneral"
        Me.rbtGeneral.Size = New System.Drawing.Size(70, 17)
        Me.rbtGeneral.TabIndex = 16
        Me.rbtGeneral.Text = "General"
        Me.rbtGeneral.UseVisualStyleBackColor = True
        Me.rbtGeneral.Visible = False
        '
        'chkWeightDetail
        '
        Me.chkWeightDetail.AutoSize = True
        Me.chkWeightDetail.Location = New System.Drawing.Point(195, 448)
        Me.chkWeightDetail.Name = "chkWeightDetail"
        Me.chkWeightDetail.Size = New System.Drawing.Size(94, 17)
        Me.chkWeightDetail.TabIndex = 20
        Me.chkWeightDetail.Text = "With Weight"
        Me.chkWeightDetail.UseVisualStyleBackColor = True
        Me.chkWeightDetail.Visible = False
        '
        'ChkCreditDebit
        '
        Me.ChkCreditDebit.AutoSize = True
        Me.ChkCreditDebit.Location = New System.Drawing.Point(30, 481)
        Me.ChkCreditDebit.Name = "ChkCreditDebit"
        Me.ChkCreditDebit.Size = New System.Drawing.Size(111, 17)
        Me.ChkCreditDebit.TabIndex = 21
        Me.ChkCreditDebit.Text = "CREDIT,DEBIT"
        Me.ChkCreditDebit.UseVisualStyleBackColor = True
        Me.ChkCreditDebit.Visible = False
        '
        'ChkSummary
        '
        Me.ChkSummary.AutoSize = True
        Me.ChkSummary.Location = New System.Drawing.Point(9, 448)
        Me.ChkSummary.Name = "ChkSummary"
        Me.ChkSummary.Size = New System.Drawing.Size(164, 17)
        Me.ChkSummary.TabIndex = 19
        Me.ChkSummary.Text = "AcName Wise Summary"
        Me.ChkSummary.UseVisualStyleBackColor = True
        Me.ChkSummary.Visible = False
        '
        'grpMore
        '
        Me.grpMore.Controls.Add(Me.chkRefDate)
        Me.grpMore.Controls.Add(Me.chkChequeNo)
        Me.grpMore.Controls.Add(Me.chkRefNo)
        Me.grpMore.Controls.Add(Me.chkChequeDate)
        Me.grpMore.Location = New System.Drawing.Point(84, 380)
        Me.grpMore.Name = "grpMore"
        Me.grpMore.Size = New System.Drawing.Size(202, 64)
        Me.grpMore.TabIndex = 17
        Me.grpMore.TabStop = False
        Me.grpMore.Visible = False
        '
        'chkRefDate
        '
        Me.chkRefDate.AutoSize = True
        Me.chkRefDate.Location = New System.Drawing.Point(128, 41)
        Me.chkRefDate.Name = "chkRefDate"
        Me.chkRefDate.Size = New System.Drawing.Size(72, 17)
        Me.chkRefDate.TabIndex = 3
        Me.chkRefDate.Text = "RefDate"
        Me.chkRefDate.UseVisualStyleBackColor = True
        '
        'chkChequeNo
        '
        Me.chkChequeNo.AutoSize = True
        Me.chkChequeNo.Location = New System.Drawing.Point(6, 18)
        Me.chkChequeNo.Name = "chkChequeNo"
        Me.chkChequeNo.Size = New System.Drawing.Size(89, 17)
        Me.chkChequeNo.TabIndex = 0
        Me.chkChequeNo.Text = "Cheque No"
        Me.chkChequeNo.UseVisualStyleBackColor = True
        '
        'chkRefNo
        '
        Me.chkRefNo.AutoSize = True
        Me.chkRefNo.Location = New System.Drawing.Point(128, 19)
        Me.chkRefNo.Name = "chkRefNo"
        Me.chkRefNo.Size = New System.Drawing.Size(64, 17)
        Me.chkRefNo.TabIndex = 1
        Me.chkRefNo.Text = "Ref No"
        Me.chkRefNo.UseVisualStyleBackColor = True
        '
        'chkChequeDate
        '
        Me.chkChequeDate.AutoSize = True
        Me.chkChequeDate.Location = New System.Drawing.Point(6, 41)
        Me.chkChequeDate.Name = "chkChequeDate"
        Me.chkChequeDate.Size = New System.Drawing.Size(101, 17)
        Me.chkChequeDate.TabIndex = 2
        Me.chkChequeDate.Text = "Cheque Date"
        Me.chkChequeDate.UseVisualStyleBackColor = True
        '
        'chkMore
        '
        Me.chkMore.AutoSize = True
        Me.chkMore.Location = New System.Drawing.Point(10, 415)
        Me.chkMore.Name = "chkMore"
        Me.chkMore.Size = New System.Drawing.Size(54, 17)
        Me.chkMore.TabIndex = 18
        Me.chkMore.Text = "More"
        Me.chkMore.UseVisualStyleBackColor = True
        Me.chkMore.Visible = False
        '
        'chkDetailed
        '
        Me.chkDetailed.AutoSize = True
        Me.chkDetailed.Location = New System.Drawing.Point(173, 341)
        Me.chkDetailed.Name = "chkDetailed"
        Me.chkDetailed.Size = New System.Drawing.Size(144, 17)
        Me.chkDetailed.TabIndex = 13
        Me.chkDetailed.Text = "Bill No Wise Detailed"
        Me.chkDetailed.UseVisualStyleBackColor = True
        Me.chkDetailed.Visible = False
        '
        'rbtTranNo
        '
        Me.rbtTranNo.AutoSize = True
        Me.rbtTranNo.Location = New System.Drawing.Point(147, 364)
        Me.rbtTranNo.Name = "rbtTranNo"
        Me.rbtTranNo.Size = New System.Drawing.Size(66, 17)
        Me.rbtTranNo.TabIndex = 15
        Me.rbtTranNo.Text = "TranNo"
        Me.rbtTranNo.UseVisualStyleBackColor = True
        Me.rbtTranNo.Visible = False
        '
        'rbtAcName
        '
        Me.rbtAcName.AutoSize = True
        Me.rbtAcName.Checked = True
        Me.rbtAcName.Location = New System.Drawing.Point(69, 364)
        Me.rbtAcName.Name = "rbtAcName"
        Me.rbtAcName.Size = New System.Drawing.Size(72, 17)
        Me.rbtAcName.TabIndex = 14
        Me.rbtAcName.TabStop = True
        Me.rbtAcName.Text = "AcName"
        Me.rbtAcName.UseVisualStyleBackColor = True
        Me.rbtAcName.Visible = False
        '
        'chkOpening
        '
        Me.chkOpening.AutoSize = True
        Me.chkOpening.Location = New System.Drawing.Point(5, 343)
        Me.chkOpening.Name = "chkOpening"
        Me.chkOpening.Size = New System.Drawing.Size(73, 17)
        Me.chkOpening.TabIndex = 11
        Me.chkOpening.Text = "Opening"
        Me.chkOpening.UseVisualStyleBackColor = True
        Me.chkOpening.Visible = False
        '
        'chkPageBreak
        '
        Me.chkPageBreak.AutoSize = True
        Me.chkPageBreak.Location = New System.Drawing.Point(84, 343)
        Me.chkPageBreak.Name = "chkPageBreak"
        Me.chkPageBreak.Size = New System.Drawing.Size(92, 17)
        Me.chkPageBreak.TabIndex = 12
        Me.chkPageBreak.Text = "Page Break"
        Me.chkPageBreak.UseVisualStyleBackColor = True
        Me.chkPageBreak.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(4, 322)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 13)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Order By"
        Me.Label2.Visible = False
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'frmCostCentreWiseSales
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(928, 575)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.grpContainer)
        Me.Controls.Add(Me.chkLstUserWise)
        Me.Controls.Add(Me.ChkCreditDebit)
        Me.Controls.Add(Me.chkUserWiseSelectAll)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.chkPageBreak)
        Me.Controls.Add(Me.chkOpening)
        Me.Controls.Add(Me.rbtGeneral)
        Me.Controls.Add(Me.chkWeightDetail)
        Me.Controls.Add(Me.rbtAcName)
        Me.Controls.Add(Me.rbtTranNo)
        Me.Controls.Add(Me.ChkSummary)
        Me.Controls.Add(Me.grpMore)
        Me.Controls.Add(Me.chkDetailed)
        Me.Controls.Add(Me.chkMore)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "frmCostCentreWiseSales"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CostCentreWiseSales"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpContainer.ResumeLayout(False)
        Me.grpContainer.PerformLayout()
        Me.grpMore.ResumeLayout(False)
        Me.grpMore.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkCostCentreSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents grpContainer As System.Windows.Forms.GroupBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkOpening As System.Windows.Forms.CheckBox
    Friend WithEvents chkPageBreak As System.Windows.Forms.CheckBox
    Friend WithEvents rbtTranNo As System.Windows.Forms.RadioButton
    Friend WithEvents rbtAcName As System.Windows.Forms.RadioButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkDetailed As System.Windows.Forms.CheckBox
    Friend WithEvents chkMore As System.Windows.Forms.CheckBox
    Friend WithEvents chkChequeDate As System.Windows.Forms.CheckBox
    Friend WithEvents chkRefDate As System.Windows.Forms.CheckBox
    Friend WithEvents chkRefNo As System.Windows.Forms.CheckBox
    Friend WithEvents chkChequeNo As System.Windows.Forms.CheckBox
    Friend WithEvents grpMore As System.Windows.Forms.GroupBox
    Friend WithEvents ChkSummary As System.Windows.Forms.CheckBox
    Friend WithEvents ChkCreditDebit As System.Windows.Forms.CheckBox
    Friend WithEvents chkWeightDetail As System.Windows.Forms.CheckBox
    Friend WithEvents rbtGeneral As System.Windows.Forms.RadioButton
    Friend WithEvents chkLstUserWise As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkUserWiseSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkCompanySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCompany As System.Windows.Forms.CheckedListBox
End Class
