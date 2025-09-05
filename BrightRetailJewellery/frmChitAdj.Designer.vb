<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChitAdj
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
        Me.grpCHIT = New CodeVendor.Controls.Grouper()
        Me.cmbBonusType = New System.Windows.Forms.ComboBox()
        Me.lblBonusType = New System.Windows.Forms.Label()
        Me.lblBonusDeduction = New System.Windows.Forms.Label()
        Me.lblRemarks = New System.Windows.Forms.Label()
        Me.lblPDCCheque = New System.Windows.Forms.Label()
        Me.lblCardLost = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtCHITGST_AMT = New System.Windows.Forms.TextBox()
        Me.lblMobileNo = New System.Windows.Forms.Label()
        Me.lblPname = New System.Windows.Forms.Label()
        Me.txtChitslipNo = New System.Windows.Forms.TextBox()
        Me.lblSlipNo = New System.Windows.Forms.Label()
        Me.lblRate = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lblSetwt = New System.Windows.Forms.Label()
        Me.txtSettleWt = New System.Windows.Forms.TextBox()
        Me.txtCHITCardSchemeId = New System.Windows.Forms.TextBox()
        Me.lblInterestPer = New System.Windows.Forms.Label()
        Me.lblAddress = New System.Windows.Forms.Label()
        Me.lblWeightSchemeDetail = New System.Windows.Forms.Label()
        Me.cmbCHITtCardType_MAN = New System.Windows.Forms.ComboBox()
        Me.txtCHITCardRegNo_NUM = New System.Windows.Forms.TextBox()
        Me.txtCHITCardGrpCode = New System.Windows.Forms.TextBox()
        Me.txtCHITCardTotal_AMT = New System.Windows.Forms.TextBox()
        Me.txtCHITCardDeduction_AMT = New System.Windows.Forms.TextBox()
        Me.txtCHITCardPrize_AMT = New System.Windows.Forms.TextBox()
        Me.txtCHITCardBonus_AMT = New System.Windows.Forms.TextBox()
        Me.txtCHITCardGift_AMT = New System.Windows.Forms.TextBox()
        Me.txtCHITCardAmount_AMT = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label126 = New System.Windows.Forms.Label()
        Me.Label124 = New System.Windows.Forms.Label()
        Me.txtCHITCardRowIndex = New System.Windows.Forms.TextBox()
        Me.gridCHITCardTotal = New System.Windows.Forms.DataGridView()
        Me.gridCHITCard = New System.Windows.Forms.DataGridView()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.grpCHIT.SuspendLayout()
        CType(Me.gridCHITCardTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridCHITCard, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpCHIT
        '
        Me.grpCHIT.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpCHIT.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpCHIT.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpCHIT.BorderColor = System.Drawing.Color.Transparent
        Me.grpCHIT.BorderThickness = 1.0!
        Me.grpCHIT.Controls.Add(Me.cmbBonusType)
        Me.grpCHIT.Controls.Add(Me.lblBonusType)
        Me.grpCHIT.Controls.Add(Me.lblBonusDeduction)
        Me.grpCHIT.Controls.Add(Me.lblRemarks)
        Me.grpCHIT.Controls.Add(Me.lblPDCCheque)
        Me.grpCHIT.Controls.Add(Me.lblCardLost)
        Me.grpCHIT.Controls.Add(Me.Label10)
        Me.grpCHIT.Controls.Add(Me.txtCHITGST_AMT)
        Me.grpCHIT.Controls.Add(Me.lblMobileNo)
        Me.grpCHIT.Controls.Add(Me.lblPname)
        Me.grpCHIT.Controls.Add(Me.txtChitslipNo)
        Me.grpCHIT.Controls.Add(Me.lblSlipNo)
        Me.grpCHIT.Controls.Add(Me.lblRate)
        Me.grpCHIT.Controls.Add(Me.Label8)
        Me.grpCHIT.Controls.Add(Me.lblSetwt)
        Me.grpCHIT.Controls.Add(Me.txtSettleWt)
        Me.grpCHIT.Controls.Add(Me.txtCHITCardSchemeId)
        Me.grpCHIT.Controls.Add(Me.lblInterestPer)
        Me.grpCHIT.Controls.Add(Me.lblAddress)
        Me.grpCHIT.Controls.Add(Me.lblWeightSchemeDetail)
        Me.grpCHIT.Controls.Add(Me.cmbCHITtCardType_MAN)
        Me.grpCHIT.Controls.Add(Me.txtCHITCardRegNo_NUM)
        Me.grpCHIT.Controls.Add(Me.txtCHITCardGrpCode)
        Me.grpCHIT.Controls.Add(Me.txtCHITCardTotal_AMT)
        Me.grpCHIT.Controls.Add(Me.txtCHITCardDeduction_AMT)
        Me.grpCHIT.Controls.Add(Me.txtCHITCardPrize_AMT)
        Me.grpCHIT.Controls.Add(Me.txtCHITCardBonus_AMT)
        Me.grpCHIT.Controls.Add(Me.txtCHITCardGift_AMT)
        Me.grpCHIT.Controls.Add(Me.txtCHITCardAmount_AMT)
        Me.grpCHIT.Controls.Add(Me.Label2)
        Me.grpCHIT.Controls.Add(Me.Label1)
        Me.grpCHIT.Controls.Add(Me.Label7)
        Me.grpCHIT.Controls.Add(Me.Label6)
        Me.grpCHIT.Controls.Add(Me.Label4)
        Me.grpCHIT.Controls.Add(Me.Label5)
        Me.grpCHIT.Controls.Add(Me.Label3)
        Me.grpCHIT.Controls.Add(Me.Label126)
        Me.grpCHIT.Controls.Add(Me.Label124)
        Me.grpCHIT.Controls.Add(Me.txtCHITCardRowIndex)
        Me.grpCHIT.Controls.Add(Me.gridCHITCardTotal)
        Me.grpCHIT.Controls.Add(Me.gridCHITCard)
        Me.grpCHIT.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpCHIT.GroupImage = Nothing
        Me.grpCHIT.GroupTitle = ""
        Me.grpCHIT.Location = New System.Drawing.Point(4, -5)
        Me.grpCHIT.Name = "grpCHIT"
        Me.grpCHIT.Padding = New System.Windows.Forms.Padding(20)
        Me.grpCHIT.PaintGroupBox = False
        Me.grpCHIT.RoundCorners = 10
        Me.grpCHIT.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpCHIT.ShadowControl = False
        Me.grpCHIT.ShadowThickness = 3
        Me.grpCHIT.Size = New System.Drawing.Size(821, 316)
        Me.grpCHIT.TabIndex = 0
        '
        'cmbBonusType
        '
        Me.cmbBonusType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBonusType.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbBonusType.FormattingEnabled = True
        Me.cmbBonusType.Location = New System.Drawing.Point(88, 73)
        Me.cmbBonusType.Name = "cmbBonusType"
        Me.cmbBonusType.Size = New System.Drawing.Size(116, 22)
        Me.cmbBonusType.TabIndex = 6
        Me.cmbBonusType.Visible = False
        '
        'lblBonusType
        '
        Me.lblBonusType.AutoSize = True
        Me.lblBonusType.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBonusType.Location = New System.Drawing.Point(3, 77)
        Me.lblBonusType.Name = "lblBonusType"
        Me.lblBonusType.Size = New System.Drawing.Size(79, 14)
        Me.lblBonusType.TabIndex = 5
        Me.lblBonusType.Text = "BonusType"
        Me.lblBonusType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblBonusType.Visible = False
        '
        'lblBonusDeduction
        '
        Me.lblBonusDeduction.AutoSize = True
        Me.lblBonusDeduction.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBonusDeduction.ForeColor = System.Drawing.Color.Red
        Me.lblBonusDeduction.Location = New System.Drawing.Point(3, 78)
        Me.lblBonusDeduction.Name = "lblBonusDeduction"
        Me.lblBonusDeduction.Size = New System.Drawing.Size(0, 13)
        Me.lblBonusDeduction.TabIndex = 40
        '
        'lblRemarks
        '
        Me.lblRemarks.AutoSize = True
        Me.lblRemarks.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRemarks.ForeColor = System.Drawing.Color.Red
        Me.lblRemarks.Location = New System.Drawing.Point(395, 97)
        Me.lblRemarks.Name = "lblRemarks"
        Me.lblRemarks.Size = New System.Drawing.Size(64, 13)
        Me.lblRemarks.TabIndex = 37
        Me.lblRemarks.Text = "Remarks"
        '
        'lblPDCCheque
        '
        Me.lblPDCCheque.AutoSize = True
        Me.lblPDCCheque.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPDCCheque.ForeColor = System.Drawing.Color.Red
        Me.lblPDCCheque.Location = New System.Drawing.Point(8, 97)
        Me.lblPDCCheque.Name = "lblPDCCheque"
        Me.lblPDCCheque.Size = New System.Drawing.Size(149, 13)
        Me.lblPDCCheque.TabIndex = 38
        Me.lblPDCCheque.Text = "PDC Cheque Available"
        '
        'lblCardLost
        '
        Me.lblCardLost.AutoSize = True
        Me.lblCardLost.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCardLost.ForeColor = System.Drawing.Color.Red
        Me.lblCardLost.Location = New System.Drawing.Point(169, 97)
        Me.lblCardLost.Name = "lblCardLost"
        Me.lblCardLost.Size = New System.Drawing.Size(168, 13)
        Me.lblCardLost.TabIndex = 39
        Me.lblCardLost.Text = "Card Lost on 30-05-2018"
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(296, 116)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(69, 17)
        Me.Label10.TabIndex = 17
        Me.Label10.Text = "GST"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtCHITGST_AMT
        '
        Me.txtCHITGST_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCHITGST_AMT.Location = New System.Drawing.Point(293, 135)
        Me.txtCHITGST_AMT.MaxLength = 12
        Me.txtCHITGST_AMT.Name = "txtCHITGST_AMT"
        Me.txtCHITGST_AMT.Size = New System.Drawing.Size(74, 22)
        Me.txtCHITGST_AMT.TabIndex = 18
        Me.txtCHITGST_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblMobileNo
        '
        Me.lblMobileNo.AutoSize = True
        Me.lblMobileNo.Location = New System.Drawing.Point(379, 199)
        Me.lblMobileNo.Name = "lblMobileNo"
        Me.lblMobileNo.Size = New System.Drawing.Size(51, 13)
        Me.lblMobileNo.TabIndex = 31
        Me.lblMobileNo.Text = "Label10"
        Me.lblMobileNo.Visible = False
        '
        'lblPname
        '
        Me.lblPname.AutoSize = True
        Me.lblPname.Location = New System.Drawing.Point(322, 199)
        Me.lblPname.Name = "lblPname"
        Me.lblPname.Size = New System.Drawing.Size(51, 13)
        Me.lblPname.TabIndex = 30
        Me.lblPname.Text = "Label10"
        Me.lblPname.Visible = False
        '
        'txtChitslipNo
        '
        Me.txtChitslipNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtChitslipNo.Location = New System.Drawing.Point(3, 135)
        Me.txtChitslipNo.MaxLength = 12
        Me.txtChitslipNo.Name = "txtChitslipNo"
        Me.txtChitslipNo.Size = New System.Drawing.Size(48, 22)
        Me.txtChitslipNo.TabIndex = 10
        Me.txtChitslipNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblSlipNo
        '
        Me.lblSlipNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSlipNo.Location = New System.Drawing.Point(8, 115)
        Me.lblSlipNo.Name = "lblSlipNo"
        Me.lblSlipNo.Size = New System.Drawing.Size(48, 17)
        Me.lblSlipNo.TabIndex = 9
        Me.lblSlipNo.Text = "Slip"
        Me.lblSlipNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblRate
        '
        Me.lblRate.AutoSize = True
        Me.lblRate.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRate.ForeColor = System.Drawing.Color.Red
        Me.lblRate.Location = New System.Drawing.Point(237, 12)
        Me.lblRate.Name = "lblRate"
        Me.lblRate.Size = New System.Drawing.Size(36, 13)
        Me.lblRate.TabIndex = 34
        Me.lblRate.Text = "Rate"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(3, 36)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(45, 14)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "Name"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSetwt
        '
        Me.lblSetwt.AutoSize = True
        Me.lblSetwt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSetwt.Location = New System.Drawing.Point(210, 77)
        Me.lblSetwt.Name = "lblSetwt"
        Me.lblSetwt.Size = New System.Drawing.Size(63, 14)
        Me.lblSetwt.TabIndex = 7
        Me.lblSetwt.Text = "Paid Wt."
        Me.lblSetwt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblSetwt.Visible = False
        '
        'txtSettleWt
        '
        Me.txtSettleWt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSettleWt.Location = New System.Drawing.Point(276, 73)
        Me.txtSettleWt.MaxLength = 12
        Me.txtSettleWt.Name = "txtSettleWt"
        Me.txtSettleWt.Size = New System.Drawing.Size(67, 22)
        Me.txtSettleWt.TabIndex = 8
        Me.txtSettleWt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtSettleWt.Visible = False
        '
        'txtCHITCardSchemeId
        '
        Me.txtCHITCardSchemeId.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.txtCHITCardSchemeId.Location = New System.Drawing.Point(58, 12)
        Me.txtCHITCardSchemeId.Name = "txtCHITCardSchemeId"
        Me.txtCHITCardSchemeId.Size = New System.Drawing.Size(61, 22)
        Me.txtCHITCardSchemeId.TabIndex = 1
        '
        'lblInterestPer
        '
        Me.lblInterestPer.AutoSize = True
        Me.lblInterestPer.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInterestPer.ForeColor = System.Drawing.Color.Red
        Me.lblInterestPer.Location = New System.Drawing.Point(134, 13)
        Me.lblInterestPer.Name = "lblInterestPer"
        Me.lblInterestPer.Size = New System.Drawing.Size(94, 13)
        Me.lblInterestPer.TabIndex = 33
        Me.lblInterestPer.Text = "Interest 17%"
        '
        'lblAddress
        '
        Me.lblAddress.ForeColor = System.Drawing.Color.DarkBlue
        Me.lblAddress.Location = New System.Drawing.Point(395, 12)
        Me.lblAddress.Name = "lblAddress"
        Me.lblAddress.Size = New System.Drawing.Size(391, 77)
        Me.lblAddress.TabIndex = 36
        Me.lblAddress.Text = "Label8" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "3" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "4" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "5" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "6"
        '
        'lblWeightSchemeDetail
        '
        Me.lblWeightSchemeDetail.AutoSize = True
        Me.lblWeightSchemeDetail.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWeightSchemeDetail.ForeColor = System.Drawing.Color.Red
        Me.lblWeightSchemeDetail.Location = New System.Drawing.Point(3, 61)
        Me.lblWeightSchemeDetail.Name = "lblWeightSchemeDetail"
        Me.lblWeightSchemeDetail.Size = New System.Drawing.Size(50, 13)
        Me.lblWeightSchemeDetail.TabIndex = 4
        Me.lblWeightSchemeDetail.Text = "Label9"
        '
        'cmbCHITtCardType_MAN
        '
        Me.cmbCHITtCardType_MAN.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbCHITtCardType_MAN.FormattingEnabled = True
        Me.cmbCHITtCardType_MAN.Location = New System.Drawing.Point(58, 36)
        Me.cmbCHITtCardType_MAN.Name = "cmbCHITtCardType_MAN"
        Me.cmbCHITtCardType_MAN.Size = New System.Drawing.Size(285, 22)
        Me.cmbCHITtCardType_MAN.TabIndex = 3
        '
        'txtCHITCardRegNo_NUM
        '
        Me.txtCHITCardRegNo_NUM.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCHITCardRegNo_NUM.Location = New System.Drawing.Point(121, 135)
        Me.txtCHITCardRegNo_NUM.MaxLength = 12
        Me.txtCHITCardRegNo_NUM.Name = "txtCHITCardRegNo_NUM"
        Me.txtCHITCardRegNo_NUM.Size = New System.Drawing.Size(84, 22)
        Me.txtCHITCardRegNo_NUM.TabIndex = 14
        Me.txtCHITCardRegNo_NUM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCHITCardGrpCode
        '
        Me.txtCHITCardGrpCode.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCHITCardGrpCode.Location = New System.Drawing.Point(52, 135)
        Me.txtCHITCardGrpCode.MaxLength = 12
        Me.txtCHITCardGrpCode.Name = "txtCHITCardGrpCode"
        Me.txtCHITCardGrpCode.Size = New System.Drawing.Size(68, 22)
        Me.txtCHITCardGrpCode.TabIndex = 12
        Me.txtCHITCardGrpCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCHITCardTotal_AMT
        '
        Me.txtCHITCardTotal_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCHITCardTotal_AMT.Location = New System.Drawing.Point(716, 135)
        Me.txtCHITCardTotal_AMT.MaxLength = 12
        Me.txtCHITCardTotal_AMT.Name = "txtCHITCardTotal_AMT"
        Me.txtCHITCardTotal_AMT.Size = New System.Drawing.Size(86, 22)
        Me.txtCHITCardTotal_AMT.TabIndex = 28
        Me.txtCHITCardTotal_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCHITCardDeduction_AMT
        '
        Me.txtCHITCardDeduction_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCHITCardDeduction_AMT.Location = New System.Drawing.Point(629, 135)
        Me.txtCHITCardDeduction_AMT.MaxLength = 12
        Me.txtCHITCardDeduction_AMT.Name = "txtCHITCardDeduction_AMT"
        Me.txtCHITCardDeduction_AMT.Size = New System.Drawing.Size(86, 22)
        Me.txtCHITCardDeduction_AMT.TabIndex = 26
        Me.txtCHITCardDeduction_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCHITCardPrize_AMT
        '
        Me.txtCHITCardPrize_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCHITCardPrize_AMT.Location = New System.Drawing.Point(455, 135)
        Me.txtCHITCardPrize_AMT.MaxLength = 12
        Me.txtCHITCardPrize_AMT.Name = "txtCHITCardPrize_AMT"
        Me.txtCHITCardPrize_AMT.Size = New System.Drawing.Size(86, 22)
        Me.txtCHITCardPrize_AMT.TabIndex = 22
        Me.txtCHITCardPrize_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCHITCardBonus_AMT
        '
        Me.txtCHITCardBonus_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCHITCardBonus_AMT.Location = New System.Drawing.Point(542, 135)
        Me.txtCHITCardBonus_AMT.MaxLength = 12
        Me.txtCHITCardBonus_AMT.Name = "txtCHITCardBonus_AMT"
        Me.txtCHITCardBonus_AMT.Size = New System.Drawing.Size(86, 22)
        Me.txtCHITCardBonus_AMT.TabIndex = 24
        Me.txtCHITCardBonus_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCHITCardGift_AMT
        '
        Me.txtCHITCardGift_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCHITCardGift_AMT.Location = New System.Drawing.Point(368, 135)
        Me.txtCHITCardGift_AMT.MaxLength = 12
        Me.txtCHITCardGift_AMT.Name = "txtCHITCardGift_AMT"
        Me.txtCHITCardGift_AMT.Size = New System.Drawing.Size(86, 22)
        Me.txtCHITCardGift_AMT.TabIndex = 20
        Me.txtCHITCardGift_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCHITCardAmount_AMT
        '
        Me.txtCHITCardAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCHITCardAmount_AMT.Location = New System.Drawing.Point(206, 135)
        Me.txtCHITCardAmount_AMT.MaxLength = 12
        Me.txtCHITCardAmount_AMT.Name = "txtCHITCardAmount_AMT"
        Me.txtCHITCardAmount_AMT.Size = New System.Drawing.Size(86, 22)
        Me.txtCHITCardAmount_AMT.TabIndex = 16
        Me.txtCHITCardAmount_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(121, 116)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 17)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "Reg No"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(52, 116)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 17)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Grp Code"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(716, 116)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(86, 17)
        Me.Label7.TabIndex = 27
        Me.Label7.Text = "Total"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(629, 116)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(86, 17)
        Me.Label6.TabIndex = 25
        Me.Label6.Text = "Deduction"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(455, 116)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(86, 17)
        Me.Label4.TabIndex = 21
        Me.Label4.Text = "Prize Val"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(542, 116)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(86, 17)
        Me.Label5.TabIndex = 23
        Me.Label5.Text = "Bonus"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(368, 116)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(86, 17)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "Gift Val"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label126
        '
        Me.Label126.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label126.Location = New System.Drawing.Point(206, 116)
        Me.Label126.Name = "Label126"
        Me.Label126.Size = New System.Drawing.Size(86, 17)
        Me.Label126.TabIndex = 15
        Me.Label126.Text = "Amount"
        Me.Label126.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label124
        '
        Me.Label124.AutoSize = True
        Me.Label124.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label124.Location = New System.Drawing.Point(3, 12)
        Me.Label124.Name = "Label124"
        Me.Label124.Size = New System.Drawing.Size(53, 14)
        Me.Label124.TabIndex = 0
        Me.Label124.Text = "Sch. Id"
        Me.Label124.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtCHITCardRowIndex
        '
        Me.txtCHITCardRowIndex.Location = New System.Drawing.Point(292, 13)
        Me.txtCHITCardRowIndex.Name = "txtCHITCardRowIndex"
        Me.txtCHITCardRowIndex.Size = New System.Drawing.Size(14, 21)
        Me.txtCHITCardRowIndex.TabIndex = 35
        Me.txtCHITCardRowIndex.Visible = False
        '
        'gridCHITCardTotal
        '
        Me.gridCHITCardTotal.AllowUserToAddRows = False
        Me.gridCHITCardTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridCHITCardTotal.ColumnHeadersVisible = False
        Me.gridCHITCardTotal.Location = New System.Drawing.Point(6, 292)
        Me.gridCHITCardTotal.MultiSelect = False
        Me.gridCHITCardTotal.Name = "gridCHITCardTotal"
        Me.gridCHITCardTotal.ReadOnly = True
        Me.gridCHITCardTotal.RowHeadersVisible = False
        Me.gridCHITCardTotal.RowTemplate.Height = 20
        Me.gridCHITCardTotal.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridCHITCardTotal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridCHITCardTotal.Size = New System.Drawing.Size(809, 19)
        Me.gridCHITCardTotal.TabIndex = 32
        '
        'gridCHITCard
        '
        Me.gridCHITCard.AllowUserToAddRows = False
        Me.gridCHITCard.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridCHITCard.ColumnHeadersVisible = False
        Me.gridCHITCard.Location = New System.Drawing.Point(6, 158)
        Me.gridCHITCard.MultiSelect = False
        Me.gridCHITCard.Name = "gridCHITCard"
        Me.gridCHITCard.ReadOnly = True
        Me.gridCHITCard.RowHeadersVisible = False
        Me.gridCHITCard.RowTemplate.Height = 20
        Me.gridCHITCard.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.gridCHITCard.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridCHITCard.Size = New System.Drawing.Size(809, 134)
        Me.gridCHITCard.TabIndex = 29
        '
        'Timer1
        '
        Me.Timer1.Interval = 60
        '
        'frmChitAdj
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(831, 312)
        Me.ControlBox = False
        Me.Controls.Add(Me.grpCHIT)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmChitAdj"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Scheme Adjustment"
        Me.grpCHIT.ResumeLayout(False)
        Me.grpCHIT.PerformLayout()
        CType(Me.gridCHITCardTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridCHITCard, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpCHIT As CodeVendor.Controls.Grouper
    Friend WithEvents txtCHITCardRowIndex As System.Windows.Forms.TextBox
    Friend WithEvents gridCHITCardTotal As System.Windows.Forms.DataGridView
    Friend WithEvents gridCHITCard As System.Windows.Forms.DataGridView
    Friend WithEvents cmbCHITtCardType_MAN As System.Windows.Forms.ComboBox
    Friend WithEvents txtCHITCardAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label126 As System.Windows.Forms.Label
    Friend WithEvents Label124 As System.Windows.Forms.Label
    Friend WithEvents txtCHITCardRegNo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtCHITCardGrpCode As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtCHITCardDeduction_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtCHITCardPrize_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtCHITCardBonus_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtCHITCardGift_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtCHITCardTotal_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents lblAddress As System.Windows.Forms.Label
    Friend WithEvents lblWeightSchemeDetail As System.Windows.Forms.Label
    Friend WithEvents lblInterestPer As System.Windows.Forms.Label
    Friend WithEvents txtCHITCardSchemeId As System.Windows.Forms.TextBox
    Friend WithEvents lblSetwt As System.Windows.Forms.Label
    Friend WithEvents txtSettleWt As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lblRate As System.Windows.Forms.Label
    Friend WithEvents txtChitslipNo As System.Windows.Forms.TextBox
    Friend WithEvents lblSlipNo As System.Windows.Forms.Label
    Friend WithEvents lblMobileNo As System.Windows.Forms.Label
    Friend WithEvents lblPname As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtCHITGST_AMT As System.Windows.Forms.TextBox
    Friend WithEvents lblRemarks As System.Windows.Forms.Label
    Friend WithEvents lblPDCCheque As System.Windows.Forms.Label
    Friend WithEvents lblCardLost As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents lblBonusDeduction As Label
    Friend WithEvents lblBonusType As Label
    Friend WithEvents cmbBonusType As ComboBox
End Class
