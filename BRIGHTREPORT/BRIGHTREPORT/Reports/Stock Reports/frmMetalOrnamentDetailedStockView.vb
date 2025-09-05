Imports System.Data.OleDb
Imports System.IO
Public Class frmMetalOrnamentDetailedStockView
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim objGridShower As frmGridDispDia
    Dim Weight As String = "GRSWT"
    Dim dtItem As New DataTable

    Dim chkCostName As String = ""
    Dim chkMetal As String = ""
    Dim chkCategory As String = ""
    Dim TranType As String = ""
    Dim Apptrantype As String = ""
    Dim Misctrantype As String = ""
    Dim FileWrite As StreamWriter
    Dim strprint As String
    Dim funcEndPrint As Boolean
    Dim PgNo As Integer
    Dim line As Integer
    Dim GS As String
    Dim gsformname As String = GetAdmindbSoftValue("GSFORMNAME", "GS11,GS12")
    Dim gstitlename As String = GetAdmindbSoftValue("GSTITLENAME", "")
    Dim Gs11Name As String = "GS11", Gs12Name As String = "GS12"
    Dim Format2 As Boolean = IIf(GetAdmindbSoftValue("GS_FORMAT2", "N") = "Y", True, False)
    Dim ExcludeItem As String = GetAdmindbSoftValue("CHIT_MINVA_EXCLITEM", "")
    Dim IncludeActype As String = GetAdmindbSoftValue("METALORNDETRPT_ACTYPE", "I")


    Function LoadItemName() As Integer
        Dim chkMetalNames As String = GetChecked_CheckedList(chkLstMetal)
        Dim chkCatNames As String = GetChecked_CheckedList(chkLstCategory)
        strSql = " SELECT 'ALL' ITEMNAME,'ALL' ITEMID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ITEMNAME,CONVERT(vARCHAR,ITEMID),2 RESULT FROM " & cnAdminDb & "..ITEMMAST"
        strSql += " WHERE ACTIVE = 'Y'"
        If chkMetalNames <> "" Then
            strSql += " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalNames & "))"
        End If
        If chkCatNames <> "" Then
            strSql += " AND CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCatNames & "))"
        End If
        strSql += " ORDER BY RESULT,ITEMNAME"
        dtItem = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtItem)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbItem, dtItem, "ITEMNAME", , "ALL")
    End Function

    Private Sub InsGsOutStDt(ByVal Tran As String, Optional ByVal InsComments As String = "")
        If Not (rbtWeight.Checked Or rbtBoth.Checked) Then Exit Sub
        strSql = ""
        strSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..OUTSTANDING WHERE GRSWT <> 0"
        If chkCostName <> "" Then strSql += vbCrLf + "  AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        strSql += vbCrLf + " )>0"
        strSql += vbCrLf + " BEGIN"
        If Tran = "T" Then
            strSql += vbCrLf + " /** " & InsComments & "**/"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " CASE WHEN RECPAY = 'P' THEN 'ISS' ELSE 'REC' END SEP"
            strSql += vbCrLf + " ,I.TRANNO,0 PCS," & Weight & ""
            strSql += vbCrLf + " ,CA.CATNAME,ME.METALNAME AS METAL"
            strSql += vbCrLf + " ,CASE WHEN CA.GS11 = 'Y' THEN '" & Gs11Name & "' WHEN CA.GS12 = 'Y' THEN '" & Gs12Name & "' ELSE 'OTHER' END AS GS,I.TRANDATE"
            strSql += vbCrLf + " ,CASE WHEN LEN(I.TRANTYPE) = 3 THEN HE.ACNAME "
            strSql += vbCrLf + "       WHEN I.TRANTYPE = 'D' THEN 'DUE'"
            strSql += vbCrLf + "       WHEN I.TRANTYPE = 'T' THEN 'OTHER'"
            strSql += vbCrLf + "       WHEN I.TRANTYPE = 'A' THEN 'ADVANCE'"
            strSql += vbCrLf + "       ELSE I.TRANTYPE END"
            strSql += vbCrLf + "       AS ACNAME,CONVERT(VARCHAR(100),NULL) AS ITEMNAME"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING I"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD AS HE ON HE.ACCODE = I.ACCODE"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID"
            strSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND ISNULL(I.GRSWT,0) <> 0"
            strSql += vbCrLf + " AND NOT EXISTS(SELECT 1 FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE = I.TRANDATE AND BATCHNO = I.BATCHNO AND TRANTYPE = 'AD' AND GRSWT = I.GRSWT)"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkMetal <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetal & ")))"
            If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        Else 'O'
            strSql += vbCrLf + " /** " & InsComments & "**/"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " CASE WHEN RECPAY = 'P' THEN 'ISS' ELSE 'REC' END SEP"
            strSql += vbCrLf + " ,NULL TRANNO,0 PCS,SUM(" & Weight & ") AS " & Weight & ""
            strSql += vbCrLf + " ,CA.CATNAME,ME.METALNAME AS METAL"
            strSql += vbCrLf + " ,CASE WHEN CA.GS11 = 'Y' THEN '" & Gs11Name & "' WHEN CA.GS12 = 'Y' THEN '" & Gs12Name & "' ELSE 'OTHER' END AS GS"
            strSql += vbCrLf + " ,NULL TRANDATE,NULL AS ACNAME,NULL ITEMNAME"
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING I "
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID"
            strSql += vbCrLf + " WHERE I.TRANDATE >= '" & cnTranFromDate.Date.ToString("yyyy-MM-dd") & "' AND I.TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND ISNULL(I.GRSWT,0) <> 0"
            strSql += vbCrLf + " AND NOT EXISTS(SELECT 1 FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE = I.TRANDATE AND BATCHNO = I.BATCHNO AND TRANTYPE = 'AD' AND GRSWT = I.GRSWT)"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkMetal <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetal & ")))"
            If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            strSql += vbCrLf + " GROUP BY CA.CATNAME,CA.GS11,CA.GS12,ME.METALNAME,I.RECPAY"
        End If
        strSql += vbCrLf + " END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
    End Sub
    Private Sub InsGsOutStRecPay(ByVal Tran As String, Optional ByVal InsComments As String = "")
        If Not (rbtWeight.Checked Or rbtBoth.Checked) Then Exit Sub
        Dim bothWeight As String()
        bothWeight = Weight.Split(",")
        strSql = ""
        strSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..OUTSTANDING WHERE GRSWT <> 0"
        If chkCostName <> "" Then strSql += vbCrLf + "  AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        strSql += vbCrLf + " )>0"
        strSql += vbCrLf + " BEGIN"
        If Tran = "T" Then
            strSql += vbCrLf + " /** " & InsComments & "**/"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " SELECT SEP,TRANNO"
            'If chkSpecificFormat.Checked Then 
            strSql += vbCrLf + " ,TRANTYPE"
            strSql += vbCrLf + " ,PCS"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ," & bothWeight(0) & ""
                strSql += vbCrLf + " ," & bothWeight(1) & ""
            Else
                strSql += vbCrLf + " ," & Weight & ""
            End If
            strSql += vbCrLf + " ,0 AS STNPCS,0 AS STNWT,0 AS DIAPCS,0 AS DIAWT,0 AS PREPCS,0 AS PREWT"
            strSql += vbCrLf + " ,CATNAME,METAL"
            strSql += vbCrLf + " ,GS,TRANDATE,ACNAME,ITEMNAME" & IIf(chkSpecificFormat.Checked = False, ", REMARK,REFNO", "") & " FROM ( "
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " CASE WHEN RECPAY = 'P' THEN 'ISS' ELSE 'REC' END SEP"
            strSql += vbCrLf + " ,I.TRANNO"
            'If chkSpecificFormat.Checked Then

            strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE='A' THEN 'ORD ADVANCE' ELSE 'OTHERS' END TRANTYPE"
            strSql += vbCrLf + " ,0 PCS"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ,SUM(ISNULL(I." & bothWeight(0) & ",0))-ISNULL((SELECT SUM(" & bothWeight(0) & ") FROM " & cnStockDb & "..ISSUE WHERE BATCHNO =I.BATCHNO AND TRANTYPE='OD'),0) AS " & bothWeight(0) & ""
                strSql += vbCrLf + " ,SUM(ISNULL(I." & bothWeight(1) & ",0))-ISNULL((SELECT SUM(" & bothWeight(1) & ") FROM " & cnStockDb & "..ISSUE WHERE BATCHNO =I.BATCHNO AND TRANTYPE='OD'),0) AS " & bothWeight(1) & ""
            Else
                strSql += vbCrLf + " ,SUM(ISNULL(I." & Weight & ",0))-ISNULL((SELECT SUM(" & Weight & ") FROM " & cnStockDb & "..ISSUE WHERE BATCHNO =I.BATCHNO AND TRANTYPE='OD'),0) AS " & Weight & ""
            End If
            strSql += vbCrLf + " ,CA.CATNAME,ME.METALNAME AS METAL"
            strSql += vbCrLf + " ,CASE WHEN CA.GS11 = 'Y' THEN '" & Gs11Name & "' WHEN CA.GS12 = 'Y' THEN '" & Gs12Name & "' ELSE 'OTHER' END AS GS,I.TRANDATE"
            strSql += vbCrLf + " ,CASE WHEN LEN(I.TRANTYPE) = 3 THEN HE.ACNAME "
            strSql += vbCrLf + " WHEN I.TRANTYPE = 'T' THEN 'OTHER'"
            strSql += vbCrLf + " WHEN I.TRANTYPE = 'A' THEN 'ORDER RECPAY'"
            strSql += vbCrLf + " ELSE I.TRANTYPE END"
            strSql += vbCrLf + " AS ACNAME,CONVERT(VARCHAR(100),NULL) AS ITEMNAME" & IIf(chkSpecificFormat.Checked = False, ",CONVERT(VARCHAR(400),NULL) AS REMARK,CONVERT(VARCHAR(40),NULL) AS REFNO", "") & ""
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING I"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD AS HE ON HE.ACCODE = I.ACCODE"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID"
            strSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND SUBSTRING(I.RUNNO,6,20) LIKE 'O%' "
            strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND ISNULL(I.GRSWT,0) <> 0"
            strSql += vbCrLf + " AND NOT EXISTS(SELECT 1 FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE = I.TRANDATE AND BATCHNO = I.BATCHNO AND TRANTYPE = 'AD' AND GRSWT = I.GRSWT)"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkMetal <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetal & ")))"
            If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            strSql += vbCrLf + " GROUP BY CA.CATNAME,CA.GS11,CA.GS12,ME.METALNAME,I.RECPAY,I.TRANNO,I.TRANDATE,I.TRANTYPE,HE.ACNAME,I.BATCHNO,I.RUNNO"
            strSql += vbCrLf + " )X WHERE"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " (" & bothWeight(0) & ">0 OR " & bothWeight(1) & ">0 )"
            Else
                strSql += vbCrLf + " " & Weight & ">0"
            End If
        Else 'O'
            strSql += vbCrLf + " /** " & InsComments & "**/"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " SELECT SEP,TRANNO"
            strSql += vbCrLf + " ,TRANTYPE"
            strSql += vbCrLf + " ,PCS"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ," & bothWeight(0) & ""
                strSql += vbCrLf + " ," & bothWeight(1) & ""
            Else
                strSql += vbCrLf + " ," & Weight & ""
            End If
            strSql += vbCrLf + " ,0 AS STNPCS,0 AS STNWT,0 AS DIAPCS,0 AS DIAWT,0 AS PREPCS,0 AS PREWT"
            strSql += vbCrLf + " ,CATNAME,METAL"
            strSql += vbCrLf + " ,GS,TRANDATE,ACNAME,ITEMNAME" & IIf(chkSpecificFormat.Checked = False, ", REMARK,REFNO", "") & " FROM ( "
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " CASE WHEN RECPAY = 'P' THEN 'ISS' ELSE 'REC' END SEP"
            strSql += vbCrLf + " ,NULL TRANNO,'OP' TRANTYPE,0 PCS"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ,SUM(ISNULL(I." & bothWeight(0) & ",0))-ISNULL((SELECT SUM(" & bothWeight(0) & ") FROM " & cnStockDb & "..ISSUE WHERE BATCHNO =I.BATCHNO AND TRANTYPE='OD'),0) AS " & bothWeight(0) & ""
                strSql += vbCrLf + " ,SUM(ISNULL(I." & bothWeight(1) & ",0))-ISNULL((SELECT SUM(" & bothWeight(1) & ") FROM " & cnStockDb & "..ISSUE WHERE BATCHNO =I.BATCHNO AND TRANTYPE='OD'),0) AS " & bothWeight(1) & ""
            Else
                strSql += vbCrLf + " ,SUM(ISNULL(I." & Weight & ",0))-ISNULL((SELECT SUM(" & Weight & ") FROM " & cnStockDb & "..ISSUE WHERE BATCHNO =I.BATCHNO AND TRANTYPE='OD'),0) AS " & Weight & ""
            End If
            strSql += vbCrLf + " ,0 AS STNPCS,0 AS STNWT,0 AS DIAPCS,0 AS DIAWT,0 AS PREPCS,0 AS PREWT"
            strSql += vbCrLf + " ,CA.CATNAME,ME.METALNAME AS METAL"
            strSql += vbCrLf + " ,CASE WHEN CA.GS11 = 'Y' THEN '" & Gs11Name & "' WHEN CA.GS12 = 'Y' THEN '" & Gs12Name & "' ELSE 'OTHER' END AS GS"
            strSql += vbCrLf + " ,NULL TRANDATE,NULL AS ACNAME,NULL ITEMNAME" & IIf(chkSpecificFormat.Checked = False, ",CONVERT(VARCHAR(400),NULL) AS REMARK,CONVERT(VARCHAR(40),NULL) AS REFNO", "")
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING I "
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID"
            strSql += vbCrLf + " WHERE I.TRANDATE >= '" & cnTranFromDate.Date.ToString("yyyy-MM-dd") & "' AND I.TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND SUBSTRING(I.RUNNO,6,20) LIKE 'O%' "
            strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND ISNULL(I.GRSWT,0) <> 0"
            strSql += vbCrLf + " AND NOT EXISTS(SELECT 1 FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE = I.TRANDATE AND BATCHNO = I.BATCHNO AND TRANTYPE = 'AD' AND GRSWT = I.GRSWT)"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkMetal <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetal & ")))"
            If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            strSql += vbCrLf + " GROUP BY CA.CATNAME,CA.GS11,CA.GS12,ME.METALNAME,I.RECPAY,I.BATCHNO"
            strSql += vbCrLf + " )X WHERE"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " (" & bothWeight(0) & ">0 OR " & bothWeight(1) & ">0 )"
            Else
                strSql += vbCrLf + " " & Weight & ">0"
            End If
        End If
        strSql += vbCrLf + " END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
    End Sub
    Private Sub InsGsOutStRepairRecPay(ByVal Tran As String, Optional ByVal InsComments As String = "")
        If Not (rbtWeight.Checked Or rbtBoth.Checked) Then Exit Sub
        Dim bothWeight As String()
        bothWeight = Weight.Split(",")
        strSql = ""
        strSql = " IF (SELECT COUNT(*) FROM " & cnAdminDb & "..OUTSTANDING WHERE GRSWT <> 0"
        If chkCostName <> "" Then strSql += vbCrLf + "  AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        strSql += vbCrLf + " )>0"
        strSql += vbCrLf + " BEGIN"
        If Tran = "T" Then
            strSql += vbCrLf + " /** " & InsComments & "**/"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " SELECT SEP,TRANNO"
            'If chkSpecificFormat.Checked Then strSql += vbCrLf + " ,TRANTYPE"
            strSql += vbCrLf + " ,TRANTYPE"
            strSql += vbCrLf + " ,PCS"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ," & bothWeight(0) & ""
                strSql += vbCrLf + " ," & bothWeight(1) & ""
            Else
                strSql += vbCrLf + " ," & Weight & ""
            End If
            strSql += vbCrLf + " ,0 AS STNPCS,0 AS STNWT,0 AS DIAPCS,0 AS DIAWT,0 AS PREPCS,0 AS PREWT"
            strSql += vbCrLf + " ,CATNAME,METAL"
            strSql += vbCrLf + " ,GS,TRANDATE,ACNAME,ITEMNAME" & IIf(chkSpecificFormat.Checked = False, ", REMARK,REFNO", "") & " FROM ( "
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " CASE WHEN RECPAY = 'P' THEN 'ISS' ELSE 'REC' END SEP"
            strSql += vbCrLf + " ,I.TRANNO"
            strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE='A' THEN 'REP ADVANCE' ELSE 'OTHERS' END TRANTYPE"
            strSql += vbCrLf + " ,0 PCS"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ,SUM(ISNULL(I." & bothWeight(0) & ",0)) AS " & bothWeight(0) & ""
                strSql += vbCrLf + " ,SUM(ISNULL(I." & bothWeight(1) & ",0)) AS " & bothWeight(1) & ""
            Else
                strSql += vbCrLf + " ,SUM(ISNULL(I." & Weight & ",0)) AS " & Weight & ""
            End If
            strSql += vbCrLf + " ,CA.CATNAME,ME.METALNAME AS METAL"
            strSql += vbCrLf + " ,CASE WHEN CA.GS11 = 'Y' THEN '" & Gs11Name & "' WHEN CA.GS12 = 'Y' THEN '" & Gs12Name & "' ELSE 'OTHER' END AS GS,I.TRANDATE"
            strSql += vbCrLf + " ,CASE WHEN LEN(I.TRANTYPE) = 3 THEN HE.ACNAME "
            strSql += vbCrLf + " WHEN I.TRANTYPE = 'T' THEN 'OTHER'"
            strSql += vbCrLf + " WHEN I.TRANTYPE = 'A' THEN 'REPAIR REC'"
            strSql += vbCrLf + " ELSE I.TRANTYPE END"
            strSql += vbCrLf + " AS ACNAME,CONVERT(VARCHAR(100),NULL) AS ITEMNAME" & IIf(chkSpecificFormat.Checked = False, ",CONVERT(VARCHAR(400),NULL) AS REMARK,CONVERT(VARCHAR(40),NULL) AS REFNO", "") & ""
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING I"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD AS HE ON HE.ACCODE = I.ACCODE"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID"
            strSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND SUBSTRING(I.RUNNO,6,20) LIKE 'R%' AND RECPAY='R'"
            strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND ISNULL(I.GRSWT,0) <> 0"
            strSql += vbCrLf + " AND NOT EXISTS(SELECT 1 FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE = I.TRANDATE AND BATCHNO = I.BATCHNO AND TRANTYPE = 'AD' AND GRSWT = I.GRSWT)"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkMetal <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetal & ")))"
            If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            strSql += vbCrLf + " GROUP BY CA.CATNAME,CA.GS11,CA.GS12,ME.METALNAME,I.RECPAY,I.TRANNO,I.TRANDATE,I.TRANTYPE,HE.ACNAME,I.BATCHNO,I.RUNNO"
            strSql += vbCrLf + " )X WHERE"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " (" & bothWeight(0) & ">0 OR " & bothWeight(1) & ">0 )"
            Else
                strSql += vbCrLf + " " & Weight & ">0"
            End If
        Else 'O'
            strSql += vbCrLf + " /** " & InsComments & "**/"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " CASE WHEN RECPAY = 'P' THEN 'ISS' ELSE 'REC' END SEP"
            strSql += vbCrLf + " ,NULL TRANNO,'OP' TRANTYPE,0 PCS"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ,SUM(I." & bothWeight(0) & ") AS" & bothWeight(0) & ""
                strSql += vbCrLf + " ,SUM(I." & bothWeight(1) & ") AS" & bothWeight(1) & ""
            Else
                strSql += vbCrLf + " ,SUM(I." & Weight & ") AS " & Weight & ""
            End If
            strSql += vbCrLf + " ,0 AS STNPCS,0 AS STNWT,0 AS DIAPCS,0 AS DIAWT,0 AS PREPCS,0 AS PREWT"
            strSql += vbCrLf + " ,CA.CATNAME,ME.METALNAME AS METAL"
            strSql += vbCrLf + " ,CASE WHEN CA.GS11 = 'Y' THEN '" & Gs11Name & "' WHEN CA.GS12 = 'Y' THEN '" & Gs12Name & "' ELSE 'OTHER' END AS GS"
            strSql += vbCrLf + " ,NULL TRANDATE,NULL AS ACNAME,NULL ITEMNAME" & IIf(chkSpecificFormat.Checked = False, ",CONVERT(VARCHAR(400),NULL) AS REMARK,CONVERT(VARCHAR(40),NULL) AS REFNO", "") & ""
            strSql += vbCrLf + " FROM " & cnAdminDb & "..OUTSTANDING I "
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID"
            strSql += vbCrLf + " WHERE I.TRANDATE >= '" & cnTranFromDate.Date.ToString("yyyy-MM-dd") & "' AND I.TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND SUBSTRING(I.RUNNO,6,20) LIKE 'R%' AND RECPAY='R'"
            strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            strSql += vbCrLf + " AND ISNULL(I.GRSWT,0) <> 0"
            strSql += vbCrLf + " AND NOT EXISTS(SELECT 1 FROM " & cnStockDb & "..RECEIPT WHERE TRANDATE = I.TRANDATE AND BATCHNO = I.BATCHNO AND TRANTYPE = 'AD' AND GRSWT = I.GRSWT)"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkMetal <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetal & ")))"
            If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            strSql += vbCrLf + " GROUP BY CA.CATNAME,CA.GS11,CA.GS12,ME.METALNAME,I.RECPAY"
        End If
        strSql += vbCrLf + " END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
    End Sub
    Private Sub InsGsIssRec(ByVal Tran As String, ByVal RecIss As String, ByVal OrderInfo As Boolean, Optional ByVal InsComments As String = "")
        Dim bothWeight As String()
        bothWeight = Weight.Split(",")
        If Tran = "T" Then
            strSql = " /** " & InsComments & "**/"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '" & IIf(RecIss = "I", "ISS", "REC") & "' SEP"
            If rbtTranno.Checked Then
                strSql += vbCrLf + "  	,TRANNO"
            Else
                strSql += vbCrLf + "  	,NULL TRANNO"
            End If
            strSql += ",I.TRANTYPE"
            strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.PCS ELSE 0 END AS PCS"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & bothWeight(0) & " ELSE 0 END " & bothWeight(0) & ""
                strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & bothWeight(1) & " ELSE 0 END " & bothWeight(1) & ""
            Else
                strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & Weight & " ELSE 0 END " & Weight & ""
            End If
            strSql += vbCrLf + " ,CASE WHEN (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.STNPCS ELSE 0 END AS STNPCS"
            strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I.STNWT ELSE 0 END AS STNWT"
            strSql += vbCrLf + " ,CASE WHEN (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.DIAPCS ELSE 0 END AS DIAPCS"
            strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I.DIAWT ELSE 0 END AS DIAWT"
            strSql += vbCrLf + " ,CASE WHEN (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.PREPCS ELSE 0 END AS PREPCS"
            strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I.PREWT ELSE 0 END AS PREWT"
            strSql += vbCrLf + " ,CA.CATNAME,ME.METALNAME AS METAL"
            strSql += vbCrLf + " ,CASE WHEN CA.GS11 = 'Y' THEN '" & Gs11Name & "' WHEN CA.GS12 = 'Y' THEN '" & Gs12Name & "' ELSE 'OTHER' END AS GS"
            strSql += vbCrLf + " ,I.TRANDATE"
            strSql += vbCrLf + " ,CASE WHEN LEN(I.TRANTYPE) = 3 THEN "
            If ChkWithTrantype.Checked Then
                strSql += vbCrLf + " (CASE WHEN I.TRANTYPE = 'RRE' THEN 'RECEIPT'"
                strSql += vbCrLf + "       WHEN I.TRANTYPE = 'RPU' THEN 'PURCHASE'"
                strSql += vbCrLf + "       WHEN I.TRANTYPE = 'RIN' THEN 'INTERNAL TRANSFER'"
                strSql += vbCrLf + "       WHEN I.TRANTYPE = 'RAP' THEN 'APPROVAL RECEIPT'"
                strSql += vbCrLf + "       WHEN I.TRANTYPE = 'ROT' THEN 'OTHER RECEIPT'"
                strSql += vbCrLf + "       WHEN I.TRANTYPE = 'RPA' THEN 'PACKING'"
                strSql += vbCrLf + "       WHEN I.TRANTYPE = 'IIS' THEN 'ISSUE'"
                strSql += vbCrLf + "       WHEN I.TRANTYPE = 'IPU' THEN 'PURCHASE RETURN'"
                strSql += vbCrLf + "       WHEN I.TRANTYPE = 'IIN' THEN 'INTERNAL TRANSFER'"
                strSql += vbCrLf + "       WHEN I.TRANTYPE = 'IAP' THEN 'APPROVAL ISSUE'"
                strSql += vbCrLf + "       WHEN I.TRANTYPE = 'IOT' THEN 'OTHER ISSUE'"
                strSql += vbCrLf + "       WHEN I.TRANTYPE = 'IPA' THEN 'PACKING'"
                strSql += vbCrLf + "       ELSE I.TRANTYPE END)+'-'+ "
                strSql += vbCrLf + "       HE.ACNAME + "
                strSql += vbCrLf + " (CASE WHEN HE.ACTYPE = 'D' THEN '-DEALER'"
                strSql += vbCrLf + "       WHEN HE.ACTYPE = 'G' THEN '-SMITH'"
                strSql += vbCrLf + "       ELSE '' END) "
            Else
                strSql += vbCrLf + "       HE.ACNAME "
            End If

            strSql += vbCrLf + "       WHEN I.TRANTYPE = 'SA' THEN 'SALES'"
            'strSql += vbCrLf + "     WHEN I.TRANTYPE = 'MI' THEN 'MISC ISS'"
            strSql += vbCrLf + "       WHEN I.TRANTYPE='MI' THEN HE.ACNAME"
            strSql += vbCrLf + "       WHEN I.TRANTYPE='IOT' THEN HE.ACNAME"
            strSql += vbCrLf + "       WHEN I.TRANTYPE='ROT' THEN HE.ACNAME"
            strSql += vbCrLf + "       WHEN I.TRANTYPE='RIS' THEN HE.ACNAME"
            strSql += vbCrLf + "       WHEN I.TRANTYPE = 'AI' THEN 'APPROVAL'"
            strSql += vbCrLf + "       WHEN I.TRANTYPE = 'OD' THEN 'ORDER DEL'"
            strSql += vbCrLf + "       WHEN I.TRANTYPE = 'RD' THEN 'REPAIR DEL'"
            strSql += vbCrLf + "       WHEN I.TRANTYPE = 'PU' THEN 'PURCHASE'"
            strSql += vbCrLf + "       WHEN I.TRANTYPE = 'AR' THEN 'APPROVAL'"
            strSql += vbCrLf + "       ELSE I.TRANTYPE END AS ACNAME"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(100),IM.ITEMNAME) AS ITEMNAME"
            strSql += vbCrLf + "" & IIf(chkSpecificFormat.Checked = False, ",CONVERT(VARCHAR(400),ISNULL(I.REMARK1,'')+ CASE WHEN ISNULL(I.REMARK2,'') <>'' THEN '-'+ISNULL(I.REMARK2,'') END) AS REMARK", "")
            strSql += vbCrLf + "" & IIf(chkSpecificFormat.Checked = False, ",I.REFNO AS REFNO", "")
            strSql += vbCrLf + " from TEMPTABLEDB..TEMP" & systemId & IIf(RecIss = "I", "ISS", "REC") & " I "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD AS HE ON HE.ACCODE = I.ACCODE"
            strSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            If OrderInfo = False Then
                If ChkRepairDet.Checked Then
                    If RecIss = "I" Then strSql += vbCrLf + " AND I.BATCHNO NOT IN (SELECT BATCHNO from TEMPTABLEDB..TEMP" & systemId & "ORDREP_DEL WHERE TRANTYPE<>'RD') AND I.TRANTYPE <> 'RD'"
                Else
                    If RecIss = "I" Then strSql += vbCrLf + " AND I.BATCHNO NOT IN (SELECT BATCHNO from TEMPTABLEDB..TEMP" & systemId & "ORDREP_DEL)"
                End If
            Else
                strSql += vbCrLf + "    AND I.BATCHNO IN (SELECT BATCHNO from TEMPTABLEDB..TEMP" & systemId & "ORDREP_DEL) AND I.TRANTYPE NOT IN ('SA','PU','SR')  /*AND I.GRSWT > 0*/"
                If RecIss = "I" Then strSql += vbCrLf + " AND I.BATCHNO NOT IN (SELECT BATCHNO from TEMPTABLEDB..TEMP" & systemId & "REC WHERE TRANTYPE = 'PU') "
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        Else 'O'
            strSql = " /** " & InsComments & "**/"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPDB..#OPETRAN" & IIf(RecIss = "I", "ISS", "REC") & "','U') IS NOT NULL DROP TABLE #OPETRAN" & IIf(RecIss = "I", "ISS", "REC") & ""
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            strSql = vbCrLf + " SELECT 'OP' TRANTYPE,"
            strSql += vbCrLf + " CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.PCS ELSE 0 END AS PCS"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & bothWeight(0) & " ELSE 0 END " & bothWeight(0) & ""
                strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & bothWeight(1) & " ELSE 0 END " & bothWeight(1) & ""
            Else
                strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & Weight & " ELSE 0 END " & Weight & ""
            End If
            strSql += vbCrLf + " ,CASE WHEN (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.STNPCS ELSE 0 END AS STNPCS"
            strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I.STNWT ELSE 0 END AS STNWT"
            strSql += vbCrLf + " ,CASE WHEN (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.DIAPCS ELSE 0 END AS DIAPCS"
            strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I.DIAWT ELSE 0 END AS DIAWT"
            strSql += vbCrLf + " ,CASE WHEN (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.PREPCS ELSE 0 END AS PREPCS"
            strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I.PREWT ELSE 0 END AS PREWT"
            strSql += vbCrLf + " ,CA.CATNAME,ME.METALNAME AS METAL"
            strSql += vbCrLf + " ,CASE WHEN CA.GS11 = 'Y' THEN '" & Gs11Name & "' WHEN CA.GS12 = 'Y' THEN '" & Gs12Name & "' ELSE 'OTHER' END AS GS"
            strSql += vbCrLf + " INTO #OPETRAN" & IIf(RecIss = "I", "ISS", "REC") & ""

            strSql += vbCrLf + " from TEMPTABLEDB..TEMP" & systemId & IIf(RecIss = "I", "ISS", "REC") & " I "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID"
            strSql += vbCrLf + " WHERE I.TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            If OrderInfo = False Then
                ''strSql += vbCrLf + " AND I.BATCHNO NOT IN (SELECT BATCHNO from TEMPTABLEDB..TEMP" & systemId & "ORDREP_DEL)"
                If ChkRepairDet.Checked Then
                    If RecIss = "I" Then strSql += vbCrLf + " AND I.BATCHNO NOT IN (SELECT BATCHNO from TEMPTABLEDB..TEMP" & systemId & "ORDREP_DEL WHERE TRANTYPE<>'RD') AND I.TRANTYPE <> 'RD'"
                Else
                    If RecIss = "I" Then strSql += vbCrLf + " AND I.BATCHNO NOT IN (SELECT BATCHNO from TEMPTABLEDB..TEMP" & systemId & "ORDREP_DEL)"
                End If
            Else
                strSql += vbCrLf + "    AND I.BATCHNO IN (SELECT BATCHNO from TEMPTABLEDB..TEMP" & systemId & "ORDREP_DEL) AND I.TRANTYPE NOT IN ('SA','PU','SR')   /*AND I.GRSWT > 0*/"
            End If
            strSql += vbCrLf + ""
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '" & IIf(RecIss = "I", "ISS", "REC") & "' SEP"
            strSql += vbCrLf + " ,NULL TRANNO,'OP' AS TRANTYPE"
            strSql += vbCrLf + " ,SUM(PCS) AS PCS"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ,SUM(" & bothWeight(0) & ") " & bothWeight(0) & ""
                strSql += vbCrLf + " ,SUM(" & bothWeight(1) & ") " & bothWeight(1) & ""
            Else
                strSql += vbCrLf + " ,SUM(" & Weight & ") " & Weight & ""
            End If
            strSql += vbCrLf + " ,SUM(STNPCS) AS STNPCS"
            strSql += vbCrLf + " ,SUM(STNWT) AS STNWT"
            strSql += vbCrLf + " ,SUM(DIAPCS) AS DIAPCS"
            strSql += vbCrLf + " ,SUM(DIAWT)  AS DIAWT"
            strSql += vbCrLf + " ,SUM(PREPCS) AS PREPCS"
            strSql += vbCrLf + " ,SUM(PREWT) AS PREWT"
            strSql += vbCrLf + " ,CATNAME,METAL"
            strSql += vbCrLf + " ,GS"
            strSql += vbCrLf + " ,NULL TRANDATE,NULL AS ACNAME,NULL ITEMNAME" & IIf(chkSpecificFormat.Checked = False, ",CONVERT(VARCHAR(400),NULL) AS REMARK,CONVERT(VARCHAR(40),NULL) AS REFNO", "")
            strSql += vbCrLf + " FROM #OPETRAN" & IIf(RecIss = "I", "ISS", "REC") & " AS I"
            strSql += vbCrLf + " GROUP BY CATNAME,GS,METAL"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        End If
    End Sub

    Private Sub InsGsStone(ByVal Tran As String, ByVal RecIss As String, Optional ByVal InsComments As String = "")
        Dim bothWeight As String()
        bothWeight = Weight.Split(",")
        If Tran = "T" Then
            strSql = " /** " & InsComments & "**/"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '" & IIf(RecIss = "I", "ISS", "REC") & "' SEP,S.TRANNO"
            strSql += ",I.TRANTYPE"
            strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('P','B')) THEN S.STNPCS ELSE 0 END PCS"
            If rdbBothwt.Checked Then
                If chkIntoCt.Checked Then
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN case when S.STONEUNIT = 'G' THEN S.STNWT*5 ELSE S.STNWT END ELSE 0 END " & bothWeight(0) & ""
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN case when S.STONEUNIT = 'G' THEN S.STNWT*5 ELSE S.STNWT END ELSE 0 END " & bothWeight(1) & ""
                ElseIf chkIntoGm.Checked Then
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN case when S.STONEUNIT = 'C' THEN S.STNWT/5 ELSE S.STNWT END ELSE 0 END " & bothWeight(0) & ""
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN case when S.STONEUNIT = 'C' THEN S.STNWT/5 ELSE S.STNWT END ELSE 0 END " & bothWeight(1) & ""
                Else
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END " & bothWeight(0) & ""
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END " & bothWeight(1) & ""
                End If
            Else
                If chkIntoCt.Checked Then
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN case when S.STONEUNIT = 'G' THEN S.STNWT*5 ELSE S.STNWT END ELSE 0 END " & Weight & ""
                ElseIf chkIntoGm.Checked Then
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN case when S.STONEUNIT = 'C' THEN S.STNWT/5 ELSE S.STNWT END ELSE 0 END " & Weight & ""
                Else
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END " & Weight & ""
                End If
            End If
            strSql += vbCrLf + " ,0 AS STNPCS"
            strSql += vbCrLf + " ,0 AS STNWT"
            strSql += vbCrLf + " ,0 AS DIAPCS"
            strSql += vbCrLf + " ,0 AS DIAWT"
            strSql += vbCrLf + " ,0 AS PREPCS"
            strSql += vbCrLf + " ,0 AS PREWT"
            strSql += vbCrLf + " ,CA.CATNAME AS CATNAME,ME.METALNAME AS METAL"
            strSql += vbCrLf + " ,CASE WHEN CA.GS11 = 'Y' THEN '" & Gs11Name & "' WHEN CA.GS12 = 'Y' THEN '" & Gs12Name & "' ELSE 'OTHER' END AS GS"
            strSql += vbCrLf + " ,S.TRANDATE"
            strSql += vbCrLf + " ,CASE WHEN LEN(S.TRANTYPE) = 3 THEN "
            If ChkWithTrantype.Checked Then
                strSql += vbCrLf + " (CASE WHEN I.TRANTYPE = 'RRE' THEN 'RECEIPT'"
                strSql += vbCrLf + "       WHEN I.TRANTYPE = 'RPU' THEN 'PURCHASE'"
                strSql += vbCrLf + "       WHEN I.TRANTYPE = 'RIN' THEN 'INTERNAL TRANSFER'"
                strSql += vbCrLf + "       WHEN I.TRANTYPE = 'RAP' THEN 'APPROVAL RECEIPT'"
                strSql += vbCrLf + "       WHEN I.TRANTYPE = 'ROT' THEN 'OTHER RECEIPT'"
                strSql += vbCrLf + "       WHEN I.TRANTYPE = 'RPA' THEN 'PACKING'"
                strSql += vbCrLf + "       WHEN I.TRANTYPE = 'IIS' THEN 'ISSUE'"
                strSql += vbCrLf + "       WHEN I.TRANTYPE = 'IPU' THEN 'PURCHASE RETURN'"
                strSql += vbCrLf + "       WHEN I.TRANTYPE = 'IIN' THEN 'INTERNAL TRANSFER'"
                strSql += vbCrLf + "       WHEN I.TRANTYPE = 'IAP' THEN 'APPROVAL ISSUE'"
                strSql += vbCrLf + "       WHEN I.TRANTYPE = 'IOT' THEN 'OTHER ISSUE'"
                strSql += vbCrLf + "       WHEN I.TRANTYPE = 'IPA' THEN 'PACKING'"
                strSql += vbCrLf + "       ELSE I.TRANTYPE END)+'-'+ "
                strSql += vbCrLf + "       HE.ACNAME +"
                strSql += vbCrLf + " (CASE WHEN HE.ACTYPE = 'D' THEN '-DEALER'"
                strSql += vbCrLf + "       WHEN HE.ACTYPE = 'G' THEN '-SMITH'"
                strSql += vbCrLf + "       ELSE '' END) "
            Else
                strSql += vbCrLf + "       HE.ACNAME "
            End If
            strSql += vbCrLf + "       WHEN S.TRANTYPE = 'SA' THEN 'SALES'"
            'strSql += vbCrLf + "       WHEN S.TRANTYPE = 'MI' THEN 'MISC ISS'"
            strSql += vbCrLf + "       WHEN I.TRANTYPE='MI' THEN HE.ACNAME"
            strSql += vbCrLf + "       WHEN I.TRANTYPE='IOT' THEN HE.ACNAME"
            strSql += vbCrLf + "       WHEN I.TRANTYPE='ROT' THEN HE.ACNAME"
            strSql += vbCrLf + "       WHEN I.TRANTYPE='RIS' THEN HE.ACNAME"
            strSql += vbCrLf + "       WHEN S.TRANTYPE = 'AI' THEN 'APPROVAL'"
            strSql += vbCrLf + "       WHEN S.TRANTYPE = 'OD' THEN 'ORDER DEL'"
            strSql += vbCrLf + "       WHEN S.TRANTYPE = 'RD' THEN 'REPAIR DEL'"
            strSql += vbCrLf + "       WHEN S.TRANTYPE = 'PU' THEN 'PURCHASE'"
            strSql += vbCrLf + "       WHEN S.TRANTYPE = 'AR' THEN 'APPROVAL'"
            strSql += vbCrLf + "       ELSE S.TRANTYPE END AS ACNAME"

            strSql += vbCrLf + " ,CONVERT(VARCHAR(100),IM.ITEMNAME) AS ITEMNAME"
            strSql += vbCrLf + "" & IIf(chkSpecificFormat.Checked = False, ",CASE WHEN I.TRANTYPE IN ('IIN','RIN') AND I.FLAG IN ('T','X') THEN CONVERT(VARCHAR(400),ISNULL(I.REMARK1,'')+ CASE WHEN ISNULL(I.REMARK2,'') <>'' THEN '-'+ISNULL(I.REMARK2,'') END) ELSE '' END AS REMARK", "")
            strSql += vbCrLf + "" & IIf(chkSpecificFormat.Checked = False, ",CASE WHEN I.TRANTYPE IN ('IIN','RIN') AND I.FLAG IN ('T','X') THEN ISNULL(I.REFNO,'') ELSE '' END AS REFNO", "")
            strSql += vbCrLf + " FROM " & cnStockDb & ".." & IIf(RecIss = "I", "ISSSTONE", "RECEIPTSTONE") & " S "
            'strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = S.STNITEMID AND SM.SUBITEMID = S.STNSUBITEMID"
            'strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "")
            'strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = IM.METALID " & IIf(chkMetal <> "", " AND ME.METALNAME IN (" & chkMetal & ")", "")
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = S.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "")
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID " & IIf(chkMetal <> "", " AND ME.METALNAME IN (" & chkMetal & ")", "")
            strSql += vbCrLf + " INNER JOIN " & cnStockDb & ".." & IIf(RecIss = "I", "ISSUE", "RECEIPT") & " AS I ON I.TRANDATE = S.TRANDATE AND I.BATCHNO = S.BATCHNO AND I.SNO = S.ISSSNO AND ISNULL(I.CANCEL,'') = ''"
            'strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IIM ON IIM.ITEMID = I.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IIM ON IIM.ITEMID = I.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS ISM ON ISM.ITEMID = I.ITEMID AND ISM.SUBITEMID = I.SUBITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD AS HE ON HE.ACCODE = I.ACCODE"
            strSql += vbCrLf + " WHERE S.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND S.COMPANYID = '" & strCompanyId & "'"
            If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
                strSql += vbCrLf + " AND ISNULL(S.STNITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
            End If
            If TranType <> "" Then strSql += vbCrLf + " AND S.TRANTYPE IN (" & TranType & ")"
            If Apptrantype <> "" Then strSql += vbCrLf + " AND S.TRANTYPE NOT IN (" & Apptrantype & ")"
            If Misctrantype <> "" Then strSql += vbCrLf + " AND S.TRANTYPE NOT IN (" & Misctrantype & ")"
            If chkCostName <> "" Then strSql += vbCrLf + " AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If ChkRepairDet.Checked = False Then strSql += vbCrLf + " AND S.BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & ".." & IIf(RecIss = "I", "ISSSTONE", "RECEIPTSTONE") & " WHERE TRANTYPE = 'RD')"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        Else 'O'
            strSql = " /** " & InsComments & "**/"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPDB..#OPESTUD" & IIf(RecIss = "I", "ISS", "REC") & "') IS NOT NULL DROP TABLE #OPESTUD" & IIf(RecIss = "I", "ISS", "REC") & ""
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            strSql = vbCrLf + " SELECT 'OP' TRANTYPE,"
            strSql += vbCrLf + " CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('P','B')) THEN S.STNPCS ELSE 0 END PCS"

            If rdbBothwt.Checked Then
                If chkIntoCt.Checked Then
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN case when S.STONEUNIT = 'G' THEN S.STNWT*5 ELSE S.STNWT END ELSE 0 END " & bothWeight(0) & ""
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN case when S.STONEUNIT = 'G' THEN S.STNWT*5 ELSE S.STNWT END ELSE 0 END " & bothWeight(1) & ""
                ElseIf chkIntoGm.Checked Then
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN case when S.STONEUNIT = 'C' THEN S.STNWT/5 ELSE S.STNWT END ELSE 0 END " & bothWeight(0) & ""
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN case when S.STONEUNIT = 'C' THEN S.STNWT/5 ELSE S.STNWT END ELSE 0 END " & bothWeight(1) & ""
                Else
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END " & bothWeight(0) & ""
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END " & bothWeight(1) & ""
                End If
            Else
                If chkIntoCt.Checked Then
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN case when S.STONEUNIT = 'G' THEN S.STNWT*5 ELSE S.STNWT END ELSE 0 END " & Weight & ""
                ElseIf chkIntoGm.Checked Then
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN case when S.STONEUNIT = 'C' THEN S.STNWT/5 ELSE S.STNWT END ELSE 0 END " & Weight & ""
                Else
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END " & Weight & ""
                End If
                '    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END " & Weight & ""
            End If
            strSql += vbCrLf + " ,0 AS STNPCS"
            strSql += vbCrLf + " ,0 AS STNWT"
            strSql += vbCrLf + " ,0 AS DIAPCS"
            strSql += vbCrLf + " ,0 AS DIAWT"
            strSql += vbCrLf + " ,0 AS PREPCS"
            strSql += vbCrLf + " ,0 AS PREWT"
            strSql += vbCrLf + " ,CA.CATNAME AS CATNAME,ME.METALNAME AS METAL"
            strSql += vbCrLf + " ,CASE WHEN CA.GS11 = 'Y' THEN '" & Gs11Name & "' WHEN CA.GS12 = 'Y' THEN '" & Gs12Name & "' ELSE 'OTHER' END AS GS"
            strSql += vbCrLf + " INTO #OPESTUD" & IIf(RecIss = "I", "ISS", "REC") & ""
            strSql += vbCrLf + " FROM " & cnStockDb & ".." & IIf(RecIss = "I", "ISSSTONE", "RECEIPTSTONE") & " S "
            'strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = S.STNITEMID AND SM.SUBITEMID = S.STNSUBITEMID"
            'strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "")
            'strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = IM.METALID " & IIf(chkMetal <> "", " AND ME.METALNAME IN (" & chkMetal & ")", "")
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = S.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "")
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID " & IIf(chkMetal <> "", " AND ME.METALNAME IN (" & chkMetal & ")", "")
            strSql += vbCrLf + " INNER JOIN " & cnStockDb & ".." & IIf(RecIss = "I", "ISSUE", "RECEIPT") & " AS I ON I.TRANDATE = S.TRANDATE AND I.BATCHNO = S.BATCHNO AND I.SNO = S.ISSSNO AND ISNULL(I.CANCEL,'') = ''"
            'strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IIM ON IIM.ITEMID = I.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IIM ON IIM.ITEMID = I.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS ISM ON ISM.ITEMID = I.ITEMID AND ISM.SUBITEMID = I.SUBITEMID"
            strSql += vbCrLf + " WHERE S.TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND S.COMPANYID = '" & strCompanyId & "'"
            If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
                strSql += vbCrLf + " AND ISNULL(S.STNITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
            End If
            If TranType <> "" Then strSql += vbCrLf + " AND S.TRANTYPE IN (" & TranType & ")"
            If Apptrantype <> "" Then strSql += vbCrLf + " AND S.TRANTYPE NOT IN (" & Apptrantype & ")"
            If Misctrantype <> "" Then strSql += vbCrLf + " AND S.TRANTYPE NOT IN (" & Misctrantype & ")"
            If ChkRepairDet.Checked = False Then strSql += vbCrLf + " AND S.BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & ".." & IIf(RecIss = "I", "ISSSTONE", "RECEIPTSTONE") & " WHERE TRANTYPE = 'RD')"
            If chkCostName <> "" Then strSql += vbCrLf + " AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            strSql += vbCrLf + ""
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '" & IIf(RecIss = "I", "ISS", "REC") & "' SEP"
            strSql += vbCrLf + " ,NULL TRANNO,'OP' TRANTYPE"
            strSql += vbCrLf + " ,SUM(PCS)PCS"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ,SUM(" & bothWeight(0) & ")" & bothWeight(0) & ""
                strSql += vbCrLf + " ,SUM(" & bothWeight(1) & ")" & bothWeight(1) & ""
            Else
                strSql += vbCrLf + " ,SUM(" & Weight & ")" & Weight & ""
            End If
            strSql += vbCrLf + " ,SUM(STNPCS) AS STNPCS"
            strSql += vbCrLf + " ,SUM(STNWT) AS STNWT"
            strSql += vbCrLf + " ,SUM(DIAPCS) AS DIAPCS"
            strSql += vbCrLf + " ,SUM(DIAWT)  AS DIAWT"
            strSql += vbCrLf + " ,SUM(PREPCS) AS PREPCS"
            strSql += vbCrLf + " ,SUM(PREWT) AS PREWT"

            strSql += vbCrLf + " ,CATNAME,METAL"
            strSql += vbCrLf + " ,GS"
            strSql += vbCrLf + " ,NULL TRANDATE,CONVERT(VARCHAR(100),NULL) AS ACNAME,CONVERT(VARCHAR(100),NULL) AS ITEMNAME"
            strSql += vbCrLf + "" & IIf(chkSpecificFormat.Checked = False, ",CONVERT(VARCHAR(400),NULL) AS REMARK,CONVERT(VARCHAR(40),NULL) AS REFNO", "")
            strSql += vbCrLf + " FROM #OPESTUD" & IIf(RecIss = "I", "ISS", "REC") & ""
            strSql += vbCrLf + " GROUP BY CATNAME,METAL,GS"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        End If
    End Sub

    Private Sub InsGsIssRecPrint(ByVal Tran As String, ByVal RecIss As String, ByVal OrderInfo As Boolean, Optional ByVal InsComments As String = "")
        Dim bothWeight As String()
        bothWeight = Weight.Split(",")

        If Tran = "T" Then
            strSql = " /** " & InsComments & "**/"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '" & IIf(RecIss = "I", "ISS", "REC") & "' SEP"
            If rbtTranno.Checked Then
                If Format2 = True Then
                    strSql += vbCrLf + "  ,TRANNO"
                Else
                    If RecIss = "R" Then
                        strSql += vbCrLf + " ,CASE WHEN ISNULL(REFNO,'') = ''  THEN CONVERT(VARCHAR(20),TRANNO) ELSE REFNO END AS TRANNO "
                    Else
                        strSql += vbCrLf + " ,CASE WHEN ISNULL(REFNO,'') = ''  THEN CONVERT(VARCHAR(20),TRANNO) ELSE REFNO END AS TRANNO "
                    End If
                End If
            Else
                strSql += vbCrLf + "  ,NULL TRANNO"
            End If
            ' strSql += vbCrLf + " ,NULL TRANTYPE"
            If RecIss = "I" Then
                strSql += vbCrLf + " ,CASE  WHEN I.TRANTYPE = 'SA' THEN  'SALES'"
                strSql += vbCrLf + " WHEN I.TRANTYPE LIKE '%IS' THEN 'ISSUE'"
                strSql += vbCrLf + " WHEN I.TRANTYPE = 'MI' THEN 'MISC ISSUE'"
                strSql += vbCrLf + " WHEN I.TRANTYPE = 'AD' THEN 'ADV REPAY'"
                strSql += vbCrLf + " Else i.trantype end "
                strSql += vbCrLf + " as TRANTYPE"
            Else
                strSql += vbCrLf + " ,CASE  WHEN I.TRANTYPE LIKE 'PU' THEN  'PURCHASE'"
                strSql += vbCrLf + " WHEN I.TRANTYPE LIKE 'RPU' THEN 'DIR PURCH'"
                strSql += vbCrLf + " WHEN I.TRANTYPE LIKE '%RE' THEN 'RECEIPT'"
                strSql += vbCrLf + " WHEN I.TRANTYPE = 'AD' THEN 'ADV RECEIPT'"
                strSql += vbCrLf + " else i.trantype end "
                strSql += vbCrLf + " as TRANTYPE"
            End If
            strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.PCS ELSE 0 END AS PCS"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & bothWeight(0) & " ELSE 0 END " & bothWeight(0) & ""
                strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & bothWeight(1) & " ELSE 0 END " & bothWeight(1) & ""
            Else
                strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & Weight & " ELSE 0 END " & Weight & ""
            End If
            strSql += vbCrLf + " ,CASE WHEN (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.STNPCS ELSE 0 END AS STNPCS"

            strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I.STNWT ELSE 0 END AS STNWT"

            strSql += vbCrLf + " ,CASE WHEN (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.DIAPCS ELSE 0 END AS DIAPCS"

            strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I.DIAWT ELSE 0 END AS DIAWT"

            strSql += vbCrLf + " ,CASE WHEN (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.PREPCS ELSE 0 END AS PREPCS"

            strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I.PREWT ELSE 0 END AS PREWT"

            strSql += vbCrLf + " ,CA.CATNAME,ME.METALNAME AS METAL"
            strSql += vbCrLf + " ,CASE WHEN CA.GS11 = 'Y' THEN '" & Gs11Name & "' WHEN CA.GS12 = 'Y' THEN '" & Gs12Name & "' ELSE 'OTHER' END AS GS"
            strSql += vbCrLf + " ,I.TRANDATE,I.ACCODE AS ACNAME"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(100),IM.ITEMNAME) AS ITEMNAME"
            strSql += vbCrLf + " from TEMPTABLEDB..TEMP" & systemId & IIf(RecIss = "I", "ISS", "REC") & " I "
            'strSql += vbCrLf + " LEFT JOIN " & cnStockDb & "..RECEIPT R ON R.ITEMID=I.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD AS HE ON HE.ACCODE = I.ACCODE"
            strSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            If OrderInfo = False Then
                strSql += vbCrLf + " AND I.BATCHNO NOT IN (SELECT BATCHNO from TEMPTABLEDB..TEMP" & systemId & "ORDREP_DEL)"
            Else
                strSql += vbCrLf + " AND I.BATCHNO IN (SELECT BATCHNO from TEMPTABLEDB..TEMP" & systemId & "ORDREP_DEL)  --AND I.GRSWT > 0 AND I.TRANTYPE <> 'SA' "
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        Else 'O'

            strSql = " /** " & InsComments & "**/"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPDB..#OPETRAN" & IIf(RecIss = "I", "ISS", "REC") & "','U') IS NOT NULL DROP TABLE #OPETRAN" & IIf(RecIss = "I", "ISS", "REC") & ""
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            strSql = vbCrLf + " SELECT "
            strSql += vbCrLf + " 'OP'TRANTYPE"
            strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.PCS ELSE 0 END AS PCS"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & bothWeight(0) & " ELSE 0 END " & bothWeight(0) & ""
                strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & bothWeight(1) & " ELSE 0 END " & bothWeight(1) & ""
            Else
                strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & Weight & " ELSE 0 END " & Weight & ""
            End If
            strSql += vbCrLf + " ,CASE WHEN (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.STNPCS ELSE 0 END AS STNPCS"
            strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I.STNWT ELSE 0 END AS STNWT"
            strSql += vbCrLf + " ,CASE WHEN (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.DIAPCS ELSE 0 END AS DIAPCS"
            strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I.DIAWT ELSE 0 END AS DIAWT"
            strSql += vbCrLf + " ,CASE WHEN (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.PREPCS ELSE 0 END AS PREPCS"
            strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I.PREWT ELSE 0 END AS PREWT"
            strSql += vbCrLf + " ,CA.CATNAME,ME.METALNAME AS METAL"
            strSql += vbCrLf + " ,CASE WHEN CA.GS11 = 'Y' THEN '" & Gs11Name & "' WHEN CA.GS12 = 'Y' THEN '" & Gs12Name & "' ELSE 'OTHER' END AS GS"
            strSql += vbCrLf + " INTO #OPETRAN" & IIf(RecIss = "I", "ISS", "REC") & ""
            strSql += vbCrLf + " from TEMPTABLEDB..TEMP" & systemId & IIf(RecIss = "I", "ISS", "REC") & " I "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID"
            strSql += vbCrLf + " WHERE I.TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            If OrderInfo = False Then
                strSql += vbCrLf + " AND I.BATCHNO NOT IN (SELECT BATCHNO from TEMPTABLEDB..TEMP" & systemId & "ORDREP_DEL)"
            Else
                strSql += vbCrLf + " AND I.BATCHNO IN (SELECT BATCHNO from TEMPTABLEDB..TEMP" & systemId & "ORDREP_DEL) AND I.GRSWT > 0 AND I.TRANTYPE <> 'SA' "
            End If
            strSql += vbCrLf + ""

            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '" & IIf(RecIss = "I", "ISS", "REC") & "' SEP"
            strSql += vbCrLf + " ,NULL TRANNO"
            strSql += vbCrLf + " ,TRANTYPE"
            strSql += vbCrLf + " ,SUM(PCS) AS PCS"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ,SUM(" & bothWeight(0) & ") " & bothWeight(0) & ""
                strSql += vbCrLf + " ,SUM(" & bothWeight(1) & ") " & bothWeight(1) & ""
            Else
                strSql += vbCrLf + " ,SUM(" & Weight & ") " & Weight & ""
            End If
            strSql += vbCrLf + " ,SUM(STNPCS) AS STNPCS"
            strSql += vbCrLf + " ,SUM(STNWT) AS STNWT"
            strSql += vbCrLf + " ,SUM(DIAPCS) AS DIAPCS"
            strSql += vbCrLf + " ,SUM(DIAWT)  AS DIAWT"
            strSql += vbCrLf + " ,SUM(PREPCS) AS PREPCS"
            strSql += vbCrLf + " ,SUM(PREWT) AS PREWT"
            strSql += vbCrLf + " ,CATNAME,METAL"
            strSql += vbCrLf + " ,GS"
            strSql += vbCrLf + " ,NULL TRANDATE,NULL AS ACNAME,NULL ITEMNAME"
            strSql += vbCrLf + " FROM #OPETRAN" & IIf(RecIss = "I", "ISS", "REC") & " AS I"
            strSql += vbCrLf + " GROUP BY TRANTYPE,CATNAME,GS,METAL"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        End If

    End Sub

    Private Sub InsGsStonePrint(ByVal Tran As String, ByVal RecIss As String, Optional ByVal InsComments As String = "", Optional ByVal apptran As String = "")
        Dim bothWeight As String()
        bothWeight = Weight.Split(",")
        If Tran = "T" Then
            strSql = " /** " & InsComments & "**/"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '" & IIf(RecIss = "I", "ISS", "REC") & "' SEP,S.TRANNO"
            If RecIss = "I" Then
                strSql += vbCrLf + " ,CASE   WHEN I.TRANTYPE = 'SA' THEN  'SALES'"
                strSql += vbCrLf + " WHEN I.TRANTYPE = 'MI' THEN 'MAT ISS'"
                strSql += vbCrLf + " WHEN I.TRANTYPE = 'IIS' THEN 'ISSUE'"
                strSql += vbCrLf + " WHEN I.TRANTYPE = 'OD' THEN 'ORD DELIVERY'"
                strSql += vbCrLf + " WHEN I.TRANTYPE = 'RD' THEN 'REP DELIVERY'"
                strSql += vbCrLf + " else i.trantype end  "
                strSql += vbCrLf + " as TRANTYPE"
            Else
                strSql += vbCrLf + " ,CASE   WHEN S.TRANTYPE = 'PU' THEN  'PURCHASE'"
                strSql += vbCrLf + " WHEN S.TRANTYPE = 'RPU' THEN 'DIR.PURCHASE'"
                strSql += vbCrLf + " WHEN S.TRANTYPE = 'RRE' THEN 'MAT RECEIPT'"
                strSql += vbCrLf + " WHEN S.TRANTYPE = 'SR' THEN 'SALES RETURN'"
                strSql += vbCrLf + " ELSE S.TRANTYPE END "
                strSql += vbCrLf + " AS TRANTYPE"
            End If
            strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('P','B')) THEN S.STNPCS ELSE 0 END PCS"
            If rdbBothwt.Checked Then
                If chkIntoCt.Checked Then
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN case when S.STONEUNIT = 'G' THEN S.STNWT*5 ELSE S.STNWT END ELSE 0 END " & bothWeight(0) & ""
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN case when S.STONEUNIT = 'G' THEN S.STNWT*5 ELSE S.STNWT END ELSE 0 END " & bothWeight(1) & ""
                ElseIf chkIntoGm.Checked Then
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN case when S.STONEUNIT = 'C' THEN S.STNWT/5 ELSE S.STNWT END ELSE 0 END " & bothWeight(0) & ""
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN case when S.STONEUNIT = 'C' THEN S.STNWT/5 ELSE S.STNWT END ELSE 0 END " & bothWeight(1) & ""
                Else
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END " & bothWeight(0) & ""
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END " & bothWeight(1) & ""
                End If
            Else
                If chkIntoCt.Checked Then
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN case when S.STONEUNIT = 'G' THEN S.STNWT*5 ELSE S.STNWT END ELSE 0 END " & Weight & ""
                ElseIf chkIntoGm.Checked Then
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN case when S.STONEUNIT = 'C' THEN S.STNWT/5 ELSE S.STNWT END ELSE 0 END " & Weight & ""
                Else
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END " & Weight & ""
                End If
            End If
            strSql += vbCrLf + " ,0 AS STNPCS,0 AS STNWT,0 AS DIAPCS,0 AS DIAWT,0 AS PREPCS,0 AS PREWT"
            strSql += vbCrLf + " ,CA.CATNAME AS CATNAME,ME.METALNAME AS METAL"
            strSql += vbCrLf + " ,CASE WHEN CA.GS11 = 'Y' THEN '" & Gs11Name & "' WHEN CA.GS12 = 'Y' THEN '" & Gs12Name & "' ELSE 'OTHER' END AS GS"
            strSql += vbCrLf + " ,S.TRANDATE"
            strSql += vbCrLf + " ,CASE WHEN ISNULL(HE.ACNAME,'')='' THEN  PINF.PNAME ELSE HE.ACNAME END AS ACNAME"

            strSql += vbCrLf + " ,CONVERT(VARCHAR(100),IM.ITEMNAME) AS ITEMNAME"
            strSql += vbCrLf + " FROM " & cnStockDb & ".." & IIf(RecIss = "I", "ISSSTONE", "RECEIPTSTONE") & " S "

            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = S.STNITEMID AND SM.SUBITEMID = S.STNSUBITEMID"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = S.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "")
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID " & IIf(chkMetal <> "", " AND ME.METALNAME IN (" & chkMetal & ")", "")
            strSql += vbCrLf + " INNER JOIN " & cnStockDb & ".." & IIf(RecIss = "I", "ISSUE", "RECEIPT") & " AS I ON I.TRANDATE = S.TRANDATE AND I.BATCHNO = S.BATCHNO AND I.SNO = S.ISSSNO AND ISNULL(I.CANCEL,'') = ''"

            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IIM ON IIM.ITEMID = I.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS ISM ON ISM.ITEMID = I.ITEMID AND ISM.SUBITEMID = I.SUBITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO AS CINF ON I.BATCHNO = CINF.BATCHNO"
            strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..PERSONALINFO AS PINF ON CINF.PSNO = PINF.SNO"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD AS HE ON HE.ACCODE = I.ACCODE"
            strSql += vbCrLf + " WHERE S.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND S.COMPANYID = '" & strCompanyId & "'"
            If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
                strSql += vbCrLf + " AND ISNULL(S.STNITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
            End If
            If TranType <> "" Then strSql += vbCrLf + " AND S.TRANTYPE IN (" & TranType & ")"
            If chkCostName <> "" Then strSql += vbCrLf + " AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If apptran <> "" Then strSql += vbCrLf + " AND S.TRANTYPE NOT IN (" & apptran & ")"

            If ChkRepairDet.Checked = False Then strSql += vbCrLf + " AND S.BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & ".." & IIf(RecIss = "I", "ISSSTONE", "RECEIPTSTONE") & " WHERE TRANTYPE = 'RD')"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        Else 'O'
            strSql = " /** " & InsComments & "**/"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPDB..#OPESTUD" & IIf(RecIss = "I", "ISS", "REC") & "') IS NOT NULL DROP TABLE #OPESTUD" & IIf(RecIss = "I", "ISS", "REC") & ""
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            strSql = vbCrLf + " SELECT "
            strSql += vbCrLf + " 'OP'TRANTYPE"
            strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('P','B')) THEN S.STNPCS ELSE 0 END PCS"

            If rdbBothwt.Checked Then
                If chkIntoCt.Checked Then
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN case when S.STONEUNIT = 'G' THEN S.STNWT*5 ELSE S.STNWT END ELSE 0 END " & bothWeight(0) & ""
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN case when S.STONEUNIT = 'G' THEN S.STNWT*5 ELSE S.STNWT END ELSE 0 END " & bothWeight(1) & ""
                ElseIf chkIntoGm.Checked Then
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN case when S.STONEUNIT = 'C' THEN S.STNWT/5 ELSE S.STNWT END ELSE 0 END " & bothWeight(0) & ""
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN case when S.STONEUNIT = 'C' THEN S.STNWT/5 ELSE S.STNWT END ELSE 0 END " & bothWeight(1) & ""
                Else
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END " & bothWeight(0) & ""
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END " & bothWeight(1) & ""
                End If
            Else
                If chkIntoCt.Checked Then
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN case when S.STONEUNIT = 'G' THEN S.STNWT*5 ELSE S.STNWT END ELSE 0 END " & Weight & ""
                ElseIf chkIntoGm.Checked Then
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN case when S.STONEUNIT = 'C' THEN S.STNWT/5 ELSE S.STNWT END ELSE 0 END " & Weight & ""
                Else
                    strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END " & Weight & ""
                End If
            End If
            strSql += vbCrLf + " ,0 AS STNPCS,0 AS STNWT,0 AS DIAPCS,0 AS DIAWT,0 AS PREPCS,0 AS PREWT"
            strSql += vbCrLf + " ,CA.CATNAME AS CATNAME,ME.METALNAME AS METAL"
            strSql += vbCrLf + " ,CASE WHEN CA.GS11 = 'Y' THEN '" & Gs11Name & "' WHEN CA.GS12 = 'Y' THEN '" & Gs12Name & "' ELSE 'OTHER' END AS GS"
            strSql += vbCrLf + " INTO #OPESTUD" & IIf(RecIss = "I", "ISS", "REC") & ""
            strSql += vbCrLf + " FROM " & cnStockDb & ".." & IIf(RecIss = "I", "ISSSTONE", "RECEIPTSTONE") & " S "
            'strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = S.STNITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = S.STNITEMID AND SM.SUBITEMID = S.STNSUBITEMID"
            'strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = IM.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "")
            'strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = IM.METALID " & IIf(chkMetal <> "", " AND ME.METALNAME IN (" & chkMetal & ")", "")
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = S.CATCODE " & IIf(chkCategory <> "", " AND CA.CATNAME IN (" & chkCategory & ")", "")
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID " & IIf(chkMetal <> "", " AND ME.METALNAME IN (" & chkMetal & ")", "")
            strSql += vbCrLf + " INNER JOIN " & cnStockDb & ".." & IIf(RecIss = "I", "ISSUE", "RECEIPT") & " AS I ON I.TRANDATE = S.TRANDATE AND I.BATCHNO = S.BATCHNO AND I.SNO = S.ISSSNO AND ISNULL(I.CANCEL,'') = ''"
            'strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..ITEMMAST AS IIM ON IIM.ITEMID = I.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IIM ON IIM.ITEMID = I.ITEMID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS ISM ON ISM.ITEMID = I.ITEMID AND ISM.SUBITEMID = I.SUBITEMID"
            strSql += vbCrLf + " WHERE S.TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND S.COMPANYID = '" & strCompanyId & "'"
            If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
                strSql += vbCrLf + " AND ISNULL(S.STNITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
            End If
            If TranType <> "" Then strSql += vbCrLf + " AND S.TRANTYPE IN (" & TranType & ")"
            If apptran <> "" Then strSql += vbCrLf + " AND S.TRANTYPE NOT IN (" & apptran & ")"
            If ChkRepairDet.Checked = False Then strSql += vbCrLf + " AND S.BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & ".." & IIf(RecIss = "I", "ISSSTONE", "RECEIPTSTONE") & " WHERE TRANTYPE = 'RD')"
            If chkCostName <> "" Then strSql += vbCrLf + " AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            strSql += vbCrLf + ""
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '" & IIf(RecIss = "I", "ISS", "REC") & "' SEP"
            strSql += vbCrLf + " ,NULL TRANNO"
            strSql += vbCrLf + " ,TRANTYPE"
            strSql += vbCrLf + " ,SUM(PCS)PCS"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ,SUM(" & bothWeight(0) & ")" & bothWeight(0) & ""
                strSql += vbCrLf + " ,SUM(" & bothWeight(1) & ")" & bothWeight(1) & ""
            Else
                strSql += vbCrLf + " ,SUM(" & Weight & ")" & Weight & ""
            End If
            strSql += vbCrLf + " ,SUM(STNPCS) AS STNPCS"
            strSql += vbCrLf + " ,SUM(STNWT) AS STNWT"
            strSql += vbCrLf + " ,SUM(DIAPCS) AS DIAPCS"
            strSql += vbCrLf + " ,SUM(DIAWT)  AS DIAWT"
            strSql += vbCrLf + " ,SUM(PREPCS) AS PREPCS"
            strSql += vbCrLf + " ,SUM(PREWT) AS PREWT"
            strSql += vbCrLf + " ,CATNAME,METAL"
            strSql += vbCrLf + " ,GS"
            strSql += vbCrLf + " ,NULL TRANDATE,CONVERT(VARCHAR(100),NULL) AS ACNAME,CONVERT(VARCHAR(100),NULL) AS ITEMNAME"
            strSql += vbCrLf + " FROM #OPESTUD" & IIf(RecIss = "I", "ISS", "REC") & ""
            strSql += vbCrLf + " GROUP BY TRANTYPE,CATNAME,METAL,GS"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        End If

    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Dim bothWeight As String()
        Weight = IIf(rbtGrsWeight.Checked, "GRSWT", "NETWT")
        If rbtPureWeight.Checked Then Weight = "PUREWT"
        If rdbBothwt.Checked Then
            Weight = "GRSWT,NETWT"
        End If
        If rdbBothwt.Checked Then
            bothWeight = Weight.Split(",")
        End If
        If chkIntoCt.Checked And chkIntoGm.Checked Then chkIntoCt.Checked = False : chkIntoGm.Checked = False
        If Not chkLstCostCentre.CheckedItems.Count > 0 Then
            chkCostCentreSelectAll.Checked = True
        End If
        If Not chkLstMetal.CheckedItems.Count > 0 Then
            chkMetalSelectAll.Checked = True
            LoadCategory()
        End If
        If Not chkLstCategory.CheckedItems.Count > 0 Then
            chkCategorySelectAll.Checked = True
        End If
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        chkCostName = GetChecked_CheckedList(chkLstCostCentre)
        chkMetal = GetChecked_CheckedList(chkLstMetal)
        chkCategory = GetChecked_CheckedList(chkLstCategory)
        TranType = ""
        Apptrantype = ""
        Misctrantype = ""
        If chkCmbTransation.Text <> "ALL" And chkCmbTransation.Text <> "" Then
            If chkCmbTransation.Text.Contains("GENERAL") Then
                TranType += "'SA','OD','SR','PU','IIS','IPU','RRE','RPU','AD',"
            End If
            If chkCmbTransation.Text.Contains("MISCISSUE") Then
                TranType += "'MI','IOT','ROT','RIS',"
            End If
            If chkCmbTransation.Text.Contains("APPROVAL") Then
                TranType += "'AI','AR','IAP','RAP',"
            End If
            If chkCmbTransation.Text.Contains("INTERNAL") Then
                TranType += "'IIN','RIN',"
            End If
            If TranType <> "" Then
                TranType = Mid(TranType, 1, TranType.Length - 1)
            End If
            If chkCmbTransation.Text.Contains("APPROVAL") And ChkWithApproval.Checked = False And chkMIMRApproval.Checked = False Then
                MsgBox("Must select Any One Approval Type Counter/MIMR", MsgBoxStyle.Information)
                Exit Sub
            End If
        Else
            If ChkWithApproval.Checked = False And chkMIMRApproval.Checked = False Then
                Apptrantype = "'AI','AR','IAP','RAP'"
            ElseIf ChkWithApproval.Checked = False Then
                Apptrantype = "'AI','AR'"
            ElseIf chkMIMRApproval.Checked = False Then
                Apptrantype = "'IAP','RAP'"
            End If
        End If
        If chkWithMiscIssue.Checked = False Then
            Misctrantype = "'MI'"
        End If

        Dim FilterAccode As String = ""
        If CmbAcname.Text <> "ALL" And CmbAcname.Text <> "" Then
            strSql = "SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & CmbAcname.Text & "'"
            FilterAccode = GetSqlValue(cn, strSql)
        End If

        strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ORDREP_DEL' AND XTYPE = 'U')>0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ORDREP_DEL"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "GSSTOCK' AND XTYPE = 'U')>0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "GSSTOCK_1' AND XTYPE = 'U')>0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ISS' AND XTYPE = 'U')>0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ISS"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "REC' AND XTYPE = 'U')>0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "REC"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ISS' AND XTYPE = 'V')>0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ISS"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "REC' AND XTYPE = 'V')>0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "REC"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        If chkSpecificFormat.Checked = False Then
            strSql = " "
            strSql += vbCrLf + " "
            strSql += vbCrLf + " SELECT ITEMID,SUBITEMID,TRANNO,TRANDATE,BATCHNO,CATCODE,SNO,ACCODE,TRANTYPE"
            strSql += vbCrLf + " ,PCS,GRSWT,NETWT,PUREWT"
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
            If chkIntoCt.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(case when STONEUNIT = 'G' THEN STNWT*5 ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            ElseIf chkIntoGm.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            Else
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            End If
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
            If chkIntoCt.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT*5 ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            ElseIf chkIntoGm.Checked Then
                strSql += vbCrLf + " ,(SELECT sum(CASE WHEN STONEUNIT = 'C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            Else
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            End If
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREPCS"
            If chkIntoCt.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT*5 ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
            ElseIf chkIntoGm.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
            Else
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
            End If
            strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE='IIN' AND ISNULL(I.FLAG,'') IN ('T','X') THEN ISNULL(I.REMARK1,'') ELSE '' END REMARK1"
            strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE='IIN' AND ISNULL(I.FLAG,'') IN ('T','X') THEN ISNULL(I.REMARK2,'') ELSE '' END REMARK2"
            strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE='IIN' AND ISNULL(I.FLAG,'') IN ('T','X') THEN ISNULL(I.REFNO,'') ELSE '' END REFNO"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ISS FROM " & cnStockDb & "..ISSUE AS I"
            strSql += vbCrLf + " WHERE I.TRANTYPE <> 'IRC' AND I.TRANDATE <= '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            If CmbAcname.Text <> "ALL" And CmbAcname.Text <> "" Then
                strSql += vbCrLf + " AND ISNULL(I.ACCODE,'') = '" & FilterAccode & "'"
            End If
            If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
                strSql += " AND ISNULL(I.ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
            End If
            If TranType <> "" Then strSql += vbCrLf + " AND I.TRANTYPE IN (" & TranType & ")"
            If Apptrantype <> "" Then strSql += vbCrLf + " AND I.TRANTYPE NOT IN (" & Apptrantype & ")"
            If Misctrantype <> "" Then strSql += vbCrLf + " AND I.TRANTYPE NOT IN (" & Misctrantype & ")"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            If ChkRepairDet.Checked = False Then strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = I.BATCHNO AND TRANTYPE = 'RD')"
            strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSMETAL WHERE ISSSNO = I.SNO)"
            If cmbstocktype.Text = "MANUFACTURING" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='M'"
            ElseIf cmbstocktype.Text = "EXEMPTED" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='E'"
            ElseIf cmbstocktype.Text = "TRADING" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'') NOT IN('M','E')"
            End If
            If ExcludeItem <> "" And ChkMfgItem.Checked Then
                strSql += vbCrLf + " AND I.ITEMID NOT IN(" & ExcludeItem & ")"
            End If
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT I.ITEMID,I.SUBITEMID,ISM.TRANNO,ISM.TRANDATE,ISM.BATCHNO,ISM.CATCODE,I.SNO,I.ACCODE,ISM.TRANTYPE"
            strSql += vbCrLf + " ,I.PCS,ISM.GRSWT AS GRSWT,CASE WHEN ISNULL(ISM.NETWT,0)<>0 THEN ISM.NETWT ELSE ISM.GRSWT END AS NETWT,CASE WHEN ISNULL(ISM.NETWT,0)<>0 THEN ISM.NETWT ELSE ISM.GRSWT END AS PUREWT"
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
            If chkIntoCt.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(case when stoneunit ='G' THEN STNWT*5 ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            ElseIf chkIntoGm.Checked Then
                strSql += vbCrLf + " ,(SELECT  SUM(case when stoneunit = 'C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            Else
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            End If
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
            If chkIntoCt.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT*5 ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            ElseIf chkIntoGm.Checked Then
                strSql += vbCrLf + " ,(SELECT sum(CASE WHEN STONEUNIT ='C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            Else
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            End If
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREPCS"
            If chkIntoCt.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT*5 ELSE STNWT END)  FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
            ElseIf chkIntoGm.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
            Else
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
            End If
            strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE='IIN' AND ISNULL(I.FLAG,'') IN ('T','X') THEN ISNULL(I.REMARK1,'') ELSE '' END REMARK1"
            strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE='IIN' AND ISNULL(I.FLAG,'') IN ('T','X') THEN ISNULL(I.REMARK2,'') ELSE '' END REMARK2"
            strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE='IIN' AND ISNULL(I.FLAG,'') IN ('T','X') THEN ISNULL(I.REFNO,'') ELSE '' END REFNO"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSMETAL AS ISM "
            strSql += vbCrLf + " LEFT JOIN " & cnStockDb & "..ISSUE AS I ON ISM.ISSSNO=I.SNO"
            strSql += vbCrLf + " WHERE ISM.TRANTYPE <> 'IRC' AND ISM.TRANDATE <= '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND ISM.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            If CmbAcname.Text <> "ALL" And CmbAcname.Text <> "" Then
                strSql += vbCrLf + " AND ISNULL(I.ACCODE,'') = '" & FilterAccode & "'"
            End If
            If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
                strSql += " AND ISNULL(I.ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
            End If
            If TranType <> "" Then strSql += vbCrLf + " AND ISM.TRANTYPE IN (" & TranType & ")"
            If Apptrantype <> "" Then strSql += vbCrLf + " AND ISM.TRANTYPE NOT IN (" & Apptrantype & ")"
            If Misctrantype <> "" Then strSql += vbCrLf + " AND ISM.TRANTYPE NOT IN (" & Misctrantype & ")"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND ISM.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkCategory <> "" Then strSql += vbCrLf + "  AND ISM.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            If ChkRepairDet.Checked = False Then strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = I.BATCHNO AND TRANTYPE = 'RD')"
            If chkWithAlloy.Checked Then
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT I.ITEMID,I.SUBITEMID,AD.TRANNO,AD.TRANDATE,AD.BATCHNO"
                strSql += vbCrLf + " ,ISNULL((SELECT TOP 1 CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID=AD.ALLOYID),I.CATCODE) CATCODE"
                strSql += vbCrLf + " ,AD.SNO,I.ACCODE,AD.TRANTYPE,0 PCS,WEIGHT GRSWT,WEIGHT NETWT,WEIGHT PUREWT"
                strSql += vbCrLf + " ,NULL AS STNPCS,NULL AS STNWT,NULL AS DIAPCS,NULL AS DIAWT,NULL AS PREPCS,NULL AS PREWT,NULL AS REMARK1,NULL AS REMARK2,NULL AS REFNO"
                strSql += vbCrLf + " FROM " & cnStockDb & "..ALLOYDETAILS AS AD"
                strSql += vbCrLf + " LEFT JOIN " & cnStockDb & "..ISSUE AS I ON AD.ISSSNO=I.SNO"
                strSql += vbCrLf + " WHERE AD.TRANDATE <= '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
                If CmbAcname.Text <> "ALL" And CmbAcname.Text <> "" Then
                    strSql += vbCrLf + " AND ISNULL(I.ACCODE,'') = '" & FilterAccode & "'"
                End If
                If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
                    strSql += " AND ISNULL(I.ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
                End If
                If TranType <> "" Then strSql += vbCrLf + " AND AD.TRANTYPE IN (" & TranType & ")"
                If Apptrantype <> "" Then strSql += vbCrLf + " AND AD.TRANTYPE NOT IN (" & Apptrantype & ")"
                If Misctrantype <> "" Then strSql += vbCrLf + " AND AD.TRANTYPE NOT IN (" & Misctrantype & ")"
                If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
                If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
                If ChkRepairDet.Checked = False Then strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = I.BATCHNO AND TRANTYPE IN('RD'))"
                strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = I.BATCHNO AND TRANTYPE IN('RRE'))"
                If cmbstocktype.Text = "MANUFACTURING" Then
                    strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='M'"
                ElseIf cmbstocktype.Text = "EXEMPTED" Then
                    strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='E'"
                ElseIf cmbstocktype.Text = "TRADING" Then
                    strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'') NOT IN('M','E')"
                End If
                If ExcludeItem <> "" And ChkMfgItem.Checked Then
                    strSql += vbCrLf + " AND I.ITEMID NOT IN(" & ExcludeItem & ")"
                End If
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

            strSql = " "
            strSql += vbCrLf + " "
            strSql += vbCrLf + " SELECT ITEMID,SUBITEMID,TRANNO,TRANDATE,BATCHNO,CATCODE,SNO,ACCODE,TRANTYPE"
            strSql += vbCrLf + " ,PCS,GRSWT,NETWT,PUREWT"
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
            If chkIntoCt.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT*5 ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            ElseIf chkIntoGm.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            Else
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            End If
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
            If chkIntoCt.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT*5 ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            ElseIf chkIntoGm.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT/5  ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            Else
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            End If
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREPCS"
            If chkIntoCt.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT*5 ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
            ElseIf chkIntoGm.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT ='C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
            Else
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
            End If
            strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE='RIN' AND ISNULL(I.FLAG,'') IN ('T','X') THEN ISNULL(I.REMARK1,'') ELSE '' END REMARK1"
            strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE='RIN' AND ISNULL(I.FLAG,'') IN ('T','X') THEN ISNULL(I.REMARK2,'') ELSE '' END REMARK2"
            strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE='RIN' AND ISNULL(I.FLAG,'') IN ('T','X') THEN ISNULL(I.REFNO,'') ELSE '' END REFNO"
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "REC FROM " & cnStockDb & "..RECEIPT AS I"
            strSql += vbCrLf + " WHERE I.TRANTYPE <> 'RRC' AND I.TRANDATE <= '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            If CmbAcname.Text <> "ALL" And CmbAcname.Text <> "" Then
                strSql += vbCrLf + " AND ISNULL(I.ACCODE,'') = '" & FilterAccode & "'"
            End If
            If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
                strSql += " AND ISNULL(I.ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
            End If
            If TranType <> "" Then strSql += vbCrLf + " AND I.TRANTYPE IN (" & TranType & ")"
            If Apptrantype <> "" Then strSql += vbCrLf + " AND I.TRANTYPE NOT IN (" & Apptrantype & ")"
            If Misctrantype <> "" Then strSql += vbCrLf + " AND I.TRANTYPE NOT IN (" & Misctrantype & ")"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            If ChkRepairDet.Checked = False Then strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO = I.BATCHNO AND TRANTYPE = 'RD')"
            strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..RECEIPTMETAL WHERE ISSSNO = I.SNO)"
            If cmbstocktype.Text = "MANUFACTURING" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='M'"
            ElseIf cmbstocktype.Text = "EXEMPTED" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='E'"
            ElseIf cmbstocktype.Text = "TRADING" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'') NOT IN('M','E')"
            End If
            If ExcludeItem <> "" And ChkMfgItem.Checked Then
                strSql += vbCrLf + " AND I.ITEMID NOT IN(" & ExcludeItem & ")"
            End If
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT I.ITEMID,I.SUBITEMID,ISM.TRANNO,ISM.TRANDATE,ISM.BATCHNO,ISM.CATCODE,I.SNO,I.ACCODE,ISM.TRANTYPE"
            strSql += vbCrLf + " ,I.PCS,ISM.GRSWT AS GRSWT,CASE WHEN ISNULL(ISM.NETWT,0)<>0 THEN ISM.NETWT ELSE ISM.GRSWT END AS NETWT,CASE WHEN ISNULL(ISM.NETWT,0)<>0 THEN ISM.NETWT ELSE ISM.GRSWT END AS PUREWT"
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
            If chkIntoCt.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(case when stoneunit ='G' THEN STNWT*5 ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            ElseIf chkIntoGm.Checked Then
                strSql += vbCrLf + " ,(SELECT  SUM(case when stoneunit = 'C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            Else
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            End If
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
            If chkIntoCt.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT*5 ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            ElseIf chkIntoGm.Checked Then
                strSql += vbCrLf + " ,(SELECT sum(CASE WHEN STONEUNIT ='C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            Else
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            End If
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREPCS"
            If chkIntoCt.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT*5 ELSE STNWT END)  FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
            ElseIf chkIntoGm.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
            Else
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
            End If
            strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE='RIN' AND ISNULL(I.FLAG,'') IN ('T','X') THEN ISNULL(I.REMARK1,'') ELSE '' END REMARK1"
            strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE='RIN' AND ISNULL(I.FLAG,'') IN ('T','X') THEN ISNULL(I.REMARK2,'') ELSE '' END REMARK2"
            strSql += vbCrLf + " ,CASE WHEN I.TRANTYPE='RIN' AND ISNULL(I.FLAG,'') IN ('T','X') THEN ISNULL(I.REFNO,'') ELSE '' END REFNO"
            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTMETAL AS ISM "
            strSql += vbCrLf + " LEFT JOIN " & cnStockDb & "..RECEIPT AS I ON ISM.ISSSNO=I.SNO"
            strSql += vbCrLf + " WHERE ISM.TRANTYPE <> 'RRC' AND ISM.TRANDATE <= '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND ISM.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            If CmbAcname.Text <> "ALL" And CmbAcname.Text <> "" Then
                strSql += vbCrLf + " AND ISNULL(I.ACCODE,'') = '" & FilterAccode & "'"
            End If
            If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
                strSql += " AND ISNULL(I.ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
            End If
            If TranType <> "" Then strSql += vbCrLf + " AND ISM.TRANTYPE IN (" & TranType & ")"
            If Apptrantype <> "" Then strSql += vbCrLf + " AND ISM.TRANTYPE NOT IN (" & Apptrantype & ")"
            If Misctrantype <> "" Then strSql += vbCrLf + " AND ISM.TRANTYPE NOT IN (" & Misctrantype & ")"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND ISM.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkCategory <> "" Then strSql += vbCrLf + "  AND ISM.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            If ChkRepairDet.Checked = False Then strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO = I.BATCHNO AND TRANTYPE = 'RD')"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

            strSql = " SELECT DISTINCT BATCHNO,TRANTYPE INTO TEMPTABLEDB..TEMP" & systemId & "ORDREP_DEL "
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
            strSql += vbCrLf + " WHERE TRANTYPE IN ('OD','RD') AND ISNULL(CANCEL,'') = ''"
            If cmbstocktype.Text = "MANUFACTURING" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='M'"
            ElseIf cmbstocktype.Text = "EXEMPTED" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='E'"
            ElseIf cmbstocktype.Text = "TRADING" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'') NOT IN('M','E')"
            End If
            If ExcludeItem <> "" And ChkMfgItem.Checked Then
                strSql += vbCrLf + " AND I.ITEMID NOT IN(" & ExcludeItem & ")"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

            strSql = " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK"
            strSql += " ("
            strSql += " PARTICULAR VARCHAR(500),REMARK VARCHAR(400),REFNO VARCHAR(40)"
            strSql += vbCrLf + " ,TRANNO INT,TRANTYPE VARCHAR(20),TTRANDATE SMALLDATETIME"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ,[RECEIPT PCS] INT,[RECEIPT " & bothWeight(0) & "] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[RECEIPT " & bothWeight(1) & "] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[RECEIPT STNPCS] INT,[RECEIPT STNWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[RECEIPT DIAPCS] INT,[RECEIPT DIAWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[RECEIPT PREPCS] INT,[RECEIPT PREWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[ISSUE PCS] INT,[ISSUE " & bothWeight(0) & "] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[ISSUE " & bothWeight(1) & "] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[ISSUE STNPCS] INT,[ISSUE STNWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[ISSUE DIAPCS] INT,[ISSUE DIAWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[ISSUE PREPCS] INT,[ISSUE PREWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[CLOSING PCS] INT,[CLOSING " & bothWeight(0) & "] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[CLOSING " & bothWeight(1) & "] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[CLOSING STNPCS] INT,[CLOSING STNWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[CLOSING DIAPCS] INT,[CLOSING DIAWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[CLOSING PREPCS] INT,[CLOSING PREWT] NUMERIC(15,3)"
            Else
                strSql += vbCrLf + " ,[RECEIPT PCS] INT,[RECEIPT " & Weight & "] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[RECEIPT STNPCS] INT,[RECEIPT STNWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[RECEIPT DIAPCS] INT,[RECEIPT DIAWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[RECEIPT PREPCS] INT,[RECEIPT PREWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[ISSUE PCS] INT,[ISSUE " & Weight & "] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[ISSUE STNPCS] INT,[ISSUE STNWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[ISSUE DIAPCS] INT,[ISSUE DIAWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[ISSUE PREPCS] INT,[ISSUE PREWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[CLOSING PCS] INT,[CLOSING " & Weight & "] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[CLOSING STNPCS] INT,[CLOSING STNWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[CLOSING DIAPCS] INT,[CLOSING DIAWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[CLOSING PREPCS] INT,[CLOSING PREWT] NUMERIC(15,3)"
            End If
            strSql += vbCrLf + " ,CATNAME VARCHAR(100),METAL VARCHAR(100),GS VARCHAR(100)"
            strSql += vbCrLf + " ,TRANDATE SMALLDATETIME,ACNAME VARCHAR(200)"
            strSql += vbCrLf + " ,ITEMNAME VARCHAR(80)"
            strSql += vbCrLf + " ,RESULT INT,COLHEAD VARCHAR(3)"
            strSql += vbCrLf + " )"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

            strSql = " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SEP VARCHAR(10),TRANNO INT,TRANTYPE VARCHAR(50),PCS INT"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + "," & bothWeight(0) & " NUMERIC(15,3)"
                strSql += vbCrLf + "," & bothWeight(1) & " NUMERIC(15,3)"
            Else
                strSql += vbCrLf + "," & Weight & " NUMERIC(15,3)"
            End If
            strSql += vbCrLf + " ,STNPCS INT,STNWT NUMERIC(15,3),DIAPCS INT,DIAWT NUMERIC(15,3),PREPCS INT,PREWT NUMERIC(15,3)"
            strSql += vbCrLf + " ,CATNAME VARCHAR(100),METAL VARCHAR(100),GS VARCHAR(100)"
            strSql += vbCrLf + " ,TRANDATE SMALLDATETIME,ACNAME VARCHAR(200)"
            strSql += vbCrLf + " ,ITEMNAME VARCHAR(80),REMARK VARCHAR(400),REFNO VARCHAR(40)"
            strSql += vbCrLf + " )"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

            strSql = " /** INSERTING OPENING FROM OPENWEIGHT **/"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + "  	SELECT "
            strSql += vbCrLf + "  	CASE WHEN I.TRANTYPE = 'I' THEN 'ISS' ELSE 'REC' END SEP"
            strSql += vbCrLf + "  	,NULL TRANNO"
            strSql += vbCrLf + "  	,'OP' TRANTYPE"
            strSql += vbCrLf + "    ,SUM(CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.PCS ELSE 0 END) AS PCS"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + "    ,SUM(CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & bothWeight(0) & " ELSE 0 END) AS " & bothWeight(0) & ""
                strSql += vbCrLf + "    ,SUM(CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & bothWeight(1) & " ELSE 0 END) AS " & bothWeight(1) & ""
            Else
                strSql += vbCrLf + "    ,SUM(CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & Weight & " ELSE 0 END) AS " & Weight & ""
            End If
            strSql += vbCrLf + "    ,SUM(CASE WHEN CA.DIASTNTYPE='T' THEN I.PCS ELSE 0 END) AS STNPCS"
            strSql += vbCrLf + "    ,SUM(CASE WHEN CA.DIASTNTYPE='T' THEN I.GRSWT ELSE 0 END) AS STNWT"
            strSql += vbCrLf + "    ,SUM(CASE WHEN CA.DIASTNTYPE='D' THEN I.PCS ELSE 0 END) AS DIAPCS"
            strSql += vbCrLf + "    ,SUM(CASE WHEN CA.DIASTNTYPE='D' THEN I.GRSWT ELSE 0 END) AS DIAWT"
            strSql += vbCrLf + "    ,SUM(CASE WHEN CA.DIASTNTYPE='P' THEN I.PCS ELSE 0 END) AS PREPCS"
            strSql += vbCrLf + "    ,SUM(CASE WHEN CA.DIASTNTYPE='P' THEN I.GRSWT ELSE 0 END) AS PREWT"
            strSql += vbCrLf + "    ,CA.CATNAME,ME.METALNAME AS METAL"
            strSql += vbCrLf + "    ,CASE WHEN CA.GS11 = 'Y' THEN '" & Gs11Name & "' WHEN CA.GS12 = 'Y' THEN '" & Gs12Name & "' ELSE 'OTHER' END AS GS,NULL TRANDATE,NULL AS ACNAME,NULL ITEMNAME,NULL REMARK,NULL REFNO"
            strSql += vbCrLf + "  	FROM " & cnStockDb & "..OPENWEIGHT I "
            strSql += vbCrLf + "    LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID "
            strSql += vbCrLf + "    LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
            strSql += vbCrLf + "    INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE"
            strSql += vbCrLf + "    INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID"
            strSql += vbCrLf + "  	WHERE I.STOCKTYPE IN ('C')"
            If (ChkWithApproval.Checked = False And chkCmbTransation.Text = "ALL") Then
                strSql += vbCrLf + " AND ISNULL(I.APPROVAL,'')<>'A'"
            ElseIf chkCmbTransation.Text = "APPROVAL" Then
                strSql += vbCrLf + " AND ISNULL(I.APPROVAL,'')='A'"
            End If
            If chkCmbTransation.Text = "INTERNAL" Then
                strSql += vbCrLf + " AND ISNULL(I.APPROVAL,'')='I'"
            End If
            strSql += vbCrLf + "    AND I.COMPANYID = '" & strCompanyId & "'"
            If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
                strSql += " AND ISNULL(I.ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
            End If
            'If TranType <> "" Then strSql += vbCrLf + " AND I.TRANTYPE IN (" & TranType & ")"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkMetal <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetal & ")))"
            If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            If cmbstocktype.Text = "MANUFACTURING" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='M'"
            ElseIf cmbstocktype.Text = "EXEMPTED" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='E'"
            ElseIf cmbstocktype.Text = "TRADING" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'') NOT IN('M','E')"
            End If
            strSql += vbCrLf + "    GROUP BY I.TRANTYPE,CA.CATNAME,CA.GS11,CA.GS12,ME.METALNAME"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            'InsGsOutStDt("O", "INSERTING OPENING FROM OUTSTANDING")
            'InsGsOutStDt("T", "INSERTING TRANSACTION FROM OUTSTANDING")
            If ChkWithOrRecpay.Checked Then InsGsOutStRecPay("O", "INSERTING TRANSACTION FROM OUTSTANDING ORDER INFO")
            If ChkRepairDet.Checked = True Then InsGsOutStRepairRecPay("O", "INSERTING TRANSACTION FROM OUTSTANDING REPAIR INFO")

            If ChkWithOrRecpay.Checked Then InsGsOutStRecPay("T", "INSERTING TRANSACTION FROM OUTSTANDING ORDER INFO")
            If ChkRepairDet.Checked = True Then InsGsOutStRepairRecPay("T", "INSERTING TRANSACTION FROM OUTSTANDING REPAIR INFO")
            InsGsIssRec("O", "I", False, "INSERTING OPENING FROM ISSUE WITHOUT ORDREP INFO")
            InsGsIssRec("O", "R", False, "INSERTING OPENING FROM RECEIPT WITHOUT ORDREP INFO")
            InsGsIssRec("O", "I", True, "INSERTING OPENING ORDREP INFO FROM ISSUE")
            InsGsIssRec("O", "R", True, "INSERTING OPENING ORDREP INFO FROM RECEIPT")
            InsGsStone("O", "I", "INSERTING OPENING FROM ISSUESTONE")
            InsGsStone("O", "R", "INSERTING OPENING FROM RECEIPTSTONE")

            InsGsIssRec("T", "I", False, "INSERTING TRANSACTION FROM ISSUE WITHOUT ORDREP INFO")
            InsGsIssRec("T", "R", False, "INSERTING TRANSACTION FROM RECEIPT WITHOUT ORDREP INFO")
            InsGsIssRec("T", "I", True, "INSERTING TRANSACTION ORDREP INFO FROM ISSUE")
            InsGsIssRec("T", "R", True, "INSERTING TRANSACTION ORDREP INFO FROM RECEIPT")
            InsGsStone("T", "I", "INSERTING TRANSACTION FROM ISSUESTONE")
            InsGsStone("T", "R", "INSERTING TRANSACTION FROM RECEIPTSTONE")

        Else
            strSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPISSUE','U')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPISSUE"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = "SELECT STKTYPE,PCS,CANCEL,COMPANYID,GRSWT,NETWT,PUREWT,ITEMID,SUBITEMID,TRANNO,REFNO,TRANDATE,BATCHNO,CATCODE,ACCODE ,TRANTYPE,SNO,COSTID "
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMPISSUE FROM " & cnStockDb & "..ISSUE I"
            strSql += vbCrLf + " WHERE TRANTYPE <> 'IRC' AND  TRANDATE <= '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND ISNULL(CANCEL,'') = ''"
            If CmbAcname.Text <> "ALL" And CmbAcname.Text <> "" Then
                strSql += vbCrLf + " AND ISNULL(I.ACCODE,'') = '" & FilterAccode & "'"
            End If
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + " UPDATE TEMPTABLEDB..TEMPISSUE SET ACCODE='' WHERE ACCODE='0'"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = " "
            strSql += vbCrLf + " "
            strSql = vbCrLf + " SELECT ITEMID,SUBITEMID,TRANNO,REFNO,I.TRANDATE,I.BATCHNO,CATCODE,CASE WHEN ISNULL(I.ACCODE,'') ='' then pinf.PNAME else A.ACNAME end as ACCODE,TRANTYPE"
            strSql += vbCrLf + " ,SUM(PCS) PCS,"
            'strSql += vbCrLf + "CASE WHEN TRANTYPE = 'RD' THEN SUM(GRSWT)+ISNULL((SELECT SUM(GRSWT) FROM " & cnStockDb & "..ISSUE B WHERE B.BATCHNO = I.BATCHNO AND TRANTYPE = 'RD' AND B.CATCODE =I.CATCODE AND B.GRSWT < 0 ),0)"
            'strSql += vbCrLf + " ELSE SUM(GRSWT) END GRSWT,"
            'strSql += vbCrLf + "CASE WHEN TRANTYPE = 'RD' THEN SUM(NETWT) +ISNULL((SELECT SUM(NETWT) FROM " & cnStockDb & "..ISSUE B WHERE B.BATCHNO = I.BATCHNO AND TRANTYPE = 'RD' AND B.CATCODE =I.CATCODE AND B.NETWT <0 ),0)"
            'strSql += vbCrLf + "ELSE SUM(NETWT) END NETWT"
            strSql += vbCrLf + " SUM(GRSWT) GRSWT,SUM(NETWT) NETWT,SUM(PUREWT) PUREWT"
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
            If chkIntoCt.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT*5 ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            ElseIf chkIntoGm.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(case when stoneunit = 'C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            Else
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            End If
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
            If chkIntoCt.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT*5 ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            ElseIf chkIntoGm.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            Else
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            End If

            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREPCS"
            If chkIntoCt.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT*5 ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
            ElseIf chkIntoGm.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
            Else
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
            End If
            strSql += " ,IDENTITY(INT,1,1) SSNO"
            strSql += " INTO TEMPTABLEDB..TEMP" & systemId & "ISS"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMPISSUE AS I"
            strSql += vbCrLf + " left join " & cnAdminDb & "..CUSTOMERINFO as cinf on i.batchno = cinf.batchno"
            strSql += vbCrLf + " left join  " & cnAdminDb & "..personalinfo as pinf on cinf.psno = pinf.sno"
            strSql += vbCrLf + " left join " & cnAdminDb & "..Achead as A on i.accode = a.accode"
            strSql += vbCrLf + " WHERE I.TRANTYPE <> 'IRC' AND  I.TRANDATE <= '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            If CmbAcname.Text <> "ALL" And CmbAcname.Text <> "" Then
                strSql += vbCrLf + " AND ISNULL(I.ACCODE,'') = '" & FilterAccode & "'"
            End If
            If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
                strSql += " AND ISNULL(I.ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
            End If
            If TranType <> "" Then strSql += vbCrLf + " AND I.TRANTYPE IN (" & TranType & ")"
            If Apptrantype <> "" Then strSql += vbCrLf + " AND I.TRANTYPE NOT IN (" & Apptrantype & ")"
            If Misctrantype <> "" Then strSql += vbCrLf + " AND I.TRANTYPE NOT IN (" & Misctrantype & ")"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            If ChkRepairDet.Checked = False Then strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = I.BATCHNO AND TRANTYPE = 'RD')"
            strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSMETAL WHERE ISSSNO = I.SNO)"
            If cmbstocktype.Text = "MANUFACTURING" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='M'"
            ElseIf cmbstocktype.Text = "EXEMPTED" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='E'"
            ElseIf cmbstocktype.Text = "TRADING" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'') NOT IN('M','E')"
            End If
            strSql += vbCrLf + " group by ITEMID,SUBITEMID,TRANNO,REFNO,I.TRANDATE,I.BATCHNO,CATCODE,i.ACCODE,pinf.PNAME ,TRANTYPE,ACNAME,I.SNO"

            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()



            strSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "ISS"
            strSql += vbCrLf + " SELECT I.ITEMID,I.SUBITEMID,ISM.TRANNO,I.REFNO,ISM.TRANDATE,ISM.BATCHNO,ISM.CATCODE,CASE WHEN ISNULL(I.ACCODE,'') ='' THEN PINF.PNAME ELSE A.ACNAME END AS ACCODE,ISM.TRANTYPE"
            strSql += vbCrLf + " ,SUM(I.PCS) PCS,SUM(ISM.GRSWT) GRSWT,SUM(CASE WHEN ISNULL(ISM.NETWT,0)<>0 THEN ISM.NETWT ELSE ISM.GRSWT END) NETWT,SUM(CASE WHEN ISNULL(ISM.NETWT,0)<>0 THEN ISM.NETWT ELSE ISM.GRSWT END) PUREWT"
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
            If chkIntoCt.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT*5 ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            ElseIf chkIntoGm.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            Else
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            End If
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
            If chkIntoCt.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT ='G' THEN STNWT*5 ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            ElseIf chkIntoGm.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT ='C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            Else
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            End If
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREPCS"
            If chkIntoCt.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT*5 ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
            ElseIf chkIntoGm.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
            Else
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
            End If

            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSMETAL AS ISM "
            strSql += vbCrLf + " LEFT JOIN " & cnStockDb & "..ISSUE AS I ON ISM.ISSSNO=I.SNO"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO AS CINF ON I.BATCHNO = CINF.BATCHNO"
            strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..PERSONALINFO AS PINF ON CINF.PSNO = PINF.SNO"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD AS A ON I.ACCODE = A.ACCODE"
            strSql += vbCrLf + " WHERE ISM.TRANDATE <= '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND ISM.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            If CmbAcname.Text <> "ALL" And CmbAcname.Text <> "" Then
                strSql += vbCrLf + " AND ISNULL(I.ACCODE,'') = '" & FilterAccode & "'"
            End If
            If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
                strSql += " AND ISNULL(I.ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
            End If
            If TranType <> "" Then strSql += vbCrLf + " AND ISM.TRANTYPE IN (" & TranType & ")"
            If Apptrantype <> "" Then strSql += vbCrLf + " AND ISM.TRANTYPE NOT IN (" & Apptrantype & ")"
            If Misctrantype <> "" Then strSql += vbCrLf + " AND ISM.TRANTYPE NOT IN (" & Misctrantype & ")"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND ISM.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkCategory <> "" Then strSql += vbCrLf + "  AND ISM.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            If ChkRepairDet.Checked = False Then strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = I.BATCHNO AND TRANTYPE = 'RD')"
            strSql += vbCrLf + " GROUP BY I.ITEMID,I.SUBITEMID,ISM.TRANNO,I.REFNO,ISM.TRANDATE,ISM.BATCHNO,ISM.CATCODE,I.ACCODE,PINF.PNAME,ISM.TRANTYPE,ACNAME,I.SNO"

            If chkWithAlloy.Checked Then
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT I.ITEMID,I.SUBITEMID,AD.TRANNO,I.REFNO,AD.TRANDATE,AD.BATCHNO,I.CATCODE,CASE WHEN ISNULL(I.ACCODE,'') ='' THEN PINF.PNAME ELSE A.ACNAME END AS ACCODE,AD.TRANTYPE"
                strSql += vbCrLf + " ,0 PCS,SUM(WEIGHT) GRSWT,SUM(WEIGHT) NETWT,SUM(WEIGHT) PUREWT"
                strSql += vbCrLf + " ,NULL AS STNPCS,NULL AS STNWT,NULL AS DIAPCS,NULL AS DIAWT,NULL AS PREPCS,NULL AS PREWT"
                strSql += vbCrLf + " FROM " & cnStockDb & "..ALLOYDETAILS AS AD"
                strSql += vbCrLf + " LEFT JOIN " & cnStockDb & "..ISSUE AS I ON AD.ISSSNO=I.SNO"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO AS CINF ON I.BATCHNO = CINF.BATCHNO"
                strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..PERSONALINFO AS PINF ON CINF.PSNO = PINF.SNO"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD AS A ON I.ACCODE = A.ACCODE"
                strSql += vbCrLf + " WHERE AD.TRANDATE <= '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
                If CmbAcname.Text <> "ALL" And CmbAcname.Text <> "" Then
                    strSql += vbCrLf + " AND ISNULL(I.ACCODE,'') = '" & FilterAccode & "'"
                End If
                If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
                    strSql += " AND ISNULL(I.ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
                End If
                If TranType <> "" Then strSql += vbCrLf + " AND AD.TRANTYPE IN (" & TranType & ")"
                If Apptrantype <> "" Then strSql += vbCrLf + " AND AD.TRANTYPE NOT IN (" & Apptrantype & ")"
                If Misctrantype <> "" Then strSql += vbCrLf + " AND AD.TRANTYPE NOT IN (" & Misctrantype & ")"
                If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
                If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
                If ChkRepairDet.Checked = False Then strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = I.BATCHNO AND TRANTYPE = 'RD')"
                strSql += vbCrLf + " GROUP BY ITEMID,SUBITEMID,AD.TRANNO,REFNO,AD.TRANDATE,AD.BATCHNO,CATCODE,I.ACCODE,PINF.PNAME ,AD.TRANTYPE,ACNAME"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()


            strSql = " "
            strSql += vbCrLf + " "
            strSql += vbCrLf + " SELECT ITEMID,SUBITEMID,REFNO,TRANNO,I.TRANDATE,I.BATCHNO,CATCODE,CASE WHEN ISNULL(I.ACCODE,'') ='' then pinf.PNAME else A.ACNAME end as ACCODE,TRANTYPE"
            'strSql += vbCrLf + " ,CASE   WHEN I.TRANTYPE = 'PU' THEN  'PURCHASE'"
            'strSql += vbCrLf + " WHEN I.TRANTYPE = 'RPU' THEN 'PURCHASE'"
            'strSql += vbCrLf + " WHEN I.TRANTYPE = 'RRE' THEN 'PUR REC'"
            'strSql += vbCrLf + "  else 'OTHERS' end "
            'strSql += vbCrLf + " as TRANTYPE"
            strSql += vbCrLf + " ,SUM(PCS) PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT,SUM(PUREWT) PUREWT"
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
            If chkIntoCt.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT*5 ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            ElseIf chkIntoGm.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            Else
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            End If

            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
            If chkIntoCt.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT*5 ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            ElseIf chkIntoGm.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            Else
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            End If
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREPCS"
            If chkIntoCt.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT*5 ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
            ElseIf chkIntoGm.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
            Else
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
            End If
            strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "REC FROM " & cnStockDb & "..RECEIPT AS I"
            strSql += vbCrLf + " left join " & cnAdminDb & "..CUSTOMERINFO as cinf on i.batchno = cinf.batchno"
            strSql += vbCrLf + " left join  " & cnAdminDb & "..personalinfo as pinf on cinf.psno = pinf.sno"
            strSql += vbCrLf + " left join " & cnAdminDb & "..Achead as A on i.accode = a.accode"
            strSql += vbCrLf + " WHERE I.TRANTYPE <> 'RRC' AND I.TRANDATE <= '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            If CmbAcname.Text <> "ALL" And CmbAcname.Text <> "" Then
                strSql += vbCrLf + " AND ISNULL(I.ACCODE,'') = '" & FilterAccode & "'"
            End If
            If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
                strSql += " AND ISNULL(I.ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
            End If
            If TranType <> "" Then strSql += vbCrLf + " AND I.TRANTYPE IN (" & TranType & ")"
            If Apptrantype <> "" Then strSql += vbCrLf + " AND I.TRANTYPE NOT IN (" & Apptrantype & ")"
            If Misctrantype <> "" Then strSql += vbCrLf + " AND I.TRANTYPE NOT IN (" & Misctrantype & ")"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            If ChkRepairDet.Checked = False Then strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO = I.BATCHNO AND TRANTYPE = 'RD')"
            strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..RECEIPTMETAL WHERE ISSSNO = I.SNO)"
            If cmbstocktype.Text = "MANUFACTURING" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='M'"
            ElseIf cmbstocktype.Text = "EXEMPTED" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='E'"
            ElseIf cmbstocktype.Text = "TRADING" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'') NOT IN('M','E')"
            End If
            strSql += vbCrLf + " group by ITEMID,SUBITEMID,TRANNO,REFNO,I.TRANDATE,I.BATCHNO,CATCODE,i.ACCODE,pinf.PNAME ,TRANTYPE,ACNAME,I.SNO"
            'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

            'strSql = " INSERT INTO TEMP" & systemId & "REC"

            strSql += vbCrLf + " UNION ALL"

            strSql += vbCrLf + " SELECT I.ITEMID,I.SUBITEMID,I.REFNO,ISM.TRANNO,ISM.TRANDATE,ISM.BATCHNO,ISM.CATCODE,CASE WHEN ISNULL(I.ACCODE,'') ='' THEN PINF.PNAME ELSE A.ACNAME END AS ACCODE,ISM.TRANTYPE"
            strSql += vbCrLf + " ,SUM(I.PCS) PCS,SUM(ISM.GRSWT) GRSWT,SUM(CASE WHEN ISNULL(ISM.NETWT,0)<>0 THEN ISM.NETWT ELSE ISM.GRSWT END) NETWT,SUM(CASE WHEN ISNULL(ISM.NETWT,0)<>0 THEN ISM.NETWT ELSE ISM.GRSWT END) PUREWT"
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
            If chkIntoCt.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT*5 ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            ElseIf chkIntoGm.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            Else
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
            End If
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
            If chkIntoCt.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT ='G' THEN STNWT*5 ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            ElseIf chkIntoGm.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT ='C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            Else
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
            End If
            strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREPCS"
            If chkIntoCt.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'G' THEN STNWT*5 ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
            ElseIf chkIntoGm.Checked Then
                strSql += vbCrLf + " ,(SELECT SUM(CASE WHEN STONEUNIT = 'C' THEN STNWT/5 ELSE STNWT END) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
            Else
                strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
            End If

            strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPTMETAL AS ISM "
            strSql += vbCrLf + " LEFT JOIN " & cnStockDb & "..RECEIPT AS I ON ISM.ISSSNO=I.SNO"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO AS CINF ON I.BATCHNO = CINF.BATCHNO"
            strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..PERSONALINFO AS PINF ON CINF.PSNO = PINF.SNO"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD AS A ON I.ACCODE = A.ACCODE"
            strSql += vbCrLf + " WHERE ISM.TRANDATE <= '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND ISM.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            If CmbAcname.Text <> "ALL" And CmbAcname.Text <> "" Then
                strSql += vbCrLf + " AND ISNULL(I.ACCODE,'') = '" & FilterAccode & "'"
            End If
            If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
                strSql += " AND ISNULL(I.ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
            End If
            If TranType <> "" Then strSql += vbCrLf + " AND ISM.TRANTYPE IN (" & TranType & ")"
            If Apptrantype <> "" Then strSql += vbCrLf + " AND ISM.TRANTYPE NOT IN (" & Apptrantype & ")"
            If Misctrantype <> "" Then strSql += vbCrLf + " AND ISM.TRANTYPE NOT IN (" & Misctrantype & ")"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND ISM.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkCategory <> "" Then strSql += vbCrLf + "  AND ISM.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            If ChkRepairDet.Checked = False Then strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO = I.BATCHNO AND TRANTYPE = 'RD')"
            strSql += vbCrLf + " GROUP BY I.ITEMID,I.SUBITEMID,ISM.TRANNO,I.REFNO,ISM.TRANDATE,ISM.BATCHNO,ISM.CATCODE,I.ACCODE,PINF.PNAME,ISM.TRANTYPE,ACNAME,I.SNO"

            If chkWithAlloy.Checked Then
                strSql += vbCrLf + " UNION ALL"
                strSql += vbCrLf + " SELECT I.ITEMID,I.SUBITEMID,AD.TRANNO,I.REFNO,AD.TRANDATE,AD.BATCHNO,I.CATCODE,CASE WHEN ISNULL(I.ACCODE,'') ='' THEN PINF.PNAME ELSE A.ACNAME END AS ACCODE,AD.TRANTYPE"
                strSql += vbCrLf + " ,0 PCS,SUM(WEIGHT) GRSWT,SUM(WEIGHT) NETWT,SUM(WEIGHT) PUREWT"
                strSql += vbCrLf + " ,NULL AS STNPCS,NULL AS STNWT,NULL AS DIAPCS,NULL AS DIAWT,NULL AS PREPCS,NULL AS PREWT"
                strSql += vbCrLf + " FROM " & cnStockDb & "..ALLOYDETAILS AS AD"
                strSql += vbCrLf + " LEFT JOIN " & cnStockDb & "..RECEIPT AS I ON AD.ISSSNO=I.SNO"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO AS CINF ON I.BATCHNO = CINF.BATCHNO"
                strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..PERSONALINFO AS PINF ON CINF.PSNO = PINF.SNO"
                strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD AS A ON I.ACCODE = A.ACCODE"
                strSql += vbCrLf + " WHERE AD.TRANDATE <= '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
                strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
                strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
                If CmbAcname.Text <> "ALL" And CmbAcname.Text <> "" Then
                    strSql += vbCrLf + " AND ISNULL(I.ACCODE,'') = '" & FilterAccode & "'"
                End If
                If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
                    strSql += " AND ISNULL(I.ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
                End If
                If TranType <> "" Then strSql += vbCrLf + " AND AD.TRANTYPE IN (" & TranType & ")"
                If Apptrantype <> "" Then strSql += vbCrLf + " AND AD.TRANTYPE NOT IN (" & Apptrantype & ")"
                If Misctrantype <> "" Then strSql += vbCrLf + " AND AD.TRANTYPE NOT IN (" & Misctrantype & ")"
                If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
                If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
                If ChkRepairDet.Checked = False Then strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO = I.BATCHNO AND TRANTYPE = 'RD')"
                strSql += vbCrLf + " GROUP BY ITEMID,SUBITEMID,AD.TRANNO,REFNO,AD.TRANDATE,AD.BATCHNO,CATCODE,I.ACCODE,PINF.PNAME ,AD.TRANTYPE,ACNAME"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()



            strSql = " SELECT DISTINCT BATCHNO,TRANTYPE INTO TEMPTABLEDB..TEMP" & systemId & "ORDREP_DEL "
            strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
            strSql += vbCrLf + " WHERE TRANTYPE IN ('OD','RD') AND ISNULL(CANCEL,'') = ''"
            If cmbstocktype.Text = "MANUFACTURING" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='M'"
            ElseIf cmbstocktype.Text = "EXEMPTED" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='E'"
            ElseIf cmbstocktype.Text = "TRADING" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'') NOT IN('M','E')"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            strSql = "SELECT DISTINCT BATCHNO FROM TEMPTABLEDB..TEMP" & systemId & "ORDREP_DEL WHERE TRANTYPE = 'RD'"
            Dim dtrepair As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dtrepair)
            If dtrepair.Rows.Count > 0 Then
                For Each ro As DataRow In dtrepair.Rows
                    strSql = "select top 1 ssno from TEMPTABLEDB..TEMP" & systemId & "ISS WHERE BATCHNO = '" & ro!BATCHNO.ToString & "'"
                    Dim sno As String = objGPack.GetSqlValue(strSql).ToString
                    Dim excesswt As Double = Val(objGPack.GetSqlValue("select grswt from TEMPTABLEDB..TEMP" & systemId & "ISS a where TRANTYPE ='RD' AND BATCHNO='" & ro!BATCHNO.ToString & "' AND GRSWT < 0 ").ToString)
                    strSql = "update a set a.grswt=a.grswt+" & excesswt & " from TEMPTABLEDB..TEMP" & systemId & "ISS a WHERE BATCHNO = '" & ro!BATCHNO.ToString & "' AND SSNO = '" & sno & "'"
                    cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
                Next
            End If

            strSql = " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK"
            strSql += " ("
            strSql += " PARTICULAR VARCHAR(500)"
            'strSql += vbCrLf + " ,TRANNO VARCHAR(20),TTRANDATE VARCHAR(10)"
            strSql += vbCrLf + " ,TRANNO VARCHAR(20),TTRANDATE SMALLDATETIME"
            strSql += vbCrLf + " ,TRANTYPE varchar(50)"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ,[RECEIPT PCS] INT,[RECEIPT " & bothWeight(0) & "] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[RECEIPT " & bothWeight(1) & "] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[RECEIPT STNPCS] INT,[RECEIPT STNWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[RECEIPT DIAPCS] INT,[RECEIPT DIAWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[RECEIPT PREPCS] INT,[RECEIPT PREWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[ISSUE PCS] INT,[ISSUE " & bothWeight(0) & "] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[ISSUE " & bothWeight(1) & "] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[ISSUE STNPCS] INT,[ISSUE STNWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[ISSUE DIAPCS] INT,[ISSUE DIAWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[ISSUE PREPCS] INT,[ISSUE PREWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[CLOSING PCS] INT,[CLOSING " & bothWeight(0) & "] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[CLOSING " & bothWeight(1) & "] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[CLOSING STNPCS] INT,[CLOSING STNWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[CLOSING DIAPCS] INT,[CLOSING DIAWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[CLOSING PREPCS] INT,[CLOSING PREWT] NUMERIC(15,3)"
            Else
                strSql += vbCrLf + " ,[RECEIPT PCS] INT,[RECEIPT " & Weight & "] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[RECEIPT STNPCS] INT,[RECEIPT STNWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[RECEIPT DIAPCS] INT,[RECEIPT DIAWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[RECEIPT PREPCS] INT,[RECEIPT PREWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[ISSUE PCS] INT,[ISSUE " & Weight & "] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[ISSUE STNPCS] INT,[ISSUE STNWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[ISSUE DIAPCS] INT,[ISSUE DIAWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[ISSUE PREPCS] INT,[ISSUE PREWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[CLOSING PCS] INT,[CLOSING " & Weight & "] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[CLOSING STNPCS] INT,[CLOSING STNWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[CLOSING DIAPCS] INT,[CLOSING DIAWT] NUMERIC(15,3)"
                strSql += vbCrLf + " ,[CLOSING PREPCS] INT,[CLOSING PREWT] NUMERIC(15,3)"
            End If
            strSql += vbCrLf + " ,CATNAME VARCHAR(100),METAL VARCHAR(100),GS VARCHAR(100)"
            strSql += vbCrLf + " ,TRANDATE SMALLDATETIME,ACNAME VARCHAR(200)"
            strSql += vbCrLf + " ,ITEMNAME VARCHAR(80)"
            strSql += vbCrLf + " ,RESULT INT,COLHEAD VARCHAR(3)"
            strSql += vbCrLf + " )"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

            strSql = " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " ("
            strSql += vbCrLf + " SEP VARCHAR(10),TRANNO VARCHAR(20),"
            strSql += vbCrLf + " TRANTYPE VARCHAR(50),"
            strSql += vbCrLf + " PCS Int,"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + bothWeight(0) & " NUMERIC(15,3),"
                strSql += vbCrLf + bothWeight(1) & " NUMERIC(15,3)"
            Else
                strSql += vbCrLf + "" & Weight & " NUMERIC(15,3)"
            End If
            strSql += vbCrLf + " ,STNPCS INT,STNWT NUMERIC(15,3),DIAPCS INT,DIAWT NUMERIC(15,3),PREPCS INT,PREWT NUMERIC(15,3)"
            strSql += vbCrLf + " ,CATNAME VARCHAR(100),METAL VARCHAR(100),GS VARCHAR(100)"
            strSql += vbCrLf + " ,TRANDATE SMALLDATETIME,ACNAME VARCHAR(200)"
            strSql += vbCrLf + " ,ITEMNAME VARCHAR(80)"
            strSql += vbCrLf + " )"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

            strSql = " /** INSERTING OPENING FROM OPENWEIGHT **/"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + "  	SELECT "
            strSql += vbCrLf + "  	CASE WHEN I.TRANTYPE = 'I' THEN 'ISS' ELSE 'REC' END SEP"
            strSql += vbCrLf + "  	,NULL TRANNO"
            strSql += vbCrLf + " ,'OP'TRANTYPE"
            strSql += vbCrLf + " ,SUM(CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.PCS ELSE 0 END) AS PCS"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + "    ,SUM(CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & bothWeight(0) & " ELSE 0 END) AS " & bothWeight(0) & ""
                strSql += vbCrLf + "    ,SUM(CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & bothWeight(1) & " ELSE 0 END) AS " & bothWeight(1) & ""
            Else
                strSql += vbCrLf + "    ,SUM(CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & Weight & " ELSE 0 END) AS " & Weight & ""
            End If
            strSql += vbCrLf + "    ,SUM(CASE WHEN CA.DIASTNTYPE='T' THEN I.PCS ELSE 0 END) AS STNPCS"
            strSql += vbCrLf + "    ,SUM(CASE WHEN CA.DIASTNTYPE='T' THEN I.GRSWT ELSE 0 END) AS STNWT"
            strSql += vbCrLf + "    ,SUM(CASE WHEN CA.DIASTNTYPE='D' THEN I.PCS ELSE 0 END) AS DIAPCS"
            strSql += vbCrLf + "    ,SUM(CASE WHEN CA.DIASTNTYPE='D' THEN I.GRSWT ELSE 0 END) AS DIAWT"
            strSql += vbCrLf + "    ,SUM(CASE WHEN CA.DIASTNTYPE='P' THEN I.PCS ELSE 0 END) AS PREPCS"
            strSql += vbCrLf + "    ,SUM(CASE WHEN CA.DIASTNTYPE='P' THEN I.GRSWT ELSE 0 END) AS PREWT"
            strSql += vbCrLf + "    ,CA.CATNAME,ME.METALNAME AS METAL"
            strSql += vbCrLf + "    ,CASE WHEN CA.GS11 = 'Y' THEN '" & Gs11Name & "' WHEN CA.GS12 = 'Y' THEN '" & Gs12Name & "' ELSE 'OTHER' END AS GS,NULL TRANDATE,NULL AS ACNAME,NULL ITEMNAME"
            strSql += vbCrLf + "  	FROM " & cnStockDb & "..OPENWEIGHT I "
            strSql += vbCrLf + "    LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID "
            strSql += vbCrLf + "    LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
            strSql += vbCrLf + "    INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE"
            strSql += vbCrLf + "    INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID"
            strSql += vbCrLf + "  	WHERE I.STOCKTYPE IN ('C')"
            strSql += vbCrLf + "    AND I.COMPANYID = '" & strCompanyId & "'"
            If CmbAcname.Text <> "ALL" And CmbAcname.Text <> "" Then
                strSql += vbCrLf + " AND ISNULL(I.ACCODE,'') = '" & FilterAccode & "'"
            End If
            If (ChkWithApproval.Checked = False And chkCmbTransation.Text = "ALL") Then
                strSql += vbCrLf + " AND ISNULL(I.APPROVAL,'')<>'A'"
            ElseIf chkCmbTransation.Text = "APPROVAL" Then
                strSql += vbCrLf + " AND ISNULL(I.APPROVAL,'')='A'"
            End If
            If chkCmbTransation.Text = "INTERNAL" Then
                strSql += vbCrLf + " AND ISNULL(I.APPROVAL,'')='I'"
            End If
            If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
                strSql += " AND ISNULL(I.ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
            End If
            'If TranType <> "" Then strSql += vbCrLf + " AND I.TRANTYPE IN (" & TranType & ")"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkMetal <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetal & ")))"
            If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            If cmbstocktype.Text = "MANUFACTURING" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='M'"
            ElseIf cmbstocktype.Text = "EXEMPTED" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'')='E'"
            ElseIf cmbstocktype.Text = "TRADING" Then
                strSql += vbCrLf + " AND ISNULL(I.STKTYPE,'') NOT IN('M','E')"
            End If
            strSql += vbCrLf + "    GROUP BY I.TRANTYPE,CA.CATNAME,CA.GS11,CA.GS12,ME.METALNAME"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            'InsGsOutStDt("O", "INSERTING OPENING FROM OUTSTANDING")
            'InsGsOutStDt("T", "INSERTING TRANSACTION FROM OUTSTANDING")

            If ChkWithOrRecpay.Checked Then InsGsOutStRecPay("T", "INSERTING TRANSACTION FROM OUTSTANDING ORDER INFO")
            If ChkRepairDet.Checked = True Then InsGsOutStRepairRecPay("T", "INSERTING TRANSACTION FROM OUTSTANDING REPAIR INFO")

            InsGsIssRecPrint("O", "I", False, "INSERTING OPENING FROM ISSUE WITHOUT ORDREP INFO")
            InsGsIssRecPrint("O", "R", False, "INSERTING OPENING FROM RECEIPT WITHOUT ORDREP INFO")
            InsGsIssRecPrint("O", "I", True, "INSERTING OPENING ORDREP INFO FROM ISSUE")
            InsGsIssRecPrint("O", "R", True, "INSERTING OPENING ORDREP INFO FROM RECEIPT")
            InsGsStonePrint("O", "I", "INSERTING OPENING FROM ISSUESTONE", Apptrantype)
            InsGsStonePrint("O", "R", "INSERTING OPENING FROM RECEIPTSTONE", Apptrantype)
            InsGsIssRecPrint("T", "I", False, "INSERTING TRANSACTION FROM ISSUE WITHOUT ORDREP INFO")
            InsGsIssRecPrint("T", "R", False, "INSERTING TRANSACTION FROM RECEIPT WITHOUT ORDREP INFO")
            InsGsIssRecPrint("T", "I", True, "INSERTING TRANSACTION ORDREP INFO FROM ISSUE")
            InsGsIssRecPrint("T", "R", True, "INSERTING TRANSACTION ORDREP INFO FROM RECEIPT")
            InsGsStonePrint("T", "I", "INSERTING TRANSACTION FROM ISSUESTONE", Apptrantype)
            InsGsStonePrint("T", "R", "INSERTING TRANSACTION FROM RECEIPTSTONE", Apptrantype)

        End If

        strSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK"
        strSql += " (PARTICULAR "
        strSql += vbCrLf + " ,TRANNO,TTRANDATE"

        If rdbBothwt.Checked Then
            strSql += vbCrLf + " ,[RECEIPT PCS],[RECEIPT " & bothWeight(0) & "] "
            strSql += vbCrLf + " ,[RECEIPT " & bothWeight(1) & "] "
            strSql += vbCrLf + " ,[RECEIPT STNPCS] ,[RECEIPT STNWT] "
            strSql += vbCrLf + " ,[RECEIPT DIAPCS] ,[RECEIPT DIAWT] "
            strSql += vbCrLf + " ,[RECEIPT PREPCS] ,[RECEIPT PREWT] "
            strSql += vbCrLf + " ,[ISSUE PCS] ,[ISSUE " & bothWeight(0) & "] "
            strSql += vbCrLf + " ,[ISSUE " & bothWeight(1) & "] "
            strSql += vbCrLf + " ,[ISSUE STNPCS] ,[ISSUE STNWT] "
            strSql += vbCrLf + " ,[ISSUE DIAPCS] ,[ISSUE DIAWT] "
            strSql += vbCrLf + " ,[ISSUE PREPCS] ,[ISSUE PREWT] "
            strSql += vbCrLf + " ,[CLOSING PCS] ,[CLOSING " & bothWeight(0) & "] "
            strSql += vbCrLf + " ,[CLOSING " & bothWeight(1) & "] "
            strSql += vbCrLf + " ,[CLOSING STNPCS] ,[CLOSING STNWT] "
            strSql += vbCrLf + " ,[CLOSING DIAPCS] ,[CLOSING DIAWT] "
            strSql += vbCrLf + " ,[CLOSING PREPCS] ,[CLOSING PREWT] "
        Else
            strSql += vbCrLf + " ,[RECEIPT PCS],[RECEIPT " & Weight & "] "
            strSql += vbCrLf + " ,[RECEIPT STNPCS] ,[RECEIPT STNWT] "
            strSql += vbCrLf + " ,[RECEIPT DIAPCS] ,[RECEIPT DIAWT] "
            strSql += vbCrLf + " ,[RECEIPT PREPCS] ,[RECEIPT PREWT] "
            strSql += vbCrLf + " ,[ISSUE PCS] ,[ISSUE " & Weight & "] "
            strSql += vbCrLf + " ,[ISSUE STNPCS] ,[ISSUE STNWT] "
            strSql += vbCrLf + " ,[ISSUE DIAPCS] ,[ISSUE DIAWT] "
            strSql += vbCrLf + " ,[ISSUE PREPCS] ,[ISSUE PREWT] "
            strSql += vbCrLf + " ,[CLOSING PCS] ,[CLOSING " & Weight & "] "
            strSql += vbCrLf + " ,[CLOSING STNPCS] ,[CLOSING STNWT] "
            strSql += vbCrLf + " ,[CLOSING DIAPCS] ,[CLOSING DIAWT] "
            strSql += vbCrLf + " ,[CLOSING PREPCS] ,[CLOSING PREWT] "
        End If

        If ChkWithTrantype.Checked Then strSql += ",TRANTYPE"
        If chkSpecificFormat.Checked = False Then strSql += ",REMARK,REFNO"

        strSql += vbCrLf + " ,CATNAME,METAL,GS,TRANDATE,ACNAME,ITEMNAME,RESULT,COLHEAD)"
        strSql += vbCrLf + " SELECT * FROM "
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT"
        strSql += vbCrLf + " CONVERT(VARCHAR(500),NULL) PARTICULAR"
        strSql += vbCrLf + " ,TRANNO"
        'strSql += vbCrLf + " ,CONVERT(VARCHAR,TRANDATE,103)TTRANDATE"
        strSql += vbCrLf + " ,TRANDATE AS TTRANDATE"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PCS ELSE 0 END) AS [RECEIPT PCS]"
        If rdbBothwt.Checked Then
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN " & bothWeight(0) & " ELSE 0 END) AS [RECEIPT " & bothWeight(0) & "]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN " & bothWeight(1) & " ELSE 0 END) AS [RECEIPT " & bothWeight(1) & "]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNPCS ELSE 0 END) AS [RECEIPT STNPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNWT ELSE 0 END) AS [RECEIPT STNWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN DIAPCS ELSE 0 END) AS [RECEIPT DIAPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN DIAWT ELSE 0 END) AS [RECEIPT DIAWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PREPCS ELSE 0 END) AS [RECEIPT PREPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PREWT ELSE 0 END) AS [RECEIPT PREWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PCS ELSE 0 END) AS [ISSUE PCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN " & bothWeight(0) & " ELSE 0 END) AS [ISSUE " & bothWeight(0) & "]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN " & bothWeight(1) & " ELSE 0 END) AS [ISSUE " & bothWeight(1) & "]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNPCS ELSE 0 END) AS [ISSUE STNPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNWT ELSE 0 END) AS [ISSUE STNWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN DIAPCS ELSE 0 END) AS [ISSUE DIAPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN DIAWT ELSE 0 END) AS [ISSUE DIAWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PREPCS ELSE 0 END) AS [ISSUE PREPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PREWT ELSE 0 END) AS [ISSUE PREWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*PCS ELSE " & bothWeight(0) & " END) AS [CLOSING PCS]"
            'strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*PCS ELSE " & bothWeight(1) & " END) AS [CLOSING PCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*" & bothWeight(0) & " ELSE " & bothWeight(0) & " END) AS [CLOSING " & bothWeight(0) & "]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*" & bothWeight(1) & " ELSE " & bothWeight(1) & " END) AS [CLOSING " & bothWeight(1) & "]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*STNPCS ELSE 0 END) AS [CLOSING STNPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*STNWT ELSE 0 END) AS [CLOSING STNWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*DIAPCS ELSE 0 END) AS [CLOSING DIAPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*DIAWT ELSE 0 END) AS [CLOSING DIAWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*PREPCS ELSE 0 END) AS [CLOSING PREPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*PREWT ELSE 0 END) AS [CLOSING PREWT]"
        Else
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN " & Weight & " ELSE 0 END) AS [RECEIPT " & Weight & "]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNPCS ELSE 0 END) AS [RECEIPT STNPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNWT ELSE 0 END) AS [RECEIPT STNWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN DIAPCS ELSE 0 END) AS [RECEIPT DIAPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN DIAWT ELSE 0 END) AS [RECEIPT DIAWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PREPCS ELSE 0 END) AS [RECEIPT PREPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PREWT ELSE 0 END) AS [RECEIPT PREWT]"

            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PCS ELSE 0 END) AS [ISSUE PCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN " & Weight & " ELSE 0 END) AS [ISSUE " & Weight & "]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNPCS ELSE 0 END) AS [ISSUE STNPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNWT ELSE 0 END) AS [ISSUE STNWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN DIAPCS ELSE 0 END) AS [ISSUE DIAPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN DIAWT ELSE 0 END) AS [ISSUE DIAWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PREPCS ELSE 0 END) AS [ISSUE PREPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PREWT ELSE 0 END) AS [ISSUE PREWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*PCS ELSE " & Weight & " END) AS [CLOSING PCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*" & Weight & " ELSE " & Weight & " END) AS [CLOSING " & Weight & "]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*STNPCS ELSE STNPCS END) AS [CLOSING STNPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*STNWT ELSE STNWT END) AS [CLOSING STNWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*DIAPCS ELSE DIAPCS END) AS [CLOSING DIAPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*DIAWT ELSE DIAWT END) AS [CLOSING DIAWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*PREPCS ELSE PREPCS END) AS [CLOSING PREPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*PREWT ELSE PREWT END) AS [CLOSING PREWT]"
        End If

        If ChkWithTrantype.Checked Then strSql += ",TRANTYPE"
        If chkSpecificFormat.Checked = False Then strSql += ",REMARK,REFNO"

        If chkCategorywise.Checked = False Then
            strSql += vbCrLf + " ," & IIf(rbtPcs.Checked, "CATNAME", "NULL") & " AS CATNAME"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(100),NULL) AS METAL"
            strSql += vbCrLf + " ," & IIf(rbtPcs.Checked, "CONVERT(vARCHAR,NULL)AS GS", "GS") & ",TRANDATE,ACNAME"
            strSql += vbCrLf + " ," & IIf(rbtPcs.Checked, "ITEMNAME", "NULL") & " AS ITEMNAME,1 AS RESULT,NULL COLHEAD"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " GROUP BY TRANNO,TRANDATE,ACNAME,GS"
            If ChkWithTrantype.Checked Then strSql += ",TRANTYPE"
            If chkSpecificFormat.Checked = False Then strSql += ",REMARK,REFNO"
            If rbtPcs.Checked Then
                strSql += vbCrLf + ",CATNAME,GS,ITEMNAME"
            End If
        ElseIf chkCategorywise.Checked Then
            strSql += vbCrLf + " , CATNAME"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(100),NULL) AS METAL"
            strSql += vbCrLf + " ,GS,TRANDATE,ACNAME"
            strSql += vbCrLf + " ,NULL ITEMNAME,1 AS RESULT,NULL COLHEAD"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " GROUP BY TRANNO,TRANDATE,ACNAME,GS"
            If ChkWithTrantype.Checked Then strSql += ",TRANTYPE"
            If chkSpecificFormat.Checked = False Then strSql += ",REMARK,REFNO"
            strSql += vbCrLf + ",CATNAME,GS--,ITEMNAME"
        End If

        strSql += vbCrLf + " )X"
        If rbtBoth.Checked Then
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " WHERE NOT ([RECEIPT " & bothWeight(0) & "] = 0 AND [RECEIPT " & bothWeight(1) & "] = 0  AND [ISSUE " & bothWeight(0) & "] = 0 AND [ISSUE " & bothWeight(1) & "] = 0 AND [RECEIPT PCS] = 0 AND [ISSUE PCS] = 0)"
            Else
                strSql += vbCrLf + " WHERE NOT ([RECEIPT " & Weight & "] = 0 AND [ISSUE " & Weight & "] = 0 AND [RECEIPT PCS] = 0 AND [ISSUE PCS] = 0)"
            End If
        ElseIf rbtWeight.Checked Then
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " WHERE NOT ([RECEIPT " & bothWeight(0) & "] = 0 AND [RECEIPT " & bothWeight(1) & "] = 0"
                strSql += vbCrLf + " AND [ISSUE " & bothWeight(0) & "] = 0 AND [ISSUE " & bothWeight(1) & "] = 0)"
            Else
                strSql += vbCrLf + " WHERE NOT ([RECEIPT " & Weight & "] = 0 AND [ISSUE " & Weight & "] = 0)"
            End If
        Else
            strSql += vbCrLf + " WHERE NOT ([RECEIPT PCS] = 0 AND [ISSUE PCS] = 0)"
            ''PCS
        End If
        strSql += vbCrLf + "  ORDER BY GS,CATNAME,ITEMNAME,TRANDATE"
        If chkOrderbyTranno.Checked And chkOrderbyTranno.Visible Then
            strSql += vbCrLf + " ,TRANNO"
        Else
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ,[RECEIPT " & bothWeight(0) & "] DESC,[RECEIPT PCS] DESC,TRANNO"
            Else
                strSql += vbCrLf + " ,[RECEIPT " & Weight & "] DESC,[RECEIPT PCS] DESC,TRANNO"
            End If
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        If chkCategorywise.Checked = False Then
            strSql = "  IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "GSSTOCK)>0"
            strSql += vbCrLf + "   BEGIN"
            strSql += vbCrLf + "   INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK(METAL,GS,PARTICULAR,RESULT,COLHEAD)"
            strSql += vbCrLf + "   SELECT DISTINCT METAL,GS,' '+GS,0 RESULT,'T'COLHEAD FROM TEMPTABLEDB..TEMP" & systemId & "GSSTOCK WHERE RESULT = 1  "
            strSql += vbCrLf + "   AND (ISNULL(METAL,'') <> '' or ISNULL(GS,'') <> '')"
            If rbtTranno.Checked Then
                strSql += vbCrLf + "   INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK(METAL,GS,TRANDATE,CATNAME,ITEMNAME,PARTICULAR"
                If rdbBothwt.Checked Then
                    strSql += vbCrLf + " ,[RECEIPT " & bothWeight(0) & "]"
                    strSql += vbCrLf + " ,[RECEIPT " & bothWeight(1) & "]"
                    strSql += vbCrLf + " ,[RECEIPT STNWT],[RECEIPT DIAWT],[RECEIPT PREWT]"
                    strSql += vbCrLf + " ,[ISSUE " & bothWeight(0) & "]"
                    strSql += vbCrLf + " ,[ISSUE " & bothWeight(1) & "]"
                    strSql += vbCrLf + " ,[ISSUE STNWT],[ISSUE DIAWT],[ISSUE PREWT]"
                Else
                    strSql += vbCrLf + " ,[RECEIPT " & Weight & "]"
                    strSql += vbCrLf + " ,[RECEIPT STNWT],[RECEIPT DIAWT],[RECEIPT PREWT]"
                    strSql += vbCrLf + " ,[ISSUE " & Weight & "]"
                    strSql += vbCrLf + " ,[ISSUE STNWT],[ISSUE DIAWT],[ISSUE PREWT]"
                End If
                strSql += vbCrLf + " ,[RECEIPT PCS]"
                strSql += vbCrLf + " ,[RECEIPT STNPCS],[RECEIPT DIAPCS],[RECEIPT PREPCS]"
                strSql += vbCrLf + " ,[ISSUE PCS]"
                strSql += vbCrLf + " ,[ISSUE STNPCS],[ISSUE DIAPCS],[ISSUE PREPCS]"
                If ChkWithTrantype.Checked Then strSql += ",TRANTYPE"
                strSql += vbCrLf + " ,RESULT,COLHEAD)"
                strSql += vbCrLf + " SELECT METAL,GS,TRANDATE,CATNAME,ITEMNAME,'   '+'SUB TOTAL'"
                If rdbBothwt.Checked Then
                    strSql += vbCrLf + " ,SUM([RECEIPT " & bothWeight(0) & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT " & bothWeight(1) & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT STNWT]),SUM([RECEIPT DIAWT]),SUM([RECEIPT PREWT])"
                    strSql += vbCrLf + " ,SUM([ISSUE " & bothWeight(0) & "])"
                    strSql += vbCrLf + " ,SUM([ISSUE " & bothWeight(1) & "])"
                    strSql += vbCrLf + " ,SUM([ISSUE STNWT]),SUM([ISSUE DIAWT]),SUM([ISSUE PREWT])"
                Else
                    strSql += vbCrLf + " ,SUM([RECEIPT " & Weight & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT STNWT]),SUM([RECEIPT DIAWT]),SUM([RECEIPT PREWT])"
                    strSql += vbCrLf + " ,SUM([ISSUE " & Weight & "])"
                    strSql += vbCrLf + " ,SUM([ISSUE STNWT]),SUM([ISSUE DIAWT]),SUM([ISSUE PREWT])"
                End If
                strSql += vbCrLf + " ,SUM([RECEIPT PCS])"
                strSql += vbCrLf + " ,SUM([RECEIPT STNPCS]),SUM([RECEIPT DIAPCS]),SUM([RECEIPT PREPCS])"
                strSql += vbCrLf + " ,SUM([ISSUE PCS])"
                strSql += vbCrLf + " ,SUM([ISSUE STNPCS]),SUM([ISSUE DIAPCS]),SUM([ISSUE PREPCS])"
                If ChkWithTrantype.Checked Then strSql += ",TRANTYPE"
                strSql += vbCrLf + " ,4,'S2'COLHEAD"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 AND TRANDATE IS NOT NULL GROUP BY METAL,GS,TRANDATE,CATNAME,ITEMNAME"
                If ChkWithTrantype.Checked Then strSql += ",TRANTYPE"
            End If
            strSql += vbCrLf + "  "
            If rbtPcs.Checked = False Then
                strSql += vbCrLf + "   INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK(METAL,GS,TRANDATE,PARTICULAR"
                If rdbBothwt.Checked Then
                    strSql += vbCrLf + " ,[RECEIPT " & bothWeight(0) & "]"
                    strSql += vbCrLf + " ,[RECEIPT " & bothWeight(1) & "]"
                    strSql += vbCrLf + " ,[RECEIPT STNWT],[RECEIPT DIAWT],[RECEIPT PREWT]"
                    strSql += vbCrLf + " ,[ISSUE " & bothWeight(0) & "]"
                    strSql += vbCrLf + " ,[ISSUE " & bothWeight(1) & "]"
                    strSql += vbCrLf + " ,[ISSUE STNWT],[ISSUE DIAWT],[ISSUE PREWT]"
                Else
                    strSql += vbCrLf + " ,[RECEIPT " & Weight & "]"
                    strSql += vbCrLf + " ,[RECEIPT STNWT],[RECEIPT DIAWT],[RECEIPT PREWT]"
                    strSql += vbCrLf + " ,[ISSUE " & Weight & "]"
                    strSql += vbCrLf + " ,[ISSUE STNWT],[ISSUE DIAWT],[ISSUE PREWT]"
                End If
                strSql += vbCrLf + " ,[RECEIPT PCS]"
                strSql += vbCrLf + " ,[RECEIPT STNPCS],[RECEIPT DIAPCS],[RECEIPT PREPCS]"
                strSql += vbCrLf + " ,[ISSUE PCS]"
                strSql += vbCrLf + " ,[ISSUE STNPCS],[ISSUE DIAPCS],[ISSUE PREPCS]"
                If ChkWithTrantype.Checked Then strSql += ",TRANTYPE"
                strSql += vbCrLf + " ,RESULT,COLHEAD)"
                strSql += vbCrLf + " SELECT METAL,GS,'" & _MaxDate.ToString("yyyy-MM-dd") & "','  '+GS+'=>TOTAL'"
                If rdbBothwt.Checked Then
                    strSql += vbCrLf + " ,SUM([RECEIPT " & bothWeight(0) & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT " & bothWeight(1) & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT STNWT]),SUM([RECEIPT DIAWT]),SUM([RECEIPT PREWT])"
                    strSql += vbCrLf + " ,SUM([ISSUE " & bothWeight(0) & "])"
                    strSql += vbCrLf + " ,SUM([ISSUE " & bothWeight(1) & "])"
                    strSql += vbCrLf + " ,SUM([ISSUE STNWT]),SUM([ISSUE DIAWT]),SUM([ISSUE PREWT])"
                Else
                    strSql += vbCrLf + " ,SUM([RECEIPT " & Weight & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT STNWT]),SUM([RECEIPT DIAWT]),SUM([RECEIPT PREWT])"
                    strSql += vbCrLf + " ,SUM([ISSUE " & Weight & "])"
                    strSql += vbCrLf + " ,SUM([ISSUE STNWT]),SUM([ISSUE DIAWT]),SUM([ISSUE PREWT])"
                End If
                strSql += vbCrLf + " ,SUM([RECEIPT PCS])"
                strSql += vbCrLf + " ,SUM([RECEIPT STNPCS]),SUM([RECEIPT DIAPCS]),SUM([RECEIPT PREPCS])"
                strSql += vbCrLf + " ,SUM([ISSUE PCS])"
                strSql += vbCrLf + " ,SUM([ISSUE STNPCS]),SUM([ISSUE DIAPCS]),SUM([ISSUE PREPCS])"
                If ChkWithTrantype.Checked Then strSql += ",TRANTYPE"
                strSql += vbCrLf + " ,3,'S'COLHEAD"
                strSql += vbCrLf + "   FROM TEMPTABLEDB..TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 AND TRANDATE IS NOT NULL GROUP BY METAL,GS"
                If ChkWithTrantype.Checked Then strSql += ",TRANTYPE"
                strSql += vbCrLf + "  "
            Else
                strSql += vbCrLf + "   INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK(METAL,GS,CATNAME,PARTICULAR,RESULT,COLHEAD)"
                strSql += vbCrLf + "   SELECT DISTINCT METAL,GS,CATNAME,CATNAME,0 RESULT,'T'COLHEAD FROM TEMPTABLEDB..TEMP" & systemId & "GSSTOCK WHERE RESULT = 1"
                strSql += vbCrLf + "   INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK(METAL,GS,CATNAME,ITEMNAME,TRANDATE,PARTICULAR"
                If rdbBothwt.Checked Then
                    strSql += vbCrLf + " ,[RECEIPT " & bothWeight(0) & "]"
                    strSql += vbCrLf + " ,[RECEIPT " & bothWeight(1) & "]"
                    strSql += vbCrLf + " ,[RECEIPT STNWT],[RECEIPT DIAWT],[RECEIPT PREWT]"
                    strSql += vbCrLf + " ,[ISSUE " & bothWeight(0) & "]"
                    strSql += vbCrLf + " ,[ISSUE " & bothWeight(1) & "]"
                    strSql += vbCrLf + " ,[ISSUE STNWT],[ISSUE DIAWT],[ISSUE PREWT]"
                Else
                    strSql += vbCrLf + " ,[RECEIPT " & Weight & "]"
                    strSql += vbCrLf + " ,[RECEIPT STNWT],[RECEIPT DIAWT],[RECEIPT PREWT]"
                    strSql += vbCrLf + " ,[ISSUE " & Weight & "]"
                    strSql += vbCrLf + " ,[ISSUE STNWT],[ISSUE DIAWT],[ISSUE PREWT]"
                End If
                strSql += vbCrLf + " ,[RECEIPT PCS]"
                strSql += vbCrLf + " ,[RECEIPT STNPCS],[RECEIPT DIAPCS],[RECEIPT PREPCS]"
                strSql += vbCrLf + " ,[ISSUE PCS]"
                strSql += vbCrLf + " ,[ISSUE STNPCS],[ISSUE DIAPCS],[ISSUE PREPCS]"
                If ChkWithTrantype.Checked Then strSql += ",TRANTYPE"
                strSql += vbCrLf + " ,RESULT,COLHEAD)"
                strSql += vbCrLf + "   SELECT METAL,GS,CATNAME,ITEMNAME,'" & _MaxDate.ToString("yyyy-MM-dd") & "',' '+ITEMNAME+' TOT'"
                If rdbBothwt.Checked Then
                    strSql += vbCrLf + " ,SUM([RECEIPT " & bothWeight(0) & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT " & bothWeight(1) & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT STNWT]),SUM([RECEIPT DIAWT]),SUM([RECEIPT PREWT])"
                    strSql += vbCrLf + " ,SUM([ISSUE " & bothWeight(0) & "])"
                    strSql += vbCrLf + " ,SUM([ISSUE " & bothWeight(1) & "])"
                    strSql += vbCrLf + " ,SUM([ISSUE STNWT]),SUM([ISSUE DIAWT]),SUM([ISSUE PREWT])"
                Else
                    strSql += vbCrLf + " ,SUM([RECEIPT " & Weight & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT STNWT]),SUM([RECEIPT DIAWT]),SUM([RECEIPT PREWT])"
                    strSql += vbCrLf + " ,SUM([ISSUE " & Weight & "])"
                    strSql += vbCrLf + " ,SUM([ISSUE STNWT]),SUM([ISSUE DIAWT]),SUM([ISSUE PREWT])"
                End If
                strSql += vbCrLf + " ,SUM([RECEIPT PCS])"
                strSql += vbCrLf + " ,SUM([RECEIPT STNPCS]),SUM([RECEIPT DIAPCS]),SUM([RECEIPT PREPCS])"
                strSql += vbCrLf + " ,SUM([ISSUE PCS])"
                strSql += vbCrLf + " ,SUM([ISSUE STNPCS]),SUM([ISSUE DIAPCS]),SUM([ISSUE PREPCS])"
                If ChkWithTrantype.Checked Then strSql += ",TRANTYPE"
                strSql += vbCrLf + " ,3,'S1'COLHEAD"
                strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 AND TRANDATE IS NOT NULL GROUP BY METAL,GS,CATNAME,ITEMNAME"
                If ChkWithTrantype.Checked Then strSql += ",TRANTYPE"
            End If
            strSql += vbCrLf + "   END"
        ElseIf chkCategorywise.Checked = True Then
            strSql = " IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP" & systemId & "GSSTOCK)>0"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK(METAL,GS,PARTICULAR,RESULT,COLHEAD)"
            strSql += vbCrLf + " SELECT DISTINCT METAL,GS,' '+GS,0 RESULT,'T'COLHEAD FROM TEMPTABLEDB..TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 "
            strSql += vbCrLf + " AND (ISNULL(METAL,'') <> '' or ISNULL(GS,'') <> '')"
            If rbtTranno.Checked Then
                strSql += vbCrLf + "   INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK(METAL,GS,TRANDATE,CATNAME,PARTICULAR"
                If rdbBothwt.Checked Then
                    strSql += vbCrLf + " ,[RECEIPT " & bothWeight(0) & "]"
                    strSql += vbCrLf + " ,[RECEIPT " & bothWeight(1) & "]"
                    strSql += vbCrLf + " ,[RECEIPT STNWT],[RECEIPT DIAWT],[RECEIPT PREWT]"
                    strSql += vbCrLf + " ,[ISSUE " & bothWeight(0) & "]"
                    strSql += vbCrLf + " ,[ISSUE " & bothWeight(1) & "]"
                    strSql += vbCrLf + " ,[ISSUE STNWT],[ISSUE DIAWT],[ISSUE PREWT]"
                Else
                    strSql += vbCrLf + " ,[RECEIPT " & Weight & "]"
                    strSql += vbCrLf + " ,[RECEIPT STNWT],[RECEIPT DIAWT],[RECEIPT PREWT]"
                    strSql += vbCrLf + " ,[ISSUE " & Weight & "]"
                    strSql += vbCrLf + " ,[ISSUE STNWT],[ISSUE DIAWT],[ISSUE PREWT]"
                End If
                strSql += vbCrLf + " ,[RECEIPT PCS]"
                strSql += vbCrLf + " ,[RECEIPT STNPCS],[RECEIPT DIAPCS],[RECEIPT PREPCS]"
                strSql += vbCrLf + " ,[ISSUE PCS]"
                strSql += vbCrLf + " ,[ISSUE STNPCS],[ISSUE DIAPCS],[ISSUE PREPCS]"
                'If ChkWithTrantype.Checked Then strSql += ",TRANTYPE"
                strSql += vbCrLf + " ,RESULT,COLHEAD)"
                strSql += vbCrLf + "   SELECT METAL,GS,TRANDATE,CATNAME,'   '+'SUB TOTAL'"
                If rdbBothwt.Checked Then
                    strSql += vbCrLf + " ,SUM([RECEIPT " & bothWeight(0) & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT " & bothWeight(1) & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT STNWT]),SUM([RECEIPT DIAWT]),SUM([RECEIPT PREWT])"
                    strSql += vbCrLf + " ,SUM([ISSUE " & bothWeight(0) & "])"
                    strSql += vbCrLf + " ,SUM([ISSUE " & bothWeight(1) & "])"
                    strSql += vbCrLf + " ,SUM([ISSUE STNWT]),SUM([ISSUE DIAWT]),SUM([ISSUE PREWT])"
                    'strSql += vbCrLf + " ,SUM([CLOSING " & bothWeight(0) & "])"
                    'strSql += vbCrLf + " ,SUM([CLOSING " & bothWeight(1) & "])"
                Else
                    strSql += vbCrLf + " ,SUM([RECEIPT " & Weight & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT STNWT]),SUM([RECEIPT DIAWT]),SUM([RECEIPT PREWT])"
                    strSql += vbCrLf + " ,SUM([ISSUE " & Weight & "])" ',SUM([CLOSING " & Weight & "])"
                    strSql += vbCrLf + " ,SUM([ISSUE STNWT]),SUM([ISSUE DIAWT]),SUM([ISSUE PREWT])"
                End If
                strSql += vbCrLf + " ,SUM([RECEIPT PCS])"
                strSql += vbCrLf + " ,SUM([RECEIPT STNPCS]),SUM([RECEIPT DIAPCS]),SUM([RECEIPT PREPCS])"
                strSql += vbCrLf + " ,SUM([ISSUE PCS])"
                strSql += vbCrLf + " ,SUM([ISSUE STNPCS]),SUM([ISSUE DIAPCS]),SUM([ISSUE PREPCS])"
                ' If ChkWithTrantype.Checked Then strSql += ",TRANTYPE"
                strSql += vbCrLf + " ,4,'S2'COLHEAD"
                strSql += vbCrLf + "   FROM TEMPTABLEDB..TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 AND TRANDATE IS NOT NULL GROUP BY CATNAME,METAL,GS,TRANDATE"
                ' If ChkWithTrantype.Checked Then strSql += ",TRANTYPE"

            End If
            strSql += vbCrLf + "  "

            strSql += vbCrLf + "   INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK(METAL,GS,CATNAME,PARTICULAR,RESULT,COLHEAD)"
            strSql += vbCrLf + "   SELECT DISTINCT METAL,GS,CATNAME,CATNAME,0 RESULT,'T'COLHEAD FROM TEMPTABLEDB..TEMP" & systemId & "GSSTOCK WHERE RESULT = 1"
            strSql += vbCrLf + "   INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK(METAL,GS,CATNAME,PARTICULAR,TRANDATE"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ,[RECEIPT " & bothWeight(0) & "]"
                strSql += vbCrLf + " ,[RECEIPT " & bothWeight(1) & "]"
                strSql += vbCrLf + " ,[RECEIPT STNWT],[RECEIPT DIAWT],[RECEIPT PREWT]"
                strSql += vbCrLf + " ,[ISSUE " & bothWeight(0) & "]"
                strSql += vbCrLf + " ,[ISSUE " & bothWeight(1) & "]"
                strSql += vbCrLf + " ,[ISSUE STNWT],[ISSUE DIAWT],[ISSUE PREWT]"
            Else
                strSql += vbCrLf + " ,[RECEIPT " & Weight & "]"
                strSql += vbCrLf + " ,[RECEIPT STNWT],[RECEIPT DIAWT],[RECEIPT PREWT]"
                strSql += vbCrLf + " ,[ISSUE " & Weight & "]"
                strSql += vbCrLf + " ,[ISSUE STNWT],[ISSUE DIAWT],[ISSUE PREWT]"
            End If
            strSql += vbCrLf + " ,[RECEIPT PCS]"
            strSql += vbCrLf + " ,[RECEIPT STNPCS],[RECEIPT DIAPCS],[RECEIPT PREPCS]"
            strSql += vbCrLf + " ,[ISSUE PCS]"
            strSql += vbCrLf + " ,[ISSUE STNPCS],[ISSUE DIAPCS],[ISSUE PREPCS]"
            'If ChkWithTrantype.Checked Then strSql += ",TRANTYPE"
            strSql += vbCrLf + " ,RESULT,COLHEAD)"
            strSql += vbCrLf + "   SELECT METAL,GS,CATNAME,' '+CATNAME+' TOT','" & _MaxDate.ToString("yyyy-MM-dd") & "'" 'ITEMNAME,'" & _MaxDate.ToString("yyyy-MM-dd") & "',' '+ITEMNAME+' TOT'"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ,SUM([RECEIPT " & bothWeight(0) & "])"
                strSql += vbCrLf + " ,SUM([RECEIPT " & bothWeight(1) & "])"
                strSql += vbCrLf + " ,SUM([RECEIPT STNWT]),SUM([RECEIPT DIAWT]),SUM([RECEIPT PREWT])"
                strSql += vbCrLf + " ,SUM([ISSUE " & bothWeight(0) & "])"
                strSql += vbCrLf + " ,SUM([ISSUE " & bothWeight(1) & "])"
                strSql += vbCrLf + " ,SUM([ISSUE STNWT]),SUM([ISSUE DIAWT]),SUM([ISSUE PREWT])"
                'strSql += vbCrLf + " ,SUM([CLOSING " & bothWeight(0) & "])"
                'strSql += vbCrLf + " ,SUM([CLOSING " & bothWeight(1) & "])"
            Else
                strSql += vbCrLf + " ,SUM([RECEIPT " & Weight & "])"
                strSql += vbCrLf + " ,SUM([RECEIPT STNWT]),SUM([RECEIPT DIAWT]),SUM([RECEIPT PREWT])"
                strSql += vbCrLf + " ,SUM([ISSUE " & Weight & "])" ',SUM([CLOSING " & Weight & "])"
                strSql += vbCrLf + " ,SUM([ISSUE STNWT]),SUM([ISSUE DIAWT]),SUM([ISSUE PREWT])"
            End If

            strSql += vbCrLf + " ,SUM([RECEIPT PCS])"
            strSql += vbCrLf + " ,SUM([RECEIPT STNPCS]),SUM([RECEIPT DIAPCS]),SUM([RECEIPT PREPCS])"
            strSql += vbCrLf + " ,SUM([ISSUE PCS])"
            strSql += vbCrLf + " ,SUM([ISSUE STNPCS]),SUM([ISSUE DIAPCS]),SUM([ISSUE PREPCS])"
            'If ChkWithTrantype.Checked Then strSql += ",TRANTYPE"
            strSql += vbCrLf + " ,3,'S1'COLHEAD"
            strSql += vbCrLf + "   FROM TEMPTABLEDB..TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 AND TRANDATE IS NOT NULL GROUP BY METAL,GS,CATNAME"
            'If ChkWithTrantype.Checked Then strSql += ",TRANTYPE"
            strSql += vbCrLf + "   END"
        End If

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET PARTICULAR = '   '+'OPENING..' WHERE RESULT = 1 AND TRANDATE IS NULL"
        'strSql += vbCrLf + " UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [RECEIPT PCS] = NULL,[ISSUE PCS] = NULL  WHERE RESULT = 1 AND TRANDATE IS NULL"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET PARTICULAR = '   '+ACNAME WHERE RESULT = 1 AND TRANDATE IS NOT NULL"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [RECEIPT PCS] = NULL WHERE [RECEIPT PCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE PCS] = NULL WHERE [ISSUE PCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [CLOSING PCS] = NULL WHERE [CLOSING PCS] = 0"

        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [RECEIPT STNPCS] = NULL WHERE [RECEIPT STNPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE STNPCS] = NULL WHERE [ISSUE STNPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [CLOSING STNPCS] = NULL WHERE [CLOSING STNPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [RECEIPT DIAPCS] = NULL WHERE [RECEIPT DIAPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE DIAPCS] = NULL WHERE [ISSUE DIAPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [CLOSING DIAPCS] = NULL WHERE [CLOSING DIAPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [RECEIPT PREPCS] = NULL WHERE [RECEIPT PREPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE PREPCS] = NULL WHERE [ISSUE PREPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [CLOSING PREPCS] = NULL WHERE [CLOSING PREPCS] = 0"

        If rdbBothwt.Checked Then
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [RECEIPT " & bothWeight(0) & "] = NULL WHERE [RECEIPT " & bothWeight(0) & "] = 0"
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [RECEIPT " & bothWeight(1) & "] = NULL WHERE [RECEIPT " & bothWeight(1) & "] = 0"
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE " & bothWeight(0) & "] = NULL WHERE [ISSUE " & bothWeight(0) & "] = 0"
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE " & bothWeight(1) & "] = NULL WHERE [ISSUE " & bothWeight(1) & "] = 0"
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [CLOSING " & bothWeight(0) & "] = NULL WHERE [CLOSING " & bothWeight(0) & "] = 0"
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [CLOSING " & bothWeight(1) & "] = NULL WHERE [CLOSING " & bothWeight(1) & "] = 0"
        Else
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [RECEIPT " & Weight & "] = NULL WHERE [RECEIPT " & Weight & "] = 0"
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE " & Weight & "] = NULL WHERE [ISSUE " & Weight & "] = 0"
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [CLOSING " & Weight & "] = NULL WHERE [CLOSING " & Weight & "] = 0"
        End If
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [RECEIPT STNWT] = NULL WHERE [RECEIPT STNWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE STNWT] = NULL WHERE [ISSUE STNWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [CLOSING STNWT] = NULL WHERE [CLOSING STNWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [RECEIPT DIAWT] = NULL WHERE [RECEIPT DIAWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE DIAWT] = NULL WHERE [ISSUE DIAWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [CLOSING DIAWT] = NULL WHERE [CLOSING DIAWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [RECEIPT PREWT] = NULL WHERE [RECEIPT PREWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE PREWT] = NULL WHERE [ISSUE PREWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [CLOSING PREWT] = NULL WHERE [CLOSING PREWT] = 0"

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        Prop_Sets()

        Dim dtGrid As New DataTable("SUMMARY")
        dtGrid.Columns.Add("TABLENAME", GetType(String))
        dtGrid.Columns("TABLENAME").DefaultValue = "SUMMARY"
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        If chkCategorywise.Checked = True Then
            strSql = " SELECT PARTICULAR,TRANNO"
            If ChkWithTrantype.Checked Then strSql += ",TRANTYPE"
            strSql += ",TTRANDATE"
            If chkSpecificFormat.Checked = False Then strSql += ",REMARK,REFNO"
            strSql += ",CONVERT(VARCHAR(50),[RECEIPT PCS])AS [RECEIPT PCS]"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[RECEIPT " & bothWeight(0) & "])AS [RECEIPT " & bothWeight(0) & "],CONVERT(VARCHAR(50),[RECEIPT " & bothWeight(1) & "])AS [RECEIPT " & bothWeight(1) & "]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[RECEIPT STNPCS])AS [RECEIPT STNPCS],CONVERT(VARCHAR(50),[RECEIPT DIAPCS])AS [RECEIPT DIAPCS],CONVERT(VARCHAR(50),[RECEIPT PREPCS])AS [RECEIPT PREPCS]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[RECEIPT STNWT])AS [RECEIPT STNWT],CONVERT(VARCHAR(50),[RECEIPT DIAWT])AS [RECEIPT DIAWT],CONVERT(VARCHAR(50),[RECEIPT PREWT])AS [RECEIPT PREWT]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[ISSUE PCS])AS [ISSUE PCS],CONVERT(VARCHAR(50),[ISSUE " & bothWeight(0) & "])AS [ISSUE " & bothWeight(0) & "],CONVERT(VARCHAR(50),[ISSUE " & bothWeight(1) & "]) AS [ISSUE " & bothWeight(1) & "]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[ISSUE STNPCS])AS [ISSUE STNPCS],CONVERT(VARCHAR(50),[ISSUE DIAPCS])AS [ISSUE DIAPCS],CONVERT(VARCHAR(50),[ISSUE PREPCS])AS [ISSUE PREPCS]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[ISSUE STNWT])AS [ISSUE STNWT],CONVERT(VARCHAR(50),[ISSUE DIAWT])AS [ISSUE DIAWT],CONVERT(VARCHAR(50),[ISSUE PREWT])AS [ISSUE PREWT]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[CLOSING PCS])AS [CLOSING PCS],[CLOSING " & bothWeight(0) & "],[CLOSING " & bothWeight(1) & "]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[CLOSING STNPCS])AS [CLOSING STNPCS],CONVERT(VARCHAR(50),[CLOSING DIAPCS])AS [CLOSING DIAPCS],CONVERT(VARCHAR(50),[CLOSING PREPCS])AS [CLOSING PREPCS]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[CLOSING STNWT])AS [CLOSING STNWT],CONVERT(VARCHAR(50),[CLOSING DIAWT])AS [CLOSING DIAWT],CONVERT(VARCHAR(50),[CLOSING PREWT])AS [CLOSING PREWT]"
            Else
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[RECEIPT " & Weight & "])AS [RECEIPT " & Weight & "]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[RECEIPT STNPCS])AS [RECEIPT STNPCS],CONVERT(VARCHAR(50),[RECEIPT DIAPCS])AS [RECEIPT DIAPCS],CONVERT(VARCHAR(50),[RECEIPT PREPCS])AS [RECEIPT PREPCS]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[RECEIPT STNWT])AS [RECEIPT STNWT],CONVERT(VARCHAR(50),[RECEIPT DIAWT])AS [RECEIPT DIAWT],CONVERT(VARCHAR(50),[RECEIPT PREWT])AS [RECEIPT PREWT]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[ISSUE PCS])AS [ISSUE PCS],CONVERT(VARCHAR(50),[ISSUE " & Weight & "])AS [ISSUE " & Weight & "]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[ISSUE STNPCS])AS [ISSUE STNPCS],CONVERT(VARCHAR(50),[ISSUE DIAPCS])AS [ISSUE DIAPCS],CONVERT(VARCHAR(50),[ISSUE PREPCS])AS [ISSUE PREPCS]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[ISSUE STNWT])AS [ISSUE STNWT],CONVERT(VARCHAR(50),[ISSUE DIAWT])AS [ISSUE DIAWT],CONVERT(VARCHAR(50),[ISSUE PREWT])AS [ISSUE PREWT]"
                strSql += vbCrLf + " ,[CLOSING PCS],[CLOSING " & Weight & "]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[CLOSING STNPCS])AS [CLOSING STNPCS],CONVERT(VARCHAR(50),[CLOSING DIAPCS])AS [CLOSING DIAPCS],CONVERT(VARCHAR(50),[CLOSING PREPCS])AS [CLOSING PREPCS]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[CLOSING STNWT])AS [CLOSING STNWT],CONVERT(VARCHAR(50),[CLOSING DIAWT])AS [CLOSING DIAWT],CONVERT(VARCHAR(50),[CLOSING PREWT])AS [CLOSING PREWT]"
            End If
            strSql += vbCrLf + " ,CATNAME,METAL,GS,TRANDATE,ACNAME,ITEMNAME,RESULT,COLHEAD"

            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "GSSTOCK "
            strSql += vbCrLf + "  ORDER BY METAL,GS,CATNAME,TRANDATE,RESULT"
        ElseIf chkCategorywise.Checked = False Then
            strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "GSSTOCK "
            strSql += vbCrLf + "  ORDER BY METAL,GS,CATNAME,ITEMNAME,TRANDATE,RESULT"
        End If

        If chkOrderbyTranno.Checked And chkOrderbyTranno.Visible Then
            strSql += vbCrLf + " ,TRANNO, CASE WHEN TRANTYPE IN ('RRE','RPU','AR') THEN 0 ELSE 1 END"
        Else
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ,[RECEIPT " & bothWeight(0) & "],[RECEIPT " & bothWeight(1) & "] DESC"
            Else
                strSql += vbCrLf + " ,[RECEIPT " & Weight & "] DESC"
            End If
            strSql += vbCrLf + " ,[RECEIPT PCS] DESC,TRANNO"
        End If
        da = New OleDbDataAdapter(strSql, cn)
        dtGrid.Rows.Clear()
        da.Fill(dtGrid)

        Dim CheckCol As String = "GS"
        Dim Checker As String = "##gs2932"
        Dim RunWt As Decimal = 0
        Dim RunWt1 As Decimal = 0
        Dim RunPcs As Integer = 0
        Dim TotRunWt As Decimal = 0
        Dim TotRunWt1 As Decimal = 0
        Dim TotRunPcs As Integer = 0

        Dim RunStnWt As Decimal = 0
        Dim RunDiaWt As Decimal = 0
        Dim RunPreWt As Decimal = 0
        Dim RunStnPcs As Integer = 0
        Dim RunDiaPcs As Integer = 0
        Dim RunPrePcs As Integer = 0

        Dim checkCat As String = "CAT"
        Dim checkItem As String = "ITEM"
        Dim checkerCat As String = "##cat"
        'If rbtPcs.Checked Then
        If chkCategorywise.Checked Then CheckCol = "CATNAME"

        For Each Ro As DataRow In dtGrid.Rows
            'If chkCategorywise.Checked = False Then
            If Ro.Item("PARTICULAR").ToString = "   OPENING.." Then
                RunWt = 0 : RunPcs = 0 : RunWt1 = 0
            End If
            If Ro.Item(CheckCol).ToString <> Checker Then
                Checker = Ro.Item(CheckCol).ToString
                RunWt = 0 : RunPcs = 0 : RunWt1 = 0
            End If
            If Ro.Item("COLHEAD").ToString = "S" Then GoTo PRINTCOLUMN
            If Ro.Item("COLHEAD").ToString <> "" Then Continue For
            RunPcs += Val(Ro.Item("RECEIPT PCS").ToString) - Val(Ro.Item("ISSUE PCS").ToString)
            RunStnPcs += Val(Ro.Item("RECEIPT STNPCS").ToString) - Val(Ro.Item("ISSUE STNPCS").ToString)
            RunDiaPcs += Val(Ro.Item("RECEIPT DIAPCS").ToString) - Val(Ro.Item("ISSUE DIAPCS").ToString)
            RunPrePcs += Val(Ro.Item("RECEIPT PREPCS").ToString) - Val(Ro.Item("ISSUE PREPCS").ToString)
            If rdbBothwt.Checked Then
                RunWt += Val(Ro.Item("RECEIPT " & bothWeight(0) & "").ToString) - Val(Ro.Item("ISSUE " & bothWeight(0) & "").ToString)

                RunWt1 += Val(Ro.Item("RECEIPT " & bothWeight(1) & "").ToString) - Val(Ro.Item("ISSUE " & bothWeight(1) & "").ToString)

            Else
                RunWt += Val(Ro.Item("RECEIPT " & Weight & "").ToString) - Val(Ro.Item("ISSUE " & Weight & "").ToString)

            End If
            RunStnWt += Val(Ro.Item("RECEIPT STNWT").ToString) - Val(Ro.Item("ISSUE STNWT").ToString)
            RunDiaWt += Val(Ro.Item("RECEIPT DIAWT").ToString) - Val(Ro.Item("ISSUE DIAWT").ToString)
            RunPreWt += Val(Ro.Item("RECEIPT PREWT").ToString) - Val(Ro.Item("ISSUE PREWT").ToString)
