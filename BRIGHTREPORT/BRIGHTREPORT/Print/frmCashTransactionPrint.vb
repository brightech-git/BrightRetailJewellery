Imports System.Data.OleDb
Imports System.IO
Public Class frmCashTransactionPrint
#Region "VARIABLE"
    Dim strsql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dtPrint As DataTable
    Dim dtPrintCheque As DataTable
    Dim dtPrintChequeIssdate As DataTable
    Dim fontRegular As New Font("Palatino Linotype", 9, FontStyle.Regular)
    Dim fontRegular8 As New Font("Palatino Linotype", 8, FontStyle.Regular)
    Dim fontRegular7 As New Font("Palatino Linotype", 7, FontStyle.Regular)
    Dim fontBold As New Font("Palatino Linotype", 9, FontStyle.Bold)
    Dim fontUnderLine As New Font("Palatino Linotype", 9, FontStyle.Underline)
    Dim fontBoldTitle As New Font("Palatino Linotype", 11, FontStyle.Bold)
    Dim LeftFormat As New StringFormat(StringAlignment.Near)
    Dim RightFormat As New StringFormat(StringAlignment.Far)
    Dim CentreFormat As New StringFormat(StringAlignment.Center)

    Dim c0 As Integer = 60  ' 
    Dim c1 As Integer = 550  ' 
    Dim c2 As Integer = 650 ' 

    Public SilverBrush As SolidBrush = New SolidBrush(Color.Silver)
    Public BlackBrush As SolidBrush = New SolidBrush(Color.Black)

    Dim fontRegular6 As New Font("Palatino Linotype", 6, FontStyle.Regular)
    Dim fontitalic As New Font("Palatino Linotype", 8, FontStyle.Italic)
    Dim fontRegularsmall As New Font("Palatino Linotype", 8, FontStyle.Regular)
    Dim fontBoldsmall As New Font("Palatino Linotype", 7, FontStyle.Bold)
    Dim fontBoldUnderLine As New Font("Palatino Linotype", 9, FontStyle.Bold And FontStyle.Underline)
    Dim fontBoldHead As New Font("Palatino Linotype", 20, FontStyle.Bold)
    Dim fontBoldHead16 As New Font("Palatino Linotype", 16, FontStyle.Bold)
    Dim fontAmtInWord As New Font("Palatino Linotype", 7, FontStyle.Bold)

    Dim h1 As Integer = 250  ' Description
    Dim c3 As Integer = 120  ' Description
    Dim c4 As Integer = 350 ' Qty 
    Dim c5 As Integer = 450 ' Grs. Wt '430
    Dim c6 As Integer = 530 ' Less. Wt 510
    Dim c7 As Integer = 560 ' VA
    Dim c8 As Integer = 600 ' Rate
    Dim c9 As Integer = 680 ' MC
    Dim c10 As Integer = 750 ' ' Amount
    Dim c11 As Integer = 752 ' Empid
    Dim BOTTOM_POS As Integer = 960 'Ending POSITION
    Dim APPBOTTOM_POS As Integer = 800 'Approval Ending POSITION
    Dim TAPPBOTTOM_POS As Integer = 800 'Approval Ending POSITION
    Dim LINE_SPACE As Integer = 18
    Dim dt1 As New DataTable
    Dim PagecountSale As Integer = 0

    Dim START_POS As Integer = 250 ' TOP STARTING POSITION '135
    Dim TSTART_POS As Integer = 250 ' TOP STARTING POSITION
    Dim HLINEPOINT As Integer = 0




    Dim Topy As Integer = 120
    Dim systemName As String = Environment.MachineName
    Dim temp As String = System.Environment.GetEnvironmentVariable("temp")
    Dim barcode As New BarcodeLib.Barcode.Linear
    Dim Trantype As String = ""
    Dim finYear As String
    Dim count As Integer = 0
    Dim duplicateprint As Boolean = False
    Dim pBankName As String = ""
    Dim pAddressId As String = ""
    Dim prtcopy As String = ""
    Dim dtAddresssupplier As New DataTable
    Public chqIssuedate As Boolean = False
    Dim strCompanyGstin As String = ""
    Dim chqPaymode As String = ""
    Dim MNVOUCHERSIZE As String = "Y"
    Dim Print_Ver_No As String = "V.01.7.1"
#End Region

