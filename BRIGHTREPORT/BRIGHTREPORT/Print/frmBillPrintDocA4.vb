Imports System.Data.OleDb
Imports System.IO
Public Class frmBillPrintDocA4

#Region " Variable"
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
    Dim strCompanyGst As String = ""
    Dim strEstPrinterName As String = GetAdmindbSoftValue("ESTPRINTER_" & systemId, "")
    Dim printCustoInfo As Boolean = True
#End Region

#Region " User Define Function"

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
            Dim Cgst As Double = 0
            Dim Sgst As Double = 0
            If True Then
                strsql = " EXEC " & cnAdminDb & "..SP_BILL_BILLPRINTVIEW_NEW "
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
                ''strsql += vbCrLf + " ,@IGST = 0 "
                ''strsql += vbCrLf + " ,@GSTPER = '" & gstPer & "' "
                cmd = New OleDbCommand(strsql, cn)
                cmd.ExecuteNonQuery()
            End If
        End If

        strsql = "UPDATE TEMPTABLEDB..[TEMP" & SystemName & "SALEINVOICE] SET RATE='CASH PAID' WHERE AMOUNT<0 AND RATE='CASH RECEIVED'"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = "SELECT TRANNO FROM TEMPTABLEDB..[TEMP" & SystemName & "SALEINVOICE] WHERE TRANNO IS NOT NULL"
        Tranno = objGPack.GetSqlValue(strsql, "TRANNO")

        strsql = "SELECT BILLPREFIX FROM TEMPTABLEDB..[TEMP" & SystemName & "SALEINVOICE] WHERE TRANNO IS NOT NULL"
        BillPrefix = objGPack.GetSqlValue(strsql, "BILLPREFIX")



        strsql = "SELECT CONVERT(VARCHAR(15),BILLDATE,103) AS BILLDATE FROM TEMPTABLEDB..[TEMP" & SystemName & "SALEINVOICE] WHERE TRANNO IS NOT NULL"
        prtBilldate = objGPack.GetSqlValue(strsql, "BILLDATE")

        strsql = "SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = '" & strBilldate & "' AND BATCHNO = '" & pBatchno & "' "
        NodeId = objGPack.GetSqlValue(strsql, "SYSTEMID")

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
                    Dim objprinterlist As New frmBillPrinterSelect
                    objprinterlist.txtNoofCopies.Visible = True
                    objprinterlist.txtNoofCopies.Text = "1"
                    If objprinterlist.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDialog1.PrinterSettings.PrinterName = objprinterlist.cmbrecprinter.Text
                        PrintDialog1.Document = PrtDoc
                        PrtDoc.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = Printing.PaperKind.A4
                        PrtDoc.PrinterSettings.Copies = 1
                        PrtDoc.PrintController = New System.Drawing.Printing.StandardPrintController
                        If Val(objprinterlist.txtNoofCopies.Text) > 1 Then
                            For cnt As Integer = 0 To Val(objprinterlist.txtNoofCopies.Text) - 1
                                PagecountSale = 0
                                TopY = 135
                                PrtDoc.Print()
                            Next
                        Else
                            PrtDoc.Print()
                        End If

                        strsql = "SELECT COUNT(*)CNT FROM " & cnAdminDb & "..OUTSTANDING O WHERE BATCHNO='" & pBatchno & "'"
                        strsql += vbCrLf + " AND PAYMODE IN('AR','AP','MR','DR','MP')"
                        strsql += vbCrLf + " AND EXISTS(SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE BATCHNO=O.BATCHNO"
                        strsql += vbCrLf + " UNION SELECT 1 FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO=O.BATCHNO AND TRANTYPE IN ('SR','PU')  "
                        strsql += vbCrLf + " )"
                        If Val(objGPack.GetSqlValue(strsql, "CNT", 0)) > 0 Then  ''OG WITH ADVANCE
                            printCustoInfo = True
                            AdvancePrint(pBatchno, strBilldate)
                            PrtDiaAdvance.PrinterSettings.PrinterName = objprinterlist.cmbrecprinter.Text
                            PrtDiaAdvance.Document = printAdvance
                            printAdvance.PrinterSettings.Copies = 1
                            printAdvance.PrintController = New System.Drawing.Printing.StandardPrintController()
                            printAdvance.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = Printing.PaperKind.A4
                            If Val(objprinterlist.txtNoofCopies.Text) > 1 Then
                                For cnt As Integer = 0 To Val(objprinterlist.txtNoofCopies.Text) - 1
                                    PagecountSale = 0
                                    TopY = 135
                                    printAdvance.Print()
                                Next
                            Else
                                PagecountSale = 0
                                TopY = 135
                                printAdvance.Print()
                            End If
                        End If

                    End If
                    Exit Sub
                End If
            Next
        End If
    End Sub
#End Region

#Region " Constructor"
    Public Sub New(ByVal type As String, ByVal batchno As String, ByVal trandate As String, ByVal duplicate As String)
        InitializeComponent()
        pBatchno = batchno
        Trantype = type ' CHECKING HEADLING ONLY
        _duplicate = duplicate
        strBilldate = trandate '
        Dim dr As DataRow = Nothing
        strsql = "SELECT ISNULL(GSTNO,'') EXCISENO/*,ISNULL(ASIGN,'') ASIGN*/,isnull(GSTNO,'') GSTNO  FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID = '" & strCompanyId & "'" 'SSB
        dr = GetSqlRow(strsql, cn)
        If Not dr Is Nothing Then
            GSTNO = dr.Item("EXCISENO").ToString
            COMPANYGSTNO = dr.Item("EXCISENO").ToString
            ''UserImgbyte = dr.Item("ASIGN")
            ''ImgPrint = New MemoryStream(UserImgbyte)
            strCompanyGst = dr.Item("GSTNO")
        End If
        Dim chkEInvTran As Double = 0
        strsql = " SELECT COUNT(*) CNT FROM " & cnStockDb & ".SYS.TABLES WHERE NAME ='EINVTRAN' "
        chkEInvTran = Val(GetSqlValue(cn, strsql).ToString)
        If chkEInvTran > 0 Then
            strsql = " SELECT * FROM " & cnStockDb & "..EINVTRAN WHERE BATCHNO = '" & pBatchno & "'"
            dtInvTran = GetSqlTable(strsql, cn)
        Else
            dtInvTran = New DataTable
        End If
        Rules(0) = " No E-way bill is required to be generated as the Goods covered under this Invoice are exempted as per "
        Rules(1) = " Serial No. 150/151 to the Annexure to Rule 138(14) of the CGST Rules 2017 "
        If type = "POS" Then
            strsql = "SELECT COUNT(*)CNT FROM " & cnAdminDb & "..OUTSTANDING O WHERE BATCHNO='" & pBatchno & "'"
            strsql += vbCrLf + " AND PAYMODE IN('AR','AP','MR','DR','MP')"
            strsql += vbCrLf + " AND NOT EXISTS(SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE BATCHNO=O.BATCHNO"
            strsql += vbCrLf + " UNION SELECT 1 FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO=O.BATCHNO AND TRANTYPE IN ('SR','PU') "
            strsql += vbCrLf + " )"
            If Val(objGPack.GetSqlValue(strsql, "CNT", 0)) > 0 Then  ''OG WITH ADVANCE
                printCustoInfo = True
                AdvancePrint(pBatchno, strBilldate)

                Dim objprinterlist As New frmBillPrinterSelect
                objprinterlist.txtNoofCopies.Visible = True
                objprinterlist.txtNoofCopies.Text = "1"
                If objprinterlist.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    PrtDiaAdvance.PrinterSettings.PrinterName = objprinterlist.cmbrecprinter.Text
                    PrtDiaAdvance.Document = printAdvance
                    printAdvance.PrinterSettings.Copies = 1
                    printAdvance.PrintController = New System.Drawing.Printing.StandardPrintController()
                    printAdvance.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = Printing.PaperKind.A4
                    If Val(objprinterlist.txtNoofCopies.Text) > 1 Then
                        For cnt As Integer = 0 To Val(objprinterlist.txtNoofCopies.Text) - 1
                            PagecountSale = 0
                            TopY = 135
                            printAdvance.Print()
                        Next
                    Else
                        printAdvance.Print()
                    End If
                End If
                ''printAdvance.Print()
            Else
                Print(pBatchno)
            End If
        ElseIf type = "EST" Then
            If pBatchno.Contains(":") Then
                Dim strEstNo() As String = pBatchno.Split(":")
                EstNo_SA = strEstNo(0).Replace("S.", "")
                EstNo_PU = strEstNo(1).Replace("P.", "")
                If strEstPrinterName.ToString <> "" Then
                    PrintDialog2.PrinterSettings.PrinterName = strEstPrinterName.ToString
                Else
                    Dim objprinterlist As New frmBillPrinterSelect
                    If objprinterlist.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        PrintDialog2.PrinterSettings.PrinterName = objprinterlist.cmbrecprinter.Text
                    Else
                        Exit Sub
                    End If
                End If
                PrintDialog2.Document = PrtSEst
                PrtSEst.PrinterSettings.Copies = 1
                PrtSEst.PrintController = New System.Drawing.Printing.StandardPrintController
                PrtSEst.Print()
            End If
        End If
    End Sub
