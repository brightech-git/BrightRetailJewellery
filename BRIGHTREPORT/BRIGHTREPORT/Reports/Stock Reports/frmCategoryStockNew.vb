Imports System.Data.OleDb
Public Class frmCategoryStockNew
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

    Function funcTranNoWise() As Integer

        strSql = vbCrLf + " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "CATSTOCKTRANNO ')>0"
        strSql += vbCrLf + " DROP TABLE TEMP" & systemId & "CATSTOCKTRANNO "
        strSql += vbCrLf + " SELECT SPACE(50) PARTICULAR,' 'METALNAME,CATNAME,SUM(OPENING)OPENING,"
        strSql += vbCrLf + " RECEIPT,ISSUE"
        strSql += vbCrLf + " ,SUM(CLOSING)CLOSING,RATE,VALUE,CATCODE,RESULT,ACCODE,ITEMID,FRM"
        strSql += vbCrLf + " ,' 'YYEAR,' 'MMONTHID,' 'MMONTH,' 'DDATE"
        strSql += vbCrLf + " ,TRANDATE,TRANNO, ' 'COLHEAD ,IDENTITY(INT,1,1)SNO"
        strSql += vbCrLf + " INTO TEMP" & systemId & "CATSTOCKTRANNO "
        strSql += vbCrLf + " FROM TEMP" & systemId & "CATSTOCK AS T "
        strSql += vbCrLf + " GROUP BY CATNAME,CATCODE,RECEIPT,ISSUE,RESULT,RATE,VALUE,FRM,TRANNO,TRANDATE,ACCODE,ITEMID"
        If rbtnTranno.Checked Then
            strSql += vbCrLf + ",CASE WHEN TRANNO<>-1 THEN SNO END"
            strSql += vbCrLf + " ORDER BY CATNAME,TRANNO,TRANDATE  "
        Else
            strSql += vbCrLf + " ORDER BY CATNAME,(SELECT ACNAME FROM HONADMINDB..ACHEAD WHERE ACCODE=T.ACCODE),TRANNO,TRANDATE  "
        End If
        strSql += vbCrLf + " DECLARE @CATNAME VARCHAR(50)"
        strSql += vbCrLf + " DECLARE @TRANNO INT"
        strSql += vbCrLf + " DECLARE @OPENING NUMERIC(12,3)"
        strSql += vbCrLf + " DECLARE @ISSUE NUMERIC(12,3)"
        strSql += vbCrLf + " DECLARE @RECEIPT NUMERIC(12,3)"
        strSql += vbCrLf + " DECLARE @CLOSING NUMERIC(12,3)"
        strSql += vbCrLf + " DECLARE @RUNBAL NUMERIC(12,3)"
        strSql += vbCrLf + " DECLARE @PRESNO INT"
        strSql += vbCrLf + " DECLARE @SNO VARCHAR(50)"
        strSql += vbCrLf + " DECLARE CURCAT CURSOR"
        strSql += vbCrLf + " FOR SELECT CATNAME FROM TEMP" & systemId & "CATSTOCKTRANNO  GROUP BY CATNAME"
        strSql += vbCrLf + " OPEN CURCAT"
        strSql += vbCrLf + " WHILE 1=1"
        strSql += vbCrLf + " BEGIN"
        strSql += vbCrLf + " FETCH NEXT FROM CURCAT INTO @CATNAME"
        strSql += vbCrLf + " SELECT @TRANNO = NULL"
        strSql += vbCrLf + " SELECT @PRESNO = NULL"
        strSql += vbCrLf + " IF @@FETCH_STATUS = -1 BREAK"
        strSql += vbCrLf + " DECLARE CURMONTH CURSOR"
        strSql += vbCrLf + " FOR SELECT TRANNO,OPENING,ISSUE,RECEIPT,SNO FROM TEMP" & systemId & "CATSTOCKTRANNO  WHERE CATNAME = @CATNAME AND TRANNO<>-1 ORDER BY SNO"
        strSql += vbCrLf + " OPEN CURMONTH"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "CATSTOCKTRANNO  SET CLOSING = OPENING+RECEIPT-ISSUE WHERE TRANNO = -1 AND CATNAME = @CATNAME          "
        strSql += vbCrLf + " SELECT @RUNBAL = 0.000"
        strSql += vbCrLf + " SELECT @RUNBAL = OPENING+RECEIPT-ISSUE FROM TEMP" & systemId & "CATSTOCKTRANNO  WHERE TRANNO = -1 AND CATNAME = @CATNAME "
        strSql += vbCrLf + " PRINT @RUNBAL"
        strSql += vbCrLf + " WHILE 1 = 1"
        strSql += vbCrLf + " BEGIN"
        strSql += vbCrLf + " FETCH NEXT FROM CURMONTH INTO @TRANNO,@OPENING,@ISSUE,@RECEIPT,@SNO"
        strSql += vbCrLf + " IF @@FETCH_STATUS = -1 BREAK"
        strSql += vbCrLf + " PRINT 'OP-' + CONVERT(VARCHAR,(@CLOSING))"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "CATSTOCKTRANNO  SET OPENING = @RUNBAL WHERE SNO = @SNO					"
        strSql += vbCrLf + " SELECT @RUNBAL = OPENING+@RECEIPT-@ISSUE FROM TEMP" & systemId & "CATSTOCKTRANNO  WHERE SNO=@SNO "
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "CATSTOCKTRANNO  SET CLOSING = @RUNBAL WHERE SNO = @SNO"
        strSql += vbCrLf + " PRINT 'SNO-' + CONVERT(VARCHAR,(@SNO)) + '-' + @CATNAME"
        strSql += vbCrLf + " PRINT 'RE-' + CONVERT(VARCHAR,(@RECEIPT))"
        strSql += vbCrLf + " PRINT 'IS-' + CONVERT(VARCHAR,(@ISSUE))"
        strSql += vbCrLf + " PRINT 'CL-' + CONVERT(VARCHAR,(@RUNBAL)) 					"
        strSql += vbCrLf + " SET @PRESNO = @SNO			"
        strSql += vbCrLf + " END"
        strSql += vbCrLf + " CLOSE CURMONTH"
        strSql += vbCrLf + " DEALLOCATE CURMONTH"
        strSql += vbCrLf + " END"
        strSql += vbCrLf + " CLOSE CURCAT"
        strSql += vbCrLf + " DEALLOCATE CURCAT "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Function funcMonthWise() As Integer
        '--MONGH WISE CATSTOCK
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "CATSTOCKMONTH')>0"
        strSql += vbCrLf + " DROP TABLE TEMP" & systemId & "CATSTOCKMONTH"
        ' Year Filtration
        strSql += vbCrLf + " DECLARE @frmDATE SMALLDATETIME "
        strSql += vbCrLf + " DECLARE @toDATE SMALLDATETIME "
        strSql += vbCrLf + " SELECT @frmDATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT @toDATE = '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        ' Year Filtration
        strSql += vbcrlf + " SELECT SPACE(50) PARTICULAR,METALNAME,CATNAME,SUM(OPENING)OPENING,SUM(RECEIPT)RECEIPT,SUM(ISSUE)ISSUE"
        strSql += vbcrlf + " ,SUM(CLOSING)CLOSING,RATE,VALUE,CATCODE,RESULT"
        strSql += vbcrlf + " ,FRM"
        strSql += vbcrlf + " ,YYEAR,MMONTHID,MMONTH,' 'DDATE"
        strSql += vbcrlf + " ,' 'TRANDATE,' 'TRANNO, ' 'COLHEAD "
        strSql += vbcrlf + " INTO TEMP" & systemId & "CATSTOCKMONTH"
        strSql += vbCrLf + " FROM TEMP" & systemId & "CATSTOCK"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN @frmDATE AND @toDATE"
        strSql += vbcrlf + " GROUP BY METALNAME,CATNAME,CATCODE,RESULT,RATE,VALUE,FRM,YYEAR,MMONTHID,MMONTH"
        strSql += vbcrlf + " ORDER BY CATNAME,YYEAR,MMONTHID"
        strSql += vbcrlf + " DECLARE @CATNAME VARCHAR(50)"
        strSql += vbcrlf + " DECLARE @MMONTHID INT"
        strSql += vbcrlf + " DECLARE @OPENING NUMERIC(12,3)"
        strSql += vbcrlf + " DECLARE @ISSUE NUMERIC(12,3)"
        strSql += vbcrlf + " DECLARE @RECEIPT NUMERIC(12,3)"
        strSql += vbcrlf + " DECLARE @CLOSING NUMERIC(12,3)"
        strSql += vbcrlf + " DECLARE @PREMMONTHID INT"
        strSql += vbcrlf + " DECLARE CURCAT CURSOR"
        strSql += vbcrlf + " FOR SELECT CATNAME FROM TEMP" & systemId & "CATSTOCKMONTH GROUP BY CATNAME"
        strSql += vbcrlf + " OPEN CURCAT"
        strSql += vbcrlf + " WHILE 1=1"
        strSql += vbcrlf + " BEGIN"
        strSql += vbcrlf + " 	FETCH NEXT FROM CURCAT INTO @CATNAME"
        strSql += vbcrlf + " 	SELECT @MMONTHID = NULL"
        strSql += vbcrlf + " 	SELECT @PREMMONTHID = NULL"
        strSql += vbcrlf + " 	IF @@FETCH_STATUS = -1 BREAK"
        strSql += vbcrlf + "         DECLARE CURMONTH CURSOR"
        strSql += vbcrlf + "         FOR SELECT MMONTHID,OPENING,ISSUE,RECEIPT,CLOSING FROM TEMP" & systemId & "CATSTOCKMONTH WHERE CATNAME = @CATNAME"
        strSql += vbcrlf + "         OPEN CURMONTH"
        strSql += vbcrlf + "         WHILE 1=1"
        strSql += vbcrlf + " 	BEGIN"
        strSql += vbcrlf + " 		SELECT @MMONTHID"
        strSql += vbcrlf + " 		UPDATE TEMP" & systemId & "CATSTOCKMONTH SET CLOSING = OPENING+RECEIPT-ISSUE WHERE MMONTHID = @MMONTHID AND CATNAME = @CATNAME"
        strSql += vbcrlf + "  		FETCH NEXT FROM CURMONTH INTO @MMONTHID,@OPENING,@ISSUE,@RECEIPT,@CLOSING"
        strSql += vbcrlf + " 		IF @PREMMONTHID <> @MMONTHID"
        strSql += vbcrlf + "                    BEGIN"
        strSql += vbcrlf + "                    UPDATE TEMP" & systemId & "CATSTOCKMONTH SET OPENING = (SELECT CLOSING FROM TEMP" & systemId & "CATSTOCKMONTH WHERE MMONTHID = @PREMMONTHID AND CATNAME = @CATNAME)"
        strSql += vbcrlf + "                    WHERE MMONTHID = @MMONTHID AND CATNAME = @CATNAME"
        strSql += vbcrlf + "                    END"
        strSql += vbcrlf + " 		SELECT @PREMMONTHID = @MMONTHID"
        strSql += vbcrlf + " 		IF @@FETCH_STATUS = -1 BREAK"
        strSql += vbcrlf + " 	END"
        strSql += vbcrlf + "         CLOSE CURMONTH"
        strSql += vbcrlf + "         DEALLOCATE CURMONTH"
        strSql += vbcrlf + " END"
        strSql += vbcrlf + " CLOSE CURCAT"
        strSql += vbcrlf + " DEALLOCATE CURCAT"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Function funcSummaryWise() As Integer
        '-------SUMMARY WISE
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "CATSTOCKSUMMARY')>0"
        strSql += vbcrlf + " DROP TABLE TEMP" & systemId & "CATSTOCKSUMMARY"
        strSql += vbcrlf + " SELECT SPACE(50) PARTICULAR,METALNAME,CATNAME,SUM(OPENING)OPENING,"
        strSql += vbcrlf + " SUM(RECEIPT)RECEIPT,SUM(ISSUE)ISSUE"
        strSql += vbCrLf + " ,SUM(CLOSING)CLOSING,RATE,VALUE VALUE,CATCODE,RESULT"
        strSql += vbcrlf + " ,FRM"
        strSql += vbcrlf + " ,' ' YYEAR,' 'MMONTHID,' 'MMONTH,' 'DDATE"
        strSql += vbcrlf + " ,' 'TRANDATE,' 'TRANNO, ' 'COLHEAD "
        strSql += vbcrlf + " INTO TEMP" & systemId & "CATSTOCKSUMMARY"
        strSql += vbcrlf + " FROM TEMP" & systemId & "CATSTOCK"
        strSql += vbCrLf + " GROUP BY METALNAME,CATNAME,CATCODE,RESULT,RATE,VALUE,FRM"
        'strSql += " HAVING SUM(OPENING) <> 0 OR  SUM(RECEIPT) <> 0 OR SUM(ISSUE) <> 0 OR SUM(CLOSING) <> 0 "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Function funcDateWise() As Integer
        '--TRANDATE WISE CATSTOCK
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "CATSTOCKTRANDATE')>0"
        strSql += vbcrlf + " DROP TABLE TEMP" & systemId & "CATSTOCKTRANDATE"
        strSql += vbcrlf + " SELECT SPACE(50) PARTICULAR,' 'METALNAME,CATNAME,SUM(OPENING)OPENING,"
        strSql += vbcrlf + " SUM(RECEIPT)RECEIPT,SUM(ISSUE)ISSUE"
        strSql += vbcrlf + " ,SUM(CLOSING)CLOSING,RATE,VALUE,CATCODE,RESULT"
        strSql += vbcrlf + " ,FRM"
        strSql += vbcrlf + " ,' ' YYEAR,' 'MMONTHID,' 'MMONTH,CONVERT(VARCHAR,TRANDATE,103)DDATE"
        strSql += vbcrlf + " ,TRANDATE,' 'TRANNO, ' 'COLHEAD "
        strSql += vbcrlf + " INTO TEMP" & systemId & "CATSTOCKTRANDATE "
        strSql += vbcrlf + " FROM TEMP" & systemId & "CATSTOCK"
        strSql += vbcrlf + " GROUP BY CATNAME,CATCODE,RESULT,RATE,VALUE,FRM,TRANDATE"
        strSql += vbcrlf + " ORDER BY CATNAME,TRANDATE"

        strSql += vbcrlf + " DECLARE @CATNAME VARCHAR(50)"
        strSql += vbcrlf + " DECLARE @TRANDATE SMALLDATETIME"
        strSql += vbcrlf + " DECLARE @OPENING NUMERIC(12,3)"
        strSql += vbcrlf + " DECLARE @ISSUE NUMERIC(12,3)"
        strSql += vbcrlf + " DECLARE @RECEIPT NUMERIC(12,3)"
        strSql += vbcrlf + " DECLARE @CLOSING NUMERIC(12,3)"
        strSql += vbcrlf + " DECLARE @PREDATE SMALLDATETIME"
        strSql += vbcrlf + " DECLARE CURCAT CURSOR"
        strSql += vbcrlf + " FOR SELECT CATNAME FROM TEMP" & systemId & "CATSTOCKTRANDATE GROUP BY CATNAME"
        strSql += vbcrlf + " OPEN CURCAT"
        strSql += vbcrlf + " WHILE 1=1"
        strSql += vbcrlf + " BEGIN"
        strSql += vbcrlf + " 	FETCH NEXT FROM CURCAT INTO @CATNAME"
        strSql += vbcrlf + "  	SELECT @TRANDATE = NULL"
        strSql += vbcrlf + " 	SELECT @PREDATE = NULL"
        strSql += vbcrlf + " 	IF @@FETCH_STATUS = -1 BREAK"
        strSql += vbcrlf + "         DECLARE CURTRANDATE CURSOR"
        strSql += vbcrlf + "         FOR SELECT TRANDATE,OPENING,ISSUE,RECEIPT,CLOSING FROM TEMP" & systemId & "CATSTOCKTRANDATE WHERE CATNAME = @CATNAME"
        strSql += vbcrlf + "         OPEN CURTRANDATE"
        strSql += vbcrlf + "         WHILE 1=1"
        strSql += vbcrlf + " 	BEGIN"
        strSql += vbcrlf + " 		UPDATE TEMP" & systemId & "CATSTOCKTRANDATE SET CLOSING = OPENING+RECEIPT-ISSUE WHERE TRANDATE = @TRANDATE AND CATNAME = @CATNAME"
        strSql += vbcrlf + "  		FETCH NEXT FROM CURTRANDATE INTO @TRANDATE,@OPENING,@ISSUE,@RECEIPT,@CLOSING"
        strSql += vbcrlf + " 		IF @PREDATE <> @TRANDATE"
        strSql += vbcrlf + "                    BEGIN"
        strSql += vbcrlf + "                    UPDATE TEMP" & systemId & "CATSTOCKTRANDATE SET OPENING = (SELECT CLOSING FROM TEMP" & systemId & "CATSTOCKTRANDATE WHERE TRANDATE = @PREDATE AND CATNAME = @CATNAME)"
        strSql += vbcrlf + "                    WHERE TRANDATE = @TRANDATE AND CATNAME = @CATNAME"
        strSql += vbcrlf + "                    END"
        strSql += vbcrlf + " 		SELECT @PREDATE = @TRANDATE"
        strSql += vbcrlf + " 		IF @@FETCH_STATUS = -1 BREAK"
        strSql += vbcrlf + " 	END"
        strSql += vbcrlf + "         CLOSE CURTRANDATE"
        strSql += vbcrlf + "         DEALLOCATE CURTRANDATE"

        strSql += vbcrlf + " END"
        strSql += vbcrlf + " CLOSE CURCAT"
        strSql += vbcrlf + " DEALLOCATE CURCAT"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Function funcIssueFiltration() As String
        Dim Qry As String = Nothing
        Qry += " AND I.TRANTYPE <> 'WOT'"
        If cmbCatName.Text <> "ALL" Then
            Qry += " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY "
            Qry += " WHERE CATNAME = '" & cmbCatName.Text & "')"
        End If
        If chkWithOtherIssue.Checked = False Then
            Qry += " AND I.TRANTYPE <> 'MI'"
        End If
        If cmbMetal.Text <> "ALL" Then
            Qry += " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY "
            Qry += " WHERE I.METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST "
            Qry += " WHERE METALNAME = '" & cmbMetal.Text & "'))"
        End If
        If cmbMetalType.Text <> "ALL" Then
            If cmbMetalType.Text = "ORNAMENT" Then
                Qry += " AND ISNULL((SELECT METALTYPE FROM " & cnAdminDb & "..PURITYMAST "
                Qry += " WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY "
                Qry += " WHERE CATCODE = I.CATCODE)),'') = 'O'"
            ElseIf cmbMetalType.Text = "METAL" Then
                Qry += " AND ISNULL((SELECT METALTYPE FROM " & cnAdminDb & "..PURITYMAST "
                Qry += " WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY "
                Qry += " WHERE CATCODE = I.CATCODE)),'') = 'M'"
            Else
                Qry += " AND ISNULL((SELECT METALTYPE FROM " & cnAdminDb & "..PURITYMAST "
                Qry += " WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY "
                Qry += " WHERE CATCODE = I.CATCODE)),'') = ''"
            End If
        End If
        Return Qry
    End Function

    Function funcReceiptFiltration() As String
        Dim Qry As String = Nothing
        Qry += " AND R.TRANTYPE <> 'WOT'"
        If cmbCatName.Text <> "ALL" Then
            Qry += " AND R.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY "
            Qry += " WHERE CATNAME = '" & cmbCatName.Text & "')"
        End If
        If cmbMetal.Text <> "ALL" Then
            Qry += " AND R.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY "
            Qry += " WHERE R.METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST "
            Qry += " WHERE METALNAME = '" & cmbMetal.Text & "'))"
        End If
        If cmbMetalType.Text <> "ALL" Then
            If cmbMetalType.Text = "ORNAMENT" Then
                Qry += " AND ISNULL((SELECT METALTYPE FROM " & cnAdminDb & "..PURITYMAST "
                Qry += " WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY "
                Qry += " WHERE CATCODE = R.CATCODE)),'') = 'O'"
            ElseIf cmbMetalType.Text = "METAL" Then
                Qry += " AND ISNULL((SELECT METALTYPE FROM " & cnAdminDb & "..PURITYMAST "
                Qry += " WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY "
                Qry += " WHERE CATCODE = R.CATCODE)),'') = 'M'"
            Else
                Qry += " AND ISNULL((SELECT METALTYPE FROM " & cnAdminDb & "..PURITYMAST "
                Qry += " WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY "
                Qry += " WHERE CATCODE = R.CATCODE)),'') = ''"
            End If
        End If
        Return Qry
    End Function

    Private Function funcCompanyFilt() As String
        Dim Str As String = ""
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            Str += " AND COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        Else
            Str += " AND COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
        End If
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            Str += " AND COSTID IN"
            Str += "(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
        End If
        Return Str
    End Function

    Private Sub frmCategoryStock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            dtpFrom.Focus()
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        gridView.DataSource = Nothing
        pnlHeading.Visible = False
        ftrIssue = funcIssueFiltration()
        ftrReceipt = funcReceiptFiltration()
        Dim CompanyFilt As String = funcCompanyFilt()
        Dim CompanyFilt1 As String
        Dim Gstflag As Boolean = funcGstView(dtpFrom.Value)
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "CATSTOCK')>0"
        strSql += vbCrLf + " DROP TABLE TEMP" & systemId & "CATSTOCK"
        strSql += vbCrLf + " DECLARE @frmDATE SMALLDATETIME "
        strSql += vbCrLf + " DECLARE @toDATE SMALLDATETIME "

        strSql += vbCrLf + " SELECT @frmDATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT @toDATE = '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT"
        strSql += vbCrLf + " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST "
        strSql += vbCrLf + " WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..CATEGORY "
        strSql += vbCrLf + " WHERE CATCODE=X.CATCODE))AS METALNAME"
        strSql += vbCrLf + " ,CASE WHEN FRM = '1LOOSE' AND (SELECT METALID FROM " & cnAdminDb & "..CATEGORY "
        strSql += vbCrLf + " WHERE CATCODE = X.CATCODE) IN ('D','T') THEN (SELECT CATNAME "
        strSql += vbCrLf + " FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = X.CATCODE) + '[L]'"
        strSql += vbCrLf + " WHEN FRM = '2STUD' AND (SELECT METALID FROM " & cnAdminDb & "..CATEGORY "
        strSql += vbCrLf + " WHERE CATCODE = X.CATCODE) IN ('D','T') THEN (SELECT CATNAME "
        strSql += vbCrLf + " FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = X.CATCODE)+ '[S]'"
        strSql += vbCrLf + " ELSE (SELECT " & IIf(Gstflag, "CATNAME", "VATNAME") & " FROM " & cnAdminDb & "..CATEGORY "
        strSql += vbCrLf + " WHERE CATCODE = X.CATCODE) END AS CATNAME"


        If rbtGrsWt.Checked = True Then
            strSql += vbCrLf + " ,ISNULL((CASE WHEN SEP = 'REC_OPENING' THEN GRSWT WHEN SEP = 'ISS_OPENING' THEN -1*GRSWT END),0)AS OPENING"
            strSql += vbCrLf + " ,ISNULL((CASE WHEN SEP = 'ISSUE' THEN GRSWT END),0)AS ISSUE"
            strSql += vbCrLf + " ,ISNULL((CASE WHEN SEP = 'RECEIPT' THEN GRSWT END),0)AS RECEIPT"
            strSql += vbCrLf + " ,ISNULL((CASE WHEN SEP IN ('REC_OPENING','RECEIPT') THEN GRSWT ELSE -1*GRSWT END),0)AS CLOSING"
        ElseIf rbtPurewt.Checked = True Then
            strSql += vbCrLf + " ,ISNULL((CASE WHEN SEP = 'REC_OPENING' THEN PUREWT WHEN SEP = 'ISS_OPENING' THEN -1*PUREWT END),0)AS OPENING"
            strSql += vbCrLf + " ,ISNULL((CASE WHEN SEP = 'ISSUE' THEN PUREWT END),0)AS ISSUE"
            strSql += vbCrLf + " ,ISNULL((CASE WHEN SEP = 'RECEIPT' THEN PUREWT END),0)AS RECEIPT"
            strSql += vbCrLf + " ,ISNULL((CASE WHEN SEP IN ('REC_OPENING','RECEIPT') THEN PUREWT ELSE -1*PUREWT END),0)AS CLOSING"
        Else 'NETWT
            strSql += vbCrLf + " ,ISNULL((CASE WHEN SEP = 'REC_OPENING' THEN NETWT WHEN SEP = 'ISS_OPENING' THEN -1*NETWT END),0)AS OPENING"
            strSql += vbCrLf + " ,ISNULL((CASE WHEN SEP = 'ISSUE' THEN NETWT END),0)AS ISSUE"
            strSql += vbCrLf + " ,ISNULL((CASE WHEN SEP = 'RECEIPT' THEN NETWT END),0)AS RECEIPT"
            strSql += vbCrLf + " ,ISNULL((CASE WHEN SEP IN ('REC_OPENING','RECEIPT') THEN NETWT ELSE -1*NETWT END),0)AS CLOSING"
        End If
        strSql += vbCrLf + " ,' 'RATE"
        strSql += vbCrLf + " ,' 'VALUE"
        strSql += vbCrLf + " ,CATCODE"
        strSql += vbCrLf + " ,1 RESULT"
        strSql += vbCrLf + " ,' ' COLHEAD"
        strSql += vbCrLf + " ,FRM"
        strSql += vbCrLf + " ,0 YYEAR,0 MMONTHID, 0 MMONTH,TRANDATE,SNO,TRANNO"
        strSql += vbCrLf + " ,CASE WHEN TRANNO=-1 THEN '' ELSE ACCODE END AS ACCODE"
        strSql += vbCrLf + " ,CASE WHEN TRANNO=-1 THEN 0 ELSE ITEMID END AS ITEMID"
        'strSql += vbCrLf + " ,ACCODE,ITEMID"
        strSql += vbCrLf + " INTO TEMP" & systemId & "CATSTOCK"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " '1LOOSE'FRM,'ISS_OPENING'SEP,I.CATCODE"
        If rbtGeneral.Checked = True Then
            strSql += vbCrLf + " ,(GRSWT)GRSWT"
            strSql += vbCrLf + " ,(NETWT)NETWT"
            strSql += vbCrLf + " ,(PUREWT)PUREWT"
        ElseIf rbtCarat.Checked = True Then
            strSql += vbCrLf + " ,(CASE WHEN STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN GRSWT * 5 ELSE GRSWT END)GRSWT"
            strSql += vbCrLf + " ,(CASE WHEN STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN NETWT * 5 ELSE NETWT END)NETWT"
            strSql += vbCrLf + " ,(CASE WHEN STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN PUREWT * 5 ELSE PUREWT END)PUREWT"
        Else
            strSql += vbCrLf + " ,(CASE WHEN STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN GRSWT / 5 ELSE GRSWT END)GRSWT"
            strSql += vbCrLf + " ,(CASE WHEN STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN NETWT / 5 ELSE NETWT END)NETWT"
            strSql += vbCrLf + " ,(CASE WHEN STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN PUREWT / 5 ELSE PUREWT END)PUREWT"
        End If
        strSql += vbCrLf + " ,@FRMDATE TRANDATE,SNO,-1 TRANNO,TRANSTATUS"
        strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
        strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH,I.ACCODE,I.ITEMID"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON I.CATCODE = C.CATCODE"
        strSql += vbCrLf + " WHERE TRANDATE < @frmDATE AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND ISNULL(I.TRANTYPE,'') NOT IN  ('IRC') "
        strSql += vbCrLf + " AND I.SNO NOT IN( SELECT ISSSNO FROM " & cnStockDb & "..ISSMETAL WHERE TRANDATE <  @FRMDATE AND I.TRANTYPE IN ('SA','OD'))"
        If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(I.TRANTYPE,'') NOT IN  ('AI') "
        strSql += vbCrLf + ftrIssue
        CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND I.COMPANYID")
        strSql += vbCrLf + CompanyFilt1

        strSql += vbCrLf + " UNION ALL"

        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " '1LOOSE'FRM,'REC_OPENING'SEP,R.CATCODE"
        If rbtGeneral.Checked = True Then
            strSql += vbCrLf + " ,(GRSWT)GRSWT"
            strSql += vbCrLf + " ,(NETWT)NETWT"
            strSql += vbCrLf + " ,(PUREWT)PUREWT"
        ElseIf rbtCarat.Checked = True Then
            strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN GRSWT * 5 ELSE GRSWT END)GRSWT"
            strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN NETWT * 5 ELSE NETWT END)NETWT"
            strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN PUREWT * 5 ELSE PUREWT END)PUREWT"
        Else
            strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN GRSWT / 5 ELSE GRSWT END)GRSWT"
            strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN NETWT / 5 ELSE NETWT END)NETWT"
            strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN PUREWT / 5 ELSE PUREWT END)PUREWT"
        End If
        strSql += vbCrLf + " ,@FRMDATE TRANDATE,SNO,-1 TRANNO,TRANSTATUS"
        strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
        strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH,R.ACCODE,R.ITEMID"
        strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS R"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON R.CATCODE = C.CATCODE"
        strSql += vbCrLf + " WHERE TRANDATE < @frmDATE AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND ISNULL(R.TRANTYPE,'') NOT IN  ('RRC') "
        If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(R.TRANTYPE,'') NOT IN  ('AR') "
        strSql += vbCrLf + ftrReceipt
        CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND R.COMPANYID")
        strSql += vbCrLf + CompanyFilt1



        ''*****************************OPENWEIGHT LOOSE
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " '1LOOSE'FRM,'REC_OPENING'SEP,R.CATCODE"
        If rbtGeneral.Checked = True Then
            strSql += vbCrLf + " ,(R.GRSWT)GRSWT"
            strSql += vbCrLf + " ,(NETWT)NETWT"
            strSql += vbCrLf + " ,(PUREWT)PUREWT"
        ElseIf rbtCarat.Checked = True Then
            strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN R.GRSWT * 5 ELSE R.GRSWT END)GRSWT"
            strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN NETWT * 5 ELSE NETWT END)NETWT"
            strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN PUREWT * 5 ELSE PUREWT END)PUREWT"
        Else
            strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN R.GRSWT / 5 ELSE R.GRSWT END)GRSWT"
            strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN NETWT / 5 ELSE NETWT END)NETWT"
            strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN PUREWT / 5 ELSE PUREWT END)PUREWT"
        End If
        strSql += vbCrLf + " ,@FRMDATE TRANDATE,SNO,-1 TRANNO,' 'TRANSTATUS"
        strSql += vbCrLf + " ,DATEPART(YEAR,@FRMDATE)YYEAR,DATEPART(MONTH,@FRMDATE)MMONTHID,"
        strSql += vbCrLf + " LEFT(DATENAME(MONTH,@FRMDATE),3)MMONTH,R.ACCODE,R.ITEMID"
        strSql += vbCrLf + " FROM " & cnStockDb & "..OPENWEIGHT AS R"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON R.CATCODE = C.CATCODE"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON ISNULL(R.ITEMID,'')=ISNULL(IM.ITEMID,'')"
        strSql += vbCrLf + " WHERE R.STOCKTYPE = 'C' AND ISNULL(IM.STUDDED,'')<>'S' AND ISNULL(R.TRANTYPE,'')='R'"
        'strSql += vbcrlf + " WHERE TRANDATE < @frmDATE AND ISNULL(CANCEL,'') = ''"
        'strSql += vbCrLf + Replace(ftrReceipt, "AND CATCODE", "AND R.CATCODE")
        'If cmbMetal.Text <> "ALL" Then
        '    strSql += vbCrLf + " AND R.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY "
        '    strSql += vbCrLf + " WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST "
        '    strSql += vbCrLf + " WHERE METALNAME = '" & cmbMetal.Text & "'))"
        'End If
        If cmbCatName.Text <> "ALL" And cmbCatName.Text <> "" Then
            strSql += vbCrLf + " AND R.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY "
            strSql += vbCrLf + " WHERE CATNAME = '" & cmbCatName.Text & "')"
        End If
        'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
        If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(R.TRANTYPE,'') NOT IN  ('AR') "
        'strSql += vbCrLf + ftrReceipt
        CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND R.COMPANYID")
        strSql += vbCrLf + CompanyFilt1
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " '1LOOSE'FRM,'ISS_OPENING'SEP,R.CATCODE"
        If rbtGeneral.Checked = True Then
            strSql += vbCrLf + " ,(R.GRSWT)GRSWT"
            strSql += vbCrLf + " ,(NETWT)NETWT"
            strSql += vbCrLf + " ,(PUREWT)PUREWT"
        ElseIf rbtCarat.Checked = True Then
            strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN R.GRSWT * 5 ELSE R.GRSWT END)GRSWT"
            strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN NETWT * 5 ELSE NETWT END)NETWT"
            strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN PUREWT * 5 ELSE PUREWT END)PUREWT"
        Else
            strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN R.GRSWT / 5 ELSE R.GRSWT END)GRSWT"
            strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN NETWT / 5 ELSE NETWT END)NETWT"
            strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN PUREWT / 5 ELSE PUREWT END)PUREWT"
        End If
        strSql += vbCrLf + " ,@FRMDATE TRANDATE,SNO,-1 TRANNO,' 'TRANSTATUS"
        strSql += vbCrLf + " ,DATEPART(YEAR,@FRMDATE)YYEAR,DATEPART(MONTH,@FRMDATE)MMONTHID,"
        strSql += vbCrLf + " LEFT(DATENAME(MONTH,@FRMDATE),3)MMONTH,R.ACCODE,R.ITEMID"
        strSql += vbCrLf + " FROM " & cnStockDb & "..OPENWEIGHT AS R"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON R.CATCODE = C.CATCODE"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON ISNULL(R.ITEMID,'')=ISNULL(IM.ITEMID,'')"
        strSql += vbCrLf + " WHERE R.STOCKTYPE = 'C' AND ISNULL(IM.STUDDED,'')<>'S' AND ISNULL(R.TRANTYPE,'')='I'"
        If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(R.TRANTYPE,'') NOT IN  ('AR') "
        'strSql += vbCrLf + ftrReceipt
        If cmbCatName.Text <> "ALL" And cmbCatName.Text <> "" Then
            strSql += vbCrLf + " AND R.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY "
            strSql += vbCrLf + " WHERE CATNAME = '" & cmbCatName.Text & "')"
        End If
        CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND R.COMPANYID")
        strSql += vbCrLf + CompanyFilt1


        ''******************************

        strSql += vbCrLf + " UNION ALL"

        strSql += vbCrLf + " SELECT '1LOOSE'FRM,'ISSUE'SEP,CATCODE ,GRSWT ,NETWT,PUREWT,TRANDATE,SNO,TRANNO,TRANSTATUS "
        strSql += vbCrLf + " ,YYEAR,MMONTHID,MMONTH,ACCODE,ITEMID FROM ("


        strSql += vbCrLf + " SELECT I.CATCODE"
        If rbtGeneral.Checked = True Then
            strSql += vbCrLf + " ,(GRSWT)GRSWT"
            strSql += vbCrLf + " ,(NETWT)NETWT"
            strSql += vbCrLf + " ,(PUREWT)PUREWT"
        ElseIf rbtCarat.Checked = True Then
            strSql += vbCrLf + " ,(CASE WHEN I.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN GRSWT * 5 ELSE GRSWT END)GRSWT"
            strSql += vbCrLf + " ,(CASE WHEN I.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN NETWT * 5 ELSE NETWT END)NETWT"
            strSql += vbCrLf + " ,(CASE WHEN I.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN PUREWT * 5 ELSE PUREWT END)PUREWT"
        Else
            strSql += vbCrLf + " ,(CASE WHEN I.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN GRSWT / 5 ELSE GRSWT END)GRSWT"
            strSql += vbCrLf + " ,(CASE WHEN I.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN NETWT / 5 ELSE NETWT END)NETWT"
            strSql += vbCrLf + " ,(CASE WHEN I.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN PUREWT / 5 ELSE PUREWT END)PUREWT"
        End If
        strSql += vbCrLf + " ,TRANDATE,SNO,TRANNO,TRANSTATUS"
        strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
        strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH,I.ACCODE,I.ITEMID"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON I.CATCODE = C.CATCODE"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN @frmDATE AND @toDATE AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND I.SNO NOT IN( SELECT ISSSNO FROM " & cnStockDb & "..ISSMETAL WHERE TRANDATE BETWEEN  @FRMDATE AND @TODATE AND I.TRANTYPE IN ('SA','OD'))"
        strSql += vbCrLf + " AND ISNULL(I.TRANTYPE,'') NOT IN  ('IRC') "
        strSql += vbCrLf + " AND ISNULL(I.TRANTYPE,'') NOT IN  ('OD') "
        If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(I.TRANTYPE,'') NOT IN  ('AI') "
        'strSql += vbcrlf + " AND ISNULL(TRANTYPE,'') NOT IN  ('OD','RD') "
        strSql += vbCrLf + ftrIssue
        'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
        CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND I.COMPANYID")
        strSql += vbCrLf + CompanyFilt1

        ''ORDER & REPAIR
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT  "
        strSql += vbCrLf + " ISNULL(I.CATCODE,'') CATCODE "
        If rbtGeneral.Checked = True Then
            strSql += vbCrLf + " ,GRSWT"
            strSql += vbCrLf + " ,NETWT"
            strSql += vbCrLf + " ,PUREWT"
        Else
            strSql += vbCrLf + " ,GRSWT * 5 GRSWT"
            strSql += vbCrLf + " ,NETWT * 5 NETWT"
            strSql += vbCrLf + " ,PUREWT * 5 PUREWT"
        End If
        strSql += vbCrLf + " ,TRANDATE,SNO,TRANNO,TRANSTATUS"
        strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
        strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH,I.ACCODE,0 ITEMID"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS I"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN @frmDATE AND @toDATE AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND RECPAY = 'P' "
        strSql += vbCrLf + Replace(ftrIssue, "I.METALID", "METALID")
        'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
        CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND I.COMPANYID")
        strSql += vbCrLf + CompanyFilt1
        strSql += vbCrLf + " ) X"

        strSql += vbCrLf + " UNION ALL"

        strSql += vbCrLf + " SELECT '1LOOSE'FRM,'RECEIPT'SEP,CATCODE ,GRSWT ,NETWT,PUREWT ,TRANDATE,SNO,TRANNO,TRANSTATUS "
        strSql += vbCrLf + " ,YYEAR,MMONTHID,MMONTH,ACCODE,ITEMID FROM ("
        strSql += vbCrLf + " SELECT R.CATCODE"
        If rbtGeneral.Checked = True Then
            strSql += vbCrLf + " ,GRSWT"
            strSql += vbCrLf + " ,NETWT"
            strSql += vbCrLf + " ,PUREWT"
        ElseIf rbtCarat.Checked = True Then
            strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN GRSWT * 5 ELSE GRSWT END)GRSWT"
            strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN NETWT * 5 ELSE NETWT END)NETWT"
            strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN PUREWT * 5 ELSE PUREWT END)PUREWT"
        Else
            strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN GRSWT / 5 ELSE GRSWT END)GRSWT"
            strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN NETWT / 5 ELSE NETWT END)NETWT"
            strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN PUREWT / 5 ELSE PUREWT END)PUREWT"
        End If
        strSql += vbCrLf + " ,TRANDATE,SNO,TRANNO,TRANSTATUS"
        strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
        strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH,R.ACCODE,R.ITEMID"
        strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS R"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON R.CATCODE = C.CATCODE"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN @frmDATE AND @toDATE AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND ISNULL(R.TRANTYPE,'') NOT IN  ('RRC') "
        If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(R.TRANTYPE,'') NOT IN  ('AR') "
        ''strSql += vbcrlf + " AND ISNULL(TRANTYPE,'') <> 'RPU' "
        strSql += vbCrLf + Replace(ftrReceipt, "AND CATCODE", "AND R.CATCODE")
        'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
        CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND R.COMPANYID")
        strSql += vbCrLf + CompanyFilt1
        ''ORDER & REPAIR
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT  "
        strSql += vbCrLf + " ISNULL(I.CATCODE,'') CATCODE "
        If rbtGeneral.Checked = True Then
            strSql += vbCrLf + " ,GRSWT"
            strSql += vbCrLf + " ,NETWT"
            strSql += vbCrLf + " ,PUREWT"
        Else
            strSql += vbCrLf + " ,GRSWT * 5 GRSWT"
            strSql += vbCrLf + " ,NETWT * 5 NETWT"
            strSql += vbCrLf + " ,PUREWT * 5 PUREWT"
        End If
        strSql += vbCrLf + " ,TRANDATE,SNO,TRANNO,TRANSTATUS"
        strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
        strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH,I.ACCODE,0 ITEMID"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS I"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN @frmDATE AND @toDATE AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND RECPAY = 'R' AND BATCHNO IN (SELECT BATCHNO FROM " & cnAdminDb & "..ORMAST)"
        strSql += vbCrLf + Replace(ftrIssue, "I.METALID", "METALID")
        'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
        CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND I.COMPANYID")
        strSql += vbCrLf + CompanyFilt1
        strSql += vbCrLf + " ) X"
        If chkWithStuddedStone.Checked = True Then
            strSql += vbCrLf + " UNION ALL"

            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '2STUD'FRM,'ISS_OPENING'SEP,CATCODE"
            If rbtGeneral.Checked = True Then
                strSql += vbCrLf + " ,STNWT"
                strSql += vbCrLf + " ,STNWT"
                strSql += vbCrLf + " ,STNWT"
            ElseIf rbtCarat.Checked = True Then
                strSql += vbCrLf + " ,(CASE WHEN I.STONEUNIT = 'G' THEN STNWT * 5 ELSE STNWT END)GRSWT"
                strSql += vbCrLf + " ,(CASE WHEN I.STONEUNIT = 'G' THEN STNWT * 5 ELSE STNWT END)NETWT"
                strSql += vbCrLf + " ,(CASE WHEN I.STONEUNIT = 'G' THEN STNWT * 5 ELSE STNWT END)PUREWT"
            Else
                strSql += vbCrLf + " ,(CASE WHEN I.STONEUNIT = 'C' THEN STNWT / 5 ELSE STNWT END)GRSWT"
                strSql += vbCrLf + " ,(CASE WHEN I.STONEUNIT = 'C' THEN STNWT / 5 ELSE STNWT END)NETWT"
                strSql += vbCrLf + " ,(CASE WHEN I.STONEUNIT = 'C' THEN STNWT / 5 ELSE STNWT END)PUREWT"
            End If
            strSql += vbCrLf + " ,@FRMDATE TRANDATE,SNO,-1 TRANNO,TRANSTATUS"
            strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
            strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
            strSql += vbCrLf + " ,(SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ISSUE WHERE SNO=ISSSNO)ACCODE,(SELECT TOP 1 ITEMID FROM " & cnStockDb & "..ISSUE WHERE SNO=ISSSNO)ITEMID"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE AS I"
            strSql += vbCrLf + " WHERE TRANDATE < @frmDATE"
            strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..ISSUE "
            strSql += vbCrLf + " WHERE ISNULL(CANCEL,'') = '' "
            strSql += vbCrLf + " AND ISNULL(TRANTYPE,'') NOT IN  ('IRC') "
            If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(TRANTYPE,'') NOT IN  ('AI') "
            strSql += vbCrLf + " AND (TRANDATE < @frmDATE)"
            strSql += vbCrLf + Replace(ftrIssue, "I.METALID", "METALID")
            'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
            CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND I.COMPANYID")
            strSql += vbCrLf + CompanyFilt1
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '2STUD'FRM,'REC_OPENING'SEP,CATCODE"
            If rbtGeneral.Checked = True Then
                strSql += vbCrLf + " ,STNWT"
                strSql += vbCrLf + " ,STNWT"
                strSql += vbCrLf + " ,STNWT"
            ElseIf rbtCarat.Checked = True Then
                strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'G' THEN STNWT * 5 ELSE STNWT END)GRSWT"
                strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'G' THEN STNWT * 5 ELSE STNWT END)NETWT"
                strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'G' THEN STNWT * 5 ELSE STNWT END)PUREWT"
            Else
                strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'C' THEN STNWT / 5 ELSE STNWT END)GRSWT"
                strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'C' THEN STNWT / 5 ELSE STNWT END)NETWT"
                strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'C' THEN STNWT / 5 ELSE STNWT END)PUREWT"
            End If
            strSql += vbCrLf + " ,@FRMDATE TRANDATE,SNO,-1 TRANNO,TRANSTATUS"
            strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
            strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
            strSql += vbCrLf + " ,(SELECT TOP 1 ACCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO=ISSSNO)ACCODE,(SELECT TOP 1 ITEMID FROM " & cnStockDb & "..RECEIPT WHERE SNO=ISSSNO)ITEMID"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE AS R"
            strSql += vbCrLf + " WHERE TRANDATE < @frmDATE"
            strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT "
            strSql += vbCrLf + " WHERE ISNULL(CANCEL,'') = '' "
            strSql += vbCrLf + " AND ISNULL(R.TRANTYPE,'') NOT IN  ('RRC') "
            If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(R.TRANTYPE,'') NOT IN  ('AR') "
            strSql += vbCrLf + " AND (TRANDATE < @frmDATE )"
            strSql += vbCrLf + Replace(ftrReceipt, "AND CATCODE", "AND R.CATCODE")
            'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
            CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND R.COMPANYID")
            strSql += vbCrLf + CompanyFilt1
            strSql += vbCrLf + "               )"
            ''*****************************OPENWEIGHT STUD
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '2STUD'FRM,'REC_OPENING'SEP,R.CATCODE"
            If rbtGeneral.Checked = True Then
                strSql += vbCrLf + " ,R.GRSWT"
                strSql += vbCrLf + " ,NETWT"
                strSql += vbCrLf + " ,PUREWT"
            ElseIf rbtCarat.Checked = True Then
                strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN R.GRSWT * 5 ELSE R.GRSWT END)GRSWT"
                strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN NETWT * 5 ELSE NETWT END)NETWT"
                strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN PUREWT * 5 ELSE PUREWT END)PUREWT"
            Else
                strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN R.GRSWT / 5 ELSE R.GRSWT END)GRSWT"
                strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN NETWT / 5 ELSE NETWT END)NETWT"
                strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN PUREWT / 5 ELSE PUREWT END)PUREWT"
            End If
            strSql += vbCrLf + " ,@FRMDATE TRANDATE,SNO,-1 TRANNO,' 'TRANSTATUS"
            strSql += vbCrLf + " ,DATEPART(YEAR,@FRMDATE)YYEAR,DATEPART(MONTH,@FRMDATE)MMONTHID,"
            strSql += vbCrLf + " LEFT(DATENAME(MONTH,@FRMDATE),3)MMONTH,'' ACCODE,0 AS ITEMID"
            strSql += vbCrLf + " FROM " & cnStockDb & "..OPENWEIGHT AS R"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON R.CATCODE = C.CATCODE"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON R.ITEMID=IM.ITEMID"
            strSql += vbCrLf + " WHERE R.STOCKTYPE = 'C' AND IM.STUDDED='S'"
            'strSql += vbcrlf + " WHERE TRANDATE < @frmDATE AND ISNULL(CANCEL,'') = ''"
            If cmbCatName.Text <> "ALL" And cmbCatName.Text <> "" Then
                strSql += vbCrLf + " AND R.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY "
                strSql += vbCrLf + " WHERE CATNAME = '" & cmbCatName.Text & "')"
            End If
            'strSql += vbCrLf + Replace(ftrReceipt, "AND CATCODE", "AND R.CATCODE")
            'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
            CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND R.COMPANYID")
            strSql += vbCrLf + CompanyFilt1

            strSql += vbCrLf + " UNION ALL"

            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '2STUD'FRM,'ISSUE'SEP,I.CATCODE"
            If rbtGeneral.Checked = True Then
                strSql += vbCrLf + " ,I.STNWT GRSWT"
                strSql += vbCrLf + " ,I.STNWT NETWT"
                strSql += vbCrLf + " ,I.STNWT PUREWT"
            ElseIf rbtCarat.Checked = True Then
                strSql += vbCrLf + " ,(CASE WHEN I.STONEUNIT = 'G' THEN I.STNWT * 5 ELSE I.STNWT END)GRSWT"
                strSql += vbCrLf + " ,(CASE WHEN I.STONEUNIT = 'G' THEN I.STNWT * 5 ELSE I.STNWT END)NETWT"
                strSql += vbCrLf + " ,(CASE WHEN I.STONEUNIT = 'G' THEN I.STNWT * 5 ELSE I.STNWT END)PUREWT"
            Else
                strSql += vbCrLf + " ,(CASE WHEN I.STONEUNIT = 'C' THEN I.STNWT / 5 ELSE I.STNWT END)GRSWT"
                strSql += vbCrLf + " ,(CASE WHEN I.STONEUNIT = 'C' THEN I.STNWT / 5 ELSE I.STNWT END)NETWT"
                strSql += vbCrLf + " ,(CASE WHEN I.STONEUNIT = 'C' THEN I.STNWT / 5 ELSE I.STNWT END)PUREWT"
            End If
            strSql += vbCrLf + " ,TRANDATE,SNO,TRANNO,TRANSTATUS"
            strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
            strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
            strSql += vbCrLf + " ,(SELECT TOP 1 ACCODE FROM " & cnStockDb & "..ISSUE WHERE SNO=ISSSNO)ACCODE,(SELECT TOP 1 ITEMID FROM " & cnStockDb & "..ISSUE WHERE SNO=ISSSNO)ITEMID"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE AS I"
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN @frmDATE AND @toDATE "
            strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..ISSUE "
            strSql += vbCrLf + " WHERE ISNULL(CANCEL,'') = '' "
            strSql += vbCrLf + " AND ISNULL(I.TRANTYPE,'') NOT IN  ('IRC') "
            If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(I.TRANTYPE,'') NOT IN  ('AI') "
            strSql += vbCrLf + " AND (TRANDATE BETWEEN @frmDATE AND @toDATE )"
            strSql += vbCrLf + Replace(ftrIssue, "I.METALID", "METALID")
            strSql += vbCrLf + CompanyFilt
            'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
            strSql += vbCrLf + "               )"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '2STUD'FRM,'RECEIPT'SEP,CATCODE"
            If rbtGeneral.Checked = True Then
                strSql += vbCrLf + " ,R.STNWT GRSWT"
                strSql += vbCrLf + " ,R.STNWT NETWT"
                strSql += vbCrLf + " ,R.STNWT PUREWT"
            ElseIf rbtCarat.Checked = True Then
                strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'G' THEN R.STNWT * 5 ELSE R.STNWT END)GRSWT"
                strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'G' THEN R.STNWT * 5 ELSE R.STNWT END)NETWT"
                strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'G' THEN R.STNWT * 5 ELSE R.STNWT END)PUREWT"
            Else
                strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'C' THEN R.STNWT / 5 ELSE R.STNWT END)GRSWT"
                strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'C' THEN R.STNWT / 5 ELSE R.STNWT END)NETWT"
                strSql += vbCrLf + " ,(CASE WHEN R.STONEUNIT = 'C' THEN R.STNWT / 5 ELSE R.STNWT END)PUREWT"
            End If
            strSql += vbCrLf + " ,TRANDATE,SNO,TRANNO,TRANSTATUS"
            strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
            strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
            strSql += vbCrLf + " ,(SELECT TOP 1 ACCODE FROM " & cnStockDb & "..RECEIPT WHERE SNO=ISSSNO)ACCODE,(SELECT TOP 1 ITEMID FROM " & cnStockDb & "..RECEIPT WHERE SNO=ISSSNO)ITEMID"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE AS R"
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN @frmDATE AND @toDATE "
            strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT "
            strSql += vbCrLf + " WHERE ISNULL(CANCEL,'') = '' "
            strSql += vbCrLf + " AND ISNULL(R.TRANTYPE,'') NOT IN  ('RRC') "
            If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(R.TRANTYPE,'') NOT IN  ('AR') "
            strSql += vbCrLf + " AND (TRANDATE BETWEEN @frmDATE AND @toDATE )"
            strSql += vbCrLf + Replace(ftrReceipt, "AND CATCODE", "AND R.CATCODE")
            CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND R.COMPANYID")
            strSql += vbCrLf + CompanyFilt1
            strSql += vbCrLf + "               )"
        End If
        strSql += vbCrLf + " )X"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "delete from TEMP" & systemId & "CATSTOCK where opening = 0 and issue = 0 and receipt = 0 and closing = 0 and value = 0"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' "
        strSql += vbCrLf + " AND NAME = 'TEMP" & systemId & "CATSUMM')>0 "
        strSql += vbCrLf + " DROP TABLE TEMP" & systemId & "CATSUMM"
        strSql += vbCrLf + " CREATE TABLE TEMP" & systemId & "CATSUMM("
        strSql += vbCrLf + " METALNAME VARCHAR(50),"
        strSql += vbCrLf + " TRANDATE SMALLDATETIME,"
        strSql += vbCrLf + " TRANNO VARCHAR(12),"
        strSql += vbCrLf + " DESCRIPTION VARCHAR(55),"
        strSql += vbCrLf + " CATNAME VARCHAR(100),"
        strSql += vbCrLf + " OPENING NUMERIC(12,3),"
        strSql += vbCrLf + " RECEIPT NUMERIC(12,3),"
        strSql += vbCrLf + " ISSUE NUMERIC(12,3),"
        strSql += vbCrLf + " CLOSING NUMERIC(12,3),"
        strSql += vbCrLf + " REMARKS VARCHAR(300),"
        strSql += vbCrLf + " RATE VARCHAR(30),"
        strSql += vbCrLf + " VALUE VARCHAR(30),"
        strSql += vbCrLf + " CATCODE VARCHAR(15),"
        strSql += vbCrLf + " RESULT INT,"
        strSql += vbCrLf + " FRM VARCHAR(30),"
        strSql += vbCrLf + " YYEAR INT,"
        strSql += vbCrLf + " MMONTHID INT,"
        strSql += vbCrLf + " MMONTH	NVARCHAR(9),"
        strSql += vbCrLf + " DDATE VARCHAR(12),"
        strSql += vbCrLf + " COLHEAD VARCHAR(1),"
        strSql += vbCrLf + " ACCODE VARCHAR(12),"
        strSql += vbCrLf + " ITEMID INT,"
        strSql += vbCrLf + " SNO INT IDENTITY)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        funcTranNoWise()


        strSql = vbCrLf + " IF (SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND "
        strSql += vbCrLf + " NAME = 'TEMP" & systemId & "CTRSTOCKTEMP')>0 "
        strSql += vbCrLf + " DROP TABLE TEMP" & systemId & "CTRSTOCKTEMP"
        strSql += vbCrLf + " SELECT DESCRIPTION,PARTICULAR,METALNAME,CATNAME,OPENING,RECEIPT,ISSUE,CLOSING,RATE,VALUE,CATCODE,"
        strSql += vbCrLf + " RESULT,FRM,YYEAR,MMONTHID,MMONTH, DDATE, TRANDATE, TRANNO, COLHEAD ,REMARKS"
        strSql += vbCrLf + " INTO TEMP" & systemId & "CTRSTOCKTEMP "
        strSql += vbCrLf + " FROM ("
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " (SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE=A.ACCODE)DESCRIPTION,PARTICULAR,"
        strSql += vbCrLf + " METALNAME,CATNAME,OPENING,RECEIPT,ISSUE,CLOSING,RATE,VALUE,CATCODE,"
        strSql += vbCrLf + " RESULT,FRM,YYEAR,MMONTHID,MMONTH,DDATE,TRANDATE,"
        strSql += vbCrLf + " TRANNO,COLHEAD ,"
        strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=A.ITEMID)REMARKS"
        strSql += vbCrLf + " FROM TEMP" & systemId & "CATSTOCKTRANNO AS A"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT"
        strSql += vbCrLf + " '' AS DESCRIPTION,'SUB TOTAL'PARTICULAR,METALNAME,CATNAME"
        strSql += vbCrLf + " ,SUM(OPENING)OPENING,SUM(RECEIPT)RECEIPT,SUM(ISSUE)ISSUE,SUM(CLOSING)CLOSING"
        strSql += vbCrLf + " ,' ' RATE,' ' VALUE,' 'CATCODE,2 RESULT"
        strSql += vbCrLf + " ,' 'FRM"
        strSql += vbCrLf + " ,' 'YYEAR,' 'MMONTHID,' 'MMONTH,' 'DDATE,' 'TRANDATE,' 'TRANNO, 'S'COLHEAD,'' REMARKS"
        strSql += vbCrLf + " FROM TEMP" & systemId & "CATSTOCK AS T"
        strSql += vbCrLf + " GROUP BY METALNAME,CATNAME"
        strSql += vbCrLf + " ) X"
        strSql += vbCrLf + " ORDER BY METALNAME,RESULT"


        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "CTRSTOCKTEMP)>0 "
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "CTRSTOCKTEMP(METALNAME,CATNAME,RESULT,RATE,"
        strSql += vbCrLf + " VALUE,FRM,YYEAR,MMONTHID,MMONTH,DDATE,TRANDATE,TRANNO,COLHEAD,PARTICULAR )"
        strSql += vbCrLf + " SELECT DISTINCT METALNAME,CATNAME,0,' ',' ',' ','','','','','','','T','' "
        strSql += vbCrLf + " FROM TEMP" & systemId & "CTRSTOCKTEMP WHERE RESULT =1"
        strSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "CTRSTOCKTEMP)>0 "
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "CTRSTOCKTEMP SET DESCRIPTION = CASE WHEN RESULT = 0 THEN "
        strSql += vbCrLf + " CATNAME WHEN RESULT = 2 THEN 'SUB TOTAL'  ELSE DESCRIPTION END"
        strSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()



        strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "CTRSTOCKTEMP)>0 "
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "CATSUMM(DESCRIPTION,REMARKS,METALNAME,CATNAME,OPENING,RECEIPT,ISSUE,"
        strSql += vbCrLf + " Closing, Rate, VALUE, CATCODE, RESULT, FRM, YYEAR, MMONTHID, MMONTH, DDATE, "
        strSql += vbCrLf + " TRANDATE, TRANNO, COLHEAD) "
        strSql += vbCrLf + " SELECT DESCRIPTION,REMARKS,METALNAME,PARTICULAR,OPENING,RECEIPT,ISSUE,"
        strSql += vbCrLf + " Closing, Rate, VALUE, CATCODE, RESULT, FRM, YYEAR, MMONTHID, MMONTH, DDATE, "
        strSql += vbCrLf + " TRANDATE, TRANNO, COLHEAD"
        strSql += vbCrLf + " FROM TEMP" & systemId & "CTRSTOCKTEMP"
        strSql += vbCrLf + " ORDER BY CATNAME,RESULT"
        strSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If chkWithValue.Checked = True Then funcRateValUpdation()

        strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "CATSUMM)>0 "
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "CATSUMM SET TRANDATE=NULL,TRANNO=NULL WHERE COLHEAD IN ('T','S')"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "CATSUMM SET DESCRIPTION='OPENING',TRANNO=NULL WHERE TRANNO='-1'"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "CATSUMM SET OPENING=NULL WHERE OPENING = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "CATSUMM SET RECEIPT=NULL WHERE RECEIPT = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "CATSUMM SET ISSUE=NULL WHERE ISSUE = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "CATSUMM SET CLOSING=NULL WHERE CLOSING= 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "CATSUMM SET RATE = '' WHERE RATE = '0.00'"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "CATSUMM SET VALUE = '' WHERE VALUE='0.00'"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "CATSUMM SET VALUE = CONVERT(VARCHAR,(SELECT CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(VALUE,'') = '' THEN '0' ELSE VALUE END))) FROM TEMP" & systemId & "CATSUMM WHERE METALNAME = T.METALNAME))"
        strSql += vbCrLf + " FROM TEMP" & systemId & "CATSUMM AS T WHERE RESULT = 2"
        strSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT * FROM TEMP" & systemId & "CATSUMM ORDER BY SNO"

        Prop_Sets()
        dtGridView = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGridView)
        gridView.DataSource = Nothing
        If Not dtGridView.Rows.Count > 0 Then
            MsgBox("Record Not Found")
            Exit Sub
        End If
        gridView.DefaultCellStyle = Nothing
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        gridView.DataSource = dtGridView
        GridViewFormat()
        With gridView
            .Columns("METALNAME").Visible = False
            .Columns("CATNAME").Visible = False
            With .Columns("DESCRIPTION")
                .ReadOnly = True
                .Width = 400
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("OPENING")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("ISSUE")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("RECEIPT")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TRANNO")
                .Width = 35
                .HeaderText = "NO"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CLOSING")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("REMARKS")
                .Width = 120
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("TRANDATE")
                .Width = 80
                .DefaultCellStyle.Format = "dd/MM/yyyy"
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            With .Columns("CLOSING")
                .Width = 80
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .ReadOnly = True
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            If chkWithValue.Checked = True Then
                With .Columns("RATE")
                    .Width = 60
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .ReadOnly = False
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("VALUE")
                    .HeaderText = "TOTAL"
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .ReadOnly = True
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            Else
                .Columns("RATE").Visible = False
                .Columns("VALUE").Visible = False
            End If
            Dim st As Integer = Nothing
            For CNT As Integer = 10 To gridView.ColumnCount - 1
                .Columns(CNT).Visible = False
            Next
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            '.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        End With

        pnlHeading.Visible = True
        Dim title As String = Nothing
        title += " TRANNO WISE"
        title += " STOCK REPORT BASED ON"
        If rbtGrsWt.Checked = True Then
            title += " GROSS WEIGHT"
        Else
            title += " NET WEIGHT"
        End If
        title += " ("
        If rbtCarat.Checked = True Then
            title += "CARAT UNIT"
        ElseIf rbtGram.Checked = True Then
            title += "GRAM UNIT"
        Else
            title += "GENERAL UNIT"
        End If
        title += ")"
        title += " FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        title += IIf(chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "", " :" & chkCmbCostCentre.Text, "")
        lblTitle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        lblTitle.Text = title
        If chkWithValue.Checked = True Then btnSave.Enabled = True
        gridView.Focus()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        pnlHeading.Visible = False
        ''rbtGrsWt.Checked = True
        ''cmbCatName.Text = "ALL"
        ''cmbMetal.Text = "ALL"
        ''cmbMetalType.Text = "ALL"
        ''rbtGeneral.Checked = True
        ''rbtSummary.Checked = True
        rbtnTranno.Checked = True
        gridView.DataSource = Nothing
        dsGridView.Clear()
        dtpFrom.Value = GetServerDate()
        btnSave.Enabled = False
        dtpFrom.Focus()

        Prop_Gets()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub gridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEndEdit
        With gridView.Rows(e.RowIndex)
            If .Cells(e.ColumnIndex - 1).Value.ToString = "" Then
                .Cells(e.ColumnIndex).Value = ""
                Exit Sub
            End If
            If IsNumeric(.Cells(e.ColumnIndex).Value.ToString) = False Then
                .Cells(e.ColumnIndex).Value = ""
                Exit Sub
            End If
            Dim totVal As Double = Val(.Cells(e.ColumnIndex).Value.ToString) * Val(.Cells(e.ColumnIndex - 1).Value.ToString)
            If totVal <> 0 Then
                .Cells(e.ColumnIndex + 1).Value = Format(totVal, "0.00")
            End If

            Dim drGrid As DataRow()
            drGrid = dtGridView.Select("METALNAME = '" & .Cells("METALNAME").Value.ToString().Trim & "' AND COLHEAD = 'S'")
            If drGrid.Length > 0 Then
                Dim Value As Double = 0
                For Each dr As DataRow In dtGridView.Select("METALNAME = '" & .Cells("METALNAME").Value.ToString().Trim & "' AND RESULT = 1")
                    Value += Val(dr.Item("VALUE").ToString())
                Next
                gridView.Rows(Val(drGrid(0).Item("SNO").ToString) - 1).Cells("VALUE").Value = Val(Value).ToString("0.00")
                'dtGridView.Rows(Val(drGrid(0).Item("SNO").ToString) - 1).Item("VALUE") = Val(Value).ToString("0.00")
                'dtGridView.AcceptChanges()
            End If
        End With
    End Sub

    Private Sub funcClosingStock()


    End Sub

    ''Private Sub gridView_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles gridView.CellFormatting
    ''    Select Case gridView.Rows(e.RowIndex).Cells("COLHEAD").Value.ToString
    ''        Case " "
    ''            gridView.Rows(e.RowIndex).Cells("CATNAME").Style.BackColor = Nothing
    ''        Case "S"
    ''            gridView.Rows(e.RowIndex).DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
    ''            gridView.Rows(e.RowIndex).DefaultCellStyle.Font = reportSubTotalStyle.Font
    ''        Case "T"
    ''            gridView.Rows(e.RowIndex).Cells("CATNAME").Style.BackColor = reportHeadStyle.BackColor
    ''            gridView.Rows(e.RowIndex).Cells("CATNAME").Style.Font = reportHeadStyle.Font
    ''            'gridView.Rows(e.RowIndex).DefaultCellStyle.BackColor = Nothing

    ''    End Select
    ''End Sub

    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case " "
                        .Cells("DESCRIPTION").Style.BackColor = Nothing
                    Case "S"
                        .DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                        .Cells("COLHEAD").Value = "G"
                    Case "T"
                        .Cells("DESCRIPTION").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("DESCRIPTION").Style.Font = reportHeadStyle.Font
                End Select
            End With
        Next
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
                If chkWithValue.Checked = True Then
                    btnSave.Focus()
                End If
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
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export)
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
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print)
        End If
    End Sub

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        pnlHeading.Size = New System.Drawing.Size(100, 21)
        cmbCatName.Items.Add("ALL")
        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY ORDER BY CATNAME"
        objGPack.FillCombo(strSql, cmbCatName, False, False)

        cmbMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbMetal, False)

        cmbMetalType.Items.Add("ALL")
        cmbMetalType.Items.Add("ORNAMENT")
        cmbMetalType.Items.Add("METAL")
        cmbMetalType.Items.Add("STONE")

        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub frmCategoryStock_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Leave

    End Sub

    Private Sub frmCategoryStock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += vbCrLf + " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , "ALL")
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

    Private Sub chkWithValue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkWithValue.CheckedChanged
        If chkWithValue.Checked Then
            If chkCmbCompany.CheckedItems.Count > 0 Then
                If (chkCmbCompany.GetItemChecked(0) = True And chkCmbCompany.Items.Count > 1) Or chkCmbCompany.CheckedItems.Count > 1 Then
                    MessageBox.Show("Only one company needed for Rate updation...", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return
                End If
            End If
            If chkCmbCostCentre.CheckedItems.Count > 0 Then
                If (chkCmbCostCentre.GetItemChecked(0) = True And chkCmbCostCentre.Items.Count > 1) Or chkCmbCostCentre.CheckedItems.Count > 1 Then
                    MessageBox.Show("Only one costcentre needed for Rate updation...", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return
                End If
            End If
            btnSave.Enabled = True
        Else
            btnSave.Enabled = False
        End If
    End Sub

    Private Sub funcRateValUpdation()
        If chkCmbCompany.CheckedItems.Count > 0 Then
            If (chkCmbCompany.GetItemChecked(0) = True And chkCmbCompany.Items.Count > 1) Or chkCmbCompany.CheckedItems.Count > 1 Then
                Return
            End If
        End If
        If chkCmbCostCentre.CheckedItems.Count > 0 Then
            If (chkCmbCostCentre.GetItemChecked(0) = True And chkCmbCostCentre.Items.Count > 1) Or chkCmbCostCentre.CheckedItems.Count > 1 Then
                Return
            End If
        End If
        If rbtGrsWt.Checked = False Then
            Return
        End If

        If rbtGeneral.Checked = False Then
            Return
        End If

        If chkWithOtherIssue.Checked = True Then
            Return
        End If

        Dim strCompId As String = cnCompanyId
        Dim strCostId As String = cnCostId

        If chkCmbCompany.Items.Count > 0 Then
            strSql = " SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" + chkCmbCompany.Text + "'"
            strCompId = BrighttechPack.GetSqlValue(cn, strSql, "COMPANYID", "")
        End If

        If chkCmbCostCentre.Items.Count > 0 Then
            strSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" + chkCmbCostCentre.Text + "'"
            strCostId = BrighttechPack.GetSqlValue(cn, strSql, "COSTID", "")
        End If

        strSql = " UPDATE TEMP" & systemId & "CATSUMM SET RATE = C.RATE,VALUE = CONVERT(NUMERIC(15,2),ISNULL(C.CLOSING,0) * ISNULL(C.RATE ,0))"
        strSql += " FROM TEMP" & systemId & "CATSUMM AS T INNER JOIN " + cnStockDb + "..CLOSINGVAL AS C ON ISNULL(T.CATCODE,'') = ISNULL(C.CATCODE,'')"
        strSql += " WHERE C.COMPANYID = '" + strCompId + "' AND C.COSTID = '" + strCostId + "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If chkWithValue.Checked = True Then
            If chkCmbCompany.CheckedItems.Count > 0 Then
                If (chkCmbCompany.GetItemChecked(0) = True And chkCmbCompany.Items.Count > 1) Or chkCmbCompany.CheckedItems.Count > 1 Then
                    MessageBox.Show("Only one company needed for Rate updation...", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    btnSave.Enabled = False
                    chkCmbCompany.Focus()
                    Return
                End If
            End If
            If chkCmbCostCentre.CheckedItems.Count > 0 Then
                If (chkCmbCostCentre.GetItemChecked(0) = True And chkCmbCostCentre.Items.Count > 1) Or chkCmbCostCentre.CheckedItems.Count > 1 Then
                    MessageBox.Show("Only one costcentre needed for Rate updation...", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    btnSave.Enabled = False
                    chkCmbCostCentre.Focus()
                    Return
                End If
            End If
            If rbtGrsWt.Checked = False Then
                MessageBox.Show("GrsWt needed for Rate updation...", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
                btnSave.Enabled = False
                rbtGrsWt.Focus()
                Return
            End If

            If rbtGeneral.Checked = False Then
                MessageBox.Show("General Mode set in stone unit for Rate updation...", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
                btnSave.Enabled = False
                rbtGrsWt.Focus()
                Return
            End If

            If chkWithOtherIssue.Checked = True Then
                MessageBox.Show("Update rate not needed Other Issue...", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
                btnSave.Enabled = False
                chkWithOtherIssue.Focus()
                Return
            End If

            Dim strCompId As String = cnCompanyId
            Dim strCostId As String = cnCostId

            If chkCmbCompany.Items.Count > 0 Then
                strSql = " SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME = '" + chkCmbCompany.Text + "'"
                strCompId = BrighttechPack.GetSqlValue(cn, strSql, "COMPANYID", "")
            End If
            If chkCmbCostCentre.Items.Count > 0 Then
                strSql = " SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" + chkCmbCostCentre.Text + "'"
                strCostId = BrighttechPack.GetSqlValue(cn, strSql, "COSTID", "")
            End If

            If gridView.Rows.Count > 0 Then
                For cnt As Integer = 0 To gridView.Rows.Count - 1
                    If gridView.Rows(cnt).Cells("CATCODE").Value.ToString().Trim <> "" Then
                        strSql = " DELETE FROM " & cnStockDb & "..CLOSINGVAL WHERE COMPANYID = '" + strCompId + "' AND COSTID = '" + strCostId + "' AND CATCODE = '" + gridView.Rows(cnt).Cells("CATCODE").Value.ToString + "'"
                        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                        cmd.ExecuteNonQuery()

                        strSql = " INSERT INTO " & cnStockDb & "..CLOSINGVAL"
                        strSql += " (COMPANYID,COSTID,CATCODE,CLOSING,RATE,VALUE,UPDATED,UPTIME)"
                        strSql += " VALUES("
                        strSql += "  '" + strCompId + "'"
                        strSql += " ,'" + strCostId + "'"
                        strSql += " ,'" + gridView.Rows(cnt).Cells("CATCODE").Value.ToString + "'"
                        strSql += " ,'" + IIf(gridView.Rows(cnt).Cells("CLOSING").Value.ToString.Trim = "", "0", gridView.Rows(cnt).Cells("CLOSING").Value.ToString) + "'"
                        strSql += " ,'" + IIf(gridView.Rows(cnt).Cells("RATE").Value.ToString.Trim = "", "0", gridView.Rows(cnt).Cells("RATE").Value.ToString) + "'"
                        strSql += " ,'" + IIf(gridView.Rows(cnt).Cells("VALUE").Value.ToString.Trim = "", "0", gridView.Rows(cnt).Cells("VALUE").Value.ToString) + "'"
                        strSql += " ,'" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "'"
                        strSql += " ,'" + DateTime.Now.ToShortTimeString + "'"
                        strSql += " )"
                        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                        cmd.ExecuteNonQuery()
                    End If
                Next cnt
            End If
            btnNew_Click(Me, New EventArgs)
        End If
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmCategoryStockNew_Properties
        GetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany)
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        obj.p_cmbMetal = cmbMetal.Text
        obj.p_cmbMetalType = cmbMetalType.Text
        obj.p_rbtGrsWt = rbtGrsWt.Checked
        obj.p_rbtNetWt = rbtNetWt.Checked
        obj.p_cmbCatName = cmbCatName.Text
        obj.p_rbtGeneral = rbtGeneral.Checked
        obj.p_rbtGram = rbtGram.Checked
        obj.p_rbtCarat = rbtCarat.Checked
        obj.p_chkWithValue = chkWithValue.Checked
        obj.p_chkWithOtherIssue = chkWithOtherIssue.Checked
        obj.p_chkWithStuddedStone = chkWithStuddedStone.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmCategoryStockNew_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmCategoryStockNew_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmCategoryStockNew_Properties))
        SetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany, "ALL")
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        cmbMetal.Text = obj.p_cmbMetal
        cmbMetalType.Text = obj.p_cmbMetalType
        rbtGrsWt.Checked = obj.p_rbtGrsWt
        rbtNetWt.Checked = obj.p_rbtNetWt
        cmbCatName.Text = obj.p_cmbCatName
        rbtGeneral.Checked = obj.p_rbtGeneral
        rbtGram.Checked = obj.p_rbtGram
        rbtCarat.Checked = obj.p_rbtCarat
        chkWithValue.Checked = obj.p_chkWithValue
        chkWithOtherIssue.Checked = obj.p_chkWithOtherIssue
        chkWithStuddedStone.Checked = obj.p_chkWithStuddedStone
    End Sub

    Private Sub AutoResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoResizeToolStripMenuItem.Click
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


End Class

Public Class frmCategoryStockNew_Properties
    Private chkCmbCompany As New List(Of String)
    Public Property p_chkCmbCompany() As List(Of String)
        Get
            Return chkCmbCompany
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCompany = value
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

    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property

    Private cmbMetalType As String = "ALL"
    Public Property p_cmbMetalType() As String
        Get
            Return cmbMetalType
        End Get
        Set(ByVal value As String)
            cmbMetalType = value
        End Set
    End Property
    Private rbtGrsWt As Boolean = True
    Public Property p_rbtGrsWt() As Boolean
        Get
            Return rbtGrsWt
        End Get
        Set(ByVal value As Boolean)
            rbtGrsWt = value
        End Set
    End Property

    Private rbtNetWt As Boolean = False
    Public Property p_rbtNetWt() As Boolean
        Get
            Return rbtNetWt
        End Get
        Set(ByVal value As Boolean)
            rbtNetWt = value
        End Set
    End Property

    Private cmbCatName As String = "ALL"
    Public Property p_cmbCatName() As String
        Get
            Return cmbCatName
        End Get
        Set(ByVal value As String)
            cmbCatName = value
        End Set
    End Property

    Private rbtGeneral As Boolean = True
    Public Property p_rbtGeneral() As Boolean
        Get
            Return rbtGeneral
        End Get
        Set(ByVal value As Boolean)
            rbtGeneral = value
        End Set
    End Property

    Private rbtGram As Boolean = False
    Public Property p_rbtGram() As Boolean
        Get
            Return rbtGram
        End Get
        Set(ByVal value As Boolean)
            rbtGram = value
        End Set
    End Property

    Private rbtCarat As Boolean = False
    Public Property p_rbtCarat() As Boolean
        Get
            Return rbtCarat
        End Get
        Set(ByVal value As Boolean)
            rbtCarat = value
        End Set
    End Property

    Private rbtSummary As Boolean = True
    Public Property p_rbtSummary() As Boolean
        Get
            Return rbtSummary
        End Get
        Set(ByVal value As Boolean)
            rbtSummary = value
        End Set
    End Property

    Private rbtMonthWise As Boolean = False
    Public Property p_rbtMonthWise() As Boolean
        Get
            Return rbtMonthWise
        End Get
        Set(ByVal value As Boolean)
            rbtMonthWise = value
        End Set
    End Property

    Private rbtDateWise As Boolean = False
    Public Property p_rbtDateWise() As Boolean
        Get
            Return rbtDateWise
        End Get
        Set(ByVal value As Boolean)
            rbtDateWise = value
        End Set
    End Property

    Private rbtTranNoWise As Boolean = False
    Public Property p_rbtTranNoWise() As Boolean
        Get
            Return rbtTranNoWise
        End Get
        Set(ByVal value As Boolean)
            rbtTranNoWise = value
        End Set
    End Property

    Private chkWithValue As Boolean = False
    Public Property p_chkWithValue() As Boolean
        Get
            Return chkWithValue
        End Get
        Set(ByVal value As Boolean)
            chkWithValue = value
        End Set
    End Property

    Private chkWithOtherIssue As Boolean = False
    Public Property p_chkWithOtherIssue() As Boolean
        Get
            Return chkWithOtherIssue

        End Get
        Set(ByVal value As Boolean)
            chkWithOtherIssue = value
        End Set
    End Property

    Private chkWithStuddedStone As Boolean = False
    Public Property p_chkWithStuddedStone() As Boolean
        Get
            Return chkWithStuddedStone

        End Get
        Set(ByVal value As Boolean)
            chkWithStuddedStone = value
        End Set
    End Property
End Class
