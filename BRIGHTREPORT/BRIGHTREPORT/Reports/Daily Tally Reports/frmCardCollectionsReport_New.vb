Imports System.Data.OleDb
Public Class frmCardCollectionsReport_New
    'CALNO 271112  VASANTHAN, CLIENT-PRINCE
    Dim objGridShower As frmGridDispDia
    Dim dtCardCollection As New DataTable
    Dim StrItemFilter As String = Nothing
    Dim StrHeader As String = Nothing
    Dim strChit As String = Nothing
    Dim strcredit As String = Nothing
    Dim strcheque As String = Nothing
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dt As New DataTable
    Dim CCENTRE As String = Nothing
    Dim NOID As String = Nothing
    Dim SelectedCompany As String

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus

        dtpFrom.Value = GetServerDate(tran)
        dtpTo.Value = GetServerDate(tran)
    End Sub

    Private Sub frmCardCollectionsReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.WindowState = FormWindowState.Maximized
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        LoadCompany(chkLstCompany)
        Me.tabMain.ItemSize = New Size(1, 1)
        Me.tabMain.Region = New Region(New RectangleF(Me.tabGen.Left, Me.tabGen.Top, Me.tabGen.Width, Me.tabGen.Height))
        Me.tabMain.SelectedTab = tabGen
        rdbChitCard.Checked = True
        rdbDetail.Checked = True
        btnNew_Click(Me, New EventArgs)
        dtpFrom.Select()
    End Sub

    Public Function funcAddCostName() As Integer
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'COSTCENTRE' "
        If objGPack.GetSqlValue(strSql, , "N") = "Y" Then
            strSql = "SELECT DISTINCT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            dt = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                chkLstCostCentre.Enabled = True
                chkLstCostCentre.Items.Clear()
                chkLstCostCentre.Items.Add("ALL", IIf(cnDefaultCostId, True, False))
                'For cnt As Integer = 0 To dt.Rows.Count - 1
                '    chkLstCostCentre.Items.Add(dt.Rows(cnt).Item(0).ToString)
                'Next
                For i As Integer = 0 To dt.Rows.Count - 1
                    If cnCostName = dt.Rows(i).Item("COSTNAME").ToString And cnDefaultCostId = False Then
                        chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, True)
                    Else
                        chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, False)
                    End If
                Next
                If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkLstCostCentre.Enabled = False
            End If
        Else
            chkLstCostCentre.Items.Clear()
            chkLstCostCentre.Enabled = False
        End If
    End Function

    Private Sub funcAddNodeId()
        strSql = "SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ACCTRAN  WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
        'strSql += " UNION"
        'strSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..ISSUE  WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
        'strSql += " UNION"
        'strSql += " SELECT DISTINCT SYSTEMID FROM " & cnStockDb & "..RECEIPT  WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value, "yyyy-MM-dd") & "'"
        dt = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            chkLstNodeId.Items.Clear()
            chkLstNodeId.Items.Add("ALL", True)
            For CNT As Integer = 0 To dt.Rows.Count - 1
                chkLstNodeId.Items.Add(dt.Rows(CNT).Item(0).ToString)
            Next
        Else
            chkLstNodeId.Items.Clear()
            chkLstNodeId.Items.Add("ALL", True)
            'chkLstNodeId.Enabled = False
        End If
    End Sub

    Function funcCostName(ByVal WithQuotes As Boolean) As String
        ''COSTCENTRE
        CCENTRE = ""
        If chkLstCostCentre.Enabled Then
            If chkLstCostCentre.CheckedItems.Count > 0 And chkLstCostCentre.GetItemChecked(0) <> True Then
                For CNT As Integer = 0 To chkLstCostCentre.CheckedItems.Count - 1
                    If WithQuotes Then CCENTRE += "'"
                    CCENTRE += chkLstCostCentre.CheckedItems.Item(CNT).ToString
                    If WithQuotes Then CCENTRE += "'"
                    If Not (CNT = chkLstCostCentre.CheckedItems.Count - 1) Then CCENTRE += ","
                Next
            End If
        End If
        Return CCENTRE
    End Function

    Function funcSystemId(ByVal WithQuotes As Boolean) As String
        ''NODE ID
        NOID = ""
        If chkLstNodeId.CheckedItems.Count > 0 And chkLstNodeId.GetItemChecked(0) <> True Then
            For CNT As Integer = 0 To chkLstNodeId.CheckedItems.Count - 1
                If WithQuotes Then NOID += "'"
                NOID += chkLstNodeId.CheckedItems.Item(CNT).ToString
                If WithQuotes Then NOID += "'"
                If Not (CNT = chkLstNodeId.CheckedItems.Count - 1) Then NOID += ","
            Next
        End If
        Return NOID
    End Function

    Private Sub frmCardCollectionsReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
                SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) And tabMain.SelectedTab.Name = tabView.Name Then
            btnBack_Click(Me, New EventArgs)
        End If
        If UCase(e.KeyChar) = Chr(Keys.X) And tabMain.SelectedTab.Name = tabView.Name Then
            btnExcel_Click(Me, New EventArgs)
        End If
        If UCase(e.KeyChar) = Chr(Keys.P) And tabMain.SelectedTab.Name = tabView.Name Then
            btnPrint_Click(Me, New EventArgs)
        End If
    End Sub
    Private Function Filtration(ByVal PayMode As String) As String
        Dim strFilt As String = ""
        If chkLstCostCentre.Enabled = True Then
            If Not chkLstCostCentre.CheckedItems.Count > 0 Then
                chkLstCostCentre.SetItemChecked(0, True)
            End If
        End If
        If chkLstNodeId.Items.Count = 0 Then
            funcAddNodeId()
        End If
        If Not chkLstNodeId.CheckedItems.Count > 0 Then
            chkLstNodeId.SetItemChecked(0, True)
        End If
        SelectedCompany = GetSelectedCompanyId(chkLstCompany, True)
        Dim CostFtr As String = funcCostName(True)
        Dim NodeFtr As String = funcSystemId(True)

        strFilt += vbCrLf & " 		WHERE TRANDATE BETWEEN '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "' "
        strFilt += vbCrLf & " 		AND PAYMODE IN (" & PayMode & ") AND ISNULL(CANCEL,'') != 'Y'    "
        If chkLstCostCentre.Enabled = True And chkLstCostCentre.CheckedItems.Count > 0 Then
            If chkLstCostCentre.GetItemChecked(0) <> True Then
                strFilt += vbCrLf & " 		AND ISNULL(COSTID,'') IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & CostFtr & "))    "
            End If
        End If
        If chkLstNodeId.Enabled = True And chkLstNodeId.CheckedItems.Count > 0 Then
            If chkLstNodeId.GetItemChecked(0) <> True Then
                strFilt += vbCrLf & " 		AND SYSTEMID IN (" & NodeFtr & ") "
            End If
        End If
        If chkLstCompany.Enabled = True And chkLstCompany.CheckedItems.Count > 0 Then
            strFilt += vbCrLf & " 		AND COMPANYID IN (" & SelectedCompany & ")"
        End If
        If ChkInclChit.Checked = False Then strFilt += vbCrLf & " AND ISNULL(FROMFLAG,'') <> 'C'"
        Return strFilt
    End Function

   
    Private Function ChitCard() As Integer
        strSql = vbCrLf & " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPCHITCOLLECT')>0 "
        strSql += vbCrLf & " 	DROP TABLE TEMPTABLEDB..TEMPCHITCOLLECT    "
        strSql += vbCrLf & " SELECT "
        strSql += vbCrLf & " 	 PARTICULAR"
        If rdbDetail.Checked = True Then strSql += vbCrLf & " ,GROUPNAME,CONVERT(VARCHAR,TRANDATE,103) TRANDATE,TRANNO,REGNO,CUSTOMER,TRANDATE TRANDATE1,SYSTEMID /* DETAIL */"
        strSql += vbCrLf & " 	,SUM(ISNULL(AMOUNT,0)) PRINCIPAL"
        strSql += vbCrLf & " 	,SUM(ADDAMT) ADDAMT,SUM(DEDCAMT) DEDCAMT,SUM(ISNULL(AMOUNT,0) + ISNULL(ADDAMT,0) - ISNULL(DEDCAMT,0)) AMOUNT"
        strSql += vbCrLf & " 	,RESULT,CONVERT(VARCHAR(3),COLHEAD)COLHEAD, COMPANYNAME"
        If ChkGrpcounter.Checked Then
            strSql += vbCrLf & " 	,CASHCOUNTER PARTICULAR1"
        Else
            strSql += vbCrLf & " 	,PARTICULAR PARTICULAR1"
        End If
        strSql += vbCrLf & " 	,1 SEP,CASHCOUNTER"
        strSql += vbCrLf & " INTO TEMPTABLEDB..TEMPCHITCOLLECT     "
        strSql += vbCrLf & " FROM (    "
        strSql += vbCrLf & " 		SELECT "
        strSql += vbCrLf & " 			 CHQCARDREF GROUPNAME,TRANDATE"
        If rdbDetail.Checked = True Then
            strSql += vbCrLf & " 			 ,(SELECT TOP 1 NAME FROM " & cnAdminDb & "..CREDITCARD WHERE CARDCODE = T.CARDID)AS PARTICULAR"
        Else
            strSql += vbCrLf & " 			 ,(SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.ACCODE)AS PARTICULAR"
        End If
        If rdbDetail.Checked = True Then strSql += vbCrLf & " 			 ,TRANNO,CHQCARDNO REGNO /* DETAIL */"
        'CALNO 271112
        If rdbDetail.Checked = True Then strSql += vbCrLf & " 			,(SELECT CASE WHEN INITIAL <> '' THEN INITIAL + '.' ELSE '' END + PNAME AS NAME FROM " & cnAdminDb & "..PERSONALINFO "
        If rdbDetail.Checked = True Then strSql += vbCrLf & " 			WHERE SNO =(SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO=T.BATCHNO))AS CUSTOMER /* DETAIL */			 		 "
        strSql += vbCrLf & " 			,CASE WHEN TRANMODE = 'D' AND ISNULL(PAYMODE,'') = 'SS' THEN AMOUNT WHEN TRANMODE = 'C' AND PAYMODE in('SS','HP','HR','HB','HG','HZ') THEN -1 * AMOUNT END AMOUNT"
        strSql += vbCrLf & " 			,CASE WHEN TRANMODE = 'D' AND ISNULL(PAYMODE,'') != 'SS' THEN AMOUNT END ADDAMT"
        strSql += vbCrLf & " 			,CASE WHEN TRANMODE = 'C' AND ISNULL(PAYMODE,'') != 'SS' THEN AMOUNT END DEDCAMT"
        strSql += vbCrLf & " 			,SYSTEMID,CONVERT(VARCHAR(1),NULL)COLHEAD,CONVERT(INT,2) RESULT"
        'CALNO 120214
        strSql += vbCrLf & " 			,(SELECT TOP 1 CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHID = T.CASHID)AS CASHCOUNTER"
        strSql += vbCrLf & " ,(SELECT TOP 1 COMPANYNAME FROM " & cnAdminDb & "..COMPANY  WHERE COMPANYID  = T.COMPANYID)AS COMPANYNAME,T.COMPANYID "
        strSql += vbCrLf & " 		FROM " & cnStockDb & "..ACCTRAN T     "
        strSql += Filtration("'SS','CG','CB','CZ','CD','HP','HR','HD','HB','HG','HZ'")
        strSql += vbCrLf & " )X    "
        strSql += vbCrLf & " GROUP BY PARTICULAR"
        If rdbDetail.Checked = True Then strSql += vbCrLf & " ,TRANNO,REGNO,CUSTOMER,TRANDATE,GROUPNAME,SYSTEMID "
        strSql += vbCrLf & " ,COLHEAD,RESULT,CASHCOUNTER, COMPANYNAME"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf & " /** INSERTING TITLE AND SUBTOTAL AND GRAND TOTAL **/    "
        strSql += vbCrLf & " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPCHITCOLLECT)>0    "
        strSql += vbCrLf & " BEGIN    "
        'strSql += vbCrLf & " INSERT INTO TEMPTABLEDB..TEMPCHITCOLLECT(PARTICULAR,PRINCIPAL,ADDAMT,DEDCAMT,AMOUNT,RESULT,COLHEAD,TRANDATE1,SEP)"
        'strSql += vbCrLf & " SELECT DISTINCT TRANDATE,NULL PRINCIPAL,NULL ADDAMT,NULL DEDCAMT"
        'strSql += vbCrLf & " ,NULL AMOUNT,0 RESULT,'T' COLHEAD,TRANDATE1,1 SEP FROM TEMPTABLEDB..TEMPCHITCOLLECT WHERE RESULT = 2  "
        If rdbDetail.Checked Then
            If ChkGrpcounter.Checked Then
                strSql += vbCrLf & " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPCHITCOLLECT)>0    "
                strSql += vbCrLf & " BEGIN    "
                strSql += vbCrLf & " INSERT INTO TEMPTABLEDB..TEMPCHITCOLLECT(PARTICULAR,PRINCIPAL,ADDAMT,DEDCAMT,AMOUNT,RESULT,COLHEAD,PARTICULAR1,SEP,REGNO)"
                strSql += vbCrLf & " SELECT DISTINCT PARTICULAR1,NULL PRINCIPAL,NULL ADDAMT,NULL DEDAMT,NULL AMOUNT,0 RESULT,'T' COLHEAD,PARTICULAR1,1 SEP,NULL REGNO "
                strSql += vbCrLf & " FROM TEMPTABLEDB..TEMPCHITCOLLECT WHERE RESULT = 2  "
                strSql += vbCrLf & " END "
            End If
            strSql += vbCrLf & " INSERT INTO TEMPTABLEDB..TEMPCHITCOLLECT(PARTICULAR,PRINCIPAL,ADDAMT,DEDCAMT,AMOUNT,RESULT,COLHEAD,PARTICULAR1,SEP,REGNO)"
            strSql += vbCrLf & " SELECT 'SUB TOTAL',SUM(PRINCIPAL),SUM(ADDAMT),SUM(DEDCAMT)"
            strSql += vbCrLf & " ,SUM(AMOUNT),3 RESULT,'S' COLHEAD,PARTICULAR1,1 SEP,COUNT(REGNO) FROM TEMPTABLEDB..TEMPCHITCOLLECT WHERE RESULT = 2 GROUP BY PARTICULAR1    "
        End If
        strSql += vbCrLf & " INSERT INTO TEMPTABLEDB..TEMPCHITCOLLECT(PARTICULAR,PRINCIPAL,ADDAMT,DEDCAMT,AMOUNT,RESULT,COLHEAD,"
        If rdbDetail.Checked Then strSql += vbCrLf & " TRANDATE1,"
        strSql += vbCrLf & " SEP)SELECT 'GRAND TOTAL',SUM(PRINCIPAL),SUM(ADDAMT),SUM(DEDCAMT)"
        strSql += vbCrLf & " ,SUM(AMOUNT),4 RESULT,'G' COLHEAD,"
        If rdbDetail.Checked Then strSql += vbCrLf & " NULL TRANDATE1,"
        strSql += vbCrLf & " 2 SEP FROM TEMPTABLEDB..TEMPCHITCOLLECT WHERE RESULT = 2 END        "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        'PARTICULAR, TRANDATE, TRANNO, REGNO, PRINCIPAL, ADDAMT, DEDCAMT, AMOUNT, systemId
        ',RESULT,COLHEAD,TRANDATE1, SEP

        If rdbDetail.Checked = True Then
            strSql = vbCrLf & " SELECT TRANNO BILLNO,TRANDATE BILLDATE,PARTICULAR,GROUPNAME,REGNO,PRINCIPAL,ADDAMT ADDITION,DEDCAMT DEDUCTION,AMOUNT,CUSTOMER, COMPANYNAME, COLHEAD,SYSTEMID"
            strSql += vbCrLf & " FROM TEMPTABLEDB..TEMPCHITCOLLECT"
            strSql += vbCrLf & " ORDER BY SEP,PARTICULAR1,RESULT,TRANDATE1,TRANNO,COLHEAD"
        Else
            strSql = vbCrLf & " SELECT PARTICULAR,PRINCIPAL,ADDAMT ADDITION,DEDCAMT DEDUCTION,AMOUNT,COLHEAD, COMPANYNAME"
            strSql += vbCrLf & " FROM TEMPTABLEDB..TEMPCHITCOLLECT"
            strSql += vbCrLf & " ORDER BY SEP,PARTICULAR1,RESULT,COLHEAD"
        End If
    End Function

    Private Function CreditCard() As Integer
        '--/* CREDIT CARD */
        '--/* DETAIL */
        '--BILLNO
        '--BILLDATE
        '--CARDNAME (CARDID CREDIT CARD MAST)
        '--CARDNO (CHQ.CARDNO)
        '--APP.NO (CHQ.CARD REF)
        '--AMOUNT

        '--BILLDATE
        '--CARDNAME
        '--AMOUNT
        strSql = vbCrLf & " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPCCARDCOLLECT')>0 "
        strSql += vbCrLf & " 	DROP TABLE TEMPTABLEDB..TEMPCCARDCOLLECT    "
        strSql += vbCrLf & " SELECT "
        strSql += vbCrLf & " 	 CONVERT(VARCHAR,TRANDATE,103) BILLDATE"
        If rdbDetail.Checked = True Then strSql += vbCrLf & " 	,TRANNO,CARDNO,CHQCARDREF,CASE WHEN ISNULL(CUSTOMER,'') = '' THEN REMARK1 ELSE CUSTOMER END CUSTOMER /* DETAIL */	"
        strSql += vbCrLf & " 	,SUM(AMOUNT) AMOUNT, COMPANYNAME"
        strSql += vbCrLf & " 	,SYSTEMID,RESULT,CONVERT(VARCHAR(3),COLHEAD)COLHEAD"
        strSql += vbCrLf & " 	,PARTICULAR"
        If ChkGrpcounter.Checked Then
            strSql += vbCrLf & " 	,CASHCOUNTER PARTICULAR1"
        ElseIf chkGroupbyFromFlag.Checked = True Then
            strSql += vbCrLf & " 	,JEWELCHIT PARTICULAR1"
        Else
            strSql += vbCrLf & " 	,PARTICULAR PARTICULAR1"
        End If
        strSql += vbCrLf & " 	,TRANDATE TRANDATE1,1 SEP,CASHCOUNTER,JEWELCHIT,REMARK1,REMARK2,ORDERFLAG"
        strSql += vbCrLf & " INTO TEMPTABLEDB..TEMPCCARDCOLLECT     "
        strSql += vbCrLf & " FROM (    "
        strSql += vbCrLf & " 		SELECT "
        strSql += vbCrLf & " 			 (SELECT TOP 1 NAME FROM " & cnAdminDb & "..CREDITCARD WHERE CARDCODE = T.CARDID)AS PARTICULAR,TRANDATE			 "
        strSql += vbCrLf & " 			 ,TRANNO,CHQCARDNO CARDNO,CHQCARDREF "
        'CALNO 271112
        strSql += vbCrLf & " 			,(SELECT CASE WHEN INITIAL <> '' THEN INITIAL + '.' ELSE '' END + PNAME AS NAME FROM " & cnAdminDb & "..PERSONALINFO "
        strSql += vbCrLf & " 			WHERE SNO =(SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO=T.BATCHNO))AS CUSTOMER /* DETAIL */			 		 "
        strSql += vbCrLf & " 			,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1 * AMOUNT END AMOUNT			"
        strSql += vbCrLf & " ,(SELECT TOP 1 COMPANYNAME FROM " & cnAdminDb & "..COMPANY  WHERE COMPANYID  = T.COMPANYID)AS COMPANYNAME,T.COMPANYID "
        strSql += vbCrLf & " 			,SYSTEMID,CONVERT(VARCHAR(1),NULL)COLHEAD,CONVERT(INT,2) RESULT"
        'CALNO 120214
        strSql += vbCrLf & " 			,(SELECT TOP 1 CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHID = T.CASHID)AS CASHCOUNTER"
        strSql += vbCrLf & " 			,CASE WHEN FROMFLAG='C' THEN 'CHIT' ELSE CASE WHEN FROMFLAG='P' THEN 'JEWEL' ELSE NULL END END JEWELCHIT"
        strSql += vbCrLf & " 			,REMARK1,REMARK2,CASE WHEN FROMFLAG = 'P' THEN 0 ELSE 1 END ORDERFLAG"
        strSql += vbCrLf & " 		FROM " & cnStockDb & "..ACCTRAN T     "
        strSql += Filtration("'CC'")
        strSql += vbCrLf & " )X    "
        strSql += vbCrLf & " GROUP BY PARTICULAR,TRANDATE"
        If rdbDetail.Checked = True Then strSql += vbCrLf & " ,TRANNO,CARDNO,CHQCARDREF,CUSTOMER"
        strSql += vbCrLf & " ,SYSTEMID,COLHEAD,RESULT,CASHCOUNTER, COMPANYNAME,JEWELCHIT,REMARK1,REMARK2,ORDERFLAG"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If rdbDetail.Checked = True Then
            strSql = vbCrLf & " /** INSERTING TITLE AND SUBTOTAL AND GRAND TOTAL **/    "
            strSql += vbCrLf & " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPCCARDCOLLECT)>0    "
            strSql += vbCrLf & " BEGIN    "
            If ChkGrpcounter.Checked = True Or chkGroupbyFromFlag.Checked = True Then
                strSql += vbCrLf & " INSERT INTO TEMPTABLEDB..TEMPCCARDCOLLECT(BILLDATE,AMOUNT,RESULT,COLHEAD,PARTICULAR1,TRANDATE1,SEP,JEWELCHIT,ORDERFLAG)"
                strSql += vbCrLf & " SELECT DISTINCT PARTICULAR1,NULL AMOUNT,0 RESULT,'T' COLHEAD,PARTICULAR1,NULL TRANDATE1,1 SEP,'' JEWELCHIT,0 ORDERFLAG "
                strSql += vbCrLf & " FROM TEMPTABLEDB..TEMPCCARDCOLLECT WHERE RESULT = 2  "
            End If
            strSql += vbCrLf & " INSERT INTO TEMPTABLEDB..TEMPCCARDCOLLECT(PARTICULAR,AMOUNT,RESULT,COLHEAD,PARTICULAR1,TRANDATE1,SEP,JEWELCHIT,ORDERFLAG)"
            strSql += vbCrLf & " SELECT 'SUB TOTAL',SUM(AMOUNT),3 RESULT,'S' COLHEAD,PARTICULAR1,NULL TRANDATE1,1 SEP,'' JEWELCHIT,0 ORDERFLAG "
            strSql += vbCrLf & " FROM TEMPTABLEDB..TEMPCCARDCOLLECT WHERE RESULT = 2 GROUP BY PARTICULAR1    "

            strSql += vbCrLf & " INSERT INTO TEMPTABLEDB..TEMPCCARDCOLLECT(PARTICULAR,AMOUNT,RESULT,COLHEAD,PARTICULAR1,TRANDATE1,SEP,JEWELCHIT,ORDERFLAG)"
            strSql += vbCrLf & " SELECT 'GRAND TOTAL',SUM(AMOUNT),4 RESULT,'G' COLHEAD,NULL PARTICULAR1,NULL TRANDATE1,2 SEP,'' JEWELCHIT,0 ORDERFLAG "
            strSql += vbCrLf & " FROM TEMPTABLEDB..TEMPCCARDCOLLECT WHERE RESULT = 2    "
            strSql += vbCrLf & " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()


            strSql = vbCrLf & " SELECT TRANNO BILLNO,BILLDATE,PARTICULAR CARDNAME"
            If rdbDetail.Checked = True Then strSql += vbCrLf & " ,CARDNO,CHQCARDREF APPNO"
            strSql += vbCrLf & " ,AMOUNT,CUSTOMER, COMPANYNAME, COLHEAD,SYSTEMID,JEWELCHIT,REMARK1,REMARK2 FROM TEMPTABLEDB..TEMPCCARDCOLLECT "
            strSql += vbCrLf & " ORDER BY SEP,PARTICULAR1,RESULT,TRANDATE1,COLHEAD,ORDERFLAG,TRANNO"
        Else
            strSql = vbCrLf & " IF OBJECT_ID('TEMPTABLEDB..TEMPCCPARTICULAR') IS NOT NULL "
            strSql += vbCrLf & " 	DROP TABLE TEMPTABLEDB..TEMPCCPARTICULAR"
            strSql += vbCrLf & " CREATE TABLE TEMPTABLEDB..TEMPCCPARTICULAR(QRYCOL VARCHAR(4000))"
            strSql += vbCrLf & " DECLARE @STRQRY AS VARCHAR(4000)"
            strSql += vbCrLf & " DECLARE @PARTICULAR AS VARCHAR(250)"
            strSql += vbCrLf & " SELECT @STRQRY = ''"
            strSql += vbCrLf & " DECLARE CURCRCARD CURSOR "
            strSql += vbCrLf & " 	FOR SELECT DISTINCT ISNULL(PARTICULAR1,'EMPTYCOL') PARTICULAR1 FROM TEMPTABLEDB..TEMPCCARDCOLLECT ORDER BY PARTICULAR1"
            strSql += vbCrLf & " OPEN CURCRCARD"
            strSql += vbCrLf & " WHILE 1=1"
            strSql += vbCrLf & " BEGIN"
            strSql += vbCrLf & " FETCH NEXT FROM CURCRCARD INTO @PARTICULAR"
            strSql += vbCrLf & " 	IF @@FETCH_STATUS = -1  BREAK 	"
            strSql += vbCrLf & " 	SELECT @STRQRY = @STRQRY + ' SUM(CASE WHEN PARTICULAR1 = '''+ @PARTICULAR +''' THEN AMOUNT ELSE 0 END) ['+ @PARTICULAR +'],'	 	 	"
            strSql += vbCrLf & " 	PRINT @STRQRY"
            strSql += vbCrLf & " END"
            strSql += vbCrLf & " CLOSE CURCRCARD"
            strSql += vbCrLf & " DEALLOCATE CURCRCARD"
            strSql += vbCrLf & " SELECT @STRQRY = @STRQRY + ' SUM(AMOUNT) TOTAL'"
            strSql += vbCrLf & " INSERT INTO TEMPTABLEDB..TEMPCCPARTICULAR"
            strSql += vbCrLf & " (QRYCOL) "
            strSql += vbCrLf & " VALUES(@STRQRY)"
            'strSql += vbCrLf & " VALUES(SUBSTRING(@STRQRY,1,LEN(@STRQRY) -1))"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " SELECT QRYCOL FROM TEMPTABLEDB..TEMPCCPARTICULAR"
            Dim dtQryCol As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtQryCol)
            Dim strQryCol As String = ""
            If dtQryCol.Rows.Count > 0 Then
                strQryCol += dtQryCol.Rows(0).Item("QRYCOL").ToString
            End If

            strSql = vbCrLf & " SELECT BILLDATE,1 SEP,TRANDATE1,COLHEAD,"
            strSql += vbCrLf & strQryCol
            strSql += vbCrLf & " FROM TEMPTABLEDB..TEMPCCARDCOLLECT"
            strSql += vbCrLf & " GROUP BY BILLDATE,TRANDATE1,COLHEAD"
            strSql += vbCrLf & " UNION ALL"
            strSql += vbCrLf & " SELECT 'GRAND TOTAL' BILLDATE,2 SEP,NULL TRANDATE1,'G' COLHEAD,"
            strSql += vbCrLf & strQryCol
            strSql += vbCrLf & " FROM TEMPTABLEDB..TEMPCCARDCOLLECT"
            strSql += vbCrLf & " ORDER BY SEP,TRANDATE1,COLHEAD"
        End If
    End Function

    Private Function Cheque() As Integer

        ' '' '' ''        SUMMARY()
        ' '' '' ''-------
        ' '' '' ''        BILLDATE()
        ' '' '' ''        ACCTCODE()
        ' '' '' ''        AMOUNT()

        ' '' '' ''        Cheque()
        ' '' '' ''------
        ' '' '' ''        DETAIL()
        ' '' '' ''-----
        ' '' '' ''        BILLNO()
        ' '' '' ''        BILLDATE()
        ' '' '' ''        accode()
        ' '' '' ''        CHEQDETAIL(CHQCARDREF)
        ' '' '' ''        CHEQNO(CHQCARDNO)
        ' '' '' ''        CHEQDATE(-CHQDATE)
        ' '' '' ''        rec(AMOUNT - CREDIT)
        ' '' '' ''        pay(amount - DEBIT)

        strSql = vbCrLf & " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPCHQCOLLECT')>0 "
        strSql += vbCrLf & " 	DROP TABLE TEMPTABLEDB..TEMPCHQCOLLECT    "
        strSql += vbCrLf & " SELECT "
        strSql += vbCrLf & " 	 CONVERT(VARCHAR,TRANDATE,103) BILLDATE"
        If rdbDetail.Checked = True Then strSql += vbCrLf & " 	,TRANNO,CHQNO,CHQDETAIL,CHQDATE,CASE WHEN ISNULL(CUSTOMER,'') = '' THEN REMARK1 ELSE CUSTOMER END CUSTOMER /* DETAIL */	"
        strSql += vbCrLf & " 	,SUM(RECEIPT) RECEIPT,SUM(PAYMENT) PAYMENT,ISNULL(SUM(RECEIPT),0) - ISNULL(SUM(PAYMENT),0) AMOUNT"
        strSql += vbCrLf & " 	,COMPANYNAME, SYSTEMID,RESULT,CONVERT(VARCHAR(3),COLHEAD)COLHEAD,PARTICULAR"
        If ChkGrpcounter.Checked Then
            strSql += vbCrLf & " 	,CASHCOUNTER PARTICULAR1"
        ElseIf chkGroupbyFromFlag.Checked = True Then
            strSql += vbCrLf & " 	,JEWELCHIT PARTICULAR1"
        Else
            strSql += vbCrLf & " 	,PARTICULAR PARTICULAR1"
        End If
        strSql += vbCrLf & " 	,TRANDATE TRANDATE1,1 SEP,CASHCOUNTER,REMARK1,REMARK2,ORDERFLAG"
        strSql += vbCrLf & " 	,JEWELCHIT"
        strSql += vbCrLf & " INTO TEMPTABLEDB..TEMPCHQCOLLECT     "
        strSql += vbCrLf & " FROM (    "
        strSql += vbCrLf & " 		SELECT "
        strSql += vbCrLf & " 			 (SELECT TOP 1 ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ISNULL(ACCODE,'') = T.ACCODE)AS PARTICULAR,TRANDATE			 "
        strSql += vbCrLf & " 			 ,TRANNO,CHQCARDNO CHQNO,CHQCARDREF CHQDETAIL,CHQDATE "
        'CALNO 271112
        strSql += vbCrLf & " 			,(SELECT CASE WHEN INITIAL <> '' THEN INITIAL + '.' ELSE '' END + PNAME AS NAME FROM " & cnAdminDb & "..PERSONALINFO "
        strSql += vbCrLf & " 			WHERE SNO =(SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO=T.BATCHNO))AS CUSTOMER /* DETAIL */			 		 "
        strSql += vbCrLf & " 			,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE 0 END RECEIPT			"
        strSql += vbCrLf & " 			,CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE 0 END PAYMENT			"
        strSql += vbCrLf & " ,(SELECT TOP 1 COMPANYNAME FROM " & cnAdminDb & "..COMPANY  WHERE COMPANYID  = T.COMPANYID)AS COMPANYNAME,T.COMPANYID, SYSTEMID "
        strSql += vbCrLf & " 			,CONVERT(VARCHAR(1),NULL)COLHEAD,CONVERT(INT,2) RESULT"
        strSql += vbCrLf & " 			,REMARK1,REMARK2,CASE WHEN FROMFLAG = 'P' THEN 0 ELSE 1 END ORDERFLAG"
        'CALNO 120214
        strSql += vbCrLf & " 			,(SELECT TOP 1 CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHID = T.CASHID)AS CASHCOUNTER"
        strSql += vbCrLf & " 			,CASE WHEN FROMFLAG='C' THEN 'CHIT' ELSE CASE WHEN FROMFLAG='P' THEN 'JEWEL' ELSE NULL END END JEWELCHIT"
        strSql += vbCrLf & " 		FROM " & cnStockDb & "..ACCTRAN T     "
        strSql += Filtration("'CH'")
        strSql += vbCrLf & " )X    "
        strSql += vbCrLf & " GROUP BY PARTICULAR,TRANDATE"
        If rdbDetail.Checked = True Then strSql += vbCrLf & " ,TRANNO,CHQNO,CHQDETAIL,CHQDATE,CUSTOMER"
        strSql += vbCrLf & " ,SYSTEMID,COLHEAD,RESULT,CASHCOUNTER, COMPANYNAME,REMARK1,REMARK2,ORDERFLAG,JEWELCHIT"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If rdbDetail.Checked = True Then

            strSql = vbCrLf & " /** INSERTING TITLE AND SUBTOTAL AND GRAND TOTAL **/    "
            strSql += vbCrLf & " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPCHQCOLLECT)>0    "
            strSql += vbCrLf & " BEGIN    "
            If ChkGrpcounter.Checked Or chkGroupbyFromFlag.Checked Then
                strSql += vbCrLf & " INSERT INTO TEMPTABLEDB..TEMPCHQCOLLECT(BILLDATE,AMOUNT,RESULT,COLHEAD,PARTICULAR1,TRANDATE1,SEP,ORDERFLAG,JEWELCHIT)"
                strSql += vbCrLf & " SELECT DISTINCT PARTICULAR1,NULL AMOUNT,0 RESULT,'T' COLHEAD,PARTICULAR1,NULL TRANDATE1,1 SEP,0 ORDERFLAG,'' JEWELCHIT "
                strSql += vbCrLf & " FROM TEMPTABLEDB..TEMPCHQCOLLECT WHERE RESULT = 2  "
            End If
            strSql += vbCrLf & " INSERT INTO TEMPTABLEDB..TEMPCHQCOLLECT(PARTICULAR,RECEIPT,PAYMENT,AMOUNT,RESULT,COLHEAD,PARTICULAR1,TRANDATE1,SEP,ORDERFLAG,JEWELCHIT)"
            strSql += vbCrLf & " SELECT 'SUB TOTAL',SUM(RECEIPT),SUM(PAYMENT),SUM(AMOUNT),3 RESULT,'S' COLHEAD,PARTICULAR1,NULL TRANDATE1,1 SEP,0 ORDERFLAG,'' JEWELCHIT "
            strSql += vbCrLf & " FROM TEMPTABLEDB..TEMPCHQCOLLECT WHERE RESULT = 2 GROUP BY PARTICULAR1    "

            strSql += vbCrLf & " INSERT INTO TEMPTABLEDB..TEMPCHQCOLLECT(PARTICULAR,RECEIPT,PAYMENT,AMOUNT,RESULT,COLHEAD,PARTICULAR1,TRANDATE1,SEP,ORDERFLAG,JEWELCHIT)"
            strSql += vbCrLf & " SELECT 'GRAND TOTAL',SUM(RECEIPT),SUM(PAYMENT),SUM(AMOUNT),4 RESULT,'G' COLHEAD,NULL PARTICULAR1,NULL TRANDATE1,2 SEP,0 ORDERFLAG,''JEWELCHIT "
            strSql += vbCrLf & " FROM TEMPTABLEDB..TEMPCHQCOLLECT WHERE RESULT = 2    "
            strSql += vbCrLf & " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf & " SELECT TRANNO BILLNO,BILLDATE,PARTICULAR ACNAME"
            strSql += vbCrLf & " ,CHQDETAIL,CHQNO,CHQDATE"
            strSql += vbCrLf & " ,RECEIPT,PAYMENT,AMOUNT,CUSTOMER,COLHEAD, COMPANYNAME, SYSTEMID "
            strSql += vbCrLf & " ,REMARK1,REMARK2"
            strSql += vbCrLf & " FROM TEMPTABLEDB..TEMPCHQCOLLECT "
            strSql += vbCrLf & " ORDER BY SEP,PARTICULAR1,RESULT,TRANDATE1,COLHEAD,ORDERFLAG,TRANNO"
        Else
            strSql = vbCrLf & " IF OBJECT_ID('TEMPTABLEDB..TEMPCHPARTICULAR') IS NOT NULL "
            strSql += vbCrLf & " 	DROP TABLE TEMPTABLEDB..TEMPCHPARTICULAR"
            strSql += vbCrLf & " CREATE TABLE TEMPTABLEDB..TEMPCHPARTICULAR(QRYCOL VARCHAR(4000))"
            strSql += vbCrLf & " DECLARE @STRQRY AS VARCHAR(4000)"
            strSql += vbCrLf & " DECLARE @PARTICULAR AS VARCHAR(250)"
            strSql += vbCrLf & " SELECT @STRQRY = ''"
            strSql += vbCrLf & " DECLARE CURCRCARD CURSOR "
            strSql += vbCrLf & " 	FOR SELECT DISTINCT ISNULL(PARTICULAR1,'EMPTYCOL') PARTICULAR1 FROM TEMPTABLEDB..TEMPCHQCOLLECT ORDER BY PARTICULAR1"
            strSql += vbCrLf & " OPEN CURCRCARD"
            strSql += vbCrLf & " WHILE 1=1"
            strSql += vbCrLf & " BEGIN"
            strSql += vbCrLf & " FETCH NEXT FROM CURCRCARD INTO @PARTICULAR"
            strSql += vbCrLf & " 	IF @@FETCH_STATUS = -1  BREAK 	"
            strSql += vbCrLf & " 	SELECT @STRQRY = @STRQRY + ' SUM(CASE WHEN PARTICULAR1 = '''+ @PARTICULAR +''' THEN AMOUNT ELSE 0 END) ['+ @PARTICULAR +'],'	 	 	"
            strSql += vbCrLf & " 	PRINT @STRQRY"
            strSql += vbCrLf & " END"
            strSql += vbCrLf & " CLOSE CURCRCARD"
            strSql += vbCrLf & " DEALLOCATE CURCRCARD"
            strSql += vbCrLf & " SELECT @STRQRY = @STRQRY + ' SUM(AMOUNT) TOTAL'"
            strSql += vbCrLf & " INSERT INTO TEMPTABLEDB..TEMPCHPARTICULAR"
            strSql += vbCrLf & " (QRYCOL) "
            strSql += vbCrLf & " VALUES(@STRQRY)"
            'strSql += vbCrLf & " VALUES(SUBSTRING(@STRQRY,1,LEN(@STRQRY) -1))"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " SELECT QRYCOL FROM TEMPTABLEDB..TEMPCHPARTICULAR"
            Dim dtQryCol As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtQryCol)
            Dim strQryCol As String = ""
            If dtQryCol.Rows.Count > 0 Then
                strQryCol += dtQryCol.Rows(0).Item("QRYCOL").ToString
            End If

            strSql = vbCrLf & " SELECT BILLDATE,1 SEP,TRANDATE1,COLHEAD,"
            strSql += vbCrLf & strQryCol
            strSql += vbCrLf & " FROM TEMPTABLEDB..TEMPCHQCOLLECT"
            strSql += vbCrLf & " GROUP BY BILLDATE,TRANDATE1,COLHEAD"
            strSql += vbCrLf & " UNION ALL"
            strSql += vbCrLf & " SELECT 'GRAND TOTAL' BILLDATE,2 SEP,NULL TRANDATE1,'G' COLHEAD,"
            strSql += vbCrLf & strQryCol
            strSql += vbCrLf & " FROM TEMPTABLEDB..TEMPCHQCOLLECT"
            strSql += vbCrLf & " ORDER BY SEP,TRANDATE1,COLHEAD"
        End If
    End Function
    Private Function GiftVoucher() As Integer
        '--/* CREDIT CARD */
        '--/* DETAIL */
        '--BILLNO
        '--BILLDATE
        '--CARDNAME (CARDID CREDIT CARD MAST)
        '--CARDNO (CHQ.CARDNO)
        '--APP.NO (CHQ.CARD REF)
        '--AMOUNT

        '--BILLDATE
        '--CARDNAME
        '--AMOUNT
        strSql = vbCrLf & " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPCARDCOLLECT')>0 "
        strSql += vbCrLf & " 	DROP TABLE TEMPTABLEDB..TEMPCARDCOLLECT    "
        strSql += vbCrLf & " SELECT "
        strSql += vbCrLf & " 	 CONVERT(VARCHAR,TRANDATE,103) BILLDATE"
        If rdbDetail.Checked = True Then strSql += vbCrLf & " 	,TRANNO,CARDNO,CHQCARDREF,CUSTOMER /* DETAIL */	"
        strSql += vbCrLf & " 	,SUM(AMOUNT) AMOUNT"
        strSql += vbCrLf & " 	,COMPANYNAME,SYSTEMID,RESULT,CONVERT(VARCHAR(3),COLHEAD)COLHEAD,PARTICULAR"
        If ChkGrpcounter.Checked Then
            strSql += vbCrLf & " 	,CASHCOUNTER PARTICULAR1"
        Else
            strSql += vbCrLf & " 	,PARTICULAR PARTICULAR1"
        End If
        strSql += vbCrLf & " 	,TRANDATE TRANDATE1,1 SEP,CASHCOUNTER"
        strSql += vbCrLf & " INTO TEMPTABLEDB..TEMPCARDCOLLECT     "
        strSql += vbCrLf & " FROM (    "
        strSql += vbCrLf & " 		SELECT "
        strSql += vbCrLf & " 			 (SELECT TOP 1 NAME FROM " & cnAdminDb & "..CREDITCARD WHERE CARDCODE = T.CARDID)AS PARTICULAR,TRANDATE			 "
        strSql += vbCrLf & " 			 ,TRANNO,CHQCARDNO CARDNO,CHQCARDREF "
        'CALNO 271112
        strSql += vbCrLf & " 			,(SELECT CASE WHEN INITIAL <> '' THEN INITIAL + '.' ELSE '' END + PNAME AS NAME FROM " & cnAdminDb & "..PERSONALINFO "
        strSql += vbCrLf & " 			WHERE SNO =(SELECT TOP 1 PSNO FROM " & cnAdminDb & "..CUSTOMERINFO WHERE BATCHNO=T.BATCHNO))AS CUSTOMER /* DETAIL */			 		 "
        strSql += vbCrLf & " 			,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1 * AMOUNT END AMOUNT			"
        strSql += vbCrLf & " ,(SELECT TOP 1 COMPANYNAME FROM " & cnAdminDb & "..COMPANY  WHERE COMPANYID  = T.COMPANYID)AS COMPANYNAME,T.COMPANYID"
        strSql += vbCrLf & " 			,SYSTEMID,CONVERT(VARCHAR(1),NULL)COLHEAD,CONVERT(INT,2) RESULT"
        'CALNO 120214
        strSql += vbCrLf & " 			,(SELECT TOP 1 CASHNAME FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHID = T.CASHID)AS CASHCOUNTER"
        strSql += vbCrLf & " 		FROM " & cnStockDb & "..ACCTRAN T     "
        strSql += Filtration("'GV'")
        strSql += vbCrLf & " )X    "
        strSql += vbCrLf & " GROUP BY PARTICULAR,TRANDATE"
        If rdbDetail.Checked = True Then strSql += vbCrLf & " ,TRANNO,CARDNO,CHQCARDREF,CUSTOMER"
        strSql += vbCrLf & " ,SYSTEMID,COLHEAD,RESULT,CASHCOUNTER, COMPANYNAME"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If rdbDetail.Checked = True Then
            strSql = vbCrLf & " /** INSERTING TITLE AND SUBTOTAL AND GRAND TOTAL **/    "
            strSql += vbCrLf & " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPCARDCOLLECT)>0    "
            strSql += vbCrLf & " BEGIN    "
            If ChkGrpcounter.Checked Then
                strSql += vbCrLf & " INSERT INTO TEMPTABLEDB..TEMPCARDCOLLECT(BILLDATE,AMOUNT,RESULT,COLHEAD,PARTICULAR1,TRANDATE1,SEP)"
                strSql += vbCrLf & " SELECT DISTINCT PARTICULAR1,NULL AMOUNT,0 RESULT,'T' COLHEAD,PARTICULAR1,NULL TRANDATE1,1 SEP "
                strSql += vbCrLf & " FROM TEMPTABLEDB..TEMPCARDCOLLECT WHERE RESULT = 2  "
            End If
            strSql += vbCrLf & " INSERT INTO TEMPTABLEDB..TEMPCARDCOLLECT(PARTICULAR,AMOUNT,RESULT,COLHEAD,PARTICULAR1,TRANDATE1,SEP)"
            strSql += vbCrLf & " SELECT 'SUB TOTAL',SUM(AMOUNT),3 RESULT,'S' COLHEAD,PARTICULAR1,NULL TRANDATE1,1 SEP "
            strSql += vbCrLf & " FROM TEMPTABLEDB..TEMPCARDCOLLECT WHERE RESULT = 2 GROUP BY PARTICULAR1    "

            strSql += vbCrLf & " INSERT INTO TEMPTABLEDB..TEMPCARDCOLLECT(PARTICULAR,AMOUNT,RESULT,COLHEAD,PARTICULAR1,TRANDATE1,SEP)"
            strSql += vbCrLf & " SELECT 'GRAND TOTAL',SUM(AMOUNT),4 RESULT,'G' COLHEAD,NULL PARTICULAR1,NULL TRANDATE1,2 SEP "
            strSql += vbCrLf & " FROM TEMPTABLEDB..TEMPCARDCOLLECT WHERE RESULT = 2    "
            strSql += vbCrLf & " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()


            strSql = vbCrLf & " SELECT TRANNO BILLNO,BILLDATE,PARTICULAR CARDNAME"
            If rdbDetail.Checked = True Then strSql += vbCrLf & " ,CARDNO,CHQCARDREF APPNO"
            strSql += vbCrLf & " ,AMOUNT,CUSTOMER,COLHEAD, COMPANYNAME, SYSTEMID FROM TEMPTABLEDB..TEMPCARDCOLLECT "
            strSql += vbCrLf & " ORDER BY SEP,PARTICULAR1,RESULT,TRANDATE1,COLHEAD"
        Else
            strSql = vbCrLf & " IF OBJECT_ID('TEMPTABLEDB..TEMPGVPARTICULAR') IS NOT NULL "
            strSql += vbCrLf & " DROP TABLE TEMPTABLEDB..TEMPGVPARTICULAR"
            strSql += vbCrLf & " CREATE TABLE TEMPTABLEDB..TEMPGVPARTICULAR(QRYCOL VARCHAR(4000))"
            strSql += vbCrLf & " DECLARE @STRQRY AS VARCHAR(4000)"
            strSql += vbCrLf & " DECLARE @PARTICULAR AS VARCHAR(250)"
            strSql += vbCrLf & " SELECT @STRQRY = ''"
            strSql += vbCrLf & " DECLARE CURCRCARD CURSOR "
            strSql += vbCrLf & " 	FOR SELECT DISTINCT ISNULL(PARTICULAR1,'EMPTYCOL') PARTICULAR1 FROM TEMPTABLEDB..TEMPCARDCOLLECT ORDER BY PARTICULAR1"
            strSql += vbCrLf & " OPEN CURCRCARD"
            strSql += vbCrLf & " WHILE 1=1"
            strSql += vbCrLf & " BEGIN"
            strSql += vbCrLf & " FETCH NEXT FROM CURCRCARD INTO @PARTICULAR"
            strSql += vbCrLf & " 	IF @@FETCH_STATUS = -1  BREAK 	"
            strSql += vbCrLf & " 	SELECT @STRQRY = @STRQRY + ' SUM(CASE WHEN PARTICULAR1 = '''+ @PARTICULAR +''' THEN AMOUNT ELSE 0 END) ['+ @PARTICULAR +'],'	 	 	"
            strSql += vbCrLf & " 	PRINT @STRQRY"
            strSql += vbCrLf & " END"
            strSql += vbCrLf & " CLOSE CURCRCARD"
            strSql += vbCrLf & " DEALLOCATE CURCRCARD"
            strSql += vbCrLf & " SELECT @STRQRY = @STRQRY + ' SUM(AMOUNT) TOTAL'"
            strSql += vbCrLf & " INSERT INTO TEMPTABLEDB..TEMPGVPARTICULAR"
            strSql += vbCrLf & " (QRYCOL) "
            strSql += vbCrLf & " VALUES(@STRQRY)"
            'strSql += vbCrLf & " VALUES(SUBSTRING(@STRQRY,1,LEN(@STRQRY) -1))"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " SELECT QRYCOL FROM TEMPTABLEDB..TEMPGVPARTICULAR"
            Dim dtQryCol As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtQryCol)
            Dim strQryCol As String = ""
            If dtQryCol.Rows.Count > 0 Then
                strQryCol += dtQryCol.Rows(0).Item("QRYCOL").ToString
            End If

            strSql = vbCrLf & " SELECT BILLDATE,1 SEP,TRANDATE1,COLHEAD,"
            strSql += vbCrLf & strQryCol
            strSql += vbCrLf & " FROM TEMPTABLEDB..TEMPCARDCOLLECT"
            strSql += vbCrLf & " GROUP BY BILLDATE,TRANDATE1,COLHEAD"
            strSql += vbCrLf & " UNION ALL"
            strSql += vbCrLf & " SELECT 'GRAND TOTAL' BILLDATE,2 SEP,NULL TRANDATE1,'G' COLHEAD,"
            strSql += vbCrLf & strQryCol
            strSql += vbCrLf & " FROM TEMPTABLEDB..TEMPCARDCOLLECT"
            strSql += vbCrLf & " ORDER BY SEP,TRANDATE1,COLHEAD"
        End If
    End Function
    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        ''Try
        'If chkLstCostCentre.Enabled = True Then
        '    If Not chkLstCostCentre.CheckedItems.Count > 0 Then
        '        chkLstCostCentre.SetItemChecked(0, True)
        '    End If
        'End If
        'If Not chkLstNodeId.CheckedItems.Count > 0 Then
        'If chkLstNodeId.Items.Count = 0 Then
        ' funcAddNodeId()
        'End If
        '    chkLstNodeId.SetItemChecked(0, True)
        'End If
        'SelectedCompany = GetSelectedCompanyId(chkLstCompany, False)
        'Dim CostFtr As String = funcCostName()
        'Dim NodeFtr As String = funcSystemId()
        'Dim Type As String = ""
        'If chkChitCard.Checked Then Type += "SS,CG,CB,CZ,CD,"
        'If chkCreditCard.Checked Then Type += "CC,"
        'If chkCheque.Checked Then Type += "CH,"

        'If Type <> "" Then Type = Mid(Type, 1, Type.Length - 1)

        'strSql = " EXEC " & cnStockDb & "..SP_RPT_CARDCOLLECTION"
        'strSql += vbCrLf + " @FRMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        'strSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        'strSql += vbCrLf + " ,@TYPE = '" & Type & "'"
        'strSql += vbCrLf + " ,@NODEID = '" & NodeFtr & "'"
        'strSql += vbCrLf + " ,@SYSTEMID = '" & systemId & "'"
        'strSql += vbCrLf + " ,@COSTNAME = '" & CostFtr & "'"
        'strSql += vbCrLf + " ,@COMPANYID = '" & SelectedCompany & "'"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()

        'strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "CARDCOLLECTION  ORDER BY PAYMODE,CARDNAME,RESULT,PARTICULAR,TRANDATE,TRANNO"
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        If rdbChitCard.Checked Or rbtAll.Checked Then ChitCard()
        If rdbCreditCard.Checked Or rbtAll.Checked Then CreditCard()
        If rdbCheque.Checked Or rbtAll.Checked Then Cheque()
        If rdbGiftVoucher.Checked Or rbtAll.Checked Then GiftVoucher()
        Dim qry As String = ""
        If rdbDetail.Checked = True And rbtAll.Checked Then
            strSql = vbCrLf & " SELECT NULL BILLNO,NULL  BILLDATE,'SCHEME' PARTICULAR"
            strSql += vbCrLf & " ,NULL GROUPNAME,NULL REGNO,CONVERT(SMALLDATETIME,NULL) CHQDATE,NULL PRINCIPAL,NULL  RECEIPT,NULL  PAYMENT"
            strSql += vbCrLf & " ,NULL AMOUNT,NULL CUSTOMER, NULL COMPANYNAME, 'T' COLHEAD,NULL SYSTEMID,NULL SEP,0 RESULT,1 ORD,NULL PARTICULAR1,NULL TRANDATE1 "
            strSql += vbCrLf & " UNION ALL"
            strSql += vbCrLf & " SELECT TRANNO BILLNO,TRANDATE BILLDATE,PARTICULAR"
            strSql += vbCrLf & " ,GROUPNAME,REGNO,CONVERT(SMALLDATETIME,NULL) CHQDATE,PRINCIPAL,ADDAMT RECEIPT,DEDCAMT PAYMENT"
            strSql += vbCrLf & " ,AMOUNT,CUSTOMER, COMPANYNAME, COLHEAD,SYSTEMID,SEP,RESULT,1 ORD,PARTICULAR1,TRANDATE1 FROM TEMPTABLEDB..TEMPCHITCOLLECT"
            strSql += vbCrLf & " UNION ALL"

            strSql += vbCrLf & " SELECT NULL BILLNO,NULL  BILLDATE,'CREDIT CARD' PARTICULAR"
            strSql += vbCrLf & " ,NULL GROUPNAME,NULL REGNO,CONVERT(SMALLDATETIME,NULL) CHQDATE,NULL PRINCIPAL,NULL  RECEIPT,NULL  PAYMENT"
            strSql += vbCrLf & " ,NULL AMOUNT,NULL CUSTOMER, NULL COMPANYNAME, 'T' COLHEAD,NULL SYSTEMID,NULL SEP,0 RESULT,2 ORD,NULL PARTICULAR1,NULL TRANDATE1 "
            strSql += vbCrLf & " UNION ALL"
            strSql += vbCrLf & " SELECT TRANNO BILLNO,BILLDATE,PARTICULAR CARDNAME"
            strSql += vbCrLf & " ,CARDNO,CHQCARDREF APPNO,NULL CHQDATE,NULL PRINCIPAL,NULL RECEIPT,NULL PAYMENT"
            strSql += vbCrLf & " ,AMOUNT,CUSTOMER,COMPANYNAME, COLHEAD,SYSTEMID,SEP,RESULT,2 ORD,PARTICULAR1,TRANDATE1 FROM TEMPTABLEDB..TEMPCCARDCOLLECT "
            strSql += vbCrLf & " UNION ALL"

            strSql += vbCrLf & " SELECT NULL BILLNO,NULL  BILLDATE,'CHEQUE' PARTICULAR"
            strSql += vbCrLf & " ,NULL GROUPNAME,NULL REGNO,CONVERT(SMALLDATETIME,NULL) CHQDATE,NULL PRINCIPAL,NULL  RECEIPT,NULL  PAYMENT"
            strSql += vbCrLf & " ,NULL AMOUNT,NULL CUSTOMER, NULL COMPANYNAME, 'T' COLHEAD,NULL SYSTEMID,NULL SEP,0 RESULT,3 ORD,NULL PARTICULAR1,NULL TRANDATE1 "
            strSql += vbCrLf & " UNION ALL"
            strSql += vbCrLf & " SELECT TRANNO BILLNO,BILLDATE,PARTICULAR"
            strSql += vbCrLf & " ,CHQDETAIL,CHQNO,CHQDATE,NULL PRINCIPAL,RECEIPT,PAYMENT"
            strSql += vbCrLf & " ,AMOUNT,CUSTOMER, COMPANYNAME, COLHEAD,SYSTEMID,SEP,RESULT,3 ORD,PARTICULAR1,TRANDATE1 FROM TEMPTABLEDB..TEMPCHQCOLLECT "
            strSql += vbCrLf & " UNION ALL"

            strSql += vbCrLf & " SELECT NULL BILLNO,NULL  BILLDATE,'GIFT VOUCHER' PARTICULAR"
            strSql += vbCrLf & " ,NULL GROUPNAME,NULL REGNO,CONVERT(SMALLDATETIME,NULL) CHQDATE,NULL PRINCIPAL,NULL  RECEIPT,NULL  PAYMENT"
            strSql += vbCrLf & " ,NULL AMOUNT,NULL CUSTOMER, NULL COMPANYNAME, 'T' COLHEAD,NULL SYSTEMID,NULL SEP,0 RESULT,4 ORD,NULL PARTICULAR1,NULL TRANDATE1 "
            strSql += vbCrLf & " UNION ALL"
            strSql += vbCrLf & " SELECT TRANNO BILLNO,BILLDATE,PARTICULAR"
            strSql += vbCrLf & " ,CARDNO,CHQCARDREF APPNO,NULL CHQDATE,NULL PRINCIPAL,NULL RECEIPT,NULL PAYMENT"
            strSql += vbCrLf & " ,AMOUNT,CUSTOMER,COMPANYNAME, COLHEAD,SYSTEMID,SEP,RESULT,4 ORD,PARTICULAR1,TRANDATE1 FROM TEMPTABLEDB..TEMPCARDCOLLECT "
            strSql += vbCrLf & " ORDER BY ORD,SEP,PARTICULAR1,RESULT,TRANDATE1,BILLNO,COLHEAD"
        End If


        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)

        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        'objGridShower.Size = New Size(778, 550)
        objGridShower.Text = "CARD COLLECTION REPORT"
        Dim tit As String = "CARD COLLECTION REPORT FROM " & dtpFrom.Text & " TO " & dtpTo.Text & "" & vbCrLf & "        "
        If rdbChitCard.Checked = True Then tit += "[SCHEME]"
        If rdbCreditCard.Checked = True Then tit += "[CREDIT CARD]"
        If rdbCheque.Checked = True Then tit += "[CHEQUE]"
        Dim Cname As String = ""
        If chkLstCostCentre.Items.Count > 0 And chkLstCostCentre.CheckedItems.Count > 0 Then
            For CNT As Integer = 0 To chkLstCostCentre.Items.Count - 1
                If chkLstCostCentre.GetItemChecked(CNT) = True Then
                    Cname += "" & chkLstCostCentre.Items(CNT) + ","
                End If
            Next
            If Cname <> "" Then Cname = " :" + Mid(Cname, 1, Len(Cname) - 1)
        End If
        objGridShower.lblTitle.Text = tit + Cname
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False

        If rdbChitCard.Checked = True Then
            DataGridView_SummaryFormatting_ChitCard(objGridShower.gridView)
            objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))

        ElseIf rdbCreditCard.Checked Then
            DataGridView_SummaryFormatting_CreditCard(objGridShower.gridView)
            If rdbDetail.Checked = True Then
                objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("BILLNO")))
            Else
                objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("BILLDATE")))
            End If
        ElseIf rdbCheque.Checked Then
            DataGridView_SummaryFormatting_Cheque(objGridShower.gridView)
            If rdbDetail.Checked = True Then
                objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("BILLNO")))
            Else
                objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("BILLDATE")))
            End If
        End If


        objGridShower.Show()
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)

        If rbtAll.Checked Then
            If objGridShower.gridView.Columns.Contains("CHQDATE") Then objGridShower.gridView.Columns("CHQDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            If objGridShower.gridView.Columns.Contains("SEP") Then objGridShower.gridView.Columns("SEP").Visible = False
            If objGridShower.gridView.Columns.Contains("COLHEAD") Then objGridShower.gridView.Columns("COLHEAD").Visible = False
            If objGridShower.gridView.Columns.Contains("RESULT") Then objGridShower.gridView.Columns("RESULT").Visible = False
            If objGridShower.gridView.Columns.Contains("ORD") Then objGridShower.gridView.Columns("ORD").Visible = False
            If objGridShower.gridView.Columns.Contains("KEYNO") Then objGridShower.gridView.Columns("KEYNO").Visible = False
            If objGridShower.gridView.Columns.Contains("PARTICULAR1") Then objGridShower.gridView.Columns("PARTICULAR1").Visible = False
            If objGridShower.gridView.Columns.Contains("TRANDATE1") Then objGridShower.gridView.Columns("TRANDATE1").Visible = False
        End If
        Prop_Sets()

    End Sub

    Private Sub DataGridView_SummaryFormatting_CreditCard(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            FormatGridColumns(dgv, False, False, , False)

            .Columns("BILLDATE").Width = 100
            If rdbDetail.Checked = True Then
                .Columns("CARDNAME").Width = 120
                If rdbDetail.Checked = True Then
                    .Columns("BILLNO").Width = 60
                    .Columns("CARDNO").Width = 120
                    .Columns("APPNO").Width = 120
                    'CALNO 271112
                    .Columns("CUSTOMER").Width = 120
                End If
                .Columns("AMOUNT").Width = 100
                .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
            Else
                For cnt As Integer = 1 To dgv.Columns.Count - 1
                    .Columns(cnt).Width = 100
                Next
                .Columns("TRANDATE1").Visible = False
                .Columns("SEP").Visible = False
            End If
            .Columns("COLHEAD").Visible = False
            .Columns("KEYNO").Visible = False
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub

    Private Sub DataGridView_SummaryFormatting_Cheque(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            FormatGridColumns(dgv, False, False, , False)

            ' '' '' ''        BILLNO()
            ' '' '' ''        BILLDATE()
            ' '' '' ''        accode()
            ' '' '' ''        CHEQDETAIL(CHQCARDREF)
            ' '' '' ''        CHEQNO(CHQCARDNO)
            ' '' '' ''        CHEQDATE(-CHQDATE)
            ' '' '' ''        rec(AMOUNT - CREDIT)
            ' '' '' ''        pay(amount - DEBIT)
            '-------
            '            BILLDATE()
            '            CARDNAME()
            '            AMOUNT()
            .Columns("BILLDATE").Width = 100
            If rdbDetail.Checked = True Then
                .Columns("ACNAME").Width = 120
                .Columns("BILLNO").Width = 60
                .Columns("CHQDETAIL").Width = 100
                .Columns("CHQNO").Width = 120
                .Columns("CHQDATE").Width = 120
                .Columns("RECEIPT").Width = 100
                .Columns("RECEIPT").DefaultCellStyle.Format = "0.00"
                .Columns("PAYMENT").Width = 100
                .Columns("PAYMENT").DefaultCellStyle.Format = "0.00"
                .Columns("AMOUNT").Width = 100
                .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
                'CALNO 271112
                .Columns("CUSTOMER").Width = 120
            Else
                For cnt As Integer = 1 To dgv.Columns.Count - 1
                    .Columns(cnt).Width = 100
                Next
                .Columns("TRANDATE1").Visible = False
                .Columns("SEP").Visible = False
            End If
            If .Columns.Contains("CHQDATE") Then .Columns("CHQDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("COLHEAD").Visible = False
            .Columns("KEYNO").Visible = False
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub

    Private Sub DataGridView_SummaryFormatting_ChitCard(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            FormatGridColumns(dgv, False, False, , False)

            '        DETAIL()
            '------
            '        BILLNO()
            '        BILLDATE()
            '        GROUP(CHQCARDREF)
            '        REGNO(CHQCARDNO)
            '        PRINCIPLE()
            'ADDITION DEBIT + (CG,CB,
            'DEDUCTION CREDIT + (CG,CB,CZ,CD)
            '        AMOUNT(SS)

            '        SUMMARY()
            '-------
            '        BILLDATE()
            '        PARTICULARS(ACCOUNTNAME)(CHQCARDREF)
            '        PRINCIPLE()
            '        ADDITION()
            '        DEDUCTION()
            '        AMOUNT()


            If rdbDetail.Checked = True Then
                .Columns("BILLNO").Width = 60
                .Columns("GROUPNAME").Width = 100
                .Columns("GROUPNAME").HeaderText = "GROUP"
                .Columns("REGNO").Width = 60
                'CALNO 271112
                .Columns("CUSTOMER").Width = 120
                .Columns("BILLDATE").Width = 100
            End If
            .Columns("PARTICULAR").Width = 100
            .Columns("PRINCIPAL").Width = 100
            .Columns("PRINCIPAL").DefaultCellStyle.Format = "0.00"
            .Columns("ADDITION").Width = 100
            .Columns("ADDITION").DefaultCellStyle.Format = "0.00"
            .Columns("DEDUCTION").Width = 100
            .Columns("DEDUCTION").DefaultCellStyle.Format = "0.00"
            .Columns("AMOUNT").Width = 100
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
            .Columns("COLHEAD").Visible = False
            .Columns("KEYNO").Visible = False
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetEntryDate(GetServerDate)
        dtpTo.Value = GetEntryDate(GetServerDate)
        Prop_Gets()
        funcNew()
        funcAddCostName()
        'funcAddNodeId()
        dtpFrom.Select()
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Function funcNew() As Integer
        Try
            dtCardCollection.Clear()
            lblTitle.Text = "TITLE"
            strSql = "select ''BATCHNO,''TRANNO,''TRANDATE,''CHQCARDREF,' 'AMOUNT,'' SYSTEMID,''CHQCARDNO,1 RESULT,''DUMMYDATE,''DUMMYNO where 1<>1"
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtCardCollection)
            gridView.DataSource = dtCardCollection
            funcGridStyle()
            lblTitle.Height = gridView.ColumnHeadersHeight
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function

    Function funcGridStyle() As Integer
        With gridView
            .Columns("RESULT").Visible = False
            .Columns("BATCHNO").Visible = False
            .Columns("DUMMYDATE").Visible = False
            .Columns("DUMMYNO").Visible = False
            With .Columns("TRANNO")
                .Width = 100
                .HeaderText = "BILLNO"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TRANDATE")
                .Width = 100
                .HeaderText = "BILLDATE"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CHQCARDREF")
                .Width = 150
                .HeaderText = "DESCRIPTION"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("AMOUNT")
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SYSTEMID")
                .Width = 100
                .HeaderText = "NODEID"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CHQCARDNO")
                .Width = 100
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
    End Function

    Function funcCellColor() As Integer
        For rowCount As Integer = 0 To dtCardCollection.Rows.Count - 1
            If gridView.Rows(rowCount).Cells(1).Value = "CHITCARD" Or gridView.Rows(rowCount).Cells(1).Value = "CREDIT" Or gridView.Rows(rowCount).Cells(1).Value = "CHEQUE" Then
                gridView.Rows(rowCount).Cells(1).Style.BackColor = Color.LightBlue
            End If
        Next
    End Function

    Function funcHeaderCall() As String
        'Query for add the Header to tables
        Dim strHeaderCall As String = Nothing
        strHeaderCall += " select BATCHNO,CONVERT(VARCHAR,TranNo)TRANNO,CONVERT(VARCHAR,TranDate,103)TRANDATE"
        strHeaderCall += ",CHQCARDREF,CONVERT(VARCHAR,Amount)AMOUNT,SYSTEMID"
        strHeaderCall += ",CHQCARDNO,' ' RESULT"
        Return strHeaderCall
    End Function

    Function funcExecute() As Integer
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    ''Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
    ''    With gridView
    ''        Select Case .Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''            Case "T"
    ''                .Rows(e.RowIndex).Cells("TRANNO").Style.BackColor = reportHeadStyle.BackColor
    ''                .Rows(e.RowIndex).Cells("TRANNO").Style.Font = reportHeadStyle.Font
    ''        End Select
    ''    End With
    ''End Sub

    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "T"
                        .Cells("TRANNO").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("TRANNO").Style.Font = reportHeadStyle.Font
                End Select
            End With
        Next
    End Function
    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnNew.Focus()
        End If
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        tabMain.SelectedTab = tabGen
        dtpFrom.Focus()
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmCardCollectionsReport_New_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmCardCollectionsReport_New_Properties))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, "ALL")
        SetChecked_CheckedList(chkLstNodeId, obj.p_chkLstNodeId, "ALL")
        rdbChitCard.Checked = obj.p_chkChitCard
        rdbCreditCard.Checked = obj.p_chkCreditCard
        rdbCheque.Checked = obj.p_chkCheque
        ChkInclChit.Checked = obj.p_chkInclChit
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmCardCollectionsReport_New_Properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        GetChecked_CheckedList(chkLstNodeId, obj.p_chkLstNodeId)
        obj.p_chkChitCard = rdbChitCard.Checked
        obj.p_chkCreditCard = rdbCreditCard.Checked
        obj.p_chkCheque = rdbCheque.Checked
        obj.p_chkInclChit = ChkInclChit.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmCardCollectionsReport_New_Properties))
    End Sub

    Private Sub dtpTo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpTo.LostFocus
        funcAddNodeId()
    End Sub

    Private Sub chkLstNodeId_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstNodeId.GotFocus
        If chkLstNodeId.Items.Count = 0 Then
            funcAddNodeId()
        End If
    End Sub

    Private Sub rdbDetail_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbDetail.CheckedChanged
        If rdbDetail.Checked = True Then
            ChkGrpcounter.Enabled = True
            chkGroupbyFromFlag.Enabled = True
        Else
            ChkGrpcounter.Enabled = False
            ChkGrpcounter.Checked = False

            chkGroupbyFromFlag.Enabled = False
            chkGroupbyFromFlag.Checked = False
        End If
    End Sub

    Private Sub rbtAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtAll.CheckedChanged
        If rbtAll.Checked Then
            rdbDetail.Checked = True
            rdbSummary.Enabled = False
            ChkInclChit.Enabled = True
            ChkInclChit.Checked = False
        Else
            rdbSummary.Enabled = True
        End If
    End Sub

    Private Sub rdbCreditCard_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbCreditCard.CheckedChanged
        If rdbCreditCard.Checked Then
            ChkInclChit.Enabled = True
        End If
    End Sub

    Private Sub rdbCheque_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbCheque.CheckedChanged
        If rdbCheque.Checked Then
            ChkInclChit.Enabled = True
        End If
    End Sub

    Private Sub rdbGiftVoucher_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbGiftVoucher.CheckedChanged
        If rdbGiftVoucher.Checked Then
            ChkInclChit.Enabled = False
            ChkInclChit.Checked = False
        End If
    End Sub

    Private Sub rdbChitCard_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdbChitCard.CheckedChanged
        If rdbChitCard.Checked Then
            ChkInclChit.Enabled = False
            ChkInclChit.Checked = False
        End If
    End Sub

    Private Sub chkLstNodeId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkLstNodeId.KeyDown
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then
            rbtAll.Checked = True
            rbtAll.Focus()
        End If
    End Sub

    Private Sub rdbSummary_CheckedChanged(sender As Object, e As EventArgs) Handles rdbSummary.CheckedChanged

    End Sub
