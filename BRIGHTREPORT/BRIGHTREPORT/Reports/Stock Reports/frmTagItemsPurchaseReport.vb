Imports System.Data.OleDb
Public Class frmTagItemsPurchaseReport
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim objGridShower As frmGridDispDia

    Private Sub frmTagItemsPurchaseReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmTagItemsPurchaseReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ACTIVE = 'Y' ORDER BY METALNAME"
        FillCheckedListBox(strSql, chkLstMetal)
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkLstCostCentre.Enabled = True
            chkLstCostCentre.Items.Clear()
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            'FillCheckedListBox(strSql, chkLstCostCentre)
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            For i As Integer = 0 To dt.Rows.Count - 1
                If cnCostName = dt.Rows(i).Item("COSTNAME").ToString And cnDefaultCostId = False Then
                    chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, True)
                Else
                    chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, False)
                End If
            Next
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkLstCostCentre.Enabled = False
        Else
            chkLstCostCentre.Enabled = False
            chkLstCostCentre.Items.Clear()
        End If
        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACTIVE = 'Y' ORDER BY DESIGNERNAME"
        FillCheckedListBox(strSql, chkLstDesigner)
        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' ORDER BY ITEMCTRNAME"
        FillCheckedListBox(strSql, chkLstItemCounter)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        ' chkStockOnly.Checked = True
        ' chkWithStudded.Checked = True
        dtpAsOnDate.Value = GetServerDate()
        dtpAsOnDate.Select()
        Prop_Gets()
    End Sub
    Private Sub chkMetalSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkMetalSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstMetal, chkMetalSelectAll.Checked)
    End Sub

    Private Sub chkCostCentreSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkCostCentreSelectAll.Checked)
    End Sub

    Private Sub chkDesignerSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDesignerSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstDesigner, chkDesignerSelectAll.Checked)
    End Sub

    Private Sub chkItemCounterSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkItemCounterSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstItemCounter, chkItemCounterSelectAll.Checked)
    End Sub

    Private Sub chkItemAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkItemSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstItem, chkItemSelectAll.Checked)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'If rbtDetailed.Checked = True Then
        Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre)
        Dim chkItemCounter As String = GetChecked_CheckedList(chkLstItemCounter)
        Dim chkMetalName As String = GetChecked_CheckedList(chkLstMetal)
        Dim chkItemName As String = GetChecked_CheckedList(chkLstItem)
        Dim chkDesigner As String = GetChecked_CheckedList(chkLstDesigner)
        Dim LocalOutSt As String = ""
        If rbtLocal.Checked Then
            LocalOutSt = "L"
        ElseIf rbtOutstation.Checked Then
            LocalOutSt = "O"
        End If

        'strSql = vbCrLf + "  DECLARE @DETAILED VARCHAR(1)"
        'strSql += vbCrLf + "  DECLARE @STUDDED VARCHAR(1)"
        'strSql += vbCrLf + "  SELECT @STUDDED = '" & IIf(chkWithStudded.Checked, "Y", "N") & "'"
        'strSql += vbCrLf + "  SELECT @DETAILED = '" & IIf(rbtDetailed.Checked, "Y", "N") & "'"
        'strSql += vbCrLf + "  IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPPURTAGVIEW') > 0 DROP TABLE TEMPPURTAGVIEW"
        'strSql += vbCrLf + "  SELECT IM.ITEMNAME AS PARTICULAR,IM.ITEMNAME,T.TAGNO,T.STYLENO,DE.DESIGNERNAME + ' ['+CONVERT(VARCHAR,T.DESIGNERID)+']' AS DESIGNERNAME,P.RECDATE PURDATE"
        'strSql += vbCrLf + "  ,T.TRANINVNO,T.SUPBILLNO"
        'strSql += vbCrLf + "  ,CASE WHEN ISNULL(T.ITEMTYPEID,0) <> 0 THEN (SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID)) ELSE T.PURITY END AS PURITY"
        'strSql += vbCrLf + "  ,T.PCS"
        'strSql += vbCrLf + "  ,CASE WHEN P.PURGRSNET <> 'G' THEN T.GRSWT ELSE P.PURNETWT END AS GOLDWT,P.PURRATE"
        'strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),CASE WHEN P.PURGRSNET <> 'G' THEN T.GRSWT ELSE P.PURNETWT END * P.PURRATE)"
        'strSql += vbCrLf + "  AS GOLDVALUE"
        'strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
        'strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),NULL)DIARATE"
        'strSql += vbCrLf + "  ,(SELECT SUM(PURAMT) FROM " & cnAdminDb & "..PURITEMTAGSTONE WHERE TAGSNO = T.SNO AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAVALUE"
        'strSql += vbCrLf + "  ,(SELECT SUM(PURAMOUNT) FROM " & cnAdminDb & "..PURITEMTAGMISCCHAR WHERE TAGSNO = T.SNO)AS MISVALUE"
        'strSql += vbCrLf + "  ,PURMC,PURTAX,PURVALUE"
        'strSql += vbCrLf + "  ,ISS.AMOUNT + ISNULL(ISS.TAX,0) AS ISSAMOUNT"
        'strSql += vbCrLf + "  ,ISS.TRANDATE AS ISSDATE"
        'strSql += vbCrLf + "  ,CONVERT(vARCHAR(15),T.SNO)SNO,T.ITEMID"
        'strSql += vbCrLf + "  ,T.TAGVAL,CONVERT(INT,1) ITEMORDER,CONVERT(INT,1) RESULT,CONVERT(VARCHAR(3),NULL)COLHEAD"
        'strSql += vbCrLf + "  INTO TEMPPURTAGVIEW"
        'strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAG AS T "
        'strSql += vbCrLf + "  INNER JOIN " & CnAdmindb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        'strSql += vbCrLf + "  LEFT OUTER JOIN " & CnAdmindb & "..DESIGNER AS DE ON DE.DESIGNERID = T.DESIGNERID"
        'strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..PURITEMTAG AS P ON P.TAGSNO = T.SNO"
        'strSql += vbCrLf + "  LEFT OUTER JOIN " & cnStockDb & "..ISSUE AS ISS ON ISS.TAGNO = T.TAGNO AND ISS.ITEMID = T.ITEMID AND ISS.BATCHNO = T.BATCHNO AND ISS.TRANDATE = T.ISSDATE"
        'strSql += vbCrLf + " WHERE T.RECDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
        'If chkCostName <> "" Then strSql += vbCrLf + " AND T.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        'If chkItemCounter <> "" Then strSql += vbCrLf + " AND T.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & chkItemCounter & "))"
        'If chkItemName <> "" Then strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkItemName & "))"
        'If chkMetalName <> "" Then strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        'If chkDesigner <> "" Then strSql += vbCrLf + " AND T.DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & chkDesigner & "))"
        'If txtTranInvNo.Text <> "" Then strSql += vbCrLf + " AND T.TRANINVNO = '" & txtTranInvNo.Text & "'"
        'If chkStockOnly.Checked Then strSql += vbCrLf + " AND T.ISSDATE IS NULL"
        'If Not cnCentStock Then strSql += " AND T.COMPANYID = '" & GetStockCompId() & "'"
        ''strSql += vbCrLf + " AND T.COMPANYID = '" & GetStockCompId() & "'"
        'strSql += vbCrLf + "  "
        'strSql += vbCrLf + "  IF @DETAILED = 'N'"
        'strSql += vbCrLf + "  	BEGIN"
        'strSql += vbCrLf + "  	UPDATE TEMPPURTAGVIEW SET TAGNO = NULL,STYLENO = NULL,DESIGNERNAME = NULL,PCS = NULL,PURRATE = NULL,PURDATE = NULL"
        'strSql += vbCrLf + "  	,TRANINVNO = NULL,SUPBILLNO = NULL,PURITY = NULL,DIARATE = NULL,ISSAMOUNT = NULL,ISSDATE = NULL,SNO = NULL,TAGVAL = NULL"
        'strSql += vbCrLf + "  	END"
        'strSql += vbCrLf + "  "
        'strSql += vbCrLf + "  IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPPURTAG') > 0 DROP TABLE TEMPPURTAG"
        'strSql += vbCrLf + "  SELECT "
        'strSql += vbCrLf + "  PARTICULAR,ITEMNAME,TAGNO,STYLENO,DESIGNERNAME,PURDATE"
        'strSql += vbCrLf + "  ,TRANINVNO,SUPBILLNO,PURITY,PCS"
        'strSql += vbCrLf + "  ,SUM(ISNULL(GOLDWT,0))GOLDWT,PURRATE,SUM(ISNULL(GOLDVALUE,0))GOLDVALUE"
        'strSql += vbCrLf + "  ,SUM(ISNULL(DIAWT,0))DIAWT"
        'strSql += vbCrLf + "  ,DIARATE"
        'strSql += vbCrLf + "  ,SUM(ISNULL(DIAVALUE,0))DIAVALUE"
        'strSql += vbCrLf + "  ,SUM(ISNULL(MISVALUE,0))MISVALUE"
        'strSql += vbCrLf + "  ,SUM(ISNULL(PURMC,0))PURMC,SUM(ISNULL(PURTAX,0))PURTAX,SUM(ISNULL(PURVALUE,0))PURVALUE"
        'strSql += vbCrLf + "  ,ISSAMOUNT,ISSDATE,SNO,ITEMID,TAGVAL,ITEMORDER,RESULT,COLHEAD"
        'strSql += vbCrLf + "  INTO TEMPPURTAG "
        'strSql += vbCrLf + "  FROM TEMPPURTAGVIEW"
        'strSql += vbCrLf + "  GROUP BY"
        'strSql += vbCrLf + "  PARTICULAR,ITEMNAME,TAGNO,STYLENO,DESIGNERNAME,PURDATE"
        'strSql += vbCrLf + "  ,TRANINVNO,SUPBILLNO,PURITY,PCS"
        'strSql += vbCrLf + "  ,PURRATE,DIARATE"
        'strSql += vbCrLf + "  ,ISSAMOUNT,ISSDATE,SNO,ITEMID,TAGVAL,ITEMORDER,RESULT,COLHEAD"
        'strSql += vbCrLf + "  "
        'strSql += vbCrLf + "  "
        'strSql += vbCrLf + "  IF @DETAILED = 'Y' AND @STUDDED = 'Y'"
        'strSql += vbCrLf + "  	BEGIN"
        'strSql += vbCrLf + "  	INSERT INTO TEMPPURTAG(DIAWT,DIARATE,DIAVALUE,ITEMORDER,RESULT,COLHEAD,SNO,ITEMID,TAGVAL,ITEMNAME)"
        'strSql += vbCrLf + "  	SELECT SUM(S.STNWT)DIAWT,S.PURRATE DIARATE,SUM(S.PURAMT)DIAVALUE"
        'strSql += vbCrLf + "  	,2 ITEMORDER,1 RESULT,'DET'COLHEAD,S.TAGSNO AS SNO"
        'strSql += vbCrLf + "  	,P.ITEMID,P.TAGVAL,P.ITEMNAME"
        'strSql += vbCrLf + "  	FROM " & cnAdminDb & "..PURITEMTAGSTONE S"
        'strSql += vbCrLf + "  	INNER JOIN TEMPPURTAG P ON P.SNO = S.TAGSNO"
        'strSql += vbCrLf + "  	INNER JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = S.STNITEMID AND IM.DIASTONE = 'D'"
        'strSql += vbCrLf + "  	GROUP BY S.PURRATE,S.TAGSNO,P.ITEMID,P.TAGVAL,P.DESIGNERNAME,P.ITEMNAME"
        'strSql += vbCrLf + "  	END"
        'strSql += vbCrLf + "  IF @DETAILED = 'Y'"
        'strSql += vbCrLf + "  	BEGIN"
        'strSql += vbCrLf + "  	INSERT INTO TEMPPURTAG(PARTICULAR,ITEMNAME,RESULT,COLHEAD)SELECT DISTINCT ITEMNAME,ITEMNAME,0 RESULT,'T'COLHEAD FROM TEMPPURTAG"
        'strSql += vbCrLf + "  	INSERT INTO TEMPPURTAG(PARTICULAR,ITEMNAME,PCS,GOLDWT,GOLDVALUE,DIAWT,DIAVALUE,MISVALUE,PURMC,PURTAX,PURVALUE,RESULT,COLHEAD)"
        'strSql += vbCrLf + "  	SELECT 'SUB TOTAL',ITEMNAME,SUM(ISNULL(PCS,0)),SUM(ISNULL(GOLDWT,0)),SUM(ISNULL(GOLDVALUE,0)),SUM(ISNULL(DIAWT,0)),SUM(ISNULL(DIAVALUE,0)),SUM(ISNULL(MISVALUE,0)),SUM(ISNULL(PURMC,0)),SUM(ISNULL(PURTAX,0)),SUM(ISNULL(PURVALUE,0))"
        'strSql += vbCrLf + "  	,2 RESULT,'S'COLHEAD"
        'strSql += vbCrLf + "  	FROM TEMPPURTAG WHERE ITEMORDER = 1 GROUP BY ITEMNAME"
        'strSql += vbCrLf + "  	END"
        'strSql += vbCrLf + "  "
        'strSql += vbCrLf + "  INSERT INTO TEMPPURTAG(PARTICULAR,ITEMNAME,PCS,GOLDWT,GOLDVALUE,DIAWT,DIAVALUE,MISVALUE,PURMC,PURTAX,PURVALUE,RESULT,COLHEAD)"
        'strSql += vbCrLf + "  SELECT 'GRAND TOTAL','ZZZZZ',SUM(ISNULL(PCS,0)),SUM(ISNULL(GOLDWT,0)),SUM(ISNULL(GOLDVALUE,0)),SUM(ISNULL(DIAWT,0)),SUM(ISNULL(DIAVALUE,0)),SUM(ISNULL(MISVALUE,0)),SUM(ISNULL(PURMC,0)),SUM(ISNULL(PURTAX,0)),SUM(ISNULL(PURVALUE,0))"
        'strSql += vbCrLf + "  ,3 RESULT,'G'COLHEAD"
        'strSql += vbCrLf + "  FROM TEMPPURTAG WHERE ITEMORDER = 1"

        strSql = vbCrLf + "  DECLARE @DETAILED VARCHAR(1)"
        strSql += vbCrLf + "  DECLARE @STUDDED VARCHAR(1)"
        strSql += vbCrLf + "  SELECT @STUDDED = '" & IIf(chkWithStudded.Checked, "Y", "N") & "'"
        strSql += vbCrLf + "  SELECT @DETAILED = '" & IIf(rbtDetailed.Checked, "Y", "N") & "'"
        strSql += vbCrLf + "  IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPPURTAGVIEW') > 0 DROP TABLE TEMPTABLEDB..TEMPPURTAGVIEW"
        strSql += vbCrLf + "  SELECT IM.ITEMNAME AS PARTICULAR,IM.ITEMNAME,T.TAGNO,T.STYLENO,DE.DESIGNERNAME + ' ['+CONVERT(VARCHAR,T.DESIGNERID)+']' AS DESIGNERNAME,P.RECDATE PURDATE"
        strSql += vbCrLf + "  ,T.TRANINVNO,T.SUPBILLNO"
        strSql += vbCrLf + "  ,CASE WHEN ISNULL(T.ITEMTYPEID,0) <> 0 THEN (SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = (SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID)) ELSE T.PURITY END AS PURITY"
        strSql += vbCrLf + "  ,T.PCS"
        'strSql += vbCrLf + "  ,CASE WHEN P.PURGRSNET <> 'G' THEN T.GRSWT ELSE P.PURNETWT END AS GOLDWT"
        If rbtSummary.Checked = True Then
            strSql += vbCrLf + "  ,'' as MODCAL"
        Else
            strSql += vbCrLf + "  ,P.PURGRSNET as MODCAL"
        End If
        strSql += vbCrLf + "  ,T.GRSWT ,P.PURNETWT as NETWT"
        strSql += vbCrLf + "  ,P.PURRATE"
        'strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),CASE WHEN P.PURGRSNET <> 'G' THEN T.GRSWT ELSE P.PURNETWT END * P.PURRATE) AS GOLDVALUE"
        strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),T.GRSWT  * P.PURRATE)  AS GRSVALUE,CONVERT(NUMERIC(15,2),P.PURNETWT * P.PURRATE)  AS NETVALUE"
        If rbtDetailed.Checked And chkWithStudded.Checked = False Then
            strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..PURITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAWT"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),NULL)DIARATE"
        Else
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,4),NULL)DIAWT"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),NULL)DIARATE"
        End If
        strSql += vbCrLf + "  ,(SELECT SUM(PURAMT) FROM " & cnAdminDb & "..PURITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))AS DIAVALUE"
        strSql += vbCrLf + "  ,(SELECT SUM(PURAMOUNT) FROM " & cnAdminDb & "..PURITEMTAGMISCCHAR WHERE TAGSNO = T.SNO)AS MISVALUE"
        'strSql += vbCrLf + "  ,PURMC,PURTAX,PURVALUE"
        strSql += vbCrLf + "  ,PURMC,PURTAX"
        strSql += vbCrLf + "  ,ISS.AMOUNT + ISNULL(ISS.TAX,0) AS ISSAMOUNT"
        strSql += vbCrLf + "  ,ISS.TRANDATE AS ISSDATE"
        strSql += vbCrLf + "  ,CONVERT(vARCHAR(15),T.SNO)SNO,T.ITEMID"
        strSql += vbCrLf + "  ,T.TAGVAL,CONVERT(INT,1) ITEMORDER,CONVERT(INT,1) RESULT,CONVERT(VARCHAR(3),NULL)COLHEAD"
        strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPPURTAGVIEW"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAG AS T "
        strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = T.ITEMID"
        strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..DESIGNER AS DE ON DE.DESIGNERID = T.DESIGNERID"
        strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..PURITEMTAG AS P ON P.TAGSNO = T.SNO"
        strSql += vbCrLf + "  LEFT OUTER JOIN " & cnStockDb & "..ISSUE AS ISS ON ISS.TAGNO = T.TAGNO AND ISS.ITEMID = T.ITEMID AND ISS.BATCHNO = T.BATCHNO AND ISS.TRANDATE = T.ISSDATE"
        'strSql += vbCrLf + "  LEFT OUTER JOIN " & cnAdminDb & "..ACHEAD AS AC ON DE.ACCODE = AC.ACCODE"
        strSql += vbCrLf + " WHERE T.RECDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
        If chkCostName <> "" Then strSql += vbCrLf + " AND T.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkItemCounter <> "" Then strSql += vbCrLf + " AND T.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & chkItemCounter & "))"
        If chkItemName <> "" Then strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkItemName & "))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkDesigner <> "" Then strSql += vbCrLf + " AND T.DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & chkDesigner & "))"
        If txtTranInvNo.Text <> "" Then strSql += vbCrLf + " AND T.TRANINVNO = '" & txtTranInvNo.Text & "'"
        If chkStockOnly.Checked Then strSql += vbCrLf + " AND T.ISSDATE IS NULL"
        If Not cnCentStock Then strSql += " AND T.COMPANYID = '" & GetStockCompId() & "'"
        If LocalOutSt <> "" Then strSql += vbCrLf + " AND DE.ACCODE IN (SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE LOCALOUTST ='" & LocalOutSt & "')"
        'strSql += vbCrLf + " AND T.COMPANYID = '" & GetStockCompId() & "'"
        strSql += vbCrLf + "  "
        strSql += vbCrLf + "  IF @DETAILED = 'N'"
        strSql += vbCrLf + "  	BEGIN"
        strSql += vbCrLf + "  	UPDATE TEMPTABLEDB..TEMPPURTAGVIEW SET TAGNO = NULL,STYLENO = NULL,DESIGNERNAME = NULL,PCS = NULL,PURRATE = NULL,PURDATE = NULL"
        strSql += vbCrLf + "  	,TRANINVNO = NULL,SUPBILLNO = NULL,PURITY = NULL,DIARATE = NULL,ISSAMOUNT = NULL,ISSDATE = NULL,SNO = NULL,TAGVAL = NULL"
        strSql += vbCrLf + "  	END"
        strSql += vbCrLf + "  "
        strSql += vbCrLf + "  IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPPURTAG') > 0 DROP TABLE TEMPTABLEDB..TEMPPURTAG"
        strSql += vbCrLf + "  SELECT "
        strSql += vbCrLf + "  PARTICULAR, ITEMNAME, TAGNO, STYLENO, DESIGNERNAME, PURDATE"
        strSql += vbCrLf + "  ,TRANINVNO,SUPBILLNO,PURITY,PCS"
        strSql += vbCrLf + "  ,MODCAL"
        strSql += vbCrLf + "  ,Sum(GRSWT) as GRSWT "
        strSql += vbCrLf + "  ,sum(NETWT) as NETWT"
        strSql += vbCrLf + "  ,PURRATE  "
        strSql += vbCrLf + "  ,SUM(GRSVALUE) as GRSVALUE"
        strSql += vbCrLf + "  ,Sum(NEtVALUE) as NetVALUE"
        strSql += vbCrLf + "  ,SUM(ISNULL(DIAWT,0))DIAWT"
        strSql += vbCrLf + "  ,DIARATE"
        strSql += vbCrLf + "  ,SUM(ISNULL(DIAVALUE,0))DIAVALUE"
        strSql += vbCrLf + "  ,SUM(ISNULL(MISVALUE,0))MISVALUE"
        strSql += vbCrLf + "  ,SUM(ISNULL(PURMC,0))PURMC,SUM(ISNULL(PURTAX,0))PURTAX"
        strSql += vbCrLf + "  ,ISSAMOUNT,ISSDATE,SNO,ITEMID,TAGVAL,ITEMORDER,RESULT,COLHEAD"
        strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPPURTAG "
        strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPPURTAGVIEW"
        strSql += vbCrLf + "  GROUP BY"
        strSql += vbCrLf + "  PARTICULAR,ITEMNAME,TAGNO,STYLENO,DESIGNERNAME,PURDATE"
        strSql += vbCrLf + "  ,TRANINVNO,SUPBILLNO,PURITY,PCS"
        strSql += vbCrLf + "  ,PURRATE,DIARATE"
        strSql += vbCrLf + "  ,ISSAMOUNT,ISSDATE,SNO,ITEMID,TAGVAL,ITEMORDER,RESULT,COLHEAD,MODCAL"
        strSql += vbCrLf + "  "
        strSql += vbCrLf + "  "
        strSql += vbCrLf + "  IF @DETAILED = 'Y' AND @STUDDED = 'Y'"
        strSql += vbCrLf + "  	BEGIN"
        strSql += vbCrLf + "  	INSERT INTO TEMPTABLEDB..TEMPPURTAG(PARTICULAR,DIAWT,DIARATE,DIAVALUE,ITEMORDER,RESULT,COLHEAD,SNO,ITEMID,TAGVAL,ITEMNAME)"
        strSql += vbCrLf + "  	SELECT CASE WHEN ISNULL(SI.SUBITEMNAME,'')<>'' THEN SI.SUBITEMNAME ELSE IM.ITEMNAME END  AS PARTICULAR,SUM(S.STNWT)DIAWT,NULL DIARATE,SUM(S.PURAMT)DIAVALUE"
        strSql += vbCrLf + "  	,2 ITEMORDER,1 RESULT,'DET'COLHEAD,S.TAGSNO AS SNO"
        strSql += vbCrLf + "  	,P.ITEMID,P.TAGVAL,P.ITEMNAME"
        strSql += vbCrLf + "  	FROM " & cnAdminDb & "..PURITEMTAGSTONE S"
        strSql += vbCrLf + "  	INNER JOIN TEMPTABLEDB..TEMPPURTAG P ON P.SNO = S.TAGSNO"
        strSql += vbCrLf + "  	INNER JOIN " & cnAdminDb & "..ITEMMAST IM ON IM.ITEMID = S.STNITEMID AND IM.DIASTONE = 'D'"
        strSql += vbCrLf + "  	INNER JOIN " & cnAdminDb & "..SUBITEMMAST SI ON SI.SUBITEMID = S.STNSUBITEMID "
        strSql += vbCrLf + "  	GROUP BY S.TAGSNO,P.ITEMID,P.TAGVAL,P.DESIGNERNAME,P.ITEMNAME,IM.ITEMNAME,SI.SUBITEMNAME"
        strSql += vbCrLf + "  	END"
        strSql += vbCrLf + "  IF @DETAILED = 'Y'"
        strSql += vbCrLf + "  	BEGIN"
        strSql += vbCrLf + "  	INSERT INTO TEMPTABLEDB..TEMPPURTAG(PARTICULAR,ITEMNAME,RESULT,COLHEAD)SELECT DISTINCT ITEMNAME,ITEMNAME,0 RESULT,'T'COLHEAD FROM TEMPTABLEDB..TEMPPURTAG"
        strSql += vbCrLf + "  	INSERT INTO TEMPTABLEDB..TEMPPURTAG(PARTICULAR,ITEMNAME,PCS,GRSWT,NETWT,GRSVALUE,NETVALUE,DIAWT,DIAVALUE,MISVALUE,PURMC,PURTAX,RESULT,COLHEAD)"
        strSql += vbCrLf + "  	SELECT 'SUB TOTAL',ITEMNAME,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(NETWT,0)),SUM(ISNULL(GRSVALUE,0)),SUM(ISNULL(NETVALUE,0)),SUM(ISNULL(DIAWT,0)),SUM(ISNULL(DIAVALUE,0)),SUM(ISNULL(MISVALUE,0)),SUM(ISNULL(PURMC,0)),SUM(ISNULL(PURTAX,0))"
        strSql += vbCrLf + "  	,2 RESULT,'S'COLHEAD"
        strSql += vbCrLf + "  	FROM TEMPTABLEDB..TEMPPURTAG WHERE ITEMORDER = 1 GROUP BY ITEMNAME"
        strSql += vbCrLf + "  	END"
        strSql += vbCrLf + "  "
        If rbtSummary.Checked = True Then
            strSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMPPURTAG(PARTICULAR,ITEMNAME,PCS,GRSWT,NETWT,GRSVALUE,NETVALUE,DIAWT,DIAVALUE,MISVALUE,PURMC,PURTAX,RESULT,COLHEAD,MODCAL)"
            strSql += vbCrLf + "  SELECT 'GRAND TOTAL','ZZZZZ',SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(NETWT,0)),SUM(ISNULL(GRSVALUE,0)),SUM(ISNULL(NETVALUE,0)),SUM(ISNULL(DIAWT,0)),SUM(ISNULL(DIAVALUE,0)),SUM(ISNULL(MISVALUE,0)),SUM(ISNULL(PURMC,0)),SUM(ISNULL(PURTAX,0)) "
            strSql += vbCrLf + "  ,3 RESULT,'G'COLHEAD,' ' "
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPPURTAG WHERE ITEMORDER = 1"

        Else
            strSql += vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMPPURTAG(PARTICULAR,ITEMNAME,PCS,GRSWT,NETWT,GRSVALUE,NETVALUE,DIAWT,DIAVALUE,MISVALUE,PURMC,PURTAX,RESULT,COLHEAD)"
            strSql += vbCrLf + "  SELECT 'GRAND TOTAL','ZZZZZ',SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(NETWT,0)),SUM(ISNULL(GRSVALUE,0)),SUM(ISNULL(NETVALUE,0)),SUM(ISNULL(DIAWT,0)),SUM(ISNULL(DIAVALUE,0)),SUM(ISNULL(MISVALUE,0)),SUM(ISNULL(PURMC,0)),SUM(ISNULL(PURTAX,0)) "
            strSql += vbCrLf + "  ,3 RESULT,'G'COLHEAD"
            strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMPPURTAG WHERE ITEMORDER = 1"

        End If


        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMPTABLEDB..TEMPPURTAG SET"
        strSql += "  PCS = CASE WHEN PCS = 0 THEN NULL ELSE PCS END"
        strSql += " ,GRSWT = CASE WHEN GRSWT = 0 THEN NULL ELSE GRSWT END"
        strSql += " ,NETWT = CASE WHEN NETWT = 0 THEN NULL ELSE NETWT END"
        strSql += " ,GRSVALUE = CASE WHEN GRSVALUE = 0 THEN NULL ELSE GRSVALUE END"
        strSql += " ,NETVALUE = CASE WHEN NETVALUE = 0 THEN NULL ELSE NETVALUE END"
        strSql += " ,DIAWT = CASE WHEN DIAWT = 0 THEN NULL ELSE DIAWT END"
        strSql += " ,DIARATE = CASE WHEN DIARATE = 0 THEN NULL ELSE DIARATE END"
        strSql += " ,DIAVALUE = CASE WHEN DIAVALUE = 0 THEN NULL ELSE DIAVALUE END"
        strSql += " ,MISVALUE = CASE WHEN MISVALUE = 0 THEN NULL ELSE MISVALUE END"
        strSql += " ,PURMC = CASE WHEN PURMC = 0 THEN NULL ELSE PURMC END"
        strSql += " ,PURTAX = CASE WHEN PURTAX = 0 THEN NULL ELSE PURTAX END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = vbCrLf + "  SELECT * FROM TEMPTABLEDB..TEMPPURTAG AS T"
        strSql += vbCrLf + "  ORDER BY ITEMNAME,RESULT,TAGVAL,ITEMORDER"


        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If

        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Size = New Size(778, 550)
        objGridShower.Text = "TAGED ITEMS PURCHASE REPORT"
        Dim tit As String = "TAGED ITEMS PURCHASE REPORT" + IIf(chkMetalName <> "", chkMetalName.Replace("'", ""), " ALL METAL") + vbCrLf
        tit += "AS ON DATE  " + dtpAsOnDate.Text
        Dim Cname As String = ""
        If chkLstCostCentre.Items.Count > 0 And chkLstCostCentre.CheckedItems.Count > 0 Then
            For CNT As Integer = 0 To chkLstCostCentre.Items.Count - 1
                If chkLstCostCentre.GetItemChecked(CNT) = True Then
                    Cname += "" & chkLstCostCentre.Items(CNT) + ","
                End If
            Next
            If Cname <> "" Then Cname = " :" + Mid(Cname, 1, Len(Cname) - 1)
        End If
        objGridShower.lblTitle.Text = tit & Cname
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.pnlFooter.Visible = False
        objGridShower.FormReLocation = False
        objGridShower.FormReSize = False
        objGridShower.Show()
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        DataGridView_Detailed(objGridShower.gridView)
        objGridShower.FormReSize = True
        objGridShower.gridViewHeader.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle = objGridShower.gridView.ColumnHeadersDefaultCellStyle


        'Else
        'Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre)
        'Dim chkItemCounter As String = GetChecked_CheckedList(chkLstItemCounter)
        'Dim chkMetalName As String = GetChecked_CheckedList(chkLstMetal)
        'Dim chkItemName As String = GetChecked_CheckedList(chkLstItem)
        'Dim chkDesigner As String = GetChecked_CheckedList(chkLstDesigner)
        'strSql = " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "TAGPURDET')>0 DROP TABLE TEMP" & systemId & "TAGPURDET"
        'STRSQL += VBCRLF + " SELECT CONVERT(VARCHAR(60),PARTICULAR)PARTICULAR,TAGNO,STYLENO,PURVALUE + PURTAX TOTAL,PCS,NETWT,PURVALUE-STNAMT-DIAAMT-PURMC AS NETAMT,CONVERT(NUMERIC(15,4),DIAWT)DIAWT,CONVERT(NUMERIC(15,2),DIAAMT)DIAAMT,CONVERT(NUMERIC(15,3),STNWT)STNWT"
        'STRSQL += VBCRLF + " ,CONVERT(NUMERIC(15,2),STNAMT)STNAMT,CONVERT(NUMERIC(15,2),PURMC)PURMC,CONVERT(NUMERIC(15,2),PURVALUE) GRANDTOTAL,CONVERT(NUMERIC(15,2),((PURVALUE * PURTAX)/100)) PURTAX,TRANINVNO,SUPBILLNO,ITEM,1 RESULT,' 'COLHEAD,CONVERT(INT,TAGVAL)TAGVAL"
        'STRSQL += VBCRLF + " INTO TEMP" & systemId & "TAGPURDET"
        'STRSQL += VBCRLF + " FROM"
        'STRSQL += VBCRLF + " ("
        'STRSQL += VBCRLF + " SELECT "
        'STRSQL += VBCRLF + " (SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS PARTICULAR"
        'STRSQL += VBCRLF + " ,TAGNO,STYLENO"
        'STRSQL += VBCRLF + " ,PCS,NETWT"
        'STRSQL += VBCRLF + " ,ISNULL((SELECT SUM(ISNULL(STNWT,0)) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)AS DIAWT"
        'STRSQL += VBCRLF + " ,ISNULL((SELECT SUM(ISNULL(PURAMT,0)) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')),0)AS DIAAMT"
        'STRSQL += VBCRLF + " ,ISNULL((SELECT SUM(ISNULL(STNWT,0)) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')),0)AS STNWT"
        'STRSQL += VBCRLF + " ,ISNULL((SELECT SUM(ISNULL(PURAMT,0)) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')),0)AS STNAMT"
        'STRSQL += VBCRLF + " ,ISNULL(PURMC,0)PURMC"
        'STRSQL += VBCRLF + " ,ISNULL(PURVALUE,0)PURVALUE"
        'STRSQL += VBCRLF + " ,ISNULL((SELECT PTAX FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)),0)PURTAX"
        'STRSQL += VBCRLF + " ,TRANINVNO,SUPBILLNO"
        'STRSQL += VBCRLF + " ,CASE WHEN ISSDATE IS NOT NULL THEN 'DATE : ' + CONVERT(VARCHAR,ISSDATE,103) + ';PCS : ' + CONVERT(VARCHAR,ISSPCS) + ';WEIGHT : ' + CONVERT(VARCHAR,ISSWT) ELSE '' END AS SALESDET"
        'STRSQL += VBCRLF + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
        'STRSQL += VBCRLF + " ,TAGVAL"
        'STRSQL += VBCRLF + " FROM " & cnAdminDb & "..ITEMTAG AS T"
        'STRSQL += VBCRLF + " WHERE RECDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "'"
        'If chkCostName <> "" Then STRSQL += VBCRLF + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        'If chkItemCounter <> "" Then STRSQL += VBCRLF + " AND ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & chkItemCounter & "))"
        'If chkItemName <> "" Then STRSQL += VBCRLF + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkItemName & "))"
        'If chkMetalName <> "" Then STRSQL += VBCRLF + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        'If chkDesigner <> "" Then STRSQL += VBCRLF + " AND DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & chkDesigner & "))"
        'If txtTranInvNo.Text <> "" Then STRSQL += VBCRLF + " AND TRANINVNO = '" & txtTranInvNo.Text & "'"
        'If chkStockOnly.Checked Then STRSQL += VBCRLF + " AND ISSDATE IS NULL"
        'STRSQL += VBCRLF + " AND COMPANYID = '" & GetStockCompId() & "'"
        'STRSQL += VBCRLF + " )X"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()
        'strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "TAGPURDET)>0"
        'STRSQL += VBCRLF + " BEGIN"
        'STRSQL += VBCRLF + " INSERT INTO TEMP" & systemId & "TAGPURDET(ITEM,PARTICULAR,RESULT,COLHEAD)"
        'STRSQL += VBCRLF + " SELECT DISTINCT ITEM,ITEM,0 RESULT,'T'COLHEAD FROM TEMP" & systemId & "TAGPURDET"
        'STRSQL += VBCRLF + " INSERT INTO TEMP" & systemId & "TAGPURDET(ITEM,PARTICULAR,RESULT,COLHEAD,TOTAL,PCS,NETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,PURTAX)"
        'STRSQL += VBCRLF + " SELECT DISTINCT ITEM,ITEM,2 RESULT,'S'COLHEAD"
        'STRSQL += VBCRLF + " ,SUM(TOTAL)TOTAL,SUM(PCS)PCS,SUM(NETWT),SUM(NETAMT),SUM(DIAWT),SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(PURTAX)"
        'STRSQL += VBCRLF + " FROM TEMP" & systemId & "TAGPURDET GROUP BY ITEM"
        'STRSQL += VBCRLF + " INSERT INTO TEMP" & systemId & "TAGPURDET(ITEM,PARTICULAR,RESULT,COLHEAD,TOTAL,PCS,NETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,PURTAX)"
        'STRSQL += VBCRLF + " SELECT DISTINCT 'ZZZZZ','GRAND TOTAL',3 RESULT,'G'COLHEAD"
        'STRSQL += VBCRLF + " ,SUM(TOTAL)TOTAL,SUM(PCS)PCS,SUM(NETWT),SUM(NETAMT),SUM(DIAWT),SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(PURTAX)"
        'STRSQL += VBCRLF + " FROM TEMP" & systemId & "TAGPURDET WHERE RESULT = 2"
        'STRSQL += VBCRLF + " END"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()
        'strSql = " SELECT PARTICULAR,TOTAL,PCS,NETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,PURTAX ,COLHEAD"
        'STRSQL += VBCRLF + " FROM TEMP" & systemId & "TAGPURDET where result <> '1' and result <> '0' ORDER BY ITEM,RESULT,TAGVAL"


        ''strSql = " SELECT * FROM TEMP" & systemId & "TAGPURDET ORDER BY ITEM,RESULT,TAGVAL"
        'Dim dtGrid As New DataTable
        'dtGrid.Columns.Add("KEYNO", GetType(Integer))
        'dtGrid.Columns("KEYNO").AutoIncrement = True
        'dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        'dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtGrid)
        'dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        'If Not dtGrid.Rows.Count > 0 Then
        '    MsgBox("Record not found", MsgBoxStyle.Information)
        '    Exit Sub
        'End If

        'objGridShower = New frmGridDispDia
        'objGridShower.Name = Me.Name
        'objGridShower.gridView.RowTemplate.Height = 21
        'objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        'objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        'objGridShower.Size = New Size(778, 550)
        'objGridShower.Text = "TAGED ITEMS PURCHASE REPORT"
        'Dim tit As String = "TAGED ITEMS PURCHASE REPORT" + IIf(chkMetalName <> "", chkMetalName.Replace("'", ""), " ALL METAL") + vbCrLf
        'tit += "AS ON DATE  " + dtpAsOnDate.Text
        'objGridShower.lblTitle.Text = tit
        'objGridShower.StartPosition = FormStartPosition.CenterScreen
        'objGridShower.dsGrid.DataSetName = objGridShower.Name
        'objGridShower.dsGrid.Tables.Add(dtGrid)
        'objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        'objGridShower.pnlFooter.Visible = False
        'objGridShower.FormReLocation = False
        'objGridShower.FormReSize = False
        'objGridShower.Show()
        'FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        'DataGridView_Summary(objGridShower.gridView)
        'objGridShower.FormReSize = True
        'objGridShower.gridViewHeader.Visible = False
        'objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle = objGridShower.gridView.ColumnHeadersDefaultCellStyle
        'End If
        Prop_Sets()
    End Sub
    Private Sub DataGridView_Detailed(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("ITEMNAME").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("ITEMORDER").Visible = False
            .Columns("TRANINVNO").Visible = False
            .Columns("SNO").Visible = False
            .Columns("ITEMID").Visible = False
            .Columns("TAGVAL").Visible = False
            .Columns("DESIGNERNAME").HeaderText = "DESIGNER"
            .Columns("PARTICULAR").Width = 175
            .Columns("TAGNO").Width = 70
            .Columns("STYLENO").Width = 70
            .Columns("DESIGNERNAME").Width = 130
            .Columns("PURDATE").Width = 80
            .Columns("SUPBILLNO").Width = 90
            .Columns("PURITY").Width = 60
            .Columns("MODCAL").Width = 30
            .Columns("MODCAL").HeaderText = "MOD"
            .Columns("GRSWT").Width = 60
            .Columns("NETWT").Width = 60
            .Columns("PURRATE").Width = 65
            .Columns("GRSVALUE").Width = 100
            .Columns("NetVALUE").Width = 100
            .Columns("GRSVALUE").HeaderText = "AMOUNT"
            .Columns("NetVALUE").HeaderText = "AMOUNT"
            .Columns("DIAWT").Width = 70
            .Columns("DIARATE").Width = 80
            .Columns("DIAVALUE").Width = 100
            .Columns("MISVALUE").Width = 80
            .Columns("PURMC").Width = 80
            .Columns("PURTAX").Width = 80
            '.Columns("PURVALUE").Width = 100


            .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            .Columns("GRSVALUE").Visible = Rbgwt.Checked
            .Columns("NetVALUE").Visible = rbnwt.Checked
            .Columns("DESIGNERNAME").Visible = rbtDetailed.Checked
            .Columns("MODCAL").Visible = rbtDetailed.Checked
            .Columns("PARTICULAR").Visible = rbtDetailed.Checked
            .Columns("TAGNO").Visible = rbtDetailed.Checked
            .Columns("STYLENO").Visible = rbtDetailed.Checked
            .Columns("DESIGNERNAME").Visible = rbtDetailed.Checked
            .Columns("PURDATE").Visible = rbtDetailed.Checked
            .Columns("SUPBILLNO").Visible = rbtDetailed.Checked
            .Columns("PURITY").Visible = rbtDetailed.Checked
            .Columns("PCS").Visible = rbtDetailed.Checked
            .Columns("PCS").Width = 60
            '.Columns("GOLDWT").Width = 80
            .Columns("PURRATE").Visible = rbtDetailed.Checked
            '.Columns("GOLDVALUE").Width = 100
            .Columns("DIAWT").Width = 70
            .Columns("DIARATE").Visible = rbtDetailed.Checked
            .Columns("DIAVALUE").Width = 100
            .Columns("MISVALUE").Width = 80
            .Columns("PURMC").Width = 80
            .Columns("PURTAX").Width = 80
            '.Columns("PURVALUE").Width = 100



            '.Columns("PARTICULAR").Width = 200
            '.Columns("TAGNO").Width = 75
            '.Columns("STYLENO").Width = 60
            '.Columns("TOTAL").Width = 100
            '.Columns("PCS").Width = 70
            '.Columns("NETWT").Width = 80
            '.Columns("NETAMT").Width = 90
            '.Columns("DIAWT").Width = 80
            '.Columns("DIAAMT").Width = 100
            '.Columns("STNWT").Width = 80
            '.Columns("STNAMT").Width = 100
            '.Columns("PURMC").Width = 80
            '.Columns("GRANDTOTAL").Width = 100
            '.Columns("PURTAX").Width = 70
            '.Columns("TRANINVNO").Width = 100
            '.Columns("SUPBILLNO").Width = 100

            '.Columns("STYLENO").Visible = rbtDetailed.Checked



            '.Columns("RESULT").Visible = False
            '.Columns("COLHEAD").Visible = False
            '.Columns("ITEM").Visible = False
            '.Columns("KEYNO").Visible = False
            '.Columns("TAGVAL").Visible = False
            FormatGridColumns(dgv, False, False, , False)
            .Columns("ISSDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("PURDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            '.Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            '.Columns("STNWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
        End With
    End Sub
    Private Sub DataGridView_Summary(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            '.Columns("PARTICULAR").Width = 200

            '.Columns("TOTAL").Width = 100
            '.Columns("PCS").Width = 70
            '.Columns("NETWT").Width = 80
            '.Columns("NETAMT").Width = 90
            '.Columns("DIAWT").Width = 80
            '.Columns("DIAAMT").Width = 100
            '.Columns("STNWT").Width = 80
            '.Columns("STNAMT").Width = 100
            '.Columns("PURMC").Width = 80
            '.Columns("GRANDTOTAL").Width = 100
            '.Columns("PURTAX").Width = 70

            '.Columns("COLHEAD").Visible = False
            ''.Columns("ITEM").Visible = False
            '.Columns("KEYNO").Visible = False
            ''.Columns("TAGVAL").Visible = False
            'FormatGridColumns(dgv, False, False)
            '.Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            '.Columns("STNWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
        End With
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub chkLstItem_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstItem.GotFocus
        Dim chkMetalName As String = GetChecked_CheckedList(chkLstMetal)
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
        strSql += " WHERE ACTIVE = 'Y'"
        If chkMetalName <> "" Then strSql += " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & "))"
        strSql += " ORDER BY ITEMNAME"
        FillCheckedListBox(strSql, chkLstItem)
        SetChecked_CheckedList(chkLstItem, chkItemSelectAll.Checked)
    End Sub

    Private Sub rbtDetailed_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtDetailed.CheckedChanged
        chkWithStudded.Visible = rbtDetailed.Checked
        chkWithStudded.Checked = rbtDetailed.Checked
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmTagItemsPurchaseReport_Properties
        obj.p_txtTranInvNo = txtTranInvNo.Text
        obj.p_chkDesignerSelectAll = chkDesignerSelectAll.Checked
        GetChecked_CheckedList(chkLstDesigner, obj.p_chkLstDesigner)
        'obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_chkMetalSelectAll = chkMetalSelectAll.Checked
        GetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal)
        obj.p_chkItemSelectAll = chkItemSelectAll.Checked
        GetChecked_CheckedList(chkLstItem, obj.p_chkLstItem)
        obj.p_chkItemCounterSelectAll = chkItemCounterSelectAll.Checked
        GetChecked_CheckedList(chkLstItemCounter, obj.p_chkLstItemCounter)
        obj.p_chkStockOnly = chkStockOnly.Checked
        obj.p_chkWithStudded = chkWithStudded.Checked
        obj.p_rbtDetailed = rbtDetailed.Checked
        obj.p_rbtSummary = rbtSummary.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmTagItemsPurchaseReport_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmTagItemsPurchaseReport_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmTagItemsPurchaseReport_Properties))
        txtTranInvNo.Text = obj.p_txtTranInvNo
        chkDesignerSelectAll.Checked = obj.p_chkDesignerSelectAll
        SetChecked_CheckedList(chkLstDesigner, obj.p_chkLstDesigner, Nothing)
        'chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        chkMetalSelectAll.Checked = obj.p_chkMetalSelectAll
        SetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal, Nothing)
        chkItemSelectAll.Checked = obj.p_chkItemSelectAll
        SetChecked_CheckedList(chkLstItem, obj.p_chkLstItem, Nothing)
        chkItemCounterSelectAll.Checked = obj.p_chkItemCounterSelectAll
        SetChecked_CheckedList(chkLstItemCounter, obj.p_chkLstItemCounter, Nothing)
        chkStockOnly.Checked = obj.p_chkStockOnly
        chkWithStudded.Checked = obj.p_chkWithStudded
        rbtDetailed.Checked = obj.p_rbtDetailed
        rbtSummary.Checked = obj.p_rbtSummary
    End Sub
