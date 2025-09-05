Imports System.Data.OleDb
Public Class frmTagItemsPurchaseReport_Old
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim objGridShower As frmGridDispDia
    Dim defaultPic As String = GetAdmindbSoftValue("PICPATH")
    Dim RPT_DISPSTKTYPE As Boolean = IIf(GetAdmindbSoftValue("RPT_DISPSTKTYPE", "N", ) = "Y", True, False)
    Dim STOCKVIEW_GRSWT_AS_DIAWT As Boolean = IIf(GetAdmindbSoftValue("GRSWT_AS_DIAWT", "N") = "Y", True, False)

    Private Sub frmTagItemsPurchaseReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmTagItemsPurchaseReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not defaultPic.EndsWith("\") Then defaultPic += "\"
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ACTIVE = 'Y' ORDER BY METALNAME"
        FillCheckedListBox(strSql, chkLstMetal)
        If _IsCostCentre Then
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
        'chkWithStudded.Checked = True
        dtpAsOnDate.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        dtpAsOnDate.Select()
        Prop_Gets()
        rbtStockOnly.Checked = True
        If RPT_DISPSTKTYPE Then GrpStkType.Enabled = True Else GrpStkType.Enabled = False
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
        If chkItemCategory.Checked = False And chkWithCategory.Checked = False Then
            'Temporary Checking
            chkNone.Checked = True
        End If
        Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre)
        Dim chkItemCounter As String = GetChecked_CheckedList(chkLstItemCounter)
        Dim chkMetalName As String = GetChecked_CheckedList(chkLstMetal)
        Dim chkItemName As String = GetChecked_CheckedList(chkLstItem)
        Dim chkDesigner As String = GetChecked_CheckedList(chkLstDesigner)
        strSql = vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPTAGVIEW')>0 DROP TABLE TEMPTABLEDB..TEMPTAGVIEW"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " (SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)aS DESIGNER"
        strSql += vbCrLf + " ,T.TAGNO,T.RECDATE"
        If chktranno.Checked = True Then
            strSql += vbCrLf + " ,(SELECT TRANNO FROM " & cnStockDb & "..RECEIPT WHERE SNO IN("
            strSql += vbCrLf + " SELECT RECSNO FROM " & cnStockDb & "..LOTISSUE WHERE LOTSNO IN("
            strSql += vbCrLf + " SELECT SNO  FROM " & cnAdminDb & "..ITEMLOT WHERE SNO IN("
            strSql += vbCrLf + " SELECT LOTSNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=T.TAGNO ))))TRANNO"
        End If
        
        strSql += vbCrLf + " ,T.STYLENO,T.PCS,T.GRSWT AS TAGGRSWT,(ISNULL(P.PURNETWT,0)+ISNULL(P.PURLESSWT,0)) AS PURGRSWT,T.NETWT AS TAGNETWT,P.PURNETWT PURNETWT"
        'strSql += vbCrLf + " ,ISNULL((SELECT PTAX FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)),0)PURTAX"
        strSql += vbCrLf + " ,T.TRANINVNO,T.SUPBILLNO"
        strSql += vbCrLf + " ,CASE WHEN T.ISSDATE IS NOT NULL THEN 'DATE : ' + CONVERT(VARCHAR,T.ISSDATE,103) + ';PCS : ' + CONVERT(VARCHAR,T.ISSPCS) + ';WEIGHT : ' + CONVERT(VARCHAR,T.ISSWT) ELSE '' END AS SALESDET"
        strSql += vbCrLf + " ,(SELECT ITEMNAME FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
        strSql += vbCrLf + " ,(SELECT GROUPNAME FROM " & cnAdminDb & "..ITEMGROUPMAST WHERE GROUPID =(SELECT ITEMGROUP FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID))AS ITEMGROUP"
        strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM  " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = T.ITEMID AND SUBITEMID = T.SUBITEMID)AS SUBITEM"
        strSql += vbCrLf + " ,(SELECT CATNAME FROM  " & cnAdminDb & "..CATEGORY WHERE CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID) )AS CATEGORY"
        strSql += vbCrLf + " ,T.TAGVAL"
        strSql += vbCrLf + " ,P.PURVALUE,P.PURMC,P.PURTAX"
        strSql += vbCrLf + " ,T.SNO,T.ITEMID,T.PCTFILE AS PCTFILE,CONVERT(VARCHAR(10),'ITEMTAG') STKTABLE"
        strSql += vbCrLf + " ,CASE WHEN T.STKTYPE='M' THEN 'MANUFACTURING' WHEN T.STKTYPE='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STKTYPE"
        strSql += vbCrLf + " ,(SELECT SUM(AMOUNT) FROM " & cnStockDb & "..ISSUE WHERE TAGNO=T.TAGNO AND ITEMID=T.ITEMID)SALESAMOUNT"
        strSql += vbCrLf + " ,(SELECT SUM(TAXAMOUNT) FROM " & cnStockDb & "..TAXTRAN WHERE SNO= (SELECT TOP 1 SNO FROM " & cnStockDb & "..ISSUE WHERE TAGNO=T.TAGNO AND ITEMID=T.ITEMID))SALESED"
        strSql += vbCrLf + " ,(SELECT SUM(TAX) FROM " & cnStockDb & "..ISSUE WHERE TAGNO=T.TAGNO AND ITEMID=T.ITEMID)SALESTAX"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPTAGVIEW"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMTAG AS T"
        strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..PURITEMTAG AS P ON P.TAGSNO = T.SNO"
        strSql += vbCrLf + " WHERE 1 = 1 "
        If rbtStockOnly.Checked Then strSql += vbCrLf + " AND T.RECDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND (T.ISSDATE IS NULL OR T.ISSDATE > '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "')"
        'If rbtStockOnly.Checked Then strSql += vbCrLf + " AND T.RECDATE BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND (T.ISSDATE IS NULL OR T.ISSDATE > '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "')"
        If rbtReceipt.Checked Then strSql += vbCrLf + " AND T.RECDATE BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        If rbtIssue.Checked Then strSql += vbCrLf + " AND T.ISSDATE  BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        If chkCostName <> "" Then strSql += vbCrLf + " AND T.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkItemCounter <> "" Then strSql += vbCrLf + " AND T.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & chkItemCounter & "))"
        If chkItemName <> "" Then strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkItemName & "))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkDesigner <> "" Then
            strSql += vbCrLf + " AND T.DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE "
            strSql += vbCrLf + " DESIGNERNAME IN (" & chkDesigner & "))"
        End If
        If txtTranInvNo.Text <> "" Then strSql += vbCrLf + " AND T.TRANINVNO = '" & txtTranInvNo.Text & "'"
        strSql += vbCrLf + " AND T.COMPANYID = '" & GetStockCompId() & "'"
        If GrpStkType.Enabled Then
            If rbtManufacturin.Checked Then
                strSql += vbCrLf + " AND T.STKTYPE = 'M'"
            ElseIf rbtTrading.Checked Then
                strSql += vbCrLf + " AND T.STKTYPE = 'T'"
            ElseIf rbtExem.Checked Then
                strSql += vbCrLf + " AND T.STKTYPE = 'E'"
            End If
        End If
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        If ChkWithTrf.Checked = True Then
            strSql = vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPTAGVIEW2')>0 DROP TABLE TEMPTABLEDB..TEMPTAGVIEW2"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " (SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)aS DESIGNER"
            strSql += vbCrLf + " ,T.TAGNO,T.RECDATE"
            If chktranno.Checked = True Then
                strSql += vbCrLf + " ,(SELECT TRANNO FROM " & cnStockDb & "..RECEIPT WHERE SNO IN("
                strSql += vbCrLf + " SELECT RECSNO FROM " & cnStockDb & "..LOTISSUE WHERE LOTSNO IN("
                strSql += vbCrLf + " SELECT SNO  FROM " & cnAdminDb & "..ITEMLOT WHERE SNO IN("
                strSql += vbCrLf + " SELECT LOTSNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=T.TAGNO ))))TRANNO"
            End If

            strSql += vbCrLf + ",T.STYLENO,T.PCS,T.GRSWT AS TAGGRSWT,(ISNULL(P.PURNETWT,0)+ISNULL(P.PURLESSWT,0)) AS PURGRSWT,T.NETWT AS TAGNETWT,P.PURNETWT PURNETWT"
            'strSql += vbCrLf + " ,ISNULL((SELECT PTAX FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)),0)PURTAX"
            strSql += vbCrLf + " ,T.TRANINVNO,T.SUPBILLNO"
            strSql += vbCrLf + " ,CASE WHEN T.ISSDATE IS NOT NULL THEN 'DATE : ' + CONVERT(VARCHAR,T.ISSDATE,103) + ';PCS : ' + CONVERT(VARCHAR,T.ISSPCS) + ';WEIGHT : ' + CONVERT(VARCHAR,T.ISSWT) ELSE '' END AS SALESDET"
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
            strSql += vbCrLf + " ,(SELECT GROUPNAME FROM " & cnAdminDb & "..ITEMGROUPMAST WHERE GROUPID =(SELECT ITEMGROUP FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID))AS ITEMGROUP"
            strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM  " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = T.ITEMID AND SUBITEMID = T.SUBITEMID)AS SUBITEM"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM  " & cnAdminDb & "..CATEGORY WHERE CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID) )AS CATEGORY"
            strSql += vbCrLf + " ,T.TAGVAL"
            strSql += vbCrLf + " ,P.PURVALUE,P.PURMC,P.PURTAX"
            strSql += vbCrLf + " ,T.SNO,T.ITEMID,T.PCTFILE AS PCTFILE,CONVERT(VARCHAR(10),'TITEMTAG') STKTABLE"
            strSql += vbCrLf + " ,CASE WHEN T.STKTYPE='M' THEN 'MANUFACTURING' WHEN T.STKTYPE='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STKTYPE"
            strSql += vbCrLf + " ,NULL SALESAMOUNT"
            strSql += vbCrLf + " ,NULL SALESED"
            strSql += vbCrLf + " ,NULL SALESTAX"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPTAGVIEW2"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..TITEMTAG AS T"
            strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..PURITEMTAG AS P ON P.TAGSNO = T.SNO"
            strSql += vbCrLf + " WHERE 1=1 "
            If rbtStockOnly.Checked Then strSql += vbCrLf + " AND T.RECDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND (T.ISSDATE IS NULL OR T.ISSDATE > '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "')"
            If rbtReceipt.Checked Then strSql += vbCrLf + " AND T.RECDATE BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            If rbtIssue.Checked Then strSql += vbCrLf + " AND T.ISSDATE  BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            If chkCostName <> "" Then strSql += vbCrLf + " AND T.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkItemCounter <> "" Then strSql += vbCrLf + " AND T.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & chkItemCounter & "))"
            If chkItemName <> "" Then strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkItemName & "))"
            If chkMetalName <> "" Then strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
            If chkDesigner <> "" Then strSql += vbCrLf + " AND T.DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & chkDesigner & "))"
            If txtTranInvNo.Text <> "" Then strSql += vbCrLf + " AND T.TRANINVNO = '" & txtTranInvNo.Text & "'"
            strSql += vbCrLf + " AND T.COMPANYID = '" & GetStockCompId() & "'"
            If GrpStkType.Enabled Then
                If rbtManufacturin.Checked Then
                    strSql += vbCrLf + " AND T.STKTYPE = 'M'"
                ElseIf rbtTrading.Checked Then
                    strSql += vbCrLf + " AND T.STKTYPE = 'T'"
                ElseIf rbtExem.Checked Then
                    strSql += vbCrLf + " AND T.STKTYPE = 'E'"
                End If
            End If
            cmd = New OleDbCommand(strSql, cn)
            cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        If ChkWithPend.Checked = True Then
            strSql = vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPTAGVIEW3')>0 DROP TABLE TEMPTABLEDB..TEMPTAGVIEW3"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " (SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)aS DESIGNER"
            strSql += vbCrLf + " ,T.TAGNO,T.RECDATE"
            If chktranno.Checked = True Then
                strSql += vbCrLf + " ,(SELECT TRANNO FROM " & cnStockDb & "..RECEIPT WHERE SNO IN("
                strSql += vbCrLf + " SELECT RECSNO FROM " & cnStockDb & "..LOTISSUE WHERE LOTSNO IN("
                strSql += vbCrLf + " SELECT SNO  FROM " & cnAdminDb & "..ITEMLOT WHERE SNO IN("
                strSql += vbCrLf + " SELECT LOTSNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=T.TAGNO ))))TRANNO"
            End If

            strSql += vbCrLf + ",T.STYLENO,T.PCS,T.GRSWT AS TAGGRSWT,(ISNULL(P.PURNETWT,0)+ISNULL(P.PURLESSWT,0)) AS PURGRSWT,T.NETWT AS TAGNETWT,P.PURNETWT PURNETWT"
            'strSql += vbCrLf + " ,ISNULL((SELECT PTAX FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)),0)PURTAX"
            strSql += vbCrLf + " ,T.TRANINVNO,T.SUPBILLNO"
            strSql += vbCrLf + " ,CASE WHEN T.ISSDATE IS NOT NULL THEN 'DATE : ' + CONVERT(VARCHAR,T.ISSDATE,103) + ';PCS : ' + CONVERT(VARCHAR,T.ISSPCS) + ';WEIGHT : ' + CONVERT(VARCHAR,T.ISSWT) ELSE '' END AS SALESDET"
            strSql += vbCrLf + " ,(SELECT ITEMNAME FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
            strSql += vbCrLf + " ,(SELECT GROUPNAME FROM " & cnAdminDb & "..ITEMGROUPMAST WHERE GROUPID =(SELECT ITEMGROUP FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID))AS ITEMGROUP"
            strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM  " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = T.ITEMID AND SUBITEMID = T.SUBITEMID)AS SUBITEM"
            strSql += vbCrLf + " ,(SELECT CATNAME FROM  " & cnAdminDb & "..CATEGORY WHERE CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID) )AS CATEGORY"
            strSql += vbCrLf + " ,T.TAGVAL"
            strSql += vbCrLf + " ,P.PURVALUE,P.PURMC,P.PURTAX"
            strSql += vbCrLf + " ,T.SNO,T.ITEMID,T.PCTFILE AS PCTFILE,CONVERT(VARCHAR(10),'PITEMTAG') STKTABLE"
            strSql += vbCrLf + " ,CASE WHEN T.STKTYPE='M' THEN 'MANUFACTURING' WHEN T.STKTYPE='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STKTYPE"
            strSql += vbCrLf + " ,NULL SALESAMOUNT"
            strSql += vbCrLf + " ,NULL SALESED"
            strSql += vbCrLf + " ,NULL SALESTAX"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPTAGVIEW3"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..PITEMTAG AS T"
            strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..PURITEMTAG AS P ON P.TAGSNO = T.SNO"
            strSql += vbCrLf + " WHERE 1=1 "
            If rbtStockOnly.Checked Then strSql += vbCrLf + " AND T.RECDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND (T.ISSDATE IS NULL OR T.ISSDATE > '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "')"
            'If rbtStockOnly.Checked Then strSql += vbCrLf + " AND T.RECDATE BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND (T.ISSDATE IS NULL OR T.ISSDATE > '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "')"
            If rbtReceipt.Checked Then strSql += vbCrLf + " AND T.RECDATE BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            If rbtIssue.Checked Then strSql += vbCrLf + " AND T.ISSDATE  BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            If chkCostName <> "" Then strSql += vbCrLf + " AND T.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkItemCounter <> "" Then strSql += vbCrLf + " AND T.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & chkItemCounter & "))"
            If chkItemName <> "" Then strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkItemName & "))"
            If chkMetalName <> "" Then strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
            If chkDesigner <> "" Then strSql += vbCrLf + " AND T.DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & chkDesigner & "))"
            If txtTranInvNo.Text <> "" Then strSql += vbCrLf + " AND T.TRANINVNO = '" & txtTranInvNo.Text & "'"
            strSql += vbCrLf + " AND T.COMPANYID = '" & GetStockCompId() & "'"
            If GrpStkType.Enabled Then
                If rbtManufacturin.Checked Then
                    strSql += vbCrLf + " AND T.STKTYPE = 'M'"
                ElseIf rbtTrading.Checked Then
                    strSql += vbCrLf + " AND T.STKTYPE = 'T'"
                ElseIf rbtExem.Checked Then
                    strSql += vbCrLf + " AND T.STKTYPE = 'E'"
                End If
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        strSql = vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPTAGVIEW1')>0 DROP TABLE TEMPTABLEDB..TEMPTAGVIEW1"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " (SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)aS DESIGNER"
        strSql += vbCrLf + " ,T.TAGNO,T.RECDATE"
        If chktranno.Checked = True Then
            strSql += vbCrLf + " ,(SELECT TRANNO FROM " & cnStockDb & "..RECEIPT WHERE SNO IN("
            strSql += vbCrLf + " SELECT RECSNO FROM " & cnStockDb & "..LOTISSUE WHERE LOTSNO IN("
            strSql += vbCrLf + " SELECT SNO  FROM " & cnAdminDb & "..ITEMLOT WHERE SNO IN("
            strSql += vbCrLf + " SELECT LOTSNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO=T.TAGNO ))))TRANNO"
        End If

        strSql += vbCrLf + ",T.STYLENO,T.PCS,T.GRSWT AS TAGGRSWT,(ISNULL(P.PURNETWT,0)+ISNULL(P.PURLESSWT,0)) AS PURGRSWT,T.NETWT AS TAGNETWT,P.PURNETWT PURNETWT"
        'strSql += vbCrLf + " ,ISNULL((SELECT PTAX FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)),0)PURTAX"
        strSql += vbCrLf + " ,T.TRANINVNO,T.SUPBILLNO"
        strSql += vbCrLf + " ,CASE WHEN T.ISSDATE IS NOT NULL THEN 'DATE : ' + CONVERT(VARCHAR,T.ISSDATE,103) + ';PCS : ' + CONVERT(VARCHAR,T.ISSPCS) + ';WEIGHT : ' + CONVERT(VARCHAR,T.ISSWT) ELSE '' END AS SALESDET"
        strSql += vbCrLf + " ,(SELECT ITEMNAME FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
        strSql += vbCrLf + " ,(SELECT GROUPNAME FROM " & cnAdminDb & "..ITEMGROUPMAST WHERE GROUPID =(SELECT ITEMGROUP FROM  " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID))AS ITEMGROUP"
        strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM  " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID = T.ITEMID AND SUBITEMID = T.SUBITEMID)AS SUBITEM"
        strSql += vbCrLf + " ,(SELECT CATNAME FROM  " & cnAdminDb & "..CATEGORY WHERE CATCODE = (SELECT CATCODE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.ITEMID) )AS CATEGORY"
        strSql += vbCrLf + " ,T.TAGVAL"
        strSql += vbCrLf + " ,P.PURVALUE,P.PURMC,P.PURTAX"
        strSql += vbCrLf + " ,T.SNO,T.ITEMID,T.PCTFILE AS PCTFILE,CONVERT(VARCHAR(10),'CITEMTAG') STKTABLE"
        strSql += vbCrLf + " ,CASE WHEN T.STKTYPE='M' THEN 'MANUFACTURING' WHEN T.STKTYPE='E' THEN 'EXEMPTED' ELSE 'TRADING' END AS STKTYPE"
        strSql += vbCrLf + " ,NULL SALESAMOUNT"
        strSql += vbCrLf + " ,NULL SALESED"
        strSql += vbCrLf + " ,NULL SALESTAX"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPTAGVIEW1"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..CITEMTAG AS T"
        strSql += vbCrLf + " LEFT OUTER JOIN " & cnAdminDb & "..PURITEMTAG AS P ON P.TAGSNO = T.SNO"
        strSql += vbCrLf + " WHERE 1=1 "
        If rbtStockOnly.Checked Then strSql += vbCrLf + " AND T.RECDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND (T.ISSDATE IS NULL OR T.ISSDATE > '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "')"
        'If rbtStockOnly.Checked Then strSql += vbCrLf + " AND T.RECDATE BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "' AND (T.ISSDATE IS NULL OR T.ISSDATE > '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "')"
        If rbtReceipt.Checked Then strSql += vbCrLf + " AND T.RECDATE BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        If rbtIssue.Checked Then strSql += vbCrLf + " AND T.ISSDATE  BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        If chkCostName <> "" Then strSql += vbCrLf + " AND T.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkItemCounter <> "" Then strSql += vbCrLf + " AND T.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & chkItemCounter & "))"
        If chkItemName <> "" Then strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkItemName & "))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If chkDesigner <> "" Then strSql += vbCrLf + " AND T.DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & chkDesigner & "))"
        If txtTranInvNo.Text <> "" Then strSql += vbCrLf + " AND T.TRANINVNO = '" & txtTranInvNo.Text & "'"
        strSql += vbCrLf + " AND T.COMPANYID = '" & GetStockCompId() & "'"
        If GrpStkType.Enabled Then
            If rbtManufacturin.Checked Then
                strSql += vbCrLf + " AND T.STKTYPE = 'M'"
            ElseIf rbtTrading.Checked Then
                strSql += vbCrLf + " AND T.STKTYPE = 'T'"
            ElseIf rbtExem.Checked Then
                strSql += vbCrLf + " AND T.STKTYPE = 'E'"
            End If
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPTAGVIEW SELECT * FROM TEMPTABLEDB..TEMPTAGVIEW1"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        If ChkWithTrf.Checked = True Then
            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPTAGVIEW SELECT * FROM TEMPTABLEDB..TEMPTAGVIEW2"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        If ChkWithPend.Checked = True Then
            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPTAGVIEW SELECT * FROM TEMPTABLEDB..TEMPTAGVIEW3"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        strSql = vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPTAGSTONEVIEW')>0 DROP TABLE TEMPTABLEDB..TEMPTAGSTONEVIEW"
        strSql += vbCrLf + " SELECT "
        strSql += vbCrLf + " SUM(CASE WHEN IM.DIASTONE = 'D' THEN PST.STNWT ELSE 0 END) AS DIAWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN IM.DIASTONE = 'D' THEN PST.PURAMT ELSE 0 END) AS DIAAMT"
        strSql += vbCrLf + " ,SUM(CASE WHEN IM.DIASTONE = 'S' THEN PST.STNWT ELSE 0 END) AS STNWT"
        strSql += vbCrLf + " ,SUM(CASE WHEN IM.DIASTONE = 'S' THEN PST.PURAMT ELSE 0 END) AS STNAMT"
        strSql += vbCrLf + " ,PST.TAGSNO,CONVERT(VARCHAR(10),'ITEMTAG') STKTABLE"
        strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPTAGSTONEVIEW"
        strSql += vbCrLf + " FROM " & cnAdminDb & "..PURITEMTAGSTONE AS PST"
        strSql += vbCrLf + " INNER JOIN TEMPTABLEDB..TEMPTAGVIEW AS T ON T.SNO = PST.TAGSNO"
        strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = PST.STNITEMID"
        strSql += vbCrLf + " WHERE T.STKTABLE IN('CITEMTAG','ITEMTAG')"
        strSql += vbCrLf + "  GROUP BY PST.TAGSNO"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        If ChkWithTrf.Checked = True Then
            strSql = vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPTAGSTONEVIEW1')>0 DROP TABLE TEMPTABLEDB..TEMPTAGSTONEVIEW1"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " SUM(CASE WHEN IM.DIASTONE = 'D' THEN PST.STNWT ELSE 0 END) AS DIAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN IM.DIASTONE = 'D' THEN PST.PURAMT ELSE 0 END) AS DIAAMT"
            strSql += vbCrLf + " ,SUM(CASE WHEN IM.DIASTONE = 'S' THEN PST.STNWT ELSE 0 END) AS STNWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN IM.DIASTONE = 'S' THEN PST.PURAMT ELSE 0 END) AS STNAMT"
            strSql += vbCrLf + " ,PST.TAGSNO,CONVERT(VARCHAR(10),'TITEMTAG') STKTABLE"
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPTAGSTONEVIEW1"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..PURITEMTAGSTONE AS PST"
            strSql += vbCrLf + " INNER JOIN TEMPTABLEDB..TEMPTAGVIEW AS T ON T.SNO = PST.TAGSNO"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = PST.STNITEMID"
            strSql += vbCrLf + " WHERE T.STKTABLE='TITEMTAG'"
            strSql += vbCrLf + "  GROUP BY PST.TAGSNO"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPTAGSTONEVIEW SELECT * FROM TEMPTABLEDB..TEMPTAGSTONEVIEW1"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        If ChkWithPend.Checked = True Then
            strSql = vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMPTAGSTONEVIEW2')>0 DROP TABLE TEMPTABLEDB..TEMPTAGSTONEVIEW2"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " SUM(CASE WHEN IM.DIASTONE = 'D' THEN PST.STNWT ELSE 0 END) AS DIAWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN IM.DIASTONE = 'D' THEN PST.PURAMT ELSE 0 END) AS DIAAMT"
            strSql += vbCrLf + " ,SUM(CASE WHEN IM.DIASTONE = 'S' THEN PST.STNWT ELSE 0 END) AS STNWT"
            strSql += vbCrLf + " ,SUM(CASE WHEN IM.DIASTONE = 'S' THEN PST.PURAMT ELSE 0 END) AS STNAMT"
            strSql += vbCrLf + " ,PST.TAGSNO,CONVERT(VARCHAR(10),'PITEMTAG') STKTABLE"
            strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMPTAGSTONEVIEW2"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..PURITEMTAGSTONE AS PST"
            strSql += vbCrLf + " INNER JOIN TEMPTABLEDB..TEMPTAGVIEW AS T ON T.SNO = PST.TAGSNO"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = PST.STNITEMID"
            strSql += vbCrLf + " WHERE T.STKTABLE='PITEMTAG'"
            strSql += vbCrLf + "  GROUP BY PST.TAGSNO"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMPTAGSTONEVIEW SELECT * FROM TEMPTABLEDB..TEMPTAGSTONEVIEW2"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''START 
        strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "TAGPURDET')>0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "TAGPURDET"
        If rbtSummary.Checked = True Then
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(60),item) PARTICULAR,NULL DESIGNER,NULL TAGNO,NULL RECDATE,NULL  TRANNO,NULL STYLENO"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SUM(PURVALUE)) TOTAL"
            strSql += vbCrLf + " ,SUM(PCS) PCS,SUM(TAGGRSWT)TAGGRSWT,SUM(PURGRSWT)PURGRSWT,SUM(TAGNETWT)TAGNETWT"
            strSql += vbCrLf + " ,SUM(PURNETWT)PURNETWT"
            strSql += vbCrLf + " ,SUM(ISNULL(PURVALUE,0)-ISNULL(STNAMT,0)-ISNULL(DIAAMT,0)-ISNULL(PURMC,0)-ISNULL(PURTAX,0)) AS NETAMT"
            strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,4),DIAWT))DIAWT,SUM(CONVERT(NUMERIC(15,2),DIAAMT))DIAAMT,SUM(CONVERT(NUMERIC(15,3),STNWT))STNWT"
            strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),STNAMT))STNAMT,SUM(CONVERT(NUMERIC(15,2),PURMC))PURMC"
            strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),PURVALUE-ISNULL(PURTAX,0))) GRANDTOTAL"
            strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),PURTAX)) PURTAX"
            strSql += vbCrLf + " , CASE WHEN DESIGNEROUTSTANDING = 'L'  THEN SUM(CONVERT(NUMERIC(15,2),PURVALUE-ISNULL(PURTAX,0))) ELSE SUM(CONVERT(NUMERIC(15,2),PURVALUE-ISNULL(PURTAX,0))) + SUM(CONVERT(NUMERIC(15,2),PURTAX)) END AS FINALTOTAL"
            strSql += vbCrLf + " ,NULL TRANINVNO, NULL SUPBILLNO,CONVERT(VARCHAR(10),'AITEMTAG') ORDERITEM,CONVERT(VARCHAR,NULL) AS ITEM"
            strSql += vbCrLf + " ,ITEMGROUP, CATEGORY,CONVERT(NUMERIC(15,2),2)AS RESULT,'   'COLHEAD,NULL TAGVAL,NULL PCTFILE,ITEMID"
            strSql += vbCrLf + " ,CONVERT (VARCHAR(20),NULL)STKTYPE"
            strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),SALESAMOUNT)) SALESAMOUNT"
            strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),SALESED)) SALESED"
            strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),SALESTAX)) SALESTAX"
            strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),ISNULL(SALESAMOUNT,0)+ISNULL(SALESED,0)+ISNULL(SALESTAX,0)))SALESTOTAL"
        Else
            strSql += vbCrLf + " SELECT CONVERT(VARCHAR(60),PARTICULAR)PARTICULAR,DESIGNER,TAGNO,RECDATE,"
            If chktranno.Checked = True Then strSql += vbCrLf + "TRANNO, "
            strSql += vbCrLf + " STYLENO"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),PURVALUE) TOTAL"
            strSql += vbCrLf + " ,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT"
            strSql += vbCrLf + " ,ISNULL(PURVALUE,0)-ISNULL(STNAMT,0)-ISNULL(DIAAMT,0)-ISNULL(PURMC,0)-ISNULL(PURTAX,0) AS NETAMT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),DIAWT)DIAWT,CONVERT(NUMERIC(15,2),DIAAMT)DIAAMT,CONVERT(NUMERIC(15,3),STNWT)STNWT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),STNAMT)STNAMT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),PURMC)PURMC,CONVERT(NUMERIC(15,2),PURVALUE-ISNULL(PURTAX,0)) GRANDTOTAL"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),PURTAX) PURTAX"
            strSql += vbCrLf + " ,CASE WHEN DESIGNEROUTSTANDING = 'L' THEN CONVERT(NUMERIC(15,2),PURVALUE-ISNULL(PURTAX,0)) ELSE CONVERT(NUMERIC(15,2),PURVALUE-ISNULL(PURTAX,0)) + CONVERT(NUMERIC(15,2),PURTAX) END AS FINALTOTAL"
            strSql += vbCrLf + " ,TRANINVNO,SUPBILLNO,CONVERT(VARCHAR(10),'AITEMTAG') ORDERITEM"
            strSql += vbCrLf + " ,ITEM,ITEMGROUP,CATEGORY"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),2)AS RESULT,'    'COLHEAD"
            strSql += vbCrLf + " ,CONVERT(INT,TAGVAL)TAGVAL,PCTFILE,ITEMID"
            strSql += vbCrLf + " ,CONVERT (VARCHAR(20),STKTYPE)STKTYPE"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SALESAMOUNT) SALESAMOUNT"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SALESED) SALESED"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SALESTAX) SALESTAX"
            strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),ISNULL(SALESAMOUNT,0)+ISNULL(SALESED,0)+ISNULL(SALESTAX,0)) SALESTOTAL"
        End If
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET"
        strSql += vbCrLf + " FROM"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT CASE WHEN ISNULL(T.SUBITEM,'') <> '' THEN T.SUBITEM ELSE T.ITEM END AS PARTICULAR"
        strSql += vbCrLf + " ,T.TAGNO,T.CATEGORY,T.RECDATE,"
        If chktranno.Checked = True Then strSql += vbCrLf + " TRANNO, "
        strSql += vbCrLf + "  T.STYLENO"
        If STOCKVIEW_GRSWT_AS_DIAWT Then
            strSql += vbCrLf + " ,CASE WHEN  ISNULL((SELECT  1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID AND METALID IN ('D','T')  AND ISNULL(STUDDED,'')='L'),0) <> 1 THEN T.PCS ELSE NULL END PCS "
            strSql += vbCrLf + " ,CASE WHEN  ISNULL((SELECT  1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID AND METALID IN ('D','T')  AND ISNULL(STUDDED,'')='L'),0) <> 1 THEN T.TAGGRSWT ELSE NULL END TAGGRSWT "
            strSql += vbCrLf + " ,CASE WHEN  ISNULL((SELECT  1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID AND METALID IN ('D','T')  AND ISNULL(STUDDED,'')='L'),0) <> 1 THEN T.PURGRSWT ELSE NULL END PURGRSWT "
            strSql += vbCrLf + " ,CASE WHEN  ISNULL((SELECT  1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID AND METALID IN ('D','T')  AND ISNULL(STUDDED,'')='L'),0) <> 1 THEN T.TAGNETWT ELSE NULL END TAGNETWT "
            strSql += vbCrLf + " ,CASE WHEN  ISNULL((SELECT  1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID AND METALID IN ('D','T')  AND ISNULL(STUDDED,'')='L'),0) <> 1 THEN T.PURNETWT ELSE NULL END PURNETWT "
        Else
            strSql += vbCrLf + " ,T.PCS,T.TAGGRSWT,T.PURGRSWT,T.TAGNETWT,T.PURNETWT"
        End If
        strSql += vbCrLf + " ,T.PURMC,T.PURVALUE"
        strSql += vbCrLf + " ,T.TRANINVNO,T.SUPBILLNO"
        strSql += vbCrLf + " ,T.SALESDET"
        strSql += vbCrLf + " ,T.ITEM"
        strSql += vbCrLf + " ,T.ITEMGROUP"
        strSql += vbCrLf + " ,T.TAGVAL,T.PURTAX"
        If STOCKVIEW_GRSWT_AS_DIAWT Then
            strSql += vbCrLf + " ,CASE WHEN  ISNULL((SELECT  1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID AND METALID IN ('D')  AND ISNULL(STUDDED,'')='L'),0) = 1 THEN T.TAGGRSWT ELSE ST.DIAWT END DIAWT "
            strSql += vbCrLf + " ,CASE WHEN  ISNULL((SELECT  1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID AND METALID IN ('D')  AND ISNULL(STUDDED,'')='L'),0) = 1 THEN (ISNULL(T.PURVALUE,0)-ISNULL(T.PURTAX,0)) ELSE ST.DIAAMT END DIAAMT "
            strSql += vbCrLf + " ,CASE WHEN  ISNULL((SELECT  1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID AND METALID IN ('T')  AND ISNULL(STUDDED,'')='L'),0) = 1 THEN T.TAGGRSWT ELSE ST.STNWT END STNWT "
            strSql += vbCrLf + " ,CASE WHEN  ISNULL((SELECT  1 FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID AND METALID IN ('T')  AND ISNULL(STUDDED,'')='L'),0) = 1 THEN (ISNULL(T.PURVALUE,0)-ISNULL(T.PURTAX,0)) ELSE ST.STNAMT END STNAMT "
        Else
            strSql += vbCrLf + " ,ST.DIAWT,ST.DIAAMT,ST.STNWT,ST.STNAMT"
        End If
        strSql += vbCrLf + " ,T.PCTFILE,T.ITEMID,T.DESIGNER"
        strSql += vbCrLf + " ,(SELECT LOCALOUTST FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE =( SELECT ACCODE FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = T.DESIGNER)) AS DESIGNEROUTSTANDING"
        strSql += vbCrLf + " ,T.STKTYPE"
        strSql += vbCrLf + " ,T.SALESAMOUNT,T.SALESED,T.SALESTAX"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPTAGVIEW AS T"
        strSql += vbCrLf + " LEFT OUTER JOIN TEMPTABLEDB..TEMPTAGSTONEVIEW AS ST ON ST.TAGSNO = T.SNO"
        strSql += vbCrLf + " WHERE T.STKTABLE IN('CITEMTAG','ITEMTAG')"
        strSql += vbCrLf + " )X"
        If rbtSummary.Checked = True Then
            strSql += vbCrLf + " GROUP BY ITEM,ITEMID,CATEGORY,ITEMGROUP,DESIGNEROUTSTANDING "
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        If ChkWithTrf.Checked = True Then
            strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "TAGPURDET1')>0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "TAGPURDET1"
            If rbtSummary.Checked = True Then
                strSql += vbCrLf + " SELECT CONVERT(VARCHAR(60),item) PARTICULAR,NULL DESIGNER,NULL TAGNO,NULL RECDATE,NULL TRANNO,NULL STYLENO"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SUM(PURVALUE)) TOTAL"
                strSql += vbCrLf + " ,SUM(PCS) PCS,SUM(TAGGRSWT)TAGGRSWT,SUM(PURGRSWT)PURGRSWT"
                strSql += vbCrLf + " ,SUM(TAGNETWT)TAGNETWT,SUM(PURNETWT)PURNETWT"
                strSql += vbCrLf + " ,SUM(ISNULL(PURVALUE,0)-ISNULL(STNAMT,0)-ISNULL(DIAAMT,0)-ISNULL(PURMC,0)-ISNULL(PURTAX,0)) AS NETAMT"
                strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,4),DIAWT))DIAWT,SUM(CONVERT(NUMERIC(15,2),DIAAMT))DIAAMT,SUM(CONVERT(NUMERIC(15,3),STNWT))STNWT"
                strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),STNAMT))STNAMT,SUM(CONVERT(NUMERIC(15,2),PURMC))PURMC"
                strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),PURVALUE-ISNULL(PURTAX,0))) GRANDTOTAL"
                strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),PURTAX)) PURTAX"
                strSql += vbCrLf + " ,CASE WHEN DESIGNEROUTSTANDING = 'L'  THEN SUM(CONVERT(NUMERIC(15,2),PURVALUE-ISNULL(PURTAX,0))) ELSE SUM(CONVERT(NUMERIC(15,2),PURVALUE-ISNULL(PURTAX,0))) + SUM(CONVERT(NUMERIC(15,2),PURTAX)) END AS FINALTOTAL"
                strSql += vbCrLf + " ,NULL TRANINVNO"
                strSql += vbCrLf + " ,NULL SUPBILLNO,CONVERT(VARCHAR(10),'BITEMTAG') ORDERITEM,CONVERT(VARCHAR,NULL) AS ITEM"
                strSql += vbCrLf + " , ITEMGROUP"
                strSql += vbCrLf + " ,CATEGORY,7 RESULT,'   'COLHEAD,NULL TAGVAL"
                strSql += vbCrLf + " ,NULL PCTFILE,ITEMID"
                strSql += vbCrLf + " ,CONVERT (VARCHAR(20),NULL)STKTYPE"
                strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),SALESAMOUNT)) SALESAMOUNT"
                strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),SALESED)) SALESED"
                strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),SALESTAX)) SALESTAX"
                strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),ISNULL(SALESAMOUNT,0)+ISNULL(SALESED,0)+ISNULL(SALESTAX,0)))SALESTOTAL"
            Else
                strSql += vbCrLf + " SELECT CONVERT(VARCHAR(60),PARTICULAR)PARTICULAR,DESIGNER,TAGNO,RECDATE,"
                If chktranno.Checked Then strSql += vbCrLf + " TRANNO,"
                strSql += vbCrLf + " STYLENO"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),PURVALUE) TOTAL"
                strSql += vbCrLf + " ,PCS,TAGGRSWT,PURGRSWT"
                strSql += vbCrLf + " ,TAGNETWT,PURNETWT,ISNULL(PURVALUE,0)-ISNULL(STNAMT,0)-ISNULL(DIAAMT,0)-ISNULL(PURMC,0)-ISNULL(PURTAX,0) AS NETAMT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),DIAWT)DIAWT,CONVERT(NUMERIC(15,2),DIAAMT)DIAAMT,CONVERT(NUMERIC(15,3),STNWT)STNWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),STNAMT)STNAMT,CONVERT(NUMERIC(15,2),PURMC)PURMC"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),PURVALUE-ISNULL(PURTAX,0)) GRANDTOTAL"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),PURTAX) PURTAX"
                strSql += vbCrLf + " ,CASE WHEN DESIGNEROUTSTANDING = 'L' THEN CONVERT(NUMERIC(15,2),PURVALUE-ISNULL(PURTAX,0)) ELSE CONVERT(NUMERIC(15,2),PURVALUE-ISNULL(PURTAX,0)) + CONVERT(NUMERIC(15,2),PURTAX) END AS FINALTOTAL"
                strSql += vbCrLf + " ,TRANINVNO,SUPBILLNO,CONVERT(VARCHAR(10),'BITEMTAG') ORDERITEM"
                strSql += vbCrLf + " ,ITEM,ITEMGROUP, CATEGORY,7 RESULT,'    'COLHEAD,CONVERT(INT,TAGVAL)TAGVAL"
                strSql += vbCrLf + " ,PCTFILE,ITEMID"
                strSql += vbCrLf + " ,CONVERT (VARCHAR(20),STKTYPE)STKTYPE"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SALESAMOUNT) SALESAMOUNT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SALESED) SALESED"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SALESTAX) SALESTAX"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),ISNULL(SALESAMOUNT,0)+ISNULL(SALESED,0)+ISNULL(SALESTAX,0)) SALESTOTAL"
            End If
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET1"
            strSql += vbCrLf + " FROM"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT CASE WHEN ISNULL(T.SUBITEM,'') <> '' THEN T.SUBITEM ELSE T.ITEM END AS PARTICULAR"
            strSql += vbCrLf + " ,T.TAGNO,T.CATEGORY,T.RECDATE,"
            If chktranno.Checked Then strSql += vbCrLf + " TRANNO,"
            strSql += vbCrLf + "T.STYLENO,T.PCS,T.TAGGRSWT,T.PURGRSWT,T.TAGNETWT,T.PURNETWT"
            strSql += vbCrLf + " ,T.PURMC,T.PURVALUE"
            strSql += vbCrLf + " ,T.TRANINVNO,T.SUPBILLNO"
            strSql += vbCrLf + " ,T.SALESDET"
            strSql += vbCrLf + " ,T.ITEM"
            strSql += vbCrLf + " ,T.ITEMGROUP"
            strSql += vbCrLf + " ,T.TAGVAL,T.PURTAX"
            strSql += vbCrLf + " ,ST.DIAWT,ST.DIAAMT,ST.STNWT,ST.STNAMT,T.PCTFILE,T.ITEMID,T.DESIGNER"
            strSql += vbCrLf + " ,(SELECT LOCALOUTST FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE =( SELECT ACCODE FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = T.DESIGNER)) AS DESIGNEROUTSTANDING"
            strSql += vbCrLf + " ,T.STKTYPE"
            strSql += vbCrLf + " ,T.SALESAMOUNT,T.SALESED,T.SALESTAX"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPTAGVIEW AS T"
            strSql += vbCrLf + " LEFT OUTER JOIN TEMPTABLEDB..TEMPTAGSTONEVIEW AS ST ON ST.TAGSNO = T.SNO"
            strSql += vbCrLf + " WHERE T.STKTABLE IN('TITEMTAG')"
            strSql += vbCrLf + " )X"
            If rbtSummary.Checked = True Then
                strSql += vbCrLf + " GROUP BY ITEM,ITEMID,CATEGORY,ITEMGROUP "
                strSql += vbCrLf + ",DESIGNEROUTSTANDING"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET1"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        If ChkWithPend.Checked = True Then
            strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "TAGPURDET2')>0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "TAGPURDET2"
            If rbtSummary.Checked = True Then
                strSql += vbCrLf + " SELECT CONVERT(VARCHAR(60),item) PARTICULAR,NULL DESIGNER,NULL TAGNO,NULL RECDATE,NULL TRANNO,NULL STYLENO"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SUM(PURVALUE)) TOTAL"
                strSql += vbCrLf + " ,SUM(PCS) PCS,SUM(TAGGRSWT)TAGGRSWT,SUM(PURGRSWT)PURGRSWT,SUM(TAGNETWT)TAGNETWT"
                strSql += vbCrLf + " ,SUM(PURNETWT)PURNETWT"
                strSql += vbCrLf + " ,SUM(ISNULL(PURVALUE,0)-ISNULL(STNAMT,0)-ISNULL(DIAAMT,0)-ISNULL(PURMC,0)-ISNULL(PURTAX,0)) AS NETAMT"
                strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,4),DIAWT))DIAWT,SUM(CONVERT(NUMERIC(15,2),DIAAMT))DIAAMT,SUM(CONVERT(NUMERIC(15,3),STNWT))STNWT"
                strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),STNAMT))STNAMT,SUM(CONVERT(NUMERIC(15,2),PURMC))PURMC,SUM(CONVERT(NUMERIC(15,2)"
                strSql += vbCrLf + " ,PURVALUE-ISNULL(PURTAX,0))) GRANDTOTAL"
                strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),PURTAX)) PURTAX"
                strSql += vbCrLf + " ,CASE WHEN DESIGNEROUTSTANDING = 'L'  THEN SUM(CONVERT(NUMERIC(15,2),PURVALUE-ISNULL(PURTAX,0))) ELSE SUM(CONVERT(NUMERIC(15,2),PURVALUE-ISNULL(PURTAX,0))) + SUM(CONVERT(NUMERIC(15,2),PURTAX)) END AS FINALTOTAL"
                strSql += vbCrLf + " ,NULL TRANINVNO,NULL SUPBILLNO,CONVERT(VARCHAR(10),'DITEMTAG') ORDERITEM,CONVERT(VARCHAR,NULL) AS ITEM"
                strSql += vbCrLf + " , ITEMGROUP, CATEGORY,7.1 RESULT,'   'COLHEAD,NULL TAGVAL,NULL PCTFILE,ITEMID"
                strSql += vbCrLf + " ,CONVERT (VARCHAR(20),NULL)STKTYPE"
                strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),SALESAMOUNT)) SALESAMOUNT"
                strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),SALESED)) SALESED"
                strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),SALESTAX)) SALESTAX"
                strSql += vbCrLf + " ,SUM(CONVERT(NUMERIC(15,2),ISNULL(SALESAMOUNT,0)+ISNULL(SALESED,0)+ISNULL(SALESTAX,0)))SALESTOTAL"
            Else
                strSql += vbCrLf + " SELECT CONVERT(VARCHAR(60),PARTICULAR)PARTICULAR,DESIGNER,TAGNO,RECDATE,"
                If chktranno.Checked Then strSql += vbCrLf + " TRANNO,"
                strSql += vbCrLf + " STYLENO"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),PURVALUE) TOTAL"
                strSql += vbCrLf + " ,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT"
                strSql += vbCrLf + " ,ISNULL(PURVALUE,0)-ISNULL(STNAMT,0)-ISNULL(DIAAMT,0)-ISNULL(PURMC,0)-ISNULL(PURTAX,0) AS NETAMT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,4),DIAWT)DIAWT,CONVERT(NUMERIC(15,2),DIAAMT)DIAAMT,CONVERT(NUMERIC(15,3),STNWT)STNWT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),STNAMT)STNAMT,CONVERT(NUMERIC(15,2),PURMC)PURMC,CONVERT(NUMERIC(15,2)"
                strSql += vbCrLf + " ,PURVALUE-ISNULL(PURTAX,0)) GRANDTOTAL"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),PURTAX) PURTAX"
                strSql += vbCrLf + " ,CASE WHEN DESIGNEROUTSTANDING = 'L' THEN CONVERT(NUMERIC(15,2),PURVALUE-ISNULL(PURTAX,0)) ELSE CONVERT(NUMERIC(15,2),PURVALUE-ISNULL(PURTAX,0)) + CONVERT(NUMERIC(15,2),PURTAX) END AS FINALTOTAL"
                strSql += vbCrLf + " ,TRANINVNO,SUPBILLNO,CONVERT(VARCHAR(10),'DITEMTAG') ORDERITEM"
                strSql += vbCrLf + " ,ITEM,ITEMGROUP, CATEGORY,7.1 RESULT,'    'COLHEAD,CONVERT(INT,TAGVAL)TAGVAL,PCTFILE,ITEMID"
                strSql += vbCrLf + " ,CONVERT (VARCHAR(20),STKTYPE)STKTYPE"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SALESAMOUNT) SALESAMOUNT"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SALESED) SALESED"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),SALESTAX) SALESTAX"
                strSql += vbCrLf + " ,CONVERT(NUMERIC(15,2),ISNULL(SALESAMOUNT,0)+ISNULL(SALESED,0)+ISNULL(SALESTAX,0)) SALESTOTAL"
            End If
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET2"
            strSql += vbCrLf + " FROM"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SELECT CASE WHEN ISNULL(T.SUBITEM,'') <> '' THEN T.SUBITEM ELSE T.ITEM END AS PARTICULAR"
            strSql += vbCrLf + " ,T.TAGNO,T.CATEGORY,T.RECDATE,"
            If chktranno.Checked Then strSql += vbCrLf + " TRANNO,"
            strSql += vbCrLf + "T.STYLENO,T.PCS,T.TAGGRSWT,T.PURGRSWT,T.TAGNETWT,T.PURNETWT"
            strSql += vbCrLf + " ,T.PURMC,T.PURVALUE"
            strSql += vbCrLf + " ,T.TRANINVNO,T.SUPBILLNO"
            strSql += vbCrLf + " ,T.SALESDET"
            strSql += vbCrLf + " ,T.ITEM"
            strSql += vbCrLf + " ,T.ITEMGROUP"
            strSql += vbCrLf + " ,T.TAGVAL,T.PURTAX"
            strSql += vbCrLf + " ,ST.DIAWT,ST.DIAAMT,ST.STNWT,ST.STNAMT,T.PCTFILE,T.ITEMID,T.DESIGNER"
            strSql += vbCrLf + " ,(SELECT LOCALOUTST FROM " & cnAdminDb & "..ACHEAD WHERE ACCODE =( SELECT ACCODE FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = T.DESIGNER)) AS DESIGNEROUTSTANDING"
            strSql += vbCrLf + " ,T.STKTYPE"
            strSql += vbCrLf + " ,T.SALESAMOUNT,T.SALESED,T.SALESTAX"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPTAGVIEW AS T"
            strSql += vbCrLf + " LEFT OUTER JOIN TEMPTABLEDB..TEMPTAGSTONEVIEW AS ST ON ST.TAGSNO = T.SNO"
            strSql += vbCrLf + " WHERE T.STKTABLE IN('PITEMTAG')"
            strSql += vbCrLf + " )X"
            If rbtSummary.Checked = True Then
                strSql += vbCrLf + " GROUP BY ITEM,ITEMID,CATEGORY,ITEMGROUP "
                strSql += vbCrLf + ",DESIGNEROUTSTANDING"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET2"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If
        '''''''''''''''''''''''''''''''''''''END TOTAL
        'strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET)>0"
        'strSql += vbCrLf + " BEGIN"
        If rbtSummary.Checked = True And chkWithCategory.Checked = True Then
            If chkWithCategory.Checked = True Then
                strSql = vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET (PARTICULAR,CATEGORY,ORDERITEM,COLHEAD,RESULT)"
                strSql += vbCrLf + " SELECT DISTINCT CATEGORY  AS PARTICULAR,CATEGORY,'AITEMTAG','T',0 AS RESULT FROM "
                strSql += vbCrLf + " TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT=2 "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                If ChkWithTrf.Checked Then
                    strSql = vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET (PARTICULAR,CATEGORY,ORDERITEM,COLHEAD,RESULT)"
                    strSql += vbCrLf + " SELECT DISTINCT CATEGORY  AS PARTICULAR,CATEGORY,'BITEMTAG','T',6.1 AS RESULT FROM "
                    strSql += vbCrLf + " TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT=7"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
                If ChkWithPend.Checked Then
                    strSql = vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET (PARTICULAR,CATEGORY,ORDERITEM,COLHEAD,RESULT)"
                    strSql += vbCrLf + " SELECT DISTINCT CATEGORY  AS PARTICULAR,CATEGORY,'DITEMTAG','T',6.2 AS RESULT "
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT=7.1"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
            End If
        End If
        If rbtSummary.Checked = True And chkItemCategory.Checked = True Then
            If chkItemCategory.Checked = True Then
                strSql = vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET (PARTICULAR,ITEMGROUP,ORDERITEM,COLHEAD,RESULT)"
                strSql += vbCrLf + " SELECT DISTINCT ITEMGROUP  AS PARTICULAR,ITEMGROUP,'AITEMTAG','T',0 AS RESULT FROM "
                strSql += vbCrLf + " TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT=2 "
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                If ChkWithTrf.Checked Then
                    strSql = vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET (PARTICULAR,ITEMGROUP,ORDERITEM,COLHEAD,RESULT)"
                    strSql += vbCrLf + " SELECT DISTINCT ITEMGROUP AS PARTICULAR,ITEMGROUP,'BITEMTAG','T',6.1 AS RESULT FROM "
                    strSql += vbCrLf + " TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT=7"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
                If ChkWithPend.Checked Then
                    strSql = vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET (PARTICULAR,ITEMGROUP,ORDERITEM,COLHEAD,RESULT)"
                    strSql += vbCrLf + " SELECT DISTINCT ITEMGROUP  AS PARTICULAR,ITEMGROUP,'DITEMTAG','T',6.2 AS RESULT "
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT=7.1"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
            End If
        End If

        If rbtDetailed.Checked = True Then
            'GROUPBY CATEGORY
            If chkWithCategory.Checked = True Then
                strSql = vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET (PARTICULAR,CATEGORY,ORDERITEM,ITEM,COLHEAD,RESULT)"
                strSql += vbCrLf + " SELECT DISTINCT CATEGORY  AS PARTICULAR,CATEGORY,'AITEMTAG',ITEM,'T',0 AS RESULT "
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT=2"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                If ChkWithTrf.Checked Then
                    strSql = vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET (PARTICULAR,CATEGORY,ORDERITEM,ITEM,COLHEAD,RESULT)"
                    strSql += vbCrLf + " SELECT DISTINCT CATEGORY  AS PARTICULAR,CATEGORY,'BITEMTAG',ITEM,'T',6.2 AS RESULT "
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT=7"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
                If ChkWithPend.Checked Then
                    strSql = vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET (PARTICULAR,CATEGORY,ORDERITEM,ITEM,COLHEAD,RESULT)"
                    strSql += vbCrLf + " SELECT DISTINCT CATEGORY  AS PARTICULAR,CATEGORY,'BITEMTAG',ITEM,'T',6.3 AS RESULT"
                    strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT=7.1"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
            End If
            If chkWithCategory.Checked = True Then
                strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(PARTICULAR,CATEGORY,ORDERITEM,ITEM,RESULT,COLHEAD)"
                strSql += vbCrLf + " SELECT DISTINCT ITEM,CATEGORY,'AITEMTAG',ITEM,1 RESULT,'T1'COLHEAD FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET"
                strSql += vbCrLf + "  WHERE RESULT = 2"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                If ChkWithTrf.Checked Then
                    strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(PARTICULAR,CATEGORY,ORDERITEM,ITEM,RESULT,COLHEAD)"
                    strSql += vbCrLf + " SELECT DISTINCT ITEM,CATEGORY,'BITEMTAG',ITEM,7 RESULT,'T1'COLHEAD FROM "
                    strSql += vbCrLf + " TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 7"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
                If ChkWithPend.Checked = True Then
                    strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(PARTICULAR,CATEGORY,ORDERITEM,ITEM,RESULT,COLHEAD)"
                    strSql += vbCrLf + " SELECT DISTINCT ITEM,CATEGORY,'DITEMTAG',ITEM,7.1 RESULT,'T1'COLHEAD "
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 7.1"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
            End If
            If chkWithCategory.Checked = True Then
                strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,CATEGORY,ITEM,PARTICULAR,RESULT,"
                strSql += vbCrLf + " COLHEAD,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
                strSql += vbCrLf + " SELECT DISTINCT 'AITEMTAG',CATEGORY,item,'SUB TOTAL',3 RESULT,'S'COLHEAD"
                strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),SUM(DIAWT),"
                strSql += vbCrLf + " SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET  WHERE RESULT = 2 GROUP BY CATEGORY,ITEM"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                If ChkWithTrf.Checked Then
                    strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,CATEGORY,ITEM,PARTICULAR,"
                    strSql += vbCrLf + " RESULT,COLHEAD,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,"
                    strSql += vbCrLf + " STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
                    strSql += vbCrLf + " SELECT DISTINCT 'BITEMTAG',CATEGORY,item,'SUB TOTAL',9 RESULT,'S'COLHEAD"
                    strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),SUM(DIAWT),"
                    strSql += vbCrLf + " SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL), SUM(FINALTOTAL),SUM(PURTAX)"
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET  WHERE RESULT = 7 GROUP BY CATEGORY,ITEM"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
                If ChkWithPend.Checked Then
                    strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,CATEGORY,ITEM,PARTICULAR,"
                    strSql += vbCrLf + " RESULT,COLHEAD,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,"
                    strSql += vbCrLf + " PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
                    strSql += vbCrLf + " SELECT DISTINCT 'DITEMTAG',CATEGORY,item,'SUB TOTAL',9.1 RESULT,'S'COLHEAD"
                    strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),SUM(DIAWT),"
                    strSql += vbCrLf + " SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET  WHERE RESULT = 7.1 GROUP BY CATEGORY,ITEM"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
            End If
            'GROUP BY ITEMGROUP
            If chkItemCategory.Checked = True Then
                strSql = vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET (PARTICULAR,ITEMGROUP,ORDERITEM,ITEM,COLHEAD,RESULT)"
                strSql += vbCrLf + " SELECT DISTINCT ITEMGROUP  AS PARTICULAR,ITEMGROUP,'AITEMTAG',ITEM,'T',0 AS RESULT "
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT=2"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                If ChkWithTrf.Checked Then
                    strSql = vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET (PARTICULAR,ITEMGROUP,ORDERITEM,ITEM,COLHEAD,RESULT)"
                    strSql += vbCrLf + " SELECT DISTINCT ITEMGROUP  AS PARTICULAR,ITEMGROUP,'BITEMTAG',ITEM,'T',6.2 AS RESULT "
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT=7"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
                If ChkWithPend.Checked Then
                    strSql = vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET (PARTICULAR,ITEMGROUP,ORDERITEM,ITEM,COLHEAD,RESULT)"
                    strSql += vbCrLf + " SELECT DISTINCT ITEMGROUP  AS PARTICULAR,ITEMGROUP,'BITEMTAG',ITEM,'T',6.3 AS RESULT"
                    strSql += vbCrLf + "  FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT=7.1"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
            End If
            If chkItemCategory.Checked = True Then
                strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(PARTICULAR,ITEMGROUP,ORDERITEM,ITEM,RESULT,COLHEAD)"
                strSql += vbCrLf + " SELECT DISTINCT ITEM,ITEMGROUP,'AITEMTAG',ITEM,1 RESULT,'T1'COLHEAD FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET"
                strSql += vbCrLf + "  WHERE RESULT = 2"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                If ChkWithTrf.Checked Then
                    strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(PARTICULAR,ITEMGROUP,ORDERITEM,ITEM,RESULT,COLHEAD)"
                    strSql += vbCrLf + " SELECT DISTINCT ITEM,ITEMGROUP,'BITEMTAG',ITEM,7 RESULT,'T1'COLHEAD FROM "
                    strSql += vbCrLf + " TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 7"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
                If ChkWithPend.Checked = True Then
                    strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(PARTICULAR,ITEMGROUP,ORDERITEM,ITEM,RESULT,COLHEAD)"
                    strSql += vbCrLf + " SELECT DISTINCT ITEM,ITEMGROUP,'DITEMTAG',ITEM,7.1 RESULT,'T1'COLHEAD "
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 7.1"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
            End If
            If chkItemCategory.Checked = True Then
                strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEMGROUP,ITEM,PARTICULAR,RESULT,"
                strSql += vbCrLf + " COLHEAD,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
                strSql += vbCrLf + " SELECT DISTINCT 'AITEMTAG',ITEMGROUP,ITEM,'SUB TOTAL',3 RESULT,'S'COLHEAD"
                strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),SUM(DIAWT),"
                strSql += vbCrLf + " SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET  WHERE RESULT = 2 GROUP BY ITEMGROUP,ITEM"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                If ChkWithTrf.Checked Then
                    strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEMGROUP,ITEM,PARTICULAR,"
                    strSql += vbCrLf + " RESULT,COLHEAD,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,"
                    strSql += vbCrLf + " STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
                    strSql += vbCrLf + " SELECT DISTINCT 'BITEMTAG',ITEMGROUP,ITEM,'SUB TOTAL',9 RESULT,'S'COLHEAD"
                    strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),SUM(DIAWT),"
                    strSql += vbCrLf + " SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET  WHERE RESULT = 7 GROUP BY ITEMGROUP,ITEM"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
                If ChkWithPend.Checked Then
                    strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEMGROUP,ITEM,PARTICULAR,"
                    strSql += vbCrLf + " RESULT,COLHEAD,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,"
                    strSql += vbCrLf + " PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
                    strSql += vbCrLf + " SELECT DISTINCT 'DITEMTAG',ITEMGROUP,ITEM,'SUB TOTAL',9.1 RESULT,'S'COLHEAD"
                    strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),SUM(DIAWT),"
                    strSql += vbCrLf + " SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL), SUM(FINALTOTAL),SUM(PURTAX)"
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET  WHERE RESULT = 7.1 GROUP BY ITEMGROUP,ITEM"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
                'DETAILS CATEGORY SUB TOTAL
                If chkWithCategory.Checked = True Then
                    strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEM,CATEGORY,PARTICULAR,RESULT,"
                    strSql += vbCrLf + " COLHEAD,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
                    strSql += vbCrLf + " SELECT DISTINCT 'AITEMTAG',ITEM,CATEGORY,'SUB TOTAL',4 RESULT,'S1'COLHEAD,"
                    strSql += vbCrLf + " SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),SUM(DIAWT),SUM(DIAAMT),"
                    strSql += vbCrLf + " SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 2 GROUP BY ITEM,CATEGORY"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                    If ChkWithTrf.Checked Then
                        strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEM,CATEGORY,PARTICULAR,RESULT,"
                        strSql += vbCrLf + " COLHEAD,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
                        strSql += vbCrLf + " SELECT DISTINCT 'BITEMTAG',ITEM,CATEGORY,'SUB TOTAL',9 RESULT,'S1'COLHEAD,"
                        strSql += vbCrLf + " SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),"
                        strSql += vbCrLf + " SUM(DIAWT),SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 7 GROUP BY ITEM,CATEGORY"
                        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                        cmd.ExecuteNonQuery()
                    End If
                    If ChkWithPend.Checked = True Then
                        strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEM,"
                        strSql += vbCrLf + " CATEGORY,PARTICULAR,RESULT,COLHEAD,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,"
                        strSql += vbCrLf + " PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
                        strSql += vbCrLf + " SELECT DISTINCT 'DITEMTAG',ITEM,CATEGORY,'SUB TOTAL',9.1 RESULT,'S1'COLHEAD,"
                        strSql += vbCrLf + " SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),"
                        strSql += vbCrLf + " SUM(DIAWT),SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 7.1 GROUP BY ITEM,CATEGORY"
                        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                        cmd.ExecuteNonQuery()
                    End If
                End If
                'DETAILS ITEMGROUP SUB TOTAL
                If chkItemCategory.Checked = True Then
                    strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEM,ITEMGROUP,PARTICULAR,RESULT,"
                    strSql += vbCrLf + " COLHEAD,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
                    strSql += vbCrLf + " SELECT DISTINCT 'AITEMTAG',ITEM,ITEMGROUP,'SUB TOTAL',4 RESULT,'S1'COLHEAD,"
                    strSql += vbCrLf + " SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),SUM(DIAWT),SUM(DIAAMT),"
                    strSql += vbCrLf + " SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL), SUM(FINALTOTAL),SUM(PURTAX)"
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 2 GROUP BY ITEM,ITEMGROUP"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                    If ChkWithTrf.Checked Then
                        strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEM,ITEMGROUP,PARTICULAR,RESULT,"
                        strSql += vbCrLf + " COLHEAD,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
                        strSql += vbCrLf + " SELECT DISTINCT 'BITEMTAG',ITEM,ITEMGROUP,'SUB TOTAL',9 RESULT,'S1'COLHEAD,"
                        strSql += vbCrLf + " SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),"
                        strSql += vbCrLf + " SUM(DIAWT),SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 7 GROUP BY ITEM,ITEMGROUP"
                        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                        cmd.ExecuteNonQuery()
                    End If
                    If ChkWithPend.Checked = True Then
                        strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEM,"
                        strSql += vbCrLf + " ITEMGROUP,PARTICULAR,RESULT,COLHEAD,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,"
                        strSql += vbCrLf + " PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
                        strSql += vbCrLf + " SELECT DISTINCT 'DITEMTAG',ITEM,ITEMGROUP,'SUB TOTAL',9.1 RESULT,'S1'COLHEAD,"
                        strSql += vbCrLf + " SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),"
                        strSql += vbCrLf + " SUM(DIAWT),SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 7.1 GROUP BY ITEM,ITEMGROUP"
                        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                        cmd.ExecuteNonQuery()
                    End If
                End If
            End If
        End If
        'SUBITEMTOTAL CATEGORY
        If rbtSummary.Checked = True And chkWithCategory.Checked = True Then
            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,CATEGORY,ITEM,"
            strSql += vbCrLf + " PARTICULAR,RESULT,COLHEAD,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,"
            strSql += vbCrLf + " NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
            strSql += vbCrLf + " SELECT DISTINCT 'AITEMTAG',CATEGORY,ITEM,'SUB TOTAL',3 RESULT,'S'COLHEAD"
            strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT)"
            strSql += vbCrLf + " ,SUM(DIAWT),SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET  WHERE RESULT = 2 GROUP BY CATEGORY,ITEM"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            If ChkWithTrf.Checked Then
                strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,CATEGORY,ITEM,PARTICULAR,RESULT,"
                strSql += vbCrLf + " COLHEAD,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
                strSql += vbCrLf + " SELECT DISTINCT 'BITEMTAG',CATEGORY,ITEM,'SUB TOTAL',8 RESULT,'S'COLHEAD"
                strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),SUM(DIAWT),SUM(DIAAMT),"
                strSql += vbCrLf + " SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET  WHERE RESULT = 7 GROUP BY CATEGORY,ITEM"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If
            If ChkWithPend.Checked Then
                strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,CATEGORY,ITEM,PARTICULAR,RESULT,COLHEAD,"
                strSql += vbCrLf + " PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
                strSql += vbCrLf + " SELECT DISTINCT 'DITEMTAG',CATEGORY,ITEM,'SUB TOTAL',8.1 RESULT,'S'COLHEAD"
                strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),SUM(DIAWT),"
                strSql += vbCrLf + " SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET  WHERE RESULT = 7.1 GROUP BY CATEGORY,ITEM"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If
        End If
        'SUBITEM TOTAL ITEMGROUP
        If rbtSummary.Checked = True And chkItemCategory.Checked = True Then
            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEMGROUP,ITEM,"
            strSql += vbCrLf + " PARTICULAR,RESULT,COLHEAD,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,"
            strSql += vbCrLf + " NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
            strSql += vbCrLf + " SELECT DISTINCT 'AITEMTAG',ITEMGROUP,ITEM,'SUB TOTAL',3 RESULT,'S'COLHEAD"
            strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT)"
            strSql += vbCrLf + " ,SUM(DIAWT),SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET  WHERE RESULT = 2 GROUP BY ITEMGROUP,ITEM"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            If ChkWithTrf.Checked Then
                strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEMGROUP,ITEM,PARTICULAR,RESULT,"
                strSql += vbCrLf + " COLHEAD,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
                strSql += vbCrLf + " SELECT DISTINCT 'BITEMTAG',ITEMGROUP,ITEM,'SUB TOTAL',8 RESULT,'S'COLHEAD"
                strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),SUM(DIAWT),SUM(DIAAMT),"
                strSql += vbCrLf + " SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET  WHERE RESULT = 7 GROUP BY ITEMGROUP,ITEM"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If
            If ChkWithPend.Checked Then
                strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEMGROUP,ITEM,PARTICULAR,RESULT,COLHEAD,"
                strSql += vbCrLf + " PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
                strSql += vbCrLf + " SELECT DISTINCT 'DITEMTAG',ITEMGROUP,ITEM,'SUB TOTAL',8.1 RESULT,'S'COLHEAD"
                strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),SUM(DIAWT),"
                strSql += vbCrLf + " SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET  WHERE RESULT = 7.1 GROUP BY ITEMGROUP,ITEM"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If
        End If
        'CATEGORY TOTAL ONLY'
        If chkWithCategory.Checked = True Then
            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,CATEGORY,PARTICULAR,RESULT,"
            strSql += vbCrLf + " COLHEAD,TOTAL,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
            strSql += vbCrLf + " SELECT DISTINCT 'AITEMTAG','YYYY','CATEGORY TOTAL',5 RESULT,'G'COLHEAD,"
            strSql += vbCrLf + " SUM(TOTAL)TOTAL,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),SUM(DIAWT),"
            strSql += vbCrLf + " SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 2"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            If ChkWithTrf.Checked Then
                strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,CATEGORY,PARTICULAR,RESULT,COLHEAD,"
                strSql += vbCrLf + " TOTAL,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
                strSql += vbCrLf + " SELECT DISTINCT 'BITEMTAG','YYYY','CATEGORY TOTAL',8.5 RESULT,'G'COLHEAD,"
                strSql += vbCrLf + " SUM(TOTAL)TOTAL,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT)"
                strSql += vbCrLf + " ,SUM(DIAWT),SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 7"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If
            If ChkWithPend.Checked Then
                strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,CATEGORY,PARTICULAR,RESULT,"
                strSql += vbCrLf + " COLHEAD,TOTAL,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
                strSql += vbCrLf + " SELECT DISTINCT 'DITEMTAG','YYYY','CATEGORY TOTAL',8.6 RESULT,'G'COLHEAD,"
                strSql += vbCrLf + " SUM(TOTAL)TOTAL,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),"
                strSql += vbCrLf + " SUM(NETAMT),SUM(DIAWT),SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 7.1"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If
        End If
        ' ITEMGROUP TOTAL ONLY
        If chkItemCategory.Checked = True Then
            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEMGROUP,PARTICULAR,RESULT,"
            strSql += vbCrLf + " COLHEAD,TOTAL,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
            strSql += vbCrLf + " SELECT DISTINCT 'AITEMTAG','YYYY','ITEMGROUP TOTAL',5 RESULT,'G'COLHEAD,"
            strSql += vbCrLf + " SUM(TOTAL)TOTAL,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),SUM(DIAWT),"
            strSql += vbCrLf + " SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 2"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            If ChkWithTrf.Checked Then
                strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEMGROUP,PARTICULAR,RESULT,COLHEAD,"
                strSql += vbCrLf + " TOTAL,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
                strSql += vbCrLf + " SELECT DISTINCT 'BITEMTAG','YYYY','ITEMGROUP TOTAL',8.5 RESULT,'G'COLHEAD,"
                strSql += vbCrLf + " SUM(TOTAL)TOTAL,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT)"
                strSql += vbCrLf + " ,SUM(DIAWT),SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 7"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If
            If ChkWithPend.Checked Then
                strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEMGROUP,PARTICULAR,RESULT,"
                strSql += vbCrLf + " COLHEAD,TOTAL,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL, FINALTOTAL,PURTAX)"
                strSql += vbCrLf + " SELECT DISTINCT 'DITEMTAG','YYYY','ITEMGROUP TOTAL',8.6 RESULT,'G'COLHEAD,"
                strSql += vbCrLf + " SUM(TOTAL)TOTAL,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),"
                strSql += vbCrLf + " SUM(NETAMT),SUM(DIAWT),SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 7.1"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If
        End If
        ' CATEGORY GRAND TOTAL
        If chkWithCategory.Checked = True Then
            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEM,CATEGORY,"
            strSql += vbCrLf + " PARTICULAR,RESULT,COLHEAD,TOTAL,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,"
            strSql += vbCrLf + " STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
            strSql += vbCrLf + " SELECT DISTINCT 'AITEMTAG','ZZZZZ','ZZZZ','GRAND TOTAL',6 RESULT,'G'COLHEAD,"
            strSql += vbCrLf + " SUM(TOTAL)TOTAL,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),SUM(DIAWT),"
            strSql += vbCrLf + " SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 2"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            If ChkWithTrf.Checked Then
                strSql = vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET (PARTICULAR,ORDERITEM,CATEGORY,COLHEAD,RESULT)"
                strSql += vbCrLf + " SELECT   ' TAGTRANSIT 'AS PARTICULAR,'BITEMTAG','','T',6.5 AS RESULT "
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEM,CATEGORY,PARTICULAR,RESULT,COLHEAD,"
                strSql += vbCrLf + " TOTAL,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
                strSql += vbCrLf + " SELECT DISTINCT 'BITEMTAG','ZZZZZ','ZZZZ','GRAND TOTAL',10 RESULT,'G'COLHEAD,"
                strSql += vbCrLf + " SUM(TOTAL)TOTAL,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),SUM(DIAWT),"
                strSql += vbCrLf + " SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 7"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If
            If ChkWithPend.Checked Then
                strSql = vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET (PARTICULAR,ORDERITEM,CATEGORY,COLHEAD,RESULT)"
                strSql += vbCrLf + " SELECT   ' PENDING TAG 'AS PARTICULAR,'DITEMTAG','','T',6.6 AS RESULT "
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEM,CATEGORY,PARTICULAR,RESULT,"
                strSql += vbCrLf + " COLHEAD,TOTAL,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL, PURTAX)"
                strSql += vbCrLf + " SELECT DISTINCT 'DITEMTAG','ZZZZZ','ZZZZ','GRAND TOTAL',10.1 RESULT,'G'COLHEAD,"
                strSql += vbCrLf + " SUM(TOTAL)TOTAL,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),"
                strSql += vbCrLf + " SUM(NETAMT),SUM(DIAWT),SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 7.1"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If
        End If
        'ITEMCATEORY GRAND TOTAL
        If chkItemCategory.Checked = True Then
            strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEM,ITEMGROUP,"
            strSql += vbCrLf + " PARTICULAR,RESULT,COLHEAD,TOTAL,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,"
            strSql += vbCrLf + " STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL, PURTAX)"
            strSql += vbCrLf + " SELECT DISTINCT 'AITEMTAG','ZZZZZ','ZZZZ','GRAND TOTAL',6 RESULT,'G'COLHEAD,"
            strSql += vbCrLf + " SUM(TOTAL)TOTAL,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),SUM(DIAWT),"
            strSql += vbCrLf + " SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 2"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            If ChkWithTrf.Checked Then
                strSql = vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET (PARTICULAR,ORDERITEM,ITEMGROUP,COLHEAD,RESULT)"
                strSql += vbCrLf + " SELECT   ' TAGTRANSIT 'AS PARTICULAR,'BITEMTAG','','T',6.5 AS RESULT "
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEM,ITEMGROUP,PARTICULAR,RESULT,COLHEAD,"
                strSql += vbCrLf + " TOTAL,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL ,FINALTOTAL,PURTAX)"
                strSql += vbCrLf + " SELECT DISTINCT 'BITEMTAG','ZZZZZ','ZZZZ','GRAND TOTAL',10 RESULT,'G'COLHEAD,"
                strSql += vbCrLf + " SUM(TOTAL)TOTAL,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),SUM(DIAWT),"
                strSql += vbCrLf + " SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 7"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If
            If ChkWithPend.Checked Then
                strSql = vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET (PARTICULAR,ORDERITEM,ITEMGROUP,COLHEAD,RESULT)"
                strSql += vbCrLf + " SELECT   ' PENDING TAG 'AS PARTICULAR,'DITEMTAG','','T',6.6 AS RESULT "
                strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEM,ITEMGROUP,PARTICULAR,RESULT,"
                strSql += vbCrLf + " COLHEAD,TOTAL,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL ,FINALTOTAL,PURTAX)"
                strSql += vbCrLf + " SELECT DISTINCT 'DITEMTAG','ZZZZZ','ZZZZ','GRAND TOTAL',10.1 RESULT,'G'COLHEAD,"
                strSql += vbCrLf + " SUM(TOTAL)TOTAL,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),"
                strSql += vbCrLf + " SUM(NETAMT),SUM(DIAWT),SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 7.1"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If
        End If
        'WITH NONE 
        If chkNone.Checked = True Or (chkWithCategory.Checked = False And chkItemCategory.Checked = False And chkNone.Checked = False) Then
            If rbtSummary.Checked = True Then
                strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEM,ITEMGROUP,"
                strSql += vbCrLf + " PARTICULAR,RESULT,COLHEAD,TOTAL,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,"
                strSql += vbCrLf + " STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX"
                strSql += vbCrLf + " ,SALESAMOUNT,SALESED,SALESTAX,SALESTOTAL)"
                strSql += vbCrLf + " SELECT DISTINCT 'AITEMTAG','ZZZZZ','ZZZZ','GRAND TOTAL',6 RESULT,'G'COLHEAD,"
                strSql += vbCrLf + " SUM(TOTAL)TOTAL,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),SUM(DIAWT),"
                strSql += vbCrLf + " SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                strSql += vbCrLf + " ,SUM(SALESAMOUNT),SUM(SALESED),SUM(SALESTAX),SUM(SALESTOTAL)"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 2"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                'GRAND TOTAL
                If ChkWithTrf.Checked Then
                    strSql = vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET (PARTICULAR,ORDERITEM,ITEMGROUP,COLHEAD,RESULT)"
                    strSql += vbCrLf + " SELECT   ' TAGTRANSIT 'AS PARTICULAR,'BITEMTAG','','T',6.5 AS RESULT "
                    strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEM,ITEMGROUP,PARTICULAR,RESULT,COLHEAD,"
                    strSql += vbCrLf + " TOTAL,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
                    strSql += vbCrLf + " SELECT DISTINCT 'BITEMTAG','ZZZZZ','ZZZZ','GRAND TOTAL',10 RESULT,'G'COLHEAD,"
                    strSql += vbCrLf + " SUM(TOTAL)TOTAL,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),SUM(DIAWT),"
                    strSql += vbCrLf + " SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 7"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
                'GRAND TOTAL
                If ChkWithPend.Checked Then
                    strSql = vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET (PARTICULAR,ORDERITEM,ITEMGROUP,COLHEAD,RESULT)"
                    strSql += vbCrLf + " SELECT   ' PENDING TAG 'AS PARTICULAR,'DITEMTAG','','T',6.6 AS RESULT "
                    strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEM,ITEMGROUP,PARTICULAR,RESULT,"
                    strSql += vbCrLf + " COLHEAD,TOTAL,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
                    strSql += vbCrLf + " SELECT DISTINCT 'DITEMTAG','ZZZZZ','ZZZZ','GRAND TOTAL',10.1 RESULT,'G'COLHEAD,"
                    strSql += vbCrLf + " SUM(TOTAL)TOTAL,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),"
                    strSql += vbCrLf + " SUM(NETAMT),SUM(DIAWT),SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 7.1"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
            ElseIf rbtDetailed.Checked = True Then
                'HEADLINE
                strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(PARTICULAR,CATEGORY,ORDERITEM,ITEM,RESULT,COLHEAD)"
                strSql += vbCrLf + " SELECT DISTINCT ITEM,CATEGORY,'AITEMTAG',ITEM,1 RESULT,'T1'COLHEAD FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET"
                strSql += vbCrLf + "  WHERE RESULT = 2"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                If ChkWithTrf.Checked Then
                    strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(PARTICULAR,CATEGORY,ORDERITEM,ITEM,RESULT,COLHEAD)"
                    strSql += vbCrLf + " SELECT DISTINCT ITEM,CATEGORY,'BITEMTAG',ITEM,7 RESULT,'T1'COLHEAD FROM "
                    strSql += vbCrLf + " TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 7"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
                If ChkWithPend.Checked = True Then
                    strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(PARTICULAR,CATEGORY,ORDERITEM,ITEM,RESULT,COLHEAD)"
                    strSql += vbCrLf + " SELECT DISTINCT ITEM,CATEGORY,'DITEMTAG',ITEM,7.1 RESULT,'T1'COLHEAD "
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 7.1"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
                strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,CATEGORY,ITEM,PARTICULAR,RESULT,"
                strSql += vbCrLf + " COLHEAD,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX"
                strSql += vbCrLf + " ,SALESAMOUNT,SALESED,SALESTAX,SALESTOTAL)"
                strSql += vbCrLf + " SELECT DISTINCT 'AITEMTAG',CATEGORY,ITEM,'SUB TOTAL',3 RESULT,'S'COLHEAD"
                strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),SUM(DIAWT),"
                strSql += vbCrLf + " SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                strSql += vbCrLf + " ,SUM(SALESAMOUNT),SUM(SALESED),SUM(SALESTAX),SUM(SALESTOTAL)"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET  WHERE RESULT = 2 GROUP BY CATEGORY,ITEM"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                If ChkWithTrf.Checked Then
                    strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,CATEGORY,ITEM,PARTICULAR,"
                    strSql += vbCrLf + " RESULT,COLHEAD,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,"
                    strSql += vbCrLf + " STNWT,STNAMT,PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
                    strSql += vbCrLf + " SELECT DISTINCT 'BITEMTAG',CATEGORY,item,'SUB TOTAL',9 RESULT,'S'COLHEAD"
                    strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),SUM(DIAWT),"
                    strSql += vbCrLf + " SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET  WHERE RESULT = 7 GROUP BY CATEGORY,ITEM"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
                If ChkWithPend.Checked Then
                    strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,CATEGORY,ITEM,PARTICULAR,"
                    strSql += vbCrLf + " RESULT,COLHEAD,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,"
                    strSql += vbCrLf + " PURMC,GRANDTOTAL,FINALTOTAL,PURTAX)"
                    strSql += vbCrLf + " SELECT DISTINCT 'DITEMTAG',CATEGORY,item,'SUB TOTAL',9.1 RESULT,'S'COLHEAD"
                    strSql += vbCrLf + " ,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),SUM(DIAWT),"
                    strSql += vbCrLf + " SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET  WHERE RESULT = 7.1 GROUP BY CATEGORY,ITEM"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
                strSql = vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEM,ITEMGROUP,"
                strSql += vbCrLf + " PARTICULAR,RESULT,COLHEAD,TOTAL,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,"
                strSql += vbCrLf + " STNWT,STNAMT,PURMC,GRANDTOTAL ,FINALTOTAL,PURTAX"
                strSql += vbCrLf + " ,SALESAMOUNT,SALESED,SALESTAX,SALESTOTAL)"
                strSql += vbCrLf + " SELECT DISTINCT 'AITEMTAG','ZZZZZ','ZZZZ','GRAND TOTAL',6 RESULT,'G'COLHEAD,"
                strSql += vbCrLf + " SUM(TOTAL)TOTAL,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),SUM(DIAWT),"
                strSql += vbCrLf + " SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                strSql += vbCrLf + " ,SUM(SALESAMOUNT),SUM(SALESED),SUM(SALESTAX),SUM(SALESTOTAL)"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 2"
                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
                'GRAND TOTAL
                If ChkWithTrf.Checked Then
                    strSql = vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET (PARTICULAR,ORDERITEM,ITEMGROUP,COLHEAD,RESULT)"
                    strSql += vbCrLf + " SELECT ' TAGTRANSIT 'AS PARTICULAR,'BITEMTAG','','T',6.5 AS RESULT "
                    strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEM,ITEMGROUP,PARTICULAR,RESULT,COLHEAD,"
                    strSql += vbCrLf + " TOTAL,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL ,FINALTOTAL,PURTAX)"
                    strSql += vbCrLf + " SELECT DISTINCT 'BITEMTAG','ZZZZZ','ZZZZ','GRAND TOTAL',10 RESULT,'G'COLHEAD,"
                    strSql += vbCrLf + " SUM(TOTAL)TOTAL,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),SUM(NETAMT),SUM(DIAWT),"
                    strSql += vbCrLf + " SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 7"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
                'GRAND TOTAL
                If ChkWithPend.Checked Then
                    strSql = vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET (PARTICULAR,ORDERITEM,ITEMGROUP,COLHEAD,RESULT)"
                    strSql += vbCrLf + " SELECT ' PENDING TAG 'AS PARTICULAR,'DITEMTAG','','T',6.6 AS RESULT "
                    strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "TAGPURDET(ORDERITEM,ITEM,ITEMGROUP,PARTICULAR,RESULT,"
                    strSql += vbCrLf + " COLHEAD,TOTAL,PCS,TAGGRSWT,PURGRSWT,TAGNETWT,PURNETWT,NETAMT,DIAWT,DIAAMT,STNWT,STNAMT,PURMC,GRANDTOTAL ,FINALTOTAL,PURTAX)"
                    strSql += vbCrLf + " SELECT DISTINCT 'DITEMTAG','ZZZZZ','ZZZZ','GRAND TOTAL',10.1 RESULT,'G'COLHEAD,"
                    strSql += vbCrLf + " SUM(TOTAL)TOTAL,SUM(PCS)PCS,SUM(TAGGRSWT),SUM(PURGRSWT),SUM(TAGNETWT),SUM(PURNETWT),"
                    strSql += vbCrLf + " SUM(NETAMT),SUM(DIAWT),SUM(DIAAMT),SUM(STNWT),SUM(STNAMT),SUM(PURMC),SUM(GRANDTOTAL),SUM(FINALTOTAL),SUM(PURTAX)"
                    strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET WHERE RESULT = 7.1"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                    cmd.ExecuteNonQuery()
                End If
            End If
        End If
        'strSql += vbCrLf + " END"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()
        If chkWithCategory.Checked = True Then
            strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET ORDER BY ORDERITEM,CATEGORY,ITEM,RESULT,TAGVAL"
        ElseIf chkItemCategory.Checked = True Then
            strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET ORDER BY ORDERITEM,ITEMGROUP,ITEM,RESULT,TAGVAL"
        Else
            strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "TAGPURDET ORDER BY ORDERITEM,ITEM,RESULT,TAGVAL"
        End If
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        dtGrid.Columns.Add("TAGIMAGE", GetType(Image))
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        dtGrid.Columns("TAGIMAGE").SetOrdinal(2)
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Size = New Size(778, 550)
        objGridShower.Text = "TAGED ITEMS DETAILED PURCHASE REPORT "
        Dim tit As String = "TAGED ITEMS DETAILED PURCHASE REPORT " + IIf(chkMetalName <> "", chkMetalName.Replace("'", ""), " ALL METAL") + vbCrLf
        'tit += "AS ON DATE  " + dtpAsOnDate.Text
        tit += "BETWEEN  " + dtpAsOnDate.Text + " To " + dtpTo.Text
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
        'objGridShower.gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        AddHandler objGridShower.gridView.KeyPress, AddressOf GridView_KeyPress
        objGridShower.pnlFooter.Visible = False
        objGridShower.FormReLocation = False
        objGridShower.FormReSize = False
        objGridShower.Show()
        If chkWithImage.Checked And chkWithImage.Visible Then
            Dim fileDestPath As String = Nothing
            Dim picBox As New PictureBox
            picBox.Size = New Size(50, 50)
            For Each ro As DataGridViewRow In objGridShower.gridView.Rows
                If ro.Cells("PCTFILE").Value.ToString = "" Then
                    ro.Cells("TAGIMAGE").Value = My.Resources.EmptyImage
                    Continue For
                End If
                fileDestPath = defaultPic & ro.Cells("PCTFILE").Value.ToString
                If IO.File.Exists(fileDestPath) Then
                    AutoImageSizer(fileDestPath, picBox, PictureBoxSizeMode.StretchImage)
                    ro.Cells("TAGIMAGE").Value = picBox.Image
                    ro.Resizable = DataGridViewTriState.True
                    ro.Height = 50
                Else
                    ro.Cells("TAGIMAGE").Value = My.Resources.EmptyImage
                End If
            Next
        End If
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        DataGridView_Detailed(objGridShower.gridView)
        objGridShower.FormReSize = True
        objGridShower.WindowState = FormWindowState.Maximized
        objGridShower.gridViewHeader.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle = objGridShower.gridView.ColumnHeadersDefaultCellStyle
        Prop_Sets()
    End Sub
    Private Sub DataGridView_Detailed(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .Columns("PARTICULAR").Width = 200
            .Columns("TAGNO").Width = 75
            .Columns("STYLENO").Width = 60
            .Columns("TOTAL").Width = 100
            .Columns("PCS").Width = 70
            .Columns("PURNETWT").Width = 80
            .Columns("NETAMT").Width = 90
            .Columns("DIAWT").Width = 80
            .Columns("DIAAMT").Width = 100
            .Columns("STNWT").Width = 80
            .Columns("STNAMT").Width = 100
            .Columns("PURMC").Width = 80
            .Columns("GRANDTOTAL").Width = 100
            .Columns("FINALTOTAL").Width = 100
            .Columns("PURTAX").Width = 70
            .Columns("TRANINVNO").Width = 100
            .Columns("SUPBILLNO").Width = 100
            .Columns("TAGIMAGE").Width = 100
            .Columns("CATEGORY").Visible = False
            .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            If chkWithImage.Checked And chkWithImage.Visible Then
                .Columns("TAGIMAGE").Visible = True
            Else
                .Columns("TAGIMAGE").Visible = False
            End If
            If GrpStkType.Enabled = False Then
                If .Columns.Contains("STKTYPE") Then .Columns("STKTYPE").Visible = False
            End If
            If .Columns.Contains("SALESAMOUNT") Then .Columns("SALESAMOUNT").HeaderText = "AMOUNT"
            If .Columns.Contains("SALESED") Then .Columns("SALESED").HeaderText = "ED"
            If .Columns.Contains("SALESTAX") Then .Columns("SALESTAX").HeaderText = "TAX"
            If .Columns.Contains("SALESTOTAL") Then .Columns("SALESTOTAL").HeaderText = "TOTAL"
            If .Columns.Contains("SALESAMOUNT") And rbtIssue.Checked = False Then .Columns("SALESAMOUNT").Visible = False
            If .Columns.Contains("SALESED") And rbtIssue.Checked = False Then .Columns("SALESED").Visible = False
            If .Columns.Contains("SALESTAX") And rbtIssue.Checked = False Then .Columns("SALESTAX").Visible = False
            If .Columns.Contains("SALESTOTAL") And rbtIssue.Checked = False Then .Columns("SALESTOTAL").Visible = False
            .Columns("DESIGNER").Visible = rbtDetailed.Checked
            .Columns("STYLENO").Visible = rbtDetailed.Checked
            .Columns("TAGNO").Visible = Not rbtSummary.Checked
            .Columns("STYLENO").Visible = Not rbtSummary.Checked
            .Columns("TRANINVNO").Visible = Not rbtSummary.Checked
            .Columns("SUPBILLNO").Visible = Not rbtSummary.Checked
            .Columns("PURNETWT").Visible = False
            .Columns("PURGRSWT").Visible = False
            .Columns("PCTFILE").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("ITEMGROUP").Visible = False
            .Columns("ITEM").Visible = False
            .Columns("KEYNO").Visible = False
            .Columns("TAGVAL").Visible = False
            .Columns("ITEMID").Visible = False
            .Columns("ORDERITEM").Visible = False

            .Columns("GRANDTOTAL").HeaderText = "PURVALUES"
            .Columns("FINALTOTAL").HeaderText = "TOTALAMOUNT"
            If .Columns.Contains("RECDATE") = True Then .Columns("RECDATE").DefaultCellStyle.Format = "dd/MM/yyyy"
            FormatGridColumns(dgv, False, False, , False)
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
        chkWithImage.Visible = rbtDetailed.Checked
    End Sub
    Private Sub GridView_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If e.KeyChar = Chr(Keys.Enter) Then
            With objGridShower
                If Not .gridView.RowCount > 0 Then Exit Sub
                If .gridView.CurrentRow Is Nothing Then Exit Sub
                If .gridView.CurrentRow.Cells("ITEMID").Value.ToString = "" Then Exit Sub
                If .gridView.CurrentRow.Cells("TAGNO").Value.ToString = "" Then Exit Sub
                Dim objTagViewer As New frmTagImageViewer( _
                 .gridView.CurrentRow.Cells("TAGNO").Value.ToString, _
                 Val(.gridView.CurrentRow.Cells("ITEMID").Value.ToString), _
                 BrighttechPack.Methods.GetRights(_DtUserRights, frmTagCheck.Name, BrighttechPack.Methods.RightMode.Authorize, False))
                objTagViewer.ShowDialog()
            End With
        Else
            With objGridShower
                'If UCase(e.KeyChar) = "D" Then
                If UCase(e.KeyChar) = "D" And .gridView.CurrentRow.Cells("TAGNO").Value.ToString <> "" Then
                    DetailedView(objGridShower.gridView)
                Else
                    MsgBox("Record not found in this criteria", MsgBoxStyle.Information)
                End If
                'End If
            End With
        End If
    End Sub
    Private Sub DetailedView(ByVal dgv As DataGridView)
        If MsgBox("Do You want Selected details ", MsgBoxStyle.YesNo, ) = MsgBoxResult.Yes Then
            strSql = "IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMTAGSTONE')>0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ITEMTAGSTONE"
            strSql += "  SELECT ITEMNAME AS ITEM,SUBITEMNAME AS SUBITEM ,T.TAGNO,STNPCS,STNWT,STNRATE,STNAMT, 1 RESULT,'    ' COLHEAD INTO TEMPTABLEDB..TEMP" & systemId & "ITEMTAGSTONE FROM " & cnAdminDb & "..ITEMTAGSTONE TS "
            strSql += " INNER JOIN " & cnAdminDb & "..ITEMMAST IT ON IT.ITEMID=TS.ITEMID"
            strSql += " INNER JOIN " & cnAdminDb & "..SUBITEMMAST S ON IT.ITEMID=S.ITEMID   "
            strSql += " INNER JOIN " & cnAdminDb & "..ITEMTAG T ON S.SUBITEMID=T.SUBITEMID AND T.SNO=TS.TAGSNO "
            strSql += " WHERE T.TAGNO='" & dgv.CurrentRow.Cells("TAGNO").Value.ToString & "'"
            strSql += "INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ITEMTAGSTONE(ITEM,SUBITEM,STNPCS,STNWT,STNRATE,STNAMT,RESULT,COLHEAD)"
            strSql += "SELECT 'GRANDTOTAL',SUBITEM,SUM(STNPCS),SUM(STNWT),SUM(STNRATE),SUM(STNAMT),2 RESULT,'G' FROM TEMPTABLEDB..TEMP" & systemId & "ITEMTAGSTONE WHERE RESULT='1' GROUP BY ITEM,SUBITEM "

            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = "SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "ITEMTAGSTONE ORDER BY RESULT"
            Dim dtGrid As New DataTable
            dtGrid.Columns.Add("KEYNO", GetType(Integer))
            dtGrid.Columns("KEYNO").AutoIncrement = True
            dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("KEYNO").AutoIncrementStep = 1
            da = New OleDbDataAdapter(strSql, cn)
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGrid)
            If Not dtGrid.Rows.Count > 0 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                Exit Sub
            End If
            Dim objGridShower As New frmGridDispDia
            objGridShower.Name = Me.Name
            objGridShower.gridView.RowTemplate.Height = 21
            objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
            objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            'objGridShower.Size = New Size(778, 550)
            objGridShower.Text = "TAGSTONE DETAIL"
            Dim tit As String = Nothing
            'If chkLstItem.Text <> "" Then
            '    tit += chkLstItem.ToUpper
            '    'Else
            '    '    tit += itemName.ToUpper
            'End If

            objGridShower.lblTitle.Text = tit
            'AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
            objGridShower.StartPosition = FormStartPosition.CenterScreen
            objGridShower.dsGrid.DataSetName = objGridShower.Name
            objGridShower.dsGrid.Tables.Add(dtGrid)
            objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
            objGridShower.FormReSize = True
            objGridShower.FormReLocation = False
            objGridShower.pnlFooter.Visible = False
            objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            DataGridView_SummaryFormatting(objGridShower.gridView)
            FormatGridColumns(objGridShower.gridView, False, False, , False)

            objGridShower.Show()
            objGridShower.FormReSize = True
            FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        Else
            Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre)
            Dim chkItemCounter As String = GetChecked_CheckedList(chkLstItemCounter)
            Dim chkMetalName As String = GetChecked_CheckedList(chkLstMetal)
            Dim chkItemName As String = GetChecked_CheckedList(chkLstItem)
            Dim chkDesigner As String = GetChecked_CheckedList(chkLstDesigner)
            strSql = "IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ITEMTAGSTONE1')>0 "
            strSql += vbCrLf + "DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ITEMTAGSTONE1  "

            strSql += vbCrLf + " SELECT  SUBITEMNAME AS PARTICULAR,ITEMNAME AS ITEM,SUBITEMNAME AS SUBITEM ,SUM(STNPCS)STNPCS,SUM(STNWT)STNWT,STNRATE,SUM(STNAMT)STNAMT, 1 RESULT,'   ' COLHEAD "
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ITEMTAGSTONE1 FROM " & cnAdminDb & "..ITEMTAGSTONE TS  "
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMTAG T ON T.SNO=TS.TAGSNO  "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST IT ON IT.ITEMID=TS.STNITEMID "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST S ON TS.STNSUBITEMID=S.SUBITEMID  WHERE 1=1  "
            If rbtStockOnly.Checked Then strSql += vbCrLf + " AND T.RECDATE <= '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND (T.ISSDATE IS NULL OR T.ISSDATE > '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "')"
            If rbtReceipt.Checked Then strSql += vbCrLf + " AND T.RECDATE BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            If rbtIssue.Checked Then strSql += vbCrLf + " AND T.ISSDATE  BETWEEN '" & dtpAsOnDate.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            If chkCostName <> "" Then strSql += vbCrLf + " AND T.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkItemCounter <> "" Then strSql += vbCrLf + " AND T.ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & chkItemCounter & "))"
            If chkItemName <> "" Then strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkItemName & "))"
            If chkMetalName <> "" Then strSql += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
            If chkDesigner <> "" Then strSql += vbCrLf + " AND T.DESIGNERID IN (SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & chkDesigner & "))"
            If txtTranInvNo.Text <> "" Then strSql += vbCrLf + " AND T.TRANINVNO = '" & txtTranInvNo.Text & "'"
            strSql += vbCrLf + " AND T.COMPANYID = '" & GetStockCompId() & "' GROUP BY ITEMNAME,SUBITEMNAME,STNRATE"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + "  INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ITEMTAGSTONE1( PARTICULAR,ITEM,RESULT,COLHEAD)"
            strSql += vbCrLf + "  SELECT DISTINCT ITEM AS PARTICULAR ,ITEM,0 RESULT,'T' FROM  TEMPTABLEDB..TEMP" & systemId & "ITEMTAGSTONE1"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + "  IF(SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ITEMTAGSTONE1)>0"
            strSql += vbCrLf + "  BEGIN"
            strSql += vbCrLf + "    INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ITEMTAGSTONE1(ITEM,PARTICULAR,STNPCS,STNWT,STNAMT,RESULT,COLHEAD)"
            strSql += vbCrLf + "    SELECT DISTINCT ITEM, 'SUB TOTAL',SUM(STNPCS),SUM(STNWT) ,SUM(STNAMT),2 RESULT,'S' FROM "
            strSql += vbCrLf + "    TEMPTABLEDB..TEMP" & systemId & "ITEMTAGSTONE1 WHERE RESULT='1' GROUP BY ITEM"
            strSql += vbCrLf + "  END"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + "  IF(SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "ITEMTAGSTONE1)>0"
            strSql += vbCrLf + "  BEGIN"
            strSql += vbCrLf + "    INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ITEMTAGSTONE1(PARTICULAR,ITEM,STNPCS,STNWT,STNRATE,STNAMT,RESULT,COLHEAD)"
            strSql += vbCrLf + "    SELECT 'GRAND TOTAL','ZZZZZ',SUM(STNPCS),SUM(STNWT),SUM(STNRATE),SUM(STNAMT),3 RESULT,'G' FROM "
            strSql += vbCrLf + "    TEMPTABLEDB..TEMP" & systemId & "ITEMTAGSTONE1 WHERE RESULT='1' "
            strSql += vbCrLf + "  END"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
            'strSql = "UPDATE TEMPTABLEDB..TEMP" & systemId & "ITEMTAGSTONE1 SET STNRATE=NULL WHERE RESULT=0"
            'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            'cmd.ExecuteNonQuery()
            strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "ITEMTAGSTONE1 ORDER BY ITEM,RESULT"
            Dim dtGrid As New DataTable
            dtGrid.Columns.Add("KEYNO", GetType(Integer))
            dtGrid.Columns("KEYNO").AutoIncrement = True
            dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("KEYNO").AutoIncrementStep = 1
            da = New OleDbDataAdapter(strSql, cn)
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtGrid)
            If Not dtGrid.Rows.Count > 0 Then
                MsgBox("Record not found", MsgBoxStyle.Information)
                Exit Sub
            End If
            Dim objGridShower As New frmGridDispDia
            objGridShower.Name = Me.Name
            objGridShower.gridView.RowTemplate.Height = 21
            objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
            objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            'objGridShower.Size = New Size(778, 550)
            objGridShower.Text = "TAGSTONE DETAIL"
            Dim tit As String = Nothing
            'If chkLstItem.Text <> "" Then
            '    tit += chkLstItem.ToUpper
            '    'Else
            '    '    tit += itemName.ToUpper
            'End If

            objGridShower.lblTitle.Text = tit
            'AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf gridView_ColumnWidthChanged
            objGridShower.StartPosition = FormStartPosition.CenterScreen
            objGridShower.dsGrid.DataSetName = objGridShower.Name
            objGridShower.dsGrid.Tables.Add(dtGrid)
            objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
            objGridShower.FormReSize = True
            objGridShower.FormReLocation = False
            objGridShower.pnlFooter.Visible = False
            objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            DataGridView_SummaryFormatting1(objGridShower.gridView)
            FormatGridColumns(objGridShower.gridView, False, False, , False)
            objGridShower.Show()
            objGridShower.FormReSize = True
            FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        End If
    End Sub
    Function DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            For cnt As Integer = 7 To dgv.ColumnCount - 1
                .Columns(cnt).Visible = False
            Next
            .Columns("TAGNO").Width = 120
            .Columns("ITEM").Width = 120
            .Columns("SUBITEM").Width = 120
            .Columns("STNPCS").Width = 80
            .Columns("STNWT").Width = 120
            .Columns("STNRATE").Width = 120
            .Columns("STNAMT").Width = 100
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("KEYNO").Visible = False
            '.Columns("DAYS").Width = 80
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
            GridViewFormat(dgv)
        End With
    End Function
    Function DataGridView_SummaryFormatting1(ByVal dgv As DataGridView)
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            For cnt As Integer = 7 To dgv.ColumnCount - 1
                .Columns(cnt).Visible = False
            Next
            .Columns("PARTICULAR").Width = 250
            If .Columns.Contains("TAGNO") Then .Columns("TAGNO").Width = 120
            '.Columns("ITEM").Width = 120
            .Columns("ITEM").Visible = False
            .Columns("SUBITEM").Visible = False
            'Columns("SUBITEM").Width = 120
            .Columns("STNPCS").Width = 80
            .Columns("STNWT").Width = 120
            .Columns("STNRATE").Width = 120
            .Columns("STNRATE").Visible = True
            .Columns("STNAMT").Visible = True
            .Columns("STNAMT").Width = 100
            '.Columns("DAYS").Width = 80
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("KEYNO").Visible = False
            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
            GridViewFormat(dgv)
        End With
    End Function
    Function GridViewFormat(ByVal dgv As DataGridView) As Integer
        For Each dgvRow As DataGridViewRow In dgv.Rows
            With dgvRow
                Select Case .Cells("COLHEAD").Value.ToString
                    Case "S"
                        .DefaultCellStyle.Font = reportSubTotalStyle.Font
                        .DefaultCellStyle.ForeColor = reportSubTotalStyle.ForeColor
                    Case "T"
                        .Cells("PARTICULAR").Style.BackColor = reportHeadStyle.BackColor
                        .Cells("PARTICULAR").Style.Font = reportHeadStyle.Font
                    Case "B"
                        .Cells("PARTICULAR").Style.BackColor = Color.Orange
                        .Cells("PARTICULAR").Style.Font = reportHeadStyle.Font
                    Case "G"
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                        .DefaultCellStyle.BackColor = reportTotalStyle.BackColor
                    Case "Z"
                        .DefaultCellStyle.Font = reportTotalStyle.Font
                        .DefaultCellStyle.BackColor = Color.Yellow
                End Select
            End With
        Next
    End Function
    Private Sub Prop_Sets()
        Dim obj As New frmTagItemsPurchaseReport_Old_Properties
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
        obj.p_rbtStockOnly = rbtStockOnly.Checked
        obj.p_rbtDetailed = rbtDetailed.Checked
        obj.p_rbtSummary = rbtSummary.Checked
        obj.p_chkWithImage = chkWithImage.Checked
        obj.p_chkWithCategory = chkWithCategory.Checked
        obj.p_chkWithPend = ChkWithPend.Checked
        obj.p_chkWithTransist = ChkWithTrf.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmTagItemsPurchaseReport_Old_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmTagItemsPurchaseReport_Old_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmTagItemsPurchaseReport_Old_Properties))
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
        rbtStockOnly.Checked = obj.p_rbtStockOnly
        rbtIssue.Checked = obj.p_rbtissue
        rbtReceipt.Checked = obj.p_rbtReceipt
        rbtDetailed.Checked = obj.p_rbtDetailed
        rbtSummary.Checked = obj.p_rbtSummary
        chkWithImage.Checked = obj.p_chkWithImage
        chkWithCategory.Checked = obj.p_chkWithCategory
        ChkWithPend.Checked = obj.p_chkWithPend
        ChkWithTrf.Checked = obj.p_chkWithTransist
    End Sub

    Private Sub rbtStockOnly_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtStockOnly.CheckedChanged
        If rbtStockOnly.Checked = True Then
            PnlTodate.Visible = False
            lblAsOnDate.Text = "As On Date"
            'PnlTodate.Visible = True
            'lblAsOnDate.Text = "From Date"
        End If
    End Sub

    Private Sub rbtReceipt_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtReceipt.CheckedChanged
        If rbtReceipt.Checked = True Then
            PnlTodate.Visible = True
            lblAsOnDate.Text = "From Date"
        End If
    End Sub

    Private Sub rbtIssue_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtIssue.CheckedChanged
        If rbtIssue.Checked = True Then
            PnlTodate.Visible = True
            lblAsOnDate.Text = "From Date"
        End If
    End Sub

    Private Sub chkWithCategory_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkWithCategory.CheckedChanged
        If chkWithCategory.Checked = True Then
            chkItemCategory.Checked = False
            chkNone.Checked = False
        End If
    End Sub

    Private Sub chkItemCategory_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkItemCategory.CheckedChanged
        If chkItemCategory.Checked = True Then
            chkWithCategory.Checked = False
            chkNone.Checked = False
        End If
    End Sub

    Private Sub chkNone_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNone.CheckedChanged
        If chkNone.Checked = True Then
            chkItemCategory.Checked = False
            chkWithCategory.Checked = False
        End If
    End Sub

    Private Sub rbtSummary_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtSummary.CheckedChanged

    End Sub
