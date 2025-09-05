Imports System.Data.OleDb
Public Class frmSaleReportWastMc
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim objGridShower As frmGridDispDia
    Dim SelectedCompany As String
    Dim SqlVersion As Integer = 8
    Dim FORMAT2 As Boolean = IIf(GetAdmindbSoftValue("AGR8-18-FORMAT2", "") = "Y", True, False)
    Dim SPECIFICFORMAT As String = GetAdmindbSoftValue("SPECIFICFORMAT", "0")

    Private Sub frmSaleReportWastMc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        'rbtTag.Checked = True
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        Prop_Gets()
        If ChkInclude.Checked Then
            For i As Integer = 0 To ChklstboxInclude.Items.Count - 1
                ChklstboxInclude.SetItemChecked(i, True)
            Next
        End If
        dtpFrom.Select()

    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre)
        Dim chkMetalName As String = GetChecked_CheckedList(chkLstMetal)
        Dim chkcategoryName As String = GetChecked_CheckedList(chkLstCategory)
        Dim SelectedCompany As String = GetSelectedCompanyId(chkLstCompany, True)
        Dim tableName As String = "ISSUE"
        Dim trantype As String = "SA"
        strSql = " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "SALEDET')>0 DROP TABLE TEMP" & systemId & "SALEDET"
        strSql += vbCrLf + " SELECT  "
        strSql += vbCrLf + " * INTO TEMP" & systemId & "SALEDET FROM ( "
        strSql += vbCrLf + " SELECT "
        If chkSpecificFormat.Checked = True Then
            strSql += vbCrLf + " TRANDATE,TRANNO"
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID ) ITEMNAME"
            strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = I.ITEMID AND SUBITEMID = I.SUBITEMID) SUBITEMNAME"
            strSql += vbCrLf + " ,WASTPER,MCHARGE,AMOUNT"
            strSql += vbCrLf + " ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = I.EMPID) EMPNAME "
            strSql += vbCrLf + " ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = I.COSTID) COSTNAME"
        Else
            strSql += vbCrLf + " CASE WHEN SUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID)"
            strSql += vbCrLf + " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID)END AS [DESCRIPTION]"
            strSql += vbCrLf + " ,TAGNO"
            strSql += vbCrLf + " ,CASE WHEN TAGGRSWT = 0 THEN GRSWT ELSE TAGGRSWT END AS STGRSWT,GRSWT SAGRSWT"
            If SPECIFICFORMAT = "1" Then
                strSql += vbCrLf + " ,CASE WHEN ISNULL((SELECT SUM(WEIGHT)WEIGHT FROM " & cnStockDb & "..WT2WTAMTCAL WHERE ISSSNO =I.SNO and RUNNO like '%CHIT'),0) > 0 and ISNULL(I.GRSWT,0) > 0 THEN "
                strSql += vbCrLf + " ISNULL(I.GRSWT,0) - ISNULL((SELECT SUM(WEIGHT)WEIGHT FROM " & cnStockDb & "..WT2WTAMTCAL WHERE ISSSNO =I.SNO and RUNNO like '%CHIT'),0) "
                strSql += vbCrLf + " ELSE 0 END SAEXTRAWT"
            End If
            If chkstnamt.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(STNAMT)STNAMT FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGNO =I.TAGNO )STSTNAMT"
                strSql += vbCrLf + " ,(SELECT SUM(STNAMT)STNAMT FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO =I.SNO ) SASTNAMT"
            End If
            strSql += vbCrLf + " ,CONVERT(VARCHAR(50),'')STWASTAGE"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(50),'')STWASTAGE1"

            If SPECIFICFORMAT = "1" Then
                strSql += vbCrLf + " ,CASE WHEN ISNULL((SELECT COUNT(*)CNT FROM " & cnStockDb & "..WT2WTAMTCAL WHERE ISSSNO =I.SNO ),0) > 0 THEN "
                strSql += vbCrLf + " ISNULL((SELECT AVG(WASTPER)WASTPER FROM " & cnStockDb & "..WT2WTAMTCAL WHERE ISSSNO =I.SNO),0) "
                strSql += vbCrLf + " ELSE ISNULL(REPLICATE(' ',5-LEN(ISNULL(CONVERT(NUMERIC(15,2),(WASTAGE/CASE WHEN GRSWT <> 0 THEN GRSWT ELSE 1 END)*100),0))),'')+CONVERT(VARCHAR,ISNULL(CONVERT(NUMERIC(15,2),(WASTAGE/CASE WHEN GRSWT <> 0  THEN GRSWT ELSE 1 END)*100),0)) "
                strSql += vbCrLf + " END SAWASTAGE"
                strSql += vbCrLf + " ,CASE WHEN ISNULL((SELECT COUNT(*)CNT FROM " & cnStockDb & "..WT2WTAMTCAL WHERE ISSSNO =I.SNO ),0) > 0 THEN "
                strSql += vbCrLf + " ISNULL((SELECT SUM(WASTAGE)WASTAGE FROM " & cnStockDb & "..WT2WTAMTCAL WHERE ISSSNO =I.SNO),0) "
                strSql += vbCrLf + " ELSE ISNULL(REPLICATE(' ',7-LEN(ISNULL(WASTAGE,0))),'')+CONVERT(VARCHAR,ISNULL(WASTAGE,0)) "
                strSql += vbCrLf + " END SAWASTAGE1"
            Else
                strSql += vbCrLf + " ,ISNULL(REPLICATE(' ',5-LEN(ISNULL(CONVERT(NUMERIC(15,2),(WASTAGE/CASE WHEN GRSWT <> 0 THEN GRSWT ELSE 1 END)*100),0))),'')+CONVERT(VARCHAR,ISNULL(CONVERT(NUMERIC(15,2),(WASTAGE/CASE WHEN GRSWT <> 0  THEN GRSWT ELSE 1 END)*100),0))"
                strSql += vbCrLf + " AS SAWASTAGE"
                strSql += vbCrLf + " ,ISNULL(REPLICATE(' ',7-LEN(ISNULL(WASTAGE,0))),'')+CONVERT(VARCHAR,ISNULL(WASTAGE,0))"
                strSql += vbCrLf + " AS SAWASTAGE1"
            End If
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0)STMC"
            strSql += vbCrLf + " ,MCHARGE SAMC"
            strSql += vbCrLf + " ,(ISNULL(DISCOUNT,0)+ISNULL(FIN_DISCOUNT,0))DISCOUNT"
            strSql += vbCrLf + " ,CASE WHEN (ISNULL(DISCOUNT,0)+ISNULL(FIN_DISCOUNT,0))<>0 "
            strSql += vbCrLf + " THEN CONVERT(NUMERIC(15,2),((ISNULL(DISCOUNT,0)+ISNULL(FIN_DISCOUNT,0))*100)/(AMOUNT+ISNULL(DISCOUNT,0)+ISNULL(FIN_DISCOUNT,0))) ELSE NULL END DISPER"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),((GRSWT+WASTAGE)*BOARDRATE)+MCHARGE)STVALUE"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),((GRSWT+WASTAGE)*RATE)+MCHARGE-(ISNULL(DISCOUNT,0)+ISNULL(FIN_DISCOUNT,0)) )SAVALUE"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),BOARDRATE)STRATE"
            strSql += vbCrLf + " ,RATE SARATE"
            strSql += vbCrLf + " ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = I.EMPID)AS EMPNAME"
            strSql += vbCrLf + " ,(SELECT EMPNAME FROM " & cnAdminDb & "..EMPMASTER WHERE EMPID = I.DISC_EMPID)AS DISCEMPNAME"
            strSql += vbCrLf + " ,ITEMID,SUBITEMID"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0)STWASTPER,ISNULL(CONVERT(NUMERIC(15,2),(WASTAGE/CASE WHEN GRSWT <> 0  THEN GRSWT ELSE 1 END)*100),0) SAWASTPER"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),0)STWAST "
            If SPECIFICFORMAT = "1" Then
                strSql += vbCrLf + " ,CASE WHEN ISNULL((SELECT COUNT(*)CNT FROM " & cnStockDb & "..WT2WTAMTCAL WHERE ISSSNO =I.SNO ),0) > 0 THEN "
                strSql += vbCrLf + " ISNULL((SELECT SUM(WASTAGE)WASTAGE FROM " & cnStockDb & "..WT2WTAMTCAL WHERE ISSSNO =I.SNO),0) "
                strSql += vbCrLf + " ELSE WASTAGE "
                strSql += vbCrLf + " END SAWAST"
            Else
                strSql += vbCrLf + " ,WASTAGE SAWAST"
            End If
            strSql += vbCrLf + " ,TRANNO,TRANDATE"
            strSql += vbCrLf + " ,CASE WHEN SUBITEMID <> 0 THEN (SELECT CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID)"
            strSql += vbCrLf + " ELSE (SELECT CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = I.ITEMID) END AS CALTYPE"
            strSql += vbCrLf + " ,AMOUNT,BATCHNO,SNO AS ISSSNO"
            strSql += vbCrLf + " ,(CASE WHEN I.BATCHNO <> '' THEN (SELECT DISTINCT 'OD' FROM " & cnAdminDb & "..ORMAST WHERE ISNULL(ODBATCHNO,'') <> '' AND ORTYPE IN('O','B') AND ODBATCHNO=I.BATCHNO)"
            '     strSql += vbCrLf + " ELSE I.TRANTYPE END) TRANTYPE"
            strSql += vbCrLf + " ELSE I.TRANTYPE END) TRANTYPE"
            strSql += vbCrLf + " ,RUNNO"
            strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID=(SELECT DESIGNERID FROM " & cnAdminDb & "..ITEMTAG "
            strSql += vbCrLf + " WHERE ITEMID=I.ITEMID AND TAGNO=I.TAGNO))DESIGNER"
            strSql += vbCrLf + " ,CASE WHEN I.TAGNO<>'' THEN DATEDIFF(DAY,(SELECT ACTUALRECDATE FROM " & cnAdminDb & "..ITEMTAG  WHERE ITEMID=I.ITEMID AND TAGNO=I.TAGNO),I.TRANDATE)ELSE '' END AGE"
        End If
        strSql += vbCrLf + " FROM " & cnStockDb & ".." & tableName & " I"
        strSql += vbCrLf + " WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND TRANTYPE = 'SA'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = '' "
        strSql += vbCrLf + " AND COMPANYID IN (" & SelectedCompany & ")"
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkcategoryName <> "" Then strSql += vbCrLf + " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkcategoryName & "))"
        If rbtTag.Checked Then
            strSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'T')"
            If Not chkHomeSales.Checked Then
                strSql += vbCrLf + " AND TAGNO <> ''"
            End If
        ElseIf rbtNonTag.Checked Then
            strSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE = 'N')"
        Else
            strSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE STOCKTYPE IN ('T','N'))"
            If Not chkHomeSales.Checked Then
                strSql += vbCrLf + " AND TAGNO <> ''"
            End If
        End If
        If chkcmbCashCtr.Text <> "" And chkcmbCashCtr.Text <> "ALL" Then
            strSql += vbCrLf + " AND CASHID IN (SELECT CASHID FROM " & cnAdminDb & "..CASHCOUNTER WHERE CASHNAME IN ('" + chkcmbCashCtr.Text.Replace(", ", "','").Replace("','','", ",") + "'))"
        End If
        If chkcmbItemCtr.Text <> "" And chkcmbItemCtr.Text <> "ALL" Then
            strSql += vbCrLf + " AND ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN ('" + chkcmbItemCtr.Text.Replace(", ", "','").Replace("','','", ",") + "'))"
        End If
        strSql += vbCrLf + " ) x "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        If chkSpecificFormat.Checked = True Then
            GoTo SPECIFICFORMAT
        End If
        strSql = ""
        strSql += vbCrLf + " UPDATE A SET  A.TRANTYPE = 'SR' FROM TEMP" & systemId & "SALEDET AS A"
        strSql += vbCrLf + ", " & cnStockDb & "..RECEIPT AS B"
        strSql += vbCrLf + " WHERE A.BATCHNO = B.BATCHNO AND B.TRANTYPE='SR'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If SqlVersion > 8 Then
            strSql = vbCrLf + "DECLARE @BATCHNO VARCHAR(15)"
            strSql += vbCrLf + "DECLARE CUR CURSOR FOR SELECT  DISTINCT BATCHNO FROM TEMP" & systemId & "SALEDET"
            strSql += vbCrLf + "OPEN CUR "
            strSql += vbCrLf + "FETCH NEXT FROM CUR INTO @BATCHNO"
            strSql += vbCrLf + "WHILE @@FETCH_STATUS<>-1"
            strSql += vbCrLf + "BEGIN"
            strSql += vbCrLf + "	UPDATE TOP(1) TEMP" & systemId & "SALEDET SET "
            strSql += vbCrLf + "	DISCOUNT=ISNULL(DISCOUNT,0)+(SELECT ISNULL(SUM(AMOUNT),0) FROM " & cnStockDb & "..ACCTRAN "
            strSql += vbCrLf + "	WHERE BATCHNO=@BATCHNO AND PAYMODE='DI' AND TRANMODE='D')"
            strSql += vbCrLf + "	WHERE BATCHNO=@BATCHNO"
            strSql += vbCrLf + "	FETCH NEXT FROM CUR INTO @BATCHNO"
            strSql += vbCrLf + "END"
            strSql += vbCrLf + "CLOSE CUR "
            strSql += vbCrLf + "DEALLOCATE CUR"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = "UPDATE TEMP" & systemId & "SALEDET SET DISPER=(DISCOUNT*100)/(AMOUNT+DISCOUNT) WHERE DISCOUNT>0"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        strSql = " UPDATE TEMP" & systemId & "SALEDET SET STWASTAGE = TAG.STWASTAGE,STWAST = TAG.STWAST,STMC = TAG.STMC"
        'strSql += vbcrlf + " ,STVALUE = TAG.STVALUE"
        strSql += vbCrLf + " ,STRATE = TAG.STRATE"
        strSql += vbCrLf + " ,STWASTPER = TAG.STWASTPER"
        strSql += vbCrLf + " FROM TEMP" & systemId & "SALEDET AS T"
        strSql += vbCrLf + " INNER JOIN"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " TAG.TAGNO,TAG.ITEMID"
        strSql += vbCrLf + " ,ISNULL(REPLICATE(' ',5-LEN(ISNULL(MAXWASTPER,0))),'')+CONVERT(VARCHAR,ISNULL(MAXWASTPER,0))"
        strSql += vbCrLf + " AS STWASTAGE "
        strSql += vbCrLf + " ,MAXMC STMC ,"
        If chkBasedOnPartlyWeight.Checked Then
            strSql += vbCrLf + "  CASE WHEN T.SAGRSWT <> T.STGRSWT AND ISNULL(T.SAGRSWT,0)  <> 0 THEN "
            strSql += vbCrLf + "  ISNULL(CONVERT(NUMERIC(15,3),ISNULL(T.SAGRSWT,0) * (ISNULL(MAXWASTPER,0)/100)),0)"
            strSql += vbCrLf + "  ELSE"
            strSql += vbCrLf + "  MAXWAST "
            strSql += vbCrLf + "  END"
        Else
            strSql += vbCrLf + " MAXWAST "
        End If
        strSql += " AS STWAST "
        strSql += vbCrLf + " ,MAXWASTPER STWASTPER"
        'strSql += vbcrlf + " ,SALVALUE STVALUE"
        strSql += vbCrLf + " ,RATE STRATE"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG TAG INNER JOIN TEMP" & systemId & "SALEDET T ON TAG.ITEMID = T.ITEMID AND TAG.TAGNO = T.TAGNO"
        strSql += vbCrLf + " )TAG"
        strSql += vbCrLf + " ON TAG.ITEMID = T.ITEMID AND TAG.TAGNO = T.TAGNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        'strSql = " UPDATE TEMP" & systemId & "SALEDET SET STWASTAGE = TAG.STWASTAGE,STWAST = TAG.STWAST,STMC = TAG.STMC"
        'strSql += vbCrLf + " FROM TEMP" & systemId & "SALEDET AS T"
        'strSql += vbCrLf + " INNER JOIN"
        'strSql += vbCrLf + " ("
        'strSql += vbCrLf + " SELECT "
        'strSql += vbCrLf + " T.TAGNO,T.ITEMID"
        'strSql += vbCrLf + " ,ISNULL(REPLICATE(' ',5-LEN(ISNULL(MAXWASTPER,0))),'')+CONVERT(VARCHAR,ISNULL(MAXWASTPER,0))+'%'+' '+"
        'strSql += vbCrLf + "  ISNULL(REPLICATE(' ',7-LEN(ISNULL(MAXWAST,0))),'')+CONVERT(VARCHAR,ISNULL(MAXWAST,0)) AS STWASTAGE"
        'strSql += vbCrLf + " ,MAXMC STMC"
        'strSql += vbCrLf + " ,MAXWAST STWAST,ISSSNO"
        'strSql += vbCrLf + " FROM " & cnAdminDb & "..WMCTABLE AS W "
        'strSql += vbCrLf + " INNER JOIN TEMP" & systemId & "SALEDET T ON W.ITEMID = T.ITEMID AND ISNULL(T.TAGNO,'')='' "
        'strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST M ON M.ITEMID = T.ITEMID AND M.VALUEADDEDTYPE='I' AND M.STOCKTYPE='T' "
        'strSql += vbCrLf + " )TAG"
        'strSql += vbCrLf + " ON TAG.ITEMID = T.ITEMID AND TAG.ISSSNO = T.ISSSNO"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()

        strSql = " ALTER TABLE TEMP" & systemId & "SALEDET ADD SNO INT IDENTITY(1,1)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "WMC') > 0 DROP TABLE TEMP" & systemId & "WMC"
        strSql += vbCrLf + " SELECT SNO,SAGRSWT"
        If chkstnamt.Checked Then
            strSql += vbCrLf + ",SASTNAMT"
        End If
        strSql += vbCrLf + ",WASTPER,CONVERT(NUMERIC(15,3),CASE WHEN WASTPER = 0 THEN WASTAGE ELSE SAGRSWT*(WASTPER/100) END) AS WASTAGE,"
        strSql += vbCrLf + " CONVERT(NUMERIC(15,2),CASE WHEN MCGRM = 0 THEN MC ELSE SAGRSWT*MCGRM END) AS MC"
        strSql += vbCrLf + " INTO TEMP" & systemId & "WMC"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " 	SELECT "
        strSql += vbCrLf + " 		*"
        strSql += vbCrLf + " 		,CASE WHEN TYPE = 'T' THEN ISNULL((SELECT TOP 1 MAXWASTPER FROM " & cnAdminDb & "..WMCTABLE WHERE TABLECODE = X.TABLECODE AND X.SAGRSWT BETWEEN FROMWEIGHT AND TOWEIGHT),0)"
        strSql += vbCrLf + " 		ELSE "

        strSql += vbCrLf + " 		(CASE WHEN "
        strSql += vbCrLf + " 		ISNULL((SELECT TOP 1 MAXWASTPER FROM " & cnAdminDb & "..WMCTABLE WHERE ITEMID = X.ITEMID AND SUBITEMID = X.SUBITEMID AND X.SAGRSWT BETWEEN FROMWEIGHT AND TOWEIGHT),0) > 0"
        strSql += vbCrLf + " 		THEN "
        strSql += vbCrLf + " 		ISNULL((SELECT TOP 1 MAXWASTPER FROM " & cnAdminDb & "..WMCTABLE WHERE ITEMID = X.ITEMID AND SUBITEMID = X.SUBITEMID AND X.SAGRSWT BETWEEN FROMWEIGHT AND TOWEIGHT),0)"
        strSql += vbCrLf + " 		ELSE "
        strSql += vbCrLf + " 		ISNULL((SELECT TOP 1 MAXWASTPER FROM " & cnAdminDb & "..WMCTABLE WHERE ITEMID = X.ITEMID AND X.SAGRSWT BETWEEN FROMWEIGHT AND TOWEIGHT),0)"
        strSql += vbCrLf + " 		END)"

        strSql += vbCrLf + " 		END WASTPER"
        strSql += vbCrLf + " 		,CASE WHEN TYPE = 'T' THEN ISNULL((SELECT TOP 1  MAXWAST FROM " & cnAdminDb & "..WMCTABLE WHERE TABLECODE = X.TABLECODE AND X.SAGRSWT BETWEEN FROMWEIGHT AND TOWEIGHT),0)"
        strSql += vbCrLf + " 		ELSE "
        strSql += vbCrLf + " 		(CASE WHEN "
        strSql += vbCrLf + " 		ISNULL((SELECT TOP 1 MAXWAST FROM " & cnAdminDb & "..WMCTABLE WHERE ITEMID = X.ITEMID AND SUBITEMID = X.SUBITEMID AND X.SAGRSWT BETWEEN FROMWEIGHT AND TOWEIGHT),0) > 0"
        strSql += vbCrLf + " 		THEN "
        strSql += vbCrLf + " 		ISNULL((SELECT TOP 1 MAXWAST FROM " & cnAdminDb & "..WMCTABLE WHERE ITEMID = X.ITEMID AND SUBITEMID = X.SUBITEMID AND X.SAGRSWT BETWEEN FROMWEIGHT AND TOWEIGHT),0)"
        strSql += vbCrLf + " 		ELSE"
        strSql += vbCrLf + " 		ISNULL((SELECT TOP 1 MAXWAST FROM " & cnAdminDb & "..WMCTABLE WHERE ITEMID = X.ITEMID AND X.SAGRSWT BETWEEN FROMWEIGHT AND TOWEIGHT),0)"
        strSql += vbCrLf + " 		END)"
        strSql += vbCrLf + " 		END WASTAGE"

        strSql += vbCrLf + " 		,CASE WHEN TYPE = 'T' THEN ISNULL((SELECT TOP 1  MAXMCGRM FROM " & cnAdminDb & "..WMCTABLE WHERE TABLECODE = X.TABLECODE AND X.SAGRSWT BETWEEN FROMWEIGHT AND TOWEIGHT),0)"
        strSql += vbCrLf + " 		ELSE "
        strSql += vbCrLf + " 		(CASE WHEN "
        strSql += vbCrLf + " 		ISNULL((SELECT TOP 1 MAXMCGRM FROM " & cnAdminDb & "..WMCTABLE WHERE ITEMID = X.ITEMID AND SUBITEMID = X.SUBITEMID AND X.SAGRSWT BETWEEN FROMWEIGHT AND TOWEIGHT),0) > 0"
        strSql += vbCrLf + " 		THEN "
        strSql += vbCrLf + " 		ISNULL((SELECT TOP 1 MAXMCGRM FROM " & cnAdminDb & "..WMCTABLE WHERE ITEMID = X.ITEMID AND SUBITEMID = X.SUBITEMID AND X.SAGRSWT BETWEEN FROMWEIGHT AND TOWEIGHT),0)"
        strSql += vbCrLf + " 		ELSE "
        strSql += vbCrLf + " 		ISNULL((SELECT TOP 1 MAXMCGRM FROM " & cnAdminDb & "..WMCTABLE WHERE ITEMID = X.ITEMID AND X.SAGRSWT BETWEEN FROMWEIGHT AND TOWEIGHT),0)"
        strSql += vbCrLf + " 		END)"
        strSql += vbCrLf + " 		END MCGRM"

        strSql += vbCrLf + " 		,CASE WHEN TYPE = 'T' THEN ISNULL((SELECT TOP 1 MAXMC FROM " & cnAdminDb & "..WMCTABLE WHERE TABLECODE = X.TABLECODE AND X.SAGRSWT BETWEEN FROMWEIGHT AND TOWEIGHT),0)"
        strSql += vbCrLf + " 		ELSE "
        strSql += vbCrLf + " 		(CASE WHEN "
        strSql += vbCrLf + " 		ISNULL((SELECT TOP 1 MAXMC FROM " & cnAdminDb & "..WMCTABLE WHERE ITEMID = X.ITEMID AND SUBITEMID = X.SUBITEMID AND X.SAGRSWT BETWEEN FROMWEIGHT AND TOWEIGHT),0) > 0"
        strSql += vbCrLf + " 		THEN "
        strSql += vbCrLf + " 		ISNULL((SELECT TOP 1 MAXMC FROM " & cnAdminDb & "..WMCTABLE WHERE ITEMID = X.ITEMID AND SUBITEMID = X.SUBITEMID AND X.SAGRSWT BETWEEN FROMWEIGHT AND TOWEIGHT),0)"
        strSql += vbCrLf + " 		ELSE "
        strSql += vbCrLf + " 		ISNULL((SELECT TOP 1 MAXMC FROM " & cnAdminDb & "..WMCTABLE WHERE ITEMID = X.ITEMID AND X.SAGRSWT BETWEEN FROMWEIGHT AND TOWEIGHT),0)"
        strSql += vbCrLf + " 		END)"
        strSql += vbCrLf + " 		END MC"

        strSql += vbCrLf + " 		FROM"
        strSql += vbCrLf + " 		("
        strSql += vbCrLf + " 		SELECT T.SNO,T.ITEMID,T.SUBITEMID,I.VALUEADDEDTYPE TYPE,I.TABLECODE,T.SAGRSWT"
        If chkstnamt.Checked Then
            strSql += vbCrLf + " ,T.SASTNAMT"
        End If
        strSql += vbCrLf + " 		FROM " & cnAdminDb & "..ITEMMAST AS I INNER JOIN TEMP" & systemId & "SALEDET T ON I.ITEMID = T.ITEMID"
        strSql += vbCrLf + " 		WHERE T.TAGNO = ''"
        strSql += vbCrLf + " 	)X"
        strSql += vbCrLf + " )Y"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " UPDATE TEMP" & systemId & "SALEDET SET STWASTAGE = TAG.STWASTAGE,STWAST = TAG.STWAST,STMC = TAG.STMC"
        strSql += vbCrLf + " ,STWASTPER = TAG.STWASTPER"
        strSql += vbCrLf + " FROM TEMP" & systemId & "SALEDET AS T"
        strSql += vbCrLf + " INNER JOIN "
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT SNO"
        strSql += vbCrLf + " ,ISNULL(REPLICATE(' ',5-LEN(ISNULL(WASTPER,0))),'')+CONVERT(VARCHAR,ISNULL(WASTPER,0))"
        strSql += vbCrLf + "  AS STWASTAGE"
        strSql += vbCrLf + " ,WASTAGE STWAST"
        strSql += vbCrLf + " ,WASTPER STWASTPER"
        strSql += vbCrLf + " ,MC STMC"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " 	("
        strSql += vbCrLf + " 	SELECT * FROM TEMP" & systemId & "WMC"
        strSql += vbCrLf + " 	)Y"
        strSql += vbCrLf + " )TAG"
        strSql += vbCrLf + " ON T.SNO = TAG.SNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMP" & systemId & "SALEDET SET STWASTAGE1 = TAG.STWASTAGE1,STWAST = TAG.STWAST,STMC = TAG.STMC"
        strSql += vbCrLf + " ,STWASTPER = TAG.STWASTPER"
        strSql += vbCrLf + " FROM TEMP" & systemId & "SALEDET AS T"
        strSql += vbCrLf + " INNER JOIN "
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT SNO"
        strSql += vbCrLf + " ,ISNULL(REPLICATE(' ',7-LEN(ISNULL(WASTAGE,0))),'')+CONVERT(VARCHAR,ISNULL(WASTAGE,0)) AS STWASTAGE1"
        strSql += vbCrLf + " ,WASTAGE STWAST"
        strSql += vbCrLf + " ,WASTPER STWASTPER"
        strSql += vbCrLf + " ,MC STMC"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " 	("
        strSql += vbCrLf + " 	SELECT * FROM TEMP" & systemId & "WMC"
        strSql += vbCrLf + " 	)Y"
        strSql += vbCrLf + " )TAG"
        strSql += vbCrLf + " ON T.SNO = TAG.SNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " ALTER TABLE TEMP" & systemId & "SALEDET ADD TYPE VARCHAR(100)NULL "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        Dim DTTT As New DataTable
        strSql = "SELECT DISTINCT BATCHNO FROM TEMP" & systemId & "SALEDET"
        da = New OleDbDataAdapter(strSql, cn)
        DTTT = New DataTable
        da.Fill(DTTT)
        If DTTT.Rows.Count > 0 Then
            Dim TBL As String = "MASTER..TEMP" & systemId & "SALEDET"
            For I As Integer = 0 To DTTT.Rows.Count - 1
                strSql = "EXEC " & cnAdminDb & "..[SP_RPT_SALRPTWASTMCPAYMODE]"
                strSql += vbCrLf + " @DBNAME='" & cnStockDb & "',@BATCHNO='" & DTTT.Rows(I).Item("BATCHNO").ToString & "'"
                strSql += vbCrLf + " ,@TBL='" & TBL & "'"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            Next
        End If

        ' strSql = " UPDATE TT SET TYPE=T.PAYMODE  FROM " & cnStockDb & " ..ACCTRAN T,TEMP" & systemId & "SALEDET TT WHERE T.BATCHNO =TT.BATCHNO  AND T.TRANMODE ='D'"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()
        strSql = " ALTER TABLE TEMP" & systemId & "SALEDET ADD DIWASTTOT NUMERIC(15,2) NULL "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMP" & systemId & "SALEDET SET DIWASTTOT= CONVERT(NUMERIC(15,2),STWASTPER-SAWASTPER)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "SALEDETRES')>0 DROP TABLE TEMP" & systemId & "SALEDETRES"
        strSql += vbCrLf + " SELECT CONVERT(VARCHAR(200),[DESCRIPTION]) AS PARTICULAR,TRANNO,TRANDATE,TAGNO,DESIGNER,STGRSWT,SAGRSWT,"
        If SPECIFICFORMAT = "1" Then
            strSql += vbCrLf + " CONVERT(NUMERIC(15,3),SAEXTRAWT)SAEXTRAWT, "
        End If
        strSql += vbCrLf + "  CONVERT(NUMERIC(15,3),STGRSWT-SAGRSWT) DIGRSWT"
        If chkstnamt.Checked Then strSql += vbCrLf + "  ,STSTNAMT,SASTNAMT,CONVERT(NUMERIC(15,3),STSTNAMT-SASTNAMT) DISTNAMT "
        'strSql += vbCrLf + " ,STWASTAGE,SAWASTAGE,CONVERT(NUMERIC(15,3),STWAST-SAWAST) DIWAST"
        strSql += vbCrLf + " ,STWASTAGE,CONVERT(NUMERIC(15,3),STWAST) STWASTAGE1,CONVERT(NUMERIC(15,2),SAWASTAGE)SAWASTAGE,CONVERT(NUMERIC(15,3),SAWAST) SAWASTAGE1 "
        strSql += vbCrLf + " ,ISNULL(REPLICATE(' ',5-LEN(ISNULL(DIWASTTOT,0))),'')+CONVERT(VARCHAR,ISNULL(DIWASTTOT,0)) AS DIWAST"
        strSql += vbCrLf + " ,ISNULL(REPLICATE(' ',7-LEN(ISNULL(STWAST-SAWAST,0))),'')+CONVERT(VARCHAR,ISNULL(STWAST-SAWAST,0)) AS DIWAST1"
        strSql += vbCrLf + " ,STMC,SAMC,CONVERT(NUMERIC(15,2),STMC-SAMC) AS DIMC"
        strSql += vbCrLf + " ,DISCOUNT,DISPER"
        If FORMAT2 Then strSql += vbCrLf + " ,ISNULL((CASE WHEN CHARINDEX('%',SAWASTAGE) > 0 THEN SUBSTRING(SAWASTAGE,1,CHARINDEX('%',SAWASTAGE)-1) END),0)-ISNULL(DISPER,0) AS DISCVA "
        strSql += vbCrLf + " ,DISCEMPNAME"
        strSql += vbCrLf + " ,STRATE,SARATE,CONVERT(NUMERIC(15,2),STRATE-SARATE) AS DIRATE"
        strSql += vbCrLf + " ,STVALUE,SAVALUE,CONVERT(NUMERIC(15,2),STVALUE-SAVALUE) AS DIVALUE"
        strSql += vbCrLf + " ,EMPNAME,ITEMID,SUBITEMID,TRANTYPE,1 RESULT,' 'COLHEAD,STWAST,SAWAST,CALTYPE,TYPE,RUNNO,AGE "
        strSql += vbCrLf + " INTO TEMP" & systemId & "SALEDETRES"
        strSql += vbCrLf + " FROM TEMP" & systemId & "SALEDET"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        'strSql = " ALTER TABLE TEMP" & systemId & "SALEDETRES ALTER COLUMN TYPE VARCHAR(100)NULL "
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()

        'strSql = " UPDATE TEMP" & systemId & "SALEDETRES SET STRATE = 0,SARATE = 0,DIRATE = 0 WHERE CALTYPE <> 'R'"
        'strSql = ""
        'strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET STVALUE = 0,SAVALUE = 0,DIVALUE = 0 WHERE CALTYPE <> 'M'"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()

        If ChkGrpbyEmp.Checked Then
            strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "SALEDETRES)>0"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SALEDETRES(EMPNAME,PARTICULAR,RESULT,COLHEAD)"
            strSql += vbCrLf + " SELECT DISTINCT EMPNAME,EMPNAME,0 RESULT,'T'COLHEAD FROM TEMP" & systemId & "SALEDETRES"
            strSql += vbCrLf + " "
            strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SALEDETRES(EMPNAME,PARTICULAR"
            strSql += vbCrLf + " ,STGRSWT,SAGRSWT,DIGRSWT"
            If SPECIFICFORMAT = "1" Then
                strSql += vbCrLf + " ,SAEXTRAWT "
            End If
            If chkstnamt.Checked Then strSql += vbCrLf + " ,STSTNAMT,SASTNAMT,DISTNAMT"
            strSql += vbCrLf + " ,STWASTAGE1,SAWASTAGE1,DIWAST1"
            strSql += vbCrLf + " ,STMC,SAMC,DIMC"
            strSql += vbCrLf + " ,DISCOUNT,DISPER,STRATE,SARATE,DIRATE"
            If FORMAT2 Then strSql += vbCrLf + " ,DISCVA"
            strSql += vbCrLf + " ,STVALUE,SAVALUE,DIVALUE,RESULT,COLHEAD)"
            strSql += vbCrLf + " SELECT EMPNAME,'TOTAL NO(S) :' + LTRIM(STR(COUNT(TAGNO)))"
            strSql += vbCrLf + " ,SUM(STGRSWT),SUM(SAGRSWT),SUM(DIGRSWT)"
            If SPECIFICFORMAT = "1" Then
                strSql += vbCrLf + " ,SUM(SAEXTRAWT)SAEXTRAWT "
            End If
            If chkstnamt.Checked Then strSql += vbCrLf + " ,SUM(STSTNAMT),SUM(SASTNAMT),SUM(DISTNAMT)"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),SUM(STWAST)),CONVERT(NUMERIC(15,3),SUM(SAWAST))/*,CONVERT(NUMERIC(15,3),SUM(DIWAST))*/"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),SUM(CONVERT(NUMERIC(15,3),STWAST))- SUM(CONVERT(NUMERIC(15,3),SAWAST)))/*,CONVERT(NUMERIC(15,3),SUM(STWAST)- SUM(SAWAST))*/"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SUM(STMC)),CONVERT(NUMERIC(15,2),SUM(SAMC)),CONVERT(NUMERIC(15,2),SUM(DIMC))"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SUM(DISCOUNT)),CONVERT(NUMERIC(15,2),AVG(DISPER)),CONVERT(NUMERIC(15,2),SUM(STRATE)),CONVERT(NUMERIC(15,2),SUM(SARATE)),CONVERT(NUMERIC(15,2),SUM(DIRATE))"
            If FORMAT2 Then strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),AVG(DISCVA))"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SUM(STVALUE)),CONVERT(NUMERIC(15,2),SUM(SAVALUE)),CONVERT(NUMERIC(15,2),SUM(DIVALUE)),2 RESULT,'S'COLHEAD"
            strSql += vbCrLf + " FROM TEMP" & systemId & "SALEDETRES"
            strSql += vbCrLf + " GROUP BY EMPNAME"
            strSql += vbCrLf + " END"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "SALEDETRES)>0"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SALEDETRES(EMPNAME,PARTICULAR"
            strSql += vbCrLf + " ,STGRSWT,SAGRSWT,DIGRSWT"
            If SPECIFICFORMAT = "1" Then
                strSql += vbCrLf + " ,SAEXTRAWT "
            End If
            If chkstnamt.Checked Then strSql += vbCrLf + " ,STSTNAMT,SASTNAMT,DISTNAMT"
            strSql += vbCrLf + " ,STWASTAGE1,SAWASTAGE1,DIWAST1"
            strSql += vbCrLf + " ,STMC,SAMC,DIMC"
            strSql += vbCrLf + " ,DISCOUNT,DISPER,STRATE,SARATE,DIRATE"
            If FORMAT2 Then strSql += vbCrLf + " ,DISCVA"
            strSql += vbCrLf + " ,STVALUE,SAVALUE,DIVALUE,RESULT,COLHEAD,RUNNO)"
            strSql += vbCrLf + " SELECT 'ZZZZZ'EMPNAME,'GRAND NO(S) :' + LTRIM(STR(COUNT(TAGNO)))"
            strSql += vbCrLf + " ,SUM(STGRSWT),SUM(SAGRSWT),SUM(DIGRSWT)"
            If SPECIFICFORMAT = "1" Then
                strSql += vbCrLf + " ,SUM(SAEXTRAWT)SAEXTRAWT "
            End If
            If chkstnamt.Checked Then strSql += vbCrLf + " ,SUM(STSTNAMT),SUM(SASTNAMT),SUM(DISTNAMT)"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),SUM(STWAST)),CONVERT(NUMERIC(15,3),SUM(SAWAST))/*,CONVERT(NUMERIC(15,3),SUM(DIWAST))*/"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),SUM(STWAST)- SUM(SAWAST))"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SUM(STMC)),CONVERT(NUMERIC(15,2),SUM(SAMC)),CONVERT(NUMERIC(15,2),SUM(DIMC))"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SUM(DISCOUNT)),CONVERT(NUMERIC(15,2),AVG(DISPER)),CONVERT(NUMERIC(15,2),SUM(STRATE)),CONVERT(NUMERIC(15,2),SUM(SARATE)),CONVERT(NUMERIC(15,2),SUM(DIRATE))"
            If FORMAT2 Then strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),AVG(DISCVA))"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SUM(STVALUE)),CONVERT(NUMERIC(15,2),SUM(SAVALUE)),CONVERT(NUMERIC(15,2),SUM(DIVALUE)),3 RESULT,'G'COLHEAD,'' RUNNO"
            strSql += vbCrLf + " FROM TEMP" & systemId & "SALEDETRES WHERE RESULT <> 2"
            strSql += vbCrLf + " END"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        Else
            strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "SALEDETRES)>0"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "SALEDETRES(EMPNAME,PARTICULAR"
            strSql += vbCrLf + " ,STGRSWT,SAGRSWT,DIGRSWT"
            If SPECIFICFORMAT = "1" Then
                strSql += vbCrLf + " ,SAEXTRAWT "
            End If
            If chkstnamt.Checked Then strSql += vbCrLf + " ,STSTNAMT,SASTNAMT,DISTNAMT"
            strSql += vbCrLf + " ,STWASTAGE1,SAWASTAGE1,DIWAST1"
            strSql += vbCrLf + " ,STMC,SAMC,DIMC"
            strSql += vbCrLf + " ,DISCOUNT,DISPER,STRATE,SARATE,DIRATE"
            If FORMAT2 Then strSql += vbCrLf + " ,DISCVA"
            strSql += vbCrLf + " ,STVALUE,SAVALUE,DIVALUE,RESULT,COLHEAD,RUNNO)"
            strSql += vbCrLf + " SELECT 'ZZZZZ'EMPNAME,'GRAND NO(S) :' + LTRIM(STR(COUNT(TAGNO)))"
            strSql += vbCrLf + " ,SUM(STGRSWT),SUM(SAGRSWT),SUM(DIGRSWT)"
            If SPECIFICFORMAT = "1" Then
                strSql += vbCrLf + " ,SUM(SAEXTRAWT)SAEXTRAWT "
            End If
            If chkstnamt.Checked Then strSql += vbCrLf + " ,SUM(STSTNAMT),SUM(SASTNAMT),SUM(DISTNAMT)"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),SUM(STWAST)),CONVERT(NUMERIC(15,3),SUM(SAWAST))/*,CONVERT(NUMERIC(15,3),SUM(DIWAST))*/"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,3),SUM(STWAST)- SUM(SAWAST))"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SUM(STMC)),CONVERT(NUMERIC(15,2),SUM(SAMC)),CONVERT(NUMERIC(15,2),SUM(DIMC))"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SUM(DISCOUNT)),CONVERT(NUMERIC(15,2),AVG(DISPER)),CONVERT(NUMERIC(15,2),SUM(STRATE)),CONVERT(NUMERIC(15,2),SUM(SARATE)),CONVERT(NUMERIC(15,2),SUM(DIRATE))"
            If FORMAT2 Then strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),AVG(DISCVA))"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SUM(STVALUE)),CONVERT(NUMERIC(15,2),SUM(SAVALUE)),CONVERT(NUMERIC(15,2),SUM(DIVALUE)),3 RESULT,'G'COLHEAD,''RUNNO"
            strSql += vbCrLf + " FROM TEMP" & systemId & "SALEDETRES  "
            strSql += vbCrLf + " END"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If

        strSql = " UPDATE TEMP" & systemId & "SALEDETRES SET STGRSWT = NULL WHERE CONVERT(NUMERIC(15,2),STGRSWT) = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET SAGRSWT = NULL WHERE CONVERT(NUMERIC(15,2),SAGRSWT) = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET DIGRSWT = NULL WHERE CONVERT(NUMERIC(15,2),DIGRSWT) = 0"
        If SPECIFICFORMAT = "1" Then
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET SAEXTRAWT = NULL WHERE CONVERT(NUMERIC(15,3),SAEXTRAWT) = 0"
        End If
        If chkstnamt.Checked Then
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET STSTNAMT = NULL WHERE CONVERT(NUMERIC(15,2),STSTNAMT) = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET SASTNAMT = NULL WHERE CONVERT(NUMERIC(15,2),SASTNAMT) = 0"
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET DISTNAMT = NULL WHERE CONVERT(NUMERIC(15,2),DISTNAMT) = 0"
        End If
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET STWASTAGE = NULL WHERE CONVERT(NUMERIC(15,2),STWAST) = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET STWASTAGE1 = NULL WHERE CONVERT(NUMERIC(15,2),STWAST) = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET SAWASTAGE = NULL WHERE CONVERT(NUMERIC(15,2),SAWAST) = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET STMC = NULL WHERE CONVERT(NUMERIC(15,2),STMC) = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET SAMC = NULL WHERE CONVERT(NUMERIC(15,2),SAMC) = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET DIMC = NULL WHERE CONVERT(NUMERIC(15,2),DIMC) = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET DISCOUNT = NULL WHERE CONVERT(NUMERIC(15,2),DISCOUNT) = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET DISPER = NULL WHERE CONVERT(NUMERIC(15,2),DISPER) = 0"
        'strSql += vbcrlf + " UPDATE TEMP" & systemId & "SALEDETRES SET STRATE = NULL,SARATE = NULL WHERE CALTYPE <> 'R'"
        'strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET STVALUE = NULL,SAVALUE = NULL WHERE CALTYPE <> 'M'"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET DIRATE = NULL WHERE CONVERT(NUMERIC(15,2),DIRATE) = 0"
        'strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET DIWAST = NULL WHERE CONVERT(NUMERIC(15,2),DIWAST) = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET DIWAST = NULL WHERE CONVERT(NUMERIC(15,2),STWAST) = 0 AND CONVERT(NUMERIC(15,2),SAWAST) = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET DIVALUE = NULL WHERE CONVERT(NUMERIC(15,2),DIVALUE) = 0"
        If FORMAT2 Then strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET DISCVA = NULL WHERE CONVERT(NUMERIC(15,2),DISCVA) = 0"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET PARTICULAR = ' ' + PARTICULAR WHERE RESULT = 1"
        strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET COLHEAD='N' WHERE  ISNULL(RUNNO,'')<>''"
        If ChkHighLight.Checked Then
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET COLHEAD='H' WHERE TYPE LIKE '%CHIT CARD%'"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If chkWithDiffRate.Checked Then
            strSql = ""
            strSql += vbCrLf + " ALTER TABLE TEMP" & systemId & "SALEDETRES ADD DIFFRATE NUMERIC(15,2)"
            strSql += vbCrLf + " ALTER TABLE TEMP" & systemId & "SALEDETRES ADD RATEPER NUMERIC(15,2)"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = ""
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET DIFFRATE=CONVERT(NUMERIC(15,2),((SAVALUE/SAGRSWT)-SARATE)) WHERE RESULT=1 AND CALTYPE='W'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = ""
            strSql += vbCrLf + " UPDATE TEMP" & systemId & "SALEDETRES SET RATEPER=CONVERT(NUMERIC(15,2),CASE WHEN ISNULL(DIFFRATE,0) <> 0 THEN ((DIFFRATE/SARATE)*100) ELSE NULL END) "
            strSql += vbCrLf + " WHERE RESULT=1 And CALTYPE='W'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
