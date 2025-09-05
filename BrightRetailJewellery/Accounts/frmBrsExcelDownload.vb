Imports System.Data.OleDb

Public Class frmBrsExcelDownload
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dtpGridFlag As Boolean

    Dim preTranDb As String
    Dim curTranDb As String
    Dim objSearch As Object
    Dim dtCostCentre As New DataTable
    Dim EditMode As Boolean = False
    Dim commonTranDb As New ArrayList()
    Dim dtGrid As New DataTable()
    Dim Dt As New DataTable
    Dim dtView, dttemp As New DataTable

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        Dim tDate As Date = cnTranToDate  'Convert.ToDateTime(GetServerDate())
        If tDate.Month > 3 Then
            preTranDb = cnCompanyId + "T" + Mid(tDate.Year - 1, 3, 2) + Mid(tDate.Year, 3, 2)
        Else
            preTranDb = cnCompanyId + "T" + Mid(tDate.Year - 2, 3, 2) + Mid(tDate.Year - 1, 3, 2)
        End If
        strSql = " Select DBNAME from " & cnAdminDb & "..DBMASTER "
        Dim dtName As New DataTable()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtName)
        If dtName.Rows.Count > 0 Then
            For i As Integer = 0 To dtName.Rows.Count - 1
                commonTranDb.Add(dtName.Rows(i).Item("DBNAME"))
            Next
        End If
        If Not objGPack.GetSqlValue("SELECT NAME FROM SYSDATABASES WHERE NAME = '" & preTranDb & "'").Length > 0 Then
            preTranDb = Nothing
        End If
    End Sub

    Private Sub frmBrsExcelDownload_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmBrsExcelDownload_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadAcname()
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")
        lblStatus.Text = ""
    End Sub

    Private Sub LoadAcname()
        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
        strSql += " WHERE ACGRPCODE = '2' "
        strSql += GetAcNameQryFilteration()
        strSql += vbCrLf + "  ORDER BY ACNAME"
        objGPack.FillCombo(strSql, cmbBank_MAN)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        cmbBank_MAN.Text = ""
        txtPath.Clear()
        gridView_OWN.DataSource = Nothing
        lblBankName.Text = ""
        lblStatus.Text = ""
        LoadAcname()
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")
        cmbBank_MAN.Select()
    End Sub

    Private Sub frmBankReconciliation_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If objSearch IsNot Nothing Then
            objSearch = Nothing
        End If
    End Sub
    Private Sub CreateTable()
        strSql = "CREATE TABLE TEMPTABLEDB..ACRECONS("
        strSql += vbCrLf + " [TTRANDATE] [varchar](30) NULL,"
        strSql += vbCrLf + " [PARTICULAR] [varchar](55) NULL,"
        strSql += vbCrLf + " [CHQCARDNO] [varchar](30) NULL,"
        strSql += vbCrLf + " [CHQDATE] [smalldatetime] NULL,"
        strSql += vbCrLf + " [DEBIT] [numeric](15, 2) NULL,"
        strSql += vbCrLf + " [CREDIT] [numeric](15, 2) NULL,"
        strSql += vbCrLf + " [RELIASEDATE] [smalldatetime] NULL,"
        strSql += vbCrLf + " [SNO] [varchar](15) NOT NULL,"
        strSql += vbCrLf + " [BATCHNO] [varchar](12) NULL,"
        strSql += vbCrLf + " [DB] [varchar](8) NOT NULL,"
        strSql += vbCrLf + " [TRANDATE] [smalldatetime] NULL,"
        strSql += vbCrLf + " [CHQCARDREF] [varchar](50) NULL,"
        strSql += vbCrLf + " [AMOUNT] [numeric](15, 2) NULL,"
        strSql += vbCrLf + " [ACCODE] [varchar](7) NULL,"
        strSql += vbCrLf + " [BRSSNO] [varchar](15) NULL,"
        strSql += vbCrLf + " [CARDID] [int] NULL,"
        strSql += vbCrLf + " [COSTID] [varchar](2) NULL,"
        strSql += vbCrLf + " [REMARK1] [varchar](100) NULL,"
        strSql += vbCrLf + " [REMARK2] [varchar](100) NULL,"
        strSql += vbCrLf + " [FROMFLAG] [varchar](2) NULL,"
        strSql += vbCrLf + " [CONTRA] [varchar](7) NULL,"
        strSql += vbCrLf + " [COLHEAD] [varchar](1) NULL,"
        strSql += vbCrLf + " [BRSACC] [varchar](1) NOT NULL,"
        strSql += vbCrLf + " [REFNAME] [varchar](50) NULL,"
        strSql += vbCrLf + " [REMARKS1] [varchar](50) NULL,"
        strSql += vbCrLf + " [REMARKS2] [varchar](50) NULL"
        strSql += vbCrLf + " ) ON [PRIMARY]"
        cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()
    End Sub
    Private Sub InsertIntoTemptable()
        strSql = "INSERT INTO [TEMPTABLEDB].[dbo].[ACRECONS]"
        strSql += vbCrLf + " ([TTRANDATE]"
        strSql += vbCrLf + " ,[PARTICULAR]"
        strSql += vbCrLf + " ,[CHQCARDNO]"
        strSql += vbCrLf + " ,[CHQDATE]"
        strSql += vbCrLf + " ,[DEBIT]"
        strSql += vbCrLf + " ,[CREDIT]"
        strSql += vbCrLf + " ,[RELIASEDATE]"
        strSql += vbCrLf + " ,[SNO]"
        strSql += vbCrLf + " ,[BATCHNO]"
        strSql += vbCrLf + " ,[DB]"
        strSql += vbCrLf + " ,[TRANDATE]"
        strSql += vbCrLf + " ,[CHQCARDREF]"
        strSql += vbCrLf + " ,[AMOUNT]"
        strSql += vbCrLf + " ,[ACCODE]"
        strSql += vbCrLf + " ,[BRSSNO]"
        strSql += vbCrLf + " ,[CARDID]"
        strSql += vbCrLf + " ,[COSTID]"
        strSql += vbCrLf + " ,[REMARK1]"
        strSql += vbCrLf + " ,[REMARK2]"
        strSql += vbCrLf + " ,[FROMFLAG]"
        strSql += vbCrLf + " ,[CONTRA]"
        strSql += vbCrLf + " ,[COLHEAD]"
        strSql += vbCrLf + " ,[BRSACC]"
        strSql += vbCrLf + " ,[REFNAME]"
        strSql += vbCrLf + " ,[REMARKS1]"
        strSql += vbCrLf + " ,[REMARKS2])"
    End Sub

    Public Function GetChequeNo(ByVal ChequeNo As String) As String
        If ChequeNo.ToString.Length >= 6 Then Return ChequeNo
        Dim RetStr As String = ""
        For cnt As Integer = 1 To 6 - ChequeNo.ToString.Length
            RetStr += "0"
        Next
        RetStr += ChequeNo
        Return RetStr
    End Function

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        Dim dv As New DataView
        Dim Sno As String = Nothing
        Dim CardRefNo As String = Nothing
        strSql = vbCrLf + " SELECT DISTINCT CHQCARDREF FROM " & cnStockDb & "..ACCTRAN WHERE ACCODE="
        strSql += vbCrLf + " (SELECT ACCODE FROM " & cnAdminDb & " ..ACHEAD WHERE ACNAME='" & cmbBank_MAN.Text & "' )"
        strSql += vbCrLf + " AND TRANDATE <='" & dtpTranDate.Value.Date & "' AND CHQCARDREF<>'' "
        dttemp = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dttemp)
        For j As Integer = 0 To dttemp.Rows.Count - 1
            CardRefNo += "'" & dttemp.Rows(j).Item("CHQCARDREF") & "',"
        Next
        If CardRefNo Is Nothing Then MsgBox("Record Not Found", MsgBoxStyle.Information) : Exit Sub
        CardRefNo = Mid(CardRefNo, 1, Len(CardRefNo) - 1)
        If txtPath.Text Is Nothing Or txtPath.Text = "" Then MsgBox("Select Excel File Path", MsgBoxStyle.Information) : txtPath.Focus() : Exit Sub
        getExcelData()
        If Dt.Columns.Count <> 5 Then MsgBox("Excel Template Not in Format", MsgBoxStyle.Information) : btnNew_Click(Me, New EventArgs) : Exit Sub
        dv = Dt.DefaultView
        Try
            dv.RowFilter = "BANKNAME IN (" & CardRefNo & ")"
        Catch ex As Exception
            MsgBox("Excel Template Not in Format", MsgBoxStyle.Information) : btnNew_Click(Me, New EventArgs) : Exit Sub
        End Try
        Dt = dv.ToTable
        dtView = New DataTable
        strSql = "IF OBJECT_ID('TEMPTABLEDB..ACRECONS') IS NOT NULL DROP TABLE TEMPTABLEDB..ACRECONS"
        cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()
        CreateTable()
        For i As Integer = 0 To Dt.Rows.Count - 1
            With Dt.Rows(i)
                Dim ValueDate As Date = .Item(0).ToString.Trim
                strSql = "SELECT SNO FROM " & cnStockDb & "..ACCTRAN WHERE CHQCARDNO='" & GetChequeNo(.Item(2).ToString.Trim) & "'"
                strSql += vbCrLf + " AND CHQCARDREF='" & .Item(1).ToString.Trim & "' AND TRANDATE<='" & dtpTranDate.Value.Date & "'"
                If .Item(3).ToString.Trim = "" Then
                    If .Item(4).ToString.Trim = "" Then MsgBox("Credit Column is Empty in Excel", MsgBoxStyle.Information) : Exit Sub
                    strSql += vbCrLf + " AND TRANMODE='C' AND AMOUNT='" & .Item(4).ToString & "'"
                Else
                    If .Item(3).ToString.Trim = "" Then MsgBox("Debit Column is Empty in Excel", MsgBoxStyle.Information) : Exit Sub
                    strSql += vbCrLf + " AND TRANMODE='D' AND AMOUNT='" & .Item(3).ToString & "'"
                End If
                Sno = GetSqlValue(cn, strSql)
                If Sno Is Nothing Then Continue For
                InsertIntoTemptable()
                strSql += vbCrLf + "  SELECT CONVERT(VARCHAR,TRANDATE,103)TTRANDATE"
                strSql += vbCrLf + "  ,CASE WHEN ISNULL(FROMFLAG,'') IN ('A','S')  THEN (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)" 'CHQCARDREF"
                strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'C' THEN 'SCHEME'" 'CHQCARDREF"
                strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'C' THEN 'PURCHASE'"
                strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'D' THEN 'SALES'"
                strSql += vbCrLf + "   ELSE '' END AS PARTICULAR"
                strSql += vbCrLf + "  ,CHQCARDNO,CHQDATE"
                strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE NULL END AS DEBIT"
                strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE NULL END AS CREDIT,'" & ValueDate & "'AS RELIASEDATE,SNO,BATCHNO,'" & cnStockDb & "'DB,TRANDATE,CHQCARDREF,AMOUNT,ACCODE"
                strSql += vbCrLf + "  ,CONVERT(VARCHAR(15),ISNULL(("
                strSql += vbCrLf + "                             SELECT TOP 1 SNO FROM " & cnStockDb & "..BRSINFO "
                strSql += vbCrLf + "                             WHERE "
                strSql += vbCrLf + "                             TRANDATE = T.TRANDATE AND BATCHNO = T.BATCHNO "
                strSql += vbCrLf + "                             AND ACCODE = T.ACCODE"
                strSql += vbCrLf + "                             AND ISNULL(CHQCARDNO,'') = ISNULL(T.CHQCARDNO,'')"
                strSql += vbCrLf + "                             AND ISNULL(CARDID,'') = ISNULL(T.CARDID,'')"
                strSql += vbCrLf + "                             AND ISNULL(CHQCARDREF,'') = ISNULL(T.CHQCARDREF,'')"
                strSql += vbCrLf + "                             AND ISNULL(CHQDATE,'') = ISNULL(T.CHQDATE,'')"
                strSql += vbCrLf + "                             AND ISNULL(COSTID,'') = ISNULL(T.COSTID,'')"
                strSql += vbCrLf + "                             AND COMPANYID = T.COMPANYID"
                strSql += vbCrLf + "                             AND AMOUNT = T.AMOUNT"
                strSql += vbCrLf + "                             ),''))BRSSNO,CARDID,COSTID,CONVERT(VARCHAR(100),REMARK1)REMARK1,CONVERT(VARCHAR(100),REMARK2)REMARK2,CONVERT(VARCHAR(2),FROMFLAG)FROMFLAG,CONVERT(VARCHAR(7),CONTRA)CONTRA,CONVERT(VARCHAR(1),'') COLHEAD,'A' BRSACC"
                strSql += vbCrLf + " , CHQCARDREF AS REFNAME,REMARK1 AS REMARKS1,REMARK2 AS REMARKS2"
                strSql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN AS T WHERE "
                strSql += vbCrLf + "  TRANDATE <= '" & dtpTranDate.Value.Date & "' "
                strSql += vbCrLf + "  AND (RELIASEDATE IS NULL OR RELIASEDATE > '" & dtpTranDate.Value.Date.ToString("yyyy-MM-dd") & "')"
                strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
                If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                    strSql += " AND COSTID IN"
                    strSql += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
                End If
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                strSql += vbCrLf + " AND SNO = '" & Sno & " '"
                strSql += vbCrLf + " AND ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + "  SELECT CONVERT(VARCHAR,TRANDATE,103)TTRANDATE"
                strSql += vbCrLf + "  ,CASE WHEN ISNULL(FROMFLAG,'') IN ('A','S')  THEN (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)" 'CHQCARDREF"
                strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'C' THEN 'SCHEME'" 'CHQCARDREF"
                strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'C' THEN 'PURCHASE'"
                strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'D' THEN 'SALES'"
                strSql += vbCrLf + "   ELSE '' END AS PARTICULAR"
                strSql += vbCrLf + "  ,CHQCARDNO,CHQDATE"
                strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE NULL END AS DEBIT"
                strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE NULL END AS CREDIT,'" & ValueDate & "'AS RELIASEDATE,SNO,BATCHNO,'" & cnStockDb & "'DB,TRANDATE,CHQCARDREF,AMOUNT,ACCODE"
                strSql += vbCrLf + "  ,CONVERT(VARCHAR(15),ISNULL(("
                strSql += vbCrLf + "                             SELECT TOP 1 SNO FROM " & cnStockDb & "..BRSINFO "
                strSql += vbCrLf + "                             WHERE "
                strSql += vbCrLf + "                             TRANDATE = T.TRANDATE AND BATCHNO = T.BATCHNO "
                strSql += vbCrLf + "                             AND ACCODE = T.ACCODE"
                strSql += vbCrLf + "                             AND ISNULL(CHQCARDNO,'') = ISNULL(T.CHQCARDNO,'')"
                strSql += vbCrLf + "                             AND ISNULL(CARDID,'') = ISNULL(T.CARDID,'')"
                strSql += vbCrLf + "                             AND ISNULL(CHQCARDREF,'') = ISNULL(T.CHQCARDREF,'')"
                strSql += vbCrLf + "                             AND ISNULL(CHQDATE,'') = ISNULL(T.CHQDATE,'')"
                strSql += vbCrLf + "                             AND ISNULL(COSTID,'') = ISNULL(T.COSTID,'')"
                strSql += vbCrLf + "                             AND COMPANYID = T.COMPANYID"
                strSql += vbCrLf + "                             AND AMOUNT = T.AMOUNT"
                strSql += vbCrLf + "                             ),''))BRSSNO,CARDID,COSTID,CONVERT(VARCHAR(100),REMARK1)REMARK1,CONVERT(VARCHAR(100),REMARK2)REMARK2,CONVERT(VARCHAR(2),FROMFLAG)FROMFLAG,CONVERT(VARCHAR(7),CONTRA)CONTRA,CONVERT(VARCHAR(1),'') COLHEAD,'B' BRSACC"
                strSql += vbCrLf + " , CHQCARDREF as REFNAME,REMARK1 AS REMARKS1,REMARK2 AS REMARKS2"
                strSql += vbCrLf + "  FROM " & cnStockDb & "..BRS_ACCTRAN AS T WHERE "

                strSql += vbCrLf + "  TRANDATE <= '" & dtpTranDate.Value.Date & "' "
                strSql += vbCrLf + "  AND (RELIASEDATE IS NULL OR RELIASEDATE > '" & dtpTranDate.Value.Date.ToString("yyyy-MM-dd") & "')"
                strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
                If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                    strSql += " AND COSTID IN"
                    strSql += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
                End If
                strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
                strSql += vbCrLf + " AND SNO = '" & Sno & " '"
                strSql += vbCrLf + " AND ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
                strSql += vbCrLf + " ORDER BY TRANDATE"
                cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()
            End With
        Next
        strSql = " UPDATE TEMPTABLEDB..ACRECONS SET PARTICULAR = CASE WHEN SUBSTRING(ISNULL(PAYMODE,'  '),2,1) = 'R' THEN 'RECEIPT' WHEN SUBSTRING(ISNULL(PAYMODE,'  '),2,1) = 'P' THEN 'PAYMENT' ELSE PARTICULAR END"
        strSql += vbCrLf + " FROM TEMPTABLEDB..ACRECONS AS T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..OUTSTANDING AS O ON O.BATCHNO = T.BATCHNO"
        strSql += vbCrLf + " WHERE T.FROMFLAG = 'P' AND T.DB = '" & cnStockDb & "'"
        cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMPTABLEDB..ACRECONS SET PARTICULAR = 'SCHEME'"
        strSql += vbCrLf + " WHERE FROMFLAG = 'C'"
        cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()
        strSql = " SELECT * FROM TEMPTABLEDB..ACRECONS ORDER BY TRANDATE"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtView)
        gridView_OWN.DataSource = dtView
        dttemp = New DataTable
        dttemp = dtView.Copy
        ''ADDING TOTAL
        Dim DebitAmt As Decimal = Nothing
        Dim CreditAmt As Decimal = Nothing
        Dim Row As DataRow = Nothing
        Row = dtView.NewRow
        DebitAmt = Val(dtView.Compute("SUM(DEBIT)", "COLHEAD = '' OR COLHEAD IS NULL").ToString)
        CreditAmt = Val(dtView.Compute("SUM(CREDIT)", "COLHEAD = '' OR COLHEAD IS NULL").ToString)
        Row.Item("PARTICULAR") = "TOTAL ->"
        Row.Item("DEBIT") = IIf(DebitAmt <> 0, Format(DebitAmt, "0.00"), DBNull.Value)
        Row.Item("CREDIT") = IIf(CreditAmt <> 0, Format(CreditAmt, "0.00"), DBNull.Value)
        Row.Item("COLHEAD") = ""
        dtView.Rows.Add(Row)
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).DefaultCellStyle = GlobalVariables.reportTotalStyle

        ''Accounts Not Reflected In Bank
        Row = dtView.NewRow
        Row.Item("PARTICULAR") = "Accounts Not Reflected In Account"
        Row.Item("DEBIT") = IIf(CreditAmt - DebitAmt > 0, Format(CreditAmt - DebitAmt, "0.00"), DBNull.Value)
        Row.Item("CREDIT") = IIf(DebitAmt - CreditAmt > 0, Format(DebitAmt - CreditAmt, "0.00"), DBNull.Value)
        Row.Item("COLHEAD") = "G"
        dtView.Rows.Add(Row)
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).DefaultCellStyle = GlobalVariables.reportTotalStyle
        ''AS PER THE COMPANY BOOK
        strSql = " SELECT SUM(AMOUNT) AS AMOUNT FROM "
        strSql += vbCrLf + " ("
        strSql += vbCrLf + "  SELECT DEBIT-CREDIT AS AMOUNT FROM " & cnStockDb & "..OPENTRAILBALANCE"
        strSql += vbCrLf + "  WHERE ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
        strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END AS AMOUNT"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN"
        strSql += vbCrLf + "  WHERE TRANDATE <= '" & dtpTranDate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + "  AND ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
        strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " )X"
        Dim Bal As Decimal = Nothing
        Bal = Val(objGPack.GetSqlValue(strSql))
        Row = dtView.NewRow
        Row.Item("PARTICULAR") = "Balance As Per the Company"
        Row.Item("DEBIT") = IIf(Bal > 0, Format(Bal, "0.00"), DBNull.Value)
        Row.Item("CREDIT") = IIf(Bal < 0, Format(Bal, "0.00"), DBNull.Value)
        Row.Item("COLHEAD") = "G"
        dtView.Rows.Add(Row)
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).DefaultCellStyle = GlobalVariables.reportTotalStyle
        'Balance as per Bank

        strSql = " SELECT SUM(AMOUNT) AS AMOUNT FROM "
        strSql += vbCrLf + " ("
        strSql += vbCrLf + "  SELECT CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END AS AMOUNT"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN"
        strSql += vbCrLf + "  WHERE RELIASEDATE <= '" & dtpTranDate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + "  AND ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
        strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + "  SELECT CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END AS AMOUNT"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..BRS_ACCTRAN"
        strSql += vbCrLf + "  WHERE RELIASEDATE <= '" & dtpTranDate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + "  AND ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
        strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " )X"
        Bal = Val(objGPack.GetSqlValue(strSql))
        Row = dtView.NewRow
        Row.Item("PARTICULAR") = "Balance As Per Account"
        Row.Item("DEBIT") = IIf(Bal > 0, Format(Bal, "0.00"), DBNull.Value)
        Row.Item("CREDIT") = IIf(Bal < 0, Format(Bal, "0.00"), DBNull.Value)
        Row.Item("COLHEAD") = "G"
        dtView.Rows.Add(Row)
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).DefaultCellStyle = GlobalVariables.reportTotalStyle
        With gridView_OWN
            For cnt As Integer = 0 To gridView_OWN.Columns.Count - 1
                .Columns(cnt).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            .Columns("TTRANDATE").Width = 80
            .Columns("TTRANDATE").HeaderText = "TRANDATE"
            .Columns("PARTICULAR").Width = 300
            .Columns("CHQCARDNO").Width = 150
            .Columns("CHQCARDNO").HeaderText = "CHQ NO"
            .Columns("CHQDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("CHQDATE").Width = 80
            .Columns("DEBIT").Width = 100
            .Columns("DEBIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CREDIT").Width = 100
            .Columns("CREDIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RELIASEDATE").Width = 120
            .Columns("RELIASEDATE").HeaderText = "REALIZE DATE"
            .Columns("RELIASEDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("COSTID").Width = 60
            .Columns("REMARK1").Visible = False
            .Columns("REMARK2").Visible = False
            .Columns("CHQCARDREF").Visible = False
            .Columns("SNO").Visible = False
            .Columns("BATCHNO").Visible = False
            .Columns("TRANDATE").Visible = False
            .Columns("AMOUNT").Visible = False
            .Columns("DB").Visible = False
            .Columns("ACCODE").Visible = False
            .Columns("CARDID").Visible = False
            .Columns("BRSSNO").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("FROMFLAG").Visible = False
            .Columns("CONTRA").Visible = False
            .Columns("BRSACC").Visible = False
        End With
        objSearch = New frmGridSearch(gridView_OWN)
        gridView_OWN.Focus()
    End Sub
    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView_OWN.GotFocus
        lblStatus.Visible = True
        lblStatus.Text = "Press Enter to Set Realize Date"
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView_OWN.KeyDown
        If Not gridView_OWN.RowCount > 0 Then Exit Sub
        If e.KeyCode = Keys.E Then
            e.Handled = True
            If gridView_OWN.CurrentRow.Cells("TRANDATE").Value.ToString = "" Then Exit Sub
            gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("RELIASEDATE")
        ElseIf e.KeyCode = Keys.Enter Then
            UpdateRealise()
        End If
    End Sub

    Private Sub UpdateRealise()
        Try
            Dim BrsSno As String = ""
            If dttemp.Rows.Count = 0 Then Exit Sub
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
            tran = Nothing
            tran = cn.BeginTransaction
            For i As Integer = 0 To dttemp.Rows.Count - 1
                With dttemp.Rows(i)
                    If .Item("BRSSNO").ToString = "" Then
                        BrsSno = GetNewSno  (TranSnoType.BRSINFOCODE, tran, , strCompanyId, .Item("DB").ToString)
                        Dim DtBrsInfo As New DataTable
                        Dim Row As DataRow = Nothing
                        DtBrsInfo = BrighttechPack.GlobalMethods.GetTableStructure(.Item("DB").ToString, "BRSINFO", cn, tran)
                        Row = DtBrsInfo.NewRow
                        Row.Item("SNO") = BrsSno
                        Row.Item("TRANDATE") = .Item("TRANDATE").ToString
                        Row.Item("AMOUNT") = Val(.Item("AMOUNT").ToString)
                        Row.Item("ACCODE") = .Item("ACCODE").ToString
                        Row.Item("BATCHNO") = .Item("BATCHNO").ToString
                        Row.Item("CHQCARDNO") = .Item("CHQCARDNO").ToString
                        Row.Item("CARDID") = .Item("CARDID").ToString
                        Row.Item("CHQCARDREF") = .Item("CHQCARDREF").ToString
                        Row.Item("CHQDATE") = .Item("CHQDATE").ToString
                        Row.Item("REALIZEDATE") = .Item("RELIASEDATE").ToString
                        Row.Item("COSTID") = .Item("COSTID").ToString
                        Row.Item("COMPANYID") = strCompanyId
                        DtBrsInfo.Rows.Add(Row)
                        InsertData(SyncMode.Transaction, DtBrsInfo, cn, tran, , , , , , False)
                    ElseIf .Item("BRSSNO").Value.ToString <> "" Then
                        BrsSno = .Item("BRSSNO").ToString
                        strSql = " UPDATE " & .Item("DB").Value & "..BRSINFO SET REALIZEDATE = '" & .Item("RELIASEDATE").ToString & "'"
                        strSql += " WHERE SNO = '" & BrsSno & "'"
                        cmd = New OleDbCommand(strSql, cn, tran)
                        cmd.ExecuteNonQuery()
                    End If
                    If .Item("BRSACC").ToString = "B" Then
                        strSql = " UPDATE " & cnStockDb & "..BRS_ACCTRAN SET RELIASEDATE = '" & .Item("RELIASEDATE").ToString & "'"
                        strSql += vbCrLf + "  WHERE TRANDATE = '" & .Item("TRANDATE").ToString & "'"
                        strSql += vbCrLf + "  AND BATCHNO = '" & .Item("BATCHNO").ToString & "'"
                        strSql += vbCrLf + "  AND SNO = '" & .Item("SNO").ToString & "'"
                        strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
                    Else
                        strSql = " UPDATE " & cnStockDb & "..ACCTRAN SET RELIASEDATE = '" & .Item("RELIASEDATE").ToString & "'"
                        strSql += vbCrLf + "  WHERE TRANDATE = '" & .Item("TRANDATE").ToString & "'"
                        strSql += vbCrLf + "  AND BATCHNO = '" & .Item("BATCHNO").ToString & "'"
                        strSql += vbCrLf + "  AND SNO = '" & .Item("SNO").ToString & "'"
                        strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
                    End If
                    cmd = New OleDbCommand(strSql, cn, tran)
                    cmd.ExecuteNonQuery()
                End With
            Next
            tran.Commit()
            tran = Nothing
            MsgBox("Updated Successfully", MsgBoxStyle.Information)
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub


    'Private Sub UpdateUnRealise()
    '    Try
    '        tran = Nothing
    '        tran = cn.BeginTransaction
    '        strSql = " DELETE FROM " & gridView_OWN.CurrentRow.Cells("DB").Value & "..BRSINFO"
    '        strSql += vbCrLf + " WHERE SNO = '" & gridView_OWN.CurrentRow.Cells("BRSSNO").Value.ToString & "'"
    '        If gridView_OWN.CurrentRow.Cells("BRSACC").Value.ToString = "B" Then
    '            strSql += vbCrLf + " UPDATE " & gridView_OWN.CurrentRow.Cells("DB").Value & "..BRS_ACCTRAN SET RELIASEDATE = NULL"
    '            strSql += vbCrLf + "  WHERE TRANDATE = '" & gridView_OWN.CurrentRow.Cells("TRANDATE").Value & "'"
    '            strSql += vbCrLf + "  AND BATCHNO = '" & gridView_OWN.CurrentRow.Cells("BATCHNO").Value & "'"
    '            strSql += vbCrLf + "  AND SNO = '" & gridView_OWN.CurrentRow.Cells("SNO").Value & "'"
    '            strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
    '        Else
    '            strSql += vbCrLf + " UPDATE " & gridView_OWN.CurrentRow.Cells("DB").Value & "..ACCTRAN SET RELIASEDATE = NULL"
    '            strSql += vbCrLf + "  WHERE TRANDATE = '" & gridView_OWN.CurrentRow.Cells("TRANDATE").Value & "'"
    '            strSql += vbCrLf + "  AND BATCHNO = '" & gridView_OWN.CurrentRow.Cells("BATCHNO").Value & "'"
    '            strSql += vbCrLf + "  AND SNO = '" & gridView_OWN.CurrentRow.Cells("SNO").Value & "'"
    '            strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
    '        End If
    '        cmd = New OleDbCommand(strSql, cn, tran)
    '        cmd.ExecuteNonQuery()
    '        gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("RELIASEDATE").Value = DBNull.Value
    '        gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("BRSSNO").Value = DBNull.Value
    '        tran.Commit()
    '        tran = Nothing
    '    Catch ex As Exception
    '        If tran IsNot Nothing Then tran.Rollback()
    '        MsgBox(ex.Message + vbCrLf + ex.StackTrace)
    '    End Try
    'End Sub

    Private Sub gridView_OWN_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView_OWN.RowEnter
        
    End Sub

    Private Sub dtpGridRealise_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        lblStatus.Visible = True
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnPath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPath.Click
        Dim OpenDialog As New OpenFileDialog
        Dim Str As String
        Str = "(*.xls) | *.xls"
        Str += "|(*.xlsx) | *.xlsx"
        OpenDialog.Filter = Str
        If OpenDialog.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtPath.Text = OpenDialog.FileName
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub getExcelData()
        Dim MyConnection As System.Data.OleDb.OleDbConnection
        MyConnection = New OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" & txtPath.Text & "';Extended Properties=Excel 8.0;")
        strSql = "SELECT * FROM [SHEET1$]"
        da = New OleDbDataAdapter(strSql, MyConnection)
        Dt = New DataTable
        da.Fill(Dt)
        MyConnection.Close()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView_OWN.Rows.Count > 0 Then
            Dim Title As String = cmbBank_MAN.Text + vbCrLf
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "", gridView_OWN, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView_OWN.Rows.Count > 0 Then
            Dim Title As String = cmbBank_MAN.Text + vbCrLf
            BrightPosting.GExport.Post(Me.Name, strCompanyName, "", gridView_OWN, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub txtPath_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPath.GotFocus
        lblStatus.Text = "Press F1 to See Sample Template For Excel"
    End Sub
    Private Sub txtPath_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPath.KeyDown
            If e.KeyCode = Keys.F1 Then
            Dim dtex As New DataTable
            With dtex.Columns
                .Add("ValueDate", GetType(Date))
                .Add("BankName", GetType(String))
                .Add("CheqNo", GetType(String))
                .Add("Debit", GetType(Double))
                .Add("Credit", GetType(Double))
            End With
            gridView_OWN.DataSource = Nothing
            Dim row As DataRow
            row = dtex.NewRow
            dtex.Rows.Add(row)
            row = dtex.NewRow
            dtex.Rows.Add(row)
            gridView_OWN.DataSource = dtex
        End If
    End Sub

    Private Sub txtPath_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPath.LostFocus
        lblStatus.Text = ""
        gridView_OWN.DataSource = Nothing
    End Sub
End Class