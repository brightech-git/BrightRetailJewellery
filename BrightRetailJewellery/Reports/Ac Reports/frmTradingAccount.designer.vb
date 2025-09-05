<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTradingAccount
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
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.lblTitle = New System.Windows.Forms.Label
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnExport = New System.Windows.Forms.Button
        Me.btnView = New System.Windows.Forms.Button
        Me.dtpDate = New GiriDatePicker.DatePicker(Me.components)
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.cmbMetal = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmbCompany = New System.Windows.Forms.ComboBox
        Me.contextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.viewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.newToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.exitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.ChkSRate = New System.Windows.Forms.CheckBox
        Me.ChkRate = New System.Windows.Forms.CheckBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.rbtGs11Netwt = New System.Windows.Forms.RadioButton
        Me.rbtGs11Grswt = New System.Windows.Forms.RadioButton
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.rbtGs12Netwt = New System.Windows.Forms.RadioButton
        Me.rbtGs12Grswt = New System.Windows.Forms.RadioButton
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.contextMenuStrip1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.SuspendLayout()
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
        Me.gridView.Size = New System.Drawing.Size(1042, 420)
        Me.gridView.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(0, 130)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1042, 31)
        Me.lblTitle.TabIndex = 16
        Me.lblTitle.Text = "Label1"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(195, 94)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(96, 31)
        Me.btnNew.TabIndex = 11
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(399, 94)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(96, 31)
        Me.btnPrint.TabIndex = 13
        Me.btnPrint.Text = "Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(501, 94)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(96, 31)
        Me.btnExit.TabIndex = 14
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(297, 94)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(96, 31)
        Me.btnExport.TabIndex = 12
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnView
        '
        Me.btnView.Location = New System.Drawing.Point(94, 94)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(96, 31)
        Me.btnView.TabIndex = 10
        Me.btnView.Text = "View [F2]"
        Me.btnView.UseVisualStyleBackColor = True
        '
        'dtpDate
        '
        Me.dtpDate.Location = New System.Drawing.Point(425, 67)
        Me.dtpDate.Mask = "##-##-####"
        Me.dtpDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Seperator = Global.Microsoft.VisualBasic.ChrW(45)
        Me.dtpDate.Size = New System.Drawing.Size(88, 21)
        Me.dtpDate.TabIndex = 9
        Me.dtpDate.Text = "01-03-9998"
        Me.dtpDate.Value = New Date(9998, 3, 1, 0, 0, 0, 0)
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.Panel4)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.ChkSRate)
        Me.Panel1.Controls.Add(Me.ChkRate)
        Me.Panel1.Controls.Add(Me.cmbMetal)
        Me.Panel1.Controls.Add(Me.lblTitle)
        Me.Panel1.Controls.Add(Me.Label4)
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
        Me.Panel1.Size = New System.Drawing.Size(1042, 161)
        Me.Panel1.TabIndex = 0
        '
        'cmbMetal
        '
        Me.cmbMetal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Items.AddRange(New Object() {"ALL", "GOLD", "SILVER", "DIAMOND", "PLATINUM"})
        Me.cmbMetal.Location = New System.Drawing.Point(94, 67)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(252, 21)
        Me.cmbMetal.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 71)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(41, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Metal "
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(94, 39)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(252, 22)
        Me.chkCmbCostCentre.TabIndex = 3
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(11, 44)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(62, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Company"
        '
        'cmbCompany
        '
        Me.cmbCompany.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCompany.FormattingEnabled = True
        Me.cmbCompany.Location = New System.Drawing.Point(94, 12)
        Me.cmbCompany.Name = "cmbCompany"
        Me.cmbCompany.Size = New System.Drawing.Size(252, 21)
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
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.gridView)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(0, 161)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1042, 420)
        Me.Panel3.TabIndex = 5
        '
        'ChkSRate
        '
        Me.ChkSRate.AutoSize = True
        Me.ChkSRate.Location = New System.Drawing.Point(619, 14)
        Me.ChkSRate.Name = "ChkSRate"
        Me.ChkSRate.Size = New System.Drawing.Size(271, 17)
        Me.ChkSRate.TabIndex = 7
        Me.ChkSRate.Text = "Issue to GS12 Rate from Purchase [Silver]"
        Me.ChkSRate.UseVisualStyleBackColor = True
        '
        'ChkRate
        '
        Me.ChkRate.AutoSize = True
        Me.ChkRate.Location = New System.Drawing.Point(619, 42)
        Me.ChkRate.Name = "ChkRate"
        Me.ChkRate.Size = New System.Drawing.Size(264, 17)
        Me.ChkRate.TabIndex = 6
        Me.ChkRate.Text = "Issue to GS12 Rate from Purchase [Gold]"
        Me.ChkRate.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(381, 71)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(34, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Date"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(381, 44)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(38, 13)
        Me.Label2.TabIndex = 19
        Me.Label2.Text = "GS12"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(381, 16)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(38, 13)
        Me.Label6.TabIndex = 17
        Me.Label6.Text = "GS11"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rbtGs11Netwt)
        Me.Panel2.Controls.Add(Me.rbtGs11Grswt)
        Me.Panel2.Location = New System.Drawing.Point(425, 11)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(172, 22)
        Me.Panel2.TabIndex = 18
        '
        'rbtGs11Netwt
        '
        Me.rbtGs11Netwt.AutoSize = True
        Me.rbtGs11Netwt.Location = New System.Drawing.Point(97, 3)
        Me.rbtGs11Netwt.Name = "rbtGs11Netwt"
        Me.rbtGs11Netwt.Size = New System.Drawing.Size(63, 17)
        Me.rbtGs11Netwt.TabIndex = 0
        Me.rbtGs11Netwt.Text = "Net Wt"
        Me.rbtGs11Netwt.UseVisualStyleBackColor = True
        '
        'rbtGs11Grswt
        '
        Me.rbtGs11Grswt.AutoSize = True
        Me.rbtGs11Grswt.Checked = True
        Me.rbtGs11Grswt.Location = New System.Drawing.Point(5, 3)
        Me.rbtGs11Grswt.Name = "rbtGs11Grswt"
        Me.rbtGs11Grswt.Size = New System.Drawing.Size(64, 17)
        Me.rbtGs11Grswt.TabIndex = 1
        Me.rbtGs11Grswt.TabStop = True
        Me.rbtGs11Grswt.Text = "Grs Wt"
        Me.rbtGs11Grswt.UseVisualStyleBackColor = True
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.rbtGs12Netwt)
        Me.Panel4.Controls.Add(Me.rbtGs12Grswt)
        Me.Panel4.Location = New System.Drawing.Point(425, 39)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(172, 22)
        Me.Panel4.TabIndex = 20
        '
        'rbtGs12Netwt
        '
        Me.rbtGs12Netwt.AutoSize = True
        Me.rbtGs12Netwt.Location = New System.Drawing.Point(97, 3)
        Me.rbtGs12Netwt.Name = "rbtGs12Netwt"
        Me.rbtGs12Netwt.Size = New System.Drawing.Size(63, 17)
        Me.rbtGs12Netwt.TabIndex = 0
        Me.rbtGs12Netwt.Text = "Net Wt"
        Me.rbtGs12Netwt.UseVisualStyleBackColor = True
        '
        'rbtGs12Grswt
        '
        Me.rbtGs12Grswt.AutoSize = True
        Me.rbtGs12Grswt.Checked = True
        Me.rbtGs12Grswt.Location = New System.Drawing.Point(5, 3)
        Me.rbtGs12Grswt.Name = "rbtGs12Grswt"
        Me.rbtGs12Grswt.Size = New System.Drawing.Size(64, 17)
        Me.rbtGs12Grswt.TabIndex = 1
        Me.rbtGs12Grswt.TabStop = True
        Me.rbtGs12Grswt.Text = "Grs Wt"
        Me.rbtGs12Grswt.UseVisualStyleBackColor = True
        '
        'frmTradingAccount
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1042, 581)
        Me.ContextMenuStrip = Me.contextMenuStrip1
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name = "frmTradingAccount"
        Me.Text = "Metal Wise Trading & Profit and Loss"
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.contextMenuStrip1.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
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
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents cmbMetal As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ChkSRate As System.Windows.Forms.CheckBox
    Friend WithEvents ChkRate As System.Windows.Forms.CheckBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rbtGs11Netwt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGs11Grswt As System.Windows.Forms.RadioButton
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents rbtGs12Netwt As System.Windows.Forms.RadioButton
    Friend WithEvents rbtGs12Grswt As System.Windows.Forms.RadioButton
End Class
