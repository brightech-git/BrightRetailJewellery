Imports System.Data.OleDb
Public Class MaterialIssRec
    Implements ICloneable
    Public Enum Material
        Issue = 0
        Receipt = 1
    End Enum
    Dim StrSql As String
    Dim Cmd As OleDbCommand
    Dim Da As OleDbDataAdapter
    Private oMaterial As Material
    Private oTransactionType As String
    Private oTranDate As Date
    Public oBoardRate As Decimal
    Public objStone As New frmStoneDiaAc
    Public objMIRTran As New MaterialIssRecTran
    Dim objTds As New frmAccEd
    Dim defProcVAlue As String = Nothing
    Public objOrderInfo As MATERIALISSREC_ORDERINFO = Nothing
    Public objMisc As New frmMiscDia
    Private oStud As Boolean = False
    Private oRateLock As Boolean = True
    Public oEditRowIndex As Integer = -1
    Private ROUNDOFF_ACC As String = GetAdmindbSoftValue("ROUNDOFF-ACC", "N").ToUpper
    Private ROUNDOFF_ACC_TDS As String = GetAdmindbSoftValue("ROUNDOFF-ACC-TDS", "N").ToUpper
    Private LOCKLESSWT_ACC As Boolean = IIf(GetAdmindbSoftValue("LOCKLESSWT-ACC", "Y").ToUpper = "Y", True, False)
    Private D_VA_DATAENABLE As Boolean = IIf(GetAdmindbSoftValue("D_VA_DATAENABLE", "Y").ToUpper = "Y", True, False)
    Private ALLOY_DETAILS As Boolean = IIf(GetAdmindbSoftValue("ALLOY_DETAILS", "N").ToUpper = "Y", True, False)
    Private ROUNDEDWT As Integer = Val(GetAdmindbSoftValue("ROUNDOFF-WT", "0"))
    Private ROUNDOFF_WT() As String = GetAdmindbSoftValue("ROUNDOFF-MRMI", "0,0,0,0").Split(",")
    Private ROUNDOFF_PWT As String = GetAdmindbSoftValue("ROUNDOFF-PUREWT", "")
    Private GRSNETCAL As String = GetAdmindbSoftValue("ACC_GROSSNET", "B")
    Private MCPERGMPC As String = ""
    Private WASTPERPC As String = ""
    Private DVAACCESS As String = GetAdmindbSoftValue("ACC_D_VA_ACCESS", "N").ToUpper
    Private DEFPROCESSIDS As String = GetAdmindbSoftValue("ACC_DEF_PROCESSID", "")
    Private ED_COMPONENTS As String = GetAdmindbSoftValue("ED_COMPONENTS", "")
    Private TOUCHPERVALID As String = GetAdmindbSoftValue("TOUCHPERVALID", "N").ToUpper
    Private PURAMTEDITACC As String = GetAdmindbSoftValue("PURAMTEDIT-ACC", "N").ToUpper
    Private PURWTPERACC As String = GetAdmindbSoftValue("PURWTPER-ACC", "100")
    Private PURVATEDITACC As Boolean = IIf(GetAdmindbSoftValue("PURVATEDIT-ACC", "N").ToUpper = "Y", True, False)
    Private LOCKWOPCRATE As Boolean = IIf(GetAdmindbSoftValue("LOCKWOPCRATE", "N").ToUpper = "Y", True, False)
    Private ACC_STONEISSCAT As Boolean = IIf(GetAdmindbSoftValue("ACC_STONEISSCAT", "N").ToUpper = "Y", True, False)
    Private ACC_STUDDEDUCT_OPTIONAL As Boolean = IIf(GetAdmindbSoftValue("ACC_STUDDEDUCT_OPTIONAL", "N").ToUpper = "Y", True, False)
    Private RFID_Enable As Boolean = IIf(GetAdmindbSoftValue("RFID_ENABLE", "N").ToUpper = "Y", True, False)
    Private STONEISSCAT_EDIT As Boolean = False
    Private ACC_STUDDEDUCT_OPTIONAL_EDIT As Boolean = False
    Private AlloyRnd = Val(GetAdmindbSoftValue("ROUNDOFF-ALLOY", 3))
    Private RateRnd = Val(GetAdmindbSoftValue("ROUNDOFF-RATEACC", 2))
    Public Remark1 As String = ""
    Public Remark2 As String = ""
    Public BillCostId As String
    Dim TdsPer As Decimal = Nothing
    Dim _Tdsaccode As String = "" ''tdscheck
    Private Maccode As String
    Private MCostId As String = ""
    Public _mimr_i_r As String = ""
    Public MaterialStoneDia As New MaterialStoneDia
    Public ObjAlloy As New frmAlloyDetails
    Public JobNo As Integer
    Dim _JobNoEnable As Boolean = IIf(GetAdmindbSoftValue("MRMIJOBNO", "N") = "Y", True, False)
    Public dtGridStuddedStone As New DataTable
    Dim MultiStone As Boolean = False
    Public ORSNO As String = ""
    Public ORIRSNO As String = ""
    Public OGrswt As Decimal
    Public ONetwt As Decimal
    Dim objMultiSelect As MultiSelectRowDia = Nothing
    Public StnSno As String = ""
    Dim DdtItemDet As New DataTable
    Public Oordno, Mordno, sordno As String
    Public OProcessname, MProcessname As String
    Public Otdsname, Mtdsname, stdsname As String
    Public OLotNo, MLotNo, SLotNo As String
    Public OMetalname, MmetalName As String
    Public OCategoryname, Mcategoryname, OIssCatName, MIsscatName, OAcpostCatname, MAcPostcatname As String
    Public Opurityper, Mpurityper As Decimal
    Public SelectedOrder As String = Nothing
    Dim STOCKVALIDATION As Boolean = IIf(GetAdmindbSoftValue("MRMISTOCKLOCK", "N") = "Y", True, False)
    Dim STOCKVALIDATION_MR As Boolean = IIf(GetAdmindbSoftValue("MIMRSTOCKLOCK", "N") = "Y", True, False)
    Dim RateCalcAmt As Boolean = IIf(GetAdmindbSoftValue("MIMR_RATECALCBYAMT", "N") = "Y", True, False)
    Public RESNO As String = ""
    Dim StkPcs As Integer = 0
    Dim StkGrsWt As Double = 0
    Dim StkUnit As String = ""
    Dim ExFlag As Boolean = False
    Dim InsSno As String = ""
    Public ACCODE As String = ""
    Dim MdtOdDet As New DataTable
    Dim ObjDiaDetails As New frmDiamondDetails
    Dim MIMR_allowClosedOrder As Boolean = IIf(GetAdmindbSoftValue("MIMR_ALLOWCLOSEDORDER", "N") = "Y", True, False)
    Dim _FourCMaintain As Boolean = IIf(GetAdmindbSoftValue("4CMAINTAIN", "Y") = "Y", True, False)
    Dim PURTDSEDITACC As Boolean = IIf(GetAdmindbSoftValue("PURTDSEDIT-ACC", "N").ToUpper = "Y", True, False)
    Dim Lock_Ctrl As String = GetAdmindbSoftValue("MRMI_LOCK_CONTROLS", "").ToUpper
    Dim strLockCtrl() As String
    Dim MRMI_PreTouch As Boolean = IIf(GetAdmindbSoftValue("MRMI_PRETOUCH", "Y") = "Y", True, False)
    Dim MIMR_BAGISSUE As Boolean = IIf(GetAdmindbSoftValue("MIMR_BAGISSUE", "N") = "Y", True, False)
    Public Exdutyper As Decimal
    Dim MRMI_ALLOWZERONETWT As Boolean = IIf(GetAdmindbSoftValue("MRMI_ALLOWZERONETWT", "N") = "Y", True, False)
    Dim MR_ALLOWZEROPCS As Boolean = IIf(GetAdmindbSoftValue("MR_ALLOWZEROPCS", "Y") = "Y", True, False)
    Dim MRMI_LOCK_ITEMSUBITEM As Boolean = IIf(GetAdmindbSoftValue("MRMI_LOCK_ITEMSUBITEM", "N") = "Y", True, False)
    Dim MRMI_CATBAL_DISPLAY As Boolean = IIf(GetAdmindbSoftValue("MRMI_CATBAL_DISPLAY", "Y") = "Y", True, False)
    Dim CALON_METISSREC As Boolean = IIf(GetAdmindbSoftValue("CALC_MODE_METRECISS", "Y") = "Y", True, False)
    Dim MRMI_JOBNOLINK As Boolean = IIf(GetAdmindbSoftValue("MRMI-JOBNOLINK", "N") = "Y", True, False)
    Public editflag As Boolean = False
    Public EditBatchno As String = Nothing
    Public ListTagNos As New List(Of String)
    Public arrTagNos(1000, 1) As String
    Dim OProcessNo As String = ""
    Dim PTaxPer As Decimal
    Dim _accStateId As Integer
    Public McBill As Boolean = False
    Public SrVtTax As Decimal
    Public GstFlag As Boolean = False
    Public TdsomsFlag As Boolean = False
    Dim __TdsomsFlag As Boolean

    Dim MI_STOCKCHECK As Boolean = IIf(GetAdmindbSoftValue("MI_STOCKCHECK", "N") = "Y", True, False)
    Dim RateEnable As Boolean = IIf(GetAdmindbSoftValue("MR_RATEENABLE", "N") = "Y", True, False)
    Dim GST_CALCULATION As Boolean = IIf(GetAdmindbSoftValue("GST", "N") = "Y", True, False)
    Dim ORDER_MULTI_MIMR As Boolean = IIf(GetAdmindbSoftValue("ORDER_MULTI_MIMR", "N") = "Y", True, False)
    Public INTERNALTRANSFER_NONTAXABLE As Boolean = IIf(GetAdmindbSoftValue("INTERNALTRANSFER_NONTAXABLE", "N") = "Y", True, False)
    Public objAddtionalDetails As New frmOrAdditionalDetails
    Dim ORADDITIONALDETAIL As Boolean = IIf(GetAdmindbSoftValue("ORADDITIONALDETAIL", "N") = "Y", True, False)
    Dim ColorId As String = "0"
    Dim CutId As String = "0"
    Dim ClarityId As String = "0"
    Dim ShapeId As String = "0"
    Dim SetTypeId As String = "0"
    Dim StnHeight As String = "0"
    Dim StnWidth As String = "0"
    Dim MIMR_RATEFIXED As Boolean = IIf(GetAdmindbSoftValue("MIMR_RATEFIXED", "N") = "Y", True, False)
    Dim Lotautopost As Boolean = IIf(GetAdmindbSoftValue("ACC_LOTAUTOPOST", "N") = "Y", True, False)
    Dim MR_INCWASTINPURECAlC As Boolean = IIf(GetAdmindbSoftValue("MR_PUREWTCALC_INC_WASTAGE", "Y") = "Y", True, False)
    Dim MetalBasedStone As Boolean = IIf(GetAdmindbSoftValue("MULTIMETALBASEDSTONE", "N") = "Y", True, False)
    Dim NeedItemType_accpost As Boolean = IIf(GetAdmindbSoftValue("POS_SEPACCPOST_ITEMTYPE", "N") = "Y", True, False)


    Public Sub New(ByVal oMaterial As Material, ByVal oTransactionType As String, ByVal oTranDate As Date, ByVal Accode As String, Optional ByVal SelectedSno As String = "", Optional ByVal CostId As String = "", Optional ByVal OrdNo As String = "", Optional ByVal Stax As Decimal = 0, Optional ByVal _Gst As Boolean = True, Optional ByVal _Tdsomsflag As Boolean = True, Optional ByVal _Tdsname As String = "")
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        objGPack.TextClear(Me)
        Me.GstFlag = _Gst
        If GST_CALCULATION = False Then Me.GstFlag = False
        Me.oMaterial = oMaterial
        Me.oTransactionType = oTransactionType
        Me.oTranDate = oTranDate
        Me.SrVtTax = Stax
        Me.__TdsomsFlag = _Tdsomsflag
        Me.Otdsname = _Tdsname

        If oMaterial = Material.Issue Then
            cmbOIssuedCategory.Enabled = False
            cmbMIssuedCategory.Enabled = False
            cmbSIssuedCategory.Enabled = False
        End If
        If UCase(oTransactionType) = "PURCHASE[APPROVAL]" Then SelectedOrder = SelectedSno
        Maccode = Accode
        MCostId = CostId
        objStone.FromFlag = "A"
        Me.OProcessNo = OrdNo
        If DVAACCESS = "Y" Then
            StrSql = " SELECT I.CATCODE,D.ITEMID,I.ITEMNAME,D.SUBITEMID,ISNULL(S.SUBITEMNAME,'')SUBITEMNAME,ACCODE FROM " & cnAdminDb & "..DEALER_WMCTABLE D"
            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON D.ITEMID =I.ITEMID"
            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST S ON D.ITEMID =S.ITEMID AND D.SUBITEMID=S.SUBITEMID  "
            StrSql += vbCrLf + " WHERE ACCODE='" & Accode & "'"
            DdtItemDet = New DataTable
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(DdtItemDet)
        End If
        StrSql = "SELECT PTAX FROM " & cnAdminDb & "..TAXMAST WHERE TAXCODE='GT'"
        PTaxPer = Val(objGPack.GetSqlValue(StrSql, "PTAX", 3).ToString)
        StrSql = "SELECT STATEID FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & Maccode & "'"
        _accStateId = Val(objGPack.GetSqlValue(StrSql, "STATEID", ).ToString)
        If _accStateId = 0 Then _accStateId = CompanyStateId

        ''StuddedStone
        With dtGridStuddedStone.Columns
            .Add("ORDNO", GetType(String))
            .Add("METAL", GetType(String))
            .Add("ITEM", GetType(String))
            .Add("SUBITEM", GetType(String))
            .Add("CATEGORY", GetType(String))
            .Add("ISSCATEGORY", GetType(String))
            .Add("ACCATEGORY", GetType(String))
            .Add("PURITY", GetType(Decimal))
            .Add("PCS", GetType(Int32))
            .Add("WEIGHT", GetType(Decimal))
            .Add("GRSWT", GetType(Decimal))
            .Add("UNIT", GetType(String))
            .Add("CALC", GetType(String))
            .Add("RATE", GetType(Double))
            .Add("BOARDRATE", GetType(Decimal))
            .Add("GROSSAMT", GetType(Decimal))
            .Add("VATPER", GetType(Decimal))
            .Add("VAT", GetType(Decimal))
            .Add("AMOUNT", GetType(Double))
            .Add("REMARK1", GetType(String))
            .Add("REMARK2", GetType(String))
            .Add("ORDSTATE_NAME", GetType(String))
            .Add("ADDCHARGE", GetType(Decimal))
            .Add("STNSNO", GetType(String))
            .Add("SEIVE", GetType(String))
            .Add("RESNO", GetType(String))
            .Add("ACCODE", GetType(String))
            .Add("RFID", GetType(String))
            .Add("CUTID", GetType(String))
            .Add("COLORID", GetType(String))
            .Add("CLARITYID", GetType(String))
            .Add("SHAPEID", GetType(String))
            .Add("SETTYPEID", GetType(String))
            .Add("HEIGHT", GetType(String))
            .Add("WIDTH", GetType(String))
            .Add("TAGNO", GetType(String))
            .Add("SGSTPER", GetType(Decimal))
            .Add("CGSTPER", GetType(Decimal))
            .Add("IGSTPER", GetType(Decimal))
            .Add("SGST", GetType(Decimal))
            .Add("CGST", GetType(Decimal))
            .Add("IGST", GetType(Decimal))
            .Add("STNGRPID", GetType(Integer))
            .Add("TCS", GetType(Decimal))
            .Add("WASTAGE", GetType(Decimal))
        End With
        MdtOdDet = New DataTable
        With MdtOdDet.Columns
            .Add("ORNO", GetType(String))
            .Add("ORSNO", GetType(String))
            .Add("PCS", GetType(Int32))
            .Add("GRSWT", GetType(Decimal))
            .Add("NETWT", GetType(Decimal))
            .Add("WASTAGE", GetType(Decimal))
            .Add("MC", GetType(Decimal))
        End With

        'Dim mDr As DataRow
        'mDr = MdtOdDet.NewRow
        'mDr("ORNO") = "TOTAL"
        'mDr("PCS") = "0"
        'mDr("GRSWT") = "0.000"
        'mDr("NETWT") = "0.000"
        'mDr("WASTAGE") = "0.000"
        'mDr("MC") = "0.000"
        'MdtOdDet.Rows.Add(mDr)
        'MdtOdDet.AcceptChanges()

        With dgvOrder
            .DataSource = Nothing
            MdtOdDet.Rows.Clear()
            .DataSource = MdtOdDet
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            .ScrollBars = ScrollBars.Vertical
            .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WASTAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("MC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("WASTAGE").Visible = False
            .Columns("MC").Visible = False
        End With
        grpOrder.Visible = False

        With GridStuddStone
            .DataSource = dtGridStuddedStone
            For i As Integer = 0 To .ColumnCount - 1
                If .Columns(i).ValueType.Name Is GetType(Decimal).Name Then
                    .Columns(i).DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf .Columns(i).ValueType.Name Is GetType(Double).Name Then
                    .Columns(i).DefaultCellStyle.Format = "0.00"
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf .Columns(i).ValueType.Name Is GetType(Integer).Name Then
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End If
                .Columns(i).Resizable = DataGridViewTriState.False
            Next
            .Columns("WEIGHT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
        End With
        StyleGridStone(GridStuddStone)
        TdsPer = Val(objGPack.GetSqlValue("SELECT TDSPER FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & Accode & "' AND ISNULL(TDSFLAG,'') = 'Y'"))
        ' And UCase(oTransactionType) <> "PURCHASE" And UCase(oTransactionType) <> "PURCHASE[APPROVAL]" Newly Comment on 29-June-2021
        If UCase(oTransactionType) <> "PURCHASE RETURN" And UCase(oTransactionType) <> "INTERNAL TRANSFER" And UCase(oTransactionType) <> "APPROVAL RECEIPT" Then
            lblOEd.Visible = False
            txtOED_AMT.Visible = False
            Label73.Visible = False
            txtOedPer_AMT.Visible = False
            lblOVatPer.Text = "Tds%"
            lblOVat.Text = "Tds"
            lblMVatPer.Text = "Tds%"
            lblMVat.Text = "Tds"
            lblSVatPer.Text = "Tds%"
            lblSVat.Text = "Tds"
            lblOthVatPer.Text = "Tds%"
            lblOthVat.Text = "Tds"
            txtOVatPer_PER.Text = IIf(TdsPer <> 0, Format(TdsPer, "0.00"), "")
            txtMVatPer_PER.Text = IIf(TdsPer <> 0, Format(TdsPer, "0.00"), "")
            txtSVatPer_PER.Text = IIf(TdsPer <> 0, Format(TdsPer, "0.00"), "")
            txtOthVatPer_PER.Text = IIf(TdsPer <> 0, Format(TdsPer, "0.00"), "")

            If RateCalcAmt = True And (UCase(oTransactionType) = "PURCHASE" Or UCase(oTransactionType) = "ISSUE") Then
                oRateLock = False
            ElseIf UCase(oTransactionType) = "PURCHASE" Then
                oRateLock = False
            ElseIf UCase(oTransactionType) = "PURCHASE[APPROVAL]" Then
                oRateLock = False
            Else
                oRateLock = False
            End If

            If PURTDSEDITACC Then
                txtOVatPer_PER.ReadOnly = False
                txtMVatPer_PER.ReadOnly = False
                txtSVatPer_PER.ReadOnly = False
                txtOthVatPer_PER.ReadOnly = False
                txtOVat_AMT.ReadOnly = False
                txtMVat_AMT.ReadOnly = False
                txtSVat_AMT.ReadOnly = False
                txtOthVat_AMT.ReadOnly = False
            Else
                txtOVatPer_PER.ReadOnly = True
                txtMVatPer_PER.ReadOnly = True
                txtSVatPer_PER.ReadOnly = True
                txtOthVatPer_PER.ReadOnly = True
                txtOVat_AMT.ReadOnly = True
                txtMVat_AMT.ReadOnly = True
                txtSVat_AMT.ReadOnly = True
                txtOthVat_AMT.ReadOnly = True
            End If
            'PnlGst.Enabled = False
            'PnlMGst.Enabled = False
            'PnlSGst.Enabled = False
            'PnlOthGst.Enabled = False
        Else
            lblOVatPer.Text = "Vat%"
            lblOVat.Text = "Vat"
            lblMVatPer.Text = "Vat%"
            lblMVat.Text = "Vat"
            lblSVatPer.Text = "Vat%"
            lblSVat.Text = "Vat"
            lblOthVatPer.Text = "Vat%"
            lblOthVat.Text = "Vat"
            oRateLock = False
            If GstFlag Then
                txtOVatPer_PER.ReadOnly = True
                txtOVat_AMT.ReadOnly = True
                txtMVatPer_PER.ReadOnly = True
                txtMVat_AMT.ReadOnly = True
                txtSVatPer_PER.ReadOnly = True
                txtSVat_AMT.ReadOnly = True
                txtOthVatPer_PER.ReadOnly = True
                txtOthVat_AMT.ReadOnly = True
                lblOVatPer.Text = "GST%"
                lblOVat.Text = "GST"
                lblMVatPer.Text = "GST%"
                lblMVat.Text = "GST"
                lblSVatPer.Text = "GST%"
                lblSVat.Text = "GST"
                lblOthVatPer.Text = "GST%"
                lblOthVat.Text = "GST"
                txtOVatPer_PER.Text = IIf(PTaxPer <> 0, Format(PTaxPer, "0.00"), "")
                txtMVatPer_PER.Text = IIf(PTaxPer <> 0, Format(PTaxPer, "0.00"), "")
                txtSVatPer_PER.Text = IIf(PTaxPer <> 0, Format(PTaxPer, "0.00"), "")
                txtOthVatPer_PER.Text = IIf(PTaxPer <> 0, Format(PTaxPer, "0.00"), "")
            Else
                PnlGst.Enabled = False
                PnlMGst.Enabled = False
                PnlSGst.Enabled = False
                PnlOthGst.Enabled = False
            End If
        End If
        If UCase(oTransactionType) = "PURCHASE" Or UCase(oTransactionType) = "PURCHASE APPROVAL" Then
            lblOEd.Visible = True
            txtOED_AMT.Visible = True
            Label73.Visible = True
            txtOedPer_AMT.Visible = True
        End If
        If UCase(oTransactionType) = "RECEIPT" And RateEnable Then
            oRateLock = False
            txtOTCS_AMT.ReadOnly = False
            txtMTCS_AMT.ReadOnly = False
            txtSTCS_AMT.ReadOnly = False
        End If
        If UCase(oTransactionType) = "PURCHASE RETURN" Or
            UCase(oTransactionType) = "PURCHASE" Then
            txtOTCS_AMT.ReadOnly = False
            txtMTCS_AMT.ReadOnly = False
            txtSTCS_AMT.ReadOnly = False
        End If
        cmbOCategory.Enabled = False
        cmbOItem.Enabled = False
        cmbOSubItem.Enabled = False

        If _JobNoEnable = True Then
            lblMordNo.Text = "JobNo"
            lblOOrdno.Text = "JobNo"
            lblSOrdNo.Text = "JobNo"
        End If

        If STOCKVALIDATION = True Or STOCKVALIDATION_MR = True Then
            lblMordNo.Text = "Iss/Rec No"
            lblOOrdno.Text = "Iss/Rec No"
            lblSOrdNo.Text = "Iss/Rec No"
        End If

        ''Metal
        LoadMetal(cmbOMetal)
        LoadMetal(cmbMMetal)
        LoadMetal(cmbSMetal)
        LoadMetal(cmbOthMetal)

        ''Process
        LoadProcess(cmbOProcess)
        LoadProcess(cmbMProcess)
        LoadProcess(cmbSProcess)

        'PURITY
        LoadPurity(CmbOPurity)
        LoadPurity(CmbMPurity)
        LoadPurity(CmbSPurity)
        ''GrsNet
        cmbOGrsNet.DropDownStyle = ComboBoxStyle.DropDownList
        cmbOGrsNet.Items.Add("GRS WT")
        cmbOGrsNet.Items.Add("NET WT")
        cmbOGrsNet.Text = "GRS WT"
        'GrsNet
        cmbMGrsNet.DropDownStyle = ComboBoxStyle.DropDownList
        cmbMGrsNet.Items.Add("GRS WT")
        cmbMGrsNet.Items.Add("NET WT")
        cmbMGrsNet.Text = "GRS WT"
        ''GrsNet
        cmbSUnit.DropDownStyle = ComboBoxStyle.DropDownList
        cmbSUnit.Items.Add("CARAT")
        cmbSUnit.Items.Add("GRAM")
        cmbSUnit.Text = "CARAT"
        ''CalcMode
        cmbSCalcMode.DropDownStyle = ComboBoxStyle.DropDownList
        cmbSCalcMode.Items.Add("PIECE")
        cmbSCalcMode.Items.Add("WEIGHT")
        cmbSCalcMode.Text = "WEIGHT"
        ''Seive
        LoadSeiveSize(cmbSSeive)
        grpContainer.Controls.Add(grpMetal)
        grpContainer.Controls.Add(grpOrnament)
        grpContainer.Controls.Add(grpStone)
        grpContainer.Controls.Add(grpOthers)
        tabMain.Visible = False
        grpOrnament.Location = New Point(grpMain.Location.X, grpMain.Location.Y + grpMain.Size.Height)
        grpMetal.Location = grpOrnament.Location
        grpStone.Location = grpOrnament.Location
        grpOthers.Location = grpOrnament.Location
        STONEISSCAT_EDIT = False
        ACC_STUDDEDUCT_OPTIONAL_EDIT = False
        MultiStone = False
        ORSNO = ""
        ORIRSNO = ""
        RESNO = ""
        OGrswt = 0
        ONetwt = 0

        ''Calc Mode
        cmbOcalcon.DropDownStyle = ComboBoxStyle.DropDownList
        cmbOcalcon.Items.Add("GRS WT")
        cmbOcalcon.Items.Add("NET WT")
        cmbOcalcon.Items.Add("PURE WT")
        cmbOcalcon.Text = "PURE WT"
        cmbOcalcon.Visible = False
        Label72.Visible = False
        If CALON_METISSREC = True Then
            If UCase(oTransactionType) = "PURCHASE" Then
                cmbOcalcon.Visible = True
                Label72.Visible = True
            Else
                cmbOcalcon.Visible = False
                Label72.Visible = False
            End If
        End If
        If oTransactionType = "RECEIPT" Then
            cmbOStkType.Text = "Manufacturing"
            CmbMStktype.Text = "Manufacturing"
        ElseIf oTransactionType = "PURCHASE" Then
            cmbOStkType.Text = "Trading"
            CmbMStktype.Text = "Trading"
        End If

        CmbOHallmark.Items.Clear()
        CmbOHallmark.Items.Add("YES")
        CmbOHallmark.Items.Add("NO")
        CmbOHallmark.Text = "NO"

        If Lotautopost And oMaterial = Material.Receipt _
            And oTransactionType <> "PURCHASE[APPROVAL]" _
            And oTransactionType <> "APPROVAL RECEIPT" Then
            lblOhallmark.Visible = True
            CmbOHallmark.Visible = True
        Else
            lblOhallmark.Visible = False
            CmbOHallmark.Visible = False
        End If
        '1404
        ''TdsomsFlag = __TdsomsFlag
        __TdsomsFlag = TdsomsFlag
        If TdsomsFlag = False Then
            StrSql = "SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY  ORDER BY DISPLAYORDER "
            objGPack.FillCombo(StrSql, Cmboacname, True, False)
            objGPack.FillCombo(StrSql, Cmbmacname, True, False)
            objGPack.FillCombo(StrSql, Cmbsacname, True, False)
            Dim TDSCATID As Integer = Val(objGPack.GetSqlValue("SELECT TDSCATID FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & Maccode & "' AND ISNULL(TDSFLAG,'') = 'Y'", , 0))
            If TDSCATID <> 0 Then
                Dim tds_acname As String = objGPack.GetSqlValue("SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATID = '" & TDSCATID & "'", , "")
                Dim tds_accode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATID = '" & TDSCATID & "'", , "")
                Cmboacname.Text = tds_acname
                txtotdsaccode.Text = tds_accode
                Cmbmacname.Text = tds_acname
                txtmtdsaccode.Text = tds_accode
                Cmbsacname.Text = tds_acname
                txtstdsaccode.Text = tds_accode
            End If
            Otdsname = Nothing
            Mtdsname = Nothing
            stdsname = Nothing
        Else
            If Otdsname <> Nothing Then
                StrSql = "SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY where TDSCATNAME='" & Otdsname.ToString & "'  ORDER BY DISPLAYORDER "
            ElseIf Mtdsname <> Nothing Then
                StrSql = "SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY where TDSCATNAME='" & Mtdsname.ToString & "'  ORDER BY DISPLAYORDER "
            ElseIf stdsname <> Nothing Then
                StrSql = "SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY where TDSCATNAME='" & stdsname.ToString & "'  ORDER BY DISPLAYORDER "
            Else
                StrSql = "SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY  ORDER BY DISPLAYORDER "
            End If
            objGPack.FillCombo(StrSql, Cmboacname, True, False)
            objGPack.FillCombo(StrSql, Cmbmacname, True, False)
            objGPack.FillCombo(StrSql, Cmbsacname, True, False)
            If Otdsname Is Nothing And Mtdsname Is Nothing And stdsname Is Nothing Then
                Dim TDSCATID As Integer = Val(objGPack.GetSqlValue("SELECT TDSCATID FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & Maccode & "' AND ISNULL(TDSFLAG,'') = 'Y'",, 0))
                If TDSCATID <> 0 Then
                    Dim tds_acname As String = objGPack.GetSqlValue("SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATNAME = '" & Otdsname.ToString & "'",, "")
                    Dim tds_accode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATNAME = '" & Otdsname.ToString & "'",, "")
                    Cmboacname.Text = tds_acname
                    txtotdsaccode.Text = tds_accode
                    Cmbmacname.Text = tds_acname
                    txtmtdsaccode.Text = tds_accode
                    Cmbsacname.Text = tds_acname
                    txtstdsaccode.Text = tds_accode
                Else
                    Otdsname = Nothing
                    Mtdsname = Nothing
                    stdsname = Nothing
                End If
            Else
                If Otdsname <> Nothing Then
                    StrSql = "SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY where TDSCATNAME='" & Otdsname.ToString & "' "
                ElseIf Mtdsname <> Nothing Then
                    StrSql = "SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY where TDSCATNAME='" & Mtdsname.ToString & "' "
                ElseIf stdsname <> Nothing Then
                    StrSql = "SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY where TDSCATNAME='" & stdsname.ToString & "' "
                End If
                Dim tds_accode As String = objGPack.GetSqlValue(StrSql,, "")
                Cmboacname.Text = Otdsname
                txtotdsaccode.Text = tds_accode
                Cmbmacname.Text = Mtdsname
                txtmtdsaccode.Text = tds_accode
                Cmbsacname.Text = stdsname
                txtstdsaccode.Text = tds_accode
            End If
        End If
        ArrayClear()
    End Sub
    Public Sub ArrayClear()
        For i As Integer = 0 To 1000
            arrTagNos(i, 0) = ""
            arrTagNos(i, 1) = ""
        Next
    End Sub
    Public Sub StyleGridStone(ByVal gridStone As DataGridView)
        With gridStone
            .Columns("ORDNO").Width = 60
            .Columns("ITEM").Width = 100
            .Columns("SUBITEM").Width = 100
            .Columns("CATEGORY").Width = 70
            .Columns("PURITY").Width = 50
            .Columns("PCS").Width = 50
            .Columns("WEIGHT").Width = 70
            .Columns("GRSWT").Width = 70
            .Columns("UNIT").Width = 50
            .Columns("CALC").Width = 50
            .Columns("RATE").Width = 70
            .Columns("GROSSAMT").Width = 80
            .Columns("VAT").Width = 70
            .Columns("AMOUNT").Width = 80
            .Columns("SEIVE").Width = 60

            .Columns("METAL").Visible = False
            .Columns("ISSCATEGORY").Visible = False
            .Columns("ACCATEGORY").Visible = False
            .Columns("BOARDRATE").Visible = False
            .Columns("VATPER").Visible = False
            .Columns("REMARK1").Visible = False
            .Columns("REMARK2").Visible = False
            .Columns("ORDSTATE_NAME").Visible = False
            .Columns("ADDCHARGE").Visible = False
            .Columns("STNSNO").Visible = False
            .Columns("RESNO").Visible = False
            .Columns("ACCODE").Visible = False
            .Columns("SGSTPER").Visible = False
            .Columns("CGSTPER").Visible = False
            .Columns("IGSTPER").Visible = False
            .Columns("SGST").Visible = False
            .Columns("CGST").Visible = False
            .Columns("IGST").Visible = False
            '.Columns("TAGNO").Visible = False
        End With
    End Sub


    Private Function CheckJobNo(ByVal txtJobNo As TextBox) As Boolean
        If cmbOProcess.Text = "LOT" Then 'lottt
            If oMaterial = Material.Issue Then
                StrSql = "SELECT JOBNO FROM " & cnStockDb & "..ISSUE "
                StrSql += " WHERE JOBNO = '" & txtJobNo.Text & "'  "
                StrSql += " AND ISNULL(CANCEL,'') = '' "
            Else
                StrSql = "SELECT JOBNO FROM " & cnStockDb & "..RECEIPT "
                StrSql += " WHERE JOBNO = '" & txtJobNo.Text & "'  "
                StrSql += " AND ISNULL(CANCEL,'') = '' "
            End If
            If editflag = False Then
                If __TdsomsFlag = False Then
                    If objGPack.GetSqlValue(StrSql) <> "" Then
                        MsgBox("Lot No Already Exit", MsgBoxStyle.Information)
                        txtOOrdNo.Focus()
                        txtOOrdNo.SelectAll()
                        Return False
                    Else
                        Return True
                    End If
                Else
                    Return True
                End If

            Else
                Return True
            End If
        ElseIf cmbMProcess.Text = "LOT" Then
            If oMaterial = Material.Issue Then
                StrSql = "SELECT JOBNO FROM " & cnStockDb & "..ISSUE "
                StrSql += " WHERE JOBNO = '" & txtJobNo.Text & "'  "
                StrSql += " AND ISNULL(CANCEL,'') = '' "
            Else
                StrSql = "SELECT JOBNO FROM " & cnStockDb & "..RECEIPT "
                StrSql += " WHERE JOBNO = '" & txtJobNo.Text & "'  "
                StrSql += " AND ISNULL(CANCEL,'') = '' "
            End If
            If editflag = False Then
                If __TdsomsFlag = False Then
                    If objGPack.GetSqlValue(StrSql) <> "" Then
                        MsgBox("Lot No Already Exit", MsgBoxStyle.Information)
                        txtMOrdNo.Focus()
                        txtMOrdNo.SelectAll()
                        Return False
                    Else
                        Return True
                    End If
                Else
                    Return True
                End If
            Else
                Return True
            End If

        ElseIf cmbSProcess.Text = "LOT" Then
            If oMaterial = Material.Issue Then
                StrSql = "SELECT JOBNO FROM " & cnStockDb & "..ISSUE "
                StrSql += " WHERE JOBNO = '" & txtJobNo.Text & "'  "
                StrSql += " AND ISNULL(CANCEL,'') = '' "
            Else
                StrSql = "SELECT JOBNO FROM " & cnStockDb & "..RECEIPT "
                StrSql += " WHERE JOBNO = '" & txtJobNo.Text & "'  "
                StrSql += " AND ISNULL(CANCEL,'') = '' "
            End If
            If editflag = False Then
                If __TdsomsFlag = False Then
                    If objGPack.GetSqlValue(StrSql) <> "" Then
                        MsgBox("Lot No Already Exit", MsgBoxStyle.Information)
                        txtSOrdNo.Focus()
                        txtSOrdNo.SelectAll()
                        Return False
                    Else
                        Return True
                    End If
                Else
                    Return True
                End If
            Else
                Return True
            End If
        Else
            If txtJobNo.Text = "" Then
                Return True
            End If
            If _JobNoEnable = False Then
                StrSql = "SELECT SNO FROM " & cnAdminDb & "..ORMAST "
                StrSql += " WHERE ORNO = '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & "" & txtJobNo.Text & "'"
                'For order/repair not load
                'StrSql += " AND PROPSMITH = '" & Maccode & "'"
                If MIMR_allowClosedOrder = False Then
                    StrSql += " AND ISNULL(ODBATCHNO,'') = ''"
                End If
                StrSql += " AND ISNULL(CANCEL,'') = '' AND ISNULL(ORDCANCEL,'') = ''"
                If objGPack.GetSqlValue(StrSql) <> "" Then
                    Return True
                End If
                If STOCKVALIDATION Or STOCKVALIDATION_MR Then Return True
                If MIMR_BAGISSUE And txtJobNo.Text.Contains("B") Then Return True
                Return False
            Else
                Return True
            End If
        End If
    End Function

    Private Sub LoadSeiveSize(ByVal Cmb As ComboBox)
        StrSql = vbCrLf + " SELECT DISTINCT SIZEDESC FROM " & cnAdminDb & "..CENTSIZE ORDER BY SIZEDESC"
        objGPack.FillCombo(StrSql, Cmb, True, False)
    End Sub

    Private Sub LoadProcess(ByVal Cmb As ComboBox)
        StrSql = vbCrLf + " SELECT ORDSTATE_NAME,ORDSTATE_ID FROM " & cnAdminDb & "..ORDERSTATUS ORDER BY ORDSTATE_NAME"
        Dim dtMetal As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtMetal)
        Cmb.DataSource = dtMetal
        Cmb.DisplayMember = "ORDSTATE_NAME"
        Cmb.ValueMember = "ORDSTATE_ID"
        Cmb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        Cmb.AutoCompleteSource = AutoCompleteSource.ListItems
    End Sub

    Private Sub LoadMetal(ByVal Cmb As ComboBox)
        StrSql = " SELECT METALNAME,METALID FROM " & cnAdminDb & "..METALMAST WHERE ISNULL(ACTIVE,'') <> 'N' "
        If Cmb.Name = cmbOMetal.Name Or Cmb.Name = cmbMMetal.Name Then
            ''StrSql += " AND TTYPE='M'"
            StrSql += " AND TTYPE IN ('M','A')"
        ElseIf Cmb.Name = cmbSMetal.Name Then
            StrSql += " AND TTYPE='S'"
        End If
        StrSql += " ORDER BY DISPLAYORDER,METALNAME"
        Dim dtMetal As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtMetal)
        Cmb.DataSource = dtMetal
        Cmb.DisplayMember = "METALNAME"
        Cmb.ValueMember = "METALID"
        Cmb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        Cmb.AutoCompleteSource = AutoCompleteSource.ListItems
    End Sub
    Private Sub LoadPurity(ByVal Cmb As ComboBox)
        StrSql = "SELECT DISTINCT CONVERT(DECIMAL(12,2),PURITY) FROM " & cnAdminDb & "..PURITYMAST  "
        objGPack.FillCombo(StrSql, Cmb, True, False)
        'Dim dtMetal As New DataTable
        'Da = New OleDbDataAdapter(StrSql, cn)
        'Da.Fill(dtMetal)
        'Cmb.DataSource = dtMetal
        'Cmb.DisplayMember = "PURITY"
        'Cmb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        'Cmb.AutoCompleteSource = AutoCompleteSource.ListItems
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        ''VALIDATION
        If rbtOrnament.Checked Then
            If CheckJobNo(txtOOrdNo) = False Then Exit Sub
            If txtOOrdNo.Text <> "" Then
                If _JobNoEnable = False Then
                    If txtOOrdNo.Text.StartsWith("O") Or txtOOrdNo.Text.StartsWith("R") Then
                        If objOrderInfo.DialogResult <> Windows.Forms.DialogResult.OK Then
                            txtOOrdNo.Select()
                            MsgBox("Invalid Order Selection", MsgBoxStyle.Information)
                            Exit Sub
                        End If
                    End If
                End If
            End If
            If Val(txtOGrsWt_WET.Text) = 0 And Val(txtOPcs_NUM.Text) = 0 Then
                MsgBox("Grs. Weight should not empty", MsgBoxStyle.Information)
                txtOGrsWt_WET.Select()
                Exit Sub
            End If
            If MRMI_ALLOWZERONETWT = False And Val(txtONetWt_WET.Text) = 0 And Val(txtOPcs_NUM.Text) = 0 Then
                MsgBox("Net Weight should not empty", MsgBoxStyle.Information)
                Exit Sub
            End If
            If Val(txtOGrsWt_WET.Text) <> 0 And MR_ALLOWZEROPCS = False And Val(txtOPcs_NUM.Text) = 0 And oMaterial = Material.Receipt Then
                MsgBox("Pcs should not empty", MsgBoxStyle.Information)
                txtOPcs_NUM.Select()
                Exit Sub
            End If
            If Val(txtOSgst_AMT.Text) <> Val(txtOCgst_AMT.Text) Then
                MsgBox("SGST/CGST percent Mismatch...", MsgBoxStyle.Information)
                txtOSgst_AMT.Focus()
                Exit Sub
            End If
            If lblOVat.Text.ToUpper.ToString = "GST" Then
                If Val(txtOVat_AMT.Text) > 0 Then
                    If Val(txtOVat_AMT.Text) <> Val(txtOSG_AMT.Text) + Val(txtOCG_AMT.Text) + Val(txtOIG_AMT.Text) Then
                        MsgBox("GST Mismatch...", MsgBoxStyle.Information)
                        Exit Sub
                    ElseIf Val(txtOSG_AMT.Text) + Val(txtOCG_AMT.Text) > 0 Then
                        If Val(txtOSG_AMT.Text) <> Val(txtOCG_AMT.Text) Then
                            MsgBox("SGST/CGST Mismatch...", MsgBoxStyle.Information)
                            txtOSG_AMT.Focus()
                            Exit Sub
                        End If
                    End If
                End If
            End If
            If Val(txtOGrsAmt_AMT.Text) > 0 And Val(txtOMc_AMT.Text) > 0 Then
                If oTransactionType = "RECEIPT" And GstFlag Then
                    McBill = True
                Else
                    McBill = False
                End If
            Else
                McBill = False
            End If
            Remark1 = txtORemark1.Text
            Remark2 = txtORemark2.Text
            Oordno = txtOOrdNo.Text
            Otdsname = Cmboacname.Text

            OProcessname = cmbOProcess.Text
            OLotNo = txtOOrdNo.Text
            OMetalname = cmbOMetal.Text
            OCategoryname = cmbOCategory.Text : OIssCatName = cmbOIssuedCategory.Text : OAcpostCatname = cmbOAcPostCategory.Text
            'opurityper = txtop
        ElseIf rbtMetal.Checked Then
            If CheckJobNo(txtMOrdNo) = False Then Exit Sub
            If txtMOrdNo.Text <> "" Then
                If _JobNoEnable = False Then
                    If txtOOrdNo.Text.StartsWith("O") Or txtOOrdNo.Text.StartsWith("R") Then
                        If objOrderInfo.DialogResult <> Windows.Forms.DialogResult.OK Then
                            txtMOrdNo.Select()
                            MsgBox("Invalid Order Selection", MsgBoxStyle.Information)
                            Exit Sub
                        End If
                    End If
                End If
            End If
            If Val(txtMGrsWt_WET.Text) = 0 And Val(txtMPcs_NUM.Text) = 0 Then
                MsgBox("Grs. Weight should not empty", MsgBoxStyle.Information)
                txtMGrsWt_WET.Select()
                Exit Sub
            End If
            If Val(txtMSgst_AMT.Text) <> Val(txtMCgst_AMT.Text) Then
                MsgBox("SGST/CGST percent Mismatch...", MsgBoxStyle.Information)
                txtMSgst_AMT.Focus()
                Exit Sub
            End If
            If lblMVat.Text.ToUpper.ToString = "GST" Then
                If Val(txtMVat_AMT.Text) > 0 Then
                    If Val(txtMVat_AMT.Text) <> Val(txtMSG_AMT.Text) + Val(txtMCG_AMT.Text) + Val(txtMIG_AMT.Text) Then
                        MsgBox("GST Mismatch...", MsgBoxStyle.Information)
                        Exit Sub
                    ElseIf Val(txtMSG_AMT.Text) + Val(txtMCG_AMT.Text) > 0 Then
                        If Val(txtMSG_AMT.Text) <> Val(txtMCG_AMT.Text) Then
                            MsgBox("SGST/CGST Mismatch...", MsgBoxStyle.Information)
                            txtMSG_AMT.Focus()
                            Exit Sub
                        End If
                    End If
                End If
            End If
            Remark1 = txtMRemark1.Text
            Remark2 = txtMRemark2.Text
            Mordno = txtMOrdNo.Text
            Mtdsname = Cmbmacname.Text
            MProcessname = cmbMProcess.Text
            MmetalName = cmbMMetal.Text
            Mcategoryname = cmbMCategory.Text : MIsscatName = cmbMIssuedCategory.Text : MAcPostcatname = cmbMAcPostCategory.Text
        ElseIf rbtStone.Checked Then
            If oEditRowIndex <> -1 Then
                If txtSOrdNo.Text <> "" Then
                    If _JobNoEnable = False Then
                        If objOrderInfo.DialogResult <> Windows.Forms.DialogResult.OK Then
                            txtSOrdNo.Select()
                            MsgBox("Invalid Order Selection", MsgBoxStyle.Information)
                            Exit Sub
                        End If
                    End If
                End If
                If Val(txtSGrsWt_WET.Text) = 0 And Val(txtSPcs_NUM.Text) = 0 Then
                    MsgBox("Weight should not empty", MsgBoxStyle.Information)
                    txtSGrsWt_WET.Select()
                    Exit Sub
                End If
                Remark1 = txtSRemark1.Text
                Remark2 = txtSRemark2.Text
                stdsname = Cmbsacname.Text
                sordno = txtSOrdNo.Text
            Else
                If Not GridStuddStone.Rows.Count > 0 Then
                    MsgBox("Enter valid entry", MsgBoxStyle.Information)
                    txtSGrsWt_WET.Select()
                    Exit Sub
                End If
            End If
        End If
        ListTagNos.Add(txtTagNo.Text)
        For i As Integer = 0 To arrTagNos.Length
            If arrTagNos(i, 0).ToString = "" Then
                arrTagNos(i, 0) = txtTagNo.Text
                arrTagNos(i, 1) = ""
                If OCategoryname <> "" Then
                    arrTagNos(i, 1) = OCategoryname
                ElseIf Mcategoryname <> "" Then
                    arrTagNos(i, 1) = Mcategoryname
                End If
                Exit For
            End If
        Next
        StkPcs = 0 : StkGrsWt = 0 : StkUnit = "" : InsSno = ""
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        StkPcs = 0 : StkGrsWt = 0 : StkUnit = ""
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub MaterialIssRec_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If cmbOProcess.Focused Then Exit Sub
            If cmbMProcess.Focused Then Exit Sub
            If cmbSProcess.Focused Then Exit Sub
            If txtOGrsWt_WET.Focused Then Call calculationget() : Exit Sub
            If txtOWast_WET.Focused Then Exit Sub
            If txtMWast_WET.Focused Then Exit Sub
            If txtOOrdNo.Focused Then Exit Sub
            If txtMOrdNo.Focused Then Exit Sub
            If txtSOrdNo.Focused Then Exit Sub
            If txtOTouchAMT.Focused Then Exit Sub
            If txtMReceivePurity_AMT.Focused Then Exit Sub
            If rbtMetal.Focused Then Exit Sub
            If rbtOrnament.Focused Then Exit Sub
            If rbtStone.Focused Then Exit Sub
            If rbtOthers.Focused Then Exit Sub
            'If cmbOStkType.Focused Then Exit Sub
            'If CmbMStktype.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            btnOk.Focus()
        End If
    End Sub
    Private Sub calculationget()
        If GRSNETCAL = "G" Then cmbOGrsNet.Text = "GRS WT" : cmbOGrsNet.Enabled = D_VA_DATAENABLE
        If GRSNETCAL = "N" Then cmbOGrsNet.Text = "NET WT" : cmbOGrsNet.Enabled = D_VA_DATAENABLE
        If GRSNETCAL = "B" Then cmbOGrsNet.Enabled = True
        If GRSNETCAL = "G" Then cmbMGrsNet.Text = "GRS WT" : cmbMGrsNet.Enabled = D_VA_DATAENABLE
        If GRSNETCAL = "N" Then cmbMGrsNet.Text = "NET WT" : cmbMGrsNet.Enabled = D_VA_DATAENABLE
        If GRSNETCAL = "B" Then cmbMGrsNet.Enabled = True
        If GRSNETCAL = "B,N" Then cmbMGrsNet.Enabled = True : cmbMGrsNet.Text = "NET WT"
        If GRSNETCAL = "B,N" Then cmbOGrsNet.Enabled = True : cmbOGrsNet.Text = "NET WT"
    End Sub

    Private Sub MaterialIssRec_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strLockCtrl = Lock_Ctrl.Split(",")
        For Each cr As String In strLockCtrl
            Select Case cr.ToString
                Case "T"
                    txtOVat_AMT.Enabled = False
                    txtOVatPer_PER.Enabled = False
                    txtMVatPer_PER.Enabled = False
                    txtMVat_AMT.Enabled = False
                    txtSVat_AMT.Enabled = False
                    txtSVatPer_PER.Enabled = False
                    txtOthVatPer_PER.Enabled = False
                    txtOthVat_AMT.Enabled = False
                Case "M"
                    txtOAddlCharge_AMT.Enabled = False
                    txtMAddlCharge_AMT.Enabled = False
                    txtSAddlCharge_AMT.Enabled = False
                Case "MC"
                    txtOMcGrm_AMT.Enabled = False
                    txtMMcGrm_AMT.Enabled = False
                Case "WP"
                    txtOWastPer.Enabled = False
                    txtMWastPER.Enabled = False
                Case "WA"
                    txtOWast_WET.Enabled = False
                    txtMWast_WET.Enabled = False
            End Select
        Next
        grpMain.BackColor = Color.Lavender
        tabOrnament.BackColor = Color.Lavender
        tabMetal.BackColor = Color.Lavender
        tabStone.BackColor = Color.Lavender
        tabOthers.BackColor = Color.Lavender
        grpOrnament.BackColor = Color.Lavender
        grpMetal.BackColor = Color.Lavender
        grpStone.BackColor = Color.Lavender
        grpOthers.BackColor = Color.Lavender
        pnlEd.BackColor = Color.Lavender
        PnlGst.BackColor = Color.Lavender
        PnlMGst.BackColor = Color.Lavender
        PnlSGst.BackColor = Color.Lavender
        PnlOthGst.BackColor = Color.Lavender
        tabMain.ItemSize = New System.Drawing.Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabOrnament.Left, Me.tabOrnament.Top, Me.tabOrnament.Width, Me.tabOrnament.Height))
        If Remark1 <> "" Then txtORemark1.Text = Remark1
        If Remark2 <> "" Then txtORemark2.Text = Remark2
        If Remark1 <> "" Then txtMRemark1.Text = Remark1
        If Remark2 <> "" Then txtMRemark2.Text = Remark2
        If Remark1 <> "" Then txtSRemark1.Text = Remark1
        If Remark2 <> "" Then txtSRemark2.Text = Remark2
        If Remark1 <> "" Then txtOthRemark1.Text = Remark1
        If Remark2 <> "" Then txtOthRemark2.Text = Remark2

        ''TdsomsFlag = __TdsomsFlag
        __TdsomsFlag = TdsomsFlag
        If TdsomsFlag = False Then
            StrSql = "SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY  ORDER BY DISPLAYORDER "
            objGPack.FillCombo(StrSql, Cmboacname, True, False)
            objGPack.FillCombo(StrSql, Cmbmacname, True, False)
            objGPack.FillCombo(StrSql, Cmbsacname, True, False)
            Dim TDSCATID As Integer = Val(objGPack.GetSqlValue("SELECT TDSCATID FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & Maccode & "' AND ISNULL(TDSFLAG,'') = 'Y'",, 0))
            If TDSCATID <> 0 Then
                Dim tds_acname As String = objGPack.GetSqlValue("SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATID = '" & TDSCATID & "'",, "")
                Dim tds_accode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATID = '" & TDSCATID & "'",, "")
                Cmboacname.Text = tds_acname
                txtotdsaccode.Text = tds_accode
                Cmbmacname.Text = tds_acname
                txtmtdsaccode.Text = tds_accode
                Cmbsacname.Text = tds_acname
                txtstdsaccode.Text = tds_accode
            End If
            Otdsname = Nothing
            Mtdsname = Nothing
            stdsname = Nothing
        Else
            If Otdsname <> Nothing Then
                StrSql = "SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY where TDSCATNAME='" & Otdsname.ToString & "'  ORDER BY DISPLAYORDER "
            ElseIf Mtdsname <> Nothing Then
                StrSql = "SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY where TDSCATNAME='" & Mtdsname.ToString & "'  ORDER BY DISPLAYORDER "
            ElseIf stdsname <> Nothing Then
                StrSql = "SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY where TDSCATNAME='" & stdsname.ToString & "'  ORDER BY DISPLAYORDER "
            Else
                StrSql = "SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY  ORDER BY DISPLAYORDER "
            End If
            objGPack.FillCombo(StrSql, Cmboacname, True, False)
            objGPack.FillCombo(StrSql, Cmbmacname, True, False)
            objGPack.FillCombo(StrSql, Cmbsacname, True, False)
            If Otdsname Is Nothing And Mtdsname Is Nothing And stdsname Is Nothing Then
                Dim TDSCATID As Integer = Val(objGPack.GetSqlValue("SELECT TDSCATID FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & Maccode & "' AND ISNULL(TDSFLAG,'') = 'Y'",, 0))
                If TDSCATID <> 0 Then
                    Dim tds_acname As String = objGPack.GetSqlValue("SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATNAME = '" & Otdsname.ToString & "'",, "")
                    Dim tds_accode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATNAME = '" & Otdsname.ToString & "'",, "")
                    Cmboacname.Text = tds_acname
                    txtotdsaccode.Text = tds_accode
                    Cmbmacname.Text = tds_acname
                    txtmtdsaccode.Text = tds_accode
                    Cmbsacname.Text = tds_acname
                    txtstdsaccode.TabIndex = tds_accode
                Else
                    Otdsname = Nothing
                    Mtdsname = Nothing
                    stdsname = Nothing
                End If
            Else
                If Otdsname <> Nothing Then
                    StrSql = "SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY where TDSCATNAME='" & Otdsname.ToString & "' "
                ElseIf Mtdsname <> Nothing Then
                    StrSql = "SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY where TDSCATNAME='" & Mtdsname.ToString & "' "
                ElseIf stdsname <> Nothing Then
                    StrSql = "SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY where TDSCATNAME='" & stdsname.ToString & "' "
                End If
                Dim tds_accode As String = objGPack.GetSqlValue(StrSql,, "")
                Cmboacname.Text = Otdsname
                txtotdsaccode.Text = tds_accode
                Cmbmacname.Text = Mtdsname
                txtmtdsaccode.Text = tds_accode
                Cmbsacname.Text = stdsname
                txtstdsaccode.Text = tds_accode
            End If
        End If

        chkMulti.Visible = False
        If oMaterial = Material.Receipt Then
            txtTagNo.Visible = False
            Label71.Visible = False
            txtItemId.Visible = False
            Label109.Visible = False
        End If
        If MRMI_PreTouch And oMaterial = Material.Receipt Then
            lblLTouch.Visible = True
            txtLastTouch.Visible = True
        End If
        If oEditRowIndex <> -1 Then
            grpMain.Enabled = False
            If rbtOrnament.Checked Then
                txtOOrdNo.Select()
            ElseIf rbtMetal.Checked Then
                txtMOrdNo.Select()
            ElseIf rbtStone.Checked Then
                GridStuddStone.Visible = False
                txtSOrdNo.Select()
            Else
                cmbOthMetal.Select()
            End If
        Else
            If STOCKVALIDATION_MR Then
                grpMain.Enabled = True
                If oMaterial = Material.Issue Then
                    rbtOrnament.Checked = False
                    rbtOrnament.Checked = True
                    Me.SelectNextControl(rbtOrnament, False, True, True, True)
                    txtMOrdNo.Select()
                    rbtOrnament.Focus()
                Else
                    rbtOrnament.Checked = False
                    rbtOrnament.Checked = True
                    If OProcessname <> Nothing And UCase(oTransactionType) <> "PURCHASE[APPROVAL]" Then
                        If rbtOrnament.Checked Then CmbOPurity.Select()
                        If rbtMetal.Checked Then CmbMPurity.Select()
                    Else
                        Me.SelectNextControl(rbtOrnament, False, True, True, True)
                    End If
                    If TdsomsFlag = True Then
                        If Oordno <> Nothing Then
                            txtOOrdNo.Text = Oordno
                            txtMOrdNo.Text = Oordno
                            txtSOrdNo.Text = Oordno
                            txtOOrdNo.Select()
                        ElseIf Mordno <> Nothing Then
                            txtOOrdNo.Text = Mordno
                            txtMOrdNo.Text = Mordno
                            txtSOrdNo.Text = Mordno
                            txtOOrdNo.Select()
                        ElseIf sordno <> Nothing Then
                            txtOOrdNo.Text = sordno
                            txtMOrdNo.Text = sordno
                            txtSOrdNo.Text = sordno
                            txtOOrdNo.Select()
                        Else
                            txtOOrdNo.Text = Oordno
                            txtMOrdNo.Text = Oordno
                            txtSOrdNo.Text = Oordno
                            txtOOrdNo.Select()
                        End If
                    Else
                        txtOOrdNo.Select()
                    End If

                    rbtOrnament.Focus()
                End If
                Exit Sub
            End If
            grpMain.Enabled = True
            If oMaterial = Material.Issue Then
                'rbtMetal.Checked = False
                'rbtMetal.Checked = True
                'Me.SelectNextControl(rbtMetal, False, True, True, True)
                rbtOrnament.Checked = False
                rbtOrnament.Checked = True
                Me.SelectNextControl(rbtOrnament, False, True, True, True)
                'rbtMetal.Focus()
            Else
                rbtOrnament.Checked = False
                rbtOrnament.Checked = True
                If OProcessname <> Nothing And UCase(oTransactionType) <> "PURCHASE[APPROVAL]" Then
                    If OProcessNo <> "" Then
                        txtOOrdNo.Text = OProcessNo
                    ElseIf Oordno <> "" Then
                        txtOOrdNo.Text = Oordno
                    ElseIf Mordno <> "" Then
                        txtMOrdNo.Text = Mordno
                    End If
                    If rbtOrnament.Checked Then CmbOPurity.Select() : Exit Sub
                    If rbtMetal.Checked Then CmbMPurity.Select() : Exit Sub
                Else
                    Me.SelectNextControl(rbtOrnament, False, True, True, True)
                End If
                'rbtOrnament.Focus()
            End If
        End If
        If oTransactionType = "RECEIPT" Then
            cmbOStkType.Text = "Manufacturing"
            CmbMStktype.Text = "Manufacturing"
        ElseIf oTransactionType = "PURCHASE" Then
            cmbOStkType.Text = "Trading"
            CmbMStktype.Text = "Trading"
        End If
        If RFID_Enable = False Then
            txtRfId.Visible = False
            lblRFID.Visible = False
        End If
        If LOCKWOPCRATE Then
            rbtOrnament.Enabled = Not LOCKWOPCRATE
            rbtMetal.Enabled = Not LOCKWOPCRATE
            rbtStone.Enabled = Not LOCKWOPCRATE
            rbtOthers.Enabled = True
            rbtOthers.Checked = True
            Me.SelectNextControl(rbtOthers, False, True, True, True)
            'rbtOthers.Focus()
        End If
        If OProcessNo <> "" Then
            txtOOrdNo.Text = OProcessNo
        ElseIf Oordno <> "" Then
            txtOOrdNo.Text = Oordno
        ElseIf Mordno <> "" Then
            txtMOrdNo.Text = Mordno
        End If
        lblBalance.Text = ""
        If UCase(oTransactionType) = "PURCHASE RETURN" Or
            UCase(oTransactionType) = "PURCHASE" Then
            txtOTCS_AMT.ReadOnly = False
            txtMTCS_AMT.ReadOnly = False
            txtSTCS_AMT.ReadOnly = False
        End If
        If (UCase(oTransactionType) = "PURCHASE RETURN" Or
            UCase(oTransactionType) = "PURCHASE") And MIMR_RATEFIXED Then
            chkORateFixed.Visible = True
            chkMRateFixed.Visible = True
            chkSRateFixed.Visible = False
            chkSRateFixed.Checked = False
            If editflag = False Then
                chkORateFixed.Checked = False
                chkMRateFixed.Checked = False
                chkSRateFixed.Checked = False
            End If
        Else
            chkORateFixed.Visible = False
            chkMRateFixed.Visible = False
            chkSRateFixed.Visible = False
            chkORateFixed.Checked = False
            chkMRateFixed.Checked = False
            chkSRateFixed.Checked = False
        End If
        ' UCase(oTransactionType) = "PURCHASE" Or UCase(oTransactionType) = "PURCHASE[APPROVAL]" Or  Newly Comment on 29-June-2021
        If UCase(oTransactionType) = "PURCHASE RETURN" Or
            UCase(oTransactionType) = "INTERNAL TRANSFER" Or
            UCase(oTransactionType) = "APPROVAL RECEIPT" Then
            If GstFlag Then
                lblOVatPer.Text = "Gst%"
                lblOVat.Text = "Gst"
                lblMVatPer.Text = "Gst%"
                lblMVat.Text = "Gst"
                lblSVatPer.Text = "Gst%"
                lblSVat.Text = "Gst"
                lblOthVatPer.Text = "Gst%"
                lblOthVat.Text = "Gst"
                txtOVatPer_PER.Text = IIf(PTaxPer <> 0, Format(PTaxPer, "0.00"), "")
                txtMVatPer_PER.Text = IIf(PTaxPer <> 0, Format(PTaxPer, "0.00"), "")
                txtSVatPer_PER.Text = IIf(PTaxPer <> 0, Format(PTaxPer, "0.00"), "")
                txtOthVatPer_PER.Text = IIf(PTaxPer <> 0, Format(PTaxPer, "0.00"), "")
            End If
        End If
        If GstFlag Then
            pnlEd.Visible = False
            PnlGst.Visible = True
            Label74.Visible = False
            Label75.Visible = False
            CmbMStktype.Visible = False
            cmbOStkType.Visible = False
        Else
            pnlEd.Visible = True
            PnlGst.Visible = False
        End If
        If DEFPROCESSIDS <> "" Then
            Dim defprocessarry() As String = DEFPROCESSIDS.Split(",")
            If defprocessarry.Length < 2 Then MsgBox("DEFAULT PROCESS PARAMETER NOT SET PROPERLY", MsgBoxStyle.Information) : GoTo SKIPDEFPROCESS
            If oMaterial = Material.Issue Then
                defProcVAlue = objGPack.GetSqlValue("SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID = " & Val(defprocessarry(0).ToString)).ToString
            ElseIf oMaterial = Material.Receipt Then
                defProcVAlue = objGPack.GetSqlValue("SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID = " & Val(defprocessarry(1).ToString)).ToString
            End If
SKIPDEFPROCESS:
        End If
        If editflag = True Then
            If oTransactionType = "RECEIPT" Then
                defProcVAlue = objGPack.GetSqlValue("SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID IN(SELECT  ORDSTATE_ID FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO='" & EditBatchno & "')")
            ElseIf oTransactionType = "ISSUE" Then
                defProcVAlue = objGPack.GetSqlValue("SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID IN(SELECT  ORDSTATE_ID FROM " & cnStockDb & "..ISSUE WHERE BATCHNO='" & EditBatchno & "')")
            End If
        End If
        cmbOProcess.Text = defProcVAlue
        cmbMProcess.Text = defProcVAlue
        cmbSProcess.Text = defProcVAlue
        rbtOrnament.Focus()
    End Sub
    Private Sub ClearAll() '1404
        objGPack.TextClear(grpOrnament)
        objGPack.TextClear(grpMetal)
        objGPack.TextClear(grpStone)
        objGPack.TextClear(grpOthers)
        objStone = New frmStoneDiaAc
        objMisc = New frmMiscDia
        ObjAlloy = New frmAlloyDetails
        MaterialStoneDia = New MaterialStoneDia
        objStone.FromFlag = "A"
        objStone.IssRecCat = ACC_STONEISSCAT
        objStone.IssRecStudWtDedut = ACC_STUDDEDUCT_OPTIONAL
        If rbtOrnament.Checked Then
            If OProcessname <> Nothing Then cmbOProcess.Text = OProcessname
            If OProcessname <> Nothing Then cmbMProcess.Text = OProcessname
            If OProcessname <> Nothing Then cmbSProcess.Text = OProcessname
            LoadMetal(cmbOMetal)
            LoadMetalDetails(cmbOMetal)
            If OMetalname <> Nothing Then cmbOMetal.Text = OMetalname
            If OCategoryname <> Nothing Then cmbOCategory.Text = OCategoryname
            If OIssCatName <> Nothing Then cmbOIssuedCategory.Text = OIssCatName
            If OAcpostCatname <> Nothing Then cmbOAcPostCategory.Text = OAcpostCatname
            If Opurityper <> 0 Then CmbOPurity.Text = Opurityper

            'If OProcessname <> Nothing Then cmbOItem.Focus()
        ElseIf rbtMetal.Checked Then
            If OProcessname <> Nothing Then cmbOProcess.Text = OProcessname
            If OProcessname <> Nothing Then cmbMProcess.Text = OProcessname
            If OProcessname <> Nothing Then cmbSProcess.Text = OProcessname
            LoadMetal(cmbMMetal)
            LoadMetalDetails(cmbMMetal)
        ElseIf rbtStone.Checked Then
            If OProcessname <> Nothing Then cmbOProcess.Text = OProcessname
            If OProcessname <> Nothing Then cmbMProcess.Text = OProcessname
            If OProcessname <> Nothing Then cmbSProcess.Text = OProcessname
            LoadMetal(cmbSMetal)
            LoadMetalDetails(cmbSMetal)
        ElseIf rbtOthers.Checked Then
            LoadMetal(cmbOthMetal)
            LoadMetalDetails(cmbOthMetal)
        End If
        MdtOdDet.Clear()
        dgvOrder.DataSource = Nothing
        grpOrder.Visible = False
        'MdtOdDet = New DataTable
        'With MdtOdDet.Columns
        '    .Add("ORNO", GetType(String))
        '    .Add("ORSNO", GetType(String))
        '    .Add("PCS", GetType(Int32))
        '    .Add("GRSWT", GetType(Decimal))
        '    .Add("NETWT", GetType(Decimal))
        '    .Add("WASTAGE", GetType(Decimal))
        '    .Add("MC", GetType(Decimal))
        'End With
        'Dim mDr As DataRow
        'mDr = MdtOdDet.NewRow
        'mDr("ORNO") = "TOTAL"
        'mDr("PCS") = "0"
        'mDr("GRSWT") = "0.000"
        'mDr("NETWT") = "0.000"
        'mDr("WASTAGE") = "0.000"
        'mDr("MC") = "0.000"
        'MdtOdDet.Rows.Add(mDr)
        'MdtOdDet.AcceptChanges()
        'dgvOrder = New DataGridView
        'With dgvOrder
        '    .DataSource = Nothing
        '    .DataSource = MdtOdDet
        '    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        '    .ScrollBars = ScrollBars.Vertical
        '    dgvOrder.Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        '    dgvOrder.Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        '    dgvOrder.Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        '    dgvOrder.Columns("WASTAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        '    dgvOrder.Columns("MC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        '    dgvOrder.Columns("WASTAGE").Visible = False
        '    dgvOrder.Columns("MC").Visible = False
        'End With

        txtORemark1.Text = Remark1
        txtORemark2.Text = Remark2
        txtMRemark1.Text = Remark1
        txtMRemark2.Text = Remark2
        txtSRemark1.Text = Remark1
        txtSRemark2.Text = Remark2
        txtOthRemark1.Text = Remark1
        txtOthRemark2.Text = Remark2
        If UCase(oTransactionType) <> Nothing And UCase(oTransactionType) <> "PURCHASE RETURN" And UCase(oTransactionType) <> "PURCHASE" And UCase(oTransactionType) <> "PURCHASE[APPROVAL]" And UCase(oTransactionType) <> "INTERNAL TRANSFER" Then
            txtOVatPer_PER.Text = IIf(TdsPer <> 0, Format(TdsPer, "0.00"), "")
            txtMVatPer_PER.Text = IIf(TdsPer <> 0, Format(TdsPer, "0.00"), "")
            txtSVatPer_PER.Text = IIf(TdsPer <> 0, Format(TdsPer, "0.00"), "")
            txtOthVatPer_PER.Text = IIf(TdsPer <> 0, Format(TdsPer, "0.00"), "")
            If PURTDSEDITACC Then
                txtOVatPer_PER.ReadOnly = False
                txtMVatPer_PER.ReadOnly = False
                txtSVatPer_PER.ReadOnly = False
                txtOthVatPer_PER.ReadOnly = False
                txtOVat_AMT.ReadOnly = False
                txtMVat_AMT.ReadOnly = False
                txtSVat_AMT.ReadOnly = False
                txtOthVat_AMT.ReadOnly = False
            Else
                txtOVatPer_PER.ReadOnly = True
                txtMVatPer_PER.ReadOnly = True
                txtSVatPer_PER.ReadOnly = True
                txtOthVatPer_PER.ReadOnly = True
                txtOVat_AMT.ReadOnly = True
                txtMVat_AMT.ReadOnly = True
                txtSVat_AMT.ReadOnly = True
                txtOthVat_AMT.ReadOnly = True
            End If
        End If
        ''1404
        ''TdsomsFlag = __TdsomsFlag
        __TdsomsFlag = TdsomsFlag
        If TdsomsFlag = False Then
            StrSql = "SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY  ORDER BY DISPLAYORDER "
            objGPack.FillCombo(StrSql, Cmboacname, True, False)
            objGPack.FillCombo(StrSql, Cmbmacname, True, False)
            objGPack.FillCombo(StrSql, Cmbsacname, True, False)
            Dim TDSCATID As Integer = Val(objGPack.GetSqlValue("SELECT TDSCATID FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & Maccode & "' AND ISNULL(TDSFLAG,'') = 'Y'",, 0))
            If TDSCATID <> 0 Then
                Dim tds_acname As String = objGPack.GetSqlValue("SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATID = '" & TDSCATID & "'",, "")
                Dim tds_accode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATID = '" & TDSCATID & "'",, "")
                Cmboacname.Text = tds_acname
                txtotdsaccode.Text = tds_accode
                Cmbmacname.Text = tds_acname
                txtmtdsaccode.Text = tds_accode
                Cmbsacname.Text = tds_acname
                txtstdsaccode.Text = tds_accode
            End If
            Otdsname = Nothing
            Mtdsname = Nothing
            stdsname = Nothing
        Else
            If Otdsname <> Nothing Then
                StrSql = "SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY where TDSCATNAME='" & Otdsname.ToString & "'  ORDER BY DISPLAYORDER "
            ElseIf Mtdsname <> Nothing Then
                StrSql = "SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY where TDSCATNAME='" & Mtdsname.ToString & "'  ORDER BY DISPLAYORDER "
            ElseIf stdsname <> Nothing Then
                StrSql = "SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY where TDSCATNAME='" & stdsname.ToString & "'  ORDER BY DISPLAYORDER "
            Else
                StrSql = "SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY  ORDER BY DISPLAYORDER "
            End If
            objGPack.FillCombo(StrSql, Cmboacname, True, False)
            objGPack.FillCombo(StrSql, Cmbmacname, True, False)
            objGPack.FillCombo(StrSql, Cmbsacname, True, False)
            If Otdsname Is Nothing And Mtdsname Is Nothing And stdsname Is Nothing Then
                Dim TDSCATID As Integer = Val(objGPack.GetSqlValue("SELECT TDSCATID FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & Maccode & "' AND ISNULL(TDSFLAG,'') = 'Y'",, 0))
                If TDSCATID <> 0 Then
                    Dim tds_acname As String = objGPack.GetSqlValue("SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATNAME = '" & Otdsname.ToString & "'",, "")
                    Dim tds_accode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATNAME = '" & Otdsname.ToString & "'",, "")
                    Cmboacname.Text = tds_acname
                    txtotdsaccode.Text = tds_accode
                    Cmbmacname.Text = tds_acname
                    txtmtdsaccode.Text = tds_accode
                    Cmbsacname.Text = tds_acname
                    txtstdsaccode.Text = tds_accode
                Else
                    Otdsname = Nothing
                    Mtdsname = Nothing
                    stdsname = Nothing
                End If
            Else
                If Otdsname <> Nothing Then
                    StrSql = "SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY where TDSCATNAME='" & Otdsname.ToString & "' "
                ElseIf Mtdsname <> Nothing Then
                    StrSql = "SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY where TDSCATNAME='" & Mtdsname.ToString & "' "
                ElseIf stdsname <> Nothing Then
                    StrSql = "SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY where TDSCATNAME='" & stdsname.ToString & "' "
                End If
                Dim tds_accode As String = objGPack.GetSqlValue(StrSql,, "")
                Cmboacname.Text = Otdsname
                txtotdsaccode.Text = tds_accode
                Cmbmacname.Text = Mtdsname
                txtmtdsaccode.Text = tds_accode
                Cmbsacname.Text = stdsname
                txtstdsaccode.Text = tds_accode
            End If
        End If

    End Sub

    Private Sub rbtMetal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles _
    rbtMetal.KeyPress _
    , rbtOrnament.KeyPress _
    , rbtStone.KeyPress _
    , rbtOthers.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If rbtMetal.Checked Then
                If oTransactionType = "PURCHASE[APPROVAL]" Then LoadSmithAppDetails()
                txtMOrdNo.Focus()
            ElseIf rbtOrnament.Checked Then
                If oTransactionType = "PURCHASE[APPROVAL]" Then LoadSmithAppDetails()
                txtOOrdNo.Focus()
            ElseIf rbtStone.Checked Then
                If oTransactionType = "PURCHASE[APPROVAL]" Then LoadSmithStnAppDetails()
                txtSOrdNo.Focus()
            ElseIf rbtOthers.Checked Then
                cmbOthMetal.Focus()
            End If
        End If
        If oMaterial = Material.Issue Then
            If rbtOrnament.Checked = True Then
                txtItemId.Enabled = True
                txtTagNo.Enabled = True
                ''txtTagNo.Select()
                txtItemId.Select()
            ElseIf rbtStone.Checked = True Then
                txtItemId.Enabled = True
                txtTagNo.Enabled = True
                ''txtTagNo.Select()
                txtItemId.Select()
            Else
                txtItemId.Enabled = False
                txtTagNo.Enabled = False
                txtItemId.Clear()
                txtTagNo.Clear()
            End If
        Else
            txtItemId.Enabled = False
            txtTagNo.Enabled = False
            txtItemId.Clear()
            txtTagNo.Clear()
        End If
    End Sub
    Private Sub Type_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
    rbtMetal.CheckedChanged _
    , rbtOrnament.CheckedChanged _
    , rbtStone.CheckedChanged _
    , rbtOthers.CheckedChanged
        ClearAll()
        grpOrnament.Visible = False
        grpMetal.Visible = False
        grpStone.Visible = False
        grpOthers.Visible = False
        If rbtMetal.Checked Then
            grpMetal.Visible = True
        ElseIf rbtOrnament.Checked Then
            grpOrnament.Visible = True
        ElseIf rbtStone.Checked Then
            grpStone.Visible = True
        ElseIf rbtOthers.Checked Then
            grpOthers.Visible = True
        End If

        If oMaterial = Material.Issue Then
            If rbtOrnament.Checked = True Then
                txtItemId.Enabled = True
                txtTagNo.Enabled = True
                ''txtTagNo.Select()
                txtItemId.Select()
            ElseIf rbtStone.Checked = True Then
                txtItemId.Enabled = True
                txtTagNo.Enabled = True
                ''txtTagNo.Select()
                txtItemId.Select()
            Else
                txtItemId.Enabled = False
                txtTagNo.Enabled = False
                txtItemId.Clear()
                txtTagNo.Clear()
            End If
        Else
            txtItemId.Enabled = False
            txtTagNo.Enabled = False
            txtItemId.Clear()
            txtTagNo.Clear()
        End If
        If rbtStone.Checked = True Then
            If oMaterial = Material.Receipt Then
                CmbSPurity.Enabled = False
            End If
        End If
        If Oordno <> Nothing Then
            txtOOrdNo.Text = Oordno
            txtMOrdNo.Text = Oordno
            txtSOrdNo.Text = Oordno
            txtOOrdNo.Select()
        ElseIf Mordno <> Nothing Then
            txtOOrdNo.Text = Mordno
            txtMOrdNo.Text = Mordno
            txtSOrdNo.Text = Mordno
            txtOOrdNo.Select()
        ElseIf sordno <> Nothing Then
            txtOOrdNo.Text = sordno
            txtMOrdNo.Text = sordno
            txtSOrdNo.Text = sordno
            txtOOrdNo.Select()
        Else
            txtOOrdNo.Text = Oordno
            txtMOrdNo.Text = Oordno
            txtSOrdNo.Text = Oordno
            txtOOrdNo.Select()
        End If
        If rbtMetal.Checked Then
            rbtMetal.Select()
        ElseIf rbtStone.Checked Then
            rbtStone.Select()
        ElseIf rbtOthers.Checked Then
            rbtOthers.Select()
        End If
    End Sub

    Private Sub cmbOProcess_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOProcess.GotFocus
        If defProcVAlue <> "" Then cmbOProcess.Text = defProcVAlue
    End Sub

    Private Sub cmbMProcess_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMProcess.GotFocus
        If defProcVAlue <> "" Then cmbMProcess.Text = defProcVAlue

    End Sub

    Private Sub cmbSProcess_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSProcess.GotFocus
        If defProcVAlue <> "" Then cmbSProcess.Text = defProcVAlue
    End Sub

    Private Sub Combo_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles _
    cmbOMetal.KeyDown _
    , cmbOCategory.KeyDown _
    , cmbOIssuedCategory.KeyDown _
    , cmbOAcPostCategory.KeyDown _
    , cmbOItem.KeyDown _
    , cmbOSubItem.KeyDown _
    , cmbMMetal.KeyDown _
    , cmbMCategory.KeyDown _
    , cmbMIssuedCategory.KeyDown _
    , cmbMAcPostCategory.KeyDown _
    , cmbSMetal.KeyDown _
    , cmbSCategory.KeyDown _
    , cmbSIssuedCategory.KeyDown _
    , cmbSAcPostCategory.KeyDown _
    , cmbSItem.KeyDown _
    , cmbSSubItem.KeyDown _
    , cmbOthMetal.KeyDown _
    , cmbOthCategory.KeyDown _
    , cmbOthItem.KeyDown _
    , cmbOthSubItem.KeyDown _
    , cmbOProcess.KeyDown _
    , cmbMProcess.KeyDown _
    , cmbSProcess.KeyDown
        If e.KeyCode = Keys.Enter Then
            If CType(sender, ComboBox).FindStringExact(CType(sender, ComboBox).SelectedText) <> -1 Then
                SendKeys.Send("{TAB}")
            Else
                If cmbOItem.Focused And txtTagNo.Text <> "" Then
                    SendKeys.Send("{TAB}")
                End If
            End If
        End If
    End Sub

    Private Sub tabMain_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabMain.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Public Sub LoadMetalDetails(ByVal CmbMetal As ComboBox)
        Dim metalId As String = ""
        If CmbMetal.SelectedValue Is Nothing Or CmbMetal.Text = "" Then
            metalId = ""
            If rbtOthers.Checked Then metalId = CmbMetal.SelectedValue.ToString
            If rbtStone.Checked Then metalId = CmbMetal.SelectedValue.ToString
        Else
            metalId = CmbMetal.SelectedValue.ToString
        End If
        Dim metaltype As String = ""
        If rbtOrnament.Checked Then
            metaltype = "O"
        ElseIf rbtMetal.Checked Then
            metaltype = "M"
        ElseIf rbtStone.Checked Then
            metaltype = "T"
            If metalId = "D" Then metaltype = ""
        End If
        If metalId = "" Then metalId = "G"
        StrSql = "SELECT CATNAME,CATCODE FROM " & cnAdminDb & "..CATEGORY "
        StrSql += " WHERE METALID = '" & metalId & "'"
        StrSql += " AND ISNULL(ACTIVE,'')<>'N' "
        If metaltype <> "T" And metaltype <> "" Then StrSql += " AND PURITYID IN (SELECT PURITYID FROM " & cnAdminDb & ".. PURITYMAST WHERE METALTYPE ='" & metaltype & "')"
        StrSql += " ORDER BY DISPLAYORDER,CATNAME"
        Dim dtTemp As New DataTable
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtTemp)
        Dim Cmb As ComboBox = Nothing
        If rbtOrnament.Checked Then
            Cmb = cmbOCategory
        ElseIf rbtMetal.Checked Then
            Cmb = cmbMCategory
        ElseIf rbtStone.Checked Then
            Cmb = cmbSCategory
        ElseIf rbtOthers.Checked Then
            Cmb = cmbOthCategory
        End If
        If Not dtTemp.Rows.Count > 0 Then
            Cmb.Enabled = False
            Cmb.Text = ""
        Else
            Cmb.Enabled = True
        End If
        Cmb.DataSource = dtTemp
        Cmb.ValueMember = "CATCODE"
        Cmb.DisplayMember = "CATNAME"
        Cmb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        Cmb.AutoCompleteSource = AutoCompleteSource.ListItems
        If Cmb.ValueMember Is Nothing Then
            oBoardRate = Val(GetRate(oTranDate, objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & Cmb.Text & "'")))
        End If
        If rbtOrnament.Checked Then
            CalcORate()
        ElseIf rbtMetal.Checked Then
            CalcMRate()
        ElseIf rbtStone.Checked Then
            CalcSRate()
        ElseIf rbtOthers.Checked Then
            CalcOthRate()
        End If


        Dim dtTemp1 As New DataTable
        StrSql = "SELECT CATNAME,CATCODE FROM " & cnAdminDb & "..CATEGORY "
        StrSql += " WHERE METALID = '" & metalId & "'"
        StrSql += " AND ISNULL(ACTIVE,'')<>'N' "
        If metaltype <> "T" And metaltype <> "" Then StrSql += " AND PURITYID IN (SELECT PURITYID FROM " & cnAdminDb & ".. PURITYMAST WHERE METALTYPE ='" & metaltype & "')"
        StrSql += " ORDER BY DISPLAYORDER,CATNAME"

        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(dtTemp1)
        Dim Cmbx As ComboBox = Nothing
        If rbtOrnament.Checked Then
            Cmbx = cmbOAcPostCategory
        ElseIf rbtMetal.Checked Then
            Cmbx = cmbMAcPostCategory
        ElseIf rbtStone.Checked Then
            Cmbx = cmbSAcPostCategory
        ElseIf rbtOthers.Checked Then
            ' Cmb = cmbOthCategory
            Cmbx = New ComboBox
        End If
        If Not dtTemp1.Rows.Count > 0 Then
            Cmbx.Enabled = False
            Cmbx.Text = ""
        Else
            Cmbx.Enabled = True
        End If
        Cmbx.DataSource = dtTemp1
        Cmbx.ValueMember = "CATCODE"
        Cmbx.DisplayMember = "CATNAME"
        Cmbx.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        Cmbx.AutoCompleteSource = AutoCompleteSource.ListItems


        ''ISSUED CATEGORY
        If oMaterial = Material.Receipt Then
            StrSql = "SELECT CATNAME,CATCODE FROM " & cnAdminDb & "..CATEGORY "
            StrSql += " WHERE METALID = '" & metalId & "'"
            StrSql += " AND ISNULL(ACTIVE,'')<>'N' "
            StrSql += " ORDER BY DISPLAYORDER,CATNAME"
            dtTemp = New DataTable
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dtTemp)
            Dim Cmb1 As ComboBox
            If rbtOrnament.Checked Then
                Cmb1 = cmbOIssuedCategory
            ElseIf rbtMetal.Checked Then
                Cmb1 = cmbMIssuedCategory
            ElseIf rbtStone.Checked Then
                Cmb1 = cmbSIssuedCategory
            Else
                Cmb1 = New ComboBox
            End If
            If Not dtTemp.Rows.Count > 0 Then
                Cmb1.Enabled = False
                Cmb1.Text = ""
            Else
                Cmb1.Enabled = True
            End If
            Cmb1.DataSource = dtTemp
            Cmb1.ValueMember = "CATCODE"
            Cmb1.DisplayMember = "CATNAME"
            Cmb1.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            Cmb1.AutoCompleteSource = AutoCompleteSource.ListItems
        End If
    End Sub

    Public Sub LoadCategoryDetails(ByVal cmbCat As ComboBox)
        Dim Cmb As ComboBox
        Dim isBar As Boolean = False
        Dim CatCode As String = Nothing
        Cmb = cmbCat
        If Cmb.Text = "" Or Cmb.SelectedValue Is Nothing Then
            CatCode = ""
        Else
            CatCode = Cmb.SelectedValue.ToString
        End If
        If UCase(oTransactionType) = "PURCHASE RETURN" Or
            UCase(oTransactionType) = "PURCHASE" Or
            UCase(oTransactionType) = "PURCHASE[APPROVAL]" Or
            UCase(oTransactionType) = "INTERNAL TRANSFER" Or
            UCase(oTransactionType) = "APPROVAL RECEIPT" Then
            If GstFlag Then
                StrSql = "SELECT * FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE='" & CatCode & "'"
                Dim dr As DataRow = GetSqlRow(StrSql, cn)
                If Not dr Is Nothing Then
                    PTaxPer = Val(dr("P_SGSTTAX").ToString) + Val(dr("P_CGSTTAX").ToString)
                    If UCase(oTransactionType) = "INTERNAL TRANSFER" Then
                        If INTERNALTRANSFER_NONTAXABLE = True Then
                            PTaxPer = 0
                        End If
                    End If
                    If _accStateId = CompanyStateId Then
                        txtOSgst_AMT.Text = Format(Val(dr("P_SGSTTAX").ToString), "0.000")
                        txtOCgst_AMT.Text = Format(Val(dr("P_CGSTTAX").ToString), "0.000")
                        txtMSgst_AMT.Text = Format(Val(dr("P_SGSTTAX").ToString), "0.000")
                        txtMCgst_AMT.Text = Format(Val(dr("P_CGSTTAX").ToString), "0.000")
                        txtSSgst_WET.Text = Format(Val(dr("P_SGSTTAX").ToString), "0.000")
                        txtSCgst_WET.Text = Format(Val(dr("P_CGSTTAX").ToString), "0.000")
                        txtOthSgst_AMT.Text = Format(Val(dr("P_SGSTTAX").ToString), "0.000")
                        txtOthCgst_AMT.Text = Format(Val(dr("P_CGSTTAX").ToString), "0.000")
                        If UCase(oTransactionType) = "INTERNAL TRANSFER" Then
                            If INTERNALTRANSFER_NONTAXABLE = True Then
                                If PTaxPer = 0 Then
                                    txtOSgst_AMT.Text = ""
                                    txtOCgst_AMT.Text = ""
                                    txtMSgst_AMT.Text = ""
                                    txtMCgst_AMT.Text = ""
                                    txtSSgst_WET.Text = ""
                                    txtSCgst_WET.Text = ""
                                    txtOthSgst_AMT.Text = ""
                                    txtOthCgst_AMT.Text = ""
                                    txtOSgst_AMT.ReadOnly = True
                                    txtOCgst_AMT.ReadOnly = True
                                    txtOSG_AMT.ReadOnly = True
                                    txtOCG_AMT.ReadOnly = True
                                    txtMSgst_AMT.ReadOnly = True
                                    txtMCgst_AMT.ReadOnly = True
                                    txtMSG_AMT.ReadOnly = True
                                    txtMCG_AMT.ReadOnly = True
                                    txtSSgst_WET.ReadOnly = True
                                    txtSCgst_WET.ReadOnly = True
                                    txtSSG_AMT.ReadOnly = True
                                    txtSCG_AMT.ReadOnly = True
                                    txtOthSgst_AMT.ReadOnly = True
                                    txtOthCgst_AMT.ReadOnly = True
                                    txtOthSG_AMT.ReadOnly = True
                                    txtOthCG_AMT.ReadOnly = True
                                End If
                            End If
                        End If
                    Else
                        txtOIgst_AMT.Text = Format(Val(dr("P_IGSTTAX").ToString), "0.000")
                        txtMIgst_AMT.Text = Format(Val(dr("P_IGSTTAX").ToString), "0.000")
                        txtSIgst_WET.Text = Format(Val(dr("P_IGSTTAX").ToString), "0.000")
                        txtOthIgst_AMT.Text = Format(Val(dr("P_IGSTTAX").ToString), "0.000")
                        If UCase(oTransactionType) = "INTERNAL TRANSFER" Then
                            If INTERNALTRANSFER_NONTAXABLE = True Then
                                If PTaxPer = 0 Then
                                    txtOIgst_AMT.Text = ""
                                    txtMIgst_AMT.Text = ""
                                    txtSIgst_WET.Text = ""
                                    txtOthIgst_AMT.Text = ""
                                    txtOIgst_AMT.ReadOnly = True
                                    txtOIG_AMT.ReadOnly = True
                                    txtMIgst_AMT.ReadOnly = True
                                    txtMIG_AMT.ReadOnly = True
                                    txtSIgst_WET.ReadOnly = True
                                    txtSIG_AMT.ReadOnly = True
                                    txtOthIgst_AMT.ReadOnly = True
                                    txtOthIG_AMT.ReadOnly = True
                                End If
                            End If
                        End If
                    End If
                End If
                txtOVatPer_PER.Text = IIf(PTaxPer <> 0, Format(PTaxPer, "0.000"), "")
                txtMVatPer_PER.Text = IIf(PTaxPer <> 0, Format(PTaxPer, "0.000"), "")
                txtSVatPer_PER.Text = IIf(PTaxPer <> 0, Format(PTaxPer, "0.000"), "")
                txtOthVatPer_PER.Text = IIf(PTaxPer <> 0, Format(PTaxPer, "0.000"), "")
            End If
        End If
        If _accStateId = CompanyStateId Then
            txtOIgst_AMT.Enabled = False
            txtOIG_AMT.Enabled = False
            txtOSgst_AMT.Enabled = True
            txtOCgst_AMT.Enabled = True
            txtOSG_AMT.Enabled = True
            txtOCG_AMT.Enabled = True
            txtMIgst_AMT.Enabled = False
            txtMIG_AMT.Enabled = False
            txtMSgst_AMT.Enabled = True
            txtMCgst_AMT.Enabled = True
            txtMSG_AMT.Enabled = True
            txtMCG_AMT.Enabled = True
            txtSIgst_WET.Enabled = False
            txtSIG_AMT.Enabled = False
            txtSSgst_WET.Enabled = True
            txtSCgst_WET.Enabled = True
            txtSSG_AMT.Enabled = True
            txtSCG_AMT.Enabled = True
            txtOthIgst_AMT.Enabled = False
            txtOthIG_AMT.Enabled = False
            txtOthSgst_AMT.Enabled = True
            txtOthCgst_AMT.Enabled = True
            txtOthSG_AMT.Enabled = True
            txtOthCG_AMT.Enabled = True
        Else
            txtOIgst_AMT.Enabled = True
            txtOIG_AMT.Enabled = True
            txtOSgst_AMT.Enabled = False
            txtOCgst_AMT.Enabled = False
            txtOSG_AMT.Enabled = False
            txtOCG_AMT.Enabled = False
            txtMIgst_AMT.Enabled = True
            txtMIG_AMT.Enabled = True
            txtMSgst_AMT.Enabled = False
            txtMCgst_AMT.Enabled = False
            txtMSG_AMT.Enabled = False
            txtMCG_AMT.Enabled = False
            txtSIgst_WET.Enabled = True
            txtSIG_AMT.Enabled = True
            txtSSgst_WET.Enabled = False
            txtSCgst_WET.Enabled = False
            txtSSG_AMT.Enabled = False
            txtSCG_AMT.Enabled = False
            txtOthIgst_AMT.Enabled = True
            txtOthIG_AMT.Enabled = True
            txtOthSgst_AMT.Enabled = False
            txtOthCgst_AMT.Enabled = False
            txtOthSG_AMT.Enabled = False
            txtOthCG_AMT.Enabled = False
        End If
        If objGPack.GetSqlValue("SELECT CATGROUP FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & CatCode & "'") = "B" Then
            isBar = True
        End If
        lblMRecPurity.Visible = False
        txtMReceivePurity_AMT.Visible = False

        ''PURITY
        StrSql = " SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & CatCode & "')"
        Dim purity As Decimal = Nothing
        purity = Val(objGPack.GetSqlValue(StrSql))
        StrSql = " SELECT METALTYPE FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & CatCode & "')"
        Dim METALTYPE As String = ""
        METALTYPE = objGPack.GetSqlValue(StrSql)
        If METALTYPE = "O" Then oStud = True
        Dim dtTemp As New DataTable
        If DVAACCESS = "Y" And Maccode <> Nothing And DdtItemDet.Rows.Count > 0 Then
            Dim DView As New DataView
            DView = DdtItemDet.DefaultView
            DView.RowFilter = "CATCODE='" & CatCode & "'"
            dtTemp = DView.ToTable(True, New String() {"ITEMID", "ITEMNAME"})
        Else
            StrSql = " SELECT ITEMNAME,ITEMID FROM " & cnAdminDb & "..ITEMMAST"
            StrSql += " WHERE ACTIVE = 'Y'"
            StrSql += GetItemQryFilteration()
            StrSql += " AND CATCODE = '" & CatCode & "'"
            StrSql += " ORDER BY ITEMNAME"
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dtTemp)
        End If

        Dim CmbDest As ComboBox = Nothing
        If rbtOrnament.Checked Then
            If isBar = True Then
                'lblOWastPer.Text = "Alloy %"
                'lblOWast.Text = "Alloy"
                lblOalloy.Visible = True
                txtOalloy_WET.Visible = True
            Else
                'lblOWastPer.Text = "Wastage %"
                'lblOWast.Text = "Wastage"
                lblOalloy.Visible = False
                txtOalloy_WET.Visible = False
            End If
            CmbOPurity.Text = IIf(purity <> 0, Format(purity, "0.00"), Nothing)
            CalcOGrossAmt()
            CmbDest = cmbOItem
            cmbOAcPostCategory.Text = cmbOCategory.Text
        ElseIf rbtMetal.Checked Then
            If isBar = True Then
                lblMRecPurity.Visible = True
                txtMReceivePurity_AMT.Visible = True
                'lblMWastPer.Text = "Alloy %"
                'lblMWast.Text = "Alloy"

                'txtMAlloy_WET.Enabled = True
                'txtMAlloyper.Enabled = True
            Else
                'lblMWastPer.Text = "Wastage %"
                'lblMWast.Text = "Wastage"

                'txtMAlloy_WET.Enabled = False
                'txtMAlloyper.Enabled = False
            End If
            cmbMAcPostCategory.Text = cmbMCategory.Text
            CmbMPurity.Text = IIf(purity <> 0, Format(purity, "0.00"), Nothing)
            CalcMGrossAmt()

            CmbDest = New ComboBox
        ElseIf rbtStone.Checked Then
            cmbSAcPostCategory.Text = cmbSCategory.Text
            CmbSPurity.Text = IIf(purity <> 0, Format(purity, "0.00"), Nothing)
            CmbDest = cmbSItem

        ElseIf rbtOthers.Checked Then
            CalcOthGrossAmt(Me, New EventArgs)
            CmbDest = cmbOthItem
        End If
        If Not dtTemp.Rows.Count > 0 Then
            CmbDest.Enabled = False
            CmbDest.Text = ""
        Else
            CmbDest.Enabled = True
        End If
        If (lblOVatPer.Text.Contains("Tds") Or lblMVatPer.Text.Contains("Tds") Or lblSVatPer.Text.Contains("Tds")) Then
            txtOVatPer_PER.Text = ""
            txtMVatPer_PER.Text = ""
            If rbtStone.Checked = False Then
                txtSVatPer_PER.Text = ""
            End If
        End If
        If Maccode <> "" Then
            If TdsomsFlag = False Then
                Dim _Temptdsper As Decimal = 0
                StrSql = "SELECT ISNULL(TDSPER,0)TDSPER FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & Maccode & "'"
                _Temptdsper = Val(objGPack.GetSqlValue(StrSql, "TDSPER", ).ToString)
                If (lblOVatPer.Text.Contains("Tds") Or lblMVatPer.Text.Contains("Tds") Or lblSVatPer.Text.Contains("Tds")) And Val(_Temptdsper) > 0 Then
                    txtOVatPer_PER.Text = Format(Val(_Temptdsper), "0.000")
                    txtMVatPer_PER.Text = Format(Val(_Temptdsper), "0.000")
                    txtSVatPer_PER.Text = Format(Val(_Temptdsper), "0.000")
                End If
            Else
                If Otdsname <> Nothing Then
                    StrSql = "SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY where TDSCATNAME='" & Otdsname.ToString & "'  ORDER BY DISPLAYORDER "
                ElseIf mtdsname <> Nothing Then
                    StrSql = "SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY where TDSCATNAME='" & Mtdsname.ToString & "'  ORDER BY DISPLAYORDER "
                ElseIf stdsname <> Nothing Then
                    StrSql = "SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY where TDSCATNAME='" & stdsname.ToString & "'  ORDER BY DISPLAYORDER "
                Else
                    StrSql = "SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY  ORDER BY DISPLAYORDER "
                End If
                objGPack.FillCombo(StrSql, Cmboacname, True, False)
                objGPack.FillCombo(StrSql, Cmbmacname, True, False)
                objGPack.FillCombo(StrSql, Cmbsacname, True, False)
                If Otdsname Is Nothing And Mtdsname Is Nothing And stdsname Is Nothing Then
                    Dim TDSCATID As Integer = Val(objGPack.GetSqlValue("SELECT TDSCATID FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & Maccode & "' AND ISNULL(TDSFLAG,'') = 'Y'",, 0))
                    If TDSCATID <> 0 Then
                        Dim tds_acname As String = objGPack.GetSqlValue("SELECT TDSCATNAME FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATNAME = '" & Otdsname.ToString & "'",, "")
                        Dim tds_accode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATNAME = '" & Otdsname.ToString & "'",, "")
                        Cmboacname.Text = tds_acname
                        txtotdsaccode.Text = tds_accode
                        Cmbmacname.Text = tds_acname
                        txtmtdsaccode.Text = tds_accode
                        Cmbsacname.Text = tds_acname
                        txtstdsaccode.Text = tds_accode
                    Else
                        Otdsname = Nothing
                        Mtdsname = Nothing
                        stdsname = Nothing
                    End If
                Else
                    If Otdsname <> Nothing Then
                        StrSql = "SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY where TDSCATNAME='" & Otdsname.ToString & "' "
                    ElseIf Mtdsname <> Nothing Then
                        StrSql = "SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY where TDSCATNAME='" & Mtdsname.ToString & "' "
                    ElseIf stdsname <> Nothing Then
                        StrSql = "SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY where TDSCATNAME='" & stdsname.ToString & "' "
                    End If
                    Dim tds_accode As String = objGPack.GetSqlValue(StrSql,, "")
                    Cmboacname.Text = Otdsname
                    txtotdsaccode.Text = tds_accode
                    Cmbmacname.Text = Mtdsname
                    txtmtdsaccode.Text = tds_accode
                    Cmbsacname.Text = stdsname
                    txtstdsaccode.Text = tds_accode
                End If

            End If

        End If
        CmbDest.DataSource = dtTemp
        CmbDest.ValueMember = "ITEMID"
        CmbDest.DisplayMember = "ITEMNAME"
        CmbDest.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        CmbDest.AutoCompleteSource = AutoCompleteSource.ListItems
    End Sub

    Public Sub LoadItemDetail(ByVal CmbItem As ComboBox)
        Dim Cmb As ComboBox
        Dim ItemId As Integer = Nothing
        Cmb = CmbItem
        If Cmb.Text = "" Or Cmb.SelectedValue Is Nothing Then ItemId = 0 Else ItemId = Val(Cmb.SelectedValue.ToString)
        Dim dtTemp As New DataTable
        If DVAACCESS = "Y" And Maccode <> Nothing And DdtItemDet.Rows.Count > 0 Then
            Dim DView As New DataView
            DView = DdtItemDet.DefaultView
            DView.RowFilter = "ITEMID='" & ItemId & "'"
            dtTemp = DView.ToTable(True, New String() {"SUBITEMID", "SUBITEMNAME"})
        Else
            StrSql = " SELECT SUBITEMNAME,SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST"
            StrSql += " WHERE ACTIVE = 'Y'"
            StrSql += " AND ITEMID = " & ItemId & ""
            StrSql += " ORDER BY SUBITEMNAME"
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dtTemp)
        End If
        Dim CmbDest As ComboBox
        If rbtOrnament.Checked Then
            CmbDest = cmbOSubItem
        ElseIf rbtStone.Checked Then
            CmbDest = cmbSSubItem
        ElseIf rbtOthers.Checked Then
            CmbDest = cmbOthSubItem
        Else
            CmbDest = New ComboBox
        End If
        If Not dtTemp.Rows.Count > 0 Then
            CmbDest.Enabled = False
            CmbDest.Text = ""
        Else
            CmbDest.Enabled = True
        End If
        CmbDest.DataSource = dtTemp
        CmbDest.ValueMember = "SUBITEMID"
        CmbDest.DisplayMember = "SUBITEMNAME"
        CmbDest.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        CmbDest.AutoCompleteSource = AutoCompleteSource.ListItems
    End Sub
    Private Sub stonepropint()
        objStone.cmbIssRecCat.Visible = ACC_STONEISSCAT
        objStone.Label1.Visible = ACC_STONEISSCAT
        objStone.lblStud.Visible = ACC_STUDDEDUCT_OPTIONAL
        objStone.CmbStudDeduct.Visible = ACC_STUDDEDUCT_OPTIONAL
        If ACC_STONEISSCAT = False And STONEISSCAT_EDIT = False Then
            objStone.Label26.Left -= objStone.cmbIssRecCat.Width
            objStone.Label57.Left -= objStone.cmbIssRecCat.Width
            objStone.Label58.Left -= objStone.cmbIssRecCat.Width
            objStone.Label59.Left -= objStone.cmbIssRecCat.Width
            objStone.Label60.Left -= objStone.cmbIssRecCat.Width
            objStone.Label61.Left -= objStone.cmbIssRecCat.Width
            objStone.Label51.Left -= objStone.cmbIssRecCat.Width
            If ACC_STUDDEDUCT_OPTIONAL = False Then
                objStone.Label26.Left -= objStone.CmbStudDeduct.Width
                objStone.Label57.Left -= objStone.CmbStudDeduct.Width
                objStone.Label58.Left -= objStone.CmbStudDeduct.Width
                objStone.Label59.Left -= objStone.CmbStudDeduct.Width
                objStone.Label60.Left -= objStone.CmbStudDeduct.Width
                objStone.Label61.Left -= objStone.CmbStudDeduct.Width
                objStone.Label51.Left -= objStone.CmbStudDeduct.Width
                objStone.txtStPcs_NUM.Left -= objStone.CmbStudDeduct.Width
                objStone.txtStWeight.Left -= objStone.CmbStudDeduct.Width
                objStone.txtStRate_AMT.Left -= objStone.CmbStudDeduct.Width
                objStone.txtStAmount_AMT.Left -= objStone.CmbStudDeduct.Width
                objStone.cmbStUnit.Left -= objStone.CmbStudDeduct.Width
                objStone.cmbStCalc.Left -= objStone.CmbStudDeduct.Width
                objStone.CmbSeive.Left -= objStone.CmbStudDeduct.Width
            Else
                objStone.CmbStudDeduct.Left -= objStone.cmbIssRecCat.Width
                objStone.lblStud.Left -= objStone.cmbIssRecCat.Width
            End If
            objStone.txtStPcs_NUM.Left -= objStone.cmbIssRecCat.Width
            objStone.txtStWeight.Left -= objStone.cmbIssRecCat.Width
            objStone.txtStRate_AMT.Left -= objStone.cmbIssRecCat.Width
            objStone.txtStAmount_AMT.Left -= objStone.cmbIssRecCat.Width
            objStone.cmbStUnit.Left -= objStone.cmbIssRecCat.Width
            objStone.cmbStCalc.Left -= objStone.cmbIssRecCat.Width
            objStone.CmbSeive.Left -= objStone.cmbIssRecCat.Width
            objStone.Size = New Size(objStone.Size.Width - objStone.cmbIssRecCat.Width - (IIf(ACC_STUDDEDUCT_OPTIONAL, 0, objStone.CmbStudDeduct.Width)), objStone.Size.Height)
            objStone.gridStone.Size = New Size(objStone.gridStone.Size.Width - objStone.cmbIssRecCat.Width - (IIf(ACC_STUDDEDUCT_OPTIONAL, 0, objStone.CmbStudDeduct.Width)), objStone.gridStone.Size.Height)
            objStone.gridStoneTotal.Size = New Size(objStone.gridStoneTotal.Size.Width - objStone.cmbIssRecCat.Width, objStone.gridStoneTotal.Size.Height)
            STONEISSCAT_EDIT = True
        Else
            If STONEISSCAT_EDIT = False Then
                If ACC_STUDDEDUCT_OPTIONAL_EDIT = False And ACC_STUDDEDUCT_OPTIONAL = False Then
                    objStone.Label26.Left -= objStone.CmbStudDeduct.Width
                    objStone.Label57.Left -= objStone.CmbStudDeduct.Width
                    objStone.Label58.Left -= objStone.CmbStudDeduct.Width
                    objStone.Label59.Left -= objStone.CmbStudDeduct.Width
                    objStone.Label60.Left -= objStone.CmbStudDeduct.Width
                    objStone.Label61.Left -= objStone.CmbStudDeduct.Width
                    objStone.Label51.Left -= objStone.CmbStudDeduct.Width
                    objStone.txtStPcs_NUM.Left -= objStone.CmbStudDeduct.Width
                    objStone.txtStWeight.Left -= objStone.CmbStudDeduct.Width
                    objStone.txtStRate_AMT.Left -= objStone.CmbStudDeduct.Width
                    objStone.txtStAmount_AMT.Left -= objStone.CmbStudDeduct.Width
                    objStone.cmbStUnit.Left -= objStone.CmbStudDeduct.Width
                    objStone.cmbStCalc.Left -= objStone.CmbStudDeduct.Width
                    objStone.CmbSeive.Left -= objStone.CmbStudDeduct.Width
                    ACC_STUDDEDUCT_OPTIONAL_EDIT = True
                    objStone.Size = New Size(objStone.Size.Width - objStone.CmbStudDeduct.Width, objStone.Size.Height)
                    objStone.gridStone.Size = New Size(objStone.gridStone.Size.Width - objStone.CmbStudDeduct.Width, objStone.gridStone.Size.Height)
                    objStone.gridStoneTotal.Size = New Size(objStone.gridStoneTotal.Size.Width - objStone.CmbStudDeduct.Width, objStone.gridStoneTotal.Size.Height)
                Else
                    objStone.Size = New Size(objStone.Size.Width, objStone.Size.Height)
                    objStone.gridStone.Size = New Size(objStone.gridStone.Size.Width, objStone.gridStone.Size.Height)
                    objStone.gridStoneTotal.Size = New Size(objStone.gridStoneTotal.Size.Width, objStone.gridStoneTotal.Size.Height)
                End If
            Else
                objStone.Size = New Size(objStone.Size.Width, objStone.Size.Height)
                objStone.gridStone.Size = New Size(objStone.gridStone.Size.Width, objStone.gridStone.Size.Height)
                objStone.gridStoneTotal.Size = New Size(objStone.gridStoneTotal.Size.Width, objStone.gridStoneTotal.Size.Height)
            End If
        End If
    End Sub
    Private Sub ShowStoneDia()
        If objStone.Visible Then Exit Sub
        objStone.grsWt = Val(txtOGrsWt_WET.Text)
        objStone._Authorize = True
        objStone.BackColor = Me.BackColor
        objStone.StartPosition = FormStartPosition.CenterScreen
        objStone.MaximizeBox = False
        objStone.grpStone.BackgroundColor = grpContainer.BackgroundColor
        objStone.StyleGridStone(objStone.gridStone)
        objStone.FromFlag = "A"
        'If oMaterial = Material.Receipt Then objStone.txtTranno_OWN.Select() Else objStone.txtStItem.Select()
        'If oMaterial = Material.Receipt Then objStone.txtTranno_OWN.Visible = True
        objStone._Accode = Maccode
        objStone.oMaterial = oMaterial
        objStone.txtTranno_OWN.Select()
        objStone.txtTranno_OWN.Visible = True

        'If oMaterial = Material.Issue Then objStone.lanb.Text = "Recd. Catgeory" Else objStone.lblIsCat.Text = "Issued Catgeory"
        objStone.IssRecCat = ACC_STONEISSCAT
        objStone.IssRecStudWtDedut = ACC_STUDDEDUCT_OPTIONAL
        objStone.MIMRSTONETYPE = oTransactionType.ToString
        stonepropint()
        objStone.ShowDialog()
        Dim stnWt As Double = Val(objStone.gridStoneTotal.Rows(0).Cells("WEIGHT").Value.ToString)
        Dim stnAmt As Double = Val(objStone.gridStoneTotal.Rows(0).Cells("AMOUNT").Value.ToString)
        txtOLessWt_WET.Text = IIf(stnWt <> 0, Format(stnWt, "0.000"), Nothing)
        txtOStudAmt_AMT.Text = IIf(stnAmt <> 0, Format(stnAmt, "0.00"), Nothing)
        Me.SelectNextControl(txtOGrsWt_WET, True, True, True, True)
    End Sub

    Private Sub ShowAlloyDetDia()
        If ObjAlloy.Visible Then Exit Sub
        If txtMAlloy_WET.Focused Then
            ObjAlloy.AlloyTotWt = Val(txtMAlloy_WET.Text)
        Else
            ObjAlloy.AlloyTotWt = Val(txtOalloy_WET.Text)
        End If
        ObjAlloy.BackColor = Me.BackColor
        ObjAlloy.StartPosition = FormStartPosition.CenterScreen
        ObjAlloy.MaximizeBox = False
        ObjAlloy.BackColor = grpContainer.BackgroundColor
        ObjAlloy.txtAlloy_MAN.Select()
        If Val(ObjAlloy.AlloyTotWt) <> 0 Then ObjAlloy.ShowDialog()
        If txtMAlloy_WET.Focused Then
            Me.SelectNextControl(txtMAlloy_WET, True, True, True, True)
        Else
            Me.SelectNextControl(txtOalloy_WET, True, True, True, True)
        End If
    End Sub

    Private Sub SetStud(ByVal sender As Object, ByVal e As EventArgs) Handles _
    cmbOItem.LostFocus, cmbOSubItem.LostFocus
        If cmbOItem.Text <> "" Then
            StrSql = " SELECT STUDDEDSTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbOItem.Text & "'"
            If cmbOSubItem.Text <> "" And cmbOItem.Text <> "" Then
                ''StrSql = " SELECT STUDDEDSTONE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbOSubItem.Text & "' AND ITEMID = " & Val(cmbOItem.SelectedValue.ToString) & ""
                StrSql = " SELECT STUDDEDSTONE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbOSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbOItem.Text & "')"
            End If
            If objGPack.GetSqlValue(StrSql) = "Y" Then
                oStud = True
            Else
                oStud = False
            End If
            If cmbOSubItem.Text = "ALL" Then
                StrSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbOItem.Text & "' AND ISNULL(HALLMARK,'Y') = 'Y'"
            Else
                ''StrSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbOSubItem.Text & "' AND ISNULL(HALLMARK,'Y') = 'Y' AND ITEMID = " & Val(cmbOItem.SelectedValue.ToString) & ""
                StrSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbOSubItem.Text & "' AND ISNULL(HALLMARK,'Y') = 'Y' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbOItem.Text & "')"
            End If
            If objGPack.GetSqlValue(StrSql).Length > 0 Then
                CmbOHallmark.Text = "YES"
            Else
                CmbOHallmark.Text = "NO"
            End If
        End If
    End Sub

#Region "Ornament Methods & Events"

    Private Sub cmbOCategory_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOCategory.LostFocus
        If (UCase(oTransactionType) = "PURCHASE RETURN" Or UCase(oTransactionType) = "PURCHASE" Or UCase(oTransactionType) = "PURCHASE[APPROVAL]") And cmbOIssuedCategory.Enabled Then
            cmbOIssuedCategory.Text = cmbOCategory.Text
        End If
        If editflag = True Then
            cmbOCategory_SelectedValueChanged(Me, New EventArgs)
            'cmbOIssuedCategory.Text = cmbOCategory.Text
        Else
            If UCase(oTransactionType) <> "PURCHASE[APPROVAL]" Then cmbOCategory_SelectedValueChanged(Me, New EventArgs)
        End If
    End Sub


    Private Sub cmbOCategory_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOCategory.EnabledChanged
        If cmbOCategory.Enabled = False Then
            cmbOItem.Text = ""
            cmbOSubItem.Text = ""
        End If
        cmbOItem.Enabled = cmbOCategory.Enabled
        cmbOSubItem.Enabled = cmbOCategory.Enabled
    End Sub

    Public Sub cmbOMetal_SelectedValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbOMetal.SelectedValueChanged
        If rbtOrnament.Checked Then LoadMetalDetails(cmbOMetal)
        loadJobDetails()
    End Sub

    Public Sub cmbOCategory_SelectedValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbOCategory.SelectedValueChanged
        If txtTagNo.Text <> "" And cmbOItem.Text <> "" Then Exit Sub
        If MRMI_LOCK_ITEMSUBITEM Then
            cmbOItem.Enabled = False
            cmbOSubItem.Enabled = False
            Exit Sub
        End If
        If editflag = False Then LoadCategoryDetails(cmbOCategory)
        If MI_STOCKCHECK Then
            LoadBalanceStock(cmbOCategory.Text)
        End If
    End Sub
    Private Sub LoadBalanceStock(ByVal CatName As String)
        If CatName <> "" Then
            Dim DtGrid As New DataSet()
            StrSql = "EXEC " & cnAdminDb & "..SP_RPT_CATEGORYBALANCE"
            StrSql += vbCrLf + " @DBNAME='" & cnStockDb & "'"
            StrSql += vbCrLf + ",@TODATE='" & oTranDate & "'"
            StrSql += vbCrLf + ",@CATCODE='" & objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME='" & CatName & "'", "CATCODE", "") & "'"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.CommandTimeout = 1000
            Da = New OleDbDataAdapter(Cmd)
            Da.Fill(DtGrid)
            Dim dt As New DataTable
            dt = DtGrid.Tables(0)
            If dt.Rows.Count > 0 Then
                If Val(dt.Rows(0).Item("GRS").ToString) <> 0 Then
                    StkGrsWt = Val(dt.Rows(0).Item("GRS").ToString)
                End If
            End If
        End If
    End Sub
    Private Sub LoadBalanceWt()
        lblBalance.Text = ""
        If Not MRMI_CATBAL_DISPLAY Then Exit Sub
        If cmbMCategory.Text <> "" Then
            Dim DtGrid As New DataSet()
            StrSql = "EXEC " & cnAdminDb & "..SP_RPT_CATEGORYBALANCE"
            StrSql += vbCrLf + " @DBNAME='" & cnStockDb & "'"
            StrSql += vbCrLf + ",@TODATE='" & oTranDate & "'"
            StrSql += vbCrLf + ",@CATCODE='" & objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME='" & cmbMCategory.Text & "'", "CATCODE", "") & "'"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.CommandTimeout = 1000
            Da = New OleDbDataAdapter(Cmd)
            Da.Fill(DtGrid)
            Dim dt As New DataTable
            dt = DtGrid.Tables(0)
            If dt.Rows.Count > 0 Then
                If Val(dt.Rows(0).Item("GRS").ToString) <> 0 Then lblBalance.Text = Val(dt.Rows(0).Item("GRS").ToString) & " Wt"
            End If
        End If
    End Sub

    Public Sub cmbOItem_SelectedValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbOItem.SelectedValueChanged
        If txtTagNo.Text = "" And cmbOItem.Text = "" Then Exit Sub
        LoadItemDetail(cmbOItem)
    End Sub


    Private Sub cmbOItem_EnabledChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _
    cmbOItem.EnabledChanged
        cmbOSubItem.Text = ""
        If cmbOItem.Enabled = False Then
            cmbOSubItem.Enabled = False
        End If
    End Sub

    Private Sub CalcORate()
        Dim Rate As Decimal = Nothing
        If cmbOCategory.SelectedValue IsNot Nothing Then
            If UCase(oTransactionType) = "PURCHASE RETURN" Or UCase(oTransactionType) = "PURCHASE" Or UCase(oTransactionType) = "PURCHASE[APPROVAL]" Then
                Rate = Val(GetRate(oTranDate, objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & cmbOCategory.SelectedValue.ToString & "'")))
            End If
        End If
        txtORate_OWN.Text = IIf(Rate <> 0, Format(Rate, "0.00"), Nothing)
    End Sub

    Private Sub txtONetWt_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtONetWt_WET.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtOWastPer_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOWastPer.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtOWastPer.Text) = 0 Then
                txtOWast_WET.Clear()
                Exit Sub
            End If
            CalcOWastage()
        End If
    End Sub

    Private Sub txtOWastPer_PER_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOWastPer.TextChanged
        If Val(txtOWastPer.Text) = 0 Then
            txtOWast_WET.Clear()
            Exit Sub
        End If

        CalcOWastage()
    End Sub

    Private Sub txtOWast_WET_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOWast_WET.KeyDown
        If Val(txtOWastPer.Text) <> 0 Then e.Handled = True
    End Sub

    Private Sub txtOWast_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOWast_WET.KeyPress
        If Val(txtOWastPer.Text) <> 0 Then e.Handled = True
        If e.KeyChar = Chr(Keys.Enter) Then
            'If ALLOY_DETAILS Then
            '    If lblOWast.Text = "Alloy" Then
            '        ShowAlloyDetDia()
            '    Else
            '        SendKeys.Send("{TAB}")
            '    End If
            'Else
            SendKeys.Send("{TAB}")
            'End If
        End If
    End Sub
    Private Sub txtOalloy_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOalloy_WET.KeyPress
        If Val(txtOalloyper.Text) <> 0 Then e.Handled = True
        If e.KeyChar = Chr(Keys.Enter) Then
            If ALLOY_DETAILS Then
                ShowAlloyDetDia()
            Else
                'SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub txtOMcGrm_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOMcGrm_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If Val(txtOMcGrm_AMT.Text) = 0 Then
                txtOMc_AMT.Clear()
                Exit Sub
            End If
            CalcOMc()
        End If
    End Sub
    Private Sub txtOMcGrm_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOMcGrm_AMT.TextChanged
        If Val(txtOMcGrm_AMT.Text) = 0 Then
            txtOMc_AMT.Clear()
            Exit Sub
        End If
        CalcOMc()
    End Sub

    Private Sub txtMMcGrm_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMMcGrm_AMT.TextChanged
        If Val(txtMMcGrm_AMT.Text) = 0 Then
            txtMMc_AMT.Clear()
            Exit Sub
        End If
        CalcMMc()
    End Sub

    Private Sub txtMMcInfo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMMc_AMT.GotFocus, txtMMcGrm_AMT.GotFocus
        If oMaterial = Material.Receipt Then
            If oTransactionType = "RECEIPT" Then
                Exit Sub
            End If
            If oTransactionType = "OTHER RECEIPT" Then
                Exit Sub
            End If
        End If
        'If lblMWast.Text = "Alloy" Then
        '    txtMWast_WET.Text = IIf(Val(txtMWast_WET.Text) <> 0, Format(Val(txtMWast_WET.Text), FormatNumberStyle(AlloyRnd)), Nothing)
        'End If
        If txtMAlloy_WET.Enabled = True Then
            txtMAlloy_WET.Text = IIf(Val(txtMAlloy_WET.Text) <> 0, Format(Val(txtMAlloy_WET.Text), FormatNumberStyle(AlloyRnd)), Nothing)
        End If
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtMMc_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMMc_AMT.KeyDown
        If Val(txtMMcGrm_AMT.Text) <> 0 Then e.Handled = True
    End Sub


    Private Sub txtOMc_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOMc_AMT.KeyDown
        If Val(txtOMcGrm_AMT.Text) <> 0 Then e.Handled = True
    End Sub

    Private Sub txtOMc_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOMc_AMT.KeyPress
        If Val(txtOMcGrm_AMT.Text) <> 0 Then e.Handled = True
    End Sub

    Private Sub txttdsPer_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOVatPer_PER.KeyPress, txtMVatPer_PER.KeyPress, txtSVatPer_PER.KeyPress
        If e.KeyChar = Chr(Keys.Enter) And (lblOVatPer.Text.Contains("Tds") Or lblMVatPer.Text.Contains("Tds") Or lblSVatPer.Text.Contains("Tds")) Then
            'If lblOVatPer.Text = "S Tax%" Or lblMVatPer.Text = "S Tax%" Then
            '    Exit Sub
            'End If
            Dim TTdscatid As Integer = Val(objGPack.GetSqlValue("SELECT ISNULL(TDSCATID,0) FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE='" & Maccode & "'"))
            Dim TdsCatId As Integer = Val(GetAdmindbSoftValue("TDS_CAT_MC", 1))
            If TTdscatid = 0 And TdsCatId = 0 Then MsgBox("TDS Category dosn't map for the selected party.." & vbCrLf & vbCrLf & "Please configure to party or Set value for TDS_CAT_MC Control", MsgBoxStyle.OkOnly) : SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtOVatPer_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOVatPer_PER.TextChanged
        If Val(txtOVatPer_PER.Text) = 0 Then
            txtOVat_AMT.Clear()
            Exit Sub
        End If
        CalcOVatTds()
    End Sub

    Private Sub txtOVat_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOVat_AMT.KeyDown
        If Val(txtOVatPer_PER.Text) <> 0 And Not PURVATEDITACC Then e.Handled = True
    End Sub

    Private Sub txtOVat_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOVat_AMT.KeyPress
        If Val(txtOVatPer_PER.Text) <> 0 And Not PURVATEDITACC Then e.Handled = True
    End Sub

    Private Sub txtOVat_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOVat_AMT.TextChanged
        CalcONetAmt()
    End Sub

    Private Sub CalcONetWt()
        Dim netWt As Decimal = Val(txtOGrsWt_WET.Text) - Val(txtOLessWt_WET.Text)
        txtONetWt_WET.Text = IIf(netWt <> 0, Format(netWt, "0.000"), "")
    End Sub

    Private Sub CalcMNetWt()
        Dim netWt As Decimal = Val(txtMGrsWt_WET.Text) - Val(txtMLessWt_WET.Text)
        txtMNetWt_WET.Text = IIf(netWt <> 0, Format(netWt, "0.000"), "")
    End Sub

    Private Sub CalcOWastage()
        If Val(txtOWastPer.Text) = 0 Then Exit Sub
        Dim was As Decimal = Nothing
        was = IIf(cmbOGrsNet.Text = "GRS WT", Val(txtOGrsWt_WET.Text), Val(txtONetWt_WET.Text)) * (Val(txtOWastPer.Text) / 100)
        If WASTPERPC = "Y" Then was = Val(txtOPcs_NUM.Text) * Val(txtOWastPer.Text)
        txtOWast_WET.Text = IIf(was <> 0, Format(was, "0.000"), Nothing)
    End Sub

    Private Sub CalcOAlloy()
        If Val(txtOalloyper.Text) = 0 Then Exit Sub
        Dim was As Decimal = Nothing
        was = IIf(cmbOGrsNet.Text = "GRS WT", Val(txtOGrsWt_WET.Text), Val(txtONetWt_WET.Text)) * (Val(txtOalloyper.Text) / 100)
        txtOalloy_WET.Text = IIf(was <> 0, Format(was, FormatNumberStyle(AlloyRnd)), Nothing)
    End Sub

    Private Sub CalcOMc()
        Dim mc As Decimal = Nothing
        If Val(txtOMcGrm_AMT.Text) = 0 Then Exit Sub
        If IIf(cmbOGrsNet.Text = "GRS WT", Val(txtOGrsWt_WET.Text), Val(txtONetWt_WET.Text)) > 0 Then 'If Val(txtOGrsAmt_AMT.Text) > 0 Then
            mc = IIf(cmbOGrsNet.Text = "GRS WT", Val(txtOGrsWt_WET.Text), Val(txtONetWt_WET.Text)) * Val(txtOMcGrm_AMT.Text)
        Else
            mc = Val(txtOPcs_NUM.Text) * Val(txtOMcGrm_AMT.Text)
        End If
        If MCPERGMPC = "P" Then
            mc = Val(txtOPcs_NUM.Text) * Val(txtOMcGrm_AMT.Text)
        End If
        mc = RoundOffPisa(mc)
        txtOMc_AMT.Text = IIf(mc <> 0, Format(mc, "0.00"), Nothing)
        CalcOGrossAmt()
    End Sub

    Private Sub Getdealerwmcwtrange()
        Dim avgwt As Double = 0
        If Val(txtOPcs_NUM.Text) <> 0 Then avgwt = IIf(cmbOGrsNet.Text.ToUpper = "GRS WT", Val(txtOGrsWt_WET.Text), Val(txtONetWt_WET.Text)) / Val(txtOPcs_NUM.Text)
        If avgwt <> 0 Then Getdealerwmc(avgwt)
    End Sub

    Function funcCalcOPureWt() As Decimal
        Dim pureWt As Decimal = Nothing
        If Val(PURWTPERACC.ToString) = 0 Then PURWTPERACC = 100
        If MR_INCWASTINPURECAlC = False And oMaterial = Material.Receipt And (UCase(oTransactionType) = "PURCHASE" Or UCase(oTransactionType) = "RECEIPT") Then
            If Val(txtOTouchAMT.Text) <> 0 Then
                pureWt = (IIf(cmbOGrsNet.Text.ToUpper = "GRS WT", Val(txtOGrsWt_WET.Text), Val(txtONetWt_WET.Text))) * (Val(txtOTouchAMT.Text) / PURWTPERACC)
            Else
                pureWt = (IIf(cmbOGrsNet.Text.ToUpper = "GRS WT", Val(txtOGrsWt_WET.Text), Val(txtONetWt_WET.Text))) * (Val(CmbOPurity.Text) / PURWTPERACC)
            End If
        Else
            If Val(txtOTouchAMT.Text) <> 0 Then
                pureWt = (IIf(cmbOGrsNet.Text.ToUpper = "GRS WT", Val(txtOGrsWt_WET.Text), Val(txtONetWt_WET.Text)) + Val(txtOWast_WET.Text)) * (Val(txtOTouchAMT.Text) / PURWTPERACC)
            Else
                pureWt = (IIf(cmbOGrsNet.Text.ToUpper = "GRS WT", Val(txtOGrsWt_WET.Text), Val(txtONetWt_WET.Text)) + Val(txtOWast_WET.Text)) * (Val(CmbOPurity.Text) / PURWTPERACC)
            End If
        End If

        Dim Round_G As Integer = ROUNDOFF_WT(0)
        Dim Round_D As Integer = ROUNDOFF_WT(1)
        Dim Round_S As Integer = ROUNDOFF_WT(2)
        Dim Round_P As Integer = ROUNDOFF_WT(3)
        Dim Str() As String = pureWt.ToString.Split(".")
        If cmbOMetal.Text = "GOLD" Then
            If ROUNDOFF_PWT = "L" Then
                If Str.Length > 1 Then pureWt = Str(0) & "." & Mid(Str(1), 1, 2) & "0"
                Return (IIf(pureWt <> 0, Format(Math.Round(pureWt, Round_G), "0.00"), Nothing))
            ElseIf ROUNDOFF_PWT = "H" Then
                If Str.Length > 1 Then pureWt = Str(0) & "." & Mid(Str(1), 1, 2) & "9"
                Return (IIf(pureWt <> 0, Format(Math.Round(pureWt, Round_G), "0.000"), Nothing))
            Else
                Return (IIf(pureWt <> 0, Format(Math.Round(pureWt, Round_G), "0.000"), Nothing))
            End If
        ElseIf cmbOMetal.Text = "DIAMOND" Then
            Return (IIf(pureWt <> 0, Format(Math.Round(pureWt, Round_D), "0.000"), Nothing))
        ElseIf cmbOMetal.Text = "SILVER" Then
            Return (IIf(pureWt <> 0, Format(Math.Round(pureWt, Round_S), "0.000"), Nothing))
        ElseIf cmbOMetal.Text = "PLATINUM" Then
            Return (IIf(pureWt <> 0, Format(Math.Round(pureWt, Round_P), "0.000"), Nothing))
        Else
            Return (IIf(pureWt <> 0, Format(pureWt, "0.000"), Nothing))
        End If
    End Function
    Private Sub CalcOPureWt()
        Dim pureWt As Decimal = Nothing
        If Val(PURWTPERACC.ToString) = 0 Then PURWTPERACC = 100
        If MR_INCWASTINPURECAlC = False And oMaterial = Material.Receipt And (UCase(oTransactionType) = "PURCHASE" Or UCase(oTransactionType) = "RECEIPT") Then
            If Val(txtOTouchAMT.Text) <> 0 Then
                pureWt = (IIf(cmbOGrsNet.Text.ToUpper = "GRS WT", Val(txtOGrsWt_WET.Text), Val(txtONetWt_WET.Text))) * (Val(txtOTouchAMT.Text) / PURWTPERACC)
            Else
                pureWt = (IIf(cmbOGrsNet.Text.ToUpper = "GRS WT", Val(txtOGrsWt_WET.Text), Val(txtONetWt_WET.Text))) * (Val(CmbOPurity.Text) / PURWTPERACC)
            End If
        Else
            If Val(txtOTouchAMT.Text) <> 0 Then
                pureWt = (IIf(cmbOGrsNet.Text.ToUpper = "GRS WT", Val(txtOGrsWt_WET.Text), Val(txtONetWt_WET.Text)) + Val(txtOWast_WET.Text)) * (Val(txtOTouchAMT.Text) / PURWTPERACC)
            Else
                pureWt = (IIf(cmbOGrsNet.Text.ToUpper = "GRS WT", Val(txtOGrsWt_WET.Text), Val(txtONetWt_WET.Text)) + Val(txtOWast_WET.Text)) * (Val(CmbOPurity.Text) / PURWTPERACC)
            End If
        End If

        Dim Round_G As Integer = ROUNDOFF_WT(0)
        Dim Round_D As Integer = ROUNDOFF_WT(1)
        Dim Round_S As Integer = ROUNDOFF_WT(2)
        Dim Round_P As Integer = ROUNDOFF_WT(3)
        Dim str() As String = pureWt.ToString.Split(".")
        If cmbOMetal.Text = "GOLD" Then
            If ROUNDOFF_PWT = "L" Then
                If str.Length > 1 Then pureWt = str(0) & "." & Mid(str(1), 1, 2) & "0"
                txtOPureWt_WET.Text = IIf(pureWt <> 0, Format(Math.Round(pureWt, Round_G), "0.000"), Nothing)
            ElseIf ROUNDOFF_PWT = "H" Then
                If str.Length > 1 Then pureWt = str(0) & "." & Mid(str(1), 1, 2) & "9"
                txtOPureWt_WET.Text = IIf(pureWt <> 0, Format(Math.Round(pureWt, Round_G), "0.000"), Nothing)
            Else
                txtOPureWt_WET.Text = IIf(pureWt <> 0, Format(Math.Round(pureWt, Round_G), "0.000"), Nothing)
            End If
        ElseIf cmbOMetal.Text = "DIAMOND" Then
            txtOPureWt_WET.Text = IIf(pureWt <> 0, Format(Math.Round(pureWt, Round_D), "0.000"), Nothing)
        ElseIf cmbOMetal.Text = "SILVER" Then
            txtOPureWt_WET.Text = IIf(pureWt <> 0, Format(Math.Round(pureWt, Round_S), "0.000"), Nothing)
        ElseIf cmbOMetal.Text = "PLATINUM" Then
            txtOPureWt_WET.Text = IIf(pureWt <> 0, Format(Math.Round(pureWt, Round_P), "0.000"), Nothing)
        Else
            txtOPureWt_WET.Text = IIf(pureWt <> 0, Format(pureWt, "0.000"), Nothing)
        End If
    End Sub


    Public Sub CalcOGrossAmt()
        Dim GrsAmt As Decimal = Nothing
        If Val(txtOGrsWt_WET.Text) <> 0 Then
            If MR_INCWASTINPURECAlC = False And oMaterial = Material.Receipt And (UCase(oTransactionType) = "PURCHASE" Or UCase(oTransactionType) = "RECEIPT") Then
                GrsAmt = (IIf(cmbOGrsNet.Text = "GRS WT", Val(txtOGrsWt_WET.Text), Val(txtONetWt_WET.Text)) _
             + (-1 * Val(txtOalloy_WET.Text))) _
            * Val(txtORate_OWN.Text)
            Else
                GrsAmt = (IIf(cmbOGrsNet.Text = "GRS WT", Val(txtOGrsWt_WET.Text), Val(txtONetWt_WET.Text)) _
            + Val(txtOWast_WET.Text) + (-1 * Val(txtOalloy_WET.Text))) _
            * Val(txtORate_OWN.Text)
            End If


            If cmbOcalcon.Text = "GRS WT" And cmbOcalcon.Visible = True Then
                GrsAmt = Val(txtOGrsWt_WET.Text) * Val(txtORate_OWN.Text)
            ElseIf cmbOcalcon.Text = "NET WT" And cmbOcalcon.Visible = True Then
                GrsAmt = Val(txtONetWt_WET.Text) * Val(txtORate_OWN.Text)
            ElseIf cmbOcalcon.Text = "PURE WT" And cmbOcalcon.Visible = True Then
                GrsAmt = Val(txtOPureWt_WET.Text) * Val(txtORate_OWN.Text)
            Else
                If Val(txtOTouchAMT.Text) <> 0 Then
                    GrsAmt = Val(txtOPureWt_WET.Text) * Val(txtORate_OWN.Text)
                End If
            End If
        Else
            GrsAmt = Val(txtOPcs_NUM.Text) * Val(txtORate_OWN.Text)
        End If
        GrsAmt = GrsAmt + Val(txtOMc_AMT.Text) + Val(txtOStudAmt_AMT.Text) + Val(txtOAddlCharge_AMT.Text) '+ Val(txtOED_AMT.Text)
        GrsAmt = GrsAmt - Val(txtODisc.Text)
        GrsAmt = RoundOffPisa(GrsAmt)
        txtOGrsAmt_AMT.Text = IIf(GrsAmt <> 0, Format(GrsAmt, "0.00"), Nothing)
        CalcOVatTds()
        CalcOGST()
        CalcONetAmt()
    End Sub
    Private Sub CalcOGST()
        If oTransactionType = "RECEIPT" Then
            Dim Gst As Decimal = Nothing
            Gst = (Val(txtOGrsAmt_AMT.Text) + Val(txtOED_AMT.Text)) * (Val(txtOSgst_AMT.Text) / 100)
            Gst = RoundOffPisa(Gst)
            txtOSG_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.00"), Nothing)

            Gst = (Val(txtOGrsAmt_AMT.Text) + Val(txtOED_AMT.Text)) * (Val(txtOCgst_AMT.Text) / 100)
            Gst = RoundOffPisa(Gst)
            txtOCG_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.00"), Nothing)

            Gst = (Val(txtOGrsAmt_AMT.Text) + Val(txtOED_AMT.Text)) * (Val(txtOIgst_AMT.Text) / 100)
            Gst = RoundOffPisa(Gst)
            txtOIG_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.00"), Nothing)
            If lblOVat.Text.ToUpper <> "TDS" Then
                Gst = Val(txtOSG_AMT.Text) + Val(txtOCG_AMT.Text) + Val(txtOIG_AMT.Text)
                txtOVat_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.00"), Nothing)
            End If
        Else
            Dim Gst As Decimal = Nothing
            Gst = (Val(txtOGrsAmt_AMT.Text) + Val(txtOED_AMT.Text)) * (Val(txtOSgst_AMT.Text) / 100)
            Gst = RoundOffPisa(Gst)
            txtOSG_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.00"), Nothing)

            Gst = (Val(txtOGrsAmt_AMT.Text) + Val(txtOED_AMT.Text)) * (Val(txtOCgst_AMT.Text) / 100)
            Gst = RoundOffPisa(Gst)
            txtOCG_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.00"), Nothing)

            Gst = (Val(txtOGrsAmt_AMT.Text) + Val(txtOED_AMT.Text)) * (Val(txtOIgst_AMT.Text) / 100)
            Gst = RoundOffPisa(Gst)
            txtOIG_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.00"), Nothing)
            If lblOVat.Text.ToUpper <> "TDS" Then
                Gst = Val(txtOSG_AMT.Text) + Val(txtOCG_AMT.Text) + Val(txtOIG_AMT.Text)
                txtOVat_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.00"), Nothing)
            End If
        End If

    End Sub
    Private Sub CalcOVatTds()
        Dim vatTds As Decimal = Nothing
        vatTds = (Val(txtOGrsAmt_AMT.Text) + Val(txtOED_AMT.Text)) * (Val(txtOVatPer_PER.Text) / 100)
        vatTds = IIf(lblOVat.Text.ToUpper = "TDS", RoundOffPisa(vatTds, True), RoundOffPisa(vatTds))
        txtOVat_AMT.Text = IIf(vatTds <> 0, Format(vatTds, "0.00"), Nothing)
        CalcONetAmt()
    End Sub

    Public Sub CalcONetAmt()
        Dim netAmt As Decimal = Nothing
        Dim vatAmt As Decimal = Val(txtOVat_AMT.Text)
        'If oTransactionType = "PURCHASE RETURN" Then vatAmt = vatAmt * -1
        netAmt = Val(txtOGrsAmt_AMT.Text) + Val(txtOED_AMT.Text) + IIf(lblOVat.Text.ToUpper = "TDS", -1 * vatAmt, vatAmt)
        If lblOVat.Text.ToUpper = "TDS" Then
            netAmt += Val(txtOSG_AMT.Text) + Val(txtOCG_AMT.Text) + Val(txtOIG_AMT.Text)
        End If
        netAmt += Val(txtOTCS_AMT.Text.ToString)
        netAmt = RoundOffPisa(netAmt)
        txtOAmount_AMT.Text = IIf(netAmt <> 0, Format(netAmt, "0.00"), Nothing)
    End Sub
    Private Sub txtOLessWt_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOLessWt_WET.GotFocus
        If LOCKLESSWT_ACC Then
            If oStud Then
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub txtOGrsWt_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOGrsWt_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If oStud Then
                If STOCKVALIDATION And oMaterial = Material.Issue Then
                    If objStone.dtGridStone.Rows.Count > 0 Then
                        ShowStoneDia()
                    Else
                        SendKeys.Send("{TAB}")
                    End If
                    If ORADDITIONALDETAIL = True Then
                        ShowOrderAdditionalDetails()
                    End If
                ElseIf STOCKVALIDATION_MR And oMaterial = Material.Receipt Then
                    If objStone.dtGridStone.Rows.Count > 0 Then
                        ShowStoneDia()
                    Else
                        SendKeys.Send("{TAB}")
                    End If
                    If ORADDITIONALDETAIL = True Then
                        ShowOrderAdditionalDetails()
                    End If
                Else
                    ShowStoneDia()
                    If ORADDITIONALDETAIL = True Then
                        ShowOrderAdditionalDetails()
                    End If
                End If
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub txtOGrsWt_WET_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOGrsWt_WET.LostFocus
        Getdealerwmcwtrange()
    End Sub


    Private Sub CalcONetWt(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
    txtOLessWt_WET.TextChanged _
    , txtOGrsWt_WET.TextChanged _
    , cmbOGrsNet.SelectedIndexChanged
        CalcONetWt()
        CalcOWastage()
        CalcOMc()
        CalcOPureWt()
        CalcOGrossAmt()
    End Sub

    Private Sub ShowOrderAdditionalDetails()
        If objAddtionalDetails.Visible Then Exit Sub
        objAddtionalDetails.StartPosition = FormStartPosition.CenterScreen
        objAddtionalDetails.MaximizeBox = False
        objAddtionalDetails.cmbType.Select()
        objAddtionalDetails.ShowDialog()
        Me.SelectNextControl(txtOGrsWt_WET, True, True, True, True)
    End Sub

    Private Sub txtOStudAmt_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOStudAmt_AMT.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub txtOPureWt_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOPureWt_WET.GotFocus
        'SendKeys.Send("{TAB}")
    End Sub
    Private Sub txtOPureWt_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOPureWt_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim APureWt As Decimal = funcCalcOPureWt()
            Dim DPureWt As Decimal
            If Val(txtOPureWt_WET.Text) <> APureWt Then
                If ROUNDOFF_PWT = "" Then
                    DPureWt = Math.Abs(Val(txtOPureWt_WET.Text) - APureWt)
                    If DPureWt > 0.5 Then
                        txtOPureWt_WET.Text = funcCalcOPureWt()
                    End If
                    If DPureWt < -0.5 Then
                        txtOPureWt_WET.Text = funcCalcOPureWt()
                    End If
                Else
                    txtOPureWt_WET.Text = APureWt
                End If
            End If
        End If
    End Sub

    Private Sub txtORate_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtORate_OWN.GotFocus
        txtORate_OWN.Text = IIf(Val(txtORate_OWN.Text) <> 0, Format(Val(txtORate_OWN.Text), FormatNumberStyle(RateRnd)), Nothing)
        If oRateLock Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtORate_OWN_KeyDown(sender As Object, e As KeyEventArgs) Handles txtORate_OWN.KeyDown, txtMRate_OWN.KeyDown
        If e.KeyCode = Keys.F6 Then
            Dim temptouchrate As Double = 0
            If Val(txtORate_OWN.Text) <> 0 And Val(txtOTouchAMT.Text) <> 0 Then
                temptouchrate = Val(txtORate_OWN.Text) * (Val(txtOTouchAMT.Text) / 100)
                txtORate_OWN.Text = Math.Round(temptouchrate, 2)
            End If
            If Val(txtMRate_OWN.Text) <> 0 And Val(txtMTouchAMT.Text) <> 0 Then
                temptouchrate = Val(txtMRate_OWN.Text) * (Val(txtMTouchAMT.Text) / 100)
                txtMRate_OWN.Text = Math.Round(temptouchrate, 2)
            End If
        End If
    End Sub
    Private Sub txtORate_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtORate_OWN.KeyPress _
    , txtMRate_OWN.KeyPress _
    , txtSRate_OWN.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then Exit Sub
        If e.KeyChar = "." And sender.Text.Contains(".") Then
            e.Handled = True
            Exit Sub
        End If
        Select Case e.KeyChar
            Case "+", "-", "0" To "9", ChrW(Keys.Back), ".",
            ChrW(Keys.Enter), ChrW(Keys.Escape)
            Case Else
                e.Handled = True
                MsgBox("Digits only Allowed 0 to 9", MsgBoxStyle.Information)
                sender.Focus()
        End Select
        If CType(sender, TextBox).Text.Contains(".") Then
            Dim dotPos As Integer = InStr(CType(sender, TextBox).Text, ".", CompareMethod.Text)
            Dim sp() As String = CType(sender, TextBox).Text.Split(".")
            Dim curPos As Integer = CType(sender, TextBox).SelectionStart
            If sp.Length >= 2 Then
                If curPos >= dotPos Then
                    If sp(1).Length > (RateRnd - 1) Then
                        e.Handled = True
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub txtONetWt_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtONetWt_WET.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Getdealerwmcwtrange()
        End If
    End Sub

    Private Sub CalcOGrossAmt(ByVal sender As Object, ByVal e As EventArgs) Handles _
    txtOGrsWt_WET.TextChanged _
    , txtONetWt_WET.TextChanged _
    , txtOWast_WET.TextChanged _
    , txtOMc_AMT.TextChanged _
    , txtORate_OWN.TextChanged _
    , txtOStudAmt_AMT.TextChanged _
        , txtOTCS_AMT.TextChanged
        CalcOPureWt()
        If txtOGrsAmt_AMT.Focused = False Then
            CalcOGrossAmt()
            'CalcONGrossAmt()
        End If
    End Sub


    Private Sub txtOTouch_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOTouchAMT.GotFocus
        'If lblOWast.Text = "Alloy" Then
        '    txtOWast_WET.Text = IIf(Val(txtOWast_WET.Text) <> 0, Format(Val(txtOWast_WET.Text), FormatNumberStyle(AlloyRnd)), Nothing)
        'End If       

        If oMaterial = Material.Receipt And MRMI_PreTouch = True Then
            Dim _ItemId As String = "" : Dim _SubItemId As String = ""
            If cmbOItem.Text <> "" Then
                StrSql = "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbOItem.Text.ToString.Trim & "' "
                _ItemId = objGPack.GetSqlValue(StrSql, , "")
            End If
            If cmbOSubItem.Text <> "" And cmbOSubItem.Text <> "ALL" Then
                StrSql = "SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE  ITEMID=" & _ItemId & " AND SUBITEMNAME='" & cmbOSubItem.Text.ToString.Trim & "' "
                _SubItemId = objGPack.GetSqlValue(StrSql, , "")
            End If
            StrSql = vbCrLf + "SELECT "
            StrSql += vbCrLf + "TOP 1 TOUCH "
            StrSql += vbCrLf + "FROM " & cnStockDb & "..RECEIPT R"
            StrSql += vbCrLf + "WHERE 1=1  "
            StrSql += vbCrLf + "AND TRANTYPE IN('RRE','RPU')  "
            StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
            If _ItemId <> "" Then StrSql += vbCrLf + "AND ITEMID=" & _ItemId & ""
            If _SubItemId <> "" Then StrSql += vbCrLf + "AND SUBITEMID=" & _SubItemId & ""
            If CmbOPurity.Text <> "" Then StrSql += vbCrLf + "AND PURITY='" & CmbOPurity.Text.ToString & "'"
            StrSql += vbCrLf + "ORDER BY TRANDATE DESC,TRANNO DESC"
            txtLastTouch.Text = objGPack.GetSqlValue(StrSql, , "")
        End If

        If txtOalloy_WET.Visible = True Then
            txtOalloy_WET.Text = IIf(Val(txtOalloy_WET.Text) <> 0, Format(Val(txtOalloy_WET.Text), FormatNumberStyle(AlloyRnd)), Nothing)
        End If
    End Sub

    Private Sub txtOTouch_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOTouchAMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then

            If oMaterial = Material.Receipt And MRMI_PreTouch = True And Val(txtOTouchAMT.Text.ToString) > 0 Then
                Dim _ItemId As String = "" : Dim _SubItemId As String = ""
                If cmbOItem.Text <> "" Then
                    StrSql = "SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbOItem.Text.ToString.Trim & "' "
                    _ItemId = objGPack.GetSqlValue(StrSql, , "")
                End If
                If cmbOSubItem.Text <> "" And cmbOSubItem.Text <> "ALL" Then
                    StrSql = "SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID=" & _ItemId & " AND SUBITEMNAME='" & cmbOSubItem.Text.ToString.Trim & "' "
                    _SubItemId = objGPack.GetSqlValue(StrSql, , "")
                End If
                StrSql = vbCrLf + "SELECT "
                StrSql += vbCrLf + "TOP 1 TOUCH "
                StrSql += vbCrLf + "FROM " & cnStockDb & "..RECEIPT R"
                StrSql += vbCrLf + "WHERE 1=1  "
                StrSql += vbCrLf + "AND TRANTYPE IN('RRE','RPU')  "
                StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                If _ItemId <> "" Then StrSql += vbCrLf + "AND ITEMID=" & _ItemId & ""
                If _SubItemId <> "" Then StrSql += vbCrLf + "AND SUBITEMID=" & _SubItemId & ""
                If CmbOPurity.Text <> "" Then StrSql += vbCrLf + "AND PURITY='" & CmbOPurity.Text.ToString & "'"
                StrSql += vbCrLf + "ORDER BY TRANDATE DESC,TRANNO DESC"
                Dim _Touch As String = objGPack.GetSqlValue(StrSql, , "")
                If Val(_Touch) > 0 And Val(txtOTouchAMT.Text.ToString) > Val(_Touch) And txtOOrdNo.Text.ToString = "" Then
                    MsgBox("Current touch greater than previous touch...", MsgBoxStyle.Information)
                    txtOTouchAMT.Text = "" '_Touch.ToString
                    txtOTouchAMT.Focus()
                    Exit Sub
                End If
            End If

            If TOUCHPERVALID = "Y" Then
                If Val(txtOTouchAMT.Text) = 0 Then MsgBox("Enter the Touch percent") : Exit Sub Else SendKeys.Send("{TAB}")
            Else
                If UCase(oTransactionType) = "PURCHASE" Or oTransactionType = "PURCHASE[APPROVAL]" Then txtORate_OWN.Focus() : Exit Sub
                SendKeys.Send("{TAB}")
                Exit Sub
            End If
        End If
    End Sub

    Private Sub txtMTouch_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMTouchAMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If TOUCHPERVALID = "Y" Then
                If Val(txtMTouchAMT.Text) = 0 Then MsgBox("Enter the Touch percent") : Exit Sub Else SendKeys.Send("{TAB}")
            Else
                SendKeys.Send("{TAB}")
                Exit Sub
            End If
        End If

    End Sub

    Private Sub txtOTouch_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOTouchAMT.TextChanged
        CalcOPureWt()
        CalcOGrossAmt()
    End Sub

    Private Sub OVat_GotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles txtOVatPer_PER.GotFocus
        If Val(txtOGrsAmt_AMT.Text) = 0 And PURAMTEDITACC = "N" Then
            SendKeys.Send("{TAB}")
        ElseIf Val(txtOGrsAmt_AMT.Text) > 0 And Val(txtOMc_AMT.Text) > 0 Then
            If (oTransactionType = "RECEIPT" Or oTransactionType = "ISSUE") And GstFlag Then
                McBill = True
                If oTransactionType = "RECEIPT" Then 'VBJ GST SOFT CHANGE
                    Dim splitAccode() As String = Nothing
                    If _accStateId <> CompanyStateId Then
                        StrSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='LABOUR_IGST'"
                        splitAccode = objGPack.GetSqlValue(StrSql, "CTLTEXT", "").Split(":")
                        If splitAccode.Length = 2 Then
                            txtOIgst_AMT.Text = splitAccode(1)
                            txtMIgst_AMT.Text = splitAccode(1)
                            txtSIgst_WET.Text = splitAccode(1)
                            txtOthIgst_AMT.Text = splitAccode(1)
                        Else
                            txtOIgst_AMT.Text = Format(SrVtTax, "0.000")
                            txtMIgst_AMT.Text = Format(SrVtTax, "0.000")
                            txtSIgst_WET.Text = Format(SrVtTax, "0.000")
                            txtOthIgst_AMT.Text = Format(SrVtTax, "0.000")
                        End If

                        'txtOIgst_AMT.Text = Format(SrVtTax, "0.000")
                        'txtMIgst_AMT.Text = Format(SrVtTax, "0.000")
                        'txtSIgst_WET.Text = Format(SrVtTax, "0.000")
                        'txtOthIgst_AMT.Text = Format(SrVtTax, "0.000")
                    Else
                        StrSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='LABOUR_CGST' "
                        splitAccode = objGPack.GetSqlValue(StrSql, "CTLTEXT", "").Split(":")
                        If splitAccode.Length = 2 Then
                            txtOCgst_AMT.Text = splitAccode(1)
                            txtMCgst_AMT.Text = splitAccode(1)
                            txtSCgst_WET.Text = splitAccode(1)
                            txtOthCgst_AMT.Text = splitAccode(1)
                        Else
                            txtOCgst_AMT.Text = Format((SrVtTax / 2), "0.000")
                            txtMCgst_AMT.Text = Format((SrVtTax / 2), "0.000")
                            txtSCgst_WET.Text = Format((SrVtTax / 2), "0.000")
                            txtOthCgst_AMT.Text = Format((SrVtTax / 2), "0.000")
                        End If
                        StrSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='LABOUR_SGST' "
                        splitAccode = objGPack.GetSqlValue(StrSql, "CTLTEXT", "").Split(":")
                        If splitAccode.Length = 2 Then
                            txtOSgst_AMT.Text = splitAccode(1)
                            txtMSgst_AMT.Text = splitAccode(1)
                            txtSSgst_WET.Text = splitAccode(1)
                            txtOthSgst_AMT.Text = splitAccode(1)
                        Else
                            txtOSgst_AMT.Text = Format((SrVtTax / 2), "0.000")
                            txtMSgst_AMT.Text = Format((SrVtTax / 2), "0.000")
                            txtSSgst_WET.Text = Format((SrVtTax / 2), "0.000")
                            txtOthSgst_AMT.Text = Format((SrVtTax / 2), "0.000")
                        End If
                        'txtOCgst_AMT.Text = Format((SrVtTax / 2), "0.000")
                        'txtOSgst_AMT.Text = Format((SrVtTax / 2), "0.000")
                        'txtMCgst_AMT.Text = Format((SrVtTax / 2), "0.000")
                        'txtMSgst_AMT.Text = Format((SrVtTax / 2), "0.000")
                        'txtSCgst_WET.Text = Format((SrVtTax / 2), "0.000")
                        'txtSSgst_WET.Text = Format((SrVtTax / 2), "0.000")
                        'txtOthCgst_AMT.Text = Format((SrVtTax / 2), "0.000")
                        'txtOthSgst_AMT.Text = Format((SrVtTax / 2), "0.000")
                    End If
                Else
                    If _accStateId <> CompanyStateId Then
                        txtOIgst_AMT.Text = Format(SrVtTax, "0.000")
                        txtMIgst_AMT.Text = Format(SrVtTax, "0.000")
                        txtSIgst_WET.Text = Format(SrVtTax, "0.000")
                        txtOthIgst_AMT.Text = Format(SrVtTax, "0.000")
                    Else
                        txtOCgst_AMT.Text = Format((SrVtTax / 2), "0.000")
                        txtOSgst_AMT.Text = Format((SrVtTax / 2), "0.000")
                        txtMCgst_AMT.Text = Format((SrVtTax / 2), "0.000")
                        txtMSgst_AMT.Text = Format((SrVtTax / 2), "0.000")
                        txtSCgst_WET.Text = Format((SrVtTax / 2), "0.000")
                        txtSSgst_WET.Text = Format((SrVtTax / 2), "0.000")
                        txtOthCgst_AMT.Text = Format((SrVtTax / 2), "0.000")
                        txtOthSgst_AMT.Text = Format((SrVtTax / 2), "0.000")
                    End If
                End If

            End If
        End If
    End Sub

    Private Sub txtOVat_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOVat_AMT.GotFocus
        If Val(txtOGrsAmt_AMT.Text) = 0 And PURAMTEDITACC = "N" Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

#End Region

#Region "Stone Methods & Events"

    Private Sub cmbSCategory_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSCategory.LostFocus
        If (UCase(oTransactionType) = "PURCHASE RETURN" Or UCase(oTransactionType) = "PURCHASE" Or UCase(oTransactionType) = "PURCHASE[APPROVAL]") And cmbSIssuedCategory.Enabled Then
            cmbSIssuedCategory.Text = cmbSCategory.Text
        End If
    End Sub

    Private Sub txtSRate_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSRate_OWN.GotFocus
        txtSRate_OWN.Text = IIf(Val(txtSRate_OWN.Text) <> 0, Format(Val(txtSRate_OWN.Text), FormatNumberStyle(RateRnd)), Nothing)
        If oRateLock Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub cmbSItem_EnabledChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _
cmbSItem.EnabledChanged
        cmbSSubItem.Text = ""
        If cmbSItem.Enabled = False Then
            cmbSSubItem.Enabled = False
        End If
    End Sub

    Public Sub cmbSCategory_SelectedValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbSCategory.SelectedValueChanged
        LoadCategoryDetails(cmbSCategory)
    End Sub

    Public Sub cmbSItem_SelectedValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _
cmbSItem.SelectedValueChanged
        LoadItemDetail(cmbSItem)
    End Sub

    Public Sub cmbSMetal_SelectedValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbSMetal.SelectedValueChanged
        If rbtStone.Checked Then LoadMetalDetails(cmbSMetal)
        If Not (STOCKVALIDATION Or STOCKVALIDATION_MR) Then loadJobDetails()
    End Sub

    Private Sub CalcOthRate()
        Dim Rate As Decimal = Nothing
        If cmbOthCategory.SelectedValue IsNot Nothing Then
            If UCase(oTransactionType) = "PURCHASE RETURN" Or UCase(oTransactionType) = "PURCHASE" Or UCase(oTransactionType) = "PURCHASE[APPROVAL]" Then
                Rate = Val(GetRate(oTranDate, objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & cmbOthCategory.SelectedValue.ToString & "'")))
            End If
        End If
        txtOthRate_AMT.Text = IIf(Rate <> 0, Format(Rate, "0.00"), Nothing)
    End Sub


    Private Sub CalcSRate()
        Dim Rate As Decimal = Nothing
        If cmbSCategory.SelectedValue IsNot Nothing Then
            If UCase(oTransactionType) = "PURCHASE RETURN" Or UCase(oTransactionType) = "PURCHASE" Or UCase(oTransactionType) = "PURCHASE[APPROVAL]" Then
                Rate = Val(GetRate(oTranDate, objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & cmbSCategory.SelectedValue.ToString & "'")))
            End If
        End If
        txtSRate_OWN.Text = IIf(Rate <> 0, Format(Rate, "0.00"), Nothing)
    End Sub

    Private Sub cmbSCategory_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSCategory.EnabledChanged
        If cmbSCategory.Enabled = False Then
            cmbSItem.Text = ""
            cmbSSubItem.Text = ""
        End If
        cmbSItem.Enabled = cmbSCategory.Enabled
        cmbSSubItem.Enabled = cmbSCategory.Enabled
    End Sub

    Private Sub CalcSGrossAmt()
        Dim GrsAmt As Decimal = Nothing
        If txtSRate_OWN.Enabled = True Then
            If Val(txtSGrsWt_WET.Text) <> 0 Then
                GrsAmt = IIf(cmbSCalcMode.Text.ToUpper = "WEIGHT", Val(txtSGrsWt_WET.Text), Val(txtSPcs_NUM.Text)) * Val(txtSRate_OWN.Text)
            Else
                GrsAmt = Val(txtSPcs_NUM.Text) * Val(txtSRate_OWN.Text)
            End If
        Else
            If MaterialStoneDia.gridStoneTotal.Rows.Count > 0 Then
                GrsAmt = Val(MaterialStoneDia.gridStoneTotal.Rows(0).Cells("AMOUNT").Value.ToString)
            End If
        End If
        GrsAmt = GrsAmt + Val(txtSAddlCharge_AMT.Text)
        GrsAmt = RoundOffPisa(GrsAmt)
        txtSGrsAmt_AMT.Text = IIf(GrsAmt <> 0, Format(GrsAmt, "0.00"), Nothing)
        CalcSVatTds()
        CalcSGST()
        CalcSNetAmt()
    End Sub

    Private Sub cmbSCalcMode_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSCalcMode.GotFocus
        ExFlag = False
    End Sub

    Private Sub txtSGrsWt_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSGrsWt_WET.GotFocus
        ExFlag = False
    End Sub

    Private Sub CalcSGrossAmt(ByVal sender As Object, ByVal e As EventArgs) Handles _
    txtSGrsWt_WET.TextChanged _
    , txtSRate_OWN.TextChanged _
    , cmbSCalcMode.SelectedIndexChanged
        CalcSGrossAmt()
        CalcSSeive()
    End Sub
    Private Sub CalcSGST()
        Dim Gst As Decimal = Nothing
        Gst = Val(txtSGrsAmt_AMT.Text) * (Val(txtSSgst_WET.Text) / 100)
        Gst = RoundOffPisa(Gst)
        txtSSG_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.000"), Nothing)

        Gst = Val(txtSGrsAmt_AMT.Text) * (Val(txtSCgst_WET.Text) / 100)
        Gst = RoundOffPisa(Gst)
        txtSCG_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.000"), Nothing)

        Gst = Val(txtSGrsAmt_AMT.Text) * (Val(txtSIgst_WET.Text) / 100)
        Gst = RoundOffPisa(Gst)
        txtSIG_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.000"), Nothing)
        If lblSVat.Text.ToUpper <> "TDS" Then
            Gst = Val(txtSSG_AMT.Text) + Val(txtSCG_AMT.Text) + Val(txtSIG_AMT.Text)
            txtSVat_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.000"), Nothing)
        End If
    End Sub
    Private Sub CalcSVatTds()
        Dim vatTds As Decimal = Nothing
        vatTds = Val(txtSGrsAmt_AMT.Text) * (Val(txtSVatPer_PER.Text) / 100)
        vatTds = IIf(lblOVat.Text.ToUpper = "TDS", RoundOffPisa(vatTds, True), RoundOffPisa(vatTds))
        txtSVat_AMT.Text = IIf(vatTds <> 0, Format(vatTds, "0.00"), Nothing)
        CalcSNetAmt()
    End Sub
    Public Sub CalcSNetAmt()
        Dim netAmt As Decimal = Nothing
        Dim vatAmt As Decimal = Val(txtSVat_AMT.Text)
        vatAmt = RoundOffPisa(vatAmt)
        'If oTransactionType = "PURCHASE RETURN" Then vatAmt = vatAmt * -1
        netAmt = Val(txtSGrsAmt_AMT.Text) + IIf(lblSVat.Text.ToUpper = "TDS", -1 * vatAmt, vatAmt)
        If lblSVat.Text.ToUpper = "TDS" Then
            netAmt += Val(txtSSG_AMT.Text) + Val(txtSCG_AMT.Text) + Val(txtSIG_AMT.Text)
        End If
        netAmt += Val(txtSTCS_AMT.Text.ToString)
        txtSAmount_AMT.Text = IIf(netAmt <> 0, Format(netAmt, "0.00"), Nothing)
    End Sub
    Private Sub CalcSSeive()
        If cmbSItem.Text = "" Then Exit Sub
        Dim cent As Double = 0
        If cmbSUnit.Text = "CARAT" Then cent = (Val(txtSGrsWt_WET.Text) / IIf(Val(txtSPcs_NUM.Text) = 0, 1, Val(txtSPcs_NUM.Text))) Else cent = Val(txtSGrsWt_WET.Text) / IIf(Val(txtSPcs_NUM.Text) = 0, 1, Val(txtSPcs_NUM.Text))
        cent *= 100
        StrSql = " DECLARE @CENT FLOAT"
        StrSql += " SET @CENT = " & cent & ""
        StrSql += " SELECT SIZEDESC FROM " & cnAdminDb & "..CENTSIZE WHERE"
        StrSql += " @CENT BETWEEN FROMCENT AND TOCENT "
        StrSql += " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbSItem.Text & "')"
        StrSql += " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbSItem.Text & "')),0)"
        Dim sizedisc As String = ""
        sizedisc = objGPack.GetSqlValue(StrSql, "SIZEDESC", "", tran)
        cmbSSeive.Text = sizedisc
    End Sub
    Private Sub txtSVatPer_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSVatPer_PER.TextChanged
        If Val(txtSVatPer_PER.Text) = 0 Then
            txtSVat_AMT.Clear()
            Exit Sub
        End If
        CalcSVatTds()
    End Sub

    Private Sub txtSVat_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSVat_AMT.KeyDown
        If Val(txtSVatPer_PER.Text) <> 0 Then e.Handled = True
    End Sub

    Private Sub txtSVat_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSVat_AMT.KeyPress
        If Val(txtSVatPer_PER.Text) <> 0 Then e.Handled = True
    End Sub

    Private Sub txtSVat_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSVat_AMT.TextChanged
        CalcSNetAmt()
    End Sub

    Private Sub SVat_GotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles _
txtSVat_AMT.GotFocus, txtSVatPer_PER.GotFocus
        If Val(txtSGrsAmt_AMT.Text) = 0 And PURAMTEDITACC = "N" Then
            SendKeys.Send("{TAB}")
        End If
    End Sub


#End Region

#Region "Metal Methods & Events"

    Private Sub txtMRate_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMRate_OWN.GotFocus
        txtMRate_OWN.Text = IIf(Val(txtMRate_OWN.Text) <> 0, Format(Val(txtMRate_OWN.Text), FormatNumberStyle(RateRnd)), Nothing)
        If oRateLock Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Public Sub cmbMMetal_SelectedValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbMMetal.SelectedValueChanged
        If rbtMetal.Checked Then LoadMetalDetails(cmbMMetal)
        loadJobDetails()
    End Sub

    Private Sub cmbMCategory_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMCategory.LostFocus
        If (UCase(oTransactionType) = "PURCHASE RETURN" Or UCase(oTransactionType) = "PURCHASE" Or UCase(oTransactionType) = "PURCHASE[APPROVAL]") And cmbMIssuedCategory.Enabled Then
            cmbMIssuedCategory.Text = cmbMCategory.Text
        End If
    End Sub

    Public Sub cmbMCategory_SelectedValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbMCategory.SelectedValueChanged
        If cmbMIssuedCategory.Enabled And rbtMetal.Checked Then
            LoadCategoryDetails(cmbMIssuedCategory)
        Else
            LoadCategoryDetails(cmbMCategory)
        End If
        If objGPack.GetSqlValue("SELECT CATGROUP FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbMCategory.Text.ToString() & "'") = "B" Then
            lblOalloy.Visible = True
            txtOalloy_WET.Visible = True
            txtMAlloyper.Enabled = True
            txtMAlloy_WET.Enabled = True
        Else
            lblOalloy.Visible = False
            txtOalloy_WET.Visible = False
            txtMAlloyper.Enabled = False
            txtMAlloy_WET.Enabled = False
        End If
        LoadBalanceWt()
        If MI_STOCKCHECK Then
            LoadBalanceStock(cmbMCategory.Text)
        End If
    End Sub

    Private Sub CalcMRate()
        Dim Rate As Decimal = Nothing
        If cmbMCategory.SelectedValue IsNot Nothing Then
            If UCase(oTransactionType) = "PURCHASE RETURN" Or UCase(oTransactionType) = "PURCHASE" Or UCase(oTransactionType) = "PURCHASE[APPROVAL]" Then
                Rate = Val(GetRate(oTranDate, objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = '" & cmbMCategory.SelectedValue.ToString & "'")))
            End If
        End If
        txtMRate_OWN.Text = IIf(Rate <> 0, Format(Rate, "0.00"), Nothing)
    End Sub

    Private Sub txtMWastPer_PER_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMWastPER.TextChanged
        If Val(txtMWastPER.Text) = 0 Then
            txtMWast_WET.Clear()
            Exit Sub
        End If
        CalcMWastage()
        'txtMWastPER_PER.Text = Format(Val(txtMWastPER_PER.Text), "0.000")
    End Sub

    Private Sub CalcMMc()
        Dim mc As Decimal = Nothing
        If Val(txtMMcGrm_AMT.Text) = 0 Then Exit Sub
        If Val(txtMGrsWt_WET.Text) > 0 Then 'If Val(txtMGrsAmt_AMT.Text) > 0 Then
            mc = Val(txtMGrsWt_WET.Text) * Val(txtMMcGrm_AMT.Text)
        Else
            mc = Val(txtMPcs_NUM.Text) * Val(txtMMcGrm_AMT.Text)
        End If

        mc = RoundOffPisa(mc)
        txtMMc_AMT.Text = IIf(mc <> 0, Format(mc, "0.00"), Nothing)
        CalcMGrossAmt()
    End Sub

    Private Sub CalcMWastage()
        If Val(txtMWastPER.Text) = 0 Then Exit Sub
        Dim was As Decimal = Nothing
        was = Val(txtMGrsWt_WET.Text) * (Val(txtMWastPER.Text) / 100)
        'If lblMWast.Text = "Alloy" Then
        '    txtMWast_WET.Text = IIf(was <> 0, Format(was, FormatNumberStyle(AlloyRnd)), Nothing)
        'Else
        txtMWast_WET.Text = IIf(was <> 0, Format(was, "0.000"), Nothing)
        'End If
    End Sub
    Private Sub CalcMalloy()
        If Val(txtMAlloyper.Text) = 0 Then Exit Sub
        Dim was As Decimal = Nothing
        was = Val(txtMGrsWt_WET.Text) * (Val(txtMAlloyper.Text) / 100)
        txtMAlloy_WET.Text = IIf(was <> 0, Format(was, FormatNumberStyle(AlloyRnd)), Nothing)
    End Sub

    Private Sub txtMWast_WET_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMWast_WET.KeyDown
        If Val(txtMWastPER.Text) <> 0 Then e.Handled = True
    End Sub

    Private Sub txtMWast_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMWast_WET.KeyPress
        If Val(txtMWastPER.Text) <> 0 Then e.Handled = True
        If e.KeyChar = Chr(Keys.Enter) Then
            'If ALLOY_DETAILS Then
            '    If lblMWast.Text = "Alloy" Then
            '        ShowAlloyDetDia()
            '    Else
            '        SendKeys.Send("{TAB}")
            '    End If
            'Else
            SendKeys.Send("{TAB}")
            'End If
        End If
    End Sub
    Private Sub txtMAlloy_WET_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMAlloy_WET.KeyPress
        If Val(txtMAlloyper.Text) <> 0 Then e.Handled = True
        If e.KeyChar = Chr(Keys.Enter) Then
            If ALLOY_DETAILS Then
                ShowAlloyDetDia()
            Else
                'SendKeys.Send("{TAB}")
            End If
            txtTotalWt.Text = IIf(Val(txtMPureWt_WET.Text) <> 0, Format(Val(txtMPureWt_WET.Text) + Val(txtMAlloy_WET.Text), "0.000"), Nothing)

        End If
    End Sub
    Private Sub txtMTouch_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMTouchAMT.TextChanged
        CalcMPureWt()
    End Sub
    'Private Sub CalcMPureWt()
    '    Dim pureWt As Decimal = Nothing
    '    Dim touch As Decimal = IIf(Val(txtMTouch_AMT.Text) <> 0, Val(txtMTouch_AMT.Text), Val(CmbMPurity.Text))
    '    pureWt = (IIf(cmbMGrsNet.Text.ToUpper = "GRS WT", Val(txtMGrsWt_WET.Text), Val(txtMNetWt_WET.Text)) + IIf(lblMWast.Text.ToUpper <> "ALLOY", Val(txtMWast_WET.Text), 0)) * (touch / 100)

    '    If ROUNDEDWT <> 0 Then
    '        txtMPureWt_WET.Text = IIf(pureWt <> 0, Format(Math.Round(pureWt, ROUNDEDWT), "0.000"), Nothing)
    '    Else
    '        txtMPureWt_WET.Text = IIf(pureWt <> 0, Format(pureWt, "0.000"), Nothing)
    '    End If
    '    'If Val(txtMTouch_AMT.Text) <> 0 Then
    '    '    pureWt = (Val(txtMGrsWt_WET.Text) + Val(txtMWast_WET.Text)) * (Val(txtMTouch_AMT.Text) / 100)
    '    'Else
    '    '    pureWt = (Val(txtMGrsWt_WET.Text) + Val(txtMWast_WET.Text)) * (Val(txtMPurity_AMT.Text) / 100)
    '    'End If
    '    'txtMPureWt_WET.Text = IIf(pureWt <> 0, Format(pureWt, "0.000"), Nothing)
    'End Sub
    Private Sub CalcMPureWt()
        Dim pureWt As Decimal = Nothing
        Dim touch As Decimal = IIf(Val(txtMTouchAMT.Text) <> 0, Val(txtMTouchAMT.Text), Val(CmbMPurity.Text))

        pureWt = (IIf(cmbMGrsNet.Text.ToUpper = "GRS WT", Val(txtMGrsWt_WET.Text), Val(txtMNetWt_WET.Text)) + Val(txtMWast_WET.Text)) * (touch / 100)

        Dim Round_G As Integer = ROUNDOFF_WT(0)
        Dim Round_D As Integer = ROUNDOFF_WT(1)
        Dim Round_S As Integer = ROUNDOFF_WT(2)
        Dim Round_P As Integer = ROUNDOFF_WT(3)

        If cmbMMetal.Text = "GOLD" Then
            txtMPureWt_WET.Text = IIf(pureWt <> 0, Format(Math.Round(pureWt, Round_G), "0.000"), Nothing)
        ElseIf cmbMMetal.Text = "DIAMOND" Then
            txtMPureWt_WET.Text = IIf(pureWt <> 0, Format(Math.Round(pureWt, Round_D), "0.000"), Nothing)
        ElseIf cmbMMetal.Text = "SILVER" Then
            txtMPureWt_WET.Text = IIf(pureWt <> 0, Format(Math.Round(pureWt, Round_S), "0.000"), Nothing)
        ElseIf cmbMMetal.Text = "PLATINUM" Then
            txtMPureWt_WET.Text = IIf(pureWt <> 0, Format(Math.Round(pureWt, Round_P), "0.000"), Nothing)
        Else
            txtMPureWt_WET.Text = IIf(pureWt <> 0, Format(pureWt, "0.000"), Nothing)
        End If
    End Sub

    Private Sub txtMGrsWt_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMGrsWt_WET.TextChanged
        CalcMNetWt()
        CalcMWastage()
        CalcMGrossAmt()
    End Sub

    Private Sub CalcMGrossAmt()
        Dim GrsAmt As Decimal = Nothing
        If Val(txtMGrsWt_WET.Text) <> 0 Then
            GrsAmt = (Val(txtMGrsWt_WET.Text) + Val(txtMWast_WET.Text) + (-1 * Val(txtMAlloy_WET.Text))) _
            * Val(txtMRate_OWN.Text)
            If Val(txtMTouchAMT.Text) <> 0 Then
                GrsAmt = Val(txtMPureWt_WET.Text) * Val(txtMRate_OWN.Text)
            End If
        Else
            GrsAmt = Val(txtMPcs_NUM.Text) * Val(txtMRate_OWN.Text)
        End If
        GrsAmt = GrsAmt + Val(txtMAddlCharge_AMT.Text) + Val(txtMMc_AMT.Text)
        GrsAmt = RoundOffPisa(GrsAmt)

        txtMGrsAmt_AMT.Text = IIf(GrsAmt <> 0, Format(GrsAmt, "0.00"), Nothing)
        CalcMVatTds()
        CalcMGST()
        CalcMNetAmt()
    End Sub

    Private Sub txtMPureWt_WET_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMPureWt_WET.GotFocus
        SendKeys.Send("{TAB}")
    End Sub

    Private Sub CalcMGrossAmt(ByVal sender As Object, ByVal e As EventArgs) Handles _
    txtMGrsWt_WET.TextChanged _
    , txtMWast_WET.TextChanged _
    , txtMPureWt_WET.TextChanged _
    , txtMRate_OWN.TextChanged _
    , cmbMGrsNet.SelectedIndexChanged
        CalcMPureWt()
        CalcMGrossAmt()
    End Sub
    Private Sub CalcMGST()
        Dim Gst As Decimal = Nothing
        Gst = Val(txtMGrsAmt_AMT.Text) * (Val(txtMSgst_AMT.Text) / 100)
        Gst = RoundOffPisa(Gst)
        txtMSG_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.00"), Nothing)

        Gst = Val(txtMGrsAmt_AMT.Text) * (Val(txtMCgst_AMT.Text) / 100)
        Gst = RoundOffPisa(Gst)
        txtMCG_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.00"), Nothing)

        Gst = Val(txtMGrsAmt_AMT.Text) * (Val(txtMIgst_AMT.Text) / 100)
        Gst = RoundOffPisa(Gst)
        txtMIG_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.00"), Nothing)
        If lblMVat.Text.ToUpper <> "TDS" Then
            Gst = Val(txtMSG_AMT.Text) + Val(txtMCG_AMT.Text) + Val(txtMIG_AMT.Text)
            txtMVat_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.00"), Nothing)
        End If
    End Sub
    Private Sub CalcMVatTds()
        Dim vatTds As Decimal = Nothing
        vatTds = Val(txtMGrsAmt_AMT.Text) * (Val(txtMVatPer_PER.Text) / 100)
        vatTds = IIf(lblOVat.Text.ToUpper = "TDS", RoundOffPisa(vatTds, True), RoundOffPisa(vatTds))
        txtMVat_AMT.Text = IIf(vatTds <> 0, Format(vatTds, "0.00"), Nothing)
        CalcMNetAmt()
    End Sub
    Public Sub CalcMNetAmt()
        Dim netAmt As Decimal = Nothing
        Dim vatAmt As Decimal = Val(txtMVat_AMT.Text)
        'If oTransactionType = "PURCHASE RETURN" Then vatAmt = vatAmt * -1
        netAmt = Val(txtMGrsAmt_AMT.Text) + IIf(lblMVat.Text.ToUpper = "TDS", -1 * vatAmt, vatAmt)
        If lblMVat.Text.ToUpper = "TDS" Then
            netAmt += Val(txtMSG_AMT.Text) + Val(txtMCG_AMT.Text) + Val(txtMIG_AMT.Text)
        End If
        netAmt += Val(txtMTCS_AMT.Text.ToString)
        netAmt = RoundOffPisa(netAmt)
        txtMAmount_AMT.Text = IIf(netAmt <> 0, Format(netAmt, "0.00"), Nothing)
    End Sub
    Private Sub txtMVatPer_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMVatPer_PER.TextChanged
        If Val(txtMVatPer_PER.Text) = 0 Then
            txtMVat_AMT.Clear()
            Exit Sub
        End If
        CalcMVatTds()
    End Sub

    Private Sub txtMVat_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMVat_AMT.GotFocus
        If Val(txtMGrsAmt_AMT.Text) = 0 And PURAMTEDITACC = "N" Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtMVat_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMVat_AMT.KeyDown
        If Val(txtMVatPer_PER.Text) <> 0 Then e.Handled = True
    End Sub

    Private Sub txtMVat_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMVat_AMT.KeyPress
        If Val(txtMVatPer_PER.Text) <> 0 Then e.Handled = True
    End Sub

    Private Sub txtMVat_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMVat_AMT.TextChanged
        CalcMNetAmt()
    End Sub

    Private Sub Lock_GotFocus_Orn(ByVal sender As Object, ByVal e As EventArgs) Handles _
    txtOGrsAmt_AMT.GotFocus, txtOAmount_AMT.GotFocus
        If oTransactionType = "PURCHASE" Or UCase(oTransactionType) = "PURCHASE[APPROVAL]" Or oTransactionType = "PURCHASE RETURN" Or oTransactionType = "INTERNAL TRANSFER" Then
            If Not (Val(txtORate_OWN.Text) = 0) And PURAMTEDITACC = "N" And RateCalcAmt = False Then  'And Val(txtOMc_AMT.Text) = 0
                SendKeys.Send("{TAB}")
            End If
        ElseIf (oTransactionType = "ISSUE" Or UCase(oTransactionType) = "RECEIPT") And RateCalcAmt = True And PURAMTEDITACC = "N" And txtOAmount_AMT.Focused = False Then
            txtOGrsAmt_AMT.Select()
        Else
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub Lock_GotFocus_Met(ByVal sender As Object, ByVal e As EventArgs) Handles _
txtMGrsAmt_AMT.GotFocus, txtMAmount_AMT.GotFocus
        If oTransactionType = "PURCHASE" Or UCase(oTransactionType) = "PURCHASE[APPROVAL]" Or oTransactionType = "PURCHASE RETURN" Or oTransactionType = "INTERNAL TRANSFER" Then
            If Not (Val(txtMRate_OWN.Text) = 0) And PURAMTEDITACC = "N" Then
                SendKeys.Send("{TAB}")
            End If
        ElseIf (oTransactionType = "ISSUE" Or UCase(oTransactionType) = "RECEIPT") And RateCalcAmt = True And txtMAmount_AMT.Focused = False Then
            txtMGrsAmt_AMT.Select()
        Else
            SendKeys.Send("{TAB}")
        End If
    End Sub


    Private Sub Lock_GotFocus_Stn(ByVal sender As Object, ByVal e As EventArgs) Handles _
        txtSGrsAmt_AMT.GotFocus, txtSAmount_AMT.GotFocus
        If oTransactionType = "PURCHASE" Or UCase(oTransactionType) = "PURCHASE[APPROVAL]" Or oTransactionType = "PURCHASE RETURN" Or oTransactionType = "INTERNAL TRANSFER" Then
            If Not (Val(txtSRate_OWN.Text) = 0) And PURAMTEDITACC = "N" Then
                SendKeys.Send("{TAB}")
            End If
        ElseIf (oTransactionType = "ISSUE" Or UCase(oTransactionType) = "RECEIPT") And RateCalcAmt = True And txtSAmount_AMT.Focused = False Then
            txtSGrsAmt_AMT.Select()
        Else
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub MVat_GotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles txtMVatPer_PER.GotFocus
        If Val(txtMGrsAmt_AMT.Text) = 0 And PURAMTEDITACC = "N" Then
            SendKeys.Send("{TAB}")
        ElseIf Val(txtOGrsAmt_AMT.Text) > 0 And Val(txtMMc_AMT.Text) > 0 Then
            If oTransactionType = "RECEIPT" And GstFlag Then
                McBill = True
                If oTransactionType = "RECEIPT" Then 'VBJ GST SOFT CHANGE
                    If _accStateId <> CompanyStateId Then
                        'txtMIgst_AMT.Text = Format(SrVtTax, "0.00")
                        Dim splitAccode() As String
                        StrSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='LABOUR_IGST'"
                        splitAccode = objGPack.GetSqlValue(StrSql, "CTLTEXT", "").Split(":")
                        If splitAccode.Length = 2 Then
                            txtOIgst_AMT.Text = splitAccode(1)
                            txtMIgst_AMT.Text = splitAccode(1)
                            txtSIgst_WET.Text = splitAccode(1)
                            txtOthIgst_AMT.Text = splitAccode(1)
                        Else
                            txtOIgst_AMT.Text = Format(SrVtTax, "0.000")
                            txtMIgst_AMT.Text = Format(SrVtTax, "0.000")
                            txtSIgst_WET.Text = Format(SrVtTax, "0.000")
                            txtOthIgst_AMT.Text = Format(SrVtTax, "0.000")
                        End If

                    Else
                        Dim splitAccode() As String
                        StrSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='LABOUR_CGST'"
                        splitAccode = objGPack.GetSqlValue(StrSql, "CTLTEXT", "").Split(":")
                        If splitAccode.Length = 2 Then
                            txtOCgst_AMT.Text = splitAccode(1)
                            txtMCgst_AMT.Text = splitAccode(1)
                            txtSCgst_WET.Text = splitAccode(1)
                            txtOthCgst_AMT.Text = splitAccode(1)
                        Else
                            txtOCgst_AMT.Text = Format((SrVtTax / 2), "0.000")
                            txtMCgst_AMT.Text = Format((SrVtTax / 2), "0.000")
                            txtSCgst_WET.Text = Format((SrVtTax / 2), "0.000")
                            txtOthCgst_AMT.Text = Format((SrVtTax / 2), "0.000")
                        End If
                        ' txtMCgst_AMT.Text = Format((SrVtTax / 2), "0.00")

                        StrSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='LABOUR_SGST'"
                        splitAccode = objGPack.GetSqlValue(StrSql, "CTLTEXT", "").Split(":")
                        If splitAccode.Length = 2 Then
                            txtOSgst_AMT.Text = splitAccode(1)
                            txtMSgst_AMT.Text = splitAccode(1)
                            txtSSgst_WET.Text = splitAccode(1)
                            txtOthSgst_AMT.Text = splitAccode(1)
                        Else
                            txtOSgst_AMT.Text = Format((SrVtTax / 2), "0.000")
                            txtMSgst_AMT.Text = Format((SrVtTax / 2), "0.000")
                            txtSSgst_WET.Text = Format((SrVtTax / 2), "0.000")
                            txtOthSgst_AMT.Text = Format((SrVtTax / 2), "0.000")
                        End If
                        'txtMSgst_AMT.Text = Format((SrVtTax / 2), "0.00")
                    End If
                Else
                    If _accStateId <> CompanyStateId Then
                        txtMIgst_AMT.Text = Format(SrVtTax, "0.00")
                    Else
                        txtMCgst_AMT.Text = Format((SrVtTax / 2), "0.00")
                        txtMSgst_AMT.Text = Format((SrVtTax / 2), "0.00")
                    End If
                End If

            End If
        End If
    End Sub

#End Region

#Region "Others Methods And Events"
    Private Sub cmbOthCategory_EnabledChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOthCategory.EnabledChanged
        If cmbOthCategory.Enabled = False Then
            cmbOthItem.Text = ""
            cmbOthSubItem.Text = ""
        End If
        cmbOthItem.Enabled = cmbOthCategory.Enabled
        cmbOthSubItem.Enabled = cmbOthCategory.Enabled
    End Sub

    Public Sub cmbOthMetal_SelectedValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbOthMetal.SelectedValueChanged
        If rbtOthers.Checked Then LoadMetalDetails(cmbOthMetal)
    End Sub

    Public Sub cmbOthCategory_SelectedValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbOthCategory.SelectedValueChanged
        LoadCategoryDetails(cmbOthCategory)
    End Sub

    Public Sub cmbOthItem_SelectedValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _
        cmbOthItem.SelectedValueChanged
        LoadItemDetail(cmbOthItem)
    End Sub

    Private Sub cmbOthItem_EnabledChanged(ByVal sender As Object, ByVal e As EventArgs) Handles _
    cmbOthItem.EnabledChanged
        cmbOthSubItem.Text = ""
        If cmbOthItem.Enabled = False Then
            cmbOthSubItem.Enabled = False
        End If
    End Sub

    Private Sub txtOthRate_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOthRate_AMT.KeyPress

    End Sub

    Private Sub CalcOthGrossAmt(ByVal sender As Object, ByVal e As EventArgs) Handles _
    txtOthPcs_NUM.TextChanged _
    , txtOthRate_AMT.TextChanged
        Dim GrsAmt As Decimal = Nothing
        GrsAmt = Val(txtOthPcs_NUM.Text) * Val(txtOthRate_AMT.Text)
        txtOthGrsAmt_AMT.Text = IIf(GrsAmt <> 0, Format(GrsAmt, "0.00"), Nothing)
        CalcOthVatTds()
        CalcOthGST()
        CalcOthNetAmt()
    End Sub
    Private Sub CalcOthGST()
        Dim Gst As Decimal = Nothing
        Gst = Val(txtOthGrsAmt_AMT.Text) * (Val(txtOthSgst_AMT.Text) / 100)
        Gst = RoundOffPisa(Gst)
        txtOthSG_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.00"), Nothing)

        Gst = Val(txtOthGrsAmt_AMT.Text) * (Val(txtOthCgst_AMT.Text) / 100)
        Gst = RoundOffPisa(Gst)
        txtOthCG_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.00"), Nothing)

        Gst = Val(txtOthGrsAmt_AMT.Text) * (Val(txtOthIgst_AMT.Text) / 100)
        Gst = RoundOffPisa(Gst)
        txtOthIG_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.00"), Nothing)
        If lblOthVat.Text.ToUpper <> "TDS" Then
            Gst = Val(txtOthSG_AMT.Text) + Val(txtOthCG_AMT.Text) + Val(txtOthIG_AMT.Text)
            txtOthVat_AMT.Text = IIf(Gst <> 0, Format(Gst, "0.00"), Nothing)
        End If
    End Sub
    Private Sub CalcOthVatTds()
        Dim vatTds As Decimal = Nothing
        vatTds = Val(txtOthGrsAmt_AMT.Text) * (Val(txtOthVatPer_PER.Text) / 100)
        vatTds = RoundOffPisa(vatTds)
        txtOthVat_AMT.Text = IIf(vatTds <> 0, Format(vatTds, "0.00"), Nothing)
        CalcOthNetAmt()
    End Sub
    Public Sub CalcOthNetAmt()
        Dim netAmt As Decimal = Nothing
        Dim vatAmt As Decimal = Val(txtOthVat_AMT.Text)
        'If oTransactionType = "PURCHASE RETURN" Then vatAmt = vatAmt * -1
        netAmt = Val(txtOthGrsAmt_AMT.Text) + IIf(lblOthVat.Text.ToUpper = "TDS", -1 * vatAmt, vatAmt)
        If lblOthVat.Text.ToUpper = "TDS" Then
            netAmt += Val(txtOthSG_AMT.Text) + Val(txtOthCG_AMT.Text) + Val(txtOthIG_AMT.Text)
        End If
        netAmt = RoundOffPisa(netAmt)
        txtOthAmount_AMT.Text = IIf(netAmt <> 0, Format(netAmt, "0.00"), Nothing)
    End Sub

    Private Sub txtOthVatPer_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOthVatPer_PER.TextChanged
        If Val(txtOthVatPer_PER.Text) = 0 Then
            txtOthVat_AMT.Clear()
            Exit Sub
        End If
        CalcOthVatTds()
    End Sub

    Private Sub txtOthVat_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOthVat_AMT.KeyDown
        If Val(txtOthVatPer_PER.Text) <> 0 Then e.Handled = True
    End Sub

    Private Sub txtOthVat_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOthVat_AMT.KeyPress
        If Val(txtOthVatPer_PER.Text) <> 0 Then e.Handled = True
    End Sub

    Private Sub txtOthVat_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOthVat_AMT.TextChanged
        CalcOthNetAmt()
    End Sub

    Private Sub OthVat_GotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles _
txtOthVat_AMT.GotFocus, txtOthVatPer_PER.GotFocus
        If Val(txtOthGrsAmt_AMT.Text) = 0 Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

#End Region

    Private Sub txtOJobNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOOrdNo.GotFocus
        If _JobNoEnable = True Then
            If oEditRowIndex = -1 And MultiStone = False Then
                StrSql = " SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'GEN-JOBNO' AND COMPANYID = '" & strCompanyId & "'"
                JobNo = Val(objGPack.GetSqlValue(StrSql, , , ))
                txtOOrdNo.Text = "J" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & (JobNo + 1)
            End If
        End If
    End Sub

    Private Sub txtSJobNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSOrdNo.GotFocus
        If _JobNoEnable = True Then
            If oEditRowIndex = -1 And MultiStone = False Then
                StrSql = " SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'GEN-JOBNO' AND COMPANYID = '" & strCompanyId & "'"
                JobNo = Val(objGPack.GetSqlValue(StrSql, , , ))
                txtSOrdNo.Text = "J" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & (JobNo + 1)
            End If
        End If
    End Sub

    Private Sub txtMJobNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMOrdNo.GotFocus
        If _JobNoEnable = True Then
            If oEditRowIndex = -1 And MultiStone = False Then
                StrSql = " SELECT CONVERT(INT,CTLTEXT) FROM " & cnStockDb & "..BILLCONTROL WHERE CTLID = 'GEN-JOBNO' AND COMPANYID = '" & strCompanyId & "'"
                JobNo = Val(objGPack.GetSqlValue(StrSql, , , ))
                txtMOrdNo.Text = "J" & Mid(Format(cnTranToDate, "dd/MM/yyyy"), 9, 2).ToString & (JobNo + 1)
            End If
        End If
    End Sub

    Private Sub txtOJobNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtOOrdNo.KeyDown
        If e.KeyCode = Keys.Insert Then
            StrSql = ""
            If STOCKVALIDATION Then
                InsSno = ""
                If oMaterial = Material.Issue Then
                    StrSql = vbCrLf + " SELECT "
                    StrSql += vbCrLf + " ISNULL((SELECT TRANNO FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO),9999)TRANNO"
                    StrSql += vbCrLf + " ,(SELECT CONVERT(VARCHAR,TRANDATE,105) FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)TRANDATE"
                    StrSql += vbCrLf + " ,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE="
                    StrSql += vbCrLf + " (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO))ACNAME"
                    StrSql += vbCrLf + " ,METAL"
                    StrSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT"
                    StrSql += vbCrLf + " ,SNO"
                    'StrSql += vbCrLf + " ,SUM(REC)REC"
                    StrSql += vbCrLf + " FROM"
                    StrSql += vbCrLf + " ("
                    StrSql += vbCrLf + " SELECT SNO,M.METALNAME METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                    StrSql += vbCrLf + " ,SUM(ISNULL(PCS,0)) PCS"
                    StrSql += vbCrLf + " ,SUM(ISNULL(R.GRSWT,0)) GRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) NETWT"
                    StrSql += vbCrLf + " ,SUM(CASE WHEN R.GRSWT=0 THEN R.PCS END) REC"
                    StrSql += vbCrLf + " FROM " & cnStockDb & "..OPENWEIGHT R "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON C.METALID = M.METALID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = R.ITEMID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = R.SUBITEMID"
                    StrSql += vbCrLf + " WHERE R.STOCKTYPE='C'"
                    If MCostId <> "" Then StrSql += vbCrLf + " AND R.COSTID='" & MCostId & "'"
                    StrSql += vbCrLf + " AND C.PURITYID IN(SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALTYPE='O')"
                    StrSql += vbCrLf + " GROUP BY SNO,M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME  "
                    StrSql += vbCrLf + " UNION ALL"
                    StrSql += vbCrLf + " SELECT SNO,M.METALNAME METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                    StrSql += vbCrLf + " ,SUM(ISNULL(PCS,0)) PCS"
                    StrSql += vbCrLf + " ,SUM(ISNULL(R.GRSWT,0)) GRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) NETWT"
                    StrSql += vbCrLf + " ,SUM(CASE WHEN R.GRSWT=0 THEN R.PCS END) REC"
                    StrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT R "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON R.METALID = M.METALID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = R.ITEMID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = R.SUBITEMID"
                    StrSql += vbCrLf + " WHERE R.TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "'"
                    StrSql += vbCrLf + " AND LEN(R.TRANTYPE)>2"
                    'StrSql += vbCrLf + " AND R.TRANTYPE<>'RIN'"
                    'StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    If MCostId <> "" Then StrSql += vbCrLf + " AND R.COSTID='" & MCostId & "'"
                    StrSql += vbCrLf + " AND C.PURITYID IN(SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALTYPE='O')"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
                    StrSql += vbCrLf + " GROUP BY SNO,M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME  "
                    StrSql += vbCrLf + " UNION ALL"
                    StrSql += vbCrLf + " SELECT RESNO AS SNO,M.METALNAME AS METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                    StrSql += vbCrLf + " ,SUM(ISNULL(PCS,0))*(-1) PCS"
                    StrSql += vbCrLf + " ,SUM(ISNULL(I.GRSWT,0))*(-1) GRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0))*(-1) NETWT"
                    StrSql += vbCrLf + " ,0 REC"
                    StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE  I "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = I.ITEMID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = I.SUBITEMID"
                    StrSql += vbCrLf + " WHERE I.TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "'"
                    StrSql += vbCrLf + " AND LEN(I.TRANTYPE)>2"
                    StrSql += vbCrLf + " AND I.TRANTYPE<>'IIN'"
                    'StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    If MCostId <> "" Then StrSql += vbCrLf + " AND I.COSTID='" & MCostId & "'"
                    StrSql += vbCrLf + " AND C.PURITYID IN(SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALTYPE='O')"
                    StrSql += vbCrLf + " AND RESNO IN"
                    StrSql += vbCrLf + " (SELECT SNO FROM " & cnStockDb & "..OPENWEIGHT UNION ALL SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "' AND ISNULL(CANCEL,'')=''"
                    If MCostId <> "" Then StrSql += vbCrLf + " AND COSTID='" & MCostId & "'"
                    StrSql += vbCrLf + " AND LEN(TRANTYPE)>2 "
                    'StrSql += vbCrLf + " AND TRANTYPE<>'RIN' "
                    StrSql += vbCrLf + " )"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
                    StrSql += vbCrLf + " GROUP BY RESNO,M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME  "
                    StrSql += vbCrLf + " UNION ALL"
                    StrSql += vbCrLf + " SELECT RECSNO AS SNO,M.METALNAME AS METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                    StrSql += vbCrLf + " ,SUM(ISNULL(PCS,0))*(-1) PCS"
                    StrSql += vbCrLf + " ,SUM(ISNULL(I.GRSWT,0))*(-1) GRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0))*(-1) NETWT"
                    StrSql += vbCrLf + " ,0 REC"
                    StrSql += vbCrLf + " FROM " & cnStockDb & "..LOTISSUE  I "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = I.ITEMID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON IM.METALID = M.METALID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = I.SUBITEMID"
                    StrSql += vbCrLf + " WHERE  "
                    StrSql += vbCrLf + " RECSNO IN"
                    StrSql += vbCrLf + " (SELECT SNO FROM " & cnStockDb & "..OPENWEIGHT UNION ALL SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "' AND ISNULL(CANCEL,'')=''"
                    StrSql += vbCrLf + " AND LEN(TRANTYPE)>2 "
                    'StrSql += vbCrLf + " AND TRANTYPE<>'RIN' "
                    StrSql += vbCrLf + " )"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
                    StrSql += vbCrLf + " GROUP BY RECSNO,M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME  "
                    StrSql += vbCrLf + " )X GROUP BY SNO,METAL HAVING (SUM(GRSWT)>0 OR (SUM(PCS)>0 AND SUM(REC)>0))"
                    StrSql += vbCrLf + " ORDER BY CONVERT(INT,SUBSTRING(SNO,7,LEN(SNO))) ASC"
                ElseIf STOCKVALIDATION_MR And oMaterial = Material.Receipt Then
                    GoTo reciss
                End If
                If StrSql = "" Then txtOOrdNo.SelectAll() : Exit Sub
                Dim dtStk As New DataTable
                Cmd = New OleDbCommand(StrSql, cn)
                Da = New OleDbDataAdapter(Cmd)
                Da.Fill(dtStk)
                Dim StkRow As DataRow
                If dtStk.Rows.Count > 0 Then
                    StkRow = BrighttechPack.SearchDialog.Show_R("Find ", StrSql, cn, , 0)
                    If Not StkRow Is Nothing Then
                        txtOOrdNo.Text = StkRow.Item("TRANNO").ToString
                        InsSno = StkRow.Item("SNO").ToString
                    End If
                Else
                    MsgBox("Record Not Found", MsgBoxStyle.Information)
                End If
                txtOOrdNo.SelectAll()
            ElseIf STOCKVALIDATION_MR Then
reciss:
                InsSno = ""
                If oMaterial = Material.Receipt Then
                    StrSql = vbCrLf + " SELECT "
                    StrSql += vbCrLf + " ISNULL((SELECT TRANNO FROM " & cnStockDb & "..ISSUE WHERE SNO=X.SNO),9999)TRANNO"
                    StrSql += vbCrLf + " ,(SELECT CONVERT(VARCHAR,TRANDATE,105) FROM " & cnStockDb & "..ISSUE WHERE SNO=X.SNO)TRANDATE"
                    StrSql += vbCrLf + " ,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE="
                    StrSql += vbCrLf + " (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ISSUE WHERE SNO=X.SNO))ACNAME"
                    StrSql += vbCrLf + " ,METAL"
                    StrSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT"
                    StrSql += vbCrLf + " ,SNO"
                    'StrSql += vbCrLf + " ,SUM(REC)REC"
                    StrSql += vbCrLf + " FROM"
                    StrSql += vbCrLf + " ("
                    StrSql += vbCrLf + " SELECT SNO,M.METALNAME METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                    StrSql += vbCrLf + " ,SUM(ISNULL(PCS,0)) PCS"
                    StrSql += vbCrLf + " ,SUM(ISNULL(R.GRSWT,0)) GRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) NETWT"
                    StrSql += vbCrLf + " ,SUM(CASE WHEN R.GRSWT=0 THEN R.PCS END) REC"
                    StrSql += vbCrLf + " FROM " & cnStockDb & "..OPENWEIGHT R "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON C.METALID = M.METALID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = R.ITEMID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = R.SUBITEMID"
                    StrSql += vbCrLf + " WHERE R.STOCKTYPE='C'"
                    If MCostId <> "" Then StrSql += vbCrLf + " AND R.COSTID='" & MCostId & "'"
                    StrSql += vbCrLf + " AND C.PURITYID IN(SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALTYPE='O')"
                    StrSql += vbCrLf + " GROUP BY SNO,M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME  "
                    StrSql += vbCrLf + " UNION ALL"

                    StrSql += vbCrLf + " SELECT SNO,M.METALNAME METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                    StrSql += vbCrLf + " ,SUM(ISNULL(PCS,0)) PCS"
                    StrSql += vbCrLf + " ,SUM(ISNULL(R.GRSWT,0)) GRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) NETWT"
                    StrSql += vbCrLf + " ,SUM(CASE WHEN R.GRSWT=0 THEN R.PCS END) REC"
                    StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE R "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON R.METALID = M.METALID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = R.ITEMID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = R.SUBITEMID"
                    StrSql += vbCrLf + " WHERE R.TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "'"
                    StrSql += vbCrLf + " AND LEN(R.TRANTYPE)>2"
                    'StrSql += vbCrLf + " AND R.TRANTYPE<>'RIN'"
                    'StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    If MCostId <> "" Then StrSql += vbCrLf + " AND R.COSTID='" & MCostId & "'"
                    StrSql += vbCrLf + " AND C.PURITYID IN(SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALTYPE='O')"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
                    StrSql += vbCrLf + " GROUP BY SNO,M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME  "

                    StrSql += vbCrLf + " UNION ALL"
                    StrSql += vbCrLf + " SELECT ISSNO AS SNO,M.METALNAME AS METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                    StrSql += vbCrLf + " ,SUM(ISNULL(PCS,0))*(-1) PCS"
                    StrSql += vbCrLf + " ,SUM(ISNULL(I.GRSWT,0))*(-1) GRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0))*(-1) NETWT"
                    StrSql += vbCrLf + " ,0 REC"
                    StrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT I "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = I.ITEMID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = I.SUBITEMID"
                    StrSql += vbCrLf + " WHERE I.TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "'"
                    StrSql += vbCrLf + " AND LEN(I.TRANTYPE)>2"
                    StrSql += vbCrLf + " AND I.TRANTYPE<>'RIN'"
                    'StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    If MCostId <> "" Then StrSql += vbCrLf + " AND I.COSTID='" & MCostId & "'"
                    StrSql += vbCrLf + " AND C.PURITYID IN(SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALTYPE='O')"
                    StrSql += vbCrLf + " AND ISSNO IN"
                    StrSql += vbCrLf + " (SELECT SNO FROM " & cnStockDb & "..OPENWEIGHT UNION ALL SELECT SNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "' AND ISNULL(CANCEL,'')=''"
                    If MCostId <> "" Then StrSql += vbCrLf + " AND COSTID='" & MCostId & "'"
                    StrSql += vbCrLf + " AND LEN(TRANTYPE)>2 "
                    'StrSql += vbCrLf + " AND TRANTYPE<>'RIN' "
                    StrSql += vbCrLf + " )"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
                    StrSql += vbCrLf + " GROUP BY ISSNO,M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME  "

                    StrSql += vbCrLf + " UNION ALL"
                    StrSql += vbCrLf + " SELECT RECSNO AS SNO,M.METALNAME AS METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                    StrSql += vbCrLf + " ,SUM(ISNULL(PCS,0))*(-1) PCS"
                    StrSql += vbCrLf + " ,SUM(ISNULL(I.GRSWT,0))*(-1) GRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0))*(-1) NETWT"
                    StrSql += vbCrLf + " ,0 REC"
                    StrSql += vbCrLf + " FROM " & cnStockDb & "..LOTISSUE  I "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = I.ITEMID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON IM.METALID = M.METALID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = I.SUBITEMID"
                    StrSql += vbCrLf + " WHERE  "
                    StrSql += vbCrLf + " RECSNO IN"
                    StrSql += vbCrLf + " (SELECT SNO FROM " & cnStockDb & "..OPENWEIGHT UNION ALL SELECT SNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "' AND ISNULL(CANCEL,'')=''"
                    StrSql += vbCrLf + " AND LEN(TRANTYPE)>2 "
                    'StrSql += vbCrLf + " AND TRANTYPE<>'RIN' "
                    StrSql += vbCrLf + " )"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
                    StrSql += vbCrLf + " GROUP BY RECSNO,M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME  "

                    StrSql += vbCrLf + " )X GROUP BY SNO,METAL HAVING (SUM(GRSWT)>0 OR (SUM(PCS)>0 AND SUM(REC)>0))"
                    StrSql += vbCrLf + " ORDER BY CONVERT(INT,SUBSTRING(SNO,7,LEN(SNO))) ASC"
                End If
                If StrSql = "" Then txtOOrdNo.SelectAll() : Exit Sub
                Dim dtStk As New DataTable
                Cmd = New OleDbCommand(StrSql, cn)
                Da = New OleDbDataAdapter(Cmd)
                Da.Fill(dtStk)
                Dim StkRow As DataRow
                If dtStk.Rows.Count > 0 Then
                    StkRow = BrighttechPack.SearchDialog.Show_R("Find ", StrSql, cn, , 0)
                    If Not StkRow Is Nothing Then
                        txtOOrdNo.Text = StkRow.Item("TRANNO").ToString
                        InsSno = StkRow.Item("SNO").ToString
                    End If
                Else
                    MsgBox("Record Not Found", MsgBoxStyle.Information)
                End If
                txtOOrdNo.SelectAll()
            Else
                If oMaterial = Material.Issue Then
                    StrSql = vbCrLf + " SELECT METAL,JOBNO,SUM(RPCS) [REC PCS],SUM(RGRSWT) [REC GRSWT],SUM(RNETWT) [REC NETWT],SUM(IPCS) [ISS PCS],SUM(IGRSWT) [ISS GRSWT],SUM(INETWT) [ISS NETWT]  FROM"
                    StrSql += vbCrLf + " ("
                    StrSql += vbCrLf + " SELECT M.METALNAME METAL,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,SUM(ISNULL(PCS,0)) RPCS,SUM(ISNULL(GRSWT,0)) RGRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) RNETWT,0 IPCS,0 IGRSWT,0 INETWT  FROM " & cnStockDb & "..RECEIPT R "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON R.METALID = M.METALID"
                    StrSql += vbCrLf + " WHERE TRANTYPE IN('RRE','RPU') AND ISNULL(JOBNO,'')<>'' "
                    StrSql += vbCrLf + " AND ACCODE='" & Maccode & "' AND R.METALID NOT IN ('T')"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
                    StrSql += vbCrLf + " GROUP BY M.METALNAME,JOBNO "
                    StrSql += vbCrLf + " UNION ALL"
                    StrSql += vbCrLf + " SELECT M.METALNAME AS METAL,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,0 IPCS,0 IGRSWT,0 INETWT,SUM(ISNULL(PCS,0)) IPCS,SUM(ISNULL(GRSWT,0)) IGRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) INETWT  FROM " & cnStockDb & "..ISSUE  I "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
                    StrSql += vbCrLf + " WHERE TRANTYPE IN('IIS','IPU','IRC') AND ISNULL(JOBNO,'')<>'' "
                    StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y' AND I.METALID NOT IN ('T')"
                    StrSql += vbCrLf + " GROUP BY M.METALNAME,JOBNO "
                    StrSql += vbCrLf + " )X GROUP BY METAL,JOBNO HAVING SUM(IGRSWT-RGRSWT) <>0 "
                    StrSql += vbCrLf + " ORDER BY JOBNO,METAL"
                ElseIf oMaterial = Material.Receipt Then
                    StrSql = vbCrLf + " SELECT METAL,JOBNO,SUM(IPCS)[ISSUE PCS],SUM(IGRSWT) [ISS GRSWT],SUM(INETWT) [ISS NETWT],"
                    StrSql += vbCrLf + "SUM(RPCS)[REC PCS],SUM(RGRSWT) [REC GRSWT],SUM(RNETWT) [REC NETWT] FROM"
                    StrSql += vbCrLf + " ("
                    StrSql += vbCrLf + " SELECT M.METALNAME AS METAL,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,SUM(ISNULL(PCS,0)) IPCS,SUM(ISNULL(GRSWT,0)) IGRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) INETWT,0 RPCS,0 RGRSWT,0 RNETWT  FROM " & cnStockDb & "..ISSUE I"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
                    StrSql += vbCrLf + " WHERE TRANTYPE IN('IIS','IPU','IRC') AND ISNULL(JOBNO,'')<>'' "
                    StrSql += vbCrLf + " AND ACCODE='" & Maccode & "' AND I.METALID NOT IN ('T')"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
                    StrSql += vbCrLf + " GROUP BY METALNAME,JOBNO "
                    StrSql += vbCrLf + " UNION ALL"
                    StrSql += vbCrLf + " SELECT M.METALNAME AS METAL,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,0 IPCS,0 IGRSWT,0 INETWT,SUM(ISNULL(PCS,0)) RPCS,SUM(ISNULL(GRSWT,0)) RGRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) RNETWT  FROM " & cnStockDb & "..RECEIPT R "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON R.METALID = M.METALID"
                    StrSql += vbCrLf + " WHERE TRANTYPE IN('RRE','RPU') AND ISNULL(JOBNO,'')<>'' "
                    StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y' AND R.METALID NOT IN ('T')"
                    StrSql += vbCrLf + " GROUP BY METALNAME,JOBNO "
                    StrSql += vbCrLf + " )X GROUP BY METAL,JOBNO HAVING SUM(IGRSWT-RGRSWT)<>0 "
                    StrSql += vbCrLf + " ORDER BY JOBNO,METAL"
                End If
                Dim JnoRow As DataRow
                JnoRow = BrighttechPack.SearchDialog.Show_R("Find JobNo", StrSql, cn, , 1, , , , , , False)
                If Not JnoRow Is Nothing Then
                    txtOOrdNo.Text = JnoRow.Item("JOBNO").ToString
                    cmbOMetal.Text = JnoRow.Item("METAL").ToString
                End If
                txtOOrdNo.SelectAll()
            End If
        End If
    End Sub

    Private Sub txtSJobNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSOrdNo.KeyDown
        If e.KeyCode = Keys.Insert Then
            If txtSOrdNo.Text.Trim = "" Then
                'Exit Sub
            End If
            StrSql = ""
            If STOCKVALIDATION Then
                InsSno = ""
                If oMaterial = Material.Issue Then
                    StrSql = vbCrLf + " SELECT "
                    StrSql += vbCrLf + " (SELECT TRANNO FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)TRANNO"
                    StrSql += vbCrLf + " ,(SELECT TRANDATE FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)TRANDATE"
                    StrSql += vbCrLf + " ,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE="
                    StrSql += vbCrLf + " (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO))ACNAME"
                    StrSql += vbCrLf + " ,METAL,ITEM,SUBITEM"
                    StrSql += vbCrLf + " ,(SELECT STONEUNIT FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)UNIT"
                    StrSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT"
                    StrSql += vbCrLf + " ,SNO"
                    StrSql += vbCrLf + " FROM"
                    StrSql += vbCrLf + " ("
                    StrSql += vbCrLf + " SELECT SNO,M.METALNAME METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                    StrSql += vbCrLf + " ,SUM(ISNULL(PCS,0)) PCS"
                    StrSql += vbCrLf + " ,SUM(ISNULL(R.GRSWT,0)) GRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) NETWT"
                    StrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT R "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON R.METALID = M.METALID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = R.ITEMID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = R.SUBITEMID"
                    StrSql += vbCrLf + " WHERE R.TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "'"
                    StrSql += vbCrLf + " AND LEN(R.TRANTYPE)>2"
                    StrSql += vbCrLf + " AND R.TRANTYPE<>'RIN'"
                    'StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    If MCostId <> "" Then StrSql += vbCrLf + " AND R.COSTID='" & MCostId & "'"
                    StrSql += vbCrLf + " AND M.TTYPE='S'"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
                    StrSql += vbCrLf + " GROUP BY SNO,M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME  "
                    StrSql += vbCrLf + " UNION ALL"
                    StrSql += vbCrLf + " SELECT RESNO AS SNO,M.METALNAME AS METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                    StrSql += vbCrLf + " ,SUM(ISNULL(PCS,0))*(-1) PCS"
                    StrSql += vbCrLf + " ,CASE "
                    StrSql += vbCrLf + " WHEN (SELECT STONEUNIT FROM " & cnStockDb & "..RECEIPT WHERE SNO=I.RESNO)='G' AND I.STONEUNIT ='C' THEN (SUM(ISNULL(I.GRSWT,0))/5)*(-1) "
                    StrSql += vbCrLf + " WHEN (SELECT STONEUNIT FROM " & cnStockDb & "..RECEIPT WHERE SNO=I.RESNO)='C' AND I.STONEUNIT ='G' THEN (SUM(ISNULL(I.GRSWT,0))*5)*(-1) "
                    StrSql += vbCrLf + " ELSE SUM(ISNULL(I.GRSWT,0))*(-1) END GRSWT"
                    StrSql += vbCrLf + " ,CASE "
                    StrSql += vbCrLf + " WHEN (SELECT STONEUNIT FROM " & cnStockDb & "..RECEIPT WHERE SNO=I.RESNO)='G' AND I.STONEUNIT ='C' THEN (SUM(ISNULL(I.NETWT,0))/5)*(-1) "
                    StrSql += vbCrLf + " WHEN (SELECT STONEUNIT FROM " & cnStockDb & "..RECEIPT WHERE SNO=I.RESNO)='C' AND I.STONEUNIT ='G' THEN (SUM(ISNULL(I.NETWT,0))*5)*(-1) "
                    StrSql += vbCrLf + " ELSE SUM(ISNULL(I.NETWT,0))*(-1) END NETWT"
                    StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE  I "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = I.ITEMID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = I.SUBITEMID"
                    StrSql += vbCrLf + " WHERE I.TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "'"
                    StrSql += vbCrLf + " AND LEN(I.TRANTYPE)>2"
                    StrSql += vbCrLf + " AND I.TRANTYPE<>'IIN'"
                    'StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    If MCostId <> "" Then StrSql += vbCrLf + " AND I.COSTID='" & MCostId & "'"
                    StrSql += vbCrLf + " AND M.TTYPE='S'"
                    StrSql += vbCrLf + " AND RESNO IN"
                    StrSql += vbCrLf + " (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "' "
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')='' AND LEN(TRANTYPE)>2 AND TRANTYPE<>'RIN' "
                    If MCostId <> "" Then StrSql += vbCrLf + " AND COSTID='" & MCostId & "'"
                    StrSql += vbCrLf + " )"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
                    StrSql += vbCrLf + " GROUP BY RESNO,M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME,I.STONEUNIT  "
                    StrSql += vbCrLf + " UNION ALL"
                    StrSql += vbCrLf + " SELECT RECSNO AS SNO,M.METALNAME AS METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                    StrSql += vbCrLf + " ,SUM(ISNULL(PCS,0))*(-1) PCS"
                    StrSql += vbCrLf + " ,SUM(ISNULL(I.GRSWT,0))*(-1) GRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0))*(-1) NETWT"
                    StrSql += vbCrLf + " FROM " & cnStockDb & "..LOTISSUE  I "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = I.ITEMID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON IM.METALID = M.METALID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = I.SUBITEMID"
                    StrSql += vbCrLf + " WHERE  "
                    StrSql += vbCrLf + " RECSNO IN"
                    StrSql += vbCrLf + " (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "' AND ISNULL(CANCEL,'')=''"
                    StrSql += vbCrLf + " AND LEN(TRANTYPE)>2 AND TRANTYPE<>'RIN' "
                    StrSql += vbCrLf + " )"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
                    StrSql += vbCrLf + " GROUP BY RECSNO,M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME  "
                    StrSql += vbCrLf + " )X GROUP BY SNO,METAL,ITEM,SUBITEM  HAVING (SUM(GRSWT)>0 OR SUM(PCS)>0)"
                    StrSql += vbCrLf + " ORDER BY CONVERT(INT,SUBSTRING(SNO,7,LEN(SNO))) ASC"
                ElseIf STOCKVALIDATION And STOCKVALIDATION_MR = False And oMaterial = Material.Receipt Then
                    Dim sql As String = ""
                    StrSql = vbCrLf + "  SELECT TRANNO"
                    StrSql += vbCrLf + " ,TRANDATE,ITEM,SUBITEM,PCS,UNIT,CALC,WEIGHT,RATE,AMOUNT"
                    StrSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = X.CATCODE)CATNAME"
                    StrSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = X.METALID)METAL,SNO,''STNTYPE "
                    StrSql += vbCrLf + " FROM("
                    StrSql += vbCrLf + " SELECT I.TRANNO,I.TRANDATE,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM"
                    StrSql += vbCrLf + " ,PCS-"
                    StrSql += vbCrLf + " (SELECT SUM(PCS)PCS FROM ("
                    StrSql += vbCrLf + " SELECT ISNULL(SUM(PCS),0)PCS FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(ISSNO,'')=I.SNO AND ISNULL(CANCEL,'')='' "
                    StrSql += vbCrLf + " UNION ALL "
                    StrSql += vbCrLf + " SELECT ISNULL(SUM(STNPCS),0)PCS FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISNULL(JOBISNO,'')=I.SNO AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE  ISNULL(CANCEL,'')='') )X"
                    StrSql += vbCrLf + " ) AS PCS"
                    StrSql += vbCrLf + " ,I.STONEUNIT UNIT,"
                    StrSql += vbCrLf + " CASE WHEN ISNULL(I.WEIGHTUNIT,'')='' THEN 'W' ELSE I.WEIGHTUNIT END CALC"
                    StrSql += vbCrLf + " ,PUREWT-"
                    StrSql += vbCrLf + " (SELECT SUM(PUREWT)PUREWT FROM ("
                    StrSql += vbCrLf + " SELECT ISNULL(SUM(PUREWT),0)PUREWT FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(ISSNO,'')=I.SNO  AND ISNULL(CANCEL,'')=''"
                    StrSql += vbCrLf + " UNION ALL "
                    StrSql += vbCrLf + " SELECT ISNULL(SUM(STNWT),0)PUREWT FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISNULL(JOBISNO,'')=I.SNO AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE  ISNULL(CANCEL,'')='') )X"
                    StrSql += vbCrLf + " ) AS WEIGHT"
                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) RATE,"
                    StrSql += vbCrLf + " CONVERT(NUMERIC(15,2),NULL) AMOUNT,I.METALID,I.CATCODE,I.SNO"
                    StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I INNER JOIN " & cnAdminDb & "..ITEMMAST IM ON I.ITEMID=IM.ITEMID "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.ITEMID =I.ITEMID AND SI.SUBITEMID =I.SUBITEMID "
                    StrSql += vbCrLf + " WHERE LEN(I.TRANTYPE)>2  "
                    StrSql += vbCrLf + " AND I.TRANTYPE<>'IIN' AND I.TRANTYPE ='IIS'"
                    StrSql += vbCrLf + " And ISNULL(STUDDED,'') IN ('S','L','B') AND ISNULL(I.CANCEL,'')='' "
                    StrSql += vbCrLf + " AND I.ACCODE='" & Maccode & "'"
                    StrSql += vbCrLf + " ) X WHERE ISNULL(PCS,0)<>0 OR ISNULL(WEIGHT,0)<>0 "
                    sql = " SELECT count(*) FROM " & cnAdminDb & "..DBMASTER "
                    Dim prevdb As String = cnStockDb
                    Dim cnt As Integer = (objGPack.GetSqlValue(sql))
                    For X As Integer = 0 To cnt - 1
                        sql = " SELECT STARTDATE-1 FROM " & cnAdminDb & "..DBMASTER WHERE DBNAME = '" & prevdb & "'"
                        Dim menddate As Date = (objGPack.GetSqlValue(sql))
                        sql = " SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER WHERE '" & menddate & "' BETWEEN STARTDATE AND ENDDATE"
                        prevdb = objGPack.GetSqlValue(sql)
                        sql = " SELECT 'EXISTS' FROM MASTER..SYSDATABASES WHERE NAME = '" & prevdb & "'"
                        If objGPack.GetSqlValue(sql, , "-1") <> "-1" Then
                            StrSql += vbCrLf + "  UNION ALL"
                            StrSql += vbCrLf + "  SELECT TRANNO"
                            StrSql += vbCrLf + " ,TRANDATE,ITEM,SUBITEM,PCS,UNIT,CALC,WEIGHT,RATE,AMOUNT"
                            StrSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = X.CATCODE)CATNAME"
                            StrSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = X.METALID)METAL,SNO,''STNTYPE "
                            StrSql += vbCrLf + " FROM("
                            StrSql += vbCrLf + " SELECT I.TRANNO,I.TRANDATE,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM"
                            StrSql += vbCrLf + " ,PCS-"
                            StrSql += vbCrLf + " (SELECT SUM(PCS)PCS FROM ("
                            StrSql += vbCrLf + " SELECT ISNULL(SUM(PCS),0)PCS FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO AND ISNULL(CANCEL,'')='' "
                            StrSql += vbCrLf + " UNION ALL "
                            StrSql += vbCrLf + " SELECT ISNULL(SUM(STNPCS),0)PCS FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISNULL(JOBISNO,'')=I.SNO AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO  AND ISNULL(CANCEL,'')='') )X"
                            StrSql += vbCrLf + " ) AS PCS"
                            StrSql += vbCrLf + " ,I.STONEUNIT UNIT,"
                            StrSql += vbCrLf + " CASE WHEN ISNULL(I.WEIGHTUNIT,'')='' THEN 'W' ELSE I.WEIGHTUNIT END CALC"
                            StrSql += vbCrLf + " ,PUREWT-"
                            StrSql += vbCrLf + " (SELECT SUM(PUREWT)PUREWT FROM ("
                            StrSql += vbCrLf + " SELECT ISNULL(SUM(PUREWT),0)PUREWT FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO  AND ISNULL(CANCEL,'')=''"
                            StrSql += vbCrLf + " UNION ALL "
                            StrSql += vbCrLf + " SELECT ISNULL(SUM(STNWT),0)PUREWT FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISNULL(JOBISNO,'')=I.SNO AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO  AND ISNULL(CANCEL,'')='') )X"
                            StrSql += vbCrLf + " ) AS WEIGHT"
                            StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) RATE,"
                            StrSql += vbCrLf + " CONVERT(NUMERIC(15,2),NULL) AMOUNT,I.METALID,I.CATCODE,I.SNO"
                            StrSql += vbCrLf + " FROM " & prevdb & "..ISSUE I INNER JOIN " & cnAdminDb & "..ITEMMAST IM ON I.ITEMID=IM.ITEMID "
                            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.ITEMID =I.ITEMID AND SI.SUBITEMID =I.SUBITEMID "
                            StrSql += vbCrLf + " WHERE LEN(I.TRANTYPE)>2  "
                            StrSql += vbCrLf + " AND I.TRANTYPE<>'IIN' AND I.TRANTYPE ='IIS'"
                            StrSql += vbCrLf + " And ISNULL(STUDDED,'') IN ('S','L','B') AND ISNULL(I.CANCEL,'')='' "
                            StrSql += vbCrLf + " AND I.ACCODE='" & Maccode & "'"
                            StrSql += vbCrLf + " ) X WHERE ISNULL(PCS,0)<>0 OR ISNULL(WEIGHT,0)<>0 "
                        End If
                    Next
                ElseIf STOCKVALIDATION_MR And oMaterial = Material.Receipt Then
                    GoTo reciss
                End If
                If StrSql = "" Then txtSOrdNo.SelectAll() : Exit Sub
                Dim dtStk As New DataTable
                Cmd = New OleDbCommand(StrSql, cn)
                Da = New OleDbDataAdapter(Cmd)
                Da.Fill(dtStk)
                Dim StkRow As DataRow
                If dtStk.Rows.Count > 0 Then
                    StkRow = BrighttechPack.SearchDialog.Show_R("Find ", StrSql, cn, , 0)
                    If Not StkRow Is Nothing Then
                        If STOCKVALIDATION And STOCKVALIDATION_MR = False And oMaterial = Material.Receipt Then
                            txtSOrdNo.Text = StkRow.Item("TRANNO").ToString
                            InsSno = StkRow.Item("SNO").ToString
                            cmbSMetal.Text = StkRow.Item("METAL").ToString
                        Else
                            txtSOrdNo.Text = StkRow.Item("TRANNO").ToString
                            InsSno = StkRow.Item("SNO").ToString
                            cmbSMetal.Text = StkRow.Item("METAL").ToString
                        End If

                    End If
                Else
                    MsgBox("Record Not Found", MsgBoxStyle.Information)
                End If
                txtSOrdNo.SelectAll()
            ElseIf STOCKVALIDATION_MR Then
reciss:
                InsSno = ""
                If oMaterial = Material.Receipt Then
                    StrSql = vbCrLf + " SELECT "
                    StrSql += vbCrLf + " (SELECT TRANNO FROM " & cnStockDb & "..ISSUE WHERE SNO=X.SNO)TRANNO"
                    StrSql += vbCrLf + " ,(SELECT TRANDATE FROM " & cnStockDb & "..ISSUE WHERE SNO=X.SNO)TRANDATE"
                    StrSql += vbCrLf + " ,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE="
                    StrSql += vbCrLf + " (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ISSUE WHERE SNO=X.SNO))ACNAME"
                    StrSql += vbCrLf + " ,METAL,ITEM,SUBITEM"
                    StrSql += vbCrLf + " ,(SELECT STONEUNIT FROM " & cnStockDb & "..ISSUE WHERE SNO=X.SNO)UNIT"
                    StrSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT"
                    StrSql += vbCrLf + " ,SNO"
                    StrSql += vbCrLf + " FROM"
                    StrSql += vbCrLf + " ("
                    StrSql += vbCrLf + " SELECT SNO,M.METALNAME METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                    StrSql += vbCrLf + " ,SUM(ISNULL(PCS,0)) PCS"
                    StrSql += vbCrLf + " ,SUM(ISNULL(R.GRSWT,0)) GRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) NETWT"
                    StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE R "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON R.METALID = M.METALID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = R.ITEMID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = R.SUBITEMID"
                    StrSql += vbCrLf + " WHERE R.TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "'"
                    StrSql += vbCrLf + " AND LEN(R.TRANTYPE)>2"
                    StrSql += vbCrLf + " AND R.TRANTYPE<>'RIN'"
                    'StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    If MCostId <> "" Then StrSql += vbCrLf + " AND R.COSTID='" & MCostId & "'"
                    StrSql += vbCrLf + " AND M.TTYPE='S'"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
                    '09April2022
                    ''StrSql += vbCrLf + " AND SNO NOT IN"
                    ''StrSql += vbCrLf + " (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "' "
                    ''StrSql += vbCrLf + " AND ISNULL(CANCEL,'')='' AND LEN(TRANTYPE)>2 AND TRANTYPE<>'RIN' "
                    ''If MCostId <> "" Then StrSql += vbCrLf + " AND COSTID='" & MCostId & "'"
                    ''StrSql += vbCrLf + " )"
                    StrSql += vbCrLf + " GROUP BY SNO,M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME  "
                    StrSql += vbCrLf + " UNION ALL"
                    ''StrSql += vbCrLf + " SELECT ISSNO AS SNO,M.METALNAME AS METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                    StrSql += vbCrLf + " SELECT ISSNO AS SNO,M.METALNAME AS METAL "
                    StrSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY where catcode =(SELECT CATCODE FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSNO)) CATEGORY"
                    StrSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST where ITEMID =(SELECT ITEMID FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSNO)) ITEM"
                    StrSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST where SUBITEMID =(SELECT SUBITEMID FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSNO)) SUBITEM"
                    StrSql += vbCrLf + " ,SUM(ISNULL(PCS,0))*(-1) PCS"
                    StrSql += vbCrLf + " ,CASE "
                    StrSql += vbCrLf + " WHEN (SELECT STONEUNIT FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSNO)='G' AND I.STONEUNIT ='C' THEN (SUM(ISNULL(I.GRSWT,0))/5)*(-1) "
                    StrSql += vbCrLf + " WHEN (SELECT STONEUNIT FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSNO)='C' AND I.STONEUNIT ='G' THEN (SUM(ISNULL(I.GRSWT,0))*5)*(-1) "
                    StrSql += vbCrLf + " ELSE SUM(ISNULL(I.GRSWT,0))*(-1) END GRSWT"
                    StrSql += vbCrLf + " ,CASE "
                    StrSql += vbCrLf + " WHEN (SELECT STONEUNIT FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSNO)='G' AND I.STONEUNIT ='C' THEN (SUM(ISNULL(I.NETWT,0))/5)*(-1) "
                    StrSql += vbCrLf + " WHEN (SELECT STONEUNIT FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSNO)='C' AND I.STONEUNIT ='G' THEN (SUM(ISNULL(I.NETWT,0))*5)*(-1) "
                    StrSql += vbCrLf + " ELSE SUM(ISNULL(I.NETWT,0))*(-1) END NETWT"
                    StrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT  I "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = I.ITEMID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = I.SUBITEMID"
                    StrSql += vbCrLf + " WHERE I.TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "'"
                    StrSql += vbCrLf + " AND LEN(I.TRANTYPE)>2"
                    StrSql += vbCrLf + " AND I.TRANTYPE<>'IIN'"
                    'StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    If MCostId <> "" Then StrSql += vbCrLf + " AND I.COSTID='" & MCostId & "'"
                    StrSql += vbCrLf + " AND M.TTYPE='S'"
                    StrSql += vbCrLf + " AND ISSNO IN"
                    StrSql += vbCrLf + " (SELECT SNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "' "
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')='' AND LEN(TRANTYPE)>2 AND TRANTYPE<>'RIN' "
                    If MCostId <> "" Then StrSql += vbCrLf + " AND COSTID='" & MCostId & "'"
                    StrSql += vbCrLf + " )"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
                    '09april2022
                    ''StrSql += vbCrLf + " AND ISSNO NOT IN"
                    ''StrSql += vbCrLf + " (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "' "
                    ''StrSql += vbCrLf + " AND ISNULL(CANCEL,'')='' AND LEN(TRANTYPE)>2 AND TRANTYPE<>'RIN' "
                    ''If MCostId <> "" Then StrSql += vbCrLf + " AND COSTID='" & MCostId & "'"
                    ''StrSql += vbCrLf + " )"
                    StrSql += vbCrLf + " GROUP BY ISSNO,M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME,I.STONEUNIT  "
                    StrSql += vbCrLf + " UNION ALL"
                    StrSql += vbCrLf + " SELECT RECSNO AS SNO,M.METALNAME AS METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                    StrSql += vbCrLf + " ,SUM(ISNULL(PCS,0))*(-1) PCS"
                    StrSql += vbCrLf + " ,SUM(ISNULL(I.GRSWT,0))*(-1) GRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0))*(-1) NETWT"
                    StrSql += vbCrLf + " FROM " & cnStockDb & "..LOTISSUE  I "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = I.ITEMID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON IM.METALID = M.METALID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = I.SUBITEMID"
                    StrSql += vbCrLf + " WHERE  "
                    StrSql += vbCrLf + " RECSNO IN"
                    StrSql += vbCrLf + " (SELECT SNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "' AND ISNULL(CANCEL,'')=''"
                    StrSql += vbCrLf + " AND LEN(TRANTYPE)>2 AND TRANTYPE<>'RIN' "
                    StrSql += vbCrLf + " )"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
                    StrSql += vbCrLf + " GROUP BY RECSNO,M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME  "
                    StrSql += vbCrLf + " )X GROUP BY SNO,METAL,ITEM,SUBITEM  HAVING (SUM(GRSWT)>0 OR SUM(PCS)>0)"
                    StrSql += vbCrLf + " ORDER BY CONVERT(INT,SUBSTRING(SNO,7,LEN(SNO))) ASC"
                End If
                If StrSql = "" Then txtSOrdNo.SelectAll() : Exit Sub
                Dim dtStk As New DataTable
                Cmd = New OleDbCommand(StrSql, cn)
                Da = New OleDbDataAdapter(Cmd)
                Da.Fill(dtStk)
                Dim StkRow As DataRow
                If dtStk.Rows.Count > 0 Then
                    StkRow = BrighttechPack.SearchDialog.Show_R("Find ", StrSql, cn, , 0)
                    If Not StkRow Is Nothing Then
                        txtSOrdNo.Text = StkRow.Item("TRANNO").ToString
                        InsSno = StkRow.Item("SNO").ToString
                        cmbSMetal.Text = StkRow.Item("METAL").ToString
                    End If
                Else
                    MsgBox("Record Not Found", MsgBoxStyle.Information)
                End If
                txtSOrdNo.SelectAll()
            Else
                If oMaterial = Material.Issue Then
                    StrSql = vbCrLf + " SELECT METAL,JOBNO,SUM(RPCS) [REC PCS],SUM(RGRSWT) [REC GRSWT],SUM(RNETWT) [REC NETWT],SUM(IPCS) [ISS PCS],SUM(IGRSWT) [ISS GRSWT],SUM(INETWT) [ISS NETWT]  FROM"
                    StrSql += vbCrLf + " ("
                    StrSql += vbCrLf + " SELECT M.METALNAME METAL,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,SUM(ISNULL(PCS,0)) RPCS,SUM(ISNULL(GRSWT,0)) RGRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) RNETWT,0 IPCS,0 IGRSWT,0 INETWT  FROM " & cnStockDb & "..RECEIPT R "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON R.METALID = M.METALID"
                    StrSql += vbCrLf + " WHERE TRANTYPE IN('RRE','RPU') AND ISNULL(JOBNO,'')<>'' "
                    StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
                    StrSql += vbCrLf + " GROUP BY M.METALNAME,JOBNO "
                    StrSql += vbCrLf + " UNION ALL"
                    StrSql += vbCrLf + " SELECT M.METALNAME AS METAL,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,0 IPCS,0 IGRSWT,0 INETWT,SUM(ISNULL(PCS,0)) IPCS,SUM(ISNULL(GRSWT,0)) IGRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) INETWT  FROM " & cnStockDb & "..ISSUE  I "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
                    StrSql += vbCrLf + " WHERE TRANTYPE IN('IIS','IPU','IRC') AND ISNULL(JOBNO,'')<>'' "
                    StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
                    StrSql += vbCrLf + " GROUP BY M.METALNAME,JOBNO "
                    StrSql += vbCrLf + " )X GROUP BY METAL,JOBNO HAVING SUM(IGRSWT-RGRSWT) <>0 "
                    StrSql += vbCrLf + " ORDER BY JOBNO,METAL"

                    'StrSql = vbCrLf + " SELECT METAL,JOBNO,SUM(PCS)PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT FROM"
                    'StrSql += vbCrLf + " ("
                    'StrSql += vbCrLf + " SELECT M.METALNAME METAL,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,SUM(ISNULL(PCS,0)) PCS,SUM(ISNULL(GRSWT,0)) GRSWT "
                    'StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) NETWT  FROM " & cnStockDb & "..RECEIPT R "
                    'StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON R.METALID = M.METALID"
                    'StrSql += vbCrLf + " WHERE TRANTYPE IN('RRE','RPU') AND ISNULL(JOBNO,'')<>'' "
                    'StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    'StrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
                    'StrSql += vbCrLf + " GROUP BY M.METALNAME,JOBNO "
                    'StrSql += vbCrLf + " UNION ALL"
                    'StrSql += vbCrLf + " SELECT M.METALNAME AS METAL,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,SUM(ISNULL(PCS,0))*(-1) PCS,SUM(ISNULL(GRSWT,0))*(-1) GRSWT "
                    'StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0))*(-1) NETWT  FROM " & cnStockDb & "..ISSUE  I "
                    'StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
                    'StrSql += vbCrLf + " WHERE TRANTYPE IN('IIS','IPU','IRC','ISS') AND ISNULL(JOBNO,'')<>'' "
                    'StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    'StrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
                    'StrSql += vbCrLf + " GROUP BY M.METALNAME,JOBNO "
                    'StrSql += vbCrLf + " )X GROUP BY METAL,JOBNO HAVING SUM(GRSWT)<>0 "
                    'StrSql += vbCrLf + " ORDER BY JOBNO,METAL"
                ElseIf oMaterial = Material.Receipt Then
                    StrSql = vbCrLf + " SELECT METAL,JOBNO,SUM(IPCS)[ISSUE PCS],SUM(IGRSWT) [ISS GRSWT],SUM(INETWT) [ISS NETWT],"
                    StrSql += vbCrLf + "SUM(RPCS)[REC PCS],SUM(RGRSWT) [REC GRSWT],SUM(RNETWT) [REC NETWT] FROM"
                    StrSql += vbCrLf + " ("
                    StrSql += vbCrLf + " SELECT M.METALNAME AS METAL,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,SUM(ISNULL(PCS,0)) IPCS,SUM(ISNULL(GRSWT,0)) IGRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) INETWT,0 RPCS,0 RGRSWT,0 RNETWT  FROM " & cnStockDb & "..ISSUE I"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
                    StrSql += vbCrLf + " WHERE TRANTYPE IN('IIS','IPU','IRC','ISS') AND ISNULL(JOBNO,'')<>'' "
                    StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
                    StrSql += vbCrLf + " GROUP BY METALNAME,JOBNO "
                    StrSql += vbCrLf + " UNION ALL"
                    StrSql += vbCrLf + " SELECT M.METALNAME AS METAL,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,0 IPCS,0 IGRSWT,0 INETWT,SUM(ISNULL(PCS,0)) RPCS,SUM(ISNULL(GRSWT,0)) RGRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) RNETWT  FROM " & cnStockDb & "..RECEIPT R "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON R.METALID = M.METALID"
                    StrSql += vbCrLf + " WHERE TRANTYPE IN('RRE','RPU') AND ISNULL(JOBNO,'')<>'' "
                    StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
                    StrSql += vbCrLf + " GROUP BY METALNAME,JOBNO "
                    StrSql += vbCrLf + " )X GROUP BY METAL,JOBNO HAVING SUM(IGRSWT-RGRSWT)<>0 "
                    StrSql += vbCrLf + " ORDER BY JOBNO,METAL"

                    'StrSql = vbCrLf + " SELECT METAL,JOBNO,SUM(PCS)PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT FROM"
                    'StrSql += vbCrLf + " ("
                    'StrSql += vbCrLf + " SELECT M.METALNAME AS METAL,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,SUM(ISNULL(PCS,0)) PCS,SUM(ISNULL(GRSWT,0)) GRSWT "
                    'StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) NETWT  FROM " & cnStockDb & "..ISSUE I"
                    'StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
                    'StrSql += vbCrLf + " WHERE TRANTYPE IN('IIS','IPU','IRC','ISS') AND ISNULL(JOBNO,'')<>'' "
                    'StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    'StrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
                    'StrSql += vbCrLf + " GROUP BY METALNAME,JOBNO "
                    'StrSql += vbCrLf + " UNION ALL"
                    'StrSql += vbCrLf + " SELECT M.METALNAME AS METAL,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,SUM(ISNULL(PCS,0))*(-1) PCS,SUM(ISNULL(GRSWT,0))*(-1) GRSWT "
                    'StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0))*(-1) NETWT  FROM " & cnStockDb & "..RECEIPT R "
                    'StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON R.METALID = M.METALID"
                    'StrSql += vbCrLf + " WHERE TRANTYPE IN('RRE','RPU') AND ISNULL(JOBNO,'')<>'' "
                    'StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    'StrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
                    'StrSql += vbCrLf + " GROUP BY METALNAME,JOBNO "
                    'StrSql += vbCrLf + " )X GROUP BY METAL,JOBNO HAVING SUM(GRSWT)<>0 "
                    'StrSql += vbCrLf + " ORDER BY JOBNO,METAL"
                End If
                Dim JnoRow As DataRow
                JnoRow = BrighttechPack.SearchDialog.Show_R("Find JobNo", StrSql, cn, , 1, , , , , , False)
                If Not JnoRow Is Nothing Then
                    txtSOrdNo.Text = JnoRow.Item("JOBNO").ToString
                    cmbSMetal.Text = JnoRow.Item("METAL").ToString
                End If
                txtSOrdNo.SelectAll()
            End If
        End If
    End Sub

    Private Sub txtMJobNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMOrdNo.KeyDown
        If e.KeyCode = Keys.Insert Then
            StrSql = ""
            If STOCKVALIDATION Then
                InsSno = ""
                If oMaterial = Material.Issue Then
                    StrSql = vbCrLf + " SELECT "
                    StrSql += vbCrLf + " (SELECT TRANNO FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)TRANNO"
                    StrSql += vbCrLf + " ,(SELECT CONVERT(VARCHAR,TRANDATE,105) FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)TRANDATE"
                    StrSql += vbCrLf + " ,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE="
                    StrSql += vbCrLf + " (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO))ACNAME"
                    StrSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT"
                    StrSql += vbCrLf + " ,SNO"
                    StrSql += vbCrLf + " FROM"
                    StrSql += vbCrLf + " ("
                    StrSql += vbCrLf + " SELECT SNO,M.METALNAME METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                    StrSql += vbCrLf + " ,SUM(ISNULL(PCS,0)) PCS"
                    StrSql += vbCrLf + " ,SUM(ISNULL(R.GRSWT,0)) GRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) NETWT"
                    StrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT R "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON R.METALID = M.METALID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = R.ITEMID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = R.SUBITEMID"
                    StrSql += vbCrLf + " WHERE R.TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "'"
                    StrSql += vbCrLf + " AND LEN(R.TRANTYPE)>2"
                    StrSql += vbCrLf + " AND R.TRANTYPE<>'RIN'"
                    'StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    If MCostId <> "" Then StrSql += vbCrLf + " AND R.COSTID='" & MCostId & "'"
                    StrSql += vbCrLf + " AND C.PURITYID IN(SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALTYPE='M')"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
                    StrSql += vbCrLf + " GROUP BY SNO,M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME  "
                    StrSql += vbCrLf + " UNION ALL"
                    StrSql += vbCrLf + " SELECT RESNO AS SNO,M.METALNAME AS METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                    StrSql += vbCrLf + " ,SUM(ISNULL(PCS,0))*(-1) PCS"
                    StrSql += vbCrLf + " ,SUM(ISNULL(I.GRSWT,0))*(-1) GRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0))*(-1) NETWT"
                    StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE  I "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = I.ITEMID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = I.SUBITEMID"
                    StrSql += vbCrLf + " WHERE I.TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "'"
                    StrSql += vbCrLf + " AND LEN(I.TRANTYPE)>2"
                    StrSql += vbCrLf + " AND I.TRANTYPE<>'IIN'"
                    'StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    If MCostId <> "" Then StrSql += vbCrLf + " AND I.COSTID='" & MCostId & "'"
                    StrSql += vbCrLf + " AND C.PURITYID IN(SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALTYPE='M')"
                    StrSql += vbCrLf + " AND RESNO IN"
                    StrSql += vbCrLf + " (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "' AND ISNULL(CANCEL,'')=''"
                    If MCostId <> "" Then StrSql += vbCrLf + " AND COSTID='" & MCostId & "'"
                    StrSql += vbCrLf + " AND LEN(TRANTYPE)>2 AND TRANTYPE<>'RIN' "
                    StrSql += vbCrLf + " )"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
                    StrSql += vbCrLf + " GROUP BY RESNO,M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME  "
                    StrSql += vbCrLf + " )X GROUP BY SNO   HAVING (SUM(GRSWT)>0 OR SUM(PCS)>0)"
                    StrSql += vbCrLf + " ORDER BY CONVERT(INT,SUBSTRING(SNO,7,LEN(SNO))) ASC"
                ElseIf STOCKVALIDATION_MR And oMaterial = Material.Receipt Then
                    GoTo reciss
                End If
                Dim dtStk As New DataTable
                If StrSql = "" Then txtMOrdNo.SelectAll() : Exit Sub
                Cmd = New OleDbCommand(StrSql, cn)
                Da = New OleDbDataAdapter(Cmd)
                Da.Fill(dtStk)
                Dim StkRow As DataRow
                If dtStk.Rows.Count > 0 Then
                    StkRow = BrighttechPack.SearchDialog.Show_R("Find ", StrSql, cn, , 0)
                    If Not StkRow Is Nothing Then
                        txtMOrdNo.Text = StkRow.Item("TRANNO").ToString
                        InsSno = StkRow.Item("SNO").ToString
                    End If
                Else
                    MsgBox("Record Not Found", MsgBoxStyle.Information)
                End If
                txtMOrdNo.SelectAll()
            ElseIf STOCKVALIDATION_MR Then
reciss:
                InsSno = ""
                If oMaterial = Material.Receipt Then
                    StrSql = vbCrLf + " SELECT "
                    StrSql += vbCrLf + " (SELECT TRANNO FROM " & cnStockDb & "..ISSUE WHERE SNO=X.SNO)TRANNO"
                    StrSql += vbCrLf + " ,(SELECT CONVERT(VARCHAR,TRANDATE,105) FROM " & cnStockDb & "..ISSUE WHERE SNO=X.SNO)TRANDATE"
                    StrSql += vbCrLf + " ,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE="
                    StrSql += vbCrLf + " (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ISSUE WHERE SNO=X.SNO))ACNAME"
                    StrSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT"
                    StrSql += vbCrLf + " ,SNO"
                    StrSql += vbCrLf + " FROM"
                    StrSql += vbCrLf + " ("
                    StrSql += vbCrLf + " SELECT SNO,M.METALNAME METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                    StrSql += vbCrLf + " ,SUM(ISNULL(PCS,0)) PCS"
                    StrSql += vbCrLf + " ,SUM(ISNULL(R.GRSWT,0)) GRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) NETWT"
                    StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE R "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON R.METALID = M.METALID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = R.ITEMID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = R.SUBITEMID"
                    StrSql += vbCrLf + " WHERE R.TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "'"
                    StrSql += vbCrLf + " AND LEN(R.TRANTYPE)>2"
                    StrSql += vbCrLf + " AND R.TRANTYPE<>'RIN'"
                    'StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    If MCostId <> "" Then StrSql += vbCrLf + " AND R.COSTID='" & MCostId & "'"
                    StrSql += vbCrLf + " AND C.PURITYID IN(SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALTYPE='M')"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
                    StrSql += vbCrLf + " GROUP BY SNO,M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME  "
                    StrSql += vbCrLf + " UNION ALL"
                    StrSql += vbCrLf + " SELECT ISSNO AS SNO,M.METALNAME AS METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                    StrSql += vbCrLf + " ,SUM(ISNULL(PCS,0))*(-1) PCS"
                    StrSql += vbCrLf + " ,SUM(ISNULL(I.GRSWT,0))*(-1) GRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0))*(-1) NETWT"
                    StrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT  I "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = I.ITEMID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = I.SUBITEMID"
                    StrSql += vbCrLf + " WHERE I.TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "'"
                    StrSql += vbCrLf + " AND LEN(I.TRANTYPE)>2"
                    StrSql += vbCrLf + " AND I.TRANTYPE<>'IIN'"
                    'StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    If MCostId <> "" Then StrSql += vbCrLf + " AND I.COSTID='" & MCostId & "'"
                    StrSql += vbCrLf + " AND C.PURITYID IN(SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALTYPE='M')"
                    StrSql += vbCrLf + " AND ISSNO IN"
                    StrSql += vbCrLf + " (SELECT SNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "' AND ISNULL(CANCEL,'')=''"
                    If MCostId <> "" Then StrSql += vbCrLf + " AND COSTID='" & MCostId & "'"
                    StrSql += vbCrLf + " AND LEN(TRANTYPE)>2 AND TRANTYPE<>'RIN' "
                    StrSql += vbCrLf + " )"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
                    StrSql += vbCrLf + " GROUP BY ISSNO,M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME  "
                    StrSql += vbCrLf + " )X GROUP BY SNO   HAVING (SUM(GRSWT)>0 OR SUM(PCS)>0)"
                    StrSql += vbCrLf + " ORDER BY CONVERT(INT,SUBSTRING(SNO,7,LEN(SNO))) ASC"
                End If
                Dim dtStk As New DataTable
                If StrSql = "" Then txtMOrdNo.SelectAll() : Exit Sub
                Cmd = New OleDbCommand(StrSql, cn)
                Da = New OleDbDataAdapter(Cmd)
                Da.Fill(dtStk)
                Dim StkRow As DataRow
                If dtStk.Rows.Count > 0 Then
                    StkRow = BrighttechPack.SearchDialog.Show_R("Find ", StrSql, cn, , 0)
                    If Not StkRow Is Nothing Then
                        txtMOrdNo.Text = StkRow.Item("TRANNO").ToString
                        InsSno = StkRow.Item("SNO").ToString
                    End If
                Else
                    MsgBox("Record Not Found", MsgBoxStyle.Information)
                End If
                txtMOrdNo.SelectAll()
            Else
                If oMaterial = Material.Issue Then
                    StrSql = vbCrLf + " SELECT METAL,JOBNO,SUM(RPCS) [REC PCS],SUM(RGRSWT) [REC GRSWT],SUM(RNETWT) [REC NETWT],SUM(IPCS) [ISS PCS],SUM(IGRSWT) [ISS GRSWT],SUM(INETWT) [ISS NETWT]  FROM"
                    StrSql += vbCrLf + " ("
                    StrSql += vbCrLf + " SELECT M.METALNAME METAL,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,SUM(ISNULL(PCS,0)) RPCS,SUM(ISNULL(GRSWT,0)) RGRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) RNETWT,0 IPCS,0 IGRSWT,0 INETWT  FROM " & cnStockDb & "..RECEIPT R "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON R.METALID = M.METALID"
                    StrSql += vbCrLf + " WHERE TRANTYPE IN('RRE','RPU') AND ISNULL(JOBNO,'')<>'' "
                    StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
                    StrSql += vbCrLf + " GROUP BY M.METALNAME,JOBNO "
                    StrSql += vbCrLf + " UNION ALL"
                    StrSql += vbCrLf + " SELECT M.METALNAME AS METAL,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,0 IPCS,0 IGRSWT,0 INETWT,SUM(ISNULL(PCS,0)) IPCS,SUM(ISNULL(GRSWT,0)) IGRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) INETWT  FROM " & cnStockDb & "..ISSUE  I "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
                    StrSql += vbCrLf + " WHERE TRANTYPE IN('IIS','IPU','IRC') AND ISNULL(JOBNO,'')<>'' "
                    StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
                    StrSql += vbCrLf + " GROUP BY M.METALNAME,JOBNO "
                    StrSql += vbCrLf + " )X GROUP BY METAL,JOBNO HAVING SUM(IGRSWT-RGRSWT) <>0 "
                    StrSql += vbCrLf + " ORDER BY JOBNO,METAL"


                    'StrSql = vbCrLf + " SELECT METAL,JOBNO,SUM(PCS)PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT FROM"
                    'StrSql += vbCrLf + " ("
                    'StrSql += vbCrLf + " SELECT M.METALNAME METAL,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,SUM(ISNULL(PCS,0)) PCS,SUM(ISNULL(GRSWT,0)) GRSWT "
                    'StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) NETWT  FROM " & cnStockDb & "..RECEIPT R "
                    'StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON R.METALID = M.METALID"
                    'StrSql += vbCrLf + " WHERE TRANTYPE IN('RRE','RPU') AND ISNULL(JOBNO,'')<>'' "
                    'StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    'StrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
                    'StrSql += vbCrLf + " GROUP BY M.METALNAME,JOBNO "
                    'StrSql += vbCrLf + " UNION ALL"
                    'StrSql += vbCrLf + " SELECT M.METALNAME AS METAL,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,SUM(ISNULL(PCS,0))*(-1) PCS,SUM(ISNULL(GRSWT,0))*(-1) GRSWT "
                    'StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0))*(-1) NETWT  FROM " & cnStockDb & "..ISSUE  I "
                    'StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
                    'StrSql += vbCrLf + " WHERE TRANTYPE IN('IIS','IPU','IRC','ISS') AND ISNULL(JOBNO,'')<>'' "
                    'StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    'StrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
                    'StrSql += vbCrLf + " GROUP BY M.METALNAME,JOBNO "
                    'StrSql += vbCrLf + " )X GROUP BY METAL,JOBNO HAVING SUM(GRSWT)<>0 "
                    'StrSql += vbCrLf + " ORDER BY JOBNO,METAL"
                ElseIf oMaterial = Material.Receipt Then
                    StrSql = vbCrLf + " SELECT METAL,JOBNO,SUM(IPCS)[ISSUE PCS],SUM(IGRSWT) [ISS GRSWT],SUM(INETWT) [ISS NETWT],"
                    StrSql += vbCrLf + "SUM(RPCS)[REC PCS],SUM(RGRSWT) [REC GRSWT],SUM(RNETWT) [REC NETWT] FROM"
                    StrSql += vbCrLf + " ("
                    StrSql += vbCrLf + " SELECT M.METALNAME AS METAL,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,SUM(ISNULL(PCS,0)) IPCS,SUM(ISNULL(GRSWT,0)) IGRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) INETWT,0 RPCS,0 RGRSWT,0 RNETWT  FROM " & cnStockDb & "..ISSUE I"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
                    StrSql += vbCrLf + " WHERE TRANTYPE IN('IIS','IPU','IRC') AND ISNULL(JOBNO,'')<>'' "
                    StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
                    StrSql += vbCrLf + " GROUP BY METALNAME,JOBNO "
                    StrSql += vbCrLf + " UNION ALL"
                    StrSql += vbCrLf + " SELECT M.METALNAME AS METAL,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,0 IPCS,0 IGRSWT,0 INETWT,SUM(ISNULL(PCS,0)) RPCS,SUM(ISNULL(GRSWT,0)) RGRSWT "
                    StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) RNETWT  FROM " & cnStockDb & "..RECEIPT R "
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON R.METALID = M.METALID"
                    StrSql += vbCrLf + " WHERE TRANTYPE IN('RRE','RPU') AND ISNULL(JOBNO,'')<>'' "
                    StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    StrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
                    StrSql += vbCrLf + " GROUP BY METALNAME,JOBNO "
                    StrSql += vbCrLf + " )X GROUP BY METAL,JOBNO HAVING SUM(IGRSWT-RGRSWT)<>0 "
                    StrSql += vbCrLf + " ORDER BY JOBNO,METAL"

                    'StrSql = vbCrLf + " SELECT METAL,JOBNO,SUM(PCS)PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT FROM"
                    'StrSql += vbCrLf + " ("
                    'StrSql += vbCrLf + " SELECT M.METALNAME AS METAL,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,SUM(ISNULL(PCS,0)) PCS,SUM(ISNULL(GRSWT,0)) GRSWT "
                    'StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) NETWT  FROM " & cnStockDb & "..ISSUE I"
                    'StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
                    'StrSql += vbCrLf + " WHERE TRANTYPE IN('IIS','IPU','IRC','ISS') AND ISNULL(JOBNO,'')<>'' "
                    'StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    'StrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
                    'StrSql += vbCrLf + " GROUP BY METALNAME,JOBNO "
                    'StrSql += vbCrLf + " UNION ALL"
                    'StrSql += vbCrLf + " SELECT M.METALNAME AS METAL,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,SUM(ISNULL(PCS,0))*(-1) PCS,SUM(ISNULL(GRSWT,0))*(-1) GRSWT "
                    'StrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0))*(-1) NETWT  FROM " & cnStockDb & "..RECEIPT R "
                    'StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON R.METALID = M.METALID"
                    'StrSql += vbCrLf + " WHERE TRANTYPE IN('RRE','RPU') AND ISNULL(JOBNO,'')<>'' "
                    'StrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                    'StrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
                    'StrSql += vbCrLf + " GROUP BY METALNAME,JOBNO "
                    'StrSql += vbCrLf + " )X GROUP BY METAL,JOBNO HAVING SUM(GRSWT)<>0 "
                    'StrSql += vbCrLf + " ORDER BY JOBNO,METAL"
                End If
                Dim JnoRow As DataRow
                JnoRow = BrighttechPack.SearchDialog.Show_R("Find JobNo", StrSql, cn, , 1, , , , , , False)
                If Not JnoRow Is Nothing Then
                    txtMOrdNo.Text = JnoRow.Item("JOBNO").ToString
                    cmbMMetal.Text = JnoRow.Item("METAL").ToString
                End If
                txtMOrdNo.SelectAll()
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            'SendKeys.Send("{TAB}")
            cmbMProcess.Focus()
        End If
    End Sub


    Private Sub txtJobNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles _
     txtOOrdNo.KeyPress, txtMOrdNo.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim StkAvail As Boolean = True
            If _JobNoEnable = False Then
                'FOR BOMBAY JEWELLERY 04-07-2015 ************
                'If CheckJobNo(CType(sender, Control)) = False Then
                '    MsgBox("Invalid Order Info" & vbCrLf & "This order no is already delivered or cancelled", MsgBoxStyle.Information)
                '    Exit Sub
                'End If
                '*****************
                If CType(sender, Control).Text <> "" Then
                    If CType(sender, Control).Text.StartsWith("O") Or CType(sender, Control).Text.StartsWith("R") Then
                        'If objOrderInfo Is Nothing Then
                        objOrderInfo = New MATERIALISSREC_ORDERINFO(CType(sender, Control).Text, BillCostId, IIf(oMaterial = Material.Issue, True, False), IIf(ORDER_MULTI_MIMR, Maccode, ""))
                        'objOrderInfo.BillCostId = BillCostId
                        'End If
                        If objOrderInfo.ShowDialog() <> Windows.Forms.DialogResult.OK Then
                            Exit Sub
                        End If
                        Dim TempOrSno As String = ""
                        For cnt As Integer = 0 To objOrderInfo.DgvOrder.Rows.Count - 1
                            If CType(objOrderInfo.DgvOrder.Rows(cnt).Cells("CHECK").Value, Boolean) = True Then
                                TempOrSno += "'" & objOrderInfo.DgvOrder.Rows(cnt).Cells("SNO").Value.ToString & "',"
                            End If
                        Next
                        If TempOrSno.Contains(",") = True Then
                            TempOrSno = TempOrSno.Substring(0, TempOrSno.Length - 1)
                        End If
                        Dim DtOrder As New DataTable
                        If MIMR_allowClosedOrder Then
                            StrSql = "SELECT M.METALNAME     AS METAL,ISNULL(CA.CATNAME,C.CATNAME) CATEGORY,IT.ITEMNAME ITEM,SIT.SUBITEMNAME SUBITEM,"
                            StrSql += vbCrLf + " O.PCS--ISNULL((SELECT SUM(isnull(PCS,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND  ORSTATUS = 'R' AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54),0) PCS"
                            StrSql += vbCrLf + " ,O.GRSWT--ISNULL((SELECT SUM(isnull(GRSWT,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND ORSTATUS = 'R'  AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54),0) GRSWT "
                            StrSql += vbCrLf + " ,O.NETWT--ISNULL((SELECT SUM(isnull(NETWT,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND ORSTATUS = 'R'  AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54),0) NETWT"
                            StrSql += vbCrLf + " ,O.SNO,O.ORNO "
                            StrSql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST O "
                            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON O.ITEMID=IT.ITEMID"
                            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SIT ON O.SUBITEMID = SIT.SUBITEMID"
                            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALID = IT.METALID "
                            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE= IT.CATCODE "
                            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..OUTSTANDING OU ON OU.BATCHNO= O.BATCHNO AND OU.RUNNO=O.ORNO "
                            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY CA ON CA.CATCODE= OU.CATCODE "
                            StrSql += vbCrLf + " WHERE O.SNO IN (" & TempOrSno & ")"
                        Else
                            If ORDER_MULTI_MIMR = False Then
                                StrSql = "SELECT M.METALNAME     AS METAL,ISNULL(CA.CATNAME,C.CATNAME) CATEGORY,IT.ITEMNAME ITEM,SIT.SUBITEMNAME SUBITEM,"
                                StrSql += vbCrLf + " O.PCS-ISNULL((SELECT SUM(isnull(PCS,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND  ORSTATUS = 'R' AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54),0) PCS"
                                StrSql += vbCrLf + " ,O.GRSWT-ISNULL((SELECT SUM(isnull(GRSWT,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND ORSTATUS = 'R'  AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54),0) GRSWT "
                                StrSql += vbCrLf + " ,O.NETWT-ISNULL((SELECT SUM(isnull(NETWT,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND ORSTATUS = 'R'  AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54),0) NETWT"
                                StrSql += vbCrLf + " ,O.SNO,O.ORNO "
                                StrSql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST O "
                                StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON O.ITEMID=IT.ITEMID"
                                StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SIT ON O.SUBITEMID = SIT.SUBITEMID"
                                StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALID = IT.METALID "
                                StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE= IT.CATCODE "
                                StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..OUTSTANDING OU ON OU.BATCHNO= O.BATCHNO AND OU.RUNNO=O.ORNO "
                                StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY CA ON CA.CATCODE= OU.CATCODE "
                                StrSql += vbCrLf + " WHERE O.SNO IN (" & TempOrSno & ")"
                            Else
                                StrSql = vbCrLf + " SELECT 1 FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO IN (" & TempOrSno & ")"
                                If Val(objGPack.GetSqlValue(StrSql, "").ToString) = 0 Then
                                    StrSql = "SELECT M.METALNAME     AS METAL,ISNULL(CA.CATNAME,C.CATNAME) CATEGORY,IT.ITEMNAME ITEM,SIT.SUBITEMNAME SUBITEM,"
                                    StrSql += vbCrLf + " O.PCS-ISNULL((SELECT SUM(isnull(PCS,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND  ORSTATUS = 'R' AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54),0) PCS"
                                    StrSql += vbCrLf + " ,O.GRSWT-ISNULL((SELECT SUM(isnull(GRSWT,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND ORSTATUS = 'R'  AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54),0) GRSWT "
                                    StrSql += vbCrLf + " ,O.NETWT-ISNULL((SELECT SUM(isnull(NETWT,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND ORSTATUS = 'R'  AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54),0) NETWT"
                                    StrSql += vbCrLf + " ,ISNULL((SELECT TOP 1 SNO FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND ORSTATUS = '" & IIf(oMaterial = Material.Issue, "R", "I") & "'  AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54 ORDER BY ENTRYORDER DESC),0) ORIRSNO"
                                    StrSql += vbCrLf + " ,O.SNO,O.ORNO "
                                    StrSql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST O "
                                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON O.ITEMID=IT.ITEMID"
                                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SIT ON O.SUBITEMID = SIT.SUBITEMID"
                                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALID = IT.METALID "
                                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE= IT.CATCODE "
                                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..OUTSTANDING OU ON OU.BATCHNO= O.BATCHNO AND OU.RUNNO=O.ORNO "
                                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY CA ON CA.CATCODE= OU.CATCODE "
                                    StrSql += vbCrLf + " WHERE O.SNO IN (" & TempOrSno & ")"
                                Else
                                    StrSql = "SELECT M.METALNAME     AS METAL,ISNULL(CA.CATNAME,C.CATNAME) CATEGORY,IT.ITEMNAME ITEM,SIT.SUBITEMNAME SUBITEM,"
                                    StrSql += vbCrLf + " ISNULL((SELECT TOP 1 PCS FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND  ORSTATUS = '" & IIf(oMaterial = Material.Issue, "R", "I") & "' AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54 ORDER BY ENTRYORDER DESC),0) PCS"
                                    StrSql += vbCrLf + " ,ISNULL((SELECT TOP 1 GRSWT FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND ORSTATUS = '" & IIf(oMaterial = Material.Issue, "R", "I") & "'  AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54 ORDER BY ENTRYORDER DESC),0) GRSWT "
                                    StrSql += vbCrLf + " ,ISNULL((SELECT TOP 1 NETWT FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND ORSTATUS = '" & IIf(oMaterial = Material.Issue, "R", "I") & "'  AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54 ORDER BY ENTRYORDER DESC),0) NETWT"
                                    StrSql += vbCrLf + " ,ISNULL((SELECT TOP 1 SNO FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND ORSTATUS = '" & IIf(oMaterial = Material.Issue, "R", "I") & "'  AND ISNULL(CANCEL,'')='' AND ORDSTATE_ID<>54 ORDER BY ENTRYORDER DESC),0) ORIRSNO"
                                    StrSql += vbCrLf + " ,O.SNO,O.ORNO "
                                    StrSql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST O "
                                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON O.ITEMID=IT.ITEMID"
                                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SIT ON O.SUBITEMID = SIT.SUBITEMID"
                                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALID = IT.METALID "
                                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE= IT.CATCODE "
                                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..OUTSTANDING OU ON OU.BATCHNO= O.BATCHNO AND OU.RUNNO=O.ORNO "
                                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY CA ON CA.CATCODE= OU.CATCODE "
                                    StrSql += vbCrLf + " WHERE O.SNO IN (" & TempOrSno & ")"
                                End If
                            End If
                        End If
                        Da = New OleDbDataAdapter(StrSql, cn)
                        Da.Fill(DtOrder)
                        DtOrder.AcceptChanges()
                        Dim dTOrAdditionalDetails As New DataTable
                        StrSql = "SELECT ORSNO,(SELECT TYPENAME FROM " & cnAdminDb & "..ORADMAST WHERE TYPEID = O.TYPEID)TYPENAME,VALUENAME FROM  " & cnAdminDb & "..ORADTRAN O  WHERE ORSNO IN (" & TempOrSno & ")"
                        Da = New OleDbDataAdapter(StrSql, cn)
                        Da.Fill(dTOrAdditionalDetails)
                        dTOrAdditionalDetails.AcceptChanges()
                        objAddtionalDetails.DtView = dTOrAdditionalDetails
                        If DtOrder.Rows.Count > 0 Then
                            grpOrder.Visible = True
                            If txtMOrdNo.Focus <> True Then
                                Dim Pcs As Integer = Val(DtOrder.Compute("SUM(PCS)", Nothing).ToString)
                                Dim Grswt As Decimal = Val(DtOrder.Compute("SUM(GRSWT)", Nothing).ToString)
                                Dim NetWt As Decimal = Val(DtOrder.Compute("SUM(NETWT)", Nothing).ToString)
                                cmbOCategory.Text = DtOrder.Rows(0).Item("CATEGORY").ToString : cmbOCategory.Enabled = False
                                cmbOMetal.Text = DtOrder.Rows(0).Item("METAL").ToString : cmbOMetal.Enabled = False
                                cmbOItem.Text = DtOrder.Rows(0).Item("ITEM").ToString : cmbOItem.Enabled = False
                                cmbOSubItem.Text = DtOrder.Rows(0).Item("SUBITEM").ToString : cmbOSubItem.Enabled = False
                                txtOPcs_NUM.Text = Pcs 'DtOrder.Rows(0).Item("PCS").ToString
                                txtOGrsWt_WET.Text = Grswt ' DtOrder.Rows(0).Item("GRSWT").ToString
                                txtONetWt_WET.Text = NetWt 'DtOrder.Rows(0).Item("NETWT").ToString
                                ORSNO = DtOrder.Rows(0).Item("SNO").ToString
                                If ORDER_MULTI_MIMR Then
                                    ORIRSNO = DtOrder.Rows(0).Item("ORIRSNO").ToString
                                    If ORIRSNO.ToString <> "" And ORIRSNO.ToString <> "0" Then
                                        StrSql = "SELECT "
                                        StrSql += vbCrLf + "ST.SNO"
                                        StrSql += vbCrLf + ",IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
                                        StrSql += vbCrLf + ",ST.STNPCS AS PCS,ST.STNWT AS WEIGHT"
                                        StrSql += vbCrLf + ",ST.STNUNIT AS UNIT,ST.CALCMODE AS CALC,ST.STNRATE AS RATE,ST.STNAMT AS AMOUNT"
                                        StrSql += vbCrLf + ",IM.DIASTONE AS METALID,'' AS SEIVE"
                                        StrSql += vbCrLf + ",'NO' AS STUDDEDUCT"
                                        StrSql += vbCrLf + "FROM " & cnAdminDb & "..ORIRDETAILSTONE AS ST"
                                        StrSql += vbCrLf + "INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
                                        StrSql += vbCrLf + "LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = ST.STNITEMID AND SM.SUBITEMID = ST.STNSUBITEMID"
                                        StrSql += vbCrLf + "WHERE ST.ORSNO = '" & ORIRSNO & "'"
                                        Dim dtStone As New DataTable
                                        Da = New OleDbDataAdapter(StrSql, cn)
                                        Da.Fill(dtStone)
                                        If objStone.dtGridStone.Rows.Count > 0 Then
                                            For i As Integer = 0 To dtStone.Rows.Count - 1
                                                For j As Integer = 0 To objStone.dtGridStone.Rows.Count - 1
                                                    If objStone.dtGridStone.Rows(j).Item("SNO").ToString = dtStone.Rows(i).Item("SNO").ToString Then
                                                        GoTo duplicate1
                                                    End If
                                                Next
                                            Next
                                        End If
                                        For Each RoStn As DataRow In dtStone.Rows
                                            Dim RoStnIm As DataRow = objStone.dtGridStone.NewRow
                                            For Each Col As DataColumn In dtStone.Columns
                                                RoStnIm.Item(Col.ColumnName) = RoStn.Item(Col.ColumnName)
                                            Next
                                            objStone.dtGridStone.Rows.Add(RoStnIm)
                                        Next
duplicate1:
                                        objStone.dtGridStone.AcceptChanges()
                                        objStone.CalcStoneWtAmount()
                                    Else
                                        StrSql = vbCrLf + " SELECT  "
                                        StrSql += vbCrLf + "     (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID =Y.STNITEMID)) METALID"
                                        StrSql += vbCrLf + "    ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID =Y.STNITEMID) ITEM"
                                        StrSql += vbCrLf + "    ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID =Y.STNSUBITEMID) SUBITEM"
                                        StrSql += vbCrLf + "    ,CUTID,COLORID,CLARITYID,SETTYPEID,SHAPEID, HEIGHT,WIDTH"
                                        StrSql += vbCrLf + "    , STNWT AS WEIGHT"
                                        StrSql += vbCrLf + "    ,CONVERT(VARCHAR,CONVERT(NUMERIC(15,4),SUM(STNWT)/CASE WHEN SUM(STNPCS) > 0 THEN SUM(STNPCS) ELSE 1 END)) SEIVE"
                                        StrSql += vbCrLf + "    ,STNUNIT UNIT,CALCMODE CALC,STNRATE RATE,STNAMT AMOUNT"
                                        StrSql += vbCrLf + "    ,SNO"
                                        StrSql += vbCrLf + " FROM"
                                        StrSql += vbCrLf + " ("
                                        StrSql += vbCrLf + " 	SELECT X.*,SZ.SIZEDESC FROM "
                                        StrSql += vbCrLf + " 	("
                                        StrSql += vbCrLf + " 	SELECT SNO,STNITEMID,STNSUBITEMID,STNPCS,STNWT,CONVERT(NUMERIC(15,4),CASE WHEN STNPCS > 0 THEN (STNWT/STNPCS)*100 ELSE STNWT*100 END) AS WTRANGE"
                                        StrSql += vbCrLf + " 	,CONVERT(NUMERIC(15,3),CASE WHEN STNPCS > 0 THEN (STNWT/STNPCS) ELSE STNWT END) AS DIASIZE"
                                        StrSql += vbCrLf + " 	,CALCMODE,STNUNIT,STNRATE,STNAMT,CUTID,COLORID,CLARITYID,SETTYPEID,SHAPEID, HEIGHT,WIDTH FROM " & cnAdminDb & "..ORSTONE"
                                        StrSql += vbCrLf + " 	WHERE ORSNO IN (SELECT SNO FROM " & cnAdminDb & "..ORMAST WHERE ORNO= '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & txtOOrdNo.Text & "')"
                                        StrSql += vbCrLf + "    AND ORSNO IN (" & TempOrSno & ")"
                                        StrSql += vbCrLf + " 	)X"
                                        StrSql += vbCrLf + " 	LEFT OUTER JOIN " & cnAdminDb & "..CENTSIZE AS SZ ON SZ.ITEMID = STNITEMID AND SZ.SUBITEMID = STNSUBITEMID AND WTRANGE BETWEEN FROMCENT AND TOCENT"
                                        StrSql += vbCrLf + " )Y GROUP BY SIZEDESC,STNITEMID,STNSUBITEMID,STNUNIT,CALCMODE,STNPCS,STNWT,STNRATE,STNAMT,CUTID,COLORID ,CLARITYID,SETTYPEID,SHAPEID,HEIGHT,WIDTH"
                                        StrSql += vbCrLf + " ,SNO"
                                        Dim dtStone As New DataTable
                                        Da = New OleDbDataAdapter(StrSql, cn)
                                        Da.Fill(dtStone)
                                        With dtStone.Columns
                                            .Add("KEYNO", GetType(String))
                                            .Add("TRANTYPE", GetType(String))
                                            .Add("STNTYPE", GetType(String))
                                            .Add("OCATCODE", GetType(String))
                                            .Add("STUDDEDUCT", GetType(String))
                                            .Add("DISCOUNT", GetType(Double))
                                            .Add("TAGSTNPCS", GetType(Integer))
                                            .Add("TAGSTNWT", GetType(Decimal))
                                            .Add("TAGSNO", GetType(String))
                                            .Add("R_VAT", GetType(Decimal))
                                            .Add("ISSSNO", GetType(String))
                                            .Add("RESNO", GetType(String))
                                        End With
                                        If objStone.dtGridStone.Rows.Count > 0 Then
                                            For i As Integer = 0 To dtStone.Rows.Count - 1
                                                For j As Integer = 0 To objStone.dtGridStone.Rows.Count - 1
                                                    If objStone.dtGridStone.Rows(j).Item("SNO").ToString = dtStone.Rows(i).Item("SNO").ToString Then
                                                        GoTo duplicate2
                                                    End If
                                                Next
                                            Next
                                        End If
                                        For Each RoStn As DataRow In dtStone.Rows
                                            Dim RoStnIm As DataRow = objStone.dtGridStone.NewRow
                                            For Each Col As DataColumn In dtStone.Columns
                                                Try
                                                    RoStnIm.Item(Col.ColumnName) = RoStn.Item(Col.ColumnName)
                                                Catch ex As Exception
                                                    MsgBox(Col.ColumnName)
                                                End Try
                                            Next
                                            objStone.dtGridStone.Rows.Add(RoStnIm)
                                        Next
duplicate2:
                                        objStone.dtGridStone.AcceptChanges()
                                        objStone.CalcStoneWtAmount()
                                    End If
                                End If
                                OGrswt = Grswt 'Val(DtOrder.Rows(0).Item("GRSWT").ToString)
                                ONetwt = NetWt 'Val(DtOrder.Rows(0).Item("NETWT").ToString)
                                If oMaterial = Material.Issue Then
                                    cmbOProcess.Text = "ISSUE TO SMITH"
                                Else
                                    cmbOProcess.Text = "RECEIVED FROM SMITH"
                                End If
                                For i As Integer = 0 To DtOrder.Rows.Count - 1
                                    For j As Integer = 0 To dgvOrder.Rows.Count - 1
                                        If DtOrder.Rows(i)("SNO").ToString = dgvOrder.Rows(j).Cells("ORSNO").Value.ToString Then GoTo againskip
                                    Next
                                    Dim mDr As DataRow
                                    mDr = MdtOdDet.NewRow
                                    mDr("ORNO") = DtOrder.Rows(i)("ORNO").ToString
                                    mDr("ORSNO") = DtOrder.Rows(i)("SNO").ToString
                                    mDr("PCS") = DtOrder.Rows(i)("PCS").ToString
                                    mDr("GRSWT") = DtOrder.Rows(i)("GRSWT").ToString
                                    mDr("NETWT") = DtOrder.Rows(i)("NETWT").ToString
                                    MdtOdDet.Rows.Add(mDr)
                                    MdtOdDet.AcceptChanges()
                                Next
againskip:
                                dgvOrder.DataSource = Nothing
                                dgvOrder.DataSource = MdtOdDet
                            Else

                                'If chkMulti.Checked Then
                                ''For cnt As Integer = 0 To objOrderInfo.DgvOrder.Rows.Count - 1
                                ''If CType(objOrderInfo.DgvOrder.Rows(cnt).Cells("CHECK").Value, Boolean) = True Then
                                ''    For i As Integer = 0 To dgvOrder.Rows.Count - 1
                                ''        If objOrderInfo.DgvOrder.Rows(cnt).Cells("SNO").Value.ToString = dgvOrder.Rows(i).Cells("ORSNO").Value.ToString Then GoTo Skip
                                ''        ' MsgBox("Item already Loaded...", MsgBoxStyle.Information) : txtMOrdNo.Text = "" : Exit Sub
                                ''    Next
                                ''    Dim mDr As DataRow
                                ''    mDr = MdtOdDet.NewRow
                                ''    mDr("ORNO") = txtMOrdNo.Text.ToString()
                                ''    mDr("ORSNO") = objOrderInfo.DgvOrder.Rows(cnt).Cells("SNO").Value.ToString
                                ''    mDr("PCS") = objOrderInfo.DgvOrder.Rows(cnt).Cells("PCS").Value.ToString
                                ''    mDr("GRSWT") = objOrderInfo.DgvOrder.Rows(cnt).Cells("GRSWT").Value.ToString
                                ''    mDr("NETWT") = objOrderInfo.DgvOrder.Rows(cnt).Cells("NETWT").Value.ToString
                                ''    MdtOdDet.Rows.Add(mDr)
                                ''    MdtOdDet.AcceptChanges()
                                ''    dgvOrder.DataSource = Nothing
                                ''    dgvOrder.DataSource = MdtOdDet
                                ''    cmbMCategory.Text = DtOrder.Rows(0).Item("CATEGORY").ToString : cmbOCategory.Enabled = False
                                ''    cmbMMetal.Text = DtOrder.Rows(0).Item("METAL").ToString : cmbMMetal.Enabled = False
                                ''    txtMPcs_NUM.Text = Convert.ToString(Val(txtMPcs_NUM.Text.ToString & "") + Val(objOrderInfo.DgvOrder.Rows(cnt).Cells("PCS").Value.ToString & ""))
                                ''    txtMGrsWt_WET.Text = Convert.ToString(Val(txtMGrsWt_WET.Text.ToString) + Val(objOrderInfo.DgvOrder.Rows(cnt).Cells("GRSWT").Value))
                                ''    'txtMNetWt_WET.Text = DtOrder.Rows(0).Item("NETWT").ToString
                                ''    ORSNO = objOrderInfo.DgvOrder.Rows(cnt).Cells("SNO").Value.ToString
                                ''    OGrswt = Val(objOrderInfo.DgvOrder.Rows(cnt).Cells("GRSWT").Value)
                                ''        ONetwt = Val(objOrderInfo.DgvOrder.Rows(cnt).Cells("NETWT").Value)
                                ''    cmbMProcess.Text = "RECEIVED FROM SMITH"
                                ''    dgvOrder.Columns("WASTAGE").Visible = False
                                ''    dgvOrder.Columns("MC").Visible = False
                                ''    txtMOrdNo.Text = ""
                                ''End If

                                For cnt As Integer = 0 To DtOrder.Rows.Count - 1
                                    For i As Integer = 0 To dgvOrder.Rows.Count - 1
                                        If DtOrder.Rows(cnt)("SNO").ToString = dgvOrder.Rows(i).Cells("ORSNO").Value.ToString Then GoTo Skip
                                        ' MsgBox("Item already Loaded...", MsgBoxStyle.Information) : txtMOrdNo.Text = "" : Exit Sub
                                    Next
                                    Dim mDr As DataRow
                                    mDr = MdtOdDet.NewRow
                                    mDr("ORNO") = DtOrder.Rows(cnt)("ORNO").ToString
                                    mDr("ORSNO") = DtOrder.Rows(cnt)("SNO").ToString
                                    mDr("PCS") = DtOrder.Rows(cnt)("PCS").ToString
                                    mDr("GRSWT") = DtOrder.Rows(cnt)("GRSWT").ToString
                                    mDr("NETWT") = DtOrder.Rows(cnt)("NETWT").ToString
                                    mDr("WASTAGE") = 0.0
                                    mDr("MC") = 0
                                    MdtOdDet.Rows.Add(mDr)
                                    MdtOdDet.AcceptChanges()
                                    dgvOrder.DataSource = Nothing
                                    dgvOrder.DataSource = MdtOdDet
                                    LoadMetal(cmbMMetal)
                                    'cmbMMetal.Text = DtOrder.Rows(0).Item("METAL").ToString ': cmbMMetal.Enabled = False
                                    LoadCategoryDetails(cmbMCategory)
                                    'cmbMCategory.Text = DtOrder.Rows(0).Item("CATEGORY").ToString ': cmbOCategory.Enabled = False
                                    txtMPcs_NUM.Text = Convert.ToString(Val(txtMPcs_NUM.Text.ToString & "") + Val(DtOrder.Rows(cnt)("PCS").ToString & ""))
                                    txtMGrsWt_WET.Text = Convert.ToString(Val(txtMGrsWt_WET.Text.ToString & "") + Val(DtOrder.Rows(cnt)("GRSWT").ToString & ""))
                                    ORSNO = DtOrder.Rows(cnt)("SNO").ToString
                                    If ORDER_MULTI_MIMR Then
                                        ORIRSNO = DtOrder.Rows(cnt)("ORIRSNO").ToString
                                    End If
                                    OGrswt = Val(DtOrder.Rows(cnt)("GRSWT").ToString)
                                    ONetwt = Val(DtOrder.Rows(cnt)("NETWT").ToString)
                                    If oMaterial = Material.Issue Then
                                        cmbOProcess.Text = "ISSUE TO SMITH"
                                    Else
                                        cmbOProcess.Text = "RECEIVED FROM SMITH"
                                    End If
                                    dgvOrder.Columns("WASTAGE").Visible = False
                                    dgvOrder.Columns("MC").Visible = False
Skip:
                                Next
                                If MdtOdDet.Rows.Count > 0 Then
                                    'MdtOdDet.Rows(0)("PCS") = MdtOdDet.Compute("SUM(PCS)", "ORNO<>'TOTAL'")
                                    'MdtOdDet.Rows(0)("GRSWT") = MdtOdDet.Compute("SUM(GRSWT)", "ORNO<>'TOTAL'")
                                    'MdtOdDet.Rows(0)("NETWT") = MdtOdDet.Compute("SUM(NETWT)", "ORNO<>'TOTAL'")
                                    txtMPcs_NUM.Text = MdtOdDet.Compute("SUM(PCS)", "ORNO<>'TOTAL'")
                                    txtMGrsWt_WET.Text = MdtOdDet.Compute("SUM(GRSWT)", "ORNO<>'TOTAL'")
                                End If
                                txtMOrdNo.Text = ""
                                lblMordNo.Focus()
                                'Else
                                '    cmbMCategory.Text = DtOrder.Rows(0).Item("CATEGORY").ToString : cmbOCategory.Enabled = False
                                '    cmbMMetal.Text = DtOrder.Rows(0).Item("METAL").ToString : cmbMMetal.Enabled = False
                                '    txtMPcs_NUM.Text = DtOrder.Rows(0).Item("PCS").ToString
                                '    txtMGrsWt_WET.Text = DtOrder.Rows(0).Item("GRSWT").ToString
                                '    txtMNetWt_WET.Text = DtOrder.Rows(0).Item("NETWT").ToString
                                '    ORSNO = DtOrder.Rows(0).Item("SNO").ToString
                                '    OGrswt = Val(DtOrder.Rows(0).Item("GRSWT").ToString)
                                '    ONetwt = Val(DtOrder.Rows(0).Item("NETWT").ToString)
                                '    cmbMProcess.Text = "RECEIVED FROM SMITH"
                                '    dgvOrder.Enabled = False
                            End If
                            'End If
                        End If
                        If rbtStone.Checked Then
                            StrSql = vbCrLf + " SELECT "
                            StrSql += vbCrLf + "     (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID =Y.STNITEMID) ITEM"
                            StrSql += vbCrLf + "    ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID =Y.STNSUBITEMID) SUBITEM"
                            StrSql += vbCrLf + "    ,CONVERT(VARCHAR,SUM(STNPCS)) PCS,CONVERT(VARCHAR,SUM(STNWT)) WEIGHT"
                            StrSql += vbCrLf + "    ,CONVERT(VARCHAR,CONVERT(NUMERIC(15,4),SUM(STNWT)/CASE WHEN SUM(STNPCS) > 0 THEN SUM(STNPCS) ELSE 1 END)) SIZE"
                            StrSql += vbCrLf + "    ,STNUNIT UNIT,CALCMODE CALC,STNRATE RATE,STNAMT AMOUNT"
                            StrSql += vbCrLf + " FROM"
                            StrSql += vbCrLf + " ("
                            StrSql += vbCrLf + " 	SELECT X.*,SZ.SIZEDESC FROM "
                            StrSql += vbCrLf + " 	("
                            StrSql += vbCrLf + " 	SELECT STNITEMID,STNSUBITEMID,STNPCS,STNWT,CONVERT(NUMERIC(15,4),CASE WHEN STNPCS > 0 THEN (STNWT/STNPCS)*100 ELSE STNWT*100 END) AS WTRANGE"
                            StrSql += vbCrLf + " 	,CONVERT(NUMERIC(15,3),CASE WHEN STNPCS > 0 THEN (STNWT/STNPCS) ELSE STNWT END) AS DIASIZE"
                            StrSql += vbCrLf + " 	,CALCMODE,STNUNIT,STNRATE,STNAMT FROM " & cnAdminDb & "..ORSTONE"
                            StrSql += vbCrLf + " 	WHERE ORSNO IN (SELECT SNO FROM " & cnAdminDb & "..ORMAST WHERE ORNO= '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & txtSOrdNo.Text & "')"
                            StrSql += vbCrLf + "    AND ORSNO IN (" & TempOrSno & ")"
                            StrSql += vbCrLf + " 	)X"
                            StrSql += vbCrLf + " 	LEFT OUTER JOIN " & cnAdminDb & "..CENTSIZE AS SZ ON SZ.ITEMID = STNITEMID AND SZ.SUBITEMID = STNSUBITEMID AND WTRANGE BETWEEN FROMCENT AND TOCENT"
                            StrSql += vbCrLf + " )Y GROUP BY SIZEDESC,STNITEMID,STNSUBITEMID,STNUNIT,CALCMODE,STNPCS,STNWT,STNRATE,STNAMT"
                            MaterialStoneDia.dtGridStone.Rows.Clear()
                            Da = New OleDbDataAdapter(StrSql, cn)
                            Da.Fill(MaterialStoneDia.dtGridStone)
                            MaterialStoneDia.dtGridStone.AcceptChanges()
                            MaterialStoneDia.StartPosition = FormStartPosition.CenterScreen
                            MaterialStoneDia.ShowDialog()

                            If MaterialStoneDia.gridStoneTotal.Rows.Count > 0 Then
                                txtSPcs_NUM.Text = MaterialStoneDia.gridStoneTotal.Rows(0).Cells("PCS").Value.ToString
                                txtSGrsWt_WET.Text = Format(Convert.ToDouble(MaterialStoneDia.gridStoneTotal.Rows(0).Cells("WEIGHT").Value.ToString), "0.000")
                                txtSGrsAmt_AMT.Text = Format(Convert.ToDouble(MaterialStoneDia.gridStoneTotal.Rows(0).Cells("AMOUNT").Value.ToString), "0.00")
                            End If
                            cmbSMetal.Enabled = False
                            cmbSCategory.Enabled = False
                            cmbSIssuedCategory.Enabled = False
                            CmbSPurity.Enabled = False
                            cmbSItem.Enabled = False
                            cmbSSubItem.Enabled = False
                            txtSPcs_NUM.Enabled = False
                            txtSGrsWt_WET.Enabled = False
                            cmbSUnit.Enabled = False
                            cmbSCalcMode.Enabled = False
                            txtSRate_OWN.Enabled = False
                            txtSGrsAmt_AMT.Enabled = False
                            txtSAddlCharge_AMT.Enabled = False
                            txtSVatPer_PER.Enabled = False
                            txtSVat_AMT.Enabled = False
                            txtSAmount_AMT.Enabled = False
                        End If
                    Else
                        If STOCKVALIDATION And oMaterial = Material.Issue Then
                            StkAvail = LoadBalanceStock()
                        End If
                        If STOCKVALIDATION_MR And oMaterial = Material.Receipt Then
                            StkAvail = LoadBalanceStock_MR(cmbOMetal.Text)
                        End If
                        If MIMR_BAGISSUE And oMaterial = Material.Issue Then StkAvail = LoadMeltIssue()
                    End If
                Else
                    cmbSMetal.Enabled = True
                    cmbSCategory.Enabled = True
                    cmbSIssuedCategory.Enabled = True
                    CmbSPurity.Enabled = True
                    cmbSItem.Enabled = True
                    cmbSSubItem.Enabled = True
                    txtSPcs_NUM.Enabled = True
                    txtSGrsWt_WET.Enabled = True
                    cmbSUnit.Enabled = True
                    cmbSCalcMode.Enabled = True
                    txtSRate_OWN.Enabled = True
                    txtSGrsAmt_AMT.Enabled = True
                    txtSAddlCharge_AMT.Enabled = True
                    txtSVatPer_PER.Enabled = True
                    txtSVat_AMT.Enabled = True
                    txtSAmount_AMT.Enabled = True
                End If
            Else
                loadJobDetails()
            End If
            If StkAvail Then
                'If chkMulti.Checked Then
                '    txtMOrdNo.Select()
                'Else
                SendKeys.Send("{TAB}")
                'End If
            End If
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            btnOk.Focus()
        End If
    End Sub
    Function LoadMeltIssue()
        If rbtMetal.Checked And txtMOrdNo.Text <> "" Then
            StrSql = vbCrLf + " SELECT BAGNO,METAL,'' CATEGORY"
            StrSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'R' THEN GRSWT ELSE -1*GRSWT END) AS GRSWT"
            StrSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'R' THEN NETWT ELSE -1*NETWT END) AS NETWT"
            StrSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'R' THEN LESSWT ELSE -1*LESSWT END) AS LESSWT"
            StrSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'R' THEN AMOUNT ELSE -1*AMOUNT END) AS AMOUNT"
            StrSql += vbCrLf + " FROM"
            StrSql += vbCrLf + " ("
            StrSql += vbCrLf + " SELECT 'R' SEP,BAGNO"
            StrSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID "
            StrSql += vbCrLf + " = (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE))AS METAL"
            StrSql += vbCrLf + " ,'' AS CATEGORY"
            StrSql += vbCrLf + " ,GRSWT,NETWT,LESSWT,AMOUNT"
            StrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT R"
            StrSql += vbCrLf + " WHERE ISNULL(BAGNO,'') = '" & txtMOrdNo.Text & "'"
            StrSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') = 'M'"
            StrSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            StrSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'S'"
            StrSql += vbCrLf + " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "'"
            StrSql += vbCrLf + " UNION ALL"
            StrSql += vbCrLf + " SELECT 'R' SEP,BAGNO"
            StrSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = "
            StrSql += vbCrLf + " (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE))AS METAL"
            StrSql += vbCrLf + " ,'' AS CATEGORY"
            StrSql += vbCrLf + " ,CASE WHEN TRANTYPE='SA' THEN TAGGRSWT-GRSWT ELSE GRSWT END GRSWT"
            StrSql += vbCrLf + " ,CASE WHEN TRANTYPE='SA' THEN TAGNETWT-NETWT ELSE NETWT END NETWT,LESSWT,AMOUNT"
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE R"
            StrSql += vbCrLf + " WHERE ISNULL(BAGNO,'') = '" & txtMOrdNo.Text & "'"
            StrSql += vbCrLf + " AND ISNULL(MELT_RETAG,'') = 'M'"
            StrSql += vbCrLf + " AND TRANTYPE ='SA' "
            StrSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            StrSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'S'"
            StrSql += vbCrLf + " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "'"
            StrSql += vbCrLf + " UNION ALL"
            StrSql += vbCrLf + " SELECT 'I' SEP,BAGNO"
            StrSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = "
            StrSql += vbCrLf + " (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = R.CATCODE))AS METAL"
            StrSql += vbCrLf + " ,'' AS CATEGORY"
            StrSql += vbCrLf + " ,GRSWT,NETWT,LESSWT,AMOUNT"
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE R"
            StrSql += vbCrLf + " WHERE ISNULL(BAGNO,'') = '" & txtMOrdNo.Text & "'"
            StrSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            StrSql += vbCrLf + " AND ISNULL(TRANFLAG,'') <> 'S'"
            StrSql += vbCrLf + " AND ISNULL(TRANTYPE,'') = 'IIS'"
            StrSql += vbCrLf + " AND ISNULL(COMPANYID,'') = '" & strCompanyId & "'"
            StrSql += vbCrLf + " )X"
            StrSql += vbCrLf + " GROUP BY BAGNO,METAL"
            StrSql += vbCrLf + " HAVING SUM(CASE WHEN SEP = 'R' THEN GRSWT ELSE -1*GRSWT END) > 0 "
            Cmd = New OleDbCommand(StrSql, cn)
            Da = New OleDbDataAdapter(Cmd)
            Dim dtStk As New DataTable
            Da.Fill(dtStk)
            If dtStk.Rows.Count > 0 Then
                cmbMCategory.Text = dtStk.Rows(0).Item("CATEGORY").ToString
                cmbMMetal.Text = dtStk.Rows(0).Item("METAL").ToString
                cmbMMetal.Enabled = False
                txtMGrsWt_WET.Text = Val(dtStk.Compute("SUM(GRSWT)", Nothing).ToString)
                txtMNetWt_WET.Text = Val(dtStk.Compute("SUM(NETWT)", Nothing).ToString)
                txtMLessWt_WET.Text = Val(dtStk.Compute("SUM(LESSWT)", Nothing).ToString)
                cmbMProcess.Text = "ISSUE TO SMITH"
                Return True
            Else
                MsgBox("Record Not Found.", MsgBoxStyle.Information)
                Return False
            End If
        End If
    End Function
    Function LoadBalanceStock_MR(ByVal _Metal As String) As Boolean
        If oTransactionType <> "RECEIPT" Then Return True

        _Metal = objGPack.GetSqlValue("SELECT TOP 1 METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & _Metal & "'")
        If _Metal = "" Then
            If rbtOrnament.Checked Then
                _Metal = "O"
            ElseIf rbtMetal.Checked Then
                _Metal = "M"
            ElseIf rbtStone.Checked Then
                _Metal = "T"
            End If
        End If
        If _Metal <> "" Then
            'Dim TranNo As String = ""
            Dim Metal As String = ""
            If rbtOrnament.Checked Then
                'TranNo = txtOOrdNo.Text
                Metal = "O"
            ElseIf rbtMetal.Checked Then
                'TranNo = txtMOrdNo.Text
                Metal = "M"
            ElseIf rbtStone.Checked Then
                'TranNo = txtSOrdNo.Text
                Metal = "T"
            End If
            Dim mStrSql As String = ""
            If oMaterial = Material.Receipt Then
                mStrSql = vbCrLf + " SELECT ACCODE,METAL"
                mStrSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT"
                mStrSql += vbCrLf + " ,(SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE METALID= X.METAL) ITEM"
                mStrSql += vbCrLf + " ,(SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID IN  ((SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID= X.METAL))) SUBITEM"
                mStrSql += vbCrLf + " ,'G' UNIT"
                mStrSql += vbCrLf + " FROM"
                mStrSql += vbCrLf + " ("
                mStrSql += vbCrLf + " SELECT SNO,M.METALNAME METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                mStrSql += vbCrLf + " ,SUM(ISNULL(PCS,0)) PCS"
                mStrSql += vbCrLf + " ,SUM(ISNULL(CASE WHEN R.TRANTYPE='I' THEN R.GRSWT ELSE -1*R.GRSWT END,0)) GRSWT"
                mStrSql += vbCrLf + " ,SUM(ISNULL(CASE WHEN R.TRANTYPE='I' THEN R.NETWT ELSE -1*R.NETWT END,0)) NETWT,R.ACCODE"
                mStrSql += vbCrLf + " FROM " & cnStockDb & "..OPENWEIGHT R "
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON C.METALID = M.METALID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = R.ITEMID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = R.SUBITEMID"
                mStrSql += vbCrLf + " WHERE R.STOCKTYPE='S'"
                mStrSql += vbCrLf + " AND R.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID='" & _Metal & "')"
                mStrSql += vbCrLf + " AND R.ACCODE ='" & Maccode & "'"
                If MCostId <> "" Then mStrSql += vbCrLf + " AND R.COSTID='" & MCostId & "'"
                If Metal = "S" Then
                    mStrSql += vbCrLf + " AND M.TTYPE='" & Metal & "'"
                Else
                    'mStrSql += vbCrLf + " AND C.PURITYID IN(SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALTYPE='" & Metal & "')"
                End If
                If InsSno <> "" Then mStrSql += vbCrLf + " AND R.SNO='" & InsSno & "'"
                mStrSql += vbCrLf + " GROUP BY SNO,M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME,R.ACCODE  "
                mStrSql += vbCrLf + " UNION ALL"
                mStrSql += vbCrLf + " SELECT SNO,M.METALNAME METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                mStrSql += vbCrLf + " ,SUM(ISNULL(PCS,0)) PCS"
                mStrSql += vbCrLf + " ,SUM(ISNULL(R.GRSWT,0))+SUM(ISNULL(R.ALLOY,0)) GRSWT "
                mStrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) NETWT,R.ACCODE"
                mStrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE R "
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON R.METALID = M.METALID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = R.ITEMID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = R.SUBITEMID"
                mStrSql += vbCrLf + " WHERE R.TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "'"
                mStrSql += vbCrLf + " AND LEN(R.TRANTYPE)>2"
                'mStrSql += vbCrLf + " AND R.TRANTYPE<>'RIN'"
                'mStrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                If MCostId <> "" Then mStrSql += vbCrLf + " AND R.COSTID='" & MCostId & "'"
                mStrSql += vbCrLf + " AND R.METALID ='" & _Metal & "'"
                mStrSql += vbCrLf + " AND R.ACCODE ='" & Maccode & "'"
                If Metal = "S" Then
                    mStrSql += vbCrLf + " AND M.TTYPE='" & Metal & "'"
                Else
                    'mStrSql += vbCrLf + " AND C.PURITYID IN(SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALTYPE='" & Metal & "')"
                End If
                mStrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
                If InsSno <> "" Then mStrSql += vbCrLf + " AND R.SNO='" & InsSno & "'"
                mStrSql += vbCrLf + " GROUP BY SNO,M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME,R.ACCODE  "
                mStrSql += vbCrLf + " UNION ALL"
                ''mStrSql += vbCrLf + " SELECT ISSNO AS SNO,M.METALNAME AS METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                mStrSql += vbCrLf + " SELECT ISSNO AS SNO,M.METALNAME AS METAL "
                mStrSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY where catcode =(SELECT CATCODE FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSNO)) CATEGORY"
                mStrSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST where ITEMID =(SELECT ITEMID FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSNO)) ITEM"
                mStrSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST where SUBITEMID =(SELECT SUBITEMID FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSNO)) SUBITEM"
                mStrSql += vbCrLf + " ,SUM(ISNULL(PCS,0))*(-1) PCS"
                If Metal = "S" Then
                    mStrSql += vbCrLf + " ,CASE "
                    mStrSql += vbCrLf + " WHEN (SELECT STONEUNIT FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSNO)='G' AND I.STONEUNIT ='C' THEN (SUM(ISNULL(I.GRSWT,0))/5)*(-1) "
                    mStrSql += vbCrLf + " WHEN (SELECT STONEUNIT FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSNO)='C' AND I.STONEUNIT ='G' THEN (SUM(ISNULL(I.GRSWT,0))*5)*(-1) "
                    mStrSql += vbCrLf + " ELSE SUM(ISNULL(I.GRSWT,0))*(-1) END GRSWT"
                    mStrSql += vbCrLf + " ,CASE "
                    mStrSql += vbCrLf + " WHEN (SELECT STONEUNIT FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSNO)='G' AND I.STONEUNIT ='C' THEN (SUM(ISNULL(I.NETWT,0))/5)*(-1) "
                    mStrSql += vbCrLf + " WHEN (SELECT STONEUNIT FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSNO)='C' AND I.STONEUNIT ='G' THEN (SUM(ISNULL(I.NETWT,0))*5)*(-1) "
                    mStrSql += vbCrLf + " ELSE SUM(ISNULL(I.NETWT,0))*(-1) END NETWT"
                Else
                    mStrSql += vbCrLf + " ,SUM(ISNULL(I.GRSWT,0))*(-1) GRSWT "
                    mStrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0))*(-1) NETWT"
                End If
                mStrSql += vbCrLf + " ,I.ACCODE FROM " & cnStockDb & "..RECEIPT  I "
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = I.ITEMID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = I.SUBITEMID"
                mStrSql += vbCrLf + " WHERE I.TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "'"
                mStrSql += vbCrLf + " AND LEN(I.TRANTYPE)>2"
                mStrSql += vbCrLf + " AND I.TRANTYPE='RRE'"
                mStrSql += vbCrLf + " AND I.METALID ='" & _Metal & "'"
                mStrSql += vbCrLf + " AND I.ACCODE ='" & Maccode & "'"
                'mStrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                If MCostId <> "" Then mStrSql += vbCrLf + " AND I.COSTID='" & MCostId & "'"
                'mStrSql += vbCrLf + " AND ISSNO IN"
                'mStrSql += vbCrLf + " (SELECT SNO FROM " & cnStockDb & "..OPENWEIGHT UNION ALL SELECT SNO FROM " & cnStockDb & "..ISSUE "
                'mStrSql += vbCrLf + " WHERE TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "' "
                'mStrSql += vbCrLf + " AND I.METALID ='" & _Metal & "'"
                'mStrSql += vbCrLf + " AND I.ACCODE ='" & Maccode & "'"
                'mStrSql += vbCrLf + " AND ISNULL(CANCEL,'')='' "
                'If MCostId <> "" Then mStrSql += vbCrLf + " AND COSTID='" & MCostId & "'"
                'mStrSql += vbCrLf + " AND LEN(TRANTYPE)>2 "
                ''mStrSql += vbCrLf + "AND TRANTYPE<>'RIN'"
                'mStrSql += vbCrLf + ")"
                If Metal = "S" Then
                    mStrSql += vbCrLf + " AND M.TTYPE='" & Metal & "'"
                Else
                    'mStrSql += vbCrLf + " AND C.PURITYID IN(SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALTYPE='" & Metal & "')"
                End If
                mStrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
                If InsSno <> "" Then mStrSql += vbCrLf + " AND I.ISSNO='" & InsSno & "'"
                mStrSql += vbCrLf + " GROUP BY ISSNO,M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME,I.ACCODE  "
                If Metal = "S" Then
                    mStrSql += vbCrLf + " ,I.STONEUNIT"
                End If
                mStrSql += vbCrLf + " )X GROUP BY ACCODE,METAL  "
                If Metal = "S" Then
                    mStrSql += vbCrLf + " ,ITEM,SUBITEM"
                End If
                mStrSql += vbCrLf + " HAVING (SUM(GRSWT)>0  OR SUM(PCS)>0)"
            End If
            If mStrSql.Trim = "" Then Return True
            Dim dtStk As New DataTable
            Cmd = New OleDbCommand(mStrSql, cn)
            Da = New OleDbDataAdapter(Cmd)
            Da.Fill(dtStk)
            Dim StkRow As DataRow
            If dtStk.Rows.Count > 0 Then

                If dtStk.Rows.Count = 1 Then
                    'StrSql = vbCrLf + " SELECT ITEM,SUBITEM"
                    'StrSql += vbCrLf + " ,SUM(PCS)PCS,SUM(WEIGHT) WEIGHT"
                    'StrSql += vbCrLf + " ,UNIT,CALC,RATE,SUM(AMOUNT)AMOUNT"
                    'StrSql += vbCrLf + " ,METALID,SEIVE,STUDDEDUCT,RESNO"
                    'StrSql += vbCrLf + " FROM"
                    'StrSql += vbCrLf + " ("

                    'StrSql += vbCrLf + "  SELECT "
                    'StrSql += vbCrLf + "  IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
                    'StrSql += vbCrLf + "  ,ST.STNPCS AS PCS,(CASE WHEN ST.STONEUNIT='G' THEN ST.STNWT*5 ELSE ST.STNWT END ) AS WEIGHT"
                    'StrSql += vbCrLf + "  ,'C' AS UNIT,ST.CALCMODE AS CALC,ST.STNRATE AS RATE,ST.STNAMT AS AMOUNT"
                    'StrSql += vbCrLf + "  ,IM.DIASTONE AS METALID,ISNULL(SEIVE,'') AS SEIVE"
                    'StrSql += vbCrLf + "  ,CASE WHEN ISNULL(ST.STUDDEDUCT,'Y')='Y' THEN 'YES' ELSE 'NO' END AS STUDDEDUCT"
                    'StrSql += vbCrLf + "  ,SNO AS RESNO"
                    'StrSql += vbCrLf + "  FROM " & cnStockDb & "..ISSSTONE AS ST"
                    'StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
                    'StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = ST.STNITEMID AND SM.SUBITEMID = ST.STNSUBITEMID"
                    'StrSql += vbCrLf + "  WHERE ST.ISSSNO = '" & dtStk.Rows(0).Item("SNO").ToString & "'"
                    'StrSql += vbCrLf + "  AND ISSSNO IN(SELECT SNO FROM " & cnStockDb & "..ISSUE WHERE ISNULL(CANCEL,'')='')"
                    'If MCostId <> "" Then StrSql += vbCrLf + "  AND ST.COSTID='" & MCostId & "'"

                    'StrSql += vbCrLf + "  "
                    'StrSql += vbCrLf + "  UNION ALL"
                    'StrSql += vbCrLf + "  SELECT "
                    'StrSql += vbCrLf + "  IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
                    'StrSql += vbCrLf + "  ,-1*ST.STNPCS AS PCS,-1*(CASE WHEN ST.STONEUNIT='G' THEN ST.STNWT*5 ELSE ST.STNWT END) AS WEIGHT"
                    'StrSql += vbCrLf + "  ,'C' AS UNIT,ST.CALCMODE AS CALC,ST.STNRATE AS RATE,-1*ST.STNAMT AS AMOUNT"
                    'StrSql += vbCrLf + "  ,IM.DIASTONE AS METALID,ISNULL(SEIVE,'') AS SEIVE"
                    'StrSql += vbCrLf + "  ,CASE WHEN ISNULL(ST.STUDDEDUCT,'Y')='Y' THEN 'YES' ELSE 'NO' END AS STUDDEDUCT"
                    'StrSql += vbCrLf + "  ,ISSNO AS RESNO"
                    'StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPTSTONE AS ST"
                    'StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
                    'StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = ST.STNITEMID AND SM.SUBITEMID = ST.STNSUBITEMID"
                    'StrSql += vbCrLf + "  WHERE "
                    'StrSql += vbCrLf + "  ST.ISSNO IN "
                    'StrSql += vbCrLf + "  (SELECT SNO FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO= '" & dtStk.Rows(0).Item("SNO").ToString & "'"
                    'StrSql += vbCrLf + "  AND ISSSNO IN(SELECT SNO FROM " & cnStockDb & "..ISSUE WHERE ISNULL(CANCEL,'')='')"
                    'StrSql += vbCrLf + "  )"
                    'If MCostId <> "" Then StrSql += vbCrLf + "  AND ST.COSTID='" & MCostId & "'"
                    'StrSql += vbCrLf + "  "

                    'StrSql += vbCrLf + " UNION ALL"
                    'StrSql += vbCrLf + " SELECT "
                    'StrSql += vbCrLf + " IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
                    'StrSql += vbCrLf + " ,-1*IL.STNPCS AS PCS,-1*(CASE WHEN IL.STNUNIT='G' THEN IL.STNWT*5 ELSE IL.STNWT END) AS WEIGHT"
                    'StrSql += vbCrLf + " ,'C' AS UNIT,ST.CALCMODE AS CALC,ST.STNRATE AS RATE,ST.STNAMT AS AMOUNT"
                    'StrSql += vbCrLf + " ,IM.DIASTONE AS METALID,ISNULL(SEIVE,'') AS SEIVE"
                    'StrSql += vbCrLf + " ,CASE WHEN ISNULL(ST.STUDDEDUCT,'Y')='Y' THEN 'YES' ELSE 'NO' END AS STUDDEDUCT"
                    'StrSql += vbCrLf + " ,ST.SNO AS RESNO"
                    'StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE AS ST"
                    'StrSql += vbCrLf + " INNER JOIN " & cnStockDb & "..LOTISSUE L ON L.RECSNO=ST.ISSSNO"
                    'StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMLOT IL ON L.LOTSNO=IL.SNO"
                    'StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
                    'StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = ST.STNITEMID AND SM.SUBITEMID = ST.STNSUBITEMID"
                    'StrSql += vbCrLf + " WHERE ST.ISSSNO = '" & dtStk.Rows(0).Item("SNO").ToString & "'"
                    'StrSql += vbCrLf + " AND ISSSNO IN(SELECT SNO FROM " & cnStockDb & "..ISSUE WHERE ISNULL(CANCEL,'')='')"
                    'If MCostId <> "" Then StrSql += vbCrLf + "  AND ST.COSTID='" & MCostId & "'"
                    'StrSql += vbCrLf + " )X GROUP BY RESNO,ITEM,SUBITEM,UNIT,CALC,RATE,METALID,SEIVE,STUDDEDUCT  HAVING SUM(WEIGHT)<>0 "
                    'Dim dtStone As New DataTable
                    'Da = New OleDbDataAdapter(StrSql, cn)
                    'Da.Fill(dtStone)
                    'objStone.dtGridStone.Clear()
                    'For Each RoStn As DataRow In dtStone.Rows
                    '    Dim RoStnIm As DataRow = objStone.dtGridStone.NewRow
                    '    For Each Col As DataColumn In dtStone.Columns
                    '        RoStnIm.Item(Col.ColumnName) = RoStn.Item(Col.ColumnName)
                    '    Next
                    '    objStone.dtGridStone.Rows.Add(RoStnIm)
                    'Next
                    'objStone.dtGridStone.AcceptChanges()
                    'objStone.CalcStoneWtAmount()
                    'RESNO = dtStk.Rows(0).Item("SNO").ToString

                    'StkPcs = Val(dtStk.Compute("SUM(PCS)", Nothing).ToString)
                    StkPcs = 10000
                    StkGrsWt = Val(dtStk.Compute("SUM(GRSWT)", Nothing).ToString)
                    If rbtOrnament.Checked Then
                        Dim Assignval As Boolean = True
                        'txtOOrdNo.Text = TranNo
                        'cmbOCategory.Text = dtStk.Rows(0).Item("CATEGORY").ToString
                        cmbOMetal.Text = dtStk.Rows(0).Item("METAL").ToString
                        cmbOMetal.Enabled = False
                        If Assignval Then
                            txtOPcs_NUM.Text = Val(dtStk.Compute("SUM(PCS)", Nothing).ToString)
                            txtOGrsWt_WET.Text = Val(dtStk.Compute("SUM(GRSWT)", Nothing).ToString)
                            txtONetWt_WET.Text = Val(dtStk.Compute("SUM(NETWT)", Nothing).ToString)
                        End If
                    ElseIf rbtMetal.Checked Then
                        Dim Assignval As Boolean = True
                        'txtMOrdNo.Text = TranNo
                        'cmbMCategory.Text = dtStk.Rows(0).Item("CATEGORY").ToString
                        cmbMMetal.Text = dtStk.Rows(0).Item("METAL").ToString
                        cmbMMetal.Enabled = False
                        If Assignval Then
                            txtMPcs_NUM.Text = Val(dtStk.Compute("SUM(PCS)", Nothing).ToString)
                            txtMGrsWt_WET.Text = Val(dtStk.Compute("SUM(GRSWT)", Nothing).ToString)
                            txtMNetWt_WET.Text = Val(dtStk.Compute("SUM(NETWT)", Nothing).ToString)
                        End If
                    ElseIf rbtStone.Checked Then
                        Dim Assignval As Boolean = True
                        'txtSOrdNo.Text = TranNo
                        LoadMetal(cmbSMetal)
                        cmbSMetal.Text = dtStk.Rows(0).Item("METAL").ToString
                        ''cmbSCategory.Text = dtStk.Rows(0).Item("CATEGORY").ToString
                        cmbSItem.Text = dtStk.Rows(0).Item("ITEM").ToString
                        cmbSSubItem.Text = dtStk.Rows(0).Item("SUBITEM").ToString
                        cmbSUnit.Text = IIf(dtStk.Rows(0).Item("UNIT").ToString = "G", "GRAM", "CARAT")
                        StkUnit = dtStk.Rows(0).Item("UNIT").ToString
                        If Assignval Then
                            txtSPcs_NUM.Text = Val(dtStk.Compute("SUM(PCS)", Nothing).ToString)
                            txtSGrsWt_WET.Text = Val(dtStk.Compute("SUM(GRSWT)", Nothing).ToString)
                        End If
                        'cmbSMetal.Enabled = False
                    End If
                    Return True
                Else
                    StkRow = BrighttechPack.SearchDialog.Show_R("Find ", mStrSql, cn, , 1)
                    If Not StkRow Is Nothing Then
                        StrSql = vbCrLf + " SELECT ITEM,SUBITEM"
                        StrSql += vbCrLf + " ,SUM(PCS)PCS,SUM(WEIGHT) WEIGHT"
                        StrSql += vbCrLf + " ,UNIT,CALC,RATE,SUM(AMOUNT)AMOUNT"
                        StrSql += vbCrLf + " ,METALID,SEIVE,RESNO"
                        StrSql += vbCrLf + " FROM"
                        StrSql += vbCrLf + " ("
                        StrSql += vbCrLf + "  SELECT "
                        StrSql += vbCrLf + "  IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
                        StrSql += vbCrLf + "  ,ST.STNPCS AS PCS,ST.STNWT AS WEIGHT"
                        StrSql += vbCrLf + "  ,ST.STONEUNIT AS UNIT,ST.CALCMODE AS CALC,ST.STNRATE AS RATE,ST.STNAMT AS AMOUNT"
                        StrSql += vbCrLf + "  ,IM.DIASTONE AS METALID,ISNULL(SEIVE,'') AS SEIVE"
                        StrSql += vbCrLf + "  ,SNO AS RESNO"
                        StrSql += vbCrLf + "  FROM " & cnStockDb & "..ISSSTONE AS ST"
                        StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
                        StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = ST.STNITEMID AND SM.SUBITEMID = ST.STNSUBITEMID"
                        StrSql += vbCrLf + "  WHERE ST.ISSSNO = '" & StkRow.Item("SNO").ToString & "'"
                        StrSql += vbCrLf + "  AND ISSSNO IN(SELECT ISSNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(CANCEL,'')='' AND ACCODE ='" & Maccode & "')"
                        If MCostId <> "" Then StrSql += vbCrLf + "  And ST.COSTID='" & MCostId & "'"
                        StrSql += vbCrLf + "  /*"
                        StrSql += vbCrLf + "  UNION ALL"
                        StrSql += vbCrLf + "  SELECT "
                        StrSql += vbCrLf + "  IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
                        StrSql += vbCrLf + "  ,-1*ST.STNPCS AS PCS,-1*ST.STNWT AS WEIGHT"
                        StrSql += vbCrLf + "  ,ST.STONEUNIT AS UNIT,ST.CALCMODE AS CALC,ST.STNRATE AS RATE,-1*ST.STNAMT AS AMOUNT"
                        StrSql += vbCrLf + "  ,IM.DIASTONE AS METALID,ISNULL(SEIVE,'') AS SEIVE"
                        StrSql += vbCrLf + "  ,RESNO AS RESNO"
                        StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPTSTONE AS ST"
                        StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
                        StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = ST.STNITEMID AND SM.SUBITEMID = ST.STNSUBITEMID"
                        StrSql += vbCrLf + "  WHERE "
                        StrSql += vbCrLf + "  ST.RESNO IN "
                        StrSql += vbCrLf + "  (SELECT SNO FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO= '" & StkRow.Item("SNO").ToString & "'"
                        StrSql += vbCrLf + "  AND ISSSNO IN(SELECT ISSNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(CANCEL,'')='' AND ACCODE ='" & Maccode & "')"
                        StrSql += vbCrLf + "  )"
                        StrSql += vbCrLf + "  */"
                        If MCostId <> "" Then StrSql += vbCrLf + "  And ST.COSTID='" & MCostId & "'"
                        StrSql += vbCrLf + " )X GROUP BY RESNO,ITEM,SUBITEM,UNIT,CALC,RATE,METALID,SEIVE  HAVING SUM(WEIGHT)<>0 "
                        Dim dtStone As New DataTable
                        Da = New OleDbDataAdapter(StrSql, cn)
                        Da.Fill(dtStone)
                        objStone.dtGridStone.Clear()
                        For Each RoStn As DataRow In dtStone.Rows
                            Dim RoStnIm As DataRow = objStone.dtGridStone.NewRow
                            For Each Col As DataColumn In dtStone.Columns
                                RoStnIm.Item(Col.ColumnName) = RoStn.Item(Col.ColumnName)
                            Next
                            objStone.dtGridStone.Rows.Add(RoStnIm)
                        Next
                        objStone.dtGridStone.AcceptChanges()
                        objStone.CalcStoneWtAmount()
                        RESNO = StkRow.Item("SNO").ToString
                        'StkPcs = Val(StkRow.Item("PCS").ToString)
                        StkPcs = 10000
                        StkGrsWt = Val(StkRow.Item("GRSWT").ToString)
                        If rbtOrnament.Checked Then
                            Dim Assignval As Boolean = True
                            'txtOOrdNo.Text = TranNo
                            'cmbOCategory.Text = StkRow.Item("CATEGORY").ToString
                            cmbOMetal.Text = StkRow.Item("METAL").ToString
                            cmbOMetal.Enabled = False
                            If Assignval Then
                                txtOPcs_NUM.Text = Val(StkRow.Item("PCS").ToString)
                                txtOGrsWt_WET.Text = Val(StkRow.Item("GRSWT").ToString)
                                txtONetWt_WET.Text = Val(StkRow.Item("NETWT").ToString)
                            End If
                            'cmbOCategory.Enabled = False
                        ElseIf rbtMetal.Checked Then
                            Dim Assignval As Boolean = True
                            'txtMOrdNo.Text = TranNo
                            'cmbMCategory.Text = StkRow.Item("CATEGORY").ToString
                            cmbMMetal.Text = StkRow.Item("METAL").ToString
                            cmbMMetal.Enabled = False
                            If Assignval Then
                                txtMPcs_NUM.Text = Val(StkRow.Item("PCS").ToString)
                                txtMGrsWt_WET.Text = Val(StkRow.Item("GRSWT").ToString)
                                txtMNetWt_WET.Text = Val(StkRow.Item("NETWT").ToString)
                            End If
                            'cmbMCategory.Enabled = False
                        ElseIf rbtStone.Checked Then
                            Dim Assignval As Boolean = True
                            'txtSOrdNo.Text = TranNo
                            LoadMetal(cmbSMetal)
                            cmbSMetal.Text = StkRow.Item("METAL").ToString
                            'cmbSCategory.Text = StkRow.Item("CATEGORY").ToString
                            cmbSItem.Text = StkRow.Item("ITEM").ToString
                            cmbSSubItem.Text = StkRow.Item("SUBITEM").ToString
                            cmbSUnit.Text = IIf(StkRow.Item("UNIT").ToString = "G", "GRAM", "CARAT")
                            StkUnit = StkRow.Item("UNIT").ToString
                            If Assignval Then
                                txtSPcs_NUM.Text = Val(StkRow.Item("PCS").ToString)
                                txtSGrsWt_WET.Text = Val(StkRow.Item("GRSWT").ToString)
                            End If
                            'cmbSMetal.Enabled = False
                            'cmbSCategory.Enabled = False
                        End If
                        Return True
                    Else
                        MsgBox("Record Not Found.", MsgBoxStyle.Information)
                        If rbtOrnament.Checked Then
                            txtOPcs_NUM.Clear()
                            txtOGrsWt_WET.Clear()
                            txtONetWt_WET.Clear()
                            txtOTouchAMT.Clear()
                        ElseIf rbtMetal.Checked Then
                            txtMPcs_NUM.Clear()
                            txtMGrsWt_WET.Clear()
                            txtMNetWt_WET.Clear()
                            txtMTouchAMT.Clear()
                        ElseIf rbtStone.Checked Then
                            txtSPcs_NUM.Clear()
                            txtSGrsWt_WET.Clear()
                        End If
                        Return False
                    End If
                End If
            Else
                MsgBox("Record Not Found.", MsgBoxStyle.Information)
                If rbtOrnament.Checked Then
                    txtOPcs_NUM.Clear()
                    txtOGrsWt_WET.Clear()
                    txtONetWt_WET.Clear()
                    txtOTouchAMT.Clear()
                ElseIf rbtMetal.Checked Then
                    txtMPcs_NUM.Clear()
                    txtMGrsWt_WET.Clear()
                    txtMNetWt_WET.Clear()
                    txtMTouchAMT.Clear()
                ElseIf rbtStone.Checked Then
                    txtSPcs_NUM.Clear()
                    txtSGrsWt_WET.Clear()
                End If
                Return False
            End If
        Else
            Return True
        End If
    End Function


    Function LoadBalanceStock() As Boolean
        If txtSOrdNo.Text <> "" Or txtOOrdNo.Text <> "" Or txtMOrdNo.Text <> "" Then
            Dim TranNo As String = ""
            Dim Metal As String = ""
            If rbtOrnament.Checked Then
                TranNo = txtOOrdNo.Text
                Metal = "O"
            ElseIf rbtMetal.Checked Then
                TranNo = txtMOrdNo.Text
                Metal = "M"
            ElseIf rbtStone.Checked Then
                TranNo = txtSOrdNo.Text
                Metal = "S"
            End If
            Dim mStrSql As String = ""
            If oMaterial = Material.Issue Then
                mStrSql = vbCrLf + " SELECT SNO,METAL"
                mStrSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE="
                mStrSql += vbCrLf + " (SELECT TOP 1 CATCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO))CATEGORY"
                If Metal = "S" Then
                    mStrSql += vbCrLf + " ,ITEM,SUBITEM"
                    mStrSql += vbCrLf + " ,(SELECT STONEUNIT FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)UNIT"
                End If
                mStrSql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT"
                mStrSql += vbCrLf + " FROM"
                mStrSql += vbCrLf + " ("
                mStrSql += vbCrLf + " SELECT SNO,M.METALNAME METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                mStrSql += vbCrLf + " ,SUM(ISNULL(PCS,0)) PCS"
                mStrSql += vbCrLf + " ,SUM(ISNULL(R.GRSWT,0)) GRSWT "
                mStrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) NETWT"
                mStrSql += vbCrLf + " FROM " & cnStockDb & "..OPENWEIGHT R "
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON C.METALID = M.METALID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = R.ITEMID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = R.SUBITEMID"
                mStrSql += vbCrLf + " WHERE R.STOCKTYPE='C'"
                If MCostId <> "" Then mStrSql += vbCrLf + " AND R.COSTID='" & MCostId & "'"
                If Metal = "S" Then
                    mStrSql += vbCrLf + " AND M.TTYPE='" & Metal & "'"
                Else
                    mStrSql += vbCrLf + " AND C.PURITYID IN(SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALTYPE='" & Metal & "')"
                End If
                If InsSno <> "" Then mStrSql += vbCrLf + " AND R.SNO='" & InsSno & "'"
                mStrSql += vbCrLf + " GROUP BY SNO,M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME  "
                mStrSql += vbCrLf + " UNION ALL"
                mStrSql += vbCrLf + " SELECT SNO,M.METALNAME METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                mStrSql += vbCrLf + " ,SUM(ISNULL(PCS,0)) PCS"
                mStrSql += vbCrLf + " ,SUM(ISNULL(R.GRSWT,0)) GRSWT "
                mStrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) NETWT"
                mStrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT R "
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON R.METALID = M.METALID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = R.ITEMID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = R.SUBITEMID"
                mStrSql += vbCrLf + " WHERE R.TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "'"
                mStrSql += vbCrLf + " AND LEN(R.TRANTYPE)>2"
                'mStrSql += vbCrLf + " AND R.TRANTYPE<>'RIN'"
                'mStrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                If MCostId <> "" Then mStrSql += vbCrLf + " AND R.COSTID='" & MCostId & "'"
                mStrSql += vbCrLf + " AND TRANNO =" & TranNo
                If Metal = "S" Then
                    mStrSql += vbCrLf + " AND M.TTYPE='" & Metal & "'"
                Else
                    mStrSql += vbCrLf + " AND C.PURITYID IN(SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALTYPE='" & Metal & "')"
                End If
                mStrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
                If InsSno <> "" Then mStrSql += vbCrLf + " AND R.SNO='" & InsSno & "'"
                mStrSql += vbCrLf + " GROUP BY SNO,M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME  "
                mStrSql += vbCrLf + " UNION ALL"
                mStrSql += vbCrLf + " SELECT RESNO AS SNO,M.METALNAME AS METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                mStrSql += vbCrLf + " ,SUM(ISNULL(PCS,0))*(-1) PCS"
                If Metal = "S" Then
                    mStrSql += vbCrLf + " ,CASE "
                    mStrSql += vbCrLf + " WHEN (SELECT STONEUNIT FROM " & cnStockDb & "..RECEIPT WHERE SNO=I.RESNO)='G' AND I.STONEUNIT ='C' THEN (SUM(ISNULL(I.GRSWT,0))/5)*(-1) "
                    mStrSql += vbCrLf + " WHEN (SELECT STONEUNIT FROM " & cnStockDb & "..RECEIPT WHERE SNO=I.RESNO)='C' AND I.STONEUNIT ='G' THEN (SUM(ISNULL(I.GRSWT,0))*5)*(-1) "
                    mStrSql += vbCrLf + " ELSE SUM(ISNULL(I.GRSWT,0))*(-1) END GRSWT"
                    mStrSql += vbCrLf + " ,CASE "
                    mStrSql += vbCrLf + " WHEN (SELECT STONEUNIT FROM " & cnStockDb & "..RECEIPT WHERE SNO=I.RESNO)='G' AND I.STONEUNIT ='C' THEN (SUM(ISNULL(I.NETWT,0))/5)*(-1) "
                    mStrSql += vbCrLf + " WHEN (SELECT STONEUNIT FROM " & cnStockDb & "..RECEIPT WHERE SNO=I.RESNO)='C' AND I.STONEUNIT ='G' THEN (SUM(ISNULL(I.NETWT,0))*5)*(-1) "
                    mStrSql += vbCrLf + " ELSE SUM(ISNULL(I.NETWT,0))*(-1) END NETWT"
                Else
                    mStrSql += vbCrLf + " ,SUM(ISNULL(I.GRSWT,0))*(-1) GRSWT "
                    mStrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0))*(-1) NETWT"
                End If
                mStrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE  I "
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = I.ITEMID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = I.SUBITEMID"
                mStrSql += vbCrLf + " WHERE I.TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "'"
                mStrSql += vbCrLf + " AND LEN(I.TRANTYPE)>2"
                mStrSql += vbCrLf + " AND I.TRANTYPE<>'IIN'"
                'mStrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                If MCostId <> "" Then mStrSql += vbCrLf + " AND I.COSTID='" & MCostId & "'"
                mStrSql += vbCrLf + " AND RESNO IN"
                mStrSql += vbCrLf + " (SELECT SNO FROM " & cnStockDb & "..OPENWEIGHT UNION ALL SELECT SNO FROM " & cnStockDb & "..RECEIPT "
                mStrSql += vbCrLf + " WHERE TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "' "
                mStrSql += vbCrLf + " AND TRANNO=" & TranNo & " AND ISNULL(CANCEL,'')='' "
                If MCostId <> "" Then mStrSql += vbCrLf + " AND COSTID='" & MCostId & "'"
                mStrSql += vbCrLf + " AND LEN(TRANTYPE)>2 "
                'mStrSql += vbCrLf + "AND TRANTYPE<>'RIN'"
                mStrSql += vbCrLf + ")"
                If Metal = "S" Then
                    mStrSql += vbCrLf + " AND M.TTYPE='" & Metal & "'"
                Else
                    mStrSql += vbCrLf + " AND C.PURITYID IN(SELECT PURITYID FROM " & cnAdminDb & "..PURITYMAST WHERE METALTYPE='" & Metal & "')"
                End If
                mStrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
                If InsSno <> "" Then mStrSql += vbCrLf + " AND I.RESNO='" & InsSno & "'"
                mStrSql += vbCrLf + " GROUP BY RESNO,M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME  "
                If Metal = "S" Then
                    mStrSql += vbCrLf + " ,I.STONEUNIT"
                End If
                mStrSql += vbCrLf + " UNION ALL"
                mStrSql += vbCrLf + " SELECT RECSNO AS SNO,M.METALNAME AS METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                mStrSql += vbCrLf + " ,SUM(ISNULL(PCS,0))*(-1) PCS"
                mStrSql += vbCrLf + " ,SUM(ISNULL(I.GRSWT,0))*(-1) GRSWT "
                mStrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0))*(-1) NETWT"
                mStrSql += vbCrLf + " FROM " & cnStockDb & "..LOTISSUE  I "
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = I.ITEMID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = IM.CATCODE"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON IM.METALID = M.METALID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = I.SUBITEMID"
                mStrSql += vbCrLf + " WHERE  "
                mStrSql += vbCrLf + " RECSNO IN"
                mStrSql += vbCrLf + " (SELECT SNO FROM " & cnStockDb & "..OPENWEIGHT "
                mStrSql += vbCrLf + " UNION ALL "
                mStrSql += vbCrLf + " SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE<='" & Format(oTranDate, "yyyy-MM-dd") & "' AND ISNULL(CANCEL,'')=''"
                mStrSql += vbCrLf + " AND LEN(TRANTYPE)>2 "
                'mStrSql += vbCrLf + "AND TRANTYPE<>'RIN' "
                mStrSql += vbCrLf + " )"
                mStrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
                mStrSql += vbCrLf + " GROUP BY RECSNO,M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME  "
                mStrSql += vbCrLf + " )X GROUP BY SNO,METAL  "
                If Metal = "S" Then
                    mStrSql += vbCrLf + " ,ITEM,SUBITEM"
                End If
                mStrSql += vbCrLf + " HAVING (SUM(GRSWT)>0  OR SUM(PCS)>0)"
                mStrSql += vbCrLf + " ORDER BY CAST(SUBSTRING(SNO,7,LEN(SNO)) AS INT) ASC"
            End If
            If mStrSql.Trim = "" Then Return True
            Dim dtStk As New DataTable
            Cmd = New OleDbCommand(mStrSql, cn)
            Da = New OleDbDataAdapter(Cmd)
            Da.Fill(dtStk)
            Dim StkRow As DataRow
            If dtStk.Rows.Count > 0 Then
                If dtStk.Rows.Count = 1 Then
                    StrSql = vbCrLf + " SELECT ITEM,SUBITEM"
                    StrSql += vbCrLf + " ,SUM(PCS)PCS,SUM(WEIGHT) WEIGHT"
                    StrSql += vbCrLf + " ,UNIT,CALC,RATE,SUM(AMOUNT)AMOUNT"
                    StrSql += vbCrLf + " ,METALID,SEIVE,STUDDEDUCT,RESNO"
                    StrSql += vbCrLf + " FROM"
                    StrSql += vbCrLf + " ("
                    StrSql += vbCrLf + "  SELECT "
                    StrSql += vbCrLf + "  IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
                    StrSql += vbCrLf + "  ,ST.STNPCS AS PCS,(CASE WHEN ST.STONEUNIT='G' THEN ST.STNWT*5 ELSE ST.STNWT END ) AS WEIGHT"
                    StrSql += vbCrLf + "  ,'C' AS UNIT,ST.CALCMODE AS CALC,ST.STNRATE AS RATE,ST.STNAMT AS AMOUNT"
                    StrSql += vbCrLf + "  ,IM.DIASTONE AS METALID,ISNULL(SEIVE,'') AS SEIVE"
                    StrSql += vbCrLf + "  ,CASE WHEN ISNULL(ST.STUDDEDUCT,'Y')='Y' THEN 'YES' ELSE 'NO' END AS STUDDEDUCT"
                    StrSql += vbCrLf + "  ,SNO AS RESNO"
                    StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPTSTONE AS ST"
                    StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
                    StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = ST.STNITEMID AND SM.SUBITEMID = ST.STNSUBITEMID"
                    StrSql += vbCrLf + "  WHERE ST.ISSSNO = '" & dtStk.Rows(0).Item("SNO").ToString & "'"
                    StrSql += vbCrLf + "  AND ISSSNO IN(SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(CANCEL,'')='')"
                    If MCostId <> "" Then StrSql += vbCrLf + "  AND ST.COSTID='" & MCostId & "'"
                    StrSql += vbCrLf + "  UNION ALL"
                    StrSql += vbCrLf + "  SELECT "
                    StrSql += vbCrLf + "  IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
                    StrSql += vbCrLf + "  ,-1*ST.STNPCS AS PCS,-1*(CASE WHEN ST.STONEUNIT='G' THEN ST.STNWT*5 ELSE ST.STNWT END) AS WEIGHT"
                    StrSql += vbCrLf + "  ,'C' AS UNIT,ST.CALCMODE AS CALC,ST.STNRATE AS RATE,-1*ST.STNAMT AS AMOUNT"
                    StrSql += vbCrLf + "  ,IM.DIASTONE AS METALID,ISNULL(SEIVE,'') AS SEIVE"
                    StrSql += vbCrLf + "  ,CASE WHEN ISNULL(ST.STUDDEDUCT,'Y')='Y' THEN 'YES' ELSE 'NO' END AS STUDDEDUCT"
                    StrSql += vbCrLf + "  ,RESNO AS RESNO"
                    StrSql += vbCrLf + "  FROM " & cnStockDb & "..ISSSTONE AS ST"
                    StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
                    StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = ST.STNITEMID AND SM.SUBITEMID = ST.STNSUBITEMID"
                    StrSql += vbCrLf + "  WHERE "
                    StrSql += vbCrLf + "  ST.RESNO IN "
                    StrSql += vbCrLf + "  (SELECT SNO FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO= '" & dtStk.Rows(0).Item("SNO").ToString & "'"
                    StrSql += vbCrLf + "  AND ISSSNO IN(SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(CANCEL,'')='')"
                    StrSql += vbCrLf + "  )"
                    If MCostId <> "" Then StrSql += vbCrLf + "  AND ST.COSTID='" & MCostId & "'"
                    StrSql += vbCrLf + " UNION ALL"
                    StrSql += vbCrLf + " SELECT "
                    StrSql += vbCrLf + " IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
                    StrSql += vbCrLf + " ,-1*IL.STNPCS AS PCS,-1*(CASE WHEN IL.STNUNIT='G' THEN IL.STNWT*5 ELSE IL.STNWT END) AS WEIGHT"
                    StrSql += vbCrLf + " ,'C' AS UNIT,ST.CALCMODE AS CALC,ST.STNRATE AS RATE,ST.STNAMT AS AMOUNT"
                    StrSql += vbCrLf + " ,IM.DIASTONE AS METALID,ISNULL(SEIVE,'') AS SEIVE"
                    StrSql += vbCrLf + " ,CASE WHEN ISNULL(ST.STUDDEDUCT,'Y')='Y' THEN 'YES' ELSE 'NO' END AS STUDDEDUCT"
                    StrSql += vbCrLf + " ,ST.SNO AS RESNO"
                    StrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE AS ST"
                    StrSql += vbCrLf + " INNER JOIN " & cnStockDb & "..LOTISSUE L ON L.RECSNO=ST.ISSSNO"
                    StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMLOT IL ON L.LOTSNO=IL.SNO"
                    StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = ST.STNITEMID AND SM.SUBITEMID = ST.STNSUBITEMID"
                    StrSql += vbCrLf + " WHERE ST.ISSSNO = '" & dtStk.Rows(0).Item("SNO").ToString & "'"
                    StrSql += vbCrLf + " AND ISSSNO IN(SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(CANCEL,'')='')"
                    If MCostId <> "" Then StrSql += vbCrLf + "  AND ST.COSTID='" & MCostId & "'"
                    StrSql += vbCrLf + " )X GROUP BY RESNO,ITEM,SUBITEM,UNIT,CALC,RATE,METALID,SEIVE,STUDDEDUCT  HAVING SUM(WEIGHT)<>0 "
                    Dim dtStone As New DataTable
                    Da = New OleDbDataAdapter(StrSql, cn)
                    Da.Fill(dtStone)
                    objStone.dtGridStone.Clear()
                    For Each RoStn As DataRow In dtStone.Rows
                        Dim RoStnIm As DataRow = objStone.dtGridStone.NewRow
                        For Each Col As DataColumn In dtStone.Columns
                            RoStnIm.Item(Col.ColumnName) = RoStn.Item(Col.ColumnName)
                        Next
                        objStone.dtGridStone.Rows.Add(RoStnIm)
                    Next
                    objStone.dtGridStone.AcceptChanges()
                    objStone.CalcStoneWtAmount()
                    RESNO = dtStk.Rows(0).Item("SNO").ToString
                    StkPcs = Val(dtStk.Compute("SUM(PCS)", Nothing).ToString)
                    StkGrsWt = Val(dtStk.Compute("SUM(GRSWT)", Nothing).ToString)
                    If rbtOrnament.Checked Then
                        Dim Assignval As Boolean = True
                        txtOOrdNo.Text = TranNo
                        cmbOCategory.Text = dtStk.Rows(0).Item("CATEGORY").ToString
                        cmbOMetal.Text = dtStk.Rows(0).Item("METAL").ToString
                        cmbOMetal.Enabled = False
                        If Assignval Then
                            txtOPcs_NUM.Text = Val(dtStk.Compute("SUM(PCS)", Nothing).ToString)
                            txtOGrsWt_WET.Text = Val(dtStk.Compute("SUM(GRSWT)", Nothing).ToString)
                            txtONetWt_WET.Text = Val(dtStk.Compute("SUM(NETWT)", Nothing).ToString)
                        End If
                    ElseIf rbtMetal.Checked Then
                        Dim Assignval As Boolean = True
                        txtMOrdNo.Text = TranNo
                        cmbMCategory.Text = dtStk.Rows(0).Item("CATEGORY").ToString
                        cmbMMetal.Text = dtStk.Rows(0).Item("METAL").ToString
                        cmbMMetal.Enabled = False
                        If Assignval Then
                            txtMPcs_NUM.Text = Val(dtStk.Compute("SUM(PCS)", Nothing).ToString)
                            txtMGrsWt_WET.Text = Val(dtStk.Compute("SUM(GRSWT)", Nothing).ToString)
                            txtMNetWt_WET.Text = Val(dtStk.Compute("SUM(NETWT)", Nothing).ToString)
                        End If
                    ElseIf rbtStone.Checked Then
                        Dim Assignval As Boolean = True
                        txtSOrdNo.Text = TranNo
                        LoadMetal(cmbSMetal)
                        cmbSMetal.Text = dtStk.Rows(0).Item("METAL").ToString
                        cmbSCategory.Text = dtStk.Rows(0).Item("CATEGORY").ToString
                        cmbSItem.Text = dtStk.Rows(0).Item("ITEM").ToString
                        cmbSSubItem.Text = dtStk.Rows(0).Item("SUBITEM").ToString
                        cmbSUnit.Text = IIf(dtStk.Rows(0).Item("UNIT").ToString = "G", "GRAM", "CARAT")
                        StkUnit = dtStk.Rows(0).Item("UNIT").ToString
                        If Assignval Then
                            txtSPcs_NUM.Text = Val(dtStk.Compute("SUM(PCS)", Nothing).ToString)
                            txtSGrsWt_WET.Text = Val(dtStk.Compute("SUM(GRSWT)", Nothing).ToString)
                        End If
                        'cmbSMetal.Enabled = False
                    End If
                    Return True
                Else
                    StkRow = BrighttechPack.SearchDialog.Show_R("Find ", mStrSql, cn, , 1)
                    If Not StkRow Is Nothing Then
                        StrSql = vbCrLf + " SELECT ITEM,SUBITEM"
                        StrSql += vbCrLf + " ,SUM(PCS)PCS,SUM(WEIGHT) WEIGHT"
                        StrSql += vbCrLf + " ,UNIT,CALC,RATE,SUM(AMOUNT)AMOUNT"
                        StrSql += vbCrLf + " ,METALID,SEIVE,RESNO"
                        StrSql += vbCrLf + " FROM"
                        StrSql += vbCrLf + " ("
                        StrSql += vbCrLf + "  SELECT "
                        StrSql += vbCrLf + "  IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
                        StrSql += vbCrLf + "  ,ST.STNPCS AS PCS,ST.STNWT AS WEIGHT"
                        StrSql += vbCrLf + "  ,ST.STONEUNIT AS UNIT,ST.CALCMODE AS CALC,ST.STNRATE AS RATE,ST.STNAMT AS AMOUNT"
                        StrSql += vbCrLf + "  ,IM.DIASTONE AS METALID,ISNULL(SEIVE,'') AS SEIVE"
                        StrSql += vbCrLf + "  ,SNO AS RESNO"
                        StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPTSTONE AS ST"
                        StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
                        StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = ST.STNITEMID AND SM.SUBITEMID = ST.STNSUBITEMID"
                        StrSql += vbCrLf + "  WHERE ST.ISSSNO = '" & StkRow.Item("SNO").ToString & "'"
                        StrSql += vbCrLf + "  AND ISSSNO IN(SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(CANCEL,'')='')"
                        If MCostId <> "" Then StrSql += vbCrLf + "  AND ST.COSTID='" & MCostId & "'"
                        StrSql += vbCrLf + "  UNION ALL"
                        StrSql += vbCrLf + "  SELECT "
                        StrSql += vbCrLf + "  IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
                        StrSql += vbCrLf + "  ,-1*ST.STNPCS AS PCS,-1*ST.STNWT AS WEIGHT"
                        StrSql += vbCrLf + "  ,ST.STONEUNIT AS UNIT,ST.CALCMODE AS CALC,ST.STNRATE AS RATE,-1*ST.STNAMT AS AMOUNT"
                        StrSql += vbCrLf + "  ,IM.DIASTONE AS METALID,ISNULL(SEIVE,'') AS SEIVE"
                        StrSql += vbCrLf + "  ,RESNO AS RESNO"
                        StrSql += vbCrLf + "  FROM " & cnStockDb & "..ISSSTONE AS ST"
                        StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
                        StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = ST.STNITEMID AND SM.SUBITEMID = ST.STNSUBITEMID"
                        StrSql += vbCrLf + "  WHERE "
                        StrSql += vbCrLf + "  ST.RESNO IN "
                        StrSql += vbCrLf + "  (SELECT SNO FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO= '" & StkRow.Item("SNO").ToString & "'"
                        StrSql += vbCrLf + "  AND ISSSNO IN(SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(CANCEL,'')='')"
                        StrSql += vbCrLf + "  )"
                        If MCostId <> "" Then StrSql += vbCrLf + "  AND ST.COSTID='" & MCostId & "'"
                        StrSql += vbCrLf + " )X GROUP BY RESNO,ITEM,SUBITEM,UNIT,CALC,RATE,METALID,SEIVE  HAVING SUM(WEIGHT)<>0 "
                        Dim dtStone As New DataTable
                        Da = New OleDbDataAdapter(StrSql, cn)
                        Da.Fill(dtStone)
                        objStone.dtGridStone.Clear()
                        For Each RoStn As DataRow In dtStone.Rows
                            Dim RoStnIm As DataRow = objStone.dtGridStone.NewRow
                            For Each Col As DataColumn In dtStone.Columns
                                RoStnIm.Item(Col.ColumnName) = RoStn.Item(Col.ColumnName)
                            Next
                            objStone.dtGridStone.Rows.Add(RoStnIm)
                        Next
                        objStone.dtGridStone.AcceptChanges()
                        objStone.CalcStoneWtAmount()
                        RESNO = StkRow.Item("SNO").ToString
                        StkPcs = Val(StkRow.Item("PCS").ToString)
                        StkGrsWt = Val(StkRow.Item("GRSWT").ToString)
                        If rbtOrnament.Checked Then
                            Dim Assignval As Boolean = True
                            txtOOrdNo.Text = TranNo
                            cmbOCategory.Text = StkRow.Item("CATEGORY").ToString
                            cmbOMetal.Text = StkRow.Item("METAL").ToString
                            cmbOMetal.Enabled = False
                            If Assignval Then
                                txtOPcs_NUM.Text = Val(StkRow.Item("PCS").ToString)
                                txtOGrsWt_WET.Text = Val(StkRow.Item("GRSWT").ToString)
                                txtONetWt_WET.Text = Val(StkRow.Item("NETWT").ToString)
                            End If
                            'cmbOCategory.Enabled = False
                        ElseIf rbtMetal.Checked Then
                            Dim Assignval As Boolean = True
                            txtMOrdNo.Text = TranNo
                            cmbMCategory.Text = StkRow.Item("CATEGORY").ToString
                            cmbMMetal.Text = StkRow.Item("METAL").ToString
                            cmbMMetal.Enabled = False
                            If Assignval Then
                                txtMPcs_NUM.Text = Val(StkRow.Item("PCS").ToString)
                                txtMGrsWt_WET.Text = Val(StkRow.Item("GRSWT").ToString)
                                txtMNetWt_WET.Text = Val(StkRow.Item("NETWT").ToString)
                            End If
                            'cmbMCategory.Enabled = False
                        ElseIf rbtStone.Checked Then
                            Dim Assignval As Boolean = True
                            txtSOrdNo.Text = TranNo
                            LoadMetal(cmbSMetal)
                            cmbSMetal.Text = StkRow.Item("METAL").ToString
                            cmbSCategory.Text = StkRow.Item("CATEGORY").ToString
                            cmbSItem.Text = StkRow.Item("ITEM").ToString
                            cmbSSubItem.Text = StkRow.Item("SUBITEM").ToString
                            cmbSUnit.Text = IIf(StkRow.Item("UNIT").ToString = "G", "GRAM", "CARAT")
                            StkUnit = StkRow.Item("UNIT").ToString
                            If Assignval Then
                                txtSPcs_NUM.Text = Val(StkRow.Item("PCS").ToString)
                                txtSGrsWt_WET.Text = Val(StkRow.Item("GRSWT").ToString)
                            End If
                            'cmbSMetal.Enabled = False
                            'cmbSCategory.Enabled = False
                        End If
                        Return True
                    Else
                        MsgBox("Record Not Found.", MsgBoxStyle.Information)
                        If rbtOrnament.Checked Then
                            txtOPcs_NUM.Clear()
                            txtOGrsWt_WET.Clear()
                            txtONetWt_WET.Clear()
                            txtOTouchAMT.Clear()
                        ElseIf rbtMetal.Checked Then
                            txtMPcs_NUM.Clear()
                            txtMGrsWt_WET.Clear()
                            txtMNetWt_WET.Clear()
                            txtMTouchAMT.Clear()
                        ElseIf rbtStone.Checked Then
                            txtSPcs_NUM.Clear()
                            txtSGrsWt_WET.Clear()
                        End If
                        Return False
                    End If
                End If
            Else
                MsgBox("Record Not Found.", MsgBoxStyle.Information)
                If rbtOrnament.Checked Then
                    txtOPcs_NUM.Clear()
                    txtOGrsWt_WET.Clear()
                    txtONetWt_WET.Clear()
                    txtOTouchAMT.Clear()
                ElseIf rbtMetal.Checked Then
                    txtMPcs_NUM.Clear()
                    txtMGrsWt_WET.Clear()
                    txtMNetWt_WET.Clear()
                    txtMTouchAMT.Clear()
                ElseIf rbtStone.Checked Then
                    txtSPcs_NUM.Clear()
                    txtSGrsWt_WET.Clear()
                End If
                Return False
            End If
        Else
            Return True
        End If
    End Function


    Private Sub loadJobDetailsNew()
        If txtSOrdNo.Text <> "" Or txtOOrdNo.Text <> "" Or txtMOrdNo.Text <> "" Then
            Dim JObno As String = ""
            Dim Metal As String = ""
            If rbtOrnament.Checked Then
                JObno = txtOOrdNo.Text
                Metal = cmbOMetal.Text
            ElseIf rbtMetal.Checked Then
                JObno = txtMOrdNo.Text
                Metal = cmbMMetal.Text
            ElseIf rbtStone.Checked Then
                JObno = txtSOrdNo.Text
                Metal = cmbSMetal.Text
            End If
            Dim mBatchnos As String
            Dim mType As String = ""
            Dim mStrSql As String = ""
            StrSql = " SELECT * FROM " & cnStockDb & "..JOBNODETAILS "
            StrSql += vbCrLf + " WHERE SUBSTRING(JOBNO,6,LEN(JOBNO))='" & JObno & "' "
            StrSql += vbCrLf + " ORDER BY SNO DESC"
            Dim dtJobNo As New DataTable
            Cmd = New OleDbCommand(StrSql, cn)
            Da = New OleDbDataAdapter(Cmd)
            Da.Fill(dtJobNo)
            If dtJobNo.Rows.Count > 0 Then
                mType = dtJobNo.Rows(0).Item("TYPE").ToString
                For i As Integer = 1 To dtJobNo.Rows.Count - 1
                    mBatchnos = mBatchnos & "'" & dtJobNo.Rows(i).Item("BATCHNO").ToString & "',"
                Next
                If mBatchnos <> "" Then mBatchnos = Mid(mBatchnos, 1, Len(mBatchnos) - 1)
            Else
                If rbtOrnament.Checked Then
                    txtOPcs_NUM.Clear()
                    txtOGrsWt_WET.Clear()
                    txtONetWt_WET.Clear()
                    txtOTouchAMT.Clear()
                ElseIf rbtMetal.Checked Then
                    txtMPcs_NUM.Clear()
                    txtMGrsWt_WET.Clear()
                    txtMNetWt_WET.Clear()
                    txtMTouchAMT.Clear()
                ElseIf rbtStone.Checked Then
                    txtSPcs_NUM.Clear()
                    txtSGrsWt_WET.Clear()
                End If
                Exit Sub
            End If
            Dim EntCnt As Integer = 0
            If mBatchnos <> "" Then
                Dim Str() As String = mBatchnos.Split(",")
                If Not Str Is Nothing Then
                    EntCnt = Str.Length
                End If
            End If
            If mType = "I" Then
                mStrSql = vbCrLf + " SELECT 'ISSUE' DESCRIP,M.METALNAME AS METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                mStrSql += vbCrLf + " ,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,SUM(ISNULL(PCS,0)) PCS,SUM(ISNULL(I.GRSWT,0)) GRSWT "
                mStrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) NETWT,ISNULL(TOUCH,0)TOUCH  FROM " & cnStockDb & "..ISSUE  I "
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = I.ITEMID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = I.SUBITEMID"
                mStrSql += vbCrLf + " WHERE I.TRANTYPE IN('IIS','IPU','IRC','ISS') "
                mStrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                mStrSql += vbCrLf + " AND SUBSTRING(JOBNO,6,LEN(JOBNO))='" & JObno & "' "
                If Metal <> "" Then mStrSql += vbCrLf + " AND M.METALNAME='" & Metal & "'"
                If mBatchnos <> "" Then
                    mStrSql += vbCrLf + " AND BATCHNO NOT IN(" & mBatchnos & ")"
                End If
                mStrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
                mStrSql += vbCrLf + " GROUP BY M.METALNAME,JOBNO,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME,TOUCH  "
            Else
                mStrSql = vbCrLf + " SELECT 'RECEIPT' DESCRIP,M.METALNAME AS METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                mStrSql += vbCrLf + " ,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,SUM(ISNULL(PCS,0)) PCS,SUM(ISNULL(R.GRSWT,0)) GRSWT "
                mStrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) NETWT,ISNULL(TOUCH,0)TOUCH  FROM " & cnStockDb & "..RECEIPT R "
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON R.METALID = M.METALID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = R.ITEMID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = R.SUBITEMID"
                mStrSql += vbCrLf + " WHERE R.TRANTYPE IN('RRE','RPU') "
                'mStrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                mStrSql += vbCrLf + " AND SUBSTRING(JOBNO,6,LEN(JOBNO))='" & JObno & "' "
                mStrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
                If Metal <> "" Then mStrSql += vbCrLf + " AND M.METALNAME='" & Metal & "'"
                If mBatchnos <> "" Then
                    mStrSql += vbCrLf + " AND BATCHNO NOT IN(" & mBatchnos & ")"
                End If
                mStrSql += vbCrLf + " GROUP BY M.METALNAME,JOBNO,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME,TOUCH  "
            End If
            Dim dtJob As New DataTable
            Cmd = New OleDbCommand(mStrSql, cn)
            Da = New OleDbDataAdapter(Cmd)
            Da.Fill(dtJob)
showjobs:
            Dim JnoRow As DataRow
            If dtJob.Rows.Count > 0 Then
                JnoRow = BrighttechPack.SearchDialog.Show_R("Find ", mStrSql, cn, , 1, , , , , , False)
            End If
            If Not JnoRow Is Nothing Then
                If mType = "R" Then
                    StrSql = vbCrLf + " SELECT ITEM,SUBITEM"
                    StrSql += vbCrLf + " ,SUM(PCS)PCS,SUM(WEIGHT) WEIGHT"
                    StrSql += vbCrLf + " ,UNIT,CALC,RATE,SUM(AMOUNT)AMOUNT"
                    StrSql += vbCrLf + " ,METALID,SEIVE,STUDDEDUCT,RESNO"
                    StrSql += vbCrLf + " FROM"
                    StrSql += vbCrLf + " ("
                    StrSql += vbCrLf + "  SELECT "
                    StrSql += vbCrLf + "  IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
                    StrSql += vbCrLf + "  ,ST.STNPCS AS PCS,(CASE WHEN ST.STONEUNIT='G' THEN ST.STNWT*5 ELSE ST.STNWT END ) AS WEIGHT"
                    StrSql += vbCrLf + "  ,'C' AS UNIT,ST.CALCMODE AS CALC,ST.STNRATE AS RATE,ST.STNAMT AS AMOUNT"
                    StrSql += vbCrLf + "  ,IM.DIASTONE AS METALID,ISNULL(SEIVE,'') AS SEIVE"
                    StrSql += vbCrLf + "  ,CASE WHEN ISNULL(ST.STUDDEDUCT,'Y')='Y' THEN 'YES' ELSE 'NO' END AS STUDDEDUCT"
                    StrSql += vbCrLf + "  ,SNO AS RESNO"
                    StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPTSTONE AS ST"
                    StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
                    StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = ST.STNITEMID AND SM.SUBITEMID = ST.STNSUBITEMID"
                    StrSql += vbCrLf + "  WHERE 1=1"
                    If mBatchnos <> "" Then
                        StrSql += vbCrLf + " AND BATCHNO NOT IN(" & mBatchnos & ")"
                    End If
                    StrSql += vbCrLf + " AND ISSSNO IN(SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE "
                    StrSql += vbCrLf + " SUBSTRING(JOBNO,6,LEN(JOBNO))='" & JObno & "') "
                    If MCostId <> "" Then StrSql += vbCrLf + "  AND ST.COSTID='" & MCostId & "'"
                    StrSql += vbCrLf + " )X GROUP BY RESNO,ITEM,SUBITEM,UNIT,CALC,RATE,METALID,SEIVE,STUDDEDUCT  HAVING SUM(WEIGHT)<>0 "
                Else
                    StrSql = vbCrLf + " SELECT ITEM,SUBITEM"
                    StrSql += vbCrLf + " ,SUM(PCS)PCS,SUM(WEIGHT) WEIGHT"
                    StrSql += vbCrLf + " ,UNIT,CALC,RATE,SUM(AMOUNT)AMOUNT"
                    StrSql += vbCrLf + " ,METALID,SEIVE,STUDDEDUCT,RESNO,STNGRPID"
                    StrSql += vbCrLf + " FROM"
                    StrSql += vbCrLf + " ("
                    StrSql += vbCrLf + "  SELECT "
                    StrSql += vbCrLf + "  IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
                    StrSql += vbCrLf + "  ,ST.STNPCS AS PCS,(CASE WHEN ST.STONEUNIT='G' THEN ST.STNWT*5 ELSE ST.STNWT END) AS WEIGHT"
                    StrSql += vbCrLf + "  ,'C' AS UNIT,ST.CALCMODE AS CALC,ST.STNRATE AS RATE,ST.STNAMT AS AMOUNT"
                    StrSql += vbCrLf + "  ,IM.DIASTONE AS METALID,ISNULL(SEIVE,'') AS SEIVE"
                    StrSql += vbCrLf + "  ,CASE WHEN ISNULL(ST.STUDDEDUCT,'Y')='Y' THEN 'YES' ELSE 'NO' END AS STUDDEDUCT"
                    StrSql += vbCrLf + "  ,RESNO AS RESNO,STNGRPID"
                    StrSql += vbCrLf + "  FROM " & cnStockDb & "..ISSSTONE AS ST"
                    StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
                    StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = ST.STNITEMID AND SM.SUBITEMID = ST.STNSUBITEMID"
                    StrSql += vbCrLf + "  WHERE 1=1"
                    If mBatchnos <> "" Then
                        StrSql += vbCrLf + " AND BATCHNO NOT IN(" & mBatchnos & ")"
                    End If
                    StrSql += vbCrLf + " AND ISSSNO IN(SELECT SNO FROM " & cnStockDb & "..ISSUE WHERE "
                    StrSql += vbCrLf + " SUBSTRING(JOBNO,6,LEN(JOBNO))='" & JObno & "') "
                    If MCostId <> "" Then StrSql += vbCrLf + "  AND ST.COSTID='" & MCostId & "'"
                    StrSql += vbCrLf + " )X GROUP BY RESNO,ITEM,SUBITEM,UNIT,CALC,RATE,METALID,SEIVE,STUDDEDUCT,STNGRPID  HAVING SUM(WEIGHT)<>0 "
                End If
                Dim dtStone As New DataTable
                Da = New OleDbDataAdapter(StrSql, cn)
                Da.Fill(dtStone)
                objStone.dtGridStone.Clear()
                For Each RoStn As DataRow In dtStone.Rows
                    Dim RoStnIm As DataRow = objStone.dtGridStone.NewRow
                    For Each Col As DataColumn In dtStone.Columns
                        RoStnIm.Item(Col.ColumnName) = RoStn.Item(Col.ColumnName)
                    Next
                    objStone.dtGridStone.Rows.Add(RoStnIm)
                Next
                objStone.FromFlag = "A"
                objStone.dtGridStone.AcceptChanges()
                objStone.CalcStoneWtAmount()

                If rbtOrnament.Checked Then
                    Dim Assignval As Boolean = True
                    If txtOOrdNo.Focused Then cmbOProcess.Focus()
                    txtOOrdNo.Text = JObno
                    If Assignval Then
                        txtOPcs_NUM.Text = Val(dtJob.Compute("SUM(PCS)", Nothing).ToString) 'Val(JnoRow.Item("PCS").ToString)
                        txtOGrsWt_WET.Text = Val(dtJob.Compute("SUM(GRSWT)", Nothing).ToString) 'Val(JnoRow.Item("GRSWT").ToString)
                        txtONetWt_WET.Text = Val(dtJob.Compute("SUM(NETWT)", Nothing).ToString) 'Val(JnoRow.Item("NETWT").ToString)
                        txtOLessWt_WET.Text = Val(objStone.gridStoneTotal.Rows(0).Cells("WEIGHT").Value.ToString)
                    End If
                    txtOTouchAMT.Text = Val(JnoRow.Item("TOUCH").ToString)
                    cmbOCategory.Text = JnoRow.Item("CATEGORY").ToString
                    If mType = "R" Or EntCnt >= 2 Then
                        cmbOCategory.Enabled = False
                        cmbOAcPostCategory.Enabled = False
                        cmbOIssuedCategory.Enabled = False
                        If mType = "R" Then
                            txtOPcs_NUM.Enabled = False
                            txtOGrsWt_WET.Enabled = False
                            txtONetWt_WET.Enabled = False
                            txtOPureWt_WET.Enabled = False
                            txtOLessWt_WET.Enabled = False
                            txtOTouchAMT.Enabled = False
                            txtOAmount_AMT.Focus()
                        End If
                    End If
                    cmbOItem.Text = JnoRow.Item("ITEM").ToString
                    cmbOSubItem.Text = JnoRow.Item("SUBITEM").ToString
                    If JnoRow.Item("ITEM").ToString <> "" Then
                        cmbOItem.Enabled = False
                    End If
                    If JnoRow.Item("ITEM").ToString <> "" Then
                        cmbOSubItem.Enabled = False
                    End If
                ElseIf rbtMetal.Checked Then
                    Dim Assignval As Boolean = True
                    If txtMOrdNo.Focused Then cmbMProcess.Focus()
                    txtMOrdNo.Text = JObno
                    If Assignval Then
                        txtMPcs_NUM.Text = Val(dtJob.Compute("SUM(PCS)", Nothing).ToString)  'Val(JnoRow.Item("PCS").ToString)
                        txtMGrsWt_WET.Text = Val(dtJob.Compute("SUM(GRSWT)", Nothing).ToString) 'Val(JnoRow.Item("GRSWT").ToString)
                        txtMNetWt_WET.Text = Val(dtJob.Compute("SUM(NETWT)", Nothing).ToString) 'Val(JnoRow.Item("NETWT").ToString)
                        txtMLessWt_WET.Text = Val(objStone.gridStoneTotal.Rows(0).Cells("WEIGHT").Value.ToString)
                    End If
                    txtMTouchAMT.Text = Val(JnoRow.Item("TOUCH").ToString)
                    cmbMCategory.Text = JnoRow.Item("CATEGORY").ToString
                    If mType = "R" Then
                        txtMAmount_AMT.Focus()
                    End If
                ElseIf rbtStone.Checked Then
                    Dim Assignval As Boolean = True
                    txtSOrdNo.Text = JObno
                    If Assignval Then
                        txtSPcs_NUM.Text = Val(dtJob.Compute("SUM(PCS)", Nothing).ToString) 'Val(JnoRow.Item("PCS").ToString)
                        txtSGrsWt_WET.Text = Val(dtJob.Compute("SUM(GRSWT)", Nothing).ToString) 'Val(JnoRow.Item("GRSWT").ToString)
                    End If
                    cmbSCategory.Text = JnoRow.Item("CATEGORY").ToString
                End If
            Else
                If rbtOrnament.Checked Then
                    txtOPcs_NUM.Clear()
                    txtOGrsWt_WET.Clear()
                    txtONetWt_WET.Clear()
                    txtOTouchAMT.Clear()
                ElseIf rbtMetal.Checked Then
                    txtMPcs_NUM.Clear()
                    txtMGrsWt_WET.Clear()
                    txtMNetWt_WET.Clear()
                    txtMTouchAMT.Clear()
                ElseIf rbtStone.Checked Then
                    txtSPcs_NUM.Clear()
                    txtSGrsWt_WET.Clear()
                End If
            End If
        End If
    End Sub
    Private Sub loadJobDetails()
        If MRMI_JOBNOLINK Then
            loadJobDetailsNew()
            Exit Sub
        End If
        If txtSOrdNo.Text <> "" Or txtOOrdNo.Text <> "" Or txtMOrdNo.Text <> "" Then
            Dim JObno As String = ""
            Dim Metal As String = ""
            If rbtOrnament.Checked Then
                JObno = txtOOrdNo.Text
                Metal = cmbOMetal.Text
            ElseIf rbtMetal.Checked Then
                JObno = txtMOrdNo.Text
                Metal = cmbMMetal.Text
            ElseIf rbtStone.Checked Then
                JObno = txtSOrdNo.Text
                Metal = cmbSMetal.Text
            End If
            Dim mStrSql As String = ""
            If oMaterial = Material.Issue Then
                mStrSql = vbCrLf + " SELECT DESCRIP,METAL,CATEGORY,ITEM,SUBITEM,JOBNO,SUM(PCS)PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT,TOUCH FROM"
                mStrSql += vbCrLf + " ("
                mStrSql += vbCrLf + " SELECT 'RECEIPT' DESCRIP,M.METALNAME METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                mStrSql += vbCrLf + " ,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,SUM(ISNULL(PCS,0)) PCS,SUM(ISNULL(R.GRSWT,0)) GRSWT "
                mStrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) NETWT,ISNULL(TOUCH,0)TOUCH  FROM " & cnStockDb & "..RECEIPT R "
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON R.METALID = M.METALID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = R.ITEMID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = R.SUBITEMID"
                mStrSql += vbCrLf + " WHERE R.TRANTYPE IN('RRE','RPU') "
                mStrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                mStrSql += vbCrLf + " AND SUBSTRING(JOBNO,6,LEN(JOBNO))='" & JObno & "' "
                If Metal <> "" Then mStrSql += vbCrLf + " AND M.METALNAME='" & Metal & "'"
                mStrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
                mStrSql += vbCrLf + " GROUP BY M.METALNAME,JOBNO,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME,TOUCH  "
                mStrSql += vbCrLf + " UNION ALL"
                mStrSql += vbCrLf + " SELECT 'ISSUE' DESCRIP,M.METALNAME AS METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                mStrSql += vbCrLf + " ,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,SUM(ISNULL(PCS,0))*(-1) PCS,SUM(ISNULL(I.GRSWT,0))*(-1) GRSWT "
                mStrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0))*(-1) NETWT,ISNULL(TOUCH,0)TOUCH  FROM " & cnStockDb & "..ISSUE  I "
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = I.ITEMID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = I.SUBITEMID"
                mStrSql += vbCrLf + " WHERE I.TRANTYPE IN('IIS','IPU','IRC','ISS') "
                mStrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                mStrSql += vbCrLf + " AND SUBSTRING(JOBNO,6,LEN(JOBNO))='" & JObno & "' "
                If Metal <> "" Then mStrSql += vbCrLf + " AND M.METALNAME='" & Metal & "'"
                mStrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
                mStrSql += vbCrLf + " GROUP BY M.METALNAME,JOBNO,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME,TOUCH  "
                mStrSql += vbCrLf + " )X GROUP BY DESCRIP,METAL,JOBNO,ITEM,CATEGORY,SUBITEM,TOUCH HAVING SUM(GRSWT)<>0 "
            ElseIf oMaterial = Material.Receipt Then
                mStrSql = vbCrLf + " SELECT DESCRIP, METAL,CATEGORY,ITEM,SUBITEM,JOBNO,SUM(PCS)PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT,TOUCH FROM"
                mStrSql += vbCrLf + " ("
                mStrSql += vbCrLf + " SELECT 'ISSUE' DESCRIP,M.METALNAME AS METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                mStrSql += vbCrLf + " ,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,SUM(ISNULL(PCS,0)) PCS,SUM(ISNULL(I.GRSWT,0)) GRSWT "
                mStrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) NETWT,ISNULL(TOUCH,0)TOUCH  FROM " & cnStockDb & "..ISSUE I"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = I.ITEMID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = I.SUBITEMID"
                mStrSql += vbCrLf + " WHERE I.TRANTYPE IN('IIS','IPU','IRC','ISS') "
                mStrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                mStrSql += vbCrLf + " AND SUBSTRING(JOBNO,6,LEN(JOBNO))='" & JObno & "' "
                mStrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
                If Metal <> "" Then mStrSql += vbCrLf + " AND M.METALNAME='" & Metal & "'"
                mStrSql += vbCrLf + " GROUP BY M.METALNAME,JOBNO,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME,TOUCH  "
                mStrSql += vbCrLf + " UNION ALL"
                mStrSql += vbCrLf + " SELECT 'RECEIPT' DESCRIP,M.METALNAME AS METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
                mStrSql += vbCrLf + " ,SUBSTRING(JOBNO,6,LEN(JOBNO)) JOBNO,SUM(ISNULL(PCS,0))*(-1) PCS,SUM(ISNULL(R.GRSWT,0))*(-1) GRSWT "
                mStrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0))*(-1) NETWT,ISNULL(TOUCH,0)TOUCH  FROM " & cnStockDb & "..RECEIPT R "
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON R.METALID = M.METALID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = R.CATCODE"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = R.ITEMID"
                mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = R.SUBITEMID"
                mStrSql += vbCrLf + " WHERE R.TRANTYPE IN('RRE','RPU') "
                mStrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
                mStrSql += vbCrLf + " AND SUBSTRING(JOBNO,6,LEN(JOBNO))='" & JObno & "' "
                mStrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
                If Metal <> "" Then mStrSql += vbCrLf + " AND M.METALNAME='" & Metal & "'"
                mStrSql += vbCrLf + " GROUP BY M.METALNAME,JOBNO,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME,TOUCH  "
                mStrSql += vbCrLf + " )X GROUP BY DESCRIP,METAL,JOBNO,ITEM,CATEGORY,SUBITEM,TOUCH HAVING SUM(GRSWT)<>0 "
            End If
            Dim dtJob As New DataTable
            Cmd = New OleDbCommand(mStrSql, cn)
            Da = New OleDbDataAdapter(Cmd)
            Da.Fill(dtJob)
showjobs:
            Dim JnoRow As DataRow
            If dtJob.Rows.Count > 0 Then
                JnoRow = BrighttechPack.SearchDialog.Show_R("Find ", mStrSql, cn, , 1, , , , , , False)
            End If
            If Not JnoRow Is Nothing Then
                If rbtOrnament.Checked Then
                    Dim Assignval As Boolean = True
                    If oMaterial = Material.Receipt And JnoRow.Item(0) = "RECEIPT" Then
                        If MsgBox("Further Receipt add to this Job No", MsgBoxStyle.YesNo) = MsgBoxResult.No Then GoTo showjobs Else Assignval = False
                    End If
                    If oMaterial = Material.Issue And JnoRow.Item(0) = "ISSUE" Then
                        If MsgBox("Further Issue add to this Job No", MsgBoxStyle.YesNo) = MsgBoxResult.No Then GoTo showjobs Else Assignval = False
                    End If
                    If txtOOrdNo.Focused Then cmbOProcess.Focus()
                    txtOOrdNo.Text = JObno
                    If Assignval Then
                        txtOPcs_NUM.Text = Val(dtJob.Compute("SUM(PCS)", Nothing).ToString) 'Val(JnoRow.Item("PCS").ToString)
                        txtOGrsWt_WET.Text = Val(dtJob.Compute("SUM(GRSWT)", Nothing).ToString) 'Val(JnoRow.Item("GRSWT").ToString)
                        txtONetWt_WET.Text = Val(dtJob.Compute("SUM(NETWT)", Nothing).ToString) 'Val(JnoRow.Item("NETWT").ToString)
                    End If
                    txtOTouchAMT.Text = Val(JnoRow.Item("TOUCH").ToString)

                    cmbOCategory.Text = JnoRow.Item("CATEGORY").ToString
                    cmbOItem.Text = JnoRow.Item("ITEM").ToString
                    cmbOSubItem.Text = JnoRow.Item("SUBITEM").ToString
                ElseIf rbtMetal.Checked Then
                    Dim Assignval As Boolean = True
                    If oMaterial = Material.Receipt And JnoRow.Item(0) = "RECEIPT" Then
                        If MsgBox("Further Receipt add to this Job No", MsgBoxStyle.YesNo) = MsgBoxResult.No Then GoTo showjobs Else Assignval = False
                    End If
                    If oMaterial = Material.Issue And JnoRow.Item(0) = "ISSUE" Then
                        If MsgBox("Further Issue add to this Job No", MsgBoxStyle.YesNo) = MsgBoxResult.No Then GoTo showjobs Else Assignval = False
                    End If
                    If txtMOrdNo.Focused Then cmbMProcess.Focus()
                    txtMOrdNo.Text = JObno
                    If Assignval Then
                        txtMPcs_NUM.Text = Val(dtJob.Compute("SUM(PCS)", Nothing).ToString)  'Val(JnoRow.Item("PCS").ToString)
                        txtMGrsWt_WET.Text = Val(dtJob.Compute("SUM(GRSWT)", Nothing).ToString) 'Val(JnoRow.Item("GRSWT").ToString)
                        txtMNetWt_WET.Text = Val(dtJob.Compute("SUM(NETWT)", Nothing).ToString) 'Val(JnoRow.Item("NETWT").ToString)
                    End If
                    txtMTouchAMT.Text = Val(JnoRow.Item("TOUCH").ToString)
                    cmbMCategory.Text = JnoRow.Item("CATEGORY").ToString
                ElseIf rbtStone.Checked Then
                    Dim Assignval As Boolean = True
                    If oMaterial = Material.Receipt And JnoRow.Item(0) = "RECEIPT" Then
                        If MsgBox("Further Receipt add to this Job No", MsgBoxStyle.YesNo) = MsgBoxResult.No Then GoTo showjobs Else Assignval = False
                    End If
                    If oMaterial = Material.Issue And JnoRow.Item(0) = "ISSUE" Then
                        If MsgBox("Further Issue add to this Job No", MsgBoxStyle.YesNo) = MsgBoxResult.No Then GoTo showjobs Else Assignval = False
                    End If
                    txtSOrdNo.Text = JObno
                    If Assignval Then
                        txtSPcs_NUM.Text = Val(dtJob.Compute("SUM(PCS)", Nothing).ToString) 'Val(JnoRow.Item("PCS").ToString)
                        txtSGrsWt_WET.Text = Val(dtJob.Compute("SUM(GRSWT)", Nothing).ToString) 'Val(JnoRow.Item("GRSWT").ToString)
                    End If
                    'cmbSCategory.Text = JnoRow.Item("CATEGORY").ToString
                    LoadMetalDetails(cmbSMetal)
                    cmbSMetal.Text = JnoRow.Item("METAL").ToString
                    cmbSCategory.Text = JnoRow.Item("CATEGORY").ToString
                    cmbSItem.Text = JnoRow.Item("ITEM").ToString
                    cmbSSubItem.Text = JnoRow.Item("SUBITEM").ToString
                End If
                If oMaterial = Material.Issue Then
                    cmbSProcess.Text = "ISSUE TO SMITH"
                Else
                    cmbSProcess.Text = "RECEIVED FROM SMITH"
                End If
            Else
                If rbtOrnament.Checked Then
                    txtOPcs_NUM.Clear()
                    txtOGrsWt_WET.Clear()
                    txtONetWt_WET.Clear()
                    txtOTouchAMT.Clear()
                ElseIf rbtMetal.Checked Then
                    txtMPcs_NUM.Clear()
                    txtMGrsWt_WET.Clear()
                    txtMNetWt_WET.Clear()
                    txtMTouchAMT.Clear()
                ElseIf rbtStone.Checked Then
                    txtSPcs_NUM.Clear()
                    txtSGrsWt_WET.Clear()
                End If
            End If
        End If
    End Sub

    'Private Sub txtSOrdNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSOrdNo.KeyPress
    '    If e.KeyChar = Chr(Keys.Enter) Then
    '        Dim StkAvail As Boolean = True
    '        If _JobNoEnable = False Then
    '            If Mid(txtSOrdNo.Text, 1, 1).ToUpper = "R" Or Mid(txtSOrdNo.Text, 1, 1).ToUpper = "O" Then
    '                If CheckJobNo(CType(sender, Control)) = False Then
    '                    MsgBox("Invalid Order Info" & vbCrLf & "This order no is already delivered or cancelled", MsgBoxStyle.Information)
    '                    Exit Sub
    '                End If
    '                If CType(sender, Control).Text <> "" Then
    '                    If objOrderInfo Is Nothing Then
    '                        objOrderInfo = New MATERIALISSREC_ORDERINFO(CType(sender, Control).Text, BillCostId, IIf(oMaterial = Material.Issue, True, False), IIf(ORDER_MULTI_MIMR, Maccode, ""))
    '                        'objOrderInfo.BillCostId = BillCostId
    '                    End If
    '                    If objOrderInfo.ShowDialog() <> Windows.Forms.DialogResult.OK Then
    '                        Exit Sub
    '                    End If
    '                    Dim TempOrSno As String = ""
    '                    For cnt As Integer = 0 To objOrderInfo.DgvOrder.Rows.Count - 1
    '                        If CType(objOrderInfo.DgvOrder.Rows(cnt).Cells("CHECK").Value, Boolean) = True Then
    '                            TempOrSno += "'" & objOrderInfo.DgvOrder.Rows(cnt).Cells("SNO").Value.ToString & "',"
    '                        End If
    '                    Next
    '                    If TempOrSno.Contains(",") = True Then
    '                        TempOrSno = TempOrSno.Substring(0, TempOrSno.Length - 1)
    '                    End If
    '                    Dim DtOrder As New DataTable
    '                    StrSql = "SELECT M.METALNAME AS METAL,ISNULL(CA.CATNAME,C.CATNAME) CATEGORY,IT.ITEMNAME ITEM,SIT.SUBITEMNAME SUBITEM,"
    '                    StrSql += " O.PCS-ISNULL((SELECT SUM(isnull(PCS,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND  ORSTATUS = 'R' AND ISNULL(CANCEL,'')=''),0) PCS,O.GRSWT-ISNULL((SELECT SUM(isnull(GRSWT,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND ORSTATUS = 'R'  AND ISNULL(CANCEL,'')=''),0) GRSWT ,O.NETWT-ISNULL((SELECT SUM(isnull(NETWT,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND ORSTATUS = 'R'  AND ISNULL(CANCEL,'')=''),0) NETWT"
    '                    StrSql += " ,O.SNO "
    '                    StrSql += " FROM " & cnAdminDb & "..ORMAST O "
    '                    StrSql += " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON O.ITEMID=IT.ITEMID"
    '                    StrSql += " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SIT ON O.SUBITEMID = SIT.SUBITEMID"
    '                    StrSql += " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALID = IT.METALID "
    '                    StrSql += " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE= IT.CATCODE "
    '                    StrSql += " LEFT JOIN " & cnAdminDb & "..OUTSTANDING OU ON OU.BATCHNO= O.BATCHNO AND OU.RUNNO=O.ORNO "
    '                    StrSql += " LEFT JOIN " & cnAdminDb & "..CATEGORY CA ON CA.CATCODE= OU.CATCODE "
    '                    StrSql += " WHERE O.SNO IN (" & TempOrSno & ")"
    '                    Da = New OleDbDataAdapter(StrSql, cn)
    '                    Da.Fill(DtOrder)
    '                    DtOrder.AcceptChanges()
    '                    If DtOrder.Rows.Count > 0 Then
    '                        cmbOCategory.Text = DtOrder.Rows(0).Item("CATEGORY").ToString : cmbOCategory.Enabled = False
    '                        cmbOMetal.Text = DtOrder.Rows(0).Item("METAL").ToString : cmbOMetal.Enabled = False
    '                        cmbOItem.Text = DtOrder.Rows(0).Item("ITEM").ToString : cmbOItem.Enabled = False
    '                        cmbOSubItem.Text = DtOrder.Rows(0).Item("SUBITEM").ToString : cmbOSubItem.Enabled = False
    '                        txtOPcs_NUM.Text = DtOrder.Rows(0).Item("PCS").ToString
    '                        txtOGrsWt_WET.Text = DtOrder.Rows(0).Item("GRSWT").ToString
    '                        txtONetWt_WET.Text = DtOrder.Rows(0).Item("NETWT").ToString
    '                        ORSNO = DtOrder.Rows(0).Item("SNO").ToString
    '                        OGrswt = Val(DtOrder.Rows(0).Item("GRSWT").ToString)
    '                        ONetwt = Val(DtOrder.Rows(0).Item("NETWT").ToString)
    '                    End If
    '                    If rbtStone.Checked Then
    '                        StrSql = vbCrLf + " SELECT "
    '                        StrSql += vbCrLf + "     (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID =Y.STNITEMID) ITEM"
    '                        StrSql += vbCrLf + "    ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID =Y.STNSUBITEMID) SUBITEM"
    '                        StrSql += vbCrLf + "    ,CONVERT(VARCHAR,SUM(STNPCS)) PCS,CONVERT(VARCHAR,SUM(STNWT)) WEIGHT"
    '                        StrSql += vbCrLf + "    ,CONVERT(VARCHAR,CONVERT(NUMERIC(15,4),SUM(STNWT)/CASE WHEN SUM(STNPCS) > 0 THEN SUM(STNPCS) ELSE 1 END)) SIZE"
    '                        StrSql += vbCrLf + "    ,STNUNIT UNIT,CALCMODE CALC,STNRATE RATE,STNAMT AMOUNT"
    '                        StrSql += vbCrLf + " FROM"
    '                        StrSql += vbCrLf + " ("
    '                        StrSql += vbCrLf + " 	SELECT X.*,SZ.SIZEDESC FROM "
    '                        StrSql += vbCrLf + " 	("
    '                        StrSql += vbCrLf + " 	SELECT STNITEMID,STNSUBITEMID,STNPCS,STNWT,CONVERT(NUMERIC(15,4),CASE WHEN STNPCS > 0 THEN (STNWT/STNPCS)*100 ELSE STNWT*100 END) AS WTRANGE"
    '                        StrSql += vbCrLf + " 	,CONVERT(NUMERIC(15,3),CASE WHEN STNPCS > 0 THEN (STNWT/STNPCS) ELSE STNWT END) AS DIASIZE"
    '                        StrSql += vbCrLf + " 	,CALCMODE,STNUNIT,STNRATE,STNAMT FROM " & cnAdminDb & "..ORSTONE"
    '                        StrSql += vbCrLf + " 	WHERE ORSNO IN (SELECT SNO FROM " & cnAdminDb & "..ORMAST WHERE ORNO= '" & GetCostId(BillCostId) & GetCompanyId(strCompanyId) & txtSOrdNo.Text & "')"
    '                        StrSql += vbCrLf + "    AND ORSNO IN (" & TempOrSno & ")"
    '                        StrSql += vbCrLf + " 	)X"
    '                        StrSql += vbCrLf + " 	LEFT OUTER JOIN " & cnAdminDb & "..CENTSIZE AS SZ ON SZ.ITEMID = STNITEMID AND SZ.SUBITEMID = STNSUBITEMID AND WTRANGE BETWEEN FROMCENT AND TOCENT"
    '                        StrSql += vbCrLf + " )Y GROUP BY SIZEDESC,STNITEMID,STNSUBITEMID,STNUNIT,CALCMODE,STNPCS,STNWT,STNRATE,STNAMT"
    '                        MaterialStoneDia.dtGridStone.Rows.Clear()
    '                        Da = New OleDbDataAdapter(StrSql, cn)
    '                        Da.Fill(MaterialStoneDia.dtGridStone)
    '                        MaterialStoneDia.dtGridStone.AcceptChanges()
    '                        MaterialStoneDia.StartPosition = FormStartPosition.CenterScreen
    '                        MaterialStoneDia.ShowDialog()

    '                        If MaterialStoneDia.gridStoneTotal.Rows.Count > 0 Then
    '                            txtSPcs_NUM.Text = MaterialStoneDia.gridStoneTotal.Rows(0).Cells("PCS").Value.ToString
    '                            txtSGrsWt_WET.Text = Format(Convert.ToDouble(MaterialStoneDia.gridStoneTotal.Rows(0).Cells("WEIGHT").Value.ToString), "0.000")
    '                            txtSGrsAmt_AMT.Text = Format(Convert.ToDouble(MaterialStoneDia.gridStoneTotal.Rows(0).Cells("AMOUNT").Value.ToString), "0.00")
    '                        End If
    '                        cmbSMetal.Enabled = False
    '                        cmbSCategory.Enabled = False
    '                        cmbSIssuedCategory.Enabled = False
    '                        CmbSPurity.Enabled = False
    '                        cmbSItem.Enabled = False
    '                        cmbSSubItem.Enabled = False
    '                        txtSPcs_NUM.Enabled = False
    '                        txtSGrsWt_WET.Enabled = False
    '                        cmbSUnit.Enabled = False
    '                        cmbSCalcMode.Enabled = False
    '                        txtSRate_OWN.Enabled = False
    '                        txtSGrsAmt_AMT.Enabled = False

    '                        txtSAddlCharge_AMT.Enabled = False
    '                        txtSVatPer_PER.Enabled = False
    '                        txtSVat_AMT.Enabled = False
    '                        txtSAmount_AMT.Enabled = False
    '                    End If
    '                Else
    '                    cmbSMetal.Enabled = True
    '                    cmbSCategory.Enabled = True
    '                    cmbSIssuedCategory.Enabled = True
    '                    CmbSPurity.Enabled = True
    '                    cmbSItem.Enabled = True
    '                    cmbSSubItem.Enabled = True
    '                    txtSPcs_NUM.Enabled = True
    '                    txtSGrsWt_WET.Enabled = True
    '                    cmbSUnit.Enabled = True
    '                    cmbSCalcMode.Enabled = True
    '                    txtSRate_OWN.Enabled = True
    '                    txtSGrsAmt_AMT.Enabled = True

    '                    txtSAddlCharge_AMT.Enabled = True
    '                    txtSVatPer_PER.Enabled = True
    '                    txtSVat_AMT.Enabled = True
    '                    txtSAmount_AMT.Enabled = True
    '                End If
    '            Else
    '                If STOCKVALIDATION Then
    '                    StkAvail = LoadBalanceStock()
    '                Else
    '                    If Val(txtSOrdNo.Text) <> 0 Then
    '                        If rbtStone.Checked Then
    '                            StrSql = vbCrLf + "  SELECT ITEM,SUBITEM,PCS,UNIT,CALC,WEIGHT,RATE,AMOUNT,METALID,SNO,CATNAME,METAL FROM("
    '                            StrSql += vbCrLf + " SELECT IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM"
    '                            StrSql += vbCrLf + " ,(PCS-ISNULL((SELECT SUM(PCS) FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO ),0)) AS PCS,I.STONEUNIT UNIT,"
    '                            StrSql += vbCrLf + " CASE WHEN ISNULL(I.WEIGHTUNIT,'')='' THEN 'W' ELSE I.WEIGHTUNIT END CALC"
    '                            StrSql += vbCrLf + " ,(PUREWT-ISNULL((SELECT SUM(PUREWT) FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO ),0)) WEIGHT"
    '                            StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) RATE,"
    '                            StrSql += vbCrLf + " CONVERT(NUMERIC(15,2),NULL) AMOUNT,I.METALID,I.SNO,C.CATNAME,M.METALNAME AS METAL"
    '                            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON I.ITEMID=IM.ITEMID "
    '                            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.ITEMID =I.ITEMID AND SI.SUBITEMID =I.SUBITEMID "
    '                            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE =I.CATCODE "
    '                            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALID =I.METALID "
    '                            StrSql += vbCrLf + " WHERE I.TRANTYPE ='IIS'  AND ISNULL(STUDDED,'') IN ('S','L') AND ISNULL(I.CANCEL,'')='' "
    '                            StrSql += vbCrLf + " AND I.SNO NOT IN (SELECT DISTINCT JOBISNO FROM " & cnStockDb & "..RECEIPTSTONE WHERE "
    '                            StrSql += vbCrLf + " ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(CANCEL,'')='') AND ISNULL(JOBISNO,'')<>'')"
    '                            StrSql += vbCrLf + " AND I.TRANNO =" & Val(txtSOrdNo.Text)
    '                            StrSql += vbCrLf + " ) X WHERE ISNULL(PCS,0)<>0 OR ISNULL(WEIGHT,0)<>0 "
    '                            Dim dtGrid As New DataTable
    '                            dtGrid.Columns.Add("CHECK", GetType(Boolean))
    '                            Da = New OleDbDataAdapter(StrSql, cn)
    '                            Da.Fill(dtGrid)
    '                            If Not dtGrid.Rows.Count > 0 Then
    '                                StrSql = " SELECT STARTDATE-1 FROM " & cnAdminDb & "..DBMASTER WHERE DBNAME = '" & cnStockDb & "'"
    '                                Dim menddate As Date = (objGPack.GetSqlValue(StrSql))
    '                                StrSql = " SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER WHERE '" & menddate & "' BETWEEN STARTDATE AND ENDDATE"
    '                                Dim prevdb As String = objGPack.GetSqlValue(StrSql)
    '                                StrSql = " SELECT 'EXISTS' FROM MASTER..SYSDATABASES WHERE NAME = '" & prevdb & "'"
    '                                If objGPack.GetSqlValue(StrSql, , "-1") = "-1" Then
    '                                    txtSOrdNo.Select()
    '                                    Exit Sub
    '                                End If
    '                                'StrSql = vbCrLf + "  SELECT IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM,PCS,I.STONEUNIT UNIT,"
    '                                'StrSql += vbCrLf + " CASE WHEN ISNULL(I.WEIGHTUNIT,'')='' THEN 'W' ELSE I.WEIGHTUNIT END CALC,"
    '                                'StrSql += vbCrLf + " PUREWT WEIGHT,CONVERT(NUMERIC(15,2),NULL) RATE,"
    '                                'StrSql += vbCrLf + " CONVERT(NUMERIC(15,2),NULL) AMOUNT,I.METALID,I.SNO,C.CATNAME"
    '                                'StrSql += vbCrLf + " FROM " & prevdb & "..ISSUE I INNER JOIN " & cnAdminDb & "..ITEMMAST IM ON I.ITEMID=IM.ITEMID "
    '                                'StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.ITEMID =I.ITEMID AND SI.SUBITEMID =I.SUBITEMID "
    '                                'StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON I.CATCODE=C.CATCODE"
    '                                'StrSql += vbCrLf + " WHERE I.TRANTYPE ='IIS'  AND ISNULL(STUDDED,'') IN ('S','L') AND ISNULL(I.CANCEL,'')='' "
    '                                'StrSql += vbCrLf + " AND I.SNO NOT IN (SELECT DISTINCT JOBISNO FROM " & prevdb & "..RECEIPTSTONE WHERE "
    '                                'StrSql += vbCrLf + " ISSSNO IN (SELECT SNO FROM " & prevdb & "..RECEIPT WHERE ISNULL(CANCEL,'')='') and isnull(jobisno,'')<>'')"
    '                                'StrSql += vbCrLf + " AND I.TRANNO =" & Val(txtSOrdNo.Text)

    '                                StrSql = vbCrLf + "  SELECT ITEM,SUBITEM,PCS,UNIT,CALC,WEIGHT,RATE,AMOUNT,METALID,SNO,CATNAME,METAL FROM("
    '                                StrSql += vbCrLf + " SELECT IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM"
    '                                StrSql += vbCrLf + " ,(PCS-ISNULL((SELECT SUM(PCS) FROM " & prevdb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO ),0)) AS PCS,I.STONEUNIT UNIT,"
    '                                StrSql += vbCrLf + " CASE WHEN ISNULL(I.WEIGHTUNIT,'')='' THEN 'W' ELSE I.WEIGHTUNIT END CALC"
    '                                StrSql += vbCrLf + " ,(PUREWT-ISNULL((SELECT SUM(PUREWT) FROM " & prevdb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO ),0)) WEIGHT"
    '                                StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) RATE,"
    '                                StrSql += vbCrLf + " CONVERT(NUMERIC(15,2),NULL) AMOUNT,I.METALID,I.SNO,C.CATNAME,M.METALNAME AS METAL"
    '                                StrSql += vbCrLf + " FROM " & prevdb & "..ISSUE I LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON I.ITEMID=IM.ITEMID "
    '                                StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.ITEMID =I.ITEMID AND SI.SUBITEMID =I.SUBITEMID "
    '                                StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON I.CATCODE =I.CATCODE "
    '                                StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALID =I.METALID "
    '                                StrSql += vbCrLf + " WHERE I.TRANTYPE ='IIS'  AND ISNULL(STUDDED,'') IN ('S','L') AND ISNULL(I.CANCEL,'')='' "
    '                                StrSql += vbCrLf + " AND I.SNO NOT IN (SELECT DISTINCT JOBISNO FROM " & prevdb & "..RECEIPTSTONE WHERE "
    '                                StrSql += vbCrLf + " ISSSNO IN (SELECT SNO FROM " & prevdb & "..RECEIPT WHERE ISNULL(CANCEL,'')='') AND ISNULL(JOBISNO,'')<>'')"
    '                                StrSql += vbCrLf + " AND I.TRANNO =" & Val(txtSOrdNo.Text)
    '                                StrSql += vbCrLf + " ) X WHERE ISNULL(PCS,0)<>0 OR ISNULL(WEIGHT,0)<>0 "
    '                                dtGrid = New DataTable
    '                                dtGrid.Columns.Add("CHECK", GetType(Boolean))
    '                                Da = New OleDbDataAdapter(StrSql, cn)
    '                                Da.Fill(dtGrid)
    '                                If Not dtGrid.Rows.Count > 0 Then
    '                                    txtSOrdNo.Select()
    '                                    Exit Sub
    '                                End If
    '                            End If
    '                            If dtGridStuddedStone.Rows.Count > 0 Then
    '                                With dtGridStuddedStone
    '                                    For J As Integer = 0 To .Rows.Count - 1
    '                                        Dim STNSNO As String = .Rows(J).Item("STNSNO").ToString
    '                                        For K As Integer = 0 To dtGrid.Rows.Count - 1
    '                                            If dtGrid.Rows(K).Item("SNO").ToString = STNSNO Then
    '                                                dtGrid.Rows(K).Item("PCS") = Val(dtGrid.Rows(K).Item("PCS").ToString) - Val(.Rows(J).Item("PCS").ToString)
    '                                                dtGrid.Rows(K).Item("WEIGHT") = Val(dtGrid.Rows(K).Item("WEIGHT").ToString) - Val(.Rows(J).Item("WEIGHT").ToString)
    '                                            End If
    '                                        Next
    '                                    Next
    '                                End With
    '                            End If
    '                            objMultiSelect = New MultiSelectRowDia(dtGrid, "SNO")
    '                            objMultiSelect.chkAppSales.Visible = False
    '                            If objMultiSelect.ShowDialog = Windows.Forms.DialogResult.OK Then
    '                                Me.Refresh()
    '                                Dim sno As String = ""
    '                                For Each RoS As DataRow In objMultiSelect.RowSelected
    '                                    cmbSCategory.Text = RoS.Item("CATNAME").ToString : cmbSCategory.Enabled = False
    '                                    cmbSMetal.Text = RoS.Item("METAL").ToString : cmbSMetal.Enabled = False
    '                                    cmbSItem.Text = RoS.Item("ITEM").ToString : cmbSItem.Enabled = False
    '                                    cmbSSubItem.Text = RoS.Item("SUBITEM").ToString : cmbSSubItem.Enabled = False
    '                                    txtSPcs_NUM.Text = RoS.Item("PCS").ToString
    '                                    txtSGrsWt_WET.Text = RoS.Item("WEIGHT").ToString
    '                                    cmbSUnit.Text = RoS.Item("UNIT").ToString
    '                                    cmbSCalcMode.Text = RoS.Item("CALC").ToString
    '                                    txtSRate_OWN.Text = Val(RoS.Item("RATE").ToString)
    '                                    txtSGrsAmt_AMT.Text = Val(RoS.Item("AMOUNT").ToString)
    '                                    StnSno = RoS.Item("SNO").ToString

    '                                    If oMaterial = Material.Receipt Then
    '                                        cmbSProcess.Text = objGPack.GetSqlValue("SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID=3", "", "", )
    '                                    Else
    '                                        cmbSProcess.Text = objGPack.GetSqlValue("SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID=2", "", "", )
    '                                    End If
    '                                    'txtSRemark2_KeyPress(Me, New KeyPressEventArgs(e.KeyChar))
    '                                Next
    '                            End If
    '                        End If
    '                        'cmbSProcess.Select()
    '                    End If
    '                End If
    '            End If
    '        Else
    '            loadJobDetails()
    '        End If
    '        If StkAvail Then SendKeys.Send("{TAB}")
    '    End If
    'End Sub

    Private Sub txtSOrdNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSOrdNo.KeyPress '''''''''''''SUDHARSAN CHANGED
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtSOrdNo.Text.Trim = "" Then
                SendKeys.Send("{tab}")
                Exit Sub
            End If
            Dim StkAvail As Boolean = True
            If _JobNoEnable = False Then
                If Mid(txtSOrdNo.Text, 1, 1).ToUpper = "R" Or Mid(txtSOrdNo.Text, 1, 1).ToUpper = "O" Then
                    If CheckJobNo(CType(sender, Control)) = False Then
                        MsgBox("Invalid Order Info" & vbCrLf & "This order no is already delivered or cancelled", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                    If CType(sender, Control).Text <> "" Then
                        If objOrderInfo Is Nothing Then
                            objOrderInfo = New MATERIALISSREC_ORDERINFO(CType(sender, Control).Text, BillCostId, IIf(oMaterial = Material.Issue, True, False), IIf(ORDER_MULTI_MIMR, Maccode, ""))
                            'objOrderInfo.BillCostId = BillCostId
                        End If
                        If objOrderInfo.ShowDialog() <> Windows.Forms.DialogResult.OK Then
                            Exit Sub
                        End If
                        Dim TempOrSno As String = ""
                        For cnt As Integer = 0 To objOrderInfo.DgvOrder.Rows.Count - 1
                            If CType(objOrderInfo.DgvOrder.Rows(cnt).Cells("CHECK").Value, Boolean) = True Then
                                TempOrSno += "'" & objOrderInfo.DgvOrder.Rows(cnt).Cells("SNO").Value.ToString & "',"
                            End If
                        Next
                        If TempOrSno.Contains(",") = True Then
                            TempOrSno = TempOrSno.Substring(0, TempOrSno.Length - 1)
                        End If
                        Dim DtOrder As New DataTable

                        If rbtStone.Checked Then

                            StrSql = vbCrLf + "SELECT M.METALNAME AS METAL,ISNULL(CA.CATNAME,C.CATNAME) CATEGORY,IT.ITEMNAME ITEM,SIT.SUBITEMNAME SUBITEM,"
                            StrSql += vbCrLf + " O.PCS-ISNULL((SELECT SUM(isnull(STNPCS,0)) FROM " & cnAdminDb & "..ORIRDETAILSTONE  WHERE ORSNO = ORD.SNO  "
                            StrSql += vbCrLf + " AND ISNULL(CANCEL,'')=''),0) PCS,OS.STNWT -ISNULL((SELECT SUM(isnull(STNWT,0)) "
                            StrSql += vbCrLf + " FROM " & cnAdminDb & "..ORIRDETAILSTONE  WHERE ORSNO = ORD.SNO   AND ISNULL(CANCEL,'')=''),0) WEIGHT "
                            StrSql += vbCrLf + " ,O.SNO,O.ORNO,OS.CUTID, OS.COLORID,OS.CLARITYID,OS.SHAPEID ,OS.SETTYPEID,OS.HEIGHT,OS.WIDTH,''SEIVE  FROM " & cnAdminDb & "..ORSTONE  OS  "
                            StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ORMAST AS O ON OS.ORSNO = O.SNO"
                            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ORIRDETAIL AS ORD ON ORD.ORSNO = O.SNO AND ORD.ORSTATUS= 'R'"
                            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON OS.STNITEMID=IT.ITEMID "
                            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SIT ON OS.STNSUBITEMID = SIT.SUBITEMID "
                            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALID = IT.METALID  "
                            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE= IT.CATCODE  "
                            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..OUTSTANDING OU ON OU.BATCHNO= O.BATCHNO AND OU.RUNNO=O.ORNO  "
                            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY CA ON CA.CATCODE= OU.CATCODE  WHERE O.SNO IN (" & TempOrSno & ")"
                            Da = New OleDbDataAdapter(StrSql, cn)
                            Da.Fill(DtOrder)
                            DtOrder.AcceptChanges()
                            If DtOrder.Rows.Count > 0 Then
                                cmbSCategory.Text = DtOrder.Rows(0).Item("CATEGORY").ToString
                                cmbSAcPostCategory.Items.Add(DtOrder.Rows(0).Item("CATEGORY").ToString)
                                cmbSAcPostCategory.Text = DtOrder.Rows(0).Item("CATEGORY").ToString
                                cmbSMetal.Text = DtOrder.Rows(0).Item("METAL").ToString
                                cmbSItem.Text = DtOrder.Rows(0).Item("ITEM").ToString
                                cmbSSubItem.Text = DtOrder.Rows(0).Item("SUBITEM").ToString
                                txtSPcs_NUM.Text = DtOrder.Rows(0).Item("PCS").ToString
                                txtSGrsWt_WET.Text = DtOrder.Rows(0).Item("WEIGHT").ToString
                                ORSNO = DtOrder.Rows(0).Item("SNO").ToString
                                ColorId = DtOrder.Rows(0).Item("COLORID").ToString
                                CutId = DtOrder.Rows(0).Item("CUTID").ToString
                                ClarityId = DtOrder.Rows(0).Item("CLARITYID").ToString
                                ShapeId = DtOrder.Rows(0).Item("SHAPEID").ToString
                                SetTypeId = DtOrder.Rows(0).Item("SETTYPEID").ToString
                                StnHeight = DtOrder.Rows(0).Item("HEIGHT").ToString
                                StnWidth = DtOrder.Rows(0).Item("WIDTH").ToString
                            End If
                            If oMaterial = Material.Issue Then
                                cmbSProcess.Text = "ISSUE TO SMITH"
                            Else
                                cmbSProcess.Text = "RECEIVED FROM SMITH"
                            End If

                            If txtStRowIndex.Text = "" Then
                                ''Insertion
                                Dim ro As DataRow = Nothing
                                ro = dtGridStuddedStone.NewRow
                                ro("ORDNO") = txtSOrdNo.Text : txtSOrdNo.Enabled = False
                                ro("METAL") = cmbSMetal.Text : cmbSMetal.Enabled = False
                                ro("CATEGORY") = cmbSCategory.Text : cmbSCategory.Enabled = False
                                ro("ISSCATEGORY") = cmbSIssuedCategory.Text : cmbSIssuedCategory.Enabled = False
                                ro("PURITY") = Val(CmbSPurity.Text) : CmbSPurity.Enabled = False
                                ro("ITEM") = cmbSItem.Text : cmbSItem.Enabled = False
                                ro("SUBITEM") = cmbSSubItem.Text : cmbSSubItem.Enabled = False
                                ro("PCS") = Val(txtSPcs_NUM.Text) : txtSPcs_NUM.Enabled = False
                                ro("WEIGHT") = Val(txtSGrsWt_WET.Text) : txtSGrsWt_WET.Enabled = False
                                ro("UNIT") = cmbSUnit.Text
                                ro("CALC") = cmbSCalcMode.Text
                                ro("RATE") = Val(txtSRate_OWN.Text) : txtSRate_OWN.Enabled = False
                                ro("BOARDRATE") = Val(oBoardRate)
                                ro("GROSSAMT") = Val(txtSGrsAmt_AMT.Text) : txtSGrsAmt_AMT.Enabled = False
                                ro("VATPER") = Val(txtSVatPer_PER.Text) : txtSVatPer_PER.Enabled = False
                                ro("VAT") = Val(txtSVat_AMT.Text) : txtSVat_AMT.Enabled = False
                                ro("AMOUNT") = Val(txtSAmount_AMT.Text) : txtSAmount_AMT.Enabled = False
                                ro("REMARK1") = txtSRemark1.Text : txtSRemark1.Enabled = False
                                ro("REMARK2") = txtSRemark2.Text : txtSRemark2.Enabled = False
                                ro("ORDSTATE_NAME") = cmbSProcess.Text
                                ro("ADDCHARGE") = Val(txtSAddlCharge_AMT.Text)
                                ro("SEIVE") = cmbSSeive.Text
                                ro("STNSNO") = StnSno
                                ro("RESNO") = RESNO
                                ro("ACCODE") = ACCODE
                                ro("RFID") = txtRfId.Text.ToString.Trim
                                ro("TAGNO") = txtTagNo.Text.ToString.Trim
                                ro("SGSTPER") = Val(txtSSgst_WET.Text)
                                ro("SGST") = Val(txtSSG_AMT.Text)
                                ro("CGSTPER") = Val(txtSCgst_WET.Text)
                                ro("CGST") = Val(txtSCG_AMT.Text)
                                ro("IGSTPER") = Val(txtSIgst_WET.Text)
                                ro("IGST") = Val(txtSIG_AMT.Text)
                                ro("TCS") = Val(txtSTCS_AMT.Text)
                                ro("WASTAGE") = Val(txtSWast_WET.Text)

                                If ro("CUTID") Is DBNull.Value Then ro("CUTID") = Convert.ToString(CutId)
                                If ro("COLORID") Is DBNull.Value Then ro("COLORID") = Convert.ToString(ColorId)
                                If ro("CLARITYID") Is DBNull.Value Then ro("CLARITYID") = Convert.ToString(ClarityId)
                                If ro("SHAPEID") Is DBNull.Value Then ro("SHAPEID") = Convert.ToString(ShapeId)
                                If ro("SETTYPEID") Is DBNull.Value Then ro("SETTYPEID") = Convert.ToString(SetTypeId)
                                If ro("HEIGHT") Is DBNull.Value Then ro("HEIGHT") = Convert.ToString(StnHeight)
                                If ro("WIDTH") Is DBNull.Value Then ro("WIDTH") = Convert.ToString(StnWidth)
                                dtGridStuddedStone.Rows.Add(ro)
                            End If
                            GridStuddStone.DataSource = Nothing
                            GridStuddStone.DataSource = dtGridStuddedStone
                        Else
                            StrSql = "SELECT M.METALNAME AS METAL,ISNULL(CA.CATNAME,C.CATNAME) CATEGORY,IT.ITEMNAME ITEM,SIT.SUBITEMNAME SUBITEM,"
                            StrSql += " O.PCS-ISNULL((SELECT SUM(isnull(PCS,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND  ORSTATUS = 'R' AND ISNULL(CANCEL,'')=''),0) PCS,O.GRSWT-ISNULL((SELECT SUM(isnull(GRSWT,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND ORSTATUS = 'R'  AND ISNULL(CANCEL,'')=''),0) GRSWT ,O.NETWT-ISNULL((SELECT SUM(isnull(NETWT,0)) FROM " & cnAdminDb & "..ORIRDETAIL WHERE ORSNO = O.SNO AND ORSTATUS = 'R'  AND ISNULL(CANCEL,'')=''),0) NETWT"
                            StrSql += " ,O.SNO "
                            StrSql += " FROM " & cnAdminDb & "..ORMAST O "
                            StrSql += " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON O.ITEMID=IT.ITEMID"
                            StrSql += " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SIT ON O.SUBITEMID = SIT.SUBITEMID"
                            StrSql += " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALID = IT.METALID "
                            StrSql += " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE= IT.CATCODE "
                            StrSql += " LEFT JOIN " & cnAdminDb & "..OUTSTANDING OU ON OU.BATCHNO= O.BATCHNO AND OU.RUNNO=O.ORNO "
                            StrSql += " LEFT JOIN " & cnAdminDb & "..CATEGORY CA ON CA.CATCODE= OU.CATCODE "
                            StrSql += " WHERE O.SNO IN (" & TempOrSno & ")"
                            Da = New OleDbDataAdapter(StrSql, cn)
                            Da.Fill(DtOrder)
                            DtOrder.AcceptChanges()
                            If DtOrder.Rows.Count > 0 Then
                                cmbOCategory.Text = DtOrder.Rows(0).Item("CATEGORY").ToString : cmbOCategory.Enabled = False
                                cmbOMetal.Text = DtOrder.Rows(0).Item("METAL").ToString : cmbOMetal.Enabled = False
                                cmbOItem.Text = DtOrder.Rows(0).Item("ITEM").ToString : cmbOItem.Enabled = False
                                cmbOSubItem.Text = DtOrder.Rows(0).Item("SUBITEM").ToString : cmbOSubItem.Enabled = False
                                txtOPcs_NUM.Text = DtOrder.Rows(0).Item("PCS").ToString
                                txtOGrsWt_WET.Text = DtOrder.Rows(0).Item("GRSWT").ToString
                                txtONetWt_WET.Text = DtOrder.Rows(0).Item("NETWT").ToString
                                ORSNO = DtOrder.Rows(0).Item("SNO").ToString
                                OGrswt = Val(DtOrder.Rows(0).Item("GRSWT").ToString)
                                ONetwt = Val(DtOrder.Rows(0).Item("NETWT").ToString)
                            End If
                        End If
                    Else
                        cmbSMetal.Enabled = True
                        cmbSCategory.Enabled = True
                        cmbSIssuedCategory.Enabled = True
                        CmbSPurity.Enabled = True
                        cmbSItem.Enabled = True
                        cmbSSubItem.Enabled = True
                        txtSPcs_NUM.Enabled = True
                        txtSGrsWt_WET.Enabled = True
                        cmbSUnit.Enabled = True
                        cmbSCalcMode.Enabled = True
                        txtSRate_OWN.Enabled = True
                        txtSGrsAmt_AMT.Enabled = True

                        txtSAddlCharge_AMT.Enabled = True
                        txtSVatPer_PER.Enabled = True
                        txtSVat_AMT.Enabled = True
                        txtSAmount_AMT.Enabled = True
                    End If

                Else
                    If STOCKVALIDATION And oMaterial = Material.Issue Then
                        StkAvail = LoadBalanceStock()
                    ElseIf STOCKVALIDATION And STOCKVALIDATION_MR = False And oMaterial = Material.Receipt Then
                        Dim sql As String = ""
                        StrSql = vbCrLf + "  SELECT TRANNO"
                        StrSql += vbCrLf + " ,TRANDATE,ITEM,SUBITEM,PCS,UNIT,CALC,WEIGHT,RATE,AMOUNT"
                        StrSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = X.CATCODE)CATNAME"
                        StrSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = X.METALID)METAL,SNO,''STNTYPE "
                        StrSql += vbCrLf + " FROM("
                        StrSql += vbCrLf + " SELECT I.TRANNO,I.TRANDATE,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM"
                        StrSql += vbCrLf + " ,PCS-"
                        StrSql += vbCrLf + " (SELECT SUM(PCS)PCS FROM ("
                        StrSql += vbCrLf + " SELECT ISNULL(SUM(PCS),0)PCS FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(ISSNO,'')=I.SNO AND ISNULL(CANCEL,'')='' "
                        StrSql += vbCrLf + " UNION ALL "
                        StrSql += vbCrLf + " SELECT ISNULL(SUM(STNPCS),0)PCS FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISNULL(JOBISNO,'')=I.SNO AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE  ISNULL(CANCEL,'')='') )X"
                        StrSql += vbCrLf + " ) AS PCS"
                        StrSql += vbCrLf + " ,I.STONEUNIT UNIT,"
                        StrSql += vbCrLf + " CASE WHEN ISNULL(I.WEIGHTUNIT,'')='' THEN 'W' ELSE I.WEIGHTUNIT END CALC"
                        StrSql += vbCrLf + " ,PUREWT-"
                        StrSql += vbCrLf + " (SELECT SUM(PUREWT)PUREWT FROM ("
                        StrSql += vbCrLf + " SELECT ISNULL(SUM(PUREWT),0)PUREWT FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(ISSNO,'')=I.SNO  AND ISNULL(CANCEL,'')=''"
                        StrSql += vbCrLf + " UNION ALL "
                        StrSql += vbCrLf + " SELECT ISNULL(SUM(STNWT),0)PUREWT FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISNULL(JOBISNO,'')=I.SNO AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE  ISNULL(CANCEL,'')='') )X"
                        StrSql += vbCrLf + " ) AS WEIGHT"
                        StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) RATE,"
                        StrSql += vbCrLf + " CONVERT(NUMERIC(15,2),NULL) AMOUNT,I.METALID,I.CATCODE,I.SNO"
                        StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I INNER JOIN " & cnAdminDb & "..ITEMMAST IM ON I.ITEMID=IM.ITEMID "
                        StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.ITEMID =I.ITEMID AND SI.SUBITEMID =I.SUBITEMID "
                        StrSql += vbCrLf + " WHERE LEN(I.TRANTYPE)>2  "
                        StrSql += vbCrLf + " AND I.TRANTYPE<>'IIN' AND I.TRANTYPE ='IIS'"
                        StrSql += vbCrLf + " And ISNULL(STUDDED,'') IN ('S','L','B') AND ISNULL(I.CANCEL,'')='' "
                        StrSql += vbCrLf + " AND I.ACCODE='" & Maccode & "'"
                        StrSql += vbCrLf + " AND I.SNO='" & InsSno & "'"
                        StrSql += vbCrLf + " ) X WHERE ISNULL(PCS,0)<>0 OR ISNULL(WEIGHT,0)<>0 "
                        sql = " SELECT count(*) FROM " & cnAdminDb & "..DBMASTER "
                        Dim prevdb As String = cnStockDb
                        Dim cnt As Integer = (objGPack.GetSqlValue(sql))
                        For X As Integer = 0 To cnt - 1
                            sql = " SELECT STARTDATE-1 FROM " & cnAdminDb & "..DBMASTER WHERE DBNAME = '" & prevdb & "'"
                            Dim menddate As Date = (objGPack.GetSqlValue(sql))
                            sql = " SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER WHERE '" & menddate & "' BETWEEN STARTDATE AND ENDDATE"
                            prevdb = objGPack.GetSqlValue(sql)
                            sql = " SELECT 'EXISTS' FROM MASTER..SYSDATABASES WHERE NAME = '" & prevdb & "'"
                            If objGPack.GetSqlValue(sql, , "-1") <> "-1" Then
                                StrSql += vbCrLf + "  UNION ALL"
                                StrSql += vbCrLf + "  SELECT TRANNO"
                                StrSql += vbCrLf + " ,TRANDATE,ITEM,SUBITEM,PCS,UNIT,CALC,WEIGHT,RATE,AMOUNT"
                                StrSql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = X.CATCODE)CATNAME"
                                StrSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = X.METALID)METAL,SNO,''STNTYPE "
                                StrSql += vbCrLf + " FROM("
                                StrSql += vbCrLf + " SELECT I.TRANNO,I.TRANDATE,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM"
                                StrSql += vbCrLf + " ,PCS-"
                                StrSql += vbCrLf + " (SELECT SUM(PCS)PCS FROM ("
                                StrSql += vbCrLf + " SELECT ISNULL(SUM(PCS),0)PCS FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(ISSNO,'')=I.SNO AND ISNULL(CANCEL,'')='' "
                                StrSql += vbCrLf + " UNION ALL "
                                StrSql += vbCrLf + " SELECT ISNULL(SUM(STNPCS),0)PCS FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISNULL(JOBISNO,'')=I.SNO AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE  ISNULL(CANCEL,'')='') )X"
                                StrSql += vbCrLf + " ) AS PCS"
                                StrSql += vbCrLf + " ,I.STONEUNIT UNIT,"
                                StrSql += vbCrLf + " CASE WHEN ISNULL(I.WEIGHTUNIT,'')='' THEN 'W' ELSE I.WEIGHTUNIT END CALC"
                                StrSql += vbCrLf + " ,PUREWT-"
                                StrSql += vbCrLf + " (SELECT SUM(PUREWT)PUREWT FROM ("
                                StrSql += vbCrLf + " SELECT ISNULL(SUM(PUREWT),0)PUREWT FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(ISSNO,'')=I.SNO  AND ISNULL(CANCEL,'')=''"
                                StrSql += vbCrLf + " UNION ALL "
                                StrSql += vbCrLf + " SELECT ISNULL(SUM(STNWT),0)PUREWT FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISNULL(JOBISNO,'')=I.SNO AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE  ISNULL(CANCEL,'')='') )X"
                                StrSql += vbCrLf + " ) AS WEIGHT"
                                StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) RATE,"
                                StrSql += vbCrLf + " CONVERT(NUMERIC(15,2),NULL) AMOUNT,I.METALID,I.CATCODE,I.SNO"
                                StrSql += vbCrLf + " FROM " & prevdb & "..ISSUE I INNER JOIN " & cnAdminDb & "..ITEMMAST IM ON I.ITEMID=IM.ITEMID "
                                StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.ITEMID =I.ITEMID AND SI.SUBITEMID =I.SUBITEMID "
                                StrSql += vbCrLf + " WHERE LEN(I.TRANTYPE)>2  "
                                StrSql += vbCrLf + " AND I.TRANTYPE<>'IIN' AND I.TRANTYPE ='IIS'"
                                StrSql += vbCrLf + " And ISNULL(STUDDED,'') IN ('S','L','B') AND ISNULL(I.CANCEL,'')='' "
                                StrSql += vbCrLf + " AND I.ACCODE='" & Maccode & "'"
                                StrSql += vbCrLf + " AND I.SNO='" & InsSno & "'"
                                StrSql += vbCrLf + " ) X WHERE ISNULL(PCS,0)<>0 OR ISNULL(WEIGHT,0)<>0 "
                            End If
                        Next
                        Dim dtStk As New DataTable
                        Cmd = New OleDbCommand(StrSql, cn)
                        Da = New OleDbDataAdapter(Cmd)
                        Da.Fill(dtStk)
                        If dtStk.Rows.Count > 0 Then
                            LoadMetal(cmbSMetal)
                            cmbSMetal.Text = dtStk.Rows(0).Item("METAL").ToString
                            cmbSCategory.Text = dtStk.Rows(0).Item("CATNAME").ToString
                            cmbMIssuedCategory.Text = dtStk.Rows(0).Item("CATNAME").ToString
                            cmbSItem.Text = dtStk.Rows(0).Item("ITEM").ToString
                            cmbSSubItem.Text = dtStk.Rows(0).Item("SUBITEM").ToString
                            cmbSUnit.Text = IIf(dtStk.Rows(0).Item("UNIT").ToString = "G", "GRAM", "CARAT")
                            StkUnit = dtStk.Rows(0).Item("UNIT").ToString
                            txtSPcs_NUM.Text = Val(dtStk.Compute("SUM(PCS)", Nothing).ToString)
                            txtSGrsWt_WET.Text = Val(dtStk.Compute("SUM(WEIGHT)", Nothing).ToString)
                        End If
                    ElseIf STOCKVALIDATION_MR And oMaterial = Material.Receipt Then
                        StkAvail = LoadBalanceStock_MR(cmbSMetal.Text)
                    Else
                        If Val(txtSOrdNo.Text) <> 0 Then
                            If rbtStone.Checked Then
                                StrSql = vbCrLf + "  SELECT ITEM,SUBITEM,PCS,UNIT,CALC,WEIGHT,RATE,AMOUNT,METALID,SNO,CATNAME,METAL FROM("
                                StrSql += vbCrLf + " SELECT IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM"
                                StrSql += vbCrLf + " ,(PCS-ISNULL((SELECT SUM(PCS) FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO ),0)) AS PCS,I.STONEUNIT UNIT,"
                                StrSql += vbCrLf + " CASE WHEN ISNULL(I.WEIGHTUNIT,'')='' THEN 'W' ELSE I.WEIGHTUNIT END CALC"
                                StrSql += vbCrLf + " ,(PUREWT-ISNULL((SELECT SUM(PUREWT) FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO ),0)) WEIGHT"
                                StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) RATE,"
                                StrSql += vbCrLf + " CONVERT(NUMERIC(15,2),NULL) AMOUNT,I.METALID,I.SNO,C.CATNAME,M.METALNAME AS METAL"
                                StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON I.ITEMID=IM.ITEMID "
                                StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.ITEMID =I.ITEMID AND SI.SUBITEMID =I.SUBITEMID "
                                StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE =I.CATCODE "
                                StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALID =I.METALID "
                                StrSql += vbCrLf + " WHERE I.TRANTYPE ='IIS'  AND ISNULL(STUDDED,'') IN ('S','L') AND ISNULL(I.CANCEL,'')='' "
                                StrSql += vbCrLf + " AND I.SNO NOT IN (SELECT DISTINCT JOBISNO FROM " & cnStockDb & "..RECEIPTSTONE WHERE "
                                StrSql += vbCrLf + " ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(CANCEL,'')='') AND ISNULL(JOBISNO,'')<>'')"
                                StrSql += vbCrLf + " AND I.TRANNO =" & Val(txtSOrdNo.Text)
                                StrSql += vbCrLf + " ) X WHERE ISNULL(PCS,0)<>0 OR ISNULL(WEIGHT,0)<>0 "
                                Dim dtGrid As New DataTable
                                dtGrid.Columns.Add("CHECK", GetType(Boolean))
                                Da = New OleDbDataAdapter(StrSql, cn)
                                Da.Fill(dtGrid)
                                If Not dtGrid.Rows.Count > 0 Then
                                    StrSql = " SELECT STARTDATE-1 FROM " & cnAdminDb & "..DBMASTER WHERE DBNAME = '" & cnStockDb & "'"
                                    Dim menddate As Date = (objGPack.GetSqlValue(StrSql))
                                    StrSql = " SELECT DBNAME FROM " & cnAdminDb & "..DBMASTER WHERE '" & menddate & "' BETWEEN STARTDATE AND ENDDATE"
                                    Dim prevdb As String = objGPack.GetSqlValue(StrSql)
                                    StrSql = " SELECT 'EXISTS' FROM MASTER..SYSDATABASES WHERE NAME = '" & prevdb & "'"
                                    If objGPack.GetSqlValue(StrSql, , "-1") = "-1" Then
                                        txtSOrdNo.Select()
                                        Exit Sub
                                    End If
                                    'StrSql = vbCrLf + "  SELECT IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM,PCS,I.STONEUNIT UNIT,"
                                    'StrSql += vbCrLf + " CASE WHEN ISNULL(I.WEIGHTUNIT,'')='' THEN 'W' ELSE I.WEIGHTUNIT END CALC,"
                                    'StrSql += vbCrLf + " PUREWT WEIGHT,CONVERT(NUMERIC(15,2),NULL) RATE,"
                                    'StrSql += vbCrLf + " CONVERT(NUMERIC(15,2),NULL) AMOUNT,I.METALID,I.SNO,C.CATNAME"
                                    'StrSql += vbCrLf + " FROM " & prevdb & "..ISSUE I INNER JOIN " & cnAdminDb & "..ITEMMAST IM ON I.ITEMID=IM.ITEMID "
                                    'StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.ITEMID =I.ITEMID AND SI.SUBITEMID =I.SUBITEMID "
                                    'StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON I.CATCODE=C.CATCODE"
                                    'StrSql += vbCrLf + " WHERE I.TRANTYPE ='IIS'  AND ISNULL(STUDDED,'') IN ('S','L') AND ISNULL(I.CANCEL,'')='' "
                                    'StrSql += vbCrLf + " AND I.SNO NOT IN (SELECT DISTINCT JOBISNO FROM " & prevdb & "..RECEIPTSTONE WHERE "
                                    'StrSql += vbCrLf + " ISSSNO IN (SELECT SNO FROM " & prevdb & "..RECEIPT WHERE ISNULL(CANCEL,'')='') and isnull(jobisno,'')<>'')"
                                    'StrSql += vbCrLf + " AND I.TRANNO =" & Val(txtSOrdNo.Text)

                                    StrSql = vbCrLf + "  SELECT ITEM,SUBITEM,PCS,UNIT,CALC,WEIGHT,RATE,AMOUNT,METALID,SNO,CATNAME,METAL FROM("
                                    StrSql += vbCrLf + " SELECT IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM"
                                    StrSql += vbCrLf + " ,(PCS-ISNULL((SELECT SUM(PCS) FROM " & prevdb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO ),0)) AS PCS,I.STONEUNIT UNIT,"
                                    StrSql += vbCrLf + " CASE WHEN ISNULL(I.WEIGHTUNIT,'')='' THEN 'W' ELSE I.WEIGHTUNIT END CALC"
                                    StrSql += vbCrLf + " ,(PUREWT-ISNULL((SELECT SUM(PUREWT) FROM " & prevdb & "..RECEIPT WHERE ISNULL(JOBISNO,'')=I.SNO ),0)) WEIGHT"
                                    StrSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),NULL) RATE,"
                                    StrSql += vbCrLf + " CONVERT(NUMERIC(15,2),NULL) AMOUNT,I.METALID,I.SNO,C.CATNAME,M.METALNAME AS METAL"
                                    StrSql += vbCrLf + " FROM " & prevdb & "..ISSUE I LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON I.ITEMID=IM.ITEMID "
                                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.ITEMID =I.ITEMID AND SI.SUBITEMID =I.SUBITEMID "
                                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON I.CATCODE =I.CATCODE "
                                    StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON M.METALID =I.METALID "
                                    StrSql += vbCrLf + " WHERE I.TRANTYPE ='IIS'  AND ISNULL(STUDDED,'') IN ('S','L') AND ISNULL(I.CANCEL,'')='' "
                                    StrSql += vbCrLf + " AND I.SNO NOT IN (SELECT DISTINCT JOBISNO FROM " & prevdb & "..RECEIPTSTONE WHERE "
                                    StrSql += vbCrLf + " ISSSNO IN (SELECT SNO FROM " & prevdb & "..RECEIPT WHERE ISNULL(CANCEL,'')='') AND ISNULL(JOBISNO,'')<>'')"
                                    StrSql += vbCrLf + " AND I.TRANNO =" & Val(txtSOrdNo.Text)
                                    StrSql += vbCrLf + " ) X WHERE ISNULL(PCS,0)<>0 OR ISNULL(WEIGHT,0)<>0 "
                                    dtGrid = New DataTable
                                    dtGrid.Columns.Add("CHECK", GetType(Boolean))
                                    Da = New OleDbDataAdapter(StrSql, cn)
                                    Da.Fill(dtGrid)
                                    If Not dtGrid.Rows.Count > 0 Then
                                        txtSOrdNo.Select()
                                        Exit Sub
                                    End If
                                End If
                                If dtGridStuddedStone.Rows.Count > 0 Then
                                    With dtGridStuddedStone
                                        For J As Integer = 0 To .Rows.Count - 1
                                            Dim STNSNO As String = .Rows(J).Item("STNSNO").ToString
                                            For K As Integer = 0 To dtGrid.Rows.Count - 1
                                                If dtGrid.Rows(K).Item("SNO").ToString = STNSNO Then
                                                    dtGrid.Rows(K).Item("PCS") = Val(dtGrid.Rows(K).Item("PCS").ToString) - Val(.Rows(J).Item("PCS").ToString)
                                                    dtGrid.Rows(K).Item("WEIGHT") = Val(dtGrid.Rows(K).Item("WEIGHT").ToString) - Val(.Rows(J).Item("WEIGHT").ToString)
                                                End If
                                            Next
                                        Next
                                    End With
                                End If
                                objMultiSelect = New MultiSelectRowDia(dtGrid, "SNO")
                                objMultiSelect.chkAppSales.Visible = False
                                If objMultiSelect.ShowDialog = Windows.Forms.DialogResult.OK Then
                                    Me.Refresh()
                                    Dim sno As String = ""
                                    For Each RoS As DataRow In objMultiSelect.RowSelected
                                        cmbSCategory.Text = RoS.Item("CATNAME").ToString : cmbSCategory.Enabled = False
                                        cmbSMetal.Text = RoS.Item("METAL").ToString : cmbSMetal.Enabled = False
                                        cmbSItem.Text = RoS.Item("ITEM").ToString : cmbSItem.Enabled = False
                                        cmbSSubItem.Text = RoS.Item("SUBITEM").ToString : cmbSSubItem.Enabled = False
                                        txtSPcs_NUM.Text = RoS.Item("PCS").ToString
                                        txtSGrsWt_WET.Text = RoS.Item("WEIGHT").ToString
                                        cmbSUnit.Text = RoS.Item("UNIT").ToString
                                        cmbSCalcMode.Text = RoS.Item("CALC").ToString
                                        txtSRate_OWN.Text = Val(RoS.Item("RATE").ToString)
                                        txtSGrsAmt_AMT.Text = Val(RoS.Item("AMOUNT").ToString)
                                        StnSno = RoS.Item("SNO").ToString

                                        If oMaterial = Material.Receipt Then
                                            cmbSProcess.Text = objGPack.GetSqlValue("SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID=3", "", "", )
                                        Else
                                            cmbSProcess.Text = objGPack.GetSqlValue("SELECT ORDSTATE_NAME FROM " & cnAdminDb & "..ORDERSTATUS WHERE ORDSTATE_ID=2", "", "", )
                                        End If
                                        'txtSRemark2_KeyPress(Me, New KeyPressEventArgs(e.KeyChar))
                                    Next
                                End If
                            End If
                            'cmbSProcess.Select()
                        End If
                    End If
                End If
            Else
                loadJobDetails()
            End If
            If StkAvail Then SendKeys.Send("{TAB}")
        End If
    End Sub



    Private Sub txtMJobNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMOrdNo.TextChanged
        objOrderInfo = Nothing
    End Sub

    Private Function RoundOffPisa(ByVal value As Decimal, Optional ByVal Istds As Boolean = False) As Decimal
        Dim mRnd As String
        If Istds Then mRnd = ROUNDOFF_ACC_TDS Else mRnd = ROUNDOFF_ACC
        Select Case mRnd
            Case "L"
                Return Math.Floor(value)
            Case "F"
                If Math.Abs(value - Math.Floor(value)) >= 0.5 Then
                    Return Math.Ceiling(value)
                Else
                    Return Math.Floor(value)
                End If
            Case "H"
                Return Math.Ceiling(value)
            Case Else
                Return value
        End Select
        Return value
    End Function

    Private Sub txtOAmount_AMT_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOAmount_AMT.LostFocus
        Dim grsAmt As Decimal = Nothing
        Dim vatAmt As Decimal = Val(txtOVat_AMT.Text)
        'If oTransactionType = "PURCHASE RETURN" Then vatAmt = vatAmt * -1
        grsAmt = Val(txtOAmount_AMT.Text) - IIf(lblOVat.Text.ToUpper = "TDS", -1 * vatAmt, vatAmt) - Val(txtOED_AMT.Text) - Val(txtOTCS_AMT.Text.ToString)
        If lblOVat.Text.ToUpper = "TDS" Then
            grsAmt -= Val(txtOSG_AMT.Text) + Val(txtOCG_AMT.Text) + Val(txtOIG_AMT.Text)
        End If
        grsAmt = RoundOffPisa(grsAmt)
        txtOGrsAmt_AMT.Text = IIf(grsAmt <> 0, Format(grsAmt, "0.00"), Nothing)
    End Sub

    Private Sub txtOGrsAmt_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOGrsAmt_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) And RateCalcAmt = True And UCase(oTransactionType) = "PURCHASE" Then
            Dim GrsAmt As Decimal = Val(txtOGrsAmt_AMT.Text.ToString) - (Val(txtOED_AMT.Text) + Val(txtOMc_AMT.Text) + Val(txtOStudAmt_AMT.Text) + Val(txtOAddlCharge_AMT.Text))
            Dim AvgRate As Decimal = Nothing
            If GrsAmt = 0 Then Exit Sub
            If Val(txtOGrsWt_WET.Text) <> 0 Then
                AvgRate = GrsAmt / (IIf(cmbOGrsNet.Text = "GRS WT", Val(txtOGrsWt_WET.Text), Val(txtONetWt_WET.Text)) _
                + Val(txtOWast_WET.Text) + (-1 * Val(txtOalloy_WET.Text)))
                If Val(txtOTouchAMT.Text) <> 0 Then
                    AvgRate = GrsAmt / Val(txtOPureWt_WET.Text)
                End If
            Else
                AvgRate = GrsAmt / Val(txtOPcs_NUM.Text)
            End If
            AvgRate = RoundOffPisa(AvgRate)
            If AvgRate < 0 Then AvgRate = 0
            txtORate_OWN.Text = IIf(AvgRate <> 0, Format(AvgRate, "0.00"), Nothing)
        End If
    End Sub

    Private Sub txtOGrsAmt_AMT_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOGrsAmt_AMT.LostFocus
        If oTransactionType = "PURCHASE" Or oTransactionType = "PURCHASE[APPROVAL]" Then EdcalculationNew(Val(txtOGrsAmt_AMT.Text))
        CalcONetAmt()
    End Sub

    Private Sub txtMAmount_AMT_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMAmount_AMT.LostFocus
        Dim grsAmt As Decimal = Nothing
        Dim vatAmt As Decimal = Val(txtMVat_AMT.Text)
        'If oTransactionType = "PURCHASE RETURN" Then vatAmt = vatAmt * -1
        grsAmt = Val(txtMAmount_AMT.Text) - IIf(lblMVat.Text.ToUpper = "TDS", -1 * vatAmt, vatAmt) - Val(txtMTCS_AMT.Text.ToString)
        If lblMVat.Text.ToUpper = "TDS" Then
            grsAmt -= Val(txtMSG_AMT.Text) + Val(txtMCG_AMT.Text) + Val(txtMIG_AMT.Text)
        End If
        grsAmt = RoundOffPisa(grsAmt)
        txtMGrsAmt_AMT.Text = IIf(grsAmt <> 0, Format(grsAmt, "0.00"), Nothing)
    End Sub

    Private Sub txtMGrsAmt_AMT_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMGrsAmt_AMT.LostFocus
        CalcMNetAmt()
    End Sub

    Private Sub txtSAmount_AMT_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSAmount_AMT.LostFocus
        Dim grsAmt As Decimal = Nothing
        Dim vatAmt As Decimal = Val(txtSVat_AMT.Text)
        'If oTransactionType = "PURCHASE RETURN" Then vatAmt = vatAmt * -1
        grsAmt = Val(txtSAmount_AMT.Text) - IIf(lblSVat.Text.ToUpper = "TDS", -1 * vatAmt, vatAmt) - Val(txtSTCS_AMT.Text.ToString)
        If lblSVat.Text.ToUpper = "TDS" Then
            grsAmt -= Val(txtSSG_AMT.Text) + Val(txtSCG_AMT.Text) + Val(txtSIG_AMT.Text)
        End If
        grsAmt = RoundOffPisa(grsAmt)
        txtSGrsAmt_AMT.Text = IIf(grsAmt <> 0, Format(grsAmt, "0.00"), Nothing)
    End Sub

    Private Sub txtSGrsAmt_AMT_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSGrsAmt_AMT.LostFocus
        CalcSNetAmt()
    End Sub

    Private Sub txtMReceivePurity_AMT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMReceivePurity_AMT.KeyDown
        If e.KeyCode = Keys.Insert Then
            LoadMReceivePurity()
        End If
    End Sub

    Private Sub EdcalculationNew(ByVal GrsAmount As Decimal)
        If ED_COMPONENTS = "" Then Exit Sub
        If GstFlag Then Exit Sub
        Dim DtEd As New DataTable
        StrSql = "SELECT TAXCODE,STAX FROM " & cnAdminDb & "..TAXMAST WHERE TAXCODE IN ('" & Replace(ED_COMPONENTS, ",", "','") & "') ORDER BY DISPLAYORDER"
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(DtEd)
        If DtEd.Rows.Count > 0 Then
            Exdutyper = Val("" & DtEd.Rows(0).Item(1).ToString)
            If Exdutyper > 0 Then
                txtOED_AMT.Text = Format((GrsAmount * Exdutyper) / 100, "0.00")
            End If
        End If
    End Sub

    Private Sub Edcalculation(ByVal GrsAmount As Decimal)
        If ED_COMPONENTS = "" Then Exit Sub
        Dim DtEd As New DataTable
        objTds.txtActualAmount_AMT.Text = GrsAmount
        StrSql = "SELECT TAXCODE,STAX FROM " & cnAdminDb & "..TAXMAST WHERE TAXCODE IN ('" & Replace(ED_COMPONENTS, ",", "','") & "') ORDER BY DISPLAYORDER"
        Da = New OleDbDataAdapter(StrSql, cn)
        Da.Fill(DtEd)
        If DtEd.Rows.Count > 0 Then
            objTds.EditFlag = True
            objTds.txtEdPer_PER.Text = Val("" & DtEd.Rows(0).Item(1).ToString)
            If DtEd.Rows.Count > 1 Then
                objTds.txtEcPer_PER.Text = Val("" & DtEd.Rows(1).Item(1).ToString)
            End If
            If DtEd.Rows.Count > 2 Then
                objTds.txtHePer_PER.Text = Val("" & DtEd.Rows(2).Item(1).ToString)
            End If
        End If
        '        objTds.txtActualAmount_AMT.Select()

        objTds.ShowDialog()
        'gridView_OWN.CurrentRow.Cells("TDSPER").Value = Val(objTds.txtTdsPer_PER.Text)
        'gridView_OWN.CurrentRow.Cells("TDSAMT").Value = Val(objTds.txtTdsAmt_AMT.Text)
        'gridView_OWN.CurrentRow.Cells("TDSCATID").Value = Val(objTds.cmbTdsCategory_OWN.SelectedValue.ToString)
    End Sub
    Private Sub LoadMReceivePurity()
        StrSql = vbCrLf + " SELECT DISTINCT PU.PURITYNAME,PU.PURITY,PU.METALTYPE FROM " & cnAdminDb & "..PURITYMAST AS PU"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.PURITYID = PU.PURITYID AND CA.METALID = PU.METALID"
        StrSql += vbCrLf + " WHERE PU.METALID IN (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbMCategory.Text & "')"
        txtMReceivePurity_AMT.Text = BrighttechPack.SearchDialog.Show("Select Receive Purity", StrSql, cn, 1, 1)
        If Val(txtMReceivePurity_AMT.Text) = 0 Then
            txtMReceivePurity_AMT.Text = IIf(txtMTouchAMT.Text <> "", txtMTouchAMT.Text, CmbMPurity.Text)
        End If
    End Sub

    Private Sub txtMReceivePurity_AMT_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMReceivePurity_AMT.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtMReceivePurity_AMT.Text = "" Then
                LoadMReceivePurity()
                Exit Sub
            Else

                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Sub txtMReceivePurity_AMT_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMReceivePurity_AMT.LostFocus
        'If Val(txtMReceivePurity_AMT.Text) <> 0 And lblMWast.Text.ToUpper = "ALLOY" Then
        '    ''Alloy Calc
        '    If Val(txtMPureWt_WET.Text) <> 0 And Val(txtMGrsWt_WET.Text) <> 0 Then
        '        Dim allwt As Decimal = Val("" & txtMPureWt_WET.Text) / (Val("" & txtMReceivePurity_AMT.Text) / 100)
        '        allwt = ((allwt - Val("" & txtMGrsWt_WET.Text)) / Val("" & txtMGrsWt_WET.Text)) * 100
        '        txtMWastPER.Text = IIf(allwt <> 0, Format(allwt, FormatNumberStyle(AlloyRnd)), Nothing)
        '    End If
        'End If
        If Val(txtMReceivePurity_AMT.Text) <> 0 And txtMAlloy_WET.Enabled = True Then
            ''Alloy Calc
            If Val(txtMPureWt_WET.Text) <> 0 And Val(txtMGrsWt_WET.Text) <> 0 Then
                Dim allwt As Decimal = Val("" & txtMPureWt_WET.Text) / (Val("" & txtMReceivePurity_AMT.Text) / 100)
                allwt = ((allwt - Val("" & txtMGrsWt_WET.Text)) / Val("" & txtMGrsWt_WET.Text)) * 100
                txtMAlloyper.Text = IIf(allwt <> 0, Format(allwt, FormatNumberStyle(AlloyRnd)), Nothing)
            End If
        End If
    End Sub

    Private Sub txtOAddlCharge_AMT_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOAddlCharge_AMT.LostFocus
        If Val(txtOAddlCharge_AMT.Text) <> 0 Then
            objMisc.BackColor = Me.BackColor
            objMisc.StartPosition = FormStartPosition.CenterScreen
            objMisc.MaximizeBox = False
            'objMisc.grpMisc.BackgroundColor = grpHeader.BackgroundColor
            objMisc.StyleGridMisc(objMisc.gridMisc)
            objMisc.txtMiscMisc.Select()
            objMisc.ShowDialog()
            Dim miscAmt As Double = Val(objMisc.gridMiscTotal.Rows(0).Cells("AMOUNT").Value.ToString)
            txtOAddlCharge_AMT.Text = IIf(miscAmt <> 0, Format(miscAmt, "0.00"), Nothing)
        Else
            objMisc = New frmMiscDia
        End If
    End Sub
    Private Sub txtMAddlCharge_AMT_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMAddlCharge_AMT.LostFocus
        If Val(txtMAddlCharge_AMT.Text) <> 0 Then
            objMisc.BackColor = Me.BackColor
            objMisc.StartPosition = FormStartPosition.CenterScreen
            objMisc.MaximizeBox = False
            'objMisc.grpMisc.BackgroundColor = grpHeader.BackgroundColor
            objMisc.StyleGridMisc(objMisc.gridMisc)
            objMisc.txtMiscMisc.Select()
            objMisc.ShowDialog()
            Dim miscAmt As Double = Val(objMisc.gridMiscTotal.Rows(0).Cells("AMOUNT").Value.ToString)
            txtMAddlCharge_AMT.Text = IIf(miscAmt <> 0, Format(miscAmt, "0.00"), Nothing)
        Else
            objMisc = New frmMiscDia
        End If
    End Sub
    Private Sub txtSAddlCharge_AMT_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSAddlCharge_AMT.LostFocus
        If Val(txtSAddlCharge_AMT.Text) <> 0 Then
            objMisc.BackColor = Me.BackColor
            objMisc.StartPosition = FormStartPosition.CenterScreen
            objMisc.MaximizeBox = False
            'objMisc.grpMisc.BackgroundColor = grpHeader.BackgroundColor
            objMisc.StyleGridMisc(objMisc.gridMisc)
            objMisc.txtMiscMisc.Select()
            objMisc.ShowDialog()
            Dim miscAmt As Double = Val(objMisc.gridMiscTotal.Rows(0).Cells("AMOUNT").Value.ToString)
            txtSAddlCharge_AMT.Text = IIf(miscAmt <> 0, Format(miscAmt, "0.00"), Nothing)
        Else
            objMisc = New frmMiscDia
        End If
    End Sub

    Private Sub txtMAddlCharge_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMAddlCharge_AMT.TextChanged
        CalcMGrossAmt()
    End Sub

    Private Sub txtOAddlCharge_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOAddlCharge_AMT.TextChanged, txtODisc.TextChanged
        CalcOGrossAmt()
    End Sub

    Private Sub txtSAddlCharge_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSAddlCharge_AMT.TextChanged
        CalcSGrossAmt()
    End Sub
    Public Function Clone() As Object Implements System.ICloneable.Clone
        Return MyBase.MemberwiseClone()
    End Function

    Private Sub cmbMIssuedCategory_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMIssuedCategory.Leave
        If cmbMIssuedCategory.Enabled And rbtMetal.Checked Then
            LoadCategoryDetails(cmbMIssuedCategory)
        Else
            LoadCategoryDetails(cmbMCategory)
        End If
    End Sub

    Private Sub txtMMc_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMMc_AMT.TextChanged
        CalcMGrossAmt()
    End Sub
    Private Sub txtOPcs_NUM_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOPcs_NUM.GotFocus
        ''If STOCKVALIDATION_MR Then
        ''    If LoadBalanceStock_MR(cmbOMetal.Text) = False Then
        ''        SendKeys.Send("{TAB}")
        ''    End If
        ''End If
        If DVAACCESS = "Y" Then Getdealerwmc()
    End Sub

    Private Sub Getdealerwmc(Optional ByVal wt As Double = 0)
        Dim dttab As New DataTable
        MCPERGMPC = ""
        WASTPERPC = ""
        If strLockCtrl.Length > 0 Then
            If Not (Array.IndexOf(strLockCtrl, "WP")) >= 0 Then
                Me.txtOWast_WET.Enabled = True : Me.txtOWastPer.Enabled = True : Me.txtOTouchAMT.Enabled = True
            End If
        Else
            Me.txtOWast_WET.Enabled = True : Me.txtOWastPer.Enabled = True : Me.txtOTouchAMT.Enabled = True
        End If
        Dim msubitemid As Integer
        Dim mitemid As Integer
        ''If cmbOSubItem.SelectedValue Is Nothing Then msubitemid = 0 Else msubitemid = Val(cmbOSubItem.SelectedValue.ToString & "")
        ''If cmbOItem.SelectedValue Is Nothing Then mitemid = 0 Else mitemid = Val(cmbOItem.SelectedValue.ToString & "")
        If cmbOSubItem.Text = "" Then msubitemid = 0 Else msubitemid = Val(GetSqlValue(cn, " SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbOSubItem.Text & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbOItem.Text & "')") & "")
        If cmbOItem.Text = "" Then mitemid = 0 Else mitemid = Val(GetSqlValue(cn, " SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbOItem.Text & "'") & "")
        If editflag = False Then
            txtOWastPer.Text = "" : txtOWastPer.Text = ""
            If txtOOrdNo.Text = "" And Val(txtOTouchAMT.Text) = 0 Then txtOTouchAMT.Text = ""
            txtOMc_AMT.Text = "" : txtOMcGrm_AMT.Text = ""
        End If
nextt:
        StrSql = " SELECT * FROM " & cnAdminDb & "..DEALER_WMCTABLE WHERE ACCODE = '" & Maccode & "'"
        'If mitemid <> 0 Then
        StrSql = StrSql & " AND  ITEMID = " & mitemid
        '        If msubitemid <> 0 Then
        StrSql = StrSql & " AND  SUBITEMID = " & msubitemid
        'If wt <> 0 Then        End If
        If wt <> 0 Then
            StrSql += " and " & wt.ToString & " between from_wt and to_wt"
        End If
        dttab = Nothing
        dttab = GetSqlTable(StrSql, cn, tran)
        If dttab.Rows.Count > 0 Then
            With dttab.Rows(0)
                If .Item("calcmode").ToString = "T" Then Me.txtOWast_WET.Enabled = False : Me.txtOWastPer.Enabled = False : Me.txtOTouchAMT.Enabled = True
                If .Item("calcmode").ToString = "W" Then Me.txtOWast_WET.Enabled = True : Me.txtOWastPer.Enabled = True : Me.txtOTouchAMT.Enabled = False
                If .Item("MCCALC").ToString = "W" Then Me.txtOMcGrm_AMT.Enabled = True : Me.txtOMc_AMT.Enabled = D_VA_DATAENABLE
                If .Item("MCCALC").ToString = "P" Then Me.txtOMcGrm_AMT.Enabled = D_VA_DATAENABLE : Me.txtOMc_AMT.Enabled = True
                GRSNETCAL = .Item("grsnet").ToString & ""
                MCPERGMPC = .Item("MCCALC").ToString
                calculationget()
                txtOWastPer.Text = Val(.Item("wastper").ToString) : txtOWast_WET.Text = Val(.Item("wast").ToString)
                txtOTouchAMT.Text = Val(.Item("TOUCH").ToString)
                If Val(.Item("WASTPIE").ToString) <> 0 Then txtOWastPer.Text = Val(.Item("WASTPIE").ToString) : WASTPERPC = "Y" : CalcOWastage()
                txtOMcGrm_AMT.Text = (Val(.Item("MCGRM").ToString))
                txtOalloyper.Text = Val(.Item("ALLOY").ToString)
                txtMAlloyper.Text = Val(.Item("ALLOY").ToString)
                txtOMc_AMT.Text = (Val(.Item("MC").ToString))
                If MCPERGMPC = "P" Then CalcOMc()
                Exit Sub
            End With
        Else
            If wt <> 0 Then wt = 0 : GoTo nextt
            If msubitemid <> 0 Then msubitemid = 0 : GoTo nextt
            If mitemid <> 0 Then
                mitemid = 0
                GoTo nextt
            Else
                If editflag = False Then
                    txtOWastPer.Text = ""
                    txtOWastPer.Text = ""
                    If txtOOrdNo.Text = "" And Val(txtOTouchAMT.Text) = 0 Then txtOTouchAMT.Text = ""
                    txtOMc_AMT.Text = ""
                    txtOMcGrm_AMT.Text = ""
                End If
                Exit Sub
            End If

        End If
    End Sub

    Private Sub txtMLessWt_WET_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMLessWt_WET.TextChanged
        CalcMNetWt()
        CalcMPureWt()
    End Sub

    Private Sub CmbOPurity_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmbOPurity.TextChanged
        CalcONetWt()
        CalcOWastage()
        CalcOMc()
        CalcOPureWt()
        CalcOGrossAmt()
    End Sub

    Private Sub txtSRemark2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSRemark2.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If oEditRowIndex <> -1 Then
                'btnOk.Focus()
                Exit Sub
            End If
            If txtSOrdNo.Text <> "" Then
                If _JobNoEnable = False And (Mid(txtSOrdNo.Text, 1, 1) = "R" Or Mid(txtSOrdNo.Text, 1, 1) = "O") Then
                    If CheckJobNo(txtSOrdNo) = False Then Exit Sub
                    If objOrderInfo.DialogResult <> Windows.Forms.DialogResult.OK Then
                        txtSOrdNo.Select()
                        MsgBox("Invalid Order Selection", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                End If
            End If
            If Val(txtSGrsWt_WET.Text) = 0 And Val(txtSPcs_NUM.Text) = 0 Then
                MsgBox("Weight should not empty", MsgBoxStyle.Information)
                txtSGrsWt_WET.Select()
                Exit Sub
            End If
            If txtStRowIndex.Text = "" Then
                ''Insertion
                Dim ro As DataRow = Nothing
                ro = dtGridStuddedStone.NewRow
                ro("ORDNO") = txtSOrdNo.Text
                ro("METAL") = cmbSMetal.Text
                ro("CATEGORY") = cmbSCategory.Text
                ro("ISSCATEGORY") = cmbSIssuedCategory.Text
                ro("PURITY") = Val(CmbSPurity.Text)
                ro("ITEM") = cmbSItem.Text
                ro("SUBITEM") = cmbSSubItem.Text
                ro("PCS") = Val(txtSPcs_NUM.Text)
                ro("WEIGHT") = Val(txtSGrsWt_WET.Text)
                ro("GRSWT") = Val(txtSGrsWt_WET.Text)
                ro("UNIT") = cmbSUnit.Text
                ro("CALC") = cmbSCalcMode.Text
                ro("RATE") = Val(txtSRate_OWN.Text)
                ro("BOARDRATE") = Val(oBoardRate)
                ro("GROSSAMT") = Val(txtSGrsAmt_AMT.Text)
                ro("VATPER") = Val(txtSVatPer_PER.Text)
                ro("VAT") = Val(txtSVat_AMT.Text)
                ro("AMOUNT") = Val(txtSAmount_AMT.Text)
                ro("REMARK1") = txtSRemark1.Text
                ro("REMARK2") = txtSRemark2.Text
                ro("ORDSTATE_NAME") = cmbSProcess.Text
                ro("ADDCHARGE") = Val(txtSAddlCharge_AMT.Text)
                ro("SEIVE") = cmbSSeive.Text
                ro("STNSNO") = StnSno
                '09April22
                If InsSno <> "" Then
                    ro("RESNO") = InsSno
                Else
                    ro("RESNO") = RESNO
                End If
                ro("ACCODE") = ACCODE
                ro("RFID") = txtRfId.Text.ToString.Trim
                ro("TAGNO") = txtTagNo.Text.ToString.Trim
                ro("SGSTPER") = Val(txtSSgst_WET.Text)
                ro("SGST") = Val(txtSSG_AMT.Text)
                ro("CGSTPER") = Val(txtSCgst_WET.Text)
                ro("CGST") = Val(txtSCG_AMT.Text)
                ro("IGSTPER") = Val(txtSIgst_WET.Text)
                ro("IGST") = Val(txtSIG_AMT.Text)
                ro("TCS") = Val(txtSTCS_AMT.Text)
                ro("WASTAGE") = Val(txtSWast_WET.Text)
                Dim ColorId As String = "0"
                Dim CutId As String = "0"
                Dim ClarityId As String = "0"
                Dim ShapeId As String = "0"
                Dim SetTypeId As String = "0"
                Dim StnHeight As String = "0"
                Dim StnWidth As String = "0"
                Dim StnGrpId As String = ""
                ColorId = objGPack.GetSqlValue("SELECT COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME = '" & ObjDiaDetails.CmbColor.Text & "'", "COLORID", 0)
                CutId = objGPack.GetSqlValue("SELECT CUTID FROM " & cnAdminDb & "..STNCUT WHERE CUTNAME = '" & ObjDiaDetails.CmbCut.Text & "'", "CUTID", 0)
                ClarityId = objGPack.GetSqlValue("SELECT CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME = '" & ObjDiaDetails.CmbClarity.Text & "'", "CLARITYID", 0)
                ShapeId = objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME = '" & ObjDiaDetails.cmbShape.Text & "'", "SHAPEID", 0)
                SetTypeId = objGPack.GetSqlValue("SELECT SETTYPEID FROM " & cnAdminDb & "..STNSETTYPE WHERE SETTYPENAME = '" & ObjDiaDetails.cmbSetType.Text & "'", "SETTYPEID", 0)
                StnHeight = ObjDiaDetails.txtWidth_WET.Text.ToString
                StnWidth = ObjDiaDetails.txtHeight_WET.Text.ToString
                StnGrpId = objGPack.GetSqlValue("SELECT GROUPID FROM " & cnAdminDb & "..STONEGROUP WHERE GROUPNAME = '" & ObjDiaDetails.cmbStnGrp.Text & "'", "GROUPID", 0)
                If ro("CUTID") Is DBNull.Value Then ro("CUTID") = Convert.ToString(CutId)
                If ro("COLORID") Is DBNull.Value Then ro("COLORID") = Convert.ToString(ColorId)
                If ro("CLARITYID") Is DBNull.Value Then ro("CLARITYID") = Convert.ToString(ClarityId)
                If ro("SHAPEID") Is DBNull.Value Then ro("SHAPEID") = Convert.ToString(ShapeId)
                If ro("SETTYPEID") Is DBNull.Value Then ro("SETTYPEID") = Convert.ToString(SetTypeId)
                If ro("HEIGHT") Is DBNull.Value Then ro("HEIGHT") = Convert.ToString(StnHeight)
                If ro("WIDTH") Is DBNull.Value Then ro("WIDTH") = Convert.ToString(StnWidth)
                If ro("STNGRPID") Is DBNull.Value Then ro("STNGRPID") = Convert.ToString(StnGrpId)

                'ro("CUTID").Value = Convert.ToString(CutId)
                'ro("COLORID").Value = Convert.ToString(ColorId)
                'ro("CLARITYID").Value = Convert.ToString(ClarityId)
                'ro("SHAPEID").Value = Convert.ToString(ShapeId)
                'ro("SETTYPEID").Value = Convert.ToString(SetTypeId)
                'ro("HEIGHT").Value = Convert.ToString(StnHeight)
                'ro("WIDTH").Value = Convert.ToString(StnWidth)
                dtGridStuddedStone.Rows.Add(ro)
            Else
                With GridStuddStone.Rows(Val(txtStRowIndex.Text))
                    .Cells("ORDNO").Value = txtSOrdNo.Text
                    .Cells("METAL").Value = cmbSMetal.Text
                    .Cells("CATEGORY").Value = cmbSCategory.Text
                    .Cells("ISSCATEGORY").Value = cmbSIssuedCategory.Text
                    .Cells("PURITY").Value = Val(CmbSPurity.Text)
                    .Cells("ITEM").Value = cmbSItem.Text
                    .Cells("SUBITEM").Value = cmbSSubItem.Text
                    .Cells("PCS").Value = Val(txtSPcs_NUM.Text)
                    .Cells("WEIGHT").Value = Val(txtSGrsWt_WET.Text)
                    .Cells("GRSWT").Value = Val(txtSGrsWt_WET.Text)
                    .Cells("UNIT").Value = cmbSUnit.Text
                    .Cells("CALC").Value = cmbSCalcMode.Text
                    .Cells("RATE").Value = Val(txtSRate_OWN.Text)
                    .Cells("BOARDRATE").Value = Val(oBoardRate)
                    .Cells("GROSSAMT").Value = Val(txtSGrsAmt_AMT.Text)
                    .Cells("VATPER").Value = Val(txtSVatPer_PER.Text)
                    .Cells("VAT").Value = Val(txtSVat_AMT.Text)
                    .Cells("SGSTPER").Value = Val(txtSSgst_WET.Text)
                    .Cells("SGST").Value = Val(txtSSG_AMT.Text)
                    .Cells("CGSTPER").Value = Val(txtSCgst_WET.Text)
                    .Cells("CGST").Value = Val(txtSCG_AMT.Text)
                    .Cells("IGSTPER").Value = Val(txtSIgst_WET.Text)
                    .Cells("IGST").Value = Val(txtSIG_AMT.Text)
                    .Cells("AMOUNT").Value = Val(txtSAmount_AMT.Text)
                    .Cells("REMARK1").Value = txtSRemark1.Text
                    .Cells("REMARK2").Value = txtSRemark2.Text
                    .Cells("ORDSTATE_NAME").Value = cmbSProcess.Text
                    .Cells("ADDCHARGE").Value = Val(txtSAddlCharge_AMT.Text)
                    .Cells("SEIVE").Value = cmbSSeive.Text
                    .Cells("STNSNO").Value = StnSno
                    .Cells("RESNO").Value = RESNO
                    .Cells("ACCODE").Value = ACCODE
                    .Cells("RFID").Value = txtRfId.Text.ToString.Trim
                    .Cells("TAGNO").Value = txtTagNo.Text.ToString.Trim
                    .Cells("STNGRPID").Value = ObjDiaDetails.cmbStnGrp.Text.Trim
                    .Cells("TCS").Value = Val(txtSTCS_AMT.Text)
                    .Cells("WASTAGE").Value = Val(txtSWast_WET.Text)
                End With
            End If
            dtGridStuddedStone.AcceptChanges()
            txtSOrdNo.Text = ""
            Remark1 = txtSRemark1.Text
            Remark2 = txtSRemark2.Text
            GridStuddStone.CurrentCell = GridStuddStone.Rows(GridStuddStone.RowCount - 1).Cells("ITEM")
            StyleGridStone(GridStuddStone)
            txtSPcs_NUM.Clear()
            txtSGrsWt_WET.Clear()
            txtSGrsAmt_AMT.Clear()
            txtSAmount_AMT.Clear()
            txtSVat_AMT.Clear()
            txtSRate_OWN.Clear()
            txtSVatPer_PER.Clear()
            txtRfId.Clear()
            txtSAddlCharge_AMT.Clear()
            StnSno = ""
            ACCODE = ""
            txtTagNo.Text = ""
            txtSRemark1.Clear()
            txtSRemark2.Clear()
            MultiStone = True
            lblSOrdNo.Focus()
            txtStnGrpName.Text = ""
            txtSTCS_AMT.Clear()
            txtSWast_WET.Clear()
            'txtSOrdNo.Focus()
            'txtSOrdNo.SelectAll()
        End If
    End Sub

    Private Sub LoadSmithAppDetails()
        Dim JObno As String = ""
        Dim Metal As String = ""
        If rbtOrnament.Checked Then
            JObno = txtOOrdNo.Text
            Metal = cmbOMetal.Text
        ElseIf rbtMetal.Checked Then
            JObno = txtMOrdNo.Text
            Metal = cmbMMetal.Text
        ElseIf rbtStone.Checked Then
            JObno = txtSOrdNo.Text
            Metal = cmbSMetal.Text
        End If
        Dim mStrSql As String = ""
        If oMaterial = Material.Receipt Then
            mStrSql = vbCrLf + " SELECT TRANNO,TRANDATE,METAL,CATEGORY,ITEM,SUBITEM,SUM(PCS)PCS"
            mStrSql += vbCrLf + " ,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT"
            mStrSql += vbCrLf + " ,(SELECT TOP 1 WASTPER FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)WASTPER"
            mStrSql += vbCrLf + " ,(SELECT TOP 1 WASTAGE FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)WASTAGE"
            mStrSql += vbCrLf + " ,SNO AS ORSNO,ISNULL(TOUCH,0)TOUCH FROM"
            mStrSql += vbCrLf + " ("
            mStrSql += vbCrLf + " SELECT SNO,M.METALNAME AS METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
            mStrSql += vbCrLf + " ,SUM(ISNULL(PCS,0)) PCS,SUM(ISNULL(I.GRSWT,0)) GRSWT "
            mStrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) NETWT"
            mStrSql += vbCrLf + " ,TRANNO,CONVERT(VARCHAR(15),TRANDATE,105)TRANDATE,ISNULL(TOUCH,0)TOUCH "
            mStrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT I"
            mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
            mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = I.ITEMID"
            mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = I.SUBITEMID"
            mStrSql += vbCrLf + " WHERE I.TRANTYPE='RAP'"
            If SelectedOrder <> Nothing Then
                mStrSql += vbCrLf + "  AND I.SNO NOT IN (" & SelectedOrder & ")"
            End If
            mStrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
            mStrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
            If Metal <> "" Then mStrSql += vbCrLf + " AND M.METALNAME='" & Metal & "'"
            mStrSql += vbCrLf + " GROUP BY M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME,SNO,TRANNO,TRANDATE,TOUCH "
            mStrSql += vbCrLf + " UNION ALL"
            mStrSql += vbCrLf + " SELECT REFNO AS SNO,M.METALNAME AS METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
            mStrSql += vbCrLf + " ,-1*SUM(ISNULL(PCS,0)) PCS,-1*SUM(ISNULL(I.GRSWT,0)) GRSWT "
            mStrSql += vbCrLf + " ,-1*SUM(ISNULL(NETWT,0)) NETWT"
            mStrSql += vbCrLf + " ,(SELECT TOP 1 TRANNO FROM " & cnStockDb & "..RECEIPT WHERE SNO=I.REFNO)TRANNO"
            mStrSql += vbCrLf + " ,(SELECT TOP 1 CONVERT(VARCHAR(15),TRANDATE,105) FROM " & cnStockDb & "..RECEIPT WHERE SNO=I.REFNO)TRANDATE"
            mStrSql += vbCrLf + " ,ISNULL((SELECT TOP 1 TOUCH FROM " & cnStockDb & "..RECEIPT WHERE SNO=I.REFNO),0) TOUCH"
            mStrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
            mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = I.ITEMID"
            mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = I.SUBITEMID"
            mStrSql += vbCrLf + " WHERE I.TRANTYPE='IAP'"
            If SelectedOrder <> Nothing Then
                mStrSql += vbCrLf + "  AND I.REFNO NOT IN (" & SelectedOrder & ")"
            End If
            mStrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
            mStrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
            If Metal <> "" Then mStrSql += vbCrLf + " AND M.METALNAME='" & Metal & "'"
            mStrSql += vbCrLf + " GROUP BY M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME,REFNO "
            mStrSql += vbCrLf + " )X GROUP BY TRANNO,TRANDATE,METAL,ITEM,CATEGORY,SUBITEM,SNO,X.TOUCH HAVING SUM(GRSWT)>0 "
        End If
        Dim dtJob As New DataTable
        Cmd = New OleDbCommand(mStrSql, cn)
        Da = New OleDbDataAdapter(Cmd)
        Da.Fill(dtJob)
showjobs:
        Dim JnoRow As DataRow
        If dtJob.Rows.Count > 0 Then
            JnoRow = BrighttechPack.SearchDialog.Show_R("Find ", mStrSql, cn, , 1, , , , , , True)
        End If
        If Not JnoRow Is Nothing Then
            StrSql = vbCrLf + "  SELECT "
            StrSql += vbCrLf + "  IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
            StrSql += vbCrLf + "  ,ST.STNPCS AS PCS,ST.STNWT AS WEIGHT"
            StrSql += vbCrLf + "  ,ST.STONEUNIT AS UNIT,ST.CALCMODE AS CALC,ST.STNRATE AS RATE,ST.STNAMT AS AMOUNT"
            StrSql += vbCrLf + "  ,IM.DIASTONE AS METALID,ISNULL(SEIVE,'') AS SEIVE"
            StrSql += vbCrLf + "  ,CASE WHEN ST.TRANTYPE='RPU' THEN 'PURCHASE' "
            StrSql += vbCrLf + "  WHEN ST.TRANTYPE='RAP' THEN 'APPROVAL' "
            StrSql += vbCrLf + "  ELSE 'RECEIPT' "
            StrSql += vbCrLf + "  END STNTYPE,SNO AS ISSSNO"
            StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPTSTONE AS ST"
            StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
            StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = ST.STNITEMID AND SM.SUBITEMID = ST.STNSUBITEMID"
            StrSql += vbCrLf + "  WHERE ST.ISSSNO = '" & JnoRow.Item("ORSNO").ToString & "'"
            Dim dtStone As New DataTable
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dtStone)
            objStone.dtGridStone.Clear()
            For Each RoStn As DataRow In dtStone.Rows
                Dim RoStnIm As DataRow = objStone.dtGridStone.NewRow
                For Each Col As DataColumn In dtStone.Columns
                    RoStnIm.Item(Col.ColumnName) = RoStn.Item(Col.ColumnName)
                Next
                objStone.dtGridStone.Rows.Add(RoStnIm)
            Next
            objStone.dtGridStone.AcceptChanges()
            objStone.CalcStoneWtAmount()

            If rbtOrnament.Checked Then
                Dim Assignval As Boolean = False
                If txtOOrdNo.Focused Then cmbOProcess.Focus()
                txtOOrdNo.Text = JObno
                ORSNO = JnoRow.Item("ORSNO").ToString
                If Assignval Then
                    txtOPcs_NUM.Text = Val(dtJob.Compute("SUM(PCS)", Nothing).ToString) 'Val(JnoRow.Item("PCS").ToString)
                    txtOGrsWt_WET.Text = Val(dtJob.Compute("SUM(GRSWT)", Nothing).ToString) 'Val(JnoRow.Item("GRSWT").ToString)
                    txtONetWt_WET.Text = Val(dtJob.Compute("SUM(NETWT)", Nothing).ToString) 'Val(JnoRow.Item("NETWT").ToString)
                Else
                    txtOPcs_NUM.Text = Val(JnoRow.Item("PCS").ToString)
                    txtOGrsWt_WET.Text = Val(JnoRow.Item("GRSWT").ToString)
                    txtONetWt_WET.Text = Val(JnoRow.Item("NETWT").ToString)
                End If
                txtOTouchAMT.Text = Val(JnoRow.Item("TOUCH").ToString)

                cmbOCategory.Text = JnoRow.Item("CATEGORY").ToString
                cmbOItem.Text = JnoRow.Item("ITEM").ToString
                cmbOSubItem.Text = JnoRow.Item("SUBITEM").ToString
                txtOWastPer.Text = JnoRow.Item("WASTPER").ToString
                txtOWast_WET.Text = JnoRow.Item("WASTAGE").ToString
            ElseIf rbtMetal.Checked Then
                Dim Assignval As Boolean = False
                If txtMOrdNo.Focused Then cmbMProcess.Focus()
                txtMOrdNo.Text = JObno
                ORSNO = JnoRow.Item("ORSNO").ToString
                If Assignval Then
                    txtMPcs_NUM.Text = Val(dtJob.Compute("SUM(PCS)", Nothing).ToString)  'Val(JnoRow.Item("PCS").ToString)
                    txtMGrsWt_WET.Text = Val(dtJob.Compute("SUM(GRSWT)", Nothing).ToString) 'Val(JnoRow.Item("GRSWT").ToString)
                    txtMNetWt_WET.Text = Val(dtJob.Compute("SUM(NETWT)", Nothing).ToString) 'Val(JnoRow.Item("NETWT").ToString)
                Else
                    txtMPcs_NUM.Text = Val(JnoRow.Item("PCS").ToString)
                    txtMGrsWt_WET.Text = Val(JnoRow.Item("GRSWT").ToString)
                    txtMNetWt_WET.Text = Val(JnoRow.Item("NETWT").ToString)
                End If
                txtMTouchAMT.Text = Val(JnoRow.Item("TOUCH").ToString)
                cmbMCategory.Text = JnoRow.Item("CATEGORY").ToString
            ElseIf rbtStone.Checked Then
                Dim Assignval As Boolean = False
                txtSOrdNo.Text = JObno
                ORSNO = JnoRow.Item("ORSNO").ToString
                If Assignval Then
                    txtSPcs_NUM.Text = Val(dtJob.Compute("SUM(PCS)", Nothing).ToString) 'Val(JnoRow.Item("PCS").ToString)
                    txtSGrsWt_WET.Text = Val(dtJob.Compute("SUM(GRSWT)", Nothing).ToString) 'Val(JnoRow.Item("GRSWT").ToString)
                Else
                    txtSPcs_NUM.Text = Val(JnoRow.Item("PCS").ToString)
                    txtSGrsWt_WET.Text = Val(JnoRow.Item("GRSWT").ToString)
                End If
                cmbSCategory.Text = JnoRow.Item("CATEGORY").ToString
            End If
        Else
            If rbtOrnament.Checked Then
                txtOPcs_NUM.Clear()
                txtOGrsWt_WET.Clear()
                txtONetWt_WET.Clear()
                txtOTouchAMT.Clear()
            ElseIf rbtMetal.Checked Then
                txtMPcs_NUM.Clear()
                txtMGrsWt_WET.Clear()
                txtMNetWt_WET.Clear()
                txtMTouchAMT.Clear()
            ElseIf rbtStone.Checked Then
                txtSPcs_NUM.Clear()
                txtSGrsWt_WET.Clear()
            End If
        End If
    End Sub

    Private Sub LoadSmithAppDetails_21_SEP_2021()
        Dim JObno As String = ""
        Dim Metal As String = ""
        If rbtOrnament.Checked Then
            JObno = txtOOrdNo.Text
            Metal = cmbOMetal.Text
        ElseIf rbtMetal.Checked Then
            JObno = txtMOrdNo.Text
            Metal = cmbMMetal.Text
        ElseIf rbtStone.Checked Then
            JObno = txtSOrdNo.Text
            Metal = cmbSMetal.Text
        End If
        Dim mStrSql As String = ""
        If oMaterial = Material.Receipt Then
            mStrSql = vbCrLf + " SELECT TRANNO,TRANDATE,METAL,CATEGORY,ITEM,SUBITEM,SUM(PCS)PCS"
            mStrSql += vbCrLf + " ,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT"
            mStrSql += vbCrLf + " ,(SELECT TOP 1 WASTPER FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)WASTPER"
            mStrSql += vbCrLf + " ,(SELECT TOP 1 WASTAGE FROM " & cnStockDb & "..RECEIPT WHERE SNO=X.SNO)WASTAGE"
            mStrSql += vbCrLf + " ,SNO AS ORSNO FROM"
            mStrSql += vbCrLf + " ("
            mStrSql += vbCrLf + " SELECT SNO,M.METALNAME AS METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
            mStrSql += vbCrLf + " ,SUM(ISNULL(PCS,0)) PCS,SUM(ISNULL(I.GRSWT,0)) GRSWT "
            mStrSql += vbCrLf + " ,SUM(ISNULL(NETWT,0)) NETWT"
            mStrSql += vbCrLf + " ,TRANNO,CONVERT(VARCHAR(15),TRANDATE,105)TRANDATE"
            mStrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT I"
            mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
            mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = I.ITEMID"
            mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = I.SUBITEMID"
            mStrSql += vbCrLf + " WHERE I.TRANTYPE='RAP'"
            If SelectedOrder <> Nothing Then
                mStrSql += vbCrLf + "  AND I.SNO NOT IN (" & SelectedOrder & ")"
            End If
            mStrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
            mStrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
            If Metal <> "" Then mStrSql += vbCrLf + " AND M.METALNAME='" & Metal & "'"
            mStrSql += vbCrLf + " GROUP BY M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME,SNO,TRANNO,TRANDATE "
            mStrSql += vbCrLf + " UNION ALL"
            mStrSql += vbCrLf + " SELECT REFNO AS SNO,M.METALNAME AS METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
            mStrSql += vbCrLf + " ,-1*SUM(ISNULL(PCS,0)) PCS,-1*SUM(ISNULL(I.GRSWT,0)) GRSWT "
            mStrSql += vbCrLf + " ,-1*SUM(ISNULL(NETWT,0)) NETWT"
            mStrSql += vbCrLf + " ,(SELECT TOP 1 TRANNO FROM " & cnStockDb & "..RECEIPT WHERE SNO=I.REFNO)TRANNO"
            mStrSql += vbCrLf + " ,(SELECT TOP 1 CONVERT(VARCHAR(15),TRANDATE,105) FROM " & cnStockDb & "..RECEIPT WHERE SNO=I.REFNO)TRANDATE"
            mStrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
            mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = I.ITEMID"
            mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = I.SUBITEMID"
            mStrSql += vbCrLf + " WHERE I.TRANTYPE='IAP'"
            If SelectedOrder <> Nothing Then
                mStrSql += vbCrLf + "  AND I.REFNO NOT IN (" & SelectedOrder & ")"
            End If
            mStrSql += vbCrLf + " AND ACCODE='" & Maccode & "'"
            mStrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
            If Metal <> "" Then mStrSql += vbCrLf + " AND M.METALNAME='" & Metal & "'"
            mStrSql += vbCrLf + " GROUP BY M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME,REFNO "
            mStrSql += vbCrLf + " )X GROUP BY TRANNO,TRANDATE,METAL,ITEM,CATEGORY,SUBITEM,SNO HAVING SUM(GRSWT)>0 "
        End If
        Dim dtJob As New DataTable
        Cmd = New OleDbCommand(mStrSql, cn)
        Da = New OleDbDataAdapter(Cmd)
        Da.Fill(dtJob)
showjobs:
        Dim JnoRow As DataRow
        If dtJob.Rows.Count > 0 Then
            JnoRow = BrighttechPack.SearchDialog.Show_R("Find ", mStrSql, cn, , 1, , , , , , True)
        End If
        If Not JnoRow Is Nothing Then
            StrSql = vbCrLf + "  SELECT "
            StrSql += vbCrLf + "  IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM"
            StrSql += vbCrLf + "  ,ST.STNPCS AS PCS,ST.STNWT AS WEIGHT"
            StrSql += vbCrLf + "  ,ST.STONEUNIT AS UNIT,ST.CALCMODE AS CALC,ST.STNRATE AS RATE,ST.STNAMT AS AMOUNT"
            StrSql += vbCrLf + "  ,IM.DIASTONE AS METALID,ISNULL(SEIVE,'') AS SEIVE"
            StrSql += vbCrLf + "  ,CASE WHEN ST.TRANTYPE='RPU' THEN 'PURCHASE' "
            StrSql += vbCrLf + "  WHEN ST.TRANTYPE='RAP' THEN 'APPROVAL' "
            StrSql += vbCrLf + "  ELSE 'RECEIPT' "
            StrSql += vbCrLf + "  END STNTYPE,SNO AS ISSSNO"
            StrSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPTSTONE AS ST"
            StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = ST.STNITEMID"
            StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = ST.STNITEMID AND SM.SUBITEMID = ST.STNSUBITEMID"
            StrSql += vbCrLf + "  WHERE ST.ISSSNO = '" & JnoRow.Item("ORSNO").ToString & "'"
            Dim dtStone As New DataTable
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dtStone)
            objStone.dtGridStone.Clear()
            For Each RoStn As DataRow In dtStone.Rows
                Dim RoStnIm As DataRow = objStone.dtGridStone.NewRow
                For Each Col As DataColumn In dtStone.Columns
                    RoStnIm.Item(Col.ColumnName) = RoStn.Item(Col.ColumnName)
                Next
                objStone.dtGridStone.Rows.Add(RoStnIm)
            Next
            objStone.dtGridStone.AcceptChanges()
            objStone.CalcStoneWtAmount()
            If rbtOrnament.Checked Then
                Dim Assignval As Boolean = False
                If txtOOrdNo.Focused Then cmbOProcess.Focus()
                txtOOrdNo.Text = JObno
                ORSNO = JnoRow.Item("ORSNO").ToString
                If Assignval Then
                    txtOPcs_NUM.Text = Val(dtJob.Compute("SUM(PCS)", Nothing).ToString) 'Val(JnoRow.Item("PCS").ToString)
                    txtOGrsWt_WET.Text = Val(dtJob.Compute("SUM(GRSWT)", Nothing).ToString) 'Val(JnoRow.Item("GRSWT").ToString)
                    txtONetWt_WET.Text = Val(dtJob.Compute("SUM(NETWT)", Nothing).ToString) 'Val(JnoRow.Item("NETWT").ToString)
                Else
                    txtOPcs_NUM.Text = Val(JnoRow.Item("PCS").ToString)
                    txtOGrsWt_WET.Text = Val(JnoRow.Item("GRSWT").ToString)
                    txtONetWt_WET.Text = Val(JnoRow.Item("NETWT").ToString)
                End If
                'txtOTouch_AMT.Text = Val(JnoRow.Item("TOUCH").ToString)

                cmbOCategory.Text = JnoRow.Item("CATEGORY").ToString
                cmbOItem.Text = JnoRow.Item("ITEM").ToString
                cmbOSubItem.Text = JnoRow.Item("SUBITEM").ToString
                txtOWastPer.Text = JnoRow.Item("WASTPER").ToString
                txtOWast_WET.Text = JnoRow.Item("WASTAGE").ToString
            ElseIf rbtMetal.Checked Then
                Dim Assignval As Boolean = False
                If txtMOrdNo.Focused Then cmbMProcess.Focus()
                txtMOrdNo.Text = JObno
                ORSNO = JnoRow.Item("ORSNO").ToString
                If Assignval Then
                    txtMPcs_NUM.Text = Val(dtJob.Compute("SUM(PCS)", Nothing).ToString)  'Val(JnoRow.Item("PCS").ToString)
                    txtMGrsWt_WET.Text = Val(dtJob.Compute("SUM(GRSWT)", Nothing).ToString) 'Val(JnoRow.Item("GRSWT").ToString)
                    txtMNetWt_WET.Text = Val(dtJob.Compute("SUM(NETWT)", Nothing).ToString) 'Val(JnoRow.Item("NETWT").ToString)
                Else
                    txtMPcs_NUM.Text = Val(JnoRow.Item("PCS").ToString)
                    txtMGrsWt_WET.Text = Val(JnoRow.Item("GRSWT").ToString)
                    txtMNetWt_WET.Text = Val(JnoRow.Item("NETWT").ToString)
                End If
                'txtMTouch_AMT.Text = Val(JnoRow.Item("TOUCH").ToString)
                cmbMCategory.Text = JnoRow.Item("CATEGORY").ToString
            ElseIf rbtStone.Checked Then
                Dim Assignval As Boolean = False
                txtSOrdNo.Text = JObno
                ORSNO = JnoRow.Item("ORSNO").ToString
                If Assignval Then
                    txtSPcs_NUM.Text = Val(dtJob.Compute("SUM(PCS)", Nothing).ToString) 'Val(JnoRow.Item("PCS").ToString)
                    txtSGrsWt_WET.Text = Val(dtJob.Compute("SUM(GRSWT)", Nothing).ToString) 'Val(JnoRow.Item("GRSWT").ToString)
                Else
                    txtSPcs_NUM.Text = Val(JnoRow.Item("PCS").ToString)
                    txtSGrsWt_WET.Text = Val(JnoRow.Item("GRSWT").ToString)
                End If
                cmbSCategory.Text = JnoRow.Item("CATEGORY").ToString
            End If
        Else
            If rbtOrnament.Checked Then
                txtOPcs_NUM.Clear()
                txtOGrsWt_WET.Clear()
                txtONetWt_WET.Clear()
                txtOTouchAMT.Clear()
            ElseIf rbtMetal.Checked Then
                txtMPcs_NUM.Clear()
                txtMGrsWt_WET.Clear()
                txtMNetWt_WET.Clear()
                txtMTouchAMT.Clear()
            ElseIf rbtStone.Checked Then
                txtSPcs_NUM.Clear()
                txtSGrsWt_WET.Clear()
            End If
        End If
    End Sub

    Private Sub LoadSmithStnAppDetails()
        Dim JObno As String = ""
        Dim Metal As String = ""
        If rbtStone.Checked Then
            JObno = txtSOrdNo.Text
            Metal = cmbSMetal.Text
        End If
        Dim mStrSql As String = ""
        If oMaterial = Material.Receipt Then
            mStrSql = vbCrLf + " SELECT TRANNO,TRANDATE,METAL,CATEGORY,ITEM,SUBITEM,SUM(PCS)PCS"
            mStrSql += vbCrLf + " ,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT"
            mStrSql += vbCrLf + " ,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE="
            mStrSql += vbCrLf + " (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO="
            mStrSql += vbCrLf + " (SELECT TOP 1 ISSSNO FROM " & cnStockDb & "..RECEIPTSTONE WHERE SNO=X.SNO)))ACNAME"
            mStrSql += vbCrLf + " ,(SELECT TOP 1 ACCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO="
            mStrSql += vbCrLf + " (SELECT TOP 1 ISSSNO FROM " & cnStockDb & "..RECEIPTSTONE WHERE SNO=X.SNO))ACCODE"
            mStrSql += vbCrLf + " ,SNO AS ORSNO FROM"
            mStrSql += vbCrLf + " ("
            mStrSql += vbCrLf + " SELECT SNO,M.METALNAME AS METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
            mStrSql += vbCrLf + " ,SUM(ISNULL(STNPCS,0)) PCS,SUM(ISNULL(STNWT,0)) GRSWT "
            mStrSql += vbCrLf + " ,SUM(ISNULL(STNWT,0)) NETWT"
            mStrSql += vbCrLf + " ,TRANNO,CONVERT(VARCHAR(15),TRANDATE,105)TRANDATE"
            mStrSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE I"
            mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON C.METALID = M.METALID"
            mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = I.STNITEMID"
            mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = I.STNSUBITEMID"
            mStrSql += vbCrLf + " WHERE I.TRANTYPE='RAP'"
            If SelectedOrder <> Nothing Then
                mStrSql += vbCrLf + "  AND I.SNO NOT IN (" & SelectedOrder & ")"
            End If
            mStrSql += vbCrLf + " AND BATCHNO IN(SELECT BATCHNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(CANCEL,'')<>'Y')"
            If Metal <> "" Then mStrSql += vbCrLf + " AND M.METALNAME='" & Metal & "'"
            mStrSql += vbCrLf + " GROUP BY M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME,SNO,TRANNO,TRANDATE "
            mStrSql += vbCrLf + " UNION ALL"
            mStrSql += vbCrLf + " SELECT REFNO AS SNO,M.METALNAME AS METAL,C.CATNAME CATEGORY,IM.ITEMNAME ITEM,SI.SUBITEMNAME SUBITEM "
            mStrSql += vbCrLf + " ,-1*SUM(ISNULL(PCS,0)) PCS,-1*SUM(ISNULL(I.GRSWT,0)) GRSWT "
            mStrSql += vbCrLf + " ,-1*SUM(ISNULL(NETWT,0)) NETWT"
            mStrSql += vbCrLf + " ,(SELECT TOP 1 TRANNO FROM " & cnStockDb & "..RECEIPTSTONE WHERE SNO=I.REFNO)TRANNO"
            mStrSql += vbCrLf + " ,(SELECT TOP 1 CONVERT(VARCHAR(15),TRANDATE,105) FROM " & cnStockDb & "..RECEIPTSTONE WHERE SNO=I.REFNO)TRANDATE"
            mStrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..METALMAST M ON I.METALID = M.METALID"
            mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = I.ITEMID"
            mStrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = I.SUBITEMID"
            mStrSql += vbCrLf + " WHERE I.TRANTYPE='IAP'"
            If SelectedOrder <> Nothing Then
                mStrSql += vbCrLf + "  AND I.REFNO NOT IN (" & SelectedOrder & ")"
            End If
            mStrSql += vbCrLf + " AND ISNULL(CANCEL,'')<>'Y'"
            If Metal <> "" Then mStrSql += vbCrLf + " AND M.METALNAME='" & Metal & "'"
            mStrSql += vbCrLf + " GROUP BY M.METALNAME,IM.ITEMNAME,SI.SUBITEMNAME,C.CATNAME,REFNO "
            mStrSql += vbCrLf + " )X GROUP BY TRANNO,TRANDATE,METAL,ITEM,CATEGORY,SUBITEM,SNO HAVING SUM(GRSWT)>0 "
        End If
        Dim dtJob As New DataTable
        Cmd = New OleDbCommand(mStrSql, cn)
        Da = New OleDbDataAdapter(Cmd)
        Da.Fill(dtJob)
showjobs:
        Dim JnoRow As DataRow
        If dtJob.Rows.Count > 0 Then
            JnoRow = BrighttechPack.SearchDialog.Show_R("Find ", mStrSql, cn, , 1, , , , , , True)
        End If
        If Not JnoRow Is Nothing Then
            If rbtStone.Checked Then
                txtSOrdNo.Text = JObno
                ORSNO = JnoRow.Item("ORSNO").ToString
                StnSno = JnoRow.Item("ORSNO").ToString
                ACCODE = JnoRow.Item("ACCODE").ToString
                txtSPcs_NUM.Text = Val(JnoRow.Item("PCS").ToString)
                txtSGrsWt_WET.Text = Val(JnoRow.Item("GRSWT").ToString)
                cmbSMetal.Text = JnoRow.Item("METAL").ToString
                cmbSCategory.Text = JnoRow.Item("CATEGORY").ToString
                cmbSItem.Text = JnoRow.Item("ITEM").ToString
                cmbSSubItem.Text = JnoRow.Item("SUBITEM").ToString
            End If
        Else
            If rbtStone.Checked Then
                txtSPcs_NUM.Clear()
                txtSGrsWt_WET.Clear()
            End If
        End If
    End Sub
    Private Sub cmbSUnit_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSUnit.GotFocus
        If oMaterial = Material.Issue Then Exit Sub
        Dim Unit As String = ""
        If cmbSSubItem.Enabled = False Then
            StrSql = "SELECT STONEUNIT FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbSItem.Text & "'"
            Unit = objGPack.GetSqlValue(StrSql, "STONEUNIT", "C")
            If Unit = "C" Then cmbSUnit.Text = "CARAT" Else cmbSUnit.Text = "GRAM"
        Else
            StrSql = "SELECT STONEUNIT FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & cmbSSubItem.Text & "'"
            Unit = objGPack.GetSqlValue(StrSql, "STONEUNIT", "C")
            If Unit = "C" Then cmbSUnit.Text = "CARAT" Else cmbSUnit.Text = "GRAM"
        End If
    End Sub

    Private Sub cmbSUnit_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSUnit.SelectedIndexChanged
        CalcSSeive()
    End Sub

    Private Sub txtMAlloyper_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMAlloyper.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then CalcMalloy()
    End Sub

    Private Sub txtMAlloyper_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMAlloyper.TextChanged
        If Val(txtMAlloyper.Text) = 0 Then
            txtMAlloyper.Clear()
            Exit Sub
        End If
        CalcMalloy()
    End Sub
    Private Sub txtMPcs_NUM_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMPcs_NUM.GotFocus
        'If STOCKVALIDATION_MR Then
        '    LoadBalanceStock_MR(cmbOMetal.Text)
        'End If
        If DVAACCESS = "Y" Then Getdealerwmc()
    End Sub

    Private Sub txtOPcs_NUM_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOPcs_NUM.TextChanged
        If MCPERGMPC = "P" Then lblmcpergm.Text = "Mc/Pie" : CalcOMc() Else lblmcpergm.Text = "Mc/Grm"
        If WASTPERPC = "Y" Then lblOWastPer.Text = "Wastage/Pie" : CalcOWastage() Else lblOWastPer.Text = "Wastage %"
    End Sub

    Private Sub txtOPcs_NUM_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOPcs_NUM.Leave, txtMPcs_NUM.Leave, txtSPcs_NUM.Leave
        ''''''
        Dim view4c As String = ""
        Dim maintain4c As Boolean
        Dim DtStnGrp As New DataTable
        StrSql = "SELECT ISNULL(MAINTAIN4C,'N') FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbSItem.Text & "' AND METALID='D'"
        If objGPack.GetSqlValue(StrSql).ToString.ToUpper = "Y" Then maintain4c = True Else maintain4c = False
        If cmbSSubItem.Text <> "" Then
            StrSql = "SELECT ISNULL(MAINTAIN4C,'N') FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSSubItem.Text & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbSItem.Text & "')"
            If objGPack.GetSqlValue(StrSql).ToString.ToUpper = "Y" Then maintain4c = True Else maintain4c = False
        End If
        If _FourCMaintain And maintain4c And rbtStone.Checked Then
            ObjDiaDetails = New frmDiamondDetails
            ObjDiaDetails.cmbStnGrp.Text = txtStnGrpName.Text
            ObjDiaDetails.SetDefaultValues(txtStnGrpName.Text)
            If cmbSItem.Text <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbSItem.Text & "'", , , )
            If cmbSSubItem.Text <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & cmbSSubItem.Text & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbSItem.Text & "')", , , )
            If Not view4c.Contains("CU") Then ObjDiaDetails.CmbCut.Enabled = False
            If Not view4c.Contains("CO") Then ObjDiaDetails.CmbColor.Enabled = False
            If Not view4c.Contains("CL") Then ObjDiaDetails.CmbClarity.Enabled = False
            If Not view4c.Contains("SH") Then ObjDiaDetails.cmbShape.Enabled = False
            If Not view4c.Contains("SE") Then ObjDiaDetails.cmbSetType.Enabled = False
            If Not view4c.Contains("HE") Then ObjDiaDetails.txtHeight_WET.Enabled = False
            If Not view4c.Contains("WI") Then ObjDiaDetails.txtWidth_WET.Enabled = False

            ObjDiaDetails.BackColor = Color.Lavender
            ObjDiaDetails.StartPosition = FormStartPosition.CenterParent
            ObjDiaDetails.CmbCut.Focus()
            ObjDiaDetails.ShowDialog()
            txtStnGrpName.Text = ObjDiaDetails.cmbStnGrp.Text
            'ElseIf _FourCMaintain And tagEdit And maintain4c Then
            '    ObjDiaDetails = New frmDiamondDetails
            '    ObjDiaDetails.CmbCut.Text = Cut
            '    ObjDiaDetails.CmbColor.Text = Color
            '    ObjDiaDetails.CmbClarity.Text = Clarity
            '    ObjDiaDetails.cmbShape.Text = Shape
            '    ObjDiaDetails.cmbSetType.Text = SetType
            '    ObjDiaDetails.txtWidth_WET.Text = Width
            '    ObjDiaDetails.txtHeight_WET.Text = Height
            '    If cmbSItem.Text <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbSItem.Text & "'", , , )
            '    If cmbSSubItem.Text <> "" Then view4c = objGPack.GetSqlValue("SELECT VIEW4C FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & cmbSSubItem.Text & "' AND ITEMID=(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbSItem.Text & "')", , , )
            '    If Not view4c.Contains("CU") Then ObjDiaDetails.CmbCut.Enabled = False
            '    If Not view4c.Contains("CO") Then ObjDiaDetails.CmbColor.Enabled = False
            '    If Not view4c.Contains("CL") Then ObjDiaDetails.CmbClarity.Enabled = False
            '    If Not view4c.Contains("SH") Then ObjDiaDetails.cmbShape.Enabled = False
            '    If Not view4c.Contains("SE") Then ObjDiaDetails.cmbSetType.Enabled = False
            '    If Not view4c.Contains("HE") Then ObjDiaDetails.txtHeight_WET.Enabled = False
            '    If Not view4c.Contains("WI") Then ObjDiaDetails.txtWidth_WET.Enabled = False
            '    ObjDiaDetails.CmbCut.Focus()
            '    ObjDiaDetails.ShowDialog()
        End If
        ''''''
        If STOCKVALIDATION And oMaterial = Material.Issue And txtTagNo.Text.Trim = "" Then
            If CheckOrdRep() Then Exit Sub
            If CType(sender, Control).Name = "txtSPcs_NUM" Then
                If CheckRateItem() Then Exit Sub
            End If
            If Val(CType(sender, Control).Text) > StkPcs Then
                MsgBox("Pcs Should not Exceeds Receipt Pcs", MsgBoxStyle.Information)
                CType(sender, Control).Text = StkPcs
                CType(sender, Control).Focus()
            End If
        End If
        If STOCKVALIDATION_MR And oMaterial = Material.Receipt And txtTagNo.Text.Trim = "" Then
            If CheckOrdRep() Then Exit Sub
            If CType(sender, Control).Name = "txtSPcs_NUM" Then
                If CheckRateItem() Then Exit Sub
            End If
            'If Val(CType(sender, Control).Text) > StkPcs Then
            '    MsgBox("Pcs Should not Exceeds Issue Pcs", MsgBoxStyle.Information)
            '    CType(sender, Control).Text = StkPcs
            '    CType(sender, Control).Focus()
            'End If
        End If
    End Sub

    Private Sub txtOGrsWt_WET_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOGrsWt_WET.Leave, txtMGrsWt_WET.Leave, txtSGrsWt_WET.Leave

        If STOCKVALIDATION And oMaterial = Material.Issue And txtTagNo.Text.Trim = "" Then
            If CheckOrdRep() Then Exit Sub
            If CType(sender, Control).Name = "txtSGrsWt_WET" And StkGrsWt > 0 Then Exit Sub
            If Val(CType(sender, Control).Text) > StkGrsWt Then
                MsgBox("GrsWt Should not Exceeds Receipt GrsWt", MsgBoxStyle.Information)
                CType(sender, Control).Text = StkGrsWt
                CType(sender, Control).Focus()
            End If
        ElseIf MI_STOCKCHECK And oMaterial = Material.Issue And txtTagNo.Text.Trim = "" Then
            If CheckOrdRep() Then Exit Sub
            'If CType(sender, Control).Name = "txtSGrsWt_WET" And StkGrsWt > 0 Then Exit Sub
            If Val(CType(sender, Control).Text) > StkGrsWt And Val(CType(sender, Control).Text) <> 0 Then
                MsgBox("GrsWt Should not Exceeds balance Weight", MsgBoxStyle.Information)
                CType(sender, Control).Text = 0
                CType(sender, Control).Focus()
                Exit Sub
            End If
        ElseIf STOCKVALIDATION_MR And oMaterial = Material.Receipt And txtTagNo.Text.Trim = "" Then
            If oTransactionType <> "RECEIPT" Then Exit Sub
            If txtSOrdNo.Text = "" And Val(CType(sender, Control).Text) <> 0 And rbtStone.Checked = True Then
                If MsgBox("Do You Want To proceed Without Issue no ?", vbYesNo + vbQuestion + vbDefaultButton2) = MsgBoxResult.Yes Then
                    Exit Sub
                End If
            End If
            If CheckOrdRep() Then Exit Sub
            If CType(sender, Control).Name = "txtSGrsWt_WET" And StkGrsWt > 0 Then Exit Sub
            If Val(CType(sender, Control).Text) > StkGrsWt Then
                MsgBox("GrsWt Should not Exceeds Issue GrsWt", MsgBoxStyle.Information)
                CType(sender, Control).Text = StkGrsWt
                CType(sender, Control).Focus()
            End If
        Else
            If Val(txtOGrsWt_WET.Text) = 0 And Val(txtOPcs_NUM.Text) = 0 And rbtOrnament.Checked = True Then
                'MsgBox("Grs. Weight should not empty", MsgBoxStyle.Information)
                txtOGrsWt_WET.Focus()
                Exit Sub
            End If
            If Val(txtMGrsWt_WET.Text) = 0 And Val(txtMPcs_NUM.Text) = 0 And rbtMetal.Checked = True Then
                'MsgBox("Grs. Weight should not empty", MsgBoxStyle.Information)
                txtMGrsWt_WET.Focus()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub cmbSUnit_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSUnit.Leave
        If STOCKVALIDATION And oMaterial = Material.Issue And txtTagNo.Text.Trim = "" Then
            If StkGrsWt > 0 And StkUnit <> "" And ExFlag = False Then
                If Mid(cmbSUnit.Text, 1, 1) = StkUnit Then
                    If Val(txtSGrsWt_WET.Text) > StkGrsWt Then
                        MsgBox("GrsWt Should not Exceeds Receipt GrsWt", MsgBoxStyle.Information)
                        ExFlag = True
                        txtSGrsWt_WET.Text = StkGrsWt
                        txtSGrsWt_WET.Focus()
                    End If
                Else
                    If Mid(cmbSUnit.Text, 1, 1) = "G" And StkUnit = "C" Then
                        If Val(txtSGrsWt_WET.Text) > (StkGrsWt / 5) Then
                            MsgBox("GrsWt Should not Exceeds Receipt GrsWt", MsgBoxStyle.Information)
                            ExFlag = True
                            txtSGrsWt_WET.Text = StkGrsWt
                            txtSGrsWt_WET.Select()
                        End If
                    ElseIf Mid(cmbSUnit.Text, 1, 1) = "C" And StkUnit = "G" Then
                        If Val(txtSGrsWt_WET.Text) > (StkGrsWt * 5) Then
                            MsgBox("GrsWt Should not Exceeds Receipt GrsWt", MsgBoxStyle.Information)
                            ExFlag = True
                            txtSGrsWt_WET.Text = StkGrsWt
                            txtSGrsWt_WET.Select()
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Function CheckOrdRep()
        If txtOOrdNo.Text.StartsWith("O") Then Return True
        If txtOOrdNo.Text.StartsWith("R") Then Return True
        If txtMOrdNo.Text.StartsWith("O") Then Return True
        If txtMOrdNo.Text.StartsWith("R") Then Return True
        If txtSOrdNo.Text.StartsWith("O") Then Return True
        If txtSOrdNo.Text.StartsWith("R") Then Return True
        Return False
    End Function
    Function CheckRateItem()
        Dim CalcType As String = ""
        If cmbSSubItem.Enabled = False Then
            StrSql = "SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & cmbSItem.Text & "'"
            CalcType = objGPack.GetSqlValue(StrSql, "CALTYPE", "W")
            If CalcType = "R" Then Return False Else Return True
        Else
            StrSql = "SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & cmbSSubItem.Text & "'"
            CalcType = objGPack.GetSqlValue(StrSql, "CALTYPE", "W")
            If CalcType = "R" Then Return False Else Return True
        End If
    End Function

    Private Sub txtTagNo_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTagNo.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Val(txtItemId.Text) = 0 Then Exit Sub
            If txtTagNo.Text = "" Then Exit Sub
            If editflag Then Exit Sub
            Dim dtTemp As New DataTable
            Dim dtTag As New DataTable
            Dim _Catname As String = ""
            StrSql = "Select TAGKEY FROM " & cnAdminDb & "..ITEMTAG WHERE ITEMID = '" & Val(txtItemId.Text) & "' AND TAGNO = '" & txtTagNo.Text & "'"
            Dim _SearchTagKey As String = objGPack.GetSqlValue(StrSql, "TAGKEY", "")
            If _SearchTagKey.ToString = "" Then
                MsgBox("Invalid Itemid and Tagno..." & vbCrLf & "Record not found...", MsgBoxStyle.Information)
                Exit Sub
            End If

            StrSql = "  SELECT M.METALNAME,C.CATNAME,CONVERT(NUMERIC(15,2),P.PURITY)PURITY,I.ITEMNAME,S.SUBITEMNAME,T.PCS,TM.GRSWT,"
            StrSql += vbCrLf + "  CASE WHEN T.LESSWT > 0 THEN ISNULL(TM.GRSWT,0)-ISNULL(T.LESSWT,0) ELSE TM.GRSWT END NETWT,T.LESSWT,T.GRSNET,PT.PURTOUCH,TM.SNO TAGESNO "
            StrSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAGMETAL AS TM"
            StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMTAG AS T ON T.SNO=TM.TAGSNO"
            StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID=T.ITEMID"
            StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS S ON S.SUBITEMID=T.SUBITEMID"
            StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..PURITEMTAG AS PT ON PT.TAGNO=T.TAGNO AND PT.ITEMID=T.ITEMID"
            StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE=TM.CATCODE"
            StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..METALMAST AS M ON M.METALID=C.METALID"
            StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..PURITYMAST AS P ON P.PURITYID=C.PURITYID"
            ''StrSql += vbCrLf + "  WHERE T.TAGNO = '" & txtTagNo.Text.ToString.Trim & "' AND T.ISSDATE IS NULL AND TM.ISSDATE IS NULL"
            ''StrSql += vbCrLf + "  WHERE T.TAGKEY = '" & txtTagNo.Text.ToString.Trim & "' AND T.ISSDATE IS NULL AND TM.ISSDATE IS NULL"
            StrSql += vbCrLf + "  WHERE T.TAGKEY = '" & _SearchTagKey.ToString & "' AND T.ISSDATE IS NULL AND TM.ISSDATE IS NULL"
            StrSql += vbCrLf + "  AND T.COMPANYID='" & strCompanyId & "'"
            StrSql += vbCrLf + "  AND ISNULL(T.COSTID,'')='" & cnCostId & "'"
            StrSql += vbCrLf + "  AND ISNULL(T.APPROVAL,'') <> 'A'"
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dtTemp)
            If dtTemp.Rows.Count > 0 And MetalBasedStone Then
                _Catname = BrighttechPack.SearchDialog.Show("MultiMetal Selection", dtTemp, cn, 1, 1)
                If _Catname <> "" Then
                    For cnt As Integer = 0 To 10
                        If arrTagNos(cnt, 0) = txtTagNo.Text.ToString.Trim And arrTagNos(cnt, 1) = _Catname Then
                            MsgBox("TagNo Already Loaded...", MsgBoxStyle.Information)
                            Exit Sub
                        End If
                    Next
                    Dim _temptagno1 As String = objGPack.GetSqlValue("SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY = '" & _SearchTagKey.ToString & "'", "TAGNO", "")
                    For cnt As Integer = 0 To 10
                        If arrTagNos(cnt, 0) = _temptagno1.ToString.Trim And arrTagNos(cnt, 1) = _Catname Then
                            MsgBox("TagNo Already Loaded...", MsgBoxStyle.Information)
                            Exit Sub
                        End If
                    Next
                    dtTag = New DataTable
                    dtTag = dtTemp.Clone
                    For Each ro As DataRow In dtTemp.Rows
                        If _Catname = ro.Item("CATNAME").ToString Then
                            dtTag.ImportRow(ro)
                            Exit For
                        End If
                    Next
                End If
            Else
                Dim _temptagno1 As String = objGPack.GetSqlValue("SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY = '" & _SearchTagKey.ToString & "'", "TAGNO", "")
                For Each T As String In ListTagNos
                    If Trim(T) = txtTagNo.Text.ToString.Trim Then
                        MsgBox("TagNo Already Loaded...", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                Next
                For Each T As String In ListTagNos
                    If Trim(T) = _temptagno1.ToString Then
                        MsgBox("TagNo Already Loaded...", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                Next
                StrSql = "  SELECT M.METALNAME,C.CATNAME,CONVERT(NUMERIC(15,2),P.PURITY)PURITY,I.ITEMNAME,S.SUBITEMNAME,T.PCS,T.GRSWT,"
                StrSql += vbCrLf + "  T.NETWT,T.LESSWT,T.GRSNET,PT.PURTOUCH,'' TAGESNO "
                StrSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAG AS T"
                StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID=T.ITEMID"
                StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS S ON S.SUBITEMID=T.SUBITEMID"
                StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..PURITEMTAG AS PT ON PT.TAGNO=T.TAGNO AND PT.ITEMID=T.ITEMID"
                StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..METALMAST AS M ON M.METALID=I.METALID"
                If NeedItemType_accpost And rbtOrnament.Checked Then
                    StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMTYPE AS IT ON IT.ITEMTYPEID=T.ITEMTYPEID"
                    StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE=IT.CATCODE"
                Else
                    StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..ITEMTYPE AS IT ON IT.ITEMTYPEID=T.ITEMTYPEID"
                    StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS C ON C.CATCODE=I.CATCODE"
                End If
                StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..PURITYMAST AS P ON P.PURITYID=C.PURITYID"
                'StrSql += vbCrLf + "  WHERE T.TAGNO = '" & txtTagNo.Text.ToString.Trim & "' AND ISSDATE IS NULL"
                StrSql += vbCrLf + "  WHERE T.TAGKEY = '" & _SearchTagKey.ToString & "' AND ISSDATE IS NULL"
                StrSql += vbCrLf + "  AND T.COMPANYID='" & strCompanyId & "'"
                StrSql += vbCrLf + "  AND ISNULL(T.COSTID,'')='" & cnCostId & "'"
                StrSql += vbCrLf + "  AND ISNULL(T.APPROVAL,'') <> 'A'"
                Cmd = New OleDbCommand(StrSql, cn)
                Da = New OleDbDataAdapter(Cmd)
                dtTag = New DataTable
                Da.Fill(dtTag)
            End If
            If dtTag.Rows.Count > 0 Then
                If rbtStone.Checked Then
                    LoadMetal(cmbSMetal)
                    cmbSMetal.Text = dtTag.Rows(0)("METALNAME").ToString
                    cmbSItem.Text = ""
                    cmbSCategory.Text = dtTag.Rows(0)("CATNAME").ToString
                    cmbSItem.Text = dtTag.Rows(0)("ITEMNAME").ToString
                    cmbSSubItem.Text = dtTag.Rows(0)("SUBITEMNAME").ToString
                    txtSPcs_NUM.Text = dtTag.Rows(0)("PCS").ToString
                    txtSGrsWt_WET.Text = dtTag.Rows(0)("GRSWT").ToString
                ElseIf rbtOrnament.Checked Then
                    LoadMetal(cmbOMetal)
                    cmbOMetal.Text = dtTag.Rows(0)("METALNAME").ToString
                    cmbOItem.Text = ""
                    cmbOCategory.Text = dtTag.Rows(0)("CATNAME").ToString
                    CmbOPurity.Text = dtTag.Rows(0)("PURITY").ToString
                    cmbOItem.Text = dtTag.Rows(0)("ITEMNAME").ToString
                    cmbOSubItem.Text = dtTag.Rows(0)("SUBITEMNAME").ToString
                    txtOPcs_NUM.Text = dtTag.Rows(0)("PCS").ToString
                    txtOGrsWt_WET.Text = dtTag.Rows(0)("GRSWT").ToString
                    txtONetWt_WET.Text = dtTag.Rows(0)("NETWT").ToString
                    txtOLessWt_WET.Text = dtTag.Rows(0)("LESSWT").ToString
                    cmbOGrsNet.Text = dtTag.Rows(0)("GRSNET").ToString
                    txtOTouchAMT.Text = dtTag.Rows(0)("PURTOUCH").ToString
                    StrSql = "  SELECT  "
                    StrSql += vbCrLf + "  I.METALID,I.CATCODE,I.ITEMNAME,S.SUBITEMNAME,T.STNPCS,T.STNWT,T.STONEUNIT,T.CALCMODE"
                    If oTransactionType = "PURCHASE RETURN" Then
                        StrSql += vbCrLf + "  ,CASE WHEN ISNULL(PT.PURRATE,0)<>0 THEN  PT.PURRATE ELSE T.STNRATE END AS RATE "
                        StrSql += vbCrLf + "  ,CASE WHEN ISNULL(PT.PURAMT,0)<>0 THEN  PT.PURAMT ELSE T.STNAMT END AS AMT"
                        StrSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAGSTONE AS T "
                        StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..PURITEMTAGSTONE AS PT ON PT.TAGNO=T.TAGNO AND PT.ITEMID=T.ITEMID  AND PT.STNSNO=T.SNO"
                    ElseIf oTransactionType = "ISSUE" Then
                        StrSql += vbCrLf + "  ,0 AS RATE,0 AS AMT"
                        StrSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAGSTONE AS T "
                    Else
                        StrSql += vbCrLf + "  ,T.STNRATE AS RATE,T.STNAMT AS AMT"
                        StrSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAGSTONE AS T "
                    End If
                    StrSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID=T.STNITEMID"
                    StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS S ON S.SUBITEMID=T.STNSUBITEMID"
                    ''StrSql += vbCrLf + "  WHERE TAGNO = '" & txtTagNo.Text.ToString.Trim & "' "
                    StrSql += vbCrLf + "  WHERE T.TAGSNO IN(SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE  TAGKEY = '" & _SearchTagKey.ToString & "' AND ISSDATE IS NULL) "
                    If oTransactionType = "PURCHASE RETURN" Then
                        StrSql += vbCrLf + "  AND T.TAGSNO IN(SELECT TAGSNO FROM " & cnAdminDb & "..ITEMTAGSTONE "
                        'StrSql += vbCrLf + "  WHERE TAGNO = '" & txtTagNo.Text.ToString.Trim & "' "
                        StrSql += vbCrLf + "  WHERE TAGSNO IN(SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE  TAGKEY = '" & _SearchTagKey.ToString & "') "
                        StrSql += vbCrLf + "  AND ISSDATE IS NULL)"
                    End If
                    StrSql += vbCrLf + "  AND T.COMPANYID='" & strCompanyId & "'"
                    If MetalBasedStone And dtTag.Rows(0).Item("TAGESNO").ToString <> "" Then
                        StrSql += vbCrLf + "  AND T.TAGMSNO='" & dtTag.Rows(0).Item("TAGESNO").ToString & "'"
                    End If
                    ''StrSql += vbCrLf + "  AND ISNULL(T.COSTID,'')='" & cnCostId & "'"
                    Cmd = New OleDbCommand(StrSql, cn)
                    Da = New OleDbDataAdapter(Cmd)
                    Dim dtTagStn As DataTable
                    dtTagStn = New DataTable
                    Da.Fill(dtTagStn)
                    Dim Stonetrantype As String = ""
                    Dim Stonelesswt As Double = 0
                    If UCase(oTransactionType) = "ISSUE" Then
                        Stonetrantype = "ISSUE"
                    ElseIf UCase(oTransactionType) = "PURCHASE RETURN" Then
                        Stonetrantype = "PURCHASE RETURN"
                    ElseIf UCase(oTransactionType) = "APPROVAL ISSUE" Then
                        Stonetrantype = "APPROVAL"
                    Else
                        Stonetrantype = "ISSUE"
                    End If
                    If dtTagStn.Rows.Count > 0 Then
                        ''Insertion
                        objStone.dtGridStone.Clear()
                        For i As Integer = 0 To dtTagStn.Rows.Count - 1
                            Dim ro As DataRow
                            ro = objStone.dtGridStone.NewRow
                            ro("STNTYPE") = Stonetrantype.ToString
                            ro("ITEM") = dtTagStn.Rows(i)("ITEMNAME").ToString
                            ro("SUBITEM") = dtTagStn.Rows(i)("SUBITEMNAME").ToString
                            ro("PCS") = IIf(Val(dtTagStn.Rows(i)("STNPCS").ToString) <> 0, Val(dtTagStn.Rows(i)("STNPCS").ToString), DBNull.Value)
                            ro("UNIT") = dtTagStn.Rows(i)("STONEUNIT").ToString
                            ro("CALC") = dtTagStn.Rows(i)("CALCMODE").ToString
                            ro("WEIGHT") = IIf(Val(dtTagStn.Rows(i)("STNWT").ToString) <> 0, Format(Val(dtTagStn.Rows(i)("STNWT").ToString), FormatNumberStyle(DiaRnd)), DBNull.Value)
                            ro("METALID") = dtTagStn.Rows(i)("METALID").ToString
                            ro("RATE") = Val(dtTagStn.Rows(i)("RATE").ToString)
                            ro("AMOUNT") = Val(dtTagStn.Rows(i)("AMT").ToString)
                            ro("SEIVE") = ""
                            ro("OCATCODE") = "" & dtTagStn.Rows(i)("CATCODE").ToString
                            If dtTagStn.Rows(i)("STONEUNIT").ToString = "C" Then
                                Stonelesswt += IIf(Val(dtTagStn.Rows(i)("STNWT").ToString) <> 0, Format(Val(dtTagStn.Rows(i)("STNWT").ToString), FormatNumberStyle(DiaRnd)), 0) / 5
                            Else
                                Stonelesswt += IIf(Val(dtTagStn.Rows(i)("STNWT").ToString) <> 0, Format(Val(dtTagStn.Rows(i)("STNWT").ToString), FormatNumberStyle(DiaRnd)), 0)
                            End If
                            objStone.dtGridStone.Rows.Add(ro)
                            objStone.dtGridStone.AcceptChanges()
                        Next
                    End If
                    If Val(Stonelesswt) >= 0 Then
                        txtOLessWt_WET.Text = Format(Val(Stonelesswt), "0.000")
                    End If
                End If

                Dim _temptagno As String = objGPack.GetSqlValue("SELECT TAGNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGKEY = '" & _SearchTagKey.ToString & "'", "TAGNO", "")
                If _temptagno.ToString <> "" Then
                    txtTagNo.Text = _temptagno.ToString
                    If rbtOrnament.Checked Then
                        Dim _tempitem As String = cmbOItem.Text
                        Dim _tempsubitem As String = cmbOSubItem.Text
                        Dim _dtTempitem As DataTable
                        _dtTempitem = New DataTable
                        If _tempitem.ToString <> "" Then
                            StrSql = "SELECT ITEMID,ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & _tempitem.ToString & "'"
                            _dtTempitem = New DataTable
                            Da = New OleDbDataAdapter(StrSql, cn)
                            Da.Fill(_dtTempitem)
                            If _dtTempitem.Rows.Count > 0 Then
                                cmbOItem.DataSource = Nothing
                                cmbOItem.DataSource = _dtTempitem
                                cmbOItem.ValueMember = "ITEMID"
                                cmbOItem.DisplayMember = "ITEMNAME"
                                cmbOItem.AutoCompleteMode = AutoCompleteMode.SuggestAppend
                                cmbOItem.AutoCompleteSource = AutoCompleteSource.ListItems
                            End If
                        End If
                        If _tempsubitem.ToString <> "" Then
                            StrSql = " SELECT SUBITEMID,SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & _tempsubitem.ToString & "' AND"
                            StrSql += " ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & _tempitem.ToString & "')"
                            _dtTempitem = New DataTable
                            Da = New OleDbDataAdapter(StrSql, cn)
                            Da.Fill(_dtTempitem)
                            If _dtTempitem.Rows.Count > 0 Then
                                cmbOSubItem.DataSource = Nothing
                                cmbOSubItem.DataSource = _dtTempitem
                                cmbOSubItem.ValueMember = "SUBITEMID"
                                cmbOSubItem.DisplayMember = "SUBITEMNAME"
                                cmbOSubItem.AutoCompleteMode = AutoCompleteMode.SuggestAppend
                                cmbOSubItem.AutoCompleteSource = AutoCompleteSource.ListItems
                            End If
                        End If
                    ElseIf rbtStone.Checked Then
                        Dim _tempitem As String = cmbSItem.Text
                        Dim _tempsubitem As String = cmbSSubItem.Text
                        Dim _dtTempitem As DataTable
                        _dtTempitem = New DataTable
                        If _tempitem.ToString <> "" Then
                            StrSql = "SELECT ITEMID,ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & _tempitem.ToString & "'"
                            _dtTempitem = New DataTable
                            Da = New OleDbDataAdapter(StrSql, cn)
                            Da.Fill(_dtTempitem)
                            If _dtTempitem.Rows.Count > 0 Then
                                cmbSItem.DataSource = Nothing
                                cmbSItem.DataSource = _dtTempitem
                                cmbSItem.ValueMember = "ITEMID"
                                cmbSItem.DisplayMember = "ITEMNAME"
                                cmbSItem.AutoCompleteMode = AutoCompleteMode.SuggestAppend
                                cmbSItem.AutoCompleteSource = AutoCompleteSource.ListItems
                            End If
                        End If
                        If _tempsubitem.ToString <> "" Then
                            StrSql = " SELECT SUBITEMID,SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & _tempsubitem.ToString & "' AND"
                            StrSql += " ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & _tempitem.ToString & "')"
                            _dtTempitem = New DataTable
                            Da = New OleDbDataAdapter(StrSql, cn)
                            Da.Fill(_dtTempitem)
                            If _dtTempitem.Rows.Count > 0 Then
                                cmbSSubItem.DataSource = Nothing
                                cmbSSubItem.DataSource = _dtTempitem
                                cmbSSubItem.ValueMember = "SUBITEMID"
                                cmbSSubItem.DisplayMember = "SUBITEMNAME"
                                cmbSSubItem.AutoCompleteMode = AutoCompleteMode.SuggestAppend
                                cmbSSubItem.AutoCompleteSource = AutoCompleteSource.ListItems
                            End If
                        End If
                    End If
                End If
                If _Catname.ToString <> "" And MetalBasedStone Then
                    SendKeys.Send("{TAB}")
                End If
            Else
                MsgBox("Record not found...", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If
    End Sub

    Private Sub cmbOProcess_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbOProcess.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If txtTagNo.Text <> "" Then
                txtOPcs_NUM.Select()
            End If
        End If
    End Sub


    Private Sub dgvOrder_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvOrder.KeyDown
        If e.KeyCode = Keys.Delete Then
            dgvOrder.Rows.RemoveAt(dgvOrder.CurrentRow.Index)
            'MdtOdDet = New DataTable(dgvOrder.DataSource)
            MdtOdDet.AcceptChanges()
        ElseIf e.KeyCode = Keys.Enter Then
            btnOk.Focus()
        End If
    End Sub

    Private Sub txtMRemark2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMRemark2.KeyPress
        If e.KeyChar = Chr(Keys.Enter) And grpOrder.Visible = True Then
            lblMordNo.Focus()
            lblMordNo.Select()
        End If
    End Sub

    Private Sub txtRfId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRfId.KeyDown
        If e.KeyCode = Keys.Insert Then
            StrSql = " SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME='RFIDITEMTAGSTONE'"
            If objGPack.GetSqlValue(StrSql, , 0) = 1 Then
                StrSql = " SELECT RFID,ITEM,SUBITEM"
                StrSql += vbCrLf + " ,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT"
                StrSql += vbCrLf + " ,(SELECT CALCMODE FROM " & cnAdminDb & "..RFIDITEMTAGSTONE WHERE SNO=X.SNO)CALCMODE"
                StrSql += vbCrLf + " ,(SELECT STONEUNIT FROM " & cnAdminDb & "..RFIDITEMTAGSTONE WHERE SNO=X.SNO)STONEUNIT"
                StrSql += vbCrLf + " ,(SELECT STNRATE FROM " & cnAdminDb & "..RFIDITEMTAGSTONE WHERE SNO=X.SNO)STNRATE"
                StrSql += vbCrLf + " ,(SELECT STNAMT FROM " & cnAdminDb & "..RFIDITEMTAGSTONE WHERE SNO=X.SNO)STNAMT"
                StrSql += vbCrLf + " ,SNO"
                StrSql += vbCrLf + " FROM ("
                StrSql += vbCrLf + " SELECT RFIDNO AS RFID,"
                StrSql += vbCrLf + "(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=R.STNITEMID)ITEM"
                StrSql += vbCrLf + ",(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=R.STNSUBITEMID)SUBITEM"
                StrSql += vbCrLf + ",STNPCS,STNWT"
                StrSql += vbCrLf + ",CALCMODE,STONEUNIT"
                StrSql += vbCrLf + ",STNRATE,STNAMT,SNO"
                StrSql += vbCrLf + " FROM " & cnAdminDb & "..RFIDITEMTAGSTONE R WHERE ISSREC='R'"
                StrSql += vbCrLf + " AND COMPANYID='" & strCompanyId & "'"
                StrSql += vbCrLf + " AND ISNULL(COSTID,'')='" & cnCostId & "'"
                StrSql += vbCrLf + " UNION ALL"
                StrSql += vbCrLf + " SELECT  RFID,"
                StrSql += vbCrLf + "(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=R.ITEMID)ITEM"
                StrSql += vbCrLf + ",(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=R.SUBITEMID)SUBITEM"
                StrSql += vbCrLf + ",-1*PCS AS STNPCS,-1*GRSWT AS STNWT"
                StrSql += vbCrLf + ",WEIGHTUNIT,STONEUNIT"
                StrSql += vbCrLf + ",0 STNRATE,0 STNAMT,RESNO AS SNO"
                StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE R "
                StrSql += vbCrLf + " WHERE COMPANYID='" & strCompanyId & "'"
                StrSql += vbCrLf + " AND ISNULL(COSTID,'')='" & cnCostId & "'"
                StrSql += vbCrLf + " )X GROUP BY SNO,ITEM,SUBITEM,RFID "
                StrSql += vbCrLf + " HAVING SUM(STNWT)>0"
                Cmd = New OleDbCommand(StrSql, cn)
                Da = New OleDbDataAdapter(Cmd)
                Dim dtTagStn As DataTable
                dtTagStn = New DataTable
                Da.Fill(dtTagStn)
                If dtTagStn.Rows.Count > 0 Then
                    Dim JnoRow As DataRow
                    JnoRow = BrighttechPack.SearchDialog.Show_R("Find ", StrSql, cn, , 0, , , , , , True)
                    If Not JnoRow Is Nothing Then
                        txtRfId.Text = JnoRow.Item("RFID").ToString
                        RESNO = JnoRow.Item("SNO").ToString
                        cmbSItem.Text = JnoRow.Item("ITEM").ToString
                        cmbSSubItem.Text = JnoRow.Item("SUBITEM").ToString
                        txtSPcs_NUM.Text = Val(JnoRow.Item("STNPCS").ToString)
                        txtSGrsWt_WET.Text = Val(JnoRow.Item("STNWT").ToString)
                        cmbSCalcMode.Text = IIf(JnoRow.Item("CALCMODE").ToString = "W", "WEIGHT", "PIECE")
                        cmbSUnit.Text = IIf(JnoRow.Item("STONEUNIT").ToString = "G", "GRAM", "CARAT")
                        txtSRate_OWN.Text = Val(JnoRow.Item("STNRATE").ToString)
                        txtSGrsAmt_AMT.Text = Val(JnoRow.Item("STNAMT").ToString)
                    End If
                Else
                    MsgBox("RFID No not Found", MsgBoxStyle.Information)
                    txtRfId.Focus()
                    txtRfId.Select()
                End If
            End If
        End If
    End Sub

    Private Sub txtRfId_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRfId.KeyPress
        If txtRfId.Text = "" Then Exit Sub
        If e.KeyChar = Chr(Keys.Enter) Then
            StrSql = " SELECT 1 FROM " & cnAdminDb & "..SYSOBJECTS WHERE NAME='RFIDITEMTAGSTONE'"
            If objGPack.GetSqlValue(StrSql, , 0) = 1 Then
                StrSql = " SELECT RFID,ITEM,SUBITEM"
                StrSql += vbCrLf + " ,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT"
                StrSql += vbCrLf + " ,(SELECT CALCMODE FROM " & cnAdminDb & "..RFIDITEMTAGSTONE WHERE SNO=X.SNO)CALCMODE"
                StrSql += vbCrLf + " ,(SELECT STONEUNIT FROM " & cnAdminDb & "..RFIDITEMTAGSTONE WHERE SNO=X.SNO)STONEUNIT"
                StrSql += vbCrLf + " ,(SELECT STNRATE FROM " & cnAdminDb & "..RFIDITEMTAGSTONE WHERE SNO=X.SNO)STNRATE"
                StrSql += vbCrLf + " ,(SELECT STNAMT FROM " & cnAdminDb & "..RFIDITEMTAGSTONE WHERE SNO=X.SNO)STNAMT"
                StrSql += vbCrLf + " ,SNO"
                StrSql += vbCrLf + " FROM ("
                StrSql += vbCrLf + " SELECT RFIDNO AS RFID,"
                StrSql += vbCrLf + "(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=R.STNITEMID)ITEM"
                StrSql += vbCrLf + ",(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=R.STNSUBITEMID)SUBITEM"
                StrSql += vbCrLf + ",STNPCS,STNWT"
                StrSql += vbCrLf + ",CALCMODE,STONEUNIT"
                StrSql += vbCrLf + ",STNRATE,STNAMT,SNO"
                StrSql += vbCrLf + " FROM " & cnAdminDb & "..RFIDITEMTAGSTONE R WHERE ISSREC='R'"
                StrSql += vbCrLf + " AND COMPANYID='" & strCompanyId & "'"
                StrSql += vbCrLf + " AND ISNULL(COSTID,'')='" & cnCostId & "'"
                StrSql += vbCrLf + " AND RFIDNO='" & txtRfId.Text & "'"
                StrSql += vbCrLf + " UNION ALL"
                StrSql += vbCrLf + " SELECT  RFID,"
                StrSql += vbCrLf + "(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=R.ITEMID)ITEM"
                StrSql += vbCrLf + ",(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=R.SUBITEMID)SUBITEM"
                StrSql += vbCrLf + ",-1*PCS AS STNPCS,-1*GRSWT AS STNWT"
                StrSql += vbCrLf + ",WEIGHTUNIT,STONEUNIT"
                StrSql += vbCrLf + ",0 STNRATE,0 STNAMT,RESNO AS SNO"
                StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE R "
                StrSql += vbCrLf + " WHERE COMPANYID='" & strCompanyId & "'"
                StrSql += vbCrLf + " AND ISNULL(COSTID,'')='" & cnCostId & "'"
                StrSql += vbCrLf + " AND RFID='" & txtRfId.Text & "'"
                StrSql += vbCrLf + " )X GROUP BY SNO,ITEM,SUBITEM,RFID "
                StrSql += vbCrLf + " HAVING SUM(STNWT)>0"
                Cmd = New OleDbCommand(StrSql, cn)
                Da = New OleDbDataAdapter(Cmd)
                Dim dtTagStn As DataTable
                dtTagStn = New DataTable
                Da.Fill(dtTagStn)
                If dtTagStn.Rows.Count > 0 Then
                    With dtTagStn.Rows(0)
                        RESNO = .Item("SNO").ToString
                        cmbSItem.Text = .Item("ITEM").ToString
                        cmbSSubItem.Text = .Item("SUBITEM").ToString
                        txtSPcs_NUM.Text = Val(.Item("STNPCS").ToString)
                        txtSGrsWt_WET.Text = Val(.Item("STNWT").ToString)
                        cmbSCalcMode.Text = IIf(.Item("CALCMODE").ToString = "W", "WEIGHT", "PIECE")
                        cmbSUnit.Text = IIf(.Item("STONEUNIT").ToString = "G", "GRAM", "CARAT")
                        txtSRate_OWN.Text = Val(.Item("STNRATE").ToString)
                        txtSGrsAmt_AMT.Text = Val(.Item("STNAMT").ToString)
                    End With
                Else
                    MsgBox("RFID No not Found", MsgBoxStyle.Information)
                    txtRfId.Focus()
                    txtRfId.Select()
                End If
            End If
        End If
    End Sub

    Private Sub txtOED_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOED_AMT.TextChanged
        CalcOGrossAmt()
    End Sub

    Private Sub CalcONGrossAmt()
        Dim GrsAmt As Decimal = Nothing
        If Val(txtOGrsWt_WET.Text) <> 0 Then
            If cmbOcalcon.Text = "GRS WT" Then
                GrsAmt = Val(txtOGrsWt_WET.Text) + Val(txtOWast_WET.Text) + (-1 * Val(txtOalloy_WET.Text)) _
                           * Val(txtORate_OWN.Text)
                GrsAmt = Val(txtOGrsWt_WET.Text) * Val(txtORate_OWN.Text)
            ElseIf cmbOcalcon.Text = "NET WT" Then
                GrsAmt = Val(txtONetWt_WET.Text) + Val(txtOWast_WET.Text) + (-1 * Val(txtOalloy_WET.Text)) _
                                         * Val(txtORate_OWN.Text)
                GrsAmt = Val(txtONetWt_WET.Text) * Val(txtORate_OWN.Text)
            ElseIf cmbOcalcon.Text = "PURE WT" Then
                GrsAmt = Val(txtOPureWt_WET.Text) + Val(txtOWast_WET.Text) + (-1 * Val(txtOalloy_WET.Text)) _
                                         * Val(txtORate_OWN.Text)
                GrsAmt = Val(txtOPureWt_WET.Text) * Val(txtORate_OWN.Text)
            End If
            'If Val(txtOTouchAMT.Text) <> 0 Then
            '    GrsAmt = Val(txtOPureWt_WET.Text) * Val(txtORate_OWN.Text)
            'End If
        Else
            GrsAmt = Val(txtOPcs_NUM.Text) * Val(txtORate_OWN.Text)
        End If
        GrsAmt = GrsAmt + Val(txtOMc_AMT.Text) + Val(txtOStudAmt_AMT.Text) + Val(txtOAddlCharge_AMT.Text) '+ Val(txtOED_AMT.Text)
        GrsAmt -= Val(txtODisc.Text)
        GrsAmt = RoundOffPisa(GrsAmt)
        txtOGrsAmt_AMT.Text = IIf(GrsAmt <> 0, Format(GrsAmt, "0.00"), Nothing)
        CalcOVatTds()
        CalcOGST()
        CalcONetAmt()
    End Sub

    Private Sub cmbOcalcon_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbOcalcon.SelectedIndexChanged
        CalcOGrossAmt()
    End Sub

    Private Sub txtOedPer_AMT_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOedPer_AMT.TextChanged
        If Val(txtOedPer_AMT.Text) = 0 Then
            txtOED_AMT.Clear()
            Exit Sub
        End If
        CalcOEd()
    End Sub
    Private Sub CalcOEd()
        If GstFlag Then Exit Sub
        Dim edTds As Decimal = Nothing
        edTds = (Val(txtOGrsAmt_AMT.Text)) * (Val(txtOedPer_AMT.Text) / 100)
        'edTds = IIf(lblOVat.Text.ToUpper = "TDS", RoundOffPisa(edTds, True), RoundOffPisa(edTds))
        txtOED_AMT.Text = IIf(edTds <> 0, Format(edTds, "0.00"), Nothing)
        CalcONetAmt()
    End Sub


    Private Sub txtOSgst_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
        txtOSgst_AMT.GotFocus, txtOCgst_AMT.GotFocus, txtOIgst_AMT.GotFocus,
        txtOSG_AMT.GotFocus, txtOCG_AMT.GotFocus, txtOIG_AMT.GotFocus
        If PURVATEDITACC = False Then
            If lblOVat.Text.ToUpper.ToString = "GST" Then
                txtOSgst_AMT.ReadOnly = True
                txtOCgst_AMT.ReadOnly = True
                txtOIgst_AMT.ReadOnly = True
                txtOSG_AMT.ReadOnly = True
                txtOCG_AMT.ReadOnly = True
                txtOIG_AMT.ReadOnly = True
            End If
        End If
    End Sub

    Private Sub txtLostFocus(sender As Object, e As EventArgs) Handles _
        txtOSgst_AMT.LostFocus, txtOCgst_AMT.LostFocus, txtOIgst_AMT.LostFocus,
        txtMSgst_AMT.LostFocus, txtMCgst_AMT.LostFocus, txtMIgst_AMT.LostFocus,
        txtSSgst_WET.LostFocus, txtSCgst_WET.LostFocus, txtSIgst_WET.LostFocus,
        txtOthSgst_AMT.LostFocus, txtOthCgst_AMT.LostFocus, txtOthIgst_AMT.LostFocus
        If Val(txtOSgst_AMT.Text) = 0 Then txtOSgst_AMT.Clear() Else CalcOGST()
        If Val(txtOCgst_AMT.Text) = 0 Then txtOCgst_AMT.Clear() Else CalcOGST()
        If Val(txtOIgst_AMT.Text) = 0 Then txtOIgst_AMT.Clear() Else CalcOGST()

        If Val(txtMSgst_AMT.Text) = 0 Then txtMSgst_AMT.Clear() Else CalcMGST()
        If Val(txtMCgst_AMT.Text) = 0 Then txtMCgst_AMT.Clear() Else CalcMGST()
        If Val(txtMIgst_AMT.Text) = 0 Then txtMIgst_AMT.Clear() Else CalcMGST()

        If Val(txtSSgst_WET.Text) = 0 Then txtSSgst_WET.Clear() Else CalcSGST()
        If Val(txtSCgst_WET.Text) = 0 Then txtSCgst_WET.Clear() Else CalcSGST()
        If Val(txtSIgst_WET.Text) = 0 Then txtSIgst_WET.Clear() Else CalcSGST()

        If Val(txtOthSgst_AMT.Text) = 0 Then txtOthSgst_AMT.Clear() Else CalcOthGST()
        If Val(txtOthCgst_AMT.Text) = 0 Then txtOthCgst_AMT.Clear() Else CalcOthGST()
        If Val(txtOthIgst_AMT.Text) = 0 Then txtOthIgst_AMT.Clear() Else CalcOthGST()
    End Sub

    Private Sub txtOSgst_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOSgst_AMT.TextChanged
        If Val(txtOSgst_AMT.Text) >= 0 And (IsNumeric(txtOSgst_AMT.Text)) Then
        Else
            txtOSgst_AMT.Clear()
            Exit Sub
        End If
        CalcOGST()
    End Sub

    Private Sub txtOCgst_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOCgst_AMT.TextChanged
        If Val(txtOCgst_AMT.Text) >= 0 And (IsNumeric(txtOCgst_AMT.Text)) Then
        Else
            txtOCgst_AMT.Clear()
            Exit Sub
        End If
        CalcOGST()
    End Sub

    Private Sub txtOIgst_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOIgst_AMT.TextChanged
        If Val(txtOIgst_AMT.Text) >= 0 And (IsNumeric(txtOIgst_AMT.Text)) Then
        Else
            txtOIgst_AMT.Clear()
            Exit Sub
        End If
        CalcOGST()
    End Sub

    Private Sub txtMSgst_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
        txtMSgst_AMT.GotFocus, txtMCgst_AMT.GotFocus, txtMIgst_AMT.GotFocus,
        txtMSG_AMT.GotFocus, txtMCG_AMT.GotFocus, txtMIG_AMT.GotFocus
        If PURVATEDITACC = False Then
            If lblMVat.Text.ToUpper.ToString = "GST" Then
                txtMSgst_AMT.ReadOnly = True
                txtMCgst_AMT.ReadOnly = True
                txtMIgst_AMT.ReadOnly = True
                txtMSG_AMT.ReadOnly = True
                txtMCG_AMT.ReadOnly = True
                txtMIG_AMT.ReadOnly = True
            End If
        End If
    End Sub

    Private Sub txtMSgst_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMSgst_AMT.TextChanged
        If Val(txtMSgst_AMT.Text) >= 0 And (IsNumeric(txtMSgst_AMT.Text)) Then
        Else
            txtMSgst_AMT.Clear()
            Exit Sub
        End If
        CalcMGST()
    End Sub

    Private Sub txtMCgst_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMCgst_AMT.TextChanged
        If Val(txtMCgst_AMT.Text) >= 0 And (IsNumeric(txtMCgst_AMT.Text)) Then
        Else
            txtMCgst_AMT.Clear()
            Exit Sub
        End If
        CalcMGST()
    End Sub

    Private Sub txtMIgst_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMIgst_AMT.TextChanged
        If Val(txtMIgst_AMT.Text) >= 0 And (IsNumeric(txtMIgst_AMT.Text)) Then
        Else
            txtMIgst_AMT.Clear()
            Exit Sub
        End If
        CalcMGST()
    End Sub

    Private Sub txtSIgst_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSIgst_WET.TextChanged
        If Val(txtSIgst_WET.Text) >= 0 And (IsNumeric(txtSIgst_WET.Text)) Then
        Else
            txtSIgst_WET.Clear()
            Exit Sub
        End If
        CalcSGST()
    End Sub

    Private Sub txtSCgst_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSCgst_WET.TextChanged
        If Val(txtSCgst_WET.Text) >= 0 And (IsNumeric(txtSCgst_WET.Text)) Then
        Else
            txtSCgst_WET.Clear()
            Exit Sub
        End If
        CalcSGST()
    End Sub

    Private Sub txtSSgst_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
     txtSSgst_WET.GotFocus, txtSCgst_WET.GotFocus, txtSIgst_WET.GotFocus,
     txtSSG_AMT.GotFocus, txtSCG_AMT.GotFocus, txtSIG_AMT.GotFocus
        If PURVATEDITACC = False Then
            If lblSVat.Text.ToUpper.ToString = "GST" Then
                txtSSgst_WET.ReadOnly = True
                txtSCgst_WET.ReadOnly = True
                txtSIgst_WET.ReadOnly = True
                txtSSG_AMT.ReadOnly = True
                txtSCG_AMT.ReadOnly = True
                txtSIG_AMT.ReadOnly = True
            End If
        End If
    End Sub

    Private Sub txtSSgst_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSSgst_WET.TextChanged
        If Val(txtSSgst_WET.Text) >= 0 And (IsNumeric(txtSSgst_WET.Text)) Then
        Else
            txtSSgst_WET.Clear()
            Exit Sub
        End If
        CalcSGST()
    End Sub

    Private Sub txtOthSgst_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOthSgst_AMT.TextChanged
        If Val(txtOthSgst_AMT.Text) >= 0 And (IsNumeric(txtOthSgst_AMT.Text)) Then
        Else
            txtOthSgst_AMT.Clear()
            Exit Sub
        End If
        CalcOthGST()
    End Sub

    Private Sub txtMGrsWt_WET_KeyDown(sender As Object, e As KeyEventArgs) Handles txtMGrsWt_WET.KeyDown
        If e.KeyCode = Keys.Enter Then
            If ORADDITIONALDETAIL = True Then
                ShowOrderAdditionalDetails()
            End If
        End If
    End Sub

    Private Sub txtOthCgst_AMT_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
        txtOthCgst_AMT.GotFocus, txtOthSgst_AMT.GotFocus, txtOthIgst_AMT.GotFocus,
        txtOthCG_AMT.GotFocus, txtOthSG_AMT.GotFocus, txtOthIG_AMT.GotFocus
        If PURVATEDITACC = False Then
            If lblOthVat.Text.ToUpper.ToString = "GST" Then
                txtOthSgst_AMT.ReadOnly = True
                txtOthCgst_AMT.ReadOnly = True
                txtOthIgst_AMT.ReadOnly = True
                txtOthSG_AMT.ReadOnly = True
                txtOthCG_AMT.ReadOnly = True
                txtOthIG_AMT.ReadOnly = True
            End If
        End If
    End Sub

    Private Sub txtOthCgst_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOthCgst_AMT.TextChanged
        If Val(txtOthCgst_AMT.Text) >= 0 And (IsNumeric(txtOthCgst_AMT.Text)) Then
        Else
            txtOthCgst_AMT.Clear()
            Exit Sub
        End If
        CalcOthGST()
    End Sub

    Private Sub txtOthIgst_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOthIgst_AMT.TextChanged
        If Val(txtOthIgst_AMT.Text) >= 0 And (IsNumeric(txtOthIgst_AMT.Text)) Then
        Else
            txtOthIgst_AMT.Clear()
            Exit Sub
        End If
        CalcOthGST()
    End Sub

    Private Sub txtOCG_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
        txtOCG_AMT.TextChanged, txtOSG_AMT.TextChanged, txtOIG_AMT.TextChanged
        CalcONetAmt()
        If lblOVat.Text.ToUpper.ToString = "GST" And PnlGst.Enabled Then
            Dim _GstAmt As Double
            _GstAmt = Val(txtOSG_AMT.Text) + Val(txtOCG_AMT.Text) + Val(txtOIG_AMT.Text)
            txtOVat_AMT.Text = IIf(_GstAmt <> 0, Format(_GstAmt, "0.00"), Nothing)
        End If
    End Sub

    Private Sub txtMSG_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
        txtMSG_AMT.TextChanged, txtMCG_AMT.TextChanged, txtMIG_AMT.TextChanged
        CalcMNetAmt()
        If lblMVat.Text.ToUpper.ToString = "GST" And PnlMGst.Enabled Then
            Dim _GstAmt As Double
            _GstAmt = Val(txtMSG_AMT.Text) + Val(txtMCG_AMT.Text) + Val(txtMIG_AMT.Text)
            txtMVat_AMT.Text = IIf(_GstAmt <> 0, Format(_GstAmt, "0.00"), Nothing)
        End If
    End Sub

    Private Sub txtMAprxAmount_AMT_TextChanged(sender As Object, e As EventArgs) Handles txtMAprxAmount_AMT.TextChanged
        If Val(txtMAprxAmount_AMT.Text) > 0 Then
            StrSql = "SELECT * FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME='" & cmbMCategory.Text & "'"
            Dim dtcat As DataTable
            dtcat = New DataTable
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dtcat)
            If dtcat.Rows.Count > 0 Then
                Dim gstper As Double = Val(dtcat.Rows(0).Item("S_SGSTTAX").ToString) + Val(dtcat.Rows(0).Item("S_CGSTTAX").ToString)
                Dim aprxGst As Decimal = Nothing
                aprxGst = Val(txtMAprxAmount_AMT.Text) * (Val(gstper) / 100)
                aprxGst = RoundOffPisa(aprxGst)
                txtMAprxTaxAmt_AMT.Text = IIf(aprxGst <> 0, Format(aprxGst, "0.00"), Nothing)
            End If
        End If
    End Sub

    Private Sub txtOAprxAmount_AMT_TextChanged(sender As Object, e As EventArgs) Handles txtOAprxAmount_AMT.TextChanged
        If Val(txtOAprxAmount_AMT.Text) > 0 Then
            StrSql = "SELECT * FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME='" & cmbOCategory.Text & "'"
            Dim dtcat As DataTable
            dtcat = New DataTable
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dtcat)
            If dtcat.Rows.Count > 0 Then
                Dim gstper As Double = Val(dtcat.Rows(0).Item("S_SGSTTAX").ToString) + Val(dtcat.Rows(0).Item("S_CGSTTAX").ToString)
                Dim aprxGst As Decimal = Nothing
                aprxGst = Val(txtOAprxAmount_AMT.Text) * (Val(gstper) / 100)
                aprxGst = RoundOffPisa(aprxGst)
                txtOAprxTaxAmt_AMT.Text = IIf(aprxGst <> 0, Format(aprxGst, "0.00"), Nothing)
            End If
        End If
    End Sub
    Private Sub txtItemId_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtItemId.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim barcode2d() As String = txtItemId.Text.Split("-")
            If barcode2d.Length = 2 Then
                txtItemId.Text = barcode2d(0)
                txtTagNo.Text = barcode2d(1)
                txtTagNo_KeyDown(sender, New KeyEventArgs(Keys.Enter))
                btnOk.Focus()
            End If
        End If
    End Sub

    Private Sub Cmboacname_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cmboacname.SelectedIndexChanged
        If Cmboacname.Text <> "" Then
            If UCase(oTransactionType) <> "PURCHASE RETURN" And UCase(oTransactionType) <> "INTERNAL TRANSFER" And UCase(oTransactionType) <> "APPROVAL RECEIPT" Then
                Dim tds_accode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATNAME = '" & Cmboacname.Text & "'")
                txtotdsaccode.Text = tds_accode
                TdsPer = Val(objGPack.GetSqlValue("SELECT TDSPER FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATNAME = '" & Cmboacname.Text & "'"))
                txtOVatPer_PER.Text = IIf(TdsPer <> 0, Format(TdsPer, "0.00"), "")
                txtMVatPer_PER.Text = IIf(TdsPer <> 0, Format(TdsPer, "0.00"), "")
                txtSVatPer_PER.Text = IIf(TdsPer <> 0, Format(TdsPer, "0.00"), "")
                txtOthVatPer_PER.Text = IIf(TdsPer <> 0, Format(TdsPer, "0.00"), "")
                If txtOGrsAmt_AMT.Text <> "" Then
                    CalcOVatTds()
                    CalcOGST()
                    CalcONetAmt()
                End If
            End If
        End If
    End Sub
    Private Sub Cmbmacname_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cmbmacname.SelectedIndexChanged
        If Cmbmacname.Text <> "" Then
            If UCase(oTransactionType) <> "PURCHASE RETURN" And UCase(oTransactionType) <> "INTERNAL TRANSFER" And UCase(oTransactionType) <> "APPROVAL RECEIPT" Then
                Dim tds_accode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATNAME = '" & Cmbmacname.Text & "'")
                txtmtdsaccode.Text = tds_accode
                TdsPer = Val(objGPack.GetSqlValue("SELECT TDSPER FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATNAME = '" & Cmbmacname.Text & "'"))
                txtOVatPer_PER.Text = IIf(TdsPer <> 0, Format(TdsPer, "0.00"), "")
                txtMVatPer_PER.Text = IIf(TdsPer <> 0, Format(TdsPer, "0.00"), "")
                txtSVatPer_PER.Text = IIf(TdsPer <> 0, Format(TdsPer, "0.00"), "")
                txtOthVatPer_PER.Text = IIf(TdsPer <> 0, Format(TdsPer, "0.00"), "")
                If txtOGrsAmt_AMT.Text <> "" Then
                    CalcOVatTds()
                    CalcOGST()
                    CalcONetAmt()
                End If
            End If
        End If
    End Sub
    Private Sub Cmbsacname_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cmbsacname.SelectedIndexChanged
        If Cmbsacname.Text <> "" Then
            If UCase(oTransactionType) <> "PURCHASE RETURN" And UCase(oTransactionType) <> "INTERNAL TRANSFER" And UCase(oTransactionType) <> "APPROVAL RECEIPT" Then
                Dim tds_accode As String = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATNAME = '" & Cmbsacname.Text & "'")
                txtstdsaccode.Text = tds_accode
                TdsPer = Val(objGPack.GetSqlValue("SELECT TDSPER FROM  " & cnAdminDb & "..TDSCATEGORY WHERE TDSCATNAME = '" & Cmbsacname.Text & "'"))
                txtOVatPer_PER.Text = IIf(TdsPer <> 0, Format(TdsPer, "0.00"), "")
                txtMVatPer_PER.Text = IIf(TdsPer <> 0, Format(TdsPer, "0.00"), "")
                txtSVatPer_PER.Text = IIf(TdsPer <> 0, Format(TdsPer, "0.00"), "")
                txtOthVatPer_PER.Text = IIf(TdsPer <> 0, Format(TdsPer, "0.00"), "")
                If txtOGrsAmt_AMT.Text <> "" Then
                    CalcOVatTds()
                    CalcOGST()
                    CalcONetAmt()
                End If
            End If
        End If
    End Sub

    Private Sub txtOthSG_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
        txtOthSG_AMT.TextChanged, txtOthCG_AMT.TextChanged, txtOthIG_AMT.TextChanged
        CalcOthNetAmt()
        If lblOthVat.Text.ToUpper.ToString = "GST" And PnlOthGst.Enabled Then
            Dim _GstAmt As Double
            _GstAmt = Val(txtOthSG_AMT.Text) + Val(txtOthCG_AMT.Text) + Val(txtOthIG_AMT.Text)
            txtOthVat_AMT.Text = IIf(_GstAmt <> 0, Format(_GstAmt, "0.00"), Nothing)
        End If
    End Sub

    Private Sub txtSSG_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
        txtSSG_AMT.TextChanged, txtSCG_AMT.TextChanged, txtSIG_AMT.TextChanged
        CalcSNetAmt()
        If lblSVat.Text.ToUpper.ToString = "GST" And PnlSGst.Enabled Then
            Dim _GstAmt As Double
            _GstAmt = Val(txtSSG_AMT.Text) + Val(txtSCG_AMT.Text) + Val(txtSIG_AMT.Text)
            txtSVat_AMT.Text = IIf(_GstAmt <> 0, Format(_GstAmt, "0.00"), Nothing)
        End If
    End Sub

    Private Sub txtOGrsAmt_AMT_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOGrsAmt_AMT.TextChanged
        If PURAMTEDITACC = "Y" Or Val(txtORate_OWN.Text) = 0 Then
            CalcOVatTds()
            CalcOGST()
            CalcONetAmt()
        End If
    End Sub

    Private Sub cmbSCategory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSCategory.SelectedIndexChanged
        If MI_STOCKCHECK Then
            LoadBalanceStock(cmbSCategory.Text)
        End If
    End Sub

    Private Sub tabMain_Resize(sender As Object, e As EventArgs) Handles tabMain.Resize

    End Sub

    Private Sub cmbOMetal_LostFocus(sender As Object, e As EventArgs) Handles cmbOMetal.LostFocus
        If STOCKVALIDATION_MR And oMaterial = Material.Receipt Then
            LoadBalanceStock_MR(cmbOMetal.Text)
        End If
    End Sub

    Private Sub cmbMMetal_LostFocus(sender As Object, e As EventArgs) Handles cmbMMetal.LostFocus
        If STOCKVALIDATION_MR And oMaterial = Material.Receipt Then
            LoadBalanceStock_MR(cmbMMetal.Text)
        End If
    End Sub

    Private Sub cmbSMetal_LostFocus(sender As Object, e As EventArgs) Handles cmbSMetal.LostFocus
        If STOCKVALIDATION_MR And oMaterial = Material.Receipt Then
            LoadBalanceStock_MR(cmbSMetal.Text)
        End If
    End Sub

    Private Sub txtSPcs_NUM_GotFocus(sender As Object, e As EventArgs) Handles txtSPcs_NUM.GotFocus
        'If STOCKVALIDATION_MR Then
        '    LoadBalanceStock_MR(cmbOMetal.Text)
        'End If
    End Sub

    Private Sub txtMTCS_AMT_TextChanged(sender As Object, e As EventArgs) Handles txtMTCS_AMT.TextChanged
        CalcMGrossAmt()
    End Sub

    Private Sub txtSTCS_AMT_TextChanged(sender As Object, e As EventArgs) Handles txtSTCS_AMT.TextChanged
        CalcSGrossAmt()
    End Sub


End Class