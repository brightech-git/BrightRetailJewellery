Imports System.Data.OleDb
Public Class frmCatStockED
    Dim strSql As String = Nothing
    Dim dtTemp As New DataTable
    Dim ftrIssue As String
    Dim ftrReceipt As String
    Dim ftrStoneStr As String
    Dim dsGridView As New DataSet
    Dim cmd As OleDbCommand
    Dim dtCompany As New DataTable
    Dim dtCostCentre As New DataTable
    Dim dtGridView As New DataTable

    Private Sub frmCategoryStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            dtpFrom.Focus()
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        Me.Cursor = Cursors.Arrow
        gridView.DataSource = Nothing
        
        strSql = " IF OBJECT_ID('ED') IS NOT NULL DROP TABLE ED"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + " SELECT '" & cnTranFromDate.Date.AddDays(-1) & "' AS TRANDATE"
        strSql += vbCrLf + " ,STKTYPE,RTRIM(REMARK1) AS RMETAL,CATCODE,GRSWT AS OGRSWT"
        strSql += vbCrLf + " ,NETWT AS ONETWT,DWT AS ODIAWT,GEMWT AS OGEMWT,"
        strSql += vbCrLf + " 0 AS RGRSWT,0 AS RNETWT,0 AS RDWT,0 AS RGEMWT,"
        strSql += vbCrLf + " 0 AS IGRSWT,0 AS INETWT,0 AS SALAMT,0 AS VAT,0 AS IDWT,0 AS SALEDIAAMT,0 AS SALEDIAVAT,0 AS IGEMWT,0 AS SALEGEMAMT,0 AS SALEGEMVAT"
        strSql += vbCrLf + " INTO ED"
        strSql += vbCrLf + " FROM " & cnStockDb & "..OPENWEIGHT "
        strSql += vbCrLf + " WHERE CATCODE IN('00006','00040','00051','00048') "
        strSql += vbCrLf + " AND STOCKTYPE='C'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " /*RECEIPT GOLD*/"
        strSql += vbCrLf + " SELECT TRANDATE,STKTYPE,'G' AS RMETAL,CATCODE,"
        strSql += vbCrLf + " 0 AS OGRSWT,0 AS ONETWT,0 AS ODIAWT,0 AS OGEMWT,"
        strSql += vbCrLf + " GRSWT AS RGRSWT,NETWT AS RNETWT,"
        strSql += vbCrLf + " (SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE AS S WHERE S.ISSSNO = R.SNO AND STNITEMID = 9998) AS RDWT,"
        strSql += vbCrLf + " (SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE AS S WHERE S.ISSSNO = R.SNO AND STNITEMID = 108) AS RGEMWT,"
        strSql += vbCrLf + " 0 AS IGRSWT,0 AS INETWT,0 AS SALAMT,0 AS VAT,0 AS IDWT,0 AS SALEDIAAMT,0 AS SALEDIAVAT,0 AS IGEMWT,0 AS SALEGEMAMT,0 AS SALEGEMVAT"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS R WHERE CATCODE IN('00006','00040','00051','00048') AND ISNULL(CANCEL,'') <> 'Y' "
        strSql += vbCrLf + "  AND TRANDATE BETWEEN '" & cnTranFromDate & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "  AND SNO NOT IN(SELECT ISSSNO FROM " & cnStockDb & "..RECEIPTSTONE WHERE STNITEMID IN(9998,108))"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + "  /*GOLD WITH DIA RECEIPT*/"
        strSql += vbCrLf + " SELECT TRANDATE,STKTYPE,'GDIA' AS RMETAL,CATCODE,"
        strSql += vbCrLf + " 0 AS OGRSWT,0 AS ONETWT,0 AS ODIAWT,0 AS OGEMWT,"
        strSql += vbCrLf + " GRSWT AS RGRSWT,NETWT AS RNETWT,"
        strSql += vbCrLf + " (SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE AS S WHERE S.ISSSNO = R.SNO AND STNITEMID = 9998) AS RDWT,"
        strSql += vbCrLf + " (SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE AS S WHERE S.ISSSNO = R.SNO AND STNITEMID = 108) AS RGEMWT,"
        strSql += vbCrLf + " 0 AS IGRSWT,0 AS INETWT,0 AS SALAMT,0 AS VAT,0 AS IDWT,0 AS SALEDIAAMT,0 AS SALEDIAVAT,0 AS IGEMWT,0 AS SALEGEMAMT,0 AS SALEGEMVAT"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS R WHERE CATCODE IN('00006','00040','00051','00048') AND ISNULL(CANCEL,'') <> 'Y'  AND TRANDATE BETWEEN '" & cnTranFromDate & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "  AND SNO IN(SELECT ISSSNO FROM " & cnStockDb & "..RECEIPTSTONE WHERE STNITEMID IN(9998))"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  /*GOLD WITH GEMS RECEIPT*/"
        strSql += vbCrLf + " SELECT TRANDATE,STKTYPE,'GGEM' AS RMETAL,CATCODE,"
        strSql += vbCrLf + " 0 AS OGRSWT,0 AS ONETWT,0 AS ODIAWT,0 AS OGEMWT,"
        strSql += vbCrLf + " GRSWT AS RGRSWT,NETWT AS RNETWT,"
        strSql += vbCrLf + " (SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE AS S WHERE S.ISSSNO = R.SNO AND STNITEMID = 9998) AS RDWT,"
        strSql += vbCrLf + " (SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE AS S WHERE S.ISSSNO = R.SNO AND STNITEMID = 108) AS RGEMWT,"
        strSql += vbCrLf + " 0 AS IGRSWT,0 AS INETWT,0 AS SALAMT,0 AS VAT,0 AS IDWT,0 AS SALEDIAAMT,0 AS SALEDIAVAT,0 AS IGEMWT,0 AS SALEGEMAMT,0 AS SALEGEMVAT"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..RECEIPT AS R WHERE CATCODE IN('00006','00040','00051','00048') AND ISNULL(CANCEL,'') <> 'Y' AND TRANDATE BETWEEN '" & cnTranFromDate & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "  AND SNO IN(SELECT ISSSNO FROM " & cnStockDb & "..RECEIPTSTONE WHERE STNITEMID IN(108))"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " /*ISSUE GOLD*/"
        strSql += vbCrLf + "  SELECT TRANDATE,STKTYPE,'G' AS RMETAL,CATCODE,"
        strSql += vbCrLf + " 0 AS OGRSWT,0 AS ONETWT,0 AS ODIAWT,0 AS OGEMWT,"
        strSql += vbCrLf + " 0 AS RGRSWT,0 AS RNETWT,0 AS RDWT,0 AS RGEMWT"
        strSql += vbCrLf + " , GRSWT AS  IGRSWT,NETWT AS INETWT"
        strSql += vbCrLf + " ,AMOUNT+(SELECT ISNULL(SUM(TAXAMOUNT),0) FROM " & cnStockDb & "..TAXTRAN WHERE ISSSNO=R.SNO AND TAXID='ED' AND TRANTYPE='SA') AS SALAMT"
        strSql += vbCrLf + " ,TAX AS VAT,"
        strSql += vbCrLf + " (SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE AS S WHERE S.ISSSNO = R.SNO AND STNITEMID = 9998) AS IDWT,"
        strSql += vbCrLf + " 0 AS SALEDIAAMT,0 AS SALEDIAVAT,"
        strSql += vbCrLf + " (SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE AS S WHERE S.ISSSNO = R.SNO AND STNITEMID = 108) AS IGEMWT"
        strSql += vbCrLf + " ,0 AS SALEGEMAMT,0 AS SALEGEMVAT"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS R WHERE CATCODE IN('00006','00040','00051','00048') AND ISNULL(CANCEL,'') <> 'Y' AND ISNULL(TRANTYPE,'') NOT IN('MI') "
        strSql += vbCrLf + "  AND TRANDATE BETWEEN '" & cnTranFromDate & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "    AND SNO NOT IN(SELECT ISSSNO FROM " & cnStockDb & "..ISSSTONE WHERE STNITEMID IN(9998,108))"
        strSql += vbCrLf + "   UNION ALL"
        strSql += vbCrLf + "  /*ISSUE GOLD WITH DIA*/"
        strSql += vbCrLf + "  SELECT TRANDATE,STKTYPE,'GDIA' AS RMETAL,CATCODE,"
        strSql += vbCrLf + " 0 AS OGRSWT,0 AS ONETWT,0 AS ODIAWT,0 AS OGEMWT,"
        strSql += vbCrLf + " 0 AS RGRSWT,0 AS RNETWT,0 AS RDWT,0 AS RGEMWT, GRSWT AS  IGRSWT,NETWT AS INETWT"
        strSql += vbCrLf + " ,AMOUNT+(SELECT ISNULL(SUM(TAXAMOUNT),0) FROM " & cnStockDb & "..TAXTRAN WHERE ISSSNO=R.SNO AND TAXID='ED' AND TRANTYPE='SA') AS SALAMT"
        strSql += vbCrLf + " ,TAX AS VAT,"
        strSql += vbCrLf + " (SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE AS S WHERE S.ISSSNO = R.SNO AND STNITEMID = 9998) AS IDWT,"
        strSql += vbCrLf + " (SELECT SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE AS S WHERE S.ISSSNO = R.SNO AND STNITEMID = 9998) AS SALEDIAAMT,"
        strSql += vbCrLf + " (SELECT SUM(TAX) FROM " & cnStockDb & "..ISSSTONE AS S WHERE S.ISSSNO = R.SNO AND STNITEMID = 9998) AS SALEDIAVAT,"
        strSql += vbCrLf + " (SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE AS S WHERE S.ISSSNO = R.SNO AND STNITEMID = 108) AS IGEMWT,"
        strSql += vbCrLf + " (SELECT SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE AS S WHERE S.ISSSNO = R.SNO AND STNITEMID = 108) AS SALEGEMAMT,"
        strSql += vbCrLf + " (SELECT SUM(TAX) FROM " & cnStockDb & "..ISSSTONE AS S WHERE S.ISSSNO = R.SNO AND STNITEMID = 108) AS SALEGEMVAT"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS R WHERE CATCODE IN('00006','00040','00051','00048') AND ISNULL(CANCEL,'') <> 'Y' AND ISNULL(TRANTYPE,'') NOT IN('MI') "
        strSql += vbCrLf + "  AND TRANDATE BETWEEN '" & cnTranFromDate & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "   AND SNO IN(SELECT ISSSNO FROM " & cnStockDb & "..ISSSTONE WHERE STNITEMID IN(9998))"
        If cnStockDb <> "SJGT1617" Then
            strSql += vbCrLf + "   UNION ALL"
            strSql += vbCrLf + "  /*ISSUE LOOSE DIAMOND*/"
            strSql += vbCrLf + "  SELECT TRANDATE,STKTYPE,'GDIA' AS RMETAL,CATCODE,"
            strSql += vbCrLf + " 0 AS OGRSWT,0 AS ONETWT,0 AS ODIAWT,0 AS OGEMWT,"
            strSql += vbCrLf + " 0 AS RGRSWT,0 AS RNETWT,0 AS RDWT,0 AS RGEMWT, 0 AS  IGRSWT,0 AS INETWT"
            strSql += vbCrLf + " ,AMOUNT+(SELECT ISNULL(SUM(TAXAMOUNT),0) FROM " & cnStockDb & "..TAXTRAN WHERE ISSSNO=R.SNO AND TAXID='ED' AND TRANTYPE='SA') AS SALAMT"
            strSql += vbCrLf + " ,TAX AS VAT,"
            strSql += vbCrLf + " GRSWT AS IDWT,"
            strSql += vbCrLf + " AMOUNT AS SALEDIAAMT,"
            strSql += vbCrLf + " TAX AS SALEDIAVAT,"
            strSql += vbCrLf + " 0 AS IGEMWT,"
            strSql += vbCrLf + " 0 AS SALEGEMAMT,"
            strSql += vbCrLf + " 0 AS SALEGEMVAT"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS R WHERE CATCODE IN('00010') AND ISNULL(CANCEL,'') <> 'Y' AND ISNULL(TRANTYPE,'') NOT IN('MI') "
            strSql += vbCrLf + "  AND TRANDATE BETWEEN '" & cnTranFromDate & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "   AND SNO NOT IN(SELECT ISSSNO FROM " & cnStockDb & "..ISSSTONE )"
            strSql += vbCrLf + "   UNION ALL"
            strSql += vbCrLf + "  /*ISSUE LOOSE GEMS*/"
            strSql += vbCrLf + "  SELECT TRANDATE,STKTYPE,'GGEM' AS RMETAL,CATCODE,"
            strSql += vbCrLf + " 0 AS OGRSWT,0 AS ONETWT,0 AS ODIAWT,0 AS OGEMWT,"
            strSql += vbCrLf + " 0 AS RGRSWT,0 AS RNETWT,0 AS RDWT,0 AS RGEMWT, 0 AS  IGRSWT,0 AS INETWT"
            strSql += vbCrLf + " ,AMOUNT+(SELECT ISNULL(SUM(TAXAMOUNT),0) FROM " & cnStockDb & "..TAXTRAN WHERE ISSSNO=R.SNO AND TAXID='ED' AND TRANTYPE='SA') AS SALAMT"
            strSql += vbCrLf + " ,TAX AS VAT,"
            strSql += vbCrLf + " 0 AS IDWT,"
            strSql += vbCrLf + " 0 AS SALEDIAAMT,"
            strSql += vbCrLf + " 0 AS SALEDIAVAT,"
            strSql += vbCrLf + " GRSWT AS IGEMWT,"
            strSql += vbCrLf + " AMOUNT AS SALEGEMAMT,"
            strSql += vbCrLf + " TAX AS SALEGEMVAT"
            strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS R WHERE CATCODE IN('00067') AND ISNULL(CANCEL,'') <> 'Y' AND ISNULL(TRANTYPE,'') NOT IN('MI') "
            strSql += vbCrLf + "  AND TRANDATE BETWEEN '" & cnTranFromDate & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "   AND SNO NOT IN(SELECT ISSSNO FROM " & cnStockDb & "..ISSSTONE )"
        End If
        strSql += vbCrLf + "   UNION ALL"
        strSql += vbCrLf + "  /*ISSUE GOLD WITH GEMS*/"
        strSql += vbCrLf + "  SELECT TRANDATE,STKTYPE,'GGEM' AS RMETAL,CATCODE,"
        strSql += vbCrLf + " 0 AS OGRSWT,0 AS ONETWT,0 AS ODIAWT,0 AS OGEMWT,"
        strSql += vbCrLf + " 0 AS RGRSWT,0 AS RNETWT,0 AS RDWT,0 AS RGEMWT, GRSWT AS  IGRSWT,NETWT AS INETWT"
        strSql += vbCrLf + " ,AMOUNT+(SELECT ISNULL(SUM(TAXAMOUNT),0) FROM " & cnStockDb & "..TAXTRAN WHERE ISSSNO=R.SNO AND TAXID='ED' AND TRANTYPE='SA') AS SALAMT"
        strSql += vbCrLf + " ,TAX AS VAT,"
        strSql += vbCrLf + " (SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE AS S WHERE S.ISSSNO = R.SNO AND STNITEMID = 9998) AS IDWT,"
        strSql += vbCrLf + " (SELECT SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE AS S WHERE S.ISSSNO = R.SNO AND STNITEMID = 9998) AS SALEDIAAMT,"
        strSql += vbCrLf + " (SELECT SUM(TAX) FROM " & cnStockDb & "..ISSSTONE AS S WHERE S.ISSSNO = R.SNO AND STNITEMID = 9998) AS SALEDIAVAT,"
        strSql += vbCrLf + " (SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE AS S WHERE S.ISSSNO = R.SNO AND STNITEMID = 108) AS IGEMWT,"
        strSql += vbCrLf + " (SELECT SUM(STNAMT) FROM " & cnStockDb & "..ISSSTONE AS S WHERE S.ISSSNO = R.SNO AND STNITEMID = 108) AS SALEGEMAMT,"
        strSql += vbCrLf + " (SELECT SUM(TAX) FROM " & cnStockDb & "..ISSSTONE AS S WHERE S.ISSSNO = R.SNO AND STNITEMID = 108) AS SALEGEMVAT"
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS R WHERE CATCODE IN('00006','00040','00051','00048') AND ISNULL(CANCEL,'') <> 'Y' AND "
        strSql += vbCrLf + "  ISNULL(TRANTYPE,'') NOT IN('MI') AND TRANDATE BETWEEN '" & cnTranFromDate & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "   AND SNO IN(SELECT ISSSNO FROM " & cnStockDb & "..ISSSTONE WHERE STNITEMID IN(108))"
        'strSql += vbCrLf + "   UNION ALL"
        'strSql += vbCrLf + "  /*ISSUE GOLD WITH GEMS*/"
        'strSql += vbCrLf + "  SELECT TRANDATE,STKTYPE,'GGEM' AS RMETAL,CATCODE,"
        'strSql += vbCrLf + " 0 AS OGRSWT,0 AS ONETWT,0 AS ODIAWT,0 AS OGEMWT,"
        'strSql += vbCrLf + " 0 AS RGRSWT,0 AS RNETWT,0 AS RDWT,0 AS RGEMWT, GRSWT AS  IGRSWT,NETWT AS INETWT"
        'strSql += vbCrLf + " ,AMOUNT+(SELECT ISNULL(SUM(TAXAMOUNT),0) FROM " & cnStockDb & "..TAXTRAN WHERE SNO=R.SNO AND TAXID='ED') AS SALAMT"
        'strSql += vbCrLf + " ,TAX AS VAT,"
        'strSql += vbCrLf + " 0 AS IDWT,"
        'strSql += vbCrLf + " 0 AS SALEDIAAMT,"
        'strSql += vbCrLf + " 0 AS SALEDIAVAT,"
        'strSql += vbCrLf + " GRSWT AS IGEMWT,"
        'strSql += vbCrLf + " AMOUNT+(SELECT ISNULL(SUM(TAXAMOUNT),0) FROM " & cnStockDb & "..TAXTRAN WHERE SNO=R.SNO AND TAXID='ED') AS SALEGEMAMT,"
        'strSql += vbCrLf + " TAX AS SALEGEMVAT"
        'strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS R WHERE CATCODE IN('00067') AND ISNULL(CANCEL,'') <> 'Y' AND "
        'strSql += vbCrLf + " ISNULL(TRANTYPE,'') NOT IN('MI') AND TRANDATE BETWEEN '" & cnTranFromDate & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        'strSql += vbCrLf + " AND SNO NOT IN(SELECT ISSSNO FROM " & cnStockDb & "..ISSSTONE WHERE STNITEMID IN(108))"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " UPDATE ED SET STKTYPE='T' WHERE ISNULL(STKTYPE,'')=''"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()


        strSql = vbCrLf + " /*REPORT*/"
        strSql += vbCrLf + " IF OBJECT_ID('EDFINAL" & systemId & "') IS NOT NULL DROP TABLE EDFINAL" & systemId & ""
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " SELECT "
        strSql += vbCrLf + " UPPER(CONVERT(VARCHAR(1000),DATENAME(MONTH,TRANDATE)))PARTICULAR,"
        strSql += vbCrLf + " YEAR(TRANDATE) AS YEAR ,MONTH(TRANDATE) AS MON,"
        strSql += vbCrLf + " CONVERT(SMALLDATETIME,CONVERT(VARCHAR,YEAR(TRANDATE))+'-'+CONVERT(VARCHAR,MONTH(TRANDATE))+'-01') TRANDATE,"
        strSql += vbCrLf + " STKTYPE,RMETAL,"
        strSql += vbCrLf + " SUM(ISNULL(OGRSWT,0)) AS OGRSWT,"
        strSql += vbCrLf + " /*SUM(ISNULL(ONETWT,0)) AS ONETWT,*/"
        strSql += vbCrLf + " SUM(ISNULL(ODIAWT,0)) AS ODWT,"
        strSql += vbCrLf + " SUM(ISNULL(OGEMWT,0)) AS OGEMWT,"
        strSql += vbCrLf + " SUM(ISNULL(RGRSWT,0)) AS RGRSWT,"
        strSql += vbCrLf + " /*SUM(ISNULL(RNETWT,0)) AS RNETWT,*/"
        strSql += vbCrLf + " SUM(ISNULL(RDWT,0)) AS RDWT,"
        strSql += vbCrLf + " SUM(ISNULL(RGEMWT,0)) AS RGEMWT,"
        strSql += vbCrLf + " SUM(ISNULL(IGRSWT,0)) AS IGRSWT,"
        strSql += vbCrLf + " /*SUM(ISNULL(INETWT,0)) AS INETWT,*/"
        strSql += vbCrLf + " SUM(ISNULL(IDWT,0)) AS IDWT,"
        strSql += vbCrLf + " SUM(ISNULL(IGEMWT,0)) AS IGEMWT,"

        strSql += vbCrLf + " CONVERT(NUMERIC(15,3),NULL) AS CGRSWT,"
        strSql += vbCrLf + " /*CONVERT(NUMERIC(15,3),NULL) AS CNETWT,*/"
        strSql += vbCrLf + " CONVERT(NUMERIC(15,3),NULL) AS CDWT,"
        strSql += vbCrLf + " CONVERT(NUMERIC(15,3),NULL) AS CGEMWT,"

        strSql += vbCrLf + " SUM(ISNULL(SALAMT,0)) AS SALAMT,"
        strSql += vbCrLf + " SUM(ISNULL(VAT,0)) AS VAT,"
        strSql += vbCrLf + " SUM(ISNULL(SALEDIAAMT,0)) AS SALEDIAAMT,"
        strSql += vbCrLf + " SUM(ISNULL(SALEDIAVAT,0)) AS SALEDIAVAT,"
        strSql += vbCrLf + " SUM(ISNULL(SALEGEMAMT,0)) AS SALEGEMAMT,"
        strSql += vbCrLf + " SUM(ISNULL(SALEGEMVAT,0)) AS SALEGEMVAT"
        strSql += vbCrLf + " INTO EDFINAL" & systemId & ""
        strSql += vbCrLf + "  FROM ED "
        strSql += vbCrLf + "  /*WHERE MONTH(TRANDATE) = 1 */"
        strSql += vbCrLf + " GROUP BY MONTH(TRANDATE),YEAR(TRANDATE),STKTYPE,RMETAL,DATENAME(MONTH,TRANDATE)"
        strSql += vbCrLf + " ORDER BY YEAR(TRANDATE),MONTH(TRANDATE),RMETAL,STKTYPE"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " DECLARE @OGRSWT AS NUMERIC(15,3),@ODIAWT AS NUMERIC(15,3),@OGEMWT AS NUMERIC(15,3)"
        strSql += vbCrLf + " DECLARE @RGRSWT AS NUMERIC(15,3),@RDWT AS NUMERIC(15,3),@RGEMWT AS NUMERIC(15,3)"
        strSql += vbCrLf + " DECLARE @IGRSWT AS NUMERIC(15,3),@IDWT AS NUMERIC(15,3),@IGEMWT AS NUMERIC(15,3)"
        strSql += vbCrLf + " DECLARE @YEAR AS NUMERIC(15,3),@MONTH AS NUMERIC(15,3),@STKTYPE AS VARCHAR(100),@BSTKTYPE AS VARCHAR(100)"
        strSql += vbCrLf + " DECLARE @BGRSWT AS NUMERIC(15,3),@BDIAWT AS NUMERIC(15,3),@BGEMWT AS NUMERIC(15,3)"
        strSql += vbCrLf + " DECLARE @QRY AS VARCHAR(8000)"
        strSql += vbCrLf + " DECLARE @CNT AS INT"
        strSql += vbCrLf + " DECLARE CUR CURSOR FOR "
        strSql += vbCrLf + " SELECT OGRSWT,ODWT,OGEMWT,RGRSWT,RDWT,RGEMWT,IGRSWT,IDWT,IGEMWT,YEAR,MON,STKTYPE+RMETAL TYPE FROM EDFINAL" & systemId & " ORDER BY RMETAL,STKTYPE,YEAR,MON"
        strSql += vbCrLf + " OPEN CUR"
        strSql += vbCrLf + " SET @BGRSWT=0"
        strSql += vbCrLf + " SET @BDIAWT=0"
        strSql += vbCrLf + " SET @BGEMWT=0"
        strSql += vbCrLf + " SET @CNT=0"
        strSql += vbCrLf + " FETCH NEXT FROM CUR INTO @OGRSWT,@ODIAWT,@OGEMWT,@RGRSWT,@RDWT,@RGEMWT,@IGRSWT,@IDWT,@IGEMWT,@YEAR,@MONTH,@STKTYPE"
        strSql += vbCrLf + " WHILE @@FETCH_STATUS=0"
        strSql += vbCrLf + " BEGIN"
        strSql += vbCrLf + " /*PRINT @BGRSWT"
        strSql += vbCrLf + " PRINT @BDIAWT"
        strSql += vbCrLf + " PRINT @BGEMWT*/"

        strSql += vbCrLf + " PRINT @RGRSWT"
        strSql += vbCrLf + " PRINT @RDWT"
        strSql += vbCrLf + " PRINT @RGEMWT"
        strSql += vbCrLf + " PRINT @IGRSWT"
        strSql += vbCrLf + " PRINT @IDWT"
        strSql += vbCrLf + " PRINT @IGEMWT"

        strSql += vbCrLf + " IF @CNT<>0"
        strSql += vbCrLf + " BEGIN"
        strSql += vbCrLf + " 	IF @BSTKTYPE<>@STKTYPE"
        strSql += vbCrLf + " 	BEGIN		"
        strSql += vbCrLf + " 		SET @BGRSWT=0"
        strSql += vbCrLf + " 		SET @BDIAWT=0"
        strSql += vbCrLf + " 		SET @BGEMWT=0"
        strSql += vbCrLf + "        SET @CNT=0"
        strSql += vbCrLf + " 	END"
        strSql += vbCrLf + " 	ELSE"
        strSql += vbCrLf + " 	BEGIN"
        strSql += vbCrLf + " 	    SET @QRY='UPDATE EDFINAL" & systemId & " SET OGRSWT='''+ CONVERT(VARCHAR,@BGRSWT) +''' WHERE YEAR='+ CONVERT(VARCHAR,@YEAR) +' AND MON='+ CONVERT(VARCHAR,@MONTH) +' AND STKTYPE+RMETAL='''+ @STKTYPE +''''"
        strSql += vbCrLf + " 	    PRINT @QRY"
        strSql += vbCrLf + " 	    EXEC (@QRY)"
        strSql += vbCrLf + " 	    SET @QRY='UPDATE EDFINAL" & systemId & " SET ODWT='''+ CONVERT(VARCHAR,@BDIAWT) +''' WHERE YEAR='+ CONVERT(VARCHAR,@YEAR) +' AND MON='+ CONVERT(VARCHAR,@MONTH) +' AND STKTYPE+RMETAL='''+ @STKTYPE +''''"
        strSql += vbCrLf + " 	    PRINT @QRY"
        strSql += vbCrLf + " 	    EXEC (@QRY)"
        strSql += vbCrLf + " 	    SET @QRY='UPDATE EDFINAL" & systemId & " SET OGEMWT='''+ CONVERT(VARCHAR,@BGEMWT) +''' WHERE YEAR='+ CONVERT(VARCHAR,@YEAR) +' AND MON='+ CONVERT(VARCHAR,@MONTH) +' AND STKTYPE+RMETAL='''+ @STKTYPE +''''"
        strSql += vbCrLf + " 	    PRINT @QRY"
        strSql += vbCrLf + " 	    EXEC (@QRY)"
        strSql += vbCrLf + " 	END"
        strSql += vbCrLf + " END	"

        strSql += vbCrLf + " SET @QRY='UPDATE EDFINAL" & systemId & " SET CGRSWT=OGRSWT+RGRSWT-IGRSWT WHERE YEAR='+ CONVERT(VARCHAR,@YEAR) +' AND MON='+ CONVERT(VARCHAR,@MONTH) +' AND STKTYPE+RMETAL='''+ @STKTYPE +''''"
        strSql += vbCrLf + " PRINT @QRY"
        strSql += vbCrLf + " EXEC (@QRY)"
        strSql += vbCrLf + " SET @QRY='UPDATE EDFINAL" & systemId & " SET CDWT=ODWT+RDWT-IDWT WHERE YEAR='+ CONVERT(VARCHAR,@YEAR) +' AND MON='+ CONVERT(VARCHAR,@MONTH) +' AND STKTYPE+RMETAL='''+ @STKTYPE +''''"
        strSql += vbCrLf + " PRINT @QRY"
        strSql += vbCrLf + " EXEC (@QRY)"
        strSql += vbCrLf + " SET @QRY='UPDATE EDFINAL" & systemId & " SET CGEMWT=OGEMWT+RGEMWT-IGEMWT WHERE YEAR='+ CONVERT(VARCHAR,@YEAR) +' AND MON='+ CONVERT(VARCHAR,@MONTH) +' AND STKTYPE+RMETAL='''+ @STKTYPE +''''"
        strSql += vbCrLf + " PRINT @QRY"
        strSql += vbCrLf + " EXEC (@QRY)"
        strSql += vbCrLf + " IF @CNT<>0"
        strSql += vbCrLf + " BEGIN"
        strSql += vbCrLf + " 	SET @BGRSWT=@BGRSWT+@RGRSWT-@IGRSWT"
        strSql += vbCrLf + " 	SET @BDIAWT=@BDIAWT+@RDWT-@IDWT"
        strSql += vbCrLf + " 	SET @BGEMWT=@BGEMWT+@RGEMWT-@IGEMWT"
        strSql += vbCrLf + " 	SET @BSTKTYPE=@STKTYPE"
        strSql += vbCrLf + " END"
        strSql += vbCrLf + " ELSE"
        strSql += vbCrLf + " BEGIN"
        strSql += vbCrLf + " 	SET @BGRSWT=@BGRSWT+@OGRSWT+@RGRSWT-@IGRSWT"
        strSql += vbCrLf + " 	SET @BDIAWT=@BDIAWT+@ODIAWT+@RDWT-@IDWT"
        strSql += vbCrLf + " 	SET @BGEMWT=@BGEMWT+@OGEMWT+@RGEMWT-@IGEMWT"
        strSql += vbCrLf + " 	SET @BSTKTYPE=@STKTYPE"
        strSql += vbCrLf + " END"


        strSql += vbCrLf + " PRINT @BGRSWT"
        strSql += vbCrLf + " PRINT @BDIAWT"
        strSql += vbCrLf + " PRINT @BGEMWT"


        strSql += vbCrLf + " SET @CNT=@CNT+1"
        strSql += vbCrLf + " FETCH NEXT FROM CUR INTO @OGRSWT,@ODIAWT,@OGEMWT,@RGRSWT,@RDWT,@RGEMWT,@IGRSWT,@IDWT,@IGEMWT,@YEAR,@MONTH,@STKTYPE"
        strSql += vbCrLf + " END"
        strSql += vbCrLf + " CLOSE CUR"
        strSql += vbCrLf + " DEALLOCATE CUR"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()


        strSql = vbCrLf + " UPDATE EDFINAL" & systemId & " SET PARTICULAR='OPENING' WHERE ISNULL(PARTICULAR,'')='March'"
        strSql += vbCrLf + " AND TRANDATE NOT BETWEEN '" & cnTranFromDate & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " ALTER TABLE EDFINAL" & systemId & " ADD RESULT INT"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + " UPDATE EDFINAL" & systemId & " SET RESULT=3"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " INSERT INTO EDFINAL" & systemId & " (PARTICULAR,RESULT,STKTYPE,RMETAL)"
        strSql += vbCrLf + " SELECT TOP 1 'PLAIN GOLD EXCEMPTED',0,'E','G'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT TOP 1 'PLAIN GOLD MANUFACTURING',0,'M','G'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT TOP 1 'PLAIN GOLD TRADING',0,'T','G'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT TOP 1 'GOLD STUDDED DIAMOND EXCEMPTED',0,'E','GDIA'"
        strSql += vbCrLf + " FROM EDFINAL" & systemId & " WHERE RMETAL='GDIA' AND STKTYPE='E'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT TOP 1 'GOLD STUDDED GEMS EXCEMPTED',0,'E','GGEM'"
        strSql += vbCrLf + " FROM EDFINAL" & systemId & " WHERE RMETAL='GGEM' AND STKTYPE='E'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT TOP 1 'GOLD STUDDED DIAMOND TRADING',0,'T','GDIA'"
        strSql += vbCrLf + " FROM EDFINAL" & systemId & " WHERE RMETAL='GDIA' AND STKTYPE='T'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT TOP 1 'GOLD STUDDED GEMS TRADING',0,'T','GGEM'"
        strSql += vbCrLf + " FROM EDFINAL" & systemId & " WHERE RMETAL='GGEM' AND STKTYPE='T'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT TOP 1 'GOLD STUDDED DIAMOND MANUFACTURING',0,'M','GDIA'"
        strSql += vbCrLf + " FROM EDFINAL" & systemId & " WHERE RMETAL='GDIA' AND STKTYPE='M'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT TOP 1 'GOLD STUDDED GEMS MANUFACTURING',0,'M','GGEM'"
        strSql += vbCrLf + " FROM EDFINAL" & systemId & " WHERE RMETAL='GGEM' AND STKTYPE='M'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()



        strSql = vbCrLf + " UPDATE EDFINAL" & systemId & " SET OGRSWT=NULL WHERE OGRSWT=0"
        strSql += vbCrLf + " UPDATE EDFINAL" & systemId & " SET ODWT=NULL WHERE ODWT=0"
        strSql += vbCrLf + " UPDATE EDFINAL" & systemId & " SET OGEMWT=NULL WHERE OGEMWT=0"
        strSql += vbCrLf + " UPDATE EDFINAL" & systemId & " SET RGRSWT=NULL WHERE RGRSWT=0"
        strSql += vbCrLf + " UPDATE EDFINAL" & systemId & " SET RDWT=NULL WHERE RDWT=0"
        strSql += vbCrLf + " UPDATE EDFINAL" & systemId & " SET RGEMWT=NULL WHERE RGEMWT=0"
        strSql += vbCrLf + " UPDATE EDFINAL" & systemId & " SET IGRSWT=NULL WHERE IGRSWT=0"
        strSql += vbCrLf + " UPDATE EDFINAL" & systemId & " SET IDWT=NULL WHERE IDWT=0"
        strSql += vbCrLf + " UPDATE EDFINAL" & systemId & " SET IGEMWT=NULL WHERE IGEMWT=0"
        strSql += vbCrLf + " UPDATE EDFINAL" & systemId & " SET CDWT=NULL WHERE CDWT=0"
        strSql += vbCrLf + " UPDATE EDFINAL" & systemId & " SET CGEMWT=NULL WHERE CGEMWT=0"
        strSql += vbCrLf + " UPDATE EDFINAL" & systemId & " SET SALAMT=NULL WHERE SALAMT=0"
        strSql += vbCrLf + " UPDATE EDFINAL" & systemId & " SET VAT=NULL WHERE VAT=0"
        strSql += vbCrLf + " UPDATE EDFINAL" & systemId & " SET SALEDIAAMT=NULL WHERE SALEDIAAMT=0"
        strSql += vbCrLf + " UPDATE EDFINAL" & systemId & " SET SALEDIAVAT=NULL WHERE SALEDIAVAT=0"
        strSql += vbCrLf + " UPDATE EDFINAL" & systemId & " SET SALEGEMAMT=NULL WHERE SALEGEMAMT=0"
        strSql += vbCrLf + " UPDATE EDFINAL" & systemId & " SET SALEGEMVAT=NULL WHERE SALEGEMVAT=0"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " SELECT * FROM ( SELECT * FROM EDFINAL" & systemId & " "
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.AddMonths(-1).Date.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT * FROM EDFINAL" & systemId & " "
        strSql += vbCrLf + " WHERE RESULT='0'"
        strSql += vbCrLf + " )X WHERE 1=1"
        If rbtExem.Checked Then
            strSql += vbCrLf + " AND STKTYPE='E'"
        ElseIf rbtMfg.Checked Then
            strSql += vbCrLf + " AND STKTYPE='M'"
        ElseIf rbtTrading.Checked Then
            strSql += vbCrLf + " AND STKTYPE='T'"
        End If
        strSql += vbCrLf + " ORDER BY RMETAL,STKTYPE,YEAR,MON"

        dtGridView = New DataTable
        dsGridView = New DataSet
        gridView.DataSource = Nothing
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 2000
        da = New OleDbDataAdapter(cmd)
        da.Fill(dsGridView)
        dtGridView = dsGridView.Tables(0)
        If Not dtGridView.Rows.Count > 0 Then
            MsgBox("Record Not Found")
            lblTitle.Text = ""
            Exit Sub
        End If
        gridView.DefaultCellStyle = Nothing
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        gridView.DataSource = dtGridView
        GridViewFormat()
        AutoResizeToolStripMenuItem_Click(Me, New EventArgs)
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("PARTICULAR", GetType(String))
            .Columns.Add("OGRSWT~ODWT~OGEMWT", GetType(String))
            .Columns.Add("RGRSWT~RDWT~RGEMWT", GetType(String))
            .Columns.Add("IGRSWT~IDWT~IGEMWT", GetType(String))
            .Columns.Add("CGRSWT~CDWT~CGEMWT", GetType(String))
            .Columns.Add("SALAMT~VAT~SALEDIAAMT~SALEDIAVAT~SALEGEMAMT~SALEGEMVAT", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("PARTICULAR").Caption = ""
            .Columns("SCROLL").Caption = ""
        End With
        gridHead.DataSource = dtMergeHeader
        funcGridHeaderStyle()
        pnlHeading.Visible = True
        Dim title As String = Nothing
        title += " CATEGORY STOCK (ED) REPORT "
        title += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        title += IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " :" & chkCmbCostCentre.Text, "")
        lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        lblTitle.Text = title

        Dim colWid As Integer = 0
        For cnt As Integer = 0 To gridView.ColumnCount - 1
            If gridView.Columns(cnt).Visible Then colWid += gridView.Columns(cnt).Width
        Next
        With gridHead
            If colWid >= gridView.Width Then
                .Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                .Columns("SCROLL").Visible = False
            End If
        End With
        gridView.Focus()
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        pnlHeading.Visible = False
        gridView.DataSource = Nothing
        dsGridView.Clear()
        dtpFrom.Value = GetServerDate()
        dtpFrom.Focus()
        lblTitle.Text = ""

    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("RESULT").Value.ToString
                    Case "0"
                        .Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("PARTICULAR").Style.Font = reportHeadStyle.Font
                End Select
            End With
        Next
        With gridView
            .Columns("YEAR").Visible = False
            .Columns("MON").Visible = False
            .Columns("TRANDATE").Visible = False
            .Columns("STKTYPE").Visible = False
            .Columns("RMETAL").Visible = False
            .Columns("RESULT").Visible = False

            With .Columns("PARTICULAR")
                .HeaderText = "PARTICULAR"
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With

            With .Columns("OGRSWT")
                .HeaderText = "GRSWT"
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("RGRSWT")
                .HeaderText = "GRSWT"
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("IGRSWT")
                .HeaderText = "GRSWT"
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CGRSWT")
                .HeaderText = "GRSWT"
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With

            With .Columns("ODWT")
                .HeaderText = "DIAWT"
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("RDWT")
                .HeaderText = "DIAWT"
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("IDWT")
                .HeaderText = "DIAWT"
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CDWT")
                .HeaderText = "DIAWT"
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With

            With .Columns("OGEMWT")
                .HeaderText = "GEMWT"
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("RGEMWT")
                .HeaderText = "GEMWT"
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("IGEMWT")
                .HeaderText = "GEMWT"
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CGEMWT")
                .HeaderText = "GEMWT"
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With





            With .Columns("SALAMT")
                .HeaderText = "AMOUNT"
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("VAT")
                .HeaderText = "VAT"
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SALEDIAAMT")
                .HeaderText = "DIA_AMT"
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SALEDIAVAT")
                .HeaderText = "DIA_VAT"
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SALEGEMAMT")
                .HeaderText = "GEM_AMT"
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("SALEGEMVAT")
                .HeaderText = "GEM_VAT"
                .ReadOnly = True
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With

            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        End With
    End Function
    Function funcGridHeaderStyle() As Integer
        With gridHead
            With .Columns("PARTICULAR")
                If gridView.Columns.Contains("TRANNO") Then
                    .Width = gridView.Columns("PARTICULAR").Width
                Else
                    .Width = gridView.Columns("PARTICULAR").Width
                End If
                .HeaderText = ""
            End With
            With .Columns("OGRSWT~ODWT~OGEMWT")
                .HeaderText = "OPENING"
                .Width = gridView.Columns("OGRSWT").Width + gridView.Columns("ODWT").Width + gridView.Columns("OGEMWT").Width
            End With

            With .Columns("RGRSWT~RDWT~RGEMWT")
                .HeaderText = "RECEIPT"
                .Width = gridView.Columns("RGRSWT").Width + gridView.Columns("RDWT").Width + gridView.Columns("RGEMWT").Width
            End With

            With .Columns("IGRSWT~IDWT~IGEMWT")
                .HeaderText = "ISSUE"
                .Width = gridView.Columns("IGRSWT").Width + gridView.Columns("IDWT").Width + gridView.Columns("IGEMWT").Width
            End With

            With .Columns("CGRSWT~CDWT~CGEMWT")
                .HeaderText = "CLOSING"
                .Width = gridView.Columns("CGRSWT").Width + gridView.Columns("CDWT").Width + gridView.Columns("CGEMWT").Width
            End With

            With .Columns("SALAMT~VAT~SALEDIAAMT~SALEDIAVAT~SALEGEMAMT~SALEGEMVAT")
                .HeaderText = "SALES"
                .Width = gridView.Columns("SALAMT").Width + gridView.Columns("VAT").Width + gridView.Columns("SALEDIAAMT").Width
                .Width += gridView.Columns("SALEDIAVAT").Width + gridView.Columns("SALEGEMAMT").Width + gridView.Columns("SALEGEMVAT").Width
            End With

            With .Columns("SCROLL")
                .HeaderText = ""
                .Width = 0
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8.25!, FontStyle.Bold)
        End With
    End Function
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If gridView.RowCount > 0 Then
            If e.KeyChar = Chr(Keys.Enter) Then
                Dim rwIndex As Integer
                If gridView.RowCount = 1 Then
                    rwIndex = gridView.CurrentRow.Index
                Else
                    rwIndex = gridView.CurrentRow.Index - 1
                End If
                gridView.CurrentCell = gridView.Rows(rwIndex).Cells("OPENING")
            ElseIf e.KeyChar = Chr(Keys.Escape) Then

            End If
            If UCase(e.KeyChar) = "X" Then
                btnExcel_Click(Me, New EventArgs)
            End If
            If UCase(e.KeyChar) = "P" Then
                btnPrint_Click(Me, New EventArgs)
            End If
        End If
    End Sub

    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridHead)
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridHead)
        End If
    End Sub

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        pnlHeading.Size = New System.Drawing.Size(100, 21)

        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub frmCategoryStock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strSql = " SELECT 'ALL' COMPANYNAME"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        objGPack.FillCombo(strSql, CmbCompany, True)
        CmbCompany.Text = "ALL"

        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += vbCrLf + " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        btnNew_Click(Me, e)
    End Sub
    
    Private Sub AutoResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoResizeToolStripMenuItem.Click
        AutoResizeToolStripMenuItem.Checked = True
        If gridView.RowCount > 0 Then
            If AutoResizeToolStripMenuItem.Checked Then
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                gridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In gridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
        End If
    End Sub

    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        If gridHead Is Nothing Then Exit Sub
        If Not gridHead.Columns.Count > 0 Then Exit Sub
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridHead.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridHead.HorizontalScrollingOffset = e.NewValue
                gridHead.Columns("SCROLL").Visible = CType(gridHead.Controls(0), HScrollBar).Visible
                gridHead.Columns("SCROLL").Width = CType(gridHead.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub
End Class


