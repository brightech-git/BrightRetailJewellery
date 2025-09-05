<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBalanceSheet_Format1
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbCompany = New System.Windows.Forms.ComboBox()
        Me.dtpDate = New GiriDatePicker.DatePicker(Me.components)
        Me.btnView = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.gridView = New System.Windows.Forms.DataGridView()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.contextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.viewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.newToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.exitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlGirdhead = New System.Windows.Forms.Panel()
        Me.gridViewHead = New System.Windows.Forms.DataGridView()
        Me.pnlGrid = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.rbtSummary = New System.Windows.Forms.RadioButton()
        Me.rbtDetailed = New System.Windows.Forms.RadioButton()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.rbtReport = New System.Windows.Forms.RadioButton()
        Me.rbtAccount = New System.Windows.Forms.RadioButton()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.contextMenuStrip1.SuspendLayout()
        Me.pnlGirdhead.SuspendLayout()
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlGrid.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Controls.Add(Me.Panel2)
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
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1086, 83)
        Me.Panel1.TabIndex = 0
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(417, 11)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(276, 22)
        Me.chkCmbCostCentre.TabIndex = 3
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(340, 14)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 14)
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
        'dtpDate
        '
        Me.dtpDate.Location = New System.Drawing.Point(75, 45)
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
        'btnView
        '
        Me.btnView.Location = New System.Drawing.Point(191, 40)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(96, 31)
        Me.btnView.TabIndex = 6
        Me.btnView.Text = "View [F2]"
        Me.btnView.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(394, 40)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(96, 31)
        Me.btnExport.TabIndex = 8
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(495, 40)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(96, 31)
        Me.btnExit.TabIndex = 9
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(597, 40)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(96, 31)
        Me.btnPrint.TabIndex = 10
        Me.btnPrint.Text = "Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(292, 40)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(96, 31)
        Me.btnNew.TabIndex = 7
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 49)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Date"
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.AllowUserToResizeRows = False
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 0)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.Size = New System.Drawing.Size(1086, 442)
        Me.gridView.TabIndex = 2
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 83)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1086, 36)
        Me.lblTitle.TabIndex = 1
        Me.lblTitle.Text = "Label1"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
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
        'pnlGirdhead
        '
        Me.pnlGirdhead.Controls.Add(Me.gridViewHead)
        Me.pnlGirdhead.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlGirdhead.Location = New System.Drawing.Point(0, 119)
        Me.pnlGirdhead.Name = "pnlGirdhead"
        Me.pnlGirdhead.Size = New System.Drawing.Size(1086, 20)
        Me.pnlGirdhead.TabIndex = 3
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
        Me.gridViewHead.Size = New System.Drawing.Size(1086, 20)
        Me.gridViewHead.TabIndex = 4
        '
        'pnlGrid
        '
        Me.pnlGrid.Controls.Add(Me.gridView)
        Me.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlGrid.Location = New System.Drawing.Point(0, 139)
        Me.pnlGrid.Name = "pnlGrid"
        Me.pnlGrid.Size = New System.Drawing.Size(1086, 442)
        Me.pnlGrid.TabIndex = 4
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rbtDetailed)
        Me.Panel2.Controls.Add(Me.rbtSummary)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Location = New System.Drawing.Point(698, 7)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(255, 29)
        Me.Panel2.TabIndex = 11
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 8)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(34, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Type"
        '
        'rbtSummary
        '
        Me.rbtSummary.AutoSize = True
        Me.rbtSummary.Checked = True
        Me.rbtSummary.Location = New System.Drawing.Point(63, 7)
        Me.rbtSummary.Name = "rbtSummary"
        Me.rbtSummary.Size = New System.Drawing.Size(81, 17)
        Me.rbtSummary.TabIndex = 1
        Me.rbtSummary.TabStop = True
        Me.rbtSummary.Text = "Summary"
        Me.rbtSummary.UseVisualStyleBackColor = True
        '
        'rbtDetailed
        '
        Me.rbtDetailed.AutoSize = True
        Me.rbtDetailed.Location = New System.Drawing.Point(154, 6)
        Me.rbtDetailed.Name = "rbtDetailed"
        Me.rbtDetailed.Size = New System.Drawing.Size(72, 17)
        Me.rbtDetailed.TabIndex = 2
        Me.rbtDetailed.Text = "Detailed"
        Me.rbtDetailed.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.rbtReport)
        Me.Panel3.Controls.Add(Me.rbtAccount)
        Me.Panel3.Controls.Add(Me.Label5)
        Me.Panel3.Location = New System.Drawing.Point(698, 42)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(255, 29)
        Me.Panel3.TabIndex = 12
        '
        'rbtReport
        '
        Me.rbtReport.AutoSize = True
        Me.rbtReport.Location = New System.Drawing.Point(154, 6)
        Me.rbtReport.Name = "rbtReport"
        Me.rbtReport.Size = New System.Drawing.Size(63, 17)
        Me.rbtReport.TabIndex = 2
        Me.rbtReport.Text = "Report"
        Me.rbtReport.UseVisualStyleBackColor = True
        '
        'rbtAccount
        '
        Me.rbtAccount.AutoSize = True
        Me.rbtAccount.Checked = True
        Me.rbtAccount.Location = New System.Drawing.Point(63, 7)
        Me.rbtAccount.Name = "rbtAccount"
        Me.rbtAccount.Size = New System.Drawing.Size(70, 17)
        Me.rbtAccount.TabIndex = 1
        Me.rbtAccount.TabStop = True
        Me.rbtAccount.Text = "Account"
        Me.rbtAccount.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 8)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(47, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Format"
        '
        'frmBalanceSheet_Format1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1086, 581)
        Me.ContextMenuStrip = Me.contextMenuStrip1
        Me.Controls.Add(Me.pnlGrid)
        Me.Controls.Add(Me.pnlGirdhead)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name = "frmBalanceSheet_Format1"
        Me.Text = "Balance Sheet New"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.contextMenuStrip1.ResumeLayout(False)
        Me.pnlGirdhead.ResumeLayout(False)
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlGrid.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents dtpDate As GiriDatePicker.DatePicker
    Friend WithEvents btnView As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents cmbCompany As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Private WithEvents contextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Private WithEvents viewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents newToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents exitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlGirdhead As System.Windows.Forms.Panel
    Friend WithEvents pnlGrid As System.Windows.Forms.Panel
    Friend WithEvents gridViewHead As System.Windows.Forms.DataGridView

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Friend WithEvents Panel3 As Panel
    Friend WithEvents rbtReport As RadioButton
    Friend WithEvents rbtAccount As RadioButton
    Friend WithEvents Label5 As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents rbtDetailed As RadioButton
    Friend WithEvents rbtSummary As RadioButton
    Friend WithEvents Label4 As Label
End Class
