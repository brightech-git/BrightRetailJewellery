<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGiftVoucherCheckLoading
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
        Me.pnlMain = New System.Windows.Forms.Panel
        Me.pnlgrid = New System.Windows.Forms.Panel
        Me.gridFullView = New System.Windows.Forms.DataGridView
        Me.pnlTop = New System.Windows.Forms.Panel
        Me.chkBoth = New System.Windows.Forms.CheckBox
        Me.chkUnMarked = New System.Windows.Forms.CheckBox
        Me.chkMarked = New System.Windows.Forms.CheckBox
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.cmbCostCentre_MAN = New System.Windows.Forms.ComboBox
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.Label7 = New System.Windows.Forms.Label
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtRunno = New System.Windows.Forms.TextBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Label14 = New System.Windows.Forms.Label
        Me.CmbCompany = New System.Windows.Forms.ComboBox
        Me.pnlMain.SuspendLayout()
        Me.pnlgrid.SuspendLayout()
        CType(Me.gridFullView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlTop.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlMain
        '
        Me.pnlMain.Controls.Add(Me.pnlgrid)
        Me.pnlMain.Controls.Add(Me.pnlTop)
        Me.pnlMain.Controls.Add(Me.Panel2)
        Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlMain.Location = New System.Drawing.Point(0, 0)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(929, 526)
        Me.pnlMain.TabIndex = 0
        '
        'pnlgrid
        '
        Me.pnlgrid.Controls.Add(Me.gridFullView)
        Me.pnlgrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlgrid.Location = New System.Drawing.Point(0, 131)
        Me.pnlgrid.Name = "pnlgrid"
        Me.pnlgrid.Size = New System.Drawing.Size(929, 385)
        Me.pnlgrid.TabIndex = 1
        '
        'gridFullView
        '
        Me.gridFullView.AllowUserToAddRows = False
        Me.gridFullView.AllowUserToDeleteRows = False
        Me.gridFullView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridFullView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.gridFullView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridFullView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridFullView.Location = New System.Drawing.Point(0, 0)
        Me.gridFullView.MultiSelect = False
        Me.gridFullView.Name = "gridFullView"
        Me.gridFullView.ReadOnly = True
        Me.gridFullView.RowHeadersVisible = False
        Me.gridFullView.RowTemplate.Height = 17
        Me.gridFullView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridFullView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridFullView.Size = New System.Drawing.Size(929, 385)
        Me.gridFullView.TabIndex = 0
        '
        'pnlTop
        '
        Me.pnlTop.Controls.Add(Me.Label14)
        Me.pnlTop.Controls.Add(Me.CmbCompany)
        Me.pnlTop.Controls.Add(Me.chkBoth)
        Me.pnlTop.Controls.Add(Me.chkUnMarked)
        Me.pnlTop.Controls.Add(Me.chkMarked)
        Me.pnlTop.Controls.Add(Me.btnPrint)
        Me.pnlTop.Controls.Add(Me.btnExport)
        Me.pnlTop.Controls.Add(Me.cmbCostCentre_MAN)
        Me.pnlTop.Controls.Add(Me.btnNew)
        Me.pnlTop.Controls.Add(Me.btnExit)
        Me.pnlTop.Controls.Add(Me.Label7)
        Me.pnlTop.Controls.Add(Me.btnView_Search)
        Me.pnlTop.Controls.Add(Me.Label6)
        Me.pnlTop.Controls.Add(Me.txtRunno)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(929, 131)
        Me.pnlTop.TabIndex = 0
        '
        'chkBoth
        '
        Me.chkBoth.AutoSize = True
        Me.chkBoth.Location = New System.Drawing.Point(727, 64)
        Me.chkBoth.Name = "chkBoth"
        Me.chkBoth.Size = New System.Drawing.Size(52, 17)
        Me.chkBoth.TabIndex = 8
        Me.chkBoth.Text = "Both"
        Me.chkBoth.UseVisualStyleBackColor = True
        '
        'chkUnMarked
        '
        Me.chkUnMarked.AutoSize = True
        Me.chkUnMarked.Location = New System.Drawing.Point(646, 65)
        Me.chkUnMarked.Name = "chkUnMarked"
        Me.chkUnMarked.Size = New System.Drawing.Size(83, 17)
        Me.chkUnMarked.TabIndex = 7
        Me.chkUnMarked.Text = "UnMarked"
        Me.chkUnMarked.UseVisualStyleBackColor = True
        '
        'chkMarked
        '
        Me.chkMarked.AutoSize = True
        Me.chkMarked.Location = New System.Drawing.Point(578, 64)
        Me.chkMarked.Name = "chkMarked"
        Me.chkMarked.Size = New System.Drawing.Size(68, 17)
        Me.chkMarked.TabIndex = 6
        Me.chkMarked.Text = "Marked"
        Me.chkMarked.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(518, 89)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(91, 30)
        Me.btnPrint.TabIndex = 12
        Me.btnPrint.Text = "Print [P]"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(429, 89)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(90, 30)
        Me.btnExport.TabIndex = 11
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'cmbCostCentre_MAN
        '
        Me.cmbCostCentre_MAN.FormattingEnabled = True
        Me.cmbCostCentre_MAN.Location = New System.Drawing.Point(377, 35)
        Me.cmbCostCentre_MAN.Name = "cmbCostCentre_MAN"
        Me.cmbCostCentre_MAN.Size = New System.Drawing.Size(192, 21)
        Me.cmbCostCentre_MAN.TabIndex = 3
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(335, 90)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(94, 30)
        Me.btnNew.TabIndex = 10
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(609, 89)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 13
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(305, 37)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(70, 13)
        Me.Label7.TabIndex = 2
        Me.Label7.Text = "Cost Name"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(246, 90)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(90, 30)
        Me.btnView_Search.TabIndex = 9
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(306, 62)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(48, 13)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Run No"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtRunno
        '
        Me.txtRunno.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtRunno.Location = New System.Drawing.Point(377, 61)
        Me.txtRunno.MaxLength = 15
        Me.txtRunno.Name = "txtRunno"
        Me.txtRunno.Size = New System.Drawing.Size(190, 21)
        Me.txtRunno.TabIndex = 5
        '
        'Panel2
        '
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 516)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(929, 10)
        Me.Panel2.TabIndex = 0
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
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(305, 11)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(62, 13)
        Me.Label14.TabIndex = 0
        Me.Label14.Text = "Company"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CmbCompany
        '
        Me.CmbCompany.FormattingEnabled = True
        Me.CmbCompany.Location = New System.Drawing.Point(377, 7)
        Me.CmbCompany.Name = "CmbCompany"
        Me.CmbCompany.Size = New System.Drawing.Size(190, 21)
        Me.CmbCompany.TabIndex = 1
        '
        'frmGiftVoucherCheckLoading
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(929, 526)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlMain)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmGiftVoucherCheckLoading"
        Me.Text = "StockCheckLoading"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlMain.ResumeLayout(False)
        Me.pnlgrid.ResumeLayout(False)
        CType(Me.gridFullView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlMain As System.Windows.Forms.Panel
    Friend WithEvents txtRunno As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents pnlgrid As System.Windows.Forms.Panel
    Friend WithEvents gridFullView As System.Windows.Forms.DataGridView
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbCostCentre_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents chkBoth As System.Windows.Forms.CheckBox
    Friend WithEvents chkUnMarked As System.Windows.Forms.CheckBox
    Friend WithEvents chkMarked As System.Windows.Forms.CheckBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents CmbCompany As System.Windows.Forms.ComboBox
End Class
