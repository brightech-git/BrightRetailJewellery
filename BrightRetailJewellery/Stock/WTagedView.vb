Imports System.Data.OleDb
Public Class WTagedView
    Dim objGridShower As frmGridDispDia
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim strSql As String
    Dim count As Integer = 0
    Dim Sysid As String = ""

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

    Private Function WGetMetalRate() As Double
        Dim purityId As String = Nothing
        ''objGPack.GetSqlValue("SELECT CATCODE FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = " & saItemTypeId & " AND RATEGET = 'Y'", , )
        'If wcmbPurity.Text <> "" Then
        'purityId = objGPack.GetSqlValue("SELECT PURITYID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType.Text & "' AND RATEGET = 'Y' AND SOFTMODULE = 'S'", , )
        'End If
        If Not Trim(purityId).Length > 0 Then
            purityId = objGPack.GetSqlValue("SELECT PURITYID FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & Val(txtItemCode_NUM.Text) & ")")
        End If
        If purityId = "" Then Return 0
        Dim metalId As String = objGPack.GetSqlValue("SELECT METALID FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = " & purityId & "")

        Dim rate As Double = Nothing
        strSql = " SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST AS M"
        strSql += " WHERE RDATE = '" & dtpAsOnDate.Value & "'"
        strSql += " AND RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = M.RDATE)"
        strSql += " AND METALID = '" & metalId & "'"
        strSql += " AND PURITY = (SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = '" & purityId & "')"
        strSql += " ORDER BY SNO DESC"
        rate = Val(objGPack.GetSqlValue(strSql, , , tran))
        If IsDate(dtpAsOnDate.Value) Then
            Return rate
        Else
            Return 0
        End If
    End Function

    Private Function WebtagView()
        Sysid = Trim(LSet(Mid(Guid.NewGuid.ToString, 1, InStr(Guid.NewGuid.ToString, "-") - 1), 10))
        Dim metalrate As Double = Math.Round(WGetMetalRate())
        strSql = vbCrLf + "  IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & Sysid & "TAGSTOCKVIEW')>0 "
        strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW"
        strSql += vbCrLf + "  SELECT IDENTITY(INT,1,1)KEYNO,I.CATCODE AS CATCODE,W.SNO,W.DESCRIP,W.TAGNO,I.ITEMNAME"
        strSql += vbCrLf + "  ," & cnAdminDb & ".DBO.FUNCSPECIFIC(W.SNO,CASE WHEN ISNULL(W.REASON,'')='' THEN CAST(W.SUBITEMID AS VARCHAR)+',' ELSE W.REASON+',' END,'M') AS SUBITEMNAME"
        strSql += vbCrLf + "  ," & cnAdminDb & ".DBO.FUNCSPECIFIC(W.SNO,'1','S') AS CATEGORY"
        strSql += vbCrLf + "  ," & cnAdminDb & ".DBO.FUNCSPECIFIC(W.SNO,'2','S') AS COLLECTION"
        strSql += vbCrLf + "  ,NARRATION,W.HEIGHT,W.WIDTH,IT.NAME AS MPURITY,W.GRSWT,W.NETWT,ROUND(CONVERT(NUMERIC(15,2),ISNULL(W.NETWT,0)*" & IIf(metalrate <> 0, metalrate, "ISNULL(RATE,0)") & "),0) AS SALVALUE,ROUND(W.MAXMC,0) MAXMC"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(200),NULL)DNAME1,CONVERT(VARCHAR(200),NULL)DSHAPE1,CONVERT(VARCHAR(200),NULL)DCLARITY1,CONVERT(VARCHAR(200),NULL)DCOLOR1,CONVERT(VARCHAR(50),NULL)DPCS1,CONVERT(VARCHAR(50),NULL)DWT1"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(50),NULL)DAMTSIIJ1,CONVERT(VARCHAR(50),NULL)DAMTSIGH1,CONVERT(VARCHAR(50),NULL)DAMTVSSIGH1"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(50),NULL)DAMTVSGH1,CONVERT(VARCHAR(50),NULL)DAMTVVSVSGH1"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(50),NULL)DAMTVVSEF1,CONVERT(VARCHAR(50),NULL)DAMTVVSDEF1"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(200),NULL)DNAME2,CONVERT(VARCHAR(200),NULL)DSHAPE2,CONVERT(VARCHAR(200),NULL)DCLARITY2,CONVERT(VARCHAR(200),NULL)DCOLOR2,CONVERT(VARCHAR(50),NULL)DPCS2,CONVERT(VARCHAR(50),NULL)DWT2"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(50),NULL)DAMTSIIJ2,CONVERT(VARCHAR(50),NULL)DAMTSIGH2,CONVERT(VARCHAR(50),NULL)DAMTVSSIGH2"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(50),NULL)DAMTVSGH2,CONVERT(VARCHAR(50),NULL)DAMTVVSVSGH2"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(50),NULL)DAMTVVSEF2,CONVERT(VARCHAR(50),NULL)DAMTVVSDEF2"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(200),NULL)DNAME3,CONVERT(VARCHAR(200),NULL)DSHAPE3,CONVERT(VARCHAR(200),NULL)DCLARITY3,CONVERT(VARCHAR(200),NULL)DCOLOR3,CONVERT(VARCHAR(50),NULL)DPCS3,CONVERT(VARCHAR(50),NULL)DWT3"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(50),NULL)DAMTSIIJ3,CONVERT(VARCHAR(50),NULL)DAMTSIGH3,CONVERT(VARCHAR(50),NULL)DAMTVSSIGH3"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(50),NULL)DAMTVSGH3,CONVERT(VARCHAR(50),NULL)DAMTVVSVSGH3"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(50),NULL)DAMTVVSEF3,CONVERT(VARCHAR(50),NULL)DAMTVVSDEF3"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(200),NULL)DNAME4,CONVERT(VARCHAR(200),NULL)DSHAPE4,CONVERT(VARCHAR(200),NULL)DCLARITY4,CONVERT(VARCHAR(200),NULL)DCOLOR4,CONVERT(VARCHAR(50),NULL)DPCS4,CONVERT(VARCHAR(50),NULL)DWT4"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(50),NULL)DAMTSIIJ4,CONVERT(VARCHAR(50),NULL)DAMTSIGH4,CONVERT(VARCHAR(50),NULL)DAMTVSSIGH4"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(50),NULL)DAMTVSGH4,CONVERT(VARCHAR(50),NULL)DAMTVVSVSGH4"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(50),NULL)DAMTVVSEF4,CONVERT(VARCHAR(50),NULL)DAMTVVSDEF4"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(200),NULL)DNAME5,CONVERT(VARCHAR(200),NULL)DSHAPE5,CONVERT(VARCHAR(200),NULL)DCLARITY5,CONVERT(VARCHAR(200),NULL)DCOLOR5,CONVERT(VARCHAR(50),NULL)DPCS5,CONVERT(VARCHAR(50),NULL)DWT5"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(50),NULL)DAMTSIIJ5,CONVERT(VARCHAR(50),NULL)DAMTSIGH5,CONVERT(VARCHAR(50),NULL)DAMTVSSIGH5"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(50),NULL)DAMTVSGH5,CONVERT(VARCHAR(50),NULL)DAMTVVSVSGH5"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(50),NULL)DAMTVVSEF5,CONVERT(VARCHAR(50),NULL)DAMTVVSDEF5"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(200),NULL)SNAME1,CONVERT(VARCHAR(200),NULL)SSHAPE1,CONVERT(VARCHAR(50),NULL)SPCS1,CONVERT(VARCHAR(50),NULL)SWT1,CONVERT(VARCHAR(50),NULL)SAMT1"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(200),NULL)SNAME2,CONVERT(VARCHAR(200),NULL)SSHAPE2,CONVERT(VARCHAR(50),NULL)SPCS2,CONVERT(VARCHAR(50),NULL)SWT2,CONVERT(VARCHAR(50),NULL)SAMT2"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(200),NULL)SNAME3,CONVERT(VARCHAR(200),NULL)SSHAPE3,CONVERT(VARCHAR(50),NULL)SPCS3,CONVERT(VARCHAR(50),NULL)SWT3,CONVERT(VARCHAR(50),NULL)SAMT3"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(200),NULL)SNAME4,CONVERT(VARCHAR(200),NULL)SSHAPE4,CONVERT(VARCHAR(50),NULL)SPCS4,CONVERT(VARCHAR(50),NULL)SWT4,CONVERT(VARCHAR(50),NULL)SAMT4"
        strSql += vbCrLf + "  ,CONVERT(VARCHAR(200),NULL)SNAME5,CONVERT(VARCHAR(200),NULL)SSHAPE5,CONVERT(VARCHAR(50),NULL)SPCS5,CONVERT(VARCHAR(50),NULL)SWT5,CONVERT(VARCHAR(50),NULL)SAMT5"
        strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW FROM " & cnAdminDb & "..WITEMTAG W"
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON W.ITEMID=I.ITEMID "
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON W.ITEMID=SI.ITEMID AND W.SUBITEMID=SI.SUBITEMID "
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..ITEMTYPE IT ON W.ITEMTYPEID =IT.ITEMTYPEID "
        strSql += vbCrLf + "  WHERE W.RECDATE <= '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "'"
        If Not cnCentStock Then strSql += " AND W.COMPANYID = '" & GetStockCompId() & "'"
        If txtItemCode_NUM.Text <> "" Then
            strSql += vbCrLf + "  AND W.ITEMID = '" & txtItemCode_NUM.Text & "'"
        End If
        If txtTagNo.Text <> "" Then
            strSql += vbCrLf + "  AND W.TAGNO = '" & txtTagNo.Text & "'"
        End If
        strSql += vbCrLf + "  AND (W.ISSDATE IS NULL OR W.ISSDATE > '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "')"

        If cmbItemType.Text <> "ALL" Then
            strSql += vbCrLf + "  AND IT.NAME = '" & cmbItemType.Text & "'"
        End If
        If cmbCostCenter.Text <> "ALL" And cmbCostCenter.Text <> "" Then
            strSql += vbCrLf + "  AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter.Text & "')"
        End If

        If txtFromWt_WET.Text <> "" Or txtToWt_WET.Text <> "" Then
            strSql += vbCrLf + "  AND (W.GRSWT BETWEEN '" & Val(txtFromWt_WET.Text) & "' AND '" & Val(txtToWt_WET.Text) & "')"
        End If
        If txtFromDiaWt_WET.Text <> "" Or txtToDiaWt_WET.Text <> "" Then
            strSql += " AND ((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = W.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) BETWEEN '" & Val(txtFromDiaWt_WET.Text) & "' AND '" & Val(txtToDiaWt_WET.Text) & "')"
        End If
        If txtFromRate_WET.Text <> "" Or txtToRate_WET.Text <> "" Then
            strSql += vbCrLf + "  AND (W.SALVALUE BETWEEN '" & Val(txtFromRate_WET.Text) & "' AND '" & Val(txtToRate_WET.Text) & "')"
        End If
        If txtSearch_txt.Text <> "" Then
            strSql += vbCrLf + " AND W." & cmbSearchKey.Text & " LIKE"
            strSql += vbCrLf + " '" & txtSearch_txt.Text & "%'"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + "UPDATE A SET A.SALVALUE = ROUND(CONVERT(NUMERIC(15,2),A.NETWT*"
        strSql += vbCrLf + "ISNULL((SELECT TOP 1 SRATE FROM " & cnAdminDb & "..RATEMAST AS M"
        strSql += vbCrLf + " WHERE RDATE = '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND RATEGROUP = (SELECT MAX(RATEGROUP) FROM " & cnAdminDb & "..RATEMAST WHERE RDATE = M.RDATE)"
        strSql += vbCrLf + " AND METALID = C.METALID AND PURITY = (SELECT PURITY FROM " & cnAdminDb & "..PURITYMAST WHERE PURITYID = C.PURITYID)"
        strSql += vbCrLf + "ORDER BY SNO DESC ),0)),0)"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW A "
        strSql += vbCrLf + "INNER JOIN " & cnAdminDb & "..CATEGORY C ON A.CATCODE= C.CATCODE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        'FOR DIAMOND
        strSql = vbCrLf + "  IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & Sysid & "DIAMOND')>0 "
        strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & Sysid & "DIAMOND"
        strSql += vbCrLf + " SELECT  S.SHAPENAME,I.ITEMNAME,SI.SUBITEMNAME ,WS.STNPCS,WS.STNWT,WS.STNRATE,WS.TAGSNO,WS.TAGNO,WS.SHAPEID,CL.CLARITYNAME,CO.COLORNAME "
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & Sysid & "DIAMOND FROM " & cnAdminDb & "..WITEMTAGSTONE WS"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..WITEMTAG W ON WS.TAGSNO=W.SNO"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..STNSHAPE S ON S.SHAPEID =WS.SHAPEID "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON WS.STNITEMID=I.ITEMID "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON WS.STNITEMID=SI.ITEMID  AND SI.SUBITEMID=WS.STNSUBITEMID "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..STNCLARITY CL ON CL.CLARITYID =WS.CLARITYID  "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..STNCOLOR CO ON CO.COLORID =WS.COLORID "
        strSql += vbCrLf + " WHERE W.RECDATE<= '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "' AND I.METALID ='D'"
        strSql += vbCrLf + " AND WS.TAGSNO IN(SELECT SNO FROM TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW) "
        strSql += vbCrLf + " AND ISNULL(WS.TAGSNO,'')<>'' AND ISNULL(WS.TAGNO,'')<>'' "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        'FOR STONE
        strSql = vbCrLf + "  IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & Sysid & "STONE')>0 "
        strSql += vbCrLf + "  DROP TABLE TEMPTABLEDB..TEMP" & Sysid & "STONE"
        strSql += vbCrLf + " SELECT  S.SHAPENAME,CASE WHEN ISNULL(SI.SUBITEMNAME,'')<>'' THEN SI.SUBITEMNAME ELSE I.ITEMNAME END ITEMNAME"
        strSql += vbCrLf + " ,WS.STNPCS,WS.STNWT,WS.STNRATE,ROUND(WS.STNAMT,0)STNAMT,WS.TAGSNO,WS.TAGNO "
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & Sysid & "STONE FROM " & cnAdminDb & "..WITEMTAGSTONE WS"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..WITEMTAG W ON WS.TAGSNO=W.SNO"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..STNSHAPE S ON S.SHAPEID =WS.SHAPEID "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST I ON WS.STNITEMID=I.ITEMID "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST SI ON WS.STNITEMID=SI.ITEMID  AND SI.SUBITEMID=WS.STNSUBITEMID "
        strSql += vbCrLf + " WHERE W.RECDATE<= '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "' AND I.METALID <>'D'"
        strSql += vbCrLf + " AND WS.TAGSNO IN(SELECT SNO FROM TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW) "
        strSql += vbCrLf + " AND ISNULL(WS.TAGSNO,'')<>'' AND ISNULL(WS.TAGNO,'')<>'' "
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW ADD TOTDPCS INTEGER"
        strSql += vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW ADD TOTDWT VARCHAR(100)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = vbCrLf + "SELECT DISTINCT DISP1,DISP2,DISP3,UNIQUECODE FROM ("
        strSql += vbCrLf + "SELECT (SELECT DISPORDER FROM " & cnAdminDb & "..STNCUT WHERE CUTID = D.CUTID)DISP1"
        strSql += vbCrLf + ",(SELECT DISPORDER FROM " & cnAdminDb & "..STNCOLOR WHERE COLORID = D.COLORID)DISP2"
        strSql += vbCrLf + ",(SELECT DISPORDER FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYID = D.CLARITYID)DISP3"
        strSql += vbCrLf + ",UNIQUECODE  FROM " & cnAdminDb & "..DIASTYLE D)X ORDER BY DISP1,DISP2,DISP3,UNIQUECODE"
        'strSql = " SELECT DISTINCT UNIQUECODE FROM " & cnAdminDb & "..DIASTYLE WHERE SHAPEID IN(SELECT SHAPEID FROM TEMPTABLEDB..TEMP" & Sysid & "DIAMOND) "
        Dim dtShape As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtShape)
        For v As Integer = 0 To dtShape.Rows.Count - 1
            strSql = " ALTER TABLE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW ADD [D-" & dtShape.Rows(v).Item(3).ToString & "] NUMERIC(15,2)"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        Next
        strSql = "SELECT COUNT(*)CNT,TAGSNO FROM TEMPTABLEDB..TEMP" & Sysid & "DIAMOND GROUP BY TAGSNO ORDER BY COUNT(*) DESC"
        Dim diaCnt As Integer = Val(objGPack.GetSqlValue(strSql, , "0").ToString)

        strSql = "SELECT COUNT(*)CNT,TAGSNO FROM TEMPTABLEDB..TEMP" & Sysid & "STONE GROUP BY TAGSNO ORDER BY COUNT(*) DESC"
        Dim stnCnt As Integer = Val(objGPack.GetSqlValue(strSql, , "0").ToString)

        strSql = "ALTER TABLE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW ADD TOTSTNPCS INTEGER"
        strSql += vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW ADD TOTSTNWT VARCHAR(100)"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " ALTER TABLE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW ADD STNAMT VARCHAR(100)"
        strSql += vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW DROP COLUMN CATCODE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        'For v As Integer = 0 To dtShape.Rows.Count - 1
        '    strSql = " ALTER TABLE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW ADD [VAT-" & dtShape.Rows(v).Item(0).ToString & "] NUMERIC(15,2)"
        '    strSql += " ALTER TABLE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW ADD [TOT-" & dtShape.Rows(v).Item(0).ToString & "] NUMERIC(15,2)"
        '    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        '    cmd.ExecuteNonQuery()
        'Next
        strSql = Nothing
        strSql += vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW ADD TOTDAMTSIIJ VARCHAR(100)"
        strSql += vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW ADD TOTDVATSIIJ VARCHAR(100)"

        strSql += vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW ADD TOTDAMTSIGH VARCHAR(100)"
        strSql += vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW ADD TOTDVATSIGH VARCHAR(100)"

        strSql += vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW ADD TOTDAMTVSSIGH VARCHAR(100)"
        strSql += vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW ADD TOTDVATVSSIGH VARCHAR(100)"

        strSql += vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW ADD TOTDAMTVSGH VARCHAR(100)"
        strSql += vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW ADD TOTDVATVSGH VARCHAR(100)"

        strSql += vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW ADD TOTDAMTVVSVSGH VARCHAR(100)"
        strSql += vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW ADD TOTDVATVVSVSGH VARCHAR(100)"

        strSql += vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW ADD TOTDAMTVVSEF VARCHAR(100)"
        strSql += vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW ADD TOTDVATVVSEF VARCHAR(100)"

        strSql += vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW ADD TOTDAMTVVSDEF VARCHAR(100)"
        strSql += vbCrLf + " ALTER TABLE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW ADD TOTDVATVVSDEF VARCHAR(100)"

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "SELECT MISCID,MISCNAME FROM " & cnAdminDb & "..OTHERMASTERENTRY WHERE MISCID>2 ORDER BY MISCID"
        Dim dtMisc As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtMisc)
        For k As Integer = 0 To dtMisc.Rows.Count - 1
            strSql = " ALTER TABLE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW ADD [" & dtMisc.Rows(k)("MISCNAME").ToString & "] VARCHAR(400)"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = "UPDATE A SET [" & dtMisc.Rows(k)("MISCNAME").ToString & "]=" & cnAdminDb & ".DBO.FUNCSPECIFIC(A.SNO,'" & dtMisc.Rows(k)("MISCID").ToString & "','S')"
            strSql += vbCrLf + "FROM TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW AS A"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        Next


        strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW ORDER BY KEYNO"
        Dim dtDet As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDet)
        For v As Integer = 0 To dtDet.Rows.Count - 1
            Dim Dshape As String = ""
            Dim DColor As String = ""
            Dim DClarity As String = ""
            Dim Dpcs As String = ""
            Dim Dwt As String = ""
            Dim Stnname As String = ""
            Dim Dianame As String = ""
            Dim StnnShape As String = ""
            Dim Spcs As String = ""
            Dim Swt As String = ""
            Dim Samt As Decimal = 0
            Dim SamtTot As String = ""
            Dim DAmtSIGH As String = ""
            Dim DAmtSIIJ As String = ""
            Dim DAmtVSGH As String = ""
            Dim DAmtVVSEF As String = ""

            Dim DAmtVVSVSGH As String = ""
            Dim DAmtVSSIGH As String = ""
            Dim DAmtVVSDEF As String = ""

            strSql = "SELECT * FROM TEMPTABLEDB..TEMP" & Sysid & "STONE WHERE TAGSNO='" & dtDet.Rows(v).Item("SNO").ToString & "' AND TAGNO='" & dtDet.Rows(v).Item("TAGNO").ToString & "'"
            Dim dtStone As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtStone)

            Dim i As Integer = 1
            For Each roStn As DataRow In dtStone.Rows
                Spcs = Spcs + IIf(roStn!STNPCS.ToString.Trim <> "", roStn!STNPCS.ToString + ",", "")
                Swt = Swt + IIf(roStn!STNWT.ToString.Trim <> "", roStn!STNWT.ToString + ",", "")
                Samt += Val(roStn!STNAMT.ToString)
                SamtTot = SamtTot + IIf(roStn!STNAMT.ToString.Trim <> "", roStn!STNAMT.ToString + ",", "")
                StnnShape = StnnShape + IIf(roStn!SHAPENAME.ToString.Trim <> "", roStn!SHAPENAME.ToString + ",", "")
                Stnname = Stnname + IIf(roStn!ITEMNAME.ToString.Trim <> "", roStn!ITEMNAME.ToString + ",", "")
            Next
            If Spcs <> "" Then Spcs = Mid(Spcs, 1, Len(Spcs) - 1)
            If Swt <> "" Then Swt = Mid(Swt, 1, Len(Swt) - 1)
            If Stnname <> "" Then Stnname = Mid(Stnname, 1, Len(Stnname) - 1)
            If StnnShape <> "" Then StnnShape = Mid(StnnShape, 1, Len(StnnShape) - 1)

            strSql = "SELECT * FROM TEMPTABLEDB..TEMP" & Sysid & "DIAMOND WHERE TAGSNO='" & dtDet.Rows(v).Item("SNO").ToString & "' AND TAGNO='" & dtDet.Rows(v).Item("TAGNO").ToString & "'"
            Dim dtDiamond As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtDiamond)
            i = 1
            For Each roDia As DataRow In dtDiamond.Rows
                Dpcs = Dpcs + IIf(roDia!STNPCS.ToString.Trim <> "", roDia!STNPCS.ToString + ",", "")
                Dwt = Dwt + IIf(roDia!STNWT.ToString.Trim <> "", roDia!STNWT.ToString + ",", "")
                'Dshape = Dshape + IIf(roDia!SHAPENAME.ToString.Trim <> "" And Dshape.Contains(roDia!SHAPENAME.ToString.Trim) = False, roDia!SHAPENAME.ToString + ",", "")
                Dshape = Dshape + IIf(roDia!SHAPENAME.ToString.Trim <> "", roDia!SHAPENAME.ToString + ",", "")
                'DClarity = DClarity + IIf(roDia!CLARITYNAME.ToString.Trim <> "" And DClarity.Contains(roDia!CLARITYNAME.ToString.Trim) = False, roDia!CLARITYNAME.ToString + ",", "")
                DClarity = DClarity + IIf(roDia!CLARITYNAME.ToString.Trim <> "", roDia!CLARITYNAME.ToString + ",", "")
                DColor = DColor + IIf(roDia!COLORNAME.ToString.Trim <> "", roDia!COLORNAME.ToString + ",", "")
                Dianame = Dianame + IIf(roDia!ITEMNAME.ToString.Trim <> "", roDia!ITEMNAME.ToString + ",", "")
                Dim centrate As Double = 0
                Dim cent As Double

                cent = 0
                strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & roDia!ITEMNAME.ToString.Trim & "'"
                Dim mCaltype As String = objGPack.GetSqlValue(strSql, "CALTYPE", "")
                strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(SUBITEMNAME,'') ='" & roDia!SUBITEMNAME.ToString.Trim & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & roDia!ITEMNAME.ToString.Trim & "')"
                mCaltype = objGPack.GetSqlValue(strSql, "CALTYPE", "")
                If mCaltype = "D" Then
                    cent = Val(roDia!STNWT.ToString.Trim)
                Else
                    cent = (Val(roDia!STNWT.ToString.Trim) / IIf(Val(roDia!STNPCS.ToString.Trim) = 0, 1, Val(roDia!STNPCS.ToString.Trim)))
                End If
                cent *= 100

                strSql = "SELECT TOP 1 MAXRATE FROM " & cnAdminDb & "..CENTRATE "
                strSql += vbCrLf + "WHERE 1=1 "
                strSql += vbCrLf + "AND CLARITYID IN (SELECT TOP 1 CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME='SI') "
                strSql += vbCrLf + "AND COLORID IN (SELECT TOP 1 COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME='GH') "
                strSql += vbCrLf + "AND SHAPEID IN (SELECT TOP 1 SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME='" & roDia!SHAPENAME.ToString.Trim & "') "
                strSql += vbCrLf + "AND ITEMID IN (SELECT TOP 1 ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & roDia!ITEMNAME.ToString.Trim & "') "
                strSql += vbCrLf + "AND SUBITEMID IN (SELECT TOP 1 SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & roDia!SUBITEMNAME.ToString.Trim & "') "
                strSql += vbCrLf + "AND " & cent & " BETWEEN FROMCENT AND TOCENT "
                strSql += vbCrLf + "AND COSTID='" & cnCostId & "'"
                centrate = Val(objGPack.GetSqlValue(strSql, "", "0").ToString)
                centrate = Format(Math.Round(centrate * Val(roDia!STNWT.ToString.Trim), 2), "0.00")
                DAmtSIGH = DAmtSIGH + IIf(centrate.ToString <> "", centrate.ToString + ",", "")

                strSql = "SELECT TOP 1 MAXRATE FROM " & cnAdminDb & "..CENTRATE "
                strSql += vbCrLf + "WHERE 1=1 "
                strSql += vbCrLf + "AND CLARITYID IN (SELECT TOP 1 CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME='SI') "
                strSql += vbCrLf + "AND COLORID IN (SELECT TOP 1 COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME='IJ') "
                strSql += vbCrLf + "AND SHAPEID IN (SELECT TOP 1 SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME='" & roDia!SHAPENAME.ToString.Trim & "') "
                strSql += vbCrLf + "AND ITEMID IN (SELECT TOP 1 ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & roDia!ITEMNAME.ToString.Trim & "') "
                strSql += vbCrLf + "AND SUBITEMID IN (SELECT TOP 1 SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & roDia!SUBITEMNAME.ToString.Trim & "') "
                strSql += vbCrLf + "AND " & cent & " BETWEEN FROMCENT AND TOCENT "
                strSql += vbCrLf + "AND COSTID='" & cnCostId & "'"
                centrate = Val(objGPack.GetSqlValue(strSql, "", "0").ToString)
                centrate = Format(Math.Round(centrate * Val(roDia!STNWT.ToString.Trim), 2), "0.00")
                DAmtSIIJ = DAmtSIIJ + IIf(centrate.ToString <> "", centrate.ToString + ",", "")

                strSql = "SELECT TOP 1 MAXRATE FROM " & cnAdminDb & "..CENTRATE "
                strSql += vbCrLf + "WHERE 1=1 "
                strSql += vbCrLf + "AND CLARITYID IN (SELECT TOP 1 CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME='VS') "
                strSql += vbCrLf + "AND COLORID IN (SELECT TOP 1 COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME='GH') "
                strSql += vbCrLf + "AND SHAPEID IN (SELECT TOP 1 SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME='" & roDia!SHAPENAME.ToString.Trim & "') "
                strSql += vbCrLf + "AND ITEMID IN (SELECT TOP 1 ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & roDia!ITEMNAME.ToString.Trim & "') "
                strSql += vbCrLf + "AND SUBITEMID IN (SELECT TOP 1 SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & roDia!SUBITEMNAME.ToString.Trim & "') "
                strSql += vbCrLf + "AND " & cent & " BETWEEN FROMCENT AND TOCENT "
                strSql += vbCrLf + "AND COSTID='" & cnCostId & "'"
                centrate = Val(objGPack.GetSqlValue(strSql, "", "0").ToString)
                centrate = Format(Math.Round(centrate * Val(roDia!STNWT.ToString.Trim), 2), "0.00")
                DAmtVSGH = DAmtVSGH + IIf(centrate.ToString <> "", centrate.ToString + ",", "")

                strSql = "SELECT TOP 1 MAXRATE FROM " & cnAdminDb & "..CENTRATE "
                strSql += vbCrLf + "WHERE 1=1 "
                strSql += vbCrLf + "AND CLARITYID IN (SELECT TOP 1 CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME='VVS') "
                strSql += vbCrLf + "AND COLORID IN (SELECT TOP 1 COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME='EF') "
                strSql += vbCrLf + "AND SHAPEID IN (SELECT TOP 1 SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME='" & roDia!SHAPENAME.ToString.Trim & "') "
                strSql += vbCrLf + "AND ITEMID IN (SELECT TOP 1 ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & roDia!ITEMNAME.ToString.Trim & "') "
                strSql += vbCrLf + "AND SUBITEMID IN (SELECT TOP 1 SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & roDia!SUBITEMNAME.ToString.Trim & "') "
                strSql += vbCrLf + "AND " & cent & " BETWEEN FROMCENT AND TOCENT "
                strSql += vbCrLf + "AND COSTID='" & cnCostId & "'"
                centrate = Val(objGPack.GetSqlValue(strSql, "", "0").ToString)
                centrate = Format(Math.Round(centrate * Val(roDia!STNWT.ToString.Trim), 2), "0.00")
                DAmtVVSEF = DAmtVVSEF + IIf(centrate.ToString <> "", centrate.ToString + ",", "")


                strSql = "SELECT TOP 1 MAXRATE FROM " & cnAdminDb & "..CENTRATE "
                strSql += vbCrLf + "WHERE 1=1 "
                strSql += vbCrLf + "AND CLARITYID IN (SELECT TOP 1 CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME='VVS VS') "
                strSql += vbCrLf + "AND COLORID IN (SELECT TOP 1 COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME='GH') "
                strSql += vbCrLf + "AND SHAPEID IN (SELECT TOP 1 SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME='" & roDia!SHAPENAME.ToString.Trim & "') "
                strSql += vbCrLf + "AND ITEMID IN (SELECT TOP 1 ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & roDia!ITEMNAME.ToString.Trim & "') "
                strSql += vbCrLf + "AND SUBITEMID IN (SELECT TOP 1 SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & roDia!SUBITEMNAME.ToString.Trim & "') "
                strSql += vbCrLf + "AND " & cent & " BETWEEN FROMCENT AND TOCENT "
                strSql += vbCrLf + "AND COSTID='" & cnCostId & "'"
                centrate = Val(objGPack.GetSqlValue(strSql, "", "0").ToString)
                centrate = Format(Math.Round(centrate * Val(roDia!STNWT.ToString.Trim), 2), "0.00")
                DAmtVVSVSGH = DAmtVVSVSGH + IIf(centrate.ToString <> "", centrate.ToString + ",", "")


                strSql = "SELECT TOP 1 MAXRATE FROM " & cnAdminDb & "..CENTRATE "
                strSql += vbCrLf + "WHERE 1=1 "
                strSql += vbCrLf + "AND CLARITYID IN (SELECT TOP 1 CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME='VS SI') "
                strSql += vbCrLf + "AND COLORID IN (SELECT TOP 1 COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME='GH') "
                strSql += vbCrLf + "AND SHAPEID IN (SELECT TOP 1 SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME='" & roDia!SHAPENAME.ToString.Trim & "') "
                strSql += vbCrLf + "AND ITEMID IN (SELECT TOP 1 ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & roDia!ITEMNAME.ToString.Trim & "') "
                strSql += vbCrLf + "AND SUBITEMID IN (SELECT TOP 1 SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & roDia!SUBITEMNAME.ToString.Trim & "') "
                strSql += vbCrLf + "AND " & cent & " BETWEEN FROMCENT AND TOCENT "
                strSql += vbCrLf + "AND COSTID='" & cnCostId & "'"
                centrate = Val(objGPack.GetSqlValue(strSql, "", "0").ToString)
                centrate = Format(Math.Round(centrate * Val(roDia!STNWT.ToString.Trim), 2), "0.00")
                DAmtVSSIGH = DAmtVSSIGH + IIf(centrate.ToString <> "", centrate.ToString + ",", "")


                strSql = "SELECT TOP 1 MAXRATE FROM " & cnAdminDb & "..CENTRATE "
                strSql += vbCrLf + "WHERE 1=1 "
                strSql += vbCrLf + "AND CLARITYID IN (SELECT TOP 1 CLARITYID FROM " & cnAdminDb & "..STNCLARITY WHERE CLARITYNAME='VVS') "
                strSql += vbCrLf + "AND COLORID IN (SELECT TOP 1 COLORID FROM " & cnAdminDb & "..STNCOLOR WHERE COLORNAME='DEF') "
                strSql += vbCrLf + "AND SHAPEID IN (SELECT TOP 1 SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME='" & roDia!SHAPENAME.ToString.Trim & "') "
                strSql += vbCrLf + "AND ITEMID IN (SELECT TOP 1 ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & roDia!ITEMNAME.ToString.Trim & "') "
                strSql += vbCrLf + "AND SUBITEMID IN (SELECT TOP 1 SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME='" & roDia!SUBITEMNAME.ToString.Trim & "') "
                strSql += vbCrLf + "AND " & cent & " BETWEEN FROMCENT AND TOCENT "
                strSql += vbCrLf + "AND COSTID='" & cnCostId & "'"
                centrate = Val(objGPack.GetSqlValue(strSql, "", "0").ToString)
                centrate = Format(Math.Round(centrate * Val(roDia!STNWT.ToString.Trim), 2), "0.00")
                DAmtVVSDEF = DAmtVVSDEF + IIf(centrate.ToString <> "", centrate.ToString + ",", "")
            Next
            If Dpcs <> "" Then Dpcs = Mid(Dpcs, 1, Len(Dpcs) - 1)
            If Dwt <> "" Then Dwt = Mid(Dwt, 1, Len(Dwt) - 1)
            If Dshape <> "" Then Dshape = Mid(Dshape, 1, Len(Dshape) - 1)
            If DClarity <> "" Then DClarity = Mid(DClarity, 1, Len(DClarity) - 1)
            If DColor <> "" Then DColor = Mid(DColor, 1, Len(DColor) - 1)
            If Dpcs Is Nothing Then Dpcs = ""
            If Dwt Is Nothing Then Dwt = ""
            If Spcs Is Nothing Then Spcs = ""
            If Swt Is Nothing Then Swt = ""
            Dim DpcsAr As String() = Dpcs.Split(",")
            Dim DwtAr As String() = Dwt.Split(",")
            Dim SpcsAr As String() = Spcs.Split(",")
            Dim SwtAr As String() = Swt.Split(",")
            Dim DshapeAr As String() = Dshape.Split(",")
            Dim DClarityAr As String() = DClarity.Split(",")
            Dim DColorAr As String() = DColor.Split(",")
            Dim DianameAr As String() = Dianame.Split(",")
            Dim StnnameAr As String() = Stnname.Split(",")
            Dim StnnShapeAr As String() = StnnShape.Split(",")
            Dim DAmtSIGHAr As String() = DAmtSIGH.Split(",")
            Dim DAmtSIIJAr As String() = DAmtSIIJ.Split(",")
            Dim DAmtVSGHAr As String() = DAmtVSGH.Split(",")
            Dim DAmtVVSEFAr As String() = DAmtVVSEF.Split(",")

            Dim DAmtVVSVSGHAr As String() = DAmtVVSVSGH.Split(",")
            Dim DAmtVSSIGHAr As String() = DAmtVSSIGH.Split(",")
            Dim DAmtVVSDEFAr As String() = DAmtVVSDEF.Split(",")

            Dim SamtTotAr As String() = SamtTot.Split(",")
            strSql = " UPDATE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW"
            strSql += vbCrLf + " SET STNAMT='" & Samt & "' "
            Dim totDiaPcs As Integer = 0
            Dim totDiaWt As Double = 0
            Dim totStnPcs As Integer = 0
            Dim totStnWt As Double = 0
            Dim totDAmtSIGH As Double = 0
            Dim totDAmtSIIJ As Double = 0
            Dim totDAmtVSGH As Double = 0
            Dim totDAmtVVSEF As Double = 0

            Dim totDAmtVVSVSGH As Double = 0
            Dim totDAmtVSSIGH As Double = 0
            Dim totDAmtVVSDEF As Double = 0

            If Not DAmtSIGHAr Is Nothing Then
                For k As Integer = 0 To IIf(DAmtSIGHAr.Length > 4, 4, DAmtSIGHAr.Length - 1)
                    If k = 0 Then
                        strSql += vbCrLf + " ,DAMTSIGH1='" & DAmtSIGHAr(0).ToString & "'"
                        totDAmtSIGH += Val(DAmtSIGHAr(0).ToString)
                    Else
                        strSql += vbCrLf + " ,DAMTSIGH" & (k + 1).ToString & "='" & DAmtSIGHAr(k).ToString & "'"
                        totDAmtSIGH += Val(DAmtSIGHAr(k).ToString)
                    End If
                Next
            Else
                strSql += vbCrLf + " ,DAMTSIGH1='" & DAmtSIGH & "'"
                totDAmtSIGH += Val(DAmtSIGH.ToString)
            End If

            If Not DAmtSIIJAr Is Nothing Then
                For k As Integer = 0 To IIf(DAmtSIIJAr.Length > 4, 4, DAmtSIIJAr.Length - 1)
                    If k = 0 Then
                        strSql += vbCrLf + " ,DAMTSIIJ1='" & DAmtSIIJAr(0).ToString & "'"
                        totDAmtSIIJ += Val(DAmtSIIJAr(0).ToString)
                    Else
                        strSql += vbCrLf + " ,DAMTSIIJ" & (k + 1).ToString & "='" & DAmtSIIJAr(k).ToString & "'"
                        totDAmtSIIJ += Val(DAmtSIIJAr(k).ToString)
                    End If
                Next
            Else
                strSql += vbCrLf + " ,DAMTSIIJ1='" & DAmtSIIJ & "'"
                totDAmtSIIJ += Val(DAmtSIIJ.ToString)
            End If

            If Not DAmtVSGHAr Is Nothing Then
                For k As Integer = 0 To IIf(DAmtVSGHAr.Length > 4, 4, DAmtVSGHAr.Length - 1)
                    If k = 0 Then
                        strSql += vbCrLf + " ,DAMTVSGH1='" & DAmtVSGHAr(0).ToString & "'"
                        totDAmtVSGH += Val(DAmtVSGHAr(0).ToString)
                    Else
                        strSql += vbCrLf + " ,DAMTVSGH" & (k + 1).ToString & "='" & DAmtVSGHAr(k).ToString & "'"
                        totDAmtVSGH += Val(DAmtVSGHAr(k).ToString)
                    End If
                Next
            Else
                strSql += vbCrLf + " ,DAMTVSGH1='" & DAmtVSGH & "'"
                totDAmtVSGH += Val(DAmtVSGH.ToString)
            End If

            If Not DAmtVVSVSGHAr Is Nothing Then
                For k As Integer = 0 To IIf(DAmtVVSVSGHAr.Length > 4, 4, DAmtVVSVSGHAr.Length - 1)
                    If k = 0 Then
                        strSql += vbCrLf + " ,DAMTVVSVSGH1='" & DAmtVVSVSGHAr(0).ToString & "'"
                        totDAmtVVSVSGH += Val(DAmtVVSVSGHAr(0).ToString)
                    Else
                        strSql += vbCrLf + " ,DAMTVVSVSGH" & (k + 1).ToString & "='" & DAmtVVSVSGHAr(k).ToString & "'"
                        totDAmtVVSVSGH += Val(DAmtVVSVSGHAr(k).ToString)
                    End If
                Next
            Else
                strSql += vbCrLf + " ,DAMTVVSVSGH1='" & DAmtVVSVSGH & "'"
                totDAmtVVSVSGH += Val(DAmtVVSVSGH.ToString)
            End If

            If Not DAmtVSSIGHAr Is Nothing Then
                For k As Integer = 0 To IIf(DAmtVSSIGHAr.Length > 4, 4, DAmtVSSIGHAr.Length - 1)
                    If k = 0 Then
                        strSql += vbCrLf + " ,DAMTVSSIGH1='" & DAmtVSSIGHAr(0).ToString & "'"
                        totDAmtVSSIGH += Val(DAmtVSSIGHAr(0).ToString)
                    Else
                        strSql += vbCrLf + " ,DAMTVSSIGH" & (k + 1).ToString & "='" & DAmtVSSIGHAr(k).ToString & "'"
                        totDAmtVSSIGH += Val(DAmtVSSIGHAr(k).ToString)
                    End If
                Next
            Else
                strSql += vbCrLf + " ,DAMTVSSIGH1='" & DAmtVSSIGH & "'"
                totDAmtVSSIGH += Val(DAmtVSSIGH.ToString)
            End If

            If Not DAmtVVSDEFAr Is Nothing Then
                For k As Integer = 0 To IIf(DAmtVVSDEFAr.Length > 4, 4, DAmtVVSDEFAr.Length - 1)
                    If k = 0 Then
                        strSql += vbCrLf + " ,DAMTVVSDEF1='" & DAmtVVSDEFAr(0).ToString & "'"
                        totDAmtVVSDEF += Val(DAmtVVSDEFAr(0).ToString)
                    Else
                        strSql += vbCrLf + " ,DAMTVVSDEF" & (k + 1).ToString & "='" & DAmtVVSDEFAr(k).ToString & "'"
                        totDAmtVVSDEF += Val(DAmtVVSDEFAr(k).ToString)
                    End If
                Next
            Else
                strSql += vbCrLf + " ,DAMTVVSDEF1='" & DAmtVVSDEF & "'"
                totDAmtVVSDEF += Val(DAmtVVSDEF.ToString)
            End If

            If Not DAmtVVSEFAr Is Nothing Then
                For k As Integer = 0 To IIf(DAmtVVSEFAr.Length > 4, 4, DAmtVVSEFAr.Length - 1)
                    If k = 0 Then
                        strSql += vbCrLf + " ,DAMTVVSEF1='" & DAmtVVSEFAr(0).ToString & "'"
                        totDAmtVVSEF += Val(DAmtVVSEFAr(0).ToString)
                    Else
                        strSql += vbCrLf + " ,DAMTVVSEF" & (k + 1).ToString & "='" & DAmtVVSEFAr(k).ToString & "'"
                        totDAmtVVSEF += Val(DAmtVVSEFAr(k).ToString)
                    End If
                Next
            Else
                strSql += vbCrLf + " ,DAMTVVSEF1='" & DAmtVVSEF & "'"
                totDAmtVVSEF += Val(DAmtVVSEF.ToString)
            End If

            If Not DpcsAr Is Nothing Then
                For k As Integer = 0 To IIf(DpcsAr.Length > 4, 4, DpcsAr.Length - 1)
                    If k = 0 Then
                        strSql += vbCrLf + " ,DPCS1='" & DpcsAr(0).ToString & "'"
                        totDiaPcs += Val(DpcsAr(0).ToString)
                    Else
                        strSql += vbCrLf + " ,DPCS" & (k + 1).ToString & "='" & DpcsAr(k).ToString & "'"
                        totDiaPcs += Val(DpcsAr(k).ToString)
                    End If
                Next
            Else
                strSql += vbCrLf + " ,DPCS1='" & Dpcs & "'"
                totDiaPcs += Val(Dpcs.ToString)
            End If

            If Not DwtAr Is Nothing Then
                For k As Integer = 0 To IIf(DwtAr.Length > 4, 4, DwtAr.Length - 1)
                    If k = 0 Then
                        strSql += vbCrLf + " ,DWT1='" & DwtAr(0).ToString & "'"
                        totDiaWt += Val(DwtAr(0).ToString)
                    Else
                        strSql += vbCrLf + " ,DWT" & (k + 1).ToString & "='" & DwtAr(k).ToString & "'"
                        totDiaWt += Val(DwtAr(k).ToString)
                    End If
                Next
            Else
                strSql += vbCrLf + " ,DWT1='" & Dwt & "'"
                totDiaWt += Val(Dwt.ToString)
            End If

            If Not DshapeAr Is Nothing Then
                For k As Integer = 0 To IIf(DshapeAr.Length > 4, 4, DshapeAr.Length - 1)
                    If k = 0 Then
                        strSql += vbCrLf + " ,DSHAPE1='" & DshapeAr(0).ToString & "'"
                    Else
                        strSql += vbCrLf + " ,DSHAPE" & (k + 1).ToString & "='" & DshapeAr(k).ToString & "'"
                    End If
                Next
            Else
                strSql += vbCrLf + " ,DSHAPE1='" & Dshape & "'"
            End If

            If Not DClarityAr Is Nothing Then
                For k As Integer = 0 To IIf(DClarityAr.Length > 4, 4, DClarityAr.Length - 1)
                    If k = 0 Then
                        strSql += vbCrLf + " ,DCLARITY1='" & DClarityAr(0).ToString & "'"
                    Else
                        strSql += vbCrLf + " ,DCLARITY" & (k + 1).ToString & "='" & DClarityAr(k).ToString & "'"
                    End If
                Next
            Else
                strSql += vbCrLf + " ,DCLARITY1='" & DClarity & "'"
            End If

            If Not DColorAr Is Nothing Then
                For k As Integer = 0 To IIf(DColorAr.Length > 4, 4, DColorAr.Length - 1)
                    If k = 0 Then
                        strSql += vbCrLf + " ,DCOLOR1='" & DColorAr(0).ToString & "'"
                    Else
                        strSql += vbCrLf + " ,DCOLOR" & (k + 1).ToString & "='" & DColorAr(k).ToString & "'"
                    End If
                Next
            Else
                strSql += vbCrLf + " ,DCOLOR1='" & DColor & "'"
            End If

            If Not DianameAr Is Nothing Then
                For k As Integer = 0 To IIf(DianameAr.Length > 4, 4, DianameAr.Length - 1)
                    If k = 0 Then
                        strSql += vbCrLf + " ,DNAME1='" & DianameAr(0).ToString & "'"
                    Else
                        strSql += vbCrLf + " ,DNAME" & (k + 1).ToString & "='" & DianameAr(k).ToString & "'"
                    End If
                Next
            Else
                strSql += vbCrLf + " ,DNAME1='" & Dianame & "'"
            End If

            If Not StnnameAr Is Nothing Then
                For k As Integer = 0 To IIf(StnnameAr.Length > 4, 4, StnnameAr.Length - 1)
                    If k = 0 Then
                        strSql += vbCrLf + " ,SNAME1='" & StnnameAr(0).ToString & "'"
                    Else
                        strSql += vbCrLf + " ,SNAME" & (k + 1).ToString & "='" & StnnameAr(k).ToString & "'"
                    End If
                Next
            Else
                strSql += vbCrLf + " ,SNAME1='" & Stnname & "'"
            End If

            If Not StnnShapeAr Is Nothing Then
                For k As Integer = 0 To IIf(StnnShapeAr.Length > 4, 4, StnnShapeAr.Length - 1)
                    If k = 0 Then
                        strSql += vbCrLf + " ,SSHAPE1='" & StnnShapeAr(0).ToString & "'"
                    Else
                        strSql += vbCrLf + " ,SSHAPE" & (k + 1).ToString & "='" & StnnShapeAr(k).ToString & "'"
                    End If
                Next
            Else
                strSql += vbCrLf + " ,SSHAPE='" & StnnShape & "'"
            End If

            If Not SamtTotAr Is Nothing Then
                For k As Integer = 0 To IIf(SamtTotAr.Length > 4, 4, SamtTotAr.Length - 1)
                    If k = 0 Then
                        strSql += vbCrLf + " ,SAMT1='" & SamtTotAr(0).ToString & "'"
                    Else
                        strSql += vbCrLf + " ,SAMT" & (k + 1).ToString & "='" & SamtTotAr(k).ToString & "'"
                    End If
                Next
            Else
                strSql += vbCrLf + " ,SAMT1='" & SamtTot & "'"
            End If

            strSql += vbCrLf + " ,TOTDPCS='" & totDiaPcs & "'"
            strSql += vbCrLf + " ,TOTDWT='" & totDiaWt & "'"
            If Not SpcsAr Is Nothing Then
                For k As Integer = 0 To IIf(SpcsAr.Length > 4, 4, SpcsAr.Length - 1)
                    If k = 0 Then
                        strSql += vbCrLf + " ,SPCS1='" & SpcsAr(0).ToString & "'"
                        totStnPcs += Val(SpcsAr(0).ToString)
                    Else
                        strSql += vbCrLf + " ,SPCS" & (k + 1).ToString & "='" & SpcsAr(k).ToString & "'"
                        totStnPcs += Val(SpcsAr(k).ToString)
                    End If
                Next
            Else
                strSql += vbCrLf + " ,SPCS1='" & Spcs & "'"
                totStnPcs += Val(Spcs.ToString)
            End If
            If Not SwtAr Is Nothing Then
                For k As Integer = 0 To IIf(SwtAr.Length > 4, 4, SwtAr.Length - 1)
                    If k = 0 Then
                        strSql += vbCrLf + " ,SWT1='" & SwtAr(0).ToString & "'"
                        totStnWt += Val(SwtAr(0).ToString)
                    Else
                        strSql += vbCrLf + " ,SWT" & (k + 1).ToString & "='" & SwtAr(k).ToString & "'"
                        totStnWt += Val(SwtAr(k).ToString)
                    End If
                Next
            Else
                strSql += vbCrLf + " ,SWT1='" & Swt & "'"
                totStnWt += Val(Swt.ToString)
            End If
            strSql += vbCrLf + " ,TOTDAMTSIGH= CONVERT(VARCHAR,CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),SALVALUE)+CONVERT(NUMERIC(15,2),MAXMC)+ " & Val(Samt.ToString) + Val(totDAmtSIGH.ToString) & ")*103/100)) "
            strSql += vbCrLf + " ,TOTDAMTSIIJ= CONVERT(VARCHAR,CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),SALVALUE)+CONVERT(NUMERIC(15,2),MAXMC)+ " & Val(Samt.ToString) + Val(totDAmtSIIJ.ToString) & ")*103/100)) "
            strSql += vbCrLf + " ,TOTDAMTVSGH= CONVERT(VARCHAR,CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),SALVALUE)+CONVERT(NUMERIC(15,2),MAXMC)+ " & Val(Samt.ToString) + Val(totDAmtVSGH.ToString) & ")*103/100)) "
            strSql += vbCrLf + " ,TOTDAMTVVSEF= CONVERT(VARCHAR,CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),SALVALUE)+CONVERT(NUMERIC(15,2),MAXMC)+ " & Val(Samt.ToString) + Val(totDAmtVVSEF.ToString) & ")*103/100)) "
            strSql += vbCrLf + " ,TOTDAMTVVSVSGH= CONVERT(VARCHAR,CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),SALVALUE)+CONVERT(NUMERIC(15,2),MAXMC)+ " & Val(Samt.ToString) + Val(totDAmtVVSVSGH.ToString) & ")*103/100)) "
            strSql += vbCrLf + " ,TOTDAMTVSSIGH= CONVERT(VARCHAR,CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),SALVALUE)+CONVERT(NUMERIC(15,2),MAXMC)+ " & Val(Samt.ToString) + Val(totDAmtVSSIGH.ToString) & ")*103/100)) "
            strSql += vbCrLf + " ,TOTDAMTVVSDEF= CONVERT(VARCHAR,CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),SALVALUE)+CONVERT(NUMERIC(15,2),MAXMC)+ " & Val(Samt.ToString) + Val(totDAmtVVSDEF.ToString) & ")*103/100)) "

            strSql += vbCrLf + " ,TOTDVATSIGH= CONVERT(VARCHAR,CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),SALVALUE)+CONVERT(NUMERIC(15,2),MAXMC)+ " & Val(Samt.ToString) + Val(totDAmtSIGH.ToString) & ")*3/100)) "
            strSql += vbCrLf + " ,TOTDVATSIIJ= CONVERT(VARCHAR,CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),SALVALUE)+CONVERT(NUMERIC(15,2),MAXMC)+ " & Val(Samt.ToString) + Val(totDAmtSIIJ.ToString) & ")*3/100)) "
            strSql += vbCrLf + " ,TOTDVATVSGH= CONVERT(VARCHAR,CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),SALVALUE)+CONVERT(NUMERIC(15,2),MAXMC)+ " & Val(Samt.ToString) + Val(totDAmtVSGH.ToString) & ")*3/100)) "
            strSql += vbCrLf + " ,TOTDVATVVSEF= CONVERT(VARCHAR,CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),SALVALUE)+CONVERT(NUMERIC(15,2),MAXMC)+ " & Val(Samt.ToString) + Val(totDAmtVVSEF.ToString) & ")*3/100)) "

            strSql += vbCrLf + " ,TOTDVATVVSVSGH= CONVERT(VARCHAR,CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),SALVALUE)+CONVERT(NUMERIC(15,2),MAXMC)+ " & Val(Samt.ToString) + Val(totDAmtVVSVSGH.ToString) & ")*3/100)) "
            strSql += vbCrLf + " ,TOTDVATVSSIGH= CONVERT(VARCHAR,CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),SALVALUE)+CONVERT(NUMERIC(15,2),MAXMC)+ " & Val(Samt.ToString) + Val(totDAmtVSSIGH.ToString) & ")*3/100)) "
            strSql += vbCrLf + " ,TOTDVATVVSDEF= CONVERT(VARCHAR,CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),CONVERT(NUMERIC(15,2),SALVALUE)+CONVERT(NUMERIC(15,2),MAXMC)+ " & Val(Samt.ToString) + Val(totDAmtVVSDEF.ToString) & ")*3/100)) "

            strSql += vbCrLf + " ,TOTSTNPCS='" & totStnPcs & "'"
            strSql += vbCrLf + " ,TOTSTNWT='" & totStnWt & "'"
            strSql += vbCrLf + " WHERE KEYNO='" & dtDet.Rows(v).Item("KEYNO").ToString & "'"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        Next
        strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW ORDER BY KEYNO"
        dtDet = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtDet)
        For i As Integer = 0 To dtDet.Rows.Count - 1
            Dim Gamt As Decimal
            Dim Mc As Decimal
            Dim Diaamt As Decimal
            Dim Stnamt As Decimal
            Dim Vat As Decimal
            Dim Total As Decimal
            Dim vatper As Decimal = Val(objGPack.GetSqlValue("SELECT SALESTAX FROM " & cnAdminDb & "..CATEGORY WHERE ITEMID IN(SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME='" & dtDet.Rows(i).Item("ITEMNAME").ToString & "' )", , , ))
            If vatper = 0 Then vatper = 1
            strSql = "SELECT DISTINCT UNIQUECODE,MAX(COLORID)COLORID,MAX(CLARITYID)CLARITYID FROM " & cnAdminDb & "..DIASTYLE GROUP BY UNIQUECODE "
            Dim DtDiastyle As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(DtDiastyle)
            strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & Sysid & "DIAMOND WHERE TAGSNO='" & dtDet.Rows(i).Item("SNO").ToString & "' AND TAGNO='" & dtDet.Rows(i).Item("TAGNO").ToString & "'"
            Dim dtDia1 As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtDia1)
            Dim dtDia As New DataTable
            dtDia = dtDia1.DefaultView.ToTable(True, "SHAPENAME", "STNPCS", "STNWT", "STNRATE", "ITEMNAME", "SUBITEMNAME")
            Dim Dtdiashape As New DataTable
            Dtdiashape = dtDia.DefaultView.ToTable(True, "SHAPENAME", "STNRATE", "ITEMNAME", "SUBITEMNAME")
            Stnamt = Math.Round(Val(dtDet.Rows(i).Item("STNAMT").ToString))
            Gamt = Math.Round(Val(dtDet.Rows(i).Item("SALVALUE").ToString))
            Mc = Math.Round(Val(dtDet.Rows(i).Item("MAXMC").ToString))
            If dtDia1.Rows.Count > 0 And DtDiastyle.Rows.Count > 0 And Dtdiashape.Rows.Count > 0 Then
                For v As Integer = 0 To DtDiastyle.Rows.Count - 1
                    Diaamt = 0
                    For m As Integer = 0 To Dtdiashape.Rows.Count - 1
                        'Dim shapename As String = objGPack.GetSqlValue("SELECT SHAPENAME FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPEID='" & Val(DtDiastyle.Rows(v).Item("SHAPEID").ToString) & "'", , , )
                        Dim shapeid As Integer = Val(objGPack.GetSqlValue("SELECT SHAPEID FROM " & cnAdminDb & "..STNSHAPE WHERE SHAPENAME='" & Dtdiashape.Rows(m).Item("SHAPENAME").ToString & "'", , , ).ToString)
                        'If shapename = Dtdiashape.Rows(m).Item("SHAPENAME").ToString Then
                        Dim Diawt As Decimal = 0
                        Dim Diapcs As Integer = 0
                        'Diawt = Val(dtDia.Compute("SUM(WEIGHT)", "SHAPE='" & Dtdiashape.Rows(m).Item("SHAPE").ToString & "' AND ITEM='" & Dtdiashape.Rows(m).Item("ITEM").ToString & "' AND SUBITEM='" & Dtdiashape.Rows(m).Item("SUBITEM").ToString & "' AND RATE='" & Dtdiashape.Rows(m).Item("RATE").ToString & "'").ToString)
                        Diawt = Val(dtDia.Compute("SUM(STNWT)", "SHAPENAME='" & Dtdiashape.Rows(m).Item("SHAPENAME").ToString & "' AND ITEMNAME='" & Dtdiashape.Rows(m).Item("ITEMNAME").ToString & "' AND SUBITEMNAME='" & Dtdiashape.Rows(m).Item("SUBITEMNAME").ToString & "' AND STNRATE='" & Dtdiashape.Rows(m).Item("STNRATE").ToString & "'").ToString)
                        Diapcs = Val(dtDia.Compute("SUM(STNPCS)", "SHAPENAME='" & Dtdiashape.Rows(m).Item("SHAPENAME").ToString & "' AND ITEMNAME='" & Dtdiashape.Rows(m).Item("ITEMNAME").ToString & "' AND SUBITEMNAME='" & Dtdiashape.Rows(m).Item("SUBITEMNAME").ToString & "' AND STNRATE='" & Dtdiashape.Rows(m).Item("STNRATE").ToString & "'").ToString)
                        Dim cent As Double = 0
                        If Diapcs = 0 Then Diapcs = 1
                        strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ITEMNAME,'') ='" & Dtdiashape.Rows(m).Item("ITEMNAME").ToString & "'"
                        Dim mCaltype As String = objGPack.GetSqlValue(strSql, "CALTYPE", "")
                        strSql = " SELECT ISNULL(CALTYPE,'') CALTYPE FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(SUBITEMNAME,'') ='" & Dtdiashape.Rows(m).Item("SUBITEMNAME").ToString & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & Dtdiashape.Rows(m).Item("ITEMNAME").ToString & "')"
                        mCaltype = objGPack.GetSqlValue(strSql, "CALTYPE", "")
                        If mCaltype = "D" Then
                            cent = Diawt
                        Else
                            cent = Diawt / Diapcs
                        End If
                        '                        cent = Diawt / Diapcs
                        cent *= 100
                        strSql = " DECLARE @CENT FLOAT"
                        strSql += vbCrLf + " SET @CENT = " & cent & ""
                        strSql += vbCrLf + " SELECT MAXRATE FROM " & cnAdminDb & "..CENTRATE WHERE"
                        strSql += vbCrLf + " @CENT BETWEEN FROMCENT AND TOCENT "
                        strSql += vbCrLf + " AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & Dtdiashape.Rows(m).Item("ITEMNAME").ToString & "')"
                        strSql += vbCrLf + " AND SUBITEMID = ISNULL((SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME = '" & Dtdiashape.Rows(m).Item("SUBITEMNAME").ToString & "' AND ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & Dtdiashape.Rows(m).Item("ITEMNAME").ToString & "')),0)"
                        strSql += vbCrLf + " AND COLORID=" & Val(DtDiastyle.Rows(v).Item("COLORID").ToString)
                        strSql += vbCrLf + " AND CLARITYID=" & Val(DtDiastyle.Rows(v).Item("CLARITYID").ToString)
                        strSql += vbCrLf + " AND SHAPEID=" & shapeid
                        Dim rate As Double = Val(objGPack.GetSqlValue(strSql, "MAXRATE", "", tran))
                        If rate <> 0 Then
                            Diaamt = Diaamt + Math.Round(Diawt * rate, 2)
                        Else
                            Diaamt = Diaamt + Math.Round(Diawt * Val(Dtdiashape.Rows(m).Item("STNRATE").ToString), 2)
                        End If
                        'End If
                    Next
                    Diaamt = Math.Round(Diaamt)
                    Vat = Math.Round(((Gamt + Mc + Stnamt + Diaamt) / 100) * vatper)
                    Total = Gamt + Mc + Stnamt + Diaamt + Vat
                    If dtDet.Columns.Contains("D-" & DtDiastyle.Rows(v).Item("UNIQUECODE").ToString & "") Then
                        strSql = " UPDATE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW"
                        strSql += vbCrLf + " SET [D-" & DtDiastyle.Rows(v).Item("UNIQUECODE").ToString & "]='" & Diaamt & "'"
                        strSql += vbCrLf + " WHERE KEYNO='" & dtDet.Rows(i).Item("KEYNO").ToString & "'"
                        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                        cmd.ExecuteNonQuery()
                    End If
                    If dtDet.Columns.Contains("VAT-" & DtDiastyle.Rows(v).Item("UNIQUECODE").ToString & "") Then
                        strSql = " UPDATE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW"
                        strSql += vbCrLf + " SET [VAT-" & DtDiastyle.Rows(v).Item("UNIQUECODE").ToString & "]='" & Vat & "'"
                        strSql += vbCrLf + " WHERE KEYNO='" & dtDet.Rows(i).Item("KEYNO").ToString & "'"
                        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                        cmd.ExecuteNonQuery()
                    End If
                    If dtDet.Columns.Contains("TOT-" & DtDiastyle.Rows(v).Item("UNIQUECODE").ToString & "") Then
                        strSql = " UPDATE TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW"
                        strSql += vbCrLf + " SET [TOT-" & DtDiastyle.Rows(v).Item("UNIQUECODE").ToString & "]='" & Total & "'"
                        strSql += vbCrLf + " WHERE KEYNO='" & dtDet.Rows(i).Item("KEYNO").ToString & "'"
                        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                        cmd.ExecuteNonQuery()
                    End If
                Next
            End If
        Next

        strSql = vbCrLf + "  SELECT * FROM TEMPTABLEDB..TEMP" & Sysid & "TAGSTOCKVIEW"
        strSql += vbCrLf + "  ORDER BY KEYNO"
        Dim dtGrid As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Function
        End If
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        objGridShower.gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "WEB TAG STOCK REPORT"
        Dim tit As String = Nothing
        tit += "WEB TAG STOCK REPORT"
        If txtItemName.Text <> "" Then tit += " OF " & txtItemName.Text + "'s "
        tit += " AS ON " & dtpAsOnDate.Text
        objGridShower.lblTitle.Text = tit & IIf(cmbCostCenter.Text <> "" And cmbCostCenter.Text <> "ALL", " :" & cmbCostCenter.Text, "")
        AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
        AddHandler objGridShower.gridView.KeyPress, AddressOf GridView_KeyPress
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.gridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        objGridShower.gridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft
        objGridShower.gridView.DataSource = dtGrid
        objGridShower.gridView.Rows(objGridShower.gridView.RowCount - 1).DefaultCellStyle = reportTotalStyle
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        objGridShower.pnlFooter.Visible = False
        For j As Integer = 0 To objGridShower.gridView.ColumnCount - 1
            If objGridShower.gridView.Columns(j).Name.Contains("D-") Then objGridShower.gridView.Columns(j).HeaderText = objGridShower.gridView.Columns(j).Name.Replace("D-", "Dia Amount For ")
            If objGridShower.gridView.Columns(j).Name.Contains("VAT-") Then objGridShower.gridView.Columns(j).HeaderText = objGridShower.gridView.Columns(j).Name.Replace("VAT-", "VAT For ")
            If objGridShower.gridView.Columns(j).Name.Contains("TOT-") Then objGridShower.gridView.Columns(j).HeaderText = objGridShower.gridView.Columns(j).Name.Replace("TOT-", "Total Value of Product with ")
        Next
        objGridShower.gridView.Columns("SNO").Visible = False
        objGridShower.gridView.Columns("KEYNO").HeaderText = "SNO"
        objGridShower.gridView.Columns("ITEMNAME").HeaderText = "Product"
        objGridShower.gridView.Columns("TAGNO").HeaderText = "SKU"
        objGridShower.gridView.Columns("DESCRIP").HeaderText = "Name of Product"
        objGridShower.gridView.Columns("SUBITEMNAME").HeaderText = "Type of Product"
        objGridShower.gridView.Columns("CATEGORY").HeaderText = "Category"
        objGridShower.gridView.Columns("COLLECTION").HeaderText = "Collection"
        objGridShower.gridView.Columns("NARRATION").HeaderText = "Description"
        objGridShower.gridView.Columns("MPURITY").HeaderText = "Metal Type"
        objGridShower.gridView.Columns("GRSWT").HeaderText = "Grossweight"
        objGridShower.gridView.Columns("NETWT").HeaderText = "Metal Weight"
        objGridShower.gridView.Columns("SALVALUE").HeaderText = "Metal Price"
        objGridShower.gridView.Columns("MAXMC").HeaderText = "Making Charges"
        objGridShower.gridView.Columns("TOTDPCS").HeaderText = "Total No. of Diamonds"
        objGridShower.gridView.Columns("TOTDWT").HeaderText = "Total Diamond Weight"
        objGridShower.gridView.Columns("TOTSTNPCS").HeaderText = "Total No. of Gemstones"
        objGridShower.gridView.Columns("TOTSTNWT").HeaderText = "Total Gemstone Weight"
        objGridShower.gridView.Columns("STNAMT").HeaderText = "Total Gemstone Amount"

        For K As Integer = 1 To 5
            objGridShower.gridView.Columns("DPCS" & K.ToString).HeaderText = "No. of Diamonds " & K.ToString
            objGridShower.gridView.Columns("DWT" & K.ToString).HeaderText = "Diamond Weight " & K.ToString
            objGridShower.gridView.Columns("DSHAPE" & K.ToString).HeaderText = "Diamond shape " & K.ToString
            objGridShower.gridView.Columns("DCOLOR" & K.ToString).HeaderText = "Diamond color " & K.ToString
            objGridShower.gridView.Columns("DCLARITY" & K.ToString).HeaderText = "Diamond clarity " & K.ToString
            objGridShower.gridView.Columns("DNAME" & K.ToString).HeaderText = "Diamond Name " & K.ToString
        Next

        For K As Integer = 1 To 5
            objGridShower.gridView.Columns("SPCS" & K.ToString).HeaderText = "No. of Gemstones " & K.ToString
            objGridShower.gridView.Columns("SWT" & K.ToString).HeaderText = "Gemstone Weight " & K.ToString
            objGridShower.gridView.Columns("SNAME" & K.ToString).HeaderText = "Gem Stone Type " & K.ToString
            objGridShower.gridView.Columns("SSHAPE" & K.ToString).HeaderText = "Gem shape shape " & K.ToString
        Next

        For K As Integer = 1 To 5
            objGridShower.gridView.Columns("DAMTSIGH" & K.ToString).HeaderText = "Dia Amount For SI GH " & K.ToString
            objGridShower.gridView.Columns("DAMTSIIJ" & K.ToString).HeaderText = "Dia Amount For SI IJ " & K.ToString
            objGridShower.gridView.Columns("DAMTVSGH" & K.ToString).HeaderText = "Dia Amount For VS GH " & K.ToString
            objGridShower.gridView.Columns("DAMTVVSEF" & K.ToString).HeaderText = "Dia Amount For VVS EF " & K.ToString

            objGridShower.gridView.Columns("DAMTVVSVSGH" & K.ToString).HeaderText = "Dia Amount For VVS VS GH " & K.ToString
            objGridShower.gridView.Columns("DAMTVSSIGH" & K.ToString).HeaderText = "Dia Amount For VS SI GH " & K.ToString
            objGridShower.gridView.Columns("DAMTVVSDEF" & K.ToString).HeaderText = "Dia Amount For VVS DEF " & K.ToString
            objGridShower.gridView.Columns("SAMT" & K.ToString).HeaderText = "Gemstone Amount " & K.ToString
        Next

        objGridShower.gridView.Columns("TOTDAMTSIGH").HeaderText = "Total Value of Product with SI GH"
        objGridShower.gridView.Columns("TOTDAMTSIIJ").HeaderText = "Total Value of Product with SI IJ"
        objGridShower.gridView.Columns("TOTDAMTVSGH").HeaderText = "Total Value of Product with VS GH"
        objGridShower.gridView.Columns("TOTDAMTVVSEF").HeaderText = "Total Value of Product with VVS EF"

        objGridShower.gridView.Columns("TOTDAMTVVSVSGH").HeaderText = "Total Value of Product with VVS VS GH"
        objGridShower.gridView.Columns("TOTDAMTVSSIGH").HeaderText = "Total Value of Product with VS SI GH"
        objGridShower.gridView.Columns("TOTDAMTVVSDEF").HeaderText = "Total Value of Product with VVS DEF"

        objGridShower.gridView.Columns("TOTDVATSIGH").HeaderText = "GST For SI GH"
        objGridShower.gridView.Columns("TOTDVATSIIJ").HeaderText = "GST For SI IJ"
        objGridShower.gridView.Columns("TOTDVATVSGH").HeaderText = "GST For VS GH"
        objGridShower.gridView.Columns("TOTDVATVVSEF").HeaderText = "GST For VVS EF"

        objGridShower.gridView.Columns("TOTDVATVVSVSGH").HeaderText = "GST For VVS VS GH"
        objGridShower.gridView.Columns("TOTDVATVSSIGH").HeaderText = "GST For VS SI GH"
        objGridShower.gridView.Columns("TOTDVATVVSDEF").HeaderText = "GST For VVS DEF"

        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.gridViewHeader.Visible = False
        'objGridShower.btnre
        objGridShower.AutoResize()
        Prop_Sets()
        GrdAlign()
    End Function

    Private Sub GrdAlign()
        Dim k As Integer = 1

        If objGridShower.gridView.Columns.Contains("SNO") Then objGridShower.gridView.Columns("SNO").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("DESCRIP") Then objGridShower.gridView.Columns("DESCRIP").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("TAGNO") Then objGridShower.gridView.Columns("TAGNO").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("ITEMNAME") Then objGridShower.gridView.Columns("ITEMNAME").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("SUBITEMNAME") Then objGridShower.gridView.Columns("SUBITEMNAME").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("CATEGORY") Then objGridShower.gridView.Columns("CATEGORY").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("COLLECTION") Then objGridShower.gridView.Columns("COLLECTION").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("NARRATION") Then objGridShower.gridView.Columns("NARRATION").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("HEIGHT") Then objGridShower.gridView.Columns("HEIGHT").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("WIDTH") Then objGridShower.gridView.Columns("WIDTH").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("MPURITY") Then objGridShower.gridView.Columns("MPURITY").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("GRSWT") Then objGridShower.gridView.Columns("GRSWT").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("NETWT") Then objGridShower.gridView.Columns("NETWT").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("SALVALUE") Then objGridShower.gridView.Columns("SALVALUE").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("MAXMC") Then objGridShower.gridView.Columns("MAXMC").DisplayIndex = k : k = k + 1

        For i As Integer = 1 To 5
            If objGridShower.gridView.Columns.Contains("DNAME" + i.ToString()) Then objGridShower.gridView.Columns("DNAME" + i.ToString()).DisplayIndex = k : k = k + 1
            If objGridShower.gridView.Columns.Contains("DSHAPE" + i.ToString()) Then objGridShower.gridView.Columns("DSHAPE" + i.ToString()).DisplayIndex = k : k = k + 1
            If objGridShower.gridView.Columns.Contains("DCLARITY" + i.ToString()) Then objGridShower.gridView.Columns("DCLARITY" + i.ToString()).DisplayIndex = k : k = k + 1
            If objGridShower.gridView.Columns.Contains("DCOLOR" + i.ToString()) Then objGridShower.gridView.Columns("DCOLOR" + i.ToString()).DisplayIndex = k : k = k + 1
            If objGridShower.gridView.Columns.Contains("DPCS" + i.ToString()) Then objGridShower.gridView.Columns("DPCS" + i.ToString()).DisplayIndex = k : k = k + 1
            If objGridShower.gridView.Columns.Contains("DWT" + i.ToString()) Then objGridShower.gridView.Columns("DWT" + i.ToString()).DisplayIndex = k : k = k + 1
            If objGridShower.gridView.Columns.Contains("DAMTSIIJ" + i.ToString()) Then objGridShower.gridView.Columns("DAMTSIIJ" + i.ToString()).DisplayIndex = k : k = k + 1
            If objGridShower.gridView.Columns.Contains("DAMTSIGH" + i.ToString()) Then objGridShower.gridView.Columns("DAMTSIGH" + i.ToString()).DisplayIndex = k : k = k + 1
            If objGridShower.gridView.Columns.Contains("DAMTVSSIGH" + i.ToString()) Then objGridShower.gridView.Columns("DAMTVSSIGH" + i.ToString()).DisplayIndex = k : k = k + 1
            If objGridShower.gridView.Columns.Contains("DAMTVSGH" + i.ToString()) Then objGridShower.gridView.Columns("DAMTVSGH" + i.ToString()).DisplayIndex = k : k = k + 1
            If objGridShower.gridView.Columns.Contains("DAMTVVSVSGH" + i.ToString()) Then objGridShower.gridView.Columns("DAMTVVSVSGH" + i.ToString()).DisplayIndex = k : k = k + 1
            If objGridShower.gridView.Columns.Contains("DAMTVVSEF" + i.ToString()) Then objGridShower.gridView.Columns("DAMTVVSEF" + i.ToString()).DisplayIndex = k : k = k + 1
            If objGridShower.gridView.Columns.Contains("DAMTVVSDEF" + i.ToString()) Then objGridShower.gridView.Columns("DAMTVVSDEF" + i.ToString()).DisplayIndex = k : k = k + 1
        Next
        For i As Integer = 1 To 5
            If objGridShower.gridView.Columns.Contains("SNAME" + i.ToString()) Then objGridShower.gridView.Columns("SNAME" + i.ToString()).DisplayIndex = k : k = k + 1
            If objGridShower.gridView.Columns.Contains("SSHAPE" + i.ToString()) Then objGridShower.gridView.Columns("SSHAPE" + i.ToString()).DisplayIndex = k : k = k + 1
            If objGridShower.gridView.Columns.Contains("SPCS" + i.ToString()) Then objGridShower.gridView.Columns("SPCS" + i.ToString()).DisplayIndex = k : k = k + 1
            If objGridShower.gridView.Columns.Contains("SWT" + i.ToString()) Then objGridShower.gridView.Columns("SWT" + i.ToString()).DisplayIndex = k : k = k + 1
            If objGridShower.gridView.Columns.Contains("SAMT" + i.ToString()) Then objGridShower.gridView.Columns("SAMT" + i.ToString()).DisplayIndex = k : k = k + 1
        Next

        ''re:
        ''        For i As Integer = 0 To objGridShower.gridView.Columns.Count - 1
        ''            If objGridShower.gridView.Columns(i).HeaderText = "Total Value of Product with SI GH" Then
        ''                objGridShower.gridView.Columns(i).Name = objGridShower.gridView.Columns(i).Name
        ''                GoTo re
        ''            End If
        ''        Next
        If objGridShower.gridView.Columns.Contains("TOTDPCS") Then objGridShower.gridView.Columns("TOTDPCS").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("TOTDWT") Then objGridShower.gridView.Columns("TOTDWT").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("D-SI IJ") Then objGridShower.gridView.Columns("D-SI IJ").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("D-SI GH") Then objGridShower.gridView.Columns("D-SI GH").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("D-VS SI GH") Then objGridShower.gridView.Columns("D-VS SI GH").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("D-VS GH") Then objGridShower.gridView.Columns("D-VS GH").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("D-VVS VS GH") Then objGridShower.gridView.Columns("D-VVS VS GH").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("D-VVS EF") Then objGridShower.gridView.Columns("D-VVS EF").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("D-VVS DEF") Then objGridShower.gridView.Columns("D-VVS DEF").DisplayIndex = k : k = k + 1

        If objGridShower.gridView.Columns.Contains("TOTSTNPCS") Then objGridShower.gridView.Columns("TOTSTNPCS").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("TOTSTNPCS") Then objGridShower.gridView.Columns("TOTSTNPCS").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("TOTSTNWT") Then objGridShower.gridView.Columns("TOTSTNWT").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("STNAMT") Then objGridShower.gridView.Columns("STNAMT").DisplayIndex = k : k = k + 1

        If objGridShower.gridView.Columns.Contains("TOTDVATSIIJ") Then objGridShower.gridView.Columns("TOTDVATSIIJ").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("TOTDAMTSIIJ") Then objGridShower.gridView.Columns("TOTDAMTSIIJ").DisplayIndex = k : k = k + 1

        If objGridShower.gridView.Columns.Contains("TOTDVATSIGH") Then objGridShower.gridView.Columns("TOTDVATSIGH").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("TOTDAMTSIGH") Then objGridShower.gridView.Columns("TOTDAMTSIGH").DisplayIndex = k : k = k + 1

        If objGridShower.gridView.Columns.Contains("TOTDVATVSSIGH") Then objGridShower.gridView.Columns("TOTDVATVSSIGH").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("TOTDAMTVSSIGH") Then objGridShower.gridView.Columns("TOTDAMTVSSIGH").DisplayIndex = k : k = k + 1

        If objGridShower.gridView.Columns.Contains("TOTDVATVSGH") Then objGridShower.gridView.Columns("TOTDVATVSGH").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("TOTDAMTVSGH") Then objGridShower.gridView.Columns("TOTDAMTVSGH").DisplayIndex = k : k = k + 1

        If objGridShower.gridView.Columns.Contains("TOTDVATVVSVSGH") Then objGridShower.gridView.Columns("TOTDVATVVSVSGH").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("TOTDAMTVVSVSGH") Then objGridShower.gridView.Columns("TOTDAMTVVSVSGH").DisplayIndex = k : k = k + 1

        If objGridShower.gridView.Columns.Contains("TOTDVATVVSEF") Then objGridShower.gridView.Columns("TOTDVATVVSEF").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("TOTDAMTVVSEF") Then objGridShower.gridView.Columns("TOTDAMTVVSEF").DisplayIndex = k : k = k + 1

        If objGridShower.gridView.Columns.Contains("TOTDVATVVSDEF") Then objGridShower.gridView.Columns("TOTDVATVVSDEF").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("TOTDAMTVVSDEF") Then objGridShower.gridView.Columns("TOTDAMTVVSDEF").DisplayIndex = k : k = k + 1

        If objGridShower.gridView.Columns.Contains("GENDER") Then objGridShower.gridView.Columns("GENDER").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("OCCASION") Then objGridShower.gridView.Columns("OCCASION").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("SIZE") Then objGridShower.gridView.Columns("SIZE").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("HEADER") Then objGridShower.gridView.Columns("HEADER").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("GOLD SHAPE") Then objGridShower.gridView.Columns("GOLD SHAPE").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("WORSHIP") Then objGridShower.gridView.Columns("WORSHIP").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("CHOOSE ZODIAC") Then objGridShower.gridView.Columns("CHOOSE ZODIAC").DisplayIndex = k : k = k + 1
        If objGridShower.gridView.Columns.Contains("CHOOSE ALPHABET") Then objGridShower.gridView.Columns("CHOOSE ALPHABET").DisplayIndex = k : k = k + 1
    End Sub

    Private Sub btnView_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView_Search.Click

        If Not CheckBckDays(userId, Me.Name, dtpAsOnDate.Value) Then dtpAsOnDate.Focus() : Exit Sub
        chkStudColumn.Checked = False
        WebtagView()
        Exit Sub


        If chkStudColumn.Checked = False Then
            strSql = vbCrLf + "  IF (SELECT TOP 1 1 FROM MASTER..SYSOBJECTS WHERE NAME = 'TEMPTAGSTOCKVIEW')>0 "
            strSql += vbCrLf + "  DROP TABLE MASTER..TEMPTAGSTOCKVIEW"
            strSql += vbCrLf + "  SELECT T.TAGNO,T.PCS,T.GRSWT,CONVERT(NUMERIC(15,3),NETWT)NETWT,CONVERT(INT,NULL)DIAPCS,CONVERT(NUMERIC(15,3),NULL)DIAWT,CONVERT(INT,NULL)STNPCS,CONVERT(NUMERIC(15,3),NULL)STNWT"
            strSql += vbCrLf + "  ,T.RATE,T.MAXMCGRM,T.MAXMC"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR(50),CASE WHEN t.descrip <> '' THEN t.descrip ELSE I.ITEMNAME END)PARTICULAR"
            strSql += vbCrLf + "  ,T.ITEMID" ',T.SUBITEMID,T.DESIGNERID"
            strSql += vbCrLf + "  ,CONVERT(INT,1)RESULT,CONVERT(VARCHAR(15),T.SNO)SNO,I.ITEMNAME,T.TAGVAL,CONVERT(VARCHAR(2),NULL)COLHEAD"
            strSql += vbCrLf + "  INTO MASTER..TEMPTAGSTOCKVIEW"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..WITEMTAG AS T"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID = T.ITEMID"
            '            strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS S ON S.ITEMID = T.ITEMID AND S.SUBITEMID = T.SUBITEMID"
            strSql += vbCrLf + " WHERE T.RECDATE <= '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "' "
            If Not cnCentStock Then strSql += " AND T.COMPANYID = '" & GetStockCompId() & "'"
            If txtItemCode_NUM.Text <> "" Then
                strSql += vbCrLf + "  AND T.ITEMID = '" & txtItemCode_NUM.Text & "'"
            End If

            If txtTagNo.Text <> "" Then
                strSql += vbCrLf + "  AND T.TAGNO = '" & txtTagNo.Text & "'"
            End If
            strSql += vbCrLf + "  AND (ISSDATE IS NULL OR ISSDATE > '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "')"

            If cmbItemType.Text <> "ALL" Then
                strSql += vbCrLf + "  AND ITEMTYPEID = (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType.Text & "')"
            End If
            If cmbCostCenter.Text <> "ALL" And cmbCostCenter.Text <> "" Then
                strSql += vbCrLf + "  AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter.Text & "')"
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
            If txtSearch_txt.Text <> "" Then
                strSql += vbCrLf + " AND T." & cmbSearchKey.Text & " LIKE"
                strSql += vbCrLf + " '" & txtSearch_txt.Text & "%'"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + "  /** Inserting Stone Details **/"
            strSql += vbCrLf + "  IF (SELECT COUNT(*) FROM MASTER..TEMPTAGSTOCKVIEW)>0"
            strSql += vbCrLf + "  	BEGIN"
            strSql += vbCrLf + "  	INSERT INTO MASTER..TEMPTAGSTOCKVIEW"
            strSql += vbCrLf + "  	("
            strSql += vbCrLf + "  	PARTICULAR,DIAPCS,DIAWT,STNPCS,STNWT,SNO,RESULT,TAGVAL,ITEMNAME"
            strSql += vbCrLf + "  	)"
            strSql += vbCrLf + "  	SELECT CASE WHEN ISNULL(S.SHORTNAME,'') = '' THEN I.SHORTNAME ELSE S.SHORTNAME END AS PARTICULAR"
            strSql += vbCrLf + "  	,CASE WHEN I.DIASTONE = 'D' THEN T.STNPCS ELSE 0 END AS DIAPCS"
            strSql += vbCrLf + "  	,CASE WHEN I.DIASTONE = 'D' THEN T.STNWT ELSE 0 END AS DIAWT"
            strSql += vbCrLf + "  	,CASE WHEN I.DIASTONE <>'D' THEN T.STNPCS ELSE 0 END AS STNPCS"
            strSql += vbCrLf + "  	,CASE WHEN I.DIASTONE <>'D' THEN T.STNWT ELSE 0 END AS STNWT"

            strSql += vbCrLf + "  	,T.TAGSNO AS SNO,2 RESULT,TT.TAGVAL,I.ITEMNAME"
            strSql += vbCrLf + "  	FROM " & cnAdminDb & "..WITEMTAGSTONE AS T"
            strSql += vbCrLf + "  	INNER JOIN MASTER..TEMPTAGSTOCKVIEW AS TT ON TT.SNO = T.TAGSNO"
            strSql += vbCrLf + "  	INNER JOIN " & cnAdminDb & "..ITEMMAST I ON I.ITEMID = T.STNITEMID"
            strSql += vbCrLf + "  	LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS S ON S.ITEMID = T.STNITEMID AND S.SUBITEMID = T.STNSUBITEMID"

            strSql += vbCrLf + "    INSERT INTO MASTER..TEMPTAGSTOCKVIEW"
            strSql += vbCrLf + "    ("
            strSql += vbCrLf + "    TAGNO,PCS,GRSWT,NETWT,DIAPCS,DIAWT,STNPCS,STNWT,COLHEAD,TAGVAL"
            strSql += vbCrLf + "    )"
            strSql += vbCrLf + "    SELECT 'TOTAL',SUM(ISNULL(PCS,0)),SUM(GRSWT),SUM(NETWT),SUM(ISNULL(DIAPCS,0)),SUM(ISNULL(DIAWT,0)),SUM(ISNULL(DIAPCS,0)),SUM(ISNULL(STNWT,0))"
            strSql += vbCrLf + "    ,'G' COLHEAD,99999999 TAGVAL"
            strSql += vbCrLf + "    FROM MASTER..TEMPTAGSTOCKVIEW"

            strSql += vbCrLf + "  	END"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " UPDATE MASTER..TEMPTAGSTOCKVIEW SET"
            strSql += " PARTICULAR = SUBSTRING(PARTICULAR,1,15)"

            strSql += " ,PCS = CASE WHEN PCS = 0 THEN NULL ELSE PCS END"
            strSql += " ,GRSWT = CASE WHEN GRSWT = 0 THEN NULL ELSE GRSWT END"
            strSql += " ,NETWT = CASE WHEN NETWT = 0 THEN NULL ELSE NETWT END"
            strSql += " ,DIAPCS = CASE WHEN DIAPCS = 0 THEN NULL ELSE DIAPCS END"
            strSql += " ,DIAWT = CASE WHEN DIAWT = 0 THEN NULL ELSE DIAWT END"
            strSql += " ,STNWT = CASE WHEN STNWT = 0 THEN NULL ELSE STNWT END"
            strSql += " ,RATE = CASE WHEN RATE = 0 THEN NULL ELSE RATE END"
            strSql += " ,MAXMCGRM= CASE WHEN MAXMCGRM = 0 THEN NULL ELSE MAXMCGRM END"
            strSql += " ,MAXMC = CASE WHEN MAXMC = 0 THEN NULL ELSE MAXMC END"

            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            count = 0
        Else

            strSql = vbCrLf + "  IF (SELECT TOP 1 1 FROM MASTER..SYSOBJECTS WHERE NAME = 'TEMPTAGSTOCKVIEW')>0 "
            strSql += vbCrLf + "  DROP TABLE MASTER..TEMPTAGSTOCKVIEW"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  T.ITEMID ID"
            strSql += vbCrLf + "  ,I.ITEMNAME ITEM"
            strSql += vbCrLf + "  ,IT.NAME [IT TYPE]"
            strSql += vbCrLf + "  ,PCS "
            strSql += vbCrLf + "  ,T.GRSWT "
            strSql += vbCrLf + "  ,T.SNO "
            strSql += vbCrLf + "  INTO MASTER..TEMPTAGSTOCKVIEW"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..WITEMTAG AS T"
            strSql += vbCrLf + "  INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID = T.ITEMID"
            strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..ITEMTYPE AS IT ON IT.ITEMTYPEID = T.ITEMTYPEID"
            strSql += vbCrLf + " WHERE T.RECDATE <= '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "' "
            If Not cnCentStock Then strSql += " AND T.COMPANYID = '" & GetStockCompId() & "'"
            If txtItemCode_NUM.Text <> "" Then
                strSql += vbCrLf + "  AND T.ITEMID = '" & txtItemCode_NUM.Text & "'"
            End If
            If txtTagNo.Text <> "" Then
                strSql += vbCrLf + "  AND T.TAGNO = '" & txtTagNo.Text & "'"
            End If
            strSql += vbCrLf + "  AND (ISSDATE IS NULL OR ISSDATE > '" & dtpAsOnDate.Value.Date.ToString("yyyy-MM-dd") & "')"
            If cmbItemType.Text <> "ALL" Then
                strSql += vbCrLf + "  AND ITEMTYPEID = (SELECT ITEMTYPEID FROM " & cnAdminDb & "..ITEMTYPE WHERE NAME = '" & cmbItemType.Text & "')"
            End If
            If cmbCostCenter.Text <> "ALL" And cmbCostCenter.Text <> "" Then
                strSql += vbCrLf + "  AND COSTID = (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCenter.Text & "')"
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
            If txtSearch_txt.Text <> "" Then
                strSql += vbCrLf + " AND T." & cmbSearchKey.Text & " LIKE"
                strSql += vbCrLf + " '" & txtSearch_txt.Text & "%'"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

            strSql = vbCrLf + "  IF (SELECT TOP 1 1 FROM MASTER..SYSOBJECTS WHERE NAME = 'TEMPTAGSTOCKVIEWDET')>0 "
            strSql += vbCrLf + "    DROP TABLE MASTER..TEMPTAGSTOCKVIEWDET"
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  TAGSNO"
            strSql += vbCrLf + "  ,I.ITEMNAME ITEM"
            strSql += vbCrLf + "  ,S.SUBITEMNAME SUBITEM"
            strSql += vbCrLf + "  ,T.STNPCS PCS"
            strSql += vbCrLf + "  ,T.STNWT WEIGHT"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR(15),ISNULL(CASE WHEN T.STONEUNIT='C' THEN (SELECT SIZEDESC FROM " & cnAdminDb & "..CENTSIZE WHERE T.STNWT BETWEEN FROMCENT AND TOCENT ) ELSE NULL END,'')) SEIVE"
            strSql += vbCrLf + "  INTO MASTER..TEMPTAGSTOCKVIEWDET"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..WITEMTAGSTONE AS T"
            strSql += vbCrLf + "  INNER JOIN MASTER..TEMPTAGSTOCKVIEW AS TT ON TT.SNO = T.TAGSNO"
            strSql += vbCrLf + "    INNER JOIN " & cnAdminDb & "..ITEMMAST AS I ON I.ITEMID = T.STNITEMID "
            strSql += vbCrLf + "    LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS S ON S.ITEMID = T.STNITEMID AND S.SUBITEMID = T.STNSUBITEMID "

            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

            count = Val(objGPack.GetSqlValue("SELECT TOP 1 COUNT(*) T FROM MASTER..TEMPTAGSTOCKVIEWDET GROUP BY TAGSNO ORDER BY COUNT(*) DESC"))

            For i As Integer = 1 To count
                strSql = vbCrLf + "   ALTER TABLE MASTER..TEMPTAGSTOCKVIEW ADD ITEM" + i.ToString() + " VARCHAR(50)"
                strSql += vbCrLf + "   ALTER TABLE MASTER..TEMPTAGSTOCKVIEW ADD SUBITEM" + i.ToString() + " VARCHAR(50)"
                strSql += vbCrLf + "   ALTER TABLE MASTER..TEMPTAGSTOCKVIEW ADD PCS" + i.ToString() + " int"
                strSql += vbCrLf + "   ALTER TABLE MASTER..TEMPTAGSTOCKVIEW ADD WEIGHT" + i.ToString() + " Numeric(12,4)"
                strSql += vbCrLf + "   ALTER TABLE MASTER..TEMPTAGSTOCKVIEW ADD SEIVE" + i.ToString() + " VARCHAR(15)"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

                strSql = vbCrLf + "   UPDATE TV SET ITEM" + i.ToString() + "=Y.ITEM ,SUBITEM" + i.ToString() + "=Y.SUBITEM, PCS" + i.ToString() + "=Y.PCS, WEIGHT" + i.ToString() + "=Y.WEIGHT, SEIVE" + i.ToString() + "=Y.SEIVE"
                strSql += vbCrLf + "   FROM MASTER..TEMPTAGSTOCKVIEW TV ,"
                strSql += vbCrLf + "   (SELECT SNO"
                strSql += vbCrLf + "   ,(SELECT ITEM FROM"
                strSql += vbCrLf + "  (SELECT ROW_NUMBER() OVER (ORDER BY TAGSNO,ITEM,SUBITEM,PCS,WEIGHT)AS ROWNUMBER ,ITEM"
                strSql += vbCrLf + "   FROM MASTER..TEMPTAGSTOCKVIEWDET "
                strSql += vbCrLf + "   WHERE TAGSNO =T.SNO"
                strSql += vbCrLf + "   )X WHERE X.ROWNUMBER =" + i.ToString() + " ) ITEM"
                strSql += vbCrLf + "   ,(SELECT SUBITEM FROM"
                strSql += vbCrLf + "  (SELECT ROW_NUMBER() OVER (ORDER BY TAGSNO,ITEM,SUBITEM,PCS,WEIGHT)AS ROWNUMBER ,SUBITEM"
                strSql += vbCrLf + "   FROM MASTER..TEMPTAGSTOCKVIEWDET "
                strSql += vbCrLf + "   WHERE TAGSNO =T.SNO"
                strSql += vbCrLf + "   )X WHERE X.ROWNUMBER =" + i.ToString() + " ) SUBITEM"
                strSql += vbCrLf + "   ,(SELECT PCS FROM"
                strSql += vbCrLf + "  (SELECT ROW_NUMBER() OVER (ORDER BY TAGSNO,ITEM,SUBITEM,PCS,WEIGHT)AS ROWNUMBER ,PCS"
                strSql += vbCrLf + "   FROM MASTER..TEMPTAGSTOCKVIEWDET "
                strSql += vbCrLf + "   WHERE TAGSNO =T.SNO"
                strSql += vbCrLf + "   )X WHERE X.ROWNUMBER =" + i.ToString() + " ) PCS"
                strSql += vbCrLf + "   ,(SELECT WEIGHT FROM"
                strSql += vbCrLf + "  (SELECT ROW_NUMBER() OVER (ORDER BY TAGSNO,ITEM,SUBITEM,PCS,WEIGHT)AS ROWNUMBER ,WEIGHT"
                strSql += vbCrLf + "   FROM MASTER..TEMPTAGSTOCKVIEWDET "
                strSql += vbCrLf + "   WHERE TAGSNO =T.SNO"
                strSql += vbCrLf + "   )X WHERE X.ROWNUMBER =" + i.ToString() + " ) WEIGHT"
                strSql += vbCrLf + "   ,(SELECT SEIVE FROM"
                strSql += vbCrLf + "  (SELECT ROW_NUMBER() OVER (ORDER BY TAGSNO,ITEM,SUBITEM,PCS,WEIGHT)AS ROWNUMBER ,SEIVE"
                strSql += vbCrLf + "   FROM MASTER..TEMPTAGSTOCKVIEWDET "
                strSql += vbCrLf + "   WHERE TAGSNO =T.SNO"
                strSql += vbCrLf + "   )X WHERE X.ROWNUMBER =" + i.ToString() + " ) SEIVE"
                strSql += vbCrLf + "   FROM MASTER..TEMPTAGSTOCKVIEW T)Y"
                strSql += vbCrLf + "  WHERE TV.SNO =Y.SNO"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

            Next i

        End If

        strSql = vbCrLf + "  SELECT * FROM MASTER..TEMPTAGSTOCKVIEW"
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
                dtGridResult.Rows.Add()
                RoDest = dtGridResult.Rows(SlNo)
                For j As Integer = 0 To dtGrid.Columns.Count - 1
                    ''If RoSource.Item(j).ToString <> "" Then
                    'SlNo += 1
                    'If i <> dtGrid.Rows.Count - 1 Then RoDest.Item("SLNO") = SlNo
                    'TagSno = RoSource.Item("SNO").ToString

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
        objGridShower.Text = "WEB TAG STOCK REPORT"
        Dim tit As String = Nothing
        tit += "WEB TAG STOCK REPORT"
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
        objGridShower.gridView.Rows(objGridShower.gridView.RowCount - 1).DefaultCellStyle = GlobalVariables.reportTotalStyle
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
    Private Sub GridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then
            Dim dgv As DataGridView = CType(sender, DataGridView)
            If dgv.CurrentRow Is Nothing Then Exit Sub
            If UCase(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'TAGEDITB4DATE'", , "N")) = "N" Then
                If Format(dgv.Rows(dgv.CurrentRow.Index).Cells("RECDATE").Value, "yyyy-MM-dd") <> GetServerDate(tran) Then
                    MsgBox("Cannot Edit this Tag", MsgBoxStyle.Information)
                    Exit Sub
                End If
            End If
            Dim IS_USERLEVELPWD As Boolean = IIf(GetAdmindbSoftValue("USERLEVELPWD", "N") = "Y", True, False)
            If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Edit) Then Exit Sub
            If Not usrpwdok("TAGEDIT", IS_USERLEVELPWD) Then Exit Sub
            Try
                Dim obj As New frmWebTag(dgv.Rows(dgv.CurrentRow.Index).Cells("SNO").Value.ToString)
                obj.Show()
            Catch ex As Exception

            Finally
                objGridShower.Close()
                'btnView_Search.Focus()
                'btnView_Search_Click(Me, New EventArgs)
            End Try
        End If
    End Sub
    Private Sub txtItemCode_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtItemCode_NUM.KeyDown
        Dim itemId As String
        If e.KeyCode = Keys.Insert Then
            strSql = " SELECT DISTINCT"
            strSql += vbCrLf + " ITEMID, "
            strSql += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..WITEMTAG AS T"
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

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)

        ' rbtAll.Checked = True
        txtItemCode_NUM.Select()
        cmbGroup.Items.Clear()
        cmbGroup.Items.Add("NONE")
        cmbGroup.Items.Add("COUNTER")


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
                '.Columns("MAXWASTPER").HeaderText = "WAS%"
                '.Columns("MAXWAST").HeaderText = "WAST"
                .Columns("MAXMCGRM").HeaderText = "MC/G"
                .Columns("MAXMC").HeaderText = "MC"
                '.Columns("MINWASTPER").HeaderText = "WAS%"
                '.Columns("MINWAST").HeaderText = "WAST"
                '.Columns("MINMCGRM").HeaderText = "MC/G"
                '.Columns("MINMC").HeaderText = "MC"
                '.Columns("DESIGNER").HeaderText = "DES"

                .Columns("PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                .Columns("GRSWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                .Columns("NETWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                .Columns("DIAPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                .Columns("DIAWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                .Columns("STNPCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                .Columns("STNWT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                .Columns("RATE").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight

                .Columns("SLNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                .Columns("MAXMCGRM").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                .Columns("MAXMC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                '.Columns("MINWASTPER").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                '.Columns("MINWAST").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                '.Columns("MINMCGRM").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                '.Columns("MINMC").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
                .Columns("SNO").Visible = False
                .Columns("RESULT").Visible = False
                .Columns("COLHEAD").Visible = False
                .Columns("TAGVAL").Visible = False
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
            'strSql = " SELECT "
            'strSql += " ''[SLNO~TAGNO~PCS~GRSWT~NETWT~DIAPCS~DIAWT~STNPCS~STNWT~RATE]"
            'strSql += " ,''[MAXMCGRM~MAXMC]"
            ''strSql += " ,''[MINWASTPER~MINWAST~MINMCGRM~MINMC]"
            'strSql += " ,''[PARTICULAR~D.DESIGNER],''SCROLL"
        Else
            strSql = " SELECT "
            strSql += " ''[SLNO~STYLENO~ID~ITEM~IT TYPE~PCS~GRSWT~SNO]"

            For i As Integer = 1 To count
                strSql += " ,''[ITEM" & i & "~SUBITEM" & i & "~PCS" & i & "~WEIGHT" & i & "~SEIVE" & i & "" + i.ToString() + "]"
            Next
            strSql += " ,''SCROLL"

            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtHead)
            gridviewHead.DataSource = dtHead
        End If
        If chkStudColumn.Checked = False Then

            'gridviewHead.Columns("SLNO~TAGNO~PCS~GRSWT~NETWT~DIAPCS~DIAWT~STNPCS~STNWT~RATE").HeaderText = ""
            'gridviewHead.Columns("MAXMCGRM~MAXMC").HeaderText = "MAXIMUM"
            ''gridviewHead.Columns("MINWASTPER~MINWAST~MINMCGRM~MINMC").HeaderText = "MINIMUM"
            'gridviewHead.Columns("PARTICULAR~D.DESIGNER").HeaderText = ""
            'gridviewHead.Columns("SCROLL").HeaderText = ""
        Else
            gridviewHead.Columns("SLNO~STYLENO~ID~ITEM~IT TYPE~PCS~GRSWT~SNO").HeaderText = ""
            For i As Integer = 1 To count
                gridviewHead.Columns("ITEM" & i & "~SUBITEM" & i & "~PCS" & i & "~WEIGHT" & i & "~SEIVE" & i & "" + i.ToString()).HeaderText = "DIA " + i.ToString()
            Next
            gridviewHead.Columns("SCROLL").HeaderText = ""
            gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            SetGridHeadColWid(gridviewHead, count)
        End If

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
                .Columns("SLNO~STYLENO~ID~ITEM~IT TYPE~PCS~GRSWT~SNO").Width =
                f.gridView.Columns("SLNO").Width +
                f.gridView.Columns("STYLENO").Width +
                f.gridView.Columns("ID").Width +
                f.gridView.Columns("ITEM").Width +
                f.gridView.Columns("SUBITEM").Width +
                f.gridView.Columns(5).Width +
                f.gridView.Columns("PCS").Width +
                f.gridView.Columns("GRSWT").Width
                For i As Integer = 1 To count
                    Dim J As Integer = 8 + (i - 1) * 5
                    .Columns(i).Width =
                    f.gridView.Columns(J + 1).Width _
                    + f.gridView.Columns(J + 2).Width _
                    + f.gridView.Columns(J + 3).Width _
                    + f.gridView.Columns(J + 4).Width _
                    + f.gridView.Columns(J + 5).Width
                Next
            Else
                '.Columns("SLNO~TAGNO~PCS~GRSWT~NETWT~DIAPCS~DIAWT~STNPCS~STNWT~RATE").Width = _
                'f.gridView.Columns("SLNO").Width + _
                'f.gridView.Columns("TAGNO").Width + _
                'f.gridView.Columns("PCS").Width + _
                'f.gridView.Columns("GRSWT").Width + _
                'f.gridView.Columns("NETWT").Width + _
                'f.gridView.Columns("DIAPCS").Width + _
                'f.gridView.Columns("DIAWT").Width + _
                'f.gridView.Columns("STNWT").Width + _
                'f.gridView.Columns("RATE").Width
                '.Columns("MAXMCGRM~MAXMC").Width = _
                '+f.gridView.Columns("MAXMCGRM").Width _
                '+ f.gridView.Columns("MAXMC").Width
                '.Columns("PARTICULAR").Width = f.gridView.Columns("PARTICULAR").Width

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

        obj.p_txtTagNo = txtTagNo.Text


        obj.p_cmbItemType = cmbItemType.Text

        obj.p_cmbCostCenter = cmbCostCenter.Text

        obj.p_txtFromWt_WET = txtFromWt_WET.Text
        obj.p_txtToWt_WET = txtToWt_WET.Text
        obj.p_txtFromDiaWt_WET = txtFromDiaWt_WET.Text
        obj.p_txtToDiaWt_WET = txtToDiaWt_WET.Text
        obj.p_txtFromRate_WET = txtFromRate_WET.Text
        obj.p_txtToRate_WET = txtToRate_WET.Text
        obj.p_cmbGroup = cmbGroup.Text

        obj.p_chkStudColumn = chkStudColumn.Checked
        SetSettingsObj(obj, Me.Name, GetType(TagedItemsStockViewDetailed_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New TagedItemsStockViewDetailed_Properties
        GetSettingsObj(obj, Me.Name, GetType(TagedItemsStockViewDetailed_Properties))
        txtItemCode_NUM.Text = obj.p_txtItemCode_NUM
        txtItemName.Text = obj.p_txtItemName

        txtTagNo.Text = obj.p_txtTagNo


        cmbItemType.Text = obj.p_cmbItemType

        'cmbCostCenter.Text = obj.p_cmbCostCenter

        txtFromWt_WET.Text = obj.p_txtFromWt_WET
        txtToWt_WET.Text = obj.p_txtToWt_WET
        txtFromDiaWt_WET.Text = obj.p_txtFromDiaWt_WET
        txtToDiaWt_WET.Text = obj.p_txtToDiaWt_WET
        txtFromRate_WET.Text = obj.p_txtFromRate_WET
        txtToRate_WET.Text = obj.p_txtToRate_WET
        cmbGroup.Text = obj.p_cmbGroup
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



