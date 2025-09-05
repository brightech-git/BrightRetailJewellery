Imports System.Data.OleDb
Imports System.Net.Mail
Imports System.IO

Public Class frmVaultReport
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dsGridView As New DataSet
    Dim ftrStr As String
    Dim dtCostCentre As New DataTable
    Dim dtMetal As New DataTable
    Dim Specificformat As Boolean = False
    Dim viewtype As String = ""
    Dim msgTxt As String = ""
    Dim INSERTFLAG As String = "I"
    'Dim dt_Head As New DataTable

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.btnExit_Click(Me, New EventArgs)
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        ResizeToolStripMenuItem.Checked = False
        gridView.DataSource = Nothing
        gridHead.DataSource = Nothing
        lblTitle.Text = ""
        Me.Refresh()
        Dim MetalId As String = ""
        Dim chkCostId As String
        If ChkCmbMetal.Text = "ALL" Then
            MetalId = ""
        Else
            MetalId = GetQryStringForSp(ChkCmbMetal.Text, cnAdminDb & "..METALMAST", "METALID", "METALNAME", False)
        End If
        chkCostId = GetQryStringForSp(chkCmbCostCentre.Text, cnAdminDb & "..COSTCENTRE", "COSTID", "COSTNAME", False)
        strSql = "SELECT COSTNAME FROM " & cnAdminDb & "..SYNCCOSTCENTRE S JOIN " & cnAdminDb & "..COSTCENTRE C ON C.COSTID=S.COSTID WHERE MAIN='Y'"
        Dim HoCostname As String = objGPack.GetSqlValue(strSql, "COSTNAME", "CORP")
        strSql = "SELECT S.COSTID FROM " & cnAdminDb & "..SYNCCOSTCENTRE S JOIN " & cnAdminDb & "..COSTCENTRE C ON C.COSTID=S.COSTID WHERE MAIN='Y'"
        Dim HoCostId As String = objGPack.GetSqlValue(strSql, "COSTID")
        Dim TEMPTABLE As String = "TEMPTABLEDB..TEMP" & systemId & "RECLOTTAG"
        strSql = "IF OBJECT_ID('" & TEMPTABLE & "') IS NOT NULL DROP TABLE " & TEMPTABLE
        cmd = New OleDbCommand(strSql, cn) : cmd.ExecuteNonQuery()
        strSql = "EXEC " & cnAdminDb & "..SP_RPT_MATERIALRECEIPTVSSTOCK"
        strSql += vbCrLf + "   @DBNAME='" & cnStockDb & "'"
        strSql += vbCrLf + "  ,@TEMPTABLE='" & TEMPTABLE & "'"
        strSql += vbCrLf + "  ,@FROMDATE='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "'"
        strSql += vbCrLf + "  ,@TODATE='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "'"
        strSql += vbCrLf + "  ,@COSTID='" & HoCostId & "'"
        strSql += vbCrLf + "  ,@SUMMARY='Y'"
        strSql += vbCrLf + "  ,@METALID='" & MetalId & "'"
        strSql += vbCrLf + "  ,@STARTDATE='" & Format(cnTranFromDate, "yyyy-MM-dd") & "'"
        strSql += vbCrLf + "  ,@TRANNO=''"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + " EXEC " & cnAdminDb & "..RPT_ITEMWISESTOCK"
        strSql += vbCrLf + " @DBNAME = '" & cnStockDb & "'"
        strSql += vbCrLf + ",@FRMDATE = '" & dtpFrom.Value.ToString("yyyy/MM/dd") & "'"
        strSql += vbCrLf + ",@TODATE = '" & dtpFrom.Value.ToString("yyyy/MM/dd") & "'"
        strSql += vbCrLf + ",@METALID = '" & GetSelectedMetalid(ChkCmbMetal, False) & "'"
        strSql += vbCrLf + ",@CATCODE ='ALL'"
        strSql += vbCrLf + ",@ITEMTYPE = ''"
        strSql += vbCrLf + ",@ITEMIDS = ''"
        strSql += vbCrLf + ",@ITEMTYPEIDS = ''"
        strSql += vbCrLf + ",@DESIGNERIDS = ''"
        strSql += vbCrLf + ",@COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + ",@COUNTERIDS = ''"
        strSql += vbCrLf + ",@COSTIDS = ''"
        strSql += vbCrLf + ",@SYSTEMID = '" & systemId & "'"
        strSql += vbCrLf + ",@TAGTYPE='B'"
        strSql += vbCrLf + ",@WITHSUBITEM = 'N'"
        strSql += vbCrLf + ",@WITHTR = 'Y'"
        strSql += vbCrLf + ",@WITHDIA='Y'"
        strSql += vbCrLf + ",@WITHSTONE='Y'"
        strSql += vbCrLf + ",@DIASTNBYROW = 'N'"
        strSql += vbCrLf + ",@WITHAPP = 'Y'"
        strSql += vbCrLf + ",@SHORTNAME = 'N'"
        strSql += vbCrLf + ",@ORDERBY = 'Y'"
        strSql += vbCrLf + ",@HIDEBACKOFF = 'N'"
        strSql += vbCrLf + ",@GROUPBY='CO'"
        strSql += vbCrLf + ",@SEPAPP = 'N'"
        strSql += vbCrLf + ",@WITHCUMSTK = 'Y'"
        strSql += vbCrLf + ",@WITHRATE = 'N'"
        strSql += vbCrLf + ",@ISORDREG = ''"
        strSql += vbCrLf + ",@SUMMARY = ''"
        strSql += vbCrLf + ",@STNINGRSWT='N'"
        strSql += vbCrLf + ",@COUNTERGROUP=''"
        strSql += vbCrLf + ",@ONLYAPP=''"
        strSql += vbCrLf + ",@STKTYPE=''"
        strSql += vbCrLf + " ,@ZEROSTK ='Y'"
        strSql += vbCrLf + " ,@PENDINGSTK ='Y'"
        strSql += vbCrLf + " ,@MINUSSTK ='Y'"
        strSql += vbCrLf + " ,@ORDERBYIDNAME ='N'"
        strSql += vbCrLf + " ,@SPECIFICFORMAT ='0'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "IF OBJECT_ID('TEMPTABLEDB..TEMP" & systemId & "RECEIPT','U')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & systemId & "RECEIPT"
        strSql += " IF OBJECT_ID('TEMPTABLEDB..TEMP" & systemId & "RECEIPTPEND','U')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & systemId & "RECEIPTPEND"
        strSql += " IF OBJECT_ID('TEMPTABLEDB..TEMP" & systemId & "CLOSINGRECEIPT','U')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & systemId & "CLOSINGRECEIPT"
        strSql += " IF OBJECT_ID('TEMPTABLEDB..TEMP" & systemId & "ISSUE','U')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ISSUE"
        strSql += " IF OBJECT_ID('TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT','U')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT"
        strSql += " IF OBJECT_ID('TEMPTABLEDB..TEMP" & systemId & "PENDTRANS','U')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP" & systemId & "PENDTRANS"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + "SELECT COSTID,TRANTYPE,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,TRFNO "
        strSql += vbCrLf + ",(SELECT SUM(CASE WHEN STONEUNIT='G' THEN  5*STNWT ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE AS S WHERE S.ISSSNO=R.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D'))DIAWT "
        strSql += vbCrLf + "INTO TEMPTABLEDB..TEMP" & systemId & "RECEIPT"
        strSql += vbCrLf + "FROM " & cnStockDb & "..RECEIPT AS R "
        strSql += vbCrLf + "WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' "
        strSql += vbCrLf + "AND '" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' "
        strSql += vbCrLf + "AND ISNULL(CANCEL,'')=''"
        If MetalId <> "" Then
            strSql += vbCrLf + "AND METALID IN('" & Replace(MetalId, ",", "','") & "')"
        End If
        strSql += vbCrLf + "GROUP BY TRANTYPE,COSTID,TRFNO,R.SNO"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + "SELECT COSTID,TRANTYPE,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,TRFNO "
        strSql += vbCrLf + ",(SELECT SUM(CASE WHEN STONEUNIT='G' THEN 5*STNWT ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE AS S WHERE S.ISSSNO=R.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D'))DIAWT "
        strSql += vbCrLf + "INTO TEMPTABLEDB..TEMP" & systemId & "RECEIPTPEND"
        strSql += vbCrLf + "FROM " & cnStockDb & "..RECEIPT AS R"
        strSql += vbCrLf + "WHERE TRANDATE <= '" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' "
        strSql += vbCrLf + "AND ISNULL(CANCEL,'')=''"
        If MetalId <> "" Then
            strSql += vbCrLf + "AND METALID IN('" & Replace(MetalId, ",", "','") & "')"
        End If
        strSql += vbCrLf + "GROUP BY TRANTYPE,COSTID,TRFNO,R.SNO"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + "SELECT COSTID,TRANTYPE,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,TRFNO "
        strSql += vbCrLf + ",(SELECT SUM(CASE WHEN STONEUNIT='G' THEN 5*STNWT ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE AS S WHERE S.ISSSNO=R.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D'))DIAWT "
        strSql += vbCrLf + "INTO TEMPTABLEDB..TEMP" & systemId & "ISSUE"
        strSql += vbCrLf + "FROM " & cnStockDb & "..ISSUE AS R"
        strSql += vbCrLf + "WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' "
        strSql += vbCrLf + "AND '" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' "
        strSql += vbCrLf + "AND ISNULL(CANCEL,'')=''"
        If MetalId <> "" Then
            strSql += vbCrLf + "AND METALID IN('" & Replace(MetalId, ",", "','") & "')"
        End If
        strSql += vbCrLf + "GROUP BY TRANTYPE,COSTID,TRFNO,R.SNO"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " SELECT COSTID"
        strSql += vbCrLf + " ,T.TAGGRSWT-T.GRSWT GRSWT,T.TAGNETWT-T.NETWT NETWT,TRFNO,TRANDATE "
        'DIA WT
        'strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT='C' THEN STNWT ELSE 5*STNWT END)DIAWT FROM " & cnStockDb & "..ISSSTONE AS T"
        'strSql += vbCrLf + "  WHERE T.TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        'strSql += vbCrLf + " AND T.TRANTYPE = 'SA'"
        'strSql += vbCrLf + " AND (T.TAGSTNPCS <> T.STNPCS OR T.TAGSTNWT <> T.STNWT)"
        'strSql += vbCrLf + " AND (T.TAGSTNPCS <> 0 OR T.TAGSTNWT <> 0 ) "
        'strSql += vbCrLf + " AND ISSSNO IN (SELECT SNO FROM " & cnStockDb & "..ISSUE WHERE ISNULL(CANCEL,'')='')"
        'strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D'))DIAWT"
        strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT='G' THEN 5*(TAGSTNWT-STNWT) ELSE TAGSTNWT-STNWT END) FROM " & cnStockDb & "..ISSSTONE AS S "
        strSql += vbCrLf + " WHERE S.ISSSNO=T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D'))DIAWT "
        '
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "PENDTRANS"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE T "
        strSql += vbCrLf + " WHERE T.TRANDATE <=  '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + " AND T.TRANTYPE = 'SA' AND ISNULL(T.CANCEL,'') = ''"
        strSql += vbCrLf + " AND (T.TAGPCS <> T.PCS OR T.TAGGRSWT <> T.GRSWT)"
        strSql += vbCrLf + " AND (T.TAGPCS <> 0 OR T.TAGGRSWT <> 0 )"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " SELECT COSTID,TRANTYPE "
        ''''' FOR SECOND SALES''''
        'strSql += vbCrLf + ",SUM(GRSWT)GRSWT,SUM(NETWT)NETWT "
        strSql += vbCrLf + " ,CASE WHEN TRANTYPE='PU' THEN SUM(GRSWT)-ISNULL((select SUM(GRSWT) FROM " & cnStockDb & "..ISSUE WHERE "
        strSql += vbCrLf + " RUNNO=R.SNO AND TRANDATE  <'" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' AND RTRIM(LTRIM(REMARK1))='SECOND HAND SALES' AND ISNULL(CANCEL,'')=''),0) ELSE SUM(GRSWT) END GRSWT"
        strSql += vbCrLf + " ,CASE WHEN TRANTYPE='PU' THEN ISNULL((select SUM(GRSWT) FROM " & cnStockDb & "..ISSUE WHERE "
        strSql += vbCrLf + " RUNNO=R.SNO AND TRANDATE  ='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' AND RTRIM(LTRIM(REMARK1))='SECOND HAND SALES' AND ISNULL(CANCEL,'')=''),0) ELSE 0 END SGRSWT"
        strSql += vbCrLf + " ,CASE WHEN TRANTYPE='PU' THEN SUM(NETWT)-ISNULL((select SUM(NETWT) FROM " & cnStockDb & "..ISSUE WHERE "
        strSql += vbCrLf + " RUNNO=R.SNO AND TRANDATE  <'" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' AND RTRIM(LTRIM(REMARK1))='SECOND HAND SALES' AND ISNULL(CANCEL,'')=''),0) ELSE SUM(NETWT) END NETWT"
        strSql += vbCrLf + " ,CASE WHEN TRANTYPE='PU' THEN ISNULL((select SUM(NETWT) FROM " & cnStockDb & "..ISSUE WHERE "
        strSql += vbCrLf + " RUNNO=R.SNO AND TRANDATE  ='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' AND RTRIM(LTRIM(REMARK1))='SECOND HAND SALES' AND ISNULL(CANCEL,'')=''),0) ELSE 0 END SNETWT"
        ''''' END FOR SECOND SALES''''
        strSql += vbCrLf + " ,TRFNO,TRANDATE "
        ''''' FOR SECOND SALES''''
        ''strSql += vbCrLf + ",(SELECT SUM(CASE WHEN STONEUNIT='G' THEN 5*STNWT ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE AS S "
        ''strSql += vbCrLf + "WHERE S.ISSSNO=R.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D'))DIAWT "
        strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT='G' THEN 5*STNWT ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE AS S "
        strSql += vbCrLf + " WHERE S.ISSSNO=R.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')) "
        strSql += vbCrLf + " -ISNULL((SELECT SUM(CASE WHEN STONEUNIT='G' THEN 5*STNWT ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE AS S "
        strSql += vbCrLf + " WHERE S.ISSSNO IN ((SELECT SNO FROM " & cnStockDb & "..ISSUE WHERE RUNNO=R.SNO AND TRANDATE  <'" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' "
        strSql += vbCrLf + " AND RTRIM(LTRIM(REMARK1))='SECOND HAND SALES' AND ISNULL(CANCEL,'')=''))"
        strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')),0)DIAWT "
        strSql += vbCrLf + " ,ISNULL((SELECT SUM(CASE WHEN STONEUNIT='G' THEN 5*STNWT ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE AS S "
        strSql += vbCrLf + " WHERE S.ISSSNO IN ((SELECT SNO FROM " & cnStockDb & "..ISSUE WHERE RUNNO=R.SNO AND TRANDATE  ='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' "
        strSql += vbCrLf + " AND RTRIM(LTRIM(REMARK1))='SECOND HAND SALES' AND ISNULL(CANCEL,'')=''))"
        strSql += vbCrLf + " AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE='D')),0)SDIAWT "
        ''''' END FOR SECOND SALES''''
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "CLOSINGRECEIPT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS R"
        strSql += vbCrLf + " WHERE TRANDATE  <='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' "
        strSql += vbCrLf + " AND ISNULL(CANCEL,'')=''"
        If MetalId <> "" Then
            strSql += vbCrLf + " AND METALID IN('" & Replace(MetalId, ",", "','") & "')"
        End If
        strSql += vbCrLf + " GROUP BY TRANTYPE,COSTID,TRFNO,TRANDATE,R.SNO"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + "CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT("
        strSql += vbCrLf + "IN_SNO INT NULL,IN_COSTCENTRE VARCHAR(50),IN_PARTICULARS VARCHAR(150)"
        strSql += vbCrLf + ",IN_GRSWT NUMERIC(20,3),IN_NETWT NUMERIC(20,3),IN_DIAWT NUMERIC(20,3),IN_RESULT INT,IN_COLHEAD VARCHAR(1)"
        strSql += vbCrLf + ",OUT_SNO INT NULL,OUT_COSTCENTRE VARCHAR(50),OUT_PARTICULARS VARCHAR(150)"
        strSql += vbCrLf + ",OUT_GRSWT NUMERIC(20,3),OUT_NETWT NUMERIC(20,3),OUT_DIAWT NUMERIC(20,3),OUT_RESULT INT,OUT_COLHEAD VARCHAR(1)"
        strSql += vbCrLf + ",SNO INT IDENTITY)"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        If cnCostId = cnHOCostId And (chkCostId.Contains(cnHOCostId) Or chkCostId.Contains("ALL")) Then
            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
            strSql += vbCrLf + "SELECT NULL,'','" & HoCostname & "',NULL,NULL,NULL,0,'H',NULL,'',''"
            strSql += vbCrLf + ",NULL,NULL,NULL,0,''"
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + "SELECT NULL,'','OPENING',NULL,NULL,NULL,0,'T',NULL,'','ISSUES'"
            strSql += vbCrLf + ",NULL,NULL,NULL,0,'T'"
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + "SELECT 1,'" & HoCostname & "','OPENING VAULT STOCK',RGRSWT,RNETWT,RDIAWT,1,'',1,'" & HoCostname & "','SALES'"
            strSql += vbCrLf + ",(SELECT SUM(GRSWT) FROM TEMPTABLEDB..TEMP" & systemId & "ISSUE WHERE TRANTYPE='SA' AND COSTID='" & HoCostId & "')"
            strSql += vbCrLf + ",(SELECT SUM(NETWT) FROM TEMPTABLEDB..TEMP" & systemId & "ISSUE WHERE TRANTYPE='SA' AND COSTID='" & HoCostId & "')"
            strSql += vbCrLf + ",(SELECT SUM(DIAWT) FROM TEMPTABLEDB..TEMP" & systemId & "ISSUE WHERE TRANTYPE='SA' AND COSTID='" & HoCostId & "'),1,''"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "RECLOTTAG1 WHERE RESULT=-1"
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + "SELECT 2,'" & HoCostname & "','OPENING LOT PENDING',LGRSWT,LNETWT,LDIAWT,2,'',2,'" & HoCostname & "','MISC ISSUE'"
            strSql += vbCrLf + ",(SELECT SUM(GRSWT) FROM TEMPTABLEDB..TEMP" & systemId & "ISSUE WHERE TRANTYPE='MI' AND COSTID='" & HoCostId & "')"
            strSql += vbCrLf + ",(SELECT SUM(NETWT) FROM TEMPTABLEDB..TEMP" & systemId & "ISSUE WHERE TRANTYPE='MI' AND COSTID='" & HoCostId & "')"
            strSql += vbCrLf + ",(SELECT SUM(DIAWT) FROM TEMPTABLEDB..TEMP" & systemId & "ISSUE WHERE TRANTYPE='MI' AND COSTID='" & HoCostId & "'),2,''"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "RECLOTTAG1 WHERE RESULT=-1"
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + "SELECT 3,'" & HoCostname & "','OPENING TAG + NON TAG STOCK',SUM(OGRSWT),SUM(ONETWT),SUM(ODIAWT),3,'',3,'" & HoCostname & "','APPROVAL ISSUE'"
            strSql += vbCrLf + ",(SELECT SUM(GRSWT) FROM TEMPTABLEDB..TEMP" & systemId & "ISSUE WHERE TRANTYPE='IAP' AND COSTID='" & HoCostId & "')"
            strSql += vbCrLf + ",(SELECT SUM(NETWT) FROM TEMPTABLEDB..TEMP" & systemId & "ISSUE WHERE TRANTYPE='IAP' AND COSTID='" & HoCostId & "')"
            strSql += vbCrLf + ",(SELECT SUM(DIAWT) FROM TEMPTABLEDB..TEMP" & systemId & "ISSUE WHERE TRANTYPE='IAP' AND COSTID='" & HoCostId & "'),3,''"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "ITEMSTK WHERE COSTCENTRE="
            strSql += vbCrLf + "(SELECT TOP 1 COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & HoCostId & "')"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
            strSql += vbCrLf + "SELECT NULL,'" & HoCostname & "','OPENING',SUM(IN_GRSWT),SUM(IN_NETWT),SUM(IN_DIAWT),0,'O',NULL,'',''"
            strSql += vbCrLf + ",NULL,NULL,NULL,0,'O'"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE IN_SNO IN (1,2,3)"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
            strSql += vbCrLf + "SELECT NULL,'" & HoCostname & "','RECEIPT',NULL,NULL,NULL,4,'T',4,'" & HoCostname & "','ISSUES'"
            strSql += vbCrLf + ",(SELECT SUM(GRSWT) FROM TEMPTABLEDB..TEMP" & systemId & "ISSUE WHERE TRANTYPE='IIS' AND COSTID='" & HoCostId & "')"
            strSql += vbCrLf + ",(SELECT SUM(NETWT) FROM TEMPTABLEDB..TEMP" & systemId & "ISSUE WHERE TRANTYPE='IIS' AND COSTID='" & HoCostId & "')"
            strSql += vbCrLf + ",(SELECT SUM(DIAWT) FROM TEMPTABLEDB..TEMP" & systemId & "ISSUE WHERE TRANTYPE='IIS' AND COSTID='" & HoCostId & "'),4,''"
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + "SELECT 4,'" & HoCostname & "','APPROVAL RECEIPTS',SUM(GRSWT),SUM(NETWT),SUM(DIAWT),5,'',5,'" & HoCostname & "','PURCHASE RETURN'"
            strSql += vbCrLf + ",(SELECT SUM(GRSWT) FROM TEMPTABLEDB..TEMP" & systemId & "ISSUE WHERE TRANTYPE='IPU' AND COSTID='" & HoCostId & "')"
            strSql += vbCrLf + ",(SELECT SUM(NETWT) FROM TEMPTABLEDB..TEMP" & systemId & "ISSUE WHERE TRANTYPE='IPU' AND COSTID='" & HoCostId & "')"
            strSql += vbCrLf + ",(SELECT SUM(DIAWT) FROM TEMPTABLEDB..TEMP" & systemId & "ISSUE WHERE TRANTYPE='IPU' AND COSTID='" & HoCostId & "'),5,''"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "RECEIPT WHERE TRANTYPE='RAP' AND COSTID='" & HoCostId & "'"
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + "SELECT 5,'" & HoCostname & "','RECEIPT',SUM(GRSWT),SUM(NETWT),SUM(DIAWT),6,'',6,'" & HoCostname & "','STOCK ISSUE TO BRANCH(TAG/NON TAG)'"
            strSql += vbCrLf + ",(SELECT SUM(GRSWT) FROM TEMPTABLEDB..TEMP" & systemId & "ISSUE WHERE TRANTYPE='IIN' AND COSTID='" & HoCostId & "')"
            strSql += vbCrLf + ",(SELECT SUM(NETWT) FROM TEMPTABLEDB..TEMP" & systemId & "ISSUE WHERE TRANTYPE='IIN' AND COSTID='" & HoCostId & "')"
            strSql += vbCrLf + ",(SELECT SUM(DIAWT) FROM TEMPTABLEDB..TEMP" & systemId & "ISSUE WHERE TRANTYPE='IIN' AND COSTID='" & HoCostId & "'),6,''"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "RECEIPT WHERE TRANTYPE='RRE' AND COSTID='" & HoCostId & "'"
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + "SELECT 6,'" & HoCostname & "','PURCHASE',SUM(GRSWT),SUM(NETWT),SUM(DIAWT),7,'',7,'" & HoCostname & "','TAG DIFF WT(LOSS)'"
            strSql += vbCrLf + ",(SELECT SUM(DGRSWT) FROM " & cnAdminDb & "..ITEMLOT "
            strSql += vbCrLf + "WHERE CDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' "
            strSql += vbCrLf + "AND '" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' HAVING SUM(DGRSWT)<0)"
            strSql += vbCrLf + ",(SELECT SUM(DNETWT) FROM " & cnAdminDb & "..ITEMLOT "
            strSql += vbCrLf + "WHERE CDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' "
            strSql += vbCrLf + "AND '" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' HAVING SUM(DNETWT)<0)"
            strSql += vbCrLf + ",(SELECT SUM(DIAWT) FROM " & cnAdminDb & "..ITEMLOT "
            strSql += vbCrLf + "WHERE CDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' "
            strSql += vbCrLf + "AND '" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' HAVING SUM(DNETWT)<0),7,''"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "RECEIPT WHERE TRANTYPE='RPU' AND COSTID='" & HoCostId & "'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
            strSql += vbCrLf + "SELECT NULL,'" & HoCostname & "','',NULL,NULL,NULL,13,'',NULL,'" & HoCostname & "','SUB TOTAL'"
            strSql += vbCrLf + ",SUM(OUT_GRSWT),SUM(OUT_NETWT),SUM(OUT_DIAWT),0,'J'"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE ISNULL(OUT_SNO,0) BETWEEN 1 AND 7 "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
            strSql += vbCrLf + "SELECT 7,'" & HoCostname & "','INTERNAL RECEIPTS(MI/OG/SR/PS) FROM BRANCH'"
            strSql += vbCrLf + ",SUM(GRSWT),SUM(NETWT),SUM(DIAWT),8,'',NULL,'" & HoCostname & "','CLOSING',NULL,NULL,NULL,8,'T'"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "RECEIPT WHERE TRANTYPE='RIN' AND COSTID='" & HoCostId & "'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
            strSql += vbCrLf + "SELECT NULL,'" & HoCostname & "','SUB TOTAL',SUM(IN_GRSWT),SUM(IN_NETWT),SUM(IN_DIAWT),0,'J',NULL,'',''"
            strSql += vbCrLf + ",NULL,NULL,NULL,0,''"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE ISNULL(IN_SNO,0) BETWEEN 4 AND 7 "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
            strSql += vbCrLf + "SELECT 8,'" & HoCostname & "','TAG DIFF WT(GAIN)'"
            strSql += vbCrLf + ",(SELECT SUM(DGRSWT) FROM " & cnAdminDb & "..ITEMLOT "
            strSql += vbCrLf + "WHERE CDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' "
            strSql += vbCrLf + "AND '" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' HAVING SUM(DGRSWT)>0)"
            strSql += vbCrLf + ",(SELECT SUM(DNETWT) FROM " & cnAdminDb & "..ITEMLOT "
            strSql += vbCrLf + "WHERE CDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' "
            strSql += vbCrLf + "AND '" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' HAVING SUM(DNETWT)>0)"
            strSql += vbCrLf + ",(SELECT SUM(DIAWT) FROM " & cnAdminDb & "..ITEMLOT "
            strSql += vbCrLf + "WHERE CDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' "
            strSql += vbCrLf + "AND '" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' HAVING SUM(DNETWT)>0)"

            strSql += vbCrLf + ",9,'',8,'" & HoCostname & "','VAULT CLOSING STOCK',RGRSWT,RNETWT,RDIAWT,9,''"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "RECLOTTAG1 WHERE RESULT=12"
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + "SELECT 9,'" & HoCostname & "','STOCK IN TRANSIT',NULL,NULL,NULL,10,'',9,'" & HoCostname & "','PENDING LOT',LGRSWT,LNETWT,LDIAWT,10,''"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "RECLOTTAG1 WHERE RESULT=12"
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + "SELECT NULL,' ',' ',NULL,NULL,NULL,11,'',10,'" & HoCostname & "','TAG AND NON TAG STOCK',SUM(CGRSWT),SUM(CNETWT),SUM(CDIAWT),11,''"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "ITEMSTK WHERE COSTCENTRE="
            strSql += vbCrLf + "(SELECT TOP 1 COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID='" & HoCostId & "')"
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + "SELECT 10,' ','MR PENDING IN TRANSIT',SUM(GRSWT),SUM(NETWT),NULL,12,'R',11,'" & HoCostname & "','MR PENDING IN TRANSIT',SUM(GRSWT),SUM(NETWT),NULL,12,'R'"
            strSql += vbCrLf + "FROM " & cnStockDb & "..TRECEIPT WHERE TRANDATE<='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "'"
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + "SELECT NULL,' ',' ',NULL,NULL,NULL,13,'',12,'" & HoCostname & "','IN TRANSIT, STOCK ISSUED TO BR',NULL,NULL,NULL,13,''"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
            strSql += vbCrLf + "SELECT NULL,NULL,'',NULL,NULL,NULL,13,'',NULL,'" & HoCostname & "','CLOSING'"
            strSql += vbCrLf + ",SUM(OUT_GRSWT),SUM(OUT_NETWT),SUM(OUT_DIAWT),0,'C'"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE ISNULL(OUT_SNO,0) BETWEEN 8 AND 12 "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
            strSql += vbCrLf + "SELECT NULL,'','" & HoCostname & " TOTAL',SUM(IN_GRSWT),SUM(IN_NETWT),SUM(IN_DIAWT),13,'S',NULL,'',''"
            strSql += vbCrLf + ",SUM(OUT_GRSWT),SUM(OUT_NETWT),SUM(OUT_DIAWT),15,'S'"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE IN_COLHEAD NOT IN ('V','K','O','C','J') AND OUT_COLHEAD NOT IN ('V','K','O','C','J')"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
            strSql += vbCrLf + "SELECT NULL,'',' ',NULL,NULL,NULL,16,'',NULL,'','DIFF IN " & HoCostname & "'"
            strSql += vbCrLf + ",(IN_GRSWT-OUT_GRSWT)"
            strSql += vbCrLf + ",(IN_NETWT-OUT_NETWT)"
            strSql += vbCrLf + ",(IN_DIAWT-OUT_DIAWT)"
            strSql += vbCrLf + ",14,'G'"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE OUT_RESULT=15 "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
        End If
        strSql = "SELECT C.COSTID,C.COSTNAME FROM " & cnAdminDb & "..SYNCCOSTCENTRE S JOIN " & cnAdminDb & "..COSTCENTRE C ON C.COSTID=S.COSTID WHERE MAIN<>'Y'"
        If cnCostId <> cnHOCostId Then
            strSql += vbCrLf + "  AND C.COSTID='" & cnCostId & "' "
        End If
        Dim dtCost As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCost)
        'If cnCostId <> cnHOCostId Then
        '    strSql = "DELETE FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
        '    cmd = New OleDbCommand(strSql, cn)
        '    cmd.ExecuteNonQuery()
        'End If
        For Cnt As Integer = 0 To dtCost.Rows.Count - 1
            Dim CostName As String = dtCost.Rows(Cnt).Item("COSTNAME").ToString
            Dim CostId As String = dtCost.Rows(Cnt).Item("COSTID").ToString
            Dim Grswt, NetWt As Decimal
            If chkCostId.Contains("ALL") = False Then
                If chkCostId.Contains(CostId) = False Then Continue For
            End If
            strSql = vbCrLf + "SELECT SUM(I.GRSWT)GRSWT,SUM(I.NETWT)NETWT "
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE I"
            strSql += vbCrLf + "JOIN " & cnAdminDb & "..ITEMMAST M ON M.ITEMID=I.ITEMID"
            strSql += vbCrLf + "WHERE I.TRANDATE='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' "
            strSql += vbCrLf + "AND M.STOCKTYPE='T'"
            strSql += vbCrLf + "AND I.TRANTYPE='SA' AND I.COSTID='" & CostId & "' AND ISNULL(I.TAGNO,'')='' AND ISNULL(I.CANCEL,'') = ''"
            Dim dr As DataRow = GetSqlRow(strSql, cn)
            If Not dr Is Nothing Then
                Grswt = Val(dr.Item("GRSWT").ToString)
                NetWt = Val(dr.Item("NETWT").ToString)
            End If
            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
            strSql += vbCrLf + "SELECT NULL,'','',NULL,NULL,NULL,0,'',NULL,'',''"
            strSql += vbCrLf + ",NULL,NULL,NULL,0,''"
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + "SELECT NULL,'','" & CostName & "',NULL,NULL,NULL,0,'H',NULL,'',''"
            strSql += vbCrLf + ",NULL,NULL,NULL,0,''"
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + "SELECT NULL,'','OPENING',NULL,NULL,NULL,0,'T',NULL,'','ISSUES'"
            strSql += vbCrLf + ",NULL,NULL,NULL,0,'T'"
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + "SELECT 1,'" & CostName & "','OPENING STOCK (TAG & NONTAG)',SUM(OGRSWT),SUM(ONETWT),SUM(ODIAWT),1,'',1,'" & CostName & "','SALES'"
            strSql += vbCrLf + ",(SELECT SUM(GRSWT) FROM TEMPTABLEDB..TEMP" & systemId & "ISSUE WHERE TRANTYPE='SA' AND COSTID='" & CostId & "')"
            strSql += vbCrLf + ",(SELECT SUM(NETWT) FROM TEMPTABLEDB..TEMP" & systemId & "ISSUE WHERE TRANTYPE='SA' AND COSTID='" & CostId & "')"
            strSql += vbCrLf + ",(SELECT SUM(DIAWT) FROM TEMPTABLEDB..TEMP" & systemId & "ISSUE WHERE TRANTYPE='SA' AND COSTID='" & CostId & "'),1,''"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "ITEMSTK WHERE COSTCENTRE='" & CostName & "'"
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + "SELECT 2,'" & CostName & "','PARTLY SALES STK (PENDING)'"
            strSql += vbCrLf + ",(SELECT SUM(GRSWT) FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TRANDATE<'" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' AND COSTID='" & CostId & "'"
            strSql += vbCrLf + "AND (TRFNO IN(SELECT REFNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE>='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' AND TRANTYPE='IIN' AND COSTID='" & CostId & "') OR TRFNO IS NULL))"
            strSql += vbCrLf + ",(SELECT SUM(NETWT) FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TRANDATE<'" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' AND COSTID='" & CostId & "'"
            strSql += vbCrLf + "AND (TRFNO IN(SELECT REFNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE>='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' AND TRANTYPE='IIN' AND COSTID='" & CostId & "') OR TRFNO IS NULL))"
            strSql += vbCrLf + ",(SELECT SUM(DIAWT) FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE TRANDATE<'" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' AND COSTID='" & CostId & "'"
            strSql += vbCrLf + "AND (TRFNO IN(SELECT REFNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE>='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' AND TRANTYPE='IIN' AND COSTID='" & CostId & "') OR TRFNO IS NULL))"
            strSql += vbCrLf + ",2,'',2,'" & CostName & "','MISC ISSUE'"
            strSql += vbCrLf + ",SUM(GRSWT)"
            strSql += vbCrLf + ",SUM(NETWT),SUM(DIAWT),2,''"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "ISSUE WHERE TRANTYPE='MI' AND COSTID='" & CostId & "'"
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + "SELECT 3,'" & CostName & "','CUSTOMER PURCHASE STK (PENDING)',SUM(GRSWT),SUM(NETWT),SUM(DIAWT),3,'',3,'" & CostName & "','PARTSALES ISSUE TO CORP'"
            strSql += vbCrLf + ",(SELECT SUM(GRSWT) FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE COSTID='" & CostId & "'"
            strSql += vbCrLf + "AND TRFNO IN(SELECT REFNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' AND TRANTYPE='IIN' AND COSTID='" & CostId & "')) "
            strSql += vbCrLf + ",(SELECT SUM(NETWT) FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS  WHERE COSTID='" & CostId & "'"
            strSql += vbCrLf + "AND TRFNO IN(SELECT REFNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' AND TRANTYPE='IIN' AND COSTID='" & CostId & "')) "
            strSql += vbCrLf + ",(SELECT SUM(DIAWT) FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS  WHERE COSTID='" & CostId & "'"
            strSql += vbCrLf + "AND TRFNO IN(SELECT REFNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' AND TRANTYPE='IIN' AND COSTID='" & CostId & "')) "
            strSql += vbCrLf + ",3,''"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "CLOSINGRECEIPT WHERE TRANTYPE='PU' AND COSTID='" & CostId & "'"
            strSql += vbCrLf + "AND  TRANDATE<'" & dtpFrom.Value.ToString("yyyy/MM/dd") & "'"
            strSql += vbCrLf + "AND (TRFNO IN(SELECT REFNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE>='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' AND TRANTYPE='IIN' AND COSTID='" & CostId & "') OR TRFNO IS NULL)"
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + "SELECT 4,'" & CostName & "','SALES RETURN STOCK (PENDING)',SUM(GRSWT),SUM(NETWT),SUM(DIAWT),4,'',4,'" & CostName & "','PURCHASE  ISSUE TO CORP'"
            strSql += vbCrLf + ",(SELECT SUM(GRSWT) FROM TEMPTABLEDB..TEMP" & systemId & "RECEIPTPEND WHERE TRANTYPE='PU' AND COSTID='" & CostId & "'"
            strSql += vbCrLf + "AND TRFNO IN(SELECT REFNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' AND TRANTYPE='IIN' AND COSTID='" & CostId & "')) "
            strSql += vbCrLf + ",(SELECT SUM(NETWT) FROM TEMPTABLEDB..TEMP" & systemId & "RECEIPTPEND WHERE TRANTYPE='PU' AND COSTID='" & CostId & "'"
            strSql += vbCrLf + "AND TRFNO IN(SELECT REFNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' AND TRANTYPE='IIN' AND COSTID='" & CostId & "')) "
            strSql += vbCrLf + ",(SELECT SUM(DIAWT) FROM TEMPTABLEDB..TEMP" & systemId & "RECEIPTPEND WHERE TRANTYPE='PU' AND COSTID='" & CostId & "'"
            strSql += vbCrLf + "AND TRFNO IN(SELECT REFNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' AND TRANTYPE='IIN' AND COSTID='" & CostId & "')) "
            strSql += vbCrLf + ",4,''"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "CLOSINGRECEIPT WHERE TRANTYPE='SR' AND COSTID='" & CostId & "'"
            strSql += vbCrLf + "AND  TRANDATE<'" & dtpFrom.Value.ToString("yyyy/MM/dd") & "'"
            strSql += vbCrLf + "AND (TRFNO IN(SELECT REFNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE>='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' AND TRANTYPE='IIN' AND COSTID='" & CostId & "') OR TRFNO IS NULL)"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
            strSql += vbCrLf + "SELECT NULL,'" & CostName & "','OPENING',SUM(IN_GRSWT),SUM(IN_NETWT),SUM(IN_DIAWT),0,'O',NULL,'" & CostName & "',''"
            strSql += vbCrLf + ",NULL,NULL,NULL,0,'O'"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
            strSql += vbCrLf + "WHERE (IN_COSTCENTRE='" & CostName & "' OR  OUT_COSTCENTRE='" & CostName & "') AND ISNULL(IN_SNO,0) BETWEEN 1 AND 4"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
            strSql += vbCrLf + "SELECT NULL,'" & CostName & "','RECEIPT',NULL,NULL,NULL,5,'T',5,'" & CostName & "','SALES RETURN TO CORP'"
            strSql += vbCrLf + ",SUM(GRSWT)"
            strSql += vbCrLf + ",SUM(NETWT),SUM(DIAWT),5,''"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "RECEIPTPEND WHERE TRANTYPE='SR' AND COSTID='" & CostId & "'"
            strSql += vbCrLf + "AND TRFNO IN(SELECT REFNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' AND TRANTYPE='IIN' AND COSTID='" & CostId & "') "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
            strSql += vbCrLf + "SELECT NULL,'','',NULL,NULL,NULL,0,'',NULL,'" & CostName & "','SUB TOTAL'"
            strSql += vbCrLf + ",SUM(OUT_GRSWT),SUM(OUT_NETWT),SUM(OUT_DIAWT),0,'J'"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
            strSql += vbCrLf + "WHERE (IN_COSTCENTRE='" & CostName & "' OR  OUT_COSTCENTRE='" & CostName & "') AND ISNULL(OUT_SNO,0) BETWEEN 1 AND 5"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
            strSql += vbCrLf + "SELECT 5,'" & CostName & "','STOCK RECEIVED FROM HO (TAG & NONTAG)',SUM(RGRSWT),SUM(RNETWT),SUM(RDIAWT),6,'',NULL,'" & CostName & "','CLOSING'"
            strSql += vbCrLf + ",NULL,NULL,NULL,6,'T'"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "ITEMSTK WHERE COSTCENTRE='" & CostName & "'"
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + "SELECT 6,'" & CostName & "','CUSTOMER PURCHASE'"
            strSql += vbCrLf + ",(SELECT SUM(GRSWT) FROM TEMPTABLEDB..TEMP" & systemId & "RECEIPT WHERE TRANTYPE='PU' AND COSTID='" & CostId & "')"
            strSql += vbCrLf + ",(SELECT SUM(NETWT) FROM TEMPTABLEDB..TEMP" & systemId & "RECEIPT WHERE TRANTYPE='PU' AND COSTID='" & CostId & "')"
            strSql += vbCrLf + ",(SELECT SUM(DIAWT) FROM TEMPTABLEDB..TEMP" & systemId & "RECEIPT WHERE TRANTYPE='PU' AND COSTID='" & CostId & "')"
            strSql += vbCrLf + ",7,'',6,'" & CostName & "','CLOSING-TAG/NON TAG STOCK'"
            strSql += vbCrLf + ",SUM(CGRSWT),SUM(CNETWT),SUM(CDIAWT),7,''"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "ITEMSTK WHERE COSTCENTRE='" & CostName & "'"
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + "SELECT 7,'" & CostName & "','SALES RETURN'"
            strSql += vbCrLf + ",(SELECT SUM(GRSWT) FROM TEMPTABLEDB..TEMP" & systemId & "RECEIPT WHERE TRANTYPE='SR' AND COSTID='" & CostId & "')"
            strSql += vbCrLf + ",(SELECT SUM(NETWT) FROM TEMPTABLEDB..TEMP" & systemId & "RECEIPT WHERE TRANTYPE='SR' AND COSTID='" & CostId & "')"
            strSql += vbCrLf + ",(SELECT SUM(DIAWT) FROM TEMPTABLEDB..TEMP" & systemId & "RECEIPT WHERE TRANTYPE='SR' AND COSTID='" & CostId & "')"
            strSql += vbCrLf + ",8,'',7,'" & CostName & "','CLOSING PARTLY SALES'"
            strSql += vbCrLf + ",SUM(GRSWT),SUM(NETWT),SUM(DIAWT),8,''"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "PENDTRANS WHERE COSTID='" & CostId & "'"
            strSql += vbCrLf + "AND  TRANDATE<='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "'"
            strSql += vbCrLf + "AND (TRFNO IN(SELECT REFNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE>'" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' AND TRANTYPE='IIN' AND COSTID='" & CostId & "') OR TRFNO IS NULL)"
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + "SELECT 8,'" & CostName & "','HOME SALES'," & IIf(Grswt = 0, "NULL", Grswt) & "," & IIf(NetWt = 0, "NULL", NetWt) & ""
            strSql += vbCrLf + ",NULL,9,'" & IIf(Grswt = 0, "", "R") & "',8,'" & CostName & "','CLOSING PURCHASE'"
            ''''' FOR SECOND SALES''''
            ''strSql += vbCrLf + ",SUM(GRSWT),SUM(NETWT),SUM(DIAWT)"
            strSql += vbCrLf + ",ISNULL(SUM(GRSWT),0)-ISNULL(SUM(SGRSWT),0),ISNULL(SUM(NETWT),0)-ISNULL(SUM(SNETWT),0),ISNULL(SUM(DIAWT),0)-ISNULL(SUM(SDIAWT),0)"
            ''''' END FOR SECOND SALES''''
            strSql += vbCrLf + ",9,''"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "CLOSINGRECEIPT WHERE COSTID='" & CostId & "' AND TRANTYPE='PU'"
            strSql += vbCrLf + "AND  TRANDATE<='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "'"
            strSql += vbCrLf + "AND (TRFNO IN(SELECT REFNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE>'" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' AND TRANTYPE='IIN' AND COSTID='" & CostId & "') OR TRFNO IS NULL)"
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + "SELECT NULL,'','',NULL,NULL,NULL"
            strSql += vbCrLf + ",10,'',9,'" & CostName & "','CLOSING SALES RETURN'"
            strSql += vbCrLf + ",SUM(GRSWT),SUM(NETWT),SUM(DIAWT)"
            strSql += vbCrLf + ",10,''"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "CLOSINGRECEIPT WHERE TRANTYPE='SR' AND COSTID='" & CostId & "'"
            strSql += vbCrLf + "AND  TRANDATE<='" & dtpFrom.Value.ToString("yyyy/MM/dd") & "'"
            strSql += vbCrLf + "AND (TRFNO IN(SELECT REFNO FROM " & cnStockDb & "..ISSUE WHERE TRANDATE>'" & dtpFrom.Value.ToString("yyyy/MM/dd") & "' AND TRANTYPE='IIN' AND COSTID='" & CostId & "') OR TRFNO IS NULL)"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
            strSql += vbCrLf + "SELECT NULL,'" & CostName & "','SUB TOTAL',SUM(IN_GRSWT),SUM(IN_NETWT),SUM(IN_DIAWT),0,'J',NULL,'" & CostName & "','CLOSING'"
            strSql += vbCrLf + ",SUM(OUT_GRSWT),SUM(OUT_NETWT),SUM(OUT_DIAWT),0,'C'"
            strSql += vbCrLf + "FROM ( "
            strSql += vbCrLf + "SELECT SUM(IN_GRSWT)IN_GRSWT,SUM(IN_NETWT)IN_NETWT,SUM(IN_DIAWT)IN_DIAWT,NULL OUT_GRSWT,NULL OUT_NETWT,NULL OUT_DIAWT "
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
            strSql += vbCrLf + "WHERE (IN_COSTCENTRE='" & CostName & "' OR  OUT_COSTCENTRE='" & CostName & "') AND ISNULL(IN_SNO,0) BETWEEN 5 AND 8 "
            strSql += vbCrLf + "UNION ALL"
            strSql += vbCrLf + "SELECT NULL IN_GRSWT,NULL IN_NETWT,NULL IN_DIAWT,SUM(OUT_GRSWT)OUT_GRSWT,SUM(OUT_NETWT)OUT_NETWT,SUM(OUT_DIAWT)OUT_DIAWT "
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
            strSql += vbCrLf + "WHERE (IN_COSTCENTRE='" & CostName & "' OR  OUT_COSTCENTRE='" & CostName & "') AND ISNULL(OUT_SNO,0) BETWEEN 6 AND 9 "
            strSql += vbCrLf + ")X "
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
            strSql += vbCrLf + "SELECT NULL,'" & CostName & "','" & CostName & " TOTAL',SUM(IN_GRSWT),SUM(IN_NETWT),SUM(IN_DIAWT),15,'S',NULL,'" & CostName & "',''"
            strSql += vbCrLf + ",SUM(OUT_GRSWT),SUM(OUT_NETWT),SUM(OUT_DIAWT),15,'S'"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
            strSql += vbCrLf + "WHERE (IN_COSTCENTRE='" & CostName & "' OR  OUT_COSTCENTRE='" & CostName & "') AND IN_COLHEAD NOT IN ('V','K','O','C','J') AND OUT_COLHEAD NOT IN ('V','K','O','C','J')"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
            strSql += vbCrLf + "SELECT NULL,'',' ',NULL,NULL,NULL,16,'',NULL,'','DIFF IN " & CostName & "'"
            strSql += vbCrLf + ",(IN_GRSWT-OUT_GRSWT)"
            strSql += vbCrLf + ",(IN_NETWT-OUT_NETWT)"
            strSql += vbCrLf + ",(IN_DIAWT-OUT_DIAWT)"
            strSql += vbCrLf + ",16,'G'"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE OUT_RESULT=15  "
            strSql += vbCrLf + "AND (IN_COSTCENTRE='" & CostName & "' OR  OUT_COSTCENTRE='" & CostName & "')"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
        Next
        strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
        strSql += vbCrLf + "SELECT NULL,'','GRAND TOTAL',SUM(IN_GRSWT),SUM(IN_NETWT),SUM(IN_DIAWT),17,'G',NULL,'',''"
        strSql += vbCrLf + ",SUM(OUT_GRSWT),SUM(OUT_NETWT),SUM(OUT_DIAWT),17,'G'"
        strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE IN_COLHEAD='S' AND OUT_COLHEAD='S' AND IN_COLHEAD NOT IN ('V','K','O','C','J') AND OUT_COLHEAD NOT IN ('V','K','O','C','J')"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
        strSql += vbCrLf + "SELECT NULL,'',' ',NULL,NULL,NULL,16,'',NULL,'','DIFF(INW-OUT) '"
        strSql += vbCrLf + ",(IN_GRSWT-OUT_GRSWT)"
        strSql += vbCrLf + ",(IN_NETWT-OUT_NETWT)"
        strSql += vbCrLf + ",(IN_DIAWT-OUT_DIAWT)"
        strSql += vbCrLf + ",18,'G'"
        strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE OUT_RESULT=17  "
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
        strSql += vbCrLf + " SELECT NULL,NULL,'SUMMARY',NULL,NULL,NULL,0,'V',NULL,NULL,NULL"
        strSql += vbCrLf + " ,NULL,NULL,NULL,0,'V' "
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT NULL,NULL,IN_COSTCENTRE,SUM(IN_GRSWT),SUM(IN_NETWT),SUM(IN_DIAWT),0,'K',NULL,NULL OUT_COSTCENTRE,NULL OUT_PARTICULARS"
        strSql += vbCrLf + " ,SUM(OUT_GRSWT),SUM(OUT_NETWT),SUM(OUT_DIAWT),0,'K'"
        strSql += vbCrLf + " FROM ( "
        strSql += vbCrLf + " SELECT IN_COSTCENTRE,IN_GRSWT,IN_NETWT,IN_DIAWT,"
        strSql += vbCrLf + " NULL OUT_GRSWT,NULL OUT_NETWT,NULL OUT_DIAWT "
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
        strSql += vbCrLf + " WHERE IN_COLHEAD='O'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT OUT_COSTCENTRE AS IN_COSTCENTRE,NULL IN_GRSWT,NULL IN_NETWT,NULL IN_DIAWT,"
        strSql += vbCrLf + " OUT_GRSWT,OUT_NETWT,OUT_DIAWT "
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
        strSql += vbCrLf + " WHERE OUT_COLHEAD='C'"
        strSql += vbCrLf + " )X GROUP BY IN_COSTCENTRE"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT NULL,NULL,'TOTAL',SUM(IN_GRSWT),SUM(IN_NETWT),SUM(IN_DIAWT),0,'V',NULL,NULL OUT_COSTCENTRE,NULL OUT_PARTICULARS"
        strSql += vbCrLf + " ,SUM(OUT_GRSWT),SUM(OUT_NETWT),SUM(OUT_DIAWT),0,'V'"
        strSql += vbCrLf + " FROM ( "
        strSql += vbCrLf + " SELECT IN_COSTCENTRE,IN_GRSWT,IN_NETWT,IN_DIAWT,"
        strSql += vbCrLf + " NULL OUT_GRSWT,NULL OUT_NETWT,NULL OUT_DIAWT "
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
        strSql += vbCrLf + " WHERE IN_COLHEAD='O'"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT OUT_COSTCENTRE AS IN_COSTCENTRE,NULL IN_GRSWT,NULL IN_NETWT,NULL IN_DIAWT,"
        strSql += vbCrLf + " OUT_GRSWT,OUT_NETWT,OUT_DIAWT "
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
        strSql += vbCrLf + " WHERE OUT_COLHEAD='C'"
        strSql += vbCrLf + " )X "
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        'strSql = vbCrLf + "  SELECT  SUBSTRING(IN_COSTCENTRE,1,3) AS COSTCENTRE,"
        'strSql += vbCrLf + "  'O/P' OPENING,IN_GRSWT,IN_NETWT,IN_DIAWT,0 AS RESULT"
        'strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE IN_COLHEAD IN ('O') "
        'strSql += vbCrLf + "  UNION ALL"
        'strSql += vbCrLf + "  SELECT  SUBSTRING(IN_COSTCENTRE,1,3) AS COSTCENTRE,"
        'strSql += vbCrLf + "  'INW' OPENING,IN_GRSWT,IN_NETWT,IN_DIAWT,1 AS RESULT"
        'strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE IN_COLHEAD IN ('J') "
        'strSql += vbCrLf + "  UNION ALL"
        'strSql += vbCrLf + "  SELECT  SUBSTRING(IN_COSTCENTRE,1,3) AS COSTCENTRE,"
        'strSql += vbCrLf + "  'C\L' OPENING,IN_GRSWT,IN_NETWT,IN_DIAWT,3 AS RESULT"
        'strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE IN_COLHEAD IN ('C') "
        'strSql += vbCrLf + "  UNION ALL"
        'strSql += vbCrLf + "  SELECT  SUBSTRING(OUT_COSTCENTRE,1,3) AS COSTCENTRE,"
        'strSql += vbCrLf + "  'OUT' OPENING,OUT_GRSWT,OUT_NETWT,OUT_DIAWT,4 AS RESULT"
        'strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE OUT_COLHEAD IN ('J') "
        'strSql += vbCrLf + "  UNION ALL"
        'strSql += vbCrLf + "  SELECT  SUBSTRING(OUT_COSTCENTRE,1,3) AS COSTCENTRE,"
        'strSql += vbCrLf + "  'C/L' OPENING,OUT_GRSWT,OUT_NETWT,OUT_DIAWT,5 AS RESULT"
        'strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE OUT_COLHEAD IN ('C') "
        'strSql += vbCrLf + "  ORDER BY COSTCENTRE,RESULT"

        strSql = vbCrLf + "  SELECT * FROM ("
        strSql += vbCrLf + "  SELECT  SUBSTRING(IN_COSTCENTRE,1,3) AS COSTCENTRE,"
        strSql += vbCrLf + "  'O/P' OPENING,ISNULL(IN_GRSWT,0)IN_GRSWT,ISNULL(IN_NETWT,0)IN_NETWT,ISNULL(IN_DIAWT,0)IN_DIAWT,0 AS RESULT"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE IN_COSTCENTRE='" & HoCostname & "' AND IN_COLHEAD IN ('O')"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT  SUBSTRING(IN_COSTCENTRE,1,3) AS COSTCENTRE,"
        strSql += vbCrLf + "  'INW1' OPENING,ISNULL(SUM(IN_GRSWT),0)IN_GRSWT,ISNULL(SUM(IN_NETWT),0)IN_NETWT,ISNULL(SUM(IN_DIAWT),0)IN_DIAWT,1 AS RESULT"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE IN_COSTCENTRE='" & HoCostname & "' AND IN_PARTICULARS IN ('APPROVAL RECEIPTS','RECEIPT','PURCHASE')"
        strSql += vbCrLf + "  GROUP BY IN_COSTCENTRE"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT  SUBSTRING(IN_COSTCENTRE,1,3) AS COSTCENTRE,"
        strSql += vbCrLf + "  'INW2' OPENING,ISNULL(IN_GRSWT,0)IN_GRSWT,ISNULL(IN_NETWT,0)IN_NETWT,ISNULL(IN_DIAWT,0)IN_DIAWT,2 AS RESULT"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE IN_COSTCENTRE='" & HoCostname & "' AND IN_PARTICULARS IN ('INTERNAL RECEIPTS(MI/OG/SR/PS) FROM BRANCH')"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT  SUBSTRING(OUT_COSTCENTRE,1,3) AS COSTCENTRE,"
        strSql += vbCrLf + "  'OUT1' OPENING,ISNULL(SUM(OUT_GRSWT),0)OUT_GRSWT,ISNULL(SUM(OUT_NETWT),0)OUT_NETWT,ISNULL(SUM(OUT_DIAWT),0)OUT_DIAWT,4 AS RESULT"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE OUT_COSTCENTRE='" & HoCostname & "' AND OUT_PARTICULARS IN ('APPROVAL ISSUE','ISSUES','PURCHASE RETURN')"
        strSql += vbCrLf + "  GROUP BY OUT_COSTCENTRE"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT  SUBSTRING(OUT_COSTCENTRE,1,3) AS COSTCENTRE,"
        strSql += vbCrLf + "  'OUT2' OPENING,ISNULL(SUM(OUT_GRSWT),0)OUT_GRSWT,ISNULL(SUM(OUT_NETWT),0)OUT_NETWT,ISNULL(SUM(OUT_DIAWT),0)OUT_DIAWT,5 AS RESULT"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE OUT_COSTCENTRE='" & HoCostname & "' AND OUT_PARTICULARS IN ('STOCK ISSUE TO BRANCH(TAG/NON TAG)')"
        strSql += vbCrLf + "  GROUP BY OUT_COSTCENTRE"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT  SUBSTRING(OUT_COSTCENTRE,1,3) AS COSTCENTRE,"
        strSql += vbCrLf + "  'C/L' OPENING,ISNULL(OUT_GRSWT,0)OUT_GRSWT,ISNULL(OUT_NETWT,0)OUT_NETWT,ISNULL(OUT_DIAWT,0)OUT_DIAWT,6 AS RESULT"
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE OUT_COSTCENTRE='" & HoCostname & "' AND OUT_COLHEAD IN ('C')"
        For k As Integer = 0 To dtCost.Rows.Count - 1
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT  SUBSTRING(IN_COSTCENTRE,1,3) AS COSTCENTRE,"
            strSql += vbCrLf + "  'O/P' OPENING,ISNULL(IN_GRSWT,0)IN_GRSWT,ISNULL(IN_NETWT,0)IN_NETWT,ISNULL(IN_DIAWT,0)IN_DIAWT,0 AS RESULT"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE IN_COSTCENTRE='" & dtCost.Rows(k)("COSTNAME").ToString & "' AND IN_COLHEAD IN ('O')"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT  SUBSTRING(IN_COSTCENTRE,1,3) AS COSTCENTRE,"
            strSql += vbCrLf + "  'INW1' OPENING,ISNULL(SUM(IN_GRSWT),0)IN_GRSWT,ISNULL(SUM(IN_NETWT),0)IN_NETWT,ISNULL(SUM(IN_DIAWT),0)IN_DIAWT,1 AS RESULT"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE IN_COSTCENTRE='" & dtCost.Rows(k)("COSTNAME").ToString & "' AND IN_PARTICULARS IN ('CUSTOMER PURCHASE','SALES RETURN')"
            strSql += vbCrLf + "  GROUP BY IN_COSTCENTRE"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT  SUBSTRING(IN_COSTCENTRE,1,3) AS COSTCENTRE,"
            strSql += vbCrLf + "  'INW2' OPENING,IN_GRSWT,IN_NETWT,IN_DIAWT,2 AS RESULT"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE IN_COSTCENTRE='" & dtCost.Rows(k)("COSTNAME").ToString & "' AND IN_PARTICULARS IN ('STOCK RECEIVED FROM HO (TAG & NONTAG)')"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT  SUBSTRING(OUT_COSTCENTRE,1,3) AS COSTCENTRE,"
            strSql += vbCrLf + "  'OUT1' OPENING,ISNULL(SUM(OUT_GRSWT),0)OUT_GRSWT,ISNULL(SUM(OUT_NETWT),0)OUT_NETWT,ISNULL(SUM(OUT_DIAWT),0)OUT_DIAWT,4 AS RESULT"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE OUT_COSTCENTRE='" & dtCost.Rows(k)("COSTNAME").ToString & "' AND OUT_PARTICULARS IN ('SALES')"
            strSql += vbCrLf + "  GROUP BY OUT_COSTCENTRE"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT  SUBSTRING(OUT_COSTCENTRE,1,3) AS COSTCENTRE,"
            strSql += vbCrLf + "  'OUT2' OPENING,ISNULL(SUM(OUT_GRSWT),0)OUT_GRSWT,ISNULL(SUM(OUT_NETWT),0)OUT_NETWT,ISNULL(SUM(OUT_DIAWT),0)OUT_DIAWT,5 AS RESULT"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE OUT_COSTCENTRE='" & dtCost.Rows(k)("COSTNAME").ToString & "' AND OUT_PARTICULARS IN ('MISC ISSUE','PARTSALES ISSUE TO CORP','PURCHASE  ISSUE TO CORP','SALES RETURN TO CORP')"
            strSql += vbCrLf + "  GROUP BY OUT_COSTCENTRE"
            strSql += vbCrLf + "  UNION ALL"
            strSql += vbCrLf + "  SELECT  SUBSTRING(OUT_COSTCENTRE,1,3) AS COSTCENTRE,"
            strSql += vbCrLf + "  'C/L' OPENING,ISNULL(OUT_GRSWT,0)OUT_GRSWT,ISNULL(OUT_NETWT,0)OUT_NETWT,ISNULL(OUT_DIAWT,0)OUT_DIAWT,6 AS RESULT"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE OUT_COSTCENTRE='" & dtCost.Rows(k)("COSTNAME").ToString & "' AND OUT_COLHEAD IN ('C')"
        Next
        strSql += vbCrLf + " )X"
        If cnCostId <> cnHOCostId Then
            strSql += vbCrLf + " WHERE COSTCENTRE=SUBSTRING('" & cnCostName & "',1,3) "
        End If
        strSql += vbCrLf + "  ORDER BY COSTCENTRE,RESULT"

        If chkSummary.Checked = True Then
            strSql = "IF OBJECT_ID('TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY','U')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "CREATE TABLE TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY(PARTICULARS VARCHAR(250), COSTNAME VARCHAR(25), "
            strSql += vbCrLf + "GRSWT NUMERIC(15,3), "
            strSql += vbCrLf + "NETWT NUMERIC(15,3), DIAWT NUMERIC(15,3), RESULT INT, COLHEAD VARCHAR(1))"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY"
            strSql += vbCrLf + " SELECT IN_PARTICULARS AS PARTICULARS, IN_COSTCENTRE AS COSTNAME, IN_GRSWT AS GRSWT,IN_NETWT AS NETWT,IN_DIAWT AS DIAWT"
            strSql += vbCrLf + " ,1 AS RESULT,'O' AS COLHEAD"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE IN_COLHEAD = 'O' AND IN_RESULT = 0 "

            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY"
            strSql += vbCrLf + " SELECT 'INWARDS' AS PARTICULARS, IN_COSTCENTRE AS COSTNAME, IN_GRSWT AS GRSWT,IN_NETWT AS NETWT,IN_DIAWT AS DIAWT"
            strSql += vbCrLf + " ,2 AS RESULT,'J' AS COLHEAD"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE IN_COLHEAD = 'J' "

            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY"
            strSql += vbCrLf + " SELECT 'OUTWARDS' AS OUT_PARTICULARS,OUT_COSTCENTRE AS COSTNAME, OUT_GRSWT AS GRSWT,OUT_NETWT AS NETWT,OUT_DIAWT AS DIAWT"
            strSql += vbCrLf + " ,3 AS RESULT, 'J' AS COLHEAD"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT WHERE OUT_COLHEAD = 'J'"

            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY"
            strSql += vbCrLf + " SELECT OUT_PARTICULARS AS PARTICULARS, OUT_COSTCENTRE AS COSTNAME, OUT_GRSWT AS GRSWT,OUT_NETWT AS NETWT,OUT_DIAWT AS DIAWT"
            strSql += vbCrLf + " ,4 AS RESULT, 'C' AS COLHEAD"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT  WHERE OUT_COLHEAD = 'C' AND OUT_RESULT = 0"


            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY"
            strSql += vbCrLf + " SELECT 'OPENING','',SUM(GRSWT),SUM(NETWT),SUM(DIAWT),1 AS RESULT, 'T' AS COLHEAD "
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY"
            strSql += vbCrLf + " WHERE RESULT = 1"

            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY"
            strSql += vbCrLf + " SELECT 'INWARDS','',SUM(GRSWT),SUM(NETWT),SUM(DIAWT),2 AS RESULT, 'T' AS COLHEAD "
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY"
            strSql += vbCrLf + " WHERE RESULT = 2"

            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY"
            strSql += vbCrLf + " SELECT 'OUTWARDS','',SUM(GRSWT),SUM(NETWT),SUM(DIAWT),3 AS RESULT, 'T' AS COLHEAD "
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY"
            strSql += vbCrLf + " WHERE RESULT = 3"

            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY"
            strSql += vbCrLf + " SELECT 'CLOSING','',SUM(GRSWT),SUM(NETWT),SUM(DIAWT),4 AS RESULT, 'T' AS COLHEAD "
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY"
            strSql += vbCrLf + " WHERE RESULT = 4"

            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            Dim dt_vaultSum As New DataTable
            strSql = vbCrLf + " SELECT DISTINCT COSTNAME AS COSTNAME "
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY AS T"
            strSql += vbCrLf + " WHERE T.COSTNAME <> ''"
            dt_vaultSum = GetSqlTable(strSql, cn)
            Dim CostName1 As String = ""
            Dim CostName2 As String = ""
            If dt_vaultSum.Rows.Count > 0 Then
                For i As Integer = 0 To dt_vaultSum.Rows.Count - 1
                    CostName1 += "[" + dt_vaultSum.Rows(i).Item("COSTNAME").ToString() + "GRSWT]@" + "[" + dt_vaultSum.Rows(i).Item("COSTNAME").ToString() + "NETWT]@" + "[" + dt_vaultSum.Rows(i).Item("COSTNAME").ToString() + "DIAWT]@"
                Next
            End If
            CostName1 = CostName1.Replace("@", " NUMERIC(15,3),")

            strSql = "IF OBJECT_ID('TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY_FINAL','U')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY_FINAL"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = "CREATE TABLE TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY_FINAL(PARTICULARS VARCHAR(250),"
            strSql += vbCrLf + " " & CostName1 & "  "
            strSql += vbCrLf + " TGRSWT NUMERIC(15,3),TNETWT NUMERIC(15,3),TDIAWT NUMERIC(15,3),"
            strSql += vbCrLf + " RESULT INT, COLHEAD VARCHAR(1))"
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            'strSql = "SELECT * FROM TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY WHERE PARTICULARS = 'OPENING' ORDER BY RESULT,COLHEAD"
            strSql = "SELECT * FROM TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY ORDER BY RESULT,COLHEAD"
            Dim dt_summaryfinal As New DataTable
            dt_summaryfinal = GetSqlTable(strSql, cn)

            If dt_summaryfinal.Rows.Count > 1 Then
                For i As Integer = 0 To dt_summaryfinal.Rows.Count - 1
                    Dim TempStr As String
                    Dim Tempcount As Integer
                    TempStr = dt_summaryfinal.Rows(i).Item("COLHEAD").ToString
                    CostName2 = "[" + dt_summaryfinal.Rows(i).Item("COSTNAME").ToString() + "GRSWT]," + "[" + dt_summaryfinal.Rows(i).Item("COSTNAME").ToString() + "NETWT]," + "[" + dt_summaryfinal.Rows(i).Item("COSTNAME").ToString() + "DIAWT],"

                    strSql = " SELECT COUNT(*) FROM TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY_FINAL "
                    strSql += vbCrLf + " WHERE PARTICULARS = '" & dt_summaryfinal.Rows(i).Item("PARTICULARS").ToString & "' "
                    Tempcount = objGPack.GetSqlValue(strSql)
                    'INSERTFLAG = "I" And
                    If Tempcount = 0 Then
                        strSql = "INSERT INTO TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY_FINAL (PARTICULARS,"
                        strSql += vbCrLf + " " & CostName2 & " "
                        strSql += vbCrLf + " RESULT,COLHEAD ) "
                        strSql += vbCrLf + " VALUES( "
                        strSql += vbCrLf + " '" & dt_summaryfinal.Rows(i).Item("PARTICULARS") & "', "
                        strSql += vbCrLf + " " & Val(dt_summaryfinal.Rows(i).Item("GRSWT").ToString) & ","
                        strSql += vbCrLf + " " & Val(dt_summaryfinal.Rows(i).Item("NETWT").ToString) & ", "
                        strSql += vbCrLf + " " & Val(dt_summaryfinal.Rows(i).Item("DIAWT").ToString) & " "
                        strSql += vbCrLf + " ,1,'F')"
                        cmd = New OleDbCommand(strSql, cn)
                        cmd.CommandTimeout = 1000
                        cmd.ExecuteNonQuery()
                        'INSERTFLAG = "U"
                    Else
                        If TempStr <> "T" Then
                            strSql = "UPDATE TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY_FINAL SET"
                            strSql += vbCrLf + " " & "[" + dt_summaryfinal.Rows(i).Item("COSTNAME") + "GRSWT]" & " = " & Val(dt_summaryfinal.Rows(i).Item("GRSWT").ToString) & " "
                            strSql += vbCrLf + " ," & "[" + dt_summaryfinal.Rows(i).Item("COSTNAME") + "NETWT]" & " = " & Val(dt_summaryfinal.Rows(i).Item("NETWT").ToString) & " "
                            strSql += vbCrLf + " ," & "[" + dt_summaryfinal.Rows(i).Item("COSTNAME") + "DIAWT]" & " = " & Val(dt_summaryfinal.Rows(i).Item("DIAWT").ToString) & " "
                            'strSql += vbCrLf + " WHERE PARTICULARS = 'OPENING'"
                            strSql += vbCrLf + " WHERE PARTICULARS = '" & dt_summaryfinal.Rows(i).Item("PARTICULARS").ToString & "'"
                            cmd = New OleDbCommand(strSql, cn)
                            cmd.CommandTimeout = 1000
                            cmd.ExecuteNonQuery()
                        Else
                            strSql = "UPDATE TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY_FINAL SET"
                            strSql += vbCrLf + " TGRSWT =  " & Val(dt_summaryfinal.Rows(i).Item("GRSWT").ToString) & " "
                            strSql += vbCrLf + " ,TNETWT = " & Val(dt_summaryfinal.Rows(i).Item("NETWT").ToString) & " "
                            strSql += vbCrLf + " ,TDIAWT = " & Val(dt_summaryfinal.Rows(i).Item("DIAWT").ToString) & " "
                            'strSql += vbCrLf + " WHERE PARTICULARS = 'OPENING'"
                            strSql += vbCrLf + " WHERE PARTICULARS = '" & dt_summaryfinal.Rows(i).Item("PARTICULARS").ToString & "'"
                            cmd = New OleDbCommand(strSql, cn)
                            cmd.CommandTimeout = 1000
                            cmd.ExecuteNonQuery()
                        End If
                    End If
                Next
            End If

            Dim dt_final As New DataTable
            strSql = "SELECT * FROM TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY_FINAL"
            dt_final.Columns.Add("SNO", GetType(Integer))
            dt_final.Columns("SNO").AutoIncrementStep = 1
            dt_final.Columns("SNO").AutoIncrement = True
            'dt_final.Columns("SNO").AutoIncrementSeed = 0
            dt_final.Columns("SNO").AutoIncrementSeed = 1
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt_final)
            gridView.DataSource = dt_final
            funcGridStyle2()
            FuncGridHead2()
            Dim TITLE As String
            TITLE = " VAULT REPORT DATE: " & dtpFrom.Text & ""
            TITLE += IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " : " & chkCmbCostCentre.Text, "")
            lblTitle.Font = New System.Drawing.Font("VERDANA", 9, FontStyle.Bold)
            lblTitle.Text = TITLE
            gridView.Select()
            Exit Sub
        End If

        Dim dtt As DataTable
        dtt = New DataTable
        cmd.CommandTimeout = 1000
        cmd = New OleDbCommand(strSql, cn)
        da = New OleDbDataAdapter(cmd)
        da.Fill(dtt)
        msgTxt = ""
        If dtt.Rows.Count > 0 Then
            For i As Integer = 0 To dtt.Rows.Count - 1
                For j As Integer = 0 To dtt.Columns.Count - 1
                    If dtt.Columns(j).ColumnName = "RESULT" Then Continue For
                    msgTxt = msgTxt + dtt.Rows(i)(j).ToString & " "
                Next
                msgTxt = msgTxt + Environment.NewLine
            Next
        End If

        strSql = "SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "VAULTREPORT "
        strSql += vbCrLf + "  ORDER BY SNO"
        dsGridView = New DataSet
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        da = New OleDbDataAdapter(cmd)
        da.Fill(dsGridView)
        If dsGridView.Tables(0).Rows.Count > 0 Then
            gridView.DataSource = dsGridView.Tables(0)
            funcGridStyle()
            pnlGridHeading.Visible = True
            Dim TITLE As String
            TITLE = " VAULT REPORT DATE: " & dtpFrom.Text & ""
            TITLE += IIf(chkCmbCostCentre.Text <> "" And chkCmbCostCentre.Text <> "ALL", " : " & chkCmbCostCentre.Text, "")
            lblTitle.Font = New System.Drawing.Font("VERDANA", 9, FontStyle.Bold)
            lblTitle.Text = TITLE
            gridView.Select()
        Else
            MsgBox("No Records Found.")
            lblTitle.Text = ""
        End If
        Prop_Sets()
    End Sub
    Private Sub frmTrailBal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmTrailBal_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pnlGridHeading.Visible = False
        ''CostCentre
        strSql = " Select 1 from " & cnAdminDb & "..SoftControl where ctlId = 'CostCentre' and ctlText = 'Y'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            chkCmbCostCentre.Enabled = True
        Else
            chkCmbCostCentre.Enabled = False
        End If
        If cnCostId = cnHOCostId Then
            strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
            strSql += " UNION ALL"
        Else
            strSql = " "
        End If
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        If cnCostId <> cnHOCostId Then
            strSql += " where costid='" & cnCostId & "'"
        End If
        strSql += " ORDER BY RESULT,COSTNAME"
        dtCostCentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCostCentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME")
        If strUserCentrailsed <> "Y" And cnDefaultCostId Then chkCmbCostCentre.Enabled = False

        strSql = " SELECT 'ALL' METALNAME"
        strSql += " UNION ALL"
        strSql += " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        dtMetal = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMetal)
        BrighttechPack.GlobalMethods.FillCombo(ChkCmbMetal, dtMetal, "METALNAME")
        btnNew_Click(Me, New EventArgs)
        dtpFrom.Focus()
        Prop_Gets()
    End Sub
    Private Sub btnExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Export, gridHead)
        End If
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        dtpFrom.Value = GetEntryDate(GetServerDate)
        gridView.DataSource = Nothing
        btnView_Search.Enabled = True
        pnlGridTot.Visible = False
        dtpFrom.Focus()
    End Sub
    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Me.btnNew_Click(Me, New EventArgs)
    End Sub
    Private Sub gridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridView.KeyPress
        If e.KeyChar = Chr(Keys.X) Or e.KeyChar = "x" Then
            Me.btnExcel_Click(Me, New EventArgs)
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            dtpFrom.Focus()
        End If
    End Sub
    Private Sub gridDetailView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            Visible = False
            gridView.Focus()
        End If
    End Sub
    
    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles gridView.Scroll
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            gridHead.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                gridHead.HorizontalScrollingOffset = e.NewValue
                gridHead.Columns("SCROLL").Visible = CType(gridView.Controls(0), HScrollBar).Visible
                gridHead.Columns("SCROLL").Width = CType(gridView.Controls(1), VScrollBar).Width
            Else
                gridHead.Columns("SCROLL").Visible = False
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub
    Function FuncGridHead()
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("IN_SNO~IN_PARTICULARS~IN_GRSWT~IN_NETWT~IN_DIAWT", GetType(String))
            .Columns.Add("OUT_SNO~OUT_PARTICULARS~OUT_GRSWT~OUT_NETWT~OUT_DIAWT", GetType(String))
            .Columns.Add("SCROLL", GetType(String))
            .Columns("IN_SNO~IN_PARTICULARS~IN_GRSWT~IN_NETWT~IN_DIAWT").Caption = "INWARD"
            .Columns("OUT_SNO~OUT_PARTICULARS~OUT_GRSWT~OUT_NETWT~OUT_DIAWT").Caption = "OUTWARD"
            .Columns("SCROLL").Caption = ""
        End With
        gridHead.DataSource = Nothing
        gridHead.DataSource = dtMergeHeader
    End Function

    Function FuncGridHead2() As Integer
        Dim dt_Head As New DataTable
        Dim dtMergeHeader As New DataTable("~MERGEHEADER")
        With dtMergeHeader
            .Columns.Add("SNO~PARTICULARS", GetType(String))
            strSql = "SELECT DISTINCT COSTNAME FROM TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY WHERE COSTNAME <> ''"
            ' Dim dt_Head As New DataTable
            dt_Head = GetSqlTable(strSql, cn)
            If dt_Head.Rows.Count > 0 Then
                For i As Integer = 0 To dt_Head.Rows.Count - 1
                    .Columns.Add(dt_Head.Rows(i).Item("COSTNAME").ToString + "GRSWT~" + dt_Head.Rows(i).Item("COSTNAME").ToString + "NETWT~" + dt_Head.Rows(i).Item("COSTNAME").ToString + "DIAWT", GetType(String))
                Next
            End If
            .Columns.Add("TGRSWT~TNETWT~TDIAWT", GetType(String))
            .Columns.Add("SCROLL", GetType(String))

            .Columns("SNO~PARTICULARS").Caption = "PARTICULARS"
            If dt_Head.Rows.Count > 0 Then
                For i As Integer = 0 To dt_Head.Rows.Count - 1
                    .Columns(dt_Head.Rows(i).Item("COSTNAME").ToString + "GRSWT~" + dt_Head.Rows(i).Item("COSTNAME").ToString + "NETWT~" + dt_Head.Rows(i).Item("COSTNAME").ToString + "DIAWT").Caption = dt_Head.Rows(i).Item("COSTNAME").ToString
                Next
            End If
            .Columns("TGRSWT~TNETWT~TDIAWT").Caption = "TOTAL"
            .Columns("SCROLL").Caption = ""
            gridHead.DataSource = Nothing
            gridHead.DataSource = dtMergeHeader
        End With
        With gridHead
            With .Columns("SNO~PARTICULARS")
                .Width = gridView.Columns("PARTICULARS").Width + gridView.Columns("SNO").Width
                .HeaderText = "PARTICULARS"
            End With
            If dt_Head.Rows.Count > 0 Then
                For i As Integer = 0 To dt_Head.Rows.Count - 1
                    Dim TempWidth As Integer = 0
                    With gridHead.Columns(dt_Head.Rows(i).Item("COSTNAME").ToString + "GRSWT~" + dt_Head.Rows(i).Item("COSTNAME").ToString + "NETWT~" + dt_Head.Rows(i).Item("COSTNAME").ToString + "DIAWT")
                        For d As Integer = 0 To gridView.Columns.Count - 1
                            If gridView.Columns(d).Name.Contains(dt_Head.Rows(i).Item("COSTNAME").ToString) Then
                                TempWidth += Val(gridView.Columns(d).Width.ToString)
                            End If
                        Next
                        .Width = TempWidth
                        .HeaderText = dt_Head.Rows(i).Item("COSTNAME").ToString
                    End With
                Next
            End If
            With .Columns("TGRSWT~TNETWT~TDIAWT")
                .Width = gridView.Columns("TGRSWT").Width + gridView.Columns("TNETWT").Width + gridView.Columns("TDIAWT").Width
                .HeaderText = "TOTAL"
            End With
            With .Columns("SCROLL")
                .Width = 0
                .HeaderText = ""
                .Visible = False
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8.25!, FontStyle.Bold)
        End With
    End Function

    Function funcGridStyle2() As Integer
        With gridView
            For J As Integer = 0 To gridView.Columns.Count - 1
                If .Columns(J).Name.Contains("SNO") Then
                    .Columns(J).SortMode = DataGridViewColumnSortMode.NotSortable
                End If
                If .Columns(J).Name.Contains("PARTICULARS") Then
                    .Columns(J).SortMode = DataGridViewColumnSortMode.NotSortable
                End If
                If .Columns(J).Name.Contains("GRSWT") Then
                    .Columns(J).HeaderText = "GRSWT"
                    .Columns(J).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns(J).SortMode = DataGridViewColumnSortMode.NotSortable
                End If
                If .Columns(J).Name.Contains("NETWT") Then
                    .Columns(J).HeaderText = "NETWT"
                    .Columns(J).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns(J).SortMode = DataGridViewColumnSortMode.NotSortable
                End If
                If .Columns(J).Name.Contains("DIAWT") Then
                    .Columns(J).HeaderText = "DIAWT"
                    .Columns(J).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns(J).SortMode = DataGridViewColumnSortMode.NotSortable
                End If
                If .Columns(J).Name.Contains("RESULT") Then
                    .Columns(J).Visible = False
                End If
                If .Columns(J).Name.Contains("COLHEAD") Then
                    .Columns(J).Visible = False
                End If
            Next
        End With
        With gridView
            Dim dt_row As New DataTable
            strSql = "SELECT DISTINCT COSTNAME FROM TEMPTABLEDB..TEMPVAULT" & systemId & "REPORTSUMMARY WHERE COSTNAME <> ''"
            dt_row = GetSqlTable(strSql, cn)
            For k As Integer = 0 To dt_row.Rows.Count - 1
                For j1 As Integer = 0 To gridView.Columns.Count - 1
                    If .Columns(j1).Name.Contains(dt_row.Rows(k).Item("COSTNAME")) Then
                        If k = 0 Then
                            .Columns(j1).DefaultCellStyle.BackColor = Color.Lavender
                        ElseIf k = 1 Then
                            .Columns(j1).DefaultCellStyle.BackColor = Color.Bisque
                        ElseIf k = 2 Then
                            .Columns(j1).DefaultCellStyle.BackColor = Color.LavenderBlush
                        ElseIf k = 3 Then
                            .Columns(j1).DefaultCellStyle.BackColor = Color.MintCream
                        Else
                            .Columns(j1).DefaultCellStyle.BackColor = Color.Lavender
                        End If
                    End If
                Next
            Next
        End With
    End Function

    Function funcGridStyle() As Integer
        With gridView
            If .Columns.Contains("IN_RESULT") Then .Columns("IN_RESULT").Visible = False
            If .Columns.Contains("OUT_RESULT") Then .Columns("OUT_RESULT").Visible = False
            If .Columns.Contains("IN_COLHEAD") Then .Columns("IN_COLHEAD").Visible = False
            If .Columns.Contains("OUT_COLHEAD") Then .Columns("OUT_COLHEAD").Visible = False
            If .Columns.Contains("IN_COSTCENTRE") Then .Columns("IN_COSTCENTRE").Visible = False
            If .Columns.Contains("OUT_COSTCENTRE") Then .Columns("OUT_COSTCENTRE").Visible = False
            If .Columns.Contains("SNO") Then .Columns("SNO").Visible = False
            For i As Integer = 0 To .ColumnCount - 1
                .Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
            Next
            Dim colhead(1) As String
            colhead(0) = "IN_"
            colhead(1) = "OUT_"
            For i As Integer = 0 To colhead.Length - 1
                With .Columns(colhead(i) & "SNO")
                    .HeaderText = "SNO"
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns(colhead(i) & "PARTICULARS")
                    .HeaderText = "PARTICULARS"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns(colhead(i) & "COSTCENTRE")
                    .HeaderText = "COSTCENTRE"
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
                With .Columns(colhead(i) & "GRSWT")
                    .HeaderText = "GRSWT"
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Format = "#,##0.000"
                End With
                With .Columns(colhead(i) & "NETWT")
                    .HeaderText = "NETWT"
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Format = "#,##0.000"
                End With
                With .Columns(colhead(i) & "DIAWT")
                    .HeaderText = "DIAWT"
                    .Width = 80
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                    .DefaultCellStyle.Format = "#,##0.000"
                End With
            Next
            For Each dgvRow As DataGridViewRow In gridView.Rows
                Select Case dgvRow.Cells("IN_COLHEAD").Value.ToString
                    Case "G"
                        dgvRow.DefaultCellStyle.ForeColor = reportHeadStyle1.ForeColor
                        dgvRow.DefaultCellStyle.Font = reportHeadStyle1.Font
                    Case "T"
                        dgvRow.Cells("IN_PARTICULARS").Style.ForeColor = reportHeadStyle.ForeColor
                        dgvRow.Cells("IN_PARTICULARS").Style.Font = reportHeadStyle.Font
                    Case "H"
                        dgvRow.Cells("IN_PARTICULARS").Style.ForeColor = reportHeadStyle.ForeColor
                        dgvRow.Cells("IN_PARTICULARS").Style.BackColor = reportHeadStyle.BackColor
                        dgvRow.Cells("IN_PARTICULARS").Style.Font = reportHeadStyle.Font
                    Case "S"
                        dgvRow.DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                        dgvRow.DefaultCellStyle.Font = reportSubTotalStyle.Font
                    Case "R"
                        If Val(dgvRow.Cells("IN_GRSWT").Value.ToString) <> 0 Then
                            dgvRow.Cells("IN_PARTICULARS").Style.Font = reportHeadStyle.Font
                            dgvRow.Cells("IN_GRSWT").Style.Font = reportHeadStyle.Font
                            dgvRow.Cells("IN_NETWT").Style.Font = reportHeadStyle.Font
                            dgvRow.Cells("IN_DIAWT").Style.Font = reportHeadStyle.Font
                        End If
                    Case "V"
                        dgvRow.DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                        dgvRow.DefaultCellStyle.Font = reportHeadStyle.Font
                    Case "K"
                        dgvRow.DefaultCellStyle.ForeColor = reportSubTotalStyle1.ForeColor
                        dgvRow.DefaultCellStyle.Font = reportSubTotalStyle1.Font
                    Case "J"
                        dgvRow.DefaultCellStyle.ForeColor = reportSubTotalStyle1.ForeColor
                        dgvRow.DefaultCellStyle.Font = reportSubTotalStyle1.Font
                    Case "O"
                        dgvRow.Cells("IN_PARTICULARS").Value = "SUB TOTAL"
                        dgvRow.DefaultCellStyle.ForeColor = reportSubTotalStyle1.ForeColor
                        dgvRow.DefaultCellStyle.Font = reportSubTotalStyle1.Font
                    Case "C"
                        'dgvRow.Cells("IN_PARTICULARS").Value = "SUB TOTAL"
                        dgvRow.DefaultCellStyle.ForeColor = reportSubTotalStyle1.ForeColor
                        dgvRow.DefaultCellStyle.Font = reportSubTotalStyle1.Font
                End Select
                Select Case dgvRow.Cells("OUT_COLHEAD").Value.ToString
                    Case "G"
                        dgvRow.DefaultCellStyle.ForeColor = reportHeadStyle1.ForeColor
                        dgvRow.DefaultCellStyle.Font = reportHeadStyle1.Font
                    Case "T"
                        dgvRow.Cells("OUT_PARTICULARS").Style.ForeColor = reportHeadStyle.ForeColor
                        dgvRow.Cells("OUT_PARTICULARS").Style.Font = reportHeadStyle.Font
                    Case "H"
                        dgvRow.Cells("OUT_PARTICULARS").Style.ForeColor = reportHeadStyle.ForeColor
                        dgvRow.Cells("OUT_PARTICULARS").Style.BackColor = reportHeadStyle.BackColor
                        dgvRow.Cells("OUT_PARTICULARS").Style.Font = reportHeadStyle.Font
                    Case "S"
                        dgvRow.DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                        dgvRow.DefaultCellStyle.Font = reportSubTotalStyle.Font
                    Case "V"
                        dgvRow.DefaultCellStyle.ForeColor = reportHeadStyle.ForeColor
                        dgvRow.DefaultCellStyle.Font = reportHeadStyle.Font
                    Case "K"
                        dgvRow.DefaultCellStyle.ForeColor = reportSubTotalStyle1.ForeColor
                        dgvRow.DefaultCellStyle.Font = reportSubTotalStyle1.Font
                    Case "J"
                        dgvRow.DefaultCellStyle.ForeColor = reportSubTotalStyle1.ForeColor
                        dgvRow.DefaultCellStyle.Font = reportSubTotalStyle1.Font
                    Case "O"
                        'dgvRow.Cells("OUT_PARTICULARS").Value = "SUB TOTAL"
                        dgvRow.DefaultCellStyle.ForeColor = reportSubTotalStyle1.ForeColor
                        dgvRow.DefaultCellStyle.Font = reportSubTotalStyle1.Font
                    Case "C"
                        dgvRow.Cells("OUT_PARTICULARS").Value = "SUB TOTAL"
                        dgvRow.DefaultCellStyle.ForeColor = reportSubTotalStyle1.ForeColor
                        dgvRow.DefaultCellStyle.Font = reportSubTotalStyle1.Font
                    Case "R"
                        If Val(dgvRow.Cells("OUT_GRSWT").Value.ToString) <> 0 Then
                            dgvRow.Cells("OUT_PARTICULARS").Style.Font = reportHeadStyle.Font
                            dgvRow.Cells("OUT_GRSWT").Style.Font = reportHeadStyle.Font
                            dgvRow.Cells("OUT_NETWT").Style.Font = reportHeadStyle.Font
                            dgvRow.Cells("OUT_DIAWT").Style.Font = reportHeadStyle.Font
                        End If
                End Select
            Next
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8.25!, FontStyle.Bold)
        End With
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
        gridView.Invalidate()
        For Each dgvCol As DataGridViewColumn In gridView.Columns
            dgvCol.Width = dgvCol.Width
        Next
        gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        funcGridHeaderStyle()
        Me.gridView.Refresh()
    End Function
    Function funcGridHeaderStyle() As Integer
        FuncGridHead()
        With gridHead
            With .Columns("IN_SNO~IN_PARTICULARS~IN_GRSWT~IN_NETWT~IN_DIAWT")
                .Width = gridView.Columns("IN_SNO").Width + gridView.Columns("IN_PARTICULARS").Width + gridView.Columns("IN_GRSWT").Width + gridView.Columns("IN_NETWT").Width + gridView.Columns("IN_DIAWT").Width
                .HeaderText = "INWARD"
            End With
            With .Columns("OUT_SNO~OUT_PARTICULARS~OUT_GRSWT~OUT_NETWT~OUT_DIAWT")
                .Width = gridView.Columns("OUT_SNO").Width + gridView.Columns("OUT_PARTICULARS").Width + gridView.Columns("OUT_GRSWT").Width + gridView.Columns("OUT_NETWT").Width + gridView.Columns("OUT_DIAWT").Width
                .HeaderText = "OUTWARD"
            End With
            With .Columns("SCROLL")
                .Width = 0
                .HeaderText = ""
                .Visible = False
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End With
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8.25!, FontStyle.Bold)
        End With
    End Function

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If gridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, strCompanyName, lblTitle.Text, gridView, BrightPosting.GExport.GExportType.Print, gridHead)
        End If
    End Sub

    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        'ResizeToolStripMenuItem.Checked = True
        If gridView.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
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
            If chkSummary.Checked = False Then
                funcGridHeaderStyle()
            Else
                funcGridStyle2()
                FuncGridHead2()
            End If
        End If
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmVaultReport_Properties
        GetChecked_CheckedList(ChkCmbMetal, obj.p_chkCmbMetal)
        SetSettingsObj(obj, Me.Name, GetType(frmVaultReport_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmVaultReport_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmVaultReport_Properties))
        SetChecked_CheckedList(ChkCmbMetal, obj.p_chkCmbMetal, "ALL")
    End Sub

    Private Sub btnSendSMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSendSMS.Click
        'If gridView.Rows.Count > 0 Then
        '    SendMail(ConvertDataGridViewToHTML(gridView), "Vault Report " & dtpFrom.Value.Date.ToString("yyyy-MM-dd"))
        'End If
        If Not MsgBox("Do You want to Send SMS?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then Exit Sub
        strSql = " SELECT 1 FROM SYSDATABASES WHERE NAME='AKSHAYASMSDB'"
        If Val(objGPack.GetSqlValue(strSql)) = 0 Then MsgBox("SMS DB not found ") : Exit Sub
        strSql = " SELECT CHARACTER_MAXIMUM_LENGTH AS A FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_CATALOG='AKSHAYASMSDB' AND TABLE_NAME='SMSDATA' AND COLUMN_NAME='MESSAGES'"
        Dim strColLen As String = objGPack.GetSqlValue(strSql)
        If Val(strColLen & "") < 4000 Then
            strSql = " ALTER TABLE AKSHAYASMSDB..SMSDATA ALTER COLUMN MESSAGES VARCHAR(4000)"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
        End If
        Dim SMSTo As String = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'RPTSMSNO'")
        Dim SMS_To() As String = Split(SMSTo, ",")
        If msgTxt <> "" Then
            For i As Integer = 1 To SMS_To.Length
                If SMS_To(i - 1).ToString = "" Then Continue For
                'If Len(SMS_To(i - 1).ToString) <> 10 Then Continue For
                strSql = vbCrLf + "  INSERT INTO AKSHAYASMSDB..SMSDATA (MOBILENO,MESSAGES,STATUS)"
                strSql += vbCrLf + "  SELECT '" & SMS_To(i - 1).ToString & "','" & msgTxt & "','N'"
                cmd = New OleDbCommand(strSql, cn)
                cmd.ExecuteNonQuery()
            Next
        End If
    End Sub
    Private Sub SendMail(ByVal msgTxt As String, Optional ByVal msgSubject As String = Nothing)
        Dim ToMail As String = ""
        Dim MESSAGE As String = ""
        Dim Attachpath As String = ""
        Dim obj As System.Web.Mail.SmtpMail
        Dim smtpServer As New SmtpClient()
        Dim mail As New MailMessage
        Dim Counter As Integer = 0

        Dim FromId As String = Nothing
        Dim MailServer As String = Nothing
        Dim Password As String = Nothing
        Dim MailTag As String = Nothing
        Try
            FromId = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MAILSERVER'", "CTLTEXT", , ).ToString.ToLower()
            ToMail = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'REPORTING_MAILID'", "CTLTEXT", , ).ToString.ToLower()
            If FromId.Contains("@") = False Or FromId.Contains(".") = False Then
                FromId = objGPack.GetSqlValue("SELECT CTLNAME FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'FROMMAILSERVER'", "CTLNAME", , ).ToString.ToLower()
            End If
            Password = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MAILPASSWORD'", "CTLTEXT", , )
            Password = BrighttechPack.Methods.Decrypt(Password)

            If FromId = "" Then MsgBox("From Mail Id is Empty") : Exit Sub
            If Password = "" Then MsgBox("Mail Password is Empty") : Exit Sub
            If ToMail = "" Then MsgBox("To Mail Id is Empty") : Exit Sub
            Dim TomailIds As String()
            TomailIds = Split(ToMail, ",")

            Dim MailServer1 As String = Nothing
            Dim MailServer2 As String = Nothing
            If FromId.Contains("@") = True Then
                Dim SplitMailServer() As String = Split(FromId, "@")
                If Not SplitMailServer Is Nothing Then
                    MailServer1 = SplitMailServer(0)
                    MailServer2 = Trim(SplitMailServer(1))
                    MailServer2 = "@" & MailServer2
                End If
            End If
            If Trim(MailServer2) = "@gmail.com" Then
                smtpServer.Host = "smtp.gmail.com"
                smtpServer.Port = 587
                smtpServer.EnableSsl = True
            ElseIf Trim(MailServer2) = "@yahoo.com" Then
                smtpServer.Port = 465
                smtpServer.Host = "smtp.mail.yahoo.com"
                smtpServer.EnableSsl = True
            ElseIf Trim(MailServer2) = "@yahoo.co.in" Then
                smtpServer.Port = 587
                smtpServer.Host = "smtp.mail.yahoo.com"
                ' smtpServer.EnableSsl = True
            ElseIf Trim(MailServer2) = "@hotmail.com" Then
                smtpServer.Port = 587
                smtpServer.Host = "smtp.live.com"
                smtpServer.EnableSsl = True
            Else
                smtpServer.Port = Val(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-PORT'", "CTLTEXT", , ).ToString)
                smtpServer.Host = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-HOSTNAME'", "CTLTEXT", , ).ToString
                smtpServer.EnableSsl = IIf(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-SSL'", "CTLTEXT", , ).ToString.ToUpper() = "Y", True, False)
            End If

            mail.From = New MailAddress(FromId)
            mail.To.Add(New MailAddress(ToMail))
            'If TomailIds.Length > 1 Then mail.CC.Add(New MailAddress(TomailIds(1).ToString))
            mail.Subject = IIf(msgSubject <> "", msgSubject, "Daily Report " & dtpFrom.Value.Date.ToString("yyyy-MM-dd"))
            mail.Body = msgTxt
            mail.IsBodyHtml = True
            'If File.Exists(Attachpath) = True Then mail.Attachments.Add(New Attachment(Attachpath))
            smtpServer.Credentials = New Net.NetworkCredential(FromId.Trim.ToString, Password.Trim.ToString)
            smtpServer.Timeout = 400000
            smtpServer.Send(mail)

        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace, MsgBoxStyle.Information, "Brighttech")
        End Try
    End Sub
    Public Function ConvertDataGridViewToHTML(ByVal dgv As DataGridView, Optional ByVal dgvHdr As DataGridView = Nothing)
        Dim html As String = ""
        'If dgvHdr.DataSource <> Nothing Then
        '    If dgvHdr.Columns.Count > 0 Then
        '        html = "<html><body bgcolor=#FFFFFF><table align=center width=100% bgcolor=#FFFFFF>"
        '        html += "<table border=1 align=center id=tblprint style=width: 100%; height: 100%>"
        '        html += "<tr bgcolor=#AF9B60 style=""font-family:Baskerville; font-size:11pt"">"
        '        For i As Integer = 0 To dgv.Columns.Count - 4
        '            html += "<td>" + dgv.Columns(i).HeaderText + "</td>"
        '        Next
        '        html += "</tr>"
        '        html += "</table>"
        '    End If
        'End If
        html += "<html><body bgcolor=#FFFFFF><table align=center width=100% bgcolor=#FFFFFF>"
        html += "<table border=1 align=center id=tblprint style=width: 100%; height: 100%>"
        html += "<tr bgcolor=#AF9B60 style=""font-family:Baskerville; font-size:11pt"">"
        For i As Integer = 0 To dgv.Columns.Count - 1
            If Not dgv.Columns(i).Visible = True Then Continue For
            html += "<td><b>" + dgv.Columns(i).HeaderText + "</b></td>"
        Next
        html += "</tr>"
        For i As Integer = 0 To dgv.Rows.Count - 1
            If dgv.Rows(i).Cells("IN_COLHEAD").Value.ToString = "T" Then
                html += "<tr bgcolor=#F3E5AB style=""font-family:Baskerville; font-size:9pt"">"
            ElseIf dgv.Rows(i).Cells("IN_COLHEAD").Value.ToString = "S" Then
                html += "<tr bgcolor=#F5F5DC style=""font-family:Baskerville; font-size:9pt"">"
            ElseIf dgv.Rows(i).Cells("IN_COLHEAD").Value.ToString = "G" Then
                html += "<tr bgcolor=#AF9B60 style=""font-family:Baskerville; font-size:9pt"">"
            ElseIf dgv.Rows(i).Cells("IN_COLHEAD").Value.ToString = "H" Then
                html += "<tr bgcolor=#AF9B60 style=""font-family:Baskerville; font-size:9pt"">"
            ElseIf dgv.Rows(i).Cells("IN_COLHEAD").Value.ToString = "O" Then
                html += "<tr bgcolor=#AF9B60 style=""font-family:Baskerville; font-size:9pt"">"
            Else
                html += "<tr bgcolor=#F5F5DC style=""font-family:Baskerville; font-size:9pt"">"
            End If
            html += "<tr bgcolor=#F5F5DC style=""font-family:Baskerville; font-size:9pt"">"
            For j As Integer = 0 To dgv.Columns.Count - 1
                If Val(dgv.Rows(i).Cells(j).Value.ToString()) = 0 Then
                    If Not dgv.Rows(i).Cells(j).Visible = True Then Continue For
                    If dgv.Rows(i).Cells(j).Value.ToString = "" Then
                        html += "<td>" + dgv.Rows(i).Cells(j).Value.ToString() + "</td>"
                    Else
                        'If dgv.Rows(i).Cells(j).Style.Font.Bold = True Then
                        '    html += "<td><b>" + dgv.Rows(i).Cells(j).Value.ToString() + "</b></td>"
                        'Else
                        html += "<td>" + dgv.Rows(i).Cells(j).Value.ToString() + "</td>"
                        'End If
                    End If
                Else
                    If Not dgv.Rows(i).Cells(j).Visible = True Then Continue For
                    If dgv.Rows(i).Cells(j).Value.ToString = "" Then
                        html += "<td align=""right"">" + dgv.Rows(i).Cells(j).Value.ToString() + "</td>"
                    Else
                        'If dgv.Rows(i).Cells(j).Style.Font.Bold = True Then
                        '    html += "<td align=""right""><b>" + dgv.Rows(i).Cells(j).Value.ToString() + "</b></td>"
                        'Else
                        html += "<td align=""right"">" + dgv.Rows(i).Cells(j).Value.ToString() + "</td>"
                        'End If
                    End If
                End If
            Next
            html += "</tr>"
        Next
        html += "</table>"
        Return html
    End Function
End Class
Public Class frmVaultReport_Properties
    Private ChkSummary As Boolean = False
    Public Property p_ChkSummary() As Boolean
        Get
            Return ChkSummary
        End Get
        Set(ByVal value As Boolean)
            ChkSummary = value
        End Set
    End Property
    Private chkCmbMetal As New List(Of String)
    Public Property p_chkCmbMetal() As List(Of String)
        Get
            Return chkCmbMetal
        End Get
        Set(ByVal value As List(Of String))
            chkCmbMetal = value
        End Set
    End Property
End Class