#End Region

#Region " Print Events"



    Public Sub Footer(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        Dim T1 As String = "Page " & NoofPrint.ToString
        '' Horizontal Line
        g.DrawLine(Pens.Silver, 20.0F, 207.0F, 20.0F, 1060.0F) 'LEFT TOP
        g.DrawLine(Pens.Silver, 775.0F, 207.0F, 775.0F, 1060.0F) 'RIGHT TOP
        'g.DrawLine(Pens.Silver, 20.0F, 777.0F, 20.0F, 1060.0F) 'LEFT BOTTOM
        'g.DrawLine(Pens.Silver, 775.0F, 777.0F, 775.0F, 1060.0F) 'RIGHT BOTTOM
        '' Vertical Line
        Select Case Trantype
            Case "REP"
                g.DrawString("NOTE : ITEM(S) MENTIONED ABOVE WILL BE DELIVERED ONLY BY PRODUCING ORIGINAL VOUCHER", fontBold, Brushes.Black, c2, 470)
            Case "ORD"
                g.DrawString("NOTE : ORIGINAL ORDER BOOKING RECEIPT SHOULD BE PRODUCED AT THE TIME OF ITEM(S) DELIVERY", fontBold, Brushes.Black, c2, 470)
            Case Else
        End Select
        'g.DrawLine(Pens.Silver, 20.0F, 490.0F, 775.0F, 490.0F)
        Select Case Trantype
            Case "POS"
        End Select
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
        End Select
        g.DrawString("E&O.E      Tax is Payable on Reverse Charge : NO ", fontRegular, Brushes.Black, c1, 1063)
        g.DrawString(NodeId, fontRegular, Brushes.Black, c3, 1063)
        If placeofSupply <> "" Then
            g.DrawString("PLACE OF SUPPLY : " & placeofSupply, fontRegular, Brushes.Black, c3 + 50, 1063)
        End If
        g.DrawString("Cashier", fontRegular, Brushes.Black, c7, 1063)
        ''If ImgPrint.Length > 0 Then
        ''    g.DrawString("For " & strCompanyName & "", New Font("Times New Roman", 8, FontStyle.Regular), Brushes.Black, c6, 1083)
        ''    Dim Image1 As Image = Image.FromStream(ImgPrint)
        ''    e.Graphics.DrawImage(Image1, 600, 1093, 100, 70)
        ''End If
        g.DrawString(Rules(0).ToString, New Font("Palatino Linotype", 8, FontStyle.Regular), Brushes.Black, c1, 1073)
        g.DrawString(Rules(1).ToString, New Font("Palatino Linotype", 8, FontStyle.Regular), Brushes.Black, c1, 1083)
        If NoofPrint > 1 Then
            g.DrawString(T1, fontRegular, Brushes.Black, c8, 1063)
        End If
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
        g.DrawString("MC", fontRegular, Brushes.Black, c8, TopY, RightFormat)
        g.DrawString("AMOUNT", fontRegular, Brushes.Black, c9, TopY, RightFormat)
        TopY = TopY + 20
        DrawLine(g, TopY, 0)
        TopY = TopY + 10
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
            g.DrawString(Cusname, fontRegular, Brushes.Black, c1, TopY)
            Dim c7I As Integer = c7 - 20
            Dim c8D As Integer = c8 - 22
            Dim c8D2 As Integer = c8D + 7
            Select Case Trantype
                Case "POS"
                    g.DrawString("Invoice No", fontRegular, Brushes.Black, c7I, TopY)
                    g.DrawString(" : ", fontRegular, Brushes.Black, c8D, TopY)
                    g.DrawString(BBill & "" & Bno, fontRegular, Brushes.Black, c8D2, TopY)
                Case "ORD"
                    g.DrawString("Order No", fontRegular, Brushes.Black, c7I, TopY)
                    g.DrawString(" : ", fontRegular, Brushes.Black, c8D, TopY)
                    g.DrawString(BBill & "" & Bno, fontRegular, Brushes.Black, c8D2, TopY)
                Case "REP"
                    g.DrawString("Repair No", fontRegular, Brushes.Black, c7I, TopY)
                    g.DrawString(" : ", fontRegular, Brushes.Black, c8D, TopY)
                    g.DrawString(BBill & "" & Bno, fontRegular, Brushes.Black, c8D2, TopY)
            End Select
            TopY = TopY + 18
            CusAddress1 = Trim(dtCustInfo.Rows(0).Item("DoorNo") & "").ToString & " " & Trim(dtCustInfo.Rows(0).Item("address1") & "").ToString & Trim(dtCustInfo.Rows(0).Item("address2") & "").ToString & " " & Trim(dtCustInfo.Rows(0).Item("area") & "").ToString
            g.DrawString(CusAddress1, New Font("Palatino Linotype", 7, FontStyle.Regular), Brushes.Black, c1, TopY)
            g.DrawString("Date", fontRegular, Brushes.Black, c7I, TopY)
            g.DrawString(" : ", fontRegular, Brushes.Black, c8D, TopY)
            g.DrawString(Bdate, fontRegular, Brushes.Black, c8D2, TopY)
            Select Case Trantype
                Case "ORD"
                    TopY = TopY + 18
                    g.DrawString("Due", fontRegular, Brushes.Black, c7I, TopY)
                    g.DrawString(" : ", fontRegular, Brushes.Black, c8D, TopY)
                    g.DrawString(DueDate, fontRegular, Brushes.Black, c8D2, TopY)
                    TopY = TopY - 18
            End Select
            TopY = TopY + 18
            CusArea = Trim(dtCustInfo.Rows(0).Item("City") & "").ToString & " " & Trim(dtCustInfo.Rows(0).Item("PinCode") & "").ToString
            g.DrawString(CusArea, New Font("Palatino Linotype", 7, FontStyle.Regular), Brushes.Black, c1, TopY)
            Select Case Trantype
                Case "POS"
                    If TrantypeMI = "RD" Then
                        g.DrawString("REPAIR DELIVERY", fontBoldTitle, Brushes.Black, c3, TopY)
                    ElseIf TrantypeMI = "MI" Then
                        g.DrawString("MISC ISSUE", fontBoldTitle, Brushes.Black, c3, TopY)
                    Else
                        g.DrawString("  TAX INVOICE", fontBoldTitle, Brushes.Black, c3, TopY) 'SALES
                    End If
                Case "ORD"
                    g.DrawString("ORDER BOOKING", fontBoldTitle, Brushes.Black, c3, TopY)
                Case "REP"
                    g.DrawString("REPAIR BOOKING", fontBoldTitle, Brushes.Black, c3, TopY)
            End Select
            TopY = TopY + 18
            If Trim(dtCustInfo.Rows(0).Item("phoneres") & "").ToString <> "" Then
                CusPhone_Mobi = Trim(dtCustInfo.Rows(0).Item("phoneres") & "").ToString & " " & Trim(dtCustInfo.Rows(0).Item("mobile") & "").ToString & " " & Trim(IIf(dtCustInfo.Rows(0).Item("PAN").ToString <> "", "PAN :" & dtCustInfo.Rows(0).Item("PAN").ToString, "")).ToString
                g.DrawString(CusPhone_Mobi, New Font("Palatino Linotype", 7, FontStyle.Regular), Brushes.Black, c1, TopY)
                TopY = TopY + 18
            Else
                CusPhone = Trim(dtCustInfo.Rows(0).Item("mobile").ToString) & " " & Trim(IIf(dtCustInfo.Rows(0).Item("PAN").ToString <> "", "PAN :" & dtCustInfo.Rows(0).Item("PAN").ToString, "")).ToString
                g.DrawString(CusPhone, New Font("Palatino Linotype", 7, FontStyle.Regular), Brushes.Black, c1, TopY)
                TopY = TopY + 18
            End If
            If Trim(dtCustInfo.Rows(0).Item("GSTNO") & "").ToString <> "" Then
                CusGSTNo = dtCustInfo.Rows(0).Item("GSTNO").ToString
                g.DrawString("GSTIN : " & CusGSTNo, New Font("Palatino Linotype", 7, FontStyle.Regular), Brushes.Black, c1, TopY)
                If dtInvTran.Rows.Count > 0 Then
                    Dim cusIRNNo As String = dtInvTran.Rows(0).Item("IRN").ToString
                    g.DrawString("IRN : " & cusIRNNo, New Font("Palatino Linotype", 7, FontStyle.Regular), Brushes.Black, h1 - 95, TopY)
                End If
                TopY = TopY + 12 '10
            End If
            If dtInvTran.Rows.Count > 0 Then
                'Dim barcodeDesign As New BarcodeLib.Barcode.QRCode
                'Dim LocalTemp As String = System.Environment.GetEnvironmentVariable("temp")
                'Dim SystemName As String
                'SystemName = Environment.MachineName
                'BarcodeTempFileDelete(LocalTemp, SystemName, "")
                'barcodeDesign.Rotate = BarcodeLib.Barcode.RotateOrientation.BottomFacingDown
                'barcodeDesign.Data = dtInvTran.Rows(0)("QRDATA")
                'barcodeDesign.drawBarcode(LocalTemp & "\barcode" & SystemName & ".png")
                'Dim imgstream As New Bitmap(LocalTemp & "\barcode" & SystemName & ".png")
                'e.Graphics.DrawImage(imgstream, c6, TopY - 60, 50, 50)
                'imgstream.Dispose()
                'BarcodeTempFileDelete(LocalTemp, SystemName, "")
            End If
            If dtCustInfo.Rows(0).Item("STATEID").ToString <> "" Then
                Dim Qry As String = ""
                Qry = " SELECT ISNULL(STATENAME,'') STATENAME FROM " & cnAdminDb & "..STATEMAST "
                Qry += vbCrLf + " WHERE STATEID = " & dtCustInfo.Rows(0).Item("STATEID").ToString & " "
                placeofSupply = GetSqlValue(cn, Qry)
            End If
        End If
        DrawLine(g, TopY, 0)
    End Sub
    Private Sub PrtDoc_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrtDoc.PrintPage
        Try
            Dim NoofPage As Integer = 0
            Dim rAlign As New StringFormat
            rAlign.Alignment = StringAlignment.Far
            NoofPrint += 1
            Using g As Graphics = e.Graphics
                g.DrawString("GSTIN : " & GSTNO, fontRegular, Brushes.Black, 336, TopY)
                g.DrawString("STATE CODE : " & Mid(GSTNO, 1, 2) & "", fontRegular, Brushes.Black, 360, TopY + 18)
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
                            End If
                            If .Item("RESULT").ToString = "3.0" And .Item("TYPE").ToString = "11.0" And .Item("COLHEAD").ToString = "G" Then
                                g.DrawLine(Pens.Silver, 340, (TopY - 2), c9, (TopY - 2))
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
                            If (.Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" And Val(.Item("QTY").ToString) >= 2) _
                            Or (.Item("TYPE").ToString = "2" And .Item("COLHEAD").ToString = "G") _
                            Or (.Item("TYPE").ToString = "3" And .Item("COLHEAD").ToString = "G") Then
                                g.DrawLine(Pens.Silver, 340, (TopY + 2), c9, (TopY + 2))
                            End If
                            If TopY >= 489 Then
                                PagecountSale = PagecountSale + 1
                                TopY = 135 ' TOP AGAIN STARTING POSITION
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, 513)
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, 1075)
                                e.HasMorePages = True
                                Exit For
                            End If
                            'New Line Start
                            If .Item("CALTYPE").ToString = "HA" And .Item("RESULT").ToString = "1.1" Then
                                TopY = TopY + 8
                            Else
                                TopY = TopY + 17
                            End If
                            If .Item("RESULT").ToString = "1.0" And (.Item("CALTYPE").ToString = "F" Or .Item("CALTYPE").ToString = "R") Then '.Item("COLHEAD").ToString <> "G" And .Item("COLHEAD").ToString <> "P" And .Item("COLHEAD").ToString <> "T"
                                g.DrawString("PCS COST ITEM NO EXCHANGE", fontRegularsmall, BlackBrush, c2, TopY, LeftFormat)
                                TopY = TopY + 17
                            End If
                            If .Item("RESULT").ToString = "1.0" And (.Item("BILLNAME").ToString = "Y") Then '.Item("COLHEAD").ToString <> "G" And .Item("COLHEAD").ToString <> "P" And .Item("COLHEAD").ToString <> "T"
                                g.DrawString("Fixing of Kemp, Kundan Stones needs wax, Hence at the time of melting weight loss will be there upto 10%..", fontRegularsmall, BlackBrush, c2, TopY, LeftFormat)
                                TopY = TopY + 17
                                'End If
                            End If
                            If TopY >= 489 Then
                                PagecountSale = PagecountSale + 1
                                TopY = 135 ' TOP AGAIN STARTING POSITION
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, 1075)
                                e.HasMorePages = True
                                Exit For
                            End If
                            If (.Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" And Val(.Item("QTY").ToString) >= 2) Or
                            (.Item("TYPE").ToString = "3" And .Item("COLHEAD").ToString = "G") Then
                                g.DrawLine(Pens.Silver, 340, (TopY - 2), c9, (TopY - 2))
                            End If
                            If (.Item("RESULT").ToString = "3.10" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G") Then
                                g.DrawLine(Pens.Silver, 710, (TopY - 2), c9, (TopY - 2))
                            End If
                            If (.Item("TYPE").ToString = "4" And .Item("COLHEAD").ToString = "T") Then
                                If Val(.Item("AMOUNT").ToString) <> 0 Then
                                    g.DrawLine(Pens.Silver, c8, TopY, c9, TopY)
                                End If
                            End If
                            If .Item("RESULT").ToString = "3.0" And .Item("TYPE").ToString = "11.0" And .Item("COLHEAD").ToString = "G" Then
                                g.DrawLine(Pens.Silver, 340, (TopY - 2), c9, (TopY - 2))
                            End If
                            'If NoofPage > 14 Then
                            If TopY >= 489 Then
                                PagecountSale = PagecountSale + 1
                                TopY = 135 ' TOP AGAIN STARTING POSITION
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

    Private Sub PrtSEst_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrtSEst.PrintPage
        Using g As Graphics = e.Graphics
            Dim dtBillPrint As New DataTable
            Dim brush As New SolidBrush(Color.Black)
            Dim pen As New Pen(brush)
            Dim fontRegular As New Font("Times New Roman", 9, FontStyle.Regular)
            Dim fontRegularsmall As New Font("Times New Roman", 8, FontStyle.Regular)
            Dim fontBold As New Font("Times New Roman", 9, FontStyle.Bold)
            Dim fontBoldtotal As New Font("Times New Roman", 11, FontStyle.Bold)
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
            Dim _strDesc1 As String = ""
            Dim strTranno As String = ""
            Dim sa_totalamt As Double = 0
            Dim Pu_totalamt As Double = 0
            If Val(EstNo_SA) <> 0 Then
                c1 = 10
                c2 = 80
                c3 = 90
                c4 = 140
                c5 = 190
                c6 = 240
                y1 = 20
                strsql = " SELECT * "
                strsql += vbCrLf + " FROM " & cnStockDb & "..ESTISSUE AS T "
                strsql += vbCrLf + " WHERE "
                strsql += vbCrLf + " TRANDATE='" & strBilldate & "' "
                strsql += vbCrLf + " AND TRANNO='" & EstNo_SA & "' "
                strsql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                dtBillPrint = New DataTable
                cmd = New OleDbCommand(strsql, cn)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtBillPrint)
                If dtBillPrint.Rows.Count = 0 Then Exit Sub
                _strDesc = ""
                ''y1 = y1 + 20
                ''g.DrawString(strTranno.ToString(), New Font("Times New Roman", 7, FontStyle.Bold), brush, 10, y1)
                y1 = y1 + 20
                g.DrawString("ESTIMATE SLIP", New Font("Times New Roman", 12, FontStyle.Bold), brush, 40, y1)
                y1 = y1 + 20
                g.DrawString("EST No", fontRegular, brush, c1, y1)
                g.DrawString(":", fontRegular, brush, c2, y1)
                g.DrawString(EstNo_SA.ToString, fontRegular, brush, c3, y1)
                y1 = y1 + 20
                strsql = " SELECT TOP 1 SUBSTRING(CONVERT(VARCHAR,UPTIME,109),13,5)+SUBSTRING(CONVERT(VARCHAR,UPTIME,109),25,5) AS BILLTIME, "
                strsql += vbCrLf + " CONVERT(VARCHAR,TRANNO) AS TRANNO,CONVERT(VARCHAR(12),TRANDATE,103) AS TRANDATE "
                strsql += vbCrLf + " FROM " & cnStockDb & "..ESTISSUE AS T WHERE "
                strsql += vbCrLf + " TRANDATE='" & strBilldate & "' AND TRANNO='" & EstNo_SA & "' "
                strsql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
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
                g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                y1 = y1 + 15
                g.DrawString("Descrip", fontRegular, brush, c1, y1)
                g.DrawString("Weight", fontRegular, brush, c3, y1)
                g.DrawString("Wast", fontRegular, brush, c4, y1)
                g.DrawString("M.c", fontRegular, brush, c5, y1)
                g.DrawString("Amount", fontRegular, brush, c6, y1)
                y1 = y1 + 15
                g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                For k As Integer = 0 To dtBillPrint.Rows.Count - 1
                    _strDesc1 = ""
                    strsql = " SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST "
                    strsql += vbCrLf + " WHERE ITEMID='" & dtBillPrint.Rows(k)("ITEMID").ToString & "' "
                    strsql += vbCrLf + " AND SUBITEMID='" & dtBillPrint.Rows(k)("SUBITEMID").ToString & "' "
                    _strDesc = objGPack.GetSqlValue(strsql).ToString
                    If _strDesc1 = "" Then
                        strsql = " SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
                        strsql += vbCrLf + " WHERE ITEMID='" & dtBillPrint.Rows(k)("ITEMID").ToString & "' "
                        _strDesc1 = objGPack.GetSqlValue(strsql)
                    End If
                    y1 = y1 + 15
                    g.DrawString(Mid(_strDesc1, 1, 10), fontRegularsmall, brush, c1, y1)
                    If Val(dtBillPrint.Rows(k)("GRSWT").ToString) <> Val(dtBillPrint.Rows(k)("NETWT").ToString) Then
                        g.DrawString(Format(Val(dtBillPrint.Rows(k)("NETWT").ToString), "0.000"), fontRegular, brush, c3, y1)
                    Else
                        g.DrawString(Format(Val(dtBillPrint.Rows(k)("GRSWT").ToString), "0.000"), fontRegular, brush, c3, y1)
                    End If
                    If Val(dtBillPrint.Rows(k)("WASTAGE").ToString) <> 0 Then
                        g.DrawString(Format(Val(dtBillPrint.Rows(k)("WASTAGE").ToString), "0.000"), fontRegular, brush, c4, y1)
                    End If
                    If Val(dtBillPrint.Rows(k)("WASTAGE").ToString) <> 0 Then
                        g.DrawString(Format(Val(dtBillPrint.Rows(k)("WASTAGE").ToString), "0.000"), fontRegular, brush, c4, y1)
                    End If
                    If Val(dtBillPrint.Rows(k)("MCHARGE").ToString) <> 0 Then
                        g.DrawString(Format(Val(dtBillPrint.Rows(k)("MCHARGE").ToString), "0.000"), fontRegular, brush, c5, y1)
                    End If
                    g.DrawString(Format(Val(dtBillPrint.Rows(k)("AMOUNT").ToString), "0.00"), fontRegular, brush, c6, y1)
                    If _strDesc <> "" Then
                        y1 = y1 + 15
                        g.DrawString(_strDesc.ToString, fontRegularsmall, brush, c1, y1)
                    End If

                    strsql = " SELECT * FROM " & cnStockDb & "..ESTISSSTONE AS E WHERE ISSSNO='" & dtBillPrint.Rows(k)("SNO").ToString & "' "
                    Dim dtBillPrintStone As DataTable
                    dtBillPrintStone = New DataTable
                    cmd = New OleDbCommand(strsql, cn)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dtBillPrintStone)
                    For i As Integer = 0 To dtBillPrintStone.Rows.Count - 1
                        y1 = y1 + 20
                        strsql = " SELECT TOP 1 SUBSTRING(SUBITEMNAME,1,10) SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST "
                        strsql += vbCrLf + " WHERE ITEMID='" & dtBillPrintStone.Rows(i)("STNITEMID").ToString & "' "
                        strsql += vbCrLf + " AND SUBITEMID='" & dtBillPrintStone.Rows(i)("STNSUBITEMID").ToString & "' "
                        _strDesc = objGPack.GetSqlValue(strsql).ToString
                        If _strDesc = "" Then
                            strsql = " SELECT TOP 1 SUBSTRING(ITEMNAME,1,10) ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
                            strsql += vbCrLf + " WHERE ITEMID='" & dtBillPrintStone.Rows(i)("STNITEMID").ToString & "' "
                            _strDesc = objGPack.GetSqlValue(strsql).ToString
                        End If
                        If _strDesc <> "" Then
                            g.DrawString(_strDesc.ToString, fontRegular, brush, c1, y1)
                            g.DrawString(Format(Val(dtBillPrintStone.Rows(i)("STNPCS").ToString), "#0") & "Pc", fontRegular, brush, c3, y1)
                            g.DrawString(Format(Val(dtBillPrintStone.Rows(i)("STNWT").ToString), "#0.000") & "X" _
                                             & Format(Val(dtBillPrintStone.Rows(i)("STNRATE").ToString), "0.00"), fontRegular, brush, c4, y1)

                            g.DrawString(Format(Val(dtBillPrintStone.Rows(i)("STNAMT").ToString), "0.00"), fontRegular, brush, c6, y1)
                            '' y1 = y1 + 15
                        End If
                    Next
                    y1 = y1 + 15
                    g.DrawString(dtBillPrint.Rows(k)("ITEMID").ToString + IIf(dtBillPrint.Rows(k)("TAGNO").ToString <> "", "-" + dtBillPrint.Rows(k)("TAGNO").ToString, ""), fontRegular, brush, c1, y1)
                    g.DrawString(Val(dtBillPrint.Rows(k)("PCS").ToString) & " Pcs", fontRegular, brush, c4, y1)
                Next
                sa_totalamt = Format(Val(dtBillPrint.Compute("SUM(AMOUNT)", "").ToString()) + Val(dtBillPrint.Compute("SUM(TAX)", "").ToString()), "0.00")
                y1 = y1 + 15
                g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                y1 = y1 + 15
                Dim _taxper As Double = 0
                _taxper = Math.Round(Val((Val(dtBillPrint.Compute("SUM(TAX)", "").ToString()) / Val(dtBillPrint.Compute("SUM(AMOUNT)", "").ToString())) * 100), 2)
                g.DrawString("Gst " & _taxper.ToString & "%", fontBold, brush, c5, y1)
                g.DrawString(Format(Val(dtBillPrint.Compute("SUM(TAX)", "").ToString()), "0.00"), fontBold, brush, c6, y1)
                y1 = y1 + 15
                g.DrawString("Total", fontBold, brush, c1, y1)
                If Val(dtBillPrint.Compute("SUM(GRSWT)", "").ToString()) <> Val(dtBillPrint.Compute("SUM(NETWT)", "").ToString()) Then
                    g.DrawString(Format(Val(dtBillPrint.Compute("SUM(NETWT)", "").ToString()), "0.000"), fontBold, brush, c2, y1)
                Else
                    g.DrawString(Format(Val(dtBillPrint.Compute("SUM(GRSWT)", "").ToString()), "0.000"), fontBold, brush, c2, y1)
                End If
                g.DrawString("[Pcs : " & Val(dtBillPrint.Compute("SUM(PCS)", "").ToString()) & "]", fontBold, brush, c4, y1)
                g.DrawString(sa_totalamt, fontBold, brush, c6, y1)
                y1 = y1 + 20
                g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)

            End If
            If Val(EstNo_PU) <> 0 Then
                y1 = y1 + 10
                g.DrawString("SALES TOTAL", fontBoldtotal, brush, c1, y1, lAlign)
                g.DrawString(Format(Val(sa_totalamt.ToString()), "0.00"), fontBoldtotal, brush, c5, y1, rAlign)
            ElseIf Val(sa_totalamt) <> 0 Then
                y1 = y1 + 10
                g.DrawString("TOTAL", fontBoldtotal, brush, c1, y1, lAlign)
                g.DrawString(Format(Val(sa_totalamt.ToString()), "0.00"), fontBoldtotal, brush, c5, y1, rAlign)
            End If
            If Val(EstNo_PU) <> 0 Then
                strsql = " SELECT "
                strsql += vbCrLf + " (SELECT TOP 1 CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=T.CATCODE)CATNAME,"
                strsql += vbCrLf + " T.GRSWT,T.WASTAGE,MCHARGE,AMOUNT,T.DUSTWT,RATE,EMPID,USERID,SNO,PUREXCH,T.WASTPER"
                '
                strsql += vbCrLf + " ,T.OTHERAMT,T.NETWT "
                strsql += vbCrLf + " FROM " & cnStockDb & "..ESTRECEIPT AS T "
                strsql += vbCrLf + " WHERE "
                strsql += vbCrLf + " TRANDATE='" & strBilldate & "' AND TRANNO='" & EstNo_PU & "' AND TRANTYPE='PU' "
                strsql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                dtBillPrint = New DataTable
                cmd = New OleDbCommand(strsql, cn)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtBillPrint)
                If dtBillPrint.Rows.Count = 0 Then Exit Sub
                ''y1 = y1 + 20
                ''g.DrawString(strTranno.ToString(), New Font("Times New Roman", 7, FontStyle.Bold), brush, 10, y1)
                y1 = y1 + 30
                ''g.DrawString("PURCHASE ", New Font("Times New Roman", 12, FontStyle.Bold), brush, 10, y1)
                g.DrawString("PURCHASE ", New Font("Times New Roman", 12, FontStyle.Bold), brush, 10, y1)
                y1 = y1 + 20

                ''strsql = " SELECT TOP 1 SUBSTRING(CONVERT(VARCHAR,UPTIME,109),13,5)+SUBSTRING(CONVERT(VARCHAR,UPTIME,109),25,5) AS BILLTIME, "
                ''strsql += vbCrLf + " CONVERT(VARCHAR,TRANNO) AS TRANNO,CONVERT(VARCHAR(12),TRANDATE,103) AS TRANDATE "
                ''strsql += vbCrLf + " FROM " & cnStockDb & "..ESTRECEIPT AS T WHERE "
                ''strsql += vbCrLf + " TRANDATE='" & strBilldate & "' AND TRANNO='" & EstNo_PU & "' AND TRANTYPE='PU' "
                ''strsql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                ''Dim dtTot As New DataTable
                ''cmd = New OleDbCommand(strsql, cn)
                ''da = New OleDbDataAdapter(cmd)
                ''da.Fill(dtTot)
                ''Dim strTot As String = ""
                ''If dtTot.Rows.Count > 0 Then
                ''    strTot = "Date :" + dtTot.Rows(0).Item("TRANDATE").ToString + "  " + dtTot.Rows(0).Item("BILLTIME").ToString
                ''    ''g.DrawString(strTot.ToString(), fontRegular, brush, 10, y1)
                ''    y1 = y1 + 20
                ''    strTot = "EST No :" + dtTot.Rows(0).Item("TRANNO").ToString
                ''    ''g.DrawString(strTot, fontRegular, brush, c1, y1, lAlign)
                ''End If
                ''y1 = y1 + 20
                ''strTot = ""
                ''strsql = vbCrLf + " SELECT "
                ''strsql += vbCrLf + " TOP 1 SRATE"
                ''strsql += vbCrLf + " FROM " & cnAdminDb & "..RATEMAST WHERE RDATE <= '" & strBilldate & "' AND PURITY='91.6' AND METALID IN ('G')"
                ''strsql += vbCrLf + " ORDER BY RATEGROUP DESC"
                ''dtTot = New DataTable
                ''cmd = New OleDbCommand(strsql, cn)
                ''da = New OleDbDataAdapter(cmd)
                ''da.Fill(dtTot)
                ''If dtTot.Rows.Count > 0 Then
                ''    strTot += "Gold : " + dtTot.Rows(0).Item("SRATE").ToString
                ''End If
                ''strsql = vbCrLf + " SELECT "
                ''strsql += vbCrLf + " TOP 1 SRATE"
                ''strsql += vbCrLf + " FROM " & cnAdminDb & "..RATEMAST WHERE RDATE <= '" & strBilldate & "' AND PURITY='91.6' AND METALID IN ('S')"
                ''strsql += vbCrLf + " ORDER BY RATEGROUP DESC"
                ''dtTot = New DataTable
                ''cmd = New OleDbCommand(strsql, cn)
                ''da = New OleDbDataAdapter(cmd)
                ''da.Fill(dtTot)
                ''If dtTot.Rows.Count > 0 Then
                ''    strTot += " Silver : " + dtTot.Rows(0).Item("SRATE").ToString
                ''End If
                ''''g.DrawString(strTot, fontRegular, brush, c1, y1, lAlign)

                ''y1 = y1 + 20
                ''g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                ''y1 = y1 + 10

                g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                y1 = y1 + 15
                g.DrawString("Descrip", fontRegular, brush, c1, y1)
                g.DrawString("Grswt", fontRegular, brush, c2 - 20, y1)
                g.DrawString("Netwt", fontRegular, brush, c3 + 10, y1)
                g.DrawString("Wast", fontRegular, brush, c4, y1)
                g.DrawString("Rate", fontRegular, brush, c5, y1)
                g.DrawString("Amount", fontRegular, brush, c6, y1)
                y1 = y1 + 15
                g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)

                Dim Type As String = ""

                For i As Integer = 0 To dtBillPrint.Rows.Count - 1
                    With dtBillPrint.Rows(i)
                        y1 = y1 + 18
                        'If Val(.Item("DUSTWT").ToString()) <> 0 Then
                        g.DrawString(.Item("CATNAME").ToString().Substring(0, 6), fontRegularsmall, brush, c1, y1)
                        g.DrawString(Format(Val(.Item("GRSWT").ToString()), "0.000"), fontRegularsmall, brush, c2 - 20, y1)
                        g.DrawString(Format(Val(.Item("NETWT").ToString()), "0.000"), fontRegularsmall, brush, c3 + 10, y1)
                        g.DrawString(Format(Val(.Item("WASTAGE").ToString()), "0.000"), fontRegularsmall, brush, c4, y1)
                        g.DrawString(Format(Val(.Item("RATE").ToString()), "0"), fontRegularsmall, brush, c5, y1)
                        g.DrawString(Format(Val(.Item("AMOUNT").ToString()), "0"), fontRegularsmall, brush, c6, y1)

                        If Val(.Item("DUSTWT").ToString()) <> 0 Then
                            y1 = y1 + 15
                            g.DrawString("DustWt : " & Format(Val(.Item("DUSTWT").ToString()), "0.000") & " gms", fontRegularsmall, brush, c1, y1)
                        End If

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
                                y1 = y1 + 15
                                g.DrawString(Format(Val(dtPuStone.Rows(K).Item("STNWT").ToString()), "0.00").ToString & " ct", fontRegularsmall, brush, c5, y1)
                            Else
                                y1 = y1 + 15
                                g.DrawString(Format(Val(dtPuStone.Rows(K).Item("STNWT").ToString()), "0.00").ToString & " gm", fontRegularsmall, brush, c5, y1)
                            End If
                            y1 = y1 + 15
                            g.DrawString(Format(Val(dtPuStone.Rows(K).Item("STNRATE").ToString()), "0.00"), fontRegularsmall, brush, c5, y1)
                        Next
                    End With
                Next
                y1 = y1 + 15
                g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                y1 = y1 + 15
                g.DrawString("TOT PUR", fontRegularsmall, brush, c1, y1)
                g.DrawString(Format(Val(dtBillPrint.Compute("SUM(GRSWT)", "").ToString()), "0.000"), fontRegularsmall, brush, c2 - 20, y1)
                g.DrawString(Format(Val(dtBillPrint.Compute("SUM(NETWT)", "").ToString()), "0.000"), fontRegularsmall, brush, c3 + 10, y1)
                g.DrawString(Format(Val(dtBillPrint.Compute("SUM(AMOUNT)", "").ToString()), "0"), fontRegularsmall, brush, c6, y1)
                Pu_totalamt = Format(Val(dtBillPrint.Compute("SUM(AMOUNT)", "").ToString()), "0")
                y1 = y1 + 15
                g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                y1 = y1 + 15
                g.DrawString("TOTAL", fontBoldtotal, brush, c1, y1, lAlign)
                g.DrawString("Rs." & Format(Val(sa_totalamt) - Val(Pu_totalamt), "0.00"), fontBoldtotal, brush, c5, y1)
                y1 = y1 + 20
                g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                y1 = y1 + 15
                If dtBillPrint.Rows.Count > 0 Then
                    ''g.DrawString("[ESTPU - " + EstNo_PU + "] [EMPID - " + dtBillPrint.Rows(0)("EMPID").ToString + "] [USERID - " + dtBillPrint.Rows(0)("USERID").ToString + "]" _
                    '', fontRegularsmall, brush, c1, y1)
                    g.DrawString("[ESTPU - " + EstNo_PU + "]", fontRegularsmall, brush, c1, y1)
                Else
                    g.DrawString("[ESTPU - " + EstNo_PU + "]", fontRegularsmall, brush, c1, y1)
                End If
                y1 = y1 + 60
            Else
                y1 = y1 + 60
            End If
        End Using
    End Sub

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

            Dim ItemDetail As String = IIf(Val(dsAdvanceReceipt.Tables("ITEMTAG").Rows(0).Item("ITEMID").ToString) = 0, "", "ITEM DETAIL")
            Dim RateFixed As String = dsAdvanceReceipt.Tables("ITEMTAG").Rows(0).Item("RATENAME").ToString
            Dim RateValue As String = dsAdvanceReceipt.Tables("ITEMTAG").Rows(0).Item("RATE").ToString

            g.DrawString(pname, fontRegular, BlackBrush, c1, TopY, LeftFormat)
            g.DrawString(TranName, fontRegular, BlackBrush, c6, TopY, LeftFormat)
            g.DrawString("   : " & Tranno, fontRegular, BlackBrush, c7, TopY, LeftFormat)

            TopY = TopY + 20

            g.DrawString(Address1, fontRegular, BlackBrush, c1, TopY, LeftFormat)
            g.DrawString("Date", fontRegular, BlackBrush, c6, TopY, LeftFormat)
            g.DrawString("   : " & Billdate, fontRegular, BlackBrush, c7, TopY, LeftFormat)

            TopY = TopY + 20

            g.DrawString(Address2, fontRegular, BlackBrush, c1, TopY, LeftFormat)
            g.DrawString(Field1, fontBoldTitle, Brushes.Black, c3, TopY)
            TopY = TopY + 20
            g.DrawString(Mobile & IIf(Tin <> "", " PAN : " & Tin, "") & IIf(gstno <> "", " GSTNO : " & gstno, ""), fontRegular, BlackBrush, c1, TopY, LeftFormat)
            TopY = TopY + 20
            g.DrawLine(Pens.Silver, c1, TopY, c9, TopY)
            TopY = TopY + 20
            If printCustoInfo = True Then
                g.DrawString(Field2, fontRegular, BlackBrush, c1, TopY, LeftFormat)
                If accode <> "" Then
                    If accode.Length < 50 Then
                        g.DrawString(accode, fontRegular, BlackBrush, c4, TopY, LeftFormat)
                    End If
                    If accode.Length >= 50 Then
                        g.DrawString(Mid(accode, 1, 50), New Font("Palatino Linotype", 8, FontStyle.Regular), BlackBrush, c4, TopY, LeftFormat)
                        g.DrawString(Mid(accode, 51, 50), New Font("Palatino Linotype", 8, FontStyle.Regular), BlackBrush, c4, TopY + 20, LeftFormat)
                        If accode.Length > 100 Then
                            TopY = TopY + 20
                            g.DrawString(Mid(accode, 101), New Font("Palatino Linotype", 8, FontStyle.Regular), BlackBrush, c4, TopY + 20, LeftFormat)
                            TopY = TopY - 20
                        End If
                    End If
                End If
                TopY = TopY + 20
                g.DrawString(AmtWords, fontRegular, BlackBrush, c1, TopY, LeftFormat)
                TopY = TopY + 20
                g.DrawString(Field5, fontRegular, BlackBrush, c1, TopY, LeftFormat)
                TopY = TopY + 20

                If Remarks <> "" Then
                    g.DrawString(Remarks, fontRegular, BlackBrush, c1, TopY, LeftFormat)
                    TopY = TopY + 20
                End If

                If dsAdvanceReceipt.Tables("ISSUE").Rows.Count > 0 Then
                    Dim GrswA As String = dsAdvanceReceipt.Tables("ISSUE").Rows(0).Item("GRSWT")
                    Dim AmtA As String = dsAdvanceReceipt.Tables("ISSUE").Rows(0).Item("AMOUNT")
                    g.DrawString("GRSWT" & Space(6) & ":" & GrswA & "GRSAMT" & Space(5) & ":" & AmtA, fontRegular, BlackBrush, c4, TopY, LeftFormat)
                End If
                TopY = TopY + 20
                printCustoInfo = False
            End If
            'ADVANCE ONLY DISPLAY
            If dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("Field1").ToString.Contains("ADVANCE ") Then
                g.DrawString(ItemDetail, fontBold, BlackBrush, c1, TopY, LeftFormat)
                g.DrawString(RateFixed, fontBold, BlackBrush, c3, TopY, LeftFormat)

                g.DrawString(IIf(Val(RateValue) <> 0, " : " & RateValue, ""), fontBold, BlackBrush, c4, TopY, LeftFormat)

                g.DrawString(PurityType, fontBold, BlackBrush, c5, TopY, LeftFormat)
                If ItemDetail <> "" Then
                    TopY = TopY + 15
                    g.DrawLine(Pens.Black, c1, TopY, 110, TopY)
                End If
                TopY = TopY + 20
            End If
        End If
    End Sub


    Public Sub AdvancePrint(ByVal pBatchno As String, ByVal _Trandate As String)
        strsql = " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "ADVANCERECEIPT]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "ADVANCERECEIPT]"
        strsql += " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "CUSTADVANCERECEIPT]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "CUSTADVANCERECEIPT]"
        strsql += " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "ACCTRANADVANCERECEIPT]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "ACCTRANADVANCERECEIPT]"
        strsql += " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "ITEMTAG]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "ITEMTAG]"
        strsql += " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "ADVANCEPURRET]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "ADVANCEPURRET]"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = " CREATE TABLE TEMPTABLEDB..[TEMP" & SystemName & "ACCTRANADVANCERECEIPT] (ACCODE VARCHAR(250), BATCHNO_N VARCHAR(20))"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = vbCrLf + " SELECT "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'CC' THEN 'CARD ' + AC.SHORTNAME + ' ' + CHQCARDNO + ' :' + LTRIM(SUM(AMOUNT)) ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'AA' THEN 'ADVANCE ' + 'REFER : ' + SUBSTRING(REFNO,1,3) + '-' + SUBSTRING(REFNO,4,10) + ' :' + LTRIM(SUM(AMOUNT)) ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'CA' THEN 'CASH ' + ' :' + LTRIM(SUM(AMOUNT)) ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'PU' THEN 'PURCHASE ' + ' :' + LTRIM(SUM(AMOUNT)) ELSE"
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'SR' THEN 'SALES RETURN ' + ' :' + LTRIM(SUM(AMOUNT)) ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'AP' AND SUBSTRING(REFNO,1,1) = 'A' THEN 'ADVANCE REPAY ' + ' :' + LTRIM(SUM(AMOUNT)) ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'AP' AND SUBSTRING(REFNO,1,1) = 'O' THEN 'ORDER REPAY ' + ' :' + LTRIM(SUM(AMOUNT)) ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'RV' THEN 'RETURN ' + ' :' + LTRIM(SUM(AMOUNT)) ELSE "
        strsql += vbCrLf + " 'CHEQUE ' + CHQCARDREF + ' ' + CHQCARDNO + ' :' + LTRIM(SUM(AMOUNT))" 'CH
        strsql += vbCrLf + " END END END END END END END END AS AMT "
        strsql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN T"
        strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CREDITCARD AC ON AC.CARDCODE = T.CARDID "
        strsql += vbCrLf + " WHERE "
        strsql += vbCrLf + " TRANDATE = '" & _Trandate & "' " 'Newly Add
        strsql += vbCrLf + " AND BATCHNO = '" & pBatchno & "' "
        strsql += vbCrLf + " AND PAYMODE IN ('AA','CC','CH','CA','PU','SR','RV','AP')"
        strsql += vbCrLf + " GROUP BY PAYMODE,CHQCARDNO,BATCHNO,AC.SHORTNAME,CHQCARDREF,REFNO,SYSTEMID"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
        da = New OleDbDataAdapter(strsql, cn)
        Dim dt As New DataTable
        da.Fill(dt)
        Dim TempAcc As String = ""
        Dim TempBatch As String = ""
        If dt.Rows.Count > 0 Then
            For i As Integer = 0 To dt.Rows.Count - 1
                TempAcc += "," & dt.Rows(i).Item("AMT").ToString
            Next
            TempBatch = ""
            TempAcc = TempAcc.Trim(",")
            strsql = "INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ACCTRANADVANCERECEIPT](ACCODE,BATCHNO_N) VALUES "
            strsql += " ('" & TempAcc & "', '" & TempBatch & "')"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        End If
        strsql = " SELECT * INTO TEMPTABLEDB..[TEMP" & SystemName & "ADVANCERECEIPT] FROM( "
        strsql += vbCrLf + " SELECT NULL AS PAYMODE,"
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'AR' THEN 'Receipt No ' ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'RE' THEN 'Due No ' ELSE CASE WHEN PAYMODE = 'MP' THEN 'Payment No ' ELSE 'Voucher No ' END END"
        strsql += vbCrLf + " END AS TranName,"
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'AR' THEN 'ADVANCE  RECEIPT ' ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'RE' THEN 'DUE  RECEIPT ' ELSE CASE WHEN PAYMODE = 'MP' THEN 'OTHER PAYMENT' ELSE 'RECEIPT  VOUCHER' END END"
        strsql += vbCrLf + " END AS Field1,"
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'AR' THEN 'Received with thanks from ' ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'RE' THEN 'Credit bill ' ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'DR' THEN  'Received with thanks from ' ELSE CASE WHEN PAYMODE = 'MP' THEN 'Payment Bill ' ELSE 'Receipt Bill ' END   "
        strsql += vbCrLf + " END END END + "
        strsql += vbCrLf + " (SELECT (SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = C.PSNO )"
        strsql += vbCrLf + " FROM " & cnAdminDb & "..CUSTOMERINFO AS C "
        strsql += vbCrLf + " WHERE BATCHNO = o.BATCHNO) + ' Rs.' + CONVERT(VARCHAR,AMOUNT+ISNULL(GSTVAL,0)) Field2 " ' Newly Add ISNULL(GSTVAL,0)
        strsql += vbCrLf + " ,(AMOUNT+ISNULL(GSTVAL,0))AMOUNT " 'NEWLY ADD
        strsql += vbCrLf + " , NULL AMT "
        'strSql += vbCrLf + ", 'Refer No :' + CONVERT(VARCHAR, SUBSTRING(O.RUNNO,6,20)) as  Field3"
        strsql += vbCrLf + " , CASE WHEN PAYMODE = 'AR' THEN 'Towards ADVANCE' ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'RE' THEN 'Towards CREDIT' ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'MP' THEN 'Towards PAYMENT' ELSE 'Towards RECEIPT' "
        strsql += vbCrLf + " END END END + ' Refer No :' + CONVERT(VARCHAR, SUBSTRING(O.RUNNO,6,20)) AS Field5 ,"
        strsql += vbCrLf + " BATCHNO AS BATCHNO_N, TRANNO,"
        strsql += vbCrLf + " CONVERT(VARCHAR(15),TRANDATE,103) + SUBSTRING(CONVERT(VARCHAR,UPTIME,109),13,5)+SUBSTRING(CONVERT(VARCHAR,UPTIME,109),25,5) AS BILLDATE, "
        strsql += vbCrLf + " 'C' AS TRANMODE,0 AS ISTAG,0 AS ISOG"
        strsql += vbCrLf + " ,SYSTEMID AS NODE"
        strsql += vbCrLf + " ,ISNULL ((SELECT "
        strsql += vbCrLf + " (SELECT TOP 1 SHORTNAME FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID =C.PURITYID) "
        strsql += vbCrLf + " FROM " & cnAdminDb & "..CATEGORY AS C WHERE CATCODE = O.CATCODE), '') CATCODETYPE"
        strsql += vbCrLf + " ,LTRIM(REMARK1)+LTRIM(REMARK2) AS REMARKS "
        strsql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS O "
        strsql += vbCrLf + " WHERE TRANDATE = '" & _Trandate & "' " 'Newly Add
        strsql += vbCrLf + " AND  BATCHNO = '" & pBatchno & "'"
        strsql += vbCrLf + " )X"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = vbCrLf + " SELECT  "
        strsql += vbCrLf + " (SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS PNAME,  "
        strsql += vbCrLf + " (SELECT (DOORNO + ' ' + ADDRESS1) FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS ADDRESS1,"
        strsql += vbCrLf + " (SELECT (ADDRESS2 + ' ' + ADDRESS3 + ' ' + CITY + ' ' + PINCODE ) FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS ADDRESS2,"
        strsql += vbCrLf + " (SELECT CASE WHEN MOBILE<> '' THEN MOBILE ELSE PHONERES END FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS MOBILENO,"
        strsql += vbCrLf + " (SELECT PAN  FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS TIN,"
        strsql += vbCrLf + " BATCHNO AS BATCHNO_N,"
        strsql += vbCrLf + " (SELECT ISNULL(GSTNO,'')GSTNO  FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS GSTNO, "
        strsql += vbCrLf + " ISNULL(P.PAN,'') AS PANNO "
        strsql += vbCrLf + " INTO TEMPTABLEDB..[TEMP" & SystemName & "CUSTADVANCERECEIPT] "
        strsql += vbCrLf + " FROM " & cnAdminDb & "..CUSTOMERINFO AS P "
        strsql += vbCrLf + " WHERE BATCHNO = '" & pBatchno & "'"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = "SELECT DISTINCT BATCHNO FROM " & cnAdminDb & "..ITEMTAG T WHERE BATCHNO='" & pBatchno & "'  " ' DISTINCT NOT USED ORDER BY ITEMID 
        da = New OleDbDataAdapter(strsql, cn)
        Dim DtAdvRecor As New DataTable
        da.Fill(DtAdvRecor)
        If DtAdvRecor.Rows.Count > 0 Then            '
            strsql = "SELECT ITEMID,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)ITEMNAME"
            strsql += vbCrLf + " , GRSWT AS WEIGHT"
            strsql += vbCrLf + " , (SELECT TOP 1 RATE FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO=T.BATCHNO) AS RATE"
            strsql += vbCrLf + " , (SELECT TOP 1 CASE WHEN RATE = 0 THEN 'RATE NOT FIXED' ELSE 'RATE FIXED' END FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO=T.BATCHNO) AS RATENAME"
            strsql += vbCrLf + "  INTO TEMPTABLEDB..[TEMP" & SystemName & "ITEMTAG] "
            strsql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T WHERE BATCHNO='" & pBatchno & "' ORDER BY ITEMID"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        Else
            strsql = "CREATE TABLE TEMPTABLEDB..[TEMP" & SystemName & "ITEMTAG](ITEMID INT, ITEMNAME VARCHAR(50), WEIGHT INT, RATE VARCHAR(15), RATENAME VARCHAR(50)) "
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
            strsql = " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ITEMTAG] (ITEMID,ITEMNAME,WEIGHT,RATE,RATENAME) "
            strsql += vbCrLf + " SELECT 0 ,NULL,0 "
            strsql += vbCrLf + " ,(SELECT TOP 1 RATE FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO= '" & pBatchno & "' ) AS RATE"
            strsql += vbCrLf + " ,(SELECT TOP 1 CASE WHEN RATE = 0 THEN 'RATE NOT FIXED' ELSE 'RATE FIXED' END FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO= '" & pBatchno & "' ) AS RATENAME"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        End If
        strsql = vbCrLf + "  SELECT "
        strsql += vbCrLf + "  NULL AS SRNO,"
        strsql += vbCrLf + "  NULL AS TRANNO,"
        strsql += vbCrLf + "  CONVERT(VARCHAR(15),I.TRANDATE,103) AS BILLDATE,"
        strsql += vbCrLf + "  BATCHNO AS BATCHNO_N,"
        strsql += vbCrLf + "  'TOTAL' ITEMNAME"
        strsql += vbCrLf + "  ,SUM(I.PCS) AS QTY,SUM(I.GRSWT)GRSWT,SUM(I.NETWT)NETWT,SUM(I.LESSWT) LESSWT,SUM(I.WASTAGE) WASTAGE"
        strsql += vbCrLf + "  ,CONVERT(VARCHAR,NULL)RATE, CONVERT(VARCHAR, SUM(I.MCHARGE)) MCHARGE,0 TAX,SUM(I.AMOUNT + I.TAX) AMOUNT "
        strsql += vbCrLf + "  ,'ZZZZZZZZ' SNO,3 AS RESULT,2 AS TYPE,'G' AS COLHEAD"
        strsql += vbCrLf + "  INTO TEMPTABLEDB..[TEMP" & SystemName & "ADVANCEPURRET]"
        strsql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS I "
        strsql += vbCrLf + "  WHERE I.TRANDATE = '" & _Trandate & "' "
        strsql += vbCrLf + "  AND I.BATCHNO='" & pBatchno & "' "
        strsql += vbCrLf + "  AND I.TRANTYPE IN ('PU','SR')"
        strsql += vbCrLf + "  GROUP BY I.TRANDATE,I.BATCHNO  " ' ,I.TRANNO
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = "SELECT COUNT(*)CNT FROM TEMPTABLEDB..[TEMP" & SystemName & "ITEMTAG] "
        If Val(BrighttechPack.GetSqlValue(cn, strsql, "CNT", 0)) > 0 Then
            strsql = " UPDATE TEMPTABLEDB..[TEMP" & SystemName & "ADVANCERECEIPT] SET ISTAG=1"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        End If
        strsql = " SELECT COUNT(*)CNT FROM " & cnStockDb & "..RECEIPT "
        strsql += vbCrLf + " WHERE TRANDATE = '" & _Trandate & "' "
        strsql += vbCrLf + " AND  BATCHNO='" & pBatchno & "' "
        strsql += vbCrLf + " AND TRANTYPE IN ('PU','SR')"
        If Val(BrighttechPack.GetSqlValue(cn, strsql, "CNT", 0)) > 0 Then
            strsql = " UPDATE TEMPTABLEDB..[TEMP" & SystemName & "ADVANCERECEIPT] SET ISOG=1"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        End If

        strsql = vbCrLf + " SELECT TRANMODE, SUM(CGST) CGST,SUM(SGST)SGST, SUM(IGST)IGST FROM "
        strsql += vbCrLf + " ("
        strsql += vbCrLf + " SELECT TRANMODE,AMOUNT AS CGST, 0 AS SGST, 0 AS IGST "
        strsql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN WHERE "
        strsql += vbCrLf + " TRANDATE = '" & _Trandate & "' "
        strsql += vbCrLf + " AND BATCHNO = '" & pBatchno & "' "
        strsql += vbCrLf + " AND PAYMODE IN ('SV') AND ACCODE = 'CGST' "
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT TRANMODE,0 CGST,AMOUNT AS SGST, 0 AS IGST"
        strsql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN WHERE "
        strsql += vbCrLf + " TRANDATE = '" & _Trandate & "' "
        strsql += vbCrLf + " AND BATCHNO = '" & pBatchno & "' "
        strsql += vbCrLf + " AND PAYMODE IN ('SV') AND ACCODE = 'SGST'"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT 'IGST' TRANMODE,0 CGST, 0 AS SGST, AMOUNT AS IGST "
        strsql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN WHERE "
        strsql += vbCrLf + " TRANDATE = '" & _Trandate & "' "
        strsql += vbCrLf + " AND BATCHNO = '" & pBatchno & "' "
        strsql += vbCrLf + " AND PAYMODE IN ('SV') AND ACCODE = 'IGST' "
        strsql += vbCrLf + " )X"
        strsql += vbCrLf + " GROUP BY TRANMODE "
        da = New OleDbDataAdapter(strsql, cn)
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
            TopY = TopY + 20
        End If
    End Sub
    Private Sub printAdvance_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles printAdvance.PrintPage
        Try
            Dim NoofPage As Integer = 0
            Dim gstPrint As Boolean = False
            Dim rAlign As New StringFormat
            rAlign.Alignment = StringAlignment.Far
            NoofPrint += 1
            Using g As Graphics = e.Graphics
                CustomerInfoAdvanceReceipt(e.Graphics, e)
                If dsAdvanceReceipt.Tables("ITEMTAG").Rows.Count > 0 Then
                    If Val(dsAdvanceReceipt.Tables("ITEMTAG").Rows(0).Item("itemid").ToString) <> 0 Then
                        TitleAdvanceReceipt(e.Graphics, e)
                    End If

                    For PagecountSale = PagecountSale To dsAdvanceReceipt.Tables("ITEMTAG").Rows.Count - 1
                        NoofPage += 1
                        With dsAdvanceReceipt.Tables("ITEMTAG").Rows(PagecountSale)
                            g.DrawString(IIf(Val(.Item("ITEMID").ToString()) > 0, .Item("ITEMID").ToString(), ""), fontRegular, BlackBrush, c1, TopY, LeftFormat)
                            g.DrawString(.Item("ITEMNAME").ToString(), fontRegular, BlackBrush, c2, TopY, LeftFormat)
                            g.DrawString(IIf(Val(.Item("WEIGHT").ToString()) > 0, .Item("WEIGHT").ToString(), ""), fontRegular, BlackBrush, c6, TopY, rAlign)
                            TopY = TopY + 20

                            If NoofPage > 10 Then
                                PagecountSale = PagecountSale + 1
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c1, TopY)
                                TopY = 150 ' TOP AGAIN STARTING POSITION
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
                        g.DrawString("SGST : " & dsAdvanceReceipt.Tables("GSTTRAN").Rows(0).Item("SGST").ToString(), fontRegular, BlackBrush, c3, TopY, LeftFormat)
                        g.DrawString("IGST : " & dsAdvanceReceipt.Tables("GSTTRAN").Rows(0).Item("IGST").ToString(), fontRegular, BlackBrush, c4, TopY, LeftFormat)
                    End If
                End If
                'g.DrawLine(Pens.Silver, 20.0F, 1060.0F, 775.0F, 1060.0F)
                TopY = TopY + 15
                g.DrawLine(Pens.Silver, 20.0F, TopY, 775.0F, TopY)
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            Exit Sub
        End Try
    End Sub
#End Region

End Class