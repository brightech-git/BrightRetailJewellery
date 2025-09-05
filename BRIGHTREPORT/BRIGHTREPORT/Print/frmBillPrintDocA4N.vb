Imports System.Data.OleDb
Imports System.IO
Imports System.Drawing.Printing
Imports QRCoder

Public Class frmBillPrintDocA4N
#Region "Variable"
    Dim strsql As String
    Dim strsqlCheck As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand

    Dim pBatchno As String
    Dim NodeId As String 'SystemId
    Dim Trantype As String
    Dim TrantypeMI As String = ""
    Dim Trantypefoot As String = ""
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

    Dim fontBoldHead16 As New Font("Palatino Linotype", 16, FontStyle.Bold)
    Dim fontRegular As New Font("Palatino Linotype", 9, FontStyle.Regular)
    Dim fontRegular6 As New Font("Palatino Linotype", 6, FontStyle.Regular)
    Dim fontRegular7 As New Font("Palatino Linotype", 7, FontStyle.Regular)
    Dim fontitalic As New Font("Palatino Linotype", 8, FontStyle.Italic)
    Dim fontRegularsmall As New Font("Palatino Linotype", 8, FontStyle.Regular)
    Dim fontBold As New Font("Palatino Linotype", 9, FontStyle.Bold)
    Dim fontBoldsmall As New Font("Palatino Linotype", 7, FontStyle.Bold)
    Dim fontUnderLine As New Font("Palatino Linotype", 9, FontStyle.Underline)
    Dim fontBoldUnderLine As New Font("Palatino Linotype", 9, FontStyle.Bold And FontStyle.Underline)
    Dim fontBoldTitle As New Font("Palatino Linotype", 11, FontStyle.Bold)
    Dim fontBoldHead As New Font("Palatino Linotype", 20, FontStyle.Bold)
    Dim fontAmtInWord As New Font("Palatino Linotype", 7, FontStyle.Bold)

    Dim EstNo_SA As String = ""
    Dim EstNo_PU As String = ""

    Dim c1 As Integer = 25  ' SNo
    Dim c2 As Integer = 70  ' Description
    Dim h1 As Integer = 250  ' Description
    Dim c3 As Integer = 120  ' Description
    Dim c4 As Integer = 350 ' Qty 
    Dim c5 As Integer = 450 ' Grs. Wt '430
    Dim c6 As Integer = 530 ' Less. Wt 510
    Dim c7 As Integer = 560 ' VA
    Dim c8 As Integer = 600 ' Rate
    Dim c9 As Integer = 680 ' MC
    Dim c10 As Integer = 748 ' ' Amount
    Dim c11 As Integer = 750 ' Empid
    Dim BOTTOM_POS As Integer = 960 'Ending POSITION
    Dim SMITH_BOTTOM_POS As Integer = 520 'Ending POSITION
    Dim APPBOTTOM_POS As Integer = 800 'Approval Ending POSITION
    Dim TAPPBOTTOM_POS As Integer = 800 'Approval Ending POSITION
    Dim LINE_SPACE As Integer = 18
    Dim dt1 As New DataTable
    Dim PagecountSale As Integer = 0
    Dim GCPagecountSale As Integer = 0

    Dim START_POS As Integer = 250 ' TOP STARTING POSITION '135
    Dim TSTART_POS As Integer = 250 ' TOP STARTING POSITION
    Dim HLINEPOINT As Integer = 0

    Dim dtCustInfo As DataTable ' CustomerInfo
    Dim dtCustInforepair As DataTable ' CustomerInfo_Repair
    Dim dtAdvanceflag As DataTable ' Advance
    Dim dtGVInfo As DataTable ' GiftInfo
    Dim dtSales As New DataTable ' SALEs, PURCHASEs, SALEsReturn
    Dim dtApproval As New DataTable 'APPROVAL
    Dim dtOrderRepair As New DataTable ' Repair & Order
    Dim dsAdvanceReceipt As New DataSet ' Advance & Receipt
    Dim dsGiftvoucher As New DataSet ' GiftVoucher
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
    Dim QrcodeCustomerGf As Image = Nothing
    Dim PrintRPU_IPU As Boolean = False
    Dim strCompanyGst As String = ""
    Dim strEstPrinterName As String = GetAdmindbSoftValue("ESTPRINTER_" & SystemName, "")
    Dim _PRINTCPY As Boolean = IIf(GetAdmindbSoftValue("BILLPRINT_COPY", "N") = "Y", True, False)
    Dim SMALLBILLPRINT As String = GetAdmindbSoftValue("SMALLBILLPRINT_" & SystemName, "") ' SMALL DEFAULT PRINT
    Dim APPROVALBILLPRINT As String = GetAdmindbSoftValue("APPROVALBILLPRINT_" & SystemName, "") ' APPROVAL DEFAULT PRINT
    Dim FOOTER_BILLPRINT As String = GetAdmindbSoftValue("BILL_PRINT_FOOTER", "") ' BILLPRINT_FOOTER_TEXT  
    Dim printcpy As String = ""
    Dim Appsummary As Boolean = False ' APPROVAL SUMMARY PRINT
    Dim _smallprint_trantype As String = "" ' SMALLPRINT PRINT TRANTYPE
    Dim Duplicate As String
    Dim _top_flag As Boolean = False
    Dim _SILVERBILLPRINT As String = GetAdmindbSoftValue("SILVER_PRINT", "") '' SILVER DATABASE CHECK 
    Dim _SILVERBILLPRINT_HEADER As String = GetAdmindbSoftValue("SILVER_PRINT_HEADER", "") '' SILVER HEADER CHECK 
    Dim subputitle As Boolean = False
    Dim subsrtitle As Boolean = False
    Dim ac_pusrtitle As Boolean = False
    Dim gv_Cusname As String = ""
    Dim gvprint_flag As Boolean = True
    Dim gvprinted_flag As Boolean = False
    Dim smithfooter_flag As Boolean = False
    Dim smithCont_flag As Boolean = False
    Dim Print_Ver_No As String = "V:17.6.1"
    Dim BillPrint_SmithBal As Boolean = IIf(GetAdmindbSoftValue("BILLPRINT_SMITHBALANCE", "N") = "Y", True, False)
    Dim bMap As Bitmap
#End Region
    Public Sub pwdMaster(ByVal PBatchno As String)
        strsql = ""
        strsql += vbCrLf + " Select RIGHT(ISNULL(PWDMOBILENO,''),4)PWDMOBILENO"
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
        strsql = " SELECT SUM(CNT)CNT FROM (SELECT COUNT(*) CNT FROM " & cnStockDb & "..ISSUE WHERE "
        strsql += vbCrLf + " ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='D') AND TRANTYPE='SA' AND BATCHNO='" & batchno & "'"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT COUNT(*) CNT FROM " & cnStockDb & "..ISSSTONE WHERE STNITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID='D') AND TRANTYPE='SA' AND BATCHNO='" & batchno & "'"
        strsql += vbCrLf + " )X"
        If Val(objGPack.GetSqlValue(strsql, "", 0)) > 0 Then
            chkGuarantee.Checked = True
        Else
            chkGuarantee.Checked = False
        End If
        If Not Me.ShowDialog() = DialogResult.OK Then
            Exit Sub
        End If

        strsql = vbCrLf + "SELECT COUNT(*)CNT FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE='U' AND NAME ='BILLPRINT_LAYOUT' "
        If Val(GetSqlValue(cn, strsql).ToString) > 0 Then
            strsql = vbCrLf + "SELECT * FROM  " & cnAdminDb & "..BILLPRINT_LAYOUT "
            cmd = New OleDbCommand(strsql, cn)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dt1)
            If dt1.Rows.Count Then
                START_POS = Val(dt1.Compute("SUM(XPOS)", "NAME='START_POS'").ToString)
                TSTART_POS = Val(dt1.Compute("SUM(XPOS)", "NAME='START_POS'").ToString)
                h1 = Val(dt1.Compute("SUM(XPOS)", "NAME='h1'").ToString)
                c1 = Val(dt1.Compute("SUM(XPOS)", "NAME='c1'").ToString)
                c2 = Val(dt1.Compute("SUM(XPOS)", "NAME='c2'").ToString)
                c3 = Val(dt1.Compute("SUM(XPOS)", "NAME='c3'").ToString)
                c4 = Val(dt1.Compute("SUM(XPOS)", "NAME='c4'").ToString)
                c5 = Val(dt1.Compute("SUM(XPOS)", "NAME='c5'").ToString)
                c6 = Val(dt1.Compute("SUM(XPOS)", "NAME='c6'").ToString)
                c7 = Val(dt1.Compute("SUM(XPOS)", "NAME='c7'").ToString)
                c8 = Val(dt1.Compute("SUM(XPOS)", "NAME='c8'").ToString)
                c9 = Val(dt1.Compute("SUM(XPOS)", "NAME='c9'").ToString)
                BOTTOM_POS = Val(dt1.Compute("SUM(XPOS)", "NAME='BOTTOM_POS'").ToString)
                LINE_SPACE = Val(dt1.Compute("SUM(XPOS)", "NAME='LINE_SPACE'").ToString)
            End If
        End If

        ' Add any initialization after the InitializeComponent() call.
        pBatchno = batchno
        strsql = " SELECT DISTINCT ORTYPE  AS TRANTYPE FROM " & cnAdminDb & "..ORMAST "
        strsql += vbCrLf + " WHERE  BATCHNO='" & pBatchno & "' "
        Dim HEADPRINT As String = GetSqlValue(cn, strsql)
        If HEADPRINT = "O" Then 'ORDER 
            Trantype = "ORD"
            type = "ORD"
        ElseIf HEADPRINT = "R" Then 'REPAIR
            Trantype = "REP"
            type = "REP"
        Else
            Trantype = type ' CHECKING HEADLING ONLY
        End If
        _MIMR = Mimr
        _duplicate = duplicate

        Dim dr As DataRow = Nothing
        strsql = "SELECT ISNULL(GSTNO,'') EXCISENO,isnull(GSTNO,'') GSTNO  FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID = '" & strCompanyId & "'" 'SSB
        dr = GetSqlRow(strsql, cn)
        If Not dr Is Nothing Then
            GSTNO = dr.Item("EXCISENO").ToString
            COMPANYGSTNO = dr.Item("EXCISENO").ToString
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

        If B2C_QRCODE = True Then
            strsql = " Select  DISTINCT PNAME,TRANNO,TRANDATE,SUM(AMOUNT)AMOUNT,SUM(PCS)PCS,HSN FROM ("
            strsql += vbCrLf + " Select  (SELECT TOP 1 PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO IN "
            strsql += vbCrLf + " (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO)) AS PNAME, "
            strsql += vbCrLf + " TRANTYPE+'-'+ CONVERT(VARCHAR(10),TRANNO) TRANNO,TRANDATE,SUM(ISNULL(AMOUNT,0) + ISNULL(TAX,0))AMOUNT,SUM(PCS) PCS"
            strsql += vbCrLf + "  ,(SELECT TOP 1 HSN FROM " & cnAdminDb & "..ITEMMAST IM WHERE ITEMID=I.ITEMID) HSN"
            strsql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I WHERE "
            strsql += vbCrLf + " BATCHNO='" & batchno & "' GROUP BY TRANNO,TRANDATE,BATCHNO,TRANTYPE,I.ITEMID "
            strsql += vbCrLf + " UNION ALL "
            strsql += vbCrLf + " SELECT (SELECT TOP 1 PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO IN "
            strsql += vbCrLf + " (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO)) AS PNAME, "
            strsql += vbCrLf + " TRANTYPE+'-'+ CONVERT(VARCHAR(10),TRANNO) TRANNO,TRANDATE,SUM(ISNULL(AMOUNT,0) + ISNULL(TAX,0))AMOUNT,SUM(PCS) PCS"
            strsql += vbCrLf + "  ,(SELECT TOP 1 HSN FROM " & cnAdminDb & "..ITEMMAST IM WHERE ITEMID=I.ITEMID) HSN"
            strsql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT I WHERE "
            strsql += vbCrLf + " BATCHNO='" & batchno & "' "
            strsql += vbCrLf + " AND BATCHNO NOT IN (SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..ISSUE WHERE  "
            strsql += vbCrLf + " BATCHNO='" & batchno & "')"
            strsql += vbCrLf + " GROUP BY TRANNO,TRANDATE,BATCHNO,TRANTYPE,I.ITEMID "
            strsql += vbCrLf + " UNION ALL "
            strsql += vbCrLf + " SELECT (SELECT TOP 1 PNAME FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO IN "
            strsql += vbCrLf + " (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = I.BATCHNO)) AS PNAME, "
            strsql += vbCrLf + " TRANTYPE+RECPAY+'-'+ CONVERT(VARCHAR(10),TRANNO) TRANNO,TRANDATE,SUM(ISNULL(AMOUNT,0) + ISNULL(GSTVAL,0))AMOUNT, 0 PCS  ,NULL HSN"
            strsql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING I WHERE "
            strsql += vbCrLf + " BATCHNO='" & batchno & "' "
            strsql += vbCrLf + " AND BATCHNO NOT IN (SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..ISSUE WHERE  "
            strsql += vbCrLf + " BATCHNO='" & batchno & "')"
            strsql += vbCrLf + " AND BATCHNO NOT IN (SELECT DISTINCT BATCHNO FROM " & cnStockDb & "..RECEIPT WHERE  "
            strsql += vbCrLf + " BATCHNO='" & batchno & "')"
            strsql += vbCrLf + " GROUP BY TRANNO,TRANDATE,BATCHNO,TRANTYPE,RECPAY "
            strsql += vbCrLf + " )X GROUP BY PNAME,TRANNO,TRANDATE,HSN"
            Dim DT As DataRow = Nothing
            DT = GetSqlRow(strsql, cn, Nothing)
            If Not DT Is Nothing Then
                Dim strqrdata As String = "  Supplier_GST - " & strCompanyGst.ToString & vbCrLf & " Recipient_Name -" & DT!PNAME.ToString & vbCrLf & " Bill No - " & DT!TRANNO.ToString & vbCrLf & " Bill Date - " & DT!TRANDATE.ToString & vbCrLf & " BillAmount - " & DT!AMOUNT.ToString & vbCrLf & " No_Of_Line_items - " & DT!PCS.ToString
                strqrdata += " hsncode  " & DT!HSN.ToString & vbCrLf & " Payment_Link" & strCompanyGst.ToString & ""
                Dim gen As New QRCodeGenerator
                Dim qr_data = gen.CreateQrCode(strqrdata, QRCodeGenerator.ECCLevel.Q)
                Dim code As New QRCode(qr_data)
                QrcodeCustomer = code.GetGraphic(6)
            End If
        End If
        strBilldate = trandate
        Rules(0) = " No E-way bill Is required To be generated As the Goods covered under this Invoice are exempted As per "
        Rules(1) = " Serial No. 150/151 To the Annexure To Rule 138(14) Of the CGST Rules 2017 "
        If type = "POS" Then
            strsql = "Select COUNT(*)CNT FROM " & cnAdminDb & "..OUTSTANDING O WHERE BATCHNO='" & pBatchno & "'"
            strsql += vbCrLf + " AND PAYMODE IN('AR','AP','MR','DR','MP')"
            strsql += vbCrLf + " AND NOT EXISTS(SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE BATCHNO=O.BATCHNO"
            strsql += vbCrLf + " UNION SELECT 1 FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO=O.BATCHNO AND TRANTYPE IN ('SR','PU')"
            strsql += vbCrLf + " )"
            If Val(GetSqlValue_Bill(strsql, "CNT", 0)) > 0 Then
                strsql = "SELECT DISTINCT PAYMODE FROM " & cnAdminDb & "..OUTSTANDING O WHERE BATCHNO='" & pBatchno & "' AND PAYMODE IN('AR','DR')"
                da = New OleDbDataAdapter(strsql, cn)
                dtAdvanceflag = New DataTable
                da.Fill(dtAdvanceflag)
                Dim count1 As String = ""
                Dim count2 As String = ""
                If dtAdvanceflag.Rows.Count > 0 Then
                    count1 = Trim(dtAdvanceflag.Rows(0).Item("PAYMODE") & "").ToString
                    If dtAdvanceflag.Rows.Count > 1 Then
                        count2 = Trim(dtAdvanceflag.Rows(1).Item("PAYMODE") & "").ToString
                    End If
                End If
                printCustoInfo = True
                If count2 <> "" Or count1 = "AR" Then
                    strsql = "SELECT TOP 1 PAYMODE FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = '" & strBilldate & "' AND BATCHNO ='" & pBatchno & "' AND   PAYMODE IN ('CC','CH','CA')"
                    Dim CACQCR As String = GetSqlValue(cn, strsql)
                    If CACQCR = "" Then
                        AdvancePrint(pBatchno, strBilldate, "AR")
                    Else
                        AdvancePrint(pBatchno, strBilldate, "AR", "Y")
                    End If
                    PrtDiaAdvance.Document = PrintAdvance
                    PrtDiaAdvance.PrinterSettings.PrinterName = cmbPrinte_Name.Text
                    PrintAdvance.PrinterSettings.Copies = 1
                    PrintAdvance.PrintController = New System.Drawing.Printing.StandardPrintController()
                    If ChkCusCopy.Checked Then printcpy = "CUSTOMER COPY" : PrintAdvance.Print()
                    If ChkOffCopy.Checked Then
                        printcpy = "ACCOUNTS COPY "
                        START_POS = TSTART_POS
                        PagecountSale = 0
                        PrintAdvance.Print()
                    End If
                Else
                    AdvancePrint(pBatchno, strBilldate)
                    PrtDiaAdvance.Document = PrintAdvance
                    PrtDiaAdvance.PrinterSettings.PrinterName = cmbPrinte_Name.Text
                    PrintAdvance.PrinterSettings.Copies = 1
                    PrintAdvance.PrintController = New System.Drawing.Printing.StandardPrintController()
                    If ChkCusCopy.Checked Then printcpy = "CUSTOMER COPY" : PrintAdvance.Print()
                    If ChkOffCopy.Checked Then
                        printcpy = "ACCOUNTS COPY "
                        START_POS = TSTART_POS
                        PagecountSale = 0
                        PrintAdvance.Print()
                    End If
                End If


            Else
                strsql = "SELECT COUNT(*)CNT FROM " & cnAdminDb & "..OUTSTANDING O WHERE BATCHNO='" & pBatchno & "'"
                strsql += vbCrLf + " AND PAYMODE IN('AR','AP','MR','DR','MP')"
                strsql += vbCrLf + " AND EXISTS( SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE BATCHNO=O.BATCHNO UNION "
                strsql += vbCrLf + " SELECT 1 FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO=O.BATCHNO AND TRANTYPE IN ('SR','PU')"
                strsql += vbCrLf + " )"
                If Val(GetSqlValue_Bill(strsql, "CNT", 0)) > 0 Then
                    Print(pBatchno)
                    START_POS = TSTART_POS
                    printCustoInfo = True

                    'AdvancePrint(pBatchno, strBilldate)
                    'PrtDiaAdvance.Document = PrintAdvance
                    'PrtDiaAdvance.PrinterSettings.PrinterName = cmbPrinte_Name.Text
                    'PrintAdvance.PrinterSettings.Copies = 1
                    'PrintAdvance.PrintController = New System.Drawing.Printing.StandardPrintController()
                    'If ChkCusCopy.Checked Then printcpy = "CUSTOMER COPY" : START_POS = TSTART_POS : PagecountSale = 0 : PrintAdvance.Print()
                    'If ChkOffCopy.Checked Then
                    '    printcpy = "ACCOUNTS COPY"
                    '    START_POS = TSTART_POS
                    '    PagecountSale = 0
                    '    printCustoInfo = True
                    '    PrintAdvance.Print()
                    'End If

                    strsql = "SELECT PAYMODE FROM " & cnAdminDb & "..OUTSTANDING O WHERE BATCHNO='" & pBatchno & "' AND PAYMODE IN('AR','DR')"
                    da = New OleDbDataAdapter(strsql, cn)
                    dtAdvanceflag = New DataTable
                    da.Fill(dtAdvanceflag)
                    Dim count1 As String = ""
                    Dim count2 As String = ""
                    If dtAdvanceflag.Rows.Count > 0 Then
                        count1 = Trim(dtAdvanceflag.Rows(0).Item("PAYMODE") & "").ToString
                        If dtAdvanceflag.Rows.Count > 1 Then
                            count2 = Trim(dtAdvanceflag.Rows(1).Item("PAYMODE") & "").ToString
                        End If
                    End If
                    printCustoInfo = True
                    If count2 <> "" Or count1 = "AR" Then
                        strsql = "SELECT TOP 1 PAYMODE FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = '" & strBilldate & "' AND BATCHNO ='" & pBatchno & "' AND   PAYMODE IN ('CC','CH','CA')"
                        Dim CACQCR As String = GetSqlValue(cn, strsql)
                        If CACQCR = "" Then
                            AdvancePrint(pBatchno, strBilldate, "AR")
                        Else
                            AdvancePrint(pBatchno, strBilldate, "AR", "Y")
                        End If
                        PrtDiaAdvance.Document = PrintAdvance
                        PrtDiaAdvance.PrinterSettings.PrinterName = cmbPrinte_Name.Text
                        PrintAdvance.PrinterSettings.Copies = 1
                        PrintAdvance.PrintController = New System.Drawing.Printing.StandardPrintController()
                        If ChkCusCopy.Checked Then printcpy = "CUSTOMER COPY" : PrintAdvance.Print()
                        If ChkOffCopy.Checked Then
                            printcpy = "ACCOUNTS COPY "
                            START_POS = TSTART_POS
                            PagecountSale = 0
                            PrintAdvance.Print()
                        End If
                    Else
                        AdvancePrint(pBatchno, strBilldate)
                        PrtDiaAdvance.Document = PrintAdvance
                        PrtDiaAdvance.PrinterSettings.PrinterName = cmbPrinte_Name.Text
                        PrintAdvance.PrinterSettings.Copies = 1
                        PrintAdvance.PrintController = New System.Drawing.Printing.StandardPrintController()
                        If ChkCusCopy.Checked Then printcpy = "CUSTOMER COPY" : PrintAdvance.Print()
                        If ChkOffCopy.Checked Then
                            printcpy = "ACCOUNTS COPY "
                            START_POS = TSTART_POS
                            PagecountSale = 0
                            PrintAdvance.Print()
                        End If
                    End If
                Else
                    Print(pBatchno)
                End If
            End If
        ElseIf type = "ACC" Then
            Print(pBatchno)
        ElseIf type = "GST" Then
            Print(pBatchno)
        ElseIf type = "ORD" Or type = "REP" Then
            OrderReceipt(pBatchno, type, strBilldate)
            PrtDiaOrder.Document = PrintOrder
            PrtDiaOrder.PrinterSettings.PrinterName = cmbPrinte_Name.Text
            PrintOrder.PrinterSettings.Copies = 1
            PrintOrder.PrintController = New System.Drawing.Printing.StandardPrintController()
            If ChkCusCopy.Checked Then printcpy = "CUSTOMER COPY" : PrintOrder.Print()
            If ChkOffCopy.Checked Then
                printcpy = "ACCOUNTS COPY"
                START_POS = TSTART_POS
                PagecountSale = 0
                PrintOrder.Print()
            End If
        ElseIf type = "EST" Then ' EST- > PURCHASE & SALES
            If pBatchno.Contains(":") Then
                Dim strEstNo() As String = pBatchno.Split(":")
                EstNo_SA = strEstNo(0).Replace("S.", "")
                EstNo_PU = strEstNo(1).Replace("P.", "")
            End If
            If Val(EstNo_SA.ToString) <> 0 Then
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
                PrintDialog2.Document = PrintDocument1
                PrintDialog2.PrinterSettings.PrinterName = cmbPrinte_Name.Text
                PrintDocument1.PrinterSettings.Copies = 1
                PrintDocument1.PrintController = New System.Drawing.Printing.StandardPrintController
                PrintDocument1.Print()
            Else
                PrintDialog2.Document = PrintDocument1
                PrintDialog2.PrinterSettings.PrinterName = cmbPrinte_Name.Text
                PrintDocument1.PrinterSettings.Copies = 1
                PrintDocument1.PrintController = New System.Drawing.Printing.StandardPrintController
                PrintDocument1.Print()
            End If
        ElseIf type = "SMI" Or type = "SMR" Then
            smithIssueReceipt(pBatchno, "", Mimr)
            pwdMaster(pBatchno)
            If Mimr = "NEW" Then
                Dim PrintRPU As Boolean = False
                PrintRPU_IPU = False
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
                        START_POS = START_POSSMITHISSUEVALUE
                        boolPrintSmithA4Issue = True
                        boolPrintSmithA4IssueTax = True
                        PrtDiaA4First.Document = PrtSmithA4First
                        PrtDiaA4First.PrinterSettings.PrinterName = cmbPrinte_Name.Text
                        PrtSmithA4First.PrinterSettings.Copies = 1
                        PrtSmithA4First.PrintController = New System.Drawing.Printing.StandardPrintController
                        PrtSmithA4First.Print()
                    Else
                        PagecountSale = 0
                        START_POS = START_POSSMITHISSUEVALUE
                        boolPrintSmithA4Issue = True
                        boolPrintSmithA4IssueTax = True
                        PrtDiaA4First.Document = PrtSmithA4First
                        PrtDiaA4First.PrinterSettings.PrinterName = cmbPrinte_Name.Text
                        PrtSmithA4First.PrinterSettings.Copies = 1
                        PrtSmithA4First.PrintController = New System.Drawing.Printing.StandardPrintController
                        PrtSmithA4First.Print()
                    End If

                    If PrintRPU = True Then
                    ElseIf PrintRPU_IPU = True Then
                    Else
                        PrintRPU_IPU = False
                        PagecountSale = 0
                        START_POS = START_POSSMITHISSUEVALUE
                        boolPrintSmithA4Issue = True
                        boolPrintSmithA4IssueTax = True
                        PrtDiaA4Second.Document = PrtSmithA4Second
                        PrtDiaA4Second.PrinterSettings.PrinterName = cmbPrinte_Name.Text
                        PrtSmithA4Second.PrinterSettings.Copies = 1
                        PrtSmithA4Second.PrintController = New System.Drawing.Printing.StandardPrintController
                        PrtSmithA4Second.Print()
                    End If

                    PrintRPU_IPU = False
                    PagecountSale = 0
                    START_POS = START_POSSMITHISSUEVALUE
                    boolPrintSmithA4Issue = True
                    boolPrintSmithA4IssueTax = True
                    PrtDiaA4Third.Document = PrtSmithA4Third
                    PrtDiaA4Third.PrinterSettings.PrinterName = cmbPrinte_Name.Text
                    PrtSmithA4Third.PrinterSettings.Copies = 1
                    PrtSmithA4Third.PrintController = New System.Drawing.Printing.StandardPrintController
                    PrtSmithA4Third.Print()

                    PrintRPU_IPU = False
                    PagecountSale = 0
                    START_POS = START_POSSMITHISSUEVALUE
                    boolPrintSmithA4Issue = True
                    boolPrintSmithA4IssueTax = True
                    PrtDiaA4Fourth.Document = PrtSmithA4Fourth
                    PrtDiaA4Fourth.PrinterSettings.PrinterName = cmbPrinte_Name.Text
                    PrtSmithA4Fourth.PrinterSettings.Copies = 1
                    PrtSmithA4Fourth.PrintController = New System.Drawing.Printing.StandardPrintController
                    PrtSmithA4Fourth.Print()

                Else
                    PrtDisSmith2.Document = PrtSmith2
                    PrtDisSmith2.PrinterSettings.PrinterName = cmbPrinte_Name.Text
                    PrtSmith2.PrinterSettings.Copies = 1
                    PrtSmith2.PrintController = New System.Drawing.Printing.StandardPrintController
                    PrtSmith2.Print()
                End If
            Else
                PrtDiaSmith.Document = PrtSmith
                PrtDiaSmith.PrinterSettings.PrinterName = cmbPrinte_Name.Text
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
    Public Sub smithIssueReceipt(ByVal BatchNo As String, ByVal Trantype As String, ByVal mimrformat As String) '' SIRE
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
        strsql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE_BAL]','U') "
        strsql += vbCrLf + " IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE_BAL]"
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
        strsql += vbCrLf + " ,TRANNO AS ISSUENO "
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
        strsql += vbCrLf + " AMOUNT-ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO),0) AMOUNT,"
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
        strsql += vbCrLf + " ,'RECEIPT                                     ' TABLENAME"
        strsql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL) TAXID "
        strsql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NULL) TAXPER "
        strsql += vbCrLf + " ,I.CATCODE"
        strsql += vbCrLf + " ,'A' OTABLENAME,I.FLAG"
        strsql += vbCrLf + " INTO TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE]"
        strsql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT I "
        strsql += vbCrLf + " WHERE TRANDATE = '" & strBilldate & "' "
        strsql += vbCrLf + "  AND BATCHNO = '" & BatchNo & "' "
        strsql += vbCrLf + " AND TRANTYPE IN ('RRE','RMP','RAP','RIN','ROT','RDN') " ' RECEIPT
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        Dim cntrecet As Double = 0
        strsql = "SELECT COUNT(*) CNT FROM  TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE]"
        cntrecet = Val(GetSqlValue(cn, strsql).ToString)

        'ISSUE & RECEIPT PURCHASE
        strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE] "
        strsql += vbCrLf + " SELECT ROW_NUMBER() OVER(ORDER BY TRANNO DESC) AS SNO,"
        strsql += vbCrLf + " TRANNO AS ISSUENO , "
        strsql += vbCrLf + " CONVERT(VARCHAR(15),TRANDATE,103) AS DATE,"
        strsql += vbCrLf + " (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE) AS CATNAME,"
        strsql += vbCrLf + " CASE WHEN ISNULL((SELECT HSN FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID), '') <> '' THEN "
        strsql += vbCrLf + " ISNULL((SELECT HSN FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID), '') "
        strsql += vbCrLf + " ELSE (SELECT HSN FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)  END HSN, "
        strsql += vbCrLf + " (SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = I.USERID) USERNAME,"
        strsql += vbCrLf + " CASE WHEN ALLOY > 0 THEN 'ADD : ALLOY' ELSE '' END  AS ALLOYNAME,"
        strsql += vbCrLf + " CASE WHEN ALLOY > 0 THEN (CONVERT(NUMERIC(15,3), ALLOY)) ELSE 0 END  AS ALLOY,"
        strsql += vbCrLf + " CASE WHEN  WASTAGE > 0 then 'WASTAGE' ELSE '' END AS WASTAGENAME,"
        strsql += vbCrLf + " CASE WHEN  WASTAGE > 0 then WASTAGE ELSE 0 END AS WASTAGE, "
        strsql += vbCrLf + " CASE WHEN PCS > 0 THEN PCS END AS PCS,"
        strsql += vbCrLf + " CASE WHEN GRSWT > 0 THEN (CONVERT(NUMERIC(15,3), GRSWT)) END AS GRSWT,"
        strsql += vbCrLf + " CASE WHEN NETWT > 0 THEN (CONVERT(NUMERIC(15,3), NETWT))  END AS NETWT,"
        strsql += vbCrLf + " CASE WHEN RATE > 0 THEN (CONVERT(NUMERIC(15,2), RATE)) END AS RATE,"
        strsql += vbCrLf + " (SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT,"
        strsql += vbCrLf + " CASE WHEN MCHARGE > 0 THEN MCHARGE END AS MC,"
        strsql += vbCrLf + " CASE WHEN TAX > 0 THEN TAX ELSE 0 END AS TAX,"
        strsql += vbCrLf + " AMOUNT-ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO),0) AMOUNT,"
        strsql += vbCrLf + " ACCODE,REMARK1,REMARK2"
        If mimrformat = "NEW" Then
            strsql += vbCrLf + " ,(CASE WHEN TRANTYPE = 'IIS' THEN '' ELSE CASE WHEN TRANTYPE = 'IMP' THEN '' ELSE '' END END) TRANTYPE"
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
        strsql += vbCrLf + " ,'ISSUE                                 ' TABLENAME"
        strsql += vbCrLf + " ,NULL TAXID, NULL TAXPER "
        strsql += vbCrLf + " ,I.CATCODE CATCODE"
        strsql += vbCrLf + " ,'B' OTABLENAME,I.FLAG"
        strsql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I "
        strsql += vbCrLf + " WHERE TRANDATE = '" & strBilldate & "' "
        strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
        strsql += vbCrLf + " AND TRANTYPE IN('IIS','IAP','IPU','IMP','IIN','IOT','IDN') " ' ISSUE 

        strsql += vbCrLf + " UNION ALL"

        strsql += vbCrLf + " SELECT NULL SNO,"
        strsql += vbCrLf + " NULL ISSUENO , "
        strsql += vbCrLf + " NULL AS DATE,"
        strsql += vbCrLf + " (SELECT  ITEMNAME FROM " & cnAdminDb & "..ITEMMAST  WHERE ITEMID = X.STNITEMID ) AS CATNAME,"
        strsql += vbCrLf + " NULL HSN, "
        strsql += vbCrLf + " NULL USERNAME,"
        strsql += vbCrLf + " NULL  AS ALLOYNAME,"
        strsql += vbCrLf + " 0  AS ALLOY,"
        strsql += vbCrLf + " NULL AS WASTAGENAME,"
        strsql += vbCrLf + " NULL WASTAGE, "
        strsql += vbCrLf + "  PCS,"
        strsql += vbCrLf + "  GRSWT,"
        strsql += vbCrLf + "  NETWT,"
        strsql += vbCrLf + " NULL RATE,"
        strsql += vbCrLf + " NULL AS DIAWT,"
        strsql += vbCrLf + " NULL AS MC,"
        strsql += vbCrLf + " NULL TAX,"
        strsql += vbCrLf + " STNAMT  AMOUNT,"
        strsql += vbCrLf + " NULL ACCODE,NULL REMARK1,NULL REMARK2,  (CASE WHEN STONEUNIT='G' THEN 'gms' ELSE 'cts' END) TRANTYPE " '' SIRE
        strsql += vbCrLf + " , RESULT"
        strsql += vbCrLf + " , TRANTYPE TTYPE "
        strsql += vbCrLf + " , 0 TOUCH"
        strsql += vbCrLf + " , NULL PURITY"
        strsql += vbCrLf + " , NULL PUREWT"
        strsql += vbCrLf + " , NULL LESSWT"
        strsql += vbCrLf + " , NULL DIAAMT"
        strsql += vbCrLf + " , '' ITEMNAME"
        strsql += vbCrLf + " , 2 TYPE"
        strsql += vbCrLf + " ,'ZZZZZZZZ' [SERNO]"
        strsql += vbCrLf + " ,CASE WHEN TRANTYPE='RPU' THEN 'RECEIPT'  WHEN TRANTYPE='IPU'  THEN 'PURCHASE' WHEN  TRANTYPE='IAP'  THEN 'APPROVAL' ELSE 'ISSUE' END  TABLENAME"
        strsql += vbCrLf + " ,0 TAXID,0 TAXPER "
        strsql += vbCrLf + " ,NULL CATCODE"
        strsql += vbCrLf + " ,CASE WHEN TRANTYPE='RPU'  THEN 'R' WHEN TRANTYPE='RRE'  THEN 'R' WHEN  TRANTYPE='RAP'  THEN 'R'   ELSE 'I' END  OTABLENAME,NULL FLAG"
        strsql += vbCrLf + "  FROM ( "
        strsql += vbCrLf + " SELECT  STNITEMID,SUM(STNPCS)PCS,SUM(STNWT)GRSWT,SUM(STNWT)NETWT,SUM(STNAMT)STNAMT,BATCHNO,5 RESULT  ,TRANTYPE,STONEUNIT "
        strsql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE WHERE TRANDATE = '" & strBilldate & "' "
        strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
        strsql += vbCrLf + " AND TRANTYPE IN ('IIS','IAP','IPU','IMP','IIN','IOT','IDN') " ' ISSUE
        strsql += vbCrLf + "  GROUP BY STNITEMID,BATCHNO,TRANTYPE,STONEUNIT"
        strsql += vbCrLf + " )X"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT ROW_NUMBER() OVER(ORDER BY TRANNO DESC) AS SNO,"
        strsql += vbCrLf + " TRANNO AS ISSUENO , "
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
        strsql += vbCrLf + " AMOUNT-ISNULL((SELECT SUM(STNAMT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO),0) AMOUNT,"
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
        strsql += vbCrLf + " ,'RECEIPT                                    ' TABLENAME"
        strsql += vbCrLf + " ,NULL TAXID, NULL TAXPER "
        strsql += vbCrLf + " ,I.CATCODE CATCODE"
        strsql += vbCrLf + " ,'A' OTABLENAME,I.FLAG"
        strsql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT I "
        strsql += vbCrLf + " WHERE TRANDATE = '" & strBilldate & "' "
        strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
        strsql += vbCrLf + " AND TRANTYPE IN ('RPU') " ' RPU - > PURCHASE VOUCHER

        strsql += vbCrLf + " UNION ALL"

        strsql += vbCrLf + " SELECT NULL SNO,"
        strsql += vbCrLf + " NULL ISSUENO , "
        strsql += vbCrLf + " NULL AS DATE,"
        strsql += vbCrLf + " (SELECT  ITEMNAME FROM " & cnAdminDb & "..ITEMMAST  WHERE ITEMID = X.STNITEMID ) AS CATNAME,"
        strsql += vbCrLf + " NULL HSN, "
        strsql += vbCrLf + " NULL USERNAME,"
        strsql += vbCrLf + " NULL  AS ALLOYNAME,"
        strsql += vbCrLf + " 0  AS ALLOY,"
        strsql += vbCrLf + " NULL AS WASTAGENAME,"
        strsql += vbCrLf + " NULL WASTAGE, "
        strsql += vbCrLf + "  PCS,"
        strsql += vbCrLf + "  GRSWT,"
        strsql += vbCrLf + "  NETWT,"
        strsql += vbCrLf + " NULL RATE,"
        strsql += vbCrLf + " NULL AS DIAWT,"
        strsql += vbCrLf + " NULL AS MC,"
        strsql += vbCrLf + " NULL TAX,"
        strsql += vbCrLf + " STNAMT  AMOUNT,"
        strsql += vbCrLf + " NULL ACCODE,NULL REMARK1,NULL REMARK2,  (CASE WHEN STONEUNIT='G' THEN 'gms' ELSE 'cts' END) TRANTYPE "
        strsql += vbCrLf + " , RESULT"
        strsql += vbCrLf + " , TRANTYPE TTYPE "
        strsql += vbCrLf + " , 0 TOUCH"
        strsql += vbCrLf + " , NULL PURITY"
        strsql += vbCrLf + " , NULL PUREWT"
        strsql += vbCrLf + " , NULL LESSWT"
        strsql += vbCrLf + " , NULL DIAAMT"
        strsql += vbCrLf + " , '' ITEMNAME"
        strsql += vbCrLf + " , 2 TYPE"
        strsql += vbCrLf + " ,'ZZZZZZZZ' [SERNO]"
        strsql += vbCrLf + " ,CASE WHEN TRANTYPE='RRE' THEN 'RECEIPT' WHEN TRANTYPE='RPU'  THEN 'PURCHASE' WHEN  TRANTYPE='RAP'  THEN 'APPROVAL' ELSE 'ISSUE' END  TABLENAME"
        strsql += vbCrLf + " ,0 TAXID,0 TAXPER "
        strsql += vbCrLf + " ,NULL CATCODE"
        strsql += vbCrLf + " ,CASE WHEN TRANTYPE='RPU'  THEN 'R' WHEN TRANTYPE='RRE'  THEN 'R' WHEN  TRANTYPE='RAP'  THEN 'R'  ELSE 'I' END  OTABLENAME,NULL FLAG"
        strsql += vbCrLf + "  FROM ( "
        strsql += vbCrLf + " SELECT   STNITEMID,SUM(STNPCS)PCS,SUM(STNWT)GRSWT,SUM(STNWT)NETWT,SUM(STNAMT)STNAMT,BATCHNO,6 RESULT ,TRANTYPE,STONEUNIT   "
        strsql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE WHERE TRANDATE = '" & strBilldate & "' "
        strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
        strsql += vbCrLf + " AND   TRANTYPE IN ('RRE','RMP','RAP','RIN','ROT','RDN','RPU') " ' RECEIPT
        strsql += vbCrLf + " GROUP BY STNITEMID,BATCHNO,TRANTYPE,STONEUNIT"
        strsql += vbCrLf + " )X"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
        If mimrformat = "NEW" Then
            If mimrformat = "NEW" Then
                'GST CALCUATION RECEIPT
                strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE] "
                strsql += vbCrLf + " ( "
                strsql += vbCrLf + " SNO,ISSUENO,DATE,CATNAME,HSN,USERNAME,ALLOYNAME,ALLOY,WASTAGENAME"
                strsql += vbCrLf + " ,WASTAGE,PCS,GRSWT,NETWT,RATE,DIAWT,MC,TAX"
                strsql += vbCrLf + " ,AMOUNT,ACCODE,REMARK1,REMARK2,TRANTYPE,RESULT,TTYPE,TOUCH,PURITY,PUREWT,LESSWT,DIAAMT"
                strsql += vbCrLf + " ,ITEMNAME,TYPE,SERNO,TABLENAME,TAXID,TAXPER,CATCODE,OTABLENAME,FLAG "
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
                strsql += vbCrLf + " ,2 TYPE"
                strsql += vbCrLf + " ,'ZZZZZZZZ' [SERNO]"
                strsql += vbCrLf + " ,'RECEIPT' TABLENAME"
                strsql += vbCrLf + " ,TAXID,TAXPER "
                strsql += vbCrLf + " ,NULL CATCODE"
                strsql += vbCrLf + " ,'A' OTABLENAME,NULL FLAG"
                strsql += vbCrLf + "  FROM ( "
                strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT)TAXAMOUNT,BATCHNO  "
                strsql += vbCrLf + " ,CASE WHEN TAXTYPE = 'RG' THEN 'GST Reversal on wastage' ELSE '' END TAXTYPENAME"
                strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
                strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
                strsql += vbCrLf + " AND TAXID = 'CG' "
                strsql += vbCrLf + " AND ISNULL(TAXTYPE,'') <> 'RG'"
                strsql += vbCrLf + " AND TRANTYPE IN ('RRE','RMP','RAP','RIN','ROT','RDN') " ' RECEIPT
                strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TAXTYPE"
                strsql += vbCrLf + " UNION ALL"
                strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO "
                strsql += vbCrLf + " ,CASE WHEN TAXTYPE = 'RG' THEN 'GST Reversal on wastage' ELSE '' END TAXTYPENAME"
                strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
                strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
                strsql += vbCrLf + " AND TAXID = 'SG' "
                strsql += vbCrLf + " AND ISNULL(TAXTYPE,'') <> 'RG'"
                strsql += vbCrLf + " AND TRANTYPE IN ('RRE','RMP','RAP','RIN','ROT','RDN') " ' RECEIPT
                strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TAXTYPE"
                strsql += vbCrLf + " UNION ALL"
                strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO "
                strsql += vbCrLf + " ,CASE WHEN TAXTYPE = 'RG' THEN 'GST Reversal on wastage' ELSE '' END TAXTYPENAME"
                strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
                strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
                strsql += vbCrLf + " AND TAXID = 'IG' "
                strsql += vbCrLf + " AND ISNULL(TAXTYPE,'') <> 'RG'"
                strsql += vbCrLf + " AND TRANTYPE IN ('RRE','RMP','RAP','RIN','ROT','RDN') " ' RECEIPT
                strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TAXTYPE"
                strsql += vbCrLf + " UNION ALL"
                strsql += vbCrLf + " SELECT 'TC' TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO, '' TAXTYPENAME "
                strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
                strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
                strsql += vbCrLf + " AND TAXID = 'TC' "
                strsql += vbCrLf + " AND TRANTYPE IN ('RRE','RMP','RAP','RIN','ROT','RDN') " ' RECEIPT
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
                    strsql += vbCrLf + " ,ITEMNAME,TYPE,SERNO,TABLENAME,TAXID,TAXPER,CATCODE,OTABLENAME,FLAG "
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
                    strsql += vbCrLf + " ,'A' OTABLENAME,NULL FLAG"
                    cmd = New OleDbCommand(strsql, cn)
                    cmd.ExecuteNonQuery()
                End If
            End If
            'GST CALCUATION RECEIPT
            strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE] "
            strsql += vbCrLf + " ( "
            strsql += vbCrLf + " SNO,ISSUENO,DATE,CATNAME,HSN,USERNAME,ALLOYNAME,ALLOY,WASTAGENAME"
            strsql += vbCrLf + " ,WASTAGE,PCS,GRSWT,NETWT,RATE,DIAWT,MC,TAX"
            strsql += vbCrLf + " ,AMOUNT,ACCODE,REMARK1,REMARK2,TRANTYPE,RESULT,TTYPE,TOUCH,PURITY,PUREWT,LESSWT,DIAAMT"
            strsql += vbCrLf + " ,ITEMNAME,TYPE,SERNO,TABLENAME "
            strsql += vbCrLf + " ,TAXID,TAXPER,CATCODE,OTABLENAME,FLAG "
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
            strsql += vbCrLf + " ,CASE WHEN TRANTYPE='RPU' THEN 'A' ELSE 'B' END  OTABLENAME,NULL FLAG"
            strsql += vbCrLf + "  FROM ( "
            strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT)TAXAMOUNT,BATCHNO,TRANTYPE  "
            strsql += vbCrLf + " ,CASE WHEN TAXTYPE = 'RG' THEN 'GST Reversal on wastage' ELSE '' END TAXTYPENAME"
            strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
            strsql += vbCrLf + " AND TAXID = 'CG' "
            strsql += vbCrLf + " AND ISNULL(TAXTYPE,'') <> 'RG'"
            strsql += vbCrLf + " AND TRANTYPE IN ('IIS','IAP','IPU','IMP','RPU','IIN','RIN','IOT','ROT','IDN','RDN') " ' RECEIPT
            strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TAXTYPE,TRANTYPE"
            strsql += vbCrLf + " UNION ALL"
            strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO,TRANTYPE "
            strsql += vbCrLf + " ,CASE WHEN TAXTYPE = 'RG' THEN 'GST Reversal on wastage' ELSE '' END TAXTYPENAME"
            strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
            strsql += vbCrLf + " AND TAXID = 'SG' "
            strsql += vbCrLf + " AND ISNULL(TAXTYPE,'') <> 'RG'"
            strsql += vbCrLf + " AND TRANTYPE IN ('IIS','IAP','IPU','IMP','RPU','IIN','RIN','IOT','ROT','IDN','RDN') " ' RECEIPT
            strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TAXTYPE,TRANTYPE"
            strsql += vbCrLf + " UNION ALL"
            strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO,TRANTYPE "
            strsql += vbCrLf + " ,CASE WHEN TAXTYPE = 'RG' THEN 'GST Reversal on wastage' ELSE '' END TAXTYPENAME"
            strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
            strsql += vbCrLf + " AND TAXID = 'IG' "
            strsql += vbCrLf + " AND ISNULL(TAXTYPE,'') <> 'RG'"
            strsql += vbCrLf + " AND TRANTYPE IN ('IIS','IAP','IPU','IMP','RPU','IIN','RIN','IOT','ROT','IDN','RDN') " ' RECEIPT
            strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TAXTYPE,TRANTYPE"
            strsql += vbCrLf + " UNION ALL"
            strsql += vbCrLf + " SELECT 'TC' TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO,TRANTYPE "
            strsql += vbCrLf + " ,'' TAXTYPENAME "
            strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
            strsql += vbCrLf + " AND TAXID = 'TC' "
            strsql += vbCrLf + " AND TRANTYPE IN ('IIS','IAP','IPU','IMP','RPU','IIN','RIN','IOT','ROT','IDN','RDN') " ' ISSUE
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
        strsql += vbCrLf + " SELECT 'TOTAL' AS TOTAL "
        strsql += vbCrLf + ",SUM(PCS) PCS " '+ ISNULL((SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE BATCHNO = A.BATCHNO),0)
        If mimrformat = "NEW" Then
            strsql += vbCrLf + " ,SUM(GRSWT) + SUM(WASTAGE) + SUM(ALLOY)  AS GRSWT  " '+SUM(WASTAGE)+SUM(ALLOY)
            strsql += vbCrLf + " ,SUM(NETWT) + SUM(WASTAGE) + SUM(ALLOY)  AS NETWT " '+SUM(WASTAGE)+SUM(ALLOY)
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
        strsql += vbCrLf + " AND TRANTYPE IN('RRE','RMP','RAP','RIN') " ' RECEIPT TOTAL
        strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
        strsql += vbCrLf + " GROUP BY BATCHNO"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT 'TOTAL' AS TOTAL"
        strsql += vbCrLf + " ,SUM(PCS) PCS " '+ ISNULL((SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE BATCHNO = A.BATCHNO),0)
        If mimrformat = "NEW" Then
            strsql += vbCrLf + " ,SUM(GRSWT) + SUM(WASTAGE)+ SUM(ALLOY) AS GRSWT " '+SUM(WASTAGE)+SUM(ALLOY)
            strsql += vbCrLf + " ,SUM(NETWT) + SUM(WASTAGE)+ SUM(ALLOY) AS NETWT " '+SUM(WASTAGE)+SUM(ALLOY)
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
        strsql += vbCrLf + " SELECT 'TOTAL' AS TOTAL "
        strsql += vbCrLf + " ,SUM(PCS) PCS " '+ ISNULL((SELECT SUM(STNPCS) FROM " & cnStockDb & "..ISSSTONE WHERE BATCHNO = A.BATCHNO),0)
        If mimrformat = "NEW" Then
            strsql += vbCrLf + " , SUM(GRSWT) + SUM(WASTAGE) + SUM(ALLOY)  AS GRSWT " '+SUM(WASTAGE)+SUM(ALLOY)
            strsql += vbCrLf + " , SUM(NETWT) + SUM(WASTAGE) + SUM(ALLOY) AS NETWT " '+SUM(WASTAGE)+SUM(ALLOY)
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
        strsql += vbCrLf + " AND TRANTYPE IN('IIS','IAP','IPU','IMP','IIN') " ' ISSUE TOTAL
        strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "'"
        strsql += vbCrLf + " GROUP BY BATCHNO"
        strsql += vbCrLf + " ) X"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        If mimrformat = "NEW" Then

            strsql = vbCrLf + "SELECT TAXID,TAXPER,SUM(TAXAMOUNT)TAXAMOUNT,BATCHNO,TAXTYPENAME  INTO TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE_TAXTRAN] FROM ( "
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
            strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT)*-1 TAXAMOUNT,BATCHNO,'' TAXTYPENAME "
            strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
            strsql += vbCrLf + " AND TAXID IN (SELECT DISTINCT CAST(TDSCATID AS varchar) FROM " & cnAdminDb & "..TDSCATEGORY) "
            strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO"
            strsql += vbCrLf + " )X GROUP BY X.TAXID,X.TAXPER,X.BATCHNO,X.TAXTYPENAME"
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

        strsql = vbCrLf + " SELECT COSTID FROM " & cnStockDb & "..RECEIPT "
        strsql += vbCrLf + " WHERE TRANDATE = '" & strBilldate & "' AND BATCHNO = '" & BatchNo & "' "
        strsql += vbCrLf + " UNION "
        strsql += vbCrLf + " SELECT COSTID FROM " & cnStockDb & "..ISSUE "
        strsql += vbCrLf + " WHERE TRANDATE = '" & strBilldate & "' AND BATCHNO = '" & BatchNo & "' "
        Dim _tempCostid As String = GetSqlValue_Bill(strsql)


        '''''''''''''''''''''''''''''''''''''''''''''''
        '''''' /* SMITHI BALANCE PART */ '''''''
        '''''''''''''''''''''''''''''''''''''''''''''''

        strsql = vbCrLf + "SELECT SUM(GRSWT) AS CLOSING_GRSWT,X.ACCODE,A.ACNAME ,(SELECT METALID FROM " & cnAdminDb & "..CATEGORY C WHERE C.CATCODE=X.CATCODE )AS METALID,X.STONEUNIT STONE_TYPE"
        strsql += vbCrLf + " , (SELECT CGROUPID FROM " & cnAdminDb & "..CATEGORY C WHERE C.CATCODE=X.CATCODE )AS CAT_GROUP "
        strsql += vbCrLf + "  INTO TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE_BAL] "
        strsql += vbCrLf + " FROM ("
        '' OPEN WEIGHT
        strsql += vbCrLf + "SELECT CASE WHEN TRANTYPE ='R' THEN -1*ISNULL(SUM(ISNULL(CASE WHEN ISNULL(STONEUNIT,'')='C' THEN NETWT/5 ELSE NETWT END,0)),0) ELSE ISNULL(SUM(ISNULL(CASE WHEN ISNULL(STONEUNIT,'')='C' THEN NETWT/5 ELSE NETWT END,0)),0) END  GRSWT"
        strsql += vbCrLf + ",CASE WHEN TRANTYPE='R' THEN 'OR' ELSE 'OI' END  TRANTYPE"
        strsql += vbCrLf + ",ACCODE ,CATCODE"
        strsql += vbCrLf + ",0 TRANNO,STONEUNIT"
        strsql += vbCrLf + "FROM " & cnStockDb & "..OPENWEIGHT "
        strsql += vbCrLf + "WHERE  "
        strsql += vbCrLf + " ACCODE = '" & ACCODE & "'AND ISNULL(APPROVAL,'')<>'A' AND ISNULL(APPROVAL,'')<>'I'  AND ISNULL(ACCODE,'') <> ''  "
        strsql += vbCrLf + "AND COMPANYID= '" & strCompanyId & "' "
        strsql += vbCrLf + "AND COSTID= '" & _tempCostid & "' "
        strsql += vbCrLf + "GROUP BY ACCODE,CATCODE,TRANTYPE,STONEUNIT"
        strsql += vbCrLf + "UNION ALL "
        '' RECEIPT
        strsql += vbCrLf + "SELECT SUM(ISNULL(NETWT,0) +CASE WHEN TRANTYPE NOT IN ('MI','AR') THEN ISNULL(ALLOY,0)+ISNULL(WASTAGE,0) ELSE 0 END )*(-1) GRSWT "
        strsql += vbCrLf + ",'R' TRANTYPE "
        strsql += vbCrLf + ",ACCODE,CATCODE,TRANNO,STONEUNIT "
        strsql += vbCrLf + "FROM " & cnStockDb & "..RECEIPT WHERE "
        strsql += vbCrLf + "TRANDATE<='" & strBilldate & "' "
        strsql += vbCrLf + "AND ACCODE='" & ACCODE & "'"
        strsql += vbCrLf + "AND ISNULL(CANCEL,'') ='' "
        If Trantype = "IIN" Or Trantype = "RIN" Then
            strsql += vbCrLf + "AND TRANTYPE IN ('IIN','RIN') "
        Else
            strsql += vbCrLf + "AND TRANTYPE IN ('IIS','RRE') "
        End If
        strsql += vbCrLf + "AND COMPANYID='" & strCompanyId & "' "
        strsql += vbCrLf + "AND COSTID= '" & _tempCostid & "' "
        strsql += vbCrLf + "GROUP BY TRANTYPE,ACCODE,TRANNO,CATCODE,STONEUNIT"
        strsql += vbCrLf + "UNION ALL"
        '' ISSUE
        strsql += vbCrLf + "SELECT SUM(ISNULL(NETWT,0) +CASE WHEN TRANTYPE NOT IN ('MI','AI') THEN ISNULL(WASTAGE,0)+ISNULL(ALLOY,0) ELSE 0 END ) GRSWT"
        strsql += vbCrLf + ",'I' TRANTYPE,ACCODE,CATCODE "
        strsql += vbCrLf + ",TRANNO,STONEUNIT"
        strsql += vbCrLf + "FROM " & cnStockDb & "..ISSUE WHERE "
        strsql += vbCrLf + "TRANDATE<='" & strBilldate & "'"
        strsql += vbCrLf + "AND ACCODE='" & ACCODE & "'"
        strsql += vbCrLf + "AND ISNULL(CANCEL,'') =''  "
        If Trantype = "IIN" Or Trantype = "RIN" Then
            strsql += vbCrLf + "AND TRANTYPE IN ('IIN','RIN') "
        Else
            strsql += vbCrLf + "AND TRANTYPE IN ('IIS','RRE') "
        End If
        strsql += vbCrLf + "AND COMPANYID = '" & strCompanyId & "' "
        strsql += vbCrLf + "AND COSTID= '" & _tempCostid & "' "
        strsql += vbCrLf + "GROUP BY TRANTYPE,ACCODE,TRANNO,CATCODE,STONEUNIT "
        strsql += vbCrLf + "UNION ALL"
        '' ISSUE STONE
        strsql += vbCrLf + "SELECT SUM(CASE WHEN AC.ACTYPE<>'I' OR I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN STNWT ELSE 0 END) GRSWT"
        strsql += vbCrLf + ",'I' TRANTYPE,I.ACCODE,S.CATCODE,I.TRANNO,S.STONEUNIT"
        strsql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE S INNER JOIN " & cnStockDb & "..ISSUE I ON I.SNO=S.ISSSNO LEFT JOIN " & cnAdminDb & "..ACHEAD AC ON AC.ACCODE =I.ACCODE   "
        strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID=I.ITEMID LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID And SM.SUBITEMID = I.SUBITEMID "
        strsql += vbCrLf + "  WHERE  "
        strsql += vbCrLf + "  I.TRANDATE <='" & strBilldate & "' AND I.ACCODE='" & ACCODE & "'  AND ISNULL(I.ACCODE,'') <> '' AND ISNULL(CANCEL,'') =''   "
        If Trantype = "IIN" Or Trantype = "RIN" Then
            strsql += vbCrLf + "AND S.TRANTYPE IN ('IIN','RIN') "
        Else
            strsql += vbCrLf + "AND S.TRANTYPE IN ('IIS','RRE') "
        End If
        strsql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "' "
        strsql += vbCrLf + " AND I.COSTID= '" & _tempCostid & "' "
        strsql += vbCrLf + " GROUP BY I.ACCODE  ,S.CATCODE,I.TRANNO,S.STONEUNIT "
        strsql += vbCrLf + "UNION ALL"
        '' RECEIPT STONE
        strsql += vbCrLf + "SELECT -1 * SUM(CASE WHEN AC.ACTYPE<>'I' OR I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN STNWT ELSE 0 END) GRSWT"
        strsql += vbCrLf + ",'I' TRANTYPE,I.ACCODE,S.CATCODE,I.TRANNO,S.STONEUNIT"
        strsql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE S INNER JOIN " & cnStockDb & "..RECEIPT I ON I.SNO=S.ISSSNO LEFT JOIN " & cnAdminDb & ".. ACHEAD AC ON AC.ACCODE =I.ACCODE   "
        strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID=I.ITEMID LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID And SM.SUBITEMID = I.SUBITEMID  "
        strsql += vbCrLf + " WHERE  "
        strsql += vbCrLf + "  I.TRANDATE <='" & strBilldate & "' AND I.ACCODE='" & ACCODE & "'  AND ISNULL(I.ACCODE,'') <> '' AND ISNULL(CANCEL,'') =''   "
        If Trantype = "IIN" Or Trantype = "RIN" Then
            strsql += vbCrLf + "AND S.TRANTYPE IN ('IIN','RIN') "
        Else
            strsql += vbCrLf + "AND S.TRANTYPE IN ('IIS','RRE') "
        End If
        strsql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "' "
        strsql += vbCrLf + " AND I.COSTID= '" & _tempCostid & "' "
        strsql += vbCrLf + " GROUP BY I.ACCODE  ,S.CATCODE,I.TRANNO,S.STONEUNIT "
        strsql += vbCrLf + ")X, " & cnAdminDb & "..ACHEAD AS A WHERE X.ACCODE = A.ACCODE "
        strsql += vbCrLf + "GROUP BY A.ACNAME,X.ACCODE,X.CATCODE,X.STONEUNIT"
        strsql += vbCrLf + "ORDER BY A.ACNAME"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        '''''''''''''''''''''''''''''''''''''''''''''''
        '''''' /* END OF SMITHI BALANCE PART */ '''''''
        '''''''''''''''''''''''''''''''''''''''''''''''


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
        strsql += vbCrLf + " ,OTABLENAME ,NULL FLAG"
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
        strsql += vbCrLf + " ,OTABLENAME,NULL FLAG"
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
            strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY='R' THEN GRSWT ELSE 1*GRSWT  END)GRSWT"
            strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY='R' THEN NETWT ELSE 1*NETWT  END)NETWT"
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
            strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY='R' THEN GRSWT ELSE -1*GRSWT  END)GRSWT"
            strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY='R' THEN NETWT ELSE -1*NETWT  END)NETWT"
            strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY='R' THEN MC ELSE -1*MC END)MC"
            strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY='R' THEN AMOUNT ELSE -1*AMOUNT END)AMOUNT"
            strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY='R' THEN LESSWT ELSE -1*LESSWT END)LESSWT"
            strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY='R' THEN PUREWT ELSE -1*PUREWT END)PUREWT"
            strsql += vbCrLf + " ,SUM(CASE WHEN RECPAY='R' THEN DIAWT ELSE -1*DIAWT END)DIAWT"
            strsql += vbCrLf + " FROM TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECETOTAL]"
        End If
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dsSmithIssueReceipt, "DTISSRECTOTAL")


        strsql = "DELETE TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE] WHERE ISNULL(ISSUENO,'') = '' AND ISNULL(DATE,'') = '' AND  ISNULL(CATNAME,'') = ''"
        strsql += vbCrLf + "  AND  ISNULL(HSN,'') = '' AND  ISNULL(GRSWT,0) = 0 AND  ISNULL(NETWT,0) = 0 AND  ISNULL(MC,0) = 0 AND  ISNULL(TAX,0) = 0 AND  ISNULL(AMOUNT,0) = 0 "
        strsql += vbCrLf + "  AND  ISNULL(RATE,0) = 0 AND  ISNULL(PCS,0) = 0 "
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = "SELECT TOP 1 TTYPE FROM TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE]  WHERE ISNULL(FLAG,'') <> '' "
        Dim ____TYPE As String = GetSqlValue(cn, strsql) 'SIREA


        strsql = " UPDATE TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE] SET  TABLENAME = ''"
        strsql += vbCrLf + " WHERE TTYPE = '" & ____TYPE & "' AND ISNULL(GRSWT,0) <> 0 AND ISNULL(FLAG,'') ='' AND  RESULT IN (5.0,6.0)"
        strsql += vbCrLf + " UPDATE TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE] SET  TABLENAME = CASE "
        strsql += vbCrLf + " WHEN TTYPE ='RRE' THEN '[RECEIPT]' WHEN TTYPE ='IIS' THEN '[ISSUE]' "
        strsql += vbCrLf + "WHEN TTYPE ='RPU'  THEN '[PURCHASE]'  WHEN TTYPE ='IPU'  THEN '[PURCHASE RETURN]'"
        strsql += vbCrLf + "WHEN TTYPE ='RAP'  THEN '[APPROVAL RECEIPT]'WHEN TTYPE ='IAP'  THEN '[APPROVAL ISSUE]' ELSE '' END"
        strsql += vbCrLf + " WHERE TTYPE <> '" & ____TYPE & "' AND ISNULL(GRSWT,0) <> 0 AND ISNULL(FLAG,'') ='' AND  RESULT IN (5.0,6.0)"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        If chkmeltsummary.Checked = False Then
            strsql = "SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE] ORDER BY OTABLENAME,TYPE,SERNO,RESULT"
        Else
            strsql = "SELECT ROW_NUMBER() OVER (ORDER BY OTABLENAME,TYPE,RESULT) SNO,ISSUENO,DATE,CATNAME,HSN,USERNAME,ALLOYNAME,ALLOY,WASTAGENAME"
            strsql += vbCrLf + ",WASTAGE,PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,NULL RATE,"
            strsql += vbCrLf + "DIAWT,MC,TAX,SUM(AMOUNT)AMOUNT,ACCODE" ',REMARK1,REMARK2,
            strsql += vbCrLf + ",(SELECT TOP 1 REMARK1 FROM TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE]) REMARK1"
            strsql += vbCrLf + ",(SELECT TOP 1 REMARK2 FROM TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE]) REMARK2"
            strsql += vbCrLf + ",TRANTYPE,RESULT,TTYPE,TOUCH,PURITY,SUM(PUREWT)PUREWT,SUM(LESSWT)LESSWT,"
            strsql += vbCrLf + "DIAAMT,ITEMNAME,TYPE,TABLENAME,TAXID,TAXPER,CATCODE,OTABLENAME,FLAG"
            strsql += vbCrLf + "FROM "
            strsql += vbCrLf + "TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE]"
            strsql += vbCrLf + "GROUP BY ISSUENO,DATE,CATNAME,HSN,USERNAME,ALLOYNAME,ALLOY,WASTAGENAME,"
            strsql += vbCrLf + "WASTAGE,PCS,DIAWT,MC,TAX,ACCODE" ',REMARK1,REMARK2
            strsql += vbCrLf + ",TRANTYPE,RESULT,TTYPE,TOUCH,PURITY,DIAAMT,ITEMNAME,TYPE,TABLENAME,"
            strsql += vbCrLf + "TAXID,TAXPER,CATCODE,OTABLENAME,FLAG"
            strsql += vbCrLf + "ORDER BY OTABLENAME,TYPE,RESULT"
        End If
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dsSmithIssueReceipt, "DTISSREC")




        strsql = "SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "SMITHISSRECE_BAL]"
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dsSmithIssueReceipt, "BALANCE")

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
    Public Sub stocktransferIssueReceiptold(ByVal BatchNo As String, ByVal Trantype As String, ByVal mimrformat As String)
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
        strsql += vbCrLf + " ,TRANNO AS ISSUENO "
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
        strsql += vbCrLf + " AND TRANTYPE IN ('RIN') " ' RECEIPT
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        smithIssueReceiptStone(BatchNo, "'RRE','RMP','RAP'", mimrformat, 2, "RECEIPT", "A", "1")

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
            strsql += vbCrLf + " AND TRANTYPE IN ('RIN') " ' RECEIPT
            strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TAXTYPE"
            strsql += vbCrLf + " UNION ALL"
            strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO "
            strsql += vbCrLf + " ,CASE WHEN TAXTYPE = 'RG' THEN 'GST Reversal on wastage' ELSE '' END TAXTYPENAME"
            strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
            strsql += vbCrLf + " AND TAXID = 'SG' "
            strsql += vbCrLf + " AND ISNULL(TAXTYPE,'') <> 'RG'"
            strsql += vbCrLf + " AND TRANTYPE IN ('RIN') " ' RECEIPT
            strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TAXTYPE"
            strsql += vbCrLf + " UNION ALL"
            strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO "
            strsql += vbCrLf + " ,CASE WHEN TAXTYPE = 'RG' THEN 'GST Reversal on wastage' ELSE '' END TAXTYPENAME"
            strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
            strsql += vbCrLf + " AND TAXID = 'IG' "
            strsql += vbCrLf + " AND ISNULL(TAXTYPE,'') <> 'RG'"
            strsql += vbCrLf + " AND TRANTYPE IN ('RIN') " ' RECEIPT
            strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TAXTYPE"
            strsql += vbCrLf + " UNION ALL"
            strsql += vbCrLf + " SELECT 'TC' TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO, '' TAXTYPENAME "
            strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
            strsql += vbCrLf + " AND TAXID = 'TC' "
            strsql += vbCrLf + " AND TRANTYPE IN ('RIN') " ' RECEIPT
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
        strsql += vbCrLf + " TRANNO AS ISSUENO , "
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
        strsql += vbCrLf + " ACCODE,REMARK1,REMARK2"
        If mimrformat = "NEW" Then
            strsql += vbCrLf + " ,(CASE WHEN TRANTYPE = 'IIN' THEN 'TRANSFER TO BRANCH' ELSE CASE WHEN TRANTYPE = 'IMP' THEN 'PURIFICATION ISSUE VOUCHER' ELSE 'PURCHASE RETURN' END END) TRANTYPE"
        Else
            strsql += vbCrLf + " ,(CASE WHEN TRANTYPE = 'IIN' THEN 'TRANSFER TO BRANCH' ELSE CASE WHEN TRANTYPE = 'IMP' THEN 'PURIFICATION ISSUE VOUCHER' ELSE 'PURCHASE RETURN' END END) TRANTYPE"
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
        strsql += vbCrLf + " AND TRANTYPE IN('IIN') " ' ISSUE 
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT ROW_NUMBER() OVER(ORDER BY TRANNO DESC) AS SNO,"
        strsql += vbCrLf + " TRANNO AS ISSUENO , "
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
        strsql += vbCrLf + " AND TRANTYPE IN ('RIN') " ' 
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()
        smithIssueReceiptStone(BatchNo, "'RIN'", mimrformat, 7, "RECEIPT", "A", "2")
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
            strsql += vbCrLf + " ,CASE WHEN TRANTYPE='RIN' THEN 'RECEIPT' ELSE 'ISSUE' END  TABLENAME"
            strsql += vbCrLf + " ,TAXID,TAXPER "
            strsql += vbCrLf + " ,NULL CATCODE"
            strsql += vbCrLf + " ,CASE WHEN TRANTYPE='RIN' THEN 'A' ELSE 'B' END  OTABLENAME"
            strsql += vbCrLf + "  FROM ( "
            strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT)TAXAMOUNT,BATCHNO,TRANTYPE  "
            strsql += vbCrLf + " ,CASE WHEN TAXTYPE = 'RG' THEN 'GST Reversal on wastage' ELSE '' END TAXTYPENAME"
            strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
            strsql += vbCrLf + " AND TAXID = 'CG' "
            strsql += vbCrLf + " AND ISNULL(TAXTYPE,'') <> 'RG'"
            strsql += vbCrLf + " AND TRANTYPE IN ('IIN','RIN') " ' RECEIPT
            strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TAXTYPE,TRANTYPE"
            strsql += vbCrLf + " UNION ALL"
            strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO,TRANTYPE "
            strsql += vbCrLf + " ,CASE WHEN TAXTYPE = 'RG' THEN 'GST Reversal on wastage' ELSE '' END TAXTYPENAME"
            strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
            strsql += vbCrLf + " AND TAXID = 'SG' "
            strsql += vbCrLf + " AND ISNULL(TAXTYPE,'') <> 'RG'"
            strsql += vbCrLf + " AND TRANTYPE IN ('IIN','RIN') " ' RECEIPT
            strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TAXTYPE,TRANTYPE"
            strsql += vbCrLf + " UNION ALL"
            strsql += vbCrLf + " SELECT TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO,TRANTYPE "
            strsql += vbCrLf + " ,CASE WHEN TAXTYPE = 'RG' THEN 'GST Reversal on wastage' ELSE '' END TAXTYPENAME"
            strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
            strsql += vbCrLf + " AND TAXID = 'IG' "
            strsql += vbCrLf + " AND ISNULL(TAXTYPE,'') <> 'RG'"
            strsql += vbCrLf + " AND TRANTYPE IN ('IIN','RIN') " ' RECEIPT
            strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TAXTYPE,TRANTYPE"
            strsql += vbCrLf + " UNION ALL"
            strsql += vbCrLf + " SELECT 'TC' TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO,TRANTYPE "
            strsql += vbCrLf + " ,'' TAXTYPENAME "
            strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
            strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
            strsql += vbCrLf + " AND TAXID = 'TC' "
            strsql += vbCrLf + " AND TRANTYPE IN ('IIN','RIN') " ' ISSUE
            strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TRANTYPE"
            If cntrecet = 0 Then
                strsql += vbCrLf + " UNION ALL"
                strsql += vbCrLf + " SELECT 'TC' TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO,'RIN' TRANTYPE,'' TAXTYPENAME "
                strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
                strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
                strsql += vbCrLf + " AND TRANTYPE IN ('TR') " ' ISSUE
                strsql += vbCrLf + " AND "
                strsql += vbCrLf + " (TAXID IN (SELECT DISTINCT CAST(TDSCATID AS varchar) FROM " & cnAdminDb & "..TDSCATEGORY) OR TAXID IN ('TC'))"
                strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TRANTYPE"
                strsql += vbCrLf + " UNION ALL"
                strsql += vbCrLf + " SELECT 'TD' TAXID,TAXPER,SUM(TAXAMOUNT) TAXAMOUNT,BATCHNO,'RIN' TRANTYPE,'' TAXTYPENAME "
                strsql += vbCrLf + " FROM " & cnStockDb & "..TAXTRAN WHERE TRANDATE = '" & strBilldate & "' "
                strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "' "
                strsql += vbCrLf + " AND TRANTYPE IN ('TR') " ' ISSUE
                strsql += vbCrLf + " AND "
                strsql += vbCrLf + " (TAXID IN (SELECT DISTINCT CAST(TDSCATID AS varchar) FROM " & cnAdminDb & "..TDSCATEGORY) OR TAXID IN ('TD'))"
                strsql += vbCrLf + " GROUP BY TAXID,TAXPER,BATCHNO,TRANTYPE"
                strsql += vbCrLf + " UNION ALL"
                strsql += vbCrLf + " SELECT 'ZRO' TAXID,0 TAXPER "
                strsql += vbCrLf + " ,SUM(CASE WHEN TRANMODE='D' THEN AMOUNT ELSE -1* AMOUNT END) TAXAMOUNT "
                strsql += vbCrLf + " ,BATCHNO,'RIN' TRANTYPE,'' TAXTYPENAME "
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
        strsql += vbCrLf + " AND TRANTYPE = 'RIN' " ' PURCHASE TOTAL
        strsql += vbCrLf + " AND BATCHNO = '" & BatchNo & "'"
        strsql += vbCrLf + " GROUP BY BATCHNO"
        strsql += vbCrLf + " UNION ALL"
        strsql += vbCrLf + " SELECT 'TOTAL' AS TOTAL,SUM(PCS)PCS "
        If mimrformat = "NEW" Then
            strsql += vbCrLf + " , SUM(GRSWT) + SUM(ALLOY) AS GRSWT "
            strsql += vbCrLf + " , SUM(NETWT) + SUM(ALLOY) AS NETWT "
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
        strsql += vbCrLf + " AND TRANTYPE IN('IIN') " ' ISSUE TOTAL
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
        strsql += vbCrLf + "EXEC " & cnAdminDb & "..PROC_ORDERBILLPRINTVIEW_A4N"
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

    Public Sub OrderReceipt_12_jan_2022(ByVal PbatchNo As String, ByVal Type As String, ByVal _ordate As String)
        strsql = " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "ORMASTACCTRAN]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "ORMASTACCTRAN]"
        strsql += " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "ORMAST]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "ORMAST]"
        strsql += " IF OBJECT_ID('TEMPTABLEDB..[TEMP" & SystemName & "OCUSTORDER]','U')IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMP" & SystemName & "OCUSTORDER]"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = " CREATE TABLE TEMPTABLEDB..[TEMP" & SystemName & "ORMASTACCTRAN] (ACCODE VARCHAR(250), BATCHNO_N VARCHAR(20))"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

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
        strsql += vbCrLf + " WHERE ORDATE = '" & _ordate & "' "
        strsql += vbCrLf + " AND BATCHNO =  '" & PbatchNo & "'"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] "
        strsql += vbCrLf + " SELECT NULL,NULL,NULL,DESCRIPT,NULL,NULL,NULL,BATCHNO,NULL,NULL,SYSTEMID"
        strsql += vbCrLf + " ,2 AS RESULT, 1 AS TYPE, '' AS COLHEAD "
        strsql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST as O"
        strsql += vbCrLf + " WHERE ORDATE = '" & _ordate & "' "
        strsql += vbCrLf + " AND BATCHNO =  '" & PbatchNo & "' "
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] "
        strsql += vbCrLf + " SELECT NULL,NULL,NULL,REASON,NULL,NULL,NULL,BATCHNO,NULL,NULL,SYSTEMID"
        strsql += vbCrLf + " ,3 AS RESULT, 1 AS TYPE, '' AS COLHEAD "
        strsql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST as O"
        strsql += vbCrLf + " WHERE ORDATE = '" & _ordate & "' "
        strsql += vbCrLf + " AND BATCHNO =  '" & PbatchNo & "' "
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] "
        strsql += vbCrLf + " SELECT NULL,NULL,NULL,'TOTAL',SUM(PCS),SUM(GRSWT),NULL,BATCHNO,NULL,NULL,SYSTEMID"
        strsql += vbCrLf + " ,9 AS RESULT, 1 AS TYPE, 'U' AS COLHEAD "
        strsql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST as O"
        strsql += vbCrLf + " WHERE ORDATE = '" & _ordate & "' "
        strsql += vbCrLf + " AND BATCHNO =  '" & PbatchNo & "' "
        strsql += vbCrLf + " GROUP BY SYSTEMID,BATCHNO"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        'Space Line
        strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] "
        strsql += vbCrLf + " SELECT NULL,NULL,NULL,NULL,NULL,NULL,NULL,BATCHNO,NULL,NULL,SYSTEMID"
        strsql += vbCrLf + " ,10 AS RESULT, 1 AS TYPE, '' AS COLHEAD "
        strsql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST AS O"
        strsql += vbCrLf + " WHERE ORDATE = '" & _ordate & "' "
        strsql += vbCrLf + " AND BATCHNO =  '" & PbatchNo & "' "
        strsql += vbCrLf + " GROUP BY SYSTEMID,BATCHNO"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] "
        strsql += vbCrLf + " SELECT NULL,NULL,NULL,'RATE FIXED   :'+CONVERT(VARCHAR(20),RATE)"
        strsql += vbCrLf + " ,NULL,NULL,NULL,BATCHNO,NULL,NULL,SYSTEMID"
        strsql += vbCrLf + " ,12 AS RESULT, 1 AS TYPE, 'G' AS COLHEAD "
        strsql += vbCrLf + " FROM " & cnAdminDb & "..ORMAST AS O "
        strsql += vbCrLf + " WHERE ORDATE = '" & _ordate & "' "
        strsql += vbCrLf + " AND BATCHNO =  '" & PbatchNo & "' "
        strsql += vbCrLf + " AND ORRATE='C' "
        strsql += vbCrLf + " GROUP BY SYSTEMID,BATCHNO,RATE"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] "
        strsql += vbCrLf + " SELECT NULL,NULL,NULL, "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'CC' THEN 'CARD ' + AC.SHORTNAME + ' ' + CHQCARDNO + '     :' + LTRIM(SUM(AMOUNT)) ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'AA' THEN 'ADVANCE ' + 'REFER : ' + SUBSTRING(REFNO,1,3) + '-' + SUBSTRING(REFNO,4,10) + '     :' + LTRIM(SUM(AMOUNT)) ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'CA' THEN 'CASH ' + '     :' + LTRIM(SUM(AMOUNT)) ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'PU' THEN 'PURCHASE ' + ' :' + LTRIM(SUM(AMOUNT)) ELSE"
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'SR' THEN 'SALES RETURN ' + ' :' + LTRIM(SUM(AMOUNT)) ELSE "
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'CH' AND ISNULL(FLAG,'') <> '' THEN (SELECT MODENAME FROM " & cnAdminDb & "..PAYMENTMODE WHERE MODEID =ISNULL(T.FLAG,'')) + ' ' + CHQCARDREF + ' ' + CHQCARDNO + '     :' + LTRIM(SUM(AMOUNT)) ELSE "
        strsql += vbCrLf + " 'CHEQUE ' + CHQCARDREF + ' ' + CHQCARDNO + '     :' + LTRIM(SUM(AMOUNT))" 'CH
        strsql += vbCrLf + " END END END END END END"
        strsql += vbCrLf + " ,NULL,NULL,NULL,BATCHNO,NULL,NULL,SYSTEMID, "
        strsql += vbCrLf + " 13 RESULT, 1 TYPE, 'G' AS COLHEAD FROM " & cnStockDb & "..ACCTRAN T"
        strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CREDITCARD AC ON AC.CARDCODE=T.CARDID "
        strsql += vbCrLf + " WHERE TRANDATE = '" & _ordate & "' "
        strsql += vbCrLf + " AND BATCHNO = '" & PbatchNo & "'"
        strsql += vbCrLf + " AND PAYMODE IN ('AA','CC','CH','CA','PU','SR')"
        strsql += vbCrLf + " GROUP BY PAYMODE,CHQCARDNO,BATCHNO"
        strsql += vbCrLf + " ,AC.SHORTNAME,CHQCARDREF,REFNO,SYSTEMID,ISNULL(T.FLAG,'')"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] "
        strsql += vbCrLf + " SELECT NULL,NULL,NULL"
        strsql += vbCrLf + " ,'TOTAL ADVANCE  : '+CONVERT(VARCHAR(20),SUM(AMOUNT)) + 'GST : ' + CONVERT(VARCHAR(20),SUM(GSTVAL)) "
        strsql += vbCrLf + " ,NULL,NULL,NULL,BATCHNO,NULL,NULL,SYSTEMID"
        strsql += vbCrLf + " ,14 AS RESULT, 1 AS TYPE, 'G' AS COLHEAD "
        strsql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS O"
        strsql += vbCrLf + " WHERE TRANDATE = '" & _ordate & "' "
        strsql += vbCrLf + " AND BATCHNO =  '" & PbatchNo & "' "
        strsql += vbCrLf + " AND PAYMODE='OR' GROUP BY SYSTEMID,BATCHNO"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = "SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "ORMAST] ORDER BY RESULT"
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtOrderRepair)

        Tranno = dtOrderRepair.Rows(0).Item("ORNO").ToString
        strBilldate = dtOrderRepair.Rows(0).Item("BILLDATE").ToString
        NodeId = dtOrderRepair.Rows(0).Item("NODE").ToString
        DueDate = dtOrderRepair.Rows(0).Item("DUEDATE").ToString
    End Sub
    Public Sub CustomerInfoAdvanceReceiptold(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        Dim rAlign As New StringFormat
        rAlign.Alignment = StringAlignment.Far
        g.DrawRectangle(New Pen(Color.Silver, 1), c8 - 20, 28, 10, 10)
        g.DrawString("Original for Recipient", fontBold, BlackBrush, c8, 25, LeftFormat)
        g.DrawRectangle(New Pen(Color.Silver, 1), c8 - 20, 48, 10, 10)
        g.DrawString("Duplicate for Supplier", fontBold, BlackBrush, c8, 45, LeftFormat)

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
            Dim AD_Tranno As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("AD_tranno").ToString
            Dim Billdate As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("billdate").ToString
            Dim Field1 As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field1").ToString
            Dim Field2 As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field2").ToString
            Dim Accode As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("Accode").ToString
            Dim Amt As Double = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("amount").ToString

            Dim PurityType As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("CATCODETYPE").ToString

            Dim AmtWords As String = ConvertRupees.RupeesToWord(Amt)
            Dim Field5 As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field5").ToString
            Dim Remarks As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("REMARKS").ToString
            Dim Remark1 As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("REMARK1").ToString
            NodeId = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("node").ToString

            Dim ItemDetail As String = "ITEM DETAIL"
            Dim RateFixed As String = dsAdvanceReceipt.Tables("ITEMTAG").Rows(0).Item("RATENAME").ToString
            Dim RateValue As String = dsAdvanceReceipt.Tables("ITEMTAG").Rows(0).Item("RATE").ToString
            Dim address_head As String = "Name And Address :"
            g.DrawString(address_head, fontBold, BlackBrush, c1, START_POS, LeftFormat)
            START_POS = START_POS + LINE_SPACE
            g.DrawString(pname, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
            g.DrawString("RECEIPT", fontBoldTitle, Brushes.Black, c6, START_POS)


            START_POS = START_POS + LINE_SPACE


            g.DrawString(Address1, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
            g.DrawString(TranName, fontRegular, BlackBrush, c6, START_POS, LeftFormat)
            If dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field1").ToString.Contains("RECEIPT  VOUCHER") Then
                g.DrawString("   : " & Tranno, fontRegular, BlackBrush, c8, START_POS, LeftFormat)
            Else
                g.DrawString("   : " & AD_Tranno, fontRegular, BlackBrush, c8, START_POS, LeftFormat)
            End If

            g.DrawString(printcpy, fontBoldTitle, Brushes.Black, c4, START_POS)
            START_POS = START_POS + LINE_SPACE
            g.DrawString("Date", fontRegular, BlackBrush, c6, START_POS, LeftFormat)
            g.DrawString("   : " & Billdate, fontRegular, BlackBrush, c8, START_POS, LeftFormat)
            g.DrawString(Address2, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
            g.DrawString(Field1, fontBoldTitle, Brushes.Black, c4, START_POS)

            START_POS = START_POS + LINE_SPACE

            g.DrawString(Mobile & IIf(Tin <> "", " PAN : " & Tin, "") & IIf(gstno <> "", " GSTNO : " & gstno, ""), fontRegular, BlackBrush, c1, START_POS, LeftFormat)
            START_POS = START_POS + LINE_SPACE
            g.DrawLine(Pens.Silver, c1, START_POS, c10, START_POS)
            START_POS = START_POS + LINE_SPACE
            If printCustoInfo = True Then
                g.DrawString(Field2, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
                If Field5 = "" Then
                    g.DrawString(dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("amount").ToString, fontBoldTitle, BlackBrush, c9, START_POS, LeftFormat)
                End If
                START_POS = START_POS + LINE_SPACE
                START_POS = START_POS + LINE_SPACE
                If Field5 <> "" Then
                    g.DrawString(Field5, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
                Else
                    START_POS = START_POS + LINE_SPACE
                End If
                START_POS = START_POS + LINE_SPACE
                START_POS = START_POS + LINE_SPACE
                If Field5 <> "" Then
                    g.DrawString(dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("amount").ToString, fontBoldTitle, BlackBrush, c9, START_POS, LeftFormat)
                Else
                    START_POS = START_POS + LINE_SPACE
                End If
                START_POS = START_POS + LINE_SPACE
                START_POS = START_POS + LINE_SPACE
                START_POS = START_POS + LINE_SPACE


                If Remarks <> "" Then
                    g.DrawString(Remarks, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
                    START_POS = START_POS + LINE_SPACE
                    START_POS = START_POS + LINE_SPACE
                End If

                If dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field1").ToString.Contains("RECEIPT  VOUCHER") Then
                    START_POS = START_POS + LINE_SPACE
                Else
                    If Remark1 <> "" Then
                        g.DrawString(Remark1, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
                        START_POS = START_POS + LINE_SPACE
                    End If
                End If

                If dsAdvanceReceipt.Tables("ISSUE").Rows.Count > 0 Then
                    Dim GrswA As String = dsAdvanceReceipt.Tables("ISSUE").Rows(0).Item("GRSWT")
                    Dim AmtA As String = dsAdvanceReceipt.Tables("ISSUE").Rows(0).Item("AMOUNT")
                End If
                START_POS = START_POS + LINE_SPACE
            End If
            'ADVANCE ONLY DISPLAY
            If dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("Field1").ToString.Contains("ADVANCE ") Or dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field1").ToString.Contains("RECEIPT  VOUCHER") Then
                START_POS = START_POS + LINE_SPACE + 120
            End If
        End If
    End Sub
    Public Sub CustomerInfoAdvanceReceipt(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        Dim Cusaccode As String = ""
        Dim grate As String = ""
        Dim srate As String = ""
        Dim BBill As String = ""
        Dim CusGSTNo As String = ""
        Dim rAlign As New StringFormat
        rAlign.Alignment = StringAlignment.Far
        g.DrawString(Print_Ver_No.ToString, fontRegular6, SilverBrush, c9 + 10, 5, LeftFormat)
        g.DrawRectangle(New Pen(Color.Silver, 1), c8 - 20, 28, 10, 10)
        g.DrawString("Original for Recipient", fontBold, BlackBrush, c8, 25, LeftFormat)
        g.DrawRectangle(New Pen(Color.Silver, 1), c8 - 20, 48, 10, 10)
        g.DrawString("Duplicate for Supplier", fontBold, BlackBrush, c8, 45, LeftFormat)

        If _SILVERBILLPRINT = "Y" Then
            g.DrawRectangle(New Pen(Color.Silver, 1), c8 - 20, 68, 10, 10)
            g.DrawString("Triplicate for Supplier", fontBold, BlackBrush, c8, 65, LeftFormat)
        End If
        If _SILVERBILLPRINT = "Y" And _SILVERBILLPRINT_HEADER = "Y" Then
            Dim Strimgpath As String = ""
            Dim logo As Image
            If IO.File.Exists(Application.StartupPath & "\VBJ_LOGO.jpg") Then
                Strimgpath = Application.StartupPath & "\VBJ_LOGO.jpg"
                logo = Image.FromFile(Strimgpath)
                '' e.Graphics.DrawImage(logo, c3, 80, 120, 70)
            End If
            Dim dtcomp As New DataTable
            'strsql = " SELECT * FROM " & cnAdminDb & "..COMPANY "
            'strsql += vbCrLf + "WHERE COMPANYID='" & strCompanyId & "'"
            'da = New OleDbDataAdapter(strsql, cn)
            'dtcomp = New DataTable
            'da.Fill(dtcomp)
            strsql = " SELECT TOP 1  * FROM " & cnAdminDb & "..COSTCENTRE "
            strsql += vbCrLf + "WHERE COSTID='" & cnCostId & "'"
            da = New OleDbDataAdapter(strsql, cn)
            dtcomp = New DataTable
            da.Fill(dtcomp)

            If dtcomp.Rows.Count > 0 Then
                Dim Statename As String = ""
                strsql = " SELECT ISNULL(STATENAME,'') STATENAME FROM " & cnAdminDb & "..STATEMAST "
                strsql += vbCrLf + " WHERE STATEID = " & dtcomp.Rows(0).Item("STATEID").ToString & " "
                Statename = GetSqlValue(cn, strsql)
                g.DrawString(printcpy, fontBoldTitle, Brushes.Black, c4, 20, LeftFormat)
                g.DrawString("TAX INVOICE", fontBoldTitle, Brushes.Black, c4, 40, LeftFormat)
                g.DrawString(StrConv(strCompanyName.ToString, VbStrConv.ProperCase), fontBoldHead, BlackBrush, c4 - 60, 80, LeftFormat)
                g.DrawString(dtcomp.Rows(0).Item("ADDRESS1").ToString, fontBold, BlackBrush, c4 - 50, 115, LeftFormat)
                g.DrawString("GSTIN :" & dtcomp.Rows(0).Item("GSTNO").ToString, fontBold, BlackBrush, c7, 115, LeftFormat)
                g.DrawString(dtcomp.Rows(0).Item("ADDRESS2").ToString, fontBold, BlackBrush, c4 - 50, 130, LeftFormat)
                g.DrawString("STATE :" & Statename.ToString, fontBold, BlackBrush, c7, 130, LeftFormat)
                g.DrawString("Phone :" & dtcomp.Rows(0).Item("PHONE").ToString, fontBold, BlackBrush, c4 - 50, 145, LeftFormat)
                ''g.DrawString("PANNO :" & dtcomp.Rows(0).Item("PANNO").ToString, fontBold, BlackBrush, c7, 145, LeftFormat)
            End If
        End If

        Dim dtcustleft As DataTable
        'If dsAdvanceReceipt.Tables("CUSTOMER").Rows.Count > 0 And dsAdvanceReceipt.Tables("OUTSTANDING").Rows.Count > 0 And dsAdvanceReceipt.Tables("ACCTRAN").Rows.Count > 0 Then
        If dsAdvanceReceipt.Tables("CUSTOMER").Rows.Count > 0 And dsAdvanceReceipt.Tables("OUTSTANDING").Rows.Count > 0 Then
            strsql = " SELECT * FROM " & cnAdminDb & "..PERSONALINFO "
            strsql += vbCrLf + "WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = '" & pBatchno & "')"
            da = New OleDbDataAdapter(strsql, cn)
            dtCustInfo = New DataTable
            da.Fill(dtCustInfo)
            If dtCustInfo.Rows.Count > 0 Then
                Cusaccode = dtCustInfo.Rows(0).Item("accode").ToString
            End If
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
            Dim AD_Tranno As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("AD_tranno").ToString
            Dim Billdate As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("billdate").ToString
            Dim Field1 As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field1").ToString
            Dim Field2 As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field2").ToString
            Dim Field3 As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field3").ToString
            Dim Accode As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("Accode").ToString
            Dim Amt As Double = dsAdvanceReceipt.Tables("OUTSTANDING").Compute("SUM(amount)", "").ToString ''dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("amount").ToString
            Dim PurityType As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("CATCODETYPE").ToString
            Dim AmtWords As String = ConvertRupees.RupeesToWord(Amt)
            Dim Field5 As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field5").ToString
            Dim Remarks As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("REMARKS").ToString
            Dim Remark1 As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("REMARK1").ToString
            NodeId = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("node").ToString
            Dim ItemDetail As String = "ITEM DETAIL"
            Dim RateFixed As String = dsAdvanceReceipt.Tables("ITEMTAG").Rows(0).Item("RATENAME").ToString
            Dim RateValue As String = dsAdvanceReceipt.Tables("ITEMTAG").Rows(0).Item("RATE").ToString
            Dim address_head As String = "Name And Address :"

            dtcustleft = New DataTable
            strsql = vbCrLf + "SELECT '' LEFT1,'' RIGHT1 "
            cmd = New OleDbCommand(strsql, cn)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtcustleft)
            'left
            dtcustleft.Rows.Add(address_head, "")
            dtcustleft.Rows.Add(pname, "")
            dtcustleft.Rows.Add(Address1, "")
            dtcustleft.Rows.Add(Address2, "")
            dtcustleft.Rows.Add(IIf(Mobile <> "", " CELL : " & Mobile, "") & IIf(Tin <> "", " PAN : " & Tin, "") & IIf(gstno <> "", " GSTNO : " & gstno, ""), "")
            'right
            dtcustleft.Rows.Add("", "RECEIPT")
            If dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field1").ToString.Contains("RECEIPT  VOUCHER") Then
                dtcustleft.Rows.Add("", TranName & "   : " & Tranno)
            Else
                dtcustleft.Rows.Add("", TranName & "   : " & AD_Tranno)
            End If
            dtcustleft.Rows.Add("", "Date               : " & Billdate)
            If Cusaccode <> "" Then
                dtcustleft.Rows.Add("", "Accode          : " & Cusaccode)
            End If

            funcCustomerQrcode(g, e, 510, START_POS)
            Dim leftcnt As Integer = 0
            For I As Integer = 0 To dtcustleft.Rows.Count - 1
                If dtcustleft.Rows(I).Item("LEFT1").ToString.Trim <> "" Then
                    If dtcustleft.Rows(I).Item("LEFT1").ToString.Contains("Name") Then
                        g.DrawString(dtcustleft.Rows(I).Item("LEFT1").ToString, fontBold, Brushes.Black, c1, START_POS)
                        ''If _SILVERBILLPRINT <> "Y" Then
                        ''    If printcpy <> "" Then
                        ''        g.DrawString(printcpy, fontBoldTitle, Brushes.Black, c4, START_POS)
                        ''    End If
                        ''    g.DrawString(Field3, fontBoldTitle, Brushes.Black, c4, START_POS + LINE_SPACE + 10)
                        ''End If
                        ''If _SILVERBILLPRINT = "Y" Then
                        ''    g.DrawString(Field3, fontBoldTitle, Brushes.Black, c4, START_POS + LINE_SPACE + 10)
                        ''End If
                        If printcpy <> "" Then
                            g.DrawString(printcpy, fontBoldTitle, Brushes.Black, c4, START_POS)
                        End If
                        g.DrawString(Field3, fontBoldTitle, Brushes.Black, c4, START_POS + LINE_SPACE + 10)

                        leftcnt += 1
                    Else
                        g.DrawString(dtcustleft.Rows(I).Item("LEFT1").ToString, fontRegular, Brushes.Black, c1, START_POS)
                        leftcnt += 1
                    End If
                    START_POS = START_POS + LINE_SPACE
                End If
            Next
            If leftcnt < 3 Then
                START_POS = START_POS + LINE_SPACE
                START_POS = START_POS + LINE_SPACE
            End If
            Dim tempstartpos As Integer = 0
            tempstartpos = START_POS
            START_POS = TSTART_POS
            For I As Integer = 0 To dtcustleft.Rows.Count - 1
                If dtcustleft.Rows(I).Item("RIGHT1").ToString.Trim <> "" Then
                    If dtcustleft.Rows(I).Item("RIGHT1").ToString.Contains("JEWELS") Then
                        g.DrawString(dtcustleft.Rows(I).Item("RIGHT1").ToString, fontBold, Brushes.Black, c8 - 20, START_POS)
                    Else
                        g.DrawString(dtcustleft.Rows(I).Item("RIGHT1").ToString, fontRegular, Brushes.Black, c8 - 20, START_POS)
                    End If
                    START_POS = START_POS + LINE_SPACE
                End If
            Next
            START_POS = IIf(START_POS > tempstartpos, START_POS, tempstartpos)
            g.DrawLine(Pens.Silver, c1, START_POS, c10, START_POS)
            ''START_POS = START_POS + LINE_SPACE
            If dsAdvanceReceipt.Tables("OUTSTANDING").Rows.Count > 0 Then
                If dsAdvanceReceipt.Tables("OUTSTANDING").Rows.Count = 1 Then
                    START_POS = START_POS + LINE_SPACE
                    g.DrawString(Field2, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
                Else
                    Dim _repstr As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("AMOUNT").ToString
                    Field2 = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field2").ToString.Replace(_repstr.ToString, Format(Val(Amt.ToString), "0.00"))
                    START_POS = START_POS + LINE_SPACE
                    g.DrawString(Field2, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
                End If
            End If

            If Field5 = "" Then
                g.DrawString(Format(Val(Amt.ToString), "0.00"), fontBoldTitle, BlackBrush, c10, START_POS, rAlign)
            End If
            START_POS = START_POS + LINE_SPACE
            START_POS = START_POS + LINE_SPACE

            If dsAdvanceReceipt.Tables("OUTSTANDING").Rows.Count > 0 Then
                If dsAdvanceReceipt.Tables("OUTSTANDING").Rows.Count = 1 Then
                    If Field5 <> "" Then
                        g.DrawString(Field5, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
                    Else
                        START_POS = START_POS + LINE_SPACE
                    End If
                Else
                    For Each outdr As DataRow In dsAdvanceReceipt.Tables("OUTSTANDING").Rows
                        Field5 = outdr("field5").ToString & ". AMT : " & outdr("AMOUNT").ToString
                        START_POS = START_POS + LINE_SPACE
                        g.DrawString(Field5, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
                    Next
                End If
            End If

            START_POS = START_POS + LINE_SPACE
            START_POS = START_POS + LINE_SPACE
            If Field5 <> "" Then
                g.DrawString(Format(Val(Amt.ToString), "0.00"), fontBoldTitle, BlackBrush, c10, START_POS, rAlign)
            Else
                START_POS = START_POS + LINE_SPACE
            End If
            START_POS = START_POS + LINE_SPACE
            START_POS = START_POS + LINE_SPACE
            START_POS = START_POS + LINE_SPACE


            If Remarks <> "" Then
                g.DrawString(Remarks, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
                START_POS = START_POS + LINE_SPACE
                START_POS = START_POS + LINE_SPACE
            End If
            If dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field1").ToString.Contains("RECEIPT  VOUCHER") Then
                START_POS = START_POS + LINE_SPACE
            Else
                If Remark1 <> "" Then
                    g.DrawString(Remark1, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
                    START_POS = START_POS + LINE_SPACE
                End If
            End If
            If dsAdvanceReceipt.Tables("ISSUE").Rows.Count > 0 Then
                Dim GrswA As String = dsAdvanceReceipt.Tables("ISSUE").Rows(0).Item("GRSWT")
                Dim AmtA As String = dsAdvanceReceipt.Tables("ISSUE").Rows(0).Item("AMOUNT")
            End If
            START_POS = START_POS + LINE_SPACE
            printCustoInfo = False
            'ADVANCE ONLY DISPLAY
            If dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("Field1").ToString.Contains("ADVANCE ") Or dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field1").ToString.Contains("RECEIPT  VOUCHER") Or dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field1").ToString.Contains("INVOICE") Then
                START_POS = START_POS + LINE_SPACE + 120
            End If
        End If
    End Sub
    Public Sub CustomerInfoGiftVoucher(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        Dim Cusaccode As String = ""
        Dim grate As String = ""
        Dim srate As String = ""
        Dim BBill As String = ""
        Dim CusGSTNo As String = ""
        Dim rAlign As New StringFormat
        rAlign.Alignment = StringAlignment.Far
        g.DrawString(Print_Ver_No.ToString, fontRegular6, SilverBrush, c9 + 10, 5, LeftFormat)
        g.DrawRectangle(New Pen(Color.Silver, 1), c8 - 20, 28, 10, 10)
        g.DrawString("Original for Recipient", fontBold, BlackBrush, c8, 25, LeftFormat)
        g.DrawRectangle(New Pen(Color.Silver, 1), c8 - 20, 48, 10, 10)
        g.DrawString("Duplicate for Supplier", fontBold, BlackBrush, c8, 45, LeftFormat)

        If _SILVERBILLPRINT = "Y" Then
            g.DrawRectangle(New Pen(Color.Silver, 1), c8 - 20, 68, 10, 10)
            g.DrawString("Triplicate for Supplier", fontBold, BlackBrush, c8, 65, LeftFormat)
        End If
        If _SILVERBILLPRINT = "Y" And _SILVERBILLPRINT_HEADER = "Y" Then
            Dim Strimgpath As String = ""
            Dim logo As Image
            If IO.File.Exists(Application.StartupPath & "\VBJ_LOGO.jpg") Then
                Strimgpath = Application.StartupPath & "\VBJ_LOGO.jpg"
                logo = Image.FromFile(Strimgpath)
                '' e.Graphics.DrawImage(logo, c3, 80, 120, 70)
            End If
            Dim dtcomp As New DataTable
            'strsql = " SELECT * FROM " & cnAdminDb & "..COMPANY "
            'strsql += vbCrLf + "WHERE COMPANYID='" & strCompanyId & "'"
            'da = New OleDbDataAdapter(strsql, cn)
            'dtcomp = New DataTable
            'da.Fill(dtcomp)
            strsql = " SELECT TOP 1  * FROM " & cnAdminDb & "..COSTCENTRE "
            strsql += vbCrLf + "WHERE COSTID='" & cnCostId & "'"
            da = New OleDbDataAdapter(strsql, cn)
            dtcomp = New DataTable
            da.Fill(dtcomp)

            If dtcomp.Rows.Count > 0 Then
                Dim Statename As String = ""
                strsql = " SELECT ISNULL(STATENAME,'') STATENAME FROM " & cnAdminDb & "..STATEMAST "
                strsql += vbCrLf + " WHERE STATEID = " & dtcomp.Rows(0).Item("STATEID").ToString & " "
                Statename = GetSqlValue(cn, strsql)
                g.DrawString(printcpy, fontBoldTitle, Brushes.Black, c4, 20, LeftFormat)
                g.DrawString("TAX INVOICE", fontBoldTitle, Brushes.Black, c4, 40, LeftFormat)
                g.DrawString(StrConv(strCompanyName.ToString, VbStrConv.ProperCase), fontBoldHead, BlackBrush, c4 - 60, 80, LeftFormat)
                g.DrawString(dtcomp.Rows(0).Item("ADDRESS1").ToString, fontBold, BlackBrush, c4 - 50, 115, LeftFormat)
                g.DrawString("GSTIN :" & dtcomp.Rows(0).Item("GSTNO").ToString, fontBold, BlackBrush, c7, 115, LeftFormat)
                g.DrawString(dtcomp.Rows(0).Item("ADDRESS2").ToString, fontBold, BlackBrush, c4 - 50, 130, LeftFormat)
                g.DrawString("STATE :" & Statename.ToString, fontBold, BlackBrush, c7, 130, LeftFormat)
                g.DrawString("Phone :" & dtcomp.Rows(0).Item("PHONE").ToString, fontBold, BlackBrush, c4 - 50, 145, LeftFormat)
                ''g.DrawString("PANNO :" & dtcomp.Rows(0).Item("PANNO").ToString, fontBold, BlackBrush, c7, 145, LeftFormat)
            End If
        End If

        Dim dtcustleft As DataTable

        If dsGiftvoucher.Tables("CUSTOMER").Rows.Count > 0 And dsGiftvoucher.Tables("OUTSTANDING").Rows.Count > 0 Then
            strsql = " SELECT * FROM " & cnAdminDb & "..PERSONALINFO "
            strsql += vbCrLf + "WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = '" & pBatchno & "')"
            da = New OleDbDataAdapter(strsql, cn)
            dtCustInfo = New DataTable
            da.Fill(dtCustInfo)
            If dtCustInfo.Rows.Count > 0 Then
                Cusaccode = dtCustInfo.Rows(0).Item("accode").ToString
            End If
            Dim pname As String = dsGiftvoucher.Tables("CUSTOMER").Rows(0).Item("pname").ToString
            Dim Address1 As String = dsGiftvoucher.Tables("CUSTOMER").Rows(0).Item("address1").ToString
            Dim Address2 As String = dsGiftvoucher.Tables("CUSTOMER").Rows(0).Item("address2").ToString
            Dim Mobile As String = dsGiftvoucher.Tables("CUSTOMER").Rows(0).Item("mobileno").ToString
            Dim Tin As String = dsGiftvoucher.Tables("CUSTOMER").Rows(0).Item("tin").ToString
            Dim gstno As String = dsGiftvoucher.Tables("CUSTOMER").Rows(0).Item("GSTNO").ToString
            Dim panno As String = dsGiftvoucher.Tables("CUSTOMER").Rows(0).Item("PANNO").ToString
            If dtCustInfo.Rows(0).Item("STATEID").ToString <> "" Then
                Dim Qry As String = ""
                Qry = " SELECT ISNULL(STATENAME,'') STATENAME FROM " & cnAdminDb & "..STATEMAST "
                Qry += vbCrLf + " WHERE STATEID = " & dtCustInfo.Rows(0).Item("STATEID").ToString & " "
                placeofSupply = GetSqlValue(cn, Qry)
            End If
            If Tin = "" Then
                Tin = panno
            End If
            Dim Tranno As String = dsGiftvoucher.Tables("OUTSTANDING").Rows(0).Item("TRANNO").ToString
            Dim Billdate As String = dsGiftvoucher.Tables("OUTSTANDING").Rows(0).Item("billdate").ToString
            Dim Amt As Double = dsGiftvoucher.Tables("OUTSTANDING").Compute("SUM(amount)", "").ToString ''dsGiftvoucher.Tables("OUTSTANDING").Rows(0).Item("amount").ToString
            Dim AmtWords As String = ConvertRupees.RupeesToWord(Amt)
            NodeId = dsGiftvoucher.Tables("OUTSTANDING").Rows(0).Item("node").ToString
            Dim address_head As String = "Name And Address :"

            dtcustleft = New DataTable
            strsql = vbCrLf + "SELECT '' LEFT1,'' RIGHT1 "
            cmd = New OleDbCommand(strsql, cn)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtcustleft)
            'left
            dtcustleft.Rows.Add(address_head, "")
            dtcustleft.Rows.Add(pname, "")
            dtcustleft.Rows.Add(Address1, "")
            dtcustleft.Rows.Add(Address2, "")
            dtcustleft.Rows.Add(IIf(Mobile <> "", " CELL : " & Mobile, "") & IIf(Tin <> "", " PAN : " & Tin, "") & IIf(gstno <> "", " GSTNO : " & gstno, ""), "")
            'right
            dtcustleft.Rows.Add("", "GIFT VOUCHER")
            dtcustleft.Rows.Add("", "GIFT NO    : " & Tranno)

            dtcustleft.Rows.Add("", "Date       : " & Billdate)
            If Cusaccode <> "" Then
                dtcustleft.Rows.Add("", "Accode : " & Cusaccode)
            End If

            funcCustomerQrcode(g, e, 510, START_POS)
            Dim leftcnt As Integer = 0
            For I As Integer = 0 To dtcustleft.Rows.Count - 1
                If dtcustleft.Rows(I).Item("LEFT1").ToString.Trim <> "" Then
                    If dtcustleft.Rows(I).Item("LEFT1").ToString.Contains("Name") Then
                        g.DrawString(dtcustleft.Rows(I).Item("LEFT1").ToString, fontBold, Brushes.Black, c1, START_POS)
                        ''If _SILVERBILLPRINT <> "Y" Then
                        ''    If printcpy <> "" Then
                        ''        g.DrawString(printcpy, fontBoldTitle, Brushes.Black, c4, START_POS)
                        ''    End If
                        ''    g.DrawString(Field3, fontBoldTitle, Brushes.Black, c4, START_POS + LINE_SPACE + 10)
                        ''End If
                        ''If _SILVERBILLPRINT = "Y" Then
                        ''    g.DrawString(Field3, fontBoldTitle, Brushes.Black, c4, START_POS + LINE_SPACE + 10)
                        ''End If
                        If printcpy <> "" Then
                            g.DrawString(printcpy, fontBoldTitle, Brushes.Black, c4, START_POS)
                        End If
                        leftcnt += 1
                    Else
                        g.DrawString(dtcustleft.Rows(I).Item("LEFT1").ToString, fontRegular, Brushes.Black, c1, START_POS)
                        leftcnt += 1
                    End If
                    START_POS = START_POS + LINE_SPACE
                End If
            Next
            If leftcnt < 3 Then
                START_POS = START_POS + LINE_SPACE
                START_POS = START_POS + LINE_SPACE
            End If
            Dim tempstartpos As Integer = 0
            tempstartpos = START_POS
            START_POS = TSTART_POS
            For I As Integer = 0 To dtcustleft.Rows.Count - 1
                If dtcustleft.Rows(I).Item("RIGHT1").ToString.Trim <> "" Then
                    If dtcustleft.Rows(I).Item("RIGHT1").ToString.Contains("JEWELS") Then
                        g.DrawString(dtcustleft.Rows(I).Item("RIGHT1").ToString, fontBold, Brushes.Black, c8 - 20, START_POS)
                    Else
                        g.DrawString(dtcustleft.Rows(I).Item("RIGHT1").ToString, fontRegular, Brushes.Black, c8 - 20, START_POS)
                    End If
                    START_POS = START_POS + LINE_SPACE
                End If
            Next
            START_POS = IIf(START_POS > tempstartpos, START_POS, tempstartpos)
            g.DrawLine(Pens.Silver, c1, START_POS, c10, START_POS)
            START_POS = START_POS + LINE_SPACE

            TitleGift(e.Graphics, e) ''GIFTTTT

            strsql = " SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "GIFTVOUCHER] "
            da = New OleDbDataAdapter(strsql, cn)
            Dim dtGiftInfo As DataTable
            dtGiftInfo = New DataTable
            da.Fill(dtGiftInfo)
            If dtGiftInfo.Rows.Count > 0 Then
                For i As Integer = 0 To dtGiftInfo.Rows.Count - 1
                    g.DrawString(dtGiftInfo.Rows(i).Item("SRNO").ToString, fontRegular, Brushes.Black, c1, START_POS, LeftFormat)
                    g.DrawString(dtGiftInfo.Rows(i).Item("RUNNO").ToString, fontRegular, Brushes.Black, c3, START_POS, LeftFormat)
                    g.DrawString(dtGiftInfo.Rows(i).Item("UNIT").ToString, fontRegular, Brushes.Black, c5, START_POS, RightFormat)
                    g.DrawString(dtGiftInfo.Rows(i).Item("AMOUNT").ToString, fontRegular, Brushes.Black, c7 + 10, START_POS, RightFormat)
                    g.DrawString(dtGiftInfo.Rows(i).Item("AMOUNT").ToString, fontRegular, Brushes.Black, c10, START_POS, RightFormat)
                    START_POS = START_POS + LINE_SPACE
                Next
            End If

            'g.DrawString(Field2, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
            'If Field5 = "" Then
            '    g.DrawString(Format(Val(Amt.ToString), "0.00"), fontBoldTitle, BlackBrush, c10, START_POS, rAlign)
            'End If
            'START_POS = START_POS + LINE_SPACE
            'START_POS = START_POS + LINE_SPACE
            'If Field5 <> "" Then
            '    g.DrawString(Field5, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
            'Else
            '    START_POS = START_POS + LINE_SPACE
            'End If
            'START_POS = START_POS + LINE_SPACE
            'START_POS = START_POS + LINE_SPACE
            'If Field5 <> "" Then
            '    g.DrawString(Format(Val(Amt.ToString), "0.00"), fontBoldTitle, BlackBrush, c10, START_POS, rAlign)
            'Else
            '    START_POS = START_POS + LINE_SPACE
            'End If
            'START_POS = START_POS + LINE_SPACE
            'START_POS = START_POS + LINE_SPACE
            'START_POS = START_POS + LINE_SPACE


            'If Remarks <> "" Then
            '    g.DrawString(Remarks, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
            '    START_POS = START_POS + LINE_SPACE
            '    START_POS = START_POS + LINE_SPACE
            'End If
            'If dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field1").ToString.Contains("RECEIPT  VOUCHER") Then
            '    START_POS = START_POS + LINE_SPACE
            'Else
            '    If Remark1 <> "" Then
            '        g.DrawString(Remark1, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
            '        START_POS = START_POS + LINE_SPACE
            '    End If
            'End If
            'If dsAdvanceReceipt.Tables("ISSUE").Rows.Count > 0 Then
            '    Dim GrswA As String = dsAdvanceReceipt.Tables("ISSUE").Rows(0).Item("GRSWT")
            '    Dim AmtA As String = dsAdvanceReceipt.Tables("ISSUE").Rows(0).Item("AMOUNT")
            'End If
            'START_POS = START_POS + LINE_SPACE
            'printCustoInfo = False
            ''ADVANCE ONLY DISPLAY
            'If dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("Field1").ToString.Contains("ADVANCE ") Or dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field1").ToString.Contains("RECEIPT  VOUCHER") Or dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field1").ToString.Contains("INVOICE") Then
            '    START_POS = START_POS + LINE_SPACE + 120
            'End If
        End If
    End Sub
    Public Sub GiftVoucherPrint(ByVal pBatchno As String, ByVal _Trandate As String)
        strsql = ""
        strsql += vbCrLf + " EXEC " & cnAdminDb & "..PROC_GIFTVOUCHERBILLPRINTVIEW_A4N"
        strsql += vbCrLf + " @ADMINDB ='" & cnAdminDb & "'"
        strsql += vbCrLf + " ,@STOCKDB ='" & cnStockDb & "'"
        strsql += vbCrLf + " ,@SYSID ='" & SystemName & "'"
        strsql += vbCrLf + " ,@BILLDATE ='" & _Trandate & "'"
        strsql += vbCrLf + " ,@BATCHNO ='" & pBatchno & "'"
        strsql += vbCrLf + " ,@STEP ='1'"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = ""
        strsql += vbCrLf + " EXEC " & cnAdminDb & "..PROC_GIFTVOUCHERBILLPRINTVIEW_A4N"
        strsql += vbCrLf + " @ADMINDB ='" & cnAdminDb & "'"
        strsql += vbCrLf + " ,@STOCKDB ='" & cnStockDb & "'"
        strsql += vbCrLf + " ,@SYSID ='" & SystemName & "'"
        strsql += vbCrLf + " ,@BILLDATE ='" & _Trandate & "'"
        strsql += vbCrLf + " ,@BATCHNO ='" & pBatchno & "'"
        strsql += vbCrLf + " ,@STEP ='2'"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = ""
        strsql += vbCrLf + " EXEC " & cnAdminDb & "..PROC_GIFTVOUCHERBILLPRINTVIEW_A4N"
        strsql += vbCrLf + " @ADMINDB ='" & cnAdminDb & "'"
        strsql += vbCrLf + " ,@STOCKDB ='" & cnStockDb & "'"
        strsql += vbCrLf + " ,@SYSID ='" & SystemName & "'"
        strsql += vbCrLf + " ,@BILLDATE ='" & _Trandate & "'"
        strsql += vbCrLf + " ,@BATCHNO ='" & pBatchno & "'"
        strsql += vbCrLf + " ,@STEP ='3'"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = " SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "GIFTVOUCHER]"
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dsGiftvoucher, "OUTSTANDING")

        strsql = " SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "GIFTVOUCHER_ACCTRAN]"
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dsGiftvoucher, "ACCTRAN")

        strsql = " SELECT * FROM TEMPTABLEDB..[TEMP" & SystemName & "CUSTGIFTVOUCHER]"
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dsGiftvoucher, "CUSTOMER")

    End Sub
    Public Sub AdvancePrint(ByVal pBatchno As String, ByVal _Trandate As String, Optional __Paymode As String = "", Optional CACQCR As String = "")
        strsql = ""
        strsql += vbCrLf + " EXEC " & cnAdminDb & "..PROC_ADVANCEBILLPRINTVIEW_A4N"
        strsql += vbCrLf + " @ADMINDB ='" & cnAdminDb & "'"
        strsql += vbCrLf + " ,@STOCKDB ='" & cnStockDb & "'"
        strsql += vbCrLf + " ,@SYSID ='" & SystemName & "'"
        strsql += vbCrLf + " ,@BILLDATE ='" & _Trandate & "'"
        strsql += vbCrLf + " ,@BATCHNO ='" & pBatchno & "'"
        strsql += vbCrLf + " ,@PAYMODE ='" & __Paymode & "'"
        strsql += vbCrLf + " ,@CACQCR ='" & CACQCR & "'"
        strsql += vbCrLf + " ,@STEP ='1'"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        Dim dt As New DataTable
        strsql = ""
        strsql += vbCrLf + " EXEC " & cnAdminDb & "..PROC_ADVANCEBILLPRINTVIEW_A4N"
        strsql += vbCrLf + " @ADMINDB ='" & cnAdminDb & "'"
        strsql += vbCrLf + " ,@STOCKDB ='" & cnStockDb & "'"
        strsql += vbCrLf + " ,@SYSID ='" & SystemName & "'"
        strsql += vbCrLf + " ,@BILLDATE ='" & _Trandate & "'"
        strsql += vbCrLf + " ,@BATCHNO ='" & pBatchno & "'"
        strsql += vbCrLf + " ,@PAYMODE ='" & __Paymode & "'"
        strsql += vbCrLf + " ,@CACQCR ='" & CACQCR & "'"
        strsql += vbCrLf + " ,@STEP ='2'"
        cmd = New OleDbCommand(strsql, cn)
        Dim reader As OleDbDataReader = cmd.ExecuteReader
        dt.Load(reader)
        Dim TempAcc As String = ""
        Dim TempBatch As String = ""
        If dt.Rows.Count > 0 Then
            TempBatch = ""
            For i As Integer = 0 To dt.Rows.Count - 1
                strsql = "INSERT INTO TEMPTABLEDB..[TEMP" & SystemName & "ACCTRANADVANCERECEIPT](ACCODE,AMOUNT,BATCHNO_N) VALUES "
                strsql += " ('" & dt.Rows(i).Item("AMT").ToString & "', '" & dt.Rows(i).Item("AMOUNT").ToString & "', '" & TempBatch & "')"
                cmd = New OleDbCommand(strsql, cn)
                cmd.ExecuteNonQuery()
            Next
        End If
        strsql = ""
        strsql += vbCrLf + " EXEC " & cnAdminDb & "..PROC_ADVANCEBILLPRINTVIEW_A4N"
        strsql += vbCrLf + " @ADMINDB ='" & cnAdminDb & "'"
        strsql += vbCrLf + " ,@STOCKDB ='" & cnStockDb & "'"
        strsql += vbCrLf + " ,@SYSID ='" & SystemName & "'"
        strsql += vbCrLf + " ,@BILLDATE ='" & _Trandate & "'"
        strsql += vbCrLf + " ,@BATCHNO ='" & pBatchno & "'"
        strsql += vbCrLf + " ,@PAYMODE ='" & __Paymode & "'"
        strsql += vbCrLf + " ,@CACQCR ='" & CACQCR & "'"
        strsql += vbCrLf + " ,@STEP ='3'"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = ""
        strsql += vbCrLf + " EXEC " & cnAdminDb & "..PROC_ADVANCEBILLPRINTVIEW_A4N"
        strsql += vbCrLf + " @ADMINDB ='" & cnAdminDb & "'"
        strsql += vbCrLf + " ,@STOCKDB ='" & cnStockDb & "'"
        strsql += vbCrLf + " ,@SYSID ='" & SystemName & "'"
        strsql += vbCrLf + " ,@BILLDATE ='" & _Trandate & "'"
        strsql += vbCrLf + " ,@BATCHNO ='" & pBatchno & "'"
        strsql += vbCrLf + " ,@PAYMODE ='" & __Paymode & "'"
        strsql += vbCrLf + " ,@CACQCR ='" & CACQCR & "'"
        strsql += vbCrLf + " ,@STEP ='4'"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        strsql = "SELECT DISTINCT BATCHNO FROM " & cnAdminDb & "..ITEMTAG T WHERE BATCHNO='" & pBatchno & "'  " ' DISTINCT NOT USED ORDER BY ITEMID 
        da = New OleDbDataAdapter(strsql, cn)
        Dim DtAdvRecor As New DataTable
        da.Fill(DtAdvRecor)
        If DtAdvRecor.Rows.Count > 0 Then            '
            strsql = ""
            strsql += vbCrLf + " EXEC " & cnAdminDb & "..PROC_ADVANCEBILLPRINTVIEW_A4N"
            strsql += vbCrLf + " @ADMINDB ='" & cnAdminDb & "'"
            strsql += vbCrLf + " ,@STOCKDB ='" & cnStockDb & "'"
            strsql += vbCrLf + " ,@SYSID ='" & SystemName & "'"
            strsql += vbCrLf + " ,@BILLDATE ='" & _Trandate & "'"
            strsql += vbCrLf + " ,@BATCHNO ='" & pBatchno & "'"
            strsql += vbCrLf + " ,@PAYMODE ='" & __Paymode & "'"
            strsql += vbCrLf + " ,@CACQCR ='" & CACQCR & "'"
            strsql += vbCrLf + " ,@STEP ='5.0'"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        Else
            strsql = ""
            strsql += vbCrLf + " EXEC " & cnAdminDb & "..PROC_ADVANCEBILLPRINTVIEW_A4N"
            strsql += vbCrLf + " @ADMINDB ='" & cnAdminDb & "'"
            strsql += vbCrLf + " ,@STOCKDB ='" & cnStockDb & "'"
            strsql += vbCrLf + " ,@SYSID ='" & SystemName & "'"
            strsql += vbCrLf + " ,@BILLDATE ='" & _Trandate & "'"
            strsql += vbCrLf + " ,@BATCHNO ='" & pBatchno & "'"
            strsql += vbCrLf + " ,@PAYMODE ='" & __Paymode & "'"
            strsql += vbCrLf + " ,@CACQCR ='" & CACQCR & "'"
            strsql += vbCrLf + " ,@STEP ='5.1'"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        End If
        strsql = ""
        strsql += vbCrLf + " EXEC " & cnAdminDb & "..PROC_ADVANCEBILLPRINTVIEW_A4N"
        strsql += vbCrLf + " @ADMINDB ='" & cnAdminDb & "'"
        strsql += vbCrLf + " ,@STOCKDB ='" & cnStockDb & "'"
        strsql += vbCrLf + " ,@SYSID ='" & SystemName & "'"
        strsql += vbCrLf + " ,@BILLDATE ='" & _Trandate & "'"
        strsql += vbCrLf + " ,@BATCHNO ='" & pBatchno & "'"
        strsql += vbCrLf + " ,@PAYMODE ='" & __Paymode & "'"
        strsql += vbCrLf + " ,@CACQCR ='" & CACQCR & "'"
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
        strsql += vbCrLf + " EXEC " & cnAdminDb & "..PROC_ADVANCEBILLPRINTVIEW_A4N"
        strsql += vbCrLf + " @ADMINDB ='" & cnAdminDb & "'"
        strsql += vbCrLf + " ,@STOCKDB ='" & cnStockDb & "'"
        strsql += vbCrLf + " ,@SYSID ='" & SystemName & "'"
        strsql += vbCrLf + " ,@BILLDATE ='" & _Trandate & "'"
        strsql += vbCrLf + " ,@BATCHNO ='" & pBatchno & "'"
        strsql += vbCrLf + " ,@PAYMODE ='" & __Paymode & "'"
        strsql += vbCrLf + " ,@CACQCR ='" & CACQCR & "'"
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

    Public Sub AdvancePrint_12_JAN_2022(ByVal pBatchno As String, ByVal _Trandate As String)
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
        strsql += vbCrLf + " CASE WHEN PAYMODE = 'RE' THEN 'Due No ' ELSE CASE WHEN PAYMODE = 'MP' THEN 'Payment No ' ELSE 'Invoice No ' END END"
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
                    PrintDialog1.PrinterSettings.PrinterName = cmbPrinte_Name.Text
                    PrtDoc.PrinterSettings.Copies = 1
                    PrtDoc.PrintController = New System.Drawing.Printing.StandardPrintController
                    If ChkCusCopy.Checked Then printcpy = "CUSTOMER COPY" : START_POS = TSTART_POS : PagecountSale = 0 : PrtDoc.Print()
                    If ChkOffCopy.Checked Then
                        NoofPrint = 0
                        printcpy = "ACCOUNTS COPY"
                        START_POS = TSTART_POS
                        PagecountSale = 0
                        PrtDoc.Print()
                    End If
                    For T As Integer = 0 To dtPrint.Rows.Count - 1
                        If dtPrint.Rows(T).Item("TRANTYPE").ToString = "PU" Then
                            If SMALLBILLPRINT.ToString <> "" And chkSmallPrint.Checked Then
                                _smallprint_trantype = "PU"
                                PrtDiaPurSmall.Document = PrtPurSmall
                                PrtPurSmall.PrinterSettings.Copies = 1
                                PrtPurSmall.PrinterSettings.PrinterName = SMALLBILLPRINT.ToString
                                PrtPurSmall.PrintController = New System.Drawing.Printing.StandardPrintController
                                PrtPurSmall.Print()
                            End If
                        End If
                        If dtPrint.Rows(T).Item("TRANTYPE").ToString = "SR" Then
                            If SMALLBILLPRINT.ToString <> "" And chkSmallPrint.Checked Then
                                _smallprint_trantype = "SR"
                                PrtDiaPurSmall.Document = PrtPurSmall
                                PrtPurSmall.PrinterSettings.Copies = 1
                                PrtPurSmall.PrinterSettings.PrinterName = SMALLBILLPRINT.ToString
                                PrtPurSmall.PrintController = New System.Drawing.Printing.StandardPrintController
                                PrtPurSmall.Print()
                            End If
                        End If
                        If dtPrint.Rows(T).Item("TRANTYPE").ToString = "SA" And chkpartlysale.Checked = True Then
                            strsql = " SELECT "
                            strsql += vbCrLf + " (SUM(T.TAGGRSWT) - SUM(T.GRSWT)) BALGRSWT,(SUM(T.TAGNETWT) - SUM(T.NETWT)) BALNETWT,"
                            strsql += vbCrLf + " T.TRANTYPE,EMPID,(SELECT TOP 1 EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE T.EMPID=EMPID)EMPNAME, USERID,SNO"
                            strsql += vbCrLf + " ,(SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)ITEMNAME"
                            strsql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS T "
                            strsql += vbCrLf + " WHERE "
                            strsql += vbCrLf + " TRANDATE='" & strBilldate & "' AND BATCHNO='" & pBatchno & "' AND TRANTYPE IN ('SA')  AND  TAGGRSWT > 0"
                            strsql += vbCrLf + "  GROUP BY BATCHNO,EMPID,USERID,TRANTYPE,ITEMID,SNO  HAVING (SUM(T.TAGGRSWT) - SUM(T.GRSWT))>0 "
                            Dim dtBillPrint = New DataTable
                            cmd = New OleDbCommand(strsql, cn)
                            da = New OleDbDataAdapter(cmd)
                            da.Fill(dtBillPrint)
                            If dtBillPrint.Rows.Count = 0 Then
                            Else
                                _smallprint_trantype = "SA"
                                PrtDiaPurSmall.Document = PrtPurSmall
                                PrtPurSmall.PrinterSettings.Copies = 1
                                PrtPurSmall.PrinterSettings.PrinterName = SMALLBILLPRINT.ToString
                                PrtPurSmall.PrintController = New System.Drawing.Printing.StandardPrintController
                                PrtPurSmall.Print()
                            End If
                        End If
                        If dtPrint.Rows(T).Item("TRANTYPE").ToString = "SA" And chknontag.Checked = True Then
                            strsql = " SELECT GRSWT "
                            strsql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS T "
                            strsql += vbCrLf + " WHERE "
                            strsql += vbCrLf + " TRANDATE='" & strBilldate & "' AND BATCHNO='" & pBatchno & "' AND TRANTYPE IN ('SA')  AND  ISNULL(TAGNO,'') = ''"
                            Dim dtBillPrint = New DataTable
                            cmd = New OleDbCommand(strsql, cn)
                            da = New OleDbDataAdapter(cmd)
                            da.Fill(dtBillPrint)
                            If dtBillPrint.Rows.Count = 0 Then
                            Else
                                _smallprint_trantype = "NT"
                                PrtDiaPurSmall.Document = PrtPurSmall
                                PrtPurSmall.PrinterSettings.Copies = 1
                                PrtPurSmall.PrinterSettings.PrinterName = SMALLBILLPRINT.ToString
                                PrtPurSmall.PrintController = New System.Drawing.Printing.StandardPrintController
                                PrtPurSmall.Print()
                            End If
                        End If
                        If dtPrint.Rows(T).Item("TRANTYPE").ToString = "SA" And chkGuarantee.Checked = True Then
                            strsql = " SELECT TOP 1 TRANNO "
                            strsql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS T "
                            strsql += vbCrLf + " WHERE "
                            strsql += vbCrLf + " TRANDATE='" & strBilldate & "' AND BATCHNO='" & pBatchno & "' AND TRANTYPE IN ('SA') "
                            Dim _mailtranno As String = objGPack.GetSqlValue(strsql, "TRANNO", "")
                            Dim doc As New Spire.Pdf.PdfDocument
                            PrtGuarantee.DocumentName = PrtGuarantee.DocumentName & "_" & _mailtranno
                            Dim tempFile As String = Application.StartupPath & "\" & PrtGuarantee.DocumentName & ".xps"
                            Dim tempFile1 As String = Application.StartupPath & "\" & PrtGuarantee.DocumentName & ".pdf"
                            If System.IO.File.Exists(tempFile) Then
                                System.IO.File.Delete(tempFile)
                            End If
                            If System.IO.File.Exists(tempFile1) Then
                                System.IO.File.Delete(tempFile1)
                            End If
                            PrtGuarantee.PrinterSettings.PrinterName = "Microsoft XPS Document Writer"
                            PrtGuarantee.PrinterSettings.Copies = 1
                            PrtGuarantee.PrinterSettings.PrintToFile = True
                            PrtGuarantee.DefaultPageSettings.PaperSize = New System.Drawing.Printing.PaperSize("custom", 793, 1122)
                            PrtGuarantee.PrinterSettings.PrintFileName = tempFile
                            PrtGuarantee.PrintController = New System.Drawing.Printing.StandardPrintController
                            PrtGuarantee.Print()
                            Threading.Thread.Sleep(2000)
                            'doc.LoadFromXPS(tempFile)
                            doc.LoadFromXPS(tempFile)
                            tempFile = tempFile.Replace(".xps", ".PDF")
                            doc.SaveToFile(tempFile)
                            doc.Close()
                            Dim TOMAILGUARANTEE As String = GetAdmindbSoftValue("TOMAILGUARANTEE", "")
                            If TOMAILGUARANTEE = "" Then
                                strsql = " SELECT EMAIL FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO='" & pBatchno & "')"
                                Dim MAILID As String = GetSqlValue(cn, strsql)
                                If MAILID <> "" Then
                                    NEWMAILSEND(MAILID, tempFile)
                                Else
                                    MsgBox("Mail id not found", MsgBoxStyle.Information)
                                End If
                            Else
                                NEWMAILSEND(TOMAILGUARANTEE, tempFile)
                            End If
                            If System.IO.File.Exists(tempFile) Then
                                System.IO.File.Delete(tempFile)
                            End If
                        End If
                    Next
                    Exit Sub
                ElseIf dtPrint.Rows(k).Item("TRANTYPE").ToString = "AI" _
                    Or dtPrint.Rows(k).Item("TRANTYPE").ToString = "AR" Then
                    Appsummary = False
                    TrantypeMI = dtPrint.Rows(k).Item("TRANTYPE").ToString()
                    PrintSALEINVOICE(pBatchno, dtPrint.Rows(k).Item("TRANTYPE").ToString())
                    PrtDiaAppDoc.Document = PrtAppDoc
                    If APPROVALBILLPRINT <> "" Then
                        If _SILVERBILLPRINT <> "Y" Then
                            PrtDiaAppDoc.PrinterSettings.PrinterName = APPROVALBILLPRINT.ToString
                        Else
                            PrtDiaAppDoc.PrinterSettings.PrinterName = cmbPrinte_Name.Text
                        End If
                    Else
                        PrtDiaAppDoc.PrinterSettings.PrinterName = cmbPrinte_Name.Text
                    End If
                    PrtAppDoc.PrinterSettings.Copies = 1
                    PrtAppDoc.PrintController = New System.Drawing.Printing.StandardPrintController
                    If ChkCusCopy.Checked Then
                        printcpy = "CUSTOMER COPY"
                        START_POS = TSTART_POS
                        PagecountSale = 0
                        PrtAppDoc.Print()
                        If chkapp.Checked = True Then
                            If Appsummary = True And dtPrint.Rows(k).Item("TRANTYPE").ToString = "AI" Then
                                START_POS = TSTART_POS
                                PagecountSale = 0
                                PrtAppDoc.Print()
                                Appsummary = False
                            End If
                        End If
                    End If
                    If ChkOffCopy.Checked Then
                        Appsummary = False
                        NoofPrint = 0
                        printcpy = "ACCOUNTS COPY"
                        START_POS = TSTART_POS
                        PagecountSale = 0
                        PrtAppDoc.Print()
                        If chkapp.Checked = True Then
                            If Appsummary = True And dtPrint.Rows(k).Item("TRANTYPE").ToString = "AI" Then
                                START_POS = TSTART_POS
                                PagecountSale = 0
                                PrtAppDoc.Print()
                                Appsummary = False
                            End If
                        End If
                    End If
                    Exit Sub
                ElseIf dtPrint.Rows(k).Item("TRANTYPE").ToString = "IIS" _ '' MIMR_IIS
                    Or dtPrint.Rows(k).Item("TRANTYPE").ToString = "IMP" _
                    Or dtPrint.Rows(k).Item("TRANTYPE").ToString = "IAP" _
                    Or dtPrint.Rows(k).Item("TRANTYPE").ToString = "IOT" _
                    Or dtPrint.Rows(k).Item("TRANTYPE").ToString = "IPU" _
                    Or dtPrint.Rows(k).Item("TRANTYPE").ToString = "ROT" _
                     Or dtPrint.Rows(k).Item("TRANTYPE").ToString = "RPU" _
                    Or dtPrint.Rows(k).Item("TRANTYPE").ToString = "RMP" _
                    Or dtPrint.Rows(k).Item("TRANTYPE").ToString = "RAP" _
                    Or dtPrint.Rows(k).Item("TRANTYPE").ToString = "RRE" Then
                    TrantypeMI = dtPrint.Rows(k).Item("TRANTYPE").ToString()
                    smithIssueReceipt(pBatchno, dtPrint.Rows(k).Item("TRANTYPE").ToString(), "NEW")
                    PrintDialog1.Document = PrtSmith
                    PrintDialog1.PrinterSettings.PrinterName = cmbPrinte_Name.Text
                    PrtSmith.PrinterSettings.Copies = 1
                    PrtSmith.PrintController = New System.Drawing.Printing.StandardPrintController
                    If ChkCusCopy.Checked Then printcpy = "CUSTOMER COPY" : START_POS = TSTART_POS : PagecountSale = 0 : PrtSmith.Print()
                    If ChkOffCopy.Checked Then
                        NoofPrint = 0
                        printcpy = "ACCOUNTS COPY"
                        START_POS = TSTART_POS
                        PagecountSale = 0
                        PrtSmith.Print()
                    End If
                    Exit Sub
                ElseIf dtPrint.Rows(k).Item("TRANTYPE").ToString = "IIN" _ '' TRANSFER
                Or dtPrint.Rows(k).Item("TRANTYPE").ToString = "RIN" Then
                    TrantypeMI = dtPrint.Rows(k).Item("TRANTYPE").ToString()
                    smithIssueReceipt(pBatchno, dtPrint.Rows(k).Item("TRANTYPE").ToString(), "NEW")
                    PrintDialog1.Document = PrtSmith
                    PrintDialog1.PrinterSettings.PrinterName = cmbPrinte_Name.Text
                    PrtSmith.PrinterSettings.Copies = 1
                    PrtSmith.PrintController = New System.Drawing.Printing.StandardPrintController
                    If ChkCusCopy.Checked Then printcpy = "CUSTOMER COPY" : START_POS = TSTART_POS : PagecountSale = 0 : PrtSmith.Print()
                    If ChkOffCopy.Checked Then
                        NoofPrint = 0
                        printcpy = "ACCOUNTS COPY"
                        START_POS = TSTART_POS
                        PagecountSale = 0
                        PrtSmith.Print()
                    End If
                    Exit Sub
                End If
            Next
        Else
            strsql = " SELECT DISTINCT * FROM ("
            strsql += vbCrLf + " SELECT DISTINCT PAYMODE TRANTYPE,TRANNO FROM " & cnStockDb & "..ACCTRAN "
            strsql += vbCrLf + " WHERE TRANDATE = '" & strBilldate & "' AND BATCHNO='" & pBatchno & "' "
            strsql += vbCrLf + " ) X "
            cmd = New OleDbCommand(strsql, cn)
            dtPrint = New DataTable
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtPrint)
            If dtPrint.Rows.Count > 0 Then
                strsql = " SELECT DISTINCT * FROM ("
                strsql += vbCrLf + " SELECT DISTINCT PAYMODE TRANTYPE,TRANNO FROM " & cnStockDb & "..ACCTRAN "
                strsql += vbCrLf + " WHERE TRANDATE = '" & strBilldate & "' AND BATCHNO='" & pBatchno & "' AND PAYMODE='GV' "
                strsql += vbCrLf + " ) X "
                cmd = New OleDbCommand(strsql, cn)
                Dim dtprintgv As DataTable
                dtprintgv = New DataTable
                da = New OleDbDataAdapter(cmd)
                da.Fill(dtprintgv)
                If dtprintgv.Rows.Count > 0 Then
                    GiftVoucherPrint(pBatchno, strBilldate)
                    PrtDiaAdvance.Document = PrintGift
                    PrtDiaAdvance.PrinterSettings.PrinterName = cmbPrinte_Name.Text
                    PrintGift.PrinterSettings.Copies = 1
                    PrintGift.PrintController = New System.Drawing.Printing.StandardPrintController()
                    If ChkCusCopy.Checked Then printcpy = "CUSTOMER COPY" : PrintGift.Print()
                    If ChkOffCopy.Checked Then
                        printcpy = "ACCOUNTS COPY "
                        START_POS = TSTART_POS
                        PagecountSale = 0
                        PrintGift.Print()
                    End If
                Else
                    For k As Integer = 0 To dtPrint.Rows.Count - 1
                        If dtPrint.Rows(k).Item("TRANTYPE").ToString = "CR" _ '' TRANSFER
                    Or dtPrint.Rows(k).Item("TRANTYPE").ToString = "MN" _
                    Or dtPrint.Rows(k).Item("TRANTYPE").ToString = "JE" _
                    Or dtPrint.Rows(k).Item("TRANTYPE").ToString = "TA" _
                    Or dtPrint.Rows(k).Item("TRANTYPE").ToString = "DN" _
                    Or dtPrint.Rows(k).Item("TRANTYPE").ToString = "CN" _
                    Or dtPrint.Rows(k).Item("TRANTYPE").ToString = "CP" Then
                            If ChkCusCopy.Checked Then
                                Dim obj As New Global.BrighttechREPORT.frmCashTransactionPrint(dtPrint.Rows(0).Item("TRANTYPE").ToString, cnStockDb _
                                                                             , strBilldate _
                                                                             , dtPrint.Rows(0).Item("TRANNO"), pBatchno _
                                                                             , "ACCTRAN", False, True, "OUTSTANDING", "", True, cmbPrinte_Name.Text, "CUSTOMER COPY")
                            End If

                            If ChkOffCopy.Checked Then
                                Dim obj1 As New Global.BrighttechREPORT.frmCashTransactionPrint(dtPrint.Rows(0).Item("TRANTYPE").ToString, cnStockDb _
                                                                         , strBilldate _
                                                                         , dtPrint.Rows(0).Item("TRANNO"), pBatchno _
                                                                         , "ACCTRAN", False, True, "OUTSTANDING", "", True, cmbPrinte_Name.Text, "ACCOUNTS COPY")
                            End If
                            Exit Sub
                        ElseIf dtPrint.Rows(k).Item("TRANTYPE").ToString = "PE" _
                        Or dtPrint.Rows(k).Item("TRANTYPE").ToString = "PC" _
                        Or dtPrint.Rows(k).Item("TRANTYPE").ToString = "PD" Then '' GST 
                            Exit Sub
                        End If
                    Next
                End If
            End If
        End If
    End Sub

    Private Sub PrtDoc_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrtDoc.PrintPage
        Try
            gvprint_flag = True
            Dim NoofPage As Integer = 0
            Dim _dt_count As Integer = 0
            Dim rAlign As New StringFormat
            rAlign.Alignment = StringAlignment.Far
            NoofPrint += 1
            Using g As Graphics = e.Graphics
                CustomerInfo(Tranno, prtBilldate, e.Graphics, e)
                If dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("SA") Or dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("RD") Then Title(e.Graphics, e)
                RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
                _dt_count = dtSales.Rows.Count
                If dtSales.Rows.Count > 0 Then
                    For PagecountSale = PagecountSale To dtSales.Rows.Count - 1
                        If Not dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("PU") _
                            And Not dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("SR") _
                            Then
                            With dtSales.Rows(PagecountSale)
                                ''Top '/************************************////
                                NoofPage += 1
                                If .Item("RESULT").ToString = "3.5" Then
                                    g.DrawLine(Pens.Silver, 710, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.0" And .Item("TYPE").ToString = "11.0" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawLine(Pens.Silver, 340, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.4" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.6" And .Item("TYPE").ToString = "12" Then
                                    g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS))
                                End If
                                g.DrawString(.Item("SRNO").ToString(), fontRegular, BlackBrush, c1, START_POS, LeftFormat)
                                If .Item("TYPE").ToString = "10" And .Item("COLHEAD").ToString = "G" Then
                                    If .Item("ITEMNAME").ToString.Length > 25 And .Item("RESULT").ToString <> "3.3" Then
                                        g.DrawString(.Item("ITEMNAME").ToString(), fontBoldsmall, BlackBrush, c3 - 20, START_POS, LeftFormat)
                                    ElseIf .Item("RESULT").ToString = "3.3" Then
                                        g.DrawString(.Item("ITEMNAME").ToString(), fontRegularsmall, BlackBrush, c3, START_POS, LeftFormat)
                                    Else
                                        g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c3, START_POS, LeftFormat)
                                    End If
                                Else
                                    If _SILVERBILLPRINT = "Y" Then
                                        If .Item("CALTYPE").ToString = "HA" And .Item("RESULT").ToString = "1.1" Then
                                            g.DrawString(.Item("ITEMNAME").ToString(), fontRegular6, BlackBrush, c3, START_POS, LeftFormat)

                                        ElseIf .Item("RESULT").ToString = "2.1" And .Item("TYPE").ToString = "1" And (.Item("COLHEAD").ToString = "G" Or .Item("COLHEAD").ToString = "S") Then
                                            g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c3, START_POS, LeftFormat)
                                        ElseIf .Item("RESULT").ToString = "3.1" And .Item("TYPE").ToString = "1" And .Item("ITEMNAME").ToString = "SCHEME BENEFIT" Then
                                            g.DrawString("", fontRegularsmall, BlackBrush, c3, START_POS, LeftFormat) ''  g.DrawString("DISCOUNT AMOUNT", fontRegularsmall, BlackBrush, c3, START_POS, LeftFormat)
                                        ElseIf .Item("RESULT").ToString = "2.2" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "V" Then
                                            If printcpy = "ACCOUNTS COPY" Then ''PARTLY SALES PRINT SILVER
                                                g.DrawString(.Item("ITEMNAME").ToString(), fontRegularsmall, BlackBrush, c3, START_POS, LeftFormat)
                                            End If
                                        ElseIf .Item("CALTYPE").ToString = "REPDEL" And .Item("RESULT").ToString = "1.1" Then
                                            If .Item("ITEMNAME").ToString = "Advance Weight" Or .Item("ITEMNAME").ToString = "Excess Weight" Then
                                                g.DrawLine(Pens.Silver, c5, (START_POS), c6 + 20, (START_POS))
                                            End If
                                            g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c3 + 80, START_POS, LeftFormat)
                                        Else
                                            g.DrawString(.Item("ITEMNAME").ToString(), fontRegularsmall, BlackBrush, c3, START_POS, LeftFormat)
                                        End If
                                    Else
                                        If .Item("CALTYPE").ToString = "HA" And .Item("RESULT").ToString = "1.1" Then
                                            g.DrawString(.Item("ITEMNAME").ToString(), fontRegular6, BlackBrush, c3, START_POS, LeftFormat)
                                        ElseIf .Item("RESULT").ToString = "2.1" And .Item("TYPE").ToString = "1" And (.Item("COLHEAD").ToString = "G" Or .Item("COLHEAD").ToString = "S") Then
                                            g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c3, START_POS, LeftFormat)
                                        ElseIf .Item("RESULT").ToString = "2.2" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "V" Then
                                            If printcpy = "ACCOUNTS COPY" Then ''PARTLY SALES PRINT SILVER
                                                g.DrawString(.Item("ITEMNAME").ToString(), fontRegularsmall, BlackBrush, c3, START_POS, LeftFormat)
                                            End If
                                        ElseIf .Item("CALTYPE").ToString = "NOEX" And .Item("TYPE").ToString = "1" And .Item("RESULT").ToString = "1.1" Then
                                            g.DrawString(.Item("ITEMNAME").ToString(), fontRegularsmall, BlackBrush, c3, START_POS, LeftFormat)
                                        ElseIf .Item("CALTYPE").ToString = "REPDEL" And .Item("RESULT").ToString = "1.1" Then
                                            If .Item("ITEMNAME").ToString = "Advance Weight" Or .Item("ITEMNAME").ToString = "Excess Weight" Then
                                                g.DrawLine(Pens.Silver, c5, (START_POS), c6 + 20, (START_POS))
                                            End If
                                            g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c3 + 80, START_POS, LeftFormat)
                                        Else
                                            g.DrawString(.Item("ITEMNAME").ToString(), fontRegularsmall, BlackBrush, c3, START_POS, LeftFormat)
                                        End If
                                    End If
                                End If
                                If .Item("RESULT").ToString = "3.6" And .Item("TYPE").ToString = "11" Then
                                    g.DrawString(.Item("HSN").ToString(), fontRegularsmall, BlackBrush, c2, START_POS, LeftFormat) 'fontBold
                                    g.DrawString(.Item("QTY").ToString(), fontBold, BlackBrush, c4, START_POS, rAlign)
                                ElseIf .Item("RESULT").ToString = "3.4" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawString(.Item("HSN").ToString(), fontAmtInWord, BlackBrush, c1, START_POS, LeftFormat)
                                    g.DrawString(.Item("QTY").ToString(), fontBold, BlackBrush, c4, START_POS, RightFormat)
                                ElseIf .Item("CALTYPE").ToString = "REPDEL" And .Item("RESULT").ToString = "1.1" Then
                                    g.DrawString(.Item("HSN").ToString(), fontAmtInWord, BlackBrush, c2, START_POS, LeftFormat)
                                Else
                                    If .Item("RESULT").ToString = "2.2" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "V" Then
                                        If printcpy = "ACCOUNTS COPY" Then ''PARTLY SALES PRINT SILVER
                                            g.DrawString(.Item("HSN").ToString(), fontRegularsmall, BlackBrush, c2, START_POS, LeftFormat)
                                        End If
                                    Else
                                        If _SILVERBILLPRINT = "Y" Then
                                            If .Item("CALTYPE").ToString = "NOEX" And .Item("TYPE").ToString = "1" And .Item("RESULT").ToString = "1.1" Then
                                                g.DrawString(.Item("HSN").ToString(), fontBold, BlackBrush, c2, START_POS, LeftFormat)
                                            Else
                                                g.DrawString(.Item("HSN").ToString(), fontRegularsmall, BlackBrush, c2, START_POS, LeftFormat)
                                            End If
                                        Else
                                            g.DrawString(.Item("HSN").ToString(), fontRegularsmall, BlackBrush, c2, START_POS, LeftFormat)
                                        End If
                                    End If
                                    If .Item("RESULT").ToString = "2.1" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" Then
                                        g.DrawString(.Item("QTY").ToString(), fontBold, BlackBrush, c4, START_POS, RightFormat)
                                    Else
                                        g.DrawString(IIf(Val(.Item("QTY").ToString()) <> 0, Val(.Item("QTY").ToString()), ""), fontRegularsmall, BlackBrush, c4, START_POS, RightFormat)
                                    End If
                                End If
                                If .Item("RESULT").ToString = "2.1" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontBold, BlackBrush, c5, START_POS, RightFormat)
                                    If _SILVERBILLPRINT = "Y" Then
                                        g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontBold, BlackBrush, c7, START_POS, RightFormat)
                                    Else
                                        g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontBold, BlackBrush, c6, START_POS, RightFormat)
                                    End If
                                Else '' 
                                    If .Item("ITEMNAME").ToString <> "" And .Item("TYPE").ToString = "1" And .Item("RESULT").ToString = "1.0" Then
                                        If .Item("BILLNAME").ToString = "C" Or .Item("BILLNAME").ToString = "G" Then

                                            If .Item("BILLNAME").ToString = "C" Then
                                                g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), 0.00), fontRegularsmall, BlackBrush, c5, START_POS, RightFormat)
                                                g.DrawString("cts ", fontRegularsmall, BlackBrush, c5 + 3, START_POS, LeftFormat)
                                                If _SILVERBILLPRINT = "Y" Then
                                                    g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), 0.00), fontRegularsmall, BlackBrush, c7, START_POS, RightFormat)
                                                    g.DrawString("cts ", fontRegularsmall, BlackBrush, c7 + 3, START_POS, LeftFormat)
                                                Else
                                                    g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), 0.00), fontRegularsmall, BlackBrush, c6, START_POS, RightFormat)
                                                    g.DrawString("cts ", fontRegularsmall, BlackBrush, c6 + 3, START_POS, LeftFormat)
                                                End If
                                            ElseIf .Item("BILLNAME").ToString = "G" Then
                                                g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), 0.00), fontRegularsmall, BlackBrush, c5, START_POS, RightFormat)
                                                g.DrawString("gms ", fontRegularsmall, BlackBrush, c5 + 3, START_POS, LeftFormat)
                                                If _SILVERBILLPRINT = "Y" Then
                                                    g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), 0.00), fontRegularsmall, BlackBrush, c7, START_POS, RightFormat)
                                                    g.DrawString("gms ", fontRegularsmall, BlackBrush, c7 + 3, START_POS, LeftFormat)
                                                Else
                                                    g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), 0.00), fontRegularsmall, BlackBrush, c6, START_POS, RightFormat)
                                                    g.DrawString("gms ", fontRegularsmall, BlackBrush, c6 + 3, START_POS, LeftFormat)
                                                End If
                                            End If
                                        Else
                                            g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), 0.00), fontRegularsmall, BlackBrush, c5, START_POS, RightFormat)
                                            If _SILVERBILLPRINT = "Y" Then
                                                g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), 0.00), fontRegularsmall, BlackBrush, c7, START_POS, RightFormat)
                                            Else
                                                g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), 0.00), fontRegularsmall, BlackBrush, c6, START_POS, RightFormat)
                                            End If
                                        End If

                                    Else
                                        If .Item("BILLNAME").ToString = "C" Or .Item("BILLNAME").ToString = "G" Then
                                            If .Item("BILLNAME").ToString = "C" Then
                                                g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontRegularsmall, BlackBrush, c5, START_POS, RightFormat)
                                                g.DrawString("cts ", fontRegularsmall, BlackBrush, c5 + 3, START_POS, LeftFormat)
                                                If _SILVERBILLPRINT = "Y" Then
                                                    g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, c7, START_POS, RightFormat)
                                                    g.DrawString("cts ", fontRegularsmall, BlackBrush, c7 + 3, START_POS, LeftFormat)
                                                Else
                                                    g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, c6, START_POS, RightFormat)
                                                    g.DrawString("cts ", fontRegularsmall, BlackBrush, c6 + 3, START_POS, LeftFormat)
                                                End If
                                            ElseIf .Item("BILLNAME").ToString = "G" Then
                                                g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontRegularsmall, BlackBrush, c5, START_POS, RightFormat)
                                                g.DrawString("gms ", fontRegularsmall, BlackBrush, c5 + 3, START_POS, LeftFormat)
                                                If _SILVERBILLPRINT = "Y" Then
                                                    g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, c7, START_POS, RightFormat)
                                                    g.DrawString("gms ", fontRegularsmall, BlackBrush, c7 + 3, START_POS, LeftFormat)
                                                Else
                                                    g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, c6, START_POS, RightFormat)
                                                    g.DrawString("gms ", fontRegularsmall, BlackBrush, c6 + 3, START_POS, LeftFormat)
                                                End If
                                            End If
                                        Else
                                            g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontRegularsmall, BlackBrush, c5, START_POS, RightFormat)
                                            If _SILVERBILLPRINT = "Y" Then
                                                g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, c7, START_POS, RightFormat)
                                            Else
                                                g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, c6, START_POS, RightFormat)
                                            End If
                                        End If
                                    End If
                                End If
                                If .Item("COLHEAD").ToString() = "G" Or .Item("COLHEAD").ToString = "P" Or .Item("COLHEAD").ToString = "V" Then
                                    If .Item("COLHEAD").ToString = "G" And (.Item("TYPE").ToString = "1" Or .Item("TYPE").ToString = "3") Then
                                        g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, START_POS, LeftFormat)
                                    ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString <> "10" Then
                                        g.DrawString(.Item("RATE").ToString(), fontRegularsmall, BlackBrush, 550, START_POS, LeftFormat)
                                    ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString <> "3.50") Then
                                        g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, START_POS, LeftFormat)
                                    ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString = "3.50") Then
                                        g.DrawString(.Item("RATE").ToString(), fontRegularsmall, BlackBrush, 550, START_POS, LeftFormat)
                                    ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10" Then
                                        g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, START_POS, LeftFormat)
                                    ElseIf .Item("COLHEAD").ToString = "V" And .Item("TYPE").ToString = "1" Then
                                        g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, START_POS, LeftFormat)
                                    ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "1" Then
                                        g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, START_POS, LeftFormat)
                                    End If
                                Else
                                    If _SILVERBILLPRINT = "Y" Then
                                        g.DrawString("", fontRegular, BlackBrush, c8, START_POS, RightFormat)
                                    Else
                                        If dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString = "PU" Or dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString = "SR" Then
                                            g.DrawString(.Item("RATE").ToString() & IIf(Val(.Item("MCGRM").ToString()) <> 0, "+" & .Item("MCGRM").ToString(), ""), fontRegular, BlackBrush, c9, START_POS, RightFormat)
                                        Else
                                            g.DrawString(.Item("RATE").ToString() & IIf(Val(.Item("MCGRM").ToString()) <> 0, "+" & .Item("MCGRM").ToString(), ""), fontRegular, BlackBrush, c8, START_POS, RightFormat)
                                        End If

                                    End If
                                End If
                                If .Item("CALTYPE").ToString = "F" Then 'Fixed Item Should Not print mC
                                    g.DrawString("", fontRegular, BlackBrush, c8, START_POS, RightFormat)
                                Else
                                    g.DrawString("", fontRegular, BlackBrush, c8, START_POS, RightFormat)
                                End If
                                If .Item("COLHEAD").ToString = "G" Or .Item("COLHEAD").ToString = "P" Or .Item("COLHEAD").ToString = "V" Then
                                    If .Item("RESULT").ToString = "2.1" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" Then
                                        If Val(.Item("AMOUNT").ToString) <> 0 Then
                                            g.DrawString(FormatNumber(.Item("AMOUNT"), 2).ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                        Else
                                            g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                        End If
                                    ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString <> "10" And .Item("RESULT").ToString <> "3.5" Then
                                        If Val(.Item("AMOUNT").ToString) <> 0 Then
                                            g.DrawString(FormatNumber(.Item("AMOUNT"), 2).ToString(), fontRegularsmall, BlackBrush, c10, START_POS, rAlign)
                                        Else
                                            g.DrawString(.Item("AMOUNT").ToString(), fontRegularsmall, BlackBrush, c10, START_POS, rAlign)
                                        End If

                                    ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString <> "3.50") Or (.Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10") Then
                                        If Val(.Item("AMOUNT").ToString) <> 0 Then
                                            g.DrawString(FormatNumber(.Item("AMOUNT"), 2).ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                        Else
                                            g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                        End If

                                        If .Item("RESULT").ToString = "3.6" And .Item("TYPE").ToString = "11" Then
                                        End If
                                    ElseIf (.Item("RESULT").ToString = "3.4" Or .Item("RESULT").ToString = "3.5") And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" Then
                                        If Val(.Item("AMOUNT").ToString) <> 0 Then
                                            g.DrawString(FormatNumber(.Item("AMOUNT"), 2).ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                        Else
                                            g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                        End If

                                    ElseIf .Item("COLHEAD").ToString = "V" And .Item("TYPE").ToString = "1" Then
                                        If Val(.Item("AMOUNT").ToString) <> 0 Then
                                            g.DrawString(FormatNumber(.Item("AMOUNT"), 2).ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                        Else
                                            g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                        End If

                                    ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString = "3.50") Then
                                        If Val(.Item("AMOUNT").ToString) <> 0 Then
                                            g.DrawString(FormatNumber(.Item("AMOUNT"), 2).ToString(), fontRegularsmall, BlackBrush, c10, START_POS, rAlign)
                                        Else
                                            g.DrawString(.Item("AMOUNT").ToString(), fontRegularsmall, BlackBrush, c10, START_POS, rAlign)
                                        End If

                                    ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10" Then
                                        If Val(.Item("AMOUNT").ToString) <> 0 Then
                                            g.DrawString(FormatNumber(.Item("AMOUNT"), 2).ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                        Else
                                            g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                        End If

                                    End If
                                Else
                                    If Val(.Item("AMOUNT").ToString) <> 0 Then
                                        g.DrawString(FormatNumber(.Item("AMOUNT"), 2).ToString(), fontRegularsmall, BlackBrush, c10, START_POS, rAlign)
                                    Else
                                        g.DrawString(.Item("AMOUNT").ToString(), fontRegularsmall, BlackBrush, c10, START_POS, rAlign)
                                    End If


                                End If
                                g.DrawString(" " & .Item("EMPID").ToString(), fontRegular7, BlackBrush, c11, START_POS, LeftFormat)
                                If (.Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" And .Item("RESULT").ToString <> "3.5") _
                                Or (.Item("TYPE").ToString = "2.1" And .Item("COLHEAD").ToString = "G") _
                                Or (.Item("TYPE").ToString = "3" And .Item("COLHEAD").ToString = "G") Then
                                    g.DrawLine(Pens.Silver, 340, (START_POS), c10, (START_POS))
                                End If
                                If START_POS >= BOTTOM_POS Then
                                    PagecountSale = PagecountSale + 1
                                    If _dt_count = PagecountSale Then
                                        GoTo gotofooter
                                    Else
                                        START_POS = TSTART_POS ' TOP AGAIN STARTING POSITION
                                        g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, BOTTOM_POS + 10)
                                        e.HasMorePages = True
                                        gvprint_flag = False
                                        Exit Sub
                                    End If
                                End If
                                'New Line Start
                                If .Item("CALTYPE").ToString = "HA" And .Item("RESULT").ToString = "1.1" Then
                                    START_POS = START_POS + LINE_SPACE
                                Else ''NOEXC
                                    START_POS = START_POS + LINE_SPACE
                                End If
                                If .Item("RESULT").ToString = "1.0" And (.Item("BILLNAME").ToString = "Y") Then '.Item("COLHEAD").ToString <> "G" And .Item("COLHEAD").ToString <> "P" And .Item("COLHEAD").ToString <> "T"
                                    g.DrawString("Fixing of Kemp, Kundan Stones needs wax, Hence at the time of melting weight loss will be there upto 10%..", fontRegularsmall, BlackBrush, c2, START_POS, LeftFormat)
                                    START_POS = START_POS + LINE_SPACE
                                End If
                                If START_POS >= BOTTOM_POS Then
                                    PagecountSale = PagecountSale + 1 ' test for last 16-03-2022
                                    If _dt_count = PagecountSale Then
                                        GoTo gotofooter
                                    Else
                                        START_POS = TSTART_POS ' TOP AGAIN STARTING POSITION
                                        g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, BOTTOM_POS + 10)
                                        e.HasMorePages = True
                                        gvprint_flag = False
                                        Exit Sub
                                    End If
                                End If
                                If (.Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" And Val(.Item("QTY").ToString) >= 2) Or
                                (.Item("TYPE").ToString = "3" And .Item("COLHEAD").ToString = "G") Then
                                    g.DrawLine(Pens.Silver, 340, (START_POS), c10, (START_POS))
                                End If
                                If _SILVERBILLPRINT = "Y" Then
                                Else
                                    If (.Item("RESULT").ToString = "3.10" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G") Then
                                        g.DrawLine(Pens.Silver, 710, (START_POS), c10, (START_POS))
                                    End If
                                End If
                                If (.Item("TYPE").ToString = "4" And .Item("COLHEAD").ToString = "T") Then
                                    If Val(.Item("AMOUNT").ToString) <> 0 Then
                                        g.DrawLine(Pens.Silver, c8, START_POS, c10, START_POS)
                                    End If
                                End If
                                If .Item("RESULT").ToString = "3.6" And .Item("TYPE").ToString = "12" Then
                                    g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.0" And .Item("TYPE").ToString = "11.0" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawLine(Pens.Silver, 340, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.4" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.5" Then
                                    g.DrawLine(Pens.Silver, 710, (START_POS), c10, (START_POS))
                                End If
                                If START_POS >= BOTTOM_POS Then
                                    PagecountSale = PagecountSale + 1
                                    If _dt_count = PagecountSale Then
                                        GoTo gotofooter
                                    Else
                                        START_POS = TSTART_POS ' TOP AGAIN STARTING POSITION
                                        g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, BOTTOM_POS + 10)
                                        e.HasMorePages = True
                                        gvprint_flag = False
                                        Exit Sub
                                    End If
                                End If
                            End With
                        End If
                        If dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("PU") _
                            Or dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("SR") _
                            Then
                            If START_POS >= BOTTOM_POS Then
                                PagecountSale = PagecountSale + 1
                                If _dt_count = PagecountSale Then
                                    GoTo gotofooter
                                Else
                                    START_POS = TSTART_POS ' TOP AGAIN STARTING POSITION
                                    g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, BOTTOM_POS + 10)
                                    e.HasMorePages = True
                                    gvprint_flag = False
                                    Exit Sub
                                End If
                            End If
                            If dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString = "PU" Then
                                If subputitle = False Then TitlePur(e.Graphics, e, dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString) : subputitle = True
                            ElseIf dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString = "SR" Then
                                If subsrtitle = False Then TitlePur(e.Graphics, e, dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString) : subsrtitle = True
                            End If
                            With dtSales.Rows(PagecountSale)
                                ''Top '/************************************////
                                NoofPage += 1
                                If dtSales.Rows(PagecountSale).Item("RESULT") = "4.1" Or dtSales.Rows(PagecountSale).Item("RESULT") = "4.2" Then
                                    START_POS = START_POS + LINE_SPACE
                                    g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.3" And .Item("TYPE").ToString = "2" Then
                                    g.DrawLine(Pens.Silver, 710, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.0" And (.Item("TYPE").ToString = "2" Or .Item("TYPE").ToString = "3") And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawLine(Pens.Silver, 310, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.2" And (.Item("TYPE").ToString = "2" Or .Item("TYPE").ToString = "3") And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.5" And (.Item("TYPE").ToString = "2" Or .Item("TYPE").ToString = "3") And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS))
                                End If
                                g.DrawString(.Item("SRNO").ToString(), fontRegular, BlackBrush, c1, START_POS, LeftFormat)

                                If .Item("TYPE").ToString = "10" And .Item("COLHEAD").ToString = "G" Then
                                    If .Item("ITEMNAME").ToString.Length > 25 Then
                                        If .Item("RESULT").ToString <> "0.0" Then g.DrawString(.Item("ITEMNAME").ToString(), fontBoldsmall, BlackBrush, c3 - 20, START_POS, LeftFormat)
                                    Else
                                        If .Item("RESULT").ToString <> "0.0" Then g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c3, START_POS, LeftFormat)
                                    End If
                                Else
                                    If .Item("CALTYPE").ToString = "HA" And .Item("RESULT").ToString = "1.1" Then
                                        If .Item("RESULT").ToString <> "0.0" Then g.DrawString(.Item("ITEMNAME").ToString(), fontRegular6, BlackBrush, c3, START_POS, LeftFormat)
                                    ElseIf .Item("TRANTYPE").ToString = "SR" And .Item("RESULT").ToString = "3.3" And .Item("TYPE").ToString = "3" And .Item("COLHEAD").ToString = "V" And .Item("ITEMNAME").ToString <> "" And .Item("RATE").ToString = "" Then
                                        g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c3, START_POS, LeftFormat)
                                    ElseIf .Item("RESULT").ToString = "3.0" And (.Item("TYPE").ToString = "2" Or .Item("TYPE").ToString = "3") And (.Item("COLHEAD").ToString = "G" Or .Item("COLHEAD").ToString = "S") Then
                                        g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c3, START_POS, LeftFormat)
                                    ElseIf .Item("RESULT").ToString = "4.1" And .Item("TYPE").ToString = "4" Then
                                        g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c3, START_POS, LeftFormat)
                                    Else
                                        If .Item("RESULT").ToString <> "0.0" Then g.DrawString(.Item("ITEMNAME").ToString(), fontRegularsmall, BlackBrush, c3, START_POS, LeftFormat)
                                    End If
                                End If
                                If .Item("RESULT").ToString = "4.2" And .Item("TYPE").ToString = "4" Then
                                    g.DrawString(.Item("HSN").ToString(), fontBold, BlackBrush, c1, START_POS, LeftFormat)
                                ElseIf .Item("RESULT").ToString = "3.2" And .Item("TYPE").ToString = "2" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawString(.Item("HSN").ToString(), fontAmtInWord, BlackBrush, c1, START_POS, LeftFormat)
                                ElseIf .Item("RESULT").ToString = "3.5" And .Item("TYPE").ToString = "3" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawString(.Item("HSN").ToString(), fontBold, BlackBrush, c1, START_POS, LeftFormat)
                                Else
                                    g.DrawString(.Item("HSN").ToString(), fontRegularsmall, BlackBrush, c2, START_POS, LeftFormat)
                                End If

                                If .Item("CALTYPE").ToString = "F" Then 'Fixed Item Should Not print GrossWt change 
                                    g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontRegularsmall, BlackBrush, c4, START_POS, RightFormat)
                                    If _SILVERBILLPRINT = "Y" Then
                                        If dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString = "SR" Then
                                            g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, c5, START_POS, RightFormat)
                                        Else
                                            g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0 And _SILVERBILLPRINT <> "Y", .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, c5, START_POS, RightFormat)
                                        End If
                                    Else
                                        g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, c5, START_POS, RightFormat)
                                    End If
                                Else
                                    If .Item("RESULT").ToString = "3.0" And (.Item("TYPE").ToString = "2" Or .Item("TYPE").ToString = "3") And .Item("COLHEAD").ToString = "G" Then
                                        g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontBold, BlackBrush, c4, START_POS, RightFormat)
                                        If _SILVERBILLPRINT = "Y" Then
                                            If dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString = "SR" Then
                                                g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontBold, BlackBrush, c5, START_POS, RightFormat)
                                            Else
                                                g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0 And _SILVERBILLPRINT <> "Y", .Item("NETWT").ToString(), ""), fontBold, BlackBrush, c5, START_POS, RightFormat)
                                            End If
                                        Else
                                            g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontBold, BlackBrush, c5, START_POS, RightFormat)
                                        End If
                                    Else
                                        g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontRegularsmall, BlackBrush, c4, START_POS, RightFormat)
                                        If _SILVERBILLPRINT = "Y" Then
                                            If dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString = "SR" Then
                                                g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, c5, START_POS, RightFormat)
                                            Else
                                                g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0 And _SILVERBILLPRINT <> "Y", .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, c5, START_POS, RightFormat)
                                            End If
                                        Else
                                            g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, c5, START_POS, RightFormat)
                                        End If
                                    End If
                                End If
                                g.DrawString(IIf(Val(.Item("WASTAGE").ToString()) <> 0 And _SILVERBILLPRINT <> "Y", .Item("WASTAGE").ToString, ""), fontRegularsmall, BlackBrush, c6, START_POS, RightFormat)
                                If _SILVERBILLPRINT <> "Y" Then
                                    If .Item("COLHEAD").ToString() = "G" Or .Item("COLHEAD").ToString = "P" Or .Item("COLHEAD").ToString = "V" Then
                                        If .Item("COLHEAD").ToString = "G" And (.Item("TYPE").ToString = "2" Or .Item("TYPE").ToString = "3") Then
                                            g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, START_POS, LeftFormat)
                                        ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString <> "10" Then
                                            g.DrawString(.Item("RATE").ToString(), fontRegularsmall, BlackBrush, 550, START_POS, LeftFormat)
                                        ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString <> "3.50") Then
                                            g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, START_POS, LeftFormat)
                                        ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString = "3.50") Then
                                            g.DrawString(.Item("RATE").ToString(), fontRegularsmall, BlackBrush, 550, START_POS, LeftFormat)
                                        ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10" Then
                                            g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, START_POS, LeftFormat)
                                        ElseIf .Item("COLHEAD").ToString = "V" And .Item("TYPE").ToString = "3" Then
                                            g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, START_POS, LeftFormat)
                                        End If
                                    Else
                                        g.DrawString(.Item("RATE").ToString(), fontRegularsmall, BlackBrush, c8, START_POS, RightFormat)
                                    End If

                                    If .Item("CALTYPE").ToString = "F" Then 'Fixed Item Should Not print mC
                                        g.DrawString("", fontRegular, BlackBrush, c8, START_POS, RightFormat)
                                    Else
                                        If dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString = "PU" Or dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString = "SR" Then
                                            g.DrawString(IIf(.Item("MCGRM").ToString() <> "0.00", .Item("MCGRM").ToString(), ""), fontRegular, BlackBrush, c9, START_POS, RightFormat)
                                        Else
                                            g.DrawString(IIf(.Item("MCGRM").ToString() <> "0.00", .Item("MCGRM").ToString(), ""), fontRegular, BlackBrush, c8, START_POS, RightFormat)
                                        End If

                                    End If
                                End If
                                If .Item("COLHEAD").ToString = "G" Or .Item("COLHEAD").ToString = "P" Or .Item("COLHEAD").ToString = "T" Or .Item("COLHEAD").ToString = "V" Then
                                    If .Item("COLHEAD").ToString = "G" And (.Item("TYPE").ToString = "2" Or .Item("TYPE").ToString = "3") Then
                                        If Val(.Item("AMOUNT").ToString) <> 0 Then
                                            g.DrawString(FormatNumber(.Item("AMOUNT"), 2).ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                        Else
                                            g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                        End If

                                    ElseIf .Item("COLHEAD").ToString = "T" And .Item("TYPE").ToString = "4" Then
                                        If Val(.Item("AMOUNT").ToString) <> 0 Then
                                            g.DrawString(FormatNumber(.Item("AMOUNT"), 2).ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                        Else
                                            g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                        End If

                                    ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString <> "10" Then
                                        If Val(.Item("AMOUNT").ToString) <> 0 Then
                                            g.DrawString(FormatNumber(.Item("AMOUNT"), 2).ToString(), fontRegularsmall, BlackBrush, c10, START_POS, rAlign)
                                        Else
                                            g.DrawString(.Item("AMOUNT").ToString(), fontRegularsmall, BlackBrush, c10, START_POS, rAlign)
                                        End If

                                    ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString <> "3.50") Or (.Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10") Then
                                        If Val(.Item("AMOUNT").ToString) <> 0 Then
                                            g.DrawString(FormatNumber(.Item("AMOUNT"), 2).ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                        Else
                                            g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                        End If

                                    ElseIf .Item("COLHEAD").ToString = "V" And .Item("TYPE").ToString = "3" Then
                                        If Val(.Item("AMOUNT").ToString) <> 0 Then
                                            g.DrawString(FormatNumber(.Item("AMOUNT"), 2).ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                        Else
                                            g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                        End If

                                    ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString = "3.50") Then
                                        If Val(.Item("AMOUNT").ToString) <> 0 Then
                                            g.DrawString(FormatNumber(.Item("AMOUNT"), 2).ToString(), fontRegularsmall, BlackBrush, c10, START_POS, rAlign)
                                        Else
                                            g.DrawString(.Item("AMOUNT").ToString(), fontRegularsmall, BlackBrush, c10, START_POS, rAlign)
                                        End If

                                    ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10" Then
                                        If Val(.Item("AMOUNT").ToString) <> 0 Then
                                            g.DrawString(FormatNumber(.Item("AMOUNT"), 2).ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                        Else
                                            g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                        End If

                                    End If
                                Else
                                    If Val(.Item("AMOUNT").ToString) <> 0 Then
                                        g.DrawString(FormatNumber(.Item("AMOUNT"), 2).ToString(), fontRegularsmall, BlackBrush, c10, START_POS, rAlign)
                                    Else
                                        g.DrawString(.Item("AMOUNT").ToString(), fontRegularsmall, BlackBrush, c10, START_POS, rAlign)
                                    End If

                                End If
                                g.DrawString(" " & .Item("EMPID").ToString(), fontRegular7, BlackBrush, c11, START_POS, LeftFormat)
                                If (.Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" And Val(.Item("QTY").ToString) >= 2) _
                                    Or (.Item("TYPE").ToString = "2.1" And .Item("COLHEAD").ToString = "G" And Val(.Item("QTY").ToString) >= 2) _
                                    Or (.Item("TYPE").ToString = "3" And .Item("COLHEAD").ToString = "G" And Val(.Item("QTY").ToString) >= 2) Then
                                    g.DrawLine(Pens.Silver, 340, (START_POS), c10, (START_POS))
                                End If
                                If START_POS >= BOTTOM_POS Then
                                    PagecountSale = PagecountSale + 1
                                    If _dt_count = PagecountSale Then
                                        GoTo gotofooter
                                    Else
                                        START_POS = TSTART_POS ' TOP AGAIN STARTING POSITION
                                        g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, BOTTOM_POS + 10)
                                        e.HasMorePages = True
                                        gvprint_flag = False
                                        Exit Sub
                                    End If
                                End If
                                'New Line Start
                                If .Item("CALTYPE").ToString = "HA" And .Item("RESULT").ToString = "1.1" Then
                                    START_POS = START_POS + LINE_SPACE
                                Else
                                    START_POS = START_POS + LINE_SPACE
                                End If
                                If .Item("RESULT").ToString = "1.0" And (.Item("BILLNAME").ToString = "Y") Then '.Item("COLHEAD").ToString <> "G" And .Item("COLHEAD").ToString <> "P" And .Item("COLHEAD").ToString <> "T"
                                    g.DrawString("Fixing of Kemp, Kundan Stones needs wax, Hence at the time of melting weight loss will be there upto 10%..", fontRegularsmall, BlackBrush, c2, START_POS, LeftFormat)
                                    START_POS = START_POS + LINE_SPACE
                                End If
                                If START_POS >= BOTTOM_POS Then
                                    PagecountSale = PagecountSale + 1
                                    If _dt_count = PagecountSale Then
                                        GoTo gotofooter
                                    Else
                                        START_POS = TSTART_POS ' TOP AGAIN STARTING POSITION
                                        g.DrawString("Continue.... ", fontRegularsmall, Brushes.Black, c8, BOTTOM_POS + 10)
                                        e.HasMorePages = True
                                        gvprint_flag = False
                                        Exit For
                                    End If
                                End If
                                If (.Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" And Val(.Item("QTY").ToString) >= 2) Or
                                    (.Item("TYPE").ToString = "3" And .Item("COLHEAD").ToString = "G" And Val(.Item("QTY").ToString) >= 2) Then
                                    g.DrawLine(Pens.Silver, 340, (START_POS), c10, (START_POS))
                                End If
                                If _SILVERBILLPRINT = "Y" Then
                                Else
                                    If (.Item("RESULT").ToString = "3.10" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G") Then
                                        g.DrawLine(Pens.Silver, 710, (START_POS), c10, (START_POS))
                                    End If
                                End If

                                If .Item("RESULT").ToString = "4.2" And .Item("COLHEAD").ToString = "T" Then
                                    If Val(.Item("AMOUNT").ToString) <> 0 Then
                                        g.DrawLine(Pens.Silver, 20, START_POS, c10, START_POS)
                                    End If
                                End If
                                If .Item("RESULT").ToString = "3.0" And .Item("TYPE").ToString = "11.0" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawLine(Pens.Silver, 340, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.2" And .Item("TYPE").ToString = "2" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.5" And .Item("TYPE").ToString = "3" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.3" And .Item("TYPE").ToString = "2" Then
                                    g.DrawLine(Pens.Silver, 710, (START_POS), c10, (START_POS))
                                End If
                                If START_POS >= BOTTOM_POS Then
                                    PagecountSale = PagecountSale + 1
                                    If _dt_count = PagecountSale Then
                                        GoTo gotofooter
                                    Else
                                        START_POS = TSTART_POS ' TOP AGAIN STARTING POSITION
                                        g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, BOTTOM_POS + 10)
                                        e.HasMorePages = True
                                        gvprint_flag = False
                                        Exit Sub
                                    End If
                                End If
                            End With
                        End If
                    Next
                End If
gotofooter:
                Footer(e.Graphics, e)
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            Exit Sub
        End Try
    End Sub

    Public Function NewPage(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal START_POS As Integer) As Integer
        If START_POS > BOTTOM_POS Then
            PagecountSale = PagecountSale + 1
            START_POS = TSTART_POS ' TOP AGAIN STARTING POSITION
            g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, BOTTOM_POS + 10)
            e.HasMorePages = True
            Return 1
        End If
    End Function
#Region "SALE INVOICE"
    Public Sub PrintSALEINVOICE(ByVal pBatchno As String, ByVal Trantype As String)
        Dim TSummary As String = "N"
        If chkmeltsummary.Checked = True Then
            TSummary = "Y"
        End If
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
        ElseIf Trantype = "ORD" Or Trantype = "REP" Then

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
                If FinalAmt = 0 Then
                    FinalAmt = 1.11
                End If
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
                strsql = " EXEC " & cnAdminDb & ".. SP_BILL_BILLPRINTVIEW_A4N "
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
                strsql += vbCrLf + " ,@SUMMARY = '" & TSummary & "' "
                'strsql += vbCrLf + " ,@IGST = 0 "
                'strsql += vbCrLf + " ,@GSTPER = '" & gstPer & "' "d
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

        Dim systabletemp As String = ""
        If TSummary = "N" Then
            systabletemp = "[TEMP" & SystemName & "SALEINVOICE]"

            strsql = "UPDATE TEMPTABLEDB.." & systabletemp & " Set RATE='CASH PAID' WHERE AMOUNT<0 AND RATE='CASH RECEIVED'"
            cmd = New OleDbCommand(strsql, cn)
            cmd.ExecuteNonQuery()
        Else
            systabletemp = "[TEMP" & SystemName & "SALEINVOICESUMMARY]"
        End If

        strsql = "SELECT TRANNO FROM TEMPTABLEDB.." & systabletemp & " WHERE TRANNO IS NOT NULL"
        Tranno = GetSqlValue_Bill(strsql, "TRANNO")

        strsql = "SELECT BILLPREFIX FROM TEMPTABLEDB.." & systabletemp & " WHERE TRANNO IS NOT NULL"
        BillPrefix = GetSqlValue_Bill(strsql, "BILLPREFIX")


        strsql = "SELECT CONVERT(VARCHAR(15),BILLDATE,103) AS BILLDATE FROM TEMPTABLEDB.." & systabletemp & " WHERE TRANNO IS NOT NULL"
        prtBilldate = GetSqlValue_Bill(strsql, "BILLDATE")

        strsql = "SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE = '" & strBilldate & "' AND BATCHNO = '" & pBatchno & "' "
        NodeId = GetSqlValue_Bill(strsql, "SYSTEMID")

        strsql = "UPDATE TEMPTABLEDB.." & systabletemp & " SET TRANNO='" & Tranno & "' WHERE TRANNO IS  NULL"
        cmd = New OleDbCommand(strsql, cn)
        cmd.ExecuteNonQuery()

        If Trantype = "MI" Then
            strsql = "SELECT * FROM TEMPTABLEDB.." & systabletemp & " ORDER BY RESULT,PGNO"
        Else
            strsql = "SELECT * FROM TEMPTABLEDB.." & systabletemp & " ORDER BY PGNO "
        End If
        da = New OleDbDataAdapter(strsql, cn)
        dtSales = New DataTable
        da.Fill(dtSales)
    End Sub
#End Region
#Region "Header Print Documents"
    Public Sub CustomerInfoSmithIssRec(ByVal Billno As String, ByVal Billdate As String, ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        ' g.DrawRectangle(New Pen(Color.Silver, 1), c8 - 20, 68, 10, 10)
        ' g.DrawString("Triplicate for Supplier", fontBold, BlackBrush, c8, 65, LeftFormat)

        Dim Strimgpath As String = ""
        Dim logo As Image
        If IO.File.Exists(Application.StartupPath & "\VBJ_LOGO.jpg") Then
            Strimgpath = Application.StartupPath & "\VBJ_LOGO.jpg"
            logo = Image.FromFile(Strimgpath)
            e.Graphics.DrawImage(logo, c8 + 40, 20, 120, 70)
        End If
        Dim dtcomp As New DataTable
        strsql = " SELECT TOP 1  * FROM " & cnAdminDb & "..COSTCENTRE "
        strsql += vbCrLf + "WHERE COSTID='" & cnCostId & "'"
        da = New OleDbDataAdapter(strsql, cn)
        dtcomp = New DataTable
        da.Fill(dtcomp)
        'strsql = " SELECT * FROM " & cnAdminDb & "..COMPANY "
        'strsql += vbCrLf + "WHERE COMPANYID='" & strCompanyId & "'"
        'da = New OleDbDataAdapter(strsql, cn)
        'dtcomp = New DataTable
        'da.Fill(dtcomp)
        If dtcomp.Rows.Count > 0 Then
            Dim Statename As String = ""
            strsql = " SELECT ISNULL(STATENAME,'') STATENAME FROM " & cnAdminDb & "..STATEMAST "
            strsql += vbCrLf + " WHERE STATEID = " & dtcomp.Rows(0).Item("STATEID").ToString & " "
            Statename = GetSqlValue(cn, strsql)
            'g.DrawString(printcpy, fontBoldTitle, Brushes.Black, c4, 20, LeftFormat)
            'g.DrawString("TAX INVOICE", fontBoldTitle, Brushes.Black, c4, 40, LeftFormat)
            g.DrawString(Print_Ver_No.ToString, fontRegular6, SilverBrush, c9 + 10, 5, LeftFormat)
            'g.DrawString(StrConv(strCompanyName.ToString, VbStrConv.Uppercase), fontBoldHead, BlackBrush, c3, 20, LeftFormat)
            'g.DrawString(dtcomp.Rows(0).Item("ADDRESS1").ToString + dtcomp.Rows(0).Item("ADDRESS2").ToString, fontRegular, BlackBrush, c4 - 50, 50, LeftFormat)
            'g.DrawString(dtcomp.Rows(0).Item("ADDRESS3").ToString + " - " + dtcomp.Rows(0).Item("ADDRESS4").ToString + " - " + dtcomp.Rows(0).Item("PHONE").ToString, fontRegular, BlackBrush, c4 - 95, 70, LeftFormat)
            'g.DrawString("GSTIN :" & dtcomp.Rows(0).Item("GSTNO").ToString, fontRegular, BlackBrush, c4 - 50, 90, LeftFormat)
            g.DrawString(Print_Ver_No.ToString, fontRegular6, SilverBrush, c9 + 10, 5, LeftFormat)
            g.DrawString(StrConv(strCompanyName.ToString, VbStrConv.Uppercase), fontBoldHead16, BlackBrush, c4 - 150, 20, LeftFormat)
            g.DrawString(dtcomp.Rows(0).Item("ADDRESS1").ToString + "," + dtcomp.Rows(0).Item("ADDRESS2").ToString, fontRegular, BlackBrush, c4 - 95, 50, LeftFormat)
            g.DrawString(dtcomp.Rows(0).Item("ADDRESS3").ToString + " - " + dtcomp.Rows(0).Item("ADDRESS4").ToString + " - " + dtcomp.Rows(0).Item("PHONE").ToString, fontRegular, BlackBrush, c4 - 90, 70, LeftFormat)
            g.DrawString("GSTIN :" & dtcomp.Rows(0).Item("GSTNO").ToString, fontRegular, BlackBrush, c4 - 35, 90, LeftFormat)
            ''g.DrawString("GSTIN :" & dtcomp.Rows(0).Item("GSTNO").ToString, fontRegular, BlackBrush, c4 - 70, 90, LeftFormat)
            'g.DrawString("STATE :" & Statename.ToString, fontBold, BlackBrush, c7, 130, LeftFormat)
            'g.DrawString("Phone :" & dtcomp.Rows(0).Item("PHONE").ToString, fontBold, BlackBrush, c4 - 50, 145, LeftFormat)
            ''g.DrawString("PANNO :" & dtcomp.Rows(0).Item("PANNO").ToString, fontBold, BlackBrush, c7, 145, LeftFormat)
        End If
        If dsSmithIssueReceipt.Tables("ACHEAD").Rows.Count > 0 And dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count > 0 Then
            Dim Acname As String = dsSmithIssueReceipt.Tables("ACHEAD").Rows(0).Item("ACNAME").ToString
            Dim Address1 As String = dsSmithIssueReceipt.Tables("ACHEAD").Rows(0).Item("ADDRESS1").ToString
            Dim Address2 As String = dsSmithIssueReceipt.Tables("ACHEAD").Rows(0).Item("ADDRESS2").ToString
            Dim Tin As String = dsSmithIssueReceipt.Tables("ACHEAD").Rows(0).Item("TIN").ToString
            Dim Pan As String = dsSmithIssueReceipt.Tables("ACHEAD").Rows(0).Item("PAN").ToString
            Dim GstNo As String = "GSTIN : " & dsSmithIssueReceipt.Tables("ACHEAD").Rows(0).Item("GSTNO").ToString
            Dim Stateid As String = dsSmithIssueReceipt.Tables("ACHEAD").Rows(0).Item("STATEID").ToString
            Dim TranNo1 As String = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("ISSUENO").ToString
            Dim METALTYPE As String = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("FLAG").ToString
            Dim __TRANTYPE As String = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("TTYPE").ToString
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
            START_POS = 105
            If METALTYPE = "O" Then
                g.DrawString("ORNAMENT", New Font("Palatino Linotype", 8, FontStyle.Bold), Brushes.Black, c6, START_POS, LeftFormat)
            ElseIf METALTYPE = "M" Then
                g.DrawString("METAL", New Font("Palatino Linotype", 8, FontStyle.Bold), Brushes.Black, c6, START_POS, LeftFormat)
            ElseIf METALTYPE = "T" Then
                g.DrawString("STONE", New Font("Palatino Linotype", 8, FontStyle.Bold), Brushes.Black, c6, START_POS, LeftFormat)
            Else
                g.DrawString("OTHERS", New Font("Palatino Linotype", 8, FontStyle.Bold), Brushes.Black, c6, START_POS, LeftFormat)
            End If
            START_POS = START_POS + LINE_SPACE
            If printcpy = "ACCOUNTS COPY" Then
                g.DrawString(printcpy, New Font("Palatino Linotype", 8, FontStyle.Regular), Brushes.Black, c6, START_POS, LeftFormat)
            Else
                g.DrawString("", New Font("Palatino Linotype", 8, FontStyle.Regular), Brushes.Black, c6, START_POS, LeftFormat)
            End If
            START_POS = START_POS + LINE_SPACE
            g.DrawString("NAME AND ADDRESS :", New Font("Palatino Linotype", 8, FontStyle.Bold), BlackBrush, c1, START_POS, LeftFormat)
            'g.DrawString("BILLING & SHIPPING DETAILS :", New Font("Palatino Linotype", 8, FontStyle.Regular), BlackBrush, c1, START_POS, LeftFormat)
            If printcpy = "ACCOUNTS COPY" Then
                g.DrawString("", New Font("Palatino Linotype", 8, FontStyle.Regular), Brushes.Black, c6, START_POS)
            Else
                g.DrawString("ORIGINAL FOR RECIPIENT  ", New Font("Palatino Linotype", 8, FontStyle.Regular), Brushes.Black, c6, START_POS)
            End If
            START_POS = START_POS + LINE_SPACE
            'End If
            g.DrawString(Acname, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
            If __TRANTYPE = "IIS" Or __TRANTYPE = "IMP" Or __TRANTYPE = "IPU" Or __TRANTYPE = "IIN" Or __TRANTYPE = "IAP" Or __TRANTYPE = "IDN" Or __TRANTYPE = "IOT" Then
                g.DrawString("Issue No", fontRegular, BlackBrush, c6, START_POS, LeftFormat)
                g.DrawString("  : " & TranNo1, fontRegular, BlackBrush, c8, START_POS, LeftFormat)
                START_POS = START_POS + LINE_SPACE
                g.DrawString(Address1, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
                g.DrawString("Issue Date", fontRegular, BlackBrush, c6, START_POS, LeftFormat)
                g.DrawString("  : " & TDate1, fontRegular, BlackBrush, c8, START_POS, LeftFormat)
                START_POS = START_POS + LINE_SPACE
                g.DrawString(Address2, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
            Else
                g.DrawString("Receipt No", fontRegular, BlackBrush, c6, START_POS, LeftFormat)
                g.DrawString("  : " & TranNo1, fontRegular, BlackBrush, c8, START_POS, LeftFormat)
                START_POS = START_POS + LINE_SPACE
                g.DrawString(Address1, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
                g.DrawString("Receipt Date", fontRegular, BlackBrush, c6, START_POS, LeftFormat)
                g.DrawString("  : " & TDate1, fontRegular, BlackBrush, c8, START_POS, LeftFormat)
                START_POS = START_POS + LINE_SPACE
                g.DrawString(Address2, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
            End If

            'g.DrawString("GST", fontRegular, BlackBrush, c6, START_POS, LeftFormat)
            'g.DrawString("  : " & COMPANYGSTNO, fontRegular, BlackBrush, c8, START_POS, LeftFormat)
            If _MIMR = "NEW" Then
                'g.DrawString("RATE FIXED : " & RateFixed & " (TAX INCLUSIVE)", fontBold, Brushes.Black, 320, START_POS, LeftFormat)
                'g.DrawString("RATE FIXED : " & RateFixed & " (TAX INCLUSIVE)", fontBold, Brushes.Black, 320, BOTTOM_POS, LeftFormat)
            End If
            START_POS = START_POS + LINE_SPACE
            g.DrawString(GstNo, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
            g.DrawString(Stateid, fontRegular, BlackBrush, 200, START_POS, LeftFormat)
            'If TranType1.Length <= 20 Then
            '    If TranType1 = "ISSUE FOR JOBWORK" Then
            '        g.DrawString(TranType1, fontBoldTitle, BlackBrush, c3 + 50, START_POS - LINE_SPACE, LeftFormat)
            '        g.DrawString("CUM DELIVERY CHALLAN", fontBoldTitle, BlackBrush, c3 + 50, START_POS, LeftFormat)
            '    Else
            '        g.DrawString(TranType1, fontBoldTitle, BlackBrush, c3 + 145, START_POS, LeftFormat)
            '    End If
            'Else
            If METALTYPE = "O" Then
                g.DrawString("ORNAMENT", fontBold, BlackBrush, c4 + 10, START_POS, LeftFormat)
            ElseIf METALTYPE = "M" Then
                g.DrawString("METAL", fontBold, BlackBrush, c4 + 10, START_POS, LeftFormat)
            ElseIf METALTYPE = "T" Then
                g.DrawString("STONE", fontBold, BlackBrush, c4 + 10, START_POS, LeftFormat)
            Else
                g.DrawString("OTHERS", fontBold, BlackBrush, c4 + 10, START_POS, LeftFormat)
            End If
            'End If

            'End If
            'g.DrawString("STATE CODE", fontRegular, BlackBrush, c6, START_POS, LeftFormat)
            'g.DrawString("  : 33", fontRegular, BlackBrush, c8, START_POS, LeftFormat)
            START_POS = START_POS + LINE_SPACE
            If __TRANTYPE = "IIS" Or __TRANTYPE = "IMP" Then
                g.DrawString("ISSUE", fontBold, BlackBrush, c4 + 10, START_POS, LeftFormat)
                START_POS = START_POS + LINE_SPACE
            ElseIf __TRANTYPE = "IOT" Then
                g.DrawString("OTHER ISSUE", fontBold, BlackBrush, c4 + 10, START_POS, LeftFormat)
            ElseIf __TRANTYPE = "IDN" Then
                g.DrawString("DELIVERY NOTE", fontBold, BlackBrush, c4 + 10, START_POS, LeftFormat)
                START_POS = START_POS + LINE_SPACE
            ElseIf __TRANTYPE = "IPU" Then
                g.DrawString("PURCHASE RETURN", fontBold, BlackBrush, c4 + 10, START_POS, LeftFormat)
                START_POS = START_POS + LINE_SPACE
            ElseIf __TRANTYPE = "IIN" Or __TRANTYPE = "RIN" Then
                g.DrawString("INTERNAL TRANSFER", fontBold, BlackBrush, c4 + 10, START_POS, LeftFormat)
                START_POS = START_POS + LINE_SPACE
            ElseIf __TRANTYPE = "IAP" Or __TRANTYPE = "RAP" Then
                g.DrawString("APPROVAL ", fontBold, BlackBrush, c4 + 10, START_POS, LeftFormat)
                START_POS = START_POS + LINE_SPACE
            ElseIf __TRANTYPE = "ROT" Then
                g.DrawString("OTHER RECEIPT", fontBold, BlackBrush, c4 + 10, START_POS, LeftFormat)
                START_POS = START_POS + LINE_SPACE
            ElseIf __TRANTYPE = "RRE" Then
                g.DrawString("RECEIPT", fontBold, BlackBrush, c4 + 10, START_POS, LeftFormat)
                START_POS = START_POS + LINE_SPACE
            ElseIf __TRANTYPE = "RPU" Then
                g.DrawString("PURCHASE", fontBold, BlackBrush, c4 + 10, START_POS, LeftFormat)
                START_POS = START_POS + LINE_SPACE
            ElseIf __TRANTYPE = "RDN" Then
                g.DrawString("RECEIPT NOTE", fontBold, BlackBrush, c4 + 10, START_POS, LeftFormat)
                START_POS = START_POS + LINE_SPACE
            End If
            HLINEPOINT = 0
            HLINEPOINT = START_POS
            g.DrawLine(Pens.Silver, 20, START_POS, 775, START_POS)
        End If
        If _top_flag = True Then
            _top_flag = False
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
            START_POS = START_POS + LINE_SPACE
            g.DrawString(TranType1, fontBoldTitle, BlackBrush, c3 - 50, START_POS, LeftFormat)
            START_POS = START_POS + LINE_SPACE
            If dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("TTYPE").ToString.Contains("RR") Then
            Else
                g.DrawString("BILLING & SHIPPING DETAILS :", New Font("Palatino Linotype", 8, FontStyle.Regular), BlackBrush, c1, START_POS, LeftFormat)
                g.DrawString("ORIGINAL FOR RECIPIENT  ", New Font("Palatino Linotype", 8, FontStyle.Regular), Brushes.Black, c6, START_POS)
                START_POS = START_POS + LINE_SPACE
            End If
            g.DrawString(Acname, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
            g.DrawString("Voucher No", fontRegular, BlackBrush, c6, START_POS, LeftFormat)
            g.DrawString("  : " & TranNo1, fontRegular, BlackBrush, c7, START_POS, LeftFormat)
            START_POS = START_POS + LINE_SPACE
            g.DrawString(Address1, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
            g.DrawString("Date", fontRegular, BlackBrush, c6, START_POS, LeftFormat)
            g.DrawString("  : " & TDate1, fontRegular, BlackBrush, c7, START_POS, LeftFormat)
            START_POS = START_POS + LINE_SPACE
            g.DrawString(Address2, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
            g.DrawString("GST", fontRegular, BlackBrush, c6, START_POS, LeftFormat)
            g.DrawString("  : " & COMPANYGSTNO, fontRegular, BlackBrush, c7, START_POS, LeftFormat)
            'g.DrawString("RATE FIXED : " & RateFixed & " (TAX INCLUSIVE)", fontBold, Brushes.Black, 320, START_POS, LeftFormat)
            START_POS = START_POS + LINE_SPACE
            g.DrawString(GstNo, fontRegular, BlackBrush, c1, START_POS, LeftFormat)
            g.DrawString(Stateid, fontRegular, BlackBrush, 200, START_POS, LeftFormat)
            ' g.DrawString(TranType1, fontBoldTitle, BlackBrush, c3, START_POS, LeftFormat)
            g.DrawString("STATE CODE", fontRegular, BlackBrush, c6, START_POS, LeftFormat)
            g.DrawString("  : 33", fontRegular, BlackBrush, c7, START_POS, LeftFormat)
            START_POS = START_POS + LINE_SPACE
            g.DrawLine(Pens.Silver, 20, START_POS, 775, START_POS)
        End If
    End Sub

    Public Sub funcCustomerQrcode(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal x As Integer, ByVal y As Integer)
        If Not QrcodeCustomer Is Nothing Then
            'g.DrawImage(QrcodeCustomer, New Rectangle(365, 190, 140, 140))
            g.DrawImage(QrcodeCustomer, New Rectangle(x, y, 60, 60))
        End If
    End Sub
    Public Sub funcCustomerQrcodeGF(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal x As Integer, ByVal y As Integer)
        If Not QrcodeCustomerGf Is Nothing Then
            g.DrawImage(QrcodeCustomerGf, New Rectangle(x, y, 60, 60))
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
        Dim Cusaccode As String = ""
        Dim grate As String = ""
        Dim srate As String = ""
        Dim BBill As String = ""
        Dim CusGSTNo As String = ""
        Dim _Billtype As String = ""
        Dim Bcompid As String = ""
        Dim dtcustleft As DataTable
        Dim dtcustright As DataTable
        'Dim rect As New Rectangle(20, 20, 20, 20)
        g.DrawString(Print_Ver_No.ToString, fontRegular6, SilverBrush, c9 + 10, 5, LeftFormat)
        If printcpy = "ACCOUNTS COPY" And ac_pusrtitle = False Then
            ac_pusrtitle = True
            subputitle = False
            subsrtitle = False
        End If
        g.DrawRectangle(New Pen(Color.Silver, 1), c8 - 20, 28, 10, 10)
        g.DrawString("Original for Recipient", fontBold, BlackBrush, c8, 25, LeftFormat)
        g.DrawRectangle(New Pen(Color.Silver, 1), c8 - 20, 48, 10, 10)
        g.DrawString("Duplicate for Supplier", fontBold, BlackBrush, c8, 45, LeftFormat)

        If _SILVERBILLPRINT = "Y" Then
            g.DrawRectangle(New Pen(Color.Silver, 1), c8 - 20, 68, 10, 10)
            g.DrawString("Triplicate for Supplier", fontBold, BlackBrush, c8, 65, LeftFormat)
        End If

        If _SILVERBILLPRINT = "Y" And _SILVERBILLPRINT_HEADER = "Y" Then
            Dim Strimgpath As String = ""
            Dim logo As Image
            If IO.File.Exists(Application.StartupPath & "\VBJ_LOGO.jpg") Then
                Strimgpath = Application.StartupPath & "\VBJ_LOGO.jpg"
                logo = Image.FromFile(Strimgpath)
                ''e.Graphics.DrawImage(logo, c3, 80, 120, 70)
            End If
            Dim dtcomp As New DataTable
            'strsql = " SELECT * FROM " & cnAdminDb & "..COMPANY "
            'strsql += vbCrLf + "WHERE COMPANYID='" & strCompanyId & "'"
            'da = New OleDbDataAdapter(strsql, cn)
            'dtcomp = New DataTable
            'da.Fill(dtcomp)
            strsql = " SELECT TOP 1  * FROM " & cnAdminDb & "..COSTCENTRE "
            strsql += vbCrLf + "WHERE COSTID='" & cnCostId & "'"
            da = New OleDbDataAdapter(strsql, cn)
            dtcomp = New DataTable
            da.Fill(dtcomp)
            If dtcomp.Rows.Count > 0 Then
                Dim Statename As String = ""
                strsql = " SELECT ISNULL(STATENAME,'') STATENAME FROM " & cnAdminDb & "..STATEMAST "
                strsql += vbCrLf + " WHERE STATEID = " & dtcomp.Rows(0).Item("STATEID").ToString & " "
                Statename = GetSqlValue(cn, strsql)
                g.DrawString(printcpy, fontBoldTitle, Brushes.Black, c4, 20, LeftFormat)
                g.DrawString("TAX INVOICE", fontBoldTitle, Brushes.Black, c4, 40, LeftFormat)
                g.DrawString(StrConv(strCompanyName.ToString, VbStrConv.ProperCase), fontBoldHead, BlackBrush, c4 - 60, 80, LeftFormat)
                g.DrawString(dtcomp.Rows(0).Item("ADDRESS1").ToString, fontBold, BlackBrush, c4 - 50, 115, LeftFormat)
                g.DrawString("GSTIN :" & dtcomp.Rows(0).Item("GSTNO").ToString, fontBold, BlackBrush, c7, 115, LeftFormat)
                g.DrawString(dtcomp.Rows(0).Item("ADDRESS2").ToString, fontBold, BlackBrush, c4 - 50, 130, LeftFormat)
                g.DrawString("STATE :" & Statename.ToString, fontBold, BlackBrush, c7, 130, LeftFormat)
                g.DrawString("PHONE :" & dtcomp.Rows(0).Item("PHONE").ToString, fontBold, BlackBrush, c4 - 50, 145, LeftFormat)
                'g.DrawString("PANNO :" & dtcomp.Rows(0).Item("PANNO").ToString, fontBold, BlackBrush, c7, 145, LeftFormat)
            End If
        End If

        strsql = " SELECT * FROM " & cnAdminDb & "..PERSONALINFO "
        strsql += vbCrLf + "WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = '" & pBatchno & "')"
        da = New OleDbDataAdapter(strsql, cn)
        dtCustInfo = New DataTable
        da.Fill(dtCustInfo)
        'If dtCustInfo.Rows.Count > 0 Then
        Dim c7I As Integer = c8 - 20
        Dim c8D As Integer = c9 - 22
        Dim c8D2 As Integer = c8D + 7

        Bno = Billno
        Bdate = Billdate
        BBill = BillPrefix
        If dtCustInfo.Rows.Count > 0 Then
            Bcompid = dtCustInfo.Rows(0).Item("COMPANYID").ToString
            Cusaccode = dtCustInfo.Rows(0).Item("ACCODE").ToString
        Else
            Bcompid = strCompanyId
            Cusaccode = ""
        End If

        If Trantype = "ORD" Then
            _Billtype = "ORDER  BILL"
        ElseIf Trantype = "REP" Then
            _Billtype = "REPAIR  BILL"
        Else
            If dtSales.Select("TRANTYPE='DU'").Length > 0 Then
                _Billtype = "CREDIT BILL"
            ElseIf dtSales.Select("TRANTYPE='AA'").Length > 0 Then
                _Billtype = "BILL AGAINST ADVANCE"
            ElseIf dtSales.Select("TRANTYPE='DU'").Length > 0 And dtSales.Select("TRANTYPE='PU'").Length > 0 Then
                _Billtype = "CREDIT BILL"
            ElseIf dtSales.Select("TRANTYPE='PU'").Length > 0 And dtSales.Select("TRANTYPE='SA'").Length > 0 Then
                _Billtype = "CASH BILL"
            ElseIf dtSales.Select("TRANTYPE='PU'").Length > 0 Then
                _Billtype = "PURCHASE BILL"
            ElseIf dtSales.Select("TRANTYPE='CA'").Length > 0 Then
                _Billtype = "CASH BILL"
                'ElseIf dtSales.Select("TRANTYPE='SR'").Length > 0 Then
                '    _Billtype = "SalesReturn "
            Else
                _Billtype = "CASH BILL"
            End If
        End If

        If dtCustInfo.Rows.Count > 0 Then
            Cusname = Trim(dtCustInfo.Rows(0).Item("TITLE").ToString) & " " & Trim(dtCustInfo.Rows(0).Item("PNAME").ToString) & Trim(dtCustInfo.Rows(0).Item("INITIAL").ToString)
            gv_Cusname = Trim(dtCustInfo.Rows(0).Item("PNAME").ToString) & Trim(dtCustInfo.Rows(0).Item("INITIAL").ToString)
            CusAddress1 = Trim(dtCustInfo.Rows(0).Item("DoorNo") & "").ToString & "" & Trim(dtCustInfo.Rows(0).Item("address1") & "").ToString
            CusArea = Trim(dtCustInfo.Rows(0).Item("area") & "").ToString & " " & Trim(dtCustInfo.Rows(0).Item("City") & "").ToString & " " & Trim(dtCustInfo.Rows(0).Item("PinCode") & "").ToString
        Else
            strsql = " SELECT TOP 1  ACNAME FROM " & cnAdminDb & "..ACHEAD  WHERE ACCODE IN (SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = '" & pBatchno & "')"
            Cusname = GetSqlValue(cn, strsql)
        End If

        dtcustleft = New DataTable
        strsql = vbCrLf + "SELECT '' LEFT1,'' RIGHT1 "
        cmd = New OleDbCommand(strsql, cn)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtcustleft)

        dtcustleft.Rows.Add("Name and Address :", "")
        dtcustleft.Rows.Add(Cusname, "")
        dtcustleft.Rows.Add(CusAddress1, "")
        If dtCustInfo.Rows.Count > 0 Then
            dtcustleft.Rows.Add(Trim(dtCustInfo.Rows(0).Item("address2") & "").ToString, "")
            dtcustleft.Rows.Add(Trim(dtCustInfo.Rows(0).Item("address3") & "").ToString, "")
        End If
        dtcustleft.Rows.Add(CusArea, "")
        If dtCustInfo.Rows.Count > 0 Then
            If Trim(dtCustInfo.Rows(0).Item("phoneres") & "").ToString <> "" Then
                dtcustleft.Rows.Add(Trim(IIf(dtCustInfo.Rows(0).Item("phoneres").ToString <> "", "PH: " & dtCustInfo.Rows(0).Item("phoneres").ToString.ToString, "")) & " " & Trim(IIf(dtCustInfo.Rows(0).Item("mobile").ToString <> "", "CELL :" & dtCustInfo.Rows(0).Item("mobile").ToString.ToString, "")) & " " & Trim(IIf(dtCustInfo.Rows(0).Item("PAN").ToString <> "", "PAN :" & dtCustInfo.Rows(0).Item("PAN").ToString, "")).ToString, "")
            Else
                dtcustleft.Rows.Add(Trim(IIf(dtCustInfo.Rows(0).Item("mobile").ToString <> "", "CELL :" & dtCustInfo.Rows(0).Item("mobile").ToString.ToString, "")) & " " & Trim(IIf(dtCustInfo.Rows(0).Item("PAN").ToString.Trim <> "", "PAN :" & dtCustInfo.Rows(0).Item("PAN").ToString, "")).ToString, "")
            End If
            If Trim(dtCustInfo.Rows(0).Item("GSTNO") & "").ToString <> "" Then
                CusGSTNo = dtCustInfo.Rows(0).Item("GSTNO").ToString
                dtcustleft.Rows.Add("GSTIN : " & CusGSTNo, "")
                If dtInvTran.Rows.Count > 0 Then
                    Dim cusIRNNo As String = dtInvTran.Rows(0).Item("IRN").ToString
                    dtcustleft.Rows.Add("IRN : " & cusIRNNo, "")
                End If
            Else
                If dtInvTran.Rows.Count > 0 Then
                    Dim cusIRNNo As String = dtInvTran.Rows(0).Item("IRN").ToString
                    dtcustleft.Rows.Add("IRN : " & cusIRNNo, "")
                End If
            End If
        End If
        strsql = " SELECT * FROM " & cnAdminDb & "..ITEMTAG "
        strsql += vbCrLf + "WHERE TAGNO IN (SELECT TOP 1 TAGNO FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = '" & pBatchno & "' AND ISNULL(TAGNO,'') <> '' AND TRANTYPE ='RD')"
        da = New OleDbDataAdapter(strsql, cn)
        dtCustInforepair = New DataTable
        da.Fill(dtCustInforepair)
        If dtCustInforepair.Rows.Count > 0 Then
            Dim Rep_del_No As String = "REPAIR DELIVERY : " + Trim(dtCustInforepair.Rows(0).Item("ORDREPNO") & "").ToString
            dtcustleft.Rows.Add("", Rep_del_No)
        End If
        If Trantype = "ORD" Then
            dtcustleft.Rows.Add("", "ORDER JEWELS")
        ElseIf Trantype = "REP" Then
            dtcustleft.Rows.Add("", "REPAIR JEWELS")
        Else

            If dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("PU") Then
                If _SILVERBILLPRINT = "Y" Then
                    dtcustleft.Rows.Add("", "")
                Else
                    dtcustleft.Rows.Add("", "OLD JEWELS")
                End If

            ElseIf dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("SR") Then
                dtcustleft.Rows.Add("", "Sales Return")
            ElseIf _SILVERBILLPRINT = "Y" Then
                dtcustleft.Rows.Add("", "SILVER BILL")
            Else
                dtcustleft.Rows.Add("", "GOLD JEWELS")
            End If

        End If

        dtcustleft.Rows.Add("", _Billtype)
        Select Case Trantype
            Case "POS"
                If BBill = "" Then
                    dtcustleft.Rows.Add("", "Invoice No  : " & Bcompid & "/" & Bno & "")
                Else
                    dtcustleft.Rows.Add("", "Invoice No  : " & Bcompid & "/" & BBill & "/" & Bno & "")
                End If

            Case "ORD"
                dtcustleft.Rows.Add("", "Order No    : " & Bcompid & "/" & BBill & "/" & Bno & "")
                dtcustleft.Rows.Add("", "Due         : " & DueDate & "")
            Case "REP"
                dtcustleft.Rows.Add("", "Repair No   : " & Bcompid & "/" & BBill & "/" & Bno & "")
        End Select
        dtcustleft.Rows.Add("", "Date           : " & Bdate & "")
        If Cusaccode <> "" Then
            dtcustleft.Rows.Add("", "Accode        : " & Cusaccode & "")
        End If
        Dim EXPORTSALES As String = GetSqlValue(cn, " SELECT EINVOICETYPE FROM " & cnAdminDb & ".. CUSTOMERINFO WHERE BATCHNO='" & pBatchno & "' AND  ISNULL(EINVOICETYPE,'') <> '' ")
        If EXPORTSALES <> "" Then
            If EXPORTSALES.Contains("SEZ") Then
                dtcustleft.Rows.Add("", "SEZ")
            Else
                dtcustleft.Rows.Add("", "EXPORT SALES")
            End If

        Else
            EXPORTSALES = GetSqlValue(cn, " SELECT REMARK1 FROM " & cnAdminDb & ".. CUSTOMERINFO WHERE BATCHNO='" & pBatchno & "' AND  ISNULL(REMARK1,'') like '%NRI SALES%'")
            If EXPORTSALES <> "" Then
                dtcustleft.Rows.Add("", "NRI SALES")
            End If
        End If
        If _SILVERBILLPRINT = "Y" Then
            strsql = vbCrLf + " SELECT "
            strsql += vbCrLf + " TOP 1 SRATE"
            strsql += vbCrLf + " FROM " & cnAdminDb & "..RATEMAST WHERE RDATE <= '" & strBilldate & "' AND PURITY='91.6' AND METALID IN ('G')"
            strsql += vbCrLf + " ORDER BY RATEGROUP DESC"
            Dim dtgrate As New DataTable
            dtgrate = New DataTable
            cmd = New OleDbCommand(strsql, cn)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtgrate)
            If dtgrate.Rows.Count > 0 Then
                grate = dtgrate.Rows(0).Item("SRATE").ToString
            End If
            strsql = vbCrLf + " SELECT "
            strsql += vbCrLf + " TOP 1 SRATE"
            strsql += vbCrLf + " FROM " & cnAdminDb & "..RATEMAST WHERE RDATE <= '" & strBilldate & "' AND PURITY='91.6' AND METALID IN ('S')"
            strsql += vbCrLf + " ORDER BY RATEGROUP DESC"
            Dim dtsrate As New DataTable
            dtsrate = New DataTable
            cmd = New OleDbCommand(strsql, cn)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtsrate)
            If dtsrate.Rows.Count > 0 Then
                srate = dtsrate.Rows(0).Item("SRATE").ToString
            End If

            dtcustleft.Rows.Add("", "Gold Rate    : " & grate & "")
            dtcustleft.Rows.Add("", "Silver Rate  : " & srate & "")
        End If

        If dtInvTran.Rows.Count > 0 Then
            ''Dim barcodeDesign As New BarcodeLib.Barcode.QRCode
            ''Dim LocalTemp As String = System.Environment.GetEnvironmentVariable("temp")
            ''Dim SystemName As String
            ''SystemName = Environment.MachineName
            ''BarcodeTempFileDelete(LocalTemp, SystemName, "")
            ''barcodeDesign.Rotate = BarcodeLib.Barcode.RotateOrientation.BottomFacingDown
            ''barcodeDesign.Data = dtInvTran.Rows(0)("QRDATA")
            ''barcodeDesign.drawBarcode(LocalTemp & "\barcode" & SystemName & ".png")
            ''Dim imgstream As New Bitmap(LocalTemp & "\barcode" & SystemName & ".png")
            ''e.Graphics.DrawImage(imgstream, 510, START_POS, 60, 60)
            ''imgstream.Dispose()
            ''BarcodeTempFileDelete(LocalTemp, SystemName, "")
            Dim gen As New QRCodeGenerator
            Dim qr_data = gen.CreateQrCode(dtInvTran.Rows(0)("QRDATA"), QRCodeGenerator.ECCLevel.Q)
            Dim code As New QRCode(qr_data)
            QrcodeCustomer = code.GetGraphic(20)
            g.DrawImage(QrcodeCustomer, New Rectangle(470, START_POS + LINE_SPACE, 100, 100))


        Else
            funcCustomerQrcode(g, e, 510, START_POS)
        End If

        Dim boldflag As Boolean = False
        Dim leftcnt As Integer = 0
        For I As Integer = 0 To dtcustleft.Rows.Count - 1
            If dtcustleft.Rows(I).Item("LEFT1").ToString.Trim <> "" Then
                If dtcustleft.Rows(I).Item("LEFT1").ToString.Contains("Name") Then
                    g.DrawString(dtcustleft.Rows(I).Item("LEFT1").ToString, fontBold, Brushes.Black, c1, START_POS)
                    If _SILVERBILLPRINT <> "Y" Then
                        g.DrawString(printcpy, fontBoldTitle, Brushes.Black, c4, START_POS)
                    ElseIf _SILVERBILLPRINT = "Y" And _SILVERBILLPRINT_HEADER = "N" Then
                        g.DrawString(printcpy, fontBoldTitle, Brushes.Black, c4, START_POS)
                    End If
                    boldflag = True
                    leftcnt += 1
                Else
                    If boldflag = True Then
                        g.DrawString(dtcustleft.Rows(I).Item("LEFT1").ToString, fontBold, Brushes.Black, c1, START_POS)
                        boldflag = False
                    Else
                        g.DrawString(dtcustleft.Rows(I).Item("LEFT1").ToString, fontRegular, Brushes.Black, c1, START_POS)
                    End If

                    leftcnt += 1
                End If
                START_POS = START_POS + LINE_SPACE
            End If
        Next
        If leftcnt <= 3 Then
            START_POS = START_POS + LINE_SPACE
            START_POS = START_POS + LINE_SPACE
        End If
        Dim tempstartpos As Integer = 0
        tempstartpos = START_POS
        START_POS = TSTART_POS
        For I As Integer = 0 To dtcustleft.Rows.Count - 1
            If dtcustleft.Rows(I).Item("RIGHT1").ToString.Trim <> "" Then
                If dtcustleft.Rows(I).Item("RIGHT1").ToString.Contains("JEWELS") Or dtcustleft.Rows(I).Item("RIGHT1").ToString.Contains("SILVER BILL") Then
                    If _SILVERBILLPRINT = "Y" And _SILVERBILLPRINT_HEADER = "Y" Then
                        g.DrawString(dtcustleft.Rows(I).Item("RIGHT1").ToString, fontBold, Brushes.Black, c7, START_POS)
                    Else
                        g.DrawString(dtcustleft.Rows(I).Item("RIGHT1").ToString, fontBold, Brushes.Black, c8 - 20, START_POS)
                    End If
                Else
                    If dtcustleft.Rows(I).Item("RIGHT1").ToString.Contains("BILL") Then
                        If Trantype = "ORD" Or Trantype = "REP" Then
                            Select Case Trantype
                                Case "ORD"
                                    g.DrawString("ORDER BOOKING", fontBoldTitle, Brushes.Black, c4, START_POS)
                                Case "REP"
                                    g.DrawString("REPAIR BOOKING", fontBoldTitle, Brushes.Black, c4, START_POS)
                            End Select
                        Else
                            If dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("SA") _
                                Or dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("RD") _
                                Or dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("MI") _
                                  Or dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("PU") _
                                Or dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("SR") _
                                Or dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("ORD") _
                                Or dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("REP") _
                                Then
                                TrantypeMI = dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString
                                'Select Case Trantype
                                '    Case "POS"
                                If TrantypeMI = "RD" Then
                                    g.DrawString("REPAIR DELIVERY", fontBoldTitle, Brushes.Black, c4, START_POS + LINE_SPACE)
                                ElseIf TrantypeMI = "MI" Then
                                    g.DrawString("MISC ISSUE", fontBoldTitle, Brushes.Black, c4, START_POS + LINE_SPACE)
                                ElseIf TrantypeMI = "PU" Then
                                    Trantypefoot = "PU"
                                    g.DrawString("   PURCHASE", fontBoldTitle, Brushes.Black, c4, START_POS + LINE_SPACE)
                                ElseIf TrantypeMI = "SR" Then
                                    Trantypefoot = "SR"
                                    g.DrawString(" TAX INVOICE", fontBoldTitle, Brushes.Black, c4, START_POS + 10) 'SALES
                                    g.DrawString(" SALES RETURN", fontBoldTitle, Brushes.Black, c4, START_POS + LINE_SPACE + 20)
                                ElseIf TrantypeMI = "AI" Then
                                    Trantypefoot = "AI"
                                    g.DrawString("  APPROVAL ISSUE", fontBoldTitle, Brushes.Black, c4, START_POS)
                                ElseIf TrantypeMI = "AR" Then
                                    Trantypefoot = "AR"
                                    g.DrawString("  APPROVAL RECEIPT", fontBoldTitle, Brushes.Black, c4, START_POS)
                                Else
                                    If _SILVERBILLPRINT = "Y" And _SILVERBILLPRINT_HEADER = "Y" Then
                                        g.DrawString("     SALES", fontBoldTitle, Brushes.Black, c4 + LINE_SPACE, START_POS + LINE_SPACE + 20)
                                    Else
                                        g.DrawString("  TAX INVOICE", fontBoldTitle, Brushes.Black, c4, START_POS + 10) 'SALES
                                        g.DrawString("     SALES", fontBoldTitle, Brushes.Black, c4 + LINE_SPACE, START_POS + LINE_SPACE + 20)
                                    End If
                                End If
                                '    Case "ORD"
                                '        g.DrawString("ORDER BOOKING", fontBoldTitle, Brushes.Black, c4, START_POS)
                                '    Case "REP"
                                '        g.DrawString("REPAIR BOOKING", fontBoldTitle, Brushes.Black, c4, START_POS)
                                'End Select
                            End If
                        End If
                    End If
                    If _SILVERBILLPRINT = "Y" And _SILVERBILLPRINT_HEADER = "Y" Then
                        g.DrawString(dtcustleft.Rows(I).Item("RIGHT1").ToString, fontRegular, Brushes.Black, c7, START_POS)
                    Else
                        g.DrawString(dtcustleft.Rows(I).Item("RIGHT1").ToString, fontRegular, Brushes.Black, c8 - 20, START_POS)
                    End If
                End If
                START_POS = START_POS + LINE_SPACE
            End If
        Next
        'START_POS = tempstartpos
        START_POS = IIf(START_POS > tempstartpos, START_POS, tempstartpos)
        START_POS = START_POS + LINE_SPACE
        'g.DrawString("GOLD JEWELS", fontBold, Brushes.Black, c7I, START_POS)
        'g.DrawString("Name And Address : ", fontBold, Brushes.Black, c1, START_POS)
        'START_POS = START_POS + LINE_SPACE

        'g.DrawString(Cusname, fontRegular, Brushes.Black, c1, START_POS)

        'g.DrawString(_Billtype, fontRegular, Brushes.Black, c7I, START_POS)
        'START_POS = START_POS + LINE_SPACE


        'g.DrawString(CusAddress1, fontRegular, Brushes.Black, c1, START_POS)


        'Select Case Trantype
        '    Case "POS"
        '        g.DrawString("Invoice No", fontRegular, Brushes.Black, c7I, START_POS)
        '        g.DrawString(" : ", fontRegular, Brushes.Black, c8D, START_POS)
        '        g.DrawString(BBill & "/" & Bno, fontRegular, Brushes.Black, c8D2 + 8, START_POS)
        '    Case "ORD"
        '        g.DrawString("Order No", fontRegular, Brushes.Black, c7I, START_POS)
        '        g.DrawString(" : ", fontRegular, Brushes.Black, c8D, START_POS)
        '        g.DrawString(BBill & "/" & Bno, fontRegular, Brushes.Black, c8D2 + 8, START_POS)
        '    Case "REP"
        '        g.DrawString("Repair No", fontRegular, Brushes.Black, c7I, START_POS)
        '        g.DrawString(" : ", fontRegular, Brushes.Black, c8D, START_POS)
        '        g.DrawString(BBill & "/" & Bno, fontRegular, Brushes.Black, c8D2 + 8, START_POS)
        'End Select
        'Select Case Trantype
        '    Case "ORD"
        '        START_POS = START_POS + LINE_SPACE
        '        g.DrawString("Due", fontRegular, Brushes.Black, c7I, START_POS)
        '        g.DrawString(" : ", fontRegular, Brushes.Black, c8D, START_POS)
        '        g.DrawString(DueDate, fontRegular, Brushes.Black, c8D2, START_POS)
        '        START_POS = START_POS - LINE_SPACE
        'End Select
        'START_POS = START_POS + LINE_SPACE
        'CusAddress1 = Trim(dtCustInfo.Rows(0).Item("address2") & "").ToString
        'g.DrawString(CusAddress1, fontRegular, Brushes.Black, c1, START_POS)


        'START_POS = START_POS + LINE_SPACE
        'g.DrawString("Date", fontRegular, Brushes.Black, c7I, START_POS)
        'g.DrawString(" :  ", fontRegular, Brushes.Black, c8D, START_POS)
        'g.DrawString(Bdate, fontRegular, Brushes.Black, c8D2, START_POS)
        'START_POS = START_POS + LINE_SPACE

        'g.DrawString(CusArea, fontRegular, Brushes.Black, c1, START_POS)
        'START_POS = START_POS + LINE_SPACE
        'If Trim(dtCustInfo.Rows(0).Item("phoneres") & "").ToString <> "" Then
        '    CusPhone_Mobi = Trim(dtCustInfo.Rows(0).Item("phoneres") & "").ToString & "" & Trim(dtCustInfo.Rows(0).Item("mobile") & "").ToString & "" & Trim(IIf(dtCustInfo.Rows(0).Item("PAN").ToString <> "", "PAN :" & dtCustInfo.Rows(0).Item("PAN").ToString, "")).ToString
        '    g.DrawString(CusPhone_Mobi, fontRegular, Brushes.Black, c1, START_POS)
        '    START_POS = START_POS + LINE_SPACE
        'Else
        '    CusPhone = Trim(dtCustInfo.Rows(0).Item("mobile").ToString) & "" & Trim(IIf(dtCustInfo.Rows(0).Item("PAN").ToString <> "", "PAN :" & dtCustInfo.Rows(0).Item("PAN").ToString, "")).ToString
        '    g.DrawString(CusPhone, fontRegular, Brushes.Black, c1, START_POS)
        '    START_POS = START_POS + LINE_SPACE
        'End If
        'If Trim(dtCustInfo.Rows(0).Item("GSTNO") & "").ToString <> "" Then
        '    CusGSTNo = dtCustInfo.Rows(0).Item("GSTNO").ToString
        '    g.DrawString("GSTIN : " & CusGSTNo, fontRegular, Brushes.Black, c1, START_POS)
        '    If dtInvTran.Rows.Count > 0 Then
        '        Dim cusIRNNo As String = dtInvTran.Rows(0).Item("IRN").ToString
        '        g.DrawString("IRN : " & cusIRNNo, fontRegular, Brushes.Black, h1 - 95, START_POS)
        '    End If
        '    START_POS = START_POS + LINE_SPACE '10
        'End If
        If dtCustInfo.Rows.Count > 0 Then
            If dtCustInfo.Rows(0).Item("STATEID").ToString <> "" Then
                Dim Qry As String = ""
                Qry = " SELECT ISNULL(STATENAME,'') STATENAME FROM " & cnAdminDb & "..STATEMAST "
                Qry += vbCrLf + " WHERE STATEID = " & dtCustInfo.Rows(0).Item("STATEID").ToString & " "
                placeofSupply = GetSqlValue(cn, Qry)
            End If
        End If
        'End If
        If Trantype = "ORD" Or Trantype = "REP" Then
            DrawLine(g, START_POS, 0)
        Else
            If dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("SA") _
                Or dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("RD") _
                Or dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("MI") _
                Or dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("ORD") _
                Or dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("REP") _
                Then
                DrawLine(g, START_POS, 0)
            End If
        End If
    End Sub

    Public Sub CustomerInfoApproval(ByVal Billno As String, ByVal Billdate As String, ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
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
        Dim Cusaccode As String = ""
        Dim grate As String = ""
        Dim srate As String = ""
        Dim BBill As String = ""
        Dim CusGSTNo As String = ""
        Dim _Billtype As String = ""
        Dim Bcompid As String = ""
        Dim dtcustleft As DataTable
        Dim dtcustright As DataTable
        'Dim rect As New Rectangle(20, 20, 20, 20)
        g.DrawString(Print_Ver_No.ToString.ToString, fontRegular6, SilverBrush, c9 + 10, 5, LeftFormat)
        g.DrawRectangle(New Pen(Color.Silver, 1), c8 - 20, 28, 10, 10)
        g.DrawString("Original for Recipient", fontBold, BlackBrush, c8, 25, LeftFormat)
        g.DrawRectangle(New Pen(Color.Silver, 1), c8 - 20, 48, 10, 10)
        g.DrawString("Duplicate for Supplier", fontBold, BlackBrush, c8, 45, LeftFormat)


        If _SILVERBILLPRINT = "Y" Then
            g.DrawRectangle(New Pen(Color.Silver, 1), c8 - 20, 68, 10, 10)
            g.DrawString("Triplicate for Supplier", fontBold, BlackBrush, c8, 65, LeftFormat)
        End If
        Dim Strimgpath As String = ""
        Dim logo As Image
        If IO.File.Exists(Application.StartupPath & "\VBJ_LOGO.jpg") Then
            Strimgpath = Application.StartupPath & "\VBJ_LOGO.jpg"
            logo = Image.FromFile(Strimgpath)
            ''e.Graphics.DrawImage(logo, c3, 80, 120, 70)
        End If
        Dim dtcomp As New DataTable
        strsql = " SELECT TOP 1  * FROM " & cnAdminDb & "..COSTCENTRE "
        strsql += vbCrLf + "WHERE COSTID='" & cnCostId & "'"
        da = New OleDbDataAdapter(strsql, cn)
        dtcomp = New DataTable
        da.Fill(dtcomp)


        'strsql = " SELECT * FROM " & cnAdminDb & "..COMPANY "
        'strsql += vbCrLf + "WHERE COMPANYID='" & strCompanyId & "'"
        'da = New OleDbDataAdapter(strsql, cn)
        'dtcomp = New DataTable
        'da.Fill(dtcomp)
        If dtcomp.Rows.Count > 0 Then
            Dim Statename As String = ""
            strsql = " SELECT ISNULL(STATENAME,'') STATENAME FROM " & cnAdminDb & "..STATEMAST "
            strsql += vbCrLf + " WHERE STATEID = " & dtcomp.Rows(0).Item("STATEID").ToString & " "
            Statename = GetSqlValue(cn, strsql)
            If _SILVERBILLPRINT = "Y" And _SILVERBILLPRINT_HEADER = "Y" Then
                g.DrawString(printcpy, fontBoldTitle, Brushes.Black, c4, 20, LeftFormat)
                g.DrawString("TAX INVOICE", fontBoldTitle, Brushes.Black, c4, 40, LeftFormat)
            End If
            g.DrawString(StrConv(strCompanyName.ToString, VbStrConv.ProperCase), fontBoldHead, BlackBrush, c4 - 50, 80, LeftFormat)
            g.DrawString(dtcomp.Rows(0).Item("ADDRESS1").ToString, fontBold, BlackBrush, c4 - 50, 115, LeftFormat)
            g.DrawString("GSTIN :" & dtcomp.Rows(0).Item("GSTNO").ToString, fontBold, BlackBrush, c7, 115, LeftFormat)
            g.DrawString(dtcomp.Rows(0).Item("ADDRESS2").ToString, fontBold, BlackBrush, c4 - 50, 130, LeftFormat)
            g.DrawString("STATE :" & Statename.ToString, fontBold, BlackBrush, c7, 130, LeftFormat)
            g.DrawString("Phone :" & dtcomp.Rows(0).Item("PHONE").ToString, fontBold, BlackBrush, c4 - 50, 145, LeftFormat)
            ''g.DrawString("PANNO :" & dtcomp.Rows(0).Item("PANNO").ToString, fontBold, BlackBrush, c7, 145, LeftFormat)
        End If

        strsql = " SELECT * FROM " & cnAdminDb & "..PERSONALINFO "
        strsql += vbCrLf + "WHERE SNO IN (SELECT PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = '" & pBatchno & "')"
        da = New OleDbDataAdapter(strsql, cn)
        dtCustInfo = New DataTable
        da.Fill(dtCustInfo)
        If dtCustInfo.Rows.Count > 0 Then
            Dim c7I As Integer = c8 - 20
            Dim c8D As Integer = c9 - 22
            Dim c8D2 As Integer = c8D + 7

            Bno = Billno
            Bdate = Billdate
            BBill = BillPrefix
            Bcompid = dtCustInfo.Rows(0).Item("COMPANYID").ToString
            Cusaccode = dtCustInfo.Rows(0).Item("ACCODE").ToString
            If dtSales.Select("TRANTYPE='AI'").Length > 0 Then
                _Billtype = "APPROVAL ISSUE"
            ElseIf dtSales.Select("TRANTYPE='AR'").Length > 0 Then
                _Billtype = "APPROVAL RECEIPT"
            Else
                _Billtype = "APPROVAL ISSUE"
            End If
            Cusname = Trim(dtCustInfo.Rows(0).Item("TITLE").ToString) & " " & Trim(dtCustInfo.Rows(0).Item("PNAME").ToString) & Trim(dtCustInfo.Rows(0).Item("INITIAL").ToString)
            CusAddress1 = Trim(dtCustInfo.Rows(0).Item("DoorNo") & "").ToString & "" & Trim(dtCustInfo.Rows(0).Item("address1") & "").ToString
            CusArea = Trim(dtCustInfo.Rows(0).Item("area") & "").ToString & " " & Trim(dtCustInfo.Rows(0).Item("City") & "").ToString & " " & Trim(dtCustInfo.Rows(0).Item("PinCode") & "").ToString
            dtcustleft = New DataTable
            strsql = vbCrLf + "SELECT '' LEFT1,'' RIGHT1 "
            cmd = New OleDbCommand(strsql, cn)
            da = New OleDbDataAdapter(cmd)
            da.Fill(dtcustleft)

            dtcustleft.Rows.Add("Name and Address :", "")
            dtcustleft.Rows.Add(Cusname, "")
            dtcustleft.Rows.Add(CusAddress1, "")
            dtcustleft.Rows.Add(Trim(dtCustInfo.Rows(0).Item("address2") & "").ToString, "")
            dtcustleft.Rows.Add(Trim(dtCustInfo.Rows(0).Item("address3") & "").ToString, "")
            dtcustleft.Rows.Add(CusArea, "")
            If Trim(dtCustInfo.Rows(0).Item("phoneres") & "").ToString <> "" Then
                dtcustleft.Rows.Add(Trim(IIf(dtCustInfo.Rows(0).Item("phoneres").ToString <> "", "PH: " & dtCustInfo.Rows(0).Item("phoneres").ToString.ToString, "")) & " " & Trim(IIf(dtCustInfo.Rows(0).Item("mobile").ToString <> "", "CELL :" & dtCustInfo.Rows(0).Item("mobile").ToString.ToString, "")) & " " & Trim(IIf(dtCustInfo.Rows(0).Item("PAN").ToString <> "", "PAN :" & dtCustInfo.Rows(0).Item("PAN").ToString, "")).ToString, "")
            Else
                dtcustleft.Rows.Add(Trim(IIf(dtCustInfo.Rows(0).Item("mobile").ToString <> "", "CELL :" & dtCustInfo.Rows(0).Item("mobile").ToString.ToString, "")) & " " & Trim(IIf(dtCustInfo.Rows(0).Item("PAN").ToString.Trim <> "", "PAN :" & dtCustInfo.Rows(0).Item("PAN").ToString, "")).ToString, "")
            End If
            If Trim(dtCustInfo.Rows(0).Item("GSTNO") & "").ToString <> "" Then
                CusGSTNo = dtCustInfo.Rows(0).Item("GSTNO").ToString
                dtcustleft.Rows.Add("GSTIN : " & CusGSTNo, "")
                If dtInvTran.Rows.Count > 0 Then
                    Dim cusIRNNo As String = dtInvTran.Rows(0).Item("IRN").ToString
                    dtcustleft.Rows.Add("IRN : " & cusIRNNo, "")
                End If
            Else
                ''If _SILVERBILLPRINT = "Y" Then
                ''    dtcustleft.Rows.Add(". ")
                ''    dtcustleft.Rows.Add(". ")


                ''    dtcustleft.Rows.Add(". ")
                ''End If
            End If
            If dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("AI") Then
                dtcustleft.Rows.Add("", "APPROVAL ISSUE")
            ElseIf dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("AR") Then
                dtcustleft.Rows.Add("", "APPROVAL RECEIPT")
            Else
                dtcustleft.Rows.Add("", "APPROVAL ISSUE")
            End If

            'dtcustleft.Rows.Add("", _Billtype)

            Select Case Trantype
                Case "POS"
                    dtcustleft.Rows.Add("", "Bill No  : " & Bcompid & "/" & BBill & "/" & Bno & "")
                Case "ORD"
                    dtcustleft.Rows.Add("", "Order No    : " & Bcompid & "/" & BBill & "/" & Bno & "")
                    dtcustleft.Rows.Add("", "Due         : " & DueDate & "")
                Case "REP"
                    dtcustleft.Rows.Add("", "Repair No   : " & Bcompid & "/" & BBill & "/" & Bno & "")
            End Select


            dtcustleft.Rows.Add("", "Bill Date   : " & Bdate & "")
            If Cusaccode <> "" Then
                dtcustleft.Rows.Add("", "Accode       : " & Cusaccode & "")
            End If

            If dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("AR") Then
                Dim dtref As DataTable
                strsql = " SELECT TOP 1 REFNO,CONVERT(VARCHAR,REFDATE,103) AS REFDATE FROM " & cnStockDb & ".. RECEIPT WHERE BATCHNO = '" & pBatchno & "'"
                da = New OleDbDataAdapter(strsql, cn)
                dtref = New DataTable
                da.Fill(dtref)
                If dtref.Rows.Count > 0 Then
                    dtcustleft.Rows.Add("", "Iss No       : " & dtref.Rows(0).Item("REFNO").ToString)
                    dtcustleft.Rows.Add("", "Iss Date    : " & dtref.Rows(0).Item("REFDATE").ToString)
                End If
            End If

            ''If _SILVERBILLPRINT = "Y" Then
            ''    strsql = vbCrLf + " SELECT "
            ''    strsql += vbCrLf + " TOP 1 SRATE"
            ''    strsql += vbCrLf + " FROM " & cnAdminDb & "..RATEMAST WHERE RDATE <= '" & strBilldate & "' AND PURITY='91.6' AND METALID IN ('G')"
            ''    strsql += vbCrLf + " ORDER BY RATEGROUP DESC"
            ''    Dim dtgrate As New DataTable
            ''    dtgrate = New DataTable
            ''    cmd = New OleDbCommand(strsql, cn)
            ''    da = New OleDbDataAdapter(cmd)
            ''    da.Fill(dtgrate)
            ''    If dtgrate.Rows.Count > 0 Then
            ''        grate = dtgrate.Rows(0).Item("SRATE").ToString
            ''    End If
            ''    strsql = vbCrLf + " SELECT "
            ''    strsql += vbCrLf + " TOP 1 SRATE"
            ''    strsql += vbCrLf + " FROM " & cnAdminDb & "..RATEMAST WHERE RDATE <= '" & strBilldate & "' AND PURITY='91.6' AND METALID IN ('S')"
            ''    strsql += vbCrLf + " ORDER BY RATEGROUP DESC"
            ''    Dim dtsrate As New DataTable
            ''    dtsrate = New DataTable
            ''    cmd = New OleDbCommand(strsql, cn)
            ''    da = New OleDbDataAdapter(cmd)
            ''    da.Fill(dtsrate)
            ''    If dtsrate.Rows.Count > 0 Then
            ''        srate = dtsrate.Rows(0).Item("SRATE").ToString
            ''    End If
            ''    dtcustleft.Rows.Add("", "Gold Rate    : " & grate & "")
            ''    dtcustleft.Rows.Add("", "Silver Rate  : " & srate & "")
            ''End If

            'funcCustomerQrcode(g, e, 520, START_POS)

            Dim leftcnt As Integer = 0
            For I As Integer = 0 To dtcustleft.Rows.Count - 1
                If dtcustleft.Rows(I).Item("LEFT1").ToString.Trim <> "" Then
                    If dtcustleft.Rows(I).Item("LEFT1").ToString.Contains("Name") Then
                        g.DrawString(dtcustleft.Rows(I).Item("LEFT1").ToString, fontBold, Brushes.Black, c1, START_POS)
                        If _SILVERBILLPRINT = "Y" And _SILVERBILLPRINT_HEADER = "Y" Then
                            g.DrawString("", fontBoldTitle, Brushes.Black, c4, START_POS)
                        Else
                            g.DrawString(printcpy, fontBoldTitle, Brushes.Black, c4, START_POS)
                        End If

                        leftcnt += 1
                    Else
                        g.DrawString(dtcustleft.Rows(I).Item("LEFT1").ToString, fontRegular, Brushes.Black, c1, START_POS)
                        leftcnt += 1
                    End If
                    START_POS = START_POS + LINE_SPACE
                End If
            Next
            If leftcnt <= 3 Then
                START_POS = START_POS + LINE_SPACE
                START_POS = START_POS + LINE_SPACE
            End If
            Dim tempstartpos As Integer = 0

            tempstartpos = START_POS
            START_POS = TSTART_POS
            For I As Integer = 0 To dtcustleft.Rows.Count - 1
                If dtcustleft.Rows(I).Item("RIGHT1").ToString.Trim <> "" Then
                    If dtcustleft.Rows(I).Item("RIGHT1").ToString.Contains("APPROVAL") Then
                        g.DrawString(dtcustleft.Rows(I).Item("RIGHT1").ToString, fontBold, Brushes.Black, c8 - 20, START_POS)
                    Else
                        If dtcustleft.Rows(I).Item("RIGHT1").ToString.Contains("Bill No") Then
                            If dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("AI") _
                                Or dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("AR") _
                                Then
                                TrantypeMI = dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString
                                Select Case Trantype
                                    Case "POS"
                                        If TrantypeMI = "AI" Then
                                            Trantypefoot = "AI"
                                            g.DrawString("APPROVAL ISSUE", fontBoldTitle, Brushes.Black, c4, START_POS)
                                        ElseIf TrantypeMI = "AR" Then
                                            Trantypefoot = "AR"
                                            g.DrawString("APPROVAL RECEIPT", fontBoldTitle, Brushes.Black, c4, START_POS)
                                        Else
                                            Trantypefoot = "AI"
                                            g.DrawString("APPROVAL ISSUE", fontBoldTitle, Brushes.Black, c4, START_POS)
                                        End If
                                End Select
                            End If
                        End If
                        g.DrawString(dtcustleft.Rows(I).Item("RIGHT1").ToString, fontRegular, Brushes.Black, c8 - 20, START_POS)
                    End If

                    START_POS = START_POS + LINE_SPACE
                End If
            Next
            ' START_POS = tempstartpos
            START_POS = IIf(START_POS > tempstartpos, START_POS, tempstartpos)
            START_POS = START_POS + LINE_SPACE

            If dtInvTran.Rows.Count > 0 Then
                Dim barcodeDesign As New BarcodeLib.Barcode.QRCode
                Dim LocalTemp As String = System.Environment.GetEnvironmentVariable("temp")
                Dim SystemName As String
                SystemName = Environment.MachineName
                BarcodeTempFileDelete(LocalTemp, SystemName, "")
                barcodeDesign.Rotate = BarcodeLib.Barcode.RotateOrientation.BottomFacingDown
                barcodeDesign.Data = dtInvTran.Rows(0)("QRDATA")
                barcodeDesign.drawBarcode(LocalTemp & "\barcode" & SystemName & ".png")
                Dim imgstream As New Bitmap(LocalTemp & "\barcode" & SystemName & ".png")
                e.Graphics.DrawImage(imgstream, c6, START_POS - 60, 50, 50)
                imgstream.Dispose()
                BarcodeTempFileDelete(LocalTemp, SystemName, "")
            End If
            If dtCustInfo.Rows(0).Item("STATEID").ToString <> "" Then
                Dim Qry As String = ""
                Qry = " SELECT ISNULL(STATENAME,'') STATENAME FROM " & cnAdminDb & "..STATEMAST "
                Qry += vbCrLf + " WHERE STATEID = " & dtCustInfo.Rows(0).Item("STATEID").ToString & " "
                placeofSupply = GetSqlValue(cn, Qry)
            End If
        End If
        If dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("AI") _
                Or dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("AR") _
                Then
            DrawLine(g, START_POS, 0)
        End If

    End Sub

    Public Sub BarcodeTempFileDelete(ByVal LocalTemp As String, ByVal SystemName As String, ByVal Tranno As String)
        If System.IO.File.Exists(LocalTemp & "\barcode" & SystemName & ".png") = True Then
            System.IO.File.Delete(LocalTemp & "\barcode" & SystemName & ".png")
        End If
    End Sub

    Public Sub DrawLine(ByVal g As Graphics, ByVal Y1 As Integer, ByVal Y2 As Integer)
        'c1 - > 25.00 c7 - > 770.0F
        g.DrawLine(Pens.Silver, 20.0F, Y1, 775.0F, Y1)
        ''g.DrawLine(Pens.Silver, 20.0F, Y2, 775.0F, Y2)
    End Sub
    Public Sub Title(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)


        RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
        'c9 = Val(dt1.Compute("SUM(XPOS)", "NAME='c9'").ToString)
        'g.DrawString(dt1.Rows(2).Item("header"), fontRegular, Brushes.Black, c1, START_POS, LeftFormat)
        'g.DrawString(dt1.Rows(3).Item("header"), fontRegular, Brushes.Black, c2, START_POS, LeftFormat)
        ''   g.DrawString("HSN", fontRegular, Brushes.Black, h1, START_POS, LeftFormat)
        'g.DrawString(dt1.Rows(4).Item("header"), fontRegular, Brushes.Black, c3, START_POS, RightFormat)
        'g.DrawString(dt1.Rows(5).Item("header"), fontRegular, Brushes.Black, c4, START_POS, RightFormat)
        'g.DrawString(dt1.Rows(6).Item("header"), fontRegular, Brushes.Black, c5, START_POS, RightFormat)
        'g.DrawString(dt1.Rows(7).Item("header"), fontRegular, Brushes.Black, c6, START_POS, RightFormat) 'MC
        'g.DrawString(dt1.Rows(8).Item("header"), fontRegular, Brushes.Black, c7, START_POS, RightFormat)
        'g.DrawString(dt1.Rows(9).Item("header"), fontRegular, Brushes.Black, c8, START_POS, RightFormat)
        'g.DrawString(dt1.Rows(10).Item("header"), fontRegular, Brushes.Black, c9, START_POS, RightFormat)
        'g.DrawString(dt1.Rows(11).Item("header"), fontRegular, Brushes.Black, c9, START_POS, RightFormat)
        g.DrawString("SNo", fontRegular, Brushes.Black, c1, START_POS, LeftFormat)
        g.DrawString("DESCRIPTION", fontRegular, Brushes.Black, c3, START_POS, LeftFormat)
        g.DrawString("HSN", fontRegular, Brushes.Black, c2, START_POS, LeftFormat)
        g.DrawString("PCS", fontRegular, Brushes.Black, c4, START_POS, RightFormat)
        g.DrawString("GROSS WT", fontRegular, Brushes.Black, c5, START_POS, RightFormat)

        If _SILVERBILLPRINT = "Y" Then
            g.DrawString("", fontRegular, Brushes.Black, c6, START_POS, RightFormat) 'VA
            g.DrawString("NET WT", fontRegular, Brushes.Black, c7, START_POS, RightFormat)
            g.DrawString("", fontRegular, Brushes.Black, c8, START_POS, RightFormat)
        Else
            g.DrawString("NET WT", fontRegular, Brushes.Black, c6, START_POS, RightFormat)
            g.DrawString("", fontRegular, Brushes.Black, c7, START_POS, RightFormat) 'VA
            g.DrawString("RATE/MC", fontRegular, Brushes.Black, c8, START_POS, RightFormat)
        End If
        g.DrawString("", fontRegular, Brushes.Black, c9, START_POS, RightFormat)
        g.DrawString("AMOUNT", fontRegular, Brushes.Black, c10, START_POS, RightFormat)
        START_POS = START_POS + LINE_SPACE
        DrawLine(g, START_POS, 0)
        START_POS = START_POS + LINE_SPACE
    End Sub

    Public Sub TitleGift(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
        g.DrawString("SNo", fontRegular, Brushes.Black, c1, START_POS, LeftFormat)
        g.DrawString("DESCRIPTION", fontRegular, Brushes.Black, c3, START_POS, LeftFormat)
        g.DrawString("UNIT", fontRegular, Brushes.Black, c5, START_POS, RightFormat)
        g.DrawString("DENOMINATION", fontRegular, Brushes.Black, c7 + LINE_SPACE, START_POS, RightFormat)
        g.DrawString("AMOUNT", fontRegular, Brushes.Black, c10, START_POS, RightFormat)
        START_POS = START_POS + LINE_SPACE
        DrawLine(g, START_POS, 0)
        START_POS = START_POS + LINE_SPACE
    End Sub
    Public Sub TitleApproval(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft

        g.DrawString("SNo", fontBold, Brushes.Black, c1, START_POS, LeftFormat)
        g.DrawString("DESCRIPTION", fontBold, Brushes.Black, c3, START_POS, LeftFormat)
        g.DrawString("TAGNO", fontBold, Brushes.Black, c2 - 5, START_POS, LeftFormat)
        g.DrawString("PCS", fontBold, Brushes.Black, c4 + 10, START_POS, RightFormat) 'C4
        g.DrawString("GROSS WT", fontBold, Brushes.Black, c5 + 110, START_POS, RightFormat) 'C5
        g.DrawString("NET WT", fontBold, Brushes.Black, c6 + 110, START_POS, RightFormat) 'C6
        'g.DrawString("", fontRegular, Brushes.Black, c7, START_POS, RightFormat) 'VA
        'g.DrawString("RATE/MC", fontRegular, Brushes.Black, c9, START_POS, RightFormat)
        'g.DrawString("", fontRegular, Brushes.Black, c8, START_POS, RightFormat)
        g.DrawString("AMOUNT", fontBold, Brushes.Black, c10, START_POS, RightFormat)
        START_POS = START_POS + LINE_SPACE
        DrawLine(g, START_POS, 0)
        START_POS = START_POS + LINE_SPACE
    End Sub
    Public Sub TitlePur(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs, Optional ByVal ftrantype As String = "")
        If ftrantype = "PU" Then
            START_POS = START_POS + LINE_SPACE
            If ftrantype = "PU" And TrantypeMI <> ftrantype Then
                g.DrawString("", fontBoldTitle, Brushes.Black, c4, START_POS)
                Trantypefoot = "PU"
                g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS))
                START_POS = START_POS + LINE_SPACE
            Else
                Trantypefoot = "PU"
            End If
            'If ftrantype = "SR" And TrantypeMI <> ftrantype Then g.DrawString("SALES RETURN", fontBoldTitle, Brushes.Black, c4, START_POS) : Trantypefoot = "SR" : g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS)) : START_POS = START_POS + LINE_SPACE
            If dtSales.Rows(PagecountSale).Item("RESULT").ToString = "0.0" And TrantypeMI <> ftrantype Then
                g.DrawString(dtSales.Rows(PagecountSale).Item("ITEMNAME").ToString(), fontBoldsmall, BlackBrush, c8 - 20, START_POS, LeftFormat)
            ElseIf dtSales.Rows(PagecountSale).Item("RESULT").ToString = "0.0" And TrantypeMI = ftrantype Then
                g.DrawString("PURCHASE", fontBoldTitle, Brushes.Black, c4, START_POS - 18)
                g.DrawString(dtSales.Rows(PagecountSale).Item("ITEMNAME").ToString(), fontBoldsmall, BlackBrush, c8 - 20, START_POS - 18, LeftFormat)
            End If
            If TrantypeMI <> ftrantype Then
                START_POS = START_POS + LINE_SPACE
            End If
            DrawLine(g, START_POS, 0)
            RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
            If _SILVERBILLPRINT = "Y" Then
                g.DrawString("SNo", fontRegular, Brushes.Black, c1, START_POS, LeftFormat)
                g.DrawString("DESCRIPTION", fontRegular, Brushes.Black, c3, START_POS, LeftFormat)
                g.DrawString("", fontRegular, Brushes.Black, c2, START_POS, LeftFormat)
                'g.DrawString("PCS", fontRegular, Brushes.Black, c4, START_POS, RightFormat)
                g.DrawString("GROSS WT", fontRegular, Brushes.Black, c4, START_POS, RightFormat)
                g.DrawString("NET WT", fontRegular, Brushes.Black, c5, START_POS, RightFormat)
                g.DrawString("", fontRegular, Brushes.Black, c6, START_POS, RightFormat)
                g.DrawString("", fontRegular, Brushes.Black, c8, START_POS, RightFormat) 'VA
                g.DrawString("", fontRegular, Brushes.Black, c9, START_POS, RightFormat)
                'g.DrawString("MC", fontRegular, Brushes.Black, c9, START_POS, RightFormat)
                g.DrawString("AMOUNT", fontRegular, Brushes.Black, c10, START_POS, RightFormat)
            Else
                g.DrawString("SNo", fontRegular, Brushes.Black, c1, START_POS, LeftFormat)
                g.DrawString("DESCRIPTION", fontRegular, Brushes.Black, c3, START_POS, LeftFormat)
                g.DrawString("HSN", fontRegular, Brushes.Black, c2, START_POS, LeftFormat)
                'g.DrawString("PCS", fontRegular, Brushes.Black, c4, START_POS, RightFormat)
                g.DrawString("GROSS WT", fontRegular, Brushes.Black, c4, START_POS, RightFormat)
                g.DrawString("NET WT", fontRegular, Brushes.Black, c5, START_POS, RightFormat)
                g.DrawString("WAST", fontRegular, Brushes.Black, c6, START_POS, RightFormat)
                g.DrawString("STN/DUST", fontRegular, Brushes.Black, c8, START_POS, RightFormat) 'VA
                g.DrawString("RATE", fontRegular, Brushes.Black, c9, START_POS, RightFormat)
                'g.DrawString("MC", fontRegular, Brushes.Black, c9, START_POS, RightFormat)
                g.DrawString("AMOUNT", fontRegular, Brushes.Black, c10, START_POS, RightFormat)
            End If
            START_POS = START_POS + LINE_SPACE
            DrawLine(g, START_POS, 0)
            START_POS = START_POS + LINE_SPACE
        ElseIf ftrantype = "SR" Then
            START_POS = START_POS + LINE_SPACE
            ' If ftrantype = "PU" And TrantypeMI <> ftrantype Then g.DrawString("PURCHASE", fontBoldTitle, Brushes.Black, c4, START_POS) : Trantypefoot = "PU" : g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS)) : START_POS = START_POS + LINE_SPACE
            If ftrantype = "SR" And TrantypeMI <> ftrantype Then g.DrawString("", fontBoldTitle, Brushes.Black, c4, START_POS) : Trantypefoot = "SR" : g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS)) : START_POS = START_POS + LINE_SPACE
            If dtSales.Rows(PagecountSale).Item("RESULT").ToString = "0.0" And TrantypeMI <> ftrantype Then
                g.DrawString(dtSales.Rows(PagecountSale).Item("ITEMNAME").ToString(), fontBoldsmall, BlackBrush, c8 - 20, START_POS, LeftFormat)
            ElseIf dtSales.Rows(PagecountSale).Item("RESULT").ToString = "0.0" And TrantypeMI = ftrantype Then
                g.DrawString("SALES RETURN", fontBoldTitle, Brushes.Black, c4, START_POS - 18)
                g.DrawString(dtSales.Rows(PagecountSale).Item("ITEMNAME").ToString(), fontBoldsmall, BlackBrush, c8 - 20, START_POS - 18, LeftFormat)
            End If
            If TrantypeMI <> ftrantype Then
                START_POS = START_POS + LINE_SPACE
            End If
            DrawLine(g, START_POS, 0)
            RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
            If _SILVERBILLPRINT = "Y" Then
                g.DrawString("SNo", fontRegular, Brushes.Black, c1, START_POS, LeftFormat)
                g.DrawString("DESCRIPTION", fontRegular, Brushes.Black, c3, START_POS, LeftFormat)
                g.DrawString("", fontRegular, Brushes.Black, c2, START_POS, LeftFormat)
                'g.DrawString("PCS", fontRegular, Brushes.Black, c4, START_POS, RightFormat)
                g.DrawString("GROSS WT", fontRegular, Brushes.Black, c4, START_POS, RightFormat)
                g.DrawString("NET WT", fontRegular, Brushes.Black, c5, START_POS, RightFormat)
                g.DrawString("", fontRegular, Brushes.Black, c6, START_POS, RightFormat)
                g.DrawString("", fontRegular, Brushes.Black, c8, START_POS, RightFormat) 'VA
                g.DrawString("", fontRegular, Brushes.Black, c9, START_POS, RightFormat)
                'g.DrawString("MC", fontRegular, Brushes.Black, c9, START_POS, RightFormat)
                g.DrawString("AMOUNT", fontRegular, Brushes.Black, c10, START_POS, RightFormat)
            Else
                g.DrawString("SNo", fontRegular, Brushes.Black, c1, START_POS, LeftFormat)
                g.DrawString("DESCRIPTION", fontRegular, Brushes.Black, c3, START_POS, LeftFormat)
                g.DrawString("HSN", fontRegular, Brushes.Black, c2, START_POS, LeftFormat)
                'g.DrawString("PCS", fontRegular, Brushes.Black, c4, START_POS, RightFormat)
                g.DrawString("GROSS WT", fontRegular, Brushes.Black, c4, START_POS, RightFormat)
                g.DrawString("NET WT", fontRegular, Brushes.Black, c5, START_POS, RightFormat)
                g.DrawString("WAST", fontRegular, Brushes.Black, c6, START_POS, RightFormat)
                g.DrawString("STN/DUST", fontRegular, Brushes.Black, c8, START_POS, RightFormat) 'VA
                g.DrawString("RATE", fontRegular, Brushes.Black, c9, START_POS, RightFormat)
                'g.DrawString("MC", fontRegular, Brushes.Black, c9, START_POS, RightFormat)
                g.DrawString("AMOUNT", fontRegular, Brushes.Black, c10, START_POS, RightFormat)
            End If
            START_POS = START_POS + LINE_SPACE
            DrawLine(g, START_POS, 0)
            'START_POS = START_POS + LINE_SPACE
        End If
    End Sub
    Public Sub TitleOrderRepairBooking(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
        g.DrawString("SNo", fontRegular, Brushes.Black, c1, START_POS, LeftFormat)
        g.DrawString("DESCRIPTION", fontRegular, Brushes.Black, c2, START_POS, LeftFormat)
        g.DrawString("", fontRegular, Brushes.Black, c3, START_POS, RightFormat)
        g.DrawString("", fontRegular, Brushes.Black, c4, START_POS, RightFormat)
        g.DrawString("", fontRegular, Brushes.Black, c5, START_POS, RightFormat)
        g.DrawString("QTY", fontRegular, Brushes.Black, c6, START_POS, RightFormat)
        g.DrawString("", fontRegular, Brushes.Black, c7, START_POS, RightFormat)
        g.DrawString("", fontRegular, Brushes.Black, c8, START_POS, RightFormat)
        g.DrawString("GROSS WT", fontRegular, Brushes.Black, c9, START_POS, RightFormat)
        START_POS = START_POS + LINE_SPACE
        DrawLine(g, START_POS, 0)
        START_POS = START_POS + LINE_SPACE
    End Sub
    Public Sub TitleAdvanceReceipt(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        'ADVANCE ONLY DISPLAY
        If dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("Field1").ToString.Contains("ADVANCE ") Then
            RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
            g.DrawString("ItemId", fontBold, Brushes.Black, c1, START_POS, LeftFormat)
            g.DrawString("ItemName", fontBold, Brushes.Black, c2, START_POS, LeftFormat)
            g.DrawString("", fontRegular, Brushes.Black, c3, START_POS, RightFormat)
            g.DrawString("", fontRegular, Brushes.Black, c4, START_POS, RightFormat)
            g.DrawString("", fontRegular, Brushes.Black, c5, START_POS, RightFormat)
            g.DrawString("Weight", fontBold, Brushes.Black, c6, START_POS, RightFormat)
            g.DrawString("", fontRegular, Brushes.Black, c7, START_POS, RightFormat)
            g.DrawString("", fontRegular, Brushes.Black, c8, START_POS, RightFormat)
            g.DrawString("", fontRegular, Brushes.Black, c9, START_POS, RightFormat)
            START_POS = START_POS + LINE_SPACE
        End If
    End Sub
    Public Sub TitleSmithIssueReceipt(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
        g.DrawString("SNo", fontRegular, Brushes.Black, c1, START_POS, LeftFormat)
        g.DrawString("HSN", fontRegular, Brushes.Black, c2, START_POS, LeftFormat)
        g.DrawString("Description", fontRegular, Brushes.Black, c3, START_POS, LeftFormat)
        g.DrawString("Purity", fontRegular, Brushes.Black, c4, START_POS, RightFormat)
        g.DrawString("Quantity", fontRegular, Brushes.Black, c5, START_POS, RightFormat)
        g.DrawString("Gross.Wt", fontRegular, Brushes.Black, c6, START_POS, RightFormat)
        g.DrawString("", fontRegular, Brushes.Black, c7, START_POS, RightFormat)
        g.DrawString("Net.Wt", fontRegular, Brushes.Black, c8, START_POS, RightFormat)
        g.DrawString("", fontRegular, Brushes.Black, c9, START_POS, RightFormat)
        g.DrawString("Amount", fontRegular, Brushes.Black, c10, START_POS, RightFormat)
        START_POS = START_POS + LINE_SPACE
    End Sub
    Public Sub TitleSmithIssueReceipt2(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
        g.DrawString("SNo", fontRegularsmall, Brushes.Black, sm1, START_POS, LeftFormat)
        g.DrawString("Description", fontRegularsmall, Brushes.Black, sm2, START_POS, LeftFormat)
        g.DrawString("HSN", fontRegularsmall, Brushes.Black, sm20, START_POS, LeftFormat)
        g.DrawString("Touch", fontRegularsmall, Brushes.Black, sm3, START_POS, RightFormat)
        g.DrawString("Grs.Wt", fontRegularsmall, Brushes.Black, sm4, START_POS, RightFormat)
        g.DrawString("Stone.Wt", fontRegularsmall, Brushes.Black, sm5, START_POS, RightFormat)
        g.DrawString("Net.Wt", fontRegularsmall, Brushes.Black, sm6, START_POS, RightFormat)
        g.DrawString("Purity", fontRegularsmall, Brushes.Black, sm7, START_POS, RightFormat)
        g.DrawString("Pure.Wt", fontRegularsmall, Brushes.Black, sm8, START_POS, RightFormat)
        g.DrawString("Wastage", fontRegularsmall, Brushes.Black, sm9, START_POS, RightFormat)
        g.DrawString("MC", fontRegularsmall, Brushes.Black, sm10, START_POS, RightFormat)
        g.DrawString("Amount", fontRegularsmall, Brushes.Black, sm11, START_POS, RightFormat)

        START_POS = START_POS + LINE_SPACE
    End Sub
    Public Sub TitleSmithIssueReceiptSmithItemNameA4(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
        g.DrawString("SNo", fontRegularsmall, Brushes.Black, smI1, START_POS, LeftFormat)
        g.DrawString("Description", fontRegularsmall, Brushes.Black, smI2, START_POS, LeftFormat)
        g.DrawString("HSN", fontRegularsmall, Brushes.Black, smI20, START_POS, LeftFormat)
        g.DrawString("Touch", fontRegularsmall, Brushes.Black, smI3, START_POS, RightFormat)
        g.DrawString("PCS", fontRegularsmall, Brushes.Black, smI21, START_POS, RightFormat)
        g.DrawString("Grs.Wt", fontRegularsmall, Brushes.Black, smI4, START_POS, RightFormat)
        g.DrawString("Stone.Wt", fontRegularsmall, Brushes.Black, smI5, START_POS, RightFormat)
        g.DrawString("Net.Wt", fontRegularsmall, Brushes.Black, smI6, START_POS, RightFormat)
        g.DrawString("Purity", fontRegularsmall, Brushes.Black, smI7, START_POS, RightFormat)
        g.DrawString("Pure.Wt", fontRegularsmall, Brushes.Black, smI8, START_POS, RightFormat)
        g.DrawString("Yield", fontRegularsmall, Brushes.Black, smI9, START_POS, RightFormat)
        g.DrawString(Space(1) & "/Processing Loss", fontRegularsmall, Brushes.Black, smI9 - 40, START_POS + LINE_SPACE, LeftFormat)
        g.DrawString("MC", fontRegularsmall, Brushes.Black, smI10, START_POS, RightFormat)
        g.DrawString("Amount", fontRegularsmall, Brushes.Black, smI11, START_POS, RightFormat)
        START_POS = START_POS + LINE_SPACE
    End Sub
    Public Sub TitleSmithIssueReceiptSmithCatNameA4(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
        g.DrawString("SNo", fontRegularsmall, Brushes.Black, smc1, START_POS, LeftFormat)
        g.DrawString("Description", fontRegularsmall, Brushes.Black, smc2, START_POS, LeftFormat)
        g.DrawString("HSN", fontRegularsmall, Brushes.Black, smc3, START_POS, LeftFormat)
        g.DrawString("PCS", fontRegularsmall, Brushes.Black, smc10, START_POS, RightFormat)
        g.DrawString("Grs.Wt", fontRegularsmall, Brushes.Black, smc4, START_POS, RightFormat)
        g.DrawString("Stone.Wt", fontRegularsmall, Brushes.Black, smc5, START_POS, RightFormat)
        g.DrawString("Net.Wt", fontRegularsmall, Brushes.Black, smc6, START_POS, RightFormat)
        'g.DrawString("Wastage", fontRegularsmall, Brushes.Black, smc7, START_POS, RightFormat)
        g.DrawString("Yield", fontRegularsmall, Brushes.Black, smc7, START_POS, RightFormat)
        g.DrawString(Space(1) & "/Processing Loss", fontRegularsmall, Brushes.Black, smc7 - 40, START_POS + LINE_SPACE, LeftFormat)
        g.DrawString("MC", fontRegularsmall, Brushes.Black, smc8, START_POS, RightFormat)
        g.DrawString("Amount", fontRegularsmall, Brushes.Black, smc9, START_POS, RightFormat)
        START_POS = START_POS + LINE_SPACE
    End Sub

    Public Sub FooterSmithIssueReceipt(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        ' g.DrawString("COPY  ", New Font("Palatino Linotype", 8, FontStyle.Bold), Brushes.Black, c6, 600)
        g.DrawLine(Pens.Silver, 20.0F, HLINEPOINT, 20.0F, 445.0F) 'LEFT TOP
        g.DrawLine(Pens.Silver, 775.0F, HLINEPOINT, 775.0F, 445.0F) 'RIGHT TOP


        'g.DrawLine(Pens.Silver, 20.0F, 800.0F, 20.0F, 1000.0F) 'LEFT BOTTOM
        'g.DrawLine(Pens.Silver, 775.0F, 800.0F, 775.0F, 1000.0F) 'RIGHT BOTTOM
        ''
        '' Vertical Line
        g.DrawLine(Pens.Silver, 20.0F, 445.0F, 775.0F, 445.0F)

        If dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count > 0 And smithfooter_flag = True Then
            Dim UserName As String = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("USERNAME").ToString
            Dim MF_GD_BAL As String = Val(dsSmithIssueReceipt.Tables("BALANCE").Compute("SUM(CLOSING_GRSWT)", "METALID='G'").ToString)
            Dim MF_S_DIA_C_BAL As String = Val(dsSmithIssueReceipt.Tables("BALANCE").Compute("SUM(CLOSING_GRSWT)", "METALID='T' AND  STONE_TYPE = 'C' ").ToString)
            Dim MF_S_DIA_G_BAL As String = Val(dsSmithIssueReceipt.Tables("BALANCE").Compute("SUM(CLOSING_GRSWT)", "METALID='T' AND  STONE_TYPE = 'G' ").ToString)
            Dim MF_B_DIA_BAL As String = Val(dsSmithIssueReceipt.Tables("BALANCE").Compute("SUM(CLOSING_GRSWT)", "METALID='D' AND CAT_GROUP= 1").ToString)
            Dim MF_S_DIA_BAL As String = Val(dsSmithIssueReceipt.Tables("BALANCE").Compute("SUM(CLOSING_GRSWT)", "METALID='D' AND CAT_GROUP <> 1").ToString)
            Dim MF_SL_BAL As String = Val(dsSmithIssueReceipt.Tables("BALANCE").Compute("SUM(CLOSING_GRSWT)", "METALID='S'").ToString)

            'g.DrawLine(Pens.Silver, 20.0F, 230.0F, 20.0F, 445.0F) 'LEFT TOP
            'g.DrawLine(Pens.Silver, 775.0F, 230.0F, 775.0F, 445.0F) 'RIGHT TOP


            If BillPrint_SmithBal And smithfooter_flag = True Then
                If MF_GD_BAL <> "0" And _SILVERBILLPRINT <> "Y" Then
                    g.DrawString("MF GD  : " + MF_GD_BAL + "gm", fontRegular, Brushes.Black, c1, 460)
                End If
                If _SILVERBILLPRINT = "Y" And MF_SL_BAL <> "0" Then
                    g.DrawString("MF SL  : " + MF_SL_BAL + "gm", fontRegular, Brushes.Black, c1, 460)
                End If
                If MF_S_DIA_G_BAL <> "0" Then
                    g.DrawString("MF STN : " + MF_S_DIA_G_BAL + "gm", fontRegular, Brushes.Black, c1, 475)
                End If
                If MF_S_DIA_C_BAL <> "0" Then
                    g.DrawString("MF STN : " + MF_S_DIA_C_BAL + "ct", fontRegular, Brushes.Black, c2 + 90, 475)
                End If

                If MF_B_DIA_BAL <> "0" Then
                    g.DrawString("MF B DIA  : " + MF_B_DIA_BAL + "ct", fontRegular, Brushes.Black, c2 + 90, 460)
                End If
                If MF_S_DIA_BAL <> "0" Then
                    g.DrawString("MF S DIA : " + MF_S_DIA_BAL + "ct", fontRegular, Brushes.Black, h1 + 66, 460)
                End If
            End If

            g.DrawString("For " & strCompanyName, New Font("Times New Roman", 10, FontStyle.Regular), Brushes.Black, c5, 460)
            'g.DrawString(Rules(0).ToString, fontRegular, Brushes.Black, c1, 450)
            ' g.DrawString(Rules(1).ToString, fontRegular, Brushes.Black, c1, 465)
            g.DrawString("Entered By ", fontRegular, Brushes.Black, c1, 510)
            g.DrawString(UserName, fontRegularsmall, Brushes.Black, c1, 525)
            g.DrawString("Receiver Signature", fontRegular, Brushes.Black, 250, 510)
            g.DrawString("Verified By", fontRegular, Brushes.Black, c5, 510)
            g.DrawString("Authorised Signatory", fontRegular, Brushes.Black, c7, 510)
            ' g.DrawString("Tax is Payable on Reverse Charge : YES/NO ", fontRegular, Brushes.Black, c5, 515)
        End If
        ''
        smithfooter_flag = True
    End Sub

    Public Sub FooterSmithIssueReceiptA4(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal y1 As Integer, ByVal y2 As Integer)
        If dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count > 0 Then
            Dim UserName As String = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("USERNAME").ToString
            'g.DrawLine(Pens.Silver, 20.0F, y1, 20.0F, y2)
            'g.DrawLine(Pens.Silver, 775.0F, y1, 775.0F, y2)
            g.DrawString("For " & strCompanyName, New Font("Times New Roman", 7, FontStyle.Regular), Brushes.Black, 450, y2 + 18)
            'g.DrawString(Rules(0).ToString, fontRegular, Brushes.Black, c1, y2)
            'y2 = y2 + 18
            'g.DrawString(Rules(1).ToString, fontRegular, Brushes.Black, c1, y2)
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
        Dim T1 As String = "Page " & NoofPrint.ToString
        '' Vertical Line
        Select Case Trantype
            Case "REP"
                g.DrawString("NOTE : ITEM(S) MENTIONED ABOVE WILL BE DELIVERED ONLY BY PRODUCING ORIGINAL VOUCHER", fontBold, Brushes.Black, c2, BOTTOM_POS - 590)
            Case "ORD"
                g.DrawString("NOTE : ORIGINAL ORDER BOOKING RECEIPT SHOULD BE PRODUCED AT THE TIME OF ITEM(S) DELIVERY", fontBold, Brushes.Black, c2, Bottom - 590)
            Case Else
        End Select
        Select Case Trantype
            Case "POS"
        End Select
        If _SILVERBILLPRINT = "Y" Then
            g.DrawString("Salesman", fontRegular, Brushes.Black, c1, BOTTOM_POS + LINE_SPACE + 3)
            g.DrawString("Cashier", fontRegular, Brushes.Black, c3, BOTTOM_POS + LINE_SPACE + 3)
            g.DrawString("Customer Signature", fontRegular, Brushes.Black, c4, BOTTOM_POS + LINE_SPACE + 3)
            g.DrawString("For " & strCompanyName, New Font("Times New Roman", 8, FontStyle.Regular), Brushes.Black, c8, BOTTOM_POS + LINE_SPACE + 3)
        End If
        If NoofPrint > 1 Then
            g.DrawString(T1, fontRegular, Brushes.Black, c8, BOTTOM_POS + 3)
        End If
        If _SILVERBILLPRINT = "Y" Then
            DrawLine(g, BOTTOM_POS + LINE_SPACE + LINE_SPACE + 3, 0)
        End If
        If placeofSupply = GetSqlValue(cn, "SELECT (SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID=C.STATEID) STATENAME FROM " & cnAdminDb & "..COMPANY  C WHERE C.COMPANYID='" & strCompanyId & "'") Then
            If FOOTER_BILLPRINT <> "" Then
                g.DrawString(FOOTER_BILLPRINT, fontRegular, Brushes.Black, c1, BOTTOM_POS + 55)
            Else
                g.DrawString("Delivery at Showroom", fontRegular, Brushes.Black, c1, BOTTOM_POS + 55)
            End If
        Else
            strsql = " SELECT  COUNT(*)CNT FROM " & cnStockDb & "..TAXTRAN "
            strsql += vbCrLf + " WHERE BATCHNO = '" & pBatchno & "' AND ISNULL(TAXID,'')='IG' "
            Dim dtIgstInfo As DataTable
            da = New OleDbDataAdapter(strsql, cn)
            dtIgstInfo = New DataTable
            da.Fill(dtIgstInfo)
            If dtIgstInfo.Rows.Count > 0 Then
                If Val(dtIgstInfo.Rows(0).Item("CNT").ToString) > 0 Then
                    g.DrawString("Delivery at " & placeofSupply, fontRegular, Brushes.Black, c1, BOTTOM_POS + 50)
                    g.DrawString(Rules(0).ToString, New Font("Palatino Linotype", 8, FontStyle.Regular), Brushes.Black, c3, BOTTOM_POS + 70)
                    g.DrawString(Rules(1).ToString, New Font("Palatino Linotype", 8, FontStyle.Regular), Brushes.Black, c3, BOTTOM_POS + 90)
                Else
                    If FOOTER_BILLPRINT <> "" Then
                        g.DrawString(FOOTER_BILLPRINT, fontRegular, Brushes.Black, c1, BOTTOM_POS + 55)
                    Else
                        g.DrawString("Delivery at Showroom", fontRegular, Brushes.Black, c1, BOTTOM_POS + 55)
                    End If
                End If
            Else
                If FOOTER_BILLPRINT <> "" Then
                    g.DrawString(FOOTER_BILLPRINT, fontRegular, Brushes.Black, c1, BOTTOM_POS + 55)
                Else
                    g.DrawString("Delivery at Showroom", fontRegular, Brushes.Black, c1, BOTTOM_POS + 55)
                End If
            End If
        End If
        If _SILVERBILLPRINT = "Y" Then
            g.DrawString("(Certified that the particulars given above are true and correct)", fontRegular, Brushes.Black, c4, BOTTOM_POS + 50)
        End If
        If Trantypefoot = "PU" Then
            g.DrawRectangle(New Pen(Color.Silver, 1), c1 - 10, BOTTOM_POS + 100, 130, 15)
            g.DrawString("PURCHASE BILL", fontRegular, Brushes.Black, c1, BOTTOM_POS + 100)
        ElseIf Trantypefoot = "SR" Then
            If _SILVERBILLPRINT = "Y" Then
                g.DrawString("THE ITEMS SOLD VIDE THIS INVOICE IS / ARE NOT ELIGIBLE FOR EXCHANGE AT FULL VALUE AS IT IS AGAINST SALES RETURN", fontRegular, Brushes.Black, c1, BOTTOM_POS + 100)
            Else
                g.DrawRectangle(New Pen(Color.Silver, 1), c1 - 10, BOTTOM_POS + 100, 130, 15)
                g.DrawString("SALES RETURN", fontRegular, Brushes.Black, c1, BOTTOM_POS + 100)
            End If
        End If
        If gvprint_flag = True Then
            strsql = " SELECT  TRANNO,TRANDATE,RUNNO,SUM(GRSWT)GRSWT,REMARK1 FROM " & cnAdminDb & "..OUTSTANDING "
            strsql += vbCrLf + " WHERE BATCHNO = '" & pBatchno & "' AND TRANTYPE ='GV' AND RECPAY='R' GROUP BY TRANNO,TRANDATE,RUNNO,REMARK1 "
            da = New OleDbDataAdapter(strsql, cn)
            dtGVInfo = New DataTable
            da.Fill(dtGVInfo)
            If dtGVInfo.Rows.Count > 0 Then
                Dim strqrdata As String = dtGVInfo.Rows(0).Item("RUNNO").ToString
                Dim gen As New QRCodeGenerator
                Dim qr_data = gen.CreateQrCode(strqrdata, QRCodeGenerator.ECCLevel.Q)
                Dim code As New QRCode(qr_data)
                QrcodeCustomerGf = code.GetGraphic(6)
                funcCustomerQrcodeGF(g, e, c6, BOTTOM_POS + 240)
                g.DrawString("Name of the Customer :  " + gv_Cusname, fontRegular, Brushes.Black, c3 + 20, BOTTOM_POS + 240)
                g.DrawString("Bill No    :  " + dtGVInfo.Rows(0).Item("TRANNO").ToString, fontRegular, Brushes.Black, c8, BOTTOM_POS + 240)
                g.DrawString("Bill Date :  " + prtBilldate.ToString, fontRegular, Brushes.Black, c8, BOTTOM_POS + 260)
                g.DrawString("G V No   :  " + dtGVInfo.Rows(0).Item("RUNNO").ToString, fontRegular, Brushes.Black, c8, BOTTOM_POS + 280)

                g.DrawString("Please exchange this Gift coupon for   ", fontRegular, Brushes.Black, c3 + 20, BOTTOM_POS + 310)
                g.DrawString(dtGVInfo.Rows(0).Item("GRSWT").ToString, fontRegular, Brushes.Black, c4 + 20, BOTTOM_POS + 310)
                g.DrawString("___________", fontRegular, Brushes.Black, c4 + 5, BOTTOM_POS + 315)
                If dtGVInfo.Rows(0).Item("REMARK1").ToString.Contains("TO SILVER WEIGHT") Then
                    g.DrawString("grams of Silver at our Silver Section.  ", fontRegular, Brushes.Black, c5, BOTTOM_POS + 310)
                ElseIf dtGVInfo.Rows(0).Item("REMARK1").ToString.Contains("TO GOLD WEIGHT") Then
                    g.DrawString("grams of Gold at our Gold Section.  ", fontRegular, Brushes.Black, c5, BOTTOM_POS + 310)
                End If
                If printcpy = "ACCOUNTS COPY" Then
                    g.DrawString("*This is Only For Accounts Purpose and NOT a valid Gift Voucher.  ", fontRegular, Brushes.Black, c3 + 20, BOTTOM_POS + 345)
                End If
                g.DrawString("For " & strCompanyName, New Font("Times New Roman", 8, FontStyle.Regular), Brushes.Black, c8, BOTTOM_POS + 345)
                e.Graphics.RotateTransform(0)
                Dim myfont As New Font("Arial", 40)
                Dim myBrush As New SolidBrush(Color.FromArgb(30, 5, 5, 255))
                If _duplicate = "Y" Then
                    If printcpy = "ACCOUNTS COPY" Then
                        e.Graphics.DrawString(printcpy, myfont, myBrush, c3, BOTTOM_POS + 320)
                    End If
                    e.Graphics.DrawString("DUPLICATE COPY", myfont, myBrush, c3, BOTTOM_POS + 360)
                Else
                    If printcpy = "ACCOUNTS COPY" Then
                        e.Graphics.DrawString(printcpy, myfont, myBrush, c3, BOTTOM_POS + 320)
                    End If
                End If
            End If
        End If
    End Sub
    Public Sub FooterApproval(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        Dim TAPPBOTTOM_POS As Integer = APPBOTTOM_POS
        Dim EMPID As String = dtSales.Rows(0).Item("EMPID").ToString
        If TrantypeMI = "AI" Then
            Dim DUEDATE As String = Format(GetSqlValue(cn, "SELECT DUEDATE FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO = '" & pBatchno & "'"), "dd-MM-yyyy").ToString
            Dim T1 As String = "Page " & NoofPrint.ToString
            If _SILVERBILLPRINT = "Y" Then
                g.DrawString("SILVER RATE WHICHEVER IS HIGHER WILL BE APPLICABLE FOR JEWELES ISSUED ON INSPECTION BILL AT THE TIME OF BILLING", fontBoldsmall, Brushes.Black, c1, TAPPBOTTOM_POS)
            Else
                g.DrawString("GOLD RATE WHICHEVER IS HIGHER WILL BE APPLICABLE FOR JEWELES ISSUED ON INSPECTION BILL AT THE TIME OF BILLING", fontBoldsmall, Brushes.Black, c1, TAPPBOTTOM_POS)
            End If
            TAPPBOTTOM_POS = TAPPBOTTOM_POS + LINE_SPACE
            g.DrawLine(Pens.Silver, 20.0F, TAPPBOTTOM_POS, 775.0F, TAPPBOTTOM_POS)
            TAPPBOTTOM_POS = TAPPBOTTOM_POS + LINE_SPACE
            g.DrawString("ACKNOWLEDGEMENT OF ENTRUSTMENT", fontBoldsmall, Brushes.Black, c1, TAPPBOTTOM_POS)
            TAPPBOTTOM_POS = TAPPBOTTOM_POS + LINE_SPACE
            g.DrawString("I/We have received from you goods specifiled above under the following conditions:", fontBoldsmall, Brushes.Black, c1, TAPPBOTTOM_POS)
            TAPPBOTTOM_POS = TAPPBOTTOM_POS + LINE_SPACE
            g.DrawString("1. The goods have been entrustment to me/us on approval for the purpose mentioned above", fontBoldsmall, Brushes.Black, c1, TAPPBOTTOM_POS)
            TAPPBOTTOM_POS = TAPPBOTTOM_POS + LINE_SPACE
            g.DrawString("2. the goods remain your property and I/We have no right or interest in them ", fontBoldsmall, Brushes.Black, c1, TAPPBOTTOM_POS)
            TAPPBOTTOM_POS = TAPPBOTTOM_POS + LINE_SPACE
            g.DrawString("3. I/We aggree not to sell or plegde or mortgage or hypothecate the said goods or other wise deal them in any manner", fontBoldsmall, Brushes.Black, c1, TAPPBOTTOM_POS)
            TAPPBOTTOM_POS = TAPPBOTTOM_POS + LINE_SPACE
            g.DrawString("4. The goods are to be returned to you forthwith whenever demanded back on or before the due date mentioned below", fontBoldsmall, Brushes.Black, c1, TAPPBOTTOM_POS)
            TAPPBOTTOM_POS = TAPPBOTTOM_POS + LINE_SPACE
            g.DrawString("5. I am responsible for the return of the good in the same condition as i received them ", fontBoldsmall, Brushes.Black, c1, TAPPBOTTOM_POS)
            TAPPBOTTOM_POS = TAPPBOTTOM_POS + LINE_SPACE
            g.DrawString("Due Date :" & DUEDATE.ToString, fontBoldsmall, Brushes.Black, c4, TAPPBOTTOM_POS)
            TAPPBOTTOM_POS = TAPPBOTTOM_POS + LINE_SPACE
            g.DrawString(" " & EMPID.ToString(), fontBoldsmall, BlackBrush, c1, TAPPBOTTOM_POS, LeftFormat)
            TAPPBOTTOM_POS = TAPPBOTTOM_POS + LINE_SPACE
            g.DrawString("SalesPerson", fontitalic, BlackBrush, c1, TAPPBOTTOM_POS, LeftFormat)
            g.DrawString("Cashier", fontitalic, BlackBrush, c3, TAPPBOTTOM_POS, LeftFormat)
            g.DrawString("Customer Signature", fontitalic, BlackBrush, c4, TAPPBOTTOM_POS, LeftFormat)
            g.DrawString("For " & strCompanyName, fontitalic, BlackBrush, c7, TAPPBOTTOM_POS, LeftFormat)
            TAPPBOTTOM_POS = TAPPBOTTOM_POS + LINE_SPACE
            g.DrawString("(Certified that the particualr given above are the true and correct)", fontRegular7, BlackBrush, c4 - 50, TAPPBOTTOM_POS, LeftFormat)
            g.DrawString("InsTime:", fontRegular7, BlackBrush, c9, TAPPBOTTOM_POS, LeftFormat)
        Else
            g.DrawString(" " & EMPID.ToString(), fontBoldsmall, BlackBrush, c1, BOTTOM_POS, LeftFormat)
            BOTTOM_POS = BOTTOM_POS + LINE_SPACE
            g.DrawString("SalesPerson", fontitalic, BlackBrush, c1, BOTTOM_POS, LeftFormat)
            g.DrawString("Cashier", fontitalic, BlackBrush, c3, BOTTOM_POS, LeftFormat)
            g.DrawString("Customer Signature", fontitalic, BlackBrush, c4, BOTTOM_POS, LeftFormat)
            g.DrawString("For " & strCompanyName, fontitalic, BlackBrush, c7, BOTTOM_POS, LeftFormat)
            BOTTOM_POS = BOTTOM_POS + LINE_SPACE
            g.DrawString("(Certified that the particualr given above are the true and correct)", fontRegular7, BlackBrush, c4 - 50, BOTTOM_POS, LeftFormat)
            g.DrawString("InsTime:", fontRegular7, BlackBrush, c9, BOTTOM_POS, LeftFormat)
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

    ''Public Sub salesEstimation()
    ''    strsql = "SELECT PRINTERPATH,PORT FROM " & cnAdminDb & "..PRINTERLIST WHERE PRINTERNAME = '" & Environment.MachineName & "' "
    ''    Dim dr As DataRow = Nothing
    ''    dr = GetSqlRow(strsql, cn)
    ''    If Not dr Is Nothing Then
    ''        Dim centPrint As New frmCentPrint(dr.Item("PRINTERPATH").ToString, Val(dr.Item("PORT").ToString) _
    ''        , System.IO.Path.GetTempPath() & "\centprint" & SystemName & " .mem" _
    ''        , strBilldate _
    ''        , EstNo_SA)
    ''    Else
    ''        MsgBox("System Id not in master", MsgBoxStyle.Information)
    ''        Exit Sub
    ''    End If
    ''End Sub


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
                ''TitleAdvanceReceipt(e.Graphics, e)
                'g.DrawString("C O P Y  ", fontBold, Brushes.Black, c8, 600)
                If dsAdvanceReceipt.Tables("ITEMTAG").Rows.Count > 0 Then
                    For PagecountSale = PagecountSale To dsAdvanceReceipt.Tables("ITEMTAG").Rows.Count - 1
                        Exit For
                        NoofPage += 1
                        With dsAdvanceReceipt.Tables("ITEMTAG").Rows(PagecountSale)
                            ''g.DrawString(IIf(Val(.Item("ITEMID").ToString()) > 0, .Item("ITEMID").ToString(), ""), fontRegular, BlackBrush, c1, START_POS, LeftFormat)
                            ''g.DrawString(.Item("ITEMNAME").ToString(), fontRegular, BlackBrush, c2, START_POS, LeftFormat)
                            ''g.DrawString(IIf(Val(.Item("WEIGHT").ToString()) > 0, .Item("WEIGHT").ToString(), ""), fontRegular, BlackBrush, c6, START_POS, rAlign)

                            START_POS = START_POS + LINE_SPACE
                            If START_POS >= BOTTOM_POS Then
                                PagecountSale = PagecountSale + 1
                                START_POS = TSTART_POS ' TOP AGAIN STARTING POSITION
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c1, BOTTOM_POS + 10)
                                gstPrint = True
                                e.HasMorePages = True
                                Exit For
                            End If
                        End With
                    Next
                End If
                If gstPrint = False Then
                    If dsAdvanceReceipt.Tables("GSTTRAN").Rows.Count > 0 Then
                        g.DrawString("CGST : " & dsAdvanceReceipt.Tables("GSTTRAN").Rows(0).Item("CGST").ToString(), fontRegular, BlackBrush, 270, START_POS, LeftFormat)
                        g.DrawString("SGST : " & dsAdvanceReceipt.Tables("GSTTRAN").Rows(0).Item("SGST").ToString(), fontRegular, BlackBrush, c3, START_POS, LeftFormat)
                        g.DrawString("IGST : " & dsAdvanceReceipt.Tables("GSTTRAN").Rows(0).Item("IGST").ToString(), fontRegular, BlackBrush, c4, START_POS, LeftFormat)
                    End If
                End If
                ModeofpaymentInfoAdvanceReceipt(e.Graphics, e)
                g.DrawLine(Pens.DarkGray, c1, START_POS, c10, START_POS)
                If dsAdvanceReceipt.Tables("OUTSTANDING").Rows.Count > 0 Then
                    If dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field1").ToString.Contains("ADVANCE ") Or dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field1").ToString.Contains("RECEIPT  VOUCHER") Or dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field1").ToString.Contains("INVOICE") Then
                        Dim Amt As Double = Val(dsAdvanceReceipt.Tables("OUTSTANDING").Compute("SUM(AMOUNT1)", "").ToString)
                        Dim AmtWords As String = ConvertRupees.RupeesToWord(Amt)

                        START_POS = START_POS + 5
                        'g.DrawString("NOTE : ORIGINAL ADVANCE RECEIPT SHOULD BE PRODUCED AT THE TIME OF ITEM(S) DELIVERY ", fontBold, Brushes.Black, c2, START_POS)
                        g.DrawString("(" + AmtWords + ")", fontBold, Brushes.Black, c2, START_POS)
                        g.DrawString("Total", fontBold, Brushes.Black, c7, START_POS)
                        ''g.DrawString(dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("AMOUNT"), fontBoldTitle, Brushes.Black, c10, START_POS, rAlign)
                        g.DrawString(Format(Val(Amt.ToString), "0.00"), fontBoldTitle, Brushes.Black, c10, START_POS, rAlign)
                        ''g.DrawString(NodeId, fontRegular, BlackBrush, c7, START_POS, rAlign)
                    Else
                        ''g.DrawString(NodeId, fontRegular, BlackBrush, c7, START_POS, rAlign)
                    End If
                End If
                START_POS = START_POS + LINE_SPACE + 5

                g.DrawLine(Pens.DarkGray, c1, START_POS, c10, START_POS)

                If START_POS <= BOTTOM_POS Then '' 838
                    START_POS = BOTTOM_POS
                Else
                    START_POS = BOTTOM_POS ''1050
                End If
                Dim EMP_NAME As String = dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("EMP_NAME").ToString
                START_POS = START_POS
                g.DrawString(EMP_NAME, fontRegular, Brushes.Black, c1, START_POS)
                START_POS = START_POS + LINE_SPACE
                If _SILVERBILLPRINT = "Y" Then
                    g.DrawString("Salesman", fontRegular, Brushes.Black, c1, START_POS + 3)
                    g.DrawString("Cashier", fontRegular, Brushes.Black, c3, START_POS + 3)
                    g.DrawString("Customer Signature", fontRegular, Brushes.Black, c4, START_POS + 3)
                    g.DrawString("For " & strCompanyName, New Font("Times New Roman", 8, FontStyle.Regular), Brushes.Black, c8, START_POS + 3)
                    START_POS = START_POS + LINE_SPACE + LINE_SPACE
                    g.DrawLine(Pens.DarkGray, c1, START_POS, c10, START_POS)
                End If
                If placeofSupply = GetSqlValue(cn, "SELECT (SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID=C.STATEID) STATENAME FROM " & cnAdminDb & "..COMPANY  C WHERE C.COMPANYID='" & strCompanyId & "'") Then
                    If FOOTER_BILLPRINT <> "" Then
                        g.DrawString(FOOTER_BILLPRINT, fontRegular, Brushes.Black, c1, BOTTOM_POS + 60)
                    Else
                        g.DrawString("Delivery at Showroom", fontRegular, Brushes.Black, c1, BOTTOM_POS + 60)
                    End If
                Else
                    g.DrawString("Delivery at " & placeofSupply, fontRegular, Brushes.Black, c1, BOTTOM_POS + 60)
                    ' g.DrawString(Rules(0).ToString, New Font("Palatino Linotype", 8, FontStyle.Regular), Brushes.Black, c3, BOTTOM_POS + 60)
                    'g.DrawString(Rules(1).ToString, New Font("Palatino Linotype", 8, FontStyle.Regular), Brushes.Black, c3, BOTTOM_POS + 80)
                End If
                If _SILVERBILLPRINT = "Y" Then
                    g.DrawString("(Certified that the particulars given above are true and correct)", fontRegular, Brushes.Black, c4, BOTTOM_POS + 60)
                End If

                'TOP 
                ''g.DrawLine(Pens.Silver, 20.0F, 490.0F, 300.0F, 490.0F)
                ''g.DrawLine(Pens.Silver, 460.0F, 490.0F, 775.0F, 490.0F)
                ''g.DrawString("[GSTIN " & GSTNO & "]", fontRegular, Brushes.Black, 300, 480)
                ''g.DrawString("For " & strCompanyName, New Font("Times New Roman", 8, FontStyle.Regular), Brushes.Black, c6, 490)
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            Exit Sub
        End Try
    End Sub
    Public Sub ModeofpaymentInfoAdvanceReceipt(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        Dim rAlign As New StringFormat
        rAlign.Alignment = StringAlignment.Far
        START_POS = START_POS + LINE_SPACE
        Dim J As Integer = 0
        Dim CASHPAIDCHK As String = ""
        If dsAdvanceReceipt.Tables("ACCTRAN").Rows.Count <> 0 Then
            If dsAdvanceReceipt.Tables("ACCTRAN").Rows(0).Item("ACCODE").ToString <> "SALES RETURN " Or dsAdvanceReceipt.Tables("ACCTRAN").Rows(0).Item("ACCODE").ToString <> "PURCHASE     " Then
                g.DrawString("Received By", fontBold, BlackBrush, c2, START_POS, LeftFormat)
            End If
            For J = 0 To dsAdvanceReceipt.Tables("ACCTRAN").Rows.Count - 1
                If CASHPAIDCHK.ToString <> "CASH PAID" Then
                    CASHPAIDCHK = dsAdvanceReceipt.Tables("ACCTRAN").Rows(J).Item("ACCODE").ToString()
                End If
            Next
            For J = 0 To dsAdvanceReceipt.Tables("ACCTRAN").Rows.Count - 1
                If dsAdvanceReceipt.Tables("ACCTRAN").Rows(J).Item("ACCODE").ToString <> "SALES RETURN " Or dsAdvanceReceipt.Tables("ACCTRAN").Rows(0).Item("ACCODE").ToString <> "PURCHASE     " Then
                    START_POS = START_POS + LINE_SPACE
                    If CASHPAIDCHK <> "" Then
                        g.DrawString(dsAdvanceReceipt.Tables("ACCTRAN").Rows(J).Item("ACCODE").ToString, fontRegular, BlackBrush, c2, START_POS, LeftFormat)
                        g.DrawString(dsAdvanceReceipt.Tables("ACCTRAN").Rows(J).Item("AMOUNT").ToString, fontBold, BlackBrush, c10, START_POS, rAlign)
                    Else
                        If Val(dsAdvanceReceipt.Tables("OUTSTANDING").Compute("SUM(AMOUNT)", "").ToString) < dsAdvanceReceipt.Tables("ACCTRAN").Rows(J).Item("AMOUNT").ToString Then
                            g.DrawString("*" & dsAdvanceReceipt.Tables("ACCTRAN").Rows(J).Item("ACCODE").ToString, fontRegular, BlackBrush, c2, START_POS, LeftFormat)
                            g.DrawString(Val(dsAdvanceReceipt.Tables("OUTSTANDING").Compute("SUM(amount)", "").ToString), fontBold, BlackBrush, c10, START_POS, rAlign)
                        Else
                            g.DrawString(dsAdvanceReceipt.Tables("ACCTRAN").Rows(J).Item("ACCODE").ToString, fontRegular, BlackBrush, c2, START_POS, LeftFormat)
                            g.DrawString(dsAdvanceReceipt.Tables("ACCTRAN").Rows(J).Item("AMOUNT").ToString, fontBold, BlackBrush, c10, START_POS, rAlign)
                        End If
                    End If
                    'START_POS = START_POS + LINE_SPACE
                End If
            Next
        End If
        START_POS = START_POS + LINE_SPACE
    End Sub

    Public Sub ModeofpaymentInfogiftvoucher(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        Dim rAlign As New StringFormat
        rAlign.Alignment = StringAlignment.Far
        START_POS = START_POS + LINE_SPACE
        Dim J As Integer = 0
        Dim CASHPAIDCHK As String
        If dsGiftvoucher.Tables("ACCTRAN").Rows.Count <> 0 Then
            If dsGiftvoucher.Tables("ACCTRAN").Rows(0).Item("ACCODE").ToString <> "SALES RETURN " Or dsGiftvoucher.Tables("ACCTRAN").Rows(0).Item("ACCODE").ToString <> "PURCHASE     " Then
                g.DrawString("Received By", fontBold, BlackBrush, c2, START_POS, LeftFormat)
            End If
            For J = 0 To dsGiftvoucher.Tables("ACCTRAN").Rows.Count - 1
                If CASHPAIDCHK <> "CASH PAID" Then
                    CASHPAIDCHK = dsGiftvoucher.Tables("ACCTRAN").Rows(J).Item("ACCODE").ToString()
                End If
            Next
            For J = 0 To dsGiftvoucher.Tables("ACCTRAN").Rows.Count - 1
                If dsGiftvoucher.Tables("ACCTRAN").Rows(J).Item("ACCODE").ToString <> "SALES RETURN " Or dsGiftvoucher.Tables("ACCTRAN").Rows(0).Item("ACCODE").ToString <> "PURCHASE     " Then
                    START_POS = START_POS + LINE_SPACE
                    If CASHPAIDCHK <> "" Then
                        g.DrawString(dsGiftvoucher.Tables("ACCTRAN").Rows(J).Item("AMT").ToString, fontRegular, BlackBrush, c2, START_POS, LeftFormat)
                        g.DrawString(dsGiftvoucher.Tables("ACCTRAN").Rows(J).Item("AMOUNT").ToString, fontBold, BlackBrush, c10, START_POS, rAlign)
                    Else
                        If Val(dsGiftvoucher.Tables("OUTSTANDING").Compute("SUM(AMOUNT)", "").ToString) < dsGiftvoucher.Tables("ACCTRAN").Rows(J).Item("AMOUNT").ToString Then
                            g.DrawString("*" & dsGiftvoucher.Tables("ACCTRAN").Rows(J).Item("AMT").ToString, fontRegular, BlackBrush, c2, START_POS, LeftFormat)
                            g.DrawString(Val(dsGiftvoucher.Tables("OUTSTANDING").Compute("SUM(amount)", "").ToString), fontBold, BlackBrush, c10, START_POS, rAlign)
                        Else
                            g.DrawString(dsGiftvoucher.Tables("ACCTRAN").Rows(J).Item("AMT").ToString, fontRegular, BlackBrush, c2, START_POS, LeftFormat)
                            g.DrawString(dsGiftvoucher.Tables("ACCTRAN").Rows(J).Item("AMOUNT").ToString, fontBold, BlackBrush, c10, START_POS, rAlign)
                        End If
                    End If
                    'START_POS = START_POS + LINE_SPACE
                End If
            Next
        End If
        START_POS = START_POS + LINE_SPACE
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

                                'c6
                                g.DrawLine(Pens.Silver, 530, START_POS, c6, START_POS)
                                g.DrawLine(Pens.Silver, 710, START_POS, c10, START_POS)

                            End If
                            g.DrawString(IIf(Val(.Item("SRNO").ToString()) > 0, .Item("SRNO").ToString(), ""), fontRegular, BlackBrush, c1, START_POS, LeftFormat)
                            If .Item("COLHEAD").ToString <> "G" Then
                                g.DrawString(.Item("ITEMNAME").ToString(), fontRegular, BlackBrush, c2, START_POS, LeftFormat)
                            ElseIf .Item("COLHEAD").ToString = "G" And .Item("RESULT").ToString = "12" Then
                                g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c3, START_POS, LeftFormat)
                            ElseIf .Item("COLHEAD").ToString = "G" Then
                                g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c2, START_POS, LeftFormat)
                            End If
                            g.DrawString(IIf(Val(.Item("PCS").ToString()) > 0, Val(.Item("PCS").ToString()), ""), fontRegular, BlackBrush, c6, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("GRSWT").ToString()) > 0, .Item("GRSWT").ToString(), ""), fontRegular, BlackBrush, c9, START_POS, rAlign)
                            START_POS = START_POS + LINE_SPACE
                            If .Item("RESULT").ToString = "9" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "U" Then
                                'c6
                                g.DrawLine(Pens.Silver, 530, START_POS, c6, START_POS)
                                g.DrawLine(Pens.Silver, 710, START_POS, c10, START_POS)
                            End If
                            If START_POS >= BOTTOM_POS Then
                                PagecountSale = PagecountSale + 1
                                START_POS = TSTART_POS ' TOP AGAIN STARTING POSITION
                                g.DrawString("Continue.... ", fontRegular, Brushes.Black, c1, BOTTOM_POS + 10)
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
            Dim _WASTAGE_REC As Double = 0
            Dim _WASTAGE_ISS As Double = 0
            Dim _Alloy As Double = 0
            Dim NoofPage As Integer = 0
            Dim rAlign As New StringFormat
            rAlign.Alignment = StringAlignment.Far
            NoofPrint += 1
            Using g As Graphics = e.Graphics
                CustomerInfoSmithIssRec(Tranno, strBilldate, e.Graphics, e)
                TitleSmithIssueReceipt(e.Graphics, e)
                g.DrawLine(Pens.Silver, 20, START_POS, 775, START_POS)
                RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
                If dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count > 0 And dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows.Count > 0 Then
                    For PagecountSale = PagecountSale To dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count - 1
                        With dsSmithIssueReceipt.Tables("DTISSREC").Rows(PagecountSale)
                            If .Item("ACCODE").ToString = "STKTRAN" And .Item("TTYPE").ToString = "IIN" Then
                                g.DrawString("ISSUE :", fontRegular, BlackBrush, c1, START_POS, LeftFormat)
                                START_POS = START_POS + LINE_SPACE
                            End If
                            If .Item("ACCODE").ToString = "STKTRAN" And .Item("TTYPE").ToString = "RIN" Then
                                g.DrawString("RECEIPT :", fontRegular, BlackBrush, c1, START_POS, LeftFormat)
                                START_POS = START_POS + LINE_SPACE
                            End If
                            If .Item("ACCODE").ToString = "DIFFWT" And .Item("TTYPE").ToString = "IIS" Then
                                g.DrawString("", fontRegular, BlackBrush, c1, START_POS, LeftFormat)
                                g.DrawString("LESS DIFF. WGT ", fontRegular, BlackBrush, c2, START_POS, LeftFormat)
                            Else '' SIRE
                                'If .Item("RESULT").ToString = "6.0" And .Item("RESULT").ToString = "5.0" And Val(.Item("GRSWT").ToString()) <> 0 And .Item("SERNO").ToString = "ZZZZZZZZ" And (.Item("TRANTYPE").ToString = "gms" Or .Item("TRANTYPE").ToString = "cts") Then

                                If .Item("TRANTYPE").ToString = "gms" Or .Item("TRANTYPE").ToString = "cts" Then

                                    g.DrawString(IIf(Val(.Item("SNO").ToString()) > 0, .Item("SNO").ToString(), ""), fontRegular, BlackBrush, c1, START_POS, LeftFormat)
                                    If .Item("FLAG").ToString() = "M" Then
                                        g.DrawString(.Item("CATNAME").ToString() + .Item("TABLENAME").ToString(), fontRegular, BlackBrush, c3, START_POS, LeftFormat)
                                    Else
                                        If .Item("ITEMNAME").ToString() <> "" And .Item("CATNAME").ToString() <> "" Then
                                            g.DrawString(.Item("ITEMNAME").ToString() + .Item("TABLENAME").ToString(), fontRegular, BlackBrush, c3, START_POS, LeftFormat)
                                        Else
                                            g.DrawString(.Item("CATNAME").ToString() + .Item("TABLENAME").ToString(), fontRegular, BlackBrush, c3, START_POS, LeftFormat)
                                        End If
                                    End If
                                Else
                                    g.DrawString(IIf(Val(.Item("SNO").ToString()) > 0, .Item("SNO").ToString(), ""), fontRegular, BlackBrush, c1, START_POS, LeftFormat)
                                    If .Item("FLAG").ToString() = "M" Then
                                        g.DrawString(.Item("CATNAME").ToString(), fontRegular, BlackBrush, c3, START_POS, LeftFormat)
                                    Else
                                        If .Item("ITEMNAME").ToString() <> "" And .Item("CATNAME").ToString() <> "" Then
                                            g.DrawString(.Item("ITEMNAME").ToString(), fontRegular, BlackBrush, c3, START_POS, LeftFormat)
                                        Else
                                            g.DrawString(.Item("CATNAME").ToString(), fontRegular, BlackBrush, c3, START_POS, LeftFormat)
                                        End If
                                    End If
                                End If

                            End If
                            g.DrawString(.Item("HSN").ToString(), fontRegularsmall, BlackBrush, c2, START_POS, LeftFormat)
                            g.DrawString(IIf(Val(.Item("PURITY").ToString()) > 0, .Item("PURITY").ToString(), ""), fontRegular, BlackBrush, c4, START_POS, rAlign) '' PURITY
                            g.DrawString(IIf(Val(.Item("PCS").ToString()) > 0, .Item("PCS").ToString(), ""), fontRegular, BlackBrush, c5, START_POS, rAlign)
                            'If .Item("RESULT").ToString = "6.0" And .Item("RESULT").ToString = "5.0" And Val(.Item("GRSWT").ToString()) <> 0 And .Item("CATCODE").ToString = "" And .Item("SERNO").ToString = "ZZZZZZZZ" And (.Item("TRANTYPE").ToString = "gms" Or .Item("TRANTYPE").ToString = "cts") Then
                            If .Item("TRANTYPE").ToString = "gms" Or .Item("TRANTYPE").ToString = "cts" Then
                                g.DrawString(IIf(Val(.Item("GRSWT").ToString()) > 0, .Item("GRSWT").ToString(), "") + " " + .Item("TRANTYPE").ToString(), fontRegular, BlackBrush, c6, START_POS, rAlign)
                                g.DrawString(IIf(Val(.Item("NETWT").ToString()) > 0, .Item("NETWT").ToString(), "") + " " + .Item("TRANTYPE").ToString(), fontRegular, BlackBrush, c8, START_POS, rAlign)
                            Else
                                g.DrawString(IIf(Val(.Item("GRSWT").ToString()) > 0, .Item("GRSWT").ToString(), ""), fontRegular, BlackBrush, c6, START_POS, rAlign)
                                g.DrawString(IIf(Val(.Item("NETWT").ToString()) > 0, .Item("NETWT").ToString(), ""), fontRegular, BlackBrush, c8, START_POS, rAlign)
                            End If
                            'g.DrawString(IIf(Val(.Item("RATE").ToString()) > 0, "", ""), fontRegular, BlackBrush, c6, START_POS, rAlign)
                            'g.DrawString(IIf(Val(.Item("DIAWT").ToString()) > 0, .Item("DIAWT").ToString, ""), fontRegular, BlackBrush, c7, START_POS, rAlign)
                            'g.DrawString(IIf(Val(.Item("MC").ToString()) > 0, .Item("MC").ToString(), ""), fontRegular, BlackBrush, c8, START_POS, rAlign)
                            If .Item("RESULT").ToString <> "3.0" And .Item("RESULT").ToString <> "7.0" Then
                                g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) > 0, .Item("AMOUNT").ToString(), ""), fontRegular, BlackBrush, c10, START_POS, rAlign)
                                If .Item("CATNAME").ToString() <> "" Then
                                    START_POS = START_POS + LINE_SPACE
                                End If
                            End If
                            ''_WASTAGE = _WASTAGE + .Item("WASTAGE").ToString()

                            If START_POS >= 390 Then
                                smithfooter_flag = False
                                smithCont_flag = True
                                'FooterSmithIssueReceipt(e.Graphics, e)
                                PagecountSale = PagecountSale + 1
                                START_POS = TSTART_POS ' TOP AGAIN STARTING POSITION
                                g.DrawString("Continue.... ", fontBold, Brushes.Black, c1, SMITH_BOTTOM_POS + 10)
                                e.HasMorePages = True
                                _top_flag = True
                                Return
                                Exit For
                            End If
                        End With
                    Next
                End If
                If START_POS < 390 And _top_flag = False Then
                    _Alloy = Val(dsSmithIssueReceipt.Tables("DTISSREC").Compute("SUM(ALLOY)", "").ToString)
                    If Val(_Alloy) > 0 Then
                        'g.DrawString(.Item("ALLOYNAME").ToString(), fontRegular, BlackBrush, c3, START_POS, LeftFormat)
                        'g.DrawString(.Item("ALLOY").ToString(), fontRegular, BlackBrush, c6, START_POS, rAlign)
                        'g.DrawString(.Item("ALLOY").ToString(), fontRegular, BlackBrush, c8, START_POS, rAlign)
                        g.DrawString("ADD : ALLOY", fontRegular, BlackBrush, c3, START_POS, LeftFormat)
                        g.DrawString(Format(_Alloy, "0.000"), fontRegular, BlackBrush, c6, START_POS, rAlign)
                        g.DrawString(Format(_Alloy, "0.000"), fontRegular, BlackBrush, c8, START_POS, rAlign)
                        START_POS = START_POS + LINE_SPACE
                        If START_POS >= 390 Then
                            smithfooter_flag = False
                            smithCont_flag = True
                            ' FooterSmithIssueReceipt(e.Graphics, e)
                            PagecountSale = PagecountSale + 1
                            START_POS = TSTART_POS ' TOP AGAIN STARTING POSITION
                            g.DrawString("Continue.... ", fontBold, Brushes.Black, c1, SMITH_BOTTOM_POS + 10)
                            e.HasMorePages = True
                            _top_flag = True
                            Return
                        End If
                    End If
                    _WASTAGE_ISS = Val(dsSmithIssueReceipt.Tables("DTISSREC").Compute("SUM(WASTAGE)", "TABLENAME='ISSUE'").ToString)
                    If Val(_WASTAGE_ISS) > 0 Then
                        g.DrawString("WASTAGE", fontRegular, BlackBrush, c3, START_POS, LeftFormat)
                        g.DrawString(Format(_WASTAGE_ISS, "0.000"), fontRegular, BlackBrush, c6, START_POS, rAlign)
                        g.DrawString(Format(_WASTAGE_ISS, "0.000"), fontRegular, BlackBrush, c8, START_POS, rAlign)
                        START_POS = START_POS + LINE_SPACE
                        If START_POS >= 390 Then
                            smithfooter_flag = False
                            smithCont_flag = True
                            ' FooterSmithIssueReceipt(e.Graphics, e)
                            PagecountSale = PagecountSale + 1
                            START_POS = TSTART_POS ' TOP AGAIN STARTING POSITION
                            g.DrawString("Continue.... ", fontBold, Brushes.Black, c1, SMITH_BOTTOM_POS + 10)
                            e.HasMorePages = True
                            _top_flag = True
                            Return
                        End If
                    End If


                    _WASTAGE_REC = Val(dsSmithIssueReceipt.Tables("DTISSREC").Compute("SUM(WASTAGE)", "TABLENAME='RECEIPT'").ToString)
                    If Val(_WASTAGE_REC) > 0 Then
                        g.DrawString("LWASTAGE", fontRegular, BlackBrush, c3, START_POS, LeftFormat)
                        g.DrawString(Format(_WASTAGE_REC, "0.000"), fontRegular, BlackBrush, c6, START_POS, rAlign)
                        g.DrawString(Format(_WASTAGE_REC, "0.000"), fontRegular, BlackBrush, c8, START_POS, rAlign)
                        START_POS = START_POS + LINE_SPACE
                        If START_POS >= 390 Then
                            smithfooter_flag = False
                            smithCont_flag = True
                            ' FooterSmithIssueReceipt(e.Graphics, e)
                            PagecountSale = PagecountSale + 1
                            START_POS = TSTART_POS ' TOP AGAIN STARTING POSITION
                            g.DrawString("Continue.... ", fontBold, Brushes.Black, c1, SMITH_BOTTOM_POS + 10)
                            e.HasMorePages = True
                            _top_flag = True
                            Return
                        End If
                    End If
                End If

                If START_POS >= 390 Then
                    smithfooter_flag = False
                    smithCont_flag = True
                    ' FooterSmithIssueReceipt(e.Graphics, e)
                    PagecountSale = PagecountSale + 1
                    START_POS = TSTART_POS ' TOP AGAIN STARTING POSITION
                    g.DrawString("Continue.... ", fontBold, Brushes.Black, c1, SMITH_BOTTOM_POS + 10)
                    e.HasMorePages = True
                    Return
                End If
                If dsSmithIssueReceipt.Tables("TAXTRAN").Rows.Count > 0 Then
                    For t As Integer = 0 To dsSmithIssueReceipt.Tables("TAXTRAN").Rows.Count - 1
                        With dsSmithIssueReceipt.Tables("TAXTRAN").Rows(t)
                            If .Item("TAXID").ToString() = "CG" Then
                                g.DrawString("CGST " & Format(Val(.Item("TAXPER").ToString()), "0.00") & "%", fontRegular, BlackBrush, c6, START_POS, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegular, BlackBrush, c10, START_POS, rAlign)
                            ElseIf .Item("TAXID").ToString() = "SG" Then
                                g.DrawString("SGST " & Format(Val(.Item("TAXPER").ToString()), "0.00") & "%", fontRegular, BlackBrush, c6, START_POS, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegular, BlackBrush, c10, START_POS, rAlign)
                            ElseIf .Item("TAXID").ToString() = "TC" Then
                                g.DrawString("TCS " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", Format(Val(.Item("TAXPER").ToString()), "0.00")) & "", fontRegular, BlackBrush, c6, START_POS, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegular, BlackBrush, c10, START_POS, rAlign)
                            ElseIf .Item("TAXID").ToString() = "IG" Then
                                g.DrawString("IGST " & Format(Val(.Item("TAXPER").ToString()), "0.00") & "%", fontRegular, BlackBrush, c6, START_POS, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegular, BlackBrush, c10, START_POS, rAlign)
                            ElseIf .Item("TAXID").ToString() = "61" Then
                                g.DrawString("TDS" & Format(Val(.Item("TAXPER").ToString()), "0.00") & "%", fontRegular, BlackBrush, c6, START_POS, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegular, BlackBrush, c10, START_POS, rAlign)
                            End If
                        End With
                        START_POS = START_POS + LINE_SPACE
                    Next
                End If
                If dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows.Count > 0 And _top_flag = False Then
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
                    If chkmeltsummary.Checked = False Then
                        g.DrawString("Remarks : " & Remark1 & " " & Remark2, fontRegular, BlackBrush, c2, 400, LeftFormat) 'Top
                    End If
                    g.DrawLine(Pens.Silver, 20, 420, 775, 420)
                    Dim START_POSTemp As Integer = 425
                    With dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(0)
                        g.DrawString(.Item("TOTAL").ToString(), fontRegular, BlackBrush, c2, START_POSTemp, LeftFormat)
                        g.DrawString(IIf(Val(.Item("PCS").ToString()) > 0, .Item("PCS").ToString(), Format(-1 * Val(.Item("PCS").ToString()), "0.00")), fontRegular, BlackBrush, c5, START_POSTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("GRSWT").ToString()) > 0, .Item("GRSWT").ToString(), Format(-1 * Val(.Item("GRSWT").ToString()), "0.00")), fontRegular, BlackBrush, c6, START_POSTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("NETWT").ToString()) > 0, .Item("NETWT").ToString(), Format(-1 * Val(.Item("NETWT").ToString()), "0.00")), fontRegular, BlackBrush, c8, START_POSTemp, rAlign)
                        'g.DrawString(IIf(Val(.Item("DIAWT").ToString()) > 0, .Item("DIAWT").ToString(), Format(-1 * Val(.Item("DIAWT").ToString()), "0.00")), fontRegular, BlackBrush, c7, START_POSTemp, rAlign)
                        'g.DrawString(IIf(Val(.Item("MC").ToString()) > 0, .Item("MC").ToString(), Format(-1 * Val(.Item("MC").ToString()), "0.00")), fontRegular, BlackBrush, c8, START_POSTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) > 0, .Item("AMOUNT").ToString(), Format(-1 * Val(.Item("AMOUNT").ToString()), "0.00")), fontRegular, BlackBrush, c10, START_POSTemp, rAlign)
                    End With
                End If
                smithfooter_flag = True
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
                g.DrawLine(Pens.Silver, 20, START_POS, 775, START_POS)
                RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
                If dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count > 0 And dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows.Count > 0 Then
                    For PagecountSale = PagecountSale To dsSmithIssueReceipt.Tables("DTISSREC").Rows.Count - 1
                        With dsSmithIssueReceipt.Tables("DTISSREC").Rows(PagecountSale)

                            g.DrawString(IIf(Val(.Item("SNO").ToString()) > 0, .Item("SNO").ToString(), ""), fontRegularsmall, BlackBrush, sm1, START_POS, LeftFormat)
                            g.DrawString(.Item("ITEMNAME").ToString(), fontRegularsmall, BlackBrush, sm2, START_POS, LeftFormat)
                            g.DrawString(.Item("HSN").ToString(), fontRegularsmall, BlackBrush, sm20, START_POS, LeftFormat)
                            g.DrawString(IIf(Val(.Item("TOUCH").ToString()) > 0, .Item("TOUCH").ToString, ""), fontRegularsmall, BlackBrush, sm3, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("GRSWT").ToString()) > 0, .Item("GRSWT").ToString(), ""), fontRegularsmall, BlackBrush, sm4, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("LESSWT").ToString()) > 0, .Item("LESSWT").ToString(), ""), fontRegularsmall, BlackBrush, sm5, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("NETWT").ToString()) > 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, sm6, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("PURITY").ToString()) > 0, .Item("PURITY").ToString(), ""), fontRegularsmall, BlackBrush, sm7, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("PUREWT").ToString()) > 0, .Item("PUREWT").ToString(), ""), fontRegularsmall, BlackBrush, sm8, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("WASTAGE").ToString()) > 0, .Item("WASTAGE").ToString(), ""), fontRegularsmall, BlackBrush, sm9, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("MC").ToString()) > 0, .Item("MC").ToString(), ""), fontRegularsmall, BlackBrush, sm10, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) > 0, .Item("AMOUNT").ToString(), ""), fontRegularsmall, BlackBrush, sm11, START_POS, rAlign)

                            START_POS = START_POS + LINE_SPACE
                            If START_POS >= BOTTOM_POS Then 'NoofPage > 6
                                PagecountSale = PagecountSale + 1
                                START_POS = TSTART_POS '150 ' TOP AGAIN STARTING POSITION
                                g.DrawString("Continue.... ", fontRegularsmall, Brushes.Black, c1, BOTTOM_POS + 10)
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
                                g.DrawString("CGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, START_POS, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, sm11, START_POS, rAlign)
                            ElseIf .Item("TAXID").ToString() = "SG" Then
                                g.DrawString("SGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, START_POS, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, sm11, START_POS, rAlign)
                            ElseIf .Item("TAXID").ToString() = "TC" Then
                                g.DrawString("TCS " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "", fontRegularsmall, BlackBrush, sm8, START_POS, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, sm11, START_POS, rAlign)
                            ElseIf .Item("TAXID").ToString() = "IG" Then
                                g.DrawString("IGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, START_POS, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, sm11, START_POS, rAlign)
                            Else
                                g.DrawString("LESS TDS " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, START_POS, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, sm11, START_POS, rAlign)
                            End If
                        End With
                        START_POS = START_POS + LINE_SPACE
                    Next
                End If

                If dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows.Count > 0 Then
                    Dim Remark1 As String = ""
                    Dim Remark2 As String = ""
                    Remark1 = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("REMARK1").ToString
                    Remark2 = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("REMARK2").ToString
                    g.DrawString(Remark1 & " " & Remark2, fontRegularsmall, BlackBrush, sm2, 400, LeftFormat) 'Top
                    g.DrawLine(Pens.Silver, 20, 420, 775, 420)
                    Dim START_POSTemp As Integer = 425
                    With dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(0)
                        g.DrawString(.Item("TOTAL").ToString(), fontRegularsmall, BlackBrush, sm2, START_POSTemp, LeftFormat)
                        g.DrawString(IIf(Val(.Item("GRSWT").ToString()) > 0, .Item("GRSWT").ToString(), ""), fontRegularsmall, BlackBrush, sm4, START_POSTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("LESSWT").ToString()) > 0, .Item("LESSWT").ToString(), ""), fontRegularsmall, BlackBrush, sm5, START_POSTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("NETWT").ToString()) > 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, sm6, START_POSTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("PUREWT").ToString()) > 0, .Item("PUREWT").ToString(), ""), fontRegularsmall, BlackBrush, sm8, START_POSTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("MC").ToString()) > 0, .Item("MC").ToString(), ""), fontRegularsmall, BlackBrush, sm10, START_POSTemp, rAlign)
                        g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) > 0, .Item("AMOUNT").ToString(), ""), fontRegularsmall, BlackBrush, sm11, START_POSTemp, rAlign)
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
                                 , ByVal CopyName As String, ByVal catNamegroup As String, ByVal _START_POS As Integer)
        g1.DrawLine(Pens.Silver, 20, _START_POS, 775, _START_POS)
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
                FirstY1 = START_POS
                TitleSmithIssueReceiptSmithItemNameA4(e.Graphics, e)
                PrintDocumnt_NewCopyName(g1, e, "")
                PrintDocumnt_smith_DrawLine(g1, e, CopyName, catNamegroup, START_POS)
                RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
                If dsSmithIssueReceipt.Tables(catNamegroup).Rows.Count > 0 And dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows.Count > 0 Then 'DTISSREC
                    For PagecountSale = PagecountSale To dsSmithIssueReceipt.Tables(catNamegroup).Rows.Count - 1 'DTISSREC
                        With dsSmithIssueReceipt.Tables(catNamegroup).Rows(PagecountSale) 'DTISSREC
                            If dtIssRecDistinct.Rows.Count > 1 Then
                                If .Item("TABLENAME").ToString = "ISSUE" And boolPrintSmithA4Issue = True Then
                                    START_POS = START_POS + LINE_SPACE
                                    START_POS = START_POS + LINE_SPACE
                                    boolPrintSmithA4Issue = False
                                    GoTo NextImageIssue
                                End If
                            End If
                            If .Item("SERNO").ToString = "ZZZZZZZZZZZZ" Then
                                wastageValue = Val(dsSmithIssueReceipt.Tables(catNamegroup).Compute("SUM(WASTAGE)", "TABLENAME='RECEIPT'").ToString)
                                If wastageValue > 0 Then
                                    g.DrawString("YIELD / PROCESSING LOSS ", fontRegularsmall, BlackBrush, smI2, START_POS, LeftFormat) 'ADD : WASTAGE
                                    g.DrawString(Format(wastageValue, "0.000"), fontRegularsmall, BlackBrush, smI8, START_POS, rAlign)
                                    START_POS = START_POS + LINE_SPACE
                                    wastageValue2 = 1 * wastageValue
                                End If
                                wastageValue = 0
                            End If
                            g.DrawString(IIf(Val(.Item("SNO").ToString()) > 0, .Item("SNO").ToString(), ""), fontRegularsmall, BlackBrush, smI1, START_POS, LeftFormat)
                            g.DrawString(.Item("ITEMNAME").ToString(), fontRegularsmall, BlackBrush, smI2, START_POS, LeftFormat)
                            g.DrawString(Mid(.Item("HSN").ToString(), 1, 4), fontRegularsmall, BlackBrush, smI20, START_POS, LeftFormat)
                            g.DrawString(IIf(Val(.Item("TOUCH").ToString()) <> 0, .Item("TOUCH").ToString, ""), fontRegularsmall, BlackBrush, smI3, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("PCS").ToString()) <> 0, .Item("PCS").ToString(), ""), fontRegularsmall, BlackBrush, smI21, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontRegularsmall, BlackBrush, smI4, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("LESSWT").ToString()) <> 0, .Item("LESSWT").ToString(), ""), fontRegularsmall, BlackBrush, smI5, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, smI6, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("PURITY").ToString()) <> 0, .Item("PURITY").ToString(), ""), fontRegularsmall, BlackBrush, smI7, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("PUREWT").ToString()) <> 0, .Item("PUREWT").ToString(), ""), fontRegularsmall, BlackBrush, smI8, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("WASTAGE").ToString()) <> 0, .Item("WASTAGE").ToString(), ""), fontRegularsmall, BlackBrush, smI9, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("MC").ToString()) <> 0, .Item("MC").ToString(), ""), fontRegularsmall, BlackBrush, smI10, START_POS, rAlign)
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
                                g.DrawString(printGst, fontRegularsmall, BlackBrush, smI8, START_POS, rAlign)
                                g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) <> 0, .Item("AMOUNT").ToString(), ""), fontRegularsmall, BlackBrush, smI11, START_POS, rAlign)
                            Else
                                g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) <> 0, .Item("AMOUNT").ToString(), ""), fontRegularsmall, BlackBrush, smI11, START_POS, rAlign)
                            End If
                            START_POS = START_POS + LINE_SPACE
                            If START_POS > BOTTOM_POS Then '850
                                START_POS = START_POS + LINE_SPACE
                                PagecountSale = PagecountSale + 1
                                g.DrawString("Continue.... ", fontRegularsmall, Brushes.Black, c1, START_POS) '490
                                START_POS = START_POS + LINE_SPACE
                                PrintDocumnt_smith_DrawLine(g1, e, CopyName, catNamegroup, START_POS)
                                FooterSmithIssueReceiptA4(e.Graphics, e, FirstY1, START_POS)
                                START_POS = START_POSSMITHISSUEVALUE
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
                                g.DrawString("YIELD / PROCESSING LOSS ", fontRegularsmall, BlackBrush, smI2, START_POS, LeftFormat) 'ADD : WASTAGE
                                g.DrawString(Format(wastageValue, "0.000"), fontRegularsmall, BlackBrush, smI8, START_POS, rAlign)
                                START_POS = START_POS + LINE_SPACE
                            End If
                        Else
                            If dsSmithIssueReceipt.Tables(catNamegroup).Rows(0).Item("TTYPE").ToString = "RRE" Then
                                If dsSmithIssueReceipt.Tables(catNamegroup).Rows(dsSmithIssueReceipt.Tables(catNamegroup).Rows.Count - 1).Item("TABLENAME").ToString = "ISSUE" And boolPrintSmithA4Issue = False Then
                                    If dsSmithIssueReceipt.Tables(catNamegroup).Rows.Count = PagecountSale Then
                                        wastageValue = Val(dsSmithIssueReceipt.Tables(catNamegroup).Compute("SUM(WASTAGE)", "TABLENAME='ISSUE'").ToString)
                                        If wastageValue > 0 Then
                                            wastageValue2 = wastageValue
                                            g.DrawString("YIELD / PROCESSING LOSS ", fontRegularsmall, BlackBrush, smI2, START_POS, LeftFormat) 'ADD : WASTAGE
                                            g.DrawString(Format(wastageValue, "0.000"), fontRegularsmall, BlackBrush, smI8, START_POS, rAlign)
                                            START_POS = START_POS + LINE_SPACE
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
                                g.DrawString("CGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, smI8, START_POS, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smI11, START_POS, rAlign)
                            ElseIf .Item("TAXID").ToString() = "SG" Then
                                g.DrawString("SGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, smI8, START_POS, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smI11, START_POS, rAlign)
                            ElseIf .Item("TAXID").ToString() = "TC" Then
                                g.DrawString("TCS " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "", fontRegularsmall, BlackBrush, smI8, START_POS, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smI11, START_POS, rAlign)
                            ElseIf .Item("TAXID").ToString() = "IG" Then
                                g.DrawString("IGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, smI8, START_POS, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smI11, START_POS, rAlign)
                            Else
                                g.DrawString("LESS : TDS " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, smI8, START_POS, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smI11, START_POS, rAlign)
                            End If
                        End With
                        START_POS = START_POS + LINE_SPACE
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
                        START_POS = START_POS + LINE_SPACE
                        Dim Remark1 As String = ""
                        Dim Remark2 As String = ""
                        Remark1 = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("REMARK1").ToString
                        Remark2 = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("REMARK2").ToString
                        g.DrawString(Remark1 & " " & Remark2, fontRegularsmall, BlackBrush, smI2, START_POS, LeftFormat) 'Top
                        Dim WastageValueTotal As Double = 0
                        WastageValueTotal = IIf(Val(dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(Rec).Item("PUREWT").ToString()) > 0, dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(Rec).Item("PUREWT").ToString(), 0) + wastageValue2

                        If dsSmithIssueReceipt.Tables(catNamegroup).Rows(0).Item("TTYPE").ToString = "RPU" Then
                            If dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(Rec).Item("RECPAY").ToString = "R" Then
                                If dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(Rec).Item("RECPAY").ToString = "R" Then
                                    g.DrawString(Format(WastageValueTotal, "0.000"), New Font("Palatino Linotype", 12, FontStyle.Regular), BlackBrush, smI8, START_POS, rAlign) '
                                Else
                                    g.DrawString(Format(WastageValueTotal, "0.000"), New Font("Palatino Linotype", 12, FontStyle.Regular), BlackBrush, smI8, START_POS, rAlign) 'Top'Impact
                                End If
                            Else
                                g.DrawString("-" & Format(WastageValueTotal, "0.000"), New Font("Palatino Linotype", 12, FontStyle.Regular), BlackBrush, smI8, START_POS, rAlign) 'Top'Impact
                            End If
                            START_POS = 980
                        Else
                            START_POS = 980
                            If dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(Rec).Item("RECPAY").ToString = "R" Then 'dsSmithIssueReceipt.Tables(catNamegroup).Rows(Rec).Item("TABLENAME").ToString = "RECEIPT"
                                If dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(Rec).Item("RECPAY").ToString = "R" Then 'dsSmithIssueReceipt.Tables(catNamegroup).Rows(Rec).Item("TABLENAME").ToString = "RPU"
                                    g.DrawString(Format(WastageValueTotal, "0.000"), New Font("Impact", 18, FontStyle.Regular), BlackBrush, smI8, START_POS, rAlign) '
                                Else
                                    g.DrawString(Format(WastageValueTotal, "0.000"), New Font("Impact", 18, FontStyle.Regular), BlackBrush, smI8, START_POS, rAlign) 'Top'Impact
                                End If
                            Else
                                g.DrawString("-" & Format(WastageValueTotal, "0.000"), New Font("Impact", 18, FontStyle.Regular), BlackBrush, smI8, START_POS, rAlign) 'Top'Impact
                            End If
                        End If
                        g.DrawString(CopyName, New Font("Impact", 18, FontStyle.Regular), BlackBrush, smI2, START_POS, LeftFormat) 'Top
                        PrintDocumnt_smith_DrawLine(g1, e, CopyName, catNamegroup, START_POS)
                        With dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(Rec)
                            g.DrawString(.Item("TOTAL").ToString(), fontRegularsmall, BlackBrush, smI2, START_POS, LeftFormat)
                            g.DrawString(IIf(Val(.Item("PCS").ToString()) <> 0, .Item("PCS").ToString(), ""), fontRegularsmall, BlackBrush, smI21, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontRegularsmall, BlackBrush, smI4, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("LESSWT").ToString()) <> 0, .Item("LESSWT").ToString(), ""), fontRegularsmall, BlackBrush, smI5, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, smI6, START_POS, rAlign)
                            g.DrawString(Format(WastageValueTotal, "0.000"), fontRegularsmall, BlackBrush, sm8, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("MC").ToString()) <> 0, .Item("MC").ToString(), ""), fontRegularsmall, BlackBrush, smI10, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) <> 0, .Item("AMOUNT").ToString(), ""), fontRegularsmall, BlackBrush, smI11, START_POS, rAlign)
                        End With
                        START_POS = START_POS + LINE_SPACE
                        PrintDocumnt_smith_DrawLine(g1, e, CopyName, catNamegroup, START_POS)
                        boolPrintSmithA4IssueTax = False
                        Exit For
                    Next
                End If
                FooterSmithIssueReceiptA4(e.Graphics, e, FirstY1, START_POS)
                If boolPrintSmithA4Issue = False Then
                    If PagecountSale <= dsSmithIssueReceipt.Tables(catNamegroup).Rows.Count - 1 Then
                        g.DrawString("Continue.... ", fontRegularsmall, Brushes.Black, c1, START_POS) '490
                        START_POS = START_POS + LINE_SPACE
                        START_POS = START_POSSMITHISSUEVALUE
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
    Dim START_POSSMITHISSUEVALUE As Integer = TSTART_POS
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
                FirstY1 = START_POS
                TitleSmithIssueReceiptSmithCatNameA4(e.Graphics, e)
                PrintDocumnt_NewCopyName(g1, e, "")
                g.DrawLine(Pens.Silver, 20, START_POS, 775, START_POS)
                START_POS = START_POS + LINE_SPACE
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
                                        START_POS = START_POS + LINE_SPACE
                                        START_POS = START_POS + LINE_SPACE
                                        boolPrintSmithA4Issue = False
                                        GoTo NextImageIssue
                                    End If
                                End If
                            End If
                            If .Item("TAXID").ToString <> "" Then
                            Else
                                g.DrawString(IIf(Val(.Item("SNO").ToString()) > 0, .Item("SNO").ToString(), ""), fontRegularsmall, BlackBrush, smc1, START_POS, LeftFormat)
                            End If
                            g.DrawString(.Item("ITEMNAME").ToString(), fontRegularsmall, BlackBrush, smc2, START_POS, LeftFormat)
                            g.DrawString(.Item("HSN").ToString(), fontRegularsmall, BlackBrush, smc3, START_POS, LeftFormat)
                            g.DrawString(IIf(Val(.Item("PCS").ToString()) <> 0, .Item("PCS").ToString(), ""), fontRegularsmall, BlackBrush, smc10, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontRegularsmall, BlackBrush, smc4, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("LESSWT").ToString()) <> 0, .Item("LESSWT").ToString(), ""), fontRegularsmall, BlackBrush, smc5, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, smc6, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("WASTAGE").ToString()) <> 0, .Item("WASTAGE").ToString(), ""), fontRegularsmall, BlackBrush, smc7, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("MC").ToString()) <> 0, .Item("MC").ToString(), ""), fontRegularsmall, BlackBrush, smc8, START_POS, rAlign)
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
                                g.DrawString(printGst, fontRegularsmall, BlackBrush, smc7, START_POS, rAlign)
                                g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) <> 0, .Item("AMOUNT").ToString(), ""), fontRegularsmall, BlackBrush, smc9, START_POS, rAlign)
                            Else
                                g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) <> 0, .Item("AMOUNT").ToString(), ""), fontRegularsmall, BlackBrush, smc9, START_POS, rAlign)
                            End If
                            START_POS = START_POS + LINE_SPACE
                            If START_POS > BOTTOM_POS Then
                                PagecountSale = PagecountSale + 1
                                g.DrawString("Continue.... ", fontRegularsmall, Brushes.Black, c1, BOTTOM_POS + 10)
                                FooterSmithIssueReceiptA4(e.Graphics, e, FirstY1, START_POS)
                                START_POS = START_POSSMITHISSUEVALUE
                                e.HasMorePages = True
                                Return
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
                                g.DrawString("CGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, START_POS, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smc9, START_POS, rAlign)
                            ElseIf .Item("TAXID").ToString() = "SG" Then
                                g.DrawString("SGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, START_POS, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smc9, START_POS, rAlign)
                            ElseIf .Item("TAXID").ToString() = "TC" Then
                                g.DrawString("TCS " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "", fontRegularsmall, BlackBrush, sm8, START_POS, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smc9, START_POS, rAlign)
                            ElseIf .Item("TAXID").ToString() = "IG" Then
                                g.DrawString("IGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, START_POS, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smc9, START_POS, rAlign)
                            Else
                                g.DrawString("LESS : TDS " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, START_POS, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smc9, START_POS, rAlign)
                            End If
                        End With
                        START_POS = START_POS + LINE_SPACE
                    Next
                End If
                If dsSmithIssueReceipt.Tables("TAXTRAN2").Rows.Count > 0 Then
                    g.DrawString("GST Reversal on wastage", fontBold, BlackBrush, sm5, START_POS, rAlign)
                    START_POS = START_POS + LINE_SPACE
                    For t As Integer = 0 To dsSmithIssueReceipt.Tables("TAXTRAN2").Rows.Count - 1
                        With dsSmithIssueReceipt.Tables("TAXTRAN2").Rows(t)
                            If .Item("TAXID").ToString() = "CG" Then
                                g.DrawString("CGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, START_POS, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smc9, START_POS, rAlign)
                            ElseIf .Item("TAXID").ToString() = "SG" Then
                                g.DrawString("SGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, START_POS, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smc9, START_POS, rAlign)
                            ElseIf .Item("TAXID").ToString() = "IG" Then
                                g.DrawString("IGST " & IIf(Val(.Item("TAXPER").ToString()) = 0, "", .Item("TAXPER").ToString()) & "%", fontRegularsmall, BlackBrush, sm8, START_POS, rAlign)
                                g.DrawString(.Item("TAXAMOUNT").ToString(), fontRegularsmall, BlackBrush, smc9, START_POS, rAlign)
                            End If
                        End With
                        START_POS = START_POS + LINE_SPACE
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
                        START_POS = START_POS + LINE_SPACE
                        Dim Remark1 As String = ""
                        Dim Remark2 As String = ""
                        Remark1 = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("REMARK1").ToString
                        Remark2 = dsSmithIssueReceipt.Tables("DTISSREC").Rows(0).Item("REMARK2").ToString
                        g.DrawString(Remark1 & " " & Remark2, fontRegularsmall, BlackBrush, sm2, START_POS, LeftFormat) 'Top
                        START_POS = 980 ' 1027
                        Dim TcsMinTemp As Double = 0
                        If PrintRPU_IPU = True Then
                            TcsMinTemp = Val(dsSmithIssueReceipt.Tables("DTISSREC").Compute("SUM(AMOUNT)", "TAXID='TC'").ToString)
                        End If
                        g.DrawString(CopyName, New Font("Impact", 18, FontStyle.Regular), BlackBrush, sm2, START_POS, LeftFormat) 'Top
                        g.DrawLine(Pens.Silver, 20, START_POS, 775, START_POS)
                        With dsSmithIssueReceipt.Tables("DTISSRECTOTAL").Rows(Rec)
                            g.DrawString(.Item("TOTAL").ToString(), fontRegularsmall, BlackBrush, smc2, START_POS, LeftFormat)
                            g.DrawString(IIf(Val(.Item("PCS").ToString()) <> 0, .Item("PCS").ToString(), ""), fontRegularsmall, BlackBrush, smc10, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontRegularsmall, BlackBrush, smc4, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("LESSWT").ToString()) <> 0, .Item("LESSWT").ToString(), ""), fontRegularsmall, BlackBrush, smc5, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, smc6, START_POS, rAlign)
                            g.DrawString(IIf(Val(.Item("MC").ToString()) <> 0, .Item("MC").ToString(), ""), fontRegularsmall, BlackBrush, smc8, START_POS, rAlign)
                            Dim IssRecAmt As Double = 0
                            IssRecAmt = Val(.Item("AMOUNT").ToString()) - TcsMinTemp
                            g.DrawString(IIf(Val(.Item("AMOUNT").ToString()) <> 0, Format(IssRecAmt, "0.00"), ""), fontRegularsmall, BlackBrush, smc9, START_POS, rAlign)
                        End With
                        START_POS = START_POS + LINE_SPACE
                        g.DrawLine(Pens.Silver, 20, START_POS, 775, START_POS)
                        boolPrintSmithA4IssueTax = False
                        Exit For
                    Next
                End If
                FooterSmithIssueReceiptA4(e.Graphics, e, FirstY1, START_POS)
                If boolPrintSmithA4Issue = False Then
                    If PagecountSale <= dsSmithIssueReceipt.Tables(catNamegroup).Rows.Count - 1 Then
                        START_POS = START_POSSMITHISSUEVALUE
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

    Private Sub PrintPurSmall_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrtPurSmall.PrintPage
        Using g As Graphics = e.Graphics
            Dim dtBillPrint As New DataTable
            Dim brush As New SolidBrush(Color.Black)
            Dim pen As New Pen(brush)
            Dim fontRegular As New Font("Times New Roman", 9, FontStyle.Regular)
            Dim fontBold As New Font("Times New Roman", 7.5, FontStyle.Bold)
            Dim fontBold9 As New Font("Times New Roman", 9, FontStyle.Bold)
            Dim fontProduct As New Font("Times New Roman", 9, FontStyle.Regular)
            Dim fontstProduct As New Font("Times New Roman", 7, FontStyle.Bold)
            Dim rAlign As New StringFormat
            Dim lAlign As New StringFormat
            rAlign.Alignment = StringAlignment.Far
            lAlign.Alignment = StringAlignment.Near
            Dim t1 As Integer = 10
            Dim c1 As Integer = 50
            Dim c2 As Integer = 100
            Dim c3 As Integer = 145
            Dim c4 As Integer = 190
            Dim c5 As Integer = 230
            Dim c6 As Integer = 285
            Dim y1 As Integer = 0
            Dim _strDesc As String = ""
            Dim strTranno As String = ""
            Dim strTot As String = ""
            Dim dtTot As New DataTable
            If pBatchno <> "" Then

                If chkpartlysale.Checked And _smallprint_trantype = "SA" Then
                    strsql = " SELECT "
                    strsql += vbCrLf + " (SUM(T.TAGGRSWT) - SUM(T.GRSWT)) BALGRSWT,(SUM(T.TAGNETWT) - SUM(T.NETWT)) BALNETWT,"
                    strsql += vbCrLf + " T.TRANTYPE,EMPID,(SELECT TOP 1 EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE T.EMPID=EMPID)EMPNAME, USERID,SNO"
                    strsql += vbCrLf + " ,(SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)ITEMNAME"
                    strsql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS T "
                    strsql += vbCrLf + " WHERE "
                    strsql += vbCrLf + " TRANDATE='" & strBilldate & "' AND BATCHNO='" & pBatchno & "' AND TRANTYPE IN ('SA')  AND  TAGGRSWT > 0"
                    strsql += vbCrLf + "  GROUP BY BATCHNO,EMPID,USERID,TRANTYPE,ITEMID,SNO  HAVING (SUM(T.TAGGRSWT) - SUM(T.GRSWT))>0 "
                    dtBillPrint = New DataTable
                    cmd = New OleDbCommand(strsql, cn)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dtBillPrint)
                    If dtBillPrint.Rows.Count = 0 Then Exit Sub
                    y1 = y1 + 10
                    strsql = " SELECT TOP 1 SUBSTRING(CONVERT(VARCHAR,UPTIME,109),13,5)+SUBSTRING(CONVERT(VARCHAR,UPTIME,109),25,5) AS BILLTIME, "
                    strsql += vbCrLf + " CONVERT(VARCHAR,TRANNO) AS TRANNO,CONVERT(VARCHAR(12),TRANDATE,103) AS TRANDATE,T.TRANTYPE "
                    strsql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS T WHERE "
                    strsql += vbCrLf + " TRANDATE='" & strBilldate & "' AND  BATCHNO='" & pBatchno & "' AND TRANTYPE IN ('SA') "
                    cmd = New OleDbCommand(strsql, cn)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dtTot)
                    If dtTot.Rows.Count > 0 Then
                        strTot = "Date :" + dtTot.Rows(0).Item("TRANDATE").ToString + "  " + dtTot.Rows(0).Item("BILLTIME").ToString
                        g.DrawString(strTot.ToString(), fontRegular, brush, c3, y1)
                        strTot = "Bill No :" + dtTot.Rows(0).Item("TRANNO").ToString
                        g.DrawString(strTot, fontRegular, brush, t1, y1, lAlign)
                    End If
                ElseIf chknontag.Checked = True And _smallprint_trantype = "NT" Then
                    strsql = " SELECT "
                    strsql += vbCrLf + " (SELECT TOP 1 CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=T.CATCODE)CATNAME,"
                    strsql += vbCrLf + " T.GRSWT,T.NETWT,AMOUNT,"
                    strsql += vbCrLf + "  EMPID,(SELECT TOP 1 EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE T.EMPID=EMPID)EMPNAME, userId, SNO"
                    strsql += vbCrLf + " ,T.TRANTYPE"
                    strsql += vbCrLf + " ,(SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)ITEMNAME,(SELECT TOP 1 ISNULL(DIASTNTYPE,'Z') FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=T.CATCODE ) S_TYPE"
                    strsql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS T "
                    strsql += vbCrLf + " WHERE "
                    strsql += vbCrLf + " TRANDATE='" & strBilldate & "' AND BATCHNO='" & pBatchno & "' AND TRANTYPE IN ('SA') AND  ISNULL(TAGNO,'') = '' "
                    dtBillPrint = New DataTable
                    cmd = New OleDbCommand(strsql, cn)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dtBillPrint)
                    If dtBillPrint.Rows.Count = 0 Then Exit Sub
                    y1 = y1 + 10
                    strsql = " SELECT TOP 1 SUBSTRING(CONVERT(VARCHAR,UPTIME,109),13,5)+SUBSTRING(CONVERT(VARCHAR,UPTIME,109),25,5) AS BILLTIME, "
                    strsql += vbCrLf + " CONVERT(VARCHAR,TRANNO) AS TRANNO,CONVERT(VARCHAR(12),TRANDATE,103) AS TRANDATE,T.TRANTYPE "
                    strsql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS T WHERE "
                    strsql += vbCrLf + " TRANDATE='" & strBilldate & "' AND  BATCHNO='" & pBatchno & "' AND TRANTYPE IN ('SA') "
                    cmd = New OleDbCommand(strsql, cn)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dtTot)
                    If dtTot.Rows.Count > 0 Then
                        strTot = "Date :" + dtTot.Rows(0).Item("TRANDATE").ToString + "  " + dtTot.Rows(0).Item("BILLTIME").ToString
                        g.DrawString(strTot.ToString(), fontRegular, brush, c3, y1)
                        strTot = "Bill No :" + dtTot.Rows(0).Item("TRANNO").ToString
                        g.DrawString(strTot, fontRegular, brush, t1, y1, lAlign)
                    End If
                Else
                    strsql = " SELECT "
                    strsql += vbCrLf + " (SELECT TOP 1 CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=T.CATCODE)CATNAME,"
                    strsql += vbCrLf + " T.GRSWT,T.DUSTWT,CONVERT(NUMERIC(15,2),T.PURITY)PURITY,T.NETWT,CONVERT(NUMERIC(15,0),RATE)RATE,AMOUNT,T.WASTAGE,MCHARGE,"
                    strsql += vbCrLf + "  EMPID,(SELECT TOP 1 EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE T.EMPID=EMPID)EMPNAME, userId, SNO, PUREXCH, T.WASTPER"
                    strsql += vbCrLf + " ,T.OTHERAMT,T.TRANTYPE"
                    strsql += vbCrLf + " ,(SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID)ITEMNAME,(SELECT TOP 1 ISNULL(DIASTNTYPE,'Z') FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=T.CATCODE ) S_TYPE"
                    strsql += vbCrLf + " ,ISNULL(REMARK1,'') REMARK1,ISNULL(REMARK2,'') REMARK2 "
                    strsql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS T "
                    strsql += vbCrLf + " WHERE "
                    strsql += vbCrLf + " TRANDATE='" & strBilldate & "' AND BATCHNO='" & pBatchno & "' AND TRANTYPE IN ('" & _smallprint_trantype & "') "
                    dtBillPrint = New DataTable
                    cmd = New OleDbCommand(strsql, cn)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dtBillPrint)
                    If dtBillPrint.Rows.Count = 0 Then Exit Sub
                    y1 = y1 + 10
                    strsql = " SELECT TOP 1 SUBSTRING(CONVERT(VARCHAR,UPTIME,109),13,5)+SUBSTRING(CONVERT(VARCHAR,UPTIME,109),25,5) AS BILLTIME, "
                    strsql += vbCrLf + " CONVERT(VARCHAR,TRANNO) AS TRANNO,CONVERT(VARCHAR(12),TRANDATE,103) AS TRANDATE,T.TRANTYPE "
                    strsql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS T WHERE "
                    strsql += vbCrLf + " TRANDATE='" & strBilldate & "' AND  BATCHNO='" & pBatchno & "' AND TRANTYPE IN ('" & _smallprint_trantype & "') "
                    cmd = New OleDbCommand(strsql, cn)
                    da = New OleDbDataAdapter(cmd)
                    da.Fill(dtTot)
                    If dtTot.Rows.Count > 0 Then
                        'y1 = y1 + 20
                        strTot = "Date :" + dtTot.Rows(0).Item("TRANDATE").ToString + "  " + dtTot.Rows(0).Item("BILLTIME").ToString
                        g.DrawString(strTot.ToString(), fontRegular, brush, c3, y1)
                        strTot = "Bill No :" + dtTot.Rows(0).Item("TRANNO").ToString
                        g.DrawString(strTot, fontRegular, brush, t1, y1, lAlign)
                    End If
                End If
                y1 = y1 + 10
                If _SILVERBILLPRINT <> "" Then
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
                Else
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
                End If
                g.DrawString(strTot, fontRegular, brush, t1, y1, lAlign)
                y1 = y1 + 20
                g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                y1 = y1 + 10
                If dtBillPrint.Rows(0).Item("TRANTYPE").ToString = "SR" Then
                    g.DrawString("Description.", fontBold9, brush, t1, y1, lAlign)
                    g.DrawString("GWT", fontBold9, brush, c5, y1, rAlign)
                    g.DrawString("NWT", fontBold9, brush, c6, y1, rAlign)
                    y1 = y1 + 20
                    g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                    y1 = y1 + 20
                    Dim dtPuStone As New DataTable
                    Dim dtPuStone_DIA As New DataTable
                    For i As Integer = 0 To dtBillPrint.Rows.Count - 1
                        With dtBillPrint.Rows(i)
                            g.DrawString(.Item("ITEMNAME").ToString(), fontRegular, brush, t1, y1, lAlign)
                            g.DrawString(Format(Val(.Item("GRSWT").ToString()), "0.000"), fontProduct, brush, c5, y1, rAlign)
                            g.DrawString(Format(Val(.Item("NETWT").ToString()), "0.000"), fontRegular, brush, c6, y1, rAlign)
                            y1 = y1 + 25
                            strsql = vbCrLf + " SELECT "
                            strsql += vbCrLf + " (SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.STNITEMID) ITEMNAME,(SELECT TOP 1 DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.STNITEMID ) S_TYPE,"
                            strsql += vbCrLf + " (Select TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=T.STNSUBITEMID) SUBITEMNAME,"
                            strsql += vbCrLf + " STNPCS,STNWT,STNRATE,STNAMT,STONEUNIT FROM " & cnStockDb & "..RECEIPTSTONE As T WHERE ISSSNO = '" & dtBillPrint.Rows(i).Item("SNO").ToString() & "' "
                            cmd = New OleDbCommand(strsql, cn)
                            da = New OleDbDataAdapter(cmd)
                            dtPuStone = New DataTable
                            da.Fill(dtPuStone)
                            strsql = vbCrLf + " SELECT "
                            strsql += vbCrLf + " (SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.STNITEMID) ITEMNAME,(SELECT TOP 1 DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.STNITEMID ) S_TYPE,"
                            strsql += vbCrLf + " (Select TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=T.STNSUBITEMID) SUBITEMNAME,"
                            strsql += vbCrLf + " STNPCS,STNWT,STNRATE,STNAMT,STONEUNIT FROM " & cnStockDb & "..RECEIPTSTONE As T WHERE ISSSNO = '" & dtBillPrint.Rows(i).Item("SNO").ToString() & "' "
                            cmd = New OleDbCommand(strsql, cn)
                            da = New OleDbDataAdapter(cmd)
                            da.Fill(dtPuStone_DIA)
                            For K As Integer = 0 To dtPuStone.Rows.Count - 1
                                g.DrawString(dtPuStone.Rows(K).Item("SUBITEMNAME").ToString(), fontProduct, brush, t1, y1, lAlign)

                                If dtPuStone.Rows(K).Item("S_TYPE").ToString = "D" Then
                                    g.DrawString("Dia Pcs : " & Val(dtPuStone.Rows(K).Item("STNPCS").ToString()), fontProduct, brush, c3, y1, lAlign)
                                    y1 = y1 + 15
                                    g.DrawString("Dia Wt  : " & Format(Val(dtPuStone.Rows(K).Item("STNWT").ToString()), "0.00").ToString & " ct", fontProduct, brush, c3, y1, lAlign)
                                Else
                                    g.DrawString("Stone Pcs : " & Val(dtPuStone.Rows(K).Item("STNPCS").ToString()), fontProduct, brush, c3, y1, lAlign)
                                    y1 = y1 + 15
                                    g.DrawString("Stone Wt  : " & Format(Val(dtPuStone.Rows(K).Item("STNWT").ToString()), "0.00").ToString & " gm", fontProduct, brush, c3, y1, lAlign)
                                End If
                                y1 = y1 + 15

                            Next
                        End With
                    Next
                    y1 = y1 + 20
                    g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                    y1 = y1 + 25
                    g.DrawString("SalesReturn :", fontProduct, brush, t1, y1, lAlign)
                    y1 = y1 + 20
                    g.DrawString("Gwt      : -" & Val(dtBillPrint.Compute("SUM(GRSWT)", Nothing).ToString), fontProduct, brush, t1, y1, lAlign)
                    g.DrawString("Nwt      : -" & Val(dtBillPrint.Compute("SUM(NETWT)", Nothing).ToString), fontProduct, brush, c3, y1, lAlign)
                    y1 = y1 + 25
                    g.DrawString("Dia Pcs : " & Val(dtPuStone_DIA.Compute("SUM(STNPCS)", "S_TYPE='D'").ToString), fontProduct, brush, c3, y1, lAlign)
                    y1 = y1 + 25
                    g.DrawString("Dia Wt  : " & Val(dtPuStone_DIA.Compute("SUM(STNWT)", "S_TYPE='D'").ToString), fontProduct, brush, c3, y1, lAlign)
                    If dtBillPrint.Rows(0)("REMARK1").ToString <> " " Then
                        y1 = y1 + 25
                        g.DrawString("[REASON :" + dtBillPrint.Rows(0)("REMARK1").ToString + "/" + dtBillPrint.Rows(0)("REMARK2").ToString + "]", fontRegular, brush, t1, y1, lAlign)
                    End If
                    y1 = y1 + 25
                    g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                    y1 = y1 + 25
                    g.DrawString("[SalesPerson :" + dtBillPrint.Rows(0)("EMPID").ToString + "/" + Mid(dtBillPrint.Rows(0)("EMPNAME").ToString, 1, 3) + "]", fontRegular, brush, t1, y1, lAlign)
                ElseIf dtBillPrint.Rows(0).Item("TRANTYPE").ToString = "PU" Then
                    g.DrawString("GR. WT.", fontBold, brush, c1, y1, rAlign)
                    g.DrawString("ST/DUST", fontBold, brush, c2, y1, rAlign)
                    g.DrawString("PURITY", fontBold, brush, c3, y1, rAlign)
                    g.DrawString("NETWT.", fontBold, brush, c4, y1, rAlign)
                    g.DrawString("RATE", fontBold, brush, c5, y1, rAlign)
                    g.DrawString(" AMOUNT", fontBold, brush, c6, y1, rAlign)
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
                    Type = ""
                    For i As Integer = 0 To dtBillPrint.Rows.Count - 1
                        With dtBillPrint.Rows(i)
                            g.DrawString(.Item("CATNAME").ToString(), fontRegular, brush, t1, y1, lAlign)
                            y1 = y1 + 25
                            g.DrawString(Format(Val(.Item("GRSWT").ToString()), "0.000"), fontProduct, brush, c1, y1, rAlign)
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
                                    End If
                                End If
                                g.DrawString("+" & Format(Val(.Item("WASTAGE").ToString()), "0.000"), fontRegular, brush, c4, y1, rAlign)
                                y1 = y1 + 15
                                g.DrawString("+" & Format(Val(.Item("OTHERAMT").ToString()), "0.00"), fontRegular, brush, c4, y1, rAlign)
                                y1 = y1 - 15
                            Else
                                g.DrawString(Format(Val(.Item("DUSTWT").ToString()), "0.000"), fontRegular, brush, c2, y1, rAlign)
                                g.DrawString(Format(Val(.Item("PURITY").ToString()), "0.00"), fontRegular, brush, c3, y1, rAlign)
                                If Val(.Item("OTHERAMT").ToString) > 0 Then
                                    y1 = y1 + 15
                                    g.DrawString("+" & Format(Val(.Item("OTHERAMT").ToString()), "0.00"), fontRegular, brush, c4, y1, rAlign)
                                    y1 = y1 - 15
                                End If
                            End If
                            g.DrawString(Format(Val(.Item("NETWT").ToString()), "0.000"), fontRegular, brush, c4, y1, rAlign)
                            g.DrawString(Format(Val(.Item("RATE").ToString()), "0"), fontRegular, brush, c5, y1, rAlign)
                            g.DrawString(Format(Val(.Item("AMOUNT").ToString()), "0"), fontRegular, brush, c6, y1, rAlign)
                            y1 = y1 + 25
                            Dim dtPuStone As New DataTable
                            strsql = vbCrLf + " SELECT "
                            strsql += vbCrLf + " (SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.STNITEMID)  ITEMNAME,(SELECT TOP 1 DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.STNITEMID ) S_TYPE,"
                            strsql += vbCrLf + "(Select TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=T.STNSUBITEMID) SUBITEMNAME,"
                            strsql += vbCrLf + " STNWT,STNRATE,STNAMT,STONEUNIT FROM " & cnStockDb & "..RECEIPTSTONE As T WHERE ISSSNO = '" & .Item("SNO").ToString() & "' "
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
                    y1 = y1 + 10
                    g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                    y1 = y1 + 10
                    g.DrawString(dtBillPrint.Compute("SUM(GRSWT)", "S_TYPE NOT IN ('D','T')").ToString, fontBold, brush, c1, y1, rAlign)
                    g.DrawString(dtBillPrint.Compute("SUM(DUSTWT) ", "").ToString, fontBold, brush, c2, y1, rAlign)
                    g.DrawString(dtBillPrint.Compute("SUM(NETWT)", "S_TYPE NOT IN ('D','T')").ToString, fontBold, brush, c4, y1, rAlign)
                    g.DrawString(Format(Val(dtBillPrint.Compute("SUM(AMOUNT)", "").ToString()), "0"), fontBold, brush, c6, y1, rAlign)
                    y1 = y1 + 20
                    g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                    y1 = y1 + 30
                    If dtBillPrint.Rows.Count > 0 Then
                        g.DrawString("[EMPID - " + dtBillPrint.Rows(0)("EMPID").ToString + "/" + Mid(dtBillPrint.Rows(0)("EMPNAME").ToString, 1, 3) + "]" _
                        , fontRegular, brush, t1, y1, lAlign)
                    Else
                        g.DrawString("[Bill - " + EstNo_PU + "]", fontRegular, brush, t1, y1, lAlign)
                    End If
                    y1 = y1 + 10
                ElseIf dtBillPrint.Rows(0).Item("TRANTYPE").ToString = "SA" And chknontag.Checked = True Then
                    Dim dtPuStone As New DataTable
                    g.DrawString("Description.", fontBold9, brush, t1, y1, lAlign)
                    g.DrawString("GRSWT", fontBold9, brush, c4, y1, rAlign)
                    g.DrawString("NETWT", fontBold9, brush, c6, y1, rAlign)
                    y1 = y1 + 20
                    g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                    y1 = y1 + 20
                    For i As Integer = 0 To dtBillPrint.Rows.Count - 1
                        With dtBillPrint.Rows(i)
                            g.DrawString(.Item("ITEMNAME").ToString(), fontRegular, brush, t1, y1, lAlign)
                            g.DrawString(Format(Val(.Item("GRSWT").ToString()), "0.000"), fontProduct, brush, c4, y1, rAlign)
                            g.DrawString(Format(Val(.Item("NETWT").ToString()), "0.000"), fontRegular, brush, c6, y1, rAlign)
                            y1 = y1 + 25
                        End With
                    Next
                    y1 = y1 + 20
                    g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                    y1 = y1 + 25
                    g.DrawString("Non Tag Item :", fontProduct, brush, t1, y1, lAlign)
                    y1 = y1 + 20
                    g.DrawString("Gwt      : " & Val(dtBillPrint.Compute("SUM(GRSWT)", Nothing).ToString), fontProduct, brush, t1, y1, lAlign)
                    g.DrawString("Nwt      : " & Val(dtBillPrint.Compute("SUM(NETWT)", Nothing).ToString), fontProduct, brush, c3, y1, lAlign)
                    y1 = y1 + 25
                    g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                    y1 = y1 + 25
                    g.DrawString("[SalesPerson :" + dtBillPrint.Rows(0)("EMPID").ToString + "/" + Mid(dtBillPrint.Rows(0)("EMPNAME").ToString, 1, 3) + "]", fontRegular, brush, t1, y1, lAlign)
                ElseIf dtBillPrint.Rows(0).Item("TRANTYPE").ToString = "SA" And chkpartlysale.Checked = True Then
                    Dim dtPuStone As New DataTable
                    g.DrawString("Description.", fontBold9, brush, t1, y1, lAlign)
                    g.DrawString("BalGWT", fontBold9, brush, c4, y1, rAlign)
                    g.DrawString("BalNWT", fontBold9, brush, c6, y1, rAlign)
                    y1 = y1 + 20
                    g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                    y1 = y1 + 20
                    Dim dtsaStone As New DataTable
                    For i As Integer = 0 To dtBillPrint.Rows.Count - 1
                        With dtBillPrint.Rows(i)
                            g.DrawString(.Item("ITEMNAME").ToString(), fontRegular, brush, t1, y1, lAlign)
                            g.DrawString(Format(Val(.Item("BalGRSWT").ToString()), "0.000"), fontProduct, brush, c4, y1, rAlign)
                            g.DrawString(Format(Val(.Item("BalNETWT").ToString()), "0.000"), fontRegular, brush, c6, y1, rAlign)
                            y1 = y1 + 25

                            strsql = vbCrLf + " SELECT "
                            strsql += vbCrLf + " (SELECT TOP 1 ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.STNITEMID)  ITEMNAME,(SELECT TOP 1 DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.STNITEMID ) S_TYPE,"
                            strsql += vbCrLf + "(SELECT TOP 1 SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=T.STNSUBITEMID) SUBITEMNAME,"
                            strsql += vbCrLf + "  SUM(TAGSTNWT-STNWT) STNWT,(sum(TAGSTNPCS)-SUM(STNPCS))STNPCS,STNRATE,STNAMT,STONEUNIT FROM " & cnStockDb & "..ISSSTONE As T WHERE ISSSNO = '" & .Item("SNO").ToString() & "'  GROUP BY BATCHNO,SNO,STNWT,STNRATE,STNAMT,STONEUNIT,STNITEMID,STNSUBITEMID  HAVING SUM(TAGSTNWT-STNWT) <> 0 "
                            cmd = New OleDbCommand(strsql, cn)
                            da = New OleDbDataAdapter(cmd)
                            da.Fill(dtsaStone)
                            For K As Integer = 0 To dtsaStone.Rows.Count - 1
                                g.DrawString(dtsaStone.Rows(K).Item("SUBITEMNAME").ToString(), fontRegular, brush, t1, y1, lAlign)
                                If dtsaStone.Rows(K).Item("S_TYPE").ToString = "D" Then
                                    g.DrawString(Format(Val(dtsaStone.Rows(K).Item("STNWT").ToString()), "0.00").ToString & " ct", fontProduct, brush, c4, y1, rAlign)
                                Else
                                    g.DrawString(Format(Val(dtsaStone.Rows(K).Item("STNWT").ToString()), "0.00").ToString & " gm", fontProduct, brush, c4, y1, rAlign)
                                End If
                                y1 = y1 + 25
                            Next
                        End With
                    Next
                    y1 = y1 + 20
                    g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                    y1 = y1 + 25
                    g.DrawString("Total Less Cutting :", fontProduct, brush, t1, y1, lAlign)
                    y1 = y1 + 20
                    g.DrawString("Gwt      : " & Val(dtBillPrint.Compute("SUM(BALGRSWT)", Nothing).ToString), fontProduct, brush, t1, y1, lAlign)
                    g.DrawString("Nwt      : " & Val(dtBillPrint.Compute("SUM(BALNETWT)", Nothing).ToString), fontProduct, brush, c3, y1, lAlign)

                    y1 = y1 + 25
                    If dtsaStone.Rows.Count > 0 Then
                        g.DrawString("Total Stn & Dia Cutting:", fontProduct, brush, t1, y1, lAlign)
                        y1 = y1 + 20
                        g.DrawString("Wt      : " & Val(dtsaStone.Compute("SUM(STNWT)", Nothing).ToString), fontProduct, brush, t1, y1, lAlign)
                        g.DrawString("Pcs      : " & Val(dtsaStone.Compute("SUM(STNPCS)", Nothing).ToString), fontProduct, brush, c3, y1, lAlign)
                        y1 = y1 + 25
                    End If
                    g.DrawLine(Pens.Black, 10.0F, y1, 300.0F, y1)
                    y1 = y1 + 25
                    g.DrawString("[SalesPerson :" + dtBillPrint.Rows(0)("EMPID").ToString + "/" + Mid(dtBillPrint.Rows(0)("EMPNAME").ToString, 1, 3) + "]", fontRegular, brush, t1, y1, lAlign)
                End If
            End If
        End Using
    End Sub

    Private Sub PrtAppDoc_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrtAppDoc.PrintPage
        Try
            Dim NoofPage As Integer = 0
            Dim subputitle As Boolean = False
            Dim _dt_count As Integer = 0
            Dim rAlign As New StringFormat
            rAlign.Alignment = StringAlignment.Far
            NoofPrint += 1
            Using g As Graphics = e.Graphics
                CustomerInfoApproval(Tranno, prtBilldate, e.Graphics, e)
                If dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("AI") Then TitleApproval(e.Graphics, e)
                RightFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft
                _dt_count = dtSales.Rows.Count
                If dtSales.Rows.Count > 0 And Appsummary = False Then
                    For PagecountSale = PagecountSale To dtSales.Rows.Count - 1
                        If Not dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("AR") _
                            Then
                            With dtSales.Rows(PagecountSale)
                                ''Top '/************************************////
                                NoofPage += 1
                                If .Item("RESULT").ToString = "3.5" Then
                                    g.DrawLine(Pens.Silver, 710, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.0" And .Item("TYPE").ToString = "11.0" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawLine(Pens.Silver, 340, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.4" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.6" And .Item("TYPE").ToString = "12" Then
                                    g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "2.0" And .Item("COLHEAD").ToString = "S" And .Item("TYPE").ToString = "1" Then
                                    START_POS = START_POS - LINE_SPACE
                                    START_POS = START_POS - LINE_SPACE
                                    START_POS = START_POS + 13
                                End If
                                g.DrawString(.Item("SRNO").ToString(), fontRegular, BlackBrush, c1, START_POS) 'LeftFormat
                                If .Item("TYPE").ToString = "10" And .Item("COLHEAD").ToString = "G" Then
                                    If .Item("ITEMNAME").ToString.Length > 25 And .Item("RESULT").ToString <> "3.3" Then
                                        g.DrawString(.Item("ITEMNAME").ToString(), fontBoldsmall, BlackBrush, c3 - 20, START_POS, LeftFormat)
                                    ElseIf .Item("RESULT").ToString = "3.3" Then
                                        g.DrawString(.Item("ITEMNAME").ToString(), fontRegularsmall, BlackBrush, c3, START_POS, LeftFormat)
                                    Else
                                        g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c3, START_POS, LeftFormat)
                                    End If
                                Else
                                    If .Item("CALTYPE").ToString = "HA" And .Item("RESULT").ToString = "1.1" Then
                                        g.DrawString(.Item("ITEMNAME").ToString(), fontRegular6, BlackBrush, c3, START_POS, LeftFormat)
                                    ElseIf .Item("RESULT").ToString = "2.1" And .Item("TYPE").ToString = "1" And (.Item("COLHEAD").ToString = "G" Or .Item("COLHEAD").ToString = "S") Then
                                        g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c3, START_POS, LeftFormat)
                                    Else
                                        g.DrawString(.Item("TAGNO").ToString(), fontRegularsmall, BlackBrush, c2 - 5, START_POS, LeftFormat)
                                        g.DrawString(.Item("ITEMNAME").ToString(), fontRegularsmall, BlackBrush, c3, START_POS, LeftFormat)
                                    End If
                                End If
                                If .Item("RESULT").ToString = "3.6" And .Item("TYPE").ToString = "11" Then
                                    g.DrawString(.Item("HSN").ToString(), fontRegularsmall, BlackBrush, c2, START_POS, LeftFormat) 'fontBold
                                    g.DrawString(.Item("QTY").ToString(), fontBold, BlackBrush, c4, START_POS, rAlign)
                                ElseIf .Item("RESULT").ToString = "3.4" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawString(.Item("HSN").ToString(), fontAmtInWord, BlackBrush, c2, START_POS, LeftFormat)
                                    g.DrawString(.Item("QTY").ToString(), fontBold, BlackBrush, c4, START_POS, RightFormat)
                                Else
                                    If .Item("RESULT").ToString = "2.1" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" Then
                                        g.DrawString(.Item("QTY").ToString(), fontBold, BlackBrush, c4, START_POS, RightFormat)
                                    Else
                                        g.DrawString(IIf(Val(.Item("QTY").ToString()) <> 0, Val(.Item("QTY").ToString()), ""), fontRegularsmall, BlackBrush, c4, START_POS, RightFormat)
                                    End If
                                End If
                                If .Item("CALTYPE").ToString = "F" Then 'Fixed Item Should Not print GrossWt '' CHANGED 28-03-2022 
                                    g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontRegularsmall, BlackBrush, c5 + 100, START_POS, RightFormat)
                                    g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, c6 + 100, START_POS, RightFormat)
                                ElseIf .Item("RESULT").ToString = "2.1" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontBold, BlackBrush, c5 + 100, START_POS, RightFormat)
                                    g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontBold, BlackBrush, c6 + 100, START_POS, RightFormat)
                                Else
                                    g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontRegularsmall, BlackBrush, c5 + 100, START_POS, RightFormat)
                                    g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, c6 + 100, START_POS, RightFormat)
                                End If
                                If .Item("COLHEAD").ToString() = "G" Or .Item("COLHEAD").ToString = "P" Or .Item("COLHEAD").ToString = "V" Then
                                    If .Item("COLHEAD").ToString = "G" And (.Item("TYPE").ToString = "1" Or .Item("TYPE").ToString = "3") Then
                                        g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, START_POS, LeftFormat)
                                    ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString <> "10" Then
                                        g.DrawString(.Item("RATE").ToString(), fontRegularsmall, BlackBrush, 550, START_POS, LeftFormat)
                                    ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString <> "3.50") Then
                                        g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, START_POS, LeftFormat)
                                    ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString = "3.50") Then
                                        g.DrawString(.Item("RATE").ToString(), fontRegularsmall, BlackBrush, 550, START_POS, LeftFormat)
                                    ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10" Then
                                        g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, START_POS, LeftFormat)
                                    ElseIf .Item("COLHEAD").ToString = "V" And .Item("TYPE").ToString = "1" Then
                                        g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, START_POS, LeftFormat)
                                    ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "1" Then
                                        g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, START_POS, LeftFormat)
                                    End If
                                Else
                                End If
                                If .Item("CALTYPE").ToString = "F" Then 'Fixed Item Should Not print mC
                                    g.DrawString("", fontRegular, BlackBrush, c8, START_POS, RightFormat)
                                Else
                                    g.DrawString("", fontRegular, BlackBrush, c8, START_POS, RightFormat)
                                End If
                                If .Item("COLHEAD").ToString = "G" Or .Item("COLHEAD").ToString = "P" Or .Item("COLHEAD").ToString = "V" Then
                                    If .Item("RESULT").ToString = "2.1" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" Then
                                        g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                    ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString <> "10" Then
                                        g.DrawString(.Item("AMOUNT").ToString(), fontRegularsmall, BlackBrush, c10, START_POS, rAlign)
                                    ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString <> "3.50") Or (.Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10") Then
                                        g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                        If .Item("RESULT").ToString = "3.6" And .Item("TYPE").ToString = "11" Then
                                        End If
                                    ElseIf (.Item("RESULT").ToString = "3.4" Or .Item("RESULT").ToString = "3.5") And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" Then
                                        g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                    ElseIf .Item("COLHEAD").ToString = "V" And .Item("TYPE").ToString = "1" Then
                                        g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                    ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString = "3.50") Then
                                        g.DrawString(.Item("AMOUNT").ToString(), fontRegularsmall, BlackBrush, c10, START_POS, rAlign)
                                    ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10" Then
                                        g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                    End If
                                Else
                                    g.DrawString(.Item("AMOUNT").ToString(), fontRegularsmall, BlackBrush, c10, START_POS, rAlign)
                                End If
                                If (.Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" And .Item("RESULT").ToString <> "3.5") _
                                Or (.Item("TYPE").ToString = "2.1" And .Item("COLHEAD").ToString = "G") _
                                Or (.Item("TYPE").ToString = "3" And .Item("COLHEAD").ToString = "G") Then
                                    g.DrawLine(Pens.Silver, 340, (START_POS), c10, (START_POS))
                                    If .Item("TRANTYPE").ToString = "AI" And .Item("RESULT").ToString = "2.1" And .Item("COLHEAD").ToString = "G" Then
                                        g.DrawLine(Pens.Silver, 340, (START_POS), c10, (START_POS))
                                        g.DrawString("( + GST EXTRA)", fontBold, BlackBrush, c9, START_POS + LINE_SPACE, rAlign)
                                    End If
                                End If
                                If START_POS >= APPBOTTOM_POS Then
                                    PagecountSale = PagecountSale + 1
                                    If _dt_count = PagecountSale Then
                                        GoTo gotofooter
                                    Else
                                        START_POS = TSTART_POS ' TOP AGAIN STARTING POSITION
                                        g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, APPBOTTOM_POS + 10)
                                        e.HasMorePages = True
                                        Exit Sub
                                    End If
                                End If
                                'New Line Start
                                If .Item("CALTYPE").ToString = "HA" And .Item("RESULT").ToString = "1.1" Then
                                    START_POS = START_POS + LINE_SPACE
                                Else
                                    START_POS = START_POS + LINE_SPACE
                                    START_POS = START_POS + LINE_SPACE
                                End If
                                If .Item("RESULT").ToString = "1.0" And (.Item("CALTYPE").ToString = "F" Or .Item("CALTYPE").ToString = "R") Then '.Item("COLHEAD").ToString <> "G" And .Item("COLHEAD").ToString <> "P" And .Item("COLHEAD").ToString <> "T"
                                    If _SILVERBILLPRINT <> "Y" Then
                                    End If
                                End If
                                If .Item("RESULT").ToString = "1.0" And (.Item("BILLNAME").ToString = "Y") Then '.Item("COLHEAD").ToString <> "G" And .Item("COLHEAD").ToString <> "P" And .Item("COLHEAD").ToString <> "T"
                                    g.DrawString("Fixing of Kemp, Kundan Stones needs wax, Hence at the time of melting weight loss will be there upto 10%..", fontRegularsmall, BlackBrush, c2, START_POS, LeftFormat)
                                    START_POS = START_POS + LINE_SPACE
                                End If
                                If START_POS >= APPBOTTOM_POS Then
                                    PagecountSale = PagecountSale + 1
                                    If _dt_count = PagecountSale Then
                                        GoTo gotofooter
                                    Else
                                        START_POS = TSTART_POS ' TOP AGAIN STARTING POSITION
                                        g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, APPBOTTOM_POS + 10)
                                        e.HasMorePages = True
                                        Exit Sub
                                    End If
                                End If
                                If (.Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" And Val(.Item("QTY").ToString) >= 2) Or
                                (.Item("TYPE").ToString = "3" And .Item("COLHEAD").ToString = "G") Then
                                End If
                                If (.Item("RESULT").ToString = "3.10" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G") Then
                                    g.DrawLine(Pens.Silver, 710, (START_POS), c10, (START_POS))
                                End If
                                If (.Item("TYPE").ToString = "4" And .Item("COLHEAD").ToString = "T") Then
                                    If Val(.Item("AMOUNT").ToString) <> 0 Then
                                        g.DrawLine(Pens.Silver, c8, START_POS, c10, START_POS)
                                    End If
                                End If
                                If .Item("RESULT").ToString = "3.6" And .Item("TYPE").ToString = "12" Then
                                    g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.0" And .Item("TYPE").ToString = "11.0" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawLine(Pens.Silver, 340, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.4" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.5" Then
                                    g.DrawLine(Pens.Silver, 710, (START_POS), c10, (START_POS))
                                End If
                                If START_POS >= APPBOTTOM_POS Then
                                    PagecountSale = PagecountSale + 1
                                    If _dt_count = PagecountSale Then
                                        GoTo gotofooter
                                    Else
                                        START_POS = TSTART_POS ' TOP AGAIN STARTING POSITION
                                        g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, APPBOTTOM_POS + 10)
                                        e.HasMorePages = True
                                        Exit Sub
                                    End If
                                End If
                            End With
                        End If

                        If dtSales.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("AR") _
                            Then
                            If START_POS >= BOTTOM_POS Then
                                PagecountSale = PagecountSale + 1
                                If _dt_count = PagecountSale Then
                                    GoTo gotofooter
                                Else
                                    START_POS = TSTART_POS ' TOP AGAIN STARTING POSITION
                                    g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, APPBOTTOM_POS + 10)
                                    e.HasMorePages = True
                                    Exit For
                                End If
                            End If
                            If subputitle = False Then TitleApproval(e.Graphics, e) : subputitle = True
                            With dtSales.Rows(PagecountSale)
                                ''Top '/************************************////
                                NoofPage += 1
                                If dtSales.Rows(PagecountSale).Item("RESULT") = "4.1" Or dtSales.Rows(PagecountSale).Item("RESULT") = "4.2" Then
                                    START_POS = START_POS + LINE_SPACE
                                    g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.3" And .Item("TYPE").ToString = "2" Then
                                    g.DrawLine(Pens.Silver, 710, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.0" And (.Item("TYPE").ToString = "2" Or .Item("TYPE").ToString = "3") And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawLine(Pens.Silver, 310, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.2" And (.Item("TYPE").ToString = "2" Or .Item("TYPE").ToString = "3") And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.5" And (.Item("TYPE").ToString = "2" Or .Item("TYPE").ToString = "3") And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS))
                                End If
                                g.DrawString(.Item("SRNO").ToString(), fontRegular, BlackBrush, c1, START_POS, LeftFormat)
                                If .Item("TYPE").ToString = "10" And .Item("COLHEAD").ToString = "G" Then
                                    If .Item("ITEMNAME").ToString.Length > 25 Then
                                        If .Item("RESULT").ToString <> "0.0" Then g.DrawString(.Item("ITEMNAME").ToString(), fontBoldsmall, BlackBrush, c3 - 20, START_POS, LeftFormat)
                                    Else
                                        If .Item("RESULT").ToString <> "0.0" Then g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c3, START_POS, LeftFormat)
                                    End If
                                Else
                                    If .Item("CALTYPE").ToString = "HA" And .Item("RESULT").ToString = "1.1" Then
                                        If .Item("RESULT").ToString <> "0.0" Then g.DrawString(.Item("ITEMNAME").ToString(), fontRegular6, BlackBrush, c3, START_POS, LeftFormat)
                                    ElseIf .Item("RESULT").ToString = "3.0" And (.Item("TYPE").ToString = "2" Or .Item("TYPE").ToString = "3") And (.Item("COLHEAD").ToString = "G" Or .Item("COLHEAD").ToString = "S") Then
                                        g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c3, START_POS, LeftFormat)
                                    ElseIf .Item("RESULT").ToString = "4.1" And .Item("TYPE").ToString = "4" Then
                                        g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c3, START_POS, LeftFormat)
                                    Else
                                        If .Item("RESULT").ToString <> "0.0" Then
                                            g.DrawString(.Item("TAGNO").ToString(), fontRegularsmall, BlackBrush, c2 - 5, START_POS, LeftFormat)
                                            g.DrawString(.Item("ITEMNAME").ToString(), fontRegularsmall, BlackBrush, c3, START_POS, LeftFormat)
                                        End If
                                    End If
                                End If
                                If .Item("RESULT").ToString = "4.2" And .Item("TYPE").ToString = "4" Then
                                    g.DrawString(.Item("HSN").ToString(), fontBold, BlackBrush, c2, START_POS, LeftFormat)
                                ElseIf .Item("RESULT").ToString = "3.2" And .Item("TYPE").ToString = "2" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawString(.Item("HSN").ToString(), fontAmtInWord, BlackBrush, c2, START_POS, LeftFormat)
                                ElseIf .Item("RESULT").ToString = "3.5" And .Item("TYPE").ToString = "3" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawString(.Item("HSN").ToString(), fontBold, BlackBrush, c2, START_POS, LeftFormat)
                                End If
                                g.DrawString(IIf(Val(.Item("QTY").ToString()) <> 0, Val(.Item("QTY").ToString()), ""), fontRegular, BlackBrush, c4, START_POS, RightFormat)
                                If .Item("CALTYPE").ToString = "F" Then 'Fixed Item Should Not print GrossWt
                                    g.DrawString("", fontRegular, BlackBrush, c4, START_POS, RightFormat)
                                    g.DrawString("", fontRegular, BlackBrush, c5, START_POS, RightFormat)
                                Else
                                    If .Item("RESULT").ToString = "3.0" And (.Item("TYPE").ToString = "2" Or .Item("TYPE").ToString = "3") And .Item("COLHEAD").ToString = "G" Then
                                        g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontBold, BlackBrush, c5 + 100, START_POS, RightFormat)
                                        g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontBold, BlackBrush, c6 + 100, START_POS, RightFormat)
                                    Else
                                        g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontRegularsmall, BlackBrush, c5 + 100, START_POS, RightFormat)
                                        g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, c6 + 100, START_POS, RightFormat)
                                    End If
                                End If
                                If .Item("COLHEAD").ToString() = "G" Or .Item("COLHEAD").ToString = "P" Or .Item("COLHEAD").ToString = "V" Then
                                    If .Item("COLHEAD").ToString = "G" And (.Item("TYPE").ToString = "2" Or .Item("TYPE").ToString = "3") Then
                                        g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, START_POS, LeftFormat)
                                    ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString <> "10" Then
                                        g.DrawString(.Item("RATE").ToString(), fontRegularsmall, BlackBrush, 550, START_POS, LeftFormat)
                                    ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString <> "3.50") Then
                                        g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, START_POS, LeftFormat)
                                    ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString = "3.50") Then
                                        g.DrawString(.Item("RATE").ToString(), fontRegularsmall, BlackBrush, 550, START_POS, LeftFormat)
                                    ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10" Then
                                        g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, START_POS, LeftFormat)
                                    ElseIf .Item("COLHEAD").ToString = "V" And .Item("TYPE").ToString = "3" Then
                                        g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, START_POS, LeftFormat)
                                    End If
                                Else
                                End If
                                If .Item("CALTYPE").ToString = "F" Then 'Fixed Item Should Not print mC
                                    g.DrawString("", fontRegular, BlackBrush, c8, START_POS, RightFormat)
                                End If
                                If .Item("COLHEAD").ToString = "G" Or .Item("COLHEAD").ToString = "P" Or .Item("COLHEAD").ToString = "T" Or .Item("COLHEAD").ToString = "V" Then
                                    If .Item("COLHEAD").ToString = "G" And (.Item("TYPE").ToString = "2" Or .Item("TYPE").ToString = "3") Then
                                        g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                    ElseIf .Item("COLHEAD").ToString = "T" And .Item("TYPE").ToString = "4" Then
                                        g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                    ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString <> "10" Then
                                        g.DrawString(.Item("AMOUNT").ToString(), fontRegularsmall, BlackBrush, c10, START_POS, rAlign)
                                    ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString <> "3.50") Or (.Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10") Then
                                        g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                    ElseIf .Item("COLHEAD").ToString = "V" And .Item("TYPE").ToString = "3" Then
                                        g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                    ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString = "3.50") Then
                                        g.DrawString(.Item("AMOUNT").ToString(), fontRegularsmall, BlackBrush, c10, START_POS, rAlign)
                                    ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10" Then
                                        g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                    End If
                                Else
                                    g.DrawString(.Item("AMOUNT").ToString(), fontRegularsmall, BlackBrush, c10, START_POS, rAlign)
                                End If
                                If (.Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" And Val(.Item("QTY").ToString) >= 2) _
                                    Or (.Item("TYPE").ToString = "2.1" And .Item("COLHEAD").ToString = "G" And Val(.Item("QTY").ToString) >= 2) _
                                    Or (.Item("TYPE").ToString = "3" And .Item("COLHEAD").ToString = "G" And Val(.Item("QTY").ToString) >= 2) Then
                                    g.DrawLine(Pens.Silver, 340, (START_POS), c10, (START_POS))
                                    If .Item("TRANTYPE").ToString = "AI" And .Item("RESULT").ToString = "2.1" And .Item("COLHEAD").ToString = "G" And Val(.Item("QTY").ToString) >= 2 Then
                                        g.DrawLine(Pens.Silver, 340, (START_POS), c10, (START_POS))
                                        g.DrawString("( INCL . GST )", fontBold, BlackBrush, c9, START_POS + LINE_SPACE, rAlign)
                                    End If
                                End If
                                If START_POS >= APPBOTTOM_POS Then
                                    PagecountSale = PagecountSale + 1
                                    If _dt_count = PagecountSale Then
                                        GoTo gotofooter
                                    Else
                                        START_POS = TSTART_POS ' TOP AGAIN STARTING POSITION
                                        g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, APPBOTTOM_POS + 10)
                                        e.HasMorePages = True
                                        Exit For
                                    End If
                                End If
                                'New Line Start
                                If .Item("CALTYPE").ToString = "HA" And .Item("RESULT").ToString = "1.1" Then
                                    START_POS = START_POS + LINE_SPACE
                                Else
                                    START_POS = START_POS + LINE_SPACE
                                End If
                                If .Item("RESULT").ToString = "1.0" And (.Item("CALTYPE").ToString = "F" Or .Item("CALTYPE").ToString = "R") Then '.Item("COLHEAD").ToString <> "G" And .Item("COLHEAD").ToString <> "P" And .Item("COLHEAD").ToString <> "T"
                                    If _SILVERBILLPRINT <> "Y" Then
                                    End If
                                End If
                                If .Item("RESULT").ToString = "1.0" And (.Item("BILLNAME").ToString = "Y") Then '.Item("COLHEAD").ToString <> "G" And .Item("COLHEAD").ToString <> "P" And .Item("COLHEAD").ToString <> "T"
                                    g.DrawString("Fixing of Kemp, Kundan Stones needs wax, Hence at the time of melting weight loss will be there upto 10%..", fontRegularsmall, BlackBrush, c2, START_POS, LeftFormat)
                                    START_POS = START_POS + LINE_SPACE
                                End If
                                If START_POS >= APPBOTTOM_POS Then
                                    PagecountSale = PagecountSale + 1
                                    If _dt_count = PagecountSale Then
                                        GoTo gotofooter
                                    Else
                                        START_POS = TSTART_POS ' TOP AGAIN STARTING POSITION
                                        g.DrawString("Continue.... ", fontRegularsmall, Brushes.Black, c8, APPBOTTOM_POS + 10)
                                        e.HasMorePages = True
                                        Exit For
                                    End If
                                End If
                                If (.Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" And Val(.Item("QTY").ToString) >= 2) Or
                                    (.Item("TYPE").ToString = "3" And .Item("COLHEAD").ToString = "G" And Val(.Item("QTY").ToString) >= 2) Then
                                End If
                                If (.Item("RESULT").ToString = "3.10" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G") Then
                                    g.DrawLine(Pens.Silver, 710, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "4.2" And .Item("COLHEAD").ToString = "T" Then
                                    If Val(.Item("AMOUNT").ToString) <> 0 Then
                                        g.DrawLine(Pens.Silver, 20, START_POS, c10, START_POS)
                                    End If
                                End If
                                If .Item("RESULT").ToString = "3.0" And .Item("TYPE").ToString = "11.0" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawLine(Pens.Silver, 340, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.2" And .Item("TYPE").ToString = "2" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.5" And .Item("TYPE").ToString = "3" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.3" And .Item("TYPE").ToString = "2" Then
                                    g.DrawLine(Pens.Silver, 710, (START_POS), c10, (START_POS))
                                End If
                                If START_POS >= APPBOTTOM_POS Then
                                    PagecountSale = PagecountSale + 1
                                    If _dt_count = PagecountSale Then
                                        GoTo gotofooter
                                    Else
                                        START_POS = TSTART_POS ' TOP AGAIN STARTING POSITION
                                        g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, APPBOTTOM_POS + 10)
                                        e.HasMorePages = True
                                        Exit For
                                    End If
                                End If
                            End With
                        End If
                    Next
                End If
                If dtSales.Rows.Count > 0 And Appsummary = True Then
                    InsertApprovalDetails()
                    For PagecountSale = PagecountSale To dtApproval.Rows.Count - 1
                        If Not dtApproval.Rows(PagecountSale).Item("TRANTYPE").ToString.Contains("AR") _
                            Then
                            With dtApproval.Rows(PagecountSale)
                                ''Top '/************************************////
                                NoofPage += 1
                                If .Item("RESULT").ToString = "3.5" Then
                                    g.DrawLine(Pens.Silver, 710, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.0" And .Item("TYPE").ToString = "11.0" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawLine(Pens.Silver, 340, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.4" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.6" And .Item("TYPE").ToString = "12" Then
                                    g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS))
                                End If
                                g.DrawString(.Item("SRNO").ToString(), fontRegular, BlackBrush, c1, START_POS) 'LeftFormat
                                If .Item("TYPE").ToString = "10" And .Item("COLHEAD").ToString = "G" Then
                                    If .Item("ITEMNAME").ToString.Length > 25 And .Item("RESULT").ToString <> "3.3" Then
                                        g.DrawString(.Item("ITEMNAME").ToString(), fontBoldsmall, BlackBrush, c3 - 20, START_POS, LeftFormat)
                                    ElseIf .Item("RESULT").ToString = "3.3" Then
                                        g.DrawString(.Item("ITEMNAME").ToString(), fontRegularsmall, BlackBrush, c3, START_POS, LeftFormat)
                                    Else
                                        g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c3, START_POS, LeftFormat)
                                    End If
                                Else
                                    If .Item("CALTYPE").ToString = "HA" And .Item("RESULT").ToString = "1.1" Then
                                        g.DrawString(.Item("ITEMNAME").ToString(), fontRegular6, BlackBrush, c3, START_POS, LeftFormat)
                                    ElseIf .Item("RESULT").ToString = "2.1" And .Item("TYPE").ToString = "1" And (.Item("COLHEAD").ToString = "G" Or .Item("COLHEAD").ToString = "S") Then
                                        g.DrawString(.Item("ITEMNAME").ToString(), fontBold, BlackBrush, c3, START_POS, LeftFormat)
                                    Else
                                        g.DrawString(.Item("ITEMNAME").ToString(), fontRegularsmall, BlackBrush, c3, START_POS, LeftFormat)
                                    End If
                                End If
                                If .Item("RESULT").ToString = "3.6" And .Item("TYPE").ToString = "11" Then
                                    g.DrawString(.Item("HSN").ToString(), fontRegularsmall, BlackBrush, c2, START_POS, LeftFormat) 'fontBold
                                    g.DrawString(.Item("QTY").ToString(), fontBold, BlackBrush, c4, START_POS, rAlign)
                                ElseIf .Item("RESULT").ToString = "3.4" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawString(.Item("HSN").ToString(), fontAmtInWord, BlackBrush, c2, START_POS, LeftFormat)
                                    g.DrawString(.Item("QTY").ToString(), fontBold, BlackBrush, c4, START_POS, RightFormat)
                                Else
                                    g.DrawString(.Item("HSN").ToString(), fontRegularsmall, BlackBrush, c2, START_POS, LeftFormat)
                                    If .Item("RESULT").ToString = "2.1" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" Then
                                        g.DrawString(.Item("QTY").ToString(), fontBold, BlackBrush, c4, START_POS, RightFormat)
                                    Else
                                        g.DrawString(IIf(Val(.Item("QTY").ToString()) <> 0, Val(.Item("QTY").ToString()), ""), fontRegularsmall, BlackBrush, c4, START_POS, RightFormat)
                                    End If
                                End If
                                If .Item("CALTYPE").ToString = "F" Then 'Fixed Item Should Not print GrossWt
                                    g.DrawString("", fontRegular, BlackBrush, c5, START_POS, RightFormat)
                                    g.DrawString("", fontRegular, BlackBrush, c6, START_POS, RightFormat)
                                ElseIf .Item("RESULT").ToString = "2.1" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontBold, BlackBrush, c5 + 100, START_POS, RightFormat)
                                    g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontBold, BlackBrush, c6 + 100, START_POS, RightFormat)
                                Else
                                    g.DrawString(IIf(Val(.Item("GRSWT").ToString()) <> 0, .Item("GRSWT").ToString(), ""), fontRegularsmall, BlackBrush, c5 + 100, START_POS, RightFormat)
                                    g.DrawString(IIf(Val(.Item("NETWT").ToString()) <> 0, .Item("NETWT").ToString(), ""), fontRegularsmall, BlackBrush, c6 + 100, START_POS, RightFormat)
                                End If
                                If .Item("COLHEAD").ToString() = "G" Or .Item("COLHEAD").ToString = "P" Or .Item("COLHEAD").ToString = "V" Then
                                    If .Item("COLHEAD").ToString = "G" And (.Item("TYPE").ToString = "1" Or .Item("TYPE").ToString = "3") Then
                                        g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, START_POS, LeftFormat)
                                    ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString <> "10" Then
                                        g.DrawString(.Item("RATE").ToString(), fontRegularsmall, BlackBrush, 550, START_POS, LeftFormat)
                                    ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString <> "3.50") Then
                                        g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, START_POS, LeftFormat)
                                    ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString = "3.50") Then
                                        g.DrawString(.Item("RATE").ToString(), fontRegularsmall, BlackBrush, 550, START_POS, LeftFormat)
                                    ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10" Then
                                        g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, START_POS, LeftFormat)
                                    ElseIf .Item("COLHEAD").ToString = "V" And .Item("TYPE").ToString = "1" Then
                                        g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, START_POS, LeftFormat)
                                    ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "1" Then
                                        g.DrawString(.Item("RATE").ToString(), fontBold, BlackBrush, 550, START_POS, LeftFormat)
                                    End If
                                Else
                                End If
                                If .Item("CALTYPE").ToString = "F" Then 'Fixed Item Should Not print mC
                                    g.DrawString("", fontRegular, BlackBrush, c8, START_POS, RightFormat)
                                Else
                                    g.DrawString("", fontRegular, BlackBrush, c8, START_POS, RightFormat)
                                End If
                                If .Item("COLHEAD").ToString = "G" Or .Item("COLHEAD").ToString = "P" Or .Item("COLHEAD").ToString = "V" Then
                                    If .Item("RESULT").ToString = "2.1" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" Then
                                        g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                    ElseIf (.Item("RESULT").ToString = "3.4" Or .Item("RESULT").ToString = "3.5") And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" Then
                                        g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                    ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString <> "10" Then
                                        g.DrawString(.Item("AMOUNT").ToString(), fontRegularsmall, BlackBrush, c10, START_POS, rAlign)
                                    ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString <> "3.50") Or (.Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10") Then
                                        g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                        If .Item("RESULT").ToString = "3.6" And .Item("TYPE").ToString = "11" Then
                                        End If
                                    ElseIf .Item("COLHEAD").ToString = "V" And .Item("TYPE").ToString = "1" Then
                                        g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                    ElseIf (.Item("COLHEAD").ToString = "P" And .Item("RESULT").ToString = "3.50") Then
                                        g.DrawString(.Item("AMOUNT").ToString(), fontRegularsmall, BlackBrush, c10, START_POS, rAlign)
                                    ElseIf .Item("COLHEAD").ToString = "G" And .Item("TYPE").ToString = "10" Then
                                        g.DrawString(.Item("AMOUNT").ToString(), fontBold, BlackBrush, c10, START_POS, rAlign)
                                    End If
                                Else
                                    g.DrawString(.Item("AMOUNT").ToString(), fontRegularsmall, BlackBrush, c10, START_POS, rAlign)
                                End If
                                If (.Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" And .Item("RESULT").ToString <> "3.5") _
                                Or (.Item("TYPE").ToString = "2.1" And .Item("COLHEAD").ToString = "G") _
                                Or (.Item("TYPE").ToString = "3" And .Item("COLHEAD").ToString = "G") Then
                                    g.DrawLine(Pens.Silver, 340, (START_POS), c10, (START_POS))
                                    If .Item("TRANTYPE").ToString = "AI" And .Item("TYPE").ToString = "2.1" And .Item("COLHEAD").ToString = "G" Then
                                        g.DrawString("(INCL . GST)", fontBold, BlackBrush, c9, START_POS, rAlign)
                                    End If
                                End If
                                If START_POS >= APPBOTTOM_POS Then
                                    PagecountSale = PagecountSale + 1
                                    If _dt_count = PagecountSale Then
                                        GoTo gotofooter
                                    Else
                                        START_POS = TSTART_POS ' TOP AGAIN STARTING POSITION
                                        g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, APPBOTTOM_POS + 10)
                                        e.HasMorePages = True
                                        Exit Sub
                                    End If
                                End If
                                'New Line Start
                                If .Item("CALTYPE").ToString = "HA" And .Item("RESULT").ToString = "1.1" Then
                                    START_POS = START_POS + LINE_SPACE
                                Else
                                    START_POS = START_POS + LINE_SPACE
                                    START_POS = START_POS + LINE_SPACE
                                End If
                                If .Item("RESULT").ToString = "1.0" And (.Item("CALTYPE").ToString = "F" Or .Item("CALTYPE").ToString = "R") Then '.Item("COLHEAD").ToString <> "G" And .Item("COLHEAD").ToString <> "P" And .Item("COLHEAD").ToString <> "T"
                                    If _SILVERBILLPRINT <> "Y" Then
                                    End If
                                End If
                                If .Item("RESULT").ToString = "1.0" And (.Item("BILLNAME").ToString = "Y") Then '.Item("COLHEAD").ToString <> "G" And .Item("COLHEAD").ToString <> "P" And .Item("COLHEAD").ToString <> "T"
                                    g.DrawString("Fixing of Kemp, Kundan Stones needs wax, Hence at the time of melting weight loss will be there upto 10%..", fontRegularsmall, BlackBrush, c2, START_POS, LeftFormat)
                                    START_POS = START_POS + LINE_SPACE
                                End If
                                If START_POS >= APPBOTTOM_POS Then
                                    PagecountSale = PagecountSale + 1
                                    If _dt_count = PagecountSale Then
                                        GoTo gotofooter
                                    Else
                                        START_POS = TSTART_POS ' TOP AGAIN STARTING POSITION
                                        g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, APPBOTTOM_POS + 10)
                                        e.HasMorePages = True
                                        Exit Sub
                                    End If
                                End If
                                If (.Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" And Val(.Item("QTY").ToString) >= 2) Or
                                (.Item("TYPE").ToString = "3" And .Item("COLHEAD").ToString = "G") Then
                                End If
                                If (.Item("RESULT").ToString = "3.10" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G") Then
                                    g.DrawLine(Pens.Silver, 710, (START_POS), c10, (START_POS))
                                End If
                                If (.Item("TYPE").ToString = "4" And .Item("COLHEAD").ToString = "T") Then
                                    If Val(.Item("AMOUNT").ToString) <> 0 Then
                                        g.DrawLine(Pens.Silver, c8, START_POS, c10, START_POS)
                                    End If
                                End If
                                If .Item("RESULT").ToString = "3.6" And .Item("TYPE").ToString = "12" Then
                                    g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.0" And .Item("TYPE").ToString = "11.0" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawLine(Pens.Silver, 340, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.4" And .Item("TYPE").ToString = "1" And .Item("COLHEAD").ToString = "G" Then
                                    g.DrawLine(Pens.Silver, 20, (START_POS), c10, (START_POS))
                                End If
                                If .Item("RESULT").ToString = "3.5" Then
                                    g.DrawLine(Pens.Silver, 710, (START_POS), c10, (START_POS))
                                End If
                                If START_POS >= APPBOTTOM_POS Then
                                    PagecountSale = PagecountSale + 1
                                    If _dt_count = PagecountSale Then
                                        GoTo gotofooter
                                    Else
                                        START_POS = TSTART_POS ' TOP AGAIN STARTING POSITION
                                        g.DrawString("Continue.... ", fontRegular, Brushes.Black, c8, APPBOTTOM_POS + 10)
                                        e.HasMorePages = True
                                        Exit Sub
                                    End If
                                End If
                            End With
                        End If
                    Next
                End If
gotofooter:
                FooterApproval(e.Graphics, e)
                If _dt_count <= PagecountSale Then
                    Appsummary = True
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            Exit Sub
        End Try
    End Sub
    Public Sub InsertApprovalDetails()
        Dim dtapp As DataTable = dtSales.DefaultView.ToTable(True, "ITEM")
        dtApproval = dtSales.Copy
        dtApproval.Rows.Clear()
        Dim Index As Integer = 0
        For Each drr As DataRow In dtapp.Select("ITEM<>''", Nothing)
            Index = Index + 1
            If drr("ITEM").ToString = "" Then Continue For

            Dim ro As DataRow = dtApproval.NewRow
            ro("SRNO") = Index
            ro("ITEMNAME") = drr("ITEM").ToString
            ro("QTY") = Val(dtSales.Compute("SUM(QTY)", "ITEM ='" & drr("ITEM").ToString & "'").ToString)
            ro("GRSWT") = Val(dtSales.Compute("SUM(GRSWT)", "ITEM ='" & drr("ITEM").ToString & "'").ToString)
            ro("NETWT") = Val(dtSales.Compute("SUM(NETWT)", "ITEM ='" & drr("ITEM").ToString & "'").ToString)
            ro("AMOUNT") = Val(dtSales.Compute("SUM(AMOUNT)", "ITEM ='" & drr("ITEM").ToString & "'").ToString)
            ro("RESULT") = Val(dtSales.Compute("AVG(RESULT)", "ITEM ='" & drr("ITEM").ToString & "'").ToString) 'dtSales.Rows(Index).Item("RESULT").ToString
            ro("TYPE") = Val(dtSales.Compute("AVG(TYPE)", "ITEM ='" & drr("ITEM").ToString & "'").ToString)
            ro("COLHEAD") = ""
            ro("TRANTYPE") = TrantypeMI.ToString
            ro("ITEM") = drr("ITEM").ToString
            dtApproval.Rows.Add(ro)
xxx:
            dtApproval.AcceptChanges()
        Next
        For rwIndex As Integer = 0 To dtSales.Rows.Count - 1
            Dim ro As DataRow = dtApproval.NewRow
            If dtSales.Rows(rwIndex).Item("RESULT").ToString = "2.1" Then
                For colIndex As Integer = 2 To dtSales.Columns.Count - 1
                    ro(colIndex) = dtSales.Rows(rwIndex).Item(colIndex)
                Next
                dtApproval.Rows.Add(ro)
            End If
        Next
        dtApproval.AcceptChanges()
    End Sub
    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Me.DialogResult = DialogResult.OK
    End Sub
    Private Sub frmBillPrintDocA4N_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CmponentLoad()
        If _PRINTCPY Then
            ChkCusCopy.Checked = True
            ChkOffCopy.Checked = True
        Else
            ChkCusCopy.Checked = False
            ChkOffCopy.Checked = False
        End If
        If SMALLBILLPRINT.ToString <> "" Then
            chkpartlysale.Checked = True
            chkSmallPrint.Checked = True
        Else
            chkSmallPrint.Checked = False
            chkpartlysale.Checked = False
        End If
    End Sub
    Private Sub CmponentLoad()
        pnlLeft.Width = (pnlMain.Width - pctBox.Width) / 2
        pnlRight.Width = pnlLeft.Width
        pctBox.Location = New Point(pnlLeft.Width)

        Dim printeritem As String
        Dim PrnSetting As New PrinterSettings
        For i As Integer = 0 To PrinterSettings.InstalledPrinters.Count - 1
            printeritem = PrinterSettings.InstalledPrinters(i).ToString.ToUpper
            cmbPrinte_Name.Items.Add(printeritem)
        Next
        cmbPrinte_Name.Text = PrnSetting.PrinterName
    End Sub
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
    Private Sub frmBillPrintDocA4N_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If

        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub PrintGift_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintGift.PrintPage
        Try
            Dim NoofPage As Integer = 0
            Dim gstPrint As Boolean = False
            Dim rAlign As New StringFormat
            rAlign.Alignment = StringAlignment.Far
            NoofPrint += 1
            Using g As Graphics = e.Graphics
                CustomerInfoGiftVoucher(e.Graphics, e)
                ModeofpaymentInfogiftvoucher(e.Graphics, e)
                g.DrawLine(Pens.DarkGray, c1, START_POS, c10, START_POS)
                'If dsGiftvoucher.Tables("OUTSTANDING").Rows(0).Item("field1").ToString.Contains("ADVANCE ") Or dsGiftvoucher.Tables("OUTSTANDING").Rows(0).Item("field1").ToString.Contains("RECEIPT  VOUCHER") Or dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("field1").ToString.Contains("INVOICE") Then
                Dim Amt As Double = Val(dsGiftvoucher.Tables("OUTSTANDING").Compute("SUM(AMOUNT)", "").ToString) ''Val(dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("amount").ToString)
                Dim AmtWords As String = ConvertRupees.RupeesToWord(Amt)

                START_POS = START_POS + 5
                'g.DrawString("NOTE : ORIGINAL ADVANCE RECEIPT SHOULD BE PRODUCED AT THE TIME OF ITEM(S) DELIVERY ", fontBold, Brushes.Black, c2, START_POS)
                g.DrawString("(" + AmtWords + ")", fontBold, Brushes.Black, c2, START_POS)
                g.DrawString("Total", fontBold, Brushes.Black, c7, START_POS)
                ''g.DrawString(dsAdvanceReceipt.Tables("OUTSTANDING").Rows(0).Item("AMOUNT"), fontBoldTitle, Brushes.Black, c10, START_POS, rAlign)
                g.DrawString(Format(Val(Amt.ToString), "0.00"), fontBoldTitle, Brushes.Black, c10, START_POS, rAlign)
                ''g.DrawString(NodeId, fontRegular, BlackBrush, c7, START_POS, rAlign)

                START_POS = START_POS + LINE_SPACE + 5

                g.DrawLine(Pens.DarkGray, c1, START_POS, c10, START_POS)

                If START_POS <= BOTTOM_POS Then '' 838
                    START_POS = BOTTOM_POS
                Else
                    START_POS = BOTTOM_POS ''1050
                End If
                Dim EMP_NAME As String = dsGiftvoucher.Tables("OUTSTANDING").Rows(0).Item("EMP_NAME").ToString
                If dsGiftvoucher.Tables("OUTSTANDING").Rows(0).Item("EMP_NAME").ToString = "" Or Val(dsGiftvoucher.Tables("OUTSTANDING").Rows(0).Item("EMP_NAME").ToString) = 0 Then
                    EMP_NAME = ""
                End If

                START_POS = START_POS
                g.DrawString(EMP_NAME, fontRegular, Brushes.Black, c1, START_POS)
                START_POS = START_POS + LINE_SPACE
                If _SILVERBILLPRINT = "Y" Then
                    g.DrawString("Salesman", fontRegular, Brushes.Black, c1, START_POS + 3)
                    g.DrawString("Cashier", fontRegular, Brushes.Black, c3, START_POS + 3)
                    g.DrawString("Customer Signature", fontRegular, Brushes.Black, c4, START_POS + 3)
                    g.DrawString("For " & strCompanyName, New Font("Times New Roman", 8, FontStyle.Regular), Brushes.Black, c8, START_POS + 3)
                    START_POS = START_POS + LINE_SPACE + LINE_SPACE
                    g.DrawLine(Pens.DarkGray, c1, START_POS, c10, START_POS)
                End If
                If placeofSupply = GetSqlValue(cn, "SELECT (SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID=C.STATEID) STATENAME FROM " & cnAdminDb & "..COMPANY  C WHERE C.COMPANYID='" & strCompanyId & "'") Then
                    If FOOTER_BILLPRINT <> "" Then
                        g.DrawString(FOOTER_BILLPRINT, fontRegular, Brushes.Black, c1, BOTTOM_POS + 60)
                    Else
                        g.DrawString("Delivery at Showroom", fontRegular, Brushes.Black, c1, BOTTOM_POS + 60)
                    End If
                Else
                    g.DrawString("Delivery at " & placeofSupply, fontRegular, Brushes.Black, c1, BOTTOM_POS + 60)
                    ' g.DrawString(Rules(0).ToString, New Font("Palatino Linotype", 8, FontStyle.Regular), Brushes.Black, c3, BOTTOM_POS + 60)
                    'g.DrawString(Rules(1).ToString, New Font("Palatino Linotype", 8, FontStyle.Regular), Brushes.Black, c3, BOTTOM_POS + 80)
                End If
                If _SILVERBILLPRINT = "Y" Then
                    g.DrawString("(Certified that the particulars given above are true and correct)", fontRegular, Brushes.Black, c4, BOTTOM_POS + 60)
                End If

                'TOP 
                ''g.DrawLine(Pens.Silver, 20.0F, 490.0F, 300.0F, 490.0F)
                ''g.DrawLine(Pens.Silver, 460.0F, 490.0F, 775.0F, 490.0F)
                ''g.DrawString("[GSTIN " & GSTNO & "]", fontRegular, Brushes.Black, 300, 480)
                ''g.DrawString("For " & strCompanyName, New Font("Times New Roman", 8, FontStyle.Regular), Brushes.Black, c6, 490)
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            Exit Sub
        End Try
    End Sub
    Function NEWMAILSEND(ByVal ToMail As String, ByVal Attachmentpath As String)
        Dim FromId As String = ""
        Dim Password As String = Nothing
        'Dim smtpServer As New System.Net.Mail.SmtpClient("smtp.gmail.com", 587)
        'Dim smtpServer As New System.Net.Mail.SmtpClient("smtppro.zoho.com", 587)
        Dim smtpServer As New System.Net.Mail.SmtpClient
        Dim mail As New System.Net.Mail.MailMessage
        Dim tempFile As String = Attachmentpath
        Dim dt As New DataTable
        Try
            strsql = "SELECT * FROM  " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'FROMMAILSERVER'"
            cmd = New OleDbCommand(strsql, cn)
            da = New OleDbDataAdapter(cmd)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                FromId = dt.Rows(0).Item("ctltext").ToString
            Else
                MsgBox("NO MAIL ID IN SOFTCONTROL", MsgBoxStyle.Information)
            End If
            strsql = "SELECT * FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'FROMMAILPASSWARD'"
            cmd = New OleDbCommand(strsql, cn)
            da = New OleDbDataAdapter(cmd)
            dt = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Password = dt.Rows(0).Item("ctltext").ToString
            Else
                MsgBox("NO PASSWORD IN SOFTCONTROL", MsgBoxStyle.Information)
            End If
            '.......................MAIL........................
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
            If Trim(MailServer2) = "@gmail.com" Then
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
            ElseIf Trim(MailServer2) = "@hotmail.com" Then
                smtpServer.Port = 587
                smtpServer.Host = "smtp.live.com"
                smtpServer.EnableSsl = True
            Else
                smtpServer.Port = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-PORT'")
                smtpServer.Host = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-HOSTNAME'")
                smtpServer.EnableSsl = IIf(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-SSL'").ToString.ToUpper() = "Y", True, False)
            End If
            mail.From = New System.Net.Mail.MailAddress(FromId)
            mail.To.Add(New System.Net.Mail.MailAddress(ToMail))
            mail.Subject = strCompanyId & " E GURANTEE CARD"
            mail.IsBodyHtml = True
            Dim _MailMsg As String = ""
            _MailMsg = "<p style=""font-family:'Times New Roman'"">Dear Customer, <BR> <BR> Greetings from " & strCompanyName & " is excited to bring our finest <BR> creations of traditional South Indian jewellery,<BR>"
            _MailMsg += " Please find the attached your " & strCompanyId & " E GURANTEE CARD."
            _MailMsg += " <BR><BR>"
            _MailMsg += " This is an automatically generated email – please do not reply to it."
            _MailMsg += " <BR><BR>"
            _MailMsg += " Warm Regards,<BR>"
            _MailMsg += strCompanyName & "</p>"
            mail.Body = _MailMsg

            If File.Exists(tempFile) = True Then mail.Attachments.Add(New System.Net.Mail.Attachment(tempFile))
            'smtpServer.EnableSsl = True
            'smtpServer.Port = "587"
            smtpServer.Credentials = New System.Net.NetworkCredential(FromId.Trim.ToString, Password.Trim.ToString)
            smtpServer.Timeout = 400000
            smtpServer.Send(mail)
            mail.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            Return 0
        End Try
        Return 1
    End Function
    Public Sub PrtGuarantee_PrintPage(sender As Object, e As Drawing.Printing.PrintPageEventArgs) Handles PrtGuarantee.PrintPage
        Dim doc As New Spire.Pdf.PdfDocument
        Dim StrItemid As String
        Dim StrTagno As String
        Dim StrGrswt As String
        Dim StrDiawt As String = ""
        Dim Strstnpcs As String = ""
        Dim Strimgpath As String
        Dim Sign_image As Image
        Dim Background_Image As Image
        Dim dtdetails As New DataTable
        Dim dtdia As New DataTable
        Dim dtcolor As New DataTable
        Dim color As String
        Dim Itemmast_PctPath As Boolean = IIf(GetAdmindbSoftValue("PICPATHFROM", "S") = "I", True, False)
        Dim defaultPic As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'")
        strsql = " SELECT ITEMID,TAGNO,ROW_NUMBER() OVER (ORDER BY BATCHNO) T_SNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE='" & strBilldate & "' AND  BATCHNO='" & pBatchno & "' "
        Dim dttagdetail As New DataTable
        dttagdetail = GetSqlTable(strsql, cn)
        If dttagdetail.Rows.Count > 0 Then
            Dim _dtcnt As Integer = dttagdetail.Rows.Count

            For cnt As Integer = GCPagecountSale To dttagdetail.Rows.Count - 1
                Dim Imagefilename As String = ""
                strsql = " SELECT T.ITEMID, T.TAGNO, T.NARRATION AS STYLENO, T.GRSWT, T.PCTPATH, T.PCTFILE"
                strsql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE SNO=TS.SNO )As DIAWT"
                strsql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG T "
                strsql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAGSTONE TS ON TS.TAGSNO=T.SNO AND TS.STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')"
                strsql += vbCrLf + " WHERE T.TAGNO='" + dttagdetail.Rows(cnt).Item("TAGNO").ToString + "' AND T.ITEMID='" + dttagdetail.Rows(cnt).Item("ITEMID").ToString + "' "
                da = New OleDbDataAdapter(strsql, cn)
                da.Fill(dtdetails)
                If dtdetails.Rows.Count = 0 Then Continue For
                If Itemmast_PctPath Then
                    strsql = "SELECT ISNULL(ITEMPCTPATH,'') ITEMPCTPATH FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = '" & dttagdetail.Rows(cnt).Item("ITEMID").ToString & "'"
                    defaultPic = UCase(objGPack.GetSqlValue(strsql, "ITEMPCTPATH", "", tran))
                    If defaultPic.ToString = "" Then
                        strsql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'"
                        defaultPic = UCase(objGPack.GetSqlValue(strsql, "CTLTEXT", , tran))
                    End If
                    If Not defaultPic.EndsWith("\") And defaultPic <> "" Then defaultPic += "\"
                Else
                    strsql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PICPATH'"
                    defaultPic = UCase(objGPack.GetSqlValue(strsql, "CTLTEXT", , tran))
                    If Not defaultPic.EndsWith("\") And defaultPic <> "" Then defaultPic += "\"
                End If
                If dtdetails.Rows(0).Item("PCTFILE").ToString <> "" Then
                    Imagefilename = defaultPic + dtdetails.Rows(0).Item("PCTFILE").ToString
                End If
                StrGrswt = Val(dtdetails.Rows(0).Item("GRSWT").ToString)
                strsql = vbCrLf + "SELECT S.SHORTNAME,CONVERT(VARCHAR(100),SUM(STNPCS))STNPCS,"
                strsql += vbCrLf + " CONVERT(VARCHAR(100),CONVERT(NUMERIC(15,3),SUM(STNWT)))DIAWT FROM  " & cnAdminDb & "..ITEMTAGSTONE T "
                strsql += vbCrLf + " INNER JOIN  " & cnAdminDb & "..ITEMMAST I  ON T.STNITEMID=I.ITEMID     "
                strsql += vbCrLf + " INNER JOIN  " & cnAdminDb & "..SUBITEMMAST S ON  S.SUBITEMID=T.STNSUBITEMID  "
                strsql += vbCrLf + " WHERE METALID='D' AND T.TAGNO='" + dttagdetail.Rows(cnt).Item("TAGNO").ToString + "' AND T.ITEMID='" + dttagdetail.Rows(cnt).Item("ITEMID").ToString + "' "
                strsql += vbCrLf + " GROUP BY S.SHORTNAME "
                da = New OleDbDataAdapter(strsql, cn)
                da.Fill(dtdia)
                If dtdia.Rows.Count > 0 Then
                    For I As Integer = 0 To dtdia.Rows.Count - 1
                        Strstnpcs += dtdia.Rows(I).Item("SHORTNAME").ToString + " " + dtdia.Rows(I).Item("STNPCS").ToString + "/"
                        StrDiawt += dtdia.Rows(I).Item("DIAWT").ToString.Trim & "/"
                    Next
                    Strstnpcs = Strstnpcs.Remove(Strstnpcs.Length - 1)
                    StrDiawt = StrDiawt.Remove(StrDiawt.Length - 1)
                End If
                strsql = vbCrLf + " SELECT DISTINCT TOP 1 IM.SHORTNAME FROM " & cnAdminDb & "..ITEMTAG AS IT "
                strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMTAGSTONE  AS ITS ON ITS.TAGSNO=IT.SNO "
                strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID=ITS.STNITEMID"
                strsql += vbCrLf + " WHERE IT.TAGNO='" + dttagdetail.Rows(cnt).Item("TAGNO").ToString + "' AND IT.ITEMID='" + dttagdetail.Rows(cnt).Item("ITEMID").ToString + "'  ORDER BY IM.SHORTNAME DESC "
                da = New OleDbDataAdapter(strsql, cn)
                da.Fill(dtcolor)

                If dtcolor.Rows.Count > 0 Then
                    color = dtcolor.Rows(0).Item("SHORTNAME").ToString
                    If color = "EF" Then
                        Background_Image = Image.FromFile(Application.StartupPath & "\EF1.jpg")
                    ElseIf color = "DEF" Then
                        Background_Image = Image.FromFile(Application.StartupPath & "\DEF1.jpg")
                    ElseIf color = "GH" Then
                        Background_Image = Image.FromFile(Application.StartupPath & "\GH1.jpg")
                    End If
                Else
                    Background_Image = Image.FromFile(Application.StartupPath & "\GH1.jpg")
                End If
                'SIGN IMAGE
                If IO.File.Exists(Application.StartupPath & "\sign.jpg") Then
                    Strimgpath = Application.StartupPath & "\sign.jpg"
                    Sign_image = Image.FromFile(Strimgpath)
                ElseIf IO.File.Exists(Application.StartupPath & "\sign.png") Then
                    Strimgpath = Application.StartupPath & "\sign.png"
                    Sign_image = Image.FromFile(Strimgpath)
                End If
                e.Graphics.DrawImage(Background_Image, 0, 0, 793, 1122)
                e.Graphics.DrawString(dttagdetail.Rows(cnt).Item("ITEMID").ToString + "-" + dttagdetail.Rows(cnt).Item("TAGNO").ToString, New Drawing.Font("verdana", 13, FontStyle.Bold), Brushes.SaddleBrown, 530, 610)
                e.Graphics.DrawString(StrGrswt + " Gm", New Drawing.Font("verdana", 13, FontStyle.Bold), Brushes.SaddleBrown, 260, 610)
                e.Graphics.DrawString(Strstnpcs, New Drawing.Font("verdana", 13, FontStyle.Bold), Brushes.SaddleBrown, 270, 650)
                e.Graphics.DrawString(StrDiawt + " Ct", New Drawing.Font("verdana", 13, FontStyle.Bold), Brushes.SaddleBrown, 300, 695)
                e.Graphics.DrawImage(Sign_image, 300, 760, 150, 150)

                If Imagefilename = "" Then
                    bMap = My.Resources.no_photo
                Else
                    Dim Finfo As IO.FileInfo
                    Finfo = New IO.FileInfo(Imagefilename)
                    Dim fileStr As New IO.FileStream(Imagefilename, IO.FileMode.Open, IO.FileAccess.Read)
                    Try
                        bMap = Nothing
                        bMap = Bitmap.FromStream(fileStr)
                        Dim PicBoxAntique As New PictureBox
                        PicBoxAntique.Size = New Size(208, 162)
                        AutoImageSizer(bMap, PicBoxAntique, PictureBoxSizeMode.CenterImage)
                        e.Graphics.DrawImage(PicBoxAntique.Image, 560, 750, 100, 100)
                    Catch ex As Exception
                    Finally
                        fileStr.Close()
                    End Try
                End If
                If GCPagecountSale < Val(_dtcnt - 1) Then
                    GCPagecountSale = GCPagecountSale + 1
                    e.HasMorePages = True
                    Exit Sub
                ElseIf GCPagecountSale = Val(_dtcnt - 1) Then
                    e.HasMorePages = False
                    Exit Sub
                End If
            Next
        End If
    End Sub
#End Region
End Class