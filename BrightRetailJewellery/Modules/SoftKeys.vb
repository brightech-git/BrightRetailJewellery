Imports System.Data.OleDb
Public Class SoftKeys
    Private StrSql As String
    Private da As OleDbDataAdapter
    Private dt As New DataTable
    ''SoftControl Values
    Public ChitDb As String
    Public ChitDbPrefix As String

    ''Bill Variales
    Public TagLock As String
    Public AmtLok As String
    Public AmtLok_Pur As String
    Public CashPayment As String
    Public CASHPAYMENTLOCK As String
    Public CashReceived As String
    Public CASHRECEIVEDLOCK As String
    Public PANVALUE As Decimal
    Public SALVALUE As Decimal
    Public PANVALUELOCK As String
    Public PANLOCK_EXCLUDE As String
    Public PANLOCK_FINYR As Boolean
    Public POS_PANLOCK_MODE As String
    Public POS_PANLOCK As String
    Public RoundOff_Gross As String
    Public RoundOff_Final As String
    Public RoundOff_Vat As String
    Public ROUNDOFF_PURAMT As String
    Public RoundOff_Gst As String
    Public RoundOff_Sc As String
    Public ROUNDOFF_PURRATE As String
    Public SalZeroAmt As String
    Public SalDisc As String
    Public Tag_search_Disable As Boolean
    Public SalRateLok As String
    Public SalRateChk As String
    Public AutoLessWt As String
    Public RateDiscGrm As String
    Public RateDiscWt As String
    Public RateDiscIncWast As String
    Public EstCalling As String
    Public MinValChk As String
    Public BillValueChk As String
    Public PurRateChk As String
    Public Tolerance_Rate As String
    Public Tolerance_PurRate As String
    Public RateChangeF4 As Boolean
    Public VatEdit As Boolean
    Public PRODTAGSEP As Char
    Public PRODASTAG As Boolean
    Public Wet2Wet As Boolean
    Public Wet2Wet_Advrate As Boolean
    Public Wet2Wet_Pur As Boolean
    Public MiscRemark As String
    Public Finalamtlockbase As String

    Public OrdMcTax As Boolean
    Public OrdMcTaxEx As Boolean
    Public OrdMcTaxLessWt As Boolean
    Public AdjDiscCalc As String
    Public AdjDisc_MCFirst As Boolean
    Public AdjDiscStore As String
    Public McWithWastage As Boolean
    Public ORDMCPOST2EXWT As Boolean
    Public SepAccPost As Boolean
    Public SEPACCPOST_ITEM As String
    Public SEPSTUDTAXPOST As Boolean
    Public DirectRepair As Boolean
    Public HasEstPost As Boolean
    Public CallBackDtEst As Boolean
    Public PURALLOYPER As Decimal
    Public PUR22KT As Decimal
    Public PUR22KT_TYPE As Integer
    Public PUR22KT_TYPE_PURITY As Decimal
    Public PURITYPURRATE As Boolean
    Public GrossLok As Boolean
    Public DESIGNER_PROMPT As Boolean
    Public FINALDISC_SEP As Boolean
    Public ORDMCACC As String
    Public ORDMCACCTAX As String
    Public REPMCACC As String
    Public REPMCACC_G As String
    Public REPMCACC_S As String
    Public REPMCACC_P As String
    Public REPMCACC_D As String
    Public REPMCACC_T As String
    Public PURRATELOCK As Boolean
    Public PURWASTLOCK As Boolean
    Public SEPRESCTRID As Decimal
    Public ED_COMPONENTS As String = Nothing
    Public EDC1ID As String = Nothing
    Public EDC2ID As String = Nothing
    Public EDC3ID As String = Nothing
    Public EDC1PER As Decimal = Nothing
    Public EDC2PER As Decimal = Nothing
    Public EDC3PER As Decimal = Nothing
    Public EXDUTY As Boolean = False
    Public TCS_COMPONENT As String = Nothing
    Public TCSID As String = Nothing
    Public TCSCAL As Boolean = False
    Public TCSVALUEFOR As Decimal = 0

    Public TCS_LIMIT As Decimal = 0
    Public TCS_PERCENT As String = ""
    Public TCS_BASEDON As String = ""

    Public SALEVALUELIMIT As Decimal
    Public TCSINCLOGVALUE As String = Nothing
    Public TCSPER As Decimal = Nothing
    Public BARCODE2DSEP As String
    Public StkType As String = "M"
    Public EXCLUDE_EXDUTY As String
    Public CALCEXDUTY As String
    Public ORDDEL_OLD As Boolean
    Public AdjChitDiscStore As Boolean
    Public OrdMcTaxPer As Decimal
    Public OrdMcExTaxPer As Decimal
    Public ORDMC_GST_ACC As String
    Public LESS_WAST_ONDISC As Boolean
    Public MaxAsMinMC As String = "N"
    Public MaxAsMinWAST As String = "N"
    Public DUPLESTASALE As String = "N"

    Public Sub New()
        SetSoftValues()
    End Sub

    Function SetSoftValues() As Integer

        Dim dtSoftKeyss As DataTable = GetAdmindbSoftValueAll()
        LESS_WAST_ONDISC = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "LESS_WAST_ONDISC", "Y") = "Y", True, False)
        ORDMC_GST_ACC = GetAdmindbSoftValuefromDt(dtSoftKeyss, "ORDMC_GST_ACC", "").ToString
        OrdMcTaxPer = Val(GetAdmindbSoftValuefromDt(dtSoftKeyss, "ORDMCTAXPER", "5").ToString)
        OrdMcExTaxPer = Val(GetAdmindbSoftValuefromDt(dtSoftKeyss, "ORDMCEXTAXPER", "5").ToString)
        CALCEXDUTY = GetAdmindbSoftValuefromDt(dtSoftKeyss, "CALCEXDUTY", "P,E")
        ORDDEL_OLD = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "ORDDEL_OLD", "N") = "Y", True, False)
        AdjChitDiscStore = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "ADJ-CHITDISC-STORE", "N") = "Y", True, False)
        EXCLUDE_EXDUTY = GetAdmindbSoftValuefromDt(dtSoftKeyss, "EXCLUDE_EXDUTY", "")
        StkType = GetAdmindbSoftValuefromDt(dtSoftKeyss, "EXDUTY", "M")
        ChitDb = GetAdmindbSoftValuefromDt(dtSoftKeyss, "CHITDB", "N")
        ChitDbPrefix = GetAdmindbSoftValuefromDt(dtSoftKeyss, "CHITDBPREFIX", cnCompanyId)
        ''Getting Bill 
        TagLock = GetAdmindbSoftValuefromDt(dtSoftKeyss, "TAGLOCK", "N")
        AmtLok = GetAdmindbSoftValuefromDt(dtSoftKeyss, "AMTLOK", "Y")
        AmtLok_Pur = GetAdmindbSoftValuefromDt(dtSoftKeyss, "AMTLOK_PUR", "N")
        CashReceived = GetAdmindbSoftValuefromDt(dtSoftKeyss, "CASHRECEIVED", "199999")
        CASHRECEIVEDLOCK = GetAdmindbSoftValuefromDt(dtSoftKeyss, "CASHRECEIVEDLOCK", "R")
        CashPayment = GetAdmindbSoftValuefromDt(dtSoftKeyss, "CASHPAYMENT", "2000")
        CASHPAYMENTLOCK = GetAdmindbSoftValuefromDt(dtSoftKeyss, "CASHPAYMENTLOCK", "R")
        SALVALUE = Val(GetAdmindbSoftValuefromDt(dtSoftKeyss, "SALVALUE", 0))
        PANVALUE = Val(GetAdmindbSoftValuefromDt(dtSoftKeyss, "PANVALUE", 0))
        PANVALUELOCK = GetAdmindbSoftValuefromDt(dtSoftKeyss, "PANVALUELOCK", "R")
        PANLOCK_EXCLUDE = GetAdmindbSoftValuefromDt(dtSoftKeyss, "PANLOCK_EXCLUDE", "")
        POS_PANLOCK = GetAdmindbSoftValuefromDt(dtSoftKeyss, "POS_PANLOCK", "")
        POS_PANLOCK_MODE = GetAdmindbSoftValuefromDt(dtSoftKeyss, "POS_PANLOCK_MODE", "")
        PANLOCK_FINYR = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "POS_PANLOCK_FINYR", "N") = "Y", True, False)
        RoundOff_Gross = GetAdmindbSoftValuefromDt(dtSoftKeyss, "ROUNDOFF-GROSS", "N")
        RoundOff_Final = GetAdmindbSoftValuefromDt(dtSoftKeyss, "ROUNDOFF-FINAL", "N")
        RoundOff_Vat = GetAdmindbSoftValuefromDt(dtSoftKeyss, "ROUNDOFF-VAT", "N")
        RoundOff_Gst = GetAdmindbSoftValuefromDt(dtSoftKeyss, "ROUNDOFF-GST", "N")
        RoundOff_Sc = GetAdmindbSoftValuefromDt(dtSoftKeyss, "ROUNDOFF-SC", "N")
        ROUNDOFF_PURRATE = GetAdmindbSoftValuefromDt(dtSoftKeyss, "ROUNDOFF-PURRATE", "N")
        SalZeroAmt = GetAdmindbSoftValuefromDt(dtSoftKeyss, "SALZEROAMT", "N")
        SalDisc = GetAdmindbSoftValuefromDt(dtSoftKeyss, "SALDISC", "Y")
        SalRateLok = GetAdmindbSoftValuefromDt(dtSoftKeyss, "SALRATELOK", "Y")
        SalRateChk = GetAdmindbSoftValuefromDt(dtSoftKeyss, "SALRATECHK", "0")
        AutoLessWt = GetAdmindbSoftValuefromDt(dtSoftKeyss, "AUTOLESSWT", "Y")
        RateDiscGrm = GetAdmindbSoftValuefromDt(dtSoftKeyss, "RATEDISCGRM", "0")
        RateDiscWt = GetAdmindbSoftValuefromDt(dtSoftKeyss, "RATEDISCWT", "N")
        RateDiscIncWast = GetAdmindbSoftValuefromDt(dtSoftKeyss, "RATEDISCINCWAST", "N")
        EstCalling = GetAdmindbSoftValuefromDt(dtSoftKeyss, "ESTCALLING", "N")
        MinValChk = GetAdmindbSoftValuefromDt(dtSoftKeyss, "MINVALCHK", "N")
        BillValueChk = GetAdmindbSoftValuefromDt(dtSoftKeyss, "BILLVALUECHK", "Y")
        PurRateChk = GetAdmindbSoftValuefromDt(dtSoftKeyss, "PURRATECHK", "Y")
        Tolerance_Rate = GetAdmindbSoftValuefromDt(dtSoftKeyss, "TOLERANCE_RATE", "20")
        Tolerance_PurRate = GetAdmindbSoftValuefromDt(dtSoftKeyss, "TOLERANCE_PURRATE", "0")
        RateChangeF4 = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "RATECHANGEF4", "Y") = "Y", True, False)
        VatEdit = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "VATEDIT", "N") = "Y", True, False)
        GrossLok = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "GROSSLOK", "N") = "Y", True, False)
        PRODTAGSEP = GetAdmindbSoftValuefromDt(dtSoftKeyss, "PRODTAGSEP", "")
        PRODASTAG = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "PRODASTAG", "N") = "Y", True, False)
        Wet2Wet = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "WT2WT", "N") = "Y", True, False)
        Wet2Wet_Advrate = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "WT2WT_ADV", "N") = "Y", True, False)
        Wet2Wet_pur = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "WT2WT_PUR", "N") = "Y", True, False)
        'Tag_search_Disable = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "TAG_SEARCH_DISABLE", "EP") = "P", True, False)
        MiscRemark = GetAdmindbSoftValuefromDt(dtSoftKeyss, "MISCREMARK", "S")
        OrdMcTax = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "ORDMCTAX", "Y") = "Y", True, False)
        OrdMcTaxEx = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "ORDMCTAXEX", "Y") = "Y", True, False)
        OrdMcTaxLessWt = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "ORDMCTAX-LESSWT", "Y") = "Y", True, False)
        AdjDiscCalc = GetAdmindbSoftValuefromDt(dtSoftKeyss, "ADJ-DISC-CALC", "WMSD")
        AdjDisc_MCFirst = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "ADJ-DISC-CALC-MC", "N") = "Y", True, False)
        AdjDiscStore = GetAdmindbSoftValuefromDt(dtSoftKeyss, "ADJ-DISC-STORE", "N")
        McWithWastage = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "MCWITHWASTAGE", "N") = "Y", True, False)
        ORDMCPOST2EXWT = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "ORDMCPOST2EXWT", "N") = "Y", True, False)
        SepAccPost = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "SEPACCPOST", "N") = "Y", True, False)
        SEPACCPOST_ITEM = GetAdmindbSoftValuefromDt(dtSoftKeyss, "SEPACCPOST_ITEM", "WMSDPO")
        DirectRepair = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "DIRECTREPAIR", "N") = "Y", True, False)
        HasEstPost = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "ESTPOST", "Y") = "Y", True, False)
        CallBackDtEst = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "CALLBACKDATEEST", "N") = "Y", True, False)
        PURALLOYPER = Val(GetAdmindbSoftValuefromDt(dtSoftKeyss, "PURALLOYPER", "0"))
        PUR22KT = Val(GetAdmindbSoftValuefromDt(dtSoftKeyss, "PUR22KT", "0"))
        PUR22KT_TYPE = Val(GetAdmindbSoftValuefromDt(dtSoftKeyss, "PUR22KT_TYPE", "0"))
        PUR22KT_TYPE_PURITY = Val(GetSoftValues("PUR22KT_TYPE_PURITY", "0"))
        DESIGNER_PROMPT = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "DESIGNER_PROMPT", "N") = "N", False, True)
        PURITYPURRATE = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "PURITYPURRATE", "N") = "N", False, True)
        FINALDISC_SEP = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "FINALDISC_SEP", "N") = "N", False, True)
        Finalamtlockbase = GetAdmindbSoftValuefromDt(dtSoftKeyss, "FINALAMT_LOCKBASE", "M")
        ORDMCACCTAX = GetAdmindbSoftValuefromDt(dtSoftKeyss, "ORDMCTAXACC", "MISC")
        ORDMCACC = GetAdmindbSoftValuefromDt(dtSoftKeyss, "ORDMCACC", "MISC")
        REPMCACC = GetAdmindbSoftValuefromDt(dtSoftKeyss, "REPMCACC", "")
        REPMCACC_G = GetAdmindbSoftValuefromDt(dtSoftKeyss, "REPMCACC_G", "")
        REPMCACC_S = GetAdmindbSoftValuefromDt(dtSoftKeyss, "REPMCACC_S", "")
        REPMCACC_P = GetAdmindbSoftValuefromDt(dtSoftKeyss, "REPMCACC_P", "")
        REPMCACC_D = GetAdmindbSoftValuefromDt(dtSoftKeyss, "REPMCACC_D", "")
        REPMCACC_T = GetAdmindbSoftValuefromDt(dtSoftKeyss, "REPMCACC_T", "")
        If ORDMCACC = "" Then ORDMCACC = "MISC"
        PURRATELOCK = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "PURRATELOCK", "N") = "Y", True, False)
        PURWASTLOCK = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "PURWASTLOCK", "N") = "Y", True, False)
        SEPSTUDTAXPOST = IIf(GetAdmindbSoftValuefromDt(dtSoftKeyss, "SEPSTUDTAXPOST", "N") = "Y", True, False)
        SEPRESCTRID = Val(GetAdmindbSoftValuefromDt(dtSoftKeyss, "SEPRESCTRID", "0"))
        If Val(objGPack.GetSqlValue("SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID=" & SEPRESCTRID.ToString(), , "0")) = 0 Then SEPRESCTRID = 0
        ED_COMPONENTS = GetAdmindbSoftValuefromDt(dtSoftKeyss, "ED_COMPONENTS", "")
        TCS_COMPONENT = GetAdmindbSoftValuefromDt(dtSoftKeyss, "TCS_COMPONENT", "")
        SALEVALUELIMIT = Val(GetAdmindbSoftValuefromDt(dtSoftKeyss, "SALEVALUELIMIT", "0"))
        TCSINCLOGVALUE = GetAdmindbSoftValuefromDt(dtSoftKeyss, "TCSINCLOGVALUE", "N")
        BARCODE2DSEP = GetSoftValues("BARCODE2DSEP", "")
        ROUNDOFF_PURAMT = GetSoftValues("ROUNDOFF_PURAMT", "N")
        MaxAsMinMC = GetSoftValues("MAXASMIN_MC", "N")
        MaxAsMinWAST = GetSoftValues("MAXASMIN_WAST", "N")
        DUPLESTASALE = GetSoftValues("DUPLESTASALE", "N")
        TCS_LIMIT = Val(GetSoftValues("TCS_LIMIT", "0"))
        TCS_PERCENT = GetSoftValues("TCS_PERCENT", "")
        TCS_BASEDON = GetSoftValues("TCS_BASEDON", "")
        Dim sp() As String = ED_COMPONENTS.Split(",")
        If ED_COMPONENTS <> "" Then
            If sp.Length = 1 Then
                EDC1ID = sp(0)
                EDC1PER = Val(objGPack.GetSqlValue("SELECT STAX FROM " & cnAdminDb & "..TAXMAST WHERE TAXCODE ='" & EDC1ID.ToString() & "'", , "0"))
                EXDUTY = True
            ElseIf sp.Length = 2 Then
                EDC1ID = sp(0)
                EDC2ID = sp(1)
                EDC1PER = Val(objGPack.GetSqlValue("SELECT STAX FROM " & cnAdminDb & "..TAXMAST WHERE TAXCODE ='" & EDC1ID.ToString() & "'", , "0"))
                EDC2PER = Val(objGPack.GetSqlValue("SELECT STAX FROM " & cnAdminDb & "..TAXMAST WHERE TAXCODE ='" & EDC2ID.ToString() & "'", , "0"))
                EXDUTY = True
            ElseIf sp.Length = 3 Then
                EDC1ID = sp(0)
                EDC2ID = sp(1)
                EDC3ID = sp(2)
                EDC1PER = Val(objGPack.GetSqlValue("SELECT STAX FROM " & cnAdminDb & "..TAXMAST WHERE TAXCODE ='" & EDC1ID.ToString() & "'", , "0"))
                EDC2PER = Val(objGPack.GetSqlValue("SELECT STAX FROM " & cnAdminDb & "..TAXMAST WHERE TAXCODE ='" & EDC2ID.ToString() & "'", , "0"))
                EDC3PER = Val(objGPack.GetSqlValue("SELECT STAX FROM " & cnAdminDb & "..TAXMAST WHERE TAXCODE ='" & EDC3ID.ToString() & "'", , "0"))
                EXDUTY = True
            End If
        End If
        Dim Tcs() As String = TCS_COMPONENT.Split(",")
        If Tcs.Length = 2 Then
            TCSID = Tcs(0)
            TCSVALUEFOR = Tcs(1)
            TCSPER = Val(objGPack.GetSqlValue("SELECT STAX FROM " & cnAdminDb & "..TAXMAST WHERE TAXCODE ='" & TCSID.ToString() & "'", , "0"))
            TCSCAL = True
        End If
    End Function

    Function GetSoftValues(ByVal ctlId As String, ByVal ddefault As String, Optional ByVal dbName As String = "DBADMINDB") As String
        dt.Clear()
        If dbName = "DBADMINDB" Then
            StrSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL"
            StrSql += " WHERE CTLID = '" & ctlId & "'"
        Else
            StrSql = " SELECT CTLTEXT FROM " & cnStockDb & "..SOFTCONTROLTRAN"
            StrSql += " WHERE CTLID = '" & ctlId & "'"
        End If
        Return UCase(objGPack.GetSqlValue(StrSql, , ddefault))
    End Function
End Class