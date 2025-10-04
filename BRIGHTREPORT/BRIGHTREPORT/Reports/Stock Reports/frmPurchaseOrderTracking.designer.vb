<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmPurchaseOrderTracking
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabGen = New System.Windows.Forms.TabPage()
        Me.pnlGroupFilter = New System.Windows.Forms.Panel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cmbCostcentre = New System.Windows.Forms.ComboBox()
        Me.cmbMetal = New System.Windows.Forms.ComboBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.optBetween = New System.Windows.Forms.RadioButton()
        Me.optAsOn = New System.Windows.Forms.RadioButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Metal = New System.Windows.Forms.Label()
        Me.lblFrom = New System.Windows.Forms.Label()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnView_Search = New System.Windows.Forms.Button()
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.lblTo = New System.Windows.Forms.Label()
        Me.tabView = New System.Windows.Forms.TabPage()
        Me.gridviewDetail = New System.Windows.Forms.DataGridView()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.cmbGridShortCut = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.gridViewHead = New System.Windows.Forms.DataGridView()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.pnlfooter = New System.Windows.Forms.Panel()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.tabMain.SuspendLayout()
        Me.tabGen.SuspendLayout()
        Me.pnlGroupFilter.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tabView.SuspendLayout()
        CType(Me.gridviewDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmbGridShortCut.SuspendLayout()
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.tabMain.Size = New System.Drawing.Size(941, 622)
        Me.tabMain.TabIndex = 0
        '
        'tabGen
        '
        Me.tabGen.Controls.Add(Me.pnlGroupFilter)
        Me.tabGen.Location = New System.Drawing.Point(4, 29)
        Me.tabGen.Name = "tabGen"
        Me.tabGen.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGen.Size = New System.Drawing.Size(933, 589)
        Me.tabGen.TabIndex = 0
        Me.tabGen.Text = "Gen"
        Me.tabGen.UseVisualStyleBackColor = True
        '
        'pnlGroupFilter
        '
        Me.pnlGroupFilter.Controls.Add(Me.GroupBox1)
        Me.pnlGroupFilter.Location = New System.Drawing.Point(182, 151)
        Me.pnlGroupFilter.Name = "pnlGroupFilter"
        Me.pnlGroupFilter.Size = New System.Drawing.Size(532, 261)
        Me.pnlGroupFilter.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmbCostcentre)
        Me.GroupBox1.Controls.Add(Me.cmbMetal)
        Me.GroupBox1.Controls.Add(Me.Panel1)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Metal)
        Me.GroupBox1.Controls.Add(Me.lblFrom)
        Me.GroupBox1.Controls.Add(Me.btnNew)
        Me.GroupBox1.Controls.Add(Me.btnExit)
        Me.GroupBox1.Controls.Add(Me.btnView_Search)
        Me.GroupBox1.Controls.Add(Me.dtpTo)
        Me.GroupBox1.Controls.Add(Me.dtpFrom)
        Me.GroupBox1.Controls.Add(Me.lblTo)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 15)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(519, 233)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'cmbCostcentre
        '
        Me.cmbCostcentre.FormattingEnabled = True
        Me.cmbCostcentre.Location = New System.Drawing.Point(226, 152)
        Me.cmbCostcentre.Name = "cmbCostcentre"
        Me.cmbCostcentre.Size = New System.Drawing.Size(174, 25)
        Me.cmbCostcentre.TabIndex = 5
        '
        'cmbMetal
        '
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(226, 113)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(121, 25)
        Me.cmbMetal.TabIndex = 5
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.optBetween)
        Me.Panel1.Controls.Add(Me.optAsOn)
        Me.Panel1.Location = New System.Drawing.Point(124, 35)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(187, 27)
        Me.Panel1.TabIndex = 35
        '
        'optBetween
        '
        Me.optBetween.AutoSize = True
        Me.optBetween.Location = New System.Drawing.Point(93, 3)
        Me.optBetween.Name = "optBetween"
        Me.optBetween.Size = New System.Drawing.Size(89, 21)
        Me.optBetween.TabIndex = 1
        Me.optBetween.TabStop = True
        Me.optBetween.Text = "Between"
        Me.optBetween.UseVisualStyleBackColor = True
        AddHandler Me.optBetween.CheckedChanged, AddressOf Me.optBetween_CheckedChanged
        '
        'optAsOn
        '
        Me.optAsOn.AutoSize = True
        Me.optAsOn.Location = New System.Drawing.Point(3, 3)
        Me.optAsOn.Name = "optAsOn"
        Me.optAsOn.Size = New System.Drawing.Size(72, 21)
        Me.optAsOn.TabIndex = 0
        Me.optAsOn.TabStop = True
        Me.optAsOn.Text = "As On"
        Me.optAsOn.UseVisualStyleBackColor = True
        AddHandler Me.optAsOn.CheckedChanged, AddressOf Me.optAsOn_CheckedChanged
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(125, 155)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(101, 17)
        Me.Label1.TabIndex = 34
        Me.Label1.Text = "Costcentre : "
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Metal
        '
        Me.Metal.AutoSize = True
        Me.Metal.Location = New System.Drawing.Point(125, 116)
        Me.Metal.Name = "Metal"
        Me.Metal.Size = New System.Drawing.Size(60, 17)
        Me.Metal.TabIndex = 34
        Me.Metal.Text = "Metal : "
        Me.Metal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblFrom
        '
        Me.lblFrom.AutoSize = True
        Me.lblFrom.Location = New System.Drawing.Point(125, 79)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(44, 17)
        Me.lblFrom.TabIndex = 34
        Me.lblFrom.Text = "From"
        Me.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnNew
        '
        Me.btnNew.ContextMenuStrip = Me.ContextMenuStrip1
        Me.btnNew.Location = New System.Drawing.Point(218, 191)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 32
        Me.btnNew.Text = "&New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.NewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(135, 52)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(134, 24)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(134, 24)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(324, 191)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 33
        Me.btnExit.Text = "&Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(112, 191)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 6
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(351, 75)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 24)
        Me.dtpTo.TabIndex = 4
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(225, 75)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(93, 24)
        Me.dtpFrom.TabIndex = 3
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(324, 80)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(24, 17)
        Me.lblTo.TabIndex = 10
        Me.lblTo.Text = "To"
        Me.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.gridviewDetail)
        Me.tabView.Controls.Add(Me.gridView)
        Me.tabView.Controls.Add(Me.gridViewHead)
        Me.tabView.Controls.Add(Me.lblTitle)
        Me.tabView.Controls.Add(Me.pnlfooter)
        Me.tabView.Location = New System.Drawing.Point(4, 29)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(933, 589)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'gridviewDetail
        '
        Me.gridviewDetail.AllowUserToAddRows = False
        Me.gridviewDetail.AllowUserToDeleteRows = False
        Me.gridviewDetail.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridviewDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridviewDetail.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridviewDetail.Location = New System.Drawing.Point(3, 98)
        Me.gridviewDetail.MultiSelect = False
        Me.gridviewDetail.Name = "gridviewDetail"
        Me.gridviewDetail.ReadOnly = True
        Me.gridviewDetail.RowHeadersVisible = False
        Me.gridviewDetail.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.gridviewDetail.RowTemplate.Height = 20
        Me.gridviewDetail.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridviewDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridviewDetail.ShowCellToolTips = False
        Me.gridviewDetail.Size = New System.Drawing.Size(927, 446)
        Me.gridviewDetail.TabIndex = 5
        Me.gridviewDetail.Visible = False
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridView.ContextMenuStrip = Me.cmbGridShortCut
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(3, 98)
        Me.gridView.MultiSelect = False
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.gridView.RowTemplate.Height = 20
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.ShowCellToolTips = False
        Me.gridView.Size = New System.Drawing.Size(927, 446)
        Me.gridView.TabIndex = 1
        '
        'cmbGridShortCut
        '
        Me.cmbGridShortCut.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResizeToolStripMenuItem})
        Me.cmbGridShortCut.Name = "ContextMenuStrip2"
        Me.cmbGridShortCut.Size = New System.Drawing.Size(157, 28)
        '
        'ResizeToolStripMenuItem
        '
        Me.ResizeToolStripMenuItem.CheckOnClick = True
        Me.ResizeToolStripMenuItem.Name = "ResizeToolStripMenuItem"
        Me.ResizeToolStripMenuItem.Size = New System.Drawing.Size(156, 24)
        Me.ResizeToolStripMenuItem.Text = "Auto Resize"
        '
        'gridViewHead
        '
        Me.gridViewHead.AllowUserToAddRows = False
        Me.gridViewHead.AllowUserToDeleteRows = False
        Me.gridViewHead.BackgroundColor = System.Drawing.SystemColors.ControlDark
        Me.gridViewHead.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridViewHead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridViewHead.Dock = System.Windows.Forms.DockStyle.Top
        Me.gridViewHead.Enabled = False
        Me.gridViewHead.Location = New System.Drawing.Point(3, 78)
        Me.gridViewHead.Name = "gridViewHead"
        Me.gridViewHead.ReadOnly = True
        Me.gridViewHead.RowHeadersVisible = False
        Me.gridViewHead.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.gridViewHead.Size = New System.Drawing.Size(927, 20)
        Me.gridViewHead.TabIndex = 3
        '
        'lblTitle
        '
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Location = New System.Drawing.Point(3, 3)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(927, 75)
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
        Me.pnlfooter.Location = New System.Drawing.Point(3, 544)
        Me.pnlfooter.Name = "pnlfooter"
        Me.pnlfooter.Size = New System.Drawing.Size(927, 42)
        Me.pnlfooter.TabIndex = 2
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(256, 6)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 1
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(466, 6)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 0
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(360, 6)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 0
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'frmPurchaseOrderTracking
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(941, 622)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmPurchaseOrderTracking"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Purchase Order Tracking"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.tabMain.ResumeLayout(False)
        Me.tabGen.ResumeLayout(False)
        Me.pnlGroupFilter.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tabView.ResumeLayout(False)
        CType(Me.gridviewDetail, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmbGridShortCut.ResumeLayout(False)
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlfooter.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlGroupFilter As System.Windows.Forms.Panel
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGen As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlfooter As System.Windows.Forms.Panel
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents cmbGridShortCut As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents lblFrom As System.Windows.Forms.Label
    Friend WithEvents gridviewDetail As DataGridView
    Friend WithEvents gridViewHead As DataGridView
    Friend WithEvents Panel1 As Panel
    Friend WithEvents optAsOn As RadioButton
    Friend WithEvents optBetween As RadioButton
    Friend WithEvents Metal As Label
    Friend WithEvents cmbMetal As ComboBox
    Friend WithEvents cmbCostcentre As ComboBox
    Friend WithEvents Label1 As Label
End Class