End Class


Public Class frmCardCollectionsReport_New_Properties
    Private chkCompanySelectAll As Boolean = False
    Public Property p_chkCompanySelectAll() As Boolean
        Get
            Return chkCompanySelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCompanySelectAll = value
        End Set
    End Property
    Private chkLstCompany As New List(Of String)
    Public Property p_chkLstCompany() As List(Of String)
        Get
            Return chkLstCompany
        End Get
        Set(ByVal value As List(Of String))
            chkLstCompany = value
        End Set
    End Property
    Private chkLstCostCentre As New List(Of String)
    Public Property p_chkLstCostCentre() As List(Of String)
        Get
            Return chkLstCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkLstCostCentre = value
        End Set
    End Property
    Private chkLstNodeId As New List(Of String)
    Public Property p_chkLstNodeId() As List(Of String)
        Get
            Return chkLstNodeId
        End Get
        Set(ByVal value As List(Of String))
            chkLstNodeId = value
        End Set
    End Property
    Private chkChitCard As Boolean = True
    Public Property p_chkChitCard() As Boolean
        Get
            Return chkChitCard
        End Get
        Set(ByVal value As Boolean)
            chkChitCard = value
        End Set
    End Property
    Private chkInclChit As Boolean = False
    Public Property p_chkInclChit() As Boolean
        Get
            Return chkInclChit
        End Get
        Set(ByVal value As Boolean)
            chkInclChit = value
        End Set
    End Property
    Private chkCreditCard As Boolean = True
    Public Property p_chkCreditCard() As Boolean
        Get
            Return chkCreditCard
        End Get
        Set(ByVal value As Boolean)
            chkCreditCard = value
        End Set
    End Property

    Private chkCheque As Boolean = True
    Public Property p_chkCheque() As Boolean
        Get
            Return chkCheque
        End Get
        Set(ByVal value As Boolean)
            chkCheque = value
        End Set
    End Property
End Class