PRINTCOLUMN:
            Ro.Item("CLOSING PCS") = IIf(RunPcs <> 0, RunPcs, DBNull.Value)
            Ro.Item("CLOSING STNPCS") = IIf(RunStnPcs <> 0, RunStnPcs, DBNull.Value)
            Ro.Item("CLOSING DIAPCS") = IIf(RunDiaPcs <> 0, RunDiaPcs, DBNull.Value)
            Ro.Item("CLOSING PREPCS") = IIf(RunPrePcs <> 0, RunPrePcs, DBNull.Value)
            Ro.Item("CLOSING STNWT") = IIf(RunStnWt <> 0, RunStnWt, DBNull.Value)
            Ro.Item("CLOSING DIAWT") = IIf(RunDiaWt <> 0, RunDiaWt, DBNull.Value)
            Ro.Item("CLOSING PREWT") = IIf(RunPreWt <> 0, RunPreWt, DBNull.Value)

            'If Ro.Item("CLOSING PCS").ToString <> "" Then TotRunPcs += Ro.Item("CLOSING PCS").ToString
            If rdbBothwt.Checked Then
                Ro.Item("CLOSING " & bothWeight(0) & "") = IIf(RunWt <> 0, Format(RunWt, "0.000"), DBNull.Value)
                Ro.Item("CLOSING " & bothWeight(1) & "") = IIf(RunWt1 <> 0, Format(RunWt1, "0.000"), DBNull.Value)
                If Ro.Item("PARTICULAR").ToString = "   OPENING.." Then
                    Ro.Item("ISSUE " & bothWeight(0) & "") = DBNull.Value
                    Ro.Item("ISSUE " & bothWeight(1) & "") = DBNull.Value
                    Ro.Item("ISSUE STNWT") = DBNull.Value
                    Ro.Item("ISSUE DIAWT") = DBNull.Value
                    Ro.Item("ISSUE PREWT") = DBNull.Value
                    Ro.Item("RECEIPT " & bothWeight(0) & "") = DBNull.Value
                    Ro.Item("RECEIPT " & bothWeight(1) & "") = DBNull.Value
                    Ro.Item("RECEIPT STNWT") = DBNull.Value
                    Ro.Item("RECEIPT DIAWT") = DBNull.Value
                    Ro.Item("RECEIPT PREWT") = DBNull.Value
                End If
            Else
                Ro.Item("CLOSING " & Weight & "") = IIf(RunWt <> 0, Format(RunWt, "0.000"), DBNull.Value)
                If Ro.Item("PARTICULAR").ToString = "   OPENING.." Then
                    Ro.Item("ISSUE " & Weight & "") = DBNull.Value
                    Ro.Item("ISSUE STNWT") = DBNull.Value
                    Ro.Item("ISSUE DIAWT") = DBNull.Value
                    Ro.Item("ISSUE PREWT") = DBNull.Value
                    Ro.Item("RECEIPT " & Weight & "") = DBNull.Value
                    Ro.Item("RECEIPT STNWT") = DBNull.Value
                    Ro.Item("RECEIPT DIAWT") = DBNull.Value
                    Ro.Item("RECEIPT PREWT") = DBNull.Value
                End If
            End If

        Next

        dtGrid.AcceptChanges()
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        dtGrid.Columns("TABLENAME").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            Exit Sub
        End If
        dtGrid.Columns("TTRANDATE").SetOrdinal(0)
        dtGrid.Columns("TRANNO").SetOrdinal(1)
        Dim mnt As Integer = 2
        If ChkWithTrantype.Checked Then dtGrid.Columns("TRANTYPE").SetOrdinal(mnt) : mnt += 1
        dtGrid.Columns("PARTICULAR").SetOrdinal(mnt)
        If chkSpecificFormat.Checked = False Then mnt += 1 : dtGrid.Columns("REMARK").SetOrdinal(mnt)
        If chkSpecificFormat.Checked = False Then mnt += 1 : dtGrid.Columns("REFNO").SetOrdinal(mnt)

        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Size = New Size(778, 550)
        objGridShower.Text = "METAL ORNAMENT STOCK VIEW"
        Dim TitleName As String = ""
        If gstitlename <> "" Then
            strSql = "SELECT COMPANYNAME,ADDRESS1,ADDRESS2,ADDRESS3,AREACODE,PHONE,TINNO,PANNO FROM " & cnAdminDb & "..COMPANY WHERE COMPANYID='" & strCompanyId & "'"
            cmd = New OleDbCommand(strSql, cn)
            da = New OleDbDataAdapter(cmd)
            Dim dtCompany As New DataTable
            da.Fill(dtCompany)

            If dtCompany.Rows.Count > 0 Then
                TitleName += vbCrLf + IIf(dtCompany.Rows(0).Item("COMPANYNAME").ToString <> "", dtCompany.Rows(0).Item("COMPANYNAME").ToString, "")
                TitleName += vbCrLf + IIf(dtCompany.Rows(0).Item("ADDRESS1").ToString <> "", dtCompany.Rows(0).Item("ADDRESS1").ToString, "")
                TitleName += IIf(dtCompany.Rows(0).Item("ADDRESS2").ToString <> "", " " + dtCompany.Rows(0).Item("ADDRESS2").ToString, "")
                TitleName += IIf(dtCompany.Rows(0).Item("ADDRESS3").ToString <> "", " " + dtCompany.Rows(0).Item("ADDRESS3").ToString, "")
                TitleName += IIf(dtCompany.Rows(0).Item("AREACODE").ToString <> "", " " + dtCompany.Rows(0).Item("AREACODE").ToString, "")
                TitleName += IIf(dtCompany.Rows(0).Item("PHONE").ToString <> "", ". Ph: " + dtCompany.Rows(0).Item("PHONE").ToString, "")
                TitleName += vbCrLf + IIf(dtCompany.Rows(0).Item("TINNO").ToString <> "", "TIN." + dtCompany.Rows(0).Item("TINNO").ToString, "")
                TitleName += IIf(dtCompany.Rows(0).Item("PANNO").ToString <> "", ".(PAN-" + dtCompany.Rows(0).Item("PANNO").ToString + ")", "")
            End If
            TitleName += vbCrLf + gstitlename
            If chkIntoCt.Checked Then TitleName += "(STONE IN CT.)"
            If chkIntoGm.Checked Then TitleName += "(STONE IN GM.)"
            objGridShower.lblTitle.Text = TitleName
        Else
            Dim tit As String = "METAL ORNAMENT STOCK VIEW "
            tit += "DATE FROM " + dtpFrom.Text + " TO " + dtpTo.Text
            If chkCostCentreSelectAll.Checked = False Then tit += " - " + chkCostName.Replace("'", "")
            If chkMetalSelectAll.Checked = False Then tit += vbCrLf + "[" + chkMetal.Replace("'", "") + "]"
            If chkCategorySelectAll.Checked = False Then tit += " - [ " + chkCategory.Replace("'", "") + "]"
            If chkIntoCt.Checked Then tit += "(STONE IN CT.)"
            If chkIntoGm.Checked Then tit += "(STONE IN GM.)"
            If cmbstocktype.Text <> "" And cmbstocktype.Text <> "ALL" Then tit += " STOCK TYPE " & cmbstocktype.Text
            objGridShower.lblTitle.Text = tit
        End If
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.pnlFooter.Visible = False
        objGridShower.FormReLocation = False
        objGridShower.FormReSize = True
        objGridShower.gridViewHeader.Visible = True
        DataGridView_Trandate(objGridShower.gridView)
        objGridShower.gridView.ColumnHeadersVisible = True
        objGridShower.Show()
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        GridViewHeaderCreator(objGridShower.gridViewHeader, objGridShower.gridView)
        objGridShower.FormReSize = True
        dtpFrom.Select()
        If chkCategorywise.Checked = True Then
            For i As Integer = 0 To objGridShower.gridView.Rows.Count - 1
                If objGridShower.gridView.Rows(i).Cells("PARTICULAR").Value.ToString <> "" Then
                    If objGridShower.gridView.Rows(i).Cells("PARTICULAR").Value = "   OPENING.." Then
                        If rdbBothwt.Checked Then
                            objGridShower.gridView.Rows(i).Cells("RECEIPT " & bothWeight(0)).Value = String.Empty
                            objGridShower.gridView.Rows(i).Cells("ISSUE " & bothWeight(0)).Value = String.Empty
                            objGridShower.gridView.Rows(i).Cells("RECEIPT " & bothWeight(1)).Value = String.Empty
                            objGridShower.gridView.Rows(i).Cells("ISSUE " & bothWeight(1)).Value = String.Empty
                            objGridShower.gridView.Rows(i).Cells("RECEIPT PCS").Value = String.Empty
                            objGridShower.gridView.Rows(i).Cells("ISSUE PCS").Value = String.Empty
                        Else
                            objGridShower.gridView.Rows(i).Cells("RECEIPT " & Weight).Value = String.Empty
                            objGridShower.gridView.Rows(i).Cells("ISSUE " & Weight).Value = String.Empty
                            objGridShower.gridView.Rows(i).Cells("RECEIPT PCS").Value = String.Empty
                            objGridShower.gridView.Rows(i).Cells("ISSUE PCS").Value = String.Empty
                        End If
                    End If
                End If
                Dim cloPcs, cloGrswt, cloNetwt, cloWt As Decimal
                If objGridShower.gridView.Rows(i).Cells("COLHEAD").Value.ToString <> "" Then
                    If objGridShower.gridView.Rows(i).Cells("COLHEAD").Value = "S1" Then
                        If rbtTranno.Checked Then
                            If objGridShower.gridView.Rows(i - 2).Cells("CLOSING PCS").Value.ToString <> "" Then
                                cloPcs = objGridShower.gridView.Rows(i - 2).Cells("CLOSING PCS").Value
                                objGridShower.gridView.Rows(i).Cells("CLOSING PCS").Value = cloPcs
                            End If
                            If rdbBothwt.Checked Then
                                If objGridShower.gridView.Rows(i - 2).Cells("CLOSING " & bothWeight(0)).Value.ToString <> "" Then
                                    cloGrswt = objGridShower.gridView.Rows(i - 2).Cells("CLOSING " & bothWeight(0)).Value
                                    objGridShower.gridView.Rows(i).Cells("CLOSING " & bothWeight(0)).Value = cloGrswt
                                End If
                                If objGridShower.gridView.Rows(i - 2).Cells("CLOSING " & bothWeight(1)).Value.ToString <> "" Then
                                    cloNetwt = objGridShower.gridView.Rows(i - 2).Cells("CLOSING " & bothWeight(1)).Value
                                    objGridShower.gridView.Rows(i).Cells("CLOSING " & bothWeight(1)).Value = cloNetwt
                                End If
                            Else
                                If objGridShower.gridView.Rows(i - 2).Cells("CLOSING " & Weight).Value.ToString <> "" Then
                                    cloWt = objGridShower.gridView.Rows(i - 2).Cells("CLOSING " & Weight).Value
                                    objGridShower.gridView.Rows(i).Cells("CLOSING " & Weight).Value = cloWt
                                End If
                            End If
                        ElseIf rbtTrandate.Checked Then
                            If objGridShower.gridView.Rows(i - 1).Cells("CLOSING PCS").Value.ToString <> "" Then
                                cloPcs = objGridShower.gridView.Rows(i - 1).Cells("CLOSING PCS").Value
                                objGridShower.gridView.Rows(i).Cells("CLOSING PCS").Value = cloPcs
                            End If
                            If rdbBothwt.Checked Then
                                If objGridShower.gridView.Rows(i - 1).Cells("CLOSING " & bothWeight(0)).Value.ToString <> "" Then
                                    cloGrswt = objGridShower.gridView.Rows(i - 1).Cells("CLOSING " & bothWeight(0)).Value
                                    objGridShower.gridView.Rows(i).Cells("CLOSING " & bothWeight(0)).Value = cloGrswt
                                End If
                                If objGridShower.gridView.Rows(i - 1).Cells("CLOSING " & bothWeight(1)).Value.ToString <> "" Then
                                    cloNetwt = objGridShower.gridView.Rows(i - 1).Cells("CLOSING " & bothWeight(1)).Value
                                    objGridShower.gridView.Rows(i).Cells("CLOSING " & bothWeight(1)).Value = cloNetwt
                                End If
                            Else
                                If objGridShower.gridView.Rows(i - 1).Cells("CLOSING " & Weight).Value.ToString <> "" Then
                                    cloWt = objGridShower.gridView.Rows(i - 1).Cells("CLOSING " & Weight).Value
                                    objGridShower.gridView.Rows(i).Cells("CLOSING " & Weight).Value = cloWt
                                End If
                            End If
                        End If
                    End If
                End If
            Next
        End If
    End Sub
    Private Sub GridViewHeaderCreator(ByVal gridviewHead As DataGridView, ByVal gridview As DataGridView)
        Dim bothWeight As String()
        Weight = IIf(rbtGrsWeight.Checked, "GRSWT", "NETWT")
        If rbtPureWeight.Checked Then Weight = "PUREWT"
        If rdbBothwt.Checked Then
            Weight = "GRSWT,NETWT"
        End If
        If rdbBothwt.Checked Then
            bothWeight = Weight.Split(",")
        End If

        Dim dtHead As New DataTable
        strSql = "SELECT ''[TTRANDATE],''[TRANNO]"
        If ChkWithTrantype.Checked Then strSql += ",''[TRANTYPE]"
        strSql += ",''[PARTICULAR]"
        If chkSpecificFormat.Checked = False Then strSql += ",''[REMARK],''[REFNO]"
        If rdbBothwt.Checked Then
            strSql += ",''[RECEIPT PCS~RECEIPT " & bothWeight(0) & "~RECEIPT " & bothWeight(1) & "~RECEIPT STNPCS~RECEIPT DIAPCS~RECEIPT PREPCS~RECEIPT STNWT~RECEIPT DIAWT~RECEIPT PREWT]"
            strSql += ",''[ISSUE PCS~ISSUE " & bothWeight(0) & "~ISSUE " & bothWeight(1) & "~ISSUE STNPCS~ISSUE DIAPCS~ISSUE PREPCS~ISSUE STNWT~ISSUE DIAWT~ISSUE PREWT]"
            strSql += ",''[CLOSING PCS~CLOSING " & bothWeight(0) & "~CLOSING " & bothWeight(1) & "~CLOSING STNPCS~CLOSING DIAPCS~CLOSING PREPCS~CLOSING STNWT~CLOSING DIAWT~CLOSING PREWT]"
        Else
            strSql += ",''[RECEIPT PCS~RECEIPT " & Weight & "~RECEIPT STNPCS~RECEIPT DIAPCS~RECEIPT PREPCS~RECEIPT STNWT~RECEIPT DIAWT~RECEIPT PREWT]"
            strSql += ",''[ISSUE PCS~ISSUE " & Weight & "~ISSUE STNPCS~ISSUE DIAPCS~ISSUE PREPCS~ISSUE STNWT~ISSUE DIAWT~ISSUE PREWT]"
            strSql += ",''[CLOSING PCS~CLOSING " & Weight & "~CLOSING STNPCS~CLOSING DIAPCS~CLOSING PREPCS~CLOSING STNWT~CLOSING DIAWT~CLOSING PREWT]"
        End If
        strSql += " ,''SCROLL"

        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtHead)
        gridviewHead.DataSource = dtHead
        gridviewHead.Columns("TTRANDATE").HeaderText = ""
        gridviewHead.Columns("TTRANDATE").Width = IIf(gridview.Columns("TTRANDATE").Visible, gridview.Columns("TTRANDATE").Width, 0)
        gridviewHead.Columns("TRANNO").HeaderText = ""
        gridviewHead.Columns("TRANNO").Width = IIf(gridview.Columns("TRANNO").Visible, gridview.Columns("TRANNO").Width, 0)
        gridviewHead.Columns("TRANNO").Visible = gridview.Columns("TRANNO").Visible
        gridviewHead.Columns("PARTICULAR").HeaderText = "PARTICULAR"
        gridviewHead.Columns("PARTICULAR").Width = gridview.Columns("PARTICULAR").Width
        If ChkWithTrantype.Checked Then
            gridviewHead.Columns("TRANTYPE").HeaderText = ""
            ' gridviewHead.Columns("TRANTYPE").Width = 0
        End If
        If chkSpecificFormat.Checked = False Then
            gridviewHead.Columns("REMARK").HeaderText = ""
            gridviewHead.Columns("REMARK").Width = gridview.Columns("REMARK").Width
            gridviewHead.Columns("REFNO").HeaderText = ""
            gridviewHead.Columns("REFNO").Width = gridview.Columns("REFNO").Width
        End If
        If rdbBothwt.Checked Then
            gridviewHead.Columns("RECEIPT PCS~RECEIPT " & bothWeight(0) & "~RECEIPT " & bothWeight(1) & "~RECEIPT STNPCS~RECEIPT DIAPCS~RECEIPT PREPCS~RECEIPT STNWT~RECEIPT DIAWT~RECEIPT PREWT").HeaderText = "RECEIPT"
            gridviewHead.Columns("RECEIPT PCS~RECEIPT " & bothWeight(0) & "~RECEIPT " & bothWeight(1) & "~RECEIPT STNPCS~RECEIPT DIAPCS~RECEIPT PREPCS~RECEIPT STNWT~RECEIPT DIAWT~RECEIPT PREWT").Width =
            IIf(gridview.Columns("RECEIPT PCS").Visible, gridview.Columns("RECEIPT PCS").Width, 0) _
            + gridview.Columns("RECEIPT " & bothWeight(0) & "").Width + gridview.Columns("RECEIPT " & bothWeight(1) & "").Width _
            + IIf(gridview.Columns("RECEIPT STNPCS").Visible, gridview.Columns("RECEIPT STNPCS").Width, 0) + IIf(gridview.Columns("RECEIPT STNWT").Visible, gridview.Columns("RECEIPT STNWT").Width, 0) _
            + IIf(gridview.Columns("RECEIPT DIAPCS").Visible, gridview.Columns("RECEIPT DIAPCS").Width, 0) + IIf(gridview.Columns("RECEIPT DIAWT").Visible, gridview.Columns("RECEIPT DIAWT").Width, 0) _
            + IIf(gridview.Columns("RECEIPT PREPCS").Visible, gridview.Columns("RECEIPT PREPCS").Width, 0) + IIf(gridview.Columns("RECEIPT PREWT").Visible, gridview.Columns("RECEIPT PREWT").Width, 0)

            gridviewHead.Columns("ISSUE PCS~ISSUE " & bothWeight(0) & "~ISSUE " & bothWeight(1) & "~ISSUE STNPCS~ISSUE DIAPCS~ISSUE PREPCS~ISSUE STNWT~ISSUE DIAWT~ISSUE PREWT").HeaderText = "ISSUE"
            gridviewHead.Columns("ISSUE PCS~ISSUE " & bothWeight(0) & "~ISSUE " & bothWeight(1) & "~ISSUE STNPCS~ISSUE DIAPCS~ISSUE PREPCS~ISSUE STNWT~ISSUE DIAWT~ISSUE PREWT").Width =
            IIf(gridview.Columns("ISSUE PCS").Visible, gridview.Columns("ISSUE PCS").Width, 0) _
            + gridview.Columns("ISSUE " & bothWeight(0) & "").Width + gridview.Columns("ISSUE " & bothWeight(1) & "").Width _
            + IIf(gridview.Columns("ISSUE STNPCS").Visible, gridview.Columns("ISSUE STNPCS").Width, 0) + IIf(gridview.Columns("ISSUE STNWT").Visible, gridview.Columns("ISSUE STNWT").Width, 0) _
            + IIf(gridview.Columns("ISSUE DIAPCS").Visible, gridview.Columns("ISSUE DIAPCS").Width, 0) + IIf(gridview.Columns("ISSUE DIAWT").Visible, gridview.Columns("ISSUE DIAWT").Width, 0) _
            + IIf(gridview.Columns("ISSUE PREPCS").Visible, gridview.Columns("ISSUE PREPCS").Width, 0) + IIf(gridview.Columns("ISSUE PREWT").Visible, gridview.Columns("ISSUE PREWT").Width, 0)

            gridviewHead.Columns("CLOSING PCS~CLOSING " & bothWeight(0) & "~CLOSING " & bothWeight(1) & "~CLOSING STNPCS~CLOSING DIAPCS~CLOSING PREPCS~CLOSING STNWT~CLOSING DIAWT~CLOSING PREWT").HeaderText = "CLOSING"
            gridviewHead.Columns("CLOSING PCS~CLOSING " & bothWeight(0) & "~CLOSING " & bothWeight(1) & "~CLOSING STNPCS~CLOSING DIAPCS~CLOSING PREPCS~CLOSING STNWT~CLOSING DIAWT~CLOSING PREWT").Width =
            IIf(gridview.Columns("CLOSING PCS").Visible, gridview.Columns("CLOSING PCS").Width, 0) _
            + gridview.Columns("CLOSING " & bothWeight(0) & "").Width + gridview.Columns("CLOSING " & bothWeight(1) & "").Width _
            + IIf(gridview.Columns("CLOSING STNPCS").Visible, gridview.Columns("CLOSING STNPCS").Width, 0) + IIf(gridview.Columns("CLOSING STNWT").Visible, gridview.Columns("CLOSING STNWT").Width, 0) _
            + IIf(gridview.Columns("CLOSING DIAPCS").Visible, gridview.Columns("CLOSING DIAPCS").Width, 0) + IIf(gridview.Columns("CLOSING DIAWT").Visible, gridview.Columns("CLOSING DIAWT").Width, 0) _
            + IIf(gridview.Columns("CLOSING PREPCS").Visible, gridview.Columns("CLOSING PREPCS").Width, 0) + IIf(gridview.Columns("CLOSING PREWT").Visible, gridview.Columns("CLOSING PREWT").Width, 0)
        Else
            gridviewHead.Columns("RECEIPT PCS~RECEIPT " & Weight & "~RECEIPT STNPCS~RECEIPT DIAPCS~RECEIPT PREPCS~RECEIPT STNWT~RECEIPT DIAWT~RECEIPT PREWT").HeaderText = "RECEIPT"

            gridviewHead.Columns("RECEIPT PCS~RECEIPT " & Weight & "~RECEIPT STNPCS~RECEIPT DIAPCS~RECEIPT PREPCS~RECEIPT STNWT~RECEIPT DIAWT~RECEIPT PREWT").Width =
            IIf(gridview.Columns("RECEIPT PCS").Visible, gridview.Columns("RECEIPT PCS").Width, 0) + gridview.Columns("RECEIPT " & Weight & "").Width _
            + IIf(gridview.Columns("RECEIPT STNPCS").Visible, gridview.Columns("RECEIPT STNPCS").Width, 0) + IIf(gridview.Columns("RECEIPT STNWT").Visible, gridview.Columns("RECEIPT STNWT").Width, 0) _
            + IIf(gridview.Columns("RECEIPT DIAPCS").Visible, gridview.Columns("RECEIPT DIAPCS").Width, 0) + IIf(gridview.Columns("RECEIPT DIAWT").Visible, gridview.Columns("RECEIPT DIAWT").Width, 0) _
            + IIf(gridview.Columns("RECEIPT PREPCS").Visible, gridview.Columns("RECEIPT PREPCS").Width, 0) + IIf(gridview.Columns("RECEIPT PREWT").Visible, gridview.Columns("RECEIPT PREWT").Width, 0)

            gridviewHead.Columns("ISSUE PCS~ISSUE " & Weight & "~ISSUE STNPCS~ISSUE DIAPCS~ISSUE PREPCS~ISSUE STNWT~ISSUE DIAWT~ISSUE PREWT").HeaderText = "ISSUE"
            gridviewHead.Columns("ISSUE PCS~ISSUE " & Weight & "~ISSUE STNPCS~ISSUE DIAPCS~ISSUE PREPCS~ISSUE STNWT~ISSUE DIAWT~ISSUE PREWT").Width =
            IIf(gridview.Columns("ISSUE PCS").Visible, gridview.Columns("ISSUE PCS").Width, 0) + gridview.Columns("ISSUE " & Weight & "").Width _
            + IIf(gridview.Columns("ISSUE STNPCS").Visible, gridview.Columns("ISSUE STNPCS").Width, 0) + IIf(gridview.Columns("ISSUE STNWT").Visible, gridview.Columns("ISSUE STNWT").Width, 0) _
            + IIf(gridview.Columns("ISSUE DIAPCS").Visible, gridview.Columns("ISSUE DIAPCS").Width, 0) + IIf(gridview.Columns("ISSUE DIAWT").Visible, gridview.Columns("ISSUE DIAWT").Width, 0) _
            + IIf(gridview.Columns("ISSUE PREPCS").Visible, gridview.Columns("ISSUE PREPCS").Width, 0) + IIf(gridview.Columns("ISSUE PREWT").Visible, gridview.Columns("ISSUE PREWT").Width, 0)

            gridviewHead.Columns("CLOSING PCS~CLOSING " & Weight & "~CLOSING STNPCS~CLOSING DIAPCS~CLOSING PREPCS~CLOSING STNWT~CLOSING DIAWT~CLOSING PREWT").HeaderText = "CLOSING"
            gridviewHead.Columns("CLOSING PCS~CLOSING " & Weight & "~CLOSING STNPCS~CLOSING DIAPCS~CLOSING PREPCS~CLOSING STNWT~CLOSING DIAWT~CLOSING PREWT").Width =
            IIf(gridview.Columns("CLOSING PCS").Visible, gridview.Columns("CLOSING PCS").Width, 0) + gridview.Columns("CLOSING " & Weight & "").Width _
            + IIf(gridview.Columns("CLOSING STNPCS").Visible, gridview.Columns("CLOSING STNPCS").Width, 0) + IIf(gridview.Columns("CLOSING STNWT").Visible, gridview.Columns("CLOSING STNWT").Width, 0) _
            + IIf(gridview.Columns("CLOSING DIAPCS").Visible, gridview.Columns("CLOSING DIAPCS").Width, 0) + IIf(gridview.Columns("CLOSING DIAWT").Visible, gridview.Columns("CLOSING DIAWT").Width, 0) _
            + IIf(gridview.Columns("CLOSING PREPCS").Visible, gridview.Columns("CLOSING PREPCS").Width, 0) + IIf(gridview.Columns("CLOSING PREWT").Visible, gridview.Columns("CLOSING PREWT").Width, 0)
        End If

        gridview.Columns("RECEIPT PCS").HeaderText = "PCS"
        gridview.Columns("ISSUE PCS").HeaderText = "PCS"
        gridview.Columns("CLOSING PCS").HeaderText = "PCS"
        If rdbBothwt.Checked Then
            gridview.Columns("RECEIPT " & bothWeight(0) & "").HeaderText = bothWeight(0)
            gridview.Columns("RECEIPT " & bothWeight(1) & "").HeaderText = bothWeight(1)
            gridview.Columns("ISSUE " & bothWeight(0) & "").HeaderText = bothWeight(0)
            gridview.Columns("ISSUE " & bothWeight(1) & "").HeaderText = bothWeight(1)
            gridview.Columns("CLOSING " & bothWeight(0) & "").HeaderText = bothWeight(0)
            gridview.Columns("CLOSING " & bothWeight(1) & "").HeaderText = bothWeight(1)
        Else
            gridview.Columns("RECEIPT " & Weight & "").HeaderText = Weight
            gridview.Columns("ISSUE " & Weight & "").HeaderText = Weight
            gridview.Columns("CLOSING " & Weight & "").HeaderText = Weight
        End If
        gridview.Columns("RECEIPT STNPCS").HeaderText = "STNPCS"
        gridview.Columns("ISSUE STNPCS").HeaderText = "STNPCS"
        gridview.Columns("CLOSING STNPCS").HeaderText = "STNPCS"
        gridview.Columns("RECEIPT STNWT").HeaderText = "STNWT"
        gridview.Columns("ISSUE STNWT").HeaderText = "STNWT"
        gridview.Columns("CLOSING STNWT").HeaderText = "STNWT"

        gridview.Columns("RECEIPT DIAPCS").HeaderText = "DIAPCS"
        gridview.Columns("ISSUE DIAPCS").HeaderText = "DIAPCS"
        gridview.Columns("CLOSING DIAPCS").HeaderText = "DIAPCS"
        gridview.Columns("RECEIPT DIAWT").HeaderText = "DIAWT"
        gridview.Columns("ISSUE DIAWT").HeaderText = "DIAWT"
        gridview.Columns("CLOSING DIAWT").HeaderText = "DIAWT"

        gridview.Columns("RECEIPT PREPCS").HeaderText = "PREPCS"
        gridview.Columns("ISSUE PREPCS").HeaderText = "PREPCS"
        gridview.Columns("CLOSING PREPCS").HeaderText = "PREPCS"
        gridview.Columns("RECEIPT PREWT").HeaderText = "PREWT"
        gridview.Columns("ISSUE PREWT").HeaderText = "PREWT"
        gridview.Columns("CLOSING PREWT").HeaderText = "PREWT"

        gridviewHead.Columns("SCROLL").HeaderText = ""
        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        gridviewHead.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        'SetGridHeadColWidth(gridviewHead)
    End Sub


    Function PrintReport()
        Dim bothWeight As String()
        Dim LblWidth As Integer = 20
        Dim LblSpaceWidth As Integer = 1
        Dim StrDate As String = ""
        Dim SubTotalAmt As Double = 0
        Dim SubTotalEntry As Double = 0
        Dim GrossTotal As Double = 0, RecAmt As Double = 0, PayAmt As Double = 0
        Dim ds As New Data.DataSet
        Dim SubTotalRGWT, SubTOTRNWT, SubTOTIGWT, SubTOTINWT As Double
        Dim SubTotalRPCS, SubTotalIPCS As Double
        Dim GrandTotRGWT, GrandTotRNWT, GrandTOTIGWT, GrandTOTINWT, CLOTOTGWT, CLOTOTNWT As Double
        Dim GrandTotRPCS, GrandTotIPCS, CLOTOTRPCS, CLOTOTIPCS As Double
        Dim EXCLTRANFLAG As String = "'WC','AC'"
        Dim dv As New DataView
        Dim sampledate As String = ""
        funcEndPrint = False
        Dim strDate1 As String

        'Weight = IIf(rbtGrsWeight.Checked, "GRSWT", "NETWT")
        If rbtGrsWeight.Checked Then
            Weight = "GRSWT"
        ElseIf rbtNetWt.Checked Then
            Weight = "NETWT"
        Else
            Weight = "GRSWT,NETWT"
        End If
        If rdbBothwt.Checked Then
            bothWeight = Weight.Split(",")
        End If
        If Not chkLstCostCentre.CheckedItems.Count > 0 Then
            chkCostCentreSelectAll.Checked = True
        End If
        If Not chkLstMetal.CheckedItems.Count > 0 Then
            chkMetalSelectAll.Checked = True
            LoadCategory()
        End If
        If Not chkLstCategory.CheckedItems.Count > 0 Then
            chkCategorySelectAll.Checked = True
        End If

        chkCostName = GetChecked_CheckedList(chkLstCostCentre)
        chkMetal = GetChecked_CheckedList(chkLstMetal)
        chkCategory = GetChecked_CheckedList(chkLstCategory)
        TranType = ""
        Apptrantype = ""
        Misctrantype = ""
        If chkCmbTransation.Text <> "ALL" And chkCmbTransation.Text <> "" Then
            If chkCmbTransation.Text.Contains("GENERAL") Then
                TranType += "'SA','OD','SR','PU','IIS','IPU','RRE','RPU','AD','RIS',"
            End If
            If chkCmbTransation.Text.Contains("MISCISSUE") Then
                TranType += "'MI','IOT','ROT','RIS',"
            End If
            If chkCmbTransation.Text.Contains("APPROVAL") Then
                TranType += "'AI','AR','IAP','RAP',"
            End If
            If chkCmbTransation.Text.Contains("INTERNAL") Then
                TranType += "'IIN','RIN',"
            End If
            If TranType <> "" Then
                TranType = Mid(TranType, 1, TranType.Length - 1)
            End If
            If chkCmbTransation.Text.Contains("APPROVAL") And ChkWithApproval.Checked = False And chkMIMRApproval.Checked = False Then
                MsgBox("Must select Any One Approval Type Counter/MIMR", MsgBoxStyle.Information)
                Exit Function
            End If
        Else
            If ChkWithApproval.Checked = False And chkMIMRApproval.Checked = False Then
                Apptrantype = "'AI','AR','IAP','RAP'"
            ElseIf ChkWithApproval.Checked = False Then
                Apptrantype = "'AI','AR'"
            ElseIf chkMIMRApproval.Checked = False Then
                Apptrantype = "'IAP','RAP'"
            End If
        End If
        If chkWithMiscIssue.Checked = False Then
            Misctrantype = "'MI'"
        End If

        strSql = " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ORDREP_DEL' AND XTYPE = 'U')>0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ORDREP_DEL"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "GSSTOCK' AND XTYPE = 'U')>0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "GSSTOCK_1' AND XTYPE = 'U')>0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ISS' AND XTYPE = 'V')>0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ISS"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "REC' AND XTYPE = 'V')>0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "REC"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ISS' AND XTYPE = 'U')>0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "ISS"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "REC' AND XTYPE = 'U')>0 DROP TABLE TEMPTABLEDB..TEMP" & systemId & "REC"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = " "
        strSql += vbCrLf + " "
        strSql += vbCrLf + " SELECT ITEMID,SUBITEMID,TRANNO,REFNO,I.TRANDATE,I.BATCHNO,CATCODE,CASE WHEN ISNULL(I.ACCODE,'') ='' then pinf.PNAME else A.ACNAME end as ACCODE,TRANTYPE"
        strSql += vbCrLf + " ,SUM(PCS) PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREPCS"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "ISS FROM " & cnStockDb & "..ISSUE AS I"
        strSql += vbCrLf + " left join " & cnAdminDb & "..CUSTOMERINFO as cinf on i.batchno = cinf.batchno"
        strSql += vbCrLf + " left join  " & cnAdminDb & "..personalinfo as pinf on cinf.psno = pinf.sno"
        strSql += vbCrLf + " left join " & cnAdminDb & "..Achead as A on i.accode = a.accode"
        strSql += vbCrLf + " WHERE I.TRANTYPE <> 'IRC' AND  I.TRANDATE <= '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
        If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
            strSql += " AND ISNULL(I.ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
        End If
        'strSql += vbCrLf + " AND I.TRANFLAG NOT IN (" & EXCLTRANFLAG & ")"
        If TranType <> "" Then strSql += vbCrLf + " AND I.TRANTYPE IN (" & TranType & ")"
        If Apptrantype <> "" Then strSql += vbCrLf + " AND I.TRANTYPE NOT IN (" & Apptrantype & ")"
        If Misctrantype <> "" Then strSql += vbCrLf + " AND I.TRANTYPE NOT IN (" & Misctrantype & ")"
        If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        If ChkRepairDet.Checked = False Then strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = I.BATCHNO AND TRANTYPE = 'RD')"
        strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSMETAL WHERE ISSSNO = I.SNO)"
        strSql += vbCrLf + " group by ITEMID,SUBITEMID,TRANNO,REFNO,I.TRANDATE,I.BATCHNO,CATCODE,I.SNO,i.ACCODE,pinf.PNAME ,TRANTYPE,ACNAME"
        strSql += vbCrLf + " UNION ALL"
        strSql += vbCrLf + " SELECT I.ITEMID,I.SUBITEMID,ISM.TRANNO,I.REFNO,ISM.TRANDATE,ISM.BATCHNO,ISM.CATCODE,CASE WHEN ISNULL(I.ACCODE,'') ='' THEN PINF.PNAME ELSE A.ACNAME END AS ACCODE,ISM.TRANTYPE"
        strSql += vbCrLf + " ,SUM(I.PCS) PCS,SUM(ISM.GRSWT) GRSWT,SUM(CASE WHEN ISNULL(ISM.NETWT,0)<>0 THEN ISM.NETWT ELSE ISM.GRSWT END) NETWT"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREPCS"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSMETAL AS ISM "
        strSql += vbCrLf + " LEFT JOIN " & cnStockDb & "..ISSUE AS I ON ISM.ISSSNO=I.SNO"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO AS CINF ON I.BATCHNO = CINF.BATCHNO"
        strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..PERSONALINFO AS PINF ON CINF.PSNO = PINF.SNO"
        strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD AS A ON I.ACCODE = A.ACCODE"
        strSql += vbCrLf + " WHERE ISM.TRANDATE <= '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND ISM.COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
        If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
            strSql += " AND ISNULL(I.ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
        End If
        'strSql += vbCrLf + " AND I.TRANFLAG NOT IN (" & EXCLTRANFLAG & ")"
        If TranType <> "" Then strSql += vbCrLf + " AND ISM.TRANTYPE IN (" & TranType & ")"
        If Apptrantype <> "" Then strSql += vbCrLf + " AND I.TRANTYPE NOT IN (" & Apptrantype & ")"
        If Misctrantype <> "" Then strSql += vbCrLf + " AND I.TRANTYPE NOT IN (" & Misctrantype & ")"
        If chkCostName <> "" Then strSql += vbCrLf + "  AND ISM.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkCategory <> "" Then strSql += vbCrLf + "  AND ISM.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        If ChkRepairDet.Checked = False Then strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = I.BATCHNO AND TRANTYPE = 'RD')"
        strSql += vbCrLf + " GROUP BY I.ITEMID,I.SUBITEMID,ISM.TRANNO,I.REFNO,ISM.TRANDATE,ISM.BATCHNO,ISM.CATCODE,I.SNO,I.ACCODE,PINF.PNAME ,ISM.TRANTYPE,ACNAME"
        If chkWithAlloy.Checked Then
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT I.ITEMID,I.SUBITEMID,AD.TRANNO,I.REFNO,AD.TRANDATE,AD.BATCHNO,I.CATCODE,CASE WHEN ISNULL(I.ACCODE,'') ='' THEN PINF.PNAME ELSE A.ACNAME END AS ACCODE,AD.TRANTYPE"
            strSql += vbCrLf + " ,0 PCS,SUM(WEIGHT) GRSWT,SUM(WEIGHT) NETWT"
            strSql += vbCrLf + " ,NULL AS STNPCS,NULL AS STNWT,NULL AS DIAPCS,NULL AS DIAWT,NULL AS PREPCS,NULL AS PREWT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ALLOYDETAILS AS AD"
            strSql += vbCrLf + " LEFT JOIN " & cnStockDb & "..ISSUE AS I ON AD.ISSSNO=I.SNO"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO AS CINF ON I.BATCHNO = CINF.BATCHNO"
            strSql += vbCrLf + " LEFT JOIN  " & cnAdminDb & "..PERSONALINFO AS PINF ON CINF.PSNO = PINF.SNO"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD AS A ON I.ACCODE = A.ACCODE"
            strSql += vbCrLf + " WHERE AD.TRANDATE <= '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
                strSql += " AND ISNULL(I.ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
            End If
            If TranType <> "" Then strSql += vbCrLf + " AND AD.TRANTYPE IN (" & TranType & ")"
            If Apptrantype <> "" Then strSql += vbCrLf + " AND AD.TRANTYPE NOT IN (" & Apptrantype & ")"
            If Misctrantype <> "" Then strSql += vbCrLf + " AND AD.TRANTYPE NOT IN (" & Misctrantype & ")"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            If ChkRepairDet.Checked = False Then strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = I.BATCHNO AND TRANTYPE = 'RD')"
            strSql += vbCrLf + " GROUP BY ITEMID,SUBITEMID,AD.TRANNO,REFNO,AD.TRANDATE,AD.BATCHNO,CATCODE,I.ACCODE,PINF.PNAME ,AD.TRANTYPE,ACNAME"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = " "
        strSql += vbCrLf + " "
        strSql += vbCrLf + " SELECT ITEMID,SUBITEMID,REFNO,TRANNO,I.TRANDATE,I.BATCHNO,CATCODE,CASE WHEN ISNULL(I.ACCODE,'') ='' then pinf.PNAME else A.ACNAME end as ACCODE,TRANTYPE"
        'strSql += vbCrLf + " ,CASE   WHEN I.TRANTYPE = 'PU' THEN  'PURCHASE'"
        'strSql += vbCrLf + " WHEN I.TRANTYPE = 'RPU' THEN 'PURCHASE'"
        'strSql += vbCrLf + " WHEN I.TRANTYPE = 'RRE' THEN 'PUR REC'"
        'strSql += vbCrLf + "  else 'OTHERS' end "
        'strSql += vbCrLf + " as TRANTYPE"
        strSql += vbCrLf + " ,SUM(PCS) PCS,SUM(GRSWT) GRSWT,SUM(NETWT) NETWT"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREPCS"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
        strSql += vbCrLf + " INTO TEMPTABLEDB..TEMP" & systemId & "REC FROM " & cnStockDb & "..RECEIPT AS I"
        strSql += vbCrLf + " left join " & cnAdminDb & "..CUSTOMERINFO as cinf on i.batchno = cinf.batchno"
        strSql += vbCrLf + " left join  " & cnAdminDb & "..personalinfo as pinf on cinf.psno = pinf.sno"
        strSql += vbCrLf + " left join " & cnAdminDb & "..Achead as A on i.accode = a.accode"
        strSql += vbCrLf + " WHERE I.TRANTYPE <> 'RRC' AND I.TRANDATE <= '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
        If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
            strSql += " AND ISNULL(I.ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
        End If
        'strSql += vbCrLf + " AND I.TRANFLAG NOT IN (" & EXCLTRANFLAG & ")"
        If TranType <> "" Then strSql += vbCrLf + " AND I.TRANTYPE IN (" & TranType & ")"
        If Apptrantype <> "" Then strSql += vbCrLf + " AND I.TRANTYPE NOT IN (" & Apptrantype & ")"
        If Misctrantype <> "" Then strSql += vbCrLf + " AND I.TRANTYPE NOT IN (" & Misctrantype & ")"
        If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        If ChkRepairDet.Checked = False Then strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO = I.BATCHNO AND TRANTYPE = 'RD')"
        strSql += vbCrLf + " group by ITEMID,SUBITEMID,TRANNO,REFNO,I.TRANDATE,I.BATCHNO,CATCODE,i.ACCODE,I.SNO,pinf.PNAME ,TRANTYPE,ACNAME"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = " SELECT DISTINCT BATCHNO INTO TEMPTABLEDB..TEMP" & systemId & "ORDREP_DEL "
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
        strSql += vbCrLf + " WHERE TRANTYPE IN ('OD') AND ISNULL(CANCEL,'') = ''"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()


        strSql = " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK"
        strSql += " ("
        strSql += " PARTICULAR VARCHAR(500)"
        strSql += vbCrLf + " ,TRANNO VARCHAR(20),TTRANDATE VARCHAR(10)"
        strSql += vbCrLf + " ,TRANTYPE varchar(50)"
        If rdbBothwt.Checked Then
            strSql += vbCrLf + " ,[RECEIPT PCS] INT,[RECEIPT " & bothWeight(0) & "] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[RECEIPT " & bothWeight(1) & "] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[RECEIPT STNPCS] INT,[RECEIPT STNWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[RECEIPT DIAPCS] INT,[RECEIPT DIAWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[RECEIPT PREPCS] INT,[RECEIPT PREWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[ISSUE PCS] INT,[ISSUE " & bothWeight(0) & "] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[ISSUE " & bothWeight(1) & "] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[ISSUE STNPCS] INT,[ISSUE STNWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[ISSUE DIAPCS] INT,[ISSUE DIAWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[ISSUE PREPCS] INT,[ISSUE PREWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[CLOSING PCS] INT,[CLOSING " & bothWeight(0) & "] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[CLOSING " & bothWeight(1) & "] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[CLOSING STNPCS] INT,[CLOSING STNWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[CLOSING DIAPCS] INT,[CLOSING DIAWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[CLOSING PREPCS] INT,[CLOSING PREWT] NUMERIC(15,3)"
        Else
            strSql += vbCrLf + " ,[RECEIPT PCS] INT,[RECEIPT " & Weight & "] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[RECEIPT STNPCS] INT,[RECEIPT STNWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[RECEIPT DIAPCS] INT,[RECEIPT DIAWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[RECEIPT PREPCS] INT,[RECEIPT PREWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[ISSUE PCS] INT,[ISSUE " & Weight & "] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[ISSUE STNPCS] INT,[ISSUE STNWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[ISSUE DIAPCS] INT,[ISSUE DIAWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[ISSUE PREPCS] INT,[ISSUE PREWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[CLOSING PCS] INT,[CLOSING " & Weight & "] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[CLOSING STNPCS] INT,[CLOSING STNWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[CLOSING DIAPCS] INT,[CLOSING DIAWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[CLOSING PREPCS] INT,[CLOSING PREWT] NUMERIC(15,3)"
        End If

        strSql += vbCrLf + " ,CATNAME VARCHAR(100),METAL VARCHAR(100),GS VARCHAR(100)"
        strSql += vbCrLf + " ,TRANDATE SMALLDATETIME,ACNAME VARCHAR(200)"
        strSql += vbCrLf + " ,ITEMNAME VARCHAR(80)"
        strSql += vbCrLf + " ,RESULT INT,COLHEAD VARCHAR(3)"
        strSql += vbCrLf + " )"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = " CREATE TABLE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SEP VARCHAR(10),TRANNO VARCHAR(20),"
        strSql += vbCrLf + " TRANTYPE VARCHAR(50),"
        strSql += vbCrLf + " PCS Int,"
        If rdbBothwt.Checked Then
            strSql += vbCrLf + bothWeight(0) & " NUMERIC(15,3),"
            strSql += vbCrLf + bothWeight(1) & " NUMERIC(15,3)"
        Else
            strSql += vbCrLf + "" & Weight & " NUMERIC(15,3)"
        End If
        strSql += vbCrLf + " ,STNPCS INT,STNWT NUMERIC(15,3),DIAPCS INT,DIAWT NUMERIC(15,3),PREPCS INT,PREWT NUMERIC(15,3)"
        strSql += vbCrLf + " ,CATNAME VARCHAR(100),METAL VARCHAR(100),GS VARCHAR(100)"
        strSql += vbCrLf + " ,TRANDATE SMALLDATETIME,ACNAME VARCHAR(200)"
        strSql += vbCrLf + " ,ITEMNAME VARCHAR(80)"
        strSql += vbCrLf + " )"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = " /** INSERTING OPENING FROM OPENWEIGHT **/"
        strSql += vbCrLf + " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
        strSql += vbCrLf + "  	SELECT "
        strSql += vbCrLf + "  	CASE WHEN I.TRANTYPE = 'I' THEN 'ISS' ELSE 'REC' END SEP"
        strSql += vbCrLf + "  	,NULL TRANNO"
        strSql += vbCrLf + " ,'OP'TRANTYPE"
        strSql += vbCrLf + " ,SUM(CASE WHEN (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.PCS ELSE 0 END) AS PCS"
        If rdbBothwt.Checked Then
            strSql += vbCrLf + "    ,SUM(CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & bothWeight(0) & " ELSE 0 END) AS " & bothWeight(0) & ""
            strSql += vbCrLf + "    ,SUM(CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & bothWeight(1) & " ELSE 0 END) AS " & bothWeight(1) & ""
        Else
            strSql += vbCrLf + "    ,SUM(CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('W','B')) THEN I." & Weight & " ELSE 0 END) AS " & Weight & ""
        End If
        strSql += vbCrLf + "    ,SUM(CASE WHEN CA.DIASTNTYPE='T' THEN I.PCS ELSE 0 END) AS STNPCS"
        strSql += vbCrLf + "    ,SUM(CASE WHEN CA.DIASTNTYPE='T' THEN I.GRSWT ELSE 0 END) AS STNWT"
        strSql += vbCrLf + "    ,SUM(CASE WHEN CA.DIASTNTYPE='D' THEN I.PCS ELSE 0 END) AS DIAPCS"
        strSql += vbCrLf + "    ,SUM(CASE WHEN CA.DIASTNTYPE='D' THEN I.GRSWT ELSE 0 END) AS DIAWT"
        strSql += vbCrLf + "    ,SUM(CASE WHEN CA.DIASTNTYPE='P' THEN I.PCS ELSE 0 END) AS PREPCS"
        strSql += vbCrLf + "    ,SUM(CASE WHEN CA.DIASTNTYPE='P' THEN I.GRSWT ELSE 0 END) AS PREWT"
        strSql += vbCrLf + "    ,CA.CATNAME,ME.METALNAME AS METAL"
        strSql += vbCrLf + "    ,CASE WHEN CA.GS11 = 'Y' THEN '" & Gs11Name & "' WHEN CA.GS12 = 'Y' THEN '" & Gs12Name & "' ELSE 'OTHER' END AS GS,NULL TRANDATE,NULL AS ACNAME,NULL ITEMNAME"
        strSql += vbCrLf + "  	FROM " & cnStockDb & "..OPENWEIGHT I "
        strSql += vbCrLf + "    LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID "
        strSql += vbCrLf + "    LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
        strSql += vbCrLf + "    INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE"
        strSql += vbCrLf + "    INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID"
        strSql += vbCrLf + "  	WHERE I.STOCKTYPE = 'C'"
        strSql += vbCrLf + "    AND I.COMPANYID = '" & strCompanyId & "'"
        If (ChkWithApproval.Checked = False And chkCmbTransation.Text = "ALL") Then
            strSql += vbCrLf + " AND ISNULL(I.APPROVAL,'')<>'A'"
        ElseIf chkCmbTransation.Text = "APPROVAL" Then
            strSql += vbCrLf + " AND ISNULL(I.APPROVAL,'')='A'"
        End If
        If chkCmbTransation.Text = "INTERNAL" Then
            strSql += vbCrLf + " AND ISNULL(I.APPROVAL,'')='I'"
        End If
        If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
            strSql += " AND ISNULL(I.ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
        End If
        'If TranType <> "" Then strSql += vbCrLf + " AND I.TRANTYPE IN (" & TranType & ")"
        If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkMetal <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetal & ")))"
        If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        strSql += vbCrLf + "    GROUP BY I.TRANTYPE,CA.CATNAME,CA.GS11,CA.GS12,ME.METALNAME"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        'InsGsOutStDt("O", "INSERTING OPENING FROM OUTSTANDING")
        'InsGsOutStDt("T", "INSERTING TRANSACTION FROM OUTSTANDING")
        If ChkWithOrRecpay.Checked Then InsGsOutStRecPay("T", "INSERTING OPENING FROM OUTSTANDING")
        InsGsIssRecPrint("O", "I", False, "INSERTING OPENING FROM ISSUE WITHOUT ORDREP INFO")
        InsGsIssRecPrint("O", "R", False, "INSERTING OPENING FROM RECEIPT WITHOUT ORDREP INFO")
        InsGsIssRecPrint("O", "I", True, "INSERTING OPENING ORDREP INFO FROM ISSUE")
        InsGsIssRecPrint("O", "R", True, "INSERTING OPENING ORDREP INFO FROM RECEIPT")
        InsGsStonePrint("O", "I", "INSERTING OPENING FROM ISSUESTONE")
        InsGsStonePrint("O", "R", "INSERTING OPENING FROM RECEIPTSTONE")
        InsGsIssRecPrint("T", "I", False, "INSERTING TRANSACTION FROM ISSUE WITHOUT ORDREP INFO")
        InsGsIssRecPrint("T", "R", False, "INSERTING TRANSACTION FROM RECEIPT WITHOUT ORDREP INFO")
        InsGsIssRecPrint("T", "I", True, "INSERTING TRANSACTION ORDREP INFO FROM ISSUE")
        InsGsIssRecPrint("T", "R", True, "INSERTING TRANSACTION ORDREP INFO FROM RECEIPT")
        InsGsStonePrint("T", "I", "INSERTING TRANSACTION FROM ISSUESTONE")
        InsGsStonePrint("T", "R", "INSERTING TRANSACTION FROM RECEIPTSTONE")


        strSql = " INSERT INTO TEMPTABLEDB..TEMP" & systemId & "GSSTOCK"
        strSql += " (PARTICULAR "
        strSql += vbCrLf + " ,TRANNO,TTRANDATE"
        strSql += vbCrLf + " ,TRANTYPE"
        strSql += vbCrLf + " ,[RECEIPT PCS],"
        If rdbBothwt.Checked Then
            strSql += vbCrLf + " [RECEIPT " & bothWeight(0) & "] "
            strSql += vbCrLf + " ,[RECEIPT " & bothWeight(1) & "] "
            strSql += vbCrLf + " ,[RECEIPT STNPCS] ,[RECEIPT STNWT] "
            strSql += vbCrLf + " ,[RECEIPT DIAPCS] ,[RECEIPT DIAWT] "
            strSql += vbCrLf + " ,[RECEIPT PREPCS] ,[RECEIPT PREWT] "
            strSql += vbCrLf + " ,[ISSUE PCS] ,[ISSUE " & bothWeight(0) & "] "
            strSql += vbCrLf + " ,[ISSUE " & bothWeight(1) & "] "
            strSql += vbCrLf + " ,[ISSUE STNPCS] ,[ISSUE STNWT] "
            strSql += vbCrLf + " ,[ISSUE DIAPCS] ,[ISSUE DIAWT] "
            strSql += vbCrLf + " ,[ISSUE PREPCS] ,[ISSUE PREWT] "
            strSql += vbCrLf + " ,[CLOSING PCS] ,[CLOSING " & bothWeight(0) & "] "
            strSql += vbCrLf + " ,[CLOSING " & bothWeight(1) & "] "
            strSql += vbCrLf + " ,[CLOSING STNPCS] ,[CLOSING STNWT] "
            strSql += vbCrLf + " ,[CLOSING DIAPCS] ,[CLOSING DIAWT] "
            strSql += vbCrLf + " ,[CLOSING PREPCS] ,[CLOSING PREWT] "
        Else
            strSql += vbCrLf + " [RECEIPT " & Weight & "] "
            strSql += vbCrLf + " ,[RECEIPT STNPCS] ,[RECEIPT STNWT] "
            strSql += vbCrLf + " ,[RECEIPT DIAPCS] ,[RECEIPT DIAWT] "
            strSql += vbCrLf + " ,[RECEIPT PREPCS] ,[RECEIPT PREWT] "
            strSql += vbCrLf + " ,[ISSUE PCS] ,[ISSUE " & Weight & "] "
            strSql += vbCrLf + " ,[ISSUE STNPCS] ,[ISSUE STNWT] "
            strSql += vbCrLf + " ,[ISSUE DIAPCS] ,[ISSUE DIAWT] "
            strSql += vbCrLf + " ,[ISSUE PREPCS] ,[ISSUE PREWT] "
            strSql += vbCrLf + " ,[CLOSING PCS] ,[CLOSING " & Weight & "] "
            strSql += vbCrLf + " ,[CLOSING STNPCS] ,[CLOSING STNWT] "
            strSql += vbCrLf + " ,[CLOSING DIAPCS] ,[CLOSING DIAWT] "
            strSql += vbCrLf + " ,[CLOSING PREPCS] ,[CLOSING PREWT] "

        End If
        strSql += vbCrLf + " ,CATNAME,METAL,GS,TRANDATE,ACNAME,ITEMNAME,RESULT,COLHEAD)"
        strSql += vbCrLf + " SELECT * FROM "
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SELECT"
        strSql += vbCrLf + " CONVERT(VARCHAR(500),NULL) PARTICULAR"
        strSql += vbCrLf + " ,TRANNO"
        strSql += vbCrLf + " ,CONVERT(VARCHAR,TRANDATE,103)TTRANDATE"
        strSql += vbCrLf + " ,TRANTYPE"
        strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PCS ELSE 0 END) AS [RECEIPT PCS]"
        If rdbBothwt.Checked Then
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN " & bothWeight(0) & " ELSE 0 END) AS [RECEIPT " & bothWeight(0) & "]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN " & bothWeight(1) & " ELSE 0 END) AS [RECEIPT " & bothWeight(1) & "]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNPCS ELSE 0 END) AS [RECEIPT STNPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNWT ELSE 0 END) AS [RECEIPT STNWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN DIAPCS ELSE 0 END) AS [RECEIPT DIAPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN DIAWT ELSE 0 END) AS [RECEIPT DIAWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PREPCS ELSE 0 END) AS [RECEIPT PREPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PREWT ELSE 0 END) AS [RECEIPT PREWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PCS ELSE 0 END) AS [ISSUE PCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN " & bothWeight(0) & " ELSE 0 END) AS [ISSUE " & bothWeight(0) & "]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN " & bothWeight(1) & " ELSE 0 END) AS [ISSUE " & bothWeight(1) & "]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNPCS ELSE 0 END) AS [ISSUE STNPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNWT ELSE 0 END) AS [ISSUE STNWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN DIAPCS ELSE 0 END) AS [ISSUE DIAPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN DIAWT ELSE 0 END) AS [ISSUE DIAWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PREPCS ELSE 0 END) AS [ISSUE PREPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PREWT ELSE 0 END) AS [ISSUE PREWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*PCS ELSE PCS END) AS [CLOSING PCS]"
            'strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*PCS ELSE " & bothWeight(1) & " END) AS [CLOSING PCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*" & bothWeight(0) & " ELSE " & bothWeight(0) & " END) AS [CLOSING " & bothWeight(0) & "]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*" & bothWeight(1) & " ELSE " & bothWeight(1) & " END) AS [CLOSING " & bothWeight(1) & "]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*STNPCS ELSE 0 END) AS [CLOSING STNPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*STNWT ELSE 0 END) AS [CLOSING STNWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*DIAPCS ELSE 0 END) AS [CLOSING DIAPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*DIAWT ELSE 0 END) AS [CLOSING DIAWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*PREPCS ELSE 0 END) AS [CLOSING PREPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*PREWT ELSE 0 END) AS [CLOSING PREWT]"
        Else
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN " & Weight & " ELSE 0 END) AS [RECEIPT " & Weight & "]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNPCS ELSE 0 END) AS [RECEIPT STNPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN STNWT ELSE 0 END) AS [RECEIPT STNWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN DIAPCS ELSE 0 END) AS [RECEIPT DIAPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN DIAWT ELSE 0 END) AS [RECEIPT DIAWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PREPCS ELSE 0 END) AS [RECEIPT PREPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN PREWT ELSE 0 END) AS [RECEIPT PREWT]"

            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PCS ELSE 0 END) AS [ISSUE PCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN " & Weight & " ELSE 0 END) AS [ISSUE " & Weight & "]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNPCS ELSE 0 END) AS [ISSUE STNPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN STNWT ELSE 0 END) AS [ISSUE STNWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN DIAPCS ELSE 0 END) AS [ISSUE DIAPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN DIAWT ELSE 0 END) AS [ISSUE DIAWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PREPCS ELSE 0 END) AS [ISSUE PREPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN PREWT ELSE 0 END) AS [ISSUE PREWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*PCS ELSE PCS END) AS [CLOSING PCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*" & Weight & " ELSE " & Weight & " END) AS [CLOSING " & Weight & "]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*STNPCS ELSE STNPCS END) AS [CLOSING STNPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*STNWT ELSE STNWT END) AS [CLOSING STNWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*DIAPCS ELSE DIAPCS END) AS [CLOSING DIAPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*DIAWT ELSE DIAWT END) AS [CLOSING DIAWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*PREPCS ELSE PREPCS END) AS [CLOSING PREPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'ISS' THEN -1*PREWT ELSE PREWT END) AS [CLOSING PREWT]"
        End If
        If chkCategorywise.Checked = False Then
            strSql += vbCrLf + " ," & IIf(rbtPcs.Checked, "CATNAME", "NULL") & " AS CATNAME"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(100),NULL) AS METAL"
            strSql += vbCrLf + " ," & IIf(rbtPcs.Checked, "CONVERT(vARCHAR,NULL)AS GS", "GS") & ",TRANDATE,ACNAME"
            strSql += vbCrLf + " ," & IIf(rbtPcs.Checked, "ITEMNAME", "NULL") & " AS ITEMNAME,1 AS RESULT,NULL COLHEAD"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " GROUP BY TRANNO,TRANDATE,TRANTYPE,ACNAME,GS"
            If rbtPcs.Checked Then
                strSql += vbCrLf + ",CATNAME,GS,ITEMNAME"
            End If
        ElseIf chkCategorywise.Checked Then
            strSql += vbCrLf + " , CATNAME"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(100),NULL) AS METAL"
            strSql += vbCrLf + " ,GS,TRANDATE,ACNAME"
            strSql += vbCrLf + " , ITEMNAME,1 AS RESULT,NULL COLHEAD"
            strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " GROUP BY TRANNO,TRANDATE,TRANTYPE,ACNAME,GS"
            strSql += vbCrLf + ",CATNAME,GS,ITEMNAME"
        End If
        strSql += vbCrLf + " )X"

        If rbtBoth.Checked Then
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " WHERE NOT ([RECEIPT " & bothWeight(0) & "] = 0 AND [RECEIPT " & bothWeight(1) & "] = 0 "
                strSql += vbCrLf + " AND [ISSUE " & bothWeight(0) & "] = 0 AND [ISSUE " & bothWeight(1) & "] = 0 AND [RECEIPT PCS] = 0 AND [ISSUE PCS] = 0)"
            Else
                strSql += vbCrLf + " WHERE NOT ([RECEIPT " & Weight & "] = 0 AND [ISSUE " & Weight & "] = 0 AND [RECEIPT PCS] = 0 AND [ISSUE PCS] = 0)"
            End If

        ElseIf rbtWeight.Checked Then
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " WHERE NOT ([RECEIPT " & bothWeight(0) & "] = 0 AND [RECEIPT " & bothWeight(1) & "] = 0 AND [ISSUE " & bothWeight(0) & "] = 0 AND [ISSUE " & bothWeight(1) & "] = 0)"
            Else
                strSql += vbCrLf + " WHERE NOT ([RECEIPT " & Weight & "] = 0 AND [ISSUE " & Weight & "] = 0)"
            End If

        Else
            strSql += vbCrLf + " WHERE NOT ([RECEIPT PCS] = 0 AND [ISSUE PCS] = 0)"
            ''PCS
        End If
        strSql += vbCrLf + "  ORDER BY GS,CATNAME,ITEMNAME,TRANDATE"

        If chkOrderbyTranno.Checked And chkOrderbyTranno.Visible Then
            strSql += vbCrLf + " ,TRANNO"
        Else
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ,[RECEIPT " & bothWeight(0) & "] DESC,[RECEIPT PCS] DESC,TRANNO"
            Else
                strSql += vbCrLf + " ,[RECEIPT " & Weight & "] DESC,[RECEIPT PCS] DESC,TRANNO"
            End If

        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()


        strSql = " UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET PARTICULAR ='OPENING..' WHERE RESULT = 1 AND TRANDATE IS NULL"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET PARTICULAR = '   '+ACNAME WHERE RESULT = 1 AND TRANDATE IS NOT NULL"

        If rdbBothwt.Checked Then
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE " & bothWeight(0) & "]=0, "
            strSql += vbCrLf + " [RECEIPT " & bothWeight(0) & "] = [RECEIPT " & bothWeight(0) & "]-[ISSUE " & bothWeight(0) & "] WHERE  RESULT = 1 AND TRANTYPE='OP'"
            strSql += vbCrLf + " AND [RECEIPT " & bothWeight(0) & "]>=[ISSUE " & bothWeight(0) & "]"
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [RECEIPT " & bothWeight(0) & "]=0,"
            strSql += vbCrLf + " [ISSUE " & bothWeight(0) & "] = [ISSUE " & bothWeight(0) & "]-[RECEIPT " & bothWeight(0) & "] WHERE  RESULT = 1 AND TRANTYPE='OP'"
            strSql += vbCrLf + " AND [RECEIPT " & bothWeight(0) & "]<=[ISSUE " & bothWeight(0) & "]"
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE " & bothWeight(1) & "]=0,"
            strSql += vbCrLf + " [RECEIPT " & bothWeight(1) & "] = [RECEIPT " & bothWeight(1) & "]-[ISSUE " & bothWeight(1) & "] WHERE  RESULT = 1 AND TRANTYPE='OP'"
            strSql += vbCrLf + " AND [RECEIPT " & bothWeight(1) & "]>=[ISSUE " & bothWeight(1) & "]"
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [RECEIPT " & bothWeight(1) & "]=0,"
            strSql += vbCrLf + " [ISSUE " & bothWeight(1) & "] = [ISSUE " & bothWeight(1) & "]-[RECEIPT " & bothWeight(1) & "] WHERE  RESULT = 1 AND TRANTYPE='OP'"
            strSql += vbCrLf + " AND [RECEIPT " & bothWeight(1) & "]<=[ISSUE " & bothWeight(1) & "]"
        Else
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE " & Weight & "]=0, "
            strSql += vbCrLf + " [RECEIPT " & Weight & "] = [RECEIPT " & Weight & "]-[ISSUE " & Weight & "] WHERE  RESULT = 1 AND TRANTYPE='OP'"
            strSql += vbCrLf + " AND [RECEIPT " & Weight & "]>=[ISSUE " & Weight & "]"
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [RECEIPT " & Weight & "]=0,"
            strSql += vbCrLf + " [ISSUE " & Weight & "] = [ISSUE " & Weight & "]-[RECEIPT " & Weight & "] WHERE  RESULT = 1 AND TRANTYPE='OP'"
            strSql += vbCrLf + " AND [RECEIPT " & Weight & "]<=[ISSUE " & Weight & "]"

        End If

        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [RECEIPT PCS] = NULL WHERE [RECEIPT PCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE PCS] = NULL WHERE [ISSUE PCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [CLOSING PCS] = NULL WHERE [CLOSING PCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [RECEIPT STNPCS] = NULL WHERE [RECEIPT STNPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE STNPCS] = NULL WHERE [ISSUE STNPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [CLOSING STNPCS] = NULL WHERE [CLOSING STNPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [RECEIPT DIAPCS] = NULL WHERE [RECEIPT DIAPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE DIAPCS] = NULL WHERE [ISSUE DIAPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [CLOSING DIAPCS] = NULL WHERE [CLOSING DIAPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [RECEIPT PREPCS] = NULL WHERE [RECEIPT PREPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE PREPCS] = NULL WHERE [ISSUE PREPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [CLOSING PREPCS] = NULL WHERE [CLOSING PREPCS] = 0"
        If rdbBothwt.Checked Then
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [RECEIPT " & bothWeight(0) & "] = NULL WHERE [RECEIPT " & bothWeight(0) & "] = 0"
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [RECEIPT " & bothWeight(1) & "] = NULL WHERE [RECEIPT " & bothWeight(1) & "] = 0"
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE " & bothWeight(0) & "] = NULL WHERE [ISSUE " & bothWeight(0) & "] = 0"
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE " & bothWeight(1) & "] = NULL WHERE [ISSUE " & bothWeight(1) & "] = 0"
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [CLOSING " & bothWeight(0) & "] = NULL WHERE [CLOSING " & bothWeight(0) & "] = 0"
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [CLOSING " & bothWeight(1) & "] = NULL WHERE [CLOSING " & bothWeight(1) & "] = 0"
        Else
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [RECEIPT " & Weight & "] = NULL WHERE [RECEIPT " & Weight & "] = 0"
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE " & Weight & "] = NULL WHERE [ISSUE " & Weight & "] = 0"
            strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [CLOSING " & Weight & "] = NULL WHERE [CLOSING " & Weight & "] = 0"
        End If
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [RECEIPT STNWT] = NULL WHERE [RECEIPT STNWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE STNWT] = NULL WHERE [ISSUE STNWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [CLOSING STNWT] = NULL WHERE [CLOSING STNWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [RECEIPT DIAWT] = NULL WHERE [RECEIPT DIAWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE DIAWT] = NULL WHERE [ISSUE DIAWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [CLOSING DIAWT] = NULL WHERE [CLOSING DIAWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [RECEIPT PREWT] = NULL WHERE [RECEIPT PREWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE PREWT] = NULL WHERE [ISSUE PREWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [CLOSING PREWT] = NULL WHERE [CLOSING PREWT] = 0"

        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        Prop_Sets()

        Dim dtGrid As New DataTable("SUMMARY")
        dtGrid.Columns.Add("TABLENAME", GetType(String))
        dtGrid.Columns("TABLENAME").DefaultValue = "SUMMARY"
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        If chkSpecificFormat.Checked = False Then
            strSql = " SELECT * FROM TEMPTABLEDB..TEMP" & systemId & "GSSTOCK "
            strSql += vbCrLf + "  ORDER BY METAL,GS,CATNAME,ITEMNAME,TRANDATE,RESULT"
            If chkOrderbyTranno.Checked And chkOrderbyTranno.Visible Then
                strSql += vbCrLf + " ,TRANNO"
            Else
                If rdbBothwt.Checked Then
                    strSql += vbCrLf + " ,[RECEIPT " & bothWeight(0) & "] DESC"
                Else
                    strSql += vbCrLf + " ,[RECEIPT " & Weight & "] DESC"
                End If
                strSql += vbCrLf + " ,[RECEIPT PCS] DESC,TRANNO"
            End If
        Else
            If rdbBothwt.Checked Then
                strSql = "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET "
                strSql += vbCrLf + " [RECEIPT " & bothWeight(0) & "] = [CLOSING " & bothWeight(0) & "],[ISSUE " & bothWeight(0) & "]=0 WHERE PARTICULAR in (SELECT Particular "
                strSql += vbCrLf + " from TEMPTABLEDB..TEMP" & systemId & "GSSTOCK"
                strSql += vbCrLf + " GROUP BY Particular,[RECEIPT " & bothWeight(0) & "],[ISSUE " & bothWeight(0) & "]"
                strSql += vbCrLf + " HAVING [RECEIPT " & bothWeight(0) & "]>[ISSUE " & bothWeight(0) & "])"

                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [RECEIPT " & bothWeight(1) & "]=[CLOSING " & bothWeight(1) & "],[ISSUE " & bothWeight(1) & "]=0 WHERE PARTICULAR in (SELECT Particular "
                strSql += vbCrLf + " from TEMPTABLEDB..TEMP" & systemId & "GSSTOCK"
                strSql += vbCrLf + " GROUP BY Particular,[RECEIPT " & bothWeight(1) & "],[ISSUE " & bothWeight(1) & "]"
                strSql += vbCrLf + " HAVING [RECEIPT " & bothWeight(1) & "]>[ISSUE " & bothWeight(1) & "])"

                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE " & bothWeight(0) & "] = [CLOSING " & bothWeight(0) & "],[RECEIPT " & bothWeight(0) & "]=0 WHERE PARTICULAR in (SELECT Particular "
                strSql += vbCrLf + " from TEMPTABLEDB..TEMP" & systemId & "GSSTOCK"
                strSql += vbCrLf + " GROUP BY Particular,[RECEIPT " & bothWeight(0) & "],[ISSUE " & bothWeight(0) & "]"
                strSql += vbCrLf + " HAVING [RECEIPT " & bothWeight(0) & "]<[ISSUE " & bothWeight(0) & "])"

                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE " & bothWeight(1) & "] = [CLOSING " & bothWeight(1) & "],[RECEIPT " & bothWeight(1) & "]=0 WHERE PARTICULAR in (SELECT Particular "
                strSql += vbCrLf + " from TEMPTABLEDB..TEMP" & systemId & "GSSTOCK"
                strSql += vbCrLf + " GROUP BY Particular,[RECEIPT " & bothWeight(1) & "],[ISSUE " & bothWeight(1) & "]"
                strSql += vbCrLf + " HAVING [RECEIPT " & bothWeight(1) & "]<[ISSUE " & bothWeight(1) & "])"

                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            Else
                strSql = "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET "
                strSql += vbCrLf + " [RECEIPT " & Weight & "] = [CLOSING " & Weight & "],[ISSUE " & Weight & "]=0 WHERE PARTICULAR in (SELECT Particular "
                strSql += vbCrLf + " from TEMPTABLEDB..TEMP" & systemId & "GSSTOCK"
                strSql += vbCrLf + " GROUP BY Particular,[RECEIPT " & Weight & "],[ISSUE " & Weight & "]"
                strSql += vbCrLf + " HAVING [RECEIPT " & Weight & "]>[ISSUE " & Weight & "])"

                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()

                strSql = "  UPDATE TEMPTABLEDB..TEMP" & systemId & "GSSTOCK SET [ISSUE " & Weight & "] = [CLOSING " & Weight & "],[RECEIPT " & Weight & "]=0 WHERE PARTICULAR in (SELECT Particular "
                strSql += vbCrLf + " from TEMPTABLEDB..TEMP" & systemId & "GSSTOCK"
                strSql += vbCrLf + " GROUP BY Particular,[RECEIPT " & Weight & "],[ISSUE " & Weight & "]"
                strSql += vbCrLf + " HAVING [RECEIPT " & Weight & "]<[ISSUE " & Weight & "])"

                cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                cmd.ExecuteNonQuery()
            End If

            strSql = " Select convert(varchar(20),TranDate,103)as TranDate,TranNo,TRANTYPE,Particular,"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " [RECEIPT " & bothWeight(0) & "] as GWT,[RECEIPT " & bothWeight(1) & "] as NWT ,[ISSUE " & bothWeight(0) & "] as GWT,[ISSUE " & bothWeight(1) & "]as NWT "
            Else
                If rbtGrsWeight.Checked Then
                    strSql += vbCrLf + " [RECEIPT " & Weight & "] as GWT ,[ISSUE " & Weight & "] as GWT"
                Else
                    strSql += vbCrLf + " [RECEIPT " & Weight & "]as NWT ,[ISSUE " & Weight & "] as NWT "
                End If
            End If
            If rbtPcs.Checked Then
                strSql += vbCrLf + ",[RECEIPT PCS] as RECPCS ,[ISSUE PCS] as ISSPCS "
            End If
            strSql += vbCrLf + " ,GS from TEMPTABLEDB..TEMP" & systemId & "GSSTOCK "
            'strSql = " Select convert(varchar(20),TranDate,103)as TranDate,TranNo,Particular,[RECEIPT GRSWT] as GWT,[RECEIPT NETWT] as NWT,[ISSUE GRSWT] as GWT,[ISSUE NETWT] as NWT from TEMPTABLEDB..TEMP" & systemId & "GSSTOCK"
        End If
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)

        Dim CheckCol As String = "GS"
        Dim Checker As String = "##gs2932"
        Dim RunWt As Decimal = 0
        Dim RunWt1 As Decimal = 0
        Dim RunPcs As Integer = 0
        If chkSpecificFormat.Checked = False Then
            If rbtPcs.Checked Then CheckCol = "CATNAME"
            For Each Ro As DataRow In dtGrid.Rows
                If Ro.Item(CheckCol).ToString <> Checker Then
                    Checker = Ro.Item(CheckCol).ToString
                    RunWt = 0 : RunPcs = 0 : RunWt1 = 0
                End If
                If Ro.Item("COLHEAD").ToString <> "" Then Continue For
                RunPcs += Val(Ro.Item("RECEIPT PCS").ToString) - Val(Ro.Item("ISSUE PCS").ToString)
                If rdbBothwt.Checked Then
                    RunWt += Val(Ro.Item("RECEIPT " & bothWeight(0) & "").ToString) - Val(Ro.Item("ISSUE " & bothWeight(0) & "").ToString)
                    RunWt1 += Val(Ro.Item("RECEIPT " & bothWeight(1) & "").ToString) - Val(Ro.Item("ISSUE " & bothWeight(1) & "").ToString)
                Else
                    RunWt += Val(Ro.Item("RECEIPT " & Weight & "").ToString) - Val(Ro.Item("ISSUE " & Weight & "").ToString)
                End If
                Ro.Item("CLOSING PCS") = IIf(RunPcs <> 0, RunPcs, DBNull.Value)
                If rdbBothwt.Checked Then
                    Ro.Item("CLOSING " & bothWeight(0) & "") = IIf(RunWt <> 0, Format(RunWt, "0.000"), DBNull.Value)
                    Ro.Item("CLOSING " & bothWeight(1) & "") = IIf(RunWt1 <> 0, Format(RunWt1, "0.000"), DBNull.Value)
                Else
                    Ro.Item("CLOSING " & Weight & "") = IIf(RunWt <> 0, Format(RunWt, "0.000"), DBNull.Value)
                End If
            Next
        End If
        Dim totcnt As Integer
        totcnt = dtGrid.Rows.Count
        Dim str1, str2, str3, str4, str5, str6, str7, Str8, Str9 As String
        FileWrite = File.CreateText(Application.StartupPath + "\FilePrint.txt")
        PgNo = 0
        line = 0
        StrDate = ""
        SubTotalAmt = 0
        SubTotalEntry = 0
        funcPageNo()


        If dtGrid.Rows.Count > 0 Then

            For i As Integer = 0 To dtGrid.Rows.Count - 1
                'StrDate = dtGrid.Rows(i).Item("Trandate").ToString
                strprint = ""
                If StrDate <> "" Then
                    If Not StrDate = dtGrid.Rows(i).Item("TranDate").ToString Then
                        'If rdbBoth.Checked Then
                        strprint = "------------------------------------------------------------------------------------"
                        FileWrite.WriteLine(Trim(strprint))
                        line = line + 1
                        StrDate = dtGrid.Rows(i).Item("TranDate").ToString
                        If rbtPcs.Checked = False Then
                            strprint = IIf(dtGrid.Columns.Contains("GWT") = True, RSet(Format(SubTotalRGWT, "#0.000"), 12), "")
                            strprint = strprint & IIf(dtGrid.Columns.Contains("NWT") = True, RSet(Format(SubTOTRNWT, "#0.000"), 12), "")
                            strprint = strprint & IIf(dtGrid.Columns.Contains("GWT1") = True, RSet(Format(SubTOTIGWT, "#0.000"), 12), "")
                            strprint = strprint & IIf(dtGrid.Columns.Contains("NWT1") = True, RSet(Format(SubTOTINWT, "#0.000"), 12), "")
                        End If
                        If rbtPcs.Checked Then
                            strprint = IIf(dtGrid.Columns.Contains("RECPCS") = True, RSet(Format(SubTotalRPCS, "#0.000"), 12), "")
                            strprint = strprint & IIf(dtGrid.Columns.Contains("ISSPCS") = True, RSet(Format(SubTotalIPCS, "#0.000"), 12), "")
                        End If

                        strprint = LSet("Day Total ", 33) & strprint
                        FileWrite.WriteLine(Trim(strprint))
                        line = line + 1
                        strprint = "------------------------------------------------------------------------------------"
                        FileWrite.WriteLine(Trim(strprint))
                        line = line + 1
                        If line = 63 Then

                            strprint = "------------------------------------------------------------------------------------"
                            FileWrite.WriteLine(Trim(strprint))
                            strprint = Chr(12)
                            FileWrite.WriteLine(strprint)
                            funcPageNo()

                        End If
                        SubTotalRGWT = 0
                        SubTOTRNWT = 0
                        SubTOTIGWT = 0
                        SubTOTINWT = 0
                        SubTotalRPCS = 0
                        SubTotalIPCS = 0
                    End If
                End If

                StrDate = dtGrid.Rows(i).Item("Trandate").ToString

                'End If
                str1 = Space(6) : str2 = Space(6) : str3 = Space(15)
                If dtGrid.Rows(i).Item("TRANTYPE").ToString = "OP" Then str1 = (LSet("", 6))

                If dtGrid.Rows(i).Item("TRANNO").ToString <> "" Then str1 = (LSet(dtGrid.Rows(i).Item("TRANNO"), 6))

                'If dtGrid.Rows(i).Item("TRANTYPE").ToString <> "" Then str2 = (LSet(dtGrid.Rows(i).Item("TRANTYPE"), 12))
                str2 = (LSet(dtGrid.Rows(i).Item("TRANTYPE"), 12))
                str3 = Space(15)
                If dtGrid.Rows(i).Item("PARTICULAR").ToString <> "" Then str3 = LSet(dtGrid.Rows(i).Item("PARTICULAR"), 15)
                If rbtPcs.Checked = False Then
                    If dtGrid.Columns.Contains("GWT") Then
                        str4 = Space(12)
                        If dtGrid.Rows(i).Item("GWT").ToString <> "" Then str4 = RSet(dtGrid.Rows(i).Item("GWT"), 12)
                    End If
                    If dtGrid.Columns.Contains("NWT") Then
                        str5 = Space(12)
                        If dtGrid.Rows(i).Item("NWT").ToString <> "" Then str5 = RSet(dtGrid.Rows(i).Item("NWT"), 12)
                    End If
                    If dtGrid.Columns.Contains("GWT1") Then
                        str6 = Space(12)
                        If dtGrid.Rows(i).Item("GWT1").ToString <> "" Then str6 = RSet(dtGrid.Rows(i).Item("GWT1"), 12)
                    End If
                    If dtGrid.Columns.Contains("NWT1") Then
                        str7 = Space(12)
                        If dtGrid.Rows(i).Item("NWT1").ToString <> "" Then str7 = RSet(dtGrid.Rows(i).Item("NWT1"), 12)
                    End If
                End If
                If rbtPcs.Checked Then
                    If dtGrid.Columns.Contains("RECPCS") Then
                        Str8 = Space(12)
                        If dtGrid.Rows(i).Item("RECPCS").ToString <> "" Then Str8 = RSet(dtGrid.Rows(i).Item("RECPCS"), 12)
                    End If
                    If dtGrid.Columns.Contains("ISSPCS") Then
                        Str9 = Space(12)
                        If dtGrid.Rows(i).Item("ISSPCS").ToString <> "" Then Str9 = RSet(dtGrid.Rows(i).Item("ISSPCS"), 12)
                    End If
                End If

                If sampledate = "" Then
                    sampledate = dtGrid.Rows(i).Item("Trandate").ToString
                    Dim sampleDate1 As String = sampledate
                    FileWrite.WriteLine(Trim(sampleDate1))
                    line += 1
                ElseIf Not sampledate = dtGrid.Rows(i).Item("Trandate").ToString Then
                    sampledate = dtGrid.Rows(i).Item("Trandate").ToString
                    FileWrite.WriteLine(Trim(sampledate))
                    line += 1
                End If
                If rbtPcs.Checked = False Then
                    strprint = str1 + str2 + str3 + str4 + str5 + str6 + str7
                End If
                If rbtPcs.Checked Then
                    strprint = str1 + str2 + str3 + Str8 + Str9
                End If

                FileWrite.WriteLine(strprint)
                line += 1
                'FileWrite.WriteLine()
                If line = 63 Then
                    strprint = "------------------------------------------------------------------------------------"
                    FileWrite.WriteLine(Trim(strprint))
                    strprint = Chr(12)
                    FileWrite.WriteLine(strprint)
                    funcPageNo()
                End If
                'If Val(collect.Rows.Count - 1) < Val(collect.Rows(i + 1)) Then
                'If Not StrDate(i) = collect.Rows(i + 1).Item("Rdate").ToString Then GoTo getend
                If StrDate <> "" Then
                    If StrDate = dtGrid.Rows(i).Item("TranDate").ToString Then
                        If rbtPcs.Checked = False Then
                            If dtGrid.Columns.Contains("GWT") Then SubTotalRGWT = SubTotalRGWT + Val("" & dtGrid.Rows(i).Item("GWT"))
                            If dtGrid.Columns.Contains("GWT1") Then SubTOTIGWT = SubTOTIGWT + Val("" & dtGrid.Rows(i).Item("GWT1"))
                            If dtGrid.Columns.Contains("NWT") Then SubTOTRNWT = SubTOTRNWT + Val("" & dtGrid.Rows(i).Item("NWT"))
                            If dtGrid.Columns.Contains("NWT1") Then SubTOTINWT = SubTOTINWT + Val("" & dtGrid.Rows(i).Item("NWT1"))
                        End If
                        If rbtPcs.Checked Then
                            If dtGrid.Columns.Contains("RECPCS") Then SubTotalRPCS = SubTotalRPCS + Val("" & dtGrid.Rows(i).Item("RECPCS"))
                            If dtGrid.Columns.Contains("ISSPCS") Then SubTotalIPCS = SubTotalIPCS + Val("" & dtGrid.Rows(i).Item("ISSPCS"))
                        End If
                    End If
                End If
                'IIf(dtGrid.Rows(i - 1).Item("PARTICULAR").ToString, "GS11", "GS12")
                If rbtPcs.Checked = False Then
                    If dtGrid.Columns.Contains("GWT") Then GrandTotRGWT = GrandTotRGWT + Val("" & dtGrid.Rows(i).Item("GWT"))
                    If dtGrid.Columns.Contains("GWT1") Then GrandTOTIGWT = GrandTOTIGWT + Val("" & dtGrid.Rows(i).Item("GWT1"))
                    If dtGrid.Columns.Contains("NWT") Then GrandTotRNWT = GrandTotRNWT + Val("" & dtGrid.Rows(i).Item("NWT"))
                    If dtGrid.Columns.Contains("NWT1") Then GrandTOTINWT = GrandTOTINWT + Val("" & dtGrid.Rows(i).Item("NWT1"))
                End If
                If rbtPcs.Checked Then
                    If dtGrid.Columns.Contains("RECPCS") Then GrandTotRPCS = GrandTotRPCS + Val("" & dtGrid.Rows(i).Item("RECPCS"))
                    If dtGrid.Columns.Contains("ISSPCS") Then GrandTotIPCS = GrandTotIPCS + Val("" & dtGrid.Rows(i).Item("ISSPCS"))
                End If
            Next
        End If

        If StrDate <> "" Then
            'If rdbBoth.Checked Then
            strprint = "----------------------------------------------------------------------------------"
            FileWrite.WriteLine(Trim(strprint))
            line = line + 1
            If rbtPcs.Checked = False Then
                strprint = IIf(dtGrid.Columns.Contains("GWT") = True, RSet(Format(SubTotalRGWT, "#0.000"), 12), "")
                strprint = strprint & IIf(dtGrid.Columns.Contains("NWT") = True, RSet(Format(SubTOTRNWT, "#0.000"), 12), "")
                strprint = strprint & IIf(dtGrid.Columns.Contains("GWT1") = True, RSet(Format(SubTOTIGWT, "#0.000"), 12), "")
                strprint = strprint & IIf(dtGrid.Columns.Contains("NWT1") = True, RSet(Format(SubTOTINWT, "#0.000"), 12), "")
            End If
            If rbtPcs.Checked Then
                strprint = IIf(dtGrid.Columns.Contains("RECPCS") = True, RSet(Format(SubTotalRPCS, "#0.000"), 12), "")
                strprint = strprint & IIf(dtGrid.Columns.Contains("ISSPCS") = True, RSet(Format(SubTotalIPCS, "#0.000"), 12), "")
            End If
            strprint = LSet("Day Total ", 33) + strprint
            FileWrite.WriteLine(Trim(strprint))
            line = line + 1
            strprint = "------------------------------------------------------------------------------------"
            FileWrite.WriteLine(Trim(strprint))
            line = line + 1
        End If
        If rbtPcs.Checked = False Then
            CLOTOTGWT = (GrandTotRGWT - GrandTOTIGWT)
            CLOTOTNWT = (GrandTotRNWT - GrandTOTINWT)
        End If
        If rbtPcs.Checked Then
            CLOTOTRPCS = (GrandTotRPCS - GrandTotIPCS)
        End If
        'FileWrite.WriteLine(Trim(strprint))
        If rbtPcs.Checked = False Then
            strprint = IIf(dtGrid.Columns.Contains("GWT") = True, RSet(Format(GrandTotRGWT, "#0.000"), 12), "")
            strprint = strprint & IIf(dtGrid.Columns.Contains("NWT") = True, RSet(Format(GrandTotRNWT, "#0.000"), 12), "")
            strprint = strprint & IIf(dtGrid.Columns.Contains("GWT1") = True, RSet(Format(GrandTOTIGWT, "#0.000"), 12), "")
            strprint = strprint & IIf(dtGrid.Columns.Contains("NWT1") = True, RSet(Format(GrandTOTINWT, "#0.000"), 12), "")
        End If
        If rbtPcs.Checked Then
            strprint = IIf(dtGrid.Columns.Contains("RECPCS") = True, RSet(Format(GrandTotRPCS, "#0.000"), 12), "")
            strprint = strprint & IIf(dtGrid.Columns.Contains("ISSPCS") = True, RSet(Format(GrandTotIPCS, "#0.000"), 12), "")
        End If

        strprint = LSet("Total Receipt & Issue ", 33) & strprint
        FileWrite.WriteLine(strprint)
        line = line + 1
        strprint = "------------------------------------------------------------------------------------"
        FileWrite.WriteLine(Trim(strprint))
        line = line + 1

        If rbtPcs.Checked = False Then
            strprint = LSet("Closing Stock", 33) + RSet(IIf(CLOTOTGWT > 0, Format(CLOTOTGWT, "#0.000"), ""), 12) + RSet(IIf(CLOTOTNWT > 0, Format(CLOTOTNWT, "#0.000"), ""), 12) + RSet(IIf(CLOTOTGWT < 0, Format(Math.Abs(CLOTOTGWT), "#0.000"), ""), 12) + RSet(IIf(CLOTOTNWT < 0, Format(Math.Abs(CLOTOTNWT), "#0.000"), ""), 12) + RSet(IIf(CLOTOTRPCS > 0, Format(Math.Abs(CLOTOTRPCS), "#0.000"), ""), 12)
        End If
        If rbtPcs.Checked Then
            If (CLOTOTRPCS < 0) Then
                strprint = LSet("Closing Stock", 45) + RSet(IIf(CLOTOTRPCS < 0, Format(CLOTOTRPCS, "#0.000"), ""), 12)
            End If
            If (CLOTOTRPCS > 0) Then
                strprint = LSet("Closing Stock", 33) + RSet(IIf(CLOTOTRPCS > 0, Format(CLOTOTRPCS, "#0.000"), ""), 12)
            End If
        End If
        FileWrite.WriteLine(strprint)
        line = line + 1
        strprint = "------------------------------------------------------------------------------------"
        FileWrite.WriteLine(Trim(strprint))
        line = line + 1
        strprint = String.Empty
        strprint = "                                                                   End of the Report"
        FileWrite.WriteLine(strprint)
        line = line + 1
        strprint = Chr(12)
        FileWrite.WriteLine(strprint)
        strprint = String.Empty
        FileWrite.Close()
        line += 1
        Dim frmPrinterSelect As New frmPrinterSelect
        frmPrinterSelect.Show()

    End Function

    Private Sub frmMetalOrnamentDetailedStockView_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        Dim gsnamestr() As String
        If gsformname <> "" Then gsnamestr = Split(gsformname, ",")
        If gsnamestr(0) <> "" Then Gs11Name = gsnamestr(0)
        If gsnamestr(1) <> "" Then Gs12Name = gsnamestr(1)
        Me.chkGs11.Text = Gs11Name
        Me.chkGs12.Text = Gs12Name
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
        strSql = " SELECT 'ALL' TRANS UNION ALL"
        strSql += vbCrLf + " SELECT 'GENERAL' TRANS UNION ALL"
        strSql += vbCrLf + " SELECT 'APPROVAL' TRANS UNION ALL"
        strSql += vbCrLf + " SELECT 'MISCISSUE' TRANS  UNION ALL"
        strSql += vbCrLf + " SELECT 'INTERNAL' TRANS"
        Dim dtTran = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTran)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbTransation, dtTran, "TRANS", , "ALL")
        LoadCategoryGroup()
        LoadAcname()
        cmbstocktype.Items.Clear()
        cmbstocktype.Items.Add("ALL")
        cmbstocktype.Items.Add("MANUFACTURING")
        cmbstocktype.Items.Add("TRADING")
        cmbstocktype.Items.Add("EXEMPTED")
        cmbstocktype.Text = "ALL"
        btnNew_Click(Me, New EventArgs)
        rbtGrsWeight.Checked = True
        If ExcludeItem <> "" Then ChkMfgItem.Visible = True
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        ' chkGs11.Checked = True
        ' chkGs12.Checked = True
        ' chkOthers.Checked = True
        'rbtTrandate.Checked = True
        ' cmbCategoryGroup.Text = "ALL"
        cmbstocktype.Text = "ALL"
        dtpFrom.Select()
        Prop_Gets()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub frmMetalOrnamentDetailedStockView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub chkMetalSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkMetalSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstMetal, chkMetalSelectAll.Checked)
    End Sub

    Private Sub chkCostCentreSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkCostCentreSelectAll.Checked)
    End Sub

    Private Sub LoadCategoryGroup()
        cmbCategoryGroup.Items.Clear()
        cmbCategoryGroup.Items.Add("ALL")
        strSql = " SELECT CGROUPNAME FROM " & cnAdminDb & "..CATEGORYGROUP ORDER BY CGROUPNAME"
        objGPack.FillCombo(strSql, cmbCategoryGroup, False, False)
    End Sub

    Private Sub LoadAcname()
        CmbAcname.Items.Clear()
        CmbAcname.Items.Add("ALL")
        strSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACTYPE IN (" & GetQryString(IncludeActype) & ") ORDER BY ACNAME"
        objGPack.FillCombo(strSql, CmbAcname, False, False)
        CmbAcname.Text = "ALL"
    End Sub
    Private Sub LoadCategory()
        Dim chkMetalNames As String = GetChecked_CheckedList(chkLstMetal)
        chkLstCategory.Items.Clear()
        If chkMetalNames <> "" Then
            Dim StrCatGroup As String = ""
            If cmbCategoryGroup.Text <> "ALL" And cmbCategoryGroup.Text <> "" Then
                StrCatGroup = " AND CGROUPID = (SELECT CGROUPID FROM " & cnAdminDb & "..CATEGORYGROUP WHERE CGROUPNAME = '" & cmbCategoryGroup.Text & "')"
            End If
            strSql = ""
            If chkGs11.Checked Then
                strSql += " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
                strSql += " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalNames & "))"
                strSql += " AND ISNULL(GS11,'') = 'Y' AND ISNULL(ACTIVE,'')<>'N'"
                strSql += StrCatGroup
            End If
            If chkGs12.Checked Then
                If strSql <> "" Then strSql += " UNION "
                strSql += " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
                strSql += " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalNames & "))"
                strSql += " AND ISNULL(GS12,'') = 'Y' AND ISNULL(ACTIVE,'')<>'N'"
                strSql += StrCatGroup
            End If
            If chkOthers.Checked Then
                If strSql <> "" Then strSql += " UNION "
                strSql += " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
                strSql += " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalNames & "))"
                strSql += " AND ISNULL(GS11,'') = '' AND ISNULL(GS12,'') = '' AND ISNULL(ACTIVE,'')<>'N'"
                strSql += StrCatGroup
            End If
            If rbtPcs.Checked Then
                strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
                strSql += " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalNames & ")) AND ISNULL(ACTIVE,'')<>'N'"
                strSql += StrCatGroup
            End If
            If strSql <> "" Then
                strSql += " ORDER BY CATNAME"
                FillCheckedListBox(strSql, chkLstCategory, , chkCategorySelectAll.Checked)
            End If
        End If
        LoadItemName()
    End Sub

    Private Sub chkLstMetal_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstMetal.Leave
        If Not chkLstMetal.CheckedItems.Count > 0 Then
            chkMetalSelectAll.Checked = True
        End If
    End Sub

    Private Sub chkLstMetal_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstMetal.LostFocus
        LoadCategory()
    End Sub

    Private Sub chkCategorySelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCategorySelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCategory, chkCategorySelectAll.Checked)
    End Sub

    Private Sub DataGridView_Trandate(ByVal dgv As DataGridView)
        Dim bothWeight As String()
        bothWeight = Weight.Split(",")
        With dgv
            For cnt As Integer = 1 To dgv.ColumnCount - 1
                dgv.Columns(cnt).Visible = False
            Next
            .Columns("TTRANDATE").Visible = True
            .Columns("TRANNO").Visible = rbtTranno.Checked
            If ChkWithTrantype.Checked Then .Columns("TRANTYPE").Visible = True
            If chkSpecificFormat.Checked = False Then .Columns("REMARK").Visible = True
            If chkSpecificFormat.Checked = False Then .Columns("REFNO").Visible = True
            .Columns("PARTICULAR").Visible = True
            .Columns("RECEIPT PCS").Visible = rbtPcs.Checked Or rbtBoth.Checked
            .Columns("ISSUE PCS").Visible = rbtPcs.Checked Or rbtBoth.Checked
            .Columns("CLOSING PCS").Visible = rbtPcs.Checked Or rbtBoth.Checked

            .Columns("RECEIPT STNPCS").Visible = (rbtPcs.Checked Or rbtBoth.Checked) And ChkStone.Checked
            .Columns("RECEIPT DIAPCS").Visible = (rbtPcs.Checked Or rbtBoth.Checked) And ChkDia.Checked
            .Columns("RECEIPT PREPCS").Visible = (rbtPcs.Checked Or rbtBoth.Checked) And ChkPresious.Checked
            .Columns("ISSUE STNPCS").Visible = (rbtPcs.Checked Or rbtBoth.Checked) And ChkStone.Checked
            .Columns("ISSUE DIAPCS").Visible = (rbtPcs.Checked Or rbtBoth.Checked) And ChkDia.Checked
            .Columns("ISSUE PREPCS").Visible = (rbtPcs.Checked Or rbtBoth.Checked) And ChkPresious.Checked
            .Columns("CLOSING STNPCS").Visible = (rbtPcs.Checked Or rbtBoth.Checked) And ChkStone.Checked
            .Columns("CLOSING DIAPCS").Visible = (rbtPcs.Checked Or rbtBoth.Checked) And ChkDia.Checked
            .Columns("CLOSING PREPCS").Visible = (rbtPcs.Checked Or rbtBoth.Checked) And ChkPresious.Checked
            If rdbBothwt.Checked Then
                .Columns("RECEIPT " & bothWeight(0) & "").Visible = rbtWeight.Checked Or rbtBoth.Checked
                .Columns("RECEIPT " & bothWeight(1) & "").Visible = rbtWeight.Checked Or rbtBoth.Checked
                .Columns("RECEIPT STNWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkStone.Checked
                .Columns("RECEIPT DIAWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkDia.Checked
                .Columns("RECEIPT PREWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkPresious.Checked
                .Columns("ISSUE " & bothWeight(0) & "").Visible = rbtWeight.Checked Or rbtBoth.Checked
                .Columns("ISSUE " & bothWeight(1) & "").Visible = rbtWeight.Checked Or rbtBoth.Checked
                .Columns("ISSUE STNWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkStone.Checked
                .Columns("ISSUE DIAWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkDia.Checked
                .Columns("ISSUE PREWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkPresious.Checked
                .Columns("CLOSING " & bothWeight(0) & "").Visible = rbtWeight.Checked Or rbtBoth.Checked
                .Columns("CLOSING " & bothWeight(1) & "").Visible = rbtWeight.Checked Or rbtBoth.Checked
                .Columns("CLOSING STNWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkStone.Checked
                .Columns("CLOSING DIAWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkDia.Checked
                .Columns("CLOSING PREWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkPresious.Checked
            Else
                .Columns("RECEIPT " & Weight & "").Visible = rbtWeight.Checked Or rbtBoth.Checked
                .Columns("RECEIPT STNWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkStone.Checked
                .Columns("RECEIPT DIAWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkDia.Checked
                .Columns("RECEIPT PREWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkPresious.Checked
                .Columns("ISSUE " & Weight & "").Visible = rbtWeight.Checked Or rbtBoth.Checked
                .Columns("ISSUE STNWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkStone.Checked
                .Columns("ISSUE DIAWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkDia.Checked
                .Columns("ISSUE PREWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkPresious.Checked
                .Columns("CLOSING " & Weight & "").Visible = rbtWeight.Checked Or rbtBoth.Checked
                .Columns("CLOSING STNWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkStone.Checked
                .Columns("CLOSING DIAWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkDia.Checked
                .Columns("CLOSING PREWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkPresious.Checked
            End If

            .Columns("PARTICULAR").Width = 350
            .Columns("TRANNO").Width = 70
            If chkSpecificFormat.Checked = False Then .Columns("REMARK").Width = 200
            If chkSpecificFormat.Checked = False Then .Columns("REFNO").Width = 70
            .Columns("TTRANDATE").HeaderText = "TRANDATE"
            .Columns("RECEIPT PCS").Width = 70
            .Columns("ISSUE PCS").Width = 70
            .Columns("CLOSING PCS").Width = 70
            If rdbBothwt.Checked Then
                .Columns("RECEIPT " & bothWeight(0) & "").Width = 100
                .Columns("RECEIPT " & bothWeight(1) & "").Width = 100
                .Columns("ISSUE " & bothWeight(0) & "").Width = 100
                .Columns("ISSUE " & bothWeight(1) & "").Width = 100
                .Columns("CLOSING " & bothWeight(0) & "").Width = 100
                .Columns("CLOSING " & bothWeight(1) & "").Width = 100
            Else
                .Columns("RECEIPT " & Weight & "").Width = 100
                .Columns("ISSUE " & Weight & "").Width = 100
                .Columns("CLOSING " & Weight & "").Width = 100
            End If

            .Columns("TRANNO").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("RECEIPT PCS").HeaderText = "REC PCS"
            .Columns("ISSUE PCS").HeaderText = "ISS PCS"
            .Columns("CLOSING PCS").HeaderText = "CLO PCS"
            .Columns("RECEIPT STNPCS").HeaderText = "REC STNPCS"
            .Columns("RECEIPT DIAPCS").HeaderText = "REC DIAPCS"
            .Columns("RECEIPT PREPCS").HeaderText = "REC PREPCS"
            .Columns("ISSUE PCS").HeaderText = "ISS PCS"
            .Columns("ISSUE STNPCS").HeaderText = "ISS STNPCS"
            .Columns("ISSUE DIAPCS").HeaderText = "ISS DIAPCS"
            .Columns("ISSUE PREPCS").HeaderText = "ISS PREPCS"
            .Columns("CLOSING PCS").HeaderText = "CLO PCS"
            .Columns("CLOSING STNPCS").HeaderText = "CLO STNPCS"
            .Columns("CLOSING DIAPCS").HeaderText = "CLO DIAPCS"
            .Columns("CLOSING PREPCS").HeaderText = "CLO PREPCS"

            If rdbBothwt.Checked Then
                .Columns("RECEIPT " & bothWeight(0) & "").HeaderText = "REC " & bothWeight(0) & ""
                .Columns("RECEIPT " & bothWeight(1) & "").HeaderText = "REC " & bothWeight(1) & ""
                .Columns("ISSUE " & bothWeight(0) & "").HeaderText = "ISS " & bothWeight(0) & ""
                .Columns("ISSUE " & bothWeight(1) & "").HeaderText = "ISS " & bothWeight(1) & ""
                .Columns("CLOSING " & bothWeight(0) & "").HeaderText = "CLO " & bothWeight(0) & ""
                .Columns("CLOSING " & bothWeight(1) & "").HeaderText = "CLO " & bothWeight(1) & ""
            Else
                .Columns("RECEIPT " & Weight & "").HeaderText = "REC " & Weight & ""
                .Columns("ISSUE " & Weight & "").HeaderText = "ISS " & Weight & ""
                .Columns("CLOSING " & Weight & "").HeaderText = "CLO " & Weight & ""
            End If

            .Columns("RECEIPT STNWT").HeaderText = "REC STNWT"
            .Columns("RECEIPT DIAWT").HeaderText = "REC DIAWT"
            .Columns("RECEIPT PREWT").HeaderText = "REC PREWT"
            .Columns("ISSUE STNWT").HeaderText = "ISS STNWT"
            .Columns("ISSUE DIAWT").HeaderText = "ISS DIAWT"
            .Columns("ISSUE PREWT").HeaderText = "ISS PREWT"
            .Columns("CLOSING STNWT").HeaderText = "CLO STNWT"
            .Columns("CLOSING DIAWT").HeaderText = "CLO DIAWT"
            .Columns("CLOSING PREWT").HeaderText = "CLO PREWT"


            .Columns("RECEIPT PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ISSUE PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CLOSING PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            If rdbBothwt.Checked Then
                .Columns("RECEIPT " & bothWeight(0) & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("RECEIPT " & bothWeight(1) & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("ISSUE " & bothWeight(0) & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("ISSUE " & bothWeight(1) & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CLOSING " & bothWeight(0) & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CLOSING " & bothWeight(1) & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            Else
                .Columns("RECEIPT " & Weight & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("ISSUE " & Weight & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CLOSING " & Weight & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End If

            'FormatGridColumns(dgv, False, False, , False)
            FormatGridColumns(dgv)
            FillGridGroupStyle_KeyNoWise(dgv)
        End With
    End Sub
    Public Sub FormatGridColumns(ByVal grid As DataGridView, Optional ByVal colHeadVisibleSetFalse As Boolean = True, Optional ByVal colFormat As Boolean = True, Optional ByVal reeadOnly As Boolean = True, Optional ByVal sortColumns As Boolean = True)
        With grid
            If colHeadVisibleSetFalse Then .ColumnHeadersVisible = False
            For i As Integer = 0 To .ColumnCount - 1
                .Columns(i).ReadOnly = reeadOnly
                If .Columns(i).ValueType.Name Is GetType(Decimal).Name Then
                    If colFormat Then .Columns(i).DefaultCellStyle.Format = "0.000"
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf .Columns(i).ValueType.Name Is GetType(Double).Name Then
                    If colFormat Then .Columns(i).DefaultCellStyle.Format = "0.00"
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf .Columns(i).ValueType.Name Is GetType(Integer).Name Then
                    .Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf .Columns(i).ValueType.Name Is GetType(Date).Name Then
                    .Columns(i).DefaultCellStyle.Format = "dd/MM/yyyy"
                End If
                If Not sortColumns Then .Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
                '.Columns(i).Resizable = DataGridViewTriState.False 
            Next
        End With
    End Sub

    Private Sub chkGs11_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkGs11.CheckedChanged
        LoadCategory()
    End Sub

    Private Sub chkGs12_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkGs12.CheckedChanged
        LoadCategory()
    End Sub

    Private Sub chkOthers_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkOthers.CheckedChanged
        LoadCategory()
    End Sub

    Private Sub chkLstCategory_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstCategory.Leave
        If Not chkLstCategory.CheckedItems.Count > 0 Then
            chkCategorySelectAll.Checked = True
        End If
    End Sub

    Private Sub chkLstCostCentre_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstCostCentre.Leave
        If Not chkLstCostCentre.CheckedItems.Count > 0 Then
            chkCostCentreSelectAll.Checked = True
        End If
    End Sub

    Private Sub cmbCategoryGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCategoryGroup.SelectedIndexChanged
        LoadCategory()
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmMetalOrnamentDetailedStockView_Properties
        'obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_chkSpecificFormat = chkSpecificFormat.Checked
        obj.p_chkWithTrantype = ChkWithTrantype.Checked
        obj.p_chkCategorywise = chkCategorywise.Checked
        obj.p_chkRepairdet = ChkRepairDet.Checked
        obj.p_chkMetalSelectAll = chkMetalSelectAll.Checked
        GetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal)
        obj.p_rbtBoth = rbtBoth.Checked
        obj.p_rbtPcs = rbtPcs.Checked
        obj.p_rbtWeight = rbtWeight.Checked
        obj.p_chkGs11 = chkGs11.Checked
        obj.p_chkGs12 = chkGs12.Checked
        obj.p_chkOthers = chkOthers.Checked
        obj.p_cmbCategoryGroup = cmbCategoryGroup.Text
        obj.p_chkCategorySelectAll = chkCategorySelectAll.Checked
        GetChecked_CheckedList(chkLstCategory, obj.p_chkLstCategory)
        GetChecked_CheckedList(chkCmbItem, obj.p_chkCmbItem)
        GetChecked_CheckedList(chkCmbTransation, obj.p_chkCmbTransation)
        obj.p_rbtTrandate = rbtTrandate.Checked
        obj.p_rbtTranno = rbtTranno.Checked
        obj.p_rbtGrsWeight = rbtGrsWeight.Checked
        obj.p_rbtNetWt = rbtNetWt.Checked
        obj.p_chkOrderbyTranno = chkOrderbyTranno.Checked
        obj.p_chkWithMFG = ChkMfgItem.Checked
        SetSettingsObj(obj, Me.Name, GetType(frmMetalOrnamentDetailedStockView_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmMetalOrnamentDetailedStockView_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmMetalOrnamentDetailedStockView_Properties))
        'chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        chkSpecificFormat.Checked = obj.p_chkSpecificFormat
        ChkWithTrantype.Checked = obj.p_chkWithTrantype
        chkCategorywise.Checked = obj.p_chkCategorywise
        ChkRepairDet.Checked = obj.p_chkRepairdet

        chkMetalSelectAll.Checked = obj.p_chkMetalSelectAll
        SetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal, Nothing)
        rbtBoth.Checked = obj.p_rbtBoth
        rbtPcs.Checked = obj.p_rbtPcs
        rbtWeight.Checked = obj.p_rbtWeight
        chkGs11.Checked = obj.p_chkGs11
        chkGs12.Checked = obj.p_chkGs12
        chkOthers.Checked = obj.p_chkOthers
        cmbCategoryGroup.Text = obj.p_cmbCategoryGroup
        chkCategorySelectAll.Checked = obj.p_chkCategorySelectAll
        SetChecked_CheckedList(chkLstCategory, obj.p_chkLstCategory, Nothing)
        SetChecked_CheckedList(chkCmbItem, obj.p_chkCmbItem, "ALL")
        SetChecked_CheckedList(chkCmbTransation, obj.p_chkCmbTransation, "ALL")
        rbtTrandate.Checked = obj.p_rbtTrandate
        rbtTranno.Checked = obj.p_rbtTranno
        rbtGrsWeight.Checked = obj.p_rbtGrsWeight
        rbtNetWt.Checked = obj.p_rbtNetWt
        chkOrderbyTranno.Checked = obj.p_chkOrderbyTranno
        ChkMfgItem.Checked = obj.p_chkWithMFG
    End Sub

    Private Sub rbtPcs_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtPcs.CheckedChanged
        chkGs11.Enabled = Not rbtPcs.Checked
        chkGs12.Enabled = Not rbtPcs.Checked
        chkOthers.Enabled = Not rbtPcs.Checked
        chkCmbItem.Enabled = rbtPcs.Checked
        If rbtPcs.Checked Then
            chkGs11.Checked = False
            chkGs12.Checked = False
            chkOthers.Checked = False
        End If
    End Sub

    Private Sub rbtTranno_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtTranno.CheckedChanged
        chkOrderbyTranno.Visible = rbtTranno.Checked
    End Sub

    Private Sub ContextMenuStrip1_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening

    End Sub

    Private Sub grpContainer_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grpContainer.Enter

    End Sub

    Private Sub chkSpecificFormat_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSpecificFormat.CheckedChanged
        If chkSpecificFormat.Checked = True Then
            btnPrint.Enabled = True
            ' btnSearch.Enabled = False
        Else
            btnPrint.Enabled = False
            'btnSearch.Enabled = True
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        PrintReport()
    End Sub
    Function funcPageNo() As Integer
        Dim ct As New Data.DataTable
        Dim category As String = ""
        Dim metal As String = ""
        line = 0
        ct.Clear()
        GS = ""
        For i As Integer = 0 To chkLstCategory.CheckedItems.Count - 1
            If chkLstCategory.CheckedItems(i).ToString <> "GOLD ORN VAT EXEMPTED" Then
                If category <> "" Then category += ","
                category += chkLstCategory.CheckedItems(i)
            End If
        Next
        'For j As Integer = 0 To chkLstMetal.CheckedItems.Count - 1
        '    If metal <> "" Then metal += ","
        '    metal += chkLstMetal.CheckedItems(j)
        'Next
        'If chkGs11.Checked Then
        '    GS += "GS11"
        'End If
        'If chkGs12.Checked Then
        '    GS += " GS12"
        'End If
        'If chkOthers.Checked Then
        '    GS += " OTHERS"
        'End If

        strSql = "SELECT COMPANYNAME,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4 FROM " & cnAdminDb & "..COMPANY where ISNULL(ACTIVE,'')<>'N'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(ct)
        If ct.Rows.Count > 0 Then
            ' FileWrite.WriteLine(strprint)
            strprint = Chr(27) + Chr(67) + Chr(72)
            FileWrite.WriteLine(strprint)
            strprint = Chr(27) + Chr(18)
            FileWrite.WriteLine(strprint)
            strprint = Chr(27) + "j" + Chr(180)
            FileWrite.WriteLine(strprint)
            funcFindSpaceCompany(ct)
        End If
        PgNo += 1
        strprint = "                    --------------------------------------------              "
        FileWrite.WriteLine(strprint) : line += 1
        strprint = "       Stock Register  For the Period  " + dtpFrom.Text + " TO " + dtpTo.Text + " " + "      " + RSet("Page No." + PgNo.ToString, 12)
        FileWrite.WriteLine(strprint) : line += 1
        'strprint = "                                    " & GS
        'FileWrite.WriteLine(strprint) : line += 1

        'If chkMetalSelectAll.Checked = True Then metal = "ALL METAL"
        'If Len(metal) > 80 Then strprint = metal.Substring(1, 80) Else strprint = metal
        'strprint = Space((80 - Len(strprint)) / 2) + strprint
        'FileWrite.WriteLine(strprint) : line += 1
        'If Len(metal) > 80 Then strprint = metal.Substring(81, 80) Else strprint = ""
        'If Len(Trim(strprint)) > 0 Then strprint = Space(Len(strprint) / 2) + strprint : FileWrite.WriteLine(strprint) : line += 1

        If chkCategorySelectAll.Checked = True Then category = "ALL CATEGORY"
        If Len(category) > 80 Then strprint = category.Substring(1, 80) Else strprint = category
        strprint = Space((80 - Len(strprint)) / 2) + strprint
        FileWrite.WriteLine(strprint) : line += 1
        'If Len(category) > 80 Then strprint = category.Substring(81, 80) Else strprint = ""
        'If Len(Trim(strprint)) > 0 Then strprint = Space(Len(strprint) / 2) + strprint : FileWrite.WriteLine(strprint) : line += 1
        strprint = "------------------------------------------------------------------------------------"
        FileWrite.WriteLine(Trim(strprint)) : line += 1
        If funcEndPrint = False Then

            Dim str1 As String = Space(6) : Dim str2 As String = Space(6) : Dim str3 As String = Space(15)
            Dim str4, str5, str6, str7, str8, str9 As String
            Dim mstr1 As String, mstr2 As String
            str1 = LSet("NO", 6)
            str2 = LSet("TYPE", 12)
            str3 = LSet("Party Name ", 15)
            mstr1 = Space(33)
            If rbtPcs.Checked = False Then
                If rdbBothwt.Checked = True Or rbtGrsWeight.Checked = True Then str4 = Space(12) : str4 = LSet("Grs Weight", 12)
                If rdbBothwt.Checked = True Or Me.rbtNetWt.Checked = True Then str5 = Space(12) : str5 = LSet("Net Weight", 12)
                If rdbBothwt.Checked = True Or rbtGrsWeight.Checked = True Then str6 = Space(12) : str6 = LSet("Grs Weight", 12)
                If rdbBothwt.Checked = True Or Me.rbtNetWt.Checked = True Then str7 = Space(12) : str7 = LSet("Net Weight", 12)
            End If
            If rbtPcs.Checked = True Then str8 = Space(12) : str8 = LSet("Rec PCS", 12) : str9 = Space(12) : str9 = LSet("Iss PCS", 12)
            If rbtPcs.Checked = False Then
                mstr2 = Space(Len(str4 + str5) / 2) + "Receipts"
                mstr2 = mstr2 & Space(Len(str6 + str7) / 2) + "Issues"
            End If
            If rbtPcs.Checked = True Then mstr2 = Space(Len(str8) / 3) + "Receipts"
            If rbtPcs.Checked = True Then mstr2 = mstr2 & Space(Len(str9) / 3) + "Issues"
            strprint = mstr1 + mstr2
            FileWrite.WriteLine(strprint) : line += 1
            strprint = str1 + str2 + str3 + Space(2) + str4 + str5 + str6 + str7 + str8 + str9
            FileWrite.WriteLine(strprint) : line += 1
            strprint = "----------------------------------------------------------------------------------"
            FileWrite.WriteLine(Trim(strprint)) : line += 1
        End If
    End Function
    Function funcFindSpaceCompany(ByVal dt As DataTable)
        If dt.Rows.Count > 0 Then
            Dim LspaceComp, RspaceComp, i, j, Findspace As Integer
            i = Len(dt.Rows(0).Item("COMPANYNAME").ToString)
            Findspace = 80 - i
            LspaceComp = Findspace / 2
            RspaceComp = Findspace / 2
            strprint = Space(LspaceComp) + dt.Rows(0).Item("COMPANYNAME").ToString + Space(RspaceComp)
            FileWrite.WriteLine(strprint)
            If Len(dt.Rows(0).Item("ADDRESS1")) > 0 Then
                If Len(dt.Rows(0).Item("ADDRESS2")) > 0 Then
                    i = 0
                    LspaceComp = 0
                    RspaceComp = 0
                    i = Len(dt.Rows(0).Item("ADDRESS1").ToString)
                    j = Len(dt.Rows(0).Item("ADDRESS2").ToString)
                    Findspace = 80 - (i + j)
                    LspaceComp = Findspace / 2
                    RspaceComp = Findspace / 2
                    strprint = Space(LspaceComp) + dt.Rows(0).Item("ADDRESS1").ToString + "," + dt.Rows(0).Item("ADDRESS2").ToString + Space(RspaceComp)
                    FileWrite.WriteLine(strprint)
                Else
                    i = 0
                    LspaceComp = 0
                    RspaceComp = 0
                    i = Len(dt.Rows(0).Item("ADDRESS1").ToString)
                    Findspace = 80 - (i + j)
                    LspaceComp = Findspace / 2
                    RspaceComp = Findspace / 2
                    strprint = Space(LspaceComp) + dt.Rows(0).Item("ADDRESS1").ToString + Space(RspaceComp)
                    FileWrite.WriteLine(strprint)
                    Exit Function
                End If
            End If
            If Len(dt.Rows(0).Item("ADDRESS3").ToString) > 0 Then
                If Len(dt.Rows(0).Item("ADDRESS4").ToString) > 0 Then
                    i = 0
                    LspaceComp = 0
                    RspaceComp = 0
                    i = Len(dt.Rows(0).Item("ADDRESS3").ToString)
                    j = Len(dt.Rows(0).Item("ADDRESS4").ToString)
                    Findspace = 80 - (i + j)
                    LspaceComp = Findspace / 2
                    RspaceComp = Findspace / 2
                    strprint = Space(LspaceComp) + dt.Rows(0).Item("ADDRESS3").ToString + "," + dt.Rows(0).Item("ADDRESS4").ToString + Space(RspaceComp)
                    FileWrite.WriteLine(strprint)
                Else
                    i = 0
                    LspaceComp = 0
                    RspaceComp = 0
                    i = Len(dt.Rows(0).Item("ADDRESS3").ToString)
                    Findspace = 80 - (i + j)
                    LspaceComp = Findspace / 2
                    RspaceComp = Findspace / 2
                    strprint = Space(LspaceComp) + dt.Rows(0).Item("ADDRESS3").ToString + Space(RspaceComp)
                    FileWrite.WriteLine(strprint)
                    Exit Function
                End If
            End If
        End If
    End Function

    Private Sub chkWithAlloy_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkWithAlloy.CheckedChanged

    End Sub

    Private Sub chkCategorywise_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCategorywise.CheckedChanged

    End Sub

    Private Sub chkCmbTransation_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCmbTransation.TextChanged
        If chkCmbTransation.Text = "ALL" Or chkCmbTransation.Text = "" Then
            ChkWithApproval.Enabled = True
            ChkWithApproval.Checked = True
            chkMIMRApproval.Enabled = True
            chkMIMRApproval.Checked = True
        ElseIf chkCmbTransation.Text.Contains("APPROVAL") Then
            ChkWithApproval.Enabled = True
            ChkWithApproval.Checked = True
            chkMIMRApproval.Enabled = True
            chkMIMRApproval.Checked = True
        Else
            ChkWithApproval.Enabled = False
            ChkWithApproval.Checked = False
            chkMIMRApproval.Enabled = False
            chkMIMRApproval.Checked = False
        End If
    End Sub

    Private Sub ChkStone_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkStone.CheckedChanged
        StnWtConvert_Need()
    End Sub
    Private Sub StnWtConvert_Need()
        If ChkStone.Checked Or ChkDia.Checked Or ChkPresious.Checked Then
            chkIntoCt.Enabled = True : chkIntoGm.Enabled = True
        Else
            chkIntoCt.Enabled = False : chkIntoGm.Enabled = False
        End If
    End Sub

    Private Sub ChkPresious_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkPresious.CheckedChanged
        StnWtConvert_Need()
    End Sub

    Private Sub ChkDia_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkDia.CheckedChanged
        StnWtConvert_Need()
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class

Public Class frmMetalOrnamentDetailedStockView_Properties
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
    Private rbtBoth As Boolean = True
    Public Property p_rbtBoth() As Boolean
        Get
            Return rbtBoth
        End Get
        Set(ByVal value As Boolean)
            rbtBoth = value
        End Set
    End Property
    Private rbtPcs As Boolean = False
    Public Property p_rbtPcs() As Boolean
        Get
            Return rbtPcs
        End Get
        Set(ByVal value As Boolean)
            rbtPcs = value
        End Set
    End Property
    Private rbtWeight As Boolean = False
    Public Property p_rbtWeight() As Boolean
        Get
            Return rbtWeight
        End Get
        Set(ByVal value As Boolean)
            rbtWeight = value
        End Set
    End Property
    Private chkGs11 As Boolean = True
    Public Property p_chkGs11() As Boolean
        Get
            Return chkGs11
        End Get
        Set(ByVal value As Boolean)
            chkGs11 = value
        End Set
    End Property
    Private chkGs12 As Boolean = True
    Public Property p_chkGs12() As Boolean
        Get
            Return chkGs12
        End Get
        Set(ByVal value As Boolean)
            chkGs12 = value
        End Set
    End Property
    Private chkOthers As Boolean = True
    Public Property p_chkOthers() As Boolean
        Get
            Return chkOthers
        End Get
        Set(ByVal value As Boolean)
            chkOthers = value
        End Set
    End Property
    Private cmbCategoryGroup As String = "ALL"
    Public Property p_cmbCategoryGroup() As String
        Get
            Return cmbCategoryGroup
        End Get
        Set(ByVal value As String)
            cmbCategoryGroup = value
        End Set
    End Property

    Private chkSpecificFormat As Boolean = True
    Public Property p_chkSpecificFormat() As Boolean
        Get
            Return chkSpecificFormat
        End Get
        Set(ByVal value As Boolean)
            chkSpecificFormat = value
        End Set
    End Property

    Private chkRepairdet As Boolean = True
    Public Property p_chkRepairdet() As Boolean
        Get
            Return chkRepairdet
        End Get
        Set(ByVal value As Boolean)
            chkRepairdet = value
        End Set
    End Property

    Private chkWithTrantype As Boolean = True
    Public Property p_chkWithTrantype() As Boolean
        Get
            Return chkWithTrantype
        End Get
        Set(ByVal value As Boolean)
            chkWithTrantype = value
        End Set
    End Property

    Private chkCategorywise As Boolean = True
    Public Property p_chkCategorywise() As Boolean
        Get
            Return chkCategorywise
        End Get
        Set(ByVal value As Boolean)
            chkCategorywise = value
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
    Private rbtTrandate As Boolean = True
    Public Property p_rbtTrandate() As Boolean
        Get
            Return rbtTrandate
        End Get
        Set(ByVal value As Boolean)
            rbtTrandate = value
        End Set
    End Property
    Private rbtTranno As Boolean = False
    Public Property p_rbtTranno() As Boolean
        Get
            Return rbtTranno
        End Get
        Set(ByVal value As Boolean)
            rbtTranno = value
        End Set
    End Property
    Private chkCmbTransation As New List(Of String)
    Public Property p_chkCmbTransation() As List(Of String)
        Get
            Return chkCmbTransation
        End Get
        Set(ByVal value As List(Of String))
            chkCmbTransation = value
        End Set
    End Property
    Private rbtGrsWeight As Boolean = True
    Public Property p_rbtGrsWeight() As Boolean
        Get
            Return rbtGrsWeight
        End Get
        Set(ByVal value As Boolean)
            rbtGrsWeight = value
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
    Private chkCmbItem As New List(Of String)
    Public Property p_chkCmbItem() As List(Of String)
        Get
            Return chkCmbItem
        End Get
        Set(ByVal value As List(Of String))
            chkCmbItem = value
        End Set
    End Property
    Private chkOrderbyTranno As Boolean = False
    Public Property p_chkOrderbyTranno() As Boolean
        Get
            Return chkOrderbyTranno
        End Get
        Set(ByVal value As Boolean)
            chkOrderbyTranno = value
        End Set
    End Property
    Private chkWithMFG As Boolean = False
    Public Property p_chkWithMFG() As Boolean
        Get
            Return chkWithMFG
        End Get
        Set(ByVal value As Boolean)
            chkWithMFG = value
        End Set
    End Property
End Class
