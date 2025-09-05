<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class RetailBill
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RetailBill))
        Me.txtSAItemId = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtSAPcs_NUM = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtSANetWt_WET = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtSAGrsWt_WET = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtSARate_AMT = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtSAWastage_WET = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtSAMc_AMT = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtSAAmount_AMT = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtSAVat_AMT = New System.Windows.Forms.TextBox()
        Me.lblSaVat = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtPUPcs_NUM = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.txtPUGrsWt_WET = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.txtPUDustWt_WET = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.txtPUWastage_WET = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtPUStoneWt_WET = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txtPUNetWt_WET = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.txtPURate_AMT = New System.Windows.Forms.TextBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.txtPUAmount_AMT = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.txtPUVat_AMT = New System.Windows.Forms.TextBox()
        Me.lblPUVat = New System.Windows.Forms.Label()
        Me.txtSAEstNo_NUM = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtSATagNo = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblSilverRate = New System.Windows.Forms.Label()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.lblGoldRate = New System.Windows.Forms.Label()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.lblCashCounter = New System.Windows.Forms.Label()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.lblBillDate = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.tabMain = New System.Windows.Forms.TabControl()
        Me.tabSaSrPu = New System.Windows.Forms.TabPage()
        Me.grpSaSr = New CodeVendor.Controls.Grouper()
        Me.txtSARowIndex = New System.Windows.Forms.TextBox()
        Me.txtSAEmpId_NUM = New System.Windows.Forms.TextBox()
        Me.txtSAGrossAmount_AMT = New System.Windows.Forms.TextBox()
        Me.gridSASRTotal = New System.Windows.Forms.DataGridView()
        Me.gridSASR = New System.Windows.Forms.DataGridView()
        Me.txtSAStoneAmount_AMT = New System.Windows.Forms.TextBox()
        Me.Label62 = New System.Windows.Forms.Label()
        Me.Label55 = New System.Windows.Forms.Label()
        Me.grpPu = New CodeVendor.Controls.Grouper()
        Me.txtPUMeltWt_WET = New System.Windows.Forms.TextBox()
        Me.txtPUPurity_PER = New System.Windows.Forms.TextBox()
        Me.txtPuWastage_PER = New System.Windows.Forms.TextBox()
        Me.txtPUEstNo_NUM = New System.Windows.Forms.TextBox()
        Me.txtPURowIndex = New System.Windows.Forms.TextBox()
        Me.txtPUCategory = New System.Windows.Forms.TextBox()
        Me.txtPUEmpId_NUM = New System.Windows.Forms.TextBox()
        Me.gridPur = New System.Windows.Forms.DataGridView()
        Me.Label96 = New System.Windows.Forms.Label()
        Me.gridPurTotal = New System.Windows.Forms.DataGridView()
        Me.Label63 = New System.Windows.Forms.Label()
        Me.Label51 = New System.Windows.Forms.Label()
        Me.tabReceipt = New System.Windows.Forms.TabPage()
        Me.grpReceipt = New CodeVendor.Controls.Grouper()
        Me.cmenuReceiptGrid = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tStripWeightAdvance = New System.Windows.Forms.ToolStripMenuItem()
        Me.tagReservation = New System.Windows.Forms.ToolStripMenuItem()
        Me.txtReceiptTotAmt_AMT = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txtReceiptGST_AMT = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtReceiptType = New System.Windows.Forms.TextBox()
        Me.txtReceiptAccount = New System.Windows.Forms.TextBox()
        Me.lblEsthelp = New System.Windows.Forms.Label()
        Me.lblRecOnAcc = New System.Windows.Forms.Label()
        Me.txtReceiptEmpId_NUM = New System.Windows.Forms.TextBox()
        Me.txtReceiptRemark = New System.Windows.Forms.TextBox()
        Me.Label58 = New System.Windows.Forms.Label()
        Me.txtReceiptEntRefNo = New System.Windows.Forms.TextBox()
        Me.txtReceiptEntAmount = New System.Windows.Forms.TextBox()
        Me.chkReceiptRateFix = New System.Windows.Forms.CheckBox()
        Me.gridReceipt = New System.Windows.Forms.DataGridView()
        Me.cmbReceiptTranType = New System.Windows.Forms.ComboBox()
        Me.cmbReceiptReceiptType = New System.Windows.Forms.ComboBox()
        Me.txtReceiptRate_AMT = New System.Windows.Forms.TextBox()
        Me.txtReceiptAmount_AMT = New System.Windows.Forms.TextBox()
        Me.txtReceiptRefNo = New System.Windows.Forms.TextBox()
        Me.gridReceiptTotal = New System.Windows.Forms.DataGridView()
        Me.Label56 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Label169 = New System.Windows.Forms.Label()
        Me.Label168 = New System.Windows.Forms.Label()
        Me.lblRecTrantype = New System.Windows.Forms.Label()
        Me.lblRecRefno = New System.Windows.Forms.Label()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.tabPayment = New System.Windows.Forms.TabPage()
        Me.grpPayment = New CodeVendor.Controls.Grouper()
        Me.txtPaymentTotAmt_AMT = New System.Windows.Forms.TextBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.txtPaymentGST_AMT = New System.Windows.Forms.TextBox()
        Me.Label80 = New System.Windows.Forms.Label()
        Me.txtPayRefAccode = New System.Windows.Forms.TextBox()
        Me.txtPaymentAccount = New System.Windows.Forms.TextBox()
        Me.Label101 = New System.Windows.Forms.Label()
        Me.txtPaymentEmpId_NUM = New System.Windows.Forms.TextBox()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.txtPaymentRemark = New System.Windows.Forms.TextBox()
        Me.txtPaymentEntRefNo = New System.Windows.Forms.TextBox()
        Me.txtPaymentEntAmount = New System.Windows.Forms.TextBox()
        Me.gridPayment = New System.Windows.Forms.DataGridView()
        Me.cmbPaymentTranType = New System.Windows.Forms.ComboBox()
        Me.cmbPaymentPaytype = New System.Windows.Forms.ComboBox()
        Me.txtPaymentRate_AMT = New System.Windows.Forms.TextBox()
        Me.txtPaymentAmount_Amt = New System.Windows.Forms.TextBox()
        Me.txtPaymentRefNo = New System.Windows.Forms.TextBox()
        Me.gridPaymentTotal = New System.Windows.Forms.DataGridView()
        Me.Label74 = New System.Windows.Forms.Label()
        Me.Label77 = New System.Windows.Forms.Label()
        Me.lblPayType = New System.Windows.Forms.Label()
        Me.Label81 = New System.Windows.Forms.Label()
        Me.Label82 = New System.Windows.Forms.Label()
        Me.Label83 = New System.Windows.Forms.Label()
        Me.txtPaymentWeight_WET = New System.Windows.Forms.TextBox()
        Me.tabGiftVoucherMain = New System.Windows.Forms.TabPage()
        Me.grpGiftVoucherMain = New CodeVendor.Controls.Grouper()
        Me.txtMGiftRowIndex = New System.Windows.Forms.TextBox()
        Me.Label85 = New System.Windows.Forms.Label()
        Me.txtMGiftRemark = New System.Windows.Forms.TextBox()
        Me.gridMGiftVoucherTotal = New System.Windows.Forms.DataGridView()
        Me.gridMGiftVoucher = New System.Windows.Forms.DataGridView()
        Me.Label92 = New System.Windows.Forms.Label()
        Me.Label93 = New System.Windows.Forms.Label()
        Me.Label94 = New System.Windows.Forms.Label()
        Me.Label95 = New System.Windows.Forms.Label()
        Me.txtMGiftAmount_AMT = New System.Windows.Forms.TextBox()
        Me.txtMGiftUnit_NUM = New System.Windows.Forms.TextBox()
        Me.txtMGiftDenomination_AMT = New System.Windows.Forms.TextBox()
        Me.cmbMGiftVoucherType = New System.Windows.Forms.ComboBox()
        Me.txtSaDiscountAfterTax = New System.Windows.Forms.TextBox()
        Me.txtSAVatPer_PER = New System.Windows.Forms.TextBox()
        Me.grpRecReservedItem = New CodeVendor.Controls.Grouper()
        Me.txtRecEmpId_NUM = New System.Windows.Forms.TextBox()
        Me.Label140 = New System.Windows.Forms.Label()
        Me.txtReceiptEst_Num = New System.Windows.Forms.TextBox()
        Me.Label108 = New System.Windows.Forms.Label()
        Me.txtReceiptValue = New System.Windows.Forms.TextBox()
        Me.txtReceiptNetWT = New System.Windows.Forms.TextBox()
        Me.txtReceiptPcs = New System.Windows.Forms.TextBox()
        Me.txtReceiptGrsWt = New System.Windows.Forms.TextBox()
        Me.Label67 = New System.Windows.Forms.Label()
        Me.txtReceiptItemName = New System.Windows.Forms.TextBox()
        Me.gridReceiptReserved = New System.Windows.Forms.DataGridView()
        Me.txtReceiptTagNo = New System.Windows.Forms.TextBox()
        Me.txtReceiptItemId_MAN = New System.Windows.Forms.TextBox()
        Me.Label73 = New System.Windows.Forms.Label()
        Me.Label70 = New System.Windows.Forms.Label()
        Me.Label69 = New System.Windows.Forms.Label()
        Me.Label68 = New System.Windows.Forms.Label()
        Me.Label66 = New System.Windows.Forms.Label()
        Me.Label65 = New System.Windows.Forms.Label()
        Me.grpReceiptWeightAdvance = New CodeVendor.Controls.Grouper()
        Me.Label176 = New System.Windows.Forms.Label()
        Me.txtReceiptBullionRate_RATE = New System.Windows.Forms.TextBox()
        Me.txtReceiptWastage_WET = New System.Windows.Forms.TextBox()
        Me.txtReceiptNetWt_WET = New System.Windows.Forms.TextBox()
        Me.txtReceiptValue_AMT = New System.Windows.Forms.TextBox()
        Me.Label172 = New System.Windows.Forms.Label()
        Me.Label171 = New System.Windows.Forms.Label()
        Me.Label177 = New System.Windows.Forms.Label()
        Me.Label170 = New System.Windows.Forms.Label()
        Me.Label173 = New System.Windows.Forms.Label()
        Me.Label174 = New System.Windows.Forms.Label()
        Me.txtReceiptGrsWt_WET = New System.Windows.Forms.TextBox()
        Me.txtReceiptPurity_AMT = New System.Windows.Forms.TextBox()
        Me.cmbReceiptCategory = New System.Windows.Forms.ComboBox()
        Me.tabOtherOptions = New System.Windows.Forms.TabControl()
        Me.tabGeneral = New System.Windows.Forms.TabPage()
        Me.grpGeneral1 = New CodeVendor.Controls.Grouper()
        Me.pnlFinalBalance = New System.Windows.Forms.Panel()
        Me.lblTCS = New System.Windows.Forms.Label()
        Me.txtTCS_Amt = New System.Windows.Forms.TextBox()
        Me.lblFinalpercent = New System.Windows.Forms.Label()
        Me.lblFinalBalanceStatus = New System.Windows.Forms.Label()
        Me.lblFinalBalance = New System.Windows.Forms.Label()
        Me.Label110 = New System.Windows.Forms.Label()
        Me.Grouper1 = New CodeVendor.Controls.Grouper()
        Me.Label99 = New System.Windows.Forms.Label()
        Me.txtDetOffWt = New System.Windows.Forms.TextBox()
        Me.lblKFC = New System.Windows.Forms.Label()
        Me.btnAttachImage = New System.Windows.Forms.Button()
        Me.Label137 = New System.Windows.Forms.Label()
        Me.Label129 = New System.Windows.Forms.Label()
        Me.Label130 = New System.Windows.Forms.Label()
        Me.Label131 = New System.Windows.Forms.Label()
        Me.Label132 = New System.Windows.Forms.Label()
        Me.Label128 = New System.Windows.Forms.Label()
        Me.Label123 = New System.Windows.Forms.Label()
        Me.Label120 = New System.Windows.Forms.Label()
        Me.Label122 = New System.Windows.Forms.Label()
        Me.Label109 = New System.Windows.Forms.Label()
        Me.Label119 = New System.Windows.Forms.Label()
        Me.chkAppSales = New System.Windows.Forms.CheckBox()
        Me.Label198 = New System.Windows.Forms.Label()
        Me.Label166 = New System.Windows.Forms.Label()
        Me.Label185 = New System.Windows.Forms.Label()
        Me.Label197 = New System.Windows.Forms.Label()
        Me.Label165 = New System.Windows.Forms.Label()
        Me.Label184 = New System.Windows.Forms.Label()
        Me.Label196 = New System.Windows.Forms.Label()
        Me.Label164 = New System.Windows.Forms.Label()
        Me.Label183 = New System.Windows.Forms.Label()
        Me.Label163 = New System.Windows.Forms.Label()
        Me.Label162 = New System.Windows.Forms.Label()
        Me.Label161 = New System.Windows.Forms.Label()
        Me.Label182 = New System.Windows.Forms.Label()
        Me.Label193 = New System.Windows.Forms.Label()
        Me.Label158 = New System.Windows.Forms.Label()
        Me.Label181 = New System.Windows.Forms.Label()
        Me.Label192 = New System.Windows.Forms.Label()
        Me.Label157 = New System.Windows.Forms.Label()
        Me.Label180 = New System.Windows.Forms.Label()
        Me.Label190 = New System.Windows.Forms.Label()
        Me.Label156 = New System.Windows.Forms.Label()
        Me.Label179 = New System.Windows.Forms.Label()
        Me.Label189 = New System.Windows.Forms.Label()
        Me.Label155 = New System.Windows.Forms.Label()
        Me.Label178 = New System.Windows.Forms.Label()
        Me.Label154 = New System.Windows.Forms.Label()
        Me.Label175 = New System.Windows.Forms.Label()
        Me.Label153 = New System.Windows.Forms.Label()
        Me.Label152 = New System.Windows.Forms.Label()
        Me.Label151 = New System.Windows.Forms.Label()
        Me.Label150 = New System.Windows.Forms.Label()
        Me.Label149 = New System.Windows.Forms.Label()
        Me.Label48 = New System.Windows.Forms.Label()
        Me.Label186 = New System.Windows.Forms.Label()
        Me.Label187 = New System.Windows.Forms.Label()
        Me.chkPartlySales = New System.Windows.Forms.CheckBox()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.Label204 = New System.Windows.Forms.Label()
        Me.Label203 = New System.Windows.Forms.Label()
        Me.Label202 = New System.Windows.Forms.Label()
        Me.Label201 = New System.Windows.Forms.Label()
        Me.lblBookedItem = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.Label188 = New System.Windows.Forms.Label()
        Me.grpGeneral = New CodeVendor.Controls.Grouper()
        Me.lblHallmarkNo = New System.Windows.Forms.Label()
        Me.txtDetVAMin = New System.Windows.Forms.TextBox()
        Me.txtDetVAMax = New System.Windows.Forms.TextBox()
        Me.Label141 = New System.Windows.Forms.Label()
        Me.txtDetTagGrsNet = New System.Windows.Forms.TextBox()
        Me.Label139 = New System.Windows.Forms.Label()
        Me.txtDetDesigner = New System.Windows.Forms.TextBox()
        Me.Label76 = New System.Windows.Forms.Label()
        Me.txtDifftot = New System.Windows.Forms.TextBox()
        Me.txtSASurCharge = New System.Windows.Forms.TextBox()
        Me.Label75 = New System.Windows.Forms.Label()
        Me.txtDetRateId = New System.Windows.Forms.TextBox()
        Me.Label72 = New System.Windows.Forms.Label()
        Me.txtDetItemType = New System.Windows.Forms.TextBox()
        Me.txtDetWastagePer = New System.Windows.Forms.TextBox()
        Me.txtDetGrsNet = New System.Windows.Forms.TextBox()
        Me.Label127 = New System.Windows.Forms.Label()
        Me.txtDetDiffGrsNet = New System.Windows.Forms.TextBox()
        Me.txtDetTableCode = New System.Windows.Forms.TextBox()
        Me.txtDetDiffDia = New System.Windows.Forms.TextBox()
        Me.txtDetCounter = New System.Windows.Forms.TextBox()
        Me.txtDetSubItem = New System.Windows.Forms.TextBox()
        Me.txtDetItem = New System.Windows.Forms.TextBox()
        Me.txtDetValueAdded = New System.Windows.Forms.TextBox()
        Me.Label64 = New System.Windows.Forms.Label()
        Me.txtDetDiscount = New System.Windows.Forms.TextBox()
        Me.txtDetCalcType = New System.Windows.Forms.TextBox()
        Me.txtDetMiscAmt = New System.Windows.Forms.TextBox()
        Me.txtDetMcPerGrm = New System.Windows.Forms.TextBox()
        Me.txtDetLessWt = New System.Windows.Forms.TextBox()
        Me.txtDetStockType = New System.Windows.Forms.TextBox()
        Me.Label71 = New System.Windows.Forms.Label()
        Me.Label133 = New System.Windows.Forms.Label()
        Me.Label134 = New System.Windows.Forms.Label()
        Me.lblDetVat = New System.Windows.Forms.Label()
        Me.Label121 = New System.Windows.Forms.Label()
        Me.Label125 = New System.Windows.Forms.Label()
        Me.Label118 = New System.Windows.Forms.Label()
        Me.Label117 = New System.Windows.Forms.Label()
        Me.Label116 = New System.Windows.Forms.Label()
        Me.Label115 = New System.Windows.Forms.Label()
        Me.Label114 = New System.Windows.Forms.Label()
        Me.Label113 = New System.Windows.Forms.Label()
        Me.Label112 = New System.Windows.Forms.Label()
        Me.Label111 = New System.Windows.Forms.Label()
        Me.Label61 = New System.Windows.Forms.Label()
        Me.Label50 = New System.Windows.Forms.Label()
        Me.tabHideDet = New System.Windows.Forms.TabPage()
        Me.txtPurAlloy = New System.Windows.Forms.TextBox()
        Me.grpStudedDetail = New System.Windows.Forms.GroupBox()
        Me.Label102 = New System.Windows.Forms.Label()
        Me.lblDetDiaPcs = New System.Windows.Forms.Label()
        Me.lblDetStnWt = New System.Windows.Forms.Label()
        Me.lblDetStnPcs = New System.Windows.Forms.Label()
        Me.lblDetDiaWt = New System.Windows.Forms.Label()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.pnlShortCut = New System.Windows.Forms.Panel()
        Me.Label135 = New System.Windows.Forms.Label()
        Me.txtShortCut = New System.Windows.Forms.TextBox()
        Me.picTagImage = New System.Windows.Forms.PictureBox()
        Me.pnlFinalPURAmount_OWN = New System.Windows.Forms.Panel()
        Me.txtFinalPURAmount_AMT = New System.Windows.Forms.TextBox()
        Me.Label97 = New System.Windows.Forms.Label()
        Me.Label103 = New System.Windows.Forms.Label()
        Me.Label104 = New System.Windows.Forms.Label()
        Me.Label105 = New System.Windows.Forms.Label()
        Me.Label106 = New System.Windows.Forms.Label()
        Me.Label107 = New System.Windows.Forms.Label()
        Me.pnlPuExtaDetails = New System.Windows.Forms.Panel()
        Me.Label124 = New System.Windows.Forms.Label()
        Me.txtPuDiscount_AMT = New System.Windows.Forms.TextBox()
        Me.tabReceiptWeightAdvance = New System.Windows.Forms.TabPage()
        Me.tabReceiptReserve = New System.Windows.Forms.TabPage()
        Me.tabOrderDetail = New System.Windows.Forms.TabPage()
        Me.grpOrderDetail = New CodeVendor.Controls.Grouper()
        Me.txtOrdCGST_AMT = New System.Windows.Forms.TextBox()
        Me.txtOrdSGST_AMT = New System.Windows.Forms.TextBox()
        Me.txtOrdAdjdisc = New System.Windows.Forms.TextBox()
        Me.txtOrdDisc = New System.Windows.Forms.TextBox()
        Me.txtExMcvat = New System.Windows.Forms.TextBox()
        Me.Label45 = New System.Windows.Forms.Label()
        Me.txtOrdVat_AMT = New System.Windows.Forms.TextBox()
        Me.txtOrdRate_AMT = New System.Windows.Forms.TextBox()
        Me.txtOrdAmount_AMT = New System.Windows.Forms.TextBox()
        Me.txtOrdGrossAmount_AMT = New System.Windows.Forms.TextBox()
        Me.txtOrdBalanceWeight_WET = New System.Windows.Forms.TextBox()
        Me.txtOrdAdvanceWeight_WET = New System.Windows.Forms.TextBox()
        Me.txtOrdTotalWeight_WET = New System.Windows.Forms.TextBox()
        Me.lblOrdVat = New System.Windows.Forms.Label()
        Me.Label98 = New System.Windows.Forms.Label()
        Me.Label100 = New System.Windows.Forms.Label()
        Me.Label79 = New System.Windows.Forms.Label()
        Me.Label78 = New System.Windows.Forms.Label()
        Me.Label53 = New System.Windows.Forms.Label()
        Me.Label52 = New System.Windows.Forms.Label()
        Me.tabAdvanceWeightAdjCalc = New System.Windows.Forms.TabPage()
        Me.grpAdvanceCalc = New CodeVendor.Controls.Grouper()
        Me.gridAdvanceAdjCalc = New System.Windows.Forms.DataGridView()
        Me.tabWholeSaleDetails = New System.Windows.Forms.TabPage()
        Me.grpWholeSaleDetails = New CodeVendor.Controls.Grouper()
        Me.dgvWholeSaleDetail = New System.Windows.Forms.DataGridView()
        Me.tabSAExtraDetail = New System.Windows.Forms.TabPage()
        Me.txtAdjSrCredit_AMT = New System.Windows.Forms.TextBox()
        Me.lblHelpText = New System.Windows.Forms.Label()
        Me.grpAdj = New CodeVendor.Controls.Grouper()
        Me.lblDiscper = New System.Windows.Forms.Label()
        Me.txtAdjDiscount_PER = New System.Windows.Forms.TextBox()
        Me.txtAdjCash_AMT = New System.Windows.Forms.TextBox()
        Me.txtAdjGiftVoucher_AMT = New System.Windows.Forms.TextBox()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.Label41 = New System.Windows.Forms.Label()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.Label60 = New System.Windows.Forms.Label()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.Label59 = New System.Windows.Forms.Label()
        Me.txtAdjCredit_AMT = New System.Windows.Forms.TextBox()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.txtAdjDiscount_AMT = New System.Windows.Forms.TextBox()
        Me.txtAdjAdvance_AMT = New System.Windows.Forms.TextBox()
        Me.Label57 = New System.Windows.Forms.Label()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.lblHc = New System.Windows.Forms.Label()
        Me.txtAdjChitCard_AMT = New System.Windows.Forms.TextBox()
        Me.Label54 = New System.Windows.Forms.Label()
        Me.txtAdjHandlingCharge_AMT = New System.Windows.Forms.TextBox()
        Me.txtAdjRoundoff_AMT = New System.Windows.Forms.TextBox()
        Me.Label46 = New System.Windows.Forms.Label()
        Me.Label148 = New System.Windows.Forms.Label()
        Me.Label49 = New System.Windows.Forms.Label()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.txtAdjReceive_AMT = New System.Windows.Forms.TextBox()
        Me.Label47 = New System.Windows.Forms.Label()
        Me.txtAdjCheque_AMT = New System.Windows.Forms.TextBox()
        Me.txtAdjCreditCard_AMT = New System.Windows.Forms.TextBox()
        Me.grpOptions = New CodeVendor.Controls.Grouper()
        Me.picGiftVoucher = New System.Windows.Forms.PictureBox()
        Me.picRecPay = New System.Windows.Forms.PictureBox()
        Me.picOrderRepair = New System.Windows.Forms.PictureBox()
        Me.picMiscIssue = New System.Windows.Forms.PictureBox()
        Me.picPurchase = New System.Windows.Forms.PictureBox()
        Me.picReturn = New System.Windows.Forms.PictureBox()
        Me.picSales = New System.Windows.Forms.PictureBox()
        Me.picAppIssRec = New System.Windows.Forms.PictureBox()
        Me.btnRecPay_OWN = New System.Windows.Forms.Button()
        Me.cMenuRecPay = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tStripReceipt = New System.Windows.Forms.ToolStripMenuItem()
        Me.tStripPayment = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnOrderRepair_OWN = New System.Windows.Forms.Button()
        Me.cMenuOrderRepair = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tStripOrder = New System.Windows.Forms.ToolStripMenuItem()
        Me.tStripRepair = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnApproval_OWN = New System.Windows.Forms.Button()
        Me.cMenuApproval = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tStripApprovalIssue = New System.Windows.Forms.ToolStripMenuItem()
        Me.tStripApprovalReceipt = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnGiftVoucher_OWN = New System.Windows.Forms.Button()
        Me.btnSales_OWN = New System.Windows.Forms.Button()
        Me.btnSalesReturn_OWN = New System.Windows.Forms.Button()
        Me.btnPurchase_OWN = New System.Windows.Forms.Button()
        Me.btnMiscIssue_OWN = New System.Windows.Forms.Button()
        Me.grpHeader = New CodeVendor.Controls.Grouper()
        Me.cmenuTemplate = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Style1ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Style2ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Style3ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Style4ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Style5ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Style6ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnCalc = New System.Windows.Forms.Button()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.Label89 = New System.Windows.Forms.Label()
        Me.Label90 = New System.Windows.Forms.Label()
        Me.Label91 = New System.Windows.Forms.Label()
        Me.Label88 = New System.Windows.Forms.Label()
        Me.Label87 = New System.Windows.Forms.Label()
        Me.Label86 = New System.Windows.Forms.Label()
        Me.lblCompanyName = New System.Windows.Forms.Label()
        Me.Label84 = New System.Windows.Forms.Label()
        Me.lblSystemId = New System.Windows.Forms.Label()
        Me.lblNodeId = New System.Windows.Forms.Label()
        Me.lblUserName = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CompanySelectionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Company1ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Company2ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Company3ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Company4ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Company5ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Company6ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Company7ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Company8ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Company9ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Company0ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem6 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem7 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem8 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem9 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem10 = New System.Windows.Forms.ToolStripMenuItem()
        Me.SalesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SalesReturnToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PurchaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PartlySalesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AdvanceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CreditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CreditCardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChitCardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChequeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HandlingChargeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DiscountToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CashToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tStripGiftVouhcer = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmenuBookingItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tStripMiscIssue = New System.Windows.Forms.ToolStripMenuItem()
        Me.tStripGiftVoucher = New System.Windows.Forms.ToolStripMenuItem()
        Me.FinalDiscountToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FinalPurDiscountToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaleRateChangeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WastageMcPerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StoneDetailsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MiscDetailsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MultiMetalDetailsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SalesB4DiscountToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddressToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToBeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReceiptToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PaymentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AppIssueToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AppReceiptToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OrderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RepairToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EstCallToolStrip = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripDupbill = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSchemeOffer = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripRateView = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripBillno = New System.Windows.Forms.ToolStripMenuItem()
        Me.Wt2WtAdjustToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ComplementToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExcelDownloadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tStripSchemeOffer = New System.Windows.Forms.ToolStripMenuItem()
        Me.CalcStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetItemToolStripMenuItem11 = New System.Windows.Forms.ToolStripMenuItem()
        Me.DisableSetItemStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TagCompMoveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tStripSGiftVoucher = New System.Windows.Forms.ToolStripMenuItem()
        Me.BillTypeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PrivilegeDiscountToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PruchaseOrderRate = New System.Windows.Forms.ToolStripMenuItem()
        Me.KFC = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripRowFinalAmt = New System.Windows.Forms.ToolStripMenuItem()
        Me.SecondSalesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TrfNoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripHallmarkDetails = New System.Windows.Forms.ToolStripMenuItem()
        Me.TStripShipingAddress = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.clockTimer = New System.Windows.Forms.Timer(Me.components)
        Me.mTimer = New System.Windows.Forms.Timer(Me.components)
        Me.lblTcsCalcAmt = New System.Windows.Forms.Label()
        Me.lblTcsCalcPrecent = New System.Windows.Forms.Label()
        Me.lblSNo = New System.Windows.Forms.Label()
        Me.lblPNo = New System.Windows.Forms.Label()
        Me.tabMain.SuspendLayout()
        Me.tabSaSrPu.SuspendLayout()
        Me.grpSaSr.SuspendLayout()
        CType(Me.gridSASRTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridSASR, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpPu.SuspendLayout()
        CType(Me.gridPur, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridPurTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabReceipt.SuspendLayout()
        Me.grpReceipt.SuspendLayout()
        Me.cmenuReceiptGrid.SuspendLayout()
        CType(Me.gridReceipt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridReceiptTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabPayment.SuspendLayout()
        Me.grpPayment.SuspendLayout()
        CType(Me.gridPayment, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridPaymentTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabGiftVoucherMain.SuspendLayout()
        Me.grpGiftVoucherMain.SuspendLayout()
        CType(Me.gridMGiftVoucherTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridMGiftVoucher, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpRecReservedItem.SuspendLayout()
        CType(Me.gridReceiptReserved, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpReceiptWeightAdvance.SuspendLayout()
        Me.tabOtherOptions.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.grpGeneral1.SuspendLayout()
        Me.pnlFinalBalance.SuspendLayout()
        Me.Grouper1.SuspendLayout()
        Me.grpGeneral.SuspendLayout()
        Me.tabHideDet.SuspendLayout()
        Me.grpStudedDetail.SuspendLayout()
        Me.pnlShortCut.SuspendLayout()
        CType(Me.picTagImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlFinalPURAmount_OWN.SuspendLayout()
        Me.pnlPuExtaDetails.SuspendLayout()
        Me.tabReceiptWeightAdvance.SuspendLayout()
        Me.tabReceiptReserve.SuspendLayout()
        Me.tabOrderDetail.SuspendLayout()
        Me.grpOrderDetail.SuspendLayout()
        Me.tabAdvanceWeightAdjCalc.SuspendLayout()
        Me.grpAdvanceCalc.SuspendLayout()
        CType(Me.gridAdvanceAdjCalc, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabWholeSaleDetails.SuspendLayout()
        Me.grpWholeSaleDetails.SuspendLayout()
        CType(Me.dgvWholeSaleDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpAdj.SuspendLayout()
        Me.grpOptions.SuspendLayout()
        CType(Me.picGiftVoucher, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picRecPay, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picOrderRepair, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picMiscIssue, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picPurchase, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picReturn, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picSales, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picAppIssRec, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cMenuRecPay.SuspendLayout()
        Me.cMenuOrderRepair.SuspendLayout()
        Me.cMenuApproval.SuspendLayout()
        Me.grpHeader.SuspendLayout()
        Me.cmenuTemplate.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtSAItemId
        '
        Me.txtSAItemId.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSAItemId.Location = New System.Drawing.Point(61, 31)
        Me.txtSAItemId.MaxLength = 17
        Me.txtSAItemId.Name = "txtSAItemId"
        Me.txtSAItemId.Size = New System.Drawing.Size(42, 26)
        Me.txtSAItemId.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(61, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(42, 15)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Id"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtSAPcs_NUM
        '
        Me.txtSAPcs_NUM.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSAPcs_NUM.Location = New System.Drawing.Point(185, 31)
        Me.txtSAPcs_NUM.MaxLength = 5
        Me.txtSAPcs_NUM.Name = "txtSAPcs_NUM"
        Me.txtSAPcs_NUM.Size = New System.Drawing.Size(43, 26)
        Me.txtSAPcs_NUM.TabIndex = 9
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(185, 14)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(43, 15)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Pcs"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtSANetWt_WET
        '
        Me.txtSANetWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSANetWt_WET.Location = New System.Drawing.Point(305, 31)
        Me.txtSANetWt_WET.MaxLength = 10
        Me.txtSANetWt_WET.Name = "txtSANetWt_WET"
        Me.txtSANetWt_WET.Size = New System.Drawing.Size(75, 26)
        Me.txtSANetWt_WET.TabIndex = 13
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(305, 14)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(75, 15)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "NetWt"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtSAGrsWt_WET
        '
        Me.txtSAGrsWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSAGrsWt_WET.Location = New System.Drawing.Point(229, 31)
        Me.txtSAGrsWt_WET.MaxLength = 10
        Me.txtSAGrsWt_WET.Name = "txtSAGrsWt_WET"
        Me.txtSAGrsWt_WET.Size = New System.Drawing.Size(75, 26)
        Me.txtSAGrsWt_WET.TabIndex = 11
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(229, 14)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(75, 15)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "Grswt"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtSARate_AMT
        '
        Me.txtSARate_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSARate_AMT.Location = New System.Drawing.Point(381, 31)
        Me.txtSARate_AMT.MaxLength = 10
        Me.txtSARate_AMT.Name = "txtSARate_AMT"
        Me.txtSARate_AMT.Size = New System.Drawing.Size(83, 26)
        Me.txtSARate_AMT.TabIndex = 15
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(381, 14)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(83, 15)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "Rate"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtSAWastage_WET
        '
        Me.txtSAWastage_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSAWastage_WET.Location = New System.Drawing.Point(465, 31)
        Me.txtSAWastage_WET.MaxLength = 10
        Me.txtSAWastage_WET.Name = "txtSAWastage_WET"
        Me.txtSAWastage_WET.Size = New System.Drawing.Size(66, 26)
        Me.txtSAWastage_WET.TabIndex = 17
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(465, 14)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(66, 15)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "Wastage"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtSAMc_AMT
        '
        Me.txtSAMc_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSAMc_AMT.Location = New System.Drawing.Point(532, 31)
        Me.txtSAMc_AMT.MaxLength = 12
        Me.txtSAMc_AMT.Name = "txtSAMc_AMT"
        Me.txtSAMc_AMT.Size = New System.Drawing.Size(73, 26)
        Me.txtSAMc_AMT.TabIndex = 19
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(532, 14)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(73, 15)
        Me.Label9.TabIndex = 18
        Me.Label9.Text = "Mc"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtSAAmount_AMT
        '
        Me.txtSAAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSAAmount_AMT.Location = New System.Drawing.Point(849, 31)
        Me.txtSAAmount_AMT.MaxLength = 12
        Me.txtSAAmount_AMT.Name = "txtSAAmount_AMT"
        Me.txtSAAmount_AMT.Size = New System.Drawing.Size(97, 26)
        Me.txtSAAmount_AMT.TabIndex = 27
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.Color.Transparent
        Me.Label12.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(849, 14)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(97, 15)
        Me.Label12.TabIndex = 26
        Me.Label12.Text = "Amount"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtSAVat_AMT
        '
        Me.txtSAVat_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSAVat_AMT.Location = New System.Drawing.Point(775, 31)
        Me.txtSAVat_AMT.MaxLength = 12
        Me.txtSAVat_AMT.Name = "txtSAVat_AMT"
        Me.txtSAVat_AMT.Size = New System.Drawing.Size(73, 26)
        Me.txtSAVat_AMT.TabIndex = 25
        '
        'lblSaVat
        '
        Me.lblSaVat.BackColor = System.Drawing.Color.Transparent
        Me.lblSaVat.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSaVat.Location = New System.Drawing.Point(775, 14)
        Me.lblSaVat.Name = "lblSaVat"
        Me.lblSaVat.Size = New System.Drawing.Size(73, 15)
        Me.lblSaVat.TabIndex = 24
        Me.lblSaVat.Text = "Vat"
        Me.lblSaVat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(606, 14)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(75, 15)
        Me.Label10.TabIndex = 20
        Me.Label10.Text = "StoneAmt"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(58, 12)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(177, 17)
        Me.Label11.TabIndex = 2
        Me.Label11.Text = "Category"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPUPcs_NUM
        '
        Me.txtPUPcs_NUM.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUPcs_NUM.Location = New System.Drawing.Point(312, 31)
        Me.txtPUPcs_NUM.MaxLength = 5
        Me.txtPUPcs_NUM.Name = "txtPUPcs_NUM"
        Me.txtPUPcs_NUM.Size = New System.Drawing.Size(43, 26)
        Me.txtPUPcs_NUM.TabIndex = 7
        '
        'Label14
        '
        Me.Label14.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(312, 12)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(43, 17)
        Me.Label14.TabIndex = 6
        Me.Label14.Text = "Pcs"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPUGrsWt_WET
        '
        Me.txtPUGrsWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUGrsWt_WET.Location = New System.Drawing.Point(356, 31)
        Me.txtPUGrsWt_WET.MaxLength = 10
        Me.txtPUGrsWt_WET.Name = "txtPUGrsWt_WET"
        Me.txtPUGrsWt_WET.Size = New System.Drawing.Size(75, 26)
        Me.txtPUGrsWt_WET.TabIndex = 9
        '
        'Label16
        '
        Me.Label16.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(356, 12)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(75, 17)
        Me.Label16.TabIndex = 8
        Me.Label16.Text = "Grswt"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPUDustWt_WET
        '
        Me.txtPUDustWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUDustWt_WET.Location = New System.Drawing.Point(432, 31)
        Me.txtPUDustWt_WET.MaxLength = 10
        Me.txtPUDustWt_WET.Name = "txtPUDustWt_WET"
        Me.txtPUDustWt_WET.Size = New System.Drawing.Size(66, 26)
        Me.txtPUDustWt_WET.TabIndex = 11
        '
        'Label17
        '
        Me.Label17.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(432, 12)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(66, 17)
        Me.Label17.TabIndex = 10
        Me.Label17.Text = "DustWt"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPUWastage_WET
        '
        Me.txtPUWastage_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUWastage_WET.Location = New System.Drawing.Point(548, 31)
        Me.txtPUWastage_WET.MaxLength = 10
        Me.txtPUWastage_WET.Name = "txtPUWastage_WET"
        Me.txtPUWastage_WET.Size = New System.Drawing.Size(66, 26)
        Me.txtPUWastage_WET.TabIndex = 15
        '
        'Label18
        '
        Me.Label18.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(548, 12)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(66, 17)
        Me.Label18.TabIndex = 14
        Me.Label18.Text = "Wastage"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPUStoneWt_WET
        '
        Me.txtPUStoneWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUStoneWt_WET.Location = New System.Drawing.Point(284, 115)
        Me.txtPUStoneWt_WET.MaxLength = 10
        Me.txtPUStoneWt_WET.Name = "txtPUStoneWt_WET"
        Me.txtPUStoneWt_WET.Size = New System.Drawing.Size(75, 26)
        Me.txtPUStoneWt_WET.TabIndex = 11
        Me.txtPUStoneWt_WET.Text = "999.999"
        '
        'Label19
        '
        Me.Label19.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(236, 12)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(75, 17)
        Me.Label19.TabIndex = 4
        Me.Label19.Text = "Purity"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPUNetWt_WET
        '
        Me.txtPUNetWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUNetWt_WET.Location = New System.Drawing.Point(615, 31)
        Me.txtPUNetWt_WET.MaxLength = 10
        Me.txtPUNetWt_WET.Name = "txtPUNetWt_WET"
        Me.txtPUNetWt_WET.Size = New System.Drawing.Size(75, 26)
        Me.txtPUNetWt_WET.TabIndex = 17
        '
        'Label20
        '
        Me.Label20.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(615, 12)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(75, 17)
        Me.Label20.TabIndex = 16
        Me.Label20.Text = "Net WT"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPURate_AMT
        '
        Me.txtPURate_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPURate_AMT.Location = New System.Drawing.Point(691, 31)
        Me.txtPURate_AMT.MaxLength = 10
        Me.txtPURate_AMT.Name = "txtPURate_AMT"
        Me.txtPURate_AMT.Size = New System.Drawing.Size(83, 26)
        Me.txtPURate_AMT.TabIndex = 19
        '
        'Label21
        '
        Me.Label21.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(691, 12)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(83, 17)
        Me.Label21.TabIndex = 18
        Me.Label21.Text = "Rate"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPUAmount_AMT
        '
        Me.txtPUAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUAmount_AMT.Location = New System.Drawing.Point(849, 31)
        Me.txtPUAmount_AMT.MaxLength = 12
        Me.txtPUAmount_AMT.Name = "txtPUAmount_AMT"
        Me.txtPUAmount_AMT.Size = New System.Drawing.Size(97, 26)
        Me.txtPUAmount_AMT.TabIndex = 23
        '
        'Label22
        '
        Me.Label22.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.Location = New System.Drawing.Point(849, 12)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(97, 17)
        Me.Label22.TabIndex = 22
        Me.Label22.Text = "Amount"
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPUVat_AMT
        '
        Me.txtPUVat_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUVat_AMT.Location = New System.Drawing.Point(775, 31)
        Me.txtPUVat_AMT.MaxLength = 12
        Me.txtPUVat_AMT.Name = "txtPUVat_AMT"
        Me.txtPUVat_AMT.Size = New System.Drawing.Size(73, 26)
        Me.txtPUVat_AMT.TabIndex = 21
        '
        'lblPUVat
        '
        Me.lblPUVat.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPUVat.Location = New System.Drawing.Point(775, 12)
        Me.lblPUVat.Name = "lblPUVat"
        Me.lblPUVat.Size = New System.Drawing.Size(73, 17)
        Me.lblPUVat.TabIndex = 20
        Me.lblPUVat.Text = "Vat"
        Me.lblPUVat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtSAEstNo_NUM
        '
        Me.txtSAEstNo_NUM.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSAEstNo_NUM.Location = New System.Drawing.Point(11, 31)
        Me.txtSAEstNo_NUM.MaxLength = 8
        Me.txtSAEstNo_NUM.Name = "txtSAEstNo_NUM"
        Me.txtSAEstNo_NUM.Size = New System.Drawing.Size(49, 26)
        Me.txtSAEstNo_NUM.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(11, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Est No"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtSATagNo
        '
        Me.txtSATagNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSATagNo.Location = New System.Drawing.Point(104, 31)
        Me.txtSATagNo.MaxLength = 20
        Me.txtSATagNo.Name = "txtSATagNo"
        Me.txtSATagNo.ShortcutsEnabled = False
        Me.txtSATagNo.Size = New System.Drawing.Size(80, 26)
        Me.txtSATagNo.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(104, 14)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(80, 15)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Tag No"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSilverRate
        '
        Me.lblSilverRate.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSilverRate.Location = New System.Drawing.Point(898, 35)
        Me.lblSilverRate.Name = "lblSilverRate"
        Me.lblSilverRate.Size = New System.Drawing.Size(85, 16)
        Me.lblSilverRate.TabIndex = 4
        Me.lblSilverRate.Text = "200.00"
        Me.lblSilverRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label32.Location = New System.Drawing.Point(762, 35)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(135, 20)
        Me.Label32.TabIndex = 4
        Me.Label32.Text = "SILVER RATE"
        Me.ToolTip1.SetToolTip(Me.Label32, "CTRL+ALT+R - Rate View")
        '
        'lblGoldRate
        '
        Me.lblGoldRate.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGoldRate.Location = New System.Drawing.Point(898, 17)
        Me.lblGoldRate.Name = "lblGoldRate"
        Me.lblGoldRate.Size = New System.Drawing.Size(85, 16)
        Me.lblGoldRate.TabIndex = 4
        Me.lblGoldRate.Text = "1520.00"
        Me.lblGoldRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.lblGoldRate, "CTRL+ALT+R - Rate View")
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label31.Location = New System.Drawing.Point(762, 17)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(118, 20)
        Me.Label31.TabIndex = 4
        Me.Label31.Text = "GOLD RATE"
        Me.ToolTip1.SetToolTip(Me.Label31, "CTRL+ALT+R - Rate View")
        '
        'lblCashCounter
        '
        Me.lblCashCounter.AutoSize = True
        Me.lblCashCounter.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCashCounter.Location = New System.Drawing.Point(187, 35)
        Me.lblCashCounter.Name = "lblCashCounter"
        Me.lblCashCounter.Size = New System.Drawing.Size(135, 20)
        Me.lblCashCounter.TabIndex = 4
        Me.lblCashCounter.Text = "FIRST FLOOR"
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label30.Location = New System.Drawing.Point(23, 35)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(157, 20)
        Me.Label30.TabIndex = 4
        Me.Label30.Text = "CASH COUNTER"
        '
        'lblBillDate
        '
        Me.lblBillDate.AutoSize = True
        Me.lblBillDate.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBillDate.Location = New System.Drawing.Point(187, 17)
        Me.lblBillDate.Name = "lblBillDate"
        Me.lblBillDate.Size = New System.Drawing.Size(129, 20)
        Me.lblBillDate.TabIndex = 4
        Me.lblBillDate.Text = "08/03/2009"
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.Location = New System.Drawing.Point(23, 17)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(110, 20)
        Me.Label29.TabIndex = 4
        Me.Label29.Text = "BILL DATE"
        '
        'tabMain
        '
        Me.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tabMain.Controls.Add(Me.tabSaSrPu)
        Me.tabMain.Controls.Add(Me.tabReceipt)
        Me.tabMain.Controls.Add(Me.tabPayment)
        Me.tabMain.Controls.Add(Me.tabGiftVoucherMain)
        Me.tabMain.ItemSize = New System.Drawing.Size(1, 10)
        Me.tabMain.Location = New System.Drawing.Point(-9, 63)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(1031, 347)
        Me.tabMain.TabIndex = 2
        '
        'tabSaSrPu
        '
        Me.tabSaSrPu.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.tabSaSrPu.Controls.Add(Me.grpSaSr)
        Me.tabSaSrPu.Controls.Add(Me.grpPu)
        Me.tabSaSrPu.Location = New System.Drawing.Point(4, 14)
        Me.tabSaSrPu.Name = "tabSaSrPu"
        Me.tabSaSrPu.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSaSrPu.Size = New System.Drawing.Size(1023, 329)
        Me.tabSaSrPu.TabIndex = 0
        Me.tabSaSrPu.Text = "Sales,Return,Purchase"
        Me.tabSaSrPu.UseVisualStyleBackColor = True
        '
        'grpSaSr
        '
        Me.grpSaSr.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpSaSr.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpSaSr.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpSaSr.BorderColor = System.Drawing.Color.Transparent
        Me.grpSaSr.BorderThickness = 1.0!
        Me.grpSaSr.Controls.Add(Me.txtSARowIndex)
        Me.grpSaSr.Controls.Add(Me.txtSAEmpId_NUM)
        Me.grpSaSr.Controls.Add(Me.txtSAGrossAmount_AMT)
        Me.grpSaSr.Controls.Add(Me.gridSASRTotal)
        Me.grpSaSr.Controls.Add(Me.gridSASR)
        Me.grpSaSr.Controls.Add(Me.Label1)
        Me.grpSaSr.Controls.Add(Me.txtSAVat_AMT)
        Me.grpSaSr.Controls.Add(Me.txtSAStoneAmount_AMT)
        Me.grpSaSr.Controls.Add(Me.txtSATagNo)
        Me.grpSaSr.Controls.Add(Me.Label3)
        Me.grpSaSr.Controls.Add(Me.Label6)
        Me.grpSaSr.Controls.Add(Me.txtSAMc_AMT)
        Me.grpSaSr.Controls.Add(Me.txtSAEstNo_NUM)
        Me.grpSaSr.Controls.Add(Me.txtSAPcs_NUM)
        Me.grpSaSr.Controls.Add(Me.Label9)
        Me.grpSaSr.Controls.Add(Me.txtSAAmount_AMT)
        Me.grpSaSr.Controls.Add(Me.Label8)
        Me.grpSaSr.Controls.Add(Me.Label2)
        Me.grpSaSr.Controls.Add(Me.lblSaVat)
        Me.grpSaSr.Controls.Add(Me.Label62)
        Me.grpSaSr.Controls.Add(Me.Label4)
        Me.grpSaSr.Controls.Add(Me.txtSAGrsWt_WET)
        Me.grpSaSr.Controls.Add(Me.txtSARate_AMT)
        Me.grpSaSr.Controls.Add(Me.txtSAWastage_WET)
        Me.grpSaSr.Controls.Add(Me.txtSANetWt_WET)
        Me.grpSaSr.Controls.Add(Me.Label12)
        Me.grpSaSr.Controls.Add(Me.Label55)
        Me.grpSaSr.Controls.Add(Me.Label10)
        Me.grpSaSr.Controls.Add(Me.Label7)
        Me.grpSaSr.Controls.Add(Me.Label5)
        Me.grpSaSr.Controls.Add(Me.txtSAItemId)
        Me.grpSaSr.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpSaSr.GroupImage = Nothing
        Me.grpSaSr.GroupTitle = ""
        Me.grpSaSr.Location = New System.Drawing.Point(10, -10)
        Me.grpSaSr.Name = "grpSaSr"
        Me.grpSaSr.Padding = New System.Windows.Forms.Padding(20)
        Me.grpSaSr.PaintGroupBox = False
        Me.grpSaSr.RoundCorners = 10
        Me.grpSaSr.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpSaSr.ShadowControl = False
        Me.grpSaSr.ShadowThickness = 3
        Me.grpSaSr.Size = New System.Drawing.Size(1012, 189)
        Me.grpSaSr.TabIndex = 0
        '
        'txtSARowIndex
        '
        Me.txtSARowIndex.Location = New System.Drawing.Point(996, 26)
        Me.txtSARowIndex.Name = "txtSARowIndex"
        Me.txtSARowIndex.Size = New System.Drawing.Size(8, 24)
        Me.txtSARowIndex.TabIndex = 32
        Me.txtSARowIndex.Visible = False
        '
        'txtSAEmpId_NUM
        '
        Me.txtSAEmpId_NUM.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSAEmpId_NUM.Location = New System.Drawing.Point(947, 31)
        Me.txtSAEmpId_NUM.MaxLength = 8
        Me.txtSAEmpId_NUM.Name = "txtSAEmpId_NUM"
        Me.txtSAEmpId_NUM.Size = New System.Drawing.Size(35, 26)
        Me.txtSAEmpId_NUM.TabIndex = 29
        '
        'txtSAGrossAmount_AMT
        '
        Me.txtSAGrossAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSAGrossAmount_AMT.Location = New System.Drawing.Point(682, 31)
        Me.txtSAGrossAmount_AMT.MaxLength = 12
        Me.txtSAGrossAmount_AMT.Name = "txtSAGrossAmount_AMT"
        Me.txtSAGrossAmount_AMT.Size = New System.Drawing.Size(92, 26)
        Me.txtSAGrossAmount_AMT.TabIndex = 23
        '
        'gridSASRTotal
        '
        Me.gridSASRTotal.AllowUserToAddRows = False
        Me.gridSASRTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridSASRTotal.Enabled = False
        Me.gridSASRTotal.Location = New System.Drawing.Point(11, 146)
        Me.gridSASRTotal.Name = "gridSASRTotal"
        Me.gridSASRTotal.ReadOnly = True
        Me.gridSASRTotal.RowHeadersWidth = 51
        Me.gridSASRTotal.Size = New System.Drawing.Size(989, 37)
        Me.gridSASRTotal.TabIndex = 31
        '
        'gridSASR
        '
        Me.gridSASR.AllowUserToAddRows = False
        Me.gridSASR.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.gridSASR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridSASR.Location = New System.Drawing.Point(11, 54)
        Me.gridSASR.Name = "gridSASR"
        Me.gridSASR.ReadOnly = True
        Me.gridSASR.RowHeadersVisible = False
        Me.gridSASR.RowHeadersWidth = 51
        Me.gridSASR.RowTemplate.Height = 18
        Me.gridSASR.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.gridSASR.Size = New System.Drawing.Size(989, 92)
        Me.gridSASR.TabIndex = 30
        '
        'txtSAStoneAmount_AMT
        '
        Me.txtSAStoneAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSAStoneAmount_AMT.Location = New System.Drawing.Point(606, 31)
        Me.txtSAStoneAmount_AMT.Name = "txtSAStoneAmount_AMT"
        Me.txtSAStoneAmount_AMT.Size = New System.Drawing.Size(75, 26)
        Me.txtSAStoneAmount_AMT.TabIndex = 21
        '
        'Label62
        '
        Me.Label62.BackColor = System.Drawing.Color.Transparent
        Me.Label62.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label62.Location = New System.Drawing.Point(947, 14)
        Me.Label62.Name = "Label62"
        Me.Label62.Size = New System.Drawing.Size(35, 14)
        Me.Label62.TabIndex = 28
        Me.Label62.Text = "Emp"
        Me.Label62.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label55
        '
        Me.Label55.BackColor = System.Drawing.Color.Transparent
        Me.Label55.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label55.Location = New System.Drawing.Point(682, 14)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(92, 15)
        Me.Label55.TabIndex = 22
        Me.Label55.Text = "Gross Amt"
        Me.Label55.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpPu
        '
        Me.grpPu.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpPu.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpPu.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpPu.BorderColor = System.Drawing.Color.Transparent
        Me.grpPu.BorderThickness = 1.0!
        Me.grpPu.Controls.Add(Me.txtPUMeltWt_WET)
        Me.grpPu.Controls.Add(Me.txtPUPurity_PER)
        Me.grpPu.Controls.Add(Me.txtPuWastage_PER)
        Me.grpPu.Controls.Add(Me.txtPUEstNo_NUM)
        Me.grpPu.Controls.Add(Me.txtPURowIndex)
        Me.grpPu.Controls.Add(Me.txtPUCategory)
        Me.grpPu.Controls.Add(Me.txtPUEmpId_NUM)
        Me.grpPu.Controls.Add(Me.gridPur)
        Me.grpPu.Controls.Add(Me.Label96)
        Me.grpPu.Controls.Add(Me.Label11)
        Me.grpPu.Controls.Add(Me.txtPUAmount_AMT)
        Me.grpPu.Controls.Add(Me.Label21)
        Me.grpPu.Controls.Add(Me.lblPUVat)
        Me.grpPu.Controls.Add(Me.gridPurTotal)
        Me.grpPu.Controls.Add(Me.txtPUPcs_NUM)
        Me.grpPu.Controls.Add(Me.Label20)
        Me.grpPu.Controls.Add(Me.txtPUGrsWt_WET)
        Me.grpPu.Controls.Add(Me.Label63)
        Me.grpPu.Controls.Add(Me.Label22)
        Me.grpPu.Controls.Add(Me.txtPUVat_AMT)
        Me.grpPu.Controls.Add(Me.Label14)
        Me.grpPu.Controls.Add(Me.txtPURate_AMT)
        Me.grpPu.Controls.Add(Me.txtPUWastage_WET)
        Me.grpPu.Controls.Add(Me.txtPUDustWt_WET)
        Me.grpPu.Controls.Add(Me.Label51)
        Me.grpPu.Controls.Add(Me.Label19)
        Me.grpPu.Controls.Add(Me.Label18)
        Me.grpPu.Controls.Add(Me.Label16)
        Me.grpPu.Controls.Add(Me.txtPUNetWt_WET)
        Me.grpPu.Controls.Add(Me.Label17)
        Me.grpPu.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpPu.GroupImage = Nothing
        Me.grpPu.GroupTitle = ""
        Me.grpPu.Location = New System.Drawing.Point(10, 174)
        Me.grpPu.Name = "grpPu"
        Me.grpPu.Padding = New System.Windows.Forms.Padding(20)
        Me.grpPu.PaintGroupBox = False
        Me.grpPu.RoundCorners = 10
        Me.grpPu.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpPu.ShadowControl = False
        Me.grpPu.ShadowThickness = 3
        Me.grpPu.Size = New System.Drawing.Size(1012, 153)
        Me.grpPu.TabIndex = 0
        '
        'txtPUMeltWt_WET
        '
        Me.txtPUMeltWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUMeltWt_WET.Location = New System.Drawing.Point(435, 59)
        Me.txtPUMeltWt_WET.MaxLength = 10
        Me.txtPUMeltWt_WET.Name = "txtPUMeltWt_WET"
        Me.txtPUMeltWt_WET.Size = New System.Drawing.Size(53, 26)
        Me.txtPUMeltWt_WET.TabIndex = 34
        Me.txtPUMeltWt_WET.Visible = False
        '
        'txtPUPurity_PER
        '
        Me.txtPUPurity_PER.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUPurity_PER.Location = New System.Drawing.Point(236, 31)
        Me.txtPUPurity_PER.Name = "txtPUPurity_PER"
        Me.txtPUPurity_PER.Size = New System.Drawing.Size(75, 26)
        Me.txtPUPurity_PER.TabIndex = 5
        '
        'txtPuWastage_PER
        '
        Me.txtPuWastage_PER.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPuWastage_PER.Location = New System.Drawing.Point(499, 31)
        Me.txtPuWastage_PER.Name = "txtPuWastage_PER"
        Me.txtPuWastage_PER.Size = New System.Drawing.Size(48, 26)
        Me.txtPuWastage_PER.TabIndex = 13
        '
        'txtPUEstNo_NUM
        '
        Me.txtPUEstNo_NUM.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUEstNo_NUM.Location = New System.Drawing.Point(8, 31)
        Me.txtPUEstNo_NUM.Name = "txtPUEstNo_NUM"
        Me.txtPUEstNo_NUM.Size = New System.Drawing.Size(49, 26)
        Me.txtPUEstNo_NUM.TabIndex = 1
        '
        'txtPURowIndex
        '
        Me.txtPURowIndex.Location = New System.Drawing.Point(754, 80)
        Me.txtPURowIndex.Name = "txtPURowIndex"
        Me.txtPURowIndex.Size = New System.Drawing.Size(8, 24)
        Me.txtPURowIndex.TabIndex = 26
        Me.txtPURowIndex.Visible = False
        '
        'txtPUCategory
        '
        Me.txtPUCategory.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUCategory.Location = New System.Drawing.Point(58, 31)
        Me.txtPUCategory.Name = "txtPUCategory"
        Me.txtPUCategory.Size = New System.Drawing.Size(177, 26)
        Me.txtPUCategory.TabIndex = 3
        '
        'txtPUEmpId_NUM
        '
        Me.txtPUEmpId_NUM.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPUEmpId_NUM.Location = New System.Drawing.Point(947, 31)
        Me.txtPUEmpId_NUM.MaxLength = 8
        Me.txtPUEmpId_NUM.Name = "txtPUEmpId_NUM"
        Me.txtPUEmpId_NUM.Size = New System.Drawing.Size(35, 26)
        Me.txtPUEmpId_NUM.TabIndex = 25
        '
        'gridPur
        '
        Me.gridPur.AllowUserToAddRows = False
        Me.gridPur.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridPur.Location = New System.Drawing.Point(8, 54)
        Me.gridPur.Name = "gridPur"
        Me.gridPur.ReadOnly = True
        Me.gridPur.RowHeadersWidth = 51
        Me.gridPur.Size = New System.Drawing.Size(992, 74)
        Me.gridPur.TabIndex = 27
        '
        'Label96
        '
        Me.Label96.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label96.Location = New System.Drawing.Point(10, 12)
        Me.Label96.Name = "Label96"
        Me.Label96.Size = New System.Drawing.Size(49, 17)
        Me.Label96.TabIndex = 0
        Me.Label96.Text = "Est No"
        Me.Label96.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'gridPurTotal
        '
        Me.gridPurTotal.AllowUserToAddRows = False
        Me.gridPurTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridPurTotal.Enabled = False
        Me.gridPurTotal.Location = New System.Drawing.Point(8, 128)
        Me.gridPurTotal.Name = "gridPurTotal"
        Me.gridPurTotal.ReadOnly = True
        Me.gridPurTotal.RowHeadersWidth = 51
        Me.gridPurTotal.Size = New System.Drawing.Size(992, 19)
        Me.gridPurTotal.TabIndex = 28
        '
        'Label63
        '
        Me.Label63.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label63.Location = New System.Drawing.Point(947, 12)
        Me.Label63.Name = "Label63"
        Me.Label63.Size = New System.Drawing.Size(35, 17)
        Me.Label63.TabIndex = 24
        Me.Label63.Text = "Emp"
        Me.Label63.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label51
        '
        Me.Label51.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label51.Location = New System.Drawing.Point(499, 12)
        Me.Label51.Name = "Label51"
        Me.Label51.Size = New System.Drawing.Size(48, 17)
        Me.Label51.TabIndex = 12
        Me.Label51.Text = "W%"
        Me.Label51.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tabReceipt
        '
        Me.tabReceipt.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.tabReceipt.Controls.Add(Me.grpReceipt)
        Me.tabReceipt.Location = New System.Drawing.Point(4, 14)
        Me.tabReceipt.Name = "tabReceipt"
        Me.tabReceipt.Padding = New System.Windows.Forms.Padding(3)
        Me.tabReceipt.Size = New System.Drawing.Size(1023, 329)
        Me.tabReceipt.TabIndex = 1
        Me.tabReceipt.Text = "Receipt"
        Me.tabReceipt.UseVisualStyleBackColor = True
        '
        'grpReceipt
        '
        Me.grpReceipt.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpReceipt.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpReceipt.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpReceipt.BorderColor = System.Drawing.Color.Transparent
        Me.grpReceipt.BorderThickness = 1.0!
        Me.grpReceipt.ContextMenuStrip = Me.cmenuReceiptGrid
        Me.grpReceipt.Controls.Add(Me.txtReceiptTotAmt_AMT)
        Me.grpReceipt.Controls.Add(Me.Label15)
        Me.grpReceipt.Controls.Add(Me.txtReceiptGST_AMT)
        Me.grpReceipt.Controls.Add(Me.Label13)
        Me.grpReceipt.Controls.Add(Me.txtReceiptType)
        Me.grpReceipt.Controls.Add(Me.txtReceiptAccount)
        Me.grpReceipt.Controls.Add(Me.lblEsthelp)
        Me.grpReceipt.Controls.Add(Me.lblRecOnAcc)
        Me.grpReceipt.Controls.Add(Me.txtReceiptEmpId_NUM)
        Me.grpReceipt.Controls.Add(Me.txtReceiptRemark)
        Me.grpReceipt.Controls.Add(Me.Label58)
        Me.grpReceipt.Controls.Add(Me.txtReceiptEntRefNo)
        Me.grpReceipt.Controls.Add(Me.txtReceiptEntAmount)
        Me.grpReceipt.Controls.Add(Me.chkReceiptRateFix)
        Me.grpReceipt.Controls.Add(Me.gridReceipt)
        Me.grpReceipt.Controls.Add(Me.cmbReceiptTranType)
        Me.grpReceipt.Controls.Add(Me.cmbReceiptReceiptType)
        Me.grpReceipt.Controls.Add(Me.txtReceiptRate_AMT)
        Me.grpReceipt.Controls.Add(Me.txtReceiptAmount_AMT)
        Me.grpReceipt.Controls.Add(Me.txtReceiptRefNo)
        Me.grpReceipt.Controls.Add(Me.gridReceiptTotal)
        Me.grpReceipt.Controls.Add(Me.Label56)
        Me.grpReceipt.Controls.Add(Me.Label25)
        Me.grpReceipt.Controls.Add(Me.Label169)
        Me.grpReceipt.Controls.Add(Me.Label168)
        Me.grpReceipt.Controls.Add(Me.lblRecTrantype)
        Me.grpReceipt.Controls.Add(Me.lblRecRefno)
        Me.grpReceipt.Controls.Add(Me.Label37)
        Me.grpReceipt.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpReceipt.GroupImage = Nothing
        Me.grpReceipt.GroupTitle = ""
        Me.grpReceipt.Location = New System.Drawing.Point(10, -10)
        Me.grpReceipt.Name = "grpReceipt"
        Me.grpReceipt.Padding = New System.Windows.Forms.Padding(20)
        Me.grpReceipt.PaintGroupBox = False
        Me.grpReceipt.RoundCorners = 10
        Me.grpReceipt.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpReceipt.ShadowControl = False
        Me.grpReceipt.ShadowThickness = 3
        Me.grpReceipt.Size = New System.Drawing.Size(1012, 336)
        Me.grpReceipt.TabIndex = 0
        '
        'cmenuReceiptGrid
        '
        Me.cmenuReceiptGrid.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.cmenuReceiptGrid.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tStripWeightAdvance, Me.tagReservation})
        Me.cmenuReceiptGrid.Name = "cmenuReceiptGrid"
        Me.cmenuReceiptGrid.Size = New System.Drawing.Size(187, 52)
        '
        'tStripWeightAdvance
        '
        Me.tStripWeightAdvance.Name = "tStripWeightAdvance"
        Me.tStripWeightAdvance.Size = New System.Drawing.Size(186, 24)
        Me.tStripWeightAdvance.Text = "Weight Advance"
        '
        'tagReservation
        '
        Me.tagReservation.Name = "tagReservation"
        Me.tagReservation.Size = New System.Drawing.Size(186, 24)
        Me.tagReservation.Text = "Tag Reservation"
        '
        'txtReceiptTotAmt_AMT
        '
        Me.txtReceiptTotAmt_AMT.Enabled = False
        Me.txtReceiptTotAmt_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptTotAmt_AMT.Location = New System.Drawing.Point(683, 45)
        Me.txtReceiptTotAmt_AMT.MaxLength = 10
        Me.txtReceiptTotAmt_AMT.Name = "txtReceiptTotAmt_AMT"
        Me.txtReceiptTotAmt_AMT.ReadOnly = True
        Me.txtReceiptTotAmt_AMT.Size = New System.Drawing.Size(96, 26)
        Me.txtReceiptTotAmt_AMT.TabIndex = 15
        Me.txtReceiptTotAmt_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label15
        '
        Me.Label15.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(683, 29)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(92, 14)
        Me.Label15.TabIndex = 14
        Me.Label15.Text = "Total"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtReceiptGST_AMT
        '
        Me.txtReceiptGST_AMT.Enabled = False
        Me.txtReceiptGST_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptGST_AMT.Location = New System.Drawing.Point(612, 45)
        Me.txtReceiptGST_AMT.MaxLength = 10
        Me.txtReceiptGST_AMT.Name = "txtReceiptGST_AMT"
        Me.txtReceiptGST_AMT.ReadOnly = True
        Me.txtReceiptGST_AMT.Size = New System.Drawing.Size(70, 26)
        Me.txtReceiptGST_AMT.TabIndex = 13
        Me.txtReceiptGST_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label13
        '
        Me.Label13.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(612, 29)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(66, 14)
        Me.Label13.TabIndex = 12
        Me.Label13.Text = "Gst"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtReceiptType
        '
        Me.txtReceiptType.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptType.Location = New System.Drawing.Point(349, 128)
        Me.txtReceiptType.MaxLength = 10
        Me.txtReceiptType.Name = "txtReceiptType"
        Me.txtReceiptType.Size = New System.Drawing.Size(86, 26)
        Me.txtReceiptType.TabIndex = 24
        Me.txtReceiptType.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtReceiptType.Visible = False
        '
        'txtReceiptAccount
        '
        Me.txtReceiptAccount.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptAccount.Location = New System.Drawing.Point(237, 46)
        Me.txtReceiptAccount.MaxLength = 50
        Me.txtReceiptAccount.Name = "txtReceiptAccount"
        Me.txtReceiptAccount.Size = New System.Drawing.Size(125, 26)
        Me.txtReceiptAccount.TabIndex = 21
        '
        'lblEsthelp
        '
        Me.lblEsthelp.AutoSize = True
        Me.lblEsthelp.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEsthelp.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblEsthelp.Location = New System.Drawing.Point(8, 13)
        Me.lblEsthelp.Name = "lblEsthelp"
        Me.lblEsthelp.Size = New System.Drawing.Size(205, 18)
        Me.lblEsthelp.TabIndex = 22
        Me.lblEsthelp.Text = "Alt + Insert - Call Estimate"
        '
        'lblRecOnAcc
        '
        Me.lblRecOnAcc.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecOnAcc.Location = New System.Drawing.Point(253, 30)
        Me.lblRecOnAcc.Name = "lblRecOnAcc"
        Me.lblRecOnAcc.Size = New System.Drawing.Size(88, 14)
        Me.lblRecOnAcc.TabIndex = 18
        Me.lblRecOnAcc.Text = "On Account"
        Me.lblRecOnAcc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtReceiptEmpId_NUM
        '
        Me.txtReceiptEmpId_NUM.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptEmpId_NUM.Location = New System.Drawing.Point(940, 45)
        Me.txtReceiptEmpId_NUM.MaxLength = 50
        Me.txtReceiptEmpId_NUM.Name = "txtReceiptEmpId_NUM"
        Me.txtReceiptEmpId_NUM.Size = New System.Drawing.Size(41, 26)
        Me.txtReceiptEmpId_NUM.TabIndex = 19
        '
        'txtReceiptRemark
        '
        Me.txtReceiptRemark.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptRemark.Location = New System.Drawing.Point(780, 45)
        Me.txtReceiptRemark.MaxLength = 50
        Me.txtReceiptRemark.Name = "txtReceiptRemark"
        Me.txtReceiptRemark.Size = New System.Drawing.Size(159, 26)
        Me.txtReceiptRemark.TabIndex = 17
        '
        'Label58
        '
        Me.Label58.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label58.Location = New System.Drawing.Point(780, 27)
        Me.Label58.Name = "Label58"
        Me.Label58.Size = New System.Drawing.Size(155, 16)
        Me.Label58.TabIndex = 16
        Me.Label58.Text = "Remark"
        Me.Label58.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtReceiptEntRefNo
        '
        Me.txtReceiptEntRefNo.Location = New System.Drawing.Point(503, 149)
        Me.txtReceiptEntRefNo.Name = "txtReceiptEntRefNo"
        Me.txtReceiptEntRefNo.Size = New System.Drawing.Size(31, 24)
        Me.txtReceiptEntRefNo.TabIndex = 14
        Me.txtReceiptEntRefNo.Visible = False
        '
        'txtReceiptEntAmount
        '
        Me.txtReceiptEntAmount.Location = New System.Drawing.Point(503, 176)
        Me.txtReceiptEntAmount.Name = "txtReceiptEntAmount"
        Me.txtReceiptEntAmount.Size = New System.Drawing.Size(31, 24)
        Me.txtReceiptEntAmount.TabIndex = 15
        Me.txtReceiptEntAmount.Visible = False
        '
        'chkReceiptRateFix
        '
        Me.chkReceiptRateFix.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.chkReceiptRateFix.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkReceiptRateFix.Location = New System.Drawing.Point(590, 48)
        Me.chkReceiptRateFix.Name = "chkReceiptRateFix"
        Me.chkReceiptRateFix.Size = New System.Drawing.Size(22, 17)
        Me.chkReceiptRateFix.TabIndex = 11
        Me.chkReceiptRateFix.UseVisualStyleBackColor = True
        '
        'gridReceipt
        '
        Me.gridReceipt.AllowUserToAddRows = False
        Me.gridReceipt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridReceipt.Location = New System.Drawing.Point(10, 69)
        Me.gridReceipt.Name = "gridReceipt"
        Me.gridReceipt.ReadOnly = True
        Me.gridReceipt.RowHeadersWidth = 51
        Me.gridReceipt.Size = New System.Drawing.Size(991, 236)
        Me.gridReceipt.TabIndex = 16
        '
        'cmbReceiptTranType
        '
        Me.cmbReceiptTranType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbReceiptTranType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbReceiptTranType.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbReceiptTranType.FormattingEnabled = True
        Me.cmbReceiptTranType.Location = New System.Drawing.Point(133, 46)
        Me.cmbReceiptTranType.Name = "cmbReceiptTranType"
        Me.cmbReceiptTranType.Size = New System.Drawing.Size(103, 26)
        Me.cmbReceiptTranType.TabIndex = 3
        '
        'cmbReceiptReceiptType
        '
        Me.cmbReceiptReceiptType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbReceiptReceiptType.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbReceiptReceiptType.FormattingEnabled = True
        Me.cmbReceiptReceiptType.Location = New System.Drawing.Point(10, 46)
        Me.cmbReceiptReceiptType.Name = "cmbReceiptReceiptType"
        Me.cmbReceiptReceiptType.Size = New System.Drawing.Size(121, 26)
        Me.cmbReceiptReceiptType.TabIndex = 1
        '
        'txtReceiptRate_AMT
        '
        Me.txtReceiptRate_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptRate_AMT.Location = New System.Drawing.Point(521, 46)
        Me.txtReceiptRate_AMT.MaxLength = 10
        Me.txtReceiptRate_AMT.Name = "txtReceiptRate_AMT"
        Me.txtReceiptRate_AMT.Size = New System.Drawing.Size(68, 26)
        Me.txtReceiptRate_AMT.TabIndex = 9
        Me.txtReceiptRate_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtReceiptAmount_AMT
        '
        Me.txtReceiptAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptAmount_AMT.Location = New System.Drawing.Point(431, 46)
        Me.txtReceiptAmount_AMT.MaxLength = 12
        Me.txtReceiptAmount_AMT.Name = "txtReceiptAmount_AMT"
        Me.txtReceiptAmount_AMT.Size = New System.Drawing.Size(89, 26)
        Me.txtReceiptAmount_AMT.TabIndex = 7
        Me.txtReceiptAmount_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtReceiptRefNo
        '
        Me.txtReceiptRefNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptRefNo.Location = New System.Drawing.Point(363, 46)
        Me.txtReceiptRefNo.MaxLength = 10
        Me.txtReceiptRefNo.Name = "txtReceiptRefNo"
        Me.txtReceiptRefNo.Size = New System.Drawing.Size(67, 26)
        Me.txtReceiptRefNo.TabIndex = 5
        Me.txtReceiptRefNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'gridReceiptTotal
        '
        Me.gridReceiptTotal.AllowUserToAddRows = False
        Me.gridReceiptTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridReceiptTotal.Enabled = False
        Me.gridReceiptTotal.Location = New System.Drawing.Point(10, 305)
        Me.gridReceiptTotal.Name = "gridReceiptTotal"
        Me.gridReceiptTotal.ReadOnly = True
        Me.gridReceiptTotal.RowHeadersWidth = 51
        Me.gridReceiptTotal.Size = New System.Drawing.Size(991, 19)
        Me.gridReceiptTotal.TabIndex = 13
        '
        'Label56
        '
        Me.Label56.AutoSize = True
        Me.Label56.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label56.Location = New System.Drawing.Point(582, 13)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(47, 36)
        Me.Label56.TabIndex = 10
        Me.Label56.Text = "Rate" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Fix"
        Me.Label56.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label25
        '
        Me.Label25.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label25.Location = New System.Drawing.Point(940, 29)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(41, 14)
        Me.Label25.TabIndex = 18
        Me.Label25.Text = "Emp"
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label169
        '
        Me.Label169.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label169.Location = New System.Drawing.Point(521, 30)
        Me.Label169.Name = "Label169"
        Me.Label169.Size = New System.Drawing.Size(68, 14)
        Me.Label169.TabIndex = 8
        Me.Label169.Text = "Rate"
        Me.Label169.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label168
        '
        Me.Label168.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label168.Location = New System.Drawing.Point(431, 30)
        Me.Label168.Name = "Label168"
        Me.Label168.Size = New System.Drawing.Size(89, 14)
        Me.Label168.TabIndex = 6
        Me.Label168.Text = "Amount"
        Me.Label168.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblRecTrantype
        '
        Me.lblRecTrantype.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecTrantype.Location = New System.Drawing.Point(133, 30)
        Me.lblRecTrantype.Name = "lblRecTrantype"
        Me.lblRecTrantype.Size = New System.Drawing.Size(100, 14)
        Me.lblRecTrantype.TabIndex = 2
        Me.lblRecTrantype.Text = "Tran Type"
        Me.lblRecTrantype.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblRecRefno
        '
        Me.lblRecRefno.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecRefno.Location = New System.Drawing.Point(363, 30)
        Me.lblRecRefno.Name = "lblRecRefno"
        Me.lblRecRefno.Size = New System.Drawing.Size(67, 14)
        Me.lblRecRefno.TabIndex = 4
        Me.lblRecRefno.Text = "RefNo"
        Me.lblRecRefno.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label37
        '
        Me.Label37.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label37.Location = New System.Drawing.Point(8, 30)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(120, 14)
        Me.Label37.TabIndex = 0
        Me.Label37.Text = "ReceiptType"
        Me.Label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tabPayment
        '
        Me.tabPayment.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.tabPayment.Controls.Add(Me.grpPayment)
        Me.tabPayment.Location = New System.Drawing.Point(4, 14)
        Me.tabPayment.Name = "tabPayment"
        Me.tabPayment.Size = New System.Drawing.Size(1023, 329)
        Me.tabPayment.TabIndex = 2
        Me.tabPayment.Text = "Payment"
        Me.tabPayment.UseVisualStyleBackColor = True
        '
        'grpPayment
        '
        Me.grpPayment.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpPayment.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpPayment.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpPayment.BorderColor = System.Drawing.Color.Transparent
        Me.grpPayment.BorderThickness = 1.0!
        Me.grpPayment.Controls.Add(Me.txtPaymentTotAmt_AMT)
        Me.grpPayment.Controls.Add(Me.Label23)
        Me.grpPayment.Controls.Add(Me.txtPaymentGST_AMT)
        Me.grpPayment.Controls.Add(Me.Label80)
        Me.grpPayment.Controls.Add(Me.txtPayRefAccode)
        Me.grpPayment.Controls.Add(Me.txtPaymentAccount)
        Me.grpPayment.Controls.Add(Me.Label101)
        Me.grpPayment.Controls.Add(Me.txtPaymentEmpId_NUM)
        Me.grpPayment.Controls.Add(Me.Label33)
        Me.grpPayment.Controls.Add(Me.txtPaymentRemark)
        Me.grpPayment.Controls.Add(Me.txtPaymentEntRefNo)
        Me.grpPayment.Controls.Add(Me.txtPaymentEntAmount)
        Me.grpPayment.Controls.Add(Me.gridPayment)
        Me.grpPayment.Controls.Add(Me.cmbPaymentTranType)
        Me.grpPayment.Controls.Add(Me.cmbPaymentPaytype)
        Me.grpPayment.Controls.Add(Me.txtPaymentRate_AMT)
        Me.grpPayment.Controls.Add(Me.txtPaymentAmount_Amt)
        Me.grpPayment.Controls.Add(Me.txtPaymentRefNo)
        Me.grpPayment.Controls.Add(Me.gridPaymentTotal)
        Me.grpPayment.Controls.Add(Me.Label74)
        Me.grpPayment.Controls.Add(Me.Label77)
        Me.grpPayment.Controls.Add(Me.lblPayType)
        Me.grpPayment.Controls.Add(Me.Label81)
        Me.grpPayment.Controls.Add(Me.Label82)
        Me.grpPayment.Controls.Add(Me.Label83)
        Me.grpPayment.Controls.Add(Me.txtPaymentWeight_WET)
        Me.grpPayment.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpPayment.GroupImage = Nothing
        Me.grpPayment.GroupTitle = ""
        Me.grpPayment.Location = New System.Drawing.Point(10, -10)
        Me.grpPayment.Name = "grpPayment"
        Me.grpPayment.Padding = New System.Windows.Forms.Padding(20)
        Me.grpPayment.PaintGroupBox = False
        Me.grpPayment.RoundCorners = 10
        Me.grpPayment.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpPayment.ShadowControl = False
        Me.grpPayment.ShadowThickness = 3
        Me.grpPayment.Size = New System.Drawing.Size(1012, 336)
        Me.grpPayment.TabIndex = 0
        '
        'txtPaymentTotAmt_AMT
        '
        Me.txtPaymentTotAmt_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentTotAmt_AMT.Location = New System.Drawing.Point(683, 44)
        Me.txtPaymentTotAmt_AMT.MaxLength = 10
        Me.txtPaymentTotAmt_AMT.Name = "txtPaymentTotAmt_AMT"
        Me.txtPaymentTotAmt_AMT.Size = New System.Drawing.Size(99, 26)
        Me.txtPaymentTotAmt_AMT.TabIndex = 15
        Me.txtPaymentTotAmt_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label23
        '
        Me.Label23.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(681, 18)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(95, 24)
        Me.Label23.TabIndex = 14
        Me.Label23.Text = "Total"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPaymentGST_AMT
        '
        Me.txtPaymentGST_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentGST_AMT.Location = New System.Drawing.Point(612, 44)
        Me.txtPaymentGST_AMT.MaxLength = 10
        Me.txtPaymentGST_AMT.Name = "txtPaymentGST_AMT"
        Me.txtPaymentGST_AMT.Size = New System.Drawing.Size(70, 26)
        Me.txtPaymentGST_AMT.TabIndex = 13
        Me.txtPaymentGST_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label80
        '
        Me.Label80.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label80.Location = New System.Drawing.Point(612, 18)
        Me.Label80.Name = "Label80"
        Me.Label80.Size = New System.Drawing.Size(66, 24)
        Me.Label80.TabIndex = 12
        Me.Label80.Text = "Gst"
        Me.Label80.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPayRefAccode
        '
        Me.txtPayRefAccode.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPayRefAccode.Location = New System.Drawing.Point(461, 10)
        Me.txtPayRefAccode.MaxLength = 50
        Me.txtPayRefAccode.Name = "txtPayRefAccode"
        Me.txtPayRefAccode.Size = New System.Drawing.Size(39, 26)
        Me.txtPayRefAccode.TabIndex = 18
        Me.txtPayRefAccode.Visible = False
        '
        'txtPaymentAccount
        '
        Me.txtPaymentAccount.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentAccount.Location = New System.Drawing.Point(253, 44)
        Me.txtPaymentAccount.MaxLength = 50
        Me.txtPaymentAccount.Name = "txtPaymentAccount"
        Me.txtPaymentAccount.Size = New System.Drawing.Size(123, 26)
        Me.txtPaymentAccount.TabIndex = 5
        '
        'Label101
        '
        Me.Label101.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label101.Location = New System.Drawing.Point(265, 18)
        Me.Label101.Name = "Label101"
        Me.Label101.Size = New System.Drawing.Size(98, 24)
        Me.Label101.TabIndex = 4
        Me.Label101.Text = "On Account"
        Me.Label101.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPaymentEmpId_NUM
        '
        Me.txtPaymentEmpId_NUM.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentEmpId_NUM.Location = New System.Drawing.Point(934, 44)
        Me.txtPaymentEmpId_NUM.MaxLength = 50
        Me.txtPaymentEmpId_NUM.Name = "txtPaymentEmpId_NUM"
        Me.txtPaymentEmpId_NUM.Size = New System.Drawing.Size(41, 26)
        Me.txtPaymentEmpId_NUM.TabIndex = 19
        '
        'Label33
        '
        Me.Label33.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label33.Location = New System.Drawing.Point(934, 18)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(41, 24)
        Me.Label33.TabIndex = 18
        Me.Label33.Text = "Emp"
        Me.Label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPaymentRemark
        '
        Me.txtPaymentRemark.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentRemark.Location = New System.Drawing.Point(783, 44)
        Me.txtPaymentRemark.MaxLength = 50
        Me.txtPaymentRemark.Name = "txtPaymentRemark"
        Me.txtPaymentRemark.Size = New System.Drawing.Size(150, 26)
        Me.txtPaymentRemark.TabIndex = 17
        '
        'txtPaymentEntRefNo
        '
        Me.txtPaymentEntRefNo.Location = New System.Drawing.Point(483, 134)
        Me.txtPaymentEntRefNo.Name = "txtPaymentEntRefNo"
        Me.txtPaymentEntRefNo.Size = New System.Drawing.Size(31, 24)
        Me.txtPaymentEntRefNo.TabIndex = 22
        Me.txtPaymentEntRefNo.Visible = False
        '
        'txtPaymentEntAmount
        '
        Me.txtPaymentEntAmount.Location = New System.Drawing.Point(483, 161)
        Me.txtPaymentEntAmount.Name = "txtPaymentEntAmount"
        Me.txtPaymentEntAmount.Size = New System.Drawing.Size(31, 24)
        Me.txtPaymentEntAmount.TabIndex = 23
        Me.txtPaymentEntAmount.Visible = False
        '
        'gridPayment
        '
        Me.gridPayment.AllowUserToAddRows = False
        Me.gridPayment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridPayment.Location = New System.Drawing.Point(8, 67)
        Me.gridPayment.Name = "gridPayment"
        Me.gridPayment.ReadOnly = True
        Me.gridPayment.RowHeadersWidth = 51
        Me.gridPayment.Size = New System.Drawing.Size(991, 236)
        Me.gridPayment.TabIndex = 20
        '
        'cmbPaymentTranType
        '
        Me.cmbPaymentTranType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cmbPaymentTranType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cmbPaymentTranType.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbPaymentTranType.FormattingEnabled = True
        Me.cmbPaymentTranType.Location = New System.Drawing.Point(136, 44)
        Me.cmbPaymentTranType.Name = "cmbPaymentTranType"
        Me.cmbPaymentTranType.Size = New System.Drawing.Size(116, 26)
        Me.cmbPaymentTranType.TabIndex = 3
        '
        'cmbPaymentPaytype
        '
        Me.cmbPaymentPaytype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPaymentPaytype.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbPaymentPaytype.FormattingEnabled = True
        Me.cmbPaymentPaytype.Location = New System.Drawing.Point(8, 44)
        Me.cmbPaymentPaytype.Name = "cmbPaymentPaytype"
        Me.cmbPaymentPaytype.Size = New System.Drawing.Size(126, 26)
        Me.cmbPaymentPaytype.TabIndex = 1
        '
        'txtPaymentRate_AMT
        '
        Me.txtPaymentRate_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentRate_AMT.Location = New System.Drawing.Point(547, 44)
        Me.txtPaymentRate_AMT.MaxLength = 10
        Me.txtPaymentRate_AMT.Name = "txtPaymentRate_AMT"
        Me.txtPaymentRate_AMT.Size = New System.Drawing.Size(64, 26)
        Me.txtPaymentRate_AMT.TabIndex = 11
        Me.txtPaymentRate_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtPaymentAmount_Amt
        '
        Me.txtPaymentAmount_Amt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentAmount_Amt.Location = New System.Drawing.Point(454, 44)
        Me.txtPaymentAmount_Amt.MaxLength = 12
        Me.txtPaymentAmount_Amt.Name = "txtPaymentAmount_Amt"
        Me.txtPaymentAmount_Amt.Size = New System.Drawing.Size(92, 26)
        Me.txtPaymentAmount_Amt.TabIndex = 9
        Me.txtPaymentAmount_Amt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtPaymentRefNo
        '
        Me.txtPaymentRefNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentRefNo.Location = New System.Drawing.Point(377, 44)
        Me.txtPaymentRefNo.MaxLength = 10
        Me.txtPaymentRefNo.Name = "txtPaymentRefNo"
        Me.txtPaymentRefNo.Size = New System.Drawing.Size(76, 26)
        Me.txtPaymentRefNo.TabIndex = 7
        Me.txtPaymentRefNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'gridPaymentTotal
        '
        Me.gridPaymentTotal.AllowUserToAddRows = False
        Me.gridPaymentTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridPaymentTotal.Enabled = False
        Me.gridPaymentTotal.Location = New System.Drawing.Point(8, 303)
        Me.gridPaymentTotal.Name = "gridPaymentTotal"
        Me.gridPaymentTotal.ReadOnly = True
        Me.gridPaymentTotal.RowHeadersWidth = 51
        Me.gridPaymentTotal.Size = New System.Drawing.Size(991, 19)
        Me.gridPaymentTotal.TabIndex = 21
        '
        'Label74
        '
        Me.Label74.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label74.Location = New System.Drawing.Point(786, 18)
        Me.Label74.Name = "Label74"
        Me.Label74.Size = New System.Drawing.Size(146, 24)
        Me.Label74.TabIndex = 16
        Me.Label74.Text = "Remark"
        Me.Label74.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label77
        '
        Me.Label77.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label77.Location = New System.Drawing.Point(547, 18)
        Me.Label77.Name = "Label77"
        Me.Label77.Size = New System.Drawing.Size(64, 24)
        Me.Label77.TabIndex = 10
        Me.Label77.Text = "Rate"
        Me.Label77.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblPayType
        '
        Me.lblPayType.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPayType.Location = New System.Drawing.Point(454, 18)
        Me.lblPayType.Name = "lblPayType"
        Me.lblPayType.Size = New System.Drawing.Size(92, 24)
        Me.lblPayType.TabIndex = 8
        Me.lblPayType.Text = "Amount"
        Me.lblPayType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label81
        '
        Me.Label81.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label81.Location = New System.Drawing.Point(135, 20)
        Me.Label81.Name = "Label81"
        Me.Label81.Size = New System.Drawing.Size(115, 24)
        Me.Label81.TabIndex = 2
        Me.Label81.Text = "Tran Type"
        Me.Label81.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label82
        '
        Me.Label82.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label82.Location = New System.Drawing.Point(377, 18)
        Me.Label82.Name = "Label82"
        Me.Label82.Size = New System.Drawing.Size(76, 24)
        Me.Label82.TabIndex = 6
        Me.Label82.Text = "RefNo"
        Me.Label82.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label83
        '
        Me.Label83.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label83.Location = New System.Drawing.Point(8, 18)
        Me.Label83.Name = "Label83"
        Me.Label83.Size = New System.Drawing.Size(124, 24)
        Me.Label83.TabIndex = 0
        Me.Label83.Text = "Payment Type"
        Me.Label83.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPaymentWeight_WET
        '
        Me.txtPaymentWeight_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPaymentWeight_WET.Location = New System.Drawing.Point(454, 45)
        Me.txtPaymentWeight_WET.MaxLength = 12
        Me.txtPaymentWeight_WET.Name = "txtPaymentWeight_WET"
        Me.txtPaymentWeight_WET.Size = New System.Drawing.Size(92, 26)
        Me.txtPaymentWeight_WET.TabIndex = 7
        Me.txtPaymentWeight_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'tabGiftVoucherMain
        '
        Me.tabGiftVoucherMain.Controls.Add(Me.grpGiftVoucherMain)
        Me.tabGiftVoucherMain.Location = New System.Drawing.Point(4, 14)
        Me.tabGiftVoucherMain.Name = "tabGiftVoucherMain"
        Me.tabGiftVoucherMain.Size = New System.Drawing.Size(1023, 329)
        Me.tabGiftVoucherMain.TabIndex = 3
        Me.tabGiftVoucherMain.Text = "Gift Voucher"
        Me.tabGiftVoucherMain.UseVisualStyleBackColor = True
        '
        'grpGiftVoucherMain
        '
        Me.grpGiftVoucherMain.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpGiftVoucherMain.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpGiftVoucherMain.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpGiftVoucherMain.BorderColor = System.Drawing.Color.Transparent
        Me.grpGiftVoucherMain.BorderThickness = 1.0!
        Me.grpGiftVoucherMain.Controls.Add(Me.txtMGiftRowIndex)
        Me.grpGiftVoucherMain.Controls.Add(Me.Label85)
        Me.grpGiftVoucherMain.Controls.Add(Me.txtMGiftRemark)
        Me.grpGiftVoucherMain.Controls.Add(Me.gridMGiftVoucherTotal)
        Me.grpGiftVoucherMain.Controls.Add(Me.gridMGiftVoucher)
        Me.grpGiftVoucherMain.Controls.Add(Me.Label92)
        Me.grpGiftVoucherMain.Controls.Add(Me.Label93)
        Me.grpGiftVoucherMain.Controls.Add(Me.Label94)
        Me.grpGiftVoucherMain.Controls.Add(Me.Label95)
        Me.grpGiftVoucherMain.Controls.Add(Me.txtMGiftAmount_AMT)
        Me.grpGiftVoucherMain.Controls.Add(Me.txtMGiftUnit_NUM)
        Me.grpGiftVoucherMain.Controls.Add(Me.txtMGiftDenomination_AMT)
        Me.grpGiftVoucherMain.Controls.Add(Me.cmbMGiftVoucherType)
        Me.grpGiftVoucherMain.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpGiftVoucherMain.GroupImage = Nothing
        Me.grpGiftVoucherMain.GroupTitle = ""
        Me.grpGiftVoucherMain.Location = New System.Drawing.Point(10, -10)
        Me.grpGiftVoucherMain.Name = "grpGiftVoucherMain"
        Me.grpGiftVoucherMain.Padding = New System.Windows.Forms.Padding(20)
        Me.grpGiftVoucherMain.PaintGroupBox = False
        Me.grpGiftVoucherMain.RoundCorners = 10
        Me.grpGiftVoucherMain.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpGiftVoucherMain.ShadowControl = False
        Me.grpGiftVoucherMain.ShadowThickness = 3
        Me.grpGiftVoucherMain.Size = New System.Drawing.Size(1012, 336)
        Me.grpGiftVoucherMain.TabIndex = 0
        '
        'txtMGiftRowIndex
        '
        Me.txtMGiftRowIndex.Location = New System.Drawing.Point(588, 120)
        Me.txtMGiftRowIndex.Name = "txtMGiftRowIndex"
        Me.txtMGiftRowIndex.Size = New System.Drawing.Size(10, 24)
        Me.txtMGiftRowIndex.TabIndex = 12
        Me.txtMGiftRowIndex.Visible = False
        '
        'Label85
        '
        Me.Label85.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label85.Location = New System.Drawing.Point(307, 26)
        Me.Label85.Name = "Label85"
        Me.Label85.Size = New System.Drawing.Size(365, 14)
        Me.Label85.TabIndex = 2
        Me.Label85.Text = "Vocher No/Remark"
        Me.Label85.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtMGiftRemark
        '
        Me.txtMGiftRemark.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMGiftRemark.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMGiftRemark.Location = New System.Drawing.Point(307, 43)
        Me.txtMGiftRemark.MaxLength = 20
        Me.txtMGiftRemark.Name = "txtMGiftRemark"
        Me.txtMGiftRemark.Size = New System.Drawing.Size(365, 26)
        Me.txtMGiftRemark.TabIndex = 3
        Me.txtMGiftRemark.Text = "12345678901234567890"
        '
        'gridMGiftVoucherTotal
        '
        Me.gridMGiftVoucherTotal.AllowUserToAddRows = False
        Me.gridMGiftVoucherTotal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridMGiftVoucherTotal.Enabled = False
        Me.gridMGiftVoucherTotal.Location = New System.Drawing.Point(77, 302)
        Me.gridMGiftVoucherTotal.Name = "gridMGiftVoucherTotal"
        Me.gridMGiftVoucherTotal.ReadOnly = True
        Me.gridMGiftVoucherTotal.RowHeadersWidth = 51
        Me.gridMGiftVoucherTotal.Size = New System.Drawing.Size(881, 19)
        Me.gridMGiftVoucherTotal.TabIndex = 11
        '
        'gridMGiftVoucher
        '
        Me.gridMGiftVoucher.AllowUserToAddRows = False
        Me.gridMGiftVoucher.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridMGiftVoucher.Location = New System.Drawing.Point(77, 66)
        Me.gridMGiftVoucher.Name = "gridMGiftVoucher"
        Me.gridMGiftVoucher.ReadOnly = True
        Me.gridMGiftVoucher.RowHeadersWidth = 51
        Me.gridMGiftVoucher.Size = New System.Drawing.Size(881, 236)
        Me.gridMGiftVoucher.TabIndex = 10
        '
        'Label92
        '
        Me.Label92.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label92.Location = New System.Drawing.Point(840, 26)
        Me.Label92.Name = "Label92"
        Me.Label92.Size = New System.Drawing.Size(98, 14)
        Me.Label92.TabIndex = 8
        Me.Label92.Text = "Amount"
        Me.Label92.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label93
        '
        Me.Label93.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label93.Location = New System.Drawing.Point(775, 26)
        Me.Label93.Name = "Label93"
        Me.Label93.Size = New System.Drawing.Size(64, 14)
        Me.Label93.TabIndex = 6
        Me.Label93.Text = "Unit"
        Me.Label93.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label94
        '
        Me.Label94.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label94.Location = New System.Drawing.Point(673, 26)
        Me.Label94.Name = "Label94"
        Me.Label94.Size = New System.Drawing.Size(101, 14)
        Me.Label94.TabIndex = 4
        Me.Label94.Text = "Denomination"
        Me.Label94.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label95
        '
        Me.Label95.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label95.Location = New System.Drawing.Point(77, 26)
        Me.Label95.Name = "Label95"
        Me.Label95.Size = New System.Drawing.Size(229, 14)
        Me.Label95.TabIndex = 0
        Me.Label95.Text = "Voucher Type"
        Me.Label95.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtMGiftAmount_AMT
        '
        Me.txtMGiftAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMGiftAmount_AMT.Location = New System.Drawing.Point(840, 43)
        Me.txtMGiftAmount_AMT.MaxLength = 12
        Me.txtMGiftAmount_AMT.Name = "txtMGiftAmount_AMT"
        Me.txtMGiftAmount_AMT.Size = New System.Drawing.Size(98, 26)
        Me.txtMGiftAmount_AMT.TabIndex = 9
        '
        'txtMGiftUnit_NUM
        '
        Me.txtMGiftUnit_NUM.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMGiftUnit_NUM.Location = New System.Drawing.Point(775, 43)
        Me.txtMGiftUnit_NUM.MaxLength = 8
        Me.txtMGiftUnit_NUM.Name = "txtMGiftUnit_NUM"
        Me.txtMGiftUnit_NUM.Size = New System.Drawing.Size(64, 26)
        Me.txtMGiftUnit_NUM.TabIndex = 7
        '
        'txtMGiftDenomination_AMT
        '
        Me.txtMGiftDenomination_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMGiftDenomination_AMT.Location = New System.Drawing.Point(673, 43)
        Me.txtMGiftDenomination_AMT.MaxLength = 12
        Me.txtMGiftDenomination_AMT.Name = "txtMGiftDenomination_AMT"
        Me.txtMGiftDenomination_AMT.Size = New System.Drawing.Size(101, 26)
        Me.txtMGiftDenomination_AMT.TabIndex = 5
        '
        'cmbMGiftVoucherType
        '
        Me.cmbMGiftVoucherType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMGiftVoucherType.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbMGiftVoucherType.FormattingEnabled = True
        Me.cmbMGiftVoucherType.Location = New System.Drawing.Point(77, 43)
        Me.cmbMGiftVoucherType.Name = "cmbMGiftVoucherType"
        Me.cmbMGiftVoucherType.Size = New System.Drawing.Size(229, 26)
        Me.cmbMGiftVoucherType.TabIndex = 1
        '
        'txtSaDiscountAfterTax
        '
        Me.txtSaDiscountAfterTax.Location = New System.Drawing.Point(246, 63)
        Me.txtSaDiscountAfterTax.Name = "txtSaDiscountAfterTax"
        Me.txtSaDiscountAfterTax.Size = New System.Drawing.Size(88, 24)
        Me.txtSaDiscountAfterTax.TabIndex = 22
        Me.txtSaDiscountAfterTax.Visible = False
        '
        'txtSAVatPer_PER
        '
        Me.txtSAVatPer_PER.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSAVatPer_PER.Location = New System.Drawing.Point(196, 137)
        Me.txtSAVatPer_PER.Name = "txtSAVatPer_PER"
        Me.txtSAVatPer_PER.Size = New System.Drawing.Size(31, 21)
        Me.txtSAVatPer_PER.TabIndex = 0
        '
        'grpRecReservedItem
        '
        Me.grpRecReservedItem.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpRecReservedItem.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpRecReservedItem.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpRecReservedItem.BorderColor = System.Drawing.Color.Transparent
        Me.grpRecReservedItem.BorderThickness = 1.0!
        Me.grpRecReservedItem.Controls.Add(Me.txtRecEmpId_NUM)
        Me.grpRecReservedItem.Controls.Add(Me.Label140)
        Me.grpRecReservedItem.Controls.Add(Me.txtReceiptEst_Num)
        Me.grpRecReservedItem.Controls.Add(Me.Label108)
        Me.grpRecReservedItem.Controls.Add(Me.txtReceiptValue)
        Me.grpRecReservedItem.Controls.Add(Me.txtReceiptNetWT)
        Me.grpRecReservedItem.Controls.Add(Me.txtReceiptPcs)
        Me.grpRecReservedItem.Controls.Add(Me.txtReceiptGrsWt)
        Me.grpRecReservedItem.Controls.Add(Me.Label67)
        Me.grpRecReservedItem.Controls.Add(Me.txtReceiptItemName)
        Me.grpRecReservedItem.Controls.Add(Me.gridReceiptReserved)
        Me.grpRecReservedItem.Controls.Add(Me.txtReceiptTagNo)
        Me.grpRecReservedItem.Controls.Add(Me.txtReceiptItemId_MAN)
        Me.grpRecReservedItem.Controls.Add(Me.Label73)
        Me.grpRecReservedItem.Controls.Add(Me.Label70)
        Me.grpRecReservedItem.Controls.Add(Me.Label69)
        Me.grpRecReservedItem.Controls.Add(Me.Label68)
        Me.grpRecReservedItem.Controls.Add(Me.Label66)
        Me.grpRecReservedItem.Controls.Add(Me.Label65)
        Me.grpRecReservedItem.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpRecReservedItem.GroupImage = Nothing
        Me.grpRecReservedItem.GroupTitle = ""
        Me.grpRecReservedItem.Location = New System.Drawing.Point(4, -5)
        Me.grpRecReservedItem.Name = "grpRecReservedItem"
        Me.grpRecReservedItem.Padding = New System.Windows.Forms.Padding(20)
        Me.grpRecReservedItem.PaintGroupBox = False
        Me.grpRecReservedItem.RoundCorners = 10
        Me.grpRecReservedItem.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpRecReservedItem.ShadowControl = False
        Me.grpRecReservedItem.ShadowThickness = 3
        Me.grpRecReservedItem.Size = New System.Drawing.Size(576, 285)
        Me.grpRecReservedItem.TabIndex = 29
        '
        'txtRecEmpId_NUM
        '
        Me.txtRecEmpId_NUM.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRecEmpId_NUM.Location = New System.Drawing.Point(531, 38)
        Me.txtRecEmpId_NUM.Name = "txtRecEmpId_NUM"
        Me.txtRecEmpId_NUM.Size = New System.Drawing.Size(32, 26)
        Me.txtRecEmpId_NUM.TabIndex = 16
        '
        'Label140
        '
        Me.Label140.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label140.Location = New System.Drawing.Point(531, 19)
        Me.Label140.Name = "Label140"
        Me.Label140.Size = New System.Drawing.Size(36, 16)
        Me.Label140.TabIndex = 15
        Me.Label140.Text = "EmpId"
        Me.Label140.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtReceiptEst_Num
        '
        Me.txtReceiptEst_Num.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptEst_Num.Location = New System.Drawing.Point(6, 38)
        Me.txtReceiptEst_Num.Name = "txtReceiptEst_Num"
        Me.txtReceiptEst_Num.Size = New System.Drawing.Size(42, 26)
        Me.txtReceiptEst_Num.TabIndex = 1
        '
        'Label108
        '
        Me.Label108.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label108.Location = New System.Drawing.Point(3, 18)
        Me.Label108.Name = "Label108"
        Me.Label108.Size = New System.Drawing.Size(49, 17)
        Me.Label108.TabIndex = 14
        Me.Label108.Text = "Est No"
        Me.Label108.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtReceiptValue
        '
        Me.txtReceiptValue.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptValue.Location = New System.Drawing.Point(457, 38)
        Me.txtReceiptValue.Name = "txtReceiptValue"
        Me.txtReceiptValue.Size = New System.Drawing.Size(73, 26)
        Me.txtReceiptValue.TabIndex = 13
        '
        'txtReceiptNetWT
        '
        Me.txtReceiptNetWT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptNetWT.Location = New System.Drawing.Point(393, 38)
        Me.txtReceiptNetWT.Name = "txtReceiptNetWT"
        Me.txtReceiptNetWT.Size = New System.Drawing.Size(63, 26)
        Me.txtReceiptNetWT.TabIndex = 11
        '
        'txtReceiptPcs
        '
        Me.txtReceiptPcs.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptPcs.Location = New System.Drawing.Point(298, 38)
        Me.txtReceiptPcs.Name = "txtReceiptPcs"
        Me.txtReceiptPcs.Size = New System.Drawing.Size(32, 26)
        Me.txtReceiptPcs.TabIndex = 7
        '
        'txtReceiptGrsWt
        '
        Me.txtReceiptGrsWt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptGrsWt.Location = New System.Drawing.Point(331, 38)
        Me.txtReceiptGrsWt.Name = "txtReceiptGrsWt"
        Me.txtReceiptGrsWt.Size = New System.Drawing.Size(61, 26)
        Me.txtReceiptGrsWt.TabIndex = 9
        '
        'Label67
        '
        Me.Label67.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label67.Location = New System.Drawing.Point(103, 19)
        Me.Label67.Name = "Label67"
        Me.Label67.Size = New System.Drawing.Size(112, 16)
        Me.Label67.TabIndex = 2
        Me.Label67.Text = "Item Name"
        Me.Label67.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtReceiptItemName
        '
        Me.txtReceiptItemName.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptItemName.Location = New System.Drawing.Point(103, 38)
        Me.txtReceiptItemName.Name = "txtReceiptItemName"
        Me.txtReceiptItemName.Size = New System.Drawing.Size(112, 26)
        Me.txtReceiptItemName.TabIndex = 3
        '
        'gridReceiptReserved
        '
        Me.gridReceiptReserved.AllowUserToAddRows = False
        Me.gridReceiptReserved.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridReceiptReserved.Location = New System.Drawing.Point(6, 61)
        Me.gridReceiptReserved.Name = "gridReceiptReserved"
        Me.gridReceiptReserved.ReadOnly = True
        Me.gridReceiptReserved.RowHeadersWidth = 51
        Me.gridReceiptReserved.Size = New System.Drawing.Size(563, 217)
        Me.gridReceiptReserved.TabIndex = 6
        '
        'txtReceiptTagNo
        '
        Me.txtReceiptTagNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptTagNo.Location = New System.Drawing.Point(216, 38)
        Me.txtReceiptTagNo.MaxLength = 10
        Me.txtReceiptTagNo.Name = "txtReceiptTagNo"
        Me.txtReceiptTagNo.Size = New System.Drawing.Size(81, 26)
        Me.txtReceiptTagNo.TabIndex = 5
        '
        'txtReceiptItemId_MAN
        '
        Me.txtReceiptItemId_MAN.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptItemId_MAN.Location = New System.Drawing.Point(49, 38)
        Me.txtReceiptItemId_MAN.MaxLength = 15
        Me.txtReceiptItemId_MAN.Name = "txtReceiptItemId_MAN"
        Me.txtReceiptItemId_MAN.Size = New System.Drawing.Size(53, 26)
        Me.txtReceiptItemId_MAN.TabIndex = 2
        '
        'Label73
        '
        Me.Label73.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label73.Location = New System.Drawing.Point(457, 19)
        Me.Label73.Name = "Label73"
        Me.Label73.Size = New System.Drawing.Size(69, 16)
        Me.Label73.TabIndex = 12
        Me.Label73.Text = "Value"
        Me.Label73.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label70
        '
        Me.Label70.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label70.Location = New System.Drawing.Point(393, 19)
        Me.Label70.Name = "Label70"
        Me.Label70.Size = New System.Drawing.Size(63, 16)
        Me.Label70.TabIndex = 10
        Me.Label70.Text = "Net Wt"
        Me.Label70.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label69
        '
        Me.Label69.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label69.Location = New System.Drawing.Point(298, 19)
        Me.Label69.Name = "Label69"
        Me.Label69.Size = New System.Drawing.Size(30, 16)
        Me.Label69.TabIndex = 5
        Me.Label69.Text = "Pcs"
        Me.Label69.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label68
        '
        Me.Label68.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label68.Location = New System.Drawing.Point(331, 19)
        Me.Label68.Name = "Label68"
        Me.Label68.Size = New System.Drawing.Size(61, 16)
        Me.Label68.TabIndex = 8
        Me.Label68.Text = "Grs Wt"
        Me.Label68.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label66
        '
        Me.Label66.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label66.Location = New System.Drawing.Point(216, 19)
        Me.Label66.Name = "Label66"
        Me.Label66.Size = New System.Drawing.Size(81, 16)
        Me.Label66.TabIndex = 4
        Me.Label66.Text = "TagNo"
        Me.Label66.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label65
        '
        Me.Label65.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label65.Location = New System.Drawing.Point(49, 19)
        Me.Label65.Name = "Label65"
        Me.Label65.Size = New System.Drawing.Size(53, 16)
        Me.Label65.TabIndex = 0
        Me.Label65.Text = "ItemId"
        Me.Label65.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpReceiptWeightAdvance
        '
        Me.grpReceiptWeightAdvance.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpReceiptWeightAdvance.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpReceiptWeightAdvance.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpReceiptWeightAdvance.BorderColor = System.Drawing.Color.Transparent
        Me.grpReceiptWeightAdvance.BorderThickness = 1.0!
        Me.grpReceiptWeightAdvance.Controls.Add(Me.Label176)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.txtReceiptBullionRate_RATE)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.txtReceiptWastage_WET)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.txtReceiptNetWt_WET)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.txtReceiptValue_AMT)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.Label172)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.Label171)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.Label177)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.Label170)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.Label173)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.Label174)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.txtReceiptGrsWt_WET)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.txtReceiptPurity_AMT)
        Me.grpReceiptWeightAdvance.Controls.Add(Me.cmbReceiptCategory)
        Me.grpReceiptWeightAdvance.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpReceiptWeightAdvance.GroupImage = Nothing
        Me.grpReceiptWeightAdvance.GroupTitle = ""
        Me.grpReceiptWeightAdvance.Location = New System.Drawing.Point(4, -5)
        Me.grpReceiptWeightAdvance.Name = "grpReceiptWeightAdvance"
        Me.grpReceiptWeightAdvance.Padding = New System.Windows.Forms.Padding(20)
        Me.grpReceiptWeightAdvance.PaintGroupBox = False
        Me.grpReceiptWeightAdvance.RoundCorners = 10
        Me.grpReceiptWeightAdvance.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpReceiptWeightAdvance.ShadowControl = False
        Me.grpReceiptWeightAdvance.ShadowThickness = 3
        Me.grpReceiptWeightAdvance.Size = New System.Drawing.Size(576, 285)
        Me.grpReceiptWeightAdvance.TabIndex = 11
        '
        'Label176
        '
        Me.Label176.AutoSize = True
        Me.Label176.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label176.Location = New System.Drawing.Point(152, 58)
        Me.Label176.Name = "Label176"
        Me.Label176.Size = New System.Drawing.Size(83, 18)
        Me.Label176.TabIndex = 0
        Me.Label176.Text = "Category"
        Me.Label176.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtReceiptBullionRate_RATE
        '
        Me.txtReceiptBullionRate_RATE.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptBullionRate_RATE.Location = New System.Drawing.Point(394, 154)
        Me.txtReceiptBullionRate_RATE.MaxLength = 10
        Me.txtReceiptBullionRate_RATE.Name = "txtReceiptBullionRate_RATE"
        Me.txtReceiptBullionRate_RATE.Size = New System.Drawing.Size(99, 26)
        Me.txtReceiptBullionRate_RATE.TabIndex = 11
        Me.txtReceiptBullionRate_RATE.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtReceiptWastage_WET
        '
        Me.txtReceiptWastage_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptWastage_WET.Location = New System.Drawing.Point(394, 119)
        Me.txtReceiptWastage_WET.MaxLength = 9
        Me.txtReceiptWastage_WET.Name = "txtReceiptWastage_WET"
        Me.txtReceiptWastage_WET.Size = New System.Drawing.Size(99, 26)
        Me.txtReceiptWastage_WET.TabIndex = 7
        Me.txtReceiptWastage_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtReceiptNetWt_WET
        '
        Me.txtReceiptNetWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptNetWt_WET.Location = New System.Drawing.Point(230, 154)
        Me.txtReceiptNetWt_WET.MaxLength = 10
        Me.txtReceiptNetWt_WET.Name = "txtReceiptNetWt_WET"
        Me.txtReceiptNetWt_WET.Size = New System.Drawing.Size(90, 26)
        Me.txtReceiptNetWt_WET.TabIndex = 9
        Me.txtReceiptNetWt_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtReceiptValue_AMT
        '
        Me.txtReceiptValue_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptValue_AMT.Location = New System.Drawing.Point(230, 184)
        Me.txtReceiptValue_AMT.MaxLength = 12
        Me.txtReceiptValue_AMT.Name = "txtReceiptValue_AMT"
        Me.txtReceiptValue_AMT.Size = New System.Drawing.Size(90, 26)
        Me.txtReceiptValue_AMT.TabIndex = 13
        Me.txtReceiptValue_AMT.Text = "1000000.00"
        Me.txtReceiptValue_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label172
        '
        Me.Label172.AutoSize = True
        Me.Label172.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label172.Location = New System.Drawing.Point(152, 158)
        Me.Label172.Name = "Label172"
        Me.Label172.Size = New System.Drawing.Size(65, 18)
        Me.Label172.TabIndex = 8
        Me.Label172.Text = "Net Wt"
        Me.Label172.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label171
        '
        Me.Label171.AutoSize = True
        Me.Label171.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label171.Location = New System.Drawing.Point(331, 123)
        Me.Label171.Name = "Label171"
        Me.Label171.Size = New System.Drawing.Size(80, 18)
        Me.Label171.TabIndex = 6
        Me.Label171.Text = "Wastage"
        Me.Label171.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label177
        '
        Me.Label177.AutoSize = True
        Me.Label177.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label177.Location = New System.Drawing.Point(152, 89)
        Me.Label177.Name = "Label177"
        Me.Label177.Size = New System.Drawing.Size(57, 18)
        Me.Label177.TabIndex = 2
        Me.Label177.Text = "Purity"
        Me.Label177.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label170
        '
        Me.Label170.AutoSize = True
        Me.Label170.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label170.Location = New System.Drawing.Point(152, 124)
        Me.Label170.Name = "Label170"
        Me.Label170.Size = New System.Drawing.Size(64, 18)
        Me.Label170.TabIndex = 4
        Me.Label170.Text = "Grs Wt"
        Me.Label170.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label173
        '
        Me.Label173.AutoSize = True
        Me.Label173.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label173.Location = New System.Drawing.Point(331, 158)
        Me.Label173.Name = "Label173"
        Me.Label173.Size = New System.Drawing.Size(47, 18)
        Me.Label173.TabIndex = 10
        Me.Label173.Text = "Rate"
        Me.Label173.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label174
        '
        Me.Label174.AutoSize = True
        Me.Label174.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label174.Location = New System.Drawing.Point(152, 188)
        Me.Label174.Name = "Label174"
        Me.Label174.Size = New System.Drawing.Size(53, 18)
        Me.Label174.TabIndex = 12
        Me.Label174.Text = "Value"
        Me.Label174.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtReceiptGrsWt_WET
        '
        Me.txtReceiptGrsWt_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptGrsWt_WET.Location = New System.Drawing.Point(230, 120)
        Me.txtReceiptGrsWt_WET.MaxLength = 10
        Me.txtReceiptGrsWt_WET.Name = "txtReceiptGrsWt_WET"
        Me.txtReceiptGrsWt_WET.Size = New System.Drawing.Size(90, 26)
        Me.txtReceiptGrsWt_WET.TabIndex = 5
        Me.txtReceiptGrsWt_WET.Text = "100.000"
        Me.txtReceiptGrsWt_WET.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtReceiptPurity_AMT
        '
        Me.txtReceiptPurity_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReceiptPurity_AMT.Location = New System.Drawing.Point(230, 85)
        Me.txtReceiptPurity_AMT.MaxLength = 5
        Me.txtReceiptPurity_AMT.Name = "txtReceiptPurity_AMT"
        Me.txtReceiptPurity_AMT.Size = New System.Drawing.Size(90, 26)
        Me.txtReceiptPurity_AMT.TabIndex = 3
        Me.txtReceiptPurity_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cmbReceiptCategory
        '
        Me.cmbReceiptCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbReceiptCategory.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbReceiptCategory.FormattingEnabled = True
        Me.cmbReceiptCategory.Location = New System.Drawing.Point(230, 54)
        Me.cmbReceiptCategory.Name = "cmbReceiptCategory"
        Me.cmbReceiptCategory.Size = New System.Drawing.Size(263, 26)
        Me.cmbReceiptCategory.TabIndex = 0
        '
        'tabOtherOptions
        '
        Me.tabOtherOptions.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tabOtherOptions.Controls.Add(Me.tabGeneral)
        Me.tabOtherOptions.Controls.Add(Me.tabHideDet)
        Me.tabOtherOptions.Controls.Add(Me.tabReceiptWeightAdvance)
        Me.tabOtherOptions.Controls.Add(Me.tabReceiptReserve)
        Me.tabOtherOptions.Controls.Add(Me.tabOrderDetail)
        Me.tabOtherOptions.Controls.Add(Me.tabAdvanceWeightAdjCalc)
        Me.tabOtherOptions.Controls.Add(Me.tabWholeSaleDetails)
        Me.tabOtherOptions.Controls.Add(Me.tabSAExtraDetail)
        Me.tabOtherOptions.ItemSize = New System.Drawing.Size(1, 10)
        Me.tabOtherOptions.Location = New System.Drawing.Point(-3, 390)
        Me.tabOtherOptions.Name = "tabOtherOptions"
        Me.tabOtherOptions.SelectedIndex = 0
        Me.tabOtherOptions.Size = New System.Drawing.Size(589, 299)
        Me.tabOtherOptions.TabIndex = 0
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.grpGeneral1)
        Me.tabGeneral.Controls.Add(Me.Grouper1)
        Me.tabGeneral.Controls.Add(Me.grpGeneral)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 14)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Size = New System.Drawing.Size(581, 281)
        Me.tabGeneral.TabIndex = 12
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'grpGeneral1
        '
        Me.grpGeneral1.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpGeneral1.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpGeneral1.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpGeneral1.BorderColor = System.Drawing.Color.Transparent
        Me.grpGeneral1.BorderThickness = 1.0!
        Me.grpGeneral1.Controls.Add(Me.pnlFinalBalance)
        Me.grpGeneral1.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpGeneral1.GroupImage = Nothing
        Me.grpGeneral1.GroupTitle = ""
        Me.grpGeneral1.Location = New System.Drawing.Point(416, -5)
        Me.grpGeneral1.Name = "grpGeneral1"
        Me.grpGeneral1.Padding = New System.Windows.Forms.Padding(20)
        Me.grpGeneral1.PaintGroupBox = False
        Me.grpGeneral1.RoundCorners = 10
        Me.grpGeneral1.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpGeneral1.ShadowControl = False
        Me.grpGeneral1.ShadowThickness = 3
        Me.grpGeneral1.Size = New System.Drawing.Size(165, 180)
        Me.grpGeneral1.TabIndex = 2
        '
        'pnlFinalBalance
        '
        Me.pnlFinalBalance.BackColor = System.Drawing.Color.Lavender
        Me.pnlFinalBalance.Controls.Add(Me.lblTCS)
        Me.pnlFinalBalance.Controls.Add(Me.txtTCS_Amt)
        Me.pnlFinalBalance.Controls.Add(Me.lblFinalpercent)
        Me.pnlFinalBalance.Controls.Add(Me.lblFinalBalanceStatus)
        Me.pnlFinalBalance.Controls.Add(Me.lblFinalBalance)
        Me.pnlFinalBalance.Controls.Add(Me.Label110)
        Me.pnlFinalBalance.Location = New System.Drawing.Point(5, 15)
        Me.pnlFinalBalance.Name = "pnlFinalBalance"
        Me.pnlFinalBalance.Size = New System.Drawing.Size(157, 161)
        Me.pnlFinalBalance.TabIndex = 7
        '
        'lblTCS
        '
        Me.lblTCS.AutoSize = True
        Me.lblTCS.BackColor = System.Drawing.Color.Transparent
        Me.lblTCS.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTCS.Location = New System.Drawing.Point(3, 45)
        Me.lblTCS.Name = "lblTCS"
        Me.lblTCS.Size = New System.Drawing.Size(40, 18)
        Me.lblTCS.TabIndex = 35
        Me.lblTCS.Text = "TCS"
        Me.lblTCS.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblTCS.Visible = False
        '
        'txtTCS_Amt
        '
        Me.txtTCS_Amt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTCS_Amt.Location = New System.Drawing.Point(36, 42)
        Me.txtTCS_Amt.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtTCS_Amt.MaxLength = 12
        Me.txtTCS_Amt.Name = "txtTCS_Amt"
        Me.txtTCS_Amt.Size = New System.Drawing.Size(111, 26)
        Me.txtTCS_Amt.TabIndex = 34
        Me.txtTCS_Amt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtTCS_Amt.Visible = False
        '
        'lblFinalpercent
        '
        Me.lblFinalpercent.Font = New System.Drawing.Font("Verdana", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFinalpercent.ForeColor = System.Drawing.Color.Red
        Me.lblFinalpercent.Location = New System.Drawing.Point(6, 4)
        Me.lblFinalpercent.Name = "lblFinalpercent"
        Me.lblFinalpercent.Size = New System.Drawing.Size(141, 59)
        Me.lblFinalpercent.TabIndex = 3
        Me.lblFinalpercent.Text = "Rs 399" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "0.65%"
        Me.lblFinalpercent.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblFinalBalanceStatus
        '
        Me.lblFinalBalanceStatus.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFinalBalanceStatus.Location = New System.Drawing.Point(9, 63)
        Me.lblFinalBalanceStatus.Name = "lblFinalBalanceStatus"
        Me.lblFinalBalanceStatus.Size = New System.Drawing.Size(144, 56)
        Me.lblFinalBalanceStatus.TabIndex = 1
        Me.lblFinalBalanceStatus.Text = "Cash Received 200000.00" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "From Customer"
        Me.lblFinalBalanceStatus.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblFinalBalance
        '
        Me.lblFinalBalance.BackColor = System.Drawing.SystemColors.Window
        Me.lblFinalBalance.Font = New System.Drawing.Font("Verdana", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFinalBalance.Location = New System.Drawing.Point(4, 126)
        Me.lblFinalBalance.Name = "lblFinalBalance"
        Me.lblFinalBalance.Size = New System.Drawing.Size(153, 26)
        Me.lblFinalBalance.TabIndex = 0
        Me.lblFinalBalance.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label110
        '
        Me.Label110.BackColor = System.Drawing.Color.Black
        Me.Label110.Location = New System.Drawing.Point(7, 117)
        Me.Label110.Name = "Label110"
        Me.Label110.Size = New System.Drawing.Size(153, 26)
        Me.Label110.TabIndex = 2
        Me.Label110.Text = "Label110"
        '
        'Grouper1
        '
        Me.Grouper1.BackgroundColor = System.Drawing.Color.Lavender
        Me.Grouper1.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.Grouper1.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.Grouper1.BorderColor = System.Drawing.Color.Transparent
        Me.Grouper1.BorderThickness = 1.0!
        Me.Grouper1.Controls.Add(Me.Label99)
        Me.Grouper1.Controls.Add(Me.txtDetOffWt)
        Me.Grouper1.Controls.Add(Me.lblKFC)
        Me.Grouper1.Controls.Add(Me.btnAttachImage)
        Me.Grouper1.Controls.Add(Me.Label137)
        Me.Grouper1.Controls.Add(Me.Label129)
        Me.Grouper1.Controls.Add(Me.Label130)
        Me.Grouper1.Controls.Add(Me.Label131)
        Me.Grouper1.Controls.Add(Me.Label132)
        Me.Grouper1.Controls.Add(Me.Label128)
        Me.Grouper1.Controls.Add(Me.Label123)
        Me.Grouper1.Controls.Add(Me.Label120)
        Me.Grouper1.Controls.Add(Me.Label122)
        Me.Grouper1.Controls.Add(Me.Label109)
        Me.Grouper1.Controls.Add(Me.Label119)
        Me.Grouper1.Controls.Add(Me.chkAppSales)
        Me.Grouper1.Controls.Add(Me.Label198)
        Me.Grouper1.Controls.Add(Me.Label166)
        Me.Grouper1.Controls.Add(Me.Label185)
        Me.Grouper1.Controls.Add(Me.Label197)
        Me.Grouper1.Controls.Add(Me.Label165)
        Me.Grouper1.Controls.Add(Me.Label184)
        Me.Grouper1.Controls.Add(Me.Label196)
        Me.Grouper1.Controls.Add(Me.Label164)
        Me.Grouper1.Controls.Add(Me.Label183)
        Me.Grouper1.Controls.Add(Me.Label163)
        Me.Grouper1.Controls.Add(Me.Label162)
        Me.Grouper1.Controls.Add(Me.Label161)
        Me.Grouper1.Controls.Add(Me.Label182)
        Me.Grouper1.Controls.Add(Me.Label193)
        Me.Grouper1.Controls.Add(Me.Label158)
        Me.Grouper1.Controls.Add(Me.Label181)
        Me.Grouper1.Controls.Add(Me.Label192)
        Me.Grouper1.Controls.Add(Me.Label157)
        Me.Grouper1.Controls.Add(Me.Label180)
        Me.Grouper1.Controls.Add(Me.Label190)
        Me.Grouper1.Controls.Add(Me.Label156)
        Me.Grouper1.Controls.Add(Me.Label179)
        Me.Grouper1.Controls.Add(Me.Label189)
        Me.Grouper1.Controls.Add(Me.Label155)
        Me.Grouper1.Controls.Add(Me.Label178)
        Me.Grouper1.Controls.Add(Me.Label154)
        Me.Grouper1.Controls.Add(Me.Label175)
        Me.Grouper1.Controls.Add(Me.Label153)
        Me.Grouper1.Controls.Add(Me.Label152)
        Me.Grouper1.Controls.Add(Me.Label151)
        Me.Grouper1.Controls.Add(Me.Label150)
        Me.Grouper1.Controls.Add(Me.Label149)
        Me.Grouper1.Controls.Add(Me.Label48)
        Me.Grouper1.Controls.Add(Me.Label186)
        Me.Grouper1.Controls.Add(Me.Label187)
        Me.Grouper1.Controls.Add(Me.chkPartlySales)
        Me.Grouper1.Controls.Add(Me.btnExit)
        Me.Grouper1.Controls.Add(Me.btnNew)
        Me.Grouper1.Controls.Add(Me.Label204)
        Me.Grouper1.Controls.Add(Me.Label203)
        Me.Grouper1.Controls.Add(Me.Label202)
        Me.Grouper1.Controls.Add(Me.Label201)
        Me.Grouper1.Controls.Add(Me.lblBookedItem)
        Me.Grouper1.Controls.Add(Me.btnSave)
        Me.Grouper1.Controls.Add(Me.Label188)
        Me.Grouper1.CustomGroupBoxColor = System.Drawing.Color.Transparent
        Me.Grouper1.GroupImage = Nothing
        Me.Grouper1.GroupTitle = ""
        Me.Grouper1.Location = New System.Drawing.Point(4, 173)
        Me.Grouper1.Name = "Grouper1"
        Me.Grouper1.Padding = New System.Windows.Forms.Padding(20)
        Me.Grouper1.PaintGroupBox = False
        Me.Grouper1.RoundCorners = 10
        Me.Grouper1.ShadowColor = System.Drawing.Color.DarkGray
        Me.Grouper1.ShadowControl = False
        Me.Grouper1.ShadowThickness = 3
        Me.Grouper1.Size = New System.Drawing.Size(577, 112)
        Me.Grouper1.TabIndex = 1
        '
        'Label99
        '
        Me.Label99.AutoSize = True
        Me.Label99.Font = New System.Drawing.Font("Verdana", 6.5!, System.Drawing.FontStyle.Bold)
        Me.Label99.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label99.Location = New System.Drawing.Point(313, 62)
        Me.Label99.Name = "Label99"
        Me.Label99.Size = New System.Drawing.Size(164, 13)
        Me.Label99.TabIndex = 51
        Me.Label99.Text = "[Alt+A] Shiping Address"
        '
        'txtDetOffWt
        '
        Me.txtDetOffWt.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetOffWt.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.txtDetOffWt.Location = New System.Drawing.Point(370, 89)
        Me.txtDetOffWt.Name = "txtDetOffWt"
        Me.txtDetOffWt.Size = New System.Drawing.Size(45, 21)
        Me.txtDetOffWt.TabIndex = 43
        '
        'lblKFC
        '
        Me.lblKFC.AutoSize = True
        Me.lblKFC.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblKFC.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.lblKFC.Location = New System.Drawing.Point(280, 91)
        Me.lblKFC.Name = "lblKFC"
        Me.lblKFC.Size = New System.Drawing.Size(112, 17)
        Me.lblKFC.TabIndex = 50
        Me.lblKFC.Text = "[Ctl+F8] KFC"
        '
        'btnAttachImage
        '
        Me.btnAttachImage.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAttachImage.Image = Global.BrighttechRetailJewellery.My.Resources.Resources.AquaBall_Blue
        Me.btnAttachImage.Location = New System.Drawing.Point(417, 73)
        Me.btnAttachImage.Name = "btnAttachImage"
        Me.btnAttachImage.Size = New System.Drawing.Size(28, 24)
        Me.btnAttachImage.TabIndex = 49
        Me.btnAttachImage.UseVisualStyleBackColor = True
        '
        'Label137
        '
        Me.Label137.AutoSize = True
        Me.Label137.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label137.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label137.Location = New System.Drawing.Point(175, 90)
        Me.Label137.Name = "Label137"
        Me.Label137.Size = New System.Drawing.Size(136, 17)
        Me.Label137.TabIndex = 46
        Me.Label137.Text = "[Ctl+F10] Comp"
        '
        'Label129
        '
        Me.Label129.AutoSize = True
        Me.Label129.Font = New System.Drawing.Font("Verdana", 7.25!, System.Drawing.FontStyle.Bold)
        Me.Label129.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label129.Location = New System.Drawing.Point(364, 25)
        Me.Label129.Name = "Label129"
        Me.Label129.Size = New System.Drawing.Size(13, 16)
        Me.Label129.TabIndex = 44
        Me.Label129.Text = "]"
        '
        'Label130
        '
        Me.Label130.AutoSize = True
        Me.Label130.Font = New System.Drawing.Font("Verdana", 7.25!, System.Drawing.FontStyle.Bold)
        Me.Label130.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label130.Location = New System.Drawing.Point(314, 25)
        Me.Label130.Name = "Label130"
        Me.Label130.Size = New System.Drawing.Size(13, 16)
        Me.Label130.TabIndex = 43
        Me.Label130.Text = "["
        '
        'Label131
        '
        Me.Label131.AutoSize = True
        Me.Label131.Font = New System.Drawing.Font("Verdana", 7.25!, System.Drawing.FontStyle.Bold)
        Me.Label131.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label131.Location = New System.Drawing.Point(323, 26)
        Me.Label131.Name = "Label131"
        Me.Label131.Size = New System.Drawing.Size(52, 16)
        Me.Label131.TabIndex = 42
        Me.Label131.Text = "Ctl+W"
        '
        'Label132
        '
        Me.Label132.AutoSize = True
        Me.Label132.Font = New System.Drawing.Font("Verdana", 7.25!, System.Drawing.FontStyle.Bold)
        Me.Label132.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label132.Location = New System.Drawing.Point(375, 26)
        Me.Label132.Name = "Label132"
        Me.Label132.Size = New System.Drawing.Size(81, 16)
        Me.Label132.TabIndex = 41
        Me.Label132.Text = "Wt-Adjust"
        '
        'Label128
        '
        Me.Label128.AutoSize = True
        Me.Label128.Font = New System.Drawing.Font("Verdana", 7.25!, System.Drawing.FontStyle.Bold)
        Me.Label128.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label128.Location = New System.Drawing.Point(364, 13)
        Me.Label128.Name = "Label128"
        Me.Label128.Size = New System.Drawing.Size(13, 16)
        Me.Label128.TabIndex = 40
        Me.Label128.Text = "]"
        '
        'Label123
        '
        Me.Label123.AutoSize = True
        Me.Label123.Font = New System.Drawing.Font("Verdana", 7.25!, System.Drawing.FontStyle.Bold)
        Me.Label123.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label123.Location = New System.Drawing.Point(313, 13)
        Me.Label123.Name = "Label123"
        Me.Label123.Size = New System.Drawing.Size(13, 16)
        Me.Label123.TabIndex = 38
        Me.Label123.Text = "["
        '
        'Label120
        '
        Me.Label120.AutoSize = True
        Me.Label120.Font = New System.Drawing.Font("Verdana", 7.25!, System.Drawing.FontStyle.Bold)
        Me.Label120.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label120.Location = New System.Drawing.Point(323, 14)
        Me.Label120.Name = "Label120"
        Me.Label120.Size = New System.Drawing.Size(48, 16)
        Me.Label120.TabIndex = 37
        Me.Label120.Text = "Ctl+D"
        '
        'Label122
        '
        Me.Label122.AutoSize = True
        Me.Label122.Font = New System.Drawing.Font("Verdana", 7.25!, System.Drawing.FontStyle.Bold)
        Me.Label122.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label122.Location = New System.Drawing.Point(375, 14)
        Me.Label122.Name = "Label122"
        Me.Label122.Size = New System.Drawing.Size(66, 16)
        Me.Label122.TabIndex = 36
        Me.Label122.Text = "Dup. Bill"
        '
        'Label109
        '
        Me.Label109.AutoSize = True
        Me.Label109.Font = New System.Drawing.Font("Verdana", 7.25!, System.Drawing.FontStyle.Bold)
        Me.Label109.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label109.Location = New System.Drawing.Point(314, 37)
        Me.Label109.Name = "Label109"
        Me.Label109.Size = New System.Drawing.Size(166, 16)
        Me.Label109.TabIndex = 35
        Me.Label109.Text = "[Ctl+Alt+R] Rate View"
        '
        'Label119
        '
        Me.Label119.AutoSize = True
        Me.Label119.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Bold)
        Me.Label119.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label119.Location = New System.Drawing.Point(313, 50)
        Me.Label119.Name = "Label119"
        Me.Label119.Size = New System.Drawing.Size(167, 14)
        Me.Label119.TabIndex = 34
        Me.Label119.Text = "[Alt+H] HallmarkDetails"
        '
        'chkAppSales
        '
        Me.chkAppSales.AutoSize = True
        Me.chkAppSales.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkAppSales.Location = New System.Drawing.Point(335, 74)
        Me.chkAppSales.Name = "chkAppSales"
        Me.chkAppSales.Size = New System.Drawing.Size(100, 21)
        Me.chkAppSales.TabIndex = 33
        Me.chkAppSales.Text = "App Sales"
        Me.chkAppSales.UseVisualStyleBackColor = True
        Me.chkAppSales.Visible = False
        '
        'Label198
        '
        Me.Label198.AutoSize = True
        Me.Label198.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label198.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label198.Location = New System.Drawing.Point(0, 90)
        Me.Label198.Name = "Label198"
        Me.Label198.Size = New System.Drawing.Size(100, 17)
        Me.Label198.TabIndex = 13
        Me.Label198.Text = "[Ctl+J] JND"
        '
        'Label166
        '
        Me.Label166.AutoSize = True
        Me.Label166.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label166.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label166.Location = New System.Drawing.Point(182, 16)
        Me.Label166.Name = "Label166"
        Me.Label166.Size = New System.Drawing.Size(52, 17)
        Me.Label166.TabIndex = 13
        Me.Label166.Text = "Ctl+A"
        '
        'Label185
        '
        Me.Label185.AutoSize = True
        Me.Label185.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label185.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label185.Location = New System.Drawing.Point(6, 45)
        Me.Label185.Name = "Label185"
        Me.Label185.Size = New System.Drawing.Size(60, 17)
        Me.Label185.TabIndex = 13
        Me.Label185.Text = "Ctl+F3"
        '
        'Label197
        '
        Me.Label197.AutoSize = True
        Me.Label197.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label197.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label197.Location = New System.Drawing.Point(182, 30)
        Me.Label197.Name = "Label197"
        Me.Label197.Size = New System.Drawing.Size(52, 17)
        Me.Label197.TabIndex = 13
        Me.Label197.Text = "Ctl+B"
        '
        'Label165
        '
        Me.Label165.AutoSize = True
        Me.Label165.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label165.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label165.Location = New System.Drawing.Point(79, 90)
        Me.Label165.Name = "Label165"
        Me.Label165.Size = New System.Drawing.Size(107, 17)
        Me.Label165.TabIndex = 13
        Me.Label165.Text = "[Ctl+O] Misc"
        '
        'Label184
        '
        Me.Label184.AutoSize = True
        Me.Label184.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label184.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label184.Location = New System.Drawing.Point(6, 30)
        Me.Label184.Name = "Label184"
        Me.Label184.Size = New System.Drawing.Size(60, 17)
        Me.Label184.TabIndex = 13
        Me.Label184.Text = "Ctl+F2"
        '
        'Label196
        '
        Me.Label196.AutoSize = True
        Me.Label196.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label196.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label196.Location = New System.Drawing.Point(182, 75)
        Me.Label196.Name = "Label196"
        Me.Label196.Size = New System.Drawing.Size(51, 17)
        Me.Label196.TabIndex = 13
        Me.Label196.Text = "Ctl+P"
        '
        'Label164
        '
        Me.Label164.AutoSize = True
        Me.Label164.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label164.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label164.Location = New System.Drawing.Point(6, 75)
        Me.Label164.Name = "Label164"
        Me.Label164.Size = New System.Drawing.Size(54, 17)
        Me.Label164.TabIndex = 13
        Me.Label164.Text = "Ctl+M"
        '
        'Label183
        '
        Me.Label183.AutoSize = True
        Me.Label183.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label183.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label183.Location = New System.Drawing.Point(6, 16)
        Me.Label183.Name = "Label183"
        Me.Label183.Size = New System.Drawing.Size(60, 17)
        Me.Label183.TabIndex = 13
        Me.Label183.Text = "Ctl+F1"
        '
        'Label163
        '
        Me.Label163.AutoSize = True
        Me.Label163.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label163.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label163.Location = New System.Drawing.Point(182, 45)
        Me.Label163.Name = "Label163"
        Me.Label163.Size = New System.Drawing.Size(51, 17)
        Me.Label163.TabIndex = 13
        Me.Label163.Text = "Ctl+S"
        '
        'Label162
        '
        Me.Label162.AutoSize = True
        Me.Label162.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label162.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label162.Location = New System.Drawing.Point(182, 60)
        Me.Label162.Name = "Label162"
        Me.Label162.Size = New System.Drawing.Size(52, 17)
        Me.Label162.TabIndex = 13
        Me.Label162.Text = "Ctl+R"
        '
        'Label161
        '
        Me.Label161.AutoSize = True
        Me.Label161.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label161.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label161.Location = New System.Drawing.Point(6, 60)
        Me.Label161.Name = "Label161"
        Me.Label161.Size = New System.Drawing.Size(60, 17)
        Me.Label161.TabIndex = 29
        Me.Label161.Text = "Ctl+F9"
        '
        'Label182
        '
        Me.Label182.AutoSize = True
        Me.Label182.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label182.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label182.Location = New System.Drawing.Point(50, 44)
        Me.Label182.Name = "Label182"
        Me.Label182.Size = New System.Drawing.Size(16, 17)
        Me.Label182.TabIndex = 32
        Me.Label182.Text = "]"
        '
        'Label193
        '
        Me.Label193.AutoSize = True
        Me.Label193.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label193.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label193.Location = New System.Drawing.Point(50, 74)
        Me.Label193.Name = "Label193"
        Me.Label193.Size = New System.Drawing.Size(16, 17)
        Me.Label193.TabIndex = 32
        Me.Label193.Text = "]"
        '
        'Label158
        '
        Me.Label158.AutoSize = True
        Me.Label158.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label158.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label158.Location = New System.Drawing.Point(219, 74)
        Me.Label158.Name = "Label158"
        Me.Label158.Size = New System.Drawing.Size(16, 17)
        Me.Label158.TabIndex = 32
        Me.Label158.Text = "]"
        '
        'Label181
        '
        Me.Label181.AutoSize = True
        Me.Label181.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label181.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label181.Location = New System.Drawing.Point(-1, 44)
        Me.Label181.Name = "Label181"
        Me.Label181.Size = New System.Drawing.Size(16, 17)
        Me.Label181.TabIndex = 32
        Me.Label181.Text = "["
        '
        'Label192
        '
        Me.Label192.AutoSize = True
        Me.Label192.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label192.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label192.Location = New System.Drawing.Point(-1, 74)
        Me.Label192.Name = "Label192"
        Me.Label192.Size = New System.Drawing.Size(16, 17)
        Me.Label192.TabIndex = 32
        Me.Label192.Text = "["
        '
        'Label157
        '
        Me.Label157.AutoSize = True
        Me.Label157.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label157.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label157.Location = New System.Drawing.Point(175, 74)
        Me.Label157.Name = "Label157"
        Me.Label157.Size = New System.Drawing.Size(16, 17)
        Me.Label157.TabIndex = 32
        Me.Label157.Text = "["
        '
        'Label180
        '
        Me.Label180.AutoSize = True
        Me.Label180.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label180.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label180.Location = New System.Drawing.Point(50, 29)
        Me.Label180.Name = "Label180"
        Me.Label180.Size = New System.Drawing.Size(16, 17)
        Me.Label180.TabIndex = 32
        Me.Label180.Text = "]"
        '
        'Label190
        '
        Me.Label190.AutoSize = True
        Me.Label190.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label190.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label190.Location = New System.Drawing.Point(50, 59)
        Me.Label190.Name = "Label190"
        Me.Label190.Size = New System.Drawing.Size(16, 17)
        Me.Label190.TabIndex = 32
        Me.Label190.Text = "]"
        '
        'Label156
        '
        Me.Label156.AutoSize = True
        Me.Label156.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label156.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label156.Location = New System.Drawing.Point(219, 59)
        Me.Label156.Name = "Label156"
        Me.Label156.Size = New System.Drawing.Size(16, 17)
        Me.Label156.TabIndex = 32
        Me.Label156.Text = "]"
        '
        'Label179
        '
        Me.Label179.AutoSize = True
        Me.Label179.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label179.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label179.Location = New System.Drawing.Point(-1, 29)
        Me.Label179.Name = "Label179"
        Me.Label179.Size = New System.Drawing.Size(16, 17)
        Me.Label179.TabIndex = 32
        Me.Label179.Text = "["
        '
        'Label189
        '
        Me.Label189.AutoSize = True
        Me.Label189.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label189.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label189.Location = New System.Drawing.Point(-1, 59)
        Me.Label189.Name = "Label189"
        Me.Label189.Size = New System.Drawing.Size(16, 17)
        Me.Label189.TabIndex = 32
        Me.Label189.Text = "["
        '
        'Label155
        '
        Me.Label155.AutoSize = True
        Me.Label155.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label155.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label155.Location = New System.Drawing.Point(175, 59)
        Me.Label155.Name = "Label155"
        Me.Label155.Size = New System.Drawing.Size(16, 17)
        Me.Label155.TabIndex = 32
        Me.Label155.Text = "["
        '
        'Label178
        '
        Me.Label178.AutoSize = True
        Me.Label178.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label178.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label178.Location = New System.Drawing.Point(51, 15)
        Me.Label178.Name = "Label178"
        Me.Label178.Size = New System.Drawing.Size(16, 17)
        Me.Label178.TabIndex = 32
        Me.Label178.Text = "]"
        '
        'Label154
        '
        Me.Label154.AutoSize = True
        Me.Label154.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label154.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label154.Location = New System.Drawing.Point(219, 45)
        Me.Label154.Name = "Label154"
        Me.Label154.Size = New System.Drawing.Size(16, 17)
        Me.Label154.TabIndex = 32
        Me.Label154.Text = "]"
        '
        'Label175
        '
        Me.Label175.AutoSize = True
        Me.Label175.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label175.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label175.Location = New System.Drawing.Point(-1, 15)
        Me.Label175.Name = "Label175"
        Me.Label175.Size = New System.Drawing.Size(16, 17)
        Me.Label175.TabIndex = 32
        Me.Label175.Text = "["
        '
        'Label153
        '
        Me.Label153.AutoSize = True
        Me.Label153.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label153.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label153.Location = New System.Drawing.Point(175, 45)
        Me.Label153.Name = "Label153"
        Me.Label153.Size = New System.Drawing.Size(16, 17)
        Me.Label153.TabIndex = 32
        Me.Label153.Text = "["
        '
        'Label152
        '
        Me.Label152.AutoSize = True
        Me.Label152.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label152.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label152.Location = New System.Drawing.Point(219, 29)
        Me.Label152.Name = "Label152"
        Me.Label152.Size = New System.Drawing.Size(16, 17)
        Me.Label152.TabIndex = 32
        Me.Label152.Text = "]"
        '
        'Label151
        '
        Me.Label151.AutoSize = True
        Me.Label151.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label151.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label151.Location = New System.Drawing.Point(175, 29)
        Me.Label151.Name = "Label151"
        Me.Label151.Size = New System.Drawing.Size(16, 17)
        Me.Label151.TabIndex = 32
        Me.Label151.Text = "["
        '
        'Label150
        '
        Me.Label150.AutoSize = True
        Me.Label150.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label150.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label150.Location = New System.Drawing.Point(219, 15)
        Me.Label150.Name = "Label150"
        Me.Label150.Size = New System.Drawing.Size(16, 17)
        Me.Label150.TabIndex = 32
        Me.Label150.Text = "]"
        '
        'Label149
        '
        Me.Label149.AutoSize = True
        Me.Label149.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label149.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label149.Location = New System.Drawing.Point(175, 15)
        Me.Label149.Name = "Label149"
        Me.Label149.Size = New System.Drawing.Size(16, 17)
        Me.Label149.TabIndex = 32
        Me.Label149.Text = "["
        '
        'Label48
        '
        Me.Label48.AutoSize = True
        Me.Label48.BackColor = System.Drawing.Color.Transparent
        Me.Label48.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label48.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label48.Location = New System.Drawing.Point(228, 16)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(71, 17)
        Me.Label48.TabIndex = 30
        Me.Label48.Text = "Address"
        '
        'Label186
        '
        Me.Label186.AutoSize = True
        Me.Label186.BackColor = System.Drawing.Color.Transparent
        Me.Label186.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label186.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label186.Location = New System.Drawing.Point(60, 16)
        Me.Label186.Name = "Label186"
        Me.Label186.Size = New System.Drawing.Size(125, 17)
        Me.Label186.TabIndex = 14
        Me.Label186.Text = "Other State Bill"
        '
        'Label187
        '
        Me.Label187.AutoSize = True
        Me.Label187.BackColor = System.Drawing.Color.Transparent
        Me.Label187.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label187.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label187.Location = New System.Drawing.Point(60, 30)
        Me.Label187.Name = "Label187"
        Me.Label187.Size = New System.Drawing.Size(120, 17)
        Me.Label187.TabIndex = 14
        Me.Label187.Text = "Sales Discount"
        '
        'chkPartlySales
        '
        Me.chkPartlySales.AutoSize = True
        Me.chkPartlySales.BackColor = System.Drawing.Color.Transparent
        Me.chkPartlySales.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkPartlySales.Enabled = False
        Me.chkPartlySales.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPartlySales.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.chkPartlySales.Location = New System.Drawing.Point(228, 75)
        Me.chkPartlySales.Name = "chkPartlySales"
        Me.chkPartlySales.Size = New System.Drawing.Size(120, 21)
        Me.chkPartlySales.TabIndex = 0
        Me.chkPartlySales.Text = "Partly Sales"
        Me.chkPartlySales.UseVisualStyleBackColor = False
        '
        'btnExit
        '
        Me.btnExit.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.Location = New System.Drawing.Point(447, 76)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(127, 30)
        Me.btnExit.TabIndex = 12
        Me.btnExit.Text = "E&xit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNew.Location = New System.Drawing.Point(447, 46)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(127, 30)
        Me.btnNew.TabIndex = 11
        Me.btnNew.Text = "&New"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'Label204
        '
        Me.Label204.AutoSize = True
        Me.Label204.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label204.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label204.Location = New System.Drawing.Point(60, 75)
        Me.Label204.Name = "Label204"
        Me.Label204.Size = New System.Drawing.Size(139, 17)
        Me.Label204.TabIndex = 13
        Me.Label204.Text = "Multi Metal Detail"
        '
        'Label203
        '
        Me.Label203.AutoSize = True
        Me.Label203.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label203.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label203.Location = New System.Drawing.Point(60, 60)
        Me.Label203.Name = "Label203"
        Me.Label203.Size = New System.Drawing.Size(126, 17)
        Me.Label203.TabIndex = 29
        Me.Label203.Text = "Wast%|McGrm"
        '
        'Label202
        '
        Me.Label202.AutoSize = True
        Me.Label202.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label202.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label202.Location = New System.Drawing.Point(228, 46)
        Me.Label202.Name = "Label202"
        Me.Label202.Size = New System.Drawing.Size(101, 17)
        Me.Label202.TabIndex = 13
        Me.Label202.Text = "Stone Detail"
        '
        'Label201
        '
        Me.Label201.AutoSize = True
        Me.Label201.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label201.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label201.Location = New System.Drawing.Point(228, 60)
        Me.Label201.Name = "Label201"
        Me.Label201.Size = New System.Drawing.Size(107, 17)
        Me.Label201.TabIndex = 13
        Me.Label201.Text = "Rate Change"
        '
        'lblBookedItem
        '
        Me.lblBookedItem.AutoSize = True
        Me.lblBookedItem.BackColor = System.Drawing.Color.Transparent
        Me.lblBookedItem.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBookedItem.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.lblBookedItem.Location = New System.Drawing.Point(228, 30)
        Me.lblBookedItem.Name = "lblBookedItem"
        Me.lblBookedItem.Size = New System.Drawing.Size(109, 17)
        Me.lblBookedItem.TabIndex = 14
        Me.lblBookedItem.Text = "Booked Item"
        '
        'btnSave
        '
        Me.btnSave.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(447, 16)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(127, 30)
        Me.btnSave.TabIndex = 10
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'Label188
        '
        Me.Label188.AutoSize = True
        Me.Label188.BackColor = System.Drawing.Color.Transparent
        Me.Label188.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label188.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label188.Location = New System.Drawing.Point(60, 45)
        Me.Label188.Name = "Label188"
        Me.Label188.Size = New System.Drawing.Size(115, 17)
        Me.Label188.TabIndex = 14
        Me.Label188.Text = "Purchase Disc"
        '
        'grpGeneral
        '
        Me.grpGeneral.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpGeneral.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpGeneral.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpGeneral.BorderColor = System.Drawing.Color.Transparent
        Me.grpGeneral.BorderThickness = 1.0!
        Me.grpGeneral.Controls.Add(Me.lblHallmarkNo)
        Me.grpGeneral.Controls.Add(Me.txtDetVAMin)
        Me.grpGeneral.Controls.Add(Me.txtDetVAMax)
        Me.grpGeneral.Controls.Add(Me.Label141)
        Me.grpGeneral.Controls.Add(Me.txtDetTagGrsNet)
        Me.grpGeneral.Controls.Add(Me.Label139)
        Me.grpGeneral.Controls.Add(Me.txtDetDesigner)
        Me.grpGeneral.Controls.Add(Me.Label76)
        Me.grpGeneral.Controls.Add(Me.txtDifftot)
        Me.grpGeneral.Controls.Add(Me.txtSASurCharge)
        Me.grpGeneral.Controls.Add(Me.Label75)
        Me.grpGeneral.Controls.Add(Me.txtDetRateId)
        Me.grpGeneral.Controls.Add(Me.Label72)
        Me.grpGeneral.Controls.Add(Me.txtDetItemType)
        Me.grpGeneral.Controls.Add(Me.txtDetWastagePer)
        Me.grpGeneral.Controls.Add(Me.txtDetGrsNet)
        Me.grpGeneral.Controls.Add(Me.Label127)
        Me.grpGeneral.Controls.Add(Me.txtDetDiffGrsNet)
        Me.grpGeneral.Controls.Add(Me.txtDetTableCode)
        Me.grpGeneral.Controls.Add(Me.txtSAVatPer_PER)
        Me.grpGeneral.Controls.Add(Me.txtDetDiffDia)
        Me.grpGeneral.Controls.Add(Me.txtDetCounter)
        Me.grpGeneral.Controls.Add(Me.txtDetSubItem)
        Me.grpGeneral.Controls.Add(Me.txtDetItem)
        Me.grpGeneral.Controls.Add(Me.txtDetValueAdded)
        Me.grpGeneral.Controls.Add(Me.Label64)
        Me.grpGeneral.Controls.Add(Me.txtDetDiscount)
        Me.grpGeneral.Controls.Add(Me.txtDetCalcType)
        Me.grpGeneral.Controls.Add(Me.txtDetMiscAmt)
        Me.grpGeneral.Controls.Add(Me.txtDetMcPerGrm)
        Me.grpGeneral.Controls.Add(Me.txtDetLessWt)
        Me.grpGeneral.Controls.Add(Me.txtDetStockType)
        Me.grpGeneral.Controls.Add(Me.Label71)
        Me.grpGeneral.Controls.Add(Me.Label133)
        Me.grpGeneral.Controls.Add(Me.Label134)
        Me.grpGeneral.Controls.Add(Me.lblDetVat)
        Me.grpGeneral.Controls.Add(Me.Label121)
        Me.grpGeneral.Controls.Add(Me.Label125)
        Me.grpGeneral.Controls.Add(Me.Label118)
        Me.grpGeneral.Controls.Add(Me.Label117)
        Me.grpGeneral.Controls.Add(Me.Label116)
        Me.grpGeneral.Controls.Add(Me.Label115)
        Me.grpGeneral.Controls.Add(Me.Label114)
        Me.grpGeneral.Controls.Add(Me.Label113)
        Me.grpGeneral.Controls.Add(Me.Label112)
        Me.grpGeneral.Controls.Add(Me.Label111)
        Me.grpGeneral.Controls.Add(Me.Label61)
        Me.grpGeneral.Controls.Add(Me.Label50)
        Me.grpGeneral.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpGeneral.GroupImage = Nothing
        Me.grpGeneral.GroupTitle = ""
        Me.grpGeneral.Location = New System.Drawing.Point(11, -5)
        Me.grpGeneral.Name = "grpGeneral"
        Me.grpGeneral.Padding = New System.Windows.Forms.Padding(20)
        Me.grpGeneral.PaintGroupBox = False
        Me.grpGeneral.RoundCorners = 10
        Me.grpGeneral.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpGeneral.ShadowControl = False
        Me.grpGeneral.ShadowThickness = 3
        Me.grpGeneral.Size = New System.Drawing.Size(407, 185)
        Me.grpGeneral.TabIndex = 0
        '
        'lblHallmarkNo
        '
        Me.lblHallmarkNo.AutoSize = True
        Me.lblHallmarkNo.Font = New System.Drawing.Font("Arial", 7.0!, System.Drawing.FontStyle.Bold)
        Me.lblHallmarkNo.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.lblHallmarkNo.Location = New System.Drawing.Point(231, 168)
        Me.lblHallmarkNo.Name = "lblHallmarkNo"
        Me.lblHallmarkNo.Size = New System.Drawing.Size(79, 15)
        Me.lblHallmarkNo.TabIndex = 45
        Me.lblHallmarkNo.Text = "HallmarkNo :"
        Me.lblHallmarkNo.Visible = False
        '
        'txtDetVAMin
        '
        Me.txtDetVAMin.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetVAMin.ForeColor = System.Drawing.Color.Red
        Me.txtDetVAMin.Location = New System.Drawing.Point(150, 155)
        Me.txtDetVAMin.Name = "txtDetVAMin"
        Me.txtDetVAMin.Size = New System.Drawing.Size(77, 24)
        Me.txtDetVAMin.TabIndex = 41
        '
        'txtDetVAMax
        '
        Me.txtDetVAMax.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetVAMax.ForeColor = System.Drawing.Color.Red
        Me.txtDetVAMax.Location = New System.Drawing.Point(85, 155)
        Me.txtDetVAMax.Name = "txtDetVAMax"
        Me.txtDetVAMax.Size = New System.Drawing.Size(63, 24)
        Me.txtDetVAMax.TabIndex = 40
        '
        'Label141
        '
        Me.Label141.AutoSize = True
        Me.Label141.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label141.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label141.Location = New System.Drawing.Point(-1, 158)
        Me.Label141.Name = "Label141"
        Me.Label141.Size = New System.Drawing.Size(98, 17)
        Me.Label141.TabIndex = 42
        Me.Label141.Text = "VA Max,Min"
        '
        'txtDetTagGrsNet
        '
        Me.txtDetTagGrsNet.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetTagGrsNet.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.txtDetTagGrsNet.Location = New System.Drawing.Point(314, 136)
        Me.txtDetTagGrsNet.Name = "txtDetTagGrsNet"
        Me.txtDetTagGrsNet.Size = New System.Drawing.Size(84, 21)
        Me.txtDetTagGrsNet.TabIndex = 38
        Me.txtDetTagGrsNet.Text = "112.000"
        '
        'Label139
        '
        Me.Label139.AutoSize = True
        Me.Label139.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label139.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label139.Location = New System.Drawing.Point(229, 139)
        Me.Label139.Name = "Label139"
        Me.Label139.Size = New System.Drawing.Size(101, 17)
        Me.Label139.TabIndex = 39
        Me.Label139.Text = "Tag G/N Wt"
        '
        'txtDetDesigner
        '
        Me.txtDetDesigner.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetDesigner.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.txtDetDesigner.Location = New System.Drawing.Point(85, 65)
        Me.txtDetDesigner.Name = "txtDetDesigner"
        Me.txtDetDesigner.Size = New System.Drawing.Size(141, 21)
        Me.txtDetDesigner.TabIndex = 36
        '
        'Label76
        '
        Me.Label76.AutoSize = True
        Me.Label76.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label76.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label76.Location = New System.Drawing.Point(-1, 68)
        Me.Label76.Name = "Label76"
        Me.Label76.Size = New System.Drawing.Size(76, 17)
        Me.Label76.TabIndex = 37
        Me.Label76.Text = "Designer"
        '
        'txtDifftot
        '
        Me.txtDifftot.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDifftot.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.txtDifftot.Location = New System.Drawing.Point(314, 154)
        Me.txtDifftot.Name = "txtDifftot"
        Me.txtDifftot.Size = New System.Drawing.Size(84, 21)
        Me.txtDifftot.TabIndex = 35
        Me.txtDifftot.Text = "112.000"
        Me.txtDifftot.Visible = False
        '
        'txtSASurCharge
        '
        Me.txtSASurCharge.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSASurCharge.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.txtSASurCharge.Location = New System.Drawing.Point(314, 155)
        Me.txtSASurCharge.Name = "txtSASurCharge"
        Me.txtSASurCharge.Size = New System.Drawing.Size(84, 21)
        Me.txtSASurCharge.TabIndex = 33
        '
        'Label75
        '
        Me.Label75.AutoSize = True
        Me.Label75.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label75.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label75.Location = New System.Drawing.Point(229, 154)
        Me.Label75.Name = "Label75"
        Me.Label75.Size = New System.Drawing.Size(73, 17)
        Me.Label75.TabIndex = 34
        Me.Label75.Text = "SurChrg"
        '
        'txtDetRateId
        '
        Me.txtDetRateId.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetRateId.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.txtDetRateId.Location = New System.Drawing.Point(314, 81)
        Me.txtDetRateId.Name = "txtDetRateId"
        Me.txtDetRateId.Size = New System.Drawing.Size(84, 21)
        Me.txtDetRateId.TabIndex = 31
        '
        'Label72
        '
        Me.Label72.AutoSize = True
        Me.Label72.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label72.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label72.Location = New System.Drawing.Point(229, 84)
        Me.Label72.Name = "Label72"
        Me.Label72.Size = New System.Drawing.Size(66, 17)
        Me.Label72.TabIndex = 32
        Me.Label72.Text = "Rate Id"
        '
        'txtDetItemType
        '
        Me.txtDetItemType.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetItemType.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.txtDetItemType.Location = New System.Drawing.Point(85, 83)
        Me.txtDetItemType.Name = "txtDetItemType"
        Me.txtDetItemType.Size = New System.Drawing.Size(63, 21)
        Me.txtDetItemType.TabIndex = 0
        '
        'txtDetWastagePer
        '
        Me.txtDetWastagePer.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetWastagePer.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.txtDetWastagePer.Location = New System.Drawing.Point(196, 83)
        Me.txtDetWastagePer.Name = "txtDetWastagePer"
        Me.txtDetWastagePer.Size = New System.Drawing.Size(31, 21)
        Me.txtDetWastagePer.TabIndex = 0
        '
        'txtDetGrsNet
        '
        Me.txtDetGrsNet.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetGrsNet.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.txtDetGrsNet.Location = New System.Drawing.Point(196, 101)
        Me.txtDetGrsNet.Name = "txtDetGrsNet"
        Me.txtDetGrsNet.Size = New System.Drawing.Size(31, 21)
        Me.txtDetGrsNet.TabIndex = 0
        '
        'Label127
        '
        Me.Label127.AutoSize = True
        Me.Label127.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label127.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label127.Location = New System.Drawing.Point(147, 86)
        Me.Label127.Name = "Label127"
        Me.Label127.Size = New System.Drawing.Size(65, 17)
        Me.Label127.TabIndex = 28
        Me.Label127.Text = "Wast%"
        '
        'txtDetDiffGrsNet
        '
        Me.txtDetDiffGrsNet.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetDiffGrsNet.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.txtDetDiffGrsNet.Location = New System.Drawing.Point(314, 117)
        Me.txtDetDiffGrsNet.Name = "txtDetDiffGrsNet"
        Me.txtDetDiffGrsNet.Size = New System.Drawing.Size(84, 21)
        Me.txtDetDiffGrsNet.TabIndex = 0
        Me.txtDetDiffGrsNet.Text = "112.000"
        '
        'txtDetTableCode
        '
        Me.txtDetTableCode.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetTableCode.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.txtDetTableCode.Location = New System.Drawing.Point(196, 119)
        Me.txtDetTableCode.Name = "txtDetTableCode"
        Me.txtDetTableCode.Size = New System.Drawing.Size(31, 21)
        Me.txtDetTableCode.TabIndex = 0
        '
        'txtDetDiffDia
        '
        Me.txtDetDiffDia.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetDiffDia.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.txtDetDiffDia.Location = New System.Drawing.Point(314, 99)
        Me.txtDetDiffDia.Name = "txtDetDiffDia"
        Me.txtDetDiffDia.Size = New System.Drawing.Size(84, 21)
        Me.txtDetDiffDia.TabIndex = 0
        '
        'txtDetCounter
        '
        Me.txtDetCounter.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetCounter.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.txtDetCounter.Location = New System.Drawing.Point(85, 47)
        Me.txtDetCounter.Name = "txtDetCounter"
        Me.txtDetCounter.Size = New System.Drawing.Size(141, 21)
        Me.txtDetCounter.TabIndex = 0
        '
        'txtDetSubItem
        '
        Me.txtDetSubItem.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetSubItem.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.txtDetSubItem.Location = New System.Drawing.Point(85, 29)
        Me.txtDetSubItem.Name = "txtDetSubItem"
        Me.txtDetSubItem.Size = New System.Drawing.Size(141, 21)
        Me.txtDetSubItem.TabIndex = 0
        '
        'txtDetItem
        '
        Me.txtDetItem.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetItem.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.txtDetItem.Location = New System.Drawing.Point(85, 11)
        Me.txtDetItem.Name = "txtDetItem"
        Me.txtDetItem.Size = New System.Drawing.Size(141, 21)
        Me.txtDetItem.TabIndex = 0
        Me.txtDetItem.Text = "Item"
        '
        'txtDetValueAdded
        '
        Me.txtDetValueAdded.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetValueAdded.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.txtDetValueAdded.Location = New System.Drawing.Point(85, 119)
        Me.txtDetValueAdded.Name = "txtDetValueAdded"
        Me.txtDetValueAdded.Size = New System.Drawing.Size(63, 21)
        Me.txtDetValueAdded.TabIndex = 0
        '
        'Label64
        '
        Me.Label64.AutoSize = True
        Me.Label64.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label64.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label64.Location = New System.Drawing.Point(288, 120)
        Me.Label64.Name = "Label64"
        Me.Label64.Size = New System.Drawing.Size(28, 17)
        Me.Label64.TabIndex = 28
        Me.Label64.Text = "PS"
        '
        'txtDetDiscount
        '
        Me.txtDetDiscount.Font = New System.Drawing.Font("Arial", 6.75!, System.Drawing.FontStyle.Bold)
        Me.txtDetDiscount.Location = New System.Drawing.Point(314, 27)
        Me.txtDetDiscount.Name = "txtDetDiscount"
        Me.txtDetDiscount.Size = New System.Drawing.Size(84, 20)
        Me.txtDetDiscount.TabIndex = 30
        '
        'txtDetCalcType
        '
        Me.txtDetCalcType.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetCalcType.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.txtDetCalcType.Location = New System.Drawing.Point(85, 101)
        Me.txtDetCalcType.Name = "txtDetCalcType"
        Me.txtDetCalcType.Size = New System.Drawing.Size(63, 21)
        Me.txtDetCalcType.TabIndex = 0
        '
        'txtDetMiscAmt
        '
        Me.txtDetMiscAmt.Font = New System.Drawing.Font("Arial", 6.75!, System.Drawing.FontStyle.Bold)
        Me.txtDetMiscAmt.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.txtDetMiscAmt.Location = New System.Drawing.Point(314, 45)
        Me.txtDetMiscAmt.Name = "txtDetMiscAmt"
        Me.txtDetMiscAmt.Size = New System.Drawing.Size(84, 20)
        Me.txtDetMiscAmt.TabIndex = 0
        '
        'txtDetMcPerGrm
        '
        Me.txtDetMcPerGrm.Font = New System.Drawing.Font("Arial", 6.75!, System.Drawing.FontStyle.Bold)
        Me.txtDetMcPerGrm.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.txtDetMcPerGrm.Location = New System.Drawing.Point(314, 63)
        Me.txtDetMcPerGrm.Name = "txtDetMcPerGrm"
        Me.txtDetMcPerGrm.Size = New System.Drawing.Size(84, 20)
        Me.txtDetMcPerGrm.TabIndex = 0
        '
        'txtDetLessWt
        '
        Me.txtDetLessWt.Font = New System.Drawing.Font("Arial", 6.75!, System.Drawing.FontStyle.Bold)
        Me.txtDetLessWt.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.txtDetLessWt.Location = New System.Drawing.Point(314, 10)
        Me.txtDetLessWt.Name = "txtDetLessWt"
        Me.txtDetLessWt.Size = New System.Drawing.Size(84, 20)
        Me.txtDetLessWt.TabIndex = 0
        '
        'txtDetStockType
        '
        Me.txtDetStockType.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDetStockType.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.txtDetStockType.Location = New System.Drawing.Point(85, 137)
        Me.txtDetStockType.Name = "txtDetStockType"
        Me.txtDetStockType.Size = New System.Drawing.Size(63, 21)
        Me.txtDetStockType.TabIndex = 0
        '
        'Label71
        '
        Me.Label71.AutoSize = True
        Me.Label71.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label71.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label71.Location = New System.Drawing.Point(229, 66)
        Me.Label71.Name = "Label71"
        Me.Label71.Size = New System.Drawing.Size(71, 17)
        Me.Label71.TabIndex = 28
        Me.Label71.Text = "Mc/Grm"
        '
        'Label133
        '
        Me.Label133.AutoSize = True
        Me.Label133.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label133.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label133.Location = New System.Drawing.Point(147, 104)
        Me.Label133.Name = "Label133"
        Me.Label133.Size = New System.Drawing.Size(62, 17)
        Me.Label133.TabIndex = 25
        Me.Label133.Text = "Gr/Net"
        '
        'Label134
        '
        Me.Label134.AutoSize = True
        Me.Label134.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label134.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label134.Location = New System.Drawing.Point(229, 13)
        Me.Label134.Name = "Label134"
        Me.Label134.Size = New System.Drawing.Size(64, 17)
        Me.Label134.TabIndex = 26
        Me.Label134.Text = "LessWt"
        '
        'lblDetVat
        '
        Me.lblDetVat.AutoSize = True
        Me.lblDetVat.BackColor = System.Drawing.Color.Transparent
        Me.lblDetVat.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDetVat.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.lblDetVat.Location = New System.Drawing.Point(147, 140)
        Me.lblDetVat.Name = "lblDetVat"
        Me.lblDetVat.Size = New System.Drawing.Size(52, 17)
        Me.lblDetVat.TabIndex = 24
        Me.lblDetVat.Text = "Vat%"
        Me.lblDetVat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label121
        '
        Me.Label121.AutoSize = True
        Me.Label121.BackColor = System.Drawing.Color.Transparent
        Me.Label121.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label121.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label121.Location = New System.Drawing.Point(229, 30)
        Me.Label121.Name = "Label121"
        Me.Label121.Size = New System.Drawing.Size(75, 17)
        Me.Label121.TabIndex = 24
        Me.Label121.Text = "Discount"
        Me.Label121.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label125
        '
        Me.Label125.AutoSize = True
        Me.Label125.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label125.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label125.Location = New System.Drawing.Point(229, 48)
        Me.Label125.Name = "Label125"
        Me.Label125.Size = New System.Drawing.Size(107, 17)
        Me.Label125.TabIndex = 21
        Me.Label125.Text = "Misc Amount"
        '
        'Label118
        '
        Me.Label118.AutoSize = True
        Me.Label118.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label118.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label118.Location = New System.Drawing.Point(-1, 104)
        Me.Label118.Name = "Label118"
        Me.Label118.Size = New System.Drawing.Size(83, 17)
        Me.Label118.TabIndex = 1
        Me.Label118.Text = "Calc Type"
        '
        'Label117
        '
        Me.Label117.AutoSize = True
        Me.Label117.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label117.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label117.Location = New System.Drawing.Point(-1, 140)
        Me.Label117.Name = "Label117"
        Me.Label117.Size = New System.Drawing.Size(94, 17)
        Me.Label117.TabIndex = 1
        Me.Label117.Text = "Stock Type"
        '
        'Label116
        '
        Me.Label116.AutoSize = True
        Me.Label116.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label116.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label116.Location = New System.Drawing.Point(147, 122)
        Me.Label116.Name = "Label116"
        Me.Label116.Size = New System.Drawing.Size(50, 17)
        Me.Label116.TabIndex = 1
        Me.Label116.Text = "Table"
        Me.ToolTip1.SetToolTip(Me.Label116, "Table Code")
        '
        'Label115
        '
        Me.Label115.AutoSize = True
        Me.Label115.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label115.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label115.Location = New System.Drawing.Point(-1, 122)
        Me.Label115.Name = "Label115"
        Me.Label115.Size = New System.Drawing.Size(106, 17)
        Me.Label115.TabIndex = 1
        Me.Label115.Text = "Value Added"
        '
        'Label114
        '
        Me.Label114.AutoSize = True
        Me.Label114.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label114.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label114.Location = New System.Drawing.Point(-1, 86)
        Me.Label114.Name = "Label114"
        Me.Label114.Size = New System.Drawing.Size(88, 17)
        Me.Label114.TabIndex = 1
        Me.Label114.Text = "Item Type"
        '
        'Label113
        '
        Me.Label113.AutoSize = True
        Me.Label113.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label113.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label113.Location = New System.Drawing.Point(-1, 50)
        Me.Label113.Name = "Label113"
        Me.Label113.Size = New System.Drawing.Size(71, 17)
        Me.Label113.TabIndex = 1
        Me.Label113.Text = "Counter"
        '
        'Label112
        '
        Me.Label112.AutoSize = True
        Me.Label112.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label112.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label112.Location = New System.Drawing.Point(-1, 32)
        Me.Label112.Name = "Label112"
        Me.Label112.Size = New System.Drawing.Size(75, 17)
        Me.Label112.TabIndex = 1
        Me.Label112.Text = "SubItem"
        '
        'Label111
        '
        Me.Label111.AutoSize = True
        Me.Label111.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label111.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label111.Location = New System.Drawing.Point(-1, 14)
        Me.Label111.Name = "Label111"
        Me.Label111.Size = New System.Drawing.Size(45, 17)
        Me.Label111.TabIndex = 1
        Me.Label111.Text = "Item"
        '
        'Label61
        '
        Me.Label61.AutoSize = True
        Me.Label61.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label61.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label61.Location = New System.Drawing.Point(229, 120)
        Me.Label61.Name = "Label61"
        Me.Label61.Size = New System.Drawing.Size(67, 17)
        Me.Label61.TabIndex = 28
        Me.Label61.Text = "G/N Wt"
        '
        'Label50
        '
        Me.Label50.AutoSize = True
        Me.Label50.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label50.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label50.Location = New System.Drawing.Point(229, 102)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(95, 17)
        Me.Label50.TabIndex = 28
        Me.Label50.Text = "Dia Pcs/Wt"
        '
        'tabHideDet
        '
        Me.tabHideDet.Controls.Add(Me.txtPurAlloy)
        Me.tabHideDet.Controls.Add(Me.grpStudedDetail)
        Me.tabHideDet.Controls.Add(Me.pnlShortCut)
        Me.tabHideDet.Controls.Add(Me.picTagImage)
        Me.tabHideDet.Controls.Add(Me.txtSaDiscountAfterTax)
        Me.tabHideDet.Controls.Add(Me.pnlFinalPURAmount_OWN)
        Me.tabHideDet.Controls.Add(Me.pnlPuExtaDetails)
        Me.tabHideDet.Controls.Add(Me.txtPUStoneWt_WET)
        Me.tabHideDet.Location = New System.Drawing.Point(4, 14)
        Me.tabHideDet.Name = "tabHideDet"
        Me.tabHideDet.Size = New System.Drawing.Size(581, 281)
        Me.tabHideDet.TabIndex = 9
        Me.tabHideDet.Text = "Hide Details"
        Me.tabHideDet.UseVisualStyleBackColor = True
        '
        'txtPurAlloy
        '
        Me.txtPurAlloy.Location = New System.Drawing.Point(320, 204)
        Me.txtPurAlloy.Name = "txtPurAlloy"
        Me.txtPurAlloy.Size = New System.Drawing.Size(90, 24)
        Me.txtPurAlloy.TabIndex = 24
        '
        'grpStudedDetail
        '
        Me.grpStudedDetail.Controls.Add(Me.Label102)
        Me.grpStudedDetail.Controls.Add(Me.lblDetDiaPcs)
        Me.grpStudedDetail.Controls.Add(Me.lblDetStnWt)
        Me.grpStudedDetail.Controls.Add(Me.lblDetStnPcs)
        Me.grpStudedDetail.Controls.Add(Me.lblDetDiaWt)
        Me.grpStudedDetail.Controls.Add(Me.Label35)
        Me.grpStudedDetail.Controls.Add(Me.Label36)
        Me.grpStudedDetail.Controls.Add(Me.Label34)
        Me.grpStudedDetail.Location = New System.Drawing.Point(340, 10)
        Me.grpStudedDetail.Name = "grpStudedDetail"
        Me.grpStudedDetail.Size = New System.Drawing.Size(234, 97)
        Me.grpStudedDetail.TabIndex = 23
        Me.grpStudedDetail.TabStop = False
        Me.grpStudedDetail.Text = "Studded Detail"
        '
        'Label102
        '
        Me.Label102.AutoSize = True
        Me.Label102.BackColor = System.Drawing.Color.Transparent
        Me.Label102.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label102.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label102.Location = New System.Drawing.Point(6, 74)
        Me.Label102.Name = "Label102"
        Me.Label102.Size = New System.Drawing.Size(103, 17)
        Me.Label102.TabIndex = 1
        Me.Label102.Text = "Diamond Wt"
        '
        'lblDetDiaPcs
        '
        Me.lblDetDiaPcs.BackColor = System.Drawing.SystemColors.HighlightText
        Me.lblDetDiaPcs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblDetDiaPcs.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDetDiaPcs.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.lblDetDiaPcs.Location = New System.Drawing.Point(111, 54)
        Me.lblDetDiaPcs.Name = "lblDetDiaPcs"
        Me.lblDetDiaPcs.Size = New System.Drawing.Size(116, 16)
        Me.lblDetDiaPcs.TabIndex = 1
        Me.lblDetDiaPcs.Text = "Label75"
        Me.lblDetDiaPcs.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblDetStnWt
        '
        Me.lblDetStnWt.BackColor = System.Drawing.SystemColors.HighlightText
        Me.lblDetStnWt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblDetStnWt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDetStnWt.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.lblDetStnWt.Location = New System.Drawing.Point(111, 34)
        Me.lblDetStnWt.Name = "lblDetStnWt"
        Me.lblDetStnWt.Size = New System.Drawing.Size(116, 16)
        Me.lblDetStnWt.TabIndex = 1
        Me.lblDetStnWt.Text = "Label75"
        Me.lblDetStnWt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblDetStnPcs
        '
        Me.lblDetStnPcs.BackColor = System.Drawing.SystemColors.HighlightText
        Me.lblDetStnPcs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblDetStnPcs.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDetStnPcs.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.lblDetStnPcs.Location = New System.Drawing.Point(111, 14)
        Me.lblDetStnPcs.Name = "lblDetStnPcs"
        Me.lblDetStnPcs.Size = New System.Drawing.Size(116, 16)
        Me.lblDetStnPcs.TabIndex = 1
        Me.lblDetStnPcs.Text = "Label75"
        Me.lblDetStnPcs.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblDetDiaWt
        '
        Me.lblDetDiaWt.BackColor = System.Drawing.Color.White
        Me.lblDetDiaWt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblDetDiaWt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDetDiaWt.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.lblDetDiaWt.Location = New System.Drawing.Point(111, 74)
        Me.lblDetDiaWt.Name = "lblDetDiaWt"
        Me.lblDetDiaWt.Size = New System.Drawing.Size(116, 16)
        Me.lblDetDiaWt.TabIndex = 0
        Me.lblDetDiaWt.Text = "Dia Wt"
        Me.lblDetDiaWt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label35.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label35.Location = New System.Drawing.Point(6, 34)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(113, 17)
        Me.Label35.TabIndex = 0
        Me.Label35.Text = "Stone Weight"
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label36.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label36.Location = New System.Drawing.Point(6, 54)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(107, 17)
        Me.Label36.TabIndex = 0
        Me.Label36.Text = "Diamond Pcs"
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label34.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label34.Location = New System.Drawing.Point(6, 13)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(84, 17)
        Me.Label34.TabIndex = 0
        Me.Label34.Text = "Stone Pcs"
        '
        'pnlShortCut
        '
        Me.pnlShortCut.Controls.Add(Me.Label135)
        Me.pnlShortCut.Controls.Add(Me.txtShortCut)
        Me.pnlShortCut.Location = New System.Drawing.Point(17, 211)
        Me.pnlShortCut.Name = "pnlShortCut"
        Me.pnlShortCut.Size = New System.Drawing.Size(167, 21)
        Me.pnlShortCut.TabIndex = 16
        '
        'Label135
        '
        Me.Label135.AutoSize = True
        Me.Label135.Location = New System.Drawing.Point(3, 4)
        Me.Label135.Name = "Label135"
        Me.Label135.Size = New System.Drawing.Size(78, 17)
        Me.Label135.TabIndex = 0
        Me.Label135.Text = "&Short Cut"
        '
        'txtShortCut
        '
        Me.txtShortCut.Location = New System.Drawing.Point(67, 0)
        Me.txtShortCut.Name = "txtShortCut"
        Me.txtShortCut.Size = New System.Drawing.Size(100, 24)
        Me.txtShortCut.TabIndex = 1
        '
        'picTagImage
        '
        Me.picTagImage.Image = Global.BrighttechRetailJewellery.My.Resources.Resources.no_photo
        Me.picTagImage.Location = New System.Drawing.Point(15, 13)
        Me.picTagImage.Name = "picTagImage"
        Me.picTagImage.Size = New System.Drawing.Size(108, 92)
        Me.picTagImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picTagImage.TabIndex = 7
        Me.picTagImage.TabStop = False
        '
        'pnlFinalPURAmount_OWN
        '
        Me.pnlFinalPURAmount_OWN.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.pnlFinalPURAmount_OWN.Controls.Add(Me.txtFinalPURAmount_AMT)
        Me.pnlFinalPURAmount_OWN.Controls.Add(Me.Label97)
        Me.pnlFinalPURAmount_OWN.Controls.Add(Me.Label103)
        Me.pnlFinalPURAmount_OWN.Controls.Add(Me.Label104)
        Me.pnlFinalPURAmount_OWN.Controls.Add(Me.Label105)
        Me.pnlFinalPURAmount_OWN.Controls.Add(Me.Label106)
        Me.pnlFinalPURAmount_OWN.Controls.Add(Me.Label107)
        Me.pnlFinalPURAmount_OWN.Location = New System.Drawing.Point(11, 126)
        Me.pnlFinalPURAmount_OWN.Name = "pnlFinalPURAmount_OWN"
        Me.pnlFinalPURAmount_OWN.Size = New System.Drawing.Size(235, 71)
        Me.pnlFinalPURAmount_OWN.TabIndex = 5
        '
        'txtFinalPURAmount_AMT
        '
        Me.txtFinalPURAmount_AMT.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtFinalPURAmount_AMT.Font = New System.Drawing.Font("Verdana", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFinalPURAmount_AMT.Location = New System.Drawing.Point(3, 37)
        Me.txtFinalPURAmount_AMT.Name = "txtFinalPURAmount_AMT"
        Me.txtFinalPURAmount_AMT.Size = New System.Drawing.Size(229, 36)
        Me.txtFinalPURAmount_AMT.TabIndex = 4
        '
        'Label97
        '
        Me.Label97.BackColor = System.Drawing.Color.SteelBlue
        Me.Label97.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label97.Location = New System.Drawing.Point(232, 37)
        Me.Label97.Name = "Label97"
        Me.Label97.Size = New System.Drawing.Size(3, 31)
        Me.Label97.TabIndex = 3
        Me.Label97.Text = "Label101"
        '
        'Label103
        '
        Me.Label103.BackColor = System.Drawing.Color.SteelBlue
        Me.Label103.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Label103.Location = New System.Drawing.Point(3, 68)
        Me.Label103.Name = "Label103"
        Me.Label103.Size = New System.Drawing.Size(232, 3)
        Me.Label103.TabIndex = 3
        Me.Label103.Text = "Label103"
        '
        'Label104
        '
        Me.Label104.BackColor = System.Drawing.Color.SteelBlue
        Me.Label104.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label104.Location = New System.Drawing.Point(3, 34)
        Me.Label104.Name = "Label104"
        Me.Label104.Size = New System.Drawing.Size(232, 3)
        Me.Label104.TabIndex = 2
        Me.Label104.Text = "Label104"
        '
        'Label105
        '
        Me.Label105.BackColor = System.Drawing.Color.SteelBlue
        Me.Label105.Dock = System.Windows.Forms.DockStyle.Left
        Me.Label105.Location = New System.Drawing.Point(0, 34)
        Me.Label105.Name = "Label105"
        Me.Label105.Size = New System.Drawing.Size(3, 37)
        Me.Label105.TabIndex = 1
        Me.Label105.Text = "Label105"
        '
        'Label106
        '
        Me.Label106.BackColor = System.Drawing.SystemColors.Window
        Me.Label106.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label106.Location = New System.Drawing.Point(0, 32)
        Me.Label106.Name = "Label106"
        Me.Label106.Size = New System.Drawing.Size(235, 2)
        Me.Label106.TabIndex = 0
        Me.Label106.Text = "Label97"
        '
        'Label107
        '
        Me.Label107.BackColor = System.Drawing.Color.SteelBlue
        Me.Label107.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label107.Font = New System.Drawing.Font("Verdana", 14.25!, System.Drawing.FontStyle.Bold)
        Me.Label107.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label107.Location = New System.Drawing.Point(0, 0)
        Me.Label107.Name = "Label107"
        Me.Label107.Size = New System.Drawing.Size(235, 32)
        Me.Label107.TabIndex = 0
        Me.Label107.Text = "FINAL AMOUNT"
        Me.Label107.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlPuExtaDetails
        '
        Me.pnlPuExtaDetails.Controls.Add(Me.Label124)
        Me.pnlPuExtaDetails.Controls.Add(Me.txtPuDiscount_AMT)
        Me.pnlPuExtaDetails.Location = New System.Drawing.Point(133, 33)
        Me.pnlPuExtaDetails.Name = "pnlPuExtaDetails"
        Me.pnlPuExtaDetails.Size = New System.Drawing.Size(99, 66)
        Me.pnlPuExtaDetails.TabIndex = 6
        '
        'Label124
        '
        Me.Label124.AutoSize = True
        Me.Label124.Location = New System.Drawing.Point(20, 12)
        Me.Label124.Name = "Label124"
        Me.Label124.Size = New System.Drawing.Size(70, 17)
        Me.Label124.TabIndex = 7
        Me.Label124.Text = "Discount"
        '
        'txtPuDiscount_AMT
        '
        Me.txtPuDiscount_AMT.Location = New System.Drawing.Point(11, 29)
        Me.txtPuDiscount_AMT.Name = "txtPuDiscount_AMT"
        Me.txtPuDiscount_AMT.Size = New System.Drawing.Size(75, 24)
        Me.txtPuDiscount_AMT.TabIndex = 8
        '
        'tabReceiptWeightAdvance
        '
        Me.tabReceiptWeightAdvance.Controls.Add(Me.grpReceiptWeightAdvance)
        Me.tabReceiptWeightAdvance.Location = New System.Drawing.Point(4, 14)
        Me.tabReceiptWeightAdvance.Name = "tabReceiptWeightAdvance"
        Me.tabReceiptWeightAdvance.Size = New System.Drawing.Size(581, 281)
        Me.tabReceiptWeightAdvance.TabIndex = 13
        Me.tabReceiptWeightAdvance.Text = "ReceiptWeightAdvance"
        Me.tabReceiptWeightAdvance.UseVisualStyleBackColor = True
        '
        'tabReceiptReserve
        '
        Me.tabReceiptReserve.Controls.Add(Me.grpRecReservedItem)
        Me.tabReceiptReserve.Location = New System.Drawing.Point(4, 14)
        Me.tabReceiptReserve.Name = "tabReceiptReserve"
        Me.tabReceiptReserve.Size = New System.Drawing.Size(581, 281)
        Me.tabReceiptReserve.TabIndex = 14
        Me.tabReceiptReserve.Text = "ReceiptReserve"
        Me.tabReceiptReserve.UseVisualStyleBackColor = True
        '
        'tabOrderDetail
        '
        Me.tabOrderDetail.Controls.Add(Me.grpOrderDetail)
        Me.tabOrderDetail.Location = New System.Drawing.Point(4, 14)
        Me.tabOrderDetail.Name = "tabOrderDetail"
        Me.tabOrderDetail.Size = New System.Drawing.Size(581, 281)
        Me.tabOrderDetail.TabIndex = 15
        Me.tabOrderDetail.Text = "Order Detail"
        Me.tabOrderDetail.UseVisualStyleBackColor = True
        '
        'grpOrderDetail
        '
        Me.grpOrderDetail.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpOrderDetail.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpOrderDetail.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpOrderDetail.BorderColor = System.Drawing.Color.Transparent
        Me.grpOrderDetail.BorderThickness = 1.0!
        Me.grpOrderDetail.Controls.Add(Me.txtOrdCGST_AMT)
        Me.grpOrderDetail.Controls.Add(Me.txtOrdSGST_AMT)
        Me.grpOrderDetail.Controls.Add(Me.txtOrdAdjdisc)
        Me.grpOrderDetail.Controls.Add(Me.txtOrdDisc)
        Me.grpOrderDetail.Controls.Add(Me.txtExMcvat)
        Me.grpOrderDetail.Controls.Add(Me.Label45)
        Me.grpOrderDetail.Controls.Add(Me.txtOrdVat_AMT)
        Me.grpOrderDetail.Controls.Add(Me.txtOrdRate_AMT)
        Me.grpOrderDetail.Controls.Add(Me.txtOrdAmount_AMT)
        Me.grpOrderDetail.Controls.Add(Me.txtOrdGrossAmount_AMT)
        Me.grpOrderDetail.Controls.Add(Me.txtOrdBalanceWeight_WET)
        Me.grpOrderDetail.Controls.Add(Me.txtOrdAdvanceWeight_WET)
        Me.grpOrderDetail.Controls.Add(Me.txtOrdTotalWeight_WET)
        Me.grpOrderDetail.Controls.Add(Me.lblOrdVat)
        Me.grpOrderDetail.Controls.Add(Me.Label98)
        Me.grpOrderDetail.Controls.Add(Me.Label100)
        Me.grpOrderDetail.Controls.Add(Me.Label79)
        Me.grpOrderDetail.Controls.Add(Me.Label78)
        Me.grpOrderDetail.Controls.Add(Me.Label53)
        Me.grpOrderDetail.Controls.Add(Me.Label52)
        Me.grpOrderDetail.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpOrderDetail.GroupImage = Nothing
        Me.grpOrderDetail.GroupTitle = ""
        Me.grpOrderDetail.Location = New System.Drawing.Point(90, 41)
        Me.grpOrderDetail.Name = "grpOrderDetail"
        Me.grpOrderDetail.Padding = New System.Windows.Forms.Padding(20)
        Me.grpOrderDetail.PaintGroupBox = False
        Me.grpOrderDetail.RoundCorners = 10
        Me.grpOrderDetail.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpOrderDetail.ShadowControl = False
        Me.grpOrderDetail.ShadowThickness = 3
        Me.grpOrderDetail.Size = New System.Drawing.Size(407, 180)
        Me.grpOrderDetail.TabIndex = 0
        '
        'txtOrdCGST_AMT
        '
        Me.txtOrdCGST_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOrdCGST_AMT.Location = New System.Drawing.Point(100, 151)
        Me.txtOrdCGST_AMT.Name = "txtOrdCGST_AMT"
        Me.txtOrdCGST_AMT.Size = New System.Drawing.Size(78, 26)
        Me.txtOrdCGST_AMT.TabIndex = 8
        Me.txtOrdCGST_AMT.Visible = False
        '
        'txtOrdSGST_AMT
        '
        Me.txtOrdSGST_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOrdSGST_AMT.Location = New System.Drawing.Point(16, 151)
        Me.txtOrdSGST_AMT.Name = "txtOrdSGST_AMT"
        Me.txtOrdSGST_AMT.Size = New System.Drawing.Size(78, 26)
        Me.txtOrdSGST_AMT.TabIndex = 7
        Me.txtOrdSGST_AMT.Visible = False
        '
        'txtOrdAdjdisc
        '
        Me.txtOrdAdjdisc.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOrdAdjdisc.Location = New System.Drawing.Point(253, 151)
        Me.txtOrdAdjdisc.Name = "txtOrdAdjdisc"
        Me.txtOrdAdjdisc.Size = New System.Drawing.Size(79, 26)
        Me.txtOrdAdjdisc.TabIndex = 6
        Me.txtOrdAdjdisc.Visible = False
        '
        'txtOrdDisc
        '
        Me.txtOrdDisc.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOrdDisc.Location = New System.Drawing.Point(319, 151)
        Me.txtOrdDisc.Name = "txtOrdDisc"
        Me.txtOrdDisc.Size = New System.Drawing.Size(79, 26)
        Me.txtOrdDisc.TabIndex = 5
        Me.txtOrdDisc.Visible = False
        '
        'txtExMcvat
        '
        Me.txtExMcvat.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtExMcvat.Location = New System.Drawing.Point(114, 123)
        Me.txtExMcvat.Name = "txtExMcvat"
        Me.txtExMcvat.Size = New System.Drawing.Size(79, 26)
        Me.txtExMcvat.TabIndex = 3
        '
        'Label45
        '
        Me.Label45.AutoSize = True
        Me.Label45.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label45.Location = New System.Drawing.Point(3, 126)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(131, 18)
        Me.Label45.TabIndex = 4
        Me.Label45.Text = "Excess Mc/Tax"
        Me.Label45.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtOrdVat_AMT
        '
        Me.txtOrdVat_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOrdVat_AMT.Location = New System.Drawing.Point(320, 94)
        Me.txtOrdVat_AMT.Name = "txtOrdVat_AMT"
        Me.txtOrdVat_AMT.Size = New System.Drawing.Size(78, 26)
        Me.txtOrdVat_AMT.TabIndex = 0
        '
        'txtOrdRate_AMT
        '
        Me.txtOrdRate_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOrdRate_AMT.Location = New System.Drawing.Point(320, 64)
        Me.txtOrdRate_AMT.Name = "txtOrdRate_AMT"
        Me.txtOrdRate_AMT.Size = New System.Drawing.Size(78, 26)
        Me.txtOrdRate_AMT.TabIndex = 0
        '
        'txtOrdAmount_AMT
        '
        Me.txtOrdAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOrdAmount_AMT.Location = New System.Drawing.Point(320, 123)
        Me.txtOrdAmount_AMT.Name = "txtOrdAmount_AMT"
        Me.txtOrdAmount_AMT.Size = New System.Drawing.Size(79, 26)
        Me.txtOrdAmount_AMT.TabIndex = 0
        '
        'txtOrdGrossAmount_AMT
        '
        Me.txtOrdGrossAmount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOrdGrossAmount_AMT.Location = New System.Drawing.Point(114, 93)
        Me.txtOrdGrossAmount_AMT.Name = "txtOrdGrossAmount_AMT"
        Me.txtOrdGrossAmount_AMT.Size = New System.Drawing.Size(79, 26)
        Me.txtOrdGrossAmount_AMT.TabIndex = 0
        '
        'txtOrdBalanceWeight_WET
        '
        Me.txtOrdBalanceWeight_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOrdBalanceWeight_WET.Location = New System.Drawing.Point(115, 62)
        Me.txtOrdBalanceWeight_WET.Name = "txtOrdBalanceWeight_WET"
        Me.txtOrdBalanceWeight_WET.Size = New System.Drawing.Size(79, 26)
        Me.txtOrdBalanceWeight_WET.TabIndex = 0
        '
        'txtOrdAdvanceWeight_WET
        '
        Me.txtOrdAdvanceWeight_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOrdAdvanceWeight_WET.Location = New System.Drawing.Point(320, 31)
        Me.txtOrdAdvanceWeight_WET.Name = "txtOrdAdvanceWeight_WET"
        Me.txtOrdAdvanceWeight_WET.Size = New System.Drawing.Size(78, 26)
        Me.txtOrdAdvanceWeight_WET.TabIndex = 0
        '
        'txtOrdTotalWeight_WET
        '
        Me.txtOrdTotalWeight_WET.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOrdTotalWeight_WET.Location = New System.Drawing.Point(114, 32)
        Me.txtOrdTotalWeight_WET.Name = "txtOrdTotalWeight_WET"
        Me.txtOrdTotalWeight_WET.Size = New System.Drawing.Size(79, 26)
        Me.txtOrdTotalWeight_WET.TabIndex = 0
        '
        'lblOrdVat
        '
        Me.lblOrdVat.AutoSize = True
        Me.lblOrdVat.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOrdVat.Location = New System.Drawing.Point(198, 97)
        Me.lblOrdVat.Name = "lblOrdVat"
        Me.lblOrdVat.Size = New System.Drawing.Size(36, 18)
        Me.lblOrdVat.TabIndex = 2
        Me.lblOrdVat.Text = "Vat"
        Me.lblOrdVat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label98
        '
        Me.Label98.AutoSize = True
        Me.Label98.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label98.Location = New System.Drawing.Point(198, 67)
        Me.Label98.Name = "Label98"
        Me.Label98.Size = New System.Drawing.Size(47, 18)
        Me.Label98.TabIndex = 2
        Me.Label98.Text = "Rate"
        Me.Label98.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label100
        '
        Me.Label100.AutoSize = True
        Me.Label100.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label100.Location = New System.Drawing.Point(198, 126)
        Me.Label100.Name = "Label100"
        Me.Label100.Size = New System.Drawing.Size(73, 18)
        Me.Label100.TabIndex = 2
        Me.Label100.Text = "Amount"
        Me.Label100.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label79
        '
        Me.Label79.AutoSize = True
        Me.Label79.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label79.Location = New System.Drawing.Point(3, 96)
        Me.Label79.Name = "Label79"
        Me.Label79.Size = New System.Drawing.Size(125, 18)
        Me.Label79.TabIndex = 2
        Me.Label79.Text = "Gross Amount"
        Me.Label79.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label78
        '
        Me.Label78.AutoSize = True
        Me.Label78.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label78.Location = New System.Drawing.Point(4, 65)
        Me.Label78.Name = "Label78"
        Me.Label78.Size = New System.Drawing.Size(133, 18)
        Me.Label78.TabIndex = 2
        Me.Label78.Text = "Balance Weight"
        Me.Label78.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label53
        '
        Me.Label53.AutoSize = True
        Me.Label53.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label53.Location = New System.Drawing.Point(198, 35)
        Me.Label53.Name = "Label53"
        Me.Label53.Size = New System.Drawing.Size(140, 18)
        Me.Label53.TabIndex = 2
        Me.Label53.Text = "Advance Weight"
        Me.Label53.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label52
        '
        Me.Label52.AutoSize = True
        Me.Label52.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label52.Location = New System.Drawing.Point(3, 35)
        Me.Label52.Name = "Label52"
        Me.Label52.Size = New System.Drawing.Size(111, 18)
        Me.Label52.TabIndex = 2
        Me.Label52.Text = "Total Weight"
        Me.Label52.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tabAdvanceWeightAdjCalc
        '
        Me.tabAdvanceWeightAdjCalc.Controls.Add(Me.grpAdvanceCalc)
        Me.tabAdvanceWeightAdjCalc.Location = New System.Drawing.Point(4, 14)
        Me.tabAdvanceWeightAdjCalc.Name = "tabAdvanceWeightAdjCalc"
        Me.tabAdvanceWeightAdjCalc.Size = New System.Drawing.Size(581, 281)
        Me.tabAdvanceWeightAdjCalc.TabIndex = 16
        Me.tabAdvanceWeightAdjCalc.Text = "Advance Weight Adj Calc"
        Me.tabAdvanceWeightAdjCalc.UseVisualStyleBackColor = True
        '
        'grpAdvanceCalc
        '
        Me.grpAdvanceCalc.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpAdvanceCalc.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpAdvanceCalc.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpAdvanceCalc.BorderColor = System.Drawing.Color.Transparent
        Me.grpAdvanceCalc.BorderThickness = 1.0!
        Me.grpAdvanceCalc.Controls.Add(Me.gridAdvanceAdjCalc)
        Me.grpAdvanceCalc.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpAdvanceCalc.GroupImage = Nothing
        Me.grpAdvanceCalc.GroupTitle = ""
        Me.grpAdvanceCalc.Location = New System.Drawing.Point(11, 21)
        Me.grpAdvanceCalc.Name = "grpAdvanceCalc"
        Me.grpAdvanceCalc.Padding = New System.Windows.Forms.Padding(20)
        Me.grpAdvanceCalc.PaintGroupBox = False
        Me.grpAdvanceCalc.RoundCorners = 10
        Me.grpAdvanceCalc.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpAdvanceCalc.ShadowControl = False
        Me.grpAdvanceCalc.ShadowThickness = 3
        Me.grpAdvanceCalc.Size = New System.Drawing.Size(407, 180)
        Me.grpAdvanceCalc.TabIndex = 0
        '
        'gridAdvanceAdjCalc
        '
        Me.gridAdvanceAdjCalc.AllowUserToAddRows = False
        Me.gridAdvanceAdjCalc.AllowUserToDeleteRows = False
        Me.gridAdvanceAdjCalc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.gridAdvanceAdjCalc.Location = New System.Drawing.Point(9, 21)
        Me.gridAdvanceAdjCalc.Name = "gridAdvanceAdjCalc"
        Me.gridAdvanceAdjCalc.ReadOnly = True
        Me.gridAdvanceAdjCalc.RowHeadersWidth = 51
        Me.gridAdvanceAdjCalc.Size = New System.Drawing.Size(389, 149)
        Me.gridAdvanceAdjCalc.TabIndex = 1
        '
        'tabWholeSaleDetails
        '
        Me.tabWholeSaleDetails.Controls.Add(Me.grpWholeSaleDetails)
        Me.tabWholeSaleDetails.Location = New System.Drawing.Point(4, 14)
        Me.tabWholeSaleDetails.Name = "tabWholeSaleDetails"
        Me.tabWholeSaleDetails.Size = New System.Drawing.Size(581, 281)
        Me.tabWholeSaleDetails.TabIndex = 17
        Me.tabWholeSaleDetails.Text = "WholeSale Detail"
        Me.tabWholeSaleDetails.UseVisualStyleBackColor = True
        '
        'grpWholeSaleDetails
        '
        Me.grpWholeSaleDetails.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpWholeSaleDetails.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpWholeSaleDetails.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpWholeSaleDetails.BorderColor = System.Drawing.Color.Transparent
        Me.grpWholeSaleDetails.BorderThickness = 1.0!
        Me.grpWholeSaleDetails.Controls.Add(Me.dgvWholeSaleDetail)
        Me.grpWholeSaleDetails.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpWholeSaleDetails.GroupImage = Nothing
        Me.grpWholeSaleDetails.GroupTitle = ""
        Me.grpWholeSaleDetails.Location = New System.Drawing.Point(4, -5)
        Me.grpWholeSaleDetails.Name = "grpWholeSaleDetails"
        Me.grpWholeSaleDetails.Padding = New System.Windows.Forms.Padding(20)
        Me.grpWholeSaleDetails.PaintGroupBox = False
        Me.grpWholeSaleDetails.RoundCorners = 10
        Me.grpWholeSaleDetails.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpWholeSaleDetails.ShadowControl = False
        Me.grpWholeSaleDetails.ShadowThickness = 3
        Me.grpWholeSaleDetails.Size = New System.Drawing.Size(577, 180)
        Me.grpWholeSaleDetails.TabIndex = 0
        '
        'dgvWholeSaleDetail
        '
        Me.dgvWholeSaleDetail.AllowUserToAddRows = False
        Me.dgvWholeSaleDetail.AllowUserToDeleteRows = False
        Me.dgvWholeSaleDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvWholeSaleDetail.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvWholeSaleDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWholeSaleDetail.Location = New System.Drawing.Point(8, 17)
        Me.dgvWholeSaleDetail.Name = "dgvWholeSaleDetail"
        Me.dgvWholeSaleDetail.ReadOnly = True
        Me.dgvWholeSaleDetail.RowHeadersWidth = 51
        Me.dgvWholeSaleDetail.Size = New System.Drawing.Size(559, 123)
        Me.dgvWholeSaleDetail.TabIndex = 15
        '
        'tabSAExtraDetail
        '
        Me.tabSAExtraDetail.Location = New System.Drawing.Point(4, 14)
        Me.tabSAExtraDetail.Name = "tabSAExtraDetail"
        Me.tabSAExtraDetail.Size = New System.Drawing.Size(581, 281)
        Me.tabSAExtraDetail.TabIndex = 2
        Me.tabSAExtraDetail.Text = "Xtra Detail"
        Me.tabSAExtraDetail.UseVisualStyleBackColor = True
        '
        'txtAdjSrCredit_AMT
        '
        Me.txtAdjSrCredit_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold)
        Me.txtAdjSrCredit_AMT.ForeColor = System.Drawing.Color.Red
        Me.txtAdjSrCredit_AMT.Location = New System.Drawing.Point(25, 40)
        Me.txtAdjSrCredit_AMT.Name = "txtAdjSrCredit_AMT"
        Me.txtAdjSrCredit_AMT.Size = New System.Drawing.Size(96, 26)
        Me.txtAdjSrCredit_AMT.TabIndex = 32
        Me.txtAdjSrCredit_AMT.Text = "0"
        '
        'lblHelpText
        '
        Me.lblHelpText.BackColor = System.Drawing.SystemColors.Control
        Me.lblHelpText.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHelpText.ForeColor = System.Drawing.Color.Red
        Me.lblHelpText.Location = New System.Drawing.Point(587, 679)
        Me.lblHelpText.Name = "lblHelpText"
        Me.lblHelpText.Size = New System.Drawing.Size(425, 13)
        Me.lblHelpText.TabIndex = 15
        Me.lblHelpText.Text = "Help"
        Me.lblHelpText.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'grpAdj
        '
        Me.grpAdj.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpAdj.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpAdj.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpAdj.BorderColor = System.Drawing.Color.Transparent
        Me.grpAdj.BorderThickness = 1.0!
        Me.grpAdj.Controls.Add(Me.lblDiscper)
        Me.grpAdj.Controls.Add(Me.txtAdjDiscount_PER)
        Me.grpAdj.Controls.Add(Me.txtAdjCash_AMT)
        Me.grpAdj.Controls.Add(Me.txtAdjSrCredit_AMT)
        Me.grpAdj.Controls.Add(Me.txtAdjGiftVoucher_AMT)
        Me.grpAdj.Controls.Add(Me.Label28)
        Me.grpAdj.Controls.Add(Me.Label43)
        Me.grpAdj.Controls.Add(Me.Label41)
        Me.grpAdj.Controls.Add(Me.Label44)
        Me.grpAdj.Controls.Add(Me.Label60)
        Me.grpAdj.Controls.Add(Me.Label38)
        Me.grpAdj.Controls.Add(Me.Label59)
        Me.grpAdj.Controls.Add(Me.txtAdjCredit_AMT)
        Me.grpAdj.Controls.Add(Me.Label40)
        Me.grpAdj.Controls.Add(Me.Label39)
        Me.grpAdj.Controls.Add(Me.txtAdjDiscount_AMT)
        Me.grpAdj.Controls.Add(Me.txtAdjAdvance_AMT)
        Me.grpAdj.Controls.Add(Me.Label57)
        Me.grpAdj.Controls.Add(Me.Label42)
        Me.grpAdj.Controls.Add(Me.Label27)
        Me.grpAdj.Controls.Add(Me.lblHc)
        Me.grpAdj.Controls.Add(Me.txtAdjChitCard_AMT)
        Me.grpAdj.Controls.Add(Me.Label54)
        Me.grpAdj.Controls.Add(Me.txtAdjHandlingCharge_AMT)
        Me.grpAdj.Controls.Add(Me.txtAdjRoundoff_AMT)
        Me.grpAdj.Controls.Add(Me.Label46)
        Me.grpAdj.Controls.Add(Me.Label148)
        Me.grpAdj.Controls.Add(Me.Label49)
        Me.grpAdj.Controls.Add(Me.Label26)
        Me.grpAdj.Controls.Add(Me.txtAdjReceive_AMT)
        Me.grpAdj.Controls.Add(Me.Label47)
        Me.grpAdj.Controls.Add(Me.txtAdjCheque_AMT)
        Me.grpAdj.Controls.Add(Me.txtAdjCreditCard_AMT)
        Me.grpAdj.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpAdj.GroupImage = Nothing
        Me.grpAdj.GroupTitle = ""
        Me.grpAdj.Location = New System.Drawing.Point(587, 399)
        Me.grpAdj.Name = "grpAdj"
        Me.grpAdj.Padding = New System.Windows.Forms.Padding(20)
        Me.grpAdj.PaintGroupBox = False
        Me.grpAdj.RoundCorners = 10
        Me.grpAdj.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpAdj.ShadowControl = False
        Me.grpAdj.ShadowThickness = 3
        Me.grpAdj.Size = New System.Drawing.Size(245, 285)
        Me.grpAdj.TabIndex = 1
        '
        'lblDiscper
        '
        Me.lblDiscper.BackColor = System.Drawing.Color.Transparent
        Me.lblDiscper.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDiscper.Location = New System.Drawing.Point(107, 234)
        Me.lblDiscper.Name = "lblDiscper"
        Me.lblDiscper.Size = New System.Drawing.Size(18, 20)
        Me.lblDiscper.TabIndex = 35
        Me.lblDiscper.Text = "%"
        Me.lblDiscper.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAdjDiscount_PER
        '
        Me.txtAdjDiscount_PER.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjDiscount_PER.Location = New System.Drawing.Point(127, 231)
        Me.txtAdjDiscount_PER.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjDiscount_PER.MaxLength = 12
        Me.txtAdjDiscount_PER.Name = "txtAdjDiscount_PER"
        Me.txtAdjDiscount_PER.Size = New System.Drawing.Size(29, 26)
        Me.txtAdjDiscount_PER.TabIndex = 34
        Me.txtAdjDiscount_PER.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAdjCash_AMT
        '
        Me.txtAdjCash_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjCash_AMT.Location = New System.Drawing.Point(127, 255)
        Me.txtAdjCash_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjCash_AMT.MaxLength = 12
        Me.txtAdjCash_AMT.Name = "txtAdjCash_AMT"
        Me.txtAdjCash_AMT.Size = New System.Drawing.Size(111, 26)
        Me.txtAdjCash_AMT.TabIndex = 33
        Me.txtAdjCash_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAdjGiftVoucher_AMT
        '
        Me.txtAdjGiftVoucher_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjGiftVoucher_AMT.Location = New System.Drawing.Point(127, 112)
        Me.txtAdjGiftVoucher_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjGiftVoucher_AMT.MaxLength = 12
        Me.txtAdjGiftVoucher_AMT.Name = "txtAdjGiftVoucher_AMT"
        Me.txtAdjGiftVoucher_AMT.Size = New System.Drawing.Size(111, 26)
        Me.txtAdjGiftVoucher_AMT.TabIndex = 8
        Me.txtAdjGiftVoucher_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.BackColor = System.Drawing.Color.Transparent
        Me.Label28.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.Location = New System.Drawing.Point(43, 140)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(73, 18)
        Me.Label28.TabIndex = 9
        Me.Label28.Text = "Scheme"
        Me.Label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label43
        '
        Me.Label43.AutoSize = True
        Me.Label43.BackColor = System.Drawing.Color.Transparent
        Me.Label43.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label43.Location = New System.Drawing.Point(45, 20)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(72, 18)
        Me.Label43.TabIndex = 0
        Me.Label43.Text = "Receive"
        Me.Label43.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label41
        '
        Me.Label41.AutoSize = True
        Me.Label41.BackColor = System.Drawing.Color.Transparent
        Me.Label41.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label41.Location = New System.Drawing.Point(43, 236)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(78, 18)
        Me.Label41.TabIndex = 17
        Me.Label41.Text = "Discount"
        Me.Label41.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label44
        '
        Me.Label44.AutoSize = True
        Me.Label44.BackColor = System.Drawing.Color.Transparent
        Me.Label44.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label44.Location = New System.Drawing.Point(43, 116)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(103, 18)
        Me.Label44.TabIndex = 7
        Me.Label44.Text = "Gift&Voucher"
        Me.Label44.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label60
        '
        Me.Label60.AutoSize = True
        Me.Label60.BackColor = System.Drawing.Color.Transparent
        Me.Label60.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label60.Location = New System.Drawing.Point(3, 140)
        Me.Label60.Name = "Label60"
        Me.Label60.Size = New System.Drawing.Size(45, 18)
        Me.Label60.TabIndex = 9
        Me.Label60.Text = "[F9]"
        Me.Label60.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label38
        '
        Me.Label38.AutoSize = True
        Me.Label38.BackColor = System.Drawing.Color.Transparent
        Me.Label38.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label38.Location = New System.Drawing.Point(43, 164)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(80, 18)
        Me.Label38.TabIndex = 11
        Me.Label38.Text = "UPI\Chq"
        Me.Label38.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label59
        '
        Me.Label59.AutoSize = True
        Me.Label59.BackColor = System.Drawing.Color.Transparent
        Me.Label59.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label59.Location = New System.Drawing.Point(3, 236)
        Me.Label59.Name = "Label59"
        Me.Label59.Size = New System.Drawing.Size(45, 18)
        Me.Label59.TabIndex = 17
        Me.Label59.Text = "[F5]"
        Me.Label59.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAdjCredit_AMT
        '
        Me.txtAdjCredit_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjCredit_AMT.Location = New System.Drawing.Point(127, 88)
        Me.txtAdjCredit_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjCredit_AMT.MaxLength = 12
        Me.txtAdjCredit_AMT.Name = "txtAdjCredit_AMT"
        Me.txtAdjCredit_AMT.Size = New System.Drawing.Size(111, 26)
        Me.txtAdjCredit_AMT.TabIndex = 6
        Me.txtAdjCredit_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label40
        '
        Me.Label40.AutoSize = True
        Me.Label40.BackColor = System.Drawing.Color.Transparent
        Me.Label40.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label40.Location = New System.Drawing.Point(43, 188)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(100, 18)
        Me.Label40.TabIndex = 13
        Me.Label40.Text = "Credit Card"
        Me.Label40.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.BackColor = System.Drawing.Color.Transparent
        Me.Label39.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label39.Location = New System.Drawing.Point(45, 92)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(40, 18)
        Me.Label39.TabIndex = 5
        Me.Label39.Text = "Due"
        Me.Label39.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAdjDiscount_AMT
        '
        Me.txtAdjDiscount_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjDiscount_AMT.Location = New System.Drawing.Point(157, 231)
        Me.txtAdjDiscount_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjDiscount_AMT.MaxLength = 12
        Me.txtAdjDiscount_AMT.Name = "txtAdjDiscount_AMT"
        Me.txtAdjDiscount_AMT.Size = New System.Drawing.Size(80, 26)
        Me.txtAdjDiscount_AMT.TabIndex = 18
        Me.txtAdjDiscount_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAdjAdvance_AMT
        '
        Me.txtAdjAdvance_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjAdvance_AMT.Location = New System.Drawing.Point(127, 64)
        Me.txtAdjAdvance_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjAdvance_AMT.MaxLength = 12
        Me.txtAdjAdvance_AMT.Name = "txtAdjAdvance_AMT"
        Me.txtAdjAdvance_AMT.Size = New System.Drawing.Size(111, 26)
        Me.txtAdjAdvance_AMT.TabIndex = 4
        Me.txtAdjAdvance_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label57
        '
        Me.Label57.AutoSize = True
        Me.Label57.BackColor = System.Drawing.Color.Transparent
        Me.Label57.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label57.Location = New System.Drawing.Point(3, 164)
        Me.Label57.Name = "Label57"
        Me.Label57.Size = New System.Drawing.Size(45, 18)
        Me.Label57.TabIndex = 11
        Me.Label57.Text = "[F8]"
        Me.Label57.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label42
        '
        Me.Label42.AutoSize = True
        Me.Label42.BackColor = System.Drawing.Color.Transparent
        Me.Label42.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label42.Location = New System.Drawing.Point(43, 260)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(48, 18)
        Me.Label42.TabIndex = 19
        Me.Label42.Text = "Cash"
        Me.Label42.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.BackColor = System.Drawing.Color.Transparent
        Me.Label27.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label27.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label27.Location = New System.Drawing.Point(43, 68)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(78, 18)
        Me.Label27.TabIndex = 3
        Me.Label27.Text = "Advance"
        Me.Label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblHc
        '
        Me.lblHc.AutoSize = True
        Me.lblHc.BackColor = System.Drawing.Color.Transparent
        Me.lblHc.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHc.Location = New System.Drawing.Point(43, 212)
        Me.lblHc.Name = "lblHc"
        Me.lblHc.Size = New System.Drawing.Size(103, 18)
        Me.lblHc.TabIndex = 15
        Me.lblHc.Text = "Hand Charg"
        Me.lblHc.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAdjChitCard_AMT
        '
        Me.txtAdjChitCard_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjChitCard_AMT.Location = New System.Drawing.Point(127, 136)
        Me.txtAdjChitCard_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjChitCard_AMT.MaxLength = 12
        Me.txtAdjChitCard_AMT.Name = "txtAdjChitCard_AMT"
        Me.txtAdjChitCard_AMT.Size = New System.Drawing.Size(111, 26)
        Me.txtAdjChitCard_AMT.TabIndex = 10
        Me.txtAdjChitCard_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label54
        '
        Me.Label54.AutoSize = True
        Me.Label54.BackColor = System.Drawing.Color.Transparent
        Me.Label54.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label54.Location = New System.Drawing.Point(3, 188)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(45, 18)
        Me.Label54.TabIndex = 13
        Me.Label54.Text = "[F7]"
        Me.Label54.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAdjHandlingCharge_AMT
        '
        Me.txtAdjHandlingCharge_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjHandlingCharge_AMT.Location = New System.Drawing.Point(127, 208)
        Me.txtAdjHandlingCharge_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjHandlingCharge_AMT.MaxLength = 12
        Me.txtAdjHandlingCharge_AMT.Name = "txtAdjHandlingCharge_AMT"
        Me.txtAdjHandlingCharge_AMT.Size = New System.Drawing.Size(111, 26)
        Me.txtAdjHandlingCharge_AMT.TabIndex = 16
        Me.txtAdjHandlingCharge_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAdjRoundoff_AMT
        '
        Me.txtAdjRoundoff_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjRoundoff_AMT.Location = New System.Drawing.Point(127, 40)
        Me.txtAdjRoundoff_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjRoundoff_AMT.MaxLength = 12
        Me.txtAdjRoundoff_AMT.Name = "txtAdjRoundoff_AMT"
        Me.txtAdjRoundoff_AMT.Size = New System.Drawing.Size(111, 26)
        Me.txtAdjRoundoff_AMT.TabIndex = 2
        Me.txtAdjRoundoff_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label46
        '
        Me.Label46.AutoSize = True
        Me.Label46.BackColor = System.Drawing.Color.Transparent
        Me.Label46.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label46.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label46.Location = New System.Drawing.Point(3, 68)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(56, 18)
        Me.Label46.TabIndex = 3
        Me.Label46.Text = "[F12]"
        Me.Label46.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label148
        '
        Me.Label148.AutoSize = True
        Me.Label148.BackColor = System.Drawing.Color.Transparent
        Me.Label148.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label148.Location = New System.Drawing.Point(3, 116)
        Me.Label148.Name = "Label148"
        Me.Label148.Size = New System.Drawing.Size(56, 18)
        Me.Label148.TabIndex = 5
        Me.Label148.Text = "[F10]"
        Me.Label148.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label49
        '
        Me.Label49.AutoSize = True
        Me.Label49.BackColor = System.Drawing.Color.Transparent
        Me.Label49.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label49.Location = New System.Drawing.Point(3, 92)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(56, 18)
        Me.Label49.TabIndex = 5
        Me.Label49.Text = "[F11]"
        Me.Label49.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.BackColor = System.Drawing.Color.Transparent
        Me.Label26.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label26.Location = New System.Drawing.Point(3, 212)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(45, 18)
        Me.Label26.TabIndex = 15
        Me.Label26.Text = "[F6]"
        Me.Label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAdjReceive_AMT
        '
        Me.txtAdjReceive_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjReceive_AMT.Location = New System.Drawing.Point(127, 16)
        Me.txtAdjReceive_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjReceive_AMT.MaxLength = 12
        Me.txtAdjReceive_AMT.Name = "txtAdjReceive_AMT"
        Me.txtAdjReceive_AMT.Size = New System.Drawing.Size(111, 26)
        Me.txtAdjReceive_AMT.TabIndex = 1
        Me.txtAdjReceive_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label47
        '
        Me.Label47.AutoSize = True
        Me.Label47.BackColor = System.Drawing.Color.Transparent
        Me.Label47.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label47.Location = New System.Drawing.Point(3, 260)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(45, 18)
        Me.Label47.TabIndex = 19
        Me.Label47.Text = "[F4]"
        Me.Label47.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtAdjCheque_AMT
        '
        Me.txtAdjCheque_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjCheque_AMT.Location = New System.Drawing.Point(127, 160)
        Me.txtAdjCheque_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjCheque_AMT.MaxLength = 12
        Me.txtAdjCheque_AMT.Name = "txtAdjCheque_AMT"
        Me.txtAdjCheque_AMT.Size = New System.Drawing.Size(111, 26)
        Me.txtAdjCheque_AMT.TabIndex = 12
        Me.txtAdjCheque_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAdjCreditCard_AMT
        '
        Me.txtAdjCreditCard_AMT.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdjCreditCard_AMT.Location = New System.Drawing.Point(127, 184)
        Me.txtAdjCreditCard_AMT.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.txtAdjCreditCard_AMT.MaxLength = 12
        Me.txtAdjCreditCard_AMT.Name = "txtAdjCreditCard_AMT"
        Me.txtAdjCreditCard_AMT.Size = New System.Drawing.Size(111, 26)
        Me.txtAdjCreditCard_AMT.TabIndex = 14
        Me.txtAdjCreditCard_AMT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'grpOptions
        '
        Me.grpOptions.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpOptions.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpOptions.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpOptions.BorderColor = System.Drawing.Color.Transparent
        Me.grpOptions.BorderThickness = 1.0!
        Me.grpOptions.Controls.Add(Me.picGiftVoucher)
        Me.grpOptions.Controls.Add(Me.picRecPay)
        Me.grpOptions.Controls.Add(Me.picOrderRepair)
        Me.grpOptions.Controls.Add(Me.picMiscIssue)
        Me.grpOptions.Controls.Add(Me.picPurchase)
        Me.grpOptions.Controls.Add(Me.picReturn)
        Me.grpOptions.Controls.Add(Me.picSales)
        Me.grpOptions.Controls.Add(Me.picAppIssRec)
        Me.grpOptions.Controls.Add(Me.btnRecPay_OWN)
        Me.grpOptions.Controls.Add(Me.btnOrderRepair_OWN)
        Me.grpOptions.Controls.Add(Me.btnApproval_OWN)
        Me.grpOptions.Controls.Add(Me.btnGiftVoucher_OWN)
        Me.grpOptions.Controls.Add(Me.btnSales_OWN)
        Me.grpOptions.Controls.Add(Me.btnSalesReturn_OWN)
        Me.grpOptions.Controls.Add(Me.btnPurchase_OWN)
        Me.grpOptions.Controls.Add(Me.btnMiscIssue_OWN)
        Me.grpOptions.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpOptions.GroupImage = Nothing
        Me.grpOptions.GroupTitle = ""
        Me.grpOptions.Location = New System.Drawing.Point(838, 399)
        Me.grpOptions.Name = "grpOptions"
        Me.grpOptions.Padding = New System.Windows.Forms.Padding(20)
        Me.grpOptions.PaintGroupBox = False
        Me.grpOptions.RoundCorners = 10
        Me.grpOptions.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpOptions.ShadowControl = False
        Me.grpOptions.ShadowThickness = 3
        Me.grpOptions.Size = New System.Drawing.Size(179, 285)
        Me.grpOptions.TabIndex = 0
        '
        'picGiftVoucher
        '
        Me.picGiftVoucher.Image = CType(resources.GetObject("picGiftVoucher.Image"), System.Drawing.Image)
        Me.picGiftVoucher.Location = New System.Drawing.Point(12, 254)
        Me.picGiftVoucher.Name = "picGiftVoucher"
        Me.picGiftVoucher.Size = New System.Drawing.Size(124, 16)
        Me.picGiftVoucher.TabIndex = 8
        Me.picGiftVoucher.TabStop = False
        '
        'picRecPay
        '
        Me.picRecPay.Image = CType(resources.GetObject("picRecPay.Image"), System.Drawing.Image)
        Me.picRecPay.Location = New System.Drawing.Point(12, 187)
        Me.picRecPay.Name = "picRecPay"
        Me.picRecPay.Size = New System.Drawing.Size(125, 19)
        Me.picRecPay.TabIndex = 8
        Me.picRecPay.TabStop = False
        '
        'picOrderRepair
        '
        Me.picOrderRepair.Image = CType(resources.GetObject("picOrderRepair.Image"), System.Drawing.Image)
        Me.picOrderRepair.Location = New System.Drawing.Point(12, 154)
        Me.picOrderRepair.Name = "picOrderRepair"
        Me.picOrderRepair.Size = New System.Drawing.Size(124, 19)
        Me.picOrderRepair.TabIndex = 8
        Me.picOrderRepair.TabStop = False
        '
        'picMiscIssue
        '
        Me.picMiscIssue.Image = CType(resources.GetObject("picMiscIssue.Image"), System.Drawing.Image)
        Me.picMiscIssue.Location = New System.Drawing.Point(12, 122)
        Me.picMiscIssue.Name = "picMiscIssue"
        Me.picMiscIssue.Size = New System.Drawing.Size(124, 16)
        Me.picMiscIssue.TabIndex = 8
        Me.picMiscIssue.TabStop = False
        '
        'picPurchase
        '
        Me.picPurchase.Image = CType(resources.GetObject("picPurchase.Image"), System.Drawing.Image)
        Me.picPurchase.Location = New System.Drawing.Point(12, 89)
        Me.picPurchase.Name = "picPurchase"
        Me.picPurchase.Size = New System.Drawing.Size(124, 16)
        Me.picPurchase.TabIndex = 8
        Me.picPurchase.TabStop = False
        '
        'picReturn
        '
        Me.picReturn.Image = CType(resources.GetObject("picReturn.Image"), System.Drawing.Image)
        Me.picReturn.Location = New System.Drawing.Point(12, 56)
        Me.picReturn.Name = "picReturn"
        Me.picReturn.Size = New System.Drawing.Size(124, 16)
        Me.picReturn.TabIndex = 8
        Me.picReturn.TabStop = False
        '
        'picSales
        '
        Me.picSales.Image = CType(resources.GetObject("picSales.Image"), System.Drawing.Image)
        Me.picSales.Location = New System.Drawing.Point(12, 23)
        Me.picSales.Name = "picSales"
        Me.picSales.Size = New System.Drawing.Size(124, 16)
        Me.picSales.TabIndex = 8
        Me.picSales.TabStop = False
        '
        'picAppIssRec
        '
        Me.picAppIssRec.Image = CType(resources.GetObject("picAppIssRec.Image"), System.Drawing.Image)
        Me.picAppIssRec.Location = New System.Drawing.Point(12, 220)
        Me.picAppIssRec.Name = "picAppIssRec"
        Me.picAppIssRec.Size = New System.Drawing.Size(125, 18)
        Me.picAppIssRec.TabIndex = 8
        Me.picAppIssRec.TabStop = False
        '
        'btnRecPay_OWN
        '
        Me.btnRecPay_OWN.ContextMenuStrip = Me.cMenuRecPay
        Me.btnRecPay_OWN.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRecPay_OWN.Image = Global.BrighttechRetailJewellery.My.Resources.Resources.payment_icon_22
        Me.btnRecPay_OWN.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRecPay_OWN.Location = New System.Drawing.Point(7, 181)
        Me.btnRecPay_OWN.Name = "btnRecPay_OWN"
        Me.btnRecPay_OWN.Size = New System.Drawing.Size(165, 31)
        Me.btnRecPay_OWN.TabIndex = 5
        Me.btnRecPay_OWN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRecPay_OWN.UseVisualStyleBackColor = True
        '
        'cMenuRecPay
        '
        Me.cMenuRecPay.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.cMenuRecPay.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tStripReceipt, Me.tStripPayment})
        Me.cMenuRecPay.Name = "cMenuRecPay"
        Me.cMenuRecPay.Size = New System.Drawing.Size(135, 52)
        '
        'tStripReceipt
        '
        Me.tStripReceipt.Name = "tStripReceipt"
        Me.tStripReceipt.Size = New System.Drawing.Size(134, 24)
        Me.tStripReceipt.Text = "Receipt"
        '
        'tStripPayment
        '
        Me.tStripPayment.Name = "tStripPayment"
        Me.tStripPayment.Size = New System.Drawing.Size(134, 24)
        Me.tStripPayment.Text = "Payment"
        '
        'btnOrderRepair_OWN
        '
        Me.btnOrderRepair_OWN.ContextMenuStrip = Me.cMenuOrderRepair
        Me.btnOrderRepair_OWN.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOrderRepair_OWN.Image = Global.BrighttechRetailJewellery.My.Resources.Resources.receipt_1_22
        Me.btnOrderRepair_OWN.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnOrderRepair_OWN.Location = New System.Drawing.Point(7, 148)
        Me.btnOrderRepair_OWN.Name = "btnOrderRepair_OWN"
        Me.btnOrderRepair_OWN.Size = New System.Drawing.Size(165, 31)
        Me.btnOrderRepair_OWN.TabIndex = 4
        Me.btnOrderRepair_OWN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOrderRepair_OWN.UseVisualStyleBackColor = True
        '
        'cMenuOrderRepair
        '
        Me.cMenuOrderRepair.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.cMenuOrderRepair.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tStripOrder, Me.tStripRepair})
        Me.cMenuOrderRepair.Name = "cMenuOrderRepair"
        Me.cMenuOrderRepair.Size = New System.Drawing.Size(122, 52)
        '
        'tStripOrder
        '
        Me.tStripOrder.Name = "tStripOrder"
        Me.tStripOrder.Size = New System.Drawing.Size(121, 24)
        Me.tStripOrder.Text = "Order"
        '
        'tStripRepair
        '
        Me.tStripRepair.Name = "tStripRepair"
        Me.tStripRepair.Size = New System.Drawing.Size(121, 24)
        Me.tStripRepair.Text = "Repair"
        '
        'btnApproval_OWN
        '
        Me.btnApproval_OWN.ContextMenuStrip = Me.cMenuApproval
        Me.btnApproval_OWN.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnApproval_OWN.Image = Global.BrighttechRetailJewellery.My.Resources.Resources.approval_22
        Me.btnApproval_OWN.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnApproval_OWN.Location = New System.Drawing.Point(7, 214)
        Me.btnApproval_OWN.Name = "btnApproval_OWN"
        Me.btnApproval_OWN.Size = New System.Drawing.Size(165, 31)
        Me.btnApproval_OWN.TabIndex = 6
        Me.btnApproval_OWN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnApproval_OWN.UseVisualStyleBackColor = True
        '
        'cMenuApproval
        '
        Me.cMenuApproval.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.cMenuApproval.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tStripApprovalIssue, Me.tStripApprovalReceipt})
        Me.cMenuApproval.Name = "cMenuApproval"
        Me.cMenuApproval.Size = New System.Drawing.Size(194, 52)
        '
        'tStripApprovalIssue
        '
        Me.tStripApprovalIssue.Name = "tStripApprovalIssue"
        Me.tStripApprovalIssue.Size = New System.Drawing.Size(193, 24)
        Me.tStripApprovalIssue.Text = "Approval Issue"
        '
        'tStripApprovalReceipt
        '
        Me.tStripApprovalReceipt.Name = "tStripApprovalReceipt"
        Me.tStripApprovalReceipt.Size = New System.Drawing.Size(193, 24)
        Me.tStripApprovalReceipt.Text = "Approval Receipt"
        '
        'btnGiftVoucher_OWN
        '
        Me.btnGiftVoucher_OWN.BackColor = System.Drawing.SystemColors.Window
        Me.btnGiftVoucher_OWN.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGiftVoucher_OWN.Image = Global.BrighttechRetailJewellery.My.Resources.Resources.icon_voucher_22
        Me.btnGiftVoucher_OWN.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGiftVoucher_OWN.Location = New System.Drawing.Point(7, 247)
        Me.btnGiftVoucher_OWN.Name = "btnGiftVoucher_OWN"
        Me.btnGiftVoucher_OWN.Size = New System.Drawing.Size(165, 31)
        Me.btnGiftVoucher_OWN.TabIndex = 7
        Me.btnGiftVoucher_OWN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGiftVoucher_OWN.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.btnGiftVoucher_OWN.UseVisualStyleBackColor = False
        '
        'btnSales_OWN
        '
        Me.btnSales_OWN.BackColor = System.Drawing.SystemColors.Window
        Me.btnSales_OWN.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSales_OWN.Image = Global.BrighttechRetailJewellery.My.Resources.Resources.Sales
        Me.btnSales_OWN.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSales_OWN.Location = New System.Drawing.Point(7, 16)
        Me.btnSales_OWN.Name = "btnSales_OWN"
        Me.btnSales_OWN.Size = New System.Drawing.Size(164, 31)
        Me.btnSales_OWN.TabIndex = 0
        Me.btnSales_OWN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSales_OWN.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.btnSales_OWN.UseVisualStyleBackColor = False
        '
        'btnSalesReturn_OWN
        '
        Me.btnSalesReturn_OWN.BackColor = System.Drawing.SystemColors.Window
        Me.btnSalesReturn_OWN.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSalesReturn_OWN.Image = Global.BrighttechRetailJewellery.My.Resources.Resources.images_22
        Me.btnSalesReturn_OWN.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSalesReturn_OWN.Location = New System.Drawing.Point(7, 49)
        Me.btnSalesReturn_OWN.Name = "btnSalesReturn_OWN"
        Me.btnSalesReturn_OWN.Size = New System.Drawing.Size(165, 31)
        Me.btnSalesReturn_OWN.TabIndex = 1
        Me.btnSalesReturn_OWN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSalesReturn_OWN.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.btnSalesReturn_OWN.UseVisualStyleBackColor = False
        '
        'btnPurchase_OWN
        '
        Me.btnPurchase_OWN.BackColor = System.Drawing.SystemColors.Window
        Me.btnPurchase_OWN.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPurchase_OWN.Image = Global.BrighttechRetailJewellery.My.Resources.Resources.receipt_1_22
        Me.btnPurchase_OWN.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPurchase_OWN.Location = New System.Drawing.Point(7, 82)
        Me.btnPurchase_OWN.Name = "btnPurchase_OWN"
        Me.btnPurchase_OWN.Size = New System.Drawing.Size(164, 31)
        Me.btnPurchase_OWN.TabIndex = 2
        Me.btnPurchase_OWN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPurchase_OWN.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.btnPurchase_OWN.UseVisualStyleBackColor = False
        '
        'btnMiscIssue_OWN
        '
        Me.btnMiscIssue_OWN.BackColor = System.Drawing.SystemColors.Window
        Me.btnMiscIssue_OWN.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMiscIssue_OWN.Image = Global.BrighttechRetailJewellery.My.Resources.Resources.notes_22
        Me.btnMiscIssue_OWN.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMiscIssue_OWN.Location = New System.Drawing.Point(7, 115)
        Me.btnMiscIssue_OWN.Name = "btnMiscIssue_OWN"
        Me.btnMiscIssue_OWN.Size = New System.Drawing.Size(165, 31)
        Me.btnMiscIssue_OWN.TabIndex = 3
        Me.btnMiscIssue_OWN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMiscIssue_OWN.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.btnMiscIssue_OWN.UseVisualStyleBackColor = False
        '
        'grpHeader
        '
        Me.grpHeader.BackgroundColor = System.Drawing.Color.Lavender
        Me.grpHeader.BackgroundGradientColor = System.Drawing.Color.Lavender
        Me.grpHeader.BackgroundGradientMode = CodeVendor.Controls.Grouper.GroupBoxGradientMode.None
        Me.grpHeader.BorderColor = System.Drawing.Color.Transparent
        Me.grpHeader.BorderThickness = 1.0!
        Me.grpHeader.ContextMenuStrip = Me.cmenuTemplate
        Me.grpHeader.Controls.Add(Me.lblPNo)
        Me.grpHeader.Controls.Add(Me.lblSNo)
        Me.grpHeader.Controls.Add(Me.btnCalc)
        Me.grpHeader.Controls.Add(Me.lblTitle)
        Me.grpHeader.Controls.Add(Me.Label89)
        Me.grpHeader.Controls.Add(Me.Label90)
        Me.grpHeader.Controls.Add(Me.Label91)
        Me.grpHeader.Controls.Add(Me.Label88)
        Me.grpHeader.Controls.Add(Me.Label87)
        Me.grpHeader.Controls.Add(Me.Label86)
        Me.grpHeader.Controls.Add(Me.lblCompanyName)
        Me.grpHeader.Controls.Add(Me.Label84)
        Me.grpHeader.Controls.Add(Me.lblSystemId)
        Me.grpHeader.Controls.Add(Me.lblSilverRate)
        Me.grpHeader.Controls.Add(Me.Label29)
        Me.grpHeader.Controls.Add(Me.lblNodeId)
        Me.grpHeader.Controls.Add(Me.Label32)
        Me.grpHeader.Controls.Add(Me.lblBillDate)
        Me.grpHeader.Controls.Add(Me.lblGoldRate)
        Me.grpHeader.Controls.Add(Me.lblUserName)
        Me.grpHeader.Controls.Add(Me.Label24)
        Me.grpHeader.Controls.Add(Me.Label30)
        Me.grpHeader.Controls.Add(Me.Label31)
        Me.grpHeader.Controls.Add(Me.lblCashCounter)
        Me.grpHeader.CustomGroupBoxColor = System.Drawing.Color.White
        Me.grpHeader.GroupImage = Nothing
        Me.grpHeader.GroupTitle = ""
        Me.grpHeader.Location = New System.Drawing.Point(4, -6)
        Me.grpHeader.Name = "grpHeader"
        Me.grpHeader.Padding = New System.Windows.Forms.Padding(20)
        Me.grpHeader.PaintGroupBox = False
        Me.grpHeader.RoundCorners = 10
        Me.grpHeader.ShadowColor = System.Drawing.Color.DarkGray
        Me.grpHeader.ShadowControl = False
        Me.grpHeader.ShadowThickness = 3
        Me.grpHeader.Size = New System.Drawing.Size(1015, 78)
        Me.grpHeader.TabIndex = 0
        '
        'cmenuTemplate
        '
        Me.cmenuTemplate.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.cmenuTemplate.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Style1ToolStripMenuItem, Me.Style2ToolStripMenuItem, Me.Style3ToolStripMenuItem, Me.Style4ToolStripMenuItem, Me.Style5ToolStripMenuItem, Me.Style6ToolStripMenuItem})
        Me.cmenuTemplate.Name = "cmenuTemplate"
        Me.cmenuTemplate.Size = New System.Drawing.Size(123, 148)
        '
        'Style1ToolStripMenuItem
        '
        Me.Style1ToolStripMenuItem.Name = "Style1ToolStripMenuItem"
        Me.Style1ToolStripMenuItem.Size = New System.Drawing.Size(122, 24)
        Me.Style1ToolStripMenuItem.Text = "Style 1"
        '
        'Style2ToolStripMenuItem
        '
        Me.Style2ToolStripMenuItem.Name = "Style2ToolStripMenuItem"
        Me.Style2ToolStripMenuItem.Size = New System.Drawing.Size(122, 24)
        Me.Style2ToolStripMenuItem.Text = "Style 2"
        '
        'Style3ToolStripMenuItem
        '
        Me.Style3ToolStripMenuItem.Name = "Style3ToolStripMenuItem"
        Me.Style3ToolStripMenuItem.Size = New System.Drawing.Size(122, 24)
        Me.Style3ToolStripMenuItem.Text = "Style 3"
        '
        'Style4ToolStripMenuItem
        '
        Me.Style4ToolStripMenuItem.Name = "Style4ToolStripMenuItem"
        Me.Style4ToolStripMenuItem.Size = New System.Drawing.Size(122, 24)
        Me.Style4ToolStripMenuItem.Text = "Style 4"
        '
        'Style5ToolStripMenuItem
        '
        Me.Style5ToolStripMenuItem.Name = "Style5ToolStripMenuItem"
        Me.Style5ToolStripMenuItem.Size = New System.Drawing.Size(122, 24)
        Me.Style5ToolStripMenuItem.Text = "Style 5"
        '
        'Style6ToolStripMenuItem
        '
        Me.Style6ToolStripMenuItem.Name = "Style6ToolStripMenuItem"
        Me.Style6ToolStripMenuItem.Size = New System.Drawing.Size(122, 24)
        Me.Style6ToolStripMenuItem.Text = "Style 6"
        '
        'btnCalc
        '
        Me.btnCalc.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCalc.Image = CType(resources.GetObject("btnCalc.Image"), System.Drawing.Image)
        Me.btnCalc.Location = New System.Drawing.Point(983, 33)
        Me.btnCalc.Name = "btnCalc"
        Me.btnCalc.Size = New System.Drawing.Size(28, 38)
        Me.btnCalc.TabIndex = 50
        Me.btnCalc.UseVisualStyleBackColor = True
        '
        'lblTitle
        '
        Me.lblTitle.Font = New System.Drawing.Font("Verdana", 12.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.lblTitle.Location = New System.Drawing.Point(437, 53)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(190, 17)
        Me.lblTitle.TabIndex = 10
        Me.lblTitle.Text = "SALES"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label89
        '
        Me.Label89.AutoSize = True
        Me.Label89.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label89.Location = New System.Drawing.Point(875, 54)
        Me.Label89.Name = "Label89"
        Me.Label89.Size = New System.Drawing.Size(14, 18)
        Me.Label89.TabIndex = 9
        Me.Label89.Text = ":"
        '
        'Label90
        '
        Me.Label90.AutoSize = True
        Me.Label90.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label90.Location = New System.Drawing.Point(875, 36)
        Me.Label90.Name = "Label90"
        Me.Label90.Size = New System.Drawing.Size(14, 18)
        Me.Label90.TabIndex = 8
        Me.Label90.Text = ":"
        '
        'Label91
        '
        Me.Label91.AutoSize = True
        Me.Label91.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label91.Location = New System.Drawing.Point(875, 18)
        Me.Label91.Name = "Label91"
        Me.Label91.Size = New System.Drawing.Size(14, 18)
        Me.Label91.TabIndex = 7
        Me.Label91.Text = ":"
        '
        'Label88
        '
        Me.Label88.AutoSize = True
        Me.Label88.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label88.Location = New System.Drawing.Point(153, 54)
        Me.Label88.Name = "Label88"
        Me.Label88.Size = New System.Drawing.Size(14, 18)
        Me.Label88.TabIndex = 6
        Me.Label88.Text = ":"
        '
        'Label87
        '
        Me.Label87.AutoSize = True
        Me.Label87.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label87.Location = New System.Drawing.Point(153, 36)
        Me.Label87.Name = "Label87"
        Me.Label87.Size = New System.Drawing.Size(14, 18)
        Me.Label87.TabIndex = 6
        Me.Label87.Text = ":"
        '
        'Label86
        '
        Me.Label86.AutoSize = True
        Me.Label86.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label86.Location = New System.Drawing.Point(153, 18)
        Me.Label86.Name = "Label86"
        Me.Label86.Size = New System.Drawing.Size(14, 18)
        Me.Label86.TabIndex = 6
        Me.Label86.Text = ":"
        '
        'lblCompanyName
        '
        Me.lblCompanyName.Font = New System.Drawing.Font("Verdana", 12.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCompanyName.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.lblCompanyName.Location = New System.Drawing.Point(313, 13)
        Me.lblCompanyName.Name = "lblCompanyName"
        Me.lblCompanyName.Size = New System.Drawing.Size(443, 23)
        Me.lblCompanyName.TabIndex = 5
        Me.lblCompanyName.Text = "COMPANY NAME"
        Me.lblCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label84
        '
        Me.Label84.Font = New System.Drawing.Font("Verdana", 12.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label84.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label84.Location = New System.Drawing.Point(429, 34)
        Me.Label84.Name = "Label84"
        Me.Label84.Size = New System.Drawing.Size(210, 17)
        Me.Label84.TabIndex = 5
        Me.Label84.Text = "POINT OF SALE"
        Me.Label84.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSystemId
        '
        Me.lblSystemId.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSystemId.Location = New System.Drawing.Point(898, 53)
        Me.lblSystemId.Name = "lblSystemId"
        Me.lblSystemId.Size = New System.Drawing.Size(85, 16)
        Me.lblSystemId.TabIndex = 4
        Me.lblSystemId.Text = "N03"
        Me.lblSystemId.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblNodeId
        '
        Me.lblNodeId.AutoSize = True
        Me.lblNodeId.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNodeId.Location = New System.Drawing.Point(762, 53)
        Me.lblNodeId.Name = "lblNodeId"
        Me.lblNodeId.Size = New System.Drawing.Size(63, 20)
        Me.lblNodeId.TabIndex = 4
        Me.lblNodeId.Text = "NODE"
        '
        'lblUserName
        '
        Me.lblUserName.AutoSize = True
        Me.lblUserName.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUserName.Location = New System.Drawing.Point(187, 53)
        Me.lblUserName.Name = "lblUserName"
        Me.lblUserName.Size = New System.Drawing.Size(173, 20)
        Me.lblUserName.TabIndex = 4
        Me.lblUserName.Text = "ADMINISTRATOR"
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(23, 53)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(121, 20)
        Me.Label24.TabIndex = 4
        Me.Label24.Text = "USER NAME"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CompanySelectionToolStripMenuItem, Me.Company1ToolStripMenuItem, Me.Company2ToolStripMenuItem, Me.Company3ToolStripMenuItem, Me.Company4ToolStripMenuItem, Me.Company5ToolStripMenuItem, Me.Company6ToolStripMenuItem, Me.Company7ToolStripMenuItem, Me.Company8ToolStripMenuItem, Me.Company9ToolStripMenuItem, Me.Company0ToolStripMenuItem, Me.ToolStripMenuItem1, Me.ToolStripMenuItem2, Me.ToolStripMenuItem3, Me.ToolStripMenuItem4, Me.ToolStripMenuItem5, Me.ToolStripMenuItem6, Me.ToolStripMenuItem7, Me.ToolStripMenuItem8, Me.ToolStripMenuItem9, Me.ToolStripMenuItem10, Me.SalesToolStripMenuItem, Me.SalesReturnToolStripMenuItem, Me.PurchaseToolStripMenuItem, Me.PartlySalesToolStripMenuItem, Me.AdvanceToolStripMenuItem, Me.CreditToolStripMenuItem, Me.CreditCardToolStripMenuItem, Me.ChitCardToolStripMenuItem, Me.ChequeToolStripMenuItem, Me.HandlingChargeToolStripMenuItem, Me.DiscountToolStripMenuItem, Me.CashToolStripMenuItem, Me.tStripGiftVouhcer, Me.cmenuBookingItem, Me.tStripMiscIssue, Me.tStripGiftVoucher, Me.FinalDiscountToolStripMenuItem, Me.FinalPurDiscountToolStripMenuItem, Me.SaleRateChangeToolStripMenuItem, Me.WastageMcPerToolStripMenuItem, Me.StoneDetailsToolStripMenuItem, Me.MiscDetailsToolStripMenuItem, Me.MultiMetalDetailsToolStripMenuItem, Me.SalesB4DiscountToolStripMenuItem, Me.AddressToolStripMenuItem, Me.ToBeToolStripMenuItem, Me.ReceiptToolStripMenuItem, Me.PaymentToolStripMenuItem, Me.AppIssueToolStripMenuItem, Me.AppReceiptToolStripMenuItem, Me.OrderToolStripMenuItem, Me.RepairToolStripMenuItem, Me.EstCallToolStrip, Me.ToolStripDupbill, Me.ToolStripSchemeOffer, Me.ToolStripRateView, Me.ToolStripBillno, Me.Wt2WtAdjustToolStripMenuItem, Me.ComplementToolStripMenuItem, Me.ExcelDownloadToolStripMenuItem, Me.tStripSchemeOffer, Me.CalcStripMenuItem, Me.SetItemToolStripMenuItem11, Me.DisableSetItemStripMenuItem, Me.TagCompMoveToolStripMenuItem, Me.tStripSGiftVoucher, Me.BillTypeToolStripMenuItem, Me.PrivilegeDiscountToolStripMenuItem, Me.PruchaseOrderRate, Me.KFC, Me.ToolStripRowFinalAmt, Me.SecondSalesToolStripMenuItem, Me.TrfNoToolStripMenuItem, Me.ToolStripHallmarkDetails, Me.TStripShipingAddress})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(310, 1018)
        '
        'CompanySelectionToolStripMenuItem
        '
        Me.CompanySelectionToolStripMenuItem.Name = "CompanySelectionToolStripMenuItem"
        Me.CompanySelectionToolStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.M), System.Windows.Forms.Keys)
        Me.CompanySelectionToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.CompanySelectionToolStripMenuItem.Tag = "Main"
        Me.CompanySelectionToolStripMenuItem.Text = "Company Selection"
        Me.CompanySelectionToolStripMenuItem.Visible = False
        '
        'Company1ToolStripMenuItem
        '
        Me.Company1ToolStripMenuItem.Name = "Company1ToolStripMenuItem"
        Me.Company1ToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D1), System.Windows.Forms.Keys)
        Me.Company1ToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.Company1ToolStripMenuItem.Tag = "1"
        Me.Company1ToolStripMenuItem.Text = "Company1"
        Me.Company1ToolStripMenuItem.Visible = False
        '
        'Company2ToolStripMenuItem
        '
        Me.Company2ToolStripMenuItem.Name = "Company2ToolStripMenuItem"
        Me.Company2ToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D2), System.Windows.Forms.Keys)
        Me.Company2ToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.Company2ToolStripMenuItem.Tag = "2"
        Me.Company2ToolStripMenuItem.Text = "Company2"
        Me.Company2ToolStripMenuItem.Visible = False
        '
        'Company3ToolStripMenuItem
        '
        Me.Company3ToolStripMenuItem.Name = "Company3ToolStripMenuItem"
        Me.Company3ToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D3), System.Windows.Forms.Keys)
        Me.Company3ToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.Company3ToolStripMenuItem.Tag = "3"
        Me.Company3ToolStripMenuItem.Text = "Company3"
        Me.Company3ToolStripMenuItem.Visible = False
        '
        'Company4ToolStripMenuItem
        '
        Me.Company4ToolStripMenuItem.Name = "Company4ToolStripMenuItem"
        Me.Company4ToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D4), System.Windows.Forms.Keys)
        Me.Company4ToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.Company4ToolStripMenuItem.Tag = "4"
        Me.Company4ToolStripMenuItem.Text = "Company4"
        Me.Company4ToolStripMenuItem.Visible = False
        '
        'Company5ToolStripMenuItem
        '
        Me.Company5ToolStripMenuItem.Name = "Company5ToolStripMenuItem"
        Me.Company5ToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D5), System.Windows.Forms.Keys)
        Me.Company5ToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.Company5ToolStripMenuItem.Tag = "5"
        Me.Company5ToolStripMenuItem.Text = "Company5"
        Me.Company5ToolStripMenuItem.Visible = False
        '
        'Company6ToolStripMenuItem
        '
        Me.Company6ToolStripMenuItem.Name = "Company6ToolStripMenuItem"
        Me.Company6ToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D6), System.Windows.Forms.Keys)
        Me.Company6ToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.Company6ToolStripMenuItem.Tag = "6"
        Me.Company6ToolStripMenuItem.Text = "Company6"
        Me.Company6ToolStripMenuItem.Visible = False
        '
        'Company7ToolStripMenuItem
        '
        Me.Company7ToolStripMenuItem.Name = "Company7ToolStripMenuItem"
        Me.Company7ToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D7), System.Windows.Forms.Keys)
        Me.Company7ToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.Company7ToolStripMenuItem.Tag = "7"
        Me.Company7ToolStripMenuItem.Text = "Company7"
        Me.Company7ToolStripMenuItem.Visible = False
        '
        'Company8ToolStripMenuItem
        '
        Me.Company8ToolStripMenuItem.Name = "Company8ToolStripMenuItem"
        Me.Company8ToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D8), System.Windows.Forms.Keys)
        Me.Company8ToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.Company8ToolStripMenuItem.Tag = "8"
        Me.Company8ToolStripMenuItem.Text = "Company8"
        Me.Company8ToolStripMenuItem.Visible = False
        '
        'Company9ToolStripMenuItem
        '
        Me.Company9ToolStripMenuItem.Name = "Company9ToolStripMenuItem"
        Me.Company9ToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D9), System.Windows.Forms.Keys)
        Me.Company9ToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.Company9ToolStripMenuItem.Tag = "9"
        Me.Company9ToolStripMenuItem.Text = "Company9"
        Me.Company9ToolStripMenuItem.Visible = False
        '
        'Company0ToolStripMenuItem
        '
        Me.Company0ToolStripMenuItem.Name = "Company0ToolStripMenuItem"
        Me.Company0ToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D0), System.Windows.Forms.Keys)
        Me.Company0ToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.Company0ToolStripMenuItem.Tag = "0"
        Me.Company0ToolStripMenuItem.Text = "Company0"
        Me.Company0ToolStripMenuItem.Visible = False
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.NumPad1), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(309, 24)
        Me.ToolStripMenuItem1.Tag = "1"
        Me.ToolStripMenuItem1.Text = "Company1"
        Me.ToolStripMenuItem1.Visible = False
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.NumPad2), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(309, 24)
        Me.ToolStripMenuItem2.Tag = "2"
        Me.ToolStripMenuItem2.Text = "Company2"
        Me.ToolStripMenuItem2.Visible = False
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.NumPad3), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(309, 24)
        Me.ToolStripMenuItem3.Tag = "3"
        Me.ToolStripMenuItem3.Text = "Company3"
        Me.ToolStripMenuItem3.Visible = False
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.NumPad4), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(309, 24)
        Me.ToolStripMenuItem4.Tag = "4"
        Me.ToolStripMenuItem4.Text = "Company4"
        Me.ToolStripMenuItem4.Visible = False
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.NumPad5), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(309, 24)
        Me.ToolStripMenuItem5.Tag = "5"
        Me.ToolStripMenuItem5.Text = "Company5"
        Me.ToolStripMenuItem5.Visible = False
        '
        'ToolStripMenuItem6
        '
        Me.ToolStripMenuItem6.Name = "ToolStripMenuItem6"
        Me.ToolStripMenuItem6.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.NumPad6), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem6.Size = New System.Drawing.Size(309, 24)
        Me.ToolStripMenuItem6.Tag = "6"
        Me.ToolStripMenuItem6.Text = "Company6"
        Me.ToolStripMenuItem6.Visible = False
        '
        'ToolStripMenuItem7
        '
        Me.ToolStripMenuItem7.Name = "ToolStripMenuItem7"
        Me.ToolStripMenuItem7.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.NumPad7), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem7.Size = New System.Drawing.Size(309, 24)
        Me.ToolStripMenuItem7.Tag = "7"
        Me.ToolStripMenuItem7.Text = "Company7"
        Me.ToolStripMenuItem7.Visible = False
        '
        'ToolStripMenuItem8
        '
        Me.ToolStripMenuItem8.Name = "ToolStripMenuItem8"
        Me.ToolStripMenuItem8.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.NumPad8), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem8.Size = New System.Drawing.Size(309, 24)
        Me.ToolStripMenuItem8.Tag = "8"
        Me.ToolStripMenuItem8.Text = "Company8"
        Me.ToolStripMenuItem8.Visible = False
        '
        'ToolStripMenuItem9
        '
        Me.ToolStripMenuItem9.Name = "ToolStripMenuItem9"
        Me.ToolStripMenuItem9.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.NumPad9), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem9.Size = New System.Drawing.Size(309, 24)
        Me.ToolStripMenuItem9.Tag = "9"
        Me.ToolStripMenuItem9.Text = "Company9"
        Me.ToolStripMenuItem9.Visible = False
        '
        'ToolStripMenuItem10
        '
        Me.ToolStripMenuItem10.Name = "ToolStripMenuItem10"
        Me.ToolStripMenuItem10.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.NumPad0), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem10.Size = New System.Drawing.Size(309, 24)
        Me.ToolStripMenuItem10.Tag = "0"
        Me.ToolStripMenuItem10.Text = "Company0"
        Me.ToolStripMenuItem10.Visible = False
        '
        'SalesToolStripMenuItem
        '
        Me.SalesToolStripMenuItem.Name = "SalesToolStripMenuItem"
        Me.SalesToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.SalesToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.SalesToolStripMenuItem.Text = "Sales"
        Me.SalesToolStripMenuItem.Visible = False
        '
        'SalesReturnToolStripMenuItem
        '
        Me.SalesReturnToolStripMenuItem.Name = "SalesReturnToolStripMenuItem"
        Me.SalesReturnToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.R), System.Windows.Forms.Keys)
        Me.SalesReturnToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.SalesReturnToolStripMenuItem.Text = "Sales Return"
        Me.SalesReturnToolStripMenuItem.Visible = False
        '
        'PurchaseToolStripMenuItem
        '
        Me.PurchaseToolStripMenuItem.Name = "PurchaseToolStripMenuItem"
        Me.PurchaseToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.PurchaseToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.PurchaseToolStripMenuItem.Text = "Purchase"
        Me.PurchaseToolStripMenuItem.Visible = False
        '
        'PartlySalesToolStripMenuItem
        '
        Me.PartlySalesToolStripMenuItem.Name = "PartlySalesToolStripMenuItem"
        Me.PartlySalesToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.PartlySalesToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.PartlySalesToolStripMenuItem.Text = "Partly Sales"
        Me.PartlySalesToolStripMenuItem.Visible = False
        '
        'AdvanceToolStripMenuItem
        '
        Me.AdvanceToolStripMenuItem.Name = "AdvanceToolStripMenuItem"
        Me.AdvanceToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12
        Me.AdvanceToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.AdvanceToolStripMenuItem.Text = "Advance"
        Me.AdvanceToolStripMenuItem.Visible = False
        '
        'CreditToolStripMenuItem
        '
        Me.CreditToolStripMenuItem.Name = "CreditToolStripMenuItem"
        Me.CreditToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11
        Me.CreditToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.CreditToolStripMenuItem.Text = "Credit"
        Me.CreditToolStripMenuItem.Visible = False
        '
        'CreditCardToolStripMenuItem
        '
        Me.CreditCardToolStripMenuItem.Name = "CreditCardToolStripMenuItem"
        Me.CreditCardToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F7
        Me.CreditCardToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.CreditCardToolStripMenuItem.Text = "Credit Card"
        Me.CreditCardToolStripMenuItem.Visible = False
        '
        'ChitCardToolStripMenuItem
        '
        Me.ChitCardToolStripMenuItem.Name = "ChitCardToolStripMenuItem"
        Me.ChitCardToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9
        Me.ChitCardToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.ChitCardToolStripMenuItem.Text = "Scheme"
        Me.ChitCardToolStripMenuItem.Visible = False
        '
        'ChequeToolStripMenuItem
        '
        Me.ChequeToolStripMenuItem.Name = "ChequeToolStripMenuItem"
        Me.ChequeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F8
        Me.ChequeToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.ChequeToolStripMenuItem.Text = "Cheque"
        Me.ChequeToolStripMenuItem.Visible = False
        '
        'HandlingChargeToolStripMenuItem
        '
        Me.HandlingChargeToolStripMenuItem.Name = "HandlingChargeToolStripMenuItem"
        Me.HandlingChargeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6
        Me.HandlingChargeToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.HandlingChargeToolStripMenuItem.Text = "Handling Charge"
        Me.HandlingChargeToolStripMenuItem.Visible = False
        '
        'DiscountToolStripMenuItem
        '
        Me.DiscountToolStripMenuItem.Name = "DiscountToolStripMenuItem"
        Me.DiscountToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.DiscountToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.DiscountToolStripMenuItem.Text = "Discount"
        Me.DiscountToolStripMenuItem.Visible = False
        '
        'CashToolStripMenuItem
        '
        Me.CashToolStripMenuItem.Name = "CashToolStripMenuItem"
        Me.CashToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4
        Me.CashToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.CashToolStripMenuItem.Text = "Cash"
        Me.CashToolStripMenuItem.Visible = False
        '
        'tStripGiftVouhcer
        '
        Me.tStripGiftVouhcer.Name = "tStripGiftVouhcer"
        Me.tStripGiftVouhcer.ShortcutKeys = System.Windows.Forms.Keys.F10
        Me.tStripGiftVouhcer.Size = New System.Drawing.Size(309, 24)
        Me.tStripGiftVouhcer.Text = "Gift Voucher"
        Me.tStripGiftVouhcer.Visible = False
        '
        'cmenuBookingItem
        '
        Me.cmenuBookingItem.Name = "cmenuBookingItem"
        Me.cmenuBookingItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.B), System.Windows.Forms.Keys)
        Me.cmenuBookingItem.Size = New System.Drawing.Size(309, 24)
        Me.cmenuBookingItem.Text = "Booked Item"
        Me.cmenuBookingItem.Visible = False
        '
        'tStripMiscIssue
        '
        Me.tStripMiscIssue.Name = "tStripMiscIssue"
        Me.tStripMiscIssue.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.M), System.Windows.Forms.Keys)
        Me.tStripMiscIssue.Size = New System.Drawing.Size(309, 24)
        Me.tStripMiscIssue.Text = "Misc Issue"
        Me.tStripMiscIssue.Visible = False
        '
        'tStripGiftVoucher
        '
        Me.tStripGiftVoucher.Name = "tStripGiftVoucher"
        Me.tStripGiftVoucher.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.G), System.Windows.Forms.Keys)
        Me.tStripGiftVoucher.Size = New System.Drawing.Size(309, 24)
        Me.tStripGiftVoucher.Text = "Gift Voucher"
        Me.tStripGiftVoucher.Visible = False
        '
        'FinalDiscountToolStripMenuItem
        '
        Me.FinalDiscountToolStripMenuItem.Name = "FinalDiscountToolStripMenuItem"
        Me.FinalDiscountToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F2), System.Windows.Forms.Keys)
        Me.FinalDiscountToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.FinalDiscountToolStripMenuItem.Text = "Final Discount"
        Me.FinalDiscountToolStripMenuItem.Visible = False
        '
        'FinalPurDiscountToolStripMenuItem
        '
        Me.FinalPurDiscountToolStripMenuItem.Name = "FinalPurDiscountToolStripMenuItem"
        Me.FinalPurDiscountToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F3), System.Windows.Forms.Keys)
        Me.FinalPurDiscountToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.FinalPurDiscountToolStripMenuItem.Text = "Final Pur Discount"
        Me.FinalPurDiscountToolStripMenuItem.Visible = False
        '
        'SaleRateChangeToolStripMenuItem
        '
        Me.SaleRateChangeToolStripMenuItem.Name = "SaleRateChangeToolStripMenuItem"
        Me.SaleRateChangeToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.R), System.Windows.Forms.Keys)
        Me.SaleRateChangeToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.SaleRateChangeToolStripMenuItem.Text = "Sale Rate Change"
        Me.SaleRateChangeToolStripMenuItem.Visible = False
        '
        'WastageMcPerToolStripMenuItem
        '
        Me.WastageMcPerToolStripMenuItem.Name = "WastageMcPerToolStripMenuItem"
        Me.WastageMcPerToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F9), System.Windows.Forms.Keys)
        Me.WastageMcPerToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.WastageMcPerToolStripMenuItem.Text = "WastageMcPer"
        Me.WastageMcPerToolStripMenuItem.Visible = False
        '
        'StoneDetailsToolStripMenuItem
        '
        Me.StoneDetailsToolStripMenuItem.Name = "StoneDetailsToolStripMenuItem"
        Me.StoneDetailsToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.StoneDetailsToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.StoneDetailsToolStripMenuItem.Text = "Stone Details"
        Me.StoneDetailsToolStripMenuItem.Visible = False
        '
        'MiscDetailsToolStripMenuItem
        '
        Me.MiscDetailsToolStripMenuItem.Name = "MiscDetailsToolStripMenuItem"
        Me.MiscDetailsToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.MiscDetailsToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.MiscDetailsToolStripMenuItem.Text = "Misc Details"
        Me.MiscDetailsToolStripMenuItem.Visible = False
        '
        'MultiMetalDetailsToolStripMenuItem
        '
        Me.MultiMetalDetailsToolStripMenuItem.Name = "MultiMetalDetailsToolStripMenuItem"
        Me.MultiMetalDetailsToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.M), System.Windows.Forms.Keys)
        Me.MultiMetalDetailsToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.MultiMetalDetailsToolStripMenuItem.Text = "Multi Metal Details"
        Me.MultiMetalDetailsToolStripMenuItem.Visible = False
        '
        'SalesB4DiscountToolStripMenuItem
        '
        Me.SalesB4DiscountToolStripMenuItem.Name = "SalesB4DiscountToolStripMenuItem"
        Me.SalesB4DiscountToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F1), System.Windows.Forms.Keys)
        Me.SalesB4DiscountToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.SalesB4DiscountToolStripMenuItem.Text = "Sales B4 Discount"
        Me.SalesB4DiscountToolStripMenuItem.Visible = False
        '
        'AddressToolStripMenuItem
        '
        Me.AddressToolStripMenuItem.Name = "AddressToolStripMenuItem"
        Me.AddressToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.AddressToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.AddressToolStripMenuItem.Text = "Address"
        Me.AddressToolStripMenuItem.Visible = False
        '
        'ToBeToolStripMenuItem
        '
        Me.ToBeToolStripMenuItem.Name = "ToBeToolStripMenuItem"
        Me.ToBeToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.J), System.Windows.Forms.Keys)
        Me.ToBeToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.ToBeToolStripMenuItem.Text = "Jewel not delivered"
        Me.ToBeToolStripMenuItem.Visible = False
        '
        'ReceiptToolStripMenuItem
        '
        Me.ReceiptToolStripMenuItem.Name = "ReceiptToolStripMenuItem"
        Me.ReceiptToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.ReceiptToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.ReceiptToolStripMenuItem.Text = "Receipt"
        Me.ReceiptToolStripMenuItem.Visible = False
        '
        'PaymentToolStripMenuItem
        '
        Me.PaymentToolStripMenuItem.Name = "PaymentToolStripMenuItem"
        Me.PaymentToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.Y), System.Windows.Forms.Keys)
        Me.PaymentToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.PaymentToolStripMenuItem.Text = "Payment"
        Me.PaymentToolStripMenuItem.Visible = False
        '
        'AppIssueToolStripMenuItem
        '
        Me.AppIssueToolStripMenuItem.Name = "AppIssueToolStripMenuItem"
        Me.AppIssueToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.I), System.Windows.Forms.Keys)
        Me.AppIssueToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.AppIssueToolStripMenuItem.Text = "App Issue"
        Me.AppIssueToolStripMenuItem.Visible = False
        '
        'AppReceiptToolStripMenuItem
        '
        Me.AppReceiptToolStripMenuItem.Name = "AppReceiptToolStripMenuItem"
        Me.AppReceiptToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.T), System.Windows.Forms.Keys)
        Me.AppReceiptToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.AppReceiptToolStripMenuItem.Text = "App Receipt"
        Me.AppReceiptToolStripMenuItem.Visible = False
        '
        'OrderToolStripMenuItem
        '
        Me.OrderToolStripMenuItem.Name = "OrderToolStripMenuItem"
        Me.OrderToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
        Me.OrderToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.OrderToolStripMenuItem.Text = "Order"
        Me.OrderToolStripMenuItem.Visible = False
        '
        'RepairToolStripMenuItem
        '
        Me.RepairToolStripMenuItem.Name = "RepairToolStripMenuItem"
        Me.RepairToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.RepairToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.RepairToolStripMenuItem.Text = "Repair"
        Me.RepairToolStripMenuItem.Visible = False
        '
        'EstCallToolStrip
        '
        Me.EstCallToolStrip.Name = "EstCallToolStrip"
        Me.EstCallToolStrip.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.Insert), System.Windows.Forms.Keys)
        Me.EstCallToolStrip.Size = New System.Drawing.Size(309, 24)
        Me.EstCallToolStrip.Text = "ToolStripMenuItem11"
        Me.EstCallToolStrip.Visible = False
        '
        'ToolStripDupbill
        '
        Me.ToolStripDupbill.Name = "ToolStripDupbill"
        Me.ToolStripDupbill.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
        Me.ToolStripDupbill.Size = New System.Drawing.Size(309, 24)
        Me.ToolStripDupbill.Text = "ToolStripMenuItem11"
        Me.ToolStripDupbill.Visible = False
        '
        'ToolStripSchemeOffer
        '
        Me.ToolStripSchemeOffer.Name = "ToolStripSchemeOffer"
        Me.ToolStripSchemeOffer.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Insert), System.Windows.Forms.Keys)
        Me.ToolStripSchemeOffer.Size = New System.Drawing.Size(309, 24)
        Me.ToolStripSchemeOffer.Text = "ToolStripMenuItem11"
        Me.ToolStripSchemeOffer.Visible = False
        '
        'ToolStripRateView
        '
        Me.ToolStripRateView.Name = "ToolStripRateView"
        Me.ToolStripRateView.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.R), System.Windows.Forms.Keys)
        Me.ToolStripRateView.Size = New System.Drawing.Size(309, 24)
        Me.ToolStripRateView.Text = "ToolStripMenuItem11"
        Me.ToolStripRateView.Visible = False
        '
        'ToolStripBillno
        '
        Me.ToolStripBillno.Name = "ToolStripBillno"
        Me.ToolStripBillno.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.B), System.Windows.Forms.Keys)
        Me.ToolStripBillno.Size = New System.Drawing.Size(309, 24)
        Me.ToolStripBillno.Text = "ToolStripMenuItem11"
        Me.ToolStripBillno.Visible = False
        '
        'Wt2WtAdjustToolStripMenuItem
        '
        Me.Wt2WtAdjustToolStripMenuItem.Name = "Wt2WtAdjustToolStripMenuItem"
        Me.Wt2WtAdjustToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.W), System.Windows.Forms.Keys)
        Me.Wt2WtAdjustToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.Wt2WtAdjustToolStripMenuItem.Text = "Wt2WtAdust Detail"
        Me.Wt2WtAdjustToolStripMenuItem.Visible = False
        '
        'ComplementToolStripMenuItem
        '
        Me.ComplementToolStripMenuItem.Name = "ComplementToolStripMenuItem"
        Me.ComplementToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F10), System.Windows.Forms.Keys)
        Me.ComplementToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.ComplementToolStripMenuItem.Text = "ToolStripMenuItem11"
        Me.ComplementToolStripMenuItem.Visible = False
        '
        'ExcelDownloadToolStripMenuItem
        '
        Me.ExcelDownloadToolStripMenuItem.Name = "ExcelDownloadToolStripMenuItem"
        Me.ExcelDownloadToolStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.ExcelDownloadToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.ExcelDownloadToolStripMenuItem.Text = "Excel Sales Import"
        Me.ExcelDownloadToolStripMenuItem.Visible = False
        '
        'tStripSchemeOffer
        '
        Me.tStripSchemeOffer.Name = "tStripSchemeOffer"
        Me.tStripSchemeOffer.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.tStripSchemeOffer.Size = New System.Drawing.Size(309, 24)
        Me.tStripSchemeOffer.Text = "Scheme Offer"
        Me.tStripSchemeOffer.Visible = False
        '
        'CalcStripMenuItem
        '
        Me.CalcStripMenuItem.Name = "CalcStripMenuItem"
        Me.CalcStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.CalcStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.CalcStripMenuItem.Text = "Calculator"
        Me.CalcStripMenuItem.Visible = False
        '
        'SetItemToolStripMenuItem11
        '
        Me.SetItemToolStripMenuItem11.Name = "SetItemToolStripMenuItem11"
        Me.SetItemToolStripMenuItem11.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.SetItemToolStripMenuItem11.Size = New System.Drawing.Size(309, 24)
        Me.SetItemToolStripMenuItem11.Text = "Enable Set Item"
        Me.SetItemToolStripMenuItem11.Visible = False
        '
        'DisableSetItemStripMenuItem
        '
        Me.DisableSetItemStripMenuItem.Name = "DisableSetItemStripMenuItem"
        Me.DisableSetItemStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
        Me.DisableSetItemStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.DisableSetItemStripMenuItem.Text = "Disable Set Item"
        Me.DisableSetItemStripMenuItem.Visible = False
        '
        'TagCompMoveToolStripMenuItem
        '
        Me.TagCompMoveToolStripMenuItem.Name = "TagCompMoveToolStripMenuItem"
        Me.TagCompMoveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.TagCompMoveToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.TagCompMoveToolStripMenuItem.Text = "ToolStripMenuItem11"
        Me.TagCompMoveToolStripMenuItem.Visible = False
        '
        'tStripSGiftVoucher
        '
        Me.tStripSGiftVoucher.Name = "tStripSGiftVoucher"
        Me.tStripSGiftVoucher.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.F10), System.Windows.Forms.Keys)
        Me.tStripSGiftVoucher.Size = New System.Drawing.Size(309, 24)
        Me.tStripSGiftVoucher.Text = "Silver Gift Voucher"
        Me.tStripSGiftVoucher.Visible = False
        '
        'BillTypeToolStripMenuItem
        '
        Me.BillTypeToolStripMenuItem.Name = "BillTypeToolStripMenuItem"
        Me.BillTypeToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F1), System.Windows.Forms.Keys)
        Me.BillTypeToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.BillTypeToolStripMenuItem.Text = "Change Bill Type"
        Me.BillTypeToolStripMenuItem.Visible = False
        '
        'PrivilegeDiscountToolStripMenuItem
        '
        Me.PrivilegeDiscountToolStripMenuItem.Name = "PrivilegeDiscountToolStripMenuItem"
        Me.PrivilegeDiscountToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.I), System.Windows.Forms.Keys)
        Me.PrivilegeDiscountToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.PrivilegeDiscountToolStripMenuItem.Text = "ToolStripMenuItem11"
        Me.PrivilegeDiscountToolStripMenuItem.Visible = False
        '
        'PruchaseOrderRate
        '
        Me.PruchaseOrderRate.Name = "PruchaseOrderRate"
        Me.PruchaseOrderRate.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.F6), System.Windows.Forms.Keys)
        Me.PruchaseOrderRate.Size = New System.Drawing.Size(309, 24)
        Me.PruchaseOrderRate.Text = "Pruchase Order Rate"
        Me.PruchaseOrderRate.Visible = False
        '
        'KFC
        '
        Me.KFC.Name = "KFC"
        Me.KFC.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F8), System.Windows.Forms.Keys)
        Me.KFC.Size = New System.Drawing.Size(309, 24)
        Me.KFC.Text = "ToolStripMenuItem11"
        Me.KFC.Visible = False
        '
        'ToolStripRowFinalAmt
        '
        Me.ToolStripRowFinalAmt.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripRowFinalAmt.Name = "ToolStripRowFinalAmt"
        Me.ToolStripRowFinalAmt.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F7), System.Windows.Forms.Keys)
        Me.ToolStripRowFinalAmt.Size = New System.Drawing.Size(309, 24)
        Me.ToolStripRowFinalAmt.Text = "Row Final Amount"
        Me.ToolStripRowFinalAmt.Visible = False
        '
        'SecondSalesToolStripMenuItem
        '
        Me.SecondSalesToolStripMenuItem.Name = "SecondSalesToolStripMenuItem"
        Me.SecondSalesToolStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.F10), System.Windows.Forms.Keys)
        Me.SecondSalesToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.SecondSalesToolStripMenuItem.Text = "Second Sales"
        Me.SecondSalesToolStripMenuItem.Visible = False
        '
        'TrfNoToolStripMenuItem
        '
        Me.TrfNoToolStripMenuItem.Name = "TrfNoToolStripMenuItem"
        Me.TrfNoToolStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.T), System.Windows.Forms.Keys)
        Me.TrfNoToolStripMenuItem.Size = New System.Drawing.Size(309, 24)
        Me.TrfNoToolStripMenuItem.Text = "Transfer No Misc Issue"
        Me.TrfNoToolStripMenuItem.Visible = False
        '
        'ToolStripHallmarkDetails
        '
        Me.ToolStripHallmarkDetails.Name = "ToolStripHallmarkDetails"
        Me.ToolStripHallmarkDetails.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.H), System.Windows.Forms.Keys)
        Me.ToolStripHallmarkDetails.Size = New System.Drawing.Size(309, 24)
        Me.ToolStripHallmarkDetails.Text = "Hallmark Details"
        Me.ToolStripHallmarkDetails.Visible = False
        '
        'TStripShipingAddress
        '
        Me.TStripShipingAddress.Name = "TStripShipingAddress"
        Me.TStripShipingAddress.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.TStripShipingAddress.Size = New System.Drawing.Size(309, 24)
        Me.TStripShipingAddress.Text = "Shiping Address"
        Me.TStripShipingAddress.Visible = False
        '
        'ToolTip1
        '
        Me.ToolTip1.AutoPopDelay = 5000
        Me.ToolTip1.InitialDelay = 2000
        Me.ToolTip1.ReshowDelay = 100
        '
        'StatusStrip1
        '
        Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 676)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1014, 22)
        Me.StatusStrip1.TabIndex = 4
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'clockTimer
        '
        Me.clockTimer.Enabled = True
        Me.clockTimer.Interval = 60
        '
        'mTimer
        '
        Me.mTimer.Enabled = True
        Me.mTimer.Interval = 600
        '
        'lblTcsCalcAmt
        '
        Me.lblTcsCalcAmt.AutoSize = True
        Me.lblTcsCalcAmt.BackColor = System.Drawing.Color.Transparent
        Me.lblTcsCalcAmt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTcsCalcAmt.Location = New System.Drawing.Point(447, 688)
        Me.lblTcsCalcAmt.Name = "lblTcsCalcAmt"
        Me.lblTcsCalcAmt.Size = New System.Drawing.Size(103, 18)
        Me.lblTcsCalcAmt.TabIndex = 36
        Me.lblTcsCalcAmt.Text = "TcsCalcAmt"
        Me.lblTcsCalcAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblTcsCalcAmt.Visible = False
        '
        'lblTcsCalcPrecent
        '
        Me.lblTcsCalcPrecent.AutoSize = True
        Me.lblTcsCalcPrecent.BackColor = System.Drawing.Color.Transparent
        Me.lblTcsCalcPrecent.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTcsCalcPrecent.Location = New System.Drawing.Point(364, 688)
        Me.lblTcsCalcPrecent.Name = "lblTcsCalcPrecent"
        Me.lblTcsCalcPrecent.Size = New System.Drawing.Size(96, 18)
        Me.lblTcsCalcPrecent.TabIndex = 37
        Me.lblTcsCalcPrecent.Text = "TcsCalcPer"
        Me.lblTcsCalcPrecent.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblTcsCalcPrecent.Visible = False
        '
        'lblSNo
        '
        Me.lblSNo.AutoSize = True
        Me.lblSNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSNo.Location = New System.Drawing.Point(644, 34)
        Me.lblSNo.Name = "lblSNo"
        Me.lblSNo.Size = New System.Drawing.Size(24, 18)
        Me.lblSNo.TabIndex = 51
        Me.lblSNo.Text = "[]"
        '
        'lblPNo
        '
        Me.lblPNo.AutoSize = True
        Me.lblPNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPNo.Location = New System.Drawing.Point(644, 55)
        Me.lblPNo.Name = "lblPNo"
        Me.lblPNo.Size = New System.Drawing.Size(24, 18)
        Me.lblPNo.TabIndex = 52
        Me.lblPNo.Text = "[]"
        '
        'RetailBill
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.ClientSize = New System.Drawing.Size(1014, 698)
        Me.ContextMenuStrip = Me.ContextMenuStrip1
        Me.Controls.Add(Me.lblTcsCalcPrecent)
        Me.Controls.Add(Me.lblTcsCalcAmt)
        Me.Controls.Add(Me.lblHelpText)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.grpHeader)
        Me.Controls.Add(Me.tabMain)
        Me.Controls.Add(Me.tabOtherOptions)
        Me.Controls.Add(Me.grpAdj)
        Me.Controls.Add(Me.grpOptions)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.MaximumSize = New System.Drawing.Size(1032, 745)
        Me.Name = "RetailBill"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Point Of Sale"
        Me.tabMain.ResumeLayout(False)
        Me.tabSaSrPu.ResumeLayout(False)
        Me.grpSaSr.ResumeLayout(False)
        Me.grpSaSr.PerformLayout()
        CType(Me.gridSASRTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridSASR, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpPu.ResumeLayout(False)
        Me.grpPu.PerformLayout()
        CType(Me.gridPur, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridPurTotal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabReceipt.ResumeLayout(False)
        Me.grpReceipt.ResumeLayout(False)
        Me.grpReceipt.PerformLayout()
        Me.cmenuReceiptGrid.ResumeLayout(False)
        CType(Me.gridReceipt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridReceiptTotal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabPayment.ResumeLayout(False)
        Me.grpPayment.ResumeLayout(False)
        Me.grpPayment.PerformLayout()
        CType(Me.gridPayment, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridPaymentTotal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabGiftVoucherMain.ResumeLayout(False)
        Me.grpGiftVoucherMain.ResumeLayout(False)
        Me.grpGiftVoucherMain.PerformLayout()
        CType(Me.gridMGiftVoucherTotal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridMGiftVoucher, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpRecReservedItem.ResumeLayout(False)
        Me.grpRecReservedItem.PerformLayout()
        CType(Me.gridReceiptReserved, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpReceiptWeightAdvance.ResumeLayout(False)
        Me.grpReceiptWeightAdvance.PerformLayout()
        Me.tabOtherOptions.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.grpGeneral1.ResumeLayout(False)
        Me.pnlFinalBalance.ResumeLayout(False)
        Me.pnlFinalBalance.PerformLayout()
        Me.Grouper1.ResumeLayout(False)
        Me.Grouper1.PerformLayout()
        Me.grpGeneral.ResumeLayout(False)
        Me.grpGeneral.PerformLayout()
        Me.tabHideDet.ResumeLayout(False)
        Me.tabHideDet.PerformLayout()
        Me.grpStudedDetail.ResumeLayout(False)
        Me.grpStudedDetail.PerformLayout()
        Me.pnlShortCut.ResumeLayout(False)
        Me.pnlShortCut.PerformLayout()
        CType(Me.picTagImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlFinalPURAmount_OWN.ResumeLayout(False)
        Me.pnlFinalPURAmount_OWN.PerformLayout()
        Me.pnlPuExtaDetails.ResumeLayout(False)
        Me.pnlPuExtaDetails.PerformLayout()
        Me.tabReceiptWeightAdvance.ResumeLayout(False)
        Me.tabReceiptReserve.ResumeLayout(False)
        Me.tabOrderDetail.ResumeLayout(False)
        Me.grpOrderDetail.ResumeLayout(False)
        Me.grpOrderDetail.PerformLayout()
        Me.tabAdvanceWeightAdjCalc.ResumeLayout(False)
        Me.grpAdvanceCalc.ResumeLayout(False)
        CType(Me.gridAdvanceAdjCalc, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabWholeSaleDetails.ResumeLayout(False)
        Me.grpWholeSaleDetails.ResumeLayout(False)
        CType(Me.dgvWholeSaleDetail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpAdj.ResumeLayout(False)
        Me.grpAdj.PerformLayout()
        Me.grpOptions.ResumeLayout(False)
        CType(Me.picGiftVoucher, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picRecPay, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picOrderRepair, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picMiscIssue, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picPurchase, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picReturn, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picSales, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picAppIssRec, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cMenuRecPay.ResumeLayout(False)
        Me.cMenuOrderRepair.ResumeLayout(False)
        Me.cMenuApproval.ResumeLayout(False)
        Me.grpHeader.ResumeLayout(False)
        Me.grpHeader.PerformLayout()
        Me.cmenuTemplate.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtSAItemId As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtSAPcs_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtSANetWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtSAGrsWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtSARate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtSAWastage_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtSAMc_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtSAAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtSAVat_AMT As System.Windows.Forms.TextBox
    Friend WithEvents lblSaVat As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtPUPcs_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtPUGrsWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents txtPUDustWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents txtPUWastage_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents txtPUStoneWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents txtPUNetWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents txtPURate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents txtPUAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents txtPUVat_AMT As System.Windows.Forms.TextBox
    Friend WithEvents lblPUVat As System.Windows.Forms.Label
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabSaSrPu As System.Windows.Forms.TabPage
    Friend WithEvents tabReceipt As System.Windows.Forms.TabPage
    Friend WithEvents txtSAEstNo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtSATagNo As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents tabOtherOptions As System.Windows.Forms.TabControl
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents lblSilverRate As System.Windows.Forms.Label
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents lblGoldRate As System.Windows.Forms.Label
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents lblCashCounter As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents lblBillDate As System.Windows.Forms.Label
    Friend WithEvents tabPayment As System.Windows.Forms.TabPage
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents txtAdjGiftVoucher_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtAdjAdvance_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtAdjCheque_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtAdjRoundoff_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtAdjDiscount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtAdjChitCard_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtAdjHandlingCharge_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtAdjCredit_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtAdjCreditCard_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents Label28 As System.Windows.Forms.Label
    'Friend WithEvents txtAdjCash_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtAdjReceive_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents Label44 As System.Windows.Forms.Label
    Friend WithEvents lblHc As System.Windows.Forms.Label
    Friend WithEvents grpAdj As CodeVendor.Controls.Grouper
    Friend WithEvents grpSaSr As CodeVendor.Controls.Grouper
    Friend WithEvents grpPu As CodeVendor.Controls.Grouper
    Friend WithEvents grpOptions As CodeVendor.Controls.Grouper
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents chkPartlySales As System.Windows.Forms.CheckBox
    Friend WithEvents grpHeader As CodeVendor.Controls.Grouper
    Friend WithEvents grpReceipt As CodeVendor.Controls.Grouper
    Friend WithEvents lblUserName As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SalesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SalesReturnToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnGiftVoucher_OWN As System.Windows.Forms.Button
    Friend WithEvents btnMiscIssue_OWN As System.Windows.Forms.Button
    Friend WithEvents btnSalesReturn_OWN As System.Windows.Forms.Button
    Friend WithEvents btnSales_OWN As System.Windows.Forms.Button
    Friend WithEvents btnPurchase_OWN As System.Windows.Forms.Button
    Friend WithEvents PurchaseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gridSASR As System.Windows.Forms.DataGridView
    Friend WithEvents gridSASRTotal As System.Windows.Forms.DataGridView
    Friend WithEvents gridPurTotal As System.Windows.Forms.DataGridView
    Friend WithEvents gridPur As System.Windows.Forms.DataGridView
    Friend WithEvents tabSAExtraDetail As System.Windows.Forms.TabPage
    Friend WithEvents txtSAVatPer_PER As System.Windows.Forms.TextBox
    Friend WithEvents txtSAStoneAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtSAGrossAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents txtSAEmpId_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label62 As System.Windows.Forms.Label
    Friend WithEvents txtPUEmpId_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label63 As System.Windows.Forms.Label
    Friend WithEvents grpPayment As CodeVendor.Controls.Grouper
    Friend WithEvents gridReceipt As System.Windows.Forms.DataGridView
    Friend WithEvents cmbReceiptTranType As System.Windows.Forms.ComboBox
    Friend WithEvents cmbReceiptReceiptType As System.Windows.Forms.ComboBox
    Friend WithEvents txtReceiptRate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtReceiptAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtReceiptRefNo As System.Windows.Forms.TextBox
    Friend WithEvents Label169 As System.Windows.Forms.Label
    Friend WithEvents Label168 As System.Windows.Forms.Label
    Friend WithEvents lblRecTrantype As System.Windows.Forms.Label
    Friend WithEvents lblRecRefno As System.Windows.Forms.Label
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents gridReceiptTotal As System.Windows.Forms.DataGridView
    Friend WithEvents Label170 As System.Windows.Forms.Label
    Friend WithEvents Label171 As System.Windows.Forms.Label
    Friend WithEvents Label173 As System.Windows.Forms.Label
    Friend WithEvents Label172 As System.Windows.Forms.Label
    Friend WithEvents Label174 As System.Windows.Forms.Label
    Friend WithEvents txtReceiptValue_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtReceiptGrsWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtReceiptNetWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtReceiptWastage_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtReceiptBullionRate_RATE As System.Windows.Forms.TextBox
    Friend WithEvents Label176 As System.Windows.Forms.Label
    Friend WithEvents cmbReceiptCategory As System.Windows.Forms.ComboBox
    Friend WithEvents txtReceiptPurity_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label177 As System.Windows.Forms.Label
    Friend WithEvents grpReceiptWeightAdvance As CodeVendor.Controls.Grouper
    Friend WithEvents grpRecReservedItem As CodeVendor.Controls.Grouper
    Friend WithEvents chkReceiptRateFix As System.Windows.Forms.CheckBox
    Friend WithEvents gridReceiptReserved As System.Windows.Forms.DataGridView
    Friend WithEvents txtReceiptTagNo As System.Windows.Forms.TextBox
    Friend WithEvents txtReceiptItemId_MAN As System.Windows.Forms.TextBox
    Friend WithEvents Label66 As System.Windows.Forms.Label
    Friend WithEvents Label65 As System.Windows.Forms.Label
    Friend WithEvents txtReceiptItemName As System.Windows.Forms.TextBox
    Friend WithEvents Label67 As System.Windows.Forms.Label
    Friend WithEvents txtReceiptValue As System.Windows.Forms.TextBox
    Friend WithEvents txtReceiptNetWT As System.Windows.Forms.TextBox
    Friend WithEvents txtReceiptPcs As System.Windows.Forms.TextBox
    Friend WithEvents txtReceiptGrsWt As System.Windows.Forms.TextBox
    Friend WithEvents Label73 As System.Windows.Forms.Label
    Friend WithEvents Label70 As System.Windows.Forms.Label
    Friend WithEvents Label69 As System.Windows.Forms.Label
    Friend WithEvents Label68 As System.Windows.Forms.Label
    Friend WithEvents cmenuReceiptGrid As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents tStripWeightAdvance As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tagReservation As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabHideDet As System.Windows.Forms.TabPage
    Friend WithEvents PartlySalesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gridPayment As System.Windows.Forms.DataGridView
    Friend WithEvents cmbPaymentTranType As System.Windows.Forms.ComboBox
    Friend WithEvents cmbPaymentPaytype As System.Windows.Forms.ComboBox
    Friend WithEvents txtPaymentRate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtPaymentAmount_Amt As System.Windows.Forms.TextBox
    Friend WithEvents txtPaymentRefNo As System.Windows.Forms.TextBox
    Friend WithEvents gridPaymentTotal As System.Windows.Forms.DataGridView
    Friend WithEvents Label77 As System.Windows.Forms.Label
    Friend WithEvents lblPayType As System.Windows.Forms.Label
    Friend WithEvents Label81 As System.Windows.Forms.Label
    Friend WithEvents Label82 As System.Windows.Forms.Label
    Friend WithEvents Label83 As System.Windows.Forms.Label
    Friend WithEvents Label86 As System.Windows.Forms.Label
    Friend WithEvents Label84 As System.Windows.Forms.Label
    Friend WithEvents lblNodeId As System.Windows.Forms.Label
    Friend WithEvents Label89 As System.Windows.Forms.Label
    Friend WithEvents Label90 As System.Windows.Forms.Label
    Friend WithEvents Label91 As System.Windows.Forms.Label
    Friend WithEvents Label88 As System.Windows.Forms.Label
    Friend WithEvents Label87 As System.Windows.Forms.Label
    Friend WithEvents AdvanceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CreditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChitCardToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChequeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CreditCardToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HandlingChargeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DiscountToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CashToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmenuTemplate As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents Style1ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Style2ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Style3ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Style4ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Style5ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Style6ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblSystemId As System.Windows.Forms.Label
    Friend WithEvents txtReceiptEntAmount As System.Windows.Forms.TextBox
    Friend WithEvents txtReceiptEntRefNo As System.Windows.Forms.TextBox
    Friend WithEvents txtPaymentEntRefNo As System.Windows.Forms.TextBox
    Friend WithEvents txtPaymentEntAmount As System.Windows.Forms.TextBox
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents tStripGiftVouhcer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabGiftVoucherMain As System.Windows.Forms.TabPage
    Friend WithEvents grpGiftVoucherMain As CodeVendor.Controls.Grouper
    Friend WithEvents Label85 As System.Windows.Forms.Label
    Friend WithEvents txtMGiftRemark As System.Windows.Forms.TextBox
    Friend WithEvents gridMGiftVoucherTotal As System.Windows.Forms.DataGridView
    Friend WithEvents gridMGiftVoucher As System.Windows.Forms.DataGridView
    Friend WithEvents Label92 As System.Windows.Forms.Label
    Friend WithEvents Label93 As System.Windows.Forms.Label
    Friend WithEvents Label94 As System.Windows.Forms.Label
    Friend WithEvents Label95 As System.Windows.Forms.Label
    Friend WithEvents txtMGiftAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtMGiftUnit_NUM As System.Windows.Forms.TextBox
    Friend WithEvents txtMGiftDenomination_AMT As System.Windows.Forms.TextBox
    Friend WithEvents cmbMGiftVoucherType As System.Windows.Forms.ComboBox
    Friend WithEvents txtMGiftRowIndex As System.Windows.Forms.TextBox
    Friend WithEvents cmenuBookingItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtSARowIndex As System.Windows.Forms.TextBox
    Friend WithEvents txtPUCategory As System.Windows.Forms.TextBox
    Friend WithEvents txtPURowIndex As System.Windows.Forms.TextBox
    Friend WithEvents txtPUEstNo_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label96 As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents lblHelpText As System.Windows.Forms.Label
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents tStripMiscIssue As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tStripGiftVoucher As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtSaDiscountAfterTax As System.Windows.Forms.TextBox
    Friend WithEvents FinalDiscountToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FinalPurDiscountToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlFinalPURAmount_OWN As System.Windows.Forms.Panel
    Friend WithEvents txtFinalPURAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label97 As System.Windows.Forms.Label
    Friend WithEvents Label103 As System.Windows.Forms.Label
    Friend WithEvents Label104 As System.Windows.Forms.Label
    Friend WithEvents Label105 As System.Windows.Forms.Label
    Friend WithEvents Label106 As System.Windows.Forms.Label
    Friend WithEvents Label107 As System.Windows.Forms.Label
    Friend WithEvents pnlPuExtaDetails As System.Windows.Forms.Panel
    Friend WithEvents Label124 As System.Windows.Forms.Label
    Friend WithEvents txtPuDiscount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents SaleRateChangeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents grpGeneral As CodeVendor.Controls.Grouper
    Friend WithEvents lblFinalBalance As System.Windows.Forms.Label
    Friend WithEvents lblFinalBalanceStatus As System.Windows.Forms.Label
    Friend WithEvents Label110 As System.Windows.Forms.Label
    Friend WithEvents pnlFinalBalance As System.Windows.Forms.Panel
    Friend WithEvents txtDetItem As System.Windows.Forms.TextBox
    Friend WithEvents Label112 As System.Windows.Forms.Label
    Friend WithEvents Label111 As System.Windows.Forms.Label
    Friend WithEvents txtDetCounter As System.Windows.Forms.TextBox
    Friend WithEvents txtDetSubItem As System.Windows.Forms.TextBox
    Friend WithEvents Label113 As System.Windows.Forms.Label
    Friend WithEvents Label116 As System.Windows.Forms.Label
    Friend WithEvents Label115 As System.Windows.Forms.Label
    Friend WithEvents Label114 As System.Windows.Forms.Label
    Friend WithEvents Label118 As System.Windows.Forms.Label
    Friend WithEvents Label117 As System.Windows.Forms.Label
    Friend WithEvents Label127 As System.Windows.Forms.Label
    Friend WithEvents Label133 As System.Windows.Forms.Label
    Friend WithEvents Label134 As System.Windows.Forms.Label
    Friend WithEvents Label121 As System.Windows.Forms.Label
    Friend WithEvents Label125 As System.Windows.Forms.Label
    Friend WithEvents txtDetValueAdded As System.Windows.Forms.TextBox
    Friend WithEvents txtDetItemType As System.Windows.Forms.TextBox
    Friend WithEvents txtDetCalcType As System.Windows.Forms.TextBox
    Friend WithEvents txtDetStockType As System.Windows.Forms.TextBox
    Friend WithEvents txtDetMiscAmt As System.Windows.Forms.TextBox
    Friend WithEvents txtDetTableCode As System.Windows.Forms.TextBox
    Friend WithEvents txtDetMcPerGrm As System.Windows.Forms.TextBox
    Friend WithEvents txtDetWastagePer As System.Windows.Forms.TextBox
    Friend WithEvents txtDetLessWt As System.Windows.Forms.TextBox
    Friend WithEvents txtDetGrsNet As System.Windows.Forms.TextBox
    Friend WithEvents WastageMcPerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label60 As System.Windows.Forms.Label
    Friend WithEvents Label59 As System.Windows.Forms.Label
    Friend WithEvents Label57 As System.Windows.Forms.Label
    Friend WithEvents Label54 As System.Windows.Forms.Label
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents Label47 As System.Windows.Forms.Label
    Friend WithEvents Label46 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents tabReceiptWeightAdvance As System.Windows.Forms.TabPage
    Friend WithEvents tabReceiptReserve As System.Windows.Forms.TabPage
    Friend WithEvents txtReceiptRemark As System.Windows.Forms.TextBox
    Friend WithEvents Label58 As System.Windows.Forms.Label
    Friend WithEvents Label56 As System.Windows.Forms.Label
    Friend WithEvents Grouper1 As CodeVendor.Controls.Grouper
    Friend WithEvents txtDetDiscount As System.Windows.Forms.TextBox
    Friend WithEvents picTagImage As System.Windows.Forms.PictureBox
    Friend WithEvents StoneDetailsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MiscDetailsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MultiMetalDetailsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtPaymentRemark As System.Windows.Forms.TextBox
    Friend WithEvents Label74 As System.Windows.Forms.Label
    Friend WithEvents SalesB4DiscountToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblDetVat As System.Windows.Forms.Label
    Friend WithEvents AddressToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label48 As System.Windows.Forms.Label
    Friend WithEvents txtPuWastage_PER As System.Windows.Forms.TextBox
    Friend WithEvents Label51 As System.Windows.Forms.Label
    Friend WithEvents tabOrderDetail As System.Windows.Forms.TabPage
    Friend WithEvents grpOrderDetail As CodeVendor.Controls.Grouper
    Friend WithEvents txtOrdTotalWeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtOrdBalanceWeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtOrdAdvanceWeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents Label79 As System.Windows.Forms.Label
    Friend WithEvents Label78 As System.Windows.Forms.Label
    Friend WithEvents Label53 As System.Windows.Forms.Label
    Friend WithEvents Label52 As System.Windows.Forms.Label
    Friend WithEvents txtOrdVat_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtOrdRate_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtOrdAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents lblOrdVat As System.Windows.Forms.Label
    Friend WithEvents Label98 As System.Windows.Forms.Label
    Friend WithEvents Label100 As System.Windows.Forms.Label
    Friend WithEvents grpGeneral1 As CodeVendor.Controls.Grouper
    Friend WithEvents btnApproval_OWN As System.Windows.Forms.Button
    Friend WithEvents cMenuApproval As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents tStripApprovalIssue As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tStripApprovalReceipt As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnOrderRepair_OWN As System.Windows.Forms.Button
    Friend WithEvents cMenuOrderRepair As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents tStripOrder As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tStripRepair As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnRecPay_OWN As System.Windows.Forms.Button
    Friend WithEvents cMenuRecPay As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents tStripReceipt As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tStripPayment As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtPUPurity_PER As System.Windows.Forms.TextBox
    Friend WithEvents ToBeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtAdjSrCredit_AMT As System.Windows.Forms.TextBox
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents Label135 As System.Windows.Forms.Label
    Friend WithEvents txtShortCut As System.Windows.Forms.TextBox
    Friend WithEvents pnlShortCut As System.Windows.Forms.Panel
    Friend WithEvents tabAdvanceWeightAdjCalc As System.Windows.Forms.TabPage
    Friend WithEvents grpAdvanceCalc As CodeVendor.Controls.Grouper
    Friend WithEvents gridAdvanceAdjCalc As System.Windows.Forms.DataGridView
    Friend WithEvents Label148 As System.Windows.Forms.Label
    Friend WithEvents Label166 As System.Windows.Forms.Label
    Friend WithEvents Label165 As System.Windows.Forms.Label
    Friend WithEvents Label164 As System.Windows.Forms.Label
    Friend WithEvents Label163 As System.Windows.Forms.Label
    Friend WithEvents Label162 As System.Windows.Forms.Label
    Friend WithEvents Label161 As System.Windows.Forms.Label
    Friend WithEvents Label158 As System.Windows.Forms.Label
    Friend WithEvents Label157 As System.Windows.Forms.Label
    Friend WithEvents Label156 As System.Windows.Forms.Label
    Friend WithEvents Label155 As System.Windows.Forms.Label
    Friend WithEvents Label154 As System.Windows.Forms.Label
    Friend WithEvents Label153 As System.Windows.Forms.Label
    Friend WithEvents Label152 As System.Windows.Forms.Label
    Friend WithEvents Label151 As System.Windows.Forms.Label
    Friend WithEvents Label150 As System.Windows.Forms.Label
    Friend WithEvents Label149 As System.Windows.Forms.Label
    Friend WithEvents Label198 As System.Windows.Forms.Label
    Friend WithEvents Label185 As System.Windows.Forms.Label
    Friend WithEvents Label197 As System.Windows.Forms.Label
    Friend WithEvents Label184 As System.Windows.Forms.Label
    Friend WithEvents Label196 As System.Windows.Forms.Label
    Friend WithEvents Label183 As System.Windows.Forms.Label
    Friend WithEvents Label182 As System.Windows.Forms.Label
    Friend WithEvents Label193 As System.Windows.Forms.Label
    Friend WithEvents Label181 As System.Windows.Forms.Label
    Friend WithEvents Label192 As System.Windows.Forms.Label
    Friend WithEvents Label180 As System.Windows.Forms.Label
    Friend WithEvents Label190 As System.Windows.Forms.Label
    Friend WithEvents Label179 As System.Windows.Forms.Label
    Friend WithEvents Label189 As System.Windows.Forms.Label
    Friend WithEvents Label178 As System.Windows.Forms.Label
    Friend WithEvents Label175 As System.Windows.Forms.Label
    Friend WithEvents Label186 As System.Windows.Forms.Label
    Friend WithEvents Label187 As System.Windows.Forms.Label
    Friend WithEvents Label204 As System.Windows.Forms.Label
    Friend WithEvents Label203 As System.Windows.Forms.Label
    Friend WithEvents Label202 As System.Windows.Forms.Label
    Friend WithEvents Label201 As System.Windows.Forms.Label
    Friend WithEvents lblBookedItem As System.Windows.Forms.Label
    Friend WithEvents Label188 As System.Windows.Forms.Label
    Friend WithEvents picAppIssRec As System.Windows.Forms.PictureBox
    Friend WithEvents picGiftVoucher As System.Windows.Forms.PictureBox
    Friend WithEvents picRecPay As System.Windows.Forms.PictureBox
    Friend WithEvents picOrderRepair As System.Windows.Forms.PictureBox
    Friend WithEvents picMiscIssue As System.Windows.Forms.PictureBox
    Friend WithEvents picPurchase As System.Windows.Forms.PictureBox
    Friend WithEvents picReturn As System.Windows.Forms.PictureBox
    Friend WithEvents picSales As System.Windows.Forms.PictureBox
    Friend WithEvents ReceiptToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PaymentToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AppIssueToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AppReceiptToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OrderToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RepairToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label71 As System.Windows.Forms.Label
    Friend WithEvents txtDetDiffDia As System.Windows.Forms.TextBox
    Friend WithEvents Label50 As System.Windows.Forms.Label
    Friend WithEvents txtDetDiffGrsNet As System.Windows.Forms.TextBox
    Friend WithEvents Label61 As System.Windows.Forms.Label
    Friend WithEvents Label64 As System.Windows.Forms.Label
    Friend WithEvents lblCompanyName As System.Windows.Forms.Label
    Friend WithEvents CompanySelectionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Company1ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Company2ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Company3ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Company4ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Company5ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Company6ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Company7ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Company8ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Company9ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Company0ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem5 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem6 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem7 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem8 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem9 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem10 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtReceiptEmpId_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents txtPaymentEmpId_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents tabWholeSaleDetails As System.Windows.Forms.TabPage
    Friend WithEvents grpWholeSaleDetails As CodeVendor.Controls.Grouper
    Friend WithEvents dgvWholeSaleDetail As System.Windows.Forms.DataGridView
    Friend WithEvents grpStudedDetail As System.Windows.Forms.GroupBox
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents lblDetStnPcs As System.Windows.Forms.Label
    Friend WithEvents lblDetDiaWt As System.Windows.Forms.Label
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents Label102 As System.Windows.Forms.Label
    Friend WithEvents lblDetDiaPcs As System.Windows.Forms.Label
    Friend WithEvents lblDetStnWt As System.Windows.Forms.Label
    Friend WithEvents txtPurAlloy As System.Windows.Forms.TextBox
    Friend WithEvents txtDetRateId As System.Windows.Forms.TextBox
    Friend WithEvents Label72 As System.Windows.Forms.Label
    Friend WithEvents txtSASurCharge As System.Windows.Forms.TextBox
    Friend WithEvents Label75 As System.Windows.Forms.Label
    Friend WithEvents chkAppSales As System.Windows.Forms.CheckBox
    Friend WithEvents lblFinalpercent As System.Windows.Forms.Label
    Friend WithEvents txtAdjCash_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtTCS_Amt As System.Windows.Forms.TextBox
    Friend WithEvents lblTCS As System.Windows.Forms.Label
    Friend WithEvents lblRecOnAcc As System.Windows.Forms.Label
    Friend WithEvents Label101 As System.Windows.Forms.Label
    Friend WithEvents txtDifftot As System.Windows.Forms.TextBox
    Friend WithEvents lblEsthelp As System.Windows.Forms.Label
    Friend WithEvents EstCallToolStrip As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripDupbill As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSchemeOffer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripRateView As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripBillno As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtPUMeltWt_WET As System.Windows.Forms.TextBox
    Friend WithEvents txtReceiptAccount As System.Windows.Forms.TextBox
    Friend WithEvents txtPaymentAccount As System.Windows.Forms.TextBox
    Friend WithEvents txtExMcvat As System.Windows.Forms.TextBox
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents lblDiscper As System.Windows.Forms.Label
    Friend WithEvents txtAdjDiscount_PER As System.Windows.Forms.TextBox
    Friend WithEvents txtReceiptEst_Num As System.Windows.Forms.TextBox
    Friend WithEvents Label108 As System.Windows.Forms.Label
    Friend WithEvents txtOrdDisc As System.Windows.Forms.TextBox
    Friend WithEvents txtOrdGrossAmount_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtOrdAdjdisc As System.Windows.Forms.TextBox
    Friend WithEvents Label109 As System.Windows.Forms.Label
    Friend WithEvents Label119 As System.Windows.Forms.Label
    Friend WithEvents Label120 As System.Windows.Forms.Label
    Friend WithEvents Label122 As System.Windows.Forms.Label
    Friend WithEvents clockTimer As System.Windows.Forms.Timer
    Friend WithEvents txtPayRefAccode As System.Windows.Forms.TextBox
    Friend WithEvents Label123 As System.Windows.Forms.Label
    Friend WithEvents Label129 As System.Windows.Forms.Label
    Friend WithEvents Label130 As System.Windows.Forms.Label
    Friend WithEvents Label131 As System.Windows.Forms.Label
    Friend WithEvents Label132 As System.Windows.Forms.Label
    Friend WithEvents Label128 As System.Windows.Forms.Label
    Friend WithEvents Wt2WtAdjustToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ComplementToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label137 As System.Windows.Forms.Label
    Friend WithEvents txtReceiptType As System.Windows.Forms.TextBox
    Friend WithEvents txtDetDesigner As System.Windows.Forms.TextBox
    Friend WithEvents Label76 As System.Windows.Forms.Label
    Friend WithEvents ExcelDownloadToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnAttachImage As System.Windows.Forms.Button
    Friend WithEvents tStripSchemeOffer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnCalc As System.Windows.Forms.Button
    Friend WithEvents CalcStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtDetTagGrsNet As System.Windows.Forms.TextBox
    Friend WithEvents Label139 As System.Windows.Forms.Label
    Friend WithEvents txtDetVAMin As System.Windows.Forms.TextBox
    Friend WithEvents txtDetVAMax As System.Windows.Forms.TextBox
    Friend WithEvents Label141 As System.Windows.Forms.Label
    Friend WithEvents SetItemToolStripMenuItem11 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DisableSetItemStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtRecEmpId_NUM As System.Windows.Forms.TextBox
    Friend WithEvents Label140 As System.Windows.Forms.Label
    Friend WithEvents TagCompMoveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtPaymentWeight_WET As System.Windows.Forms.TextBox
    Friend WithEvents tStripSGiftVoucher As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtReceiptGST_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtReceiptTotAmt_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtOrdCGST_AMT As System.Windows.Forms.TextBox
    Friend WithEvents txtOrdSGST_AMT As System.Windows.Forms.TextBox
    Friend WithEvents BillTypeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtPaymentTotAmt_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents txtPaymentGST_AMT As System.Windows.Forms.TextBox
    Friend WithEvents Label80 As System.Windows.Forms.Label
    Friend WithEvents mTimer As System.Windows.Forms.Timer
    Friend WithEvents PrivilegeDiscountToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PruchaseOrderRate As ToolStripMenuItem
    Friend WithEvents KFC As ToolStripMenuItem
    Friend WithEvents lblKFC As Label
    Friend WithEvents ToolStripRowFinalAmt As ToolStripMenuItem
    Friend WithEvents SecondSalesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TrfNoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents txtDetOffWt As TextBox
    Friend WithEvents lblHallmarkNo As Label
    Friend WithEvents ToolStripHallmarkDetails As ToolStripMenuItem
    Friend WithEvents Label99 As Label
    Friend WithEvents TStripShipingAddress As ToolStripMenuItem
    Friend WithEvents lblTcsCalcAmt As Label
    Friend WithEvents lblTcsCalcPrecent As Label
    Friend WithEvents lblPNo As Label
    Friend WithEvents lblSNo As Label
End Class
