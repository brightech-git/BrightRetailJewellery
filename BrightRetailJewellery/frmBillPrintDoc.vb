Imports System.Data.OleDb
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Security.Principal
Imports System.Security.Permissions
Imports System
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports TouchlessLib
Imports System.Drawing.Imaging
Imports System.Threading
Imports Word = Microsoft.Office.Interop.Word
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Text
Imports System.Net.Mail
Imports System.Globalization
Imports System.Net
Imports System.Web
Imports System.Configuration
Imports Microsoft.Office.Interop

Public Class frmBillPrintDoc

    Dim cryRpt As New ReportDocument
    Dim pdfFile As String = "D:\ProductReport1.pdf"
    Dim ds As New dsBillPrint
    Dim FromId As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'FROMMAILSERVER'", "CTLTEXT", , ).ToString.ToLower()
    Dim MailServer As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'FROMMAILSERVER'", "CTLTEXT", , ).ToString.ToLower()
    Dim MailPassword As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'FROMMAILPASSWARD'", "CTLTEXT", , )
    Dim MailTag As String = Nothing
    Dim SmtpHostname As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-HOSTNAME'", "CTLTEXT", , ).ToString
    Dim SmtpPort As Long = Val(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-PORT'", "CTLTEXT", , ).ToString)
    Dim SmtpSSL As Boolean = IIf(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-SSL'", "CTLTEXT", , ).ToString.ToUpper() = "Y", True, False)
    Dim file As String
    Dim k As Integer = 0
    Dim linePerpage As Integer = 12
    Dim Printedline As Integer = 0

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
    Dim strBilldate As String = "" ' BillDate
    Dim prtBilldate As String = "" ' Print BillDate
    Dim DueDate As String = "" 'DueDate Order & Receipt
    Dim NoofPrint As Integer = 0

    Public SystemName As String = "" 'Environment.MachineName
    Dim LeftFormat As New StringFormat(StringAlignment.Near)
    Dim RightFormat As New StringFormat(StringAlignment.Far)
    Dim CentreFormat As New StringFormat(StringAlignment.Center)

    Public SilverBrush As SolidBrush = New SolidBrush(Color.Silver)
    Public BlackBrush As SolidBrush = New SolidBrush(Color.Black)

    Dim fontRegular As New Font("Palatino Linotype", 9, FontStyle.Regular)
    Dim fontBold As New Font("Palatino Linotype", 9, FontStyle.Bold)
    Dim fontUnderLine As New Font("Palatino Linotype", 9, FontStyle.Underline)
    Dim fontBoldUnderLine As New Font("Palatino Linotype", 9, FontStyle.Bold And FontStyle.Underline)
    Dim fontBoldTitle As New Font("Palatino Linotype", 11, FontStyle.Bold)

    Dim EstNo_SA As String = ""
    Dim EstNo_PU As String = ""

    Dim c1 As Integer = 25  ' SNo
    Dim c2 As Integer = 70  ' Description
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
    'Dim BottomY As Integer = 705 ' BOTTOM STATRING POSITION

    Dim dtCustInfo As New DataTable ' dsBillPrint.CUSTOMERINFODataTable
    Dim dtSales As New DataTable ' dsBillPrint.dtSalesDataTable
    Dim dtOrderRepair As New DataTable ' Repair & Order
    Dim dsAdvanceReceipt As New DataSet ' Advance & Receipt
    Dim dsSmithIssueReceipt As New DataSet ' Smith Issue & Receipt
    Dim EXCISENO As String = ""
    Dim defaultPic As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'")
    Dim dtCompany As New DataTable

#End Region
    Public Sub New(ByVal type As String, ByVal batchno As String, ByVal trandate As String, ByVal duplicate As String)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        pBatchno = batchno
        Trantype = type ' CHECKING HEADLING ONLY
        strsql = "SELECT ISNULL(EXCISENO,'') EXCISENO FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID = '" & cnCompanyId & "'"
        'EXCISENO = GetSqlValue(cn, strsql)
        EXCISENO = ""
        strsql = "SELECT * FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID = '" & cnCompanyId & "'"
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtCompany)
        strBilldate = trandate '
        If type = "POS" Then
            strsql = "SELECT COUNT(*)CNT FROM " & cnAdminDb & "..OUTSTANDING O WHERE BATCHNO='" & pBatchno & "'"
            strsql += vbCrLf + " AND PAYMODE IN('AR','AP','MR','DR')"
            strsql += vbCrLf + " AND NOT EXISTS(SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE BATCHNO=O.BATCHNO"
            ' strSql += vbCrLf + " UNION SELECT 1 FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO=O.BATCHNO AND TRANTYPE='SR' )"
            strsql += vbCrLf + " )"
            If Val(GetSqlValue_Bill(strsql, "CNT", 0)) > 0 Then  ''OG WITH ADVANCE
                AdvancePrint(pBatchno)
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
            OrderReceipt(pBatchno, type)
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
                'funcEstSalesPrint()
            Else
                'PrintDialog1.Document = PrintDocument1
                PrintDialog2.Document = PrintDocument1
                PrintDocument1.PrinterSettings.Copies = 1
                PrintDocument1.PrintController = New System.Drawing.Printing.StandardPrintController
                PrintDocument1.Print()
            End If
        ElseIf type = "SMI" Or type = "SMR" Then
            smithIssueReceipt(pBatchno, "")
            PrtDiaSmith.Document = PrtSmith
            PrtSmith.PrinterSettings.Copies = 1
            PrtSmith.PrintController = New System.Drawing.Printing.StandardPrintController
            PrtSmith.Print()
        End If
    End Sub
    Public Sub smithIssueReceipt(ByVal BatchNo As String, ByVal Trantype As String)
        strsql = " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE]"
        strsql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE_ACCODE]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE_ACCODE]"
        strsql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECETOTAL]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECETOTAL]"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
        strsql = vbCrLf + " SELECT ROW_NUMBER() OVER(ORDER BY TRANNO DESC) AS SNO,TRANNO AS ISSUENO , "
        strsql += vbCrLf + " CONVERT(VARCHAR(15),TRANDATE,103) AS DATE,"
        strsql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) AS CATNAME,"
        strsql += vbCrLf + " (SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = I.USERID) USERNAME,"
        strsql += vbCrLf + " CASE WHEN ALLOY > 0 THEN 'ADD : ALLOY' END  AS ALLOYNAME,"
        strsql += vbCrLf + " ISNULL(CASE WHEN ALLOY > 0 THEN  (CONVERT(NUMERIC(15,3), ALLOY)) ELSE 0 END,0) AS ALLOY,"
        strsql += vbCrLf + " CASE WHEN  WASTAGE > 0 then 'ADD : WASTAGE' END AS WASTAGENAME,"
        strsql += vbCrLf + " CASE WHEN  WASTAGE > 0 then WASTAGE ELSE 0 END AS WASTAGE, "
        strsql += vbCrLf + " CASE WHEN PCS > 0 THEN PCS END AS PCS,"
        strsql += vbCrLf + " CASE WHEN GRSWT > 0 THEN (CONVERT(NUMERIC(15,3), GRSWT)) END AS GRSWT,"
        strsql += vbCrLf + " CASE WHEN NETWT > 0 THEN NETWT END AS NETWT,"
        strsql += vbCrLf + " CASE WHEN RATE > 0 THEN RATE END AS RATE,"
        strsql += vbCrLf + " (SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT,"
        strsql += vbCrLf + " CASE WHEN MCHARGE > 0 THEN MCHARGE ELSE 0 END AS MC,"
        strsql += vbCrLf + " CASE WHEN TAX > 0 THEN TAX ELSE 0 END AS TAX,"
        strsql += vbCrLf + " AMOUNT,"
        strsql += vbCrLf + " ACCODE,REMARK1,REMARK2, 'RECEIPT VOUCHER' TRANTYPE, '1' AS RESULT"
        strsql += vbCrLf + " INTO TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE]"
        strsql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT I"
        strsql += vbCrLf + " WHERE BATCHNO = '" & BatchNo & "' "
        strsql += vbCrLf + " AND TRANTYPE IN ('RRE') " ' RECEIPT
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT ROW_NUMBER() OVER(ORDER BY TRANNO DESC) AS SNO,TRANNO AS ISSUENO , "
        strsql += vbCrLf + " CONVERT(VARCHAR(15),TRANDATE,103) AS DATE,"
        strsql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) AS CATNAME,"
        strsql += vbCrLf + " (SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = I.USERID) USERNAME,"
        strsql += vbCrLf + " CASE WHEN ALLOY > 0 THEN 'ADD : ALLOY' ELSE '' END  AS ALLOYNAME,"
        strsql += vbCrLf + " CASE WHEN ALLOY > 0 THEN (CONVERT(NUMERIC(15,3), ALLOY)) ELSE 0 END  AS ALLOY,"
        strsql += vbCrLf + " CASE WHEN  WASTAGE > 0 then 'LESS : WASTAGE' ELSE '' END AS WASTAGENAME,"
        strsql += vbCrLf + " CASE WHEN  WASTAGE > 0 then WASTAGE ELSE 0 END AS WASTAGE, "
        strsql += vbCrLf + " CASE WHEN PCS > 0 THEN PCS END AS PCS,"
        strsql += vbCrLf + " CASE WHEN GRSWT > 0 THEN (CONVERT(NUMERIC(15,3), GRSWT)) END AS GRSWT,"
        strsql += vbCrLf + " CASE WHEN NETWT > 0 THEN NETWT END AS NETWT,"
        strsql += vbCrLf + " CASE WHEN RATE > 0 THEN RATE END AS RATE,"
        strsql += vbCrLf + " (SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT,"
        strsql += vbCrLf + " CASE WHEN MCHARGE > 0 THEN MCHARGE END AS MC,"
        strsql += vbCrLf + " CASE WHEN TAX > 0 THEN TAX ELSE 0 END AS TAX,"
        strsql += vbCrLf + " AMOUNT,"
        strsql += vbCrLf + " ACCODE,REMARK1,REMARK2,(CASE WHEN TRANTYPE = 'IIS' THEN 'ISSUE VOUCHER' ELSE 'PURCHASE RETURN' END) TRANTYPE, 2 AS RESULT"
        strsql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
        strsql += vbCrLf + " WHERE BATCHNO = '" & BatchNo & "' "
        strsql += vbCrLf + " AND TRANTYPE IN('IIS','IPU') " ' ISSUE 
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT ROW_NUMBER() OVER(ORDER BY TRANNO DESC) AS SNO,TRANNO AS ISSUENO , "
        strsql += vbCrLf + " CONVERT(VARCHAR(15),TRANDATE,103) AS DATE,"
        strsql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) AS CATNAME,"
        strsql += vbCrLf + " (SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = I.USERID) USERNAME,"
        strsql += vbCrLf + " CASE WHEN ALLOY > 0 THEN 'ADD : ALLOY' END  AS ALLOYNAME,"
        strsql += vbCrLf + " ISNULL(CASE WHEN ALLOY > 0 THEN  (CONVERT(NUMERIC(15,3), ALLOY)) END,0)  AS ALLOY,"
        strsql += vbCrLf + " CASE WHEN  WASTAGE > 0 then 'ADD : WASTAGE' END AS WASTAGENAME,"
        strsql += vbCrLf + " CASE WHEN  WASTAGE > 0 then WASTAGE else 0 END AS WASTAGE, "
        strsql += vbCrLf + " CASE WHEN PCS > 0 THEN PCS END AS PCS,"
        strsql += vbCrLf + " CASE WHEN GRSWT > 0 THEN (CONVERT(NUMERIC(15,3), GRSWT)) END AS GRSWT,"
        strsql += vbCrLf + " CASE WHEN NETWT > 0 THEN NETWT END AS NETWT,"
        strsql += vbCrLf + " CASE WHEN RATE > 0 THEN RATE END AS RATE,"
        strsql += vbCrLf + " (SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT,"
        strsql += vbCrLf + " CASE WHEN MCHARGE > 0 THEN MCHARGE END AS MC,"
        strsql += vbCrLf + " CASE WHEN TAX > 0 THEN TAX END AS TAX,"
        strsql += vbCrLf + " AMOUNT,"
        strsql += vbCrLf + " ACCODE,REMARK1,REMARK2, 'PURCHASE VOUCHER' TRANTYPE, '3' AS RESULT"
        strsql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT I"
        strsql += vbCrLf + " WHERE BATCHNO = '" & BatchNo & "' "
        strsql += vbCrLf + " AND TRANTYPE IN ('RPU') " ' RPU - > PURCHASE VOUCHER
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = vbCrLf + "SELECT * INTO TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECETOTAL] FROM ( "
        strsql += vbCrLf + " SELECT 'TOTAL' AS TOTAL,SUM(PCS)PCS, SUM(GRSWT)+SUM(WASTAGE) AS GRSWT, SUM(GRSWT)+SUM(WASTAGE) AS NETWT, "
        strsql += vbCrLf + " SUM(MCHARGE)MC,SUM(AMOUNT+TAX)AMOUNT "
        strsql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT WHERE TRANTYPE = 'RRE' " ' RECEIPT TOTAL
        strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "'"
        strsql += vbCrLf + " GROUP BY BATCHNO"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT 'TOTAL' AS TOTAL,SUM(PCS)PCS, SUM(GRSWT)GRSWT, SUM(NETWT)NETWT, SUM(MCHARGE)MC,SUM(AMOUNT+TAX)AMOUNT "
        strsql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT WHERE TRANTYPE = 'RPU' " ' PURCHASE TOTAL
        strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "'"
        strsql += vbCrLf + " GROUP BY BATCHNO"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT 'TOTAL' AS TOTAL,SUM(PCS)PCS, SUM(GRSWT) + SUM(ALLOY) AS GRSWT, SUM(GRSWT) + SUM(ALLOY) AS NETWT, "
        strsql += vbCrLf + " SUM(MCHARGE)MC,SUM(AMOUNT+TAX)AMOUNT "
        strsql += vbCrLf + " FROM " & cnStockDb & "..ISSUE WHERE TRANTYPE IN('IIS','IPU') " ' ISSUE TOTAL
        strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "'"
        strsql += vbCrLf + " GROUP BY BATCHNO"
        strsql += vbCrLf + " ) X"

        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = vbCrLf + " SELECT ACCODE FROM " & cnStockDb & "..RECEIPT "
        strsql += vbCrLf + " WHERE BATCHNO = '" & BatchNo & "' "
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT ACCODE FROM " & cnStockDb & "..ISSUE "
        strsql += vbCrLf + " WHERE BATCHNO = '" & BatchNo & "' "
        Dim ACCODE As String = GetSqlValue_Bill(strsql)

        strsql = vbCrLf + "SELECT ACCODE"
        strsql += vbCrLf + " ,ACNAME"
        strsql += vbCrLf + " ,DOORNO + ' ' + ADDRESS1  AS ADDRESS1"
        strsql += vbCrLf + " ,ADDRESS2 + ' ' + ADDRESS3 + ' ' + CITY + ' ' + PINCODE AS ADDRESS2 "
        strsql += vbCrLf + " ,CASE WHEN TIN <> '' THEN 'TIN:'+TIN + ' ' END AS TIN"
        strsql += vbCrLf + " ,CASE WHEN PAN <> '' THEN 'PAN:'+PAN END AS PAN"
        strsql += vbCrLf + " INTO TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE_ACCODE]"
        strsql += vbCrLf + " FROM " & cnAdminDb & "..ACHEAD "
        strsql += vbCrLf + " WHERE 1 = 1 "
        strsql += vbCrLf + " AND ACCODE = '" & ACCODE & "'"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = "SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE]"
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dsSmithIssueReceipt, "DTISSREC")
        strsql = "SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECETOTAL]"
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dsSmithIssueReceipt, "DTISSRECTOTAL")
        strsql = "SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE_ACCODE]"
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dsSmithIssueReceipt, "ACHEAD")
    End Sub
    Public Sub OrderReceipt(ByVal PbatchNo As String, ByVal Type As String)
        strsql = " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "ORMASTACCTRAN]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "ORMASTACCTRAN]"
        strsql += " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "ORMAST]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "ORMAST]"
        strsql += " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "OCUSTORDER]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "OCUSTORDER]"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = " CREATE TABLE TEMPTABLEDB..[TEMP" & SystemName & "ORMASTACCTRAN] (ACCODE VARCHAR(250), BATCHNO_N VARCHAR(20))"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        'Doubt 
        'Dim OutSt As Boolean
        'strsql = vbCrLf + " SELECT ACCODE+':'+CONVERT(VARCHAR,AMOUNT)AMT,BATCHNO FROM " & cnStockDb & "..ACCTRAN "
        'strsql += vbCrLf + " WHERE BATCHNO =  '" & PbatchNo & "' AND TRANMODE = 'D'"
        'da = New OleDbDataAdapter(strsql, cn)
        'dt = New DataTable
        'da.Fill(dt)
        'Dim TempAcc As String = ""
        'Dim TempBatch As String = ""
        'If dt.Rows.Count > 0 Then
        '    For i As Integer = 0 To dt.Rows.Count - 1
        '        TempAcc += " " + dt.Rows(i).Item("AMT").ToString
        '    Next
        '    TempBatch = dt.Rows(0).Item("BATCHNO").ToString
        '    strsql = "INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ORMASTACCTRAN](ACCODE,BATCHNO_N) VALUES "
        '    strsql += " ('" & TempAcc & "', '" & TempBatch & "')"
        '    cmd = New OleDbCommand(strsql, cn)
        '    cmd.ExecuteNonQuery()
        '    OutSt = True
        'End If

        strsql = vbCrLf + " SELECT "
        strsql += vbCrLf + " ROW_NUMBER() OVER(ORDER BY ORDATE DESC) AS SRNO"
        strsql += vbCrLf + " ,SUBSTRING(ORNO,6,3) + '-' + SUBSTRING(ORNO,9,1000)  AS ORNO "
        strsql += vbCrLf + " ,CONVERT (VARCHAR(15),ORDATE,103) AS BILLDATE"
        strsql += vbCrLf + " ,(SELECT CONVERT(VARCHAR(500),ITEMNAME) ITEMNAME from " & cnAdminDb & "..ITEMMAST WHERE itemid = O.ITEMID) AS ITEMNAME"
        strsql += vbCrLf + " ,PCS"
        strsql += vbCrLf + " ,GRSWT,RATE"
        strsql += vbCrLf + " ,BATCHNO AS BATCHNO_N,CONVERT (VARCHAR(15), DUEDATE ,103) AS DUEDATE "
        strsql += vbCrLf + " ,(SELECT SUM(AMOUNT) FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO=O.BATCHNO AND PAYMODE='OR')AMOUNT"
        strsql += vbCrLf + " ,SYSTEMID AS NODE"
        strsql += vbCrLf + " ,1 AS RESULT, 1 AS TYPE, '' AS COLHEAD "
        strsql += vbCrLf + " INTO TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] "
        strsql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST as O"
        strsql += vbCrLf + " WHERE BATCHNO =  '" & PbatchNo & "'"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] "
        strsql += vbCrLf + " SELECT NULL,NULL,NULL,DESCRIPT,NULL,NULL,NULL,BATCHNO,NULL,NULL,SYSTEMID"
        strsql += vbCrLf + " ,2 AS RESULT, 1 AS TYPE, '' AS COLHEAD "
        strsql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST as O"
        strsql += vbCrLf + " WHERE BATCHNO =  '" & PbatchNo & "' "
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] "
        strsql += vbCrLf + " SELECT NULL,NULL,NULL,REASON,NULL,NULL,NULL,BATCHNO,NULL,NULL,SYSTEMID"
        strsql += vbCrLf + " ,3 AS RESULT, 1 AS TYPE, '' AS COLHEAD "
        strsql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST as O"
        strsql += vbCrLf + " WHERE BATCHNO =  '" & PbatchNo & "' "
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] "
        strsql += vbCrLf + " SELECT NULL,NULL,NULL,'TOTAL',SUM(PCS),SUM(GRSWT),NULL,BATCHNO,NULL,NULL,SYSTEMID"
        strsql += vbCrLf + " ,9 AS RESULT, 1 AS TYPE, 'U' AS COLHEAD "
        strsql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST as O"
        strsql += vbCrLf + " WHERE BATCHNO =  '" & PbatchNo & "' GROUP BY SYSTEMID,BATCHNO"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        'Space Line
        strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] "
        strsql += vbCrLf + " SELECT NULL,NULL,NULL,NULL,NULL,NULL,NULL,BATCHNO,NULL,NULL,SYSTEMID"
        strsql += vbCrLf + " ,10 AS RESULT, 1 AS TYPE, '' AS COLHEAD "
        strsql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST AS O"
        strsql += vbCrLf + " WHERE BATCHNO =  '" & PbatchNo & "' GROUP BY SYSTEMID,BATCHNO"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] "
        strsql += vbCrLf + " SELECT NULL,NULL,NULL,'RATE FIXED   :'+CONVERT(VARCHAR(20),RATE)"
        strsql += vbCrLf + " ,NULL,NULL,NULL,BATCHNO,NULL,NULL,SYSTEMID"
        strsql += vbCrLf + " ,12 AS RESULT, 1 AS TYPE, 'G' AS COLHEAD "
        strsql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST AS O "
        strsql += vbCrLf + " WHERE BATCHNO =  '" & PbatchNo & "' AND ORRATE='C' GROUP BY SYSTEMID,BATCHNO,RATE"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] "
        strsql += vbCrLf + " SELECT NULL,NULL,NULL, "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'CC' THEN 'CARD ' + AC.SHORTNAME + ' ' + CHQCARDNO + '     :' + LTRIM(SUM(AMOUNT)) ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'AA' THEN 'ADVANCE ' + 'REFER : ' + SUBSTRING(REFNO,1,3) + '-' + SUBSTRING(REFNO,4,10) + '     :' + LTRIM(SUM(AMOUNT)) ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'CA' THEN 'CASH ' + '     :' + LTRIM(SUM(AMOUNT)) ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'PU' THEN 'PURCHASE ' + ' :' + LTRIM(SUM(AMOUNT)) ELSE"
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'SR' THEN 'SALES RETURN ' + ' :' + LTRIM(SUM(AMOUNT)) ELSE "
        strsql += vbCrLf + " 'CHEQUE ' + CHQCARDREF + ' ' + CHQCARDNO + '     :' + LTRIM(SUM(AMOUNT))" 'CH
        strsql += vbCrLf + " END END END END END"
        strsql += vbCrLf + " ,NULL,NULL,NULL,BATCHNO,NULL,NULL,SYSTEMID, "
        strsql += vbCrLf + " 13 RESULT, 1 TYPE, 'G' AS COLHEAD FROM " & cnStockDb & "..ACCTRAN T"
        strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CREDITCARD AC ON AC.ACCODE=T.ACCODE "
        strsql += vbCrLf + " WHERE PAYMODE IN ('AA','CC','CH','CA','PU','SR')"
        strsql += vbCrLf + " AND BATCHNO = '" & PbatchNo & "'"
        strsql += vbCrLf + " GROUP BY PAYMODE,CHQCARDNO,BATCHNO,AC.SHORTNAME,CHQCARDREF,REFNO,SYSTEMID"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] "
        strsql += vbCrLf + " SELECT NULL,NULL,NULL,'TOTAL ADVANCE :'+CONVERT(VARCHAR(20),SUM(AMOUNT))"
        strsql += vbCrLf + " ,NULL,NULL,NULL,BATCHNO,NULL,NULL,SYSTEMID"
        strsql += vbCrLf + " ,14 AS RESULT, 1 AS TYPE, 'G' AS COLHEAD "
        strsql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS O"
        strsql += vbCrLf + " WHERE BATCHNO =  '" & PbatchNo & "' AND PAYMODE='OR' GROUP BY SYSTEMID,BATCHNO"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = "SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] ORDER BY RESULT"
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtOrderRepair)

        Tranno = dtOrderRepair.Rows(0).Item("ORNO").ToString
        strBilldate = dtOrderRepair.Rows(0).Item("BILLDATE").ToString
        NodeId = dtOrderRepair.Rows(0).Item("NODE").ToString
        DueDate = dtOrderRepair.Rows(0).Item("DUEDATE").ToString

        'strsql = vbCrLf + " SELECT  "
        'strsql += vbCrLf + " (SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS PNAME,  "
        'strsql += vbCrLf + " (SELECT (DOORNO + ' ' + ADDRESS1) FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS ADDRESS1,"
        'strsql += vbCrLf + " (SELECT (ADDRESS2 + ' ' + ADDRESS3 + ' ' + CITY + ' ' + PINCODE ) FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS ADDRESS2,"
        'strsql += vbCrLf + " (SELECT CASE WHEN MOBILE<> '' THEN MOBILE ELSE PHONERES END FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS MOBILENO,"
        'strsql += vbCrLf + " (SELECT PAN  FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS TIN, BATCHNO AS BATCHNO_N"
        'If Type = "ORD" Then
        '    strsql += vbCrLf + ",'ORDER BOOKING' AS TYPE"
        '    strsql += vbCrLf + ",'Order No' AS TYPE1"
        'Else
        '    strsql += vbCrLf + ",'REPAIR BOOKING' AS TYPE"
        '    strsql += vbCrLf + ",'Repair No' AS TYPE1"
        'End If
        'strsql += vbCrLf + " INTO TEMPTABLEDB..[TEMP" & SystemName & "OCUSTORDER] "
        'strsql += vbCrLf + " FROM " & cnAdminDb & "..CUSTOMERINFO AS P "
        'strsql += vbCrLf + " WHERE BATCHNO = '" & PbatchNo & "'"
        'cmd = New OleDbCommand(strsql, cn)
        'cmd.ExecuteNonQuery()

    End Sub
    Public Sub CustomerInfoAdvanceReceipt(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        If dsAdvanceReceipt.Tables("CUSTOMER").Rows.Count > 0 And dsAdvanceReceipt.Tables("OUTSTANDING").Rows.Count > 0 And dsAdvanceReceipt.Tables("ACCTRAN").Rows.Count > 0 Then
            Dim pname As String = dsAdvanceReceipt.Tables("CUSTOMER").Rows(0).Item("pname").ToString
            Dim Address1 As String = dsAdvanceReceipt.Tables("CUSTOMER").Rows(0).Item("address1").ToString
            Dim Address2 As String = dsAdvanceReceipt.Tables("CUSTOMER").Rows(0).Item("address2").ToString
            Dim Mobile As String = dsAdvanceReceipt.Tables("CUSTOMER").Rows(0).Item("mobileno").ToString
            Dim Tin As String = dsAdvanceReceipt.Tables("CUSTOMER").Rows(0).Item("tin").ToString

            Dim TranName As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("tranname").ToString
            Dim Tranno As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("tranno").ToString
            Dim Billdate As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("billdate").ToString
            Dim Field1 As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field1").ToString
            Dim Field2 As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field2").ToString
            Dim Amt As Integer = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("amount").ToString

            Dim PurityType As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("CATCODETYPE").ToString

            Dim AmtWords As String = ConvertRupees.RupeesToWord(Amt)
            Dim Field5 As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field5").ToString
            NodeId = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("node").ToString
            Dim accode As String = dsAdvanceReceipt.Tables("ACCTRAN").Rows(0).Item("ACCODE").ToString

            Dim ItemDetail As String = "ITEM DETAIL"
            Dim RateFixed As String = dsAdvanceReceipt.Tables("ITEMTAG").Rows(0).Item("RATENAME").ToString
            Dim RateValue As String = dsAdvanceReceipt.Tables("ITEMTAG").Rows(0).Item("RATE").ToString




            g.DrawString(pname, fontRegular, BlackBrush, c1, TopY, LeftFormat)
            'g.DrawString(pname, fontRegular, BlackBrush, c1, BottomY, LeftFormat)
            g.DrawString(TranName, fontRegular, BlackBrush, c6, TopY, LeftFormat)
            'g.DrawString(TranName, fontRegular, BlackBrush, c6, BottomY, LeftFormat)
            g.DrawString("   : " & Tranno, fontRegular, BlackBrush, c7, TopY, LeftFormat)
            'g.DrawString("   : " & Tranno, fontRegular, BlackBrush, c7, BottomY, LeftFormat)

            TopY = TopY + 20
            'BottomY = BottomY + 20

            g.DrawString(Address1, fontRegular, BlackBrush, c1, TopY, LeftFormat)
            'g.DrawString(Address1, fontRegular, BlackBrush, c1, BottomY, LeftFormat)
            g.DrawString("Date", fontRegular, BlackBrush, c6, TopY, LeftFormat)
            'g.DrawString("Date", fontRegular, BlackBrush, c6, BottomY, LeftFormat)
            g.DrawString("   : " & Billdate, fontRegular, BlackBrush, c7, TopY, LeftFormat)
            'g.DrawString("   : " & Billdate, fontRegular, BlackBrush, c7, BottomY, LeftFormat)

            TopY = TopY + 20
            'BottomY = BottomY + 20

            g.DrawString(Address2, fontRegular, BlackBrush, c1, TopY, LeftFormat)
            'g.DrawString(Address2, fontRegular, BlackBrush, c1, BottomY, LeftFormat)
            g.DrawString(Field1, fontBoldTitle, Brushes.Black, c3, TopY)
            'g.DrawString(Field1, fontBoldTitle, Brushes.Black, c3, BottomY)
            TopY = TopY + 20
            'BottomY = BottomY + 20
            g.DrawString(Mobile & IIf(Tin <> "", "TIN : " & Tin, ""), fontRegular, BlackBrush, c1, TopY, LeftFormat)
            'g.DrawString(Mobile & IIf(Tin <> "", "TIN : " & Tin, ""), fontRegular, BlackBrush, c1, BottomY, LeftFormat)
            TopY = TopY + 20
            'BottomY = BottomY + 20
            g.DrawLine(Pens.Silver, c1, TopY, c9, TopY)
            'g.DrawLine(Pens.Silver, c1, BottomY, c9, BottomY)
            TopY = TopY + 20
            'BottomY = BottomY + 20
            g.DrawString(Field2, fontRegular, BlackBrush, c1, TopY, LeftFormat)
            'g.DrawString(Field2, fontRegular, BlackBrush, c1, BottomY, LeftFormat)
            If accode <> "" Then
                If accode.Length < 50 Then
                    g.DrawString(accode, fontRegular, BlackBrush, c4, TopY, LeftFormat)
                    'g.DrawString(accode, fontRegular, BlackBrush, c4, BottomY, LeftFormat)
                End If
                If accode.Length >= 50 Then
                    g.DrawString(Mid(accode, 1, 50), New Font("Palatino Linotype", 8, FontStyle.Regular), BlackBrush, c4, TopY, LeftFormat)
                    'g.DrawString(Mid(accode, 1, 50), New Font("Palatino Linotype", 8, FontStyle.Regular), BlackBrush, c4, BottomY, LeftFormat)
                    g.DrawString(Mid(accode, 51, 50), New Font("Palatino Linotype", 8, FontStyle.Regular), BlackBrush, c4, TopY + 20, LeftFormat)
                    'g.DrawString(Mid(accode, 51, 50), New Font("Palatino Linotype", 8, FontStyle.Regular), BlackBrush, c4, BottomY + 20, LeftFormat)
                    If accode.Length > 100 Then
                        TopY = TopY + 20
                        'BottomY = BottomY + 20
                        g.DrawString(Mid(accode, 101), New Font("Palatino Linotype", 8, FontStyle.Regular), BlackBrush, c4, TopY + 20, LeftFormat)
                        'g.DrawString(Mid(accode, 101), New Font("Palatino Linotype", 8, FontStyle.Regular), BlackBrush, c4, BottomY + 20, LeftFormat)
                        TopY = TopY - 20
                        'BottomY = BottomY - 20
                    End If
                End If
            End If
            TopY = TopY + 20
            'BottomY = BottomY + 20
            g.DrawString(AmtWords, fontRegular, BlackBrush, c1, TopY, LeftFormat)
            'g.DrawString(AmtWords, fontRegular, BlackBrush, c1, BottomY, LeftFormat)
            'If dsAdvanceReceipt.Tables("ISSUE").Rows.Count > 0 Then
            '    Dim GrswA As String = dsAdvanceReceipt.Tables("ISSUE").Rows(0).Item("GRSWT")
            '    g.DrawString("GRSWT" & Space(6) & ":" & GrswA, fontRegular, BlackBrush, c4, TopY, LeftFormat)
            '    g.DrawString("GRSWT" & Space(6) & ":" & GrswA, fontRegular, BlackBrush, c4, BottomY, LeftFormat)
            'End If
            TopY = TopY + 20
            ' BottomY = BottomY + 20
            g.DrawString(Field5, fontRegular, BlackBrush, c1, TopY, LeftFormat)
            'g.DrawString(Field5, fontRegular, BlackBrush, c1, BottomY, LeftFormat)
            TopY = TopY + 20
            ' BottomY = BottomY + 20
            If dsAdvanceReceipt.Tables("ISSUE").Rows.Count > 0 Then
                Dim GrswA As String = dsAdvanceReceipt.Tables("ISSUE").Rows(0).Item("GRSWT")
                Dim AmtA As String = dsAdvanceReceipt.Tables("ISSUE").Rows(0).Item("AMOUNT")
                g.DrawString("GRSWT" & Space(6) & ":" & GrswA & "GRSAMT" & Space(5) & ":" & AmtA, fontRegular, BlackBrush, c4, TopY, LeftFormat)
                'g.DrawString("GRSWT" & Space(6) & ":" & GrswA & "GRSAMT" & Space(5) & ":" & AmtA, fontRegular, BlackBrush, c4, BottomY, LeftFormat)
            End If
            TopY = TopY + 20
            ' BottomY = BottomY + 20

            'ADVANCE ONLY DISPLAY
            If dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("Field1").ToString.Contains("ADVANCE ") Then
                g.DrawString(ItemDetail, fontBold, BlackBrush, c1, TopY, LeftFormat)
                ' g.DrawString(ItemDetail, fontBold, BlackBrush, c1, BottomY, LeftFormat)
                g.DrawString(RateFixed, fontBold, BlackBrush, c3, TopY, LeftFormat)
                ' g.DrawString(RateFixed, fontBold, BlackBrush, c3, BottomY, LeftFormat)

                g.DrawString(IIf(Val(RateValue) <> 0, " : " & RateValue, ""), fontBold, BlackBrush, c4, TopY, LeftFormat)
                ' g.DrawString(IIf(Val(RateValue) <> 0, " : " & RateValue, ""), fontBold, BlackBrush, c4, BottomY, LeftFormat)

                g.DrawString(PurityType, fontBold, BlackBrush, c5, TopY, LeftFormat)
                ' g.DrawString(PurityType, fontBold, BlackBrush, c5, BottomY, LeftFormat)

                TopY = TopY + 15
                ' BottomY = BottomY + 15
                g.DrawLine(Pens.Black, c1, TopY, 110, TopY)
                ' g.DrawLine(Pens.Black, c1, BottomY, 110, BottomY)
                TopY = TopY + 20
                'BottomY = BottomY + 20
            End If
        End If
    End Sub
    Public Sub AdvancePrint(ByVal pBatchno As String)
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
        'strsql = vbCrLf + " SELECT CASE WHEN PAYMODE='CA' THEN 'CASH' "
        'strsql += vbCrLf + " WHEN PAYMODE='PU' THEN 'PURCHASE' "
        'strsql += vbCrLf + " WHEN PAYMODE IN('SR','RV') THEN 'RETURN' "
        'strsql += vbCrLf + " WHEN PAYMODE IN('AP') THEN 'ADVANCE REPAY' "
        'strsql += vbCrLf + " WHEN PAYMODE='CC' THEN 'CARD' WHEN PAYMODE='CH' THEN 'CHEQUE' ELSE 'OTHERS' END "
        'strsql += vbCrLf + " +':'+CONVERT(VARCHAR,SUM(AMOUNT))AMT,BATCHNO FROM " & cnStockDb & "..ACCTRAN "
        'strsql += vbCrLf + " WHERE BATCHNO =  '" & pBatchno & "' AND TRANMODE = 'D' GROUP BY PAYMODE,BATCHNO"
        strsql = vbCrLf + " SELECT "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'CC' THEN 'CARD ' + AC.SHORTNAME + ' ' + CHQCARDNO + ' :' + LTRIM(SUM(AMOUNT)) ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'AA' THEN 'ADVANCE ' + 'REFER : ' + SUBSTRING(REFNO,1,3) + '-' + SUBSTRING(REFNO,4,10) + ' :' + LTRIM(SUM(AMOUNT)) ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'CA' THEN 'CASH ' + ' :' + LTRIM(SUM(AMOUNT)) ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'PU' THEN 'PURCHASE ' + ' :' + LTRIM(SUM(AMOUNT)) ELSE"
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'SR' THEN 'SALES RETURN ' + ' :' + LTRIM(SUM(AMOUNT)) ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'AP' THEN 'ADVANCE REPAY ' + ' :' + LTRIM(SUM(AMOUNT)) ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'RV' THEN 'RETURN ' + ' :' + LTRIM(SUM(AMOUNT)) ELSE "
        strsql += vbCrLf + " 'CHEQUE ' + CHQCARDREF + ' ' + CHQCARDNO + ' :' + LTRIM(SUM(AMOUNT))" 'CH
        strsql += vbCrLf + " END END END END END END END AS AMT "
        strsql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN T"
        strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CREDITCARD AC ON AC.ACCODE=T.ACCODE "
        strsql += vbCrLf + " WHERE PAYMODE IN ('AA','CC','CH','CA','PU','SR','RV','AP')"
        strsql += vbCrLf + " AND BATCHNO = '" & pBatchno & "'"
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
            'TempBatch = dt.Rows(0).Item("BATCHNO").ToString
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
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'RE' THEN 'Due No ' ELSE 'Voucher No ' END"
        strsql += vbCrLf + " END AS TranName,"
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'AR' THEN 'ADVANCE  RECEIPT ' ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'RE' THEN 'DUE  RECEIPT ' ELSE 'RECEIPT  VOUCHER' END"
        strsql += vbCrLf + " END AS Field1,"
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'AR' THEN 'Received with thanks from ' ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'RE' THEN 'Credit bill ' ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'DR' THEN  'Received with thanks from ' ELSE 'Receipt Bill'   "
        strsql += vbCrLf + " END END END + "
        strsql += vbCrLf + " (SELECT (SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = C.PSNO )"
        strsql += vbCrLf + " FROM " & cnAdminDb & "..CUSTOMERINFO AS C "
        strsql += vbCrLf + " WHERE BATCHNO = o.BATCHNO) + ' Rs.' + CONVERT(VARCHAR,AMOUNT) Field2"
        strsql += vbCrLf + " ,AMOUNT, NULL AMT "
        'strSql += vbCrLf + " ,'Refer No :' + CONVERT(VARCHAR, SUBSTRING(O.RUNNO,6,20)) as  Field3"
        strsql += vbCrLf + " ,CASE WHEN PAYMODE = 'AR' THEN 'Towards ADVANCE' ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'RE' THEN 'Towards CREDIT' ELSE "
        strsql += vbCrLf + " 'Towards RECEIPT' "
        strsql += vbCrLf + " END END + ' Refer No :' + CONVERT(VARCHAR, SUBSTRING(O.RUNNO,6,20)) AS Field5 ,"
        strsql += vbCrLf + " BATCHNO AS BATCHNO_N, TRANNO,"
        strsql += vbCrLf + " CONVERT(VARCHAR(15),TRANDATE,103) + SUBSTRING(CONVERT(VARCHAR,UPTIME,109),13,5)+SUBSTRING(CONVERT(VARCHAR,UPTIME,109),25,5) AS BILLDATE, "
        strsql += vbCrLf + " 'C' AS TRANMODE,0 AS ISTAG,0 AS ISOG"
        strsql += vbCrLf + " ,SYSTEMID AS NODE"
        strsql += vbCrLf + " ,ISNULL ((SELECT "
        strsql += vbCrLf + " (SELECT TOP 1 SHORTNAME FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID =C.PURITYID) "
        strsql += vbCrLf + " FROM " & cnAdminDb & "..CATEGORY AS C WHERE CATCODE = O.CATCODE), '') CATCODETYPE"
        strsql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS O "
        strsql += vbCrLf + " WHERE BATCHNO = '" & pBatchno & "'"
        strsql += vbCrLf + " )X"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = vbCrLf + " SELECT  "
        strsql += vbCrLf + " (SELECT PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS PNAME,  "
        strsql += vbCrLf + " (SELECT (DOORNO + ' ' + ADDRESS1) FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS ADDRESS1,"
        strsql += vbCrLf + " (SELECT (ADDRESS2 + ' ' + ADDRESS3 + ' ' + CITY + ' ' + PINCODE ) FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS ADDRESS2,"
        strsql += vbCrLf + " (SELECT CASE WHEN MOBILE<> '' THEN MOBILE ELSE PHONERES END FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS MOBILENO,"
        strsql += vbCrLf + " (SELECT PAN  FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = P.PSNO) AS TIN, BATCHNO AS BATCHNO_N"
        strsql += vbCrLf + " INTO TEMPTABLEDB..[TEMP" & SystemName & "CUSTADVANCERECEIPT] "
        strsql += vbCrLf + " FROM " & cnAdminDb & "..CUSTOMERINFO AS P "
        strsql += vbCrLf + " WHERE BATCHNO = '" & pBatchno & "'"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = "SELECT BATCHNO FROM " & cnAdminDb & "..ITEMTAG T WHERE BATCHNO='" & pBatchno & "' ORDER BY ITEMID"
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
            strsql += vbCrLf + ",(SELECT TOP 1 RATE FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO= '" & pBatchno & "' ) AS RATE"
            strsql += vbCrLf + ",(SELECT TOP 1 CASE WHEN RATE = 0 THEN 'RATE NOT FIXED' ELSE 'RATE FIXED' END FROM " & cnAdminDb & "..OUTSTANDING WHERE BATCHNO= '" & pBatchno & "' ) AS RATENAME"
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
        strsql += vbCrLf + "  WHERE I.BATCHNO='" & pBatchno & "' AND I.TRANTYPE IN ('PU','SR')"
        strsql += vbCrLf + "  GROUP BY I.TRANDATE,I.BATCHNO  " ' ,I.TRANNO
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = "SELECT COUNT(*)CNT FROM TEMPTABLEDB..[TEMP" & SystemName & "ITEMTAG] "
        If Val(GetSqlValue_Bill(strsql, "CNT", 0)) > 0 Then
            strsql = " UPDATE TEMPTABLEDB..[TEMP" & SystemName & "ADVANCERECEIPT] SET ISTAG=1"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        End If
        strsql = "SELECT COUNT(*)CNT FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO='" & pBatchno & "' AND TRANTYPE IN ('PU','SR')"
        If Val(GetSqlValue_Bill(strsql, "CNT", 0)) > 0 Then
            strsql = " UPDATE TEMPTABLEDB..[TEMP" & SystemName & "ADVANCERECEIPT] SET ISOG=1"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        End If

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
    Public Sub Print(ByVal pBatchno As String)
        PrintSALEINVOICE(pBatchno, "")
    End Sub

    Dim defaultPDFdesitnationPath As String
    Dim CheckPath As String

#Region "ImageFiletoBytes"
    Public Function ImageToByte(ByVal img As Image) As Byte()
        Dim imgStream As MemoryStream = New MemoryStream()
        img.Save(imgStream, System.Drawing.Imaging.ImageFormat.Jpeg)
        imgStream.Close()
        Dim byteArray As Byte() = imgStream.ToArray()
        imgStream.Dispose()
        Return byteArray
    End Function
#End Region


    'Private Sub sendMail()

    '    Try

    '        Dim Smtp As SmtpMail

    '        SmtpMail.SmtpServer.Insert(0, "gmail.com")

    '        Dim Msg As MailMessage = New MailMessage

    '        Msg.To = "akshaya.apps@gmail.com"

    '        Msg.From = "akshaya.apps@gmail.com"

    '        Msg.Subject = "Crystal Report Attachment "

    '        Msg.Body = "Crystal Report Attachment "

    '        Msg.Attachments.Add(New MailAttachment(pdfFile))

    '        Smtp.Send(Msg)

    '    Catch ex As Exception

    '        MsgBox(ex.ToString)

    '    End Try

    'End Sub

    Private Function PrintPdf()

        Try
            If IO.File.Exists(Application.StartupPath & "\BillPrint\crBillPrint.rpt") Then


                'MsgBox("Creating PDF...", MsgBoxStyle.Information)
                file = (Application.StartupPath & "\BillPrint\crBillPrint.rpt")
                cryRpt.Load(file)
                ' cryRpt.Load("D:\GIRITECH\SOFTWARE\NAC_NewRetail\AkshayaRetailJewellery\Bill\crBillPrint.rpt")
                cryRpt.Database.Tables("DTSALES").SetDataSource(DirectCast(dtSales, DataTable))
                CrystalReportViewer1.ReportSource = cryRpt
                CrystalReportViewer1.Refresh()
                CrystalReportViewer1.Show()

                'pdfFile = "c:\windows\temp\" & pBatchno & ".pdf"
                pdfFile = "" & Application.StartupPath & "\BillPrint\" & pBatchno & ".pdf"
            Else
                MsgBox("crBillPrint.rpt not found... Please Contact Administrator", MsgBoxStyle.Information)
            End If
        Catch ex As Exception

            MsgBox(ex.ToString)

        End Try
        Try

            Dim CrExportOptions As ExportOptions

            Dim CrDiskFileDestinationOptions As New DiskFileDestinationOptions()

            Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions

            CrDiskFileDestinationOptions.DiskFileName = pdfFile

            CrExportOptions = cryRpt.ExportOptions

            With CrExportOptions

                .ExportDestinationType = ExportDestinationType.DiskFile

                .ExportFormatType = ExportFormatType.PortableDocFormat

                .DestinationOptions = CrDiskFileDestinationOptions

                .FormatOptions = CrFormatTypeOptions

            End With

            cryRpt.Export()

        Catch ex As Exception

            MsgBox(ex.ToString)

        End Try
        'MsgBox("Creating PDF...", MsgBoxStyle.Information)
        If IO.File.Exists(Application.StartupPath & "\BillPrint\crBillPrint1.rpt") Then
            file = (Application.StartupPath & "\BillPrint\crBillPrint1.rpt")
            cryRpt.Load(file)

            'cryRpt.Load("D:\GIRITECH\SOFTWARE\NAC_NewRetail\AkshayaRetailJewellery\Bill\crBillPrint1.rpt")
            cryRpt.Database.Tables("DTSALES").SetDataSource(DirectCast(dtSales, DataTable))
            CrystalReportViewer1.ReportSource = cryRpt
            CrystalReportViewer1.Refresh()
            CrystalReportViewer1.Show()
            cryRpt.PrintToPrinter(1, False, 0, 0)
        Else
            MsgBox("crBillPrint1.rpt not found... Please Contact Administrator", MsgBoxStyle.Information)
        End If
        Dim strSub As String = "You've received a payment invoice from NAC Jewellers"
        strsql = " Dear Sir/Madam, "
        strsql += vbCrLf + " "
        strsql += vbCrLf + " Please keep the attached invoice for your records. If you have any questions, please contact us at care@nacjewellers.com"
        strsql += vbCrLf + " "
        strsql += vbCrLf + " Thank you for your kind cooperation. We appreciate your business."
        strsql += vbCrLf + " "
        strsql += vbCrLf + " Happy Shopping ! "
        strsql += vbCrLf + " Team NAC"
        If dtCustInfo.Rows.Count > 0 Then
            If dtCustInfo.Rows(0)("EMAIL").ToString <> "" Then
                If MAILSEND(dtCustInfo.Rows(0)("EMAIL").ToString, strSub, strsql, pdfFile) = False Then
                    MsgBox("Mail cannot sent..." + vbCrLf + "Pls check Internet connection or Email Address.", MsgBoxStyle.Information)
                End If
            End If
        End If
        'sendMail()
        'System.IO.File.Delete(pdfFile)
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

        Exit Function
        Using ms As New IO.MemoryStream
            Dim doc As New iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10, 10, 50, 50)
            Dim contentFont As iTextSharp.text.Font = iTextSharp.text.FontFactory.GetFont("Verdana", 9, iTextSharp.text.Font.NORMAL)
            Dim writer As iTextSharp.text.pdf.PdfWriter
            strsql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PDF_PICPATH'"
            defaultPDFdesitnationPath = UCase(objGPack.GetSqlValue(strsql, "CTLTEXT", , tran))
            If Not defaultPDFdesitnationPath.EndsWith("\") And defaultPDFdesitnationPath <> Nothing Then defaultPDFdesitnationPath += "\"
            Try
                If (System.IO.Directory.Exists(defaultPDFdesitnationPath)) Then
                    CheckPath = defaultPDFdesitnationPath + "T" + "tagno" + ".pdf"
                    'Dim fs As New FileStream(CheckPath, FileMode.Create, FileAccess.Write, FileShare.None)
                    'If File.Exists(CheckPath) = True Then
                    '    File.Delete(CheckPath)
                    'End If
                    'Dim fs As New IO.FileStream(CheckPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read)
                    strsql = " DECLARE @DEFPATH VARCHAR(200)"
                    strsql += vbCrLf + " SELECT @DEFPATH = '" & defaultPic & "'"
                    strsql += vbCrLf + " SELECT CASE WHEN PCTFILE <> '' THEN @DEFPATH + PCTFILE END AS PCTFILE"
                    strsql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T "
                    strsql += vbCrLf + "WHERE T.TAGNO = '" & "tagno" & "' AND T.ITEMID = " & "itemId" & ""
                    Dim ItextSharpImage As String = UCase(objGPack.GetSqlValue(strsql))
                    If ItextSharpImage.Contains("..jpg") Then
                        ItextSharpImage = ItextSharpImage.Replace("..", ".")
                    End If
                    Try
                        'writer.GetInstance(doc, fs)
                        writer.GetInstance(doc, ms)
                        Try
                            Try
                                doc.Open()

                                If ItextSharpImage <> "" Then
                                    Dim jpg As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(ItextSharpImage)
                                    jpg.ScaleToFit(140.0F, 140.0F)
                                    jpg.SpacingBefore = 10.0F
                                    jpg.SpacingAfter = 50.0F
                                    jpg.Alignment = 4
                                    doc.Add(jpg)
                                Else
                                    Dim bMap1 As Byte() = ImageToByte(My.Resources.no_photo)
                                    Dim jpg As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(bMap1)
                                    jpg.ScaleToFit(140.0F, 140.0F)
                                    jpg.SpacingBefore = 10.0F
                                    jpg.SpacingAfter = 50.0F
                                    jpg.Alignment = 4
                                    doc.Add(jpg)
                                End If

                                If "tagno" <> "" Then
                                    doc.Add(New iTextSharp.text.Paragraph("TAGNO" + ChrW(9) + ":" + ChrW(9) + "tagno", contentFont))
                                End If

                                For Each dr As DataRow In dtSales.Rows
                                    Dim srno As String = dr.Item("SRNO").ToString.PadLeft(4)
                                    Dim Desc As String = dr.Item("ITEMNAME").ToString.PadRight(20)
                                    Dim qty As String = dr.Item("QTY").ToString.PadLeft(4)
                                    Dim GrsWt As String = dr.Item("GRSWT").ToString.PadLeft(12)
                                    Dim NetWT As String = dr.Item("NETWT").ToString.PadLeft(12)
                                    Dim VA As String = dr.Item("WASTAGE").ToString.PadLeft(8)
                                    Dim Rate As String = dr.Item("RATE").ToString.PadLeft(8)
                                    Dim MC As String = dr.Item("MCHARGE").ToString.PadLeft(8)
                                    Dim Amount As String = dr.Item("AMOUNT").ToString.PadLeft(8)
                                    doc.Add(New iTextSharp.text.Paragraph(srno + Desc + qty + GrsWt + NetWT + VA + Rate + MC + Amount, contentFont))
                                    doc.Add(New iTextSharp.text.Cell("hggjhf"))
                                Next

                                doc.Close()
                                Dim content As Byte()
                                Dim contentLength As Integer
                                content = ms.ToArray()
                                contentLength = content.Length
                                Using fs As New IO.FileStream(CheckPath, FileMode.OpenOrCreate, FileAccess.Write)
                                    fs.Write(content, 0, contentLength)
                                End Using
                                MsgBox("Export Completed.", MsgBoxStyle.Information)
                            Catch ex As Exception
                                MsgBox("" + ex.ToString, MsgBoxStyle.Information)
                                Exit Function
                            Catch ex2 As IO.DirectoryNotFoundException
                                MsgBox("Image Directory Not Found", MsgBoxStyle.Information)
                                Exit Function
                            Catch ex1 As IO.FileNotFoundException
                                MsgBox("Image Not Found", MsgBoxStyle.Information)
                                Exit Function
                            Finally
                                'If (writer IsNot Nothing) Then
                                '    DirectCast(writer, IDisposable).Dispose()
                                'End If
                            End Try
                        Catch ex As Exception
                        Finally
                            'If (doc IsNot Nothing) Then
                            '    DirectCast(doc, IDisposable).Dispose()
                            'End If
                        End Try
                    Catch ex1 As Exception
                        MsgBox("" + ex1.ToString, MsgBoxStyle.Information)
                        Exit Function
                    Catch ex2 As iTextSharp.text.DocumentException
                        MsgBox("" + ex2.ToString, MsgBoxStyle.Information)
                        Exit Function
                    Catch ex3 As IO.IOException
                        MsgBox("" + ex3.ToString, MsgBoxStyle.Information)
                        Exit Function
                    Finally
                        'If (fs IsNot Nothing) Then
                        '    DirectCast(fs, IDisposable).Dispose()
                        'End If
                    End Try
                    Process.Start(CheckPath)
                    'fs.Dispose()
                    'fs.Close()
                Else
                    MsgBox("Pdf Path Not Found", MsgBoxStyle.Information)
                    Exit Function
                End If
            Catch ex As Exception
                If ex.Message.Contains("generic error occurred in GDI+") Then
                    MsgBox("Check Directory Permission.", MsgBoxStyle.Information)
                Else
                    MsgBox(ex.Message + vbCrLf + ex.StackTrace, MsgBoxStyle.Information)
                End If
            Finally
                'Dim p As System.Diagnostics.Process
                'p.GetProcessesByName(CheckPath)
                'p.Kill()
            End Try
        End Using
    End Function


    Function MAILSEND(ByVal ToMail As String, ByVal MailSub As String, ByVal MESSAGE As String, Optional ByVal Attachpath As String = "") As Boolean
        Dim obj As System.Web.Mail.SmtpMail
        Dim smtpServer As New SmtpClient()
        Dim mail As New MailMessage
        Dim Counter As Integer = 0
        strsql = vbCrLf + " <!DOCTYPE html>"
        strsql += vbCrLf + " <html>"
        strsql += vbCrLf + " <body>"
        strsql += vbCrLf + " <P><b>Dear Sir/Madam,</b></P>"
        strsql += vbCrLf + " <p></p>"
        strsql += vbCrLf + " <p>Please keep the attached invoice for your records. If you have any questions, please contact us at care@nacjewellers.com</p>"
        strsql += vbCrLf + " <p></p>"
        strsql += vbCrLf + " <p>Thank you for your kind cooperation. We appreciate your business.</p>"
        strsql += vbCrLf + " <p></p>"
        strsql += vbCrLf + " <p></p>"
        strsql += vbCrLf + " <p>Happy Shopping !</p>"
        strsql += vbCrLf + " <p>Team NAC</p>"
        strsql += vbCrLf + " <p></p>"
        strsql += vbCrLf + " </body>"
        strsql += vbCrLf + " </html>"
        MESSAGE = strsql
        Try
            'FromId = "akshaya.apps@gmail.com"
            'ToMail = "esyokesh@gmail.com"
            'MailPassword = "giri@123"
            Dim MailServer1 As String = Nothing
            Dim MailServer2 As String = Nothing
            If FromId.Contains("@") = True Then
                Dim SplitMailServer() As String = Split(FromId, "@")
                If Not SplitMailServer Is Nothing Then
                    MailServer1 = SplitMailServer(0)
                    MailServer2 = Trim(SplitMailServer(1))
                    MailServer2 = "@" & MailServer2
                End If
            End If
            If Trim(MailServer2) = "@gmail.com" Or Trim(MailServer2) = "@nacjewellers.com" Then
                smtpServer.Host = "smtp.gmail.com"
                smtpServer.Port = 587
                smtpServer.EnableSsl = True
            ElseIf Trim(MailServer2) = "@yahoo.com" Then
                smtpServer.Port = 465
                smtpServer.Host = "smtp.mail.yahoo.com"
                smtpServer.EnableSsl = True
            ElseIf Trim(MailServer2) = "@yahoo.co.in" Then
                smtpServer.Port = 587
                smtpServer.Host = "smtp.mail.yahoo.com"
                ' smtpServer.EnableSsl = True
            ElseIf Trim(MailServer2) = "@hotmail.com" Then
                smtpServer.Port = 587
                smtpServer.Host = "smtp.live.com"
                smtpServer.EnableSsl = True
            Else
                smtpServer.Port = 25
                smtpServer.Host = SmtpHostname
                smtpServer.EnableSsl = SmtpSSL
                smtpServer.EnableSsl = True
            End If
            NotifyIcon1.Visible = True
            NotifyIcon1.Icon = My.Resources.email
            NotifyIcon1.Text = "Sending Mail to " & ToMail
            NotifyIcon1.ShowBalloonTip(2000, "Information", "Sending Mail to " & ToMail, ToolTipIcon.Info)

            smtpServer.EnableSsl = True
            smtpServer.Credentials = New System.Net.NetworkCredential(FromId.Trim.ToString, MailPassword.Trim.ToString)

            mail.IsBodyHtml = True
            mail.From = New MailAddress(FromId)
            mail.To.Add(New MailAddress(ToMail))
            mail.Subject = MailSub
            mail.Body = MESSAGE
            If Attachpath <> "" Then Dim Attachment As System.Net.Mail.Attachment : Attachment = New System.Net.Mail.Attachment(Attachpath) : mail.Attachments.Add(Attachment)
            smtpServer.Send(mail)
            Return True
        Catch ex As Exception
            NotifyIcon1.Visible = True
            NotifyIcon1.Icon = My.Resources.email
            NotifyIcon1.ShowBalloonTip(5000, "Information", ex.Message, ToolTipIcon.Info)
            'If t2 IsNot Nothing Then t2.Abort()
            't2 = New Threading.Thread(AddressOf Initiator)
            't2.IsBackground = True
            't2.Priority = Threading.ThreadPriority.Lowest
            't2.Start()
            Return False
        End Try
    End Function


    Private Sub PrtDoc_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrtDoc.PrintPage
        Try
            Dim NoofPage As Integer = 0
            Dim rAlign As New StringFormat
            rAlign.Alignment = StringAlignment.Far
            NoofPrint += 1
            Using g As Graphics = e.Graphics
                'CustomerInfo(Tranno, strBilldate, e.Graphics, e)
                CustomerInfo(Tranno, prtBilldate, e.Graphics, e)
                Title(e.Graphics, e)
                RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
                If dtSales.Rows.Count > 0 Then
                    For PagecountSale = PagecountSale To dtSales.Rows.Count - 1
                        With dtSales.Rows(PagecountSale)
                            ''Top '/************************************////
                            NoofPage += 1
                            If .Item("RESULT").ToString = "3.40" Then
                                'TopY = TopY - 2
                                'BottomY = BottomY - 2
                                g.DrawLine(Pens.Silver, 710, (TopY - 2), c9, (TopY - 2))
                                'g.DrawLine(Pens.Silver, 710, (BottomY - 2), c9, (BottomY - 2))
                                'TopY = TopY + 2
                                'BottomY = BottomY + 2
                            End If
                            If .Item("RESULT").ToString = "3.0" And .Item("TYPE").ToString = "11.0" And .Item("COLHEAD").ToString = "G" Then
                                'MISC ISSUE TYPE 11 TOP UNDER
                                'TopY = TopY - 2
                                'BottomY = BottomY - 2
                                g.DrawLine(Pens.Silver, 340, (TopY - 2), c9, (TopY - 2))
                                'g.DrawLine(Pens.Silver, 340, (BottomY - 2), c9, (BottomY - 2))
                                'TopY = TopY + 2
                                'BottomY = BottomY + 2
                            End If
                            g.DrawString(.Item("SRNO").ToString(), fontRegular, BlackBrush, c1, TopY, LeftFormat)
                            If .Item("TYPE").ToString = "10" And .Item("COLHEAD").ToString = "G" Then
                                g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c3, TopY, LeftFormat)
                            Else
                                g.DrawString(.Item("ITEMNAME").ToString(), fontRegular, BlackBrush, c2, TopY, LeftFormat)
                            End If
                            g.DrawString(IIf(Val(.Item("QTY").ToString()) <> 0, Val(.Item("QTY").ToString()), ""), fontRegular, BlackBrush, c3, TopY, RightFormat)
                            g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontRegular, BlackBrush, c4, TopY, RightFormat)
                            g.DrawString(IIf(Val(.Item("LESSWT").ToString()) <> 0, .Item("LESSWT").ToString(), ""), fontRegular, BlackBrush, c5, TopY, RightFormat)
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
                            g.DrawString(IIf(.Item("MCHARGE").ToString() <> "0.00", .Item("MCHARGE").ToString(), ""), fontRegular, BlackBrush, c8, TopY, RightFormat)
                            If .Item("COLHEAD").ToString = "G" Or .Item("COLHEAD").ToString = "P" Then
                                If .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString <> "10" Then
                                    g.DrawString(.Item("AMOUNT").ToString(), fontRegular, BlackBrush, c9, TopY, rAlign)

                                ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString <> "3.50") Or (.Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10") Then
                                    g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c9, TopY, rAlign)
                                ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString = "3.50") Then
                                    g.DrawString(.Item("AMOUNT").ToString(), fontRegular, BlackBrush, c9, TopY, rAlign)

                                ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10" Then
                                    g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c9, TopY, rAlign)
                                End If
                            Else
                                g.DrawString(.Item("AMOUNT").ToString(), fontRegular, BlackBrush, c9, TopY, rAlign)
                            End If

                            ''BOTTOM  '/************************************////
                            'g.DrawString(.Item("SRNO").ToString(), fontRegular, BlackBrush, c1, BottomY, LeftFormat)
                            If (.Item("TYPE").ToString = "10" And .Item("COLHEAD").ToString = "G") Then
                                'g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c3, BottomY, LeftFormat)
                            Else
                                'g.DrawString(.Item("ITEMNAME").ToString(), fontRegular, BlackBrush, c2, BottomY, LeftFormat)
                            End If
                            'g.DrawString(IIf(Val(.Item("QTY").ToString()) <> 0, .Item("QTY").ToString(), ""), fontRegular, BlackBrush, c3, BottomY, RightFormat)
                            'g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontRegular, BlackBrush, c4, BottomY, RightFormat)
                            'g.DrawString(IIf(Val(.Item("LESSWT").ToString()) <> 0, .Item("LESSWT").ToString(), ""), fontRegular, BlackBrush, c5, BottomY, RightFormat)
                            'g.DrawString(IIf(Val(.Item("WASTAGE").ToString()) <> 0, .Item("WASTAGE").ToString(), ""), fontRegular, BlackBrush, c6, BottomY, RightFormat)
                            If .Item("COLHEAD").ToString = "G" Or .Item("COLHEAD").ToString = "P" Then
                                If .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString <> "10" Then
                                    'g.DrawString(.Item("RATE").ToString(), fontRegular, BlackBrush, 550, BottomY, LeftFormat)
                                ElseIf .Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString <> "3.50" Then
                                    'g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, BottomY, LeftFormat)
                                ElseIf .Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString = "3.50" Then
                                    'g.DrawString(.Item("RATE").ToString(), fontRegular, BlackBrush, 550, BottomY, LeftFormat)
                                ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10" Then
                                    'g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, BottomY, LeftFormat)
                                End If
                            Else
                                'g.DrawString(.Item("RATE").ToString(), fontRegular, BlackBrush, c7, BottomY, RightFormat)
                            End If
                            'g.DrawString(IIf(.Item("MCHARGE").ToString() <> "0.00", .Item("MCHARGE").ToString(), ""), fontRegular, BlackBrush, c8, BottomY, RightFormat)
                            If .Item("COLHEAD").ToString = "G" Or .Item("COLHEAD").ToString = "P" Then
                                If .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString <> "10" Then
                                    'g.DrawString(.Item("AMOUNT").ToString(), fontRegular, BlackBrush, c9, BottomY, rAlign)
                                ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString <> "3.50") Or (.Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10") Then
                                    'g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c9, BottomY, rAlign)
                                ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString = "3.50") Then
                                    'g.DrawString(.Item("AMOUNT").ToString(), fontRegular, BlackBrush, c9, BottomY, rAlign)
                                ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10" Then
                                    'g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c9, BottomY, rAlign)
                                End If
                            Else
                                'g.DrawString(.Item("AMOUNT").ToString(), fontRegular, BlackBrush, c9, BottomY, rAlign)
                            End If

                            If (.Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" And Val(.Item("QTY").ToString) >= 2) _
                            Or (.Item("TYPE").ToString = "2" And .Item("COLHEAD").ToString = "G") _
                            Or (.Item("TYPE").ToString = "3" And .Item("COLHEAD").ToString = "G") Then
                                'TopY = TopY + 2
                                'BottomY = BottomY + 2
                                g.DrawLine(Pens.Silver, 340, (TopY + 2), c9, (TopY + 2))
                                'g.DrawLine(Pens.Silver, 340, (BottomY + 2), c9, (BottomY + 2))
                                'TopY = TopY - 2
                                'BottomY = BottomY - 2
                            End If
                            If TopY >= 489 Then
                                PagecountSale = PagecountSale + 1
                                TopY = 135 ' TOP AGAIN STARTING POSITION
                                'BottomY = 705 ' BOTTOM AGAIN STATRING POSITION
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, 513)
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, 1075)
                                e.HasMorePages = True
                                Exit For
                            End If
                            'New Line Start
                            TopY = TopY + 17
                            'BottomY = BottomY + 17

                            If TopY >= 489 Then
                                PagecountSale = PagecountSale + 1
                                TopY = 135 ' TOP AGAIN STARTING POSITION
                                'BottomY = 705 ' BOTTOM AGAIN STATRING POSITION
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, 513)
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, 1075)
                                e.HasMorePages = True
                                Exit For
                            End If
                            If (.Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" And Val(.Item("QTY").ToString) >= 2) Or _
                            (.Item("TYPE").ToString = "2" And .Item("COLHEAD").ToString = "G") Or _
                            (.Item("TYPE").ToString = "3" And .Item("COLHEAD").ToString = "G") Then
                                'TopY = TopY - 2
                                'BottomY = BottomY - 2
                                g.DrawLine(Pens.Silver, 340, (TopY - 2), c9, (TopY - 2))
                                'g.DrawLine(Pens.Silver, 340, (BottomY - 2), c9, (BottomY - 2))
                                'TopY = TopY + 2
                                'BottomY = BottomY + 2
                            End If
                            If (.Item("RESULT").ToString = "3.10" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G") Then
                                'TopY = TopY - 2
                                'BottomY = BottomY - 2
                                g.DrawLine(Pens.Silver, 710, (TopY - 2), c9, (TopY - 2))
                                'g.DrawLine(Pens.Silver, 710, (BottomY - 2), c9, (BottomY - 2))
                                'TopY = TopY + 2
                                'BottomY = BottomY + 2
                            End If
                            If (.Item("TYPE").ToString = "4" And .Item("COLHEAD").ToString = "T") Then
                                If Val(.Item("AMOUNT").ToString) <> 0 Then
                                    g.DrawLine(Pens.Silver, c8, TopY, c9, TopY)
                                    'g.DrawLine(Pens.Silver, c8, BottomY, c9, BottomY)
                                End If
                            End If
                            If .Item("RESULT").ToString = "3.0" And .Item("TYPE").ToString = "11.0" And .Item("COLHEAD").ToString = "G" Then
                                'MISC ISSUE TYPE 11
                                'TopY = TopY - 2
                                'BottomY = BottomY - 2
                                g.DrawLine(Pens.Silver, 340, (TopY - 2), c9, (TopY - 2))
                                'g.DrawLine(Pens.Silver, 340, (BottomY - 2), c9, (BottomY - 2))
                                'TopY = TopY + 2
                                'BottomY = BottomY + 2
                            End If
                            'If NoofPage > 14 Then
                            If TopY >= 489 Then
                                PagecountSale = PagecountSale + 1
                                TopY = 135 ' TOP AGAIN STARTING POSITION
                                'BottomY = 705 ' BOTTOM AGAIN STATRING POSITION
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
            'BottomY = 705 ' BOTTOM AGAIN STATRING POSITION
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
            strsql = vbCrLf + " SELECT *,NULL ISSNO ,IDENTITY(INT,1,1) PGNO "
            strsql += vbCrLf + " INTO TEMPTABLEDB..[TEMP" & SystemName & "SALEINVOICE] "
            strsql += vbCrLf + " FROM ( "
            strsql += vbCrLf + " SELECT ROW_NUMBER() OVER(ORDER BY TRANNO) AS SRNO"
            strsql += vbCrLf + " ,TRANNO"
            strsql += vbCrLf + " ,CONVERT(VARCHAR(15), I.TRANDATE,103) AS BILLDATE"
            strsql += vbCrLf + " ,BATCHNO AS BATCHNO_N"
            strsql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) AS ITEMNAME"
            strsql += vbCrLf + " ,SUM(PCS) AS QTY"
            strsql += vbCrLf + " ,SUM(GRSWT) AS GRSWT"
            strsql += vbCrLf + " ,SUM(NETWT) AS NETWT"
            strsql += vbCrLf + " ,NULL AS LESSWT"
            strsql += vbCrLf + " ,NULL AS WASTAGE"
            strsql += vbCrLf + " ,NULL AS RATE"
            strsql += vbCrLf + " ,NULL MCHARGE"
            strsql += vbCrLf + " ,NULL TAX"
            strsql += vbCrLf + " ,NULL SNO"
            strsql += vbCrLf + " ,NULL AS AMOUNT"
            strsql += vbCrLf + " /*,SUM(AMOUNT) AS AMOUNT*/"
            strsql += vbCrLf + " ,1.0 AS RESULT , 11.0 AS TYPE, '' AS COLHEAD"
            strsql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            strsql += vbCrLf + " WHERE 1 = 1 AND "
            strsql += vbCrLf + " TRANTYPE = 'MI' "
            strsql += vbCrLf + " AND TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & pBatchno & "'"
            strsql += vbCrLf + " GROUP BY CATCODE,TRANNO,BATCHNO,I.TRANDATE "
            strsql += vbCrLf + "  /*ISSUE STONE MISCISSUE TOTAL*/"
            strsql += vbCrLf + "  UNION ALL "
            strsql += vbCrLf + " SELECT "
            strsql += vbCrLf + " NULL AS SRNO, "
            strsql += vbCrLf + " NULL TRANNO,"
            strsql += vbCrLf + " NULL AS BILLDATE,"
            strsql += vbCrLf + " BATCHNO AS BATCHNO_N,"
            strsql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = ISS.STNITEMID) AS  ITEMNAME"
            strsql += vbCrLf + " ,SUM(ISS.STNPCS) AS QTY,SUM(ISS.STNWT)GRSWT,NULL NETWT"
            strsql += vbCrLf + " ,NULL LESSWT,NULL WASTAGE"
            strsql += vbCrLf + " ,CONVERT(VARCHAR,NULL)RATE,CONVERT(VARCHAR, NULL) MCHARGE"
            strsql += vbCrLf + " ,NULL TAX,NULL AMOUNT,NULL SNO,2.0 AS RESULT,11.0 AS TYPE,'G' AS COLHEAD"
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
            strsql += vbCrLf + "  BATCHNO AS BATCHNO_N,"
            strsql += vbCrLf + "  'ISSUED FOR REPAIR AND POLISHING ' ITEMNAME"
            strsql += vbCrLf + "  ,NULL AS QTY,NULL GRSWT,NULL NETWT"
            strsql += vbCrLf + "  ,NULL LESSWT,NULL WASTAGE"
            strsql += vbCrLf + "  ,CONVERT(VARCHAR,NULL)RATE,CONVERT(VARCHAR, NULL) MCHARGE"
            strsql += vbCrLf + "  ,NULL TAX,NULL AMOUNT,NULL SNO,3 AS RESULT,11.0 AS TYPE"
            strsql += vbCrLf + "  ,'R' AS COLHEAD"
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
            strsql += vbCrLf + "  NULL AS BILLDATE,"
            strsql += vbCrLf + "  BATCHNO AS BATCHNO_N,"
            strsql += vbCrLf + "  'TOTAL : ' ITEMNAME"
            strsql += vbCrLf + "  ,SUM(I.PCS) AS QTY,SUM(I.GRSWT)GRSWT,SUM(I.NETWT)NETWT"
            strsql += vbCrLf + "  ,NULL LESSWT,NULL WASTAGE"
            strsql += vbCrLf + "  ,CONVERT(VARCHAR,NULL)RATE,CONVERT(VARCHAR, NULL) MCHARGE"
            strsql += vbCrLf + "  ,NULL TAX,NULL AMOUNT,NULL SNO,3 AS RESULT,11.0 AS TYPE,'G' AS COLHEAD"
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
                cmd = New OleDbCommand(strsql, cn)
                cmd.ExecuteNonQuery()
            Else
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
                strsql += vbCrLf + "  'TOTAL' ITEMNAME"
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
                strsql += vbCrLf + "  '' ITEMNAME"
                strsql += vbCrLf + "  ,NULL AS QTY,NULL GRSWT,NULL NETWT,NULL LESSWT,NULL WASTAGE"
                strsql += vbCrLf + "  ,CONVERT(VARCHAR,'DISCOUNT')RATE,CONVERT(VARCHAR, NULL) MCHARGE"
                strsql += vbCrLf + "  ,'DISCOUNT',SUM(I.FIN_DISCOUNT+I.DISCOUNT)AMOUNT,'ZZZZZZZZ' SNO,3.1 AS RESULT,1 AS TYPE,'G' AS COLHEAD"
                strsql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS I "
                strsql += vbCrLf + "  WHERE I.BATCHNO='" & pBatchno & "' AND I.TRANTYPE IN ('SA','OD','RD') AND (I.FIN_DISCOUNT + I.DISCOUNT) > 0"
                strsql += vbCrLf + "  GROUP BY I.TRANDATE,I.BATCHNO "
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
                    strsql += vbCrLf + "  ,'AMOUNT' TAX,SUM(I.AMOUNT)AMOUNT,'ZZZZZZZZ' SNO"
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
                strsql += vbCrLf + "  '' ITEMNAME"
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
                strsql += vbCrLf + "  'TOTAL' ITEMNAME"
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
                strsql += vbCrLf + "  ,'TAX',SUM(I.TAX) AMOUNT"
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
                strsql += vbCrLf + "  ,CONVERT(VARCHAR,NULL)RATE,CONVERT(VARCHAR,NULL) AS MCHARGE,'AMOUNT' TAX"
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
                strsql += vbCrLf + "  'TOTAL' ITEMNAME"
                strsql += vbCrLf + "  ,SUM(I.PCS) AS QTY,SUM(I.GRSWT)GRSWT,SUM(I.NETWT)NETWT,SUM(I.LESSWT) LESSWT"
                strsql += vbCrLf + "  ,NULL AS WASTAGE"
                strsql += vbCrLf + "  ,CONVERT(VARCHAR,NULL)RATE,CONVERT(VARCHAR, NULL) MCHARGE,"
                strsql += vbCrLf + "  SUM(I.TAX)TAX,(ISNULL(SUM(I.TAX),0)+ISNULL(SUM(I.AMOUNT),0)) AMOUNT,'ZZZZZZZZ' SNO,3.0 AS RESULT,3 AS TYPE,'G' AS COLHEAD"
                strsql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS I "
                strsql += vbCrLf + "  WHERE I.BATCHNO='" & pBatchno & "' AND I.TRANTYPE='SR' "
                strsql += vbCrLf + "  GROUP BY I.TRANDATE,I.BATCHNO HAVING COUNT(*)>1"

                strsqlCheck = ""
                strsqlCheck = vbCrLf + "SELECT  ISNULL(SUM(IAMT),0) IAMT, ISNULL(SUM(RAMT),0)RAMT, SUM(IAMT)-SUM(RAMT) AS FINAMT FROM("
                strsqlCheck += vbCrLf + "(SELECT SUM(AMOUNT + TAX) IAMT, 0 RAMT FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = '" & pBatchno & "') UNION ALL"
                strsqlCheck += vbCrLf + "(SELECT 0 IAMT , SUM(AMOUNT + TAX) RMT   FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO = '" & pBatchno & "')"
                strsqlCheck += vbCrLf + ")A"
                Dim ro1 As DataRow = GetSqlRow(strsqlCheck, cn)
                If Val(ro.Item("IAMT").ToString) > 0 And Val(ro.Item("RAMT").ToString) > 0 Then
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
                strsql += vbCrLf + "  ,CONVERT(VARCHAR,NULL) AS MCHARGE,'TAX' "
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
                strsql += vbCrLf + "  ,CONVERT(VARCHAR,NULL) AS MCHARGE,'TAX'"
                strsql += vbCrLf + "  ,SUM(CASE WHEN TRANMODE='C' THEN AMOUNT ELSE -1* AMOUNT END ) AMOUNT"
                strsql += vbCrLf + "  ,NULL SNO,3.1 AS RESULT,9 AS TYPE,'' AS COLHEAD"
                strsql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN AS I "
                strsql += vbCrLf + "  WHERE I.BATCHNO='" & pBatchno & "' AND ACCODE IN('TCS','HANDC') AND PAYMODE IN('SE','HC') "
                strsql += vbCrLf + "  GROUP BY I.TRANDATE,I.BATCHNO,PAYMODE "
                'Scheme
                strsql += vbCrLf + " UNION ALL"
                strsql += vbCrLf + " SELECT NULL SRNO ,NULL AS TRANNO ,NULL AS BILLDATE , BATCHNO AS BATCHNO_N ,"
                strsql += vbCrLf + " CHQCARDREF + '-' + CHQCARDNO AS ITEMNAME,"
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
        strsql = "SELECT TRANNO FROM TEMPTABLEDB..[TEMP" & SystemName & "SALEINVOICE] WHERE TRANNO IS NOT NULL"
        Tranno = GetSqlValue_Bill(strsql, "TRANNO")

        strsql = "SELECT CONVERT(VARCHAR(15),BILLDATE,103) AS BILLDATE FROM TEMPTABLEDB..[TEMP" & SystemName & "SALEINVOICE] WHERE TRANNO IS NOT NULL"
        prtBilldate = GetSqlValue_Bill(strsql, "BILLDATE")

        strsql = "SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = '" & strBilldate & "' AND BATCHNO = '" & pBatchno & "' "
        NodeId = GetSqlValue_Bill(strsql, "SYSTEMID")

        strsql = "UPDATE TEMPTABLEDB..[TEMP" & SystemName & "SALEINVOICE] SET TRANNO='" & Tranno & "' WHERE TRANNO IS  NULL"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
        strsql = "ALTER TABLE TEMPTABLEDB..[TEMP" & SystemName & "SALEINVOICE] ADD PNAME VARCHAR(200),ADD1 VARCHAR(800), ADD2 VARCHAR(800),ADD3 VARCHAR(800),AREA VARCHAR(800),MOBILE VARCHAR(800),PINCODE VARCHAR(800)"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
        CustomerInfo()

        If dtCustInfo.Rows.Count > 0 Then
            strsql = "UPDATE TEMPTABLEDB..[TEMP" & SystemName & "SALEINVOICE] "
            strsql += vbCrLf + " SET PNAME='" & dtCustInfo.Rows(0)("NAME").ToString & "'"
            strsql += vbCrLf + " ,ADD1='" & dtCustInfo.Rows(0)("ADD1").ToString & "'"
            strsql += vbCrLf + " ,ADD2='" & dtCustInfo.Rows(0)("ADD2").ToString & "'"
            strsql += vbCrLf + " ,ADD3='" & dtCustInfo.Rows(0)("ADD3").ToString & "'"
            strsql += vbCrLf + " ,MOBILE='" & dtCustInfo.Rows(0)("PHONE").ToString & "'"
            strsql += vbCrLf + " ,PINCODE='" & dtCustInfo.Rows(0)("PINCODE").ToString & "'"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        End If

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
            For j As Integer = 0 To dtPrint.Rows.Count - 1
                If dtPrint.Rows(j).Item("TRANTYPE").ToString = "SA" Then
                    strsql = "SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "SALEINVOICE] ORDER BY PGNO "
                    da = New OleDbDataAdapter(strsql, cn)
                    dtSales = New dsBillPrint.dtSalesDataTable
                    da.Fill(dtSales)