SPECIFICFORMAT:
        strSql = " SELECT * FROM TEMP" & systemId & "SALEDETRES "
        If ChkGrpbyEmp.Checked Then
            If Val(txtWper.Text) > 0 Then strSql += vbCrLf + "WHERE (DIWAST/STGRSWT)*100 >=" & Val(txtWper.Text)
        ElseIf rbtTranNo.Checked Then
            If Val(txtWper.Text) > 0 Then strSql += vbCrLf + "WHERE (DIWAST/STGRSWT)*100 >=" & Val(txtWper.Text)
        ElseIf rbtItemId.Checked Then
            If Val(txtWper.Text) > 0 Then strSql += vbCrLf + "WHERE (DIWAST/STGRSWT)*100 >=" & Val(txtWper.Text)
        End If
        strSql += vbCrLf + " ORDER BY EMPNAME,RESULT,TRANDATE,TRANNO"
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            Exit Sub
        End If
        objGridShower = New frmGridDispDia
        objGridShower.gridView.DataSource = Nothing
        AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Size = New Size(778, 550)
        Dim tit As String = "SALES ITEM WASTAGE & MC ANALYSIS" & IIf(Val(txtWper.Text) > 0, txtWper.Text & "% & Above", "")
        Dim Cname As String = ""
        If chkLstCostCentre.Items.Count > 0 And chkLstCostCentre.CheckedItems.Count > 0 Then
            For CNT As Integer = 0 To chkLstCostCentre.Items.Count - 1
                If chkLstCostCentre.GetItemChecked(CNT) = True Then
                    Cname += "" & chkLstCostCentre.Items(CNT) + ","
                End If
            Next
            If Cname <> "" Then Cname = " :" + Mid(Cname, 1, Len(Cname) - 1)
        End If
        objGridShower.Text = tit
        objGridShower.lblTitle.Text = tit + Cname
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.WindowState = FormWindowState.Maximized
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)


        If chkSpecificFormat.Checked = True Then
            FormatGridColumns(objGridShower.gridView, False, False, False, False)
            For i As Integer = 0 To objGridShower.gridView.Columns.Count - 1
                If objGridShower.gridView.Columns.Contains("KEYNO") Then objGridShower.gridView.Columns("KEYNO").Visible = False
            Next
            objGridShower.Show()
            objGridShower.FormReSize = True
            objGridShower.gridViewHeader.Visible = True
            'GridViewHeaderCreator(objGridShower.gridViewHeader)
            FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        Else
            DataGridView_SummaryFormatting(objGridShower.gridView)
            FormatGridColumns(objGridShower.gridView, False, False, , False)
            objGridShower.Show()
            objGridShower.FormReSize = True
            objGridShower.gridViewHeader.Visible = True
            GridViewHeaderCreator(objGridShower.gridViewHeader)
            FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
            With objGridShower.gridView
                .Columns("DIWAST").DefaultCellStyle.BackColor = Color.LavenderBlush
                .Columns("DIRATE").DefaultCellStyle.BackColor = Color.LavenderBlush
                .Columns("DIGRSWT").DefaultCellStyle.BackColor = Color.LavenderBlush
                .Columns("DIVALUE").DefaultCellStyle.BackColor = Color.LavenderBlush
                .Columns("DIMC").DefaultCellStyle.BackColor = Color.LavenderBlush

                If chkWithDiffRate.Checked Then
                    .Columns("DIFFRATE").Visible = True
                    .Columns("RATEPER").Visible = True
                End If

                For cnt As Integer = 0 To .Rows.Count - 1
                    If .Rows(cnt).Cells("TRANTYPE").Value.ToString = "OD" Then
                        If Val(.Rows(cnt).Cells("STRATE").Value.ToString) <> Val(.Rows(cnt).Cells("SARATE").Value.ToString) Then
                            .Rows(cnt).Cells("STRATE").Style.BackColor = Color.LightGray
                            .Rows(cnt).Cells("SARATE").Style.BackColor = Color.LightGray
                        End If
                    ElseIf Val(.Rows(cnt).Cells("STRATE").Value.ToString) <> Val(.Rows(cnt).Cells("SARATE").Value.ToString) And Val(.Rows(cnt).Cells("RESULT").Value.ToString) <> 3 Then
                        .Rows(cnt).Cells("STRATE").Style.BackColor = Color.LightGreen
                        .Rows(cnt).Cells("SARATE").Style.BackColor = Color.LightGreen
                    End If
                Next
                For cnt As Integer = 0 To .Rows.Count - 1
                    If .Rows(cnt).Cells("TRANTYPE").Value.ToString = "SR" Then
                        .Rows(cnt).DefaultCellStyle.BackColor = Color.LightPink
                    End If
                Next
            End With
        End If

        For i As Integer = 0 To ChklstboxInclude.Items.Count - 1
            If ChklstboxInclude.Items.Item(i).ToString = "DESIGNER" And ChklstboxInclude.GetItemChecked(i) = True Then
                If objGridShower.gridView.Columns.Contains("DESIGNER") Then objGridShower.gridView.Columns("DESIGNER").Visible = True
                Continue For
            ElseIf ChklstboxInclude.Items.Item(i).ToString = "DESIGNER" Then
                If objGridShower.gridView.Columns.Contains("DESIGNER") Then objGridShower.gridView.Columns("DESIGNER").Visible = False
                Continue For
            End If
            If ChklstboxInclude.Items.Item(i).ToString = "STOCK WASTAGE" And ChklstboxInclude.GetItemChecked(i) = True Then
                If objGridShower.gridView.Columns.Contains("STWASTAGE1") Then objGridShower.gridView.Columns("STWASTAGE1").Visible = True
                Continue For
            ElseIf ChklstboxInclude.Items.Item(i).ToString = "STOCK WASTAGE" Then
                If objGridShower.gridView.Columns.Contains("STWASTAGE1") Then objGridShower.gridView.Columns("STWASTAGE1").Visible = False
                Continue For
            End If
            If ChklstboxInclude.Items.Item(i).ToString = "SALES WASTAGE" And ChklstboxInclude.GetItemChecked(i) = True Then
                If objGridShower.gridView.Columns.Contains("SAWASTAGE1") Then objGridShower.gridView.Columns("SAWASTAGE1").Visible = True
                Continue For
            ElseIf ChklstboxInclude.Items.Item(i).ToString = "SALES WASTAGE" Then
                If objGridShower.gridView.Columns.Contains("SAWASTAGE1") Then objGridShower.gridView.Columns("SAWASTAGE1").Visible = False
                Continue For
            End If
            If ChklstboxInclude.Items.Item(i).ToString = "DIFF WASTAGE" And ChklstboxInclude.GetItemChecked(i) = True Then
                If objGridShower.gridView.Columns.Contains("DIWAST1") Then objGridShower.gridView.Columns("DIWAST1").Visible = True
                Continue For
            ElseIf ChklstboxInclude.Items.Item(i).ToString = "DIFF WASTAGE" Then
                If objGridShower.gridView.Columns.Contains("DIWAST1") Then objGridShower.gridView.Columns("DIWAST1").Visible = False
                Continue For
            End If
        Next
        objGridShower.AutoResize()
        Prop_Sets()
    End Sub
    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            Dim Upbound As Integer = 0
            If chkstnamt.Checked Then Upbound = 25 Else Upbound = 22

            For cnt As Integer = Upbound To dgv.Columns.Count - 1
                dgv.Columns(cnt).Visible = False
            Next

            If ChkGrpbyEmp.Checked = False Then
                .Columns("EMPNAME").Visible = True
            End If
            .Columns("PARTICULAR").Width = 200
            .Columns("TRANNO").Width = 60
            .Columns("TRANDATE").Width = 80
            .Columns("TAGNO").Width = 70
            .Columns("DESIGNER").Width = 70
            .Columns("STGRSWT").Width = 80
            .Columns("SAGRSWT").Width = 80
            .Columns("DIGRSWT").Width = 80
            If SPECIFICFORMAT = "1" Then
                .Columns("SAEXTRAWT").Width = 80
                .Columns("SAEXTRAWT").HeaderText = "EXCESS"
            End If
            If chkstnamt.Checked Then
                .Columns("STSTNAMT").Width = 80
                .Columns("SASTNAMT").Width = 80
                .Columns("DISTNAMT").Width = 80
                .Columns("STSTNAMT").DefaultCellStyle.BackColor = Color.AliceBlue
                .Columns("SASTNAMT").DefaultCellStyle.BackColor = Color.AliceBlue
                .Columns("DISTNAMT").DefaultCellStyle.BackColor = Color.AliceBlue
            End If
            .Columns("STGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("SAGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("DIGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue

            .Columns("STWASTAGE").Width = 100
            .Columns("STWASTAGE1").Width = 100
            .Columns("SAWASTAGE").Width = 100
            .Columns("SAWASTAGE1").Width = 100
            .Columns("DIWAST").Width = 100
            .Columns("DIWAST1").Width = 100


            .Columns("STMC").Width = 100
            .Columns("SAMC").Width = 100
            .Columns("DIMC").Width = 100

            .Columns("STMC").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("SAMC").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("DIMC").DefaultCellStyle.BackColor = Color.AliceBlue

            .Columns("DISCOUNT").Width = 80

            .Columns("STRATE").Width = 80
            .Columns("SARATE").Width = 80
            .Columns("DIRATE").Width = 80

            .Columns("STVALUE").Width = 80
            .Columns("SAVALUE").Width = 80
            .Columns("DIVALUE").Width = 80


            .Columns("STVALUE").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("SAVALUE").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("DIVALUE").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("DISCOUNT").DefaultCellStyle.BackColor = Color.LightYellow

            .Columns("STGRSWT").HeaderText = "STOCK"
            .Columns("SAGRSWT").HeaderText = "SALE"
            .Columns("DIGRSWT").HeaderText = "DIFF"
            If chkstnamt.Checked Then
                .Columns("STSTNAMT").HeaderText = "STOCK"
                .Columns("SASTNAMT").HeaderText = "SALE"
                .Columns("DISTNAMT").HeaderText = "DIFF"
            End If
            If .Columns.Contains("DISCVA") Then .Columns("DISCVA").HeaderText = "VA-DISC"
            .Columns("STWASTAGE").HeaderText = "STKW%"
            .Columns("STWASTAGE1").HeaderText = "STKWT"
            .Columns("SAWASTAGE").HeaderText = "SAW%"
            .Columns("SAWASTAGE1").HeaderText = "SAWT"
            .Columns("DIWAST").HeaderText = "DIF%"
            .Columns("DIWAST1").HeaderText = "DIFWT"

            .Columns("STMC").HeaderText = "STOCK"
            .Columns("SAMC").HeaderText = "SALE"
            .Columns("DIMC").HeaderText = "DIFF"

            .Columns("STRATE").HeaderText = "STOCK"
            .Columns("SARATE").HeaderText = "SALE"
            .Columns("DIRATE").HeaderText = "DIFF"

            .Columns("STVALUE").HeaderText = "STOCK"
            .Columns("SAVALUE").HeaderText = "SALE"
            .Columns("DIVALUE").HeaderText = "DIFF"
            .Columns("DISCEMPNAME").HeaderText = "AUTHORIZE"


            .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SAGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIGRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            If chkstnamt.Checked Then
                .Columns("STSTNAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("SASTNAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("DISTNAMT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End If
            .Columns("STWASTAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STWASTAGE1").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SAWASTAGE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SAWASTAGE1").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIWAST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIWAST1").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STMC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SAMC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIMC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DISCOUNT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SARATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIRATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("STVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("SAVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DIVALUE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns("TRANDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            dgv.Columns("TYPE").Visible = True
            dgv.Columns("AGE").Visible = True
        End With
    End Sub
    Private Sub GridViewHeaderCreator(ByVal gridviewHead As DataGridView)
        Dim dtHead As New DataTable
        strSql = "SELECT ''[PARTICULAR~TRANNO~TRANDATE~TAGNO~DESIGNER]"
        If SPECIFICFORMAT = "1" Then
            strSql += " ,''[STGRSWT~SAGRSWT~SAEXTRAWT~DIGRSWT]"
        Else
            strSql += " ,''[STGRSWT~SAGRSWT~DIGRSWT]"
        End If


        If chkstnamt.Checked Then strSql += " ,''[STSTNAMT~SASTNAMT~DISTNAMT]"

        strSql += " ,''[STWASTAGE~STWASTAGE1~SAWASTAGE~SAWASTAGE1~DIWAST~DIWAST1]"
        strSql += " ,''[STMC~SAMC~DIMC]"
        If FORMAT2 Then
            strSql += " ,''[DISPER~DISCOUNT~DISCEMPNAME~DISCVA]"
        Else
            strSql += " ,''[DISPER~DISCOUNT~DISCEMPNAME]"
        End If
        strSql += " ,''[STRATE~SARATE~DIRATE]"
        strSql += " ,''[STVALUE~SAVALUE~DIVALUE]"
        If ChkGrpbyEmp.Checked = False Then
            strSql += " ,''[EMPNAME]"
        End If
        If chkWithDiffRate.Checked Then
            strSql += " ,''[TYPE~DIFFRATE~RATEPER~AGE]"
        Else
            strSql += " ,''[TYPE~AGE]"
        End If
        strSql += " ,''SCROLL"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtHead)
        gridviewHead.DataSource = dtHead
        gridviewHead.Columns("PARTICULAR~TRANNO~TRANDATE~TAGNO~DESIGNER").HeaderText = ""
        If SPECIFICFORMAT = "1" Then
            gridviewHead.Columns("STGRSWT~SAGRSWT~SAEXTRAWT~DIGRSWT").HeaderText = "WEIGHT"
        Else
            gridviewHead.Columns("STGRSWT~SAGRSWT~DIGRSWT").HeaderText = "WEIGHT"
        End If


        If chkstnamt.Checked Then gridviewHead.Columns("STSTNAMT~SASTNAMT~DISTNAMT").HeaderText = "STONE"

        gridviewHead.Columns("STWASTAGE~STWASTAGE1~SAWASTAGE~SAWASTAGE1~DIWAST~DIWAST1").HeaderText = "WASTAGE"
        gridviewHead.Columns("STMC~SAMC~DIMC").HeaderText = "MC"
        If FORMAT2 Then
            gridviewHead.Columns("DISPER~DISCOUNT~DISCEMPNAME~DISCVA").HeaderText = ""
        Else
            gridviewHead.Columns("DISPER~DISCOUNT~DISCEMPNAME").HeaderText = ""
        End If
        gridviewHead.Columns("STRATE~SARATE~DIRATE").HeaderText = "RATE"
        gridviewHead.Columns("STVALUE~SAVALUE~DIVALUE").HeaderText = "VALUE"
        If ChkGrpbyEmp.Checked = False Then
            gridviewHead.Columns("EMPNAME").HeaderText = ""
        End If
        If chkWithDiffRate.Checked Then
            gridviewHead.Columns("TYPE~DIFFRATE~RATEPER~AGE").HeaderText = ""
        Else
            gridviewHead.Columns("TYPE~AGE").HeaderText = ""
        End If

        gridviewHead.Columns("SCROLL").HeaderText = ""
        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        SetGridHeadColWid(gridviewHead)
    End Sub

    Private Sub SetGridHeadColWid(ByVal gridViewHeader As DataGridView)
        Dim f As frmGridDispDia
        f = objGPack.GetParentControl(gridViewHeader)
        If Not f.gridViewHeader.Visible Then Exit Sub
        If f.gridViewHeader Is Nothing Then Exit Sub
        If Not f.gridView.ColumnCount > 0 Then Exit Sub
        If Not f.gridViewHeader.ColumnCount > 0 Then Exit Sub
        With f.gridViewHeader
            .Columns("PARTICULAR~TRANNO~TRANDATE~TAGNO~DESIGNER").Width = f.gridView.Columns("PARTICULAR").Width _
                                                    + f.gridView.Columns("TRANNO").Width _
                                                    + f.gridView.Columns("TRANDATE").Width _
                                                    + f.gridView.Columns("TAGNO").Width _
                                                    + f.gridView.Columns("DESIGNER").Width
            If SPECIFICFORMAT = "1" Then
                .Columns("STGRSWT~SAGRSWT~SAEXTRAWT~DIGRSWT").Width = f.gridView.Columns("STGRSWT").Width _
                                                    + f.gridView.Columns("SAGRSWT").Width _
                                                    + f.gridView.Columns("SAEXTRAWT").Width _
                                                    + f.gridView.Columns("DIGRSWT").Width
            Else
                .Columns("STGRSWT~SAGRSWT~DIGRSWT").Width = f.gridView.Columns("STGRSWT").Width _
                                                    + f.gridView.Columns("SAGRSWT").Width _
                                                    + f.gridView.Columns("DIGRSWT").Width
            End If


            If chkstnamt.Checked Then
                .Columns("STSTNAMT~SASTNAMT~DISTNAMT").Width = f.gridView.Columns("STSTNAMT").Width _
                                                        + f.gridView.Columns("SASTNAMT").Width _
                                                        + f.gridView.Columns("DISTNAMT").Width

            End If
            .Columns("STWASTAGE~STWASTAGE1~SAWASTAGE~SAWASTAGE1~DIWAST~DIWAST1").Width = f.gridView.Columns("STWASTAGE").Width _
                                                    + f.gridView.Columns("STWASTAGE1").Width _
                                                    + f.gridView.Columns("SAWASTAGE").Width _
                                                    + f.gridView.Columns("SAWASTAGE1").Width _
                                                    + f.gridView.Columns("DIWAST").Width _
                                                    + f.gridView.Columns("DIWAST").Width
            .Columns("STMC~SAMC~DIMC").Width = f.gridView.Columns("STMC").Width _
                                                    + f.gridView.Columns("SAMC").Width _
                                                    + f.gridView.Columns("DIMC").Width
            If FORMAT2 Then
                .Columns("DISPER~DISCOUNT~DISCEMPNAME~DISCVA").Width = f.gridView.Columns("DISCOUNT").Width + f.gridView.Columns("DISPER").Width + IIf(f.gridView.Columns("DISPER").Visible = True, f.gridView.Columns("DISPER").Width, 0) + IIf(f.gridView.Columns("DISCVA").Visible = True, f.gridView.Columns("DISCVA").Width, 0)
            Else
                .Columns("DISPER~DISCOUNT~DISCEMPNAME").Width = f.gridView.Columns("DISCOUNT").Width + f.gridView.Columns("DISPER").Width + IIf(f.gridView.Columns("DISPER").Visible = True, f.gridView.Columns("DISPER").Width, 0)
            End If
            .Columns("STRATE~SARATE~DIRATE").Width = f.gridView.Columns("STRATE").Width _
                                                    + f.gridView.Columns("SARATE").Width _
                                                    + f.gridView.Columns("DIRATE").Width
            .Columns("STVALUE~SAVALUE~DIVALUE").Width = f.gridView.Columns("STVALUE").Width _
                                                    + f.gridView.Columns("SAVALUE").Width _
                                                    + f.gridView.Columns("DIVALUE").Width
            If ChkGrpbyEmp.Checked = False Then
                .Columns("EMPNAME").Width = f.gridView.Columns("EMPNAME").Width
            End If
            If chkWithDiffRate.Checked Then
                .Columns("TYPE~DIFFRATE~RATEPER~AGE").Width = f.gridView.Columns("TYPE").Width _
                                                          + f.gridView.Columns("DIFFRATE").Width _
                                                          + f.gridView.Columns("RATEPER").Width _
                                                          + f.gridView.Columns("AGE").Width
            Else
                .Columns("TYPE~AGE").Width = f.gridView.Columns("TYPE").Width + f.gridView.Columns("AGE").Width
            End If

            Dim colWid As Integer = 0
            For cnt As Integer = 0 To f.gridView.ColumnCount - 1
                If f.gridView.Columns(cnt).Visible Then colWid += f.gridView.Columns(cnt).Width
            Next
            If colWid >= f.gridView.Width Then
                f.gridViewHeader.Columns("SCROLL").Visible = CType(f.gridView.Controls(1), VScrollBar).Visible
                f.gridViewHeader.Columns("SCROLL").Width = CType(f.gridView.Controls(1), VScrollBar).Width
                f.gridViewHeader.Columns("SCROLL").HeaderText = ""
            Else
                f.gridViewHeader.Columns("SCROLL").Visible = False
            End If
        End With
    End Sub
    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        SetGridHeadColWid(CType(sender, DataGridView))
    End Sub

    Private Sub frmSaleReportWastMc_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pnlContainer.Location = New Point((ScreenWid - pnlContainer.Width) / 2, ((ScreenHit - 128) - pnlContainer.Height) / 2)
        strSql = "SELECT @@MICROSOFTVERSION/POWER(2,24)"
        SqlVersion = objGPack.GetSqlValue(strSql, "", 8, tran)
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ACTIVE = 'Y' ORDER BY METALNAME"
        FillCheckedListBox(strSql, chkLstMetal)
        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE ACTIVE = 'Y' ORDER BY CATNAME"
        FillCheckedListBox(strSql, chkLstCategory)
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkLstCostCentre.Enabled = True
            chkLstCostCentre.Items.Clear()
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            'FillCheckedListBox(strSql, chkLstCostCentre)
            da = New OleDbDataAdapter(strSql, cn)
            Dim dtCostCentre As New DataTable
            da.Fill(dtCostCentre)
            If dtCostCentre.Rows.Count > 0 Then
                'chkLstCostCentre.Items.Add("ALL", IIf(cnDefaultCostId, "ALL", False))
                For i As Integer = 0 To dtCostCentre.Rows.Count - 1
                    If cnCostName = dtCostCentre.Rows(i).Item("COSTNAME").ToString And cnDefaultCostId = False Then
                        chkLstCostCentre.Items.Add(dtCostCentre.Rows(i).Item("COSTNAME").ToString, True)
                    Else
                        chkLstCostCentre.Items.Add(dtCostCentre.Rows(i).Item("COSTNAME").ToString, False)
                    End If
                Next
                If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkLstCostCentre.Enabled = False
            Else
                chkLstCostCentre.Items.Clear()
                chkLstCostCentre.Enabled = False
            End If
        Else
            chkLstCostCentre.Enabled = False
            chkLstCostCentre.Items.Clear()
        End If
        strSql = " SELECT 'ALL' CASHNAME,1 ORD UNION ALL SELECT CASHNAME,2 ORD FROM " & cnAdminDb & "..CASHCOUNTER ORDER BY ORD,CASHNAME "
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtcash As New DataTable
        da.Fill(dtcash)
        chkcmbCashCtr.Items.Clear()
        If dtcash.Rows.Count > 0 Then
            For i As Integer = 0 To dtcash.Rows.Count - 1
                chkcmbCashCtr.Items.Add(dtcash.Rows(i)(0).ToString.Replace(",", ",,"), dtcash.Rows(i)(0).ToString = "ALL")
            Next
        Else
            chkcmbCashCtr.Enabled = False
        End If
        chkcmbCashCtr.Text = "ALL"

        ChklstboxInclude.Items.Clear()
        ChklstboxInclude.Items.Add("DESIGNER", True)
        ChklstboxInclude.Items.Add("STOCK WASTAGE", True)
        ChklstboxInclude.Items.Add("SALES WASTAGE", True)
        ChklstboxInclude.Items.Add("DIFF WASTAGE", True)

        strSql = " SELECT 'ALL' ITEMCTRNAME,1 ORD UNION ALL SELECT ITEMCTRNAME,2 ORD FROM " & cnAdminDb & "..ITEMCOUNTER ORDER BY ORD,ITEMCTRNAME "
        da = New OleDbDataAdapter(strSql, cn)
        Dim dtCounter As New DataTable
        da.Fill(dtCounter)
        chkcmbItemCtr.Items.Clear()
        If dtCounter.Rows.Count > 0 Then
            For i As Integer = 0 To dtCounter.Rows.Count - 1
                chkcmbItemCtr.Items.Add(dtCounter.Rows(i)(0).ToString.Replace(",", ",,"), dtCounter.Rows(i)(0).ToString = "ALL")
            Next
        Else
            chkcmbItemCtr.Enabled = False
        End If
        chkcmbItemCtr.Text = "ALL"


        LoadCompany(chkLstCompany)
        btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub chkMetalSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkMetalSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstMetal, chkMetalSelectAll.Checked)
    End Sub

    Private Sub chkCostCentreSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkCostCentreSelectAll.Checked)
    End Sub

    Private Sub chkCategorySelectAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkCategorySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCategory, chkCategorySelectAll.Checked)
    End Sub

    Private Sub chkLstMetal_LostFocus(sender As Object, e As EventArgs) Handles chkLstMetal.LostFocus
        Dim chkMetalNames As String = GetChecked_CheckedList(chkLstMetal)
        If chkMetalNames <> "" Then
            chkLstCategory.Items.Clear()
            strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
            strSql += " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalNames & "))"
            strSql += " ORDER BY CATNAME"
            FillCheckedListBox(strSql, chkLstCategory)
        End If
    End Sub

    Private Sub rbtNonTag_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtNonTag.CheckedChanged
        chkHomeSales.Visible = Not rbtTag.Checked
        chkHomeSales.Checked = Not rbtTag.Checked
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddHandler chkLstCompany.LostFocus, AddressOf CheckedListCompany_Lostfocus
    End Sub
    Private Sub chkCompanySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmSaleReportWastMc_Properties
        obj.p_chkCompanySelectAll = chkCompanySelectAll.Checked
        GetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany)
        'obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_chkMetalSelectAll = chkMetalSelectAll.Checked
        GetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal)
        GetChecked_CheckedList(chkLstCategory, obj.p_chkLstCategory)
        obj.p_rbtTag = rbtTag.Checked
        obj.p_rbtNonTag = rbtNonTag.Checked
        obj.p_rbtBoth = rbtBoth.Checked
        obj.p_chkHomeSales = chkHomeSales.Checked
        obj.p_chkGroupByEmp = ChkGrpbyEmp.Checked
        obj.p_rbtItem = rbtItemId.Checked
        obj.p_rbtTranNo = rbtTranNo.Checked
        obj.p_chkstnamt = chkstnamt.Checked
        obj.p_chkHighLight = ChkHighLight.Checked
        obj.p_chkinclude = ChkInclude.Checked
        GetChecked_CheckedList(ChklstboxInclude, obj.p_ChklstboxInclude)
        SetSettingsObj(obj, Me.Name, GetType(frmSaleReportWastMc_Properties))
    End Sub
    Private Sub Prop_Gets()
        Dim obj As New frmSaleReportWastMc_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmSaleReportWastMc_Properties))
        chkCompanySelectAll.Checked = obj.p_chkCompanySelectAll
        SetChecked_CheckedList(chkLstCompany, obj.p_chkLstCompany, strCompanyName)
        'chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        chkMetalSelectAll.Checked = obj.p_chkMetalSelectAll
        SetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal, Nothing)
        SetChecked_CheckedList(chkLstCategory, obj.p_chkLstCategory, Nothing)
        rbtTag.Checked = obj.p_rbtTag
        rbtNonTag.Checked = obj.p_rbtNonTag
        rbtBoth.Checked = obj.p_rbtBoth
        chkHomeSales.Checked = obj.p_chkHomeSales
        ChkGrpbyEmp.Checked = obj.p_chkGroupByEmp
        rbtItemId.Checked = obj.p_rbtItem
        rbtTranNo.Checked = obj.p_rbtTranNo
        chkstnamt.Checked = obj.p_chkstnamt
        ChkHighLight.Checked = obj.p_chkHighLight
        ChkInclude.Checked = obj.p_chkinclude
        SetChecked_CheckedList(ChklstboxInclude, obj.p_ChklstboxInclude, Nothing)
    End Sub


