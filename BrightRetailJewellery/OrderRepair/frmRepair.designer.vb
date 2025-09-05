<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRepair
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtOItemName = New System.Windows.Forms.TextBox()
        Me.txtOMc_AMT = New System.Windows.Forms.TextBox()
        Me.txtOPcs_NUM = New System.Windows.Forms.TextBox()
        Me.txtOGrsWt_WET = New System.Windows.Forms.TextBox()
        Me.txtONetWt_WET = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.tabOtherOptions = New System.Windows.Forms.TabControl()
        Me.tabAddress = New System.Windows.Forms.TabPage()
        Me.grpAdj = New CodeVendor.Controls.Grouper()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Label68 = New System.Windows.Forms.Label()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.Label69 = New System.Windows.Forms.Label()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.Label70 = New System.Windows.Forms.Label()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.Label71 = New System.Windows.Forms.Label()
        Me.txtAdjAdvanceWt = New System.Windows.Forms.TextBox()
        Me.Label72 = New System.Windows.Forms.Label()
        Me.txtAdjAdvance_AMT = New System.Windows.Forms.TextBox()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.txtAdjChitCard_AMT = New System.Windows.Forms.TextBox()
        Me.txtAdjCash_AMT = New System.Windows.Forms.TextBox()
        Me.txtAdjCheque_AMT = New System.Windows.Forms.TextBox()
        Me.txtAdjCreditCard_AMT = New System.Windows.Forms.TextBox()
        Me.grpAddress = New CodeVendor.Controls.Grouper()
        Me.grpImage = New CodeVendor.Controls.Grouper()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.picImage = New System.Windows.Forms.PictureBox()
        Me.Grouper2 = New CodeVendor.Controls.Grouper()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.tabStone = New System.Windows.Forms.TabPage()
        Me.grpStone = New CodeVendor.Controls.Grouper()
        Me.txtStSubItem = New System.Windows.Forms.TextBox()
        Me.txtStItem = New System.Windows.Forms.TextBox()
        Me.txtStRowIndex = New System.Windows.Forms.TextBox()
        Me.gridStoneTotal = New System.Windows.Forms.DataGridView()
        Me.gridStone = New System.Windows.Forms.DataGridView()
        Me.Label61 = New System.Windows.Forms.Label()
        Me.Label57 = New System.Windows.Forms.Label()
        Me.cmbStCalc = New System.Windows.Forms.ComboBox()
        Me.cmbStUnit = New System.Windows.Forms.ComboBox()
        Me.txtStMetalCode = New System.Windows.Forms.TextBox()
        Me.Label58 = New System.Windows.Forms.Label()
        Me.Label60 = New System.Windows.Forms.Label()
        Me.Label59 = New System.Windows.Forms.Label()
        Me.Label47 = New System.Windows.Forms.Label()
        Me.Label46 = New System.Windows.Forms.Label()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.txtStPcs_NUM = New System.Windows.Forms.TextBox()
        Me.txtStAmount_AMT = New System.Windows.Forms.TextBox()
        Me.txtStWeight_WET = New System.Windows.Forms.TextBox()
        Me.txtStRate_AMT = New System.Windows.Forms.TextBox()
        Me.txtOMcPerGrm_AMT = New System.Windows.Forms.TextBox()
        Me.Label49 = New System.Windows.Forms.Label()
        Me.Label62 = New System.Windows.Forms.Label()
        Me.txtODescription = New System.Windows.Forms.TextBox()
        Me.grpOrderDetails = New CodeVendor.Controls.Grouper()
        Me.chkOImage = New System.Windows.Forms.CheckBox()
        Me.txtONatureOfRepair = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtOrderRowIndex = New System.Windows.Forms.TextBox()
        Me.gridOrderTotal = New System.Windows.Forms.DataGridView()
        Me.txtOAmount_AMT = New System.Windows.Forms.TextBox()
        Me.gridOrder = New System.Windows.Forms.DataGridView()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.grpHeader = New CodeVendor.Controls.Grouper()
        Me.cmenuTemplate = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Style1ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Style2ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Style3ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Style4ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Style5ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Style6ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblNx = New System.Windows.Forms.Label()
        Me.lblNext = New System.Windows.Forms.Label()
        Me.lblNextNo = New System.Windows.Forms.Label()
        Me.dtpRepairDate = New BrighttechPack.DatePicker(Me.components)
        Me.lblOrderDate = New System.Windows.Forms.Label()
        Me.dtpDueDate = New BrighttechPack.DatePicker(Me.components)
        Me.dtpRemDate = New BrighttechPack.DatePicker(Me.components)
        Me.pnlOrderType = New System.Windows.Forms.Panel()
        Me.rbtCustomerOrderType = New System.Windows.Forms.RadioButton()
        Me.rbtCompanyOrderType = New System.Windows.Forms.RadioButton()
        Me.txtDueDays_NUM = New System.Windows.Forms.TextBox()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.txtRemDays = New System.Windows.Forms.TextBox()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.Label89 = New System.Windows.Forms.Label()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.Label90 = New System.Windows.Forms.Label()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.Label91 = New System.Windows.Forms.Label()
        Me.Label88 = New System.Windows.Forms.Label()
        Me.Label87 = New System.Windows.Forms.Label()
        Me.Label86 = New System.Windows.Forms.Label()
        Me.lblSystemId = New System.Windows.Forms.Label()
        Me.lblSilverRate = New System.Windows.Forms.Label()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.lblNodeId = New System.Windows.Forms.Label()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.lblBillDate = New System.Windows.Forms.Label()
        Me.lblGoldRate = New System.Windows.Forms.Label()
        Me.lblUserName = New System.Windows.Forms.Label()
        Me.Label45 = New System.Windows.Forms.Label()
        Me.Label55 = New System.Windows.Forms.Label()
        Me.Label56 = New System.Windows.Forms.Label()
        Me.lblCashCounter = New System.Windows.Forms.Label()
        Me.txtSalesManName = New System.Windows.Forms.TextBox()
        Me.txtSalesMan_NUM = New System.Windows.Forms.TextBox()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AdvanceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChitCardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChequeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CreditCardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CashToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WastageMcToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlContainer_OWN = New System.Windows.Forms.Panel()
        Me.tabOtherOptions.SuspendLayout()
        Me.tabAddress.SuspendLayout()
        Me.grpAdj.SuspendLayout()
        Me.grpAddress.SuspendLayout()
        Me.grpImage.SuspendLayout()
        CType(Me.picImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Grouper2.SuspendLayout()
        Me.tabStone.SuspendLayout()
        Me.grpStone.SuspendLayout()
        CType(Me.gridStoneTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridStone, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpOrderDetails.SuspendLayout()
        CType(Me.gridOrderTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridOrder, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpHeader.SuspendLayout()
        Me.cmenuTemplate.SuspendLayout()
        Me.pnlOrderType.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.pnlContainer_OWN.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(8, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(139, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Item"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(386, 23)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(35, 17)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Pcs"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(422, 23)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(59, 17)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "GrsWt"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(482, 23)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(59, 17)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "NetWt"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtOItemName
        '
        Me.txtOItemName.Location = New System.Drawing.Point(8, 40)
        Me.txtOItemName.Name = "txtOItemName"
        Me.txtOItemName.Size = New System.Drawing.Size(139, 21)
        Me.txtOItemName.TabIndex = 1
        Me.txtOItemName.Text = "Culcutta Bangle Kasai"
        '
        'txtOMc_AMT
        '
        Me.txtOMc_AMT.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOMc_AMT.Location = New System.Drawing.Point(603, 40)
        Me.txtOMc_AMT.MaxLength = 12
        Me.txtOMc_AMT.Name = "txtOMc_AMT"
        Me.txtOMc_AMT.Size = New System.Drawing.Size(60, 21)
        Me.txtOMc_AMT.TabIndex = 13
        Me.txtOMc_AMT.Text = "99999.99"
        '
        'txtOPcs_NUM
        '
        Me.txtOPcs_NUM.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOPcs_NUM.Location = New System.Drawing.Point(386, 40)
        Me.txtOPcs_NUM.MaxLength = 5
        Me.txtOPcs_NUM.Name = "txtOPcs_NUM"
        Me.txtOPcs_NUM.Size = New System.Drawing.Size(35, 21)
        Me.txtOPcs_NUM.TabIndex = 5
        Me.txtOPcs_NUM.Text = "99999"
        '
        'txtOGrsWt_WET
        '
        Me.txtOGrsWt_WET.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOGrsWt_WET.Location = New System.Drawing.Point(422, 40)
        Me.txtOGrsWt_WET.MaxLength = 10
        Me.txtOGrsWt_WET.Name = "txtOGrsWt_WET"
        Me.txtOGrsWt_WET.Size = New System.Drawing.Size(59, 21)
        Me.txtOGrsWt_WET.TabIndex = 7
        Me.txtOGrsWt_WET.Text = "9999.999"
        '
        'txtONetWt_WET
        '
        Me.txtONetWt_WET.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtONetWt_WET.Location = New System.Drawing.Point(482, 40)
        Me.txtONetWt_WET.MaxLength = 10
        Me.txtONetWt_WET.Name = "txtONetWt_WET"
        Me.txtONetWt_WET.Size = New System.Drawing.Size(59, 21)
        Me.txtONetWt_WET.TabIndex = 9
        Me.txtONetWt_WET.Text = "999.999"
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(603, 23)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(60, 17)
        Me.Label11.TabIndex = 12
        Me.Label11.Text = "Mc"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tabOtherOptions
        '
        Me.tabOtherOptions.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tabOtherOptions.Controls.Add(Me.tabAddress)
        Me.tabOtherOptions.Controls.Add(Me.tabStone)
        Me.tabOtherOptions.ItemSize = New System.Drawing.Size(1, 10)
        Me.tabOtherOptions.Location = New System.Drawing.Point(0, 420)
        Me.tabOtherOptions.Name = "tabOtherOptions"
        Me.tabOtherOptions.SelectedIndex = 0
        Me.tabOtherOptions.Size = New System.Drawing.Size(1021, 213)
        Me.tabOtherOptions.TabIndex = 2
        '
        'tabAddress
        '
        Me.tabAddress.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.tabAddress.Controls.Add(Me.grpAdj)
        Me.tabAddress.Controls.Add(Me.grpAddress)
        Me.tabAddress.Controls.Add(Me.Grouper2)
        Me.tabAddress.Location = New System.Drawing.Point(4, 14)
        Me.tabAddress.Name = "tabAddress"
        Me.tabAddress.Padding = New System.Windows.Forms.Padding(3)
        Me.tabAddress.Size = New System.Drawing.Size(1013, 195)
        Me.tabAddress.TabIndex = 0
        Me.tabAddress.Text = "TabPage1"
        Me.tabAddress.UseVisualStyleBackColor = True
        '
        'grpAdj
        '
        Me.grpAdj.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpAdj.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpAdj.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpAdj.BorderColor = System.Drawing.Color.Transparent
        Me.grpAdj.BorderThickness = 1.0!
        Me.grpAdj.Controls.Add(Me.Label27)
        Me.grpAdj.Controls.Add(Me.Label68)
        Me.grpAdj.Controls.Add(Me.Label28)
        Me.grpAdj.Controls.Add(Me.Label69)
        Me.grpAdj.Controls.Add(Me.Label38)
        Me.grpAdj.Controls.Add(Me.Label70)
        Me.grpAdj.Controls.Add(Me.Label40)
        Me.grpAdj.Controls.Add(Me.Label71)
        Me.grpAdj.Controls.Add(Me.txtAdjAdvanceWt)
        Me.grpAdj.Controls.Add(Me.Label72)
        Me.grpAdj.Controls.Add(Me.txtAdjAdvance_AMT)
        Me.grpAdj.Controls.Add(Me.Label42)
        Me.grpAdj.Controls.Add(Me.Label14)
        Me.grpAdj.Controls.Add(Me.txtAdjChitCard_AMT)
        Me.grpAdj.Controls.Add(Me.txtAdjCash_AMT)
        Me.grpAdj.Controls.Add(Me.txtAdjCheque_AMT)
        Me.grpAdj.Controls.Add(Me.txtAdjCreditCard_AMT)
        Me.grpAdj.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpAdj.GroupImage = Nothing
        Me.grpAdj.GroupTitle = ""
        Me.grpAdj.Location = New System.Drawing.Point(624, -5)
        Me.grpAdj.Name = "grpAdj"
        Me.grpAdj.Padding = New System.Windows.Forms.Padding(20)
        Me.grpAdj.PaintGroupBox = False
        Me.grpAdj.RoundCorners = 10
        Me.grpAdj.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpAdj.ShadowControl = False
        Me.grpAdj.ShadowThickness = 3
        Me.grpAdj.Size = New System.Drawing.Size(248, 199)
        Me.grpAdj.TabIndex = 1
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.BackColor = System.Drawing.Color.Transparent
        Me.Label27.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label27.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label27.Location = New System.Drawing.Point(35, 53)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(56, 13)
        Me.Label27.TabIndex = 2
        Me.Label27.Text = "Advance"
        Me.Label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label68
        '
        Me.Label68.AutoSize = True
        Me.Label68.BackColor = System.Drawing.Color.Transparent
        Me.Label68.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label68.Location = New System.Drawing.Point(3, 143)
        Me.Label68.Name = "Label68"
        Me.Label68.Size = New System.Drawing.Size(30, 13)
        Me.Label68.TabIndex = 30
        Me.Label68.Text = "[F7]"
        Me.Label68.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.BackColor = System.Drawing.Color.Transparent
        Me.Label28.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.Location = New System.Drawing.Point(35, 83)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(55, 13)
        Me.Label28.TabIndex = 4
        Me.Label28.Text = "SCHEME"
        Me.Label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label69
        '
        Me.Label69.AutoSize = True
        Me.Label69.BackColor = System.Drawing.Color.Transparent
        Me.Label69.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label69.Location = New System.Drawing.Point(3, 53)
        Me.Label69.Name = "Label69"
        Me.Label69.Size = New System.Drawing.Size(37, 13)
        Me.Label69.TabIndex = 27
        Me.Label69.Text = "[F12]"
        Me.Label69.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label38
        '
        Me.Label38.AutoSize = True
        Me.Label38.BackColor = System.Drawing.Color.Transparent
        Me.Label38.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label38.Location = New System.Drawing.Point(35, 113)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(51, 13)
        Me.Label38.TabIndex = 6
        Me.Label38.Text = "Cheque"
        Me.Label38.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label70
        '
        Me.Label70.AutoSize = True
        Me.Label70.BackColor = System.Drawing.Color.Transparent
        Me.Label70.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label70.Location = New System.Drawing.Point(3, 83)
        Me.Label70.Name = "Label70"
        Me.Label70.Size = New System.Drawing.Size(30, 13)
        Me.Label70.TabIndex = 28
        Me.Label70.Text = "[F9]"
        Me.Label70.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label40
        '
        Me.Label40.AutoSize = True
        Me.Label40.BackColor = System.Drawing.Color.Transparent
        Me.Label40.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label40.Location = New System.Drawing.Point(35, 143)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(74, 13)
        Me.Label40.TabIndex = 8
        Me.Label40.Text = "Credit Card"
        Me.Label40.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label71
        '
        Me.Label71.AutoSize = True
        Me.Label71.BackColor = System.Drawing.Color.Transparent
        Me.Label71.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label71.Location = New System.Drawing.Point(3, 113)
        Me.Label71.Name = "Label71"
        Me.Label71.Size = New System.Drawing.Size(30, 13)
        Me.Label71.TabIndex = 29
        Me.Label71.Text = "[F8]"
        Me.Label71.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAdjAdvanceWt
        '
        Me.txtAdjAdvanceWt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjAdvanceWt.Location = New System.Drawing.Point(131, 18)
        Me.txtAdjAdvanceWt.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjAdvanceWt.MaxLength = 12
        Me.txtAdjAdvanceWt.Name = "txtAdjAdvanceWt"
        Me.txtAdjAdvanceWt.Size = New System.Drawing.Size(109, 22)
        Me.txtAdjAdvanceWt.TabIndex = 1
        Me.txtAdjAdvanceWt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label72
        '
        Me.Label72.AutoSize = True
        Me.Label72.BackColor = System.Drawing.Color.Transparent
        Me.Label72.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label72.Location = New System.Drawing.Point(3, 173)
        Me.Label72.Name = "Label72"
        Me.Label72.Size = New System.Drawing.Size(30, 13)
        Me.Label72.TabIndex = 31
        Me.Label72.Text = "[F4]"
        Me.Label72.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAdjAdvance_AMT
        '
        Me.txtAdjAdvance_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjAdvance_AMT.Location = New System.Drawing.Point(131, 48)
        Me.txtAdjAdvance_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjAdvance_AMT.MaxLength = 12
        Me.txtAdjAdvance_AMT.Name = "txtAdjAdvance_AMT"
        Me.txtAdjAdvance_AMT.Size = New System.Drawing.Size(109, 22)
        Me.txtAdjAdvance_AMT.TabIndex = 3
        Me.txtAdjAdvance_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label42
        '
        Me.Label42.AutoSize = True
        Me.Label42.BackColor = System.Drawing.Color.Transparent
        Me.Label42.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label42.Location = New System.Drawing.Point(35, 173)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(36, 13)
        Me.Label42.TabIndex = 10
        Me.Label42.Text = "Cash"
        Me.Label42.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.BackColor = System.Drawing.Color.Transparent
        Me.Label14.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label14.Location = New System.Drawing.Point(31, 23)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(99, 13)
        Me.Label14.TabIndex = 0
        Me.Label14.Text = "Advance Weight"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAdjChitCard_AMT
        '
        Me.txtAdjChitCard_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjChitCard_AMT.Location = New System.Drawing.Point(131, 78)
        Me.txtAdjChitCard_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjChitCard_AMT.MaxLength = 12
        Me.txtAdjChitCard_AMT.Name = "txtAdjChitCard_AMT"
        Me.txtAdjChitCard_AMT.Size = New System.Drawing.Size(109, 22)
        Me.txtAdjChitCard_AMT.TabIndex = 5
        Me.txtAdjChitCard_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAdjCash_AMT
        '
        Me.txtAdjCash_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjCash_AMT.Location = New System.Drawing.Point(131, 168)
        Me.txtAdjCash_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjCash_AMT.MaxLength = 12
        Me.txtAdjCash_AMT.Name = "txtAdjCash_AMT"
        Me.txtAdjCash_AMT.Size = New System.Drawing.Size(109, 22)
        Me.txtAdjCash_AMT.TabIndex = 11
        Me.txtAdjCash_AMT.Text = "99,999,999.99"
        Me.txtAdjCash_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAdjCheque_AMT
        '
        Me.txtAdjCheque_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjCheque_AMT.Location = New System.Drawing.Point(131, 108)
        Me.txtAdjCheque_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjCheque_AMT.MaxLength = 12
        Me.txtAdjCheque_AMT.Name = "txtAdjCheque_AMT"
        Me.txtAdjCheque_AMT.Size = New System.Drawing.Size(109, 22)
        Me.txtAdjCheque_AMT.TabIndex = 7
        Me.txtAdjCheque_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAdjCreditCard_AMT
        '
        Me.txtAdjCreditCard_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjCreditCard_AMT.Location = New System.Drawing.Point(131, 138)
        Me.txtAdjCreditCard_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjCreditCard_AMT.MaxLength = 12
        Me.txtAdjCreditCard_AMT.Name = "txtAdjCreditCard_AMT"
        Me.txtAdjCreditCard_AMT.Size = New System.Drawing.Size(109, 22)
        Me.txtAdjCreditCard_AMT.TabIndex = 9
        Me.txtAdjCreditCard_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'grpAddress
        '
        Me.grpAddress.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpAddress.BackgroundGradientColor = System.Drawing.Color.White
        Me.grpAddress.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpAddress.BorderColor = System.Drawing.Color.Transparent
        Me.grpAddress.BorderThickness = 1.0!
        Me.grpAddress.Controls.Add(Me.grpImage)
        Me.grpAddress.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpAddress.GroupImage = Nothing
        Me.grpAddress.GroupTitle = ""
        Me.grpAddress.Location = New System.Drawing.Point(4, -5)
        Me.grpAddress.Name = "grpAddress"
        Me.grpAddress.Padding = New System.Windows.Forms.Padding(20)
        Me.grpAddress.PaintGroupBox = False
        Me.grpAddress.RoundCorners = 10
        Me.grpAddress.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpAddress.ShadowControl = False
        Me.grpAddress.ShadowThickness = 3
        Me.grpAddress.Size = New System.Drawing.Size(615, 199)
        Me.grpAddress.TabIndex = 0
        '
        'grpImage
        '
        Me.grpImage.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpImage.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpImage.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpImage.BorderColor = System.Drawing.Color.Transparent
        Me.grpImage.BorderThickness = 1.0!
        Me.grpImage.Controls.Add(Me.btnBrowse)
        Me.grpImage.Controls.Add(Me.picImage)
        Me.grpImage.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpImage.GroupImage = Nothing
        Me.grpImage.GroupTitle = ""
        Me.grpImage.Location = New System.Drawing.Point(366, 0)
        Me.grpImage.Name = "grpImage"
        Me.grpImage.Padding = New System.Windows.Forms.Padding(20)
        Me.grpImage.PaintGroupBox = False
        Me.grpImage.RoundCorners = 10
        Me.grpImage.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpImage.ShadowControl = False
        Me.grpImage.ShadowThickness = 3
        Me.grpImage.Size = New System.Drawing.Size(248, 204)
        Me.grpImage.TabIndex = 2
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(8, 163)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(230, 27)
        Me.btnBrowse.TabIndex = 2
        Me.btnBrowse.Text = "&Browse"
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'picImage
        '
        Me.picImage.Location = New System.Drawing.Point(8, 21)
        Me.picImage.Name = "picImage"
        Me.picImage.Size = New System.Drawing.Size(229, 141)
        Me.picImage.TabIndex = 0
        Me.picImage.TabStop = False
        '
        'Grouper2
        '
        Me.Grouper2.BackgroundColor = System.Drawing.Color.Lavender
        Me.Grouper2.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.Grouper2.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.Grouper2.BorderColor = System.Drawing.Color.Transparent
        Me.Grouper2.BorderThickness = 1.0!
        Me.Grouper2.Controls.Add(Me.btnExit)
        Me.Grouper2.Controls.Add(Me.btnNew)
        Me.Grouper2.Controls.Add(Me.btnSave)
        Me.Grouper2.CustomGroupBoxColor = System.Drawing.Color.White
        Me.Grouper2.GroupImage = Nothing
        Me.Grouper2.GroupTitle = ""
        Me.Grouper2.Location = New System.Drawing.Point(878, -4)
        Me.Grouper2.Name = "Grouper2"
        Me.Grouper2.Padding = New System.Windows.Forms.Padding(20)
        Me.Grouper2.PaintGroupBox = False
        Me.Grouper2.RoundCorners = 10
        Me.Grouper2.ShadowColor = System.Drawing.Color.DarkGray
        Me.Grouper2.ShadowControl = False
        Me.Grouper2.ShadowThickness = 3
        Me.Grouper2.Size = New System.Drawing.Size(131, 199)
        Me.Grouper2.TabIndex = 2
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(16, 141)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(101, 30)
        Me.btnExit.TabIndex = 2
        Me.btnExit.Text = "E&xit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(16, 88)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(101, 30)
        Me.btnNew.TabIndex = 1
        Me.btnNew.Text = "&New"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(16, 35)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(101, 30)
        Me.btnSave.TabIndex = 0
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'tabStone
        '
        Me.tabStone.Controls.Add(Me.grpStone)
        Me.tabStone.Location = New System.Drawing.Point(4, 14)
        Me.tabStone.Name = "tabStone"
        Me.tabStone.Padding = New System.Windows.Forms.Padding(3)
        Me.tabStone.Size = New System.Drawing.Size(1013, 195)
        Me.tabStone.TabIndex = 1
        Me.tabStone.Text = "TabPage2"
        Me.tabStone.UseVisualStyleBackColor = True
        '
        'grpStone
        '
        Me.grpStone.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpStone.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpStone.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpStone.BorderColor = System.Drawing.Color.Transparent
        Me.grpStone.BorderThickness = 1.0!
        Me.grpStone.Controls.Add(Me.txtStSubItem)
        Me.grpStone.Controls.Add(Me.txtStItem)
        Me.grpStone.Controls.Add(Me.txtStRowIndex)
        Me.grpStone.Controls.Add(Me.gridStoneTotal)
        Me.grpStone.Controls.Add(Me.gridStone)
        Me.grpStone.Controls.Add(Me.Label61)
        Me.grpStone.Controls.Add(Me.Label57)
        Me.grpStone.Controls.Add(Me.cmbStCalc)
        Me.grpStone.Controls.Add(Me.cmbStUnit)
        Me.grpStone.Controls.Add(Me.txtStMetalCode)
        Me.grpStone.Controls.Add(Me.Label58)
        Me.grpStone.Controls.Add(Me.Label60)
        Me.grpStone.Controls.Add(Me.Label59)
        Me.grpStone.Controls.Add(Me.Label47)
        Me.grpStone.Controls.Add(Me.Label46)
        Me.grpStone.Controls.Add(Me.Label26)
        Me.grpStone.Controls.Add(Me.txtStPcs_NUM)
        Me.grpStone.Controls.Add(Me.txtStAmount_AMT)
        Me.grpStone.Controls.Add(Me.txtStWeight_WET)
        Me.grpStone.Controls.Add(Me.txtStRate_AMT)
        Me.grpStone.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpStone.GroupImage = Nothing
        Me.grpStone.GroupTitle = ""
        Me.grpStone.Location = New System.Drawing.Point(4, -5)
        Me.grpStone.Name = "grpStone"
        Me.grpStone.Padding = New System.Windows.Forms.Padding(20)
        Me.grpStone.PaintGroupBox = False
        Me.grpStone.RoundCorners = 10
        Me.grpStone.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpStone.ShadowControl = False
        Me.grpStone.ShadowThickness = 3
        Me.grpStone.Size = New System.Drawing.Size(1006, 199)
        Me.grpStone.TabIndex = 0
        '
        'txtStSubItem
        '
        Me.txtStSubItem.Location = New System.Drawing.Point(262, 30)
        Me.txtStSubItem.Name = "txtStSubItem"
        Me.txtStSubItem.Size = New System.Drawing.Size(224, 21)
        Me.txtStSubItem.TabIndex = 3
        '
        'txtStItem
        '
        Me.txtStItem.Location = New System.Drawing.Point(6, 30)
        Me.txtStItem.Name = "txtStItem"
        Me.txtStItem.Size = New System.Drawing.Size(255, 21)
        Me.txtStItem.TabIndex = 1
        '
        'txtStRowIndex
        '
        Me.txtStRowIndex.Location = New System.Drawing.Point(492, 111)
        Me.txtStRowIndex.Name = "txtStRowIndex"
        Me.txtStRowIndex.Size = New System.Drawing.Size(12, 21)
        Me.txtStRowIndex.TabIndex = 22
        Me.txtStRowIndex.Visible = False
        '
        'gridStoneTotal
        '
        Me.gridStoneTotal.AllowUserToAddRows = False
        Me.gridStoneTotal.AllowUserToDeleteRows = False
        Me.gridStoneTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridStoneTotal.ColumnHeadersVisible = False
        Me.gridStoneTotal.Enabled = False
        Me.gridStoneTotal.Location = New System.Drawing.Point(6, 174)
        Me.gridStoneTotal.Name = "gridStoneTotal"
        Me.gridStoneTotal.ReadOnly = True
        Me.gridStoneTotal.RowHeadersVisible = False
        Me.gridStoneTotal.Size = New System.Drawing.Size(991, 19)
        Me.gridStoneTotal.TabIndex = 10
        '
        'gridStone
        '
        Me.gridStone.AllowUserToAddRows = False
        Me.gridStone.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.gridStone.ColumnHeadersVisible = False
        Me.gridStone.Location = New System.Drawing.Point(6, 52)
        Me.gridStone.MultiSelect = False
        Me.gridStone.Name = "gridStone"
        Me.gridStone.ReadOnly = True
        Me.gridStone.RowHeadersVisible = False
        Me.gridStone.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.gridStone.RowTemplate.Height = 20
        Me.gridStone.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridStone.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.gridStone.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridStone.Size = New System.Drawing.Size(991, 122)
        Me.gridStone.TabIndex = 16
        '
        'Label61
        '
        Me.Label61.BackColor = System.Drawing.Color.Transparent
        Me.Label61.Location = New System.Drawing.Point(872, 14)
        Me.Label61.Name = "Label61"
        Me.Label61.Size = New System.Drawing.Size(95, 15)
        Me.Label61.TabIndex = 14
        Me.Label61.Text = "Amount"
        Me.Label61.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label57
        '
        Me.Label57.BackColor = System.Drawing.Color.Transparent
        Me.Label57.Location = New System.Drawing.Point(622, 14)
        Me.Label57.Name = "Label57"
        Me.Label57.Size = New System.Drawing.Size(74, 15)
        Me.Label57.TabIndex = 8
        Me.Label57.Text = "Unit"
        Me.Label57.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmbStCalc
        '
        Me.cmbStCalc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStCalc.FormattingEnabled = True
        Me.cmbStCalc.Location = New System.Drawing.Point(697, 30)
        Me.cmbStCalc.Name = "cmbStCalc"
        Me.cmbStCalc.Size = New System.Drawing.Size(74, 21)
        Me.cmbStCalc.TabIndex = 11
        '
        'cmbStUnit
        '
        Me.cmbStUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStUnit.FormattingEnabled = True
        Me.cmbStUnit.Location = New System.Drawing.Point(622, 30)
        Me.cmbStUnit.Name = "cmbStUnit"
        Me.cmbStUnit.Size = New System.Drawing.Size(74, 21)
        Me.cmbStUnit.TabIndex = 9
        '
        'txtStMetalCode
        '
        Me.txtStMetalCode.Enabled = False
        Me.txtStMetalCode.Location = New System.Drawing.Point(494, 91)
        Me.txtStMetalCode.Name = "txtStMetalCode"
        Me.txtStMetalCode.Size = New System.Drawing.Size(8, 21)
        Me.txtStMetalCode.TabIndex = 21
        Me.txtStMetalCode.Visible = False
        '
        'Label58
        '
        Me.Label58.BackColor = System.Drawing.Color.Transparent
        Me.Label58.Location = New System.Drawing.Point(697, 14)
        Me.Label58.Name = "Label58"
        Me.Label58.Size = New System.Drawing.Size(74, 15)
        Me.Label58.TabIndex = 10
        Me.Label58.Text = "Cal"
        Me.Label58.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label60
        '
        Me.Label60.BackColor = System.Drawing.Color.Transparent
        Me.Label60.Location = New System.Drawing.Point(772, 14)
        Me.Label60.Name = "Label60"
        Me.Label60.Size = New System.Drawing.Size(99, 15)
        Me.Label60.TabIndex = 12
        Me.Label60.Text = "Rate"
        Me.Label60.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label59
        '
        Me.Label59.BackColor = System.Drawing.Color.Transparent
        Me.Label59.Location = New System.Drawing.Point(533, 14)
        Me.Label59.Name = "Label59"
        Me.Label59.Size = New System.Drawing.Size(88, 15)
        Me.Label59.TabIndex = 6
        Me.Label59.Text = "Weight"
        Me.Label59.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label47
        '
        Me.Label47.BackColor = System.Drawing.Color.Transparent
        Me.Label47.Location = New System.Drawing.Point(6, 14)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(255, 15)
        Me.Label47.TabIndex = 0
        Me.Label47.Text = "Item"
        Me.Label47.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label46
        '
        Me.Label46.BackColor = System.Drawing.Color.Transparent
        Me.Label46.Location = New System.Drawing.Point(262, 14)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(224, 15)
        Me.Label46.TabIndex = 2
        Me.Label46.Text = "Sub Item"
        Me.Label46.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label26
        '
        Me.Label26.BackColor = System.Drawing.Color.Transparent
        Me.Label26.Location = New System.Drawing.Point(487, 14)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(45, 15)
        Me.Label26.TabIndex = 4
        Me.Label26.Text = "Pcs"
        Me.Label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtStPcs_NUM
        '
        Me.txtStPcs_NUM.Location = New System.Drawing.Point(487, 30)
        Me.txtStPcs_NUM.MaxLength = 8
        Me.txtStPcs_NUM.Name = "txtStPcs_NUM"
        Me.txtStPcs_NUM.Size = New System.Drawing.Size(45, 21)
        Me.txtStPcs_NUM.TabIndex = 5
        Me.txtStPcs_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtStAmount_AMT
        '
        Me.txtStAmount_AMT.Location = New System.Drawing.Point(872, 30)
        Me.txtStAmount_AMT.MaxLength = 12
        Me.txtStAmount_AMT.Name = "txtStAmount_AMT"
        Me.txtStAmount_AMT.Size = New System.Drawing.Size(99, 21)
        Me.txtStAmount_AMT.TabIndex = 15
        Me.txtStAmount_AMT.Text = "1234567.00"
        Me.txtStAmount_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtStWeight_WET
        '
        Me.txtStWeight_WET.Location = New System.Drawing.Point(533, 30)
        Me.txtStWeight_WET.MaxLength = 10
        Me.txtStWeight_WET.Name = "txtStWeight_WET"
        Me.txtStWeight_WET.Size = New System.Drawing.Size(88, 21)
        Me.txtStWeight_WET.TabIndex = 7
        Me.txtStWeight_WET.Text = "9999.999"
        Me.txtStWeight_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtStRate_AMT
        '
        Me.txtStRate_AMT.Location = New System.Drawing.Point(772, 30)
        Me.txtStRate_AMT.MaxLength = 10
        Me.txtStRate_AMT.Name = "txtStRate_AMT"
        Me.txtStRate_AMT.Size = New System.Drawing.Size(99, 21)
        Me.txtStRate_AMT.TabIndex = 13
        Me.txtStRate_AMT.Text = "500000.99"
        Me.txtStRate_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtOMcPerGrm_AMT
        '
        Me.txtOMcPerGrm_AMT.Location = New System.Drawing.Point(542, 40)
        Me.txtOMcPerGrm_AMT.Name = "txtOMcPerGrm_AMT"
        Me.txtOMcPerGrm_AMT.Size = New System.Drawing.Size(60, 21)
        Me.txtOMcPerGrm_AMT.TabIndex = 11
        '
        'Label49
        '
        Me.Label49.Location = New System.Drawing.Point(542, 23)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(61, 17)
        Me.Label49.TabIndex = 10
        Me.Label49.Text = "Mc/Grm"
        Me.Label49.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label62
        '
        Me.Label62.Location = New System.Drawing.Point(148, 23)
        Me.Label62.Name = "Label62"
        Me.Label62.Size = New System.Drawing.Size(237, 17)
        Me.Label62.TabIndex = 2
        Me.Label62.Text = "Description"
        Me.Label62.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtODescription
        '
        Me.txtODescription.Location = New System.Drawing.Point(148, 40)
        Me.txtODescription.MaxLength = 250
        Me.txtODescription.Name = "txtODescription"
        Me.txtODescription.Size = New System.Drawing.Size(237, 21)
        Me.txtODescription.TabIndex = 3
        Me.txtODescription.Text = "012345678900123456789001234567890012345678900123456789001234567890"
        '
        'grpOrderDetails
        '
        Me.grpOrderDetails.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpOrderDetails.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpOrderDetails.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpOrderDetails.BorderColor = System.Drawing.Color.Transparent
        Me.grpOrderDetails.BorderThickness = 1.0!
        Me.grpOrderDetails.Controls.Add(Me.chkOImage)
        Me.grpOrderDetails.Controls.Add(Me.Label49)
        Me.grpOrderDetails.Controls.Add(Me.txtONatureOfRepair)
        Me.grpOrderDetails.Controls.Add(Me.Label6)
        Me.grpOrderDetails.Controls.Add(Me.Label3)
        Me.grpOrderDetails.Controls.Add(Me.txtOMcPerGrm_AMT)
        Me.grpOrderDetails.Controls.Add(Me.Label62)
        Me.grpOrderDetails.Controls.Add(Me.txtONetWt_WET)
        Me.grpOrderDetails.Controls.Add(Me.txtODescription)
        Me.grpOrderDetails.Controls.Add(Me.txtOGrsWt_WET)
        Me.grpOrderDetails.Controls.Add(Me.txtOrderRowIndex)
        Me.grpOrderDetails.Controls.Add(Me.txtOPcs_NUM)
        Me.grpOrderDetails.Controls.Add(Me.gridOrderTotal)
        Me.grpOrderDetails.Controls.Add(Me.txtOAmount_AMT)
        Me.grpOrderDetails.Controls.Add(Me.gridOrder)
        Me.grpOrderDetails.Controls.Add(Me.txtOMc_AMT)
        Me.grpOrderDetails.Controls.Add(Me.Label1)
        Me.grpOrderDetails.Controls.Add(Me.Label11)
        Me.grpOrderDetails.Controls.Add(Me.txtOItemName)
        Me.grpOrderDetails.Controls.Add(Me.Label2)
        Me.grpOrderDetails.Controls.Add(Me.Label4)
        Me.grpOrderDetails.Controls.Add(Me.Label5)
        Me.grpOrderDetails.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpOrderDetails.GroupImage = Nothing
        Me.grpOrderDetails.GroupTitle = ""
        Me.grpOrderDetails.Location = New System.Drawing.Point(7, 144)
        Me.grpOrderDetails.Name = "grpOrderDetails"
        Me.grpOrderDetails.Padding = New System.Windows.Forms.Padding(20)
        Me.grpOrderDetails.PaintGroupBox = False
        Me.grpOrderDetails.RoundCorners = 10
        Me.grpOrderDetails.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpOrderDetails.ShadowControl = False
        Me.grpOrderDetails.ShadowThickness = 3
        Me.grpOrderDetails.Size = New System.Drawing.Size(1006, 290)
        Me.grpOrderDetails.TabIndex = 1
        '
        'chkOImage
        '
        Me.chkOImage.BackColor = System.Drawing.Color.Transparent
        Me.chkOImage.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.chkOImage.Location = New System.Drawing.Point(986, 42)
        Me.chkOImage.Name = "chkOImage"
        Me.chkOImage.Size = New System.Drawing.Size(15, 14)
        Me.chkOImage.TabIndex = 30
        Me.chkOImage.UseVisualStyleBackColor = False
        '
        'txtONatureOfRepair
        '
        Me.txtONatureOfRepair.Location = New System.Drawing.Point(744, 40)
        Me.txtONatureOfRepair.MaxLength = 250
        Me.txtONatureOfRepair.Name = "txtONatureOfRepair"
        Me.txtONatureOfRepair.Size = New System.Drawing.Size(233, 21)
        Me.txtONatureOfRepair.TabIndex = 17
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(744, 23)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(233, 17)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "Nature Of Repair"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtOrderRowIndex
        '
        Me.txtOrderRowIndex.Location = New System.Drawing.Point(471, 183)
        Me.txtOrderRowIndex.Name = "txtOrderRowIndex"
        Me.txtOrderRowIndex.Size = New System.Drawing.Size(60, 21)
        Me.txtOrderRowIndex.TabIndex = 19
        Me.txtOrderRowIndex.Visible = False
        '
        'gridOrderTotal
        '
        Me.gridOrderTotal.AllowUserToAddRows = False
        Me.gridOrderTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridOrderTotal.Enabled = False
        Me.gridOrderTotal.Location = New System.Drawing.Point(8, 262)
        Me.gridOrderTotal.Name = "gridOrderTotal"
        Me.gridOrderTotal.ReadOnly = True
        Me.gridOrderTotal.Size = New System.Drawing.Size(988, 19)
        Me.gridOrderTotal.TabIndex = 29
        '
        'txtOAmount_AMT
        '
        Me.txtOAmount_AMT.Location = New System.Drawing.Point(664, 40)
        Me.txtOAmount_AMT.Name = "txtOAmount_AMT"
        Me.txtOAmount_AMT.Size = New System.Drawing.Size(79, 21)
        Me.txtOAmount_AMT.TabIndex = 15
        '
        'gridOrder
        '
        Me.gridOrder.AllowUserToAddRows = False
        Me.gridOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridOrder.Location = New System.Drawing.Point(8, 62)
        Me.gridOrder.Name = "gridOrder"
        Me.gridOrder.ReadOnly = True
        Me.gridOrder.Size = New System.Drawing.Size(988, 200)
        Me.gridOrder.TabIndex = 18
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(664, 23)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(79, 17)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Amount"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpHeader
        '
        Me.grpHeader.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpHeader.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpHeader.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpHeader.BorderColor = System.Drawing.Color.Transparent
        Me.grpHeader.BorderThickness = 1.0!
        Me.grpHeader.ContextMenuStrip = Me.cmenuTemplate
        Me.grpHeader.Controls.Add(Me.lblNx)
        Me.grpHeader.Controls.Add(Me.lblNext)
        Me.grpHeader.Controls.Add(Me.lblNextNo)
        Me.grpHeader.Controls.Add(Me.dtpRepairDate)
        Me.grpHeader.Controls.Add(Me.lblOrderDate)
        Me.grpHeader.Controls.Add(Me.dtpDueDate)
        Me.grpHeader.Controls.Add(Me.dtpRemDate)
        Me.grpHeader.Controls.Add(Me.pnlOrderType)
        Me.grpHeader.Controls.Add(Me.txtDueDays_NUM)
        Me.grpHeader.Controls.Add(Me.lblTitle)
        Me.grpHeader.Controls.Add(Me.Label31)
        Me.grpHeader.Controls.Add(Me.txtRemDays)
        Me.grpHeader.Controls.Add(Me.Label30)
        Me.grpHeader.Controls.Add(Me.Label89)
        Me.grpHeader.Controls.Add(Me.Label32)
        Me.grpHeader.Controls.Add(Me.Label90)
        Me.grpHeader.Controls.Add(Me.Label37)
        Me.grpHeader.Controls.Add(Me.Label91)
        Me.grpHeader.Controls.Add(Me.Label88)
        Me.grpHeader.Controls.Add(Me.Label87)
        Me.grpHeader.Controls.Add(Me.Label86)
        Me.grpHeader.Controls.Add(Me.lblSystemId)
        Me.grpHeader.Controls.Add(Me.lblSilverRate)
        Me.grpHeader.Controls.Add(Me.Label43)
        Me.grpHeader.Controls.Add(Me.lblNodeId)
        Me.grpHeader.Controls.Add(Me.Label44)
        Me.grpHeader.Controls.Add(Me.lblBillDate)
        Me.grpHeader.Controls.Add(Me.lblGoldRate)
        Me.grpHeader.Controls.Add(Me.lblUserName)
        Me.grpHeader.Controls.Add(Me.Label45)
        Me.grpHeader.Controls.Add(Me.Label55)
        Me.grpHeader.Controls.Add(Me.Label56)
        Me.grpHeader.Controls.Add(Me.lblCashCounter)
        Me.grpHeader.Controls.Add(Me.txtSalesManName)
        Me.grpHeader.Controls.Add(Me.txtSalesMan_NUM)
        Me.grpHeader.Controls.Add(Me.Label39)
        Me.grpHeader.Controls.Add(Me.Label29)
        Me.grpHeader.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpHeader.GroupImage = Nothing
        Me.grpHeader.GroupTitle = ""
        Me.grpHeader.Location = New System.Drawing.Point(7, -4)
        Me.grpHeader.Name = "grpHeader"
        Me.grpHeader.Padding = New System.Windows.Forms.Padding(20)
        Me.grpHeader.PaintGroupBox = False
        Me.grpHeader.RoundCorners = 10
        Me.grpHeader.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpHeader.ShadowControl = False
        Me.grpHeader.ShadowThickness = 3
        Me.grpHeader.Size = New System.Drawing.Size(1006, 153)
        Me.grpHeader.TabIndex = 0
        '
        'cmenuTemplate
        '
        Me.cmenuTemplate.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Style1ToolStripMenuItem, Me.Style2ToolStripMenuItem, Me.Style3ToolStripMenuItem, Me.Style4ToolStripMenuItem, Me.Style5ToolStripMenuItem, Me.Style6ToolStripMenuItem})
        Me.cmenuTemplate.Name = "cmenuTemplate"
        Me.cmenuTemplate.Size = New System.Drawing.Size(109, 136)
        '
        'Style1ToolStripMenuItem
        '
        Me.Style1ToolStripMenuItem.Name = "Style1ToolStripMenuItem"
        Me.Style1ToolStripMenuItem.Size = New System.Drawing.Size(108, 22)
        Me.Style1ToolStripMenuItem.Text = "Style 1"
        '
        'Style2ToolStripMenuItem
        '
        Me.Style2ToolStripMenuItem.Name = "Style2ToolStripMenuItem"
        Me.Style2ToolStripMenuItem.Size = New System.Drawing.Size(108, 22)
        Me.Style2ToolStripMenuItem.Text = "Style 2"
        '
        'Style3ToolStripMenuItem
        '
        Me.Style3ToolStripMenuItem.Name = "Style3ToolStripMenuItem"
        Me.Style3ToolStripMenuItem.Size = New System.Drawing.Size(108, 22)
        Me.Style3ToolStripMenuItem.Text = "Style 3"
        '
        'Style4ToolStripMenuItem
        '
        Me.Style4ToolStripMenuItem.Name = "Style4ToolStripMenuItem"
        Me.Style4ToolStripMenuItem.Size = New System.Drawing.Size(108, 22)
        Me.Style4ToolStripMenuItem.Text = "Style 4"
        '
        'Style5ToolStripMenuItem
        '
        Me.Style5ToolStripMenuItem.Name = "Style5ToolStripMenuItem"
        Me.Style5ToolStripMenuItem.Size = New System.Drawing.Size(108, 22)
        Me.Style5ToolStripMenuItem.Text = "Style 5"
        '
        'Style6ToolStripMenuItem
        '
        Me.Style6ToolStripMenuItem.Name = "Style6ToolStripMenuItem"
        Me.Style6ToolStripMenuItem.Size = New System.Drawing.Size(108, 22)
        Me.Style6ToolStripMenuItem.Text = "Style 6"
        '
        'lblNx
        '
        Me.lblNx.AutoSize = True
        Me.lblNx.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNx.Location = New System.Drawing.Point(871, 75)
        Me.lblNx.Name = "lblNx"
        Me.lblNx.Size = New System.Drawing.Size(12, 14)
        Me.lblNx.TabIndex = 34
        Me.lblNx.Text = ":"
        '
        'lblNext
        '
        Me.lblNext.AutoSize = True
        Me.lblNext.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNext.Location = New System.Drawing.Point(758, 73)
        Me.lblNext.Name = "lblNext"
        Me.lblNext.Size = New System.Drawing.Size(115, 16)
        Me.lblNext.TabIndex = 33
        Me.lblNext.Text = "Next Repair No"
        '
        'lblNextNo
        '
        Me.lblNextNo.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNextNo.Location = New System.Drawing.Point(903, 73)
        Me.lblNextNo.Name = "lblNextNo"
        Me.lblNextNo.Size = New System.Drawing.Size(85, 16)
        Me.lblNextNo.TabIndex = 32
        Me.lblNextNo.Text = "R181"
        Me.lblNextNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'dtpRepairDate
        '
        Me.dtpRepairDate.Location = New System.Drawing.Point(390, 89)
        Me.dtpRepairDate.Mask = "##/##/####"
        Me.dtpRepairDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpRepairDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpRepairDate.Name = "dtpRepairDate"
        Me.dtpRepairDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpRepairDate.Size = New System.Drawing.Size(91, 21)
        Me.dtpRepairDate.TabIndex = 3
        Me.dtpRepairDate.Text = "07/03/9998"
        Me.dtpRepairDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'lblOrderDate
        '
        Me.lblOrderDate.AutoSize = True
        Me.lblOrderDate.Location = New System.Drawing.Point(313, 93)
        Me.lblOrderDate.Name = "lblOrderDate"
        Me.lblOrderDate.Size = New System.Drawing.Size(75, 13)
        Me.lblOrderDate.TabIndex = 2
        Me.lblOrderDate.Text = "Repair Date"
        Me.lblOrderDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dtpDueDate
        '
        Me.dtpDueDate.Location = New System.Drawing.Point(195, 118)
        Me.dtpDueDate.Mask = "##/##/####"
        Me.dtpDueDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpDueDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpDueDate.Name = "dtpDueDate"
        Me.dtpDueDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpDueDate.Size = New System.Drawing.Size(93, 21)
        Me.dtpDueDate.TabIndex = 7
        Me.dtpDueDate.Text = "07/03/9998"
        Me.dtpDueDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpRemDate
        '
        Me.dtpRemDate.Location = New System.Drawing.Point(554, 118)
        Me.dtpRemDate.Mask = "##/##/####"
        Me.dtpRemDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpRemDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpRemDate.Name = "dtpRemDate"
        Me.dtpRemDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpRemDate.Size = New System.Drawing.Size(91, 21)
        Me.dtpRemDate.TabIndex = 11
        Me.dtpRemDate.Text = "07/03/9998"
        Me.dtpRemDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'pnlOrderType
        '
        Me.pnlOrderType.Controls.Add(Me.rbtCustomerOrderType)
        Me.pnlOrderType.Controls.Add(Me.rbtCompanyOrderType)
        Me.pnlOrderType.Location = New System.Drawing.Point(101, 89)
        Me.pnlOrderType.Name = "pnlOrderType"
        Me.pnlOrderType.Size = New System.Drawing.Size(200, 21)
        Me.pnlOrderType.TabIndex = 1
        '
        'rbtCustomerOrderType
        '
        Me.rbtCustomerOrderType.AutoSize = True
        Me.rbtCustomerOrderType.Checked = True
        Me.rbtCustomerOrderType.Location = New System.Drawing.Point(0, 1)
        Me.rbtCustomerOrderType.Name = "rbtCustomerOrderType"
        Me.rbtCustomerOrderType.Size = New System.Drawing.Size(81, 17)
        Me.rbtCustomerOrderType.TabIndex = 0
        Me.rbtCustomerOrderType.TabStop = True
        Me.rbtCustomerOrderType.Text = "Customer"
        Me.rbtCustomerOrderType.UseVisualStyleBackColor = True
        '
        'rbtCompanyOrderType
        '
        Me.rbtCompanyOrderType.AutoSize = True
        Me.rbtCompanyOrderType.Location = New System.Drawing.Point(91, 1)
        Me.rbtCompanyOrderType.Name = "rbtCompanyOrderType"
        Me.rbtCompanyOrderType.Size = New System.Drawing.Size(80, 17)
        Me.rbtCompanyOrderType.TabIndex = 1
        Me.rbtCompanyOrderType.Text = "Company"
        Me.rbtCompanyOrderType.UseVisualStyleBackColor = True
        '
        'txtDueDays_NUM
        '
        Me.txtDueDays_NUM.Location = New System.Drawing.Point(87, 118)
        Me.txtDueDays_NUM.Name = "txtDueDays_NUM"
        Me.txtDueDays_NUM.Size = New System.Drawing.Size(46, 21)
        Me.txtDueDays_NUM.TabIndex = 5
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.lblTitle.Location = New System.Drawing.Point(450, 33)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(117, 29)
        Me.lblTitle.TabIndex = 28
        Me.lblTitle.Text = "REPAIR"
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Location = New System.Drawing.Point(138, 122)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(61, 13)
        Me.Label31.TabIndex = 6
        Me.Label31.Text = "Due Date"
        Me.Label31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtRemDays
        '
        Me.txtRemDays.Location = New System.Drawing.Point(397, 118)
        Me.txtRemDays.Name = "txtRemDays"
        Me.txtRemDays.Size = New System.Drawing.Size(46, 21)
        Me.txtRemDays.TabIndex = 9
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Location = New System.Drawing.Point(23, 122)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(63, 13)
        Me.Label30.TabIndex = 4
        Me.Label30.Text = "Due Days"
        Me.Label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label89
        '
        Me.Label89.AutoSize = True
        Me.Label89.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label89.Location = New System.Drawing.Point(871, 54)
        Me.Label89.Name = "Label89"
        Me.Label89.Size = New System.Drawing.Size(12, 14)
        Me.Label89.TabIndex = 27
        Me.Label89.Text = ":"
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Location = New System.Drawing.Point(448, 122)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(100, 13)
        Me.Label32.TabIndex = 10
        Me.Label32.Text = "Remainder Date"
        Me.Label32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label90
        '
        Me.Label90.AutoSize = True
        Me.Label90.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label90.Location = New System.Drawing.Point(871, 37)
        Me.Label90.Name = "Label90"
        Me.Label90.Size = New System.Drawing.Size(12, 14)
        Me.Label90.TabIndex = 26
        Me.Label90.Text = ":"
        '
        'Label37
        '
        Me.Label37.AutoSize = True
        Me.Label37.Location = New System.Drawing.Point(294, 122)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(102, 13)
        Me.Label37.TabIndex = 8
        Me.Label37.Text = "Remainder Days"
        Me.Label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label91
        '
        Me.Label91.AutoSize = True
        Me.Label91.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label91.Location = New System.Drawing.Point(871, 19)
        Me.Label91.Name = "Label91"
        Me.Label91.Size = New System.Drawing.Size(12, 14)
        Me.Label91.TabIndex = 25
        Me.Label91.Text = ":"
        '
        'Label88
        '
        Me.Label88.AutoSize = True
        Me.Label88.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label88.Location = New System.Drawing.Point(149, 55)
        Me.Label88.Name = "Label88"
        Me.Label88.Size = New System.Drawing.Size(12, 14)
        Me.Label88.TabIndex = 22
        Me.Label88.Text = ":"
        '
        'Label87
        '
        Me.Label87.AutoSize = True
        Me.Label87.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label87.Location = New System.Drawing.Point(149, 37)
        Me.Label87.Name = "Label87"
        Me.Label87.Size = New System.Drawing.Size(12, 14)
        Me.Label87.TabIndex = 23
        Me.Label87.Text = ":"
        '
        'Label86
        '
        Me.Label86.AutoSize = True
        Me.Label86.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label86.Location = New System.Drawing.Point(149, 19)
        Me.Label86.Name = "Label86"
        Me.Label86.Size = New System.Drawing.Size(12, 14)
        Me.Label86.TabIndex = 24
        Me.Label86.Text = ":"
        '
        'lblSystemId
        '
        Me.lblSystemId.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSystemId.Location = New System.Drawing.Point(903, 54)
        Me.lblSystemId.Name = "lblSystemId"
        Me.lblSystemId.Size = New System.Drawing.Size(85, 16)
        Me.lblSystemId.TabIndex = 19
        Me.lblSystemId.Text = "N03"
        Me.lblSystemId.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblSilverRate
        '
        Me.lblSilverRate.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSilverRate.Location = New System.Drawing.Point(903, 36)
        Me.lblSilverRate.Name = "lblSilverRate"
        Me.lblSilverRate.Size = New System.Drawing.Size(85, 16)
        Me.lblSilverRate.TabIndex = 20
        Me.lblSilverRate.Text = "200.00"
        Me.lblSilverRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label43
        '
        Me.Label43.AutoSize = True
        Me.Label43.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label43.Location = New System.Drawing.Point(19, 18)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(97, 16)
        Me.Label43.TabIndex = 21
        Me.Label43.Text = "ORDER DATE"
        '
        'lblNodeId
        '
        Me.lblNodeId.AutoSize = True
        Me.lblNodeId.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNodeId.Location = New System.Drawing.Point(758, 54)
        Me.lblNodeId.Name = "lblNodeId"
        Me.lblNodeId.Size = New System.Drawing.Size(48, 16)
        Me.lblNodeId.TabIndex = 12
        Me.lblNodeId.Text = "NODE"
        '
        'Label44
        '
        Me.Label44.AutoSize = True
        Me.Label44.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label44.Location = New System.Drawing.Point(758, 36)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(99, 16)
        Me.Label44.TabIndex = 13
        Me.Label44.Text = "SILVER RATE"
        '
        'lblBillDate
        '
        Me.lblBillDate.AutoSize = True
        Me.lblBillDate.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBillDate.Location = New System.Drawing.Point(183, 18)
        Me.lblBillDate.Name = "lblBillDate"
        Me.lblBillDate.Size = New System.Drawing.Size(98, 16)
        Me.lblBillDate.TabIndex = 10
        Me.lblBillDate.Text = "08/03/2009"
        '
        'lblGoldRate
        '
        Me.lblGoldRate.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGoldRate.Location = New System.Drawing.Point(903, 18)
        Me.lblGoldRate.Name = "lblGoldRate"
        Me.lblGoldRate.Size = New System.Drawing.Size(85, 16)
        Me.lblGoldRate.TabIndex = 11
        Me.lblGoldRate.Text = "1520.00"
        Me.lblGoldRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblUserName
        '
        Me.lblUserName.AutoSize = True
        Me.lblUserName.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserName.Location = New System.Drawing.Point(183, 54)
        Me.lblUserName.Name = "lblUserName"
        Me.lblUserName.Size = New System.Drawing.Size(126, 16)
        Me.lblUserName.TabIndex = 14
        Me.lblUserName.Text = "ADMINISTRATOR"
        '
        'Label45
        '
        Me.Label45.AutoSize = True
        Me.Label45.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label45.Location = New System.Drawing.Point(19, 54)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(90, 16)
        Me.Label45.TabIndex = 17
        Me.Label45.Text = "USER NAME"
        '
        'Label55
        '
        Me.Label55.AutoSize = True
        Me.Label55.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label55.Location = New System.Drawing.Point(19, 36)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(119, 16)
        Me.Label55.TabIndex = 18
        Me.Label55.Text = "CASH COUNTER"
        '
        'Label56
        '
        Me.Label56.AutoSize = True
        Me.Label56.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label56.Location = New System.Drawing.Point(758, 18)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(87, 16)
        Me.Label56.TabIndex = 15
        Me.Label56.Text = "GOLD RATE"
        '
        'lblCashCounter
        '
        Me.lblCashCounter.AutoSize = True
        Me.lblCashCounter.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCashCounter.Location = New System.Drawing.Point(183, 36)
        Me.lblCashCounter.Name = "lblCashCounter"
        Me.lblCashCounter.Size = New System.Drawing.Size(101, 16)
        Me.lblCashCounter.TabIndex = 16
        Me.lblCashCounter.Text = "FIRST FLOOR"
        '
        'txtSalesManName
        '
        Me.txtSalesManName.Location = New System.Drawing.Point(768, 118)
        Me.txtSalesManName.Name = "txtSalesManName"
        Me.txtSalesManName.Size = New System.Drawing.Size(170, 21)
        Me.txtSalesManName.TabIndex = 14
        '
        'txtSalesMan_NUM
        '
        Me.txtSalesMan_NUM.Location = New System.Drawing.Point(720, 118)
        Me.txtSalesMan_NUM.Name = "txtSalesMan_NUM"
        Me.txtSalesMan_NUM.Size = New System.Drawing.Size(46, 21)
        Me.txtSalesMan_NUM.TabIndex = 13
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.Location = New System.Drawing.Point(649, 122)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(63, 13)
        Me.Label39.TabIndex = 12
        Me.Label39.Text = "Employee"
        Me.Label39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Location = New System.Drawing.Point(23, 93)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(72, 13)
        Me.Label29.TabIndex = 0
        Me.Label29.Text = "Order Type"
        Me.Label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AdvanceToolStripMenuItem, Me.ChitCardToolStripMenuItem, Me.ChequeToolStripMenuItem, Me.CreditCardToolStripMenuItem, Me.CashToolStripMenuItem, Me.WastageMcToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(183, 136)
        '
        'AdvanceToolStripMenuItem
        '
        Me.AdvanceToolStripMenuItem.Name = "AdvanceToolStripMenuItem"
        Me.AdvanceToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.AdvanceToolStripMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.AdvanceToolStripMenuItem.Text = "Advance"
        Me.AdvanceToolStripMenuItem.Visible = False
        '
        'ChitCardToolStripMenuItem
        '
        Me.ChitCardToolStripMenuItem.Name = "ChitCardToolStripMenuItem"
        Me.ChitCardToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9
        Me.ChitCardToolStripMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.ChitCardToolStripMenuItem.Text = "SCHEME"
        Me.ChitCardToolStripMenuItem.Visible = False
        '
        'ChequeToolStripMenuItem
        '
        Me.ChequeToolStripMenuItem.Name = "ChequeToolStripMenuItem"
        Me.ChequeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F8
        Me.ChequeToolStripMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.ChequeToolStripMenuItem.Text = "Cheque"
        Me.ChequeToolStripMenuItem.Visible = False
        '
        'CreditCardToolStripMenuItem
        '
        Me.CreditCardToolStripMenuItem.Name = "CreditCardToolStripMenuItem"
        Me.CreditCardToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F7
        Me.CreditCardToolStripMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.CreditCardToolStripMenuItem.Text = "Credit Card"
        Me.CreditCardToolStripMenuItem.Visible = False
        '
        'CashToolStripMenuItem
        '
        Me.CashToolStripMenuItem.Name = "CashToolStripMenuItem"
        Me.CashToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4
        Me.CashToolStripMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.CashToolStripMenuItem.Text = "Cash"
        Me.CashToolStripMenuItem.Visible = False
        '
        'WastageMcToolStripMenuItem
        '
        Me.WastageMcToolStripMenuItem.Name = "WastageMcToolStripMenuItem"
        Me.WastageMcToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F9), System.Windows.Forms.Keys)
        Me.WastageMcToolStripMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.WastageMcToolStripMenuItem.Text = "WastageMc"
        Me.WastageMcToolStripMenuItem.Visible = False
        '
        'pnlContainer_OWN
        '
        Me.pnlContainer_OWN.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.pnlContainer_OWN.Controls.Add(Me.grpHeader)
        Me.pnlContainer_OWN.Controls.Add(Me.tabOtherOptions)
        Me.pnlContainer_OWN.Controls.Add(Me.grpOrderDetails)
        Me.pnlContainer_OWN.Location = New System.Drawing.Point(0, 0)
        Me.pnlContainer_OWN.Name = "pnlContainer_OWN"
        Me.pnlContainer_OWN.Size = New System.Drawing.Size(1024, 640)
        Me.pnlContainer_OWN.TabIndex = 3
        '
        'frmRepair
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.ClientSize = New System.Drawing.Size(1020, 630)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlContainer_OWN)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmRepair"
        Me.Text = "Repair"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.tabOtherOptions.ResumeLayout(False)
        Me.tabAddress.ResumeLayout(False)
        Me.grpAdj.ResumeLayout(False)
        Me.grpAdj.PerformLayout()
        Me.grpAddress.ResumeLayout(False)
        Me.grpImage.ResumeLayout(False)
        CType(Me.picImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Grouper2.ResumeLayout(False)
        Me.tabStone.ResumeLayout(False)
        Me.grpStone.ResumeLayout(False)
        Me.grpStone.PerformLayout()
        CType(Me.gridStoneTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridStone, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpOrderDetails.ResumeLayout(False)
        Me.grpOrderDetails.PerformLayout()
        CType(Me.gridOrderTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridOrder, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpHeader.ResumeLayout(False)
        Me.grpHeader.PerformLayout()
        Me.cmenuTemplate.ResumeLayout(False)
        Me.pnlOrderType.ResumeLayout(False)
        Me.pnlOrderType.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.pnlContainer_OWN.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents tabOtherOptions As System.Windows.Forms.TabControl
    Friend WithEvents tabAddress As System.Windows.Forms.TabPage
    Friend WithEvents grpAddress As CodeVendor.Controls.Grouper
    Friend WithEvents tabStone As System.Windows.Forms.TabPage
    Friend WithEvents grpStone As CodeVendor.Controls.Grouper
    Friend WithEvents txtStSubItem As System.Windows.Forms.TextBox
    Friend WithEvents txtStItem As System.Windows.Forms.TextBox
    Friend WithEvents txtStRowIndex As System.Windows.Forms.TextBox
    Friend WithEvents gridStoneTotal As System.Windows.Forms.DataGridView
    Friend WithEvents gridStone As System.Windows.Forms.DataGridView
    Friend WithEvents Label61 As System.Windows.Forms.Label
    Friend WithEvents Label57 As System.Windows.Forms.Label
    Friend WithEvents cmbStCalc As System.Windows.Forms.ComboBox
    Friend WithEvents cmbStUnit As System.Windows.Forms.ComboBox
    Friend WithEvents txtStMetalCode As System.Windows.Forms.TextBox
    Friend WithEvents Label58 As System.Windows.Forms.Label
    Friend WithEvents Label60 As System.Windows.Forms.Label
    Friend WithEvents Label59 As System.Windows.Forms.Label
    Friend WithEvents Label47 As System.Windows.Forms.Label
    Friend WithEvents Label46 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents txtStPcs_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtStAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtStWeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtStRate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents grpAdj As CodeVendor.Controls.Grouper
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents txtAdjChitCard_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtAdjCash_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtAdjCheque_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtAdjCreditCard_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtAdjAdvanceWt As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents grpOrderDetails As CodeVendor.Controls.Grouper
    Friend WithEvents gridOrder As System.Windows.Forms.DataGridView
    Friend WithEvents Grouper2 As CodeVendor.Controls.Grouper
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents grpHeader As CodeVendor.Controls.Grouper
    Friend WithEvents txtRemDays As System.Windows.Forms.TextBox
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents txtDueDays_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents rbtCompanyOrderType As System.Windows.Forms.RadioButton
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents rbtCustomerOrderType As System.Windows.Forms.RadioButton
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents Label89 As System.Windows.Forms.Label
    Friend WithEvents Label90 As System.Windows.Forms.Label
    Friend WithEvents Label91 As System.Windows.Forms.Label
    Friend WithEvents Label88 As System.Windows.Forms.Label
    Friend WithEvents Label87 As System.Windows.Forms.Label
    Friend WithEvents Label86 As System.Windows.Forms.Label
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents lblNodeId As System.Windows.Forms.Label
    Friend WithEvents Label44 As System.Windows.Forms.Label
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents Label56 As System.Windows.Forms.Label
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents gridOrderTotal As System.Windows.Forms.DataGridView
    Friend WithEvents cmenuTemplate As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents Style1ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Style2ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Style3ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Style4ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Style5ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Style6ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlOrderType As System.Windows.Forms.Panel
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents txtOrderRowIndex As System.Windows.Forms.TextBox
    Friend WithEvents Label62 As System.Windows.Forms.Label
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents AdvanceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChitCardToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChequeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CreditCardToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CashToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WastageMcToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents lblSystemId As System.Windows.Forms.Label
    Public WithEvents lblBillDate As System.Windows.Forms.Label
    Public WithEvents lblUserName As System.Windows.Forms.Label
    Public WithEvents lblCashCounter As System.Windows.Forms.Label
    Public WithEvents lblSilverRate As System.Windows.Forms.Label
    Public WithEvents lblGoldRate As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label68 As System.Windows.Forms.Label
    Friend WithEvents Label69 As System.Windows.Forms.Label
    Friend WithEvents Label70 As System.Windows.Forms.Label
    Friend WithEvents Label71 As System.Windows.Forms.Label
    Friend WithEvents Label72 As System.Windows.Forms.Label
    Friend WithEvents pnlContainer_OWN As System.Windows.Forms.Panel
    Friend WithEvents dtpRemDate As BrighttechPack.DatePicker
    Friend WithEvents dtpDueDate As BrighttechPack.DatePicker
    Friend WithEvents grpImage As CodeVendor.Controls.Grouper
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents picImage As System.Windows.Forms.PictureBox
    Friend WithEvents chkOImage As System.Windows.Forms.CheckBox
    Friend WithEvents dtpRepairDate As BrighttechPack.DatePicker
    Friend WithEvents lblOrderDate As System.Windows.Forms.Label
    Friend WithEvents lblNx As System.Windows.Forms.Label
    Friend WithEvents lblNext As System.Windows.Forms.Label
    Public WithEvents lblNextNo As System.Windows.Forms.Label
    Public WithEvents txtODescription As TextBox
    Public WithEvents txtOItemName As TextBox
    Public WithEvents txtOMc_AMT As TextBox
    Public WithEvents txtOPcs_NUM As TextBox
    Public WithEvents txtOGrsWt_WET As TextBox
    Public WithEvents txtONetWt_WET As TextBox
    Public WithEvents txtOMcPerGrm_AMT As TextBox
    Public WithEvents txtONatureOfRepair As TextBox
    Public WithEvents txtOAmount_AMT As TextBox
    Public WithEvents txtAdjAdvance_AMT As TextBox
    Public WithEvents txtSalesManName As TextBox
    Public WithEvents txtSalesMan_NUM As TextBox
End Class
