<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStockcheckreport
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
        Me.lblStatus = New System.Windows.Forms.Label
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.txt_grid = New System.Windows.Forms.TextBox
        Me.gridviewR = New System.Windows.Forms.DataGridView
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.btnPrint = New System.Windows.Forms.Button
        Me.ChkCmbMetal = New BrighttechPack.CheckedComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.rbttallyonly = New System.Windows.Forms.RadioButton
        Me.rbtDiffonly = New System.Windows.Forms.RadioButton
        Me.rbtboth = New System.Windows.Forms.RadioButton
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.dtpRfrmdate = New BrighttechPack.DatePicker(Me.components)
        Me.Label15 = New System.Windows.Forms.Label
        Me.btnnew = New System.Windows.Forms.Button
        Me.Label13 = New System.Windows.Forms.Label
        Me.btnRexport = New System.Windows.Forms.Button
        Me.dtpRTodate = New BrighttechPack.DatePicker(Me.components)
        Me.btnRexit = New System.Windows.Forms.Button
        Me.btnRview = New System.Windows.Forms.Button
        Me.Label14 = New System.Windows.Forms.Label
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AutoReziseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ContextMenuStrip1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me.gridviewR, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(29, 462)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(128, 13)
        Me.lblStatus.TabIndex = 2
        Me.lblStatus.Text = "Press Enter to Edit"
        Me.lblStatus.Visible = False
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        Me.SaveToolStripMenuItem.Visible = False
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        Me.OpenToolStripMenuItem.Visible = False
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
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem1, Me.ExitToolStripMenuItem1})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 48)
        '
        'NewToolStripMenuItem1
        '
        Me.NewToolStripMenuItem1.Name = "NewToolStripMenuItem1"
        Me.NewToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem1.Size = New System.Drawing.Size(128, 22)
        Me.NewToolStripMenuItem1.Text = "New"
        Me.NewToolStripMenuItem1.Visible = False
        '
        'ExitToolStripMenuItem1
        '
        Me.ExitToolStripMenuItem1.Name = "ExitToolStripMenuItem1"
        Me.ExitToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem1.Size = New System.Drawing.Size(128, 22)
        Me.ExitToolStripMenuItem1.Text = "Exit"
        Me.ExitToolStripMenuItem1.Visible = False
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.txt_grid)
        Me.GroupBox3.Controls.Add(Me.gridviewR)
        Me.GroupBox3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox3.Location = New System.Drawing.Point(0, 88)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(1020, 542)
        Me.GroupBox3.TabIndex = 1
        Me.GroupBox3.TabStop = False
        '
        'txt_grid
        '
        Me.txt_grid.Location = New System.Drawing.Point(319, 146)
        Me.txt_grid.Name = "txt_grid"
        Me.txt_grid.Size = New System.Drawing.Size(100, 21)
        Me.txt_grid.TabIndex = 2
        '
        'gridviewR
        '
        Me.gridviewR.AllowUserToAddRows = False
        Me.gridviewR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridviewR.ContextMenuStrip = Me.ContextMenuStrip2
        Me.gridviewR.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridviewR.Location = New System.Drawing.Point(3, 17)
        Me.gridviewR.Name = "gridviewR"
        Me.gridviewR.ReadOnly = True
        Me.gridviewR.Size = New System.Drawing.Size(1014, 522)
        Me.gridviewR.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.btnPrint)
        Me.Panel2.Controls.Add(Me.ChkCmbMetal)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Controls.Add(Me.rbttallyonly)
        Me.Panel2.Controls.Add(Me.rbtDiffonly)
        Me.Panel2.Controls.Add(Me.rbtboth)
        Me.Panel2.Controls.Add(Me.chkCmbCostCentre)
        Me.Panel2.Controls.Add(Me.dtpRfrmdate)
        Me.Panel2.Controls.Add(Me.Label15)
        Me.Panel2.Controls.Add(Me.btnnew)
        Me.Panel2.Controls.Add(Me.Label13)
        Me.Panel2.Controls.Add(Me.btnRexport)
        Me.Panel2.Controls.Add(Me.dtpRTodate)
        Me.Panel2.Controls.Add(Me.btnRexit)
        Me.Panel2.Controls.Add(Me.btnRview)
        Me.Panel2.Controls.Add(Me.Label14)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1020, 88)
        Me.Panel2.TabIndex = 0
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(708, 51)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 14
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'ChkCmbMetal
        '
        Me.ChkCmbMetal.CheckOnClick = True
        Me.ChkCmbMetal.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.ChkCmbMetal.DropDownHeight = 1
        Me.ChkCmbMetal.FormattingEnabled = True
        Me.ChkCmbMetal.IntegralHeight = False
        Me.ChkCmbMetal.Location = New System.Drawing.Point(123, 35)
        Me.ChkCmbMetal.Name = "ChkCmbMetal"
        Me.ChkCmbMetal.Size = New System.Drawing.Size(263, 22)
        Me.ChkCmbMetal.TabIndex = 5
        Me.ChkCmbMetal.ValueSeparator = ", "
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 38)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "MetalName"
        '
        'rbttallyonly
        '
        Me.rbttallyonly.AutoSize = True
        Me.rbttallyonly.Location = New System.Drawing.Point(468, 29)
        Me.rbttallyonly.Name = "rbttallyonly"
        Me.rbttallyonly.Size = New System.Drawing.Size(118, 17)
        Me.rbttallyonly.TabIndex = 9
        Me.rbttallyonly.Text = "Tally Stock Only"
        Me.rbttallyonly.UseVisualStyleBackColor = True
        '
        'rbtDiffonly
        '
        Me.rbtDiffonly.AutoSize = True
        Me.rbtDiffonly.Location = New System.Drawing.Point(592, 29)
        Me.rbtDiffonly.Name = "rbtDiffonly"
        Me.rbtDiffonly.Size = New System.Drawing.Size(114, 17)
        Me.rbtDiffonly.TabIndex = 10
        Me.rbtDiffonly.Text = "Difference Only"
        Me.rbtDiffonly.UseVisualStyleBackColor = True
        '
        'rbtboth
        '
        Me.rbtboth.AutoSize = True
        Me.rbtboth.Checked = True
        Me.rbtboth.Location = New System.Drawing.Point(411, 28)
        Me.rbtboth.Name = "rbtboth"
        Me.rbtboth.Size = New System.Drawing.Size(51, 17)
        Me.rbtboth.TabIndex = 8
        Me.rbtboth.TabStop = True
        Me.rbtboth.Text = "Both"
        Me.rbtboth.UseVisualStyleBackColor = True
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(123, 60)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(263, 22)
        Me.chkCmbCostCentre.TabIndex = 7
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'dtpRfrmdate
        '
        Me.dtpRfrmdate.Location = New System.Drawing.Point(123, 11)
        Me.dtpRfrmdate.Mask = "##-##-####"
        Me.dtpRfrmdate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpRfrmdate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpRfrmdate.Name = "dtpRfrmdate"
        Me.dtpRfrmdate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpRfrmdate.Size = New System.Drawing.Size(78, 21)
        Me.dtpRfrmdate.TabIndex = 1
        Me.dtpRfrmdate.Text = "29/09/2010"
        Me.dtpRfrmdate.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(21, 14)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(67, 13)
        Me.Label15.TabIndex = 0
        Me.Label15.Text = "From Date"
        '
        'btnnew
        '
        Me.btnnew.Location = New System.Drawing.Point(508, 51)
        Me.btnnew.Name = "btnnew"
        Me.btnnew.Size = New System.Drawing.Size(100, 30)
        Me.btnnew.TabIndex = 12
        Me.btnnew.Text = "New [F3]"
        Me.btnnew.UseVisualStyleBackColor = True
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(207, 14)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(21, 13)
        Me.Label13.TabIndex = 2
        Me.Label13.Text = "To"
        '
        'btnRexport
        '
        Me.btnRexport.Location = New System.Drawing.Point(608, 51)
        Me.btnRexport.Name = "btnRexport"
        Me.btnRexport.Size = New System.Drawing.Size(100, 30)
        Me.btnRexport.TabIndex = 13
        Me.btnRexport.Text = "Export [X]"
        Me.btnRexport.UseVisualStyleBackColor = True
        '
        'dtpRTodate
        '
        Me.dtpRTodate.Location = New System.Drawing.Point(234, 11)
        Me.dtpRTodate.Mask = "##-##-####"
        Me.dtpRTodate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpRTodate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpRTodate.Name = "dtpRTodate"
        Me.dtpRTodate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpRTodate.Size = New System.Drawing.Size(78, 21)
        Me.dtpRTodate.TabIndex = 3
        Me.dtpRTodate.Text = "29/09/2010"
        Me.dtpRTodate.Value = New Date(2010, 9, 29, 0, 0, 0, 0)
        '
        'btnRexit
        '
        Me.btnRexit.Location = New System.Drawing.Point(808, 51)
        Me.btnRexit.Name = "btnRexit"
        Me.btnRexit.Size = New System.Drawing.Size(100, 30)
        Me.btnRexit.TabIndex = 15
        Me.btnRexit.Text = "Exit [F12]"
        Me.btnRexit.UseVisualStyleBackColor = True
        '
        'btnRview
        '
        Me.btnRview.Location = New System.Drawing.Point(408, 51)
        Me.btnRview.Name = "btnRview"
        Me.btnRview.Size = New System.Drawing.Size(100, 30)
        Me.btnRview.TabIndex = 11
        Me.btnRview.Text = "&View"
        Me.btnRview.UseVisualStyleBackColor = True
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(20, 64)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(102, 13)
        Me.Label14.TabIndex = 6
        Me.Label14.Text = "From Costcentre"
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AutoReziseToolStripMenuItem})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(153, 48)
        '
        'AutoReziseToolStripMenuItem
        '
        Me.AutoReziseToolStripMenuItem.Name = "AutoReziseToolStripMenuItem"
        Me.AutoReziseToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.AutoReziseToolStripMenuItem.Text = "AutoRezise"
        '
        'frmStockcheckreport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1020, 630)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.lblStatus)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmStockcheckreport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "StockChecking Report"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.gridviewR, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ContextMenuStrip2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents gridviewR As System.Windows.Forms.DataGridView
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents btnRexport As System.Windows.Forms.Button
    Friend WithEvents btnRexit As System.Windows.Forms.Button
    Friend WithEvents btnRview As System.Windows.Forms.Button
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents dtpRfrmdate As BrighttechPack.DatePicker
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents dtpRTodate As BrighttechPack.DatePicker
    Friend WithEvents btnnew As System.Windows.Forms.Button
    Friend WithEvents txt_grid As System.Windows.Forms.TextBox
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents rbttallyonly As System.Windows.Forms.RadioButton
    Friend WithEvents rbtDiffonly As System.Windows.Forms.RadioButton
    Friend WithEvents rbtboth As System.Windows.Forms.RadioButton
    Friend WithEvents NewToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChkCmbMetal As BrighttechPack.CheckedComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AutoReziseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
