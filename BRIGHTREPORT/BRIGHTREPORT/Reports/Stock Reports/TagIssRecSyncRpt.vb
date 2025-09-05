Imports System.Data.OleDb
Public Class TagIssRecSyncRpt
    Dim objGridShower As frmGridDispDia
    Dim objGridShower1 As frmGridDispDia
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand

    Private Sub TagIssRecSyncRpt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub TagIssRecSyncRpt_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
        cmbItemName.Items.Clear()
        cmbItemName.Items.Add("ALL")
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ACTIVE = 'Y'"
        strSql += " AND STOCKTYPE = 'T'"
        objGPack.FillCombo(strSql, cmbItemName, False, False)

        CmbItemCounter.Items.Clear()
        CmbItemCounter.Items.Add("ALL")
        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER"
        strSql += " WHERE ACTIVE = 'Y'"
        objGPack.FillCombo(strSql, CmbItemCounter, False, False)
        CmbItemCounter.Text = "ALL"


        ''Designer
        cmbDesigner.Items.Clear()
        cmbDesigner.Items.Add("ALL")
        strSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER ORDER BY DESIGNERNAME"
        objGPack.FillCombo(strSql, cmbDesigner, False, False)
        cmbDesigner.Text = "ALL"

        cmbMetal.Items.Add("ALL")
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST ORDER BY METALNAME"
        objGPack.FillCombo(strSql, cmbMetal, False)
        cmbMetal.Text = "ALL"

        cmbCategory.Items.Clear()
        cmbCategory.Items.Add("ALL")
        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY"
        strSql += " WHERE ACTIVE = 'Y'"
        objGPack.FillCombo(strSql, cmbCategory, False, False)
        cmbCategory.Text = "ALL"

        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkLstCostCentre.Enabled = True
            chkLstCostCentre.Items.Clear()
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            FillCheckedListBox(strSql, chkLstCostCentre)
            FillCheckedListBox(strSql, chkLstFCostCentre)
        Else
            chkLstCostCentre.Enabled = False
            chkLstCostCentre.Items.Clear()
            chkLstFCostCentre.Enabled = False
            chkLstFCostCentre.Items.Clear()
        End If
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub chkAsOnDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAsOnDate.CheckedChanged
        If chkAsOnDate.Checked Then
            Label1.Visible = False
            chkAsOnDate.Text = "As OnDate"
            dtpTo.Visible = False
        Else
            chkAsOnDate.Text = "Date From"
            Label1.Visible = True
            dtpTo.Visible = True
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        cmbOrderBy.Items.Clear()
        cmbOrderBy.Items.Add("TAGNO")
        cmbOrderBy.Items.Add("COSTCENTRE")
        ' cmbItemName.Text = "ALL"
        'cmbOrderBy.Text = "TAGNO"
        'chkItemWiseGroup.Checked = False
        'chkAsOnDate.Checked = True
        chkAsOnDate.Select()
        Prop_Gets()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not chkLstCostCentre.CheckedItems.Count > 0 Then chkCostCentreSelectAll.Checked = True
        If Not chkLstFCostCentre.CheckedItems.Count > 0 Then chkFCostCentreSelectAll.Checked = True

        Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre, True)
        Dim chkFCostName As String = GetSelectedCostId(chkLstFCostCentre, True)
        Dim dtGrid As New DataTable
        strSql = vbCrLf + " DECLARE @FROMDATE SMALLDATETIME"
        strSql += vbCrLf + " DECLARE @TODATE SMALLDATETIME"
        strSql += vbCrLf + " DECLARE @TRANINVNO VARCHAR(25)"
        strSql += vbCrLf + " SET @FROMDATE = '" & IIf(chkAsOnDate.Checked, _AsOnFromDate.ToString("yyyy-MM-dd"), dtpFrom.Value.ToString("yyyy-MM-dd")) & "'"
        strSql += vbCrLf + " SET @TODATE = '" & IIf(chkAsOnDate.Checked, dtpFrom.Value.ToString("yyyy-MM-dd"), dtpTo.Value.ToString("yyyy-MM-dd")) & "'"
        strSql += vbCrLf + " SET @TRANINVNO = '" & txtrefNo.Text & "'"
        strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_SYNCISSREC') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_SYNCISSREC"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " CONVERT(VARCHAR(200),NULL)PARTICULAR"
        If rbtIssue.Checked Then
            strSql += vbCrLf + " ,IFC.COSTNAME [ISSUED FROM]"
            strSql += vbCrLf + " ,ITC.COSTNAME [ISSUED TO]"
        Else
            strSql += vbCrLf + " ,ITC.COSTNAME [ISSUED FROM]"
            strSql += vbCrLf + " ,IFC.COSTNAME [ISSUED TO]"
        End If
        strSql += vbCrLf + " ,IM.ITEMNAME,SM.SUBITEMNAME,TAG.TAGNO,TAG.PCS,TAG.GRSWT,TAG.NETWT"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = TAG.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = TAG.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = TAG.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = TAG.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
        strSql += vbCrLf + " ,CASE WHEN TAG.MAXWASTPER=0 THEN NULL ELSE TAG.MAXWASTPER END AS WASTPER "
        strSql += vbCrLf + " ,CASE WHEN TAG.MAXWAST=0 THEN NULL ELSE TAG.MAXWAST END AS WASTAGE"
        strSql += vbCrLf + " ,CASE WHEN TAG.MAXMCGRM=0 THEN NULL ELSE TAG.MAXMCGRM END AS MCGRM "
        strSql += vbCrLf + " ,CASE WHEN TAG.MAXMC=0 THEN NULL ELSE TAG.MAXMC END AS MC"
        strSql += vbCrLf + " ,TAG.RATE,TAG.SALVALUE"
        strSql += vbCrLf + " ,TAG.TABLECODE"
        strSql += vbCrLf + " ,(SELECT TOP 1 LOTNO FROM " & cnAdminDb & "..ITEMLOT L WHERE L.SNO=TAG.LOTSNO)LOTNO"
        strSql += vbCrLf + " ,(SELECT NAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID=TAG.ITEMTYPEID)ITEMTYPE"


        strSql += vbCrLf + "  ,(SELECT TOP 1 SHORTNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID IN ((SELECT STNITEMID FROM " & cnAdminDb & "..ITEMTAGSTONE S WHERE  "
        strSql += vbCrLf + " S.ITEMID = TAG.ITEMID AND S.TAGNO = TAG.TAGNO  AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')))) [CLARITY]"

        strSql += vbCrLf + " ,CASE WHEN STKTYPE='M' THEN 'MANUFACTURING' WHEN STKTYPE='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STOCKTYPE"
        strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = TAG.DESIGNERID) DESIGNERNAME"
        strSql += vbCrLf + " ,TAG.TAGVAL/*,TAG.TRANINVNO REFNO*/,CASE WHEN ISNULL(T.REFNO,'') <>'' THEN T.REFNO ELSE TAG.TRANINVNO END REFNO "
        strSql += vbCrLf + " ,CONVERT(VARCHAR(200),NULL)GROUP1"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(200),NULL)GROUP2"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(3),NULL)COLHEAD"
        strSql += vbCrLf + " ,CONVERT(INT,1)RESULT"
        strSql += vbCrLf + " ,T.TRANDATE AS [ISSUED DATE]"
        strSql += vbCrLf + " ,CONVERT(VARCHAR(100),(CASE WHEN ISNULL((SELECT ENTORDER FROM " & cnAdminDb & "..CTRANSFER WHERE TAGSNO=T.TAGSNO AND TRFNO=T.REFNO AND COSTID=T.FROMID) ,0)=1 THEN 'LOT' ELSE 'TRANSFER' END)) FLAG"
        If chkCounterWiseGroup.Checked = True Then
            strSql += vbCrLf + " , CNT.ITEMCTRNAME AS COUNTERNAME "
        End If
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_SYNCISSREC"
        strSql += vbCrLf + " FROM " & cnStockDb & "..TRANSIT_AUDIT T"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG AS TAG ON TAG.SNO = T.TAGSNO"
        If cmbItemName.Text <> "ALL" And cmbItemName.Text <> "" Then
            strSql += " AND TAG.ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItemName.Text & "')"
        End If
        If CmbItemCounter.Text <> "ALL" And CmbItemCounter.Text <> "" Then
            strSql += " AND TAG.ITEMCTRID = (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME = '" & CmbItemCounter.Text & "')"
        End If
        If cmbDesigner.Text <> "ALL" And cmbDesigner.Text <> "" Then
            strSql += " AND TAG.DESIGNERID = (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner.Text & "')"
        End If
        If cmbMetal.Text <> "ALL" And cmbMetal.Text <> "" Then
            strSql += " AND TAG.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID=(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME= '" & cmbMetal.Text & "'))"
        End If
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = TAG.ITEMID"
        If cmbCategory.Text <> "ALL" And cmbCategory.Text <> "" Then
            strSql += " AND IM.CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME = '" & cmbCategory.Text & "')"
        End If
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..COSTCENTRE AS IFC ON IFC.COSTID = T.FROMID "
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.SUBITEMID = TAG.SUBITEMID AND SM.ITEMID=TAG.ITEMID"
        If chkCostName <> "" Then
            strSql += vbCrLf + " " & IIf(rbtIssue.Checked, " AND IFC.COSTNAME IN (" & chkCostName & ")", "") & ""
        End If
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..COSTCENTRE AS ITC ON ITC.COSTID = T.TOID "
        If chkCostName <> "" Then
            strSql += vbCrLf + " " & IIf(rbtReceipt.Checked, " AND ITC.COSTNAME IN (" & chkCostName & ")", "") & ""
        End If
        If chkCounterWiseGroup.Checked = True Then
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMCOUNTER AS CNT ON CNT.ITEMCTRID = TAG.ITEMCTRID "
        End If
        strSql += vbCrLf + " WHERE 1=1 "
        If ChkActDate.Checked Then
            strSql += vbCrLf + " AND TAG.ACTUALRECDATE BETWEEN @FROMDATE AND @TODATE "
        Else
            strSql += vbCrLf + " AND T.TRANDATE BETWEEN @FROMDATE AND @TODATE "
        End If
        If txtrefNo.Text <> "" Then
            strSql += vbCrLf + " AND TRANINVNO=@TRANINVNO "
        End If
        strSql += vbCrLf + " AND T.ISSREC = '" & IIf(rbtIssue.Checked, "I", "R") & "'"
        If Not chkFCostCentreSelectAll.Checked Then
            If rbtReceipt.Checked Then
                strSql += vbCrLf + " AND T.FROMID IN(" & chkFCostName & ")"
            Else
                strSql += vbCrLf + " AND T.TOID IN(" & chkFCostName & ")"
            End If
        End If
        If rbtLot.Checked Then
            strSql += vbCrLf + " "
            strSql += vbCrLf + " DELETE FROM TEMPTABLEDB..TEMP_SYNCISSREC WHERE FLAG<>'LOT'"
        End If
        If rbtTransfer.Checked Then
            strSql += vbCrLf + " "
            strSql += vbCrLf + " DELETE FROM TEMPTABLEDB..TEMP_SYNCISSREC WHERE FLAG<>'TRANSFER'"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        If chkCounterWiseGroup.Checked Then
            strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER"
            strSql += vbCrLf + " SELECT PARTICULAR,[ISSUED FROM],[ISSUED TO],ITEMNAME,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT"
            strSql += vbCrLf + " ,SUM(DIAPCS)DIAPCS,SUM(DIAWT)DIAWT,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT,GROUP1,GROUP2,COLHEAD,RESULT"
            strSql += vbCrLf + " ,COUNTERNAME,NULL WASTAGE,NULL MC,NULL STOCKTYPE"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER "
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_SYNCISSREC "
            strSql += vbCrLf + " GROUP BY ITEMNAME ,COUNTERNAME,PARTICULAR,GROUP1,GROUP2,COLHEAD,RESULT,[ISSUED FROM],[ISSUED TO]"
            strSql += vbCrLf + " ORDER BY COUNTERNAME"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If


        strSql = vbCrLf + ""

        If chkCounterWiseGroup.Checked = True Then
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER SET PARTICULAR = ITEMNAME,GROUP1 = [ISSUED FROM] ,GROUP2 = ' '+COUNTERNAME"
        Else
            strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP_SYNCISSREC SET PARTICULAR = '   ' + CONVERT(VARCHAR,[ISSUED DATE],103),GROUP1 = [ISSUED FROM]"
            If chkItemWiseGroup.Checked Then strSql += vbCrLf + " ,GROUP2 = ' '+ITEMNAME"
        End If
        If chkCounterWiseGroup.Checked Then
            strSql += vbCrLf + " "
            strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER)>0"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER(PARTICULAR,GROUP1,COLHEAD,RESULT,STOCKTYPE)"
            strSql += vbCrLf + " SELECT DISTINCT GROUP1,GROUP1,'T',0 RESULT,'' FROM TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER WHERE RESULT = 1"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER(PARTICULAR,GROUP1,GROUP2,COLHEAD,RESULT,STOCKTYPE)"
            strSql += vbCrLf + " SELECT DISTINCT GROUP2,GROUP1,GROUP2,'T1',0,'' RESULT FROM TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER WHERE RESULT = 1"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER(PARTICULAR,GROUP1,GROUP2,COLHEAD,RESULT,PCS,GRSWT,NETWT,DIAPCS,DIAWT,STNPCS,STNWT)"
            strSql += vbCrLf + " SELECT GROUP2 + ' TOTAL',GROUP1,GROUP2,'S1',2 RESULT,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(DIAPCS),SUM(DIAWT),SUM(STNPCS),SUM(STNWT) FROM TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER WHERE RESULT = 1 GROUP BY GROUP2,GROUP1"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER(PARTICULAR,GROUP1,GROUP2,COLHEAD,RESULT,PCS,GRSWT,NETWT,DIAPCS,DIAWT,STNPCS,STNWT)"
            strSql += vbCrLf + " SELECT GROUP1 + ' TOTAL',GROUP1,'ZZZ'GROUP2,'S',2 RESULT,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(DIAPCS),SUM(DIAWT),SUM(STNPCS),SUM(STNWT) FROM TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER WHERE RESULT = 1 GROUP BY GROUP1"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER(PARTICULAR,GROUP1,GROUP2,COLHEAD,RESULT,PCS,GRSWT,NETWT,DIAPCS,DIAWT,STNPCS,STNWT)"
            strSql += vbCrLf + " SELECT 'GRAND TOTAL','ZZZZ'GROUP1,'ZZZ'GROUP2,'G',3 RESULT,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(DIAPCS),SUM(DIAWT),SUM(STNPCS),SUM(STNWT) FROM TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER WHERE RESULT = 1"
            strSql += vbCrLf + " END"
        Else
            strSql += vbCrLf + " "
            strSql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP_SYNCISSREC)>0"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_SYNCISSREC(PARTICULAR,GROUP1,COLHEAD,RESULT,STOCKTYPE)"
            strSql += vbCrLf + " SELECT DISTINCT GROUP1,GROUP1,'T',0 RESULT,'' FROM TEMPTABLEDB..TEMP_SYNCISSREC WHERE RESULT = 1"
            If CmbItemCounter.Text <> "ALL" Then
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_SYNCISSREC(PARTICULAR,GROUP1,COLHEAD,RESULT,STOCKTYPE)"
                strSql += vbCrLf + " SELECT DISTINCT '" & CmbItemCounter.Text & "',GROUP1,'T',0 RESULT,'' FROM TEMPTABLEDB..TEMP_SYNCISSREC WHERE RESULT = 1"
            End If
            If chkItemWiseGroup.Checked Then
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_SYNCISSREC(PARTICULAR,GROUP1,GROUP2,COLHEAD,RESULT,STOCKTYPE)"
                strSql += vbCrLf + " SELECT DISTINCT GROUP2,GROUP1,GROUP2,'T1',0,'' RESULT FROM TEMPTABLEDB..TEMP_SYNCISSREC WHERE RESULT = 1"
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_SYNCISSREC(PARTICULAR,GROUP1,GROUP2,COLHEAD,RESULT,PCS,GRSWT,NETWT,DIAPCS,DIAWT,WASTAGE,MC,STOCKTYPE,STNPCS,STNWT)"
                strSql += vbCrLf + " SELECT GROUP2 + ' TOTAL',GROUP1,GROUP2,'S1',2 RESULT,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(DIAPCS),SUM(DIAWT),SUM(WASTAGE),SUM(MC),'',SUM(STNPCS),SUM(STNWT) FROM TEMPTABLEDB..TEMP_SYNCISSREC WHERE RESULT = 1 GROUP BY GROUP2,GROUP1"
            End If
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_SYNCISSREC(PARTICULAR,GROUP1,GROUP2,COLHEAD,RESULT,PCS,GRSWT,NETWT,DIAPCS,DIAWT,WASTAGE,MC,STOCKTYPE,STNPCS,STNWT)"
            strSql += vbCrLf + " SELECT GROUP1 + ' TOTAL',GROUP1,'ZZZ'GROUP2,'S',2 RESULT,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(DIAPCS),SUM(DIAWT),SUM(WASTAGE),SUM(MC),'',SUM(STNPCS),SUM(STNWT) FROM TEMPTABLEDB..TEMP_SYNCISSREC WHERE RESULT = 1 GROUP BY GROUP1"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_SYNCISSREC(PARTICULAR,GROUP1,GROUP2,COLHEAD,RESULT,PCS,GRSWT,NETWT,DIAPCS,DIAWT,WASTAGE,MC,STOCKTYPE,STNPCS,STNWT)"
            strSql += vbCrLf + " SELECT 'GRAND TOTAL','ZZZZ'GROUP1,'ZZZ'GROUP2,'G',3 RESULT,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(DIAPCS),SUM(DIAWT),SUM(WASTAGE),SUM(MC),'',SUM(STNPCS),SUM(STNWT) FROM TEMPTABLEDB..TEMP_SYNCISSREC WHERE RESULT = 1"
            strSql += vbCrLf + " END"
        End If

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        If ChkSummary.Checked Then
            strSql = " IF OBJECT_ID('TEMPTABLEDB..TEMP_SYNCISSRECSUMM') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMP_SYNCISSRECSUMM"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " SELECT PARTICULAR,[ISSUED FROM],[ISSUED TO],ITEMNAME "
            strSql += vbCrLf + " ,SUM(PCS)PCS ,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT"
            strSql += vbCrLf + " ,SUM(DIAPCS)DIAPCS,SUM(DIAWT)DIAWT,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),AVG(RATE)) RATE,CONVERT(NUMERIC(15,2),SUM(SALVALUE)) SALVALUE,REFNO,FLAG,CONVERT(VARCHAR(200),NULL)GROUP1,CONVERT(VARCHAR(200)"
            strSql += vbCrLf + " ,NULL)GROUP2,CONVERT(VARCHAR(3),NULL)COLHEAD,CONVERT(INT,1)RESULT"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP_SYNCISSRECSUMM"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_SYNCISSREC "
            strSql += vbCrLf + " WHERE RESULT=1  "
            strSql += vbCrLf + " GROUP BY PARTICULAR,[ISSUED FROM],[ISSUED TO],ITEMNAME,REFNO,FLAG,RATE "
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = " UPDATE TEMPTABLEDB..TEMP_SYNCISSRECSUMM SET GROUP1 = [ISSUED FROM]"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            strSql = vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP_SYNCISSRECSUMM)>0"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_SYNCISSRECSUMM(PARTICULAR,GROUP1,COLHEAD,RESULT)"
            strSql += vbCrLf + " SELECT DISTINCT GROUP1,GROUP1,'T',0 RESULT FROM TEMPTABLEDB..TEMP_SYNCISSRECSUMM WHERE RESULT = 1"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_SYNCISSRECSUMM(PARTICULAR,GROUP1,GROUP2,COLHEAD,RESULT,PCS,GRSWT,NETWT,DIAPCS,DIAWT,RATE,SALVALUE)"
            strSql += vbCrLf + " SELECT GROUP1 + ' TOTAL',GROUP1,'ZZZ'GROUP2,'S',2 RESULT,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(DIAPCS),SUM(DIAWT),SUM(RATE),SUM(SALVALUE) FROM TEMPTABLEDB..TEMP_SYNCISSRECSUMM WHERE RESULT = 1 GROUP BY GROUP1"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP_SYNCISSRECSUMM(PARTICULAR,GROUP1,GROUP2,COLHEAD,RESULT,PCS,GRSWT,NETWT,DIAPCS,DIAWT,RATE,SALVALUE)"
            strSql += vbCrLf + " SELECT 'GRAND TOTAL','ZZZZ'GROUP1,'ZZZ'GROUP2,'G',3 RESULT,SUM(PCS),SUM(GRSWT),SUM(NETWT),SUM(DIAPCS),SUM(DIAWT),SUM(RATE),SUM(SALVALUE) FROM TEMPTABLEDB..TEMP_SYNCISSRECSUMM WHERE RESULT = 1"
            strSql += vbCrLf + " END"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMP_SYNCISSRECSUMM"
            strSql += vbCrLf + " ORDER BY GROUP1,GROUP2,RESULT"
        Else
            If chkCounterWiseGroup.Checked Then
                strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMP_SYNCISSREC_COUNTER"
            Else
                strSql = vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMP_SYNCISSREC"
            End If
            strSql += vbCrLf + " ORDER BY GROUP1,GROUP2,RESULT"
            If cmbOrderBy.Text = "COSTCENTRE" Then strSql += vbCrLf + ",[ISSUED TO]"
            If chkCounterWiseGroup.Checked = False Then
                strSql += vbCrLf + " ,TAGVAL"
            End If
        End If
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
        If chkCounterWiseGroup.Checked Then
            objGridShower.Text = "ITEM WISE TRANSFER ISSUE/RECEIPT"
        Else
            objGridShower.Text = "TAG WISE TRANSFER ISSUE/RECEIPT"
        End If

        Dim tit As String = ""
        If cmbItemName.Text <> "ALL" And cmbItemName.Text <> "" Then
            tit += cmbItemName.Text + " - "
        End If
        If chkCounterWiseGroup.Checked Then
            tit += "ITEM WISE TRANSFER " + IIf(rbtIssue.Checked, "ISSUE", "RECEIPT")
        Else
            tit += "TAG WISE TRANSFER " + IIf(rbtIssue.Checked, "ISSUE", "RECEIPT")
        End If

        If chkAsOnDate.Checked Then
            tit += " AS ON " & dtpFrom.Text & ""
        Else
            tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
        End If
        tit += vbCrLf + " FOR " & chkCostName.Replace("'", "")
        If CmbItemCounter.Text <> "ALL" Then
            tit += " [ " + CmbItemCounter.Text + " ] "
        End If

        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        'objGridShower.pnlFooter.Visible = False
        If ChkSummary.Checked = False Then objGridShower.pnlFooter.Visible = True
        If ChkSummary.Checked = False Then objGridShower.lblStatus.Text = "<Press [D] for Stone / Diamond Detail View>"
        AddHandler objGridShower.gridView.KeyDown, AddressOf DataGridView_KeyDown
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        DataGridView_SummaryFormatting(objGridShower.gridView)
        FormatGridColumns(objGridShower.gridView, False, False, , False)
        objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        objGridShower.Show()
        objGridShower.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))
        Prop_Sets()
    End Sub

    Private Sub DataGridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.D Then
            If objGridShower.gridView.Rows.Count > 0 Then
                Dim dtGrid1 As New DataTable
                Dim tagno As String = objGridShower.gridView.CurrentRow.Cells("TAGNO").Value.ToString
                Dim ItemName As String = objGridShower.gridView.CurrentRow.Cells("ITEMNAME").Value.ToString
                If tagno.ToString = "" Then Exit Sub
                strSql = vbCrLf + " SELECT (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = STNITEMID) STNITEMNAME"
                strSql += vbCrLf + " ,STNPCS"
                strSql += vbCrLf + " ,STNWT"
                strSql += vbCrLf + " ,STNRATE"
                strSql += vbCrLf + " ,STNAMT"
                strSql += vbCrLf + " ,DESCRIP"
                strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO = '" & tagno & "'"
                strSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & ItemName & "'))"
                dtGrid1.Columns.Add("KEYNO", GetType(Integer))
                dtGrid1.Columns("KEYNO").AutoIncrement = True
                dtGrid1.Columns("KEYNO").AutoIncrementSeed = 0
                dtGrid1.Columns("KEYNO").AutoIncrementStep = 1
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtGrid1)
                dtGrid1.Columns("KEYNO").SetOrdinal(dtGrid1.Columns.Count - 1)
                If Not dtGrid1.Rows.Count > 0 Then
                    MsgBox("Record not found", MsgBoxStyle.Information)
                    Exit Sub
                End If
                objGridShower1 = New frmGridDispDia
                objGridShower1.Name = Me.Name
                objGridShower1.gridView.RowTemplate.Height = 21
                objGridShower1.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
                objGridShower1.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                objGridShower1.Text = "STONE / DIAMOND DETAILS"
                Dim tit As String = ""
                tit = " STONE / DIAMOND DETAILS : " & ItemName & " / TAGNO : " & tagno
                objGridShower1.lblTitle.Text = tit
                objGridShower1.StartPosition = FormStartPosition.CenterScreen
                objGridShower1.dsGrid.DataSetName = objGridShower1.Name
                objGridShower1.dsGrid.Tables.Add(dtGrid1)
                objGridShower1.gridView.DataSource = objGridShower1.dsGrid.Tables(0)
                objGridShower1.FormReSize = False
                objGridShower1.FormReLocation = False
                objGridShower1.pnlFooter.Visible = False
                objGridShower1.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                'DataGridView_SummaryFormatting(objGridShower1.gridView)
                FormatGridColumns(objGridShower1.gridView, False, False, , False)
                objGridShower1.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
                objGridShower1.Show()
                objGridShower1.gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
                objGridShower1.FormReSize = True
                objGridShower1.FormReLocation = True
            End If
        End If
    End Sub


    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For CNT As Integer = 0 To .ColumnCount - 1
                .Columns(CNT).Visible = True
            Next
            For CNT As Integer = 20 To .ColumnCount - 1
                .Columns(CNT).Visible = False
            Next
            If chkCounterWiseGroup.Checked = True Then
                .Columns("ITEMNAME").Visible = False
                .Columns("COUNTERNAME").Visible = False
                .Columns("WASTAGE").Visible = False
                .Columns("STOCKTYPE").Visible = False
                .Columns("ISSUED TO").Visible = False
                .Columns("GROUP1").Visible = False
                .Columns("GROUP2").Visible = False
                .Columns("COLHEAD").Visible = False
                .Columns("RESULT").Visible = False
                .Columns("MC").Visible = False
                .Columns("KEYNO").Visible = False

            Else
                If ChkVaDetail.Checked = False And ChkSummary.Checked = False Then
                    .Columns("WASTAGE").Visible = False
                    .Columns("LOTNO").Visible = False
                    .Columns("WASTPER").Visible = False
                    .Columns("MCGRM").Visible = False
                    .Columns("MC").Visible = False
                    .Columns("TABLECODE").Visible = False
                End If
                If ChkSummary.Checked = True Then
                    .Columns("GROUP1").Visible = False
                    .Columns("GROUP2").Visible = False
                    .Columns("RESULT").Visible = False
                    .Columns("COLHEAD").Visible = False
                    .Columns("KEYNO").Visible = False
                End If
            End If

            .Columns("ISSUED TO").HeaderText = IIf(rbtIssue.Checked, "ISSUED TO", "RECEIVED FROM")
            .Columns("ISSUED FROM").Visible = False
            If chkCounterWiseGroup.Checked = False Then
                If ChkSummary.Checked = False Then .Columns("TABLECODE").HeaderText = "TABLE"
                .Columns("REFNO").Visible = True
                If ChkSummary.Checked = False Then .Columns("FLAG").Visible = True
                If ChkSummary.Checked = False Then .Columns("DESIGNERNAME").Visible = True
                If ChkSummary.Checked = False And .Columns.Contains("ITEMTYPE") Then .Columns("ITEMTYPE").Visible = True
                .Columns("ITEMNAME").Visible = Not chkItemWiseGroup.Checked
                If ChkSummary.Checked = False And .Columns.Contains("CLARITY") Then .Columns("CLARITY").Visible = True
            End If
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub
    Private Sub chkCostCentreSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkCostCentreSelectAll.Checked)
    End Sub
    Private Sub chkFCostCentreSelectAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkFCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstFCostCentre, chkFCostCentreSelectAll.Checked)
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New TagIssRecSyncRpt_Properties
        obj.p_chkAsOnDate = chkAsOnDate.Checked
        obj.p_cmbItemName = cmbItemName.Text
        obj.p_cmbcategory = cmbCategory.Text
        obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_chkFCostCentreSelectAll = chkFCostCentreSelectAll.Checked
        GetChecked_CheckedList(chkLstFCostCentre, obj.p_chkLstFCostCentre)
        obj.p_rbtIssue = rbtIssue.Checked
        obj.p_rbtReceipt = rbtReceipt.Checked
        obj.p_chkItemWiseGroup = chkItemWiseGroup.Checked
        obj.p_cmbOrderBy = cmbOrderBy.Text
        obj.p_chkInclVA = ChkVaDetail.Checked
        SetSettingsObj(obj, Me.Name, GetType(TagIssRecSyncRpt_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New TagIssRecSyncRpt_Properties
        GetSettingsObj(obj, Me.Name, GetType(TagIssRecSyncRpt_Properties))
        chkAsOnDate.Checked = obj.p_chkAsOnDate
        cmbItemName.Text = obj.p_cmbItemName
        cmbCategory.Text = obj.p_cmbcategory
        chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        chkFCostCentreSelectAll.Checked = obj.p_chkFCostCentreSelectAll
        SetChecked_CheckedList(chkLstFCostCentre, obj.p_chkLstFCostCentre, cnCostName)
        rbtIssue.Checked = obj.p_rbtIssue
        rbtReceipt.Checked = obj.p_rbtReceipt
        chkItemWiseGroup.Checked = obj.p_chkItemWiseGroup
        ChkVaDetail.Checked = obj.p_chkInclVA
        cmbOrderBy.Text = obj.p_cmbOrderBy
    End Sub

    Private Sub rbtIssue_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtIssue.CheckedChanged
        If rbtIssue.Checked Then
            chkFCostCentreSelectAll.Text = "To Centre"
        Else
            chkFCostCentreSelectAll.Text = "From Centre"
        End If
    End Sub

    Private Sub rbtReceipt_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtReceipt.CheckedChanged
        If rbtReceipt.Checked Then
            chkFCostCentreSelectAll.Text = "From Centre"
        Else
            chkFCostCentreSelectAll.Text = "To Centre"
        End If
    End Sub

    Private Sub cmbItemName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemName.GotFocus
        cmbItemName.Items.Clear()
        cmbItemName.Items.Add("ALL")
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ACTIVE = 'Y'"
        strSql += " AND STOCKTYPE = 'T'"
        If cmbMetal.Text <> "ALL" And cmbMetal.Text <> "" Then
            strSql += " AND METALID=(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & cmbMetal.Text & "')"
        End If
        objGPack.FillCombo(strSql, cmbItemName, False, False)
        cmbItemName.Text = "ALL"
    End Sub

    Private Sub ChkSummary_CheckedChanged(sender As Object, e As EventArgs) Handles ChkSummary.CheckedChanged
        chkItemWiseGroup.Enabled = Not ChkSummary.Checked : chkItemWiseGroup.Checked = False
        ChkActDate.Enabled = Not ChkSummary.Checked : ChkActDate.Checked = False
        ChkVaDetail.Enabled = Not ChkSummary.Checked : ChkVaDetail.Checked = False
    End Sub

    Private Sub cmbCategory_OWN_GotFocus(sender As Object, e As EventArgs) Handles cmbCategory.GotFocus
        cmbCategory.Items.Clear()
        cmbCategory.Items.Add("ALL")
        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY"
        strSql += " WHERE ACTIVE = 'Y'"
        If cmbMetal.Text <> "ALL" And cmbMetal.Text <> "" Then
            strSql += " AND METALID=(SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME='" & cmbMetal.Text & "')"
        End If
        objGPack.FillCombo(strSql, cmbCategory, False, False)
        cmbCategory.Text = "ALL"
    End Sub

    Private Sub chkCounterWiseGroup_CheckedChanged(sender As Object, e As EventArgs) Handles chkCounterWiseGroup.CheckedChanged
        If chkCounterWiseGroup.Checked Then
            ChkActDate.Enabled = False
            chkItemWiseGroup.Enabled = False
            ChkSummary.Enabled = False
            ChkVaDetail.Enabled = False
            txtrefNo.Enabled = False
            chkItemWiseGroup.Checked = False
            ChkSummary.Checked = False
        Else
            txtrefNo.Enabled = True
            ChkActDate.Enabled = True
            chkItemWiseGroup.Enabled = True
            ChkSummary.Enabled = True
            ChkVaDetail.Enabled = True
        End If
    End Sub
End Class

Public Class TagIssRecSyncRpt_Properties
    Private chkAsOnDate As Boolean = True
    Public Property p_chkAsOnDate() As Boolean
        Get
            Return chkAsOnDate
        End Get
        Set(ByVal value As Boolean)
            chkAsOnDate = value
        End Set
    End Property

    Private cmbItemName As String = "ALL"
    Public Property p_cmbItemName() As String
        Get
            Return cmbItemName
        End Get
        Set(ByVal value As String)
            cmbItemName = value
        End Set
    End Property
    Private cmbcategory As String = "ALL"
    Public Property p_cmbcategory() As String
        Get
            Return cmbcategory
        End Get
        Set(ByVal value As String)
            cmbcategory = value
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
    Private chkFCostCentreSelectAll As Boolean = False
    Public Property p_chkFCostCentreSelectAll() As Boolean
        Get
            Return chkFCostCentreSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkFCostCentreSelectAll = value
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
    Private chkLstFCostCentre As New List(Of String)
    Public Property p_chkLstFCostCentre() As List(Of String)
        Get
            Return chkLstFCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkLstFCostCentre = value
        End Set
    End Property

    Private rbtIssue As Boolean = True
    Public Property p_rbtIssue() As Boolean
        Get
            Return rbtIssue
        End Get
        Set(ByVal value As Boolean)
            rbtIssue = value
        End Set
    End Property
    Private rbtReceipt As Boolean = False
    Public Property p_rbtReceipt() As Boolean
        Get
            Return rbtReceipt
        End Get
        Set(ByVal value As Boolean)
            rbtReceipt = value
        End Set
    End Property
    Private chkItemWiseGroup As Boolean = False
    Public Property p_chkItemWiseGroup() As Boolean
        Get
            Return chkItemWiseGroup
        End Get
        Set(ByVal value As Boolean)
            chkItemWiseGroup = value
        End Set
    End Property
    Private chkCounterWiseGroup As Boolean = False
    Public Property p_chkCounterWiseGroup() As Boolean
        Get
            Return chkCounterWiseGroup
        End Get
        Set(ByVal value As Boolean)
            chkCounterWiseGroup = value
        End Set
    End Property
    Private chkInclVA As Boolean = False
    Public Property p_chkInclVA() As Boolean
        Get
            Return chkInclVA
        End Get
        Set(ByVal value As Boolean)
            chkInclVA = value
        End Set
    End Property

    Private cmbOrderBy As String = "TAGNO"
    Public Property p_cmbOrderBy() As String
        Get
            Return cmbOrderBy
        End Get
        Set(ByVal value As String)
            cmbOrderBy = value
        End Set
    End Property
End Class






