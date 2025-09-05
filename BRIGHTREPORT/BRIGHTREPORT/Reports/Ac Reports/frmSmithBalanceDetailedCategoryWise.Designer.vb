<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSmithBalanceDetailedCategoryWise
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
        Me.ChkApproval = New System.Windows.Forms.CheckBox
        Me.PnlMark = New System.Windows.Forms.Panel
        Me.rbtLocal = New System.Windows.Forms.RadioButton
        Me.rbtOutstation = New System.Windows.Forms.RadioButton
        Me.rbtBothMU = New System.Windows.Forms.RadioButton
        Me.chkDetailed = New System.Windows.Forms.CheckBox
        Me.chkCatName = New System.Windows.Forms.CheckBox
        Me.cmbSmith_MAN = New BrighttechPack.CheckedComboBox
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.cmbTranType = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnSearch = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.chkCostCentreSelectAll = New System.Windows.Forms.CheckBox
        Me.chkLstCostCentre = New System.Windows.Forms.CheckedListBox
        Me.chkLstCategory = New System.Windows.Forms.CheckedListBox
        Me.chkMetalSelectAll = New System.Windows.Forms.CheckBox
        Me.chkCategorySelectAll = New System.Windows.Forms.CheckBox
        Me.chkLstMetal = New System.Windows.Forms.CheckedListBox
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.pnlContainer.SuspendLayout()
        Me.PnlMark.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlContainer
        '
        Me.pnlContainer.Controls.Add(Me.ChkApproval)
        Me.pnlContainer.Controls.Add(Me.PnlMark)
        Me.pnlContainer.Controls.Add(Me.chkDetailed)
        Me.pnlContainer.Controls.Add(Me.chkCatName)
        Me.pnlContainer.Controls.Add(Me.cmbSmith_MAN)
        Me.pnlContainer.Controls.Add(Me.dtpTo)
        Me.pnlContainer.Controls.Add(Me.dtpFrom)
        Me.pnlContainer.Controls.Add(Me.cmbTranType)
        Me.pnlContainer.Controls.Add(Me.Label2)
        Me.pnlContainer.Controls.Add(Me.Label4)
        Me.pnlContainer.Controls.Add(Me.Label3)
        Me.pnlContainer.Controls.Add(Me.Label1)
        Me.pnlContainer.Controls.Add(Me.btnExit)
        Me.pnlContainer.Controls.Add(Me.btnSearch)
        Me.pnlContainer.Controls.Add(Me.btnNew)
        Me.pnlContainer.Controls.Add(Me.chkCostCentreSelectAll)
        Me.pnlContainer.Controls.Add(Me.chkLstCostCentre)
        Me.pnlContainer.Controls.Add(Me.chkLstCategory)
        Me.pnlContainer.Controls.Add(Me.chkMetalSelectAll)
        Me.pnlContainer.Controls.Add(Me.chkCategorySelectAll)
        Me.pnlContainer.Controls.Add(Me.chkLstMetal)
        Me.pnlContainer.Location = New System.Drawing.Point(175, 39)
        Me.pnlContainer.Name = "pnlContainer"
        Me.pnlContainer.Size = New System.Drawing.Size(437, 424)
        Me.pnlContainer.TabIndex = 0
        '
        'ChkApproval
        '
        Me.ChkApproval.AutoSize = True
        Me.ChkApproval.Enabled = False
        Me.ChkApproval.Location = New System.Drawing.Point(228, 306)
        Me.ChkApproval.Name = "ChkApproval"
        Me.ChkApproval.Size = New System.Drawing.Size(152, 17)
        Me.ChkApproval.TabIndex = 23
        Me.ChkApproval.Text = "With Approval Transaction"
        Me.ChkApproval.UseVisualStyleBackColor = True
        '
        'PnlMark
        '
        Me.PnlMark.Controls.Add(Me.rbtLocal)
        Me.PnlMark.Controls.Add(Me.rbtOutstation)
        Me.PnlMark.Controls.Add(Me.rbtBothMU)
        Me.PnlMark.Location = New System.Drawing.Point(20, 331)
        Me.PnlMark.Name = "PnlMark"
        Me.PnlMark.Size = New System.Drawing.Size(249, 29)
        Me.PnlMark.TabIndex = 16
        '
        'rbtLocal
        '
        Me.rbtLocal.AutoSize = True
        Me.rbtLocal.Location = New System.Drawing.Point(72, 4)
        Me.rbtLocal.Name = "rbtLocal"
        Me.rbtLocal.Size = New System.Drawing.Size(51, 17)
        Me.rbtLocal.TabIndex = 1
        Me.rbtLocal.Text = "Local"
        Me.rbtLocal.UseVisualStyleBackColor = True
        '
        'rbtOutstation
        '
        Me.rbtOutstation.AutoSize = True
        Me.rbtOutstation.Location = New System.Drawing.Point(142, 4)
        Me.rbtOutstation.Name = "rbtOutstation"
        Me.rbtOutstation.Size = New System.Drawing.Size(73, 17)
        Me.rbtOutstation.TabIndex = 2
        Me.rbtOutstation.Text = "Outstation"
        Me.rbtOutstation.UseVisualStyleBackColor = True
        '
        'rbtBothMU
        '
        Me.rbtBothMU.AutoSize = True
        Me.rbtBothMU.Checked = True
        Me.rbtBothMU.Location = New System.Drawing.Point(5, 3)
        Me.rbtBothMU.Name = "rbtBothMU"
        Me.rbtBothMU.Size = New System.Drawing.Size(47, 17)
        Me.rbtBothMU.TabIndex = 0
        Me.rbtBothMU.TabStop = True
        Me.rbtBothMU.Text = "Both"
        Me.rbtBothMU.UseVisualStyleBackColor = True
        '
        'chkDetailed
        '
        Me.chkDetailed.AutoSize = True
        Me.chkDetailed.Location = New System.Drawing.Point(137, 306)
        Me.chkDetailed.Name = "chkDetailed"
        Me.chkDetailed.Size = New System.Drawing.Size(65, 17)
        Me.chkDetailed.TabIndex = 15
        Me.chkDetailed.Text = "Detailed"
        Me.chkDetailed.UseVisualStyleBackColor = True
        '
        'chkCatName
        '
        Me.chkCatName.AutoSize = True
        Me.chkCatName.Location = New System.Drawing.Point(20, 306)
        Me.chkCatName.Name = "chkCatName"
        Me.chkCatName.Size = New System.Drawing.Size(95, 17)
        Me.chkCatName.TabIndex = 14
        Me.chkCatName.Text = "Category Wise"
        Me.chkCatName.UseVisualStyleBackColor = True
        '
        'cmbSmith_MAN
        '
        Me.cmbSmith_MAN.CheckOnClick = True
        Me.cmbSmith_MAN.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbSmith_MAN.DropDownHeight = 1
        Me.cmbSmith_MAN.FormattingEnabled = True
        Me.cmbSmith_MAN.IntegralHeight = False
        Me.cmbSmith_MAN.Location = New System.Drawing.Point(87, 78)
        Me.cmbSmith_MAN.Name = "cmbSmith_MAN"
        Me.cmbSmith_MAN.Size = New System.Drawing.Size(337, 21)
        Me.cmbSmith_MAN.TabIndex = 7
        Me.cmbSmith_MAN.ValueSeparator = ", "
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
        'cmbTranType
        '
        Me.cmbTranType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTranType.FormattingEnabled = True
        Me.cmbTranType.Location = New System.Drawing.Point(87, 14)
        Me.cmbTranType.Name = "cmbTranType"
        Me.cmbTranType.Size = New System.Drawing.Size(164, 21)
        Me.cmbTranType.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 17)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Tran Type"
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
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(17, 81)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(33, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Smith"
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
        Me.btnExit.Location = New System.Drawing.Point(272, 366)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 19
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(60, 366)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 17
        Me.btnSearch.Text = "&Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(166, 366)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 18
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'chkCostCentreSelectAll
        '
        Me.chkCostCentreSelectAll.AutoSize = True
        Me.chkCostCentreSelectAll.Location = New System.Drawing.Point(20, 111)
        Me.chkCostCentreSelectAll.Name = "chkCostCentreSelectAll"
        Me.chkCostCentreSelectAll.Size = New System.Drawing.Size(81, 17)
        Me.chkCostCentreSelectAll.TabIndex = 8
        Me.chkCostCentreSelectAll.Text = "Cost Centre"
        Me.chkCostCentreSelectAll.UseVisualStyleBackColor = True
        '
        'chkLstCostCentre
        '
        Me.chkLstCostCentre.FormattingEnabled = True
        Me.chkLstCostCentre.Location = New System.Drawing.Point(17, 130)
        Me.chkLstCostCentre.Name = "chkLstCostCentre"
        Me.chkLstCostCentre.Size = New System.Drawing.Size(201, 64)
        Me.chkLstCostCentre.TabIndex = 9
        '
        'chkLstCategory
        '
        Me.chkLstCategory.FormattingEnabled = True
        Me.chkLstCategory.Location = New System.Drawing.Point(17, 221)
        Me.chkLstCategory.Name = "chkLstCategory"
        Me.chkLstCategory.Size = New System.Drawing.Size(407, 79)
        Me.chkLstCategory.TabIndex = 13
        '
        'chkMetalSelectAll
        '
        Me.chkMetalSelectAll.AutoSize = True
        Me.chkMetalSelectAll.Location = New System.Drawing.Point(228, 111)
        Me.chkMetalSelectAll.Name = "chkMetalSelectAll"
        Me.chkMetalSelectAll.Size = New System.Drawing.Size(52, 17)
        Me.chkMetalSelectAll.TabIndex = 10
        Me.chkMetalSelectAll.Text = "Metal"
        Me.chkMetalSelectAll.UseVisualStyleBackColor = True
        '
        'chkCategorySelectAll
        '
        Me.chkCategorySelectAll.AutoSize = True
        Me.chkCategorySelectAll.Location = New System.Drawing.Point(20, 204)
        Me.chkCategorySelectAll.Name = "chkCategorySelectAll"
        Me.chkCategorySelectAll.Size = New System.Drawing.Size(68, 17)
        Me.chkCategorySelectAll.TabIndex = 12
        Me.chkCategorySelectAll.Text = "Category"
        Me.chkCategorySelectAll.UseVisualStyleBackColor = True
        '
        'chkLstMetal
        '
        Me.chkLstMetal.FormattingEnabled = True
        Me.chkLstMetal.Location = New System.Drawing.Point(225, 130)
        Me.chkLstMetal.Name = "chkLstMetal"
        Me.chkLstMetal.Size = New System.Drawing.Size(199, 64)
        Me.chkLstMetal.TabIndex = 11
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
        'frmSmithBalanceDetailedCategoryWise
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(787, 503)
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlContainer)
        Me.KeyPreview = True
        Me.Name = "frmSmithBalanceDetailedCategoryWise"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Smith Balance Detailed Category Wise"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlContainer.ResumeLayout(False)
        Me.pnlContainer.PerformLayout()
        Me.PnlMark.ResumeLayout(False)
        Me.PnlMark.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlContainer As System.Windows.Forms.Panel
    Friend WithEvents cmbSmith_MAN As BrighttechPack.CheckedComboBox
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents cmbTranType As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents chkCostCentreSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstCostCentre As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkLstCategory As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkMetalSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkCategorySelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkLstMetal As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkCatName As System.Windows.Forms.CheckBox
    Friend WithEvents chkDetailed As System.Windows.Forms.CheckBox
    Friend WithEvents PnlMark As System.Windows.Forms.Panel
    Friend WithEvents rbtLocal As System.Windows.Forms.RadioButton
    Friend WithEvents rbtOutstation As System.Windows.Forms.RadioButton
    Friend WithEvents rbtBothMU As System.Windows.Forms.RadioButton
    Friend WithEvents ChkApproval As System.Windows.Forms.CheckBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
