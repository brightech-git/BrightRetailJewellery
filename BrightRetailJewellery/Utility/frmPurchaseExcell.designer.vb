<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPurchaseExcell
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
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Grouper2 = New CodeVendor.Controls.Grouper
        Me.DgvTranTotal = New System.Windows.Forms.DataGridView
        Me.DgvTran = New System.Windows.Forms.DataGridView
        Me.grpHeader = New CodeVendor.Controls.Grouper
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtTaxper_PER = New System.Windows.Forms.TextBox
        Me.btntemplate = New System.Windows.Forms.Button
        Me.ChkSale = New System.Windows.Forms.CheckBox
        Me.ChkPurchase = New System.Windows.Forms.CheckBox
        Me.BtnExcelDownload = New System.Windows.Forms.Button
        Me.lblAddress = New System.Windows.Forms.Label
        Me.cmbAcName = New System.Windows.Forms.ComboBox
        Me.cmbCostCentre = New System.Windows.Forms.ComboBox
        Me.dtpBillDate_OWN = New BrighttechPack.DatePicker(Me.components)
        Me.dtpTrandate = New BrighttechPack.DatePicker(Me.components)
        Me.lblTitle = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.txtBillNo = New System.Windows.Forms.TextBox
        Me.Grouper1 = New CodeVendor.Controls.Grouper
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.pnlLeft = New System.Windows.Forms.Panel
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Panel1.SuspendLayout()
        Me.Grouper2.SuspendLayout()
        CType(Me.DgvTranTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DgvTran, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpHeader.SuspendLayout()
        Me.Grouper1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Grouper2)
        Me.Panel1.Controls.Add(Me.grpHeader)
        Me.Panel1.Controls.Add(Me.Grouper1)
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.pnlLeft)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1020, 636)
        Me.Panel1.TabIndex = 2
        '
        'Grouper2
        '
        Me.Grouper2.BackgroundColor = System.Drawing.Color.Lavender
        Me.Grouper2.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.Grouper2.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.Grouper2.BorderColor = System.Drawing.Color.Transparent
        Me.Grouper2.BorderThickness = 1.0!
        Me.Grouper2.Controls.Add(Me.DgvTranTotal)
        Me.Grouper2.Controls.Add(Me.DgvTran)
        Me.Grouper2.CustomGroupBoxColor = System.Drawing.Color.White
        Me.Grouper2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Grouper2.GroupImage = Nothing
        Me.Grouper2.GroupTitle = ""
        Me.Grouper2.Location = New System.Drawing.Point(10, 132)
        Me.Grouper2.Name = "Grouper2"
        Me.Grouper2.Padding = New System.Windows.Forms.Padding(20)
        Me.Grouper2.PaintGroupBox = False
        Me.Grouper2.RoundCorners = 10
        Me.Grouper2.ShadowColor = System.Drawing.Color.DarkGray
        Me.Grouper2.ShadowControl = False
        Me.Grouper2.ShadowThickness = 3
        Me.Grouper2.Size = New System.Drawing.Size(1000, 399)
        Me.Grouper2.TabIndex = 21
        '
        'DgvTranTotal
        '
        Me.DgvTranTotal.AllowUserToAddRows = False
        Me.DgvTranTotal.AllowUserToDeleteRows = False
        Me.DgvTranTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DgvTranTotal.ColumnHeadersVisible = False
        Me.DgvTranTotal.Enabled = False
        Me.DgvTranTotal.Location = New System.Drawing.Point(9, 377)
        Me.DgvTranTotal.Name = "DgvTranTotal"
        Me.DgvTranTotal.ReadOnly = True
        Me.DgvTranTotal.RowHeadersVisible = False
        Me.DgvTranTotal.Size = New System.Drawing.Size(981, 19)
        Me.DgvTranTotal.TabIndex = 37
        Me.DgvTranTotal.Visible = False
        '
        'DgvTran
        '
        Me.DgvTran.AllowUserToAddRows = False
        Me.DgvTran.AllowUserToDeleteRows = False
        Me.DgvTran.AllowUserToResizeRows = False
        Me.DgvTran.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DgvTran.Location = New System.Drawing.Point(9, 19)
        Me.DgvTran.Name = "DgvTran"
        Me.DgvTran.RowHeadersVisible = False
        Me.DgvTran.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.DgvTran.Size = New System.Drawing.Size(981, 374)
        Me.DgvTran.TabIndex = 0
        '
        'grpHeader
        '
        Me.grpHeader.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpHeader.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpHeader.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpHeader.BorderColor = System.Drawing.Color.Transparent
        Me.grpHeader.BorderThickness = 1.0!
        Me.grpHeader.Controls.Add(Me.Label1)
        Me.grpHeader.Controls.Add(Me.txtTaxper_PER)
        Me.grpHeader.Controls.Add(Me.btntemplate)
        Me.grpHeader.Controls.Add(Me.ChkSale)
        Me.grpHeader.Controls.Add(Me.ChkPurchase)
        Me.grpHeader.Controls.Add(Me.BtnExcelDownload)
        Me.grpHeader.Controls.Add(Me.lblAddress)
        Me.grpHeader.Controls.Add(Me.cmbAcName)
        Me.grpHeader.Controls.Add(Me.cmbCostCentre)
        Me.grpHeader.Controls.Add(Me.dtpBillDate_OWN)
        Me.grpHeader.Controls.Add(Me.dtpTrandate)
        Me.grpHeader.Controls.Add(Me.lblTitle)
        Me.grpHeader.Controls.Add(Me.Label20)
        Me.grpHeader.Controls.Add(Me.Label19)
        Me.grpHeader.Controls.Add(Me.Label18)
        Me.grpHeader.Controls.Add(Me.Label17)
        Me.grpHeader.Controls.Add(Me.Label15)
        Me.grpHeader.Controls.Add(Me.txtBillNo)
        Me.grpHeader.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpHeader.GroupImage = Nothing
        Me.grpHeader.GroupTitle = ""
        Me.grpHeader.Location = New System.Drawing.Point(10, 0)
        Me.grpHeader.Name = "grpHeader"
        Me.grpHeader.Padding = New System.Windows.Forms.Padding(20)
        Me.grpHeader.PaintGroupBox = False
        Me.grpHeader.RoundCorners = 10
        Me.grpHeader.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpHeader.ShadowControl = False
        Me.grpHeader.ShadowThickness = 3
        Me.grpHeader.Size = New System.Drawing.Size(1000, 132)
        Me.grpHeader.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(856, 55)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(50, 14)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Tax %"
        '
        'txtTaxper_PER
        '
        Me.txtTaxper_PER.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.txtTaxper_PER.Location = New System.Drawing.Point(912, 52)
        Me.txtTaxper_PER.Name = "txtTaxper_PER"
        Me.txtTaxper_PER.Size = New System.Drawing.Size(68, 22)
        Me.txtTaxper_PER.TabIndex = 6
        '
        'btntemplate
        '
        Me.btntemplate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btntemplate.Location = New System.Drawing.Point(883, 100)
        Me.btntemplate.Name = "btntemplate"
        Me.btntemplate.Size = New System.Drawing.Size(107, 30)
        Me.btntemplate.TabIndex = 17
        Me.btntemplate.Text = "Template"
        Me.btntemplate.UseVisualStyleBackColor = True
        '
        'ChkSale
        '
        Me.ChkSale.AutoSize = True
        Me.ChkSale.Location = New System.Drawing.Point(812, 108)
        Me.ChkSale.Name = "ChkSale"
        Me.ChkSale.Size = New System.Drawing.Size(47, 17)
        Me.ChkSale.TabIndex = 16
        Me.ChkSale.Text = "Tag"
        Me.ChkSale.UseVisualStyleBackColor = True
        Me.ChkSale.Visible = False
        '
        'ChkPurchase
        '
        Me.ChkPurchase.AutoSize = True
        Me.ChkPurchase.Location = New System.Drawing.Point(738, 107)
        Me.ChkPurchase.Name = "ChkPurchase"
        Me.ChkPurchase.Size = New System.Drawing.Size(68, 17)
        Me.ChkPurchase.TabIndex = 15
        Me.ChkPurchase.Text = "Receipt"
        Me.ChkPurchase.UseVisualStyleBackColor = True
        '
        'BtnExcelDownload
        '
        Me.BtnExcelDownload.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnExcelDownload.Location = New System.Drawing.Point(573, 99)
        Me.BtnExcelDownload.Name = "BtnExcelDownload"
        Me.BtnExcelDownload.Size = New System.Drawing.Size(159, 30)
        Me.BtnExcelDownload.TabIndex = 13
        Me.BtnExcelDownload.Text = "Excel Upload"
        Me.BtnExcelDownload.UseVisualStyleBackColor = True
        '
        'lblAddress
        '
        Me.lblAddress.AutoSize = True
        Me.lblAddress.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblAddress.Location = New System.Drawing.Point(455, 81)
        Me.lblAddress.Name = "lblAddress"
        Me.lblAddress.Size = New System.Drawing.Size(60, 14)
        Me.lblAddress.TabIndex = 14
        Me.lblAddress.Text = "Address"
        '
        'cmbAcName
        '
        Me.cmbAcName.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbAcName.FormattingEnabled = True
        Me.cmbAcName.Location = New System.Drawing.Point(458, 52)
        Me.cmbAcName.Name = "cmbAcName"
        Me.cmbAcName.Size = New System.Drawing.Size(395, 22)
        Me.cmbAcName.TabIndex = 4
        '
        'cmbCostCentre
        '
        Me.cmbCostCentre.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbCostCentre.FormattingEnabled = True
        Me.cmbCostCentre.Location = New System.Drawing.Point(123, 51)
        Me.cmbCostCentre.Name = "cmbCostCentre"
        Me.cmbCostCentre.Size = New System.Drawing.Size(228, 22)
        Me.cmbCostCentre.TabIndex = 2
        '
        'dtpBillDate_OWN
        '
        Me.dtpBillDate_OWN.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.dtpBillDate_OWN.Location = New System.Drawing.Point(123, 104)
        Me.dtpBillDate_OWN.Mask = "##/##/####"
        Me.dtpBillDate_OWN.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpBillDate_OWN.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpBillDate_OWN.Name = "dtpBillDate_OWN"
        Me.dtpBillDate_OWN.Seperator = Global.Microsoft.VisualBasic.ChrW(45)
        Me.dtpBillDate_OWN.Size = New System.Drawing.Size(98, 22)
        Me.dtpBillDate_OWN.TabIndex = 8
        Me.dtpBillDate_OWN.Text = "08-08-2012"
        Me.dtpBillDate_OWN.Value = New Date(2012, 8, 8, 0, 0, 0, 0)
        '
        'dtpTrandate
        '
        Me.dtpTrandate.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.dtpTrandate.Location = New System.Drawing.Point(458, 104)
        Me.dtpTrandate.Mask = "##/##/####"
        Me.dtpTrandate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpTrandate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpTrandate.Name = "dtpTrandate"
        Me.dtpTrandate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpTrandate.Size = New System.Drawing.Size(94, 22)
        Me.dtpTrandate.TabIndex = 12
        Me.dtpTrandate.Text = "06/03/9998"
        Me.dtpTrandate.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(20, 20)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(960, 25)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "ORNAMENTS RECEIPT/TAG GENERATION-EXCEL UPLOAD"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label20.Location = New System.Drawing.Point(227, 107)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(50, 14)
        Me.Label20.TabIndex = 9
        Me.Label20.Text = "Bill No"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label19.Location = New System.Drawing.Point(12, 107)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(63, 14)
        Me.Label19.TabIndex = 7
        Me.Label19.Text = "Bill Date"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label18.Location = New System.Drawing.Point(358, 107)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(72, 14)
        Me.Label18.TabIndex = 11
        Me.Label18.Text = "Tran Date"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label17.Location = New System.Drawing.Point(358, 55)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(65, 14)
        Me.Label17.TabIndex = 3
        Me.Label17.Text = "Ac Name"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label15.Location = New System.Drawing.Point(12, 54)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(84, 14)
        Me.Label15.TabIndex = 1
        Me.Label15.Text = "Cost Center"
        '
        'txtBillNo
        '
        Me.txtBillNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.txtBillNo.Location = New System.Drawing.Point(283, 104)
        Me.txtBillNo.Name = "txtBillNo"
        Me.txtBillNo.Size = New System.Drawing.Size(68, 22)
        Me.txtBillNo.TabIndex = 10
        Me.txtBillNo.Text = "10000000.00"
        '
        'Grouper1
        '
        Me.Grouper1.BackgroundColor = System.Drawing.Color.Lavender
        Me.Grouper1.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.Grouper1.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.Grouper1.BorderColor = System.Drawing.Color.Transparent
        Me.Grouper1.BorderThickness = 1.0!
        Me.Grouper1.Controls.Add(Me.btnSave)
        Me.Grouper1.Controls.Add(Me.btnExit)
        Me.Grouper1.Controls.Add(Me.btnNew)
        Me.Grouper1.CustomGroupBoxColor = System.Drawing.Color.White
        Me.Grouper1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Grouper1.GroupImage = Nothing
        Me.Grouper1.GroupTitle = ""
        Me.Grouper1.Location = New System.Drawing.Point(10, 531)
        Me.Grouper1.Name = "Grouper1"
        Me.Grouper1.Padding = New System.Windows.Forms.Padding(20)
        Me.Grouper1.PaintGroupBox = False
        Me.Grouper1.RoundCorners = 10
        Me.Grouper1.ShadowColor = System.Drawing.Color.DarkGray
        Me.Grouper1.ShadowControl = False
        Me.Grouper1.ShadowThickness = 3
        Me.Grouper1.Size = New System.Drawing.Size(1000, 95)
        Me.Grouper1.TabIndex = 2
        '
        'btnSave
        '
        Me.btnSave.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(283, 23)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(105, 30)
        Me.btnSave.TabIndex = 18
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.Location = New System.Drawing.Point(505, 23)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(105, 30)
        Me.btnExit.TabIndex = 20
        Me.btnExit.Text = "E&xit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNew.Location = New System.Drawing.Point(394, 23)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(105, 30)
        Me.btnNew.TabIndex = 19
        Me.btnNew.Text = "&New"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(10, 626)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1000, 10)
        Me.Panel3.TabIndex = 19
        '
        'Panel2
        '
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel2.Location = New System.Drawing.Point(1010, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(10, 636)
        Me.Panel2.TabIndex = 18
        '
        'pnlLeft
        '
        Me.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnlLeft.Location = New System.Drawing.Point(0, 0)
        Me.pnlLeft.Name = "pnlLeft"
        Me.pnlLeft.Size = New System.Drawing.Size(10, 636)
        Me.pnlLeft.TabIndex = 0
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(129, 48)
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
        'frmPurchaseExcell
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(1020, 636)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmPurchaseExcell"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MaterialIssRecExcel"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel1.ResumeLayout(False)
        Me.Grouper2.ResumeLayout(False)
        CType(Me.DgvTranTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DgvTran, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpHeader.ResumeLayout(False)
        Me.grpHeader.PerformLayout()
        Me.Grouper1.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents pnlLeft As System.Windows.Forms.Panel
    Friend WithEvents Grouper1 As CodeVendor.Controls.Grouper
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Grouper2 As CodeVendor.Controls.Grouper
    Friend WithEvents DgvTranTotal As System.Windows.Forms.DataGridView
    Friend WithEvents DgvTran As System.Windows.Forms.DataGridView
    Friend WithEvents grpHeader As CodeVendor.Controls.Grouper
    Friend WithEvents lblAddress As System.Windows.Forms.Label
    Friend WithEvents cmbAcName As System.Windows.Forms.ComboBox
    Friend WithEvents cmbCostCentre As System.Windows.Forms.ComboBox
    Friend WithEvents dtpBillDate_OWN As BrighttechPack.DatePicker
    Friend WithEvents dtpTrandate As BrighttechPack.DatePicker
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtBillNo As System.Windows.Forms.TextBox
    Friend WithEvents BtnExcelDownload As System.Windows.Forms.Button
    Friend WithEvents ChkSale As System.Windows.Forms.CheckBox
    Friend WithEvents ChkPurchase As System.Windows.Forms.CheckBox
    Friend WithEvents btntemplate As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtTaxper_PER As System.Windows.Forms.TextBox
End Class
