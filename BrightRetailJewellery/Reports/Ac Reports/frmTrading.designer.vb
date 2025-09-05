<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTrading
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
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnView = New System.Windows.Forms.Button()
        Me.dtpDate = New GiriDatePicker.DatePicker(Me.components)
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.dtpToDate = New GiriDatePicker.DatePicker(Me.components)
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chkAsOn = New System.Windows.Forms.CheckBox()
        Me.pnlClosingStock = New System.Windows.Forms.Panel()
        Me.rbtCls_Stock_LIFO = New System.Windows.Forms.RadioButton()
        Me.rbtCls_Stock_Man = New System.Windows.Forms.RadioButton()
        Me.rbtCls_Stock_AvgRate = New System.Windows.Forms.RadioButton()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.chkTrading = New System.Windows.Forms.CheckBox()
        Me.ChkPandL = New System.Windows.Forms.CheckBox()
        Me.chkSummary = New System.Windows.Forms.CheckBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbCompany = New System.Windows.Forms.ComboBox()
        Me.contextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.viewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.newToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.exitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.gridViewHead = New System.Windows.Forms.DataGridView()
        Me.pnlGridhead = New System.Windows.Forms.Panel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.pnlClosingStock.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.contextMenuStrip1.SuspendLayout()
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlGridhead.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.AllowUserToResizeRows = False
        Me.gridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 0)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.gridView.Size = New System.Drawing.Size(891, 409)
        Me.gridView.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 112)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(891, 40)
        Me.lblTitle.TabIndex = 1
        Me.lblTitle.Text = "Label1"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(305, 76)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(96, 31)
        Me.btnNew.TabIndex = 12
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(509, 76)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(96, 31)
        Me.btnPrint.TabIndex = 14
        Me.btnPrint.Text = "Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(611, 75)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(96, 31)
        Me.btnExit.TabIndex = 15
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(407, 76)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(96, 31)
        Me.btnExport.TabIndex = 13
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnView
        '
        Me.btnView.Location = New System.Drawing.Point(203, 75)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(96, 31)
        Me.btnView.TabIndex = 11
        Me.btnView.Text = "View [F2]"
        Me.btnView.UseVisualStyleBackColor = True
        '
        'dtpDate
        '
        Me.dtpDate.Location = New System.Drawing.Point(75, 48)
        Me.dtpDate.Mask = "##-##-####"
        Me.dtpDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Seperator = Global.Microsoft.VisualBasic.ChrW(45)
        Me.dtpDate.Size = New System.Drawing.Size(88, 21)
        Me.dtpDate.TabIndex = 5
        Me.dtpDate.Text = "01-03-9998"
        Me.dtpDate.Value = New Date(9998, 3, 1, 0, 0, 0, 0)
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.dtpToDate)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.chkAsOn)
        Me.Panel1.Controls.Add(Me.pnlClosingStock)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.chkSummary)
        Me.Panel1.Controls.Add(Me.chkCmbCostCentre)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.cmbCompany)
        Me.Panel1.Controls.Add(Me.dtpDate)
        Me.Panel1.Controls.Add(Me.btnView)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(891, 112)
        Me.Panel1.TabIndex = 0
        '
        'dtpToDate
        '
        Me.dtpToDate.Location = New System.Drawing.Point(203, 48)
        Me.dtpToDate.Mask = "##-##-####"
        Me.dtpToDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpToDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpToDate.Name = "dtpToDate"
        Me.dtpToDate.Seperator = Global.Microsoft.VisualBasic.ChrW(45)
        Me.dtpToDate.Size = New System.Drawing.Size(88, 21)
        Me.dtpToDate.TabIndex = 7
        Me.dtpToDate.Text = "01-03-9998"
        Me.dtpToDate.Value = New Date(9998, 3, 1, 0, 0, 0, 0)
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(169, 52)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(20, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "To"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkAsOn
        '
        Me.chkAsOn.AutoSize = True
        Me.chkAsOn.Location = New System.Drawing.Point(8, 50)
        Me.chkAsOn.Name = "chkAsOn"
        Me.chkAsOn.Size = New System.Drawing.Size(60, 17)
        Me.chkAsOn.TabIndex = 4
        Me.chkAsOn.Text = "As On"
        Me.chkAsOn.UseVisualStyleBackColor = True
        '
        'pnlClosingStock
        '
        Me.pnlClosingStock.Controls.Add(Me.rbtCls_Stock_LIFO)
        Me.pnlClosingStock.Controls.Add(Me.rbtCls_Stock_Man)
        Me.pnlClosingStock.Controls.Add(Me.rbtCls_Stock_AvgRate)
        Me.pnlClosingStock.Location = New System.Drawing.Point(305, 45)
        Me.pnlClosingStock.Name = "pnlClosingStock"
        Me.pnlClosingStock.Size = New System.Drawing.Size(198, 27)
        Me.pnlClosingStock.TabIndex = 8
        '
        'rbtCls_Stock_LIFO
        '
        Me.rbtCls_Stock_LIFO.AutoSize = True
        Me.rbtCls_Stock_LIFO.Location = New System.Drawing.Point(204, 6)
        Me.rbtCls_Stock_LIFO.Name = "rbtCls_Stock_LIFO"
        Me.rbtCls_Stock_LIFO.Size = New System.Drawing.Size(51, 17)
        Me.rbtCls_Stock_LIFO.TabIndex = 15
        Me.rbtCls_Stock_LIFO.TabStop = True
        Me.rbtCls_Stock_LIFO.Text = "LIFO"
        Me.rbtCls_Stock_LIFO.UseVisualStyleBackColor = True
        Me.rbtCls_Stock_LIFO.Visible = False
        '
        'rbtCls_Stock_Man
        '
        Me.rbtCls_Stock_Man.AutoSize = True
        Me.rbtCls_Stock_Man.Location = New System.Drawing.Point(113, 6)
        Me.rbtCls_Stock_Man.Name = "rbtCls_Stock_Man"
        Me.rbtCls_Stock_Man.Size = New System.Drawing.Size(65, 17)
        Me.rbtCls_Stock_Man.TabIndex = 1
        Me.rbtCls_Stock_Man.TabStop = True
        Me.rbtCls_Stock_Man.Text = "Manual"
        Me.rbtCls_Stock_Man.UseVisualStyleBackColor = True
        '
        'rbtCls_Stock_AvgRate
        '
        Me.rbtCls_Stock_AvgRate.AutoSize = True
        Me.rbtCls_Stock_AvgRate.Location = New System.Drawing.Point(13, 6)
        Me.rbtCls_Stock_AvgRate.Name = "rbtCls_Stock_AvgRate"
        Me.rbtCls_Stock_AvgRate.Size = New System.Drawing.Size(80, 17)
        Me.rbtCls_Stock_AvgRate.TabIndex = 0
        Me.rbtCls_Stock_AvgRate.TabStop = True
        Me.rbtCls_Stock_AvgRate.Text = "AVG Rate"
        Me.rbtCls_Stock_AvgRate.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.chkTrading)
        Me.Panel2.Controls.Add(Me.ChkPandL)
        Me.Panel2.Location = New System.Drawing.Point(509, 45)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(242, 27)
        Me.Panel2.TabIndex = 9
        '
        'chkTrading
        '
        Me.chkTrading.AutoSize = True
        Me.chkTrading.Checked = True
        Me.chkTrading.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkTrading.Location = New System.Drawing.Point(10, 6)
        Me.chkTrading.Name = "chkTrading"
        Me.chkTrading.Size = New System.Drawing.Size(68, 17)
        Me.chkTrading.TabIndex = 0
        Me.chkTrading.Text = "Trading"
        Me.chkTrading.UseVisualStyleBackColor = True
        '
        'ChkPandL
        '
        Me.ChkPandL.AutoSize = True
        Me.ChkPandL.Checked = True
        Me.ChkPandL.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkPandL.Location = New System.Drawing.Point(112, 6)
        Me.ChkPandL.Name = "ChkPandL"
        Me.ChkPandL.Size = New System.Drawing.Size(110, 17)
        Me.ChkPandL.TabIndex = 1
        Me.ChkPandL.Text = "Profit and Loss"
        Me.ChkPandL.UseVisualStyleBackColor = True
        '
        'chkSummary
        '
        Me.chkSummary.AutoSize = True
        Me.chkSummary.Location = New System.Drawing.Point(75, 76)
        Me.chkSummary.Name = "chkSummary"
        Me.chkSummary.Size = New System.Drawing.Size(82, 17)
        Me.chkSummary.TabIndex = 10
        Me.chkSummary.Text = "Summary"
        Me.chkSummary.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(349, 14)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(62, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Company"
        '
        'cmbCompany
        '
        Me.cmbCompany.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCompany.FormattingEnabled = True
        Me.cmbCompany.Location = New System.Drawing.Point(75, 11)
        Me.cmbCompany.Name = "cmbCompany"
        Me.cmbCompany.Size = New System.Drawing.Size(260, 21)
        Me.cmbCompany.TabIndex = 1
        '
        'contextMenuStrip1
        '
        Me.contextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.viewToolStripMenuItem, Me.newToolStripMenuItem, Me.exitToolStripMenuItem})
        Me.contextMenuStrip1.Name = "contextMenuStrip1"
        Me.contextMenuStrip1.Size = New System.Drawing.Size(119, 70)
        '
        'viewToolStripMenuItem
        '
        Me.viewToolStripMenuItem.Name = "viewToolStripMenuItem"
        Me.viewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.viewToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.viewToolStripMenuItem.Text = "View"
        Me.viewToolStripMenuItem.Visible = False
        '
        'newToolStripMenuItem
        '
        Me.newToolStripMenuItem.Name = "newToolStripMenuItem"
        Me.newToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.newToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.newToolStripMenuItem.Text = "New"
        Me.newToolStripMenuItem.Visible = False
        '
        'exitToolStripMenuItem
        '
        Me.exitToolStripMenuItem.Name = "exitToolStripMenuItem"
        Me.exitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F10
        Me.exitToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.exitToolStripMenuItem.Text = "Exit"
        Me.exitToolStripMenuItem.Visible = False
        '
        'gridViewHead
        '
        Me.gridViewHead.AllowUserToAddRows = False
        Me.gridViewHead.AllowUserToDeleteRows = False
        Me.gridViewHead.AllowUserToResizeColumns = False
        Me.gridViewHead.AllowUserToResizeRows = False
        Me.gridViewHead.BackgroundColor = System.Drawing.SystemColors.ControlDark
        Me.gridViewHead.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridViewHead.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.gridViewHead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridViewHead.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridViewHead.Location = New System.Drawing.Point(0, 0)
        Me.gridViewHead.Name = "gridViewHead"
        Me.gridViewHead.ReadOnly = True
        Me.gridViewHead.RowHeadersVisible = False
        Me.gridViewHead.Size = New System.Drawing.Size(891, 20)
        Me.gridViewHead.TabIndex = 0
        '
        'pnlGridhead
        '
        Me.pnlGridhead.Controls.Add(Me.gridViewHead)
        Me.pnlGridhead.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlGridhead.Location = New System.Drawing.Point(0, 152)
        Me.pnlGridhead.Name = "pnlGridhead"
        Me.pnlGridhead.Size = New System.Drawing.Size(891, 20)
        Me.pnlGridhead.TabIndex = 4
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.gridView)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(0, 172)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(891, 409)
        Me.Panel3.TabIndex = 5
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(428, 11)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(276, 22)
        Me.chkCmbCostCentre.TabIndex = 3
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'frmTrading
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(891, 581)
        Me.ContextMenuStrip = Me.contextMenuStrip1
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.pnlGridhead)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name = "frmTrading"
        Me.Text = "Trading & Profit and Loss"
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlClosingStock.ResumeLayout(False)
        Me.pnlClosingStock.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.contextMenuStrip1.ResumeLayout(False)
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlGridhead.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnView As System.Windows.Forms.Button
    Friend WithEvents dtpDate As GiriDatePicker.DatePicker
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbCompany As System.Windows.Forms.ComboBox
    Private WithEvents contextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Private WithEvents viewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents newToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents exitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkSummary As System.Windows.Forms.CheckBox
    Friend WithEvents gridViewHead As System.Windows.Forms.DataGridView
    Friend WithEvents pnlGridhead As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ChkPandL As System.Windows.Forms.CheckBox
    Friend WithEvents chkTrading As System.Windows.Forms.CheckBox
    Friend WithEvents pnlClosingStock As Panel
    Friend WithEvents rbtCls_Stock_AvgRate As RadioButton
    Friend WithEvents rbtCls_Stock_Man As RadioButton
    Friend WithEvents rbtCls_Stock_LIFO As RadioButton
    Friend WithEvents dtpToDate As GiriDatePicker.DatePicker
    Friend WithEvents Label2 As Label
    Friend WithEvents chkAsOn As CheckBox
End Class