#Region "Constructor"
    Public Sub New(ByVal _Trantype As String, ByVal _dbName As String, ByVal _trandate As Date _
                   , ByVal tranno As String, ByVal _batchno As String, ByVal _tableName As String _
                   , ByVal _duplicatePrint As Boolean, ByVal chequePrintOnly As Boolean _
                   , ByVal adjdbName As String, ByVal _chqIssuedate As String, ByVal voucherPrint As Boolean, ByVal printName As String, Optional ByVal printcopy As String = "")

        InitializeComponent()

        prtcopy = printcopy

        duplicateprint = _duplicatePrint

        strsql = " SELECT GSTNO FROM " & cnAdminDb & "..COMPANY "
        strsql += vbCrLf + "WHERE COMPANYID='" & strCompanyId & "'"
        strCompanyGstin = objGPack.GetSqlValue(strsql).ToString

        finYear = cnStockDb.ToString

        Trantype = _Trantype
        strsql = " SELECT TRANNO"
        strsql += vbCrLf + " ,CONVERT(VARCHAR(15),TRANDATE,103) TRANDATE "
        strsql += vbCrLf + " ,'' TRANTYPE"
        strsql += vbCrLf + " ,TRANMODE"
        strsql += vbCrLf + " ,ACCODE"
        strsql += vbCrLf + " ,(SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = A.ACCODE) ACNAME"
        strsql += vbCrLf + " ,'' CHEQUEACNAME"
        strsql += vbCrLf + " ,ISNULL((SELECT ISNULL(BANKNAME,'') BANKACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = A.ACCODE),'') BANKACNAME"
        strsql += vbCrLf + " ,ISNULL((SELECT ISNULL(ADDRESS1,'') BANKACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = A.ACCODE),'') ADDRESSID"
        strsql += vbCrLf + " ,CASE WHEN TRANMODE = 'C' THEN (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = A.CONTRA) ELSE '' END CACNAME"
        strsql += vbCrLf + " ,CASE WHEN TRANMODE = 'D' THEN (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = A.CONTRA) ELSE '' END DACNAME"
        strsql += vbCrLf + " ,AMOUNT "
        strsql += vbCrLf + " ,PCS"
        strsql += vbCrLf + " ,CHQCARDNO,CARDID,CHQCARDREF"
        strsql += vbCrLf + " ,REMARK1,REMARK2"
        strsql += vbCrLf + " ,BATCHNO"
        strsql += vbCrLf + " , USERID"
        strsql += vbCrLf + " ,(SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = A.USERID) USERNAME"
        strsql += vbCrLf + " ,UPDATED,SUBSTRING(CONVERT(VARCHAR,UPTIME,109),13,5)+SUBSTRING(CONVERT(VARCHAR,UPTIME,109),25,5) UPTIME"
        strsql += vbCrLf + " ,SYSTEMID "
        strsql += vbCrLf + " ,1 AS RESULT "
        strsql += vbCrLf + " FROM " & _dbName & ".." & _tableName & " AS A WHERE " 'ACCTRAN
        strsql += vbCrLf + " TRANDATE ='" & Format(_trandate, "yyyy-MM-dd") & "' "
        strsql += vbCrLf + " AND TRANNO = " & tranno & " "
        ''strsql += vbCrLf + " AND TRANTYPE = '" & _Trantype & "' "
        strsql += vbCrLf + " AND BATCHNO = '" & _batchno & "'"
        strsql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y' "
        dtPrint = New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtPrint)

        Dim pchqName As String = ""
        Dim pchqAmt As String = ""
        Dim pchqDate As String = ""
        Dim psupplier As String = ""


        If dtPrint.Rows.Count > 0 Then
            For i As Integer = 0 To dtPrint.Rows.Count - 1
                strsql = "SELECT UPPER(ISNULL(ACTYPE,'')) ACTYPE FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & dtPrint.Rows(i).Item("ACCODE").ToString & "'"
                If objGPack.GetSqlValue(strsql) = "O" Or objGPack.GetSqlValue(strsql) = "1" Or objGPack.GetSqlValue(strsql) = "2" Then
                    pchqName = dtPrint.Rows(i).Item("ACNAME").ToString
                    If dtPrint.Rows(i).Item("CHEQUEACNAME").ToString <> "" Then
                        pchqName = dtPrint.Rows(i).Item("CHEQUEACNAME").ToString
                    End If
                    pBankName = dtPrint.Rows(i).Item("BANKACNAME").ToString
                Else
                    psupplier = dtPrint.Rows(i).Item("ACNAME").ToString
                    If dtPrint.Rows(i).Item("CHEQUEACNAME").ToString <> "" Then
                        psupplier = dtPrint.Rows(i).Item("CHEQUEACNAME").ToString
                    End If
                    If objGPack.GetSqlValue(strsql) = "C" Or objGPack.GetSqlValue(strsql) = "D" Or objGPack.GetSqlValue(strsql) = "B" Or objGPack.GetSqlValue(strsql) = "G" Or objGPack.GetSqlValue(strsql) = "E" Then
                        strsql = " SELECT * FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = '" & dtPrint.Rows(i).Item("ACCODE").ToString & "' "
                        da = New OleDbDataAdapter(strsql, cn)
                        dtAddresssupplier = New DataTable
                        da.Fill(dtAddresssupplier)
                    End If
                End If
            Next

            Dim drChequebook As DataRow = Nothing
            '    strsql = " SELECT amount,CONVERT(VARCHAR(15),chqdate,103) chqdate,BANKCODE "
            '    strsql += vbCrLf + " ,ISNULL((SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = A.BANKCODE),'') BANKNAME"
            '    strsql += vbCrLf + " ,ISNULL((SELECT ISNULL(CHEQUEACNAME,'') CHEQUEACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ID = A.TOPARTY),'') SUPPLIERCHQACNAME"
            '    strsql += vbCrLf + " ,ISNULL((SELECT ISNULL(ACNAME,'') FROM " & cnAdminDb & "..ACHEAD WHERE ID = A.TOPARTY),'') SUPPLIERACNAME"
            '    strsql += vbCrLf + " ,ISNULL(INFAVOUROF,'') INFAVOUROF"
            '    strsql += vbCrLf + " FROM " & _dbName & "..CHEQUEBOOK AS A "
            '    strsql += vbCrLf + " WHERE CHQENTRYDATE = '" & Format(_trandate, "yyyy-MM-dd") & "' "
            '    strsql += vbCrLf + " AND BATCHNO = '" & _batchno & "' "
            '    drChequebook = GetSqlRow(strsql, cn)
            '    If Not drChequebook Is Nothing Then
            '        pchqAmt = Format(drChequebook.Item("amount"), "0.00")
            '        pchqDate = drChequebook.Item("chqdate")
            '        psupplier = drChequebook.Item("SUPPLIERACNAME")
            '        If drChequebook.Item("SUPPLIERCHQACNAME").ToString <> "" Then
            '            psupplier = drChequebook.Item("SUPPLIERCHQACNAME")
            '        End If
            '        If drChequebook.Item("INFAVOUROF").ToString <> "" Then
            '            psupplier = drChequebook.Item("INFAVOUROF")
            '        End If
            '        pchqName = drChequebook.Item("BANKNAME")
            '    Else
            '        pchqAmt = dtPrint.Rows(0).Item("AMOUNT").ToString
            '        pchqDate = dtPrint.Rows(0).Item("TRANDATE").ToString
            '    End If
        End If

        ''Dim saledate As String = GetBilldate()
        Dim saledate As String = _trandate

        'strsql = ""
        'strsql += vbCrLf + " SELECT TRANNO"
        'strsql += vbCrLf + " ,CONVERT(vARCHAR(15),TRANDATE,103)  TRANDATE "
        'strsql += vbCrLf + " ,(SELECT TRANTYPENAME FROM " & cnAdminDb & "..TRANTYPEMAST WHERE TRANTYPE =A.TRANTYPE) TRANTYPENAME"
        'strsql += vbCrLf + " ,CONVERT(VARCHAR(15),REFDATE,103) REFDATE"
        'strsql += vbCrLf + " ,CASE WHEN REFNO = '' THEN '9999999999' ELSE REFNO END REFNO"
        'strsql += vbCrLf + " ,OTRANNO"
        'strsql += vbCrLf + " ,OTRANDATE"
        'strsql += vbCrLf + " , " & cnStockDb & "  DBNAME"
        'strsql += vbCrLf + " ,CASE WHEN SUM(AMOUNT) <0 THEN   CONVERT(NUMERIC(15,2),-1* SUM(AMOUNT)) ELSE CONVERT(NUMERIC(15,2),SUM(AMOUNT)) END AMOUNT "
        'strsql += vbCrLf + " ,CONVERT(VARCHAR(2),(CASE WHEN RECPAY = 'R' THEN 'Cr' ELSE 'Dr' END)) DC1 "
        ''strsql += vbCrLf + " ,CONVERT(VARCHAR(2),(CASE WHEN TRANTYPE IN('PU','PE') THEN 'Cr' ELSE 'Dr' END)) DC "
        'strsql += vbCrLf + " ,CONVERT(VARCHAR(2),(CASE WHEN RECPAY = 'R' THEN 'Cr' ELSE 'Dr' END)) DC "
        ''strsql += vbCrLf + " ,CONVERT(VARCHAR(2),(CASE WHEN RECPAY = 'P' THEN 'Cr' ELSE 'Dr' END)) DC "
        'strsql += vbCrLf + " ,A.TRANTYPE "
        'strsql += vbCrLf + " ,A.RECPAY "
        'strsql += vbCrLf + " ,CASE WHEN TRANTYPE IN ('PU') THEN 1 "
        'strsql += vbCrLf + " ELSE CASE WHEN TRANTYPE IN('PR','DG') THEN 2 "
        'strsql += vbCrLf + " ELSE CASE WHEN TRANTYPE IN('PE') THEN 3 "
        'strsql += vbCrLf + " ELSE 4 END END END DISORDER"
        'strsql += vbCrLf + " FROM " & cnAdminDb & ".." & adjdbName & " AS A" 'AFT_ADJTRAN
        'strsql += vbCrLf + " WHERE TRANDATE = '" & Format(_trandate, "yyyy-MM-dd") & "' "
        'strsql += vbCrLf + " AND BATCHNO = '" & _batchno & "' "
        'strsql += vbCrLf + " GROUP BY TRANNO,TRANDATE,REFDATE,REFNO,RECPAY,A.TRANTYPE,OTRANNO,OTRANDATE"
        'strsql += vbCrLf + " ORDER BY REFNO,REFDATE,DISORDER "
        'dtPrintCheque = New DataTable
        'da = New OleDbDataAdapter(strsql, cn)
        'da.Fill(dtPrintCheque)
        ''If dtPrintCheque.Rows.Count > 0 Then
        '    For i As Integer = 0 To dtPrintCheque.Rows.Count - 1
        '        With dtPrintCheque.Rows(i)
        '            If .Item("TRANTYPE").ToString = "PR" And .Item("RECPAY").ToString = "R" Then
        '                strsql = " SELECT DISTINCT BATCHNO FROM " & .Item("DBNAME").ToString & "..RECEIPTRTN "
        '                strsql += vbCrLf + " WHERE ENTREFDATE = '" & Format(.Item("OTRANDATE"), "yyyy-MM-dd") & "' "
        '                strsql += vbCrLf + " AND RECNO = '" & .Item("OTRANNO") & "' "
        '                Dim recBatchno As String = objGPack.GetSqlValue(strsql).ToString
        '                If recBatchno <> "" Then
        '                    strsql = " SELECT COUNT(*) CNT FROM " & .Item("DBNAME").ToString & "..RECEIPT "
        '                    strsql += vbCrLf + " WHERE  BATCHNO = '" & recBatchno & "'"
        '                    Dim cnt As Integer = Val(objGPack.GetSqlValue(strsql))
        '                    If cnt > 0 Then
        '                        dtPrintCheque.Rows(i)("TRANTYPENAME") = "DEBIT NOTE"
        '                    End If
        '                End If
        '            End If
        '            If .Item("REFNO").ToString = "9999999999" Then
        '                dtPrintCheque.Rows(i)("REFNO") = ""
        '            End If
        '        End With
        '    Next
        '    Dim dr As DataRow = Nothing

        '    dr = dtPrintCheque.NewRow
        '    dr!TRANTYPENAME = " TOTAL"
        '    Dim credit As Double = 0
        '    Dim debit As Double = 0
        '    Dim cdAmt As Double = 0
        '    credit = Val(dtPrintCheque.Compute("SUM(AMOUNT)", "DC='Cr'").ToString)
        '    debit = Val(dtPrintCheque.Compute("SUM(AMOUNT)", "DC='Dr'").ToString)
        '    cdAmt = credit - debit
        '    If cdAmt < 0 Then
        '        cdAmt = -1 * cdAmt
        '    End If
        '    dr!AMOUNT = Format(cdAmt, "0.00") 'pchqAmt
        '    dtPrintCheque.Rows.Add(dr)
        'End If

        'chqIssuedate = False
        'If _chqIssuedate <> "" Then
        '    strsql = ""
        '    strsql += vbCrLf + "    SELECT  CHQNUMBER,CHQISSUEDATE,B.ACNAME,A.AMOUNT,CHQDATE"
        '    If dtPrint.Rows.Count > 0 Then
        '        strsql += vbCrLf + "   ,'" & dtPrint.Rows(0).Item("CHQCARDREF").ToString & "' CHQCARDREF "
        '    Else
        '        strsql += vbCrLf + "   ,'' CHQCARDREF "
        '    End If
        '    strsql += vbCrLf + "    FROM " & _dbName & "..CHEQUEBOOK AS A "
        '    strsql += vbCrLf + "    , " & cnAdminDb & "..AFA_ACHEAD AS B"
        '    strsql += vbCrLf + "    WHERE  A.BANKCODE = B.ACCODE "
        '    strsql += vbCrLf + "    AND CHQENTRYDATE = '" & Format(_trandate, "yyyy-MM-dd") & "' "
        '    strsql += vbCrLf + "    AND  CHQISSUEDATE = '" & _chqIssuedate & "'"
        '    strsql += vbCrLf + "    AND BATCHNO = '" & _batchno & "' "
        '    da = New OleDbDataAdapter(strsql, cn)
        '    dtPrintChequeIssdate = New DataTable
        '    da.Fill(dtPrintChequeIssdate)
        '    chqIssuedate = True

        '    strsql = " SELECT  DISTINCT PAYMODE FROM " & _dbName & "..ART_PAYMENTDET "
        '    strsql += vbCrLf + " WHERE CHQDATE = '" & Format(_trandate, "yyyy-MM-dd") & "' "
        '    strsql += vbCrLf + " AND CHQBATCHNO = '" & _batchno & "' "
        '    chqPaymode = objGPack.GetSqlValue(strsql).ToString
        '    If chqPaymode = "CH" Or chqPaymode = "" Then
        '        chqPaymode = "Cheque"
        '    ElseIf chqPaymode = "RT" Then
        '        chqPaymode = "RTGS"
        '    End If

        'Else
        '    dtPrintChequeIssdate = New DataTable
        'End If
        If dtPrint.Rows.Count > 0 Then
            'Dim obj1 As New frmPrinterSelect
            'obj1.ShowDialog()
            'If chequePrintOnly = True And voucherPrint = False Then
            '    If MsgBox("Do you want Cheque Print ? ", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton1) = MsgBoxResult.Yes Then
            '        If pchqAmt > 0 Then
            '            Dim obj1111 As New frmChqLayout(pchqName, pchqAmt, pchqDate, psupplier, obj.cmbrecprinter.Text) '
            '        End If
            '    End If
            '    Exit Sub
            'End If
            PrintDialog1.Document = PrintDocument1
            If printName = "" Then MsgBox("Print Name Not Select", MsgBoxStyle.Information) : Exit Sub
            PrintDocument1.PrinterSettings.PrinterName = printName
            PrintDocument1.PrinterSettings.Copies = 1
            PrintDocument1.PrintController = New System.Drawing.Printing.StandardPrintController
            PrintDocument1.Print()
            'If chequePrintOnly = True Then
            '    If MsgBox("Do you want Cheque Print ? ", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2) = MsgBoxResult.Yes Then
            '        If pchqAmt > 0 Then
            '            ''Dim obj1111 As New frmChqLayout(pchqName, pchqAmt, pchqDate, psupplier, obj.cmbrecprinter.Text)
            '        End If
            '    End If
            'End If
        End If
    End Sub
