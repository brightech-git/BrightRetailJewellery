<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAdvanceAdj
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
        Me.grpAdvance = New CodeVendor.Controls.Grouper
        Me.lblAdvGst = New System.Windows.Forms.Label
        Me.txtAdvanceGST_AMT = New System.Windows.Forms.TextBox
        Me.txtAdvCostid = New System.Windows.Forms.TextBox
        Me.lblAdvCostid = New System.Windows.Forms.Label
        Me.dtpAdvanceDate = New BrighttechPack.DatePicker(Me.components)
        Me.lblAdvBal = New System.Windows.Forms.Label
        Me.lblAdvAdj = New System.Windows.Forms.Label
        Me.lblAdvdate = New System.Windows.Forms.Label
        Me.lblAdvRecd = New System.Windows.Forms.Label
        Me.lblAdvNo = New System.Windows.Forms.Label
        Me.txtAdvanceBalance_AMT = New System.Windows.Forms.TextBox
        Me.txtAdvanceAdjusted_AMT = New System.Windows.Forms.TextBox
        Me.txtAdvanceReceived_AMT = New System.Windows.Forms.TextBox
        Me.txtAdvanceNo = New System.Windows.Forms.TextBox
        Me.gridAdvance = New System.Windows.Forms.DataGridView
        Me.gridAdvanceTotal = New System.Windows.Forms.DataGridView
        Me.PnlAddr = New System.Windows.Forms.Panel
        Me.txtAdvanceName = New System.Windows.Forms.TextBox
        Me.Label64 = New System.Windows.Forms.Label
        Me.txtAdvanceAddress2 = New System.Windows.Forms.TextBox
        Me.txtAdvanceAddress1 = New System.Windows.Forms.TextBox
        Me.txtRemark = New System.Windows.Forms.TextBox
        Me.txtAdvanceAddress3 = New System.Windows.Forms.TextBox
        Me.txtAdvanceRefNo = New System.Windows.Forms.TextBox
        Me.txtAdvanceRowIndex = New System.Windows.Forms.TextBox
        Me.txtAdvanceAcCode = New System.Windows.Forms.TextBox
        Me.txtAdvanceEntAmount = New System.Windows.Forms.TextBox
        Me.txtAdvanceEntAdvanceNo = New System.Windows.Forms.TextBox
        Me.grpAddress = New CodeVendor.Controls.Grouper
        Me.txtAddressRegularSno = New System.Windows.Forms.TextBox
        Me.dtpAddressDueDate = New System.Windows.Forms.DateTimePicker
        Me.txtAddressInitial = New System.Windows.Forms.TextBox
        Me.cmbAddressTitle_OWN = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label122 = New System.Windows.Forms.Label
        Me.txtAddressFax = New System.Windows.Forms.TextBox
        Me.txtAddressPincode_NUM = New System.Windows.Forms.TextBox
        Me.txtAddressMobile = New System.Windows.Forms.TextBox
        Me.Label110 = New System.Windows.Forms.Label
        Me.txtAddressEmailId_OWN = New System.Windows.Forms.TextBox
        Me.Label111 = New System.Windows.Forms.Label
        Me.txtAddressPhoneRes = New System.Windows.Forms.TextBox
        Me.Label112 = New System.Windows.Forms.Label
        Me.txtAddressPrevilegeId = New System.Windows.Forms.TextBox
        Me.cmbAddressCountry_OWN = New System.Windows.Forms.ComboBox
        Me.txtAddressPartyCode = New System.Windows.Forms.TextBox
        Me.cmbAddressState_OWN = New System.Windows.Forms.ComboBox
        Me.Label113 = New System.Windows.Forms.Label
        Me.Label117 = New System.Windows.Forms.Label
        Me.Label114 = New System.Windows.Forms.Label
        Me.Label116 = New System.Windows.Forms.Label
        Me.lblAddressDueDate = New System.Windows.Forms.Label
        Me.Label121 = New System.Windows.Forms.Label
        Me.Label115 = New System.Windows.Forms.Label
        Me.Label119 = New System.Windows.Forms.Label
        Me.txtAddress3 = New System.Windows.Forms.TextBox
        Me.Label120 = New System.Windows.Forms.Label
        Me.cmbAddressArea_OWN = New System.Windows.Forms.ComboBox
        Me.Label118 = New System.Windows.Forms.Label
        Me.txtAddress1 = New System.Windows.Forms.TextBox
        Me.cmbAddressCity_OWN = New System.Windows.Forms.ComboBox
        Me.txtAddress2 = New System.Windows.Forms.TextBox
        Me.txtAddressName = New System.Windows.Forms.TextBox
        Me.txtAddressDoorNo = New System.Windows.Forms.TextBox
        Me.txtAdvanceCompanyId = New System.Windows.Forms.TextBox
        Me.lblAdvAmt = New System.Windows.Forms.Label
        Me.txtAdvanceAmt_AMT = New System.Windows.Forms.TextBox
        Me.grpAdvance.SuspendLayout()
        CType(Me.gridAdvance, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridAdvanceTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlAddr.SuspendLayout()
        Me.grpAddress.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpAdvance
        '
        Me.grpAdvance.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpAdvance.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpAdvance.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpAdvance.BorderColor = System.Drawing.Color.Transparent
        Me.grpAdvance.BorderThickness = 1.0!
        Me.grpAdvance.Controls.Add(Me.lblAdvAmt)
        Me.grpAdvance.Controls.Add(Me.txtAdvanceAmt_AMT)
        Me.grpAdvance.Controls.Add(Me.lblAdvGst)
        Me.grpAdvance.Controls.Add(Me.txtAdvanceGST_AMT)
        Me.grpAdvance.Controls.Add(Me.txtAdvCostid)
        Me.grpAdvance.Controls.Add(Me.lblAdvCostid)
        Me.grpAdvance.Controls.Add(Me.dtpAdvanceDate)
        Me.grpAdvance.Controls.Add(Me.lblAdvBal)
        Me.grpAdvance.Controls.Add(Me.lblAdvAdj)
        Me.grpAdvance.Controls.Add(Me.lblAdvdate)
        Me.grpAdvance.Controls.Add(Me.lblAdvRecd)
        Me.grpAdvance.Controls.Add(Me.lblAdvNo)
        Me.grpAdvance.Controls.Add(Me.txtAdvanceBalance_AMT)
        Me.grpAdvance.Controls.Add(Me.txtAdvanceAdjusted_AMT)
        Me.grpAdvance.Controls.Add(Me.txtAdvanceReceived_AMT)
        Me.grpAdvance.Controls.Add(Me.txtAdvanceNo)
        Me.grpAdvance.Controls.Add(Me.gridAdvance)
        Me.grpAdvance.Controls.Add(Me.gridAdvanceTotal)
        Me.grpAdvance.Controls.Add(Me.PnlAddr)
        Me.grpAdvance.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpAdvance.GroupImage = Nothing
        Me.grpAdvance.GroupTitle = ""
        Me.grpAdvance.Location = New System.Drawing.Point(5, -2)
        Me.grpAdvance.Name = "grpAdvance"
        Me.grpAdvance.Padding = New System.Windows.Forms.Padding(20)
        Me.grpAdvance.PaintGroupBox = False
        Me.grpAdvance.RoundCorners = 10
        Me.grpAdvance.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpAdvance.ShadowControl = False
        Me.grpAdvance.ShadowThickness = 3
        Me.grpAdvance.Size = New System.Drawing.Size(840, 192)
        Me.grpAdvance.TabIndex = 0
        '
        'lblAdvGst
        '
        Me.lblAdvGst.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAdvGst.Location = New System.Drawing.Point(558, 21)
        Me.lblAdvGst.Name = "lblAdvGst"
        Me.lblAdvGst.Size = New System.Drawing.Size(60, 15)
        Me.lblAdvGst.TabIndex = 14
        Me.lblAdvGst.Text = "GST"
        Me.lblAdvGst.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtAdvanceGST_AMT
        '
        Me.txtAdvanceGST_AMT.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAdvanceGST_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdvanceGST_AMT.Location = New System.Drawing.Point(555, 38)
        Me.txtAdvanceGST_AMT.MaxLength = 12
        Me.txtAdvanceGST_AMT.Name = "txtAdvanceGST_AMT"
        Me.txtAdvanceGST_AMT.ReadOnly = True
        Me.txtAdvanceGST_AMT.Size = New System.Drawing.Size(66, 22)
        Me.txtAdvanceGST_AMT.TabIndex = 15
        '
        'txtAdvCostid
        '
        Me.txtAdvCostid.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAdvCostid.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdvCostid.Location = New System.Drawing.Point(7, 38)
        Me.txtAdvCostid.MaxLength = 10
        Me.txtAdvCostid.Name = "txtAdvCostid"
        Me.txtAdvCostid.Size = New System.Drawing.Size(36, 22)
        Me.txtAdvCostid.TabIndex = 1
        Me.txtAdvCostid.Text = "AAA"
        '
        'lblAdvCostid
        '
        Me.lblAdvCostid.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAdvCostid.Location = New System.Drawing.Point(-3, 21)
        Me.lblAdvCostid.Name = "lblAdvCostid"
        Me.lblAdvCostid.Size = New System.Drawing.Size(52, 14)
        Me.lblAdvCostid.TabIndex = 0
        Me.lblAdvCostid.Text = "Costid"
        Me.lblAdvCostid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dtpAdvanceDate
        '
        Me.dtpAdvanceDate.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.dtpAdvanceDate.Location = New System.Drawing.Point(130, 38)
        Me.dtpAdvanceDate.Mask = "##/##/####"
        Me.dtpAdvanceDate.MaximumDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpAdvanceDate.MinimumDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpAdvanceDate.Name = "dtpAdvanceDate"
        Me.dtpAdvanceDate.Seperator = Global.Microsoft.VisualBasic.ChrW(47)
        Me.dtpAdvanceDate.Size = New System.Drawing.Size(96, 22)
        Me.dtpAdvanceDate.TabIndex = 5
        Me.dtpAdvanceDate.Text = "07/03/9998"
        Me.dtpAdvanceDate.Value = New Date(9998, 3, 7, 0, 0, 0, 0)
        '
        'lblAdvBal
        '
        Me.lblAdvBal.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAdvBal.Location = New System.Drawing.Point(393, 21)
        Me.lblAdvBal.Name = "lblAdvBal"
        Me.lblAdvBal.Size = New System.Drawing.Size(76, 15)
        Me.lblAdvBal.TabIndex = 10
        Me.lblAdvBal.Text = "Balance"
        Me.lblAdvBal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblAdvAdj
        '
        Me.lblAdvAdj.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAdvAdj.Location = New System.Drawing.Point(315, 21)
        Me.lblAdvAdj.Name = "lblAdvAdj"
        Me.lblAdvAdj.Size = New System.Drawing.Size(76, 15)
        Me.lblAdvAdj.TabIndex = 8
        Me.lblAdvAdj.Text = "Adjusted"
        Me.lblAdvAdj.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblAdvdate
        '
        Me.lblAdvdate.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAdvdate.Location = New System.Drawing.Point(137, 20)
        Me.lblAdvdate.Name = "lblAdvdate"
        Me.lblAdvdate.Size = New System.Drawing.Size(85, 16)
        Me.lblAdvdate.TabIndex = 4
        Me.lblAdvdate.Text = "Adv Date"
        Me.lblAdvdate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblAdvRecd
        '
        Me.lblAdvRecd.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAdvRecd.Location = New System.Drawing.Point(238, 20)
        Me.lblAdvRecd.Name = "lblAdvRecd"
        Me.lblAdvRecd.Size = New System.Drawing.Size(76, 15)
        Me.lblAdvRecd.TabIndex = 6
        Me.lblAdvRecd.Text = "Received"
        Me.lblAdvRecd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblAdvNo
        '
        Me.lblAdvNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAdvNo.Location = New System.Drawing.Point(46, 21)
        Me.lblAdvNo.Name = "lblAdvNo"
        Me.lblAdvNo.Size = New System.Drawing.Size(85, 15)
        Me.lblAdvNo.TabIndex = 2
        Me.lblAdvNo.Text = "Advance No"
        Me.lblAdvNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtAdvanceBalance_AMT
        '
        Me.txtAdvanceBalance_AMT.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAdvanceBalance_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdvanceBalance_AMT.Location = New System.Drawing.Point(390, 38)
        Me.txtAdvanceBalance_AMT.MaxLength = 12
        Me.txtAdvanceBalance_AMT.Name = "txtAdvanceBalance_AMT"
        Me.txtAdvanceBalance_AMT.Size = New System.Drawing.Size(81, 22)
        Me.txtAdvanceBalance_AMT.TabIndex = 11
        '
        'txtAdvanceAdjusted_AMT
        '
        Me.txtAdvanceAdjusted_AMT.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAdvanceAdjusted_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdvanceAdjusted_AMT.Location = New System.Drawing.Point(313, 38)
        Me.txtAdvanceAdjusted_AMT.Name = "txtAdvanceAdjusted_AMT"
        Me.txtAdvanceAdjusted_AMT.Size = New System.Drawing.Size(76, 22)
        Me.txtAdvanceAdjusted_AMT.TabIndex = 9
        '
        'txtAdvanceReceived_AMT
        '
        Me.txtAdvanceReceived_AMT.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAdvanceReceived_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdvanceReceived_AMT.Location = New System.Drawing.Point(227, 38)
        Me.txtAdvanceReceived_AMT.Name = "txtAdvanceReceived_AMT"
        Me.txtAdvanceReceived_AMT.Size = New System.Drawing.Size(85, 22)
        Me.txtAdvanceReceived_AMT.TabIndex = 7
        Me.txtAdvanceReceived_AMT.Text = "1000000.00"
        '
        'txtAdvanceNo
        '
        Me.txtAdvanceNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAdvanceNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdvanceNo.Location = New System.Drawing.Point(44, 38)
        Me.txtAdvanceNo.MaxLength = 10
        Me.txtAdvanceNo.Name = "txtAdvanceNo"
        Me.txtAdvanceNo.Size = New System.Drawing.Size(85, 22)
        Me.txtAdvanceNo.TabIndex = 3
        Me.txtAdvanceNo.Text = "A12456789"
        '
        'gridAdvance
        '
        Me.gridAdvance.AllowUserToAddRows = False
        Me.gridAdvance.AllowUserToDeleteRows = False
        Me.gridAdvance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridAdvance.Location = New System.Drawing.Point(7, 60)
        Me.gridAdvance.Name = "gridAdvance"
        Me.gridAdvance.ReadOnly = True
        Me.gridAdvance.RowHeadersVisible = False
        Me.gridAdvance.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridAdvance.Size = New System.Drawing.Size(618, 108)
        Me.gridAdvance.TabIndex = 16
        '
        'gridAdvanceTotal
        '
        Me.gridAdvanceTotal.AllowUserToAddRows = False
        Me.gridAdvanceTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridAdvanceTotal.Enabled = False
        Me.gridAdvanceTotal.Location = New System.Drawing.Point(7, 168)
        Me.gridAdvanceTotal.Name = "gridAdvanceTotal"
        Me.gridAdvanceTotal.ReadOnly = True
        Me.gridAdvanceTotal.Size = New System.Drawing.Size(618, 19)
        Me.gridAdvanceTotal.TabIndex = 17
        '
        'PnlAddr
        '
        Me.PnlAddr.Controls.Add(Me.txtAdvanceName)
        Me.PnlAddr.Controls.Add(Me.Label64)
        Me.PnlAddr.Controls.Add(Me.txtAdvanceAddress2)
        Me.PnlAddr.Controls.Add(Me.txtAdvanceAddress1)
        Me.PnlAddr.Controls.Add(Me.txtRemark)
        Me.PnlAddr.Controls.Add(Me.txtAdvanceAddress3)
        Me.PnlAddr.Controls.Add(Me.txtAdvanceRefNo)
        Me.PnlAddr.Controls.Add(Me.txtAdvanceRowIndex)
        Me.PnlAddr.Controls.Add(Me.txtAdvanceAcCode)
        Me.PnlAddr.Controls.Add(Me.txtAdvanceEntAmount)
        Me.PnlAddr.Controls.Add(Me.txtAdvanceEntAdvanceNo)
        Me.PnlAddr.Location = New System.Drawing.Point(627, 15)
        Me.PnlAddr.Name = "PnlAddr"
        Me.PnlAddr.Size = New System.Drawing.Size(208, 175)
        Me.PnlAddr.TabIndex = 18
        '
        'txtAdvanceName
        '
        Me.txtAdvanceName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAdvanceName.Enabled = False
        Me.txtAdvanceName.Location = New System.Drawing.Point(4, 73)
        Me.txtAdvanceName.Name = "txtAdvanceName"
        Me.txtAdvanceName.Size = New System.Drawing.Size(198, 21)
        Me.txtAdvanceName.TabIndex = 17
        '
        'Label64
        '
        Me.Label64.Location = New System.Drawing.Point(7, 50)
        Me.Label64.Name = "Label64"
        Me.Label64.Size = New System.Drawing.Size(195, 15)
        Me.Label64.TabIndex = 16
        Me.Label64.Text = "Address"
        Me.Label64.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtAdvanceAddress2
        '
        Me.txtAdvanceAddress2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAdvanceAddress2.Enabled = False
        Me.txtAdvanceAddress2.Location = New System.Drawing.Point(4, 113)
        Me.txtAdvanceAddress2.Name = "txtAdvanceAddress2"
        Me.txtAdvanceAddress2.Size = New System.Drawing.Size(198, 21)
        Me.txtAdvanceAddress2.TabIndex = 19
        '
        'txtAdvanceAddress1
        '
        Me.txtAdvanceAddress1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAdvanceAddress1.Enabled = False
        Me.txtAdvanceAddress1.Location = New System.Drawing.Point(4, 93)
        Me.txtAdvanceAddress1.Name = "txtAdvanceAddress1"
        Me.txtAdvanceAddress1.Size = New System.Drawing.Size(198, 21)
        Me.txtAdvanceAddress1.TabIndex = 18
        '
        'txtRemark
        '
        Me.txtRemark.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtRemark.Enabled = False
        Me.txtRemark.Location = New System.Drawing.Point(4, 155)
        Me.txtRemark.Name = "txtRemark"
        Me.txtRemark.Size = New System.Drawing.Size(198, 21)
        Me.txtRemark.TabIndex = 21
        '
        'txtAdvanceAddress3
        '
        Me.txtAdvanceAddress3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAdvanceAddress3.Enabled = False
        Me.txtAdvanceAddress3.Location = New System.Drawing.Point(4, 133)
        Me.txtAdvanceAddress3.Name = "txtAdvanceAddress3"
        Me.txtAdvanceAddress3.Size = New System.Drawing.Size(198, 21)
        Me.txtAdvanceAddress3.TabIndex = 20
        '
        'txtAdvanceRefNo
        '
        Me.txtAdvanceRefNo.Location = New System.Drawing.Point(98, 1)
        Me.txtAdvanceRefNo.Name = "txtAdvanceRefNo"
        Me.txtAdvanceRefNo.Size = New System.Drawing.Size(53, 21)
        Me.txtAdvanceRefNo.TabIndex = 14
        Me.txtAdvanceRefNo.Visible = False
        '
        'txtAdvanceRowIndex
        '
        Me.txtAdvanceRowIndex.Location = New System.Drawing.Point(59, 8)
        Me.txtAdvanceRowIndex.Name = "txtAdvanceRowIndex"
        Me.txtAdvanceRowIndex.Size = New System.Drawing.Size(33, 21)
        Me.txtAdvanceRowIndex.TabIndex = 22
        Me.txtAdvanceRowIndex.Visible = False
        '
        'txtAdvanceAcCode
        '
        Me.txtAdvanceAcCode.Location = New System.Drawing.Point(157, 1)
        Me.txtAdvanceAcCode.Name = "txtAdvanceAcCode"
        Me.txtAdvanceAcCode.Size = New System.Drawing.Size(53, 21)
        Me.txtAdvanceAcCode.TabIndex = 15
        Me.txtAdvanceAcCode.Visible = False
        '
        'txtAdvanceEntAmount
        '
        Me.txtAdvanceEntAmount.Location = New System.Drawing.Point(157, 28)
        Me.txtAdvanceEntAmount.Name = "txtAdvanceEntAmount"
        Me.txtAdvanceEntAmount.Size = New System.Drawing.Size(53, 21)
        Me.txtAdvanceEntAmount.TabIndex = 24
        Me.txtAdvanceEntAmount.Visible = False
        '
        'txtAdvanceEntAdvanceNo
        '
        Me.txtAdvanceEntAdvanceNo.Location = New System.Drawing.Point(98, 28)
        Me.txtAdvanceEntAdvanceNo.Name = "txtAdvanceEntAdvanceNo"
        Me.txtAdvanceEntAdvanceNo.Size = New System.Drawing.Size(53, 21)
        Me.txtAdvanceEntAdvanceNo.TabIndex = 23
        Me.txtAdvanceEntAdvanceNo.Visible = False
        '
        'grpAddress
        '
        Me.grpAddress.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpAddress.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpAddress.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpAddress.BorderColor = System.Drawing.Color.Transparent
        Me.grpAddress.BorderThickness = 1.0!
        Me.grpAddress.Controls.Add(Me.txtAddressRegularSno)
        Me.grpAddress.Controls.Add(Me.dtpAddressDueDate)
        Me.grpAddress.Controls.Add(Me.txtAddressInitial)
        Me.grpAddress.Controls.Add(Me.cmbAddressTitle_OWN)
        Me.grpAddress.Controls.Add(Me.Label1)
        Me.grpAddress.Controls.Add(Me.Label122)
        Me.grpAddress.Controls.Add(Me.txtAddressFax)
        Me.grpAddress.Controls.Add(Me.txtAddressPincode_NUM)
        Me.grpAddress.Controls.Add(Me.txtAddressMobile)
        Me.grpAddress.Controls.Add(Me.Label110)
        Me.grpAddress.Controls.Add(Me.txtAddressEmailId_OWN)
        Me.grpAddress.Controls.Add(Me.Label111)
        Me.grpAddress.Controls.Add(Me.txtAddressPhoneRes)
        Me.grpAddress.Controls.Add(Me.Label112)
        Me.grpAddress.Controls.Add(Me.txtAddressPrevilegeId)
        Me.grpAddress.Controls.Add(Me.cmbAddressCountry_OWN)
        Me.grpAddress.Controls.Add(Me.txtAddressPartyCode)
        Me.grpAddress.Controls.Add(Me.cmbAddressState_OWN)
        Me.grpAddress.Controls.Add(Me.Label113)
        Me.grpAddress.Controls.Add(Me.Label117)
        Me.grpAddress.Controls.Add(Me.Label114)
        Me.grpAddress.Controls.Add(Me.Label116)
        Me.grpAddress.Controls.Add(Me.lblAddressDueDate)
        Me.grpAddress.Controls.Add(Me.Label121)
        Me.grpAddress.Controls.Add(Me.Label115)
        Me.grpAddress.Controls.Add(Me.Label119)
        Me.grpAddress.Controls.Add(Me.txtAddress3)
        Me.grpAddress.Controls.Add(Me.Label120)
        Me.grpAddress.Controls.Add(Me.cmbAddressArea_OWN)
        Me.grpAddress.Controls.Add(Me.Label118)
        Me.grpAddress.Controls.Add(Me.txtAddress1)
        Me.grpAddress.Controls.Add(Me.cmbAddressCity_OWN)
        Me.grpAddress.Controls.Add(Me.txtAddress2)
        Me.grpAddress.Controls.Add(Me.txtAddressName)
        Me.grpAddress.Controls.Add(Me.txtAddressDoorNo)
        Me.grpAddress.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpAddress.GroupImage = Nothing
        Me.grpAddress.GroupTitle = ""
        Me.grpAddress.Location = New System.Drawing.Point(15, 196)
        Me.grpAddress.Name = "grpAddress"
        Me.grpAddress.Padding = New System.Windows.Forms.Padding(20)
        Me.grpAddress.PaintGroupBox = False
        Me.grpAddress.RoundCorners = 10
        Me.grpAddress.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpAddress.ShadowControl = False
        Me.grpAddress.ShadowThickness = 3
        Me.grpAddress.Size = New System.Drawing.Size(390, 445)
        Me.grpAddress.TabIndex = 2
        '
        'txtAddressRegularSno
        '
        Me.txtAddressRegularSno.Location = New System.Drawing.Point(191, 420)
        Me.txtAddressRegularSno.Name = "txtAddressRegularSno"
        Me.txtAddressRegularSno.Size = New System.Drawing.Size(39, 21)
        Me.txtAddressRegularSno.TabIndex = 36
        Me.txtAddressRegularSno.Visible = False
        '
        'dtpAddressDueDate
        '
        Me.dtpAddressDueDate.CustomFormat = "dd/MM/yyyy"
        Me.dtpAddressDueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpAddressDueDate.Location = New System.Drawing.Point(83, 417)
        Me.dtpAddressDueDate.Name = "dtpAddressDueDate"
        Me.dtpAddressDueDate.Size = New System.Drawing.Size(91, 21)
        Me.dtpAddressDueDate.TabIndex = 35
        '
        'txtAddressInitial
        '
        Me.txtAddressInitial.Location = New System.Drawing.Point(157, 87)
        Me.txtAddressInitial.MaxLength = 10
        Me.txtAddressInitial.Name = "txtAddressInitial"
        Me.txtAddressInitial.Size = New System.Drawing.Size(73, 21)
        Me.txtAddressInitial.TabIndex = 8
        Me.txtAddressInitial.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cmbAddressTitle_OWN
        '
        Me.cmbAddressTitle_OWN.FormattingEnabled = True
        Me.cmbAddressTitle_OWN.Items.AddRange(New Object() {"Mr", "Mrs", "Thiru", "Sri"})
        Me.cmbAddressTitle_OWN.Location = New System.Drawing.Point(83, 87)
        Me.cmbAddressTitle_OWN.MaxLength = 10
        Me.cmbAddressTitle_OWN.Name = "cmbAddressTitle_OWN"
        Me.cmbAddressTitle_OWN.Size = New System.Drawing.Size(73, 21)
        Me.cmbAddressTitle_OWN.TabIndex = 7
        Me.cmbAddressTitle_OWN.Text = "Mr"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 47)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Previlege Id"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label122
        '
        Me.Label122.AutoSize = True
        Me.Label122.Location = New System.Drawing.Point(6, 68)
        Me.Label122.Name = "Label122"
        Me.Label122.Size = New System.Drawing.Size(71, 13)
        Me.Label122.TabIndex = 4
        Me.Label122.Text = "Party Code"
        Me.Label122.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtAddressFax
        '
        Me.txtAddressFax.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAddressFax.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddressFax.Location = New System.Drawing.Point(83, 395)
        Me.txtAddressFax.MaxLength = 30
        Me.txtAddressFax.Name = "txtAddressFax"
        Me.txtAddressFax.Size = New System.Drawing.Size(301, 21)
        Me.txtAddressFax.TabIndex = 33
        '
        'txtAddressPincode_NUM
        '
        Me.txtAddressPincode_NUM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAddressPincode_NUM.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddressPincode_NUM.Location = New System.Drawing.Point(83, 263)
        Me.txtAddressPincode_NUM.MaxLength = 7
        Me.txtAddressPincode_NUM.Name = "txtAddressPincode_NUM"
        Me.txtAddressPincode_NUM.Size = New System.Drawing.Size(97, 21)
        Me.txtAddressPincode_NUM.TabIndex = 21
        Me.txtAddressPincode_NUM.Text = "625533"
        '
        'txtAddressMobile
        '
        Me.txtAddressMobile.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAddressMobile.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddressMobile.Location = New System.Drawing.Point(83, 351)
        Me.txtAddressMobile.MaxLength = 25
        Me.txtAddressMobile.Name = "txtAddressMobile"
        Me.txtAddressMobile.Size = New System.Drawing.Size(301, 21)
        Me.txtAddressMobile.TabIndex = 29
        '
        'Label110
        '
        Me.Label110.AutoSize = True
        Me.Label110.Location = New System.Drawing.Point(6, 90)
        Me.Label110.Name = "Label110"
        Me.Label110.Size = New System.Drawing.Size(40, 13)
        Me.Label110.TabIndex = 6
        Me.Label110.Text = "Name"
        Me.Label110.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtAddressEmailId_OWN
        '
        Me.txtAddressEmailId_OWN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddressEmailId_OWN.Location = New System.Drawing.Point(83, 373)
        Me.txtAddressEmailId_OWN.MaxLength = 75
        Me.txtAddressEmailId_OWN.Name = "txtAddressEmailId_OWN"
        Me.txtAddressEmailId_OWN.Size = New System.Drawing.Size(301, 21)
        Me.txtAddressEmailId_OWN.TabIndex = 31
        Me.txtAddressEmailId_OWN.Text = "safiyullah.mhd@gmail.com"
        '
        'Label111
        '
        Me.Label111.AutoSize = True
        Me.Label111.Location = New System.Drawing.Point(6, 133)
        Me.Label111.Name = "Label111"
        Me.Label111.Size = New System.Drawing.Size(54, 13)
        Me.Label111.TabIndex = 10
        Me.Label111.Text = "Door No"
        Me.Label111.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtAddressPhoneRes
        '
        Me.txtAddressPhoneRes.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAddressPhoneRes.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddressPhoneRes.Location = New System.Drawing.Point(83, 329)
        Me.txtAddressPhoneRes.MaxLength = 25
        Me.txtAddressPhoneRes.Name = "txtAddressPhoneRes"
        Me.txtAddressPhoneRes.Size = New System.Drawing.Size(301, 21)
        Me.txtAddressPhoneRes.TabIndex = 27
        '
        'Label112
        '
        Me.Label112.AutoSize = True
        Me.Label112.Location = New System.Drawing.Point(6, 155)
        Me.Label112.Name = "Label112"
        Me.Label112.Size = New System.Drawing.Size(53, 13)
        Me.Label112.TabIndex = 12
        Me.Label112.Text = "Address"
        Me.Label112.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtAddressPrevilegeId
        '
        Me.txtAddressPrevilegeId.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAddressPrevilegeId.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddressPrevilegeId.Location = New System.Drawing.Point(83, 43)
        Me.txtAddressPrevilegeId.MaxLength = 7
        Me.txtAddressPrevilegeId.Name = "txtAddressPrevilegeId"
        Me.txtAddressPrevilegeId.Size = New System.Drawing.Size(97, 21)
        Me.txtAddressPrevilegeId.TabIndex = 3
        '
        'cmbAddressCountry_OWN
        '
        Me.cmbAddressCountry_OWN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbAddressCountry_OWN.FormattingEnabled = True
        Me.cmbAddressCountry_OWN.Location = New System.Drawing.Point(83, 307)
        Me.cmbAddressCountry_OWN.MaxLength = 30
        Me.cmbAddressCountry_OWN.Name = "cmbAddressCountry_OWN"
        Me.cmbAddressCountry_OWN.Size = New System.Drawing.Size(301, 21)
        Me.cmbAddressCountry_OWN.TabIndex = 25
        '
        'txtAddressPartyCode
        '
        Me.txtAddressPartyCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAddressPartyCode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddressPartyCode.Location = New System.Drawing.Point(83, 65)
        Me.txtAddressPartyCode.MaxLength = 7
        Me.txtAddressPartyCode.Name = "txtAddressPartyCode"
        Me.txtAddressPartyCode.Size = New System.Drawing.Size(97, 21)
        Me.txtAddressPartyCode.TabIndex = 5
        '
        'cmbAddressState_OWN
        '
        Me.cmbAddressState_OWN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbAddressState_OWN.FormattingEnabled = True
        Me.cmbAddressState_OWN.Location = New System.Drawing.Point(83, 285)
        Me.cmbAddressState_OWN.MaxLength = 30
        Me.cmbAddressState_OWN.Name = "cmbAddressState_OWN"
        Me.cmbAddressState_OWN.Size = New System.Drawing.Size(301, 21)
        Me.cmbAddressState_OWN.TabIndex = 23
        '
        'Label113
        '
        Me.Label113.AutoSize = True
        Me.Label113.Location = New System.Drawing.Point(6, 221)
        Me.Label113.Name = "Label113"
        Me.Label113.Size = New System.Drawing.Size(34, 13)
        Me.Label113.TabIndex = 16
        Me.Label113.Text = "Area"
        Me.Label113.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label117
        '
        Me.Label117.AutoSize = True
        Me.Label117.Location = New System.Drawing.Point(6, 310)
        Me.Label117.Name = "Label117"
        Me.Label117.Size = New System.Drawing.Size(53, 13)
        Me.Label117.TabIndex = 24
        Me.Label117.Text = "Country"
        Me.Label117.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label114
        '
        Me.Label114.AutoSize = True
        Me.Label114.Location = New System.Drawing.Point(6, 241)
        Me.Label114.Name = "Label114"
        Me.Label114.Size = New System.Drawing.Size(30, 13)
        Me.Label114.TabIndex = 18
        Me.Label114.Text = "City"
        Me.Label114.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label116
        '
        Me.Label116.AutoSize = True
        Me.Label116.Location = New System.Drawing.Point(6, 288)
        Me.Label116.Name = "Label116"
        Me.Label116.Size = New System.Drawing.Size(37, 13)
        Me.Label116.TabIndex = 22
        Me.Label116.Text = "State"
        Me.Label116.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblAddressDueDate
        '
        Me.lblAddressDueDate.AutoSize = True
        Me.lblAddressDueDate.Location = New System.Drawing.Point(6, 420)
        Me.lblAddressDueDate.Name = "lblAddressDueDate"
        Me.lblAddressDueDate.Size = New System.Drawing.Size(61, 13)
        Me.lblAddressDueDate.TabIndex = 34
        Me.lblAddressDueDate.Text = "Due Date"
        Me.lblAddressDueDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label121
        '
        Me.Label121.AutoSize = True
        Me.Label121.Location = New System.Drawing.Point(6, 398)
        Me.Label121.Name = "Label121"
        Me.Label121.Size = New System.Drawing.Size(27, 13)
        Me.Label121.TabIndex = 32
        Me.Label121.Text = "Fax"
        Me.Label121.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label115
        '
        Me.Label115.AutoSize = True
        Me.Label115.Location = New System.Drawing.Point(6, 267)
        Me.Label115.Name = "Label115"
        Me.Label115.Size = New System.Drawing.Size(51, 13)
        Me.Label115.TabIndex = 20
        Me.Label115.Text = "Pincode"
        Me.Label115.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label119
        '
        Me.Label119.AutoSize = True
        Me.Label119.Location = New System.Drawing.Point(6, 354)
        Me.Label119.Name = "Label119"
        Me.Label119.Size = New System.Drawing.Size(43, 13)
        Me.Label119.TabIndex = 28
        Me.Label119.Text = "Mobile"
        Me.Label119.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtAddress3
        '
        Me.txtAddress3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAddress3.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress3.Location = New System.Drawing.Point(83, 197)
        Me.txtAddress3.MaxLength = 40
        Me.txtAddress3.Name = "txtAddress3"
        Me.txtAddress3.Size = New System.Drawing.Size(301, 21)
        Me.txtAddress3.TabIndex = 15
        '
        'Label120
        '
        Me.Label120.AutoSize = True
        Me.Label120.Location = New System.Drawing.Point(6, 376)
        Me.Label120.Name = "Label120"
        Me.Label120.Size = New System.Drawing.Size(38, 13)
        Me.Label120.TabIndex = 30
        Me.Label120.Text = "Email"
        Me.Label120.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmbAddressArea_OWN
        '
        Me.cmbAddressArea_OWN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbAddressArea_OWN.FormattingEnabled = True
        Me.cmbAddressArea_OWN.Items.AddRange(New Object() {"abcdedf", "sdakdsk", "dsfsdsfkl"})
        Me.cmbAddressArea_OWN.Location = New System.Drawing.Point(83, 219)
        Me.cmbAddressArea_OWN.MaxLength = 40
        Me.cmbAddressArea_OWN.Name = "cmbAddressArea_OWN"
        Me.cmbAddressArea_OWN.Size = New System.Drawing.Size(301, 21)
        Me.cmbAddressArea_OWN.TabIndex = 17
        '
        'Label118
        '
        Me.Label118.AutoSize = True
        Me.Label118.Location = New System.Drawing.Point(6, 332)
        Me.Label118.Name = "Label118"
        Me.Label118.Size = New System.Drawing.Size(67, 13)
        Me.Label118.TabIndex = 26
        Me.Label118.Text = "Phone Res"
        Me.Label118.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtAddress1
        '
        Me.txtAddress1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAddress1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress1.Location = New System.Drawing.Point(83, 153)
        Me.txtAddress1.MaxLength = 40
        Me.txtAddress1.Name = "txtAddress1"
        Me.txtAddress1.Size = New System.Drawing.Size(301, 21)
        Me.txtAddress1.TabIndex = 13
        '
        'cmbAddressCity_OWN
        '
        Me.cmbAddressCity_OWN.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbAddressCity_OWN.FormattingEnabled = True
        Me.cmbAddressCity_OWN.Location = New System.Drawing.Point(83, 241)
        Me.cmbAddressCity_OWN.MaxLength = 30
        Me.cmbAddressCity_OWN.Name = "cmbAddressCity_OWN"
        Me.cmbAddressCity_OWN.Size = New System.Drawing.Size(301, 21)
        Me.cmbAddressCity_OWN.TabIndex = 19
        '
        'txtAddress2
        '
        Me.txtAddress2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAddress2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress2.Location = New System.Drawing.Point(83, 175)
        Me.txtAddress2.MaxLength = 40
        Me.txtAddress2.Name = "txtAddress2"
        Me.txtAddress2.Size = New System.Drawing.Size(301, 21)
        Me.txtAddress2.TabIndex = 14
        '
        'txtAddressName
        '
        Me.txtAddressName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAddressName.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddressName.Location = New System.Drawing.Point(83, 109)
        Me.txtAddressName.MaxLength = 50
        Me.txtAddressName.Name = "txtAddressName"
        Me.txtAddressName.Size = New System.Drawing.Size(301, 21)
        Me.txtAddressName.TabIndex = 9
        Me.txtAddressName.Text = "12345678901234567890123456789012345678901234567890"
        '
        'txtAddressDoorNo
        '
        Me.txtAddressDoorNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAddressDoorNo.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddressDoorNo.Location = New System.Drawing.Point(83, 131)
        Me.txtAddressDoorNo.MaxLength = 50
        Me.txtAddressDoorNo.Name = "txtAddressDoorNo"
        Me.txtAddressDoorNo.Size = New System.Drawing.Size(301, 21)
        Me.txtAddressDoorNo.TabIndex = 11
        '
        'txtAdvanceCompanyId
        '
        Me.txtAdvanceCompanyId.Location = New System.Drawing.Point(448, 250)
        Me.txtAdvanceCompanyId.Name = "txtAdvanceCompanyId"
        Me.txtAdvanceCompanyId.Size = New System.Drawing.Size(112, 21)
        Me.txtAdvanceCompanyId.TabIndex = 3
        '
        'lblAdvAmt
        '
        Me.lblAdvAmt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAdvAmt.Location = New System.Drawing.Point(473, 21)
        Me.lblAdvAmt.Name = "lblAdvAmt"
        Me.lblAdvAmt.Size = New System.Drawing.Size(76, 15)
        Me.lblAdvAmt.TabIndex = 12
        Me.lblAdvAmt.Text = "Amount"
        Me.lblAdvAmt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtAdvanceAmt_AMT
        '
        Me.txtAdvanceAmt_AMT.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAdvanceAmt_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdvanceAmt_AMT.Location = New System.Drawing.Point(472, 38)
        Me.txtAdvanceAmt_AMT.MaxLength = 12
        Me.txtAdvanceAmt_AMT.Name = "txtAdvanceAmt_AMT"
        Me.txtAdvanceAmt_AMT.ReadOnly = True
        Me.txtAdvanceAmt_AMT.Size = New System.Drawing.Size(82, 22)
        Me.txtAdvanceAmt_AMT.TabIndex = 13
        '
        'frmAdvanceAdj
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(849, 195)
        Me.ControlBox = False
        Me.Controls.Add(Me.txtAdvanceCompanyId)
        Me.Controls.Add(Me.grpAddress)
        Me.Controls.Add(Me.grpAdvance)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmAdvanceAdj"
        Me.ShowInTaskbar = False
        Me.Text = "[Adjustment] Advance"
        Me.grpAdvance.ResumeLayout(False)
        Me.grpAdvance.PerformLayout()
        CType(Me.gridAdvance, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridAdvanceTotal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlAddr.ResumeLayout(False)
        Me.PnlAddr.PerformLayout()
        Me.grpAddress.ResumeLayout(False)
        Me.grpAddress.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents grpAdvance As CodeVendor.Controls.Grouper
    Friend WithEvents txtAdvanceRowIndex As System.Windows.Forms.TextBox
    Friend WithEvents txtAdvanceEntAmount As System.Windows.Forms.TextBox
    Friend WithEvents txtAdvanceEntAdvanceNo As System.Windows.Forms.TextBox
    Friend WithEvents txtAdvanceAcCode As System.Windows.Forms.TextBox
    Friend WithEvents txtAdvanceRefNo As System.Windows.Forms.TextBox
    Friend WithEvents txtAdvanceAddress3 As System.Windows.Forms.TextBox
    Friend WithEvents txtAdvanceAddress1 As System.Windows.Forms.TextBox
    Friend WithEvents txtAdvanceAddress2 As System.Windows.Forms.TextBox
    Friend WithEvents txtAdvanceName As System.Windows.Forms.TextBox
    Friend WithEvents lblAdvBal As System.Windows.Forms.Label
    Friend WithEvents lblAdvAdj As System.Windows.Forms.Label
    Friend WithEvents lblAdvdate As System.Windows.Forms.Label
    Friend WithEvents lblAdvRecd As System.Windows.Forms.Label
    Friend WithEvents Label64 As System.Windows.Forms.Label
    Friend WithEvents lblAdvNo As System.Windows.Forms.Label
    Friend WithEvents txtAdvanceBalance_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtAdvanceAdjusted_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtAdvanceReceived_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtAdvanceNo As System.Windows.Forms.TextBox
    Friend WithEvents gridAdvance As System.Windows.Forms.DataGridView
    Friend WithEvents gridAdvanceTotal As System.Windows.Forms.DataGridView
    Friend WithEvents grpAddress As CodeVendor.Controls.Grouper
    Friend WithEvents txtAddressRegularSno As System.Windows.Forms.TextBox
    Friend WithEvents dtpAddressDueDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents txtAddressInitial As System.Windows.Forms.TextBox
    Friend WithEvents cmbAddressTitle_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label122 As System.Windows.Forms.Label
    Friend WithEvents txtAddressFax As System.Windows.Forms.TextBox
    Friend WithEvents txtAddressPincode_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtAddressMobile As System.Windows.Forms.TextBox
    Friend WithEvents Label110 As System.Windows.Forms.Label
    Friend WithEvents txtAddressEmailId_OWN As System.Windows.Forms.TextBox
    Friend WithEvents Label111 As System.Windows.Forms.Label
    Friend WithEvents txtAddressPhoneRes As System.Windows.Forms.TextBox
    Friend WithEvents Label112 As System.Windows.Forms.Label
    Friend WithEvents txtAddressPrevilegeId As System.Windows.Forms.TextBox
    Friend WithEvents cmbAddressCountry_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents txtAddressPartyCode As System.Windows.Forms.TextBox
    Friend WithEvents cmbAddressState_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label113 As System.Windows.Forms.Label
    Friend WithEvents Label117 As System.Windows.Forms.Label
    Friend WithEvents Label114 As System.Windows.Forms.Label
    Friend WithEvents Label116 As System.Windows.Forms.Label
    Friend WithEvents lblAddressDueDate As System.Windows.Forms.Label
    Friend WithEvents Label121 As System.Windows.Forms.Label
    Friend WithEvents Label115 As System.Windows.Forms.Label
    Friend WithEvents Label119 As System.Windows.Forms.Label
    Friend WithEvents txtAddress3 As System.Windows.Forms.TextBox
    Friend WithEvents Label120 As System.Windows.Forms.Label
    Friend WithEvents cmbAddressArea_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents Label118 As System.Windows.Forms.Label
    Friend WithEvents txtAddress1 As System.Windows.Forms.TextBox
    Friend WithEvents cmbAddressCity_OWN As System.Windows.Forms.ComboBox
    Friend WithEvents txtAddress2 As System.Windows.Forms.TextBox
    Friend WithEvents txtAddressName As System.Windows.Forms.TextBox
    Friend WithEvents txtAddressDoorNo As System.Windows.Forms.TextBox
    Friend WithEvents dtpAdvanceDate As BrighttechPack.DatePicker
    Friend WithEvents txtAdvanceCompanyId As System.Windows.Forms.TextBox
    Friend WithEvents txtRemark As System.Windows.Forms.TextBox
    Friend WithEvents txtAdvCostid As System.Windows.Forms.TextBox
    Friend WithEvents lblAdvCostid As System.Windows.Forms.Label
    Friend WithEvents txtAdvanceGST_AMT As System.Windows.Forms.TextBox
    Friend WithEvents lblAdvGst As System.Windows.Forms.Label
    Friend WithEvents PnlAddr As System.Windows.Forms.Panel
    Friend WithEvents lblAdvAmt As System.Windows.Forms.Label
    Friend WithEvents txtAdvanceAmt_AMT As System.Windows.Forms.TextBox
End Class
