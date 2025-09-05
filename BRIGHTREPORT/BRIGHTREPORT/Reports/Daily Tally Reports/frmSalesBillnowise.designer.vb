<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSalesBillnowise
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
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmbMetal = New System.Windows.Forms.ComboBox
        Me.btnExport = New System.Windows.Forms.Button
        Me.chkWithSR = New System.Windows.Forms.CheckBox
        Me.chkPcs = New System.Windows.Forms.CheckBox
        Me.chkGrsWt = New System.Windows.Forms.CheckBox
        Me.chkNetWt = New System.Windows.Forms.CheckBox
        Me.btnPrint = New System.Windows.Forms.Button
        Me.dtpFrom = New BrighttechPack.DatePicker(Me.components)
        Me.dtpTo = New BrighttechPack.DatePicker(Me.components)
        Me.chkCmbCostCentre = New BrighttechPack.CheckedComboBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.CmbCustomer = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.ChkSepPureMc = New System.Windows.Forms.CheckBox
        Me.cmbCategory = New BrighttechPack.CheckedComboBox
        Me.chkMC = New System.Windows.Forms.CheckBox
        Me.btn_dPrint = New System.Windows.Forms.Button
        Me.chkwithstustone = New System.Windows.Forms.CheckBox
        Me.chkDiscount = New System.Windows.Forms.CheckBox
        Me.GBmore = New System.Windows.Forms.GroupBox
        Me.Chkecs = New System.Windows.Forms.CheckBox
        Me.Chkparticular = New System.Windows.Forms.CheckBox
        Me.ChkStnWt = New System.Windows.Forms.CheckBox
        Me.ChkDIAwt = New System.Windows.Forms.CheckBox
        Me.Chkmore = New System.Windows.Forms.CheckBox
        Me.chkVA = New System.Windows.Forms.CheckBox
        Me.Panel2.SuspendLayout()
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlHeading.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GBmore.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.gridView)
        Me.Panel2.Controls.Add(Me.pnlHeading)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 132)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1028, 484)
        Me.Panel2.TabIndex = 1
        '
        'gridView
        '
        Me.gridView.AllowUserToAddRows = False
        Me.gridView.AllowUserToDeleteRows = False
        Me.gridView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridView.Location = New System.Drawing.Point(0, 25)
        Me.gridView.Name = "gridView"
        Me.gridView.ReadOnly = True
        Me.gridView.RowHeadersVisible = False
        Me.gridView.RowTemplate.Height = 21
        Me.gridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridView.Size = New System.Drawing.Size(1028, 459)
        Me.gridView.TabIndex = 0
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
        'ExtToolStripMenuItem
        '
        Me.ExtToolStripMenuItem.Name = "ExtToolStripMenuItem"
        Me.ExtToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExtToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ExtToolStripMenuItem.Text = "Exit"
        Me.ExtToolStripMenuItem.Visible = False
        '
        'btnView_Search
        '
        Me.btnView_Search.Location = New System.Drawing.Point(15, 99)
        Me.btnView_Search.Name = "btnView_Search"
        Me.btnView_Search.Size = New System.Drawing.Size(100, 30)
        Me.btnView_Search.TabIndex = 18
        Me.btnView_Search.Text = "&View"
        Me.btnView_Search.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(333, 99)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(100, 30)
        Me.btnExit.TabIndex = 21
        Me.btnExit.Text = "Exit [F12]"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(121, 100)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(100, 30)
        Me.btnNew.TabIndex = 19
        Me.btnNew.Text = "New [F3]"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(12, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 21)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Date From"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(12, 39)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(79, 21)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Cost Centre"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(12, 63)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(79, 21)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Metal"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(390, 69)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(60, 13)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "Category"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(199, 7)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(23, 21)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "To"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbMetal
        '
        Me.cmbMetal.FormattingEnabled = True
        Me.cmbMetal.Location = New System.Drawing.Point(95, 68)
        Me.cmbMetal.Name = "cmbMetal"
        Me.cmbMetal.Size = New System.Drawing.Size(277, 21)
        Me.cmbMetal.TabIndex = 15
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(227, 99)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 30)
        Me.btnExport.TabIndex = 20
        Me.btnExport.Text = "Export [X]"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'chkWithSR
        '
        Me.chkWithSR.AutoSize = True
        Me.chkWithSR.Location = New System.Drawing.Point(795, 104)
        Me.chkWithSR.Name = "chkWithSR"
        Me.chkWithSR.Size = New System.Drawing.Size(128, 17)
        Me.chkWithSR.TabIndex = 25
        Me.chkWithSR.Text = "&With Sales Return"
        Me.chkWithSR.UseVisualStyleBackColor = True
        Me.chkWithSR.Visible = False
        '
        'chkPcs
        '
        Me.chkPcs.AutoSize = True
        Me.chkPcs.Checked = True
        Me.chkPcs.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPcs.Location = New System.Drawing.Point(331, 9)
        Me.chkPcs.Name = "chkPcs"
        Me.chkPcs.Size = New System.Drawing.Size(62, 17)
        Me.chkPcs.TabIndex = 4
        Me.chkPcs.Text = "Pieces"
        Me.chkPcs.UseVisualStyleBackColor = True
        Me.chkPcs.Visible = False
        '
        'chkGrsWt
        '
        Me.chkGrsWt.AutoSize = True
        Me.chkGrsWt.Checked = True
        Me.chkGrsWt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkGrsWt.Location = New System.Drawing.Point(399, 9)
        Me.chkGrsWt.Name = "chkGrsWt"
        Me.chkGrsWt.Size = New System.Drawing.Size(74, 17)
        Me.chkGrsWt.TabIndex = 5
        Me.chkGrsWt.Text = "GrossWt"
        Me.chkGrsWt.UseVisualStyleBackColor = True
        Me.chkGrsWt.Visible = False
        '
        'chkNetWt
        '
        Me.chkNetWt.AutoSize = True
        Me.chkNetWt.Checked = True
        Me.chkNetWt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkNetWt.Location = New System.Drawing.Point(479, 9)
        Me.chkNetWt.Name = "chkNetWt"
        Me.chkNetWt.Size = New System.Drawing.Size(60, 17)
        Me.chkNetWt.TabIndex = 6
        Me.chkNetWt.Text = "NetWt"
        Me.chkNetWt.UseVisualStyleBackColor = True
        Me.chkNetWt.Visible = False
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(439, 99)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 22
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(95, 9)
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
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(229, 9)
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
        'chkCmbCostCentre
        '
        Me.chkCmbCostCentre.CheckOnClick = True
        Me.chkCmbCostCentre.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.chkCmbCostCentre.DropDownHeight = 1
        Me.chkCmbCostCentre.FormattingEnabled = True
        Me.chkCmbCostCentre.IntegralHeight = False
        Me.chkCmbCostCentre.Location = New System.Drawing.Point(95, 40)
        Me.chkCmbCostCentre.Name = "chkCmbCostCentre"
        Me.chkCmbCostCentre.Size = New System.Drawing.Size(277, 22)
        Me.chkCmbCostCentre.TabIndex = 10
        Me.chkCmbCostCentre.ValueSeparator = ", "
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.CmbCustomer)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.ChkSepPureMc)
        Me.Panel1.Controls.Add(Me.cmbCategory)
        Me.Panel1.Controls.Add(Me.chkMC)
        Me.Panel1.Controls.Add(Me.btn_dPrint)
        Me.Panel1.Controls.Add(Me.chkwithstustone)
        Me.Panel1.Controls.Add(Me.chkDiscount)
        Me.Panel1.Controls.Add(Me.GBmore)
        Me.Panel1.Controls.Add(Me.Chkmore)
        Me.Panel1.Controls.Add(Me.chkVA)
        Me.Panel1.Controls.Add(Me.chkCmbCostCentre)
        Me.Panel1.Controls.Add(Me.dtpTo)
        Me.Panel1.Controls.Add(Me.dtpFrom)
        Me.Panel1.Controls.Add(Me.btnPrint)
        Me.Panel1.Controls.Add(Me.chkNetWt)
        Me.Panel1.Controls.Add(Me.chkGrsWt)
        Me.Panel1.Controls.Add(Me.chkPcs)
        Me.Panel1.Controls.Add(Me.chkWithSR)
        Me.Panel1.Controls.Add(Me.btnExport)
        Me.Panel1.Controls.Add(Me.cmbMetal)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.btnNew)
        Me.Panel1.Controls.Add(Me.btnExit)
        Me.Panel1.Controls.Add(Me.btnView_Search)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1028, 132)
        Me.Panel1.TabIndex = 0
        '
        'CmbCustomer
        '
        Me.CmbCustomer.FormattingEnabled = True
        Me.CmbCustomer.Location = New System.Drawing.Point(463, 40)
        Me.CmbCustomer.Name = "CmbCustomer"
        Me.CmbCustomer.Size = New System.Drawing.Size(258, 21)
        Me.CmbCustomer.TabIndex = 12
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(388, 41)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(67, 21)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "Customer"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ChkSepPureMc
        '
        Me.ChkSepPureMc.AutoSize = True
        Me.ChkSepPureMc.Location = New System.Drawing.Point(655, 104)
        Me.ChkSepPureMc.Name = "ChkSepPureMc"
        Me.ChkSepPureMc.Size = New System.Drawing.Size(130, 17)
        Me.ChkSepPureMc.TabIndex = 24
        Me.ChkSepPureMc.Text = "Seperate Pure MC"
        Me.ChkSepPureMc.UseVisualStyleBackColor = True
        Me.ChkSepPureMc.Visible = False
        '
        'cmbCategory
        '
        Me.cmbCategory.CheckOnClick = True
        Me.cmbCategory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.cmbCategory.DropDownHeight = 1
        Me.cmbCategory.FormattingEnabled = True
        Me.cmbCategory.IntegralHeight = False
        Me.cmbCategory.Location = New System.Drawing.Point(463, 67)
        Me.cmbCategory.Name = "cmbCategory"
        Me.cmbCategory.Size = New System.Drawing.Size(383, 22)
        Me.cmbCategory.TabIndex = 17
        Me.cmbCategory.ValueSeparator = ", "
        '
        'chkMC
        '
        Me.chkMC.AutoSize = True
        Me.chkMC.Checked = True
        Me.chkMC.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMC.Location = New System.Drawing.Point(545, 9)
        Me.chkMC.Name = "chkMC"
        Me.chkMC.Size = New System.Drawing.Size(73, 17)
        Me.chkMC.TabIndex = 7
        Me.chkMC.Text = "With MC"
        Me.chkMC.UseVisualStyleBackColor = True
        Me.chkMC.Visible = False
        '
        'btn_dPrint
        '
        Me.btn_dPrint.Location = New System.Drawing.Point(925, 96)
        Me.btn_dPrint.Name = "btn_dPrint"
        Me.btn_dPrint.Size = New System.Drawing.Size(100, 30)
        Me.btn_dPrint.TabIndex = 28
        Me.btn_dPrint.Text = "Detail Print"
        Me.btn_dPrint.UseVisualStyleBackColor = True
        Me.btn_dPrint.Visible = False
        '
        'chkwithstustone
        '
        Me.chkwithstustone.AutoSize = True
        Me.chkwithstustone.Location = New System.Drawing.Point(737, 42)
        Me.chkwithstustone.Name = "chkwithstustone"
        Me.chkwithstustone.Size = New System.Drawing.Size(132, 17)
        Me.chkwithstustone.TabIndex = 13
        Me.chkwithstustone.Text = "With Studed Stone"
        Me.chkwithstustone.UseVisualStyleBackColor = True
        '
        'chkDiscount
        '
        Me.chkDiscount.AutoSize = True
        Me.chkDiscount.Location = New System.Drawing.Point(545, 107)
        Me.chkDiscount.Name = "chkDiscount"
        Me.chkDiscount.Size = New System.Drawing.Size(104, 17)
        Me.chkDiscount.TabIndex = 23
        Me.chkDiscount.Text = "&With Discount"
        Me.chkDiscount.UseVisualStyleBackColor = True
        Me.chkDiscount.Visible = False
        '
        'GBmore
        '
        Me.GBmore.Controls.Add(Me.Chkecs)
        Me.GBmore.Controls.Add(Me.Chkparticular)
        Me.GBmore.Controls.Add(Me.ChkStnWt)
        Me.GBmore.Controls.Add(Me.ChkDIAwt)
        Me.GBmore.Location = New System.Drawing.Point(937, 1)
        Me.GBmore.Name = "GBmore"
        Me.GBmore.Size = New System.Drawing.Size(88, 91)
        Me.GBmore.TabIndex = 27
        Me.GBmore.TabStop = False
        Me.GBmore.Visible = False
        '
        'Chkecs
        '
        Me.Chkecs.AutoSize = True
        Me.Chkecs.Location = New System.Drawing.Point(7, 65)
        Me.Chkecs.Name = "Chkecs"
        Me.Chkecs.Size = New System.Drawing.Size(50, 17)
        Me.Chkecs.TabIndex = 3
        Me.Chkecs.Text = "ECS"
        Me.Chkecs.UseVisualStyleBackColor = True
        Me.Chkecs.Visible = False
        '
        'Chkparticular
        '
        Me.Chkparticular.AutoSize = True
        Me.Chkparticular.Checked = True
        Me.Chkparticular.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Chkparticular.Location = New System.Drawing.Point(6, 46)
        Me.Chkparticular.Name = "Chkparticular"
        Me.Chkparticular.Size = New System.Drawing.Size(80, 17)
        Me.Chkparticular.TabIndex = 2
        Me.Chkparticular.Text = "Particular"
        Me.Chkparticular.UseVisualStyleBackColor = True
        '
        'ChkStnWt
        '
        Me.ChkStnWt.AutoSize = True
        Me.ChkStnWt.Checked = True
        Me.ChkStnWt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkStnWt.Location = New System.Drawing.Point(6, 10)
        Me.ChkStnWt.Name = "ChkStnWt"
        Me.ChkStnWt.Size = New System.Drawing.Size(64, 17)
        Me.ChkStnWt.TabIndex = 0
        Me.ChkStnWt.Text = "STNWt"
        Me.ChkStnWt.UseVisualStyleBackColor = True
        '
        'ChkDIAwt
        '
        Me.ChkDIAwt.AutoSize = True
        Me.ChkDIAwt.Checked = True
        Me.ChkDIAwt.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkDIAwt.Location = New System.Drawing.Point(6, 28)
        Me.ChkDIAwt.Name = "ChkDIAwt"
        Me.ChkDIAwt.Size = New System.Drawing.Size(63, 17)
        Me.ChkDIAwt.TabIndex = 1
        Me.ChkDIAwt.Text = "DIAWt"
        Me.ChkDIAwt.UseVisualStyleBackColor = True
        '
        'Chkmore
        '
        Me.Chkmore.AutoSize = True
        Me.Chkmore.Location = New System.Drawing.Point(869, 64)
        Me.Chkmore.Name = "Chkmore"
        Me.Chkmore.Size = New System.Drawing.Size(54, 17)
        Me.Chkmore.TabIndex = 26
        Me.Chkmore.Text = "More"
        Me.Chkmore.UseVisualStyleBackColor = True
        Me.Chkmore.Visible = False
        '
        'chkVA
        '
        Me.chkVA.AutoSize = True
        Me.chkVA.Location = New System.Drawing.Point(679, 9)
        Me.chkVA.Name = "chkVA"
        Me.chkVA.Size = New System.Drawing.Size(257, 17)
        Me.chkVA.TabIndex = 8
        Me.chkVA.Text = "With &Vertical Address && Payment Details"
        Me.chkVA.UseVisualStyleBackColor = True
        Me.chkVA.Visible = False
        '
        'frmSalesBillnowise
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
        Me.Name = "frmSalesBillnowise"
        Me.Text = "SALES ABSTRACT REPORT"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel2.ResumeLayout(False)
        CType(Me.gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlHeading.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GBmore.ResumeLayout(False)
        Me.GBmore.PerformLayout()
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
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbMetal As System.Windows.Forms.ComboBox
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents chkWithSR As System.Windows.Forms.CheckBox
    Friend WithEvents chkPcs As System.Windows.Forms.CheckBox
    Friend WithEvents chkGrsWt As System.Windows.Forms.CheckBox
    Friend WithEvents chkNetWt As System.Windows.Forms.CheckBox
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents dtpFrom As BrighttechPack.DatePicker
    Friend WithEvents dtpTo As BrighttechPack.DatePicker
    Friend WithEvents chkCmbCostCentre As BrighttechPack.CheckedComboBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents chkVA As System.Windows.Forms.CheckBox
    Friend WithEvents ChkStnWt As System.Windows.Forms.CheckBox
    Friend WithEvents ChkDIAwt As System.Windows.Forms.CheckBox
    Friend WithEvents GBmore As System.Windows.Forms.GroupBox
    Friend WithEvents Chkparticular As System.Windows.Forms.CheckBox
    Friend WithEvents Chkmore As System.Windows.Forms.CheckBox
    Friend WithEvents Chkecs As System.Windows.Forms.CheckBox
    Friend WithEvents chkDiscount As System.Windows.Forms.CheckBox
    Friend WithEvents btn_dPrint As System.Windows.Forms.Button
    Friend WithEvents chkwithstustone As System.Windows.Forms.CheckBox
    Friend WithEvents chkMC As System.Windows.Forms.CheckBox
    Friend WithEvents cmbCategory As BrighttechPack.CheckedComboBox
    Friend WithEvents ChkSepPureMc As System.Windows.Forms.CheckBox
    Friend WithEvents CmbCustomer As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
End Class
