<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmClosingStockReport
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
        Me.cmbCategory = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExcel = New System.Windows.Forms.Button()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel7 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.rbtTranNoWise = New System.Windows.Forms.RadioButton()
        Me.rbtDateWise = New System.Windows.Forms.RadioButton()
        Me.rbtSummary = New System.Windows.Forms.RadioButton()
        Me.rbtMonthWise = New System.Windows.Forms.RadioButton()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.chkAsOnDate = New System.Windows.Forms.CheckBox()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Label10 = New System.Windows.Forms.Label()
        Me.CmbCompany = New BrighttechPack.CheckedComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cmbMetal = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.Panel8 = New System.Windows.Forms.Panel()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.TabGeneral = New System.Windows.Forms.TabPage()
        Me.TabView = New System.Windows.Forms.TabPage()
        Me.gridHeader = New System.Windows.Forms.DataGridView()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel7.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.Panel8.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.TabGeneral.SuspendLayout()
        Me.TabView.SuspendLayout()
        CType(Me.gridHeader, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmbCategory
        '
        Me.cmbCategory.FormattingEnabled = True
        Me.cmbCategory.Location = New System.Drawing.Point(161, 119)
        Me.cmbCategory.Name = "cmbCategory"
        Me.cmbCategory.Size = New System.Drawing.Size(231, 21)
        Me.cmbCategory.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(82, 122)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Category"
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(81, 196)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(100, 30)
        Me.btnSearch.TabIndex = 14
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(189, 196)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 15
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(295, 196)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 16
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(560, 12)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 14
        Me.btnPrint.Text = "Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExcel
        '
        Me.btnExcel.Location = New System.Drawing.Point(454, 12)
        Me.btnExcel.Name = "btnExcel"
        Me.btnExcel.Size = New System.Drawing.Size(100, 30)
        Me.btnExcel.TabIndex = 15
        Me.btnExcel.Text = "Excel"
        Me.btnExcel.UseVisualStyleBackColor = True
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 45)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.Size = New System.Drawing.Size(1008, 505)
        Me.gridView.TabIndex = 16
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.NewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 48)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'lblTitle
        '
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1008, 22)
        Me.lblTitle.TabIndex = 16
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Panel7)
        Me.Panel1.Controls.Add(Me.rbtTranNoWise)
        Me.Panel1.Controls.Add(Me.rbtMonthWise)
        Me.Panel1.Controls.Add(Me.CmbCompany)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1008, 604)
        Me.Panel1.TabIndex = 0
        '
        'Panel7
        '
        Me.Panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel7.Controls.Add(Me.Label1)
        Me.Panel7.Controls.Add(Me.rbtDateWise)
        Me.Panel7.Controls.Add(Me.rbtSummary)
        Me.Panel7.Controls.Add(Me.lblTo)
        Me.Panel7.Controls.Add(Me.chkAsOnDate)
        Me.Panel7.Controls.Add(Me.cmbMetal)
        Me.Panel7.Controls.Add(Me.Label9)
        Me.Panel7.Controls.Add(Me.dtpTo)
        Me.Panel7.Controls.Add(Me.dtpFrom)
        Me.Panel7.Controls.Add(Me.Label8)
        Me.Panel7.Controls.Add(Me.chkCmbCostCentre)
        Me.Panel7.Controls.Add(Me.cmbCategory)
        Me.Panel7.Controls.Add(Me.btnSearch)
        Me.Panel7.Controls.Add(Me.btnNew)
        Me.Panel7.Controls.Add(Me.Label3)
        Me.Panel7.Controls.Add(Me.btnExit)
        Me.Panel7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Panel7.Location = New System.Drawing.Point(291, 154)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(475, 248)
        Me.Panel7.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(82, 159)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 13)
        Me.Label1.TabIndex = 27
        Me.Label1.Text = "Group By"
        '
        'rbtTranNoWise
        '
        Me.rbtTranNoWise.AutoSize = True
        Me.rbtTranNoWise.Location = New System.Drawing.Point(560, 459)
        Me.rbtTranNoWise.Name = "rbtTranNoWise"
        Me.rbtTranNoWise.Size = New System.Drawing.Size(96, 17)
        Me.rbtTranNoWise.TabIndex = 3
        Me.rbtTranNoWise.Text = "TranNo Wise"
        Me.rbtTranNoWise.UseVisualStyleBackColor = True
        Me.rbtTranNoWise.Visible = False
        '
        'rbtDateWise
        '
        Me.rbtDateWise.AutoSize = True
        Me.rbtDateWise.Location = New System.Drawing.Point(290, 159)
        Me.rbtDateWise.Name = "rbtDateWise"
        Me.rbtDateWise.Size = New System.Drawing.Size(83, 17)
        Me.rbtDateWise.TabIndex = 2
        Me.rbtDateWise.Text = "Date Wise"
        Me.rbtDateWise.UseVisualStyleBackColor = True
        '
        'rbtSummary
        '
        Me.rbtSummary.AutoSize = True
        Me.rbtSummary.Checked = True
        Me.rbtSummary.Location = New System.Drawing.Point(183, 159)
        Me.rbtSummary.Name = "rbtSummary"
        Me.rbtSummary.Size = New System.Drawing.Size(81, 17)
        Me.rbtSummary.TabIndex = 0
        Me.rbtSummary.TabStop = True
        Me.rbtSummary.Text = "Summary"
        Me.rbtSummary.UseVisualStyleBackColor = True
        '
        'rbtMonthWise
        '
        Me.rbtMonthWise.AutoSize = True
        Me.rbtMonthWise.Location = New System.Drawing.Point(455, 459)
        Me.rbtMonthWise.Name = "rbtMonthWise"
        Me.rbtMonthWise.Size = New System.Drawing.Size(90, 17)
        Me.rbtMonthWise.TabIndex = 1
        Me.rbtMonthWise.Text = "Month Wise"
        Me.rbtMonthWise.UseVisualStyleBackColor = True
        Me.rbtMonthWise.Visible = False
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(269, 31)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(20, 13)
        Me.lblTo.TabIndex = 26
        Me.lblTo.Text = "To"
        Me.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkAsOnDate
        '
        Me.chkAsOnDate.AutoSize = True
        Me.chkAsOnDate.Checked = True
        Me.chkAsOnDate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAsOnDate.Location = New System.Drawing.Point(72, 27)
        Me.chkAsOnDate.Name = "chkAsOnDate"
        Me.chkAsOnDate.Size = New System.Drawing.Size(91, 17)
        Me.chkAsOnDate.TabIndex = 25
        Me.chkAsOnDate.Text = "As On Date"
        Me.chkAsOnDate.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(300, 27)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpTo.TabIndex = 24
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(165, 27)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(93, 21)
        Me.dtpFrom.TabIndex = 23
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(417, 507)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(62, 13)
        Me.Label10.TabIndex = 22
        Me.Label10.Text = "Company"
        Me.Label10.Visible = False
        '
        'CmbCompany
        '
        Me.CmbCompany.CheckOnClick = True
        Me.CmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.CmbCompany.DropDownHeight = 1
        Me.CmbCompany.FormattingEnabled = True
        Me.CmbCompany.IntegralHeight = False
        Me.CmbCompany.Location = New System.Drawing.Point(497, 498)
        Me.CmbCompany.Name = "CmbCompany"
        Me.CmbCompany.Size = New System.Drawing.Size(228, 22)
        Me.CmbCompany.TabIndex = 21
        Me.CmbCompany.ValueSeparator = ", "
        Me.CmbCompany.Visible = False
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(82, 92)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(74, 13)
        Me.Label9.TabIndex = 20
        Me.Label9.Text = "Metal Name"
        '
        'cmbMetal
        '
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(161, 89)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(231, 21)
        Me.cmbMetal.TabIndex = 19
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(82, 62)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(70, 13)
        Me.Label8.TabIndex = 18
        Me.Label8.Text = "Cost Name"
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(161, 58)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(231, 22)
        Me.chkCmbCostCentre.TabIndex = 17
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Panel6
        '
        Me.Panel6.Controls.Add(Me.gridView)
        Me.Panel6.Controls.Add(Me.gridHeader)
        Me.Panel6.Controls.Add(Me.lblTitle)
        Me.Panel6.Controls.Add(Me.Panel8)
        Me.Panel6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel6.Location = New System.Drawing.Point(3, 3)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(1008, 604)
        Me.Panel6.TabIndex = 21
        '
        'Panel8
        '
        Me.Panel8.Controls.Add(Me.btnBack)
        Me.Panel8.Controls.Add(Me.btnPrint)
        Me.Panel8.Controls.Add(Me.btnExcel)
        Me.Panel8.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel8.Location = New System.Drawing.Point(0, 550)
        Me.Panel8.Name = "Panel8"
        Me.Panel8.Size = New System.Drawing.Size(1008, 54)
        Me.Panel8.TabIndex = 17
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(348, 12)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 16
        Me.btnBack.Text = "Back"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.TabGeneral)
        Me.tabMain.Controls.Add(Me.TabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1022, 636)
        Me.tabMain.TabIndex = 0
        '
        'TabGeneral
        '
        Me.TabGeneral.Controls.Add(Me.Panel1)
        Me.TabGeneral.Location = New System.Drawing.Point(4, 22)
        Me.TabGeneral.Name = "TabGeneral"
        Me.TabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.TabGeneral.Size = New System.Drawing.Size(1014, 610)
        Me.TabGeneral.TabIndex = 0
        Me.TabGeneral.Text = "General"
        Me.TabGeneral.UseVisualStyleBackColor = True
        '
        'TabView
        '
        Me.TabView.Controls.Add(Me.Panel6)
        Me.TabView.Location = New System.Drawing.Point(4, 22)
        Me.TabView.Name = "TabView"
        Me.TabView.Padding = New System.Windows.Forms.Padding(3)
        Me.TabView.Size = New System.Drawing.Size(1014, 610)
        Me.TabView.TabIndex = 1
        Me.TabView.Text = "View"
        Me.TabView.UseVisualStyleBackColor = True
        '
        'gridHeader
        '
        Me.gridHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridHeader.Location = New System.Drawing.Point(0, 22)
        Me.gridHeader.Name = "gridHeader"
        Me.gridHeader.Size = New System.Drawing.Size(1008, 23)
        Me.gridHeader.TabIndex = 18
        '
        'frmClosingStockReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1022, 636)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmClosingStockReport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ProcessWise Report"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel7.ResumeLayout(False)
        Me.Panel7.PerformLayout()
        Me.Panel6.ResumeLayout(False)
        Me.Panel8.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.TabGeneral.ResumeLayout(False)
        Me.TabView.ResumeLayout(False)
        CType(Me.gridHeader, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cmbCategory As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExcel As System.Windows.Forms.Button
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Friend WithEvents Panel7 As Panel
    Friend WithEvents tabMain As TabControl
    Friend WithEvents TabGeneral As TabPage
    Friend WithEvents TabView As TabPage
    Friend WithEvents Panel8 As Panel
    Friend WithEvents btnBack As Button
    Friend WithEvents Label8 As Label
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Label9 As Label
    Friend WithEvents cmbMetal As ComboBox
    Friend WithEvents Label10 As Label
    Friend WithEvents CmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents chkAsOnDate As CheckBox
    Friend WithEvents lblTo As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents rbtTranNoWise As RadioButton
    Friend WithEvents rbtDateWise As RadioButton
    Friend WithEvents rbtSummary As RadioButton
    Friend WithEvents rbtMonthWise As RadioButton
    Friend WithEvents gridHeader As DataGridView
End Class
