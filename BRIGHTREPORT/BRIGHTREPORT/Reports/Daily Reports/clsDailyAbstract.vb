Imports System.Data.OleDb
Imports System.IO

Public Class clsDailyAbstract

    Public StrSql As String = ""
    Public Cmd As OleDbCommand
    Public CategoryWise As Boolean

    Public RateAvg As Boolean
    Public CategoryShortName As Boolean
    Public HomeSales As Boolean
    Public MiscRecPaySum As Boolean
    Public CancelBillDetail As Boolean
    Public ChitInfo As Boolean
    Public SeperateBeeds As Boolean
    Public WithVat As Boolean
    Public WithCashOpening As Boolean

    Public DotMatrix As Boolean

    Public RPT_SEPVAT_DABS As Boolean
    Public dtpFrom As DateTime
    Public dtpTo As DateTime
    Public strFilter As String
    Public SelectedCompanyId As String
    Public AvgGoldRate As Double
    Public AvgSiRate As Double
    Public MorGoldRate As Double
    Public EveGoldRate As Double
    Public MorSiRate As Double
    Public EveSiRate As Double
    Public CalcRedcRate As Double
    Public Purity As Double = 91.6
    Dim dtGetVal As DataTable


    Dim MetalAmtTotRec As Double = 0
    Dim MetalAmtTotPay As Double = 0

    Dim VatSales As Double = 0
    Dim VatSR As Double = 0

    Public strTitle As String = ""

    Public dtTableAdd As DataTable
    Dim drTableAdd As DataRow

    'Dim LenDesc As Integer = 15
    'Dim LenIPcs As Integer = 4
    'Dim LenIGwt As Integer = 10
    'Dim LenINwt As Integer = 10
    'Dim LenRPcs As Integer = 4
    'Dim LenRGwt As Integer = 10
    'Dim LenRNwt As Integer = 10
    'Dim LenRec As Integer = 12
    'Dim LenPay As Integer = 12
    'Dim LenAvg As Integer = 6
    'Dim Title As Integer = 75
    Dim LenDesc As Integer = 31
    Dim LenIPcs As Integer = 6
    Dim LenIGwt As Integer = 13
    Dim LenINwt As Integer = 13
    Dim LenRPcs As Integer = 6
    Dim LenRGwt As Integer = 13
    Dim LenRNwt As Integer = 13
    Dim LenRec As Integer = 13
    Dim LenPay As Integer = 13
    Dim LenAvg As Integer = 9
    Dim Title As Integer = 75



#Region "PrintRpt"
    Dim PgNo As Integer = 0
    Dim Linecounter As Integer = 0
    Private pBoldDoubleStart As String = Chr(27) + "E"
    Private pBoldDoubleEnd As String = Chr(27) + "F"
    Private pBoldStart As String = Chr(27) + "G"
    Private pBoldEnd As String = Chr(27) + "H"
    Private pUnderLineStart As String = Chr(27) + "-1"
    Private pUnderLineEnd As String = Chr(27) + "-0"
    Private pEjectPaper As String = Chr(12)
    Private pCondens As String
    Private oLenOfString As Integer = 15  ''It May change depends the column maxlength
    Private oSepChar As Char = "|"
    Private oSepForWholeColumn As Boolean
    Private oLinesPerPage As Integer = 60
    Private oWrite As System.IO.StreamWriter
    Private oFilePath As String = Nothing
    Private oReportTitle As String = Nothing
    Private oCharPerLine As Integer = 133
    Dim PreSpaceLine As Integer = oCharPerLine - (LenIPcs + LenIGwt + LenINwt + LenRPcs + LenRGwt + LenRNwt + LenRec + LenPay + LenAvg)
    Private oMaxCharPerLine As Integer = Nothing
    Private oReportHeaderStr As String = Nothing
    Private oReportMergeHeaderStr As String = Nothing
    Private oPrinterType As PrintType
    Private oPageNoCurrent As Integer = 0
    Private oRowsTotal As Integer
    Private WithEvents txtLineCounter As New TextBox
    Private oPrinterName As String
    Private oFileName As String
    Private oMultiCol As Integer
    Private oGridAvailableColumns As New List(Of String)
    Private oModeOfPrint As PrintMode = PrintMode.Condensed

#End Region

    Public Enum PrintType
        column_80
        column_130
    End Enum

    Public Enum PrintMode
        Auto
        Medium
        Condensed
        Micro
    End Enum

    Private Function PrintLine_WithOutWrite(ByVal len As Integer, Optional ByVal character As Char = "-") As String
        Dim line As String = Nothing
        For cnt As Integer = 1 To len
            line += character
        Next
        Return line
    End Function
    Private Sub PrintLine(ByVal len As Integer, Optional ByVal character As Char = "-")
        Dim line As String = Nothing
        For cnt As Integer = 1 To len
            line += character
        Next
        WriteLine(line)
    End Sub
    Private Sub WriteLine(ByVal str As String)
        str = Mid(str, 1, oCharPerLine + 1)
        ''Find Length
        Dim lenStr As String = str
        lenStr = lenStr.Replace(pBoldDoubleStart, "")
        lenStr = lenStr.Replace(pBoldDoubleEnd, "")
        lenStr = lenStr.Replace(pBoldStart, "")
        lenStr = lenStr.Replace(pBoldEnd, "")
        lenStr = lenStr.Replace(pEjectPaper, "")
        If oMaxCharPerLine < lenStr.Length Then oMaxCharPerLine = lenStr.Length
        oWrite.WriteLine(str)
        Linecounter = Linecounter + 1
        If Linecounter >= oLinesPerPage - 1 Then
            oPageNoCurrent = oPageNoCurrent + 1
            oWrite.WriteLine(RSet("Page : " + oPageNoCurrent.ToString(), (oCharPerLine) / 2))
            oWrite.WriteLine(Chr(12))
            Linecounter = 0
        End If
        'If Linecounter = 34 Then
        '    WriteLine(Chr(12))
        'End If
        'txtLineCounter.Text = Val(txtLineCounter.Text) + 1
    End Sub

    Sub clsDailyAbstract()

    End Sub

    Public Function Print() As Integer





    End Function

    Public Sub GetRate(ByVal RDate As DateTime)
        StrSql = vbCrLf + " SELECT CONVERT(NUMERIC(15,2),SUM(SRATE) / COUNT(*)) RATE FROM " & cnAdminDb & "..RATEMAST "
        StrSql += vbCrLf + " WHERE RDATE BETWEEN '" + dtpFrom.Date.ToString("yyyy-MM-dd") + "' AND '" + dtpTo.Date.ToString("yyyy-MM-dd") + "' AND PURITY BETWEEN 91.6 AND 92"
        StrSql += vbCrLf + " AND METALID IN ('G')"
        dtGetVal = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtGetVal)
        If dtGetVal.Rows.Count > 0 Then
            AvgGoldRate = IIf(dtGetVal.Rows(0).Item("RATE").ToString = "", "0", dtGetVal.Rows(0).Item("RATE").ToString)
        End If

        StrSql = vbCrLf + " SELECT CONVERT(NUMERIC(15,2),SUM(SRATE) / COUNT(*)) RATE FROM " & cnAdminDb & "..RATEMAST "
        StrSql += vbCrLf + " WHERE RDATE BETWEEN '" + dtpFrom.Date.ToString("yyyy-MM-dd") + "' AND '" + dtpTo.Date.ToString("yyyy-MM-dd") + "' AND PURITY BETWEEN 91.6 AND 92"
        StrSql += vbCrLf + " AND METALID IN ('S')"
        dtGetVal = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtGetVal)
        If dtGetVal.Rows.Count > 0 Then
            AvgSiRate = IIf(dtGetVal.Rows(0).Item("RATE").ToString = "", "0", dtGetVal.Rows(0).Item("RATE").ToString)
        End If

        StrSql = vbCrLf + " SELECT TOP 1 SRATE,PURITY FROM " & cnAdminDb & "..RATEMAST "
        StrSql += vbCrLf + " WHERE RDATE BETWEEN '" + dtpFrom.Date.ToString("yyyy-MM-dd") + "' AND '" + dtpTo.Date.ToString("yyyy-MM-dd") + "' AND PURITY BETWEEN 91.6 AND 92"
        StrSql += vbCrLf + " AND METALID IN ('G')"
        StrSql += vbCrLf + " ORDER BY RATEGROUP"
        dtGetVal = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtGetVal)
        If dtGetVal.Rows.Count > 0 Then
            Purity = IIf(dtGetVal.Rows(0).Item("PURITY").ToString = "", "0", dtGetVal.Rows(0).Item("PURITY").ToString)
            MorGoldRate = IIf(dtGetVal.Rows(0).Item("SRATE").ToString = "", "0", dtGetVal.Rows(0).Item("SRATE").ToString)
        End If

        StrSql = vbCrLf + " SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST "
        StrSql += vbCrLf + " WHERE RDATE BETWEEN '" + dtpFrom.Date.ToString("yyyy-MM-dd") + "' AND '" + dtpTo.Date.ToString("yyyy-MM-dd") + "' AND PURITY BETWEEN 91.6 AND 92"
        StrSql += vbCrLf + " AND METALID IN ('G')"
        StrSql += vbCrLf + " ORDER BY RATEGROUP DESC"
        dtGetVal = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtGetVal)
        If dtGetVal.Rows.Count > 0 Then
            EveGoldRate = IIf(dtGetVal.Rows(0).Item("SRATE").ToString = "", "0", dtGetVal.Rows(0).Item("SRATE").ToString)
        End If

        StrSql = vbCrLf + " SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST "
        StrSql += vbCrLf + " WHERE RDATE BETWEEN '" + dtpFrom.Date.ToString("yyyy-MM-dd") + "' AND '" + dtpTo.Date.ToString("yyyy-MM-dd") + "' AND PURITY BETWEEN 91.6 AND 92"
        StrSql += vbCrLf + " AND METALID IN ('S')"
        StrSql += vbCrLf + " ORDER BY RATEGROUP"
        dtGetVal = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtGetVal)
        If dtGetVal.Rows.Count > 0 Then
            MorSiRate = IIf(dtGetVal.Rows(0).Item("SRATE").ToString = "", "0", dtGetVal.Rows(0).Item("SRATE").ToString)
        End If

        StrSql = vbCrLf + " SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST "
        StrSql += vbCrLf + " WHERE RDATE BETWEEN '" + dtpFrom.Date.ToString("yyyy-MM-dd") + "' AND '" + dtpTo.Date.ToString("yyyy-MM-dd") + "' AND PURITY BETWEEN 91.6 AND 92"
        StrSql += vbCrLf + " AND METALID IN ('S')"
        StrSql += vbCrLf + " ORDER BY RATEGROUP DESC"
        dtGetVal = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtGetVal)
        If dtGetVal.Rows.Count > 0 Then
            EveSiRate = IIf(dtGetVal.Rows(0).Item("SRATE").ToString = "", "0", dtGetVal.Rows(0).Item("SRATE").ToString)
        End If

        If DotMatrix = True Then
            Header()
        Else
            strTitle = "Transaction Summary - " & dtpFrom.Date.ToString("dd/MM/yyyy") & " To " & dtpTo.Date.ToString("dd/MM/yyyy")
            strTitle += vbCrLf & "Rate : Gold(" & Purity & "):" & AvgGoldRate.ToString("0.00") & " Mor : " & MorGoldRate.ToString("0.00") & " Eve : " & EveGoldRate.ToString("0.00") & " Silver : " & AvgSiRate.ToString("0.00")
            dtTableAdd = New DataTable
            With dtTableAdd
                .Columns.Add("DESC", GetType(String))
                .Columns.Add("IPCS", GetType(String))
                .Columns.Add("IGWT", GetType(String))
                .Columns.Add("INWT", GetType(String))
                .Columns.Add("RPCS", GetType(String))
                .Columns.Add("RGWT", GetType(String))
                .Columns.Add("RNWT", GetType(String))
                .Columns.Add("REC", GetType(String))
                .Columns.Add("PAY", GetType(String))
                .Columns.Add("AVERAGE", GetType(String))
                .Columns.Add("COLHEAD", GetType(String))
                .Columns.Add("RESULT1", GetType(String))
                .Columns.Add("SCROLL", GetType(String))
                .Columns.Add("KEYNO", GetType(Int32))
                .Columns("KEYNO").AutoIncrement = True
                .Columns("KEYNO").AutoIncrementSeed = 0
                .Columns("KEYNO").AutoIncrementStep = 1

                .Columns("DESC").Caption = "DESCRIPTION"
                .Columns("IPCS").Caption = "PCS"
                .Columns("IGWT").Caption = "GRSWT"
                .Columns("INWT").Caption = "NETWT"
                .Columns("RPCS").Caption = "PCS"
                .Columns("RGWT").Caption = "GRSWT"
                .Columns("RNWT").Caption = "NETWT"
                .Columns("REC").Caption = "RECEIPT"
                .Columns("PAY").Caption = "PAYMENT"
                .Columns("AVERAGE").Caption = "AVERAGE"
                .Columns("COLHEAD").Caption = ""
                .Columns("RESULT1").Caption = ""
                .Columns("SCROLL").Caption = ""
            End With
        End If
    End Sub

    Public Function Header() As Integer

        oFileName = "TranSummary.txt"
        oFilePath = "C:\REPORTS\" + oFileName


        If IO.File.Exists(oFilePath) = True Then
            File.Delete(oFilePath)
        End If

        oWrite = System.IO.File.CreateText(oFilePath)


        oWrite.Write(Chr(27) + Chr(67) + Chr(36))
        WriteLine("Transaction Summary - " & dtpFrom.Date.ToString("dd/MM/yyyy") & " To " & dtpTo.Date.ToString("dd/MM/yyyy"))
        WriteLine("Rate : Gold(" & Purity & "):" & AvgGoldRate.ToString("0.00") & " Mor : " & MorGoldRate.ToString("0.00") & " Eve : " & EveGoldRate.ToString("0.00") & " Silver : " & AvgSiRate.ToString("0.00"))

        PrintLine(oCharPerLine, "-")
        WriteLine(LSet("DESCRIPTION", LenDesc) + RSet("ISSUE" + PrintLine_WithOutWrite(9, " "), LenIPcs + LenIGwt + LenINwt) + RSet("RECEIPT" + PrintLine_WithOutWrite(9, " "), LenRPcs + LenRGwt + LenRNwt) + RSet("AMOUNT" + PrintLine_WithOutWrite(11, " "), LenRec + LenPay + LenAvg))
        WriteLine(LSet("", LenDesc) + RSet("PCS", LenIPcs) + RSet("GRSWT", LenIGwt) + RSet("NETWT", LenINwt) + RSet("PCS", LenRPcs) + RSet("GRSWT", LenRGwt) + RSet("NETWT", LenRNwt) + RSet("RECEIPT", LenRec) + RSet("PAYMENT", LenPay) + RSet("AVG.", LenAvg))
        PrintLine(oCharPerLine, "-")
    End Function

    Public Sub Dispo()
        oWrite.Flush()
        oWrite.Close()
        pCondens = Chr(27) + Chr(18)
        If oModeOfPrint = PrintMode.Auto Then
            If oMaxCharPerLine > 80 Then
                pCondens += Chr(15)
                'Select Case oPrinterType
                '    Case PrintType.column_80
                '        If oMaxCharPerLine < 95 Then
                '            pCondens += Chr(27) + "M"
                '        ElseIf oMaxCharPerLine < 136 Then
                '            pCondens += Chr(15)
                '        Else
                '            pCondens += Chr(27) + "M" + Chr(15)
                '        End If
                '    Case PrintType.column_130
                '        If oMaxCharPerLine < 110 Then
                '            pCondens += Chr(27) + "M"
                '        ElseIf oMaxCharPerLine < 154 Then
                '            pCondens += Chr(15)
                '        Else
                '            pCondens += Chr(27) + "M" + Chr(15)
                '        End If
                'End Select
            Else
                GoTo Exit1
            End If
        ElseIf oModeOfPrint = PrintMode.Condensed Then
            pCondens += Chr(15)
        ElseIf oModeOfPrint = PrintMode.Medium Then
            'pCondens += Chr(27) + "M"
            pCondens += Chr(15)
        ElseIf oModeOfPrint = PrintMode.Micro Then
            'pCondens += Chr(27) + "M" + Chr(15)
            pCondens += Chr(15)
        End If
        Dim filestm As FileStream
        filestm = New FileStream(oFilePath, FileMode.Open, FileAccess.Read, FileShare.None)
        Dim sReader As StreamReader = New StreamReader(filestm)
        sReader.BaseStream.Seek(0, SeekOrigin.Begin)
        Dim firstLine As String = sReader.ReadLine
        sReader.Close()

        filestm = New FileStream(oFilePath, FileMode.Open, FileAccess.Write, FileShare.None)
        Dim sWriter As StreamWriter = New StreamWriter(filestm)
        sWriter.BaseStream.Seek(0, SeekOrigin.Begin)

        sWriter.WriteLine(pCondens + firstLine)
        sWriter.Flush()
        sWriter.Close()

        Dim objPrint As New frmPrint(oFileName.Replace(".txt", ""))
        objPrint.ShowDialog()