End Class


Public Class frmTagItemsPurchaseReport_Old_Properties
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

    Private rbtStockOnly As Boolean = False
    Public Property p_rbtStockOnly() As Boolean
        Get
            Return rbtStockOnly
        End Get
        Set(ByVal value As Boolean)
            rbtStockOnly = value
        End Set
    End Property
    Private rbtReceipt As Boolean = True
    Public Property p_rbtReceipt() As Boolean
        Get
            Return rbtReceipt
        End Get
        Set(ByVal value As Boolean)
            rbtReceipt = value
        End Set
    End Property
    Private rbtissue As Boolean = True
    Public Property p_rbtissue() As Boolean
        Get
            Return rbtissue
        End Get
        Set(ByVal value As Boolean)
            rbtissue = value
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
    Private chkWithImage As Boolean = False
    Public Property p_chkWithImage() As Boolean
        Get
            Return chkWithImage
        End Get
        Set(ByVal value As Boolean)
            chkWithImage = value
        End Set
    End Property
    Private chkWithCategory As Boolean = False
    Public Property p_chkWithCategory() As Boolean
        Get
            Return chkWithCategory
        End Get
        Set(ByVal value As Boolean)
            chkWithCategory = value
        End Set
    End Property
    Private chkWithTransist As Boolean = False
    Public Property p_chkWithTransist() As Boolean
        Get
            Return chkWithTransist
        End Get
        Set(ByVal value As Boolean)
            chkWithTransist = value
        End Set
    End Property
    Private chkWithPend As Boolean = False
    Public Property p_chkWithPend() As Boolean
        Get
            Return chkWithPend
        End Get
        Set(ByVal value As Boolean)
            chkWithPend = value
        End Set
    End Property
End Class

