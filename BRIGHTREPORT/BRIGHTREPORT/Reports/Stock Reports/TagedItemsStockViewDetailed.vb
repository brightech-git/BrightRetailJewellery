Imports System.Data.OleDb
Public Class TagedItemsStockViewDetailed
    Dim objGridShower As frmGridDispDia
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim count As Integer = 0

    Private Sub frmTagedItemsStockView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmTagedItemsStockView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.WindowState = FormWindowState.Maximized
        grpFiltration.Location = New Point((ScreenWid - grpFiltration.Width) / 2, ((ScreenHit - 128) - grpFiltration.Height) / 2)
        ''Checking CostCentre Status
        strSql = " select 1 from " & cnAdminDb & "..softcontrol where ctlText = 'Y' and ctlId = 'COSTCENTRE'"
        Dim dt As New DataTable
        dt.Clear()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If dt.Rows.Count > 0 Then
            cmbCostCenter.Enabled = True
        Else
            cmbCostCenter.Enabled = False
        End If
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click
        If Not CheckBckDays(userId, Me.Name, dtpAsOnDate.Value) Then dtpAsOnDate.Focus() : Exit Sub
        If chkStudColumn.Checked = False Then
            strSql = vbCrLf + "  IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPTAGSTOCKVIEW')>0 "
            strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMPTAGSTOCKVIEW"
            strSql += vbCrLf + "  SELECT T.TAGNO,T.PCS,T.GRSWT,CONVERT(NUMERIC(15,3),NETWT)NETWT,CONVERT(INT,NULL)DIAPCS,CONVERT(NUMERIC(15,3),NULL)DIAWT,CONVERT(NUMERIC(15,3),NULL)STNWT"
            strSql += vbCrLf + "  ,T.RATE,T.MAXWASTPER,T.MAXWAST,T.MAXMCGRM,T.MAXMC,T.MINWASTPER,T.MINWAST,T.MINMCGRM,T.MINMC"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR(50),CASE WHEN S.SUBITEMNAME <> '' THEN S.SUBITEMNAME ELSE I.ITEMNAME END)PARTICULAR,CASE WHEN D.SEAL <> '' THEN D.SEAL ELSE D.DESIGNERNAME END DESIGNER"
            strSql += vbCrLf + "  ,T.ITEMID,T.SUBITEMID,T.DESIGNERID"
            strSql += vbCrLf + "  ,CONVERT(INT,1)RESULT,CONVERT(VARCHAR(15),T.SNO)SNO,I.ITEMNAME,S.SUBITEMNAME,T.TAGVAL,CONVERT(VARCHAR(2),NULL)COLHEAD"
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPTAGSTOCKVIEW"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID = T.ITEMID"
            strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS S ON S.ITEMID = T.ITEMID AND S.SUBITEMID = T.SUBITEMID"
            strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..DESIGNER AS D ON D.DESIGNERID = T.DESIGNERID"
            strSql += vbCrLf + " WHERE T.RECDATE <= '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "' "
            If Not cnCentStock Then strSql += " AND T.COMPANYID = '" & GetStockCompId() & "'"
            If txtItemCode_NUM.Text <> "" Then
                strSql += vbCrLf + "  AND T.ITEMID = '" & txtItemCode_NUM.Text & "'"
            End If
            If cmbSubItemName.Enabled = True Then
                If cmbSubItemName.Text <> "ALL" Then
                    strSql += vbCrLf + "  AND T.SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName.Text & "' AND ITEMID = " & Val(txtItemCode_NUM.Text) & "),0)"
                End If
            End If
            If txtTagNo.Text <> "" Then
                strSql += vbCrLf + "  AND T.TAGNO = '" & txtTagNo.Text & "'"
            End If
            strSql += vbCrLf + "  AND (ISSDATE IS NULL OR ISSDATE > '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "')"
            If cmbDesigner.Text <> "ALL" Then
                strSql += vbCrLf + "  AND DESIGNERID = (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner.Text & "')"
            End If
            If cmbCounterName.Text <> "ALL" Then
                strSql += vbCrLf + "  AND ITEMCTRID = (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbCounterName.Text & "')"
            End If
            If cmbItemType.Text <> "ALL" Then
                strSql += vbCrLf + "  AND ITEMTYPEID = (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType.Text & "')"
            End If
            If cmbCostCenter.Text <> "ALL" And cmbCostCenter.Text <> "" Then
                strSql += vbCrLf + "  AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter.Text & "')"
            End If

            If txtLotNoFrom_NUM.Text <> "" And txtLotNoTo_NUM.Text <> "" Then
                'strsql += vbcrlf + "  AND LOTSNO = '" & TXTLOTNO_NUM.TEXT & "'"
                strSql += vbCrLf + "  AND LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO BETWEEN '" & txtLotNoFrom_NUM.Text & "' AND '" & txtLotNoTo_NUM.Text & "')"
            ElseIf txtLotNoFrom_NUM.Text <> "" And txtLotNoTo_NUM.Text = "" Then
                strSql += vbCrLf + "  AND LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO = '" & txtLotNoFrom_NUM.Text & "')"
            End If
            If txtFromWt_WET.Text <> "" Or txtToWt_WET.Text <> "" Then
                strSql += vbCrLf + "  AND (GRSWT BETWEEN '" & Val(txtFromWt_WET.Text) & "' AND '" & Val(txtToWt_WET.Text) & "')"
            End If
            If txtFromDiaWt_WET.Text <> "" Or txtToDiaWt_WET.Text <> "" Then
                strSql += " and ((select sum(stnwt) from " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) between '" & Val(txtFromDiaWt_WET.Text) & "' and '" & Val(txtToDiaWt_WET.Text) & "')"
            End If
            If txtFromRate_WET.Text <> "" Or txtToRate_WET.Text <> "" Then
                strSql += vbCrLf + "  AND (SALVALUE BETWEEN '" & Val(txtFromRate_WET.Text) & "' AND '" & Val(txtToRate_WET.Text) & "')"
            End If
            If rbtRegular.Checked = True Then
                strSql += vbCrLf + "  AND ORDREPNO =''"
            End If
            If rbtOrder.Checked = True Then
                strSql += vbCrLf + "  AND ORDREPNO <> ''"
            End If
            If cmbSize.Text <> "ALL" Then
                strSql += vbCrLf + "  AND SIZEID = (SELECT SIZEID FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZENAME = '" & cmbSize.Text & "')"
            End If
            If Not chkApproval.Checked Then
                strSql += vbCrLf + " AND ISNULL(APPROVAL,'') = ''"
            End If
            If txtSearch_txt.Text <> "" Then
                strSql += vbCrLf + " AND T." & cmbSearchKey.Text & " LIKE"
                strSql += vbCrLf + " '" & txtSearch_txt.Text & "%'"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "  /** Inserting Stone Details **/"
            strSql += vbCrLf + "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMPTAGSTOCKVIEW)>0"
            strSql += vbCrLf + "  	BEGIN"
            strSql += vbCrLf + "  	INSERT INTO TEMPTABLEDB..TEMPTAGSTOCKVIEW"
            strSql += vbCrLf + "  	("
            strSql += vbCrLf + "  	PARTICULAR,DIAPCS,DIAWT,STNWT,SNO,RESULT,TAGVAL,ITEMNAME"
            strSql += vbCrLf + "  	)"
            strSql += vbCrLf + "  	SELECT CASE WHEN ISNULL(S.SHORTNAME,'') = '' THEN I.SHORTNAME ELSE S.SHORTNAME END AS PARTICULAR"
            strSql += vbCrLf + "  	,CASE WHEN I.DIASTONE = 'D' THEN T.STNPCS ELSE 0 END AS DIAPCS"
            strSql += vbCrLf + "  	,CASE WHEN I.DIASTONE = 'D' THEN T.STNWT ELSE 0 END AS DIAWT"
            strSql += vbCrLf + "  	,CASE WHEN I.DIASTONE = 'S' THEN T.STNWT ELSE 0 END AS STNWT"
            strSql += vbCrLf + "  	,T.TAGSNO AS SNO,2 RESULT,TT.TAGVAL,I.ITEMNAME"
            strSql += vbCrLf + "  	FROM " & cnAdminDb & "..ITEMTAGSTONE AS T"
            strSql += vbCrLf + "  	INNER JOIN TEMPTABLEDB..TEMPTAGSTOCKVIEW AS TT ON TT.SNO = T.TAGSNO"
            strSql += vbCrLf + "  	INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = T.STNITEMID"
            strSql += vbCrLf + "  	LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS S ON S.ITEMID = T.STNITEMID AND S.SUBITEMID = T.STNSUBITEMID"

            strSql += vbCrLf + "    INSERT INTO TEMPTABLEDB..TEMPTAGSTOCKVIEW"
            strSql += vbCrLf + "    ("
            strSql += vbCrLf + "    TAGNO,PCS,GRSWT,NETWT,DIAPCS,DIAWT,STNWT,COLHEAD,TAGVAL"
            strSql += vbCrLf + "    )"
            strSql += vbCrLf + "    SELECT 'TOTAL',SUM(ISNULL(PCS,0)),SUM(GRSWT),SUM(NETWT),SUM(ISNULL(DIAPCS,0)),SUM(ISNULL(DIAWT,0)),SUM(ISNULL(STNWT,0))"
            strSql += vbCrLf + "    ,'G' COLHEAD,99999999 TAGVAL"
            strSql += vbCrLf + "    FROM TEMPTABLEDB..TEMPTAGSTOCKVIEW"

            strSql += vbCrLf + "  	END"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " UPDATE TEMPTABLEDB..TEMPTAGSTOCKVIEW SET"
            strSql += " PARTICULAR = SUBSTRING(PARTICULAR,1,15)"
            strSql += " ,DESIGNER = SUBSTRING(DESIGNER,1,4)"
            strSql += " ,PCS = CASE WHEN PCS = 0 THEN NULL ELSE PCS END"
            strSql += " ,GRSWT = CASE WHEN GRSWT = 0 THEN NULL ELSE GRSWT END"
            strSql += " ,NETWT = CASE WHEN NETWT = 0 THEN NULL ELSE NETWT END"
            strSql += " ,DIAPCS = CASE WHEN DIAPCS = 0 THEN NULL ELSE DIAPCS END"
            strSql += " ,DIAWT = CASE WHEN DIAWT = 0 THEN NULL ELSE DIAWT END"
            strSql += " ,STNWT = CASE WHEN STNWT = 0 THEN NULL ELSE STNWT END"
            strSql += " ,RATE = CASE WHEN RATE = 0 THEN NULL ELSE RATE END"
            strSql += " ,MAXWASTPER = CASE WHEN MAXWASTPER = 0 THEN NULL ELSE MAXWASTPER END"
            strSql += " ,MAXWAST = CASE WHEN MAXWAST = 0 THEN NULL ELSE MAXWAST END"
            strSql += " ,MAXMCGRM= CASE WHEN MAXMCGRM = 0 THEN NULL ELSE MAXMCGRM END"
            strSql += " ,MAXMC = CASE WHEN MAXMC = 0 THEN NULL ELSE MAXMC END"
            strSql += " ,MINWASTPER = CASE WHEN MINWASTPER = 0 THEN NULL ELSE MINWASTPER END"
            strSql += " ,MINWAST = CASE WHEN MINWAST = 0 THEN NULL ELSE MINWAST END"
            strSql += " ,MINMCGRM= CASE WHEN MINMCGRM = 0 THEN NULL ELSE MINMCGRM END"
            strSql += " ,MINMC = CASE WHEN MINMC = 0 THEN NULL ELSE MINMC END"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            count = 0
        Else

            strSql = vbCrLf + "  IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPTAGSTOCKVIEW')>0 "
            strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMPTAGSTOCKVIEW"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  TAGNO STYLENO"
            strSql += vbCrLf + "  ,T.ITEMID ID"
            strSql += vbCrLf + "  ,I.ITEMNAME ITEM"
            strSql += vbCrLf + "  ,S.SUBITEMNAME SUBITEM"
            strSql += vbCrLf + "  ,IT.NAME [IT TYPE]"
            strSql += vbCrLf + "  ,PCS "
            strSql += vbCrLf + "  ,T.GRSWT "
            strSql += vbCrLf + "  ,T.SNO "
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPTAGSTOCKVIEW"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID = T.ITEMID"
            strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS S ON S.ITEMID = T.ITEMID AND S.SUBITEMID = T.SUBITEMID"
            strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..ITEMTYPE AS IT ON IT.ITEMTYPEID = T.ITEMTYPEID"
            strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..DESIGNER AS D ON D.DESIGNERID = T.DESIGNERID"
            strSql += vbCrLf + " WHERE T.RECDATE <= '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "' "
            If Not cnCentStock Then strSql += " AND T.COMPANYID = '" & GetStockCompId() & "'"
            If txtItemCode_NUM.Text <> "" Then
                strSql += vbCrLf + "  AND T.ITEMID = '" & txtItemCode_NUM.Text & "'"
            End If
            If cmbSubItemName.Enabled = True Then
                If cmbSubItemName.Text <> "ALL" Then
                    strSql += vbCrLf + "  AND T.SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & cmbSubItemName.Text & "' AND ITEMID = " & Val(txtItemCode_NUM.Text) & "),0)"
                End If
            End If
            If txtTagNo.Text <> "" Then
                strSql += vbCrLf + "  AND T.TAGNO = '" & txtTagNo.Text & "'"
            End If
            strSql += vbCrLf + "  AND (ISSDATE IS NULL OR ISSDATE > '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "')"
            If cmbDesigner.Text <> "ALL" Then
                strSql += vbCrLf + "  AND DESIGNERID = (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner.Text & "')"
            End If
            If cmbCounterName.Text <> "ALL" Then
                strSql += vbCrLf + "  AND ITEMCTRID = (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & cmbCounterName.Text & "')"
            End If
            If cmbItemType.Text <> "ALL" Then
                strSql += vbCrLf + "  AND ITEMTYPEID = (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType.Text & "')"
            End If
            If cmbCostCenter.Text <> "ALL" And cmbCostCenter.Text <> "" Then
                strSql += vbCrLf + "  AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter.Text & "')"
            End If
            If txtLotNoFrom_NUM.Text <> "" And txtLotNoTo_NUM.Text <> "" Then
                'strsql += vbcrlf + "  AND LOTSNO = '" & TXTLOTNO_NUM.TEXT & "'"
                strSql += vbCrLf + "  AND LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO BETWEEN '" & txtLotNoFrom_NUM.Text & "' AND '" & txtLotNoTo_NUM.Text & "')"
            ElseIf txtLotNoFrom_NUM.Text <> "" And txtLotNoTo_NUM.Text = "" Then
                strSql += vbCrLf + "  AND LOTSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE LOTNO = '" & txtLotNoFrom_NUM.Text & "')"
            End If

            If txtFromWt_WET.Text <> "" Or txtToWt_WET.Text <> "" Then
                strSql += vbCrLf + "  AND (GRSWT BETWEEN '" & Val(txtFromWt_WET.Text) & "' AND '" & Val(txtToWt_WET.Text) & "')"
            End If
            If txtFromDiaWt_WET.Text <> "" Or txtToDiaWt_WET.Text <> "" Then
                strSql += " and ((select sum(stnwt) from " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) between '" & Val(txtFromDiaWt_WET.Text) & "' and '" & Val(txtToDiaWt_WET.Text) & "')"
            End If
            If txtFromRate_WET.Text <> "" Or txtToRate_WET.Text <> "" Then
                strSql += vbCrLf + "  AND (SALVALUE BETWEEN '" & Val(txtFromRate_WET.Text) & "' AND '" & Val(txtToRate_WET.Text) & "')"
            End If
            If rbtRegular.Checked = True Then
                strSql += vbCrLf + "  AND ORDREPNO =''"
            End If
            If rbtOrder.Checked = True Then
                strSql += vbCrLf + "  AND ORDREPNO <> ''"
            End If
            If cmbSize.Text <> "ALL" Then
                strSql += vbCrLf + "  AND SIZEID = (SELECT SIZEID FROM " & cnAdminDb & "..ITEMSIZE WHERE SIZENAME = '" & cmbSize.Text & "')"
            End If
            If Not chkApproval.Checked Then
                strSql += vbCrLf + " AND ISNULL(APPROVAL,'') = ''"
            End If
            If txtSearch_txt.Text <> "" Then
                strSql += vbCrLf + " AND T." & cmbSearchKey.Text & " LIKE"
                strSql += vbCrLf + " '" & txtSearch_txt.Text & "%'"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

            strSql = vbCrLf + "  IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPTAGSTOCKVIEWDET')>0 "
            strSql += vbCrLf + "    DROP TABLE TEMPTABLEDB..TEMPTAGSTOCKVIEWDET"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  TAGSNO"
            strSql += vbCrLf + "  ,I.ITEMNAME ITEM"
            strSql += vbCrLf + "  ,S.SUBITEMNAME SUBITEM"
            strSql += vbCrLf + "  ,T.STNPCS PCS"
            strSql += vbCrLf + "  ,T.STNWT WEIGHT"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR(15),ISNULL(CASE WHEN T.STONEUNIT='C' THEN (SELECT SIZEDESC FROM " & cnAdminDb & "..CENTSIZE WHERE T.STNWT BETWEEN FROMCENT AND TOCENT ) ELSE NULL END,'')) SEIVE"
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPTAGSTOCKVIEWDET"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAGSTONE AS T"
            strSql += vbCrLf + "  INNER JOIN TEMPTABLEDB..TEMPTAGSTOCKVIEW AS TT ON TT.SNO = T.TAGSNO"
            strSql += vbCrLf + "    INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID = T.STNITEMID "
            strSql += vbCrLf + "    LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS S ON S.ITEMID = T.STNITEMID AND S.SUBITEMID = T.STNSUBITEMID "

            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

            count = Val(objGPack.GetSqlValue("SELECT TOP 1 COUNT(*) T FROM TEMPTABLEDB..TEMPTAGSTOCKVIEWDET GROUP BY TAGSNO ORDER BY COUNT(*) DESC"))

            For i As Integer = 1 To count
                strSql = vbCrLf + "   ALTER TABLE TEMPTABLEDB..TEMPTAGSTOCKVIEW ADD ITEM" + i.ToString() + " VARCHAR(50)"
                strSql += vbCrLf + "   ALTER TABLE TEMPTABLEDB..TEMPTAGSTOCKVIEW ADD SUBITEM" + i.ToString() + " VARCHAR(50)"
                strSql += vbCrLf + "   ALTER TABLE TEMPTABLEDB..TEMPTAGSTOCKVIEW ADD PCS" + i.ToString() + " int"
                strSql += vbCrLf + "   ALTER TABLE TEMPTABLEDB..TEMPTAGSTOCKVIEW ADD WEIGHT" + i.ToString() + " Numeric(12,4)"
                strSql += vbCrLf + "   ALTER TABLE TEMPTABLEDB..TEMPTAGSTOCKVIEW ADD SEIVE" + i.ToString() + " VARCHAR(15)"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

                strSql = vbCrLf + "   UPDATE TV SET ITEM" + i.ToString() + "=Y.ITEM ,SUBITEM" + i.ToString() + "=Y.SUBITEM, PCS" + i.ToString() + "=Y.PCS, WEIGHT" + i.ToString() + "=Y.WEIGHT, SEIVE" + i.ToString() + "=Y.SEIVE"
                strSql += vbCrLf + "   FROM TEMPTABLEDB..TEMPTAGSTOCKVIEW TV ,"
                strSql += vbCrLf + "   (SELECT SNO"
                strSql += vbCrLf + "   ,(SELECT ITEM FROM"
                strSql += vbCrLf + "  (SELECT ROW_NUMBER() OVER (ORDER BY TAGSNO,ITEM,SUBITEM,PCS,WEIGHT)AS ROWNUMBER ,ITEM"
                strSql += vbCrLf + "   FROM TEMPTABLEDB..TEMPTAGSTOCKVIEWDET "
                strSql += vbCrLf + "   WHERE TAGSNO =T.SNO"
                strSql += vbCrLf + "   )X WHERE X.ROWNUMBER =" + i.ToString() + " ) ITEM"
                strSql += vbCrLf + "   ,(SELECT SUBITEM FROM"
                strSql += vbCrLf + "  (SELECT ROW_NUMBER() OVER (ORDER BY TAGSNO,ITEM,SUBITEM,PCS,WEIGHT)AS ROWNUMBER ,SUBITEM"
                strSql += vbCrLf + "   FROM TEMPTABLEDB..TEMPTAGSTOCKVIEWDET "
                strSql += vbCrLf + "   WHERE TAGSNO =T.SNO"
                strSql += vbCrLf + "   )X WHERE X.ROWNUMBER =" + i.ToString() + " ) SUBITEM"
                strSql += vbCrLf + "   ,(SELECT PCS FROM"
                strSql += vbCrLf + "  (SELECT ROW_NUMBER() OVER (ORDER BY TAGSNO,ITEM,SUBITEM,PCS,WEIGHT)AS ROWNUMBER ,PCS"
                strSql += vbCrLf + "   FROM TEMPTABLEDB..TEMPTAGSTOCKVIEWDET "
                strSql += vbCrLf + "   WHERE TAGSNO =T.SNO"
                strSql += vbCrLf + "   )X WHERE X.ROWNUMBER =" + i.ToString() + " ) PCS"
                strSql += vbCrLf + "   ,(SELECT WEIGHT FROM"
                strSql += vbCrLf + "  (SELECT ROW_NUMBER() OVER (ORDER BY TAGSNO,ITEM,SUBITEM,PCS,WEIGHT)AS ROWNUMBER ,WEIGHT"
                strSql += vbCrLf + "   FROM TEMPTABLEDB..TEMPTAGSTOCKVIEWDET "
                strSql += vbCrLf + "   WHERE TAGSNO =T.SNO"
                strSql += vbCrLf + "   )X WHERE X.ROWNUMBER =" + i.ToString() + " ) WEIGHT"
                strSql += vbCrLf + "   ,(SELECT SEIVE FROM"
                strSql += vbCrLf + "  (SELECT ROW_NUMBER() OVER (ORDER BY TAGSNO,ITEM,SUBITEM,PCS,WEIGHT)AS ROWNUMBER ,SEIVE"
                strSql += vbCrLf + "   FROM TEMPTABLEDB..TEMPTAGSTOCKVIEWDET "
                strSql += vbCrLf + "   WHERE TAGSNO =T.SNO"
                strSql += vbCrLf + "   )X WHERE X.ROWNUMBER =" + i.ToString() + " ) SEIVE"
                strSql += vbCrLf + "   FROM TEMPTABLEDB..TEMPTAGSTOCKVIEW T)Y"
                strSql += vbCrLf + "  WHERE TV.SNO =Y.SNO"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

            Next i

        End If

        strSql = vbCrLf + "  SELECT * FROM TEMPTABLEDB..TEMPTAGSTOCKVIEW"
        If chkStudColumn.Checked = False Then
            strSql += vbCrLf + "  ORDER BY TAGVAL,SNO,RESULT"
        Else
            strSql += vbCrLf + "  ORDER BY SNO"
        End If
        Dim dtGrid As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        Dim dtGridResult As New DataTable
        dtGridResult = dtGrid.Clone
        dtGridResult.Columns.Add("SLNO", GetType(String))
        dtGridResult.Columns("SLNO").SetOrdinal(0)
        For Each col As DataColumn In dtGridResult.Columns
            col.DataType = GetType(String)
        Next

        Dim RoDest As DataRow = Nothing
        Dim RoSource As DataRow = Nothing
        Dim TagSno As String = Nothing
        Dim SlNo As Integer = 0
        For i As Integer = 0 To dtGrid.Rows.Count - 1
            RoSource = dtGrid.Rows(i)
            If TagSno <> RoSource.Item("SNO").ToString Then
                dtGridResult.Rows.Add()
                RoDest = dtGridResult.Rows(SlNo)
                SlNo += 1
                If i <> dtGrid.Rows.Count - 1 Then RoDest.Item("SLNO") = SlNo
                TagSno = RoSource.Item("SNO").ToString
                For j As Integer = 0 To dtGrid.Columns.Count - 1
                    RoDest.Item(dtGrid.Columns(j).ColumnName) = RoSource.Item(j)

                Next
            Else
                For j As Integer = 0 To dtGrid.Columns.Count - 1
                    'If RoSource.Item(j).ToString <> "" Then 
                    RoDest.Item(dtGrid.Columns(j).ColumnName) = RoDest.Item(dtGrid.Columns(j).ColumnName).ToString + vbCrLf + RoSource.Item(j).ToString
                Next
            End If
        Next
        dtGridResult.AcceptChanges()
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        objGridShower.gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "INDIVIDUAL TAG STOCK REPORT"
        Dim tit As String = Nothing
        tit += "INDIVIDUAL TAG STOCK REPORT"
        If txtItemName.Text <> "" Then tit += " OF " & txtItemName.Text + "'s "
        tit += " AS ON " & dtpAsOnDate.Text
        objGridShower.lblTitle.Text = tit & IIf(cmbCostCenter.Text <> "" And cmbCostCenter.Text <> "ALL", " :" & cmbCostCenter.Text, "")
        AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
        objGridShower.StartPosition = FormStartPosition.CenterScreen


        ''Here Row by Row Filling
        'Dim dgvCol As New DataGridViewTextBoxColumn
        'dgvCol.Name = "SLNO"
        'dgvCol.ValueType = GetType(Integer)
        'objGridShower.gridView.Columns.Add(dgvCol)
        'For Each col As DataColumn In dtGrid.Columns
        '    objGridShower.gridView.Columns.Add(col.ColumnName, col.ColumnName)
        '    objGridShower.gridView.Columns(col.ColumnName).ValueType = GetType(String)
        '    'col.DataType = GetType(String)
        'Next
        objGridShower.gridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        objGridShower.gridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft
        'Dim DgvRow As DataGridViewRow = Nothing
        'Dim RoSource As DataRow = Nothing
        'Dim TagSno As String = Nothing
        'Dim SlNo As Integer = 0
        'For i As Integer = 0 To dtGrid.Rows.Count - 1
        '    RoSource = dtGrid.Rows(i)
        '    If TagSno <> RoSource.Item("SNO").ToString Then
        '        objGridShower.gridView.Rows.Add()
        '        DgvRow = objGridShower.gridView.Rows(SlNo)
        '        SlNo += 1
        '        If i <> dtGrid.Rows.Count - 1 Then DgvRow.Cells("SLNO").Value = SlNo
        '        TagSno = RoSource.Item("SNO").ToString
        '        For j As Integer = 0 To dtGrid.Columns.Count - 1
        '            DgvRow.Cells(dtGrid.Columns(j).ColumnName).Value = RoSource.Item(j)
        '        Next
        '    Else
        '        For j As Integer = 0 To dtGrid.Columns.Count - 1
        '            'If RoSource.Item(j).ToString <> "" Then 
        '            DgvRow.Cells(dtGrid.Columns(j).ColumnName).Value = DgvRow.Cells(dtGrid.Columns(j).ColumnName).Value.ToString + vbCrLf + RoSource.Item(j).ToString
        '        Next
        '    End If
        'Next
        objGridShower.gridView.DataSource = dtGridResult
        objGridShower.gridView.Rows(objGridShower.gridView.RowCount - 1).DefaultCellStyle = Globalvariables.reportTotalStyle
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        DataGridView_SummaryFormatting(objGridShower.gridView, count)
        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.gridViewHeader.Visible = True
        GridViewHeaderCreator(objGridShower.gridViewHeader, count)
        Prop_Sets()
        'FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
    End Sub

    Private Sub txtItemCode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemCode_NUM.KeyDown
        Dim itemId As String
        If e.KeyCode = Keys.Insert Then
            strSql = " SELECT DISTINCT"
            strSql += vbCrLf + " ITEMID, "
            strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
            itemId = BrighttechPack.SearchDialog.Show("Find ItemId", strSql, cn)
            strSql = " Select itemName from " & cnAdminDb & "..itemMast where itemId = '" & itemId & "'"
            Dim dt As New DataTable
            dt.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtItemCode_NUM.Text = itemId
                txtItemName.Text = dt.Rows(0).Item("itemName")
            End If
        End If
    End Sub

    Private Sub txtItemCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtItemCode_NUM.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            strSql = " Select itemName from " & cnAdminDb & "..itemMast where itemId = '" & txtItemCode_NUM.Text & "'"
            Dim dt As New DataTable
            dt.Clear()
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtItemName.Text = dt.Rows(0).Item("itemName")
            Else
                txtItemName.Clear()
            End If
        End If
    End Sub

    Private Sub txtItemName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtItemName.TextChanged
        If txtItemName.Text <> "" Then
            cmbSubItemName.Items.Clear()
            cmbSubItemName.Items.Add("ALL")
            strSql = " Select SubItemName from " & cnAdminDb & "..SubItemMast where itemId = '" & txtItemCode_NUM.Text & "'"
            objGPack.FillCombo(strSql, cmbSubItemName, False)
            cmbSubItemName.Text = "ALL"
            If cmbSubItemName.Items.Count > 0 Then
                cmbSubItemName.Enabled = True
            Else
                cmbSubItemName.Enabled = False
            End If
        Else
            cmbSubItemName.Items.Clear()
            cmbSubItemName.Enabled = False
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        cmbSubItemName.Items.Clear()
        ' cmbSubItemName.Enabled = False

        ' rbtAll.Checked = True
        txtItemCode_NUM.Select()
        cmbGroup.Items.Clear()
        cmbGroup.Items.Add("NONE")
        cmbGroup.Items.Add("COUNTER")
        ' cmbGroup.Text = "NONE"
        ''Size
        cmbSize.Items.Clear()
        cmbSize.Items.Add("ALL")
        strSql = " SELECT SIZENAME FROM " & cnAdminDb & "..ITEMSIZE ORDER BY SIZENAME"
        objGPack.FillCombo(strSql, cmbSize, False)
        ' cmbSize.Text = "ALL"

        ''Designer
        cmbDesigner.Items.Clear()
        cmbDesigner.Items.Add("ALL")
        strSql = " Select DesignerName from " & cnAdminDb & "..Designer order by DesignerName"
        objGPack.FillCombo(strSql, cmbDesigner, False)
        'cmbDesigner.Text = "ALL"

        ''Counter
        cmbCounterName.Items.Clear()
        cmbCounterName.Items.Add("ALL")
        strSql = " Select ItemCtrName from " & cnAdminDb & "..itemCounter order by itemCtrName"
        objGPack.FillCombo(strSql, cmbCounterName, False)
        ' cmbCounterName.Text = "ALL"

        ''ItemType
        cmbItemType.Items.Clear()
        cmbItemType.Items.Add("ALL")
        strSql = " Select Name from " & cnAdminDb & "..itemType order by Name"
        objGPack.FillCombo(strSql, cmbItemType, False)
        ' cmbItemType.Text = "ALL"

        ''CostCenter
        cmbCostCenter.Items.Clear()
        If cmbCostCenter.Enabled = True Then
            cmbCostCenter.Items.Add("ALL")
            strSql = " Select CostName from " & cnAdminDb & "..CostCentre order by CostName"
            objGPack.FillCombo(strSql, cmbCostCenter, False)
            cmbCostCenter.Text = IIf(cnDefaultCostId, "ALL", cnCostName)
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then cmbCostCenter.Enabled = False
        End If
        strSql = vbCrLf + " SELECT NAME FROM " & cnAdminDb & "..SYSCOLUMNS WHERE ID IN (SELECT ID FROM " & cnAdminDb & "..SYSOBJECTS WHERE XTYPE='U' AND NAME='ITEMTAG')"
        strSql += vbCrLf + " ORDER BY NAME"
        objGPack.FillCombo(strSql, cmbSearchKey, True, False)
        cmbSearchKey.Text = ""
        txtSearch_txt.Text = ""

        txtLotNoTo_NUM.Text = ""
        txtLotNoFrom_NUM.Text = ""
        dtpAsOnDate.Value = GetServerDate()
        Prop_Gets()
    End Sub

    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        SetGridHeadColWid(CType(sender, DataGridView), count)
    End Sub

    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView, ByVal count As Integer)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            If chkStudColumn.Checked = False Then
                For cnt As Integer = 19 To dgv.ColumnCount - 1
                    .Columns(cnt).Visible = False
                Next
                .Columns("SLNO").HeaderText = "SNO"
                .Columns("DIAPCS").HeaderText = "DIAPI"
                .Columns("MAXWASTPER").HeaderText = "WAS%"
                .Columns("MAXWAST").HeaderText = "WAST"
                .Columns("MAXMCGRM").HeaderText = "MC/G"
                .Columns("MAXMC").HeaderText = "MC"
                .Columns("MINWASTPER").HeaderText = "WAS%"
                .Columns("MINWAST").HeaderText = "WAST"
                .Columns("MINMCGRM").HeaderText = "MC/G"
                .Columns("MINMC").HeaderText = "MC"
                .Columns("DESIGNER").HeaderText = "DES"

                .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                .Columns("DIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                .Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                .Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight

                .Columns("SLNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                .Columns("MAXWASTPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                .Columns("MAXWAST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                .Columns("MAXMCGRM").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                .Columns("MAXMC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                .Columns("MINWASTPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                .Columns("MINWAST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                .Columns("MINMCGRM").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                .Columns("MINMC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
            Else
                .Columns("SNO").Visible = False
                .Columns("SLNO").HeaderText = "SNO"
                For i As Integer = 0 To count - 1
                    Dim J As Integer = 8 + i * 5
                    .Columns(J + 1).HeaderText = "ITEM"
                    .Columns(J + 2).HeaderText = "SUBITEM"
                    .Columns(J + 3).HeaderText = "PCS"
                    .Columns(J + 4).HeaderText = "WEIGHT"
                    .Columns(J + 5).HeaderText = "SEIVE"
                Next
            End If

            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub

    Private Sub GridViewHeaderCreator(ByVal gridviewHead As DataGridView, ByVal count As Integer)
        Dim dtHead As New DataTable
        If chkStudColumn.Checked = False Then
            strSql = " SELECT "
            strSql += " ''[SLNO~TAGNO~PCS~GRSWT~NETWT~DIAPCS~DIAWT~STNWT~RATE]"
            strSql += " ,''[MAXWASTPER~MAXWAST~MAXMCGRM~MAXMC]"
            strSql += " ,''[MINWASTPER~MINWAST~MINMCGRM~MINMC]"
            strSql += " ,''[PARTICULAR~D.DESIGNER],''SCROLL"
        Else
            strSql = " SELECT "
            strSql += " ''[SLNO~STYLENO~ID~ITEM~IT TYPE~PCS~GRSWT~SNO]"

            For i As Integer = 1 To count
                strSql += " ,''[ITEM" & i & "~SUBITEM" & i & "~PCS" & i & "~WEIGHT" & i & "~SEIVE" & i & "" + i.ToString() + "]"
            Next
            strSql += " ,''SCROLL"
        End If
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtHead)
        gridviewHead.DataSource = dtHead
        If chkStudColumn.Checked = False Then

            gridviewHead.Columns("SLNO~TAGNO~PCS~GRSWT~NETWT~DIAPCS~DIAWT~STNWT~RATE").HeaderText = ""
            gridviewHead.Columns("MAXWASTPER~MAXWAST~MAXMCGRM~MAXMC").HeaderText = "MAXIMUM"
            gridviewHead.Columns("MINWASTPER~MINWAST~MINMCGRM~MINMC").HeaderText = "MINIMUM"
            gridviewHead.Columns("PARTICULAR~D.DESIGNER").HeaderText = ""
            gridviewHead.Columns("SCROLL").HeaderText = ""
        Else
            gridviewHead.Columns("SLNO~STYLENO~ID~ITEM~IT TYPE~PCS~GRSWT~SNO").HeaderText = ""
            For i As Integer = 1 To count
                gridviewHead.Columns("ITEM" & i & "~SUBITEM" & i & "~PCS" & i & "~WEIGHT" & i & "~SEIVE" & i & "" + i.ToString()).HeaderText = "DIA " + i.ToString()
            Next
            gridviewHead.Columns("SCROLL").HeaderText = ""
        End If
        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        SetGridHeadColWid(gridviewHead, count)
    End Sub
    Private Sub SetGridHeadColWid(ByVal gridViewHeader As DataGridView, ByVal count As Integer)
        Dim f As frmGridDispDia
        f = objGPack.GetParentControl(gridViewHeader)
        If Not f.gridViewHeader.Visible Then Exit Sub
        If f.gridViewHeader Is Nothing Then Exit Sub
        If Not f.gridView.ColumnCount > 0 Then Exit Sub
        If Not f.gridViewHeader.ColumnCount > 0 Then Exit Sub
        With f.gridViewHeader
            If chkStudColumn.Checked = True Then
                .Columns("SLNO~STYLENO~ID~ITEM~IT TYPE~PCS~GRSWT~SNO").Width = _
                f.gridView.Columns("SLNO").Width + _
                f.gridView.Columns("STYLENO").Width + _
                f.gridView.Columns("ID").Width + _
                f.gridView.Columns("ITEM").Width + _
                f.gridView.Columns("SUBITEM").Width + _
                f.gridView.Columns(5).Width + _
                f.gridView.Columns("PCS").Width + _
                f.gridView.Columns("GRSWT").Width
                For i As Integer = 1 To count
                    Dim J As Integer = 8 + (i - 1) * 5
                    .Columns(i).Width = _
                    f.gridView.Columns(J + 1).Width _
                    + f.gridView.Columns(J + 2).Width _
                    + f.gridView.Columns(J + 3).Width _
                    + f.gridView.Columns(J + 4).Width _
                    + f.gridView.Columns(J + 5).Width
                Next
            Else
                .Columns("SLNO~TAGNO~PCS~GRSWT~NETWT~DIAPCS~DIAWT~STNWT~RATE").Width = _
                f.gridView.Columns("SLNO").Width + _
                f.gridView.Columns("TAGNO").Width + _
                f.gridView.Columns("PCS").Width + _
                f.gridView.Columns("GRSWT").Width + _
                f.gridView.Columns("NETWT").Width + _
                f.gridView.Columns("DIAPCS").Width + _
                f.gridView.Columns("DIAWT").Width + _
                f.gridView.Columns("STNWT").Width + _
                f.gridView.Columns("RATE").Width
                .Columns("MAXWASTPER~MAXWAST~MAXMCGRM~MAXMC").Width = _
                f.gridView.Columns("MAXWASTPER").Width _
                + f.gridView.Columns("MAXWAST").Width _
                + f.gridView.Columns("MAXMCGRM").Width _
                + f.gridView.Columns("MAXMC").Width
                .Columns("MINWASTPER~MINWAST~MINMCGRM~MINMC").Width = _
                f.gridView.Columns("MINWASTPER").Width _
                + f.gridView.Columns("MINWAST").Width _
                + f.gridView.Columns("MINMCGRM").Width _
                + f.gridView.Columns("MINMC").Width
                .Columns("PARTICULAR~D.DESIGNER").Width = _
                f.gridView.Columns("PARTICULAR").Width _
                + f.gridView.Columns("DESIGNER").Width
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
    Private Sub Prop_Sets()
        Dim obj As New TagedItemsStockViewDetailed_Properties
        obj.p_txtItemCode_NUM = txtItemCode_NUM.Text
        obj.p_txtItemName = txtItemName.Text
        obj.p_cmbSubItemName = cmbSubItemName.Text
        obj.p_txtTagNo = txtTagNo.Text
        obj.p_cmbDesigner = cmbDesigner.Text
        obj.p_cmbCounterName = cmbCounterName.Text
        obj.p_cmbItemType = cmbItemType.Text
        obj.p_cmbSize = cmbSize.Text
        obj.p_cmbCostCenter = cmbCostCenter.Text
        obj.p_txtLotNo_NUM = txtLotNoFrom_NUM.Text
        obj.p_txtFromWt_WET = txtFromWt_WET.Text
        obj.p_txtToWt_WET = txtToWt_WET.Text
        obj.p_txtFromDiaWt_WET = txtFromDiaWt_WET.Text
        obj.p_txtToDiaWt_WET = txtToDiaWt_WET.Text
        obj.p_txtFromRate_WET = txtFromRate_WET.Text
        obj.p_txtToRate_WET = txtToRate_WET.Text
        obj.p_cmbGroup = cmbGroup.Text
        obj.p_rbtAll = rbtAll.Checked
        obj.p_rbtRegular = rbtRegular.Checked
        obj.p_rbtOrder = rbtOrder.Checked
        obj.p_chkApproval = chkApproval.Checked
        obj.p_chkStudColumn = chkStudColumn.Checked
        SetSettingsObj(obj, Me.Name, GetType(TagedItemsStockViewDetailed_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New TagedItemsStockViewDetailed_Properties
        GetSettingsObj(obj, Me.Name, GetType(TagedItemsStockViewDetailed_Properties))
        txtItemCode_NUM.Text = obj.p_txtItemCode_NUM
        txtItemName.Text = obj.p_txtItemName
        cmbSubItemName.Text = obj.p_cmbSubItemName
        txtTagNo.Text = obj.p_txtTagNo
        cmbDesigner.Text = obj.p_cmbDesigner
        cmbCounterName.Text = obj.p_cmbCounterName
        cmbItemType.Text = obj.p_cmbItemType
        cmbSize.Text = obj.p_cmbSize
        'cmbCostCenter.Text = obj.p_cmbCostCenter
        txtLotNoFrom_NUM.Text = obj.p_txtLotNo_NUM
        txtFromWt_WET.Text = obj.p_txtFromWt_WET
        txtToWt_WET.Text = obj.p_txtToWt_WET
        txtFromDiaWt_WET.Text = obj.p_txtFromDiaWt_WET
        txtToDiaWt_WET.Text = obj.p_txtToDiaWt_WET
        txtFromRate_WET.Text = obj.p_txtFromRate_WET
        txtToRate_WET.Text = obj.p_txtToRate_WET
        cmbGroup.Text = obj.p_cmbGroup
        rbtAll.Checked = obj.p_rbtAll
        rbtRegular.Checked = obj.p_rbtRegular
        rbtOrder.Checked = obj.p_rbtOrder
        chkApproval.Checked = obj.p_chkApproval
        chkStudColumn.Checked = obj.p_chkStudColumn
    End Sub
End Class


Public Class TagedItemsStockViewDetailed_Properties
    Private txtItemCode_NUM As String = ""
    Public Property p_txtItemCode_NUM() As String
        Get
            Return txtItemCode_NUM
        End Get
        Set(ByVal value As String)
            txtItemCode_NUM = value
        End Set
    End Property
    Private txtItemName As String = ""
    Public Property p_txtItemName() As String
        Get
            Return txtItemName
        End Get
        Set(ByVal value As String)
            txtItemName = value
        End Set
    End Property
    Private cmbSubItemName As String = ""
    Public Property p_cmbSubItemName() As String
        Get
            Return cmbSubItemName
        End Get
        Set(ByVal value As String)
            cmbSubItemName = value
        End Set
    End Property
    Private txtTagNo As String = ""
    Public Property p_txtTagNo() As String
        Get
            Return txtTagNo
        End Get
        Set(ByVal value As String)
            txtTagNo = value
        End Set
    End Property
    Private cmbDesigner As String = "ALL"
    Public Property p_cmbDesigner() As String
        Get
            Return cmbDesigner
        End Get
        Set(ByVal value As String)
            cmbDesigner = value
        End Set
    End Property
    Private cmbCounterName As String = "ALL"
    Public Property p_cmbCounterName() As String
        Get
            Return cmbCounterName
        End Get
        Set(ByVal value As String)
            cmbCounterName = value
        End Set
    End Property
    Private cmbItemType As String = "ALL"
    Public Property p_cmbItemType() As String
        Get
            Return cmbItemType
        End Get
        Set(ByVal value As String)
            cmbItemType = value
        End Set
    End Property
    Private cmbSize As String = "ALL"
    Public Property p_cmbSize() As String
        Get
            Return cmbSize
        End Get
        Set(ByVal value As String)
            cmbSize = value
        End Set
    End Property
    Private cmbCostCenter As String = "ALL"
    Public Property p_cmbCostCenter() As String
        Get
            Return cmbCostCenter
        End Get
        Set(ByVal value As String)
            cmbCostCenter = value
        End Set
    End Property
    Private txtLotNo_NUM As String = ""
    Public Property p_txtLotNo_NUM() As String
        Get
            Return txtLotNo_NUM
        End Get
        Set(ByVal value As String)
            txtLotNo_NUM = value
        End Set
    End Property
    Private txtFromWt_WET As String = ""
    Public Property p_txtFromWt_WET() As String
        Get
            Return txtFromWt_WET
        End Get
        Set(ByVal value As String)
            txtFromWt_WET = value
        End Set
    End Property
    Private txtToWt_WET As String = ""
    Public Property p_txtToWt_WET() As String
        Get
            Return txtToWt_WET
        End Get
        Set(ByVal value As String)
            txtToWt_WET = value
        End Set
    End Property
    Private txtFromDiaWt_WET As String = ""
    Public Property p_txtFromDiaWt_WET() As String
        Get
            Return txtFromDiaWt_WET
        End Get
        Set(ByVal value As String)
            txtFromDiaWt_WET = value
        End Set
    End Property
    Private txtToDiaWt_WET As String = ""
    Public Property p_txtToDiaWt_WET() As String
        Get
            Return txtToDiaWt_WET
        End Get
        Set(ByVal value As String)
            txtToDiaWt_WET = value
        End Set
    End Property
    Private txtFromRate_WET As String = ""
    Public Property p_txtFromRate_WET() As String
        Get
            Return txtFromRate_WET
        End Get
        Set(ByVal value As String)
            txtFromRate_WET = value
        End Set
    End Property
    Private txtToRate_WET As String = ""
    Public Property p_txtToRate_WET() As String
        Get
            Return txtToRate_WET
        End Get
        Set(ByVal value As String)
            txtToRate_WET = value
        End Set
    End Property
    Private cmbGroup As String = "NONE"
    Public Property p_cmbGroup() As String
        Get
            Return cmbGroup
        End Get
        Set(ByVal value As String)
            cmbGroup = value
        End Set
    End Property

    Private rbtAll As Boolean = True
    Public Property p_rbtAll() As Boolean
        Get
            Return rbtAll
        End Get
        Set(ByVal value As Boolean)
            rbtAll = value
        End Set
    End Property

    Private rbtRegular As Boolean = False
    Public Property p_rbtRegular() As Boolean
        Get
            Return rbtRegular
        End Get
        Set(ByVal value As Boolean)
            rbtRegular = value
        End Set
    End Property

    Private rbtOrder As Boolean = False
    Public Property p_rbtOrder() As Boolean
        Get
            Return rbtOrder
        End Get
        Set(ByVal value As Boolean)
            rbtOrder = value
        End Set
    End Property

    Private chkApproval As Boolean = True
    Public Property p_chkApproval() As Boolean
        Get
            Return chkApproval
        End Get
        Set(ByVal value As Boolean)
            chkApproval = value
        End Set
    End Property

    Private chkStudColumn As Boolean = True
    Public Property p_chkStudColumn() As Boolean
        Get
            Return chkStudColumn
        End Get
        Set(ByVal value As Boolean)
            chkStudColumn = value
        End Set
    End Property
End Class



