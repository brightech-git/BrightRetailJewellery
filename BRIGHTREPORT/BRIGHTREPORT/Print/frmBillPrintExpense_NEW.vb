Imports System.Data.OleDb
Imports System.IO
Public Class frmBillPrintExpense_NEW

    Dim strsql As String
    Dim da As OleDbDataAdapter
    Dim dt As DataTable
    Dim STKTYPE As String
    Dim Partyid As String
    Dim Pbatchno As String
    Dim USERNAME As String
    Dim SystemId As String = ""
    Dim SystemName As String = Environment.MachineName
    Dim cmd As OleDbCommand
    Dim dsPurchasePrint As New DataSet
    Dim Topy As Integer = 120
    Dim upTime As String = ""

    Dim fontRegular As New Font("Palatino Linotype", 9, FontStyle.Regular)
    Dim fontMargin As New Font("Palatino Linotype", 7, FontStyle.Regular)
    Dim fontBold As New Font("Palatino Linotype", 9, FontStyle.Bold)
    Dim fontUnderLine As New Font("Palatino Linotype", 9, FontStyle.Underline)
    Dim fontBoldUnderLine As New Font("Palatino Linotype", 9, FontStyle.Bold And FontStyle.Underline)
    Dim fontBoldTitle As New Font("Palatino Linotype", 11, FontStyle.Bold)
    Dim LeftFormat As New StringFormat(StringAlignment.Near)
    Dim RightFormat As New StringFormat(StringAlignment.Far)
    Dim CentreFormat As New StringFormat(StringAlignment.Center)

    Dim c1 As Integer = 20  ' SNo
    Dim c2 As Integer = 50  ' Description
    Dim c3 As Integer = 350  ' Remarks
    Dim c4 As Integer = 500 ' Hsn 
    Dim c5 As Integer = 650 ' TaxRate 
    Dim c6 As Integer = 750 ' Amount


    Dim BILLFORMAT As String = ""
    Dim OfflineAdmindb As String = ""

    Dim DuplicatePrint As Boolean = False
    Dim _PdATE As String
    Dim FYEAR As String = ""
    Dim ACCODE As String = ""
    Dim EMAILPATH As String = ""
    Dim temp As String = System.Environment.GetEnvironmentVariable("temp")
    Dim barcode As New BarcodeLib.Barcode.Linear
    Dim _dbName As String = ""
    Dim mreceiptNo As Integer = 0
    Dim Printcashid As String = ""
    Dim companyGstno As String = ""
    Dim InformationCopy As Boolean = False
    Dim companyPanNo As String = ""


    Public Sub New(ByVal Batchno As String, ByVal PDate As String, ByVal _DuplicatePrint As Boolean, ByVal saledbname As String, ByVal Ppreview As Boolean, ByVal _InformationCopy As Boolean, ByVal printName As String, Optional ByVal printcopy As String = "")
        Pbatchno = Batchno
        DuplicatePrint = _DuplicatePrint
        InformationCopy = _InformationCopy
        _PdATE = PDate
        InitializeComponent()
        _dbName = saledbname
        strsql = vbCrLf + " SELECT DISTINCT '' ENTREFNO,ACCODE AS PARTYID, "
        strsql += vbCrLf + " (SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE ID = R.USERID) USERNAME, "
        strsql += vbCrLf + " '' STKTYPE,'N' TAXEXEMPT "
        strsql += vbCrLf + " ,'' BILLFORMAT"
        strsql += vbCrLf + " ,ISNULL(SYSTEMID,'') AS SYSTEMID"
        strsql += vbCrLf + " ,CONVERT(VARCHAR(15),UPTIME,108) UPTIME"
        strsql += vbCrLf + " ,'' FYEAR,TRANNO RECNO  "
        strsql += vbCrLf + " ,COMPANYID"
        strsql += vbCrLf + " ,BILLPREFIX"
        strsql += vbCrLf + " FROM " & _dbName & "..GSTREGISTER as r "
        strsql += vbCrLf + " WHERE TRANDATE = '" & _PdATE & "' "
        strsql += vbCrLf + " AND BATCHNO = '" & Batchno & "' "
        da = New OleDbDataAdapter(strsql, cn)
        dt = New DataTable
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            Partyid = dt.Rows(0).Item("PARTYID").ToString
            USERNAME = dt.Rows(0).Item("USERNAME").ToString
            STKTYPE = dt.Rows(0).Item("STKTYPE").ToString
            BILLFORMAT = dt.Rows(0).Item("BILLFORMAT").ToString
            SystemId = dt.Rows(0).Item("SYSTEMID").ToString
            upTime = dt.Rows(0).Item("UPTIME").ToString
            If upTime <> "" Then
            Else
                upTime = ""
            End If
            mreceiptNo = dt.Rows(0).Item("RECNO").ToString
            'Printcashid = purchaseCashNamePrint(SystemId)
            'companyGstno = funcGetCompanyGstNo(dt.Rows(0).Item("COMPANYID").ToString)
            'companyPanNo = funcGetCompanyPanNo(dt.Rows(0).Item("COMPANYID").ToString)

            Printcashid = ""
            companyGstno = ""
            companyPanNo = ""
        End If
        DeleteTemp()
        Address()
        ReceiptNew()
        strsql = "SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "ADDRESS] "
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dsPurchasePrint, "ADDRESS")
        strsql = "SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] ORDER BY RESULT,SNO"
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dsPurchasePrint, "RECEIPT")

        'strsql = " SELECT ENTDATE,TRANSPORT,LRNO FROM " & cnAdminDb & "..LRDELIVERYSLIP "
        'strsql += vbCrLf + "  WHERE PEREFDATE = '" & _PdATE & "' AND PERECNO = '" & mreceiptNo & "'"
        'da = New OleDbDataAdapter(strsql, cn)
        'da.Fill(dsPurchasePrint, "LRDELIVERYSLIP")


        If Ppreview = True Then
            PrintDocument1.PrintController = New System.Drawing.Printing.StandardPrintController
            DirectCast(PrintPreviewDialog1, Form).WindowState = FormWindowState.Maximized
            CType(PrintPreviewDialog1.Controls(1), ToolStrip).Items(0).Enabled = False
            PrintDialog1.Document = PrintDocument1
            PrintPreviewDialog1.Document = Me.PrintDocument1
            PrintDocument1.PrinterSettings.Copies = 1
            PrintPreviewDialog1.ShowDialog()
            Exit Sub
        End If
        PrintDialog1.Document = PrintDocument1
        If printName = "" Then MsgBox("Print Name Not Select", MsgBoxStyle.Information) : Exit Sub
        PrintDocument1.PrinterSettings.PrinterName = printName
        PrintDocument1.PrinterSettings.Copies = 1
        PrintDocument1.PrintController = New System.Drawing.Printing.StandardPrintController
        PrintDocument1.Print()
        DeleteTemp()
    End Sub
    Private Sub DeleteTemp()
        strsql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "ADDRESS]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "ADDRESS] "
        strsql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] "
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
    End Sub
    Private Sub Address()
        strsql = vbCrLf + " SELECT ACCODE AS PARTYID,ACNAME,ADDRESS1,ADDRESS2,ADDRESS3,PINCODE, "
        strsql += vbCrLf + " AREA AREANAME,"
        strsql += vbCrLf + " CITY CITYNAME"
        strsql += vbCrLf + " ,'T' AS COLHEAD, 1 AS TYPE, 1 AS RESULT, ISNULL(GSTNO,'')GSTIN"
        strsql += vbCrLf + " ,ACCODE"
        strsql += vbCrLf + " ,ISNULL(PAN,'') PAN"
        strsql += vbCrLf + " INTO TEMPTABLEDB..[TEMP" & SystemName & "ADDRESS]"
        strsql += vbCrLf + " FROM " & cnAdminDb & "..ACHEAD "
        strsql += vbCrLf + " WHERE ACCODE = '" & Partyid & "'"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
    End Sub

    Private Sub ReceiptNew()
        strsql = vbCrLf + " SELECT  "
        strsql += vbCrLf + "  ROW_NUMBER() OVER(ORDER BY SNO) SNO "
        strsql += vbCrLf + "  ,DESCRIPTION NARRATION "
        strsql += vbCrLf + " ,REMARKS "
        strsql += vbCrLf + " ,R.PRODID"
        strsql += vbCrLf + " ,(SELECT NAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = R.PRODID) PRODNAME "
        strsql += vbCrLf + " ,CONVERT(vARCHAR(30),HSN) HSNCODE"
        strsql += vbCrLf + " ,CAST((CGSTPER+SGSTPER+IGSTPER) AS FLOAT) TAXPER"
        strsql += vbCrLf + " ,CONVERT(NUMERIC(15,2),(AMOUNT+DISCCASH)) NETAMOUNT"
        strsql += vbCrLf + " ,BATCHNO "
        strsql += vbCrLf + " ,CONVERT(VARCHAR(15),TRANDATE,103) ENTREFDATE"
        strsql += vbCrLf + " ,TRANDATE TENTREFDATE"
        strsql += vbCrLf + " ,TRANNO RECNO"
        strsql += vbCrLf + " ,REFNO INVNO "
        strsql += vbCrLf + " ,CONVERT(VARCHAR(15),REFDATE,103) INVDATE"
        strsql += vbCrLf + " ,CONVERT(NUMERIC(15,2),AMOUNT) AMOUNT"
        strsql += vbCrLf + " ,CONVERT(NUMERIC(15,2),DISCCASH) DISCVAL"
        strsql += vbCrLf + " ,CGSTPER "
        strsql += vbCrLf + " ,CONVERT(NUMERIC(15,2),CGSTVAL) CGSTVAL"
        strsql += vbCrLf + " ,SGSTPER "
        strsql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SGSTVAL) SGSTVAL"
        strsql += vbCrLf + " ,IGSTPER"
        strsql += vbCrLf + " ,CONVERT(NUMERIC(15,2),IGSTVAL) IGSTVAL"
        strsql += vbCrLf + " ,CESSPER"
        strsql += vbCrLf + " ,CONVERT(NUMERIC(15,2),CESSVAL) CESSVAL"
        strsql += vbCrLf + " ,TDSPER"
        strsql += vbCrLf + " ,CONVERT(NUMERIC(15,2),TDSVAL) TDSVAL"
        strsql += vbCrLf + " ,TCSPER"
        strsql += vbCrLf + " ,CONVERT(NUMERIC(15,2),TCSVAL) TCSVAL"
        strsql += vbCrLf + " ,1 RESULT, 'D' COLHEAD"
        strsql += vbCrLf + " ,CONVERT(VARCHAR(2),'') TYPE"
        strsql += vbCrLf + " , ISNULL(BILLPREFIX,'') BILLPREFIX"
        strsql += vbCrLf + " ,CASE WHEN R.TRANTYPE ='PE' THEN 'PURCHASE EXPENSE' ELSE "
        strsql += vbCrLf + "  CASE WHEN R.TRANTYPE='PC' THEN 'PURCHASE CREDIT NOTE' ELSE "
        strsql += vbCrLf + "  CASE WHEN R.TRANTYPE='PD' THEN 'PURCHASE DEBIT NOTE' ELSE  "
        strsql += vbCrLf + " '' END END END TRANTYPENAME"
        strsql += vbCrLf + " INTO TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] "
        strsql += vbCrLf + " FROM " & _dbName & "..GSTREGISTER R "
        strsql += vbCrLf + " WHERE TRANDATE = '" & _PdATE & "' "
        strsql += vbCrLf + " AND R.BATCHNO = '" & Pbatchno & "' "
        strsql += vbCrLf + " AND R.TRANTYPE IN ('PE','PD','PC') "
        strsql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y' "
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        Dim Disval As Double = 0
        strsql = " SELECT ISNULL(DISCVAL,0) DISCVAL FROM TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] AS R"
        strsql += vbCrLf + " WHERE R.TENTREFDATE = '" & _PdATE & "' "
        strsql += vbCrLf + " AND R.BATCHNO = '" & Pbatchno & "' "
        strsql += vbCrLf + " AND RESULT = 1"
        Disval = Val(objGPack.GetSqlValue(strsql).ToString)

        If Disval > 0 Then
            strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] "
            strsql += vbCrLf + " ("
            strsql += vbCrLf + " SNO,HSNCODE,NETAMOUNT,RESULT,COLHEAD,BATCHNO,TYPE,BILLPREFIX,TRANTYPENAME"
            strsql += vbCrLf + " )"
            strsql += vbCrLf + " SELECT 99999,'' HSNCODE,SUM(AMOUNT+DISCVAL) NETAMOUNT  "
            strsql += vbCrLf + " ,3 RESULT, 'D' COLHEAD "
            strsql += vbCrLf + " ,BATCHNO,'AT' TYPE"
            strsql += vbCrLf + "  ,'' BILLPREFIX"
            strsql += vbCrLf + "  ,'' TRANTYPENAME"
            strsql += vbCrLf + " FROM TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] R "
            strsql += vbCrLf + " WHERE TENTREFDATE = '" & _PdATE & "' "
            strsql += vbCrLf + " AND R.BATCHNO = '" & Pbatchno & "' "
            strsql += vbCrLf + " AND RESULT = 1"
            strsql += vbCrLf + " GROUP BY R.BATCHNO"
            strsql += vbCrLf + " HAVING SUM(AMOUNT+DISCVAL) <> 0 "
            strsql += vbCrLf + " UNION ALL"
            strsql += vbCrLf + " SELECT 99999,'DISCOUNT' HSNCODE,SUM(DISCVAL) NETAMOUNT  "
            strsql += vbCrLf + " ,4 RESULT, 'D' COLHEAD "
            strsql += vbCrLf + " ,BATCHNO,'DI' TYPE"
            strsql += vbCrLf + "  ,'' BILLPREFIX"
            strsql += vbCrLf + "  ,'' TRANTYPENAME"
            strsql += vbCrLf + " FROM TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] R "
            strsql += vbCrLf + " WHERE R.TENTREFDATE = '" & _PdATE & "' "
            strsql += vbCrLf + " AND R.BATCHNO = '" & Pbatchno & "' "
            strsql += vbCrLf + " AND RESULT = 1"
            strsql += vbCrLf + " GROUP BY R.BATCHNO"
            strsql += vbCrLf + " HAVING SUM(DISCVAL) <> 0"
            strsql += vbCrLf + " UNION ALL"
            strsql += vbCrLf + " SELECT 99999 SNO,'G. TOTAL',SUM(ISNULL(AMOUNT,0)) NETAMOUNT  "
            strsql += vbCrLf + " ,5 RESULT, 'D' COLHEAD "
            strsql += vbCrLf + " ,BATCHNO,'GT' TYPE"
            strsql += vbCrLf + "  ,'' BILLPREFIX"
            strsql += vbCrLf + "  ,'' TRANTYPENAME"
            strsql += vbCrLf + " FROM TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] R "
            strsql += vbCrLf + " WHERE TENTREFDATE = '" & _PdATE & "' "
            strsql += vbCrLf + " AND R.BATCHNO = '" & Pbatchno & "' "
            strsql += vbCrLf + " AND RESULT = 1"
            strsql += vbCrLf + " GROUP BY R.BATCHNO"
            strsql += vbCrLf + " HAVING SUM(AMOUNT-DISCVAL) <> 0"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        End If

        strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] "
        strsql += vbCrLf + " ("
        strsql += vbCrLf + " SNO,HSNCODE,TAXPER,NETAMOUNT,RESULT,COLHEAD,BATCHNO,TYPE,BILLPREFIX,TRANTYPENAME"
        strsql += vbCrLf + " )"
        strsql += vbCrLf + " SELECT 99999 SNO,'CGST ' + LTRIM(CAST(CGSTPER AS FLOAT)) + ' %' HSNCODE,NULL TAXPER,SUM(CGSTVAL) NETAMOUNT  "
        strsql += vbCrLf + " ,6 RESULT, 'D' COLHEAD "
        strsql += vbCrLf + " ,BATCHNO, 'TX' TYPE"
        strsql += vbCrLf + "  ,'' BILLPREFIX"
        strsql += vbCrLf + "  ,'' TRANTYPENAME"
        strsql += vbCrLf + " FROM TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] R "
        strsql += vbCrLf + " WHERE TENTREFDATE = '" & _PdATE & "' "
        strsql += vbCrLf + " AND R.BATCHNO = '" & Pbatchno & "' "
        strsql += vbCrLf + " AND RESULT = 1"
        strsql += vbCrLf + " GROUP BY R.BATCHNO,CGSTPER"
        strsql += vbCrLf + " HAVING SUM(CGSTVAL) > 0"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()


        strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] "
        strsql += vbCrLf + " ("
        strsql += vbCrLf + " SNO,HSNCODE,TAXPER,NETAMOUNT,RESULT,COLHEAD,BATCHNO,TYPE,BILLPREFIX,TRANTYPENAME"
        strsql += vbCrLf + " )"
        strsql += vbCrLf + " SELECT 99999 SNO,'SGST ' + LTRIM(CAST(SGSTPER AS FLOAT)) + ' %' HSNCODE,NULL TAXPER"
        strsql += vbCrLf + " ,SUM(SGSTVAL) NETAMOUNT  "
        strsql += vbCrLf + " ,6 RESULT, 'D' COLHEAD "
        strsql += vbCrLf + " ,BATCHNO,'TX' TYPE"
        strsql += vbCrLf + "  ,'' BILLPREFIX"
        strsql += vbCrLf + "  ,'' TRANTYPENAME"
        strsql += vbCrLf + " FROM TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] R "
        strsql += vbCrLf + " WHERE TENTREFDATE = '" & _PdATE & "' "
        strsql += vbCrLf + " AND R.BATCHNO = '" & Pbatchno & "' "
        strsql += vbCrLf + " AND RESULT = 1"
        strsql += vbCrLf + " GROUP BY R.BATCHNO,SGSTPER"
        strsql += vbCrLf + " HAVING SUM(SGSTVAL) > 0"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()


        strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] "
        strsql += vbCrLf + " ("
        strsql += vbCrLf + " SNO,HSNCODE,TAXPER,NETAMOUNT,RESULT,COLHEAD,BATCHNO,TYPE,BILLPREFIX,TRANTYPENAME"
        strsql += vbCrLf + " )"
        strsql += vbCrLf + " SELECT 99999 SNO,'IGST ' + LTRIM(CAST(IGSTPER AS FLOAT)) + ' %' HSNCODE,NULL TAXPER"
        strsql += vbCrLf + " ,SUM(IGSTVAL) NETAMOUNT  "
        strsql += vbCrLf + " ,6 RESULT, 'D' COLHEAD "
        strsql += vbCrLf + " ,BATCHNO,'TX' TYPE"
        strsql += vbCrLf + "  ,'' BILLPREFIX"
        strsql += vbCrLf + "  ,'' TRANTYPENAME"
        strsql += vbCrLf + " FROM TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] R "
        strsql += vbCrLf + " WHERE TENTREFDATE = '" & _PdATE & "' "
        strsql += vbCrLf + " AND R.BATCHNO = '" & Pbatchno & "' "
        strsql += vbCrLf + " AND RESULT = 1"
        strsql += vbCrLf + " GROUP BY R.BATCHNO,IGSTPER"
        strsql += vbCrLf + " HAVING SUM(IGSTVAL) > 0"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()


        strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] "
        strsql += vbCrLf + " ("
        strsql += vbCrLf + " SNO,HSNCODE,TAXPER,NETAMOUNT,RESULT,COLHEAD,BATCHNO,TYPE,BILLPREFIX,TRANTYPENAME"
        strsql += vbCrLf + " )"
        strsql += vbCrLf + " SELECT 99999 SNO,'CESS ' + LTRIM(CAST(CESSPER AS FLOAT)) + ' %' HSNCODE,NULL TAXPER"
        strsql += vbCrLf + " ,SUM(ISNULL(CESSVAL,0)) NETAMOUNT  "
        strsql += vbCrLf + " ,6 RESULT, 'D' COLHEAD "
        strsql += vbCrLf + " ,BATCHNO,'TX' TYPE"
        strsql += vbCrLf + "  ,'' BILLPREFIX"
        strsql += vbCrLf + "  ,'' TRANTYPENAME"
        strsql += vbCrLf + " FROM TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] R "
        strsql += vbCrLf + " WHERE TENTREFDATE = '" & _PdATE & "' "
        strsql += vbCrLf + " AND R.BATCHNO = '" & Pbatchno & "' "
        strsql += vbCrLf + " AND RESULT = 1"
        strsql += vbCrLf + " GROUP BY R.BATCHNO,CESSPER"
        strsql += vbCrLf + " HAVING SUM(ISNULL(CESSVAL,0)) > 0"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()



        strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] "
        strsql += vbCrLf + " ("
        strsql += vbCrLf + " SNO,HSNCODE,TAXPER,NETAMOUNT,RESULT,COLHEAD,BATCHNO,TYPE,BILLPREFIX,TRANTYPENAME"
        strsql += vbCrLf + " )"
        strsql += vbCrLf + " SELECT 999999 SNO,'TCS ' + LTRIM(CAST(TCSPER AS FLOAT)) + ' %' HSNCODE,NULL TAXPER"
        strsql += vbCrLf + " ,SUM(TCSVAL) NETAMOUNT  "
        strsql += vbCrLf + " ,6 RESULT, 'D' COLHEAD "
        strsql += vbCrLf + " ,BATCHNO,'TX' TYPE"
        strsql += vbCrLf + "  ,'' BILLPREFIX"
        strsql += vbCrLf + "  ,'' TRANTYPENAME"
        strsql += vbCrLf + " FROM TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] R "
        strsql += vbCrLf + " WHERE TENTREFDATE = '" & _PdATE & "' "
        strsql += vbCrLf + " AND R.BATCHNO = '" & Pbatchno & "' "
        strsql += vbCrLf + " AND RESULT = 1"
        strsql += vbCrLf + " GROUP BY R.BATCHNO,TCSPER"
        strsql += vbCrLf + " HAVING SUM(TCSVAL) > 0"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] "
        strsql += vbCrLf + " ("
        strsql += vbCrLf + " SNO,HSNCODE,TAXPER,NETAMOUNT,RESULT,COLHEAD,BATCHNO,TYPE,BILLPREFIX,TRANTYPENAME"
        strsql += vbCrLf + " )"
        strsql += vbCrLf + " SELECT 99999 SNO,'' HSNCODE,NULL TAXPER "
        strsql += vbCrLf + " ,SUM(CGSTVAL+SGSTVAL+IGSTVAL+AMOUNT+ISNULL(TCSVAL,0)+ISNULL(CESSVAL,0)) NETAMOUNT  "
        strsql += vbCrLf + " ,7 RESULT, 'D' COLHEAD "
        strsql += vbCrLf + " ,BATCHNO,'TX' TYPE"
        strsql += vbCrLf + "  ,'' BILLPREFIX"
        strsql += vbCrLf + "  ,'' TRANTYPENAME"
        strsql += vbCrLf + " FROM TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] R "
        strsql += vbCrLf + " WHERE TENTREFDATE = '" & _PdATE & "' "
        strsql += vbCrLf + " AND R.BATCHNO = '" & Pbatchno & "' "
        strsql += vbCrLf + " AND RESULT = 1"
        strsql += vbCrLf + " GROUP BY R.BATCHNO,IGSTPER"
        strsql += vbCrLf + " HAVING SUM(CGSTVAL+SGSTVAL+IGSTVAL+ISNULL(TCSVAL,0)+ISNULL(CESSVAL,0)) > 0"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()


        strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] "
        strsql += vbCrLf + " ("
        strsql += vbCrLf + " SNO,HSNCODE,TAXPER,NETAMOUNT,RESULT,COLHEAD,BATCHNO,TYPE,BILLPREFIX,TRANTYPENAME"
        strsql += vbCrLf + " )"
        strsql += vbCrLf + " SELECT 99999 SNO,'TDS ' + LTRIM(CAST(TDSPER AS FLOAT)) + ' %' HSNCODE"
        strsql += vbCrLf + " ,NULL TAXPER,SUM(TDSVAL) NETAMOUNT  "
        strsql += vbCrLf + " ,8 RESULT, 'D' COLHEAD "
        strsql += vbCrLf + " ,BATCHNO, 'TD' TYPE"
        strsql += vbCrLf + "  ,'' BILLPREFIX"
        strsql += vbCrLf + "  ,'' TRANTYPENAME"
        strsql += vbCrLf + " FROM TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] R "
        strsql += vbCrLf + " WHERE TENTREFDATE = '" & _PdATE & "' "
        strsql += vbCrLf + " AND R.BATCHNO = '" & Pbatchno & "' "
        strsql += vbCrLf + " AND RESULT = 1"
        strsql += vbCrLf + " GROUP BY R.BATCHNO,TDSPER"
        strsql += vbCrLf + " HAVING SUM(TDSVAL) > 0"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        Dim Roundoff As Double = 0
        Dim AcutalAmount As Double = 0
        Dim tdsval As Double = 0
        strsql = " SELECT CASE WHEN TRANMODE = 'D' THEN SUM(AMOUNT) ELSE -1 * SUM(AMOUNT) END NETAMOUNT FROM " & _dbName & "..ACCTRAN R "
        strsql += vbCrLf + " WHERE TRANDATE = '" & _PdATE & "' "
        strsql += vbCrLf + " AND R.BATCHNO = '" & Pbatchno & "' "
        strsql += vbCrLf + " AND TRANTYPE = 'RO'"
        strsql += vbCrLf + " GROUP BY TRANMODE,BATCHNO"
        strsql += vbCrLf + " HAVING SUM(AMOUNT) <> 0"
        Roundoff = Val(objGPack.GetSqlValue(strsql).ToString)

        strsql = vbCrLf + " SELECT "
        strsql += vbCrLf + " SUM(TDSVAL) NETAMOUNT  "
        strsql += vbCrLf + " FROM TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] R "
        strsql += vbCrLf + " WHERE TENTREFDATE = '" & _PdATE & "' "
        strsql += vbCrLf + " AND R.BATCHNO = '" & Pbatchno & "' "
        strsql += vbCrLf + " AND RESULT = 1"
        'strsql += vbCrLf + " GROUP BY R.BATCHNO" 'TDSPER
        tdsval = Val(objGPack.GetSqlValue(strsql).ToString)

        strsql = vbCrLf + " SELECT "
        strsql += vbCrLf + " SUM(CGSTVAL+SGSTVAL+IGSTVAL+AMOUNT+ISNULL(TCSVAL,0)+ISNULL(CESSVAL,0)) NETAMOUNT  "
        strsql += vbCrLf + " FROM TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] R "
        strsql += vbCrLf + " WHERE TENTREFDATE = '" & _PdATE & "' "
        strsql += vbCrLf + " AND R.BATCHNO = '" & Pbatchno & "' "
        strsql += vbCrLf + " AND RESULT = 1"
        AcutalAmount = Val(objGPack.GetSqlValue(strsql).ToString)

        If Roundoff <> 0 Then
            strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] "
            strsql += vbCrLf + " ("
            strsql += vbCrLf + " SNO,HSNCODE,TAXPER,NETAMOUNT,RESULT,COLHEAD,BATCHNO,TYPE,BILLPREFIX,TRANTYPENAME"
            strsql += vbCrLf + " )"
            strsql += vbCrLf + " SELECT 99999 SNO, '' HSNCODE"
            strsql += vbCrLf + " ,NULL TAXPER"
            strsql += vbCrLf + " ,CASE WHEN TRANMODE = 'D' THEN SUM(AMOUNT) ELSE -1 * SUM(AMOUNT) END NETAMOUNT  "
            strsql += vbCrLf + " ,10 RESULT, 'D' COLHEAD "
            strsql += vbCrLf + " ,BATCHNO,'RO' TYPE"
            strsql += vbCrLf + "  ,'' BILLPREFIX"
            strsql += vbCrLf + "  ,'' TRANTYPENAME"
            strsql += vbCrLf + " FROM " & _dbName & "..ACCTRAN R "
            strsql += vbCrLf + " WHERE TRANDATE = '" & _PdATE & "' "
            strsql += vbCrLf + " AND R.BATCHNO = '" & Pbatchno & "' "
            strsql += vbCrLf + " AND TRANTYPE = 'RO'"
            strsql += vbCrLf + " GROUP BY TRANMODE,BATCHNO"
            strsql += vbCrLf + " HAVING SUM(AMOUNT) > 0"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()

            strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] "
            strsql += vbCrLf + " ("
            strsql += vbCrLf + " SNO,HSNCODE,TAXPER,NETAMOUNT,RESULT,COLHEAD,BATCHNO,TYPE,BILLPREFIX,TRANTYPENAME"
            strsql += vbCrLf + " )"
            strsql += vbCrLf + " SELECT 99999 SNO, '' HSNCODE"
            strsql += vbCrLf + " ,NULL TAXPER"
            strsql += vbCrLf + " ,'" & (AcutalAmount - tdsval) + (Roundoff) & "' NETAMOUNT  "
            strsql += vbCrLf + " ,11 RESULT, 'D' COLHEAD "
            strsql += vbCrLf + " ,'' BATCHNO,'FA' TYPE"
            strsql += vbCrLf + "  ,'' BILLPREFIX"
            strsql += vbCrLf + "  ,'' TRANTYPENAME"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        Else
            strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] "
            strsql += vbCrLf + " ("
            strsql += vbCrLf + " SNO,HSNCODE,TAXPER,NETAMOUNT,RESULT,COLHEAD,BATCHNO,TYPE,BILLPREFIX,TRANTYPENAME"
            strsql += vbCrLf + " )"
            strsql += vbCrLf + " SELECT 99999 SNO, '' HSNCODE"
            strsql += vbCrLf + " ,NULL TAXPER"
            strsql += vbCrLf + " ,'" & (AcutalAmount - tdsval) + (Roundoff) & "' NETAMOUNT  "
            strsql += vbCrLf + " ,11 RESULT, 'D' COLHEAD "
            strsql += vbCrLf + " ,'' BATCHNO,'FA' TYPE"
            strsql += vbCrLf + "  ,'' BILLPREFIX"
            strsql += vbCrLf + "  ,'' TRANTYPENAME"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()

        End If
        If tdsval > 0 Then
            If Roundoff = 0 Then
                strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "RECEIPTHEAD] "
                strsql += vbCrLf + " ("
                strsql += vbCrLf + " SNO,HSNCODE,TAXPER,NETAMOUNT,RESULT,COLHEAD,BATCHNO,TYPE,BILLPREFIX,TRANTYPENAME"
                strsql += vbCrLf + " )"
                strsql += vbCrLf + " SELECT 99999 SNO, '' HSNCODE"
                strsql += vbCrLf + " ,NULL TAXPER"
                strsql += vbCrLf + " ,'" & (AcutalAmount - tdsval) + (Roundoff) & "' NETAMOUNT  "
                strsql += vbCrLf + " ,11 RESULT, 'D' COLHEAD "
                strsql += vbCrLf + " ,'' BATCHNO,'FA' TYPE"
                strsql += vbCrLf + "  ,'' BILLPREFIX"
                strsql += vbCrLf + "  ,'' TRANTYPENAME"
                cmd = New OleDbCommand(strsql, cn)
                cmd.ExecuteNonQuery()
            End If
        End If
    End Sub
    Dim NoofItem As Integer = 0
    Dim Noofcount As Integer = 0
    Dim NoofHSNItem As Integer = 0
    Dim NoofHSNCount As Integer = 0
    Dim hsnprint As Boolean = False
    Dim rAlign As New StringFormat
    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        rAlign.Alignment = StringAlignment.Far
        Using g As Graphics = e.Graphics
            CustomerTitle(e.Graphics, e)
            Title(e.Graphics, e)
            If DuplicatePrint = True Then
                e.Graphics.RotateTransform(40)
                Dim myfont As New Font("Arial", 80)
                Dim myBrush As New SolidBrush(Color.FromArgb(30, 0, 0, 255))
                e.Graphics.DrawString("Duplicate Copy", myfont, myBrush, 250, 30)
                e.Graphics.ResetTransform()
            End If
            If InformationCopy = True Then
                e.Graphics.RotateTransform(40)
                Dim myfont As New Font("Arial", 80)
                Dim myBrush As New SolidBrush(Color.FromArgb(30, 0, 0, 255))
                e.Graphics.DrawString("Internal Use", myfont, myBrush, 250, 30)
                e.Graphics.ResetTransform()
            End If
            For NoofItem = NoofItem To dsPurchasePrint.Tables("RECEIPT").Rows.Count - 1
                Noofcount += 1
                With dsPurchasePrint.Tables("RECEIPT").Rows(NoofItem)
                    If .Item("SNO").ToString = "99999" Then
                    Else
                        g.DrawString(IIf(IsDBNull(.Item("SNO")), "", .Item("SNO")), fontRegular, Brushes.Black, c1, Topy, LeftFormat)
                    End If
                    If Val(.Item("PRODID").ToString) > 0 Then
                        g.DrawString(IIf(IsDBNull(.Item("NARRATION")), "", .Item("NARRATION") & " " & .Item("PRODNAME")), fontRegular, Brushes.Black, c2, Topy, LeftFormat)
                    Else
                        g.DrawString(IIf(IsDBNull(.Item("NARRATION")), "", .Item("NARRATION")), fontRegular, Brushes.Black, c2, Topy, LeftFormat)
                    End If

                    g.DrawString(IIf(IsDBNull(.Item("HSNCODE")), "", .Item("HSNCODE")), fontRegular, Brushes.Black, c4, Topy, LeftFormat)

                    If Val(.Item("RESULT").ToString) = 1 Then
                        g.DrawString(IIf(IsDBNull(.Item("TAXPER")), "", .Item("TAXPER") & "%"), fontRegular, Brushes.Black, c5, Topy, RightFormat)
                    End If
                    If Val(.Item("RESULT").ToString) = 3 Then
                        DrawLineAmount(e.Graphics, e, Topy)
                        g.DrawString(IIf(IsDBNull(.Item("NETAMOUNT")), "", .Item("NETAMOUNT")), fontRegular, Brushes.Black, c6, Topy, RightFormat)
                    ElseIf Val(.Item("RESULT").ToString) = 4 Then
                        g.DrawString(IIf(IsDBNull(.Item("NETAMOUNT")), "", .Item("NETAMOUNT")), fontRegular, Brushes.Black, c6, Topy, RightFormat)
                    ElseIf Val(.Item("RESULT").ToString) = 5 Then
                        DrawLineAmount(e.Graphics, e, Topy)
                        g.DrawString(IIf(IsDBNull(.Item("NETAMOUNT")), "", .Item("NETAMOUNT")), fontRegular, Brushes.Black, c6, Topy, RightFormat)
                    ElseIf Val(.Item("RESULT").ToString) = 6 Then
                        g.DrawString(IIf(IsDBNull(.Item("NETAMOUNT")), "", .Item("NETAMOUNT")), fontRegular, Brushes.Black, c6, Topy, RightFormat)
                    ElseIf Val(.Item("RESULT").ToString) = 7 Then
                        DrawLineAmount(e.Graphics, e, Topy)
                        g.DrawString(IIf(IsDBNull(.Item("NETAMOUNT")), "", .Item("NETAMOUNT")), fontRegular, Brushes.Black, c6, Topy, RightFormat)
                    ElseIf Val(.Item("RESULT").ToString) = 8 Then
                        g.DrawString(IIf(IsDBNull(.Item("NETAMOUNT")), "", .Item("NETAMOUNT")), fontRegular, Brushes.Black, c6, Topy, RightFormat)
                    ElseIf Val(.Item("RESULT").ToString) = 9 Then
                        DrawLineAmount(e.Graphics, e, Topy)
                        g.DrawString(IIf(IsDBNull(.Item("NETAMOUNT")), "", .Item("NETAMOUNT")), fontRegular, Brushes.Black, c6, Topy, RightFormat)
                        DrawLineAmount(e.Graphics, e, Topy + 18)
                    ElseIf Val(.Item("RESULT").ToString) = 10 And .Item("TYPE").ToString = "RO" Then
                        If Val(.Item("NETAMOUNT").ToString) < 0 Then
                            g.DrawString("(-)", fontRegular, Brushes.Black, c5, Topy, RightFormat)
                            g.DrawString(IIf(IsDBNull(.Item("NETAMOUNT")), "", -1 * .Item("NETAMOUNT")), fontRegular, Brushes.Black, c6, Topy, RightFormat)
                        Else
                            g.DrawString(IIf(IsDBNull(.Item("NETAMOUNT")), "", .Item("NETAMOUNT")), fontRegular, Brushes.Black, c6, Topy, RightFormat)
                        End If
                    ElseIf Val(.Item("RESULT").ToString) = 11 Then
                        DrawLineAmount(e.Graphics, e, Topy)
                        g.DrawString(IIf(IsDBNull(.Item("NETAMOUNT")), "", .Item("NETAMOUNT")), fontRegular, Brushes.Black, c6, Topy, RightFormat)
                    Else
                        g.DrawString(IIf(IsDBNull(.Item("NETAMOUNT")), "", .Item("NETAMOUNT")), fontRegular, Brushes.Black, c6, Topy, RightFormat)
                    End If
                    If .Item("RESULT").ToString = 1 Then
                        If .Item("REMARKS").ToString.Trim <> "" Then
                            Topy = Topy + 15
                            g.DrawString(IIf(IsDBNull(.Item("REMARKS")), "", .Item("REMARKS")), fontRegular, Brushes.Black, c2 + 10, Topy, LeftFormat)
                            Topy = Topy - 15
                            Topy = Topy + 30
                        Else
                            Topy = Topy + 17
                        End If
                    Else
                        Topy = Topy + 17
                    End If
                    If Topy > 850 Then 'Noofcount > 44
                        g.DrawString("Continue.... ", fontRegular, Brushes.Black, c1, Topy)
                        DrawLine(e.Graphics, e)
                        footer(e.Graphics, e)
                        NoofItem = NoofItem + 1
                        Noofcount = 0
                        Topy = 120
                        e.HasMorePages = True
                        Return
                    End If
                End With
            Next
            If dsPurchasePrint.Tables("LRDELIVERYSLIP").Rows.Count > 0 Then
                With dsPurchasePrint.Tables("LRDELIVERYSLIP").Rows(0)
                    Topy = Topy + 17
                    g.DrawString("TRANSPORT DETAIL ", fontBold, Brushes.Black, c1, Topy, LeftFormat)
                    Topy = Topy + 17
                    g.DrawString("dt. " & Format(.Item("ENTDATE"), "dd/MM/yyyy") & " LR No. " & .Item("LRNO").ToString, fontMargin, Brushes.Black, c1, Topy, LeftFormat)
                    Topy = Topy + 17
                End With
            End If
            Topy = Topy + 17
            DrawLineLastLine(e.Graphics, e)
            footer(e.Graphics, e)
            If Topy > 1000 Then
                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c1, Topy)
                hsnprint = True
                footer(e.Graphics, e)
                Topy = 120
                e.HasMorePages = True
                Return
            End If
        End Using
    End Sub
    Public Sub DrawLineAmount(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal y1 As Integer)
        g.DrawLine(Pens.Black, c5, y1, c6, y1)
    End Sub
    Public Sub DrawLineLastLine(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        g.DrawLine(Pens.Black, 20, 1041, 775, 1041)
    End Sub
    Public Sub footer(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        g.DrawString("Entered By", fontRegular, Brushes.Black, c1, 1045, LeftFormat)
        g.DrawString("Verifed By ", fontRegular, Brushes.Black, c5, 1045, LeftFormat)
        g.DrawString(USERNAME, fontRegular, Brushes.Black, c1, 1060, LeftFormat)
        If Printcashid <> "" Then
            g.DrawString(SystemId & " [" & Printcashid & "] / " & upTime, fontRegular, Brushes.Black, 600, 1060, LeftFormat)
        Else
            g.DrawString(SystemId & " / " & upTime, fontRegular, Brushes.Black, 600, 1060, LeftFormat)
        End If
        Dim RECNO As String = dsPurchasePrint.Tables("RECEIPT").Rows(0).Item("RECNO").ToString
        Dim ENTREFDATE As String = dsPurchasePrint.Tables("RECEIPT").Rows(0).Item("ENTREFDATE").ToString
        Try
            Dim RECNO_REFDATE As String = RECNO & "-" & ENTREFDATE.Replace("/", "")
            If System.IO.File.Exists(temp & "\barcode" & SystemName & RECNO_REFDATE & ".png") = True Then
                System.IO.File.Delete(temp & "\barcode" & SystemName & RECNO_REFDATE & ".png")
            End If
            barcode.Type = BarcodeLib.Barcode.BarcodeType.CODE128
            barcode.Rotate = BarcodeLib.Barcode.RotateOrientation.BottomFacingDown
            barcode.Data = RECNO_REFDATE
            barcode.drawBarcode(temp & "\barcode" & SystemName & RECNO_REFDATE & ".png")
            Dim imgstream As New Bitmap(temp & "\barcode" & SystemName & RECNO_REFDATE & ".png")
            e.Graphics.DrawImage(imgstream, 330, 1050, 200, 50)
            imgstream.Dispose()
            If System.IO.File.Exists(temp & "\barcode" & SystemName & RECNO_REFDATE & ".png") = True Then
                System.IO.File.Delete(temp & "\barcode" & SystemName & RECNO_REFDATE & ".png")
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Public Sub Title(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        DrawLine(e.Graphics, e)
        Topy = Topy + 5
        RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
        g.DrawString("SNo", fontRegular, Brushes.Black, c1, Topy, LeftFormat)
        g.DrawString("DESCRIPTION", fontRegular, Brushes.Black, c2, Topy, LeftFormat)
        g.DrawString("", fontRegular, Brushes.Black, c3, Topy, LeftFormat)
        g.DrawString("HSN", fontRegular, Brushes.Black, c4, Topy, LeftFormat)
        g.DrawString("GST %", fontRegular, Brushes.Black, c5 - 40, Topy, LeftFormat)
        g.DrawString("AMOUNT", fontRegular, Brushes.Black, c6, Topy, RightFormat)
        Topy = Topy + 17
        DrawLine(e.Graphics, e)
        Topy = Topy + 10
    End Sub
    Public Sub CustomerTitle(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        'Dim imgMemoryStream As MemoryStream = New MemoryStream()
        'imgMemoryStream = funcPrintLOGO()
        'Dim Image As Image = Image.FromStream(imgMemoryStream)
        'e.Graphics.DrawImage(Image, 170, 0, 500, 130)
        g.DrawString("GSTIN : " & companyGstno, New Font("Palatino Linotype", 10, FontStyle.Bold), Brushes.Black, 10, 10)
        g.DrawString("PAN : " & companyPanNo, New Font("Palatino Linotype", 10, FontStyle.Bold), Brushes.Black, 10, 30)
        g.DrawString(BILLFORMAT, New Font("Palatino Linotype", 14, FontStyle.Bold), Brushes.Black, 750, (Topy - 20))
        Topy = Topy + 10
        If dsPurchasePrint.Tables("ADDRESS").Rows.Count > 0 Then
            Dim Partyid As String = dsPurchasePrint.Tables("ADDRESS").Rows(0).Item("PARTYID").ToString
            ACCODE = dsPurchasePrint.Tables("ADDRESS").Rows(0).Item("ACCODE").ToString
            Dim Acname As String = dsPurchasePrint.Tables("ADDRESS").Rows(0).Item("ACNAME").ToString
            Dim Address1 As String = dsPurchasePrint.Tables("ADDRESS").Rows(0).Item("ADDRESS1").ToString
            Dim Address2 As String = dsPurchasePrint.Tables("ADDRESS").Rows(0).Item("ADDRESS2").ToString
            Dim Address3 As String = dsPurchasePrint.Tables("ADDRESS").Rows(0).Item("ADDRESS3").ToString
            Dim Pincode As String = dsPurchasePrint.Tables("ADDRESS").Rows(0).Item("PINCODE").ToString
            Dim Areaname As String = dsPurchasePrint.Tables("ADDRESS").Rows(0).Item("AREANAME").ToString
            Dim Cityname As String = dsPurchasePrint.Tables("ADDRESS").Rows(0).Item("CITYNAME").ToString
            Dim gstIn As String = dsPurchasePrint.Tables("ADDRESS").Rows(0).Item("GSTIN").ToString
            Dim supPAN As String = dsPurchasePrint.Tables("ADDRESS").Rows(0).Item("PAN").ToString

            Dim RECNO As String = dsPurchasePrint.Tables("RECEIPT").Rows(0).Item("RECNO").ToString
            Dim BILLPREFIX As String = dsPurchasePrint.Tables("RECEIPT").Rows(0).Item("BILLPREFIX").ToString
            Dim ENTREFDATE As String = dsPurchasePrint.Tables("RECEIPT").Rows(0).Item("ENTREFDATE").ToString
            Dim INVNO As String = dsPurchasePrint.Tables("RECEIPT").Rows(0).Item("INVNO").ToString
            Dim INVDATE As String = dsPurchasePrint.Tables("RECEIPT").Rows(0).Item("INVDATE").ToString
            Dim TRANTYPENAME As String = dsPurchasePrint.Tables("RECEIPT").Rows(0).Item("TRANTYPENAME").ToString


            g.DrawString(TRANTYPENAME, fontBold, Brushes.Black, 350, Topy)
            Topy = Topy + 20

            g.DrawString(Acname & " [ " & ACCODE & " ] ", fontBold, Brushes.Black, c1, Topy)
            g.DrawString("Receipt No", fontRegular, Brushes.Black, c5 - 150, Topy)
            'g.DrawString(" : " & Global.GVariable.GVariable.GB_COSTID & "/" & RECNO & "/" & FYEAR, fontRegular, Brushes.Black, c6 - 150, Topy)
            g.DrawString(" : " & BILLPREFIX & RECNO, fontRegular, Brushes.Black, c6 - 150, Topy)

            Topy = Topy + 17

            g.DrawString(Address1 & " " & Address2, fontMargin, Brushes.Black, c1, Topy)

            g.DrawString("Date", fontRegular, Brushes.Black, c5 - 150, Topy)
            g.DrawString(" : " & ENTREFDATE, fontRegular, Brushes.Black, c6 - 150, Topy)
            Topy = Topy + 17

            g.DrawString(Address3 & " " & Areaname, fontMargin, Brushes.Black, c1, Topy)

            g.DrawString("Invoice No", fontRegular, Brushes.Black, c5 - 150, Topy)
            g.DrawString(" : " & INVNO, fontRegular, Brushes.Black, c6 - 150, Topy)
            Topy = Topy + 17
            g.DrawString(Cityname & " " & Pincode, fontMargin, Brushes.Black, c1, Topy)
            g.DrawString("GSTIN " & gstIn & IIf(supPAN <> "", " PAN " & supPAN, ""), fontMargin, Brushes.Black, 170, Topy)

            g.DrawString("Invoice Date", fontRegular, Brushes.Black, c5 - 150, Topy)
            g.DrawString(" : " & INVDATE, fontRegular, Brushes.Black, c6 - 150, Topy)
            Topy = Topy + 17
        End If
    End Sub
    Private Sub DrawLine(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        g.DrawLine(Pens.Black, 20, Topy, 775, Topy)
    End Sub
End Class