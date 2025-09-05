<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGSTR1
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
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.gridView = New System.Windows.Forms.DataGridView
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AutoReziseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.pnlGridHead = New System.Windows.Forms.Panel
        Me.gridViewHead = New System.Windows.Forms.DataGridView
        Me.pnlHeading = New System.Windows.Forms.Panel
        Me.lblTitle = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExtToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.btnView_Search = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmbMetal = New System.Windows.Forms.ComboBox
        Me.btnExport = New System.Windows.Forms.Button
        Me.btnPrint = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.btnExcel_Upd = New System.Windows.Forms.Button
        Me.Chk14 = New System.Windows.Forms.CheckBox
        Me.Chk13 = New System.Windows.Forms.CheckBox
        Me.Chk11 = New System.Windows.Forms.CheckBox
        Me.Chk9 = New System.Windows.Forms.CheckBox
        Me.Chk10 = New System.Windows.Forms.CheckBox
        Me.Chk12 = New System.Windows.Forms.CheckBox
        Me.ChkCRDR = New System.Windows.Forms.CheckBox
        Me.Chk7 = New System.Windows.Forms.CheckBox
        Me.Chk6 = New System.Windows.Forms.CheckBox
        Me.ChkSumm = New System.Windows.Forms.CheckBox
        Me.ChkStateSum = New System.Windows.Forms.CheckBox
        Me.ChkCountSumm = New System.Windows.Forms.CheckBox
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.Panel2.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.pnlGridHead.SuspendLayout()
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlHeading.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.gridView)
        Me.Panel2.Controls.Add(Me.pnlGridHead)
        Me.Panel2.Controls.Add(Me.pnlHeading)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 176)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1028, 440)
        Me.Panel2.TabIndex = 1
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.ContextMenuStrip = Me.ContextMenuStrip2
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 78)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 21
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.Size = New System.Drawing.Size(1028, 362)
        Me.gridView.TabIndex = 0
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AutoReziseToolStripMenuItem})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(133, 26)
        '
        'AutoReziseToolStripMenuItem
        '
        Me.AutoReziseToolStripMenuItem.CheckOnClick = True
        Me.AutoReziseToolStripMenuItem.Name = "AutoReziseToolStripMenuItem"
        Me.AutoReziseToolStripMenuItem.Size = New System.Drawing.Size(132, 22)
        Me.AutoReziseToolStripMenuItem.Text = "AutoRezise"
        '
        'pnlGridHead
        '
        Me.pnlGridHead.Controls.Add(Me.gridViewHead)
        Me.pnlGridHead.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlGridHead.Location = New System.Drawing.Point(0, 25)
        Me.pnlGridHead.Name = "pnlGridHead"
        Me.pnlGridHead.Size = New System.Drawing.Size(1028, 53)
        Me.pnlGridHead.TabIndex = 6
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
        Me.gridViewHead.Size = New System.Drawing.Size(1028, 53)
        Me.gridViewHead.TabIndex = 2
        '
        'pnlHeading
        '
        Me.pnlHeading.Controls.Add(Me.lblTitle)
        Me.pnlHeading.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeading.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeading.Name = "pnlHeading"
        Me.pnlHeading.Size = New System.Drawing.Size(1028, 25)
        Me.pnlHeading.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1028, 25)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExtToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(118, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExtToolStripMenuItem
        '
        Me.ExtToolStripMenuItem.Name = "ExtToolStripMenuItem"
        Me.ExtToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExtToolStripMenuItem.Size = New System.Drawing.Size(117, 22)
        Me.ExtToolStripMenuItem.Text = "Exit"
        Me.ExtToolStripMenuItem.Visible = False
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(81, 139)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 20
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(381, 139)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 23
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(181, 139)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 21
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date From"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(7, 40)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(7, 66)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(37, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Metal"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(186, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(23, 21)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbMetal
        '
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(87, 62)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(288, 21)
        Me.cmbMetal.TabIndex = 7
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(281, 139)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 22
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(481, 139)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 24
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnExcel_Upd)
        Me.Panel1.Controls.Add(Me.Chk14)
        Me.Panel1.Controls.Add(Me.Chk13)
        Me.Panel1.Controls.Add(Me.Chk11)
        Me.Panel1.Controls.Add(Me.Chk9)
        Me.Panel1.Controls.Add(Me.Chk10)
        Me.Panel1.Controls.Add(Me.Chk12)
        Me.Panel1.Controls.Add(Me.ChkCRDR)
        Me.Panel1.Controls.Add(Me.Chk7)
        Me.Panel1.Controls.Add(Me.Chk6)
        Me.Panel1.Controls.Add(Me.ChkSumm)
        Me.Panel1.Controls.Add(Me.ChkStateSum)
        Me.Panel1.Controls.Add(Me.ChkCountSumm)
        Me.Panel1.Controls.Add(Me.chkCmbCostCentre)
        Me.Panel1.Controls.Add(Me.dtpTo)
        Me.Panel1.Controls.Add(Me.dtpFrom)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.cmbMetal)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.btnView_Search)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1028, 176)
        Me.Panel1.TabIndex = 0
        '
        'btnExcel_Upd
        '
        Me.btnExcel_Upd.Location = New System.Drawing.Point(481, 103)
        Me.btnExcel_Upd.Name = "btnExcel_Upd"
        Me.btnExcel_Upd.Size = New System.Drawing.Size(100, 30)
        Me.btnExcel_Upd.TabIndex = 25
        Me.btnExcel_Upd.Text = "Export [X]"
        Me.btnExcel_Upd.UseVisualStyleBackColor = True
        '
        'Chk14
        '
        Me.Chk14.AutoSize = True
        Me.Chk14.Location = New System.Drawing.Point(595, 156)
        Me.Chk14.Name = "Chk14"
        Me.Chk14.Size = New System.Drawing.Size(316, 17)
        Me.Chk14.TabIndex = 19
        Me.Chk14.Text = "14) Unregistered persons liable for reverse charge"
        Me.Chk14.UseVisualStyleBackColor = True
        '
        'Chk13
        '
        Me.Chk13.AutoSize = True
        Me.Chk13.Location = New System.Drawing.Point(595, 137)
        Me.Chk13.Name = "Chk13"
        Me.Chk13.Size = New System.Drawing.Size(414, 17)
        Me.Chk13.TabIndex = 18
        Me.Chk13.Text = "13) Supplies made through e-commerce portals of other companies"
        Me.Chk13.UseVisualStyleBackColor = True
        '
        'Chk11
        '
        Me.Chk11.AutoSize = True
        Me.Chk11.Location = New System.Drawing.Point(595, 101)
        Me.Chk11.Name = "Chk11"
        Me.Chk11.Size = New System.Drawing.Size(601, 17)
        Me.Chk11.TabIndex = 16
        Me.Chk11.Text = "11) Tax liability arising on account of Time of Supply without issuance of Invoic" & _
            "e in the same period."
        Me.Chk11.UseVisualStyleBackColor = True
        '
        'Chk9
        '
        Me.Chk9.AutoSize = True
        Me.Chk9.Location = New System.Drawing.Point(595, 64)
        Me.Chk9.Name = "Chk9"
        Me.Chk9.Size = New System.Drawing.Size(334, 17)
        Me.Chk9.TabIndex = 14
        Me.Chk9.Text = "9) Nil rated, Exempted and Non GST outward supplies"
        Me.Chk9.UseVisualStyleBackColor = True
        '
        'Chk10
        '
        Me.Chk10.AutoSize = True
        Me.Chk10.Location = New System.Drawing.Point(595, 82)
        Me.Chk10.Name = "Chk10"
        Me.Chk10.Size = New System.Drawing.Size(313, 17)
        Me.Chk10.TabIndex = 15
        Me.Chk10.Text = "10) Supplies Exported (including deemed exports)"
        Me.Chk10.UseVisualStyleBackColor = True
        '
        'Chk12
        '
        Me.Chk12.AutoSize = True
        Me.Chk12.Location = New System.Drawing.Point(595, 119)
        Me.Chk12.Name = "Chk12"
        Me.Chk12.Size = New System.Drawing.Size(374, 17)
        Me.Chk12.TabIndex = 17
        Me.Chk12.Text = "12) Tax already paid on invoices issued in the current period"
        Me.Chk12.UseVisualStyleBackColor = True
        '
        'ChkCRDR
        '
        Me.ChkCRDR.AutoSize = True
        Me.ChkCRDR.Location = New System.Drawing.Point(595, 46)
        Me.ChkCRDR.Name = "ChkCRDR"
        Me.ChkCRDR.Size = New System.Drawing.Size(206, 17)
        Me.ChkCRDR.TabIndex = 13
        Me.ChkCRDR.Text = "8) Details of Credit/Debit Notes"
        Me.ChkCRDR.UseVisualStyleBackColor = True
        '
        'Chk7
        '
        Me.Chk7.AutoSize = True
        Me.Chk7.Location = New System.Drawing.Point(595, 28)
        Me.Chk7.Name = "Chk7"
        Me.Chk7.Size = New System.Drawing.Size(234, 17)
        Me.Chk7.TabIndex = 12
        Me.Chk7.Text = "7) Taxable Outward Supplies(<2.5L)"
        Me.Chk7.UseVisualStyleBackColor = True
        '
        'Chk6
        '
        Me.Chk6.AutoSize = True
        Me.Chk6.Location = New System.Drawing.Point(595, 9)
        Me.Chk6.Name = "Chk6"
        Me.Chk6.Size = New System.Drawing.Size(243, 17)
        Me.Chk6.TabIndex = 11
        Me.Chk6.Text = "6) Taxable Outward Supplies(>=2.5L)"
        Me.Chk6.UseVisualStyleBackColor = True
        '
        'ChkSumm
        '
        Me.ChkSumm.AutoSize = True
        Me.ChkSumm.Location = New System.Drawing.Point(381, 66)
        Me.ChkSumm.Name = "ChkSumm"
        Me.ChkSumm.Size = New System.Drawing.Size(82, 17)
        Me.ChkSumm.TabIndex = 10
        Me.ChkSumm.Text = "Summary"
        Me.ChkSumm.UseVisualStyleBackColor = True
        '
        'ChkStateSum
        '
        Me.ChkStateSum.AutoSize = True
        Me.ChkStateSum.Location = New System.Drawing.Point(381, 40)
        Me.ChkStateSum.Name = "ChkStateSum"
        Me.ChkStateSum.Size = New System.Drawing.Size(150, 17)
        Me.ChkStateSum.TabIndex = 9
        Me.ChkStateSum.Text = "State Code Summary"
        Me.ChkStateSum.UseVisualStyleBackColor = True
        '
        'ChkCountSumm
        '
        Me.ChkCountSumm.AutoSize = True
        Me.ChkCountSumm.Location = New System.Drawing.Point(381, 13)
        Me.ChkCountSumm.Name = "ChkCountSumm"
        Me.ChkCountSumm.Size = New System.Drawing.Size(166, 17)
        Me.ChkCountSumm.TabIndex = 8
        Me.ChkCountSumm.Text = "Counter Party Summary"
        Me.ChkCountSumm.UseVisualStyleBackColor = True
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(87, 35)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(288, 22)
        Me.chkCmbCostCentre.TabIndex = 5
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(216, 9)
        Me.dtpTo.Mask = "##/##/####"
        Me.dtpTo.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTo.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTo.Size = New System.Drawing.Size(93, 21)
        Me.dtpTo.TabIndex = 3
        Me.dtpTo.Text = "07/03/9998"
        Me.dtpTo.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(87, 9)
        Me.dtpFrom.Mask = "##/##/####"
        Me.dtpFrom.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrom.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrom.Size = New System.Drawing.Size(93, 21)
        Me.dtpFrom.TabIndex = 1
        Me.dtpFrom.Text = "07/03/9998"
        Me.dtpFrom.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'frmGSTR1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 616)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmGSTR1"
        Me.Text = "GSTR1"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel2.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip2.ResumeLayout(False)
        Me.pnlGridHead.ResumeLayout(False)
        CType(Me.gridViewHead, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlHeading.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents gridView As System.Windows.Forms.DataGridView
    Friend WithEvents pnlHeading As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExtToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnView_Search As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbMetal As System.Windows.Forms.ComboBox
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AutoReziseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChkStateSum As System.Windows.Forms.CheckBox
    Friend WithEvents ChkCountSumm As System.Windows.Forms.CheckBox
    Friend WithEvents ChkSumm As System.Windows.Forms.CheckBox
    Friend WithEvents Chk7 As System.Windows.Forms.CheckBox
    Friend WithEvents Chk6 As System.Windows.Forms.CheckBox
    Friend WithEvents ChkCRDR As System.Windows.Forms.CheckBox
    Friend WithEvents pnlGridHead As System.Windows.Forms.Panel
    Friend WithEvents gridViewHead As System.Windows.Forms.DataGridView
    Friend WithEvents Chk12 As System.Windows.Forms.CheckBox
    Friend WithEvents Chk10 As System.Windows.Forms.CheckBox
    Friend WithEvents Chk9 As System.Windows.Forms.CheckBox
    Friend WithEvents Chk11 As System.Windows.Forms.CheckBox
    Friend WithEvents Chk13 As System.Windows.Forms.CheckBox
    Friend WithEvents Chk14 As System.Windows.Forms.CheckBox
    Friend WithEvents btnExcel_Upd As System.Windows.Forms.Button
End Class
