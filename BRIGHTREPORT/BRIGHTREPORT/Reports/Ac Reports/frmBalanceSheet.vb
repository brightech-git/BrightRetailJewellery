Imports System.Data.OleDb
Public Class frmBalanceSheet
    Dim strSql As String = ""
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dt As DataTable
    Dim dtCostCentre As DataTable
    Dim dtCompany As DataTable
    Dim MultiCostcentre As Boolean = IIf(GetAdmindbSoftValue("CCWISE_FINRPT", "N", ) = "Y", True, False)
    Dim Dtcostid As DataTable
    Dim Rempty As Boolean = False
    Dim Closin_stk As String = GetAdmindbSoftValue("CLSSTK_POST_PL", "L", )
    Dim BLSHT_CLSTK_CONTRA As String = IIf(GetAdmindbSoftValue("BLSHT_CLSTK_CONTRA", "Y", ) = "Y", True, False)
    Dim TRPLFROM_CATETRPL As Boolean = IIf(GetAdmindbSoftValue("RPT_BSPL_CATEPL", "N", ) = "Y", True, False)

    Private Sub frmBalanceSheet_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmBalanceSheet_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        dtpDate.Value = DateTime.Today.Date
        ''COSTCENTRE
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False

        strSql = " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N' "
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(cmbCompany, dtCompany, "COMPANYNAME", True)
        btnView.Focus()

        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub funcView()
        strSql = " SELECT COMPANYID  FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & cmbCompany.Text & "'"
        Dim strCompId As String = BrighttechPack.GetSqlValue(cn, strSql, "COMPANYID", "")
        Dim strCostId As String = ""
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            strCostId += " AND COSTID IN"
            strCostId += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If

        strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "BALSHEET1')"
        strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET1"
        strSql += vbCrLf + "  CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET1"
        strSql += vbCrLf + "  (COMPANYID VARCHAR(3),DISPORDER INT,ACID VARCHAR(25),TRANMODE VARCHAR(1),AMOUNT NUMERIC(15,2),DEBIT NUMERIC(15,2),CREDIT NUMERIC(15,2)"
        strSql += vbCrLf + "  ,TRANDATE SMALLDATETIME,PAYMODE VARCHAR(20),TRANNO VARCHAR(25),SEP VARCHAR(1))"
        strSql += vbCrLf + "  "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET1"
        strSql += vbCrLf + "  (COMPANYID,DISPORDER,ACID,TRANMODE,AMOUNT,DEBIT,CREDIT,TRANDATE,PAYMODE,TRANNO,SEP)"
        strSql += vbCrLf + "  SELECT "
        strSql += vbCrLf + "   T.COMPANYID,G.DISPORDER ,T.ACCODE AS ACID,CASE WHEN ISNULL(DEBIT,0) > ISNULL(CREDIT,0) THEN 'D' ELSE 'C' END TRANMODE"
        strSql += vbCrLf + "  ,CASE WHEN ISNULL(DEBIT,0) <> 0 THEN DEBIT ELSE CREDIT END AMOUNT"
        strSql += vbCrLf + "  ,DEBIT,CREDIT,NULL TRANDATE,'' PAYMODE,NULL TRANNO,'O' SEP"
        strSql += vbCrLf + "  FROM  " & cnStockDb & "..OPENTRAILBALANCE T "
        strSql += vbCrLf + "  INNER JOIN  " & cnAdminDb & "..ACHEAD AS A"
        strSql += vbCrLf + "  ON T.ACCODE = A.ACCODE INNER JOIN " & cnAdminDb & "..ACGROUP AS G ON G.ACGRPCODE = A.ACGRPCODE"
        strSql += vbCrLf + "  WHERE ISNULL(T.COMPANYID,'') = '" & strCompId & "'"
        strSql += Replace(strCostId, "AND COSTID", "AND T.COSTID")
        If TRPLFROM_CATETRPL Then strSql += " and isnull(grpledger,'') NOT IN ('P','T')"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT "
        strSql += vbCrLf + "   T.COMPANYID,G.DISPORDER,T.ACCODE ACID,T.TRANMODE,T.AMOUNT"
        strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'D' THEN AMOUNT END DEBIT,CASE WHEN TRANMODE = 'C' THEN AMOUNT END CREDIT"
        strSql += vbCrLf + "  ,TRANDATE,'' PAYMODE,TRANNO,'T' SEP"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ACCTRAN T"
        strSql += vbCrLf + "  INNER JOIN  " & cnAdminDb & "..ACHEAD AS A"
        strSql += vbCrLf + "  ON T.ACCODE = A.ACCODE INNER JOIN " & cnAdminDb & "..ACGROUP AS G ON G.ACGRPCODE = A.ACGRPCODE"
        strSql += vbCrLf + "  WHERE ISNULL(T.COMPANYID,'') = '" & strCompId & "' AND TRANDATE >= '" & cnTranFromDate & "' AND TRANDATE <= '" & dtpDate.Value.Date.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') != 'Y'"
        strSql += Replace(strCostId, "AND COSTID", "AND T.COSTID")
        If TRPLFROM_CATETRPL Then strSql += " and isnull(grpledger,'') NOT IN ('P','T')"


        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "    IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "BALSHEET2')"
        strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET2"
        strSql += vbCrLf + "  SELECT "
        strSql += vbCrLf + "   A.ACCODE ACID,G.ACGRPCODE, ACGRPNAME,ACMAINCODE,GRPLEDGER,CASE WHEN ISNULL(GRPTYPE,'') = 'L' THEN 'L' ELSE 'A' END GRPTYPE"
        strSql += vbCrLf + "  ,DEBIT,CREDIT"
        strSql += vbCrLf + "  ,ACNAME ACNAME,TRANDATE,TRANMODE,AMOUNT,PAYMODE,TRANNO"
        strSql += vbCrLf + "  ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE = G.ACMAINCODE) ACMAINGRPNAME"
        strSql += vbCrLf + "  ,(SELECT DISPORDER FROM  " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE = G.ACGRPCODE) DISPORD"
        strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET2"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET1 AS T INNER JOIN  " & cnAdminDb & "..ACHEAD AS A"
        strSql += vbCrLf + "  ON T.ACID = A.ACCODE LEFT OUTER JOIN  " & cnAdminDb & "..ACGROUP AS G ON G.ACGRPCODE = A.ACGRPCODE"
        strSql += vbCrLf + "  WHERE ISNULL(GRPLEDGER,'') NOT IN ('P','T')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        Dim netprofit As Double
        Dim netexpenses As Double
        If Not TRPLFROM_CATETRPL Then
            strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "BALSHEET0')"
            strSql += vbCrLf + "   DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET0"
            strSql += vbCrLf + "  SELECT 0 AS DISPORDER,"
            strSql += vbCrLf + "  'PROFIT & LOSS ACCOUNT' PARTICULARS"
            strSql += vbCrLf + "  ,ISNULL(SUM(ISNULL((CASE WHEN GRPTYPE = 'E' THEN ISNULL(CREDIT,0) - ISNULL(DEBIT,0) END),0)"
            strSql += vbCrLf + "  - ISNULL((CASE WHEN GRPTYPE != 'E' THEN ISNULL(DEBIT,0) - ISNULL(CREDIT,0) END),0)),0) + ISNULL((SELECT SUM(VALUE) FROM " & cnStockDb & "..CLOSINGVAL WHERE COMPANYID = '" & strCompId & "'" & strCostId & "),0) AMOUNT"
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET0"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET1 AS T INNER JOIN  " & cnAdminDb & "..ACHEAD AS A"
            strSql += vbCrLf + "  ON T.ACID = A.ACCODE INNER JOIN " & cnAdminDb & "..ACGROUP AS G ON G.ACGRPCODE = A.ACGRPCODE"
            strSql += vbCrLf + "  AND ISNULL(GRPLEDGER,'') IN ('P','T')"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        Else
            Dim chkCostName As String = GetSelectedCostId(chkCmbCostCentre, False)
            strSql = " EXEC " & cnAdminDb & "..SP_RPT_TRANDING_NEW"
            strSql += vbCrLf + "@ADMINDB ='" & cnAdminDb.ToString & "'"
            strSql += vbCrLf + ",@DBNAME ='" & cnStockDb.ToString & "'"
            strSql += vbCrLf + ",@CNDATE ='" & Format(cnTranFromDate, "yyyy-MM-dd") & "'"
            strSql += vbCrLf + ",@ASONDATE ='" & Format(dtpDate.Value, "yyyy-MM-dd") & "' "
            strSql += vbCrLf + ",@TEMPTABLE='TEMPTABLEDB..TEMP" & systemId & "TRPL'"
            strSql += vbCrLf + ",@COMPANYID='" & strCompId & "'"
            strSql += vbCrLf + ",@COSTIDS='" & IIf(chkCostName = "", "ALL", chkCostName) & "'"
            strSql += vbCrLf + ",@SYSTEMID='" & systemId & "'"
            strSql += vbCrLf + ",@PL='Y'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            Dim da As New OleDbDataAdapter(cmd)
            Dim dss As New DataSet
            da.Fill(dss)
            Dim dtpl As New DataTable
            dtpl = dss.Tables(1)
            Dim npfilter As String = "PARTICULAR='NET PROFIT' AND COLHEAD = 'S'"
            'Dim Profitrow As DataRow = dtpl.Select(npfilter, Nothing)

            netprofit = Val(dtpl.Compute("SUM(INCOME)", npfilter).ToString)
            netexpenses = Val(dtpl.Compute("SUM(EXPENSES)", npfilter).ToString)


        End If

        Dim StrNew As String
        Dim Startdate As Date
        StrNew = "SELECT STARTDATE FROM " & cnAdminDb & "..DBMASTER WHERE '" & Format(dtpDate.Value, "yyyy-MM-dd") & "' BETWEEN STARTDATE AND ENDDATE "
        Startdate = GetSqlValue(cn, StrNew)
        'For Colsing Stock By vasanth
        If Closin_stk = "N" Then
            strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "BALSHEET10')"
            strSql += vbCrLf + "   DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET10"
            strSql += vbCrLf + "  SELECT 1 AS DISPORDER,"
            strSql += vbCrLf + "  CONVERT(VARCHAR(50),'CLOSING STOCK') PARTICULARS"
            strSql += vbCrLf + "  ,SUM(VALUE) AS AMOUNT"
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET10"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..CLOSINGVAL WHERE COMPANYID = '" & strCompId & "'" & strCostId & " "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        Else
            Dim Qry As String = ""
            'ProgressBarShow()
            If Closin_stk = "W" Then
                'ProgressBarStep("Closing Stock arrival Weighted avg method", 10)
                Qry = " EXEC " & cnAdminDb & "..SP_CLOSING_STOCK"
            Else
                'ProgressBarStep("Closing Stock arrival LIFO method", 10)
                Qry = " EXEC " & cnAdminDb & "..SP_CLOSINGSTOCK_LIFO"
            End If
            Qry += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
            Qry += vbCrLf + " ,@ADMINDB = '" & cnAdminDb & "'"
            Qry += vbCrLf + " ,@FRMDATE = '" & Format(Startdate, "yyyy-MM-dd") & "'"
            Qry += vbCrLf + " ,@TODATE = '" & Format(dtpDate.Value, "yyyy-MM-dd") & "'"
            Qry += vbCrLf + " ,@METALNAME = 'ALL'"
            Qry += vbCrLf + " ,@CATCODE = 'ALL'"
            Qry += vbCrLf + " ,@CATNAME = 'ALL'"
            Qry += vbCrLf + " ,@COSTNAME = '" & GetQryString(chkCmbCostCentre.Text).Replace("'", "") & "'"
            Qry += vbCrLf + " ,@COMPANYID = '" & strCompId & "'"
            Qry += vbCrLf + " ,@RPTTYPE = 'S'"
            Dim SYSID As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
            Qry += vbCrLf + " ,@SYSTEMID = '" & SYSID & "'"
            cmd = New OleDbCommand(Qry, cn) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

            strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "BALSHEET10')"
            strSql += vbCrLf + "   DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET10"
            strSql += vbCrLf + "  SELECT 1 AS DISPORDER,"
            strSql += vbCrLf + "  CONVERT(VARCHAR(50),'CLOSING STOCK') PARTICULARS"
            strSql += vbCrLf + "  ,SUM(CAMOUNT) AS AMOUNT"
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET10"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPCSTKVALUE4" & SYSID & " A "
            strSql += " WHERE A.RESULT=3 AND COLHEAD='G'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()
        End If
        If TRPLFROM_CATETRPL Then
            strSql = vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET10(DISPORDER,PARTICULARS,AMOUNT)"
            strSql += "  SELECT 0 AS DISPORDER,"
            strSql += vbCrLf + "  'PROFIT & LOSS ACCOUNT' "
            strSql += vbCrLf + "  ," & netprofit & " AMOUNT"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "BALSHEET0')"
            strSql += vbCrLf + "   DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET0"
            strSql += "  SELECT 0 AS DISPORDER,"
            strSql += vbCrLf + "  'PROFIT & LOSS ACCOUNT' PARTICULARS"
            strSql += vbCrLf + "  ," & netexpenses & " AMOUNT"
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET0"

            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "BALSHEET3')"
        strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET3"
        strSql += vbCrLf + "  CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET3"
        strSql += vbCrLf + "  ("
        strSql += vbCrLf + "   LIADISPORDER INT,LIAACGROUPNAME1 VARCHAR(250),LIABILITY1 VARCHAR(50),LIABILITY VARCHAR(50)"
        strSql += vbCrLf + "  ,ASSACGROUPNAME1 VARCHAR(250),ASSET1 VARCHAR(50),ASSET VARCHAR(50)"
        strSql += vbCrLf + "  ,RESULT INT,SEP INT,LIACMAINCODE VARCHAR(50),ASSACMAINCODE VARCHAR(50),LIAACGROUP VARCHAR(250),ASSACGROUP VARCHAR(250)"
        strSql += vbCrLf + "  ,LIAACGROUPNAME VARCHAR(250),LIAACMAINGRPNAME VARCHAR(250)"
        strSql += vbCrLf + "  ,ASSDISPORDER INT,ASSACGROUPNAME VARCHAR(250),ASSACMAINGRPNAME VARCHAR(250),SNO INT IDENTITY(1,1),COLHEAD VARCHAR(2)"
        strSql += vbCrLf + "  )"
        strSql += vbCrLf + "  "
        strSql += vbCrLf + "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "BALSHEET4')"
        strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET4"
        strSql += vbCrLf + "  CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET4"
        strSql += vbCrLf + "  ("
        strSql += vbCrLf + "   LIADISPORDER INT,LIAACGROUPNAME1 VARCHAR(250),LIABILITY1 VARCHAR(50),LIABILITY VARCHAR(50)"
        strSql += vbCrLf + "  ,ASSACGROUPNAME1 VARCHAR(250),ASSET1 VARCHAR(50),ASSET VARCHAR(50)"
        strSql += vbCrLf + "  ,RESULT INT,SEP INT,LIAACMAINCODE VARCHAR(50),ASSACMAINCODE VARCHAR(50),LIAACGROUP VARCHAR(250),ASSACGROUP VARCHAR(250)"
        strSql += vbCrLf + "  ,LIAACGROUPNAME VARCHAR(250),LIAACMAINGRPNAME VARCHAR(250)"
        strSql += vbCrLf + "  ,ASSDISPORDER INT,ASSACGROUPNAME VARCHAR(250),ASSACMAINGRPNAME VARCHAR(250),SNO INT IDENTITY(1,1),COLHEAD VARCHAR(2)"
        strSql += vbCrLf + "  )"
        strSql += vbCrLf + "  "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET3"
        strSql += vbCrLf + "  ("
        strSql += vbCrLf + "   LIADISPORDER,LIAACGROUPNAME1,LIABILITY1,LIABILITY,ASSACGROUPNAME1,ASSET1,ASSET"
        strSql += vbCrLf + "  ,RESULT,SEP,LIACMAINCODE,ASSACMAINCODE,LIAACGROUP,ASSACGROUP,LIAACGROUPNAME,LIAACMAINGRPNAME"
        strSql += vbCrLf + "  ,ASSACGROUPNAME,ASSACMAINGRPNAME,COLHEAD"
        strSql += vbCrLf + "  )"
        'strSql += vbCrLf + "  SELECT DISTINCT DISPORD LIADISPORDER,ACMAINGRPNAME LIAACGROUPNAME1"
        'strSql += vbCrLf + "  ,NULL LIABILITY1,NULL LIABILITY,NULL ASSACGROUPNAME1"
        'strSql += vbCrLf + "  ,NULL ASSET1,NULL ASSET,'1' RESULT,'1' SEP"
        'strSql += vbCrLf + "  ,ACMAINCODE LIAACMAINCODE,'' ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP"
        'strSql += vbCrLf + "  ,'' LIAACGROUPNAME,ACMAINGRPNAME LIAACMAINGRPNAME"
        'strSql += vbCrLf + "  ,''ASSACGROUPNAME,'' ASSACMAINGRPNAME,'T' COLHEAD"
        'strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET2 AS T WHERE ISNULL(GRPTYPE,'') = 'L' --AND ACGRPCODE=ACMAINCODE "
        'strSql += vbCrLf + "  GROUP BY DISPORD,ACMAINGRPNAME,ACMAINCODE"
        'strSql += vbCrLf + "  HAVING (SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0)))>0"
        'strSql += vbCrLf + "  UNION ALL"
        'strSql += vbCrLf + "  SELECT DISPORDER LIADISPORDER,PARTICULARS LIAACGROUPNAME1,NULL LIABILITY1,NULL LIABILITY"
        'strSql += vbCrLf + "  ,NULL ASSACGROUPNAME1,NULL ASSET1,NULL ASSET,'2' RESULT,'1' SEP"
        'strSql += vbCrLf + "  ,NULL LIAACMAINCODE,'' ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP"
        'strSql += vbCrLf + "  ,'' LIAACGROUPNAME,PARTICULARS LIAACMAINGRPNAME,'' ASSACGROUPNAME,'' ASSACMAINGRPNAME,'T' COLHEAD"
        'strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET0 WHERE ISNULL(AMOUNT,0)>0"
        'strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT DISPORDER LIADISPORDER,"
        strSql += vbCrLf + "  PARTICULARS LIAACGROUPNAME1,NULL LIABILITY1,CONVERT(VARCHAR,AMOUNT) LIABILITY"
        strSql += vbCrLf + "  ,NULL ASSACGROUPNAME1,NULL ASSET1,NULL ASSET,'3' RESULT,'1' SEP"
        strSql += vbCrLf + "  ,NULL LIAACMAINCODE,'' ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP"
        strSql += vbCrLf + "  ,'' LIAACGROUPNAME,PARTICULARS LIAACMAINGRPNAME,'' ASSACGROUPNAME,'' ASSACMAINGRPNAME,'T' COLHEAD"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET0 WHERE ISNULL(AMOUNT,0)>0"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT "
        strSql += vbCrLf + "  DISTINCT DISPORD LIADISPORDER,ACMAINGRPNAME LIAACGROUPNAME1"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR,ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) LIABILITY1,NULL LIABILITY"
        strSql += vbCrLf + "  ,NULL ASSACGROUPNAME1,NULL ASSET1,NULL ASSET,'4' RESULT,'2' SEP"
        strSql += vbCrLf + "  ,ACMAINCODE LIAACMAINCODE,'' ASSACMAINCODE,ACGRPCODE LIAACGROUP,'' ASSACGROUP"
        strSql += vbCrLf + "  ,ACGRPNAME LIAACGROUPNAME,ACMAINGRPNAME LIAACMAINGRPNAME"
        strSql += vbCrLf + "  ,'' ASSACGROUPNAME,'' ASSACMAINGRPNAME,'T' COLHEAD"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET2 WHERE ISNULL(GRPTYPE,'') = 'L' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')"
        strSql += vbCrLf + "  GROUP BY DISPORD,ACMAINGRPNAME,ACMAINCODE,ACGRPNAME,ACGRPCODE"
        strSql += vbCrLf + "  HAVING (SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0)))>0"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT "
        strSql += vbCrLf + "  0 LIADISPORDER,ACMAINGRPNAME LIAACGROUPNAME1,NULL LIABILITY1"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR,ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) LIABILITY"
        strSql += vbCrLf + "  ,NULL ASSACGROUPNAME1,NULL ASSET1,NULL ASSET,'5' RESULT,'3' SEP"
        strSql += vbCrLf + "  ,ACMAINCODE LIAACMAINCODE,'' ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP"
        strSql += vbCrLf + "  ,'' LIAACGROUPNAME,ACMAINGRPNAME LIAACMAINGRPNAME"
        strSql += vbCrLf + "  ,'' ASSACGROUPNAME,'' ASSACMAINGRPNAME,'T' COLHEAD"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET2 WHERE ISNULL(GRPTYPE,'') = 'L'"
        strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACMAINCODE"
        strSql += vbCrLf + "  HAVING (SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0)))>0"
        'For Asset negative value
        'strSql += vbCrLf + "  UNION ALL "
        'strSql += vbCrLf + "  SELECT DISTINCT DISPORD LIADISPORDER,ACMAINGRPNAME LIAACGROUPNAME1"
        'strSql += vbCrLf + "  ,NULL LIABILITY1,NULL LIABILITY"
        'strSql += vbCrLf + "  ,NULL  ASSACGROUPNAME1 ,NULL ASSET1,NULL ASSET,'1' RESULT,'1' SEP"
        'strSql += vbCrLf + "  ,ACMAINCODE LIAACMAINCODE,NULL ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP"
        'strSql += vbCrLf + "  ,'' LIAACGROUPNAME,ACMAINGRPNAME LIAACMAINGRPNAME"
        'strSql += vbCrLf + "  ,''ASSACGROUPNAME,'' ASSACMAINGRPNAME,'T' COLHEAD"
        'strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET2 WHERE ISNULL(GRPTYPE,'') = 'A' "
        'strSql += vbCrLf + "  GROUP BY DISPORD,ACMAINGRPNAME,ACMAINCODE"
        'strSql += vbCrLf + "  HAVING (SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0)))<0"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT DISTINCT DISPORD LIADISPORDER, ACMAINGRPNAME LIAACGROUPNAME1"
        strSql += vbCrLf + "  ,NULL LIABILITY1 ,CONVERT(VARCHAR, ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0))  LIABILITY"
        strSql += vbCrLf + "  ,NULL ASSACGROUPNAME1 ,NULL ASSET1,NULL ASSET,'5' RESULT,'3' SEP"
        strSql += vbCrLf + "  ,ACMAINCODE LIAACMAINCODE,'' ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP"
        strSql += vbCrLf + "  ,'' LIAACGROUPNAME,ACMAINGRPNAME LIAACMAINGRPNAME"
        strSql += vbCrLf + "  ,'' ASSACGROUPNAME,'' ASSACMAINGRPNAME,'T' COLHEAD"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET2 WHERE ISNULL(GRPTYPE,'') = 'A'"
        strSql += vbCrLf + "  GROUP BY DISPORD,ACMAINGRPNAME,ACMAINCODE,DISPORD"
        strSql += vbCrLf + "  HAVING (SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0)))<0"
        'For Colsing Stock By vasanth
        'strSql += vbCrLf + "  UNION ALL"
        'strSql += vbCrLf + "  SELECT DISPORDER ASSDISPORDER,PARTICULARS+' CARRY OVER'  LIAACGROUPNAME1,NULL LIABILITY1,NULL LIABILITY"
        'strSql += vbCrLf + "  ,NULL ASSACGROUPNAME1,NULL ASSET1,NULL ASSET,'2' RESULT,'1' SEP"
        'strSql += vbCrLf + "  ,NULL LIAACMAINCODE,'' ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP"
        'strSql += vbCrLf + "  ,'' LIAACGROUPNAME,'Z'+PARTICULARS LIAACMAINGRPNAME,'' ASSACGROUPNAME,''  ASSACMAINGRPNAME,'T1' COLHEAD"
        'strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET10 "
        'If BLSHT_CLSTK_CONTRA Then
        '    strSql += vbCrLf + "  WHERE ISNULL(AMOUNT,0)<>0"
        'Else
        '    strSql += vbCrLf + "  WHERE ISNULL(AMOUNT,0)<0"
        'End If
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT DISPORDER ASSDISPORDER,PARTICULARS+' CARRY OVER' LIAACGROUPNAME1,NULL LIABILITY1,CONVERT(VARCHAR,AMOUNT) LIABILITY"
        strSql += vbCrLf + "  ,NULL ASSACGROUPNAME1,NULL ASSET1,NULL ASSET,'3' RESULT,'1' SEP"
        strSql += vbCrLf + "  ,NULL LIAACMAINCODE,'' ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP"
        strSql += vbCrLf + "  ,'' LIAACGROUPNAME,'Z'+PARTICULARS LIAACMAINGRPNAME,'' ASSACGROUPNAME,'' ASSACMAINGRPNAME,'T' COLHEAD"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET10 "
        If BLSHT_CLSTK_CONTRA Then
            strSql += vbCrLf + "  WHERE ISNULL(AMOUNT,0)<>0"
        Else
            strSql += vbCrLf + "  WHERE ISNULL(AMOUNT,0)<0"
        End If
        strSql += vbCrLf + "  ORDER BY LIAACMAINGRPNAME,SEP,LIADISPORDER,LIAACGROUPNAME,RESULT"
        'strSql += vbCrLf + "  , (SELECT DISPORDER FROM ACGRPCODE WHERE ACMAINCODE = T.ACMAINCODE)";
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "   INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET4"
        strSql += vbCrLf + "  ("
        strSql += vbCrLf + "   LIAACGROUPNAME1,LIABILITY1,LIABILITY,ASSACGROUPNAME1,ASSET1,ASSET"
        strSql += vbCrLf + "  ,RESULT,SEP,LIAACMAINCODE,ASSACMAINCODE,LIAACGROUP,ASSACGROUP,LIAACGROUPNAME,LIAACMAINGRPNAME"
        strSql += vbCrLf + "  ,ASSACGROUPNAME,ASSACMAINGRPNAME,ASSDISPORDER,COLHEAD"
        strSql += vbCrLf + "  )"
        'strSql += vbCrLf + "  SELECT DISTINCT NULL LIAACGROUPNAME1,NULL LIABILITY1,NULL LIABILITY"
        'strSql += vbCrLf + "  ,ACMAINGRPNAME ASSACGROUPNAME1,NULL ASSET1,NULL ASSET,'1' RESULT,'1' SEP"
        'strSql += vbCrLf + "  ,NULL LIAACMAINCODE,ACMAINCODE ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP"
        'strSql += vbCrLf + "  ,'' LIAACGROUPNAME,'' LIAACMAINGRPNAME"
        'strSql += vbCrLf + "  ,''ASSACGROUPNAME,ACMAINGRPNAME ASSACMAINGRPNAME,DISPORD ASSDISPORDER,'T' COLHEAD"
        'strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET2 WHERE ISNULL(GRPTYPE,'') = 'A' AND ACGRPCODE=ACMAINCODE "
        'strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACMAINCODE,DISPORD"
        'strSql += vbCrLf + "  HAVING (SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0)))>0"
        'strSql += vbCrLf + "  UNION ALL"
        'strSql += vbCrLf + "  SELECT NULL LIAACGROUPNAME1,NULL LIABILITY1,NULL LIABILITY"
        'strSql += vbCrLf + "  ,PARTICULARS  ASSACGROUPNAME1,NULL ASSET1,NULL ASSET,'2' RESULT,'1' SEP"
        'strSql += vbCrLf + "  ,NULL LIAACMAINCODE,'' ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP,'' LIAACGROUPNAME,'' LIAACMAINGRPNAME"
        'strSql += vbCrLf + "  ,'' ASSACGROUPNAME,PARTICULARS ASSACMAINGRPNAME,DISPORDER ASSDISPORDER,'T' COLHEAD"
        'strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET0 WHERE ISNULL(AMOUNT,0)<0"
        'strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT NULL LIAACGROUPNAME1,NULL LIABILITY1,NULL LIABILITY"
        strSql += vbCrLf + "  ,PARTICULARS ASSACGROUPNAME1,NULL ASSET1,CONVERT(VARCHAR,(AMOUNT)*(-1)) ASSET,'3' RESULT,'1' SEP"
        strSql += vbCrLf + "  ,NULL LIAACMAINCODE,'' ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP,'' LIAACGROUPNAME,'' LIAACMAINGRPNAME"
        strSql += vbCrLf + "  ,'' ASSACGROUPNAME,PARTICULARS ASSACMAINGRPNAME,DISPORDER LIADISPORDER,'T' COLHEAD"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET0 WHERE ISNULL(AMOUNT,0)<0"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT DISTINCT '' LIAACGROUPNAME1,NULL LIABILITY1,NULL LIABILITY"
        strSql += vbCrLf + "  ,ACMAINGRPNAME ASSACGROUPNAME1"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) ASSET1,NULL ASSET,'4' RESULT,'2' SEP"
        strSql += vbCrLf + "  ,'' LIAACMAINCODE,ACMAINCODE ASSACMAINCODE,'' LIAACGROUP,ACGRPCODE ASSACGROUP"
        strSql += vbCrLf + "  ,'' LIAACGROUPNAME,'' LIAACMAINGRPNAME"
        strSql += vbCrLf + "  ,ACGRPNAME ASSACGROUPNAME,ACMAINGRPNAME ASSACMAINGRPNAME,DISPORD ASSDISPORDER,'T' COLHEAD"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET2 WHERE ISNULL(GRPTYPE,'') = 'A' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')"
        strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACMAINCODE,ACGRPNAME,ACGRPCODE,DISPORD"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT DISTINCT '' LIAACGROUPNAME1,NULL LIABILITY1,NULL LIABILITY,ACMAINGRPNAME ASSACGROUPNAME1"
        strSql += vbCrLf + "  ,NULL ASSET1,CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) ASSET"
        strSql += vbCrLf + "  ,'5' RESULT,'3' SEP,'' LIAACMAINCODE,ACMAINCODE ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP"
        strSql += vbCrLf + "  ,'' LIAACGROUPNAME,'' LIAACMAINGRPNAME"
        strSql += vbCrLf + "  ,'' ASSACGROUPNAME,ACMAINGRPNAME ASSACMAINGRPNAME,0 ASSDISPORDER,'T' COLHEAD"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET2 WHERE ISNULL(GRPTYPE,'') = 'A'"
        strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACMAINCODE"
        strSql += vbCrLf + "  HAVING (SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0)))>0"
        strSql += vbCrLf + "  UNION ALL "
        'For Liability negative value
        strSql += vbCrLf + "  SELECT DISTINCT NULL LIAACGROUPNAME1,NULL LIABILITY1,NULL LIABILITY"
        strSql += vbCrLf + "  ,ACMAINGRPNAME  ASSACGROUPNAME1,NULL ASSET1,NULL ASSET,'1' RESULT,'1' SEP"
        strSql += vbCrLf + "  ,NULL LIAACMAINCODE,ACMAINCODE ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP"
        strSql += vbCrLf + "  ,'' LIAACGROUPNAME,'' LIAACMAINGRPNAME"
        strSql += vbCrLf + "  ,''ASSACGROUPNAME,ACMAINGRPNAME ASSACMAINGRPNAME,DISPORD LIADISPORDER,'T' COLHEAD"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET2 AS T WHERE ISNULL(GRPTYPE,'') = 'L' --AND ACGRPCODE=ACMAINCODE "
        strSql += vbCrLf + "  GROUP BY DISPORD,ACMAINGRPNAME,ACMAINCODE"
        strSql += vbCrLf + "  HAVING (SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0)))<0"
        'strSql += vbCrLf + "  UNION ALL"
        'strSql += vbCrLf + "  SELECT  '' LIAACGROUPNAME1,NULL LIABILITY1,NULL LIABILITY"
        'strSql += vbCrLf + "  ,'  ' + ACGRPNAME ASSACGROUPNAME1,CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) ASSET1,NULL ASSET,'4' RESULT,'2' SEP"
        'strSql += vbCrLf + "  ,'' LIAACMAINCODE,ACMAINCODE ASSACMAINCODE,'' LIAACGROUP,ACGRPCODE ASSACGROUP"
        'strSql += vbCrLf + "  ,'' LIAACGROUPNAME,'' LIAACMAINGRPNAME"
        'strSql += vbCrLf + "  ,ACGRPNAME ASSACGROUPNAME,ACMAINGRPNAME ASSACMAINGRPNAME,DISPORD ASSDISPORDER,'T2' COLHEAD"
        'strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET2 WHERE ISNULL(GRPTYPE,'') = 'L' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')"
        'strSql += vbCrLf + "  GROUP BY DISPORD,ACMAINGRPNAME,ACMAINCODE,ACGRPNAME,ACGRPCODE"
        'strSql += vbCrLf + "  HAVING (SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0)))<0"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT '' LIAACGROUPNAME1,NULL LIABILITY1,NULL LIABILITY"
        strSql += vbCrLf + "  ,ACMAINGRPNAME ASSACGROUPNAME1,NULL ASSET1,CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) ASSET"
        strSql += vbCrLf + "  ,'5' RESULT,'3' SEP,'' LIAACMAINCODE,ACMAINCODE ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP"
        strSql += vbCrLf + "  ,'' LIAACGROUPNAME,'' LIAACMAINGRPNAME"
        strSql += vbCrLf + "  ,'' ASSACGROUPNAME,ACMAINGRPNAME ASSACMAINGRPNAME,0 LIADISPORDER,'T' COLHEAD"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET2 WHERE ISNULL(GRPTYPE,'') = 'L'"
        strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACMAINCODE"
        strSql += vbCrLf + "  HAVING (SUM(ISNULL(CREDIT,0))-SUM(ISNULL(DEBIT,0)))<0"


        'For Colsing Stock By vasanth
        'strSql += vbCrLf + "  UNION ALL"
        'strSql += vbCrLf + "  SELECT NULL LIAACGROUPNAME1,NULL LIABILITY1,NULL LIABILITY"
        'strSql += vbCrLf + "  ,PARTICULARS  ASSACGROUPNAME1,NULL ASSET1,NULL ASSET,'2' RESULT,'1' SEP"
        'strSql += vbCrLf + "  ,NULL LIAACMAINCODE,'' ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP"
        'strSql += vbCrLf + "  ,'' LIAACGROUPNAME,'' LIAACMAINGRPNAME,'' ASSACGROUPNAME,PARTICULARS  ASSACMAINGRPNAME,DISPORDER ASSDISPORDER,'T' COLHEAD"
        'strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET10 "
        'If BLSHT_CLSTK_CONTRA Then
        '    strSql += vbCrLf + "  WHERE ISNULL(AMOUNT,0)<>0"
        'Else
        '    strSql += vbCrLf + "  WHERE ISNULL(AMOUNT,0)>0"
        'End If
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT NULL LIAACGROUPNAME1,NULL LIABILITY1,NULL LIABILITY"
        strSql += vbCrLf + "  ,PARTICULARS ASSACGROUPNAME1,NULL ASSET1,CONVERT(VARCHAR,AMOUNT) ASSET,'3' RESULT,'1' SEP"
        strSql += vbCrLf + "  ,NULL LIAACMAINCODE,'' ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP"
        strSql += vbCrLf + "  ,'' LIAACGROUPNAME,'' LIAACMAINGRPNAME,'' ASSACGROUPNAME,PARTICULARS ASSACMAINGRPNAME,DISPORDER ASSDISPORDER,'T' COLHEAD"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET10 "
        If BLSHT_CLSTK_CONTRA Then
            strSql += vbCrLf + "  WHERE ISNULL(AMOUNT,0)<>0"
        Else
            strSql += vbCrLf + "  WHERE ISNULL(AMOUNT,0)>0"
        End If
        'strSql += vbCrLf + "  ORDER BY ASSACMAINGRPNAME,SEP,ASSDISPORDER,ASSACGROUPNAME,RESULT"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "BALSHEET5')"
        strSql += vbCrLf + "   DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET5"
        strSql += vbCrLf + "  SELECT "
        strSql += vbCrLf + "   T3.LIAACGROUPNAME1,T3.LIABILITY1,T3.LIABILITY,T4.ASSACGROUPNAME1,T4.ASSET1,T4.ASSET"
        strSql += vbCrLf + "  ,T3.SNO SNO3,T4.SNO SNO4,T3.RESULT RESULT3,T4.RESULT RESULT4,T3.SEP SEP3,T4.SEP SEP4,T3.COLHEAD COLHEAD3,T4.COLHEAD COLHEAD4"
        strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET5"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET3 AS T3 FULL JOIN TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET4 AS T4 "
        strSql += vbCrLf + "  ON T3.SNO = T4.SNO "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "  SELECT "
        strSql += vbCrLf + "   LIAACGROUPNAME1"
        strSql += vbCrLf + "  ,CASE WHEN RESULT3 NOT IN (6) AND ISNULL(LIABILITY1,'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(LIABILITY1,'') = '' THEN '0' ELSE LIABILITY1 END),'D','Y') ELSE LIABILITY1 END  LIABILITY1"
        strSql += vbCrLf + "  ,CASE WHEN RESULT3 NOT IN (6) AND ISNULL(LIABILITY,'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(LIABILITY,'') = '' THEN '0' ELSE LIABILITY END),'D','Y') ELSE LIABILITY END LIABILITY"
        strSql += vbCrLf + "  ,ASSACGROUPNAME1"
        strSql += vbCrLf + "  ,CASE WHEN RESULT4 NOT IN (6) AND ISNULL(ASSET1,'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(ASSET1,'') = '' THEN '0' ELSE ASSET1 END),'D','Y') ELSE ASSET1 END  ASSET1"
        strSql += vbCrLf + "  ,CASE WHEN RESULT4 NOT IN (6) AND ISNULL(ASSET,'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(ASSET,'') = '' THEN '0' ELSE ASSET END),'D','Y') ELSE ASSET END ASSET"
        strSql += vbCrLf + "  ,COLHEAD3,COLHEAD4"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET5"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT"
        strSql += vbCrLf + "  'TOTAL =>' LIAACGROUPNAME1 ,NULL LIABILITY1,CONVERT(VARCHAR," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3 IN ('3','5') THEN CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(LIABILITY,'') = '' THEN '0' ELSE LIABILITY END) ELSE '0' END)),'D','Y')) LIABILITY"
        strSql += vbCrLf + "  ,'TOTAL =>' ASSACGROUPNAME1 ,NULL ASSET1,CONVERT(VARCHAR," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4 IN ('3','5')  THEN CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(ASSET,'') = '' THEN '0' ELSE ASSET END) ELSE '0' END)),'D','Y')) ASSET,'G' COLHEAD3,'G' COLHEAD4"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET5"
        da = New OleDbDataAdapter(strSql, cn)
        dt = New DataTable()
        da.Fill(dt)
        gridView.DataSource = dt

        If gridView.RowCount > 0 Then
            lblTitle.Text = cmbCompany.Text & "BALANCE SHEET FOR THE PERIOD OF - 01/04/" & (IIf(dtpDate.Value.Date.Month > 3, dtpDate.Value.Date.Year, dtpDate.Value.Date.Year - 1)).ToString() & " - " & dtpDate.Text.Replace("-", "/")
            If chkCmbCostCentre.Text <> "" Then lblTitle.Text = lblTitle.Text & " COST CENTRE :" & chkCmbCostCentre.Text
            gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            gridView.Font = New Font("VERDANA", 9, FontStyle.Regular)
            gridView.Columns("LIAACGROUPNAME1").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

            gridView.Columns("LIAACGROUPNAME1").HeaderText = "LIABILITIES"
            gridView.Columns("LIAACGROUPNAME1").DefaultCellStyle.Font = New Font("VERDANA", 7, FontStyle.Regular)
            gridView.Columns("LIAACGROUPNAME1").Width = 220
            gridView.Columns("LIABILITY").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns("LIABILITY").HeaderText = "AMOUNT"
            gridView.Columns("LIABILITY").Width = 125
            gridView.Columns("LIABILITY1").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns("LIABILITY1").HeaderText = "AMOUNT"
            gridView.Columns("LIABILITY1").Width = 125
            gridView.Columns("ASSACGROUPNAME1").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            gridView.Columns("ASSACGROUPNAME1").HeaderText = "ASSETS"
            gridView.Columns("ASSACGROUPNAME1").Width = 220
            gridView.Columns("ASSACGROUPNAME1").DefaultCellStyle.Font = New Font("VERDANA", 7, FontStyle.Regular)
            gridView.Columns("ASSET").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns("ASSET").Width = 125
            gridView.Columns("ASSET").HeaderText = "AMOUNT"
            gridView.Columns("ASSET1").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            gridView.Columns("ASSET1").Width = 125
            gridView.Columns("ASSET1").HeaderText = "AMOUNT"
            gridView.Columns("COLHEAD3").Visible = False
            gridView.Columns("COLHEAD4").Visible = False

            For i As Integer = 0 To gridView.RowCount - 1
                If gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "T" OrElse gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "T2" Then
                    gridView.Rows(i).Cells("LIAACGROUPNAME1").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                    gridView.Rows(i).Cells("LIABILITY").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                ElseIf gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "T1" Then
                    gridView.Rows(i).Cells("LIAACGROUPNAME1").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                    gridView.Rows(i).Cells("LIABILITY").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                    gridView.Rows(i).Cells("LIAACGROUPNAME1").Style.ForeColor = Color.Blue
                    gridView.Rows(i).Cells("LIABILITY").Style.ForeColor = Color.Blue
                ElseIf gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "S" OrElse gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "S1" OrElse gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "S2" Then
                    gridView.Rows(i).Cells("LIABILITY").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                    gridView.Rows(i).Cells("LIABILITY1").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                ElseIf gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "G" Then
                    If Convert.ToDouble(IIf(gridView.Rows(i).Cells("LIABILITY").Value.ToString() <> "", gridView.Rows(i).Cells("LIABILITY").Value.ToString(), "0")) <> 0 Then
                        gridView.Rows(i).Cells("LIAACGROUPNAME1").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                        gridView.Rows(i).Cells("LIAACGROUPNAME1").Style.ForeColor = Color.Red
                        gridView.Rows(i).Cells("LIABILITY").Style.ForeColor = Color.Red
                        gridView.Rows(i).Cells("LIABILITY").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                        gridView.Rows(i).Cells("LIABILITY1").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                    End If
                ElseIf gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "G1" OrElse gridView.Rows(i).Cells("COLHEAD3").Value.ToString() = "G2" Then
                    gridView.Rows(i).Cells("LIABILITY").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                    gridView.Rows(i).Cells("LIABILITY1").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                End If

                If gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "T" OrElse gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "T1" OrElse gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "T2" Then
                    gridView.Rows(i).Cells("ASSACGROUPNAME1").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                    gridView.Rows(i).Cells("ASSET").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                ElseIf gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "S" OrElse gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "S1" OrElse gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "S2" Then
                    gridView.Rows(i).Cells("ASSET").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                    gridView.Rows(i).Cells("ASSET1").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                ElseIf gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "G" Then
                    If Convert.ToDouble(IIf(gridView.Rows(i).Cells("ASSET").Value.ToString() <> "", gridView.Rows(i).Cells("ASSET").Value.ToString(), "0")) <> 0 Then
                        gridView.Rows(i).Cells("ASSACGROUPNAME1").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                        gridView.Rows(i).Cells("ASSACGROUPNAME1").Style.ForeColor = Color.Red
                        gridView.Rows(i).Cells("ASSET").Style.ForeColor = Color.Red
                        gridView.Rows(i).Cells("ASSET").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                        gridView.Rows(i).Cells("ASSET1").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                    End If
                ElseIf gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "G1" OrElse gridView.Rows(i).Cells("COLHEAD4").Value.ToString() = "G2" Then
                    gridView.Rows(i).Cells("ASSET").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                    gridView.Rows(i).Cells("ASSET1").Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                End If
            Next

            gridView.Focus()
        Else
            MessageBox.Show("Message", "Records not found...", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            dtpDate.Focus()
        End If
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        If MultiCostcentre = False Then
            gridView.DataSource = Nothing
            gridViewHead.DataSource = Nothing
            funcView()
            gridViewHead.Visible = False
        Else
            Rempty = False
            gridView.DataSource = Nothing
            gridViewHead.DataSource = Nothing
            funcViewMultiCostCentre()
        End If
        Prop_Sets()
    End Sub
    Private Sub funcViewMultiCostCentre()

        strSql = " SELECT COMPANYID  FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" & cmbCompany.Text & "'"
        Dim strCompId As String = BrighttechPack.GetSqlValue(cn, strSql, "COMPANYID", "")
        Dim strCostId As String = ""
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            strCostId += " AND COSTID IN"
            strCostId += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"

            strSql = "SELECT DISTINCT COSTID,COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & ") ORDER BY COSTID"
            da = New OleDbDataAdapter(strSql, cn)
            Dtcostid = New DataTable
            da.Fill(Dtcostid)
        Else
            strSql = "SELECT DISTINCT COSTID,COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTID"
            da = New OleDbDataAdapter(strSql, cn)
            Dtcostid = New DataTable
            da.Fill(Dtcostid)
            If Dtcostid.Rows.Count = 0 Then
                strSql = "SELECT '' COSTID,'' COSTNAME "
                da = New OleDbDataAdapter(strSql, cn)
                Dtcostid = New DataTable
                da.Fill(Dtcostid)
            End If
        End If
        strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "BALSHEET1')"
        strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET1"
        strSql += vbCrLf + "  CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET1"
        strSql += vbCrLf + "  (COMPANYID VARCHAR(3),COSTID VARCHAR(2),DISPORDER INT,ACID VARCHAR(25),TRANMODE VARCHAR(1),AMOUNT NUMERIC(15,2),DEBIT NUMERIC(15,2),CREDIT NUMERIC(15,2)"
        strSql += vbCrLf + "  ,TRANDATE SMALLDATETIME,PAYMODE VARCHAR(20),TRANNO VARCHAR(25),SEP VARCHAR(1))"
        strSql += vbCrLf + "  "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET1"
        strSql += vbCrLf + "  (COMPANYID,COSTID,DISPORDER,ACID,TRANMODE,AMOUNT,DEBIT,CREDIT,TRANDATE,PAYMODE,TRANNO,SEP)"
        strSql += vbCrLf + "  SELECT T.COMPANYID,ISNULL(T.COSTID,'') COSTID,G.DISPORDER ,T.ACCODE AS ACID,CASE WHEN ISNULL(DEBIT,0) > ISNULL(CREDIT,0) THEN 'D' ELSE 'C' END TRANMODE"
        strSql += vbCrLf + "  ,CASE WHEN ISNULL(DEBIT,0) <> 0 THEN DEBIT ELSE CREDIT END AMOUNT"
        strSql += vbCrLf + "  ,DEBIT,CREDIT,NULL TRANDATE,'' PAYMODE,NULL TRANNO,'O' SEP"
        strSql += vbCrLf + "  FROM  " & cnStockDb & "..OPENTRAILBALANCE T  INNER JOIN  " & cnAdminDb & "..ACHEAD AS A"
        strSql += vbCrLf + "  ON T.ACCODE = A.ACCODE INNER JOIN " & cnAdminDb & "..ACGROUP AS G ON G.ACGRPCODE = A.ACGRPCODE"
        strSql += vbCrLf + "  WHERE ISNULL(T.COMPANYID,'') = '" & strCompId & "'"
        strSql += strCostId
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT T.COMPANYID,ISNULL(T.COSTID,'') COSTID,G.DISPORDER,T.ACCODE ACID,T.TRANMODE,T.AMOUNT"
        strSql += vbCrLf + "  ,CASE WHEN TRANMODE = 'D' THEN AMOUNT END DEBIT,CASE WHEN TRANMODE = 'C' THEN AMOUNT END CREDIT"
        strSql += vbCrLf + "  ,TRANDATE,'' PAYMODE,TRANNO,'T' SEP FROM " & cnStockDb & "..ACCTRAN T"
        strSql += vbCrLf + "  INNER JOIN  " & cnAdminDb & "..ACHEAD AS A"
        strSql += vbCrLf + "  ON T.ACCODE = A.ACCODE INNER JOIN " & cnAdminDb & "..ACGROUP AS G ON G.ACGRPCODE = A.ACGRPCODE"
        strSql += vbCrLf + "  WHERE ISNULL(T.COMPANYID,'') = '" & strCompId & "' AND TRANDATE BETWEEN '" & cnTranFromDate & "' AND '" & dtpDate.Value.Date.ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''"
        strSql += strCostId
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "    IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "BALSHEET2')"
        strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET2"
        strSql += vbCrLf + "  SELECT A.ACCODE ACID,ISNULL(COSTID,'') COSTID,G.ACGRPCODE, ACGRPNAME,ACMAINCODE,GRPLEDGER,CASE WHEN ISNULL(GRPTYPE,'') = 'L' THEN 'L' ELSE 'A' END GRPTYPE"
        strSql += vbCrLf + "  ,DEBIT,CREDIT"
        strSql += vbCrLf + "  ,ACNAME ACNAME,TRANDATE,TRANMODE,AMOUNT,PAYMODE,TRANNO"
        strSql += vbCrLf + "  ,(SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE = G.ACMAINCODE) ACMAINGRPNAME"
        strSql += vbCrLf + "  ,(SELECT DISPORDER FROM  " & cnAdminDb & "..ACGROUP WHERE ACGRPCODE = G.ACGRPCODE) DISPORD"
        strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET2"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET1 AS T INNER JOIN  " & cnAdminDb & "..ACHEAD AS A"
        strSql += vbCrLf + "  ON T.ACID = A.ACCODE LEFT OUTER JOIN  " & cnAdminDb & "..ACGROUP AS G ON G.ACGRPCODE = A.ACGRPCODE"
        strSql += vbCrLf + "  WHERE ISNULL(GRPLEDGER,'') NOT IN ('P','T')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "BALSHEET0')"
        strSql += vbCrLf + "   DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET0"
        strSql += vbCrLf + "  SELECT 0 AS DISPORDER,'PROFIT & LOSS ACCOUNT' PARTICULARS,ISNULL(COSTID,'') COSTID"
        strSql += vbCrLf + "  ,ISNULL(SUM(ISNULL((CASE WHEN GRPTYPE = 'E' THEN ISNULL(CREDIT,0) - ISNULL(DEBIT,0) END),0)"
        strSql += vbCrLf + "  - ISNULL((CASE WHEN GRPTYPE != 'E' THEN ISNULL(DEBIT,0) - ISNULL(CREDIT,0) END),0)),0) + ISNULL((SELECT SUM(VALUE) FROM " & cnStockDb & "..CLOSINGVAL WHERE COMPANYID = '" & strCompId & "'" & strCostId & "),0) AMOUNT"
        strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET0"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET1 AS T INNER JOIN  " & cnAdminDb & "..ACHEAD AS A"
        strSql += vbCrLf + "  ON T.ACID = A.ACCODE INNER JOIN " & cnAdminDb & "..ACGROUP AS G ON G.ACGRPCODE = A.ACGRPCODE"
        strSql += vbCrLf + "  AND ISNULL(GRPLEDGER,'') IN ('P','T')"
        strSql += vbCrLf + "  GROUP BY COSTID"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        'For Colsing Stock By vasanth
        If Closin_stk = "N" Then
            strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "BALSHEET10')"
            strSql += vbCrLf + "   DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET10"
            strSql += vbCrLf + "  SELECT 1 AS DISPORDER,'CLOSING STOCK' PARTICULARS"
            strSql += vbCrLf + "  ,ISNULL(COSTID,'') COSTID,SUM(VALUE) AS AMOUNT"
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET10"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..CLOSINGVAL WHERE COMPANYID = '" & strCompId & "'" & strCostId & " "
            strSql += vbCrLf + "  GROUP BY COSTID"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        Else
            strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "BALSHEET10')"
            strSql += vbCrLf + "   DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET10"
            strSql += vbCrLf + "  CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET10"
            strSql += vbCrLf + "  (DISPORDER INT,PARTICULARS VARCHAR(50)"
            strSql += vbCrLf + "  ,COSTID VARCHAR(2),AMOUNT DECIMAL(15,2))"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

            For K As Integer = 0 To Dtcostid.Rows.Count - 1
                Dim Qry As String = ""
                'ProgressBarShow()
                If Closin_stk = "W" Then
                    'ProgressBarStep("Closing Stock arrival Weighted avg method", 10)
                    Qry = " EXEC " & cnAdminDb & "..SP_CLOSING_STOCK"
                Else
                    'ProgressBarStep("Closing Stock arrival LIFO method", 10)
                    Qry = " EXEC " & cnAdminDb & "..SP_CLOSINGSTOCK_LIFO"
                End If
                Qry += vbCrLf + " @DBNAME = '" & cnStockDb & "',@ADMINDB = '" & cnAdminDb & "'"
                Qry += vbCrLf + " ,@FRMDATE = '" & Format(dtpDate.Value, "yyyy-MM-dd") & "'"
                Qry += vbCrLf + " ,@TODATE = '" & Format(dtpDate.Value, "yyyy-MM-dd") & "'"
                Qry += vbCrLf + " ,@METALNAME = 'ALL',@CATCODE = 'ALL',@CATNAME = 'ALL'"
                Qry += vbCrLf + " ,@COSTNAME = '" & Dtcostid.Rows(K).Item(1).ToString.Trim & "'"
                Qry += vbCrLf + " ,@COMPANYID = '" & strCompId & "',@RPTTYPE = 'S'"
                Dim SYSTEMID As String = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
                Qry += vbCrLf + " ,@SYSTEMID = '" & SYSTEMID & "'"
                cmd = New OleDbCommand(Qry, cn) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

                strSql = vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET10 (DISPORDER,PARTICULARS,COSTID,AMOUNT)"
                strSql += vbCrLf + "  SELECT 1 AS DISPORDER,'CLOSING STOCK' PARTICULARS"
                strSql += vbCrLf + "  ,'" & Dtcostid.Rows(K).Item(0).ToString.Trim & "' COSTID,SUM(CAMOUNT) AS AMOUNT"
                strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPCSTKVALUE4" & SYSTEMID & " "
                strSql += vbCrLf + "  WHERE RESULT=3 AND COLHEAD='G'"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()
            Next
        End If

        For K As Integer = 0 To Dtcostid.Rows.Count - 1
            strSql = "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "BALSHEET3" & Dtcostid.Rows(K).Item(0).ToString.Trim & "')"
            strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET3" & Dtcostid.Rows(K).Item(0).ToString.Trim & ""
            strSql += vbCrLf + "  CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET3" & Dtcostid.Rows(K).Item(0).ToString.Trim & ""
            strSql += vbCrLf + "  ("
            strSql += vbCrLf + "   LIADISPORDER INT,LIAACGROUPNAME1 VARCHAR(250),LIABILITY1 VARCHAR(50),LIABILITY VARCHAR(50)"
            strSql += vbCrLf + "  ,ASSACGROUPNAME1 VARCHAR(250),ASSET1 VARCHAR(50),ASSET VARCHAR(50)"
            strSql += vbCrLf + "  ,RESULT INT,SEP INT,LIACMAINCODE VARCHAR(50),ASSACMAINCODE VARCHAR(50),LIAACGROUP VARCHAR(250),ASSACGROUP VARCHAR(250)"
            strSql += vbCrLf + "  ,LIAACGROUPNAME VARCHAR(250),LIAACMAINGRPNAME VARCHAR(250)"
            strSql += vbCrLf + "  ,ASSDISPORDER INT,ASSACGROUPNAME VARCHAR(250),ASSACMAINGRPNAME VARCHAR(250),SNO INT IDENTITY(1,1),COLHEAD VARCHAR(2),COSTID VARCHAR(2)"
            strSql += vbCrLf + "  )"
            strSql += vbCrLf + "  IF EXISTS (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "BALSHEET4" & Dtcostid.Rows(K).Item(0).ToString.Trim & "')"
            strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET4" & Dtcostid.Rows(K).Item(0).ToString.Trim & ""
            strSql += vbCrLf + "  CREATE TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET4" & Dtcostid.Rows(K).Item(0).ToString.Trim & ""
            strSql += vbCrLf + "  ("
            strSql += vbCrLf + "   LIADISPORDER INT,LIAACGROUPNAME1 VARCHAR(250),LIABILITY1 VARCHAR(50),LIABILITY VARCHAR(50)"
            strSql += vbCrLf + "  ,ASSACGROUPNAME1 VARCHAR(250),ASSET1 VARCHAR(50),ASSET VARCHAR(50)"
            strSql += vbCrLf + "  ,RESULT INT,SEP INT,LIAACMAINCODE VARCHAR(50),ASSACMAINCODE VARCHAR(50),LIAACGROUP VARCHAR(250),ASSACGROUP VARCHAR(250)"
            strSql += vbCrLf + "  ,LIAACGROUPNAME VARCHAR(250),LIAACMAINGRPNAME VARCHAR(250)"
            strSql += vbCrLf + "  ,ASSDISPORDER INT,ASSACGROUPNAME VARCHAR(250),ASSACMAINGRPNAME VARCHAR(250),SNO INT IDENTITY(1,1),COLHEAD VARCHAR(2),COSTID VARCHAR(2)"
            strSql += vbCrLf + "  )"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET3" & Dtcostid.Rows(K).Item(0).ToString.Trim & ""
            strSql += vbCrLf + "  ("
            strSql += vbCrLf + "   LIADISPORDER,LIAACGROUPNAME1,LIABILITY1,LIABILITY,ASSACGROUPNAME1,ASSET1,ASSET"
            strSql += vbCrLf + "  ,RESULT,SEP,LIACMAINCODE,ASSACMAINCODE,LIAACGROUP,ASSACGROUP,LIAACGROUPNAME,LIAACMAINGRPNAME"
            strSql += vbCrLf + "  ,ASSACGROUPNAME,ASSACMAINGRPNAME,COLHEAD,COSTID"
            strSql += vbCrLf + "  )"
            strSql += vbCrLf + "  SELECT DISTINCT DISPORD LIADISPORDER,ACMAINGRPNAME LIAACGROUPNAME1,NULL LIABILITY1"
            strSql += vbCrLf + "  ,NULL LIABILITY,NULL ASSACGROUPNAME1,NULL ASSET1,NULL ASSET,'1' RESULT"
            strSql += vbCrLf + "  ,'1' SEP,ACMAINCODE LIAACMAINCODE,'' ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP"
            strSql += vbCrLf + "  ,'' LIAACGROUPNAME,ACMAINGRPNAME LIAACMAINGRPNAME"
            strSql += vbCrLf + "  ,''ASSACGROUPNAME,'' ASSACMAINGRPNAME,'T' COLHEAD,COSTID"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET2 AS T WHERE ISNULL(GRPTYPE,'') = 'L' AND ACGRPCODE=ACMAINCODE AND COSTID='" & Dtcostid.Rows(K).Item(0).ToString.Trim & "'"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT DISPORDER LIADISPORDER,PARTICULARS LIAACGROUPNAME1,NULL LIABILITY1,NULL LIABILITY"
            strSql += vbCrLf + "  ,NULL ASSACGROUPNAME1,NULL ASSET1,NULL ASSET,'2' RESULT,'1' SEP,NULL LIAACMAINCODE"
            strSql += vbCrLf + "  ,'' ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP,'' LIAACGROUPNAME,PARTICULARS LIAACMAINGRPNAME"
            strSql += vbCrLf + "  ,'' ASSACGROUPNAME,'' ASSACMAINGRPNAME,'T' COLHEAD,COSTID"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET0 WHERE  COSTID='" & Dtcostid.Rows(K).Item(0).ToString.Trim & "'"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT DISPORDER LIADISPORDER,NULL LIAACGROUPNAME1,NULL LIABILITY1,CONVERT(VARCHAR,AMOUNT) LIABILITY"
            strSql += vbCrLf + "  ,NULL ASSACGROUPNAME1,NULL ASSET1,NULL ASSET,'3' RESULT,'1' SEP,NULL LIAACMAINCODE"
            strSql += vbCrLf + "  ,'' ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP,'' LIAACGROUPNAME,PARTICULARS LIAACMAINGRPNAME"
            strSql += vbCrLf + "  ,'' ASSACGROUPNAME,'' ASSACMAINGRPNAME,'T' COLHEAD,COSTID"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET0 WHERE  COSTID='" & Dtcostid.Rows(K).Item(0).ToString.Trim & "'"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT DISTINCT DISPORD LIADISPORDER,'  ' + ACGRPNAME LIAACGROUPNAME1"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR,ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) LIABILITY1,NULL LIABILITY"
            strSql += vbCrLf + "  ,NULL ASSACGROUPNAME1,NULL ASSET1,NULL ASSET,'4' RESULT,'2' SEP"
            strSql += vbCrLf + "  ,ACMAINCODE LIAACMAINCODE,'' ASSACMAINCODE,ACGRPCODE LIAACGROUP,'' ASSACGROUP"
            strSql += vbCrLf + "  ,ACGRPNAME LIAACGROUPNAME,ACMAINGRPNAME LIAACMAINGRPNAME"
            strSql += vbCrLf + "  ,'' ASSACGROUPNAME,'' ASSACMAINGRPNAME,'T2' COLHEAD,COSTID"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET2 WHERE ISNULL(GRPTYPE,'') = 'L' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'')  AND COSTID='" & Dtcostid.Rows(K).Item(0).ToString.Trim & "'"
            strSql += vbCrLf + "  GROUP BY DISPORD,ACMAINGRPNAME,ACMAINCODE,ACGRPNAME,ACGRPCODE,COSTID"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT 0 LIADISPORDER,'' LIAACGROUPNAME1,NULL LIABILITY1"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR,ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0)) LIABILITY"
            strSql += vbCrLf + "  ,NULL ASSACGROUPNAME1,NULL ASSET1,NULL ASSET,'5' RESULT,'3' SEP"
            strSql += vbCrLf + "  ,ACMAINCODE LIAACMAINCODE,'' ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP,'' LIAACGROUPNAME"
            strSql += vbCrLf + "  ,ACMAINGRPNAME LIAACMAINGRPNAME,'' ASSACGROUPNAME,'' ASSACMAINGRPNAME,'G1' COLHEAD,COSTID"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET2 WHERE ISNULL(GRPTYPE,'') = 'L' AND COSTID='" & Dtcostid.Rows(K).Item(0).ToString.Trim & "'"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACMAINCODE,COSTID"
            strSql += vbCrLf + "  UNION ALL "
            strSql += vbCrLf + "  SELECT DISTINCT DISPORD LIADISPORDER,ACMAINGRPNAME LIAACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL LIABILITY1,NULL LIABILITY,NULL  ASSACGROUPNAME1 ,NULL ASSET1,NULL ASSET,'1' RESULT,'1' SEP"
            strSql += vbCrLf + "  ,ACMAINCODE LIAACMAINCODE,NULL ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP"
            strSql += vbCrLf + "  ,'' LIAACGROUPNAME,ACMAINGRPNAME LIAACMAINGRPNAME,''ASSACGROUPNAME,'' ASSACMAINGRPNAME,'T' COLHEAD,COSTID"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET2 WHERE ISNULL(GRPTYPE,'') = 'A'  AND COSTID='" & Dtcostid.Rows(K).Item(0).ToString.Trim & "'"
            strSql += vbCrLf + "  GROUP BY DISPORD,ACMAINGRPNAME,ACMAINCODE,COSTID"
            strSql += vbCrLf + "  HAVING (SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0)))<0"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT DISTINCT DISPORD LIADISPORDER, '' LIAACGROUPNAME1,NULL LIABILITY1"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR, ISNULL(SUM(CREDIT),0) - ISNULL(SUM(DEBIT),0))  LIABILITY"
            strSql += vbCrLf + "  ,NULL ASSACGROUPNAME1 ,NULL ASSET1,NULL ASSET,'5' RESULT,'3' SEP"
            strSql += vbCrLf + "  ,ACMAINCODE LIAACMAINCODE,'' ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP"
            strSql += vbCrLf + "  ,'' LIAACGROUPNAME,ACMAINGRPNAME LIAACMAINGRPNAME"
            strSql += vbCrLf + "  ,'' ASSACGROUPNAME,'' ASSACMAINGRPNAME,'G1' COLHEAD,COSTID"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET2 WHERE ISNULL(GRPTYPE,'') = 'A' AND COSTID='" & Dtcostid.Rows(K).Item(0).ToString.Trim & "'"
            strSql += vbCrLf + "  GROUP BY DISPORD,ACMAINGRPNAME,ACMAINCODE,DISPORD,COSTID"
            strSql += vbCrLf + "  HAVING (SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0)))<0"
            strSql += vbCrLf + "  ORDER BY LIAACMAINGRPNAME,SEP,LIADISPORDER,LIAACGROUPNAME,RESULT,COSTID"
            'strSql += vbCrLf + "  , (SELECT DISPORDER FROM ACGRPCODE WHERE ACMAINCODE = T.ACMAINCODE)";
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "   INSERT INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET4" & Dtcostid.Rows(K).Item(0).ToString.Trim & ""
            strSql += vbCrLf + "  ("
            strSql += vbCrLf + "   LIAACGROUPNAME1,LIABILITY1,LIABILITY,ASSACGROUPNAME1,ASSET1,ASSET"
            strSql += vbCrLf + "  ,RESULT,SEP,LIAACMAINCODE,ASSACMAINCODE,LIAACGROUP,ASSACGROUP,LIAACGROUPNAME,LIAACMAINGRPNAME"
            strSql += vbCrLf + "  ,ASSACGROUPNAME,ASSACMAINGRPNAME,ASSDISPORDER,COLHEAD,COSTID"
            strSql += vbCrLf + "  )"
            strSql += vbCrLf + "  SELECT DISTINCT NULL LIAACGROUPNAME1,NULL LIABILITY1,NULL LIABILITY"
            strSql += vbCrLf + "  ,ACMAINGRPNAME ASSACGROUPNAME1,NULL ASSET1,NULL ASSET,'1' RESULT,'1' SEP"
            strSql += vbCrLf + "  ,NULL LIAACMAINCODE,ACMAINCODE ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP"
            strSql += vbCrLf + "  ,'' LIAACGROUPNAME,'' LIAACMAINGRPNAME,''ASSACGROUPNAME,ACMAINGRPNAME ASSACMAINGRPNAME,DISPORD ASSDISPORDER,'T' COLHEAD,COSTID"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET2 WHERE ISNULL(GRPTYPE,'') = 'A' AND ACGRPCODE=ACMAINCODE  AND COSTID='" & Dtcostid.Rows(K).Item(0).ToString.Trim & "'"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACMAINCODE,DISPORD,COSTID"
            strSql += vbCrLf + "  HAVING (SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0)))>0"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT DISTINCT '' LIAACGROUPNAME1,NULL LIABILITY1,NULL LIABILITY,'  ' + ACGRPNAME ASSACGROUPNAME1"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) ASSET1,NULL ASSET,'4' RESULT,'2' SEP"
            strSql += vbCrLf + "  ,'' LIAACMAINCODE,ACMAINCODE ASSACMAINCODE,'' LIAACGROUP,ACGRPCODE ASSACGROUP"
            strSql += vbCrLf + "  ,'' LIAACGROUPNAME,'' LIAACMAINGRPNAME"
            strSql += vbCrLf + "  ,ACGRPNAME ASSACGROUPNAME,ACMAINGRPNAME ASSACMAINGRPNAME,DISPORD ASSDISPORDER,'T2' COLHEAD,COSTID"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET2 WHERE ISNULL(GRPTYPE,'') = 'A' AND ISNULL(ACMAINCODE,'') != ISNULL(ACGRPCODE,'') AND COSTID='" & Dtcostid.Rows(K).Item(0).ToString.Trim & "'"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACMAINCODE,ACGRPNAME,ACGRPCODE,DISPORD,COSTID"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT DISTINCT '' LIAACGROUPNAME1,NULL LIABILITY1,NULL LIABILITY,NULL ASSACGROUPNAME1"
            strSql += vbCrLf + "  ,NULL ASSET1,CONVERT(VARCHAR,ISNULL(SUM(DEBIT),0) - ISNULL(SUM(CREDIT),0)) ASSET"
            strSql += vbCrLf + "  ,'5' RESULT,'3' SEP,'' LIAACMAINCODE,ACMAINCODE ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP"
            strSql += vbCrLf + "  ,'' LIAACGROUPNAME,'' LIAACMAINGRPNAME,'' ASSACGROUPNAME,ACMAINGRPNAME ASSACMAINGRPNAME,0 ASSDISPORDER,'G1' COLHEAD,COSTID"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET2 WHERE ISNULL(GRPTYPE,'') = 'A' AND COSTID='" & Dtcostid.Rows(K).Item(0).ToString.Trim & "'"
            strSql += vbCrLf + "  GROUP BY ACMAINGRPNAME,ACMAINCODE,COSTID"
            strSql += vbCrLf + "  HAVING (SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0)))>0"
            'For Colsing Stock By vasanth
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT NULL LIAACGROUPNAME1,NULL LIABILITY1,NULL LIABILITY"
            strSql += vbCrLf + "  ,PARTICULARS  ASSACGROUPNAME1,NULL ASSET1,NULL ASSET,'2' RESULT,'1' SEP"
            strSql += vbCrLf + "  ,NULL LIAACMAINCODE,'' ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP"
            strSql += vbCrLf + "  ,'' LIAACGROUPNAME,'' LIAACMAINGRPNAME,'' ASSACGROUPNAME,PARTICULARS  ASSACMAINGRPNAME,DISPORDER ASSDISPORDER,'T' COLHEAD,COSTID"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET10 WHERE  COSTID='" & Dtcostid.Rows(K).Item(0).ToString.Trim & "'"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT NULL LIAACGROUPNAME1,NULL LIABILITY1,NULL LIABILITY"
            strSql += vbCrLf + "  ,NULL ASSACGROUPNAME1,NULL ASSET1,CONVERT(VARCHAR,AMOUNT) ASSET,'3' RESULT,'1' SEP"
            strSql += vbCrLf + "  ,NULL LIAACMAINCODE,'' ASSACMAINCODE,'' LIAACGROUP,'' ASSACGROUP"
            strSql += vbCrLf + "  ,'' LIAACGROUPNAME,'' LIAACMAINGRPNAME,'' ASSACGROUPNAME,PARTICULARS ASSACMAINGRPNAME,DISPORDER ASSDISPORDER,'G1' COLHEAD,COSTID"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET10  WHERE COSTID='" & Dtcostid.Rows(K).Item(0).ToString.Trim & "'"
            strSql += vbCrLf + "  ORDER BY ASSACMAINGRPNAME,SEP,ASSDISPORDER,ASSACGROUPNAME,RESULT,COSTID"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        Next


        strSql = "  IF EXISTS(SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & userId.ToString() & "BALSHEET5')"
        strSql += vbCrLf + " DROP TABLE TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET5"
        strSql += vbCrLf + " SELECT "
        For j As Integer = 0 To Dtcostid.Rows.Count - 1
            strSql += vbCrLf + " T3" & j.ToString & ".LIAACGROUPNAME1 AS LIAACGROUPNAME1" & Dtcostid.Rows(j).Item(0).ToString.Trim & ",T3" & j.ToString & ".LIABILITY1 AS LIABILITY1" & Dtcostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + " T3" & j.ToString & ".LIABILITY AS LIABILITY" & Dtcostid.Rows(j).Item(0).ToString.Trim & ",T4" & j.ToString & ".ASSACGROUPNAME1 AS ASSACGROUPNAME1" & Dtcostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + " T4" & j.ToString & ".ASSET1 AS ASSET1" & Dtcostid.Rows(j).Item(0).ToString.Trim & ", T4" & j.ToString & ".ASSET AS ASSET" & Dtcostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + " T3" & j.ToString & ".RESULT AS RESULT3" & Dtcostid.Rows(j).Item(0).ToString.Trim & ",T4" & j.ToString & ".RESULT AS RESULT4" & Dtcostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + " T3" & j.ToString & ".COLHEAD AS COLHEAD3" & Dtcostid.Rows(j).Item(0).ToString.Trim & ",T4" & j.ToString & ".COLHEAD AS COLHEAD4" & Dtcostid.Rows(j).Item(0).ToString.Trim & ","
        Next
        strSql += vbCrLf + " T30.SNO SNO3,T40.SNO SNO4,T30.SEP SEP3,T40.SEP SEP4 "
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET5 "
        Dim joinTbl As String = ""
        Dim MaxCount As Integer
        Dim ChkCount As Integer
        For j As Integer = 0 To Dtcostid.Rows.Count - 1
            If j = 0 Then
                MaxCount = Val(objGPack.GetSqlValue("SELECT  COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET3" & Dtcostid.Rows(j).Item(0).ToString.Trim, , "", ))
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET3" & Dtcostid.Rows(j).Item(0).ToString.Trim & " AS T3" & j.ToString & " "
                joinTbl = "T3" & j.ToString
                strSql += vbCrLf + " FULL JOIN TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET4" & Dtcostid.Rows(j).Item(0).ToString.Trim & " AS T4" & j.ToString & " ON T4" & j.ToString & ".SNO = " & joinTbl & ".SNO "
                ChkCount = Val(objGPack.GetSqlValue("SELECT  COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET4" & Dtcostid.Rows(j).Item(0).ToString.Trim, , "", ))
                If ChkCount > MaxCount Then MaxCount = ChkCount : joinTbl = "T4" & j.ToString
            Else
                strSql += vbCrLf + " FULL JOIN TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET3" & Dtcostid.Rows(j).Item(0).ToString.Trim & " AS T3" & j.ToString & " ON T3" & j.ToString & ".SNO = " & joinTbl & ".SNO "
                ChkCount = Val(objGPack.GetSqlValue("SELECT  COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET3" & Dtcostid.Rows(j).Item(0).ToString.Trim, , "", ))
                If ChkCount > MaxCount Then MaxCount = ChkCount : joinTbl = "T3" & j.ToString
                strSql += vbCrLf + " FULL JOIN TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET4" & Dtcostid.Rows(j).Item(0).ToString.Trim & " AS T4" & j.ToString & " ON T4" & j.ToString & ".SNO = " & joinTbl & ".SNO "
                ChkCount = Val(objGPack.GetSqlValue("SELECT  COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET4" & Dtcostid.Rows(j).Item(0).ToString.Trim, , "", ))
                If ChkCount > MaxCount Then MaxCount = ChkCount : joinTbl = "T4" & j.ToString
            End If
        Next
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "  SELECT "
        For j As Integer = 0 To Dtcostid.Rows.Count - 1
            strSql += vbCrLf + "  LIAACGROUPNAME1" & Dtcostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + "  CASE WHEN RESULT3" & Dtcostid.Rows(j).Item(0).ToString.Trim & " NOT IN (6) AND ISNULL(LIABILITY1" & Dtcostid.Rows(j).Item(0).ToString.Trim & ",'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(LIABILITY1" & Dtcostid.Rows(j).Item(0).ToString.Trim & ",'') = '' THEN '0' ELSE LIABILITY1" & Dtcostid.Rows(j).Item(0).ToString.Trim & " END),'D','Y') ELSE LIABILITY1" & Dtcostid.Rows(j).Item(0).ToString.Trim & " END  LIABILITY1" & Dtcostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + "  CASE WHEN RESULT3" & Dtcostid.Rows(j).Item(0).ToString.Trim & " NOT IN (6) AND ISNULL(LIABILITY" & Dtcostid.Rows(j).Item(0).ToString.Trim & ",'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(LIABILITY" & Dtcostid.Rows(j).Item(0).ToString.Trim & ",'') = '' THEN '0' ELSE LIABILITY" & Dtcostid.Rows(j).Item(0).ToString.Trim & " END),'D','Y') ELSE LIABILITY" & Dtcostid.Rows(j).Item(0).ToString.Trim & " END LIABILITY" & Dtcostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + "  ASSACGROUPNAME1" & Dtcostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + "  CASE WHEN RESULT4" & Dtcostid.Rows(j).Item(0).ToString.Trim & " NOT IN (6) AND ISNULL(ASSET1" & Dtcostid.Rows(j).Item(0).ToString.Trim & ",'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(ASSET1" & Dtcostid.Rows(j).Item(0).ToString.Trim & ",'') = '' THEN '0' ELSE ASSET1" & Dtcostid.Rows(j).Item(0).ToString.Trim & " END),'D','Y') ELSE ASSET1" & Dtcostid.Rows(j).Item(0).ToString.Trim & " END ASSET1" & Dtcostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + "  CASE WHEN RESULT4" & Dtcostid.Rows(j).Item(0).ToString.Trim & " NOT IN (6) AND ISNULL(ASSET" & Dtcostid.Rows(j).Item(0).ToString.Trim & ",'') != '' THEN " & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(ASSET" & Dtcostid.Rows(j).Item(0).ToString.Trim & ",'') = '' THEN '0' ELSE ASSET" & Dtcostid.Rows(j).Item(0).ToString.Trim & " END),'D','Y') ELSE ASSET" & Dtcostid.Rows(j).Item(0).ToString.Trim & " END ASSET" & Dtcostid.Rows(j).Item(0).ToString.Trim & ","
            If j = Dtcostid.Rows.Count - 1 Then
                strSql += vbCrLf + "  COLHEAD3" & Dtcostid.Rows(j).Item(0).ToString.Trim & ",COLHEAD4" & Dtcostid.Rows(j).Item(0).ToString.Trim & ""
            Else
                strSql += vbCrLf + "  COLHEAD3" & Dtcostid.Rows(j).Item(0).ToString.Trim & ",COLHEAD4" & Dtcostid.Rows(j).Item(0).ToString.Trim & ","
            End If
        Next
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET5"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT "
        For j As Integer = 0 To Dtcostid.Rows.Count - 1
            strSql += vbCrLf + "  'TOTAL =>' LIAACGROUPNAME1" & Dtcostid.Rows(j).Item(0).ToString.Trim & " ,NULL LIABILITY1" & Dtcostid.Rows(j).Item(0).ToString.Trim & ",CONVERT(VARCHAR," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT3" & Dtcostid.Rows(j).Item(0).ToString.Trim & " IN ('3','5') THEN CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(LIABILITY" & Dtcostid.Rows(j).Item(0).ToString.Trim & ",'') = '' THEN '0' ELSE LIABILITY" & Dtcostid.Rows(j).Item(0).ToString.Trim & " END) ELSE '0' END)),'D','Y')) LIABILITY" & Dtcostid.Rows(j).Item(0).ToString.Trim & ","
            strSql += vbCrLf + "  'TOTAL =>' ASSACGROUPNAME1" & Dtcostid.Rows(j).Item(0).ToString.Trim & " ,NULL ASSET1" & Dtcostid.Rows(j).Item(0).ToString.Trim & ",CONVERT(VARCHAR," & cnStockDb & ".[DBO].FUNC_RUPEES_WITHCOMMA(SUM(CONVERT(NUMERIC(15,2),CASE WHEN RESULT4" & Dtcostid.Rows(j).Item(0).ToString.Trim & " IN ('3','5')  THEN CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(ASSET" & Dtcostid.Rows(j).Item(0).ToString.Trim & ",'') = '' THEN '0' ELSE ASSET" & Dtcostid.Rows(j).Item(0).ToString.Trim & " END) ELSE '0' END)),'D','Y')) ASSET" & Dtcostid.Rows(j).Item(0).ToString.Trim & ","
            If j = Dtcostid.Rows.Count - 1 Then
                strSql += vbCrLf + "  'G' COLHEAD3" & Dtcostid.Rows(j).Item(0).ToString.Trim & ",'G' COLHEAD4" & Dtcostid.Rows(j).Item(0).ToString.Trim & ""
            Else
                strSql += vbCrLf + "  'G' COLHEAD3" & Dtcostid.Rows(j).Item(0).ToString.Trim & ",'G' COLHEAD4" & Dtcostid.Rows(j).Item(0).ToString.Trim & ","
            End If
        Next
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & userId.ToString() & "BALSHEET5"
        da = New OleDbDataAdapter(strSql, cn)
        dt = New DataTable()
        da.Fill(dt)
        gridView.DataSource = dt

        If gridView.RowCount > 0 Then
            lblTitle.Text = cmbCompany.Text & "BALANCE SHEET FOR THE PERIOD OF - 01/04/" & (IIf(dtpDate.Value.Date.Month > 3, dtpDate.Value.Date.Year, dtpDate.Value.Date.Year - 1)).ToString() & " - " & dtpDate.Text.Replace("-", "/")
            If chkCmbCostCentre.Text <> "" Then lblTitle.Text = lblTitle.Text & " COST CENTRE :" & chkCmbCostCentre.Text
            gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            gridView.Font = New Font("VERDANA", 9, FontStyle.Regular)
            For V As Integer = 0 To Dtcostid.Rows.Count - 1
                gridView.Columns("LIAACGROUPNAME1" & Dtcostid.Rows(V).Item(0).ToString.Trim).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                gridView.Columns("LIAACGROUPNAME1" & Dtcostid.Rows(V).Item(0).ToString.Trim).HeaderText = "LIABILITIES"
                gridView.Columns("LIAACGROUPNAME1" & Dtcostid.Rows(V).Item(0).ToString.Trim).DefaultCellStyle.Font = New Font("VERDANA", 7, FontStyle.Regular)
                gridView.Columns("LIAACGROUPNAME1" & Dtcostid.Rows(V).Item(0).ToString.Trim).Width = 220
                gridView.Columns("LIABILITY" & Dtcostid.Rows(V).Item(0).ToString.Trim).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns("LIABILITY" & Dtcostid.Rows(V).Item(0).ToString.Trim).HeaderText = "AMOUNT"
                gridView.Columns("LIABILITY" & Dtcostid.Rows(V).Item(0).ToString.Trim).Width = 125
                gridView.Columns("LIABILITY1" & Dtcostid.Rows(V).Item(0).ToString.Trim).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns("LIABILITY1" & Dtcostid.Rows(V).Item(0).ToString.Trim).HeaderText = "AMOUNT"
                gridView.Columns("LIABILITY1" & Dtcostid.Rows(V).Item(0).ToString.Trim).Width = 125
                gridView.Columns("ASSACGROUPNAME1" & Dtcostid.Rows(V).Item(0).ToString.Trim).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                gridView.Columns("ASSACGROUPNAME1" & Dtcostid.Rows(V).Item(0).ToString.Trim).HeaderText = "ASSETS"
                gridView.Columns("ASSACGROUPNAME1" & Dtcostid.Rows(V).Item(0).ToString.Trim).Width = 220
                gridView.Columns("ASSACGROUPNAME1" & Dtcostid.Rows(V).Item(0).ToString.Trim).DefaultCellStyle.Font = New Font("VERDANA", 7, FontStyle.Regular)
                gridView.Columns("ASSET" & Dtcostid.Rows(V).Item(0).ToString.Trim).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns("ASSET" & Dtcostid.Rows(V).Item(0).ToString.Trim).Width = 125
                gridView.Columns("ASSET" & Dtcostid.Rows(V).Item(0).ToString.Trim).HeaderText = "AMOUNT"
                gridView.Columns("ASSET1" & Dtcostid.Rows(V).Item(0).ToString.Trim).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                gridView.Columns("ASSET1" & Dtcostid.Rows(V).Item(0).ToString.Trim).Width = 125
                gridView.Columns("ASSET1" & Dtcostid.Rows(V).Item(0).ToString.Trim).HeaderText = "AMOUNT"
                gridView.Columns("COLHEAD3" & Dtcostid.Rows(V).Item(0).ToString.Trim).Visible = False
                gridView.Columns("COLHEAD4" & Dtcostid.Rows(V).Item(0).ToString.Trim).Visible = False

                For i As Integer = 0 To gridView.RowCount - 1
                    If gridView.Rows(i).Cells("COLHEAD3" & Dtcostid.Rows(V).Item(0).ToString.Trim).Value.ToString() = "T" OrElse gridView.Rows(i).Cells("COLHEAD3" & Dtcostid.Rows(V).Item(0).ToString.Trim).Value.ToString() = "T1" OrElse gridView.Rows(i).Cells("COLHEAD3" & Dtcostid.Rows(V).Item(0).ToString.Trim).Value.ToString() = "T2" Then
                        gridView.Rows(i).Cells("LIAACGROUPNAME1" & Dtcostid.Rows(V).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                        gridView.Rows(i).Cells("LIABILITY" & Dtcostid.Rows(V).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                    ElseIf gridView.Rows(i).Cells("COLHEAD3" & Dtcostid.Rows(V).Item(0).ToString.Trim).Value.ToString() = "S" OrElse gridView.Rows(i).Cells("COLHEAD3" & Dtcostid.Rows(V).Item(0).ToString.Trim).Value.ToString() = "S1" OrElse gridView.Rows(V).Cells("COLHEAD3" & Dtcostid.Rows(V).Item(0).ToString.Trim).Value.ToString() = "S2" Then
                        gridView.Rows(i).Cells("LIABILITY" & Dtcostid.Rows(V).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                        gridView.Rows(i).Cells("LIABILITY1" & Dtcostid.Rows(V).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                    ElseIf gridView.Rows(i).Cells("COLHEAD3" & Dtcostid.Rows(V).Item(0).ToString.Trim).Value.ToString() = "G" Then
                        If Convert.ToDouble(IIf(gridView.Rows(i).Cells("LIABILITY" & Dtcostid.Rows(V).Item(0).ToString.Trim).Value.ToString() <> "", gridView.Rows(i).Cells("LIABILITY" & Dtcostid.Rows(V).Item(0).ToString.Trim).Value.ToString(), "0")) <> 0 Then
                            gridView.Rows(i).Cells("LIAACGROUPNAME1" & Dtcostid.Rows(V).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                            gridView.Rows(i).Cells("LIAACGROUPNAME1" & Dtcostid.Rows(V).Item(0).ToString.Trim).Style.ForeColor = Color.Red
                            gridView.Rows(i).Cells("LIABILITY" & Dtcostid.Rows(V).Item(0).ToString.Trim).Style.ForeColor = Color.Red
                            gridView.Rows(i).Cells("LIABILITY" & Dtcostid.Rows(V).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                            gridView.Rows(i).Cells("LIABILITY1" & Dtcostid.Rows(V).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                        End If
                    ElseIf gridView.Rows(i).Cells("COLHEAD3" & Dtcostid.Rows(V).Item(0).ToString.Trim).Value.ToString() = "G1" OrElse gridView.Rows(i).Cells("COLHEAD3" & Dtcostid.Rows(V).Item(0).ToString.Trim).Value.ToString() = "G2" Then
                        gridView.Rows(i).Cells("LIABILITY" & Dtcostid.Rows(V).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                        gridView.Rows(i).Cells("LIABILITY1" & Dtcostid.Rows(V).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                    End If

                    If gridView.Rows(i).Cells("COLHEAD4" & Dtcostid.Rows(V).Item(0).ToString.Trim).Value.ToString() = "T" OrElse gridView.Rows(i).Cells("COLHEAD4" & Dtcostid.Rows(V).Item(0).ToString.Trim).Value.ToString() = "T1" OrElse gridView.Rows(i).Cells("COLHEAD4" & Dtcostid.Rows(V).Item(0).ToString.Trim).Value.ToString() = "T2" Then
                        gridView.Rows(i).Cells("ASSACGROUPNAME1" & Dtcostid.Rows(V).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                        gridView.Rows(i).Cells("ASSET" & Dtcostid.Rows(V).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                    ElseIf gridView.Rows(i).Cells("COLHEAD4" & Dtcostid.Rows(V).Item(0).ToString.Trim).Value.ToString() = "S" OrElse gridView.Rows(i).Cells("COLHEAD4" & Dtcostid.Rows(V).Item(0).ToString.Trim).Value.ToString() = "S1" OrElse gridView.Rows(i).Cells("COLHEAD4" & Dtcostid.Rows(V).Item(0).ToString.Trim).Value.ToString() = "S2" Then
                        gridView.Rows(i).Cells("ASSET" & Dtcostid.Rows(V).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                        gridView.Rows(i).Cells("ASSET1" & Dtcostid.Rows(V).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                    ElseIf gridView.Rows(i).Cells("COLHEAD4" & Dtcostid.Rows(V).Item(0).ToString.Trim).Value.ToString() = "G" Then
                        If Convert.ToDouble(IIf(gridView.Rows(i).Cells("ASSET" & Dtcostid.Rows(V).Item(0).ToString.Trim).Value.ToString() <> "", gridView.Rows(i).Cells("ASSET" & Dtcostid.Rows(V).Item(0).ToString.Trim).Value.ToString(), "0")) <> 0 Then
                            gridView.Rows(i).Cells("ASSACGROUPNAME1" & Dtcostid.Rows(V).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                            gridView.Rows(i).Cells("ASSACGROUPNAME1" & Dtcostid.Rows(V).Item(0).ToString.Trim).Style.ForeColor = Color.Red
                            gridView.Rows(i).Cells("ASSET" & Dtcostid.Rows(V).Item(0).ToString.Trim).Style.ForeColor = Color.Red
                            gridView.Rows(i).Cells("ASSET" & Dtcostid.Rows(V).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                            gridView.Rows(i).Cells("ASSET1" & Dtcostid.Rows(V).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                        End If
                    ElseIf gridView.Rows(i).Cells("COLHEAD4" & Dtcostid.Rows(V).Item(0).ToString.Trim).Value.ToString() = "G1" OrElse gridView.Rows(i).Cells("COLHEAD4" & Dtcostid.Rows(V).Item(0).ToString.Trim).Value.ToString() = "G2" Then
                        gridView.Rows(i).Cells("ASSET" & Dtcostid.Rows(V).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                        gridView.Rows(i).Cells("ASSET1" & Dtcostid.Rows(V).Item(0).ToString.Trim).Style.Font = New Font("VERDANA", 9, FontStyle.Bold)
                    End If
                Next
            Next
            gridView.Focus()
            funcHeaderNew()
            With gridViewHead
                .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
                .Height = gridView.ColumnHeadersHeight
                Dim colWid As Integer = 0
                For cnt As Integer = 0 To gridView.ColumnCount - 1
                    If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
                Next
                If Not Rempty Then
                    If colWid >= gridView.Width Then
                        .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                        .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
                    Else
                        .Columns("SCROLL").Visible = False
                    End If
                End If
            End With

        Else
            MessageBox.Show("Message", "Records not found...", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            dtpDate.Focus()
        End If
    End Sub

    Public Function funcHeaderNew() As Integer
        Dim dtheader As New DataTable
        dtheader.Clear()
        Try
            Dim dtMergeHeader As New DataTable("~MERGEHEADER")
            With dtMergeHeader
                For i As Integer = 0 To Dtcostid.Rows.Count - 1
                    .Columns.Add("LIAACGROUPNAME1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~LIABILITY" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~LIABILITY1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~ASSACGROUPNAME1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~ASSET" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~ASSET1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "", GetType(String))
                    .Columns.Add("COLHEAD3" & Dtcostid.Rows(i).Item(0).ToString.Trim, GetType(String))
                    .Columns.Add("COLHEAD4" & Dtcostid.Rows(i).Item(0).ToString.Trim, GetType(String))
                Next
                .Columns.Add("SCROLL", GetType(String))
                For i As Integer = 0 To Dtcostid.Rows.Count - 1
                    .Columns("LIAACGROUPNAME1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~LIABILITY" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~LIABILITY1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~ASSACGROUPNAME1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~ASSET" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~ASSET1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "").Caption = Dtcostid.Rows(i).Item(1).ToString
                    .Columns("COLHEAD3" & Dtcostid.Rows(i).Item(0).ToString.Trim).Caption = ""
                    .Columns("COLHEAD4" & Dtcostid.Rows(i).Item(0).ToString.Trim).Caption = ""
                Next
                .Columns("SCROLL").Caption = ""
            End With
            gridViewHead.DataSource = dtMergeHeader
            funcGridHeaderStyle()
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
    End Function
    Public Function funcGridHeaderStyle() As Integer
        With gridViewHead
            For i As Integer = 0 To DtCostid.Rows.Count - 1
                With .Columns("LIAACGROUPNAME1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~LIABILITY" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~LIABILITY1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~ASSACGROUPNAME1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~ASSET" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~ASSET1" & Dtcostid.Rows(i).Item(0).ToString.Trim)
                    .Width = gridView.Columns("LIAACGROUPNAME1" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("LIABILITY" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("LIABILITY1" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("ASSACGROUPNAME1" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("ASSET" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("ASSET1" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .HeaderText = Dtcostid.Rows(i).Item(1).ToString
                End With
                With .Columns("COLHEAD3" & DtCostid.Rows(i).Item(0).ToString.Trim)
                    .Width = gridView.Columns("COLHEAD3" & DtCostid.Rows(i).Item(0).ToString.Trim).Width
                    .Visible = gridView.Columns("COLHEAD3" & DtCostid.Rows(i).Item(0).ToString.Trim).Visible
                    .HeaderText = ""
                End With
                With .Columns("COLHEAD4" & DtCostid.Rows(i).Item(0).ToString.Trim)
                    .Width = gridView.Columns("COLHEAD4" & DtCostid.Rows(i).Item(0).ToString.Trim).Width
                    .Visible = gridView.Columns("COLHEAD4" & DtCostid.Rows(i).Item(0).ToString.Trim).Visible
                    .HeaderText = ""
                End With
            Next
            With .Columns("SCROLL")
                .Width = 20
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .HeaderText = ""
            End With
            gridViewHead.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 9, FontStyle.Bold)
            gridViewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
    End Function

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        If cmbCompany.Items.Count > 0 Then cmbCompany.SelectedIndex = 0
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        dtpDate.Value = DateTime.Today.Date
        gridView.DataSource = Nothing
        gridViewHead.DataSource = Nothing
        lblTitle.Text = ""
        Prop_Gets()
        cmbCompany.Focus()
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.RowCount > 0 Then
            BrightPosting.GExport.Post(Me.Name, "", lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridViewHead)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.RowCount > 0 Then
            BrightPosting.GExport.Post(Me.Name, "", lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridViewHead)
        End If
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles gridView.ColumnWidthChanged
        With gridViewHead
            If .ColumnCount > 0 Then
                For i As Integer = 0 To Dtcostid.Rows.Count - 1
                    .Columns("LIAACGROUPNAME1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~LIABILITY" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~LIABILITY1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~ASSACGROUPNAME1" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~ASSET" & Dtcostid.Rows(i).Item(0).ToString.Trim & "~ASSET1" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width = gridView.Columns("LIAACGROUPNAME1" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("LIABILITY" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("LIABILITY1" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("ASSACGROUPNAME1" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("ASSET" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width + gridView.Columns("ASSET1" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width
                    .Columns("COLHEAD3" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width = gridView.Columns("COLHEAD3" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width
                    .Columns("COLHEAD4" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width = gridView.Columns("COLHEAD4" & Dtcostid.Rows(i).Item(0).ToString.Trim).Width
                Next
                .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            End If
        End With
    End Sub

    Private Sub gridView_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar.ToString().ToUpper = "P" Then
            btnPrint_Click(Me, New EventArgs)
        ElseIf e.KeyChar.ToString().ToUpper = "X" Then
            btnExport_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub viewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles viewToolStripMenuItem.Click
        btnView_Click(Me, New EventArgs)
    End Sub

    Private Sub newToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles newToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub exitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmBalanceSheet_Properties
        obj.p_cmbCompany = cmbCompany.Text
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        SetSettingsObj(obj, Me.Name, GetType(frmBalanceSheet_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmBalanceSheet_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmBalanceSheet_Properties))
        cmbCompany.Text = obj.p_cmbCompany
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, cnCostName)
    End Sub

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridViewHead.HorizontalScrollingOffset = e.NewValue
        End If
        If gridViewHead.DataSource Is Nothing Then Exit Sub
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridViewHead.HorizontalScrollingOffset = e.NewValue
                gridViewHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridViewHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub btnView_Enter(sender As Object, e As EventArgs) Handles btnView.Enter

    End Sub
End Class

Public Class frmBalanceSheet_Properties
    Private cmbCompany As String = "ALL"
    Public Property p_cmbCompany() As String
        Get
            Return cmbCompany
        End Get
        Set(ByVal value As String)
            cmbCompany = value
        End Set
    End Property
    Private chkCmbCostCentre As New List(Of String)
    Public Property p_chkCmbCostCentre() As List(Of String)
        Get
            Return chkCmbCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCostCentre = value
        End Set
    End Property

End Class