Exit1:

    End Sub

    Public Sub AmtCreRecAdv()
        StrSql = " IF OBJECT_ID('TEMPCRREADVTYPES') IS NOT NULL"
        StrSql += "     DROP TABLE TEMPCRREADVTYPES"
        StrSql += "  CREATE TABLE TEMPCRREADVTYPES(TRANTYPE VARCHAR(1),PAYMODE VARCHAR(25),RECPAY VARCHAR(1),REFMODE VARCHAR(10),DESCRIPTION VARCHAR(30))"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " INSERT INTO TEMPCRREADVTYPES(TRANTYPE,PAYMODE,RECPAY,DESCRIPTION)"
        StrSql += " SELECT 'D' TRANTYPE,'DU' PAYMODE,'P' RECPAY,'CREDIT BILL DETAIL(S)' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'D' TRANTYPE,'DP' PAYMODE,'P' RECPAY,'CREDIT BILL DETAIL(S)' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'D' TRANTYPE,'DR' PAYMODE,'R' RECPAY,'RECEIPT AGAINST CREDIT BILL(S)' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'D' TRANTYPE,'DU' PAYMODE,'R' RECPAY,'RECEIPT AGAINST CREDIT BILL(S)' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'D' TRANTYPE,'IDU' PAYMODE,'P' RECPAY,'TO BE BILL DETAIL(S)' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'D' TRANTYPE,'IDP' PAYMODE,'P' RECPAY,'TO BE BILL DETAIL(S)' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'D' TRANTYPE,'IDU' PAYMODE,'R' RECPAY,'TO BE RECEIPTS' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'D' TRANTYPE,'IDR' PAYMODE,'R' RECPAY,'TO BE RECEIPTS' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'AR' PAYMODE,'R' RECPAY,'ADVANCE RECEIVED' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'AA' PAYMODE,'P' RECPAY,'ADVANCE ADJUSTED' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'AP' PAYMODE,'P' RECPAY,'ADVANCE ADJUSTED' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'IAR' PAYMODE,'R' RECPAY,'ADVANCE RECEIVED' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'IAA' PAYMODE,'P' RECPAY,'ADVANCE ADJUSTED' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'IAP' PAYMODE,'P' RECPAY,'ADVANCE ADJUSTED' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'OAA' PAYMODE,'P' RECPAY,'ORDER ADVANCE ADJUSTED' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'OAP' PAYMODE,'P' RECPAY,'ORDER ADVANCE ADJUSTED' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'OAR' PAYMODE,'R' RECPAY,'ORDER ADVANCE RECEIVED' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'OOR' PAYMODE,'R' RECPAY,'ORDER ADVANCE RECEIVED' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'RAA' PAYMODE,'P' RECPAY,'REPAIR ADVANCE ADJUSTED' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'RAP' PAYMODE,'P' RECPAY,'REPAIR ADVANCE ADJUSTED' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'RAR' PAYMODE,'R' RECPAY,'REPAIR ADVANCE RECEIVED' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'ROR' PAYMODE,'R' RECPAY,'REPAIR ADVANCE RECEIVED' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'T' TRANTYPE,'MR' PAYMODE,'R' RECPAY,'MISC RECEIPT' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'T' TRANTYPE,'MP' PAYMODE,'P' RECPAY,'MISC PAYMENT' DESCRIPTION"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " IF OBJECT_ID('TEMPCRREADVTYPE') IS NOT NULL"
        StrSql += "     DROP TABLE TEMPCRREADVTYPE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " SELECT"
        StrSql += vbCrLf + " 	BATCHNO,SUBSTRING(RUNNO,6,20)RUNNO,TRANDATE,RECPAY,AMOUNT,TRANTYPE,CONVERT(VARCHAR(5),ISNULL((SELECT TOP 1 'I' FROM " & cnAdminDb & "..ITEMDETAIL WHERE BATCHNO = O.BATCHNO),'') + PAYMODE) PAYMODE"
        StrSql += vbCrLf + " 	,CONVERT(VARCHAR(250),SUBSTRING(RUNNO,6,20) +' '+ (SELECT (ISNULL(PNAME,'') + ' ' + ISNULL(INITIAL,'') + ' ' + ISNULL(CASE WHEN ISNULL(MOBILE,'') != '' THEN MOBILE ELSE PHONERES END,''))  FROM "
        StrSql += vbCrLf + " 	" & cnAdminDb & "..PERSONALINFO WHERE SNO = (SELECT TOP 1 PSNO FROM "
        StrSql += vbCrLf + " " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = O.BATCHNO)) + CASE WHEN DUEDATE IS NOT NULL THEN ' DUE DATE :' + CONVERT(VARCHAR,DUEDATE,103) ELSE '' END)	DESCRIPTION"
        StrSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..ITEMDETAIL WHERE BATCHNO = O.BATCHNO) ITEMBATCHNO"
        StrSql += vbCrLf + " INTO TEMPCRREADVTYPE"
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS O"
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Date.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Date.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += vbCrLf + " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += strFilter
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " UPDATE MASTER..TEMPCRREADVTYPE SET PAYMODE = SUBSTRING(RUNNO,6,1) + PAYMODE WHERE SUBSTRING(RUNNO,6,1) IN ('O','R')"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " IF OBJECT_ID('TEMPCRREADVFINAL') IS NOT NULL"
        StrSql += vbCrLf + " 	DROP TABLE TEMPCRREADVFINAL"
        StrSql += vbCrLf + " SELECT "
        StrSql += vbCrLf + " 	' ' + DESCRIPTION DESCRIPTION"
        StrSql += vbCrLf + " 	,CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE NULL END RECEIPT"
        StrSql += vbCrLf + " 	,CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE NULL END PAYMENT"
        StrSql += vbCrLf + " 	,(SELECT DESCRIPTION FROM MASTER..TEMPCRREADVTYPES WHERE ISNULL(TRANTYPE,'') = T.TRANTYPE AND PAYMODE = T.PAYMODE AND RECPAY = T.RECPAY) DESCTITLE"
        StrSql += vbCrLf + " INTO TEMPCRREADVFINAL"
        StrSql += vbCrLf + " FROM MASTER..TEMPCRREADVTYPE AS T"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        'StrSql = vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        'StrSql += vbCrLf + " (DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,COLHEAD,RESULT1) "

        Dim ReceiptAmt As Double = 0
        Dim PaymentAmt As Double = 0

        StrSql = vbCrLf + " SELECT DESCRIPTION"
        StrSql += vbCrLf + " 	,NULL ISSPCS,NULL ISSGRSWT,NULL ISSNETWT,NULL RECPCS,NULL RECGRSWT,NULL RECNETWT,RECEIPT,PAYMENT,NULL AVERAGE,COLHEAD,1 RESULT1"
        StrSql += vbCrLf + " FROM ("
        StrSql += vbCrLf + " SELECT "
        StrSql += vbCrLf + " 	DESCTITLE + ' => ' + CONVERT(VARCHAR,ABS(ISNULL(SUM(RECEIPT),0) - ISNULL(SUM(PAYMENT),0))) DESCRIPTION"
        StrSql += vbCrLf + " 	,NULL RECEIPT,NULL PAYMENT,DESCTITLE,'T' COLHEAD,1 SEP"
        StrSql += vbCrLf + " FROM TEMPCRREADVFINAL"
        StrSql += vbCrLf + " GROUP BY DESCTITLE HAVING ISNULL(SUM(RECEIPT),0) - ISNULL(SUM(PAYMENT),0) <> 0"
        StrSql += vbCrLf + " UNION ALL"
        StrSql += vbCrLf + " SELECT DESCRIPTION,CASE WHEN ISNULL(RECEIPT,0) = 0 THEN NULL ELSE RECEIPT END RECEIPT,CASE WHEN ISNULL(PAYMENT,0) = 0 THEN NULL ELSE PAYMENT END PAYMENT,DESCTITLE,'' COLHEAD,2 SEP FROM TEMPCRREADVFINAL WHERE ISNULL(RECEIPT,0) <> 0 OR ISNULL(PAYMENT,0) <> 0"
        StrSql += vbCrLf + " )X"
        StrSql += vbCrLf + " ORDER BY DESCTITLE,SEP,DESCRIPTION"
        Dim dtAmtCr As New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtAmtCr)
        If dtAmtCr.Rows.Count > 0 Then
            For cnt As Integer = 0 To dtAmtCr.Rows.Count - 1
                If DotMatrix = True Then
                    If dtAmtCr.Rows(cnt).Item("COLHEAD").ToString = "T" Then
                        oWrite.Write(pBoldDoubleStart)
                        WriteLine(LSet(dtAmtCr.Rows(cnt).Item("DESCRIPTION").ToString, oCharPerLine))
                        oWrite.Write(pBoldDoubleEnd)
                    Else
                        'WriteLine(LSet(dtAmtCr.Rows(cnt).Item("DESCRIPTION").ToString, 15) + RSet(dtAmtCr.Rows(cnt).Item("ISSPCS").ToString, 4) + RSet(dtAmtCr.Rows(cnt).Item("ISSGRSWT").ToString, 10) + RSet(dtAmtCr.Rows(cnt).Item("ISSNETWT").ToString, 10) + RSet(dtAmtCr.Rows(cnt).Item("RECPCS").ToString, 4) + RSet(dtAmtCr.Rows(cnt).Item("RECGRSWT").ToString, 10) + RSet(dtAmtCr.Rows(cnt).Item("RECNETWT").ToString, 10) + RSet(dtAmtCr.Rows(cnt).Item("RECEIPT").ToString, 12) + RSet(dtAmtCr.Rows(cnt).Item("PAYMENT").ToString, 12) + RSet(dtAmtCr.Rows(cnt).Item("AVERAGE").ToString, 6))
                        WriteLine(LSet(dtAmtCr.Rows(cnt).Item("DESCRIPTION").ToString, LenDesc) + RSet(dtAmtCr.Rows(cnt).Item("ISSPCS").ToString, LenIPcs) + RSet(dtAmtCr.Rows(cnt).Item("ISSGRSWT").ToString, LenIGwt) + RSet(dtAmtCr.Rows(cnt).Item("ISSNETWT").ToString, LenINwt) + RSet(dtAmtCr.Rows(cnt).Item("RECPCS").ToString, LenRPcs) + RSet(dtAmtCr.Rows(cnt).Item("RECGRSWT").ToString, LenRGwt) + RSet(dtAmtCr.Rows(cnt).Item("RECNETWT").ToString, LenRNwt) + RSet(dtAmtCr.Rows(cnt).Item("RECEIPT").ToString, LenRec) + RSet(dtAmtCr.Rows(cnt).Item("PAYMENT").ToString, LenPay) + RSet(dtAmtCr.Rows(cnt).Item("AVERAGE").ToString, LenAvg))
                    End If
                Else
                    drTableAdd = dtTableAdd.NewRow
                    drTableAdd.Item("DESC") = dtAmtCr.Rows(cnt).Item("DESCRIPTION").ToString
                    drTableAdd.Item("IPCS") = dtAmtCr.Rows(cnt).Item("ISSPCS").ToString
                    drTableAdd.Item("IGWT") = dtAmtCr.Rows(cnt).Item("ISSGRSWT").ToString
                    drTableAdd.Item("INWT") = dtAmtCr.Rows(cnt).Item("ISSNETWT").ToString
                    drTableAdd.Item("RPCS") = dtAmtCr.Rows(cnt).Item("RECPCS").ToString
                    drTableAdd.Item("RGWT") = dtAmtCr.Rows(cnt).Item("RECGRSWT").ToString
                    drTableAdd.Item("RNWT") = dtAmtCr.Rows(cnt).Item("RECNETWT").ToString
                    drTableAdd.Item("REC") = dtAmtCr.Rows(cnt).Item("RECEIPT").ToString
                    drTableAdd.Item("PAY") = dtAmtCr.Rows(cnt).Item("PAYMENT").ToString
                    drTableAdd.Item("AVERAGE") = dtAmtCr.Rows(cnt).Item("AVERAGE").ToString
                    drTableAdd.Item("RESULT1") = dtAmtCr.Rows(cnt).Item("RESULT1").ToString
                    drTableAdd.Item("COLHEAD") = dtAmtCr.Rows(cnt).Item("COLHEAD").ToString
                    dtTableAdd.Rows.Add(drTableAdd)
                    dtTableAdd.AcceptChanges()
                End If
                ReceiptAmt += Convert.ToDouble(IIf(dtAmtCr.Rows(cnt).Item("RECEIPT").ToString = "", "0", dtAmtCr.Rows(cnt).Item("RECEIPT").ToString))
                PaymentAmt += Convert.ToDouble(IIf(dtAmtCr.Rows(cnt).Item("PAYMENT").ToString = "", "0", dtAmtCr.Rows(cnt).Item("PAYMENT").ToString))
            Next cnt
        End If


        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()

        'StrSql = vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        'StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,COLHEAD,RESULT1) "

        StrSql = vbCrLf + " SELECT CASE ISNULL(PAYMODE,'') WHEN 'HC' THEN 'HANDLING CHARGES' WHEN 'DI' THEN 'DISCOUNT' WHEN 'RO' THEN 'ROUND-OFF' END DESCRIPTION"
        StrSql += vbCrLf + "    ,CASE WHEN ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT END),0) - ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT END),0) > 0 THEN ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT END),0) - ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT END),0) ELSE NULL END RECEIPT"
        StrSql += vbCrLf + "    ,CASE WHEN ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT END),0) - ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT END),0) > 0 THEN ISNULL(SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT END),0) - ISNULL(SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT END),0) ELSE NULL END PAYMENT"
        StrSql += vbCrLf + "    ,'S' COLHEAD,CASE ISNULL(PAYMODE,'') WHEN 'HC' THEN 2 WHEN 'DI' THEN 3 WHEN 'RO' THEN 4 END RESULT1"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN "
        StrSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Date.ToString("yyyy-MM-dd") & "' AND "
        StrSql += vbCrLf + " '" & dtpTo.Date.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = '' "
        StrSql += vbCrLf + strFilter
        StrSql += vbCrLf + " AND ISNULL(PAYMODE,'') IN ('HC','DI','RO') AND ISNULL(CANCEL,'') = ''"
        StrSql += vbCrLf + " GROUP BY PAYMODE"
        dtAmtCr = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtAmtCr)
        If dtAmtCr.Rows.Count > 0 Then
            For cnt As Integer = 0 To dtAmtCr.Rows.Count - 1
                If DotMatrix = True Then
                    oWrite.Write(pBoldDoubleStart)
                    'WriteLine(LSet(dtAmtCr.Rows(cnt).Item("DESCRIPTION").ToString, 15) + RSet(" ", 4) + RSet(" ", 10) + RSet(" ", 10) + RSet(" ", 4) + RSet(" ", 10) + RSet(" ", 10) + RSet(dtAmtCr.Rows(cnt).Item("RECEIPT").ToString, 12) + RSet(dtAmtCr.Rows(cnt).Item("PAYMENT").ToString, 12) + RSet(" ", 6))
                    WriteLine(LSet(dtAmtCr.Rows(cnt).Item("DESCRIPTION").ToString, LenDesc) + RSet("", LenIPcs) + RSet("", LenIGwt) + RSet("", LenINwt) + RSet("", LenRPcs) + RSet("", LenRGwt) + RSet("", LenRNwt) + RSet(dtAmtCr.Rows(cnt).Item("RECEIPT").ToString, LenRec) + RSet(dtAmtCr.Rows(cnt).Item("PAYMENT").ToString, LenPay) + RSet("", LenAvg))
                    oWrite.Write(pBoldDoubleEnd)
                Else
                    drTableAdd = dtTableAdd.NewRow
                    drTableAdd.Item("DESC") = dtAmtCr.Rows(cnt).Item("DESCRIPTION").ToString
                    drTableAdd.Item("IPCS") = ""
                    drTableAdd.Item("IGWT") = ""
                    drTableAdd.Item("INWT") = ""
                    drTableAdd.Item("RPCS") = ""
                    drTableAdd.Item("RGWT") = ""
                    drTableAdd.Item("RNWT") = ""
                    drTableAdd.Item("REC") = dtAmtCr.Rows(cnt).Item("RECEIPT").ToString
                    drTableAdd.Item("PAY") = dtAmtCr.Rows(cnt).Item("PAYMENT").ToString
                    drTableAdd.Item("AVERAGE") = ""
                    drTableAdd.Item("RESULT1") = dtAmtCr.Rows(cnt).Item("RESULT1").ToString
                    drTableAdd.Item("COLHEAD") = dtAmtCr.Rows(cnt).Item("COLHEAD").ToString
                    dtTableAdd.Rows.Add(drTableAdd)
                    dtTableAdd.AcceptChanges()
                End If
                ReceiptAmt += Convert.ToDouble(IIf(dtAmtCr.Rows(cnt).Item("RECEIPT").ToString = "", "0", dtAmtCr.Rows(cnt).Item("RECEIPT").ToString))
                PaymentAmt += Convert.ToDouble(IIf(dtAmtCr.Rows(cnt).Item("PAYMENT").ToString = "", "0", dtAmtCr.Rows(cnt).Item("PAYMENT").ToString))
            Next cnt
        End If

        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()

        MetalAmtTotRec += ReceiptAmt
        MetalAmtTotPay += PaymentAmt

        'StrSql = vbCrLf + " INSERT INTO TEMP" & systemId & "SASRPU"
        'StrSql += vbCrLf + " (DESCRIPTION,RECEIPT,PAYMENT,COLHEAD,RESULT1) "
        'StrSql = vbCrLf + "    SELECT 'TOTAL = >' DESCRIPTION,SUM(RECEIPT) RECEIPT,SUM(PAYMENT) PAYMENT,'G' COLHEAD,5 RESULT FROM TEMP" & systemId & "SASRPU WHERE RESULT1 IN (1,2,3,4)"
        'dtAmtCr = New DataTable
        'da = New OleDbDataAdapter(StrSql, cn)
        'da.Fill(dtAmtCr)
        'If dtAmtCr.Rows.Count > 0 Then
        '    For cnt As Integer = 0 To dtAmtCr.Rows.Count - 1
        If DotMatrix = True Then
            ' oWrite.Write(pBoldDoubleStart)
            WriteLine(PrintLine_WithOutWrite(oCharPerLine, "-"))
            WriteLine(LSet("TOTAL = >", LenDesc) + RSet(" ", LenIPcs) + RSet(" ", LenIGwt) + RSet(" ", LenINwt) + RSet(" ", LenRPcs) + RSet(" ", LenRGwt) + RSet(" ", LenRNwt) + RSet(MetalAmtTotRec.ToString("0.00"), LenRec) + RSet(MetalAmtTotPay.ToString("0.00"), LenPay) + RSet(" ", LenAvg))
            WriteLine(PrintLine_WithOutWrite(oCharPerLine, "-"))
            'oWrite.Write(pBoldDoubleEnd)
        Else
            drTableAdd = dtTableAdd.NewRow
            drTableAdd.Item("DESC") = "TOTAL = >"
            drTableAdd.Item("IPCS") = ""
            drTableAdd.Item("IGWT") = ""
            drTableAdd.Item("INWT") = ""
            drTableAdd.Item("RPCS") = ""
            drTableAdd.Item("RGWT") = ""
            drTableAdd.Item("RNWT") = ""
            drTableAdd.Item("REC") = MetalAmtTotRec.ToString("0.00")
            drTableAdd.Item("PAY") = MetalAmtTotPay.ToString("0.00")
            drTableAdd.Item("AVERAGE") = ""
            drTableAdd.Item("RESULT1") = 5
            drTableAdd.Item("COLHEAD") = "G"
            dtTableAdd.Rows.Add(drTableAdd)
            dtTableAdd.AcceptChanges()
        End If
        '    Next cnt
        'End If
        'StrSql = vbCrLf + "    SELECT 'BALANCE = >' DESCRIPTION,CASE WHEN ISNULL(SUM(RECEIPT),0) - ISNULL(SUM(PAYMENT),0) > 0 THEN ISNULL(SUM(RECEIPT),0) - ISNULL(SUM(PAYMENT),0) ELSE NULL END RECEIPT,CASE WHEN ISNULL(SUM(PAYMENT),0) - ISNULL(SUM(RECEIPT),0) > 0 THEN ISNULL(SUM(PAYMENT),0) - ISNULL(SUM(RECEIPT),0) ELSE NULL END PAYMENT,'G' COLHEAD,6 RESULT FROM TEMP" & systemId & "SASRPU WHERE RESULT1 IN (5)"
        'dtAmtCr = New DataTable
        'da = New OleDbDataAdapter(StrSql, cn)
        'da.Fill(dtAmtCr)
        'If dtAmtCr.Rows.Count > 0 Then
        '    For cnt As Integer = 0 To dtAmtCr.Rows.Count - 1
        If DotMatrix = True Then
            ' oWrite.Write(pBoldDoubleStart)
            WriteLine(PrintLine_WithOutWrite(oCharPerLine, "-"))
            WriteLine(LSet("BALANCE =>", LenDesc) + RSet(" ", LenIPcs) + RSet(" ", LenIGwt) + RSet(" ", LenINwt) + RSet(" ", LenRPcs) + RSet(" ", LenRGwt) + RSet(" ", LenRNwt) + RSet(Convert.ToDouble(IIf(MetalAmtTotRec > MetalAmtTotPay, MetalAmtTotRec - MetalAmtTotPay, 0)).ToString("0.00"), LenRec) + RSet(Convert.ToDouble(IIf(MetalAmtTotRec < MetalAmtTotPay, MetalAmtTotPay - MetalAmtTotRec, 0)).ToString("0.00"), LenPay) + RSet(" ", LenAvg))
            WriteLine(PrintLine_WithOutWrite(oCharPerLine, "-"))
            'oWrite.Write(pBoldDoubleEnd)
        Else
            drTableAdd = dtTableAdd.NewRow
            drTableAdd.Item("DESC") = "BALANCE =>"
            drTableAdd.Item("IPCS") = ""
            drTableAdd.Item("IGWT") = ""
            drTableAdd.Item("INWT") = ""
            drTableAdd.Item("RPCS") = ""
            drTableAdd.Item("RGWT") = ""
            drTableAdd.Item("RNWT") = ""
            drTableAdd.Item("REC") = IIf(MetalAmtTotRec > MetalAmtTotPay, Convert.ToDouble(MetalAmtTotRec - MetalAmtTotPay).ToString("0.00"), "")
            drTableAdd.Item("PAY") = IIf(MetalAmtTotRec < MetalAmtTotPay, Convert.ToDouble(MetalAmtTotPay - MetalAmtTotRec).ToString("0.00"), "")
            drTableAdd.Item("AVERAGE") = ""
            drTableAdd.Item("RESULT1") = 6
            drTableAdd.Item("COLHEAD") = "G"
            dtTableAdd.Rows.Add(drTableAdd)
            dtTableAdd.AcceptChanges()
        End If
        '    Next cnt
        'End If
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()


    End Sub

    Public Sub funcCalc()
        Try
            GetRate(dtpFrom.Date)

            ProcSales("'SA','OD'", "SALES", "P", "ISSUE", "ISSSTONE")
            ProcSales("'SR'", "SALES RET", "R", "RECEIPT", "RECEIPTSTONE")
            ProcSales("'PU'", "ORDER/RECEIPT", "R", "RECEIPT", "RECEIPTSTONE", True)
            ProcRepairDelivery()
            ProcSales("'PU'", "PURCHASE", "R", "RECEIPT", "RECEIPTSTONE")


            Vat("'SA','OD'", "SALES", "P", "ISSUE", "ISSSTONE", False)
            Vat("'SR'", "SALES RET", "R", "RECEIPT", "RECEIPTSTONE", False)
            Vat("'PU'", "ORDER/RECEIPT", "R", "RECEIPT", "RECEIPTSTONE", False, True)
            Vat("'PU'", "PURCHASE", "R", "RECEIPT", "RECEIPTSTONE", True)

            AmtCreRecAdv()

            CAChqchitCrCardCounterWise()
            SummaryInformationAmt(33, 15, 15, 15, 15)
            If CancelBillDetail = True Then
                CancelBills()
            End If
            PartlySales()
            If MiscRecPaySum = True Then
                OtherIssue()
            End If
            StockAdd(30, 4, 15, 15)
            'RateDiff(20, 7, 12, 10, 10, 10, 20, 20, 20)
            RateDiff(15, 8, 12, 10, 10, 7, 10, 10, 10)
            If DotMatrix = True Then
                Dispo()
            End If
        Catch EX As Exception
            'If oWrite <> Nothing Then
            If oWrite IsNot Nothing Then
                oWrite.Flush()
                oWrite.Close()
            End If
            MessageBox.Show("Message :" + EX.Message & vbCrLf & "Stack Trace :" & EX.StackTrace, "Error", MessageBoxButtons.OK)
            'End If
        End Try
    End Sub

    Public Sub CAChqchitCrCardCounterWise()
        Dim Counter As String = ""
        Dim RunVal As Double = 0
        Dim TotRunVal As Double = 0
        'CASH
        StrSql = vbCrLf + " SELECT SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1 * AMOUNT END) COL,CASHID "
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN AS A"
        StrSql += vbCrLf + " WHERE A.TRANDATE BETWEEN '" & dtpFrom.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ")  "
        StrSql += vbCrLf + " AND ISNULL(PAYMODE,'') = 'CA' AND ISNULL(A.CANCEL,'') = '' "
        StrSql += strFilter
        StrSql += vbCrLf & " AND ISNULL(FROMFLAG,'') = 'P'"
        StrSql += vbCrLf + " GROUP BY CASHID"
        StrSql += vbCrLf + " ORDER BY CASHID"
        Dim dtCounter As New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtCounter)
        RunVal = 0
        If dtCounter.Rows.Count > 0 Then
            If DotMatrix = True Then
                For cnt As Integer = 0 To dtCounter.Rows.Count - 1
                    RunVal += Convert.ToDouble(IIf(dtCounter.Rows(cnt).Item("COL").ToString = "", "0", dtCounter.Rows(cnt).Item("COL").ToString))
                    Select Case cnt
                        Case 0
                            Counter += "Cash ("
                            Counter += dtCounter.Rows(cnt).Item("CASHID").ToString + " :" + dtCounter.Rows(cnt).Item("COL").ToString
                        Case 3, 7, 11, 15
                            WriteLine(LSet(Counter, oCharPerLine - (LenRec + LenPay + LenAvg)))
                            Counter = "      ,"
                            Counter += dtCounter.Rows(cnt).Item("CASHID").ToString + " :" + dtCounter.Rows(cnt).Item("COL").ToString
                        Case Else
                            Counter += " ," + dtCounter.Rows(cnt).Item("CASHID").ToString + " :" + dtCounter.Rows(cnt).Item("COL").ToString
                    End Select
                Next cnt
                WriteLine(LSet(Counter + ")", oCharPerLine - (LenRec + LenPay + LenAvg)) + RSet(RunVal.ToString("0.00"), LenRec) + RSet(" ", LenPay) + RSet(" ", LenAvg))
            Else
                For cnt As Integer = 0 To dtCounter.Rows.Count - 1
                    RunVal += Convert.ToDouble(IIf(dtCounter.Rows(cnt).Item("COL").ToString = "", "0", dtCounter.Rows(cnt).Item("COL").ToString))
                    Select Case cnt
                        Case 0
                            Counter += "Cash ("
                            Counter += dtCounter.Rows(cnt).Item("CASHID").ToString + " :" + dtCounter.Rows(cnt).Item("COL").ToString
                        Case 3, 7, 11, 15
                            Counter += " ,"
                            Counter += dtCounter.Rows(cnt).Item("CASHID").ToString + " :" + dtCounter.Rows(cnt).Item("COL").ToString
                        Case Else
                            Counter += " ," + dtCounter.Rows(cnt).Item("CASHID").ToString + " :" + dtCounter.Rows(cnt).Item("COL").ToString
                    End Select
                Next cnt
                Counter += " )"

                drTableAdd = dtTableAdd.NewRow
                drTableAdd.Item("DESC") = Counter
                drTableAdd.Item("IPCS") = ""
                drTableAdd.Item("IGWT") = ""
                drTableAdd.Item("INWT") = ""
                drTableAdd.Item("RPCS") = ""
                drTableAdd.Item("RGWT") = ""
                drTableAdd.Item("RNWT") = ""
                drTableAdd.Item("REC") = RunVal.ToString("0.00")
                drTableAdd.Item("PAY") = ""
                drTableAdd.Item("AVERAGE") = ""
                drTableAdd.Item("RESULT1") = ""
                drTableAdd.Item("COLHEAD") = ""
                dtTableAdd.Rows.Add(drTableAdd)
                dtTableAdd.AcceptChanges()
            End If
        End If

        'CHEQUE
        StrSql = vbCrLf + " SELECT SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1 * AMOUNT END) COL,CASHID "
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN AS A"
        StrSql += vbCrLf + " WHERE A.TRANDATE BETWEEN '" & dtpFrom.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ")  "
        StrSql += vbCrLf + " AND ISNULL(PAYMODE,'') = 'CH' AND ISNULL(A.CANCEL,'') = '' "
        StrSql += strFilter
        StrSql += vbCrLf & " AND ISNULL(FROMFLAG,'') = 'P'"
        StrSql += vbCrLf + " GROUP BY CASHID"
        dtCounter = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtCounter)
        TotRunVal += RunVal
        RunVal = 0
        Counter = ""
        If dtCounter.Rows.Count > 0 Then
            If DotMatrix = True Then
                For cnt As Integer = 0 To dtCounter.Rows.Count - 1
                    RunVal += Convert.ToDouble(IIf(dtCounter.Rows(cnt).Item("COL").ToString = "", "0", dtCounter.Rows(cnt).Item("COL").ToString))
                    Select Case cnt
                        Case 0
                            Counter += "Cheque ("
                            Counter += dtCounter.Rows(cnt).Item("CASHID").ToString + " :" + dtCounter.Rows(cnt).Item("COL").ToString
                        Case 3, 7, 11, 15
                            WriteLine(LSet(Counter, oCharPerLine - (LenRec + LenPay + LenAvg)))
                            Counter = "        ,"
                            Counter += dtCounter.Rows(cnt).Item("CASHID").ToString + " :" + dtCounter.Rows(cnt).Item("COL").ToString
                        Case Else
                            Counter += " ," + dtCounter.Rows(cnt).Item("CASHID").ToString + " :" + dtCounter.Rows(cnt).Item("COL").ToString
                    End Select
                Next cnt
                WriteLine(LSet(Counter + ")", oCharPerLine - (LenRec + LenPay + LenAvg)) + RSet(RunVal.ToString("0.00"), LenRec) + RSet(" ", LenPay) + RSet(" ", LenAvg))
            Else
                For cnt As Integer = 0 To dtCounter.Rows.Count - 1
                    RunVal += Convert.ToDouble(IIf(dtCounter.Rows(cnt).Item("COL").ToString = "", "0", dtCounter.Rows(cnt).Item("COL").ToString))
                    Select Case cnt
                        Case 0
                            Counter += "Cheque ("
                            Counter += dtCounter.Rows(cnt).Item("CASHID").ToString + " :" + dtCounter.Rows(cnt).Item("COL").ToString
                        Case 3, 7, 11, 15
                            Counter += " ,"
                            Counter += dtCounter.Rows(cnt).Item("CASHID").ToString + " :" + dtCounter.Rows(cnt).Item("COL").ToString
                        Case Else
                            Counter += " ," + dtCounter.Rows(cnt).Item("CASHID").ToString + " :" + dtCounter.Rows(cnt).Item("COL").ToString
                    End Select
                Next cnt
                Counter += " )"

                drTableAdd = dtTableAdd.NewRow
                drTableAdd.Item("DESC") = Counter
                drTableAdd.Item("IPCS") = ""
                drTableAdd.Item("IGWT") = ""
                drTableAdd.Item("INWT") = ""
                drTableAdd.Item("RPCS") = ""
                drTableAdd.Item("RGWT") = ""
                drTableAdd.Item("RNWT") = ""
                drTableAdd.Item("REC") = ""
                drTableAdd.Item("PAY") = RunVal.ToString("0.00")
                drTableAdd.Item("AVERAGE") = ""
                drTableAdd.Item("RESULT1") = ""
                drTableAdd.Item("COLHEAD") = ""
                dtTableAdd.Rows.Add(drTableAdd)
                dtTableAdd.AcceptChanges()
            End If
        End If


        'CREDIT CARD
        StrSql = vbCrLf + " SELECT SUM(COL) COL,SUM(COMMISION) COMMISION"
        StrSql += vbCrLf + " ,ISNULL(SUM(COL),0) - ISNULL(SUM(COMMISION),0) WITHOUTCOM,CASHID"
        StrSql += vbCrLf + " FROM"
        StrSql += vbCrLf + " ("
        StrSql += vbCrLf + " SELECT "
        StrSql += vbCrLf + "  SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1 * AMOUNT END) COL"
        StrSql += vbCrLf + " ,(SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1 * AMOUNT END) "
        StrSql += vbCrLf + " * (SELECT COMMISSION FROM " & cnAdminDb & "..CREDITCARD WHERE ACCODE = A.ACCODE)) /100 COMMISION"
        StrSql += vbCrLf + " ,CASHID "
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN AS A "
        StrSql += vbCrLf + " WHERE A.TRANDATE BETWEEN '" & dtpFrom.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") "
        StrSql += vbCrLf + " AND ISNULL(PAYMODE,'') = 'CC' AND ISNULL(A.CANCEL,'') = '' "
        StrSql += strFilter
        StrSql += vbCrLf & " AND ISNULL(FROMFLAG,'') = 'P'"
        StrSql += vbCrLf + " GROUP BY CASHID,ACCODE"
        StrSql += vbCrLf + " )X GROUP BY CASHID"
        dtCounter = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtCounter)
        TotRunVal += RunVal
        RunVal = 0
        Counter = ""
        Dim WithOutCom As Double = 0
        Dim WithCom As Double = 0
        If dtCounter.Rows.Count > 0 Then
            If DotMatrix = True Then
                For cnt As Integer = 0 To dtCounter.Rows.Count - 1
                    RunVal += Convert.ToDouble(IIf(dtCounter.Rows(cnt).Item("COL").ToString = "", "0", dtCounter.Rows(cnt).Item("COL").ToString))
                    WithOutCom += Convert.ToDouble(IIf(dtCounter.Rows(cnt).Item("WITHOUTCOM").ToString = "", "0", dtCounter.Rows(cnt).Item("WITHOUTCOM").ToString))
                    WithCom += Convert.ToDouble(IIf(dtCounter.Rows(cnt).Item("COMMISION").ToString = "", "0", dtCounter.Rows(cnt).Item("COMMISION").ToString))
                    Select Case cnt
                        Case 0
                            Counter += "Credit Card ("
                            Counter += dtCounter.Rows(cnt).Item("CASHID").ToString + " :" + dtCounter.Rows(cnt).Item("COL").ToString
                        Case 3, 7, 11, 15
                            WriteLine(LSet(Counter, oCharPerLine - (LenRec + LenPay + LenAvg)))
                            Counter = "              ,"
                            Counter += dtCounter.Rows(cnt).Item("CASHID").ToString + " :" + dtCounter.Rows(cnt).Item("COL").ToString
                        Case 1, 2
                            Counter += " ," + dtCounter.Rows(cnt).Item("CASHID").ToString + " :" + dtCounter.Rows(cnt).Item("COL").ToString
                    End Select
                Next cnt
                WriteLine(LSet(Counter + ")", oCharPerLine - (LenRec + LenPay + LenAvg)) + RSet(" ", LenRec) + RSet(" ", LenPay) + RSet(" ", LenAvg))
                Counter = " => CR.CARD COMM:" & WithCom.ToString("0.00") & " WITH OUT COMM:" & WithOutCom.ToString("0.00")
                WriteLine(LSet(Counter, oCharPerLine - (LenRec + LenPay + LenAvg)) + RSet(RunVal.ToString("0.00"), LenRec) + RSet(" ", LenPay) + RSet(" ", LenAvg))
            Else
                For cnt As Integer = 0 To dtCounter.Rows.Count - 1
                    RunVal += Convert.ToDouble(IIf(dtCounter.Rows(cnt).Item("COL").ToString = "", "0", dtCounter.Rows(cnt).Item("COL").ToString))
                    WithOutCom += Convert.ToDouble(IIf(dtCounter.Rows(cnt).Item("WITHOUTCOM").ToString = "", "0", dtCounter.Rows(cnt).Item("WITHOUTCOM").ToString))
                    WithCom += Convert.ToDouble(IIf(dtCounter.Rows(cnt).Item("COMMISION").ToString = "", "0", dtCounter.Rows(cnt).Item("COMMISION").ToString))
                    Select Case cnt
                        Case 0
                            Counter += "Credit Card ("
                            Counter += dtCounter.Rows(cnt).Item("CASHID").ToString + " :" + dtCounter.Rows(cnt).Item("COL").ToString
                        Case 3, 7, 11, 15
                            Counter += " ,"
                            Counter += dtCounter.Rows(cnt).Item("CASHID").ToString + " :" + dtCounter.Rows(cnt).Item("COL").ToString
                        Case Else
                            Counter += " ," + dtCounter.Rows(cnt).Item("CASHID").ToString + " :" + dtCounter.Rows(cnt).Item("COL").ToString
                    End Select
                Next cnt
                Counter += " )"

                drTableAdd = dtTableAdd.NewRow
                drTableAdd.Item("DESC") = Counter
                drTableAdd.Item("IPCS") = ""
                drTableAdd.Item("IGWT") = ""
                drTableAdd.Item("INWT") = ""
                drTableAdd.Item("RPCS") = ""
                drTableAdd.Item("RGWT") = ""
                drTableAdd.Item("RNWT") = ""
                drTableAdd.Item("REC") = ""
                drTableAdd.Item("PAY") = RunVal.ToString("0.00")
                drTableAdd.Item("AVERAGE") = ""
                drTableAdd.Item("RESULT1") = ""
                drTableAdd.Item("COLHEAD") = ""
                dtTableAdd.Rows.Add(drTableAdd)
                dtTableAdd.AcceptChanges()

                Counter = " => CR.CARD COMM:" & WithCom.ToString("0.00") & " WITH OUT COMM:" & WithOutCom.ToString("0.00")
                drTableAdd = dtTableAdd.NewRow
                drTableAdd.Item("DESC") = Counter
                drTableAdd.Item("IPCS") = ""
                drTableAdd.Item("IGWT") = ""
                drTableAdd.Item("INWT") = ""
                drTableAdd.Item("RPCS") = ""
                drTableAdd.Item("RGWT") = ""
                drTableAdd.Item("RNWT") = ""
                drTableAdd.Item("REC") = ""
                drTableAdd.Item("PAY") = ""
                drTableAdd.Item("AVERAGE") = ""
                drTableAdd.Item("RESULT1") = ""
                drTableAdd.Item("COLHEAD") = ""
                dtTableAdd.Rows.Add(drTableAdd)
                dtTableAdd.AcceptChanges()
            End If
        End If


        'SCHEME
        StrSql = vbCrLf + " SELECT "
        StrSql += vbCrLf + " ISNULL((SELECT top 1 NAME FROM " & cnAdminDb & "..CREDITCARD WHERE ISNULL(ACCODE,'') = A.ACCODE OR ISNULL(GIFTAC,'') = A.ACCODE OR ISNULL(PRIZEAC,'') = A.ACCODE OR ISNULL(BONUSAC,'') = A.ACCODE OR ISNULL(DEDUCTAC,'') = A.ACCODE),'')   ACNAME"
        StrSql += vbCrLf + " ,PAYMODE,SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1 * AMOUNT END) COL"
        StrSql += vbCrLf + " ,CASE PAYMODE WHEN 'SS' THEN 1 WHEN 'CG' THEN 2 WHEN 'CZ' THEN 3 WHEN 'CB' THEN 4 WHEN 'CD' THEN 5 END RESULT"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN AS A "
        StrSql += vbCrLf + " WHERE A.TRANDATE BETWEEN '" & dtpFrom.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ")  "
        StrSql += vbCrLf + " AND ISNULL(PAYMODE,'') IN ('SS','CG','CZ','CB','CD') AND ISNULL(A.CANCEL,'') = '' "
        StrSql += strFilter
        StrSql += vbCrLf & " AND ISNULL(FROMFLAG,'') = 'P'"
        StrSql += vbCrLf + " GROUP BY ACCODE,PAYMODE ORDER BY ACNAME,RESULT"
        dtCounter = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtCounter)
        TotRunVal += RunVal
        RunVal = 0
        Counter = ""
        Dim PrvScheme As String = ""
        Dim CurScheme As String = ""
        If dtCounter.Rows.Count > 0 Then
            If DotMatrix = True Then
                WriteLine(LSet("SCHEME", oCharPerLine - (LenRec + LenPay + LenAvg)) + RSet(" ", LenRec) + RSet(" ", LenPay) + RSet(" ", LenAvg))
                For cnt As Integer = 0 To dtCounter.Rows.Count - 1
                    With dtCounter.Rows(cnt)
                        If .Item("ACNAME").ToString.Length > 10 Then
                            CurScheme = .Item("ACNAME").ToString.Substring(0, 9)
                        Else
                            CurScheme = .Item("ACNAME").ToString
                        End If
                        If CurScheme <> PrvScheme Then
                            RunVal = 0
                            Counter = ""
                            If PrvScheme <> "" Then
                                WriteLine(LSet(Counter + ")", oCharPerLine - (LenRec + LenPay + LenAvg)) + RSet(RunVal.ToString("0.00"), LenRec) + RSet(" ", LenPay) + RSet(" ", LenAvg))
                            End If
                            Counter += "  " + CurScheme + "("
                        End If
                        Select Case dtCounter.Rows(cnt).Item("PAYMODE").ToString
                            Case "SS"
                                Counter += " REC.:" + .Item("COL").ToString
                                RunVal += Convert.ToDouble(IIf(.Item("COL").ToString = "", "0", .Item("COL").ToString))
                            Case "CG"
                                Counter += ",GIFT:" + .Item("COL").ToString
                                RunVal += Convert.ToDouble(IIf(.Item("COL").ToString = "", "0", .Item("COL").ToString))
                            Case "CZ"
                                Counter += ",PRIZE:" + .Item("COL").ToString
                                RunVal += Convert.ToDouble(IIf(.Item("COL").ToString = "", "0", .Item("COL").ToString))
                            Case "CB"
                                If Counter.Length + ",BONUS:".Length + .Item("COL").ToString.Length > oCharPerLine - (LenRec + LenPay + LenAvg) Then
                                    WriteLine(LSet(Counter + ")", oCharPerLine - (LenRec + LenPay + LenAvg)) + RSet(" ", LenRec) + RSet(" ", LenPay) + RSet(" ", LenAvg))
                                    Counter = "          "
                                End If
                                Counter += ",BONUS:" + .Item("COL").ToString
                                RunVal += Convert.ToDouble(IIf(.Item("COL").ToString = "", "0", .Item("COL").ToString))
                            Case "CD"
                                If Counter.Length + ",DEDC.:".Length + .Item("COL").ToString.Length > oCharPerLine - (LenRec + LenPay + LenAvg) Then
                                    WriteLine(LSet(Counter + ")", oCharPerLine - (LenRec + LenPay + LenAvg)) + RSet(" ", LenRec) + RSet(" ", LenPay) + RSet(" ", LenAvg))
                                    Counter = "          "
                                End If
                                Counter += ",DEDC.:" + .Item("COL").ToString
                                RunVal += Convert.ToDouble(IIf(.Item("COL").ToString = "", "0", .Item("COL").ToString))
                        End Select
                        PrvScheme = CurScheme
                        If cnt = dtCounter.Rows.Count - 1 Then
                            WriteLine(LSet(Counter + ")", oCharPerLine - (LenRec + LenPay + LenAvg)) + RSet(RunVal.ToString("0.00"), LenRec) + RSet(" ", LenPay) + RSet(" ", LenAvg))
                        End If
                    End With
                Next

                TotRunVal += RunVal
                WriteLine(PrintLine_WithOutWrite(oCharPerLine, "-"))
                WriteLine(LSet("Total", LenDesc) + RSet(" ", LenIPcs) + RSet(" ", LenIGwt) + RSet(" ", LenINwt) + RSet(" ", LenRPcs) + RSet(" ", LenRGwt) + RSet(" ", LenRNwt) + RSet(TotRunVal.ToString("0.00"), LenRec) + RSet("", LenPay) + RSet(" ", LenAvg))
                WriteLine(PrintLine_WithOutWrite(oCharPerLine, "-"))
            Else
                drTableAdd = dtTableAdd.NewRow
                drTableAdd.Item("DESC") = "SCHEME"
                drTableAdd.Item("IPCS") = ""
                drTableAdd.Item("IGWT") = ""
                drTableAdd.Item("INWT") = ""
                drTableAdd.Item("RPCS") = ""
                drTableAdd.Item("RGWT") = ""
                drTableAdd.Item("RNWT") = ""
                drTableAdd.Item("REC") = ""
                drTableAdd.Item("PAY") = ""
                drTableAdd.Item("AVERAGE") = ""
                drTableAdd.Item("RESULT1") = ""
                drTableAdd.Item("COLHEAD") = ""
                dtTableAdd.Rows.Add(drTableAdd)
                dtTableAdd.AcceptChanges()

                For cnt As Integer = 0 To dtCounter.Rows.Count - 1
                    With dtCounter.Rows(cnt)
                        If .Item("ACNAME").ToString.Length > 10 Then
                            CurScheme = .Item("ACNAME").ToString.Substring(0, 9)
                        Else
                            CurScheme = .Item("ACNAME").ToString
                        End If
                        If CurScheme <> PrvScheme Then
                            RunVal = 0
                            Counter = ""
                            If PrvScheme <> "" Then
                                drTableAdd = dtTableAdd.NewRow
                                drTableAdd.Item("DESC") = Counter + ")"
                                drTableAdd.Item("IPCS") = ""
                                drTableAdd.Item("IGWT") = ""
                                drTableAdd.Item("INWT") = ""
                                drTableAdd.Item("RPCS") = ""
                                drTableAdd.Item("RGWT") = ""
                                drTableAdd.Item("RNWT") = ""
                                drTableAdd.Item("REC") = ""
                                drTableAdd.Item("PAY") = RunVal.ToString("0.00")
                                drTableAdd.Item("AVERAGE") = ""
                                drTableAdd.Item("RESULT1") = ""
                                drTableAdd.Item("COLHEAD") = ""
                                dtTableAdd.Rows.Add(drTableAdd)
                                dtTableAdd.AcceptChanges()
                            End If
                            Counter += "  " + CurScheme + "("
                        End If
                        Select Case dtCounter.Rows(cnt).Item("PAYMODE").ToString
                            Case "SS"
                                Counter += " REC.:" + .Item("COL").ToString
                                RunVal += Convert.ToDouble(IIf(.Item("COL").ToString = "", "0", .Item("COL").ToString))
                            Case "CG"
                                Counter += ",GIFT:" + .Item("COL").ToString
                                RunVal += Convert.ToDouble(IIf(.Item("COL").ToString = "", "0", .Item("COL").ToString))
                            Case "CZ"
                                Counter += ",PRIZE:" + .Item("COL").ToString
                                RunVal += Convert.ToDouble(IIf(.Item("COL").ToString = "", "0", .Item("COL").ToString))
                            Case "CB"
                                Counter += ",BONUS:" + .Item("COL").ToString
                                RunVal += Convert.ToDouble(IIf(.Item("COL").ToString = "", "0", .Item("COL").ToString))
                            Case "CD"
                                Counter += ",DEDC.:" + .Item("COL").ToString
                                RunVal += Convert.ToDouble(IIf(.Item("COL").ToString = "", "0", .Item("COL").ToString))
                        End Select
                        PrvScheme = CurScheme
                        If cnt = dtCounter.Rows.Count - 1 Then
                            drTableAdd = dtTableAdd.NewRow
                            drTableAdd.Item("DESC") = Counter + ")"
                            drTableAdd.Item("IPCS") = ""
                            drTableAdd.Item("IGWT") = ""
                            drTableAdd.Item("INWT") = ""
                            drTableAdd.Item("RPCS") = ""
                            drTableAdd.Item("RGWT") = ""
                            drTableAdd.Item("RNWT") = ""
                            drTableAdd.Item("REC") = ""
                            drTableAdd.Item("PAY") = RunVal.ToString("0.00")
                            drTableAdd.Item("AVERAGE") = ""
                            drTableAdd.Item("RESULT1") = ""
                            drTableAdd.Item("COLHEAD") = ""
                            dtTableAdd.Rows.Add(drTableAdd)
                            dtTableAdd.AcceptChanges()
                        End If
                    End With
                Next

                TotRunVal += RunVal
                drTableAdd = dtTableAdd.NewRow
                drTableAdd.Item("DESC") = "Total"
                drTableAdd.Item("IPCS") = ""
                drTableAdd.Item("IGWT") = ""
                drTableAdd.Item("INWT") = ""
                drTableAdd.Item("RPCS") = ""
                drTableAdd.Item("RGWT") = ""
                drTableAdd.Item("RNWT") = ""
                drTableAdd.Item("REC") = ""
                drTableAdd.Item("PAY") = TotRunVal.ToString("0.00")
                drTableAdd.Item("AVERAGE") = ""
                drTableAdd.Item("RESULT1") = ""
                drTableAdd.Item("COLHEAD") = "G"
                dtTableAdd.Rows.Add(drTableAdd)
                dtTableAdd.AcceptChanges()
            End If
        End If

    End Sub

    Public Sub SummaryInformationAmt(ByVal LenInf As Integer, ByVal LenOpen As Integer, ByVal LenSumRec As Integer, ByVal LenSumPay As Integer, ByVal LenClose As Integer)
        StrSql = " IF OBJECT_ID('TEMPCRREADVTYPES') IS NOT NULL"
        StrSql += "     DROP TABLE TEMPCRREADVTYPES"
        StrSql += "  CREATE TABLE TEMPCRREADVTYPES(TRANTYPE VARCHAR(1),PAYMODE VARCHAR(25),RECPAY VARCHAR(1),REFMODE VARCHAR(10),DESCRIPTION VARCHAR(30))"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " INSERT INTO TEMPCRREADVTYPES(TRANTYPE,PAYMODE,RECPAY,DESCRIPTION)"
        StrSql += " SELECT 'D' TRANTYPE,'DU' PAYMODE,'P' RECPAY,'CREDIT BILL DETAILS' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'D' TRANTYPE,'DP' PAYMODE,'P' RECPAY,'CREDIT BILL DETAILS' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'D' TRANTYPE,'DR' PAYMODE,'R' RECPAY,'CREDIT BILL DETAILS' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'D' TRANTYPE,'DU' PAYMODE,'R' RECPAY,'CREDIT BILL DETAILS' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'D' TRANTYPE,'IDU' PAYMODE,'P' RECPAY,'TO BE BILL DETAILS' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'D' TRANTYPE,'IDP' PAYMODE,'P' RECPAY,'TO BE BILL DETAILS' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'D' TRANTYPE,'IDU' PAYMODE,'R' RECPAY,'TO BE BILL DETAILS' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'D' TRANTYPE,'IDR' PAYMODE,'R' RECPAY,'TO BE BILL DETAILS' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'AR' PAYMODE,'R' RECPAY,'ADVANCE DETAILS' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'AA' PAYMODE,'P' RECPAY,'ADVANCE DETAILS' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'AP' PAYMODE,'P' RECPAY,'ADVANCE DETAILS' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'IAR' PAYMODE,'R' RECPAY,'ADVANCE DETAILS' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'IAA' PAYMODE,'P' RECPAY,'ADVANCE DETAILS' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'IAP' PAYMODE,'P' RECPAY,'ADVANCE DETAILS' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'OAA' PAYMODE,'P' RECPAY,'ORDER ADVANCE DETAILS' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'OAP' PAYMODE,'P' RECPAY,'ORDER ADVANCE DETAILS' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'OAR' PAYMODE,'R' RECPAY,'ORDER ADVANCE DETAILS' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'OOR' PAYMODE,'R' RECPAY,'ORDER ADVANCE DETAILS' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'RAA' PAYMODE,'P' RECPAY,'REPAIR ADVANCE DETAILS' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'RAP' PAYMODE,'P' RECPAY,'REPAIR ADVANCE DETAILS' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'RAR' PAYMODE,'R' RECPAY,'REPAIR ADVANCE DETAILS' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'A' TRANTYPE,'ROR' PAYMODE,'R' RECPAY,'REPAIR ADVANCE DETAILS' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'T' TRANTYPE,'MR' PAYMODE,'R' RECPAY,'MISC DETAILS' DESCRIPTION"
        StrSql += " UNION ALL"
        StrSql += " SELECT 'T' TRANTYPE,'MP' PAYMODE,'P' RECPAY,'MISC DETAILS' DESCRIPTION"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " IF OBJECT_ID('TEMPCRREADVTYPE') IS NOT NULL"
        StrSql += "     DROP TABLE TEMPCRREADVTYPE"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " SELECT"
        StrSql += vbCrLf + " 	BATCHNO,SUBSTRING(RUNNO,6,20)RUNNO,TRANDATE,RECPAY,AMOUNT,TRANTYPE,CONVERT(VARCHAR(5),ISNULL((SELECT TOP 1 'I' FROM " & cnAdminDb & "..ITEMDETAIL WHERE BATCHNO = O.BATCHNO),'') + PAYMODE) PAYMODE"
        StrSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..ITEMDETAIL WHERE BATCHNO = O.BATCHNO) ITEMBATCHNO"
        StrSql += vbCrLf + " INTO TEMPCRREADVTYPE"
        StrSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS O"
        StrSql += vbCrLf + " WHERE TRANDATE <= '" & dtpTo.Date.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND ISNULL(CANCEL,'') = '' "
        StrSql += vbCrLf + " AND O.FROMFLAG NOT IN ('','S','O','A')"
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += strFilter
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " ALTER TABLE MASTER..TEMPCRREADVTYPE ADD PRVBILL VARCHAR(1)"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " UPDATE MASTER..TEMPCRREADVTYPE SET PRVBILL = 'Y'"
        StrSql += vbCrLf + " FROM MASTER..TEMPCRREADVTYPE AS T "
        StrSql += vbCrLf + " WHERE RUNNO IN (SELECT SUBSTRING(RUNNO,6,20) FROM MASTER..TEMPCRREADVTYPE WHERE TRANDATE < '" + cnTranFromDate.Date.ToString("yyyy-MM-dd") + "')"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " UPDATE MASTER..TEMPCRREADVTYPE SET PAYMODE = SUBSTRING(RUNNO,6,1) + PAYMODE WHERE SUBSTRING(RUNNO,6,1) IN ('O','R')"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " IF OBJECT_ID('TEMPCRREADVFINAL') IS NOT NULL"
        StrSql += vbCrLf + " 	DROP TABLE TEMPCRREADVFINAL"
        StrSql += vbCrLf + " SELECT "
        StrSql += vbCrLf + " 	 SUM(CASE WHEN TRANDATE < '" & dtpFrom.Date.ToString("yyyy-MM-dd") & "' THEN CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1 * AMOUNT END END) OPENING"
        StrSql += vbCrLf + " 	,SUM(CASE WHEN TRANDATE >= '" & dtpFrom.Date.ToString("yyyy-MM-dd") & "' AND TRANDATE <= '" & dtpTo.Date.ToString("yyyy-MM-dd") & "' THEN CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END END) RECEIPT"
        StrSql += vbCrLf + " 	,SUM(CASE WHEN TRANDATE >= '" & dtpFrom.Date.ToString("yyyy-MM-dd") & "' AND TRANDATE <= '" & dtpTo.Date.ToString("yyyy-MM-dd") & "' THEN CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END END) PAYMENT"
        StrSql += vbCrLf + " 	,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1 * AMOUNT END) CLOSING"
        StrSql += vbCrLf + " 	,(SELECT DESCRIPTION + CASE WHEN ISNULL(PRVBILL,'') = 'Y' THEN '( OLD)' ELSE '(NEW)' END FROM MASTER..TEMPCRREADVTYPES WHERE ISNULL(TRANTYPE,'') = T.TRANTYPE AND PAYMODE = T.PAYMODE AND RECPAY = T.RECPAY) DESCTITLE"
        StrSql += vbCrLf + " INTO TEMPCRREADVFINAL"
        StrSql += vbCrLf + " FROM MASTER..TEMPCRREADVTYPE AS T GROUP BY TRANTYPE,PAYMODE,RECPAY,PRVBILL"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " SELECT DESCTITLE DESCRIPTION,CASE WHEN SUM(OPENING) > 0 THEN SUM(OPENING) ELSE NULL END OPENING,CASE WHEN SUM(RECEIPT) > 0 THEN SUM(RECEIPT) ELSE NULL END RECEIPT,CASE WHEN SUM(PAYMENT) > 0 THEN SUM(PAYMENT) ELSE NULL END PAYMENT,CASE WHEN SUM(CLOSING) > 0 THEN SUM(CLOSING) ELSE NULL END CLOSING FROM TEMPCRREADVFINAL GROUP BY DESCTITLE ORDER BY DESCTITLE"
        Dim dtAmtCr As New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtAmtCr)
        If dtAmtCr.Rows.Count > 0 Then
            If DotMatrix = True Then
                WriteLine(PrintLine_WithOutWrite(oCharPerLine, "-"))
                WriteLine(LSet("Summary Information", LenInf) + RSet(" Opening", LenOpen) + RSet(" Receipts", LenSumRec) + RSet(" Payments", LenSumPay) + RSet(" Closing", LenClose))
                WriteLine(PrintLine_WithOutWrite(oCharPerLine, "-"))
                For cnt As Integer = 0 To dtAmtCr.Rows.Count - 1
                    With dtAmtCr.Rows(cnt)
                        WriteLine(LSet(dtAmtCr.Rows(cnt).Item("DESCRIPTION").ToString, LenInf) + RSet(Convert.ToDouble(IIf(.Item("OPENING").ToString = "", "0", .Item("OPENING").ToString)).ToString("0.00"), LenOpen) + RSet(Convert.ToDouble(IIf(.Item("RECEIPT").ToString = "", "0", .Item("RECEIPT").ToString)).ToString("0.00"), LenSumRec) + RSet(Convert.ToDouble(IIf(.Item("PAYMENT").ToString = "", "0", .Item("PAYMENT").ToString)).ToString("0.00"), LenSumPay) + RSet(Convert.ToDouble(IIf(.Item("CLOSING").ToString = "", "0", .Item("CLOSING").ToString)).ToString("0.00"), LenClose))
                    End With
                Next cnt
            Else
                drTableAdd = dtTableAdd.NewRow
                drTableAdd.Item("DESC") = "Summary Information"
                drTableAdd.Item("IPCS") = ""
                drTableAdd.Item("IGWT") = " Opening"
                drTableAdd.Item("INWT") = " Receipts"
                drTableAdd.Item("RPCS") = ""
                drTableAdd.Item("RGWT") = " Payments"
                drTableAdd.Item("RNWT") = " Closing"
                drTableAdd.Item("REC") = ""
                drTableAdd.Item("PAY") = ""
                drTableAdd.Item("AVERAGE") = ""
                drTableAdd.Item("RESULT1") = ""
                drTableAdd.Item("COLHEAD") = "S"
                dtTableAdd.Rows.Add(drTableAdd)
                dtTableAdd.AcceptChanges()


                For cnt As Integer = 0 To dtAmtCr.Rows.Count - 1
                    With dtAmtCr.Rows(cnt)
                        drTableAdd = dtTableAdd.NewRow
                        drTableAdd.Item("DESC") = dtAmtCr.Rows(cnt).Item("DESCRIPTION").ToString
                        drTableAdd.Item("IPCS") = ""
                        drTableAdd.Item("IGWT") = .Item("OPENING").ToString
                        drTableAdd.Item("INWT") = .Item("RECEIPT").ToString
                        drTableAdd.Item("RPCS") = ""
                        drTableAdd.Item("RGWT") = .Item("PAYMENT").ToString
                        drTableAdd.Item("RNWT") = .Item("CLOSING").ToString
                        drTableAdd.Item("REC") = ""
                        drTableAdd.Item("PAY") = ""
                        drTableAdd.Item("AVERAGE") = ""
                        drTableAdd.Item("RESULT1") = ""
                        drTableAdd.Item("COLHEAD") = ""
                        dtTableAdd.Rows.Add(drTableAdd)
                        dtTableAdd.AcceptChanges()
                    End With
                Next cnt
            End If
        End If
    End Sub


    'Public Sub SummaryInformationAmt(ByVal LenInf As Integer, ByVal LenOpen As Integer, ByVal LenSumRec As Integer, ByVal LenSumPay As Integer, ByVal LenClose As Integer)
    '    StrSql = " IF OBJECT_ID('TEMPCRREADVTYPES') IS NOT NULL"
    '    StrSql += "     DROP TABLE TEMPCRREADVTYPES"
    '    StrSql += "  CREATE TABLE TEMPCRREADVTYPES(TRANTYPE VARCHAR(1),PAYMODE VARCHAR(25),RECPAY VARCHAR(1),REFMODE VARCHAR(10),DESCRIPTION VARCHAR(30))"
    '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '    Cmd.ExecuteNonQuery()

    '    StrSql = " INSERT INTO TEMPCRREADVTYPES(TRANTYPE,PAYMODE,RECPAY,DESCRIPTION)"
    '    StrSql += " SELECT 'D' TRANTYPE,'DU' PAYMODE,'P' RECPAY,'CREDIT BILL DETAIL' DESCRIPTION"
    '    StrSql += " UNION ALL"
    '    StrSql += " SELECT 'D' TRANTYPE,'DP' PAYMODE,'P' RECPAY,'CREDIT BILL DETAIL' DESCRIPTION"
    '    StrSql += " UNION ALL"
    '    StrSql += " SELECT 'D' TRANTYPE,'DR' PAYMODE,'R' RECPAY,'RECEIPT AGAINST CREDIT BILL' DESCRIPTION"
    '    StrSql += " UNION ALL"
    '    StrSql += " SELECT 'D' TRANTYPE,'DU' PAYMODE,'R' RECPAY,'RECEIPT AGAINST CREDIT BILL' DESCRIPTION"
    '    StrSql += " UNION ALL"
    '    StrSql += " SELECT 'D' TRANTYPE,'IDU' PAYMODE,'P' RECPAY,'TO BE BILL DETAIL' DESCRIPTION"
    '    StrSql += " UNION ALL"
    '    StrSql += " SELECT 'D' TRANTYPE,'IDP' PAYMODE,'P' RECPAY,'TO BE BILL DETAIL' DESCRIPTION"
    '    StrSql += " UNION ALL"
    '    StrSql += " SELECT 'D' TRANTYPE,'IDU' PAYMODE,'R' RECPAY,'TO BE RECEIPTS' DESCRIPTION"
    '    StrSql += " UNION ALL"
    '    StrSql += " SELECT 'D' TRANTYPE,'IDR' PAYMODE,'R' RECPAY,'TO BE RECEIPTS' DESCRIPTION"
    '    StrSql += " UNION ALL"
    '    StrSql += " SELECT 'A' TRANTYPE,'AR' PAYMODE,'R' RECPAY,'ADVANCE RECEIVED' DESCRIPTION"
    '    StrSql += " UNION ALL"
    '    StrSql += " SELECT 'A' TRANTYPE,'AA' PAYMODE,'P' RECPAY,'ADVANCE ADJUSTED' DESCRIPTION"
    '    StrSql += " UNION ALL"
    '    StrSql += " SELECT 'A' TRANTYPE,'AP' PAYMODE,'P' RECPAY,'ADVANCE ADJUSTED' DESCRIPTION"
    '    StrSql += " UNION ALL"
    '    StrSql += " SELECT 'A' TRANTYPE,'OAA' PAYMODE,'P' RECPAY,'ORDER ADVANCE ADJUSTED' DESCRIPTION"
    '    StrSql += " UNION ALL"
    '    StrSql += " SELECT 'A' TRANTYPE,'OAP' PAYMODE,'P' RECPAY,'ORDER ADVANCE ADJUSTED' DESCRIPTION"
    '    StrSql += " UNION ALL"
    '    StrSql += " SELECT 'A' TRANTYPE,'OAR' PAYMODE,'R' RECPAY,'ORDER ADVANCE RECEIVED' DESCRIPTION"
    '    StrSql += " UNION ALL"
    '    StrSql += " SELECT 'A' TRANTYPE,'OOR' PAYMODE,'R' RECPAY,'ORDER ADVANCE RECEIVED' DESCRIPTION"
    '    StrSql += " UNION ALL"
    '    StrSql += " SELECT 'A' TRANTYPE,'RAA' PAYMODE,'P' RECPAY,'REPAIR ADVANCE ADJUSTED' DESCRIPTION"
    '    StrSql += " UNION ALL"
    '    StrSql += " SELECT 'A' TRANTYPE,'RAP' PAYMODE,'P' RECPAY,'REPAIR ADVANCE ADJUSTED' DESCRIPTION"
    '    StrSql += " UNION ALL"
    '    StrSql += " SELECT 'A' TRANTYPE,'RAR' PAYMODE,'R' RECPAY,'REPAIR ADVANCE RECEIVED' DESCRIPTION"
    '    StrSql += " UNION ALL"
    '    StrSql += " SELECT 'A' TRANTYPE,'ROR' PAYMODE,'R' RECPAY,'REPAIR ADVANCE RECEIVED' DESCRIPTION"
    '    StrSql += " UNION ALL"
    '    StrSql += " SELECT 'T' TRANTYPE,'MR' PAYMODE,'R' RECPAY,'MISC RECEIPT' DESCRIPTION"
    '    StrSql += " UNION ALL"
    '    StrSql += " SELECT 'T' TRANTYPE,'MP' PAYMODE,'P' RECPAY,'MISC PAYMENT' DESCRIPTION"
    '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '    Cmd.ExecuteNonQuery()

    '    StrSql = " IF OBJECT_ID('TEMPCRREADVTYPE') IS NOT NULL"
    '    StrSql += "     DROP TABLE TEMPCRREADVTYPE"
    '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '    Cmd.ExecuteNonQuery()

    '    StrSql = vbCrLf + " SELECT"
    '    StrSql += vbCrLf + " 	BATCHNO,RUNNO,TRANDATE,RECPAY,AMOUNT,TRANTYPE,CONVERT(VARCHAR(5),ISNULL((SELECT TOP 1 'I' FROM " & cnAdminDb & "..ITEMDETAIL WHERE BATCHNO = O.BATCHNO),'') + PAYMODE) PAYMODE"
    '    StrSql += vbCrLf + " ,(SELECT TOP 1 BATCHNO FROM " & cnAdminDb & "..ITEMDETAIL WHERE BATCHNO = O.BATCHNO) ITEMBATCHNO"
    '    StrSql += vbCrLf + " INTO TEMPCRREADVTYPE"
    '    StrSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS O"
    '    StrSql += vbCrLf + " WHERE TRANDATE <= '" & dtpTo.Date.ToString("yyyy-MM-dd") & "'"
    '    StrSql += vbCrLf + " AND ISNULL(CANCEL,'') = '' "
    '    StrSql += vbCrLf + " AND O.FROMFLAG NOT IN ('','S','O','A')"
    '    StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ")"
    '    StrSql += strFilter
    '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '    Cmd.ExecuteNonQuery()

    '    StrSql = vbCrLf + " ALTER TABLE MASTER..TEMPCRREADVTYPE ADD PRVBILL VARCHAR(1)"
    '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '    Cmd.ExecuteNonQuery()

    '    StrSql = vbCrLf + " UPDATE MASTER..TEMPCRREADVTYPE SET PRVBILL = 'Y'"
    '    StrSql += vbCrLf + " FROM MASTER..TEMPCRREADVTYPE AS T "
    '    StrSql += vbCrLf + " WHERE RUNNO IN (SELECT RUNNO FROM MASTER..TEMPCRREADVTYPE WHERE TRANDATE < '" + cnTranFromDate.Date.ToString("yyyy-MM-dd") + "')"
    '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '    Cmd.ExecuteNonQuery()

    '    StrSql = vbCrLf + " UPDATE MASTER..TEMPCRREADVTYPE SET PAYMODE = SUBSTRING(RUNNO,6,1) + PAYMODE WHERE SUBSTRING(RUNNO,6,1) IN ('O','R')"
    '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '    Cmd.ExecuteNonQuery()

    '    StrSql = vbCrLf + " IF OBJECT_ID('TEMPCRREADVFINAL') IS NOT NULL"
    '    StrSql += vbCrLf + " 	DROP TABLE TEMPCRREADVFINAL"
    '    StrSql += vbCrLf + " SELECT "
    '    StrSql += vbCrLf + " 	 SUM(CASE WHEN TRANDATE < '" & dtpFrom.Date.ToString("yyyy-MM-dd") & "' THEN CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1 * AMOUNT END END) OPENING"
    '    StrSql += vbCrLf + " 	,SUM(CASE WHEN TRANDATE >= '" & dtpFrom.Date.ToString("yyyy-MM-dd") & "' AND TRANDATE <= '" & dtpTo.Date.ToString("yyyy-MM-dd") & "' THEN CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE 0 END END) RECEIPT"
    '    StrSql += vbCrLf + " 	,SUM(CASE WHEN TRANDATE >= '" & dtpFrom.Date.ToString("yyyy-MM-dd") & "' AND TRANDATE <= '" & dtpTo.Date.ToString("yyyy-MM-dd") & "' THEN CASE WHEN RECPAY = 'P' THEN AMOUNT ELSE 0 END END) PAYMENT"
    '    StrSql += vbCrLf + " 	,SUM(CASE WHEN RECPAY = 'R' THEN AMOUNT ELSE -1 * AMOUNT END) CLOSING"
    '    StrSql += vbCrLf + " 	,(SELECT DESCRIPTION + CASE WHEN ISNULL(PRVBILL,'') = 'Y' THEN '(P)' ELSE '(T)' END FROM MASTER..TEMPCRREADVTYPES WHERE ISNULL(TRANTYPE,'') = T.TRANTYPE AND PAYMODE = T.PAYMODE AND RECPAY = T.RECPAY) DESCTITLE"
    '    StrSql += vbCrLf + " INTO TEMPCRREADVFINAL"
    '    StrSql += vbCrLf + " FROM MASTER..TEMPCRREADVTYPE AS T GROUP BY TRANTYPE,PAYMODE,RECPAY,PRVBILL"
    '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
    '    Cmd.ExecuteNonQuery()

    '    StrSql = vbCrLf + " SELECT DESCTITLE DESCRIPTION,SUM(OPENING) OPENING,SUM(RECEIPT) RECEIPT,SUM(PAYMENT) PAYMENT,SUM(CLOSING) CLOSING FROM TEMPCRREADVFINAL GROUP BY DESCTITLE ORDER BY DESCTITLE"
    '    Dim dtAmtCr As New DataTable
    '    da = New OleDbDataAdapter(StrSql, cn)
    '    da.Fill(dtAmtCr)
    '    If dtAmtCr.Rows.Count > 0 Then
    '        If DotMatrix = True Then
    '            WriteLine(PrintLine_WithOutWrite(oCharPerLine, "-"))
    '            WriteLine(LSet("Summary Information", LenInf) + RSet(" Opening", LenOpen) + RSet(" Receipts", LenSumRec) + RSet(" Payments", LenSumPay) + RSet(" Closing", LenClose))
    '            WriteLine(PrintLine_WithOutWrite(oCharPerLine, "-"))
    '            For cnt As Integer = 0 To dtAmtCr.Rows.Count - 1
    '                With dtAmtCr.Rows(cnt)
    '                    WriteLine(LSet(dtAmtCr.Rows(cnt).Item("DESCRIPTION").ToString, LenInf) + RSet(Convert.ToDouble(IIf(.Item("OPENING").ToString = "", "0", .Item("OPENING").ToString)).ToString("0.00"), LenOpen) + RSet(Convert.ToDouble(IIf(.Item("RECEIPT").ToString = "", "0", .Item("RECEIPT").ToString)).ToString("0.00"), LenSumRec) + RSet(Convert.ToDouble(IIf(.Item("PAYMENT").ToString = "", "0", .Item("PAYMENT").ToString)).ToString("0.00"), LenSumPay) + RSet(Convert.ToDouble(IIf(.Item("CLOSING").ToString = "", "0", .Item("CLOSING").ToString)).ToString("0.00"), LenClose))
    '                End With
    '            Next cnt
    '        Else
    '            drTableAdd = dtTableAdd.NewRow
    '            drTableAdd.Item("DESC") = "Summary Information"
    '            drTableAdd.Item("IPCS") = ""
    '            drTableAdd.Item("IGWT") = " Opening"
    '            drTableAdd.Item("INWT") = " Receipts"
    '            drTableAdd.Item("RPCS") = ""
    '            drTableAdd.Item("RGWT") = " Payments"
    '            drTableAdd.Item("RNWT") = " Closing"
    '            drTableAdd.Item("REC") = ""
    '            drTableAdd.Item("PAY") = ""
    '            drTableAdd.Item("AVERAGE") = ""
    '            drTableAdd.Item("RESULT1") = ""
    '            drTableAdd.Item("COLHEAD") = "S"
    '            dtTableAdd.Rows.Add(drTableAdd)
    '            dtTableAdd.AcceptChanges()


    '            For cnt As Integer = 0 To dtAmtCr.Rows.Count - 1
    '                With dtAmtCr.Rows(cnt)
    '                    drTableAdd = dtTableAdd.NewRow
    '                    drTableAdd.Item("DESC") = dtAmtCr.Rows(cnt).Item("DESCRIPTION").ToString
    '                    drTableAdd.Item("IPCS") = ""
    '                    drTableAdd.Item("IGWT") = .Item("OPENING").ToString
    '                    drTableAdd.Item("INWT") = .Item("RECEIPT").ToString
    '                    drTableAdd.Item("RPCS") = ""
    '                    drTableAdd.Item("RGWT") = .Item("PAYMENT").ToString
    '                    drTableAdd.Item("RNWT") = .Item("CLOSING").ToString
    '                    drTableAdd.Item("REC") = ""
    '                    drTableAdd.Item("PAY") = ""
    '                    drTableAdd.Item("AVERAGE") = ""
    '                    drTableAdd.Item("RESULT1") = ""
    '                    drTableAdd.Item("COLHEAD") = ""
    '                    dtTableAdd.Rows.Add(drTableAdd)
    '                    dtTableAdd.AcceptChanges()
    '                End With
    '            Next cnt
    '        End If
    '    End If
    'End Sub

    Public Sub PartlySales()
        StrSql = "  IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSPARTLYSALE') DROP TABLE TEMP" & systemId & "ABSPARTLYSALE "
        StrSql += vbCrLf + " SELECT CATNAME, SUM(TAGPCS)-SUM(ISSPCS)ISSPCS ,SUM(TAGGRSWT)-SUM(ISSGRSWT)ISSGRSWT ,SUM(TAGNETWT)-SUM(ISSNETWT)ISSNETWT"
        StrSql += vbCrLf + " ,SUM(STNPCS)STNPCS, SUM(STNWT)STNWT, 1 AS RESULT1, ' ' AS COLHEAD "
        StrSql += vbCrLf + " INTO TEMP" & systemId & "ABSPARTLYSALE FROM ("
        If CategoryShortName = True Then
            StrSql += vbCrLf + " SELECT (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY "
        Else
            StrSql += vbCrLf + " SELECT (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
        End If
        StrSql += vbCrLf + " WHERE CATCODE = I.CATCODE)AS CATNAME   "
        StrSql += vbCrLf + " ,I.PCS AS ISSPCS, I.GRSWT AS ISSGRSWT, I.NETWT AS ISSNETWT "
        StrSql += vbCrLf + " ,(SELECT ISNULL(SUM(STNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)STNPCS "
        StrSql += vbCrLf + " ,(SELECT ISNULL(SUM(TAGSTNPCS),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNPCS "
        StrSql += vbCrLf + " ,(SELECT ISNULL(SUM(STNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO )STNWT "
        StrSql += vbCrLf + " ,(SELECT ISNULL(SUM(TAGSTNWT),0) FROM " & cnStockDb & "..ISSSTONE S WHERE I.SNO = S.ISSSNO)TAGSTNWT "
        StrSql += vbCrLf + " ,TAGPCS,TAGGRSWT,TAGNETWT FROM " & cnStockDb & "..ISSUE I "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Date.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " AND '" & dtpTo.Date.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND I.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
        StrSql += vbCrLf + " AND I.TAGNO <> ''"
        StrSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''   "
        StrSql += vbCrLf + " AND I.COMPANYID IN (" & SelectedCompanyId & ")"
        strFilter = Replace(strFilter, "SYSTEMID", "I.SYSTEMID")
        StrSql += strFilter
        strFilter = Replace(strFilter, "I.SYSTEMID", "SYSTEMID")
        StrSql += vbCrLf + " ) X "
        StrSql += vbCrLf + " WHERE ((TAGPCS)<>ISSPCS OR (TAGGRSWT)<> ISSGRSWT OR (TAGNETWT)<>ISSNETWT "
        StrSql += vbCrLf + " ) GROUP BY CATNAME"
        StrSql += vbCrLf + " HAVING SUM(TAGPCS)-SUM(ISSPCS) <> 0 OR SUM(TAGGRSWT)-SUM(ISSGRSWT) <> 0"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " SELECT '  ' + CATNAME DESCRIPTION, "
        StrSql += vbCrLf + " CASE WHEN ISSPCS    <> 0 THEN ISSPCS    ELSE NULL END IPCS, "
        StrSql += vbCrLf + " CASE WHEN ISSGRSWT  <> 0 THEN ISSGRSWT  ELSE NULL END IGRSWT, "
        StrSql += vbCrLf + " CASE WHEN ISSNETWT  <> 0 THEN ISSNETWT  ELSE NULL END INETWT,NULL RPCS,NULL RGRSWT,NULL RNETWT,NULL RECEIPT,NULL PAYMENT,NULL AVERAGE "
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSPARTLYSALE WHERE ISSGRSWT <> 0 OR ISSNETWT <> 0 ORDER BY RESULT1 "
        Dim dtAmtCr As New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtAmtCr)
        If dtAmtCr.Rows.Count > 0 Then
            If DotMatrix = True Then
                WriteLine(LSet(" ", oCharPerLine))
                oWrite.Write(pBoldDoubleStart)
                WriteLine(LSet("PARTLY SALES", oCharPerLine))
                oWrite.Write(pBoldDoubleEnd)
                For cnt As Integer = 0 To dtAmtCr.Rows.Count - 1
                    With dtAmtCr.Rows(cnt)
                        WriteLine(LSet(.Item("DESCRIPTION").ToString, LenDesc) + RSet(IIf(.Item("IPCS").ToString = "", "0", .Item("IPCS").ToString()), LenIPcs) + RSet(Convert.ToDouble(IIf(.Item("IGRSWT").ToString = "", "0", .Item("IGRSWT").ToString())).ToString("0.00"), LenIGwt) + RSet(Convert.ToDouble(IIf(.Item("INETWT").ToString = "", "0", .Item("INETWT").ToString())).ToString("0.00"), LenINwt) + RSet("", LenRPcs) + RSet("", LenRGwt) + RSet("", LenRNwt) + RSet("", LenRec) + RSet("", LenPay) + RSet("", LenAvg))
                    End With
                Next cnt
            Else
                drTableAdd = dtTableAdd.NewRow
                drTableAdd.Item("DESC") = "PARTLY SALES"
                drTableAdd.Item("IPCS") = ""
                drTableAdd.Item("IGWT") = ""
                drTableAdd.Item("INWT") = ""
                drTableAdd.Item("RPCS") = ""
                drTableAdd.Item("RGWT") = ""
                drTableAdd.Item("RNWT") = ""
                drTableAdd.Item("REC") = ""
                drTableAdd.Item("PAY") = ""
                drTableAdd.Item("AVERAGE") = ""
                drTableAdd.Item("RESULT1") = ""
                drTableAdd.Item("COLHEAD") = "T"
                dtTableAdd.Rows.Add(drTableAdd)
                dtTableAdd.AcceptChanges()

                For cnt As Integer = 0 To dtAmtCr.Rows.Count - 1
                    With dtAmtCr.Rows(cnt)
                        drTableAdd = dtTableAdd.NewRow
                        drTableAdd.Item("DESC") = dtAmtCr.Rows(cnt).Item("DESCRIPTION").ToString
                        drTableAdd.Item("IPCS") = dtAmtCr.Rows(cnt).Item("IPCS").ToString
                        drTableAdd.Item("IGWT") = dtAmtCr.Rows(cnt).Item("IGRSWT").ToString
                        drTableAdd.Item("INWT") = dtAmtCr.Rows(cnt).Item("INETWT").ToString
                        drTableAdd.Item("RPCS") = dtAmtCr.Rows(cnt).Item("RPCS").ToString
                        drTableAdd.Item("RGWT") = dtAmtCr.Rows(cnt).Item("RGRSWT").ToString
                        drTableAdd.Item("RNWT") = dtAmtCr.Rows(cnt).Item("RNETWT").ToString
                        drTableAdd.Item("REC") = dtAmtCr.Rows(cnt).Item("RECEIPT").ToString
                        drTableAdd.Item("PAY") = dtAmtCr.Rows(cnt).Item("PAYMENT").ToString
                        drTableAdd.Item("AVERAGE") = dtAmtCr.Rows(cnt).Item("AVERAGE").ToString
                        drTableAdd.Item("RESULT1") = ""
                        drTableAdd.Item("COLHEAD") = ""
                        dtTableAdd.Rows.Add(drTableAdd)
                        dtTableAdd.AcceptChanges()
                    End With
                Next cnt
            End If
        End If
    End Sub

    Public Sub OtherIssue()
        StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND NAME = 'TEMP" & systemId & "ABSMISCISSUE') DROP TABLE TEMP" & systemId & "ABSMISCISSUE"
        If CategoryWise = True Then
            If CategoryShortName = True Then
                StrSql += " SELECT CATCODE, "
                StrSql += " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME, "
            Else
                StrSql += " SELECT CATCODE, "
                StrSql += " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME, "
            End If
        Else
            StrSql += " SELECT METALID, "
            StrSql += " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS CATNAME, "
        End If
        StrSql += " SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT, "
        StrSql += " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
        StrSql += " 0 AS RECEIPT, 0 AS PAYMENT, 1 AS RESULT1, ' ' COLHEAD "
        StrSql += " INTO TEMP" & systemId & "ABSMISCISSUE FROM " & cnStockDb & "..ISSUE I "
        StrSql += " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Date.ToString("yyyy-MM-dd") & "' "
        StrSql += " AND '" & dtpTo.Date.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " TRANTYPE = 'MI' AND ISNULL(I.CANCEL,'') = ''  AND I.COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += strFilter
        If CategoryWise = True Then
            StrSql += " GROUP BY CATCODE "
        Else
            StrSql += " GROUP BY METALID "
        End If
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()


        StrSql = " SELECT '  ' + CATNAME DESCRIPTION, "
        StrSql += " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END IPCS, "
        StrSql += " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END IGRSWT, "
        StrSql += " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END INETWT, "
        StrSql += " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RPCS, "
        StrSql += " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RGRSWT, "
        StrSql += " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RNETWT, "
        StrSql += " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        StrSql += " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        StrSql += " NULL AVERAGE, RESULT1 "
        StrSql += " FROM TEMP" & systemId & "ABSMISCISSUE "
        If CategoryWise = True Then
            StrSql += " ORDER BY CATCODE,RESULT1"
        Else
            StrSql += " ORDER BY CATNAME,RESULT1"
        End If
        Dim dtAmtCr As New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtAmtCr)
        If dtAmtCr.Rows.Count > 0 Then
            If DotMatrix = True Then
                WriteLine(LSet(" ", oCharPerLine))
                oWrite.Write(pBoldDoubleStart)
                WriteLine(LSet("MISC ISSUE", oCharPerLine))
                oWrite.Write(pBoldDoubleEnd)
                For cnt As Integer = 0 To dtAmtCr.Rows.Count - 1
                    With dtAmtCr.Rows(cnt)
                        WriteLine(LSet(.Item("DESCRIPTION").ToString, LenDesc) + RSet(IIf(.Item("IPCS").ToString = "", "0", .Item("IPCS").ToString()), LenIPcs) + RSet(Convert.ToDouble(IIf(.Item("IGRSWT").ToString = "", "0", .Item("IGRSWT").ToString())).ToString("0.00"), LenIGwt) + RSet(Convert.ToDouble(IIf(.Item("INETWT").ToString = "", "0", .Item("INETWT").ToString())).ToString("0.00"), LenINwt) + RSet("", LenRPcs) + RSet("", LenRGwt) + RSet("", LenRNwt) + RSet("", LenRec) + RSet("", LenPay) + RSet("", LenAvg))
                    End With
                Next cnt
            Else
                drTableAdd = dtTableAdd.NewRow
                drTableAdd.Item("DESC") = "MISC ISSUE"
                drTableAdd.Item("IPCS") = ""
                drTableAdd.Item("IGWT") = ""
                drTableAdd.Item("INWT") = ""
                drTableAdd.Item("RPCS") = ""
                drTableAdd.Item("RGWT") = ""
                drTableAdd.Item("RNWT") = ""
                drTableAdd.Item("REC") = ""
                drTableAdd.Item("PAY") = ""
                drTableAdd.Item("AVERAGE") = ""
                drTableAdd.Item("RESULT1") = ""
                drTableAdd.Item("COLHEAD") = "T"
                dtTableAdd.Rows.Add(drTableAdd)
                dtTableAdd.AcceptChanges()

                For cnt As Integer = 0 To dtAmtCr.Rows.Count - 1
                    With dtAmtCr.Rows(cnt)
                        drTableAdd = dtTableAdd.NewRow
                        drTableAdd.Item("DESC") = dtAmtCr.Rows(cnt).Item("DESCRIPTION").ToString
                        drTableAdd.Item("IPCS") = dtAmtCr.Rows(cnt).Item("IPCS").ToString
                        drTableAdd.Item("IGWT") = dtAmtCr.Rows(cnt).Item("IGRSWT").ToString
                        drTableAdd.Item("INWT") = dtAmtCr.Rows(cnt).Item("INETWT").ToString
                        drTableAdd.Item("RPCS") = dtAmtCr.Rows(cnt).Item("RPCS").ToString
                        drTableAdd.Item("RGWT") = dtAmtCr.Rows(cnt).Item("RGRSWT").ToString
                        drTableAdd.Item("RNWT") = dtAmtCr.Rows(cnt).Item("RNETWT").ToString
                        drTableAdd.Item("REC") = dtAmtCr.Rows(cnt).Item("RECEIPT").ToString
                        drTableAdd.Item("PAY") = dtAmtCr.Rows(cnt).Item("PAYMENT").ToString
                        drTableAdd.Item("AVERAGE") = dtAmtCr.Rows(cnt).Item("AVERAGE").ToString
                        drTableAdd.Item("RESULT1") = ""
                        drTableAdd.Item("COLHEAD") = ""
                        dtTableAdd.Rows.Add(drTableAdd)
                        dtTableAdd.AcceptChanges()
                    End With
                Next cnt
            End If
        End If
    End Sub

    Public Sub StockAdd(ByVal LenMetal As Integer, ByVal LenStPcs As Integer, ByVal LenStGwt As Integer, ByVal LenStNwt As Integer)
        StrSql = vbCrLf + " SELECT "
        StrSql += vbCrLf + " METALNAME DESCRIPTION,SUM(PCS) PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT"
        StrSql += vbCrLf + " FROM"
        StrSql += vbCrLf + " ("
        StrSql += vbCrLf + "  SELECT "
        StrSql += vbCrLf + " 	 (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID "
        StrSql += vbCrLf + " 	 IN (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE "
        StrSql += vbCrLf + " 	 IN (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID))) METALNAME "
        StrSql += vbCrLf + " 	 ,PCS,GRSWT,NETWT "
        StrSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAG AS I"
        StrSql += " WHERE I.RECDATE BETWEEN '" & dtpFrom.Date.ToString("yyyy-MM-dd") & "' "
        StrSql += " AND '" & dtpTo.Date.ToString("yyyy-MM-dd") & "'"
        StrSql += " AND I.COMPANYID IN (" & SelectedCompanyId & ")"
        'StrSql += strFilter
        StrSql += vbCrLf + " )X GROUP BY METALNAME ORDER BY METALNAME"
        Dim dtAmtCr As New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtAmtCr)
        If dtAmtCr.Rows.Count > 0 Then
            If DotMatrix = True Then
                WriteLine(LSet(" ", oCharPerLine))
                oWrite.Write(pBoldDoubleStart)
                WriteLine(LSet("Stock Addition Details", oCharPerLine))
                oWrite.Write(pBoldDoubleEnd)
                WriteLine(PrintLine_WithOutWrite(oCharPerLine, "-"))
                WriteLine(LSet("METAL", LenMetal) + RSet(" PCS", LenStPcs) + RSet(" GROSS.WT", LenStGwt) + RSet(" NET.WT", LenStNwt))
                WriteLine(PrintLine_WithOutWrite(oCharPerLine, "-"))
                For cnt As Integer = 0 To dtAmtCr.Rows.Count - 1
                    With dtAmtCr.Rows(cnt)
                        WriteLine(LSet(.Item("DESCRIPTION").ToString, LenMetal) + RSet(Convert.ToDouble(IIf(.Item("PCS").ToString = "", "0", .Item("PCS").ToString)), LenStPcs) + RSet(Convert.ToDouble(IIf(.Item("GRSWT").ToString = "", "0", .Item("GRSWT").ToString)).ToString("0.00"), LenStGwt) + RSet(Convert.ToDouble(IIf(.Item("NETWT").ToString = "", "0", .Item("NETWT").ToString)).ToString("0.00"), LenStNwt))
                    End With
                Next cnt
            Else
                drTableAdd = dtTableAdd.NewRow
                drTableAdd.Item("DESC") = "STOCK ADDITION DETAILS"
                drTableAdd.Item("IPCS") = ""
                drTableAdd.Item("IGWT") = ""
                drTableAdd.Item("INWT") = ""
                drTableAdd.Item("RPCS") = ""
                drTableAdd.Item("RGWT") = ""
                drTableAdd.Item("RNWT") = ""
                drTableAdd.Item("REC") = ""
                drTableAdd.Item("PAY") = ""
                drTableAdd.Item("AVERAGE") = ""
                drTableAdd.Item("RESULT1") = ""
                drTableAdd.Item("COLHEAD") = "T"
                dtTableAdd.Rows.Add(drTableAdd)
                dtTableAdd.AcceptChanges()

                drTableAdd = dtTableAdd.NewRow
                drTableAdd.Item("DESC") = "METAL"
                drTableAdd.Item("IPCS") = " PCS"
                drTableAdd.Item("IGWT") = " GROSS.WT"
                drTableAdd.Item("INWT") = " NET.WT"
                drTableAdd.Item("RPCS") = ""
                drTableAdd.Item("RGWT") = ""
                drTableAdd.Item("RNWT") = ""
                drTableAdd.Item("REC") = ""
                drTableAdd.Item("PAY") = ""
                drTableAdd.Item("AVERAGE") = ""
                drTableAdd.Item("RESULT1") = ""
                drTableAdd.Item("COLHEAD") = "S"
                dtTableAdd.Rows.Add(drTableAdd)
                dtTableAdd.AcceptChanges()

                For cnt As Integer = 0 To dtAmtCr.Rows.Count - 1
                    With dtAmtCr.Rows(cnt)
                        drTableAdd = dtTableAdd.NewRow
                        drTableAdd.Item("DESC") = dtAmtCr.Rows(cnt).Item("DESCRIPTION").ToString
                        drTableAdd.Item("IPCS") = dtAmtCr.Rows(cnt).Item("PCS").ToString
                        drTableAdd.Item("IGWT") = dtAmtCr.Rows(cnt).Item("GRSWT").ToString
                        drTableAdd.Item("INWT") = dtAmtCr.Rows(cnt).Item("NETWT").ToString
                        drTableAdd.Item("RPCS") = ""
                        drTableAdd.Item("RGWT") = ""
                        drTableAdd.Item("RNWT") = ""
                        drTableAdd.Item("REC") = ""
                        drTableAdd.Item("PAY") = ""
                        drTableAdd.Item("AVERAGE") = ""
                        drTableAdd.Item("RESULT1") = ""
                        drTableAdd.Item("COLHEAD") = ""
                        dtTableAdd.Rows.Add(drTableAdd)
                        dtTableAdd.AcceptChanges()
                    End With
                Next cnt
            End If
        End If
    End Sub

    Public Sub CancelBills()
        'StrSql = " SELECT  TRANNO, TRANTYPE, (CASE "
        'StrSql += " WHEN TRANTYPE = 'SA' THEN 'SAL BILL-'  "
        'StrSql += " WHEN TRANTYPE = 'PU' THEN 'PUR BILL-'  "
        'StrSql += " WHEN TRANTYPE = 'SR' THEN 'SAL RET BILL-'  "
        'StrSql += " WHEN TRANTYPE = 'AD' THEN 'ORD ADV BILL-'  "
        'StrSql += " WHEN TRANTYPE = 'OD' THEN 'ORD DEL BILL-'  "
        'StrSql += " WHEN TRANTYPE = 'RD' THEN 'REP DEL BILL-'  "
        'StrSql += " WHEN TRANTYPE = 'AI' THEN 'APP ISS BILL-'  END ) "
        ''StrSql += " + CONVERT(VARCHAR(5),TRANNO) + ' - ' + CONVERT(VARCHAR(11),TRANDATE,103) AS TRANS, "
        'StrSql += " + CONVERT(VARCHAR(5),TRANNO) AS DESCRIPTION, "
        'StrSql += " IPCS, IGRSWT, INETWT, RPCS, RGRSWT, RNETWT, RECEIPT, PAYMENT,NULL AVERAGE"
        ''StrSql += " INTO TEMP" & systemId & "ABSCANCELBILL FROM (  "
        'StrSql += "  FROM (  "
        'StrSql += " SELECT TRANNO, TRANDATE, TRANTYPE, SUM(PCS) IPCS, SUM(GRSWT) IGRSWT,  "
        'StrSql += " SUM(NETWT) INETWT, 0 RPCS,  0 RGRSWT, 0 RNETWT, SUM(AMOUNT) RECEIPT, 0 PAYMENT  "
        'StrSql += " FROM " & cnStockDb & "..ISSUE  "
        'StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Date.ToString("yyyy-MM-dd") & "' AND "
        'StrSql += " '" & dtpTo.Date.ToString("yyyy-MM-dd") & "' "
        'StrSql += " AND ISNULL(CANCEL,'') = 'Y'  AND COMPANYID IN (" & SelectedCompanyId & ") "
        'StrSql += strFilter
        'StrSql += " GROUP BY TRANNO, TRANDATE, TRANTYPE"
        'StrSql += " UNION ALL"
        'StrSql += " SELECT TRANNO, TRANDATE, TRANTYPE, 0 IPCS, 0 IGRSWT, 0 INETWT,  "
        'StrSql += " SUM(PCS) RPCS, SUM(GRSWT) RGRSWT, SUM(NETWT) RNETWT, "
        'StrSql += " 0 RECEIPT, SUM(AMOUNT) PAYMENT FROM " & cnStockDb & "..RECEIPT  "
        'StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Date.ToString("yyyy-MM-dd") & "' AND "
        'StrSql += " '" & dtpTo.Date.ToString("yyyy-MM-dd") & "' "
        'StrSql += " AND ISNULL(CANCEL,'') = 'Y'   AND COMPANYID IN (" & SelectedCompanyId & ")"
        'StrSql += strFilter
        'StrSql += " GROUP BY TRANNO, TRANDATE, TRANTYPE"
        'StrSql += " ) X "
        'StrSql += " WHERE TRANTYPE IN ('SA','SR','PU','AD','OD','RD','AI') ORDER BY DESCRIPTION"


        StrSql = " SELECT TRANNO BILLNO,CONVERT(VARCHAR,TRANDATE,103) BILLDATE, TAGNO TAGNO, SUM(GRSWT) GRSWT  "
        StrSql += " FROM " & cnStockDb & "..ISSUE  "
        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Date.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Date.ToString("yyyy-MM-dd") & "' "
        StrSql += " AND ISNULL(CANCEL,'') = 'Y'  AND COMPANYID IN (" & SelectedCompanyId & ") "
        StrSql += strFilter
        StrSql += " GROUP BY TRANNO, TRANDATE, TAGNO"
        StrSql += " UNION ALL"
        StrSql += " SELECT TRANNO,CONVERT(VARCHAR,TRANDATE,103) BILLDATE, TAGNO, SUM(GRSWT) GRSWT  "
        StrSql += "  FROM " & cnStockDb & "..RECEIPT  "
        StrSql += " WHERE TRANDATE BETWEEN '" & dtpFrom.Date.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Date.ToString("yyyy-MM-dd") & "' "
        StrSql += " AND ISNULL(CANCEL,'') = 'Y'   AND COMPANYID IN (" & SelectedCompanyId & ")"
        StrSql += strFilter
        StrSql += " GROUP BY TRANNO, TRANDATE, TAGNO"

        Dim dtAmtCr As New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtAmtCr)
        If dtAmtCr.Rows.Count > 0 Then
            If DotMatrix = True Then
                WriteLine(LSet(" ", oCharPerLine))
                oWrite.Write(pBoldDoubleStart)
                WriteLine(LSet("CANCEL BILL DETAILS", LenDesc) + LSet("", LenIPcs) + LSet("BILL NO", LenIGwt) + LSet("BILL DATE", LenINwt) + LSet("", LenRPcs) + LSet("TAG NO", LenRGwt) + LSet("GRS.WT", LenRNwt))
                oWrite.Write(pBoldDoubleEnd)
                For cnt As Integer = 0 To dtAmtCr.Rows.Count - 1
                    With dtAmtCr.Rows(cnt)
                        'WriteLine(LSet(dtAmtCr.Rows(cnt).Item("DESCRIPTION").ToString, LenDesc) + RSet(dtAmtCr.Rows(cnt).Item("IPCS").ToString, LenIPcs) + RSet(dtAmtCr.Rows(cnt).Item("IGRSWT").ToString, LenIGwt) + RSet(dtAmtCr.Rows(cnt).Item("INETWT").ToString, LenINwt) + RSet(dtAmtCr.Rows(cnt).Item("RPCS").ToString, LenRPcs) + RSet(dtAmtCr.Rows(cnt).Item("RGRSWT").ToString, LenRGwt) + RSet(dtAmtCr.Rows(cnt).Item("RNETWT").ToString, LenRNwt) + RSet(dtAmtCr.Rows(cnt).Item("RECEIPT").ToString, LenRec) + RSet(dtAmtCr.Rows(cnt).Item("PAYMENT").ToString, LenPay) + RSet(dtAmtCr.Rows(cnt).Item("AVERAGE").ToString, LenAvg))
                        WriteLine(LSet(" ", LenDesc) + LSet("", LenIPcs) + LSet(dtAmtCr.Rows(cnt).Item("BILLNO").ToString, LenIGwt) + LSet(dtAmtCr.Rows(cnt).Item("BILLDATE").ToString, LenINwt) + LSet("", LenRPcs) + LSet(dtAmtCr.Rows(cnt).Item("TAGNO").ToString, LenRGwt) + LSet(dtAmtCr.Rows(cnt).Item("GRSWT").ToString, LenRNwt))
                    End With
                Next cnt
            Else
                drTableAdd = dtTableAdd.NewRow
                drTableAdd.Item("DESC") = "CANCEL BILL DETAILS"
                drTableAdd.Item("IPCS") = ""
                drTableAdd.Item("IGWT") = "BILL NO"
                drTableAdd.Item("INWT") = "BILL DATE"
                drTableAdd.Item("RPCS") = ""
                drTableAdd.Item("RGWT") = "TAG NO"
                drTableAdd.Item("RNWT") = "GRS WT"
                drTableAdd.Item("REC") = ""
                drTableAdd.Item("PAY") = ""
                drTableAdd.Item("AVERAGE") = ""
                drTableAdd.Item("RESULT1") = ""
                drTableAdd.Item("COLHEAD") = "T"
                dtTableAdd.Rows.Add(drTableAdd)
                dtTableAdd.AcceptChanges()

                For cnt As Integer = 0 To dtAmtCr.Rows.Count - 1
                    With dtAmtCr.Rows(cnt)
                        drTableAdd = dtTableAdd.NewRow
                        drTableAdd.Item("DESC") = ""
                        drTableAdd.Item("IPCS") = ""
                        drTableAdd.Item("IGWT") = dtAmtCr.Rows(cnt).Item("BILLNO").ToString
                        drTableAdd.Item("INWT") = dtAmtCr.Rows(cnt).Item("BILLDATE").ToString
                        drTableAdd.Item("RPCS") = ""
                        drTableAdd.Item("RGWT") = dtAmtCr.Rows(cnt).Item("TAGNO").ToString
                        drTableAdd.Item("RNWT") = dtAmtCr.Rows(cnt).Item("GRSWT").ToString
                        drTableAdd.Item("REC") = ""
                        drTableAdd.Item("PAY") = ""
                        drTableAdd.Item("AVERAGE") = ""
                        drTableAdd.Item("RESULT1") = ""
                        drTableAdd.Item("COLHEAD") = ""
                        dtTableAdd.Rows.Add(drTableAdd)
                        dtTableAdd.AcceptChanges()
                    End With
                Next cnt
            End If
        End If
    End Sub

    Public Sub RateDiff(ByVal LenProd As Integer, ByVal LenBillNo As Integer, ByVal LenRaGwt As Integer, ByVal LenBRate As Integer, ByVal LenRate As Integer, ByVal LenDiff As Integer, ByVal LenSales As Integer, ByVal LenCounter As Integer, ByVal LenRemark As Integer)
        StrSql = vbCrLf + " SELECT "
        StrSql += vbCrLf + " 	 (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) DESCRIPTION"
        StrSql += vbCrLf + " 	,TRANNO BILLNO,GRSWT,BOARDRATE,RATE"
        StrSql += vbCrLf + " 	,ISNULL(RATE,0) - ISNULL(BOARDRATE,0) DIFF"
        StrSql += vbCrLf + " 	,' ' +(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = I.EMPID) SALESMAN"
        StrSql += vbCrLf + " 	,' ' +(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = I.ITEMCTRID) COUNTER"
        StrSql += vbCrLf + " 	,' ' +(SELECT REMARK1 FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO) REMARK"
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
        StrSql += vbCrLf + " WHERE ISNULL(BOARDRATE,0) - ISNULL(RATE,0) <> 0"
        'StrSql += vbCrLf + " AND ISNULL(TRANTYPE,'') = 'SA' "
        'StrSql += vbCrLf + " AND SALEMODE = 'W'"
        StrSql += " AND TRANDATE BETWEEN '" & dtpFrom.Date.ToString("yyyy-MM-dd") & "' AND "
        StrSql += " '" & dtpTo.Date.ToString("yyyy-MM-dd") & "' "
        StrSql += " AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ") "
        StrSql += strFilter
        StrSql += vbCrLf + " ORDER BY DESCRIPTION,BILLNO"
        Dim dtAmtCr As New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtAmtCr)
        If dtAmtCr.Rows.Count > 0 Then
            If DotMatrix = True Then
                WriteLine(LSet(" ", oCharPerLine))
                oWrite.Write(pBoldDoubleStart)
                WriteLine(LSet("Rate Diff Details", oCharPerLine))
                oWrite.Write(pBoldDoubleEnd)
                WriteLine(PrintLine_WithOutWrite(oCharPerLine, "-"))
                WriteLine(LSet("Product", LenProd) + RSet("BillNo", LenBillNo) + RSet(" Gross.Wt", LenRaGwt) + RSet("BoardRate", LenBRate) + RSet("Rate", LenRate) + RSet(" Diff", LenDiff) + RSet("Salesman", LenSales) + RSet(" Counter", LenCounter) + RSet("   Remark", LenRemark))
                WriteLine(PrintLine_WithOutWrite(oCharPerLine, "-"))
                For cnt As Integer = 0 To dtAmtCr.Rows.Count - 1
                    With dtAmtCr.Rows(cnt)
                        WriteLine(LSet(.Item("DESCRIPTION").ToString, LenProd) + RSet(.Item("BILLNO").ToString, LenBillNo) + RSet(Convert.ToDouble(IIf(.Item("GRSWT").ToString = "", "0", .Item("GRSWT").ToString())).ToString("0.00"), LenRaGwt) + RSet(Convert.ToDouble(IIf(.Item("BOARDRATE").ToString = "", "0", .Item("BOARDRATE").ToString())).ToString("0.00"), LenBRate) + RSet(Convert.ToDouble(IIf(.Item("RATE").ToString = "", "0", .Item("RATE").ToString())).ToString("0.00"), LenRate) + RSet(Convert.ToDouble(IIf(.Item("DIFF").ToString = "", "0", .Item("DIFF").ToString())).ToString("0.00"), LenDiff) + LSet(.Item("SALESMAN").ToString, LenSales) + LSet(.Item("COUNTER").ToString, LenCounter) + LSet(.Item("REMARK").ToString, LenRemark))
                    End With
                Next cnt
            Else
                drTableAdd = dtTableAdd.NewRow
                drTableAdd.Item("DESC") = "RATE DIFF DETAILS"
                drTableAdd.Item("IPCS") = ""
                drTableAdd.Item("IGWT") = ""
                drTableAdd.Item("INWT") = ""
                drTableAdd.Item("RPCS") = ""
                drTableAdd.Item("RGWT") = ""
                drTableAdd.Item("RNWT") = ""
                drTableAdd.Item("REC") = ""
                drTableAdd.Item("PAY") = ""
                drTableAdd.Item("AVERAGE") = ""
                drTableAdd.Item("RESULT1") = ""
                drTableAdd.Item("COLHEAD") = "T"
                dtTableAdd.Rows.Add(drTableAdd)
                dtTableAdd.AcceptChanges()

                drTableAdd = dtTableAdd.NewRow
                drTableAdd.Item("DESC") = "PRODUCT"
                drTableAdd.Item("IPCS") = "BILLNO"
                drTableAdd.Item("IGWT") = " GROSS.WT"
                drTableAdd.Item("INWT") = "BOARD RATE"
                drTableAdd.Item("RPCS") = " RATE"
                drTableAdd.Item("RGWT") = " DIFF"
                drTableAdd.Item("RNWT") = "SALESMAN"
                drTableAdd.Item("REC") = "COUNTER"
                drTableAdd.Item("PAY") = "REMARK"
                drTableAdd.Item("AVERAGE") = ""
                drTableAdd.Item("RESULT1") = ""
                drTableAdd.Item("COLHEAD") = "S"
                dtTableAdd.Rows.Add(drTableAdd)
                dtTableAdd.AcceptChanges()

                For cnt As Integer = 0 To dtAmtCr.Rows.Count - 1
                    With dtAmtCr.Rows(cnt)
                        drTableAdd = dtTableAdd.NewRow
                        drTableAdd.Item("DESC") = dtAmtCr.Rows(cnt).Item("DESCRIPTION").ToString
                        drTableAdd.Item("IPCS") = dtAmtCr.Rows(cnt).Item("BILLNO").ToString
                        drTableAdd.Item("IGWT") = dtAmtCr.Rows(cnt).Item("GRSWT").ToString
                        drTableAdd.Item("INWT") = dtAmtCr.Rows(cnt).Item("BOARDRATE").ToString
                        drTableAdd.Item("RPCS") = dtAmtCr.Rows(cnt).Item("RATE").ToString
                        drTableAdd.Item("RGWT") = dtAmtCr.Rows(cnt).Item("DIFF").ToString
                        drTableAdd.Item("RNWT") = dtAmtCr.Rows(cnt).Item("SALESMAN").ToString
                        drTableAdd.Item("REC") = dtAmtCr.Rows(cnt).Item("COUNTER").ToString
                        drTableAdd.Item("PAY") = dtAmtCr.Rows(cnt).Item("REMARK").ToString
                        drTableAdd.Item("AVERAGE") = ""
                        drTableAdd.Item("RESULT1") = ""
                        drTableAdd.Item("COLHEAD") = ""
                        dtTableAdd.Rows.Add(drTableAdd)
                        dtTableAdd.AcceptChanges()
                    End With
                Next cnt
            End If
        End If
    End Sub

    Public Sub ProcSales(ByVal TranType As String, ByVal TranMode As String, ByVal ViewMode As String, ByVal MainTable As String, ByVal StoneTable As String, Optional ByVal OrMast As Boolean = False)

        'If MainTable = "ISSUE" Then
        '    StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND "
        '    StrSql += vbCrLf + " NAME = 'TEMP" & systemId & "ABSDISC') DROP TABLE TEMP" & systemId & "ABSDISC"
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    Cmd.ExecuteNonQuery()

        '    StrSql = vbCrLf + " SELECT  "
        '    'StrSql += vbCrLf + "      (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ISNULL(METALID,'') = Y.METAL) METALNAME"
        '    'StrSql += vbCrLf + " 	,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = Y.CATCODE) CATNAME"
        '    StrSql += vbCrLf + " 	METAL,CATCODE,SUM((SALAMT-TAGVAL)- DISCOUNT)  AS DISC"
        '    StrSql += vbCrLf + " 	--,(CASE WHEN ISNULL(TRANTYPE,'')='RD' THEN ' R' ELSE METAL END) AS  METAL	"
        '    StrSql += vbCrLf + " 	--,BATCHNO,TRANNO,SALEMODE"
        '    StrSql += vbCrLf + " 	--,TAGWT,TAGWAST,TAGWASPER,TAGTOT,TAGVAL,SALWT,SALWAST,SALWASPER,SALAMT"
        '    StrSql += vbCrLf + " 	--,(SALAMT-TAGVAL) AS DIFF ,DISCOUNT,((SALAMT-TAGVAL)- DISCOUNT)  AS DISC"
        '    StrSql += vbCrLf + " 	--,((SALAMT-TAGVAL)- DISCOUNT) / (CASE WHEN ISNULL(SALWT,0) = 0 THEN 1 ELSE SALWT END) AS PERGRM"
        '    StrSql += vbCrLf + " 	--,(SELECT TOP 1 REMARK1 FROM  " & cnstockDb & "..ACCTRAN AS C WHERE C.BATCHNO = Y.BATCHNO) AS BILLREMARK"
        '    StrSql += vbCrLf + " INTO TEMP" & systemId & "ABSDISC"
        '    StrSql += vbCrLf + " FROM"
        '    StrSql += vbCrLf + " (/* Y */"
        '    StrSql += vbCrLf + " 	SELECT "
        '    StrSql += vbCrLf + " 		  METAL,BATCHNO,TRANNO,SALEMODE,TAGWT,TAGWAST,TAGWASPER,CATCODE"
        '    StrSql += vbCrLf + " 		 ,ISNULL(TAGWT,0)+ISNULL(TAGWAST,0) TAGTOT"
        '    StrSql += vbCrLf + " 		 ,CASE WHEN SALEMODE NOT IN ('F','R','') THEN  "
        '    StrSql += vbCrLf + " 						CASE WHEN ISNULL(SALEMODE,'') IN ('W','B') AND ISNULL(TAGNO,'') <> '' THEN "
        '    StrSql += vbCrLf + " 							((CASE WHEN X.GRSNET='N' THEN ISNULL(SALWT,0) ELSE ISNULL(TAGWT,0) END) + ISNULL(TAGWAST,0)) * (CASE WHEN ISNULL(TAGRATE,0) = 0 THEN RATE ELSE TAGRATE END) + ISNULL(TAGMC,0) + ISNULL(RATE,0) "
        '    StrSql += vbCrLf + " 						ELSE AMOUNT "
        '    StrSql += vbCrLf + " 						END  "
        '    StrSql += vbCrLf + " 		  ELSE CASE WHEN ISNULL(SALVALUE,0)<>0 THEN SALVALUE ELSE ISNULL(TAGRATE,0) END "
        '    StrSql += vbCrLf + " 		  END AS TAGVAL"
        '    StrSql += vbCrLf + " 		,SALWT,SALWAST,SALWASPER"
        '    StrSql += vbCrLf + " 		,(CASE WHEN SALEMODE NOT IN ('F','R','') THEN CASE WHEN ISNULL(SALEMODE,'') IN ('W','B') AND ISNULL(TAGNO,'') <> '' THEN (ISNULL(SALWT,0)+ISNULL(SALWAST,0))*RATE+ISNULL(RATE,0)+ISNULL(MCHARGE,0) - OFFDISC "
        '    StrSql += vbCrLf + " 		                                              ELSE AMOUNT "
        '    StrSql += vbCrLf + " 		                                              END "
        '    StrSql += vbCrLf + " 		  ELSE AMOUNT "
        '    StrSql += vbCrLf + " 		  END) AS SALAMT,DISCOUNT,TRANTYPE"
        '    StrSql += vbCrLf + " 	FROM "
        '    StrSql += vbCrLf + " 	(/* X */"
        '    StrSql += vbCrLf + " 		SELECT "
        '    StrSql += vbCrLf + " 			 CATCODE,(SELECT TOP 1 METALID FROM " & cnAdminDb & "..METALMAST WHERE METALID IN (S.METALID)) METAL,S.BATCHNO,TRANNO			"
        '    StrSql += vbCrLf + " 			,CASE WHEN ISNULL(T.SALEMODE,'') = '' OR S.TRANTYPE = 'RD' THEN S.SALEMODE ELSE T.SALEMODE END SALEMODE"
        '    StrSql += vbCrLf + " 			,(CASE WHEN ROUND(TAGGRSWT,3) <> ROUND(S.GRSWT,3) THEN (CASE WHEN S.GRSNET = 'N' THEN S.NETWT ELSE S.GRSWT END)"
        '    StrSql += vbCrLf + " 				   ELSE (CASE WHEN T.GRSNET = 'N' THEN T.NETWT ELSE T.GRSWT END)"
        '    StrSql += vbCrLf + " 			  END) TAGWT"
        '    StrSql += vbCrLf + " 		   ,ISNULL((CASE WHEN ROUND(S.TAGGRSWT,3) <> ROUND(S.GRSWT,3) "
        '    StrSql += vbCrLf + " 						 THEN ((CASE WHEN S.GRSNET = 'N' THEN S.NETWT ELSE S.GRSWT END) * T.MAXWAST) / 100 "
        '    StrSql += vbCrLf + " 					ELSE MAXWAST "
        '    StrSql += vbCrLf + " 					END),WASTAGE) TAGWAST"
        '    StrSql += vbCrLf + " 		   ,ISNULL((CASE WHEN ROUND(S.TAGGRSWT,3) <> ROUND(S.GRSWT,3) "
        '    StrSql += vbCrLf + " 						 THEN ((CASE WHEN S.GRSNET = 'N' THEN S.NETWT ELSE S.GRSWT END) * T.MAXMC)"
        '    StrSql += vbCrLf + " 					ELSE MAXMC "
        '    StrSql += vbCrLf + " 					END),MCHARGE) TAGMC"
        '    StrSql += vbCrLf + " 		   ,T.RATE TAGRATE,T.MAXWAST TAGWASPER"
        '    StrSql += vbCrLf + " 		   ,CASE WHEN T.SALEMODE = 'F' THEN SALVALUE ELSE AMOUNT END SALVALUE"
        '    StrSql += vbCrLf + " 		   ,MCHARGE,T.TAGNO,TRANTYPE,S.GRSNET "
        '    StrSql += vbCrLf + " 		   ,(SELECT SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE AMOUNT *-1 END) FROM  " & cnstockDb & "..ACCTRAN AS C WHERE C.BATCHNO = S.BATCHNO AND TRANMODE = 'D') AS DISCOUNT"
        '    StrSql += vbCrLf + " 		   ,(CASE WHEN S.GRSNET = 'N' THEN S.NETWT ELSE S.GRSWT END) AS SALWT, WASTAGE AS SALWAST, WASTAGE AS SALWASPER,AMOUNT,S.RATE,ISNULL(DISCOUNT,0) AS OFFDISC "
        '    StrSql += vbCrLf + " 		FROM " & cnstockDb & "..ISSUE AS S INNER JOIN " & cnAdminDb & "..ITEMTAG AS T"
        '    StrSql += vbCrLf + " 		ON S.ITEMID = T.ITEMID AND S.TAGNO = T.TAGNO AND S.COMPANYID = T.COMPANYID"
        '    StrSql += vbCrLf + " WHERE S.TRANDATE BETWEEN '" & dtpFrom.ToString("yyyy-MM-dd") & "' "
        '    StrSql += vbCrLf + " AND '" & dtpTo.ToString("yyyy-MM-dd") & "'  AND S.COMPANYID IN (" & SelectedCompanyId & ") AND "
        '    StrSql += vbCrLf + " S.TRANTYPE IN (" & TranType & ") AND ISNULL(S.CANCEL,'') = '' "
        '    StrSql += strFilter
        '    StrSql += vbCrLf + " 	)X"
        '    StrSql += vbCrLf + " )Y GROUP BY CATCODE,METAL "
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    Cmd.ExecuteNonQuery()
        'End If


        StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND "
        StrSql += vbCrLf + " NAME = 'TEMP" & systemId & "ABSSALES') DROP TABLE TEMP" & systemId & "ABSSALES"
        If CategoryWise = True Then
            StrSql += vbCrLf + " SELECT CATCODE,' ' + CATNAME CATNAME"
        Else
            StrSql += vbCrLf + " SELECT METALID AS CATCODE,METALNAME CATNAME "
        End If
        StrSql += vbCrLf + "  ,ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, "

        StrSql += vbCrLf + " PAYMENT,AVERAGE , RATE, COLHEAD, RESULT, RESULT1,METALID,METALNAME INTO TEMP" & systemId & "ABSSALES FROM ("
        If CategoryWise = True Then
            StrSql += vbCrLf + " SELECT I.CATCODE, "
            If CategoryShortName = True Then
                StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME, "
            Else
                StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME, "
            End If
            StrSql += vbCrLf + " I.METALID, "
            StrSql += vbCrLf + " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS METALNAME, "
        Else
            StrSql += vbCrLf + " SELECT I.METALID CATCODE,' ' CATNAME,I.METALID, "
            StrSql += vbCrLf + " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS METALNAME, "
        End If

        StrSql += vbCrLf + " SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT, "
        StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
        StrSql += vbCrLf + " SUM(AMOUNT) "
        StrSql += vbCrLf + " AS RECEIPT, "
        StrSql += vbCrLf + " 0 AS PAYMENT, 1 AS RESULT,1 AS RESULT1, "
        StrSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(CASE WHEN SUM(GRSWT)<>0 THEN "
        StrSql += vbCrLf + " ((SUM(AMOUNT) - ISNULL(SUM(FIN_DISCOUNT),0)"
        'If MainTable = "ISSUE" Then
        '    StrSql += vbCrLf + " + ISNULL((SELECT DISC FROM MASTER..TEMP" & systemId & "ABSDISC WHERE METAL = I.METALID AND CATCODE = I.CATCODE),0) "
        'End If
        StrSql += vbCrLf + " )"
        StrSql += vbCrLf + " /(CASE WHEN SUM(ISNULL(GRSWT,0))=0 THEN 1 ELSE SUM(GRSWT) END)) "
        StrSql += vbCrLf + " ELSE "
        StrSql += vbCrLf + " ((SUM(AMOUNT) - ISNULL(SUM(FIN_DISCOUNT),0)"
        'If MainTable = "ISSUE" Then
        '    StrSql += vbCrLf + " + ISNULL((SELECT DISC FROM MASTER..TEMP" & systemId & "ABSDISC WHERE METAL = I.METALID AND CATCODE = I.CATCODE),0) "
        'End If
        StrSql += vbCrLf + " )"
        StrSql += vbCrLf + " /(CASE WHEN SUM(ISNULL(PCS,0))=0 THEN 1 ELSE SUM(PCS) END)) END)) "

        StrSql += vbCrLf + " )AS AVERAGE, "
        StrSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(SUM(RATE)/COUNT(RATE)))) AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "

        StrSql += vbCrLf + " FROM "
        StrSql += vbCrLf + " ("
        StrSql += vbCrLf + " SELECT PCS"
        If SeperateBeeds = True Then
            StrSql += vbCrLf + " ,CONVERT(NUMERIC(14,3),GRSWT - "
            StrSql += vbCrLf + " ISNULL((SELECT SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & ".." & StoneTable & " AS ISS WHERE ISSSNO  = I.SNO "
            StrSql += vbCrLf + "            AND "
            StrSql += vbCrLf + "            (EXISTS (SELECT 1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = ISS.STNITEMID AND ISNULL(BEEDS,'') = 'Y' AND ISS.STNSUBITEMID = 0)"
            StrSql += vbCrLf + "            OR EXISTS (SELECT 1 FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = ISS.STNITEMID AND SUBITEMID = ISS.STNSUBITEMID AND ISNULL(BEEDS,'') = 'Y' AND ISS.STNSUBITEMID <> 0)"
            StrSql += vbCrLf + "            )"
            StrSql += vbCrLf + "),0))AS GRSWT "
        Else
            StrSql += vbCrLf + " ,GRSWT"
        End If

        StrSql += vbCrLf + " ,NETWT"
        'If WithVat = True Then
        '    If RPT_SEPVAT_DABS = False Then
        '        StrSql += vbCrLf + " ,AMOUNT+ISNULL(TAX,0)+ISNULL(SC,0)+ISNULL(ADSC,0)"
        '    Else
        '        StrSql += vbCrLf + " ,AMOUNT"
        '    End If
        'Else
        StrSql += vbCrLf + " ,AMOUNT"
        'End If
        StrSql += vbCrLf + " -ISNULL("
        StrSql += vbCrLf + "(SELECT SUM(STNAMT) FROM " & cnStockDb & ".." & StoneTable & " WHERE ISSSNO = I.SNO "
        StrSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN ('D','P'))"
        StrSql += vbCrLf + "),0)"
        StrSql += vbCrLf + " AS AMOUNT,RATE,CATCODE,METALID,FIN_DISCOUNT "
        StrSql += vbCrLf + " FROM " & cnStockDb & ".." & MainTable & " I "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
        StrSql += vbCrLf + " I.TRANTYPE IN (" & TranType & ") AND ISNULL(I.CANCEL,'') = '' "
        StrSql += Replace(strFilter, "SYSTEMID", "I.SYSTEMID")
        If OrMast = True Then StrSql += vbCrLf + " AND I.BATCHNO IN (SELECT BATCHNO FROM " & cnadmindb & "..ORMAST WHERE ISNULL(ORDCANCEL,'') = '')"
        StrSql += vbCrLf + " )AS I"
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
        If CategoryWise = True Then
            StrSql += vbCrLf + " GROUP BY I.CATCODE,C.METALID,I.METALID "
        Else
            StrSql += vbCrLf + " GROUP BY I.METALID,I.CATCODE "
        End If



        StrSql += vbCrLf + " UNION ALL "
        If CategoryWise = True Then
            StrSql += vbCrLf + " SELECT CATCODE,CATNAME,(SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE ISNULL(CATCODE,'') = Y.CATCODE) METALID"
            StrSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ISNULL(METALID,'') IN (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE ISNULL(CATCODE,'') = Y.CATCODE)) METALNAME"
        Else
            StrSql += vbCrLf + " SELECT (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE ISNULL(CATCODE,'') = Y.CATCODE) CATCODE,CATNAME,(SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE ISNULL(CATCODE,'') = Y.CATCODE) METALID"
            StrSql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ISNULL(METALID,'') IN (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE ISNULL(CATCODE,'') = Y.CATCODE)) METALNAME"
        End If
        StrSql += " , SUM(ISSPCS) AS ISSPCS, "
        StrSql += vbCrLf + " SUM(ISSGRSWT) AS ISSGRSWT, SUM(ISSNETWT) AS ISSNETWT,"
        StrSql += vbCrLf + " SUM(RECPCS), SUM(RECGRSWT), SUM(RECNETWT), SUM(RECEIPT), "
        StrSql += vbCrLf + " SUM(PAYMENT), RESULT, 1 AS RESULT1, AVERAGE, RATE, COLHEAD FROM ("
        If CategoryWise = True Then
            StrSql += vbCrLf + " SELECT S.CATCODE,"
            If CategoryShortName = True Then
                StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY  "
            Else
                StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY  "
            End If
            StrSql += vbCrLf + " WHERE CATCODE = S.CATCODE) CATNAME,  "
        Else
            StrSql += vbCrLf + " SELECT S.CATCODE, (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = S.CATCODE) AS CATNAME,  "
        End If
        StrSql += vbCrLf + " SUM(S.STNPCS) ISSPCS,  "
        StrSql += vbCrLf + " SUM(S.STNWT) AS ISSGRSWT,  "
        StrSql += vbCrLf + " SUM(S.STNWT) AS ISSNETWT,  "
        StrSql += vbCrLf + " 0 RECPCS,  0 AS RECGRSWT,  0 AS RECNETWT,  SUM(S.STNAMT) AS RECEIPT,  "
        StrSql += vbCrLf + " 0 AS PAYMENT,  2 AS RESULT, '' AS AVERAGE, '' AS RATE, "
        StrSql += vbCrLf + " 'D' COLHEAD  FROM " & cnStockDb & ".." & StoneTable & " S "
        StrSql += vbCrLf + " WHERE S.BATCHNO IN ( SELECT BATCHNO FROM " & cnStockDb & ".." & MainTable & " I "
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND  '" & dtpTo.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " " & Replace(strFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(TRANTYPE,'') IN (" & TranType & ") AND ISNULL(CANCEL,'') = ''  AND COMPANYID IN (" & SelectedCompanyId & ") "
        If OrMast = True Then StrSql += vbCrLf + " AND I.BATCHNO IN (SELECT BATCHNO FROM " & cnadmindb & "..ORMAST WHERE ISNULL(ORDCANCEL,'') = '')"
        StrSql += vbCrLf + " )"
        StrSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompanyId & ") AND  (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) IN ('D','P')  "
        If CategoryWise = True Then
            StrSql += vbCrLf + " GROUP BY S.CATCODE,S.BATCHNO  "
        Else
            StrSql += vbCrLf + " GROUP BY S.BATCHNO,S.ISSSNO,S.CATCODE  "
        End If
        StrSql += vbCrLf + " ) Y GROUP BY CATCODE, CATNAME, AVERAGE, RATE, COLHEAD, RESULT"
        StrSql += vbCrLf + " ) X "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALES(CATNAME, "
        StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,"
        StrSql += vbCrLf + " RESULT, RESULT1, COLHEAD) "
        StrSql += vbCrLf + " SELECT '" & TranMode & " ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' CATNAME, 0 ISSPCS, 0 ISSGRSWT, 0 ISSNETWT, "
        StrSql += vbCrLf + " 0 RECPCS, 0 RECGRSWT, 0 RECNETWT, 0 RECEIPT, 0 PAYMENT, -1 RESULT, -1 RESULT1, 'T' COLHEAD "
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES WHERE RESULT IN (1,2,3)"
        StrSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        If CategoryWise = True Then
            StrSql = vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALES(CATCODE,CATNAME, "
            StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,AVERAGE,RATE,COLHEAD"
            StrSql += vbCrLf + " ,RESULT, RESULT1,METALID,METALNAME) "
            'StrSql += vbCrLf + " SELECT "
            'StrSql += vbCrLf + " 	DISTINCT '' CATCODE,METALNAME CATNAME,NULL ISSPCS,NULL ISSGRSWT,NULL ISSNETWT,NULL RECPCS,NULL RECGRSWT"
            'StrSql += vbCrLf + " 	,NULL RECNETWT,NULL RECEIPT,NULL PAYMENT,NULL AVERAGE,NULL RATE,'T' COLHEAD,0 RESULT,1 RESULT1"
            'StrSql += vbCrLf + " 	,METALID,METALNAME"
            'StrSql += vbCrLf + " FROM MASTER..TEMP" & systemId & "ABSSALES"
            'StrSql += vbCrLf + " UNION ALL"
            StrSql += vbCrLf + " SELECT"
            StrSql += vbCrLf + " 	'' CATCODE,METALNAME + ' TOTAL' CATNAME,SUM(ISSPCS) ISSPCS,SUM(ISSGRSWT) ISSGRSWT,SUM(ISSNETWT) ISSNETWT"
            StrSql += vbCrLf + " 	,SUM(RECPCS) RECPCS,SUM(RECGRSWT) RECGRSWT,SUM(RECNETWT) RECNETWT,SUM(RECEIPT) RECEIPT"
            StrSql += vbCrLf + " 	,SUM(PAYMENT) PAYMENT,NULL AVERAGE,NULL RATE,'S' COLHEAD,4 RESULT,1 RESULT1,METALID,METALNAME"
            StrSql += vbCrLf + " FROM MASTER..TEMP" & systemId & "ABSSALES"
            StrSql += vbCrLf + " GROUP BY METALID,METALNAME"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If


        StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSSALES)>0 "
        StrSql += vbCrLf + " BEGIN "
        StrSql += vbCrLf + " INSERT INTO TEMP" & systemId & "ABSSALES(CATCODE,CATNAME,ISSPCS, ISSGRSWT, ISSNETWT, "
        StrSql += vbCrLf + " RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT, RESULT, RESULT1, COLHEAD) "
        StrSql += vbCrLf + " SELECT 'Z' AS CATCODE, '" & TranMode & " TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
        StrSql += vbCrLf + " SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), "
        StrSql += vbCrLf + " SUM(PAYMENT),5 RESULT,2 RESULT1, 'G' COLHEAD "
        If CategoryWise = True Then
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES WHERE ISNULL(COLHEAD,'') = 'S'"
        Else
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES WHERE ISNULL(COLHEAD,'') = ''"
        End If
        StrSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " SELECT "
        StrSql += vbCrLf + " 	(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = S.CATCODE)) STONENAME"
        StrSql += vbCrLf + " 	,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID) + ' PCS/WT/AMT ' +"
        StrSql += vbCrLf + " 	CONVERT(VARCHAR,SUM(PCS)) + '/ ' + CONVERT(VARCHAR,SUM(NETWT)) + '/ ' + CONVERT(VARCHAR,SUM(AMOUNT) - SUM(S.STNAMT)) MTDETAIL"
        StrSql += vbCrLf + " FROM " & cnStockDb & ".." & MainTable & " AS I INNER JOIN " & cnStockDb & ".." & StoneTable & " AS S"
        StrSql += vbCrLf + " ON I.SNO = S.ISSSNO AND I.BATCHNO = S.BATCHNO"
        StrSql += vbCrLf + " WHERE S.COMPANYID IN (" & SelectedCompanyId & ") AND (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) IN ('D','P')"
        StrSql += vbCrLf + " AND I.TRANDATE BETWEEN '" & dtpFrom.ToString("yyyy-MM-dd") & "' AND  '" & dtpTo.ToString("yyyy-MM-dd") & "'"
        Dim strrfilter As String = Replace(strFilter, "COSTID", "I.COSTID")
        StrSql += vbCrLf + " " & Replace(strrfilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(I.TRANTYPE,'') IN (" & TranType & ") AND ISNULL(CANCEL,'') = ''  AND I.COMPANYID IN (" & SelectedCompanyId & ")"
        If OrMast = True Then StrSql += vbCrLf + " AND I.BATCHNO IN (SELECT BATCHNO FROM " & cnadmindb & "..ORMAST WHERE ISNULL(ORDCANCEL,'') = '')"
        StrSql += vbCrLf + " GROUP BY I.METALID,S.CATCODE"
        Dim dtIssStMetal As New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        da.Fill(dtIssStMetal)

        StrSql = vbCrLf + " SELECT CATNAME, "

        If ViewMode = "P" Then
            StrSql += vbCrLf + " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
            StrSql += vbCrLf + " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
            StrSql += vbCrLf + " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "

            StrSql += vbCrLf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
            StrSql += vbCrLf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
            StrSql += vbCrLf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "

            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
        Else
            StrSql += vbCrLf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END ISSPCS, "
            StrSql += vbCrLf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END ISSGRSWT, "
            StrSql += vbCrLf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END ISSNETWT, "

            StrSql += vbCrLf + " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END RECPCS, "
            StrSql += vbCrLf + " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END RECGRSWT, "
            StrSql += vbCrLf + " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END RECNETWT, "

            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END PAYMENT, "

        End If

        'StrSql += vbCrLf + " CASE WHEN RESULT NOT IN (-1,0,4,5) THEN CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(ISSNETWT,0) = 0 THEN (RECEIPT / CASE WHEN ISSPCS = 0 THEN 1 ELSE ISSPCS END) ELSE (RECEIPT/ ISSNETWT) / (CASE WHEN METALID = 'G' THEN  " & IIf(AvgGoldRate = 0, 1, AvgGoldRate) & " WHEN METALID = 'S' THEN " & IIf(AvgSiRate - CalcRedcRate = 0, 1, AvgSiRate - CalcRedcRate) & " ELSE 1 END) END) ELSE NULL END * 100 AVERAGE, COLHEAD, RESULT1"
        StrSql += vbCrLf + " CASE WHEN RESULT NOT IN (-1,0,4,5) THEN "
        StrSql += vbCrLf + " 	CONVERT(NUMERIC(15,2),CASE WHEN METALID = 'G' THEN (CASE WHEN ISNULL(ISSNETWT,0) = 0 THEN "
        StrSql += vbCrLf + " 														(RECEIPT / CASE WHEN ISSPCS = 0 THEN 1 ELSE ISSPCS END) "
        StrSql += vbCrLf + " 													   ELSE (RECEIPT/ ISSNETWT) / (" & IIf(AvgGoldRate = 0, 1, AvgGoldRate) & ")									"
        StrSql += vbCrLf + " 													   END)"
        StrSql += vbCrLf + " 														 * 100 "
        StrSql += vbCrLf + " 							   WHEN METALID = 'S' THEN (CASE WHEN ISNULL(ISSNETWT,0) = 0 THEN "
        StrSql += vbCrLf + " 														(RECEIPT / CASE WHEN ISSPCS = 0 THEN 1 ELSE ISSPCS END) "
        StrSql += vbCrLf + " 													   ELSE (RECEIPT/ ISSNETWT) - (" & IIf(AvgSiRate - CalcRedcRate = 0, 1, AvgSiRate - CalcRedcRate) & ")									"
        StrSql += vbCrLf + " 													   END)							   "
        StrSql += vbCrLf + " 														"
        StrSql += vbCrLf + " 						 ELSE (CASE WHEN ISNULL(ISSNETWT,0) = 0 THEN "
        StrSql += vbCrLf + " 								(RECEIPT / CASE WHEN ISSPCS = 0 THEN 1 ELSE ISSPCS END) "
        StrSql += vbCrLf + " 							   ELSE (RECEIPT/ ISSNETWT) 									"
        StrSql += vbCrLf + " 							   END) "
        StrSql += vbCrLf + " 						 END)"
        StrSql += vbCrLf + " ELSE NULL "
        StrSql += vbCrLf + " END AVERAGE, COLHEAD, RESULT1"
        'StrSql += vbCrLf + " CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END, COLHEAD, RESULT1 "


        StrSql += vbCrLf + " , METALNAME"
        If CategoryWise = True Then
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES WHERE ISNULL(CATNAME,'') <> '' ORDER BY RESULT1,METALNAME,RESULT,CATNAME"
        Else
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSSALES ORDER BY CATCODE,RESULT"
        End If
        Dim dtAbsSales As DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        dtAbsSales = New DataTable
        da.Fill(dtAbsSales)

        Dim dtRow() As DataRow

        If dtAbsSales.Rows.Count > 0 Then
            For cnt As Integer = 0 To dtAbsSales.Rows.Count - 1
                If DotMatrix = True Then
                    If (dtAbsSales.Rows(cnt).Item("RESULT1").ToString = -1 And dtAbsSales.Rows(cnt).Item("COLHEAD").ToString = "T") Then
                        oWrite.Write(pBoldDoubleStart)
                        WriteLine(LSet(dtAbsSales.Rows(cnt).Item("CATNAME").ToString, oCharPerLine))
                        oWrite.Write(pBoldDoubleEnd)
                    ElseIf (dtAbsSales.Rows(cnt).Item("RESULT1").ToString = 2 And dtAbsSales.Rows(cnt).Item("COLHEAD").ToString = "G") Then
                        WriteLine(LSet("", PreSpaceLine) + PrintLine_WithOutWrite(oCharPerLine - PreSpaceLine, "-"))
                        oWrite.Write(pBoldDoubleStart)
                        WriteLine(LSet(dtAbsSales.Rows(cnt).Item("CATNAME").ToString, LenDesc) + RSet(dtAbsSales.Rows(cnt).Item("ISSPCS").ToString, LenIPcs) + RSet(dtAbsSales.Rows(cnt).Item("ISSGRSWT").ToString, LenIGwt) + RSet(dtAbsSales.Rows(cnt).Item("ISSNETWT").ToString, LenINwt) + RSet(dtAbsSales.Rows(cnt).Item("RECPCS").ToString, LenRPcs) + RSet(dtAbsSales.Rows(cnt).Item("RECGRSWT").ToString, LenRGwt) + RSet(dtAbsSales.Rows(cnt).Item("RECNETWT").ToString, LenRNwt) + RSet(dtAbsSales.Rows(cnt).Item("RECEIPT").ToString, LenRec) + RSet(dtAbsSales.Rows(cnt).Item("PAYMENT").ToString, LenPay) + RSet(dtAbsSales.Rows(cnt).Item("AVERAGE").ToString, LenAvg))
                        oWrite.Write(pBoldDoubleEnd)
                        WriteLine(LSet("", PreSpaceLine) + PrintLine_WithOutWrite(oCharPerLine - PreSpaceLine, "-"))
                        If CategoryWise = False Then
                            MetalAmtTotRec += Convert.ToDouble(IIf(dtAbsSales.Rows(cnt).Item("RECEIPT").ToString = "", "0", dtAbsSales.Rows(cnt).Item("RECEIPT").ToString))
                            MetalAmtTotPay += Convert.ToDouble(IIf(dtAbsSales.Rows(cnt).Item("PAYMENT").ToString = "", "0", dtAbsSales.Rows(cnt).Item("PAYMENT").ToString))
                        End If
                    ElseIf dtAbsSales.Rows(cnt).Item("COLHEAD").ToString = "T" Then
                        oWrite.Write(pBoldStart)
                        WriteLine(LSet(dtAbsSales.Rows(cnt).Item("CATNAME").ToString, oCharPerLine))
                        oWrite.Write(pBoldEnd)
                    ElseIf dtAbsSales.Rows(cnt).Item("COLHEAD").ToString = "S" Then
                        oWrite.Write(pBoldStart)
                        dtRow = dtIssStMetal.Select("STONENAME = '" & dtAbsSales.Rows(cnt).Item("METALNAME").ToString() & "'")
                        If dtRow.Length > 0 Then
                            For Each row As DataRow In dtRow
                                WriteLine(LSet(row.Item("MTDETAIL").ToString, oCharPerLine))
                            Next
                        End If
                        If CategoryWise = True Then
                            MetalAmtTotRec += Convert.ToDouble(IIf(dtAbsSales.Rows(cnt).Item("RECEIPT").ToString = "", "0", dtAbsSales.Rows(cnt).Item("RECEIPT").ToString))
                            MetalAmtTotPay += Convert.ToDouble(IIf(dtAbsSales.Rows(cnt).Item("PAYMENT").ToString = "", "0", dtAbsSales.Rows(cnt).Item("PAYMENT").ToString))
                        End If

                        oWrite.Write(pBoldEnd)
                        WriteLine(LSet("", PreSpaceLine) + PrintLine_WithOutWrite(oCharPerLine - PreSpaceLine, "-"))
                        oWrite.Write(pBoldStart)
                        WriteLine(LSet(dtAbsSales.Rows(cnt).Item("CATNAME").ToString, LenDesc) + RSet(dtAbsSales.Rows(cnt).Item("ISSPCS").ToString, LenIPcs) + RSet(dtAbsSales.Rows(cnt).Item("ISSGRSWT").ToString, LenIGwt) + RSet(dtAbsSales.Rows(cnt).Item("ISSNETWT").ToString, LenINwt) + RSet(dtAbsSales.Rows(cnt).Item("RECPCS").ToString, LenRPcs) + RSet(dtAbsSales.Rows(cnt).Item("RECGRSWT").ToString, LenRGwt) + RSet(dtAbsSales.Rows(cnt).Item("RECNETWT").ToString, LenRNwt) + RSet(dtAbsSales.Rows(cnt).Item("RECEIPT").ToString, LenRec) + RSet(dtAbsSales.Rows(cnt).Item("PAYMENT").ToString, LenPay) + RSet(dtAbsSales.Rows(cnt).Item("AVERAGE").ToString, LenAvg))
                        oWrite.Write(pBoldEnd)
                    Else
                        WriteLine(LSet(dtAbsSales.Rows(cnt).Item("CATNAME").ToString, LenDesc) + RSet(dtAbsSales.Rows(cnt).Item("ISSPCS").ToString, LenIPcs) + RSet(dtAbsSales.Rows(cnt).Item("ISSGRSWT").ToString, LenIGwt) + RSet(dtAbsSales.Rows(cnt).Item("ISSNETWT").ToString, LenINwt) + RSet(dtAbsSales.Rows(cnt).Item("RECPCS").ToString, LenRPcs) + RSet(dtAbsSales.Rows(cnt).Item("RECGRSWT").ToString, LenRGwt) + RSet(dtAbsSales.Rows(cnt).Item("RECNETWT").ToString, LenRNwt) + RSet(dtAbsSales.Rows(cnt).Item("RECEIPT").ToString, LenRec) + RSet(dtAbsSales.Rows(cnt).Item("PAYMENT").ToString, LenPay) + RSet(dtAbsSales.Rows(cnt).Item("AVERAGE").ToString, LenAvg))
                    End If
                Else

                    If dtAbsSales.Rows(cnt).Item("COLHEAD").ToString = "S" Then
                        dtRow = dtIssStMetal.Select("STONENAME = '" & dtAbsSales.Rows(cnt).Item("METALNAME").ToString() & "'")
                        If dtRow.Length > 0 Then
                            For Each row As DataRow In dtRow
                                drTableAdd = dtTableAdd.NewRow
                                drTableAdd.Item("DESC") = row.Item("MTDETAIL").ToString
                                drTableAdd.Item("IPCS") = ""
                                drTableAdd.Item("IGWT") = ""
                                drTableAdd.Item("INWT") = ""
                                drTableAdd.Item("RPCS") = ""
                                drTableAdd.Item("RGWT") = ""
                                drTableAdd.Item("RNWT") = ""
                                drTableAdd.Item("REC") = ""
                                drTableAdd.Item("PAY") = ""
                                drTableAdd.Item("AVERAGE") = ""
                                drTableAdd.Item("RESULT1") = ""
                                drTableAdd.Item("COLHEAD") = "S"
                                dtTableAdd.Rows.Add(drTableAdd)
                                dtTableAdd.AcceptChanges()
                            Next
                        End If
                    End If

                    drTableAdd = dtTableAdd.NewRow
                    drTableAdd.Item("DESC") = dtAbsSales.Rows(cnt).Item("CATNAME").ToString
                    drTableAdd.Item("IPCS") = dtAbsSales.Rows(cnt).Item("ISSPCS").ToString
                    drTableAdd.Item("IGWT") = dtAbsSales.Rows(cnt).Item("ISSGRSWT").ToString
                    drTableAdd.Item("INWT") = dtAbsSales.Rows(cnt).Item("ISSNETWT").ToString
                    drTableAdd.Item("RPCS") = dtAbsSales.Rows(cnt).Item("RECPCS").ToString
                    drTableAdd.Item("RGWT") = dtAbsSales.Rows(cnt).Item("RECGRSWT").ToString
                    drTableAdd.Item("RNWT") = dtAbsSales.Rows(cnt).Item("RECNETWT").ToString
                    drTableAdd.Item("REC") = dtAbsSales.Rows(cnt).Item("RECEIPT").ToString
                    drTableAdd.Item("PAY") = dtAbsSales.Rows(cnt).Item("PAYMENT").ToString
                    drTableAdd.Item("AVERAGE") = dtAbsSales.Rows(cnt).Item("AVERAGE").ToString
                    drTableAdd.Item("RESULT1") = dtAbsSales.Rows(cnt).Item("RESULT1").ToString
                    If (dtAbsSales.Rows(cnt).Item("RESULT1").ToString = -1 And dtAbsSales.Rows(cnt).Item("COLHEAD").ToString = "T") Then
                        drTableAdd.Item("COLHEAD") = "T"
                    ElseIf (dtAbsSales.Rows(cnt).Item("RESULT1").ToString = 2 And dtAbsSales.Rows(cnt).Item("COLHEAD").ToString = "G") Then
                        drTableAdd.Item("COLHEAD") = "G"
                        If CategoryWise = False Then
                            MetalAmtTotRec += Convert.ToDouble(IIf(dtAbsSales.Rows(cnt).Item("RECEIPT").ToString = "", "0", dtAbsSales.Rows(cnt).Item("RECEIPT").ToString))
                            MetalAmtTotPay += Convert.ToDouble(IIf(dtAbsSales.Rows(cnt).Item("PAYMENT").ToString = "", "0", dtAbsSales.Rows(cnt).Item("PAYMENT").ToString))
                        End If
                    ElseIf dtAbsSales.Rows(cnt).Item("COLHEAD").ToString = "T" Then
                        drTableAdd.Item("COLHEAD") = "S"
                    ElseIf dtAbsSales.Rows(cnt).Item("COLHEAD").ToString = "S" Then
                        If CategoryWise = True Then
                            MetalAmtTotRec += Convert.ToDouble(IIf(dtAbsSales.Rows(cnt).Item("RECEIPT").ToString = "", "0", dtAbsSales.Rows(cnt).Item("RECEIPT").ToString))
                            MetalAmtTotPay += Convert.ToDouble(IIf(dtAbsSales.Rows(cnt).Item("PAYMENT").ToString = "", "0", dtAbsSales.Rows(cnt).Item("PAYMENT").ToString))
                        End If
                        drTableAdd.Item("COLHEAD") = "S"
                    Else
                        drTableAdd.Item("COLHEAD") = ""
                    End If
                    dtTableAdd.Rows.Add(drTableAdd)
                    dtTableAdd.AcceptChanges()
                End If
            Next cnt
        End If
    End Sub

    Public Sub Vat(ByVal TranType As String, ByVal TranMode As String, ByVal ViewMode As String, ByVal MainTable As String, ByVal StoneTable As String, ByVal Out As Boolean, Optional ByVal OrMast As Boolean = False)
        If WithVat = True Then
            ''TAX

            If CategoryWise = True Then
                StrSql = vbCrLf + " SELECT I.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + " WHERE ACCODE = C.STAXID) AS CATNAME, "
                StrSql += vbCrLf + " I.METALID,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS METALNAME, "
            Else
                StrSql = vbCrLf + " SELECT I.METALID, 'TAX' AS METALNAME,I.METALID,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS METALNAME, "
            End If
            StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + " SUM(TAX) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + " FROM " & cnStockDb & ".." & MainTable & " I"
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
            StrSql += vbCrLf + " I.TRANTYPE IN (" & TranType & ") AND ISNULL(I.CANCEL,'') = '' "
            StrSql += Replace(strFilter, "SYSTEMID", "I.SYSTEMID")
            If OrMast = True Then StrSql += vbCrLf + " AND I.BATCHNO IN (SELECT BATCHNO FROM " & cnadmindb & "..ORMAST WHERE ISNULL(ORDCANCEL,'') = '')"
            If CategoryWise = True Then
                StrSql += vbCrLf + " GROUP BY I.CATCODE, C.STAXID,I.METALID HAVING SUM(TAX) <> 0"
            Else
                StrSql += vbCrLf + " GROUP BY I.METALID "
            End If

            '' SURCHARGE 
            StrSql += vbCrLf + " UNION ALL "
            If CategoryWise = True Then
                StrSql += vbCrLf + " SELECT I.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + " WHERE ACCODE = C.SSCID) AS CATNAME,I.METALID,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS METALNAME,"
            Else
                StrSql += vbCrLf + " SELECT I.METALID, 'SURCHARGE' AS CATNAME,I.METALID,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS METALNAME, "
            End If
            StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + " SUM(SC) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + " FROM " & cnStockDb & ".." & MainTable & " I"
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
            StrSql += vbCrLf + " I.TRANTYPE IN (" & TranType & ") AND ISNULL(I.CANCEL,'') = '' "
            StrSql += Replace(strFilter, "SYSTEMID", "I.SYSTEMID")
            StrSql += vbCrLf + " AND I.BATCHNO IN (SELECT BATCHNO FROM " & cnAdminDb & "..ORMAST WHERE ISNULL(ORDCANCEL,'') = '')"
            If CategoryWise = True Then
                StrSql += vbCrLf + " GROUP BY I.CATCODE, C.SSCID,I.METALID HAVING SUM(SC) <> 0"
            Else
                StrSql += vbCrLf + " GROUP BY I.METALID  HAVING SUM(SC) <> 0"
            End If

            '' ADDTIONAL SURCHARGE 
            StrSql += vbCrLf + " UNION ALL "
            If CategoryWise = True Then
                StrSql += vbCrLf + " SELECT I.CATCODE, (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
                StrSql += vbCrLf + " WHERE ACCODE = C.SASCID) AS CATNAME,I.METALID,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS METALNAME, "
            Else
                StrSql += vbCrLf + " SELECT I.METALID, 'ADDTIONALSURCHARGE' AS CATNAME,I.METALID,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS METALNAME, "
            End If
            StrSql += vbCrLf + " 0 AS ISSPCS, 0 AS ISSGRSWT, 0 AS ISSNETWT, "
            StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
            StrSql += vbCrLf + " SUM(ADSC) AS RECEIPT, 0 AS PAYMENT, 3 AS RESULT, 1 AS RESULT1, '' AS AVERAGE, '' AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
            StrSql += vbCrLf + " FROM " & cnStockDb & ".." & MainTable & " I"
            StrSql += vbCrLf + " JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
            StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.ToString("yyyy-MM-dd") & "' "
            StrSql += vbCrLf + " AND '" & dtpTo.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
            StrSql += vbCrLf + " I.TRANTYPE IN (" & TranType & ") AND ISNULL(I.CANCEL,'') = '' "
            StrSql += strFilter
            If OrMast = True Then StrSql += vbCrLf + " AND I.BATCHNO IN (SELECT BATCHNO FROM " & cnAdminDb & "..ORMAST WHERE ISNULL(ORDCANCEL,'') = '')"
            If CategoryWise = True Then
                StrSql += vbCrLf + " GROUP BY I.CATCODE, C.SASCID,I.METALID HAVING SUM(ADSC) <> 0"
            Else
                StrSql += vbCrLf + " GROUP BY I.METALID HAVING SUM(ADSC) <> 0 "
            End If
            Dim dtAbsSales As DataTable
            da = New OleDbDataAdapter(StrSql, cn)
            dtAbsSales = New DataTable
            da.Fill(dtAbsSales)
            If dtAbsSales.Rows.Count > 0 Then
                For cnt As Integer = 0 To dtAbsSales.Rows.Count - 1
                    If TranMode = "SALES" Then
                        VatSales += dtAbsSales.Rows(cnt).Item("RECEIPT").ToString
                        MetalAmtTotRec += Convert.ToDouble(IIf(dtAbsSales.Rows(cnt).Item("RECEIPT").ToString = "", "0", dtAbsSales.Rows(cnt).Item("RECEIPT").ToString))
                    Else
                        VatSR += dtAbsSales.Rows(cnt).Item("RECEIPT").ToString
                        MetalAmtTotPay += Convert.ToDouble(IIf(dtAbsSales.Rows(cnt).Item("RECEIPT").ToString = "", "0", dtAbsSales.Rows(cnt).Item("RECEIPT").ToString))
                    End If
                Next
            End If

            If Out = True Then
                If DotMatrix = True Then
                    WriteLine(LSet("Sales - SR. Vat", LenDesc) + RSet("", LenIPcs) + RSet("", LenIGwt) + RSet("", LenINwt) + RSet("", LenRPcs) + RSet("", LenRGwt) + RSet("", LenRNwt) + RSet(IIf(VatSales > VatSR, VatSales - VatSR, ""), LenRec) + RSet(IIf(VatSales < VatSR, VatSR - VatSales, ""), LenPay) + RSet("", LenAvg))
                Else
                    drTableAdd = dtTableAdd.NewRow
                    drTableAdd.Item("DESC") = "Sales - SR. Vat"
                    drTableAdd.Item("IPCS") = ""
                    drTableAdd.Item("IGWT") = ""
                    drTableAdd.Item("INWT") = ""
                    drTableAdd.Item("RPCS") = ""
                    drTableAdd.Item("RGWT") = ""
                    drTableAdd.Item("RNWT") = ""
                    drTableAdd.Item("REC") = IIf(VatSales > VatSR, Convert.ToDouble(VatSales - VatSR).ToString("0.00"), "")
                    drTableAdd.Item("PAY") = IIf(VatSales < VatSR, Convert.ToDouble(VatSR - VatSales).ToString("0.00"), "")
                    drTableAdd.Item("AVERAGE") = ""
                    drTableAdd.Item("RESULT1") = ""
                    drTableAdd.Item("COLHEAD") = "S"
                    dtTableAdd.Rows.Add(drTableAdd)
                    dtTableAdd.AcceptChanges()
                End If
            End If
        End If
    End Sub


    Private Sub ProcRepairDelivery()
        StrSql = "IF EXISTS(SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND "
        StrSql += vbCrLf + " NAME = 'TEMP" & systemId & "ABSREPAIR') DROP TABLE TEMP" & systemId & "ABSREPAIR"

        If CategoryWise = True Then
            StrSql += vbCrLf + " SELECT CATCODE, "
        Else
            StrSql += vbCrLf + " SELECT METALID AS CATCODE, "
        End If
        StrSql += vbCrLf + " CATNAME, ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, "
        StrSql += vbCrLf + " PAYMENT, AVERAGE, RATE, COLHEAD, RESULT, RESULT1,METALID,METALNAME INTO TEMP" & systemId & "ABSREPAIR FROM ("
        If CategoryWise = True Then
            StrSql += vbCrLf + " SELECT I.CATCODE, "
            If CategoryShortName = True Then
                StrSql += vbCrLf + " (SELECT SHORTNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME, "
            Else
                StrSql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS CATNAME, "
            End If
            StrSql += vbCrLf + " I.METALID, "
            StrSql += vbCrLf + " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS METALNAME, "
        Else
            StrSql += vbCrLf + " SELECT I.METALID CATCODE,' ' CATNAME,I.METALID, "
            StrSql += vbCrLf + " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS METALNAME, "
            'StrSql += vbCrLf + " SELECT I.METALID, "
            'StrSql += vbCrLf + " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID)AS CATNAME, "
        End If

        StrSql += vbCrLf + " SUM(PCS) AS ISSPCS, SUM(GRSWT) AS ISSGRSWT, SUM(NETWT) AS ISSNETWT, "
        StrSql += vbCrLf + " 0 AS RECPCS, 0 AS RECGRSWT, 0 AS RECNETWT, "
        StrSql += vbCrLf + " SUM(AMOUNT) "
        StrSql += vbCrLf + " AS RECEIPT, "
        StrSql += vbCrLf + " 0 AS PAYMENT, 1 AS RESULT,1 AS RESULT1, "
        StrSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(CASE WHEN SUM(GRSWT)<>0 THEN "
        StrSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(GRSWT,0))=0 THEN 1 ELSE SUM(GRSWT) END)) "
        StrSql += vbCrLf + " ELSE "
        StrSql += vbCrLf + " (SUM(AMOUNT)/(CASE WHEN SUM(ISNULL(PCS,0))=0 THEN 1 ELSE SUM(PCS) END)) END))) AS AVERAGE, "
        StrSql += vbCrLf + " CONVERT(VARCHAR(25),CONVERT(NUMERIC(15,2),(SUM(RATE)/COUNT(RATE)))) AS RATE, CONVERT(VARCHAR(3),'') COLHEAD "
        StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I "
        StrSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY C ON C.CATCODE = I.CATCODE"
        StrSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.ToString("yyyy-MM-dd") & "' "
        StrSql += vbCrLf + " AND '" & dtpTo.ToString("yyyy-MM-dd") & "'  AND COMPANYID IN (" & SelectedCompanyId & ") AND "
        StrSql += vbCrLf + " I.TRANTYPE IN('RD') AND ISNULL(I.CANCEL,'') = '' "
        StrSql += Replace(strFilter, "SYSTEMID", "I.SYSTEMID")

        If CategoryWise = True Then
            StrSql += vbCrLf + " GROUP BY I.CATCODE,C.METALID,I.METALID "
        Else
            StrSql += vbCrLf + " GROUP BY I.METALID "
        End If
        StrSql += vbCrLf + " ) X "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        If CategoryWise = True Then
            StrSql = vbCrLf + " INSERT INTO TEMP" & systemId & "ABSREPAIR(CATCODE,CATNAME, "
            StrSql += vbCrLf + " ISSPCS, ISSGRSWT, ISSNETWT, RECPCS, RECGRSWT, RECNETWT, RECEIPT, PAYMENT,AVERAGE,RATE,COLHEAD"
            StrSql += vbCrLf + " ,RESULT, RESULT1,METALID,METALNAME) "
            'StrSql += vbCrLf + " SELECT "
            'StrSql += vbCrLf + " 	DISTINCT '' CATCODE,METALNAME CATNAME,0 ISSPCS,0 ISSGRSWT,0 ISSNETWT,0 RECPCS,0 RECGRSWT"
            'StrSql += vbCrLf + " 	,0 RECNETWT,0 RECEIPT,0 PAYMENT,0 AVERAGE,0 RATE,'T' COLHEAD,2 RESULT,0 RESULT1"
            'StrSql += vbCrLf + " 	,METALID,METALNAME"
            'StrSql += vbCrLf + " FROM MASTER..TEMP" & systemId & "ABSREPAIR"
            'StrSql += vbCrLf + " UNION ALL"
            StrSql += vbCrLf + " SELECT"
            StrSql += vbCrLf + " 	'' CATCODE,METALNAME + ' TOTAL' CATNAME,SUM(ISSPCS) ISSPCS,SUM(ISSGRSWT) ISSGRSWT,SUM(ISSNETWT) ISSNETWT"
            StrSql += vbCrLf + " 	,SUM(RECPCS) RECPCS,SUM(RECGRSWT) RECGRSWT,SUM(RECNETWT) RECNETWT,SUM(RECEIPT) RECEIPT"
            StrSql += vbCrLf + " 	,SUM(PAYMENT) PAYMENT,NULL AVERAGE,NULL RATE,'S' COLHEAD,2 RESULT,2 RESULT1,METALID,METALNAME"
            StrSql += vbCrLf + " FROM MASTER..TEMP" & systemId & "ABSREPAIR"
            StrSql += vbCrLf + " GROUP BY METALID,METALNAME"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()
        End If


        StrSql = vbCrLf + " SELECT 'REPAIR DET ['+ CONVERT(VARCHAR,SUM(RECEIPT))+']' DESCRIPTION"
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSREPAIR WHERE RESULT1 = 1 AND COLHEAD <> 'D'"
        Dim dtRepairDel As New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        dtRepairDel = New DataTable
        da.Fill(dtRepairDel)

        Dim dtRow() As DataRow

        If dtRepairDel.Rows.Count > 0 Then
            If DotMatrix = True Then
                oWrite.Write(pBoldDoubleStart)
                WriteLine(LSet(dtRepairDel.Rows(0).Item("DESCRIPTION").ToString, oCharPerLine))
                oWrite.Write(pBoldDoubleEnd)
            Else
                drTableAdd = dtTableAdd.NewRow
                drTableAdd.Item("DESC") = dtRepairDel.Rows(0).Item("DESCRIPTION").ToString
                drTableAdd.Item("IPCS") = ""
                drTableAdd.Item("IGWT") = ""
                drTableAdd.Item("INWT") = ""
                drTableAdd.Item("RPCS") = ""
                drTableAdd.Item("RGWT") = ""
                drTableAdd.Item("RNWT") = ""
                drTableAdd.Item("REC") = ""
                drTableAdd.Item("PAY") = ""
                drTableAdd.Item("AVERAGE") = ""
                drTableAdd.Item("RESULT1") = ""
                drTableAdd.Item("COLHEAD") = "T"
                dtTableAdd.Rows.Add(drTableAdd)
                dtTableAdd.AcceptChanges()
            End If
        End If

            StrSql = vbCrLf + " SELECT "
            StrSql += vbCrLf + " 	(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = S.CATCODE)) STONENAME"
            StrSql += vbCrLf + " 	,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = I.METALID) + ' PCS/WT/AMT ' +"
            StrSql += vbCrLf + " 	CONVERT(VARCHAR,SUM(PCS)) + '/ ' + CONVERT(VARCHAR,SUM(NETWT)) + '/ ' + CONVERT(VARCHAR,SUM(AMOUNT)) MTDETAIL"
            StrSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I INNER JOIN " & cnStockDb & "..ISSSTONE AS S"
            StrSql += vbCrLf + " ON I.SNO = S.ISSSNO AND I.BATCHNO = S.BATCHNO"
            StrSql += vbCrLf + " WHERE S.COMPANYID IN (" & SelectedCompanyId & ") AND (SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.STNITEMID) IN ('D','P')"
            StrSql += vbCrLf + " AND I.TRANDATE BETWEEN '" & dtpFrom.ToString("yyyy-MM-dd") & "' AND  '" & dtpTo.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " " & Replace(strFilter, "SYSTEMID", "I.SYSTEMID") & " AND ISNULL(I.TRANTYPE,'') IN ('RD') AND ISNULL(CANCEL,'') = ''  AND I.COMPANYID IN (" & SelectedCompanyId & ")"
            StrSql += vbCrLf + " GROUP BY I.METALID,S.CATCODE"
            Dim dtIssStMetal As New DataTable
            da = New OleDbDataAdapter(StrSql, cn)
            da.Fill(dtIssStMetal)

            StrSql = vbCrLf + " SELECT '  ' + CATNAME CATNAME, "
            StrSql += vbCrLf + " CASE WHEN ISSPCS   <> 0 THEN ISSPCS   ELSE NULL END ISSPCS, "
            StrSql += vbCrLf + " CASE WHEN ISSGRSWT <> 0 THEN ISSGRSWT ELSE NULL END ISSGRSWT, "
            StrSql += vbCrLf + " CASE WHEN ISSNETWT <> 0 THEN ISSNETWT ELSE NULL END ISSNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECPCS   <> 0 THEN RECPCS   ELSE NULL END RECPCS, "
            StrSql += vbCrLf + " CASE WHEN RECGRSWT <> 0 THEN RECGRSWT ELSE NULL END RECGRSWT, "
            StrSql += vbCrLf + " CASE WHEN RECNETWT <> 0 THEN RECNETWT ELSE NULL END RECNETWT, "
            StrSql += vbCrLf + " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
            StrSql += vbCrLf + " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT, "
            StrSql += vbCrLf + " CASE WHEN AVERAGE <> '' THEN (AVERAGE + ' -- ' + RATE) ELSE '' END AVERAGE, COLHEAD, RESULT1,METALNAME "
            StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSREPAIR ORDER BY METALNAME,RESULT1"
            dtRepairDel = New DataTable
            da = New OleDbDataAdapter(StrSql, cn)
            dtRepairDel = New DataTable
            da.Fill(dtRepairDel)

            If dtRepairDel.Rows.Count > 0 Then
                For cnt As Integer = 0 To dtRepairDel.Rows.Count - 1
                    If DotMatrix = True Then
                        If dtRepairDel.Rows(cnt).Item("COLHEAD").ToString = "T" Then
                            oWrite.Write(pBoldStart)
                            WriteLine(LSet(dtRepairDel.Rows(cnt).Item("CATNAME").ToString, oCharPerLine))
                            oWrite.Write(pBoldEnd)
                        ElseIf dtRepairDel.Rows(cnt).Item("COLHEAD").ToString = "S" Then
                            oWrite.Write(pBoldStart)
                            dtRow = dtIssStMetal.Select("STONENAME = '" & dtRepairDel.Rows(cnt).Item("METALNAME").ToString() & "'")
                            If dtRow.Length > 0 Then
                                For Each row As DataRow In dtRow
                                    WriteLine(LSet(row.Item("MTDETAIL").ToString, oCharPerLine))
                                Next
                        End If
                        If CategoryWise = True Then
                            MetalAmtTotRec += Convert.ToDouble(IIf(dtRepairDel.Rows(cnt).Item("RECEIPT").ToString = "", "0", dtRepairDel.Rows(cnt).Item("RECEIPT").ToString))
                            MetalAmtTotPay += Convert.ToDouble(IIf(dtRepairDel.Rows(cnt).Item("PAYMENT").ToString = "", "0", dtRepairDel.Rows(cnt).Item("PAYMENT").ToString))
                        End If
                        WriteLine(LSet("", PreSpaceLine) + PrintLine_WithOutWrite(oCharPerLine - PreSpaceLine, "-"))
                        oWrite.Write(pBoldEnd)
                        oWrite.Write(pBoldStart)
                        WriteLine(LSet(dtRepairDel.Rows(cnt).Item("CATNAME").ToString, LenDesc) + RSet(dtRepairDel.Rows(cnt).Item("ISSPCS").ToString, LenIPcs) + RSet(dtRepairDel.Rows(cnt).Item("ISSGRSWT").ToString, LenIGwt) + RSet(dtRepairDel.Rows(cnt).Item("ISSNETWT").ToString, LenINwt) + RSet(dtRepairDel.Rows(cnt).Item("RECPCS").ToString, LenRPcs) + RSet(dtRepairDel.Rows(cnt).Item("RECGRSWT").ToString, LenRGwt) + RSet(dtRepairDel.Rows(cnt).Item("RECNETWT").ToString, LenRNwt) + RSet(dtRepairDel.Rows(cnt).Item("RECEIPT").ToString, LenRec) + RSet(dtRepairDel.Rows(cnt).Item("PAYMENT").ToString, LenPay) + RSet(dtRepairDel.Rows(cnt).Item("AVERAGE").ToString, LenAvg))
                        oWrite.Write(pBoldEnd)
                        Else
                            WriteLine(LSet(dtRepairDel.Rows(cnt).Item("CATNAME").ToString, LenDesc) + RSet(dtRepairDel.Rows(cnt).Item("ISSPCS").ToString, LenIPcs) + RSet(dtRepairDel.Rows(cnt).Item("ISSGRSWT").ToString, LenIGwt) + RSet(dtRepairDel.Rows(cnt).Item("ISSNETWT").ToString, LenINwt) + RSet(dtRepairDel.Rows(cnt).Item("RECPCS").ToString, LenRPcs) + RSet(dtRepairDel.Rows(cnt).Item("RECGRSWT").ToString, LenRGwt) + RSet(dtRepairDel.Rows(cnt).Item("RECNETWT").ToString, LenRNwt) + RSet(dtRepairDel.Rows(cnt).Item("RECEIPT").ToString, LenRec) + RSet(dtRepairDel.Rows(cnt).Item("PAYMENT").ToString, LenPay) + RSet(dtRepairDel.Rows(cnt).Item("AVERAGE").ToString, LenAvg))
                        End If
                    Else

                        If dtRepairDel.Rows(cnt).Item("COLHEAD").ToString = "S" Then
                            dtRow = dtIssStMetal.Select("STONENAME = '" & dtRepairDel.Rows(cnt).Item("METALNAME").ToString() & "'")
                            If dtRow.Length > 0 Then
                                For Each row As DataRow In dtRow
                                    drTableAdd = dtTableAdd.NewRow
                                    drTableAdd.Item("DESC") = row.Item("MTDETAIL").ToString
                                    drTableAdd.Item("IPCS") = ""
                                    drTableAdd.Item("IGWT") = ""
                                    drTableAdd.Item("INWT") = ""
                                    drTableAdd.Item("RPCS") = ""
                                    drTableAdd.Item("RGWT") = ""
                                    drTableAdd.Item("RNWT") = ""
                                    drTableAdd.Item("REC") = ""
                                    drTableAdd.Item("PAY") = ""
                                    drTableAdd.Item("AVERAGE") = ""
                                drTableAdd.Item("RESULT1") = ""
                                    drTableAdd.Item("COLHEAD") = "S"
                                    dtTableAdd.Rows.Add(drTableAdd)
                                    dtTableAdd.AcceptChanges()
                                Next
                            End If
                    End If

                        drTableAdd = dtTableAdd.NewRow
                        drTableAdd.Item("DESC") = dtRepairDel.Rows(cnt).Item("CATNAME").ToString
                        drTableAdd.Item("IPCS") = dtRepairDel.Rows(cnt).Item("ISSPCS").ToString
                        drTableAdd.Item("IGWT") = dtRepairDel.Rows(cnt).Item("ISSGRSWT").ToString
                        drTableAdd.Item("INWT") = dtRepairDel.Rows(cnt).Item("ISSNETWT").ToString
                        drTableAdd.Item("RPCS") = dtRepairDel.Rows(cnt).Item("RECPCS").ToString
                        drTableAdd.Item("RGWT") = dtRepairDel.Rows(cnt).Item("RECGRSWT").ToString
                        drTableAdd.Item("RNWT") = dtRepairDel.Rows(cnt).Item("RECNETWT").ToString
                        drTableAdd.Item("REC") = dtRepairDel.Rows(cnt).Item("RECEIPT").ToString
                        drTableAdd.Item("PAY") = dtRepairDel.Rows(cnt).Item("PAYMENT").ToString
                        drTableAdd.Item("AVERAGE") = dtRepairDel.Rows(cnt).Item("AVERAGE").ToString
                    drTableAdd.Item("RESULT1") = dtRepairDel.Rows(cnt).Item("RESULT1").ToString

                    If dtRepairDel.Rows(cnt).Item("COLHEAD").ToString = "T" Then
                        drTableAdd.Item("COLHEAD") = "S"
                    ElseIf dtRepairDel.Rows(cnt).Item("COLHEAD").ToString = "S" Then
                        drTableAdd.Item("COLHEAD") = "S"
                        If CategoryWise = True Then
                            MetalAmtTotRec += Convert.ToDouble(IIf(dtRepairDel.Rows(cnt).Item("RECEIPT").ToString = "", "0", dtRepairDel.Rows(cnt).Item("RECEIPT").ToString))
                            MetalAmtTotPay += Convert.ToDouble(IIf(dtRepairDel.Rows(cnt).Item("PAYMENT").ToString = "", "0", dtRepairDel.Rows(cnt).Item("PAYMENT").ToString))
                        End If
                    Else
                        drTableAdd.Item("COLHEAD") = ""
                    End If
                        dtTableAdd.Rows.Add(drTableAdd)
                        dtTableAdd.AcceptChanges()
                    End If
                Next cnt
        End If


        StrSql = vbCrLf + " SELECT 'REPAIR DET TOT' CATNAME, "
        StrSql += vbCrLf + " CASE WHEN SUM(ISSPCS) = 0 THEN NULL ELSE SUM(ISSPCS) END ISSPCS, "
        StrSql += vbCrLf + " CASE WHEN SUM(ISSGRSWT) = 0 THEN NULL ELSE SUM(ISSGRSWT) END ISSGRSWT, "
        StrSql += vbCrLf + " CASE WHEN SUM(ISSNETWT) = 0 THEN NULL ELSE SUM(ISSNETWT) END ISSNETWT, "
        StrSql += vbCrLf + " CASE WHEN SUM(RECPCS) = 0 THEN NULL ELSE SUM(RECPCS) END RECPCS, "
        StrSql += vbCrLf + " CASE WHEN SUM(RECGRSWT) = 0 THEN NULL ELSE SUM(RECGRSWT) END RECGRSWT, "
        StrSql += vbCrLf + " CASE WHEN SUM(RECNETWT) = 0 THEN NULL ELSE SUM(RECNETWT) END RECNETWT, "
        StrSql += vbCrLf + " CASE WHEN SUM(RECEIPT) = 0 THEN NULL ELSE SUM(RECEIPT) END RECEIPT, "
        StrSql += vbCrLf + " CASE WHEN SUM(PAYMENT) = 0 THEN NULL ELSE SUM(PAYMENT) END PAYMENT, "
        StrSql += vbCrLf + " MAX(AVERAGE) AVERAGE "
        StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSREPAIR"
        If CategoryWise = True Then
            StrSql += vbCrLf + " WHERE ISNULL(COLHEAD,'') ='S'"
        Else
            StrSql += vbCrLf + " WHERE ISNULL(COLHEAD,'') =''"
        End If
        dtRepairDel = New DataTable
        da = New OleDbDataAdapter(StrSql, cn)
        dtRepairDel = New DataTable
        da.Fill(dtRepairDel)

        If dtRepairDel.Rows.Count > 0 Then
            If DotMatrix = True Then
                WriteLine(LSet("", PreSpaceLine) + PrintLine_WithOutWrite(oCharPerLine - PreSpaceLine, "-"))
                oWrite.Write(pBoldDoubleStart)
                WriteLine(LSet(dtRepairDel.Rows(0).Item("CATNAME").ToString, LenDesc) + RSet(dtRepairDel.Rows(0).Item("ISSPCS").ToString, LenIPcs) + RSet(dtRepairDel.Rows(0).Item("ISSGRSWT").ToString, LenIGwt) + RSet(dtRepairDel.Rows(0).Item("ISSNETWT").ToString, LenINwt) + RSet(dtRepairDel.Rows(0).Item("RECPCS").ToString, LenRPcs) + RSet(dtRepairDel.Rows(0).Item("RECGRSWT").ToString, LenRGwt) + RSet(dtRepairDel.Rows(0).Item("RECNETWT").ToString, LenRNwt) + RSet(dtRepairDel.Rows(0).Item("RECEIPT").ToString, LenRec) + RSet(dtRepairDel.Rows(0).Item("PAYMENT").ToString, LenPay) + RSet(dtRepairDel.Rows(0).Item("AVERAGE").ToString, LenAvg))
                oWrite.Write(pBoldDoubleEnd)
                WriteLine(LSet("", PreSpaceLine) + PrintLine_WithOutWrite(oCharPerLine - PreSpaceLine, "-"))
            Else
                drTableAdd = dtTableAdd.NewRow
                drTableAdd.Item("DESC") = dtRepairDel.Rows(0).Item("CATNAME").ToString
                drTableAdd.Item("IPCS") = dtRepairDel.Rows(0).Item("ISSPCS").ToString
                drTableAdd.Item("IGWT") = dtRepairDel.Rows(0).Item("ISSGRSWT").ToString
                drTableAdd.Item("INWT") = dtRepairDel.Rows(0).Item("ISSNETWT").ToString
                drTableAdd.Item("RPCS") = dtRepairDel.Rows(0).Item("RECPCS").ToString
                drTableAdd.Item("RGWT") = dtRepairDel.Rows(0).Item("RECGRSWT").ToString
                drTableAdd.Item("RNWT") = dtRepairDel.Rows(0).Item("RECNETWT").ToString
                drTableAdd.Item("REC") = dtRepairDel.Rows(0).Item("RECEIPT").ToString
                drTableAdd.Item("PAY") = dtRepairDel.Rows(0).Item("PAYMENT").ToString
                drTableAdd.Item("AVERAGE") = dtRepairDel.Rows(0).Item("AVERAGE").ToString
                drTableAdd.Item("RESULT1") = ""
                drTableAdd.Item("COLHEAD") = "G"
                dtTableAdd.Rows.Add(drTableAdd)
                dtTableAdd.AcceptChanges()

                If CategoryWise = False Then
                    MetalAmtTotRec += Convert.ToDouble(IIf(dtRepairDel.Rows(0).Item("RECEIPT").ToString = "", "0", dtRepairDel.Rows(0).Item("RECEIPT").ToString))
                    MetalAmtTotPay += Convert.ToDouble(IIf(dtRepairDel.Rows(0).Item("PAYMENT").ToString = "", "0", dtRepairDel.Rows(0).Item("PAYMENT").ToString))
                End If
            End If
        End If
        'StrSql = vbCrLf + " SELECT 'Z' AS CATCODE, 'SALES REPAIR TOT'CATNAME,SUM(ISSPCS),SUM(ISSGRSWT),SUM(ISSNETWT), "
        'StrSql += vbCrLf + " SUM(RECPCS),SUM(RECGRSWT),SUM(RECNETWT), SUM(RECEIPT), "
        'StrSql += vbCrLf + " SUM(PAYMENT),3 RESULT,4 RESULT1, 'S' COLHEAD "
        'StrSql += vbCrLf + " FROM TEMP" & systemId & "ABSREPAIR WHERE RESULT = 1 "
        'dtRepairDel = New DataTable
        'da = New OleDbDataAdapter(StrSql, cn)
        'dtRepairDel = New DataTable
        'da.Fill(dtRepairDel)

    End Sub


    Private Sub ProcSASRPU()
        'ProcSales()
        'ProcRepairDelivery()
        'ProcSalesReturn()
        'ProcPurchase()
        'ProcMiscIssue()
        'ProcCreditSales()
        'ProcCreditPurchase()
        'ProcCreditAdjustment()
        'ProcCreditPurchasePayment()
        'ProcAdvanceReceived()
        'ProcAdvanceAdjustment()
        'ProcOrderAdvance()
        'ProcOrderAdvanceAdjusted()
        'ProcRepairAdvance()
        'ProcRepairAdvanceAdjusted()
        'ProcChitPayment()
        'ProcMiscReceipt()
        'ProcMiscPayment()
        ' ''EMPTY LINE
        'StrSql = " INSERT INTO TEMP" & systemId & "SASRPU(DESCRIPTION) VALUES('')"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()
        'ProcBeeds()
        'ProcDiscount()
        'ProcHandling()
        'ProcroundOff()
        'ProcApprovalIssue()
        'ProcApprovalReceipt()
        'ProcHomeSales()
        'ProcPartlySales()
        'ProcChitCollection()
        'ProcWtSubtot()
        'ProcCollection()
        'ProcAmtSubtot()
        'ProcAdvDueSummary()
        'If chkCancelBills.Checked Then ProcCancelBills()

        'StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SASRPU)>0"
        'StrSql += " BEGIN "
        'StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT"
        'StrSql += " (COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9,COL10,COLHEAD) "
        'StrSql += " SELECT DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT,AVERAGE,COLHEAD "
        'StrSql += " FROM TEMP" & systemId & "SASRPU WHERE ISNULL(RESULT1,1) NOT IN (0,2) ORDER BY SNO"
        'StrSql += " END "
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()

        'StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ADVDUESUMMARY)>0 "
        'StrSql += vbCrLf + " BEGIN "
        'StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT(COL1) VALUES('')"
        'StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT(COL1,COLHEAD) VALUES('ADV-DUE SUMMARY','T')"
        'StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT"
        'StrSql += " (COL1,COL6,COL7,COL8,COL9,COLHEAD) "
        'StrSql += " SELECT ' ','OPENING','RECEIPT','PAYMENT','CLOSING','S'"
        'StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT"
        'StrSql += " (COL1,COL6,COL7,COL8,COL9) "
        'StrSql += vbCrLf + " SELECT ' ' + TRANTYPE, "
        'StrSql += vbCrLf + " CASE WHEN OPENING   <> 0 THEN OPENING   ELSE NULL END OPENING, "
        'StrSql += vbCrLf + " CASE WHEN RECEIPT <> 0 THEN RECEIPT ELSE NULL END RECEIPT, "
        'StrSql += vbCrLf + " CASE WHEN PAYMENT <> 0 THEN PAYMENT ELSE NULL END PAYMENT, "
        'StrSql += vbCrLf + " CASE WHEN CLOSING   <> 0 THEN CLOSING  ELSE NULL END CLOSING "
        'StrSql += vbCrLf + " FROM TEMP" & systemId & "ADVDUESUMMARY ORDER BY TRANTYPE"
        'StrSql += vbCrLf + " END "
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'Cmd.ExecuteNonQuery()


        'If chkCancelBills.Checked Then
        '    StrSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "ABSCANCELBILL )>0 "
        '    StrSql += " BEGIN "
        '    StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT(COL1) VALUES('')"
        '    StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT(COL1) VALUES('')"
        '    StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT(COL1,COLHEAD) VALUES('CANCEL BILLS','T')"
        '    StrSql += " INSERT INTO TEMP" & systemId & "SALEABSTRACT"
        '    StrSql += " (COL1,COL2,COL3,COL4,COL5,COL6,COL7,COL8,COL9) "
        '    StrSql += " SELECT '  ' + TRANS, "
        '    StrSql += " CASE WHEN IPCS   <> 0 THEN IPCS   ELSE NULL END IPCS, "
        '    StrSql += " CASE WHEN IGRSWT <> 0 THEN IGRSWT ELSE NULL END IGRSWT, "
        '    StrSql += " CASE WHEN INETWT <> 0 THEN INETWT ELSE NULL END INETWT, "
        '    StrSql += " CASE WHEN RPCS   <> 0 THEN RPCS   ELSE NULL END RPCS, "
        '    StrSql += " CASE WHEN RGRSWT <> 0 THEN RGRSWT ELSE NULL END RGRSWT, "
        '    StrSql += " CASE WHEN RNETWT <> 0 THEN RNETWT ELSE NULL END RNETWT, "
        '    StrSql += " CASE WHEN RECEIPT  <> 0 THEN RECEIPT  ELSE NULL END RECEIPT, "
        '    StrSql += " CASE WHEN PAYMENT  <> 0 THEN PAYMENT  ELSE NULL END PAYMENT FROM "
        '    StrSql += " TEMP" & systemId & "ABSCANCELBILL ORDER BY TRANTYPE, TRANNO"
        '    StrSql += " END "
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    Cmd.ExecuteNonQuery()
        'End If

        'Dim dt As New DataTable("SASRPU")
        'With dt
        '    .Columns.Add("DESCRIPTION", GetType(String))
        '    .Columns.Add("ISSPCS", GetType(Integer))
        '    .Columns.Add("ISSGRSWT", GetType(Decimal))
        '    .Columns.Add("ISSNETWT", GetType(Decimal))
        '    .Columns.Add("RECPCS", GetType(Integer))
        '    .Columns.Add("RECGRSWT", GetType(Decimal))
        '    .Columns.Add("RECNETWT", GetType(Decimal))
        '    .Columns.Add("RECEIPT", GetType(Decimal))
        '    .Columns.Add("PAYMENT", GetType(Decimal))
        '    .Columns.Add("AVERAGE", GetType(String))
        '    .Columns.Add("COLHEAD", GetType(String))

        '    .Columns("ISSPCS").Caption = "PCS"
        '    .Columns("ISSGRSWT").Caption = "GRSWT"
        '    .Columns("ISSNETWT").Caption = "NETWT"
        '    .Columns("RECPCS").Caption = "PCS"
        '    .Columns("RECGRSWT").Caption = "GRSWT"
        '    .Columns("RECNETWT").Caption = "NETWT"
        '    .Columns("RECEIPT").DataType = GetType(Decimal)
        '    .Columns("PAYMENT").DataType = GetType(Decimal)
        '    .Columns("AVERAGE").Caption = "AVERAGE"
        '    StrSql = "IF (SELECT COUNT(*) FROM TEMP" & systemId & "SASRPU)>0 "
        '    StrSql += " BEGIN "
        '    .Columns("AVERAGE").MaxLength = objGPack.GetSqlValue(StrSql & "SELECT MAX(LEN(ISNULL(AVERAGE,0))) AS AA FROM TEMP" & systemId & "SASRPU END", , "-1")
        '    .Columns("COLHEAD").MaxLength = objGPack.GetSqlValue(StrSql & "SELECT MAX(LEN(ISNULL(COLHEAD,0))) AS AA FROM TEMP" & systemId & "SASRPU END", , "-1")
        'End With

        'StrSql = "SELECT DESCRIPTION,ISSPCS,ISSGRSWT,ISSNETWT,RECPCS,RECGRSWT,RECNETWT,RECEIPT,PAYMENT, "
        'StrSql += " AVERAGE, COLHEAD AS COLHEAD FROM TEMP" & systemId & "SASRPU ORDER BY SNO, RESULT1 "
        'da = New OleDbDataAdapter(StrSql, cn)
        'da.Fill(dt)
        'dt.Columns.Remove("AVERAGE")
        'dt.AcceptChanges()
        'If dt.Rows.Count > 0 Then dsReportCol.Tables.Add(dt)
    End Sub

End Class
