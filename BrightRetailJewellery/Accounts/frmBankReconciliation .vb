Imports System.Data.OleDb
Imports System.Globalization

Public Class frmBankReconciliation
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim dtpGridFlag As Boolean

    Dim preTranDb As String
    Dim preTranDb1 As String
    Dim curTranDb As String
    Dim objSearch As Object
    Dim dtCostCentre As New DataTable
    Dim EditMode As Boolean = False
    Dim commonTranDb As New ArrayList()
    Dim dtGrid As New DataTable()
    Dim FindSearchFormat As Integer = IIf(GetAdmindbSoftValue("FINDSEARCH", "O") = "N", 1, -1)
    Dim BRS_PRE_TranDb As Integer = IIf(GetAdmindbSoftValue("BRS_PRE_TRANDB", "O") = "2", 2, 1)
    Dim BRS_REALISED_OPENING As Boolean = IIf(GetAdmindbSoftValue("BRS_REALISED_OPENING", "N") = "Y", True, False)
    Dim SelectedCostid As String = ""
    Dim SelectedBankid As String = ""
    Dim ukculinfo As New CultureInfo("en-GB")


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim tDate As Date = cnTranToDate  'Convert.ToDateTime(GetServerDate())
        If tDate.Month > 3 Then
            preTranDb = cnCompanyId + "T" + Mid(tDate.Year - 1, 3, 2) + Mid(tDate.Year, 3, 2)
            If BRS_PRE_TranDb = 2 Then preTranDb1 = cnCompanyId + "T" + Mid(tDate.Year - 2, 3, 2) + Mid(tDate.Year - 1, 3, 2)
        Else
            preTranDb = cnCompanyId + "T" + Mid(tDate.Year - 2, 3, 2) + Mid(tDate.Year - 1, 3, 2)
            If BRS_PRE_TranDb = 2 Then preTranDb1 = cnCompanyId + "T" + Mid(tDate.Year - 3, 3, 2) + Mid(tDate.Year - 2, 3, 2)
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
        If Not objGPack.GetSqlValue("SELECT NAME FROM SYSDATABASES WHERE NAME = '" & preTranDb1 & "'").Length > 0 Then
            preTranDb1 = Nothing
        End If
        If BRS_PRE_TranDb <> 2 Then
            preTranDb1 = Nothing
        End If
    End Sub

    Private Sub LoadAcname()
        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD "
        If rbtBank.Checked Then
            strSql += " WHERE ACGRPCODE = '2' "
        Else
            strSql += " WHERE ACGRPCODE <> '2' "
        End If
        strSql += GetAcNameQryFilteration()
        strSql += vbCrLf + "  ORDER BY ACNAME"
        objGPack.FillCombo(strSql, cmbBank_MAN)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        cmbBank_MAN.Text = ""
        gridView_OWN.DataSource = Nothing
        dtpGridRealise.Visible = False
        rbtBank.Checked = True
        lblStatus.Text = ""
        lblStatus.Visible = False
        lblStatus1.Text = ""
        lblStatus1.Visible = False
        rbtRealised.Checked = False
        rbtUnRealised.Checked = True
        dtpAsOnDate.Value = GetServerDate()
        dtpTranDateFrom.Value = GetServerDate()
        dtpTranDateTo.Value = GetServerDate()
        dtpRealiseDateFrom.Value = GetServerDate()
        dtpRealiseDateTo.Value = GetServerDate()
        dtpUnrealizeDateTo.Value = GetServerDate()
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")
        lblBankName.Text = ""
        SelectedCostid = ""
        SelectedBankid = ""

        LoadAcname()
        cmbBank_MAN.Select()
    End Sub

    Private Sub rbtRealised_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtRealised.CheckedChanged
        pnlUnRealised.Visible = Not rbtRealised.Checked
        chkrupdate.Enabled = False
        dtpGridRealiseBulk.Visible = False
    End Sub

    Private Sub rbtUnRealised_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtUnRealised.CheckedChanged
        pnlRealised.Visible = Not rbtUnRealised.Checked
        chkBasedOnTrandate.Visible = Not rbtUnRealised.Checked
        chkBasedOnTrandate.Checked = Not rbtUnRealised.Checked
        chkBasedOnTrandate.Enabled = BRS_REALISED_OPENING
        chkrupdate.Enabled = rbtUnRealised.Checked
    End Sub

    Private Sub frmBankReconciliation_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If objSearch IsNot Nothing Then
            objSearch = Nothing
        End If
    End Sub

    Private Sub frmBankReconciliation_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If gridView_OWN.Focused Then Exit Sub
            If dtpGridRealise.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmBankReconciliation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gridView_OWN.SelectionMode = DataGridViewSelectionMode.CellSelect
        gridView_OWN.RowTemplate.Height = 21
        dtpAsOnDate.MinimumDate = (New DateTimePicker).MinDate
        dtpAsOnDate.MaximumDate = (New DateTimePicker).MaxDate
        dtpGridRealise.MinDate = (New DateTimePicker).MinDate
        dtpGridRealise.MaxDate = (New DateTimePicker).MaxDate
        dtpRealiseDateFrom.MinimumDate = (New DateTimePicker).MinDate
        dtpRealiseDateFrom.MaximumDate = (New DateTimePicker).MaxDate
        dtpRealiseDateTo.MinimumDate = (New DateTimePicker).MinDate
        dtpRealiseDateTo.MaximumDate = (New DateTimePicker).MaxDate
        dtpTranDateFrom.MinimumDate = (New DateTimePicker).MinDate
        dtpTranDateFrom.MaximumDate = (New DateTimePicker).MaxDate
        dtpTranDateTo.MinimumDate = (New DateTimePicker).MinDate
        dtpTranDateTo.MaximumDate = (New DateTimePicker).MaxDate
        dtpUnrealizeDateTo.MinimumDate = (New DateTimePicker).MinDate
        dtpUnrealizeDateTo.MaximumDate = (New DateTimePicker).MaxDate
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , "ALL")
        LblUnrealizeDateTo.Visible = False
        dtpUnrealizeDateTo.Visible = False
        ChkAson.Checked = True
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim dtView As New DataTable
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.View) Then Exit Sub
        gridView_OWN.DataSource = Nothing
        gridView_OWN.Columns.Clear()
        SelectedBankid = objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "'").ToString
        If Not chkCmbCostCentre.Text = "ALL" And chkCmbCostCentre.Enabled = True Then
            SelectedCostid = GetSelectedCostId(chkCmbCostCentre, True)
        End If
        strSql = ""
        strSql += vbCrLf + "  UPDATE " & cnStockDb & "..ACCTRAN SET RELIASEDATE = B.REALIZEDATE,BRSFLAG ='Y'"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN AS T"
        strSql += vbCrLf + "  INNER JOIN " & cnStockDb & "..BRSINFO AS B ON T.TRANDATE = B.TRANDATE " ' AND T.BATCHNO = B.BATCHNO"
        strSql += vbCrLf + "  AND T.ACCODE = B.ACCODE"
        strSql += vbCrLf + "  AND ISNULL(T.CHQCARDNO,'') = ISNULL(B.CHQCARDNO,'')"
        strSql += vbCrLf + "  AND ISNULL(T.CHQCARDREF,'') = ISNULL(B.CHQCARDREF,'')"
        strSql += vbCrLf + "  AND ISNULL(T.COSTID,'') = ISNULL(B.COSTID,'')"
        strSql += vbCrLf + "  AND T.COMPANYID = B.COMPANYID"
        strSql += vbCrLf + "  AND ISNULL(T.AMOUNT,0) = ISNULL(B.AMOUNT,0)"
        strSql += vbCrLf + "  AND ISNULL(T.BATCHNO,'') = ISNULL(B.BATCHNO,'')"
        strSql += vbCrLf + "  AND ISNULL(T.ENTREFNO,'') = ISNULL(B.ENTREFNO,'')"
        strSql += vbCrLf + "  WHERE "
        strSql += vbCrLf + "  T.TRANDATE <= '" & dtpTranDateFrom.Value.Date & "' "
        strSql += vbCrLf + "  AND T.ACCODE = '" & SelectedBankid & "'" '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
        strSql += vbCrLf + "  AND T.COMPANYID = '" & strCompanyId & "'"
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += " and t.COSTID in (" & SelectedCostid & ")"

        cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        strSql = ""
        strSql += vbCrLf + "  UPDATE " & cnStockDb & "..BRS_ACCTRAN SET RELIASEDATE = B.REALIZEDATE,BRSFLAG='Y'"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..BRS_ACCTRAN AS T"
        strSql += vbCrLf + "  INNER JOIN " & cnStockDb & "..BRSINFO AS B ON T.TRANDATE = B.TRANDATE " 'AND T.BATCHNO = B.BATCHNO"
        strSql += vbCrLf + "  AND T.TRANNO = B.TRANNO AND T.ACCODE = B.ACCODE"
        strSql += vbCrLf + "  AND ISNULL(T.CHQCARDNO,'') = ISNULL(B.CHQCARDNO,'')"
        strSql += vbCrLf + "  AND ISNULL(T.CHQCARDREF,'') = ISNULL(B.CHQCARDREF,'')"
        strSql += vbCrLf + "  AND T.COSTID = B.COSTID"
        strSql += vbCrLf + "  AND T.COMPANYID = B.COMPANYID"
        strSql += vbCrLf + "  AND ISNULL(T.AMOUNT,0) = ISNULL(B.AMOUNT,0)"
        strSql += vbCrLf + "  WHERE "
        strSql += vbCrLf + "  T.TRANDATE <= '" & dtpTranDateFrom.Value.Date & "' "
        strSql += vbCrLf + "  AND T.ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
        strSql += vbCrLf + "  AND T.COMPANYID = '" & strCompanyId & "'"
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += " and t.COSTID in (" & SelectedCostid & ")"
        cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        If preTranDb <> Nothing Then
            strSql = ""
            strSql += vbCrLf + "  UPDATE " & preTranDb & "..ACCTRAN SET RELIASEDATE = B.REALIZEDATE,BRSFLAG='Y'"
            strSql += vbCrLf + "  FROM " & preTranDb & "..ACCTRAN AS T"
            strSql += vbCrLf + "  INNER JOIN " & preTranDb & "..BRSINFO AS B ON T.TRANDATE = B.TRANDATE " 'AND T.BATCHNO = B.BATCHNO"
            strSql += vbCrLf + "  AND T.ACCODE = B.ACCODE"
            strSql += vbCrLf + "  AND ISNULL(T.CHQCARDNO,'') = ISNULL(B.CHQCARDNO,'')"
            'strSql += vbCrLf + "  AND T.CARDID = B.CARDID"
            strSql += vbCrLf + "  AND ISNULL(T.CHQCARDREF,'') = ISNULL(B.CHQCARDREF,'')"
            'strSql += vbCrLf + "  AND ISNULL(T.CHQDATE,'') = ISNULL(B.CHQDATE,'')"
            strSql += vbCrLf + "  AND T.COSTID = B.COSTID"
            strSql += vbCrLf + "  AND T.COMPANYID = B.COMPANYID"
            strSql += vbCrLf + "  AND ISNULL(T.AMOUNT,0) = ISNULL(B.AMOUNT,0)"
            strSql += vbCrLf + "  AND ISNULL(T.ENTREFNO,'') = ISNULL(B.ENTREFNO,'')"
            strSql += vbCrLf + "  WHERE "
            strSql += vbCrLf + "  T.TRANDATE <= '" & dtpTranDateFrom.Value.Date & "' "
            strSql += vbCrLf + "  AND T.ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
            strSql += vbCrLf + "  AND T.COMPANYID = '" & strCompanyId & "'"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strSql += " and t.COSTID in (" & SelectedCostid & ")"

            End If
            cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()
        End If

        If rbtUnRealised.Checked Then
            strSql = "IF OBJECT_ID('TEMPDB..ACRECONS') IS NOT NULL DROP TABLE TEMPDB..ACRECONS"
            cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()
        End If

        If preTranDb1 <> Nothing Then
            strSql = ""
            strSql += vbCrLf + "  UPDATE " & preTranDb1 & "..ACCTRAN SET RELIASEDATE = B.REALIZEDATE,BRSFLAG='Y'"
            strSql += vbCrLf + "  FROM " & preTranDb1 & "..ACCTRAN AS T"
            strSql += vbCrLf + "  INNER JOIN " & preTranDb1 & "..BRSINFO AS B ON T.TRANDATE = B.TRANDATE " 'AND T.BATCHNO = B.BATCHNO"
            strSql += vbCrLf + "  AND T.ACCODE = B.ACCODE"
            strSql += vbCrLf + "  AND ISNULL(T.CHQCARDNO,'') = ISNULL(B.CHQCARDNO,'')"
            'strSql += vbCrLf + "  AND T.CARDID = B.CARDID"
            strSql += vbCrLf + "  AND ISNULL(T.CHQCARDREF,'') = ISNULL(B.CHQCARDREF,'')"
            'strSql += vbCrLf + "  AND ISNULL(T.CHQDATE,'') = ISNULL(B.CHQDATE,'')"
            strSql += vbCrLf + "  AND T.COSTID = B.COSTID"
            strSql += vbCrLf + "  AND T.COMPANYID = B.COMPANYID"
            strSql += vbCrLf + "  AND ISNULL(T.AMOUNT,0) = ISNULL(B.AMOUNT,0)"
            strSql += vbCrLf + "  AND ISNULL(T.ENTREFNO,'') = ISNULL(B.ENTREFNO,'')"
            strSql += vbCrLf + "  WHERE "
            strSql += vbCrLf + "  T.TRANDATE <= '" & dtpTranDateFrom.Value.Date & "' "
            strSql += vbCrLf + "  AND T.ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
            strSql += vbCrLf + "  AND T.COMPANYID = '" & strCompanyId & "'"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strSql += " and t.COSTID in (" & SelectedCostid & ")"

            End If
            cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()
        End If

        If rbtUnRealised.Checked Then
            strSql = "IF OBJECT_ID('TEMPDB..ACRECONS') IS NOT NULL DROP TABLE TEMPDB..ACRECONS"
            cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()
        End If

        If rbtRealised.Checked Then
            If commonTranDb.Count > 0 Then
                For i As Integer = 0 To commonTranDb.Count - 1
                    If GetSqlValue(cn, "SELECT TOP 1 1 FROM MASTER..SYSDATABASES WHERE NAME = '" & commonTranDb(i).ToString & "'") <> 1 Then Continue For
                    strSql = "IF OBJECT_ID('TEMPDB..ACRECONS') IS NOT NULL DROP TABLE TEMPDB..ACRECONS"
                    cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

                    strSql += " SELECT CONVERT(VARCHAR(20),NULL)TTRANDATE,CONVERT(VARCHAR(50),'OPENING') AS PARTICULAR,CONVERT(VARCHAR(50),NULL) CHQCARDNO,CONVERT(SMALLDATETIME,NULL) CHQDATE"
                    strSql += vbCrLf + "  ,CASE WHEN DEBIT - CREDIT > 0 THEN DEBIT - CREDIT ELSE NULL END AS DEBIT"
                    strSql += vbCrLf + "  ,CASE WHEN CREDIT - DEBIT > 0 THEN CREDIT - DEBIT ELSE NULL END AS CREDIT"
                    strSql += vbCrLf + "  ,CONVERT(SMALLDATETIME,NULL) RELIASEDATE,CONVERT(VARCHAR(15),NULL) SNO,CONVERT(VARCHAR(10),NULL) BATCHNO ,CONVERT(SMALLDATETIME,NULL) TRANDATE,'" & cnStockDb & "'DB,CONVERT(VARCHAR(50),NULL)CHQCARDREF,CONVERT(NUMERIC(15,2),NULL)AMOUNT,ACCODE"
                    strSql += vbCrLf + "  ,CONVERT(VARCHAR(15),NULL)BRSSNO,CONVERT(INT,NULL)CARDID,CONVERT(VARCHAR(2),NULL)COSTID,CONVERT(VARCHAR(100),NULL)REMARK1,CONVERT(VARCHAR(100),NULL)REMARK2,CONVERT(VARCHAR(2),NULL)FROMFLAG,CONVERT(VARCHAR(7),NULL)CONTRA,COLHEAD,CONVERT(VARCHAR(1),'') BRSACC"
                    strSql += vbCrLf + " ,CONVERT(VARCHAR(100),NULL)REFNAME,CONVERT(VARCHAR(100),NULL)REMARKS1,CONVERT(VARCHAR(100),NULL)REMARKS2,CONVERT(VARCHAR(10),NULL) ENTREFNO"
                    strSql += vbCrLf + "  INTO TEMPDB..ACRECONS"
                    strSql += vbCrLf + "  FROM "
                    strSql += vbCrLf + "  (/* X STARTS */"
                    strSql += vbCrLf + "        SELECT SUM(DEBIT) DEBIT,SUM(CREDIT) CREDIT,ACCODE,COLHEAD FROM (/* Y STARTS */"
                    If BRS_REALISED_OPENING = True And commonTranDb(i).ToString = cnStockDb Then
                        Dim opendate As Date = GetSqlValue(cn, "SELECT STARTDATE FROM " & cnAdminDb & "..DBMASTER WHERE DBNAME='" & commonTranDb(i).ToString & "'")
                        strSql += vbCrLf + "  SELECT SUM(DEBIT) AS DEBIT"
                        strSql += vbCrLf + "  ,SUM(CREDIT) AS CREDIT,ACCODE,'G' COLHEAD,'A' BRSACC"
                        strSql += vbCrLf + "  FROM " & commonTranDb(i).ToString & "..OPENTRAILBALANCE WHERE "
                        strSql += vbCrLf + "  ACCODE = '" & SelectedBankid & "'"
                        strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
                        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += " and COSTID in (" & SelectedCostid & ")"
                        strSql += vbCrLf + "  GROUP BY ACCODE"
                        strSql += vbCrLf + " UNION ALL"
                        If GetSqlValue(cn, "SELECT TOP 1 1 FROM MASTER..SYSDATABASES WHERE NAME = '" & preTranDb & "'") = 1 Then
                            strSql += vbCrLf + "  SELECT SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE 0 END) AS DEBIT"
                            strSql += vbCrLf + "  ,SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE 0 END) AS CREDIT,ACCODE,'G' COLHEAD,'A' BRSACC"
                            strSql += vbCrLf + "  FROM " & preTranDb & "..ACCTRAN WHERE "
                            strSql += vbCrLf + "  (RELIASEDATE IS NULL"
                            If chkBasedOnTrandate.Checked = False Then
                                strSql += vbCrLf + "  OR RELIASEDATE >= '" & Format(dtpRealiseDateFrom.Value.Date, "yyyy-MM-dd").ToString & "') "
                            Else
                                strSql += vbCrLf + "  OR RELIASEDATE >= '" & Format(opendate, "yyyy-MM-dd").ToString & "') "
                            End If
                            strSql += vbCrLf + "  AND ACCODE = '" & SelectedBankid & "'"
                            strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
                            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += " and COSTID in (" & SelectedCostid & ")"
                            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') <> 'Y'"
                            strSql += vbCrLf + "  GROUP BY ACCODE"
                            strSql += vbCrLf + " UNION ALL"
                        End If
                    End If
                    strSql += vbCrLf + "  SELECT SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE 0 END) AS DEBIT"
                    strSql += vbCrLf + "  ,SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE 0 END) AS CREDIT,ACCODE,'G' COLHEAD,'A' BRSACC"
                    strSql += vbCrLf + "  FROM " & commonTranDb(i).ToString & "..ACCTRAN WHERE "
                    'strSql += vbCrLf + "  TRANDATE BETWEEN '" & dtpTranDateFrom.Value.Date & "' AND '" & dtpTranDateTo.Value.Date & "'"
                    If chkBasedOnTrandate.Checked Then
                        strSql += vbCrLf + "  TRANDATE BETWEEN '" & dtpTranDateFrom.Value.Date & "' AND '" & dtpTranDateTo.Value.Date & "'"
                    Else
                        strSql += vbCrLf + " 1=1 "
                    End If
                    If BRS_REALISED_OPENING = True And commonTranDb(i).ToString <> cnStockDb Then
                        strSql += vbCrLf + " AND 1<>1 "
                    End If
                    'strSql += vbCrLf + "  AND RELIASEDATE BETWEEN '" & dtpRealiseDateFrom.Value.Date & "' AND '" & dtpRealiseDateTo.Value.Date & "'"
                    strSql += vbCrLf + "  AND RELIASEDATE < '" & dtpRealiseDateFrom.Value.Date & "' "
                    strSql += vbCrLf + "  AND RELIASEDATE IS NOT NULL"
                    strSql += vbCrLf + "  AND ACCODE = '" & SelectedBankid & "'" '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
                    strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
                    If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += " and COSTID in (" & SelectedCostid & ")"
                    strSql += vbCrLf + "  AND ISNULL(CANCEL,'') <> 'Y'"
                    strSql += vbCrLf + "  GROUP BY ACCODE"
                    strSql += vbCrLf + " UNION ALL"
                    strSql += vbCrLf + "  SELECT SUM(CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE 0 END) AS DEBIT"
                    strSql += vbCrLf + "  ,SUM(CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE 0 END) AS CREDIT,ACCODE,'G' COLHEAD,'B' BRSACC"
                    strSql += vbCrLf + "  FROM " & commonTranDb(i) & "..BRS_ACCTRAN WHERE "
                    'strSql += vbCrLf + "  TRANDATE between '" & dtpTranDateFrom.Value.Date & "' AND '" & dtpTranDateTo.Value.Date & "'"
                    If chkBasedOnTrandate.Checked Then
                        strSql += vbCrLf + "  TRANDATE between '" & dtpTranDateFrom.Value.Date & "' AND '" & dtpTranDateTo.Value.Date & "'"
                    Else
                        strSql += vbCrLf + "  1=1 "
                    End If
                    If BRS_REALISED_OPENING = True And commonTranDb(i).ToString <> cnStockDb Then
                        strSql += vbCrLf + " AND 1<>1 "
                    End If
                    'strSql += vbCrLf + "  AND RELIASEDATE BETWEEN '" & dtpRealiseDateFrom.Value.Date & "' AND '" & dtpRealiseDateTo.Value.Date & "'"
                    strSql += vbCrLf + "  AND RELIASEDATE < '" & dtpRealiseDateFrom.Value.Date & "' "
                    strSql += vbCrLf + "  AND RELIASEDATE IS NOT NULL"
                    strSql += vbCrLf + "  AND ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
                    strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
                    If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += " and COSTID in (" & SelectedCostid & ")"

                    strSql += vbCrLf + "  AND ISNULL(CANCEL,'') <> 'Y'"
                    strSql += vbCrLf + "  GROUP BY ACCODE"
                    strSql += vbCrLf + "   )Y GROUP BY ACCODE,COLHEAD"
                    strSql += vbCrLf + "  )X"
                    strSql += vbCrLf + "  UNION ALL"

                    strSql += vbCrLf + "  SELECT CONVERT(VARCHAR,TRANDATE,103)TTRANDATE"
                    strSql += vbCrLf + "  ,CASE WHEN ISNULL(FROMFLAG,'') IN ('A','S','D')  THEN (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)" 'CHQCARDREF"
                    strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'C' THEN 'SCHEME'" 'CHQCARDREF"
                    strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'C' THEN 'PURCHASE'"
                    strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'D' THEN 'SALES'"
                    'strSql += vbCrLf + "   ELSE (SELECT PAYDESC FROM " & cnAdminDb & "..TRANPAYMODE WHERE PAYMODE = T.PAYMODE) END AS PARTICULAR"
                    strSql += vbCrLf + "   ELSE '' END AS PARTICULAR"
                    strSql += vbCrLf + "  ,CHQCARDNO,CHQDATE"
                    strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE NULL END AS DEBIT"
                    strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE NULL END AS CREDIT,RELIASEDATE,SNO,BATCHNO,TRANDATE,'" & commonTranDb(i) & "'DB,CHQCARDREF,AMOUNT,ACCODE"
                    strSql += vbCrLf + "  ,CONVERT(VARCHAR(15),ISNULL(("
                    strSql += vbCrLf + "                             SELECT TOP 1 SNO FROM " & commonTranDb(i) & "..BRSINFO "
                    strSql += vbCrLf + "                             WHERE "
                    strSql += vbCrLf + "                             TRANDATE = T.TRANDATE AND BATCHNO = T.BATCHNO "
                    strSql += vbCrLf + "                             AND ACCODE = T.ACCODE"
                    strSql += vbCrLf + "                             AND ISNULL(CHQCARDNO,'') = ISNULL(T.CHQCARDNO,'')"
                    strSql += vbCrLf + "                             AND ISNULL(CARDID,'') = ISNULL(T.CARDID,'')"
                    strSql += vbCrLf + "                             AND ISNULL(CHQCARDREF,'') = ISNULL(T.CHQCARDREF,'')"
                    strSql += vbCrLf + "                             AND ISNULL(ENTREFNO,'') = ISNULL(T.ENTREFNO,'')"
                    strSql += vbCrLf + "                             AND ISNULL(COSTID,'') = ISNULL(T.COSTID,'')"
                    strSql += vbCrLf + "                             AND COMPANYID = T.COMPANYID"
                    strSql += vbCrLf + "                             AND ISNULL(AMOUNT,0) = ISNULL(T.AMOUNT,0)"
                    strSql += vbCrLf + "                             ),''))BRSSNO,CARDID,COSTID,CONVERT(VARCHAR(100),REMARK1)REMARK1,CONVERT(VARCHAR(100),REMARK2)REMARK2,CONVERT(VARCHAR(2),FROMFLAG)FROMFLAG,CONVERT(VARCHAR(7),CONTRA)CONTRA,CONVERT(VARCHAR(1),'') COLHEAD,'A' BRSACC"
                    strSql += vbCrLf + " , CHQCARDREF as REFNAME,REMARK1 AS REMARKS1,REMARK2 AS REMARKS2,ENTREFNO"

                    strSql += vbCrLf + "  FROM " & commonTranDb(i) & "..ACCTRAN AS T WHERE "
                    If rbtUnRealised.Checked Then
                        strSql += vbCrLf + "  TRANDATE <= '" & dtpAsOnDate.Value.Date & "' "
                        ''strSql += vbCrLf + "  AND RELIASEDATE IS NULL"
                        strSql += vbCrLf + "  AND (RELIASEDATE IS NULL OR RELIASEDATE > '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "')"
                        strSql += vbCrLf + "  AND ACCODE = '" & SelectedBankid & "'"  ' (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
                    Else
                        'strSql += vbCrLf + "  TRANDATE BETWEEN '" & dtpTranDateFrom.Value.Date & "' AND '" & dtpTranDateTo.Value.Date & "'"
                        If chkBasedOnTrandate.Checked Then
                            strSql += vbCrLf + "  TRANDATE BETWEEN '" & dtpTranDateFrom.Value.Date & "' AND '" & dtpTranDateTo.Value.Date & "'"
                        Else
                            strSql += vbCrLf + "  1=1 "
                        End If
                        strSql += vbCrLf + "  AND RELIASEDATE BETWEEN '" & dtpRealiseDateFrom.Value.Date & "' AND '" & dtpRealiseDateTo.Value.Date & "'"
                        strSql += vbCrLf + "  AND RELIASEDATE IS NOT NULL"
                        strSql += vbCrLf + "  AND ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
                    End If
                    strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
                    If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += " and T.COSTID in (" & SelectedCostid & ")"

                    strSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y'"

                    strSql += vbCrLf + " UNION ALL"

                    strSql += vbCrLf + "  SELECT CONVERT(VARCHAR,TRANDATE,103)TTRANDATE"
                    strSql += vbCrLf + "  ,CASE WHEN ISNULL(FROMFLAG,'') IN ('A','S','D')  THEN (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)" 'CHQCARDREF"
                    strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'C' THEN 'SCHEME'" 'CHQCARDREF"
                    strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'C' THEN 'PURCHASE'"
                    strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'D' THEN 'SALES'"
                    'strSql += vbCrLf + "   ELSE (SELECT PAYDESC FROM " & cnAdminDb & "..TRANPAYMODE WHERE PAYMODE = T.PAYMODE) END AS PARTICULAR"
                    strSql += vbCrLf + "   ELSE '' END AS PARTICULAR"
                    strSql += vbCrLf + "  ,CHQCARDNO,CHQDATE"
                    strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE NULL END AS DEBIT"
                    strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE NULL END AS CREDIT,RELIASEDATE,SNO,BATCHNO,TRANDATE,'" & cnStockDb & "'DB,CHQCARDREF,AMOUNT,ACCODE"
                    strSql += vbCrLf + "  ,CONVERT(VARCHAR(15),ISNULL(("
                    strSql += vbCrLf + "                             SELECT TOP 1 SNO FROM " & cnStockDb & "..BRSINFO "
                    strSql += vbCrLf + "                             WHERE "
                    strSql += vbCrLf + "                             TRANDATE = T.TRANDATE AND BATCHNO = T.BATCHNO "
                    strSql += vbCrLf + "                             AND ACCODE = T.ACCODE"
                    strSql += vbCrLf + "                             AND ISNULL(CHQCARDNO,'') = ISNULL(T.CHQCARDNO,'')"
                    strSql += vbCrLf + "                             AND ISNULL(CARDID,'') = ISNULL(T.CARDID,'')"
                    strSql += vbCrLf + "                             AND ISNULL(CHQCARDREF,'') = ISNULL(T.CHQCARDREF,'')"
                    strSql += vbCrLf + "                             AND ISNULL(ENTREFNO,'') = ISNULL(T.ENTREFNO,'')"
                    strSql += vbCrLf + "                             AND ISNULL(COSTID,'') = ISNULL(T.COSTID,'')"
                    strSql += vbCrLf + "                             AND COMPANYID = T.COMPANYID"
                    strSql += vbCrLf + "                             AND ISNULL(AMOUNT,0) = ISNULL(T.AMOUNT,0)"
                    strSql += vbCrLf + "                             ),''))BRSSNO,CARDID,COSTID,CONVERT(VARCHAR(100),REMARK1)REMARK1,CONVERT(VARCHAR(100),REMARK2)REMARK2,CONVERT(VARCHAR(2),FROMFLAG)FROMFLAG,CONVERT(VARCHAR(7),CONTRA)CONTRA,CONVERT(VARCHAR(1),'') COLHEAD,'B' BRSACC"
                    strSql += vbCrLf + " , CHQCARDREF as REFNAME,REMARK1 AS REMARKS1,REMARK2 AS REMARKS2,ENTREFNO"
                    'If Not (rbtUnRealised.Checked And preTranDb <> Nothing) And rbtRealised.Checked = False Then  ''PREV DB
                    '    strSql += vbCrLf + "  INTO #ACRECONS"
                    'End If
                    strSql += vbCrLf + "  FROM " & commonTranDb(i) & "..BRS_ACCTRAN AS T WHERE "
                    'strSql += vbCrLf + "  TRANDATE BETWEEN '" & dtpTranDateFrom.Value.Date & "' AND '" & dtpTranDateTo.Value.Date & "'"
                    If chkBasedOnTrandate.Checked Then
                        strSql += vbCrLf + "  TRANDATE BETWEEN '" & dtpTranDateFrom.Value.Date & "' AND '" & dtpTranDateTo.Value.Date & "'"
                    Else
                        strSql += vbCrLf + "  1=1 "
                    End If
                    strSql += vbCrLf + "  AND RELIASEDATE BETWEEN '" & dtpRealiseDateFrom.Value.Date & "' AND '" & dtpRealiseDateTo.Value.Date & "'"
                    strSql += vbCrLf + "  AND RELIASEDATE IS NOT NULL"
                    strSql += vbCrLf + "  AND ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
                    strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
                    If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += " and T.COSTID in (" & SelectedCostid & ")"
                    strSql += vbCrLf + " AND ISNULL(CANCEL,'') <> 'Y'"
                    strSql += vbCrLf + "  ORDER BY TRANDATE"
                    cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

                    strSql = " UPDATE TEMPDB..ACRECONS SET PARTICULAR = CASE WHEN SUBSTRING(ISNULL(PAYMODE,'  '),2,1) = 'R' THEN 'RECEIPT' WHEN SUBSTRING(ISNULL(PAYMODE,'  '),2,1) = 'P' THEN 'PAYMENT' ELSE PARTICULAR END"
                    strSql += vbCrLf + " FROM TEMPDB..ACRECONS AS T"
                    strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..OUTSTANDING AS O ON O.BATCHNO = T.BATCHNO"
                    strSql += vbCrLf + " WHERE T.FROMFLAG = 'P' AND T.DB = '" & commonTranDb(i) & "'"
                    cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

                    strSql = " UPDATE TEMPDB..ACRECONS SET PARTICULAR = 'SCHEME'"
                    strSql += vbCrLf + " WHERE FROMFLAG = 'C'"
                    cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

                    strSql = " SELECT * FROM TEMPDB..ACRECONS ORDER BY TRANDATE"
                    da = New OleDbDataAdapter(strSql, cn)
                    dtGrid.Clear()
                    da.Fill(dtGrid)
                    dtView.Merge(dtGrid)
                Next
            End If
        End If

        strSql = "IF OBJECT_ID('TEMPDB..ACRECONS') IS NOT NULL DROP TABLE TEMPDB..ACRECONS"
        cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()
        strSql = ""
        If rbtUnRealised.Checked And preTranDb1 <> Nothing Then  ''PREV DB            

            strSql += vbCrLf + "  SELECT CONVERT(VARCHAR,TRANDATE,103)TTRANDATE"
            strSql += vbCrLf + "  ,CASE WHEN ISNULL(FROMFLAG,'') IN ('A','S','D') THEN (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)" 'CHQCARDREF"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'C' THEN 'SCHEME'" 'CHQCARDREF"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'C' THEN 'PURCHASE'"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'D' THEN 'SALES'"
            'strSql += vbCrLf + "   ELSE (SELECT PAYDESC FROM " & cnAdminDb & "..TRANPAYMODE WHERE PAYMODE = T.PAYMODE) END AS PARTICULAR"
            strSql += vbCrLf + "   ELSE '' END AS PARTICULAR"
            strSql += vbCrLf + "  ,CHQCARDNO,CHQDATE"
            strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE NULL END AS DEBIT"
            strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE NULL END AS CREDIT,RELIASEDATE,SNO,BATCHNO,TRANDATE,'" & preTranDb1 & "'DB,CHQCARDREF,AMOUNT,ACCODE"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR(15),ISNULL(("
            strSql += vbCrLf + "                             SELECT TOP 1 SNO FROM " & preTranDb1 & "..BRSINFO "
            strSql += vbCrLf + "                             WHERE "
            strSql += vbCrLf + "                             TRANDATE = T.TRANDATE AND BATCHNO = T.BATCHNO "
            strSql += vbCrLf + "                             AND ACCODE = T.ACCODE"
            strSql += vbCrLf + "                             AND ISNULL(CHQCARDNO,'') = ISNULL(T.CHQCARDNO,'')"
            strSql += vbCrLf + "                             AND ISNULL(CARDID,'') = ISNULL(T.CARDID,'')"
            strSql += vbCrLf + "                             AND ISNULL(CHQCARDREF,'') = ISNULL(T.CHQCARDREF,'')"
            strSql += vbCrLf + "                             AND ISNULL(CHQDATE,'') = ISNULL(T.CHQDATE,'')"
            strSql += vbCrLf + "                             AND ISNULL(COSTID,'') = ISNULL(T.COSTID,'')"
            strSql += vbCrLf + "                             AND COMPANYID = T.COMPANYID"
            strSql += vbCrLf + "                             AND ISNULL(AMOUNT,0) = ISNULL(T.AMOUNT,0)"
            strSql += vbCrLf + "                             ),''))BRSSNO,CARDID,COSTID,CONVERT(VARCHAR(100),REMARK1)REMARK1,CONVERT(VARCHAR(100),REMARK2)REMARK2,CONVERT(VARCHAR(2),FROMFLAG)FROMFLAG,CONVERT(VARCHAR(7),CONTRA)CONTRA,CONVERT(VARCHAR(1),'') COLHEAD,'A' BRSACC"
            strSql += vbCrLf + " , CHQCARDREF as REFNAME,REMARK1 AS REMARKS1,REMARK2 AS REMARKS2,ENTREFNO"
            strSql += vbCrLf + "  INTO TEMPDB..ACRECONS"
            strSql += vbCrLf + "  FROM " & preTranDb1 & "..ACCTRAN AS T WHERE "
            ''strSql += vbCrLf + "  TRANDATE <= '" & dtpAsOnDate.Value.Date & "' "
            If ChkAson.Checked Then
                strSql += vbCrLf + "  TRANDATE <= '" & dtpAsOnDate.Value.Date & "' "
            Else
                'strSql += vbCrLf + "  TRANDATE BETWEEN '" & dtpAsOnDate.Value.Date & "' AND '" & dtpUnrealizeDateTo.Value.Date & "'"
                strSql += vbCrLf + "  TRANDATE BETWEEN '" & dtpTranDateFrom.Value.Date & "' AND '" & dtpTranDateTo.Value.Date & "'"
            End If
            '' strSql += vbCrLf + "  AND RELIASEDATE IS NULL"
            strSql += vbCrLf + "  AND (RELIASEDATE IS NULL OR RELIASEDATE > '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "')"
            strSql += vbCrLf + "  AND ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
            strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strSql += " AND COSTID in (" & SelectedCostid & ")"
            End If
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT CONVERT(VARCHAR,TRANDATE,103)TTRANDATE"
            strSql += vbCrLf + "  ,CASE WHEN ISNULL(FROMFLAG,'') IN ('A','S','D') THEN (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)" 'CHQCARDREF"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'C' THEN 'SCHEME'" 'CHQCARDREF"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'C' THEN 'PURCHASE'"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'D' THEN 'SALES'"
            'strSql += vbCrLf + "   ELSE (SELECT PAYDESC FROM " & cnAdminDb & "..TRANPAYMODE WHERE PAYMODE = T.PAYMODE) END AS PARTICULAR"
            strSql += vbCrLf + "   ELSE '' END AS PARTICULAR"
            strSql += vbCrLf + "  ,CHQCARDNO,CHQDATE"
            strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE NULL END AS DEBIT"
            strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE NULL END AS CREDIT,RELIASEDATE,SNO,BATCHNO,TRANDATE,'" & preTranDb1 & "'DB,CHQCARDREF,AMOUNT,ACCODE"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR(15),ISNULL(("
            strSql += vbCrLf + "                             SELECT TOP 1 SNO FROM " & preTranDb1 & "..BRSINFO "
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
            strSql += vbCrLf + " , CHQCARDREF as REFNAME,REMARK1 AS REMARKS1,REMARK2 AS REMARKS2,ENTREFNO"
            'strSql += vbCrLf + "  INTO #ACRECONS"
            strSql += vbCrLf + "  FROM " & preTranDb1 & "..BRS_ACCTRAN AS T WHERE "
            ''strSql += vbCrLf + "  TRANDATE <= '" & dtpAsOnDate.Value.Date & "' "
            If ChkAson.Checked Then
                strSql += vbCrLf + "  TRANDATE <= '" & dtpAsOnDate.Value.Date & "' "
            Else
                'strSql += vbCrLf + "  TRANDATE BETWEEN '" & dtpAsOnDate.Value.Date & "' AND '" & dtpUnrealizeDateTo.Value.Date & "'"
                strSql += vbCrLf + "  TRANDATE BETWEEN '" & dtpTranDateFrom.Value.Date & "' AND '" & dtpTranDateTo.Value.Date & "'"
            End If
            ''strSql += vbCrLf + "  AND RELIASEDATE IS NULL"
            strSql += vbCrLf + "  AND (RELIASEDATE IS NULL OR RELIASEDATE > '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "')"
            strSql += vbCrLf + "  AND ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
            strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strSql += " AND COSTID in (" & SelectedCostid & ")"
            End If
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + "  UNION ALL"
        End If
        If rbtUnRealised.Checked And preTranDb <> Nothing Then  ''PREV DB


            strSql += vbCrLf + "  SELECT CONVERT(VARCHAR,TRANDATE,103)TTRANDATE"
            strSql += vbCrLf + "  ,CASE WHEN ISNULL(FROMFLAG,'') IN ('A','S','D') THEN (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)" 'CHQCARDREF"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'C' THEN 'SCHEME'" 'CHQCARDREF"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'C' THEN 'PURCHASE'"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'D' THEN 'SALES'"
            'strSql += vbCrLf + "   ELSE (SELECT PAYDESC FROM " & cnAdminDb & "..TRANPAYMODE WHERE PAYMODE = T.PAYMODE) END AS PARTICULAR"
            strSql += vbCrLf + "   ELSE '' END AS PARTICULAR"
            strSql += vbCrLf + "  ,CHQCARDNO,CHQDATE"
            strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE NULL END AS DEBIT"

            strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE NULL END AS CREDIT,RELIASEDATE,SNO,BATCHNO,TRANDATE,'" & preTranDb & "'DB,CHQCARDREF,AMOUNT,ACCODE"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR(15),ISNULL(("
            strSql += vbCrLf + "                             SELECT TOP 1 SNO FROM " & preTranDb & "..BRSINFO "
            strSql += vbCrLf + "                             WHERE "
            strSql += vbCrLf + "                             TRANDATE = T.TRANDATE AND BATCHNO = T.BATCHNO "
            strSql += vbCrLf + "                             AND ACCODE = T.ACCODE"
            strSql += vbCrLf + "                             AND ISNULL(CHQCARDNO,'') = ISNULL(T.CHQCARDNO,'')"
            strSql += vbCrLf + "                             AND ISNULL(CARDID,'') = ISNULL(T.CARDID,'')"
            strSql += vbCrLf + "                             AND ISNULL(CHQCARDREF,'') = ISNULL(T.CHQCARDREF,'')"
            strSql += vbCrLf + "                             AND ISNULL(CHQDATE,'') = ISNULL(T.CHQDATE,'')"
            strSql += vbCrLf + "                             AND ISNULL(COSTID,'') = ISNULL(T.COSTID,'')"
            strSql += vbCrLf + "                             AND COMPANYID = T.COMPANYID"
            strSql += vbCrLf + "                             AND ISNULL(AMOUNT,0) = ISNULL(T.AMOUNT,0)"
            strSql += vbCrLf + "                             ),''))BRSSNO,CARDID,COSTID,CONVERT(VARCHAR(100),REMARK1)REMARK1,CONVERT(VARCHAR(100),REMARK2)REMARK2,CONVERT(VARCHAR(2),FROMFLAG)FROMFLAG,CONVERT(VARCHAR(7),CONTRA)CONTRA,CONVERT(VARCHAR(1),'') COLHEAD,'A' BRSACC"
            strSql += vbCrLf + " , CHQCARDREF as REFNAME,REMARK1 AS REMARKS1,REMARK2 AS REMARKS2,ENTREFNO"
            If preTranDb1 = Nothing Then strSql += vbCrLf + "  INTO TEMPDB..ACRECONS"
            strSql += vbCrLf + "  FROM " & preTranDb & "..ACCTRAN AS T WHERE "
            ''strSql += vbCrLf + "  TRANDATE <= '" & dtpAsOnDate.Value.Date & "' "
            If ChkAson.Checked Then
                strSql += vbCrLf + "  TRANDATE <= '" & dtpAsOnDate.Value.Date & "' "
            Else
                strSql += vbCrLf + "  TRANDATE BETWEEN '" & dtpAsOnDate.Value.Date & "' AND '" & dtpUnrealizeDateTo.Value.Date & "'"

            End If
            ''strSql += vbCrLf + "  AND RELIASEDATE IS NULL"
            strSql += vbCrLf + "  AND (RELIASEDATE IS NULL OR RELIASEDATE > '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "')"
            strSql += vbCrLf + "  AND ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
            strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strSql += " AND COSTID in (" & SelectedCostid & ")"

            End If
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT CONVERT(VARCHAR,TRANDATE,103)TTRANDATE"
            strSql += vbCrLf + "  ,CASE WHEN ISNULL(FROMFLAG,'') IN ('A','S','D') THEN (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)" 'CHQCARDREF"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'C' THEN 'SCHEME'" 'CHQCARDREF"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'C' THEN 'PURCHASE'"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'D' THEN 'SALES'"
            'strSql += vbCrLf + "   ELSE (SELECT PAYDESC FROM " & cnAdminDb & "..TRANPAYMODE WHERE PAYMODE = T.PAYMODE) END AS PARTICULAR"
            strSql += vbCrLf + "   ELSE '' END AS PARTICULAR"
            strSql += vbCrLf + "  ,CHQCARDNO,CHQDATE"
            strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE NULL END AS DEBIT"
            strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE NULL END AS CREDIT,RELIASEDATE,SNO,BATCHNO,TRANDATE,'" & preTranDb & "'DB,CHQCARDREF,AMOUNT,ACCODE"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR(15),ISNULL(("
            strSql += vbCrLf + "                             SELECT TOP 1 SNO FROM " & preTranDb & "..BRSINFO "
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
            strSql += vbCrLf + " , CHQCARDREF as REFNAME,REMARK1 AS REMARKS1,REMARK2 AS REMARKS2,ENTREFNO"
            'strSql += vbCrLf + "  INTO #ACRECONS"
            strSql += vbCrLf + "  FROM " & preTranDb & "..BRS_ACCTRAN AS T WHERE "
            ''strSql += vbCrLf + "  TRANDATE <= '" & dtpAsOnDate.Value.Date & "' "
            If ChkAson.Checked Then
                strSql += vbCrLf + "  TRANDATE <= '" & dtpAsOnDate.Value.Date & "' "
            Else
                strSql += vbCrLf + "  TRANDATE BETWEEN '" & dtpAsOnDate.Value.Date & "' AND '" & dtpUnrealizeDateTo.Value.Date & "'"
            End If
            ''strSql += vbCrLf + "  AND RELIASEDATE IS NULL"
            strSql += vbCrLf + "  AND (RELIASEDATE IS NULL OR RELIASEDATE > '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "')"
            strSql += vbCrLf + "  AND ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
            strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strSql += " AND COSTID in (" & SelectedCostid & ")"
            End If
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + "  UNION ALL"
        End If
        If rbtUnRealised.Checked Then
            strSql += vbCrLf + "  SELECT CONVERT(VARCHAR,TRANDATE,103)TTRANDATE"
            strSql += vbCrLf + "  ,CASE WHEN ISNULL(FROMFLAG,'') IN ('A','S','D')  THEN (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)" 'CHQCARDREF"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'C' THEN 'SCHEME'" 'CHQCARDREF"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'C' THEN 'PURCHASE'"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'D' THEN 'SALES'"
            'strSql += vbCrLf + "   ELSE (SELECT PAYDESC FROM " & cnAdminDb & "..TRANPAYMODE WHERE PAYMODE = T.PAYMODE) END AS PARTICULAR"
            strSql += vbCrLf + "   ELSE '' END AS PARTICULAR"
            strSql += vbCrLf + "  ,CHQCARDNO,CHQDATE"
            strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE NULL END AS DEBIT"
            strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE NULL END AS CREDIT,RELIASEDATE,SNO,BATCHNO,TRANDATE,'" & cnStockDb & "'DB,CHQCARDREF,AMOUNT,ACCODE"
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
            strSql += vbCrLf + " , CHQCARDREF as REFNAME,REMARK1 AS REMARKS1,REMARK2 AS REMARKS2,ENTREFNO"
            If (Not (rbtUnRealised.Checked And preTranDb <> Nothing)) And rbtRealised.Checked = False Then  ''PREV DB
                'If strSql.Contains("INTO #ACRECONS") = False Then
                strSql += vbCrLf + "  INTO TEMPDB..ACRECONS"
            End If
            strSql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN AS T WHERE "
            If rbtUnRealised.Checked Then
                If ChkAson.Checked Then
                    strSql += vbCrLf + "  TRANDATE <= '" & dtpAsOnDate.Value.Date & "' "
                Else
                    strSql += vbCrLf + "  TRANDATE BETWEEN '" & dtpAsOnDate.Value.Date & "' AND '" & dtpUnrealizeDateTo.Value.Date & "'"
                End If
                ''strSql += vbCrLf + "  AND RELIASEDATE IS NULL"
                strSql += vbCrLf + "  AND (RELIASEDATE IS NULL OR RELIASEDATE > '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "')"
                strSql += vbCrLf + "  AND ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
            Else
                strSql += vbCrLf + "  TRANDATE BETWEEN '" & dtpTranDateFrom.Value.Date & "' AND '" & dtpTranDateTo.Value.Date & "'"
                strSql += vbCrLf + "  AND RELIASEDATE BETWEEN '" & dtpRealiseDateFrom.Value.Date & "' AND '" & dtpRealiseDateTo.Value.Date & "'"
                strSql += vbCrLf + "  AND RELIASEDATE IS NOT NULL"
                strSql += vbCrLf + "  AND ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
            End If
            strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strSql += " AND COSTID IN (" & SelectedCostid & ")"
            End If
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"

            strSql += vbCrLf + " UNION ALL"

            strSql += vbCrLf + "  SELECT CONVERT(VARCHAR,TRANDATE,103)TTRANDATE"
            strSql += vbCrLf + "  ,CASE WHEN ISNULL(FROMFLAG,'') IN ('A','S','D')  THEN (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)" 'CHQCARDREF"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'C' THEN 'SCHEME'" 'CHQCARDREF"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'C' THEN 'PURCHASE'"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'D' THEN 'SALES'"
            'strSql += vbCrLf + "   ELSE (SELECT PAYDESC FROM " & cnAdminDb & "..TRANPAYMODE WHERE PAYMODE = T.PAYMODE) END AS PARTICULAR"
            strSql += vbCrLf + "   ELSE '' END AS PARTICULAR"
            strSql += vbCrLf + "  ,CHQCARDNO,CHQDATE"
            strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE NULL END AS DEBIT"
            strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE NULL END AS CREDIT,RELIASEDATE,SNO,BATCHNO,TRANDATE,'" & cnStockDb & "'DB,CHQCARDREF,AMOUNT,ACCODE"
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
            strSql += vbCrLf + " , CHQCARDREF as REFNAME,REMARK1 AS REMARKS1,REMARK2 AS REMARKS2,ENTREFNO"
            'If Not (rbtUnRealised.Checked And preTranDb <> Nothing) And rbtRealised.Checked = False Then  ''PREV DB
            '    strSql += vbCrLf + "  INTO #ACRECONS"
            'End If
            strSql += vbCrLf + "  FROM " & cnStockDb & "..BRS_ACCTRAN AS T WHERE "

            ''''strSql += vbCrLf + "  TRANDATE <= '" & dtpAsOnDate.Value.Date & "' "
            If ChkAson.Checked Then
                strSql += vbCrLf + "  TRANDATE <= '" & dtpAsOnDate.Value.Date & "' "
            Else
                strSql += vbCrLf + "  TRANDATE BETWEEN '" & dtpAsOnDate.Value.Date & "' AND '" & dtpUnrealizeDateTo.Value.Date & "'"
            End If
            ''strSql += vbCrLf + "  AND RELIASEDATE IS NULL"
            strSql += vbCrLf + "  AND (RELIASEDATE IS NULL OR RELIASEDATE > '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "')"
            strSql += vbCrLf + "  AND ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"

            strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
                strSql += " AND COSTID in (" & SelectedCostid & ")"
                'strSql += " and COSTID in"
                'strSql += "(select CostId from " & cnAdminDb & "..CostCentre where CostName in (" & GetQryString(chkCmbCostCentre.Text) & "))"
            End If
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"

            strSql += vbCrLf + "  ORDER BY TRANDATE"
            cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

            If preTranDb <> Nothing Then
                strSql = " UPDATE TEMPDB..ACRECONS SET PARTICULAR = CASE WHEN SUBSTRING(ISNULL(PAYMODE,'  '),2,1) = 'R' THEN 'RECEIPT' WHEN SUBSTRING(ISNULL(PAYMODE,'  '),2,1) = 'P' THEN 'PAYMENT' ELSE PARTICULAR END"
                strSql += vbCrLf + " FROM TEMPDB..ACRECONS AS T"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..OUTSTANDING AS O ON O.BATCHNO = T.BATCHNO"
                strSql += vbCrLf + " WHERE T.FROMFLAG = 'P' AND T.DB = '" & preTranDb & "'"
                cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()
            End If
            If preTranDb1 <> Nothing Then
                strSql = " UPDATE TEMPDB..ACRECONS SET PARTICULAR = CASE WHEN SUBSTRING(ISNULL(PAYMODE,'  '),2,1) = 'R' THEN 'RECEIPT' WHEN SUBSTRING(ISNULL(PAYMODE,'  '),2,1) = 'P' THEN 'PAYMENT' ELSE PARTICULAR END"
                strSql += vbCrLf + " FROM TEMPDB..ACRECONS AS T"
                strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..OUTSTANDING AS O ON O.BATCHNO = T.BATCHNO"
                strSql += vbCrLf + " WHERE T.FROMFLAG = 'P' AND T.DB = '" & preTranDb1 & "'"
                cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()
            End If

            strSql = " UPDATE TEMPDB..ACRECONS SET PARTICULAR = CASE WHEN SUBSTRING(ISNULL(PAYMODE,'  '),2,1) = 'R' THEN 'RECEIPT' WHEN SUBSTRING(ISNULL(PAYMODE,'  '),2,1) = 'P' THEN 'PAYMENT' ELSE PARTICULAR END"
            strSql += vbCrLf + " FROM TEMPDB..ACRECONS AS T"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..OUTSTANDING AS O ON O.BATCHNO = T.BATCHNO"
            strSql += vbCrLf + " WHERE T.FROMFLAG = 'P' AND T.DB = '" & cnStockDb & "'"
            cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

            strSql = " UPDATE TEMPDB..ACRECONS SET PARTICULAR = 'SCHEME'"
            strSql += vbCrLf + " WHERE FROMFLAG = 'C'"
            cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

            strSql = " SELECT * FROM TEMPDB..ACRECONS ORDER BY TRANDATE"

            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtView)
            'dtView.Merge(dtGrid)
        End If


        gridView_OWN.DataSource = dtView
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

        ''ADDING BALANCE
        If rbtRealised.Checked Then
            dtpAsOnDate.Value = dtpRealiseDateFrom.Value
            Row = Nothing
            Row = dtView.NewRow
            DebitAmt = DebitAmt + Val(dtView.Compute("SUM(DEBIT)", "COLHEAD = 'G'").ToString)
            CreditAmt = CreditAmt + Val(dtView.Compute("SUM(CREDIT)", "COLHEAD = 'G'").ToString)
            Row.Item("PARTICULAR") = "BALANCE ->"
            Row.Item("DEBIT") = IIf(DebitAmt > CreditAmt, Format(DebitAmt - CreditAmt, "0.00"), DBNull.Value)
            Row.Item("CREDIT") = IIf(CreditAmt > DebitAmt, Format(CreditAmt - DebitAmt, "0.00"), DBNull.Value)
            Row.Item("COLHEAD") = "G"
            dtView.Rows.Add(Row)
            If gridView_OWN.Rows(0).Cells("PARTICULAR").Value.ToString().Trim = "OPENING" Then
                gridView_OWN.Rows(0).DefaultCellStyle = GlobalVariables.reportSubTotalStyle
            End If
            gridView_OWN.Rows(gridView_OWN.RowCount - 2).DefaultCellStyle = GlobalVariables.reportSubTotalStyle
            gridView_OWN.Rows(gridView_OWN.RowCount - 1).DefaultCellStyle = GlobalVariables.reportTotalStyle
            Row = Nothing
            Row = dtView.NewRow
            dtView.Rows.Add(Row)
            AmtNotReflectedAcc(dtView)
        Else
            gridView_OWN.Rows(gridView_OWN.RowCount - 1).DefaultCellStyle = GlobalVariables.reportTotalStyle
        End If


        If rbtUnRealised.Checked Then
            ''Accounts Not Reflected In Bank
            Row = dtView.NewRow
            Row.Item("PARTICULAR") = "(A) Unrealised Balance"
            Dim Adr As Decimal = IIf(DebitAmt - CreditAmt > 0, DebitAmt - CreditAmt, 0)
            Dim Acr As Decimal = IIf(CreditAmt - DebitAmt > 0, Math.Abs(CreditAmt - DebitAmt), 0)
            Row.Item("DEBIT") = Format(Adr, "0.00")
            Row.Item("CREDIT") = Format(Acr, "0.00")
            Row.Item("COLHEAD") = "G"
            dtView.Rows.Add(Row)
            gridView_OWN.Rows(gridView_OWN.RowCount - 1).DefaultCellStyle = GlobalVariables.reportTotalStyle
            ''AS PER THE COMPANY BOOK
            strSql = " SELECT SUM(AMOUNT) AS AMOUNT FROM "
            strSql += vbCrLf + " ("
            strSql += vbCrLf + "  SELECT DEBIT-CREDIT AS AMOUNT FROM " & cnStockDb & "..OPENTRAILBALANCE"
            strSql += vbCrLf + "  WHERE ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
            strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END AS AMOUNT"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN"
            strSql += vbCrLf + "  WHERE TRANDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + "  AND ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
            strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"

            strSql += vbCrLf + " )X"
            Dim Bal As Decimal = Nothing
            Bal = Val(objGPack.GetSqlValue(strSql))
            Row = dtView.NewRow
            Row.Item("PARTICULAR") = "(B) Balance As Per Account Statement"
            Dim Bdr As Decimal = IIf(Bal > 0, Bal, 0)
            Dim Bcr As Decimal = IIf(Bal < 0, Math.Abs(Bal), 0)
            Row.Item("DEBIT") = IIf(Bal > 0, Format(Bal, "0.00"), DBNull.Value)
            Row.Item("CREDIT") = IIf(Bal < 0, Format(Math.Abs(Bal), "0.00"), DBNull.Value)
            Row.Item("COLHEAD") = "G"
            dtView.Rows.Add(Row)
            gridView_OWN.Rows(gridView_OWN.RowCount - 1).DefaultCellStyle = GlobalVariables.reportTotalStyle
            'Balance as per Bank

            strSql = " SELECT SUM(AMOUNT) AS AMOUNT FROM "
            strSql += vbCrLf + " ("
            strSql += vbCrLf + "  SELECT DEBIT-CREDIT AS AMOUNT"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..OPENTRAILBALANCE  "
            strSql += vbCrLf + "  WHERE ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
            strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + "  SELECT CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END AS AMOUNT"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN"
            strSql += vbCrLf + "  WHERE RELIASEDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + "  AND ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
            strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + "  SELECT CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END AS AMOUNT"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..BRS_ACCTRAN"
            strSql += vbCrLf + "  WHERE RELIASEDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + "  AND ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
            strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
            If preTranDb <> "" Then
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + "  SELECT CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END AS AMOUNT"
                strSql += vbCrLf + "  FROM " & preTranDb & "..ACCTRAN"
                strSql += vbCrLf + "  WHERE RELIASEDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''"
                strSql += vbCrLf + "  AND ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
                strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + "  SELECT CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END AS AMOUNT"
                strSql += vbCrLf + "  FROM " & preTranDb & "..BRS_ACCTRAN"
                strSql += vbCrLf + "  WHERE RELIASEDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''"
                strSql += vbCrLf + "  AND ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
                strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"

            End If
            If preTranDb1 <> "" Then
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + "  SELECT CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END AS AMOUNT"
                strSql += vbCrLf + "  FROM " & preTranDb1 & "..ACCTRAN"
                strSql += vbCrLf + "  WHERE RELIASEDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''"
                strSql += vbCrLf + "  AND ACCODE = (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
                strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + "  SELECT CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE -1*AMOUNT END AS AMOUNT"
                strSql += vbCrLf + "  FROM " & preTranDb1 & "..BRS_ACCTRAN"
                strSql += vbCrLf + "  WHERE RELIASEDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''"
                strSql += vbCrLf + "  AND ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
                strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"

            End If
            strSql += vbCrLf + " )X"
            Bal = Val(objGPack.GetSqlValue(strSql))
            Row = dtView.NewRow
            Row.Item("PARTICULAR") = "(C) Balance As Per Bank Statement (B-A)"
            Dim Cdr As Decimal = Bdr - Adr
            Dim Ccr As Decimal = Bcr - Acr
            Bal = Cdr - Ccr
            Row.Item("DEBIT") = IIf(Bal > 0, Format(Bal, "0.00"), DBNull.Value)
            Row.Item("CREDIT") = IIf(Bal < 0, Format(Math.Abs(Bal), "0.00"), DBNull.Value)
            Row.Item("COLHEAD") = "G"
            dtView.Rows.Add(Row)
            gridView_OWN.Rows(gridView_OWN.RowCount - 1).DefaultCellStyle = GlobalVariables.reportTotalStyle
        Else

        End If
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
            .Columns("RELIASEDATE").Width = dtpGridRealise.Width
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
            .Columns("ACCODE").Visible = False
            .Columns("CARDID").Visible = False
            .Columns("DB").Visible = False
            .Columns("BRSSNO").Visible = False
            .Columns("colhead").Visible = False
            .Columns("FROMFLAG").Visible = False
            .Columns("CONTRA").Visible = False
            .Columns("BRSACC").Visible = False
        End With
        objSearch = New frmGridSearch(gridView_OWN)
        gridView_OWN.Focus()
    End Sub
    Private Sub AmtNotReflectedAcc(ByRef dtView As DataTable)
        strSql = "IF OBJECT_ID('TEMPDB..NOT_REF_ACRECONS') IS NOT NULL DROP TABLE TEMPDB..NOT_REF_ACRECONS"
        If preTranDb1 <> Nothing Then
            strSql += vbCrLf + "  SELECT CONVERT(VARCHAR,TRANDATE,103)TTRANDATE"
            strSql += vbCrLf + "  ,CASE WHEN ISNULL(FROMFLAG,'') IN ('A','S') THEN (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)" 'CHQCARDREF"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'C' THEN 'SCHEME'" 'CHQCARDREF"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'C' THEN 'PURCHASE'"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'D' THEN 'SALES'"
            'strSql += vbCrLf + "   ELSE (SELECT PAYDESC FROM " & cnAdminDb & "..TRANPAYMODE WHERE PAYMODE = T.PAYMODE) END AS PARTICULAR"
            strSql += vbCrLf + "   ELSE '' END AS PARTICULAR"
            strSql += vbCrLf + "  ,CHQCARDNO,CHQDATE"
            strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE NULL END AS DEBIT"
            strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE NULL END AS CREDIT,RELIASEDATE,SNO,BATCHNO,ENTREFNO,TRANDATE,'" & preTranDb1 & "'DB,CHQCARDREF,AMOUNT,ACCODE"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR(15),ISNULL(("
            strSql += vbCrLf + "                             SELECT TOP 1 SNO FROM " & preTranDb1 & "..BRSINFO "
            strSql += vbCrLf + "                             WHERE "
            strSql += vbCrLf + "                             TRANDATE = T.TRANDATE AND BATCHNO = T.BATCHNO AND ENTREFNO=T.ENTREFNO "
            strSql += vbCrLf + "                             AND ACCODE = T.ACCODE"
            strSql += vbCrLf + "                             AND ISNULL(CHQCARDNO,'') = ISNULL(T.CHQCARDNO,'')"
            strSql += vbCrLf + "                             AND ISNULL(CARDID,'') = ISNULL(T.CARDID,'')"
            strSql += vbCrLf + "                             AND ISNULL(CHQCARDREF,'') = ISNULL(T.CHQCARDREF,'')"
            strSql += vbCrLf + "                             AND ISNULL(CHQDATE,'') = ISNULL(T.CHQDATE,'')"
            strSql += vbCrLf + "                             AND ISNULL(COSTID,'') = ISNULL(T.COSTID,'')"
            strSql += vbCrLf + "                             AND COMPANYID = T.COMPANYID"
            strSql += vbCrLf + "                             AND AMOUNT = T.AMOUNT"
            strSql += vbCrLf + "                             ),''))BRSSNO,CARDID,COSTID,CONVERT(VARCHAR(100),REMARK1)REMARK1,CONVERT(VARCHAR(100),REMARK2)REMARK2,CONVERT(VARCHAR(2),FROMFLAG)FROMFLAG,CONVERT(VARCHAR(7),CONTRA)CONTRA,CONVERT(VARCHAR(1),'') COLHEAD,'A' BRSACC"
            strSql += vbCrLf + "  INTO TEMPDB..NOT_REF_ACRECONS"
            strSql += vbCrLf + "  FROM " & preTranDb1 & "..ACCTRAN AS T WHERE "
            strSql += vbCrLf + "  TRANDATE <= '" & dtpAsOnDate.Value.Date & "' "
            'strSql += vbCrLf + "  AND RELIASEDATE IS NULL"
            strSql += vbCrLf + "  AND (RELIASEDATE IS NULL OR RELIASEDATE > '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "')"
            strSql += vbCrLf + "  AND ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
            strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += " and COSTID in (" & SelectedCostid & ")"

            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT CONVERT(VARCHAR,TRANDATE,103)TTRANDATE"
            strSql += vbCrLf + "  ,CASE WHEN ISNULL(FROMFLAG,'') IN ('A','S') THEN (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)" 'CHQCARDREF"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'C' THEN 'SCHEME'" 'CHQCARDREF"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'C' THEN 'PURCHASE'"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'D' THEN 'SALES'"
            'strSql += vbCrLf + "   ELSE (SELECT PAYDESC FROM " & cnAdminDb & "..TRANPAYMODE WHERE PAYMODE = T.PAYMODE) END AS PARTICULAR"
            strSql += vbCrLf + "   ELSE '' END AS PARTICULAR"
            strSql += vbCrLf + "  ,CHQCARDNO,CHQDATE"
            strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE NULL END AS DEBIT"
            strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE NULL END AS CREDIT,RELIASEDATE,SNO,BATCHNO,ENTREFNO,TRANDATE,'" & preTranDb1 & "'DB,CHQCARDREF,AMOUNT,ACCODE"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR(15),ISNULL(("
            strSql += vbCrLf + "                             SELECT TOP 1 SNO FROM " & preTranDb1 & "..BRSINFO "
            strSql += vbCrLf + "                             WHERE "
            strSql += vbCrLf + "                             TRANDATE = T.TRANDATE AND BATCHNO = T.BATCHNO AND ENTREFNO = T.ENTREFNO "
            strSql += vbCrLf + "                             AND ACCODE = T.ACCODE"
            strSql += vbCrLf + "                             AND ISNULL(CHQCARDNO,'') = ISNULL(T.CHQCARDNO,'')"
            strSql += vbCrLf + "                             AND ISNULL(CARDID,'') = ISNULL(T.CARDID,'')"
            strSql += vbCrLf + "                             AND ISNULL(CHQCARDREF,'') = ISNULL(T.CHQCARDREF,'')"
            strSql += vbCrLf + "                             AND ISNULL(CHQDATE,'') = ISNULL(T.CHQDATE,'')"
            strSql += vbCrLf + "                             AND ISNULL(COSTID,'') = ISNULL(T.COSTID,'')"
            strSql += vbCrLf + "                             AND COMPANYID = T.COMPANYID"
            strSql += vbCrLf + "                             AND AMOUNT = T.AMOUNT"
            strSql += vbCrLf + "                             ),''))BRSSNO,CARDID,COSTID,CONVERT(VARCHAR(100),REMARK1)REMARK1,CONVERT(VARCHAR(100),REMARK2)REMARK2,CONVERT(VARCHAR(2),FROMFLAG)FROMFLAG,CONVERT(VARCHAR(7),CONTRA)CONTRA,CONVERT(VARCHAR(1),'') COLHEAD,'B' BRSACC"
            'strSql += vbCrLf + "  INTO #NOT_REF_ACRECONS"
            strSql += vbCrLf + "  FROM " & preTranDb1 & "..BRS_ACCTRAN AS T WHERE "
            strSql += vbCrLf + "  TRANDATE <= '" & dtpAsOnDate.Value.Date & "' "
            'strSql += vbCrLf + "  AND RELIASEDATE IS NULL"
            strSql += vbCrLf + "  AND (RELIASEDATE IS NULL OR RELIASEDATE > '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "')"
            strSql += vbCrLf + "  AND ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
            strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += " and COSTID in (" & SelectedCostid & ")"
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + "  UNION ALL"
        End If
        If preTranDb <> Nothing Then
            strSql += vbCrLf + "  SELECT CONVERT(VARCHAR,TRANDATE,103)TTRANDATE"
            strSql += vbCrLf + "  ,CASE WHEN ISNULL(FROMFLAG,'') IN ('A','S') THEN (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)" 'CHQCARDREF"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'C' THEN 'SCHEME'" 'CHQCARDREF"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'C' THEN 'PURCHASE'"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'D' THEN 'SALES'"
            'strSql += vbCrLf + "   ELSE (SELECT PAYDESC FROM " & cnAdminDb & "..TRANPAYMODE WHERE PAYMODE = T.PAYMODE) END AS PARTICULAR"
            strSql += vbCrLf + "   ELSE '' END AS PARTICULAR"
            strSql += vbCrLf + "  ,CHQCARDNO,CHQDATE"
            strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE NULL END AS DEBIT"
            strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE NULL END AS CREDIT,RELIASEDATE,SNO,BATCHNO,ENTREFNO,TRANDATE,'" & preTranDb & "'DB,CHQCARDREF,AMOUNT,ACCODE"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR(15),ISNULL(("
            strSql += vbCrLf + "                             SELECT TOP 1 SNO FROM " & preTranDb & "..BRSINFO "
            strSql += vbCrLf + "                             WHERE "
            strSql += vbCrLf + "                             TRANDATE = T.TRANDATE AND BATCHNO = T.BATCHNO AND ENTREFNO=T.ENTREFNO "
            strSql += vbCrLf + "                             AND ACCODE = T.ACCODE"
            strSql += vbCrLf + "                             AND ISNULL(CHQCARDNO,'') = ISNULL(T.CHQCARDNO,'')"
            strSql += vbCrLf + "                             AND ISNULL(CARDID,'') = ISNULL(T.CARDID,'')"
            strSql += vbCrLf + "                             AND ISNULL(CHQCARDREF,'') = ISNULL(T.CHQCARDREF,'')"
            strSql += vbCrLf + "                             AND ISNULL(CHQDATE,'') = ISNULL(T.CHQDATE,'')"
            strSql += vbCrLf + "                             AND ISNULL(COSTID,'') = ISNULL(T.COSTID,'')"
            strSql += vbCrLf + "                             AND COMPANYID = T.COMPANYID"
            strSql += vbCrLf + "                             AND AMOUNT = T.AMOUNT"
            strSql += vbCrLf + "                             ),''))BRSSNO,CARDID,COSTID,CONVERT(VARCHAR(100),REMARK1)REMARK1,CONVERT(VARCHAR(100),REMARK2)REMARK2,CONVERT(VARCHAR(2),FROMFLAG)FROMFLAG,CONVERT(VARCHAR(7),CONTRA)CONTRA,CONVERT(VARCHAR(1),'') COLHEAD,'A' BRSACC"
            If preTranDb1 = Nothing Then strSql += vbCrLf + "  INTO TEMPDB..NOT_REF_ACRECONS"
            strSql += vbCrLf + "  FROM " & preTranDb & "..ACCTRAN AS T WHERE "
            strSql += vbCrLf + "  TRANDATE <= '" & dtpAsOnDate.Value.Date & "' "
            'strSql += vbCrLf + "  AND RELIASEDATE IS NULL"
            strSql += vbCrLf + "  AND (RELIASEDATE IS NULL OR RELIASEDATE > '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "')"
            strSql += vbCrLf + "  AND ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
            strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += " and COSTID in (" & SelectedCostid & ")"

            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT CONVERT(VARCHAR,TRANDATE,103)TTRANDATE"
            strSql += vbCrLf + "  ,CASE WHEN ISNULL(FROMFLAG,'') IN ('A','S') THEN (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)" 'CHQCARDREF"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'C' THEN 'SCHEME'" 'CHQCARDREF"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'C' THEN 'PURCHASE'"
            strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'D' THEN 'SALES'"
            'strSql += vbCrLf + "   ELSE (SELECT PAYDESC FROM " & cnAdminDb & "..TRANPAYMODE WHERE PAYMODE = T.PAYMODE) END AS PARTICULAR"
            strSql += vbCrLf + "   ELSE '' END AS PARTICULAR"
            strSql += vbCrLf + "  ,CHQCARDNO,CHQDATE"
            strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE NULL END AS DEBIT"
            strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE NULL END AS CREDIT,RELIASEDATE,SNO,BATCHNO,ENTREFNO,TRANDATE,'" & preTranDb & "'DB,CHQCARDREF,AMOUNT,ACCODE"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR(15),ISNULL(("
            strSql += vbCrLf + "                             SELECT TOP 1 SNO FROM " & preTranDb & "..BRSINFO "
            strSql += vbCrLf + "                             WHERE "
            strSql += vbCrLf + "                             TRANDATE = T.TRANDATE AND BATCHNO = T.BATCHNO AND ENTREFNO = T.ENTREFNO "
            strSql += vbCrLf + "                             AND ACCODE = T.ACCODE"
            strSql += vbCrLf + "                             AND ISNULL(CHQCARDNO,'') = ISNULL(T.CHQCARDNO,'')"
            strSql += vbCrLf + "                             AND ISNULL(CARDID,'') = ISNULL(T.CARDID,'')"
            strSql += vbCrLf + "                             AND ISNULL(CHQCARDREF,'') = ISNULL(T.CHQCARDREF,'')"
            strSql += vbCrLf + "                             AND ISNULL(CHQDATE,'') = ISNULL(T.CHQDATE,'')"
            strSql += vbCrLf + "                             AND ISNULL(COSTID,'') = ISNULL(T.COSTID,'')"
            strSql += vbCrLf + "                             AND COMPANYID = T.COMPANYID"
            strSql += vbCrLf + "                             AND AMOUNT = T.AMOUNT"
            strSql += vbCrLf + "                             ),''))BRSSNO,CARDID,COSTID,CONVERT(VARCHAR(100),REMARK1)REMARK1,CONVERT(VARCHAR(100),REMARK2)REMARK2,CONVERT(VARCHAR(2),FROMFLAG)FROMFLAG,CONVERT(VARCHAR(7),CONTRA)CONTRA,CONVERT(VARCHAR(1),'') COLHEAD,'B' BRSACC"
            'strSql += vbCrLf + "  INTO #NOT_REF_ACRECONS"
            strSql += vbCrLf + "  FROM " & preTranDb & "..BRS_ACCTRAN AS T WHERE "
            strSql += vbCrLf + "  TRANDATE <= '" & dtpAsOnDate.Value.Date & "' "
            'strSql += vbCrLf + "  AND RELIASEDATE IS NULL"
            strSql += vbCrLf + "  AND (RELIASEDATE IS NULL OR RELIASEDATE > '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "')"
            strSql += vbCrLf + "  AND ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"
            strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
            If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += " and COSTID in (" & SelectedCostid & ")"
            strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + "  UNION ALL"
        End If
        strSql += vbCrLf + "  SELECT CONVERT(VARCHAR,TRANDATE,103)TTRANDATE"
        strSql += vbCrLf + "  ,CASE WHEN ISNULL(FROMFLAG,'') IN ('A','S')  THEN (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)" 'CHQCARDREF"
        strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'C' THEN 'SCHEME'" 'CHQCARDREF"
        strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'C' THEN 'PURCHASE'"
        strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'D' THEN 'SALES'"
        'strSql += vbCrLf + "   ELSE (SELECT PAYDESC FROM " & cnAdminDb & "..TRANPAYMODE WHERE PAYMODE = T.PAYMODE) END AS PARTICULAR"
        strSql += vbCrLf + "   ELSE '' END AS PARTICULAR"
        strSql += vbCrLf + "  ,CHQCARDNO,CHQDATE"
        strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE NULL END AS DEBIT"
        strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE NULL END AS CREDIT,RELIASEDATE,SNO,BATCHNO,ENTREFNO,TRANDATE,'" & cnStockDb & "'DB,CHQCARDREF,AMOUNT,ACCODE"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(15),ISNULL(("
        strSql += vbCrLf + "                             SELECT TOP 1 SNO FROM " & cnStockDb & "..BRSINFO "
        strSql += vbCrLf + "                             WHERE "
        strSql += vbCrLf + "                             TRANDATE = T.TRANDATE AND BATCHNO = T.BATCHNO AND ENTREFNO = T.ENTREFNO "
        strSql += vbCrLf + "                             AND ACCODE = T.ACCODE"
        strSql += vbCrLf + "                             AND ISNULL(CHQCARDNO,'') = ISNULL(T.CHQCARDNO,'')"
        strSql += vbCrLf + "                             AND ISNULL(CARDID,'') = ISNULL(T.CARDID,'')"
        strSql += vbCrLf + "                             AND ISNULL(CHQCARDREF,'') = ISNULL(T.CHQCARDREF,'')"
        strSql += vbCrLf + "                             AND ISNULL(CHQDATE,'') = ISNULL(T.CHQDATE,'')"
        strSql += vbCrLf + "                             AND ISNULL(COSTID,'') = ISNULL(T.COSTID,'')"
        strSql += vbCrLf + "                             AND COMPANYID = T.COMPANYID"
        strSql += vbCrLf + "                             AND AMOUNT = T.AMOUNT"
        strSql += vbCrLf + "                             ),''))BRSSNO,CARDID,COSTID,CONVERT(VARCHAR(100),REMARK1)REMARK1,CONVERT(VARCHAR(100),REMARK2)REMARK2,CONVERT(VARCHAR(2),FROMFLAG)FROMFLAG,CONVERT(VARCHAR(7),CONTRA)CONTRA,CONVERT(VARCHAR(1),'') COLHEAD,'A' BRSACC"
        If Not (preTranDb <> Nothing) Then  ''PREV DB
            strSql += vbCrLf + "  INTO TEMPDB..NOT_REF_ACRECONS"
        End If

        strSql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN AS T WHERE "
        strSql += vbCrLf + "  TRANDATE <= '" & dtpAsOnDate.Value.Date & "' "
        ''strSql += vbCrLf + "  AND RELIASEDATE IS NULL"
        strSql += vbCrLf + "  AND (RELIASEDATE IS NULL OR RELIASEDATE > '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "')"
        strSql += vbCrLf + "  AND ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"

        strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += " and COSTID in (" & SelectedCostid & ")"

        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + "  SELECT CONVERT(VARCHAR,TRANDATE,103)TTRANDATE"
        strSql += vbCrLf + "  ,CASE WHEN ISNULL(FROMFLAG,'') IN ('A','S')  THEN (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE = T.CONTRA)" 'CHQCARDREF"
        strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'C' THEN 'SCHEME'" 'CHQCARDREF"
        strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'C' THEN 'PURCHASE'"
        strSql += vbCrLf + "        WHEN ISNULL(FROMFLAG,'') = 'P' AND TRANMODE = 'D' THEN 'SALES'"
        'strSql += vbCrLf + "   ELSE (SELECT PAYDESC FROM " & cnAdminDb & "..TRANPAYMODE WHERE PAYMODE = T.PAYMODE) END AS PARTICULAR"
        strSql += vbCrLf + "   ELSE '' END AS PARTICULAR"
        strSql += vbCrLf + "  ,CHQCARDNO,CHQDATE"
        strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'D' THEN AMOUNT ELSE NULL END AS DEBIT"
        strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'C' THEN AMOUNT ELSE NULL END AS CREDIT,RELIASEDATE,SNO,BATCHNO,ENTREFNO,TRANDATE,'" & cnStockDb & "'DB,CHQCARDREF,AMOUNT,ACCODE"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(15),ISNULL(("
        strSql += vbCrLf + "                             SELECT TOP 1 SNO FROM " & cnStockDb & "..BRSINFO "
        strSql += vbCrLf + "                             WHERE "
        strSql += vbCrLf + "                             TRANDATE = T.TRANDATE AND BATCHNO = T.BATCHNO AND ENTREFNO = T.ENTREFNO "
        strSql += vbCrLf + "                             AND ACCODE = T.ACCODE"
        strSql += vbCrLf + "                             AND ISNULL(CHQCARDNO,'') = ISNULL(T.CHQCARDNO,'')"
        strSql += vbCrLf + "                             AND ISNULL(CARDID,'') = ISNULL(T.CARDID,'')"
        strSql += vbCrLf + "                             AND ISNULL(CHQCARDREF,'') = ISNULL(T.CHQCARDREF,'')"
        strSql += vbCrLf + "                             AND ISNULL(CHQDATE,'') = ISNULL(T.CHQDATE,'')"
        strSql += vbCrLf + "                             AND ISNULL(COSTID,'') = ISNULL(T.COSTID,'')"
        strSql += vbCrLf + "                             AND COMPANYID = T.COMPANYID"
        strSql += vbCrLf + "                             AND AMOUNT = T.AMOUNT"
        strSql += vbCrLf + "                             ),''))BRSSNO,CARDID,COSTID,CONVERT(VARCHAR(100),REMARK1)REMARK1,CONVERT(VARCHAR(100),REMARK2)REMARK2,CONVERT(VARCHAR(2),FROMFLAG)FROMFLAG,CONVERT(VARCHAR(7),CONTRA)CONTRA,CONVERT(VARCHAR(1),'') COLHEAD,'B' BRSACC"
        'If Not (preTranDb <> Nothing) Then  ''PREV DB
        '    strSql += vbCrLf + "  INTO #NOT_REF_ACRECONS"
        'End If
        strSql += vbCrLf + "  FROM " & cnStockDb & "..BRS_ACCTRAN AS T WHERE "
        strSql += vbCrLf + "  TRANDATE <= '" & dtpAsOnDate.Value.Date & "' "
        ''strSql += vbCrLf + "  AND RELIASEDATE IS NULL"
        strSql += vbCrLf + "  AND (RELIASEDATE IS NULL OR RELIASEDATE > '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "')"
        strSql += vbCrLf + "  AND ACCODE = '" & SelectedBankid & "'"  '(SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & cmbBank_MAN.Text & "')"

        strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then strSql += " and COSTID in (" & SelectedCostid & ")"

        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + "  ORDER BY TRANDATE"
        cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        If preTranDb1 <> Nothing Then
            strSql += vbCrLf + " UPDATE TEMPDB..NOT_REF_ACRECONS SET PARTICULAR = CASE WHEN SUBSTRING(ISNULL(PAYMODE,'  '),2,1) = 'R' THEN 'RECEIPT' WHEN SUBSTRING(ISNULL(PAYMODE,'  '),2,1) = 'P' THEN 'PAYMENT' ELSE PARTICULAR END"
            strSql += vbCrLf + " FROM TEMPDB..NOT_REF_ACRECONS AS T"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..OUTSTANDING AS O ON O.BATCHNO = T.BATCHNO"
            strSql += vbCrLf + " WHERE T.FROMFLAG = 'P' AND T.DB = '" & preTranDb1 & "'"
            cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()
        End If

        If preTranDb <> Nothing Then
            strSql += vbCrLf + " UPDATE TEMPDB..NOT_REF_ACRECONS SET PARTICULAR = CASE WHEN SUBSTRING(ISNULL(PAYMODE,'  '),2,1) = 'R' THEN 'RECEIPT' WHEN SUBSTRING(ISNULL(PAYMODE,'  '),2,1) = 'P' THEN 'PAYMENT' ELSE PARTICULAR END"
            strSql += vbCrLf + " FROM TEMPDB..NOT_REF_ACRECONS AS T"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..OUTSTANDING AS O ON O.BATCHNO = T.BATCHNO"
            strSql += vbCrLf + " WHERE T.FROMFLAG = 'P' AND T.DB = '" & preTranDb & "'"
            cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()
        End If
        strSql += vbCrLf + " UPDATE TEMPDB..NOT_REF_ACRECONS SET PARTICULAR = CASE WHEN SUBSTRING(ISNULL(PAYMODE,'  '),2,1) = 'R' THEN 'RECEIPT' WHEN SUBSTRING(ISNULL(PAYMODE,'  '),2,1) = 'P' THEN 'PAYMENT' ELSE PARTICULAR END"
        strSql += vbCrLf + " FROM TEMPDB..NOT_REF_ACRECONS AS T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..OUTSTANDING AS O ON O.BATCHNO = T.BATCHNO"
        strSql += vbCrLf + " WHERE T.FROMFLAG = 'P' AND T.DB = '" & cnStockDb & "'"
        cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        strSql += vbCrLf + " UPDATE TEMPDB..NOT_REF_ACRECONS SET PARTICULAR = 'SCHEME'"
        strSql += vbCrLf + " WHERE FROMFLAG = 'C'"
        cmd = New OleDbCommand(strSql, cn, tran) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()



        strSql = " SELECT * FROM TEMPDB..NOT_REF_ACRECONS ORDER BY TRANDATE"
        Dim dtSubView As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtSubView)
        If Not dtSubView.Rows.Count > 0 Then
            Exit Sub
        End If

        ''ADDING TOTAL
        Dim DebitAmt As Decimal = Nothing
        Dim CreditAmt As Decimal = Nothing
        Dim Row As DataRow = Nothing
        Row = dtSubView.NewRow
        DebitAmt = Val(dtSubView.Compute("SUM(DEBIT)", "COLHEAD = '' OR COLHEAD IS NULL").ToString)
        CreditAmt = Val(dtSubView.Compute("SUM(CREDIT)", "COLHEAD = '' OR COLHEAD IS NULL").ToString)

        Row = dtView.NewRow
        Row.Item("PARTICULAR") = "Accounts Not Reflected In Account"
        Row.Item("DEBIT") = IIf(CreditAmt - DebitAmt > 0, Format(CreditAmt - DebitAmt, "0.00"), DBNull.Value)
        Row.Item("CREDIT") = IIf(DebitAmt - CreditAmt > 0, Format(DebitAmt - CreditAmt, "0.00"), DBNull.Value)
        Row.Item("COLHEAD") = "G"
        dtView.Rows.Add(Row)
        gridView_OWN.Rows(gridView_OWN.RowCount - 1).DefaultCellStyle = GlobalVariables.reportSubTotalStyle
    End Sub
    Private Sub gridView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView_OWN.GotFocus
        lblStatus.Visible = True
        lblStatus1.Visible = False
        If gridView_OWN.CurrentRow.Cells("RELIASEDATE").Value.ToString <> "" Then
            lblStatus.Text = "Press Del Key to Un Realize"
        Else
            lblStatus.Text = "Press Enter to Set Realize Date"
        End If
        lblhint.Visible = True
        lblhint.Text = "Press [N] for find Next"
    End Sub

    Private Sub gridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridView_OWN.KeyDown
        If Not gridView_OWN.RowCount > 0 Then Exit Sub
        If e.Control Then
            If e.KeyCode = Keys.F Then
                'objSearch.Show() 
                If objSearch IsNot Nothing Then
                    objSearch = Nothing
                End If
                objSearch = New frmGridSearch(gridView_OWN, Val(FindSearchFormat))
                objSearch.Show()

            ElseIf e.KeyCode = Keys.S Then
                If objSearch IsNot Nothing Then
                    CType(objSearch, frmGridSearch).btnFindNext_Click(Me, New EventArgs)
                End If
            End If
        End If
        If e.KeyCode = Keys.N Then
            If objSearch IsNot Nothing Then
                CType(objSearch, frmGridSearch).btnFindNext_Click(Me, New EventArgs)
            End If
        End If
        If e.KeyCode = Keys.E Then
            e.Handled = True
            If gridView_OWN.CurrentRow.Cells("TRANDATE").Value.ToString = "" Then Exit Sub
            gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("RELIASEDATE")
        ElseIf e.KeyCode = Keys.Delete Then
            If gridView_OWN.CurrentRow.Cells("COLHEAD").Value.ToString <> "" Then Exit Sub
            If MessageBox.Show("Sure you want to unrealize?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                UpdateUnRealise()
            End If
        End If
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView_OWN.KeyPress
        If Not gridView_OWN.RowCount > 0 Then Exit Sub
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
        If gridView_OWN.CurrentRow.Cells("COLHEAD").Value.ToString <> "" Then Exit Sub
        If gridView_OWN.CurrentRow.Cells("TRANDATE").Value.ToString = "" Then Exit Sub
        If UCase(e.KeyChar) = Chr(Keys.E) Then
            Dim pt As Point = gridView_OWN.Location
            dtpGridRealise.Visible = True
            pt = pt + gridView_OWN.GetCellDisplayRectangle(gridView_OWN.Columns("RELIASEDATE").Index, gridView_OWN.CurrentRow.Index, False).Location
            If gridView_OWN.CurrentRow.Cells("CHQDATE").Value.ToString <> "" Then
                dtpGridRealise.MinDate = gridView_OWN.CurrentRow.Cells("CHQDATE").Value
                dtpGridRealise.Value = gridView_OWN.CurrentRow.Cells("CHQDATE").Value
            Else
                dtpGridRealise.MinDate = gridView_OWN.CurrentRow.Cells("TRANDATE").Value
                dtpGridRealise.Value = gridView_OWN.CurrentRow.Cells("TRANDATE").Value
            End If
            dtpGridRealise.Location = pt
            dtpGridFlag = True
            dtpGridRealise.Focus()
        ElseIf UCase(e.KeyChar) = Chr(Keys.C) Then
            Dim pt As Point = gridView_OWN.Location
            dtpGridRealise.Visible = True
            pt = pt + gridView_OWN.GetCellDisplayRectangle(gridView_OWN.Columns("RELIASEDATE").Index, gridView_OWN.CurrentRow.Index, False).Location
            dtpGridRealise.MinDate = System.DateTime.Now.ToString()
            dtpGridRealise.Value = System.DateTime.Now.ToString()
            dtpGridRealise.Location = pt
            dtpGridFlag = True
            dtpGridRealise.Focus()
        End If
    End Sub

    Private Sub gridView_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView_OWN.LostFocus
        lblStatus.Visible = False
        lblStatus1.Visible = False
        lblhint.Visible = False
    End Sub

    Private Sub UpdateRealise()
        Try
            Dim BrsSno As String = ""
            tran = Nothing
            tran = cn.BeginTransaction
            With gridView_OWN.CurrentRow
                If .Cells("BRSSNO").Value.ToString = "" Then
                    BrsSno = GetNewSno(TranSnoType.BRSINFOCODE, tran, , strCompanyId, .Cells("DB").Value.ToString)
                    Dim DtBrsInfo As New DataTable
                    Dim Row As DataRow = Nothing
                    DtBrsInfo = BrighttechPack.GlobalMethods.GetTableStructure(.Cells("DB").Value.ToString, "BRSINFO", cn, tran)
                    Row = DtBrsInfo.NewRow
                    Row.Item("SNO") = BrsSno
                    Row.Item("TRANDATE") = .Cells("TRANDATE").Value
                    Row.Item("AMOUNT") = Val(.Cells("AMOUNT").Value.ToString)
                    Row.Item("ACCODE") = .Cells("ACCODE").Value.ToString
                    Row.Item("BATCHNO") = .Cells("BATCHNO").Value.ToString
                    Row.Item("ENTREFNO") = .Cells("ENTREFNO").Value.ToString
                    Row.Item("CHQCARDNO") = .Cells("CHQCARDNO").Value.ToString
                    Row.Item("CARDID") = .Cells("CARDID").Value
                    Row.Item("CHQCARDREF") = .Cells("CHQCARDREF").Value
                    Row.Item("CHQDATE") = .Cells("CHQDATE").Value
                    Row.Item("REALIZEDATE") = dtpGridRealise.Value.Date.ToString("yyyy-MM-dd")
                    Row.Item("COSTID") = .Cells("COSTID").Value
                    Row.Item("COMPANYID") = strCompanyId
                    DtBrsInfo.Rows.Add(Row)
                    InsertData(SyncMode.Transaction, DtBrsInfo, cn, tran, , , , , , False)
                ElseIf .Cells("BRSSNO").Value.ToString <> "" Then
                    BrsSno = .Cells("BRSSNO").Value.ToString
                    strSql = " UPDATE " & .Cells("DB").Value & "..BRSINFO SET REALIZEDATE = '" & dtpGridRealise.Value.Date.ToString("yyyy-MM-dd") & "'"
                    strSql += " WHERE SNO = '" & BrsSno & "'"
                    ExecQuery(SyncMode.Transaction, strSql, cn, tran, .Cells("COSTID").Value.ToString)
                    'cmd = New OleDbCommand(strSql, cn, tran)
                    'cmd.ExecuteNonQuery()
                End If
            End With

            If gridView_OWN.CurrentRow.Cells("BRSACC").Value.ToString = "B" Then
                strSql = " UPDATE " & gridView_OWN.CurrentRow.Cells("DB").Value & "..BRS_ACCTRAN SET RELIASEDATE = '" & dtpGridRealise.Value.Date.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + "  WHERE TRANDATE = '" & gridView_OWN.CurrentRow.Cells("TRANDATE").Value & "'"
                strSql += vbCrLf + "  AND BATCHNO = '" & gridView_OWN.CurrentRow.Cells("BATCHNO").Value & "'"
                strSql += vbCrLf + "  AND isnull(ENTREFNO,'') = '" & gridView_OWN.CurrentRow.Cells("ENTREFNO").Value & "'"
                strSql += vbCrLf + "  AND SNO = '" & gridView_OWN.CurrentRow.Cells("SNO").Value & "'"
                strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
            Else
                strSql = " UPDATE " & gridView_OWN.CurrentRow.Cells("DB").Value & "..ACCTRAN SET RELIASEDATE = '" & dtpGridRealise.Value.Date.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + "  WHERE TRANDATE = '" & gridView_OWN.CurrentRow.Cells("TRANDATE").Value & "'"
                strSql += vbCrLf + "  AND BATCHNO = '" & gridView_OWN.CurrentRow.Cells("BATCHNO").Value & "'"
                strSql += vbCrLf + "  AND isnull(ENTREFNO,'') = '" & gridView_OWN.CurrentRow.Cells("ENTREFNO").Value & "'"
                strSql += vbCrLf + "  AND SNO = '" & gridView_OWN.CurrentRow.Cells("SNO").Value & "'"
                strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
            End If
            ExecQuery(SyncMode.Transaction, strSql, cn, tran, gridView_OWN.CurrentRow.Cells("COSTID").Value.ToString)
            'cmd = New OleDbCommand(strSql, cn, tran)
            'cmd.ExecuteNonQuery()
            tran.Commit()
            tran = Nothing
            gridView_OWN.CurrentRow.Cells("RELIASEDATE").Value = dtpGridRealise.Value.Date
            gridView_OWN.CurrentRow.Cells("BRSSNO").Value = BrsSno
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub UpdateUnRealise()
        Try
            tran = Nothing
            tran = cn.BeginTransaction
            strSql = " DELETE FROM " & gridView_OWN.CurrentRow.Cells("DB").Value & "..BRSINFO"
            strSql += vbCrLf + " WHERE SNO = '" & gridView_OWN.CurrentRow.Cells("BRSSNO").Value.ToString & "'"
            If gridView_OWN.CurrentRow.Cells("BRSACC").Value.ToString = "B" Then
                strSql += vbCrLf + " UPDATE " & gridView_OWN.CurrentRow.Cells("DB").Value & "..BRS_ACCTRAN SET RELIASEDATE = NULL"
                strSql += vbCrLf + "  WHERE TRANDATE = '" & gridView_OWN.CurrentRow.Cells("TRANDATE").Value & "'"
                strSql += vbCrLf + "  AND BATCHNO = '" & gridView_OWN.CurrentRow.Cells("BATCHNO").Value & "'"
                strSql += vbCrLf + "  AND ISNULL(ENTREFNO,'') = '" & gridView_OWN.CurrentRow.Cells("ENTREFNO").Value & "'"
                strSql += vbCrLf + "  AND SNO = '" & gridView_OWN.CurrentRow.Cells("SNO").Value & "'"
                strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
            Else
                strSql += vbCrLf + " UPDATE " & gridView_OWN.CurrentRow.Cells("DB").Value & "..ACCTRAN SET RELIASEDATE = NULL"
                strSql += vbCrLf + "  WHERE TRANDATE = '" & gridView_OWN.CurrentRow.Cells("TRANDATE").Value & "'"
                strSql += vbCrLf + "  AND BATCHNO = '" & gridView_OWN.CurrentRow.Cells("BATCHNO").Value & "'"
                strSql += vbCrLf + "  AND ISNULL(ENTREFNO,'') = '" & gridView_OWN.CurrentRow.Cells("ENTREFNO").Value & "'"
                strSql += vbCrLf + "  AND SNO = '" & gridView_OWN.CurrentRow.Cells("SNO").Value & "'"
                strSql += vbCrLf + "  AND COMPANYID = '" & strCompanyId & "'"
            End If
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()
            gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("RELIASEDATE").Value = DBNull.Value
            gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("BRSSNO").Value = DBNull.Value
            tran.Commit()
            tran = Nothing
        Catch ex As Exception
            If tran IsNot Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub

    Private Sub gridView_OWN_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView_OWN.RowEnter
        lblBankName.Text = ""
        If Not gridView_OWN.Rows.Count > 0 Then Exit Sub
        If gridView_OWN.CurrentRow Is Nothing Then Exit Sub
        Dim str As String = ""
        If gridView_OWN.Rows(e.RowIndex).Cells("CHQCARDREF").Value.ToString <> "" Then
            str += " REF : " & gridView_OWN.Rows(e.RowIndex).Cells("CHQCARDREF").Value.ToString
        End If
        If gridView_OWN.Rows(e.RowIndex).Cells("REMARK1").Value.ToString <> "" Then
            str += " REM1 : " & gridView_OWN.Rows(e.RowIndex).Cells("REMARK1").Value.ToString
        End If
        If gridView_OWN.Rows(e.RowIndex).Cells("REMARK2").Value.ToString <> "" Then
            str += " REM2 : " & gridView_OWN.Rows(e.RowIndex).Cells("REMARK2").Value.ToString
        End If
        If gridView_OWN.Rows(e.RowIndex).Cells("RELIASEDATE").Value.ToString <> "" Then
            Dim sql As String = "Update " & cnStockDb & "..AccTran set RELIASEDATE='" & gridView_OWN.Rows(e.RowIndex).Cells("RELIASEDATE").Value.ToString & "'"
            sql += " Where BatchNo='" & gridView_OWN.Rows(e.RowIndex).Cells("BATCHNO").Value.ToString & "'"
        End If
        lblBankName.Text = str
    End Sub

    Private Sub gridView_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gridView_OWN.SelectionChanged
        If Not gridView_OWN.RowCount > 0 Then Exit Sub
        If gridView_OWN.CurrentRow.Cells("RELIASEDATE").Value.ToString <> "" Then
            lblStatus1.Visible = False
            lblStatus.Text = "Press [Del] Key to Un Realize"
        Else
            lblStatus1.Visible = True
            lblStatus.Text = "Press [E] to Set Trandate as Realize Date "
            lblStatus1.Text = "Press [C] to Set System Date as Realize Date "
        End If
        'If gridView_OWN.Columns(gridView_OWN.CurrentCell.ColumnIndex).Name <> "RELIASEDATE" Then
        '    gridView_OWN.CurrentCell = gridView_OWN.CurrentRow.Cells("RELIASEDATE")
        'End If
    End Sub

    Private Sub dtpGridRealise_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpGridRealise.GotFocus
        lblStatus.Visible = True
        lblStatus1.Visible = True
        lblhint.Visible = True
    End Sub

    Private Sub dtpGridRealise_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtpGridRealise.KeyDown
        If e.KeyCode = Keys.Escape Then
            dtpGridRealise.Visible = False
            dtpGridFlag = False
            gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("RELIASEDATE")
            gridView_OWN.Select()
        End If
    End Sub

    Private Sub dtpGridRealise_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtpGridRealise.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            dtpGridRealise.Visible = False
            dtpGridFlag = False
            gridView_OWN.CurrentCell = gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("RELIASEDATE")
            If dtpGridRealise.Value.Date < CType(gridView_OWN.Rows(gridView_OWN.CurrentRow.Index).Cells("TRANDATE").Value, Date) Then
                MsgBox("Invalid Date" + vbCrLf + "Realise date must greater than trandate", MsgBoxStyle.Information)
                dtpAsOnDate.Select()
                Exit Sub
            End If
            UpdateRealise()
            gridView_OWN.Select()
        ElseIf UCase(e.KeyChar) = Chr(Keys.E) Or UCase(e.KeyChar) = Chr(Keys.C) Then
            gridView_KeyPress(dtpGridRealise, e)
        End If
    End Sub

    Private Sub dtpGridRealise_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpGridRealise.LostFocus
        lblStatus.Visible = False
        lblStatus1.Visible = False
        If dtpGridFlag Then
            dtpGridRealise.Select()
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub rbtBank_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtBank.CheckedChanged
        LoadAcname()
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView_OWN.Rows.Count > 0 Then
            Dim Title As String = cmbBank_MAN.Text + vbCrLf
            If rbtUnRealised.Checked Then
                ''Title += "Un-Realised Report as on Date : " & dtpAsOnDate.Text
                If ChkAson.Checked Then
                    Title += "Un-Realised Report as on Date : " & dtpAsOnDate.Text
                Else
                    Title += "Un-Realised Report as on UnRealiseDate from " & dtpAsOnDate.Text & " and " & dtpUnrealizeDateTo.Text
                End If
            Else
                Title += "Realised Report TranDate from " & dtpTranDateFrom.Text & " and " & dtpTranDateTo.Text & " RealiseDate from " & dtpRealiseDateFrom.Text & " and " & dtpRealiseDateTo.Text
            End If
            BrightPosting.GExport.Post(Me.Name, strCompanyName, Title, gridView_OWN, BrightPosting.GExport.GExportType.Export)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        'Me.Cursor = Cursors.WaitCursor
        If gridView_OWN.Rows.Count > 0 Then
            Dim Title As String = cmbBank_MAN.Text + vbCrLf
            If rbtUnRealised.Checked Then
                ''Title += "Un-Realised Report as on Date : " & dtpAsOnDate.Text
                If ChkAson.Checked Then
                    Title += "Un-Realised Report as on Date : " & dtpAsOnDate.Text
                Else
                    Title += "Un-Realised Report as on UnRealiseDate from " & dtpAsOnDate.Text & " and " & dtpUnrealizeDateTo.Text
                End If
            Else
                Title += "Realised Report TranDate from " & dtpTranDateFrom.Text & " and " & dtpTranDateTo.Text & " RealiseDate from " & dtpRealiseDateFrom.Text & " and " & dtpRealiseDateTo.Text
            End If
            BrightPosting.GExport.Post(Me.Name, strCompanyName, Title, gridView_OWN, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Private Sub ChkAson_CheckedChanged(sender As Object, e As EventArgs) Handles ChkAson.CheckedChanged
        If ChkAson.Checked Then
            LblUnrealizeDateTo.Visible = False
            dtpUnrealizeDateTo.Visible = False
            ChkAson.Text = "AsOn"
        Else
            LblUnrealizeDateTo.Visible = True
            dtpUnrealizeDateTo.Visible = True
            ChkAson.Text = "From"
        End If
    End Sub

    Private Sub chkrupdate_CheckedChanged(sender As Object, e As EventArgs) Handles chkrupdate.CheckedChanged
        If chkrupdate.Checked = True Then
            dtpGridRealiseBulk.Visible = True
        Else
            dtpGridRealiseBulk.Visible = False
        End If
    End Sub

    Private Sub dtpGridRealiseBulk_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dtpGridRealiseBulk.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            If MessageBox.Show("Sure you want to Realize All ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                For i As Integer = 0 To gridView_OWN.RowCount - 1
                    If gridView_OWN.Rows(i).Cells("RELIASEDATE").Value.ToString = "" Then
                        If gridView_OWN.Rows(i).Cells("COSTID").Value.ToString <> "" Then
                            Dim tempdate As Date
                            If gridView_OWN.CurrentRow.Cells("CHQDATE").Value.ToString <> "" Then
                                tempdate = Date.Parse(dtpGridRealiseBulk.Text, ukculinfo.DateTimeFormat)
                            Else
                                tempdate = Date.Parse(dtpGridRealiseBulk.Text, ukculinfo.DateTimeFormat)
                            End If
                            gridView_OWN.Rows(i).Cells("RELIASEDATE").Value = tempdate
                            If gridView_OWN.Rows(i).Cells("RELIASEDATE").Value.ToString <> "" Then
                                dtpGridRealise.Value = tempdate
                                gridView_OWN.CurrentCell = gridView_OWN.Rows(i).Cells("RELIASEDATE")
                                If dtpGridRealise.Value.Date < CType(gridView_OWN.Rows(i).Cells("TRANDATE").Value, Date) Then
                                    MsgBox("Invalid Date" + vbCrLf + "Realise date must greater than trandate", MsgBoxStyle.Information)
                                    dtpAsOnDate.Select()
                                    Exit Sub
                                End If
                                UpdateRealise()
                            End If
                        End If
                    End If
                Next
            End If
        End If
    End Sub
End Class