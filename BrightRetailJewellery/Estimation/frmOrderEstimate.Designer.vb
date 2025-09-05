<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOrderEstimate
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtOParticular = New System.Windows.Forms.TextBox
        Me.txtOGrossAmount_AMT = New System.Windows.Forms.TextBox
        Me.txtOOtherAmt_AMT = New System.Windows.Forms.TextBox
        Me.txtOVat_AMT = New System.Windows.Forms.TextBox
        Me.txtOMc_AMT = New System.Windows.Forms.TextBox
        Me.txtOPcs_NUM = New System.Windows.Forms.TextBox
        Me.txtOAmount_AMT = New System.Windows.Forms.TextBox
        Me.txtOGrsWt_WET = New System.Windows.Forms.TextBox
        Me.txtORate_AMT = New System.Windows.Forms.TextBox
        Me.txtOWastage_WET = New System.Windows.Forms.TextBox
        Me.txtONetWt_WET = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.chkOSample = New System.Windows.Forms.CheckBox
        Me.chkOImage = New System.Windows.Forms.CheckBox
        Me.grpOrderDetails = New CodeVendor.Controls.Grouper
        Me.Label21 = New System.Windows.Forms.Label
        Me.txtOItemId = New System.Windows.Forms.TextBox
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.txtODiaPcsWt = New System.Windows.Forms.TextBox
        Me.txtOSize = New System.Windows.Forms.TextBox
        Me.txtOrderRowIndex = New System.Windows.Forms.TextBox
        Me.gridOrderTotal = New System.Windows.Forms.DataGridView
        Me.txtOCommision_AMT = New System.Windows.Forms.TextBox
        Me.gridOrder = New System.Windows.Forms.DataGridView
        Me.Label48 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.grpHeader = New CodeVendor.Controls.Grouper
        Me.cmenuTemplate = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Style1ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Style2ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Style3ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Style4ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Style5ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Style6ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.dtpOrderDate = New BrighttechPack.DatePicker(Me.components)
        Me.lblOrderDate = New System.Windows.Forms.Label
        Me.dtpRemDate = New BrighttechPack.DatePicker(Me.components)
        Me.dtpDueDate = New BrighttechPack.DatePicker(Me.components)
        Me.pnlOrderRate = New System.Windows.Forms.Panel
        Me.rbtCurrentRate = New System.Windows.Forms.RadioButton
        Me.rbtDeliveryRate = New System.Windows.Forms.RadioButton
        Me.pnlOrderType = New System.Windows.Forms.Panel
        Me.rbtCustomerOrderType = New System.Windows.Forms.RadioButton
        Me.txtDueDays_NUM = New System.Windows.Forms.TextBox
        Me.lblTitle = New System.Windows.Forms.Label
        Me.Label31 = New System.Windows.Forms.Label
        Me.txtRemDays_NUM = New System.Windows.Forms.TextBox
        Me.Label30 = New System.Windows.Forms.Label
        Me.Label89 = New System.Windows.Forms.Label
        Me.Label32 = New System.Windows.Forms.Label
        Me.Label90 = New System.Windows.Forms.Label
        Me.Label37 = New System.Windows.Forms.Label
        Me.Label91 = New System.Windows.Forms.Label
        Me.Label88 = New System.Windows.Forms.Label
        Me.Label87 = New System.Windows.Forms.Label
        Me.Label86 = New System.Windows.Forms.Label
        Me.lblSystemId = New System.Windows.Forms.Label
        Me.lblSilverRate = New System.Windows.Forms.Label
        Me.Label43 = New System.Windows.Forms.Label
        Me.lblNodeId = New System.Windows.Forms.Label
        Me.Label44 = New System.Windows.Forms.Label
        Me.lblBillDate = New System.Windows.Forms.Label
        Me.lblGoldRate = New System.Windows.Forms.Label
        Me.lblUserName = New System.Windows.Forms.Label
        Me.Label45 = New System.Windows.Forms.Label
        Me.Label55 = New System.Windows.Forms.Label
        Me.Label56 = New System.Windows.Forms.Label
        Me.lblCashCounter = New System.Windows.Forms.Label
        Me.txtSalesManName = New System.Windows.Forms.TextBox
        Me.txtSalesMan_NUM = New System.Windows.Forms.TextBox
        Me.Label39 = New System.Windows.Forms.Label
        Me.Label41 = New System.Windows.Forms.Label
        Me.Label29 = New System.Windows.Forms.Label
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AdvanceWeightToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AdvanceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ChitCardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ChequeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CreditCardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CashToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.WastageMcToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DiscountToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EstimateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DuplicateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.pnlContainer_OWN = New System.Windows.Forms.Panel
        Me.tabOtherOptions = New System.Windows.Forms.TabControl
        Me.tabAddress = New System.Windows.Forms.TabPage
        Me.grpAddress = New CodeVendor.Controls.Grouper
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblnetamount = New System.Windows.Forms.Label
        Me.lblBalance = New System.Windows.Forms.Label
        Me.txtOItem = New System.Windows.Forms.TextBox
        Me.txtOStyleNo = New System.Windows.Forms.TextBox
        Me.pnlFurtherAdvAddress = New System.Windows.Forms.Panel
        Me.Label73 = New System.Windows.Forms.Label
        Me.Label66 = New System.Windows.Forms.Label
        Me.lblHelp = New System.Windows.Forms.Label
        Me.Grouper2 = New CodeVendor.Controls.Grouper
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.tabStone = New System.Windows.Forms.TabPage
        Me.grpStone = New CodeVendor.Controls.Grouper
        Me.txtStSubItem = New System.Windows.Forms.TextBox
        Me.txtStItem = New System.Windows.Forms.TextBox
        Me.txtStRowIndex = New System.Windows.Forms.TextBox
        Me.gridStoneTotal = New System.Windows.Forms.DataGridView
        Me.gridStone = New System.Windows.Forms.DataGridView
        Me.Label61 = New System.Windows.Forms.Label
        Me.Label57 = New System.Windows.Forms.Label
        Me.cmbStCalc = New System.Windows.Forms.ComboBox
        Me.cmbStUnit = New System.Windows.Forms.ComboBox
        Me.txtStMetalCode = New System.Windows.Forms.TextBox
        Me.Label58 = New System.Windows.Forms.Label
        Me.Label60 = New System.Windows.Forms.Label
        Me.Label59 = New System.Windows.Forms.Label
        Me.Label47 = New System.Windows.Forms.Label
        Me.Label46 = New System.Windows.Forms.Label
        Me.Label26 = New System.Windows.Forms.Label
        Me.txtStPcs_NUM = New System.Windows.Forms.TextBox
        Me.txtStAmount_AMT = New System.Windows.Forms.TextBox
        Me.txtStWeight_WET = New System.Windows.Forms.TextBox
        Me.txtStRate_AMT = New System.Windows.Forms.TextBox
        Me.tabHideDet = New System.Windows.Forms.TabPage
        Me.grpImage = New CodeVendor.Controls.Grouper
        Me.btnBrowse = New System.Windows.Forms.Button
        Me.picImage = New System.Windows.Forms.PictureBox
        Me.pnlOrderExtraDet = New System.Windows.Forms.Panel
        Me.Label54 = New System.Windows.Forms.Label
        Me.txtOWastagePer_Per = New System.Windows.Forms.TextBox
        Me.txtOMcPerGrm_AMT = New System.Windows.Forms.TextBox
        Me.Label49 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.txtOCommGrm_AMT = New System.Windows.Forms.TextBox
        Me.grpDescription = New CodeVendor.Controls.Grouper
        Me.Label62 = New System.Windows.Forms.Label
        Me.txtODescription = New System.Windows.Forms.TextBox
        Me.tabSample = New System.Windows.Forms.TabPage
        Me.grpSample = New CodeVendor.Controls.Grouper
        Me.Label50 = New System.Windows.Forms.Label
        Me.Label65 = New System.Windows.Forms.Label
        Me.Label64 = New System.Windows.Forms.Label
        Me.Label63 = New System.Windows.Forms.Label
        Me.Label53 = New System.Windows.Forms.Label
        Me.Label52 = New System.Windows.Forms.Label
        Me.Label51 = New System.Windows.Forms.Label
        Me.Label36 = New System.Windows.Forms.Label
        Me.txtSamNetWt_WET = New System.Windows.Forms.TextBox
        Me.txtSamGrsWt_WET = New System.Windows.Forms.TextBox
        Me.txtSamPcs_NUM = New System.Windows.Forms.TextBox
        Me.txtSamTagNo = New System.Windows.Forms.TextBox
        Me.txtSamDescription = New System.Windows.Forms.TextBox
        Me.txtSamItem = New System.Windows.Forms.TextBox
        Me.cmbSamFrom = New System.Windows.Forms.ComboBox
        Me.cmbSamType = New System.Windows.Forms.ComboBox
        Me.gridSample = New System.Windows.Forms.DataGridView
        Me.grpOrderDetails.SuspendLayout()
        CType(Me.gridOrderTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridOrder, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpHeader.SuspendLayout()
        Me.cmenuTemplate.SuspendLayout()
        Me.pnlOrderRate.SuspendLayout()
        Me.pnlOrderType.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.pnlContainer_OWN.SuspendLayout()
        Me.tabOtherOptions.SuspendLayout()
        Me.tabAddress.SuspendLayout()
        Me.grpAddress.SuspendLayout()
        Me.pnlFurtherAdvAddress.SuspendLayout()
        Me.Grouper2.SuspendLayout()
        Me.tabStone.SuspendLayout()
        Me.grpStone.SuspendLayout()
        CType(Me.gridStoneTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridStone, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabHideDet.SuspendLayout()
        Me.grpImage.SuspendLayout()
        CType(Me.picImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlOrderExtraDet.SuspendLayout()
        Me.grpDescription.SuspendLayout()
        Me.tabSample.SuspendLayout()
        Me.grpSample.SuspendLayout()
        CType(Me.gridSample, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(62, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(117, 17)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Item"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(182, 23)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(35, 17)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Pcs"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(216, 23)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(59, 17)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "GrsWt"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(276, 23)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(59, 17)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "NetWt"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(377, 23)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(69, 17)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Rate"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(759, 23)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(84, 17)
        Me.Label7.TabIndex = 24
        Me.Label7.Text = "GrossAmt"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(844, 23)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(60, 17)
        Me.Label8.TabIndex = 26
        Me.Label8.Text = "Vat"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(905, 23)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(73, 17)
        Me.Label9.TabIndex = 28
        Me.Label9.Text = "Total"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtOParticular
        '
        Me.txtOParticular.Location = New System.Drawing.Point(62, 40)
        Me.txtOParticular.Name = "txtOParticular"
        Me.txtOParticular.Size = New System.Drawing.Size(117, 21)
        Me.txtOParticular.TabIndex = 3
        Me.txtOParticular.Text = "Culcutta Bangle Kasai"
        '
        'txtOGrossAmount_AMT
        '
        Me.txtOGrossAmount_AMT.Location = New System.Drawing.Point(759, 40)
        Me.txtOGrossAmount_AMT.MaxLength = 12
        Me.txtOGrossAmount_AMT.Name = "txtOGrossAmount_AMT"
        Me.txtOGrossAmount_AMT.Size = New System.Drawing.Size(84, 21)
        Me.txtOGrossAmount_AMT.TabIndex = 25
        '
        'txtOOtherAmt_AMT
        '
        Me.txtOOtherAmt_AMT.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOOtherAmt_AMT.Location = New System.Drawing.Point(688, 40)
        Me.txtOOtherAmt_AMT.MaxLength = 12
        Me.txtOOtherAmt_AMT.Name = "txtOOtherAmt_AMT"
        Me.txtOOtherAmt_AMT.Size = New System.Drawing.Size(70, 21)
        Me.txtOOtherAmt_AMT.TabIndex = 23
        Me.txtOOtherAmt_AMT.Text = "99999.99"
        '
        'txtOVat_AMT
        '
        Me.txtOVat_AMT.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOVat_AMT.Location = New System.Drawing.Point(844, 40)
        Me.txtOVat_AMT.MaxLength = 12
        Me.txtOVat_AMT.Name = "txtOVat_AMT"
        Me.txtOVat_AMT.Size = New System.Drawing.Size(60, 21)
        Me.txtOVat_AMT.TabIndex = 27
        Me.txtOVat_AMT.Text = "99999.99"
        '
        'txtOMc_AMT
        '
        Me.txtOMc_AMT.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOMc_AMT.Location = New System.Drawing.Point(501, 40)
        Me.txtOMc_AMT.MaxLength = 12
        Me.txtOMc_AMT.Name = "txtOMc_AMT"
        Me.txtOMc_AMT.Size = New System.Drawing.Size(60, 21)
        Me.txtOMc_AMT.TabIndex = 17
        Me.txtOMc_AMT.Text = "99999.99"
        '
        'txtOPcs_NUM
        '
        Me.txtOPcs_NUM.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOPcs_NUM.Location = New System.Drawing.Point(180, 40)
        Me.txtOPcs_NUM.MaxLength = 5
        Me.txtOPcs_NUM.Name = "txtOPcs_NUM"
        Me.txtOPcs_NUM.Size = New System.Drawing.Size(35, 21)
        Me.txtOPcs_NUM.TabIndex = 5
        Me.txtOPcs_NUM.Text = "99999"
        '
        'txtOAmount_AMT
        '
        Me.txtOAmount_AMT.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOAmount_AMT.Location = New System.Drawing.Point(905, 40)
        Me.txtOAmount_AMT.MaxLength = 12
        Me.txtOAmount_AMT.Name = "txtOAmount_AMT"
        Me.txtOAmount_AMT.Size = New System.Drawing.Size(73, 21)
        Me.txtOAmount_AMT.TabIndex = 29
        Me.txtOAmount_AMT.Text = "9999900.99"
        '
        'txtOGrsWt_WET
        '
        Me.txtOGrsWt_WET.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOGrsWt_WET.Location = New System.Drawing.Point(216, 40)
        Me.txtOGrsWt_WET.MaxLength = 10
        Me.txtOGrsWt_WET.Name = "txtOGrsWt_WET"
        Me.txtOGrsWt_WET.Size = New System.Drawing.Size(59, 21)
        Me.txtOGrsWt_WET.TabIndex = 7
        Me.txtOGrsWt_WET.Text = "9999.999"
        '
        'txtORate_AMT
        '
        Me.txtORate_AMT.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtORate_AMT.Location = New System.Drawing.Point(377, 40)
        Me.txtORate_AMT.MaxLength = 10
        Me.txtORate_AMT.Name = "txtORate_AMT"
        Me.txtORate_AMT.Size = New System.Drawing.Size(69, 21)
        Me.txtORate_AMT.TabIndex = 13
        Me.txtORate_AMT.Text = "999999.99"
        '
        'txtOWastage_WET
        '
        Me.txtOWastage_WET.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOWastage_WET.Location = New System.Drawing.Point(447, 40)
        Me.txtOWastage_WET.MaxLength = 10
        Me.txtOWastage_WET.Name = "txtOWastage_WET"
        Me.txtOWastage_WET.Size = New System.Drawing.Size(53, 21)
        Me.txtOWastage_WET.TabIndex = 15
        Me.txtOWastage_WET.Text = "99999.99"
        '
        'txtONetWt_WET
        '
        Me.txtONetWt_WET.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtONetWt_WET.Location = New System.Drawing.Point(276, 40)
        Me.txtONetWt_WET.MaxLength = 10
        Me.txtONetWt_WET.Name = "txtONetWt_WET"
        Me.txtONetWt_WET.Size = New System.Drawing.Size(59, 21)
        Me.txtONetWt_WET.TabIndex = 9
        Me.txtONetWt_WET.Text = "999.999"
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(688, 23)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(70, 17)
        Me.Label13.TabIndex = 22
        Me.Label13.Text = "Stn Amt"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(447, 23)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(53, 17)
        Me.Label10.TabIndex = 14
        Me.Label10.Text = "Wastage"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(501, 23)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(60, 17)
        Me.Label11.TabIndex = 16
        Me.Label11.Text = "Mc"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'chkOSample
        '
        Me.chkOSample.BackColor = System.Drawing.Color.Transparent
        Me.chkOSample.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.chkOSample.Location = New System.Drawing.Point(979, 43)
        Me.chkOSample.Name = "chkOSample"
        Me.chkOSample.Size = New System.Drawing.Size(15, 14)
        Me.chkOSample.TabIndex = 31
        Me.chkOSample.UseVisualStyleBackColor = False
        '
        'chkOImage
        '
        Me.chkOImage.BackColor = System.Drawing.Color.Transparent
        Me.chkOImage.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.chkOImage.Location = New System.Drawing.Point(998, 43)
        Me.chkOImage.Name = "chkOImage"
        Me.chkOImage.Size = New System.Drawing.Size(15, 14)
        Me.chkOImage.TabIndex = 32
        Me.chkOImage.UseVisualStyleBackColor = False
        '
        'grpOrderDetails
        '
        Me.grpOrderDetails.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpOrderDetails.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpOrderDetails.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpOrderDetails.BorderColor = System.Drawing.Color.Transparent
        Me.grpOrderDetails.BorderThickness = 1.0!
        Me.grpOrderDetails.Controls.Add(Me.Label21)
        Me.grpOrderDetails.Controls.Add(Me.txtOItemId)
        Me.grpOrderDetails.Controls.Add(Me.Label20)
        Me.grpOrderDetails.Controls.Add(Me.Label19)
        Me.grpOrderDetails.Controls.Add(Me.txtODiaPcsWt)
        Me.grpOrderDetails.Controls.Add(Me.txtOSize)
        Me.grpOrderDetails.Controls.Add(Me.txtOAmount_AMT)
        Me.grpOrderDetails.Controls.Add(Me.txtOrderRowIndex)
        Me.grpOrderDetails.Controls.Add(Me.gridOrderTotal)
        Me.grpOrderDetails.Controls.Add(Me.txtOCommision_AMT)
        Me.grpOrderDetails.Controls.Add(Me.gridOrder)
        Me.grpOrderDetails.Controls.Add(Me.Label48)
        Me.grpOrderDetails.Controls.Add(Me.Label1)
        Me.grpOrderDetails.Controls.Add(Me.Label16)
        Me.grpOrderDetails.Controls.Add(Me.Label3)
        Me.grpOrderDetails.Controls.Add(Me.Label4)
        Me.grpOrderDetails.Controls.Add(Me.chkOImage)
        Me.grpOrderDetails.Controls.Add(Me.Label5)
        Me.grpOrderDetails.Controls.Add(Me.chkOSample)
        Me.grpOrderDetails.Controls.Add(Me.Label6)
        Me.grpOrderDetails.Controls.Add(Me.txtOParticular)
        Me.grpOrderDetails.Controls.Add(Me.Label10)
        Me.grpOrderDetails.Controls.Add(Me.txtOGrossAmount_AMT)
        Me.grpOrderDetails.Controls.Add(Me.Label11)
        Me.grpOrderDetails.Controls.Add(Me.txtOOtherAmt_AMT)
        Me.grpOrderDetails.Controls.Add(Me.Label13)
        Me.grpOrderDetails.Controls.Add(Me.txtOVat_AMT)
        Me.grpOrderDetails.Controls.Add(Me.Label7)
        Me.grpOrderDetails.Controls.Add(Me.txtOMc_AMT)
        Me.grpOrderDetails.Controls.Add(Me.Label8)
        Me.grpOrderDetails.Controls.Add(Me.txtOPcs_NUM)
        Me.grpOrderDetails.Controls.Add(Me.Label9)
        Me.grpOrderDetails.Controls.Add(Me.txtOGrsWt_WET)
        Me.grpOrderDetails.Controls.Add(Me.txtORate_AMT)
        Me.grpOrderDetails.Controls.Add(Me.txtONetWt_WET)
        Me.grpOrderDetails.Controls.Add(Me.txtOWastage_WET)
        Me.grpOrderDetails.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpOrderDetails.GroupImage = Nothing
        Me.grpOrderDetails.GroupTitle = ""
        Me.grpOrderDetails.Location = New System.Drawing.Point(8, 147)
        Me.grpOrderDetails.Name = "grpOrderDetails"
        Me.grpOrderDetails.Padding = New System.Windows.Forms.Padding(20)
        Me.grpOrderDetails.PaintGroupBox = False
        Me.grpOrderDetails.RoundCorners = 10
        Me.grpOrderDetails.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpOrderDetails.ShadowControl = False
        Me.grpOrderDetails.ShadowThickness = 3
        Me.grpOrderDetails.Size = New System.Drawing.Size(1013, 314)
        Me.grpOrderDetails.TabIndex = 1
        '
        'Label21
        '
        Me.Label21.Location = New System.Drawing.Point(2, 23)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(59, 17)
        Me.Label21.TabIndex = 0
        Me.Label21.Text = "ItemId"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtOItemId
        '
        Me.txtOItemId.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOItemId.Location = New System.Drawing.Point(7, 40)
        Me.txtOItemId.MaxLength = 10
        Me.txtOItemId.Name = "txtOItemId"
        Me.txtOItemId.Size = New System.Drawing.Size(54, 21)
        Me.txtOItemId.TabIndex = 1
        '
        'Label20
        '
        Me.Label20.Location = New System.Drawing.Point(960, 11)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(65, 29)
        Me.Label20.TabIndex = 30
        Me.Label20.Text = "Sample\ Image"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label19
        '
        Me.Label19.Location = New System.Drawing.Point(619, 11)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(70, 26)
        Me.Label19.TabIndex = 20
        Me.Label19.Text = "Dia " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Pcs/Wt"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtODiaPcsWt
        '
        Me.txtODiaPcsWt.Location = New System.Drawing.Point(617, 40)
        Me.txtODiaPcsWt.Name = "txtODiaPcsWt"
        Me.txtODiaPcsWt.Size = New System.Drawing.Size(70, 21)
        Me.txtODiaPcsWt.TabIndex = 21
        '
        'txtOSize
        '
        Me.txtOSize.Location = New System.Drawing.Point(336, 40)
        Me.txtOSize.MaxLength = 10
        Me.txtOSize.Name = "txtOSize"
        Me.txtOSize.Size = New System.Drawing.Size(40, 21)
        Me.txtOSize.TabIndex = 11
        Me.txtOSize.Text = "123456"
        '
        'txtOrderRowIndex
        '
        Me.txtOrderRowIndex.Location = New System.Drawing.Point(470, 183)
        Me.txtOrderRowIndex.Name = "txtOrderRowIndex"
        Me.txtOrderRowIndex.Size = New System.Drawing.Size(60, 21)
        Me.txtOrderRowIndex.TabIndex = 34
        Me.txtOrderRowIndex.Visible = False
        '
        'gridOrderTotal
        '
        Me.gridOrderTotal.AllowUserToAddRows = False
        Me.gridOrderTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridOrderTotal.Enabled = False
        Me.gridOrderTotal.Location = New System.Drawing.Point(7, 276)
        Me.gridOrderTotal.Name = "gridOrderTotal"
        Me.gridOrderTotal.ReadOnly = True
        Me.gridOrderTotal.Size = New System.Drawing.Size(1003, 28)
        Me.gridOrderTotal.TabIndex = 35
        '
        'txtOCommision_AMT
        '
        Me.txtOCommision_AMT.Location = New System.Drawing.Point(562, 40)
        Me.txtOCommision_AMT.Name = "txtOCommision_AMT"
        Me.txtOCommision_AMT.Size = New System.Drawing.Size(54, 21)
        Me.txtOCommision_AMT.TabIndex = 19
        '
        'gridOrder
        '
        Me.gridOrder.AllowUserToAddRows = False
        Me.gridOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridOrder.Location = New System.Drawing.Point(7, 62)
        Me.gridOrder.Name = "gridOrder"
        Me.gridOrder.ReadOnly = True
        Me.gridOrder.Size = New System.Drawing.Size(1003, 214)
        Me.gridOrder.TabIndex = 33
        '
        'Label48
        '
        Me.Label48.Location = New System.Drawing.Point(562, 24)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(54, 13)
        Me.Label48.TabIndex = 18
        Me.Label48.Text = "Comm"
        Me.Label48.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label16
        '
        Me.Label16.Location = New System.Drawing.Point(337, 23)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(40, 17)
        Me.Label16.TabIndex = 10
        Me.Label16.Text = "Size"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpHeader
        '
        Me.grpHeader.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpHeader.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpHeader.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpHeader.BorderColor = System.Drawing.Color.Transparent
        Me.grpHeader.BorderThickness = 1.0!
        Me.grpHeader.ContextMenuStrip = Me.cmenuTemplate
        Me.grpHeader.Controls.Add(Me.dtpOrderDate)
        Me.grpHeader.Controls.Add(Me.lblOrderDate)
        Me.grpHeader.Controls.Add(Me.dtpRemDate)
        Me.grpHeader.Controls.Add(Me.dtpDueDate)
        Me.grpHeader.Controls.Add(Me.pnlOrderRate)
        Me.grpHeader.Controls.Add(Me.pnlOrderType)
        Me.grpHeader.Controls.Add(Me.txtDueDays_NUM)
        Me.grpHeader.Controls.Add(Me.lblTitle)
        Me.grpHeader.Controls.Add(Me.Label31)
        Me.grpHeader.Controls.Add(Me.txtRemDays_NUM)
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
        Me.grpHeader.Controls.Add(Me.Label41)
        Me.grpHeader.Controls.Add(Me.Label29)
        Me.grpHeader.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpHeader.GroupImage = Nothing
        Me.grpHeader.GroupTitle = ""
        Me.grpHeader.Location = New System.Drawing.Point(8, -1)
        Me.grpHeader.Name = "grpHeader"
        Me.grpHeader.Padding = New System.Windows.Forms.Padding(20)
        Me.grpHeader.PaintGroupBox = False
        Me.grpHeader.RoundCorners = 10
        Me.grpHeader.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpHeader.ShadowControl = False
        Me.grpHeader.ShadowThickness = 3
        Me.grpHeader.Size = New System.Drawing.Size(1014, 152)
        Me.grpHeader.TabIndex = 0
        '
        'cmenuTemplate
        '
        Me.cmenuTemplate.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Style1ToolStripMenuItem, Me.Style2ToolStripMenuItem, Me.Style3ToolStripMenuItem, Me.Style4ToolStripMenuItem, Me.Style5ToolStripMenuItem, Me.Style6ToolStripMenuItem})
        Me.cmenuTemplate.Name = "cmenuTemplate"
        Me.cmenuTemplate.Size = New System.Drawing.Size(119, 136)
        '
        'Style1ToolStripMenuItem
        '
        Me.Style1ToolStripMenuItem.Name = "Style1ToolStripMenuItem"
        Me.Style1ToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.Style1ToolStripMenuItem.Text = "Style 1"
        '
        'Style2ToolStripMenuItem
        '
        Me.Style2ToolStripMenuItem.Name = "Style2ToolStripMenuItem"
        Me.Style2ToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.Style2ToolStripMenuItem.Text = "Style 2"
        '
        'Style3ToolStripMenuItem
        '
        Me.Style3ToolStripMenuItem.Name = "Style3ToolStripMenuItem"
        Me.Style3ToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.Style3ToolStripMenuItem.Text = "Style 3"
        '
        'Style4ToolStripMenuItem
        '
        Me.Style4ToolStripMenuItem.Name = "Style4ToolStripMenuItem"
        Me.Style4ToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.Style4ToolStripMenuItem.Text = "Style 4"
        '
        'Style5ToolStripMenuItem
        '
        Me.Style5ToolStripMenuItem.Name = "Style5ToolStripMenuItem"
        Me.Style5ToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.Style5ToolStripMenuItem.Text = "Style 5"
        '
        'Style6ToolStripMenuItem
        '
        Me.Style6ToolStripMenuItem.Name = "Style6ToolStripMenuItem"
        Me.Style6ToolStripMenuItem.Size = New System.Drawing.Size(118, 22)
        Me.Style6ToolStripMenuItem.Text = "Style 6"
        '
        'dtpOrderDate
        '
        Me.dtpOrderDate.Location = New System.Drawing.Point(553, 89)
        Me.dtpOrderDate.Mask = "##/##/####"
        Me.dtpOrderDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpOrderDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpOrderDate.Name = "dtpOrderDate"
        Me.dtpOrderDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpOrderDate.Size = New System.Drawing.Size(91, 21)
        Me.dtpOrderDate.TabIndex = 5
        Me.dtpOrderDate.Text = "07/03/9998"
        Me.dtpOrderDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'lblOrderDate
        '
        Me.lblOrderDate.AutoSize = True
        Me.lblOrderDate.Location = New System.Drawing.Point(467, 93)
        Me.lblOrderDate.Name = "lblOrderDate"
        Me.lblOrderDate.Size = New System.Drawing.Size(71, 13)
        Me.lblOrderDate.TabIndex = 4
        Me.lblOrderDate.Text = "Order Date"
        Me.lblOrderDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dtpRemDate
        '
        Me.dtpRemDate.Location = New System.Drawing.Point(551, 118)
        Me.dtpRemDate.Mask = "##/##/####"
        Me.dtpRemDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpRemDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpRemDate.Name = "dtpRemDate"
        Me.dtpRemDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpRemDate.Size = New System.Drawing.Size(91, 21)
        Me.dtpRemDate.TabIndex = 13
        Me.dtpRemDate.Text = "07/03/9998"
        Me.dtpRemDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'dtpDueDate
        '
        Me.dtpDueDate.Location = New System.Drawing.Point(196, 118)
        Me.dtpDueDate.Mask = "##/##/####"
        Me.dtpDueDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpDueDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpDueDate.Name = "dtpDueDate"
        Me.dtpDueDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpDueDate.Size = New System.Drawing.Size(91, 21)
        Me.dtpDueDate.TabIndex = 9
        Me.dtpDueDate.Text = "07/03/9998"
        Me.dtpDueDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'pnlOrderRate
        '
        Me.pnlOrderRate.Controls.Add(Me.rbtCurrentRate)
        Me.pnlOrderRate.Controls.Add(Me.rbtDeliveryRate)
        Me.pnlOrderRate.Location = New System.Drawing.Point(92, 88)
        Me.pnlOrderRate.Name = "pnlOrderRate"
        Me.pnlOrderRate.Size = New System.Drawing.Size(350, 22)
        Me.pnlOrderRate.TabIndex = 3
        '
        'rbtCurrentRate
        '
        Me.rbtCurrentRate.AutoSize = True
        Me.rbtCurrentRate.Location = New System.Drawing.Point(2, 3)
        Me.rbtCurrentRate.Name = "rbtCurrentRate"
        Me.rbtCurrentRate.Size = New System.Drawing.Size(173, 17)
        Me.rbtCurrentRate.TabIndex = 0
        Me.rbtCurrentRate.TabStop = True
        Me.rbtCurrentRate.Text = "Current Rate (Rate Fixed)"
        Me.rbtCurrentRate.UseVisualStyleBackColor = True
        '
        'rbtDeliveryRate
        '
        Me.rbtDeliveryRate.AutoSize = True
        Me.rbtDeliveryRate.Location = New System.Drawing.Point(204, 3)
        Me.rbtDeliveryRate.Name = "rbtDeliveryRate"
        Me.rbtDeliveryRate.Size = New System.Drawing.Size(103, 17)
        Me.rbtDeliveryRate.TabIndex = 1
        Me.rbtDeliveryRate.TabStop = True
        Me.rbtDeliveryRate.Text = "Delivery Rate"
        Me.rbtDeliveryRate.UseVisualStyleBackColor = True
        '
        'pnlOrderType
        '
        Me.pnlOrderType.Controls.Add(Me.rbtCustomerOrderType)
        Me.pnlOrderType.Location = New System.Drawing.Point(851, 89)
        Me.pnlOrderType.Name = "pnlOrderType"
        Me.pnlOrderType.Size = New System.Drawing.Size(98, 21)
        Me.pnlOrderType.TabIndex = 1
        '
        'rbtCustomerOrderType
        '
        Me.rbtCustomerOrderType.AutoSize = True
        Me.rbtCustomerOrderType.Checked = True
        Me.rbtCustomerOrderType.Location = New System.Drawing.Point(2, 1)
        Me.rbtCustomerOrderType.Name = "rbtCustomerOrderType"
        Me.rbtCustomerOrderType.Size = New System.Drawing.Size(81, 17)
        Me.rbtCustomerOrderType.TabIndex = 0
        Me.rbtCustomerOrderType.TabStop = True
        Me.rbtCustomerOrderType.Text = "Customer"
        Me.rbtCustomerOrderType.UseVisualStyleBackColor = True
        Me.rbtCustomerOrderType.Visible = False
        '
        'txtDueDays_NUM
        '
        Me.txtDueDays_NUM.Location = New System.Drawing.Point(86, 118)
        Me.txtDueDays_NUM.Name = "txtDueDays_NUM"
        Me.txtDueDays_NUM.Size = New System.Drawing.Size(46, 21)
        Me.txtDueDays_NUM.TabIndex = 7
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.lblTitle.Location = New System.Drawing.Point(378, 25)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(251, 29)
        Me.lblTitle.TabIndex = 28
        Me.lblTitle.Text = "ORDER ESTIMATE"
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Location = New System.Drawing.Point(137, 122)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(61, 13)
        Me.Label31.TabIndex = 8
        Me.Label31.Text = "Due Date"
        Me.Label31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtRemDays_NUM
        '
        Me.txtRemDays_NUM.Location = New System.Drawing.Point(396, 118)
        Me.txtRemDays_NUM.Name = "txtRemDays_NUM"
        Me.txtRemDays_NUM.Size = New System.Drawing.Size(46, 21)
        Me.txtRemDays_NUM.TabIndex = 11
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Location = New System.Drawing.Point(22, 122)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(63, 13)
        Me.Label30.TabIndex = 6
        Me.Label30.Text = "Due Days"
        Me.Label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label89
        '
        Me.Label89.AutoSize = True
        Me.Label89.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label89.Location = New System.Drawing.Point(870, 54)
        Me.Label89.Name = "Label89"
        Me.Label89.Size = New System.Drawing.Size(12, 14)
        Me.Label89.TabIndex = 27
        Me.Label89.Text = ":"
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Location = New System.Drawing.Point(447, 122)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(100, 13)
        Me.Label32.TabIndex = 12
        Me.Label32.Text = "Remainder Date"
        Me.Label32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label90
        '
        Me.Label90.AutoSize = True
        Me.Label90.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label90.Location = New System.Drawing.Point(870, 37)
        Me.Label90.Name = "Label90"
        Me.Label90.Size = New System.Drawing.Size(12, 14)
        Me.Label90.TabIndex = 26
        Me.Label90.Text = ":"
        '
        'Label37
        '
        Me.Label37.AutoSize = True
        Me.Label37.Location = New System.Drawing.Point(293, 122)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(102, 13)
        Me.Label37.TabIndex = 10
        Me.Label37.Text = "Remainder Days"
        Me.Label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label91
        '
        Me.Label91.AutoSize = True
        Me.Label91.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label91.Location = New System.Drawing.Point(870, 19)
        Me.Label91.Name = "Label91"
        Me.Label91.Size = New System.Drawing.Size(12, 14)
        Me.Label91.TabIndex = 25
        Me.Label91.Text = ":"
        '
        'Label88
        '
        Me.Label88.AutoSize = True
        Me.Label88.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label88.Location = New System.Drawing.Point(148, 39)
        Me.Label88.Name = "Label88"
        Me.Label88.Size = New System.Drawing.Size(12, 14)
        Me.Label88.TabIndex = 22
        Me.Label88.Text = ":"
        '
        'Label87
        '
        Me.Label87.AutoSize = True
        Me.Label87.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label87.Location = New System.Drawing.Point(148, 55)
        Me.Label87.Name = "Label87"
        Me.Label87.Size = New System.Drawing.Size(12, 14)
        Me.Label87.TabIndex = 23
        Me.Label87.Text = ":"
        Me.Label87.Visible = False
        '
        'Label86
        '
        Me.Label86.AutoSize = True
        Me.Label86.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label86.Location = New System.Drawing.Point(148, 19)
        Me.Label86.Name = "Label86"
        Me.Label86.Size = New System.Drawing.Size(12, 14)
        Me.Label86.TabIndex = 24
        Me.Label86.Text = ":"
        '
        'lblSystemId
        '
        Me.lblSystemId.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSystemId.Location = New System.Drawing.Point(902, 54)
        Me.lblSystemId.Name = "lblSystemId"
        Me.lblSystemId.Size = New System.Drawing.Size(85, 16)
        Me.lblSystemId.TabIndex = 19
        Me.lblSystemId.Text = "N03"
        Me.lblSystemId.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblSilverRate
        '
        Me.lblSilverRate.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSilverRate.Location = New System.Drawing.Point(902, 36)
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
        Me.Label43.Location = New System.Drawing.Point(18, 18)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(97, 16)
        Me.Label43.TabIndex = 21
        Me.Label43.Text = "ORDER DATE"
        '
        'lblNodeId
        '
        Me.lblNodeId.AutoSize = True
        Me.lblNodeId.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNodeId.Location = New System.Drawing.Point(757, 54)
        Me.lblNodeId.Name = "lblNodeId"
        Me.lblNodeId.Size = New System.Drawing.Size(48, 16)
        Me.lblNodeId.TabIndex = 12
        Me.lblNodeId.Text = "NODE"
        '
        'Label44
        '
        Me.Label44.AutoSize = True
        Me.Label44.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label44.Location = New System.Drawing.Point(757, 36)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(99, 16)
        Me.Label44.TabIndex = 13
        Me.Label44.Text = "SILVER RATE"
        '
        'lblBillDate
        '
        Me.lblBillDate.AutoSize = True
        Me.lblBillDate.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBillDate.Location = New System.Drawing.Point(182, 18)
        Me.lblBillDate.Name = "lblBillDate"
        Me.lblBillDate.Size = New System.Drawing.Size(98, 16)
        Me.lblBillDate.TabIndex = 10
        Me.lblBillDate.Text = "08/03/2009"
        '
        'lblGoldRate
        '
        Me.lblGoldRate.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGoldRate.Location = New System.Drawing.Point(902, 18)
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
        Me.lblUserName.Location = New System.Drawing.Point(182, 38)
        Me.lblUserName.Name = "lblUserName"
        Me.lblUserName.Size = New System.Drawing.Size(126, 16)
        Me.lblUserName.TabIndex = 14
        Me.lblUserName.Text = "ADMINISTRATOR"
        '
        'Label45
        '
        Me.Label45.AutoSize = True
        Me.Label45.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label45.Location = New System.Drawing.Point(18, 38)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(90, 16)
        Me.Label45.TabIndex = 17
        Me.Label45.Text = "USER NAME"
        '
        'Label55
        '
        Me.Label55.AutoSize = True
        Me.Label55.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label55.Location = New System.Drawing.Point(18, 54)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(119, 16)
        Me.Label55.TabIndex = 18
        Me.Label55.Text = "CASH COUNTER"
        Me.Label55.Visible = False
        '
        'Label56
        '
        Me.Label56.AutoSize = True
        Me.Label56.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label56.Location = New System.Drawing.Point(757, 18)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(87, 16)
        Me.Label56.TabIndex = 15
        Me.Label56.Text = "GOLD RATE"
        '
        'lblCashCounter
        '
        Me.lblCashCounter.AutoSize = True
        Me.lblCashCounter.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCashCounter.Location = New System.Drawing.Point(182, 54)
        Me.lblCashCounter.Name = "lblCashCounter"
        Me.lblCashCounter.Size = New System.Drawing.Size(101, 16)
        Me.lblCashCounter.TabIndex = 16
        Me.lblCashCounter.Text = "FIRST FLOOR"
        Me.lblCashCounter.Visible = False
        '
        'txtSalesManName
        '
        Me.txtSalesManName.Location = New System.Drawing.Point(770, 118)
        Me.txtSalesManName.Name = "txtSalesManName"
        Me.txtSalesManName.Size = New System.Drawing.Size(170, 21)
        Me.txtSalesManName.TabIndex = 16
        '
        'txtSalesMan_NUM
        '
        Me.txtSalesMan_NUM.Location = New System.Drawing.Point(722, 118)
        Me.txtSalesMan_NUM.Name = "txtSalesMan_NUM"
        Me.txtSalesMan_NUM.Size = New System.Drawing.Size(46, 21)
        Me.txtSalesMan_NUM.TabIndex = 15
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.Location = New System.Drawing.Point(651, 122)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(63, 13)
        Me.Label39.TabIndex = 14
        Me.Label39.Text = "Employee"
        Me.Label39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label41
        '
        Me.Label41.AutoSize = True
        Me.Label41.Location = New System.Drawing.Point(22, 92)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(70, 13)
        Me.Label41.TabIndex = 2
        Me.Label41.Text = "Order Rate"
        Me.Label41.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Location = New System.Drawing.Point(772, 93)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(72, 13)
        Me.Label29.TabIndex = 0
        Me.Label29.Text = "Order Type"
        Me.Label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.Label29.Visible = False
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AdvanceWeightToolStripMenuItem, Me.AdvanceToolStripMenuItem, Me.ChitCardToolStripMenuItem, Me.ChequeToolStripMenuItem, Me.CreditCardToolStripMenuItem, Me.CashToolStripMenuItem, Me.WastageMcToolStripMenuItem, Me.DiscountToolStripMenuItem, Me.EstimateToolStripMenuItem, Me.DuplicateToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(186, 224)
        '
        'AdvanceWeightToolStripMenuItem
        '
        Me.AdvanceWeightToolStripMenuItem.Name = "AdvanceWeightToolStripMenuItem"
        Me.AdvanceWeightToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.AdvanceWeightToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.AdvanceWeightToolStripMenuItem.Text = "Advance Weight"
        Me.AdvanceWeightToolStripMenuItem.Visible = False
        '
        'AdvanceToolStripMenuItem
        '
        Me.AdvanceToolStripMenuItem.Name = "AdvanceToolStripMenuItem"
        Me.AdvanceToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.AdvanceToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.AdvanceToolStripMenuItem.Text = "Advance"
        Me.AdvanceToolStripMenuItem.Visible = False
        '
        'ChitCardToolStripMenuItem
        '
        Me.ChitCardToolStripMenuItem.Name = "ChitCardToolStripMenuItem"
        Me.ChitCardToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9
        Me.ChitCardToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.ChitCardToolStripMenuItem.Text = "SCHEME"
        Me.ChitCardToolStripMenuItem.Visible = False
        '
        'ChequeToolStripMenuItem
        '
        Me.ChequeToolStripMenuItem.Name = "ChequeToolStripMenuItem"
        Me.ChequeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F8
        Me.ChequeToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.ChequeToolStripMenuItem.Text = "Cheque"
        Me.ChequeToolStripMenuItem.Visible = False
        '
        'CreditCardToolStripMenuItem
        '
        Me.CreditCardToolStripMenuItem.Name = "CreditCardToolStripMenuItem"
        Me.CreditCardToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F7
        Me.CreditCardToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.CreditCardToolStripMenuItem.Text = "Credit Card"
        Me.CreditCardToolStripMenuItem.Visible = False
        '
        'CashToolStripMenuItem
        '
        Me.CashToolStripMenuItem.Name = "CashToolStripMenuItem"
        Me.CashToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4
        Me.CashToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.CashToolStripMenuItem.Text = "Cash"
        Me.CashToolStripMenuItem.Visible = False
        '
        'WastageMcToolStripMenuItem
        '
        Me.WastageMcToolStripMenuItem.Name = "WastageMcToolStripMenuItem"
        Me.WastageMcToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F9), System.Windows.Forms.Keys)
        Me.WastageMcToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.WastageMcToolStripMenuItem.Text = "WastageMc"
        Me.WastageMcToolStripMenuItem.Visible = False
        '
        'DiscountToolStripMenuItem
        '
        Me.DiscountToolStripMenuItem.Name = "DiscountToolStripMenuItem"
        Me.DiscountToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.DiscountToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.DiscountToolStripMenuItem.Text = "Discount"
        Me.DiscountToolStripMenuItem.Visible = False
        '
        'EstimateToolStripMenuItem
        '
        Me.EstimateToolStripMenuItem.Name = "EstimateToolStripMenuItem"
        Me.EstimateToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.EstimateToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.EstimateToolStripMenuItem.Text = "Estimate"
        '
        'DuplicateToolStripMenuItem
        '
        Me.DuplicateToolStripMenuItem.Name = "DuplicateToolStripMenuItem"
        Me.DuplicateToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
        Me.DuplicateToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.DuplicateToolStripMenuItem.Text = "Duplicate"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem})
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        Me.ExitToolStripMenuItem.Visible = False
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(125, 22)
        Me.NewToolStripMenuItem.Text = "New"
        Me.NewToolStripMenuItem.Visible = False
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
        'tabOtherOptions
        '
        Me.tabOtherOptions.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tabOtherOptions.Controls.Add(Me.tabAddress)
        Me.tabOtherOptions.Controls.Add(Me.tabStone)
        Me.tabOtherOptions.Controls.Add(Me.tabHideDet)
        Me.tabOtherOptions.Controls.Add(Me.tabSample)
        Me.tabOtherOptions.ItemSize = New System.Drawing.Size(1, 10)
        Me.tabOtherOptions.Location = New System.Drawing.Point(1, 457)
        Me.tabOtherOptions.Name = "tabOtherOptions"
        Me.tabOtherOptions.SelectedIndex = 0
        Me.tabOtherOptions.Size = New System.Drawing.Size(1021, 179)
        Me.tabOtherOptions.TabIndex = 2
        '
        'tabAddress
        '
        Me.tabAddress.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.tabAddress.Controls.Add(Me.grpAddress)
        Me.tabAddress.Controls.Add(Me.Grouper2)
        Me.tabAddress.Location = New System.Drawing.Point(4, 14)
        Me.tabAddress.Name = "tabAddress"
        Me.tabAddress.Padding = New System.Windows.Forms.Padding(3)
        Me.tabAddress.Size = New System.Drawing.Size(1013, 161)
        Me.tabAddress.TabIndex = 0
        Me.tabAddress.Text = "TabPage1"
        Me.tabAddress.UseVisualStyleBackColor = True
        '
        'grpAddress
        '
        Me.grpAddress.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpAddress.BackgroundGradientColor = System.Drawing.Color.White
        Me.grpAddress.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpAddress.BorderColor = System.Drawing.Color.Transparent
        Me.grpAddress.BorderThickness = 1.0!
        Me.grpAddress.Controls.Add(Me.Label2)
        Me.grpAddress.Controls.Add(Me.lblnetamount)
        Me.grpAddress.Controls.Add(Me.lblBalance)
        Me.grpAddress.Controls.Add(Me.txtOItem)
        Me.grpAddress.Controls.Add(Me.txtOStyleNo)
        Me.grpAddress.Controls.Add(Me.pnlFurtherAdvAddress)
        Me.grpAddress.Controls.Add(Me.lblHelp)
        Me.grpAddress.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpAddress.GroupImage = Nothing
        Me.grpAddress.GroupTitle = ""
        Me.grpAddress.Location = New System.Drawing.Point(3, -9)
        Me.grpAddress.Name = "grpAddress"
        Me.grpAddress.Padding = New System.Windows.Forms.Padding(20)
        Me.grpAddress.PaintGroupBox = False
        Me.grpAddress.RoundCorners = 10
        Me.grpAddress.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpAddress.ShadowControl = False
        Me.grpAddress.ShadowThickness = 3
        Me.grpAddress.Size = New System.Drawing.Size(678, 157)
        Me.grpAddress.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(351, 110)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(178, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "[Ctrl + D] - Duplicate Print"
        '
        'lblnetamount
        '
        Me.lblnetamount.AutoSize = True
        Me.lblnetamount.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblnetamount.ForeColor = System.Drawing.Color.Red
        Me.lblnetamount.Location = New System.Drawing.Point(349, 21)
        Me.lblnetamount.Name = "lblnetamount"
        Me.lblnetamount.Size = New System.Drawing.Size(119, 18)
        Me.lblnetamount.TabIndex = 11
        Me.lblnetamount.Text = "Total Amount"
        '
        'lblBalance
        '
        Me.lblBalance.AutoSize = True
        Me.lblBalance.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBalance.ForeColor = System.Drawing.Color.Red
        Me.lblBalance.Location = New System.Drawing.Point(349, 46)
        Me.lblBalance.Name = "lblBalance"
        Me.lblBalance.Size = New System.Drawing.Size(71, 18)
        Me.lblBalance.TabIndex = 7
        Me.lblBalance.Text = "Balance"
        '
        'txtOItem
        '
        Me.txtOItem.Location = New System.Drawing.Point(17, 48)
        Me.txtOItem.Name = "txtOItem"
        Me.txtOItem.Size = New System.Drawing.Size(115, 21)
        Me.txtOItem.TabIndex = 6
        Me.txtOItem.Text = "Culcutta Bangle Kasai"
        Me.txtOItem.Visible = False
        '
        'txtOStyleNo
        '
        Me.txtOStyleNo.Location = New System.Drawing.Point(17, 18)
        Me.txtOStyleNo.Name = "txtOStyleNo"
        Me.txtOStyleNo.Size = New System.Drawing.Size(115, 21)
        Me.txtOStyleNo.TabIndex = 5
        Me.txtOStyleNo.Visible = False
        '
        'pnlFurtherAdvAddress
        '
        Me.pnlFurtherAdvAddress.Controls.Add(Me.Label73)
        Me.pnlFurtherAdvAddress.Controls.Add(Me.Label66)
        Me.pnlFurtherAdvAddress.Location = New System.Drawing.Point(138, 8)
        Me.pnlFurtherAdvAddress.Name = "pnlFurtherAdvAddress"
        Me.pnlFurtherAdvAddress.Size = New System.Drawing.Size(205, 139)
        Me.pnlFurtherAdvAddress.TabIndex = 4
        Me.pnlFurtherAdvAddress.Visible = False
        '
        'Label73
        '
        Me.Label73.AutoSize = True
        Me.Label73.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label73.ForeColor = System.Drawing.Color.Blue
        Me.Label73.Location = New System.Drawing.Point(8, 30)
        Me.Label73.Name = "Label73"
        Me.Label73.Size = New System.Drawing.Size(175, 104)
        Me.Label73.TabIndex = 1
        Me.Label73.Text = "Mr/Ms.    Customer  Name" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Address1" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Address2" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Address3" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Area" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "City - Pincode" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Pho" & _
            "ne Res" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Mobile"
        Me.Label73.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label66
        '
        Me.Label66.AutoSize = True
        Me.Label66.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label66.ForeColor = System.Drawing.Color.Red
        Me.Label66.Location = New System.Drawing.Point(8, 7)
        Me.Label66.Name = "Label66"
        Me.Label66.Size = New System.Drawing.Size(163, 18)
        Me.Label66.TabIndex = 0
        Me.Label66.Text = "Order No : O1020"
        Me.Label66.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblHelp
        '
        Me.lblHelp.AutoSize = True
        Me.lblHelp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHelp.ForeColor = System.Drawing.Color.Red
        Me.lblHelp.Location = New System.Drawing.Point(351, 74)
        Me.lblHelp.Name = "lblHelp"
        Me.lblHelp.Size = New System.Drawing.Size(302, 13)
        Me.lblHelp.TabIndex = 3
        Me.lblHelp.Text = "[Ctrl + F9] -  Wastage%, Mc/Grm,Comm/Grm"
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
        Me.Grouper2.Location = New System.Drawing.Point(687, -9)
        Me.Grouper2.Name = "Grouper2"
        Me.Grouper2.Padding = New System.Windows.Forms.Padding(20)
        Me.Grouper2.PaintGroupBox = False
        Me.Grouper2.RoundCorners = 10
        Me.Grouper2.ShadowColor = System.Drawing.Color.DarkGray
        Me.Grouper2.ShadowControl = False
        Me.Grouper2.ShadowThickness = 3
        Me.Grouper2.Size = New System.Drawing.Size(322, 157)
        Me.Grouper2.TabIndex = 2
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(219, 36)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(101, 30)
        Me.btnExit.TabIndex = 2
        Me.btnExit.Text = "E&xit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Location = New System.Drawing.Point(112, 37)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(101, 30)
        Me.btnNew.TabIndex = 1
        Me.btnNew.Text = "&New"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(5, 37)
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
        Me.tabStone.Size = New System.Drawing.Size(1013, 161)
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
        Me.grpStone.Location = New System.Drawing.Point(3, -5)
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
        Me.txtStSubItem.Location = New System.Drawing.Point(261, 30)
        Me.txtStSubItem.Name = "txtStSubItem"
        Me.txtStSubItem.Size = New System.Drawing.Size(224, 21)
        Me.txtStSubItem.TabIndex = 3
        '
        'txtStItem
        '
        Me.txtStItem.Location = New System.Drawing.Point(5, 30)
        Me.txtStItem.Name = "txtStItem"
        Me.txtStItem.Size = New System.Drawing.Size(255, 21)
        Me.txtStItem.TabIndex = 1
        '
        'txtStRowIndex
        '
        Me.txtStRowIndex.Location = New System.Drawing.Point(491, 111)
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
        Me.gridStoneTotal.Location = New System.Drawing.Point(5, 174)
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
        Me.gridStone.Location = New System.Drawing.Point(5, 52)
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
        Me.Label61.Location = New System.Drawing.Point(871, 14)
        Me.Label61.Name = "Label61"
        Me.Label61.Size = New System.Drawing.Size(95, 15)
        Me.Label61.TabIndex = 14
        Me.Label61.Text = "Amount"
        Me.Label61.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label57
        '
        Me.Label57.BackColor = System.Drawing.Color.Transparent
        Me.Label57.Location = New System.Drawing.Point(621, 14)
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
        Me.cmbStCalc.Location = New System.Drawing.Point(696, 30)
        Me.cmbStCalc.Name = "cmbStCalc"
        Me.cmbStCalc.Size = New System.Drawing.Size(74, 21)
        Me.cmbStCalc.TabIndex = 11
        '
        'cmbStUnit
        '
        Me.cmbStUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbStUnit.FormattingEnabled = True
        Me.cmbStUnit.Location = New System.Drawing.Point(621, 30)
        Me.cmbStUnit.Name = "cmbStUnit"
        Me.cmbStUnit.Size = New System.Drawing.Size(74, 21)
        Me.cmbStUnit.TabIndex = 9
        '
        'txtStMetalCode
        '
        Me.txtStMetalCode.Enabled = False
        Me.txtStMetalCode.Location = New System.Drawing.Point(493, 91)
        Me.txtStMetalCode.Name = "txtStMetalCode"
        Me.txtStMetalCode.Size = New System.Drawing.Size(8, 21)
        Me.txtStMetalCode.TabIndex = 21
        Me.txtStMetalCode.Visible = False
        '
        'Label58
        '
        Me.Label58.BackColor = System.Drawing.Color.Transparent
        Me.Label58.Location = New System.Drawing.Point(696, 14)
        Me.Label58.Name = "Label58"
        Me.Label58.Size = New System.Drawing.Size(74, 15)
        Me.Label58.TabIndex = 10
        Me.Label58.Text = "Cal"
        Me.Label58.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label60
        '
        Me.Label60.BackColor = System.Drawing.Color.Transparent
        Me.Label60.Location = New System.Drawing.Point(771, 14)
        Me.Label60.Name = "Label60"
        Me.Label60.Size = New System.Drawing.Size(99, 15)
        Me.Label60.TabIndex = 12
        Me.Label60.Text = "Rate"
        Me.Label60.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label59
        '
        Me.Label59.BackColor = System.Drawing.Color.Transparent
        Me.Label59.Location = New System.Drawing.Point(532, 14)
        Me.Label59.Name = "Label59"
        Me.Label59.Size = New System.Drawing.Size(88, 15)
        Me.Label59.TabIndex = 6
        Me.Label59.Text = "Weight"
        Me.Label59.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label47
        '
        Me.Label47.BackColor = System.Drawing.Color.Transparent
        Me.Label47.Location = New System.Drawing.Point(5, 14)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(255, 15)
        Me.Label47.TabIndex = 0
        Me.Label47.Text = "Item"
        Me.Label47.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label46
        '
        Me.Label46.BackColor = System.Drawing.Color.Transparent
        Me.Label46.Location = New System.Drawing.Point(261, 14)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(224, 15)
        Me.Label46.TabIndex = 2
        Me.Label46.Text = "Sub Item"
        Me.Label46.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label26
        '
        Me.Label26.BackColor = System.Drawing.Color.Transparent
        Me.Label26.Location = New System.Drawing.Point(486, 14)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(45, 15)
        Me.Label26.TabIndex = 4
        Me.Label26.Text = "Pcs"
        Me.Label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtStPcs_NUM
        '
        Me.txtStPcs_NUM.Location = New System.Drawing.Point(486, 30)
        Me.txtStPcs_NUM.MaxLength = 8
        Me.txtStPcs_NUM.Name = "txtStPcs_NUM"
        Me.txtStPcs_NUM.Size = New System.Drawing.Size(45, 21)
        Me.txtStPcs_NUM.TabIndex = 5
        Me.txtStPcs_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtStAmount_AMT
        '
        Me.txtStAmount_AMT.Location = New System.Drawing.Point(871, 30)
        Me.txtStAmount_AMT.MaxLength = 12
        Me.txtStAmount_AMT.Name = "txtStAmount_AMT"
        Me.txtStAmount_AMT.Size = New System.Drawing.Size(99, 21)
        Me.txtStAmount_AMT.TabIndex = 15
        Me.txtStAmount_AMT.Text = "1234567.00"
        Me.txtStAmount_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtStWeight_WET
        '
        Me.txtStWeight_WET.Location = New System.Drawing.Point(532, 30)
        Me.txtStWeight_WET.MaxLength = 10
        Me.txtStWeight_WET.Name = "txtStWeight_WET"
        Me.txtStWeight_WET.Size = New System.Drawing.Size(88, 21)
        Me.txtStWeight_WET.TabIndex = 7
        Me.txtStWeight_WET.Text = "9999.999"
        Me.txtStWeight_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtStRate_AMT
        '
        Me.txtStRate_AMT.Location = New System.Drawing.Point(771, 30)
        Me.txtStRate_AMT.MaxLength = 10
        Me.txtStRate_AMT.Name = "txtStRate_AMT"
        Me.txtStRate_AMT.Size = New System.Drawing.Size(99, 21)
        Me.txtStRate_AMT.TabIndex = 13
        Me.txtStRate_AMT.Text = "500000.99"
        Me.txtStRate_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tabHideDet
        '
        Me.tabHideDet.Controls.Add(Me.grpImage)
        Me.tabHideDet.Controls.Add(Me.pnlOrderExtraDet)
        Me.tabHideDet.Controls.Add(Me.grpDescription)
        Me.tabHideDet.Location = New System.Drawing.Point(4, 14)
        Me.tabHideDet.Name = "tabHideDet"
        Me.tabHideDet.Size = New System.Drawing.Size(1013, 161)
        Me.tabHideDet.TabIndex = 9
        Me.tabHideDet.Text = "Hide Details"
        Me.tabHideDet.UseVisualStyleBackColor = True
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
        Me.grpImage.Location = New System.Drawing.Point(629, 3)
        Me.grpImage.Name = "grpImage"
        Me.grpImage.Padding = New System.Windows.Forms.Padding(20)
        Me.grpImage.PaintGroupBox = False
        Me.grpImage.RoundCorners = 10
        Me.grpImage.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpImage.ShadowControl = False
        Me.grpImage.ShadowThickness = 3
        Me.grpImage.Size = New System.Drawing.Size(248, 199)
        Me.grpImage.TabIndex = 1
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
        'pnlOrderExtraDet
        '
        Me.pnlOrderExtraDet.Controls.Add(Me.Label54)
        Me.pnlOrderExtraDet.Controls.Add(Me.txtOWastagePer_Per)
        Me.pnlOrderExtraDet.Controls.Add(Me.txtOMcPerGrm_AMT)
        Me.pnlOrderExtraDet.Controls.Add(Me.Label49)
        Me.pnlOrderExtraDet.Controls.Add(Me.Label15)
        Me.pnlOrderExtraDet.Controls.Add(Me.txtOCommGrm_AMT)
        Me.pnlOrderExtraDet.Location = New System.Drawing.Point(15, 86)
        Me.pnlOrderExtraDet.Name = "pnlOrderExtraDet"
        Me.pnlOrderExtraDet.Size = New System.Drawing.Size(186, 97)
        Me.pnlOrderExtraDet.TabIndex = 0
        '
        'Label54
        '
        Me.Label54.AutoSize = True
        Me.Label54.Location = New System.Drawing.Point(5, 13)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(72, 13)
        Me.Label54.TabIndex = 0
        Me.Label54.Text = "Wastage %"
        '
        'txtOWastagePer_Per
        '
        Me.txtOWastagePer_Per.Location = New System.Drawing.Point(83, 9)
        Me.txtOWastagePer_Per.Name = "txtOWastagePer_Per"
        Me.txtOWastagePer_Per.Size = New System.Drawing.Size(92, 21)
        Me.txtOWastagePer_Per.TabIndex = 1
        '
        'txtOMcPerGrm_AMT
        '
        Me.txtOMcPerGrm_AMT.Location = New System.Drawing.Point(83, 38)
        Me.txtOMcPerGrm_AMT.Name = "txtOMcPerGrm_AMT"
        Me.txtOMcPerGrm_AMT.Size = New System.Drawing.Size(92, 21)
        Me.txtOMcPerGrm_AMT.TabIndex = 3
        '
        'Label49
        '
        Me.Label49.AutoSize = True
        Me.Label49.Location = New System.Drawing.Point(5, 42)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(52, 13)
        Me.Label49.TabIndex = 2
        Me.Label49.Text = "Mc/Grm"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(5, 71)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(75, 13)
        Me.Label15.TabIndex = 4
        Me.Label15.Text = "Comm/Grm"
        '
        'txtOCommGrm_AMT
        '
        Me.txtOCommGrm_AMT.Location = New System.Drawing.Point(83, 67)
        Me.txtOCommGrm_AMT.Name = "txtOCommGrm_AMT"
        Me.txtOCommGrm_AMT.Size = New System.Drawing.Size(92, 21)
        Me.txtOCommGrm_AMT.TabIndex = 5
        '
        'grpDescription
        '
        Me.grpDescription.BackgroundColor = System.Drawing.SystemColors.InactiveCaption
        Me.grpDescription.BackgroundGradientColor = System.Drawing.Color.White
        Me.grpDescription.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpDescription.BorderColor = System.Drawing.Color.LightGray
        Me.grpDescription.BorderThickness = 1.0!
        Me.grpDescription.Controls.Add(Me.Label62)
        Me.grpDescription.Controls.Add(Me.txtODescription)
        Me.grpDescription.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpDescription.GroupImage = Nothing
        Me.grpDescription.GroupTitle = ""
        Me.grpDescription.Location = New System.Drawing.Point(13, 1)
        Me.grpDescription.Name = "grpDescription"
        Me.grpDescription.Padding = New System.Windows.Forms.Padding(20)
        Me.grpDescription.PaintGroupBox = False
        Me.grpDescription.RoundCorners = 10
        Me.grpDescription.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpDescription.ShadowControl = False
        Me.grpDescription.ShadowThickness = 3
        Me.grpDescription.Size = New System.Drawing.Size(515, 71)
        Me.grpDescription.TabIndex = 0
        '
        'Label62
        '
        Me.Label62.AutoSize = True
        Me.Label62.Location = New System.Drawing.Point(213, 20)
        Me.Label62.Name = "Label62"
        Me.Label62.Size = New System.Drawing.Size(71, 13)
        Me.Label62.TabIndex = 1
        Me.Label62.Text = "Description"
        '
        'txtODescription
        '
        Me.txtODescription.Location = New System.Drawing.Point(16, 36)
        Me.txtODescription.MaxLength = 250
        Me.txtODescription.Name = "txtODescription"
        Me.txtODescription.Size = New System.Drawing.Size(482, 21)
        Me.txtODescription.TabIndex = 1
        Me.txtODescription.Text = "012345678900123456789001234567890012345678900123456789001234567890"
        '
        'tabSample
        '
        Me.tabSample.Controls.Add(Me.grpSample)
        Me.tabSample.Location = New System.Drawing.Point(4, 14)
        Me.tabSample.Name = "tabSample"
        Me.tabSample.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSample.Size = New System.Drawing.Size(1013, 161)
        Me.tabSample.TabIndex = 10
        Me.tabSample.Text = "Sample"
        Me.tabSample.UseVisualStyleBackColor = True
        '
        'grpSample
        '
        Me.grpSample.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpSample.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpSample.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpSample.BorderColor = System.Drawing.Color.Transparent
        Me.grpSample.BorderThickness = 1.0!
        Me.grpSample.Controls.Add(Me.Label50)
        Me.grpSample.Controls.Add(Me.Label65)
        Me.grpSample.Controls.Add(Me.Label64)
        Me.grpSample.Controls.Add(Me.Label63)
        Me.grpSample.Controls.Add(Me.Label53)
        Me.grpSample.Controls.Add(Me.Label52)
        Me.grpSample.Controls.Add(Me.Label51)
        Me.grpSample.Controls.Add(Me.Label36)
        Me.grpSample.Controls.Add(Me.txtSamNetWt_WET)
        Me.grpSample.Controls.Add(Me.txtSamGrsWt_WET)
        Me.grpSample.Controls.Add(Me.txtSamPcs_NUM)
        Me.grpSample.Controls.Add(Me.txtSamTagNo)
        Me.grpSample.Controls.Add(Me.txtSamDescription)
        Me.grpSample.Controls.Add(Me.txtSamItem)
        Me.grpSample.Controls.Add(Me.cmbSamFrom)
        Me.grpSample.Controls.Add(Me.cmbSamType)
        Me.grpSample.Controls.Add(Me.gridSample)
        Me.grpSample.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpSample.GroupImage = Nothing
        Me.grpSample.GroupTitle = ""
        Me.grpSample.Location = New System.Drawing.Point(3, -24)
        Me.grpSample.Name = "grpSample"
        Me.grpSample.Padding = New System.Windows.Forms.Padding(20)
        Me.grpSample.PaintGroupBox = False
        Me.grpSample.RoundCorners = 10
        Me.grpSample.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpSample.ShadowControl = False
        Me.grpSample.ShadowThickness = 3
        Me.grpSample.Size = New System.Drawing.Size(1006, 175)
        Me.grpSample.TabIndex = 1
        '
        'Label50
        '
        Me.Label50.Location = New System.Drawing.Point(9, 24)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(97, 13)
        Me.Label50.TabIndex = 0
        Me.Label50.Text = "Type"
        Me.Label50.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label65
        '
        Me.Label65.Location = New System.Drawing.Point(912, 24)
        Me.Label65.Name = "Label65"
        Me.Label65.Size = New System.Drawing.Size(59, 13)
        Me.Label65.TabIndex = 14
        Me.Label65.Text = "NetWt"
        Me.Label65.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label64
        '
        Me.Label64.Location = New System.Drawing.Point(852, 24)
        Me.Label64.Name = "Label64"
        Me.Label64.Size = New System.Drawing.Size(59, 13)
        Me.Label64.TabIndex = 12
        Me.Label64.Text = "GrsWt"
        Me.Label64.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label63
        '
        Me.Label63.Location = New System.Drawing.Point(816, 24)
        Me.Label63.Name = "Label63"
        Me.Label63.Size = New System.Drawing.Size(35, 13)
        Me.Label63.TabIndex = 10
        Me.Label63.Text = "Pcs"
        Me.Label63.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label53
        '
        Me.Label53.Location = New System.Drawing.Point(756, 24)
        Me.Label53.Name = "Label53"
        Me.Label53.Size = New System.Drawing.Size(59, 13)
        Me.Label53.TabIndex = 8
        Me.Label53.Text = "TagNo"
        Me.Label53.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label52
        '
        Me.Label52.Location = New System.Drawing.Point(337, 24)
        Me.Label52.Name = "Label52"
        Me.Label52.Size = New System.Drawing.Size(418, 13)
        Me.Label52.TabIndex = 6
        Me.Label52.Text = "Description"
        Me.Label52.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label51
        '
        Me.Label51.Location = New System.Drawing.Point(197, 24)
        Me.Label51.Name = "Label51"
        Me.Label51.Size = New System.Drawing.Size(139, 13)
        Me.Label51.TabIndex = 4
        Me.Label51.Text = "Item"
        Me.Label51.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label36
        '
        Me.Label36.Location = New System.Drawing.Point(107, 24)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(89, 13)
        Me.Label36.TabIndex = 2
        Me.Label36.Text = "From"
        Me.Label36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtSamNetWt_WET
        '
        Me.txtSamNetWt_WET.Location = New System.Drawing.Point(912, 39)
        Me.txtSamNetWt_WET.Name = "txtSamNetWt_WET"
        Me.txtSamNetWt_WET.Size = New System.Drawing.Size(59, 21)
        Me.txtSamNetWt_WET.TabIndex = 15
        '
        'txtSamGrsWt_WET
        '
        Me.txtSamGrsWt_WET.Location = New System.Drawing.Point(852, 39)
        Me.txtSamGrsWt_WET.Name = "txtSamGrsWt_WET"
        Me.txtSamGrsWt_WET.Size = New System.Drawing.Size(59, 21)
        Me.txtSamGrsWt_WET.TabIndex = 13
        '
        'txtSamPcs_NUM
        '
        Me.txtSamPcs_NUM.Location = New System.Drawing.Point(816, 39)
        Me.txtSamPcs_NUM.Name = "txtSamPcs_NUM"
        Me.txtSamPcs_NUM.Size = New System.Drawing.Size(35, 21)
        Me.txtSamPcs_NUM.TabIndex = 11
        '
        'txtSamTagNo
        '
        Me.txtSamTagNo.Location = New System.Drawing.Point(756, 39)
        Me.txtSamTagNo.Name = "txtSamTagNo"
        Me.txtSamTagNo.Size = New System.Drawing.Size(59, 21)
        Me.txtSamTagNo.TabIndex = 9
        '
        'txtSamDescription
        '
        Me.txtSamDescription.Location = New System.Drawing.Point(337, 39)
        Me.txtSamDescription.MaxLength = 150
        Me.txtSamDescription.Name = "txtSamDescription"
        Me.txtSamDescription.Size = New System.Drawing.Size(418, 21)
        Me.txtSamDescription.TabIndex = 7
        '
        'txtSamItem
        '
        Me.txtSamItem.Location = New System.Drawing.Point(197, 39)
        Me.txtSamItem.Name = "txtSamItem"
        Me.txtSamItem.Size = New System.Drawing.Size(139, 21)
        Me.txtSamItem.TabIndex = 5
        '
        'cmbSamFrom
        '
        Me.cmbSamFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSamFrom.FormattingEnabled = True
        Me.cmbSamFrom.Location = New System.Drawing.Point(107, 39)
        Me.cmbSamFrom.Name = "cmbSamFrom"
        Me.cmbSamFrom.Size = New System.Drawing.Size(89, 21)
        Me.cmbSamFrom.TabIndex = 3
        '
        'cmbSamType
        '
        Me.cmbSamType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSamType.FormattingEnabled = True
        Me.cmbSamType.Location = New System.Drawing.Point(9, 39)
        Me.cmbSamType.Name = "cmbSamType"
        Me.cmbSamType.Size = New System.Drawing.Size(97, 21)
        Me.cmbSamType.TabIndex = 1
        '
        'gridSample
        '
        Me.gridSample.AllowUserToAddRows = False
        Me.gridSample.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridSample.Location = New System.Drawing.Point(9, 61)
        Me.gridSample.Name = "gridSample"
        Me.gridSample.ReadOnly = True
        Me.gridSample.Size = New System.Drawing.Size(987, 112)
        Me.gridSample.TabIndex = 20
        '
        'frmOrderEstimate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.ClientSize = New System.Drawing.Size(1028, 642)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.pnlContainer_OWN)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.Name = "frmOrderEstimate"
        Me.Text = "Order"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.grpOrderDetails.ResumeLayout(False)
        Me.grpOrderDetails.PerformLayout()
        CType(Me.gridOrderTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridOrder, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpHeader.ResumeLayout(False)
        Me.grpHeader.PerformLayout()
        Me.cmenuTemplate.ResumeLayout(False)
        Me.pnlOrderRate.ResumeLayout(False)
        Me.pnlOrderRate.PerformLayout()
        Me.pnlOrderType.ResumeLayout(False)
        Me.pnlOrderType.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.pnlContainer_OWN.ResumeLayout(False)
        Me.tabOtherOptions.ResumeLayout(False)
        Me.tabAddress.ResumeLayout(False)
        Me.grpAddress.ResumeLayout(False)
        Me.grpAddress.PerformLayout()
        Me.pnlFurtherAdvAddress.ResumeLayout(False)
        Me.pnlFurtherAdvAddress.PerformLayout()
        Me.Grouper2.ResumeLayout(False)
        Me.tabStone.ResumeLayout(False)
        Me.grpStone.ResumeLayout(False)
        Me.grpStone.PerformLayout()
        CType(Me.gridStoneTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridStone, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabHideDet.ResumeLayout(False)
        Me.grpImage.ResumeLayout(False)
        CType(Me.picImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlOrderExtraDet.ResumeLayout(False)
        Me.pnlOrderExtraDet.PerformLayout()
        Me.grpDescription.ResumeLayout(False)
        Me.grpDescription.PerformLayout()
        Me.tabSample.ResumeLayout(False)
        Me.grpSample.ResumeLayout(False)
        Me.grpSample.PerformLayout()
        CType(Me.gridSample, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtOParticular As System.Windows.Forms.TextBox
    Friend WithEvents txtOGrossAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtOOtherAmt_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtOVat_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtOMc_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtOPcs_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtOAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtOGrsWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtORate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtOWastage_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtONetWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents chkOSample As System.Windows.Forms.CheckBox
    Friend WithEvents chkOImage As System.Windows.Forms.CheckBox
    Friend WithEvents grpOrderDetails As CodeVendor.Controls.Grouper
    Friend WithEvents gridOrder As System.Windows.Forms.DataGridView
    Friend WithEvents grpHeader As CodeVendor.Controls.Grouper
    Friend WithEvents txtRemDays_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents txtSalesManName As System.Windows.Forms.TextBox
    Friend WithEvents txtSalesMan_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtDueDays_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents rbtDeliveryRate As System.Windows.Forms.RadioButton
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents rbtCurrentRate As System.Windows.Forms.RadioButton
    Friend WithEvents rbtCustomerOrderType As System.Windows.Forms.RadioButton
    Friend WithEvents Label41 As System.Windows.Forms.Label
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
    Friend WithEvents pnlOrderRate As System.Windows.Forms.Panel
    Friend WithEvents pnlOrderType As System.Windows.Forms.Panel
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents txtOrderRowIndex As System.Windows.Forms.TextBox
    Friend WithEvents txtOCommision_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label48 As System.Windows.Forms.Label
    Friend WithEvents AdvanceWeightToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AdvanceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChitCardToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChequeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CreditCardToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CashToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WastageMcToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents lblUserName As System.Windows.Forms.Label
    Public WithEvents lblBillDate As System.Windows.Forms.Label
    Public WithEvents lblCashCounter As System.Windows.Forms.Label
    Public WithEvents lblSystemId As System.Windows.Forms.Label
    Public WithEvents lblSilverRate As System.Windows.Forms.Label
    Public WithEvents lblGoldRate As System.Windows.Forms.Label
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlContainer_OWN As System.Windows.Forms.Panel
    Friend WithEvents txtOSize As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents dtpRemDate As BrighttechPack.DatePicker
    Friend WithEvents dtpDueDate As BrighttechPack.DatePicker
    Friend WithEvents DiscountToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents txtODiaPcsWt As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents dtpOrderDate As BrighttechPack.DatePicker
    Friend WithEvents lblOrderDate As System.Windows.Forms.Label
    Friend WithEvents EstimateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabOtherOptions As System.Windows.Forms.TabControl
    Friend WithEvents tabAddress As System.Windows.Forms.TabPage
    Friend WithEvents grpAddress As CodeVendor.Controls.Grouper
    Friend WithEvents lblnetamount As System.Windows.Forms.Label
    Friend WithEvents lblBalance As System.Windows.Forms.Label
    Friend WithEvents txtOItem As System.Windows.Forms.TextBox
    Friend WithEvents txtOStyleNo As System.Windows.Forms.TextBox
    Friend WithEvents pnlFurtherAdvAddress As System.Windows.Forms.Panel
    Friend WithEvents Label73 As System.Windows.Forms.Label
    Friend WithEvents Label66 As System.Windows.Forms.Label
    Friend WithEvents lblHelp As System.Windows.Forms.Label
    Friend WithEvents Grouper2 As CodeVendor.Controls.Grouper
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
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
    Friend WithEvents tabHideDet As System.Windows.Forms.TabPage
    Friend WithEvents grpImage As CodeVendor.Controls.Grouper
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents picImage As System.Windows.Forms.PictureBox
    Friend WithEvents pnlOrderExtraDet As System.Windows.Forms.Panel
    Friend WithEvents Label54 As System.Windows.Forms.Label
    Friend WithEvents txtOWastagePer_Per As System.Windows.Forms.TextBox
    Friend WithEvents txtOMcPerGrm_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtOCommGrm_AMT As System.Windows.Forms.TextBox
    Friend WithEvents grpDescription As CodeVendor.Controls.Grouper
    Friend WithEvents Label62 As System.Windows.Forms.Label
    Friend WithEvents txtODescription As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents txtOItemId As System.Windows.Forms.TextBox
    Friend WithEvents DuplicateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tabSample As System.Windows.Forms.TabPage
    Friend WithEvents grpSample As CodeVendor.Controls.Grouper
    Friend WithEvents Label50 As System.Windows.Forms.Label
    Friend WithEvents Label65 As System.Windows.Forms.Label
    Friend WithEvents Label64 As System.Windows.Forms.Label
    Friend WithEvents Label63 As System.Windows.Forms.Label
    Friend WithEvents Label53 As System.Windows.Forms.Label
    Friend WithEvents Label52 As System.Windows.Forms.Label
    Friend WithEvents Label51 As System.Windows.Forms.Label
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents txtSamNetWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtSamGrsWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtSamPcs_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtSamTagNo As System.Windows.Forms.TextBox
    Friend WithEvents txtSamDescription As System.Windows.Forms.TextBox
    Friend WithEvents txtSamItem As System.Windows.Forms.TextBox
    Friend WithEvents cmbSamFrom As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSamType As System.Windows.Forms.ComboBox
    Friend WithEvents gridSample As System.Windows.Forms.DataGridView
End Class
