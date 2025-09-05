Imports System.Data.OleDb
Public Class EstimationSoftKeys
    Private StrSql As String
    Private da As OleDbDataAdapter
    Private dt As New DataTable
    ''Bill Variales
    Public FINALDISC_SEP As Boolean
    Public TagLock As String
    Public AmtLok As String
    Public AmtLok_Pur As String
    Public SalZeroAmt As String
    Public SalDisc As String
    Public SalRateLok As String
    Public SalRateChk As String
    Public AutoLessWt As String
    Public RateDiscGrm As String
    Public RateDiscWt As String
    Public RateDiscIncWast As String
    Public MinValChk As String
    Public BillValueChk As String
    Public PurRateChk As String
    Public Tolerance_Rate As String
    Public RateChangeF4 As Boolean
    Public VatEdit As Boolean
    Public PurVatEdit As Boolean
    Public RoundOff_Gross As String
    Public RoundOff_Final As String
    Public RoundOff_Vat As String
    Public RoundOff_Sc As String
    Public ROUNDOFF_PURRATE As String
    Public ROUNDOFF_PURAMT As String
    Public PRODTAGSEP As Char
    Public PRODASTAG As Boolean
    Public McWithWastage As Boolean
    Public PURALLOYPER As Decimal
    Public GrossLok As Boolean
    Public PUR22KT As Decimal
    Public PUR22KT_TYPE As Integer
    Public PUR22KT_TYPE_PURITY As Decimal
    Public DESIGNER_PROMPT As Boolean
    Public HIDE_EST_ADDRESS As Boolean
    Public AdjDiscStore As String
    Public AdjDiscCalc As String
    Public ESTMARGINMULTI As Boolean
    Public ESTPURRATELOCK As Boolean
    Public ESTORDERTAG As Boolean
    Public ESTORDERTAG_DET As Boolean
    Public ED_COMPONENTS As String = Nothing
    Public EDC1ID As String = Nothing
    Public EDC2ID As String = Nothing
    Public EDC3ID As String = Nothing
    Public EDC1PER As Decimal = Nothing
    Public EDC2PER As Decimal = Nothing
    Public EDC3PER As Decimal = Nothing
    Public EXDUTY As Boolean = False
    Public ESTPURPCSZERO As Boolean = True
    Public MaxAsMinMC As String = "N"
    Public MaxAsMinWAST As String = "N"
    Public Wet2Wet As Boolean
    Public Estsalratelock As Boolean = True
    Public BARCODE2DSEP As String
    Public Finalamtlockbase As String = "M"
    Public StkType As String = "M"
    Public EXCLUDE_EXDUTY As String
    Public CALCEXDUTY As String

    Public Sub New()
        SetSoftValues()
    End Sub

    Function SetSoftValues() As Integer
        ''Getting Estimation
        CALCEXDUTY = GetSoftValues("CALCEXDUTY", "P,E")
        EXCLUDE_EXDUTY = GetSoftValues("EXCLUDE_EXDUTY", "")
        StkType = GetSoftValues("EXDUTY", "M")
        TagLock = GetSoftValues("ESTTAGLOCK", "N")
        AmtLok = GetSoftValues("ESTAMTLOK", "Y")
        AmtLok_Pur = GetSoftValues("ESTAMTLOK_PUR", "Y")
        SalDisc = GetSoftValues("ESTSALDISC", "Y")
        AutoLessWt = GetSoftValues("ESTAUTOLESSWT", "Y")
        RateDiscGrm = GetSoftValues("ESTRATEDISCGRM", "0")
        RateDiscWt = GetSoftValues("ESTRATEDISCWT", "N")
        RateDiscIncWast = GetSoftValues("ESTRATEDISCINCWAST", "N")
        GrossLok = IIf(GetSoftValues("GROSSLOK", "N") = "Y", True, False)
        MinValChk = GetSoftValues("ESTMINVALCHK", "N")
        MaxAsMinMC = GetSoftValues("ESTMAXASMIN_MC", "N")
        MaxAsMinWAST = GetSoftValues("ESTMAXASMIN_WAST", "N")
        BillValueChk = GetSoftValues("ESTBILLVALUECHK", "Y")
        PurRateChk = GetSoftValues("ESTPURRATECHK", "Y")
        Tolerance_Rate = GetSoftValues("ESTTOLERANCE_RATE", "20")
        RateChangeF4 = IIf(GetSoftValues("ESTRATECHANGEF4", "Y") = "Y", True, False)
        VatEdit = IIf(GetSoftValues("ESTVATEDIT", "N") = "Y", True, False)
        PurVatEdit = IIf(GetSoftValues("ESTPURVATEDIT", "Y") = "Y", True, False)
        RoundOff_Gross = GetSoftValues("ROUNDOFF-GROSS", "N")
        RoundOff_Final = GetSoftValues("ROUNDOFF-FINAL", "N")
        RoundOff_Vat = GetSoftValues("ROUNDOFF-VAT", "N")
        RoundOff_Sc = GetSoftValues("ROUNDOFF-SC", "N")
        ROUNDOFF_PURRATE = GetSoftValues("ROUNDOFF-PURRATE", "N")
        ROUNDOFF_PURAMT = GetSoftValues("ROUNDOFF_PURAMT", "N")
        PRODTAGSEP = GetSoftValues("PRODTAGSEP", "")
        PRODASTAG = IIf(GetSoftValues("PRODASTAG", "Y") = "Y", True, False)
        McWithWastage = IIf(GetSoftValues("MCWITHWASTAGE", "N") = "Y", True, False)
        PURALLOYPER = Val(GetSoftValues("PURALLOYPER", "0"))
        PUR22KT = Val(GetSoftValues("PUR22KT", "0"))
        PUR22KT_TYPE = Val(GetSoftValues("PUR22KT_TYPE", "0"))
        PUR22KT_TYPE_PURITY = Val(GetSoftValues("PUR22KT_TYPE_PURITY", "0"))
        DESIGNER_PROMPT = IIf(GetSoftValues("DESIGNER_PROMPT", "N") = "N", False, True)
        HIDE_EST_ADDRESS = IIf(GetSoftValues("HIDE_EST_ADDRESS", "N") = "N", False, True)
        AdjDiscStore = GetSoftValues("ADJ-DISC-STORE", "N")
        AdjDiscCalc = GetSoftValues("ADJ-DISC-CALC", "WMSD")
        ESTMARGINMULTI = IIf(GetSoftValues("ESTMARGINMULTI", "N") = "N", False, True)
        ESTPURRATELOCK = IIf(GetSoftValues("ESTPURRATELOCK", "N") = "Y", True, False)
        ESTORDERTAG = IIf(GetSoftValues("ESTORDERTAG", "Y") = "Y", True, False)
        ESTORDERTAG_DET = IIf(GetSoftValues("ESTORDERTAG_DETAIL", "Y") = "Y", True, False)
        ESTPURPCSZERO = IIf(GetSoftValues("ESTPURPCS_ZERO", "Y") = "Y", True, False)
        ED_COMPONENTS = GetSoftValues("ED_COMPONENTS", "")
        FINALDISC_SEP = IIf(GetSoftValues("FINALDISC_SEP", "N") = "N", False, True)
        Wet2Wet = IIf(GetSoftValues("WT2WT", "N") = "Y", True, False)
        Estsalratelock = IIf(GetSoftValues("ESTSALRATELOK", "Y") = "Y", True, False)
        BARCODE2DSEP = GetSoftValues("BARCODE2DSEP", "")
        Finalamtlockbase = GetSoftValues("FINALAMT_LOCKBASE", "M")
        Dim sp() As String = ED_COMPONENTS.Split(",")
        If ED_COMPONENTS <> "" Then
            If sp.Length = 1 Then
                EDC1ID = sp(0)
                EDC1PER = Val(objGPack.GetSqlValue("SELECT STAX FROM " & cnAdminDb & "..TAXMAST where TAXCODE ='" & EDC1ID.ToString() & "'", , "0"))
                EXDUTY = True
            ElseIf sp.Length = 2 Then
                EDC1ID = sp(0)
                EDC2ID = sp(1)
                EDC1PER = Val(objGPack.GetSqlValue("SELECT STAX FROM " & cnAdminDb & "..TAXMAST where TAXCODE ='" & EDC1ID.ToString() & "'", , "0"))
                EDC2PER = Val(objGPack.GetSqlValue("SELECT STAX FROM " & cnAdminDb & "..TAXMAST where TAXCODE ='" & EDC2ID.ToString() & "'", , "0"))
                EXDUTY = True
            ElseIf sp.Length = 3 Then
                EDC1ID = sp(0)
                EDC2ID = sp(1)
                EDC3ID = sp(2)
                EDC1PER = Val(objGPack.GetSqlValue("SELECT STAX FROM " & cnAdminDb & "..TAXMAST where TAXCODE ='" & EDC1ID.ToString() & "'", , "0"))
                EDC2PER = Val(objGPack.GetSqlValue("SELECT STAX FROM " & cnAdminDb & "..TAXMAST where TAXCODE ='" & EDC2ID.ToString() & "'", , "0"))
                EDC3PER = Val(objGPack.GetSqlValue("SELECT STAX FROM " & cnAdminDb & "..TAXMAST where TAXCODE ='" & EDC3ID.ToString() & "'", , "0"))
                EXDUTY = True
            End If
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
