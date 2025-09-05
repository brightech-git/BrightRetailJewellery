<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCounterWiseSalesReport
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
        Me.pnlTop = New System.Windows.Forms.Panel
        Me.chkCmbCompany = New BrighttechPack.CheckedComboBox
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.dtpToDate = New BrighttechPack.DatePicker(Me.components)
        Me.Label9 = New System.Windows.Forms.Label
        Me.lblEditDate = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmbCounterName = New System.Windows.Forms.ComboBox
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.lblCostCenter = New System.Windows.Forms.Label
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnexit = New System.Windows.Forms.Button
        Me.btnExcel = New System.Windows.Forms.Button
        Me.btnnew = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.dtpFrmDate = New BrighttechPack.DatePicker(Me.components)
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.dGrid = New System.Windows.Forms.DataGridView
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AutoResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.lblTitle = New System.Windows.Forms.Label
        Me.pnlTop.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.dGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlTop
        '
        Me.pnlTop.Controls.Add(Me.chkCmbCompany)
        Me.pnlTop.Controls.Add(Me.btnView_Search)
        Me.pnlTop.Controls.Add(Me.dtpToDate)
        Me.pnlTop.Controls.Add(Me.Label9)
        Me.pnlTop.Controls.Add(Me.lblEditDate)
        Me.pnlTop.Controls.Add(Me.Label1)
        Me.pnlTop.Controls.Add(Me.cmbCounterName)
        Me.pnlTop.Controls.Add(Me.chkCmbCostCentre)
        Me.pnlTop.Controls.Add(Me.lblCostCenter)
        Me.pnlTop.Controls.Add(Me.btnPrint)
        Me.pnlTop.Controls.Add(Me.btnexit)
        Me.pnlTop.Controls.Add(Me.btnExcel)
        Me.pnlTop.Controls.Add(Me.btnnew)
        Me.pnlTop.Controls.Add(Me.Label2)
        Me.pnlTop.Controls.Add(Me.dtpFrmDate)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(927, 113)
        Me.pnlTop.TabIndex = 0
        '
        'chkCmbCompany
        '
        Me.chkCmbCompany.CheckOnClick = True
        Me.chkCmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCompany.DropDownHeight = 1
        Me.chkCmbCompany.FormattingEnabled = True
        Me.chkCmbCompany.IntegralHeight = False
        Me.chkCmbCompany.Location = New System.Drawing.Point(367, 37)
        Me.chkCmbCompany.Name = "chkCmbCompany"
        Me.chkCmbCompany.Size = New System.Drawing.Size(239, 21)
        Me.chkCmbCompany.TabIndex = 9
        Me.chkCmbCompany.ValueSeparator = ", "
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(84, 72)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 10
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'dtpToDate
        '
        Me.dtpToDate.Location = New System.Drawing.Point(196, 7)
        Me.dtpToDate.Mask = "##/##/####"
        Me.dtpToDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpToDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpToDate.Name = "dtpToDate"
        Me.dtpToDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpToDate.Size = New System.Drawing.Size(84, 20)
        Me.dtpToDate.TabIndex = 3
        Me.dtpToDate.Text = "07/03/9998"
        Me.dtpToDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(286, 45)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(51, 13)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "Company"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblEditDate
        '
        Me.lblEditDate.AutoSize = True
        Me.lblEditDate.Location = New System.Drawing.Point(172, 11)
        Me.lblEditDate.Name = "lblEditDate"
        Me.lblEditDate.Size = New System.Drawing.Size(20, 13)
        Me.lblEditDate.TabIndex = 2
        Me.lblEditDate.Text = "To"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 45)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Counter Name"
        '
        'cmbCounterName
        '
        Me.cmbCounterName.FormattingEnabled = True
        Me.cmbCounterName.Location = New System.Drawing.Point(84, 37)
        Me.cmbCounterName.Name = "cmbCounterName"
        Me.cmbCounterName.Size = New System.Drawing.Size(196, 21)
        Me.cmbCounterName.TabIndex = 7
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(367, 6)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(239, 21)
        Me.chkCmbCostCentre.TabIndex = 5
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'lblCostCenter
        '
        Me.lblCostCenter.Location = New System.Drawing.Point(286, 8)
        Me.lblCostCenter.Name = "lblCostCenter"
        Me.lblCostCenter.Size = New System.Drawing.Size(78, 17)
        Me.lblCostCenter.TabIndex = 4
        Me.lblCostCenter.Text = "&Cost Centre"
        Me.lblCostCenter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(370, 73)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(84, 30)
        Me.btnPrint.TabIndex = 13
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnexit
        '
        Me.btnexit.Location = New System.Drawing.Point(460, 73)
        Me.btnexit.Name = "btnexit"
        Me.btnexit.Size = New System.Drawing.Size(84, 30)
        Me.btnexit.TabIndex = 14
        Me.btnexit.Text = "Exit[F12]"
        Me.btnexit.UseVisualStyleBackColor = True
        '
        'btnExcel
        '
        Me.btnExcel.Location = New System.Drawing.Point(280, 73)
        Me.btnExcel.Name = "btnExcel"
        Me.btnExcel.Size = New System.Drawing.Size(84, 30)
        Me.btnExcel.TabIndex = 12
        Me.btnExcel.Text = "Export[x]"
        Me.btnExcel.UseVisualStyleBackColor = True
        '
        'btnnew
        '
        Me.btnnew.Location = New System.Drawing.Point(190, 73)
        Me.btnnew.Name = "btnnew"
        Me.btnnew.Size = New System.Drawing.Size(84, 30)
        Me.btnnew.TabIndex = 11
        Me.btnnew.Text = "New[F3]"
        Me.btnnew.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(7, 12)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Date From"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpFrmDate
        '
        Me.dtpFrmDate.Location = New System.Drawing.Point(84, 7)
        Me.dtpFrmDate.Mask = "##/##/####"
        Me.dtpFrmDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrmDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrmDate.Name = "dtpFrmDate"
        Me.dtpFrmDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrmDate.Size = New System.Drawing.Size(84, 20)
        Me.dtpFrmDate.TabIndex = 1
        Me.dtpFrmDate.Text = "07/03/9998"
        Me.dtpFrmDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.dGrid)
        Me.Panel1.Controls.Add(Me.lblTitle)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 113)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(927, 447)
        Me.Panel1.TabIndex = 2
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
        Me.dGrid.Size = New System.Drawing.Size(927, 408)
        Me.dGrid.TabIndex = 1
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AutoResizeToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(143, 26)
        '
        'AutoResizeToolStripMenuItem
        '
        Me.AutoResizeToolStripMenuItem.Checked = True
        Me.AutoResizeToolStripMenuItem.CheckOnClick = True
        Me.AutoResizeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.AutoResizeToolStripMenuItem.Name = "AutoResizeToolStripMenuItem"
        Me.AutoResizeToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.AutoResizeToolStripMenuItem.Text = "Auto Resize"
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(927, 39)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmCounterWiseSalesReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(927, 560)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.pnlTop)
        Me.Name = "frmCounterWiseSalesReport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Counterwise Sales Summery"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        CType(Me.dGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbCounterName As System.Windows.Forms.ComboBox
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents lblCostCenter As System.Windows.Forms.Label
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnexit As System.Windows.Forms.Button
    Friend WithEvents btnExcel As System.Windows.Forms.Button
    Friend WithEvents btnnew As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtpFrmDate As BrighttechPack.DatePicker
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents dGrid As System.Windows.Forms.DataGridView
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents dtpToDate As BrighttechPack.DatePicker
    Friend WithEvents lblEditDate As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AutoResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents chkCmbCompany As BrighttechPack.CheckedComboBox
End Class