printSA:
                    PrintDocument2.Print()
                    If k <> dtSales.Rows.Count - 1 Then
                        GoTo printSA
                    End If
                    k = 0
                End If
                If dtPrint.Rows(j).Item("TRANTYPE").ToString = "SR" Then
                    strsql = "SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "RETURNINVOICE] ORDER BY PGNO "
                    da = New OleDbDataAdapter(strsql, cn)
                    dtSales = New dsBillPrint.dtSalesDataTable
                    da.Fill(dtSales)
printSR:
                    PrintDocument2.Print()
                    If k <> dtSales.Rows.Count - 1 Then
                        GoTo printSR
                    End If
                    k = 0
                End If
                If dtPrint.Rows(j).Item("TRANTYPE").ToString = "PU" Then
                    strsql = "SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "PURCHASEINVOICE] ORDER BY PGNO "
                    da = New OleDbDataAdapter(strsql, cn)
                    dtSales = New dsBillPrint.dtSalesDataTable
                    da.Fill(dtSales)
printPU:
                    PrintDocument2.Print()
                    If k <> dtSales.Rows.Count - 1 Then
                        GoTo printPU
                    End If
                    k = 0
                End If
            Next
        End If

    End Sub
    Public Sub PrintDocument2_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument2.PrintPage
        Using g As Graphics = e.Graphics
