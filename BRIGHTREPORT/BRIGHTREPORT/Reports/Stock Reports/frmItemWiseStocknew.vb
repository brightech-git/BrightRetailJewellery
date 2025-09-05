
Imports System.Data.OleDb
Public Class frmItemWiseStocknew
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
        Dim DISABLE_ECISEDUTY As Boolean = IIf(GetAdmindbSoftValue("DISABLE_EXCISEDUTY", "N") = "Y", True, False)
        Dim _SepStud As Boolean = IIf(GetAdmindbSoftValue("LOOSESTUDSEPRATE", "N") = "Y", True, False)
        Dim CATSTOCK_NETWT_BASED As Boolean = IIf(GetAdmindbSoftValue("CATSTOCK_NETWT_BASED", "N") = "Y", True, False)
        Dim CATSTOCK_PCS_VALUE As Boolean = IIf(GetAdmindbSoftValue("CATSTOCK_PCS_VALUE", "N") = "Y", True, False)

        Function funcTranNoWise() As Integer
            '--TRANNO WISE CATSTOCK
            strSql = " IF (SELECT 1 FROM " & Temptable & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "CATSTOCKTRANNO')>0"
            strSql += vbCrLf + " DROP TABLE " & Temptable & "..TEMP" & systemId & "CATSTOCKTRANNO"
            strSql += vbCrLf + " SELECT SPACE(50) PARTICULAR,' 'METALNAME,CATNAME,SUM(OPENING)OPENING,"
            strSql += vbCrLf + " SUM(RECEIPT)RECEIPT,SUM(ISSUE)ISSUE"
            strSql += vbCrLf + " ,SUM(CLOSING)CLOSING,RATE,VALUE,CATCODE,RESULT"
            strSql += vbCrLf + " ,FRM"
            strSql += vbCrLf + " ,' 'YYEAR,' 'MMONTHID,' 'MMONTH,' 'DDATE"
            strSql += vbCrLf + " ,' 'TRANDATE,TRANNO, ' 'COLHEAD "
            strSql += vbCrLf + " INTO " & Temptable & "..TEMP" & systemId & "CATSTOCKTRANNO"
            strSql += vbCrLf + " FROM " & Temptable & "..TEMP" & systemId & "CATSTOCK"
            strSql += vbCrLf + " GROUP BY CATNAME,CATCODE,RESULT,RATE,VALUE,FRM,TRANNO"
            strSql += vbCrLf + " ORDER BY CATNAME,TRANNO"
            strSql += vbCrLf + " DECLARE @CATNAME VARCHAR(50)"
            strSql += vbCrLf + " DECLARE @TRANNO INT"
            strSql += vbCrLf + " DECLARE @OPENING NUMERIC(12,3)"
            strSql += vbCrLf + " DECLARE @ISSUE NUMERIC(12,3)"
            strSql += vbCrLf + " DECLARE @RECEIPT NUMERIC(12,3)"
            strSql += vbCrLf + " DECLARE @CLOSING NUMERIC(12,3)"
            strSql += vbCrLf + " DECLARE @PRETRANNO INT"
            strSql += vbCrLf + " DECLARE CURCAT CURSOR"
            strSql += vbCrLf + " FOR SELECT CATNAME FROM " & Temptable & "..TEMP" & systemId & "CATSTOCKTRANNO GROUP BY CATNAME"
            strSql += vbCrLf + " OPEN CURCAT"
            strSql += vbCrLf + " WHILE 1=1"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " 	FETCH NEXT FROM CURCAT INTO @CATNAME"
            strSql += vbCrLf + " 	SELECT @TRANNO = NULL"
            strSql += vbCrLf + " 	SELECT @PRETRANNO = NULL"
            strSql += vbCrLf + " 	IF @@FETCH_STATUS = -1 BREAK"
            strSql += vbCrLf + "         DECLARE CURMONTH CURSOR"
            strSql += vbCrLf + "         FOR SELECT TRANNO,OPENING,ISSUE,RECEIPT,CLOSING FROM " & Temptable & "..TEMP" & systemId & "CATSTOCKTRANNO WHERE CATNAME = @CATNAME"
            strSql += vbCrLf + "         OPEN CURMONTH"
            strSql += vbCrLf + "         WHILE 1=1"
            strSql += vbCrLf + " 	BEGIN"
            strSql += vbCrLf + " 		SELECT @TRANNO"
            strSql += vbCrLf + " 		UPDATE " & Temptable & "..TEMP" & systemId & "CATSTOCKTRANNO SET CLOSING = OPENING+RECEIPT-ISSUE WHERE TRANNO = @TRANNO AND CATNAME = @CATNAME"
            strSql += vbCrLf + "  		FETCH NEXT FROM CURMONTH INTO @TRANNO,@OPENING,@ISSUE,@RECEIPT,@CLOSING"
            strSql += vbCrLf + " 		IF @PRETRANNO <> @TRANNO"
            strSql += vbCrLf + "                    BEGIN"
            strSql += vbCrLf + "                    UPDATE " & Temptable & "..TEMP" & systemId & "CATSTOCKTRANNO SET OPENING = (SELECT CLOSING FROM " & Temptable & "..TEMP" & systemId & "CATSTOCKTRANNO WHERE TRANNO = @PRETRANNO AND CATNAME = @CATNAME)"
            strSql += vbCrLf + "                    WHERE TRANNO = @TRANNO AND CATNAME = @CATNAME"
            strSql += vbCrLf + "                    END"
            strSql += vbCrLf + " 		SELECT @PRETRANNO = @TRANNO"
            strSql += vbCrLf + " 		IF @@FETCH_STATUS = -1 BREAK"
            strSql += vbCrLf + " 	END"
            strSql += vbCrLf + "         CLOSE CURMONTH"
            strSql += vbCrLf + "         DEALLOCATE CURMONTH"
            strSql += vbCrLf + " END"
            strSql += vbCrLf + " CLOSE CURCAT"
            strSql += vbCrLf + " DEALLOCATE CURCAT"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End Function

        Function funcMonthWise() As Integer
            '--MONGH WISE CATSTOCK
            strSql = " IF (SELECT 1 FROM " & Temptable & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "CATSTOCKMONTH')>0"
            strSql += vbCrLf + " DROP TABLE " & Temptable & "..TEMP" & systemId & "CATSTOCKMONTH"
            ' Year Filtration
            strSql += vbCrLf + " DECLARE @frmDATE SMALLDATETIME "
            strSql += vbCrLf + " DECLARE @toDATE SMALLDATETIME "
            strSql += vbCrLf + " SELECT @frmDATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " SELECT @toDATE = '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            ' Year Filtration
            strSql += vbCrLf + " SELECT SPACE(50) PARTICULAR,METALNAME,CATNAME,SUM(OPENING)OPENING,SUM(RECEIPT)RECEIPT,SUM(ISSUE)ISSUE"
            strSql += vbCrLf + " ,SUM(CLOSING)CLOSING,RATE,VALUE,CATCODE,RESULT"
            strSql += vbCrLf + " ,FRM"
            strSql += vbCrLf + " ,YYEAR,MMONTHID,MMONTH,' 'DDATE"
            strSql += vbCrLf + " ,' 'TRANDATE,' 'TRANNO, ' 'COLHEAD "
            strSql += vbCrLf + " INTO " & Temptable & "..TEMP" & systemId & "CATSTOCKMONTH"
            strSql += vbCrLf + " FROM " & Temptable & "..TEMP" & systemId & "CATSTOCK"
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN @frmDATE AND @toDATE"
            strSql += vbCrLf + " GROUP BY METALNAME,CATNAME,CATCODE,RESULT,RATE,VALUE,FRM,YYEAR,MMONTHID,MMONTH"
            strSql += vbCrLf + " ORDER BY CATNAME,YYEAR,MMONTHID"
            strSql += vbCrLf + " DECLARE @CATNAME VARCHAR(50)"
            strSql += vbCrLf + " DECLARE @MMONTHID INT"
            strSql += vbCrLf + " DECLARE @OPENING NUMERIC(12,3)"
            strSql += vbCrLf + " DECLARE @ISSUE NUMERIC(12,3)"
            strSql += vbCrLf + " DECLARE @RECEIPT NUMERIC(12,3)"
            strSql += vbCrLf + " DECLARE @CLOSING NUMERIC(12,3)"
            strSql += vbCrLf + " DECLARE @PREMMONTHID INT"
            strSql += vbCrLf + " DECLARE CURCAT CURSOR"
            strSql += vbCrLf + " FOR SELECT CATNAME FROM " & Temptable & "..TEMP" & systemId & "CATSTOCKMONTH GROUP BY CATNAME"
            strSql += vbCrLf + " OPEN CURCAT"
            strSql += vbCrLf + " WHILE 1=1"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " 	FETCH NEXT FROM CURCAT INTO @CATNAME"
            strSql += vbCrLf + " 	SELECT @MMONTHID = NULL"
            strSql += vbCrLf + " 	SELECT @PREMMONTHID = NULL"
            strSql += vbCrLf + " 	IF @@FETCH_STATUS = -1 BREAK"
            strSql += vbCrLf + "         DECLARE CURMONTH CURSOR"
            strSql += vbCrLf + "         FOR SELECT MMONTHID,OPENING,ISSUE,RECEIPT,CLOSING FROM " & Temptable & "..TEMP" & systemId & "CATSTOCKMONTH WHERE CATNAME = @CATNAME"
            strSql += vbCrLf + "         OPEN CURMONTH"
            strSql += vbCrLf + "         WHILE 1=1"
            strSql += vbCrLf + " 	BEGIN"
            strSql += vbCrLf + " 		SELECT @MMONTHID"
            strSql += vbCrLf + " 		UPDATE " & Temptable & "..TEMP" & systemId & "CATSTOCKMONTH SET CLOSING = OPENING+RECEIPT-ISSUE WHERE MMONTHID = @MMONTHID AND CATNAME = @CATNAME"
            strSql += vbCrLf + "  		FETCH NEXT FROM CURMONTH INTO @MMONTHID,@OPENING,@ISSUE,@RECEIPT,@CLOSING"
            strSql += vbCrLf + " 		IF @PREMMONTHID <> @MMONTHID"
            strSql += vbCrLf + "                    BEGIN"
            strSql += vbCrLf + "                    UPDATE " & Temptable & "..TEMP" & systemId & "CATSTOCKMONTH SET OPENING = (SELECT CLOSING FROM " & Temptable & "..TEMP" & systemId & "CATSTOCKMONTH WHERE MMONTHID = @PREMMONTHID AND CATNAME = @CATNAME)"
            strSql += vbCrLf + "                    WHERE MMONTHID = @MMONTHID AND CATNAME = @CATNAME"
            strSql += vbCrLf + "                    END"
            strSql += vbCrLf + " 		SELECT @PREMMONTHID = @MMONTHID"
            strSql += vbCrLf + " 		IF @@FETCH_STATUS = -1 BREAK"
            strSql += vbCrLf + " 	END"
            strSql += vbCrLf + "         CLOSE CURMONTH"
            strSql += vbCrLf + "         DEALLOCATE CURMONTH"
            strSql += vbCrLf + " END"
            strSql += vbCrLf + " CLOSE CURCAT"
            strSql += vbCrLf + " DEALLOCATE CURCAT"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End Function

        Function funcSummaryWise() As Integer
        '-------SUMMARY WISE
        If chkitemname.Checked = True Or chksubitemname.Checked = True Then
            strSql = " IF (SELECT 1 FROM " & Temptable & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "CATSTOCKSUMMARY')>0"
            strSql += vbCrLf + " DROP TABLE " & Temptable & "..TEMP" & systemId & "CATSTOCKSUMMARY"
            strSql += vbCrLf + " SELECT SPACE(50) PARTICULAR,METALNAME,CATNAME,ITEMNAME"
            If chksubitemname.Checked Then
                strSql += vbCrLf + ",SUBITEMNAME"
            End If
            strSql += vbCrLf + " ,SUM(OPENING)OPENING,SUM(RECEIPT)RECEIPT,SUM(ISSUE)ISSUE"
            strSql += vbCrLf + " ,SUM(CLOSING)CLOSING,ITEMID"
            If chksubitemname.Checked Then
                strSql += vbCrLf + ",SUBITEMID"
            End If
            strSql += vbCrLf + " ,RATE,VALUE VALUE,CATCODE,RESULT,FRM"
            strSql += vbCrLf + " ,' ' YYEAR,' 'MMONTHID,' 'MMONTH,' 'DDATE"
            strSql += vbCrLf + " ,' 'TRANDATE,' 'TRANNO, ' 'COLHEAD "
            strSql += vbCrLf + " INTO " & Temptable & "..TEMP" & systemId & "CATSTOCKSUMMARY"
            strSql += vbCrLf + " FROM " & Temptable & "..TEMP" & systemId & "CATSTOCK"
            strSql += vbCrLf + " GROUP BY METALNAME,CATNAME,CATCODE,RESULT,RATE,VALUE,FRM,ITEMID"
            If chksubitemname.Checked Then
                strSql += vbCrLf + ",SUBITEMID"
            End If
            strSql += vbCrLf + ",ITEMNAME"
            If chksubitemname.Checked Then
                strSql += vbCrLf + ",SUBITEMNAME"
            End If
            'strSql += " HAVING SUM(OPENING) <> 0 OR  SUM(RECEIPT) <> 0 OR SUM(ISSUE) <> 0 OR SUM(CLOSING) <> 0 "
        Else
            strSql = " IF (SELECT 1 FROM " & Temptable & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "CATSTOCKSUMMARY')>0"
            strSql += vbCrLf + " DROP TABLE " & Temptable & "..TEMP" & systemId & "CATSTOCKSUMMARY"
            strSql += vbCrLf + " SELECT SPACE(50) PARTICULAR,METALNAME,CATNAME,SUM(OPENING)OPENING,"
            strSql += vbCrLf + " SUM(RECEIPT)RECEIPT,SUM(ISSUE)ISSUE"
            strSql += vbCrLf + " ,SUM(CLOSING)CLOSING,RATE,VALUE VALUE,CATCODE,RESULT"
            strSql += vbCrLf + " ,FRM"
            strSql += vbCrLf + " ,' ' YYEAR,' 'MMONTHID,' 'MMONTH,' 'DDATE"
            strSql += vbCrLf + " ,' 'TRANDATE,' 'TRANNO, ' 'COLHEAD "
            strSql += vbCrLf + " INTO " & Temptable & "..TEMP" & systemId & "CATSTOCKSUMMARY"
            strSql += vbCrLf + " FROM " & Temptable & "..TEMP" & systemId & "CATSTOCK"
            strSql += vbCrLf + " GROUP BY METALNAME,CATNAME,CATCODE,RESULT,RATE,VALUE,FRM"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End Function
        Function funcstkTypeWise() As Integer
            strSql = " IF (SELECT 1 FROM " & Temptable & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "CATSTOCKSUMMARY')>0"
            strSql += vbCrLf + " DROP TABLE " & Temptable & "..TEMP" & systemId & "CATSTOCKSUMMARY"
        strSql += vbCrLf + " SELECT SPACE(50) PARTICULAR,METALNAME,CATNAME,ITEMNAME,SUBITEMNAME,SUM(OPENING)OPENING,"
        strSql += vbCrLf + " SUM(RECEIPT)RECEIPT,SUM(ISSUE)ISSUE"
        strSql += vbCrLf + " ,SUM(CLOSING)CLOSING,ITEMID,SUBITEMID,RATE,VALUE VALUE,CATCODE,RESULT"
        strSql += vbCrLf + " ,FRM"
            strSql += vbCrLf + " ,' ' YYEAR,' 'MMONTHID,' 'MMONTH,' 'DDATE"
            strSql += vbCrLf + " ,' 'TRANDATE,' 'TRANNO, ' 'COLHEAD,STOCKTYPE "
            strSql += vbCrLf + " INTO " & Temptable & "..TEMP" & systemId & "CATSTOCKSUMMARY"
            strSql += vbCrLf + " FROM " & Temptable & "..TEMP" & systemId & "CATSTOCK"
        strSql += vbCrLf + " GROUP BY METALNAME,CATNAME,CATCODE,RESULT,RATE,VALUE,FRM,STOCKTYPE,ITEMID,SUBITEMID,ITEMNAME,SUBITEMNAME"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End Function

        Function funcDateWise() As Integer
            '--TRANDATE WISE CATSTOCK

            strSql = " IF (SELECT 1 FROM " & Temptable & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "CATSTOCKTRANDATE')>0"
            strSql += vbCrLf + " DROP TABLE " & Temptable & "..TEMP" & systemId & "CATSTOCKTRANDATE"
        strSql += vbCrLf + " SELECT SPACE(50) PARTICULAR,' 'METALNAME,CATNAME,ITEMNAME,SUBITEMNAME,SUM(OPENING)OPENING,"
        strSql += vbCrLf + " SUM(RECEIPT)RECEIPT,SUM(ISSUE)ISSUE"
        strSql += vbCrLf + " ,SUM(CLOSING)CLOSING,ITEMID,SUBITEMID,RATE,VALUE,CATCODE,RESULT"
        strSql += vbCrLf + " ,FRM"
            strSql += vbCrLf + " ,' ' YYEAR,' 'MMONTHID,' 'MMONTH,CONVERT(VARCHAR,TRANDATE,103)DDATE"
            strSql += vbCrLf + " ,TRANDATE,' 'TRANNO, ' 'COLHEAD "
            strSql += vbCrLf + " INTO " & Temptable & "..TEMP" & systemId & "CATSTOCKTRANDATE "
            strSql += vbCrLf + " FROM " & Temptable & "..TEMP" & systemId & "CATSTOCK"
        strSql += vbCrLf + " GROUP BY CATNAME,CATCODE,RESULT,RATE,VALUE,FRM,TRANDATE,ITEMID,SUBITEMID,ITEMNAME,SUBITEMNAME"
        strSql += vbCrLf + " ORDER BY CATNAME,TRANDATE"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = ""
            strSql += vbCrLf + " DECLARE @CATNAME VARCHAR(50)"
            strSql += vbCrLf + " DECLARE @TRANDATE SMALLDATETIME"
            strSql += vbCrLf + " DECLARE @OPENING NUMERIC(12,3)"
            strSql += vbCrLf + " DECLARE @ISSUE NUMERIC(12,3)"
            strSql += vbCrLf + " DECLARE @RECEIPT NUMERIC(12,3)"
            strSql += vbCrLf + " DECLARE @CLOSING NUMERIC(12,3)"
            strSql += vbCrLf + " DECLARE @PREDATE SMALLDATETIME"
            strSql += vbCrLf + " DECLARE CURCAT CURSOR"
            strSql += vbCrLf + " FOR SELECT CATNAME FROM " & Temptable & "..TEMP" & systemId & "CATSTOCKTRANDATE GROUP BY CATNAME"
            strSql += vbCrLf + " OPEN CURCAT"
            strSql += vbCrLf + " WHILE 1=1"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " 	FETCH NEXT FROM CURCAT INTO @CATNAME"
            strSql += vbCrLf + "  	SELECT @TRANDATE = NULL"
            strSql += vbCrLf + " 	SELECT @PREDATE = NULL"
            strSql += vbCrLf + " 	IF @@FETCH_STATUS = -1 BREAK"
            strSql += vbCrLf + "         DECLARE CURTRANDATE CURSOR"
            strSql += vbCrLf + "         FOR SELECT TRANDATE,OPENING,ISSUE,RECEIPT,CLOSING FROM " & Temptable & "..TEMP" & systemId & "CATSTOCKTRANDATE WHERE CATNAME = @CATNAME"
            strSql += vbCrLf + "         OPEN CURTRANDATE"
            strSql += vbCrLf + "         WHILE 1=1"
            strSql += vbCrLf + " 	BEGIN"
            strSql += vbCrLf + " 		UPDATE " & Temptable & "..TEMP" & systemId & "CATSTOCKTRANDATE SET CLOSING = OPENING+RECEIPT-ISSUE WHERE TRANDATE = @TRANDATE AND CATNAME = @CATNAME"
            strSql += vbCrLf + "  		FETCH NEXT FROM CURTRANDATE INTO @TRANDATE,@OPENING,@ISSUE,@RECEIPT,@CLOSING"
            strSql += vbCrLf + " 		IF @PREDATE <> @TRANDATE"
            strSql += vbCrLf + "                    BEGIN"
            strSql += vbCrLf + "                    UPDATE " & Temptable & "..TEMP" & systemId & "CATSTOCKTRANDATE SET OPENING = (SELECT CLOSING FROM " & Temptable & "..TEMP" & systemId & "CATSTOCKTRANDATE WHERE TRANDATE = @PREDATE AND CATNAME = @CATNAME)"
            strSql += vbCrLf + "                    WHERE TRANDATE = @TRANDATE AND CATNAME = @CATNAME"
            strSql += vbCrLf + "                    END"
            strSql += vbCrLf + " 		SELECT @PREDATE = @TRANDATE"
            strSql += vbCrLf + " 		IF @@FETCH_STATUS = -1 BREAK"
            strSql += vbCrLf + " 	END"
            strSql += vbCrLf + "         CLOSE CURTRANDATE"
            strSql += vbCrLf + "         DEALLOCATE CURTRANDATE"

            strSql += vbCrLf + " END"
            strSql += vbCrLf + " CLOSE CURCAT"
            strSql += vbCrLf + " DEALLOCATE CURCAT"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End Function

        Function funcMetalDateWise() As Integer
            '--TRANDATE WISE CATSTOCK

            strSql = "UPDATE " & Temptable & "..TEMP" & systemId & "CATSTOCK SET TRANDATE='" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "' WHERE TRANDATE<'" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = "UPDATE " & Temptable & "..TEMP" & systemId & "CATSTOCK SET FRM=''"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " IF (SELECT 1 FROM " & Temptable & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "CATSTOCKTRANDATE')>0"
            strSql += vbCrLf + " DROP TABLE " & Temptable & "..TEMP" & systemId & "CATSTOCKTRANDATE"
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(50),CONVERT(VARCHAR,TRANDATE,103)) PARTICULAR,METALNAME,ITEMID,SUBITEMID,ITEMNAME,SUBITEMNAME,SUM(OPENING)OPENING,"
        strSql += vbCrLf + " SUM(RECEIPT)RECEIPT,SUM(ISSUE)ISSUE"
            strSql += vbCrLf + " ,SUM(CLOSING)CLOSING,RATE,VALUE,RESULT"
            strSql += vbCrLf + " ,FRM"
            strSql += vbCrLf + " ,' ' YYEAR,' 'MMONTHID,' 'MMONTH,CONVERT(VARCHAR,TRANDATE,103)DDATE"
            strSql += vbCrLf + " ,TRANDATE,' 'TRANNO, ' 'COLHEAD "
            strSql += vbCrLf + " INTO " & Temptable & "..TEMP" & systemId & "CATSTOCKTRANDATE "
            strSql += vbCrLf + " FROM " & Temptable & "..TEMP" & systemId & "CATSTOCK"
        strSql += vbCrLf + " GROUP BY METALNAME,RESULT,RATE,VALUE,FRM,TRANDATE,ITEMID,SUBITEMID,ITEMNAME,SUBITEMNAME "
        strSql += vbCrLf + " ORDER BY METALNAME,TRANDATE"

            strSql += vbCrLf + " UPDATE " & Temptable & "..TEMP" & systemId & "CATSTOCKTRANDATE SET FRM=''"

            strSql += vbCrLf + " DECLARE @STRSQL AS VARCHAR(2000)"
            strSql += vbCrLf + " DECLARE @METALNAME VARCHAR(50)"
            strSql += vbCrLf + " DECLARE @TRANDATE SMALLDATETIME"
            strSql += vbCrLf + " DECLARE @OPENING NUMERIC(12,3)"
            strSql += vbCrLf + " DECLARE @ISSUE NUMERIC(12,3)"
            strSql += vbCrLf + " DECLARE @RECEIPT NUMERIC(12,3)"
            strSql += vbCrLf + " DECLARE @CLOSING NUMERIC(12,3)"
            strSql += vbCrLf + " DECLARE @PREDATE SMALLDATETIME"
            strSql += vbCrLf + " DECLARE CURCAT CURSOR"
            strSql += vbCrLf + " FOR SELECT METALNAME FROM " & Temptable & "..TEMP" & systemId & "CATSTOCKTRANDATE GROUP BY METALNAME"
            strSql += vbCrLf + " OPEN CURCAT"
            strSql += vbCrLf + " WHILE 1=1"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " 	FETCH NEXT FROM CURCAT INTO @METALNAME"
            strSql += vbCrLf + "  	SELECT @TRANDATE = NULL"
            strSql += vbCrLf + " 	SELECT @PREDATE = NULL"

            strSql += vbCrLf + " 	IF @@FETCH_STATUS = -1 BREAK"
            strSql += vbCrLf + "         DECLARE CURTRANDATE CURSOR"
            strSql += vbCrLf + "         FOR SELECT TRANDATE,OPENING,ISSUE,RECEIPT,CLOSING FROM " & Temptable & "..TEMP" & systemId & "CATSTOCKTRANDATE WHERE METALNAME = @METALNAME"
            strSql += vbCrLf + "         OPEN CURTRANDATE"
            strSql += vbCrLf + "         WHILE 1=1"
            strSql += vbCrLf + " 	BEGIN"

            strSql += vbCrLf + " 		UPDATE " & Temptable & "..TEMP" & systemId & "CATSTOCKTRANDATE SET CLOSING = OPENING+RECEIPT-ISSUE WHERE TRANDATE = @TRANDATE AND METALNAME = @METALNAME"
            strSql += vbCrLf + "  		FETCH NEXT FROM CURTRANDATE INTO @TRANDATE,@OPENING,@ISSUE,@RECEIPT,@CLOSING"
            strSql += vbCrLf + " 		IF @PREDATE <> @TRANDATE"
            strSql += vbCrLf + "                    BEGIN"
            strSql += vbCrLf + "                     	PRINT @METALNAME"
            strSql += vbCrLf + " 						PRINT @TRANDATE"
            strSql += vbCrLf + "                    UPDATE " & Temptable & "..TEMP" & systemId & "CATSTOCKTRANDATE SET OPENING = (SELECT SUM(CLOSING) FROM " & Temptable & "..TEMP" & systemId & "CATSTOCKTRANDATE WHERE TRANDATE = @PREDATE AND METALNAME = @METALNAME)"
            strSql += vbCrLf + "                    WHERE TRANDATE = @TRANDATE AND METALNAME = @METALNAME"
            strSql += vbCrLf + "                    END"
            strSql += vbCrLf + " 		SELECT @PREDATE = @TRANDATE"
            strSql += vbCrLf + " 		IF @@FETCH_STATUS = -1 BREAK"
            strSql += vbCrLf + " 	END"
            strSql += vbCrLf + "         CLOSE CURTRANDATE"
            strSql += vbCrLf + "         DEALLOCATE CURTRANDATE"
            strSql += vbCrLf + " END"
            strSql += vbCrLf + " CLOSE CURCAT"
            strSql += vbCrLf + " DEALLOCATE CURCAT"
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
        Dim Temptable As String = "TEMPTABLEDB"

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        gridView.DataSource = Nothing
        pnlHeading.Visible = False
        ftrIssue = funcIssueFiltration()
        ftrReceipt = funcReceiptFiltration()
        Dim CompanyFilt As String = funcCompanyFilt()
        Dim CompanyFilt1 As String
        Dim Gstflag As Boolean = funcGstView(dtpFrom.Value)




        strSql = " IF (SELECT 1 FROM " & Temptable & "..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "CATSTOCK')>0"
            strSql += vbCrLf + " DROP TABLE " & Temptable & "..TEMP" & systemId & "CATSTOCK"
            strSql += vbCrLf + " DECLARE @frmDATE SMALLDATETIME "
            strSql += vbCrLf + " DECLARE @toDATE SMALLDATETIME "

            strSql += vbCrLf + " SELECT @frmDATE = '" & dtpFrom.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " SELECT @toDATE = '" & dtpTo.Value.Date.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " SELECT"
            strSql += vbCrLf + " (SELECT METALNAME FROM " & cnAdminDb & "..METALMAST "
            strSql += vbCrLf + " WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..CATEGORY "
            strSql += vbCrLf + " WHERE CATCODE=X.CATCODE))AS METALNAME"
            strSql += vbCrLf + " ,CASE WHEN FRM = '1LOOSE' AND (SELECT METALID FROM " & cnAdminDb & "..CATEGORY "
            strSql += vbCrLf + " WHERE CATCODE = X.CATCODE) IN ('D','T') THEN (SELECT " & IIf(Gstflag, "CATNAME", "VATNAME") & " "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = X.CATCODE) + '[L]'"
            strSql += vbCrLf + " WHEN FRM = '2STUD' AND (SELECT METALID FROM " & cnAdminDb & "..CATEGORY "
            strSql += vbCrLf + " WHERE CATCODE = X.CATCODE) IN ('D','T') THEN (SELECT " & IIf(Gstflag, "CATNAME", "VATNAME") & " "
            strSql += vbCrLf + " FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = X.CATCODE)+ '[S]'"
            strSql += vbCrLf + " ELSE (SELECT " & IIf(Gstflag, "CATNAME", "VATNAME") & " FROM " & cnAdminDb & "..CATEGORY "
            strSql += vbCrLf + " WHERE CATCODE = X.CATCODE) END AS CATNAME"
        strSql += vbCrLf + ",(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
        strSql += vbCrLf + "WHERE ITEMID = X.ITEMID)  AS ITEMNAME "
        If chksubitemname.Checked = True Then
            strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = X.SUBITEMID)  AS SUBITEMNAME"
        End If
            If chkalloy.Checked = False Then
                If rbtGrsWt.Checked = True Then
                    strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP = 'REC_OPENING' THEN GRSWT WHEN SEP = 'ISS_OPENING' THEN -1*GRSWT END),0)AS OPENING"
                    strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP = 'ISSUE' THEN GRSWT END),0)AS ISSUE"
                    strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP = 'RECEIPT' THEN GRSWT END),0)AS RECEIPT"
                    strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP IN ('REC_OPENING','RECEIPT') THEN GRSWT ELSE -1*GRSWT END),0)AS CLOSING"
                ElseIf rbtPurewt.Checked = True Then
                    strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP = 'REC_OPENING' THEN PUREWT WHEN SEP = 'ISS_OPENING' THEN -1*PUREWT END),0)AS OPENING"
                    strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP = 'ISSUE' THEN PUREWT END),0)AS ISSUE"
                    strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP = 'RECEIPT' THEN PUREWT END),0)AS RECEIPT"
                    strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP IN ('REC_OPENING','RECEIPT') THEN PUREWT ELSE -1*PUREWT END),0)AS CLOSING"
                Else 'NETWT
                    strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP = 'REC_OPENING' THEN NETWT WHEN SEP = 'ISS_OPENING' THEN -1*NETWT END),0)AS OPENING"
                    strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP = 'ISSUE' THEN NETWT END),0)AS ISSUE"
                    strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP = 'RECEIPT' THEN NETWT END),0)AS RECEIPT"
                    strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP IN ('REC_OPENING','RECEIPT') THEN NETWT ELSE -1*NETWT END),0)AS CLOSING"
                End If
            Else
                If rbtGrsWt.Checked = True Then
                    strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP = 'REC_OPENING' THEN (GRSWT+ALLOY) WHEN SEP = 'ISS_OPENING' THEN -1*(GRSWT+ALLOY) END),0)AS OPENING"
                    strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP = 'ISSUE' THEN (GRSWT+ALLOY) END),0)AS ISSUE"
                    strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP = 'RECEIPT' THEN (GRSWT+ALLOY) END),0)AS RECEIPT"
                    strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP IN ('REC_OPENING','RECEIPT') THEN (GRSWT+ALLOY) ELSE -1*(GRSWT+ALLOY) END),0)AS CLOSING"
                ElseIf rbtPurewt.Checked = True Then
                    strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP = 'REC_OPENING' THEN (PUREWT+ALLOY) WHEN SEP = 'ISS_OPENING' THEN -1*(PUREWT+ALLOY) END),0)AS OPENING"
                    strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP = 'ISSUE' THEN (PUREWT+ALLOY) END),0)AS ISSUE"
                    strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP = 'RECEIPT' THEN (PUREWT+ALLOY) END),0)AS RECEIPT"
                    strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP IN ('REC_OPENING','RECEIPT') THEN (PUREWT+ALLOY) ELSE -1*(PUREWT+ALLOY) END),0)AS CLOSING"
                Else 'NETWT
                    strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP = 'REC_OPENING' THEN (NETWT+ALLOY) WHEN SEP = 'ISS_OPENING' THEN -1*(NETWT+ALLOY) END),0)AS OPENING"
                    strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP = 'ISSUE' THEN (NETWT+ALLOY) END),0)AS ISSUE"
                    strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP = 'RECEIPT' THEN (NETWT+ALLOY) END),0)AS RECEIPT"
                    strSql += vbCrLf + " ,ISNULL(SUM(CASE WHEN SEP IN ('REC_OPENING','RECEIPT') THEN (NETWT+ALLOY) ELSE -1*(NETWT+ALLOY) END),0)AS CLOSING"
                End If
            End If
            strSql += vbCrLf + " ,ISNULL(ITEMID,0)ITEMID"
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + ",ISNULL(SUBITEMID,0)SUBITEMID"
            End If
            strSql += vbCrLf + " ,' 'RATE,' 'VALUE"
            strSql += vbCrLf + " ,CATCODE"
            strSql += vbCrLf + " ,1 RESULT"
            strSql += vbCrLf + " ,' ' COLHEAD"
            strSql += vbCrLf + " ,FRM"
            strSql += vbCrLf + " ,YYEAR,MMONTHID,MMONTH,TRANDATE,TRANNO"
            If rbtStkType.Checked Then
                strSql += vbCrLf + " ,STOCKTYPE"
            End If
            strSql += vbCrLf + " INTO " & Temptable & "..TEMP" & systemId & "CATSTOCK"
            strSql += vbCrLf + " FROM"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " CASE WHEN ISNULL(M.STUDDED,'')='S' THEN '" & IIf(_SepStud = False, "", "2STUD") & "' ELSE '" & IIf(_SepStud = False, "", "1LOOSE") & "' END FRM,'ISS_OPENING'SEP,I.CATCODE"
        If chksubitemname.Checked = True Then
            strSql += vbCrLf + " ,I.ITEMID,I.SUBITEMID"
        Else
            strSql += vbCrLf + " ,I.ITEMID"
        End If
        If rbtGeneral.Checked = True Then
                strSql += vbCrLf + " ,SUM(I.GRSWT)GRSWT"
                strSql += vbCrLf + " ,SUM(I.ALLOY)ALLOY"
                strSql += vbCrLf + " ,SUM(NETWT)NETWT"
                strSql += vbCrLf + " ,SUM(PUREWT)PUREWT"
            ElseIf rbtCarat.Checked = True Then
                strSql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN I.GRSWT * 5 ELSE I.GRSWT END)GRSWT"
                strSql += vbCrLf + " ,0 ALLOY"
                strSql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN NETWT * 5 ELSE NETWT END)NETWT"
                strSql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN PUREWT * 5 ELSE PUREWT END)PUREWT"
            Else
                strSql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN I.GRSWT / 5 ELSE I.GRSWT END)GRSWT"
                strSql += vbCrLf + " ,0 ALLOY"
                strSql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN NETWT / 5 ELSE NETWT END)NETWT"
                strSql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN PUREWT / 5 ELSE PUREWT END)PUREWT"
            End If
            strSql += vbCrLf + " ,@FRMDATE TRANDATE,-1 TRANNO,TRANSTATUS"
            strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID"
            strSql += vbCrLf + " ,LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
            If rbtStkType.Checked Then
                strSql += vbCrLf + " ,CASE WHEN ISNULL(I.STKTYPE,'')='M' THEN 'MANUFACTURING' WHEN ISNULL(I.STKTYPE,'')='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STOCKTYPE"
            End If
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON I.CATCODE = C.CATCODE"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST M ON I.ITEMID = M.ITEMID"
            strSql += vbCrLf + " WHERE TRANDATE < @frmDATE AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + " AND ISNULL(I.TRANTYPE,'') NOT IN  ('IRC') "
            strSql += vbCrLf + " AND I.SNO NOT IN( SELECT ISSSNO FROM " & cnStockDb & "..ISSMETAL WHERE TRANDATE <  @FRMDATE AND I.TRANTYPE IN ('SA','OD'))"
            If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(I.TRANTYPE,'') NOT IN  ('AI') "
            ''strSql += vbcrlf + " AND ISNULL(TRANTYPE,'') NOT IN  ('OD','RD') "
            strSql += vbCrLf + ftrIssue
            CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND I.COMPANYID")
            strSql += vbCrLf + CompanyFilt1
            'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
            If rbtMfg.Checked Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='M'"
            ElseIf rbtTrading.Checked Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'') NOT IN('M','E')"
            ElseIf rbtExem.Checked Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='E'"
            End If
            strSql += vbCrLf + " GROUP BY I.CATCODE,TRANDATE,TRANNO,TRANSTATUS,M.STUDDED,I.ITEMID"
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + " ,I.SUBITEMID"
            End If
            If rbtStkType.Checked Then
                strSql += vbCrLf + " ,I.STKTYPE"
            End If
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '" & IIf(_SepStud = False, "", "1LOOSE") & "'FRM,'ISS_OPENING'SEP,I.CATCODE,0 AS ITEMID"
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + " ,0 AS SUBITEMID"
            End If
            If rbtGeneral.Checked = True Then
                strSql += vbCrLf + " ,SUM(GRSWT)GRSWT"
                strSql += vbCrLf + " ,0 AS ALLOY"
                strSql += vbCrLf + " ,SUM(GRSWT)NETWT"
                strSql += vbCrLf + " ,SUM(GRSWT)PUREWT"
            ElseIf rbtCarat.Checked = True Then
                strSql += vbCrLf + " ,SUM(CASE WHEN I.METALID = 'T' THEN I.GRSWT * 5 ELSE GRSWT END)GRSWT"
                strSql += vbCrLf + " ,0 ALLOY"
                strSql += vbCrLf + " ,SUM(CASE WHEN I.METALID= 'T' THEN GRSWT * 5 ELSE GRSWT END)NETWT"
                strSql += vbCrLf + " ,SUM(CASE WHEN I.METALID= 'T' THEN GRSWT * 5 ELSE GRSWT END)PUREWT"
            Else
                strSql += vbCrLf + " ,SUM(CASE WHEN I.METALID = 'D' THEN GRSWT / 5 ELSE GRSWT END)GRSWT"
                strSql += vbCrLf + " ,0 ALLOY"
                strSql += vbCrLf + " ,SUM(CASE WHEN I.METALID= 'D' THEN GRSWT/ 5 ELSE GRSWT END)NETWT"
                strSql += vbCrLf + " ,SUM(CASE WHEN I.METALID= 'D' THEN GRSWT/ 5 ELSE GRSWT END)PUREWT"
            End If
            strSql += vbCrLf + " ,@FRMDATE TRANDATE,-1 TRANNO,''TRANSTATUS"
            strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
            strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
            If rbtStkType.Checked Then
                strSql += vbCrLf + " ,(SELECT CASE WHEN ISNULL(STKTYPE,'')='M' THEN 'MANUFACTURING' WHEN ISNULL(STKTYPE,'')='E' THEN 'EXEMPTED' ELSE 'TRADING' END FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSSNO) AS STOCKTYPE"
            End If
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSMETAL AS I"
            strSql += vbCrLf + " WHERE TRANDATE < @frmDATE "
            strSql += vbCrLf + " AND ISNULL(TRANTYPE,'') IN  ('SA') "
            strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..ISSUE "
            strSql += vbCrLf + " WHERE ISNULL(CANCEL,'') = '' "
            strSql += vbCrLf + " AND ISNULL(I.TRANTYPE,'') NOT IN  ('IRC') "
            If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(I.TRANTYPE,'') NOT IN  ('AI') "
            strSql += vbCrLf + " AND TRANDATE < @frmDATE "
            If rbtMfg.Checked Then
                strSql += vbCrLf + " AND ISNULL(STKTYPE,'')='M'"
            ElseIf rbtTrading.Checked Then
                strSql += vbCrLf + " AND ISNULL(STKTYPE,'')NOT IN('M','E')"
            ElseIf rbtExem.Checked Then
                strSql += vbCrLf + " AND ISNULL(STKTYPE,'')='E'"
            End If
            strSql += vbCrLf + ftrIssue
            strSql += vbCrLf + CompanyFilt + "  )"

            If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(I.TRANTYPE,'') NOT IN  ('AI') "
            ''strSql += vbcrlf + " AND ISNULL(TRANTYPE,'') NOT IN  ('OD','RD') "
            strSql += vbCrLf + ftrIssue
            CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND I.COMPANYID")
            strSql += vbCrLf + CompanyFilt1
            'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
            strSql += vbCrLf + " GROUP BY I.CATCODE,TRANDATE,TRANNO"
            If rbtStkType.Checked Then
                strSql += vbCrLf + " ,I.ISSSNO"
            End If

            strSql += vbCrLf + " UNION ALL"

            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " CASE WHEN ISNULL(M.STUDDED,'')='S' THEN '" & IIf(_SepStud = False, "", "2STUD") & "' ELSE '" & IIf(_SepStud = False, "", "1LOOSE") & "' END FRM,'REC_OPENING'SEP,R.CATCODE,R.ITEMID"
        If chksubitemname.Checked = True Then
            strSql += vbCrLf + " ,R.SUBITEMID"
        End If
        ''strSql += vbCrLf + ",R.ITEMID,R.SUBITEMID"
        If rbtGeneral.Checked = True Then
                strSql += vbCrLf + " ,SUM(R.GRSWT)GRSWT"
                strSql += vbCrLf + " ,SUM(R.ALLOY)ALLOY"
                strSql += vbCrLf + " ,SUM(NETWT)NETWT"
                strSql += vbCrLf + " ,SUM(PUREWT)PUREWT"
            ElseIf rbtCarat.Checked = True Then
                strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN R.GRSWT * 5 ELSE R.GRSWT END)GRSWT"
                strSql += vbCrLf + " ,0 ALLOY"
                strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN NETWT * 5 ELSE NETWT END)NETWT"
                strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN PUREWT * 5 ELSE PUREWT END)PUREWT"
            Else
                strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN R.GRSWT / 5 ELSE R.GRSWT END)GRSWT"
                strSql += vbCrLf + " ,0 ALLOY"
                strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN NETWT / 5 ELSE NETWT END)NETWT"
                strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN PUREWT / 5 ELSE PUREWT END)PUREWT"
            End If
            strSql += vbCrLf + " ,@FRMDATE TRANDATE,-1 TRANNO,TRANSTATUS"
            strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
            strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
            If rbtStkType.Checked Then
                strSql += vbCrLf + " ,CASE WHEN ISNULL(R.STKTYPE,'')='M' THEN 'MANUFACTURING' WHEN ISNULL(R.STKTYPE,'')='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STOCKTYPE"
            End If
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS R"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON R.CATCODE = C.CATCODE"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST M ON M.ITEMID = R.ITEMID"
            strSql += vbCrLf + " WHERE TRANDATE < @frmDATE AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + " AND ISNULL(R.TRANTYPE,'') NOT IN  ('RRC') "
            If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(R.TRANTYPE,'') NOT IN  ('AR') "
            strSql += vbCrLf + ftrReceipt
            CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND R.COMPANYID")
            strSql += vbCrLf + CompanyFilt1
            If rbtMfg.Checked Then
                strSql += vbCrLf + " AND ISNULL(R.STKTYPE,'')='M'"
            ElseIf rbtTrading.Checked Then
                strSql += vbCrLf + " AND ISNULL(R.STKTYPE,'')NOT IN('M','E')"
            ElseIf rbtExem.Checked Then
                strSql += vbCrLf + " AND ISNULL(R.STKTYPE,'')='E'"
            End If
            'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
            strSql += vbCrLf + " GROUP BY R.CATCODE,TRANDATE,TRANNO,TRANSTATUS,M.STUDDED,R.ITEMID"
        If chksubitemname.Checked = True Then
            strSql += vbCrLf + " ,R.SUBITEMID"
        End If
        If rbtStkType.Checked Then
                strSql += vbCrLf + ",R.STKTYPE"
            End If


            '-------------  OPENING FROM OUTSTANDING RECEIPT ---------------
            If rbtMfg.Checked Or rbtAll.Checked Then
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT '" & IIf(_SepStud = False, "", "1LOOSE") & "'FRM,'REC_OPENING'SEP,CATCODE ,0 AS ITEMID"
                If chksubitemname.Checked = True Then
                    strSql += vbCrLf + " ,0 AS SUBITEMID  "
                End If
                strSql += vbCrLf + " ,GRSWT,ALLOY ,NETWT,PUREWT ,TRANDATE,TRANNO,TRANSTATUS,YYEAR,MMONTHID,MMONTH "
                If rbtStkType.Checked Then
                    strSql += vbCrLf + " ,'MANUFACTURING' AS STOCKTYPE"
                End If
                strSql += vbCrLf + " FROM ("
                strSql += vbCrLf + " SELECT  "
                strSql += vbCrLf + " ISNULL(I.CATCODE,'') CATCODE ,0 AS ITEMID"
                If chksubitemname.Checked = True Then
                    strSql += vbCrLf + " ,0 AS SUBITEMID  "
                End If
                If rbtGeneral.Checked = True Then
                    strSql += vbCrLf + " ,SUM(GRSWT)GRSWT"
                    strSql += vbCrLf + " ,0 AS ALLOY"
                    strSql += vbCrLf + " ,SUM(NETWT)NETWT"
                    strSql += vbCrLf + " ,SUM(PUREWT)PUREWT"
                Else
                    strSql += vbCrLf + " ,SUM(GRSWT * 5)GRSWT"
                    strSql += vbCrLf + " ,0 AS ALLOY"
                    strSql += vbCrLf + " ,SUM(NETWT * 5)NETWT"
                    strSql += vbCrLf + " ,SUM(PUREWT * 5)PUREWT"
                End If
                strSql += vbCrLf + " ,TRANDATE,TRANNO,TRANSTATUS"
                strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
                strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS I"
                strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpTo.MinimumDate.ToString("yyyy-MM-dd") & "' AND '" & dtpFrom.Value.AddDays(-1).ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''"
                strSql += vbCrLf + " AND RECPAY = 'R' AND BATCHNO IN (SELECT BATCHNO FROM " & cnAdminDb & "..ORMAST)"
                strSql += vbCrLf + Replace(ftrIssue, "I.METALID", "METALID")
                'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
                CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND I.COMPANYID")
                strSql += vbCrLf + CompanyFilt1
                strSql += vbCrLf + " GROUP BY I.CATCODE,TRANDATE,TRANNO,TRANSTATUS"
                strSql += vbCrLf + " ) X"
                '-------------  OPENING FROM OUTSTANDING ISSUE ---------------

                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT '" & IIf(_SepStud = False, "", "1LOOSE") & "'FRM,'ISS_OPENING'SEP,CATCODE,0 AS ITEMID"
                If chksubitemname.Checked = True Then
                    strSql += vbCrLf + " ,0 AS SUBITEMID"
                End If
                strSql += vbCrLf + " ,GRSWT ,ALLOY ,NETWT,PUREWT ,TRANDATE,TRANNO,TRANSTATUS,YYEAR,MMONTHID,MMONTH "
                If rbtStkType.Checked Then
                    strSql += vbCrLf + " ,'MANUFACTURING' AS STOCKTYPE"
                End If
                strSql += vbCrLf + " FROM ("
                strSql += vbCrLf + " SELECT  "
                strSql += vbCrLf + " ISNULL(I.CATCODE,'') CATCODE,0 AS ITEMID "
                If chksubitemname.Checked = True Then
                    strSql += vbCrLf + " ,0 AS SUBITEMID  "
                End If
                If rbtGeneral.Checked = True Then
                strSql += vbCrLf + " ,SUM(GRSWT)GRSWT"
                strSql += vbCrLf + " ,0 ALLOY"
                strSql += vbCrLf + " ,SUM(NETWT)NETWT"
                strSql += vbCrLf + " ,SUM(PUREWT)PUREWT"
            Else
                strSql += vbCrLf + " ,SUM(GRSWT * 5)GRSWT"
                strSql += vbCrLf + " ,0 AS ALLOY"
                strSql += vbCrLf + " ,SUM(NETWT * 5)NETWT"
                strSql += vbCrLf + " ,SUM(PUREWT * 5)PUREWT"
            End If
            strSql += vbCrLf + " ,TRANDATE,TRANNO,TRANSTATUS"
            strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
            strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS I"
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpTo.MinimumDate.ToString("yyyy-MM-dd") & "' AND '" & dtpFrom.Value.AddDays(-1).ToString("yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + " AND RECPAY = 'P' "
            strSql += vbCrLf + " AND SUBSTRING(RUNNO,6,1) <> 'R'"
            strSql += vbCrLf + Replace(ftrIssue, "I.METALID", "METALID")
            'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
            CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND I.COMPANYID")
            strSql += vbCrLf + CompanyFilt1
            strSql += vbCrLf + " GROUP BY CATCODE,TRANDATE,TRANNO,TRANSTATUS"
            strSql += vbCrLf + " ) X"
        End If

        ''*****************************OPENWEIGHT LOOSE
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " '" & IIf(_SepStud = False, "", "1LOOSE") & "'FRM,'REC_OPENING'SEP,R.CATCODE,R.ITEMID"
        If chksubitemname.Checked = True Then
            strSql += vbCrLf + " ,R.SUBITEMID"
        End If
        If rbtGeneral.Checked = True Then
            strSql += vbCrLf + " ,SUM(R.GRSWT)GRSWT"
            strSql += vbCrLf + " , 0 ALLOY"
            strSql += vbCrLf + " ,SUM(NETWT)NETWT"
            strSql += vbCrLf + " ,SUM(PUREWT)PUREWT"
        ElseIf rbtCarat.Checked = True Then
            strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN R.GRSWT * 5 ELSE R.GRSWT END)GRSWT"
            strSql += vbCrLf + " ,0 AS ALLOY"
            strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN NETWT * 5 ELSE NETWT END)NETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN PUREWT * 5 ELSE PUREWT END)PUREWT"
        Else
            strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN R.GRSWT / 5 ELSE R.GRSWT END)GRSWT"
            strSql += vbCrLf + " ,0 AS ALLOY"
            strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN NETWT / 5 ELSE NETWT END)NETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN PUREWT / 5 ELSE PUREWT END)PUREWT"
        End If
        strSql += vbCrLf + " ,@FRMDATE TRANDATE,-1 TRANNO,' 'TRANSTATUS"
        strSql += vbCrLf + " ,DATEPART(YEAR,@FRMDATE)YYEAR,DATEPART(MONTH,@FRMDATE)MMONTHID,"
        strSql += vbCrLf + " LEFT(DATENAME(MONTH,@FRMDATE),3)MMONTH"
        If rbtStkType.Checked Then
            strSql += vbCrLf + " ,CASE WHEN ISNULL(R.STKTYPE,'')='M' THEN 'MANUFACTURING' WHEN ISNULL(R.STKTYPE,'')='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STOCKTYPE"
        End If
        strSql += vbCrLf + " FROM " & cnStockDb & "..OPENWEIGHT AS R"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON R.CATCODE = C.CATCODE"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON ISNULL(R.ITEMID,'')=ISNULL(IM.ITEMID,'')"
        strSql += vbCrLf + " WHERE R.STOCKTYPE = 'C' AND ISNULL(IM.STUDDED,'')<>'S' AND ISNULL(R.TRANTYPE,'')='R'"
        'strSql += vbcrlf + " WHERE TRANDATE < @frmDATE AND ISNULL(CANCEL,'') = ''"
        'strSql += vbCrLf + Replace(ftrReceipt, "AND CATCODE", "AND R.CATCODE")
        If cmbMetal.Text <> "ALL" Then
            strSql += vbCrLf + " AND R.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY "
            strSql += vbCrLf + " WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST "
            strSql += vbCrLf + " WHERE METALNAME = '" & cmbMetal.Text & "'))"
        End If
        'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        strSql += vbCrLf + ftrReceipt.Replace("R.METALID", "METALID")

        CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND R.COMPANYID")
        strSql += vbCrLf + CompanyFilt1
        If rbtMfg.Checked Then
            strSql += vbCrLf + " AND ISNULL(R.STKTYPE,'')='M'"
        ElseIf rbtTrading.Checked Then
            strSql += vbCrLf + " AND ISNULL(R.STKTYPE,'')NOT IN('M','E')"
        ElseIf rbtExem.Checked Then
            strSql += vbCrLf + " AND ISNULL(R.STKTYPE,'')='E'"
        End If
        strSql += vbCrLf + " GROUP BY R.CATCODE,R.ITEMID"
        If chksubitemname.Checked = True Then
            strSql += vbCrLf + " ,R.SUBITEMID"
        End If

        If rbtStkType.Checked Then
            strSql += vbCrLf + " ,R.STKTYPE"
        End If
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " '" & IIf(_SepStud = False, "", "1LOOSE") & "'FRM,'ISS_OPENING'SEP,R.CATCODE,R.ITEMID"
        If chksubitemname.Checked = True Then
            strSql += vbCrLf + " ,R.SUBITEMID"
        End If
        If rbtGeneral.Checked = True Then
            strSql += vbCrLf + " ,SUM(R.GRSWT)GRSWT"
            strSql += vbCrLf + " ,0 ALLOY"
            strSql += vbCrLf + " ,SUM(NETWT)NETWT"
            strSql += vbCrLf + " ,SUM(PUREWT)PUREWT"
        ElseIf rbtCarat.Checked = True Then
            strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN R.GRSWT * 5 ELSE R.GRSWT END)GRSWT"
            strSql += vbCrLf + " ,0 AS ALLOY"
            strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN NETWT * 5 ELSE NETWT END)NETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN PUREWT * 5 ELSE PUREWT END)PUREWT"
        Else
            strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN R.GRSWT / 5 ELSE R.GRSWT END)GRSWT"
            strSql += vbCrLf + " ,0 AS ALLOY"
            strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN NETWT / 5 ELSE NETWT END)NETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN PUREWT / 5 ELSE PUREWT END)PUREWT"
        End If
        strSql += vbCrLf + " ,@FRMDATE TRANDATE,-1 TRANNO,' 'TRANSTATUS"
        strSql += vbCrLf + " ,DATEPART(YEAR,@FRMDATE)YYEAR,DATEPART(MONTH,@FRMDATE)MMONTHID,"
        strSql += vbCrLf + " LEFT(DATENAME(MONTH,@FRMDATE),3)MMONTH"
        If rbtStkType.Checked Then
            strSql += vbCrLf + " ,CASE WHEN ISNULL(R.STKTYPE,'')='M' THEN 'MANUFACTURING' WHEN ISNULL(R.STKTYPE,'')='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STOCKTYPE"
        End If
        strSql += vbCrLf + " FROM " & cnStockDb & "..OPENWEIGHT AS R"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON R.CATCODE = C.CATCODE"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON ISNULL(R.ITEMID,'')=ISNULL(IM.ITEMID,'')"
        strSql += vbCrLf + " WHERE R.STOCKTYPE = 'C' AND ISNULL(IM.STUDDED,'')<>'S' AND ISNULL(R.TRANTYPE,'')='I'"
        'strSql += vbcrlf + " WHERE TRANDATE < @frmDATE AND ISNULL(CANCEL,'') = ''"
        'strSql += vbCrLf + Replace(ftrReceipt, "AND CATCODE", "AND R.CATCODE")
        If cmbMetal.Text <> "ALL" Then
            strSql += vbCrLf + " AND R.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY "
            strSql += vbCrLf + " WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST "
            strSql += vbCrLf + " WHERE METALNAME = '" & cmbMetal.Text & "'))"
        End If
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        strSql += vbCrLf + ftrReceipt.Replace("R.METALID", "METALID")

        'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
        CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND R.COMPANYID")
        strSql += vbCrLf + CompanyFilt1
        If rbtMfg.Checked Then
            strSql += vbCrLf + " AND ISNULL(R.STKTYPE,'')='M'"
        ElseIf rbtTrading.Checked Then
            strSql += vbCrLf + " AND ISNULL(R.STKTYPE,'')NOT IN('M','E')"
        ElseIf rbtExem.Checked Then
            strSql += vbCrLf + " AND ISNULL(R.STKTYPE,'')='E'"
        End If
        strSql += vbCrLf + " GROUP BY R.CATCODE,R.ITEMID"
        If chksubitemname.Checked Then
            strSql += vbCrLf + " ,R.SUBITEMID"
        End If
        If rbtStkType.Checked Then
            strSql += vbCrLf + " ,R.STKTYPE"
        End If
        ''******************************

        strSql += vbCrLf + " UNION ALL"

        strSql += vbCrLf + " SELECT CASE WHEN FRM='' THEN '" & IIf(_SepStud = False, "", "1LOOSE") & "' ELSE FRM END AS FRM,'ISSUE'SEP,CATCODE,ITEMID"
        If chksubitemname.Checked = True Then
            strSql += vbCrLf + " ,SUBITEMID"
        End If
        strSql += vbCrLf + "  ,GRSWT,ALLOY ,NETWT,PUREWT,TRANDATE,TRANNO,TRANSTATUS,YYEAR,MMONTHID,MMONTH "

        If rbtStkType.Checked Then
            strSql += vbCrLf + " ,STOCKTYPE"
        End If
        strSql += vbCrLf + " FROM ("
        strSql += vbCrLf + " SELECT CASE WHEN ISNULL(M.STUDDED,'')='S' THEN '2STUD' ELSE '' END FRM,I.CATCODE,I.ITEMID"
        If chksubitemname.Checked = True Then
            strSql += vbCrLf + " ,I.SUBITEMID"
        End If
        If rbtGeneral.Checked = True Then
            strSql += vbCrLf + " ,SUM(I.GRSWT)GRSWT"
            strSql += vbCrLf + " ,SUM(I.ALLOY) ALLOY"
            strSql += vbCrLf + " ,SUM(NETWT)NETWT"
            strSql += vbCrLf + " ,SUM(PUREWT)PUREWT"
        ElseIf rbtCarat.Checked = True Then
            strSql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN I.GRSWT * 5 ELSE I.GRSWT END)GRSWT"
            strSql += vbCrLf + " ,0 AS ALLOY"
            strSql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN NETWT * 5 ELSE NETWT END)NETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN PUREWT * 5 ELSE PUREWT END)PUREWT"
        Else
            strSql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN I.GRSWT / 5 ELSE I.GRSWT END)GRSWT"
            strSql += vbCrLf + " ,0 AS ALLOY"
            strSql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN NETWT / 5 ELSE NETWT END)NETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN PUREWT / 5 ELSE PUREWT END)PUREWT"
        End If
        strSql += vbCrLf + " ,TRANDATE,TRANNO,TRANSTATUS"
        strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
        strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
        If rbtStkType.Checked Then
            strSql += vbCrLf + " ,CASE WHEN ISNULL(I.STKTYPE,'')='M' THEN 'MANUFACTURING' WHEN ISNULL(I.STKTYPE,'')='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STOCKTYPE"
        End If
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON I.CATCODE = C.CATCODE"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST M ON M.ITEMID = I.ITEMID"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN @FRMDATE AND @TODATE AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND I.SNO NOT IN( SELECT ISSSNO FROM " & cnStockDb & "..ISSMETAL WHERE TRANDATE BETWEEN  @FRMDATE AND @TODATE AND I.TRANTYPE IN ('SA','OD'))"
        strSql += vbCrLf + " AND ISNULL(I.TRANTYPE,'') NOT IN  ('IRC') "
        'strSql += vbCrLf + " AND ISNULL(I.TRANTYPE,'') NOT IN  ('OD') "
        If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(I.TRANTYPE,'') NOT IN  ('AI') "
        'strSql += vbcrlf + " AND ISNULL(TRANTYPE,'') NOT IN  ('OD','RD') "
        strSql += vbCrLf + ftrIssue
        'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
        CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND I.COMPANYID")
        strSql += vbCrLf + CompanyFilt1
        If rbtMfg.Checked Then
            strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='M'"
        ElseIf rbtTrading.Checked Then
            strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')NOT IN('M','E')"
        ElseIf rbtExem.Checked Then
            strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='E'"
        End If
        strSql += vbCrLf + " GROUP BY I.CATCODE,TRANDATE,TRANNO,TRANSTATUS,M.STUDDED,I.ITEMID"
        If chksubitemname.Checked = True Then
            strSql += vbCrLf + " ,I.SUBITEMID"
        End If
        If rbtStkType.Checked Then
            strSql += vbCrLf + " ,I.STKTYPE"
        End If
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT '' FRM,I.CATCODE,0 AS ITEMID"
        If chksubitemname.Checked = True Then
            strSql += vbCrLf + " ,0 AS SUBITEMID"
        End If
        If rbtGeneral.Checked = True Then
            strSql += vbCrLf + " ,SUM(I.GRSWT)GRSWT"
            strSql += vbCrLf + " ,0 AS ALLOY"
            strSql += vbCrLf + " ,SUM(I.GRSWT)NETWT"
            strSql += vbCrLf + " ,SUM(I.GRSWT)PUREWT"
        ElseIf rbtCarat.Checked = True Then
            strSql += vbCrLf + " ,SUM(CASE WHEN I.METALID= 'T' THEN I.GRSWT * 5 ELSE I.GRSWT END)GRSWT"
            strSql += vbCrLf + " ,0 AS ALLOY"
            strSql += vbCrLf + " ,SUM(CASE WHEN I.METALID = 'T' THEN I.GRSWT * 5 ELSE I.GRSWT END)NETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN I.METALID= 'T' THEN I.GRSWT * 5 ELSE I.GRSWT END)PUREWT"
        Else
            strSql += vbCrLf + " ,SUM(CASE WHEN I.METALID= 'D' THEN I.GRSWT / 5 ELSE I.GRSWT END)GRSWT"
            strSql += vbCrLf + " ,0 AS ALLOY"
            strSql += vbCrLf + " ,SUM(CASE WHEN I.METALID= 'D' THEN I.GRSWT / 5 ELSE I.GRSWT END)NETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN I.METALID= 'D' THEN I.GRSWT / 5 ELSE I.GRSWT END)PUREWT"
        End If
        strSql += vbCrLf + " ,I.TRANDATE,I.TRANNO,''TRANSTATUS"
        strSql += vbCrLf + " ,DATEPART(YEAR,I.TRANDATE)YYEAR,DATEPART(MONTH,I.TRANDATE)MMONTHID,"
        strSql += vbCrLf + " LEFT(DATENAME(MONTH,I.TRANDATE),3)MMONTH"
        If rbtStkType.Checked Then
            strSql += vbCrLf + " ,(SELECT CASE WHEN ISNULL(STKTYPE,'')='M' THEN 'MANUFACTURING' WHEN ISNULL(STKTYPE,'')='E' THEN 'EXEMPTED' ELSE 'TRADING' END FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSSNO) AS STOCKTYPE"
        End If
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSMETAL AS I "
        strSql += vbCrLf + " WHERE I.TRANDATE BETWEEN @frmDATE AND @toDATE "
        strSql += vbCrLf + " AND ISNULL(I.TRANTYPE,'') IN ('SA') "
        strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..ISSUE "
        strSql += vbCrLf + " WHERE ISNULL(CANCEL,'') = '' "
        strSql += vbCrLf + " AND ISNULL(TRANTYPE,'') NOT IN  ('IRC') "
        If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(TRANTYPE,'') NOT IN  ('AI') "
        strSql += vbCrLf + " AND (TRANDATE BETWEEN @frmDATE AND @toDATE )"
        strSql += vbCrLf + ftrIssue
        strSql += vbCrLf + CompanyFilt
        If rbtMfg.Checked Then
            strSql += vbCrLf + " AND ISNULL(STKTYPE,'')='M'"
        ElseIf rbtTrading.Checked Then
            strSql += vbCrLf + " AND ISNULL(STKTYPE,'')NOT IN('M','E')"
        ElseIf rbtExem.Checked Then
            strSql += vbCrLf + " AND ISNULL(STKTYPE,'')='E'"
        End If
        strSql += vbCrLf + "               )"
        strSql += vbCrLf + ftrIssue
        CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND I.COMPANYID")
        strSql += vbCrLf + CompanyFilt1
        strSql += vbCrLf + " GROUP BY I.CATCODE,I.TRANDATE,I.TRANNO"
        If rbtStkType.Checked Then
            strSql += vbCrLf + " ,I.ISSSNO"
        End If
        If rbtMfg.Checked Or rbtAll.Checked Then
            ''ORDER & REPAIR
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT  '' FRM,"
            strSql += vbCrLf + " ISNULL(I.CATCODE,'') CATCODE,0 AS ITEMID"
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + " ,0 AS SUBITEMID"
            End If
            If rbtGeneral.Checked = True Then
                strSql += vbCrLf + " ,SUM(GRSWT)GRSWT"
                strSql += vbCrLf + " ,0 AS ALLOY"
                strSql += vbCrLf + " ,SUM(NETWT)NETWT"
                strSql += vbCrLf + " ,SUM(PUREWT)PUREWT"
            Else
                strSql += vbCrLf + " ,SUM(GRSWT * 5)GRSWT"
                strSql += vbCrLf + " ,0 AS ALLOY"
                strSql += vbCrLf + " ,SUM(NETWT * 5)NETWT"
                strSql += vbCrLf + " ,SUM(PUREWT * 5)PUREWT"
            End If
            strSql += vbCrLf + " ,TRANDATE,TRANNO,TRANSTATUS"
            strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
            strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
            If rbtStkType.Checked Then
                strSql += vbCrLf + " ,'MANUFACTURING' AS STOCKTYPE"
            End If
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS I"
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN @frmDATE AND @toDATE AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + " AND RECPAY = 'P' "
            strSql += vbCrLf + " AND SUBSTRING(RUNNO,6,1) <> 'R'"
            strSql += vbCrLf + Replace(ftrIssue, "I.METALID", "METALID")
            'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
            CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND I.COMPANYID")
            strSql += vbCrLf + CompanyFilt1
            strSql += vbCrLf + " GROUP BY CATCODE,TRANDATE,TRANNO,TRANSTATUS"
        End If
        strSql += vbCrLf + " ) X"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT CASE WHEN FRM='' THEN '" & IIf(_SepStud = False, "", "1LOOSE") & "' ELSE FRM END AS FRM,'RECEIPT'SEP,CATCODE,ITEMID "
        If chksubitemname.Checked = True Then
            strSql += vbCrLf + " ,SUBITEMID"
        End If
        strSql += vbCrLf + " ,GRSWT,ALLOY ,NETWT,PUREWT ,TRANDATE,TRANNO,TRANSTATUS ,YYEAR,MMONTHID,MMONTH "
        If rbtStkType.Checked Then
            strSql += vbCrLf + " ,STOCKTYPE"
        End If
        strSql += vbCrLf + " FROM ("
        strSql += vbCrLf + " SELECT CASE WHEN ISNULL(M.STUDDED,'')='S' THEN '2STUD' ELSE '' END FRM,R.CATCODE,R.ITEMID"
        If chksubitemname.Checked = True Then
            strSql += vbCrLf + " ,R.SUBITEMID"
        End If
        If rbtGeneral.Checked = True Then
            strSql += vbCrLf + " ,SUM(R.GRSWT)GRSWT"
            strSql += vbCrLf + " ,SUM(R.ALLOY)ALLOY"
            strSql += vbCrLf + " ,SUM(NETWT)NETWT"
            strSql += vbCrLf + " ,SUM(PUREWT)PUREWT"
        ElseIf rbtCarat.Checked = True Then
            strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN R.GRSWT * 5 ELSE R.GRSWT END)GRSWT"
            strSql += vbCrLf + " ,0 AS ALLOY"
            strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN NETWT * 5 ELSE NETWT END)NETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN PUREWT * 5 ELSE PUREWT END)PUREWT"
        Else
            strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN R.GRSWT / 5 ELSE R.GRSWT END)GRSWT"
            strSql += vbCrLf + " ,0 AS ALLOY"
            strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN NETWT / 5 ELSE NETWT END)NETWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN PUREWT / 5 ELSE PUREWT END)PUREWT"
        End If
        strSql += vbCrLf + " ,TRANDATE,TRANNO,TRANSTATUS"
        strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
        strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
        If rbtStkType.Checked Then
            strSql += vbCrLf + " ,CASE WHEN ISNULL(R.STKTYPE,'')='M' THEN 'MANUFACTURING' WHEN ISNULL(R.STKTYPE,'')='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STOCKTYPE"
        End If
        strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS R"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON R.CATCODE = C.CATCODE"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST M ON R.ITEMID = M.ITEMID"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN @frmDATE AND @toDATE AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + " AND ISNULL(R.TRANTYPE,'') NOT IN  ('RRC') "
        If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(R.TRANTYPE,'') NOT IN  ('AR') "
        ''strSql += vbcrlf + " AND ISNULL(TRANTYPE,'') <> 'RPU' "
        strSql += vbCrLf + Replace(ftrReceipt, "AND CATCODE", "AND R.CATCODE")
        'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
        CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND R.COMPANYID")
        strSql += vbCrLf + CompanyFilt1
        If rbtMfg.Checked Then
            strSql += vbCrLf + " AND ISNULL(R.STKTYPE,'')='M'"
        ElseIf rbtTrading.Checked Then
            strSql += vbCrLf + " AND ISNULL(R.STKTYPE,'')NOT IN('M','E')"
        ElseIf rbtExem.Checked Then
            strSql += vbCrLf + " AND ISNULL(R.STKTYPE,'')='E'"
        End If
        strSql += vbCrLf + " GROUP BY R.CATCODE,TRANDATE,TRANNO,TRANSTATUS,M.STUDDED,R.ITEMID"
        If chksubitemname.Checked = True Then
            strSql += vbCrLf + " ,R.SUBITEMID"
        End If
        If rbtStkType.Checked Then
            strSql += vbCrLf + " ,R.STKTYPE"
        End If
        If rbtMfg.Checked Or rbtAll.Checked Then
            ''ORDER & REPAIR
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT  '' FRM,"
            strSql += vbCrLf + " ISNULL(I.CATCODE,'') CATCODE ,0 AS ITEMID"
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + " ,0 AS SUBITEMID"
            End If
            If rbtGeneral.Checked = True Then
                strSql += vbCrLf + " ,SUM(GRSWT)GRSWT"
                strSql += vbCrLf + " ,0 AS ALLOY"
                strSql += vbCrLf + " ,SUM(NETWT)NETWT"
                strSql += vbCrLf + " ,SUM(PUREWT)PUREWT"
            Else
                strSql += vbCrLf + " ,SUM(GRSWT * 5)GRSWT"
                strSql += vbCrLf + " ,0 AS ALLOY"
                strSql += vbCrLf + " ,SUM(NETWT * 5)NETWT"
                strSql += vbCrLf + " ,SUM(PUREWT * 5)PUREWT"
            End If
            strSql += vbCrLf + " ,TRANDATE,TRANNO,TRANSTATUS"
            strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
            strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
            If rbtStkType.Checked Then
                strSql += vbCrLf + " ,'MANUFACTURING' AS STOCKTYPE"
            End If
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING AS I"
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN @frmDATE AND @toDATE AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + " AND RECPAY = 'R' AND BATCHNO IN (SELECT BATCHNO FROM " & cnAdminDb & "..ORMAST)"
            strSql += vbCrLf + " AND NOT EXISTS(SELECT 1 FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO =I.BATCHNO AND TRANTYPE='AD')"
            strSql += vbCrLf + Replace(ftrIssue, "I.METALID", "METALID")
            'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
            CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND I.COMPANYID")
            strSql += vbCrLf + CompanyFilt1
            strSql += vbCrLf + " GROUP BY I.CATCODE,TRANDATE,TRANNO,TRANSTATUS"
        End If
        strSql += vbCrLf + " ) X"
        If chkWithStuddedStone.Checked = True Then
            strSql += vbCrLf + " UNION ALL"

            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '" & IIf(_SepStud = False, "", "2STUD") & "'FRM,'ISS_OPENING'SEP,CATCODE,0 AS ITEMID"
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + " ,0 AS SUBITEMID"
            End If
            If rbtGeneral.Checked = True Then
                strSql += vbCrLf + " ,SUM(STNWT)GRSWT"
                strSql += vbCrLf + " ,0 AS ALLOY"
                strSql += vbCrLf + " ,SUM(STNWT)NETWT"
                strSql += vbCrLf + " ,SUM(STNWT)PUREWT"
            ElseIf rbtCarat.Checked = True Then
                strSql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'G' THEN STNWT * 5 ELSE STNWT END)GRSWT"
                strSql += vbCrLf + " ,0 AS ALLOY"
                strSql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'G' THEN STNWT * 5 ELSE STNWT END)NETWT"
                strSql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'G' THEN STNWT * 5 ELSE STNWT END)PUREWT"
            Else
                strSql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'C' THEN STNWT / 5 ELSE STNWT END)GRSWT"
                strSql += vbCrLf + " ,0 AS ALLOY"
                strSql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'C' THEN STNWT / 5 ELSE STNWT END)NETWT"
                strSql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'C' THEN STNWT / 5 ELSE STNWT END)PUREWT"
            End If
            strSql += vbCrLf + " ,@FRMDATE TRANDATE,-1 TRANNO,TRANSTATUS"
            strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
            strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
            If rbtStkType.Checked Then
                strSql += vbCrLf + " ,(SELECT CASE WHEN ISNULL(STKTYPE,'')='M' THEN 'MANUFACTURING' WHEN ISNULL(STKTYPE,'')='E' THEN 'EXEMPTED' ELSE 'TRADING' END FROM " & cnStockDb & "..ISSUE WHERE SNO=I.ISSSNO)AS STOCKTYPE"
            End If
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
            If rbtMfg.Checked Then
                strSql += vbCrLf + " AND ISNULL(STKTYPE,'')='M'"
            ElseIf rbtTrading.Checked Then
                strSql += vbCrLf + " AND ISNULL(STKTYPE,'')NOT IN('M','E')"
            ElseIf rbtExem.Checked Then
                strSql += vbCrLf + " AND ISNULL(STKTYPE,'')='E'"
            End If
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " GROUP BY CATCODE,TRANDATE,TRANNO,TRANSTATUS"
            If rbtStkType.Checked Then
                strSql += vbCrLf + " ,I.ISSSNO"
            End If
            strSql += vbCrLf + " UNION ALL"

            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '" & IIf(_SepStud = False, "", "2STUD") & "'FRM,'REC_OPENING'SEP,CATCODE,0 AS ITEMID"
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + " ,0 AS SUBITEMID"
            End If
            If rbtGeneral.Checked = True Then
                strSql += vbCrLf + " ,SUM(STNWT)GRSWT"
                strSql += vbCrLf + " ,0 AS ALLOY"
                strSql += vbCrLf + " ,SUM(STNWT)NETWT"
                strSql += vbCrLf + " ,SUM(STNWT)PUREWT"
            ElseIf rbtCarat.Checked = True Then
                strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'G' THEN STNWT * 5 ELSE STNWT END)GRSWT"
                strSql += vbCrLf + " ,0 AS ALLOY"
                strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'G' THEN STNWT * 5 ELSE STNWT END)NETWT"
                strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'G' THEN STNWT * 5 ELSE STNWT END)PUREWT"
            Else
                strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'C' THEN STNWT / 5 ELSE STNWT END)GRSWT"
                strSql += vbCrLf + " ,0 AS ALLOY"
                strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'C' THEN STNWT / 5 ELSE STNWT END)NETWT"
                strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'C' THEN STNWT / 5 ELSE STNWT END)PUREWT"
            End If
            strSql += vbCrLf + " ,@FRMDATE TRANDATE,-1 TRANNO,TRANSTATUS"
            strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
            strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
            If rbtStkType.Checked Then
                strSql += vbCrLf + " ,(SELECT CASE WHEN ISNULL(STKTYPE,'')='M' THEN 'MANUFACTURING' WHEN ISNULL(STKTYPE,'')='E' THEN 'EXEMPTED' ELSE 'TRADING' END FROM " & cnStockDb & "..RECEIPT WHERE SNO=R.ISSSNO)AS STOCKTYPE"
            End If
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE AS R"
            strSql += vbCrLf + " WHERE TRANDATE < @frmDATE"
            strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT AS R"
            strSql += vbCrLf + " WHERE ISNULL(CANCEL,'') = '' "
            strSql += vbCrLf + " AND ISNULL(R.TRANTYPE,'') NOT IN  ('RRC') "
            If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(R.TRANTYPE,'') NOT IN  ('AR') "
            strSql += vbCrLf + " AND (TRANDATE < @frmDATE )"
            strSql += vbCrLf + Replace(ftrReceipt, "AND CATCODE", "AND R.CATCODE")
            'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
            CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND R.COMPANYID")
            strSql += vbCrLf + CompanyFilt1
            If rbtMfg.Checked Then
                strSql += vbCrLf + " AND ISNULL(STKTYPE,'')='M'"
            ElseIf rbtTrading.Checked Then
                strSql += vbCrLf + " AND ISNULL(STKTYPE,'')NOT IN('M','E')"
            ElseIf rbtExem.Checked Then
                strSql += vbCrLf + " AND ISNULL(STKTYPE,'')='E'"
            End If
            strSql += vbCrLf + "               )"
            strSql += vbCrLf + " GROUP BY CATCODE,TRANDATE,TRANNO,TRANSTATUS"
            If rbtStkType.Checked Then
                strSql += vbCrLf + " ,R.ISSSNO"
            End If
            ''*****************************OPENWEIGHT STUD
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '" & IIf(_SepStud = False, "", "2STUD") & "'FRM,'REC_OPENING'SEP,R.CATCODE,0 AS ITEMID"
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + " ,0 AS SUBITEMID"
            End If
            If rbtGeneral.Checked = True Then
                strSql += vbCrLf + " ,SUM(R.GRSWT)GRSWT"
                strSql += vbCrLf + " ,0 AS ALLOY"
                strSql += vbCrLf + " ,SUM(NETWT)NETWT"
                strSql += vbCrLf + " ,SUM(PUREWT)PUREWT"
            ElseIf rbtCarat.Checked = True Then
                strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN R.GRSWT * 5 ELSE R.GRSWT END)GRSWT"
                strSql += vbCrLf + " ,0 AS ALLOY"
                strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN NETWT * 5 ELSE NETWT END)NETWT"
                strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'G' AND C.METALID IN ('T','D') THEN PUREWT * 5 ELSE PUREWT END)PUREWT"
            Else
                strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN R.GRSWT / 5 ELSE R.GRSWT END)GRSWT"
                strSql += vbCrLf + " ,0 AS ALLOY"
                strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN NETWT / 5 ELSE NETWT END)NETWT"
                strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN PUREWT / 5 ELSE PUREWT END)PUREWT"
            End If
            strSql += vbCrLf + " ,@FRMDATE TRANDATE,-1 TRANNO,' 'TRANSTATUS"
            strSql += vbCrLf + " ,DATEPART(YEAR,@FRMDATE)YYEAR,DATEPART(MONTH,@FRMDATE)MMONTHID,"
            strSql += vbCrLf + " LEFT(DATENAME(MONTH,@FRMDATE),3)MMONTH"
            If rbtStkType.Checked Then
                strSql += vbCrLf + " ,CASE WHEN ISNULL(R.STKTYPE,'')='M' THEN 'MANUFACTURING' WHEN ISNULL(R.STKTYPE,'')='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STOCKTYPE"
            End If
            strSql += vbCrLf + " FROM " & cnStockDb & "..OPENWEIGHT AS R"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON R.CATCODE = C.CATCODE"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IM ON R.ITEMID=IM.ITEMID"
            strSql += vbCrLf + " WHERE R.STOCKTYPE = 'C' AND IM.STUDDED='S'"
            'strSql += vbcrlf + " WHERE TRANDATE < @frmDATE AND ISNULL(CANCEL,'') = ''"
            strSql += vbCrLf + Replace(ftrReceipt, "AND CATCODE", "AND R.CATCODE").Replace("R.METALID", "METALID")
            'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
            CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND R.COMPANYID")
            If rbtMfg.Checked Then
                strSql += vbCrLf + " AND ISNULL(STKTYPE,'')='M'"
            ElseIf rbtTrading.Checked Then
                strSql += vbCrLf + " AND ISNULL(STKTYPE,'')NOT IN('M','E')"
            ElseIf rbtExem.Checked Then
                strSql += vbCrLf + " AND ISNULL(STKTYPE,'')='E'"
            End If
            strSql += vbCrLf + CompanyFilt1
            strSql += vbCrLf + " GROUP BY R.CATCODE"
            If rbtStkType.Checked Then
                strSql += vbCrLf + " ,R.STKTYPE"
            End If
            strSql += vbCrLf + " UNION ALL"

            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '" & IIf(_SepStud = False, "", "2STUD") & "'FRM,'ISSUE'SEP,I.CATCODE,0 AS ITEMID"
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + " ,0 AS SUBITEMID"
            End If
            If rbtGeneral.Checked = True Then
                strSql += vbCrLf + " ,SUM(I.STNWT)GRSWT"
                strSql += vbCrLf + " ,0 AS ALLOY"
                strSql += vbCrLf + " ,SUM(I.STNWT)NETWT"
                strSql += vbCrLf + " ,SUM(I.STNWT)PUREWT"
            ElseIf rbtCarat.Checked = True Then
                strSql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'G' THEN I.STNWT * 5 ELSE I.STNWT END)GRSWT"
                strSql += vbCrLf + " ,0 AS ALLOY"
                strSql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'G' THEN I.STNWT * 5 ELSE I.STNWT END)NETWT"
                strSql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'G' THEN I.STNWT * 5 ELSE I.STNWT END)PUREWT"
            Else
                strSql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'C' THEN I.STNWT / 5 ELSE I.STNWT END)GRSWT"
                strSql += vbCrLf + " ,0 AS ALLOY"
                strSql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'C' THEN I.STNWT / 5 ELSE I.STNWT END)NETWT"
                strSql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'C' THEN I.STNWT / 5 ELSE I.STNWT END)PUREWT"
            End If
            strSql += vbCrLf + " ,TRANDATE,TRANNO,TRANSTATUS"
            strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
            strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
            If rbtStkType.Checked Then
                strSql += vbCrLf + " ,(SELECT CASE WHEN ISNULL(STKTYPE,'')='M' THEN 'MANUFACTURING' WHEN ISNULL(STKTYPE,'')='E' THEN 'EXEMPTED' ELSE 'TRADING' END FROM " & cnStockDb & "..RECEIPT WHERE SNO=I.ISSSNO)AS STOCKTYPE"
            End If
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE AS I"
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN @frmDATE AND @toDATE "
            strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..ISSUE "
            strSql += vbCrLf + " WHERE ISNULL(CANCEL,'') = '' "
            strSql += vbCrLf + " AND ISNULL(I.TRANTYPE,'') NOT IN  ('IRC') "
            If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(I.TRANTYPE,'') NOT IN  ('AI') "
            strSql += vbCrLf + " AND (TRANDATE BETWEEN @frmDATE AND @toDATE )"
            strSql += vbCrLf + Replace(ftrIssue, "I.METALID", "METALID")
            strSql += vbCrLf + CompanyFilt
            If rbtMfg.Checked Then
                strSql += vbCrLf + " AND ISNULL(STKTYPE,'')='M'"
            ElseIf rbtTrading.Checked Then
                strSql += vbCrLf + " AND ISNULL(STKTYPE,'')NOT IN('M','E')"
            ElseIf rbtExem.Checked Then
                strSql += vbCrLf + " AND ISNULL(STKTYPE,'')='E'"
            End If
            'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
            strSql += vbCrLf + "               )"
            strSql += vbCrLf + " GROUP BY I.CATCODE,TRANDATE,TRANNO,TRANSTATUS"
            If rbtStkType.Checked Then
                strSql += vbCrLf + " ,I.ISSSNO"
            End If
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '" & IIf(_SepStud = False, "", "2STUD") & "'FRM,'RECEIPT'SEP,CATCODE,0 AS ITEMID"
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + " ,0 AS SUBITEMID"
            End If
            If rbtGeneral.Checked = True Then
                strSql += vbCrLf + " ,SUM(R.STNWT)GRSWT"
                strSql += vbCrLf + " ,0 AS ALLOY"
                strSql += vbCrLf + " ,SUM(R.STNWT)NETWT"
                strSql += vbCrLf + " ,SUM(R.STNWT)PUREWT"
            ElseIf rbtCarat.Checked = True Then
                strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'G' THEN R.STNWT * 5 ELSE R.STNWT END)GRSWT"
                strSql += vbCrLf + " ,0 AS ALLOY"
                strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'G' THEN R.STNWT * 5 ELSE R.STNWT END)NETWT"
                strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'G' THEN R.STNWT * 5 ELSE R.STNWT END)PUREWT"
            Else
                strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'C' THEN R.STNWT / 5 ELSE R.STNWT END)GRSWT"
                strSql += vbCrLf + " ,0 AS ALLOY"
                strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'C' THEN R.STNWT / 5 ELSE R.STNWT END)NETWT"
                strSql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'C' THEN R.STNWT / 5 ELSE R.STNWT END)PUREWT"
            End If
            strSql += vbCrLf + " ,TRANDATE,TRANNO,TRANSTATUS"
            strSql += vbCrLf + " ,DATEPART(YEAR,TRANDATE)YYEAR,DATEPART(MONTH,TRANDATE)MMONTHID,"
            strSql += vbCrLf + " LEFT(DATENAME(MONTH,TRANDATE),3)MMONTH"
            If rbtStkType.Checked Then
                strSql += vbCrLf + " ,(SELECT CASE WHEN ISNULL(STKTYPE,'')='M' THEN 'MANUFACTURING' WHEN ISNULL(STKTYPE,'')='E' THEN 'EXEMPTED' ELSE 'TRADING' END FROM " & cnStockDb & "..RECEIPT WHERE SNO=R.ISSSNO)AS STOCKTYPE"
            End If
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTSTONE AS R"
            strSql += vbCrLf + " WHERE TRANDATE BETWEEN @frmDATE AND @toDATE "
            strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..RECEIPT AS R"
            strSql += vbCrLf + " WHERE ISNULL(CANCEL,'') = '' "
            strSql += vbCrLf + " AND ISNULL(R.TRANTYPE,'') NOT IN  ('RRC') "
            If chkWithApproval.Checked = False Then strSql += vbCrLf + " AND ISNULL(R.TRANTYPE,'') NOT IN  ('AR') "
            strSql += vbCrLf + " AND (TRANDATE BETWEEN @frmDATE AND @toDATE )"
            strSql += vbCrLf + Replace(ftrReceipt, "AND CATCODE", "AND R.CATCODE")
            CompanyFilt1 = Replace(CompanyFilt, "AND COMPANYID", "AND R.COMPANYID")
            strSql += vbCrLf + CompanyFilt1
            If rbtMfg.Checked Then
                strSql += vbCrLf + " AND ISNULL(STKTYPE,'')='M'"
            ElseIf rbtTrading.Checked Then
                strSql += vbCrLf + " AND ISNULL(STKTYPE,'')NOT IN('M','E')"
            ElseIf rbtExem.Checked Then
                strSql += vbCrLf + " AND ISNULL(STKTYPE,'')='E'"
            End If
            'If Not cnCentStock Then strSql += vbcrlf + " AND COMPANYID = '" & GetStockCompId() & "'"
            strSql += vbCrLf + "               )"
            strSql += vbCrLf + " GROUP BY CATCODE,TRANDATE,TRANNO,TRANSTATUS"
            If rbtStkType.Checked Then
                strSql += vbCrLf + " ,R.ISSSNO"
            End If
        End If
        strSql += vbCrLf + " )X"

        strSql += vbCrLf + " GROUP BY CATCODE,FRM,YYEAR,MMONTHID,MMONTH,TRANDATE,TRANNO,ITEMID"
        If chksubitemname.Checked = True Then
            strSql += vbCrLf + " ,SUBITEMID"
        End If
        If rbtStkType.Checked Then
            strSql += vbCrLf + " ,STOCKTYPE"
        End If

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "delete from " & Temptable & "..TEMP" & systemId & "CATSTOCK where opening = 0 and issue = 0 and receipt = 0 and closing = 0 and value = 0"
        If CATSTOCK_PCS_VALUE Then
            strSql += vbCrLf + " AND ISNULL(METALNAME,'')='' AND ISNULL(CATNAME,'')=''"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT 1 FROM " & Temptable & "..SYSOBJECTS WHERE XTYPE = 'U' "
        strSql += vbCrLf + " AND NAME = 'TEMP" & systemId & "CATSUMM')>0 "
        strSql += vbCrLf + " DROP TABLE " & Temptable & "..TEMP" & systemId & "CATSUMM"
        strSql += vbCrLf + " CREATE TABLE " & Temptable & "..TEMP" & systemId & "CATSUMM("
        strSql += vbCrLf + " METALNAME VARCHAR(50),"
        strSql += vbCrLf + " CATNAME VARCHAR(250),"
        strSql += vbCrLf + "ITEMNAME VARCHAR(250),"
        If chksubitemname.Checked = True Then
            strSql += vbCrLf + " SUBITEMNAME VARCHAR(250),"
        End If
        strSql += vbCrLf + " OPENING NUMERIC(12,3),"
        strSql += vbCrLf + " RECEIPT NUMERIC(12,3),"
        strSql += vbCrLf + " ISSUE NUMERIC(12,3),"
        strSql += vbCrLf + " CLOSING NUMERIC(12,3),"
        strSql += vbCrLf + " ITEMID VARCHAR(15),"
        strSql += vbCrLf + "SUBITEMID VARCHAR(15),"
        strSql += vbCrLf + " RATE VARCHAR(30),"
        strSql += vbCrLf + " VALUE VARCHAR(30),"
        strSql += vbCrLf + " CATCODE VARCHAR(15),"
        strSql += vbCrLf + " RESULT INT,"
        strSql += vbCrLf + " FRM VARCHAR(30),"
        strSql += vbCrLf + " YYEAR INT,"
        strSql += vbCrLf + " MMONTHID INT,"
        strSql += vbCrLf + " MMONTH	NVARCHAR(9),"
        'strSql += vbCrLf + " DDATE VARCHAR(12),"
        strSql += vbCrLf + " DDATE SMALLDATETIME,"
        strSql += vbCrLf + " TRANDATE VARCHAR(12),"
        strSql += vbCrLf + " TRANNO VARCHAR(12),"
        strSql += vbCrLf + " COLHEAD VARCHAR(1),"
        If rbtStkType.Checked Then
            strSql += vbCrLf + " STOCKTYPE VARCHAR(25),"
        End If
        strSql += vbCrLf + " SNO INT IDENTITY)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If rbtSummary.Checked = True Then
            funcSummaryWise()
        ElseIf rbtDateWise.Checked = True Then
            funcDateWise()
        ElseIf rbtMonthWise.Checked = True Then
            funcMonthWise()
        ElseIf rbtStkType.Checked = True Then
            funcstkTypeWise()
        ElseIf rbtMetalDateWise.Checked = True Then
            funcMetalDateWise()
        Else
            funcTranNoWise()
        End If
        If rbtStkType.Checked = True Then
            strSql = " IF (SELECT 1 FROM " & Temptable & "..SYSOBJECTS WHERE XTYPE = 'U' AND "
            strSql += vbCrLf + " NAME = 'TEMP" & systemId & "CTRSTOCKTEMP')>0 "
            strSql += vbCrLf + " DROP TABLE " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP"
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(250),PARTICULAR)AS PARTICULAR,METALNAME,CATNAME,ITEMNAME"
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + ",SUBITEMNAME"
            End If
            strSql += vbCrLf + ",OPENING,RECEIPT,ISSUE,CLOSING,ITEMID"
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + ",SUBITEMID"
            End If
            strSql += vbCrLf + ",RATE,VALUE,CATCODE,"
            strSql += vbCrLf + " RESULT,FRM,YYEAR,MMONTHID,MMONTH, DDATE, TRANDATE, TRANNO, COLHEAD,STOCKTYPE "
            strSql += vbCrLf + " INTO " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP FROM ("
            strSql += vbCrLf + " SELECT * FROM " & Temptable & "..TEMP" & systemId & "CATSTOCKSUMMARY"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT"
            strSql += vbCrLf + " 'SUB TOTAL'PARTICULAR,METALNAME,'SUB TOTAL'CATNAME,ITEMNAME"
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + ",SUBITEMNAME"
            End If
            strSql += vbCrLf + " ,SUM(OPENING)OPENING,SUM(RECEIPT)RECEIPT,SUM(ISSUE)ISSUE,SUM(CLOSING)CLOSING,ITEMID"
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + " ,SUBITEMID"
            End If
            strSql += vbCrLf + " ,' ' RATE,' ' VALUE,' 'CATCODE,2 RESULT,' 'FRM"
            strSql += vbCrLf + " ,' 'YYEAR,' 'MMONTHID,' 'MMONTH,' 'DDATE,' 'TRANDATE,' 'TRANNO, 'S'COLHEAD"
            strSql += vbCrLf + " ,STOCKTYPE"
            strSql += vbCrLf + " FROM " & Temptable & "..TEMP" & systemId & "CATSTOCK AS T"
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + " GROUP BY METALNAME,STOCKTYPE,ITEMID,SUBITEMID,ITEMNAME,SUBITEMNAME"
            Else
                strSql += vbCrLf + " GROUP BY METALNAME,STOCKTYPE,ITEMID,ITEMNAME"
            End If
            strSql += vbCrLf + " ) X"
            strSql += vbCrLf + " ORDER BY METALNAME,RESULT"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " IF (SELECT COUNT(*) FROM " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP)>0 "
            strSql += vbCrLf + " BEGIN "
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + " INSERT INTO " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP(METALNAME,CATNAME,ITEMNAME,SUBITEMNAME,STOCKTYPE,RESULT,ITEMID,SUBITEMID,RATE,"
            Else
                strSql += vbCrLf + " INSERT INTO " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP(METALNAME,CATNAME,ITEMNAME,STOCKTYPE,RESULT,ITEMID,RATE,"
            End If
            strSql += vbCrLf + " VALUE,FRM,YYEAR,MMONTHID,MMONTH,DDATE,TRANDATE,TRANNO,COLHEAD,PARTICULAR )"
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + " SELECT DISTINCT METALNAME,METALNAME CATNAME,'','',STOCKTYPE, 0,0,0,' ',' ',' ','','','','','','','T','' "
            Else
                strSql += vbCrLf + " SELECT DISTINCT METALNAME,METALNAME CATNAME,'',STOCKTYPE, 0,0,' ',' ',' ','','','','','','','T','' "

            End If

            strSql += vbCrLf + " FROM " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP WHERE RESULT =1"
            strSql += vbCrLf + " UNION ALL"
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + " SELECT DISTINCT NULL,NULL,0,0,'','',STOCKTYPE, -1,0,0,' ',' ',' ','','','','','','','T','' "
            Else
                strSql += vbCrLf + " SELECT DISTINCT NULL,NULL,0,0,'',STOCKTYPE, -1,0,' ',' ',' ','','','','','','','T','' "
            End If

            strSql += vbCrLf + " FROM " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP WHERE RESULT =1"
            strSql += vbCrLf + " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = " IF (SELECT COUNT(*) FROM " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP)>0 "
            strSql += vbCrLf + " BEGIN "
            strSql += vbCrLf + " UPDATE " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP SET PARTICULAR = CASE WHEN RESULT = 0 THEN METALNAME"
            strSql += vbCrLf + " WHEN RESULT = 2 THEN 'SUB TOTAL' "
            strSql += vbCrLf + " WHEN RESULT = -1 THEN SPACE(50)+ STOCKTYPE "
            strSql += vbCrLf + " ELSE '  '+ CATNAME END"
            strSql += vbCrLf + " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = " IF (SELECT COUNT(*) FROM " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP)>0 "
            strSql += vbCrLf + " BEGIN "
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + " INSERT INTO " & Temptable & "..TEMP" & systemId & "CATSUMM(METALNAME,CATNAME,ITEMNAME,SUBITEMNAME,OPENING,RECEIPT,ISSUE,"
                strSql += vbCrLf + " Closing,ITEMID,SUBITEMID, Rate, VALUE, CATCODE, RESULT, FRM, YYEAR, MMONTHID, MMONTH, DDATE, "
                strSql += vbCrLf + " TRANDATE, TRANNO, COLHEAD,STOCKTYPE) "
                strSql += vbCrLf + " SELECT METALNAME,PARTICULAR,ITEMNAME,SUBITEMNAME,OPENING,RECEIPT,ISSUE,"
                strSql += vbCrLf + " Closing,ITEMID,SUBITEMID, Rate, VALUE, CATCODE, RESULT, FRM, YYEAR, MMONTHID, MMONTH, CONVERT(SMALLDATETIME,DDATE,103)DDATE, "
            Else
                strSql += vbCrLf + " INSERT INTO " & Temptable & "..TEMP" & systemId & "CATSUMM(METALNAME,CATNAME,ITEMNAME,OPENING,RECEIPT,ISSUE,"
                strSql += vbCrLf + " Closing,ITEMID, Rate, VALUE, CATCODE, RESULT, FRM, YYEAR, MMONTHID, MMONTH, DDATE, "
                strSql += vbCrLf + " TRANDATE, TRANNO, COLHEAD,STOCKTYPE) "
                strSql += vbCrLf + " SELECT METALNAME,PARTICULAR,ITEMNAME,OPENING,RECEIPT,ISSUE,"
                strSql += vbCrLf + " Closing,ITEMID, Rate, VALUE, CATCODE, RESULT, FRM, YYEAR, MMONTHID, MMONTH, CONVERT(SMALLDATETIME,DDATE,103)DDATE, "
            End If
            strSql += vbCrLf + " CONVERT(VARCHAR,TRANDATE,102)TRANDATE, TRANNO, COLHEAD,STOCKTYPE"
            strSql += vbCrLf + " FROM " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP"
            strSql += vbCrLf + " ORDER BY STOCKTYPE,METALNAME,RESULT"
            strSql += vbCrLf + " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        ElseIf rbtMetalDateWise.Checked Then
            strSql = " IF (SELECT 1 FROM " & Temptable & "..SYSOBJECTS WHERE XTYPE = 'U' AND "
            strSql += vbCrLf + " NAME = 'TEMP" & systemId & "CTRSTOCKTEMP')>0 "
            strSql += vbCrLf + " DROP TABLE " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP"
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + " SELECT PARTICULAR,METALNAME,ITEMNAME,SUBITEMNAME,OPENING,RECEIPT,ISSUE,CLOSING,,ITEMID,SUBITEMID,RATE,VALUE,"
            Else
                strSql += vbCrLf + " SELECT PARTICULAR,METALNAME,ITEMNAME,OPENING,RECEIPT,ISSUE,CLOSING,,ITEMID,RATE,VALUE,"
            End If
            strSql += vbCrLf + " RESULT,FRM,YYEAR,MMONTHID,MMONTH, DDATE, TRANDATE, TRANNO, COLHEAD "
            strSql += vbCrLf + " INTO " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP FROM ("
            strSql += vbCrLf + " SELECT * FROM " & Temptable & "..TEMP" & systemId & "CATSTOCKTRANDATE"
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT"
            If rbtSummary.Checked = True Then
                If chksubitemname.Checked = True Then
                    strSql += vbCrLf + " METALNAME + ' TOTAL' PARTICULAR,METALNAME,'SUB TOTAL'CATNAME,ITEMNAME,SUBITEMNAME"
                Else
                    strSql += vbCrLf + " METALNAME + ' TOTAL' PARTICULAR,METALNAME,'SUB TOTAL'CATNAME,ITEMNAME"
                End If

            Else
                If chksubitemname.Checked = True Then
                    strSql += vbCrLf + " METALNAME + ' TOTAL' PARTICULAR,METALNAME,ITEMNAME,SUBITEMNAME"
                Else
                    strSql += vbCrLf + " METALNAME + ' TOTAL' PARTICULAR,METALNAME,ITEMNAME"
                End If

            End If
            strSql += vbCrLf + " ,SUM(0.000)OPENING,SUM(0.000)RECEIPT,SUM(0.000)ISSUE,SUM(0.000)CLOSING"
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + " ,ITEMID,SUBITEMID,' ' RATE,' ' VALUE,2 RESULT"
            Else
                strSql += vbCrLf + " ,ITEMID,' ' RATE,' ' VALUE,2 RESULT"
            End If

            strSql += vbCrLf + " ,' 'FRM"
            strSql += vbCrLf + " ,' 'YYEAR,' 'MMONTHID,' 'MMONTH,' 'DDATE,' 'TRANDATE,' 'TRANNO, 'S'COLHEAD"
            strSql += vbCrLf + " FROM " & Temptable & "..TEMP" & systemId & "CATSTOCK AS T"
            If rbtSummary.Checked = True Then
                strSql += vbCrLf + " GROUP BY METALNAME"
            Else
                strSql += vbCrLf + " GROUP BY METALNAME"
            End If
            strSql += vbCrLf + " ) X"
            strSql += vbCrLf + " ORDER BY METALNAME,RESULT"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()



            strSql = " UPDATE A"
            strSql += vbCrLf + " SET OPENING=(SELECT TOP 1 OPENING FROM " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP WHERE RESULT=1 AND METALNAME=A.METALNAME ORDER BY TRANDATE)"
            strSql += vbCrLf + " FROM " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP AS A WHERE RESULT=2"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " UPDATE A"
            strSql += vbCrLf + " SET CLOSING=(SELECT TOP 1 CLOSING FROM " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP WHERE RESULT=1 AND METALNAME=A.METALNAME ORDER BY TRANDATE DESC)"
            strSql += vbCrLf + " FROM " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP AS A WHERE RESULT=2"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " UPDATE A"
            strSql += vbCrLf + " SET RECEIPT=(SELECT SUM(RECEIPT) FROM " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP WHERE RESULT=1 AND METALNAME=A.METALNAME)"
            strSql += vbCrLf + " FROM " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP AS A WHERE RESULT=2"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " UPDATE A"
            strSql += vbCrLf + " SET ISSUE=(SELECT SUM(ISSUE) FROM " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP WHERE RESULT=1 AND METALNAME=A.METALNAME)"
            strSql += vbCrLf + " FROM " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP AS A WHERE RESULT=2"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()


            strSql = " IF (SELECT COUNT(*) FROM " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP)>0 "
            strSql += vbCrLf + " BEGIN "
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + " INSERT INTO " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP(PARTICULAR,METALNAME,ITEMNAME,SUBITEMNAME,RESULT,ITEMID,SUBITEMID,RATE,"
            Else
                strSql += vbCrLf + " INSERT INTO " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP(PARTICULAR,METALNAME,ITEMNAME,RESULT,ITEMID,RATE,"
            End If
            strSql += vbCrLf + " VALUE,FRM,YYEAR,MMONTHID,MMONTH,DDATE,TRANDATE,TRANNO,COLHEAD )"
            If rbtSummary.Checked = True Then
                If chksubitemname.Checked = True Then
                    strSql += vbCrLf + " SELECT DISTINCT METALNAME,METALNAME,'',0,0,' ',' ',' ','','','','','','','T' "
                Else
                    strSql += vbCrLf + " SELECT DISTINCT METALNAME,METALNAME,'','',0,0,0,' ',' ',' ','','','','','','','T' "
                End If
            ElseIf rbtDateWise.Checked = True Then
                strSql += vbCrLf + " SELECT DISTINCT METALNAME,METALNAME,'','',0,0,' ',' ',' ','','','','','','','T' "
            ElseIf rbtMonthWise.Checked = True Then
                strSql += vbCrLf + " SELECT DISTINCT METALNAME,METALNAME,'','',0,0,0,' ',' ',' ','','','','','','','T' "
            Else
                strSql += vbCrLf + " SELECT DISTINCT METALNAME,METALNAME,'','',0,0,0,' ',' ',' ','','','','','','','T' "
            End If
            strSql += vbCrLf + " FROM " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP WHERE RESULT =1"
            strSql += vbCrLf + " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            'strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "CTRSTOCKTEMP)>0 "
            'strSql += vbCrLf + " BEGIN "
            'strSql += vbCrLf + " UPDATE TEMP" & systemId & "CTRSTOCKTEMP SET PARTICULAR = CASE WHEN RESULT = 0 THEN "
            'strSql += vbCrLf + " METALNAME WHEN RESULT = 2 THEN 'SUB TOTAL' ELSE ' '+ DDATE END"
            'strSql += vbCrLf + " END "
            'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            'cmd.ExecuteNonQuery()

            strSql = " IF (SELECT COUNT(*) FROM " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP)>0 "
            strSql += vbCrLf + " BEGIN "
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + " INSERT INTO " & Temptable & "..TEMP" & systemId & "CATSUMM(METALNAME,ITEMNAME,SUBITEMNAME,OPENING,RECEIPT,ISSUE,"
                strSql += vbCrLf + " Closing,ITEMID,SUBITEMID, Rate, VALUE,RESULT, FRM, YYEAR, MMONTHID, MMONTH, DDATE, "
                strSql += vbCrLf + " TRANDATE, TRANNO, COLHEAD) "
                strSql += vbCrLf + " SELECT METALNAME,ITEMNAME,SUBITEMNAME,OPENING,RECEIPT,ISSUE,"
                strSql += vbCrLf + " Closing,ITEMID,SUBITEMID, Rate, VALUE, RESULT, FRM, YYEAR, MMONTHID, MMONTH, CONVERT(SMALLDATETIME,DDATE,103)DDATE, "
            Else
                strSql += vbCrLf + " INSERT INTO " & Temptable & "..TEMP" & systemId & "CATSUMM(METALNAME,ITEMNAME,OPENING,RECEIPT,ISSUE,"
                strSql += vbCrLf + " Closing,ITEMID, Rate, VALUE,RESULT, FRM, YYEAR, MMONTHID, MMONTH, DDATE, "
                strSql += vbCrLf + " TRANDATE, TRANNO, COLHEAD) "
                strSql += vbCrLf + " SELECT METALNAME,ITEMNAME,OPENING,RECEIPT,ISSUE,"
                strSql += vbCrLf + " Closing,ITEMID, Rate, VALUE, RESULT, FRM, YYEAR, MMONTHID, MMONTH, CONVERT(SMALLDATETIME,DDATE,103)DDATE, "
            End If
            strSql += vbCrLf + " CONVERT(VARCHAR,TRANDATE,102)TRANDATE, TRANNO, COLHEAD"
            strSql += vbCrLf + " FROM " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP"
            If rbtSummary.Checked = True Then
                strSql += vbCrLf + " ORDER BY METALNAME,RESULT"
            Else
                strSql += vbCrLf + " ORDER BY RESULT"
            End If
            strSql += vbCrLf + " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000

            cmd.ExecuteNonQuery()

            If chkWithValue.Checked = True Then funcRateValUpdation()
        Else
            strSql = " IF (SELECT 1 FROM " & Temptable & "..SYSOBJECTS WHERE XTYPE = 'U' AND "
            strSql += vbCrLf + " NAME = 'TEMP" & systemId & "CTRSTOCKTEMP')>0 "
            strSql += vbCrLf + " DROP TABLE " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP"
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + " SELECT PARTICULAR,METALNAME,CATNAME,ITEMNAME,SUBITEMNAME,OPENING,RECEIPT,ISSUE,CLOSING,ITEMID,SUBITEMID,RATE,VALUE,CATCODE,"
            ElseIf chkitemname.Checked = False And chksubitemname.Checked = False Then
                strSql += vbCrLf + " SELECT PARTICULAR,METALNAME,CATNAME,OPENING,RECEIPT,ISSUE,CLOSING,RATE,VALUE,CATCODE,"
            Else
                strSql += vbCrLf + " SELECT PARTICULAR,METALNAME,CATNAME,ITEMNAME,OPENING,RECEIPT,ISSUE,CLOSING,ITEMID,RATE,VALUE,CATCODE,"
            End If
            strSql += vbCrLf + " RESULT,FRM,YYEAR,MMONTHID,MMONTH, DDATE, TRANDATE, TRANNO, COLHEAD "
            strSql += vbCrLf + " INTO " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP FROM ("
            If rbtSummary.Checked = True Then
                strSql += vbCrLf + " SELECT * FROM " & Temptable & "..TEMP" & systemId & "CATSTOCKSUMMARY"
            ElseIf rbtDateWise.Checked = True Then
                strSql += vbCrLf + " SELECT * FROM " & Temptable & "..TEMP" & systemId & "CATSTOCKTRANDATE"
            ElseIf rbtMonthWise.Checked = True Then
                strSql += vbCrLf + " SELECT * FROM " & Temptable & "..TEMP" & systemId & "CATSTOCKMONTH"
            Else
                strSql += vbCrLf + " SELECT * FROM " & Temptable & "..TEMP" & systemId & "CATSTOCKTRANNO"
            End If
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT"
            If chksubitemname.Checked = True Then
                If rbtSummary.Checked = True Then
                    strSql += vbCrLf + " 'SUB TOTAL'PARTICULAR,''METALNAME,CATNAME,'' ITEMNAME,''SUBITEMNAME"
                Else
                    strSql += vbCrLf + " 'SUB TOTAL'PARTICULAR,''METALNAME,CATNAME,'' ITEMNAME,''SUBITEMNAME"
                End If
                strSql += vbCrLf + " ,SUM(OPENING)OPENING,SUM(RECEIPT)RECEIPT,SUM(ISSUE)ISSUE,SUM(CLOSING)CLOSING"
                strSql += vbCrLf + " ,0 ITEMID,0 SUBITEMID,' ' RATE,' ' VALUE,' 'CATCODE,2 RESULT"
            ElseIf chksubitemname.Checked = False And chkitemname.Checked = False Then

                If rbtSummary.Checked = True Then
                    strSql += vbCrLf + " 'SUB TOTAL'PARTICULAR,''METALNAME,CATNAME"
                Else
                    strSql += vbCrLf + " 'SUB TOTAL'PARTICULAR,''METALNAME,CATNAME"
                End If
                strSql += vbCrLf + " ,SUM(OPENING)OPENING,SUM(RECEIPT)RECEIPT,SUM(ISSUE)ISSUE,SUM(CLOSING)CLOSING"
                strSql += vbCrLf + " ,' ' RATE,' ' VALUE,' 'CATCODE,2 RESULT"
            Else
                If rbtSummary.Checked = True Then
                    strSql += vbCrLf + " 'SUB TOTAL'PARTICULAR,''METALNAME,CATNAME,'' ITEMNAME"
                Else
                    strSql += vbCrLf + " 'SUB TOTAL'PARTICULAR,''METALNAME,CATNAME,'' ITEMNAME"
                End If
                strSql += vbCrLf + " ,SUM(OPENING)OPENING,SUM(RECEIPT)RECEIPT,SUM(ISSUE)ISSUE,SUM(CLOSING)CLOSING"
                strSql += vbCrLf + " ,0 ITEMID,' ' RATE,' ' VALUE,' 'CATCODE,2 RESULT"
            End If
            strSql += vbCrLf + " ,' 'FRM"
            strSql += vbCrLf + " ,' 'YYEAR,' 'MMONTHID,' 'MMONTH,' 'DDATE,' 'TRANDATE,' 'TRANNO, 'S'COLHEAD"
            strSql += vbCrLf + " FROM " & Temptable & "..TEMP" & systemId & "CATSTOCK AS T"
            If rbtSummary.Checked = True Then
                strSql += vbCrLf + " GROUP BY CATNAME,METALNAME"
            Else
                strSql += vbCrLf + " GROUP BY CATNAME,METALNAME"
            End If
            strSql += vbCrLf + " ) X"
            strSql += vbCrLf + " ORDER BY CATNAME,RESULT,METALNAME"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " IF (SELECT COUNT(*) FROM " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP)>0 "
            strSql += vbCrLf + " BEGIN "
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + " INSERT INTO " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP(METALNAME,CATNAME,ITEMID,SUBITEMID,ITEMNAME,SUBITEMNAME,RESULT,RATE,"
                strSql += vbCrLf + " VALUE,FRM,YYEAR,MMONTHID,MMONTH,DDATE,TRANDATE,TRANNO,COLHEAD,PARTICULAR )"
                If rbtSummary.Checked = True Then
                    strSql += vbCrLf + " SELECT DISTINCT METALNAME,METALNAME CATNAME,'','','',' ',' ',' ',0,0,'','','','','','','T','' "
                ElseIf rbtDateWise.Checked = True Then
                    strSql += vbCrLf + " SELECT DISTINCT METALNAME,CATNAME,,'','','',' ',' ',' ',0,0,'','','','','','','T','' "
                ElseIf rbtMonthWise.Checked = True Then
                    strSql += vbCrLf + " SELECT DISTINCT METALNAME,CATNAME,'','','',' ',' ',' ',0,0,'','','','','','','T','' "
                Else
                    strSql += vbCrLf + " SELECT DISTINCT METALNAME,CATNAME,'','','',' ',' ',' ',0,0,'','','','','','','T','' "
                End If
            ElseIf chkitemname.Checked = False And chksubitemname.Checked = False Then
                strSql += vbCrLf + " INSERT INTO " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP(METALNAME,CATNAME,RESULT,RATE,"
                strSql += vbCrLf + " VALUE,FRM,YYEAR,MMONTHID,MMONTH,DDATE,TRANDATE,TRANNO,COLHEAD,PARTICULAR )"
                If rbtSummary.Checked = True Then
                    strSql += vbCrLf + " SELECT DISTINCT METALNAME,METALNAME CATNAME,'',' ',0,0,'','','','','','','T','' "
                ElseIf rbtDateWise.Checked = True Then
                    strSql += vbCrLf + " SELECT DISTINCT METALNAME,CATNAME,'','',' ',' ',0,0,'','','','','','','T','' "
                ElseIf rbtMonthWise.Checked = True Then
                    strSql += vbCrLf + " SELECT DISTINCT METALNAME,CATNAME,'','',' ',' ',0,0,'','','','','','','T','' "
                Else
                    strSql += vbCrLf + " SELECT DISTINCT METALNAME,CATNAME,'',' ',' ',' ',0,0,'','','','','','','T','' "
                End If
            Else
                strSql += vbCrLf + " INSERT INTO " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP(METALNAME,CATNAME,ITEMID,ITEMNAME,RESULT,RATE,"
                strSql += vbCrLf + " VALUE,FRM,YYEAR,MMONTHID,MMONTH,DDATE,TRANDATE,TRANNO,COLHEAD,PARTICULAR )"
                If rbtSummary.Checked = True Then
                    strSql += vbCrLf + " SELECT DISTINCT METALNAME,METALNAME CATNAME,'',0,' ',' ',0,0,'','','','','','','T','' "
                ElseIf rbtDateWise.Checked = True Then
                    strSql += vbCrLf + " SELECT DISTINCT METALNAME,CATNAME,'','',' ',' ',0,0,'','','','','','','T','' "
                ElseIf rbtMonthWise.Checked = True Then
                    strSql += vbCrLf + " SELECT DISTINCT METALNAME,CATNAME,'','',' ',' ',0,0,'','','','','','','T','' "
                Else
                    strSql += vbCrLf + " SELECT DISTINCT METALNAME,CATNAME,'',' ',' ',' ',0,0,'','','','','','','T','' "
                End If
            End If
            strSql += vbCrLf + " FROM " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP WHERE RESULT =1"
            strSql += vbCrLf + " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " IF (SELECT COUNT(*) FROM " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP)>0 "
            strSql += vbCrLf + " BEGIN "
            If rbtSummary.Checked = True Then
                strSql += vbCrLf + " UPDATE " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP SET PARTICULAR = CASE WHEN RESULT = 0 THEN "
                strSql += vbCrLf + " METALNAME WHEN RESULT = 2 THEN 'SUB TOTAL' ELSE '  '+ CATNAME END"
            ElseIf rbtMonthWise.Checked = True Then
                strSql += vbCrLf + " UPDATE " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP SET PARTICULAR = CASE WHEN RESULT = 0 THEN "
                strSql += vbCrLf + " CATNAME WHEN RESULT = 2 THEN 'SUB TOTAL' ELSE '  '+ MMONTH END"
            ElseIf rbtDateWise.Checked = True Then
                strSql += vbCrLf + " UPDATE " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP SET PARTICULAR = CASE WHEN RESULT = 0 THEN "
                strSql += vbCrLf + " CATNAME WHEN RESULT = 2 THEN 'SUB TOTAL' ELSE ' '+ DDATE END"
            Else
                strSql += vbCrLf + " UPDATE " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP SET PARTICULAR = CASE WHEN RESULT = 0 THEN "
                strSql += vbCrLf + " CATNAME WHEN RESULT = 2 THEN 'SUB TOTAL' ELSE CONVERT(VARCHAR,TRANNO) END"
            End If
            strSql += vbCrLf + " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " IF (SELECT COUNT(*) FROM " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP)>0 "
            strSql += vbCrLf + " BEGIN "
            If chksubitemname.Checked = True Then
                strSql += vbCrLf + " INSERT INTO " & Temptable & "..TEMP" & systemId & "CATSUMM(METALNAME,CATNAME,ITEMNAME,SUBITEMNAME,OPENING,RECEIPT,ISSUE,"
                strSql += vbCrLf + " Closing ,ITEMID,SUBITEMID,Rate, VALUE, CATCODE, RESULT, FRM, YYEAR, MMONTHID, MMONTH, DDATE, "
                strSql += vbCrLf + " TRANDATE, TRANNO, COLHEAD) "
                strSql += vbCrLf + " SELECT METALNAME,PARTICULAR,ITEMNAME,SUBITEMNAME,OPENING,RECEIPT,ISSUE,"
                strSql += vbCrLf + " Closing, ITEMID,SUBITEMID ,Rate, VALUE, CATCODE, RESULT, FRM, YYEAR, MMONTHID, MMONTH, CONVERT(SMALLDATETIME,DDATE,103)DDATE, "
            ElseIf chksubitemname.Checked = False And chkitemname.Checked = False Then
                strSql += vbCrLf + " INSERT INTO " & Temptable & "..TEMP" & systemId & "CATSUMM(METALNAME,CATNAME,OPENING,RECEIPT,ISSUE,"
                strSql += vbCrLf + " Closing ,Rate, VALUE, CATCODE, RESULT, FRM, YYEAR, MMONTHID, MMONTH, DDATE, "
                strSql += vbCrLf + " TRANDATE, TRANNO, COLHEAD) "
                strSql += vbCrLf + " SELECT METALNAME,PARTICULAR,OPENING,RECEIPT,ISSUE,"
                strSql += vbCrLf + " Closing ,Rate, VALUE, CATCODE, RESULT, FRM, YYEAR, MMONTHID, MMONTH, CONVERT(SMALLDATETIME,DDATE,103)DDATE, "
            Else
                strSql += vbCrLf + " INSERT INTO " & Temptable & "..TEMP" & systemId & "CATSUMM(METALNAME,CATNAME,ITEMNAME,OPENING,RECEIPT,ISSUE,"
                strSql += vbCrLf + " Closing ,ITEMID,Rate, VALUE, CATCODE, RESULT, FRM, YYEAR, MMONTHID, MMONTH, DDATE, "
                strSql += vbCrLf + " TRANDATE, TRANNO, COLHEAD) "
                strSql += vbCrLf + " SELECT METALNAME,PARTICULAR,ITEMNAME,OPENING,RECEIPT,ISSUE,"
                strSql += vbCrLf + " Closing, ITEMID ,Rate, VALUE, CATCODE, RESULT, FRM, YYEAR, MMONTHID, MMONTH, CONVERT(SMALLDATETIME,DDATE,103)DDATE, "
            End If
            strSql += vbCrLf + " CONVERT(VARCHAR,TRANDATE,102)TRANDATE, TRANNO, COLHEAD"
            strSql += vbCrLf + " FROM " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP"
            If rbtSummary.Checked = True Then
                strSql += vbCrLf + " ORDER BY CATNAME,RESULT,METALNAME"
            Else
                'strSql += vbCrLf + " ORDER BY CATNAME,RESULT"
                strSql += vbCrLf + " ORDER BY CATNAME,RESULT,METALNAME"
            End If
            strSql += vbCrLf + " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            If chkWithValue.Checked = True Then funcRateValUpdation()
        End If
        strSql = " IF (SELECT COUNT(*) FROM " & Temptable & "..TEMP" & systemId & "CATSUMM)>0 "
        strSql += vbCrLf + " BEGIN "
        strSql += vbCrLf + " UPDATE " & Temptable & "..TEMP" & systemId & "CATSUMM SET OPENING=NULL WHERE OPENING = 0"
        ' strSql += vbCrLf + " UPDATE " & Temptable & "..TEMP" & systemId & "CATSUMM SET ITEMNAME='' WHERE ITEMNAME = 0"
        strSql += vbCrLf + " UPDATE " & Temptable & "..TEMP" & systemId & "CATSUMM SET CATNAME=NULL  WHERE ITEMNAME <> '' and  itemid <> 0 "
        strSql += vbCrLf + " UPDATE " & Temptable & "..TEMP" & systemId & "CATSUMM SET RECEIPT=NULL WHERE RECEIPT = 0"
        strSql += vbCrLf + " UPDATE " & Temptable & "..TEMP" & systemId & "CATSUMM SET ISSUE=NULL WHERE ISSUE = 0"
        strSql += vbCrLf + " UPDATE " & Temptable & "..TEMP" & systemId & "CATSUMM SET CLOSING=NULL WHERE CLOSING= 0"
        strSql += vbCrLf + " UPDATE " & Temptable & "..TEMP" & systemId & "CATSUMM SET RATE = '' WHERE RATE = '0.00'"
        strSql += vbCrLf + " UPDATE " & Temptable & "..TEMP" & systemId & "CATSUMM SET VALUE = '' WHERE VALUE='0.00'"
        If chkitemname.Checked = True Or chksubitemname.Checked = True Then
            strSql += vbCrLf + "Update " & Temptable & "..TEMP" & systemId & "CATSUMM SET COLHEAD='C' WHERE ISNULL(ITEMNAME,'')='' AND COLHEAD=''"
        End If
        strSql += vbCrLf + " UPDATE " & Temptable & "..TEMP" & systemId & "CATSUMM SET VALUE = CONVERT(VARCHAR,(SELECT CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(VALUE,'') = '' THEN '0' ELSE VALUE END))) FROM " & Temptable & "..TEMP" & systemId & "CATSUMM WHERE METALNAME = T.METALNAME))"
        strSql += vbCrLf + " FROM " & Temptable & "..TEMP" & systemId & "CATSUMM AS T WHERE RESULT = 2"
        strSql += vbCrLf + " END "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        If rbtMetalDateWise.Checked Then
            strSql = " IF (SELECT COUNT(*) FROM " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP)>0 "
            strSql += vbCrLf + " BEGIN "
            strSql += vbCrLf + " UPDATE " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP SET OPENING=NULL WHERE OPENING = 0"
            strSql += vbCrLf + " UPDATE " & Temptable & "..TEMP" & systemId & "CATSUMM SET ITEMNAME='' WHERE ITEMNAME = 0"
            strSql += vbCrLf + " UPDATE " & Temptable & "..TEMP" & systemId & "CATSUMM SET CATNAME='' WHERE ITEMNAME <>'' "
            strSql += vbCrLf + " UPDATE " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP SET RECEIPT=NULL WHERE RECEIPT = 0"
            strSql += vbCrLf + " UPDATE " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP SET ISSUE=NULL WHERE ISSUE = 0"
            strSql += vbCrLf + " UPDATE " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP SET CLOSING=NULL WHERE CLOSING= 0"
            strSql += vbCrLf + " UPDATE " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP SET RATE = '' WHERE RATE = '0.00'"
            strSql += vbCrLf + " UPDATE " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP SET VALUE = '' WHERE VALUE='0.00'"
            If chkitemname.Checked = True Then
                strSql += vbCrLf + "Update " & Temptable & "..TEMP" & systemId & "CATSUMM SET COLHEAD='C' WHERE ISNULL(ITEMNAME,'')='' AND COLHEAD=''"
            End If
            'strSql += vbCrLf + " UPDATE TEMP" & systemId & "CTRSTOCKTEMP SET VALUE = CONVERT(VARCHAR,(SELECT CONVERT(VARCHAR,SUM(CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(VALUE,'') = '' THEN '0' ELSE VALUE END))) FROM TEMP" & systemId & "CTRSTOCKTEMP WHERE METALNAME = T.METALNAME))"
            'strSql += vbCrLf + " FROM TEMP" & systemId & "CTRSTOCKTEMP AS T WHERE RESULT = 2"
            strSql += vbCrLf + " END "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = " SELECT * FROM " & Temptable & "..TEMP" & systemId & "CTRSTOCKTEMP ORDER BY METALNAME,RESULT,TRANDATE"
        Else
            'strSql = " SELECT * FROM " & Temptable & "..TEMP" & systemId & "CATSUMM ORDER BY  CATNAME,RESULT,METALNAME"
            If chkitemname.Checked Then
                strSql = " SELECT METALNAME,CATNAME,ITEMNAME,OPENING,RECEIPT,ISSUE,CLOSING,COLHEAD,RATE,VALUE FROM " & Temptable & "..TEMP" & systemId & "CATSUMM "
                If chksubitemname.Checked = True Then
                    strSql = " SELECT METALNAME,CATNAME,ITEMNAME,SUBITEMNAME,OPENING,RECEIPT,ISSUE,CLOSING,COLHEAD,RATE,VALUE FROM " & Temptable & "..TEMP" & systemId & "CATSUMM "
                End If
            ElseIf chksubitemname.Checked = True Then
                strSql = " SELECT METALNAME,CATNAME,SUBITEMNAME,OPENING,RECEIPT,ISSUE,CLOSING,COLHEAD,RATE,VALUE FROM " & Temptable & "..TEMP" & systemId & "CATSUMM "
            Else
                strSql = " SELECT METALNAME,CATNAME,OPENING,RECEIPT,ISSUE,CLOSING,COLHEAD,RATE,VALUE FROM " & Temptable & "..TEMP" & systemId & "CATSUMM "
            End If
            'strSql = " SELECT * FROM " & Temptable & "..TEMP" & systemId & "CATSUMM "
        End If



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
            .Columns("COLHEAD").Visible = False
            If .Columns.Contains("CATNAME") Then
                With .Columns("CATNAME")
                    .HeaderText = "PARTICULAR"
                    .ReadOnly = True
                    .Width = 400
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            End If
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
            For CNT As Integer = 7 To gridView.ColumnCount - 1
                .Columns(CNT).Visible = False
            Next
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            '.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        End With
        FormatGridColumns(gridView)
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


    Public Sub FormatGridColumns(ByVal grid As DataGridView, Optional ByVal colHeadVisibleSetFalse As Boolean = True, Optional ByVal colFormat As Boolean = True, Optional ByVal reeadOnly As Boolean = True, Optional ByVal sortColumns As Boolean = True)
            With grid
                If colHeadVisibleSetFalse = False Then .ColumnHeadersVisible = False
                For i As Integer = 0 To .ColumnCount - 1
                    If .Columns(i).ValueType.Name Is GetType(Date).Name Then
                        If colFormat Then .Columns(i).DefaultCellStyle.Format = "dd/MM/yyyy"
                    End If
                Next
            End With
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
                    If (Val(.Cells("OPENING").Value.ToString) <> 0 Or Val(.Cells("ISSUE").Value.ToString) <> 0 Or Val(.Cells("RECEIPT").Value.ToString) <> 0 Or Val(.Cells("CLOSING").Value.ToString) <> 0) Then
                        .Cells(e.ColumnIndex).Value = ""
                        Exit Sub
                    End If
                End If
                If IsNumeric(.Cells(e.ColumnIndex).Value.ToString) = False Then
                    .Cells(e.ColumnIndex).Value = ""
                    Exit Sub
                End If
                Dim totVal As Double = Val(.Cells(e.ColumnIndex).Value.ToString) * Val(.Cells(e.ColumnIndex - 1).Value.ToString)
                If (Val(.Cells("OPENING").Value.ToString) = 0 Or Val(.Cells("ISSUE").Value.ToString) = 0 Or Val(.Cells("RECEIPT").Value.ToString) = 0 Or Val(.Cells("CLOSING").Value.ToString) = 0) Then
                    totVal = Val(.Cells(e.ColumnIndex).Value.ToString)
                    .Cells(e.ColumnIndex + 1).Value = Format(totVal, "0.00")
                End If
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
                            If gridView.Columns.Contains("CATNAME") Then .Cells("CATNAME").Style.BackColor = Nothing
                        Case "S"
                            .DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                            .DefaultCellStyle.Font = reportSubTotalStyle.Font
                        Case "T"
                        If rbtMetalDateWise.Checked Then
                            If gridView.Columns.Contains("PARTICULAR") Then .Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                            If gridView.Columns.Contains("PARTICULAR") Then .Cells("PARTICULAR").Style.Font = reportHeadStyle.Font

                        Else
                            If gridView.Columns.Contains("CATNAME") Then .Cells("CATNAME").Style.BackColor = reportHeadStyle.BackColor
                            If gridView.Columns.Contains("CATNAME") Then .Cells("CATNAME").Style.Font = reportHeadStyle.Font

                        End If
                    Case "C"
                        'If gridView.Columns.Contains("CATNAME") Then .Cells("CATNAME").Style.BackColor = reportHeadStyle.BackColor
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
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
                If UCase(e.KeyChar) = "D" Then
                    Dim strsql As String = ""
                    Dim SelectedCompany As String = GetSelectedCompanyId(chkCmbCompany, True)
                    Dim SelectedCostCentre As String = GetChecked_CheckedList(chkCmbCostCentre, True)
                    Dim objGridShower As New frmGridDispDia
                    objGridShower.Name = Me.Name
                    objGridShower.gridView.RowTemplate.Height = 21
                    objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
                    objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    objGridShower.Text = "CATEGORY STOCK DETAILS"
                    Dim tit As String = "CATEGORY STOCK DETAILS"
                    objGridShower.lblTitle.Text = tit
                    objGridShower.StartPosition = FormStartPosition.CenterScreen
                    objGridShower.dsGrid.DataSetName = objGridShower.Name

                    Dim systemName As String = Environment.MachineName

                    strsql = " IF OBJECT_ID('TEMPTABLEDB..[TEMPCATISSUERECEIPT" & systemName & "]','U') IS NOT NULL "
                    strsql += vbCrLf + " DROP TABLE TEMPTABLEDB..[TEMPCATISSUERECEIPT" & systemName & "]"
                    cmd = New OleDbCommand(strsql, cn)
                    cmd.ExecuteNonQuery()

                    strsql = " CREATE TABLE TEMPTABLEDB..[TEMPCATISSUERECEIPT" & systemName & "]"
                    strsql += " ( "
                    strsql += " TRANNO INT"
                    strsql += " ,TRANDATE VARCHAR(15)"
                    strsql += " ,CATCODE VARCHAR(50),CATNAME VARCHAR(150)"
                    If rbtGrsWt.Checked = True Then strsql += "  ,GRSWT NUMERIC(15,3)"
                    If rbtNetWt.Checked = True Then strsql += "  ,NETWT NUMERIC(15,3)"
                    If rbtPurewt.Checked = True Then strsql += " ,PUREWT NUMERIC(15,3)"
                    strsql += " ,TRANTYPE VARCHAR(100)"
                    strsql += " ,COLHEAD VARCHAR(1)"
                    strsql += " ,RESULT INT"
                    strsql += " ) "
                    cmd = New OleDbCommand(strsql, cn)
                    cmd.ExecuteNonQuery()

                    strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMPCATISSUERECEIPT" & systemName & "]"
                    strsql += vbCrLf + " (CATNAME,RESULT) VALUES('RECEIPT',0)"
                    cmd = New OleDbCommand(strsql, cn)
                    cmd.ExecuteNonQuery()


                    strsql = " INSERT INTO TEMPTABLEDB..[TEMPCATISSUERECEIPT" & systemName & "]"
                    strsql += vbCrLf + " ( "
                    strsql += vbCrLf + " CATCODE,CATNAME"
                    If rbtGrsWt.Checked = True Then strsql += vbCrLf + " ,GRSWT"
                    If rbtNetWt.Checked = True Then strsql += vbCrLf + " ,NETWT"
                    If rbtPurewt.Checked = True Then strsql += vbCrLf + " ,PUREWT"
                    strsql += vbCrLf + " ,TRANDATE,TRANNO,TRANTYPE,COLHEAD,RESULT)"
                    strsql += vbCrLf + " SELECT R.CATCODE"
                    strsql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=R.CATCODE) CATNAME"
                    If rbtGrsWt.Checked = True Then
                        strsql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN R.GRSWT / 5 ELSE R.GRSWT END) GRSWT"
                    End If
                    If rbtNetWt.Checked = True Then
                        strsql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN NETWT / 5 ELSE NETWT END) NETWT"
                    End If
                    If rbtPurewt.Checked = True Then
                        strsql += vbCrLf + " ,SUM(CASE WHEN R.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN PUREWT / 5 ELSE PUREWT END) PUREWT"
                    End If
                    strsql += vbCrLf + " ,CONVERT(VARCHAR(15),TRANDATE,105) TRANDATE,TRANNO"
                    strsql += vbCrLf + " ,CASE WHEN R.TRANTYPE = 'PU' THEN 'PURCHASE' "
                    strsql += vbCrLf + "  ELSE CASE WHEN R.TRANTYPE = 'SR' THEN 'SALES RETURN' "
                    strsql += vbCrLf + "  ELSE CASE WHEN R.TRANTYPE = 'RIN' THEN 'INTERNAL TRANSFER' "
                    strsql += vbCrLf + "  ELSE CASE WHEN R.TRANTYPE = 'AD' THEN 'AD' "
                    strsql += vbCrLf + "  ELSE CASE WHEN R.TRANTYPE = 'RPU' THEN 'PURCHASE' "
                    strsql += vbCrLf + "  ELSE CASE WHEN R.TRANTYPE = 'RAP' THEN 'APPROVAL RECEIPT' "
                    strsql += vbCrLf + "  ELSE CASE WHEN R.TRANTYPE = 'ROT' THEN 'OTHER RECEIPT' "
                    strsql += vbCrLf + "  ELSE CASE WHEN R.TRANTYPE = 'RPA' THEN 'PACKING' "
                    strsql += vbCrLf + "  ELSE CASE WHEN R.TRANTYPE = 'AD' THEN 'AD' "
                    strsql += vbCrLf + " ELSE R.TRANTYPE"
                    strsql += vbCrLf + " END END END END END END END END END"
                    strsql += vbCrLf + " ,'D' COLHEAD, 1 RESULT "
                    strsql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS R"
                    strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON R.CATCODE = C.CATCODE"
                    strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST M ON R.ITEMID = M.ITEMID"
                    strsql += vbCrLf + " WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''"
                    strsql += vbCrLf + " AND ISNULL(R.TRANTYPE,'') NOT IN  ('RRC') "
                    strsql += vbCrLf + " AND R.TRANTYPE <> 'WOT' "
                    If rbtMfg.Checked Then
                        strsql += vbCrLf + " AND ISNULL(R.STKTYPE,'')='M'"
                    ElseIf rbtTrading.Checked Then
                        strsql += vbCrLf + " AND ISNULL(R.STKTYPE,'')NOT IN('M','E')"
                    ElseIf rbtExem.Checked Then
                        strsql += vbCrLf + " AND ISNULL(R.STKTYPE,'')='E'"
                    End If
                    If cmbCatName.Text <> "ALL" Then
                        strsql += vbCrLf + " And R.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY  WHERE CATNAME IN ('" & cmbCatName.Text & "'))"
                    End If
                    If cmbMetal.Text <> "ALL" Then
                        strsql += vbCrLf + " And R.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY  WHERE R.METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST  WHERE METALNAME = '" & cmbMetal.Text & "'))"
                    End If
                    If chkCmbCompany.Text <> "ALL" Then
                        strsql += vbCrLf + " AND R.COMPANYID IN (" & SelectedCompany & ")"
                    End If
                    If chkCmbCostCentre.Text <> "ALL" Then
                        strsql += vbCrLf + " And COSTID IN(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & SelectedCostCentre & "))"
                    End If
                    strsql += vbCrLf + " GROUP BY R.CATCODE,TRANDATE,TRANNO,TRANSTATUS,M.STUDDED,R.TRANTYPE"
                    cmd = New OleDbCommand(strsql, cn)
                    cmd.ExecuteNonQuery()

                    strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMPCATISSUERECEIPT" & systemName & "]"
                    strsql += vbCrLf + " (CATNAME"
                    If rbtGrsWt.Checked = True Then strsql += vbCrLf + " ,GRSWT "
                    If rbtNetWt.Checked = True Then strsql += vbCrLf + " ,NETWT "
                    If rbtPurewt.Checked = True Then strsql += vbCrLf + " ,PUREWT "
                    strsql += vbCrLf + " ,RESULT)"
                    strsql += vbCrLf + " SELECT 'TOTAL'"
                    If rbtGrsWt.Checked = True Then strsql += vbCrLf + " , SUM(GRSWT) GRSWT "
                    If rbtNetWt.Checked = True Then strsql += vbCrLf + " , SUM(NETWT) NETWT "
                    If rbtPurewt.Checked = True Then strsql += vbCrLf + " , SUM(PUREWT) PUREWT "
                    strsql += vbCrLf + " ,2 RESULT"
                    strsql += vbCrLf + " FROM TEMPTABLEDB..[TEMPCATISSUERECEIPT" & systemName & "] WHERE RESULT = 1"
                    cmd = New OleDbCommand(strsql, cn)
                    cmd.ExecuteNonQuery()

                    strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMPCATISSUERECEIPT" & systemName & "]"
                    strsql += vbCrLf + " (CATNAME,RESULT) VALUES('ISSUE',3)"
                    cmd = New OleDbCommand(strsql, cn)
                    cmd.ExecuteNonQuery()

                    strsql = " INSERT INTO TEMPTABLEDB..[TEMPCATISSUERECEIPT" & systemName & "]"
                    strsql += vbCrLf + " ( "
                    strsql += vbCrLf + " CATCODE,CATNAME"
                    If rbtGrsWt.Checked = True Then strsql += vbCrLf + " ,GRSWT"
                    If rbtNetWt.Checked = True Then strsql += vbCrLf + " ,NETWT"
                    If rbtPurewt.Checked = True Then strsql += vbCrLf + " ,PUREWT"
                    strsql += vbCrLf + " ,TRANDATE"
                    strsql += vbCrLf + " ,TRANNO,TRANTYPE,COLHEAD,RESULT) "
                    strsql += vbCrLf + " SELECT I.CATCODE"
                    strsql += vbCrLf + " ,(SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE=I.CATCODE) CATNAME"
                    If rbtGrsWt.Checked = True Then
                        strsql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN I.GRSWT / 5 ELSE I.GRSWT END)GRSWT"
                    End If
                    If rbtNetWt.Checked = True Then
                        strsql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN NETWT / 5 ELSE NETWT END)NETWT"
                    End If
                    If rbtPurewt.Checked = True Then
                        strsql += vbCrLf + " ,SUM(CASE WHEN I.STONEUNIT = 'C' AND C.METALID IN ('T','D') THEN PUREWT / 5 ELSE PUREWT END)PUREWT"
                    End If
                    strsql += vbCrLf + " ,CONVERT(VARCHAR(15),TRANDATE,105) TRANDATE,TRANNO"
                    strsql += vbCrLf + " ,CASE WHEN I.TRANTYPE = 'MI' THEN 'MISC ISSUE'"
                    strsql += vbCrLf + " ELSE CASE WHEN I.TRANTYPE = 'IIN' THEN 'INTERNAL TRANSFER' "
                    strsql += vbCrLf + " ELSE CASE WHEN I.TRANTYPE = 'IIS' THEN 'MATERIAL ISSUE' "
                    strsql += vbCrLf + " ELSE CASE WHEN I.TRANTYPE = 'IPU' THEN 'PURCHASE RETRUN' "
                    strsql += vbCrLf + " ELSE CASE WHEN I.TRANTYPE = 'IAP' THEN 'APPROVAL ISSUE' "
                    strsql += vbCrLf + " ELSE CASE WHEN I.TRANTYPE = 'IOT' THEN 'OTHER ISSUE' "
                    strsql += vbCrLf + " ELSE CASE WHEN I.TRANTYPE = 'IPA' THEN 'PACKING' "
                    strsql += vbCrLf + " ELSE CASE WHEN I.TRANTYPE = 'OD' THEN 'ORDER DELIVERY' "
                    strsql += vbCrLf + " ELSE CASE WHEN I.TRANTYPE = 'SA' THEN 'SALES' "
                    strsql += vbCrLf + " ELSE I.TRANTYPE "
                    strsql += vbCrLf + " END END END END END END END END END"
                    strsql += vbCrLf + " ,'D' COLHEAD, 4 RESULT "
                    strsql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
                    strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CATEGORY C ON I.CATCODE = C.CATCODE"
                    strsql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST M ON M.ITEMID = I.ITEMID"
                    strsql += vbCrLf + " WHERE TRANDATE BETWEEN '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "' AND ISNULL(CANCEL,'') = ''"
                    strsql += vbCrLf + " AND I.SNO NOT IN( SELECT ISSSNO FROM " & cnStockDb & "..ISSMETAL WHERE TRANDATE BETWEEN  '" & Format(dtpFrom.Value.Date, "yyyy-MM-dd") & "' AND '" & Format(dtpTo.Value.Date, "yyyy-MM-dd") & "' AND I.TRANTYPE IN ('SA','OD'))"
                    strsql += vbCrLf + " AND ISNULL(I.TRANTYPE,'') NOT IN  ('IRC') "
                    strsql += vbCrLf + " AND I.TRANTYPE <> 'WOT' "
                    If rbtMfg.Checked Then
                        strsql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='M'"
                    ElseIf rbtTrading.Checked Then
                        strsql += vbCrLf + " AND ISNULL(I.STKTYPE,'')NOT IN('M','E')"
                    ElseIf rbtExem.Checked Then
                        strsql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='E'"
                    End If
                    If cmbCatName.Text <> "ALL" Then
                        strsql += vbCrLf + " And I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY  WHERE CATNAME IN ('" & cmbCatName.Text & "'))"
                    End If
                    If cmbMetal.Text <> "ALL" Then
                        strsql += vbCrLf + " And I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY  WHERE I.METALID = (SELECT METALID FROM " & cnAdminDb & "..METALMAST  WHERE METALNAME = '" & cmbMetal.Text & "'))"
                    End If
                    If chkCmbCompany.Text <> "ALL" Then
                        strsql += vbCrLf + " AND I.COMPANYID IN (" & SelectedCompany & ") "
                    End If
                    If chkCmbCostCentre.Text <> "ALL" Then
                        strsql += vbCrLf + " And COSTID IN(SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & SelectedCostCentre & "))"
                    End If

                    strsql += vbCrLf + " GROUP BY I.CATCODE,TRANDATE,TRANNO,TRANSTATUS,M.STUDDED,I.TRANTYPE"
                    cmd = New OleDbCommand(strsql, cn)
                    cmd.ExecuteNonQuery()

                    strsql = vbCrLf + " INSERT INTO TEMPTABLEDB..[TEMPCATISSUERECEIPT" & systemName & "]"
                    strsql += vbCrLf + " (CATNAME"
                    If rbtGrsWt.Checked = True Then strsql += vbCrLf + " ,GRSWT "
                    If rbtNetWt.Checked = True Then strsql += vbCrLf + " ,NETWT "
                    If rbtPurewt.Checked = True Then strsql += vbCrLf + " ,PUREWT "
                    strsql += vbCrLf + " ,RESULT)"
                    strsql += vbCrLf + " SELECT 'TOTAL'"
                    If rbtGrsWt.Checked = True Then strsql += vbCrLf + " , SUM(GRSWT) GRSWT "
                    If rbtNetWt.Checked = True Then strsql += vbCrLf + " , SUM(NETWT) NETWT "
                    If rbtPurewt.Checked = True Then strsql += vbCrLf + " , SUM(PUREWT) PUREWT "
                    strsql += vbCrLf + " ,5 RESULT"
                    strsql += vbCrLf + " FROM TEMPTABLEDB..[TEMPCATISSUERECEIPT" & systemName & "] WHERE RESULT = 4"
                    cmd = New OleDbCommand(strsql, cn)
                    cmd.ExecuteNonQuery()


                    strsql = "SELECT * FROM TEMPTABLEDB..[TEMPCATISSUERECEIPT" & systemName & "]"
                    Dim dtGrid As New DataTable
                    da = New OleDbDataAdapter(strsql, cn)
                    da.Fill(dtGrid)
                    objGridShower.dsGrid.Tables.Add(dtGrid)
                    objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
                    objGridShower.gridView.Columns("COLHEAD").Visible = False
                    objGridShower.gridView.Columns("RESULT").Visible = False
                    objGridShower.gridView.Columns("CATCODE").Visible = False
                    objGridShower.gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
                    objGridShower.gridView.AllowUserToOrderColumns = True
                    For Each col As DataGridViewColumn In objGridShower.gridView.Columns
                        col.SortMode = DataGridViewColumnSortMode.NotSortable
                    Next
                    objGridShower.FormReSize = False
                    objGridShower.FormReLocation = False
                    objGridShower.pnlFooter.Visible = False
                    AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
                    objGridShower.gridViewHeader.Visible = True

                    objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
                    objGridShower.gridViewHeader.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    objGridShower.formuser = userId
                    objGridShower.pnlHeader.Size = New Size(objGridShower.pnlHeader.Size.Width, objGridShower.pnlHeader.Size.Height + 10)
                    objGridShower.FormReSize = True
                    objGridShower.FormReLocation = True
                    FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
                    For i As Integer = 0 To objGridShower.gridView.Rows.Count - 1
                        If objGridShower.gridView.Rows(i).Cells("RESULT").Value.ToString = "2" Or objGridShower.gridView.Rows(i).Cells("RESULT").Value.ToString = "5" Then
                            objGridShower.gridView.Rows(i).DefaultCellStyle.BackColor = Color.LightBlue
                        ElseIf objGridShower.gridView.Rows(i).Cells("RESULT").Value.ToString = "0" Or objGridShower.gridView.Rows(i).Cells("RESULT").Value.ToString = "3" Then
                            objGridShower.gridView.Rows(i).Cells("CATNAME").Style.BackColor = Color.Blue
                        End If
                    Next
                    objGridShower.Show()
                End If
            End If
        End Sub

        Private Sub gridView_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs)
            SetGridHeadColWidth(CType(sender, DataGridView))
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
            If DISABLE_ECISEDUTY Then
                Label10.Enabled = False
                Panel4.Enabled = False
            End If
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
            If rbtGrsWt.Checked = False And CATSTOCK_NETWT_BASED = False Then
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

            strSql = " UPDATE " & Temptable & "..TEMP" & systemId & "CATSUMM SET RATE = C.RATE,VALUE = CONVERT(NUMERIC(15,2),ISNULL(C.CLOSING,0) * ISNULL(C.RATE ,0))"
            strSql += vbCrLf + " FROM " & Temptable & "..TEMP" & systemId & "CATSUMM AS T INNER JOIN " + cnStockDb + "..CLOSINGVAL AS C ON ISNULL(T.CATCODE,'') = ISNULL(C.CATCODE,'')"
            strSql += vbCrLf + " WHERE C.COMPANYID = '" + strCompId + "' AND C.COSTID = '" + strCostId + "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            If CATSTOCK_PCS_VALUE Then
                strSql = " UPDATE " & Temptable & "..TEMP" & systemId & "CATSUMM SET RATE = C.RATE,VALUE = C.VALUE"
                strSql += vbCrLf + " FROM " & Temptable & "..TEMP" & systemId & "CATSUMM AS T INNER JOIN " + cnStockDb + "..CLOSINGVAL AS C ON ISNULL(T.CATCODE,'') = ISNULL(C.CATCODE,'')"
                strSql += vbCrLf + " WHERE C.COMPANYID = '" + strCompId + "' AND C.COSTID = '" + strCostId + "' AND ISNULL(OPENING,0)=0 AND ISNULL(RECEIPT,0)=0 AND ISNULL(ISSUE,0)=0 AND ISNULL(T.CLOSING,0)=0"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If
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
                If rbtGrsWt.Checked = False And CATSTOCK_NETWT_BASED = False Then
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
            Dim obj As New frmCategoryStock_Properties
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
            obj.p_rbtStkWise = rbtStkType.Checked
            obj.p_chkWithValue = chkWithValue.Checked
            obj.p_chkWithOtherIssue = chkWithOtherIssue.Checked
            obj.p_chkWithStuddedStone = chkWithStuddedStone.Checked
            obj.p_chkAlloy = chkalloy.Checked
            SetSettingsObj(obj, Me.Name, GetType(frmCategoryStock_Properties))
        End Sub

        Private Sub Prop_Gets()
        Dim obj As New frmCategoryStock_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmCategoryStock_Properties))
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
        'chkitemname.Checked = obj.p_chkitemname
        'chksubitemname.Checked = obj.p_chksubitemname
        rbtMonthWise.Checked = obj.p_rbtMonthWise
            rbtDateWise.Checked = obj.p_rbtDateWise
            rbtTranNoWise.Checked = obj.p_rbtTranNoWise
            rbtStkType.Checked = obj.p_rbtStkWise
            chkWithValue.Checked = obj.p_chkWithValue
            chkWithOtherIssue.Checked = obj.p_chkWithOtherIssue
            chkWithStuddedStone.Checked = obj.p_chkWithStuddedStone
            chkalloy.Checked = obj.p_chkAlloy
        End Sub

        Private Sub ReziseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReziseToolStripMenuItem.Click
            If gridView.RowCount > 0 Then
                If ReziseToolStripMenuItem.Checked Then
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

Public Class frmItemWiseStocknew_Properties
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
    Private rbtStkWise As Boolean = False
    Public Property p_rbtStkWise() As Boolean
        Get
            Return rbtStkWise
        End Get
        Set(ByVal value As Boolean)
            rbtStkWise = value
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
    Private chkAlloy As Boolean = False
    Public Property p_chkAlloy() As Boolean
        Get
            Return chkAlloy

        End Get
        Set(ByVal value As Boolean)
            chkAlloy = value
        End Set
    End Property
    Private chksubitemname As Boolean = False
    Public Property p_chksubitemname() As Boolean
        Get
            Return chksubitemname

        End Get
        Set(ByVal value As Boolean)
            chksubitemname = value
        End Set
    End Property
    Private chkitemname As Boolean = False
    Public Property p_chkitemname() As Boolean
        Get
            Return chkitemname

        End Get
        Set(ByVal value As Boolean)
            chkitemname = value
        End Set
    End Property
End Class


