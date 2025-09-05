Imports System.Data.OleDb
Imports System.IO

Public Class frmBillPrintDoc_RSR
#Region "Variable"
    Dim strsql As String
    Dim strsqlCheck As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand

    Dim pBatchno As String
    Dim NodeId As String 'SystemId
    Dim Trantype As String
    Dim TrantypeMI As String = ""
    Dim Tranno As String = "" ' BillNO
    Dim BillPrefix As String = ""
    Dim strBilldate As String = "" ' BillDate
    Dim prtBilldate As String = "" ' Print BillDate
    Dim DueDate As String = "" 'DueDate Order & Receipt
    Dim NoofPrint As Integer = 0

    Public SystemName As String = Environment.MachineName
    Dim LeftFormat As New StringFormat(StringAlignment.Near)
    Dim RightFormat As New StringFormat(StringAlignment.Far)
    Dim CentreFormat As New StringFormat(StringAlignment.Center)

    Public SilverBrush As SolidBrush = New SolidBrush(Color.Silver)
    Public BlackBrush As SolidBrush = New SolidBrush(Color.Black)

    Dim fontRegular As New Font("Palatino Linotype", 9, FontStyle.Regular)
    Dim fontRegular6 As New Font("Palatino Linotype", 6, FontStyle.Regular)
    Dim fontRegular7 As New Font("Palatino Linotype", 7, FontStyle.Regular)
    Dim fontRegularsmall As New Font("Palatino Linotype", 8, FontStyle.Regular)
    Dim fontBold As New Font("Palatino Linotype", 9, FontStyle.Bold)
    Dim fontBoldsmall As New Font("Palatino Linotype", 7, FontStyle.Bold)
    Dim fontUnderLine As New Font("Palatino Linotype", 9, FontStyle.Underline)
    Dim fontBoldUnderLine As New Font("Palatino Linotype", 9, FontStyle.Bold And FontStyle.Underline)
    Dim fontBoldTitle As New Font("Palatino Linotype", 11, FontStyle.Bold)
    Dim fontAmtInWord As New Font("Palatino Linotype", 8, FontStyle.Regular)

    Dim EstNo_SA As String = ""
    Dim EstNo_PU As String = ""

    Dim c1 As Integer = 25  ' SNo
    Dim c2 As Integer = 70  ' Description
    Dim h1 As Integer = 250  ' Description
    Dim c3 As Integer = 350 ' Qty 
    Dim c4 As Integer = 430 ' Grs. Wt 
    Dim c5 As Integer = 510 ' Less. Wt
    Dim c6 As Integer = 560 ' VA
    Dim c7 As Integer = 630 ' Rate
    Dim c8 As Integer = 690 ' MC
    Dim c9 As Integer = 770 ' ' Amount
    Dim c10 As Integer = 800
    Dim PagecountSale As Integer = 0

    Dim TopY As Integer = 135 ' TOP STARTING POSITION
    Dim BottomY As Integer = 705 ' BOTTOM STATRING POSITION

    Dim dtCustInfo As DataTable ' CustomerInfo
    Dim dtSales As New DataTable ' SALEs, PURCHASEs, SALEsReturn
    Dim dtOrderRepair As New DataTable ' Repair & Order
    Dim dsAdvanceReceipt As New DataSet ' Advance & Receipt
    Dim dsSmithIssueReceipt As New DataSet ' Smith Issue & Receipt
    'Dim EXCISENO As String = ""

    Dim GSTNO As String = ""
    Dim COMPANYGSTNO As String = ""

    Dim Rules(1) As String
    Dim ImgPrint As MemoryStream = Nothing
    Dim UserImgbyte As Byte() = Nothing
    Dim placeofSupply As String = ""
    Dim dtInvTran As New DataTable
    Dim _MIMR As String = ""
    Dim _duplicate As String = ""
    Dim boolPrintSmithA4Issue As Boolean = True
    Dim boolPrintSmithA4IssueTax As Boolean = True
    Dim dtPwdMasterValue As New DataTable
    Dim B2C_QRCODE As Boolean = IIf(GetAdmindbSoftValue("B2C_QRCODE", "N") = "Y", True, False)
    Dim QrcodeCustomer As Image = Nothing
    Dim PrintRPU_IPU As Boolean = False
    Dim strCompanyGst As String = ""

#End Region
    Public Sub pwdMaster(ByVal PBatchno As String)
        strsql = ""
        strsql += vbCrLf + " SELECT RIGHT(ISNULL(PWDMOBILENO,''),4)PWDMOBILENO"
        strsql += vbCrLf + " ,ISNULL([PASSWORD],'')PWD"
        strsql += vbCrLf + " ,ISNULL(PWDSTATUS,'')[PWDSTATUS]"
        strsql += vbCrLf + " FROM " & cnAdminDb & "..PWDMASTER "
        strsql += vbCrLf + " WHERE ISNULL(BATCHNO,'') <> '' "
        strsql += vbCrLf + " AND BATCHNO='" & PBatchno & "' "
        strsql += vbCrLf + " AND ISNULL(PWDSTATUS,'') = 'C'"
        strsql += vbCrLf + " AND ISNULL(PWDMOBILENO,'') <> ''"
        da = New OleDbDataAdapter(strsql, cn)
        dtPwdMasterValue = New DataTable
        da.Fill(dtPwdMasterValue)
    End Sub
    Public Sub New(ByVal type As String, ByVal batchno As String, ByVal trandate As String, ByVal duplicate As String, Optional ByVal Mimr As String = "OLD")
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        pBatchno = batchno
        Trantype = type ' CHECKING HEADLING ONLY

        _MIMR = Mimr
        _duplicate = duplicate

        Dim dr As DataRow = Nothing
        strsql = "SELECT ISNULL(GSTNO,'') EXCISENO,NULL ASIGN,isnull(GSTNO,'') GSTNO  FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID = '" & strCompanyId & "'"
        dr = GetSqlRow(strsql, cn)
        If Not dr Is Nothing Then
            GSTNO = dr.Item("EXCISENO").ToString
            COMPANYGSTNO = dr.Item("EXCISENO").ToString
            ''UserImgbyte = dr.Item("ASIGN")
            ''ImgPrint = New MemoryStream(UserImgbyte)
            strCompanyGst = dr.Item("GSTNO")
        End If
        ' GSTNO = GetSqlValue(cn, strsql)
        Dim chkEInvTran As Double = 0
        strsql = " SELECT COUNT(*) CNT FROM " & cnStockDb & ".SYS.TABLES WHERE NAME ='EINVTRAN' "
        chkEInvTran = Val(GetSqlValue(cn, strsql).ToString)
        If chkEInvTran > 0 Then
            strsql = " SELECT * FROM " & cnStockDb & "..EINVTRAN WHERE BATCHNO = '" & pBatchno & "'"
            dtInvTran = GetSqlTable(strsql, cn)
        Else
            dtInvTran = New DataTable
        End If

        If B2C_QRCODE = True Then
            strsql = " SELECT (SELECT TOP 1 PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO IN "
            strsql += vbCrLf + " (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO)) AS PNAME, "
            strsql += vbCrLf + " TRANTYPE+'-'+ CONVERT(VARCHAR(10),TRANNO) TRANNO,TRANDATE,SUM(ISNULL(AMOUNT,0) + ISNULL(TAX,0))AMOUNT,SUM(PCS) PCS FROM " & cnStockDb & "..ISSUE I WHERE "
            strsql += vbCrLf + " BATCHNO='" & batchno & "' GROUP BY TRANNO,TRANDATE,BATCHNO,TRANTYPE "
            strsql += vbCrLf + " UNION ALL "
            strsql += vbCrLf + " SELECT (SELECT TOP 1 PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO IN "
            strsql += vbCrLf + " (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO)) AS PNAME, "
            strsql += vbCrLf + " TRANTYPE+'-'+ CONVERT(VARCHAR(10),TRANNO) TRANNO,TRANDATE,SUM(ISNULL(AMOUNT,0) + ISNULL(TAX,0))AMOUNT,SUM(PCS) PCS FROM " & cnStockDb & "..RECEIPT I WHERE "
            strsql += vbCrLf + " BATCHNO='" & batchno & "' "
            strsql += vbCrLf + " AND BATCHNO NOT IN (SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..ISSUE WHERE  "
            strsql += vbCrLf + " BATCHNO='" & batchno & "')"
            strsql += vbCrLf + " GROUP BY TRANNO,TRANDATE,BATCHNO,TRANTYPE "
            strsql += vbCrLf + " UNION ALL "
            strsql += vbCrLf + " SELECT (SELECT TOP 1 PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO IN "
            strsql += vbCrLf + " (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO)) AS PNAME, "
            strsql += vbCrLf + " TRANTYPE+RECPAY+'-'+ CONVERT(VARCHAR(10),TRANNO) TRANNO,TRANDATE,SUM(ISNULL(AMOUNT,0) + ISNULL(GSTVAL,0))AMOUNT, 0 PCS FROM " & cnAdminDb & "..OUTSTANDING I WHERE "
            strsql += vbCrLf + " BATCHNO='" & batchno & "' "
            strsql += vbCrLf + " AND BATCHNO NOT IN (SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..ISSUE WHERE  "
            strsql += vbCrLf + " BATCHNO='" & batchno & "')"
            strsql += vbCrLf + " AND BATCHNO NOT IN (SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..RECEIPT WHERE  "
            strsql += vbCrLf + " BATCHNO='" & batchno & "')"
            strsql += vbCrLf + " GROUP BY TRANNO,TRANDATE,BATCHNO,TRANTYPE,RECPAY "
            Dim DT As DataRow = Nothing
            DT = GetSqlRow(strsql, cn, Nothing)
            If Not DT Is Nothing Then
                Dim strqrdata As String = strCompanyName.ToString & "-" & strCompanyGst.ToString & "-" & DT!TRANNO.ToString & "-" & DT!TRANDATE.ToString & "-" & DT!PNAME.ToString & "-" & DT!AMOUNT.ToString & "-" & DT!PCS.ToString
                Dim objp As New BrighttechPack.Methods
                'objp.qrcode_image(strqrdata, batchno)
                QrcodeCustomer = objp.qrcode_image2(strqrdata, batchno)
            End If
        End If

        strBilldate = trandate '
        Rules(0) = " No E-way bill is required to be generated as the Goods covered under this Invoice are exempted as per "
        Rules(1) = " Serial No. 150/151 to the Annexure to Rule 138(14) of the CGST Rules 2017 "
        If type = "POS" Then
            strsql = "SELECT COUNT(*)CNT FROM " & cnAdminDb & "..OUTSTANDING O WHERE BATCHNO='" & pBatchno & "'"
            strsql += vbCrLf + " AND PAYMODE IN('AR','AP','MR','DR','MP')"
            strsql += vbCrLf + " AND NOT EXISTS(SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE BATCHNO=O.BATCHNO"
            ' strSql += vbCrLf + " UNION SELECT 1 FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO=O.BATCHNO AND TRANTYPE='SR' )"
            strsql += vbCrLf + " )"
            If Val(GetSqlValue_Bill(strsql, "CNT", 0)) > 0 Then  ''OG WITH ADVANCE
                printCustoInfo = True
                AdvancePrint(pBatchno, strBilldate)
                PrtDiaAdvance.Document = PrintAdvance
                PrintAdvance.PrinterSettings.Copies = 1
                PrintAdvance.PrintController = New System.Drawing.Printing.StandardPrintController()
                PrintAdvance.Print()
            Else
                Print(pBatchno)
            End If
            'strsql = "SELECT "
            'strsql += vbCrLf + "SUM(CASE WHEN TRANMODE='C' THEN AMOUNT ELSE -1*AMOUNT END)AMOUNT"
            'strsql += vbCrLf + "FROM " & cnStockDb & "..ACCTRAN AS A WHERE BATCHNO='" & pBatchno & "' AND ACCODE='CASH' "
        ElseIf type = "ORD" Or type = "REP" Then
            OrderReceipt(pBatchno, type, strBilldate)
            PrtDiaOrder.Document = PrintOrder
            PrintOrder.PrinterSettings.Copies = 1
            PrintOrder.PrintController = New System.Drawing.Printing.StandardPrintController()
            PrintOrder.Print()
        ElseIf type = "EST" Then ' EST- > PURCHASE & SALES
            If pBatchno.Contains(":") Then
                Dim strEstNo() As String = pBatchno.Split(":")
                EstNo_SA = strEstNo(0).Replace("S.", "")
                EstNo_PU = strEstNo(1).Replace("P.", "")
            End If
            If Val(EstNo_SA.ToString) <> 0 Then
                salesEstimation()
                'funcEstSalesPrint()
                'PrintDialog2.Document = PrintDocument1
                'PrintDocument1.PrinterSettings.Copies = 1
                'PrintDocument1.PrintController = New System.Drawing.Printing.StandardPrintController
                'PrintDocument1.Print()
            Else
                'PrintDialog1.Document = PrintDocument1
                PrintDialog2.Document = PrintDocument1
                PrintDocument1.PrinterSettings.Copies = 1
                PrintDocument1.PrintController = New System.Drawing.Printing.StandardPrintController
                PrintDocument1.Print()
            End If
        ElseIf type = "SMI" Or type = "SMR" Then
            smithIssueReceipt(pBatchno, "", Mimr)
            ''pwdMaster(pBatchno)
            If Mimr = "NEW" Then
                Dim PrintRPU As Boolean = False
                PrintRPU_IPU = False
                'If dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("TTYPE").ToString = "RPU" Then
                '    PrintRPU = True
                'End If
                If dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count > 0 Then
                    Dim TRPU As Boolean = False
                    Dim TIPU As Boolean = False
                    For i As Integer = 0 To dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count - 1
                        If dsSmithIssueReceipt.Tables("DTISSREC").Rows(i).Item("TTYPE").ToString = "RPU" Then
                            TRPU = True
                        ElseIf dsSmithIssueReceipt.Tables("DTISSREC").Rows(i).Item("TTYPE").ToString = "IPU" Then
                            TIPU = True
                        End If
                    Next
                    If TRPU = True And TIPU = True Then
                        PrintRPU_IPU = True
                    ElseIf TRPU = True Then
                        PrintRPU = True
                    End If
                End If
                'New Method
                If True Then
                    If PrintRPU = True Then
                    ElseIf PrintRPU_IPU = True Then
                        PagecountSale = 0
                        TopY = TOPYSMITHISSUEVALUE
                        boolPrintSmithA4Issue = True
                        boolPrintSmithA4IssueTax = True
                        PrtDiaA4First.Document = PrtSmithA4First
                        PrtSmithA4First.PrinterSettings.Copies = 1
                        PrtSmithA4First.PrintController = New System.Drawing.Printing.StandardPrintController
                        PrtSmithA4First.Print()
                    Else
                        PagecountSale = 0
                        TopY = TOPYSMITHISSUEVALUE
                        boolPrintSmithA4Issue = True
                        boolPrintSmithA4IssueTax = True
                        PrtDiaA4First.Document = PrtSmithA4First
                        PrtSmithA4First.PrinterSettings.Copies = 1
                        PrtSmithA4First.PrintController = New System.Drawing.Printing.StandardPrintController
                        PrtSmithA4First.Print()
                    End If

                    If PrintRPU = True Then
                    ElseIf PrintRPU_IPU = True Then
                    Else
                        PrintRPU_IPU = False
                        PagecountSale = 0
                        TopY = TOPYSMITHISSUEVALUE
                        boolPrintSmithA4Issue = True
                        boolPrintSmithA4IssueTax = True
                        PrtDiaA4Second.Document = PrtSmithA4Second
                        PrtSmithA4Second.PrinterSettings.Copies = 1
                        PrtSmithA4Second.PrintController = New System.Drawing.Printing.StandardPrintController
                        PrtSmithA4Second.Print()
                    End If

                    PrintRPU_IPU = False
                    PagecountSale = 0
                    TopY = TOPYSMITHISSUEVALUE
                    boolPrintSmithA4Issue = True
                    boolPrintSmithA4IssueTax = True
                    PrtDiaA4Third.Document = PrtSmithA4Third
                    PrtSmithA4Third.PrinterSettings.Copies = 1
                    PrtSmithA4Third.PrintController = New System.Drawing.Printing.StandardPrintController
                    PrtSmithA4Third.Print()

                    PrintRPU_IPU = False
                    PagecountSale = 0
                    TopY = TOPYSMITHISSUEVALUE
                    boolPrintSmithA4Issue = True
                    boolPrintSmithA4IssueTax = True
                    PrtDiaA4Fourth.Document = PrtSmithA4Fourth
                    PrtSmithA4Fourth.PrinterSettings.Copies = 1
                    PrtSmithA4Fourth.PrintController = New System.Drawing.Printing.StandardPrintController
                    PrtSmithA4Fourth.Print()

                Else
                    PrtDisSmith2.Document = PrtSmith2
                    PrtSmith2.PrinterSettings.Copies = 1
                    PrtSmith2.PrintController = New System.Drawing.Printing.StandardPrintController
                    PrtSmith2.Print()
                End If
            Else
                PrtDiaSmith.Document = PrtSmith
                PrtSmith.PrinterSettings.Copies = 1
                PrtSmith.PrintController = New System.Drawing.Printing.StandardPrintController
                PrtSmith.Print()
            End If
        End If
    End Sub

    Private Sub smithIssueReceiptStone(ByVal BatchNo As String, ByVal Trantype As String, ByVal mimrformat As String _
                                       , ByVal result As Double _
                                       , ByVal tableName As String, ByVal otableName As String, ByVal _Type As String)
        'RECEIPT STONE
        strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE] "
        strsql += vbCrLf + " ( "
        strsql += vbCrLf + " SNO,ISSUENO,DATE,CATNAME,HSN,USERNAME,ALLOYNAME,ALLOY,WASTAGENAME"
        strsql += vbCrLf + " ,WASTAGE,PCS,GRSWT,NETWT,RATE,DIAWT,MC,TAX"
        strsql += vbCrLf + " ,AMOUNT,ACCODE,REMARK1,REMARK2,TRANTYPE,RESULT,TTYPE,TOUCH,PURITY,PUREWT,LESSWT,DIAAMT"
        strsql += vbCrLf + " ,ITEMNAME,TYPE,SERNO,TABLENAME,TAXID,TAXPER,CATCODE,OTABLENAME"
        strsql += vbCrLf + " )"
        strsql += vbCrLf + " SELECT NULL SNO,"
        strsql += vbCrLf + " NULL ISSUENO , "
        strsql += vbCrLf + " NULL AS DATE,"
        strsql += vbCrLf + " NULL AS CATNAME,"
        strsql += vbCrLf + " NULL HSN, "
        strsql += vbCrLf + " NULL USERNAME,"
        strsql += vbCrLf + " NULL  AS ALLOYNAME,"
        strsql += vbCrLf + " 0  AS ALLOY,"
        strsql += vbCrLf + " NULL AS WASTAGENAME,"
        strsql += vbCrLf + " NULL WASTAGE, "
        strsql += vbCrLf + " NULL PCS,"
        strsql += vbCrLf + " NULL GRSWT,"
        strsql += vbCrLf + " NULL NETWT,"
        strsql += vbCrLf + " NULL RATE,"
        strsql += vbCrLf + " NULL AS DIAWT,"
        strsql += vbCrLf + " I.STNAMT AS MC,"
        strsql += vbCrLf + " NULL TAX,"
        strsql += vbCrLf + " NULL AMOUNT,"
        strsql += vbCrLf + " NULL ACCODE,NULL REMARK1,NULL REMARK2, '' TRANTYPE "
        strsql += vbCrLf + " ,'" & result & "' AS RESULT" '2.0
        strsql += vbCrLf + " , NULL TTYPE "
        strsql += vbCrLf + " , 0 TOUCH"
        strsql += vbCrLf + " , NULL PURITY"
        strsql += vbCrLf + " , NULL PUREWT"
        strsql += vbCrLf + " , NULL LESSWT"
        strsql += vbCrLf + " , NULL DIAAMT"
        strsql += vbCrLf + " ,'   '+(CASE WHEN ISNULL(SM.SUBITEMNAME,'')='' THEN IM.ITEMNAME ELSE SM.SUBITEMNAME END) + ' - ' + CONVERT(VARCHAR,I.STNPCS)"
        strsql += vbCrLf + "  + ' - ' + CONVERT(VARCHAR,I.STNWT)"
        strsql += vbCrLf + "  + ' @ ' + CONVERT(VARCHAR,I.STNRATE)"
        strsql += vbCrLf + "  ITEMNAME"
        strsql += vbCrLf + " ," & _Type & " TYPE" ' 1 ->REplace
        strsql += vbCrLf + " ,I.ISSSNO [SERNO]"
        strsql += vbCrLf + " ,'" & tableName & "' TABLENAME" 'RECEIPT
        strsql += vbCrLf + " ,NULL TAXID"
        strsql += vbCrLf + " ,NULL TAXPER"
        strsql += vbCrLf + " ,NULL CATCODE"
        strsql += vbCrLf + " ,'" & otableName & "' OTABLENAME" 'RECEIPT
        strsql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE I "
        strsql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID=I.STNITEMID"
        strsql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID=I.STNITEMID AND SM.SUBITEMID=I.STNSUBITEMID"
        strsql += vbCrLf + " WHERE I.TRANDATE = '" & strBilldate & "' "
        strsql += vbCrLf + " AND I.BATCHNO = '" & BatchNo & "' "
        If Trantype <> "" Then
            strsql += vbCrLf + " AND I.TRANTYPE IN (" & Trantype & ")"
        End If
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
    End Sub

    Public Sub smithIssueReceipt(ByVal BatchNo As String, ByVal Trantype As String, ByVal mimrformat As String)
        strsql = " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE]','U') "
        strsql += vbCrLf + " IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE]"
        strsql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE_ACCODE]','U') "
        strsql += vbCrLf + " IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE_ACCODE]"
        strsql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECETOTAL]','U') "
        strsql += vbCrLf + " IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECETOTAL]"
        strsql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE_TAXTRAN]','U') "
        strsql += vbCrLf + " IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE_TAXTRAN]"
        strsql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE_TAXTRAN2]','U') "
        strsql += vbCrLf + " IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE_TAXTRAN2]"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        Dim AccRoundReceipt As Double = 0
        Dim AccRoundIssue As Double = 0

        strsql = ""
        strsql += vbCrLf + " SELECT COUNT(*) CNT FROM ("
        strsql += vbCrLf + " SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..RECEIPT "
        strsql += vbCrLf + " WHERE TRANDATE = '" & strBilldate & "' AND BATCHNO='" & BatchNo & "' "
        strsql += vbCrLf + " )X"
        AccRoundReceipt = Val(GetSqlValue(cn, strsql).ToString)

        strsql = ""
        strsql += vbCrLf + " SELECT COUNT(*) CNT FROM ("
        strsql += vbCrLf + " SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..ISSUE "
        strsql += vbCrLf + " WHERE TRANDATE = '" & strBilldate & "' AND BATCHNO='" & BatchNo & "' "
        strsql += vbCrLf + " )X"
        AccRoundIssue = Val(GetSqlValue(cn, strsql).ToString)


        strsql = ""

        'RECEIPT
        strsql = vbCrLf + " SELECT ROW_NUMBER() OVER(ORDER BY TRANNO DESC) AS SNO"
        strsql += vbCrLf + " ,'RJ'+FORMAT(TRANNO, '0000') AS ISSUENO "
        strsql += vbCrLf + " ,CONVERT(VARCHAR(15),TRANDATE,103) AS DATE,"
        strsql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) AS CATNAME,"
        strsql += vbCrLf + " CASE WHEN ISNULL((SELECT HSN FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID), '') <> '' THEN "
        strsql += vbCrLf + " ISNULL((SELECT HSN FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID), '') "
        strsql += vbCrLf + " ELSE (SELECT HSN FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)  END HSN, "
        strsql += vbCrLf + " (SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = I.USERID) USERNAME,"
        strsql += vbCrLf + " CASE WHEN ALLOY > 0 THEN 'ADD : ALLOY' END  AS ALLOYNAME,"
        strsql += vbCrLf + " ISNULL(CASE WHEN ALLOY > 0 THEN  (CONVERT(NUMERIC(15,3), ALLOY)) ELSE 0 END,0) AS ALLOY,"
        strsql += vbCrLf + " CASE WHEN  WASTAGE > 0 then 'ADD : YIELD PROCESSING LOSS' END AS WASTAGENAME,"
        strsql += vbCrLf + " CASE WHEN  WASTAGE > 0 then WASTAGE ELSE 0 END AS WASTAGE, "
        strsql += vbCrLf + " CASE WHEN PCS > 0 THEN PCS END AS PCS,"
        strsql += vbCrLf + " CASE WHEN GRSWT > 0 THEN (CONVERT(NUMERIC(15,3), GRSWT)) END AS GRSWT,"
        strsql += vbCrLf + " CASE WHEN NETWT > 0 THEN  (CONVERT(NUMERIC(15,3), NETWT))  END AS NETWT,"
        strsql += vbCrLf + " CASE WHEN RATE > 0 THEN (CONVERT(NUMERIC(15,2), RATE)) END AS RATE,"
        strsql += vbCrLf + " (SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT,"
        strsql += vbCrLf + " CASE WHEN MCHARGE > 0 THEN MCHARGE ELSE 0 END AS MC,"
        strsql += vbCrLf + " CASE WHEN TAX > 0 THEN TAX ELSE 0 END AS TAX,"
        strsql += vbCrLf + " AMOUNT,"
        strsql += vbCrLf + " ACCODE,REMARK1,REMARK2,"
        strsql += vbCrLf + " CASE WHEN TRANTYPE = 'RRE' THEN 'RECEIPT FROM JOBWORK' ELSE 'PURIFICATION RECEIPT VOUCHER' END TRANTYPE"
        strsql += vbCrLf + " , 1.0 AS RESULT"
        strsql += vbCrLf + " , TRANTYPE AS TTYPE "
        strsql += vbCrLf + " , (CONVERT(NUMERIC(15,2), TOUCH)) AS TOUCH "
        strsql += vbCrLf + " , PURITY"
        strsql += vbCrLf + " , PUREWT"
        strsql += vbCrLf + " , LESSWT"
        strsql += vbCrLf + " ,(SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAAMT"
        strsql += vbCrLf + " , CASE WHEN ISNULL((SELECT SUBITEMNAME FROM  " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID),'') ='' THEN "
        strsql += vbCrLf + " ISNULL((SELECT ITEMNAME FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID),'')"
        strsql += vbCrLf + " ELSE ISNULL((SELECT SUBITEMNAME FROM  " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID),'') END ITEMNAME"
        strsql += vbCrLf + " ,1 TYPE"
        strsql += vbCrLf + " ,I.SNO [SERNO]"
        strsql += vbCrLf + " ,'RECEIPT' TABLENAME"
        strsql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL) TAXID "
        strsql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL) TAXPER "
        strsql += vbCrLf + " ,I.CATCODE"
        strsql += vbCrLf + " ,'A' OTABLENAME"
        strsql += vbCrLf + " INTO TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE]"
        strsql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT I "
        strsql += vbCrLf + " WHERE TRANDATE = '" & strBilldate & "' "
        strsql += vbCrLf + "  AND BATCHNO = '" & BatchNo & "' "
        strsql += vbCrLf + " AND TRANTYPE IN ('RRE','RMP') " ' RECEIPT
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        smithIssueReceiptStone(BatchNo, "'RRE','RMP'", mimrformat, 2, "RECEIPT", "A", "1")

        Dim cntrecet As Double = 0
        strsql = "SELECT COUNT(*) CNT FROM  TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE]"
        cntrecet = Val(GetSqlValue(cn, strsql).ToString)



        If mimrformat = "NEW" Then
            'GST CALCUATION RECEIPT
            strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE] "
            strsql += vbCrLf + " ( "
            strsql += vbCrLf + " SNO,ISSUENO,DATE,CATNAME,HSN,USERNAME,ALLOYNAME,ALLOY,WASTAGENAME"
            strsql += vbCrLf + " ,WASTAGE,PCS,GRSWT,NETWT,RATE,DIAWT,MC,TAX"
            strsql += vbCrLf + " ,AMOUNT,ACCODE,REMARK1,REMARK2,TRANTYPE,RESULT,TTYPE,TOUCH,PURITY,PUREWT,LESSWT,DIAAMT"
            strsql += vbCrLf + " ,ITEMNAME,TYPE,SERNO,TABLENAME,TAXID,TAXPER,CATCODE,OTABLENAME "
            strsql += vbCrLf + " )"
            strsql += vbCrLf + " SELECT NULL SNO,"
            strsql += vbCrLf + " NULL ISSUENO , "
            strsql += vbCrLf + " NULL AS DATE,"
            strsql += vbCrLf + " NULL AS CATNAME,"
            strsql += vbCrLf + " NULL HSN, "
            strsql += vbCrLf + " NULL USERNAME,"
            strsql += vbCrLf + " NULL  AS ALLOYNAME,"
            strsql += vbCrLf + " 0  AS ALLOY,"
            strsql += vbCrLf + " NULL AS WASTAGENAME,"
            strsql += vbCrLf + " NULL WASTAGE, "
            strsql += vbCrLf + " NULL PCS,"
            strsql += vbCrLf + " NULL GRSWT,"
            strsql += vbCrLf + " NULL NETWT,"
            strsql += vbCrLf + " NULL RATE,"
            strsql += vbCrLf + " NULL AS DIAWT,"
            strsql += vbCrLf + " NULL AS MC,"
            strsql += vbCrLf + " NULL TAX,"
            strsql += vbCrLf + " TAXAMOUNT AMOUNT,"
            strsql += vbCrLf + " NULL ACCODE,NULL REMARK1,NULL REMARK2, '' TRANTYPE "
            strsql += vbCrLf + " ,'3.0' AS RESULT"
            strsql += vbCrLf + " , NULL TTYPE "
            strsql += vbCrLf + " , 0 TOUCH"
            strsql += vbCrLf + " , NULL PURITY"
            strsql += vbCrLf + " , NULL PUREWT"
            strsql += vbCrLf + " , NULL LESSWT"
            strsql += vbCrLf + " , NULL DIAAMT"
            strsql += vbCrLf + " , '' ITEMNAME"
            strsql += vbCrLf + " ,1 TYPE"
            strsql += vbCrLf + " ,'ZZZZZZZZ' [SERNO]"
            strsql += vbCrLf + " ,'RECEIPT' TABLENAME"
            strsql += vbCrLf + " ,TAXID,TAXPER "
            strsql += vbCrLf + " ,NULL CATCODE"
            strsql += vbCrLf + " ,'A' OTABLENAME"
            strsql += vbCrLf + "  FROM ( "
            strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT)TAXAMOUNT,BATCHNO  "
            strsql += vbCrLf + " ,CASE WHEN TAXTYPE = 'RG' THEN 'GST Reversal on wastage' ELSE '' END TAXTYPENAME"
            strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
            strsql += vbCrLf + " AND TAXID = 'CG' "
            strsql += vbCrLf + " AND ISNULL(TAXTYPE,'') <> 'RG'"
            strsql += vbCrLf + " AND TRANTYPE IN ('RRE','RMP') " ' RECEIPT
            strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TAXTYPE"
            strsql += vbCrLf + " UNION ALL"
            strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO "
            strsql += vbCrLf + " ,CASE WHEN TAXTYPE = 'RG' THEN 'GST Reversal on wastage' ELSE '' END TAXTYPENAME"
            strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
            strsql += vbCrLf + " AND TAXID = 'SG' "
            strsql += vbCrLf + " AND ISNULL(TAXTYPE,'') <> 'RG'"
            strsql += vbCrLf + " AND TRANTYPE IN ('RRE','RMP') " ' RECEIPT
            strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TAXTYPE"
            strsql += vbCrLf + " UNION ALL"
            strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO "
            strsql += vbCrLf + " ,CASE WHEN TAXTYPE = 'RG' THEN 'GST Reversal on wastage' ELSE '' END TAXTYPENAME"
            strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
            strsql += vbCrLf + " AND TAXID = 'IG' "
            strsql += vbCrLf + " AND ISNULL(TAXTYPE,'') <> 'RG'"
            strsql += vbCrLf + " AND TRANTYPE IN ('RRE','RMP') " ' RECEIPT
            strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TAXTYPE"
            strsql += vbCrLf + " UNION ALL"
            strsql += vbCrLf + " SELECT 'TC' TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO, '' TAXTYPENAME "
            strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
            strsql += vbCrLf + " AND TAXID = 'TC' "
            strsql += vbCrLf + " AND TRANTYPE IN ('RRE','RMP') " ' RECEIPT
            strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO"

            If cntrecet > 0 Then
                strsql += vbCrLf + " UNION ALL"
                strsql += vbCrLf + " SELECT 'TD' TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO,'' TAXTYPENAME "
                strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
                strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
                strsql += vbCrLf + " AND TRANTYPE IN ('TR') " ' RECEIPT
                strsql += vbCrLf + " AND (TAXID IN (SELECT DISTINCT CAST(TDSCATID AS varchar) FROM " & cnAdminDb & "..TDSCATEGORY) OR TAXID='0' OR TAXID = 'TD') "
                strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO"
                If AccRoundReceipt > 0 Or (AccRoundReceipt > 0 And AccRoundIssue > 0) Then
                    strsql += vbCrLf + " UNION ALL"
                    strsql += vbCrLf + " SELECT 'ZRO' TAXID,0 TAXPER "
                    strsql += vbCrLf + " ,SUM(CASE WHEN TRANMODE='D' THEN AMOUNT ELSE -1* AMOUNT END) TAXAMOUNT "
                    strsql += vbCrLf + " ,BATCHNO,'' TAXTYPENAME "
                    strsql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN "
                    strsql += vbCrLf + " WHERE TRANDATE = '" & strBilldate & "' "
                    strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
                    strsql += vbCrLf + " AND PAYMODE='RO' "
                    strsql += vbCrLf + " GROUP BY BATCHNO"
                End If
            End If
            strsql += vbCrLf + " )X"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()

            Dim newRow As Integer = 0
            strsql = " select COUNT(*) CNT from TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE] "
            newRow = Val(objGPack.GetSqlValue(strsql).ToString)
            If newRow > 0 Then
                strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE] "
                strsql += vbCrLf + " ( "
                strsql += vbCrLf + " SNO,ISSUENO,DATE,CATNAME,HSN,USERNAME,ALLOYNAME,ALLOY,WASTAGENAME"
                strsql += vbCrLf + " ,WASTAGE,PCS,GRSWT,NETWT,RATE,DIAWT,MC,TAX"
                strsql += vbCrLf + " ,AMOUNT,ACCODE,REMARK1,REMARK2,TRANTYPE,RESULT,TTYPE,TOUCH,PURITY,PUREWT,LESSWT,DIAAMT"
                strsql += vbCrLf + " ,ITEMNAME,TYPE,SERNO,TABLENAME,TAXID,TAXPER,CATCODE,OTABLENAME "
                strsql += vbCrLf + " )"
                strsql += vbCrLf + " SELECT NULL SNO,"
                strsql += vbCrLf + " NULL ISSUENO , "
                strsql += vbCrLf + " NULL AS DATE,"
                strsql += vbCrLf + " NULL AS CATNAME,"
                strsql += vbCrLf + " NULL HSN, "
                strsql += vbCrLf + " NULL USERNAME,"
                strsql += vbCrLf + " NULL  AS ALLOYNAME,"
                strsql += vbCrLf + " 0  AS ALLOY,"
                strsql += vbCrLf + " NULL AS WASTAGENAME,"
                strsql += vbCrLf + " NULL WASTAGE, "
                strsql += vbCrLf + " NULL PCS,"
                strsql += vbCrLf + " NULL GRSWT,"
                strsql += vbCrLf + " NULL NETWT,"
                strsql += vbCrLf + " NULL RATE,"
                strsql += vbCrLf + " NULL AS DIAWT,"
                strsql += vbCrLf + " NULL AS MC,"
                strsql += vbCrLf + " NULL TAX,"
                strsql += vbCrLf + " 0 AMOUNT,"
                strsql += vbCrLf + " NULL ACCODE,NULL REMARK1,NULL REMARK2, '' TRANTYPE "
                strsql += vbCrLf + " ,'4.0' AS RESULT"
                strsql += vbCrLf + " , NULL TTYPE "
                strsql += vbCrLf + " , 0 TOUCH"
                strsql += vbCrLf + " , NULL PURITY"
                strsql += vbCrLf + " , NULL PUREWT"
                strsql += vbCrLf + " , NULL LESSWT"
                strsql += vbCrLf + " , NULL DIAAMT"
                strsql += vbCrLf + " , '' ITEMNAME"
                strsql += vbCrLf + " ,1 TYPE"
                strsql += vbCrLf + " ,'ZZZZZZZZZZZZ' [SERNO]"
                strsql += vbCrLf + " ,'RECEIPT' TABLENAME"
                strsql += vbCrLf + " ,0 TAXID,0 TAXPER "
                strsql += vbCrLf + " ,NULL CATCODE"
                strsql += vbCrLf + " ,'A' OTABLENAME"
                cmd = New OleDbCommand(strsql, cn)
                cmd.ExecuteNonQuery()
            End If
        End If

        'ISSUE & RECEIPT PURCHASE
        strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE] "
        strsql += vbCrLf + " SELECT ROW_NUMBER() OVER(ORDER BY TRANNO DESC) AS SNO,"
        strsql += vbCrLf + " 'IJ' + FORMAT(TRANNO, '0000') AS ISSUENO , "
        strsql += vbCrLf + " CONVERT(VARCHAR(15),TRANDATE,103) AS DATE,"
        strsql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) AS CATNAME,"
        strsql += vbCrLf + " CASE WHEN ISNULL((SELECT HSN FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID), '') <> '' THEN "
        strsql += vbCrLf + " ISNULL((SELECT HSN FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID), '') "
        strsql += vbCrLf + " ELSE (SELECT HSN FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)  END HSN, "
        strsql += vbCrLf + " (SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = I.USERID) USERNAME,"
        strsql += vbCrLf + " CASE WHEN ALLOY > 0 THEN 'ADD : ALLOY' ELSE '' END  AS ALLOYNAME,"
        strsql += vbCrLf + " CASE WHEN ALLOY > 0 THEN (CONVERT(NUMERIC(15,3), ALLOY)) ELSE 0 END  AS ALLOY,"
        strsql += vbCrLf + " CASE WHEN  WASTAGE > 0 then 'LESS : WASTAGE' ELSE '' END AS WASTAGENAME,"
        strsql += vbCrLf + " CASE WHEN  WASTAGE > 0 then WASTAGE ELSE 0 END AS WASTAGE, "
        strsql += vbCrLf + " CASE WHEN PCS > 0 THEN PCS END AS PCS,"
        strsql += vbCrLf + " CASE WHEN GRSWT > 0 THEN (CONVERT(NUMERIC(15,3), GRSWT)) END AS GRSWT,"
        strsql += vbCrLf + " CASE WHEN NETWT > 0 THEN (CONVERT(NUMERIC(15,3), NETWT))  END AS NETWT,"
        strsql += vbCrLf + " CASE WHEN RATE > 0 THEN (CONVERT(NUMERIC(15,2), RATE)) END AS RATE,"
        strsql += vbCrLf + " (SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT,"
        strsql += vbCrLf + " CASE WHEN MCHARGE > 0 THEN MCHARGE END AS MC,"
        strsql += vbCrLf + " CASE WHEN TAX > 0 THEN TAX ELSE 0 END AS TAX,"
        strsql += vbCrLf + " AMOUNT,"
        strsql += vbCrLf + " ACCODE "
        strsql += vbCrLf + ",REMARK1"
        strsql += vbCrLf + ",REMARK2"
        If mimrformat = "NEW" Then
            strsql += vbCrLf + " ,(CASE WHEN TRANTYPE = 'IIS' THEN 'DEBIT NOTE' ELSE CASE WHEN TRANTYPE = 'IMP' THEN 'PURIFICATION ISSUE VOUCHER' ELSE 'PURCHASE RETURN' END END) TRANTYPE"
        Else
            strsql += vbCrLf + " ,(CASE WHEN TRANTYPE = 'IIS' THEN 'ISSUE FOR JOBWORK' ELSE CASE WHEN TRANTYPE = 'IMP' THEN 'PURIFICATION ISSUE VOUCHER' ELSE 'PURCHASE RETURN' END END) TRANTYPE"
        End If
        strsql += vbCrLf + " , 5 AS RESULT"
        strsql += vbCrLf + " , TRANTYPE AS TTYPE "
        strsql += vbCrLf + " , TOUCH"
        strsql += vbCrLf + " , PURITY"
        strsql += vbCrLf + " , PUREWT"
        strsql += vbCrLf + " , LESSWT"
        strsql += vbCrLf + " ,(SELECT SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAAMT"
        strsql += vbCrLf + " , CASE WHEN ISNULL((SELECT SUBITEMNAME FROM  " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID),'') ='' THEN "
        strsql += vbCrLf + " ISNULL((SELECT ITEMNAME FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID),'')"
        strsql += vbCrLf + " ELSE ISNULL((SELECT SUBITEMNAME FROM  " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID),'') END ITEMNAME"
        strsql += vbCrLf + " ,2 TYPE"
        strsql += vbCrLf + " ,I.SNO [SERNO]"
        strsql += vbCrLf + " ,'ISSUE' TABLENAME"
        strsql += vbCrLf + " ,NULL TAXID, NULL TAXPER "
        strsql += vbCrLf + " ,I.CATCODE CATCODE"
        strsql += vbCrLf + " ,'B' OTABLENAME"
        strsql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I "
        strsql += vbCrLf + " WHERE TRANDATE = '" & strBilldate & "' "
        strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
        strsql += vbCrLf + " AND TRANTYPE IN('IIS','IPU','IMP') " ' ISSUE 
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT ROW_NUMBER() OVER(ORDER BY TRANNO DESC) AS SNO,"
        strsql += vbCrLf + " 'IJ' + FORMAT(TRANNO, '0000') AS ISSUENO , "
        strsql += vbCrLf + " CONVERT(VARCHAR(15),TRANDATE,103) AS DATE,"
        strsql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) AS CATNAME,"
        strsql += vbCrLf + " CASE WHEN ISNULL((SELECT HSN FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID), '') <> '' THEN "
        strsql += vbCrLf + " ISNULL((SELECT HSN FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID), '') "
        strsql += vbCrLf + " ELSE (SELECT HSN FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)  END HSN, "
        strsql += vbCrLf + " (SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = I.USERID) USERNAME,"
        strsql += vbCrLf + " CASE WHEN ALLOY > 0 THEN 'ADD : ALLOY' END  AS ALLOYNAME,"
        strsql += vbCrLf + " ISNULL(CASE WHEN ALLOY > 0 THEN  (CONVERT(NUMERIC(15,3), ALLOY)) END,0)  AS ALLOY,"
        strsql += vbCrLf + " CASE WHEN  WASTAGE > 0 then 'ADD : WASTAGE' END AS WASTAGENAME,"
        strsql += vbCrLf + " CASE WHEN  WASTAGE > 0 then WASTAGE else 0 END AS WASTAGE, "
        strsql += vbCrLf + " CASE WHEN PCS > 0 THEN PCS END AS PCS,"
        strsql += vbCrLf + " CASE WHEN GRSWT > 0 THEN (CONVERT(NUMERIC(15,3), GRSWT)) END AS GRSWT,"
        strsql += vbCrLf + " CASE WHEN NETWT > 0 THEN NETWT END AS NETWT,"
        strsql += vbCrLf + " CASE WHEN RATE > 0 THEN (CONVERT(NUMERIC(15,2), RATE)) END AS RATE,"
        strsql += vbCrLf + " (SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT,"
        strsql += vbCrLf + " CASE WHEN MCHARGE > 0 THEN MCHARGE END AS MC,"
        strsql += vbCrLf + " CASE WHEN TAX > 0 THEN TAX END AS TAX,"
        strsql += vbCrLf + " AMOUNT,"
        strsql += vbCrLf + " ACCODE,REMARK1,REMARK2, 'PURCHASE VOUCHER' TRANTYPE "
        strsql += vbCrLf + " , 6 AS RESULT"
        strsql += vbCrLf + " , TRANTYPE AS TTYPE "
        strsql += vbCrLf + " , TOUCH"
        strsql += vbCrLf + " , PURITY"
        strsql += vbCrLf + " , PUREWT"
        strsql += vbCrLf + " , LESSWT"
        strsql += vbCrLf + " ,(SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAAMT"
        strsql += vbCrLf + " , CASE WHEN ISNULL((SELECT SUBITEMNAME FROM  " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID),'') ='' THEN "
        strsql += vbCrLf + " ISNULL((SELECT ITEMNAME FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID),'')"
        strsql += vbCrLf + " ELSE ISNULL((SELECT SUBITEMNAME FROM  " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID),'') END ITEMNAME"
        strsql += vbCrLf + " ,2 TYPE"
        strsql += vbCrLf + " ,I.SNO [SERNO]"
        strsql += vbCrLf + " ,'RECEIPT' TABLENAME"
        strsql += vbCrLf + " ,NULL TAXID, NULL TAXPER "
        strsql += vbCrLf + " ,I.CATCODE CATCODE"
        strsql += vbCrLf + " ,'A' OTABLENAME"
        strsql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT I "
        strsql += vbCrLf + " WHERE TRANDATE = '" & strBilldate & "' "
        strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
        strsql += vbCrLf + " AND TRANTYPE IN ('RPU') " ' RPU - > PURCHASE VOUCHER
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
        smithIssueReceiptStone(BatchNo, "'RPU'", mimrformat, 7, "RECEIPT", "A", "2")
        If mimrformat = "NEW" Then
            'GST CALCUATION RECEIPT
            strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE] "
            strsql += vbCrLf + " ( "
            strsql += vbCrLf + " SNO,ISSUENO,DATE,CATNAME,HSN,USERNAME,ALLOYNAME,ALLOY,WASTAGENAME"
            strsql += vbCrLf + " ,WASTAGE,PCS,GRSWT,NETWT,RATE,DIAWT,MC,TAX"
            strsql += vbCrLf + " ,AMOUNT,ACCODE,REMARK1,REMARK2,TRANTYPE,RESULT,TTYPE,TOUCH,PURITY,PUREWT,LESSWT,DIAAMT"
            strsql += vbCrLf + " ,ITEMNAME,TYPE,SERNO,TABLENAME "
            strsql += vbCrLf + " ,TAXID,TAXPER,CATCODE,OTABLENAME "
            strsql += vbCrLf + " )"
            strsql += vbCrLf + " SELECT NULL SNO,"
            strsql += vbCrLf + " NULL ISSUENO , "
            strsql += vbCrLf + " NULL AS DATE,"
            strsql += vbCrLf + " NULL AS CATNAME,"
            strsql += vbCrLf + " NULL HSN, "
            strsql += vbCrLf + " NULL USERNAME,"
            strsql += vbCrLf + " NULL  AS ALLOYNAME,"
            strsql += vbCrLf + " 0  AS ALLOY,"
            strsql += vbCrLf + " NULL AS WASTAGENAME,"
            strsql += vbCrLf + " NULL WASTAGE, "
            strsql += vbCrLf + " NULL PCS,"
            strsql += vbCrLf + " NULL GRSWT,"
            strsql += vbCrLf + " NULL NETWT,"
            strsql += vbCrLf + " NULL RATE,"
            strsql += vbCrLf + " NULL AS DIAWT,"
            strsql += vbCrLf + " NULL AS MC,"
            strsql += vbCrLf + " NULL TAX,"
            strsql += vbCrLf + " TAXAMOUNT AMOUNT,"
            strsql += vbCrLf + " NULL ACCODE,NULL REMARK1,NULL REMARK2, '' TRANTYPE "
            strsql += vbCrLf + " ,'7.0' AS RESULT"
            strsql += vbCrLf + " , NULL TTYPE "
            strsql += vbCrLf + " , 0 TOUCH"
            strsql += vbCrLf + " , NULL PURITY"
            strsql += vbCrLf + " , NULL PUREWT"
            strsql += vbCrLf + " , NULL LESSWT"
            strsql += vbCrLf + " , NULL DIAAMT"
            strsql += vbCrLf + " , '' ITEMNAME"
            strsql += vbCrLf + " ,2 TYPE"
            strsql += vbCrLf + " ,'ZZZZZZZZ' [SERNO]"
            strsql += vbCrLf + " ,CASE WHEN TRANTYPE='RPU' THEN 'RECEIPT' ELSE 'ISSUE' END  TABLENAME"
            strsql += vbCrLf + " ,TAXID,TAXPER "
            strsql += vbCrLf + " ,NULL CATCODE"
            strsql += vbCrLf + " ,CASE WHEN TRANTYPE='RPU' THEN 'A' ELSE 'B' END  OTABLENAME"
            strsql += vbCrLf + "  FROM ( "
            strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT)TAXAMOUNT,BATCHNO,TRANTYPE  "
            strsql += vbCrLf + " ,CASE WHEN TAXTYPE = 'RG' THEN 'GST Reversal on wastage' ELSE '' END TAXTYPENAME"
            strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
            strsql += vbCrLf + " AND TAXID = 'CG' "
            strsql += vbCrLf + " AND ISNULL(TAXTYPE,'') <> 'RG'"
            strsql += vbCrLf + " AND TRANTYPE IN ('IIS','IPU','IMP','RPU') " ' RECEIPT
            strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TAXTYPE,TRANTYPE"
            strsql += vbCrLf + " UNION ALL"
            strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO,TRANTYPE "
            strsql += vbCrLf + " ,CASE WHEN TAXTYPE = 'RG' THEN 'GST Reversal on wastage' ELSE '' END TAXTYPENAME"
            strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
            strsql += vbCrLf + " AND TAXID = 'SG' "
            strsql += vbCrLf + " AND ISNULL(TAXTYPE,'') <> 'RG'"
            strsql += vbCrLf + " AND TRANTYPE IN ('IIS','IPU','IMP','RPU') " ' RECEIPT
            strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TAXTYPE,TRANTYPE"
            strsql += vbCrLf + " UNION ALL"
            strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO,TRANTYPE "
            strsql += vbCrLf + " ,CASE WHEN TAXTYPE = 'RG' THEN 'GST Reversal on wastage' ELSE '' END TAXTYPENAME"
            strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
            strsql += vbCrLf + " AND TAXID = 'IG' "
            strsql += vbCrLf + " AND ISNULL(TAXTYPE,'') <> 'RG'"
            strsql += vbCrLf + " AND TRANTYPE IN ('IIS','IPU','IMP','RPU') " ' RECEIPT
            strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TAXTYPE,TRANTYPE"
            strsql += vbCrLf + " UNION ALL"
            strsql += vbCrLf + " SELECT 'TC' TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO,TRANTYPE "
            strsql += vbCrLf + " ,'' TAXTYPENAME "
            strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
            strsql += vbCrLf + " AND TAXID = 'TC' "
            ' strsql += vbCrLf + " AND TRANTYPE IN ('RRE','RMP') " ' RECEIPT
            strsql += vbCrLf + " AND TRANTYPE IN ('IIS','IPU','IMP','RPU') " ' ISSUE
            strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TRANTYPE"
            If cntrecet = 0 Then
                strsql += vbCrLf + " UNION ALL"
                strsql += vbCrLf + " SELECT 'TC' TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO,'RPU' TRANTYPE,'' TAXTYPENAME "
                strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
                strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
                strsql += vbCrLf + " AND TRANTYPE IN ('TR') " ' ISSUE
                strsql += vbCrLf + " AND "
                strsql += vbCrLf + " (TAXID IN (SELECT DISTINCT CAST(TDSCATID AS varchar) FROM " & cnAdminDb & "..TDSCATEGORY) OR TAXID IN ('TC'))"
                strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TRANTYPE"
                strsql += vbCrLf + " UNION ALL"
                strsql += vbCrLf + " SELECT 'TD' TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO,'RPU' TRANTYPE,'' TAXTYPENAME "
                strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
                strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
                strsql += vbCrLf + " AND TRANTYPE IN ('TR') " ' ISSUE
                strsql += vbCrLf + " AND "
                strsql += vbCrLf + " (TAXID IN (SELECT DISTINCT CAST(TDSCATID AS varchar) FROM " & cnAdminDb & "..TDSCATEGORY) OR TAXID IN ('TD'))"
                strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TRANTYPE"
                strsql += vbCrLf + " UNION ALL"
                strsql += vbCrLf + " SELECT 'ZRO' TAXID,0 TAXPER "
                strsql += vbCrLf + " ,SUM(CASE WHEN TRANMODE='D' THEN AMOUNT ELSE -1* AMOUNT END) TAXAMOUNT "
                strsql += vbCrLf + " ,BATCHNO,'RPU' TRANTYPE,'' TAXTYPENAME "
                strsql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN "
                strsql += vbCrLf + " WHERE TRANDATE = '" & strBilldate & "' "
                strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
                strsql += vbCrLf + " AND PAYMODE='RO' "
                strsql += vbCrLf + " GROUP BY BATCHNO"
            Else
                strsql += vbCrLf + " UNION ALL"
                strsql += vbCrLf + " SELECT 'TD' TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO,TRANTYPE,'' TAXTYPENAME "
                strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
                strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
                strsql += vbCrLf + " AND TRANTYPE IN ('TI') " ' ISSUE
                strsql += vbCrLf + " AND TAXID IN (SELECT DISTINCT CAST(TDSCATID AS varchar) FROM " & cnAdminDb & "..TDSCATEGORY) "
                strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TRANTYPE"
                If AccRoundIssue > 0 Then
                    If (AccRoundReceipt = 0 And AccRoundIssue > 0) Then
                        strsql += vbCrLf + " UNION ALL"
                        strsql += vbCrLf + " SELECT 'ZRO' TAXID,0 TAXPER "
                        strsql += vbCrLf + " ,SUM(CASE WHEN TRANMODE='D' THEN AMOUNT ELSE -1* AMOUNT END) TAXAMOUNT "
                        strsql += vbCrLf + " ,BATCHNO,'' TRANTYPE,'' TAXTYPENAME "
                        strsql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN "
                        strsql += vbCrLf + " WHERE TRANDATE = '" & strBilldate & "' "
                        strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
                        strsql += vbCrLf + " AND PAYMODE='RO' "
                        strsql += vbCrLf + " GROUP BY BATCHNO"
                    End If
                End If
            End If
            strsql += vbCrLf + " )X"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        End If

        Dim Rnd As Double = 0
        strsql = " SELECT SUM(CASE WHEN TRANMODE='D' THEN AMOUNT ELSE -1*AMOUNT END)AMOUNT "
        strsql += vbCrLf + " From " & cnStockDb & "..ACCTRAN WHERE TRANDATE ='" & strBilldate & "'  "
        strsql += vbCrLf + " And BatchNo ='" & BatchNo & "' AND  PAYMODE='RO'"
        Rnd = Val(objGPack.GetSqlValue(strsql).ToString)


        strsql = vbCrLf + "SELECT * INTO TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECETOTAL] FROM ( "
        strsql += vbCrLf + " SELECT 'TOTAL' AS TOTAL,SUM(PCS)PCS "
        If mimrformat = "NEW" Then
            strsql += vbCrLf + " ,SUM(GRSWT) AS GRSWT "
            strsql += vbCrLf + " ,SUM(NETWT) AS NETWT "
        Else
            strsql += vbCrLf + " ,SUM(GRSWT)+SUM(WASTAGE)+SUM(ALLOY) AS GRSWT " 'ALLOY ADD IN 02Dec21
            strsql += vbCrLf + " ,SUM(NETWT)+SUM(WASTAGE)+SUM(ALLOY) AS NETWT " 'ALLOY ADD IN 02Dec21
        End If
        strsql += vbCrLf + " ,SUM(MCHARGE) + ISNULL((SELECT SUM(isnull(STNAMT,0)) FROM " & cnStockDb & "..RECEIPTSTONE WHERE BATCHNO=A.BATCHNO),0) MC"
        strsql += vbCrLf + " ,SUM(AMOUNT+TAX) - ISNULL((SELECT SUM(ISNULL(TAXAMOUNT,0)) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO =A.BATCHNO And TAXTYPE='TD'),0) "
        strsql += vbCrLf + " + ISNULL((SELECT SUM(ISNULL(TAXAMOUNT,0)) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO =A.BATCHNO And TAXID='TC'),0) "
        strsql += vbCrLf + " - ISNULL((SELECT SUM(ISNULL(TAXAMOUNT,0)) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO =A.BATCHNO And TAXID='RG'),0) + (" & Rnd & ") AMOUNT, "
        strsql += vbCrLf + " SUM(LESSWT) LESSWT,"
        strsql += vbCrLf + " SUM(PUREWT) PUREWT"
        strsql += vbCrLf + " ,'R' RECPAY"
        strsql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE BATCHNO = A.BATCHNO),0) DIAWT "
        strsql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS A "
        strsql += vbCrLf + " WHERE TRANDATE = '" & strBilldate & "' "
        strsql += vbCrLf + " AND TRANTYPE IN('RRE','RMP') " ' RECEIPT TOTAL
        strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
        strsql += vbCrLf + " GROUP BY BATCHNO"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT 'TOTAL' AS TOTAL"
        strsql += vbCrLf + " ,SUM(PCS)PCS "
        If mimrformat = "NEW" Then
            strsql += vbCrLf + " ,SUM(GRSWT)GRSWT "
            strsql += vbCrLf + " ,SUM(NETWT)NETWT "
        Else
            strsql += vbCrLf + " ,SUM(GRSWT+ISNULL(ALLOY,0))GRSWT "
            strsql += vbCrLf + " ,SUM(NETWT+ISNULL(ALLOY,0))NETWT "
        End If
        strsql += vbCrLf + " ,SUM(MCHARGE)+ ISNULL((SELECT SUM(isnull(STNAMT,0)) FROM " & cnStockDb & "..RECEIPTSTONE WHERE BATCHNO=A.BATCHNO),0) MC"
        strsql += vbCrLf + " ,SUM(AMOUNT+TAX)  "
        strsql += vbCrLf + " + ISNULL((SELECT SUM(ISNULL(TAXAMOUNT,0)) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO =A.BATCHNO And TAXID in('TC')),0) "
        strsql += vbCrLf + " - ISNULL((SELECT SUM(ISNULL(TAXAMOUNT,0)) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO =A.BATCHNO And TAXID in('TD')),0) "
        strsql += vbCrLf + " - ISNULL((SELECT SUM(ISNULL(TAXAMOUNT,0)) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO =A.BATCHNO And TAXTYPE in('TD') AND TAXID IN (SELECT CAST(TDSCATID AS VARCHAR) FROM " & cnAdminDb & "..TDSCATEGORY)),0) "
        strsql += vbCrLf + " + (" & Rnd & ") AMOUNT, "
        strsql += vbCrLf + " SUM(LESSWT) LESSWT,"
        strsql += vbCrLf + " SUM(PUREWT) PUREWT"
        strsql += vbCrLf + " ,'R' RECPAY"
        strsql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE BATCHNO = A.BATCHNO),0) DIAWT"
        strsql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS A WHERE TRANDATE = '" & strBilldate & "' "
        strsql += vbCrLf + " AND TRANTYPE = 'RPU' " ' PURCHASE TOTAL
        strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "'"
        strsql += vbCrLf + " GROUP BY BATCHNO"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT 'TOTAL' AS TOTAL,SUM(PCS)PCS "
        If mimrformat = "NEW" Then
            strsql += vbCrLf + " , SUM(GRSWT) + SUM(ALLOY) AS GRSWT "
            strsql += vbCrLf + " , SUM(GRSWT) + SUM(ALLOY) AS NETWT "
        Else
            strsql += vbCrLf + " , SUM(GRSWT) + SUM(ALLOY) AS GRSWT "
            strsql += vbCrLf + " , SUM(NETWT) + SUM(ALLOY) AS NETWT "
        End If
        strsql += vbCrLf + " ,SUM(MCHARGE) +ISNULL((SELECT SUM(isnull(STNAMT,0)) FROM " & cnStockDb & "..ISSSTONE WHERE BATCHNO=A.BATCHNO),0) MC"
        strsql += vbCrLf + " ,SUM(AMOUNT+TAX) + ISNULL((SELECT SUM(ISNULL(TAXAMOUNT,0)) FROM " & cnStockDb & "..TAXTRAN WHERE BATCHNO =A.BATCHNO And TAXID='TC'),0) + (" & Rnd & ") AMOUNT, "
        strsql += vbCrLf + " SUM(LESSWT) LESSWT,"
        strsql += vbCrLf + " SUM(PUREWT) PUREWT"
        strsql += vbCrLf + " ,'I' RECPAY"
        strsql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE BATCHNO = A.BATCHNO),0) DIAWT"
        strsql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS A WHERE "
        strsql += vbCrLf + " TRANDATE = '" & strBilldate & "' "
        strsql += vbCrLf + " AND TRANTYPE IN('IIS','IPU','IMP') " ' ISSUE TOTAL
        strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "'"
        strsql += vbCrLf + " GROUP BY BATCHNO"
        strsql += vbCrLf + " ) X"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        If mimrformat = "NEW" Then
            strsql = vbCrLf + " SELECT * INTO TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE_TAXTRAN] FROM ( "
            strsql += vbCrLf + " SELECT 0 TAXID, 0 TAXPER, 0 TAXAMOUNT,'' BATCHNO  "
            strsql += vbCrLf + " ,'' TAXTYPENAME"
            strsql += vbCrLf + " )X"
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        Else
            ''GST Value
            strsql = vbCrLf + "SELECT * INTO TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE_TAXTRAN] FROM ( "
            strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT)TAXAMOUNT,BATCHNO  "
            strsql += vbCrLf + " ,CASE WHEN TAXTYPE = 'RG' THEN 'GST Reversal on wastage' ELSE '' END TAXTYPENAME"
            strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
            strsql += vbCrLf + " AND TAXID = 'CG' "
            strsql += vbCrLf + " AND ISNULL(TAXTYPE,'') <> 'RG'"
            strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TAXTYPE"
            strsql += vbCrLf + " UNION ALL"
            strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO "
            strsql += vbCrLf + " ,CASE WHEN TAXTYPE = 'RG' THEN 'GST Reversal on wastage' ELSE '' END TAXTYPENAME"
            strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
            strsql += vbCrLf + " AND TAXID = 'SG' "
            strsql += vbCrLf + " AND ISNULL(TAXTYPE,'') <> 'RG'"
            strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TAXTYPE"
            strsql += vbCrLf + " UNION ALL"
            strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO "
            strsql += vbCrLf + " ,CASE WHEN TAXTYPE = 'RG' THEN 'GST Reversal on wastage' ELSE '' END TAXTYPENAME"
            strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
            strsql += vbCrLf + " AND TAXID = 'IG' "
            strsql += vbCrLf + " AND ISNULL(TAXTYPE,'') <> 'RG'"
            strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TAXTYPE"
            strsql += vbCrLf + " UNION ALL"
            strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO, '' TAXTYPENAME "
            strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
            strsql += vbCrLf + " AND TAXID = 'TC' "
            strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO"
            strsql += vbCrLf + " UNION ALL"
            strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO,'' TAXTYPENAME "
            strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
            strsql += vbCrLf + " AND TAXID IN (SELECT DISTINCT CAST(TDSCATID AS varchar) FROM " & cnAdminDb & "..TDSCATEGORY) "
            strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO"
            strsql += vbCrLf + " )X"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        End If
        'Wastage GST Value
        strsql = vbCrLf + "SELECT * INTO TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE_TAXTRAN2] FROM ( "
        strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT)TAXAMOUNT,BATCHNO  "
        strsql += vbCrLf + " ,CASE WHEN TAXTYPE = 'RG' THEN 'GST Reversal on wastage' ELSE '' END TAXTYPENAME"
        strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
        strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
        strsql += vbCrLf + " AND TAXID = 'CG' "
        strsql += vbCrLf + " AND ISNULL(TAXTYPE,'') = 'RG'"
        strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TAXTYPE"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO "
        strsql += vbCrLf + " ,CASE WHEN TAXTYPE = 'RG' THEN 'GST Reversal on wastage' ELSE '' END TAXTYPENAME"
        strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
        strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
        strsql += vbCrLf + " AND TAXID = 'SG' "
        strsql += vbCrLf + " AND ISNULL(TAXTYPE,'') = 'RG'"
        strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TAXTYPE"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO "
        strsql += vbCrLf + " ,CASE WHEN TAXTYPE = 'RG' THEN 'GST Reversal on wastage' ELSE '' END TAXTYPENAME"
        strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
        strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
        strsql += vbCrLf + " AND TAXID = 'IG' "
        strsql += vbCrLf + " AND ISNULL(TAXTYPE,'') = 'RG'"
        strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TAXTYPE"
        strsql += vbCrLf + " )X"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = vbCrLf + " SELECT ACCODE FROM " & cnStockDb & "..RECEIPT "
        strsql += vbCrLf + " WHERE TRANDATE = '" & strBilldate & "' AND BATCHNO = '" & BatchNo & "' "
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT ACCODE FROM " & cnStockDb & "..ISSUE "
        strsql += vbCrLf + " WHERE TRANDATE = '" & strBilldate & "' AND BATCHNO = '" & BatchNo & "' "
        Dim ACCODE As String = GetSqlValue_Bill(strsql)

        strsql = vbCrLf + "SELECT ACCODE"
        strsql += vbCrLf + " ,ACNAME"
        strsql += vbCrLf + " ,DOORNO + ' ' + ADDRESS1  AS ADDRESS1"
        strsql += vbCrLf + " ,ADDRESS2 + ' ' + ADDRESS3 + ' ' + CITY + ' ' + PINCODE AS ADDRESS2 "
        strsql += vbCrLf + " ,CASE WHEN TIN <> '' THEN 'TIN:'+TIN + ' ' END AS TIN "
        strsql += vbCrLf + " ,CASE WHEN PAN <> '' THEN 'PAN:'+PAN END AS PAN "
        strsql += vbCrLf + " ,CASE WHEN ISNULL(GSTNO,'') <> '' THEN 'STATE CODE :' + SUBSTRING(ISNULL(GSTNO,''),1,2) ELSE '' END AS STATEID "
        strsql += vbCrLf + " ,CASE WHEN ISNULL(GSTNO,'') <> '' THEN GSTNO ELSE '' END GSTNO "
        strsql += vbCrLf + " INTO TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE_ACCODE]"
        strsql += vbCrLf + " FROM " & cnAdminDb & "..ACHEAD "
        strsql += vbCrLf + " WHERE 1 = 1 "
        strsql += vbCrLf + " AND ACCODE = '" & ACCODE & "'"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = "SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE] ORDER BY OTABLENAME,TYPE,SERNO,RESULT"
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dsSmithIssueReceipt, "DTISSREC")

        strsql = ""
        strsql += vbCrLf + " SELECT ROW_NUMBER() OVER (ORDER BY OTABLENAME,TYPE,RESULT) SNO"
        strsql += vbCrLf + " ,ISSUENO"
        strsql += vbCrLf + " ,DATE"
        strsql += vbCrLf + " ,CATNAME"
        strsql += vbCrLf + " ,(SELECT HSN FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = A.CATCODE ) HSN"
        strsql += vbCrLf + " ,USERNAME"
        strsql += vbCrLf + " ,SUM(ISNULL(ALLOY,0)) ALLOY"
        strsql += vbCrLf + " ,SUM(ISNULL(WASTAGE,0)) WASTAGE"
        strsql += vbCrLf + " ,SUM(ISNULL(PCS,0))PCS"
        strsql += vbCrLf + " ,SUM(ISNULL(GRSWT,0))GRSWT"
        strsql += vbCrLf + " ,SUM(ISNULL(NETWT,0))NETWT"
        strsql += vbCrLf + " ,RATE"
        strsql += vbCrLf + " ,SUM(ISNULL(DIAWT,0)) DIAWT"
        strsql += vbCrLf + " ,SUM(ISNULL(MC,0)) MC"
        strsql += vbCrLf + " ,SUM(ISNULL(TAX,0)) TAX"
        strsql += vbCrLf + " ,SUM(ISNULL(AMOUNT,0)) AMOUNT"
        strsql += vbCrLf + " ,ACCODE"
        strsql += vbCrLf + " ,'' REMARK1"
        strsql += vbCrLf + " ,'' REMARK2"
        strsql += vbCrLf + " ,TRANTYPE"
        strsql += vbCrLf + " ,RESULT"
        strsql += vbCrLf + " ,TTYPE"
        strsql += vbCrLf + " ,0 TOUCH"
        strsql += vbCrLf + " ,0 PURITY"
        strsql += vbCrLf + " ,0 PUREWT"
        strsql += vbCrLf + " ,SUM(ISNULL(LESSWT,0))LESSWT"
        strsql += vbCrLf + " ,SUM(ISNULL(DIAAMT,0))DIAAMT"
        strsql += vbCrLf + " ,TYPE"
        strsql += vbCrLf + " ,'' SERNO"
        strsql += vbCrLf + " ,CATNAME ITEMNAME"
        strsql += vbCrLf + " ,TABLENAME"
        strsql += vbCrLf + " ,TAXID"
        strsql += vbCrLf + " ,TAXPER"
        strsql += vbCrLf + " ,OTABLENAME"
        strsql += vbCrLf + " FROM TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE] AS A"
        strsql += vbCrLf + " WHERE TABLENAME='RECEIPT'"
        strsql += vbCrLf + " GROUP BY ISSUENO,DATE,CATNAME,USERNAME,A.CATCODE " 'HSN
        strsql += vbCrLf + " ,RATE,ACCODE,TRANTYPE,RESULT,TTYPE,TYPE,TABLENAME,TAXID,TAXPER,A.OTABLENAME"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT ROW_NUMBER() OVER (ORDER BY OTABLENAME,TYPE,RESULT) SNO"
        strsql += vbCrLf + " ,ISSUENO"
        strsql += vbCrLf + " ,DATE"
        strsql += vbCrLf + " ,CATNAME"
        strsql += vbCrLf + " ,(SELECT HSN FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = A.CATCODE ) HSN"
        strsql += vbCrLf + " ,USERNAME"
        strsql += vbCrLf + " ,SUM(ISNULL(ALLOY,0)) ALLOY"
        strsql += vbCrLf + " ,SUM(ISNULL(WASTAGE,0)) WASTAGE"
        strsql += vbCrLf + " ,SUM(ISNULL(PCS,0))PCS"
        strsql += vbCrLf + " ,SUM(ISNULL(GRSWT,0))GRSWT"
        strsql += vbCrLf + " ,SUM(ISNULL(NETWT,0))NETWT"
        strsql += vbCrLf + " ,RATE"
        strsql += vbCrLf + " ,SUM(ISNULL(DIAWT,0)) DIAWT"
        strsql += vbCrLf + " ,SUM(ISNULL(MC,0)) MC"
        strsql += vbCrLf + " ,SUM(ISNULL(TAX,0)) TAX"
        strsql += vbCrLf + " ,SUM(ISNULL(AMOUNT,0)) AMOUNT"
        strsql += vbCrLf + " ,ACCODE"
        strsql += vbCrLf + " ,'' REMARK1"
        strsql += vbCrLf + " ,'' REMARK2"
        strsql += vbCrLf + " ,TRANTYPE"
        strsql += vbCrLf + " ,RESULT"
        strsql += vbCrLf + " ,TTYPE"
        strsql += vbCrLf + " ,0 TOUCH"
        strsql += vbCrLf + " ,0 PURITY"
        strsql += vbCrLf + " ,0 PUREWT"
        strsql += vbCrLf + " ,SUM(ISNULL(LESSWT,0))LESSWT"
        strsql += vbCrLf + " ,SUM(ISNULL(DIAAMT,0))DIAAMT"
        strsql += vbCrLf + " ,TYPE"
        strsql += vbCrLf + " ,'' SERNO"
        strsql += vbCrLf + " ,CATNAME ITEMNAME"
        strsql += vbCrLf + " ,TABLENAME"
        strsql += vbCrLf + " ,TAXID"
        strsql += vbCrLf + " ,TAXPER"
        strsql += vbCrLf + " ,OTABLENAME"
        strsql += vbCrLf + " FROM TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE] AS A "
        strsql += vbCrLf + " WHERE TABLENAME='ISSUE'"
        strsql += vbCrLf + " GROUP BY ISSUENO,DATE,CATNAME,USERNAME " 'HSN
        strsql += vbCrLf + " ,RATE,ACCODE,TRANTYPE,RESULT,TTYPE,TYPE,TABLENAME,TAXID,TAXPER,A.CATCODE,A.OTABLENAME"
        strsql += vbCrLf + " ORDER BY OTABLENAME,TYPE,SERNO,RESULT"
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dsSmithIssueReceipt, "DTISSRECCAT")
        If mimrformat = "NEW" Then
            strsql = " SELECT "
            strsql += vbCrLf + " "
            strsql += vbCrLf + " 'TOTAL' TOTAL"
            strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY='R' THEN PCS ELSE 1*PCS END)PCS"
            strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY='R' THEN GRSWT ELSE 1*GRSWT END)GRSWT"
            strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY='R' THEN NETWT ELSE 1*NETWT END)NETWT"
            strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY='R' THEN MC ELSE 1*MC END)MC"
            strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY='R' THEN AMOUNT ELSE 1*AMOUNT END)AMOUNT"
            strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY='R' THEN LESSWT ELSE 1*LESSWT END)LESSWT"
            strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY='R' THEN PUREWT ELSE 1*PUREWT END)PUREWT"
            strsql += vbCrLf + " ,RECPAY"
            strsql += vbCrLf + " ,CASE WHEN RECPAY = 'R' THEN 1 ELSE 2 END VIEWORDER"
            strsql += vbCrLf + " FROM TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECETOTAL]"
            strsql += vbCrLf + " GROUP BY RECPAY"
            strsql += vbCrLf + " ORDER BY VIEWORDER "
        Else
            strsql = " SELECT "
            strsql += vbCrLf + " "
            strsql += vbCrLf + " 'TOTAL' TOTAL"
            strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY='R' THEN PCS ELSE -1*PCS END)PCS"
            strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY='R' THEN GRSWT ELSE -1*GRSWT END)GRSWT"
            strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY='R' THEN NETWT ELSE -1*NETWT END)NETWT"
            strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY='R' THEN MC ELSE -1*MC END)MC"
            strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY='R' THEN AMOUNT ELSE -1*AMOUNT END)AMOUNT"
            strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY='R' THEN LESSWT ELSE -1*LESSWT END)LESSWT"
            strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY='R' THEN PUREWT ELSE -1*PUREWT END)PUREWT"
            strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY='R' THEN DIAWT ELSE -1*DIAWT END)DIAWT"
            strsql += vbCrLf + " FROM TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECETOTAL]"
        End If
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dsSmithIssueReceipt, "DTISSRECTOTAL")

        strsql = "SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE_ACCODE]"
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dsSmithIssueReceipt, "ACHEAD")

        strsql = "SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE_TAXTRAN]"
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dsSmithIssueReceipt, "TAXTRAN")

        strsql = "SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE_TAXTRAN2]"
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dsSmithIssueReceipt, "TAXTRAN2")
    End Sub

    Public Sub OrderReceipt(ByVal PbatchNo As String, ByVal Type As String, ByVal _ordate As String)

        Dim ds As New DataSet
        strsql = ""
        strsql += vbCrLf + "EXEC " & cnAdminDb & "..PROC_ORDERBILLPRINTVIEW"
        strsql += vbCrLf + "@ADMINDB ='" & cnAdminDb & "' "
        strsql += vbCrLf + ",@STOCKDB ='" & cnStockDb & "' "
        strsql += vbCrLf + ",@SYSID ='" & SystemName & "' "
        strsql += vbCrLf + ",@BILLDATE ='" & _ordate & "' "
        strsql += vbCrLf + ",@BATCHNO ='" & PbatchNo & "' "
        cmd = New OleDbCommand(strsql, cn)
        da = New OleDbDataAdapter(cmd)
        da.Fill(ds)
        dtOrderRepair = ds.Tables(0)

        Tranno = dtOrderRepair.Rows(0).Item("ORNO").ToString
        strBilldate = dtOrderRepair.Rows(0).Item("BILLDATE").ToString
        NodeId = dtOrderRepair.Rows(0).Item("NODE").ToString
        DueDate = dtOrderRepair.Rows(0).Item("DUEDATE").ToString
    End Sub

    'Public Sub oOrderReceipt(ByVal PbatchNo As String, ByVal Type As String, ByVal _ordate As String)
    '    strsql = " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "ORMASTACCTRAN]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "ORMASTACCTRAN]"
    '    strsql += " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "ORMAST]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "ORMAST]"
    '    strsql += " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "OCUSTORDER]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "OCUSTORDER]"
    '    cmd = New OleDbCommand(strsql, cn)
    '    cmd.ExecuteNonQuery()

    '    strsql = " CREATE TABLE TEMPTABLEDB..[TEMP" & SystemName & "ORMASTACCTRAN] (ACCODE VARCHAR(250), BATCHNO_N VARCHAR(20))"
    '    cmd = New OleDbCommand(strsql, cn)
    '    cmd.ExecuteNonQuery()

    '    'Doubt 
    '    'Dim OutSt As Boolean
    '    'strsql = vbCrLf + " SELECT ACCODE+':'+CONVERT(VARCHAR,AMOUNT)AMT,BATCHNO FROM " & cnStockDb & "..ACCTRAN "
    '    'strsql += vbCrLf + " WHERE BATCHNO =  '" & PbatchNo & "' AND TRANMODE = 'D'"
    '    'da = New OleDbDataAdapter(strsql, cn)
    '    'dt = New DataTable
    '    'da.Fill(dt)
    '    'Dim TempAcc As String = ""
    '    'Dim TempBatch As String = ""
    '    'If dt.Rows.Count > 0 Then
    '    '    For i As Integer = 0 To dt.Rows.Count - 1
    '    '        TempAcc += " " + dt.Rows(i).Item("AMT").ToString
    '    '    Next
    '    '    TempBatch = dt.Rows(0).Item("BATCHNO").ToString
    '    '    strsql = "INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ORMASTACCTRAN](ACCODE,BATCHNO_N) VALUES "
    '    '    strsql += " ('" & TempAcc & "', '" & TempBatch & "')"
    '    '    cmd = New OleDbCommand(strsql, cn)
    '    '    cmd.ExecuteNonQuery()
    '    '    OutSt = True
    '    'End If

    '    strsql = vbCrLf + " SELECT "
    '    strsql += vbCrLf + " ROW_NUMBER() OVER(ORDER BY ORDATE DESC) AS SRNO"
    '    strsql += vbCrLf + " ,SUBSTRING(ORNO,6,3) + '-' + SUBSTRING(ORNO,9,1000)  AS ORNO "
    '    strsql += vbCrLf + " ,CONVERT (VARCHAR(15),ORDATE,103) AS BILLDATE"
    '    strsql += vbCrLf + " ,(SELECT CONVERT(VARCHAR(500),ITEMNAME) ITEMNAME from " & cnAdminDb & "..ITEMMAST WHERE itemid = O.ITEMID) AS ITEMNAME"
    '    strsql += vbCrLf + " ,PCS"
    '    strsql += vbCrLf + " ,GRSWT,RATE"
    '    strsql += vbCrLf + " ,BATCHNO AS BATCHNO_N,CONVERT (VARCHAR(15), DUEDATE ,103) AS DUEDATE "
    '    strsql += vbCrLf + " ,(SELECT SUM(AMOUNT) FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO=O.BATCHNO AND PAYMODE='OR')AMOUNT"
    '    strsql += vbCrLf + " ,SYSTEMID AS NODE"
    '    strsql += vbCrLf + " ,1 AS RESULT, 1 AS TYPE, '' AS COLHEAD "
    '    strsql += vbCrLf + " INTO TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] "
    '    strsql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST as O"
    '    strsql += vbCrLf + " WHERE ORDATE = '" & _ordate & "' "
    '    strsql += vbCrLf + " AND BATCHNO =  '" & PbatchNo & "'"
    '    cmd = New OleDbCommand(strsql, cn)
    '    cmd.ExecuteNonQuery()

    '    strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] "
    '    strsql += vbCrLf + " SELECT NULL,NULL,NULL,DESCRIPT,NULL,NULL,NULL,BATCHNO,NULL,NULL,SYSTEMID"
    '    strsql += vbCrLf + " ,2 AS RESULT, 1 AS TYPE, '' AS COLHEAD "
    '    strsql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST as O"
    '    strsql += vbCrLf + " WHERE ORDATE = '" & _ordate & "' "
    '    strsql += vbCrLf + " AND BATCHNO =  '" & PbatchNo & "' "
    '    cmd = New OleDbCommand(strsql, cn)
    '    cmd.ExecuteNonQuery()

    '    strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] "
    '    strsql += vbCrLf + " SELECT NULL,NULL,NULL,REASON,NULL,NULL,NULL,BATCHNO,NULL,NULL,SYSTEMID"
    '    strsql += vbCrLf + " ,3 AS RESULT, 1 AS TYPE, '' AS COLHEAD "
    '    strsql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST as O"
    '    strsql += vbCrLf + " WHERE ORDATE = '" & _ordate & "' "
    '    strsql += vbCrLf + " AND BATCHNO =  '" & PbatchNo & "' "
    '    cmd = New OleDbCommand(strsql, cn)
    '    cmd.ExecuteNonQuery()

    '    strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] "
    '    strsql += vbCrLf + " SELECT NULL,NULL,NULL,'TOTAL',SUM(PCS),SUM(GRSWT),NULL,BATCHNO,NULL,NULL,SYSTEMID"
    '    strsql += vbCrLf + " ,9 AS RESULT, 1 AS TYPE, 'U' AS COLHEAD "
    '    strsql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST as O"
    '    strsql += vbCrLf + " WHERE ORDATE = '" & _ordate & "' "
    '    strsql += vbCrLf + " AND BATCHNO =  '" & PbatchNo & "' "
    '    strsql += vbCrLf + " GROUP BY SYSTEMID,BATCHNO"
    '    cmd = New OleDbCommand(strsql, cn)
    '    cmd.ExecuteNonQuery()

    '    'Space Line
    '    strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] "
    '    strsql += vbCrLf + " SELECT NULL,NULL,NULL,NULL,NULL,NULL,NULL,BATCHNO,NULL,NULL,SYSTEMID"
    '    strsql += vbCrLf + " ,10 AS RESULT, 1 AS TYPE, '' AS COLHEAD "
    '    strsql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST AS O"
    '    strsql += vbCrLf + " WHERE ORDATE = '" & _ordate & "' "
    '    strsql += vbCrLf + " AND BATCHNO =  '" & PbatchNo & "' "
    '    strsql += vbCrLf + " GROUP BY SYSTEMID,BATCHNO"
    '    cmd = New OleDbCommand(strsql, cn)
    '    cmd.ExecuteNonQuery()

    '    strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] "
    '    strsql += vbCrLf + " SELECT NULL,NULL,NULL,'RATE FIXED   :'+CONVERT(VARCHAR(20),RATE)"
    '    strsql += vbCrLf + " ,NULL,NULL,NULL,BATCHNO,NULL,NULL,SYSTEMID"
    '    strsql += vbCrLf + " ,12 AS RESULT, 1 AS TYPE, 'G' AS COLHEAD "
    '    strsql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST AS O "
    '    strsql += vbCrLf + " WHERE ORDATE = '" & _ordate & "' "
    '    strsql += vbCrLf + " AND BATCHNO =  '" & PbatchNo & "' "
    '    strsql += vbCrLf + " AND ORRATE='C' "
    '    strsql += vbCrLf + " GROUP BY SYSTEMID,BATCHNO,RATE"
    '    cmd = New OleDbCommand(strsql, cn)
    '    cmd.ExecuteNonQuery()

    '    strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] "
    '    strsql += vbCrLf + " SELECT NULL,NULL,NULL, "
    '    strsql += vbCrLf + " CASE WHEN PAYMODE = 'CC' THEN 'CARD ' + AC.SHORTNAME + ' ' + CHQCARDNO + '     :' + LTRIM(SUM(AMOUNT)) ELSE "
    '    strsql += vbCrLf + " CASE WHEN PAYMODE = 'AA' THEN 'ADVANCE ' + 'REFER : ' + SUBSTRING(REFNO,1,3) + '-' + SUBSTRING(REFNO,4,10) + '     :' + LTRIM(SUM(AMOUNT)) ELSE "
    '    strsql += vbCrLf + " CASE WHEN PAYMODE = 'CA' THEN 'CASH ' + '     :' + LTRIM(SUM(AMOUNT)) ELSE "
    '    strsql += vbCrLf + " CASE WHEN PAYMODE = 'PU' THEN 'PURCHASE ' + ' :' + LTRIM(SUM(AMOUNT)) ELSE"
    '    strsql += vbCrLf + " CASE WHEN PAYMODE = 'SR' THEN 'SALES RETURN ' + ' :' + LTRIM(SUM(AMOUNT)) ELSE "
    '    strsql += vbCrLf + " CASE WHEN PAYMODE = 'CH' AND ISNULL(FLAG,'') <> '' THEN (SELECT MODENAME FROM " & cnAdminDb & "..PAYMENTMODE WHERE MODEID =ISNULL(T.FLAG,'')) + ' ' + CHQCARDREF + ' ' + CHQCARDNO + '     :' + LTRIM(SUM(AMOUNT)) ELSE "
    '    strsql += vbCrLf + " 'CHEQUE ' + CHQCARDREF + ' ' + CHQCARDNO + '     :' + LTRIM(SUM(AMOUNT))" 'CH
    '    strsql += vbCrLf + " END END END END END END"
    '    strsql += vbCrLf + " ,NULL,NULL,NULL,BATCHNO,NULL,NULL,SYSTEMID, "
    '    strsql += vbCrLf + " 13 RESULT, 1 TYPE, 'G' AS COLHEAD FROM " & cnStockDb & "..ACCTRAN T"
    '    strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CREDITCARD AC ON AC.CARDCODE=T.CARDID "
    '    'strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CREDITCARD AC ON AC.ACCODE=T.ACCODE "
    '    strsql += vbCrLf + " WHERE TRANDATE = '" & _ordate & "' "
    '    strsql += vbCrLf + " AND BATCHNO = '" & PbatchNo & "'"
    '    strsql += vbCrLf + " AND PAYMODE IN ('AA','CC','CH','CA','PU','SR')"
    '    strsql += vbCrLf + " GROUP BY PAYMODE,CHQCARDNO,BATCHNO"
    '    strsql += vbCrLf + " ,AC.SHORTNAME,CHQCARDREF,REFNO,SYSTEMID,ISNULL(T.FLAG,'')"
    '    cmd = New OleDbCommand(strsql, cn)
    '    cmd.ExecuteNonQuery()

    '    strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] "
    '    strsql += vbCrLf + " SELECT NULL,NULL,NULL"
    '    strsql += vbCrLf + " ,'TOTAL ADVANCE  : '+CONVERT(VARCHAR(20),SUM(AMOUNT)) + 'GST : ' + CONVERT(VARCHAR(20),SUM(GSTVAL)) "
    '    strsql += vbCrLf + " ,NULL,NULL,NULL,BATCHNO,NULL,NULL,SYSTEMID"
    '    strsql += vbCrLf + " ,14 AS RESULT, 1 AS TYPE, 'G' AS COLHEAD "
    '    strsql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS O"
    '    strsql += vbCrLf + " WHERE TRANDATE = '" & _ordate & "' "
    '    strsql += vbCrLf + " AND BATCHNO =  '" & PbatchNo & "' "
    '    strsql += vbCrLf + " AND PAYMODE='OR' GROUP BY SYSTEMID,BATCHNO"
    '    cmd = New OleDbCommand(strsql, cn)
    '    cmd.ExecuteNonQuery()

    '    strsql = "SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] ORDER BY RESULT"
    '    da = New OleDbDataAdapter(strsql, cn)
    '    da.Fill(dtOrderRepair)

    '    Tranno = dtOrderRepair.Rows(0).Item("ORNO").ToString
    '    strBilldate = dtOrderRepair.Rows(0).Item("BILLDATE").ToString
    '    NodeId = dtOrderRepair.Rows(0).Item("NODE").ToString
    '    DueDate = dtOrderRepair.Rows(0).Item("DUEDATE").ToString

    '    'strsql = vbCrLf + " SELECT  "
    '    'strsql += vbCrLf + " (SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS PNAME,  "
    '    'strsql += vbCrLf + " (SELECT (DOORNO + ' ' + ADDRESS1) FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS ADDRESS1,"
    '    'strsql += vbCrLf + " (SELECT (ADDRESS2 + ' ' + ADDRESS3 + ' ' + CITY + ' ' + PINCODE ) FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS ADDRESS2,"
    '    'strsql += vbCrLf + " (SELECT CASE WHEN MOBILE<> '' THEN MOBILE ELSE PHONERES END FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS MOBILENO,"
    '    'strsql += vbCrLf + " (SELECT PAN  FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS TIN, BATCHNO AS BATCHNO_N"
    '    'If Type = "ORD" Then
    '    '    strsql += vbCrLf + ",'ORDER BOOKING' AS TYPE"
    '    '    strsql += vbCrLf + ",'Order No' AS TYPE1"
    '    'Else
    '    '    strsql += vbCrLf + ",'REPAIR BOOKING' AS TYPE"
    '    '    strsql += vbCrLf + ",'Repair No' AS TYPE1"
    '    'End If
    '    'strsql += vbCrLf + " INTO TEMPTABLEDB..[TEMP" & SystemName & "OCUSTORDER] "
    '    'strsql += vbCrLf + " FROM " & cnAdminDb & "..CUSTOMERINFO AS P "
    '    'strsql += vbCrLf + " WHERE BATCHNO = '" & PbatchNo & "'"
    '    'cmd = New OleDbCommand(strsql, cn)
    '    'cmd.ExecuteNonQuery()

    'End Sub
    Public Sub CustomerInfoAdvanceReceipt(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        If dsAdvanceReceipt.Tables("CUSTOMER").Rows.Count > 0 And dsAdvanceReceipt.Tables("OUTSTANDING").Rows.Count > 0 And dsAdvanceReceipt.Tables("ACCTRAN").Rows.Count > 0 Then
            Dim pname As String = dsAdvanceReceipt.Tables("CUSTOMER").Rows(0).Item("pname").ToString
            Dim Address1 As String = dsAdvanceReceipt.Tables("CUSTOMER").Rows(0).Item("address1").ToString
            Dim Address2 As String = dsAdvanceReceipt.Tables("CUSTOMER").Rows(0).Item("address2").ToString
            Dim Mobile As String = dsAdvanceReceipt.Tables("CUSTOMER").Rows(0).Item("mobileno").ToString
            Dim Tin As String = dsAdvanceReceipt.Tables("CUSTOMER").Rows(0).Item("tin").ToString
            Dim gstno As String = dsAdvanceReceipt.Tables("CUSTOMER").Rows(0).Item("GSTNO").ToString
            Dim panno As String = dsAdvanceReceipt.Tables("CUSTOMER").Rows(0).Item("PANNO").ToString
            If Tin = "" Then
                Tin = panno
            End If
            Dim TranName As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("tranname").ToString
            Dim Tranno As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("tranno").ToString
            Dim Billdate As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("billdate").ToString
            Dim Field1 As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field1").ToString
            Dim Field2 As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field2").ToString
            Dim Amt As Integer = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("amount").ToString

            Dim PurityType As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("CATCODETYPE").ToString

            Dim AmtWords As String = ConvertRupees.RupeesToWord(Amt)
            Dim Field5 As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field5").ToString
            Dim Remarks As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("REMARKS").ToString
            NodeId = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("node").ToString
            Dim accode As String = dsAdvanceReceipt.Tables("ACCTRAN").Rows(0).Item("ACCODE").ToString

            Dim ItemDetail As String = "ITEM DETAIL"
            Dim RateFixed As String = dsAdvanceReceipt.Tables("ITEMTAG").Rows(0).Item("RATENAME").ToString
            Dim RateValue As String = dsAdvanceReceipt.Tables("ITEMTAG").Rows(0).Item("RATE").ToString

            g.DrawString(pname, fontRegular, BlackBrush, c1, TopY, LeftFormat)
            g.DrawString(pname, fontRegular, BlackBrush, c1, BottomY, LeftFormat)
            g.DrawString(TranName, fontRegular, BlackBrush, c6, TopY, LeftFormat)
            g.DrawString(TranName, fontRegular, BlackBrush, c6, BottomY, LeftFormat)
            g.DrawString("   : " & Tranno, fontRegular, BlackBrush, c7, TopY, LeftFormat)
            g.DrawString("   : " & Tranno, fontRegular, BlackBrush, c7, BottomY, LeftFormat)

            TopY = TopY + 20
            BottomY = BottomY + 20

            g.DrawString(Address1, fontRegular, BlackBrush, c1, TopY, LeftFormat)
            g.DrawString(Address1, fontRegular, BlackBrush, c1, BottomY, LeftFormat)
            g.DrawString("Date", fontRegular, BlackBrush, c6, TopY, LeftFormat)
            g.DrawString("Date", fontRegular, BlackBrush, c6, BottomY, LeftFormat)
            g.DrawString("   : " & Billdate, fontRegular, BlackBrush, c7, TopY, LeftFormat)
            g.DrawString("   : " & Billdate, fontRegular, BlackBrush, c7, BottomY, LeftFormat)

            TopY = TopY + 20
            BottomY = BottomY + 20

            g.DrawString(Address2, fontRegular, BlackBrush, c1, TopY, LeftFormat)
            g.DrawString(Address2, fontRegular, BlackBrush, c1, BottomY, LeftFormat)
            g.DrawString(Field1, fontBoldTitle, Brushes.Black, c3, TopY)
            g.DrawString(Field1, fontBoldTitle, Brushes.Black, c3, BottomY)
            TopY = TopY + 20
            BottomY = BottomY + 20
            g.DrawString(Mobile & IIf(Tin <> "", " PAN : " & Tin, "") & IIf(gstno <> "", " GSTNO : " & gstno, ""), fontRegular, BlackBrush, c1, TopY, LeftFormat)
            g.DrawString(Mobile & IIf(Tin <> "", " PAN : " & Tin, "") & IIf(gstno <> "", " GSTNO : " & gstno, ""), fontRegular, BlackBrush, c1, BottomY, LeftFormat)
            TopY = TopY + 20
            BottomY = BottomY + 20
            g.DrawLine(Pens.Silver, c1, TopY, c9, TopY)
            g.DrawLine(Pens.Silver, c1, BottomY, c9, BottomY)
            TopY = TopY + 20
            BottomY = BottomY + 20
            If printCustoInfo = True Then
                g.DrawString(Field2, fontRegular, BlackBrush, c1, TopY, LeftFormat)
                g.DrawString(Field2, fontRegular, BlackBrush, c1, BottomY, LeftFormat)
                If accode <> "" Then
                    If accode.Length < 50 Then
                        g.DrawString(accode, fontRegular, BlackBrush, c4, TopY, LeftFormat)
                        g.DrawString(accode, fontRegular, BlackBrush, c4, BottomY, LeftFormat)
                    End If
                    If accode.Length >= 50 Then
                        g.DrawString(Mid(accode, 1, 50), New Font("Palatino Linotype", 8, FontStyle.Regular), BlackBrush, c4, TopY, LeftFormat)
                        g.DrawString(Mid(accode, 1, 50), New Font("Palatino Linotype", 8, FontStyle.Regular), BlackBrush, c4, BottomY, LeftFormat)
                        g.DrawString(Mid(accode, 51, 50), New Font("Palatino Linotype", 8, FontStyle.Regular), BlackBrush, c4, TopY + 20, LeftFormat)
                        g.DrawString(Mid(accode, 51, 50), New Font("Palatino Linotype", 8, FontStyle.Regular), BlackBrush, c4, BottomY + 20, LeftFormat)
                        If accode.Length > 100 Then
                            TopY = TopY + 20
                            BottomY = BottomY + 20
                            g.DrawString(Mid(accode, 101), New Font("Palatino Linotype", 8, FontStyle.Regular), BlackBrush, c4, TopY + 20, LeftFormat)
                            g.DrawString(Mid(accode, 101), New Font("Palatino Linotype", 8, FontStyle.Regular), BlackBrush, c4, BottomY + 20, LeftFormat)
                            TopY = TopY - 20
                            BottomY = BottomY - 20
                        End If
                    End If
                End If
                TopY = TopY + 20
                BottomY = BottomY + 20
                g.DrawString(AmtWords, fontRegular, BlackBrush, c1, TopY, LeftFormat)
                g.DrawString(AmtWords, fontRegular, BlackBrush, c1, BottomY, LeftFormat)
                'If dsAdvanceReceipt.Tables("ISSUE").Rows.Count > 0 Then
                '    Dim GrswA As String = dsAdvanceReceipt.Tables("ISSUE").Rows(0).Item("GRSWT")
                '    g.DrawString("GRSWT" & Space(6) & ":" & GrswA, fontRegular, BlackBrush, c4, TopY, LeftFormat)
                '    g.DrawString("GRSWT" & Space(6) & ":" & GrswA, fontRegular, BlackBrush, c4, BottomY, LeftFormat)
                'End If
                TopY = TopY + 20
                BottomY = BottomY + 20
                g.DrawString(Field5, fontRegular, BlackBrush, c1, TopY, LeftFormat)
                g.DrawString(Field5, fontRegular, BlackBrush, c1, BottomY, LeftFormat)
                TopY = TopY + 20
                BottomY = BottomY + 20

                If Remarks <> "" Then
                    g.DrawString(Remarks, fontRegular, BlackBrush, c1, TopY, LeftFormat)
                    g.DrawString(Remarks, fontRegular, BlackBrush, c1, BottomY, LeftFormat)
                    TopY = TopY + 20
                    BottomY = BottomY + 20
                End If

                If dsAdvanceReceipt.Tables("ISSUE").Rows.Count > 0 Then
                    Dim GrswA As String = dsAdvanceReceipt.Tables("ISSUE").Rows(0).Item("GRSWT")
                    Dim AmtA As String = dsAdvanceReceipt.Tables("ISSUE").Rows(0).Item("AMOUNT")
                    g.DrawString("GRSWT" & Space(6) & ":" & GrswA & "GRSAMT" & Space(5) & ":" & AmtA, fontRegular, BlackBrush, c4, TopY, LeftFormat)
                    g.DrawString("GRSWT" & Space(6) & ":" & GrswA & "GRSAMT" & Space(5) & ":" & AmtA, fontRegular, BlackBrush, c4, BottomY, LeftFormat)
                End If
                TopY = TopY + 20
                BottomY = BottomY + 20
                printCustoInfo = False
            End If
            'ADVANCE ONLY DISPLAY
            If dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("Field1").ToString.Contains("ADVANCE ") Then
                g.DrawString(ItemDetail, fontBold, BlackBrush, c1, TopY, LeftFormat)
                g.DrawString(ItemDetail, fontBold, BlackBrush, c1, BottomY, LeftFormat)
                g.DrawString(RateFixed, fontBold, BlackBrush, c3, TopY, LeftFormat)
                g.DrawString(RateFixed, fontBold, BlackBrush, c3, BottomY, LeftFormat)

                g.DrawString(IIf(Val(RateValue) <> 0, " : " & RateValue, ""), fontBold, BlackBrush, c4, TopY, LeftFormat)
                g.DrawString(IIf(Val(RateValue) <> 0, " : " & RateValue, ""), fontBold, BlackBrush, c4, BottomY, LeftFormat)

                g.DrawString(PurityType, fontBold, BlackBrush, c5, TopY, LeftFormat)
                g.DrawString(PurityType, fontBold, BlackBrush, c5, BottomY, LeftFormat)

                TopY = TopY + 15
                BottomY = BottomY + 15
                g.DrawLine(Pens.Black, c1, TopY, 110, TopY)
                g.DrawLine(Pens.Black, c1, BottomY, 110, BottomY)
                TopY = TopY + 20
                BottomY = BottomY + 20
            End If
        End If
    End Sub

    Public Sub AdvancePrint(ByVal pBatchno As String, ByVal _Trandate As String)
        strsql = ""
        strsql += vbCrLf + " EXEC " & cnAdminDb & "..PROC_ADVANCEBILLPRINTVIEW"
        strsql += vbCrLf + " @ADMINDB ='" & cnAdminDb & "'"
        strsql += vbCrLf + " ,@STOCKDB ='" & cnStockDb & "'"
        strsql += vbCrLf + " ,@SYSID ='" & SystemName & "'"
        strsql += vbCrLf + " ,@BILLDATE ='" & _Trandate & "'"
        strsql += vbCrLf + " ,@BATCHNO ='" & pBatchno & "'"
        strsql += vbCrLf + " ,@STEP ='1'"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        Dim dt As New DataTable
        strsql = ""
        strsql += vbCrLf + " EXEC " & cnAdminDb & "..PROC_ADVANCEBILLPRINTVIEW"
        strsql += vbCrLf + " @ADMINDB ='" & cnAdminDb & "'"
        strsql += vbCrLf + " ,@STOCKDB ='" & cnStockDb & "'"
        strsql += vbCrLf + " ,@SYSID ='" & SystemName & "'"
        strsql += vbCrLf + " ,@BILLDATE ='" & _Trandate & "'"
        strsql += vbCrLf + " ,@BATCHNO ='" & pBatchno & "'"
        strsql += vbCrLf + " ,@STEP ='2'"
        cmd = New OleDbCommand(strsql, cn)
        Dim reader As OleDbDataReader = cmd.ExecuteReader
        dt.Load(reader)
        Dim TempAcc As String = ""
        Dim TempBatch As String = ""
        If dt.Rows.Count > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                TempAcc += "," & dt.Rows(i).Item("AMT").ToString
            Next
            'TempBatch = dt.Rows(0).Item("BATCHNO").ToString
            TempBatch = ""
            TempAcc = TempAcc.Trim(",")
            strsql = "INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ACCTRANADVANCERECEIPT](ACCODE,BATCHNO_N) VALUES "
            strsql += " ('" & TempAcc & "', '" & TempBatch & "')"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        End If
        strsql = ""
        strsql += vbCrLf + " EXEC " & cnAdminDb & "..PROC_ADVANCEBILLPRINTVIEW"
        strsql += vbCrLf + " @ADMINDB ='" & cnAdminDb & "'"
        strsql += vbCrLf + " ,@STOCKDB ='" & cnStockDb & "'"
        strsql += vbCrLf + " ,@SYSID ='" & SystemName & "'"
        strsql += vbCrLf + " ,@BILLDATE ='" & _Trandate & "'"
        strsql += vbCrLf + " ,@BATCHNO ='" & pBatchno & "'"
        strsql += vbCrLf + " ,@STEP ='3'"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = ""
        strsql += vbCrLf + " EXEC " & cnAdminDb & "..PROC_ADVANCEBILLPRINTVIEW"
        strsql += vbCrLf + " @ADMINDB ='" & cnAdminDb & "'"
        strsql += vbCrLf + " ,@STOCKDB ='" & cnStockDb & "'"
        strsql += vbCrLf + " ,@SYSID ='" & SystemName & "'"
        strsql += vbCrLf + " ,@BILLDATE ='" & _Trandate & "'"
        strsql += vbCrLf + " ,@BATCHNO ='" & pBatchno & "'"
        strsql += vbCrLf + " ,@STEP ='4'"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = "SELECT DISTINCT BATCHNO FROM " & cnAdminDb & "..ITEMTAG T WHERE BATCHNO='" & pBatchno & "'  " ' DISTINCT NOT USED ORDER BY ITEMID 
        da = New OleDbDataAdapter(strsql, cn)
        Dim DtAdvRecor As New DataTable
        da.Fill(DtAdvRecor)
        If DtAdvRecor.Rows.Count > 0 Then            '
            strsql = ""
            strsql += vbCrLf + " EXEC " & cnAdminDb & "..PROC_ADVANCEBILLPRINTVIEW"
            strsql += vbCrLf + " @ADMINDB ='" & cnAdminDb & "'"
            strsql += vbCrLf + " ,@STOCKDB ='" & cnStockDb & "'"
            strsql += vbCrLf + " ,@SYSID ='" & SystemName & "'"
            strsql += vbCrLf + " ,@BILLDATE ='" & _Trandate & "'"
            strsql += vbCrLf + " ,@BATCHNO ='" & pBatchno & "'"
            strsql += vbCrLf + " ,@STEP ='5.0'"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        Else
            strsql = ""
            strsql += vbCrLf + " EXEC " & cnAdminDb & "..PROC_ADVANCEBILLPRINTVIEW"
            strsql += vbCrLf + " @ADMINDB ='" & cnAdminDb & "'"
            strsql += vbCrLf + " ,@STOCKDB ='" & cnStockDb & "'"
            strsql += vbCrLf + " ,@SYSID ='" & SystemName & "'"
            strsql += vbCrLf + " ,@BILLDATE ='" & _Trandate & "'"
            strsql += vbCrLf + " ,@BATCHNO ='" & pBatchno & "'"
            strsql += vbCrLf + " ,@STEP ='5.1'"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        End If
        strsql = ""
        strsql += vbCrLf + " EXEC " & cnAdminDb & "..PROC_ADVANCEBILLPRINTVIEW"
        strsql += vbCrLf + " @ADMINDB ='" & cnAdminDb & "'"
        strsql += vbCrLf + " ,@STOCKDB ='" & cnStockDb & "'"
        strsql += vbCrLf + " ,@SYSID ='" & SystemName & "'"
        strsql += vbCrLf + " ,@BILLDATE ='" & _Trandate & "'"
        strsql += vbCrLf + " ,@BATCHNO ='" & pBatchno & "'"
        strsql += vbCrLf + " ,@STEP ='6'"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = "SELECT COUNT(*)CNT FROM TEMPTABLEDB..[TEMP" & SystemName & "ITEMTAG] "
        If Val(GetSqlValue_Bill(strsql, "CNT", 0)) > 0 Then
            strsql = " UPDATE TEMPTABLEDB..[TEMP" & SystemName & "ADVANCERECEIPT] SET ISTAG=1"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        End If
        strsql = " SELECT COUNT(*)CNT FROM " & cnStockDb & "..RECEIPT "
        strsql += vbCrLf + " WHERE TRANDATE = '" & _Trandate & "' "
        strsql += vbCrLf + " AND  BATCHNO='" & pBatchno & "' "
        strsql += vbCrLf + " AND TRANTYPE IN ('PU','SR')"
        If Val(GetSqlValue_Bill(strsql, "CNT", 0)) > 0 Then
            strsql = " UPDATE TEMPTABLEDB..[TEMP" & SystemName & "ADVANCERECEIPT] SET ISOG=1"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        End If

        strsql = ""
        strsql += vbCrLf + " EXEC " & cnAdminDb & "..PROC_ADVANCEBILLPRINTVIEW"
        strsql += vbCrLf + " @ADMINDB ='" & cnAdminDb & "'"
        strsql += vbCrLf + " ,@STOCKDB ='" & cnStockDb & "'"
        strsql += vbCrLf + " ,@SYSID ='" & SystemName & "'"
        strsql += vbCrLf + " ,@BILLDATE ='" & _Trandate & "'"
        strsql += vbCrLf + " ,@BATCHNO ='" & pBatchno & "'"
        strsql += vbCrLf + " ,@STEP ='7'"
        cmd = New OleDbCommand(strsql, cn)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dsAdvanceReceipt, "GSTTRAN")


        strsql = " SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "ADVANCERECEIPT]"
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dsAdvanceReceipt, "OUTSTANDING")

        strsql = " SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "CUSTADVANCERECEIPT]"
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dsAdvanceReceipt, "CUSTOMER")

        strsql = " SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "ACCTRANADVANCERECEIPT]"
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dsAdvanceReceipt, "ACCTRAN")

        strsql = " SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "ITEMTAG]"
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dsAdvanceReceipt, "ITEMTAG")

        strsql = "SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "ADVANCEPURRET]"
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dsAdvanceReceipt, "ISSUE")

    End Sub

    'Public Sub OAdvancePrint(ByVal pBatchno As String, ByVal _Trandate As String)

    '    strsql = " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "ADVANCERECEIPT]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "ADVANCERECEIPT]"
    '    strsql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "CUSTADVANCERECEIPT]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "CUSTADVANCERECEIPT]"
    '    strsql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "ACCTRANADVANCERECEIPT]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "ACCTRANADVANCERECEIPT]"
    '    strsql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "ITEMTAG]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "ITEMTAG]"
    '    strsql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "ADVANCEPURRET]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "ADVANCEPURRET]"
    '    cmd = New OleDbCommand(strsql, cn)
    '    cmd.ExecuteNonQuery()

    '    strsql = " CREATE TABLE TEMPTABLEDB..[TEMP" & SystemName & "ACCTRANADVANCERECEIPT] (ACCODE VARCHAR(250), BATCHNO_N VARCHAR(20))"
    '    cmd = New OleDbCommand(strsql, cn)
    '    cmd.ExecuteNonQuery()

    '    strsql = vbCrLf + " SELECT "
    '    strsql += vbCrLf + " CASE WHEN PAYMODE = 'CC' THEN 'CARD ' + AC.SHORTNAME + ' ' + CHQCARDNO + ' :' + LTRIM(SUM(AMOUNT)) ELSE "
    '    strsql += vbCrLf + " CASE WHEN PAYMODE = 'AA' THEN 'ADVANCE ' + 'REFER : ' + SUBSTRING(REFNO,1,3) + '-' + SUBSTRING(REFNO,4,10) + ' :' + LTRIM(SUM(AMOUNT)) ELSE "
    '    strsql += vbCrLf + " CASE WHEN PAYMODE = 'CA' THEN 'CASH ' + ' :' + LTRIM(SUM(AMOUNT)) ELSE "
    '    strsql += vbCrLf + " CASE WHEN PAYMODE = 'PU' THEN 'PURCHASE ' + ' :' + LTRIM(SUM(AMOUNT)) ELSE"
    '    strsql += vbCrLf + " CASE WHEN PAYMODE = 'SR' THEN 'SALES RETURN ' + ' :' + LTRIM(SUM(AMOUNT)) ELSE "
    '    strsql += vbCrLf + " CASE WHEN PAYMODE = 'AP' AND SUBSTRING(REFNO,1,1) = 'A' THEN 'ADVANCE REPAY ' + ' :' + LTRIM(SUM(AMOUNT)) ELSE "
    '    strsql += vbCrLf + " CASE WHEN PAYMODE = 'AP' AND SUBSTRING(REFNO,1,1) = 'O' THEN 'ORDER REPAY ' + ' :' + LTRIM(SUM(AMOUNT)) ELSE "
    '    strsql += vbCrLf + " CASE WHEN PAYMODE = 'RV' THEN 'RETURN ' + ' :' + LTRIM(SUM(AMOUNT)) ELSE "
    '    strsql += vbCrLf + " 'CHEQUE ' + CHQCARDREF + ' ' + CHQCARDNO + ' :' + LTRIM(SUM(AMOUNT))" 'CH
    '    strsql += vbCrLf + " END END END END END END END END AS AMT "
    '    strsql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN T"
    '    ' strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CREDITCARD AC ON AC.ACCODE=T.ACCODE "
    '    strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CREDITCARD AC ON AC.CARDCODE = T.CARDID "
    '    strsql += vbCrLf + " WHERE "
    '    strsql += vbCrLf + " TRANDATE = '" & _Trandate & "' " 'Newly Add
    '    strsql += vbCrLf + " AND BATCHNO = '" & pBatchno & "' "
    '    strsql += vbCrLf + " AND PAYMODE IN ('AA','CC','CH','CA','PU','SR','RV','AP')"
    '    strsql += vbCrLf + " GROUP BY PAYMODE,CHQCARDNO,BATCHNO,AC.SHORTNAME,CHQCARDREF,REFNO,SYSTEMID"
    '    cmd = New OleDbCommand(strsql, cn)
    '    cmd.ExecuteNonQuery()
    '    da = New OleDbDataAdapter(strsql, cn)
    '    Dim dt As New DataTable
    '    da.Fill(dt)
    '    Dim TempAcc As String = ""
    '    Dim TempBatch As String = ""
    '    If dt.Rows.Count > 0 Then
    '        For i As Integer = 0 To dt.Rows.Count - 1
    '            TempAcc += "," & dt.Rows(i).Item("AMT").ToString
    '        Next
    '        'TempBatch = dt.Rows(0).Item("BATCHNO").ToString
    '        TempBatch = ""
    '        TempAcc = TempAcc.Trim(",")
    '        strsql = "INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ACCTRANADVANCERECEIPT](ACCODE,BATCHNO_N) VALUES "
    '        strsql += " ('" & TempAcc & "', '" & TempBatch & "')"
    '        cmd = New OleDbCommand(strsql, cn)
    '        cmd.ExecuteNonQuery()
    '    End If
    '    strsql = " SELECT * INTO TEMPTABLEDB..[TEMP" & SystemName & "ADVANCERECEIPT] FROM( "
    '    strsql += vbCrLf + " SELECT NULL AS PAYMODE,"
    '    strsql += vbCrLf + " CASE WHEN PAYMODE = 'AR' THEN 'Receipt No ' ELSE "
    '    strsql += vbCrLf + " CASE WHEN PAYMODE = 'RE' THEN 'Due No ' ELSE CASE WHEN PAYMODE = 'MP' THEN 'Payment No ' ELSE 'Voucher No ' END END"
    '    strsql += vbCrLf + " END AS TranName,"
    '    strsql += vbCrLf + " CASE WHEN PAYMODE = 'AR' THEN 'ADVANCE  RECEIPT ' ELSE "
    '    strsql += vbCrLf + " CASE WHEN PAYMODE = 'RE' THEN 'DUE  RECEIPT ' ELSE CASE WHEN PAYMODE = 'MP' THEN 'OTHER PAYMENT' ELSE 'RECEIPT  VOUCHER' END END"
    '    strsql += vbCrLf + " END AS Field1,"
    '    strsql += vbCrLf + " CASE WHEN PAYMODE = 'AR' THEN 'Received with thanks from ' ELSE "
    '    strsql += vbCrLf + " CASE WHEN PAYMODE = 'RE' THEN 'Credit bill ' ELSE "
    '    strsql += vbCrLf + " CASE WHEN PAYMODE = 'DR' THEN  'Received with thanks from ' ELSE CASE WHEN PAYMODE = 'MP' THEN 'Payment Bill ' ELSE 'Receipt Bill ' END   "
    '    strsql += vbCrLf + " END END END + "
    '    strsql += vbCrLf + " (SELECT (SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = C.PSNO )"
    '    strsql += vbCrLf + " FROM " & cnAdminDb & "..CUSTOMERINFO AS C "
    '    strsql += vbCrLf + " WHERE BATCHNO = o.BATCHNO) + ' Rs.' + CONVERT(VARCHAR,AMOUNT+ISNULL(GSTVAL,0)) Field2 " ' Newly Add ISNULL(GSTVAL,0)
    '    strsql += vbCrLf + " ,(AMOUNT+ISNULL(GSTVAL,0))AMOUNT " 'NEWLY ADD
    '    strsql += vbCrLf + " , NULL AMT "
    '    'strSql += vbCrLf + ", 'Refer No :' + CONVERT(VARCHAR, SUBSTRING(O.RUNNO,6,20)) as  Field3"
    '    strsql += vbCrLf + " , CASE WHEN PAYMODE = 'AR' THEN 'Towards ADVANCE' ELSE "
    '    strsql += vbCrLf + " CASE WHEN PAYMODE = 'RE' THEN 'Towards CREDIT' ELSE "
    '    strsql += vbCrLf + " CASE WHEN PAYMODE = 'MP' THEN 'Towards PAYMENT' ELSE 'Towards RECEIPT' "
    '    strsql += vbCrLf + " END END END + ' Refer No :' + CONVERT(VARCHAR, SUBSTRING(O.RUNNO,6,20)) AS Field5 ,"
    '    strsql += vbCrLf + " BATCHNO AS BATCHNO_N, TRANNO,"
    '    strsql += vbCrLf + " CONVERT(VARCHAR(15),TRANDATE,103) + SUBSTRING(CONVERT(VARCHAR,UPTIME,109),13,5)+SUBSTRING(CONVERT(VARCHAR,UPTIME,109),25,5) AS BILLDATE, "
    '    strsql += vbCrLf + " 'C' AS TRANMODE,0 AS ISTAG,0 AS ISOG"
    '    strsql += vbCrLf + " ,SYSTEMID AS NODE"
    '    strsql += vbCrLf + " ,ISNULL ((SELECT "
    '    strsql += vbCrLf + " (SELECT TOP 1 SHORTNAME FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID =C.PURITYID) "
    '    strsql += vbCrLf + " FROM " & cnAdminDb & "..CATEGORY AS C WHERE CATCODE = O.CATCODE), '') CATCODETYPE"
    '    strsql += vbCrLf + " ,LTRIM(REMARK1)+LTRIM(REMARK2) AS REMARKS "
    '    strsql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS O "
    '    strsql += vbCrLf + " WHERE TRANDATE = '" & _Trandate & "' " 'Newly Add
    '    strsql += vbCrLf + " AND  BATCHNO = '" & pBatchno & "'"
    '    strsql += vbCrLf + " )X"
    '    cmd = New OleDbCommand(strsql, cn)
    '    cmd.ExecuteNonQuery()

    '    strsql = vbCrLf + " SELECT  "
    '    strsql += vbCrLf + " (SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS PNAME,  "
    '    strsql += vbCrLf + " (SELECT (DOORNO + ' ' + ADDRESS1) FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS ADDRESS1,"
    '    strsql += vbCrLf + " (SELECT (ADDRESS2 + ' ' + ADDRESS3 + ' ' + CITY + ' ' + PINCODE ) FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS ADDRESS2,"
    '    strsql += vbCrLf + " (SELECT CASE WHEN MOBILE<> '' THEN MOBILE ELSE PHONERES END FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS MOBILENO,"
    '    strsql += vbCrLf + " (SELECT PAN  FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS TIN,"
    '    strsql += vbCrLf + " BATCHNO AS BATCHNO_N,"
    '    strsql += vbCrLf + " (SELECT ISNULL(GSTNO,'')GSTNO  FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS GSTNO, "
    '    strsql += vbCrLf + " ISNULL(P.PAN,'') AS PANNO "
    '    strsql += vbCrLf + " INTO TEMPTABLEDB..[TEMP" & SystemName & "CUSTADVANCERECEIPT] "
    '    strsql += vbCrLf + " FROM " & cnAdminDb & "..CUSTOMERINFO AS P "
    '    strsql += vbCrLf + " WHERE BATCHNO = '" & pBatchno & "'"
    '    cmd = New OleDbCommand(strsql, cn)
    '    cmd.ExecuteNonQuery()

    '    strsql = "SELECT DISTINCT BATCHNO FROM " & cnAdminDb & "..ITEMTAG T WHERE BATCHNO='" & pBatchno & "'  " ' DISTINCT NOT USED ORDER BY ITEMID 
    '    da = New OleDbDataAdapter(strsql, cn)
    '    Dim DtAdvRecor As New DataTable
    '    da.Fill(DtAdvRecor)
    '    If DtAdvRecor.Rows.Count > 0 Then            '
    '        strsql = "SELECT ITEMID,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)ITEMNAME"
    '        strsql += vbCrLf + " , GRSWT AS WEIGHT"
    '        strsql += vbCrLf + " , (SELECT TOP 1 RATE FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO=T.BATCHNO) AS RATE"
    '        strsql += vbCrLf + " , (SELECT TOP 1 CASE WHEN RATE = 0 THEN 'RATE NOT FIXED' ELSE 'RATE FIXED' END FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO=T.BATCHNO) AS RATENAME"
    '        strsql += vbCrLf + "  INTO TEMPTABLEDB..[TEMP" & SystemName & "ITEMTAG] "
    '        strsql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T WHERE BATCHNO='" & pBatchno & "' ORDER BY ITEMID"
    '        cmd = New OleDbCommand(strsql, cn)
    '        cmd.ExecuteNonQuery()
    '    Else
    '        strsql = "CREATE TABLE TEMPTABLEDB..[TEMP" & SystemName & "ITEMTAG](ITEMID INT, ITEMNAME VARCHAR(50), WEIGHT INT, RATE VARCHAR(15), RATENAME VARCHAR(50)) "
    '        cmd = New OleDbCommand(strsql, cn)
    '        cmd.ExecuteNonQuery()
    '        strsql = " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ITEMTAG] (ITEMID,ITEMNAME,WEIGHT,RATE,RATENAME) "
    '        strsql += vbCrLf + " SELECT 0 ,NULL,0 "
    '        strsql += vbCrLf + " ,(SELECT TOP 1 RATE FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO= '" & pBatchno & "' ) AS RATE"
    '        strsql += vbCrLf + " ,(SELECT TOP 1 CASE WHEN RATE = 0 THEN 'RATE NOT FIXED' ELSE 'RATE FIXED' END FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO= '" & pBatchno & "' ) AS RATENAME"
    '        cmd = New OleDbCommand(strsql, cn)
    '        cmd.ExecuteNonQuery()
    '    End If
    '    strsql = vbCrLf + "  SELECT "
    '    strsql += vbCrLf + "  NULL AS SRNO,"
    '    strsql += vbCrLf + "  NULL AS TRANNO,"
    '    strsql += vbCrLf + "  CONVERT(VARCHAR(15),I.TRANDATE,103) AS BILLDATE,"
    '    strsql += vbCrLf + "  BATCHNO AS BATCHNO_N,"
    '    strsql += vbCrLf + "  'TOTAL' ITEMNAME"
    '    strsql += vbCrLf + "  ,SUM(I.PCS) AS QTY,SUM(I.GRSWT)GRSWT,SUM(I.NETWT)NETWT,SUM(I.LESSWT) LESSWT,SUM(I.WASTAGE) WASTAGE"
    '    strsql += vbCrLf + "  ,CONVERT(VARCHAR,NULL)RATE, CONVERT(VARCHAR, SUM(I.MCHARGE)) MCHARGE,0 TAX,SUM(I.AMOUNT + I.TAX) AMOUNT "
    '    strsql += vbCrLf + "  ,'ZZZZZZZZ' SNO,3 AS RESULT,2 AS TYPE,'G' AS COLHEAD"
    '    strsql += vbCrLf + "  INTO TEMPTABLEDB..[TEMP" & SystemName & "ADVANCEPURRET]"
    '    strsql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS I "
    '    strsql += vbCrLf + "  WHERE I.TRANDATE = '" & _Trandate & "' "
    '    strsql += vbCrLf + "  AND I.BATCHNO='" & pBatchno & "' "
    '    strsql += vbCrLf + "  AND I.TRANTYPE IN ('PU','SR')"
    '    strsql += vbCrLf + "  GROUP BY I.TRANDATE,I.BATCHNO  " ' ,I.TRANNO
    '    cmd = New OleDbCommand(strsql, cn)
    '    cmd.ExecuteNonQuery()

    '    strsql = "SELECT COUNT(*)CNT FROM TEMPTABLEDB..[TEMP" & SystemName & "ITEMTAG] "
    '    If Val(GetSqlValue_Bill(strsql, "CNT", 0)) > 0 Then
    '        strsql = " UPDATE TEMPTABLEDB..[TEMP" & SystemName & "ADVANCERECEIPT] SET ISTAG=1"
    '        cmd = New OleDbCommand(strsql, cn)
    '        cmd.ExecuteNonQuery()
    '    End If
    '    strsql = " SELECT COUNT(*)CNT FROM " & cnStockDb & "..RECEIPT "
    '    strsql += vbCrLf + " WHERE TRANDATE = '" & _Trandate & "' "
    '    strsql += vbCrLf + " AND  BATCHNO='" & pBatchno & "' "
    '    strsql += vbCrLf + " AND TRANTYPE IN ('PU','SR')"
    '    If Val(GetSqlValue_Bill(strsql, "CNT", 0)) > 0 Then
    '        strsql = " UPDATE TEMPTABLEDB..[TEMP" & SystemName & "ADVANCERECEIPT] SET ISOG=1"
    '        cmd = New OleDbCommand(strsql, cn)
    '        cmd.ExecuteNonQuery()
    '    End If

    '    strsql = vbCrLf + " SELECT TRANMODE, SUM(CGST) CGST,SUM(SGST)SGST, SUM(IGST)IGST FROM "
    '    strsql += vbCrLf + " ("
    '    strsql += vbCrLf + " SELECT TRANMODE,AMOUNT AS CGST, 0 AS SGST, 0 AS IGST "
    '    strsql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN WHERE "
    '    strsql += vbCrLf + " TRANDATE = '" & _Trandate & "' "
    '    strsql += vbCrLf + " AND BATCHNO = '" & pBatchno & "' "
    '    strsql += vbCrLf + " AND PAYMODE IN ('SV') AND ACCODE = 'CGST' "
    '    strsql += vbCrLf + " UNION ALL"
    '    strsql += vbCrLf + " SELECT TRANMODE,0 CGST,AMOUNT AS SGST, 0 AS IGST"
    '    strsql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN WHERE "
    '    strsql += vbCrLf + " TRANDATE = '" & _Trandate & "' "
    '    strsql += vbCrLf + " AND BATCHNO = '" & pBatchno & "' "
    '    strsql += vbCrLf + " AND PAYMODE IN ('SV') AND ACCODE = 'SGST'"
    '    strsql += vbCrLf + " UNION ALL"
    '    strsql += vbCrLf + " SELECT 'IGST' TRANMODE,0 CGST, 0 AS SGST, AMOUNT AS IGST "
    '    strsql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN WHERE "
    '    strsql += vbCrLf + " TRANDATE = '" & _Trandate & "' "
    '    strsql += vbCrLf + " AND BATCHNO = '" & pBatchno & "' "
    '    strsql += vbCrLf + " AND PAYMODE IN ('SV') AND ACCODE = 'IGST' "
    '    strsql += vbCrLf + " )X"
    '    strsql += vbCrLf + " GROUP BY TRANMODE "
    '    da = New OleDbDataAdapter(strsql, cn)
    '    da.Fill(dsAdvanceReceipt, "GSTTRAN")


    '    strsql = " SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "ADVANCERECEIPT]"
    '    da = New OleDbDataAdapter(strsql, cn)
    '    da.Fill(dsAdvanceReceipt, "OUTSTANDING")

    '    strsql = " SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "CUSTADVANCERECEIPT]"
    '    da = New OleDbDataAdapter(strsql, cn)
    '    da.Fill(dsAdvanceReceipt, "CUSTOMER")

    '    strsql = " SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "ACCTRANADVANCERECEIPT]"
    '    da = New OleDbDataAdapter(strsql, cn)
    '    da.Fill(dsAdvanceReceipt, "ACCTRAN")

    '    strsql = " SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "ITEMTAG]"
    '    da = New OleDbDataAdapter(strsql, cn)
    '    da.Fill(dsAdvanceReceipt, "ITEMTAG")

    '    strsql = "SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "ADVANCEPURRET]"
    '    da = New OleDbDataAdapter(strsql, cn)
    '    da.Fill(dsAdvanceReceipt, "ISSUE")

    'End Sub

    Public Sub Print(ByVal pBatchno As String)
        strsql = " SELECT DISTINCT * FROM (SELECT DISTINCT TRANTYPE FROM " & cnStockDb & "..ISSUE "
        strsql += vbCrLf + " WHERE TRANDATE = '" & strBilldate & "' AND  BATCHNO='" & pBatchno & "' "
        strsql += vbCrLf + " UNION ALL "
        strsql += vbCrLf + " SELECT DISTINCT TRANTYPE FROM " & cnStockDb & "..RECEIPT "
        strsql += vbCrLf + " WHERE TRANDATE = '" & strBilldate & "' AND BATCHNO='" & pBatchno & "' "
        strsql += vbCrLf + " ) X "
        cmd = New OleDbCommand(strsql, cn)
        Dim dtPrint As New DataTable
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtPrint)
        If dtPrint.Rows.Count > 0 Then
            For k As Integer = 0 To dtPrint.Rows.Count - 1
                If dtPrint.Rows(k).Item("TRANTYPE").ToString = "SA" _
                Or dtPrint.Rows(k).Item("TRANTYPE").ToString = "PU" _
                Or dtPrint.Rows(k).Item("TRANTYPE").ToString = "SR" _
                Or dtPrint.Rows(k).Item("TRANTYPE").ToString = "MI" _
                Or dtPrint.Rows(k).Item("TRANTYPE").ToString = "RD" _
                Or dtPrint.Rows(k).Item("TRANTYPE").ToString = "OD" _
                Then
                    TrantypeMI = dtPrint.Rows(k).Item("TRANTYPE").ToString()

                    PrintSALEINVOICE(pBatchno, dtPrint.Rows(k).Item("TRANTYPE").ToString())

                    PrintDialog1.Document = PrtDoc
                    PrtDoc.PrinterSettings.Copies = 1
                    PrtDoc.PrintController = New System.Drawing.Printing.StandardPrintController
                    PrtDoc.Print()
                    Exit Sub
                End If
            Next
        End If
    End Sub

    Private Sub PrtDoc_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrtDoc.PrintPage
        Try
            Dim NoofPage As Integer = 0
            Dim rAlign As New StringFormat
            rAlign.Alignment = StringAlignment.Far
            NoofPrint += 1
            Using g As Graphics = e.Graphics
                g.DrawString("GSTIN : " & GSTNO, fontRegular, Brushes.Black, 336, TopY)
                g.DrawString("GSTIN : " & GSTNO, fontRegular, Brushes.Black, 336, BottomY)
                g.DrawString("STATE CODE : " & Mid(GSTNO, 1, 2) & "", fontRegular, Brushes.Black, 360, TopY + 18)
                g.DrawString("STATE CODE : " & Mid(GSTNO, 1, 2) & "", fontRegular, Brushes.Black, 360, BottomY + 18)
                CustomerInfo(Tranno, prtBilldate, e.Graphics, e)
                Title(e.Graphics, e)
                RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
                If dtSales.Rows.Count > 0 Then
                    For PagecountSale = PagecountSale To dtSales.Rows.Count - 1
                        With dtSales.Rows(PagecountSale)
                            ''Top '/************************************////
                            NoofPage += 1
                            If .Item("RESULT").ToString = "3.40" Then
                                g.DrawLine(Pens.Silver, 710, (TopY - 2), c9, (TopY - 2))
                                g.DrawLine(Pens.Silver, 710, (BottomY - 2), c9, (BottomY - 2))
                            End If
                            If .Item("RESULT").ToString = "3.0" And .Item("TYPE").ToString = "11.0" And .Item("COLHEAD").ToString = "G" Then
                                g.DrawLine(Pens.Silver, 340, (TopY - 2), c9, (TopY - 2))
                                g.DrawLine(Pens.Silver, 340, (BottomY - 2), c9, (BottomY - 2))
                            End If
                            g.DrawString(.Item("SRNO").ToString(), fontRegular, BlackBrush, c1, TopY, LeftFormat)
                            If .Item("TYPE").ToString = "10" And .Item("COLHEAD").ToString = "G" Then
                                If .Item("ITEMNAME").ToString.Length > 25 Then
                                    g.DrawString(.Item("ITEMNAME").ToString(), fontBoldsmall, BlackBrush, c3 - 20, TopY, LeftFormat)
                                Else
                                    g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c3, TopY, LeftFormat)
                                End If
                            Else
                                If .Item("CALTYPE").ToString = "HA" And .Item("RESULT").ToString = "1.1" Then
                                    g.DrawString(.Item("ITEMNAME").ToString(), fontRegular6, BlackBrush, c2, TopY, LeftFormat)
                                Else
                                    g.DrawString(.Item("ITEMNAME").ToString(), fontRegular, BlackBrush, c2, TopY, LeftFormat)
                                End If
                            End If
                            g.DrawString(.Item("HSN").ToString(), fontRegular, BlackBrush, h1, TopY, LeftFormat)
                            g.DrawString(IIf(Val(.Item("QTY").ToString()) <> 0, Val(.Item("QTY").ToString()), ""), fontRegular, BlackBrush, c3, TopY, RightFormat)
                            If .Item("CALTYPE").ToString = "F" Then 'Fixed Item Should Not print GrossWt
                                g.DrawString("", fontRegular, BlackBrush, c4, TopY, RightFormat)
                                g.DrawString("", fontRegular, BlackBrush, c5, TopY, RightFormat)
                            Else
                                g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontRegular, BlackBrush, c4, TopY, RightFormat)
                                g.DrawString(IIf(Val(.Item("LESSWT").ToString()) <> 0, .Item("LESSWT").ToString(), ""), fontRegular, BlackBrush, c5, TopY, RightFormat)
                            End If
                            g.DrawString(IIf(Val(.Item("WASTAGE").ToString()) <> 0, .Item("WASTAGE").ToString, ""), fontRegular, BlackBrush, c6, TopY, RightFormat)
                            If .Item("COLHEAD").ToString() = "G" Or .Item("COLHEAD").ToString = "P" Then
                                If .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString <> "10" Then
                                    g.DrawString(.Item("RATE").ToString(), fontRegular, BlackBrush, 550, TopY, LeftFormat)
                                ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString <> "3.50") Then
                                    g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, TopY, LeftFormat)
                                ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString = "3.50") Then
                                    g.DrawString(.Item("RATE").ToString(), fontRegular, BlackBrush, 550, TopY, LeftFormat)
                                ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10" Then
                                    g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, TopY, LeftFormat)
                                End If
                            Else
                                g.DrawString(.Item("RATE").ToString(), fontRegular, BlackBrush, c7, TopY, RightFormat)
                            End If
                            If .Item("CALTYPE").ToString = "F" Then 'Fixed Item Should Not print mC
                                g.DrawString("", fontRegular, BlackBrush, c8, TopY, RightFormat)
                            Else
                                g.DrawString(IIf(.Item("MCHARGE").ToString() <> "0.00", .Item("MCHARGE").ToString(), ""), fontRegular, BlackBrush, c8, TopY, RightFormat)
                            End If
                            If .Item("COLHEAD").ToString = "G" Or .Item("COLHEAD").ToString = "P" Then
                                If .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString <> "10" Then
                                    g.DrawString(.Item("AMOUNT").ToString(), fontRegular, BlackBrush, c9, TopY, rAlign)
                                ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString <> "3.50") Or (.Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10") Then
                                    g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c9, TopY, rAlign)
                                    If .Item("RESULT").ToString = "3.6" And .Item("TYPE").ToString = "11" Then
                                        Dim AmtinWord As String = ConvertRupees.RupeesToWord(Val(.Item("AMOUNT").ToString().Trim("-", ""))) '.Trim("-", "")
                                        g.DrawString(AmtinWord, fontAmtInWord, BlackBrush, c2, TopY)
                                    End If
                                ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString = "3.50") Then
                                    g.DrawString(.Item("AMOUNT").ToString(), fontRegular, BlackBrush, c9, TopY, rAlign)
                                ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10" Then
                                    g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c9, TopY, rAlign)
                                End If
                            Else
                                g.DrawString(.Item("AMOUNT").ToString(), fontRegular, BlackBrush, c9, TopY, rAlign)
                            End If
                            ''BOTTOM  '/************************************////
                            g.DrawString(.Item("SRNO").ToString(), fontRegular, BlackBrush, c1, BottomY, LeftFormat)
                            If (.Item("TYPE").ToString = "10" And .Item("COLHEAD").ToString = "G") Then
                                If .Item("ITEMNAME").ToString.Length > 25 Then
                                    g.DrawString(.Item("ITEMNAME").ToString(), fontBoldsmall, BlackBrush, c3 - 20, BottomY, LeftFormat)
                                Else
                                    g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c3, BottomY, LeftFormat)
                                End If
                            Else
                                If .Item("CALTYPE").ToString = "HA" And .Item("RESULT").ToString = "1.1" Then
                                    g.DrawString(.Item("ITEMNAME").ToString(), fontRegular6, BlackBrush, c2, BottomY, LeftFormat)
                                Else
                                    g.DrawString(.Item("ITEMNAME").ToString(), fontRegular, BlackBrush, c2, BottomY, LeftFormat)
                                End If
                            End If
                            g.DrawString(.Item("HSN").ToString(), fontRegular, BlackBrush, h1, BottomY, LeftFormat)
                            g.DrawString(IIf(Val(.Item("QTY").ToString()) <> 0, .Item("QTY").ToString(), ""), fontRegular, BlackBrush, c3, BottomY, RightFormat)
                            If .Item("CALTYPE").ToString = "F" Then 'Fixed Item should not grswt and netwt
                                g.DrawString("", fontRegular, BlackBrush, c4, BottomY, RightFormat)
                                g.DrawString("", fontRegular, BlackBrush, c5, BottomY, RightFormat)
                            Else
                                g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontRegular, BlackBrush, c4, BottomY, RightFormat)
                                g.DrawString(IIf(Val(.Item("LESSWT").ToString()) <> 0, .Item("LESSWT").ToString(), ""), fontRegular, BlackBrush, c5, BottomY, RightFormat)
                            End If
                            g.DrawString(IIf(Val(.Item("WASTAGE").ToString()) <> 0, .Item("WASTAGE").ToString(), ""), fontRegular, BlackBrush, c6, BottomY, RightFormat)
                            If .Item("COLHEAD").ToString = "G" Or .Item("COLHEAD").ToString = "P" Then
                                If .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString <> "10" Then
                                    g.DrawString(.Item("RATE").ToString(), fontRegular, BlackBrush, 550, BottomY, LeftFormat)
                                ElseIf .Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString <> "3.50" Then
                                    g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, BottomY, LeftFormat)
                                ElseIf .Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString = "3.50" Then
                                    g.DrawString(.Item("RATE").ToString(), fontRegular, BlackBrush, 550, BottomY, LeftFormat)
                                ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10" Then
                                    g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, BottomY, LeftFormat)
                                End If
                            Else
                                g.DrawString(.Item("RATE").ToString(), fontRegular, BlackBrush, c7, BottomY, RightFormat)
                            End If
                            If .Item("CALTYPE").ToString = "F" Then 'Fixed Item Should display Mc
                                g.DrawString("", fontRegular, BlackBrush, c8, BottomY, RightFormat)
                            Else
                                g.DrawString(IIf(.Item("MCHARGE").ToString() <> "0.00", .Item("MCHARGE").ToString(), ""), fontRegular, BlackBrush, c8, BottomY, RightFormat)
                            End If
                            If .Item("COLHEAD").ToString = "G" Or .Item("COLHEAD").ToString = "P" Then
                                If .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString <> "10" Then
                                    g.DrawString(.Item("AMOUNT").ToString(), fontRegular, BlackBrush, c9, BottomY, rAlign)
                                ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString <> "3.50") Or (.Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10") Then
                                    g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c9, BottomY, rAlign)
                                    If .Item("RESULT").ToString = "3.6" And .Item("TYPE").ToString = "11" Then
                                        Dim AmtinWord As String = ConvertRupees.RupeesToWord(Val(.Item("AMOUNT").ToString().Trim("-", "")))
                                        g.DrawString(AmtinWord, fontAmtInWord, BlackBrush, c2, BottomY)
                                    End If
                                ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString = "3.50") Then
                                    g.DrawString(.Item("AMOUNT").ToString(), fontRegular, BlackBrush, c9, BottomY, rAlign)
                                ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10" Then
                                    g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c9, BottomY, rAlign)
                                End If
                            Else
                                g.DrawString(.Item("AMOUNT").ToString(), fontRegular, BlackBrush, c9, BottomY, rAlign)
                            End If
                            If (.Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" And Val(.Item("QTY").ToString) >= 2) _
                            Or (.Item("TYPE").ToString = "2" And .Item("COLHEAD").ToString = "G") _
                            Or (.Item("TYPE").ToString = "3" And .Item("COLHEAD").ToString = "G") Then
                                g.DrawLine(Pens.Silver, 340, (TopY + 2), c9, (TopY + 2))
                                g.DrawLine(Pens.Silver, 340, (BottomY + 2), c9, (BottomY + 2))
                            End If
                            If TopY >= 489 Then
                                PagecountSale = PagecountSale + 1
                                TopY = 135 ' TOP AGAIN STARTING POSITION
                                BottomY = 705 ' BOTTOM AGAIN STATRING POSITION
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, 513)
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, 1075)
                                e.HasMorePages = True
                                Exit For
                            End If
                            'New Line Start
                            If .Item("CALTYPE").ToString = "HA" And .Item("RESULT").ToString = "1.1" Then
                                TopY = TopY + 8
                                BottomY = BottomY + 8
                            Else
                                TopY = TopY + 17
                                BottomY = BottomY + 17
                            End If
                            If .Item("RESULT").ToString = "1.0" And (.Item("CALTYPE").ToString = "F" Or .Item("CALTYPE").ToString = "R") Then '.Item("COLHEAD").ToString <> "G" And .Item("COLHEAD").ToString <> "P" And .Item("COLHEAD").ToString <> "T"
                                'If Val(.Item("GRSWT").ToString) = 0 Then
                                g.DrawString("PCS COST ITEM NO EXCHANGE", fontRegularsmall, BlackBrush, c2, TopY, LeftFormat)
                                g.DrawString("PCS COST ITEM NO EXCHANGE", fontRegularsmall, BlackBrush, c2, BottomY, LeftFormat)
                                TopY = TopY + 17
                                BottomY = BottomY + 17
                                'End If
                            End If
                            If .Item("RESULT").ToString = "1.0" And (.Item("BILLNAME").ToString = "Y") Then '.Item("COLHEAD").ToString <> "G" And .Item("COLHEAD").ToString <> "P" And .Item("COLHEAD").ToString <> "T"
                                g.DrawString("Fixing of Kemp, Kundan Stones needs wax, Hence at the time of melting weight loss will be there upto 10%..", fontRegularsmall, BlackBrush, c2, TopY, LeftFormat)
                                g.DrawString("Fixing of Kemp, Kundan Stones needs wax, Hence at the time of melting weight loss will be there upto 10%..", fontRegularsmall, BlackBrush, c2, BottomY, LeftFormat)
                                TopY = TopY + 17
                                BottomY = BottomY + 17
                                'End If
                            End If
                            If TopY >= 489 Then
                                PagecountSale = PagecountSale + 1
                                TopY = 135 ' TOP AGAIN STARTING POSITION
                                BottomY = 705 ' BOTTOM AGAIN STATRING POSITION
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, 513)
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, 1075)
                                e.HasMorePages = True
                                Exit For
                            End If
                            If (.Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" And Val(.Item("QTY").ToString) >= 2) Or
                            (.Item("TYPE").ToString = "3" And .Item("COLHEAD").ToString = "G") Then
                                g.DrawLine(Pens.Silver, 340, (TopY - 2), c9, (TopY - 2))
                                g.DrawLine(Pens.Silver, 340, (BottomY - 2), c9, (BottomY - 2))
                            End If
                            If (.Item("RESULT").ToString = "3.10" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G") Then
                                g.DrawLine(Pens.Silver, 710, (TopY - 2), c9, (TopY - 2))
                                g.DrawLine(Pens.Silver, 710, (BottomY - 2), c9, (BottomY - 2))
                            End If
                            If (.Item("TYPE").ToString = "4" And .Item("COLHEAD").ToString = "T") Then
                                If Val(.Item("AMOUNT").ToString) <> 0 Then
                                    g.DrawLine(Pens.Silver, c8, TopY, c9, TopY)
                                    g.DrawLine(Pens.Silver, c8, BottomY, c9, BottomY)
                                End If
                            End If
                            If .Item("RESULT").ToString = "3.0" And .Item("TYPE").ToString = "11.0" And .Item("COLHEAD").ToString = "G" Then
                                g.DrawLine(Pens.Silver, 340, (TopY - 2), c9, (TopY - 2))
                                g.DrawLine(Pens.Silver, 340, (BottomY - 2), c9, (BottomY - 2))
                            End If
                            'If NoofPage > 14 Then
                            If TopY >= 489 Then
                                PagecountSale = PagecountSale + 1
                                TopY = 135 ' TOP AGAIN STARTING POSITION
                                BottomY = 705 ' BOTTOM AGAIN STATRING POSITION
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, 513)
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, 1075)
                                e.HasMorePages = True
                                Exit For
                            End If
                        End With
                    Next
                End If
                Footer(e.Graphics, e)
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            Exit Sub
        End Try
    End Sub
    Public Function NewPage(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal topy As Integer) As Integer
        If topy > 489 Then
            PagecountSale = PagecountSale + 1
            topy = 135 ' TOP AGAIN STARTING POSITION
            BottomY = 705 ' BOTTOM AGAIN STATRING POSITION
            g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, 513)
            g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, 1075)
            e.HasMorePages = True
            Return 1
        End If
    End Function
#Region "SALE INVOICE"
    Public Sub PrintSALEINVOICE(ByVal pBatchno As String, ByVal Trantype As String)
        If Trantype = "MI" Then
            strsql = " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "SALEINVOICE]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "SALEINVOICE]"
            strsql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "SACUSTOMER]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "SACUSTOMER]"
            strsql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "SAACCTRAN]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "SAACCTRAN]"
            strsql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "SAPAYMODE]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "SAPAYMODE]"
            strsql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "SATRANMODE]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "SATRANMODE]"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
            'strSql = vbCrLf + " /* MISCISSUE DETAILS */"
            strsql = vbCrLf + " SELECT *,NULL ISSNO,NULL BILLNAME ,IDENTITY(INT,1,1) PGNO "
            strsql += vbCrLf + " INTO TEMPTABLEDB..[TEMP" & SystemName & "SALEINVOICE] "
            strsql += vbCrLf + " FROM ( "
            strsql += vbCrLf + " SELECT ROW_NUMBER() OVER(ORDER BY TRANNO) AS SRNO"
            strsql += vbCrLf + " ,TRANNO"
            strsql += vbCrLf + " ,CONVERT(VARCHAR(15), I.TRANDATE,103) AS BILLDATE"
            strsql += vbCrLf + " ,BATCHNO AS BATCHNO_N"
            strsql += vbCrLf + " ,(CASE WHEN ISNULL(SM.HSN,'''') = '''' THEN IM.HSN ELSE SM.HSN END) HSN "
            strsql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) AS ITEMNAME"
            strsql += vbCrLf + " ,SUM(PCS) AS QTY"
            strsql += vbCrLf + " ,SUM(I.GRSWT) AS GRSWT"
            strsql += vbCrLf + " ,SUM(I.NETWT) AS NETWT"
            strsql += vbCrLf + " ,NULL AS LESSWT"
            strsql += vbCrLf + " ,NULL AS WASTAGE"
            strsql += vbCrLf + " ,NULL AS RATE"
            strsql += vbCrLf + " ,NULL MCHARGE"
            strsql += vbCrLf + " ,NULL TAX"
            strsql += vbCrLf + " ,CASE WHEN ISNULL(SALEMODE,'') <> 'W' THEN SUM(AMOUNT) ELSE NULL END AS AMOUNT" 'NULL 'SUM(I.GRSWT) = 0
            strsql += vbCrLf + " ,NULL SNO "
            strsql += vbCrLf + " /*,SUM(AMOUNT) AS AMOUNT*/"
            strsql += vbCrLf + " ,1.0 AS RESULT , 11.0 AS TYPE, '' AS COLHEAD"
            strsql += vbCrLf + " ,CASE WHEN ISNULL(SM.CALTYPE,'') <> '' THEN ISNULL(SM.CALTYPE,'') ELSE IM.CALTYPE END CALTYPE  "
            strsql += vbCrLf + " ,ISNULL(BILLPREFIX,'') BILLPREFIX "
            strsql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            strsql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID=I.ITEMID "
            strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID=I.ITEMID AND SM.SUBITEMID=I.SUBITEMID "
            strsql += vbCrLf + " WHERE "
            strsql += vbCrLf + " TRANTYPE = 'MI' "
            strsql += vbCrLf + " AND TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & pBatchno & "'"
            strsql += vbCrLf + " GROUP BY I.CATCODE,TRANNO,BATCHNO,I.TRANDATE,IM.HSN,SM.HSN,SALEMODE "
            strsql += vbCrLf + " ,SM.CALTYPE,IM.CALTYPE,ISNULL(BILLPREFIX,'')"
            strsql += vbCrLf + "  /*ISSUE STONE MISCISSUE TOTAL*/"
            strsql += vbCrLf + "  UNION ALL "
            strsql += vbCrLf + " SELECT "
            strsql += vbCrLf + " NULL AS SRNO, "
            strsql += vbCrLf + " NULL TRANNO,"
            strsql += vbCrLf + " NULL AS BILLDATE,"
            strsql += vbCrLf + " BATCHNO AS BATCHNO_N"
            strsql += vbCrLf + " ,'' HSN "
            strsql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = ISS.STNITEMID) AS  ITEMNAME"
            strsql += vbCrLf + " ,SUM(ISS.STNPCS) AS QTY,SUM(ISS.STNWT)GRSWT,NULL NETWT"
            strsql += vbCrLf + " ,NULL LESSWT,NULL WASTAGE"
            strsql += vbCrLf + " ,CONVERT(VARCHAR,NULL)RATE,CONVERT(VARCHAR, NULL) MCHARGE"
            strsql += vbCrLf + " ,NULL TAX"
            strsql += vbCrLf + " ,NULL AMOUNT"
            strsql += vbCrLf + " ,NULL SNO,2.0 AS RESULT,11.0 AS TYPE,'G' AS COLHEAD"
            strsql += vbCrLf + " ,'' CALTYPE "
            strsql += vbCrLf + " ,'' BILLPREFIX "
            strsql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE AS ISS"
            strsql += vbCrLf + " WHERE ISSSNO IN( SELECT SNO FROM " & cnStockDb & "..ISSUE I"
            strsql += vbCrLf + " WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND TRANTYPE = 'MI' AND BATCHNO = '" & pBatchno & "')"
            strsql += vbCrLf + " GROUP BY BATCHNO,STNITEMID  "
            strsql += vbCrLf + "  /*REMARK MISCISSUE TOTAL*/"
            strsql += vbCrLf + "  UNION ALL "
            strsql += vbCrLf + "  SELECT "
            strsql += vbCrLf + "  NULL AS SRNO,"
            strsql += vbCrLf + "  NULL TRANNO,"
            strsql += vbCrLf + "  NULL AS BILLDATE,"
            strsql += vbCrLf + "  BATCHNO AS BATCHNO_N"
            strsql += vbCrLf + "  ,'' HSN "
            strsql += vbCrLf + "  ,'ISSUED FOR REPAIR AND POLISHING ' ITEMNAME"
            strsql += vbCrLf + "  ,NULL AS QTY,NULL GRSWT,NULL NETWT"
            strsql += vbCrLf + "  ,NULL LESSWT,NULL WASTAGE"
            strsql += vbCrLf + "  ,CONVERT(VARCHAR,NULL)RATE,CONVERT(VARCHAR, NULL) MCHARGE"
            strsql += vbCrLf + "  ,NULL TAX,NULL AMOUNT,NULL SNO,3 AS RESULT,11.0 AS TYPE"
            strsql += vbCrLf + "  ,'R' AS COLHEAD"
            strsql += vbCrLf + "  ,'' CALTYPE "
            strsql += vbCrLf + " ,'' BILLPREFIX "
            strsql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS I "
            strsql += vbCrLf + "  WHERE I.BATCHNO='" & pBatchno & "' "
            strsql += vbCrLf + "  AND I.TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + "  AND I.TRANTYPE IN ('MI')"
            strsql += vbCrLf + "  GROUP BY I.BATCHNO "
            strsql += vbCrLf + "  /*MISCISSUE TOTAL*/"
            strsql += vbCrLf + "  UNION ALL "
            strsql += vbCrLf + "  SELECT "
            strsql += vbCrLf + "  NULL AS SRNO,"
            strsql += vbCrLf + "  NULL TRANNO,"
            strsql += vbCrLf + "  NULL AS BILLDATE"
            strsql += vbCrLf + "  ,BATCHNO AS BATCHNO_N "
            strsql += vbCrLf + "  ,'' HSN "
            strsql += vbCrLf + "  ,'TOTAL : ' ITEMNAME"
            strsql += vbCrLf + "  ,SUM(I.PCS) AS QTY,SUM(I.GRSWT)GRSWT,SUM(I.NETWT)NETWT"
            strsql += vbCrLf + "  ,NULL LESSWT,NULL WASTAGE"
            strsql += vbCrLf + "  ,CONVERT(VARCHAR,NULL)RATE,CONVERT(VARCHAR, NULL) MCHARGE"
            strsql += vbCrLf + "  ,NULL TAX"
            strsql += vbCrLf + "  ,CASE WHEN SUM(I.GRSWT) = 0 THEN SUM(AMOUNT) ELSE NULL END AMOUNT"
            strsql += vbCrLf + "  ,NULL SNO "
            strsql += vbCrLf + "  ,3 AS RESULT "
            strsql += vbCrLf + "  ,11.0 AS TYPE,'G' AS COLHEAD"
            strsql += vbCrLf + "  ,'' CALTYPE "
            strsql += vbCrLf + " ,'' BILLPREFIX "
            strsql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS I "
            strsql += vbCrLf + "  WHERE "
            strsql += vbCrLf + "  I.TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + "  AND I.BATCHNO = '" & pBatchno & "' "
            strsql += vbCrLf + " AND I.TRANTYPE IN ('MI')"
            strsql += vbCrLf + "  GROUP BY I.BATCHNO HAVING COUNT(*) > 1 "
            strsql += vbCrLf + "  )X ORDER BY TYPE,SNO,RESULT"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        Else
            strsql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'DISCREMARK'"
            Dim DiscRemark As String = ""
            Dim DiscchitOffer As Double = 0
            Dim FinalAmt As Double = 0
            If DiscRemark = "" Then
                DiscRemark = GetSqlValue(cn, strsql)
            End If
            'strsql = "SELECT COUNT(BATCHNO) CNT FROM " & cnStockDb & "..CHITOFFER WHERE BATCHNO = '" & pBatchno & "'"
            'DiscchitOffer = Val(GetSqlValue(cn, strsql).ToString)
            'If DiscchitOffer > 0 Then
            '    DiscRemark = "SCHEME BENEFIT "
            'End If

            strsqlCheck = "SELECT ISNULL((SUM(FIN_DISCOUNT)+SUM(DISCOUNT)),0) DIST FROM " & cnStockDb & "..ISSUE "
            strsqlCheck += vbCrLf + " WHERE TRANDATE = '" & strBilldate & "' AND  BATCHNO = '" & pBatchno & "'"
            Dim DiscountAmt As Integer = GetSqlValue(cn, strsqlCheck)

            strsqlCheck = vbCrLf + " SELECT  ISNULL(SUM(IAMT),0) IAMT, ISNULL(SUM(RAMT),0)RAMT, SUM(IAMT)-SUM(RAMT) AS FINAMT FROM "
            strsqlCheck += vbCrLf + " ( "
            strsqlCheck += vbCrLf + " (SELECT SUM(AMOUNT + TAX) IAMT, 0 RAMT FROM " & cnStockDb & "..ISSUE WHERE "
            strsqlCheck += vbCrLf + " TRANDATE = '" & strBilldate & "' AND BATCHNO = '" & pBatchno & "') "
            strsqlCheck += vbCrLf + " UNION ALL "
            strsqlCheck += vbCrLf + " (SELECT 0 IAMT , SUM(AMOUNT + TAX) RMT   FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE = '" & strBilldate & "' "
            strsqlCheck += vbCrLf + " AND  BATCHNO = '" & pBatchno & "') "
            strsqlCheck += vbCrLf + " )A "
            Dim ro As DataRow = GetSqlRow(strsqlCheck, cn)
            If Val(ro.Item("IAMT").ToString) > 0 And Val(ro.Item("RAMT").ToString) > 0 Then
                FinalAmt = Val(ro.Item("FINAMT").ToString)
            End If
            'LTRIM(TAXPER)+' % '+LTRIM(isnull(TAXAMOUNT,0))
            'strsqlCheck = " SELECT ISNULL(SUM(TAXAMOUNT),0) AS CGST "
            'strsqlCheck += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            'strsqlCheck += vbCrLf + " AND BATCHNO = '" & pBatchno & "' AND TAXID = 'CG' "
            'strsqlCheck += vbCrLf + " AND TRANTYPE = 'SA' " 'Newly Add 23-10-2017
            'Dim Cgst As Double = GetSqlValue(cn, strsqlCheck)
            ''LTRIM(TAXPER)+' % '+LTRIM(isnull(TAXAMOUNT,0))
            'strsqlCheck = " SELECT ISNULL(SUM(TAXAMOUNT),0) AS SGST FROM " & cnStockDb & "..TAXTRAN "
            'strsqlCheck += vbCrLf + " WHERE TRANDATE = '" & strBilldate & "' "
            'strsqlCheck += vbCrLf + " AND BATCHNO = '" & pBatchno & "' AND TAXID = 'SG' "
            'strsqlCheck += vbCrLf + " AND TRANTYPE = 'SA' "
            'Dim Sgst As Double = GetSqlValue(cn, strsqlCheck)
            'strsqlCheck = " SELECT LTRIM(SUM(TAXPER)) AS GSTPER FROM " & cnStockDb & "..TAXTRAN "
            'strsqlCheck += vbCrLf + " WHERE TRANDATE = '" & strBilldate & "' "
            'strsqlCheck += vbCrLf + " AND BATCHNO = '" & pBatchno & "' AND TRANTYPE = 'SA'"
            'Dim gstPer As Double = Val(GetSqlValue(cn, strsqlCheck).ToString)
            Dim Cgst As Double = 0
            Dim Sgst As Double = 0
            If True Then
                strsql = " EXEC " & cnAdminDb & "..SP_BILL_BILLPRINTVIEW "
                strsql += vbCrLf + " @ADMINDB ='" & cnAdminDb & "' "
                strsql += vbCrLf + " ,@TRANDB ='" & cnStockDb & "' "
                strsql += vbCrLf + " ,@TRANDATE = '" & strBilldate & "' "
                strsql += vbCrLf + " ,@BATCHNO = '" & pBatchno & "' "
                strsql += vbCrLf + " ,@SYSTEMID = '" & SystemName & "' "
                strsql += vbCrLf + " ,@DISCREMARK = '" & DiscRemark & "' "
                strsql += vbCrLf + " ,@DISCOUNT = " & DiscountAmt & " "
                strsql += vbCrLf + " ,@FINALAMT = " & FinalAmt & " "
                strsql += vbCrLf + " ,@CGST = '" & Cgst & "' "
                strsql += vbCrLf + " ,@SGST = '" & Sgst & "' "
                'strsql += vbCrLf + " ,@IGST = 0 "
                'strsql += vbCrLf + " ,@GSTPER = '" & gstPer & "' "
                cmd = New OleDbCommand(strsql, cn)
                cmd.ExecuteNonQuery()
            Else
                ''strsql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'DISCREMARK'"
                ''Dim DiscRemark As String = ""
                ''If DiscRemark = "" Then
                ''    DiscRemark = GetSqlValue(cn, strsql)
                ''End If
                strsql = vbCrLf + " SELECT *,NULL ISSNO ,IDENTITY(INT,1,1) PGNO "
                strsql += vbCrLf + " INTO TEMPTABLEDB..[TEMP" & SystemName & "SALEINVOICE] "
                strsql += vbCrLf + " FROM ( "
                strsql += vbCrLf + "  /*SALES */"
                strsql += vbCrLf + " SELECT "
                strsql += vbCrLf + "  ROW_NUMBER() OVER(ORDER BY SNO ASC) AS SRNO,"
                strsql += vbCrLf + "  I.TRANNO,"
                strsql += vbCrLf + "  CONVERT(VARCHAR(15),I.TRANDATE,103) AS BILLDATE,"
                strsql += vbCrLf + "  BATCHNO AS BATCHNO_N,"
                strsql += vbCrLf + "  (CASE WHEN ISNULL(SM.SUBITEMNAME,'')='' THEN IM.ITEMNAME ELSE SM.SUBITEMNAME END) ITEMNAME"
                strsql += vbCrLf + "  ,I.PCS AS QTY,I.GRSWT,I.NETWT,I.LESSWT,I.WASTAGE,CONVERT(VARCHAR,CONVERT(NUMERIC(15,2),I.RATE))RATE"
                strsql += vbCrLf + "  ,CONVERT(VARCHAR,I.MCHARGE)MCHARGE"
                strsql += vbCrLf + "  ,I.TAX,(I.AMOUNT+I.FIN_DISCOUNT+I.DISCOUNT) AMOUNT"
                strsql += vbCrLf + "  ,I.SNO,1.0 AS RESULT,1 AS TYPE,'' AS COLHEAD"
                strsql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS I "
                strsql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID=I.ITEMID"
                strsql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID=I.ITEMID AND SM.SUBITEMID=I.SUBITEMID"
                strsql += vbCrLf + "  WHERE I.BATCHNO='" & pBatchno & "'  "
                'strsql += vbCrLf + "  AND I.TRANTYPE NOT IN ('MI')"
                strsql += vbCrLf + "  /*SALES STONE*/"
                strsql += vbCrLf + "  UNION ALL   "
                strsql += vbCrLf + "  SELECT "
                strsql += vbCrLf + "  NULL,NULL,NULL,"
                strsql += vbCrLf + "  BATCHNO AS BATCHNO_N,"
                strsql += vbCrLf + "  '   '+(CASE WHEN ISNULL(SM.SUBITEMNAME,'')='' THEN IM.ITEMNAME ELSE SM.SUBITEMNAME END) + ' - ' + CONVERT(VARCHAR,I.STNPCS)"
                strsql += vbCrLf + "  + ' - ' + CONVERT(VARCHAR,I.STNWT)"
                strsql += vbCrLf + "  + ' @ ' + CONVERT(VARCHAR,I.STNRATE)"
                strsql += vbCrLf + "  + ' = ' + CONVERT(VARCHAR,I.STNAMT) ITEMNAME"
                strsql += vbCrLf + "  ,NULL AS QTY,NULL AS GRSWT,NULL AS NETWT,NULL AS LESSWT,NULL,CONVERT(VARCHAR,NULL)RATE,NULL AS MCHARGE,I.TAX,NULL"
                strsql += vbCrLf + "  ,I.ISSSNO,2.0 AS RESULT,1 AS TYPE,'S' AS COLHEAD"
                strsql += vbCrLf + "  FROM " & cnStockDb & "..ISSSTONE AS I "
                strsql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID=I.STNITEMID"
                strsql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID=I.STNITEMID AND SM.SUBITEMID=I.STNSUBITEMID"
                strsql += vbCrLf + "  WHERE I.BATCHNO='" & pBatchno & "'  AND I.TRANTYPE IN ('SA','OD','RD')  "

                strsql += vbCrLf + "  /*SALES TOTAL*/"
                strsql += vbCrLf + "  UNION ALL "
                strsql += vbCrLf + "  SELECT "
                strsql += vbCrLf + "  NULL AS SRNO,"
                strsql += vbCrLf + "  NULL TRANNO,"
                strsql += vbCrLf + "  CONVERT(VARCHAR(15),I.TRANDATE,103) AS BILLDATE,"
                strsql += vbCrLf + "  BATCHNO AS BATCHNO_N,"
                strsql += vbCrLf + "  '' ITEMNAME"
                strsql += vbCrLf + "  ,SUM(I.PCS) AS QTY,SUM(I.GRSWT)GRSWT,SUM(I.NETWT)NETWT,SUM(I.LESSWT) LESSWT,NULL WASTAGE"
                strsql += vbCrLf + "  ,CONVERT(VARCHAR,NULL)RATE,CONVERT(VARCHAR, NULL) MCHARGE"
                strsql += vbCrLf + "  ,SUM(I.TAX)TAX,SUM(I.AMOUNT+I.FIN_DISCOUNT+I.DISCOUNT)AMOUNT"
                strsql += vbCrLf + "  ,'ZZZZZZZZ' SNO,3.0 AS RESULT,1 AS TYPE,'G' AS COLHEAD"
                strsql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS I "
                strsql += vbCrLf + "  WHERE I.BATCHNO='" & pBatchno & "' AND I.TRANTYPE IN ('SA','OD','RD')"
                strsql += vbCrLf + "  GROUP BY I.TRANDATE,I.BATCHNO HAVING COUNT(*)>1"

                strsql += vbCrLf + "  /*DISCOUNT TOTAL*/"
                strsql += vbCrLf + "  UNION ALL "
                strsql += vbCrLf + "  SELECT "
                strsql += vbCrLf + "  NULL AS SRNO,"
                strsql += vbCrLf + "  NULL TRANNO,"
                strsql += vbCrLf + "  CONVERT(VARCHAR(15),I.TRANDATE,103) AS BILLDATE,"
                strsql += vbCrLf + "  BATCHNO AS BATCHNO_N,"
                strsql += vbCrLf + "  '" & DiscRemark & "' ITEMNAME"
                strsql += vbCrLf + "  ,NULL AS QTY,NULL GRSWT,NULL NETWT,NULL LESSWT,NULL WASTAGE"
                strsql += vbCrLf + "  ,CONVERT(VARCHAR,NULL)RATE,CONVERT(VARCHAR, NULL) MCHARGE"
                strsql += vbCrLf + "  ,NULL TAX,SUM(I.FIN_DISCOUNT+I.DISCOUNT)AMOUNT,'ZZZZZZZZ' SNO,3.1 AS RESULT,1 AS TYPE,'G' AS COLHEAD"
                strsql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS I "
                strsql += vbCrLf + "  WHERE I.BATCHNO='" & pBatchno & "' AND I.TRANTYPE IN ('SA','OD','RD') AND (I.FIN_DISCOUNT + I.DISCOUNT) > 0"
                strsql += vbCrLf + "  GROUP BY I.TRANDATE,I.BATCHNO "
                'strsqlCheck = "SELECT ISNULL((SUM(FIN_DISCOUNT)+SUM(DISCOUNT)),0) DIST FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = '" & pBatchno & "'"
                'Dim DiscountAmt As Integer = GetSqlValue(cn, strsqlCheck)
                If DiscountAmt > 0 Then
                    strsql += vbCrLf + "  /*TOTAL IF DISCOUNT > 0*/"
                    strsql += vbCrLf + "  UNION ALL "
                    strsql += vbCrLf + "  SELECT "
                    strsql += vbCrLf + "  NULL AS SRNO,"
                    strsql += vbCrLf + "  NULL TRANNO,"
                    strsql += vbCrLf + "  CONVERT(VARCHAR(15),I.TRANDATE,103) AS BILLDATE,"
                    strsql += vbCrLf + "  BATCHNO AS BATCHNO_N,"
                    strsql += vbCrLf + "  NULL ITEMNAME"
                    strsql += vbCrLf + "  ,NULL AS QTY,NULL GRSWT,NULL NETWT,NULL LESSWT,NULL WASTAGE"
                    strsql += vbCrLf + "  ,CONVERT(VARCHAR,NULL)RATE,CONVERT(VARCHAR, NULL) MCHARGE"
                    strsql += vbCrLf + "  ,NULL TAX,SUM(I.AMOUNT)AMOUNT,'ZZZZZZZZ' SNO"
                    strsql += vbCrLf + "  ,3.2 AS RESULT,1 AS TYPE,'G' AS COLHEAD"
                    strsql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS I "
                    strsql += vbCrLf + "  WHERE I.BATCHNO='" & pBatchno & "' AND I.TRANTYPE IN ('SA','OD','RD') "
                    strsql += vbCrLf + "  GROUP BY I.TRANDATE,I.BATCHNO "
                End If

                strsql += vbCrLf + "  /*SALES VAT*/"
                strsql += vbCrLf + "  UNION ALL "
                strsql += vbCrLf + "  SELECT "
                strsql += vbCrLf + "  NULL AS SRNO,"
                strsql += vbCrLf + "  NULL TRANNO,"
                strsql += vbCrLf + "  CONVERT(VARCHAR(15),I.TRANDATE,103) AS BILLDATE,"
                strsql += vbCrLf + "  BATCHNO AS BATCHNO_N,"
                strsql += vbCrLf + "  NULL ITEMNAME"
                strsql += vbCrLf + "  ,NULL AS QTY,NULL GRSWT,NULL NETWT,NULL  LESSWT,NULL WASTAGE"
                strsql += vbCrLf + "  ,CONVERT(VARCHAR,'VAT')RATE,CONVERT(VARCHAR,NULL) AS MCHARGE"
                strsql += vbCrLf + "  ,NULL TAX,SUM(I.TAX) AMOUNT,'ZZZZZZZZ' SNO"
                strsql += vbCrLf + "  ,3.3 AS RESULT,1 AS TYPE,'G' AS COLHEAD"
                strsql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS I "
                strsql += vbCrLf + "  WHERE I.BATCHNO='" & pBatchno & "' AND I.TRANTYPE IN ('SA','OD','RD')"
                strsql += vbCrLf + "  GROUP BY I.TRANDATE,I.BATCHNO "
                strsql += vbCrLf + "  HAVING SUM(I.TAX) > 0"

                strsql += vbCrLf + "  /*SALES AMOUNT + VAT*/"
                strsql += vbCrLf + "  UNION ALL "
                strsql += vbCrLf + "  SELECT "
                strsql += vbCrLf + "  NULL AS SRNO,"
                strsql += vbCrLf + "  NULL TRANNO,"
                strsql += vbCrLf + "  CONVERT(VARCHAR(15),I.TRANDATE,103) AS BILLDATE,"
                strsql += vbCrLf + "  BATCHNO AS BATCHNO_N,"
                strsql += vbCrLf + "  NULL ITEMNAME"
                strsql += vbCrLf + "  ,NULL AS QTY,NULL GRSWT,NULL NETWT,NULL  LESSWT,NULL WASTAGE"
                strsql += vbCrLf + "  ,CONVERT(VARCHAR,'TOTAL')RATE,CONVERT(VARCHAR,NULL) AS MCHARGE,NULL TAX"
                strsql += vbCrLf + "  ,ISNULL(SUM(I.TAX),0)+ISNULL(SUM(I.AMOUNT),0) AMOUNT "
                strsql += vbCrLf + "  ,'ZZZZZZZZ' SNO,3.4 AS RESULT,1 AS TYPE,'G' AS COLHEAD"
                strsql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS I "
                strsql += vbCrLf + "  WHERE I.BATCHNO='" & pBatchno & "' AND I.TRANTYPE IN ('SA','OD','RD')"
                strsql += vbCrLf + "  GROUP BY I.TRANDATE,I.BATCHNO "
                strsql += vbCrLf + " HAVING SUM(I.TAX) > 0"

                strsql += vbCrLf + "  /*PU TITLE*/"
                strsql += vbCrLf + "  UNION ALL "
                strsql += vbCrLf + "  SELECT "
                strsql += vbCrLf + "  NULL AS SRNO,"
                strsql += vbCrLf + "  NULL TRANNO,"
                strsql += vbCrLf + "  CONVERT(VARCHAR(15),I.TRANDATE,103) AS BILLDATE,"
                strsql += vbCrLf + "  BATCHNO AS BATCHNO_N,"
                strsql += vbCrLf + "  'PURCHASE  No : ' + CONVERT(VARCHAR,TRANNO) ITEMNAME"
                strsql += vbCrLf + "  ,NULL AS QTY,NULL GRSWT,NULL NETWT,NULL LESSWT,NULL WASTAGE"
                strsql += vbCrLf + "  ,CONVERT(VARCHAR,NULL)RATE,CONVERT(VARCHAR,NULL) AS MCHARGE,NULL TAX"
                strsql += vbCrLf + "  ,NULL AMOUNT,NULL SNO,0 AS RESULT,2 AS TYPE,'T' AS COLHEAD"
                strsql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS I "
                strsql += vbCrLf + "  WHERE I.BATCHNO='" & pBatchno & "' AND I.TRANTYPE='PU' "
                strsql += vbCrLf + "  GROUP BY I.TRANDATE,I.BATCHNO,I.TRANNO "

                strsql += vbCrLf + "  /*PURCHASE*/"
                strsql += vbCrLf + "  UNION ALL"
                strsql += vbCrLf + "  SELECT "
                strsql += vbCrLf + "  ROW_NUMBER() OVER(ORDER BY SNO ASC) AS SRNO,"
                strsql += vbCrLf + "  I.TRANNO,"
                strsql += vbCrLf + "  CONVERT(VARCHAR(15),I.TRANDATE,103) AS BILLDATE,"
                strsql += vbCrLf + "  BATCHNO AS BATCHNO_N,"
                strsql += vbCrLf + "  IM.CATNAME ITEMNAME"
                strsql += vbCrLf + "  ,I.PCS AS QTY,I.GRSWT,I.NETWT,I.LESSWT,I.WASTAGE"
                strsql += vbCrLf + "  ,CONVERT(VARCHAR,CONVERT(NUMERIC(15,2),I.RATE))RATE"
                strsql += vbCrLf + "  ,CASE WHEN ISNULL(I.DUSTWT,0)<>0 THEN '('+ CONVERT(VARCHAR,I.DUSTWT)+ ')' ELSE NULL END AS MCHARGE"
                strsql += vbCrLf + "  ,I.TAX,I.AMOUNT,I.SNO,1.0 AS RESULT,2 AS TYPE,'' AS COLHEAD"
                strsql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS I "
                strsql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..CATEGORY AS IM ON IM.CATCODE=I.CATCODE"
                strsql += vbCrLf + "  WHERE I.BATCHNO='" & pBatchno & "' AND I.TRANTYPE='PU'"

                strsql += vbCrLf + "  /*PU AMOUNT*/"
                strsql += vbCrLf + "  UNION ALL "
                strsql += vbCrLf + "  SELECT "
                strsql += vbCrLf + "  NULL AS SRNO,"
                strsql += vbCrLf + "  NULL TRANNO,"
                strsql += vbCrLf + "  CONVERT(VARCHAR(15),I.TRANDATE,103) AS BILLDATE,"
                strsql += vbCrLf + "  BATCHNO AS BATCHNO_N,"
                strsql += vbCrLf + "  '' ITEMNAME"
                strsql += vbCrLf + "  ,SUM(I.PCS) AS QTY,SUM(I.GRSWT)GRSWT,SUM(I.NETWT)NETWT,SUM(I.LESSWT) LESSWT,NULL WASTAGE"
                strsql += vbCrLf + "  ,CONVERT(VARCHAR,NULL)RATE, CONVERT(VARCHAR, SUM(I.MCHARGE)) MCHARGE,SUM(I.TAX)TAX,SUM(I.AMOUNT)AMOUNT"
                strsql += vbCrLf + "  ,'ZZZZZZZZ' SNO,3.0 AS RESULT,2 AS TYPE,'G' AS COLHEAD"
                strsql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS I "
                strsql += vbCrLf + "  WHERE I.BATCHNO='" & pBatchno & "' AND I.TRANTYPE='PU'"
                strsql += vbCrLf + "  GROUP BY I.TRANDATE,I.BATCHNO  HAVING COUNT(*)>1"

                strsql += vbCrLf + "  /*PU VAT*/"
                strsql += vbCrLf + "  UNION ALL "
                strsql += vbCrLf + "  SELECT "
                strsql += vbCrLf + "  NULL AS SRNO,"
                strsql += vbCrLf + "  NULL TRANNO,"
                strsql += vbCrLf + "  CONVERT(VARCHAR(15),I.TRANDATE,103) AS BILLDATE,"
                strsql += vbCrLf + "  BATCHNO AS BATCHNO_N,"
                strsql += vbCrLf + "  'VAT' ITEMNAME"
                strsql += vbCrLf + "  ,NULL AS QTY,NULL GRSWT,NULL NETWT,NULL LESSWT,NULL WASTAGE"
                strsql += vbCrLf + "  ,CONVERT(VARCHAR,NULL)RATE,CONVERT(VARCHAR,NULL) AS MCHARGE"
                strsql += vbCrLf + "  ,NULL TAX,SUM(I.TAX) AMOUNT"
                strsql += vbCrLf + " ,'ZZZZZZZZ' SNO,3.1 AS RESULT,2 AS TYPE,'G' AS COLHEAD"
                strsql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS I "
                strsql += vbCrLf + "  WHERE I.BATCHNO='" & pBatchno & "' AND I.TRANTYPE='PU' AND I.TAX > 0"
                strsql += vbCrLf + "  GROUP BY I.TRANDATE,I.BATCHNO"

                strsql += vbCrLf + "  /*PU AMOUNT + VAT*/"
                strsql += vbCrLf + "  UNION ALL "
                strsql += vbCrLf + "  SELECT "
                strsql += vbCrLf + "  NULL AS SRNO,"
                strsql += vbCrLf + "  NULL TRANNO,"
                strsql += vbCrLf + "  CONVERT(VARCHAR(15),I.TRANDATE,103) AS BILLDATE,"
                strsql += vbCrLf + "  BATCHNO AS BATCHNO_N,"
                strsql += vbCrLf + "  '' ITEMNAME"
                strsql += vbCrLf + "  ,NULL AS QTY,NULL GRSWT,NULL NETWT,NULL LESSWT,NULL WASTAGE"
                strsql += vbCrLf + "  ,CONVERT(VARCHAR,NULL)RATE,CONVERT(VARCHAR,NULL) AS MCHARGE,NULL TAX"
                strsql += vbCrLf + "  ,SUM(I.TAX)+SUM(I.AMOUNT) AMOUNT,'ZZZZZZZZ' SNO"
                strsql += vbCrLf + "  ,3.2 AS RESULT,2 AS TYPE,'G' AS COLHEAD"
                strsql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS I "
                strsql += vbCrLf + "  WHERE I.BATCHNO='" & pBatchno & "' AND I.TRANTYPE='PU' AND I.TAX > 0"
                strsql += vbCrLf + "  GROUP BY I.TRANDATE,I.BATCHNO"

                strsql += vbCrLf + "  /*SR TITLE*/"
                strsql += vbCrLf + "  UNION ALL "
                strsql += vbCrLf + "  SELECT "
                strsql += vbCrLf + "  NULL AS SRNO,"
                strsql += vbCrLf + "  NULL TRANNO,"
                strsql += vbCrLf + "  CONVERT(VARCHAR(15),I.TRANDATE,103) AS BILLDATE,"
                strsql += vbCrLf + "  BATCHNO AS BATCHNO_N,"
                strsql += vbCrLf + "  'SALES RETURN  No : ' + CONVERT(VARCHAR,TRANNO)  ITEMNAME"
                strsql += vbCrLf + "  ,NULL AS QTY,NULL GRSWT,NULL NETWT,NULL LESSWT,NULL WASTAGE"
                strsql += vbCrLf + "  ,CONVERT(VARCHAR,NULL)RATE,CONVERT(VARCHAR,NULL) AS MCHARGE"
                strsql += vbCrLf + "  ,NULL TAX,NULL AMOUNT,NULL SNO,0 AS RESULT,3 AS TYPE,'T' AS COLHEAD"
                strsql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS I "
                strsql += vbCrLf + "  WHERE I.BATCHNO='" & pBatchno & "' AND I.TRANTYPE='SR' "
                strsql += vbCrLf + "  GROUP BY I.TRANDATE,I.BATCHNO,I.TRANNO "
                strsql += vbCrLf + "  /*SALES RETURN */"
                strsql += vbCrLf + "  UNION ALL"
                strsql += vbCrLf + "  SELECT "
                strsql += vbCrLf + "  ROW_NUMBER() OVER(ORDER BY SNO ASC) AS SRNO,"
                strsql += vbCrLf + "  I.TRANNO,"
                strsql += vbCrLf + "  CONVERT(VARCHAR(15),I.TRANDATE,103) AS BILLDATE,"
                strsql += vbCrLf + "  BATCHNO AS BATCHNO_N,"
                strsql += vbCrLf + "  IM.ITEMNAME "
                strsql += vbCrLf + "  ,I.PCS AS QTY,I.GRSWT,I.NETWT,I.LESSWT,I.WASTAGE"
                strsql += vbCrLf + "  ,CONVERT(VARCHAR,CONVERT(NUMERIC(15,2),I.RATE))RATE"
                strsql += vbCrLf + "  ,CONVERT(VARCHAR,I.MCHARGE)MCHARGE"
                strsql += vbCrLf + "  ,I.TAX,(ISNULL(I.TAX,0)+ISNULL(I.AMOUNT,0)),I.SNO,1.0 AS RESULT,3 AS TYPE,'' AS COLHEAD"
                strsql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS I "
                strsql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID=I.ITEMID"
                strsql += vbCrLf + "  WHERE I.BATCHNO='" & pBatchno & "' AND I.TRANTYPE='SR'"

                strsql += vbCrLf + "  /*SR AMOUNT*/"
                strsql += vbCrLf + "  UNION ALL "
                strsql += vbCrLf + "  SELECT "
                strsql += vbCrLf + "  NULL AS SRNO,"
                strsql += vbCrLf + "  NULL TRANNO,"
                strsql += vbCrLf + "  CONVERT(VARCHAR(15),I.TRANDATE,103) AS BILLDATE,"
                strsql += vbCrLf + "  BATCHNO AS BATCHNO_N,"
                strsql += vbCrLf + "  '' ITEMNAME"
                strsql += vbCrLf + "  ,SUM(I.PCS) AS QTY,SUM(I.GRSWT)GRSWT,SUM(I.NETWT)NETWT,SUM(I.LESSWT) LESSWT"
                strsql += vbCrLf + "  ,NULL AS WASTAGE"
                strsql += vbCrLf + "  ,CONVERT(VARCHAR,NULL)RATE,CONVERT(VARCHAR, NULL) MCHARGE,"
                strsql += vbCrLf + "  SUM(I.TAX)TAX,(ISNULL(SUM(I.TAX),0)+ISNULL(SUM(I.AMOUNT),0)) AMOUNT,'ZZZZZZZZ' SNO,3.0 AS RESULT,3 AS TYPE,'G' AS COLHEAD"
                strsql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS I "
                strsql += vbCrLf + "  WHERE I.BATCHNO='" & pBatchno & "' AND I.TRANTYPE='SR' "
                strsql += vbCrLf + "  GROUP BY I.TRANDATE,I.BATCHNO HAVING COUNT(*)>1"

                ' ''strSql += vbCrLf + "  /*SR VAT*/"
                ' ''strSql += vbCrLf + "  UNION ALL "
                ' ''strSql += vbCrLf + "  SELECT "
                ' ''strSql += vbCrLf + "  NULL AS SRNO,"
                ' ''strSql += vbCrLf + "  NULL TRANNO,"
                ' ''strSql += vbCrLf + "  CONVERT(VARCHAR(15),I.TRANDATE,103) AS BILLDATE,"
                ' ''strSql += vbCrLf + "  BATCHNO AS BATCHNO_N,"
                ' ''strSql += vbCrLf + "  'VAT' ITEMNAME"
                ' ''strSql += vbCrLf + "  ,NULL AS QTY,NULL GRSWT,NULL NETWT,NULL LESSWT,NULL WASTAGE"
                ' ''strSql += vbCrLf + "  ,CONVERT(VARCHAR,NULL)RATE,CONVERT(VARCHAR,NULL) AS MCHARGE,NULL TAX,SUM(I.TAX) AMOUNT,'ZZZZZZZZ' SNO,3.1 AS RESULT,2 AS TYPE,'G' AS COLHEAD"
                ' ''strSql += vbCrLf + "  FROM " & cnStockdb & "..RECEIPT AS I "
                ' ''strSql += vbCrLf + "  WHERE I.BATCHNO='" & pBatchno & "' AND I.TRANTYPE='SR' AND I.TAX>0"
                ' ''strSql += vbCrLf + "  GROUP BY I.TRANDATE,I.BATCHNO"
                ' ''strsql += vbCrLf + "  /*SR AMOUNT + VAT*/"
                ' ''strsql += vbCrLf + "  UNION ALL "
                ' ''strsql += vbCrLf + "  SELECT "
                ' ''strsql += vbCrLf + "  NULL AS SRNO,"
                ' ''strsql += vbCrLf + "  NULL TRANNO,"
                ' ''strsql += vbCrLf + "  CONVERT(VARCHAR(15),I.TRANDATE,103) AS BILLDATE,"
                ' ''strsql += vbCrLf + "  BATCHNO AS BATCHNO_N,"
                ' ''strsql += vbCrLf + "  '' ITEMNAME"
                ' ''strsql += vbCrLf + "  ,NULL AS QTY,NULL GRSWT,NULL NETWT,NULL LESSWT,NULL WASTAGE"
                ' ''strsql += vbCrLf + "  ,CONVERT(VARCHAR,'RETURN')RATE,CONVERT(VARCHAR,NULL) AS MCHARGE"
                ' ''strsql += vbCrLf + "  ,NULL TAX,SUM(I.AMOUNT)+SUM(I.TAX) AMOUNT"
                ' ''strsql += vbCrLf + "  ,'ZZZZZZZZ' SNO,3.2 AS RESULT,3 AS TYPE,'G' AS COLHEAD"
                ' ''strsql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS I "
                ' ''strsql += vbCrLf + "  WHERE I.BATCHNO='" & pBatchno & "' AND I.TRANTYPE='SR' AND I.TAX>0"
                ' ''strsql += vbCrLf + "  GROUP BY I.TRANDATE,I.BATCHNO HAVING COUNT(*)>1"

                strsqlCheck = ""
                strsqlCheck = vbCrLf + "SELECT  ISNULL(SUM(IAMT),0) IAMT, ISNULL(SUM(RAMT),0)RAMT, SUM(IAMT)-SUM(RAMT) AS FINAMT FROM("
                strsqlCheck += vbCrLf + "(SELECT SUM(AMOUNT + TAX) IAMT, 0 RAMT FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = '" & pBatchno & "') UNION ALL"
                strsqlCheck += vbCrLf + "(SELECT 0 IAMT , SUM(AMOUNT + TAX) RMT   FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO = '" & pBatchno & "')"
                strsqlCheck += vbCrLf + ")A"
                Dim ro1 As DataRow = GetSqlRow(strsqlCheck, cn)
                If Val(ro.Item("IAMT").ToString) > 0 And Val(ro.Item("RAMT").ToString) > 0 Then
                    'Dim FinalAmt As Decimal = Val(ro.Item("FINAMT").ToString)
                    FinalAmt = Val(ro.Item("FINAMT").ToString)
                    strsql += vbCrLf + "  /*FINAL TOTAL*/"
                    strsql += vbCrLf + "  UNION ALL "
                    strsql += vbCrLf + "  SELECT "
                    strsql += vbCrLf + "  NULL AS SRNO,"
                    strsql += vbCrLf + "  NULL TRANNO,"
                    strsql += vbCrLf + "  NULL AS BILLDATE,"
                    strsql += vbCrLf + "  NULL AS BATCHNO_N,"
                    strsql += vbCrLf + "  '' ITEMNAME"
                    strsql += vbCrLf + "  ,NULL AS QTY,NULL GRSWT,NULL NETWT,NULL LESSWT,NULL WASTAGE"
                    strsql += vbCrLf + "  ,NULL RATE,NULL AS MCHARGE"
                    strsql += vbCrLf + "  ,NULL TAX," & FinalAmt & " AMOUNT"
                    strsql += vbCrLf + "  ,'ZZZZZZZZ' SNO,4.1 AS RESULT,4 AS TYPE,'T' AS COLHEAD"
                End If
                'ADVANCE PART
                strsql += vbCrLf + "  UNION ALL "
                strsql += vbCrLf + "  SELECT DISTINCT "
                strsql += vbCrLf + "  NULL AS SRNO,"
                strsql += vbCrLf + "  NULL TRANNO,"
                strsql += vbCrLf + "  CONVERT(VARCHAR(15),I.TRANDATE,103) AS BILLDATE,"
                strsql += vbCrLf + "  BATCHNO AS BATCHNO_N,"
                strsql += vbCrLf + "  'Advance Received RefNo: '+ I.REFNO "
                strsql += vbCrLf + "  AS ITEMNAME"
                strsql += vbCrLf + "  ,NULL AS QTY,NULL GRSWT,NULL NETWT,NULL LESSWT,NULL WASTAGE"
                strsql += vbCrLf + "  ,CASE WHEN PAYMODE='AR' THEN '' END RATE"
                strsql += vbCrLf + "  ,CONVERT(VARCHAR,NULL) AS MCHARGE,NULL TAX"
                strsql += vbCrLf + "  ,SUM(CASE WHEN TRANMODE='C' THEN AMOUNT ELSE -1* AMOUNT END ) AMOUNT"
                strsql += vbCrLf + "  ,NULL SNO,3.1 AS RESULT,9 AS TYPE,'' AS COLHEAD"
                strsql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN AS I "
                strsql += vbCrLf + "  WHERE I.BATCHNO='" & pBatchno & "' AND PAYMODE= 'AR'"
                strsql += vbCrLf + "  GROUP BY I.TRANDATE,I.BATCHNO,PAYMODE,I.REFNO "
                'TCS AND HAND CHARGE
                strsql += vbCrLf + "  UNION ALL "
                strsql += vbCrLf + "  SELECT DISTINCT "
                strsql += vbCrLf + "  NULL AS SRNO,"
                strsql += vbCrLf + "  NULL TRANNO,"
                strsql += vbCrLf + "  CONVERT(VARCHAR(15),I.TRANDATE,103) AS BILLDATE,"
                strsql += vbCrLf + "  BATCHNO AS BATCHNO_N,"
                strsql += vbCrLf + "  '' ITEMNAME"
                strsql += vbCrLf + "  ,NULL AS QTY,NULL GRSWT,NULL NETWT,NULL LESSWT,NULL WASTAGE"
                strsql += vbCrLf + "  ,CASE WHEN PAYMODE='SE' THEN 'TCS' ELSE 'HAND.CHRG' END RATE"
                strsql += vbCrLf + "  ,CONVERT(VARCHAR,NULL) AS MCHARGE,NULL TAX"
                strsql += vbCrLf + "  ,SUM(CASE WHEN TRANMODE='C' THEN AMOUNT ELSE -1* AMOUNT END ) AMOUNT"
                strsql += vbCrLf + "  ,NULL SNO,3.1 AS RESULT,9 AS TYPE,'' AS COLHEAD"
                strsql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN AS I "
                strsql += vbCrLf + "  WHERE I.BATCHNO='" & pBatchno & "' AND ACCODE IN('TCS','HANDC') AND PAYMODE IN('SE','HC') "
                strsql += vbCrLf + "  GROUP BY I.TRANDATE,I.BATCHNO,PAYMODE "
                'Scheme
                strsql += vbCrLf + " UNION ALL"
                strsql += vbCrLf + " SELECT NULL SRNO ,NULL AS TRANNO ,NULL AS BILLDATE , BATCHNO AS BATCHNO_N ,"
                strsql += vbCrLf + " CHQCARDREF + '-' + CHQCARDNO "

                strsql += vbCrLf + " + '(' +"
                strsql += vbCrLf + " (SELECT CONVERT(VARCHAR,SUM(AMOUNT)) FROM " & cnStockDb & "..ACCTRAN "
                strsql += vbCrLf + " WHERE PAYMODE IN ('SS') "
                'strsql += vbCrLf + " AND TRANDATE = '2017-04-11'"
                strsql += vbCrLf + " AND BATCHNO = '" & pBatchno & "') + '+' +"
                strsql += vbCrLf + " (SELECT CONVERT(VARCHAR,SUM(AMOUNT)) FROM " & cnStockDb & "..ACCTRAN "
                strsql += vbCrLf + " WHERE PAYMODE IN ('CB','CG','CP','CD','CZ') "
                'strsql += vbCrLf + " AND TRANDATE = '2017-04-11'"
                strsql += vbCrLf + " AND BATCHNO = '" & pBatchno & "') + ')'"

                strsql += vbCrLf + " AS ITEMNAME,"
                strsql += vbCrLf + " NULL QTY, NULL GRSWT , NULL NETWT, NULL LESSWT , NULL WASTAGE, "
                strsql += vbCrLf + " 'SCHEME' as  RATE"
                strsql += vbCrLf + " ,CONVERT(VARCHAR,NULL) AS MCHARGE,"
                strsql += vbCrLf + " NULL TAX ,"
                strsql += vbCrLf + " SUM(CASE WHEN TRANMODE='D' THEN AMOUNT ELSE ((-1)*(AMOUNT)) END ) AS AMOUNT, "
                strsql += vbCrLf + " NULL AS SNO"
                strsql += vbCrLf + " ,3.3 RESULT ,11 AS TYPE, 'P' AS COLHEAD"
                strsql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN "
                strsql += vbCrLf + " WHERE PAYMODE IN ('SS','CB','CG','CP','CD','CZ') "
                strsql += vbCrLf + " AND BATCHNO='" & pBatchno & "'"
                strsql += vbCrLf + " GROUP BY BATCHNO,CHQCARDREF,CHQCARDNO"
                'Cheque, Credit, Advance
                strsql += vbCrLf + " UNION ALL"
                strsql += vbCrLf + " SELECT NULL SRNO,NULL TRANNO,NULL BILLNO,BATCHNO BATCHNO_N,"
                strsql += vbCrLf + " CASE WHEN PAYMODE = 'CC' "
                strsql += vbCrLf + " THEN AC.SHORTNAME + ' ' + CHQCARDNO ELSE "
                strsql += vbCrLf + " CASE WHEN PAYMODE = 'AA' THEN 'REFER : ' + SUBSTRING(REFNO,1,3) + '-' + SUBSTRING(REFNO,4,10)  "
                strsql += vbCrLf + " ELSE CHQCARDREF + ' ' + CHQCARDNO "
                strsql += vbCrLf + " END END "
                strsql += vbCrLf + " AS ITEMNAME,"
                strsql += vbCrLf + " NULL QTY, NULL GRSWT, NULL NETWT, NULL LESSWT, NULL WASTAGE"
                strsql += vbCrLf + " ,CASE WHEN PAYMODE='CC' THEN 'CARD' ELSE CASE WHEN PAYMODE = 'AA' THEN 'ADVANCE' ELSE 'CHEQUE' END END RATE"
                strsql += vbCrLf + " , NULL MCHARGE, NULL TAX, "
                strsql += vbCrLf + " SUM(AMOUNT)AMOUNT, NULL SNO, "
                strsql += vbCrLf + " 3.3 RESULT, 10 TYPE, 'G' AS COLHEAD FROM " & cnStockDb & "..ACCTRAN T"
                strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CREDITCARD AC ON AC.ACCODE=T.ACCODE"
                strsql += vbCrLf + " WHERE PAYMODE IN ('AA','CC','CH')"
                strsql += vbCrLf + " AND BATCHNO = '" & pBatchno & "'"
                strsql += vbCrLf + " GROUP BY PAYMODE,CHQCARDNO,BATCHNO,AC.SHORTNAME,CHQCARDREF,REFNO"
                'Paymode -> AccTran
                strsql += vbCrLf + " UNION ALL"
                strsql += vbCrLf + " SELECT NULL SRNO ,NULL AS TRANNO ,NULL AS BILLDATE , BATCHNO AS BATCHNO_N ,"
                ' strsql += vbCrLf + " NULL ITEMNAME,"
                strsql += vbCrLf + " NULL ITEMNAME,"
                strsql += vbCrLf + " NULL QTY, NULL GRSWT , NULL NETWT, NULL LESSWT , NULL WASTAGE, "
                strsql += vbCrLf + " (CASE "
                strsql += vbCrLf + " WHEN PAYMODE='CA' THEN 'CASH RECEIVED'"
                strsql += vbCrLf + " WHEN PAYMODE='DI' THEN 'DISCOUNT'"
                strsql += vbCrLf + " WHEN PAYMODE='DU' THEN 'DUE'"
                strsql += vbCrLf + " WHEN PAYMODE='RO' THEN 'ROUND OFF'"
                strsql += vbCrLf + " END) RATE"
                strsql += vbCrLf + " ,CONVERT(VARCHAR,NULL) AS MCHARGE,"
                strsql += vbCrLf + " NULL TAX ,"
                strsql += vbCrLf + " SUM(CASE WHEN TRANMODE='D' THEN AMOUNT ELSE ((-1)*(AMOUNT)) END ) AS AMOUNT, "
                strsql += vbCrLf + " NULL AS SNO"
                '''',3.5 RESULT
                strsql += vbCrLf + " ,(CASE "
                strsql += vbCrLf + " WHEN PAYMODE='CA' THEN 3.59 "
                strsql += vbCrLf + " WHEN PAYMODE='DI' THEN 3.51 "
                strsql += vbCrLf + " WHEN PAYMODE='DU' THEN 3.58 "
                strsql += vbCrLf + " WHEN PAYMODE='RO' THEN 3.50 "
                strsql += vbCrLf + " END) AS RESULT "
                strsql += vbCrLf + " ,11 AS TYPE, 'P' AS COLHEAD"
                strsql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN "
                strsql += vbCrLf + " WHERE PAYMODE IN ('CA','DI','DU','RO') "
                strsql += vbCrLf + " AND BATCHNO='" & pBatchno & "'"
                strsql += vbCrLf + " GROUP BY PAYMODE,BATCHNO,CHQCARDREF,CHQCARDNO"
                strsql += vbCrLf + "  )X ORDER BY TYPE,SNO,RESULT"
                cmd = New OleDbCommand(strsql, cn)
                cmd.ExecuteNonQuery()
            End If
        End If

        strsql = "UPDATE TEMPTABLEDB..[TEMP" & SystemName & "SALEINVOICE] SET RATE='CASH PAID' WHERE AMOUNT<0 AND RATE='CASH RECEIVED'"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = "SELECT TRANNO FROM TEMPTABLEDB..[TEMP" & SystemName & "SALEINVOICE] WHERE TRANNO IS NOT NULL"
        Tranno = GetSqlValue_Bill(strsql, "TRANNO")

        strsql = "SELECT BILLPREFIX FROM TEMPTABLEDB..[TEMP" & SystemName & "SALEINVOICE] WHERE TRANNO IS NOT NULL"
        BillPrefix = GetSqlValue_Bill(strsql, "BILLPREFIX")


        strsql = "SELECT CONVERT(VARCHAR(15),BILLDATE,103) AS BILLDATE FROM TEMPTABLEDB..[TEMP" & SystemName & "SALEINVOICE] WHERE TRANNO IS NOT NULL"
        prtBilldate = GetSqlValue_Bill(strsql, "BILLDATE")

        strsql = "SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = '" & strBilldate & "' AND BATCHNO = '" & pBatchno & "' "
        NodeId = GetSqlValue_Bill(strsql, "SYSTEMID")

        strsql = "UPDATE TEMPTABLEDB..[TEMP" & SystemName & "SALEINVOICE] SET TRANNO='" & Tranno & "' WHERE TRANNO IS  NULL"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
        ' prtBilldate = strBilldate
        If Trantype = "MI" Then
            strsql = "SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "SALEINVOICE] ORDER BY RESULT,PGNO"
        Else
            strsql = "SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "SALEINVOICE] ORDER BY PGNO "
        End If
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtSales)
    End Sub
#End Region
#Region "Header Print Documents"
    Public Sub CustomerInfoSmithIssRec(ByVal Billno As String, ByVal Billdate As String, ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        If dsSmithIssueReceipt.Tables("ACHEAD").Rows.Count > 0 And dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count > 0 Then
            Dim Acname As String = dsSmithIssueReceipt.Tables("ACHEAD").Rows(0).Item("ACNAME").ToString
            Dim Address1 As String = dsSmithIssueReceipt.Tables("ACHEAD").Rows(0).Item("ADDRESS1").ToString
            Dim Address2 As String = dsSmithIssueReceipt.Tables("ACHEAD").Rows(0).Item("ADDRESS2").ToString
            Dim Tin As String = dsSmithIssueReceipt.Tables("ACHEAD").Rows(0).Item("TIN").ToString
            Dim Pan As String = dsSmithIssueReceipt.Tables("ACHEAD").Rows(0).Item("PAN").ToString
            Dim GstNo As String = "GSTIN : " & dsSmithIssueReceipt.Tables("ACHEAD").Rows(0).Item("GSTNO").ToString
            Dim Stateid As String = dsSmithIssueReceipt.Tables("ACHEAD").Rows(0).Item("STATEID").ToString
            Dim TranNo1 As String = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("ISSUENO").ToString
            If TranNo1 = "" And dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count > 1 Then
                TranNo1 = dsSmithIssueReceipt.Tables("DTISSREC").Rows(1).Item("ISSUENO").ToString
            End If
            Dim TDate1 As String = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("DATE").ToString
            If TDate1 = "" And dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count > 1 Then
                TDate1 = dsSmithIssueReceipt.Tables("DTISSREC").Rows(1).Item("DATE").ToString
            End If
            Dim TranType1 As String = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("TRANTYPE").ToString
            If TranType1 = "" And dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count > 1 Then
                TranType1 = dsSmithIssueReceipt.Tables("DTISSREC").Rows(1).Item("TRANTYPE").ToString
            End If
            Dim RateFixed As String = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("RATE").ToString

            If dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("TTYPE").ToString.Contains("RR") Then
            Else
                g.DrawString("BILLING & SHIPPING DETAILS :", New Font("Palatino Linotype", 8, FontStyle.Regular), BlackBrush, c1, TopY, LeftFormat)
                g.DrawString("BILLING & SHIPPING DETAILS :", New Font("Palatino Linotype", 8, FontStyle.Regular), BlackBrush, c1, BottomY, LeftFormat)
                g.DrawString("ORIGINAL FOR RECIPIENT  ", New Font("Palatino Linotype", 8, FontStyle.Regular), Brushes.Black, c6, TopY)
                g.DrawString("DUPLICATE FOR SUPPLIER  ", New Font("Palatino Linotype", 8, FontStyle.Regular), Brushes.Black, c6, BottomY)
                TopY = TopY + 14
                BottomY = BottomY + 14
            End If
            g.DrawString(Acname, fontRegular, BlackBrush, c1, TopY, LeftFormat)
            g.DrawString(Acname, fontRegular, BlackBrush, c1, BottomY, LeftFormat)
            g.DrawString("Voucher No", fontRegular, BlackBrush, c6, TopY, LeftFormat)
            g.DrawString("Voucher No", fontRegular, BlackBrush, c6, BottomY, LeftFormat)
            g.DrawString("  : " & TranNo1, fontRegular, BlackBrush, c7, TopY, LeftFormat)
            g.DrawString("  : " & TranNo1, fontRegular, BlackBrush, c7, BottomY, LeftFormat)
            TopY = TopY + 17
            BottomY = BottomY + 17
            g.DrawString(Address1, fontRegular, BlackBrush, c1, TopY, LeftFormat)
            g.DrawString(Address1, fontRegular, BlackBrush, c1, BottomY, LeftFormat)
            g.DrawString("Date", fontRegular, BlackBrush, c6, TopY, LeftFormat)
            g.DrawString("Date", fontRegular, BlackBrush, c6, BottomY, LeftFormat)
            g.DrawString("  : " & TDate1, fontRegular, BlackBrush, c7, TopY, LeftFormat)
            g.DrawString("  : " & TDate1, fontRegular, BlackBrush, c7, BottomY, LeftFormat)
            TopY = TopY + 17
            BottomY = BottomY + 17
            g.DrawString(Address2, fontRegular, BlackBrush, c1, TopY, LeftFormat)
            g.DrawString(Address2, fontRegular, BlackBrush, c1, BottomY, LeftFormat)
            g.DrawString("GST", fontRegular, BlackBrush, c6, TopY, LeftFormat)
            g.DrawString("GST", fontRegular, BlackBrush, c6, BottomY, LeftFormat)
            g.DrawString("  : " & COMPANYGSTNO, fontRegular, BlackBrush, c7, TopY, LeftFormat)
            g.DrawString("  : " & COMPANYGSTNO, fontRegular, BlackBrush, c7, BottomY, LeftFormat)
            If _MIMR = "NEW" Then
                'g.DrawString("RATE FIXED : " & RateFixed & " (TAX INCLUSIVE)", fontBold, Brushes.Black, 320, TopY, LeftFormat)
                'g.DrawString("RATE FIXED : " & RateFixed & " (TAX INCLUSIVE)", fontBold, Brushes.Black, 320, BottomY, LeftFormat)
            End If
            TopY = TopY + 17
            BottomY = BottomY + 17
            g.DrawString(GstNo, fontRegular, BlackBrush, c1, TopY, LeftFormat)
            g.DrawString(GstNo, fontRegular, BlackBrush, c1, BottomY, LeftFormat)
            g.DrawString(Stateid, fontRegular, BlackBrush, 200, TopY, LeftFormat)
            g.DrawString(Stateid, fontRegular, BlackBrush, 200, BottomY, LeftFormat)
            If TranType1.Length <= 20 Then
                If TranType1 = "ISSUE FOR JOBWORK" Then
                    g.DrawString(TranType1, fontBoldTitle, BlackBrush, c3, TopY - 18, LeftFormat)
                    g.DrawString(TranType1, fontBoldTitle, BlackBrush, c3, BottomY - 18, LeftFormat)
                    g.DrawString("CUM DELIVERY CHALLAN", fontBoldTitle, BlackBrush, c3, TopY - 0, LeftFormat)
                    g.DrawString("CUM DELIVERY CHALLAN", fontBoldTitle, BlackBrush, c3, BottomY - 0, LeftFormat)
                Else
                    g.DrawString(TranType1, fontBoldTitle, BlackBrush, c3, TopY, LeftFormat)
                    g.DrawString(TranType1, fontBoldTitle, BlackBrush, c3, BottomY, LeftFormat)
                End If
            Else
                g.DrawString(TranType1, fontBoldTitle, BlackBrush, c3 - 100, TopY, LeftFormat)
                g.DrawString(TranType1, fontBoldTitle, BlackBrush, c3 - 100, BottomY, LeftFormat)
            End If
            g.DrawString("STATE CODE", fontRegular, BlackBrush, c6, TopY, LeftFormat)
            g.DrawString("STATE CODE", fontRegular, BlackBrush, c6, BottomY, LeftFormat)
            g.DrawString("  : 33", fontRegular, BlackBrush, c7, TopY, LeftFormat)
            g.DrawString("  : 33", fontRegular, BlackBrush, c7, BottomY, LeftFormat)
            TopY = TopY + 17
            BottomY = BottomY + 17
            g.DrawLine(Pens.Silver, 20, TopY, 775, TopY)
            g.DrawLine(Pens.Silver, 20, BottomY, 775, BottomY)
        End If
    End Sub

    Public Sub CustomerInfoSmithIssRecA4(ByVal Billno As String, ByVal Billdate As String, ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal TableName As String)
        If dsSmithIssueReceipt.Tables("ACHEAD").Rows.Count > 0 And dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count > 0 Then
            Dim Acname As String = dsSmithIssueReceipt.Tables("ACHEAD").Rows(0).Item("ACNAME").ToString
            Dim Address1 As String = dsSmithIssueReceipt.Tables("ACHEAD").Rows(0).Item("ADDRESS1").ToString
            Dim Address2 As String = dsSmithIssueReceipt.Tables("ACHEAD").Rows(0).Item("ADDRESS2").ToString
            Dim Tin As String = dsSmithIssueReceipt.Tables("ACHEAD").Rows(0).Item("TIN").ToString
            Dim Pan As String = dsSmithIssueReceipt.Tables("ACHEAD").Rows(0).Item("PAN").ToString
            Dim GstNo As String = "GSTIN : " & dsSmithIssueReceipt.Tables("ACHEAD").Rows(0).Item("GSTNO").ToString
            Dim Stateid As String = dsSmithIssueReceipt.Tables("ACHEAD").Rows(0).Item("STATEID").ToString
            Dim TranNo1 As String = "" 'dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("ISSUENO").ToString
            If TranNo1 = "" Then
                For i As Integer = 0 To dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count - 1
                    If dsSmithIssueReceipt.Tables("DTISSREC").Rows(i).Item("TABLENAME").ToString = TableName Then
                        TranNo1 = dsSmithIssueReceipt.Tables("DTISSREC").Rows(i).Item("ISSUENO").ToString
                    End If
                    If TranNo1 <> "" Then
                        Exit For
                    End If
                Next
            End If
            Dim TDate1 As String = "" ' dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("DATE").ToString
            If TDate1 = "" Then
                For i As Integer = 0 To dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count - 1
                    If dsSmithIssueReceipt.Tables("DTISSREC").Rows(i).Item("TABLENAME").ToString = TableName Then
                        TDate1 = dsSmithIssueReceipt.Tables("DTISSREC").Rows(i).Item("DATE").ToString
                    End If
                    If TDate1 <> "" Then
                        Exit For
                    End If
                Next
            End If
            Dim TranType1 As String = "" 'dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("TRANTYPE").ToString
            If TranType1 = "" Then
                For i As Integer = 0 To dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count - 1
                    If dsSmithIssueReceipt.Tables("DTISSREC").Rows(i).Item("TABLENAME").ToString = TableName Then
                        TranType1 = dsSmithIssueReceipt.Tables("DTISSREC").Rows(i).Item("TRANTYPE").ToString
                    End If
                    If TranType1 <> "" Then
                        Exit For
                    End If
                Next
            End If
            Dim RateFixed As String = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("RATE").ToString
            TopY = TopY + 10
            g.DrawString(TranType1, fontBoldTitle, BlackBrush, c3 - 50, TopY, LeftFormat)
            TopY = TopY + 17
            If dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("TTYPE").ToString.Contains("RR") Then
            Else
                g.DrawString("BILLING & SHIPPING DETAILS :", New Font("Palatino Linotype", 8, FontStyle.Regular), BlackBrush, c1, TopY, LeftFormat)
                g.DrawString("ORIGINAL FOR RECIPIENT  ", New Font("Palatino Linotype", 8, FontStyle.Regular), Brushes.Black, c6, TopY)
                TopY = TopY + 17
            End If
            g.DrawString(Acname, fontRegular, BlackBrush, c1, TopY, LeftFormat)
            g.DrawString("Voucher No", fontRegular, BlackBrush, c6, TopY, LeftFormat)
            g.DrawString("  : " & TranNo1, fontRegular, BlackBrush, c7, TopY, LeftFormat)
            TopY = TopY + 17
            g.DrawString(Address1, fontRegular, BlackBrush, c1, TopY, LeftFormat)
            g.DrawString("Date", fontRegular, BlackBrush, c6, TopY, LeftFormat)
            g.DrawString("  : " & TDate1, fontRegular, BlackBrush, c7, TopY, LeftFormat)
            TopY = TopY + 17
            g.DrawString(Address2, fontRegular, BlackBrush, c1, TopY, LeftFormat)
            g.DrawString("GST", fontRegular, BlackBrush, c6, TopY, LeftFormat)
            g.DrawString("  : " & COMPANYGSTNO, fontRegular, BlackBrush, c7, TopY, LeftFormat)
            'g.DrawString("RATE FIXED : " & RateFixed & " (TAX INCLUSIVE)", fontBold, Brushes.Black, 320, TopY, LeftFormat)
            TopY = TopY + 17
            g.DrawString(GstNo, fontRegular, BlackBrush, c1, TopY, LeftFormat)
            g.DrawString(Stateid, fontRegular, BlackBrush, 200, TopY, LeftFormat)
            ' g.DrawString(TranType1, fontBoldTitle, BlackBrush, c3, TopY, LeftFormat)
            g.DrawString("STATE CODE", fontRegular, BlackBrush, c6, TopY, LeftFormat)
            g.DrawString("  : 33", fontRegular, BlackBrush, c7, TopY, LeftFormat)
            TopY = TopY + 17
            g.DrawLine(Pens.Silver, 20, TopY, 775, TopY)
        End If
    End Sub

    Public Sub funcCustomerQrcode(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal x As Integer, ByVal y As Integer)
        If Not QrcodeCustomer Is Nothing Then
            'g.DrawImage(QrcodeCustomer, New Rectangle(365, 190, 140, 140))
            g.DrawImage(QrcodeCustomer, New Rectangle(x, y, 60, 60))
        End If
    End Sub

    Public Sub CustomerInfo(ByVal Billno As String, ByVal Billdate As String, ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        Dim CusTitle As String = ""
        Dim CusInitial As String = ""
        Dim Cusname As String = ""
        Dim CusDoorno As String = ""
        Dim CusAddress1 As String = ""
        Dim CusPhone_Mobi As String = ""
        Dim CusPhone As String = ""
        Dim CusArea As String = ""
        Dim CusPan As String = ""
        Dim Bno As String
        Dim Bdate As String
        Dim BBill As String = ""
        Dim CusGSTNo As String = ""
        strsql = " SELECT * FROM " & cnAdminDb & "..PERSONALINFO "
        strsql += vbCrLf + "WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = '" & pBatchno & "')"
        da = New OleDbDataAdapter(strsql, cn)
        dtCustInfo = New DataTable
        da.Fill(dtCustInfo)
        If dtCustInfo.Rows.Count > 0 Then
            Cusname = "Mr/Ms. " & Trim(dtCustInfo.Rows(0).Item("PNAME").ToString) & Trim(dtCustInfo.Rows(0).Item("INITIAL").ToString)
            Bno = Billno
            Bdate = Billdate
            BBill = BillPrefix

            funcCustomerQrcode(g, e, 520, TopY - 10)
            funcCustomerQrcode(g, e, 520, BottomY - 10)

            g.DrawString(Cusname, fontRegular, Brushes.Black, c1, TopY)
            g.DrawString(Cusname, fontRegular, Brushes.Black, c1, BottomY)
            Dim c7I As Integer = c7 - 20
            Dim c8D As Integer = c8 - 22
            Dim c8D2 As Integer = c8D + 7
            Select Case Trantype
                Case "POS"
                    g.DrawString("Invoice No", fontRegular, Brushes.Black, c7I, TopY)
                    g.DrawString("Invoice No", fontRegular, Brushes.Black, c7I, BottomY)
                    g.DrawString(" : ", fontRegular, Brushes.Black, c8D, TopY)
                    g.DrawString(" : ", fontRegular, Brushes.Black, c8D, BottomY)
                    g.DrawString(BBill & "" & Bno, fontRegular, Brushes.Black, c8D2, TopY)
                    g.DrawString(BBill & "" & Bno, fontRegular, Brushes.Black, c8D2, BottomY)
                Case "ORD"
                    g.DrawString("Order No", fontRegular, Brushes.Black, c7I, TopY)
                    g.DrawString("Order No", fontRegular, Brushes.Black, c7I, BottomY)
                    g.DrawString(" : ", fontRegular, Brushes.Black, c8D, TopY)
                    g.DrawString(" : ", fontRegular, Brushes.Black, c8D, BottomY)
                    g.DrawString(BBill & "" & Bno, fontRegular, Brushes.Black, c8D2, TopY)
                    g.DrawString(BBill & "" & Bno, fontRegular, Brushes.Black, c8D2, BottomY)
                Case "REP"
                    g.DrawString("Repair No", fontRegular, Brushes.Black, c7I, TopY)
                    g.DrawString("Repair No", fontRegular, Brushes.Black, c7I, BottomY)
                    g.DrawString(" : ", fontRegular, Brushes.Black, c8D, TopY)
                    g.DrawString(" : ", fontRegular, Brushes.Black, c8D, BottomY)
                    g.DrawString(BBill & "" & Bno, fontRegular, Brushes.Black, c8D2, TopY)
                    g.DrawString(BBill & "" & Bno, fontRegular, Brushes.Black, c8D2, BottomY)
            End Select
            TopY = TopY + 18
            BottomY = BottomY + 18
            CusAddress1 = Trim(dtCustInfo.Rows(0).Item("DoorNo") & "").ToString & " " & Trim(dtCustInfo.Rows(0).Item("address1") & "").ToString & Trim(dtCustInfo.Rows(0).Item("address2") & "").ToString & " " & Trim(dtCustInfo.Rows(0).Item("area") & "").ToString
            g.DrawString(CusAddress1, New Font("Palatino Linotype", 7, FontStyle.Regular), Brushes.Black, c1, TopY)
            g.DrawString(CusAddress1, New Font("Palatino Linotype", 7, FontStyle.Regular), Brushes.Black, c1, BottomY)
            g.DrawString("Date", fontRegular, Brushes.Black, c7I, TopY)
            g.DrawString("Date", fontRegular, Brushes.Black, c7I, BottomY)
            g.DrawString(" : ", fontRegular, Brushes.Black, c8D, TopY)
            g.DrawString(" : ", fontRegular, Brushes.Black, c8D, BottomY)
            g.DrawString(Bdate, fontRegular, Brushes.Black, c8D2, TopY)
            g.DrawString(Bdate, fontRegular, Brushes.Black, c8D2, BottomY)
            Select Case Trantype
                Case "ORD"
                    TopY = TopY + 18
                    BottomY = BottomY + 18
                    g.DrawString("Due", fontRegular, Brushes.Black, c7I, TopY)
                    g.DrawString("Due", fontRegular, Brushes.Black, c7I, BottomY)
                    g.DrawString(" : ", fontRegular, Brushes.Black, c8D, TopY)
                    g.DrawString(" : ", fontRegular, Brushes.Black, c8D, BottomY)
                    g.DrawString(DueDate, fontRegular, Brushes.Black, c8D2, TopY)
                    g.DrawString(DueDate, fontRegular, Brushes.Black, c8D2, BottomY)
                    TopY = TopY - 18
                    BottomY = BottomY - 18
            End Select
            TopY = TopY + 18
            BottomY = BottomY + 18
            CusArea = Trim(dtCustInfo.Rows(0).Item("City") & "").ToString & " " & Trim(dtCustInfo.Rows(0).Item("PinCode") & "").ToString
            g.DrawString(CusArea, New Font("Palatino Linotype", 7, FontStyle.Regular), Brushes.Black, c1, TopY)
            g.DrawString(CusArea, New Font("Palatino Linotype", 7, FontStyle.Regular), Brushes.Black, c1, BottomY)
            Select Case Trantype
                Case "POS"
                    If TrantypeMI = "RD" Then
                        g.DrawString("REPAIR DELIVERY", fontBoldTitle, Brushes.Black, c3, TopY)
                        g.DrawString("REPAIR DELIVERY", fontBoldTitle, Brushes.Black, c3, BottomY)
                    ElseIf TrantypeMI = "MI" Then
                        g.DrawString("MISC ISSUE", fontBoldTitle, Brushes.Black, c3, TopY)
                        g.DrawString("MISC ISSUE", fontBoldTitle, Brushes.Black, c3, BottomY)
                    Else
                        g.DrawString("  TAX INVOICE", fontBoldTitle, Brushes.Black, c3, TopY) 'SALES
                        g.DrawString("  TAX INVOICE", fontBoldTitle, Brushes.Black, c3, BottomY) 'SALES
                    End If
                Case "ORD"
                    g.DrawString("ORDER BOOKING", fontBoldTitle, Brushes.Black, c3, TopY)
                    g.DrawString("ORDER BOOKING", fontBoldTitle, Brushes.Black, c3, BottomY)
                Case "REP"
                    g.DrawString("REPAIR BOOKING", fontBoldTitle, Brushes.Black, c3, TopY)
                    g.DrawString("REPAIR BOOKING", fontBoldTitle, Brushes.Black, c3, BottomY)
            End Select
            TopY = TopY + 18
            BottomY = BottomY + 18
            If Trim(dtCustInfo.Rows(0).Item("phoneres") & "").ToString <> "" Then
                CusPhone_Mobi = Trim(dtCustInfo.Rows(0).Item("phoneres") & "").ToString & " " & Trim(dtCustInfo.Rows(0).Item("mobile") & "").ToString & " " & Trim(IIf(dtCustInfo.Rows(0).Item("PAN").ToString <> "", "PAN :" & dtCustInfo.Rows(0).Item("PAN").ToString, "")).ToString
                g.DrawString(CusPhone_Mobi, New Font("Palatino Linotype", 7, FontStyle.Regular), Brushes.Black, c1, TopY)
                g.DrawString(CusPhone_Mobi, New Font("Palatino Linotype", 7, FontStyle.Regular), Brushes.Black, c1, BottomY)
                TopY = TopY + 18
                BottomY = BottomY + 18
            Else
                CusPhone = Trim(dtCustInfo.Rows(0).Item("mobile").ToString) & " " & Trim(IIf(dtCustInfo.Rows(0).Item("PAN").ToString <> "", "PAN :" & dtCustInfo.Rows(0).Item("PAN").ToString, "")).ToString
                g.DrawString(CusPhone, New Font("Palatino Linotype", 7, FontStyle.Regular), Brushes.Black, c1, TopY)
                g.DrawString(CusPhone, New Font("Palatino Linotype", 7, FontStyle.Regular), Brushes.Black, c1, BottomY)
                TopY = TopY + 18
                BottomY = BottomY + 18
            End If
            If Trim(dtCustInfo.Rows(0).Item("GSTNO") & "").ToString <> "" Then
                CusGSTNo = dtCustInfo.Rows(0).Item("GSTNO").ToString
                g.DrawString("GSTIN : " & CusGSTNo, New Font("Palatino Linotype", 7, FontStyle.Regular), Brushes.Black, c1, TopY)
                g.DrawString("GSTIN : " & CusGSTNo, New Font("Palatino Linotype", 7, FontStyle.Regular), Brushes.Black, c1, BottomY)
                If dtInvTran.Rows.Count > 0 Then
                    Dim cusIRNNo As String = dtInvTran.Rows(0).Item("IRN").ToString
                    g.DrawString("IRN : " & cusIRNNo, New Font("Palatino Linotype", 7, FontStyle.Regular), Brushes.Black, h1 - 95, TopY)
                    g.DrawString("IRN : " & cusIRNNo, New Font("Palatino Linotype", 7, FontStyle.Regular), Brushes.Black, h1 - 95, BottomY)
                End If
                TopY = TopY + 12 '10
                BottomY = BottomY + 12 '10
            End If
            ''If dtInvTran.Rows.Count > 0 Then
            ''    Dim barcodeDesign As New BarcodeLib.Barcode.QRCode
            ''    Dim LocalTemp As String = System.Environment.GetEnvironmentVariable("temp")
            ''    Dim SystemName As String
            ''    SystemName = Environment.MachineName
            ''    BarcodeTempFileDelete(LocalTemp, SystemName, "")
            ''    barcodeDesign.Rotate = BarcodeLib.Barcode.RotateOrientation.BottomFacingDown
            ''    barcodeDesign.Data = dtInvTran.Rows(0)("QRDATA")
            ''    barcodeDesign.drawBarcode(LocalTemp & "\barcode" & SystemName & ".png")
            ''    Dim imgstream As New Bitmap(LocalTemp & "\barcode" & SystemName & ".png")
            ''    e.Graphics.DrawImage(imgstream, c6, TopY - 60, 50, 50)
            ''    e.Graphics.DrawImage(imgstream, c6, BottomY - 60, 50, 50)
            ''    imgstream.Dispose()
            ''    BarcodeTempFileDelete(LocalTemp, SystemName, "")
            ''End If
            If dtCustInfo.Rows(0).Item("STATEID").ToString <> "" Then
                Dim Qry As String = ""
                Qry = " SELECT ISNULL(STATENAME,'') STATENAME FROM " & cnAdminDb & "..STATEMAST "
                Qry += vbCrLf + " WHERE STATEID = " & dtCustInfo.Rows(0).Item("STATEID").ToString & " "
                placeofSupply = GetSqlValue(cn, Qry)
            End If
        End If
        DrawLine(g, TopY, BottomY)
    End Sub

    Public Sub BarcodeTempFileDelete(ByVal LocalTemp As String, ByVal SystemName As String, ByVal Tranno As String)
        If System.IO.File.Exists(LocalTemp & "\barcode" & SystemName & ".png") = True Then
            System.IO.File.Delete(LocalTemp & "\barcode" & SystemName & ".png")
        End If
    End Sub

    Public Sub DrawLine(ByVal g As Graphics, ByVal Y1 As Integer, ByVal Y2 As Integer)
        'c1 - > 25.00 c7 - > 770.0F
        g.DrawLine(Pens.Silver, 20.0F, Y1, 775.0F, Y1)
        g.DrawLine(Pens.Silver, 20.0F, Y2, 775.0F, Y2)
    End Sub
    Public Sub Title(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
        g.DrawString("SNo", fontRegular, Brushes.Black, c1, TopY, LeftFormat)
        g.DrawString("DESCRIPTION", fontRegular, Brushes.Black, c2, TopY, LeftFormat)
        g.DrawString("HSN", fontRegular, Brushes.Black, h1, TopY, LeftFormat)
        g.DrawString("QTY", fontRegular, Brushes.Black, c3, TopY, RightFormat)
        g.DrawString("GROSS WT", fontRegular, Brushes.Black, c4, TopY, RightFormat)
        g.DrawString("LESS WT", fontRegular, Brushes.Black, c5, TopY, RightFormat)
        g.DrawString("VA", fontRegular, Brushes.Black, c6, TopY, RightFormat) 'MC
        g.DrawString("RATE", fontRegular, Brushes.Black, c7, TopY, RightFormat)
        g.DrawString("VA", fontRegular, Brushes.Black, c8, TopY, RightFormat)
        g.DrawString("AMOUNT", fontRegular, Brushes.Black, c9, TopY, RightFormat)

        g.DrawString("SNo", fontRegular, Brushes.Black, c1, BottomY, LeftFormat)
        g.DrawString("DESCRIPTION", fontRegular, Brushes.Black, c2, BottomY, LeftFormat)
        g.DrawString("HSN", fontRegular, Brushes.Black, h1, BottomY, LeftFormat)
        g.DrawString("QTY", fontRegular, Brushes.Black, c3, BottomY, RightFormat)
        g.DrawString("GROSS WT", fontRegular, Brushes.Black, c4, BottomY, RightFormat)
        g.DrawString("LESS WT", fontRegular, Brushes.Black, c5, BottomY, RightFormat)
        g.DrawString("VA", fontRegular, Brushes.Black, c6, BottomY, RightFormat) 'MC
        g.DrawString("RATE", fontRegular, Brushes.Black, c7, BottomY, RightFormat)
        g.DrawString("VA", fontRegular, Brushes.Black, c8, BottomY, RightFormat)
        g.DrawString("AMOUNT", fontRegular, Brushes.Black, c9, BottomY, RightFormat)
        TopY = TopY + 20
        BottomY = BottomY + 20
        DrawLine(g, TopY, BottomY)
        TopY = TopY + 10
        BottomY = BottomY + 10
    End Sub
    Public Sub TitleOrderRepairBooking(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
        g.DrawString("SNo", fontRegular, Brushes.Black, c1, TopY, LeftFormat)
        g.DrawString("DESCRIPTION", fontRegular, Brushes.Black, c2, TopY, LeftFormat)
        g.DrawString("", fontRegular, Brushes.Black, c3, TopY, RightFormat)
        g.DrawString("", fontRegular, Brushes.Black, c4, TopY, RightFormat)
        g.DrawString("", fontRegular, Brushes.Black, c5, TopY, RightFormat)
        g.DrawString("QTY", fontRegular, Brushes.Black, c6, TopY, RightFormat)
        g.DrawString("", fontRegular, Brushes.Black, c7, TopY, RightFormat)
        g.DrawString("", fontRegular, Brushes.Black, c8, TopY, RightFormat)
        g.DrawString("GROSS WT", fontRegular, Brushes.Black, c9, TopY, RightFormat)

        g.DrawString("SNo", fontRegular, Brushes.Black, c1, BottomY, LeftFormat)
        g.DrawString("DESCRIPTION", fontRegular, Brushes.Black, c2, BottomY, LeftFormat)
        g.DrawString("", fontRegular, Brushes.Black, c3, BottomY, RightFormat)
        g.DrawString("", fontRegular, Brushes.Black, c4, BottomY, RightFormat)
        g.DrawString("", fontRegular, Brushes.Black, c5, BottomY, RightFormat)
        g.DrawString("QTY", fontRegular, Brushes.Black, c6, BottomY, RightFormat)
        g.DrawString("", fontRegular, Brushes.Black, c7, BottomY, RightFormat)
        g.DrawString("", fontRegular, Brushes.Black, c8, BottomY, RightFormat)
        g.DrawString("GROSS WT", fontRegular, Brushes.Black, c9, BottomY, RightFormat)
        TopY = TopY + 20
        BottomY = BottomY + 20
        DrawLine(g, TopY, BottomY)
        TopY = TopY + 10
        BottomY = BottomY + 10
    End Sub
    Public Sub TitleAdvanceReceipt(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        'ADVANCE ONLY DISPLAY
        If dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("Field1").ToString.Contains("ADVANCE ") Then
            RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
            g.DrawString("ItemId", fontBold, Brushes.Black, c1, TopY, LeftFormat)
            g.DrawString("ItemName", fontBold, Brushes.Black, c2, TopY, LeftFormat)
            g.DrawString("", fontRegular, Brushes.Black, c3, TopY, RightFormat)
            g.DrawString("", fontRegular, Brushes.Black, c4, TopY, RightFormat)
            g.DrawString("", fontRegular, Brushes.Black, c5, TopY, RightFormat)
            g.DrawString("Weight", fontBold, Brushes.Black, c6, TopY, RightFormat)
            g.DrawString("", fontRegular, Brushes.Black, c7, TopY, RightFormat)
            g.DrawString("", fontRegular, Brushes.Black, c8, TopY, RightFormat)
            g.DrawString("", fontRegular, Brushes.Black, c9, TopY, RightFormat)

            g.DrawString("ItemId", fontBold, Brushes.Black, c1, BottomY, LeftFormat)
            g.DrawString("ItemName", fontBold, Brushes.Black, c2, BottomY, LeftFormat)
            g.DrawString("", fontRegular, Brushes.Black, c3, BottomY, RightFormat)
            g.DrawString("", fontRegular, Brushes.Black, c4, BottomY, RightFormat)
            g.DrawString("", fontRegular, Brushes.Black, c5, BottomY, RightFormat)
            g.DrawString("Weight", fontBold, Brushes.Black, c6, BottomY, RightFormat)
            g.DrawString("", fontRegular, Brushes.Black, c7, BottomY, RightFormat)
            g.DrawString("", fontRegular, Brushes.Black, c8, BottomY, RightFormat)
            g.DrawString("", fontRegular, Brushes.Black, c9, BottomY, RightFormat)
            TopY = TopY + 20
            BottomY = BottomY + 20
        End If
    End Sub
    Public Sub TitleSmithIssueReceipt(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
        g.DrawString("SNo", fontRegular, Brushes.Black, c1, TopY, LeftFormat)
        g.DrawString("Description", fontRegular, Brushes.Black, c2, TopY, LeftFormat)
        g.DrawString("HSN", fontRegular, Brushes.Black, 280, TopY, LeftFormat)
        g.DrawString("PCS", fontRegular, Brushes.Black, c3, TopY, RightFormat)
        g.DrawString("Gross.Wt", fontRegular, Brushes.Black, c4, TopY, RightFormat)
        g.DrawString("Net.Wt", fontRegular, Brushes.Black, c5, TopY, RightFormat)
        g.DrawString("", fontRegular, Brushes.Black, c6, TopY, RightFormat)
        g.DrawString("Dia.Wt", fontRegular, Brushes.Black, c7, TopY, RightFormat)
        g.DrawString("Mc", fontRegular, Brushes.Black, c8, TopY, RightFormat)
        g.DrawString("Amount", fontRegular, Brushes.Black, c9, TopY, RightFormat)

        g.DrawString("SNo", fontRegular, Brushes.Black, c1, BottomY, LeftFormat)
        g.DrawString("Description", fontRegular, Brushes.Black, c2, BottomY, LeftFormat)
        g.DrawString("HSN", fontRegular, Brushes.Black, 280, BottomY, LeftFormat)
        g.DrawString("PCS", fontRegular, Brushes.Black, c3, BottomY, RightFormat) ' PCS REPLACE TO HSN
        g.DrawString("Gross.Wt", fontRegular, Brushes.Black, c4, BottomY, RightFormat)
        g.DrawString("Net.Wt", fontRegular, Brushes.Black, c5, BottomY, RightFormat)
        g.DrawString("", fontRegular, Brushes.Black, c6, BottomY, RightFormat)
        g.DrawString("Dia.Wt", fontRegular, Brushes.Black, c7, BottomY, RightFormat)
        g.DrawString("Mc", fontRegular, Brushes.Black, c8, BottomY, RightFormat)
        g.DrawString("Amount", fontRegular, Brushes.Black, c9, BottomY, RightFormat)
        TopY = TopY + 20
        BottomY = BottomY + 20
    End Sub

    Public Sub TitleSmithIssueReceipt2(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
        g.DrawString("SNo", fontRegularsmall, Brushes.Black, sm1, TopY, LeftFormat)
        g.DrawString("Description", fontRegularsmall, Brushes.Black, sm2, TopY, LeftFormat)
        g.DrawString("HSN", fontRegularsmall, Brushes.Black, sm20, TopY, LeftFormat)
        g.DrawString("Touch", fontRegularsmall, Brushes.Black, sm3, TopY, RightFormat)
        g.DrawString("Grs.Wt", fontRegularsmall, Brushes.Black, sm4, TopY, RightFormat)
        g.DrawString("Stone.Wt", fontRegularsmall, Brushes.Black, sm5, TopY, RightFormat)
        g.DrawString("Net.Wt", fontRegularsmall, Brushes.Black, sm6, TopY, RightFormat)
        g.DrawString("Purity", fontRegularsmall, Brushes.Black, sm7, TopY, RightFormat)
        g.DrawString("Pure.Wt", fontRegularsmall, Brushes.Black, sm8, TopY, RightFormat)
        g.DrawString("Wastage", fontRegularsmall, Brushes.Black, sm9, TopY, RightFormat)
        g.DrawString("MC", fontRegularsmall, Brushes.Black, sm10, TopY, RightFormat)
        g.DrawString("Amount", fontRegularsmall, Brushes.Black, sm11, TopY, RightFormat)

        g.DrawString("SNo", fontRegularsmall, Brushes.Black, sm1, BottomY, LeftFormat)
        g.DrawString("Description", fontRegularsmall, Brushes.Black, sm2, BottomY, LeftFormat)
        g.DrawString("HSN", fontRegularsmall, Brushes.Black, sm20, BottomY, LeftFormat)
        g.DrawString("Touch", fontRegularsmall, Brushes.Black, sm3, BottomY, RightFormat)
        g.DrawString("Grs.Wt", fontRegularsmall, Brushes.Black, sm4, BottomY, RightFormat)
        g.DrawString("Stone.Wt", fontRegularsmall, Brushes.Black, sm5, BottomY, RightFormat)
        g.DrawString("Net.Wt", fontRegularsmall, Brushes.Black, sm6, BottomY, RightFormat)
        g.DrawString("Purity", fontRegularsmall, Brushes.Black, sm7, BottomY, RightFormat)
        g.DrawString("Pure.Wt", fontRegularsmall, Brushes.Black, sm8, BottomY, RightFormat)
        g.DrawString("Wastage", fontRegularsmall, Brushes.Black, sm9, BottomY, RightFormat)
        g.DrawString("MC", fontRegularsmall, Brushes.Black, sm10, BottomY, RightFormat)
        g.DrawString("Amount", fontRegularsmall, Brushes.Black, sm11, BottomY, RightFormat)
        TopY = TopY + 20
        BottomY = BottomY + 20
    End Sub
    Public Sub TitleSmithIssueReceiptSmithItemNameA4(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
        g.DrawString("SNo", fontRegularsmall, Brushes.Black, smI1, TopY, LeftFormat)
        g.DrawString("Description", fontRegularsmall, Brushes.Black, smI2, TopY, LeftFormat)
        g.DrawString("HSN", fontRegularsmall, Brushes.Black, smI20, TopY, LeftFormat)
        g.DrawString("Touch", fontRegularsmall, Brushes.Black, smI3, TopY, RightFormat)
        g.DrawString("PCS", fontRegularsmall, Brushes.Black, smI21, TopY, RightFormat)
        g.DrawString("Grs.Wt", fontRegularsmall, Brushes.Black, smI4, TopY, RightFormat)
        g.DrawString("Stone.Wt", fontRegularsmall, Brushes.Black, smI5, TopY, RightFormat)
        g.DrawString("Net.Wt", fontRegularsmall, Brushes.Black, smI6, TopY, RightFormat)
        g.DrawString("Purity", fontRegularsmall, Brushes.Black, smI7, TopY, RightFormat)
        g.DrawString("Pure.Wt", fontRegularsmall, Brushes.Black, smI8, TopY, RightFormat)
        g.DrawString("Yield", fontRegularsmall, Brushes.Black, smI9, TopY, RightFormat)
        g.DrawString(Space(1) & "/Processing Loss", fontRegularsmall, Brushes.Black, smI9 - 40, TopY + 10, LeftFormat)
        g.DrawString("MC", fontRegularsmall, Brushes.Black, smI10, TopY, RightFormat)
        g.DrawString("Amount", fontRegularsmall, Brushes.Black, smI11, TopY, RightFormat)
        TopY = TopY + 38
    End Sub
    Public Sub TitleSmithIssueReceiptSmithCatNameA4(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
        g.DrawString("SNo", fontRegularsmall, Brushes.Black, smc1, TopY, LeftFormat)
        g.DrawString("Description", fontRegularsmall, Brushes.Black, smc2, TopY, LeftFormat)
        g.DrawString("HSN", fontRegularsmall, Brushes.Black, smc3, TopY, LeftFormat)
        g.DrawString("PCS", fontRegularsmall, Brushes.Black, smc10, TopY, RightFormat)
        g.DrawString("Grs.Wt", fontRegularsmall, Brushes.Black, smc4, TopY, RightFormat)
        g.DrawString("Stone.Wt", fontRegularsmall, Brushes.Black, smc5, TopY, RightFormat)
        g.DrawString("Net.Wt", fontRegularsmall, Brushes.Black, smc6, TopY, RightFormat)
        'g.DrawString("Wastage", fontRegularsmall, Brushes.Black, smc7, TopY, RightFormat)
        g.DrawString("Yield", fontRegularsmall, Brushes.Black, smc7, TopY, RightFormat)
        g.DrawString(Space(1) & "/Processing Loss", fontRegularsmall, Brushes.Black, smc7 - 40, TopY + 15, LeftFormat)
        g.DrawString("MC", fontRegularsmall, Brushes.Black, smc8, TopY, RightFormat)
        g.DrawString("Amount", fontRegularsmall, Brushes.Black, smc9, TopY, RightFormat)
        TopY = TopY + 38
    End Sub

    Public Sub FooterSmithIssueReceipt(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        g.DrawString("COPY  ", New Font("Palatino Linotype", 8, FontStyle.Bold), Brushes.Black, c6, 600)
        If dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count > 0 Then
            Dim UserName As String = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("USERNAME").ToString
            g.DrawLine(Pens.Silver, 20.0F, 230.0F, 20.0F, 445.0F) 'LEFT TOP
            g.DrawLine(Pens.Silver, 775.0F, 230.0F, 775.0F, 445.0F) 'RIGHT TOP

            g.DrawLine(Pens.Silver, 20.0F, 800.0F, 20.0F, 1000.0F) 'LEFT BOTTOM
            g.DrawLine(Pens.Silver, 775.0F, 800.0F, 775.0F, 1000.0F) 'RIGHT BOTTOM
            ''
            '' Vertical Line
            g.DrawLine(Pens.Silver, 20.0F, 445.0F, 775.0F, 445.0F)
            ''If ImgPrint.Length > 0 Then
            ''    g.DrawString("For New Saravana Stores Bramandamai", New Font("Times New Roman", 8, FontStyle.Regular), Brushes.Black, 600, 442)
            ''    Dim Image1 As Image = Image.FromStream(ImgPrint)
            ''    e.Graphics.DrawImage(Image1, 620, 452, 100, 70)
            ''End If
            g.DrawString(Rules(0).ToString, fontRegular, Brushes.Black, c1, 450)
            g.DrawString(Rules(1).ToString, fontRegular, Brushes.Black, c1, 465)
            g.DrawString("Entered By ", fontRegular, Brushes.Black, c1, 495)
            g.DrawString(UserName, fontRegular, Brushes.Black, c1, 510)
            g.DrawString("Receiver Signature", fontRegular, Brushes.Black, 250, 495)
            g.DrawString("Verified By", fontRegular, Brushes.Black, c5, 495)
            g.DrawString("Authorised Signatory", fontRegular, Brushes.Black, c7, 495)
            g.DrawString("Tax is Payable on Reverse Charge : YES/NO ", fontRegular, Brushes.Black, c5, 515)

            g.DrawLine(Pens.Silver, 20.0F, 1000.0F, 775.0F, 1000.0F)
            ''If ImgPrint.Length > 0 Then
            ''    g.DrawString("For New Saravana Stores Bramandamai", New Font("Times New Roman", 8, FontStyle.Regular), Brushes.Black, 600, 997)
            ''    Dim Image1 As Image = Image.FromStream(ImgPrint)
            ''    e.Graphics.DrawImage(Image1, 620, 1007, 100, 70)
            ''End If
            g.DrawString(Rules(0).ToString, fontRegular, Brushes.Black, c1, 1005)
            g.DrawString(Rules(1).ToString, fontRegular, Brushes.Black, c1, 1020)
            g.DrawString("Entered By", fontRegular, Brushes.Black, c1, 1050)
            g.DrawString(UserName, fontRegular, Brushes.Black, c1, 1070)
            g.DrawString("Receiver Signature", fontRegular, Brushes.Black, 250, 1050)
            g.DrawString("Verified By", fontRegular, Brushes.Black, c5, 1050)
            g.DrawString("Authorised Signatory", fontRegular, Brushes.Black, c7, 1050)
            g.DrawString("Tax is Payable on Reverse Charge : YES/NO ", fontRegular, Brushes.Black, c5, 1080)
        End If
        ''
    End Sub

    Public Sub FooterSmithIssueReceiptA4(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal y1 As Integer, ByVal y2 As Integer)
        If dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count > 0 Then
            Dim UserName As String = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("USERNAME").ToString
            g.DrawLine(Pens.Silver, 20.0F, y1, 20.0F, y2)
            g.DrawLine(Pens.Silver, 775.0F, y1, 775.0F, y2)
            ''If ImgPrint.Length > 0 Then
            ''    g.DrawString("For " & strCompanyName, New Font("Times New Roman", 7, FontStyle.Regular), Brushes.Black, 450, y2 + 18)
            ''    Dim Image1 As Image = Image.FromStream(ImgPrint)
            ''    e.Graphics.DrawImage(Image1, 620, y2 + 35, 100, 70)
            ''End If
            g.DrawString(Rules(0).ToString, fontRegular, Brushes.Black, c1, y2)
            y2 = y2 + 18
            g.DrawString(Rules(1).ToString, fontRegular, Brushes.Black, c1, y2)
            y2 = y2 + 18
            y2 = y2 + 18
            Dim PrintName As String = ""
            If dtPwdMasterValue.Rows.Count > 0 Then
                PrintName = "" + BrighttechPack.Decrypt(dtPwdMasterValue.Rows(0).Item("PWD").ToString) & "-" & dtPwdMasterValue.Rows(0).Item("PWDMOBILENO").ToString & "-" & dtPwdMasterValue.Rows.Count
            End If
            g.DrawString(PrintName, fontRegular7, Brushes.Black, c1, y2)
            y2 = y2 + 12
            g.DrawString("Entered By ", fontRegular, Brushes.Black, c1, y2)
            g.DrawString("Receiver Signature", fontRegular, Brushes.Black, 250, y2)
            g.DrawString("Verified By", fontRegular, Brushes.Black, c5, y2)
            g.DrawString("Authorised Signatory", fontRegular, Brushes.Black, c7, y2)
            y2 = y2 + 18
            g.DrawString(UserName, fontRegular, Brushes.Black, c1, y2)
            g.DrawString("Tax is Payable on Reverse Charge : YES/NO ", fontRegular, Brushes.Black, c5, y2)
            y2 = y2 + 18
        End If
    End Sub

    Public Sub Footer(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        g.DrawString("C O P Y  ", fontBold, Brushes.Black, c8, 600)
        Dim T1 As String = "Page " & NoofPrint.ToString

        '' Horizontal Line
        g.DrawLine(Pens.Silver, 20.0F, 207.0F, 20.0F, 490.0F) 'LEFT TOP
        g.DrawLine(Pens.Silver, 775.0F, 207.0F, 775.0F, 490.0F) 'RIGHT TOP

        g.DrawLine(Pens.Silver, 20.0F, 777.0F, 20.0F, 1060.0F) 'LEFT BOTTOM
        g.DrawLine(Pens.Silver, 775.0F, 777.0F, 775.0F, 1060.0F) 'RIGHT BOTTOM
        ''
        '' Vertical Line
        Select Case Trantype
            Case "REP"
                g.DrawString("NOTE : ITEM(S) MENTIONED ABOVE WILL BE DELIVERED ONLY BY PRODUCING ORIGINAL VOUCHER", fontBold, Brushes.Black, c2, 470)
            Case "ORD"
                g.DrawString("NOTE : ORIGINAL ORDER BOOKING RECEIPT SHOULD BE PRODUCED AT THE TIME OF ITEM(S) DELIVERY", fontBold, Brushes.Black, c2, 470)
            Case Else
        End Select
        g.DrawLine(Pens.Silver, 20.0F, 490.0F, 775.0F, 490.0F)
        Select Case Trantype
            Case "POS"
                ' g.DrawString("PCS ITEM No Exchange", New Font("Palatino Linotype", 7, FontStyle.Regular), Brushes.Black, c1, 490)
        End Select
        g.DrawString("E&O.E     Tax is Payable on Reverse Charge : NO ", fontRegular, Brushes.Black, c1, 500)
        g.DrawString(NodeId, fontRegular, Brushes.Black, c3, 500)
        If placeofSupply <> "" Then
            g.DrawString("PLACE OF SUPPLY : " & placeofSupply, fontRegular, Brushes.Black, c3 + 50, 500)
        End If
        g.DrawString("Cashier", fontRegular, Brushes.Black, c7, 500)
        ''If ImgPrint.Length > 0 Then
        ''    g.DrawString("For New Saravana Stores Bramandamai", New Font("Times New Roman", 8, FontStyle.Regular), Brushes.Black, c6, 520)
        ''    Dim Image1 As Image = Image.FromStream(ImgPrint)
        ''    e.Graphics.DrawImage(Image1, 600, 530, 100, 70)
        ''End If
        g.DrawString(Rules(0).ToString, New Font("Palatino Linotype", 8, FontStyle.Regular), Brushes.Black, c1, 515)
        g.DrawString(Rules(1).ToString, New Font("Palatino Linotype", 8, FontStyle.Regular), Brushes.Black, c1, 530)
        If NoofPrint > 1 Then
            g.DrawString(T1, fontRegular, Brushes.Black, c8, 500)
        End If
        Select Case Trantype
            Case "REP"
                g.DrawString("NOTE : ITEM(S) MENTIONED ABOVE WILL BE DELIVERED ONLY BY PRODUCING ORIGINAL VOUCHER", fontBold, Brushes.Black, c2, 1040)
            Case "ORD"
                g.DrawString("NOTE : ORIGINAL ORDER BOOKING RECEIPT SHOULD BE PRODUCED AT THE TIME OF ITEM(S) DELIVERY", fontBold, Brushes.Black, c2, 1040)
            Case Else
        End Select
        g.DrawLine(Pens.Silver, 20.0F, 1060.0F, 775.0F, 1060.0F)
        Select Case Trantype
            Case "POS"
                ' g.DrawString("PCS ITEM No Exchange", New Font("Palatino Linotype", 7, FontStyle.Regular), Brushes.Black, c1, 1053)
        End Select
        g.DrawString("E&O.E      Tax is Payable on Reverse Charge : NO ", fontRegular, Brushes.Black, c1, 1063)
        g.DrawString(NodeId, fontRegular, Brushes.Black, c3, 1063)
        If placeofSupply <> "" Then
            g.DrawString("PLACE OF SUPPLY : " & placeofSupply, fontRegular, Brushes.Black, c3 + 50, 1063)
        End If
        g.DrawString("Cashier", fontRegular, Brushes.Black, c7, 1063)
        ''If ImgPrint.Length > 0 Then
        ''    g.DrawString("For New Saravana Stores Bramandamai", New Font("Times New Roman", 8, FontStyle.Regular), Brushes.Black, c6, 1083)
        ''    Dim Image1 As Image = Image.FromStream(ImgPrint)
        ''    e.Graphics.DrawImage(Image1, 600, 1093, 100, 70)
        ''End If
        g.DrawString(Rules(0).ToString, New Font("Palatino Linotype", 8, FontStyle.Regular), Brushes.Black, c1, 1073)
        g.DrawString(Rules(1).ToString, New Font("Palatino Linotype", 8, FontStyle.Regular), Brushes.Black, c1, 1083)
        If NoofPrint > 1 Then
            g.DrawString(T1, fontRegular, Brushes.Black, c8, 1063)
        End If
    End Sub
#End Region
#Region "Function"
    Public Function GetSqlValue_Bill(ByVal qry As String, Optional ByVal field As String = Nothing, Optional ByVal defValue As String = "", Optional ByRef tran As OleDbTransaction = Nothing) As String
        Dim G_DTable As New DataTable
        Dim G_DAdapter As New OleDbDataAdapter
        'G_DTable = New DataTable
        If tran Is Nothing Then
            G_DAdapter = New OleDbDataAdapter(qry, cn)
            G_DAdapter.Fill(G_DTable)
        Else
            cmd = New OleDbCommand(qry, cn, tran)
            G_DAdapter = New OleDbDataAdapter(cmd)
            G_DAdapter.Fill(G_DTable)
        End If
        If field <> "" Then
            If G_DTable.Rows.Count > 0 Then Return G_DTable.Rows(0).Item(field).ToString
        Else
            If G_DTable.Rows.Count > 0 Then Return G_DTable.Rows(0).Item(0).ToString
        End If
        Return defValue
    End Function
#End Region
#Region "Estimation Printing"

    Public Sub salesEstimation()
        'strsql = "SELECT PRINTERPATH,PORT FROM " & cnAdminDb & "..PRINTERLIST WHERE PRINTERNAME = '" & Environment.MachineName & "' "
        'Dim dr As DataRow = Nothing
        'dr = GetSqlRow(strsql, cn)
        'If Not dr Is Nothing Then
        '    Dim centPrint As New frmCentPrint(dr.Item("PRINTERPATH").ToString, Val(dr.Item("PORT").ToString) _
        '    , System.IO.Path.GetTempPath() & "\centprint" & SystemName & " .mem" _
        '    , strBilldate _
        '    , EstNo_SA)
        'Else
        '    MsgBox("System Id not in PrinterList master", MsgBoxStyle.Information)
        '    Exit Sub
        'End If
    End Sub


    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Using g As Graphics = e.Graphics
            Dim dtBillPrint As New DataTable
            Dim brush As New SolidBrush(Color.Black)
            Dim pen As New Pen(brush)
            Dim fontRegular As New Font("Times New Roman", 9, FontStyle.Regular)
            Dim fontBold As New Font("Times New Roman", 9, FontStyle.Bold)
            Dim fontProduct As New Font("Times New Roman", 9, FontStyle.Regular)
            Dim fontBig As New Font("Times New Roman", 15, FontStyle.Bold)
            Dim size As SizeF
            Dim rAlign As New StringFormat
            Dim lAlign As New StringFormat
            rAlign.Alignment = StringAlignment.Far
            lAlign.Alignment = StringAlignment.Near
            Dim c1 As Integer = 10
            Dim c2 As Integer = 150
            Dim c3 As Integer = 190
            Dim c4 As Integer = 230
            Dim c5 As Integer = 300
            Dim c6 As Integer = 350
            Dim y1 As Integer = 20
            Dim _strDesc As String = ""
            Dim strTranno As String = ""
            If Val(EstNo_SA) <> 0 Then
                c1 = 10
                c2 = 80
                c3 = 90
                c4 = 140
                c5 = 190
                c6 = 270
                y1 = 20
                strsql = " SELECT * "
                strsql += vbCrLf + " FROM " & cnStockDb & "..ESTISSUE AS T "
                strsql += vbCrLf + " WHERE "
                strsql += vbCrLf + " TRANDATE='" & strBilldate & "' "
                strsql += vbCrLf + " AND TRANNO='" & EstNo_SA & "' "
                dtBillPrint = New DataTable
                cmd = New OleDbCommand(strsql, cn)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtBillPrint)
                If dtBillPrint.Rows.Count = 0 Then Exit Sub
                For k As Integer = 0 To dtBillPrint.Rows.Count - 1
                    _strDesc = ""
                    y1 = y1 + 35
                    g.DrawString(strTranno.ToString(), New Font("Times New Roman", 7, FontStyle.Bold), brush, 10, y1)
                    y1 = y1 + 20
                    g.DrawString("ESTIMATE", New Font("Times New Roman", 12, FontStyle.Bold), brush, 40, y1)
                    y1 = y1 + 30
                    g.DrawString("EST No", fontRegular, brush, c1, y1)
                    g.DrawString(":", fontRegular, brush, c2, y1)
                    g.DrawString(EstNo_SA.ToString, fontRegular, brush, c3, y1)
                    y1 = y1 + 20
                    strsql = " SELECT TOP 1 SUBSTRING(CONVERT(VARCHAR,UPTIME,109),13,5)+SUBSTRING(CONVERT(VARCHAR,UPTIME,109),25,5) AS BILLTIME, "
                    strsql += vbCrLf + " CONVERT(VARCHAR,TRANNO) AS TRANNO,CONVERT(VARCHAR(12),TRANDATE,103) AS TRANDATE "
                    strsql += vbCrLf + " FROM " & cnStockDb & "..ESTISSUE AS T WHERE "
                    strsql += vbCrLf + " TRANDATE='" & strBilldate & "' AND TRANNO='" & EstNo_SA & "' "
                    Dim dtTot As New DataTable
                    cmd = New OleDbCommand(strsql, cn)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dtTot)
                    Dim strTot As String = ""
                    If dtTot.Rows.Count > 0 Then
                        g.DrawString("Date", fontRegular, brush, c1, y1)
                        g.DrawString(":", fontRegular, brush, c2, y1)
                        g.DrawString(dtTot.Rows(0).Item("TRANDATE").ToString + "  " + dtTot.Rows(0).Item("BILLTIME").ToString, fontRegular, brush, c3, y1)
                        y1 = y1 + 20
                    End If
                    g.DrawString("TAGNO", fontRegular, brush, c1, y1)
                    g.DrawString(":", fontRegular, brush, c2, y1)
                    g.DrawString(dtBillPrint.Rows(k)("ITEMID").ToString + "-" + dtBillPrint.Rows(k)("TAGNO").ToString, fontRegular, brush, c3, y1)
                    y1 = y1 + 20
                    strsql = " SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST "
                    strsql += vbCrLf + " WHERE ITEMID='" & dtBillPrint.Rows(k)("ITEMID").ToString & "' "
                    strsql += vbCrLf + " AND SUBITEMID='" & dtBillPrint.Rows(k)("SUBITEMID").ToString & "' "
                    _strDesc = GetSqlValue_Bill(strsql)
                    If _strDesc = "" Then
                        strsql = " SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
                        strsql += vbCrLf + " WHERE ITEMID='" & dtBillPrint.Rows(k)("ITEMID").ToString & "' "
                        _strDesc = GetSqlValue_Bill(strsql)
                    End If
                    g.DrawString("DESC", fontRegular, brush, c1, y1)
                    g.DrawString(":", fontRegular, brush, c2, y1)
                    g.DrawString(_strDesc, fontRegular, brush, c3, y1)
                    y1 = y1 + 20
                    g.DrawString("RATE", fontRegular, brush, c1, y1)
                    g.DrawString(":", fontRegular, brush, c2, y1)
                    g.DrawString(dtBillPrint.Rows(k)("BOARDRATE").ToString + "/gm", fontRegular, brush, c3, y1)
                    y1 = y1 + 20
                    g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                    y1 = y1 + 20
                    If Val(dtBillPrint.Rows(k)("GRSWT").ToString) <> Val(dtBillPrint.Rows(k)("NETWT").ToString) Then
                        g.DrawString("GROSSWT", fontRegular, brush, c1, y1)
                        g.DrawString(":", fontRegular, brush, c2, y1)
                        g.DrawString(Format(Val(dtBillPrint.Rows(k)("GRSWT").ToString), "0.000"), fontRegular, brush, c4, y1, rAlign)
                        If Val(dtBillPrint.Rows(k)("GRSWT").ToString) <> Val(dtBillPrint.Rows(k)("TAGGRSWT").ToString) Then
                            g.DrawString(Format(Val(dtBillPrint.Rows(k)("TAGGRSWT").ToString), "0.000"), fontRegular, brush, c5, y1, rAlign)
                        End If
                        y1 = y1 + 20
                        g.DrawString("NETWT", fontRegular, brush, c1, y1)
                        g.DrawString(":", fontRegular, brush, c2, y1)
                        g.DrawString(Format(Val(dtBillPrint.Rows(k)("NETWT").ToString), "0.000"), fontRegular, brush, c4, y1, rAlign)
                        y1 = y1 + 20
                    Else
                        g.DrawString("GROSSWT", fontRegular, brush, c1, y1)
                        g.DrawString(":", fontRegular, brush, c2, y1)
                        g.DrawString(Format(Val(dtBillPrint.Rows(k)("GRSWT").ToString), "0.000"), fontRegular, brush, c4, y1, rAlign)
                        If Val(dtBillPrint.Rows(k)("GRSWT").ToString) <> Val(dtBillPrint.Rows(k)("TAGGRSWT").ToString) Then
                            g.DrawString(Format(Val(dtBillPrint.Rows(k)("TAGGRSWT").ToString), "0.000"), fontRegular, brush, c5, y1, rAlign)
                        End If
                        y1 = y1 + 20
                    End If
                    If Val(dtBillPrint.Rows(k)("WASTAGE").ToString) <> 0 Then
                        g.DrawString("VA (" + dtBillPrint.Rows(k)("WASTPER").ToString +
                        IIf(Val(dtBillPrint.Rows(k)("WASTPER").ToString) <> 0, "%", "") + ")", fontRegular, brush, c1, y1)
                        g.DrawString(":", fontRegular, brush, c2, y1)
                        g.DrawString(Format(Val(dtBillPrint.Rows(k)("WASTAGE").ToString), "0.000"), fontRegular, brush, c4, y1, rAlign)
                        y1 = y1 + 20
                    End If
                    g.DrawLine(Pens.Black, 80.0F, y1, 300.0F, y1)
                    y1 = y1 + 20
                    If dtBillPrint.Rows(k)("GRSNET").ToString = "G" Then
                        g.DrawString(Format(Val(dtBillPrint.Rows(k)("GRSWT").ToString) + Val(dtBillPrint.Rows(k)("WASTAGE").ToString), "0.000") _
                        , fontRegular, brush, c4, y1, rAlign)
                        g.DrawString(dtBillPrint.Rows(k)("RATE").ToString, fontRegular, brush, c5, y1, rAlign)
                        g.DrawString(Format(Val(dtBillPrint.Rows(k)("AMOUNT").ToString) - Val(dtBillPrint.Rows(k)("STNAMT").ToString), "0.00") _
                        , fontRegular, brush, c6, y1, rAlign)
                        y1 = y1 + 20
                    Else
                        g.DrawString(Format(Val(dtBillPrint.Rows(k)("NETWT").ToString) + Val(dtBillPrint.Rows(k)("WASTAGE").ToString), "0.000") _
                        , fontRegular, brush, c4, y1, rAlign)
                        g.DrawString(dtBillPrint.Rows(k)("RATE").ToString, fontRegular, brush, c5, y1, rAlign)
                        g.DrawString(Format(Val(dtBillPrint.Rows(k)("AMOUNT").ToString) - Val(dtBillPrint.Rows(k)("STNAMT").ToString), "0.00") _
                        , fontRegular, brush, c6, y1, rAlign)
                        y1 = y1 + 20
                    End If
                    strsql = " SELECT * FROM " & cnStockDb & "..ESTISSSTONE AS E WHERE ISSSNO='" & dtBillPrint.Rows(k)("SNO").ToString & "' "
                    Dim dtBillPrintStone As DataTable
                    dtBillPrintStone = New DataTable
                    cmd = New OleDbCommand(strsql, cn)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dtBillPrintStone)
                    For i As Integer = 0 To dtBillPrintStone.Rows.Count - 1
                        strsql = " SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST "
                        strsql += vbCrLf + " WHERE ITEMID='" & dtBillPrintStone.Rows(i)("STNITEMID").ToString & "' "
                        strsql += vbCrLf + " AND SUBITEMID='" & dtBillPrintStone.Rows(i)("STNSUBITEMID").ToString & "' "
                        _strDesc = GetSqlValue_Bill(strsql)
                        If _strDesc = "" Then
                            strsql = " SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
                            strsql += vbCrLf + " WHERE ITEMID='" & dtBillPrintStone.Rows(i)("STNITEMID").ToString & "' "
                            _strDesc = GetSqlValue_Bill(strsql)
                        End If
                        If Val(dtBillPrintStone.Rows(i)("STNAMT").ToString) <> 0 Then
                            g.DrawString(_strDesc, fontRegular, brush, c1, y1)
                            g.DrawString(Format(Val(dtBillPrintStone.Rows(i)("STNWT").ToString), "#0.000"), fontRegular, brush, c4, y1, rAlign)
                            g.DrawString(Format(Val(dtBillPrintStone.Rows(i)("STNRATE").ToString), "0.00"), fontRegular, brush, c5, y1, rAlign)
                            g.DrawString(Format(Val(dtBillPrintStone.Rows(i)("STNAMT").ToString), "0.00"), fontRegular, brush, c6, y1, rAlign)
                            y1 = y1 + 20
                        End If
                    Next
                    g.DrawLine(Pens.Black, 190.0F, y1, 300.0F, y1)
                    y1 = y1 + 20
                    g.DrawString("AMOUNT", fontRegular, brush, c1, y1)
                    g.DrawString(dtBillPrint.Rows(k)("AMOUNT").ToString, fontRegular, brush, c6, y1, rAlign)
                    y1 = y1 + 20
                    g.DrawString("GST", fontRegular, brush, c1, y1)
                    g.DrawString(dtBillPrint.Rows(k)("TAX").ToString, fontRegular, brush, c6, y1, rAlign)
                    y1 = y1 + 20
                    g.DrawString("TOTAL", fontRegular, brush, c1, y1)
                    g.DrawString(Format(Val(dtBillPrint.Rows(k)("AMOUNT").ToString) + Val(dtBillPrint.Rows(k)("TAX").ToString), "0.00"), fontRegular, brush, c6, y1, rAlign)
                    y1 = y1 + 20
                    g.DrawLine(Pens.Black, 190.0F, y1, 300.0F, y1)
                    y1 = y1 + 40
                Next
            End If
            If Val(EstNo_PU) <> 0 Then
                strsql = " SELECT "
                strsql += vbCrLf + " (SELECT TOP 1 CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=T.CATCODE)CATNAME,"
                strsql += vbCrLf + " T.GRSWT,T.WASTAGE,MCHARGE,AMOUNT,T.DUSTWT,RATE,EMPID,USERID,SNO,PUREXCH,T.WASTPER"
                '
                strsql += vbCrLf + " ,T.OTHERAMT "
                strsql += vbCrLf + " FROM " & cnStockDb & "..ESTRECEIPT AS T "
                strsql += vbCrLf + " WHERE "
                strsql += vbCrLf + " TRANDATE='" & strBilldate & "' AND TRANNO='" & EstNo_PU & "' AND TRANTYPE='PU' "
                dtBillPrint = New DataTable
                cmd = New OleDbCommand(strsql, cn)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtBillPrint)
                If dtBillPrint.Rows.Count = 0 Then Exit Sub
                y1 = y1 + 35
                g.DrawString(strTranno.ToString(), New Font("Times New Roman", 7, FontStyle.Bold), brush, 10, y1)
                y1 = y1 + 20
                g.DrawString("PURCHASE ESTIMATE", New Font("Times New Roman", 16, FontStyle.Bold), brush, 10, y1)
                y1 = y1 + 35

                strsql = " SELECT TOP 1 SUBSTRING(CONVERT(VARCHAR,UPTIME,109),13,5)+SUBSTRING(CONVERT(VARCHAR,UPTIME,109),25,5) AS BILLTIME, "
                strsql += vbCrLf + " CONVERT(VARCHAR,TRANNO) AS TRANNO,CONVERT(VARCHAR(12),TRANDATE,103) AS TRANDATE "
                strsql += vbCrLf + " FROM " & cnStockDb & "..ESTRECEIPT AS T WHERE "
                strsql += vbCrLf + " TRANDATE='" & strBilldate & "' AND TRANNO='" & EstNo_PU & "' AND TRANTYPE='PU' "
                Dim dtTot As New DataTable
                cmd = New OleDbCommand(strsql, cn)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtTot)
                Dim strTot As String = ""
                If dtTot.Rows.Count > 0 Then
                    strTot = "Date :" + dtTot.Rows(0).Item("TRANDATE").ToString + "  " + dtTot.Rows(0).Item("BILLTIME").ToString
                    g.DrawString(strTot.ToString(), fontRegular, brush, 10, y1)
                    y1 = y1 + 20
                    strTot = "EST No :" + dtTot.Rows(0).Item("TRANNO").ToString
                    g.DrawString(strTot, fontRegular, brush, c1, y1, lAlign)
                End If
                y1 = y1 + 20
                strTot = ""
                strsql = vbCrLf + " SELECT "
                strsql += vbCrLf + " TOP 1 SRATE"
                strsql += vbCrLf + " FROM " & cnAdminDb & "..RATEMAST WHERE RDATE <= '" & strBilldate & "' AND PURITY='91.6' AND METALID IN ('G')"
                strsql += vbCrLf + " ORDER BY RATEGROUP DESC"
                dtTot = New DataTable
                cmd = New OleDbCommand(strsql, cn)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtTot)
                If dtTot.Rows.Count > 0 Then
                    strTot += "Gold : " + dtTot.Rows(0).Item("SRATE").ToString
                End If
                strsql = vbCrLf + " SELECT "
                strsql += vbCrLf + " TOP 1 SRATE"
                strsql += vbCrLf + " FROM " & cnAdminDb & "..RATEMAST WHERE RDATE <= '" & strBilldate & "' AND PURITY='91.6' AND METALID IN ('S')"
                strsql += vbCrLf + " ORDER BY RATEGROUP DESC"
                dtTot = New DataTable
                cmd = New OleDbCommand(strsql, cn)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtTot)
                If dtTot.Rows.Count > 0 Then
                    strTot += " Silver : " + dtTot.Rows(0).Item("SRATE").ToString
                End If
                g.DrawString(strTot, fontRegular, brush, c1, y1, lAlign)

                y1 = y1 + 20
                g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                y1 = y1 + 10
                g.DrawString("DESCRIPT", fontBold, brush, c1, y1, lAlign)
                g.DrawString("WEIGHT", fontBold, brush, c2, y1, rAlign)
                g.DrawString("DUST", fontBold, brush, c3, y1, rAlign)
                g.DrawString("WAST", fontBold, brush, c4, y1, rAlign)
                g.DrawString("AMOUNT", fontBold, brush, c5, y1, rAlign)
                y1 = y1 + 20
                g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                y1 = y1 + 20
                Dim Type As String
                If dtBillPrint.Rows(0).Item("PUREXCH").ToString = "P" Then
                    Type = "CASH PURCHASE"
                ElseIf dtBillPrint.Rows(0).Item("PUREXCH").ToString = "N" Then
                    Type = "NEW EXCHANGE"
                Else
                    Type = "OLD EXCHANGE"
                End If
                g.DrawString(Type, fontBold, brush, c1, y1, lAlign)
                y1 = y1 + 20
                For i As Integer = 0 To dtBillPrint.Rows.Count - 1
                    With dtBillPrint.Rows(i)
                        'If Val(.Item("DUSTWT").ToString()) <> 0 Then
                        g.DrawString(.Item("CATNAME").ToString(), fontRegular, brush, c1, y1, lAlign)
                        g.DrawString(Format(Val(.Item("RATE").ToString()), "0.00"), fontProduct, brush, c3, y1, rAlign)
                        y1 = y1 + 25
                        'End If
                        'If Val(.Item("DUSTWT").ToString()) = 0 Then g.DrawString(.Item("CATNAME").ToString(), fontRegular, brush, c1, y1, lAlign)
                        g.DrawString(Format(Val(.Item("GRSWT").ToString()), "0.000"), fontProduct, brush, c2, y1, rAlign)
                        If Val(.Item("WASTAGE").ToString()) > 0 And Type = "NEW EXCHANGE" Then
                            Dim _strDec As String()
                            Dim WastPer As Integer = Format(Val(.Item("WASTPER").ToString()), 0)
                            _strDec = .Item("WASTPER").ToString().Split(".")
                            If _strDec.Length > 1 Then
                                If WastPer > 0 Then
                                    If Val(_strDec(1).ToString) > 0 Then
                                        g.DrawString(Format(Val(.Item("WASTPER").ToString()), "0.00").ToString + "%", fontRegular, brush, c3, y1, rAlign)
                                    Else
                                        g.DrawString(Format(Val(.Item("WASTPER").ToString()), "0").ToString + "%", fontRegular, brush, c3, y1, rAlign)
                                    End If
                                    'Else
                                    'g.DrawString(Format(Val(.Item("WASTPER").ToString()), "0").ToString + "%", fontRegular, brush, c3, y1, rAlign)
                                End If
                            End If
                            g.DrawString("+" & Format(Val(.Item("WASTAGE").ToString()), "0.000"), fontRegular, brush, c4, y1, rAlign)
                            y1 = y1 + 15
                            g.DrawString("+" & Format(Val(.Item("OTHERAMT").ToString()), "0.00"), fontRegular, brush, c4, y1, rAlign)
                            y1 = y1 - 15
                        Else
                            g.DrawString(Format(Val(.Item("DUSTWT").ToString()), "0.000"), fontRegular, brush, c3, y1, rAlign)
                            g.DrawString(Format(Val(.Item("WASTAGE").ToString()), "0.000"), fontRegular, brush, c4, y1, rAlign)
                            If Val(.Item("OTHERAMT").ToString) > 0 Then
                                y1 = y1 + 15
                                g.DrawString("+" & Format(Val(.Item("OTHERAMT").ToString()), "0.00"), fontRegular, brush, c4, y1, rAlign)
                                y1 = y1 - 15
                            End If
                        End If
                        g.DrawString(Format(Val(.Item("AMOUNT").ToString()), "0.00"), fontRegular, brush, c5, y1, rAlign)
                        y1 = y1 + 25
                        Dim dtPuStone As New DataTable
                        strsql = vbCrLf + " SELECT "
                        strsql += vbCrLf + " (SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.STNITEMID)ITEMNAME,"
                        strsql += vbCrLf + " (SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=T.STNSUBITEMID)SUBITEMNAME,"
                        strsql += vbCrLf + " STNWT,STNRATE,STNAMT,STONEUNIT FROM " & cnStockDb & "..ESTRECEIPTSTONE AS T WHERE ISSSNO = '" & .Item("SNO").ToString() & "' "
                        cmd = New OleDbCommand(strsql, cn)
                        da = New OleDbDataAdapter(cmd)
                        da.Fill(dtPuStone)
                        For K As Integer = 0 To dtPuStone.Rows.Count - 1
                            If dtPuStone.Rows(K).Item("STONEUNIT").ToString = "C" Then
                                g.DrawString(Format(Val(dtPuStone.Rows(K).Item("STNWT").ToString()), "0.00").ToString & " ct", fontProduct, brush, c2, y1, rAlign)
                            Else
                                g.DrawString(Format(Val(dtPuStone.Rows(K).Item("STNWT").ToString()), "0.00").ToString & " gm", fontProduct, brush, c2, y1, rAlign)
                            End If
                            g.DrawString(Format(Val(dtPuStone.Rows(K).Item("STNRATE").ToString()), "0.00"), fontRegular, brush, c4, y1, rAlign)
                            y1 = y1 + 25
                        Next
                    End With
                Next
                y1 = y1 + 20
                g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                y1 = y1 + 10
                g.DrawString("TOTAL", fontBold, brush, c1, y1, lAlign)
                g.DrawString(dtBillPrint.Compute("SUM(GRSWT)", "").ToString(), fontBold, brush, c2, y1, rAlign)
                g.DrawString(dtBillPrint.Compute("SUM(DUSTWT) ", "").ToString(), fontBold, brush, c3, y1, rAlign)
                g.DrawString(dtBillPrint.Compute("SUM(WASTAGE)", "").ToString, fontBold, brush, c4, y1, rAlign)
                g.DrawString(dtBillPrint.Compute("SUM(AMOUNT)", "").ToString(), fontBold, brush, c5, y1, rAlign)
                y1 = y1 + 30
                g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                y1 = y1 + 30
                If dtBillPrint.Rows.Count > 0 Then
                    g.DrawString("[EST - " + EstNo_PU + "] [EMPID - " + dtBillPrint.Rows(0)("EMPID").ToString + "] [USERID - " + dtBillPrint.Rows(0)("USERID").ToString + "]" _
                    , fontRegular, brush, c1, y1, lAlign)
                Else
                    g.DrawString("[EST - " + EstNo_PU + "]", fontRegular, brush, c1, y1, lAlign)
                End If
                y1 = y1 + 40
            End If
        End Using
    End Sub
#End Region
#Region "Advance Printing"
    Dim printCustoInfo As Boolean = True
    Private Sub PrintAdvance_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintAdvance.PrintPage
        Try
            Dim NoofPage As Integer = 0
            Dim gstPrint As Boolean = False
            Dim rAlign As New StringFormat
            rAlign.Alignment = StringAlignment.Far
            NoofPrint += 1
            Using g As Graphics = e.Graphics
                CustomerInfoAdvanceReceipt(e.Graphics, e)
                TitleAdvanceReceipt(e.Graphics, e)
                g.DrawString("C O P Y  ", fontBold, Brushes.Black, c8, 600)
                If dsAdvanceReceipt.Tables("ITEMTAG").Rows.Count > 0 Then
                    For PagecountSale = PagecountSale To dsAdvanceReceipt.Tables("ITEMTAG").Rows.Count - 1
                        NoofPage += 1
                        With dsAdvanceReceipt.Tables("ITEMTAG").Rows(PagecountSale)
                            g.DrawString(IIf(Val(.Item("ITEMID").ToString()) > 0, .Item("ITEMID").ToString(), ""), fontRegular, BlackBrush, c1, TopY, LeftFormat)
                            g.DrawString(.Item("ITEMNAME").ToString(), fontRegular, BlackBrush, c2, TopY, LeftFormat)
                            g.DrawString(IIf(Val(.Item("WEIGHT").ToString()) > 0, .Item("WEIGHT").ToString(), ""), fontRegular, BlackBrush, c6, TopY, rAlign)

                            g.DrawString(IIf(Val(.Item("ITEMID").ToString()) > 0, .Item("ITEMID").ToString(), ""), fontRegular, BlackBrush, c1, BottomY, LeftFormat)
                            g.DrawString(.Item("ITEMNAME").ToString(), fontRegular, BlackBrush, c2, BottomY, LeftFormat)
                            g.DrawString(IIf(Val(.Item("WEIGHT").ToString()) > 0, .Item("WEIGHT").ToString(), ""), fontRegular, BlackBrush, c6, BottomY, rAlign)

                            TopY = TopY + 20
                            BottomY = BottomY + 20
                            If NoofPage > 4 Then
                                PagecountSale = PagecountSale + 1
                                TopY = 150 ' TOP AGAIN STARTING POSITION
                                BottomY = 720 ' BOTTOM AGAIN STATRING POSITION
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c1, 490)
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c1, 1040)
                                gstPrint = True
                                e.HasMorePages = True
                                Exit For
                            End If
                        End With
                    Next
                End If
                If gstPrint = False Then
                    If dsAdvanceReceipt.Tables("GSTTRAN").Rows.Count > 0 Then
                        g.DrawString("CGST : " & dsAdvanceReceipt.Tables("GSTTRAN").Rows(0).Item("CGST").ToString(), fontRegular, BlackBrush, 270, TopY, LeftFormat)
                        g.DrawString("CGST : " & dsAdvanceReceipt.Tables("GSTTRAN").Rows(0).Item("CGST").ToString(), fontRegular, BlackBrush, 270, BottomY, LeftFormat)
                        g.DrawString("SGST : " & dsAdvanceReceipt.Tables("GSTTRAN").Rows(0).Item("SGST").ToString(), fontRegular, BlackBrush, c3, TopY, LeftFormat)
                        g.DrawString("SGST : " & dsAdvanceReceipt.Tables("GSTTRAN").Rows(0).Item("SGST").ToString(), fontRegular, BlackBrush, c3, BottomY, LeftFormat)
                        g.DrawString("IGST : " & dsAdvanceReceipt.Tables("GSTTRAN").Rows(0).Item("IGST").ToString(), fontRegular, BlackBrush, c4, TopY, LeftFormat)
                        g.DrawString("IGST : " & dsAdvanceReceipt.Tables("GSTTRAN").Rows(0).Item("IGST").ToString(), fontRegular, BlackBrush, c4, BottomY, LeftFormat)
                    End If
                End If
                If dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field1").ToString.Contains("ADVANCE ") Then
                    g.DrawString("NOTE : ORIGINAL ADVANCE RECEIPT SHOULD BE PRODUCED AT THE TIME OF ITEM(S) DELIVERY ", fontBold, Brushes.Black, c2, 470)
                    g.DrawString("NOTE : ORIGINAL ADVANCE RECEIPT SHOULD BE PRODUCED AT THE TIME OF ITEM(S) DELIVERY ", fontBold, Brushes.Black, c2, 1040)
                    g.DrawString(NodeId, fontRegular, BlackBrush, c7, TopY, rAlign)
                    g.DrawString(NodeId, fontRegular, BlackBrush, c7, BottomY, rAlign)
                Else
                    g.DrawString(NodeId, fontRegular, BlackBrush, c7, TopY, rAlign)
                    g.DrawString(NodeId, fontRegular, BlackBrush, c7, BottomY, rAlign)
                End If
                'TOP 
                g.DrawLine(Pens.Silver, 20.0F, 490.0F, 300.0F, 490.0F)
                g.DrawLine(Pens.Silver, 460.0F, 490.0F, 775.0F, 490.0F)
                g.DrawString("[GSTIN " & GSTNO & "]", fontRegular, Brushes.Black, 300, 480)
                ''If ImgPrint.Length > 0 Then
                ''    g.DrawString("For New Saravana Stores Bramandamai", New Font("Times New Roman", 8, FontStyle.Regular), Brushes.Black, c6, 490)
                ''    Dim Image1 As Image = Image.FromStream(ImgPrint)
                ''    e.Graphics.DrawImage(Image1, 600, 500, 100, 70)
                ''End If
                'BOTTOM
                g.DrawLine(Pens.Silver, 20.0F, 1060.0F, 300.0F, 1060.0F)
                g.DrawLine(Pens.Silver, 460.0F, 1060.0F, 775.0F, 1060.0F)
                g.DrawString("[GSTIN " & GSTNO & "] ", fontRegular, Brushes.Black, 300, 1050)
                ''If ImgPrint.Length > 0 Then
                ''    g.DrawString("For New Saravana Stores Bramandamai", New Font("Times New Roman", 8, FontStyle.Regular), Brushes.Black, c6, 1060)
                ''    Dim Image1 As Image = Image.FromStream(ImgPrint)
                ''    e.Graphics.DrawImage(Image1, 600, 1070, 100, 70)
                ''End If
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            Exit Sub
        End Try
    End Sub
#End Region
#Region "Order Printing"
    Private Sub PrintOrder_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintOrder.PrintPage
        Try
            Dim NoofPage As Integer = 0
            Dim rAlign As New StringFormat
            rAlign.Alignment = StringAlignment.Far
            NoofPrint += 1
            Using g As Graphics = e.Graphics
                CustomerInfo(Tranno, strBilldate, e.Graphics, e)
                TitleOrderRepairBooking(e.Graphics, e)
                RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
                If dtOrderRepair.Rows.Count > 0 Then
                    For PagecountSale = PagecountSale To dtOrderRepair.Rows.Count - 1
                        With dtOrderRepair.Rows(PagecountSale)
                            NoofPage += 1
                            If .Item("RESULT").ToString = "9" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "U" Then
                                TopY = TopY - 2
                                BottomY = BottomY - 2
                                'c6
                                g.DrawLine(Pens.Silver, 530, TopY, c6, TopY)
                                g.DrawLine(Pens.Silver, 530, BottomY, c6, BottomY)
                                g.DrawLine(Pens.Silver, 710, TopY, c9, TopY)
                                g.DrawLine(Pens.Silver, 710, BottomY, c9, BottomY)
                                TopY = TopY + 2
                                BottomY = BottomY + 2
                            End If
                            g.DrawString(IIf(Val(.Item("SRNO").ToString()) > 0, .Item("SRNO").ToString(), ""), fontRegular, BlackBrush, c1, TopY, LeftFormat)
                            g.DrawString(IIf(Val(.Item("SRNO").ToString()) > 0, .Item("SRNO").ToString(), ""), fontRegular, BlackBrush, c1, BottomY, LeftFormat)
                            If .Item("COLHEAD").ToString <> "G" Then
                                g.DrawString(.Item("ITEMNAME").ToString(), fontRegular, BlackBrush, c2, TopY, LeftFormat)
                                g.DrawString(.Item("ITEMNAME").ToString(), fontRegular, BlackBrush, c2, BottomY, LeftFormat)
                            ElseIf .Item("COLHEAD").ToString = "G" And .Item("RESULT").ToString = "12" Then
                                g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c3, TopY, LeftFormat)
                                g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c3, BottomY, LeftFormat)
                            ElseIf .Item("COLHEAD").ToString = "G" Then
                                g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c2, TopY, LeftFormat)
                                g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c2, BottomY, LeftFormat)
                            End If
                            g.DrawString(IIf(Val(.Item("PCS").ToString()) > 0, Val(.Item("PCS").ToString()), ""), fontRegular, BlackBrush, c6, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("PCS").ToString()) > 0, Val(.Item("PCS").ToString()), ""), fontRegular, BlackBrush, c6, BottomY, rAlign)
                            g.DrawString(IIf(Val(.Item("GRSWT").ToString()) > 0, .Item("GRSWT").ToString(), ""), fontRegular, BlackBrush, c9, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("GRSWT").ToString()) > 0, .Item("GRSWT").ToString(), ""), fontRegular, BlackBrush, c9, BottomY, rAlign)
                            TopY = TopY + 20
                            BottomY = BottomY + 20
                            If .Item("RESULT").ToString = "9" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "U" Then
                                'c6
                                g.DrawLine(Pens.Silver, 530, TopY, c6, TopY)
                                g.DrawLine(Pens.Silver, 530, BottomY, c6, BottomY)
                                g.DrawLine(Pens.Silver, 710, TopY, c9, TopY)
                                g.DrawLine(Pens.Silver, 710, BottomY, c9, BottomY)
                            End If
                            If NoofPage > 10 Then
                                PagecountSale = PagecountSale + 1
                                TopY = 150 ' TOP AGAIN STARTING POSITION
                                BottomY = 720 ' BOTTOM AGAIN STATRING POSITION
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c1, 490)
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c1, 1040)
                                e.HasMorePages = True
                                Exit For
                            End If
                        End With
                    Next
                End If
                Footer(e.Graphics, e)
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            Exit Sub
        End Try
    End Sub
#End Region
#Region "Smith Issue & Receipt"
    Private Sub PrtSmith_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrtSmith.PrintPage
        Try
            Dim NoofPage As Integer = 0
            Dim rAlign As New StringFormat
            rAlign.Alignment = StringAlignment.Far
            NoofPrint += 1
            Using g As Graphics = e.Graphics
                CustomerInfoSmithIssRec(Tranno, strBilldate, e.Graphics, e)
                TitleSmithIssueReceipt(e.Graphics, e)
                g.DrawLine(Pens.Silver, 20, TopY, 775, TopY)
                g.DrawLine(Pens.Silver, 20, BottomY, 775, BottomY)
                RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
                If dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count > 0 And dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows.Count > 0 Then
                    For PagecountSale = PagecountSale To dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count - 1
                        With dsSmithIssueReceipt.Tables("DTISSREC").Rows(PagecountSale)
                            If .Item("ACCODE").ToString = "DIFFWT" And .Item("TTYPE").ToString = "IIS" Then
                                g.DrawString("", fontRegular, BlackBrush, c1, TopY, LeftFormat)
                                g.DrawString("LESS DIFF. WGT ", fontRegular, BlackBrush, c2, TopY, LeftFormat)
                            Else
                                g.DrawString(IIf(Val(.Item("SNO").ToString()) > 0, .Item("SNO").ToString(), ""), fontRegular, BlackBrush, c1, TopY, LeftFormat)
                                g.DrawString(.Item("CATNAME").ToString(), fontRegular, BlackBrush, c2, TopY, LeftFormat)
                            End If
                            g.DrawString(.Item("HSN").ToString(), fontRegular6, BlackBrush, 280, TopY, LeftFormat)
                            g.DrawString(IIf(Val(.Item("PCS").ToString()) > 0, .Item("PCS").ToString(), ""), fontRegular, BlackBrush, c3, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("GRSWT").ToString()) > 0, .Item("GRSWT").ToString(), ""), fontRegular, BlackBrush, c4, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("NETWT").ToString()) > 0, .Item("NETWT").ToString(), ""), fontRegular, BlackBrush, c5, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("RATE").ToString()) > 0, "", ""), fontRegular, BlackBrush, c6, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("DIAWT").ToString()) > 0, .Item("DIAWT").ToString, ""), fontRegular, BlackBrush, c7, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("MC").ToString()) > 0, .Item("MC").ToString(), ""), fontRegular, BlackBrush, c8, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) > 0, .Item("AMOUNT").ToString(), ""), fontRegular, BlackBrush, c9, TopY, rAlign)

                            If .Item("ACCODE").ToString = "DIFFWT" And .Item("TTYPE").ToString = "IIS" Then
                                g.DrawString("", fontRegular, BlackBrush, c1, BottomY, LeftFormat)
                                g.DrawString("LESS DIFF. WGT", fontRegular, BlackBrush, c2, BottomY, LeftFormat)
                            Else
                                g.DrawString(IIf(Val(.Item("SNO").ToString()) > 0, .Item("SNO").ToString(), ""), fontRegular, BlackBrush, c1, BottomY, LeftFormat)
                                g.DrawString(.Item("CATNAME").ToString(), fontRegular, BlackBrush, c2, BottomY, LeftFormat)
                            End If
                            g.DrawString(.Item("HSN").ToString(), fontRegular6, BlackBrush, 280, BottomY, LeftFormat)
                            g.DrawString(IIf(Val(.Item("PCS").ToString()) > 0, .Item("PCS").ToString(), ""), fontRegular, BlackBrush, c3, BottomY, rAlign)
                            g.DrawString(IIf(Val(.Item("GRSWT").ToString()) > 0, .Item("GRSWT").ToString(), ""), fontRegular, BlackBrush, c4, BottomY, rAlign)
                            g.DrawString(IIf(Val(.Item("NETWT").ToString()) > 0, .Item("NETWT").ToString(), ""), fontRegular, BlackBrush, c5, BottomY, rAlign)
                            g.DrawString(IIf(Val(.Item("RATE").ToString()) > 0, "", ""), fontRegular, BlackBrush, c6, BottomY, rAlign)
                            g.DrawString(IIf(Val(.Item("DIAWT").ToString()) > 0, .Item("DIAWT").ToString, ""), fontRegular, BlackBrush, c7, BottomY, rAlign)
                            g.DrawString(IIf(Val(.Item("MC").ToString()) > 0, .Item("MC").ToString(), ""), fontRegular, BlackBrush, c8, BottomY, rAlign)
                            g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) > 0, .Item("AMOUNT").ToString(), ""), fontRegular, BlackBrush, c9, BottomY, rAlign)
                            TopY = TopY + 18
                            BottomY = BottomY + 18

                            If Val(.Item("WASTAGE").ToString()) > 0 Then
                                g.DrawString(.Item("WASTAGENAME").ToString(), fontRegular, BlackBrush, c2, TopY, LeftFormat)
                                g.DrawString(.Item("WASTAGENAME").ToString(), fontRegular, BlackBrush, c2, BottomY, LeftFormat)
                                g.DrawString(.Item("WASTAGE").ToString(), fontRegular, BlackBrush, c5, TopY, rAlign)
                                g.DrawString(.Item("WASTAGE").ToString(), fontRegular, BlackBrush, c5, BottomY, rAlign)
                                TopY = TopY + 18
                                BottomY = BottomY + 18
                            End If
                            If Val(.Item("ALLOY").ToString()) > 0 Then
                                g.DrawString(.Item("ALLOYNAME").ToString(), fontRegular, BlackBrush, c2, TopY, LeftFormat)
                                g.DrawString(.Item("ALLOYNAME").ToString(), fontRegular, BlackBrush, c2, BottomY, LeftFormat)
                                g.DrawString(.Item("ALLOY").ToString(), fontRegular, BlackBrush, c5, TopY, rAlign)
                                g.DrawString(.Item("ALLOY").ToString(), fontRegular, BlackBrush, c5, BottomY, rAlign)
                                TopY = TopY + 18
                                BottomY = BottomY + 18
                            End If
                            'If Val(.Item("TAX").ToString()) > 0 Then
                            '    g.DrawString("GSTIN ", fontRegular, BlackBrush, c6, TopY, LeftFormat)
                            '    g.DrawString("GSTIN ", fontRegular, BlackBrush, c6, BottomY, LeftFormat)
                            '    g.DrawString(.Item("TAX").ToString(), fontRegular, BlackBrush, c9, TopY, rAlign)
                            '    g.DrawString(.Item("TAX").ToString(), fontRegular, BlackBrush, c9, BottomY, rAlign)
                            '    TopY = TopY + 18
                            '    BottomY = BottomY + 18
                            'End If
                            If NoofPage > 6 Then
                                FooterSmithIssueReceipt(e.Graphics, e)
                                PagecountSale = PagecountSale + 1
                                TopY = 150 ' TOP AGAIN STARTING POSITION
                                BottomY = 720 ' BOTTOM AGAIN STATRING POSITION
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c1, 490)
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c1, 1040)
                                e.HasMorePages = True
                                Exit For
                            End If
                        End With
                    Next
                End If

                If TopY > 400 Then
                    FooterSmithIssueReceipt(e.Graphics, e)
                    PagecountSale = PagecountSale + 1
                    TopY = 150 ' TOP AGAIN STARTING POSITION
                    BottomY = 720 ' BOTTOM AGAIN STATRING POSITION
                    g.DrawString("Continue.... ", fontRegular, Brushes.Black, c1, 490)
                    g.DrawString("Continue.... ", fontRegular, Brushes.Black, c1, 1040)
                    e.HasMorePages = True
                    Return
                End If

                If dsSmithIssueReceipt.Tables("TAXTRAN").Rows.Count > 0 Then
                    For t As Integer = 0 To dsSmithIssueReceipt.Tables("TAXTRAN").Rows.Count - 1
                        With dsSmithIssueReceipt.Tables("TAXTRAN").Rows(t)
                            If .Item("TAXID").ToString() = "CG" Then
                                g.DrawString("CGST " & .Item("TAXPER").ToString() & "%", fontRegular, BlackBrush, c6, TopY, rAlign)
                                g.DrawString("CGST " & .Item("TAXPER").ToString() & "%", fontRegular, BlackBrush, c6, BottomY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegular, BlackBrush, c9, TopY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegular, BlackBrush, c9, BottomY, rAlign)
                            ElseIf .Item("TAXID").ToString() = "SG" Then
                                g.DrawString("SGST " & .Item("TAXPER").ToString() & "%", fontRegular, BlackBrush, c6, TopY, rAlign)
                                g.DrawString("SGST " & .Item("TAXPER").ToString() & "%", fontRegular, BlackBrush, c6, BottomY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegular, BlackBrush, c9, TopY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegular, BlackBrush, c9, BottomY, rAlign)
                            ElseIf .Item("TAXID").ToString() = "TC" Then
                                g.DrawString("TCS " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "", fontRegular, BlackBrush, c6, TopY, rAlign)
                                g.DrawString("TCS " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "", fontRegular, BlackBrush, c6, BottomY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegular, BlackBrush, c9, TopY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegular, BlackBrush, c9, BottomY, rAlign)
                            ElseIf .Item("TAXID").ToString() = "IG" Then
                                g.DrawString("IGST " & .Item("TAXPER").ToString() & "%", fontRegular, BlackBrush, c6, TopY, rAlign)
                                g.DrawString("IGST " & .Item("TAXPER").ToString() & "%", fontRegular, BlackBrush, c6, BottomY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegular, BlackBrush, c9, TopY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegular, BlackBrush, c9, BottomY, rAlign)
                            Else
                                g.DrawString("TDS" & .Item("TAXPER").ToString() & "%", fontRegular, BlackBrush, c6, TopY, rAlign)
                                g.DrawString("TDS " & .Item("TAXPER").ToString() & "%", fontRegular, BlackBrush, c6, BottomY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegular, BlackBrush, c9, TopY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegular, BlackBrush, c9, BottomY, rAlign)
                            End If
                        End With
                        TopY = TopY + 18
                        BottomY = BottomY + 18
                    Next
                End If
                If dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows.Count > 0 Then
                    Dim Remark1 As String = ""
                    Dim Remark2 As String = ""
                    Remark1 = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("REMARK1").ToString
                    If Remark1 = "" And dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count > 1 Then
                        Remark1 = dsSmithIssueReceipt.Tables("DTISSREC").Rows(1).Item("REMARK1").ToString
                    End If
                    Remark2 = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("REMARK2").ToString
                    If Remark2 = "" And dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count > 1 Then
                        Remark2 = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("REMARK2").ToString
                    End If
                    g.DrawString(Remark1 & " " & Remark2, fontRegular, BlackBrush, c2, 400, LeftFormat) 'Top
                    g.DrawString(Remark1 & " " & Remark2, fontRegular, BlackBrush, c2, 955, LeftFormat) 'Bottom
                    g.DrawLine(Pens.Silver, 20, 420, 775, 420)
                    g.DrawLine(Pens.Silver, 20, 975, 775, 975)
                    Dim TopYTemp As Integer = 425
                    Dim BottomYTemp As Integer = 980
                    With dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(0)
                        g.DrawString(.Item("TOTAL").ToString(), fontRegular, BlackBrush, c2, TopYTemp, LeftFormat)
                        g.DrawString(IIf(Val(.Item("PCS").ToString()) > 0, .Item("PCS").ToString(), Format(-1 * Val(.Item("PCS").ToString()), "0.00")), fontRegular, BlackBrush, c3, TopYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("GRSWT").ToString()) > 0, .Item("GRSWT").ToString(), Format(-1 * Val(.Item("GRSWT").ToString()), "0.00")), fontRegular, BlackBrush, c4, TopYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("NETWT").ToString()) > 0, .Item("NETWT").ToString(), Format(-1 * Val(.Item("NETWT").ToString()), "0.00")), fontRegular, BlackBrush, c5, TopYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("DIAWT").ToString()) > 0, .Item("DIAWT").ToString(), Format(-1 * Val(.Item("DIAWT").ToString()), "0.00")), fontRegular, BlackBrush, c7, TopYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("MC").ToString()) > 0, .Item("MC").ToString(), Format(-1 * Val(.Item("MC").ToString()), "0.00")), fontRegular, BlackBrush, c8, TopYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) > 0, .Item("AMOUNT").ToString(), Format(-1 * Val(.Item("AMOUNT").ToString()), "0.00")), fontRegular, BlackBrush, c9, TopYTemp, rAlign)

                        g.DrawString(.Item("TOTAL").ToString(), fontRegular, BlackBrush, c2, BottomYTemp, LeftFormat)
                        g.DrawString(IIf(Val(.Item("PCS").ToString()) > 0, .Item("PCS").ToString(), Format(-1 * Val(.Item("PCS").ToString()), "0.00")), fontRegular, BlackBrush, c3, BottomYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("GRSWT").ToString()) > 0, .Item("GRSWT").ToString(), Format(-1 * Val(.Item("GRSWT").ToString()), "0.00")), fontRegular, BlackBrush, c4, BottomYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("NETWT").ToString()) > 0, .Item("NETWT").ToString(), Format(-1 * Val(.Item("NETWT").ToString()), "0.00")), fontRegular, BlackBrush, c5, BottomYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("DIAWT").ToString()) > 0, .Item("DIAWT").ToString(), Format(-1 * Val(.Item("DIAWT").ToString()), "0.00")), fontRegular, BlackBrush, c7, BottomYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("MC").ToString()) > 0, .Item("MC").ToString(), Format(-1 * Val(.Item("MC").ToString()), "0.00")), fontRegular, BlackBrush, c8, BottomYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) > 0, .Item("AMOUNT").ToString(), Format(-1 * Val(.Item("AMOUNT").ToString()), "0.00")), fontRegular, BlackBrush, c9, BottomYTemp, rAlign)

                    End With
                End If
                FooterSmithIssueReceipt(e.Graphics, e)
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            Exit Sub
        End Try
    End Sub
#End Region

#Region " Smith Issue & Receipt 2 "
    Dim sm1 As Integer = 25 'SNo.
    Dim sm2 As Integer = 48 'Description 
    Dim sm20 As Integer = 215 'HSN
    Dim sm3 As Integer = 300 'Purity
    Dim sm4 As Integer = 350 'Grs.wt
    Dim sm5 As Integer = 410 'Stone.wt
    Dim sm6 As Integer = 470 'Net.wt
    Dim sm7 As Integer = 530 'Touch
    Dim sm8 As Integer = 590 ' Pure.wt
    Dim sm9 As Integer = 650 ' Wasate
    Dim sm10 As Integer = 710 'Mc
    Dim sm11 As Integer = 770 'Amount
    'ITEMWISE PRINT
    Dim smI1 As Integer = 25 'SNo.
    Dim smI2 As Integer = 48 'Description 
    Dim smI20 As Integer = 200 'HSN
    Dim smI3 As Integer = 275 'TOUCH
    Dim smI21 As Integer = 310 'PCS
    Dim smI4 As Integer = 365 'Grs.wt
    Dim smI5 As Integer = 425 'Stone.wt
    Dim smI6 As Integer = 475 'Net.wt
    Dim smI7 As Integer = 530 'Purity
    Dim smI8 As Integer = 590 ' Pure.wt
    Dim smI9 As Integer = 650 ' Yield
    Dim smI10 As Integer = 710 'Mc
    Dim smI11 As Integer = 770 'Amount
    Private Sub PrtSmith2_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrtSmith2.PrintPage
        Try
            Dim NoofPage As Integer = 0
            Dim rAlign As New StringFormat
            rAlign.Alignment = StringAlignment.Far
            NoofPrint += 1
            Using g As Graphics = e.Graphics
                CustomerInfoSmithIssRec(Tranno, strBilldate, e.Graphics, e)
                TitleSmithIssueReceipt2(e.Graphics, e)
                g.DrawLine(Pens.Silver, 20, TopY, 775, TopY)
                g.DrawLine(Pens.Silver, 20, BottomY, 775, BottomY)
                RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
                If dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count > 0 And dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows.Count > 0 Then
                    For PagecountSale = PagecountSale To dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count - 1
                        With dsSmithIssueReceipt.Tables("DTISSREC").Rows(PagecountSale)

                            g.DrawString(IIf(Val(.Item("SNO").ToString()) > 0, .Item("SNO").ToString(), ""), fontRegularsmall, BlackBrush, sm1, TopY, LeftFormat)
                            g.DrawString(.Item("ITEMNAME").ToString(), fontRegularsmall, BlackBrush, sm2, TopY, LeftFormat)
                            g.DrawString(.Item("HSN").ToString(), fontRegularsmall, BlackBrush, sm20, TopY, LeftFormat)
                            g.DrawString(IIf(Val(.Item("TOUCH").ToString()) > 0, .Item("TOUCH").ToString, ""), fontRegularsmall, BlackBrush, sm3, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("GRSWT").ToString()) > 0, .Item("GRSWT").ToString(), ""), fontRegularsmall, BlackBrush, sm4, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("LESSWT").ToString()) > 0, .Item("LESSWT").ToString(), ""), fontRegularsmall, BlackBrush, sm5, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("NETWT").ToString()) > 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, sm6, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("PURITY").ToString()) > 0, .Item("PURITY").ToString(), ""), fontRegularsmall, BlackBrush, sm7, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("PUREWT").ToString()) > 0, .Item("PUREWT").ToString(), ""), fontRegularsmall, BlackBrush, sm8, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("WASTAGE").ToString()) > 0, .Item("WASTAGE").ToString(), ""), fontRegularsmall, BlackBrush, sm9, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("MC").ToString()) > 0, .Item("MC").ToString(), ""), fontRegularsmall, BlackBrush, sm10, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) > 0, .Item("AMOUNT").ToString(), ""), fontRegularsmall, BlackBrush, sm11, TopY, rAlign)

                            g.DrawString(IIf(Val(.Item("SNO").ToString()) > 0, .Item("SNO").ToString(), ""), fontRegularsmall, BlackBrush, sm1, BottomY, LeftFormat)
                            g.DrawString(.Item("ITEMNAME").ToString(), fontRegularsmall, BlackBrush, sm2, BottomY, LeftFormat)
                            g.DrawString(.Item("HSN").ToString(), fontRegularsmall, BlackBrush, sm20, BottomY, LeftFormat)
                            g.DrawString(IIf(Val(.Item("TOUCH").ToString()) > 0, (.Item("TOUCH").ToString()), ""), fontRegularsmall, BlackBrush, sm3, BottomY, rAlign)
                            g.DrawString(IIf(Val(.Item("GRSWT").ToString()) > 0, .Item("GRSWT").ToString(), ""), fontRegularsmall, BlackBrush, sm4, BottomY, rAlign)
                            g.DrawString(IIf(Val(.Item("LESSWT").ToString()) > 0, .Item("LESSWT").ToString(), ""), fontRegularsmall, BlackBrush, sm5, BottomY, rAlign)
                            g.DrawString(IIf(Val(.Item("NETWT").ToString()) > 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, sm6, BottomY, rAlign)
                            g.DrawString(IIf(Val(.Item("PURITY").ToString()) > 0, .Item("PURITY").ToString(), ""), fontRegularsmall, BlackBrush, sm7, BottomY, rAlign)
                            g.DrawString(IIf(Val(.Item("PUREWT").ToString()) > 0, .Item("PUREWT").ToString, ""), fontRegularsmall, BlackBrush, sm8, BottomY, rAlign)
                            g.DrawString(IIf(Val(.Item("WASTAGE").ToString()) > 0, .Item("WASTAGE").ToString(), ""), fontRegularsmall, BlackBrush, sm9, BottomY, rAlign)
                            g.DrawString(IIf(Val(.Item("MC").ToString()) > 0, .Item("MC").ToString(), ""), fontRegularsmall, BlackBrush, sm10, BottomY, rAlign)
                            g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) > 0, .Item("AMOUNT").ToString(), ""), fontRegularsmall, BlackBrush, sm11, BottomY, rAlign)

                            TopY = TopY + 18
                            BottomY = BottomY + 18
                            If TopY > 400 Then 'NoofPage > 6
                                PagecountSale = PagecountSale + 1
                                TopY = 135 '150 ' TOP AGAIN STARTING POSITION
                                BottomY = 705 '720 ' BOTTOM AGAIN STATRING POSITION
                                g.DrawString("Continue.... ", fontRegularsmall, Brushes.Black, c1, 490)
                                g.DrawString("Continue.... ", fontRegularsmall, Brushes.Black, c1, 1040)
                                FooterSmithIssueReceipt(e.Graphics, e)
                                e.HasMorePages = True
                                Return
                                'Exit For
                            End If
                        End With
                    Next
                End If
                If dsSmithIssueReceipt.Tables("TAXTRAN").Rows.Count > 0 Then
                    For t As Integer = 0 To dsSmithIssueReceipt.Tables("TAXTRAN").Rows.Count - 1
                        With dsSmithIssueReceipt.Tables("TAXTRAN").Rows(t)
                            If .Item("TAXID").ToString() = "CG" Then
                                g.DrawString("CGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, TopY, rAlign)
                                g.DrawString("CGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, BottomY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, sm11, TopY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, sm11, BottomY, rAlign)
                            ElseIf .Item("TAXID").ToString() = "SG" Then
                                g.DrawString("SGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, TopY, rAlign)
                                g.DrawString("SGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, BottomY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, sm11, TopY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, sm11, BottomY, rAlign)
                            ElseIf .Item("TAXID").ToString() = "TC" Then
                                g.DrawString("TCS " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "", fontRegularsmall, BlackBrush, sm8, TopY, rAlign)
                                g.DrawString("TCS " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "", fontRegularsmall, BlackBrush, sm8, BottomY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, sm11, TopY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, sm11, BottomY, rAlign)
                            ElseIf .Item("TAXID").ToString() = "IG" Then
                                g.DrawString("IGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, TopY, rAlign)
                                g.DrawString("IGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, BottomY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, sm11, TopY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, sm11, BottomY, rAlign)
                            Else
                                g.DrawString("LESS TDS " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, TopY, rAlign)
                                g.DrawString("LESS TDS " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, BottomY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, sm11, TopY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, sm11, BottomY, rAlign)
                            End If
                        End With
                        TopY = TopY + 18
                        BottomY = BottomY + 18
                    Next
                End If

                If dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows.Count > 0 Then
                    Dim Remark1 As String = ""
                    Dim Remark2 As String = ""
                    Remark1 = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("REMARK1").ToString
                    Remark2 = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("REMARK2").ToString
                    g.DrawString(Remark1 & " " & Remark2, fontRegularsmall, BlackBrush, sm2, 400, LeftFormat) 'Top
                    g.DrawString(Remark1 & " " & Remark2, fontRegularsmall, BlackBrush, sm2, 955, LeftFormat) 'Bottom
                    g.DrawLine(Pens.Silver, 20, 420, 775, 420)
                    g.DrawLine(Pens.Silver, 20, 975, 775, 975)
                    Dim TopYTemp As Integer = 425
                    Dim BottomYTemp As Integer = 980
                    With dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(0)
                        g.DrawString(.Item("TOTAL").ToString(), fontRegularsmall, BlackBrush, sm2, TopYTemp, LeftFormat)
                        g.DrawString(IIf(Val(.Item("GRSWT").ToString()) > 0, .Item("GRSWT").ToString(), ""), fontRegularsmall, BlackBrush, sm4, TopYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("LESSWT").ToString()) > 0, .Item("LESSWT").ToString(), ""), fontRegularsmall, BlackBrush, sm5, TopYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("NETWT").ToString()) > 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, sm6, TopYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("PUREWT").ToString()) > 0, .Item("PUREWT").ToString(), ""), fontRegularsmall, BlackBrush, sm8, TopYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("MC").ToString()) > 0, .Item("MC").ToString(), ""), fontRegularsmall, BlackBrush, sm10, TopYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) > 0, .Item("AMOUNT").ToString(), ""), fontRegularsmall, BlackBrush, sm11, TopYTemp, rAlign)

                        g.DrawString(.Item("TOTAL").ToString(), fontRegularsmall, BlackBrush, sm2, BottomYTemp, LeftFormat)
                        g.DrawString(IIf(Val(.Item("GRSWT").ToString()) > 0, .Item("GRSWT").ToString(), ""), fontRegularsmall, BlackBrush, sm4, BottomYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("LESSWT").ToString()) > 0, .Item("LESSWT").ToString(), ""), fontRegularsmall, BlackBrush, sm5, BottomYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("NETWT").ToString()) > 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, sm6, BottomYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("PUREWT").ToString()) > 0, .Item("PUREWT").ToString(), ""), fontRegularsmall, BlackBrush, sm8, BottomYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("MC").ToString()) > 0, .Item("MC").ToString(), ""), fontRegularsmall, BlackBrush, sm10, BottomYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) > 0, .Item("AMOUNT").ToString(), ""), fontRegularsmall, BlackBrush, sm11, BottomYTemp, rAlign)
                    End With
                End If
                FooterSmithIssueReceipt(e.Graphics, e)
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            Exit Sub
        End Try
    End Sub
#End Region
#Region " Print Smith Issue A4 Sheet Print "
    Private Sub PrintDocumnt_NewCopyName(ByVal g1 As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal CopyName As String)
        If CopyName <> "" Then
            e.Graphics.RotateTransform(40)
            Dim myfont As New Font("Arial", 80)
            Dim myBrush As New SolidBrush(Color.FromArgb(30, 0, 0, 255))
            e.Graphics.DrawString(CopyName & "Copy", myfont, myBrush, 250, 30)
            e.Graphics.ResetTransform()
        End If
    End Sub

    Private Sub PrintDocumnt_smith_DrawLine(ByVal g1 As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs _
                                 , ByVal CopyName As String, ByVal catNamegroup As String, ByVal _topy As Integer)
        g1.DrawLine(Pens.Silver, 20, _topy, 775, _topy)
    End Sub

    Private Sub PrintDocumnt_smith_ItemName(ByVal g1 As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs _
                                 , ByVal CopyName As String, ByVal catNamegroup As String)
        Try
            Dim wastageValue As Double = 0
            Dim wastageValue2 As Double = 0
            Dim FirstY1 As Integer = 0
            Dim NoofPage As Integer = 0
            Dim dtIssRecDistinct As New DataTable
            Dim rAlign As New StringFormat
            rAlign.Alignment = StringAlignment.Far
            NoofPrint += 1
            Using g As Graphics = e.Graphics
                If dsSmithIssueReceipt.Tables(catNamegroup).Rows.Count > 0 Then
                    dtIssRecDistinct = dsSmithIssueReceipt.Tables(catNamegroup).DefaultView.ToTable(True, "TABLENAME")
                End If
                If dtIssRecDistinct.Rows.Count > 1 Then
                    If boolPrintSmithA4Issue = True Then
                        CustomerInfoSmithIssRecA4(Tranno, strBilldate, e.Graphics, e, "RECEIPT")
                    Else
                        CustomerInfoSmithIssRecA4(Tranno, strBilldate, e.Graphics, e, "ISSUE")
                    End If
                Else
                    CustomerInfoSmithIssRecA4(Tranno, strBilldate, e.Graphics, e, dtIssRecDistinct.Rows(0).Item("TABLENAME").ToString)
                End If
                If _duplicate = "Y" Then
                    e.Graphics.RotateTransform(40)
                    Dim myfont As New Font("Arial", 80)
                    Dim myBrush As New SolidBrush(Color.FromArgb(30, 0, 0, 255))
                    e.Graphics.DrawString("Duplicate Copy", myfont, myBrush, 250, 30)
                    e.Graphics.ResetTransform()
                End If
                'Dim imgstream As New Bitmap(My.Resources.SSLLeterpadLogo)
                'e.Graphics.DrawImage(imgstream, 170, 0, 500, 130)
                FirstY1 = TopY
                TitleSmithIssueReceiptSmithItemNameA4(e.Graphics, e)
                PrintDocumnt_NewCopyName(g1, e, "")
                ' g.DrawLine(Pens.Silver, 20, TopY, 775, TopY)
                PrintDocumnt_smith_DrawLine(g1, e, CopyName, catNamegroup, TopY)
                RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
                If dsSmithIssueReceipt.Tables(catNamegroup).Rows.Count > 0 And dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows.Count > 0 Then 'DTISSREC
                    For PagecountSale = PagecountSale To dsSmithIssueReceipt.Tables(catNamegroup).Rows.Count - 1 'DTISSREC
                        With dsSmithIssueReceipt.Tables(catNamegroup).Rows(PagecountSale) 'DTISSREC
                            If dtIssRecDistinct.Rows.Count > 1 Then
                                If .Item("TABLENAME").ToString = "ISSUE" And boolPrintSmithA4Issue = True Then
                                    TopY = TopY + 25
                                    'g.DrawString("ISSUE", fontBoldTitle, BlackBrush, 425, TopY, LeftFormat)
                                    TopY = TopY + 25
                                    boolPrintSmithA4Issue = False
                                    GoTo NextImageIssue
                                End If
                            End If
                            If .Item("SERNO").ToString = "ZZZZZZZZZZZZ" Then
                                wastageValue = Val(dsSmithIssueReceipt.Tables(catNamegroup).Compute("SUM(WASTAGE)", "TABLENAME='RECEIPT'").ToString)
                                If wastageValue > 0 Then
                                    g.DrawString("YIELD / PROCESSING LOSS ", fontRegularsmall, BlackBrush, smI2, TopY, LeftFormat) 'ADD : WASTAGE
                                    g.DrawString(Format(wastageValue, "0.000"), fontRegularsmall, BlackBrush, smI8, TopY, rAlign)
                                    TopY = TopY + 18
                                    wastageValue2 = 1 * wastageValue
                                End If
                                wastageValue = 0
                            End If
                            g.DrawString(IIf(Val(.Item("SNO").ToString()) > 0, .Item("SNO").ToString(), ""), fontRegularsmall, BlackBrush, smI1, TopY, LeftFormat)
                            g.DrawString(.Item("ITEMNAME").ToString(), fontRegularsmall, BlackBrush, smI2, TopY, LeftFormat)
                            g.DrawString(Mid(.Item("HSN").ToString(), 1, 4), fontRegularsmall, BlackBrush, smI20, TopY, LeftFormat)
                            g.DrawString(IIf(Val(.Item("TOUCH").ToString()) <> 0, .Item("TOUCH").ToString, ""), fontRegularsmall, BlackBrush, smI3, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("PCS").ToString()) <> 0, .Item("PCS").ToString(), ""), fontRegularsmall, BlackBrush, smI21, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontRegularsmall, BlackBrush, smI4, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("LESSWT").ToString()) <> 0, .Item("LESSWT").ToString(), ""), fontRegularsmall, BlackBrush, smI5, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, smI6, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("PURITY").ToString()) <> 0, .Item("PURITY").ToString(), ""), fontRegularsmall, BlackBrush, smI7, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("PUREWT").ToString()) <> 0, .Item("PUREWT").ToString(), ""), fontRegularsmall, BlackBrush, smI8, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("WASTAGE").ToString()) <> 0, .Item("WASTAGE").ToString(), ""), fontRegularsmall, BlackBrush, smI9, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("MC").ToString()) <> 0, .Item("MC").ToString(), ""), fontRegularsmall, BlackBrush, smI10, TopY, rAlign)
                            If .Item("TAXID").ToString <> "" Then
                                Dim printGst As String = ""
                                If .Item("TAXID").ToString = "CG" Then
                                    printGst = "CGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%"
                                ElseIf .Item("TAXID").ToString = "SG" Then
                                    printGst = "SGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%"
                                ElseIf .Item("TAXID").ToString = "IG" Then
                                    printGst = "IGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%"
                                ElseIf .Item("TAXID").ToString = "TC" Then
                                    printGst = "TCS " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%"
                                ElseIf .Item("TAXID").ToString = "TD" Then
                                    If .Item("TABLENAME").ToString = "ISSUE" Then
                                        printGst = "TDS "
                                    Else
                                        printGst = "LESS : TDS " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%"
                                    End If
                                ElseIf .Item("TAXID").ToString = "ZRO" Then
                                    printGst = "Rounded Off"
                                Else
                                    printGst = ""
                                End If
                                g.DrawString(printGst, fontRegularsmall, BlackBrush, smI8, TopY, rAlign)
                                g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) <> 0, .Item("AMOUNT").ToString(), ""), fontRegularsmall, BlackBrush, smI11, TopY, rAlign)
                            Else
                                g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) <> 0, .Item("AMOUNT").ToString(), ""), fontRegularsmall, BlackBrush, smI11, TopY, rAlign)
                            End If
                            TopY = TopY + 18
                            If TopY > 875 Then '850
                                TopY = TopY + 18
                                PagecountSale = PagecountSale + 1
                                g.DrawString("Continue.... ", fontRegularsmall, Brushes.Black, c1, TopY) '490
                                TopY = TopY + 5
                                PrintDocumnt_smith_DrawLine(g1, e, CopyName, catNamegroup, TopY)
                                FooterSmithIssueReceiptA4(e.Graphics, e, FirstY1, TopY)
                                TopY = TOPYSMITHISSUEVALUE
                                e.HasMorePages = True
                                Return
                                'Exit For
                            End If
                        End With
                    Next
NextImageIssue:
                    If dtIssRecDistinct.Rows.Count > 1 Then
                        If dsSmithIssueReceipt.Tables(catNamegroup).Rows(0).Item("TTYPE").ToString = "RPU" Then
                            wastageValue = Val(dsSmithIssueReceipt.Tables(catNamegroup).Compute("SUM(WASTAGE)", "TABLENAME='ISSUE'").ToString)
                            If wastageValue > 0 Then
                                wastageValue2 = wastageValue2 - wastageValue
                                g.DrawString("YIELD / PROCESSING LOSS ", fontRegularsmall, BlackBrush, smI2, TopY, LeftFormat) 'ADD : WASTAGE
                                g.DrawString(Format(wastageValue, "0.000"), fontRegularsmall, BlackBrush, smI8, TopY, rAlign)
                                TopY = TopY + 18
                            End If
                        Else
                            If dsSmithIssueReceipt.Tables(catNamegroup).Rows(0).Item("TTYPE").ToString = "RRE" Then
                                If dsSmithIssueReceipt.Tables(catNamegroup).Rows(dsSmithIssueReceipt.Tables(catNamegroup).Rows.Count - 1).Item("TABLENAME").ToString = "ISSUE" And boolPrintSmithA4Issue = False Then
                                    If dsSmithIssueReceipt.Tables(catNamegroup).Rows.Count = PagecountSale Then
                                        wastageValue = Val(dsSmithIssueReceipt.Tables(catNamegroup).Compute("SUM(WASTAGE)", "TABLENAME='ISSUE'").ToString)
                                        If wastageValue > 0 Then
                                            wastageValue2 = wastageValue
                                            g.DrawString("YIELD / PROCESSING LOSS ", fontRegularsmall, BlackBrush, smI2, TopY, LeftFormat) 'ADD : WASTAGE
                                            g.DrawString(Format(wastageValue, "0.000"), fontRegularsmall, BlackBrush, smI8, TopY, rAlign)
                                            TopY = TopY + 18
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
                If dsSmithIssueReceipt.Tables("TAXTRAN").Rows.Count > 0 Then
                    For t As Integer = 0 To dsSmithIssueReceipt.Tables("TAXTRAN").Rows.Count - 1
                        With dsSmithIssueReceipt.Tables("TAXTRAN").Rows(t)
                            If Val(.Item("TAXAMOUNT").ToString) = 0 Then Continue For
                            If .Item("TAXID").ToString() = "CG" Then
                                g.DrawString("CGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, smI8, TopY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smI11, TopY, rAlign)
                            ElseIf .Item("TAXID").ToString() = "SG" Then
                                g.DrawString("SGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, smI8, TopY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smI11, TopY, rAlign)
                            ElseIf .Item("TAXID").ToString() = "TC" Then
                                g.DrawString("TCS " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "", fontRegularsmall, BlackBrush, smI8, TopY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smI11, TopY, rAlign)
                            ElseIf .Item("TAXID").ToString() = "IG" Then
                                g.DrawString("IGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, smI8, TopY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smI11, TopY, rAlign)
                            Else
                                g.DrawString("LESS : TDS " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, smI8, TopY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smI11, TopY, rAlign)
                            End If
                        End With
                        TopY = TopY + 18
                    Next
                End If
                If dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows.Count > 0 Then
                    For Rec As Integer = 0 To dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows.Count - 1
                        If dtIssRecDistinct.Rows.Count > 1 Then
                            If boolPrintSmithA4IssueTax = True Then ' RECEIPT TABLE
                                If dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(Rec).Item("RECPAY").ToString = "R" Then
                                    GoTo funIssRecTotal
                                Else
                                    Continue For
                                End If
                            ElseIf boolPrintSmithA4IssueTax = False Then 'ISSUE TABLE
                                If dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(Rec).Item("RECPAY").ToString = "I" Then
                                    GoTo funIssRecTotal
                                Else
                                    Continue For
                                End If
                            End If
                        End If
funIssRecTotal:
                        TopY = TopY + 18
                        Dim Remark1 As String = ""
                        Dim Remark2 As String = ""
                        Remark1 = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("REMARK1").ToString
                        Remark2 = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("REMARK2").ToString
                        g.DrawString(Remark1 & " " & Remark2, fontRegularsmall, BlackBrush, smI2, TopY, LeftFormat) 'Top
                        ' TopY = TopY + 498

                        Dim WastageValueTotal As Double = 0
                        WastageValueTotal = IIf(Val(dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(Rec).Item("PUREWT").ToString()) > 0, dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(Rec).Item("PUREWT").ToString(), 0) + wastageValue2

                        If dsSmithIssueReceipt.Tables(catNamegroup).Rows(0).Item("TTYPE").ToString = "RPU" Then
                            If dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(Rec).Item("RECPAY").ToString = "R" Then
                                If dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(Rec).Item("RECPAY").ToString = "R" Then
                                    g.DrawString(Format(WastageValueTotal, "0.000"), New Font("Palatino Linotype", 12, FontStyle.Regular), BlackBrush, smI8, TopY - 27, rAlign) '
                                Else
                                    g.DrawString(Format(WastageValueTotal, "0.000"), New Font("Palatino Linotype", 12, FontStyle.Regular), BlackBrush, smI8, TopY - 27, rAlign) 'Top'Impact
                                End If
                            Else
                                g.DrawString("-" & Format(WastageValueTotal, "0.000"), New Font("Palatino Linotype", 12, FontStyle.Regular), BlackBrush, smI8, TopY - 27, rAlign) 'Top'Impact
                            End If
                            TopY = 980
                        Else
                            TopY = 980
                            If dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(Rec).Item("RECPAY").ToString = "R" Then 'dsSmithIssueReceipt.Tables(catNamegroup).Rows(Rec).Item("TABLENAME").ToString = "RECEIPT"
                                If dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(Rec).Item("RECPAY").ToString = "R" Then 'dsSmithIssueReceipt.Tables(catNamegroup).Rows(Rec).Item("TABLENAME").ToString = "RPU"
                                    g.DrawString(Format(WastageValueTotal, "0.000"), New Font("Impact", 18, FontStyle.Regular), BlackBrush, smI8, TopY - 27, rAlign) '
                                Else
                                    g.DrawString(Format(WastageValueTotal, "0.000"), New Font("Impact", 18, FontStyle.Regular), BlackBrush, smI8, TopY - 27, rAlign) 'Top'Impact
                                End If
                            Else
                                g.DrawString("-" & Format(WastageValueTotal, "0.000"), New Font("Impact", 18, FontStyle.Regular), BlackBrush, smI8, TopY - 27, rAlign) 'Top'Impact
                            End If
                        End If
                        g.DrawString(CopyName, New Font("Impact", 18, FontStyle.Regular), BlackBrush, smI2, TopY - 27, LeftFormat) 'Top
                        PrintDocumnt_smith_DrawLine(g1, e, CopyName, catNamegroup, TopY)
                        With dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(Rec)
                            g.DrawString(.Item("TOTAL").ToString(), fontRegularsmall, BlackBrush, smI2, TopY, LeftFormat)
                            g.DrawString(IIf(Val(.Item("PCS").ToString()) <> 0, .Item("PCS").ToString(), ""), fontRegularsmall, BlackBrush, smI21, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontRegularsmall, BlackBrush, smI4, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("LESSWT").ToString()) <> 0, .Item("LESSWT").ToString(), ""), fontRegularsmall, BlackBrush, smI5, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, smI6, TopY, rAlign)
                            g.DrawString(Format(WastageValueTotal, "0.000"), fontRegularsmall, BlackBrush, sm8, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("MC").ToString()) <> 0, .Item("MC").ToString(), ""), fontRegularsmall, BlackBrush, smI10, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) <> 0, .Item("AMOUNT").ToString(), ""), fontRegularsmall, BlackBrush, smI11, TopY, rAlign)
                        End With
                        TopY = TopY + 18
                        'g.DrawLine(Pens.Silver, 20, TopY, 775, TopY)
                        PrintDocumnt_smith_DrawLine(g1, e, CopyName, catNamegroup, TopY)
                        boolPrintSmithA4IssueTax = False
                        Exit For
                    Next
                End If
                FooterSmithIssueReceiptA4(e.Graphics, e, FirstY1, TopY)
                If boolPrintSmithA4Issue = False Then
                    If PagecountSale <= dsSmithIssueReceipt.Tables(catNamegroup).Rows.Count - 1 Then
                        'PagecountSale = PagecountSale + 1'Don't Use this Line
                        g.DrawString("Continue.... ", fontRegularsmall, Brushes.Black, c1, TopY) '490
                        TopY = TopY + 5
                        TopY = TOPYSMITHISSUEVALUE
                        e.HasMorePages = True
                        Return
                    End If
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            Exit Sub
        End Try
    End Sub

    Dim smc1 As Integer = 45 'SNo.
    Dim smc2 As Integer = 70 'Description 
    Dim smc3 As Integer = 235 'HSN
    Dim smc10 As Integer = 310 'PCS
    Dim smc4 As Integer = 370 'Grs.wt
    Dim smc5 As Integer = 450 'Stone.wt
    Dim smc6 As Integer = 525 'Net.wt
    Dim smc7 As Integer = 600 'Wastage
    Dim smc8 As Integer = 670 ' Mc
    Dim smc9 As Integer = 770 ' Amount
    Dim TOPYSMITHISSUEVALUE As Integer = 140
    Private Sub PrintDocumnt_Smith_Category(ByVal g1 As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs _
                                 , ByVal CopyName As String, ByVal catNamegroup As String)
        Try
            Dim FirstY1 As Integer = 0
            Dim NoofPage As Integer = 0
            Dim rAlign As New StringFormat
            rAlign.Alignment = StringAlignment.Far
            NoofPrint += 1
            Dim dtIssRecDistinct As New DataTable
            Using g As Graphics = e.Graphics
                If dsSmithIssueReceipt.Tables(catNamegroup).Rows.Count > 0 Then
                    dtIssRecDistinct = dsSmithIssueReceipt.Tables(catNamegroup).DefaultView.ToTable(True, "TABLENAME")
                End If
                If dtIssRecDistinct.Rows.Count > 1 Then
                    If PrintRPU_IPU = True Then
                        CustomerInfoSmithIssRecA4(Tranno, strBilldate, e.Graphics, e, "ISSUE")
                    Else
                        If boolPrintSmithA4Issue = True Then
                            CustomerInfoSmithIssRecA4(Tranno, strBilldate, e.Graphics, e, "RECEIPT")
                        Else
                            CustomerInfoSmithIssRecA4(Tranno, strBilldate, e.Graphics, e, "ISSUE")
                        End If
                    End If
                Else
                    CustomerInfoSmithIssRecA4(Tranno, strBilldate, e.Graphics, e, dtIssRecDistinct.Rows(0).Item("TABLENAME").ToString)
                End If
                'Dim imgstream As New Bitmap(My.Resources.SSLLeterpadLogo)
                'e.Graphics.DrawImage(imgstream, 170, 0, 500, 130)
                If _duplicate = "Y" Then
                    'As per Original copy No need
                    If CopyName = "ORIGINAL" Then 'Or CopyName = "OFFICE"
                    Else
                        e.Graphics.RotateTransform(40)
                        Dim myfont As New Font("Arial", 80)
                        Dim myBrush As New SolidBrush(Color.FromArgb(30, 0, 0, 255))
                        e.Graphics.DrawString("Duplicate Copy", myfont, myBrush, 250, 30)
                        e.Graphics.ResetTransform()
                    End If
                End If
                FirstY1 = TopY
                TitleSmithIssueReceiptSmithCatNameA4(e.Graphics, e)
                PrintDocumnt_NewCopyName(g1, e, "")
                g.DrawLine(Pens.Silver, 20, TopY, 775, TopY)
                TopY = TopY + 18
                RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
                If dsSmithIssueReceipt.Tables(catNamegroup).Rows.Count > 0 And dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows.Count > 0 Then 'DTISSREC
                    For PagecountSale = PagecountSale To dsSmithIssueReceipt.Tables(catNamegroup).Rows.Count - 1 'DTISSREC
                        With dsSmithIssueReceipt.Tables(catNamegroup).Rows(PagecountSale) 'DTISSREC
                            If PrintRPU_IPU = True Then
                                If .Item("TABLENAME").ToString = "RECEIPT" Then
                                    Continue For
                                End If
                                boolPrintSmithA4IssueTax = False
                            Else
                                If dtIssRecDistinct.Rows.Count > 1 Then
                                    If .Item("TABLENAME").ToString = "ISSUE" And boolPrintSmithA4Issue = True Then
                                        TopY = TopY + 25
                                        'g.DrawString("ISSUE", fontBoldTitle, BlackBrush, 425, TopY, LeftFormat)
                                        TopY = TopY + 25
                                        boolPrintSmithA4Issue = False
                                        GoTo NextImageIssue
                                    End If
                                End If
                            End If
                            If .Item("TAXID").ToString <> "" Then
                            Else
                                g.DrawString(IIf(Val(.Item("SNO").ToString()) > 0, .Item("SNO").ToString(), ""), fontRegularsmall, BlackBrush, smc1, TopY, LeftFormat)
                            End If
                            g.DrawString(.Item("ITEMNAME").ToString(), fontRegularsmall, BlackBrush, smc2, TopY, LeftFormat)
                            g.DrawString(.Item("HSN").ToString(), fontRegularsmall, BlackBrush, smc3, TopY, LeftFormat)
                            g.DrawString(IIf(Val(.Item("PCS").ToString()) <> 0, .Item("PCS").ToString(), ""), fontRegularsmall, BlackBrush, smc10, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontRegularsmall, BlackBrush, smc4, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("LESSWT").ToString()) <> 0, .Item("LESSWT").ToString(), ""), fontRegularsmall, BlackBrush, smc5, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, smc6, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("WASTAGE").ToString()) <> 0, .Item("WASTAGE").ToString(), ""), fontRegularsmall, BlackBrush, smc7, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("MC").ToString()) <> 0, .Item("MC").ToString(), ""), fontRegularsmall, BlackBrush, smc8, TopY, rAlign)
                            If .Item("TAXID").ToString <> "" Then
                                Dim printGst As String = ""
                                If .Item("TAXID").ToString = "CG" Then
                                    printGst = "CGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%"
                                ElseIf .Item("TAXID").ToString = "SG" Then
                                    printGst = "SGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%"
                                ElseIf .Item("TAXID").ToString = "IG" Then
                                    printGst = "IGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%"
                                ElseIf .Item("TAXID").ToString = "TC" Then
                                    printGst = "TCS " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%"
                                ElseIf .Item("TAXID").ToString = "TD" Then
                                    If .Item("TABLENAME").ToString = "ISSUE" Then
                                        printGst = "TDS "
                                    Else
                                        printGst = "LESS : TDS " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%"
                                    End If
                                ElseIf .Item("TAXID").ToString = "ZRO" Then
                                    printGst = "Rounded Off"
                                Else
                                    printGst = ""
                                End If
                                g.DrawString(printGst, fontRegularsmall, BlackBrush, smc7, TopY, rAlign)
                                g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) <> 0, .Item("AMOUNT").ToString(), ""), fontRegularsmall, BlackBrush, smc9, TopY, rAlign)
                            Else
                                g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) <> 0, .Item("AMOUNT").ToString(), ""), fontRegularsmall, BlackBrush, smc9, TopY, rAlign)
                            End If
                            TopY = TopY + 18
                            If TopY > 850 Then
                                PagecountSale = PagecountSale + 1
                                g.DrawString("Continue.... ", fontRegularsmall, Brushes.Black, c1, 490)
                                FooterSmithIssueReceiptA4(e.Graphics, e, FirstY1, TopY)
                                TopY = TOPYSMITHISSUEVALUE
                                e.HasMorePages = True
                                Return
                                'Exit For
                            End If
                        End With
                    Next
                End If
NextImageIssue:
                If dsSmithIssueReceipt.Tables("TAXTRAN").Rows.Count > 0 Then
                    For t As Integer = 0 To dsSmithIssueReceipt.Tables("TAXTRAN").Rows.Count - 1
                        With dsSmithIssueReceipt.Tables("TAXTRAN").Rows(t)
                            If Val(.Item("TAXAMOUNT").ToString()) = 0 Then Continue For
                            If dtIssRecDistinct.Rows.Count > 1 Then
                                If boolPrintSmithA4IssueTax = True Then ' RECEIPT TABLE
                                    If dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(t).Item("RECPAY").ToString = "R" Then
                                        GoTo funcTaxTran
                                    Else
                                        Continue For
                                    End If
                                ElseIf boolPrintSmithA4IssueTax = False Then 'ISSUE TABLE
                                    If dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(t).Item("RECPAY").ToString = "I" Then
                                        GoTo funcTaxTran
                                    Else
                                        Continue For
                                    End If
                                End If
                            End If
funcTaxTran:
                            If .Item("TAXID").ToString() = "CG" Then
                                g.DrawString("CGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, TopY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smc9, TopY, rAlign)
                            ElseIf .Item("TAXID").ToString() = "SG" Then
                                g.DrawString("SGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, TopY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smc9, TopY, rAlign)
                            ElseIf .Item("TAXID").ToString() = "TC" Then
                                g.DrawString("TCS " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "", fontRegularsmall, BlackBrush, sm8, TopY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smc9, TopY, rAlign)
                            ElseIf .Item("TAXID").ToString() = "IG" Then
                                g.DrawString("IGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, TopY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smc9, TopY, rAlign)
                            Else
                                g.DrawString("LESS : TDS " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, TopY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smc9, TopY, rAlign)
                            End If
                        End With
                        TopY = TopY + 18
                    Next
                End If
                If dsSmithIssueReceipt.Tables("TAXTRAN2").Rows.Count > 0 Then
                    g.DrawString("GST Reversal on wastage", fontBold, BlackBrush, sm5, TopY, rAlign)
                    TopY = TopY + 18
                    For t As Integer = 0 To dsSmithIssueReceipt.Tables("TAXTRAN2").Rows.Count - 1
                        With dsSmithIssueReceipt.Tables("TAXTRAN2").Rows(t)
                            If .Item("TAXID").ToString() = "CG" Then
                                g.DrawString("CGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, TopY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smc9, TopY, rAlign)
                            ElseIf .Item("TAXID").ToString() = "SG" Then
                                g.DrawString("SGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, TopY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smc9, TopY, rAlign)
                            ElseIf .Item("TAXID").ToString() = "IG" Then
                                g.DrawString("IGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, TopY, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smc9, TopY, rAlign)
                            End If
                        End With
                        TopY = TopY + 18
                    Next
                End If
                If dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows.Count > 0 Then
                    For Rec As Integer = 0 To dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows.Count - 1
                        If dtIssRecDistinct.Rows.Count > 1 Then
                            If boolPrintSmithA4IssueTax = True Then ' RECEIPT TABLE
                                If dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(Rec).Item("RECPAY").ToString = "R" Then
                                    GoTo funIssRecTotal
                                Else
                                    Continue For
                                End If
                            ElseIf boolPrintSmithA4IssueTax = False Then 'ISSUE TABLE
                                If dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(Rec).Item("RECPAY").ToString = "I" Then
                                    GoTo funIssRecTotal
                                Else
                                    Continue For
                                End If
                            End If
                        End If
funIssRecTotal:
                        TopY = TopY + 18
                        Dim Remark1 As String = ""
                        Dim Remark2 As String = ""
                        Remark1 = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("REMARK1").ToString
                        Remark2 = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("REMARK2").ToString
                        g.DrawString(Remark1 & " " & Remark2, fontRegularsmall, BlackBrush, sm2, TopY, LeftFormat) 'Top
                        TopY = 980 ' 1027
                        Dim TcsMinTemp As Double = 0
                        If PrintRPU_IPU = True Then
                            TcsMinTemp = Val(dsSmithIssueReceipt.Tables("DTISSREC").Compute("SUM(AMOUNT)", "TAXID='TC'").ToString)
                        End If
                        g.DrawString(CopyName, New Font("Impact", 18, FontStyle.Regular), BlackBrush, sm2, TopY - 27, LeftFormat) 'Top
                        g.DrawLine(Pens.Silver, 20, TopY, 775, TopY)
                        With dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(Rec)
                            g.DrawString(.Item("TOTAL").ToString(), fontRegularsmall, BlackBrush, smc2, TopY, LeftFormat)
                            g.DrawString(IIf(Val(.Item("PCS").ToString()) <> 0, .Item("PCS").ToString(), ""), fontRegularsmall, BlackBrush, smc10, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontRegularsmall, BlackBrush, smc4, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("LESSWT").ToString()) <> 0, .Item("LESSWT").ToString(), ""), fontRegularsmall, BlackBrush, smc5, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, smc6, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("MC").ToString()) <> 0, .Item("MC").ToString(), ""), fontRegularsmall, BlackBrush, smc8, TopY, rAlign)
                            Dim IssRecAmt As Double = 0
                            IssRecAmt = Val(.Item("AMOUNT").ToString()) - TcsMinTemp
                            g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) <> 0, Format(IssRecAmt, "0.00"), ""), fontRegularsmall, BlackBrush, smc9, TopY, rAlign)
                        End With
                        TopY = TopY + 18
                        g.DrawLine(Pens.Silver, 20, TopY, 775, TopY)
                        boolPrintSmithA4IssueTax = False
                        Exit For
                    Next
                End If
                FooterSmithIssueReceiptA4(e.Graphics, e, FirstY1, TopY)
                If boolPrintSmithA4Issue = False Then
                    If PagecountSale <= dsSmithIssueReceipt.Tables(catNamegroup).Rows.Count - 1 Then
                        'PagecountSale = PagecountSale + 1'Don't Use this Line
                        TopY = TOPYSMITHISSUEVALUE
                        e.HasMorePages = True
                        Return
                    End If
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            Exit Sub
        End Try
    End Sub

    Private Sub PrtSmithA4First_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrtSmithA4First.PrintPage
        PrintDocumnt_Smith_Category(e.Graphics, e, "ORIGINAL", "DTISSRECCAT")
    End Sub

    Private Sub PrtSmithA4Second_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrtSmithA4Second.PrintPage
        PrintDocumnt_smith_ItemName(e.Graphics, e, "", "DTISSREC") 'SUPPLIER'GRN
    End Sub

    Private Sub PrtSmithA4Third_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrtSmithA4Third.PrintPage
        PrintDocumnt_smith_ItemName(e.Graphics, e, "INTERNAL", "DTISSREC") '
    End Sub

    Private Sub PrtSmithA4Fourth_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrtSmithA4Fourth.PrintPage
        PrintDocumnt_Smith_Category(e.Graphics, e, "OFFICE", "DTISSRECCAT")
    End Sub

#End Region
End Class