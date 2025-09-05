<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTaggingReport
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
        Me.pnlTop = New System.Windows.Forms.Panel()
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox()
        Me.lblCostCenter = New System.Windows.Forms.Label()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnexit = New System.Windows.Forms.Button()
        Me.btnExcel = New System.Windows.Forms.Button()
        Me.btnview = New System.Windows.Forms.Button()
        Me.btnnew = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtpToDate = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrmDate = New BrighttechPack.DatePicker(Me.components)
        Me.lblEditDate = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.dGrid = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AutoResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.ChkComboBoxMetal = New BrighttechPack.CheckedComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pnlTop.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.dGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlTop
        '
        Me.pnlTop.Controls.Add(Me.Label1)
        Me.pnlTop.Controls.Add(Me.ChkComboBoxMetal)
        Me.pnlTop.Controls.Add(Me.chkCmbCostCentre)
        Me.pnlTop.Controls.Add(Me.lblCostCenter)
        Me.pnlTop.Controls.Add(Me.btnPrint)
        Me.pnlTop.Controls.Add(Me.btnexit)
        Me.pnlTop.Controls.Add(Me.btnExcel)
        Me.pnlTop.Controls.Add(Me.btnview)
        Me.pnlTop.Controls.Add(Me.btnnew)
        Me.pnlTop.Controls.Add(Me.Label2)
        Me.pnlTop.Controls.Add(Me.dtpToDate)
        Me.pnlTop.Controls.Add(Me.dtpFrmDate)
        Me.pnlTop.Controls.Add(Me.lblEditDate)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(922, 80)
        Me.pnlTop.TabIndex = 0
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(580, 10)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(162, 21)
        Me.chkCmbCostCentre.TabIndex = 5
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'lblCostCenter
        '
        Me.lblCostCenter.Location = New System.Drawing.Point(508, 11)
        Me.lblCostCenter.Name = "lblCostCenter"
        Me.lblCostCenter.Size = New System.Drawing.Size(78, 17)
        Me.lblCostCenter.TabIndex = 4
        Me.lblCostCenter.Text = "&Cost Centre"
        Me.lblCostCenter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(362, 38)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(84, 30)
        Me.btnPrint.TabIndex = 9
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnexit
        '
        Me.btnexit.Location = New System.Drawing.Point(452, 39)
        Me.btnexit.Name = "btnexit"
        Me.btnexit.Size = New System.Drawing.Size(84, 30)
        Me.btnexit.TabIndex = 10
        Me.btnexit.Text = "Exit[F12]"
        Me.btnexit.UseVisualStyleBackColor = True
        '
        'btnExcel
        '
        Me.btnExcel.Location = New System.Drawing.Point(266, 38)
        Me.btnExcel.Name = "btnExcel"
        Me.btnExcel.Size = New System.Drawing.Size(84, 30)
        Me.btnExcel.TabIndex = 8
        Me.btnExcel.Text = "Export[x]"
        Me.btnExcel.UseVisualStyleBackColor = True
        '
        'btnview
        '
        Me.btnview.Location = New System.Drawing.Point(74, 39)
        Me.btnview.Name = "btnview"
        Me.btnview.Size = New System.Drawing.Size(84, 30)
        Me.btnview.TabIndex = 6
        Me.btnview.Text = "&View"
        Me.btnview.UseVisualStyleBackColor = True
        '
        'btnnew
        '
        Me.btnnew.Location = New System.Drawing.Point(170, 39)
        Me.btnnew.Name = "btnnew"
        Me.btnnew.Size = New System.Drawing.Size(84, 30)
        Me.btnnew.TabIndex = 7
        Me.btnnew.Text = "New[F3]"
        Me.btnnew.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 11)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Date From"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpToDate
        '
        Me.dtpToDate.Location = New System.Drawing.Point(206, 8)
        Me.dtpToDate.Mask = "##/##/####"
        Me.dtpToDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpToDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpToDate.Name = "dtpToDate"
        Me.dtpToDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpToDate.Size = New System.Drawing.Size(97, 20)
        Me.dtpToDate.TabIndex = 3
        Me.dtpToDate.Text = "07/03/9998"
        Me.dtpToDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrmDate
        '
        Me.dtpFrmDate.Location = New System.Drawing.Point(74, 8)
        Me.dtpFrmDate.Mask = "##/##/####"
        Me.dtpFrmDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrmDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrmDate.Name = "dtpFrmDate"
        Me.dtpFrmDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrmDate.Size = New System.Drawing.Size(100, 20)
        Me.dtpFrmDate.TabIndex = 1
        Me.dtpFrmDate.Text = "07/03/9998"
        Me.dtpFrmDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'lblEditDate
        '
        Me.lblEditDate.AutoSize = True
        Me.lblEditDate.Location = New System.Drawing.Point(180, 11)
        Me.lblEditDate.Name = "lblEditDate"
        Me.lblEditDate.Size = New System.Drawing.Size(20, 13)
        Me.lblEditDate.TabIndex = 2
        Me.lblEditDate.Text = "To"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.dGrid)
        Me.Panel1.Controls.Add(Me.lblTitle)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 80)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(922, 498)
        Me.Panel1.TabIndex = 1
        '
        'dGrid
        '
        Me.dGrid.AllowUserToAddRows = False
        Me.dGrid.AllowUserToDeleteRows = False
        Me.dGrid.AllowUserToResizeColumns = False
        Me.dGrid.AllowUserToResizeRows = False
        Me.dGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dGrid.ContextMenuStrip = Me.ContextMenuStrip1
        Me.dGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dGrid.Location = New System.Drawing.Point(0, 39)
        Me.dGrid.Name = "dGrid"
        Me.dGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dGrid.Size = New System.Drawing.Size(922, 459)
        Me.dGrid.TabIndex = 1
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AutoResizeToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(136, 26)
        '
        'AutoResizeToolStripMenuItem
        '
        Me.AutoResizeToolStripMenuItem.Checked = True
        Me.AutoResizeToolStripMenuItem.CheckOnClick = True
        Me.AutoResizeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.AutoResizeToolStripMenuItem.Name = "AutoResizeToolStripMenuItem"
        Me.AutoResizeToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.AutoResizeToolStripMenuItem.Text = "Auto Resize"
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(922, 39)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ChkComboBoxMetal
        '
        Me.ChkComboBoxMetal.CheckOnClick = True
        Me.ChkComboBoxMetal.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.ChkComboBoxMetal.DropDownHeight = 1
        Me.ChkComboBoxMetal.FormattingEnabled = True
        Me.ChkComboBoxMetal.IntegralHeight = False
        Me.ChkComboBoxMetal.Location = New System.Drawing.Point(362, 8)
        Me.ChkComboBoxMetal.Name = "ChkComboBoxMetal"
        Me.ChkComboBoxMetal.Size = New System.Drawing.Size(129, 21)
        Me.ChkComboBoxMetal.TabIndex = 11
        Me.ChkComboBoxMetal.ValueSeparator = ", "
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(317, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(33, 13)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "&Metal"
        '
        'frmTaggingReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(922, 578)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.pnlTop)
        Me.Name = "frmTaggingReport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Lot Vs Tag Report"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        CType(Me.dGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents dGrid As System.Windows.Forms.DataGridView
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnexit As System.Windows.Forms.Button
    Friend WithEvents btnExcel As System.Windows.Forms.Button
    Friend WithEvents btnview As System.Windows.Forms.Button
    Friend WithEvents btnnew As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtpToDate As BrighttechPack.DatePicker
    Friend WithEvents dtpFrmDate As BrighttechPack.DatePicker
    Friend WithEvents lblEditDate As System.Windows.Forms.Label
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents lblCostCenter As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AutoResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label1 As Label
    Friend WithEvents ChkComboBoxMetal As BrighttechPack.CheckedComboBox
End Class