End Class

Public Class frmTagItemsPurchaseReport_Properties

    Private txtTranInvNo As String = ""
    Public Property p_txtTranInvNo() As String
        Get
            Return txtTranInvNo
        End Get
        Set(ByVal value As String)
            txtTranInvNo = value
        End Set
    End Property
    Private chkDesignerSelectAll As Boolean = False
    Public Property p_chkDesignerSelectAll() As Boolean
        Get
            Return chkDesignerSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkDesignerSelectAll = value
        End Set
    End Property
    Private chkLstDesigner As New List(Of String)
    Public Property p_chkLstDesigner() As List(Of String)
        Get
            Return chkLstDesigner
        End Get
        Set(ByVal value As List(Of String))
            chkLstDesigner = value
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

    Private chkItemSelectAll As Boolean = False
    Public Property p_chkItemSelectAll() As Boolean
        Get
            Return chkItemSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkItemSelectAll = value
        End Set
    End Property
    Private chkLstItem As New List(Of String)
    Public Property p_chkLstItem() As List(Of String)
        Get
            Return chkLstItem
        End Get
        Set(ByVal value As List(Of String))
            chkLstItem = value
        End Set
    End Property

    Private chkItemCounterSelectAll As Boolean = False
    Public Property p_chkItemCounterSelectAll() As Boolean
        Get
            Return chkItemCounterSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkItemCounterSelectAll = value
        End Set
    End Property
    Private chkLstItemCounter As New List(Of String)
    Public Property p_chkLstItemCounter() As List(Of String)
        Get
            Return chkLstItemCounter
        End Get
        Set(ByVal value As List(Of String))
            chkLstItemCounter = value
        End Set
    End Property

    Private chkStockOnly As Boolean = False
    Public Property p_chkStockOnly() As Boolean
        Get
            Return chkStockOnly
        End Get
        Set(ByVal value As Boolean)
            chkStockOnly = value
        End Set
    End Property
    Private chkWithStudded As Boolean = True
    Public Property p_chkWithStudded() As Boolean
        Get
            Return chkWithStudded
        End Get
        Set(ByVal value As Boolean)
            chkWithStudded = value
        End Set
    End Property
    Private rbtDetailed As Boolean = True
    Public Property p_rbtDetailed() As Boolean
        Get
            Return rbtDetailed
        End Get
        Set(ByVal value As Boolean)
            rbtDetailed = value
        End Set
    End Property
    Private rbtSummary As Boolean = False
    Public Property p_rbtSummary() As Boolean
        Get
            Return rbtSummary
        End Get
        Set(ByVal value As Boolean)
            rbtSummary = value
        End Set
    End Property
End Class