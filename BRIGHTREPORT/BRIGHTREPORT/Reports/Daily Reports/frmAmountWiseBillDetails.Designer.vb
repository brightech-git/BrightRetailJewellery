<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAmountWiseBillDetails
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
        Me.chkOnlyPanBill = New System.Windows.Forms.CheckBox()
        Me.chkAdvDetail = New System.Windows.Forms.CheckBox()
        Me.ChkSummary = New System.Windows.Forms.CheckBox()
        Me.ChkMobile = New System.Windows.Forms.CheckBox()
        Me.ChkWithCat = New System.Windows.Forms.CheckBox()
        Me.grpRptType = New System.Windows.Forms.GroupBox()
        Me.rbtnPurchase = New System.Windows.Forms.RadioButton()
        Me.rbtnSales = New System.Windows.Forms.RadioButton()
        Me.cmbCompany = New BrighttechPack.CheckedComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnexit = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnview = New System.Windows.Forms.Button()
        Me.btnnew = New System.Windows.Forms.Button()
        Me.txtToAmt_AMT = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtFrmAmt_AMT = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dtpToDate = New BrighttechPack.DatePicker(Me.components)
        Me.dtpFrmDate = New BrighttechPack.DatePicker(Me.components)
        Me.lblEditDate = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.DGrid = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AutoResizeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlHeading = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.pnlTop.SuspendLayout()
        Me.grpRptType.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.DGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.pnlHeading.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlTop
        '
        Me.pnlTop.Controls.Add(Me.chkOnlyPanBill)
        Me.pnlTop.Controls.Add(Me.chkAdvDetail)
        Me.pnlTop.Controls.Add(Me.ChkSummary)
        Me.pnlTop.Controls.Add(Me.ChkMobile)
        Me.pnlTop.Controls.Add(Me.ChkWithCat)
        Me.pnlTop.Controls.Add(Me.grpRptType)
        Me.pnlTop.Controls.Add(Me.cmbCompany)
        Me.pnlTop.Controls.Add(Me.Label5)
        Me.pnlTop.Controls.Add(Me.chkCmbCostCentre)
        Me.pnlTop.Controls.Add(Me.Label4)
        Me.pnlTop.Controls.Add(Me.btnPrint)
        Me.pnlTop.Controls.Add(Me.btnexit)
        Me.pnlTop.Controls.Add(Me.btnExport)
        Me.pnlTop.Controls.Add(Me.btnview)
        Me.pnlTop.Controls.Add(Me.btnnew)
        Me.pnlTop.Controls.Add(Me.txtToAmt_AMT)
        Me.pnlTop.Controls.Add(Me.Label3)
        Me.pnlTop.Controls.Add(Me.txtFrmAmt_AMT)
        Me.pnlTop.Controls.Add(Me.Label2)
        Me.pnlTop.Controls.Add(Me.Label1)
        Me.pnlTop.Controls.Add(Me.dtpToDate)
        Me.pnlTop.Controls.Add(Me.dtpFrmDate)
        Me.pnlTop.Controls.Add(Me.lblEditDate)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(1028, 111)
        Me.pnlTop.TabIndex = 0
        '
        'chkOnlyPanBill
        '
        Me.chkOnlyPanBill.AutoSize = True
        Me.chkOnlyPanBill.Location = New System.Drawing.Point(627, 58)
        Me.chkOnlyPanBill.Name = "chkOnlyPanBill"
        Me.chkOnlyPanBill.Size = New System.Drawing.Size(98, 17)
        Me.chkOnlyPanBill.TabIndex = 17
        Me.chkOnlyPanBill.Text = "Pan Bill Only"
        Me.chkOnlyPanBill.UseVisualStyleBackColor = True
        '
        'chkAdvDetail
        '
        Me.chkAdvDetail.AutoSize = True
        Me.chkAdvDetail.Location = New System.Drawing.Point(627, 34)
        Me.chkAdvDetail.Name = "chkAdvDetail"
        Me.chkAdvDetail.Size = New System.Drawing.Size(141, 17)
        Me.chkAdvDetail.TabIndex = 16
        Me.chkAdvDetail.Text = "With Advance Detail"
        Me.chkAdvDetail.UseVisualStyleBackColor = True
        '
        'ChkSummary
        '
        Me.ChkSummary.AutoSize = True
        Me.ChkSummary.Location = New System.Drawing.Point(627, 5)
        Me.ChkSummary.Name = "ChkSummary"
        Me.ChkSummary.Size = New System.Drawing.Size(147, 17)
        Me.ChkSummary.TabIndex = 14
        Me.ChkSummary.Text = "Summarywise Mobile"
        Me.ChkSummary.UseVisualStyleBackColor = True
        '
        'ChkMobile
        '
        Me.ChkMobile.AutoSize = True
        Me.ChkMobile.Location = New System.Drawing.Point(501, 6)
        Me.ChkMobile.Name = "ChkMobile"
        Me.ChkMobile.Size = New System.Drawing.Size(120, 17)
        Me.ChkMobile.TabIndex = 13
        Me.ChkMobile.Text = "Group By Mobile"
        Me.ChkMobile.UseVisualStyleBackColor = True
        '
        'ChkWithCat
        '
        Me.ChkWithCat.AutoSize = True
        Me.ChkWithCat.Location = New System.Drawing.Point(348, 7)
        Me.ChkWithCat.Name = "ChkWithCat"
        Me.ChkWithCat.Size = New System.Drawing.Size(136, 17)
        Me.ChkWithCat.TabIndex = 12
        Me.ChkWithCat.Text = "Group by Category"
        Me.ChkWithCat.UseVisualStyleBackColor = True
        '
        'grpRptType
        '
        Me.grpRptType.Controls.Add(Me.rbtnPurchase)
        Me.grpRptType.Controls.Add(Me.rbtnSales)
        Me.grpRptType.Location = New System.Drawing.Point(340, 25)
        Me.grpRptType.Name = "grpRptType"
        Me.grpRptType.Size = New System.Drawing.Size(180, 34)
        Me.grpRptType.TabIndex = 15
        Me.grpRptType.TabStop = False
        '
        'rbtnPurchase
        '
        Me.rbtnPurchase.AutoSize = True
        Me.rbtnPurchase.Location = New System.Drawing.Point(87, 12)
        Me.rbtnPurchase.Name = "rbtnPurchase"
        Me.rbtnPurchase.Size = New System.Drawing.Size(77, 17)
        Me.rbtnPurchase.TabIndex = 1
        Me.rbtnPurchase.Text = "Purchase"
        Me.rbtnPurchase.UseVisualStyleBackColor = True
        '
        'rbtnSales
        '
        Me.rbtnSales.AutoSize = True
        Me.rbtnSales.Checked = True
        Me.rbtnSales.Location = New System.Drawing.Point(9, 11)
        Me.rbtnSales.Name = "rbtnSales"
        Me.rbtnSales.Size = New System.Drawing.Size(56, 17)
        Me.rbtnSales.TabIndex = 0
        Me.rbtnSales.TabStop = True
        Me.rbtnSales.Text = "Sales"
        Me.rbtnSales.UseVisualStyleBackColor = True
        '
        'cmbCompany
        '
        Me.cmbCompany.CheckOnClick = True
        Me.cmbCompany.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbCompany.DropDownHeight = 1
        Me.cmbCompany.FormattingEnabled = True
        Me.cmbCompany.IntegralHeight = False
        Me.cmbCompany.Location = New System.Drawing.Point(94, 31)
        Me.cmbCompany.Name = "cmbCompany"
        Me.cmbCompany.Size = New System.Drawing.Size(240, 22)
        Me.cmbCompany.TabIndex = 5
        Me.cmbCompany.ValueSeparator = ", "
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(7, 36)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(62, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Company"
        '
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(94, 56)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(240, 22)
        Me.chkCmbCostCentre.TabIndex = 7
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(7, 60)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(76, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Cost Centre"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(632, 76)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(98, 30)
        Me.btnPrint.TabIndex = 21
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnexit
        '
        Me.btnexit.Location = New System.Drawing.Point(730, 76)
        Me.btnexit.Name = "btnexit"
        Me.btnexit.Size = New System.Drawing.Size(98, 30)
        Me.btnexit.TabIndex = 22
        Me.btnexit.Text = "Exit[F12]"
        Me.btnexit.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(534, 76)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(98, 30)
        Me.btnExport.TabIndex = 20
        Me.btnExport.Text = "Export[X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnview
        '
        Me.btnview.Location = New System.Drawing.Point(338, 76)
        Me.btnview.Name = "btnview"
        Me.btnview.Size = New System.Drawing.Size(98, 30)
        Me.btnview.TabIndex = 18
        Me.btnview.Text = "&View"
        Me.btnview.UseVisualStyleBackColor = True
        '
        'btnnew
        '
        Me.btnnew.Location = New System.Drawing.Point(436, 76)
        Me.btnnew.Name = "btnnew"
        Me.btnnew.Size = New System.Drawing.Size(98, 30)
        Me.btnnew.TabIndex = 19
        Me.btnnew.Text = "New[F3]"
        Me.btnnew.UseVisualStyleBackColor = True
        '
        'txtToAmt_AMT
        '
        Me.txtToAmt_AMT.Location = New System.Drawing.Point(234, 81)
        Me.txtToAmt_AMT.Name = "txtToAmt_AMT"
        Me.txtToAmt_AMT.Size = New System.Drawing.Size(100, 21)
        Me.txtToAmt_AMT.TabIndex = 11
        Me.txtToAmt_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(202, 85)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(21, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "To"
        '
        'txtFrmAmt_AMT
        '
        Me.txtFrmAmt_AMT.Location = New System.Drawing.Point(94, 81)
        Me.txtFrmAmt_AMT.Name = "txtFrmAmt_AMT"
        Me.txtFrmAmt_AMT.Size = New System.Drawing.Size(100, 21)
        Me.txtFrmAmt_AMT.TabIndex = 9
        Me.txtFrmAmt_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(7, 85)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Amount From"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date From"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtpToDate
        '
        Me.dtpToDate.Location = New System.Drawing.Point(248, 6)
        Me.dtpToDate.Mask = "##/##/####"
        Me.dtpToDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpToDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpToDate.Name = "dtpToDate"
        Me.dtpToDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpToDate.Size = New System.Drawing.Size(86, 21)
        Me.dtpToDate.TabIndex = 3
        Me.dtpToDate.Text = "07/03/9998"
        Me.dtpToDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpFrmDate
        '
        Me.dtpFrmDate.Location = New System.Drawing.Point(94, 6)
        Me.dtpFrmDate.Mask = "##/##/####"
        Me.dtpFrmDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpFrmDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpFrmDate.Name = "dtpFrmDate"
        Me.dtpFrmDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpFrmDate.Size = New System.Drawing.Size(86, 21)
        Me.dtpFrmDate.TabIndex = 1
        Me.dtpFrmDate.Text = "07/03/9998"
        Me.dtpFrmDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'lblEditDate
        '
        Me.lblEditDate.AutoSize = True
        Me.lblEditDate.Location = New System.Drawing.Point(193, 9)
        Me.lblEditDate.Name = "lblEditDate"
        Me.lblEditDate.Size = New System.Drawing.Size(52, 13)
        Me.lblEditDate.TabIndex = 2
        Me.lblEditDate.Text = "Date To"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.DGrid)
        Me.Panel1.Controls.Add(Me.pnlHeading)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 111)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1028, 467)
        Me.Panel1.TabIndex = 1
        '
        'DGrid
        '
        Me.DGrid.AllowUserToAddRows = False
        Me.DGrid.AllowUserToDeleteRows = False
        Me.DGrid.AllowUserToResizeColumns = False
        Me.DGrid.AllowUserToResizeRows = False
        Me.DGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGrid.ContextMenuStrip = Me.ContextMenuStrip1
        Me.DGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DGrid.Location = New System.Drawing.Point(0, 25)
        Me.DGrid.Name = "DGrid"
        Me.DGrid.ReadOnly = True
        Me.DGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DGrid.Size = New System.Drawing.Size(1028, 442)
        Me.DGrid.TabIndex = 1
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AutoResizeToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(136, 26)
        '
        'AutoResizeToolStripMenuItem
        '
        Me.AutoResizeToolStripMenuItem.CheckOnClick = True
        Me.AutoResizeToolStripMenuItem.Name = "AutoResizeToolStripMenuItem"
        Me.AutoResizeToolStripMenuItem.Size = New System.Drawing.Size(135, 22)
        Me.AutoResizeToolStripMenuItem.Text = "Auto Resize"
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
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(349, 9)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(0, 13)
        Me.lblTitle.TabIndex = 0
        '
        'frmAmountWiseBillDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 578)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.pnlTop)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmAmountWiseBillDetails"
        Me.Text = "Amount Wise Sales"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        Me.grpRptType.ResumeLayout(False)
        Me.grpRptType.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        CType(Me.DGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.pnlHeading.ResumeLayout(False)
        Me.pnlHeading.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents txtToAmt_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtFrmAmt_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dtpToDate As BrighttechPack.DatePicker
    Friend WithEvents dtpFrmDate As BrighttechPack.DatePicker
    Friend WithEvents lblEditDate As System.Windows.Forms.Label
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnexit As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnview As System.Windows.Forms.Button
    Friend WithEvents btnnew As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents pnlHeading As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents DGrid As System.Windows.Forms.DataGridView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AutoResizeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmbCompany As BrighttechPack.CheckedComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents grpRptType As System.Windows.Forms.GroupBox
    Friend WithEvents rbtnPurchase As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnSales As System.Windows.Forms.RadioButton
    Friend WithEvents ChkWithCat As System.Windows.Forms.CheckBox
    Friend WithEvents ChkMobile As System.Windows.Forms.CheckBox
    Friend WithEvents ChkSummary As System.Windows.Forms.CheckBox
    Friend WithEvents chkAdvDetail As CheckBox
    Friend WithEvents chkOnlyPanBill As CheckBox
End Class
