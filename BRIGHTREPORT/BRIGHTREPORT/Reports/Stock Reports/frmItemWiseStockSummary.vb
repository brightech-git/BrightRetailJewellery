Imports System.Data.OleDb
Public Class frmItemWiseStockSummary
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim dtCompany As New DataTable
    Dim apiv As String() = Nothing

    Private Function funrtn(ByVal prefix As String)
        Dim Qry As String = ""
        If apiv.Length > 0 Then
            For i As Integer = 0 To apiv.Length - 1
                Qry += vbCrLf + " ,[" & apiv(i) & "_" & prefix & "] "
            Next
        End If
        Return Qry
    End Function

    Private Function funrtn1()
        Dim Qry As String = ""
        If apiv.Length > 0 Then
            For i As Integer = 0 To apiv.Length - 1
                Qry += vbCrLf + " ,[" & apiv(i) & "] "
            Next
        End If
        Return Qry
    End Function

    Private Function funrtn2()
        Dim Qry As String = ""
        If apiv.Length > 0 Then
            For i As Integer = 0 To apiv.Length - 1
                Qry += vbCrLf + " ,SUM([" & apiv(i) & "_PCS]) AS " & apiv(i) & "_PCS"
                Qry += vbCrLf + " ,SUM([" & apiv(i) & "_GRSWT]) AS " & apiv(i) & "_GRSWT"
                Qry += vbCrLf + " ,SUM([" & apiv(i) & "_NETWT]) AS  " & apiv(i) & "_NETWT"
                Qry += vbCrLf + " ,SUM([" & apiv(i) & "_PCS]) AS " & apiv(i) & "_STNPCS"
                Qry += vbCrLf + " ,SUM([" & apiv(i) & "_GRSWT]) AS " & apiv(i) & "_STNWT"
                Qry += vbCrLf + " ,SUM([" & apiv(i) & "_PCS]) AS " & apiv(i) & "_DIAPCS"
                Qry += vbCrLf + " ,SUM([" & apiv(i) & "_GRSWT]) AS " & apiv(i) & "_DIAWT"
            Next
        End If
        Return Qry
    End Function
    Private Function funrtn3(ByVal prefix As String)
        Dim Qry As String = ""
        If apiv.Length > 0 Then
            For i As Integer = 0 To apiv.Length - 1
                Qry += " ISNULL(SUM([" & apiv(i) & "_" & prefix & "]),0) "
                If i <> apiv.Length - 1 Then
                    Qry += vbCrLf + "+"
                End If
            Next
        End If
        Return Qry
    End Function
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        If chkLstCategory.Items.Count > 0 And Not chkLstCategory.CheckedItems.Count > 0 Then chkCategorySelectAll.Checked = True
        If Not chkLstDesigner.CheckedItems.Count > 0 Then chkDesignerSelectAll.Checked = True
        If Not chkLstItemCounter.CheckedItems.Count > 0 Then chkItemCounterSelectAll.Checked = True
        If Not chkLstCostCentre.CheckedItems.Count > 0 Then chkCostCentreSelectAll.Checked = True
        If Not chkLstMetal.CheckedItems.Count > 0 Then chkMetalSelectAll.Checked = True
        Dim chkDesigner As String = GetChecked_CheckedList(chkLstDesigner)
        Dim chkItemCounter As String = GetChecked_CheckedList(chkLstItemCounter)
        Dim chkCategory As String = GetChecked_CheckedList(chkLstCategory)
        Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre)
        Dim chkMetalName As String = GetChecked_CheckedList(chkLstMetal)
        Dim GroupName As String = Nothing
        If cmbGroupBy.Items.Contains(cmbGroupBy.Text) = False Then cmbGroupBy.Text = "NONE"
        Select Case cmbGroupBy.Text
            Case "ITEM WISE"
                GroupName = "ITEM"
            Case "DESIGNER WISE"
                GroupName = "DESIGNER"
            Case "COUNTER WISE"
                GroupName = "COUNTER"
            Case "COST CENTRE"
                GroupName = "COSTCENTRE"
            Case Else
                GroupName = "NONE"
        End Select
        strSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "STOCKVIEW')> 0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW"
        strSql += vbCrLf + "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "TAGVIEW')> 0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "TAGVIEW"
        strSql += vbCrLf + "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "NONTAGVIEW')> 0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "NONTAGVIEW"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = "     SELECT "
        strSql += vbCrLf + "     PCS,GRSWT,NETWT,LESSWT"
        strSql += vbCrLf + "     ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNPCS"
        strSql += vbCrLf + "     ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNWT"
        strSql += vbCrLf + "     ,(SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNAMT"
        strSql += vbCrLf + "     ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAPCS"
        strSql += vbCrLf + "     ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAWT"
        strSql += vbCrLf + "     ,(SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAAMT"
        strSql += vbCrLf + "     ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREPCS"
        strSql += vbCrLf + "     ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREWT"
        strSql += vbCrLf + "     ,(SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREAMT"
        strSql += vbCrLf + "     ,CONVERT(NUMERIC(15,2),0)RATE,CONVERT(NUMERIC(15,2),0)AMOUNT,ITEMID,SUBITEMID"
        If GroupName = "COUNTER" Then
            strSql += vbCrLf + "     ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)AS " & GroupName & ""
        ElseIf GroupName = "DESIGNER" Then
            strSql += vbCrLf + "    ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS " & GroupName & ""
        ElseIf GroupName = "COSTCENTRE" Then
            'strSql += vbCrLf + "    ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.COSTID)AS " & GroupName & ""
            strSql += vbCrLf + "    ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T." & IIf(ChkOrgCostCentre.Checked = True, "TCOSTID", "COSTID") & ")AS " & GroupName & ""
        End If
        If chkItemIdOrder.Checked Then
            strSql += vbCrLf + "     ,(SELECT '['+REPLICATE(' ',5-LEN(ITEMID)) + CONVERT(VARCHAR,ITEMID) + ']' + ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
            strSql += vbCrLf + "     ,(SELECT '['+REPLICATE(' ',5-LEN(SUBITEMID)) + CONVERT(VARCHAR,SUBITEMID) + ']' + SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID)AS SUBITEM"
        Else
            strSql += vbCrLf + "     ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
            strSql += vbCrLf + "     ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID)AS SUBITEM"
        End If

        If ChkOrgCostCentre.Checked = True Then
            strSql += vbCrLf + "     ,T.TCOSTID "
        ElseIf ChkOrgCostCentre.Checked = False Then
            strSql += vbCrLf + "     ,T.COSTID "
        End If
        strSql += vbCrLf + "     ,T.ITEMCTRID"

        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "NONTAGVIEW"
        strSql += vbCrLf + "     FROM " & cnAdminDb & "..ITEMNONTAG AS T"
        If rbtIssue.Checked Then
            strSql += vbCrLf + "  WHERE RECDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "  AND RECISS = 'I'"
        ElseIf rbtReceipt.Checked Then
            strSql += vbCrLf + "  WHERE RECDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "  AND RECISS = 'R'"
        End If
        If chkCategory <> "" Then strSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & ")))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If Not chkCostCentreSelectAll.Checked Then
            'If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkCostName <> "" Then strSql += vbCrLf + " AND " & IIf(ChkOrgCostCentre.Checked = True, "TCOSTID", "COSTID") & " IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        End If
        If Not chkDesignerSelectAll.Checked Then
            If chkDesigner <> "" Then strSql += vbCrLf + " AND DESIGNERID IN (sELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & chkDesigner & "))"
        End If
        If Not chkItemCounterSelectAll.Checked Then
            If chkItemCounter <> "" Then strSql += vbCrLf + " AND ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & chkItemCounter & "))"
        End If
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            strSql += vbCrLf + " AND T.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        Else
            strSql += vbCrLf + " AND T.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
        End If
        If _HideBackOffice Then strSql += vbCrLf + " AND ISNULL(T.FLAG,'') <> 'B'"
        strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
        If txtLotno_NUM.Text <> "" Then strSql += vbCrLf + " and isnull(T.LOTNO,0) = " & Val(txtLotno_NUM.Text)
        If ChkOrgCostCentre.Checked = True Then
            strSql += vbCrLf + " AND (ISNULL(REFNO,'') = '' OR (ISSTYPE ='TR' AND REFNO <> '' "
            If chkCostName <> "" Then
                strSql += vbCrLf + "AND ISNULL(TCOSTID,'') IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))))"
            Else
                strSql += vbCrLf + "))"
            End If
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        strSql = "     SELECT "
        strSql += vbCrLf + "     PCS,GRSWT,NETWT,LESSWT"
        strSql += vbCrLf + "     ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNPCS"
        strSql += vbCrLf + "     ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNWT"
        strSql += vbCrLf + "     ,(SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNAMT"
        strSql += vbCrLf + "     ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAPCS"
        strSql += vbCrLf + "     ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAWT"
        strSql += vbCrLf + "     ,(SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAAMT"
        strSql += vbCrLf + "     ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREPCS"
        strSql += vbCrLf + "     ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREWT"
        strSql += vbCrLf + "     ,(SELECT SUM(STNAMT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREAMT"
        strSql += vbCrLf + "     ,RATE,SALVALUE AMOUNT,ITEMID,SUBITEMID"
        If GroupName = "COUNTER" Then
            strSql += vbCrLf + "     ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)AS " & GroupName & ""
        ElseIf GroupName = "DESIGNER" Then
            strSql += vbCrLf + "    ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS " & GroupName & ""
        ElseIf GroupName = "COSTCENTRE" Then
            strSql += vbCrLf + "    ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T." & IIf(ChkOrgCostCentre.Checked = True, "TCOSTID", "COSTID") & ")AS " & GroupName & ""
        End If
        If chkItemIdOrder.Checked Then
            strSql += vbCrLf + "     ,(SELECT '['+REPLICATE(' ',5-LEN(ITEMID)) + CONVERT(VARCHAR,ITEMID) + ']' + ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
            strSql += vbCrLf + "     ,(SELECT '['+REPLICATE(' ',5-LEN(SUBITEMID)) + CONVERT(VARCHAR,SUBITEMID) + ']' + SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID)AS SUBITEM"
        Else
            strSql += vbCrLf + "     ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
            strSql += vbCrLf + "     ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID)AS SUBITEM"
        End If
        If ChkOrgCostCentre.Checked = True Then
            strSql += vbCrLf + "     ,T.TCOSTID "
        ElseIf ChkOrgCostCentre.Checked = False Then
            strSql += vbCrLf + "     ,T.COSTID "
        End If
        strSql += vbCrLf + "     ,T.ITEMCTRID"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "TAGVIEW"
        strSql += vbCrLf + "     FROM " & cnAdminDb & "..ITEMTAG AS T"
        If rbtIssue.Checked Then
            strSql += vbCrLf + "     WHERE ISSDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "     AND ISSDATE IS NOT NULL"
        ElseIf rbtReceipt.Checked Then
            strSql += vbCrLf + "     WHERE " & IIf(chkActualDate.Checked, "ACTUALRECDATE", "RECDATE") & " BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            'STRSQL += VBCRLF + "     AND ISSDATE IS NULL" 
        End If
        If chkCategory <> "" Then strSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & ")))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If Not chkCostCentreSelectAll.Checked Then
            If chkCostName <> "" Then strSql += vbCrLf + " AND " & IIf(ChkOrgCostCentre.Checked = True, "TCOSTID", "COSTID") & " IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        End If
        If Not chkDesignerSelectAll.Checked Then
            If chkDesigner <> "" Then strSql += vbCrLf + " AND DESIGNERID IN (sELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & chkDesigner & "))"
        End If
        If Not chkItemCounterSelectAll.Checked Then
            If chkItemCounter <> "" Then strSql += vbCrLf + " AND ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & chkItemCounter & "))"
        End If
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            strSql += vbCrLf + " AND COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        Else
            strSql += vbCrLf + " AND COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
        End If
        If txtLotno_NUM.Text <> "" Then strSql += vbCrLf + " AND T.LOTSNO in  (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE ISNULL(LOTNO,0) = " & Val(txtLotno_NUM.Text) & ")"
        If txtRefno.Text <> "" Then strSql += vbCrLf + " AND ISNULL(T.TRANINVNO,0)='" & txtRefno.Text & "'"
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "     SELECT "
        strSql += vbCrLf + "     PCS,GRSWT,NETWT,LESSWT"
        strSql += vbCrLf + "     ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNPCS"
        strSql += vbCrLf + "     ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNWT"
        strSql += vbCrLf + "     ,(SELECT SUM(STNAMT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNAMT"
        strSql += vbCrLf + "     ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAPCS"
        strSql += vbCrLf + "     ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAWT"
        strSql += vbCrLf + "     ,(SELECT SUM(STNAMT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAAMT"
        strSql += vbCrLf + "     ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREPCS"
        strSql += vbCrLf + "     ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREWT"
        strSql += vbCrLf + "     ,(SELECT SUM(STNAMT) FROM " & cnAdminDb & "..CITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREAMT"
        strSql += vbCrLf + "     ,RATE,SALVALUE AMOUNT,ITEMID,SUBITEMID"
        If GroupName = "COUNTER" Then
            strSql += vbCrLf + "     ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)AS " & GroupName & ""
        ElseIf GroupName = "DESIGNER" Then
            strSql += vbCrLf + "    ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS " & GroupName & ""
        ElseIf GroupName = "COSTCENTRE" Then
            strSql += vbCrLf + "    ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T." & IIf(ChkOrgCostCentre.Checked = True, "TCOSTID", "COSTID") & ")AS " & GroupName & ""
        End If
        If chkItemIdOrder.Checked Then
            strSql += vbCrLf + "     ,(SELECT '['+REPLICATE(' ',5-LEN(ITEMID)) + CONVERT(VARCHAR,ITEMID) + ']' + ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
            strSql += vbCrLf + "     ,(SELECT '['+REPLICATE(' ',5-LEN(SUBITEMID)) + CONVERT(VARCHAR,SUBITEMID) + ']' + SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID)AS SUBITEM"
        Else
            strSql += vbCrLf + "     ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
            strSql += vbCrLf + "     ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID)AS SUBITEM"
        End If
        If ChkOrgCostCentre.Checked = True Then
            strSql += vbCrLf + "     ,T.TCOSTID "
        ElseIf ChkOrgCostCentre.Checked = False Then
            strSql += vbCrLf + "     ,T.COSTID "
        End If
        strSql += vbCrLf + "     ,T.ITEMCTRID"
        strSql += vbCrLf + "     FROM " & cnAdminDb & "..CITEMTAG AS T"
        If rbtIssue.Checked Then
            strSql += vbCrLf + "     WHERE ISSDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + "     AND ISSDATE IS NOT NULL"
        ElseIf rbtReceipt.Checked Then
            strSql += vbCrLf + "     WHERE " & IIf(chkActualDate.Checked, "ACTUALRECDATE", "RECDATE") & " BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            'STRSQL += VBCRLF + "     AND ISSDATE IS NULL" 
        End If
        If chkCategory <> "" Then strSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & ")))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        If Not chkCostCentreSelectAll.Checked Then
            If chkCostName <> "" Then strSql += vbCrLf + " AND " & IIf(ChkOrgCostCentre.Checked = True, "TCOSTID", "COSTID") & " IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        End If
        If Not chkDesignerSelectAll.Checked Then
            If chkDesigner <> "" Then strSql += vbCrLf + " AND DESIGNERID IN (sELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & chkDesigner & "))"
        End If
        If Not chkItemCounterSelectAll.Checked Then
            If chkItemCounter <> "" Then strSql += vbCrLf + " AND ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & chkItemCounter & "))"
        End If
        If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            strSql += vbCrLf + " AND T.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
        Else
            strSql += vbCrLf + " AND T.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
        End If
        If txtLotno_NUM.Text <> "" Then strSql += vbCrLf + " AND T.LOTSNO IN  (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE ISNULL(LOTNO,0) = " & Val(txtLotno_NUM.Text) & ")"
        If txtRefno.Text <> "" Then strSql += vbCrLf + " AND ISNULL(T.TRANINVNO,0)='" & txtRefno.Text & "'"
        If ChkOrgCostCentre.Checked = True Then strSql += vbCrLf + " AND T.TAGNO NOT IN (SELECT TAGNO FROM  " & cnAdminDb & "..ITEMTAG   WHERE ACTUALRECDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()


        strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "TAGVIEW"
        strSql += vbCrLf + " SET PCS = null,GRSWT = null,NETWT = null,DIAPCS = L.PCS"
        strSql += vbCrLf + " ,DIAWT = L.GRSWT"
        strSql += " FROM TEMPTABLEDB..TEMP" & systemId & "TAGVIEW AS L"
        'strSql += " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = 'D')"
        strSql += " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "TAGVIEW"
        strSql += vbCrLf + " SET PCS = null,GRSWT = null,NETWT = null,STNPCS = L.PCS"
        strSql += vbCrLf + " ,STNWT = L.GRSWT"
        strSql += " FROM TEMPTABLEDB..TEMP" & systemId & "TAGVIEW AS L"
        'strSql += " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = 'T')"
        strSql += " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "TAGVIEW"
        strSql += vbCrLf + " SET PCS = null,GRSWT = null,NETWT = null,PREPCS = L.PCS"
        strSql += vbCrLf + " ,PREWT = L.GRSWT"
        strSql += " FROM TEMPTABLEDB..TEMP" & systemId & "TAGVIEW AS L"
        'strSql += " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = 'T')"
        strSql += " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "NONTAGVIEW"
        strSql += vbCrLf + " SET PCS = null,GRSWT = null,NETWT = null,DIAPCS = L.PCS"
        strSql += vbCrLf + " ,DIAWT = L.GRSWT"
        strSql += " FROM TEMPTABLEDB..TEMP" & systemId & "NONTAGVIEW AS L"
        'strSql += " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = 'D')"
        strSql += " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()


        strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "NONTAGVIEW"
        strSql += vbCrLf + " SET PCS = null,GRSWT = null,NETWT =null,STNPCS = L.PCS"
        strSql += vbCrLf + " ,STNWT = L.GRSWT"
        strSql += " FROM TEMPTABLEDB..TEMP" & systemId & "NONTAGVIEW AS L"
        'strSql += " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = 'T')"
        strSql += " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "NONTAGVIEW"
        strSql += vbCrLf + " SET PCS = null,GRSWT = null,NETWT =null,PREPCS = L.PCS"
        strSql += vbCrLf + " ,PREWT = L.GRSWT"
        strSql += " FROM TEMPTABLEDB..TEMP" & systemId & "NONTAGVIEW AS L"
        'strSql += " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = 'T')"
        strSql += " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()
        If chkwithSummaryCostName.Checked = True Then
            strSql = ""

            strSql = " IF OBJECT_ID('TEMPTABLEDB..[TEMPPIVOT" & systemId & "]','U') IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMPPIVOT" & systemId & "]"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..[TEMPPIVOTPCS" & systemId & "]','U') IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMPPIVOTPCS" & systemId & "]"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..[TEMPPIVOTGRS" & systemId & "]','U') IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMPPIVOTGRS" & systemId & "]"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..[TEMPPIVOTNET" & systemId & "]','U') IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMPPIVOTNET" & systemId & "]"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..[TEMPPIVOTFINAL" & systemId & "]','U') IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMPPIVOTFINAL" & systemId & "]"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..[TEMPPIVOTSTNPCS" & systemId & "]','U') IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMPPIVOTSTNPCS" & systemId & "]"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..[TEMPPIVOTSTNWT" & systemId & "]','U') IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMPPIVOTSTNWT" & systemId & "]"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..[TEMPPIVOTDIAPCS" & systemId & "]','U') IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMPPIVOTDIAPCS" & systemId & "]"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..[TEMPPIVOTDIAWT" & systemId & "]','U') IS NOT NULL DROP TABLE TEMPTABLEDB..[TEMPPIVOTDIAWT" & systemId & "]"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = ""
            strSql += vbCrLf + "        SELECT ROW_NUMBER() OVER(ORDER BY COSTID,ITEMCTRID)ROWID "
            strSql += vbCrLf + "        ,*"
            strSql += vbCrLf + "        ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID=X.COSTID)COSTNAME "
            strSql += vbCrLf + "        ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID=X.ITEMCTRID)ITEMCOUNTERNAME "
            strSql += vbCrLf + "        INTO TEMPTABLEDB..[TEMPPIVOT" & systemId & "] FROM ("
            strSql += vbCrLf + "        SELECT ITEMCTRID,COSTID"
            strSql += vbCrLf + "        ,SUM(PCS)PCS"
            strSql += vbCrLf + "        ,SUM(GRSWT)GRSWT"
            strSql += vbCrLf + "        ,SUM(NETWT)NETWT "
            strSql += vbCrLf + "        ,SUM(STNPCS)STNPCS"
            strSql += vbCrLf + "        ,SUM(STNWT)STNWT"
            strSql += vbCrLf + "        ,SUM(DIAPCS)DIAPCS"
            strSql += vbCrLf + "        ,SUM(DIAWT)DIAWT"
            strSql += vbCrLf + "        ,'NON-TAGED' REMARKS"
            strSql += vbCrLf + "        FROM TEMPTABLEDB..TEMP" & systemId & "NONTAGVIEW"
            strSql += vbCrLf + "        GROUP BY ITEMCTRID,COSTID"
            strSql += vbCrLf + "        UNION ALL"
            strSql += vbCrLf + "        SELECT ITEMCTRID,COSTID"
            strSql += vbCrLf + "        ,SUM(PCS)PCS"
            strSql += vbCrLf + "        ,SUM(GRSWT)GRSWT"
            strSql += vbCrLf + "        ,SUM(NETWT)NETWT "
            strSql += vbCrLf + "        ,SUM(STNPCS)STNPCS"
            strSql += vbCrLf + "        ,SUM(STNWT)STNWT"
            strSql += vbCrLf + "        ,SUM(DIAPCS)DIAPCS"
            strSql += vbCrLf + "        ,SUM(DIAWT)DIAWT"
            strSql += vbCrLf + "        ,'TAGED' REMARKS"
            strSql += vbCrLf + "        FROM TEMPTABLEDB..TEMP" & systemId & "TAGVIEW"
            strSql += vbCrLf + "        GROUP BY ITEMCTRID,COSTID"
            strSql += vbCrLf + "        )X ORDER BY COSTID,ITEMCTRID"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            Dim pviotcostid As String = ""
            Dim pviotcostidwithoutanglebracket As String = ""
            Dim dtPivot As New DataTable
            strSql = " SELECT DISTINCT COSTID FROM TEMPTABLEDB..[TEMPPIVOT" & systemId & "]"
            dtPivot = GetSqlTable(strSql, cn, Nothing)
            If dtPivot.Rows.Count > 0 Then
                For i As Integer = 0 To dtPivot.Rows.Count - 1
                    pviotcostid = pviotcostid & "[" & dtPivot.Rows(i).Item("COSTID").ToString & "]"
                    pviotcostidwithoutanglebracket = pviotcostidwithoutanglebracket & "" & dtPivot.Rows(i).Item("COSTID").ToString & ""
                    If i <> dtPivot.Rows.Count - 1 Then
                        pviotcostid = pviotcostid & ","
                        pviotcostidwithoutanglebracket = pviotcostidwithoutanglebracket & ","
                    End If
                Next
            End If
            apiv = Nothing
            apiv = pviotcostidwithoutanglebracket.Split(",")

            strSql = " CREATE TABLE TEMPTABLEDB..[TEMPPIVOTFINAL" & systemId & "]"
            strSql += vbCrLf + " ( REMARKS VARCHAR(100)"
            strSql += vbCrLf + " ,ITEMCTRID INT"
            strSql += vbCrLf + " ,ITEMCOUNTERNAME VARCHAR(100)"
            strSql += vbCrLf + " ,COSTNAME VARCHAR(50)"
            If apiv.Length > 0 Then
                For i As Integer = 0 To apiv.Length - 1
                    strSql += vbCrLf + " ,[" & apiv(i) & "_PCS] INT"
                    strSql += vbCrLf + " ,[" & apiv(i) & "_GRSWT] NUMERIC(15,2)"
                    strSql += vbCrLf + " ,[" & apiv(i) & "_NETWT] NUMERIC(15,2)"
                    strSql += vbCrLf + " ,[" & apiv(i) & "_STNPCS] INT"
                    strSql += vbCrLf + " ,[" & apiv(i) & "_STNWT] NUMERIC(15,2)"
                    strSql += vbCrLf + " ,[" & apiv(i) & "_DIAPCS] INT"
                    strSql += vbCrLf + " ,[" & apiv(i) & "_DIAWT] NUMERIC(15,2)"
                Next
            End If
            strSql += vbCrLf + " )"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            'STEP1

            strSql = ""
            strSql += vbCrLf + "        SELECT *"
            strSql += vbCrLf + "        INTO TEMPTABLEDB..[TEMPPIVOTPCS" & systemId & "]"
            strSql += vbCrLf + "        FROM "
            strSql += vbCrLf + "        ("
            strSql += vbCrLf + "          SELECT ITEMCTRID,ITEMCOUNTERNAME,PCS,REMARKS,COSTID,COSTNAME"
            strSql += vbCrLf + "          FROM TEMPTABLEDB..[TEMPPIVOT" & systemId & "]"
            strSql += vbCrLf + "        ) SRC"
            strSql += vbCrLf + "        PIVOT"
            strSql += vbCrLf + "        ("
            strSql += vbCrLf + "          SUM(PCS)"
            strSql += vbCrLf + "          FOR COSTID IN (" & pviotcostid & ") " '[AN], [AS], [HO]
            strSql += vbCrLf + "        ) PIV1"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = " INSERT INTO TEMPTABLEDB..[TEMPPIVOTFINAL" & systemId & "]( "
            strSql += vbCrLf + " REMARKS"
            strSql += vbCrLf + " ,ITEMCTRID"
            strSql += vbCrLf + " ,ITEMCOUNTERNAME"
            strSql += vbCrLf + " ,COSTNAME"
            strSql += vbCrLf + funrtn("PCS")
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " SELECT  "
            strSql += vbCrLf + " REMARKS"
            strSql += vbCrLf + " ,ITEMCTRID"
            strSql += vbCrLf + " ,ITEMCOUNTERNAME"
            strSql += vbCrLf + " ,COSTNAME"
            strSql += vbCrLf + funrtn1()
            strSql += vbCrLf + " FROM TEMPTABLEDB..[TEMPPIVOTPCS" & systemId & "]"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()


            'STEP2
            strSql = ""
            strSql += vbCrLf + "        SELECT *"
            strSql += vbCrLf + "        INTO TEMPTABLEDB..[TEMPPIVOTGRS" & systemId & "]"
            strSql += vbCrLf + "        FROM "
            strSql += vbCrLf + "        ("
            strSql += vbCrLf + "          SELECT ITEMCTRID,ITEMCOUNTERNAME,GRSWT,REMARKS,COSTID,COSTNAME"
            strSql += vbCrLf + "          FROM TEMPTABLEDB..[TEMPPIVOT" & systemId & "]"
            strSql += vbCrLf + "        ) SRC"
            strSql += vbCrLf + "        PIVOT"
            strSql += vbCrLf + "        ("
            strSql += vbCrLf + "          SUM(GRSWT)"
            strSql += vbCrLf + "          FOR COSTID IN (" & pviotcostid & ")"
            strSql += vbCrLf + "        ) PIV1"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = " INSERT INTO TEMPTABLEDB..[TEMPPIVOTFINAL" & systemId & "]( "
            strSql += vbCrLf + " REMARKS"
            strSql += vbCrLf + " ,ITEMCTRID"
            strSql += vbCrLf + " ,ITEMCOUNTERNAME"
            strSql += vbCrLf + " ,COSTNAME"
            strSql += vbCrLf + funrtn("GRSWT")
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " SELECT  "
            strSql += vbCrLf + " REMARKS"
            strSql += vbCrLf + " ,ITEMCTRID"
            strSql += vbCrLf + " ,ITEMCOUNTERNAME"
            strSql += vbCrLf + " ,COSTNAME"
            strSql += vbCrLf + funrtn1()
            strSql += vbCrLf + " FROM TEMPTABLEDB..[TEMPPIVOTGRS" & systemId & "]"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            'STEP3
            strSql = ""
            strSql += vbCrLf + "        SELECT *"
            strSql += vbCrLf + "        INTO TEMPTABLEDB..[TEMPPIVOTNET" & systemId & "]"
            strSql += vbCrLf + "        FROM "
            strSql += vbCrLf + "        ("
            strSql += vbCrLf + "          SELECT ITEMCTRID,ITEMCOUNTERNAME,NETWT,REMARKS,COSTID,COSTNAME"
            strSql += vbCrLf + "          FROM TEMPTABLEDB..[TEMPPIVOT" & systemId & "]"
            strSql += vbCrLf + "        ) SRC"
            strSql += vbCrLf + "        PIVOT"
            strSql += vbCrLf + "        ("
            strSql += vbCrLf + "          SUM(NETWT)"
            strSql += vbCrLf + "          FOR COSTID IN (" & pviotcostid & ")"
            strSql += vbCrLf + "        ) PIV1"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = " INSERT INTO TEMPTABLEDB..[TEMPPIVOTFINAL" & systemId & "]( "
            strSql += vbCrLf + " REMARKS"
            strSql += vbCrLf + " ,ITEMCTRID"
            strSql += vbCrLf + " ,ITEMCOUNTERNAME"
            strSql += vbCrLf + " ,COSTNAME"
            strSql += vbCrLf + funrtn("NETWT")
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " SELECT  "
            strSql += vbCrLf + " REMARKS"
            strSql += vbCrLf + " ,ITEMCTRID"
            strSql += vbCrLf + " ,ITEMCOUNTERNAME"
            strSql += vbCrLf + " ,COSTNAME"
            strSql += vbCrLf + funrtn1()
            strSql += vbCrLf + " FROM TEMPTABLEDB..[TEMPPIVOTNET" & systemId & "]"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            'STEP4
            strSql = ""
            strSql += vbCrLf + "        SELECT *"
            strSql += vbCrLf + "        INTO TEMPTABLEDB..[TEMPPIVOTSTNPCS" & systemId & "]"
            strSql += vbCrLf + "        FROM "
            strSql += vbCrLf + "        ("
            strSql += vbCrLf + "          SELECT ITEMCTRID,ITEMCOUNTERNAME,STNPCS,REMARKS,COSTID,COSTNAME"
            strSql += vbCrLf + "          FROM TEMPTABLEDB..[TEMPPIVOT" & systemId & "]"
            strSql += vbCrLf + "        ) SRC"
            strSql += vbCrLf + "        PIVOT"
            strSql += vbCrLf + "        ("
            strSql += vbCrLf + "          SUM(STNPCS)"
            strSql += vbCrLf + "          FOR COSTID IN (" & pviotcostid & ") " '[AN], [AS], [HO]
            strSql += vbCrLf + "        ) PIV1"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = " INSERT INTO TEMPTABLEDB..[TEMPPIVOTFINAL" & systemId & "]( "
            strSql += vbCrLf + " REMARKS"
            strSql += vbCrLf + " ,ITEMCTRID"
            strSql += vbCrLf + " ,ITEMCOUNTERNAME"
            strSql += vbCrLf + " ,COSTNAME"
            strSql += vbCrLf + funrtn("STNPCS")
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " SELECT  "
            strSql += vbCrLf + " REMARKS"
            strSql += vbCrLf + " ,ITEMCTRID"
            strSql += vbCrLf + " ,ITEMCOUNTERNAME"
            strSql += vbCrLf + " ,COSTNAME"
            strSql += vbCrLf + funrtn1()
            strSql += vbCrLf + " FROM TEMPTABLEDB..[TEMPPIVOTSTNPCS" & systemId & "]"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()


            'STEP5
            strSql = ""
            strSql += vbCrLf + "        SELECT *"
            strSql += vbCrLf + "        INTO TEMPTABLEDB..[TEMPPIVOTSTNWT" & systemId & "]"
            strSql += vbCrLf + "        FROM "
            strSql += vbCrLf + "        ("
            strSql += vbCrLf + "          SELECT ITEMCTRID,ITEMCOUNTERNAME,STNWT,REMARKS,COSTID,COSTNAME"
            strSql += vbCrLf + "          FROM TEMPTABLEDB..[TEMPPIVOT" & systemId & "]"
            strSql += vbCrLf + "        ) SRC"
            strSql += vbCrLf + "        PIVOT"
            strSql += vbCrLf + "        ("
            strSql += vbCrLf + "          SUM(STNWT)"
            strSql += vbCrLf + "          FOR COSTID IN (" & pviotcostid & ") " '[AN], [AS], [HO]
            strSql += vbCrLf + "        ) PIV1"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = " INSERT INTO TEMPTABLEDB..[TEMPPIVOTFINAL" & systemId & "]( "
            strSql += vbCrLf + " REMARKS"
            strSql += vbCrLf + " ,ITEMCTRID"
            strSql += vbCrLf + " ,ITEMCOUNTERNAME"
            strSql += vbCrLf + " ,COSTNAME"
            strSql += vbCrLf + funrtn("STNWT")
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " SELECT  "
            strSql += vbCrLf + " REMARKS"
            strSql += vbCrLf + " ,ITEMCTRID"
            strSql += vbCrLf + " ,ITEMCOUNTERNAME"
            strSql += vbCrLf + " ,COSTNAME"
            strSql += vbCrLf + funrtn1()
            strSql += vbCrLf + " FROM TEMPTABLEDB..[TEMPPIVOTSTNWT" & systemId & "]"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()


            'STEP6
            strSql = ""
            strSql += vbCrLf + "        SELECT *"
            strSql += vbCrLf + "        INTO TEMPTABLEDB..[TEMPPIVOTDIAPCS" & systemId & "]"
            strSql += vbCrLf + "        FROM "
            strSql += vbCrLf + "        ("
            strSql += vbCrLf + "          SELECT ITEMCTRID,ITEMCOUNTERNAME,DIAPCS,REMARKS,COSTID,COSTNAME"
            strSql += vbCrLf + "          FROM TEMPTABLEDB..[TEMPPIVOT" & systemId & "]"
            strSql += vbCrLf + "        ) SRC"
            strSql += vbCrLf + "        PIVOT"
            strSql += vbCrLf + "        ("
            strSql += vbCrLf + "          SUM(DIAPCS)"
            strSql += vbCrLf + "          FOR COSTID IN (" & pviotcostid & ") " '[AN], [AS], [HO]
            strSql += vbCrLf + "        ) PIV1"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = " INSERT INTO TEMPTABLEDB..[TEMPPIVOTFINAL" & systemId & "]( "
            strSql += vbCrLf + " REMARKS"
            strSql += vbCrLf + " ,ITEMCTRID"
            strSql += vbCrLf + " ,ITEMCOUNTERNAME"
            strSql += vbCrLf + " ,COSTNAME"
            strSql += vbCrLf + funrtn("DIAPCS")
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " SELECT  "
            strSql += vbCrLf + " REMARKS"
            strSql += vbCrLf + " ,ITEMCTRID"
            strSql += vbCrLf + " ,ITEMCOUNTERNAME"
            strSql += vbCrLf + " ,COSTNAME"
            strSql += vbCrLf + funrtn1()
            strSql += vbCrLf + " FROM TEMPTABLEDB..[TEMPPIVOTDIAPCS" & systemId & "]"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            'STEP7
            strSql = ""
            strSql += vbCrLf + "        SELECT *"
            strSql += vbCrLf + "        INTO TEMPTABLEDB..[TEMPPIVOTDIAWT" & systemId & "]"
            strSql += vbCrLf + "        FROM "
            strSql += vbCrLf + "        ("
            strSql += vbCrLf + "          SELECT ITEMCTRID,ITEMCOUNTERNAME,DIAWT,REMARKS,COSTID,COSTNAME"
            strSql += vbCrLf + "          FROM TEMPTABLEDB..[TEMPPIVOT" & systemId & "]"
            strSql += vbCrLf + "        ) SRC"
            strSql += vbCrLf + "        PIVOT"
            strSql += vbCrLf + "        ("
            strSql += vbCrLf + "          SUM(DIAWT)"
            strSql += vbCrLf + "          FOR COSTID IN (" & pviotcostid & ") " '[AN], [AS], [HO]
            strSql += vbCrLf + "        ) PIV1"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            strSql = " INSERT INTO TEMPTABLEDB..[TEMPPIVOTFINAL" & systemId & "]( "
            strSql += vbCrLf + " REMARKS"
            strSql += vbCrLf + " ,ITEMCTRID"
            strSql += vbCrLf + " ,ITEMCOUNTERNAME"
            strSql += vbCrLf + " ,COSTNAME"
            strSql += vbCrLf + funrtn("DIAWT")
            strSql += vbCrLf + " )"
            strSql += vbCrLf + " SELECT  "
            strSql += vbCrLf + " REMARKS"
            strSql += vbCrLf + " ,ITEMCTRID"
            strSql += vbCrLf + " ,ITEMCOUNTERNAME"
            strSql += vbCrLf + " ,COSTNAME"
            strSql += vbCrLf + funrtn1()
            strSql += vbCrLf + " FROM TEMPTABLEDB..[TEMPPIVOTDIAWT" & systemId & "]"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()

            'FINAL
            strSql = ""
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " ITEMCOUNTERNAME COUNTERNAME"
            strSql += vbCrLf + " ,REMARKS STOCKTYPE"
            strSql += vbCrLf + funrtn2()
            strSql += vbCrLf + "," & funrtn3("PCS") & " AS TOTAL_PCS"
            strSql += vbCrLf + "," & funrtn3("GRSWT") & " AS TOTAL_GRSWT"
            strSql += vbCrLf + "," & funrtn3("NETWT") & " AS TOTAL_NETWT"
            strSql += vbCrLf + "," & funrtn3("STNPCS") & " AS TOTAL_STNPCS"
            strSql += vbCrLf + "," & funrtn3("STNWT") & " AS TOTAL_STNWT"
            strSql += vbCrLf + "," & funrtn3("DIAPCS") & " AS TOTAL_DIAPCS"
            strSql += vbCrLf + "," & funrtn3("DIAWT") & " AS TOTAL_DIAWT"
            strSql += vbCrLf + " FROM TEMPTABLEDB..[TEMPPIVOTFINAL" & systemId & "]"
            strSql += vbCrLf + " GROUP BY ITEMCOUNTERNAME,ITEMCTRID,REMARKS"
            strSql += vbCrLf + " ORDER BY ITEMCOUNTERNAME,ITEMCTRID"
            GoTo finresult
        End If

        If GroupName = "ITEM" Then
            strSql = "  SELECT CASE WHEN ISNULL(SUBITEM,'') <> '' THEN SUBITEM ELSE ITEM END AS PARTICULAR "
        Else
            strSql = "  SELECT ITEM AS PARTICULAR"
        End If
        strSql += vbCrLf + "  ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(LESSWT)LESSWT"
        strSql += vbCrLf + "  ,SUM(ISNULL(STNPCS,0))STNPCS,SUM(ISNULL(STNWT,0))STNWT,SUM(ISNULL(STNAMT,0))STNAMT"
        strSql += vbCrLf + "  ,SUM(ISNULL(DIAPCS,0))DIAPCS,SUM(ISNULL(DIAWT,0))DIAWT,SUM(ISNULL(DIAAMT,0))DIAAMT"
        strSql += vbCrLf + "  ,SUM(ISNULL(PREPCS,0))PREPCS,SUM(ISNULL(PREWT,0))PREWT,SUM(ISNULL(PREAMT,0))PREAMT"
        strSql += vbCrLf + "  ,SUM(ISNULL(RATE,0))[RATE (VAL)]"
        strSql += vbCrLf + "  ,SUM(ISNULL(AMOUNT,0))AMOUNT"
        strSql += vbCrLf + "  ,ITEMID,ITEM"
        If GroupName = "ITEM" Then
            strSql += vbCrLf + "  ,SUBITEMID,SUBITEM"
        ElseIf GroupName <> "NONE" Then
            strSql += vbCrLf + "  ," & GroupName & ""
        End If
        strSql += vbCrLf + "  ,1 RESULT,CONVERT(VARCHAR(1),'')COLHEAD"
        strSql += vbCrLf + "  INTO TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW"
        strSql += vbCrLf + "  FROM"
        strSql += vbCrLf + "  ("
        If rbtTag.Checked Or rbtBoth.Checked Then
            strSql += vbCrLf + "  SELECT SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(LESSWT)LESSWT"
            strSql += vbCrLf + "  ,SUM(ISNULL(STNPCS,0))STNPCS,SUM(ISNULL(STNWT,0))STNWT,SUM(ISNULL(STNAMT,0))STNAMT"
            strSql += vbCrLf + "  ,SUM(ISNULL(DIAPCS,0))DIAPCS,SUM(ISNULL(DIAWT,0))DIAWT,SUM(ISNULL(DIAAMT,0))DIAAMT"
            strSql += vbCrLf + "  ,SUM(ISNULL(PREPCS,0))PREPCS,SUM(ISNULL(PREWT,0))PREWT,SUM(ISNULL(PREAMT,0))PREAMT"
            strSql += vbCrLf + "  ,SUM(ISNULL(RATE,0))RATE"
            strSql += vbCrLf + "  ,SUM(ISNULL(AMOUNT,0))AMOUNT"
            strSql += vbCrLf + "  ,ITEMID,ITEM"
            If GroupName = "ITEM" Then
                strSql += vbCrLf + "  ,SUBITEMID,SUBITEM"
            ElseIf GroupName <> "NONE" Then
                strSql += vbCrLf + "  ," & GroupName & ""
            End If
            strSql += vbCrLf + "     FROM"
            strSql += vbCrLf + "     ("
            strSql += vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "TAGVIEW"
            strSql += vbCrLf + " )T"
            strSql += vbCrLf + " GROUP BY ITEMID,ITEM"
            If GroupName = "ITEM" Then
                strSql += vbCrLf + "  ,SUBITEMID,SUBITEM"
            ElseIf GroupName <> "NONE" Then
                strSql += vbCrLf + "  ," & GroupName & ""
            End If
        End If
        If rbtBoth.Checked Then
            strSql += vbCrLf + "  UNION ALL"
        End If
        If rbtNonTag.Checked Or rbtBoth.Checked Then
            strSql += vbCrLf + "  SELECT SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(LESSWT)LESSWT"
            strSql += vbCrLf + "  ,SUM(ISNULL(STNPCS,0))STNPCS,SUM(ISNULL(STNWT,0))STNWT,SUM(ISNULL(STNAMT,0))STNAMT"
            strSql += vbCrLf + "  ,SUM(ISNULL(DIAPCS,0))DIAPCS,SUM(ISNULL(DIAWT,0))DIAWT,SUM(ISNULL(DIAAMT,0))DIAAMT"
            strSql += vbCrLf + "  ,SUM(ISNULL(PREPCS,0))PREPCS,SUM(ISNULL(PREWT,0))PREWT,SUM(ISNULL(PREAMT,0))PREAMT"
            strSql += vbCrLf + "  ,SUM(ISNULL(RATE,0))RATE"
            strSql += vbCrLf + "  ,SUM(ISNULL(AMOUNT,0))AMOUNT"
            strSql += vbCrLf + "  ,ITEMID,ITEM"
            If GroupName = "ITEM" Then
                strSql += vbCrLf + "  ,SUBITEMID,SUBITEM"
            ElseIf GroupName <> "NONE" Then
                strSql += vbCrLf + "  ," & GroupName & ""
            End If
            strSql += vbCrLf + "     FROM"
            strSql += vbCrLf + "     ("
            strSql += vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "NONTAGVIEW"
            strSql += vbCrLf + " )T"
            strSql += vbCrLf + " GROUP BY ITEMID,ITEM"
            If GroupName = "ITEM" Then
                strSql += vbCrLf + "  ,SUBITEMID,SUBITEM"
            ElseIf GroupName <> "NONE" Then
                strSql += vbCrLf + "  ," & GroupName & ""
            End If
        End If
        strSql += vbCrLf + "  )X"
        strSql += vbCrLf + " GROUP BY ITEMID,ITEM"
        If GroupName = "ITEM" Then
            strSql += vbCrLf + "  ,SUBITEMID,SUBITEM"
        ElseIf GroupName <> "NONE" Then
            strSql += vbCrLf + "  ," & GroupName & ""
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()
        If GroupName <> "NONE" Then
            strSql = "  /* INSERTINT TITLE,SUBTOTAL,GRAND TOTAL */"
            strSql += vbCrLf + "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW) > 0"
            strSql += vbCrLf + "  BEGIN"
            strSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW(" & GroupName & ",PARTICULAR,RESULT,COLHEAD)"
            strSql += vbCrLf + "     SELECT DISTINCT " & GroupName & "," & GroupName & ",0,'T' FROM TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW"
            strSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW(" & GroupName & ",PARTICULAR"
            strSql += vbCrLf + "     ,PCS,GRSWT,LESSWT,NETWT"
            strSql += vbCrLf + "     ,STNPCS,STNWT,STNAMT"
            strSql += vbCrLf + "     ,DIAPCS,DIAWT,DIAAMT"
            strSql += vbCrLf + "     ,PREPCS,PREWT,PREAMT"
            strSql += vbCrLf + "     ,AMOUNT,RESULT,COLHEAD)"
            strSql += vbCrLf + "     SELECT DISTINCT " & GroupName & ",'SUB TOTAL'"
            strSql += vbCrLf + "     ,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(LESSWT,0)),SUM(ISNULL(NETWT,0))"
            strSql += vbCrLf + "     ,SUM(ISNULL(STNPCS,0)),SUM(ISNULL(STNWT,0)),SUM(ISNULL(STNAMT,0))"
            strSql += vbCrLf + "     ,SUM(ISNULL(DIAPCS,0)),SUM(ISNULL(DIAWT,0)),SUM(ISNULL(DIAAMT,0))"
            strSql += vbCrLf + "     ,SUM(ISNULL(PREPCS,0)),SUM(ISNULL(PREWT,0)),SUM(ISNULL(PREAMT,0))"
            strSql += vbCrLf + "     ,SUM(ISNULL(AMOUNT,0)),2,'S' FROM TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW WHERE RESULT = 1 GROUP BY " & GroupName & ""
            strSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW(" & GroupName & ",PARTICULAR"
            strSql += vbCrLf + "     ,PCS,GRSWT,LESSWT,NETWT"
            strSql += vbCrLf + "     ,STNPCS,STNWT,STNAMT"
            strSql += vbCrLf + "     ,DIAPCS,DIAWT,DIAAMT"
            strSql += vbCrLf + "     ,PREPCS,PREWT,PREAMT"
            strSql += vbCrLf + "     ,AMOUNT,RESULT,COLHEAD)"
            strSql += vbCrLf + "     SELECT DISTINCT 'ZZZZZZZZZZZZ','GRAND TOTAL'"
            strSql += vbCrLf + "     ,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(LESSWT,0)),SUM(ISNULL(NETWT,0))"
            strSql += vbCrLf + "     ,SUM(ISNULL(STNPCS,0)),SUM(ISNULL(STNWT,0)),SUM(ISNULL(STNAMT,0))"
            strSql += vbCrLf + "     ,SUM(ISNULL(DIAPCS,0)),SUM(ISNULL(DIAWT,0)),SUM(ISNULL(DIAAMT,0))"
            strSql += vbCrLf + "     ,SUM(ISNULL(PREPCS,0)),SUM(ISNULL(PREWT,0)),SUM(ISNULL(PREAMT,0))"
            strSql += vbCrLf + "     ,SUM(ISNULL(AMOUNT,0)),3,'G' FROM TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW WHERE RESULT = 2"
            strSql += vbCrLf + " END"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()
        ElseIf GroupName = "NONE" Then
            strSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW) > 0"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + "     INSERT INTO TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW(ITEM,PARTICULAR"
            strSql += vbCrLf + "     ,PCS,GRSWT,LESSWT,NETWT"
            strSql += vbCrLf + "     ,STNPCS,STNWT,STNAMT"
            strSql += vbCrLf + "     ,DIAPCS,DIAWT,DIAAMT"
            strSql += vbCrLf + "     ,PREPCS,PREWT,PREAMT"
            strSql += vbCrLf + "     ,AMOUNT,RESULT,COLHEAD)"
            strSql += vbCrLf + "     SELECT DISTINCT 'ZZZZZZ','GRAND TOTAL'"
            strSql += vbCrLf + "     ,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(LESSWT,0)),SUM(ISNULL(NETWT,0))"
            strSql += vbCrLf + "     ,SUM(ISNULL(STNPCS,0)),SUM(ISNULL(STNWT,0)),SUM(ISNULL(STNAMT,0))"
            strSql += vbCrLf + "     ,SUM(ISNULL(DIAPCS,0)),SUM(ISNULL(DIAWT,0)),SUM(ISNULL(DIAAMT,0))"
            strSql += vbCrLf + "     ,SUM(ISNULL(PREPCS,0)),SUM(ISNULL(PREWT,0)),SUM(ISNULL(PREAMT,0))"
            strSql += vbCrLf + "     ,SUM(ISNULL(AMOUNT,0)),3,'G' FROM TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW WHERE RESULT = 1"
            strSql += vbCrLf + " END"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()
        End If

        strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW SET LESSWT = NULL WHERE LESSWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW SET STNPCS = NULL WHERE STNPCS = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW SET STNWT = NULL WHERE STNWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW SET DIAPCS = NULL WHERE DIAPCS = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW SET DIAWT = NULL WHERE DIAWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW SET PREPCS = NULL WHERE PREPCS = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW SET PREWT = NULL WHERE PREWT = 0"
        strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW SET [RATE (VAL)] = NULL WHERE [RATE (VAL)] = 0"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.CommandTimeout = 2000 : cmd.ExecuteNonQuery()
finresult:
        Prop_Sets()
        Dim dtGrid As New DataTable("SUMMARY")
        Dim dtCol As New DataColumn("KEYNO")
        dtCol.AutoIncrement = True
        dtCol.AutoIncrementSeed = 0
        dtCol.AutoIncrementStep = 1
        dtGrid.Columns.Add(dtCol)
        If chkwithSummaryCostName.Checked = True Then
        Else
            strSql = " SELECT PARTICULAR,PCS,GRSWT,LESSWT,NETWT,[RATE (VAL)],AMOUNT,STNPCS,STNWT,STNAMT,DIAPCS,DIAWT,DIAAMT,PREPCS,PREWT,PREAMT"
            strSql += vbCrLf + " ,ITEMID,RESULT,COLHEAD,'SUMMARY' TABLENAME "
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW"
            If GroupName <> "NONE" Then
                strSql += vbCrLf + " ORDER BY " & GroupName & ",RESULT,PARTICULAR"
            ElseIf GroupName = "NONE" Then
                strSql += vbCrLf + " ORDER BY ITEM,RESULT,PARTICULAR"
            End If
        End If
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        CreateFilterTable(GroupName)
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "ITEM WISE " + IIf(rbtIssue.Checked = True, "ISSUE", "RECEIPT") + " VIEW"
        Dim tit As String = "ITEM WISE " + IIf(rbtIssue.Checked = True, "ISSUE", "RECEIPT") + " SUMMARY VIEW"
        tit += "DATE FROM " + dtpFrom.Text + " TO " + dtpTo.Text
        tit += "(" & Replace(chkMetalName, "'", "") & ")"
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        AddHandler objGridShower.gridView.KeyDown, AddressOf DataGridView_KeyDown
        AddHandler objGridShower.gridView.KeyPress, AddressOf DataGridView_KeyPress
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = True
        objGridShower.pnlFooter.BorderStyle = BorderStyle.Fixed3D
        objGridShower.pnlFooter.Size = New Size(objGridShower.pnlFooter.Width, 20)
        objGridShower.lblStatus.Text = "<Press [D] for Detail View>"
        If chkwithSummaryCostName.Checked = True Then
            If objGridShower.gridView.Columns.Contains("KEYNO") = True Then
                objGridShower.gridView.Columns("KEYNO").Visible = False
            End If
            For i As Integer = 0 To objGridShower.gridView.Columns.Count - 1
                If objGridShower.gridView.Columns(i).Name.Contains("_PCS") Then
                    objGridShower.gridView.Columns(i).HeaderText = "PCS"
                End If
                If objGridShower.gridView.Columns(i).Name.Contains("_GRSWT") Then
                    objGridShower.gridView.Columns(i).HeaderText = "GRSWT"
                End If
                If objGridShower.gridView.Columns(i).Name.Contains("_NETWT") Then
                    objGridShower.gridView.Columns(i).HeaderText = "NETWT"
                End If
                If objGridShower.gridView.Columns(i).Name.Contains("_STNPCS") Then
                    objGridShower.gridView.Columns(i).HeaderText = "STNPCS"
                End If
                If objGridShower.gridView.Columns(i).Name.Contains("_STNWT") Then
                    objGridShower.gridView.Columns(i).HeaderText = "STNWT"
                End If
                If objGridShower.gridView.Columns(i).Name.Contains("_DIAPCS") Then
                    objGridShower.gridView.Columns(i).HeaderText = "DIAPCS"
                End If
                If objGridShower.gridView.Columns(i).Name.Contains("_DIAWT") Then
                    objGridShower.gridView.Columns(i).HeaderText = "DIAWT"
                End If
            Next
        Else
            DataGridView_SummaryFormatting(objGridShower.gridView)
        End If
        FormatGridColumns(objGridShower.gridView, False, False, , False)
        objGridShower.Show()
        If chkwithSummaryCostName.Checked = True Then
            objGridShower.FormReSize = True
            objGridShower.FormReLocation = True
            objGridShower.gridViewHeader.Visible = True
            GridViewHeaderCreator(objGridShower.gridViewHeader)
            FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
            'objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("ITEMCOUNTERNAME")))
        End If
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
    End Sub
    Private Sub gridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        SetGridHeadColWidth(CType(sender, DataGridView))
    End Sub
    Private Sub GridViewHeaderCreator(ByVal gridviewHead As DataGridView)
        Dim dtHead As New DataTable
        strSql = " SELECT ''[COUNTERNAME]"
        strSql += vbCrLf + " ,''[STOCKTYPE]"
        If apiv.Length > 0 Then
            For i As Integer = 0 To apiv.Length - 1
                strSql += vbCrLf + " ,'' [" & apiv(i) & "_PCS~" & apiv(i) & "_GRSWT~" & apiv(i) & "_NETWT~" & apiv(i) & "_STNPCS~" & apiv(i) & "_STNWT~" & apiv(i) & "_DIAPCS~" & apiv(i) & "_DIAWT] "
            Next
        End If
        strSql += vbCrLf + " ,'' [TOTAL_PCS~TOTAL_GRSWT~TOTAL_NETWT~TOTAL_STNPCS~TOTAL_STNWT~TOTAL_DIAPCS~TOTAL_DIAWT] "
        strSql += " ,'' SCROLL"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtHead)
        gridviewHead.DataSource = dtHead
        gridviewHead.Columns("COUNTERNAME").HeaderText = ""
        gridviewHead.Columns("STOCKTYPE").HeaderText = ""
        If apiv.Length > 0 Then
            For i As Integer = 0 To apiv.Length - 1
                Dim aa As String = apiv(i) & "_PCS~" & apiv(i) & "_GRSWT~" & apiv(i) & "_NETWT~" & apiv(i) & "_STNPCS~" & apiv(i) & "_STNWT~" & apiv(i) & "_DIAPCS~" & apiv(i) & "_DIAWT"
                gridviewHead.Columns(aa).HeaderText = apiv(i).ToString
            Next
        End If
        gridviewHead.Columns("TOTAL_PCS~TOTAL_GRSWT~TOTAL_NETWT~TOTAL_STNPCS~TOTAL_STNWT~TOTAL_DIAPCS~TOTAL_DIAWT").HeaderText = "TOTAL"
        gridviewHead.Columns("SCROLL").HeaderText = ""
        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        gridviewHead.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 9, FontStyle.Bold)
        SetGridHeadColWidth(gridviewHead)
    End Sub
    Private Sub CreateFilterTable(ByVal GroupName As String)
        With objGridShower.dtFilteration
            Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre)
            Dim chkDesigner As String = GetChecked_CheckedList(chkLstDesigner)
            Dim chkItemCounter As String = GetChecked_CheckedList(chkLstItemCounter)
            Dim chkCategory As String = GetChecked_CheckedList(chkLstCategory)
            .Columns.Add(dtpFrom.Name, GetType(Date))
            .Columns.Add(dtpTo.Name, GetType(Date))
            .Columns.Add(rbtIssue.Name, GetType(Boolean))
            .Columns.Add(rbtReceipt.Name, GetType(Boolean))
            .Columns.Add(rbtTag.Name, GetType(Boolean))
            .Columns.Add(rbtNonTag.Name, GetType(Boolean))
            .Columns.Add(rbtBoth.Name, GetType(Boolean))
            .Columns.Add(chkLstCostCentre.Name, GetType(String))
            .Columns.Add(chkLstDesigner.Name, GetType(String))
            .Columns.Add(chkLstItemCounter.Name, GetType(String))
            .Columns.Add(chkLstCategory.Name, GetType(String))
            .Columns.Add("GROUPNAME", GetType(String))
            Dim ro As DataRow = .NewRow
            ro.Item(dtpFrom.Name) = dtpFrom.Value.ToString("yyyy-MM-dd")
            ro.Item(dtpTo.Name) = dtpTo.Value.ToString("yyyy-MM-dd")
            ro.Item(rbtIssue.Name) = rbtIssue.Checked
            ro.Item(rbtReceipt.Name) = rbtReceipt.Checked
            ro.Item(rbtTag.Name) = rbtTag.Checked
            ro.Item(rbtNonTag.Name) = rbtNonTag.Checked
            ro.Item(rbtBoth.Name) = rbtBoth.Checked
            ro.Item(chkLstCostCentre.Name) = GetChecked_CheckedList(chkLstCostCentre)
            ro.Item(chkLstDesigner.Name) = GetChecked_CheckedList(chkLstDesigner)
            ro.Item(chkLstItemCounter.Name) = GetChecked_CheckedList(chkLstItemCounter)
            ro.Item(chkLstCategory.Name) = GetChecked_CheckedList(chkLstCategory)
            ro.Item("GROUPNAME") = GroupName
            .Rows.Add(ro)
        End With
    End Sub
    Private Sub DataGridView_SummaryFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("PARTICULAR").Width = 150
            .Columns("PCS").Width = 70
            .Columns("GRSWT").Width = 80
            .Columns("LESSWT").Width = 60
            .Columns("NETWT").Width = 80
            .Columns("RATE (VAL)").Width = 80
            .Columns("AMOUNT").Width = 100
            .Columns("STNPCS").Width = 60
            .Columns("STNWT").Width = 70
            .Columns("DIAPCS").Width = 60
            .Columns("DIAWT").Width = 70
            .Columns("PREPCS").Width = 60
            .Columns("PREWT").Width = 70

            .Columns("ITEMID").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("TABLENAME").Visible = False
            .Columns("KEYNO").Visible = False
            FormatGridColumns(dgv, False, False, , False)
            .Columns("DIAWT").DefaultCellStyle.Format = FormatNumberStyle(DiaRnd)
            FillGridGroupStyle_KeyNoWise(dgv)
            .Columns("RATE (VAL)").DefaultCellStyle.Format = "0.00"
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
        End With
    End Sub

    Private Sub DataGridView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            Dim dgv As DataGridView = CType(sender, DataGridView)
            If Not dgv.RowCount > 0 Then Exit Sub
            If dgv.Rows(0).Cells("TABLENAME").Value.ToString = "DETAILED" Then
                Dim f As frmGridDispDia
                f = objGPack.GetParentControl(dgv)
                f.FormReSize = False
                f.gridView.DataSource = Nothing
                f.gridView.DataSource = f.dsGrid.Tables("SUMMARY")
                f.gridView.CurrentCell = f.gridView.FirstDisplayedCell
                DataGridView_SummaryFormatting(f.gridView)
                Dim frmDate As Date = f.GetFilterStr(dtpFrom.Name)
                Dim toDate As Date = f.GetFilterStr(dtpTo.Name)
                f.Text = "ITEM WISE " + IIf(rbtIssue.Checked = True, "ISSUE", "RECEIPT") + " VIEW"
                Dim tit As String = "ITEM WISE " + IIf(rbtIssue.Checked = True, "ISSUE", "RECEIPT") + " SUMMARY VIEW" + vbCrLf
                tit += "DATE FROM " + Format(frmDate.Day, "00") + "/" + Format(frmDate.Month, "00") + "/" + frmDate.Year.ToString
                tit += " TO " + Format(toDate.Day, "00") + "/" + Format(toDate.Month, "00") + "/" + toDate.Year.ToString
                f.lblStatus.Text = "<Press [D] for Detail View>"
                f.FormReSize = True
                f.gridView.Columns(0).Width = f.gridView.Columns(0).Width + 1
            End If
        End If
    End Sub

    Private Sub DataGridView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If UCase(e.KeyChar) = "D" Then
            Dim dgv As DataGridView = CType(sender, DataGridView)
            If dgv.Rows(0).Cells("TABLENAME").Value.ToString = "SUMMARY" Then
                If dgv.CurrentRow Is Nothing Then Exit Sub
                If dgv.CurrentRow.Cells("RESULT").Value.ToString <> "1" Then Exit Sub
                Dim f As frmGridDispDia
                f = objGPack.GetParentControl(dgv)
                Dim itemId As Integer = Val(dgv.CurrentRow.Cells("ITEMID").Value.ToString)
                Dim dt As DataTable = FillDetailView(itemId, f)
                If Not dt.Rows.Count > 0 Then
                    MsgBox("Record not found", MsgBoxStyle.Information)
                    Exit Sub
                End If
                If f.dsGrid.Tables.Contains(dt.TableName) Then f.dsGrid.Tables.Remove(dt.TableName)
                f.Text = "ITEM WISE " + IIf(rbtIssue.Checked = True, "ISSUE", "RECEIPT") + " VIEW"
                Dim frmDate As Date = f.GetFilterStr(dtpFrom.Name)
                Dim toDate As Date = f.GetFilterStr(dtpTo.Name)
                Dim tit As String = "ITEM WISE " + IIf(rbtIssue.Checked = True, "ISSUE", "RECEIPT") + " DETAIL VIEW FOR " + objGPack.GetSqlValue("SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = " & itemId & "") + vbCrLf
                tit += "DATE FROM " + Format(frmDate.Day, "00") + "/" + Format(frmDate.Month, "00") + "/" + frmDate.Year.ToString
                tit += " TO " + Format(toDate.Day, "00") + "/" + Format(toDate.Month, "00") + "/" + toDate.Year.ToString
                f.lblTitle.Text = tit

                f.dsGrid.Tables.Add(dt)
                f.FormReSize = False
                f.FormReLocation = False
                f.gridView.DataSource = Nothing
                f.gridView.DataSource = f.dsGrid.Tables("DETAILED")
                f.gridView.CurrentCell = f.gridView.FirstDisplayedCell
                DataGridView_DetailViewFormatting(f.gridView)
                f.lblStatus.Text = "<Press [ESCAPE] for Summary View>"
                f.FormReSize = True
                f.gridView.Columns(0).Width = f.gridView.Columns(0).Width + 1
                f.gridView.CurrentCell = f.gridView.FirstDisplayedCell
                f.gridView.Select()
            End If
        End If
    End Sub

    Private Function FillDetailView(ByVal itemId As Integer, ByVal f As frmGridDispDia) As DataTable
        Dim GroupName As String = f.GetFilterStr("GROUPNAME")
        Dim chkCostName As String = f.GetFilterStr(chkLstCostCentre.Name)
        Dim chkDesigner As String = f.GetFilterStr(chkLstDesigner.Name)
        Dim chkItemCounter As String = f.GetFilterStr(chkLstItemCounter.Name)
        Dim chkCategory As String = f.GetFilterStr(chkLstCategory.Name)

        strSql = "  IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "STOCKVIEW_DET')> 0 DROP TABLE TEMP" & systemId & "STOCKVIEW_DET"
        strSql += vbCrLf + "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "STOCKVIEW')> 0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = ""
        If CType(f.GetFilterStr(rbtTag.Name), Boolean) Or CType(f.GetFilterStr(rbtBoth.Name), Boolean) Then
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  PCS,GRSWT,NETWT,LESSWT,RATE"
            strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNPCS"
            strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNWT"
            strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAPCS"
            strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAWT"
            strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREPCS"
            strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREWT"
            strSql += vbCrLf + "  ,SALVALUE AMOUNT"
            If GroupName = "COUNTER" Then
                strSql += vbCrLf + "     ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)AS " & GroupName & ""
            ElseIf GroupName = "DESIGNER" Then
                strSql += vbCrLf + "    ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS " & GroupName & ""
            ElseIf GroupName = "COSTCENTRE" Then
                strSql += vbCrLf + "    ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T." & IIf(ChkOrgCostCentre.Checked = True, "TCOSTID", "COSTID") & ")AS " & GroupName & ""
            End If
            strSql += vbCrLf + "  ," & IIf(chkActualDate.Checked, "ACTUALRECDATE RECDATE", "RECDATE") & ",TAGNO,TAGVAL"
            strSql += vbCrLf + "  ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
            strSql += vbCrLf + "  ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID)AS SUBITEM"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR(15),SNO)SNO,ITEMID,SUBITEMID"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW"
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMTAG AS T"
            If CType(f.GetFilterStr(rbtIssue.Name), Boolean) Then
                strSql += vbCrLf + "  WHERE ISSDATE BETWEEN '" & f.GetFilterStr(dtpFrom.Name) & "' AND '" & f.GetFilterStr(dtpTo.Name) & "'"
                strSql += vbCrLf + "  AND ISSDATE IS NOT NULL"
            ElseIf CType(f.GetFilterStr(rbtReceipt.Name), Boolean) Then
                strSql += vbCrLf + "  WHERE " & IIf(chkActualDate.Checked, "ACTUALRECDATE", "RECDATE") & " BETWEEN '" & f.GetFilterStr(dtpFrom.Name) & "' AND '" & f.GetFilterStr(dtpTo.Name) & "'"
                'strSql += vbCrLf + "  AND ISSDATE IS NULL"
            End If
            strSql += vbCrLf + "  AND ITEMID = " & itemId & ""
            If chkCategory <> "" Then strSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & ")))"
            If Not chkCostCentreSelectAll.Checked Then
                If chkCostName <> "" Then strSql += vbCrLf + " AND " & IIf(ChkOrgCostCentre.Checked = True, "TCOSTID", "COSTID") & " IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            End If
            If Not chkDesignerSelectAll.Checked Then
                If chkDesigner <> "" Then strSql += vbCrLf + " AND DESIGNERID IN (sELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & chkDesigner & "))"
            End If
            If Not chkItemCounterSelectAll.Checked Then
                If chkItemCounter <> "" Then strSql += vbCrLf + " AND ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & chkItemCounter & "))"
            End If
            If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
                strSql += vbCrLf + " AND T.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
            Else
                strSql += vbCrLf + " AND T.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
            End If
            If txtLotno_NUM.Text <> "" Then strSql += vbCrLf + " AND T.LOTSNO in  (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE ISNULL(LOTNO,0) = " & Val(txtLotno_NUM.Text) & ")"
        End If
        If CType(f.GetFilterStr(rbtBoth.Name), Boolean) Then
            strSql += vbCrLf + "  UNION ALL"
        End If
        If CType(f.GetFilterStr(rbtNonTag.Name), Boolean) Or CType(f.GetFilterStr(rbtBoth.Name), Boolean) Then
            strSql += vbCrLf + "  SELECT "
            strSql += vbCrLf + "  PCS,GRSWT,NETWT,LESSWT,CONVERT(NUMERIC(15,2),0) RATE"
            strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNPCS"
            strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S'))STNWT"
            strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAPCS"
            strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D'))DIAWT"
            strSql += vbCrLf + "  ,(SELECT SUM(STNPCS) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREPCS"
            strSql += vbCrLf + "  ,(SELECT SUM(STNWT) FROM " & cnAdminDb & "..ITEMNONTAGSTONE WHERE TAGSNO = T.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P'))PREWT"
            strSql += vbCrLf + "  ,CONVERT(NUMERIC(15,2),NULL) AMOUNT"
            If GroupName = "COUNTER" Then
                strSql += vbCrLf + "     ,(SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRID = T.ITEMCTRID)AS " & GroupName & ""
            ElseIf GroupName = "DESIGNER" Then
                strSql += vbCrLf + "    ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.DESIGNERID)AS " & GroupName & ""
            ElseIf GroupName = "COSTCENTRE" Then
                strSql += vbCrLf + "    ,(SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTID = T.COSTID)AS " & GroupName & ""
            End If
            strSql += vbCrLf + "  ,RECDATE,CONVERT(VARCHAR(15),NULL)TAGNO,CONVERT(INT,NULL)TAGVAL"
            strSql += vbCrLf + "  ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEM"
            strSql += vbCrLf + "  ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID)AS SUBITEM"
            strSql += vbCrLf + "  ,CONVERT(VARCHAR(15),SNO)SNO,ITEMID,SUBITEMID"
            If CType(f.GetFilterStr(rbtNonTag.Name), Boolean) Then
                strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW"
            End If
            strSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMNONTAG AS T"
            If CType(f.GetFilterStr(rbtIssue.Name), Boolean) Then
                strSql += vbCrLf + "  WHERE RECDATE BETWEEN '" & f.GetFilterStr(dtpFrom.Name) & "' AND '" & f.GetFilterStr(dtpTo.Name) & "'"
                strSql += vbCrLf + "  AND RECISS = 'I'"
            ElseIf CType(f.GetFilterStr(rbtReceipt.Name), Boolean) Then
                strSql += vbCrLf + "  WHERE RECDATE BETWEEN '" & f.GetFilterStr(dtpFrom.Name) & "' AND '" & f.GetFilterStr(dtpTo.Name) & "'"
                strSql += vbCrLf + "  AND RECISS = 'R'"
            End If
            strSql += vbCrLf + "  AND ITEMID = " & itemId & ""
            If chkCategory <> "" Then strSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & ")))"
            If Not chkCostCentreSelectAll.Checked Then
                If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            End If
            If Not chkDesignerSelectAll.Checked Then
                If chkDesigner <> "" Then strSql += vbCrLf + " AND DESIGNERID IN (sELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME IN (" & chkDesigner & "))"
            End If
            If Not chkItemCounterSelectAll.Checked Then
                If chkItemCounter <> "" Then strSql += vbCrLf + " AND ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & chkItemCounter & "))"
            End If
            If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
                strSql += vbCrLf + " AND T.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE COMPANYNAME IN (" & GetQryString(chkCmbCompany.Text) & "))"
            Else
                strSql += vbCrLf + " AND T.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
            End If
            If txtLotno_NUM.Text <> "" Then strSql += vbCrLf + " AND T.LOTSNO in  (SELECT SNO FROM " & cnAdminDb & "..ITEMLOT WHERE ISNULL(LOTNO,0) = " & Val(txtLotno_NUM.Text) & ")"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW"
        strSql += vbCrLf + " SET PCS = null,GRSWT = null,NETWT = null,DIAPCS = L.PCS"
        strSql += vbCrLf + " ,DIAWT = L.GRSWT"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW AS L"
        strSql += vbCrLf + " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = 'D')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW"
        strSql += vbCrLf + " SET PCS = null,GRSWT = null,NETWT = null,STNPCS = L.PCS"
        strSql += vbCrLf + " ,STNWT = L.GRSWT"
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW AS L"
        strSql += vbCrLf + " WHERE L.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID = 'T')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        If GroupName = "ITEM" Then
            strSql = "  SELECT CASE WHEN ISNULL(SUBITEM,'') <> '' THEN SUBITEM ELSE ITEM END AS PARTICULAR "
        Else
            strSql = "  SELECT ITEM AS PARTICULAR,SUBITEM"
        End If
        strSql += vbCrLf + "  ,SUM(PCS)PCS,SUM(GRSWT)GRSWT,SUM(NETWT)NETWT,SUM(LESSWT)LESSWT"
        strSql += vbCrLf + "  ,SUM(ISNULL(STNPCS,0))STNPCS,SUM(ISNULL(STNWT,0))STNWT"
        strSql += vbCrLf + "  ,SUM(ISNULL(DIAPCS,0))DIAPCS,SUM(ISNULL(DIAWT,0))DIAWT"
        strSql += vbCrLf + "  ,SUM(ISNULL(PREPCS,0))PREPCS,SUM(ISNULL(PREWT,0))PREWT"
        strSql += vbCrLf + "  ,RATE"
        strSql += vbCrLf + "  ,SUM(ISNULL(AMOUNT,0))AMOUNT"
        strSql += vbCrLf + "  ,ITEMID,ITEM"
        If GroupName = "ITEM" Then
            strSql += vbCrLf + "  ,SUBITEMID,SUBITEM"
        ElseIf GroupName <> "NONE" Then
            strSql += vbCrLf + "  ," & GroupName & ""
        End If
        strSql += vbCrLf + "  ,RECDATE,TAGNO,TAGVAL,SNO,1 RESULT,CONVERT(VARCHAR(1),'')COLHEAD"
        strSql += vbCrLf + "  INTO TEMP" & systemId & "STOCKVIEW_DET"
        strSql += vbCrLf + "  FROM"
        strSql += vbCrLf + "  ("
        strSql += vbCrLf + " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "STOCKVIEW"
        strSql += vbCrLf + "  )X"
        strSql += vbCrLf + " GROUP BY ITEMID,ITEM,RECDATE,TAGNO,TAGVAL,SNO,RATE,SUBITEM"
        If GroupName = "ITEM" Then
            strSql += vbCrLf + "  ,SUBITEMID,SUBITEM"
        ElseIf GroupName <> "NONE" Then
            strSql += vbCrLf + "  ," & GroupName & ""
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        'strSql = "  INSERT INTO TEMP" & systemid & "STOCKVIEW_DET(COUNTER,PARTICULAR,RESULT,COLHEAD)"
        'STRSQL += VBCRLF + "  SELECT DISTINCT COUNTER,COUNTER,0,'T' FROM TEMP" & systemid & "STOCKVIEW_DET"
        If GroupName <> "NONE" Then
            strSql = "  /* INSERTINT TITLE,SUBTOTAL,GRAND TOTAL */"
            strSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "STOCKVIEW_DET(" & GroupName & ",PARTICULAR,RESULT,COLHEAD)"
            strSql += vbCrLf + "  SELECT DISTINCT " & GroupName & "," & GroupName & ",0,'T' FROM TEMP" & systemId & "STOCKVIEW_DET"
            strSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "STOCKVIEW_DET(" & GroupName & ",PARTICULAR"
            strSql += vbCrLf + "  ,PCS,GRSWT,LESSWT,NETWT"
            strSql += vbCrLf + "  ,STNPCS,STNWT"
            strSql += vbCrLf + "  ,DIAPCS,DIAWT"
            strSql += vbCrLf + "  ,PREPCS,PREWT"
            strSql += vbCrLf + "  ,AMOUNT,RESULT,COLHEAD)"
            strSql += vbCrLf + "  SELECT DISTINCT " & GroupName & ",'SUB TOTAL'"
            strSql += vbCrLf + " ,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(LESSWT,0)),SUM(ISNULL(NETWT,0))"
            strSql += vbCrLf + " ,SUM(ISNULL(STNPCS,0)),SUM(ISNULL(STNWT,0))"
            strSql += vbCrLf + " ,SUM(ISNULL(DIAPCS,0)),SUM(ISNULL(DIAWT,0))"
            strSql += vbCrLf + " ,SUM(ISNULL(PREPCS,0)),SUM(ISNULL(PREWT,0))"
            strSql += vbCrLf + " ,SUM(ISNULL(AMOUNT,0)),2,'S' FROM TEMP" & systemId & "STOCKVIEW_DET WHERE RESULT = 1 GROUP BY " & GroupName & ""
            strSql += vbCrLf + "  INSERT INTO TEMP" & systemId & "STOCKVIEW_DET(" & GroupName & ",PARTICULAR"
            strSql += vbCrLf + "  ,PCS,GRSWT,LESSWT,NETWT"
            strSql += vbCrLf + "  ,STNPCS,STNWT"
            strSql += vbCrLf + "  ,DIAPCS,DIAWT"
            strSql += vbCrLf + "  ,PREPCS,PREWT"
            strSql += vbCrLf + "  ,AMOUNT,RESULT,COLHEAD)"
            strSql += vbCrLf + "  SELECT DISTINCT 'ZZZZZZZZZ','GRAND TOTAL'"
            strSql += vbCrLf + " ,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(LESSWT,0)),SUM(ISNULL(NETWT,0))"
            strSql += vbCrLf + " ,SUM(ISNULL(STNPCS,0)),SUM(ISNULL(STNWT,0))"
            strSql += vbCrLf + " ,SUM(ISNULL(DIAPCS,0)),SUM(ISNULL(DIAWT,0))"
            strSql += vbCrLf + " ,SUM(ISNULL(PREPCS,0)),SUM(ISNULL(PREWT,0))"
            strSql += vbCrLf + " ,SUM(ISNULL(AMOUNT,0)),3,'G' FROM TEMP" & systemId & "STOCKVIEW_DET WHERE RESULT = 2"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        ElseIf GroupName = "NONE" Then
            strSql = "  INSERT INTO TEMP" & systemId & "STOCKVIEW_DET(ITEM,PARTICULAR"
            strSql += vbCrLf + "  ,PCS,GRSWT,LESSWT,NETWT"
            strSql += vbCrLf + "  ,STNPCS,STNWT"
            strSql += vbCrLf + "  ,DIAPCS,DIAWT"
            strSql += vbCrLf + "  ,PREPCS,PREWT"
            strSql += vbCrLf + "  ,AMOUNT,RESULT,COLHEAD)"
            strSql += vbCrLf + "  SELECT DISTINCT 'ZZZZZZZZ','GRAND TOTAL'"
            strSql += vbCrLf + " ,SUM(ISNULL(PCS,0)),SUM(ISNULL(GRSWT,0)),SUM(ISNULL(LESSWT,0)),SUM(ISNULL(NETWT,0))"
            strSql += vbCrLf + " ,SUM(ISNULL(STNPCS,0)),SUM(ISNULL(STNWT,0))"
            strSql += vbCrLf + " ,SUM(ISNULL(DIAPCS,0)),SUM(ISNULL(DIAWT,0))"
            strSql += vbCrLf + " ,SUM(ISNULL(PREPCS,0)),SUM(ISNULL(PREWT,0))"
            strSql += vbCrLf + " ,SUM(ISNULL(AMOUNT,0)),3,'G' FROM TEMP" & systemId & "STOCKVIEW_DET WHERE RESULT = 1"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
            cmd.ExecuteNonQuery()
        End If

        strSql = " SELECT PARTICULAR,SUBITEM,RECDATE,TAGNO,PCS,GRSWT,LESSWT,NETWT,RATE,AMOUNT,STNPCS,STNWT,DIAPCS,DIAWT,PREPCS,PREWT"
        strSql += vbCrLf + " ,SNO,RESULT,COLHEAD,'DETAILED' TABLENAME "
        strSql += vbCrLf + " FROM TEMP" & systemId & "STOCKVIEW_DET"
        If GroupName <> "NONE" Then
            strSql += vbCrLf + " ORDER BY " & GroupName & ",RESULT,TAGVAL"
        ElseIf GroupName = "NONE" Then
            strSql += vbCrLf + " ORDER BY RESULT,TAGVAL"
        End If

        Dim dtGrid As New DataTable("DETAILED")
        Dim dtCol As New DataColumn("KEYNO")
        dtCol.AutoIncrement = True
        dtCol.AutoIncrementSeed = 0
        dtCol.AutoIncrementStep = 1
        dtGrid.Columns.Add(dtCol)
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        Return dtGrid
    End Function

    Private Sub DataGridView_DetailViewFormatting(ByVal dgv As DataGridView)
        With dgv
            For Each dgvCol As DataGridViewColumn In dgv.Columns
                dgvCol.Visible = True
            Next
            .Columns("PARTICULAR").Width = 150
            .Columns("SUBITEM").Width = 100
            .Columns("RECDATE").Width = 80
            .Columns("TAGNO").Width = 80
            .Columns("GRSWT").Width = 80
            .Columns("LESSWT").Width = 60
            .Columns("NETWT").Width = 80
            .Columns("RATE").Width = 80
            .Columns("STNPCS").Width = 60
            .Columns("STNWT").Width = 70
            .Columns("DIAPCS").Width = 60
            .Columns("DIAWT").Width = 70
            .Columns("PREPCS").Width = 60
            .Columns("PREWT").Width = 70
            .Columns("AMOUNT").Width = 100

            .Columns("SNO").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("TABLENAME").Visible = False
            .Columns("KEYNO").Visible = False
            FormatGridColumns(dgv, False, , , False)
            FillGridGroupStyle_KeyNoWise(dgv)

            .Columns("RATE").DefaultCellStyle.Format = "0.00"
            .Columns("AMOUNT").DefaultCellStyle.Format = "0.00"
        End With
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        strSql = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        strSql += " ORDER BY RESULT,COMPANYNAME"
        dtCompany = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCompany)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, dtCompany, "COMPANYNAME", , strCompanyName)

        ' rbtIssue.Checked = True
        ' chkItemIdOrder.Checked = False
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        ' cmbGroupBy.Text = "NONE"
        '  rbtTag.Checked = True
        rbtIssue.Select()
        Prop_Gets()
        apiv = Nothing
    End Sub

    Private Sub frmItemWiseStockSummary_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmItemWiseStockSummary_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        dtpFrom.MinimumDate = (New DateTimePicker).MinDate
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ACTIVE = 'Y' ORDER BY METALNAME"
        FillCheckedListBox(strSql, chkLstMetal, True, chkMetalSelectAll.Checked)
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkLstCostCentre.Enabled = True
            chkLstCostCentre.Items.Clear()
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            'FillCheckedListBox(strSql, chkLstCostCentre, True, chkCostCentreSelectAll.Checked)
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
        FillCheckedListBox(strSql, chkLstDesigner, True, chkDesignerSelectAll.Checked)
        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' ORDER BY ITEMCTRNAME"
        FillCheckedListBox(strSql, chkLstItemCounter, True, chkItemCounterSelectAll.Checked)

        cmbGroupBy.Items.Clear()
        cmbGroupBy.Items.Add("NONE")
        cmbGroupBy.Items.Add("ITEM WISE")
        cmbGroupBy.Items.Add("COUNTER WISE")
        cmbGroupBy.Items.Add("DESIGNER WISE")
        cmbGroupBy.Items.Add("COST CENTRE")
        btnNew_Click(Me, New EventArgs)
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

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub chkLstMetal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstMetal.LostFocus
        Dim chkMetalNames As String = GetChecked_CheckedList(chkLstMetal)
        chkLstCategory.Items.Clear()
        strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
        If chkMetalNames <> "" Then
            strSql += " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalNames & "))"
        End If
        strSql += " ORDER BY CATNAME"
        FillCheckedListBox(strSql, chkLstCategory, True, chkCategorySelectAll.Checked)
    End Sub
    Private Sub chkCategorySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCategorySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCategory, chkCategorySelectAll.Checked)
    End Sub
    Private Sub Prop_Sets()
        Dim obj As New frmItemWiseStockSummary_Properties
        obj.p_rbtIssue = rbtIssue.Checked
        obj.p_rbtReceipt = rbtReceipt.Checked
        GetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany)
        'obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_chkMetalSelectAll = chkMetalSelectAll.Checked
        GetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal)
        obj.p_chkCategorySelectAll = chkCategorySelectAll.Checked
        GetChecked_CheckedList(chkLstCategory, obj.p_chkLstCategory)
        obj.p_chkDesignerSelectAll = chkDesignerSelectAll.Checked
        GetChecked_CheckedList(chkLstDesigner, obj.p_chkLstDesigner)
        obj.p_chkItemCounterSelectAll = chkItemCounterSelectAll.Checked
        GetChecked_CheckedList(chkLstItemCounter, obj.p_chkLstItemCounter)
        obj.p_rbtTag = rbtTag.Checked
        obj.p_rbtNonTag = rbtNonTag.Checked
        obj.p_rbtBoth = rbtBoth.Checked
        obj.p_cmbGroupBy = cmbGroupBy.Text
        obj.p_chkItemIdOrder = chkItemIdOrder.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmItemWiseStockSummary_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmItemWiseStockSummary_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmItemWiseStockSummary_Properties))
        rbtIssue.Checked = obj.p_rbtIssue
        rbtReceipt.Checked = obj.p_rbtReceipt
        SetChecked_CheckedList(chkCmbCompany, obj.p_chkCmbCompany, strCompanyName)
        'chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        chkMetalSelectAll.Checked = obj.p_chkMetalSelectAll
        SetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal, Nothing)
        chkCategorySelectAll.Checked = obj.p_chkCategorySelectAll
        SetChecked_CheckedList(chkLstCategory, obj.p_chkLstCategory, Nothing)
        chkDesignerSelectAll.Checked = obj.p_chkDesignerSelectAll
        SetChecked_CheckedList(chkLstDesigner, obj.p_chkLstDesigner, Nothing)
        chkItemCounterSelectAll.Checked = obj.p_chkItemCounterSelectAll
        SetChecked_CheckedList(chkLstItemCounter, obj.p_chkLstItemCounter, Nothing)
        rbtTag.Checked = obj.p_rbtTag
        rbtNonTag.Checked = obj.p_rbtNonTag
        rbtBoth.Checked = obj.p_rbtBoth
        cmbGroupBy.Text = obj.p_cmbGroupBy
        chkItemIdOrder.Checked = obj.p_chkItemIdOrder
    End Sub

End Class

Public Class frmItemWiseStockSummary_Properties
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
    Private chkCmbCompany As New List(Of String)
    Public Property p_chkCmbCompany() As List(Of String)
        Get
            Return chkCmbCompany
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCompany = value
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
    Private cmbGroupBy As String = "NONE"
    Public Property p_cmbGroupBy() As String
        Get
            Return cmbGroupBy
        End Get
        Set(ByVal value As String)
            cmbGroupBy = value
        End Set
    End Property
    Private chkItemIdOrder As Boolean = False
    Public Property p_chkItemIdOrder() As Boolean
        Get
            Return chkItemIdOrder
        End Get
        Set(ByVal value As Boolean)
            chkItemIdOrder = value
        End Set
    End Property
    

End Class