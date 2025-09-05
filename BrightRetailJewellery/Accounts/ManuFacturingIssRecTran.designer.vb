<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ManuFacturingIssRecTran
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
        Me.Grouper2 = New CodeVendor.Controls.Grouper()
        Me.DgvTranTotal = New System.Windows.Forms.DataGridView()
        Me.DgvTran = New System.Windows.Forms.DataGridView()
        Me.grpHeader = New CodeVendor.Controls.Grouper()
        Me.lblRunno = New System.Windows.Forms.Label()
        Me.lblProcessDateTime = New System.Windows.Forms.Label()
        Me.lblOrdNo = New System.Windows.Forms.Label()
        Me.txtOrdNo = New System.Windows.Forms.TextBox()
        Me.lblAddress1 = New System.Windows.Forms.Label()
        Me.lblBalance = New System.Windows.Forms.Label()
        Me.lblAddress = New System.Windows.Forms.Label()
        Me.cmbAcName = New System.Windows.Forms.ComboBox()
        Me.cmbCostCentre = New System.Windows.Forms.ComboBox()
        Me.cmbTransactionType = New System.Windows.Forms.ComboBox()
        Me.dtpBillDate_OWN = New BrighttechPack.DatePicker(Me.components)
        Me.dtpTrandate = New BrighttechPack.DatePicker(Me.components)
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txtBillNo = New System.Windows.Forms.TextBox()
        Me.Grouper1 = New CodeVendor.Controls.Grouper()
        Me.tabGroup = New System.Windows.Forms.TabControl()
        Me.TabGridView = New System.Windows.Forms.TabPage()
        Me.dgvOrderDet = New System.Windows.Forms.DataGridView()
        Me.TabPicture = New System.Windows.Forms.TabPage()
        Me.picOrder = New System.Windows.Forms.PictureBox()
        Me.lblLot = New System.Windows.Forms.Label()
        Me.gboxOrder = New System.Windows.Forms.GroupBox()
        Me.chkOrder = New System.Windows.Forms.CheckBox()
        Me.gboxBulk = New System.Windows.Forms.GroupBox()
        Me.chkMulti = New System.Windows.Forms.CheckBox()
        Me.chkBulk = New System.Windows.Forms.CheckBox()
        Me.dtCatBalance = New System.Windows.Forms.DataGridView()
        Me.btnView = New System.Windows.Forms.Button()
        Me.dtpCreditDays = New BrighttechPack.DatePicker(Me.components)
        Me.txtCreditDays_NUM = New System.Windows.Forms.TextBox()
        Me.txtRemark2 = New System.Windows.Forms.TextBox()
        Me.txtRemark1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label61 = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.txtAdditionalCharges = New System.Windows.Forms.TextBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtAdjRoundOff_AMT = New System.Windows.Forms.TextBox()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.txtAdjCredit_AMT = New System.Windows.Forms.TextBox()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.txtAdjCash_AMT = New System.Windows.Forms.TextBox()
        Me.txtAdjCheque_AMT = New System.Windows.Forms.TextBox()
        Me.txtAdjReceive_AMT = New System.Windows.Forms.TextBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.pnlLeft = New System.Windows.Forms.Panel()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Panel1.SuspendLayout()
        Me.Grouper2.SuspendLayout()
        CType(Me.DgvTranTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DgvTran, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpHeader.SuspendLayout()
        Me.Grouper1.SuspendLayout()
        Me.tabGroup.SuspendLayout()
        Me.TabGridView.SuspendLayout()
        CType(Me.dgvOrderDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPicture.SuspendLayout()
        CType(Me.picOrder, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gboxOrder.SuspendLayout()
        Me.gboxBulk.SuspendLayout()
        CType(Me.dtCatBalance, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.Grouper2.Size = New System.Drawing.Size(1000, 320)
        Me.Grouper2.TabIndex = 21
        '
        'DgvTranTotal
        '
        Me.DgvTranTotal.AllowUserToAddRows = False
        Me.DgvTranTotal.AllowUserToDeleteRows = False
        Me.DgvTranTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DgvTranTotal.ColumnHeadersVisible = False
        Me.DgvTranTotal.Enabled = False
        Me.DgvTranTotal.Location = New System.Drawing.Point(9, 292)
        Me.DgvTranTotal.Name = "DgvTranTotal"
        Me.DgvTranTotal.ReadOnly = True
        Me.DgvTranTotal.RowHeadersVisible = False
        Me.DgvTranTotal.Size = New System.Drawing.Size(981, 19)
        Me.DgvTranTotal.TabIndex = 37
        '
        'DgvTran
        '
        Me.DgvTran.AllowUserToAddRows = False
        Me.DgvTran.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DgvTran.Location = New System.Drawing.Point(9, 19)
        Me.DgvTran.Name = "DgvTran"
        Me.DgvTran.ReadOnly = True
        Me.DgvTran.RowHeadersVisible = False
        Me.DgvTran.Size = New System.Drawing.Size(981, 273)
        Me.DgvTran.TabIndex = 0
        '
        'grpHeader
        '
        Me.grpHeader.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpHeader.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpHeader.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpHeader.BorderColor = System.Drawing.Color.Transparent
        Me.grpHeader.BorderThickness = 1.0!
        Me.grpHeader.Controls.Add(Me.lblRunno)
        Me.grpHeader.Controls.Add(Me.lblProcessDateTime)
        Me.grpHeader.Controls.Add(Me.lblOrdNo)
        Me.grpHeader.Controls.Add(Me.txtOrdNo)
        Me.grpHeader.Controls.Add(Me.lblAddress1)
        Me.grpHeader.Controls.Add(Me.lblBalance)
        Me.grpHeader.Controls.Add(Me.lblAddress)
        Me.grpHeader.Controls.Add(Me.cmbAcName)
        Me.grpHeader.Controls.Add(Me.cmbCostCentre)
        Me.grpHeader.Controls.Add(Me.cmbTransactionType)
        Me.grpHeader.Controls.Add(Me.dtpBillDate_OWN)
        Me.grpHeader.Controls.Add(Me.dtpTrandate)
        Me.grpHeader.Controls.Add(Me.lblTitle)
        Me.grpHeader.Controls.Add(Me.Label16)
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
        Me.grpHeader.TabIndex = 0
        '
        'lblRunno
        '
        Me.lblRunno.AutoSize = True
        Me.lblRunno.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblRunno.ForeColor = System.Drawing.Color.Red
        Me.lblRunno.Location = New System.Drawing.Point(17, 35)
        Me.lblRunno.Name = "lblRunno"
        Me.lblRunno.Size = New System.Drawing.Size(48, 14)
        Me.lblRunno.TabIndex = 19
        Me.lblRunno.Text = "Runno"
        Me.lblRunno.Visible = False
        '
        'lblProcessDateTime
        '
        Me.lblProcessDateTime.AutoSize = True
        Me.lblProcessDateTime.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblProcessDateTime.Location = New System.Drawing.Point(256, 81)
        Me.lblProcessDateTime.Name = "lblProcessDateTime"
        Me.lblProcessDateTime.Size = New System.Drawing.Size(126, 14)
        Me.lblProcessDateTime.TabIndex = 18
        Me.lblProcessDateTime.Text = "Process DateTime"
        Me.lblProcessDateTime.Visible = False
        '
        'lblOrdNo
        '
        Me.lblOrdNo.AutoSize = True
        Me.lblOrdNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblOrdNo.Location = New System.Drawing.Point(12, 81)
        Me.lblOrdNo.Name = "lblOrdNo"
        Me.lblOrdNo.Size = New System.Drawing.Size(68, 14)
        Me.lblOrdNo.TabIndex = 3
        Me.lblOrdNo.Text = "Order No"
        Me.lblOrdNo.Visible = False
        '
        'txtOrdNo
        '
        Me.txtOrdNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.txtOrdNo.Location = New System.Drawing.Point(123, 78)
        Me.txtOrdNo.MaxLength = 20
        Me.txtOrdNo.Name = "txtOrdNo"
        Me.txtOrdNo.Size = New System.Drawing.Size(94, 22)
        Me.txtOrdNo.TabIndex = 4
        Me.txtOrdNo.Visible = False
        '
        'lblAddress1
        '
        Me.lblAddress1.AutoSize = True
        Me.lblAddress1.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddress1.Location = New System.Drawing.Point(558, 90)
        Me.lblAddress1.Name = "lblAddress1"
        Me.lblAddress1.Size = New System.Drawing.Size(47, 12)
        Me.lblAddress1.TabIndex = 16
        Me.lblAddress1.Text = "Address"
        '
        'lblBalance
        '
        Me.lblBalance.AutoSize = True
        Me.lblBalance.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblBalance.Location = New System.Drawing.Point(891, 35)
        Me.lblBalance.Name = "lblBalance"
        Me.lblBalance.Size = New System.Drawing.Size(59, 14)
        Me.lblBalance.TabIndex = 17
        Me.lblBalance.Text = "Balance"
        Me.lblBalance.Visible = False
        '
        'lblAddress
        '
        Me.lblAddress.AutoSize = True
        Me.lblAddress.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddress.Location = New System.Drawing.Point(558, 77)
        Me.lblAddress.Name = "lblAddress"
        Me.lblAddress.Size = New System.Drawing.Size(47, 12)
        Me.lblAddress.TabIndex = 15
        Me.lblAddress.Text = "Address"
        '
        'cmbAcName
        '
        Me.cmbAcName.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbAcName.FormattingEnabled = True
        Me.cmbAcName.Location = New System.Drawing.Point(555, 52)
        Me.cmbAcName.Name = "cmbAcName"
        Me.cmbAcName.Size = New System.Drawing.Size(395, 22)
        Me.cmbAcName.TabIndex = 6
        '
        'cmbCostCentre
        '
        Me.cmbCostCentre.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbCostCentre.FormattingEnabled = True
        Me.cmbCostCentre.Location = New System.Drawing.Point(123, 51)
        Me.cmbCostCentre.Name = "cmbCostCentre"
        Me.cmbCostCentre.Size = New System.Drawing.Size(355, 22)
        Me.cmbCostCentre.TabIndex = 2
        '
        'cmbTransactionType
        '
        Me.cmbTransactionType.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.cmbTransactionType.FormattingEnabled = True
        Me.cmbTransactionType.Location = New System.Drawing.Point(760, 107)
        Me.cmbTransactionType.Name = "cmbTransactionType"
        Me.cmbTransactionType.Size = New System.Drawing.Size(190, 22)
        Me.cmbTransactionType.TabIndex = 14
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
        Me.dtpBillDate_OWN.Size = New System.Drawing.Size(94, 22)
        Me.dtpBillDate_OWN.TabIndex = 8
        Me.dtpBillDate_OWN.Text = "08-08-2012"
        Me.dtpBillDate_OWN.Value = New Date(2012, 8, 8, 0, 0, 0, 0)
        '
        'dtpTrandate
        '
        Me.dtpTrandate.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.dtpTrandate.Location = New System.Drawing.Point(561, 105)
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
        Me.lblTitle.Text = "MATERIAL ISSUE && RECEIPT"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label16.Location = New System.Drawing.Point(683, 110)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(73, 14)
        Me.Label16.TabIndex = 13
        Me.Label16.Text = "&Tran Type"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label20.Location = New System.Drawing.Point(256, 110)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(50, 14)
        Me.Label20.TabIndex = 9
        Me.Label20.Text = "Bill No"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label19.Location = New System.Drawing.Point(12, 105)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(63, 14)
        Me.Label19.TabIndex = 7
        Me.Label19.Text = "Bill Date"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label18.Location = New System.Drawing.Point(477, 110)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(72, 14)
        Me.Label18.TabIndex = 11
        Me.Label18.Text = "Tran Date"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label17.Location = New System.Drawing.Point(484, 55)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(65, 14)
        Me.Label17.TabIndex = 5
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
        Me.txtBillNo.Location = New System.Drawing.Point(310, 105)
        Me.txtBillNo.MaxLength = 20
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
        Me.Grouper1.Controls.Add(Me.tabGroup)
        Me.Grouper1.Controls.Add(Me.lblLot)
        Me.Grouper1.Controls.Add(Me.gboxOrder)
        Me.Grouper1.Controls.Add(Me.gboxBulk)
        Me.Grouper1.Controls.Add(Me.dtCatBalance)
        Me.Grouper1.Controls.Add(Me.btnView)
        Me.Grouper1.Controls.Add(Me.dtpCreditDays)
        Me.Grouper1.Controls.Add(Me.txtCreditDays_NUM)
        Me.Grouper1.Controls.Add(Me.txtRemark2)
        Me.Grouper1.Controls.Add(Me.txtRemark1)
        Me.Grouper1.Controls.Add(Me.Label1)
        Me.Grouper1.Controls.Add(Me.Label61)
        Me.Grouper1.Controls.Add(Me.btnSave)
        Me.Grouper1.Controls.Add(Me.btnExit)
        Me.Grouper1.Controls.Add(Me.btnNew)
        Me.Grouper1.Controls.Add(Me.txtAdditionalCharges)
        Me.Grouper1.Controls.Add(Me.Label26)
        Me.Grouper1.Controls.Add(Me.Label9)
        Me.Grouper1.Controls.Add(Me.txtAdjRoundOff_AMT)
        Me.Grouper1.Controls.Add(Me.Label43)
        Me.Grouper1.Controls.Add(Me.Label38)
        Me.Grouper1.Controls.Add(Me.txtAdjCredit_AMT)
        Me.Grouper1.Controls.Add(Me.Label39)
        Me.Grouper1.Controls.Add(Me.Label42)
        Me.Grouper1.Controls.Add(Me.txtAdjCash_AMT)
        Me.Grouper1.Controls.Add(Me.txtAdjCheque_AMT)
        Me.Grouper1.Controls.Add(Me.txtAdjReceive_AMT)
        Me.Grouper1.CustomGroupBoxColor = System.Drawing.Color.White
        Me.Grouper1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Grouper1.GroupImage = Nothing
        Me.Grouper1.GroupTitle = ""
        Me.Grouper1.Location = New System.Drawing.Point(10, 452)
        Me.Grouper1.Name = "Grouper1"
        Me.Grouper1.Padding = New System.Windows.Forms.Padding(20)
        Me.Grouper1.PaintGroupBox = False
        Me.Grouper1.RoundCorners = 10
        Me.Grouper1.ShadowColor = System.Drawing.Color.DarkGray
        Me.Grouper1.ShadowControl = False
        Me.Grouper1.ShadowThickness = 3
        Me.Grouper1.Size = New System.Drawing.Size(1000, 174)
        Me.Grouper1.TabIndex = 2
        '
        'tabGroup
        '
        Me.tabGroup.Controls.Add(Me.TabGridView)
        Me.tabGroup.Controls.Add(Me.TabPicture)
        Me.tabGroup.Location = New System.Drawing.Point(6, 8)
        Me.tabGroup.Name = "tabGroup"
        Me.tabGroup.SelectedIndex = 0
        Me.tabGroup.Size = New System.Drawing.Size(288, 162)
        Me.tabGroup.TabIndex = 27
        '
        'TabGridView
        '
        Me.TabGridView.Controls.Add(Me.dgvOrderDet)
        Me.TabGridView.Location = New System.Drawing.Point(4, 22)
        Me.TabGridView.Name = "TabGridView"
        Me.TabGridView.Padding = New System.Windows.Forms.Padding(3)
        Me.TabGridView.Size = New System.Drawing.Size(280, 136)
        Me.TabGridView.TabIndex = 0
        Me.TabGridView.Text = "GridView"
        Me.TabGridView.UseVisualStyleBackColor = True
        '
        'dgvOrderDet
        '
        Me.dgvOrderDet.AllowUserToAddRows = False
        Me.dgvOrderDet.AllowUserToDeleteRows = False
        Me.dgvOrderDet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvOrderDet.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvOrderDet.Location = New System.Drawing.Point(3, 3)
        Me.dgvOrderDet.Name = "dgvOrderDet"
        Me.dgvOrderDet.ReadOnly = True
        Me.dgvOrderDet.RowHeadersVisible = False
        Me.dgvOrderDet.Size = New System.Drawing.Size(274, 130)
        Me.dgvOrderDet.TabIndex = 25
        Me.dgvOrderDet.Visible = False
        '
        'TabPicture
        '
        Me.TabPicture.Controls.Add(Me.picOrder)
        Me.TabPicture.Location = New System.Drawing.Point(4, 22)
        Me.TabPicture.Name = "TabPicture"
        Me.TabPicture.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPicture.Size = New System.Drawing.Size(280, 136)
        Me.TabPicture.TabIndex = 1
        Me.TabPicture.Text = "Picture"
        Me.TabPicture.UseVisualStyleBackColor = True
        '
        'picOrder
        '
        Me.picOrder.Dock = System.Windows.Forms.DockStyle.Fill
        Me.picOrder.Location = New System.Drawing.Point(3, 3)
        Me.picOrder.Name = "picOrder"
        Me.picOrder.Size = New System.Drawing.Size(274, 130)
        Me.picOrder.TabIndex = 26
        Me.picOrder.TabStop = False
        Me.picOrder.Visible = False
        '
        'lblLot
        '
        Me.lblLot.AutoSize = True
        Me.lblLot.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblLot.Location = New System.Drawing.Point(425, 116)
        Me.lblLot.Name = "lblLot"
        Me.lblLot.Size = New System.Drawing.Size(265, 13)
        Me.lblLot.TabIndex = 24
        Me.lblLot.Text = "Ctrl+B - Bulk Tag Lot, Ctrl+O - Order Lot"
        '
        'gboxOrder
        '
        Me.gboxOrder.Controls.Add(Me.chkOrder)
        Me.gboxOrder.Location = New System.Drawing.Point(630, 31)
        Me.gboxOrder.Name = "gboxOrder"
        Me.gboxOrder.Size = New System.Drawing.Size(85, 35)
        Me.gboxOrder.TabIndex = 23
        Me.gboxOrder.TabStop = False
        Me.gboxOrder.Visible = False
        '
        'chkOrder
        '
        Me.chkOrder.AutoSize = True
        Me.chkOrder.Location = New System.Drawing.Point(6, 12)
        Me.chkOrder.Name = "chkOrder"
        Me.chkOrder.Size = New System.Drawing.Size(74, 17)
        Me.chkOrder.TabIndex = 0
        Me.chkOrder.Text = "Is Order"
        Me.chkOrder.UseVisualStyleBackColor = True
        '
        'gboxBulk
        '
        Me.gboxBulk.Controls.Add(Me.chkMulti)
        Me.gboxBulk.Controls.Add(Me.chkBulk)
        Me.gboxBulk.Location = New System.Drawing.Point(555, 27)
        Me.gboxBulk.Name = "gboxBulk"
        Me.gboxBulk.Size = New System.Drawing.Size(160, 39)
        Me.gboxBulk.TabIndex = 22
        Me.gboxBulk.TabStop = False
        Me.gboxBulk.Visible = False
        '
        'chkMulti
        '
        Me.chkMulti.AutoSize = True
        Me.chkMulti.Location = New System.Drawing.Point(75, 20)
        Me.chkMulti.Name = "chkMulti"
        Me.chkMulti.Size = New System.Drawing.Size(85, 17)
        Me.chkMulti.TabIndex = 1
        Me.chkMulti.Text = "Is Multitag"
        Me.chkMulti.UseVisualStyleBackColor = True
        '
        'chkBulk
        '
        Me.chkBulk.AutoSize = True
        Me.chkBulk.Location = New System.Drawing.Point(6, 20)
        Me.chkBulk.Name = "chkBulk"
        Me.chkBulk.Size = New System.Drawing.Size(66, 17)
        Me.chkBulk.TabIndex = 0
        Me.chkBulk.Text = "Is Bulk"
        Me.chkBulk.UseVisualStyleBackColor = True
        '
        'dtCatBalance
        '
        Me.dtCatBalance.AllowUserToAddRows = False
        Me.dtCatBalance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dtCatBalance.Location = New System.Drawing.Point(6, 20)
        Me.dtCatBalance.Name = "dtCatBalance"
        Me.dtCatBalance.ReadOnly = True
        Me.dtCatBalance.RowHeadersVisible = False
        Me.dtCatBalance.Size = New System.Drawing.Size(288, 146)
        Me.dtCatBalance.TabIndex = 21
        Me.dtCatBalance.Visible = False
        '
        'btnView
        '
        Me.btnView.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnView.Location = New System.Drawing.Point(510, 136)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(105, 30)
        Me.btnView.TabIndex = 21
        Me.btnView.Text = "&View"
        Me.btnView.UseVisualStyleBackColor = True
        '
        'dtpCreditDays
        '
        Me.dtpCreditDays.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.dtpCreditDays.Location = New System.Drawing.Point(455, 44)
        Me.dtpCreditDays.Mask = "##/##/####"
        Me.dtpCreditDays.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpCreditDays.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpCreditDays.Name = "dtpCreditDays"
        Me.dtpCreditDays.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpCreditDays.Size = New System.Drawing.Size(94, 22)
        Me.dtpCreditDays.TabIndex = 14
        Me.dtpCreditDays.Text = "06/03/9998"
        Me.dtpCreditDays.Value = New Date(9998, 3, 6, 0, 0, 0, 0)
        '
        'txtCreditDays_NUM
        '
        Me.txtCreditDays_NUM.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold)
        Me.txtCreditDays_NUM.Location = New System.Drawing.Point(388, 44)
        Me.txtCreditDays_NUM.Name = "txtCreditDays_NUM"
        Me.txtCreditDays_NUM.Size = New System.Drawing.Size(64, 21)
        Me.txtCreditDays_NUM.TabIndex = 13
        Me.txtCreditDays_NUM.Text = "45"
        '
        'txtRemark2
        '
        Me.txtRemark2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold)
        Me.txtRemark2.Location = New System.Drawing.Point(388, 92)
        Me.txtRemark2.MaxLength = 50
        Me.txtRemark2.Name = "txtRemark2"
        Me.txtRemark2.Size = New System.Drawing.Size(327, 21)
        Me.txtRemark2.TabIndex = 17
        '
        'txtRemark1
        '
        Me.txtRemark1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold)
        Me.txtRemark1.Location = New System.Drawing.Point(388, 72)
        Me.txtRemark1.MaxLength = 50
        Me.txtRemark1.Name = "txtRemark1"
        Me.txtRemark1.Size = New System.Drawing.Size(327, 21)
        Me.txtRemark1.TabIndex = 16
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(296, 47)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 13)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "Credit Days"
        '
        'Label61
        '
        Me.Label61.AutoSize = True
        Me.Label61.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label61.Location = New System.Drawing.Point(296, 76)
        Me.Label61.Name = "Label61"
        Me.Label61.Size = New System.Drawing.Size(57, 13)
        Me.Label61.TabIndex = 15
        Me.Label61.Text = "Remark"
        '
        'btnSave
        '
        Me.btnSave.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(296, 136)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(105, 30)
        Me.btnSave.TabIndex = 18
        Me.btnSave.Text = "&Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.Location = New System.Drawing.Point(620, 136)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(105, 30)
        Me.btnExit.TabIndex = 20
        Me.btnExit.Text = "E&xit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNew.Location = New System.Drawing.Point(402, 136)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(105, 30)
        Me.btnNew.TabIndex = 19
        Me.btnNew.Text = "&New"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'txtAdditionalCharges
        '
        Me.txtAdditionalCharges.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdditionalCharges.Location = New System.Drawing.Point(872, 24)
        Me.txtAdditionalCharges.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdditionalCharges.MaxLength = 12
        Me.txtAdditionalCharges.Name = "txtAdditionalCharges"
        Me.txtAdditionalCharges.Size = New System.Drawing.Size(120, 22)
        Me.txtAdditionalCharges.TabIndex = 1
        Me.txtAdditionalCharges.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.BackColor = System.Drawing.Color.Transparent
        Me.Label26.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label26.Location = New System.Drawing.Point(735, 27)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(127, 14)
        Me.Label26.TabIndex = 0
        Me.Label26.Text = "[F7  ] Adl Charges"
        Me.Label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(735, 75)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(114, 14)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "[F8  ] Round Off"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAdjRoundOff_AMT
        '
        Me.txtAdjRoundOff_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjRoundOff_AMT.Location = New System.Drawing.Point(872, 72)
        Me.txtAdjRoundOff_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjRoundOff_AMT.MaxLength = 12
        Me.txtAdjRoundOff_AMT.Name = "txtAdjRoundOff_AMT"
        Me.txtAdjRoundOff_AMT.Size = New System.Drawing.Size(120, 22)
        Me.txtAdjRoundOff_AMT.TabIndex = 5
        Me.txtAdjRoundOff_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label43
        '
        Me.Label43.AutoSize = True
        Me.Label43.BackColor = System.Drawing.Color.Transparent
        Me.Label43.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label43.Location = New System.Drawing.Point(735, 51)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(59, 14)
        Me.Label43.TabIndex = 2
        Me.Label43.Text = "Receive"
        Me.Label43.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label38
        '
        Me.Label38.AutoSize = True
        Me.Label38.BackColor = System.Drawing.Color.Transparent
        Me.Label38.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label38.Location = New System.Drawing.Point(735, 123)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(97, 14)
        Me.Label38.TabIndex = 8
        Me.Label38.Text = "[F9  ] Cheque"
        Me.Label38.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAdjCredit_AMT
        '
        Me.txtAdjCredit_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjCredit_AMT.Location = New System.Drawing.Point(872, 96)
        Me.txtAdjCredit_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjCredit_AMT.MaxLength = 12
        Me.txtAdjCredit_AMT.Name = "txtAdjCredit_AMT"
        Me.txtAdjCredit_AMT.Size = New System.Drawing.Size(120, 22)
        Me.txtAdjCredit_AMT.TabIndex = 7
        Me.txtAdjCredit_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.BackColor = System.Drawing.Color.Transparent
        Me.Label39.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label39.Location = New System.Drawing.Point(735, 99)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(89, 14)
        Me.Label39.TabIndex = 6
        Me.Label39.Text = "[F11] Credit"
        Me.Label39.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label42
        '
        Me.Label42.AutoSize = True
        Me.Label42.BackColor = System.Drawing.Color.Transparent
        Me.Label42.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label42.Location = New System.Drawing.Point(735, 147)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(80, 14)
        Me.Label42.TabIndex = 10
        Me.Label42.Text = "[F5  ] Cash"
        Me.Label42.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAdjCash_AMT
        '
        Me.txtAdjCash_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjCash_AMT.Location = New System.Drawing.Point(872, 144)
        Me.txtAdjCash_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjCash_AMT.MaxLength = 12
        Me.txtAdjCash_AMT.Name = "txtAdjCash_AMT"
        Me.txtAdjCash_AMT.Size = New System.Drawing.Size(120, 22)
        Me.txtAdjCash_AMT.TabIndex = 11
        Me.txtAdjCash_AMT.Text = "99,999,999.99"
        Me.txtAdjCash_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAdjCheque_AMT
        '
        Me.txtAdjCheque_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjCheque_AMT.Location = New System.Drawing.Point(872, 120)
        Me.txtAdjCheque_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjCheque_AMT.MaxLength = 12
        Me.txtAdjCheque_AMT.Name = "txtAdjCheque_AMT"
        Me.txtAdjCheque_AMT.Size = New System.Drawing.Size(120, 22)
        Me.txtAdjCheque_AMT.TabIndex = 9
        Me.txtAdjCheque_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAdjReceive_AMT
        '
        Me.txtAdjReceive_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjReceive_AMT.Location = New System.Drawing.Point(872, 48)
        Me.txtAdjReceive_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjReceive_AMT.MaxLength = 12
        Me.txtAdjReceive_AMT.Name = "txtAdjReceive_AMT"
        Me.txtAdjReceive_AMT.Size = New System.Drawing.Size(120, 22)
        Me.txtAdjReceive_AMT.TabIndex = 3
        Me.txtAdjReceive_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
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
        Me.pnlLeft.TabIndex = 17
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(119, 48)
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'ManuFacturingIssRecTran
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(1020, 636)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "ManuFacturingIssRecTran"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ManuFacturingIssRecTran"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel1.ResumeLayout(False)
        Me.Grouper2.ResumeLayout(False)
        CType(Me.DgvTranTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DgvTran, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpHeader.ResumeLayout(False)
        Me.grpHeader.PerformLayout()
        Me.Grouper1.ResumeLayout(False)
        Me.Grouper1.PerformLayout()
        Me.tabGroup.ResumeLayout(False)
        Me.TabGridView.ResumeLayout(False)
        CType(Me.dgvOrderDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPicture.ResumeLayout(False)
        CType(Me.picOrder, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gboxOrder.ResumeLayout(False)
        Me.gboxOrder.PerformLayout()
        Me.gboxBulk.ResumeLayout(False)
        Me.gboxBulk.PerformLayout()
        CType(Me.dtCatBalance, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents pnlLeft As System.Windows.Forms.Panel
    Friend WithEvents Grouper1 As CodeVendor.Controls.Grouper
    Friend WithEvents txtAdditionalCharges As System.Windows.Forms.TextBox
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtAdjRoundOff_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents txtAdjCredit_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents txtAdjCash_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtAdjCheque_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtAdjReceive_AMT As System.Windows.Forms.TextBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents txtRemark2 As System.Windows.Forms.TextBox
    Friend WithEvents txtRemark1 As System.Windows.Forms.TextBox
    Friend WithEvents Label61 As System.Windows.Forms.Label
    Friend WithEvents txtCreditDays_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dtpCreditDays As BrighttechPack.DatePicker
    Friend WithEvents btnView As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Grouper2 As CodeVendor.Controls.Grouper
    Friend WithEvents DgvTranTotal As System.Windows.Forms.DataGridView
    Friend WithEvents DgvTran As System.Windows.Forms.DataGridView
    Friend WithEvents grpHeader As CodeVendor.Controls.Grouper
    Friend WithEvents lblBalance As System.Windows.Forms.Label
    Friend WithEvents lblAddress As System.Windows.Forms.Label
    Friend WithEvents cmbAcName As System.Windows.Forms.ComboBox
    Friend WithEvents cmbCostCentre As System.Windows.Forms.ComboBox
    Friend WithEvents cmbTransactionType As System.Windows.Forms.ComboBox
    Friend WithEvents dtpBillDate_OWN As BrighttechPack.DatePicker
    Friend WithEvents dtpTrandate As BrighttechPack.DatePicker
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtBillNo As System.Windows.Forms.TextBox
    Friend WithEvents dtCatBalance As System.Windows.Forms.DataGridView
    Friend WithEvents gboxBulk As System.Windows.Forms.GroupBox
    Friend WithEvents chkMulti As System.Windows.Forms.CheckBox
    Friend WithEvents chkBulk As System.Windows.Forms.CheckBox
    Friend WithEvents gboxOrder As System.Windows.Forms.GroupBox
    Friend WithEvents chkOrder As System.Windows.Forms.CheckBox
    Friend WithEvents lblLot As System.Windows.Forms.Label
    Friend WithEvents lblAddress1 As System.Windows.Forms.Label
    Friend WithEvents dgvOrderDet As System.Windows.Forms.DataGridView
    Friend WithEvents lblOrdNo As System.Windows.Forms.Label
    Friend WithEvents txtOrdNo As System.Windows.Forms.TextBox
    Friend WithEvents lblProcessDateTime As Label
    Friend WithEvents picOrder As PictureBox
    Friend WithEvents tabGroup As TabControl
    Friend WithEvents TabGridView As TabPage
    Friend WithEvents TabPicture As TabPage
    Friend WithEvents lblRunno As Label
End Class
