Imports System.Data.OleDb
Public Class frmClosingStock_Category
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
        '--TRANNO WISE CATSTOCK
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "CATSTOCKTRANNO')>0"
        strSql += vbcrlf + " DROP TABLE TEMP" & systemId & "CATSTOCKTRANNO"
        strSql += vbCrLf + " SELECT SPACE(50) PARTICULAR,' 'METALNAME,CATNAME,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(OPENING)OPENING,"
        strSql += vbcrlf + " SUM(RECEIPT)RECEIPT,SUM(ISSUE)ISSUE"
        strSql += vbcrlf + " ,SUM(CLOSING)CLOSING,RATE,VALUE,CATCODE,RESULT"
        strSql += vbcrlf + " ,FRM"
        strSql += vbcrlf + " ,' 'YYEAR,' 'MMONTHID,' 'MMONTH,' 'DDATE"
        strSql += vbcrlf + " ,' 'TRANDATE,TRANNO, ' 'COLHEAD "
        strSql += vbcrlf + " INTO TEMP" & systemId & "CATSTOCKTRANNO"
        strSql += vbcrlf + " FROM TEMP" & systemId & "CATSTOCK"
        strSql += vbcrlf + " GROUP BY CATNAME,CATCODE,RESULT,RATE,VALUE,FRM,TRANNO"
        strSql += vbcrlf + " ORDER BY CATNAME,TRANNO"
        strSql += vbcrlf + " DECLARE @CATNAME VARCHAR(50)"
        strSql += vbCrLf + " DECLARE @TRANNO INT"
        strSql += vbCrLf + " DECLARE @GRSWT NUMERIC(12,3)"
        strSql += vbCrLf + " DECLARE @NETWT NUMERIC(12,3)"
        strSql += vbcrlf + " DECLARE @OPENING NUMERIC(12,3)"
        strSql += vbcrlf + " DECLARE @ISSUE NUMERIC(12,3)"
        strSql += vbcrlf + " DECLARE @RECEIPT NUMERIC(12,3)"
        strSql += vbcrlf + " DECLARE @CLOSING NUMERIC(12,3)"
        strSql += vbcrlf + " DECLARE @PRETRANNO INT"
        strSql += vbcrlf + " DECLARE CURCAT CURSOR"
        strSql += vbcrlf + " FOR SELECT CATNAME FROM TEMP" & systemId & "CATSTOCKTRANNO GROUP BY CATNAME"
        strSql += vbcrlf + " OPEN CURCAT"
        strSql += vbcrlf + " WHILE 1=1"
        strSql += vbcrlf + " BEGIN"
        strSql += vbcrlf + " 	FETCH NEXT FROM CURCAT INTO @CATNAME"
        strSql += vbcrlf + " 	SELECT @TRANNO = NULL"
        strSql += vbcrlf + " 	SELECT @PRETRANNO = NULL"
        strSql += vbcrlf + " 	IF @@FETCH_STATUS = -1 BREAK"
        strSql += vbcrlf + "         DECLARE CURMONTH CURSOR"
        strSql += vbCrLf + "         FOR SELECT TRANNO,GRSWT,NETWT,OPENING,ISSUE,RECEIPT,CLOSING FROM TEMP" & systemId & "CATSTOCKTRANNO WHERE CATNAME = @CATNAME"
        strSql += vbcrlf + "         OPEN CURMONTH"
        strSql += vbcrlf + "         WHILE 1=1"
        strSql += vbcrlf + " 	BEGIN"
        strSql += vbcrlf + " 		SELECT @TRANNO"
        strSql += vbcrlf + " 		UPDATE TEMP" & systemId & "CATSTOCKTRANNO SET CLOSING = OPENING+RECEIPT-ISSUE WHERE TRANNO = @TRANNO AND CATNAME = @CATNAME"
        strSql += vbCrLf + "  		FETCH NEXT FROM CURMONTH INTO @TRANNO,@GRSWT,@NETWT,@OPENING,@ISSUE,@RECEIPT,@CLOSING"
        strSql += vbcrlf + " 		IF @PRETRANNO <> @TRANNO"
        strSql += vbcrlf + "                    BEGIN"
        strSql += vbcrlf + "                    UPDATE TEMP" & systemId & "CATSTOCKTRANNO SET OPENING = (SELECT CLOSING FROM TEMP" & systemId & "CATSTOCKTRANNO WHERE TRANNO = @PRETRANNO AND CATNAME = @CATNAME)"
        strSql += vbcrlf + "                    WHERE TRANNO = @TRANNO AND CATNAME = @CATNAME"
        strSql += vbcrlf + "                    END"
        strSql += vbcrlf + " 		SELECT @PRETRANNO = @TRANNO"
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

    Function funcMonthWise() As Integer
        '--MONGH WISE CATSTOCK
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "CATSTOCKMONTH')>0"
        strSql += vbCrLf + " DROP TABLE TEMP" & systemId & "CATSTOCKMONTH"
        ' Year Filtration
        strSql += vbCrLf + " DECLARE @frmDATE SMALLDATETIME "
        strSql += vbCrLf + " DECLARE @toDATE SMALLDATETIME "
        ' strSql += vbCrLf + " SELECT @frmDATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT @toDATE = '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        ' Year Filtration
        strSql += vbCrLf + " SELECT SPACE(50) PARTICULAR,METALNAME,CATNAME,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(OPENING)OPENING,SUM(RECEIPT)RECEIPT,SUM(ISSUE)ISSUE"
        strSql += vbcrlf + " ,SUM(CLOSING)CLOSING,RATE,VALUE,CATCODE,RESULT"
        strSql += vbcrlf + " ,FRM"
        strSql += vbcrlf + " ,YYEAR,MMONTHID,MMONTH,' 'DDATE"
        strSql += vbcrlf + " ,' 'TRANDATE,' 'TRANNO, ' 'COLHEAD "
        strSql += vbcrlf + " INTO TEMP" & systemId & "CATSTOCKMONTH"
        strSql += vbCrLf + " FROM TEMP" & systemId & "CATSTOCK"
        strSql += vbCrLf + " WHERE TRANDATE <= @toDATE"
        strSql += vbcrlf + " GROUP BY METALNAME,CATNAME,CATCODE,RESULT,RATE,VALUE,FRM,YYEAR,MMONTHID,MMONTH"
        strSql += vbcrlf + " ORDER BY CATNAME,YYEAR,MMONTHID"
        strSql += vbcrlf + " DECLARE @CATNAME VARCHAR(50)"
        strSql += vbCrLf + " DECLARE @MMONTHID INT"
        strSql += vbCrLf + " DECLARE @GRSWT NUMERIC(12,3)"
        strSql += vbCrLf + " DECLARE @NETWT NUMERIC(12,3)"
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
        strSql += vbCrLf + "         FOR SELECT MMONTHID,GRSWT,NETWT,OPENING,ISSUE,RECEIPT,CLOSING FROM TEMP" & systemId & "CATSTOCKMONTH WHERE CATNAME = @CATNAME"
        strSql += vbcrlf + "         OPEN CURMONTH"
        strSql += vbcrlf + "         WHILE 1=1"
        strSql += vbcrlf + " 	BEGIN"
        strSql += vbcrlf + " 		SELECT @MMONTHID"
        strSql += vbcrlf + " 		UPDATE TEMP" & systemId & "CATSTOCKMONTH SET CLOSING = OPENING+RECEIPT-ISSUE WHERE MMONTHID = @MMONTHID AND CATNAME = @CATNAME"
        strSql += vbCrLf + "  		FETCH NEXT FROM CURMONTH INTO @MMONTHID,@GRSWT,@NETWT,@OPENING,@ISSUE,@RECEIPT,@CLOSING"
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
        strSql += vbCrLf + " DROP TABLE TEMP" & systemId & "CATSTOCKSUMMARY"
        strSql += vbCrLf + " SELECT SPACE(50) PARTICULAR,METALNAME,CATNAME,"
        If chkPcs.Checked = True Then
            strSql += vbCrLf + "SUM(PCS)PCS,"
        End If
        If rbtBoth.Checked = True Then
            strSql += vbCrLf + " SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT) AMOUNT"
        Else
            strSql += vbCrLf + " SUM(OPENING)OPENING,SUM(RECEIPT)RECEIPT,SUM(ISSUE)ISSUE,SUM(CLOSING)CLOSING "
        End If
        strSql += vbCrLf + " ,RATE,VALUE VALUE,0 CATCODE,RESULT"
        strSql += vbCrLf + " ,FRM"
        strSql += vbCrLf + " ,' ' YYEAR,' 'MMONTHID,' 'MMONTH,' 'DDATE"
        strSql += vbCrLf + " ,' 'TRANDATE,' 'TRANNO,0 COLHEAD ,COLNO"
        strSql += vbCrLf + " INTO TEMP" & systemId & "CATSTOCKSUMMARY"
        strSql += vbCrLf + " FROM TEMP" & systemId & "CATSTOCK"
        strSql += vbCrLf + " GROUP BY METALNAME,CATNAME,RESULT,RATE,VALUE,FRM,COLNO"
        'If rbtBoth.Checked = True Then
        '    strSql += vbCrLf + "  ,AMOUNT"
        'End If
        'If chkPcs.Checked = True Then
        '    strSql += vbCrLf + " ,PCS"
        'End If
        strSql += vbCrLf + "ORDER BY COLNO "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMP" & systemId & "CATSTOCKSUMMARY SET GRSWT=((SELECT SUM(GRSWT) FROM TEMP" & systemId & "CATSTOCKSUMMARY WHERE CATNAME IN ('GOLD ORNAMENTS'))"
        strSql += vbCrLf + " +(SELECT SUM(GRSWT) FROM TEMP" & systemId & "CATSTOCKSUMMARY WHERE CATNAME IN ('GOLD ORN VAT EXEMPTED'))),"
        ' strSql += vbCrLf + "  -((SELECT SUM(NETWT) FROM TEMP" & systemId & "CATSTOCKSUMMARY WHERE CATNAME IN ('Studed Diamond in G.Orn')))) , "
        strSql += vbCrLf + " NETWT=((SELECT SUM(NETWT) FROM TEMP" & systemId & "CATSTOCKSUMMARY WHERE CATNAME IN ('GOLD ORNAMENTS')) "
        strSql += vbCrLf + " +(SELECT SUM(NETWT) FROM TEMP" & systemId & "CATSTOCKSUMMARY WHERE CATNAME IN ('GOLD ORN VAT EXEMPTED'))"
        'strSql += vbCrLf + " -((SELECT SUM(NETWT) FROM TEMP" & systemId & "CATSTOCKSUMMARY WHERE CATNAME IN ('Studed Diamond in G.Orn')))"
        strSql += vbCrLf + " ) WHERE CATNAME='GOLD ORNAMENTS' AND METALNAME='GOLD' "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
    End Function

    Function funcDateWise() As Integer
        '--TRANDATE WISE CATSTOCK
        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "CATSTOCKTRANDATE')>0"
        strSql += vbcrlf + " DROP TABLE TEMP" & systemId & "CATSTOCKTRANDATE"
        strSql += vbCrLf + " SELECT SPACE(50) PARTICULAR,' 'METALNAME,CATNAME,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(OPENING)OPENING,"
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
        strSql += vbCrLf + " DECLARE @TRANDATE SMALLDATETIME"
        strSql += vbCrLf + " DECLARE @GRSWT NUMERIC(12,3)"
        strSql += vbCrLf + " DECLARE @NETWT NUMERIC(12,3)"
        strSql += vbCrLf + " DECLARE @OPENING NUMERIC(12,3)"
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
        strSql += vbCrLf + "         FOR SELECT TRANDATE,GRSWT,NETWT,OPENING,ISSUE,RECEIPT,CLOSING FROM TEMP" & systemId & "CATSTOCKTRANDATE WHERE CATNAME = @CATNAME"
        strSql += vbcrlf + "         OPEN CURTRANDATE"
        strSql += vbcrlf + "         WHILE 1=1"
        strSql += vbcrlf + " 	BEGIN"
        strSql += vbcrlf + " 		UPDATE TEMP" & systemId & "CATSTOCKTRANDATE SET CLOSING = OPENING+RECEIPT-ISSUE WHERE TRANDATE = @TRANDATE AND CATNAME = @CATNAME"
        strSql += vbCrLf + "  		FETCH NEXT FROM CURTRANDATE INTO @TRANDATE,@GRSWT,@NETWT,@OPENING,@ISSUE,@RECEIPT,@CLOSING"
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
        Qry += " AND TRANTYPE <> 'WOT'"
        If cmbCatName.Text <> "ALL" Then
            Qry += " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY "
            Qry += " WHERE CATNAME = '" & cmbCatName.Text & "')"
        End If
        If cmbMetal.Text <> "ALL" Then
            Qry += " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY "
            Qry += " WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST "
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
        Qry += " AND TRANTYPE <> 'WOT'"
        If cmbCatName.Text <> "ALL" Then
            Qry += " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY "
            Qry += " WHERE CATNAME = '" & cmbCatName.Text & "')"
        End If
        If cmbMetal.Text <> "ALL" Then
            Qry += " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY "
            Qry += " WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST "
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

    Private Sub frmCategoryStock_New_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Escape) Then
            dtpFrom.Focus()
        ElseIf e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        gridView.DataSource = Nothing
        pnlHeading.Visible = False
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        ftrIssue = funcIssueFiltration()
        ftrReceipt = funcReceiptFiltration()
        Dim CompanyFilt As String = funcCompanyFilt()

        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "CATSTOCK')>0"
        strSql += vbCrLf + " DROP TABLE TEMP" & systemId & "CATSTOCK"
        'strSql += vbCrLf + " DECLARE @frmDATE SMALLDATETIME "
        strSql += vbCrLf + " DECLARE @toDATE SMALLDATETIME "

        'strSql += vbCrLf + " SELECT @frmDATE = '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT @toDATE = '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " SELECT"
        strSql += vbCrLf + " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST "
        strSql += vbCrLf + " WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..CATEGORY "
        strSql += vbCrLf + " WHERE CATCODE=X.CATCODE))AS METALNAME"
        strSql += vbCrLf + " ,CASE WHEN FRM = 'DAYBOOK' THEN 'DAYBOOK CASH' WHEN FRM = 'SMITH GOLD' THEN 'SMITH BALANCE (Issued for Mfr)' "
        strSql += vbCrLf + " WHEN FRM = 'SMITH SILVER' THEN 'SILVER SMITH BALANCE (Issued for Mfr)' "
        strSql += vbCrLf + " WHEN FRM = 'GOLD CHARGES' THEN 'Making Charges Outside Contract For GOLD' WHEN FRM = 'SILVER CHARGES' THEN 'Making Charges Outside Contract For SILVER'"
        strSql += vbCrLf + "  WHEN FRM = 'STUDED DIAMOND' THEN 'Studed Diamond in G.Orn ' WHEN FRM = 'DIAMOND STOCK' THEN 'Diamond Stock'"
        strSql += vbCrLf + " WHEN FRM = '1LOOSE' AND (SELECT METALID FROM " & cnAdminDb & "..CATEGORY "
        strSql += vbCrLf + " WHERE CATCODE = X.CATCODE) IN ('D','T') THEN (SELECT CATNAME "
        strSql += vbCrLf + " FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = X.CATCODE) + '[L]'"
        strSql += vbCrLf + "      WHEN FRM = '2STUD' AND (SELECT METALID FROM " & cnAdminDb & "..CATEGORY "
        strSql += vbCrLf + " WHERE CATCODE = X.CATCODE) IN ('D','T') THEN (SELECT CATNAME "
        strSql += vbCrLf + " FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = X.CATCODE)+ '[S]'"
        strSql += vbCrLf + "      ELSE (SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
        strSql += vbCrLf + " WHERE CATCODE = X.CATCODE) END AS CATNAME"
        If chkPcs.Checked = True Then
            strSql += vbCrLf + ",PCS"
        End If
        strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP IN ('REC_OPENING','RECEIPT') THEN GRSWT ELSE -1*GRSWT END),0)AS GRSWT"
        strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP IN ('REC_OPENING','RECEIPT') THEN NETWT ELSE -1*NETWT END),0)AS NETWT"
        strSql += vbCrLf + " ,' 'RATE"
        strSql += vbCrLf + " ,' 'VALUE"
        strSql += vbCrLf + " ,CATCODE"
        strSql += vbCrLf + " ,1 RESULT"
        strSql += vbCrLf + " ,' 'COLHEAD"
        strSql += vbCrLf + " ,FRM"
        strSql += vbCrLf + " ,YYEAR,MMONTHID,MMONTH,TRANDATE,TRANNO"
        strSql += vbCrLf + " ,AMOUNT,COLNO"
        strSql += vbCrLf + " INTO TEMP" & systemId & "CATSTOCK"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        'DAY BOOK CASH
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " 'DAYBOOK'FRM,'DAYBOOK CASH'SEP,0 CATCODE"
        If chkPcs.Checked = True Then
            strSql += vbCrLf + ",'' PCS"
        End If
        strSql += vbCrLf + " ,0 GRSWT"
        strSql += vbCrLf + " ,0 NETWT"
        strSql += vbCrLf + " ,@toDATE  TRANDATE,-1 TRANNO,'' TRANSTATUS"
        strSql += vbCrLf + " ,DATEPART(YEAR,@toDATE)YYEAR,DATEPART(MONTH,@toDATE )MMONTHID,"
        strSql += vbCrLf + " LEFT(DATENAME(MONTH,@toDATE),3)MMONTH,"
        strSql += vbCrLf + " ISNULL((SELECT SUM(DEBIT-CREDIT) FROM " & cnStockDb & "..OPENTRAILBALANCE "
        strSql += vbCrLf + " WHERE ACCODE IN ('CASH')  " & CompanyFilt & "),0)+"
        strSql += vbCrLf + " ISNULL((SELECT SUM(CASE WHEN TRANMODE = 'D' THEN ISNULL(AMOUNT,0) ELSE -1*ISNULL(AMOUNT,0) END) "
        strSql += vbCrLf + " FROM " & cnStockDb & "..ACCTRAN WHERE TRANDATE <=  @toDATE  AND ACCODE IN ('CASH') AND ISNULL(CANCEL,'') <> 'Y' "
        strSql += vbCrLf + " " & CompanyFilt & "),0)AMOUNT,1 COLNO"

        strSql += vbCrLf + " UNION ALL "
        'SILVER CHARGES
        strSql += vbCrLf + " SELECT 'SILVER CHARGES'FRM,''SEP,CATCODE"
        If chkPcs.Checked = True Then
            strSql += vbCrLf + " ,'' PCS"
        End If
        strSql += vbCrLf + " ,0 GRSWT ,0 NETWT,''TRANDATE,''TRANNO,''TRANSTATUS "
        strSql += vbCrLf + " ,''YYEAR,''MMONTHID,''MMONTH,AMOUNT,''COLNO FROM ("
        strSql += vbCrLf + " SELECT SUM(ISNULL(DEBIT,0)-ISNULL(CREDIT,0)) AS AMOUNT, 0 AS CATCODE"
        strSql += vbCrLf + " ,'S' AS METALID FROM  " & cnStockDb & "..OPENTRAILBALANCE I WHERE  ACCODE='SMAKP' "
        strSql += vbCrLf + CompanyFilt
        strSql += vbCrLf + " GROUP BY ACCODE "
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT SUM(CASE WHEN TRANMODE='D' THEN AMOUNT ELSE (-1)*AMOUNT END ) AS AMOUNT ,0 AS CATCODE "
        strSql += vbCrLf + " ,'S' AS METALID"
        strSql += vbCrLf + " FROM  " & cnStockDb & "..ACCTRAN I WHERE TRANDATE <=@toDATE AND ISNULL(CANCEL,'') = '' AND ACCODE='SMAKP' "
        strSql += vbCrLf + CompanyFilt
        strSql += vbCrLf + " GROUP BY ACCODE"
        strSql += vbCrLf + " ) X"
        strSql += vbCrLf + " UNION ALL "
        'GOLD CHARGES
        strSql += vbCrLf + " SELECT 'GOLD CHARGES'FRM,''SEP,CATCODE"
        If chkPcs.Checked = True Then
            strSql += vbCrLf + " ,'' PCS"
        End If
        strSql += vbCrLf + " ,0 GRSWT ,0 NETWT,''TRANDATE,''TRANNO,''TRANSTATUS "
        strSql += vbCrLf + "  ,''YYEAR,''MMONTHID,''MMONTH,AMOUNT,''COLNO FROM ("
        strSql += vbCrLf + " SELECT SUM(ISNULL(DEBIT,0)-ISNULL(CREDIT,0)) AS AMOUNT, 0 AS CATCODE"
        strSql += vbCrLf + " ,'G' AS METALID FROM  " & cnStockDb & "..OPENTRAILBALANCE I WHERE "
        strSql += vbCrLf + " ACCODE='GMAKP' "
        strSql += vbCrLf + CompanyFilt
        strSql += vbCrLf + " GROUP BY ACCODE "
        strSql += vbCrLf + " UNION ALL "
        strSql += vbCrLf + " SELECT SUM(CASE WHEN TRANMODE='D' THEN AMOUNT ELSE (-1)*AMOUNT END ) AS AMOUNT ,0 AS CATCODE "
        strSql += vbCrLf + " ,'G' AS METALID"
        strSql += vbCrLf + " FROM  " & cnStockDb & "..ACCTRAN I WHERE TRANDATE <=@toDATE AND ISNULL(CANCEL,'') = '' AND ACCODE='GMAKP' "
        strSql += vbCrLf + CompanyFilt
        strSql += vbCrLf + " GROUP BY ACCODE"
        strSql += vbCrLf + " )X "
        strSql += vbCrLf + " UNION ALL "
        'SMITH SILVER BALANCE
        strSql += vbCrLf + " SELECT 'SMITH SILVER'FRM,'REC_OPENING'SEP,CATCODE"
        If chkPcs.Checked = True Then
            strSql += vbCrLf + " ,'' PCS"
        End If
        strSql += vbCrLf + " ,GRSWT ,NETWT,''TRANDATE,''TRANNO,''TRANSTATUS "
        strSql += vbCrLf + " ,''YYEAR,''MMONTHID,''MMONTH,0 AMOUNT,''COLNO FROM ("
        strSql += vbCrLf + " SELECT"
        strSql += vbCrLf + " (GRSWT+ISNULL(WASTAGE,0)+ISNULL(ALLOY,0)) GRSWT"
        strSql += vbCrLf + " ,(NETWT+ISNULL(WASTAGE,0)+ISNULL(ALLOY,0)) NETWT"
        strSql += vbCrLf + " ,I.OCATCODE AS CATCODE"
        strSql += vbCrLf + " ,(SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS METALID"
        strSql += vbCrLf + " FROM  " & cnStockDb & "..RECEIPT I"
        strSql += vbCrLf + " WHERE TRANDATE <=@toDATE"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND METALID IN ('S')"
        strSql += vbCrLf + " AND OCATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN ('S'))"
        strSql += vbCrLf + " AND OCATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY )"
        strSql += vbCrLf + CompanyFilt
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND TRANTYPE NOT IN ('IPU','RPU')"
        strSql += vbCrLf + "  ) X"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'SMITH SILVER'FRM,'ISS_OPENING'SEP,CATCODE"
        If chkPcs.Checked = True Then
            strSql += vbCrLf + " ,'' PCS"
        End If
        strSql += vbCrLf + " ,GRSWT ,NETWT,''TRANDATE,''TRANNO,''TRANSTATUS "
        strSql += vbCrLf + " ,''YYEAR,''MMONTHID,''MMONTH,0 AMOUNT,''COLNO FROM ("
        strSql += vbCrLf + " SELECT"
        strSql += vbCrLf + " GRSWT+ISNULL(WASTAGE,0)+ISNULL(ALLOY,0) GRSWT"
        strSql += vbCrLf + " ,NETWT+ISNULL(WASTAGE,0)+ISNULL(ALLOY,0) NETWT"
        strSql += vbCrLf + " , I.OCATCODE AS CATCODE"
        strSql += vbCrLf + " ,(SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS METALID"
        strSql += vbCrLf + " FROM  " & cnStockDb & "..ISSUE I"
        strSql += vbCrLf + " WHERE TRANDATE <= @toDATE"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND METALID IN('S')"
        strSql += vbCrLf + " AND OCATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN ('S'))"
        strSql += vbCrLf + " AND OCATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY )"
        strSql += vbCrLf + CompanyFilt
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND TRANTYPE NOT IN ('IPU','RPU')"
        strSql += vbCrLf + "  ) X"
        strSql += vbCrLf + " UNION ALL"
        'SMITH GOLD BALANCE
        strSql += vbCrLf + " SELECT 'SMITH GOLD'FRM,'REC_OPENING'SEP,CATCODE"
        If chkPcs.Checked = True Then
            strSql += vbCrLf + " ,'' PCS"
        End If
        strSql += vbCrLf + " ,GRSWT ,NETWT,''TRANDATE,''TRANNO,''TRANSTATUS "
        strSql += vbCrLf + " ,''YYEAR,''MMONTHID,''MMONTH,0 AMOUNT,''COLNO FROM ("
        strSql += vbCrLf + " SELECT"
        strSql += vbCrLf + " (GRSWT+ISNULL(WASTAGE,0)+ISNULL(ALLOY,0)) GRSWT"
        strSql += vbCrLf + " ,(NETWT+ISNULL(WASTAGE,0)+ISNULL(ALLOY,0)) NETWT"
        strSql += vbCrLf + " ,I.OCATCODE AS CATCODE"
        strSql += vbCrLf + " ,(SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS METALID"
        strSql += vbCrLf + " FROM  " & cnStockDb & "..RECEIPT I"
        strSql += vbCrLf + " WHERE TRANDATE <=@toDATE"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND METALID IN ('G')"
        strSql += vbCrLf + " AND OCATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN ('G'))"
        strSql += vbCrLf + " AND OCATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY )"
        strSql += vbCrLf + CompanyFilt
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND TRANTYPE NOT IN ('IPU','RPU')"
        strSql += vbCrLf + "  )X "
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT 'SMITH GOLD'FRM,'ISS_OPENING'SEP,CATCODE"
        If chkPcs.Checked = True Then
            strSql += vbCrLf + " ,'' PCS"
        End If
        strSql += vbCrLf + " ,GRSWT ,NETWT,''TRANDATE,''TRANNO,''TRANSTATUS "
        strSql += vbCrLf + " ,''YYEAR,''MMONTHID,''MMONTH,0 AMOUNT,''COLNO FROM ("
        strSql += vbCrLf + " SELECT"
        strSql += vbCrLf + " GRSWT+ISNULL(WASTAGE,0)+ISNULL(ALLOY,0) GRSWT"
        strSql += vbCrLf + " ,NETWT+ISNULL(WASTAGE,0)+ISNULL(ALLOY,0) NETWT"
        strSql += vbCrLf + " , I.OCATCODE AS CATCODE"
        strSql += vbCrLf + " ,(SELECT METALID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = I.CATCODE)AS METALID"
        strSql += vbCrLf + " FROM  " & cnStockDb & "..ISSUE I"
        strSql += vbCrLf + " WHERE TRANDATE <= @toDATE"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND METALID IN('G')"
        strSql += vbCrLf + " AND OCATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN ('G'))"
        strSql += vbCrLf + " AND OCATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY )"
        strSql += vbCrLf + CompanyFilt
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND TRANTYPE NOT IN ('IPU','RPU')"
        strSql += vbCrLf + "  )X "
        strSql += vbCrLf + " UNION ALL"
        'DIAMOND STOCK
        strSql += vbCrLf + " SELECT 'DIAMOND STOCK'FRM,'REC_OPENING'SEP,''CATCODE"
        strSql += vbCrLf + " ,'' PCS"
        strSql += vbCrLf + " ,SUM(GRSWT)GRSWT ,SUM(NETWT)NETWT,''TRANDATE,''TRANNO,''TRANSTATUS "
        strSql += vbCrLf + " ,''YYEAR,''MMONTHID,''MMONTH,0 AMOUNT,''COLNO FROM ("
        strSql += vbCrLf + " SELECT"
        strSql += vbCrLf + " ISNULL(SUM(S.STNWT),0) AS GRSWT,ISNULL(SUM(S.STNWT),0) AS NETWT"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE AS S"
        strSql += vbCrLf + " --LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMTAG AS T ON T.SNO=S.TAGSNO  "
        strSql += vbCrLf + " WHERE S.STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN('D') AND STUDDEDSTONE IN ('Y'))"
        If chkCmbCompany.Text <> "ALL" Then
            strSql += vbCrLf + " AND S.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        Else
            strSql += vbCrLf + " AND S.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
        End If
        strSql += vbCrLf + "  AND T.RECDATE <=@toDATE"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT"
        strSql += vbCrLf + " ISNULL(SUM(S.STNWT),0) AS GRSWT,ISNULL(SUM(S.STNWT),0) AS NETWT"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAGSTONE AS S"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMNONTAG AS T ON T.SNO=S.TAGSNO  "
        strSql += vbCrLf + " WHERE S.STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN('D') AND STUDDEDSTONE IN ('Y'))"
        If chkCmbCompany.Text <> "ALL" Then
            strSql += vbCrLf + " AND S.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        Else
            strSql += vbCrLf + " AND S.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
        End If
        strSql += vbCrLf + " AND T.RECDATE <=@toDATE"
        strSql += vbCrLf + " ) X"

        strSql += vbCrLf + " UNION ALL"
        'STUDED DIAMOND
        strSql += vbCrLf + " SELECT 'STUDED DIAMOND'FRM,'REC_OPENING'SEP,''CATCODE"
        strSql += vbCrLf + " ,'' PCS"
        strSql += vbCrLf + " ,SUM(GRSWT)GRSWT ,SUM(NETWT)NETWT,''TRANDATE,''TRANNO,''TRANSTATUS "
        strSql += vbCrLf + " ,''YYEAR,''MMONTHID,''MMONTH,0 AMOUNT,''COLNO FROM ("
        strSql += vbCrLf + " SELECT"
        strSql += vbCrLf + " ISNULL(SUM(S.GRSWT),0) AS GRSWT,ISNULL(SUM(S.NETWT),0) AS NETWT"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS S"
        strSql += vbCrLf + " --LEFT JOIN SENADMINDB..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMTAG AS T ON T.SNO=S.SNO  "
        strSql += vbCrLf + " WHERE S.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN('D') AND STUDDEDSTONE IN ('Y'))"
        If chkCmbCompany.Text <> "ALL" Then
            strSql += vbCrLf + " AND S.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        Else
            strSql += vbCrLf + " AND S.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
        End If

        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT"
        strSql += vbCrLf + " ISNULL(SUM(S.GRSWT),0) AS GRSWT,ISNULL(SUM(S.NETWT),0) AS NETWT"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMNONTAG AS S"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMNONTAG AS T ON T.SNO=S.SNO  "
        strSql += vbCrLf + " WHERE S.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE IN('D') AND STUDDEDSTONE IN ('Y'))"
        If chkCmbCompany.Text <> "ALL" Then
            strSql += vbCrLf + " AND S.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        Else
            strSql += vbCrLf + " AND S.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
        End If

        strSql += vbCrLf + " ) X"
        strSql += vbCrLf + " UNION ALL"
        'DIVINITY PRODUCTS
        strSql += vbCrLf + " SELECT '1LOOSE'FRM,'REC_OPENING'SEP,CATCODE"
        strSql += vbCrLf + " ,PCS"
        strSql += vbCrLf + " ,0 GRSWT ,0 NETWT,''TRANDATE,''TRANNO,''TRANSTATUS "
        strSql += vbCrLf + " ,''YYEAR,''MMONTHID,''MMONTH,0 AMOUNT,''COLNO FROM ("
        strSql += vbCrLf + " SELECT"
        strSql += vbCrLf + " ISNULL(I.CATCODE,'') CATCODE,"
        strSql += vbCrLf + " SUM(CASE WHEN (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.PCS ELSE 0 END) AS PCS"
        strSql += vbCrLf + " FROM " & cnStockDb & "..OPENWEIGHT I "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
        strSql += vbCrLf + " WHERE I.STOCKTYPE = 'C'  AND I.TRANTYPE='R' "
        If chkCmbCompany.Text <> "ALL" Then
            strSql += vbCrLf + " AND I.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        Else
            strSql += vbCrLf + " AND I.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
        End If
        strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN ('DIVINITY')))"
        strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN ('DIVINITY PRODUCTS'))"
        strSql += vbCrLf + " GROUP BY I.TRANTYPE,I.CATCODE"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT  ISNULL(I.CATCODE,'') CATCODE"
        strSql += vbCrLf + " ,SUM(PCS)"
        strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS I"
        strSql += vbCrLf + " WHERE I.TRANDATE <= @toDATE"
        strSql += vbCrLf + CompanyFilt
        strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN ('DIVINITY PRODUCTS'))"
        strSql += vbCrLf + " GROUP BY ITEMID,I.CATCODE  "
        strSql += vbCrLf + " ) X"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT '1LOOSE'FRM,'ISS_OPENING'SEP,CATCODE"
        strSql += vbCrLf + " ,PCS"
        strSql += vbCrLf + " ,0 GRSWT ,0 NETWT,''TRANDATE,''TRANNO,''TRANSTATUS "
        strSql += vbCrLf + " ,''YYEAR,''MMONTHID,''MMONTH,0 AMOUNT,''COLNO FROM ("
        strSql += vbCrLf + " SELECT"
        strSql += vbCrLf + " ISNULL(I.CATCODE,'') CATCODE,"
        strSql += vbCrLf + " SUM(CASE WHEN (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.PCS ELSE 0 END) AS PCS"
        strSql += vbCrLf + " FROM " & cnStockDb & "..OPENWEIGHT I "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
        strSql += vbCrLf + " WHERE I.STOCKTYPE = 'C'  AND I.TRANTYPE='I' "
        If chkCmbCompany.Text <> "ALL" Then
            strSql += vbCrLf + " AND I.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        Else
            strSql += vbCrLf + " AND I.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
        End If

        strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN ('DIVINITY')))"
        strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN ('DIVINITY PRODUCTS'))"
        strSql += vbCrLf + " GROUP BY I.TRANTYPE,I.CATCODE"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT  ISNULL(I.CATCODE,'') CATCODE"
        strSql += vbCrLf + " ,SUM(PCS)*-1"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
        strSql += vbCrLf + " WHERE I.TRANDATE <= @toDATE"
        strSql += vbCrLf + CompanyFilt
        strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
        strSql += vbCrLf + " AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN ('DIVINITY PRODUCTS'))"
        strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = I.BATCHNO AND TRANTYPE = 'RD')"
        strSql += vbCrLf + " GROUP BY ITEMID,I.CATCODE  "
        strSql += vbCrLf + " ) X"
        strSql += vbCrLf + " UNION ALL"

        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " '1LOOSE'FRM,'ISS_OPENING'SEP,CATCODE"
        If chkPcs.Checked = True Then
            strSql += vbCrLf + " ,'' PCS"
        End If
        If rbtGeneral.Checked = True Then
            strSql += vbCrLf + " ,SUM(GRSWT)GRSWT"
            strSql += vbCrLf + " ,SUM(NETWT)NETWT"
        ElseIf rbtCarat.Checked = True Then
            strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'G' THEN GRSWT * 5 ELSE GRSWT END)GRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'G' THEN NETWT * 5 ELSE NETWT END)NETWT"
        Else
            strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'C' THEN GRSWT / 5 ELSE GRSWT END)GRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'C' THEN NETWT / 5 ELSE NETWT END)NETWT"
        End If
        strSql += vbCrLf + " ,@toDATE TRANDATE,-1 TRANNO,TRANSTATUS"
        strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
        strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
        strSql += vbCrLf + " ,0 AMOUNT,'' COLNO"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
        strSql += vbCrLf + " WHERE TRANDATE < @toDATE AND ISNULL(CANCEL,'') = ''"
        If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(TRANTYPE,'') NOT IN  ('AI') "
        strSql += vbCrLf + ftrIssue
        strSql += vbCrLf + CompanyFilt
        strSql += vbCrLf + " GROUP BY CATCODE,TRANDATE,TRANNO,TRANSTATUS"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " '1LOOSE'FRM,'REC_OPENING'SEP,CATCODE"
        If chkPcs.Checked = True Then
            strSql += vbCrLf + " ,'' PCS"
        End If
        If rbtGeneral.Checked = True Then
            strSql += vbCrLf + " ,SUM(GRSWT)GRSWT"
            strSql += vbCrLf + " ,SUM(NETWT)NETWT"
        ElseIf rbtCarat.Checked = True Then
            strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'G' THEN GRSWT * 5 ELSE GRSWT END)GRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'G' THEN NETWT * 5 ELSE NETWT END)NETWT"
        Else
            strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'C' THEN GRSWT / 5 ELSE GRSWT END)GRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'C' THEN NETWT / 5 ELSE NETWT END)NETWT"
        End If
        strSql += vbCrLf + " ,@toDATE TRANDATE,-1 TRANNO,TRANSTATUS"
        strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
        strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
        strSql += vbCrLf + " ,0 AMOUNT,'' COLNO"
        strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS R"
        strSql += vbCrLf + " WHERE TRANDATE < @toDATE AND ISNULL(CANCEL,'') = ''"
        If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(TRANTYPE,'') NOT IN  ('AR') "
        strSql += vbCrLf + ftrReceipt
        strSql += vbCrLf + CompanyFilt

        'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
        strSql += vbCrLf + " GROUP BY CATCODE,TRANDATE,TRANNO,TRANSTATUS"

        ''*****************************OPENWEIGHT
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " '1LOOSE'FRM,'REC_OPENING'SEP,CATCODE"
        If chkPcs.Checked = True Then
            strSql += vbCrLf + ",'' PCS"
        End If
        If rbtGeneral.Checked = True Then
            strSql += vbCrLf + " ,SUM(GRSWT)GRSWT"
            strSql += vbCrLf + " ,SUM(NETWT)NETWT"

        ElseIf rbtCarat.Checked = True Then
            strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'G' THEN GRSWT * 5 ELSE GRSWT END)GRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'G' THEN NETWT * 5 ELSE NETWT END)NETWT"
        Else
            strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'C' THEN GRSWT / 5 ELSE GRSWT END)GRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'C' THEN NETWT / 5 ELSE NETWT END)NETWT"
        End If
        strSql += vbCrLf + " ,@toDATE TRANDATE,-1 TRANNO,' 'TRANSTATUS"
        strSql += vbCrLf + " ,DATEPART(YEAR,@toDATE)YYEAR,DATEPART(MONTH,@toDATE)MMONTHID,"
        strSql += vbCrLf + " LEFT(DATENAME(MONTH,@toDATE),3)MMONTH"
        strSql += vbCrLf + " ,0 AMOUNT,'' COLNO"
        strSql += vbCrLf + " FROM " & cnStockDb & "..OPENWEIGHT AS R"
        strSql += vbCrLf + " WHERE STOCKTYPE = 'C' AND TRANTYPE='R'"
        strSql += vbCrLf + ftrReceipt
        strSql += vbCrLf + CompanyFilt
        strSql += vbCrLf + " GROUP BY CATCODE"

        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " '1LOOSE'FRM,'ISS_OPENING'SEP,CATCODE"
        If chkPcs.Checked = True Then
            strSql += vbCrLf + ",'' PCS"
        End If
        If rbtGeneral.Checked = True Then
            strSql += vbCrLf + " ,SUM(GRSWT)GRSWT"
            strSql += vbCrLf + " ,SUM(NETWT)NETWT"

        ElseIf rbtCarat.Checked = True Then
            strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'G' THEN GRSWT * 5 ELSE GRSWT END)GRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'G' THEN NETWT * 5 ELSE NETWT END)NETWT"
        Else
            strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'C' THEN GRSWT / 5 ELSE GRSWT END)GRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'C' THEN NETWT / 5 ELSE NETWT END)NETWT"
        End If
        strSql += vbCrLf + " ,@toDATE TRANDATE,-1 TRANNO,' 'TRANSTATUS"
        strSql += vbCrLf + " ,DATEPART(YEAR,@toDATE)YYEAR,DATEPART(MONTH,@toDATE)MMONTHID,"
        strSql += vbCrLf + " LEFT(DATENAME(MONTH,@toDATE),3)MMONTH"
        strSql += vbCrLf + " ,0 AMOUNT,'' COLNO"
        strSql += vbCrLf + " FROM " & cnStockDb & "..OPENWEIGHT AS R"
        strSql += vbCrLf + " WHERE STOCKTYPE = 'C'  AND TRANTYPE='I'"
        strSql += vbCrLf + ftrReceipt
        strSql += vbCrLf + CompanyFilt
        strSql += vbCrLf + " GROUP BY CATCODE"


        ''******************************
        strSql += vbCrLf + " UNION ALL"

        strSql += vbCrLf + " SELECT '1LOOSE'FRM,'ISSUE'SEP,CATCODE,'' PCS ,GRSWT ,NETWT ,TRANDATE,TRANNO,TRANSTATUS "
        strSql += vbCrLf + " ,YYEAR,MMONTHID,MMONTH,0 AMOUNT,''COLNO FROM ("
        strSql += vbCrLf + " SELECT CATCODE"
        If chkPcs.Checked = True Then
            strSql += vbCrLf + ",'' PCS"
        End If
        If rbtGeneral.Checked = True Then
            strSql += vbCrLf + " ,SUM(GRSWT)GRSWT"
            strSql += vbCrLf + " ,SUM(NETWT)NETWT"
        ElseIf rbtCarat.Checked = True Then
            strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'G' THEN GRSWT * 5 ELSE GRSWT END)GRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'G' THEN NETWT * 5 ELSE NETWT END)NETWT"
        Else
            strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'C' THEN GRSWT / 5 ELSE GRSWT END)GRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'C' THEN NETWT / 5 ELSE NETWT END)NETWT"
        End If
        strSql += vbCrLf + " ,TRANDATE,TRANNO,TRANSTATUS"
        strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
        strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
        strSql += vbCrLf + " ,0 AMOUNT,'' COLNO"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN @toDATE AND @toDATE AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND ISNULL(TRANTYPE,'') NOT IN  ('OD') "
        If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(TRANTYPE,'') NOT IN  ('AI') "
        'strSql += vbcrlf + " AND ISNULL(TRANTYPE,'') NOT IN  ('OD','RD') "
        strSql += vbCrLf + ftrIssue
        'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
        strSql += vbCrLf + CompanyFilt
        strSql += vbCrLf + " GROUP BY CATCODE,TRANDATE,TRANNO,TRANSTATUS"
        ''ORDER & REPAIR
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT  "
        strSql += vbCrLf + " ISNULL(I.CATCODE,'') CATCODE "
        If chkPcs.Checked = True Then
            strSql += vbCrLf + ",'' PCS"
        End If
        If rbtGeneral.Checked = True Then
            strSql += vbCrLf + " ,SUM(GRSWT)GRSWT"
            strSql += vbCrLf + " ,SUM(NETWT)NETWT"
        Else
            strSql += vbCrLf + " ,SUM(GRSWT * 5)GRSWT"
            strSql += vbCrLf + " ,SUM(NETWT * 5)NETWT"
        End If
        strSql += vbCrLf + " ,TRANDATE,TRANNO,TRANSTATUS"
        strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
        strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
        strSql += vbCrLf + " ,0 AMOUNT,'' COLNO"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS I"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN @toDATE AND @toDATE AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND RECPAY = 'P' "
        strSql += vbCrLf + ftrIssue
        'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
        strSql += vbCrLf + CompanyFilt
        strSql += vbCrLf + " GROUP BY CATCODE,TRANDATE,TRANNO,TRANSTATUS"
        strSql += vbCrLf + " ) X"

        strSql += vbCrLf + " UNION ALL"

        strSql += vbCrLf + " SELECT '1LOOSE'FRM,'RECEIPT'SEP,CATCODE ,'' PCS, GRSWT ,NETWT ,TRANDATE,TRANNO,TRANSTATUS "
        strSql += vbCrLf + " ,YYEAR,MMONTHID,MMONTH,0 AMOUNT,'' COLNO FROM ("
        strSql += vbCrLf + " SELECT CATCODE"
        If chkPcs.Checked = True Then
            strSql += vbCrLf + ",'' PCS"
        End If
        If rbtGeneral.Checked = True Then
            strSql += vbCrLf + " ,SUM(GRSWT)GRSWT"
            strSql += vbCrLf + " ,SUM(NETWT)NETWT"
        ElseIf rbtCarat.Checked = True Then
            strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'G' THEN GRSWT * 5 ELSE GRSWT END)GRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'G' THEN NETWT * 5 ELSE NETWT END)NETWT"
        Else
            strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'C' THEN GRSWT / 5 ELSE GRSWT END)GRSWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'C' THEN NETWT / 5 ELSE NETWT END)NETWT"
        End If
        strSql += vbCrLf + " ,TRANDATE,TRANNO,TRANSTATUS"
        strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
        strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
        strSql += vbCrLf + " ,0 AMOUNT,'' COLNO"
        strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS R"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN @toDATE AND @toDATE AND ISNULL(CANCEL,'') = ''"
        If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(TRANTYPE,'') NOT IN  ('AR') "
        strSql += vbCrLf + ftrReceipt
        strSql += vbCrLf + CompanyFilt
        strSql += vbCrLf + " GROUP BY CATCODE,TRANDATE,TRANNO,TRANSTATUS"
        ''ORDER & REPAIR
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT  "
        strSql += vbCrLf + " ISNULL(I.CATCODE,'') CATCODE "
        If chkPcs.Checked = True Then
            strSql += vbCrLf + ",'' PCS"
        End If
        If rbtGeneral.Checked = True Then
            strSql += vbCrLf + " ,SUM(GRSWT)GRSWT"
            strSql += vbCrLf + " ,SUM(NETWT)NETWT"
        Else
            strSql += vbCrLf + " ,SUM(GRSWT * 5)GRSWT"
            strSql += vbCrLf + " ,SUM(NETWT * 5)NETWT"
        End If
        strSql += vbCrLf + " ,TRANDATE,TRANNO,TRANSTATUS"
        strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
        strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
        strSql += vbCrLf + " ,0 AMOUNT,'' COLNO"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS I"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN @toDATE AND @toDATE AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND RECPAY = 'R' AND ISNULL(TRANTYPE,'') IN ('O','R')"
        strSql += vbCrLf + ftrIssue
        'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
        strSql += vbCrLf + CompanyFilt
        strSql += vbCrLf + " GROUP BY CATCODE,TRANDATE,TRANNO,TRANSTATUS"
        strSql += vbCrLf + " ) X"


        If chkWithStuddedStone.Checked = True Then
            strSql += vbCrLf + " UNION ALL"

            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '2STUD'FRM,'ISS_OPENING'SEP,CATCODE"
            If chkPcs.Checked = True Then
                strSql += vbCrLf + ",'' PCS"
            End If
            If rbtGeneral.Checked = True Then
                strSql += vbCrLf + " ,SUM(STNWT)GRSWT"
                strSql += vbCrLf + " ,SUM(STNWT)NETWT"
            ElseIf rbtCarat.Checked = True Then
                strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT * 5 ELSE STNWT END)GRSWT"
                strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT * 5 ELSE STNWT END)NETWT"
            Else
                strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT / 5 ELSE STNWT END)GRSWT"
                strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT / 5 ELSE STNWT END)NETWT"
            End If
            strSql += vbCrLf + " ,@toDATE TRANDATE,-1 TRANNO,TRANSTATUS"
            strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
            strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
            strSql += vbCrLf + " ,0 AMOUNT,'' COLNO"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE AS I"
            strSql += vbCrLf + " WHERE TRANDATE <= @toDATE"
            strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..ISSUE "
            strSql += vbCrLf + " WHERE ISNULL(CANCEL,'') = '' "
            If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(TRANTYPE,'') NOT IN  ('AI') "
            strSql += vbCrLf + " AND (TRANDATE <= @toDATE )"
            strSql += vbCrLf + ftrIssue
            strSql += vbCrLf + CompanyFilt
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " GROUP BY CATCODE,TRANDATE,TRANNO,TRANSTATUS"

            strSql += vbCrLf + " UNION ALL"

            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '2STUD'FRM,'REC_OPENING'SEP,CATCODE"
            If chkPcs.Checked = True Then
                strSql += vbCrLf + ",'' PCS"
            End If
            If rbtGeneral.Checked = True Then
                strSql += vbCrLf + " ,SUM(STNWT)GRSWT"
                strSql += vbCrLf + " ,SUM(STNWT)NETWT"
            ElseIf rbtCarat.Checked = True Then
                strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT * 5 ELSE STNWT END)GRSWT"
                strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT * 5 ELSE STNWT END)NETWT"
            Else
                strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT / 5 ELSE STNWT END)GRSWT"
                strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT / 5 ELSE STNWT END)NETWT"
            End If
            strSql += vbCrLf + " ,@toDATE TRANDATE,-1 TRANNO,TRANSTATUS"
            strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
            strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
            strSql += vbCrLf + " ,0 AMOUNT,'' COLNO"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE AS R"
            strSql += vbCrLf + " WHERE TRANDATE <= @toDATE"
            strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT "
            strSql += vbCrLf + " WHERE ISNULL(CANCEL,'') = '' "
            If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(TRANTYPE,'') NOT IN  ('AR') "
            strSql += vbCrLf + " AND (TRANDATE<= @toDATE )"
            strSql += vbCrLf + ftrReceipt
            'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
            strSql += vbCrLf + CompanyFilt
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " GROUP BY CATCODE,TRANDATE,TRANNO,TRANSTATUS"

            strSql += vbCrLf + " UNION ALL"

            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '2STUD'FRM,'ISSUE'SEP,CATCODE"
            If chkPcs.Checked = True Then
                strSql += vbCrLf + ",'' PCS"
            End If
            If rbtGeneral.Checked = True Then
                strSql += vbCrLf + " ,SUM(STNWT)GRSWT"
                strSql += vbCrLf + " ,SUM(STNWT)NETWT"
            ElseIf rbtCarat.Checked = True Then
                strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT * 5 ELSE STNWT END)GRSWT"
                strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT * 5 ELSE STNWT END)NETWT"
            Else
                strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT / 5 ELSE STNWT END)GRSWT"
                strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT / 5 ELSE STNWT END)NETWT"
            End If
            strSql += vbCrLf + " ,TRANDATE,TRANNO,TRANSTATUS"
            strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
            strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
            strSql += vbCrLf + " ,0 AMOUNT,'' COLNO"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE AS I"
            strSql += vbCrLf + " WHERE TRANDATE <= @toDATE "
            strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..ISSUE "
            strSql += vbCrLf + " WHERE ISNULL(CANCEL,'') = '' "
            If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(TRANTYPE,'') NOT IN  ('AI') "
            strSql += vbCrLf + " AND (TRANDATE <= @toDATE )"
            strSql += vbCrLf + ftrIssue
            strSql += vbCrLf + CompanyFilt
            'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " GROUP BY CATCODE,TRANDATE,TRANNO,TRANSTATUS"

            strSql += vbCrLf + " UNION ALL"

            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '2STUD'FRM,'RECEIPT'SEP,CATCODE"
            If chkPcs.Checked = True Then
                strSql += vbCrLf + ",'' PCS"
            End If
            If rbtGeneral.Checked = True Then
                strSql += vbCrLf + " ,SUM(STNWT)GRSWT"
                strSql += vbCrLf + " ,SUM(STNWT)NETWT"
            ElseIf rbtCarat.Checked = True Then
                strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT * 5 ELSE STNWT END)GRSWT"
                strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT * 5 ELSE STNWT END)NETWT"
            Else
                strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT / 5 ELSE STNWT END)GRSWT"
                strSql += vbCrLf + " ,SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT / 5 ELSE STNWT END)NETWT"
            End If
            strSql += vbCrLf + " ,TRANDATE,TRANNO,TRANSTATUS"
            strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
            strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
            strSql += vbCrLf + " ,0 AMOUNT,'' COLNO"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE AS R"
            strSql += vbCrLf + " WHERE TRANDATE <= @toDATE "
            strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT "
            strSql += vbCrLf + " WHERE ISNULL(CANCEL,'') = '' "
            If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(TRANTYPE,'') NOT IN  ('AR') "
            strSql += vbCrLf + " AND (TRANDATE <= @toDATE )"
            strSql += vbCrLf + ftrReceipt
            strSql += vbCrLf + CompanyFilt
            'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " GROUP BY CATCODE,TRANDATE,TRANNO,TRANSTATUS"
        End If
        strSql += vbCrLf + " )X"
        strSql += vbCrLf + " GROUP BY CATCODE,FRM,YYEAR,MMONTHID,MMONTH,TRANDATE,TRANNO,PCS,AMOUNT,COLNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()



        strSql = " UPDATE TEMP" & systemId & "CATSTOCK SET COLNO=2 WHERE CATNAME='GOLD ORNAMENTS'  AND METALNAME='GOLD'"
        strSql += vbCrLf + "UPDATE TEMP" & systemId & "CATSTOCK SET COLNO=3 WHERE CATNAME='OLD BEATEN GOLD'  AND METALNAME='GOLD'"
        strSql += vbCrLf + "UPDATE TEMP" & systemId & "CATSTOCK SET COLNO=4 WHERE CATNAME='BULLION FOR MANUFACTURE'  AND METALNAME='GOLD'"
        strSql += vbCrLf + "UPDATE TEMP" & systemId & "CATSTOCK SET COLNO=5 WHERE CATNAME='MELTING & PURIFYING WASTAGE'  AND METALNAME='GOLD'"
        strSql += vbCrLf + "UPDATE TEMP" & systemId & "CATSTOCK SET COLNO=6 WHERE CATNAME='MANUFACTURING WASTAGE'  AND METALNAME='GOLD'"
        strSql += vbCrLf + "UPDATE TEMP" & systemId & "CATSTOCK SET COLNO=7 WHERE FRM='SMITH GOLD'  AND METALNAME='GOLD'"
        strSql += vbCrLf + "UPDATE TEMP" & systemId & "CATSTOCK SET COLNO=8 WHERE CATNAME='BULLION FOR RESALE'  AND METALNAME='BUILION GOLD'"
        strSql += vbCrLf + "UPDATE TEMP" & systemId & "CATSTOCK SET COLNO=9 WHERE FRM='STUDED DIAMOND'  " 'AND METALNAME='DIAMOND'"
        strSql += vbCrLf + "UPDATE TEMP" & systemId & "CATSTOCK SET COLNO=10 WHERE FRM='DIAMOND STOCK' " ' AND METALNAME='DIAMOND'"
        strSql += vbCrLf + "UPDATE TEMP" & systemId & "CATSTOCK SET COLNO=11 WHERE CATNAME='SILVER ANKLETS' AND METALNAME='SILVER'"
        strSql += vbCrLf + "UPDATE TEMP" & systemId & "CATSTOCK SET COLNO=12 WHERE CATNAME='SILVER VESSELS' AND METALNAME='SILVER'"
        strSql += vbCrLf + "UPDATE TEMP" & systemId & "CATSTOCK SET COLNO=13 WHERE CATNAME='OLD BEATEN SILVER' AND METALNAME='SILVER'"
        strSql += vbCrLf + "UPDATE TEMP" & systemId & "CATSTOCK SET COLNO=14 WHERE CATNAME='SILVER BULLION FOR MANUFACTURE' AND METALNAME='SILVER'"
        strSql += vbCrLf + "UPDATE TEMP" & systemId & "CATSTOCK SET COLNO=15 WHERE CATNAME='MELTING WASTAGE SILVER' AND METALNAME='SILVER'"
        strSql += vbCrLf + "UPDATE TEMP" & systemId & "CATSTOCK SET COLNO=16 WHERE FRM='SMITH SILVER'  AND METALNAME='SILVER'"
        strSql += vbCrLf + "UPDATE TEMP" & systemId & "CATSTOCK SET COLNO=17 WHERE CATNAME='SILVER BULLION FOR RESALE' AND METALNAME='SILVER'"
        strSql += vbCrLf + "UPDATE TEMP" & systemId & "CATSTOCK SET COLNO=18 WHERE CATNAME='PUR COPPER' AND METALNAME='COPPER'"
        strSql += vbCrLf + "UPDATE TEMP" & systemId & "CATSTOCK SET COLNO=19 WHERE CATNAME='DIVINITY PRODUCTS' AND METALNAME='DIVINITY'"
        strSql += vbCrLf + "UPDATE TEMP" & systemId & "CATSTOCK SET COLNO=20 WHERE FRM='GOLD CHARGES' "
        strSql += vbCrLf + "UPDATE TEMP" & systemId & "CATSTOCK SET COLNO=21 WHERE FRM='SILVER CHARGES' "

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' "
        strSql += vbCrLf + " AND NAME = 'TEMP" & systemId & "CATSUMM')>0 "
        strSql += vbCrLf + " DROP TABLE TEMP" & systemId & "CATSUMM"
        strSql += vbCrLf + " CREATE TABLE TEMP" & systemId & "CATSUMM("
        strSql += vbCrLf + " METALNAME VARCHAR(50),"
        strSql += vbCrLf + " CATNAME VARCHAR(100),"
        If chkPcs.Checked = True Then
            strSql += vbCrLf + " PCS VARCHAR(20),"
        End If
        If rbtBoth.Checked = True Then
            strSql += vbCrLf + " GRSWT NUMERIC(12,3),"
            strSql += vbCrLf + " NETWT NUMERIC(12,3),"
            strSql += vbCrLf + " AMOUNT NUMERIC(12,2),"
        Else
            strSql += vbCrLf + " OPENING NUMERIC(12,3),"
            strSql += vbCrLf + " RECEIPT NUMERIC(12,3),"
            strSql += vbCrLf + " ISSUE NUMERIC(12,3),"
            strSql += vbCrLf + " CLOSING NUMERIC(12,3),"
        End If
        strSql += vbCrLf + " RATE VARCHAR(30),"
        strSql += vbCrLf + " VALUE VARCHAR(30),"
        strSql += vbCrLf + " CATCODE VARCHAR(15),"
        strSql += vbCrLf + " RESULT INT,"
        strSql += vbCrLf + " FRM VARCHAR(30),"
        strSql += vbCrLf + " YYEAR INT,"
        strSql += vbCrLf + " MMONTHID INT,"
        strSql += vbCrLf + " MMONTH	NVARCHAR(9),"
        strSql += vbCrLf + " DDATE VARCHAR(12),"
        strSql += vbCrLf + " TRANDATE VARCHAR(12),"
        strSql += vbCrLf + " TRANNO VARCHAR(12),"
        strSql += vbCrLf + " COLHEAD VARCHAR(2),"
        strSql += vbCrLf + " COLNO VARCHAR(2),"
        strSql += vbCrLf + " SNO INT IDENTITY)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If rbtSummary.Checked = True Then
            funcSummaryWise()
        ElseIf rbtDateWise.Checked = True Then
            funcDateWise()
        ElseIf rbtMonthWise.Checked = True Then
            funcMonthWise()
        Else
            funcTranNoWise()
        End If

        strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE XTYPE = 'U' AND "
        strSql += vbCrLf + " NAME = 'TEMP" & systemId & "CTRSTOCKTEMP')>0 "
        strSql += vbCrLf + " DROP TABLE TEMP" & systemId & "CTRSTOCKTEMP"
        strSql += vbCrLf + " SELECT PARTICULAR,METALNAME,CATNAME,"
        If chkPcs.Checked = True Then
            strSql += vbCrLf + "PCS,"
        End If
        If rbtBoth.Checked = True Then
            strSql += vbCrLf + " GRSWT, NETWT,AMOUNT, "
        Else
            strSql += vbCrLf + " OPENING, RECEIPT, ISSUE, Closing, "
        End If
        strSql += vbCrLf + " Rate, VALUE, CATCODE, "
        strSql += vbCrLf + " RESULT,FRM,YYEAR,MMONTHID,MMONTH, DDATE, TRANDATE, TRANNO, COLHEAD,COLNO "
        strSql += vbCrLf + " INTO TEMP" & systemId & "CTRSTOCKTEMP FROM ("
        If rbtSummary.Checked = True Then
            strSql += vbCrLf + " SELECT * FROM TEMP" & systemId & "CATSTOCKSUMMARY WHERE COLNO<>0"
        ElseIf rbtDateWise.Checked = True Then
            strSql += vbCrLf + " SELECT * FROM TEMP" & systemId & "CATSTOCKTRANDATE"
        ElseIf rbtMonthWise.Checked = True Then
            strSql += vbCrLf + " SELECT * FROM TEMP" & systemId & "CATSTOCKMONTH"
        Else
            strSql += vbCrLf + " SELECT * FROM TEMP" & systemId & "CATSTOCKTRANNO"
        End If
        'strSql += vbCrLf + " UNION ALL"
        'strSql += vbCrLf + " SELECT"
        'If rbtSummary.Checked = True Then
        '    strSql += vbCrLf + " 'SUB TOTAL'PARTICULAR,METALNAME,'SUB TOTAL'CATNAME"
        'Else
        '    strSql += vbCrLf + " 'SUB TOTAL'PARTICULAR,METALNAME,CATNAME"
        'End If
        'If chkPcs.Checked = True Then
        '    strSql += vbCrLf + ",SUM(PCS)"
        'End If
        'If rbtBoth.Checked = True Then
        '    strSql += vbCrLf + " ,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(AMOUNT) "
        'Else
        '    strSql += vbCrLf + " ,SUM(OPENING)OPENING,SUM(RECEIPT)RECEIPT,SUM(ISSUE)ISSUE,SUM(CLOSING)CLOSING "
        'End If
        'strSql += vbCrLf + " ,' ' RATE,' ' VALUE,' 'CATCODE,2 RESULT"
        'strSql += vbCrLf + " ,' 'FRM"
        'strSql += vbCrLf + " ,' 'YYEAR,' 'MMONTHID,' 'MMONTH,' 'DDATE,' 'TRANDATE,' 'TRANNO,'S' COLHEAD,'' COLNO"
        'strSql += vbCrLf + " FROM TEMP" & systemId & "CATSTOCK AS T"
        'If rbtSummary.Checked = True Then
        '    strSql += vbCrLf + " GROUP BY METALNAME,FRM"
        'Else
        '    strSql += vbCrLf + " GROUP BY METALNAME,CATNAME"
        'End If
        strSql += vbCrLf + " ) X"
        strSql += vbCrLf + " ORDER BY METALNAME,RESULT"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()



        'strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "CTRSTOCKTEMP)>0 "
        'strSql += vbCrLf + " BEGIN "
        'strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "CTRSTOCKTEMP(METALNAME,CATNAME,RESULT,RATE,"
        'strSql += vbCrLf + " VALUE,FRM,YYEAR,MMONTHID,MMONTH,DDATE,TRANDATE,TRANNO,COLHEAD,PARTICULAR,PCS,AMOUNT,COLNO,CATCODE )"

        'If rbtSummary.Checked = True Then
        '    If chkPcs.Checked = True Then
        '        strSql += vbCrLf + " SELECT DISTINCT METALNAME,'',0,' ',' ',' ','','','','','','',COLHEAD,'',PCS,AMOUNT,'','' "
        '    Else
        '        strSql += vbCrLf + " SELECT DISTINCT METALNAME,'',0,' ',' ',' ','','','','','','','',COLHEAD,'',AMOUNT,'','' "
        '    End If

        'ElseIf rbtDateWise.Checked = True Then
        '    strSql += vbCrLf + " SELECT DISTINCT METALNAME,CATNAME,0,' ',' ',' ','','','','','','','T','','','','' "
        'ElseIf rbtMonthWise.Checked = True Then
        '    strSql += vbCrLf + " SELECT DISTINCT METALNAME,CATNAME,0,' ',' ',' ','','','','','','','T','',''.'','' "
        'Else
        '    strSql += vbCrLf + " SELECT DISTINCT METALNAME,CATNAME,0,' ',' ',' ','','','','','','','T','','','','' "
        'End If
        'strSql += vbCrLf + " FROM TEMP" & systemId & "CTRSTOCKTEMP WHERE RESULT =1"
        'strSql += vbCrLf + " END "
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()

        'strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "CTRSTOCKTEMP)>0 "
        'strSql += vbCrLf + " BEGIN "
        'If rbtSummary.Checked = True Then
        '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "CTRSTOCKTEMP SET PARTICULAR = CASE WHEN RESULT = 0 THEN "
        '    strSql += vbCrLf + " METALNAME WHEN RESULT = 2 THEN 'SUB TOTAL' ELSE '  '+ CATNAME END"
        'ElseIf rbtMonthWise.Checked = True Then
        '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "CTRSTOCKTEMP SET PARTICULAR = CASE WHEN RESULT = 0 THEN "
        '    strSql += vbCrLf + " CATNAME WHEN RESULT = 2 THEN 'SUB TOTAL' ELSE '  '+ MMONTH END"
        'ElseIf rbtDateWise.Checked = True Then
        '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "CTRSTOCKTEMP SET PARTICULAR = CASE WHEN RESULT = 0 THEN "
        '    strSql += vbCrLf + " CATNAME WHEN RESULT = 2 THEN 'SUB TOTAL' ELSE ' '+ DDATE END"
        'Else
        '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "CTRSTOCKTEMP SET PARTICULAR = CASE WHEN RESULT = 0 THEN "
        '    strSql += vbCrLf + " CATNAME WHEN RESULT = 2 THEN 'SUB TOTAL' ELSE CONVERT(VARCHAR,TRANNO) END"
        'End If
        'strSql += vbCrLf + " END "
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()



        'strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "CTRSTOCKTEMP)>0 "
        'strSql += vbCrLf + " BEGIN "
        'strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "CATSUMM(METALNAME,CATNAME"
        'If chkPcs.Checked = True Then
        '    strSql += vbCrLf + ",PCS"
        'End If
        'If rbtBoth.Checked = True Then
        '    strSql += vbCrLf + ",GRSWT,NETWT,AMOUNT,"
        'Else
        '    strSql += vbCrLf + " ,OPENING, RECEIPT, ISSUE, Closing,"
        'End If
        'strSql += vbCrLf + "  Rate, VALUE, CATCODE, RESULT, FRM, YYEAR, MMONTHID, MMONTH, DDATE, "
        'strSql += vbCrLf + " TRANDATE, TRANNO,COLHEAD,COLNO) "

        'strSql += vbCrLf + " SELECT METALNAME,PARTICULAR,"
        'If chkPcs.Checked = True Then
        '    strSql += vbCrLf + "PCS,"
        'End If
        'If rbtBoth.Checked = True Then
        '    strSql += vbCrLf + " GRSWT, NETWT,AMOUNT,"
        'Else
        '    strSql += vbCrLf + " OPENING, RECEIPT, ISSUE,Closing,"
        'End If
        'strSql += vbCrLf + " Rate, VALUE, CATCODE, RESULT, FRM, YYEAR, MMONTHID, MMONTH, DDATE, "
        'strSql += vbCrLf + " TRANDATE, TRANNO,COLHEAD,COLNO"
        'strSql += vbCrLf + " FROM TEMP" & systemId & "CTRSTOCKTEMP WHERE COLNO<>0 "
        'If rbtSummary.Checked = True Then
        '    strSql += vbCrLf + " ORDER BY COLNO,METALNAME,RESULT"
        'Else
        '    strSql += vbCrLf + " ORDER BY CATNAME,RESULT"
        'End If
        'strSql += vbCrLf + " END "
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()

        'If chkWithValue.Checked = True Then funcRateValUpdation()

        'strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "CATSUMM)>0 "
        'strSql += vbCrLf + " BEGIN "
        'If chkPcs.Checked = True Then
        '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "CATSUMM SET PCS=NULL WHERE PCS = 0"
        'End If
        'If rbtBoth.Checked = True Then
        '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "CATSUMM SET NETWT=NULL WHERE NETWT = 0"
        '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "CATSUMM SET GRSWT=NULL WHERE GRSWT = 0"
        '    ' strSql += vbCrLf + " UPDATE TEMP" & systemId & "CATSUMM SET AMOUNT=NULL WHERE AMOUNT =0"
        'Else
        '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "CATSUMM SET OPENING=NULL WHERE OPENING = 0"
        '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "CATSUMM SET RECEIPT=NULL WHERE RECEIPT = 0"
        '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "CATSUMM SET ISSUE=NULL WHERE ISSUE = 0"
        '    strSql += vbCrLf + " UPDATE TEMP" & systemId & "CATSUMM SET CLOSING=NULL WHERE CLOSING= 0"
        'End If
        'strSql += vbCrLf + " UPDATE TEMP" & systemId & "CATSUMM SET RATE = '' WHERE RATE = '0.00'"
        'strSql += vbCrLf + " UPDATE TEMP" & systemId & "CATSUMM SET VALUE = '' WHERE VALUE='0.00'"
        'strSql += vbCrLf + " UPDATE TEMP" & systemId & "CATSUMM SET VALUE = CONVERT(VARCHAR,(SELECT CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(VALUE,'') = '' THEN '0' ELSE VALUE END))) FROM TEMP" & systemId & "CATSUMM WHERE METALNAME = T.METALNAME))"
        'strSql += vbCrLf + " FROM TEMP" & systemId & "CATSUMM AS T WHERE RESULT = 2"
        'strSql += vbCrLf + " END "
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()

        'strSql = " SELECT * FROM TEMP" & systemId & "CATSUMM ORDER BY COLNO"
        strSql = " SELECT COLNO AS SNO,* FROM TEMP" & systemId & "CTRSTOCKTEMP WHERE COLNO<>0 ORDER BY COLNO"


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
            .Columns("PARTICULAR").Visible = False
            With .Columns("CATNAME")
                .HeaderText = "PARTICULAR"
                .ReadOnly = True
                .Width = 400
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            If chkPcs.Checked = True Then
                With .Columns("PCS")
                    .HeaderText = "PCS"
                    .ReadOnly = True
                    .Width = 50
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            Else
                .Columns("PCS").Visible = False
            End If
            If rbtBoth.Checked = True Then
                With .Columns("SNO")
                    .HeaderText = "SNO"
                    .ReadOnly = True
                    .Width = 35
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .Visible = True
                End With
                With .Columns("GRSWT")
                    .HeaderText = "GRSWT"
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .ReadOnly = True
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("NETWT")
                    .HeaderText = "NETWT"
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .ReadOnly = True
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("AMOUNT")
                    .HeaderText = "AMOUNT"
                    .Width = 100
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .ReadOnly = True
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns("CATCODE")
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .ReadOnly = True
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With


                .Columns("CATCODE").Visible = False
                .Columns("RESULT").Visible = False
            Else
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
                With .Columns("CLOSING")
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .ReadOnly = True
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With

            End If

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
            For CNT As Integer = 9 To gridView.ColumnCount - 1
                .Columns(CNT).Visible = False
            Next

            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            '.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        End With

        pnlHeading.Visible = True
        Dim title As String = Nothing
        If rbtSummary.Checked = True Then
            title += " SUMMARY WISE"
        ElseIf rbtMonthWise.Checked = True Then
            title += " MONTH WISE"
        ElseIf rbtDateWise.Checked = True Then
            title += " DATE WISE"
        Else
            title += " TRANNO WISE"
        End If
        title += " STOCK REPORT"
        If rbtGrsWt.Checked = True Then
            title += "  BASED ON GROSS WEIGHT"
        ElseIf rbtNetWt.Checked = True Then
            title += "  BASED ON NET WEIGHT"
        Else
            title += " "
        End If
        'title += " ("
        'If rbtCarat.Checked = True Then
        '    title += "CARAT UNIT"
        'ElseIf rbtGram.Checked = True Then
        '    title += "GRAM UNIT"
        'Else
        '    title += "GENERAL UNIT"
        'End If
        'title += ")"
        title += " ASONDATE " + dtpFrom.Text
        title += IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " :" & chkCmbCostCentre.Text, "")
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
                .Cells(e.ColumnIndex + 1).Value = Format(totVal, "0.000")
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
    Function GridViewFormat() As Integer
        For Each dgvRow As DataGridViewRow In gridView.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case " "
                        .Cells("CATNAME").Style.BackColor = Nothing
                    Case "S"
                        .DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                    Case "T"
                        .Cells("CATNAME").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("CATNAME").Style.Font = reportHeadStyle.Font
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
                btnExport_Click(Me, New EventArgs)
            End If
            If UCase(e.KeyChar) = "P" Then
                btnPrint_Click(Me, New EventArgs)
            End If
        End If
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
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
        'If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
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

    Private Sub dtpFrom_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpFrom.LostFocus
        dtpTo.MinimumDate = dtpFrom.Value
    End Sub

    Private Sub frmCategoryStock_New_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
        strSql += vbCrLf + " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += vbCrLf + " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        btnNew_Click(Me, e)
        chkPcs.Checked = True
        rbtBoth.Checked = True
    End Sub

    Private Sub chkWithValue_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkWithValue.CheckedChanged
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

        If rbtSummary.Checked = False Then
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

            If rbtSummary.Checked = False Then
                MessageBox.Show("Update rate in summary mode only...", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
                btnSave.Enabled = False
                rbtSummary.Focus()
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
        Dim obj As New frmCategoryStock_New_Properties
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
        obj.p_rbtSummary = rbtSummary.Checked
        obj.p_rbtMonthWise = rbtMonthWise.Checked
        obj.p_rbtDateWise = rbtDateWise.Checked
        obj.p_rbtTranNoWise = rbtTranNoWise.Checked
        obj.p_chkWithValue = chkWithValue.Checked
        obj.p_chkWithOtherIssue = chkWithOtherIssue.Checked
        obj.p_chkWithStuddedStone = chkWithStuddedStone.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmCategoryStock_New_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmCategoryStock_New_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmCategoryStock_New_Properties))
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
        rbtSummary.Checked = obj.p_rbtSummary
        rbtMonthWise.Checked = obj.p_rbtMonthWise
        rbtDateWise.Checked = obj.p_rbtDateWise
        rbtTranNoWise.Checked = obj.p_rbtTranNoWise
        chkWithValue.Checked = obj.p_chkWithValue
        chkWithOtherIssue.Checked = obj.p_chkWithOtherIssue
        chkWithStuddedStone.Checked = obj.p_chkWithStuddedStone
    End Sub

End Class
Public Class frmCategoryStock_New_Properties
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