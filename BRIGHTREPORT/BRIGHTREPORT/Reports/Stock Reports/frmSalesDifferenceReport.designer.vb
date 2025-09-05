<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSalesDifferenceReport
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
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabGen = New System.Windows.Forms.TabPage
        Me.pnlGroupFilter = New System.Windows.Forms.Panel
        Me.btnNew = New System.Windows.Forms.Button
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.rbtIssue = New System.Windows.Forms.RadioButton
        Me.rbtReceipt = New System.Windows.Forms.RadioButton
        Me.chkCmbALLOY = New GiritechPack.CheckedComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.chkCmbMetal = New GiritechPack.CheckedComboBox
        Me.label10 = New System.Windows.Forms.Label
        Me.pnlDisStnResult = New System.Windows.Forms.Panel
        Me.rbtCategorywise = New System.Windows.Forms.RadioButton
        Me.rbtTranDate = New System.Windows.Forms.RadioButton
        Me.chkCmbCostCentre = New GiritechPack.CheckedComboBox
        Me.dtpTo = New GiritechPack.DatePicker(Me.components)
        Me.dtpFrom = New GiritechPack.DatePicker(Me.components)
        Me.Label = New System.Windows.Forms.Label
        Me.lblTo = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.tabView = New System.Windows.Forms.TabPage
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.lblTitle = New System.Windows.Forms.Label
        Me.pnlfooter = New System.Windows.Forms.Panel
        Me.btnBack = New System.Windows.Forms.Button
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.tabMain.SuspendLayout()
        Me.tabGen.SuspendLayout()
        Me.pnlGroupFilter.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pnlDisStnResult.SuspendLayout()
        Me.tabView.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmbGridShortCut.SuspendLayout()
        Me.pnlfooter.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabMain
        '
        Me.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tabMain.Controls.Add(Me.tabGen)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(994, 626)
        Me.tabMain.TabIndex = 3
        '
        'tabGen
        '
        Me.tabGen.Controls.Add(Me.pnlGroupFilter)
        Me.tabGen.Location = New System.Drawing.Point(4, 25)
        Me.tabGen.Name = "tabGen"
        Me.tabGen.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGen.Size = New System.Drawing.Size(986, 597)
        Me.tabGen.TabIndex = 0
        Me.tabGen.Text = "Gen"
        Me.tabGen.UseVisualStyleBackColor = True
        '
        'pnlGroupFilter
        '
        Me.pnlGroupFilter.Controls.Add(Me.btnNew)
        Me.pnlGroupFilter.Controls.Add(Me.GroupBox1)
        Me.pnlGroupFilter.Controls.Add(Me.btnExit)
        Me.pnlGroupFilter.Controls.Add(Me.btnView_Search)
        Me.pnlGroupFilter.Location = New System.Drawing.Point(276, 5)
        Me.pnlGroupFilter.Name = "pnlGroupFilter"
        Me.pnlGroupFilter.Size = New System.Drawing.Size(535, 542)
        Me.pnlGroupFilter.TabIndex = 0
        '
        'btnNew
        '
        Me.btnNew.ContextMenuStrip = Me.ContextMenuStrip1
        Me.btnNew.Location = New System.Drawing.Point(232, 506)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 2
        Me.btnNew.Text = "&New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
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
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Panel1)
        Me.GroupBox1.Controls.Add(Me.chkCmbALLOY)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.chkCmbMetal)
        Me.GroupBox1.Controls.Add(Me.label10)
        Me.GroupBox1.Controls.Add(Me.pnlDisStnResult)
        Me.GroupBox1.Controls.Add(Me.chkCmbCostCentre)
        Me.GroupBox1.Controls.Add(Me.dtpTo)
        Me.GroupBox1.Controls.Add(Me.dtpFrom)
        Me.GroupBox1.Controls.Add(Me.Label)
        Me.GroupBox1.Controls.Add(Me.lblTo)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(529, 497)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbtIssue)
        Me.Panel1.Controls.Add(Me.rbtReceipt)
        Me.Panel1.Location = New System.Drawing.Point(177, 336)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(236, 25)
        Me.Panel1.TabIndex = 11
        Me.Panel1.Visible = False
        '
        'rbtIssue
        '
        Me.rbtIssue.AutoSize = True
        Me.rbtIssue.Location = New System.Drawing.Point(102, 3)
        Me.rbtIssue.Name = "rbtIssue"
        Me.rbtIssue.Size = New System.Drawing.Size(61, 17)
        Me.rbtIssue.TabIndex = 1
        Me.rbtIssue.Text = "ISSUE"
        Me.rbtIssue.UseVisualStyleBackColor = True
        '
        'rbtReceipt
        '
        Me.rbtReceipt.AutoSize = True
        Me.rbtReceipt.Checked = True
        Me.rbtReceipt.Location = New System.Drawing.Point(8, 3)
        Me.rbtReceipt.Name = "rbtReceipt"
        Me.rbtReceipt.Size = New System.Drawing.Size(67, 17)
        Me.rbtReceipt.TabIndex = 0
        Me.rbtReceipt.TabStop = True
        Me.rbtReceipt.Text = "Receipt"
        Me.rbtReceipt.UseVisualStyleBackColor = True
        '
        'chkCmbALLOY
        '
        Me.chkCmbALLOY.CheckOnClick = True
        Me.chkCmbALLOY.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbALLOY.DropDownHeight = 1
        Me.chkCmbALLOY.FormattingEnabled = True
        Me.chkCmbALLOY.IntegralHeight = False
        Me.chkCmbALLOY.Location = New System.Drawing.Point(177, 277)
        Me.chkCmbALLOY.Name = "chkCmbALLOY"
        Me.chkCmbALLOY.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbALLOY.TabIndex = 9
        Me.chkCmbALLOY.ValueSeparator = ","
        Me.chkCmbALLOY.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(61, 282)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(69, 13)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Alloy Metal"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label1.Visible = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(61, 198)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(36, 13)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "From"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbMetal
        '
        Me.chkCmbMetal.CheckOnClick = True
        Me.chkCmbMetal.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbMetal.DropDownHeight = 1
        Me.chkCmbMetal.FormattingEnabled = True
        Me.chkCmbMetal.IntegralHeight = False
        Me.chkCmbMetal.Location = New System.Drawing.Point(177, 221)
        Me.chkCmbMetal.Name = "chkCmbMetal"
        Me.chkCmbMetal.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbMetal.TabIndex = 5
        Me.chkCmbMetal.ValueSeparator = ","
        '
        'label10
        '
        Me.label10.AutoSize = True
        Me.label10.Location = New System.Drawing.Point(61, 226)
        Me.label10.Name = "label10"
        Me.label10.Size = New System.Drawing.Size(37, 13)
        Me.label10.TabIndex = 4
        Me.label10.Text = "Metal"
        Me.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnlDisStnResult
        '
        Me.pnlDisStnResult.Controls.Add(Me.rbtCategorywise)
        Me.pnlDisStnResult.Controls.Add(Me.rbtTranDate)
        Me.pnlDisStnResult.Location = New System.Drawing.Point(177, 305)
        Me.pnlDisStnResult.Name = "pnlDisStnResult"
        Me.pnlDisStnResult.Size = New System.Drawing.Size(236, 25)
        Me.pnlDisStnResult.TabIndex = 10
        Me.pnlDisStnResult.Visible = False
        '
        'rbtCategorywise
        '
        Me.rbtCategorywise.AutoSize = True
        Me.rbtCategorywise.Location = New System.Drawing.Point(102, 3)
        Me.rbtCategorywise.Name = "rbtCategorywise"
        Me.rbtCategorywise.Size = New System.Drawing.Size(109, 17)
        Me.rbtCategorywise.TabIndex = 1
        Me.rbtCategorywise.Text = "Category Wise"
        Me.rbtCategorywise.UseVisualStyleBackColor = True
        '
        'rbtTranDate
        '
        Me.rbtTranDate.AutoSize = True
        Me.rbtTranDate.Checked = True
        Me.rbtTranDate.Location = New System.Drawing.Point(8, 3)
        Me.rbtTranDate.Name = "rbtTranDate"
        Me.rbtTranDate.Size = New System.Drawing.Size(82, 17)
        Me.rbtTranDate.TabIndex = 0
        Me.rbtTranDate.TabStop = True
        Me.rbtTranDate.Text = "Tran Date"
        Me.rbtTranDate.UseVisualStyleBackColor = True
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(177, 249)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(290, 22)
        Me.chkCmbCostCentre.TabIndex = 7
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(303, 194)
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
        Me.dtpFrom.Location = New System.Drawing.Point(177, 194)
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
        'Label
        '
        Me.Label.AutoSize = True
        Me.Label.Location = New System.Drawing.Point(61, 254)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(76, 13)
        Me.Label.TabIndex = 6
        Me.Label.Text = "Cost Centre"
        Me.Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(276, 198)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(21, 13)
        Me.lblTo.TabIndex = 2
        Me.lblTo.Text = "To"
        Me.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(338, 506)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 3
        Me.btnExit.Text = "&Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(126, 507)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 1
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.gridView)
        Me.tabView.Controls.Add(Me.lblTitle)
        Me.tabView.Controls.Add(Me.pnlfooter)
        Me.tabView.Location = New System.Drawing.Point(4, 25)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(986, 597)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridView.ContextMenuStrip = Me.cmbGridShortCut
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(3, 49)
        Me.gridView.MultiSelect = False
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.gridView.RowTemplate.Height = 20
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.ShowCellToolTips = False
        Me.gridView.Size = New System.Drawing.Size(980, 503)
        Me.gridView.TabIndex = 1
        '
        'cmbGridShortCut
        '
        Me.cmbGridShortCut.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResizeToolStripMenuItem})
        Me.cmbGridShortCut.Name = "ContextMenuStrip2"
        Me.cmbGridShortCut.Size = New System.Drawing.Size(132, 26)
        '
        'ResizeToolStripMenuItem
        '
        Me.ResizeToolStripMenuItem.CheckOnClick = True
        Me.ResizeToolStripMenuItem.Name = "ResizeToolStripMenuItem"
        Me.ResizeToolStripMenuItem.Size = New System.Drawing.Size(131, 22)
        Me.ResizeToolStripMenuItem.Text = "Auto Resize"
        '
        'lblTitle
        '
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Location = New System.Drawing.Point(3, 3)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(980, 46)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Label3"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlfooter
        '
        Me.pnlfooter.Controls.Add(Me.btnBack)
        Me.pnlfooter.Controls.Add(Me.btnPrint)
        Me.pnlfooter.Controls.Add(Me.btnExport)
        Me.pnlfooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlfooter.Location = New System.Drawing.Point(3, 552)
        Me.pnlfooter.Name = "pnlfooter"
        Me.pnlfooter.Size = New System.Drawing.Size(980, 42)
        Me.pnlfooter.TabIndex = 2
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(598, 3)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 1
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(814, 3)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 0
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(706, 3)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 0
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'frmSalesDifferenceReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(994, 626)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmSalesDifferenceReport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Sales Difference"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.tabMain.ResumeLayout(False)
        Me.tabGen.ResumeLayout(False)
        Me.pnlGroupFilter.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlDisStnResult.ResumeLayout(False)
        Me.pnlDisStnResult.PerformLayout()
        Me.tabView.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmbGridShortCut.ResumeLayout(False)
        Me.pnlfooter.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlGroupFilter As System.Windows.Forms.Panel
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label As System.Windows.Forms.Label
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents label10 As System.Windows.Forms.Label
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGen As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlfooter As System.Windows.Forms.Panel
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents dtpFrom As GiritechPack.DatePicker
    Friend WithEvents chkCmbMetal As GiritechPack.CheckedComboBox
    Friend WithEvents chkCmbCostCentre As GiritechPack.CheckedComboBox
    Friend WithEvents pnlDisStnResult As System.Windows.Forms.Panel
    Friend WithEvents rbtCategorywise As System.Windows.Forms.RadioButton
    Friend WithEvents rbtTranDate As System.Windows.Forms.RadioButton
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents dtpTo As GiritechPack.DatePicker
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents chkCmbALLOY As GiritechPack.CheckedComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rbtIssue As System.Windows.Forms.RadioButton
    Friend WithEvents rbtReceipt As System.Windows.Forms.RadioButton
End Class