#End Region

#Region "Print Doc"
    Private Sub PrintDocument1_PrintPage(sender As System.Object, e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        If Trantype = "CR" Then
            PrintDesignRECEIPT(e.Graphics, e, False)
        ElseIf Trantype = "CP" Or Trantype = "MN" Then
            PrintDesignPAYMENT(e.Graphics, e, False)
        ElseIf Trantype = "DN" Or Trantype = "CN" Then
            PrintDesignCNDN(e.Graphics, e, False)
        ElseIf Trantype = "JE" Then
            PrintDesignJOURNAL(e.Graphics, e, False)
        ElseIf Trantype = "TA" Then
            PrintDesignJOURNAL(e.Graphics, e, False)
        End If
    End Sub
#End Region

#Region "User Define Function"

    Private Sub PrintDesignJOURNAL(ByVal g1 As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal copy As Boolean)
        DetailJOURNAL(g1, e)
        TileJOURNAL(g1, e)
        Topy = Topy + 18
        If dtPrint.Rows.Count > 0 Then
            For i As Integer = 0 To dtPrint.Rows.Count - 1
                If dtPrint.Rows(i).Item("ACNAME").ToString = "" Then Continue For
                g1.DrawString(dtPrint.Rows(i).Item("ACNAME").ToString, fontRegular, Brushes.Black, c0, Topy)
                If dtPrint.Rows(i).Item("TRANMODE").ToString = "D" Then
                    g1.DrawString(dtPrint.Rows(i).Item("AMOUNT").ToString, fontRegular, Brushes.Black, c1, Topy, LeftFormat) 'DEBIT
                Else
                    g1.DrawString(dtPrint.Rows(i).Item("AMOUNT").ToString, fontRegular, Brushes.Black, c2, Topy, LeftFormat) 'CREDIT
                End If
                Topy = Topy + 20
            Next
        End If
        g1.DrawLine(Pens.Black, 20, Topy, 775, Topy)
        g1.DrawString(Format(Val(dtPrint.Compute("SUM(AMOUNT)", "TRANMODE = 'C'").ToString), "0.00"), fontRegular, Brushes.Black, c1, Topy, LeftFormat) 'CREDIT
        g1.DrawString(Format(Val(dtPrint.Compute("SUM(AMOUNT)", "TRANMODE = 'D'").ToString), "0.00"), fontRegular, Brushes.Black, c2, Topy, LeftFormat) 'DEBIT
        Topy = Topy + 20
        g1.DrawLine(Pens.Black, 20, Topy, 775, Topy)
        Topy = Topy + 5
        If dtPrint.Rows(0).Item("REMARK1").ToString <> "" Then
            Topy = Topy + 18
            g1.DrawString(dtPrint.Rows(0).Item("REMARK1").ToString, fontRegular, Brushes.Black, c0, Topy)
        End If
        If dtPrint.Rows(0).Item("REMARK2").ToString <> "" Then
            Topy = Topy + 18
            g1.DrawString(dtPrint.Rows(0).Item("REMARK2").ToString, fontRegular, Brushes.Black, c0, Topy)
        End If
        Topy = Topy + 18
        'If dtPrintCheque.Rows.Count > 0 Then
        '    'If dtPrintCheque.Rows(0).Item("CHQNUMBER").ToString <> "" Then
        '    '    g1.DrawString("CHQ NO : " & dtPrintCheque.Rows(0).Item("CHQNUMBER").ToString, fontRegular, Brushes.Black, c0, Topy)
        '    '    Topy = Topy + 18
        '    'End If
        '    'g1.DrawString("CHQ DT : " & dtPrintCheque.Rows(0).Item("CHQISSUEDATE").ToString, fontRegular, Brushes.Black, c0, Topy)
        '    'Topy = Topy + 18
        '    printadjutran(g1, e, "JOURNAL DETAILS")
        'End If
        footer(g1, e)
    End Sub


    Public Sub DetailJOURNAL(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        Dim imgMemoryStream As MemoryStream = New MemoryStream()
        '' imgMemoryStream = funcPrintLOGO()
        'imgMemoryStream = 0
        'Dim Image As Image = Image.FromStream(imgMemoryStream)
        'e.Graphics.DrawImage(Image, 170, 0, 500, 130)

        Dim Strimgpath As String = ""
        Dim logo As Image
        If IO.File.Exists(Application.StartupPath & "\VBJ_LOGO.jpg") Then
            Strimgpath = Application.StartupPath & "\VBJ_LOGO.jpg"
            logo = Image.FromFile(Strimgpath)
            e.Graphics.DrawImage(logo, c8 + 40, 20, 120, 70)
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
            'g.DrawString(printcpy, fontBoldTitle, Brushes.Black, c4, 20, LeftFormat)
            'g.DrawString("TAX INVOICE", fontBoldTitle, Brushes.Black, c4, 40, LeftFormat)
            'g.DrawString(StrConv(dtcomp.Rows(0).Item("COMPANYNAME").ToString, VbStrConv.ProperCase), fontBoldHead, BlackBrush, c4 - 150, 20, LeftFormat)
            'g.DrawString(dtcomp.Rows(0).Item("ADDRESS1").ToString, fontRegular, BlackBrush, c4 - 80, 50, LeftFormat)
            'g.DrawString(dtcomp.Rows(0).Item("ADDRESS2").ToString, fontRegular, BlackBrush, c4 - 80, 70, LeftFormat)
            'g.DrawString("GSTIN :" & dtcomp.Rows(0).Item("GSTNO").ToString, fontRegular, BlackBrush, c4 - 70, 90, LeftFormat)
            g.DrawString(Print_Ver_No.ToString, fontRegular6, SilverBrush, c9 + 10, 5, LeftFormat)
            g.DrawString(StrConv(strCompanyName.ToString, VbStrConv.Uppercase), fontBoldHead16, BlackBrush, c4 - 150, 20, LeftFormat)
            g.DrawString(dtcomp.Rows(0).Item("ADDRESS1").ToString + "," + dtcomp.Rows(0).Item("ADDRESS2").ToString, fontRegular, BlackBrush, c4 - 95, 50, LeftFormat)
            g.DrawString(dtcomp.Rows(0).Item("ADDRESS3").ToString + " - " + dtcomp.Rows(0).Item("ADDRESS4").ToString + " - " + dtcomp.Rows(0).Item("PHONE").ToString, fontRegular, BlackBrush, c4 - 95, 70, LeftFormat)
            g.DrawString("GSTIN :" & dtcomp.Rows(0).Item("GSTNO").ToString, fontRegular, BlackBrush, c4 - 35, 90, LeftFormat)
            'g.DrawString("STATE :" & Statename.ToString, fontBold, BlackBrush, c7, 130, LeftFormat)
            'g.DrawString("Phone :" & dtcomp.Rows(0).Item("PHONE").ToString, fontBold, BlackBrush, c4 - 50, 145, LeftFormat)
            ''g.DrawString("PANNO :" & dtcomp.Rows(0).Item("PANNO").ToString, fontBold, BlackBrush, c7, 145, LeftFormat)
        End If

        Topy = Topy + 30
        g.DrawString(prtcopy, fontBoldTitle, Brushes.Black, 300, Topy)
        Topy = Topy + 15
        If Trantype = "CR" Then
            g.DrawString("RECEIPT VOUCHER ", fontBoldTitle, Brushes.Black, 300, Topy)
            Topy = Topy + 15
        ElseIf Trantype = "CP" Then
            g.DrawString("PAYMENT VOUCHER ", fontBoldTitle, Brushes.Black, 300, Topy)
            Topy = Topy + 15
        ElseIf Trantype = "JE" Then
            g.DrawString("JOURNAL VOUCHER", fontBoldTitle, Brushes.Black, 300, Topy)
            Topy = Topy + 15
        ElseIf Trantype = "MN" Then
            g.DrawString("MEMORANDUM ENTRY", fontBoldTitle, Brushes.Black, 300, Topy)
            Topy = Topy + 15
        ElseIf Trantype = "TA" Then
            g.DrawString("TRANSFER ACCOUNT", fontBoldTitle, Brushes.Black, 300, Topy)
            Topy = Topy + 15
        End If

        Dim t1 As Integer = 550
        Dim t2 As Integer = 620
        Dim t3 As Integer = 650

        g.DrawString("Voucher No ", fontRegular, Brushes.Black, t1, Topy)
        g.DrawString(": ", fontRegular, Brushes.Black, t2, Topy)
        g.DrawString(cnCostId & "/" & finYear & "/" & dtPrint.Rows(0).Item("TRANNO"), fontRegular, Brushes.Black, t3, Topy)
        Topy = Topy + 18

        g.DrawString("Date ", fontRegular, Brushes.Black, t1, Topy)
        g.DrawString(": ", fontRegular, Brushes.Black, t2, Topy)
        g.DrawString("" & dtPrint.Rows(0).Item("TRANDATE"), fontRegular, Brushes.Black, t3, Topy)

        Topy = Topy + 18
        'g.DrawString("System", fontRegular, Brushes.Black, t1, Topy)
        'g.DrawString(": ", fontRegular, Brushes.Black, t2, Topy)
        'g.DrawString("" & dtPrint.Rows(0).Item("SYSTEMID"), fontRegular, Brushes.Black, t3, Topy)
        'Topy = Topy + 40
    End Sub

    Public Sub TileJOURNAL(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        Dim rAlign As New StringFormat
        rAlign.Alignment = StringAlignment.Far
        g.DrawLine(Pens.Black, 20, Topy, 775, Topy)
        Topy = Topy + 5
        g.DrawString("Particulars ", fontRegular, Brushes.Black, c0, Topy)
        g.DrawString("Debit ", fontRegular, Brushes.Black, c1, Topy, rAlign)
        g.DrawString("Credit ", fontRegular, Brushes.Black, c2, Topy, rAlign)
        Topy = Topy + 30
        g.DrawLine(Pens.Black, 20, Topy, 775, Topy)
    End Sub

    Private Sub PrintDesignPAYMENT(ByVal g1 As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal copy As Boolean)
        Dim rAlign As New StringFormat
        rAlign.Alignment = StringAlignment.Far
        DetailPAYMENT(g1, e)
        If chqIssuedate = False Then
            TilePAYMENT(g1, e)
            Topy = Topy + 18
            If count = 0 Then
                If dtPrint.Rows.Count > 0 Then
                    For i As Integer = 0 To dtPrint.Rows.Count - 1
                        If Trantype = "CN" Then
                            If dtPrint.Rows(i).Item("DACNAME").ToString = "" Then Continue For
                            If dtPrint.Rows(i).Item("TRANMODE").ToString = "C" Then Continue For
                            g1.DrawString(dtPrint.Rows(i).Item("ACNAME") & " " & pBankName.Trim, fontRegular, Brushes.Black, c0, Topy)
                            g1.DrawString(dtPrint.Rows(i).Item("AMOUNT").ToString, fontRegular, Brushes.Black, c2, Topy, rAlign) 'DEBIT
                        Else
                            If dtPrint.Rows(i).Item("ACNAME").ToString = "" Then Continue For
                            If dtPrint.Rows(i).Item("TRANMODE").ToString = "C" Then Continue For
                            g1.DrawString(dtPrint.Rows(i).Item("DACNAME") & " " & pBankName.Trim, fontRegular, Brushes.Black, c0, Topy)
                            g1.DrawString(dtPrint.Rows(i).Item("AMOUNT").ToString, fontRegular, Brushes.Black, c2, Topy, rAlign) 'DEBIT
                        End If

                        Topy = Topy + 20
                    Next
                End If
                Topy = Topy + 20
                g1.DrawLine(Pens.Black, 40, Topy, 760, Topy)
                Topy = Topy + 20
                g1.DrawString(Format(Val(dtPrint.Compute("SUM(AMOUNT)", "TRANMODE = 'D'").ToString), "0.00"), fontRegular, Brushes.Black, c2, Topy, rAlign) 'DEBIT
                Topy = Topy + 20
                g1.DrawLine(Pens.Black, 40, Topy, 760, Topy)
                Dim Amount As Double = 0
                Amount = Val(dtPrint.Compute("SUM(AMOUNT)", "TRANMODE = 'D'").ToString)
                Dim AmountInWord As String = ConvertRupees.RupeesToWord(Amount, "Rupees", "Only")
                g1.DrawString("(" & AmountInWord & ")", fontRegular, Brushes.Black, c0, Topy, LeftFormat) 'DEBIT
                Topy = Topy + 18
                If dtPrint.Rows(0).Item("REMARK1").ToString <> "" Then
                    Topy = Topy + 18
                    g1.DrawString(dtPrint.Rows(0).Item("REMARK1").ToString, fontRegular, Brushes.Black, c0, Topy)
                End If
                If dtPrint.Rows(0).Item("REMARK2").ToString <> "" Then
                    Topy = Topy + 18
                    g1.DrawString(dtPrint.Rows(0).Item("REMARK2").ToString, fontRegular, Brushes.Black, c0, Topy)
                End If
                Topy = Topy + 10
            End If
        End If
        ''printadjutran(g1, e, "PAYMENT DETAILS")
        ''Topy = Topy + 20
        ''printchequebook(g1, e, "")
        footer(g1, e)
    End Sub

    Private Sub PrintDesignCNDN(ByVal g1 As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal copy As Boolean)
        Dim rAlign As New StringFormat
        rAlign.Alignment = StringAlignment.Far
        Dim _remark_flag As Boolean = False
        Dim _remark_amount As Double = 0
        Dim _gst_c_amount As Double = 0
        Dim _gst_s_amount As Double = 0
        Dim _gst_i_amount As Double = 0
        Dim _gst_TD_amount As Double = 0
        Dim _gst_TC_amount As Double = 0
        Dim gst As String = ""
        DetailCNDN(g1, e)
        If chqIssuedate = False Then
            TilePAYMENT(g1, e)
            Topy = Topy + 18
            If count = 0 Then
                If dtPrint.Rows.Count > 0 Then
                    For i As Integer = 0 To dtPrint.Rows.Count - 1
                        If Trantype = "CN" Then
                            If dtPrint.Rows(i).Item("DACNAME").ToString = "" Then Continue For
                            If dtPrint.Rows(i).Item("TRANMODE").ToString = "C" Then Continue For
                            'g1.DrawString(dtPrint.Rows(i).Item("ACNAME") & " " & pBankName.Trim, fontRegular, Brushes.Black, c0, Topy)
                            'g1.DrawString(dtPrint.Rows(i).Item("AMOUNT").ToString, fontRegular, Brushes.Black, c2, Topy, rAlign) 'CREDIT
                            If dtPrint.Rows(i).Item("ACNAME").ToString.Contains("CENTRAL GST") Then
                                _gst_c_amount = _gst_c_amount + Val(dtPrint.Rows(i).Item("AMOUNT").ToString)
                                gst = "OWN"
                            ElseIf dtPrint.Rows(i).Item("ACNAME").ToString.Contains("STATE GST") Then
                                _gst_s_amount = _gst_s_amount + Val(dtPrint.Rows(i).Item("AMOUNT").ToString)
                                gst = "OWN"
                            ElseIf dtPrint.Rows(i).Item("ACNAME").ToString.Contains("INTEGRATED GST") Then
                                gst = "IGST"
                                _gst_i_amount = _gst_i_amount + Val(dtPrint.Rows(i).Item("AMOUNT").ToString)
                            ElseIf dtPrint.Rows(i).Item("ACNAME").ToString.Contains("TDS") Then
                                gst = "TDS"
                                _gst_TD_amount = _gst_TD_amount + Val(dtPrint.Rows(i).Item("AMOUNT").ToString)
                            ElseIf dtPrint.Rows(i).Item("ACNAME").ToString.Contains("TCS") Then
                                gst = "TCS"
                                _gst_TC_amount = _gst_TC_amount + Val(dtPrint.Rows(i).Item("AMOUNT").ToString)
                            Else
                                'g1.DrawString(dtPrint.Rows(i).Item("AMOUNT").ToString, fontRegular, Brushes.Black, c2, Topy, rAlign) 'DEBIT
                                _remark_amount = _remark_amount + Val(dtPrint.Rows(i).Item("AMOUNT").ToString)
                            End If
                        Else
                            If dtPrint.Rows(i).Item("ACNAME").ToString = "" Then Continue For
                            If dtPrint.Rows(i).Item("TRANMODE").ToString = "C" Then Continue For
                            'g1.DrawString(dtPrint.Rows(i).Item("DACNAME") & " " & pBankName.Trim, fontRegular, Brushes.Black, c0, Topy)
                            'g1.DrawString(dtPrint.Rows(i).Item("AMOUNT").ToString, fontRegular, Brushes.Black, c2, Topy, rAlign) 'DEBIT
                            If dtPrint.Rows(i).Item("DACNAME").ToString.Contains("CENTRAL GST") Then
                                _gst_c_amount = _gst_c_amount + Val(dtPrint.Rows(i).Item("AMOUNT").ToString) 'CREDIT
                                gst = "OWN"
                            ElseIf dtPrint.Rows(i).Item("DACNAME").ToString.Contains("STATE GST") Then 'CREDIT
                                _gst_s_amount = _gst_s_amount + Val(dtPrint.Rows(i).Item("AMOUNT").ToString) 'CREDIT
                                gst = "OWN"
                            ElseIf dtPrint.Rows(i).Item("DACNAME").ToString.Contains("INTEGRATED GST") Then 'CREDIT
                                gst = "IGST"
                                _gst_i_amount = _gst_i_amount + Val(dtPrint.Rows(i).Item("AMOUNT").ToString) 'CREDIT
                            Else
                                'g1.DrawString(dtPrint.Rows(i).Item("AMOUNT").ToString, fontRegular, Brushes.Black, c2, Topy, rAlign) 'DEBIT
                                _remark_amount = _remark_amount + Val(dtPrint.Rows(i).Item("AMOUNT").ToString) 'CREDIT
                            End If
                        End If
                        'Topy = Topy + 20
                    Next
                End If

                If dtPrint.Rows(0).Item("REMARK1").ToString <> "" Then
                    g1.DrawString(dtPrint.Rows(0).Item("REMARK1").ToString, fontRegular, Brushes.Black, c0, Topy)
                    g1.DrawString(_remark_amount.ToString, fontRegular, Brushes.Black, c2, Topy, rAlign)
                    Topy = Topy + 20
                End If
                If dtPrint.Rows(0).Item("REMARK2").ToString <> "" Then
                    g1.DrawString(dtPrint.Rows(0).Item("REMARK2").ToString, fontRegular, Brushes.Black, c0, Topy)
                End If

                Topy = Topy + 40
                If gst = "OWN" Then
                    g1.DrawString("SGST ", fontRegular, Brushes.Black, c1, Topy)
                    g1.DrawString(_gst_s_amount, fontRegular, Brushes.Black, c2, Topy, rAlign)
                    Topy = Topy + 20
                    g1.DrawString("CGST ", fontRegular, Brushes.Black, c1, Topy)
                    g1.DrawString(_gst_c_amount, fontRegular, Brushes.Black, c2, Topy, rAlign)
                ElseIf gst = "IGST" Then
                    g1.DrawString(gst, fontRegular, Brushes.Black, c1, Topy)
                    g1.DrawString(_gst_i_amount, fontRegular, Brushes.Black, c2, Topy, rAlign)
                ElseIf gst = "TDS" Then
                    g1.DrawString(gst, fontRegular, Brushes.Black, c1, Topy)
                    g1.DrawString(_gst_TD_amount, fontRegular, Brushes.Black, c2, Topy, rAlign)
                ElseIf gst = "TCS" Then
                    g1.DrawString(gst, fontRegular, Brushes.Black, c1, Topy)
                    g1.DrawString(_gst_TC_amount, fontRegular, Brushes.Black, c2, Topy, rAlign)
                End If
                Topy = Topy + 20
                g1.DrawLine(Pens.Black, 40, Topy, 760, Topy)
                Topy = Topy + 20
                g1.DrawString("Total ", fontRegular, Brushes.Black, c1 - 100, Topy, rAlign)
                g1.DrawString(Format(Val(dtPrint.Compute("SUM(AMOUNT)", "TRANMODE = 'D'").ToString), "0.00"), fontRegular, Brushes.Black, c2, Topy, rAlign) 'DEBIT
                Topy = Topy + 20
                g1.DrawLine(Pens.Black, 40, Topy, 760, Topy)
                Dim Amount As Double = 0
                Amount = Val(dtPrint.Compute("SUM(AMOUNT)", "TRANMODE = 'D'").ToString)
                Dim AmountInWord As String = ConvertRupees.RupeesToWord(Amount, "Rupees", "Only")
                g1.DrawString("(" & AmountInWord & ")", fontRegular, Brushes.Black, c0, Topy, LeftFormat) 'DEBIT
                Topy = Topy + 18
                'If dtPrint.Rows(0).Item("REMARK1").ToString <> "" Then
                '    Topy = Topy + 18
                '    g1.DrawString(dtPrint.Rows(0).Item("REMARK1").ToString, fontRegular, Brushes.Black, c0, Topy)
                'End If
                'If dtPrint.Rows(0).Item("REMARK2").ToString <> "" Then
                '    Topy = Topy + 18
                '    g1.DrawString(dtPrint.Rows(0).Item("REMARK2").ToString, fontRegular, Brushes.Black, c0, Topy)
                'End If
                'Topy = Topy + 10
            End If
        End If
        ''printadjutran(g1, e, "PAYMENT DETAILS")
        ''Topy = Topy + 20
        ''printchequebook(g1, e, "")
        footerCNDN(g1, e)
    End Sub

    Public Sub printadjutran(ByVal g1 As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal billtype As String)
        ''If dtPrintCheque.Rows.Count > 0 Then
        'Topy = Topy + 18
        Dim A0 As Integer = 50 'Type
            Dim A1 As Integer = 250 'Invoice No
            Dim A2 As Integer = 340 'Invdate No
            Dim A3 As Integer = 440 'Recno
            Dim A4 As Integer = 520 'Recdate
            Dim A5 As Integer = 660 'Dr
            Dim A6 As Integer = 745 'Cr

            Dim rAlign As New StringFormat
            rAlign.Alignment = StringAlignment.Far

            g1.DrawString(billtype, fontBoldTitle, Brushes.Black, 300, Topy)
            Topy = Topy + 20
            g1.DrawLine(Pens.Black, A0, Topy, 750, Topy)
            Dim topyH1 As Integer = Topy
            Topy = Topy + 5

            g1.DrawString("Type ", fontRegular, Brushes.Black, A0, Topy)
            g1.DrawString("Inv No. ", fontRegular, Brushes.Black, A1, Topy)
            g1.DrawString("Inv Date ", fontRegular, Brushes.Black, A2, Topy)
            g1.DrawString("Ref No. ", fontRegular, Brushes.Black, A3, Topy)
            g1.DrawString("Ref Date ", fontRegular, Brushes.Black, A4, Topy)
            g1.DrawString("Dr. ", fontRegular, Brushes.Black, A5, Topy, rAlign)
            g1.DrawString("Cr. ", fontRegular, Brushes.Black, A6, Topy, rAlign)
            Topy = Topy + 18
            g1.DrawLine(Pens.Black, A0, Topy, 750, Topy)
            Topy = Topy + 8
            For count = count To dtPrintCheque.Rows.Count - 1
                With dtPrintCheque.Rows(count)
                    If .Item("TRANTYPENAME").ToString <> "" Then
                        If .Item("DC").ToString = "" Then
                        Else
                            g1.DrawString(.Item("TRANTYPENAME").ToString, fontRegular, Brushes.Black, A0, Topy)
                        End If
                    End If
                    If .Item("REFNO").ToString <> "" Then
                        g1.DrawString(.Item("REFNO").ToString, fontRegular, Brushes.Black, A1, Topy)
                    End If
                    If .Item("REFDATE").ToString <> "" Then
                        g1.DrawString(.Item("REFDATE").ToString, fontRegular, Brushes.Black, A2, Topy)
                    End If
                    If .Item("OTRANNO").ToString <> "" Then
                        g1.DrawString(.Item("OTRANNO").ToString, fontRegular, Brushes.Black, A3, Topy)
                    End If
                    If .Item("OTRANDATE").ToString <> "" Then
                        g1.DrawString(Format(.Item("OTRANDATE"), "dd/MM/yyyy"), fontRegular, Brushes.Black, A4, Topy)
                    End If
                    If .Item("DC").ToString = "Dr" Then 'Cr
                        g1.DrawString(Format(.Item("AMOUNT"), "0.00"), fontRegular, Brushes.Black, A5, Topy, rAlign)
                    ElseIf .Item("DC").ToString = "Cr" Then 'Dr
                        g1.DrawString(Format(.Item("AMOUNT"), "0.00"), fontRegular, Brushes.Black, A6, Topy, rAlign)
                    ElseIf .Item("DC").ToString = "" Then
                        Dim debitAmout As Double = Val(dtPrintCheque.Compute("SUM(AMOUNT)", "DC = 'Dr'").ToString)
                        Dim creditAmount As Double = Val(dtPrintCheque.Compute("SUM(AMOUNT)", "DC = 'Cr'").ToString)

                        g1.DrawString(Format(debitAmout, "0.00"), fontRegular, Brushes.Black, A5, Topy, rAlign)
                        g1.DrawString(Format(creditAmount, "0.00"), fontRegular, Brushes.Black, A6, Topy, rAlign)
                        Topy = Topy + 22
                        g1.DrawLine(Pens.Silver, A0, Topy, 750, Topy)

                        g1.DrawString(.Item("TRANTYPENAME").ToString, fontRegular, Brushes.Black, A1, Topy)
                        If debitAmout > creditAmount Then
                            g1.DrawString(Format(.Item("AMOUNT"), "0.00"), fontRegular, Brushes.Black, A6, Topy, rAlign)
                        Else
                            g1.DrawString(Format(.Item("AMOUNT"), "0.00"), fontRegular, Brushes.Black, A5, Topy, rAlign)
                        End If
                    End If
                    Topy = Topy + 22
                    If .Item("DC").ToString = "" Then
                        g1.DrawLine(Pens.Black, A0, Topy, 750, Topy)
                    Else
                        g1.DrawLine(Pens.Silver, A0, Topy, 750, Topy)
                    End If
                End With
                If Topy > 1000 Then
                    count = count + 1
                    g1.DrawString("Continue....", fontRegular, Brushes.Black, c0, Topy)

                    g1.DrawLine(Pens.Black, A0, topyH1, A0, Topy)
                    g1.DrawLine(Pens.Black, 750, topyH1, 750, Topy)

                    e.HasMorePages = True
                    Topy = 120
                    Return
                End If
            Next
            g1.DrawLine(Pens.Black, A0, topyH1, A0, Topy)
            g1.DrawLine(Pens.Black, 750, topyH1, 750, Topy)

            If Topy > 1000 Then
                g1.DrawString("Continue....", fontRegular, Brushes.Black, c0, Topy)
                e.HasMorePages = True
                g1.DrawLine(Pens.Black, A0, topyH1, A0, Topy)
                g1.DrawLine(Pens.Black, 750, topyH1, 750, Topy)
                Topy = 120
            End If
        ''End If
    End Sub

    Public Sub printchequebook(ByVal g1 As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal billtype As String)
        If dtPrintChequeIssdate.Rows.Count > 0 Then

            Dim rAlign As New StringFormat
            rAlign.Alignment = StringAlignment.Far

            Dim c0 As Integer = 50
            Dim c1 As Integer = 125
            Dim c2 As Integer = 400
            Dim c3 As Integer = 745
            Dim topyH1 As Integer = 0
            g1.DrawLine(Pens.Black, c0, Topy, 750, Topy)
            topyH1 = Topy

            Topy = Topy + 5

            g1.DrawString("Mode ", fontRegular, Brushes.Black, c0, Topy)
            g1.DrawString("Instrument Details ", fontRegular, Brushes.Black, c1, Topy)
            g1.DrawString("Issued From ", fontRegular, Brushes.Black, c2, Topy)
            g1.DrawString("Amount ", fontRegular, Brushes.Black, c3, Topy, rAlign)
            Topy = Topy + 22

            g1.DrawLine(Pens.Black, c0, Topy, 750, Topy)
            Topy = Topy + 3
            With dtPrintChequeIssdate.Rows(0)
                g1.DrawString(chqPaymode, fontRegular, Brushes.Black, c0, Topy) '300
                g1.DrawString(.Item("CHQNUMBER") & " dt." & Format(.Item("CHQDATE"), "dd/MM/yyyy") & " " & .Item("CHQCARDREF").ToString, fontRegular, Brushes.Black, c1, Topy)
                g1.DrawString(.Item("ACNAME").ToString, fontRegular, Brushes.Black, c2, Topy)
                g1.DrawString(.Item("AMOUNT").ToString, fontRegular, Brushes.Black, c3, Topy, rAlign)
                Topy = Topy + 18
            End With
            Topy = Topy + 18
            'HORIZONTAL LINE
            g1.DrawLine(Pens.Black, c0, topyH1, c0, Topy)
            g1.DrawLine(Pens.Black, 750, topyH1, 750, Topy)
            g1.DrawLine(Pens.Black, c0, Topy, 750, Topy)
            If Topy > 1000 Then
                g1.DrawString("Continue....", fontRegular, Brushes.Black, c0, Topy)
                e.HasMorePages = True
                Topy = 120
                Return
            End If
        End If
    End Sub

    Public Sub DetailPAYMENT(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        Dim imgMemoryStream As MemoryStream = New MemoryStream()
        'imgMemoryStream = funcPrintLOGO()
        'If imgMemoryStream.Length > 0 Then
        '    Dim Image As Image = Image.FromStream(imgMemoryStream)
        '    e.Graphics.DrawImage(Image, 170, 0, 500, 130)
        'Else
        '    Dim Image As Image = My.Resources.noimagenew
        '    e.Graphics.DrawImage(Image, 170, 0, 500, 130)
        'End If


        Dim Strimgpath As String = ""
        Dim logo As Image
        If IO.File.Exists(Application.StartupPath & "\VBJ_LOGO.jpg") Then
            Strimgpath = Application.StartupPath & "\VBJ_LOGO.jpg"
            logo = Image.FromFile(Strimgpath)
            e.Graphics.DrawImage(logo, c8 + 40, 20, 120, 70)
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
            'g.DrawString(printcpy, fontBoldTitle, Brushes.Black, c4, 20, LeftFormat)
            'g.DrawString("TAX INVOICE", fontBoldTitle, Brushes.Black, c4, 40, LeftFormat)
            'g.DrawString(StrConv(dtcomp.Rows(0).Item("COMPANYNAME").ToString, VbStrConv.ProperCase), fontBoldHead, BlackBrush, c4 - 150, 20, LeftFormat)
            'g.DrawString(dtcomp.Rows(0).Item("ADDRESS1").ToString, fontRegular, BlackBrush, c4 - 80, 50, LeftFormat)
            'g.DrawString(dtcomp.Rows(0).Item("ADDRESS2").ToString, fontRegular, BlackBrush, c4 - 80, 70, LeftFormat)
            'g.DrawString("GSTIN :" & dtcomp.Rows(0).Item("GSTNO").ToString, fontRegular, BlackBrush, c4 - 70, 90, LeftFormat)
            g.DrawString(Print_Ver_No.ToString, fontRegular6, SilverBrush, c9 + 10, 5, LeftFormat)
            g.DrawString(StrConv(strCompanyName.ToString, VbStrConv.Uppercase), fontBoldHead16, BlackBrush, c4 - 150, 20, LeftFormat)
            g.DrawString(dtcomp.Rows(0).Item("ADDRESS1").ToString + "," + dtcomp.Rows(0).Item("ADDRESS2").ToString, fontRegular, BlackBrush, c4 - 95, 50, LeftFormat)
            g.DrawString(dtcomp.Rows(0).Item("ADDRESS3").ToString + " - " + dtcomp.Rows(0).Item("ADDRESS4").ToString + " - " + dtcomp.Rows(0).Item("PHONE").ToString, fontRegular, BlackBrush, c4 - 95, 70, LeftFormat)
            g.DrawString("GSTIN :" & dtcomp.Rows(0).Item("GSTNO").ToString, fontRegular, BlackBrush, c4 - 35, 90, LeftFormat)
            'g.DrawString("STATE :" & Statename.ToString, fontBold, BlackBrush, c7, 130, LeftFormat)
            'g.DrawString("Phone :" & dtcomp.Rows(0).Item("PHONE").ToString, fontBold, BlackBrush, c4 - 50, 145, LeftFormat)
            ''g.DrawString("PANNO :" & dtcomp.Rows(0).Item("PANNO").ToString, fontBold, BlackBrush, c7, 145, LeftFormat)
        End If
        Topy = Topy + 5
        g.DrawString(prtcopy, fontBoldTitle, Brushes.Black, 300, Topy)
        Topy = Topy + 25
        g.DrawString("GSTIN  : " & strCompanyGstin, fontBoldTitle, Brushes.Black, 300, Topy)
        Topy = Topy + 25
        If chqIssuedate = True Then
        Else
            If Trantype = "CR" Then
                g.DrawString("RECEIPT VOUCHER ", fontBoldTitle, Brushes.Black, 300, Topy)
                Topy = Topy + 15
            ElseIf Trantype = "CP" Then
                g.DrawString("PAYMENT VOUCHER ", fontBoldTitle, Brushes.Black, 300, Topy)
                Topy = Topy + 15
            ElseIf Trantype = "JE" Then
                g.DrawString("JOURNAL VOUCHER", fontBoldTitle, Brushes.Black, 300, Topy)
                Topy = Topy + 15
            ElseIf Trantype = "MN" Then
                g.DrawString("MEMORANDUM VOUCHER ", fontBoldTitle, Brushes.Black, 300, Topy)
                Topy = Topy + 15
            ElseIf Trantype = "DN" Then
                g.DrawString("DEBIT NOTE ", fontBoldTitle, Brushes.Black, 300, Topy)
                Topy = Topy + 15
            ElseIf Trantype = "CN" Then
                g.DrawString("CREDIT NOTE ", fontBoldTitle, Brushes.Black, 300, Topy)
                Topy = Topy + 15
            End If
        End If
        Dim t1 As Integer = 550
        Dim t2 As Integer = 620
        Dim t3 As Integer = 630
        For i As Integer = 0 To dtPrint.Rows.Count - 1
            If dtPrint.Rows(i).Item("TRANMODE").ToString = "C" Then Continue For
            If Trantype = "CN" Then
                g.DrawString(dtPrint.Rows(i).Item("DACNAME") & " " & pBankName.Trim, fontBold, Brushes.Black, c0, Topy)
            Else
                g.DrawString(dtPrint.Rows(i).Item("ACNAME") & " " & pBankName.Trim, fontBold, Brushes.Black, c0, Topy)
            End If

            g.DrawString("Voucher No ", fontRegular, Brushes.Black, t1, Topy)
            g.DrawString(": ", fontRegular, Brushes.Black, t2, Topy)
            g.DrawString(cnCostId & "/" & finYear & "/" & dtPrint.Rows(0).Item("TRANNO"), fontRegular, Brushes.Black, t3, Topy)
            Topy = Topy + 15
            g.DrawString("Date ", fontRegular, Brushes.Black, t1, Topy)
            g.DrawString(": ", fontRegular, Brushes.Black, t2, Topy)
            g.DrawString("" & dtPrint.Rows(0).Item("TRANDATE"), fontRegular, Brushes.Black, t3, Topy)
            If dtAddresssupplier.Rows.Count = 0 Then
                Topy = Topy + 15
            End If
            If dtAddresssupplier.Rows.Count > 0 Then
                g.DrawString(dtAddresssupplier.Rows(0).Item("ADDRESS1").ToString & " " & dtAddresssupplier.Rows(0).Item("ADDRESS2").ToString, fontRegular7, Brushes.Black, c0, Topy)
                Topy = Topy + 15
                If dtAddresssupplier.Rows(0).Item("ADDRESS3").ToString <> "" Then
                    g.DrawString(dtAddresssupplier.Rows(0).Item("ADDRESS3").ToString, fontRegular7, Brushes.Black, c0, Topy)
                    Topy = Topy + 15
                End If
                If dtAddresssupplier.Rows(0).Item("CITY").ToString <> "" Then
                    g.DrawString(dtAddresssupplier.Rows(0).Item("CITY").ToString & " " & dtAddresssupplier.Rows(0).Item("STATE").ToString, fontRegular7, Brushes.Black, c0, Topy)
                    Topy = Topy + 15
                End If
                If dtAddresssupplier.Rows(0).Item("PINCODE").ToString <> "" Then
                    g.DrawString(dtAddresssupplier.Rows(0).Item("PINCODE").ToString, fontRegular7, Brushes.Black, c0, Topy)
                    Topy = Topy + 15
                End If
                If dtAddresssupplier.Rows(0).Item("GSTNO").ToString <> "" Then
                    g.DrawString("GSTNO :" & dtAddresssupplier.Rows(0).Item("GSTNO").ToString, fontRegular, Brushes.Black, c0, Topy)
                    Topy = Topy + 15
                End If
            End If
            Exit For
        Next
        'Topy = Topy + 18
    End Sub
    Public Sub DetailCNDN(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        Dim imgMemoryStream As MemoryStream = New MemoryStream()
        'imgMemoryStream = funcPrintLOGO()
        'If imgMemoryStream.Length > 0 Then
        '    Dim Image As Image = Image.FromStream(imgMemoryStream)
        '    e.Graphics.DrawImage(Image, 170, 0, 500, 130)
        'Else
        '    Dim Image As Image = My.Resources.noimagenew
        '    e.Graphics.DrawImage(Image, 170, 0, 500, 130)
        'End If
        Dim t1 As Integer = 550
        Dim t2 As Integer = 620
        Dim t3 As Integer = 630

        Dim Strimgpath As String = ""
        Dim logo As Image
        If IO.File.Exists(Application.StartupPath & "\VBJ_LOGO.jpg") Then
            Strimgpath = Application.StartupPath & "\VBJ_LOGO.jpg"
            logo = Image.FromFile(Strimgpath)
            e.Graphics.DrawImage(logo, c8 + 40, 20, 120, 70)
        End If
        'Dim dtcomp As New DataTable
        'strsql = " SELECT * FROM " & cnAdminDb & "..COMPANY "
        'strsql += vbCrLf + "WHERE COMPANYID='" & strCompanyId & "'"

        'da = New OleDbDataAdapter(strsql, cn)
        'dtcomp = New DataTable
        'da.Fill(dtcomp)

        Dim dtcomp As New DataTable
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
            'g.DrawString(printcpy, fontBoldTitle, Brushes.Black, c4, 20, LeftFormat)
            'g.DrawString("TAX INVOICE", fontBoldTitle, Brushes.Black, c4, 40, LeftFormat)
            'g.DrawString(StrConv(dtcomp.Rows(0).Item("COMPANYNAME").ToString, VbStrConv.ProperCase), fontBoldHead, BlackBrush, c4 - 150, 20, LeftFormat)
            'g.DrawString(dtcomp.Rows(0).Item("ADDRESS1").ToString, fontRegular, BlackBrush, c4 - 80, 50, LeftFormat)
            'g.DrawString(dtcomp.Rows(0).Item("ADDRESS2").ToString, fontRegular, BlackBrush, c4 - 80, 70, LeftFormat)
            'g.DrawString("GSTIN :" & dtcomp.Rows(0).Item("GSTNO").ToString, fontRegular, BlackBrush, c4 - 70, 90, LeftFormat)
            g.DrawString(Print_Ver_No.ToString, fontRegular6, SilverBrush, c9 + 10, 5, LeftFormat)
            g.DrawString(StrConv(strCompanyName.ToString, VbStrConv.Uppercase), fontBoldHead16, BlackBrush, c4 - 150, 20, LeftFormat)
            g.DrawString(dtcomp.Rows(0).Item("ADDRESS1").ToString + "," + dtcomp.Rows(0).Item("ADDRESS2").ToString, fontRegular, BlackBrush, c4 - 95, 50, LeftFormat)
            g.DrawString(dtcomp.Rows(0).Item("ADDRESS3").ToString + " - " + dtcomp.Rows(0).Item("ADDRESS4").ToString + " - " + dtcomp.Rows(0).Item("PHONE").ToString, fontRegular, BlackBrush, c4 - 95, 70, LeftFormat)
            g.DrawString("GSTIN :" & dtcomp.Rows(0).Item("GSTNO").ToString, fontRegular, BlackBrush, c4 - 35, 90, LeftFormat)
            'g.DrawString("STATE :" & Statename.ToString, fontBold, BlackBrush, c7, 130, LeftFormat)
            'g.DrawString("Phone :" & dtcomp.Rows(0).Item("PHONE").ToString, fontBold, BlackBrush, c4 - 50, 145, LeftFormat)
            ''g.DrawString("PANNO :" & dtcomp.Rows(0).Item("PANNO").ToString, fontBold, BlackBrush, c7, 145, LeftFormat)
        End If
        Topy = Topy + 5
        g.DrawString(prtcopy, fontBoldTitle, Brushes.Black, 300, Topy)
        'Topy = Topy + 25
        'g.DrawString("GSTIN  : " & strCompanyGstin, fontBoldTitle, Brushes.Black, 300, Topy)
        'Topy = Topy + 25
        If chqIssuedate = True Then
        Else
            If Trantype = "DN" Then
                g.DrawString("DEBIT NOTE ", fontBoldTitle, Brushes.Black, t1, Topy)
            ElseIf Trantype = "CN" Then
                g.DrawString("CREDIT NOTE ", fontBoldTitle, Brushes.Black, t1, Topy)
            End If
        End If
        Topy = Topy + 25
        For i As Integer = 0 To dtPrint.Rows.Count - 1
            If dtPrint.Rows(i).Item("TRANMODE").ToString = "C" Then Continue For
            If Trantype = "CN" Then
                g.DrawString(dtPrint.Rows(i).Item("DACNAME") & " " & pBankName.Trim, fontBold, Brushes.Black, c0, Topy)
            Else
                g.DrawString(dtPrint.Rows(i).Item("ACNAME") & " " & pBankName.Trim, fontBold, Brushes.Black, c0, Topy)
            End If

            g.DrawString("Voucher No ", fontRegular, Brushes.Black, t1, Topy)
            g.DrawString(": ", fontRegular, Brushes.Black, t2, Topy)
            g.DrawString(cnCostId & "/" & finYear & "/" & dtPrint.Rows(0).Item("TRANNO"), fontRegular, Brushes.Black, t3, Topy)
            Topy = Topy + 15
            g.DrawString("Date ", fontRegular, Brushes.Black, t1, Topy)
            g.DrawString(": ", fontRegular, Brushes.Black, t2, Topy)
            g.DrawString("" & dtPrint.Rows(0).Item("TRANDATE"), fontRegular, Brushes.Black, t3, Topy)
            If dtAddresssupplier.Rows.Count = 0 Then
                Topy = Topy + 15
            End If
            If dtAddresssupplier.Rows.Count > 0 Then
                g.DrawString(dtAddresssupplier.Rows(0).Item("ADDRESS1").ToString & " " & dtAddresssupplier.Rows(0).Item("ADDRESS2").ToString, fontRegular7, Brushes.Black, c0, Topy)
                Topy = Topy + 15
                If dtAddresssupplier.Rows(0).Item("ADDRESS3").ToString <> "" Then
                    g.DrawString(dtAddresssupplier.Rows(0).Item("ADDRESS3").ToString, fontRegular7, Brushes.Black, c0, Topy)
                    Topy = Topy + 15
                End If
                If dtAddresssupplier.Rows(0).Item("CITY").ToString <> "" Then
                    g.DrawString(dtAddresssupplier.Rows(0).Item("CITY").ToString & " " & dtAddresssupplier.Rows(0).Item("PINCODE").ToString, fontRegular7, Brushes.Black, c0, Topy)
                    Topy = Topy + 15
                End If
                If dtAddresssupplier.Rows(0).Item("STATE").ToString <> "" Then
                    g.DrawString(dtAddresssupplier.Rows(0).Item("STATE").ToString, fontRegular7, Brushes.Black, c0, Topy)
                    Topy = Topy + 15
                End If

                If dtAddresssupplier.Rows(0).Item("GSTNO").ToString <> "" Then
                    g.DrawString(dtAddresssupplier.Rows(0).Item("GSTNO").ToString, fontRegular8, Brushes.Black, c0, Topy)
                    Topy = Topy + 15
                End If
            End If
            Exit For
        Next
        'Topy = Topy + 18
    End Sub

    Public Sub TilePAYMENT(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        Dim rAlign As New StringFormat
        rAlign.Alignment = StringAlignment.Far
        g.DrawLine(Pens.Black, 40, Topy, 760, Topy)
        Topy = Topy + 5
        g.DrawString("Particulars ", fontRegular, Brushes.Black, c0, Topy)
        g.DrawString("Amount ", fontRegular, Brushes.Black, c2, Topy, rAlign)
        Topy = Topy + 30
        g.DrawLine(Pens.Black, 40, Topy, 760, Topy)
    End Sub

    Private Sub PrintDesignRECEIPT(ByVal g1 As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal copy As Boolean)
        DetailRECEIPT(g1, e)
        TileRECEIPT(g1, e)
        Topy = Topy + 18
        Dim Cr1 As Integer = 570
        Dim Cr2 As Integer = 690
        If dtPrint.Rows.Count > 0 Then
            For i As Integer = 0 To dtPrint.Rows.Count - 1
                If dtPrint.Rows(i).Item("ACNAME").ToString = "" Then Continue For
                g1.DrawString(dtPrint.Rows(i).Item("ACNAME").ToString, fontRegular, Brushes.Black, c0, Topy)
                If dtPrint.Rows(i).Item("TRANMODE").ToString = "D" Then
                    g1.DrawString(dtPrint.Rows(i).Item("AMOUNT").ToString, fontRegular, Brushes.Black, Cr1, Topy, LeftFormat) 'DEBIT
                Else
                    g1.DrawString(dtPrint.Rows(i).Item("AMOUNT").ToString, fontRegular, Brushes.Black, Cr2, Topy, LeftFormat) 'CREDIT
                End If
                Topy = Topy + 20
            Next
        End If
        Topy = Topy + 20
        g1.DrawLine(Pens.Black, 40, Topy, 760, Topy)
        Topy = Topy + 20
        g1.DrawString(Format(Val(dtPrint.Compute("SUM(AMOUNT)", "TRANMODE = 'D'").ToString), "0.00"), fontRegular, Brushes.Black, Cr1, Topy, LeftFormat) 'DEBIT
        g1.DrawString(Format(Val(dtPrint.Compute("SUM(AMOUNT)", "TRANMODE = 'C'").ToString), "0.00"), fontRegular, Brushes.Black, Cr2, Topy, LeftFormat) 'CREDIT
        Topy = Topy + 20
        g1.DrawLine(Pens.Black, 40, Topy, 760, Topy)
        Topy = Topy + 2
        Dim Amount As Double = 0
        Amount = Val(dtPrint.Compute("SUM(AMOUNT)", "TRANMODE = 'D'").ToString)
        Dim AmountInWord As String = ConvertRupees.RupeesToWord(Amount, "Rupees", "Only")
        g1.DrawString("(" & AmountInWord & ")", fontRegular, Brushes.Black, c0, Topy, LeftFormat) 'DEBIT
        If dtPrint.Rows(0).Item("REMARK1").ToString <> "" Then
            Topy = Topy + 18
            g1.DrawString(dtPrint.Rows(0).Item("REMARK1").ToString, fontRegular, Brushes.Black, c0, Topy)
        End If
        If dtPrint.Rows(0).Item("REMARK2").ToString <> "" Then
            Topy = Topy + 18
            g1.DrawString(dtPrint.Rows(0).Item("REMARK2").ToString, fontRegular, Brushes.Black, c0, Topy)
        End If
        Topy = Topy + 18
        'If dtPrintCheque.Rows.Count > 0 Then
        '    printadjutran(g1, e, "RECEIPT DETAIL")
        'End If
        footer(g1, e)
    End Sub



    Public Sub DetailRECEIPT(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        Dim imgMemoryStream As MemoryStream = New MemoryStream()
        'imgMemoryStream = funcPrintLOGO()
        'Dim Image As Image = Image.FromStream(imgMemoryStream)
        'e.Graphics.DrawImage(Image, 170, 0, 500, 130)

        Dim Strimgpath As String = ""
        Dim logo As Image
        If IO.File.Exists(Application.StartupPath & "\VBJ_LOGO.jpg") Then
            Strimgpath = Application.StartupPath & "\VBJ_LOGO.jpg"
            logo = Image.FromFile(Strimgpath)
            e.Graphics.DrawImage(logo, c8 + 40, 20, 120, 70)
        End If
        Dim dtcomp As New DataTable
        'strsql = " SELECT * FROM " & cnAdminDb & "..COMPANY "
        'strsql += vbCrLf + "WHERE COMPANYID='" & strCompanyId & "'"
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
            'g.DrawString(printcpy, fontBoldTitle, Brushes.Black, c4, 20, LeftFormat)
            'g.DrawString("TAX INVOICE", fontBoldTitle, Brushes.Black, c4, 40, LeftFormat)
            'g.DrawString(StrConv(dtcomp.Rows(0).Item("COMPANYNAME").ToString, VbStrConv.ProperCase), fontBoldHead, BlackBrush, c4 - 150, 20, LeftFormat)
            'g.DrawString(dtcomp.Rows(0).Item("ADDRESS1").ToString, fontRegular, BlackBrush, c4 - 80, 50, LeftFormat)
            'g.DrawString(dtcomp.Rows(0).Item("ADDRESS2").ToString, fontRegular, BlackBrush, c4 - 80, 70, LeftFormat)
            'g.DrawString("GSTIN :" & dtcomp.Rows(0).Item("GSTNO").ToString, fontRegular, BlackBrush, c4 - 70, 90, LeftFormat)
            g.DrawString(Print_Ver_No.ToString, fontRegular6, SilverBrush, c9 + 10, 5, LeftFormat)
            g.DrawString(StrConv(strCompanyName.ToString, VbStrConv.Uppercase), fontBoldHead16, BlackBrush, c4 - 150, 20, LeftFormat)
            g.DrawString(dtcomp.Rows(0).Item("ADDRESS1").ToString + "," + dtcomp.Rows(0).Item("ADDRESS2").ToString, fontRegular, BlackBrush, c4 - 95, 50, LeftFormat)
            g.DrawString(dtcomp.Rows(0).Item("ADDRESS3").ToString + " - " + dtcomp.Rows(0).Item("ADDRESS4").ToString + " - " + dtcomp.Rows(0).Item("PHONE").ToString, fontRegular, BlackBrush, c4 - 95, 70, LeftFormat)
            g.DrawString("GSTIN :" & dtcomp.Rows(0).Item("GSTNO").ToString, fontRegular, BlackBrush, c4 - 35, 90, LeftFormat)
            'g.DrawString("STATE :" & Statename.ToString, fontBold, BlackBrush, c7, 130, LeftFormat)
            'g.DrawString("Phone :" & dtcomp.Rows(0).Item("PHONE").ToString, fontBold, BlackBrush, c4 - 50, 145, LeftFormat)
            ''g.DrawString("PANNO :" & dtcomp.Rows(0).Item("PANNO").ToString, fontBold, BlackBrush, c7, 145, LeftFormat)
        End If

        Topy = Topy + 30
        g.DrawString(prtcopy, fontBoldTitle, Brushes.Black, 300, Topy)
        Topy = Topy + 15
        If Trantype = "CR" Then
            g.DrawString("RECEIPT VOUCHER ", fontBoldTitle, Brushes.Black, 300, Topy)
            Topy = Topy + 15
        ElseIf Trantype = "CP" Then
            g.DrawString("PAYMENT VOUCHER ", fontBoldTitle, Brushes.Black, 300, Topy)
            Topy = Topy + 15
        ElseIf Trantype = "JE" Then
            g.DrawString("JOURNAL ", fontBoldTitle, Brushes.Black, 300, Topy)
            Topy = Topy + 15
        End If

        Dim t1 As Integer = 550
        Dim t2 As Integer = 620
        Dim t3 As Integer = 650

        g.DrawString("Voucher No ", fontRegular, Brushes.Black, t1, Topy)
        g.DrawString(": ", fontRegular, Brushes.Black, t2, Topy)
        g.DrawString(cnCostId & "/" & finYear & "/" & dtPrint.Rows(0).Item("TRANNO"), fontRegular, Brushes.Black, t3, Topy)
        Topy = Topy + 18
        g.DrawString("Date ", fontRegular, Brushes.Black, t1, Topy)
        g.DrawString(": ", fontRegular, Brushes.Black, t2, Topy)
        g.DrawString("" & dtPrint.Rows(0).Item("TRANDATE"), fontRegular, Brushes.Black, t3, Topy)
        Topy = Topy + 18
    End Sub

    Public Sub TileRECEIPT(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        Dim rAlign As New StringFormat
        rAlign.Alignment = StringAlignment.Far
        g.DrawLine(Pens.Black, 40, Topy, 760, Topy)
        Topy = Topy + 5
        g.DrawString("Particulars ", fontRegular, Brushes.Black, c0, Topy)
        g.DrawString("Debit ", fontRegular, Brushes.Black, 570, Topy, rAlign)
        g.DrawString("Credit ", fontRegular, Brushes.Black, 690, Topy, rAlign)
        Topy = Topy + 30
        g.DrawLine(Pens.Black, 40, Topy, 760, Topy)
    End Sub

    Public Sub footer(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        Try
            Dim p1 As Integer = Topy
            If MNVOUCHERSIZE = "Y" Then
                g.DrawString("For " + strCompanyName.ToString, fontRegular, Brushes.Black, 450, p1)
                p1 = p1 + 20
                g.DrawString("Entered by", fontRegular, Brushes.Black, c0, p1)
                g.DrawString("Receiver's Sign", fontRegular, Brushes.Black, 250, p1)
                g.DrawString("Passed by", fontRegular, Brushes.Black, 450, p1)
                g.DrawString("Authorised Sign", fontRegular, Brushes.Black, 560, p1)
                g.DrawString(dtPrint.Rows(0).Item("USERNAME"), fontRegular, Brushes.Black, c0, p1 + 20)
                'Dim RECNO_REFDATE As String = dtPrint.Rows(0).Item("TRANNO").ToString & "-" & dtPrint.Rows(0).Item("TRANDATE").Replace("/", "")
                'If System.IO.File.Exists(temp & "\barcode" & systemName & RECNO_REFDATE & ".png") = True Then
                '    System.IO.File.Delete(temp & "\barcode" & systemName & RECNO_REFDATE & ".png")
                'End If
                'barcode.Type = BarcodeLib.Barcode.BarcodeType.CODE128
                'barcode.Rotate = BarcodeLib.Barcode.RotateOrientation.BottomFacingDown
                'barcode.Data = RECNO_REFDATE
                'barcode.drawBarcode(temp & "\barcode" & systemName & RECNO_REFDATE & ".png")
                'Dim imgstream As New Bitmap(temp & "\barcode" & systemName & RECNO_REFDATE & ".png")
                'e.Graphics.DrawImage(imgstream, 300, p1 + 5, 200, 50)
                'imgstream.Dispose()
                'If System.IO.File.Exists(temp & "\barcode" & systemName & RECNO_REFDATE & ".png") = True Then
                '    System.IO.File.Delete(temp & "\barcode" & systemName & RECNO_REFDATE & ".png")
                'End If
            Else

            End If
        Catch ex As Exception
            MsgBox("BARCODE FONT NOT FOUND", MsgBoxStyle.Information)
            MsgBox(ex.ToString)
        End Try
    End Sub
    Public Sub footerCNDN(ByVal g As Graphics, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        Try
            Dim p1 As Integer = Topy
            If MNVOUCHERSIZE = "Y" Then
                g.DrawString("For " + strCompanyName.ToString, fontRegular, Brushes.Black, 450, p1)
                p1 = 498
                g.DrawString(dtPrint.Rows(0).Item("USERNAME"), fontRegular, Brushes.Black, c0, p1 - 20)
                g.DrawString("Entered by", fontRegular, Brushes.Black, c0, p1)
                g.DrawString("Receiver's Sign", fontRegular, Brushes.Black, 250, p1)
                'g.DrawString("Passed by", fontRegular, Brushes.Black, 450, p1)
                g.DrawString("Authorised Sign", fontRegular, Brushes.Black, 560, p1)

                'Dim RECNO_REFDATE As String = dtPrint.Rows(0).Item("TRANNO").ToString & "-" & dtPrint.Rows(0).Item("TRANDATE").Replace("/", "")
                'If System.IO.File.Exists(temp & "\barcode" & systemName & RECNO_REFDATE & ".png") = True Then
                '    System.IO.File.Delete(temp & "\barcode" & systemName & RECNO_REFDATE & ".png")
                'End If
                'barcode.Type = BarcodeLib.Barcode.BarcodeType.CODE128
                'barcode.Rotate = BarcodeLib.Barcode.RotateOrientation.BottomFacingDown
                'barcode.Data = RECNO_REFDATE
                'barcode.drawBarcode(temp & "\barcode" & systemName & RECNO_REFDATE & ".png")
                'Dim imgstream As New Bitmap(temp & "\barcode" & systemName & RECNO_REFDATE & ".png")
                'e.Graphics.DrawImage(imgstream, 300, p1 + 5, 200, 50)
                'imgstream.Dispose()
                'If System.IO.File.Exists(temp & "\barcode" & systemName & RECNO_REFDATE & ".png") = True Then
                '    System.IO.File.Delete(temp & "\barcode" & systemName & RECNO_REFDATE & ".png")
                'End If
            Else

            End If
        Catch ex As Exception
            MsgBox("BARCODE FONT NOT FOUND", MsgBoxStyle.Information)
            MsgBox(ex.ToString)
        End Try
    End Sub

#End Region
End Class