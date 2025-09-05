<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmExemptionReport
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
        Me.grp = New System.Windows.Forms.GroupBox
        Me.ChkwithDisc = New System.Windows.Forms.CheckBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.rbtFormat1 = New System.Windows.Forms.RadioButton
        Me.rbtFormat2 = New System.Windows.Forms.RadioButton
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnexit = New System.Windows.Forms.Button
        Me.pnlCostCenter = New System.Windows.Forms.Panel
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.lblCostCenter = New System.Windows.Forms.Label
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.btnExport = New System.Windows.Forms.Button
        Me.chkDate = New System.Windows.Forms.CheckBox
        Me.btnview = New System.Windows.Forms.Button
        Me.btnsendmail = New System.Windows.Forms.Button
        Me.btnnew = New System.Windows.Forms.Button
        Me.lblOptions = New System.Windows.Forms.Label
        Me.cmbOptions = New System.Windows.Forms.ComboBox
        Me.dtpEditDate = New BrighttechPack.DatePicker(Me.components)
        Me.lblEditDate = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AutoResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.PnlGridview = New System.Windows.Forms.Panel
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.GridView = New System.Windows.Forms.DataGridView
        Me.GridHead = New System.Windows.Forms.DataGridView
        Me.pnlHeading = New System.Windows.Forms.Panel
        Me.lblTitle = New System.Windows.Forms.Label
        Me.grp.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.pnlCostCenter.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.PnlGridview.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridHead, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlHeading.SuspendLayout()
        Me.SuspendLayout()
        '
        'grp
        '
        Me.grp.Controls.Add(Me.ChkwithDisc)
        Me.grp.Controls.Add(Me.Label1)
        Me.grp.Controls.Add(Me.Panel5)
        Me.grp.Controls.Add(Me.btnPrint)
        Me.grp.Controls.Add(Me.btnexit)
        Me.grp.Controls.Add(Me.pnlCostCenter)
        Me.grp.Controls.Add(Me.dtpTo)
        Me.grp.Controls.Add(Me.btnExport)
        Me.grp.Controls.Add(Me.chkDate)
        Me.grp.Controls.Add(Me.btnview)
        Me.grp.Controls.Add(Me.btnsendmail)
        Me.grp.Controls.Add(Me.btnnew)
        Me.grp.Controls.Add(Me.lblOptions)
        Me.grp.Controls.Add(Me.cmbOptions)
        Me.grp.Controls.Add(Me.dtpEditDate)
        Me.grp.Controls.Add(Me.lblEditDate)
        Me.grp.Dock = System.Windows.Forms.DockStyle.Top
        Me.grp.Location = New System.Drawing.Point(0, 0)
        Me.grp.Name = "grp"
        Me.grp.Size = New System.Drawing.Size(1020, 105)
        Me.grp.TabIndex = 0
        Me.grp.TabStop = False
        '
        'ChkwithDisc
        '
        Me.ChkwithDisc.AutoSize = True
        Me.ChkwithDisc.Location = New System.Drawing.Point(634, 45)
        Me.ChkwithDisc.Name = "ChkwithDisc"
        Me.ChkwithDisc.Size = New System.Drawing.Size(100, 17)
        Me.ChkwithDisc.TabIndex = 9
        Me.ChkwithDisc.Text = "WithDiscount"
        Me.ChkwithDisc.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(326, 45)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 13)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Method"
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.rbtFormat1)
        Me.Panel5.Controls.Add(Me.rbtFormat2)
        Me.Panel5.Location = New System.Drawing.Point(413, 40)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(215, 25)
        Me.Panel5.TabIndex = 8
        '
        'rbtFormat1
        '
        Me.rbtFormat1.AutoSize = True
        Me.rbtFormat1.Checked = True
        Me.rbtFormat1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbtFormat1.Location = New System.Drawing.Point(5, 5)
        Me.rbtFormat1.Name = "rbtFormat1"
        Me.rbtFormat1.Size = New System.Drawing.Size(72, 17)
        Me.rbtFormat1.TabIndex = 0
        Me.rbtFormat1.TabStop = True
        Me.rbtFormat1.Text = "Format1"
        Me.rbtFormat1.UseVisualStyleBackColor = True
        '
        'rbtFormat2
        '
        Me.rbtFormat2.AutoSize = True
        Me.rbtFormat2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbtFormat2.Location = New System.Drawing.Point(114, 5)
        Me.rbtFormat2.Name = "rbtFormat2"
        Me.rbtFormat2.Size = New System.Drawing.Size(72, 17)
        Me.rbtFormat2.TabIndex = 1
        Me.rbtFormat2.Text = "Format2"
        Me.rbtFormat2.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(398, 67)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(98, 30)
        Me.btnPrint.TabIndex = 13
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnexit
        '
        Me.btnexit.Location = New System.Drawing.Point(596, 67)
        Me.btnexit.Name = "btnexit"
        Me.btnexit.Size = New System.Drawing.Size(98, 30)
        Me.btnexit.TabIndex = 15
        Me.btnexit.Text = "Exit[F12]"
        Me.btnexit.UseVisualStyleBackColor = True
        '
        'pnlCostCenter
        '
        Me.pnlCostCenter.Controls.Add(Me.chkCmbCostCentre)
        Me.pnlCostCenter.Controls.Add(Me.lblCostCenter)
        Me.pnlCostCenter.Location = New System.Drawing.Point(324, 12)
        Me.pnlCostCenter.Name = "pnlCostCenter"
        Me.pnlCostCenter.Size = New System.Drawing.Size(339, 26)
        Me.pnlCostCenter.TabIndex = 6
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(90, 1)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(242, 22)
        Me.chkCmbCostCentre.TabIndex = 1
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'lblCostCenter
        '
        Me.lblCostCenter.Location = New System.Drawing.Point(3, 3)
        Me.lblCostCenter.Name = "lblCostCenter"
        Me.lblCostCenter.Size = New System.Drawing.Size(91, 17)
        Me.lblCostCenter.TabIndex = 0
        Me.lblCostCenter.Text = "&Cost Centre"
        Me.lblCostCenter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(230, 12)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(87, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(299, 67)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(98, 30)
        Me.btnExport.TabIndex = 12
        Me.btnExport.Text = "Export[X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'chkDate
        '
        Me.chkDate.AutoSize = True
        Me.chkDate.Location = New System.Drawing.Point(8, 15)
        Me.chkDate.Name = "chkDate"
        Me.chkDate.Size = New System.Drawing.Size(87, 17)
        Me.chkDate.TabIndex = 0
        Me.chkDate.Text = "As OnDate"
        Me.chkDate.UseVisualStyleBackColor = True
        '
        'btnview
        '
        Me.btnview.Location = New System.Drawing.Point(101, 67)
        Me.btnview.Name = "btnview"
        Me.btnview.Size = New System.Drawing.Size(98, 30)
        Me.btnview.TabIndex = 10
        Me.btnview.Text = "&View"
        Me.btnview.UseVisualStyleBackColor = True
        '
        'btnsendmail
        '
        Me.btnsendmail.Enabled = False
        Me.btnsendmail.Location = New System.Drawing.Point(497, 67)
        Me.btnsendmail.Name = "btnsendmail"
        Me.btnsendmail.Size = New System.Drawing.Size(98, 30)
        Me.btnsendmail.TabIndex = 14
        Me.btnsendmail.Text = "&Mail"
        Me.btnsendmail.UseVisualStyleBackColor = True
        '
        'btnnew
        '
        Me.btnnew.Location = New System.Drawing.Point(200, 67)
        Me.btnnew.Name = "btnnew"
        Me.btnnew.Size = New System.Drawing.Size(98, 30)
        Me.btnnew.TabIndex = 11
        Me.btnnew.Text = "New[F3]"
        Me.btnnew.UseVisualStyleBackColor = True
        '
        'lblOptions
        '
        Me.lblOptions.AutoSize = True
        Me.lblOptions.Location = New System.Drawing.Point(7, 44)
        Me.lblOptions.Name = "lblOptions"
        Me.lblOptions.Size = New System.Drawing.Size(81, 13)
        Me.lblOptions.TabIndex = 4
        Me.lblOptions.Text = "Option Name"
        '
        'cmbOptions
        '
        Me.cmbOptions.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbOptions.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbOptions.FormattingEnabled = True
        Me.cmbOptions.Location = New System.Drawing.Point(101, 40)
        Me.cmbOptions.Name = "cmbOptions"
        Me.cmbOptions.Size = New System.Drawing.Size(215, 21)
        Me.cmbOptions.TabIndex = 5
        '
        'dtpEditDate
        '
        Me.dtpEditDate.Location = New System.Drawing.Point(101, 12)
        Me.dtpEditDate.Mask = "##/##/####"
        Me.dtpEditDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpEditDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpEditDate.Name = "dtpEditDate"
        Me.dtpEditDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpEditDate.Size = New System.Drawing.Size(90, 21)
        Me.dtpEditDate.TabIndex = 1
        Me.dtpEditDate.Text = "07/03/9998"
        Me.dtpEditDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'lblEditDate
        '
        Me.lblEditDate.AutoSize = True
        Me.lblEditDate.Location = New System.Drawing.Point(199, 16)
        Me.lblEditDate.Name = "lblEditDate"
        Me.lblEditDate.Size = New System.Drawing.Size(21, 13)
        Me.lblEditDate.TabIndex = 2
        Me.lblEditDate.Text = "To"
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
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.pnlHeading)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 105)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1020, 635)
        Me.Panel1.TabIndex = 3
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.PnlGridview)
        Me.Panel2.Controls.Add(Me.GridHead)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 34)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1020, 601)
        Me.Panel2.TabIndex = 4
        '
        'PnlGridview
        '
        Me.PnlGridview.Controls.Add(Me.Panel3)
        Me.PnlGridview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PnlGridview.Location = New System.Drawing.Point(0, 18)
        Me.PnlGridview.Name = "PnlGridview"
        Me.PnlGridview.Size = New System.Drawing.Size(1020, 583)
        Me.PnlGridview.TabIndex = 2
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.GridView)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1020, 583)
        Me.Panel3.TabIndex = 3
        '
        'GridView
        '
        Me.GridView.AllowUserToAddRows = False
        Me.GridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridView.ContextMenuStrip = Me.ContextMenuStrip1
        Me.GridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridView.Location = New System.Drawing.Point(0, 0)
        Me.GridView.Name = "GridView"
        Me.GridView.ReadOnly = True
        Me.GridView.RowHeadersVisible = False
        Me.GridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.GridView.Size = New System.Drawing.Size(1020, 583)
        Me.GridView.TabIndex = 0
        '
        'GridHead
        '
        Me.GridHead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridHead.Dock = System.Windows.Forms.DockStyle.Top
        Me.GridHead.Location = New System.Drawing.Point(0, 0)
        Me.GridHead.Name = "GridHead"
        Me.GridHead.Size = New System.Drawing.Size(1020, 18)
        Me.GridHead.TabIndex = 1
        '
        'pnlHeading
        '
        Me.pnlHeading.Controls.Add(Me.lblTitle)
        Me.pnlHeading.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeading.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeading.Name = "pnlHeading"
        Me.pnlHeading.Size = New System.Drawing.Size(1020, 34)
        Me.pnlHeading.TabIndex = 3
        Me.pnlHeading.Visible = False
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(316, 8)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(31, 13)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Title"
        '
        'frmExemptionReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1020, 740)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.grp)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmExemptionReport"
        Me.Text = "Exception Report"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grp.ResumeLayout(False)
        Me.grp.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.pnlCostCenter.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.PnlGridview.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        CType(Me.GridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridHead, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlHeading.ResumeLayout(False)
        Me.pnlHeading.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grp As System.Windows.Forms.GroupBox
    Friend WithEvents dtpEditDate As BrighttechPack.DatePicker
    Friend WithEvents lblEditDate As System.Windows.Forms.Label
    Friend WithEvents cmbOptions As System.Windows.Forms.ComboBox
    Friend WithEvents lblOptions As System.Windows.Forms.Label
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnview As System.Windows.Forms.Button
    Friend WithEvents btnexit As System.Windows.Forms.Button
    Friend WithEvents btnsendmail As System.Windows.Forms.Button
    Friend WithEvents btnnew As System.Windows.Forms.Button
    Friend WithEvents chkDate As System.Windows.Forms.CheckBox
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents lblCostCenter As System.Windows.Forms.Label
    Friend WithEvents pnlCostCenter As System.Windows.Forms.Panel
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AutoResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents pnlHeading As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents GridView As System.Windows.Forms.DataGridView
    Friend WithEvents GridHead As System.Windows.Forms.DataGridView
    Friend WithEvents rbtFormat2 As System.Windows.Forms.RadioButton
    Friend WithEvents rbtFormat1 As System.Windows.Forms.RadioButton
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ChkwithDisc As System.Windows.Forms.CheckBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents PnlGridview As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
End Class
