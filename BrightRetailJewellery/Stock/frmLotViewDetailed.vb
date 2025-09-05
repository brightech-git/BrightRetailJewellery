Imports System.Data.OleDb
Imports Microsoft.Office.Interop.Word
Public Class frmLotViewDetailed
    Dim strsql As String
    Dim objGridShower As frmGridDispDia
    Dim cmd As OleDbCommand
    Dim ragshow As Boolean = IIf(GetAdmindbSoftValue("RANGEINLOT", "N") = "Y", True, False)

    Private Sub frmLotViewDetailed_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub frmLotViewDetailed_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        strsql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ACTIVE = 'Y' ORDER BY METALNAME"
        FillCheckedListBox(strsql, chkLstMetal)
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkLstCostCentre.Enabled = True
            chkLstCostCentre.Items.Clear()
            strsql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            FillCheckedListBox(strsql, chkLstCostCentre)
        Else
            chkLstCostCentre.Enabled = False
            chkLstCostCentre.Items.Clear()
        End If

        chkLstCompany.Items.Clear()
        strsql = " SELECT COMPANYNAME FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N' ORDER BY DISPLAYORDER,COMPANYNAME"
        Dim dtCompany As New DataTable
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtCompany)
        For Each ro As DataRow In dtCompany.Rows
            chkLstCompany.Items.Add(ro("COMPANYNAME").ToString)
            If ro("COMPANYNAME").ToString = strCompanyName Then chkLstCompany.SetItemChecked(chkLstCompany.Items.Count - 1, True)
        Next

        cmbDesigner.Items.Clear()
        cmbDesigner.Items.Add("ALL")
        strsql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER ORDER BY DESIGNERNAME"
        objGPack.FillCombo(strsql, cmbDesigner, False)
        cmbDesigner.Text = "ALL"

        cmbItemName.Items.Clear()
        cmbItemName.Items.Add("ALL")
        strsql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ACTIVE,'') = 'Y'"
        strsql += " AND ISNULL(STOCKREPORT,'') = 'Y'"
        strsql += "  ORDER BY ITEMID"
        objGPack.FillCombo(strsql, cmbItemName, False)
        cmbItemName.Text = "ALL"

        cmbUserName.Items.Clear()
        cmbUserName.Items.Add("ALL")
        strsql = " SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER ORDER BY USERNAME"
        objGPack.FillCombo(strsql, cmbUserName, False)
        cmbUserName.Text = "ALL"


        strsql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER ORDER BY DESIGNERNAME"
        FillCheckedListBox(strsql, chkLstDesigner)
        strsql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' ORDER BY ITEMCTRNAME"
        FillCheckedListBox(strsql, chkLstItemCounter)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        objGPack.TextClear(Me)
        dtpFrom.Value = GetEntryDate(GetServerDate)
        dtpTo.Value = GetEntryDate(GetServerDate)
        rbtDetailed.Checked = True
        Prop_Gets()
        cmbUserName.Text = "ALL"
        dtpFrom.Select()
    End Sub

    Private Sub chkMetalSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkMetalSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstMetal, chkMetalSelectAll.Checked)
        LoadCategory()
        funcLoadItemName()
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

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If chkLstCostCentre.Items.Count > 0 Then
            If Not chkLstCostCentre.CheckedItems.Count > 0 Then
                chkCostCentreSelectAll.Checked = True
            End If
        End If
        If chkLstDesigner.Items.Count > 0 Then
            If Not chkLstDesigner.CheckedItems.Count > 0 Then
                chkDesignerSelectAll.Checked = True
            End If
        Else

        End If
        If chkLstItemCounter.Items.Count > 0 Then
            If Not chkLstItemCounter.CheckedItems.Count > 0 Then
                chkItemCounterSelectAll.Checked = True
            End If
        End If
        If chkLstCategory.Items.Count > 0 Then
            If Not chkLstCategory.CheckedItems.Count > 0 Then
                chkCategorySelectAll.Checked = True
            End If
        End If
        Dim chkCompany As String = GetChecked_CheckedList(chkLstCompany)
        Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre)
        'Dim chkDesigner As String = GetChecked_CheckedList(chkLstDesigner)
        Dim chkDesigner As String = ""
        If chkMultiDesigner.Checked Then
            chkDesigner = GetChecked_CheckedList(chkLstDesigner)
        Else
            If cmbDesigner.Text <> "" And cmbDesigner.Text <> "ALL" Then
                chkDesigner = "'" & cmbDesigner.Text & "'"
            Else
                chkDesigner = ""
            End If

        End If
        Dim chkItemCounter As String = GetChecked_CheckedList(chkLstItemCounter)
        Dim chkCategory As String = GetChecked_CheckedList(chkLstCategory)
        Dim chkmetal As String = GetChecked_CheckedList(chkLstMetal)
        Dim chkitemname As String = ""
        If cmbItemName.Text <> "" And cmbItemName.Text <> "ALL" Then
            chkitemname = "'" & cmbItemName.Text & "'"
        Else
            chkitemname = ""
        End If

        strsql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "LOTVIEW') > 0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "LOTVIEW"
        strsql += vbCrLf + " IF (SELECT COUNT(*) FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMLOT') > 0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ITEMLOT"
        cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strsql = " SELECT * INTO TEMPTABLEDB..TEMP" & systemId & "ITEMLOT"
        strsql += vbCrLf + " FROM " & cnAdminDb & "..ITEMLOT AS L"
        strsql += vbCrLf + " WHERE LOTDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"

        If chkCompany <> "" Then strsql += vbCrLf + " AND COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & chkCompany & "))"
        If chkCostName <> "" Then strsql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkDesigner <> "" Then
            strsql += vbCrLf + " AND ISNULL(DESIGNERID,0) IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & chkDesigner & ")"
            If chkDesignerSelectAll.Checked = True Then strsql += vbCrLf + "  UNION ALL  SELECT 0 DESIGNERID FROM " & cnAdminDb & "..DESIGNER "
            strsql += vbCrLf + " )"
        End If
        If chkItemCounter <> "" Then
            strsql += vbCrLf + " AND ISNULL(ITEMCTRID,0) IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & chkItemCounter & ")"
            If chkItemCounterSelectAll.Checked = True Then strsql += vbCrLf + "  UNION ALL  SELECT 0 ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER "
            strsql += vbCrLf + " )"
        End If
        If chkCategory <> "" Then strsql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & ")))"
        If chkmetal <> "" Then strsql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkmetal & ")))"
        If chkitemname <> "" Then strsql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkitemname & "))"
        If rbtLTLot.Checked Then
            strsql += vbCrLf + " AND SNO NOT IN(SELECT LOTSNO FROM " & cnStockDb & "..LOTISSUE WHERE ISNULL(RECSNO,'') IN(SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(CANCEL,'')=''))"
        ElseIf rbtLTReceipt.Checked Then
            strsql += vbCrLf + " AND SNO IN(SELECT LOTSNO FROM " & cnStockDb & "..LOTISSUE WHERE ISNULL(RECSNO,'') IN(SELECT SNO FROM " & cnStockDb & "..RECEIPT WHERE ISNULL(CANCEL,'')=''))"
        End If
        If txtLotNo_NUM.Text <> "" And txtLotNoTo_NUM.Text <> "" Then
            strsql += vbCrLf + " AND LOTNO BETWEEN '" & txtLotNo_NUM.Text & "' AND '" & txtLotNoTo_NUM.Text & "'"
        ElseIf txtLotNo_NUM.Text <> "" Then
            strsql += vbCrLf + " AND LOTNO ='" & txtLotNo_NUM.Text & "'"
        End If
        If cmbUserName.Text <> "" And cmbUserName.Text <> "ALL" Then strsql += vbCrLf + " AND USERID IN (SELECT USERID FROM " & cnAdminDb & "..USERMASTER WHERE USERNAME IN ('" & cmbUserName.Text & "'))"
        cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strsql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "ITEMLOT"
        strsql += vbCrLf + " SET PCS = 0,GRSWT = 0,NETWT = 0,DIAPCS = T.PCS"
        strsql += vbCrLf + " ,DIAWT = T.GRSWT"
        strsql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ITEMLOT AS L"
        strsql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMLOT AS T ON T.SNO = L.SNO"
        strsql += vbCrLf + " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN ('D') AND ISNULL(STUDDED,'')<>'L')"
        cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strsql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "ITEMLOT"
        strsql += vbCrLf + " SET PCS = 0,GRSWT = 0,NETWT = 0,STNPCS = T.PCS"
        strsql += vbCrLf + " ,STNWT = T.GRSWT"
        strsql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ITEMLOT AS L"
        strsql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMLOT AS T ON T.SNO = L.SNO"
        strsql += vbCrLf + " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN ('T') AND ISNULL(STUDDED,'')<>'L')"
        cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        If rbtDetailed.Checked Then
            strsql = " SELECT LOTDATE,LOTNO,LOTNO AS LOTNO1,ITEM,COUNTER,PCS,GRSWT,NETWT,DIAPCS,DIAWT,STNPCS,STNWT,CPCS,CGRSWT,CNETWT,CONVERT(INT,CDIAPCS)CDIAPCS,CONVERT(NUMERIC(15,4),CDIAWT)CDIAWT,CONVERT(INT,CSTNPCS)CSTNPCS,CONVERT(NUMERIC(15,4),CSTNWT)CSTNWT"
            strsql += vbCrLf + " ,CONVERT(NUMERIC(15,4),RSALVALUE)RSALVALUE"
            strsql += vbCrLf + " ,CONVERT(NUMERIC(15,4),WSALVALUE)WSALVALUE"
            strsql += vbCrLf + " ,BPCS,BGRSWT,BNETWT,CONVERT(INT,DIAPCS-CDIAPCS) AS BDIAPCS,CONVERT(NUMERIC(15,4),DIAWT-CDIAWT) AS BDIAWT,CONVERT(INT,STNPCS-CSTNPCS) AS BSTNPCS,CONVERT(NUMERIC(15,4),STNWT-CSTNWT) AS BSTNWT"
            strsql += vbCrLf + " ,DESIGNER,TABLECODE,NARRATION"
            If ragshow = True Then strsql += vbCrLf + " ,RANGE"
            strsql += vbCrLf + " ,LDATE,1 RESULT,CONVERT(VARCHAR,'')COLHEAD,METAL,CONVERT(VARCHAR,OPENTIME,103)OPENDATE,CONVERT(VARCHAR(5),OPENTIME,114)OPENTIME,CONVERT(VARCHAR,CLOSETIME,103)CLOSEDATE,CONVERT(VARCHAR(5),CLOSETIME,114)CLOSETIME"
            strsql += vbCrLf + " ,(SELECT USERNAME FROM " & cnAdminDb & "..USERMASTER WHERE USERID = X.USERID)AS USERNAME"
            strsql += vbCrLf + " , CONVERT(VARCHAR,CANCEL) CANCEL,FROMTAGNO AS FROM_TAGNO,TOTAGNO AS TO_TAGNO "
            strsql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "LOTVIEW"
            strsql += vbCrLf + " FROM"
            strsql += vbCrLf + " ("
            strsql += vbCrLf + " SELECT CONVERT(VARCHAR,LOTDATE,103)LOTDATE,LOTNO"

            If chkOrderByItemId.Checked Then
                strsql += vbCrLf + "     ,(SELECT '['+REPLICATE(' ',5-LEN(L.ITEMID)) + CONVERT(VARCHAR,L.ITEMID) + '] ' + ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = L.ITEMID)AS ITEM"
            Else
                strsql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = L.ITEMID)AS ITEM"
            End If
            strsql += vbCrLf + " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = L.ITEMCTRID)AS COUNTER"
            strsql += vbCrLf + " ,PCS,GRSWT,NETWT,DIAPCS,DIAWT,STNPCS,STNWT,CPCS,CGRSWT,CNETWT"
            strsql += vbCrLf + " ,CONVERT(INT,PCS-CPCS) AS BPCS"
            strsql += vbCrLf + " ,CONVERT(NUMERIC(15,3),GRSWT-CGRSWT) AS BGRSWT"
            strsql += vbCrLf + " ,CONVERT(NUMERIC(15,3),NETWT-CNETWT) AS BNETWT"
            strsql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = L.DESIGNERID)AS DESIGNER"
            strsql += vbCrLf + " ,TABLECODE"
            strsql += vbCrLf + " ,NARRATION"

            If ragshow = True Then strsql += vbCrLf + " ,(SELECT CAPTION FROM " & cnAdminDb & "..RANGEMAST WHERE SNO=L.RANGESNO)RANGE"

            strsql += vbCrLf + " ,LOTDATE LDATE,1 RESULT,' 'COLHEAD"

            'strsql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE LOTSNO = L.SNO) AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') ),0) AS CDIAPCS"
            'strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE LOTSNO = L.SNO) AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') ),0) AS CDIAWT"
            'strSql += vbCrLf + " ,ISNULL((SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE LOTSNO = L.SNO) AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S') ),0) AS CSTNPCS"
            'strsql += vbCrLf + " ,ISNULL((SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE LOTSNO = L.SNO) AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S') ),0) AS CSTNWT"

            strsql += vbCrLf + ",ISNULL("
            strsql += vbCrLf + "(SELECT SUM(STNPCS) STNPCS FROM"
            strsql += vbCrLf + "("
            strsql += vbCrLf + "SELECT ISNULL(SUM(STNPCS),0) STNPCS FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE LOTSNO = L.SNO) "
            strsql += vbCrLf + "AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') "
            strsql += vbCrLf + "UNION ALL"
            strsql += vbCrLf + "SELECT ISNULL(SUM(STNPCS),0) STNPCS FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMNONTAG WHERE LOTSNO = L.SNO) "
            strsql += vbCrLf + "AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')"
            strsql += vbCrLf + ")X"
            strsql += vbCrLf + "),0) AS CDIAPCS"

            strsql += vbCrLf + ",ISNULL("
            strsql += vbCrLf + "(SELECT ISNULL(SUM(STNWT),0) STNWT FROM"
            strsql += vbCrLf + "("
            strsql += vbCrLf + "SELECT ISNULL(SUM(STNWT),0) STNWT FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE LOTSNO = L.SNO) "
            strsql += vbCrLf + "AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') "
            strsql += vbCrLf + "UNION ALL"
            strsql += vbCrLf + "SELECT ISNULL(SUM(STNWT),0) STNWT FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMNONTAG WHERE LOTSNO = L.SNO) "
            strsql += vbCrLf + "AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D') "
            strsql += vbCrLf + ")X"
            strsql += vbCrLf + "),0) AS CDIAWT"

            strsql += vbCrLf + ",ISNULL("
            strsql += vbCrLf + "(SELECT SUM(STNPCS) FROM "
            strsql += vbCrLf + "("
            strsql += vbCrLf + "SELECT ISNULL(SUM(STNPCS),0) STNPCS FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE LOTSNO = L.SNO) "
            strsql += vbCrLf + "AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')"
            strsql += vbCrLf + "UNION ALL"
            strsql += vbCrLf + "SELECT ISNULL(SUM(STNPCS),0) STNPCS FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMNONTAG WHERE LOTSNO = L.SNO) "
            strsql += vbCrLf + "AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')"
            strsql += vbCrLf + ")X "
            strsql += vbCrLf + "),0) AS CSTNPCS"

            strsql += vbCrLf + ",ISNULL("
            strsql += vbCrLf + "(SELECT SUM(STNWT) FROM "
            strsql += vbCrLf + "("
            strsql += vbCrLf + "SELECT ISNULL(SUM(STNWT),0) STNWT FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE LOTSNO = L.SNO) "
            strsql += vbCrLf + "AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')"
            strsql += vbCrLf + "UNION ALL "
            strsql += vbCrLf + "SELECT ISNULL(SUM(STNWT),0) STNWT FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMNONTAG WHERE LOTSNO = L.SNO) "
            strsql += vbCrLf + "AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')"
            strsql += vbCrLf + ")X"
            strsql += vbCrLf + "),0) AS CSTNWT"

            strsql += vbCrLf + " ,ISNULL((SELECT SUM(SALVALUE) FROM " & cnAdminDb & "..ITEMTAG WHERE LOTSNO = L.SNO AND SALEMODE IN('F','R')),0) AS RSALVALUE"
            strsql += vbCrLf + " ,ISNULL((SELECT SUM(SALVALUE) FROM " & cnAdminDb & "..ITEMTAG WHERE LOTSNO = L.SNO AND SALEMODE IN('W')),0) AS WSALVALUE"
            strsql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = L.ITEMID))AS METAL,OPENTIME,CLOSETIME"
            strsql += vbCrLf + " ,USERID,ISNULL(CANCEL,'') CANCEL "
            strsql += vbCrLf + " ,(SELECT TOP 1 TAGNO  FROM " & cnAdminDb & "..ITEMTAG WHERE LOTSNO =L.SNO "
            strsql += vbCrLf + " AND ACTUALRECDATE =(SELECT MIN(ACTUALRECDATE) FROM " & cnAdminDb & "..ITEMTAG WHERE LOTSNO =L.SNO)"
            strsql += vbCrLf + " AND UPTIME =(SELECT MIN(UPTIME) FROM " & cnAdminDb & "..ITEMTAG WHERE LOTSNO =L.SNO "
            strsql += vbCrLf + " AND ACTUALRECDATE =(SELECT MIN(ACTUALRECDATE) FROM " & cnAdminDb & "..ITEMTAG WHERE LOTSNO =L.SNO))) FROMTAGNO "
            strsql += vbCrLf + " ,(SELECT TOP 1 TAGNO  FROM " & cnAdminDb & "..ITEMTAG WHERE LOTSNO =L.SNO "
            strsql += vbCrLf + " AND ACTUALRECDATE =(SELECT MAX(ACTUALRECDATE) FROM " & cnAdminDb & "..ITEMTAG WHERE LOTSNO =L.SNO) "
            strsql += vbCrLf + " AND UPTIME =(SELECT MAX(UPTIME) FROM " & cnAdminDb & "..ITEMTAG WHERE LOTSNO =L.SNO "
            strsql += vbCrLf + " AND ACTUALRECDATE =(SELECT MAX(ACTUALRECDATE) FROM " & cnAdminDb & "..ITEMTAG WHERE LOTSNO =L.SNO))) TOTAGNO "
            strsql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ITEMLOT L"
            strsql += vbCrLf + " )X"
        Else
            strsql = " SELECT CONVERT(VARCHAR,LOTDATE,103)LOTDATE,LOTNO,LOTNO AS LOTNO1"
            If chkOrderByItemId.Checked Then
                strsql += vbCrLf + "     ,(SELECT '['+REPLICATE(' ',5-LEN(L.ITEMID)) + CONVERT(VARCHAR,L.ITEMID) + '] ' + ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = L.ITEMID)AS ITEM"
            Else
                strsql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = L.ITEMID)AS ITEM"
            End If
            strsql += vbCrLf + " ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = L.ITEMCTRID)AS COUNTER"
            strsql += vbCrLf + " ,PCS,GRSWT,NETWT,DIAPCS,DIAWT,STNPCS,STNWT"
            strsql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = L.DESIGNERID)AS DESIGNER"
            strsql += vbCrLf + " ,TABLECODE"
            strsql += vbCrLf + " ,NARRATION"
            If ragshow = True Then strsql += vbCrLf + " ,(SELECT CAPTION FROM " & cnAdminDb & "..RANGEMAST WHERE SNO=L.RANGESNO)RANGE"
            strsql += vbCrLf + " ,LOTDATE LDATE,1 RESULT,' 'COLHEAD"
            strsql += vbCrLf + " ,(SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE METALID = (SELECT METALID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = L.ITEMID))AS METAL,CONVERT(VARCHAR,OPENTIME,103)OPENDATE,CONVERT(VARCHAR(5),OPENTIME,114)OPENTIME,CONVERT(VARCHAR,CLOSETIME,103)CLOSEDATE,CONVERT(VARCHAR(5),CLOSETIME,114)CLOSETIME "
            strsql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "LOTVIEW"
            strsql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "ITEMLOT L"
        End If
        cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        If rbtDetailed.Checked And rbtGroupDesigner.Checked Then
            ''INSERTING TITLE
            strsql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "LOTVIEW(ITEM,DESIGNER,RESULT,COLHEAD)"
            strsql += vbCrLf + " SELECT DISTINCT DESIGNER,DESIGNER,0 RESULT,'T' COLHEAD FROM TEMPTABLEDB..TEMP" & systemId & "LOTVIEW"
            cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()

            ''INSERTING SUB TOTAL
            strsql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "LOTVIEW(ITEM,DESIGNER,RESULT,COLHEAD"
            strsql += vbCrLf + " ,PCS,GRSWT,NETWT,DIAPCS,DIAWT,STNPCS,STNWT,CPCS,CGRSWT,CNETWT,CDIAPCS,CDIAWT,CSTNPCS,CSTNWT,RSALVALUE,WSALVALUE,BPCS,BGRSWT,BNETWT,BDIAPCS,BDIAWT,BSTNPCS,BSTNWT"
            strsql += vbCrLf + " )"
            strsql += vbCrLf + " SELECT DISTINCT 'SUB TOTAL',DESIGNER,2 RESULT,'S' COLHEAD"
            strsql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(DIAPCS),SUM(DIAWT),SUM(STNPCS),SUM(STNWT)"
            strsql += vbCrLf + " ,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,SUM(CDIAPCS),SUM(CDIAWT),SUM(CSTNPCS),SUM(CSTNWT),SUM(RSALVALUE),SUM(WSALVALUE)"
            strsql += vbCrLf + " ,SUM(BPCS)BPCS,SUM(BGRSWT)BGRSWT,SUM(BNETWT)BNETWT,SUM(BDIAPCS),SUM(BDIAWT),SUM(BSTNPCS),SUM(BSTNWT)"
            strsql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "LOTVIEW GROUP BY DESIGNER"
            cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        If rbtDetailed.Checked And rbtGroupLot.Checked Then
            ''INSERTING SUB TOTAL
            strsql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "LOTVIEW(ITEM,LOTNO1,RESULT,COLHEAD"
            strsql += vbCrLf + " ,PCS,GRSWT,NETWT,DIAPCS,DIAWT,STNPCS,STNWT,CPCS,CGRSWT,CNETWT,CDIAPCS,CDIAWT,CSTNPCS,CSTNWT,RSALVALUE,WSALVALUE,BPCS,BGRSWT,BNETWT,BDIAPCS,BDIAWT,BSTNPCS,BSTNWT"
            strsql += vbCrLf + " )"
            strsql += vbCrLf + " SELECT DISTINCT 'SUB TOTAL',LOTNO1,2 RESULT,'S' COLHEAD"
            strsql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(DIAPCS),SUM(DIAWT),SUM(STNPCS),SUM(STNWT)"
            strsql += vbCrLf + " ,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,SUM(CDIAPCS),SUM(CDIAWT),SUM(CSTNPCS),SUM(CSTNWT),SUM(RSALVALUE),SUM(WSALVALUE)"
            strsql += vbCrLf + " ,SUM(BPCS)BPCS,SUM(BGRSWT)BGRSWT,SUM(BNETWT)BNETWT,SUM(BDIAPCS),SUM(BDIAWT),SUM(BSTNPCS),SUM(BSTNWT)"
            strsql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "LOTVIEW GROUP BY LOTNO1"
            cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        ''INSERTING GRAND TOTAL
        strsql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "LOTVIEW(ITEM,DESIGNER,METAL,RESULT,COLHEAD"
        strsql += vbCrLf + " ,PCS,GRSWT,NETWT,DIAPCS,DIAWT,STNPCS,STNWT"
        If rbtDetailed.Checked Then
            strsql += vbCrLf + " ,CPCS,CGRSWT,CNETWT,CDIAPCS,CDIAWT,CSTNPCS,CSTNWT,RSALVALUE,WSALVALUE,BPCS,BGRSWT,BNETWT,BDIAPCS,BDIAWT,BSTNPCS,BSTNWT"
            If rbtGroupLot.Checked Then
                strsql += vbCrLf + " ,LOTNO1"
            End If
        End If
        strsql += vbCrLf + " )"
        strsql += vbCrLf + " SELECT DISTINCT 'GRAND TOTAL','ZZZZZZ','ZZZZZZ',3 RESULT,'G' COLHEAD"
        strsql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(DIAPCS),SUM(DIAWT),SUM(STNPCS),SUM(STNWT)"
        If rbtDetailed.Checked Then
            strsql += vbCrLf + " ,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,SUM(CDIAPCS)CDIAPCS,SUM(CDIAWT)CDIAWT,SUM(CSTNPCS)CSTNPCS,SUM(CSTNWT)CSTNWT,SUM(RSALVALUE),SUM(WSALVALUE)"
            strsql += vbCrLf + " ,SUM(BPCS)BPCS,SUM(BGRSWT)BGRSWT,SUM(BNETWT)BNETWT,SUM(BDIAPCS)BDIAPCS,SUM(BDIAWT)BDIAWT,SUM(BSTNPCS)BSTNPCS,SUM(BSTNWT)BSTNWT"
            If rbtGroupLot.Checked Then
                strsql += vbCrLf + " ,'99999' AS LOTNO1"
            End If
        End If
        strsql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "LOTVIEW WHERE RESULT = 1"
        cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strsql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "LOTVIEW(ITEM,COLHEAD,RESULT,DESIGNER,LOTNO1)SELECT 'SUMMARY','G',4,'ZZZZZZ','999999'"
        cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        If rbtDetailed.Checked Then
            ''INSERTING METAL WISE TOTAL
            strsql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "LOTVIEW(ITEM,METAL,DESIGNER,LOTNO1,RESULT,COLHEAD"
            strsql += vbCrLf + " ,PCS,GRSWT,NETWT,DIAPCS,DIAWT,STNPCS,STNWT,CPCS,CGRSWT,CNETWT,CDIAPCS,CDIAWT,CSTNPCS,CSTNWT,RSALVALUE,WSALVALUE,BPCS,BGRSWT,BNETWT,BDIAPCS,BDIAWT,BSTNPCS,BSTNWT"
            strsql += vbCrLf + " )"
            strsql += vbCrLf + " SELECT DISTINCT METAL ,METAL,'ZZZZZZ','9999999',5 RESULT,'G' COLHEAD"
            strsql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(DIAPCS),SUM(DIAWT),SUM(STNPCS),SUM(STNWT)"
            strsql += vbCrLf + " ,SUM(CPCS)CPCS,SUM(CGRSWT)CGRSWT,SUM(CNETWT)CNETWT,SUM(CDIAPCS),SUM(CDIAWT),SUM(CSTNPCS),SUM(CSTNWT),SUM(RSALVALUE),SUM(WSALVALUE)"
            strsql += vbCrLf + " ,SUM(BPCS)BPCS,SUM(BGRSWT)BGRSWT,SUM(BNETWT)BNETWT,SUM(BDIAPCS),SUM(BDIAWT),SUM(BSTNPCS),SUM(BSTNWT)"
            strsql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "LOTVIEW WHERE RESULT = 1 GROUP BY METAL"
            cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        Else
            ''INSERTING METAL WISE TOTAL
            strsql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "LOTVIEW(ITEM,METAL,DESIGNER,RESULT,COLHEAD"
            strsql += vbCrLf + " ,PCS,GRSWT,NETWT,DIAPCS,DIAWT,STNPCS,STNWT"
            strsql += vbCrLf + " )"
            strsql += vbCrLf + " SELECT DISTINCT METAL ,METAL,'ZZZZZZ',5 RESULT,'G' COLHEAD"
            strsql += vbCrLf + " ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(DIAPCS),SUM(DIAWT),SUM(STNPCS),SUM(STNWT)"
            strsql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "LOTVIEW WHERE RESULT = 1 GROUP BY METAL"
            cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        If rbtDetailed.Checked Then
            strsql = vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "LOTVIEW SET"
            strsql += vbCrLf + " PCS = CASE WHEN PCS <> 0 THEN PCS ELSE NULL END"
            strsql += vbCrLf + " ,GRSWT = CASE WHEN GRSWT <> 0 THEN GRSWT ELSE NULL END"
            strsql += vbCrLf + " ,NETWT = CASE WHEN NETWT <> 0 THEN NETWT ELSE NULL END"
            strsql += vbCrLf + " ,DIAPCS = CASE WHEN DIAPCS <> 0 THEN DIAPCS ELSE NULL END"
            strsql += vbCrLf + " ,DIAWT = CASE WHEN DIAWT <> 0 THEN DIAWT ELSE NULL END"
            strsql += vbCrLf + " ,STNPCS = CASE WHEN STNPCS <> 0 THEN STNPCS ELSE NULL END"
            strsql += vbCrLf + " ,STNWT = CASE WHEN STNWT <> 0 THEN STNWT ELSE NULL END"
            strsql += vbCrLf + " ,CPCS = CASE WHEN CPCS <> 0 THEN CPCS ELSE NULL END"
            strsql += vbCrLf + " ,CGRSWT = CASE WHEN CGRSWT <> 0 THEN CGRSWT ELSE NULL END"
            strsql += vbCrLf + " ,CNETWT = CASE WHEN CNETWT <> 0 THEN CNETWT ELSE NULL END"
            strsql += vbCrLf + " ,CDIAPCS = CASE WHEN CDIAPCS <> 0 THEN CDIAPCS ELSE NULL END"
            strsql += vbCrLf + " ,CDIAWT = CASE WHEN CDIAWT <> 0 THEN CDIAWT ELSE NULL END"
            strsql += vbCrLf + " ,CSTNPCS = CASE WHEN CSTNPCS <> 0 THEN CSTNPCS ELSE NULL END"
            strsql += vbCrLf + " ,CSTNWT = CASE WHEN CSTNWT <> 0 THEN CSTNWT ELSE NULL END"
            strsql += vbCrLf + " ,BPCS = CASE WHEN BPCS <> 0 THEN BPCS ELSE NULL END"
            strsql += vbCrLf + " ,BGRSWT = CASE WHEN BGRSWT <> 0 THEN BGRSWT ELSE NULL END"
            strsql += vbCrLf + " ,BNETWT = CASE WHEN BNETWT <> 0 THEN BNETWT ELSE NULL END"
            strsql += vbCrLf + " ,BDIAPCS = CASE WHEN BDIAPCS <> 0 THEN BDIAPCS ELSE NULL END"
            strsql += vbCrLf + " ,BDIAWT = CASE WHEN BDIAWT <> 0 THEN BDIAWT ELSE NULL END"
            strsql += vbCrLf + " ,BSTNPCS = CASE WHEN BSTNPCS <> 0 THEN BSTNPCS ELSE NULL END"
            strsql += vbCrLf + " ,BSTNWT = CASE WHEN BSTNWT <> 0 THEN BSTNWT ELSE NULL END"
            strsql += vbCrLf + " ,RSALVALUE = CASE WHEN RSALVALUE <> 0 THEN RSALVALUE ELSE NULL END"
            strsql += vbCrLf + " ,WSALVALUE = CASE WHEN WSALVALUE <> 0 THEN WSALVALUE ELSE NULL END"
            'strsql += vbCrLf + " ,DESIGNER = CASE WHEN DESIGNER = 'ZZZZZZ' THEN '' /*ELSE ''*/ END"
            cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        Else
            strsql = vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "LOTVIEW SET"
            strsql += vbCrLf + " PCS = CASE WHEN PCS <> 0 THEN PCS ELSE NULL END"
            strsql += vbCrLf + " ,GRSWT = CASE WHEN GRSWT <> 0 THEN GRSWT ELSE NULL END"
            strsql += vbCrLf + " ,NETWT = CASE WHEN NETWT <> 0 THEN NETWT ELSE NULL END"
            strsql += vbCrLf + " ,DIAPCS = CASE WHEN DIAPCS <> 0 THEN DIAPCS ELSE NULL END"
            strsql += vbCrLf + " ,DIAWT = CASE WHEN DIAWT <> 0 THEN DIAWT ELSE NULL END"
            strsql += vbCrLf + " ,STNPCS = CASE WHEN STNPCS <> 0 THEN STNPCS ELSE NULL END"
            strsql += vbCrLf + " ,STNWT = CASE WHEN STNWT <> 0 THEN STNWT ELSE NULL END"
            strsql += vbCrLf + " ,DESIGNER = CASE WHEN DESIGNER = 'ZZZZZZ' THEN '' ELSE '' END"
            cmd = New OleDbCommand(strsql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        Prop_Sets()
        Dim dtGrid As New DataTable
        If rbtDetailed.Checked And rbtGroupNone.Checked Then
            strsql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "LOTVIEW ORDER BY RESULT" & IIf(chkOrderByLotno.Checked = True, ",LDATE,LOTNO", ",ITEM,LDATE")
        ElseIf rbtDetailed.Checked And rbtGroupDesigner.Checked Then
            strsql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "LOTVIEW"
            strsql += vbCrLf + " ORDER BY DESIGNER,RESULT" & IIf(chkOrderByLotno.Checked = True, ",LDATE,LOTNO", ",ITEM,LDATE")
        ElseIf rbtDetailed.Checked And rbtGroupLot.Checked Then
            strsql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "LOTVIEW"
            strsql += vbCrLf + " ORDER BY LOTNO1,RESULT" & IIf(chkOrderByLotno.Checked = True, ",LDATE", ",ITEM,LDATE")
        ElseIf rbtSummary.Checked Then
            strsql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "LOTVIEW ORDER BY RESULT" & IIf(chkOrderByLotno.Checked = True, ",LDATE,LOTNO", ",ITEM,LDATE")
        End If
        da = New OleDbDataAdapter(strsql, cn)
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            Exit Sub
        End If
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        ' objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Size = New Size(778, 550)
        objGridShower.Text = "LOT VIEW "
        Dim tit As String = ""
        Dim _chkCompany As String = GetChecked_CheckedList(chkLstCompany, False)
        tit = _chkCompany
        tit += vbCrLf + "LOT VIEW "
        'FOR " + IIf(chkCategory <> "", chkCategory.Replace("'", ""), " ALL CATEGORY") + vbCrLf
        tit += "DATE FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        If chkMetalSelectAll.Checked = False Then
            tit += vbCrLf + GetChecked_CheckedList(chkLstMetal, False)
        End If
        If chkCostCentreSelectAll.Checked = False Then
            tit += vbCrLf + GetChecked_CheckedList(chkLstCostCentre, False)
        End If
        AddHandler objGridShower.gridView.Scroll, AddressOf gridView_Scroll
        AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
        With objGridShower
            .lblTitle.Text = tit
            .StartPosition = FormStartPosition.CenterScreen
            '.MaximumSize = New Size(900, 700)
            .dsGrid.DataSetName = objGridShower.Name
            .dsGrid.Tables.Add(dtGrid)
            .gridView.DataSource = objGridShower.dsGrid.Tables(0)
            .pnlFooter.Visible = False
            .FormReLocation = True
            .FormReSize = True
            .WindowState = FormWindowState.Maximized
            .Show()
        End With
        If rbtDetailed.Checked Then
            DataGridView_Detailed_None(objGridShower.gridView)
            FillGridGroupStyle_KeyNoWise(objGridShower.gridView, "ITEM") ' FillGridGroupStyle(objGridShower.gridView)
            'If rbtGroupDesigner.Checked Then
            objGridShower.FormReSize = True
            objGridShower.gridViewHeader.Visible = True
            GridViewHeaderCreator(objGridShower.gridViewHeader)
            For i As Integer = 0 To objGridShower.gridView.Rows.Count - 1
                If objGridShower.gridView.Rows(i).Cells("CANCEL").Value.ToString = "Y" Then
                    objGridShower.gridView.Rows(i).DefaultCellStyle.ForeColor = Color.Red
                    ' objGridShower.gridView.Rows(i).DefaultCellStyle.Font = New Font("verdana", 8.25, Font.Style.Bold)
                End If
            Next
        Else
            DataGridView_Summary_None(objGridShower.gridView)
            FillGridGroupStyle_KeyNoWise(objGridShower.gridView, "ITEM") ' FillGridGroupStyle(objGridShower.gridView)
            objGridShower.gridViewHeader.Visible = False
        End If
        With objGridShower
            .gridViewHeader.ColumnHeadersDefaultCellStyle = objGridShower.gridView.ColumnHeadersDefaultCellStyle
            .gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
            .gridView.Invalidate()
            For Each dgvCol As DataGridViewColumn In .gridView.Columns
                dgvCol.Width = dgvCol.Width
            Next
            .gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End With
    End Sub
    Private Sub GridViewHeaderCreator(ByVal gridviewHead As DataGridView)
        Dim dtHead As New DataTable
        strsql = "SELECT ''[LOTDATE~LOTNO~ITEM~COUNTER],''[PCS~GRSWT~NETWT~DIAPCS~DIAWT~STNPCS~STNWT],''[CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~RSALVALUE~WSALVALUE],''[BPCS~BGRSWT~BNETWT~BDIAPCS~BDIAWT~BSTNPCS~BSTNWT],''[DESIGNER~TABLECODE~NARRATION~OPENDATE~OPENTIME~CLOSEDATE~CLOSETIME~USERNAME~FROM_TAGNO~TO_TAGNO],''SCROLL"
        da = New OleDbDataAdapter(strsql, cn)
        da.Fill(dtHead)
        gridviewHead.DataSource = dtHead
        gridviewHead.Columns("LOTDATE~LOTNO~ITEM~COUNTER").HeaderText = ""
        gridviewHead.Columns("PCS~GRSWT~NETWT~DIAPCS~DIAWT~STNPCS~STNWT").HeaderText = "LOT"
        gridviewHead.Columns("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~RSALVALUE~WSALVALUE").HeaderText = "STOCK"
        gridviewHead.Columns("BPCS~BGRSWT~BNETWT~BDIAPCS~BDIAWT~BSTNPCS~BSTNWT").HeaderText = "BALANCE"
        gridviewHead.Columns("DESIGNER~TABLECODE~NARRATION~OPENDATE~OPENTIME~CLOSEDATE~CLOSETIME~USERNAME~FROM_TAGNO~TO_TAGNO").HeaderText = ""
        'gridviewHead.Columns("DESIGNER~TABLECODE~NARRATION~OPENDATE~OPENTIME~CLOSEDATE~CLOSETIME~USERNAME").HeaderText = ""

        gridviewHead.Columns("SCROLL").HeaderText = ""
        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        SetGridHeadColWid(gridviewHead)
    End Sub
    Private Sub DataGridView_Summary_None(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            For Each dgvRow As DataGridViewRow In dgv.Rows
                If Val(dgvRow.Cells("RESULT").Value.ToString) <> 1 Then
                    dgvRow.Cells("DESIGNER").Value = DBNull.Value
                End If
            Next
            .Columns("LOTDATE").Width = 80
            .Columns("LOTNO").Width = 60
            .Columns("ITEM").Width = 120
            .Columns("PCS").Width = 60
            .Columns("GRSWT").Width = 80
            .Columns("NETWT").Width = 80
            .Columns("DIAPCS").Width = 60
            .Columns("DIAWT").Width = 80
            .Columns("STNPCS").Width = 60
            .Columns("STNWT").Width = 80
            .Columns("DESIGNER").Width = 120
            .Columns("TABLECODE").Width = 70
            .Columns("NARRATION").Width = 120
            '.Columns("OPENDATE").Width = 120
            '.Columns("OPENTIME").Width = 120
            '.Columns("CLOSEDATE").Width = 120
            '.Columns("CLOSETIME").Width = 120

            .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            .Columns("PCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("GRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("NETWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("DIAPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("DIAWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("STNPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("STNWT").DefaultCellStyle.BackColor = Color.AliceBlue

            .Columns("TABLECODE").HeaderText = "TABLE CODE"
            .Columns("LDATE").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("DESIGNER").Visible = Not rbtGroupDesigner.Checked
            .Columns("KEYNO").Visible = False
            .Columns("METAL").Visible = False
            .Columns("LOTNO1").Visible = False
            BrighttechPack.GlobalMethods.FormatGridColumns(dgv, True)
            'FormatGridColumns(dgv, False)
        End With
    End Sub
    Private Sub DataGridView_Detailed_None(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            For Each dgvRow As DataGridViewRow In dgv.Rows
                If Val(dgvRow.Cells("RESULT").Value.ToString) <> 1 Then
                    dgvRow.Cells("DESIGNER").Value = DBNull.Value
                End If
            Next
            If ChkSalVal.Checked = False And .Columns.Contains("RSALVALUE") Then .Columns("RSALVALUE").Visible = False
            If ChkSalVal.Checked = False And .Columns.Contains("WSALVALUE") Then .Columns("WSALVALUE").Visible = False
            .Columns("LOTDATE").Width = 80
            .Columns("LOTNO").Width = 60
            .Columns("ITEM").Width = IIf(rbtGroupDesigner.Checked, 150, 120)
            .Columns("PCS").Width = 60
            .Columns("GRSWT").Width = 80
            .Columns("NETWT").Width = 80
            .Columns("DIAPCS").Width = 60
            .Columns("DIAWT").Width = 80
            .Columns("STNPCS").Width = 60
            .Columns("STNWT").Width = 80
            .Columns("CPCS").Width = 60
            .Columns("CGRSWT").Width = 80
            .Columns("CNETWT").Width = 80
            .Columns("CDIAPCS").Width = 60
            .Columns("CDIAWT").Width = 80
            .Columns("CSTNPCS").Width = 60
            .Columns("CSTNWT").Width = 80
            .Columns("BPCS").Width = 60
            .Columns("BGRSWT").Width = 80
            .Columns("BNETWT").Width = 80
            .Columns("BDIAPCS").Width = 60
            .Columns("BDIAWT").Width = 80
            .Columns("BSTNPCS").Width = 60
            .Columns("BSTNWT").Width = 80
            .Columns("DESIGNER").Width = 120
            .Columns("TABLECODE").Width = 70
            .Columns("NARRATION").Width = 120
            .Columns("FROM_TAGNO").Width = 120
            .Columns("TO_TAGNO").Width = 120

            .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            .Columns("CDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            .Columns("BDIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)

            .Columns("CPCS").HeaderText = "PCS"
            .Columns("CGRSWT").HeaderText = "GRSWT"
            .Columns("CNETWT").HeaderText = "NETWT"
            .Columns("CDIAPCS").HeaderText = "DIAPCS"
            .Columns("CDIAWT").HeaderText = "DIAWT"
            .Columns("CSTNPCS").HeaderText = "STNPCS"
            .Columns("CSTNWT").HeaderText = "STNWT"
            .Columns("BPCS").HeaderText = "PCS"
            .Columns("BGRSWT").HeaderText = "GRSWT"
            .Columns("BNETWT").HeaderText = "NETWT"
            .Columns("BDIAPCS").HeaderText = "DIAPCS"
            .Columns("BDIAWT").HeaderText = "DIAWT"
            .Columns("BSTNPCS").HeaderText = "STNPCS"
            .Columns("BSTNWT").HeaderText = "STNWT"

            .Columns("PCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("GRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("NETWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("DIAPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("DIAWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("STNPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("STNWT").DefaultCellStyle.BackColor = Color.AliceBlue

            .Columns("BPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("BGRSWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("BNETWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("BDIAPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("BDIAWT").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("BSTNPCS").DefaultCellStyle.BackColor = Color.AliceBlue
            .Columns("BSTNWT").DefaultCellStyle.BackColor = Color.AliceBlue


            .Columns("TABLECODE").HeaderText = "TABLE CODE"
            .Columns("LDATE").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("DESIGNER").Visible = Not rbtGroupDesigner.Checked
            .Columns("KEYNO").Visible = False
            .Columns("METAL").Visible = False
            .Columns("LOTNO1").Visible = False
            .Columns("CANCEL").Visible = False
            BrighttechPack.GlobalMethods.FormatGridColumns(dgv, True)
            'FormatGridColumns(dgv, False)
        End With



    End Sub
    'Private Sub DataGridViewRow_KeyPress(ByVal dgv As DataGridView, ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles DataGridViewRow.KeyPress

    '    If UCase(e.KeyChar) = "D" Then

    '        For Each dgvRow As DataGridViewRow In dgv.Rows
    '            If Val(dgvRow.Cells("RESULT").Value.ToString) <> 1 Then
    '                dgvRow.Cells("lotno").Value = DBNull.Value
    '            End If
    '        Next
    '        If IO.File.Exists(Application.StartupPath & "\BillPrint.exe") Then
    '            Dim write As IO.StreamWriter
    '            Dim memfile As String = "\BillPrint" & ".mem"
    '            write = IO.File.CreateText(Application.StartupPath & memfile)
    '            'write = IO.File.CreateText(Application.StartupPath & "\BillPrint.mem")
    '            write.WriteLine(LSet("TYPE", 15) & ":" & "LOT" & "")
    '             write.WriteLine(LSet("BATCHNO", 15) & ":" & dgvRow.Cells("lotno").Value.ToString)
    '            '  write.WriteLine(LSet("TRANDATE", 15) & ":" & dtpBillDate.Value.ToString("yyyy-MM-dd"))
    '            write.WriteLine(LSet("DUPLICATE", 15) & ":Y")
    '            write.Flush()
    '            write.Close()
    '            If EXE_WITH_PARAM = False Then
    '                System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe")
    '            Else
    '                'System.Diagnostics.Process.Start(Application.StartupPath & "\BillPrint.exe",
    '                'LSet("TYPE", 15) & ":" & Type & ";" &
    '                'LSet("BATCHNO", 15) & ":" & .Cells("BATCHNO").Value.ToString & ";" &
    '                'LSet("TRANDATE", 15) & ":" & dtpBillDate.Value.ToString("yyyy-MM-dd") & ";" &
    '                'LSet("DUPLICATE", 15) & ":Y")
    '            End If
    '        Else
    '            MsgBox("Billprint exe not found", MsgBoxStyle.Information)
    '        End If
    '    End If
    'End Sub

    Private Sub SetGridHeadColWid(ByVal gridViewHeader As DataGridView)
        Dim f As frmGridDispDia
        f = objGPack.GetParentControl(gridViewHeader)
        If Not f.gridViewHeader.Visible Then Exit Sub
        If f.gridViewHeader Is Nothing Then Exit Sub
        If Not f.gridView.ColumnCount > 0 Then Exit Sub
        If Not f.gridViewHeader.ColumnCount > 0 Then Exit Sub
        With f.gridViewHeader

            .Columns("LOTDATE~LOTNO~ITEM~COUNTER").Width = f.gridView.Columns("LOTDATE").Width + f.gridView.Columns("LOTNO").Width + f.gridView.Columns("ITEM").Width + f.gridView.Columns("COUNTER").Width
            .Columns("PCS~GRSWT~NETWT~DIAPCS~DIAWT~STNPCS~STNWT").Width = f.gridView.Columns("PCS").Width + f.gridView.Columns("GRSWT").Width + f.gridView.Columns("NETWT").Width + f.gridView.Columns("DIAPCS").Width + f.gridView.Columns("DIAWT").Width + f.gridView.Columns("STNPCS").Width + f.gridView.Columns("STNWT").Width
            .Columns("CPCS~CGRSWT~CNETWT~CDIAPCS~CDIAWT~CSTNPCS~CSTNWT~RSALVALUE~WSALVALUE").Width = f.gridView.Columns("CPCS").Width + f.gridView.Columns("CGRSWT").Width + f.gridView.Columns("CNETWT").Width + f.gridView.Columns("CDIAPCS").Width + f.gridView.Columns("CDIAWT").Width + f.gridView.Columns("CSTNPCS").Width + f.gridView.Columns("CSTNWT").Width _
             + IIf(ChkSalVal.Checked, f.gridView.Columns("RSALVALUE").Width, 0) + IIf(ChkSalVal.Checked, f.gridView.Columns("WSALVALUE").Width, 0)
            .Columns("BPCS~BGRSWT~BNETWT~BDIAPCS~BDIAWT~BSTNPCS~BSTNWT").Width = f.gridView.Columns("BPCS").Width + f.gridView.Columns("BGRSWT").Width + f.gridView.Columns("BNETWT").Width + f.gridView.Columns("BDIAPCS").Width + f.gridView.Columns("BDIAWT").Width + f.gridView.Columns("BSTNPCS").Width + f.gridView.Columns("BSTNWT").Width

            .Columns("DESIGNER~TABLECODE~NARRATION~OPENDATE~OPENTIME~CLOSEDATE~CLOSETIME~USERNAME~FROM_TAGNO~TO_TAGNO").Width = f.gridView.Columns("DESIGNER").Width _
                + f.gridView.Columns("TABLECODE").Width + f.gridView.Columns("NARRATION").Width + f.gridView.Columns("OPENDATE").Width + f.gridView.Columns("OPENTIME").Width _
                + f.gridView.Columns("CLOSEDATE").Width + f.gridView.Columns("CLOSETIME").Width + f.gridView.Columns("USERNAME").Width _
                + f.gridView.Columns("FROM_TAGNO").Width + f.gridView.Columns("TO_TAGNO").Width

            '.Columns("DESIGNER~TABLECODE~NARRATION~OPENDATE~OPENTIME~CLOSEDATE~CLOSETIME").Width = IIf(f.gridView.Columns("DESIGNER").Visible, f.gridView.Columns("DESIGNER").Width, 0) _
            '+ f.gridView.Columns("TABLECODE").Width + f.gridView.Columns("NARRATION").Width

            '.Columns("DESIGNER~TABLECODE~NARRATION").Width = IIf(f.gridView.Columns("DESIGNER").Visible, f.gridView.Columns("DESIGNER").Width, 0)
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
    'Private Sub gridview_keypress(ByVal sender As Object)
    'Private Sub gridview_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles gridview.KeyPress

    'End Sub
    Private Sub gridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs)
        Try
            Dim f As frmGridDispDia
            f = objGPack.GetParentControl(CType(sender, DataGridView))
            If Not f.gridViewHeader.Visible Then Exit Sub
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                f.gridViewHeader.HorizontalScrollingOffset = e.NewValue
                f.gridViewHeader.Columns("SCROLL").Visible = CType(f.gridView.Controls(1), VScrollBar).Visible
                f.gridViewHeader.Columns("SCROLL").Width = CType(f.gridView.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

    Private Sub LoadCategory()
        Dim chkMetalNames As String = GetChecked_CheckedList(chkLstMetal)
        chkLstCategory.Items.Clear()
        If chkMetalNames <> "" Then
            strsql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
            strsql += " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalNames & "))"
            strsql += " ORDER BY CATNAME"
            FillCheckedListBox(strsql, chkLstCategory, , chkCategorySelectAll.Checked)
        End If
    End Sub

    Function funcLoadItemName() As Integer
        cmbItemName.Items.Clear()
        cmbItemName.Items.Add("ALL")
        strsql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ACTIVE,'') = 'Y'"
        strsql += " AND ISNULL(STOCKREPORT,'') = 'Y'"
        If chkMetalSelectAll.Checked = False And chkLstMetal.CheckedItems.Count > 0 Then
            strsql += " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & GetChecked_CheckedList(chkLstMetal) & "))"
            If chkCategorySelectAll.Checked = False And chkLstCategory.CheckedItems.Count > 0 Then
                strsql += " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetChecked_CheckedList(chkLstCategory) & "))"
            End If
        ElseIf chkCategorySelectAll.Checked = False And chkLstCategory.CheckedItems.Count > 0 Then
            strsql += " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & GetChecked_CheckedList(chkLstCategory) & "))"
        End If

        strsql += "  ORDER BY ITEMID"
        objGPack.FillCombo(strsql, cmbItemName, False)
        cmbItemName.Text = "ALL"
    End Function

    Private Sub chkLstMetal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstMetal.LostFocus
        LoadCategory()
        funcLoadItemName()
    End Sub

    Private Sub chkCategorySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCategorySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCategory, chkCategorySelectAll.Checked)
    End Sub

    Private Sub chkLstCategory_LostFocus(sender As Object, e As EventArgs) Handles chkLstCategory.LostFocus
        funcLoadItemName()
    End Sub
    Private Sub rbtGroupDesigner_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtGroupDesigner.CheckedChanged
        If Not rbtDetailed.Checked Then
            rbtGroupNone.Checked = True
        End If
    End Sub

    Private Sub rbtDetailed_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtDetailed.CheckedChanged
        If Not rbtDetailed.Checked Then
            rbtGroupNone.Checked = True
            ChkSalVal.Checked = False
        End If
        ChkSalVal.Enabled = rbtDetailed.Checked
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmLotViewDetailed_Properties
        obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_chkMetalSelectAll = chkMetalSelectAll.Checked
        GetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal)
        obj.p_chkCategorySelectAll = chkCategorySelectAll.Checked
        GetChecked_CheckedList(chkLstCategory, obj.p_chkLstCategory)
        obj.p_chkDesignerSelectAll = chkDesignerSelectAll.Checked
        GetChecked_CheckedList(chkLstDesigner, obj.p_chkLstDesigner)
        obj.p_chkItemCounterSelectAll = chkItemCounterSelectAll.Checked
        GetChecked_CheckedList(chkLstItemCounter, obj.p_chkLstItemCounter)
        obj.p_rbtDetailed = rbtDetailed.Checked
        obj.p_rbtSummary = rbtSummary.Checked
        obj.p_rbtGroupNone = rbtGroupNone.Checked
        obj.p_rbtGroupDesigner = rbtGroupDesigner.Checked
        obj.p_chkOrderByItemId = chkOrderByItemId.Checked
        obj.p_chkSalVal = ChkSalVal.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmLotViewDetailed_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmLotViewDetailed_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmLotViewDetailed_Properties))
        chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, Nothing)
        chkMetalSelectAll.Checked = obj.p_chkMetalSelectAll
        SetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal, Nothing)
        chkCategorySelectAll.Checked = obj.p_chkCategorySelectAll
        SetChecked_CheckedList(chkLstCategory, obj.p_chkLstCategory, Nothing)
        chkDesignerSelectAll.Checked = obj.p_chkDesignerSelectAll
        SetChecked_CheckedList(chkLstDesigner, obj.p_chkLstDesigner, Nothing)
        chkItemCounterSelectAll.Checked = obj.p_chkItemCounterSelectAll
        SetChecked_CheckedList(chkLstItemCounter, obj.p_chkLstItemCounter, Nothing)
        rbtDetailed.Checked = obj.p_rbtDetailed
        rbtSummary.Checked = obj.p_rbtSummary
        rbtGroupNone.Checked = obj.p_rbtGroupNone
        rbtGroupDesigner.Checked = obj.p_rbtGroupDesigner
        chkOrderByItemId.Checked = obj.p_chkOrderByItemId
        ChkSalVal.Checked = obj.p_chkSalVal
    End Sub
    Private Sub chkMultiDesigner_CheckStateChanged(sender As Object, e As EventArgs) Handles chkMultiDesigner.CheckStateChanged
        If chkMultiDesigner.Checked Then
            chkLstDesigner.Visible = True
            cmbDesigner.Visible = False
            chkDesignerSelectAll.Enabled = True
            chkDesignerSelectAll.Checked = True
        Else
            chkLstDesigner.Visible = False
            cmbDesigner.Visible = True
            cmbDesigner.Text = ""
            chkDesignerSelectAll.Enabled = False
            chkDesignerSelectAll.Checked = False
        End If
    End Sub

    Private Sub chkCompanySelectAll_CheckedChanged(sender As Object, e As EventArgs) Handles chkCompanySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCompany, chkCompanySelectAll.Checked)
    End Sub
End Class

Public Class frmLotViewDetailed_Properties
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
    Private chkCategorySelectAll As Boolean = False
    Public Property p_chkCategorySelectAll() As Boolean
        Get
            Return chkCategorySelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCategorySelectAll = value
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
    Private rbtGroupNone As Boolean = True
    Public Property p_rbtGroupNone() As Boolean
        Get
            Return rbtGroupNone
        End Get
        Set(ByVal value As Boolean)
            rbtGroupNone = value
        End Set
    End Property
    Private rbtGroupDesigner As Boolean = False
    Public Property p_rbtGroupDesigner() As Boolean
        Get
            Return rbtGroupDesigner
        End Get
        Set(ByVal value As Boolean)
            rbtGroupDesigner = value
        End Set
    End Property
    Private chkOrderByItemId As Boolean = False
    Public Property p_chkOrderByItemId() As Boolean
        Get
            Return chkOrderByItemId
        End Get
        Set(ByVal value As Boolean)
            chkOrderByItemId = value
        End Set
    End Property
    Private chkSalVal As Boolean = False
    Public Property p_chkSalVal() As Boolean
        Get
            Return chkSalVal
        End Get
        Set(ByVal value As Boolean)
            chkSalVal = value
        End Set
    End Property
End Class