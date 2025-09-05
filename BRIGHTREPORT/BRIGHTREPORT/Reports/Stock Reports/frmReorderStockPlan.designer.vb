<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReorderStockPlan
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
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.grpControls = New System.Windows.Forms.GroupBox
        Me.CmbItemName = New BrighttechPack.CheckedComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.cmbMetal = New BrighttechPack.CheckedComboBox
        Me.dtpAsOnDate = New BrighttechPack.DatePicker(Me.components)
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.cmbCostCentre = New System.Windows.Forms.ComboBox
        Me.cmbSubItemName = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.pnlView = New System.Windows.Forms.Panel
        Me.pnlgrid = New System.Windows.Forms.Panel
        Me.pnlFooter = New System.Windows.Forms.Panel
        Me.btnBack = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.btnPrint = New System.Windows.Forms.Button
        Me.lblTitle = New System.Windows.Forms.Label
        Me.pnlMain = New System.Windows.Forms.Panel
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabGen = New System.Windows.Forms.TabPage
        Me.tabView = New System.Windows.Forms.TabPage
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.grpControls.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlView.SuspendLayout()
        Me.pnlgrid.SuspendLayout()
        Me.pnlFooter.SuspendLayout()
        Me.pnlMain.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabGen.SuspendLayout()
        Me.tabView.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(37, 184)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 10
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'grpControls
        '
        Me.grpControls.Controls.Add(Me.CmbItemName)
        Me.grpControls.Controls.Add(Me.Label4)
        Me.grpControls.Controls.Add(Me.cmbMetal)
        Me.grpControls.Controls.Add(Me.dtpAsOnDate)
        Me.grpControls.Controls.Add(Me.btnNew)
        Me.grpControls.Controls.Add(Me.btnExit)
        Me.grpControls.Controls.Add(Me.cmbCostCentre)
        Me.grpControls.Controls.Add(Me.cmbSubItemName)
        Me.grpControls.Controls.Add(Me.Label5)
        Me.grpControls.Controls.Add(Me.Label3)
        Me.grpControls.Controls.Add(Me.Label2)
        Me.grpControls.Controls.Add(Me.Label1)
        Me.grpControls.Controls.Add(Me.btnView_Search)
        Me.grpControls.Location = New System.Drawing.Point(314, 97)
        Me.grpControls.Name = "grpControls"
        Me.grpControls.Size = New System.Drawing.Size(414, 224)
        Me.grpControls.TabIndex = 0
        Me.grpControls.TabStop = False
        '
        'CmbItemName
        '
        Me.CmbItemName.CheckOnClick = True
        Me.CmbItemName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.CmbItemName.DropDownHeight = 1
        Me.CmbItemName.FormattingEnabled = True
        Me.CmbItemName.IntegralHeight = False
        Me.CmbItemName.Location = New System.Drawing.Point(124, 48)
        Me.CmbItemName.Name = "CmbItemName"
        Me.CmbItemName.Size = New System.Drawing.Size(229, 22)
        Me.CmbItemName.TabIndex = 3
        Me.CmbItemName.ValueSeparator = ", "
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(6, 19)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(77, 21)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Metal Name"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbMetal
        '
        Me.cmbMetal.CheckOnClick = True
        Me.cmbMetal.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbMetal.DropDownHeight = 1
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.IntegralHeight = False
        Me.cmbMetal.Location = New System.Drawing.Point(124, 20)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(229, 22)
        Me.cmbMetal.TabIndex = 1
        Me.cmbMetal.ValueSeparator = ", "
        '
        'dtpAsOnDate
        '
        Me.dtpAsOnDate.Location = New System.Drawing.Point(124, 131)
        Me.dtpAsOnDate.Mask = "##/##/####"
        Me.dtpAsOnDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpAsOnDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpAsOnDate.Name = "dtpAsOnDate"
        Me.dtpAsOnDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpAsOnDate.Size = New System.Drawing.Size(93, 21)
        Me.dtpAsOnDate.TabIndex = 9
        Me.dtpAsOnDate.Text = "07/03/9998"
        Me.dtpAsOnDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(145, 184)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 11
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(253, 184)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 12
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'cmbCostCentre
        '
        Me.cmbCostCentre.FormattingEnabled = True
        Me.cmbCostCentre.Location = New System.Drawing.Point(124, 103)
        Me.cmbCostCentre.Name = "cmbCostCentre"
        Me.cmbCostCentre.Size = New System.Drawing.Size(229, 21)
        Me.cmbCostCentre.TabIndex = 7
        '
        'cmbSubItemName
        '
        Me.cmbSubItemName.FormattingEnabled = True
        Me.cmbSubItemName.Location = New System.Drawing.Point(124, 76)
        Me.cmbSubItemName.Name = "cmbSubItemName"
        Me.cmbSubItemName.Size = New System.Drawing.Size(229, 21)
        Me.cmbSubItemName.TabIndex = 5
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(6, 130)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(112, 21)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "As on Date"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(6, 102)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(112, 21)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(6, 75)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(112, 21)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Sub Item "
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(6, 47)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(77, 21)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Item Name"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 0)
        Me.gridView.MultiSelect = False
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 18
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridView.Size = New System.Drawing.Size(1008, 529)
        Me.gridView.TabIndex = 0
        '
        'pnlView
        '
        Me.pnlView.Controls.Add(Me.pnlgrid)
        Me.pnlView.Controls.Add(Me.pnlFooter)
        Me.pnlView.Controls.Add(Me.lblTitle)
        Me.pnlView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlView.Location = New System.Drawing.Point(3, 3)
        Me.pnlView.Name = "pnlView"
        Me.pnlView.Size = New System.Drawing.Size(1008, 608)
        Me.pnlView.TabIndex = 3
        '
        'pnlgrid
        '
        Me.pnlgrid.Controls.Add(Me.gridView)
        Me.pnlgrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlgrid.Location = New System.Drawing.Point(0, 31)
        Me.pnlgrid.Name = "pnlgrid"
        Me.pnlgrid.Size = New System.Drawing.Size(1008, 529)
        Me.pnlgrid.TabIndex = 3
        '
        'pnlFooter
        '
        Me.pnlFooter.Controls.Add(Me.btnBack)
        Me.pnlFooter.Controls.Add(Me.btnExport)
        Me.pnlFooter.Controls.Add(Me.btnPrint)
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Location = New System.Drawing.Point(0, 560)
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Size = New System.Drawing.Size(1008, 48)
        Me.pnlFooter.TabIndex = 2
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(610, 3)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(100, 30)
        Me.btnBack.TabIndex = 2
        Me.btnBack.Text = "Back [Esc]"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(716, 3)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 1
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(822, 3)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 0
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1008, 31)
        Me.lblTitle.TabIndex = 1
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlMain
        '
        Me.pnlMain.Controls.Add(Me.tabMain)
        Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlMain.Location = New System.Drawing.Point(0, 0)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(1022, 640)
        Me.pnlMain.TabIndex = 0
        '
        'tabMain
        '
        Me.tabMain.Controls.Add(Me.tabGen)
        Me.tabMain.Controls.Add(Me.tabView)
        Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabMain.Location = New System.Drawing.Point(0, 0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1022, 640)
        Me.tabMain.TabIndex = 4
        '
        'tabGen
        '
        Me.tabGen.Controls.Add(Me.grpControls)
        Me.tabGen.Location = New System.Drawing.Point(4, 22)
        Me.tabGen.Name = "tabGen"
        Me.tabGen.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGen.Size = New System.Drawing.Size(1014, 614)
        Me.tabGen.TabIndex = 0
        Me.tabGen.Text = "Gen"
        Me.tabGen.UseVisualStyleBackColor = True
        '
        'tabView
        '
        Me.tabView.Controls.Add(Me.pnlView)
        Me.tabView.Location = New System.Drawing.Point(4, 22)
        Me.tabView.Name = "tabView"
        Me.tabView.Padding = New System.Windows.Forms.Padding(3)
        Me.tabView.Size = New System.Drawing.Size(1014, 614)
        Me.tabView.TabIndex = 1
        Me.tabView.Text = "View"
        Me.tabView.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem, Me.NewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 48)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'frmReorderStockPlan
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1022, 640)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmReorderStockPlan"
        Me.Text = "Reorder Stock"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpControls.ResumeLayout(False)
        Me.grpControls.PerformLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlView.ResumeLayout(False)
        Me.pnlgrid.ResumeLayout(False)
        Me.pnlFooter.ResumeLayout(False)
        Me.pnlMain.ResumeLayout(False)
        Me.tabMain.ResumeLayout(False)
        Me.tabGen.ResumeLayout(False)
        Me.tabView.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents grpControls As System.Windows.Forms.GroupBox
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents pnlView As System.Windows.Forms.Panel
    Friend WithEvents pnlMain As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbCostCentre As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSubItemName As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabGen As System.Windows.Forms.TabPage
    Friend WithEvents tabView As System.Windows.Forms.TabPage
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents pnlgrid As System.Windows.Forms.Panel
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents dtpAsOnDate As BrighttechPack.DatePicker
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmbMetal As BrighttechPack.CheckedComboBox
    Friend WithEvents CmbItemName As BrighttechPack.CheckedComboBox
End Class