newPage:
            If k = dtSales.Rows.Count - 1 Then
                Printedline = 0
                e.HasMorePages = False
                Exit Sub
            End If
            Dim totAmount As Double = 0
            Dim totTax As Double = 0
            Dim totSGST As Double = 0
            Dim totCGST As Double = 0
            Dim totIGST As Double = 0
            Dim totQty As Double = 0
            Dim totGrsWt As Double = 0
            Dim totNetWt As Double = 0
            Dim totWast As Double = 0
            Dim totMCharge As Double = 0
            Dim totDiscount As Double = 0
            For Each drk As DataRow In dtSales.Rows
                If drk.Item("RESULT").ToString = "1.0" Or drk.Item("RESULT").ToString = "1" Then
                    totAmount += Val(drk.Item("AMOUNT").ToString)
                    totTax += Val(drk.Item("TAX").ToString)
                    totSGST += Val(drk.Item("SGST").ToString)
                    totCGST += Val(drk.Item("CGST").ToString)
                    totIGST += Val(drk.Item("IGST").ToString)
                    totQty += Val(drk.Item("QTY").ToString)
                    totGrsWt += Val(drk.Item("GRSWT").ToString)
                    totNetWt += Val(drk.Item("NETWT").ToString)
                    totWast += Val(drk.Item("WASTAGE").ToString)
                    totMCharge += Val(drk.Item("MCHARGE").ToString)
                    totDiscount += 0
                End If
            Next
            Dim brush As New SolidBrush(Color.Black)
            Dim pen As New Pen(brush)
            Dim fontRegular As New Font("Times New Roman", 9, FontStyle.Regular)
            Dim fontBold As New Font("Times New Roman", 9, FontStyle.Bold)
            Dim xlines As Double = e.MarginBounds.Left - 100
            Dim ylines As Double = e.MarginBounds.Top
            Dim lineheight As Double = fontRegular.GetHeight(e.Graphics) + 5
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
            Dim dtTemp1 As New DataTable
            Dim dr As DataRow
            Dim frmt As New StringFormat
            frmt.Alignment = StringAlignment.Far
            Me.Width = 1400
            c1 = 10
            c2 = 80
            c3 = 90
            c4 = 140
            c5 = 190
            c6 = 270
            '1st Left Line
            g.DrawLine(Pens.Black, 50.0F, 120.0F, 50.0F, 1035.0F)
            'Last right Line
            g.DrawLine(Pens.Black, 800.0F, 120.0F, 800.0F, 1035.0F)
            'Top Line
            g.DrawLine(Pens.Black, 50.0F, 120.0F, 800.0F, 120.0F)
            'Bottom Line
            g.DrawLine(Pens.Black, 50.0F, 1035.0F, 800.0F, 1035.0F)
            g.DrawString("Goverment of India/State", fontBold, brush, 350, 125)
            g.DrawLine(Pens.Black, 50.0F, 140.0F, 800.0F, 140.0F)
            g.DrawString("Department of................................", fontBold, brush, 350, 145)
            g.DrawLine(Pens.Black, 50.0F, 160.0F, 800.0F, 160.0F)
            g.DrawString("Form GST INV - 1 ", fontBold, brush, 350, 165)
            g.DrawLine(Pens.Black, 50.0F, 180.0F, 800.0F, 180.0F)
            g.DrawString("(See Rule ---)", fontBold, brush, 350, 185)
            g.DrawLine(Pens.Black, 50.0F, 200.0F, 800.0F, 200.0F)
            g.DrawString("Application for Electronic Reference Number of an Invoice", fontBold, brush, 270, 205)
            g.DrawLine(Pens.Black, 50.0F, 220.0F, 800.0F, 220.0F)
            g.DrawString("1. GSTIN", fontRegular, brush, 50, 225)
            g.DrawString(dtCompany.Rows(0)("GSTNO").ToString, fontRegular, brush, 190, 225)
            g.DrawLine(Pens.Black, 50.0F, 240.0F, 800.0F, 240.0F)
            g.DrawString("2. Name", fontRegular, brush, 50, 245)
            g.DrawString(dtCompany.Rows(0)("COMPANYNAME").ToString, fontRegular, brush, 190, 245)
            g.DrawLine(Pens.Black, 50.0F, 260.0F, 800.0F, 260.0F)
            g.DrawString("3.Address", fontRegular, brush, 50, 265)
            g.DrawString(dtCompany.Rows(0)("ADDRESS1").ToString & " " & dtCompany.Rows(0)("ADDRESS2").ToString, fontRegular, brush, 190, 265)
            g.DrawLine(Pens.Black, 50.0F, 280.0F, 800.0F, 280.0F)
            g.DrawString("4.Serial No. of Invoice ", fontRegular, brush, 50, 285)
            dr = dtSales.Select(String.Format("RESULT=1.0", "RESULT"))(0)
            g.DrawString(dr("TRANNO").ToString, fontRegular, brush, 190, 285)
            g.DrawLine(Pens.Black, 50.0F, 300.0F, 800.0F, 300.0F)

            If dtCustInfo.Rows.Count > 0 Then
                g.DrawLine(Pens.Black, 450.0F, 300.0F, 450.0F, 825.0F)

                g.DrawString("Details of Receiver (Billed to)", fontBold, brush, 50, 305)
                g.DrawString("Name", fontRegular, brush, 50, 320)
                g.DrawString(dtCustInfo.Rows(0)("NAME").ToString, fontRegular, brush, 190, 320)
                g.DrawString("Address", fontRegular, brush, 50, 335)
                g.DrawString(dtCustInfo.Rows(0)("ADD1").ToString, fontRegular, brush, 190, 335)
                g.DrawString("State", fontRegular, brush, 50, 350)
                g.DrawString(dtCustInfo.Rows(0)("ADD2").ToString, fontRegular, brush, 190, 350)
                g.DrawString("State Code", fontRegular, brush, 50, 365)
                g.DrawString(dtCustInfo.Rows(0)("ADD3").ToString, fontRegular, brush, 190, 365)
                g.DrawString("GSTIN/Unique ID", fontRegular, brush, 50, 380)
                g.DrawString(dtCustInfo.Rows(0)("PHONE").ToString, fontRegular, brush, 190, 380)

                g.DrawString("Details of consignee (Shipped to)", fontBold, brush, 450, 305)
                g.DrawString("Name", fontRegular, brush, 450, 320)
                g.DrawString(dtCustInfo.Rows(0)("NAME").ToString, fontRegular, brush, 580, 320)
                g.DrawString("Address", fontRegular, brush, 450, 335)
                g.DrawString(dtCustInfo.Rows(0)("ADD1").ToString, fontRegular, brush, 580, 335)
                g.DrawString("State", fontRegular, brush, 450, 350)
                g.DrawString(dtCustInfo.Rows(0)("ADD2").ToString, fontRegular, brush, 580, 350)
                g.DrawString("State Code", fontRegular, brush, 450, 365)
                g.DrawString(dtCustInfo.Rows(0)("ADD3").ToString, fontRegular, brush, 580, 365)
                g.DrawString("GSTIN/Unique ID", fontRegular, brush, 450, 380)
                g.DrawString(dtCustInfo.Rows(0)("PHONE").ToString, fontRegular, brush, 580, 380)
                g.DrawLine(Pens.Black, 50.0F, 395.0F, 800.0F, 395.0F)
            Else
                g.DrawLine(Pens.Black, 450.0F, 300.0F, 450.0F, 825.0F)
                g.DrawString("Details of consignee (Shipped to)", fontBold, brush, 450, 305)
                g.DrawString("Name", fontRegular, brush, 450, 320)
                g.DrawString("", fontRegular, brush, 580, 320)
                g.DrawString("Address", fontRegular, brush, 450, 335)
                g.DrawString("", fontRegular, brush, 580, 335)
                g.DrawString("State", fontRegular, brush, 450, 350)
                g.DrawString("", fontRegular, brush, 580, 350)
                g.DrawString("State Code", fontRegular, brush, 450, 365)
                g.DrawString("", fontRegular, brush, 580, 365)
                g.DrawString("GSTIN/Unique ID", fontRegular, brush, 450, 380)
                g.DrawString("", fontRegular, brush, 580, 380)
                g.DrawLine(Pens.Black, 50.0F, 395.0F, 800.0F, 395.0F)
            End If
            g.DrawString("Sr.No", fontBold, brush, 50, 400)
            g.DrawLine(Pens.Black, 90.0F, 395.0F, 90.0F, 985.0F)
            g.DrawString("Description of", fontBold, brush, 100, 395)
            g.DrawString("Goods", fontBold, brush, 100, 410)
            g.DrawLine(Pens.Black, 230.0F, 395.0F, 230.0F, 725.0F)
            g.DrawString("HSN", fontBold, brush, 230, 400)
            g.DrawLine(Pens.Black, 280.0F, 395.0F, 280.0F, 725.0F)
            g.DrawString("Qty.", fontBold, brush, 280, 400)
            g.DrawLine(Pens.Black, 340.0F, 395.0F, 340.0F, 725.0F)
            g.DrawString("Unit", fontBold, brush, 340, 400)
            g.DrawLine(Pens.Black, 390.0F, 395.0F, 390.0F, 725.0F)
            g.DrawString("Rate(Per", fontBold, brush, 390, 395)
            g.DrawString("item)", fontBold, brush, 390, 410)

            g.DrawString("Value", fontBold, brush, 450, 395)
            g.DrawString("Added", fontBold, brush, 450, 410)
            g.DrawLine(Pens.Black, 530.0F, 395.0F, 530.0F, 825.0F)
            g.DrawString("Max", fontBold, brush, 530, 400)
            g.DrawLine(Pens.Black, 590.0F, 395.0F, 590.0F, 825.0F)
            g.DrawString("Total", fontBold, brush, 590, 400)
            g.DrawLine(Pens.Black, 650.0F, 395.0F, 650.0F, 825.0F)
            g.DrawString("Discount", fontBold, brush, 650, 400)
            g.DrawLine(Pens.Black, 710.0F, 395.0F, 710.0F, 825.0F)
            g.DrawString("Taxable value", fontBold, brush, 710, 400)
            g.DrawLine(Pens.Black, 50.0F, 425.0F, 800.0F, 425.0F)
            g.DrawLine(Pens.Black, 90.0F, 725.0F, 800.0F, 725.0F)

            g.DrawString("Freight", fontRegular, brush, 90, 730)
            g.DrawString("", fontRegular, brush, 280, 730)
            g.DrawLine(Pens.Black, 90.0F, 745.0F, 800.0F, 745.0F)
            g.DrawString("Insurance", fontRegular, brush, 90, 750)
            g.DrawString("", fontRegular, brush, 280, 750)
            g.DrawLine(Pens.Black, 90.0F, 765.0F, 800.0F, 765.0F)
            g.DrawString("Packing and Forwarding Charges", fontRegular, brush, 90, 770)
            g.DrawString("", fontRegular, brush, 280, 770)
            g.DrawLine(Pens.Black, 90.0F, 785.0F, 800.0F, 785.0F)

            g.DrawLine(Pens.Black, 230.0F, 785.0F, 230.0F, 845.0F)
            'g.DrawLine(Pens.Black, 450.0F, 805.0F, 800.0F, 805.0F)
            g.DrawLine(Pens.Black, 230.0F, 825.0F, 800.0F, 825.0F)
            g.DrawLine(Pens.Black, 280.0F, 785.0F, 280.0F, 825.0F)
            g.DrawLine(Pens.Black, 340.0F, 785.0F, 340.0F, 825.0F)
            g.DrawLine(Pens.Black, 390.0F, 785.0F, 390.0F, 825.0F)
            g.DrawString("Total", fontBold, brush, 230, 800)
            g.DrawString(totQty, fontBold, brush, 340, 800, frmt)
            g.DrawString(Format(totGrsWt, "0.000"), fontBold, brush, 390, 800, frmt)
            g.DrawString(Format(totWast, "0.000"), fontBold, brush, 530, 800, frmt)
            g.DrawString(Format(totMCharge, "0.00"), fontBold, brush, 590, 800, frmt)
            g.DrawString(Format(totAmount, "0.00"), fontBold, brush, 650, 800, frmt)
            g.DrawString(Format(totDiscount, "0.00"), fontBold, brush, 710, 800, frmt)
            g.DrawString(Format(totAmount, "0.00"), fontBold, brush, 800, 800, frmt)

            g.DrawLine(Pens.Black, 90.0F, 845.0F, 800.0F, 845.0F)

            g.DrawString("CGST Rate / AMT.", fontBold, brush, 90, 850)
            e.Graphics.DrawString(Format(totCGST, "0.00"), fontBold, brush, 800, 850, frmt)
            g.DrawLine(Pens.Black, 90.0F, 865.0F, 800.0F, 865.0F)

            g.DrawString("SGST Rate / AMT.", fontBold, brush, 90, 870)
            e.Graphics.DrawString(Format(totSGST, "0.00"), fontBold, brush, 800, 870, frmt)
            g.DrawLine(Pens.Black, 90.0F, 885.0F, 800.0F, 885.0F)

            g.DrawString("IGST Rate / AMT.", fontBold, brush, 90, 890)
            e.Graphics.DrawString(Format(totIGST, "0.00"), fontBold, brush, 800, 890, frmt)
            g.DrawLine(Pens.Black, 90.0F, 905.0F, 800.0F, 905.0F)

            g.DrawString("Total Invoice Value (In figure)", fontBold, brush, 90, 910)
            g.DrawString(Format(totAmount, "0.00"), fontBold, brush, 800, 910, frmt)
            g.DrawLine(Pens.Black, 90.0F, 925.0F, 800.0F, 925.0F)
            g.DrawString("Total Invoice Value (In Words)", fontBold, brush, 90, 930)
            g.DrawString(ConvertRupees.RupeesToWord(totAmount), fontRegular, brush, 270, 930)
            g.DrawLine(Pens.Black, 90.0F, 945.0F, 800.0F, 945.0F)
            g.DrawString("Amount of Tax subject to Reverse Charges", fontBold, brush, 90, 950)
            g.DrawString(Format(totTax, "0.00"), fontBold, brush, 800, 950, frmt)
            g.DrawLine(Pens.Black, 90.0F, 965.0F, 800.0F, 965.0F)
            g.DrawLine(Pens.Black, 50.0F, 985.0F, 800.0F, 985.0F)

            g.DrawLine(Pens.Black, 230.0F, 965.0F, 230.0F, 985.0F)
            g.DrawLine(Pens.Black, 280.0F, 965.0F, 280.0F, 985.0F)
            g.DrawLine(Pens.Black, 340.0F, 965.0F, 340.0F, 985.0F)
            g.DrawLine(Pens.Black, 390.0F, 965.0F, 390.0F, 1035.0F)
            g.DrawLine(Pens.Black, 450.0F, 965.0F, 450.0F, 985.0F)
            g.DrawLine(Pens.Black, 530.0F, 965.0F, 530.0F, 985.0F)
            g.DrawLine(Pens.Black, 590.0F, 965.0F, 590.0F, 985.0F)
            g.DrawLine(Pens.Black, 650.0F, 965.0F, 650.0F, 985.0F)
            g.DrawLine(Pens.Black, 710.0F, 845.0F, 710.0F, 985.0F)

            g.DrawString("Declaration:", fontRegular, brush, 50, 988)
            g.DrawString("", fontRegular, brush, 230, 988)
            g.DrawString("Signatory", fontRegular, brush, 50, 1003)
            g.DrawString("", fontRegular, brush, 230, 1003)
            g.DrawString("Electronic Reference Number", fontRegular, brush, 50, 1018)
            g.DrawString("", fontRegular, brush, 230, 1018)

            g.DrawString("Signature", fontRegular, brush, 390, 988)
            g.DrawString("", fontRegular, brush, 590, 988)
            g.DrawString("Name of the Designation/Status", fontRegular, brush, 390, 1003)
            g.DrawString("", fontRegular, brush, 590, 1003)
            g.DrawString("Date", fontRegular, brush, 390, 1018)
            g.DrawString("", fontRegular, brush, 590, 1018)

            Dim len As Integer = 0
            Dim newPrint As Boolean = False

            For i As Integer = 0 To dtSales.Rows.Count - 1
                'If i = 0 Then If k > 0 Then i = k + 1 Else i = k
                If i = 0 Then i = k
                If i = 13 Then i = i
                If linePerpage = Printedline Then
                    If k = dtSales.Rows.Count - 1 Then
                        Printedline = 0
                        e.HasMorePages = False
                        Exit Sub
                    Else
                        Printedline = 0
                        e.HasMorePages = True
                        Exit Sub
                    End If
                End If
                'If i = 0 Then i = k
                'If (ylines + lineheight > e.MarginBounds.Bottom) Then
                '    e.HasMorePages = True
                '    Return
                'End If
                Dim Grswt As String = ""
                Dim Rate As String = ""
                Dim Qty As String = ""
                Dim Wastage As String = ""
                Dim Mcharge As String = ""
                Dim Amount As String = ""
                Dim Desc As String = ""
                Dim HSNCode As String = ""
                If dtSales.Rows(i).Item("COLHEAD").ToString() = "" Then
                    Qty = Format(Val(dtSales.Rows(i).Item("QTY").ToString()), "0")
                    Grswt = Format(Val(dtSales.Rows(i).Item("GRSWT").ToString()), "0.000")
                    Rate = Format(Val(dtSales.Rows(i).Item("RATE").ToString()), "0.00")
                    Wastage = Format(Val(dtSales.Rows(i).Item("WASTAGE").ToString()), "0.000")
                    Mcharge = Format(Val(dtSales.Rows(i).Item("MCHARGE").ToString()), "0.00")
                    Amount = Format(Val(dtSales.Rows(i).Item("AMOUNT").ToString()), "0.00")
                    Desc = dtSales.Rows(i).Item("ITEMNAME").ToString()
                    HSNCode = "****"
                ElseIf dtSales.Rows(i).Item("COLHEAD").ToString() = "S" Then
                    Desc = dtSales.Rows(i).Item("ITEMNAME").ToString()
                    HSNCode = ""
                ElseIf (dtSales.Rows(i).Item("COLHEAD").ToString() = "G" Or dtSales.Rows(i).Item("COLHEAD").ToString() = "P") Then
                    Amount = Format(Val(dtSales.Rows(i).Item("AMOUNT").ToString()), "0.00")
                    Desc = dtSales.Rows(i).Item("RATE").ToString()
                    HSNCode = ""
                End If
                Printedline += 1
                g.DrawString(dtSales.Rows(i).Item("SRNO").ToString(), fontRegular, brush, 70, 430 + len, frmt)
                g.DrawString(Desc, fontRegular, brush, 100, 430 + len)
                g.DrawString(HSNCode, fontRegular, brush, 280, 430 + len, frmt)
                g.DrawString(Qty, fontRegular, brush, 340, 430 + len, frmt)
                g.DrawString(Grswt, fontRegular, brush, 390, 430 + len, frmt)
                g.DrawString(Rate, fontRegular, brush, 450, 430 + len, frmt)
                g.DrawString(Wastage, fontRegular, brush, 530, 430 + len, frmt)
                g.DrawString(Mcharge, fontRegular, brush, 590, 430 + len, frmt)
                g.DrawString(Amount, fontRegular, brush, 650, 430 + len, frmt)
                g.DrawString("", fontRegular, brush, 710, 430 + len, frmt)
                g.DrawString(Amount, fontRegular, brush, 800, 430 + len, frmt)
                g.DrawLine(Pens.Black, 90.0F, 445.0F + len, 800.0F, 445.0F + len)
                len += 20
                ylines += lineheight
                If k = dtSales.Rows.Count - 1 Then
                    Printedline = 0
                    e.HasMorePages = False
                    Exit Sub
                End If
                k += 1
            Next
            k = dtSales.Rows.Count - 1
            e.HasMorePages = False
        End Using
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

            Dim TranNo1 As String = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("ISSUENO").ToString
            Dim TDate1 As String = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("DATE").ToString
            Dim TranType1 As String = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("TRANTYPE").ToString

            g.DrawString(Acname, fontRegular, BlackBrush, c1, TopY, LeftFormat)
            'g.DrawString(Acname, fontRegular, BlackBrush, c1, BottomY, LeftFormat)
            g.DrawString("Invoice No", fontRegular, BlackBrush, c6, TopY, LeftFormat)
            'g.DrawString("Invoice No", fontRegular, BlackBrush, c6, BottomY, LeftFormat)
            g.DrawString("  : " & TranNo1, fontRegular, BlackBrush, c7, TopY, LeftFormat)
            'g.DrawString("  : " & TranNo1, fontRegular, BlackBrush, c7, BottomY, LeftFormat)
            TopY = TopY + 20
            'BottomY = BottomY + 20
            g.DrawString(Address1, fontRegular, BlackBrush, c1, TopY, LeftFormat)
            'g.DrawString(Address1, fontRegular, BlackBrush, c1, BottomY, LeftFormat)
            g.DrawString("Date", fontRegular, BlackBrush, c6, TopY, LeftFormat)
            'g.DrawString("Date", fontRegular, BlackBrush, c6, BottomY, LeftFormat)
            g.DrawString("  : " & TDate1, fontRegular, BlackBrush, c7, TopY, LeftFormat)
            'g.DrawString("  : " & TDate1, fontRegular, BlackBrush, c7, BottomY, LeftFormat)
            TopY = TopY + 20
            'BottomY = BottomY + 20
            g.DrawString(Address2, fontRegular, BlackBrush, c1, TopY, LeftFormat)
            'g.DrawString(Address2, fontRegular, BlackBrush, c1, BottomY, LeftFormat)
            g.DrawString("PAN", fontRegular, BlackBrush, c6, TopY, LeftFormat)
            'g.DrawString("PAN", fontRegular, BlackBrush, c6, BottomY, LeftFormat)
            g.DrawString("  : AAAPP5750A ", fontRegular, BlackBrush, c7, TopY, LeftFormat)
            'g.DrawString("  : AAAPP5750A ", fontRegular, BlackBrush, c7, BottomY, LeftFormat)
            TopY = TopY + 20
            'BottomY = BottomY + 20
            g.DrawString(Pan, fontRegular, BlackBrush, c1, TopY, LeftFormat)
            'g.DrawString(Pan, fontRegular, BlackBrush, c1, BottomY, LeftFormat)
            g.DrawString(TranType1, fontBoldTitle, BlackBrush, c3, TopY, LeftFormat)
            'g.DrawString(TranType1, fontBoldTitle, BlackBrush, c3, BottomY, LeftFormat)
            TopY = TopY + 20
            'BottomY = BottomY + 20
            g.DrawLine(Pens.Silver, 20, TopY, 775, TopY)
            'g.DrawLine(Pens.Silver, 20, BottomY, 775, BottomY)
        End If
    End Sub

    Public Sub CustomerInfo()
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
        strsql = " SELECT PNAME AS NAME,ADDRESS1 + ' ' + ADDRESS2 ADD1,ADDRESS3 + ' ' + AREA ADD2,ISNULL(CITY,'')+' '+ISNULL(STATE,'') + ' ' + ISNULL(COUNTRY,'') ADD3,MOBILE PHONE,'" & pBatchno & "' BATCHNO_N,PINCODE,EMAIL FROM " & cnAdminDb & "..PERSONALINFO "
        strsql += vbCrLf + "WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = '" & pBatchno & "')"
        da = New OleDbDataAdapter(strsql, cn)
        dtCustInfo = New DataTable ' dsBillPrint.CUSTOMERINFODataTable
        da.Fill(dtCustInfo)

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
        strsql = " SELECT * FROM " & cnAdminDb & "..PERSONALINFO "
        strsql += vbCrLf + "WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = '" & pBatchno & "')"
        da = New OleDbDataAdapter(strsql, cn)
        dtCustInfo = New DataTable
        da.Fill(dtCustInfo)
        If dtCustInfo.Rows.Count > 0 Then
            Cusname = "Mr/Ms. " & Trim(dtCustInfo.Rows(0).Item("PNAME").ToString) & Trim(dtCustInfo.Rows(0).Item("INITIAL").ToString)
            Bno = Billno
            Bdate = Billdate
            g.DrawString(Cusname, fontRegular, Brushes.Black, c1, TopY)
            'g.DrawString(Cusname, fontRegular, Brushes.Black, c1, BottomY)
            Select Case Trantype
                Case "POS"
                    g.DrawString("Invoice No", fontRegular, Brushes.Black, c7, TopY)
                    'g.DrawString("Invoice No", fontRegular, Brushes.Black, c7, BottomY)
                    g.DrawString(" : " & Bno, fontRegular, Brushes.Black, c8, TopY)
                    'g.DrawString(" : " & Bno, fontRegular, Brushes.Black, c8, BottomY)
                Case "ORD"
                    g.DrawString("Order No", fontRegular, Brushes.Black, c7, TopY)
                    'g.DrawString("Order No", fontRegular, Brushes.Black, c7, BottomY)
                    g.DrawString(" : " & Bno, fontRegular, Brushes.Black, c8, TopY)
                    'g.DrawString(" : " & Bno, fontRegular, Brushes.Black, c8, BottomY)
                Case "REP"
                    g.DrawString("Repair No", fontRegular, Brushes.Black, c7, TopY)
                    'g.DrawString("Repair No", fontRegular, Brushes.Black, c7, BottomY)
                    g.DrawString(" : " & Bno, fontRegular, Brushes.Black, c8, TopY)
                    'g.DrawString(" : " & Bno, fontRegular, Brushes.Black, c8, BottomY)
            End Select
            TopY = TopY + 18
            'BottomY = BottomY + 18
            CusAddress1 = Trim(dtCustInfo.Rows(0).Item("DoorNo") & "").ToString & " " & Trim(dtCustInfo.Rows(0).Item("address1") & "").ToString & Trim(dtCustInfo.Rows(0).Item("address2") & "").ToString & " " & Trim(dtCustInfo.Rows(0).Item("area") & "").ToString
            g.DrawString(CusAddress1, fontRegular, Brushes.Black, c1, TopY)
            'g.DrawString(CusAddress1, fontRegular, Brushes.Black, c1, BottomY)
            g.DrawString("Date", fontRegular, Brushes.Black, c7, TopY)
            'g.DrawString("Date", fontRegular, Brushes.Black, c7, BottomY)
            g.DrawString(" : " & Bdate, fontRegular, Brushes.Black, c8, TopY)
            'g.DrawString(" : " & Bdate, fontRegular, Brushes.Black, c8, BottomY)
            Select Case Trantype
                Case "ORD"
                    TopY = TopY + 18
                    'BottomY = BottomY + 18
                    g.DrawString("Due", fontRegular, Brushes.Black, c7, TopY)
                    'g.DrawString("Due", fontRegular, Brushes.Black, c7, BottomY)
                    g.DrawString(" : " & DueDate, fontRegular, Brushes.Black, c8, TopY)
                    'g.DrawString(" : " & DueDate, fontRegular, Brushes.Black, c8, BottomY)
                    TopY = TopY - 18
                    'BottomY = BottomY - 18
            End Select
            TopY = TopY + 18
            'BottomY = BottomY + 18
            CusArea = Trim(dtCustInfo.Rows(0).Item("City") & "").ToString & " " & Trim(dtCustInfo.Rows(0).Item("PinCode") & "").ToString
            g.DrawString(CusArea, fontRegular, Brushes.Black, c1, TopY)
            'g.DrawString(CusArea, fontRegular, Brushes.Black, c1, BottomY)
            Select Case Trantype
                Case "POS"
                    If TrantypeMI = "RD" Then
                        g.DrawString("REPAIR DELIVERY", fontBoldTitle, Brushes.Black, c3, TopY)
                        'g.DrawString("REPAIR DELIVERY", fontBoldTitle, Brushes.Black, c3, BottomY)
                    ElseIf TrantypeMI = "MI" Then
                        g.DrawString("MISC ISSUE", fontBoldTitle, Brushes.Black, c3, TopY)
                        'g.DrawString("MISC ISSUE", fontBoldTitle, Brushes.Black, c3, BottomY)
                    Else
                        g.DrawString("SALES INVOICE", fontBoldTitle, Brushes.Black, c3, TopY)
                        'g.DrawString("SALES INVOICE", fontBoldTitle, Brushes.Black, c3, BottomY)
                    End If
                Case "ORD"
                    g.DrawString("ORDER BOOKING", fontBoldTitle, Brushes.Black, c3, TopY)
                    'g.DrawString("ORDER BOOKING", fontBoldTitle, Brushes.Black, c3, BottomY)
                Case "REP"
                    g.DrawString("REPAIR BOOKING", fontBoldTitle, Brushes.Black, c3, TopY)
                    'g.DrawString("REPAIR BOOKING", fontBoldTitle, Brushes.Black, c3, BottomY)
            End Select
            TopY = TopY + 18
            'BottomY = BottomY + 18
            If Trim(dtCustInfo.Rows(0).Item("phoneres") & "").ToString <> "" Then
                CusPhone_Mobi = Trim(dtCustInfo.Rows(0).Item("phoneres") & "").ToString & " " & Trim(dtCustInfo.Rows(0).Item("mobile") & "").ToString & " " & Trim(IIf(dtCustInfo.Rows(0).Item("PAN").ToString <> "", "PAN :" & dtCustInfo.Rows(0).Item("PAN").ToString, "")).ToString
                g.DrawString(CusPhone_Mobi, fontRegular, Brushes.Black, c1, TopY)
                'g.DrawString(CusPhone_Mobi, fontRegular, Brushes.Black, c1, BottomY)
                TopY = TopY + 18
                'BottomY = BottomY + 18
            Else
                CusPhone = Trim(dtCustInfo.Rows(0).Item("mobile").ToString) & " " & Trim(IIf(dtCustInfo.Rows(0).Item("PAN").ToString <> "", "PAN :" & dtCustInfo.Rows(0).Item("PAN").ToString, "")).ToString
                g.DrawString(CusPhone, fontRegular, Brushes.Black, c1, TopY)
                'g.DrawString(CusPhone, fontRegular, Brushes.Black, c1, BottomY)
                TopY = TopY + 18
                'BottomY = BottomY + 18
            End If
        End If
        DrawLine(g, TopY, TopY)
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
        g.DrawString("QTY", fontRegular, Brushes.Black, c3, TopY, RightFormat)
        g.DrawString("GROSS WT", fontRegular, Brushes.Black, c4, TopY, RightFormat)
        g.DrawString("LESS WT", fontRegular, Brushes.Black, c5, TopY, RightFormat)
        g.DrawString("VA", fontRegular, Brushes.Black, c6, TopY, RightFormat)
        g.DrawString("RATE", fontRegular, Brushes.Black, c7, TopY, RightFormat)
        g.DrawString("MC", fontRegular, Brushes.Black, c8, TopY, RightFormat)
        g.DrawString("AMOUNT", fontRegular, Brushes.Black, c9, TopY, RightFormat)

        'g.DrawString("SNo", fontRegular, Brushes.Black, c1, BottomY, LeftFormat)
        'g.DrawString("DESCRIPTION", fontRegular, Brushes.Black, c2, BottomY, LeftFormat)
        'g.DrawString("QTY", fontRegular, Brushes.Black, c3, BottomY, RightFormat)
        'g.DrawString("GROSS WT", fontRegular, Brushes.Black, c4, BottomY, RightFormat)
        'g.DrawString("LESS WT", fontRegular, Brushes.Black, c5, BottomY, RightFormat)
        'g.DrawString("VA", fontRegular, Brushes.Black, c6, BottomY, RightFormat)
        'g.DrawString("RATE", fontRegular, Brushes.Black, c7, BottomY, RightFormat)
        'g.DrawString("MC", fontRegular, Brushes.Black, c8, BottomY, RightFormat)
        'g.DrawString("AMOUNT", fontRegular, Brushes.Black, c9, BottomY, RightFormat)
        TopY = TopY + 20
        'BottomY = BottomY + 20
        'DrawLine(g, TopY, BottomY)
        TopY = TopY + 10
        'BottomY = BottomY + 10
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

        'g.DrawString("SNo", fontRegular, Brushes.Black, c1, BottomY, LeftFormat)
        'g.DrawString("DESCRIPTION", fontRegular, Brushes.Black, c2, BottomY, LeftFormat)
        'g.DrawString("", fontRegular, Brushes.Black, c3, BottomY, RightFormat)
        'g.DrawString("", fontRegular, Brushes.Black, c4, BottomY, RightFormat)
        'g.DrawString("", fontRegular, Brushes.Black, c5, BottomY, RightFormat)
        'g.DrawString("QTY", fontRegular, Brushes.Black, c6, BottomY, RightFormat)
        'g.DrawString("", fontRegular, Brushes.Black, c7, BottomY, RightFormat)
        'g.DrawString("", fontRegular, Brushes.Black, c8, BottomY, RightFormat)
        'g.DrawString("GROSS WT", fontRegular, Brushes.Black, c9, BottomY, RightFormat)
        TopY = TopY + 20
        'BottomY = BottomY + 20
        'DrawLine(g, TopY, BottomY)
        TopY = TopY + 10
        'BottomY = BottomY + 10
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

            'g.DrawString("ItemId", fontBold, Brushes.Black, c1, BottomY, LeftFormat)
            'g.DrawString("ItemName", fontBold, Brushes.Black, c2, BottomY, LeftFormat)
            'g.DrawString("", fontRegular, Brushes.Black, c3, BottomY, RightFormat)
            'g.DrawString("", fontRegular, Brushes.Black, c4, BottomY, RightFormat)
            'g.DrawString("", fontRegular, Brushes.Black, c5, BottomY, RightFormat)
            'g.DrawString("Weight", fontBold, Brushes.Black, c6, BottomY, RightFormat)
            'g.DrawString("", fontRegular, Brushes.Black, c7, BottomY, RightFormat)
            'g.DrawString("", fontRegular, Brushes.Black, c8, BottomY, RightFormat)
            'g.DrawString("", fontRegular, Brushes.Black, c9, BottomY, RightFormat)
            TopY = TopY + 20
            'BottomY = BottomY + 20
        End If
    End Sub
    Public Sub TitleSmithIssueReceipt(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
        g.DrawString("SNo", fontRegular, Brushes.Black, c1, TopY, LeftFormat)
        g.DrawString("Description", fontRegular, Brushes.Black, c2, TopY, LeftFormat)
        g.DrawString("Pcs", fontRegular, Brushes.Black, c3, TopY, RightFormat)
        g.DrawString("Gross.Wt", fontRegular, Brushes.Black, c4, TopY, RightFormat)
        g.DrawString("Net.Wt", fontRegular, Brushes.Black, c5, TopY, RightFormat)
        g.DrawString("Rate", fontRegular, Brushes.Black, c6, TopY, RightFormat)
        g.DrawString("Dia.Wt", fontRegular, Brushes.Black, c7, TopY, RightFormat)
        g.DrawString("Mc", fontRegular, Brushes.Black, c8, TopY, RightFormat)
        g.DrawString("Amount", fontRegular, Brushes.Black, c9, TopY, RightFormat)

        'g.DrawString("SNo", fontRegular, Brushes.Black, c1, BottomY, LeftFormat)
        'g.DrawString("Description", fontRegular, Brushes.Black, c2, BottomY, LeftFormat)
        'g.DrawString("Pcs", fontRegular, Brushes.Black, c3, BottomY, RightFormat)
        'g.DrawString("Gross.Wt", fontRegular, Brushes.Black, c4, BottomY, RightFormat)
        'g.DrawString("Net.Wt", fontRegular, Brushes.Black, c5, BottomY, RightFormat)
        'g.DrawString("Rate", fontRegular, Brushes.Black, c6, BottomY, RightFormat)
        'g.DrawString("Dia.Wt", fontRegular, Brushes.Black, c7, BottomY, RightFormat)
        'g.DrawString("Mc", fontRegular, Brushes.Black, c8, BottomY, RightFormat)
        'g.DrawString("Amount", fontRegular, Brushes.Black, c9, BottomY, RightFormat)
        TopY = TopY + 20
        'BottomY = BottomY + 20
    End Sub
    Public Sub FooterSmithIssueReceipt(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        g.DrawString("C O P Y  ", fontBold, Brushes.Black, c8, 600)
        'Dim T1 As String = "Page " & NoofPrint.ToString
        'g.DrawString(T1, fontRegular, Brushes.Black, c8, 490)
        'g.DrawString(T1, fontRegular, Brushes.Black, c8, 1047)
        '' Horizontal Line
        Dim UserName As String = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("USERNAME")
        g.DrawLine(Pens.Silver, 20.0F, 230.0F, 20.0F, 445.0F) 'LEFT TOP
        g.DrawLine(Pens.Silver, 775.0F, 230.0F, 775.0F, 445.0F) 'RIGHT TOP

        g.DrawLine(Pens.Silver, 20.0F, 800.0F, 20.0F, 1000.0F) 'LEFT BOTTOM
        g.DrawLine(Pens.Silver, 775.0F, 800.0F, 775.0F, 1000.0F) 'RIGHT BOTTOM
        ''
        '' Vertical Line
        g.DrawLine(Pens.Silver, 20.0F, 445.0F, 775.0F, 445.0F)
        g.DrawString("Entered By ", fontRegular, Brushes.Black, c1, 495)
        g.DrawString(UserName, fontRegular, Brushes.Black, c1, 510)
        g.DrawString("Receiver Signature", fontRegular, Brushes.Black, 250, 495)
        g.DrawString("Verified By", fontRegular, Brushes.Black, c5, 495)
        g.DrawString("Authorised Signatory", fontRegular, Brushes.Black, c7, 495)

        g.DrawLine(Pens.Silver, 20.0F, 1000.0F, 775.0F, 1000.0F)
        g.DrawString("Entered By", fontRegular, Brushes.Black, c1, 1050)
        g.DrawString(UserName, fontRegular, Brushes.Black, c1, 1070)
        g.DrawString("Receiver Signature", fontRegular, Brushes.Black, 250, 1050)
        g.DrawString("Verified By", fontRegular, Brushes.Black, c5, 1050)
        g.DrawString("Authorised Signatory", fontRegular, Brushes.Black, c7, 1050)
        ''
    End Sub

    Public Sub Footer(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        'g.DrawString("C O P Y  ", fontBold, Brushes.Black, c8, 600)
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
                g.DrawLine(Pens.Silver, 20.0F, 490.0F, 775.0F, 490.0F)
            Case "ORD"
                g.DrawString("NOTE : ORIGINAL ORDER BOOKING RECEIPT SHOULD BE PRODUCED AT THE TIME OF ITEM(S) DELIVERY", fontBold, Brushes.Black, c2, 470)
                g.DrawLine(Pens.Silver, 20.0F, 490.0F, 775.0F, 490.0F)
            Case Else
                g.DrawLine(Pens.Silver, 20.0F, 490.0F, 220.0F, 490.0F)
                g.DrawLine(Pens.Silver, 555.0F, 490.0F, 775.0F, 490.0F)
                g.DrawString("Sale Value Inclusive of Excise Duty [" & EXCISENO & "]", fontRegular, Brushes.Black, 225, 480)
        End Select

        g.DrawString("E&O.E", fontRegular, Brushes.Black, c1, 500)
        'g.DrawString("Customer's Signature", fontRegular, Brushes.Black, 100, 500)
        g.DrawString(NodeId, fontRegular, Brushes.Black, c3, 500)
        g.DrawString("Cashier", fontRegular, Brushes.Black, c7, 500)
        If NoofPrint > 1 Then
            g.DrawString(T1, fontRegular, Brushes.Black, c8, 500)
        End If
        '1045
        'Select Case Trantype
        '    Case "REP"
        '        g.DrawString("NOTE : ITEM(S) MENTIONED ABOVE WILL BE DELIVERED ONLY BY PRODUCING ORIGINAL VOUCHER", fontBold, Brushes.Black, c2, 1040)
        '        g.DrawLine(Pens.Silver, 20.0F, 1060.0F, 775.0F, 1060.0F)
        '    Case "ORD"
        '        g.DrawString("NOTE : ORIGINAL ORDER BOOKING RECEIPT SHOULD BE PRODUCED AT THE TIME OF ITEM(S) DELIVERY", fontBold, Brushes.Black, c2, 1040)
        '        g.DrawLine(Pens.Silver, 20.0F, 1060.0F, 775.0F, 1060.0F)
        '    Case Else
        '        g.DrawLine(Pens.Silver, 20.0F, 1060.0F, 220.0F, 1060.0F)
        '        g.DrawLine(Pens.Silver, 555.0F, 1060.0F, 775.0F, 1060.0F)
        '        g.DrawString("Sale Value Inclusive of Excise Duty [" & EXCISENO & "] ", fontRegular, Brushes.Black, 225, 1050)
        'End Select
        'g.DrawString("E&O.E", fontRegular, Brushes.Black, c1, 1063)
        ''g.DrawString("Customer's Signature", fontRegular, Brushes.Black, 100, 1063)
        'g.DrawString(NodeId, fontRegular, Brushes.Black, c3, 1063)
        'g.DrawString("Cashier", fontRegular, Brushes.Black, c7, 1063)
        'If NoofPrint > 1 Then
        '    g.DrawString(T1, fontRegular, Brushes.Black, c8, 1063)
        'End If
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
                strsql += vbCrLf + " WHERE 1=1 "
                strsql += vbCrLf + " AND TRANDATE='" & strBilldate & "' AND TRANNO='" & EstNo_SA & "' "
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
                    strsql += vbCrLf + " FROM " & cnStockDb & "..ESTISSUE AS T WHERE 1=1 "
                    strsql += vbCrLf + " AND TRANDATE='" & strBilldate & "' AND TRANNO='" & EstNo_SA & "' "
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
                        g.DrawString("VA (" + dtBillPrint.Rows(k)("WASTPER").ToString + _
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
                    g.DrawString("VAT", fontRegular, brush, c1, y1)
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
                strsql += vbCrLf + " WHERE 1=1 "
                strsql += vbCrLf + " AND TRANDATE='" & strBilldate & "' AND TRANNO='" & EstNo_PU & "' AND TRANTYPE='PU' "
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
                strsql += vbCrLf + " FROM " & cnStockDb & "..ESTRECEIPT AS T WHERE 1=1 "
                strsql += vbCrLf + " AND TRANDATE='" & strBilldate & "' AND TRANNO='" & EstNo_PU & "' AND TRANTYPE='PU' "
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
    Private Sub PrintAdvance_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintAdvance.PrintPage
        Try
            Dim NoofPage As Integer = 0
            Dim rAlign As New StringFormat
            rAlign.Alignment = StringAlignment.Far
            NoofPrint += 1
            Using g As Graphics = e.Graphics
                CustomerInfoAdvanceReceipt(e.Graphics, e)
                TitleAdvanceReceipt(e.Graphics, e)
                If dsAdvanceReceipt.Tables("ITEMTAG").Rows.Count > 0 Then
                    For PagecountSale = PagecountSale To dsAdvanceReceipt.Tables("ITEMTAG").Rows.Count - 1
                        NoofPage += 1
                        With dsAdvanceReceipt.Tables("ITEMTAG").Rows(PagecountSale)
                            g.DrawString(IIf(Val(.Item("ITEMID").ToString()) > 0, .Item("ITEMID").ToString(), ""), fontRegular, BlackBrush, c1, TopY, LeftFormat)
                            g.DrawString(.Item("ITEMNAME").ToString(), fontRegular, BlackBrush, c2, TopY, LeftFormat)
                            g.DrawString(IIf(Val(.Item("WEIGHT").ToString()) > 0, .Item("WEIGHT").ToString(), ""), fontRegular, BlackBrush, c6, TopY, rAlign)

                            'g.DrawString(IIf(Val(.Item("ITEMID").ToString()) > 0, .Item("ITEMID").ToString(), ""), fontRegular, BlackBrush, c1, BottomY, LeftFormat)
                            'g.DrawString(.Item("ITEMNAME").ToString(), fontRegular, BlackBrush, c2, BottomY, LeftFormat)
                            'g.DrawString(IIf(Val(.Item("WEIGHT").ToString()) > 0, .Item("WEIGHT").ToString(), ""), fontRegular, BlackBrush, c6, BottomY, rAlign)

                            TopY = TopY + 20
                            'BottomY = BottomY + 20
                            If NoofPage > 4 Then
                                PagecountSale = PagecountSale + 1
                                TopY = 150 ' TOP AGAIN STARTING POSITION
                                'BottomY = 720 ' BOTTOM AGAIN STATRING POSITION
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c1, 490)
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c1, 1040)
                                e.HasMorePages = True
                                Exit For
                            End If
                        End With
                    Next
                End If
                'g.DrawString("C O P Y  ", fontBold, Brushes.Black, c8, 600)
                If dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field1").ToString.Contains("ADVANCE ") Then
                    g.DrawString("NOTE : ORIGINAL ADVANCE RECEIPT SHOULD BE PRODUCED AT THE TIME OF ITEM(S) DELIVERY ", fontBold, Brushes.Black, c2, 470)
                    'g.DrawString("NOTE : ORIGINAL ADVANCE RECEIPT SHOULD BE PRODUCED AT THE TIME OF ITEM(S) DELIVERY ", fontBold, Brushes.Black, c2, 1040)
                    g.DrawString(NodeId, fontRegular, BlackBrush, c7, TopY, rAlign)
                    'g.DrawString(NodeId, fontRegular, BlackBrush, c7, BottomY, rAlign)
                Else
                    g.DrawString(NodeId, fontRegular, BlackBrush, c7, TopY, rAlign)
                    'g.DrawString(NodeId, fontRegular, BlackBrush, c7, BottomY, rAlign)
                End If
                g.DrawLine(Pens.Silver, 20.0F, 490.0F, 775.0F, 490.0F)
                g.DrawLine(Pens.Silver, 20.0F, 1060.0F, 775.0F, 1060.0F)

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
                                'BottomY = BottomY - 2
                                'c6
                                g.DrawLine(Pens.Silver, 530, TopY, c6, TopY)
                                'g.DrawLine(Pens.Silver, 530, BottomY, c6, BottomY)
                                g.DrawLine(Pens.Silver, 710, TopY, c9, TopY)
                                'g.DrawLine(Pens.Silver, 710, BottomY, c9, BottomY)
                                TopY = TopY + 2
                                'BottomY = BottomY + 2
                            End If
                            g.DrawString(IIf(Val(.Item("SRNO").ToString()) > 0, .Item("SRNO").ToString(), ""), fontRegular, BlackBrush, c1, TopY, LeftFormat)
                            'g.DrawString(IIf(Val(.Item("SRNO").ToString()) > 0, .Item("SRNO").ToString(), ""), fontRegular, BlackBrush, c1, BottomY, LeftFormat)
                            If .Item("COLHEAD").ToString <> "G" Then
                                g.DrawString(.Item("ITEMNAME").ToString(), fontRegular, BlackBrush, c2, TopY, LeftFormat)
                                'g.DrawString(.Item("ITEMNAME").ToString(), fontRegular, BlackBrush, c2, BottomY, LeftFormat)
                            ElseIf .Item("COLHEAD").ToString = "G" And .Item("RESULT").ToString = "12" Then
                                g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c3, TopY, LeftFormat)
                                'g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c3, BottomY, LeftFormat)
                            ElseIf .Item("COLHEAD").ToString = "G" Then
                                g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c2, TopY, LeftFormat)
                                'g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c2, BottomY, LeftFormat)
                            End If
                            g.DrawString(IIf(Val(.Item("PCS").ToString()) > 0, Val(.Item("PCS").ToString()), ""), fontRegular, BlackBrush, c6, TopY, rAlign)
                            'g.DrawString(IIf(Val(.Item("PCS").ToString()) > 0, Val(.Item("PCS").ToString()), ""), fontRegular, BlackBrush, c6, BottomY, rAlign)
                            g.DrawString(IIf(Val(.Item("GRSWT").ToString()) > 0, .Item("GRSWT").ToString(), ""), fontRegular, BlackBrush, c9, TopY, rAlign)
                            'g.DrawString(IIf(Val(.Item("GRSWT").ToString()) > 0, .Item("GRSWT").ToString(), ""), fontRegular, BlackBrush, c9, BottomY, rAlign)
                            TopY = TopY + 20
                            'BottomY = BottomY + 20
                            If .Item("RESULT").ToString = "9" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "U" Then
                                'c6
                                g.DrawLine(Pens.Silver, 530, TopY, c6, TopY)
                                'g.DrawLine(Pens.Silver, 530, BottomY, c6, BottomY)
                                g.DrawLine(Pens.Silver, 710, TopY, c9, TopY)
                                'g.DrawLine(Pens.Silver, 710, BottomY, c9, BottomY)
                            End If
                            If NoofPage > 10 Then
                                PagecountSale = PagecountSale + 1
                                TopY = 150 ' TOP AGAIN STARTING POSITION
                                'BottomY = 720 ' BOTTOM AGAIN STATRING POSITION
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
                'g.DrawLine(Pens.Silver, 20, BottomY, 775, BottomY)
                RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
                If dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count > 0 And dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows.Count > 0 Then
                    For PagecountSale = PagecountSale To dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count - 1
                        With dsSmithIssueReceipt.Tables("DTISSREC").Rows(PagecountSale)
                            g.DrawString(IIf(Val(.Item("SNO").ToString()) > 0, .Item("SNO").ToString(), ""), fontRegular, BlackBrush, c1, TopY, LeftFormat)
                            g.DrawString(.Item("CATNAME").ToString(), fontRegular, BlackBrush, c2, TopY, LeftFormat)
                            g.DrawString(IIf(Val(.Item("PCS").ToString()) > 0, .Item("PCS").ToString(), ""), fontRegular, BlackBrush, c3, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("GRSWT").ToString()) > 0, .Item("GRSWT").ToString(), ""), fontRegular, BlackBrush, c4, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("NETWT").ToString()) > 0, .Item("NETWT").ToString(), ""), fontRegular, BlackBrush, c5, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("RATE").ToString()) > 0, "", ""), fontRegular, BlackBrush, c6, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("DIAWT").ToString()) > 0, .Item("DIAWT").ToString, ""), fontRegular, BlackBrush, c7, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("MC").ToString()) > 0, .Item("MC").ToString(), ""), fontRegular, BlackBrush, c8, TopY, rAlign)
                            g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) > 0, .Item("AMOUNT").ToString(), ""), fontRegular, BlackBrush, c9, TopY, rAlign)

                            'g.DrawString(IIf(Val(.Item("SNO").ToString()) > 0, .Item("SNO").ToString(), ""), fontRegular, BlackBrush, c1, BottomY, LeftFormat)
                            'g.DrawString(.Item("CATNAME").ToString(), fontRegular, BlackBrush, c2, BottomY, LeftFormat)
                            'g.DrawString(IIf(Val(.Item("PCS").ToString()) > 0, .Item("PCS").ToString(), ""), fontRegular, BlackBrush, c3, BottomY, rAlign)
                            'g.DrawString(IIf(Val(.Item("GRSWT").ToString()) > 0, .Item("GRSWT").ToString(), ""), fontRegular, BlackBrush, c4, BottomY, rAlign)
                            'g.DrawString(IIf(Val(.Item("NETWT").ToString()) > 0, .Item("NETWT").ToString(), ""), fontRegular, BlackBrush, c5, BottomY, rAlign)
                            'g.DrawString(IIf(Val(.Item("RATE").ToString()) > 0, "", ""), fontRegular, BlackBrush, c6, BottomY, rAlign)
                            'g.DrawString(IIf(Val(.Item("DIAWT").ToString()) > 0, .Item("DIAWT").ToString, ""), fontRegular, BlackBrush, c7, BottomY, rAlign)
                            'g.DrawString(IIf(Val(.Item("MC").ToString()) > 0, .Item("MC").ToString(), ""), fontRegular, BlackBrush, c8, BottomY, rAlign)
                            'g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) > 0, .Item("AMOUNT").ToString(), ""), fontRegular, BlackBrush, c9, BottomY, rAlign)
                            TopY = TopY + 20
                            'BottomY = BottomY + 20

                            If Val(.Item("WASTAGE").ToString()) > 0 Then
                                g.DrawString(.Item("WASTAGENAME").ToString(), fontRegular, BlackBrush, c2, TopY, LeftFormat)
                                'g.DrawString(.Item("WASTAGENAME").ToString(), fontRegular, BlackBrush, c2, BottomY, LeftFormat)
                                g.DrawString(.Item("WASTAGE").ToString(), fontRegular, BlackBrush, c5, TopY, rAlign)
                                'g.DrawString(.Item("WASTAGE").ToString(), fontRegular, BlackBrush, c5, BottomY, rAlign)
                                TopY = TopY + 20
                                'BottomY = BottomY + 20
                            End If
                            If Val(.Item("ALLOY").ToString()) > 0 Then
                                g.DrawString(.Item("ALLOYNAME").ToString(), fontRegular, BlackBrush, c2, TopY, LeftFormat)
                                'g.DrawString(.Item("ALLOYNAME").ToString(), fontRegular, BlackBrush, c2, BottomY, LeftFormat)
                                g.DrawString(.Item("ALLOY").ToString(), fontRegular, BlackBrush, c5, TopY, rAlign)
                                'g.DrawString(.Item("ALLOY").ToString(), fontRegular, BlackBrush, c5, BottomY, rAlign)
                                TopY = TopY + 20
                                'BottomY = BottomY + 20
                            End If

                            If Val(.Item("TAX").ToString()) > 0 Then
                                g.DrawString("VAT ", fontRegular, BlackBrush, c6, TopY, LeftFormat)
                                'g.DrawString("VAT ", fontRegular, BlackBrush, c6, BottomY, LeftFormat)
                                g.DrawString(.Item("TAX").ToString(), fontRegular, BlackBrush, c9, TopY, rAlign)
                                ' g.DrawString(.Item("TAX").ToString(), fontRegular, BlackBrush, c9, BottomY, rAlign)
                                TopY = TopY + 20
                                'BottomY = BottomY + 20
                            End If

                            If NoofPage > 6 Then
                                PagecountSale = PagecountSale + 1
                                TopY = 150 ' TOP AGAIN STARTING POSITION
                                'BottomY = 720 ' BOTTOM AGAIN STATRING POSITION
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c1, 490)
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c1, 1040)
                                e.HasMorePages = True
                                Exit For
                            End If
                        End With
                    Next
                End If
                If dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows.Count > 0 Then
                    Dim Remark1 As String = ""
                    Dim Remark2 As String = ""
                    Remark1 = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("REMARK1").ToString
                    Remark2 = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("REMARK2").ToString
                    g.DrawString(Remark1 & " " & Remark2, fontRegular, BlackBrush, c2, 400, LeftFormat) 'Top
                    g.DrawString(Remark1 & " " & Remark2, fontRegular, BlackBrush, c2, 955, LeftFormat) 'Bottom
                    g.DrawLine(Pens.Silver, 20, 420, 775, 420)
                    g.DrawLine(Pens.Silver, 20, 975, 775, 975)
                    Dim TopYTemp As Integer = 425
                    Dim BottomYTemp As Integer = 980
                    With dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(0)
                        g.DrawString(.Item("TOTAL").ToString(), fontRegular, BlackBrush, c2, TopYTemp, LeftFormat)
                        g.DrawString(IIf(Val(.Item("PCS").ToString()) > 0, .Item("PCS").ToString(), ""), fontRegular, BlackBrush, c3, TopYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("GRSWT").ToString()) > 0, .Item("GRSWT").ToString(), ""), fontRegular, BlackBrush, c4, TopYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("NETWT").ToString()) > 0, .Item("NETWT").ToString(), ""), fontRegular, BlackBrush, c5, TopYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("MC").ToString()) > 0, .Item("MC").ToString(), ""), fontRegular, BlackBrush, c8, TopYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) > 0, .Item("AMOUNT").ToString(), ""), fontRegular, BlackBrush, c9, TopYTemp, rAlign)

                        g.DrawString(.Item("TOTAL").ToString(), fontRegular, BlackBrush, c2, BottomYTemp, LeftFormat)
                        g.DrawString(IIf(Val(.Item("PCS").ToString()) > 0, .Item("PCS").ToString(), ""), fontRegular, BlackBrush, c3, BottomYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("GRSWT").ToString()) > 0, .Item("GRSWT").ToString(), ""), fontRegular, BlackBrush, c4, BottomYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("NETWT").ToString()) > 0, .Item("NETWT").ToString(), ""), fontRegular, BlackBrush, c5, BottomYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("MC").ToString()) > 0, .Item("MC").ToString(), ""), fontRegular, BlackBrush, c8, BottomYTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) > 0, .Item("AMOUNT").ToString(), ""), fontRegular, BlackBrush, c9, BottomYTemp, rAlign)
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
End Class