End Class


Public Class frmSaleReportWastMc_Properties

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
    Private chkCostCentreSelectAll As Boolean = False
    Public Property p_chkCostCentreSelectAll() As Boolean
        Get
            Return chkCostCentreSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCostCentreSelectAll = value
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
    Private chkMetalSelectAll As Boolean = False
    Public Property p_chkMetalSelectAll() As Boolean
        Get
            Return chkMetalSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkMetalSelectAll = value
        End Set
    End Property
    Private chkLstMetal As New List(Of String)
    Public Property p_chkLstMetal() As List(Of String)
        Get
            Return chkLstMetal
        End Get
        Set(ByVal value As List(Of String))
            chkLstMetal = value
        End Set
    End Property
    Private chkLstCategory As New List(Of String)
    Public Property p_chkLstCategory() As List(Of String)
        Get
            Return chkLstCategory
        End Get
        Set(ByVal value As List(Of String))
            chkLstCategory = value
        End Set
    End Property

    Private rbtTag As Boolean = True
    Public Property p_rbtTag() As Boolean
        Get
            Return rbtTag
        End Get
        Set(ByVal value As Boolean)
            rbtTag = value
        End Set
    End Property
    Private rbtNonTag As Boolean = False
    Public Property p_rbtNonTag() As Boolean
        Get
            Return rbtNonTag
        End Get
        Set(ByVal value As Boolean)
            rbtNonTag = value
        End Set
    End Property
    Private rbtBoth As Boolean = False
    Public Property p_rbtBoth() As Boolean
        Get
            Return rbtBoth
        End Get
        Set(ByVal value As Boolean)
            rbtBoth = value
        End Set
    End Property
    Private chkHomeSales As Boolean = True
    Public Property p_chkHomeSales() As Boolean
        Get
            Return chkHomeSales
        End Get
        Set(ByVal value As Boolean)
            chkHomeSales = value
        End Set
    End Property
    Private chkGroupByEmp As Boolean = True
    Public Property p_chkGroupByEmp() As Boolean
        Get
            Return chkGroupByEmp
        End Get
        Set(ByVal value As Boolean)
            chkGroupByEmp = value
        End Set
    End Property
    Private rbtItem As Boolean = False
    Public Property p_rbtItem() As Boolean
        Get
            Return rbtItem
        End Get
        Set(ByVal value As Boolean)
            rbtItem = value
        End Set
    End Property
    Private rbtTranNo As Boolean = True
    Public Property p_rbtTranNo() As Boolean
        Get
            Return rbtTranNo
        End Get
        Set(ByVal value As Boolean)
            rbtTranNo = value
        End Set
    End Property
    Private chkstnamt As Boolean = True
    Public Property p_chkstnamt() As Boolean
        Get
            Return chkstnamt
        End Get
        Set(ByVal value As Boolean)
            chkstnamt = value
        End Set
    End Property
    Private chkHighLight As Boolean = False
    Public Property p_chkHighLight() As Boolean
        Get
            Return chkHighLight
        End Get
        Set(ByVal value As Boolean)
            chkHighLight = value
        End Set
    End Property
    Private ChklstboxInclude As New List(Of String)
    Public Property p_ChklstboxInclude() As List(Of String)
        Get
            Return ChklstboxInclude
        End Get
        Set(ByVal value As List(Of String))
            ChklstboxInclude = value
        End Set
    End Property
    Private chkinclude As Boolean = True
    Public Property p_chkinclude() As Boolean
        Get
            Return chkinclude
        End Get
        Set(ByVal value As Boolean)
            chkinclude = value
        End Set
    End Property
End Class