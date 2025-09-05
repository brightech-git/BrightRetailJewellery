Imports System.Data.OleDb
Imports System.IO

Public Class frmBillPrintDocB5

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


    Dim fontRegular6 As New Font("Palatino Linotype", 8.25, FontStyle.Regular)
    Dim fontRegular6small As New Font("Palatino Linotype", 6.25, FontStyle.Regular)
    Dim fontRegular7 As New Font("Palatino Linotype", 8.25, FontStyle.Regular)
    Dim fontRegular8 As New Font("Palatino Linotype", 8.25, FontStyle.Regular)
    Dim fontRegular9 As New Font("Palatino Linotype", 8.25, FontStyle.Regular)

    Dim fontBold9 As New Font("Palatino Linotype", 8.25, FontStyle.Bold)
    Dim fontBold8 As New Font("Palatino Linotype", 8.25, FontStyle.Bold)
    Dim fontBold7 As New Font("Palatino Linotype", 8.25, FontStyle.Bold)
    Dim fontBold6 As New Font("Palatino Linotype", 8.25, FontStyle.Bold)

    Dim fontUnderLine9 As New Font("Palatino Linotype", 9, FontStyle.Underline)

    'Dim fontBoldUnderLine As New Font("Palatino Linotype", 9, FontStyle.Bold And FontStyle.Underline)
    Dim fontBoldTitle7 As New Font("Palatino Linotype", 8.25, FontStyle.Bold)
    Dim EstNo_SA As String = ""
    Dim EstNo_PU As String = ""

    Dim c1 As Integer = 120  ' SNo  30
    Dim c2 As Integer = 140  ' Description 40
    Dim h1 As Integer = 300  ' hsn 180
    Dim c3 As Integer = 380 ' Qty  260
    Dim c4 As Integer = 420 ' Grs. Wt 300
    Dim c5 As Integer = 465 ' Less. Wt 345
    Dim c6 As Integer = 500 ' VA 370
    Dim c7 As Integer = 550 ' Rate 420
    Dim c8 As Integer = 600 ' MC 460
    Dim c9 As Integer = 670 ' ' Amount 630
    Dim PagecountSale As Integer = 0
    Dim TopyStartingPostition As Integer = 135
    Dim TopY As Integer = TopyStartingPostition ' TOP STARTING POSITION
    Dim TOPYHorizontal As Integer = 0

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
                        PrtDoc.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = Printing.PaperKind.A5
                        PrtDoc.PrinterSettings.Copies = 1
                        PrtDoc.PrintController = New System.Drawing.Printing.StandardPrintController
                        If Val(objprinterlist.txtNoofCopies.Text) > 1 Then
                            For cnt As Integer = 0 To Val(objprinterlist.txtNoofCopies.Text) - 1
                                PagecountSale = 0
                                TopyStartingPostition = 135
                                TopY = TopyStartingPostition ' TOP STARTING POSITION
                                TOPYHorizontal = 0
                                PrtDoc.Print()
                            Next
                        Else
                            PrtDoc.Print()
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
            ' strSql += vbCrLf + " UNION SELECT 1 FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO=O.BATCHNO AND TRANTYPE='SR' )"
            strsql += vbCrLf + " )"
            If Val(objGPack.GetSqlValue(strsql, "CNT", 0)) > 0 Then  ''OG WITH ADVANCE
                'printCustoInfo = True
                'AdvancePrint(pBatchno, strBilldate)
                'PrtDiaAdvance.Document = PrintAdvance
                'PrintAdvance.PrinterSettings.Copies = 1
                'PrintAdvance.PrintController = New System.Drawing.Printing.StandardPrintController()
                'PrintAdvance.Print()
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

        Select Case Trantype
            Case "REP"
                g.DrawString("NOTE : ITEM(S) MENTIONED ABOVE WILL BE DELIVERED ONLY BY PRODUCING ORIGINAL VOUCHER", fontBold6, Brushes.Black, c2, TopY)
            Case "ORD"
                g.DrawString("NOTE : ORIGINAL ORDER BOOKING RECEIPT SHOULD BE PRODUCED AT THE TIME OF ITEM(S) DELIVERY", fontBold6, Brushes.Black, c2, TopY)
            Case Else
                TopY = TopY + 10
        End Select
        g.DrawString("E&O.E      Tax is Payable on Reverse Charge : NO ", fontRegular6, Brushes.Black, c1, TopY)

        ''g.DrawString(NodeId, fontRegular6, Brushes.Black, c3, TopY)
        ''TopY = TopY + 10
        ''If placeofSupply <> "" Then
        ''    g.DrawString("PLACE OF SUPPLY : " & placeofSupply, fontRegular6, Brushes.Black, h1, TopY)
        ''End If
        ''g.DrawString("Cashier", fontRegular6, Brushes.Black, c7, TopY)

        ''If ImgPrint.Length > 0 Then
        ''    g.DrawString("For " & strCompanyName & "", New Font("Times New Roman", 8, FontStyle.Regular), Brushes.Black, c6, 1083)
        ''    Dim Image1 As Image = Image.FromStream(ImgPrint)
        ''    e.Graphics.DrawImage(Image1, 600, 1093, 100, 70)
        ''End If
        'g.DrawString(Rules(0).ToString, fontRegular6, Brushes.Black, c1, 540)
        'g.DrawString(Rules(1).ToString, fontRegular6, Brushes.Black, c1, 540)
        'If NoofPrint > 1 Then
        '    g.DrawString(T1, fontRegular6, Brushes.Black, c8, 1063)
        'End If
    End Sub
    Public Sub Title(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        TOPYHorizontal = TopY
        RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
        g.DrawString("#", fontBold6, Brushes.Black, c1, TopY, LeftFormat)
        g.DrawString("DESCRIPTION", fontBold6, Brushes.Black, c2, TopY, LeftFormat)
        g.DrawString("HSN", fontBold6, Brushes.Black, h1, TopY, LeftFormat)
        g.DrawString("QTY", fontBold6, Brushes.Black, c3, TopY, RightFormat)
        g.DrawString("G.WT", fontBold6, Brushes.Black, c4, TopY, RightFormat)
        g.DrawString("L.WT", fontBold6, Brushes.Black, c5, TopY, RightFormat)
        g.DrawString("VA", fontBold6, Brushes.Black, c6, TopY, RightFormat)
        g.DrawString("RATE", fontBold6, Brushes.Black, c7, TopY, RightFormat)
        g.DrawString("MC", fontBold6, Brushes.Black, c8, TopY, RightFormat) '"VA"
        g.DrawString("AMT", fontBold6, Brushes.Black, c9, TopY, RightFormat)
        TopY = TopY + 15
        DrawLine(g, TopY, 0)
        TopY = TopY + 10
    End Sub
    Public Sub BarcodeTempFileDelete(ByVal LocalTemp As String, ByVal SystemName As String, ByVal Tranno As String)
        If System.IO.File.Exists(LocalTemp & "\barcode" & SystemName & ".png") = True Then
            System.IO.File.Delete(LocalTemp & "\barcode" & SystemName & ".png")
        End If
    End Sub
    Public Sub DrawLine(ByVal g As Graphics, ByVal Y1 As Integer, ByVal Y2 As Integer)
        g.DrawLine(Pens.Silver, c1, Y1, c9, Y1)
        g.DrawLine(Pens.Silver, c1, Y2, c9, Y2)
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
        Dim CusAddress2 As String = ""
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
            g.DrawString(Cusname, fontRegular6, Brushes.Black, c1, TopY)
            Dim c7I As Integer = c7 - 30
            Dim c8D As Integer = c8 - 18
            Dim c8D2 As Integer = c8D + 7
            Select Case Trantype
                Case "POS"
                    g.DrawString("Invoice No", fontRegular6, Brushes.Black, c7I, TopY)
                    g.DrawString(" : ", fontRegular6, Brushes.Black, c8D, TopY)
                    g.DrawString(BBill & "" & Bno, fontRegular6, Brushes.Black, c8D2, TopY)
                Case "ORD"
                    g.DrawString("Order No", fontRegular6, Brushes.Black, c7I, TopY)
                    g.DrawString(" : ", fontRegular6, Brushes.Black, c8D, TopY)
                    g.DrawString(BBill & "" & Bno, fontRegular6, Brushes.Black, c8D2, TopY)
                Case "REP"
                    g.DrawString("Repair No", fontRegular6, Brushes.Black, c7I, TopY)
                    g.DrawString(" : ", fontRegular6, Brushes.Black, c8D, TopY)
                    g.DrawString(BBill & "" & Bno, fontRegular6, Brushes.Black, c8D2, TopY)
            End Select
            TopY = TopY + 12
            CusAddress1 = Trim(dtCustInfo.Rows(0).Item("DoorNo") & "").ToString & " " & Trim(dtCustInfo.Rows(0).Item("address1") & "").ToString
            g.DrawString(CusAddress1, fontRegular6, Brushes.Black, c1, TopY)
            g.DrawString("Date", fontRegular6, Brushes.Black, c7I, TopY)
            g.DrawString(" : ", fontRegular6, Brushes.Black, c8D, TopY)
            g.DrawString(Bdate, fontRegular6, Brushes.Black, c8D2, TopY)
            TopY = TopY + 12
            CusAddress2 = Trim(dtCustInfo.Rows(0).Item("address2") & "").ToString & " " & Trim(dtCustInfo.Rows(0).Item("area") & "").ToString
            g.DrawString(CusAddress2, fontRegular6, Brushes.Black, c1, TopY)
            Select Case Trantype
                Case "ORD"
                    TopY = TopY + 12
                    g.DrawString("Due", fontRegular6, Brushes.Black, c7I, TopY)
                    g.DrawString(" : ", fontRegular6, Brushes.Black, c8D, TopY)
                    g.DrawString(DueDate, fontRegular6, Brushes.Black, c8D2, TopY)
                    TopY = TopY - 12
            End Select
            TopY = TopY + 12
            CusArea = Trim(dtCustInfo.Rows(0).Item("City") & "").ToString & " " & Trim(dtCustInfo.Rows(0).Item("PinCode") & "").ToString
            g.DrawString(CusArea, fontRegular6, Brushes.Black, c1, TopY)
            Dim header1 As Integer = c3
            Select Case Trantype
                Case "POS"
                    If TrantypeMI = "RD" Then
                        g.DrawString("REPAIR DELIVERY", fontBoldTitle7, Brushes.Black, header1, TopY)
                    ElseIf TrantypeMI = "MI" Then
                        g.DrawString("MISC ISSUE", fontBoldTitle7, Brushes.Black, header1, TopY)
                    Else
                        g.DrawString("TAX INVOICE", fontBoldTitle7, Brushes.Black, header1, TopY) 'SALES
                    End If
                Case "ORD"
                    g.DrawString("ORDER BOOKING", fontBoldTitle7, Brushes.Black, header1, TopY)
                Case "REP"
                    g.DrawString("REPAIR BOOKING", fontBoldTitle7, Brushes.Black, header1, TopY)
            End Select
            TopY = TopY + 12
            If Trim(dtCustInfo.Rows(0).Item("phoneres") & "").ToString <> "" Then
                CusPhone_Mobi = Trim(dtCustInfo.Rows(0).Item("phoneres") & "").ToString & " " & Trim(dtCustInfo.Rows(0).Item("mobile") & "").ToString & " " & Trim(IIf(dtCustInfo.Rows(0).Item("PAN").ToString <> "", "PAN :" & dtCustInfo.Rows(0).Item("PAN").ToString, "")).ToString
                g.DrawString(CusPhone_Mobi, fontRegular6, Brushes.Black, c1, TopY)
                TopY = TopY + 12
            Else
                CusPhone = Trim(dtCustInfo.Rows(0).Item("mobile").ToString) & " " & Trim(IIf(dtCustInfo.Rows(0).Item("PAN").ToString <> "", "PAN :" & dtCustInfo.Rows(0).Item("PAN").ToString, "")).ToString
                g.DrawString(CusPhone, fontRegular6, Brushes.Black, c1, TopY)
                TopY = TopY + 12
            End If
            If Trim(dtCustInfo.Rows(0).Item("GSTNO") & "").ToString <> "" Then
                CusGSTNo = dtCustInfo.Rows(0).Item("GSTNO").ToString
                g.DrawString("GSTIN : " & CusGSTNo, fontRegular6, Brushes.Black, c1, TopY)
                If dtInvTran.Rows.Count > 0 Then
                    Dim cusIRNNo As String = dtInvTran.Rows(0).Item("IRN").ToString
                    g.DrawString("IRN : " & cusIRNNo, fontRegular6, Brushes.Black, h1 - 95, TopY)
                End If
                TopY = TopY + 12
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
        TopY = TopY + 10
        DrawLine(g, TopY, 0)
    End Sub
    Private Sub PrtDoc_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrtDoc.PrintPage
        Try
            Dim NoofPage As Integer = 0
            Dim rAlign As New StringFormat
            rAlign.Alignment = StringAlignment.Far
            NoofPrint += 1
            Using g As Graphics = e.Graphics
                g.DrawString("GSTIN : " & GSTNO, fontRegular6, Brushes.Black, h1, TopY - 25) '175
                g.DrawString("STATE CODE : " & Mid(GSTNO, 1, 2) & "", fontRegular6, Brushes.Black, h1, TopY - 15) '195
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
                            g.DrawString(.Item("SRNO").ToString(), fontRegular6, BlackBrush, c1, TopY, LeftFormat)
                            If .Item("TYPE").ToString = "10" And .Item("COLHEAD").ToString = "G" Then
                                If .Item("ITEMNAME").ToString.Length > 25 Then
                                    g.DrawString(.Item("ITEMNAME").ToString(), fontBold6, BlackBrush, h1, TopY, LeftFormat)
                                Else
                                    g.DrawString(.Item("ITEMNAME").ToString(), fontBold6, BlackBrush, h1, TopY, LeftFormat)
                                End If
                            Else
                                If .Item("CALTYPE").ToString = "HA" And .Item("RESULT").ToString = "1.1" Then
                                    g.DrawString(.Item("ITEMNAME").ToString(), fontRegular6, BlackBrush, c2, TopY, LeftFormat)
                                Else
                                    g.DrawString(.Item("ITEMNAME").ToString(), fontRegular6, BlackBrush, c2, TopY, LeftFormat)
                                End If
                            End If
                            g.DrawString(.Item("HSN").ToString(), fontRegular6, BlackBrush, h1, TopY, LeftFormat)
                            g.DrawString(IIf(Val(.Item("QTY").ToString()) <> 0, Val(.Item("QTY").ToString()), ""), fontRegular6, BlackBrush, c3, TopY, RightFormat)
                            If .Item("CALTYPE").ToString = "F" Then 'Fixed Item Should Not print GrossWt
                                g.DrawString("", fontRegular6, BlackBrush, c4, TopY, RightFormat)
                                g.DrawString("", fontRegular6, BlackBrush, c5, TopY, RightFormat)
                            Else
                                g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontRegular6, BlackBrush, c4, TopY, RightFormat)
                                g.DrawString(IIf(Val(.Item("LESSWT").ToString()) <> 0, .Item("LESSWT").ToString(), ""), fontRegular6, BlackBrush, c5, TopY, RightFormat)
                            End If
                            g.DrawString(IIf(Val(.Item("WASTAGE").ToString()) <> 0, .Item("WASTAGE").ToString, ""), fontRegular6, BlackBrush, c6, TopY, RightFormat)
                            If .Item("COLHEAD").ToString() = "G" Or .Item("COLHEAD").ToString = "P" Then
                                If .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString <> "10" Then
                                    g.DrawString(.Item("RATE").ToString(), fontRegular6, BlackBrush, c4, TopY, LeftFormat)
                                ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString <> "3.50") Then
                                    g.DrawString(.Item("RATE").ToString(), fontBold6, BlackBrush, c4, TopY, LeftFormat)
                                ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString = "3.50") Then
                                    g.DrawString(.Item("RATE").ToString(), fontRegular6, BlackBrush, c4, TopY, LeftFormat)
                                ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10" Then
                                    g.DrawString(.Item("RATE").ToString(), fontBold6, BlackBrush, c4, TopY, LeftFormat)
                                End If
                            Else
                                g.DrawString(.Item("RATE").ToString(), fontRegular6, BlackBrush, c7, TopY, RightFormat)
                            End If
                            If .Item("CALTYPE").ToString = "F" Then 'Fixed Item Should Not print mC
                                g.DrawString("", fontRegular6, BlackBrush, c8, TopY, RightFormat)
                            Else
                                g.DrawString(IIf(.Item("MCHARGE").ToString() <> "0.00", .Item("MCHARGE").ToString(), ""), fontRegular6, BlackBrush, c8, TopY, RightFormat)
                            End If
                            If .Item("COLHEAD").ToString = "G" Or .Item("COLHEAD").ToString = "P" Then
                                If .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString <> "10" Then
                                    g.DrawString(.Item("AMOUNT").ToString(), fontRegular6, BlackBrush, c9, TopY, rAlign)
                                ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString <> "3.50") Or (.Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10") Then
                                    g.DrawString(.Item("AMOUNT").ToString(), fontBold6, BlackBrush, c9, TopY, rAlign)
                                    If .Item("RESULT").ToString = "3.6" And .Item("TYPE").ToString = "11" Then
                                        Dim AmtinWord As String = ConvertRupees.RupeesToWord(Val(.Item("AMOUNT").ToString().Trim("-", ""))) '.Trim("-", "")
                                        If AmtinWord.Length > 50 Then
                                            Dim amtinwords() As String = Nothing
                                            Dim prnAmtinWord0 As String = ""
                                            Dim prnAmtinWord1 As String = ""
                                            amtinwords = AmtinWord.Split(" ")
                                            If amtinwords.Length > 0 Then
                                                Dim amthalflen As Integer = 0
                                                amthalflen = Math.Round(Val(amtinwords.Length) / 2, 0)
                                                For cntt As Integer = 0 To amtinwords.Length - 1
                                                    If cntt < amthalflen Then
                                                        prnAmtinWord0 = prnAmtinWord0.ToString & " " & amtinwords(cntt).ToString
                                                    Else
                                                        prnAmtinWord1 = prnAmtinWord1.ToString & " " & amtinwords(cntt).ToString
                                                    End If
                                                Next
                                            End If
                                            g.DrawString(prnAmtinWord0.ToString, fontRegular6small, BlackBrush, c1, TopY)
                                            g.DrawString(prnAmtinWord1.ToString, fontRegular6small, BlackBrush, c1, TopY + 8)
                                        Else
                                            g.DrawString(AmtinWord, fontRegular6small, BlackBrush, c1, TopY)
                                        End If

                                    End If
                                ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString = "3.50") Then
                                    g.DrawString(.Item("AMOUNT").ToString(), fontRegular6, BlackBrush, c9, TopY, rAlign)
                                ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10" Then
                                    g.DrawString(.Item("AMOUNT").ToString(), fontBold6, BlackBrush, c9, TopY, rAlign)
                                End If
                            Else
                                g.DrawString(.Item("AMOUNT").ToString(), fontRegular6, BlackBrush, c9, TopY, rAlign)
                            End If
                            If (.Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" And Val(.Item("QTY").ToString) >= 2) _
                            Or (.Item("TYPE").ToString = "2" And .Item("COLHEAD").ToString = "G") _
                            Or (.Item("TYPE").ToString = "3" And .Item("COLHEAD").ToString = "G") Then
                                g.DrawLine(Pens.Silver, 340, (TopY + 2), c9, (TopY + 2))
                            End If
                            If TopY >= 489 Then
                                PagecountSale = PagecountSale + 1
                                TopY = TopyStartingPostition
                                g.DrawString("Continue.... ", fontRegular6, Brushes.Black, c8, 513)
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
                                g.DrawString("PCS COST ITEM NO EXCHANGE", fontRegular6, BlackBrush, c2, TopY, LeftFormat)
                                TopY = TopY + 17
                            End If
                            If .Item("RESULT").ToString = "1.0" And (.Item("BILLNAME").ToString = "Y") Then '.Item("COLHEAD").ToString <> "G" And .Item("COLHEAD").ToString <> "P" And .Item("COLHEAD").ToString <> "T"
                                g.DrawString("Fixing of Kemp, Kundan Stones needs wax, Hence at the time of melting weight loss will be there upto 10%..", fontRegular6, BlackBrush, c2, TopY, LeftFormat)
                                TopY = TopY + 17
                                'End If
                            End If
                            If TopY >= 489 Then
                                PagecountSale = PagecountSale + 1
                                TopY = TopyStartingPostition ' TOP AGAIN STARTING POSITION
                                g.DrawString("Continue.... ", fontRegular6, Brushes.Black, c8, 513)
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
                                TopY = TopyStartingPostition ' TOP AGAIN STARTING POSITION
                                g.DrawString("Continue.... ", fontRegular6, Brushes.Black, c8, 1075)
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
    Private Sub PrtSEst_PrintPage_old(sender As Object, e As Printing.PrintPageEventArgs)
        Using g As Graphics = e.Graphics
            Dim dtBillPrint As New DataTable
            Dim brush As New SolidBrush(Color.Black)
            Dim pen As New Pen(brush)
            Dim fontRegular As New Font("Times New Roman", 9, FontStyle.Regular)
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
            Dim strTranno As String = ""
            Dim sa_totalamt As Double = 0
            Dim Pu_totalamt As Double = 0
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
                strsql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
                dtBillPrint = New DataTable
                cmd = New OleDbCommand(strsql, cn)
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtBillPrint)
                If dtBillPrint.Rows.Count = 0 Then Exit Sub
                _strDesc = ""
                y1 = y1 + 20
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
                For k As Integer = 0 To dtBillPrint.Rows.Count - 1

                    g.DrawString("TAGNO", fontRegular, brush, c1, y1)
                    g.DrawString(":", fontRegular, brush, c2, y1)
                    g.DrawString(dtBillPrint.Rows(k)("ITEMID").ToString + IIf(dtBillPrint.Rows(k)("TAGNO").ToString <> "", "-" + dtBillPrint.Rows(k)("TAGNO").ToString, ""), fontRegular, brush, c3, y1)
                    y1 = y1 + 20
                    strsql = " SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST "
                    strsql += vbCrLf + " WHERE ITEMID='" & dtBillPrint.Rows(k)("ITEMID").ToString & "' "
                    strsql += vbCrLf + " AND SUBITEMID='" & dtBillPrint.Rows(k)("SUBITEMID").ToString & "' "
                    _strDesc = objGPack.GetSqlValue(strsql).ToString
                    If _strDesc = "" Then
                        strsql = " SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
                        strsql += vbCrLf + " WHERE ITEMID='" & dtBillPrint.Rows(k)("ITEMID").ToString & "' "
                        _strDesc = objGPack.GetSqlValue(strsql)
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
                        If Val(dtBillPrint.Rows(k)("GRSWT").ToString) <> Val(dtBillPrint.Rows(k)("TAGGRSWT").ToString) And Val(dtBillPrint.Rows(k)("TAGGRSWT").ToString) > 0 Then
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
                        If Val(dtBillPrint.Rows(k)("GRSWT").ToString) <> Val(dtBillPrint.Rows(k)("TAGGRSWT").ToString) And Val(dtBillPrint.Rows(k)("TAGGRSWT").ToString) > 0 Then
                            g.DrawString(Format(Val(dtBillPrint.Rows(k)("TAGGRSWT").ToString), "0.000"), fontRegular, brush, c5, y1, rAlign)
                        End If
                        y1 = y1 + 20
                    End If
                    If Val(dtBillPrint.Rows(k)("WASTAGE").ToString) <> 0 Then
                        ''g.DrawString("VA (" +
                        ''IIf(Val(dtBillPrint.Rows(k)("WASTPER").ToString) <> 0, dtBillPrint.Rows(k)("WASTPER").ToString + "%", "") + ")", fontRegular, brush, c1, y1)
                        g.DrawString("VA ", fontRegular, brush, c1, y1)
                        g.DrawString(":", fontRegular, brush, c2, y1)
                        g.DrawString(Format(Val(dtBillPrint.Rows(k)("WASTAGE").ToString), "0.000"), fontRegular, brush, c4, y1, rAlign)
                        y1 = y1 + 20
                    End If
                    If Val(dtBillPrint.Rows(k)("MCHARGE").ToString) <> 0 Then
                        g.DrawString("MC (" +
                        IIf(Val(dtBillPrint.Rows(k)("MCGRM").ToString) <> 0, dtBillPrint.Rows(k)("MCGRM").ToString + "/gm", "") + ")", fontRegular, brush, c1, y1)
                        g.DrawString(":", fontRegular, brush, c2, y1)
                        g.DrawString(Format(Val(dtBillPrint.Rows(k)("MCHARGE").ToString), "0.00"), fontRegular, brush, c4, y1, rAlign)
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
                        strsql = " SELECT TOP 1 SUBSTRING(SUBITEMNAME,1,10) SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST "
                        strsql += vbCrLf + " WHERE ITEMID='" & dtBillPrintStone.Rows(i)("STNITEMID").ToString & "' "
                        strsql += vbCrLf + " AND SUBITEMID='" & dtBillPrintStone.Rows(i)("STNSUBITEMID").ToString & "' "
                        _strDesc = objGPack.GetSqlValue(strsql).ToString
                        If _strDesc = "" Then
                            strsql = " SELECT TOP 1 SUBSTRING(ITEMNAME,1,10) ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
                            strsql += vbCrLf + " WHERE ITEMID='" & dtBillPrintStone.Rows(i)("STNITEMID").ToString & "' "
                            _strDesc = objGPack.GetSqlValue(strsql).ToString
                        End If
                        If Val(dtBillPrintStone.Rows(i)("STNAMT").ToString) <> 0 Then
                            g.DrawString(_strDesc.ToString, fontRegular, brush, c1, y1)
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
                    y1 = y1 + 20
                Next
                sa_totalamt = Format(Val(dtBillPrint.Compute("SUM(AMOUNT)", "").ToString()) + Val(dtBillPrint.Compute("SUM(TAX)", "").ToString()), "0.00")
            End If
            If Val(EstNo_PU) <> 0 Then
                y1 = y1 + 10
                g.DrawString("SALES TOTAL", fontBoldtotal, brush, c1, y1, lAlign)
                g.DrawString(sa_totalamt.ToString(), fontBoldtotal, brush, c5, y1, rAlign)
            ElseIf Val(sa_totalamt) <> 0 Then
                y1 = y1 + 10
                g.DrawString("TOTAL", fontBoldtotal, brush, c1, y1, lAlign)
                g.DrawString(sa_totalamt.ToString(), fontBoldtotal, brush, c5, y1, rAlign)
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
                y1 = y1 + 20
                g.DrawString(strTranno.ToString(), New Font("Times New Roman", 7, FontStyle.Bold), brush, 10, y1)
                y1 = y1 + 20
                g.DrawString("PURCHASE ESTIMATE", New Font("Times New Roman", 12, FontStyle.Bold), brush, 10, y1)
                y1 = y1 + 35

                strsql = " SELECT TOP 1 SUBSTRING(CONVERT(VARCHAR,UPTIME,109),13,5)+SUBSTRING(CONVERT(VARCHAR,UPTIME,109),25,5) AS BILLTIME, "
                strsql += vbCrLf + " CONVERT(VARCHAR,TRANNO) AS TRANNO,CONVERT(VARCHAR(12),TRANDATE,103) AS TRANDATE "
                strsql += vbCrLf + " FROM " & cnStockDb & "..ESTRECEIPT AS T WHERE "
                strsql += vbCrLf + " TRANDATE='" & strBilldate & "' AND TRANNO='" & EstNo_PU & "' AND TRANTYPE='PU' "
                strsql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
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
                Dim Type As String = ""
                y1 = y1 + 20
                For i As Integer = 0 To dtBillPrint.Rows.Count - 1
                    With dtBillPrint.Rows(i)
                        'If Val(.Item("DUSTWT").ToString()) <> 0 Then
                        g.DrawString(.Item("CATNAME").ToString(), fontRegular, brush, c1, y1, lAlign)
                        g.DrawString(Format(Val(.Item("RATE").ToString()), "0.00"), fontProduct, brush, c5, y1, rAlign)
                        y1 = y1 + 20
                        'End If
                        'If Val(.Item("DUSTWT").ToString()) = 0 Then g.DrawString(.Item("CATNAME").ToString(), fontRegular, brush, c1, y1, lAlign)
                        g.DrawString("GrsWt : " & Format(Val(.Item("GRSWT").ToString()), "0.000"), fontProduct, brush, c5, y1, rAlign)

                        y1 = y1 + 20
                        If Val(.Item("WASTAGE").ToString()) > 0 And Type = "NEW EXCHANGE" Then
                            Dim _strDec As String()
                            Dim WastPer As Integer = Format(Val(.Item("WASTPER").ToString()), 0)
                            _strDec = .Item("WASTPER").ToString().Split(".")
                            If _strDec.Length > 1 Then
                                g.DrawString("+" & Format(Val(.Item("WASTAGE").ToString()), "0.000"), fontRegular, brush, c5, y1, rAlign)
                                y1 = y1 + 20
                                If WastPer > 0 Then
                                    If Val(_strDec(1).ToString) > 0 Then
                                        g.DrawString("VA    : " & Format(Val(.Item("WASTPER").ToString()), "0.00").ToString + "%", fontRegular, brush, c5, y1, rAlign)
                                    Else
                                        g.DrawString("VA    : " & Format(Val(.Item("WASTPER").ToString()), "0").ToString + "%", fontRegular, brush, c5, y1, rAlign)
                                    End If
                                    'Else
                                    'g.DrawString(Format(Val(.Item("WASTPER").ToString()), "0").ToString + "%", fontRegular, brush, c3, y1, rAlign)
                                End If
                            End If

                            ''g.DrawString("OthAmt" & Format(Val(.Item("OTHERAMT").ToString()), "0.00"), fontRegular, brush, c1, y1, rAlign)
                        Else
                            g.DrawString("VA    : " & Format(Val(.Item("WASTAGE").ToString()), "0.000"), fontRegular, brush, c5, y1, rAlign)
                            y1 = y1 + 20
                            g.DrawString("DustWt : " & Format(Val(.Item("DUSTWT").ToString()), "0.000"), fontRegular, brush, c5, y1, rAlign)

                            ''If Val(.Item("OTHERAMT").ToString) > 0 Then
                            ''    y1 = y1 + 20
                            ''    g.DrawString("+" & Format(Val(.Item("OTHERAMT").ToString()), "0.00"), fontRegular, brush, c1, y1, rAlign)
                            ''End If
                        End If
                        y1 = y1 + 20
                        g.DrawString("NetWt : " & Format(Val(.Item("NETWT").ToString()), "0.000"), fontProduct, brush, c5, y1, rAlign)
                        y1 = y1 + 20
                        g.DrawString("Amt   : " & Format(Val(.Item("AMOUNT").ToString()), "0.00"), fontRegular, brush, c5, y1, rAlign)
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
                                y1 = y1 + 20
                                g.DrawString(Format(Val(dtPuStone.Rows(K).Item("STNWT").ToString()), "0.00").ToString & " ct", fontProduct, brush, c5, y1, rAlign)
                            Else
                                y1 = y1 + 20
                                g.DrawString(Format(Val(dtPuStone.Rows(K).Item("STNWT").ToString()), "0.00").ToString & " gm", fontProduct, brush, c5, y1, rAlign)
                            End If

                            g.DrawString(Format(Val(dtPuStone.Rows(K).Item("STNRATE").ToString()), "0.00"), fontRegular, brush, c5, y1, rAlign)
                            y1 = y1 + 20
                        Next
                    End With
                Next
                y1 = y1 + 20
                g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                y1 = y1 + 10
                g.DrawString("PUR TOTAL", fontBold, brush, c1, y1, lAlign)
                ''g.DrawString(dtBillPrint.Compute("SUM(GRSWT)", "").ToString(), fontBold, brush, c2, y1, rAlign)
                ''g.DrawString(dtBillPrint.Compute("SUM(DUSTWT) ", "").ToString(), fontBold, brush, c3, y1, rAlign)
                ''g.DrawString(dtBillPrint.Compute("SUM(WASTAGE)", "").ToString, fontBold, brush, c4, y1, rAlign)
                g.DrawString(dtBillPrint.Compute("SUM(AMOUNT)", "").ToString(), fontBold, brush, c5, y1, rAlign)
                Pu_totalamt = dtBillPrint.Compute("SUM(AMOUNT)", "").ToString()
                y1 = y1 + 20
                g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                y1 = y1 + 10
                g.DrawString("TOTAL", fontBoldtotal, brush, c1, y1, lAlign)
                g.DrawString(Val(sa_totalamt) - Val(Pu_totalamt), fontBoldtotal, brush, c5, y1, rAlign)
                Pu_totalamt = dtBillPrint.Compute("SUM(AMOUNT)", "").ToString()
                y1 = y1 + 20
                g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                y1 = y1 + 20
                If dtBillPrint.Rows.Count > 0 Then
                    g.DrawString("[EST - " + EstNo_PU + "] [EMPID - " + dtBillPrint.Rows(0)("EMPID").ToString + "] [USERID - " + dtBillPrint.Rows(0)("USERID").ToString + "]" _
                    , fontRegular, brush, c1, y1, lAlign)
                Else
                    g.DrawString("[EST - " + EstNo_PU + "]", fontRegular, brush, c1, y1, lAlign)
                End If
                y1 = y1 + 60
            Else
                y1 = y1 + 60
            End If
        End Using
    End Sub
#End Region

End Class