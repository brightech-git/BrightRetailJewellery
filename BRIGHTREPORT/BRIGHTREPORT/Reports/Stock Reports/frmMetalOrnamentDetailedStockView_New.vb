Imports System.Data.OleDb
Imports System.IO
Public Class frmMetalOrnamentDetailedStockView_New
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim Weight As String = "GRSWT"
    Dim dtItem As New DataTable
    Dim FormReSize As Boolean = True
    Dim FormReLocation As Boolean = True
    Dim chkCostName As String = ""
    Dim chkMetal As String = ""
    Dim chkCategory As String = ""
    Dim TranType As String = ""
    Dim FileWrite As StreamWriter
    Dim strprint As String
    Dim funcEndPrint As Boolean
    Dim PgNo As Integer
    Dim line As Integer
    Dim GS As String
    Dim gsformname As String = GetAdmindbSoftValue("GSFORMNAME", "GS11,GS12")
    Dim gstitlename As String = GetAdmindbSoftValue("GSTITLENAME", "")
    Dim Gs11Name As String = "GS11", Gs12Name As String = "GS12"




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
            strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK_1"
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
            strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK_1"
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
    Private Sub InsGsIssRec(ByVal Tran As String, ByVal RecIss As String, ByVal OrderInfo As Boolean, Optional ByVal InsComments As String = "")
        Dim bothWeight As String()
        bothWeight = Weight.Split(",")
        If Tran = "T" Then
            strSql = " /** " & InsComments & "**/"
            strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '" & IIf(RecIss = "I", "ISS", "REC") & "' SEP,I.TRANTYPE"
            If rbtTranno.Checked Then
                strSql += vbCrLf + "  	,TRANNO"
            Else
                strSql += vbCrLf + "  	,NULL TRANNO"
            End If
            strSql += vbCrLf + " ,CASE WHEN (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.PCS ELSE 0 END AS PCS"
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
            strSql += vbCrLf + " ,CASE WHEN LEN(I.TRANTYPE) = 3 THEN HE.ACNAME "
            strSql += vbCrLf + "       WHEN I.TRANTYPE = 'SA' THEN PI.PNAME"
            ' strSql += vbCrLf + "       WHEN I.TRANTYPE = 'MI' THEN 'MISC ISS'"
            strSql += vbCrLf + "       WHEN I.TRANTYPE='MI' THEN HE.ACNAME"
            strSql += vbCrLf + "       WHEN I.TRANTYPE='IOT' THEN HE.ACNAME"
            strSql += vbCrLf + "       WHEN I.TRANTYPE='ROT' THEN HE.ACNAME"
            strSql += vbCrLf + "       WHEN I.TRANTYPE='RIS' THEN HE.ACNAME"
            strSql += vbCrLf + "       WHEN I.TRANTYPE = 'AI' THEN 'APPROVAL'"
            strSql += vbCrLf + "       WHEN I.TRANTYPE = 'OD' THEN 'ORDER DEL'"
            strSql += vbCrLf + "       WHEN I.TRANTYPE = 'RD' THEN 'REPAIR DEL'"
            strSql += vbCrLf + "       WHEN I.TRANTYPE = 'PU' THEN  PI.PNAME"
            strSql += vbCrLf + "       WHEN I.TRANTYPE = 'AR' THEN 'APPROVAL'"
            strSql += vbCrLf + "       ELSE I.TRANTYPE END AS ACNAME"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(100),IM.ITEMNAME) AS ITEMNAME"
            strSql += vbCrLf + " FROM TEMP" & systemId & IIf(RecIss = "I", "ISS", "REC") & " I "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ACHEAD AS HE ON HE.ACCODE = I.ACCODE"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO AS CU ON CU.BATCHNO = I.BATCHNO"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PERSONALINFO AS PI ON PI.SNO = CU.PSNO"
            strSql += vbCrLf + " WHERE I.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            If OrderInfo = False Then
                strSql += vbCrLf + " AND I.BATCHNO NOT IN (SELECT BATCHNO FROM TEMP" & systemId & "ORDREP_DEL)"
            Else
                strSql += vbCrLf + "    AND I.BATCHNO IN (SELECT BATCHNO FROM TEMP" & systemId & "ORDREP_DEL) AND I.TRANTYPE <> 'SA' AND I.GRSWT > 0"
            End If
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        Else 'O'
            strSql = " /** " & InsComments & "**/"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPDB..#OPETRAN" & IIf(RecIss = "I", "ISS", "REC") & "','U') IS NOT NULL DROP TABLE #OPETRAN" & IIf(RecIss = "I", "ISS", "REC") & ""
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            strSql = vbCrLf + " SELECT "
            strSql += vbCrLf + " CASE WHEN (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.PCS ELSE 0 END AS PCS"
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
            strSql += vbCrLf + " FROM TEMP" & systemId & IIf(RecIss = "I", "ISS", "REC") & " I "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..ITEMMAST AS IM ON IM.ITEMID = I.ITEMID "
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..SUBITEMMAST AS SM ON SM.ITEMID = I.ITEMID AND SM.SUBITEMID = I.SUBITEMID"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..CATEGORY AS CA ON CA.CATCODE = I.CATCODE"
            strSql += vbCrLf + " INNER JOIN " & cnAdminDb & "..METALMAST AS ME ON ME.METALID = CA.METALID"
            strSql += vbCrLf + " WHERE I.TRANDATE < '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
            If OrderInfo = False Then
                strSql += vbCrLf + " AND I.BATCHNO NOT IN (SELECT BATCHNO FROM TEMP" & systemId & "ORDREP_DEL)"
            Else
                strSql += vbCrLf + "    AND I.BATCHNO IN (SELECT BATCHNO FROM TEMP" & systemId & "ORDREP_DEL) AND I.TRANTYPE <> 'SA'  AND I.GRSWT > 0"
            End If
            strSql += vbCrLf + ""
            strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '" & IIf(RecIss = "I", "ISS", "REC") & "' SEP"
            strSql += vbCrLf + " ,'" & IIf(RecIss = "I", "IIS", "RPU") & "' TRANTYPE"
            strSql += vbCrLf + " ,NULL TRANNO"
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
            strSql += vbCrLf + " GROUP BY CATNAME,GS,METAL"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        End If
    End Sub

    Private Sub InsGsStone(ByVal Tran As String, ByVal RecIss As String, Optional ByVal InsComments As String = "")
        Dim bothWeight As String()
        bothWeight = Weight.Split(",")
        If Tran = "T" Then
            strSql = " /** " & InsComments & "**/"
            strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '" & IIf(RecIss = "I", "ISS", "REC") & "' SEP,I.TRANTYPE,S.TRANNO"
            strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('P','B')) THEN S.STNPCS ELSE 0 END PCS"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END " & bothWeight(0) & ""
                strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END " & bothWeight(1) & ""
            Else
                strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END " & Weight & ""
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
            strSql += vbCrLf + " ,CASE WHEN LEN(S.TRANTYPE) = 3 THEN HE.ACNAME "
            strSql += vbCrLf + "       WHEN S.TRANTYPE = 'SA' THEN PI.PNAME"
            'strSql += vbCrLf + "       WHEN S.TRANTYPE = 'MI' THEN 'MISC ISS'"
            strSql += vbCrLf + "       WHEN I.TRANTYPE='MI' THEN HE.ACNAME"
            strSql += vbCrLf + "       WHEN I.TRANTYPE='IOT' THEN HE.ACNAME"
            strSql += vbCrLf + "       WHEN I.TRANTYPE='ROT' THEN HE.ACNAME"
            strSql += vbCrLf + "       WHEN I.TRANTYPE='RIS' THEN HE.ACNAME"
            strSql += vbCrLf + "       WHEN S.TRANTYPE = 'AI' THEN 'APPROVAL'"
            strSql += vbCrLf + "       WHEN S.TRANTYPE = 'OD' THEN 'ORDER DEL'"
            strSql += vbCrLf + "       WHEN S.TRANTYPE = 'RD' THEN 'REPAIR DEL'"
            strSql += vbCrLf + "       WHEN S.TRANTYPE = 'PU' THEN  PI.PNAME"
            strSql += vbCrLf + "       WHEN S.TRANTYPE = 'AR' THEN 'APPROVAL'"
            strSql += vbCrLf + "       ELSE S.TRANTYPE END AS ACNAME"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(100),IM.ITEMNAME) AS ITEMNAME"
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
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..CUSTOMERINFO AS CU ON CU.BATCHNO = I.BATCHNO"
            strSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..PERSONALINFO AS PI ON PI.SNO = CU.PSNO"
            strSql += vbCrLf + " WHERE S.TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND S.COMPANYID = '" & strCompanyId & "'"
            If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
                strSql += vbCrLf + " AND ISNULL(S.STNITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
            End If
            If TranType <> "" Then strSql += vbCrLf + " AND S.TRANTYPE IN (" & TranType & ")"
            If chkCostName <> "" Then strSql += vbCrLf + " AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            strSql += vbCrLf + " AND S.BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & ".." & IIf(RecIss = "I", "ISSSTONE", "RECEIPTSTONE") & " WHERE TRANTYPE = 'RD')"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        Else 'O'
            strSql = " /** " & InsComments & "**/"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPDB..#OPESTUD" & IIf(RecIss = "I", "ISS", "REC") & "') IS NOT NULL DROP TABLE #OPESTUD" & IIf(RecIss = "I", "ISS", "REC") & ""
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
            strSql = vbCrLf + " SELECT "
            strSql += vbCrLf + " CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('P','B')) THEN S.STNPCS ELSE 0 END PCS"
            strSql += vbCrLf + " ,CASE WHEN I.ITEMID = 0 OR (I.SUBITEMID = 0 AND IIM.BOOKSTOCK IN ('W','B')) OR (I.SUBITEMID <> 0 AND ISM.BOOKSTOCK IN ('W','B')) THEN S.STNWT ELSE 0 END " & Weight & ""
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
            strSql += vbCrLf + " AND S.BATCHNO NOT IN (SELECT BATCHNO FROM " & cnStockDb & ".." & IIf(RecIss = "I", "ISSSTONE", "RECEIPTSTONE") & " WHERE TRANTYPE = 'RD')"
            If chkCostName <> "" Then strSql += vbCrLf + " AND S.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            strSql += vbCrLf + ""
            strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " SELECT "
            strSql += vbCrLf + " '" & IIf(RecIss = "I", "ISS", "REC") & "' SEP"
            strSql += vbCrLf + " ,'" & IIf(RecIss = "I", "IIS", "RPU") & "' TRANTYPE"
            strSql += vbCrLf + " ,NULL TRANNO"
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
            strSql += vbCrLf + " GROUP BY CATNAME,METAL,GS"
            cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()
        End If
    End Sub


    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Dim bothWeight As String()
        Weight = IIf(rbtGrsWeight.Checked, "GRSWT", "NETWT")
        If rdbBothwt.Checked Then
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
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        chkCostName = GetChecked_CheckedList(chkLstCostCentre)
        chkMetal = GetChecked_CheckedList(chkLstMetal)
        chkCategory = GetChecked_CheckedList(chkLstCategory)
        TranType = ""

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
        End If

        strSql = " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ORDREP_DEL' AND XTYPE = 'U')>0 DROP TABLE TEMP" & systemId & "ORDREP_DEL"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "GSSTOCK' AND XTYPE = 'U')>0 DROP TABLE TEMP" & systemId & "GSSTOCK"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "GSSTOCK_1' AND XTYPE = 'U')>0 DROP TABLE TEMP" & systemId & "GSSTOCK_1"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ISS' AND XTYPE = 'V')>0 DROP VIEW TEMP" & systemId & "ISS"
        strSql += vbCrLf + " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "REC' AND XTYPE = 'V')>0 DROP VIEW TEMP" & systemId & "REC"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = " CREATE VIEW TEMP" & systemId & "ISS"
        strSql += vbCrLf + " AS"
        strSql += vbCrLf + " SELECT ITEMID,SUBITEMID,TRANNO,TRANDATE,BATCHNO,CATCODE,SNO,ACCODE,TRANTYPE"
        strSql += vbCrLf + " ,PCS,GRSWT,NETWT"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREPCS"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..ISSSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
        strSql += vbCrLf + " WHERE I.TRANTYPE <> 'IRC' AND I.TRANDATE <= '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
        If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
            strSql += " AND ISNULL(I.ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
        End If
        If TranType <> "" Then strSql += vbCrLf + " AND I.TRANTYPE IN (" & TranType & ")"
        If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = I.BATCHNO AND TRANTYPE = 'RD')"
        If chkWithAlloy.Checked Then
            strSql += vbCrLf + " UNION ALL"
            strSql += vbCrLf + " SELECT I.ITEMID,I.SUBITEMID,AD.TRANNO,AD.TRANDATE,AD.BATCHNO,I.CATCODE,AD.SNO,I.ACCODE,AD.TRANTYPE"
            strSql += vbCrLf + " ,0 PCS,WEIGHT GRSWT,WEIGHT NETWT"
            strSql += vbCrLf + " ,NULL AS STNPCS,NULL AS STNWT,NULL AS DIAPCS,NULL AS DIAWT,NULL AS PREPCS,NULL AS PREWT"
            strSql += vbCrLf + " FROM " & cnStockDb & "..ALLOYDETAILS AS AD"
            strSql += vbCrLf + " LEFT JOIN " & cnStockDb & "..ISSUE AS I ON AD.ISSSNO=I.SNO"
            strSql += vbCrLf + " WHERE AD.TRANDATE <= '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
            strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
            strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
            If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
                strSql += " AND ISNULL(I.ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
            End If
            If TranType <> "" Then strSql += vbCrLf + " AND AD.TRANTYPE IN (" & TranType & ")"
            If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
            If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
            strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..ISSUE WHERE BATCHNO = I.BATCHNO AND TRANTYPE IN('RD','RRE'))"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = " CREATE VIEW TEMP" & systemId & "REC"
        strSql += vbCrLf + " AS"
        strSql += vbCrLf + " SELECT ITEMID,SUBITEMID,TRANNO,TRANDATE,BATCHNO,CATCODE,SNO,ACCODE,TRANTYPE"
        strSql += vbCrLf + " ,PCS,GRSWT,NETWT"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNPCS"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'S')) AS STNWT"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAPCS"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'D')) AS DIAWT"
        strSql += vbCrLf + " ,(SELECT SUM(STNPCS) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREPCS"
        strSql += vbCrLf + " ,(SELECT SUM(STNWT) FROM " & cnStockDb & "..RECEIPTSTONE WHERE ISSSNO = I.SNO AND STNITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE DIASTONE = 'P')) AS PREWT"
        strSql += vbCrLf + " FROM " & cnStockDb & "..RECEIPT AS I"
        strSql += vbCrLf + " WHERE I.TRANTYPE <> 'RRC' AND I.TRANDATE <= '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " AND I.COMPANYID = '" & strCompanyId & "'"
        strSql += vbCrLf + " AND ISNULL(I.CANCEL,'') = ''"
        If chkCmbItem.Enabled And chkCmbItem.Text <> "ALL" And chkCmbItem.Text <> "" Then
            strSql += " AND ISNULL(I.ITEMID,0) IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkCmbItem.Text) & "))"
        End If
        If TranType <> "" Then strSql += vbCrLf + " AND I.TRANTYPE IN (" & TranType & ")"
        If chkCostName <> "" Then strSql += vbCrLf + "  AND I.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkCategory <> "" Then strSql += vbCrLf + "  AND I.CATCODE IN (SELECT CATCODE FROM " & cnAdminDb & "..CATEGORY WHERE CATNAME IN (" & chkCategory & "))"
        strSql += vbCrLf + " AND NOT EXISTS (SELECT 1 FROM " & cnStockDb & "..RECEIPT WHERE BATCHNO = I.BATCHNO AND TRANTYPE = 'RD')"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = " SELECT DISTINCT BATCHNO INTO TEMP" & systemId & "ORDREP_DEL "
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSUE AS I"
        strSql += vbCrLf + " WHERE TRANTYPE IN ('OD') AND ISNULL(CANCEL,'') = ''"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = " CREATE TABLE TEMP" & systemId & "GSSTOCK"
        strSql += " ("
        strSql += " PARTICULAR VARCHAR(500)"
        strSql += vbCrLf + " ,TRANNO INT,TTRANDATE VARCHAR(10)"
        If rdbBothwt.Checked Then
            strSql += vbCrLf + " ,[RECEIPT PCS] INT,[RECEIPT " & bothWeight(0) & "] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[RECEIPT " & bothWeight(1) & "] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[RECEIPT STNPCS] INT,[RECEIPT STNWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[RECEIPT DIAPCS] INT,[RECEIPT DIAWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[RECEIPT PREPCS] INT,[RECEIPT PREWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[PUR PCS] INT,[PUR " & bothWeight(0) & "] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[PUR " & bothWeight(1) & "] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[PUR STNPCS] INT,[PUR STNWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[PUR DIAPCS] INT,[PUR DIAWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[PUR PREPCS] INT,[PUR PREWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[T] VARCHAR(1)"
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
            strSql += vbCrLf + " ,[PUR PCS] INT,[PUR " & Weight & "] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[PUR STNPCS] INT,[PUR STNWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[PUR DIAPCS] INT,[PUR DIAWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[PUR PREPCS] INT,[PUR PREWT] NUMERIC(15,3)"
            strSql += vbCrLf + " ,[T] VARCHAR(1)"
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

        strSql = " CREATE TABLE TEMP" & systemId & "GSSTOCK_1"
        strSql += vbCrLf + " ("
        strSql += vbCrLf + " SEP VARCHAR(10),TRANTYPE VARCHAR(20),TRANNO INT,PCS INT"
        If rdbBothwt.Checked Then
            strSql += vbCrLf + "," & bothWeight(0) & " NUMERIC(15,3)"
            strSql += vbCrLf + "," & bothWeight(1) & " NUMERIC(15,3)"
        Else
            strSql += vbCrLf + "," & Weight & " NUMERIC(15,3)"
        End If
        strSql += vbCrLf + " ,STNPCS INT,STNWT NUMERIC(15,3),DIAPCS INT,DIAWT NUMERIC(15,3),PREPCS INT,PREWT NUMERIC(15,3)"
        strSql += vbCrLf + " ,CATNAME VARCHAR(100),METAL VARCHAR(100),GS VARCHAR(100)"
        strSql += vbCrLf + " ,TRANDATE SMALLDATETIME,ACNAME VARCHAR(200)"
        strSql += vbCrLf + " ,ITEMNAME VARCHAR(80)"
        strSql += vbCrLf + " )"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000 : cmd.ExecuteNonQuery()

        strSql = " /** INSERTING OPENING FROM OPENWEIGHT **/"
        strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK_1"
        strSql += vbCrLf + "  	SELECT "
        strSql += vbCrLf + "  	CASE WHEN I.TRANTYPE = 'I' THEN 'ISS' ELSE 'REC' END SEP"
        strSql += vbCrLf + "  	,CASE WHEN I.TRANTYPE = 'I' THEN 'IIS' ELSE 'RPU' END TRANTYPE"
        strSql += vbCrLf + "  	,NULL TRANNO"
        strSql += vbCrLf + "    ,SUM(CASE WHEN (I.SUBITEMID = 0 AND IM.BOOKSTOCK IN ('P','B')) OR (I.SUBITEMID <> 0 AND SM.BOOKSTOCK IN ('P','B')) THEN I.PCS ELSE 0 END) AS PCS"
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


        strSql = " INSERT INTO TEMP" & systemId & "GSSTOCK"
        strSql += " (PARTICULAR "
        strSql += vbCrLf + " ,TRANNO,TTRANDATE"
        If rdbBothwt.Checked Then
            strSql += vbCrLf + " ,[RECEIPT PCS],[RECEIPT " & bothWeight(0) & "] "
            strSql += vbCrLf + " ,[RECEIPT " & bothWeight(1) & "] "
            strSql += vbCrLf + " ,[RECEIPT STNPCS] ,[RECEIPT STNWT] "
            strSql += vbCrLf + " ,[RECEIPT DIAPCS] ,[RECEIPT DIAWT] "
            strSql += vbCrLf + " ,[RECEIPT PREPCS] ,[RECEIPT PREWT] "

            strSql += vbCrLf + " ,[PUR PCS],[PUR " & bothWeight(0) & "] "
            strSql += vbCrLf + " ,[PUR " & bothWeight(1) & "] "
            strSql += vbCrLf + " ,[PUR STNPCS] ,[PUR STNWT] "
            strSql += vbCrLf + " ,[PUR DIAPCS] ,[PUR DIAWT] "
            strSql += vbCrLf + " ,[PUR PREPCS] ,[PUR PREWT] "

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

            strSql += vbCrLf + " ,[PUR PCS],[PUR " & Weight & "] "
            strSql += vbCrLf + " ,[PUR STNPCS] ,[PUR STNWT] "
            strSql += vbCrLf + " ,[PUR DIAPCS] ,[PUR DIAWT] "
            strSql += vbCrLf + " ,[PUR PREPCS] ,[PUR PREWT] "

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
        If rdbBothwt.Checked Then
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE<>'RPU' THEN PCS ELSE 0 END) AS [RECEIPT PCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE<>'RPU' THEN " & bothWeight(0) & " ELSE 0 END) AS [RECEIPT " & bothWeight(0) & "]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE<>'RPU' THEN " & bothWeight(1) & " ELSE 0 END) AS [RECEIPT " & bothWeight(1) & "]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE<>'RPU' THEN STNPCS ELSE 0 END) AS [RECEIPT STNPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE<>'RPU' THEN STNWT ELSE 0 END) AS [RECEIPT STNWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE<>'RPU' THEN DIAPCS ELSE 0 END) AS [RECEIPT DIAPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE<>'RPU' THEN DIAWT ELSE 0 END) AS [RECEIPT DIAWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE<>'RPU' THEN PREPCS ELSE 0 END) AS [RECEIPT PREPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE<>'RPU' THEN PREWT ELSE 0 END) AS [RECEIPT PREWT]"

            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE='RPU' THEN PCS ELSE 0 END) AS [PUR PCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE='RPU' THEN " & bothWeight(0) & " ELSE 0 END) AS [PUR " & bothWeight(0) & "]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE='RPU' THEN " & bothWeight(1) & " ELSE 0 END) AS [PUR " & bothWeight(1) & "]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE='RPU' THEN STNPCS ELSE 0 END) AS [PUR STNPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE='RPU' THEN STNWT ELSE 0 END) AS [PUR STNWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE='RPU' THEN DIAPCS ELSE 0 END) AS [PUR DIAPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE='RPU' THEN DIAWT ELSE 0 END) AS [PUR DIAWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE='RPU' THEN PREPCS ELSE 0 END) AS [PUR PREPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE='RPU' THEN PREWT ELSE 0 END) AS [PUR PREWT]"

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
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN -1*STNPCS ELSE 0 END) AS [CLOSING STNPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN -1*STNWT ELSE 0 END) AS [CLOSING STNWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN -1*DIAPCS ELSE 0 END) AS [CLOSING DIAPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN -1*DIAWT ELSE 0 END) AS [CLOSING DIAWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN -1*PREPCS ELSE 0 END) AS [CLOSING PREPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' THEN -1*PREWT ELSE 0 END) AS [CLOSING PREWT]"
        Else
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE<>'RPU' THEN PCS ELSE 0 END) AS [RECEIPT PCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE<>'RPU' THEN " & Weight & " ELSE 0 END) AS [RECEIPT " & Weight & "]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE<>'RPU' THEN STNPCS ELSE 0 END) AS [RECEIPT STNPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE<>'RPU' THEN STNWT ELSE 0 END) AS [RECEIPT STNWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE<>'RPU' THEN DIAPCS ELSE 0 END) AS [RECEIPT DIAPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE<>'RPU' THEN DIAWT ELSE 0 END) AS [RECEIPT DIAWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE<>'RPU' THEN PREPCS ELSE 0 END) AS [RECEIPT PREPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE<>'RPU' THEN PREWT ELSE 0 END) AS [RECEIPT PREWT]"

            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE='RPU' THEN PCS ELSE 0 END) AS [PUR PCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE='RPU' THEN " & Weight & " ELSE 0 END) AS [PUR " & Weight & "]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE='RPU' THEN STNPCS ELSE 0 END) AS [PUR STNPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE='RPU' THEN STNWT ELSE 0 END) AS [PUR STNWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE='RPU' THEN DIAPCS ELSE 0 END) AS [PUR DIAPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE='RPU' THEN DIAWT ELSE 0 END) AS [PUR DIAWT]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE='RPU' THEN PREPCS ELSE 0 END) AS [PUR PREPCS]"
            strSql += vbCrLf + " ,SUM(CASE WHEN SEP = 'REC' AND TRANTYPE='RPU' THEN PREWT ELSE 0 END) AS [PUR PREWT]"


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

        If chkCategorywise.Checked = False Then
            strSql += vbCrLf + " ," & IIf(rbtPcs.Checked, "CATNAME", "NULL") & " AS CATNAME"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(100),NULL) AS METAL"
            strSql += vbCrLf + " ," & IIf(rbtPcs.Checked, "CONVERT(vARCHAR,NULL)AS GS", "GS") & ",TRANDATE,ACNAME"
            strSql += vbCrLf + " ," & IIf(rbtPcs.Checked, "ITEMNAME", "NULL") & " AS ITEMNAME,1 AS RESULT,NULL COLHEAD"
            strSql += vbCrLf + " FROM TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " GROUP BY TRANNO,TRANDATE,ACNAME,GS"
            If rbtPcs.Checked Then
                strSql += vbCrLf + ",CATNAME,GS,ITEMNAME"
            End If
        ElseIf chkCategorywise.Checked Then
            strSql += vbCrLf + " , CATNAME"
            strSql += vbCrLf + " ,CONVERT(VARCHAR(100),NULL) AS METAL"
            strSql += vbCrLf + " ,GS,TRANDATE,ACNAME"
            strSql += vbCrLf + " , ITEMNAME,1 AS RESULT,NULL COLHEAD"
            strSql += vbCrLf + " FROM TEMP" & systemId & "GSSTOCK_1"
            strSql += vbCrLf + " GROUP BY TRANNO,TRANDATE,ACNAME,GS"
            strSql += vbCrLf + ",CATNAME,GS,ITEMNAME"
        End If
        strSql += vbCrLf + " )X"
        If rbtBoth.Checked Then
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " WHERE NOT ([RECEIPT " & bothWeight(0) & "] = 0 AND [RECEIPT " & bothWeight(1) & "] = 0 AND [PUR " & bothWeight(0) & "] = 0 AND [PUR " & bothWeight(1) & "] = 0  AND [ISSUE " & bothWeight(0) & "] = 0 AND [ISSUE " & bothWeight(1) & "] = 0 AND [RECEIPT PCS] = 0 AND [PUR PCS] = 0 AND [ISSUE PCS] = 0)"
            Else
                strSql += vbCrLf + " WHERE NOT ([RECEIPT " & Weight & "] = 0 AND [PUR " & Weight & "] = 0 AND [ISSUE " & Weight & "] = 0 AND [RECEIPT PCS] = 0 AND [PUR PCS] = 0 AND [ISSUE PCS] = 0)"
            End If
        ElseIf rbtWeight.Checked Then
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " WHERE NOT ([RECEIPT " & bothWeight(0) & "] = 0 AND [RECEIPT " & bothWeight(1) & "] = 0"
                strSql += vbCrLf + " AND [PUR " & bothWeight(0) & "] = 0 AND [PUR " & bothWeight(1) & "] = 0"
                strSql += vbCrLf + " AND [ISSUE " & bothWeight(0) & "] = 0 AND [ISSUE " & bothWeight(1) & "] = 0)"
            Else
                strSql += vbCrLf + " WHERE NOT ([RECEIPT " & Weight & "] = 0 AND [PUR " & Weight & "] = 0 AND [ISSUE " & Weight & "] = 0)"
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
            strSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "GSSTOCK)>0"
            strSql += vbCrLf + "   BEGIN"
            strSql += vbCrLf + "   INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,PARTICULAR,RESULT,COLHEAD)"
            strSql += vbCrLf + "   SELECT DISTINCT METAL,GS,' '+GS,0 RESULT,'T'COLHEAD FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1  "
            strSql += vbCrLf + "   AND (ISNULL(METAL,'') <> '' or ISNULL(GS,'') <> '')"
            If rbtTranno.Checked Then
                strSql += vbCrLf + "   INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,TRANDATE,CATNAME,ITEMNAME,PARTICULAR"
                If rdbBothwt.Checked Then
                    strSql += vbCrLf + " ,[RECEIPT " & bothWeight(0) & "]"
                    strSql += vbCrLf + " ,[RECEIPT " & bothWeight(1) & "]"
                    strSql += vbCrLf + " ,[RECEIPT STNWT],[RECEIPT DIAWT],[RECEIPT PREWT]"

                    strSql += vbCrLf + " ,[PUR " & bothWeight(0) & "]"
                    strSql += vbCrLf + " ,[PUR " & bothWeight(1) & "]"
                    strSql += vbCrLf + " ,[PUR STNWT],[PUR DIAWT],[PUR PREWT]"

                    strSql += vbCrLf + " ,[ISSUE " & bothWeight(0) & "]"
                    strSql += vbCrLf + " ,[ISSUE " & bothWeight(1) & "]"
                    strSql += vbCrLf + " ,[ISSUE STNWT],[ISSUE DIAWT],[ISSUE PREWT]"
                Else
                    strSql += vbCrLf + " ,[RECEIPT " & Weight & "]"
                    strSql += vbCrLf + " ,[RECEIPT STNWT],[RECEIPT DIAWT],[RECEIPT PREWT]"
                    strSql += vbCrLf + " ,[PUR " & Weight & "]"
                    strSql += vbCrLf + " ,[PUR STNWT],[PUR DIAWT],[PUR PREWT]"

                    strSql += vbCrLf + " ,[ISSUE " & Weight & "]"
                    strSql += vbCrLf + " ,[ISSUE STNWT],[ISSUE DIAWT],[ISSUE PREWT]"
                End If
                strSql += vbCrLf + " ,[RECEIPT PCS]"
                strSql += vbCrLf + " ,[RECEIPT STNPCS],[RECEIPT DIAPCS],[RECEIPT PREPCS]"
                strSql += vbCrLf + " ,[PUR PCS]"
                strSql += vbCrLf + " ,[PUR STNPCS],[PUR DIAPCS],[PUR PREPCS]"
                strSql += vbCrLf + " ,[ISSUE PCS]"
                strSql += vbCrLf + " ,[ISSUE STNPCS],[ISSUE DIAPCS],[ISSUE PREPCS]"
                strSql += vbCrLf + " ,RESULT,COLHEAD)"
                strSql += vbCrLf + " SELECT METAL,GS,TRANDATE,CATNAME,ITEMNAME,'   '+'SUB TOTAL'"
                If rdbBothwt.Checked Then
                    strSql += vbCrLf + " ,SUM([RECEIPT " & bothWeight(0) & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT " & bothWeight(1) & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT STNWT]),SUM([RECEIPT DIAWT]),SUM([RECEIPT PREWT])"
                    strSql += vbCrLf + " ,SUM([PUR " & bothWeight(0) & "])"
                    strSql += vbCrLf + " ,SUM([PUR " & bothWeight(1) & "])"
                    strSql += vbCrLf + " ,SUM([PUR STNWT]),SUM([PUR DIAWT]),SUM([PUR PREWT])"
                    strSql += vbCrLf + " ,SUM([ISSUE " & bothWeight(0) & "])"
                    strSql += vbCrLf + " ,SUM([ISSUE " & bothWeight(1) & "])"
                    strSql += vbCrLf + " ,SUM([ISSUE STNWT]),SUM([ISSUE DIAWT]),SUM([ISSUE PREWT])"
                Else
                    strSql += vbCrLf + " ,SUM([RECEIPT " & Weight & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT STNWT]),SUM([RECEIPT DIAWT]),SUM([RECEIPT PREWT])"
                    strSql += vbCrLf + " ,SUM([PUR " & Weight & "])"
                    strSql += vbCrLf + " ,SUM([PUR STNWT]),SUM([PUR DIAWT]),SUM([PUR PREWT])"
                    strSql += vbCrLf + " ,SUM([ISSUE " & Weight & "])"
                    strSql += vbCrLf + " ,SUM([ISSUE STNWT]),SUM([ISSUE DIAWT]),SUM([ISSUE PREWT])"
                End If
                strSql += vbCrLf + " ,SUM([RECEIPT PCS])"
                strSql += vbCrLf + " ,SUM([RECEIPT STNPCS]),SUM([RECEIPT DIAPCS]),SUM([RECEIPT PREPCS])"
                strSql += vbCrLf + " ,SUM([PUR PCS])"
                strSql += vbCrLf + " ,SUM([PUR STNPCS]),SUM([PUR DIAPCS]),SUM([PUR PREPCS])"
                strSql += vbCrLf + " ,SUM([ISSUE PCS])"
                strSql += vbCrLf + " ,SUM([ISSUE STNPCS]),SUM([ISSUE DIAPCS]),SUM([ISSUE PREPCS])"
                strSql += vbCrLf + " ,4,'S2'COLHEAD"
                strSql += vbCrLf + " FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 AND TRANDATE IS NOT NULL GROUP BY METAL,GS,TRANDATE,CATNAME,ITEMNAME"
            End If
            strSql += vbCrLf + "  "
            If rbtPcs.Checked = False Then
                strSql += vbCrLf + "   INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,TRANDATE,PARTICULAR"
                If rdbBothwt.Checked Then
                    strSql += vbCrLf + " ,[RECEIPT " & bothWeight(0) & "]"
                    strSql += vbCrLf + " ,[RECEIPT " & bothWeight(1) & "]"
                    strSql += vbCrLf + " ,[RECEIPT STNWT],[RECEIPT DIAWT],[RECEIPT PREWT]"
                    strSql += vbCrLf + " ,[PUR " & bothWeight(0) & "]"
                    strSql += vbCrLf + " ,[PUR " & bothWeight(1) & "]"
                    strSql += vbCrLf + " ,[PUR STNWT],[PUR DIAWT],[PUR PREWT]"
                    strSql += vbCrLf + " ,[ISSUE " & bothWeight(0) & "]"
                    strSql += vbCrLf + " ,[ISSUE " & bothWeight(1) & "]"
                    strSql += vbCrLf + " ,[ISSUE STNWT],[ISSUE DIAWT],[ISSUE PREWT]"
                Else
                    strSql += vbCrLf + " ,[RECEIPT " & Weight & "]"
                    strSql += vbCrLf + " ,[RECEIPT STNWT],[RECEIPT DIAWT],[RECEIPT PREWT]"
                    strSql += vbCrLf + " ,[PUR " & Weight & "]"
                    strSql += vbCrLf + " ,[PUR STNWT],[PUR DIAWT],[PUR PREWT]"
                    strSql += vbCrLf + " ,[ISSUE " & Weight & "]"
                    strSql += vbCrLf + " ,[ISSUE STNWT],[ISSUE DIAWT],[ISSUE PREWT]"
                End If
                strSql += vbCrLf + " ,[RECEIPT PCS]"
                strSql += vbCrLf + " ,[RECEIPT STNPCS],[RECEIPT DIAPCS],[RECEIPT PREPCS]"
                strSql += vbCrLf + " ,[PUR PCS]"
                strSql += vbCrLf + " ,[PUR STNPCS],[PUR DIAPCS],[PUR PREPCS]"
                strSql += vbCrLf + " ,[ISSUE PCS]"
                strSql += vbCrLf + " ,[ISSUE STNPCS],[ISSUE DIAPCS],[ISSUE PREPCS]"
                strSql += vbCrLf + " ,RESULT,COLHEAD)"
                strSql += vbCrLf + " SELECT METAL,GS,'" & _MaxDate.ToString("yyyy-MM-dd") & "','  '+GS+'=>TOTAL'"
                If rdbBothwt.Checked Then
                    strSql += vbCrLf + " ,SUM([RECEIPT " & bothWeight(0) & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT " & bothWeight(1) & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT STNWT]),SUM([RECEIPT DIAWT]),SUM([RECEIPT PREWT])"
                    strSql += vbCrLf + " ,SUM([PUR " & bothWeight(0) & "])"
                    strSql += vbCrLf + " ,SUM([PUR " & bothWeight(1) & "])"
                    strSql += vbCrLf + " ,SUM([PUR STNWT]),SUM([PUR DIAWT]),SUM([PUR PREWT])"
                    strSql += vbCrLf + " ,SUM([ISSUE " & bothWeight(0) & "])"
                    strSql += vbCrLf + " ,SUM([ISSUE " & bothWeight(1) & "])"
                    strSql += vbCrLf + " ,SUM([ISSUE STNWT]),SUM([ISSUE DIAWT]),SUM([ISSUE PREWT])"
                Else
                    strSql += vbCrLf + " ,SUM([RECEIPT " & Weight & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT STNWT]),SUM([RECEIPT DIAWT]),SUM([RECEIPT PREWT])"
                    strSql += vbCrLf + " ,SUM([PUR " & Weight & "])"
                    strSql += vbCrLf + " ,SUM([PUR STNWT]),SUM([PUR DIAWT]),SUM([PUR PREWT])"
                    strSql += vbCrLf + " ,SUM([ISSUE " & Weight & "])"
                    strSql += vbCrLf + " ,SUM([ISSUE STNWT]),SUM([ISSUE DIAWT]),SUM([ISSUE PREWT])"
                End If
                strSql += vbCrLf + " ,SUM([RECEIPT PCS])"
                strSql += vbCrLf + " ,SUM([RECEIPT STNPCS]),SUM([RECEIPT DIAPCS]),SUM([RECEIPT PREPCS])"
                strSql += vbCrLf + " ,SUM([PUR PCS])"
                strSql += vbCrLf + " ,SUM([PUR STNPCS]),SUM([PUR DIAPCS]),SUM([PUR PREPCS])"
                strSql += vbCrLf + " ,SUM([ISSUE PCS])"
                strSql += vbCrLf + " ,SUM([ISSUE STNPCS]),SUM([ISSUE DIAPCS]),SUM([ISSUE PREPCS])"
                strSql += vbCrLf + " ,3,'S'COLHEAD"
                strSql += vbCrLf + "   FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 AND TRANDATE IS NOT NULL GROUP BY METAL,GS"
                strSql += vbCrLf + "  "
            Else
                strSql += vbCrLf + "   INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,CATNAME,PARTICULAR,RESULT,COLHEAD)"
                strSql += vbCrLf + "   SELECT DISTINCT METAL,GS,CATNAME,CATNAME,0 RESULT,'T'COLHEAD FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1"
                strSql += vbCrLf + "   INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,CATNAME,ITEMNAME,TRANDATE,PARTICULAR"
                If rdbBothwt.Checked Then
                    strSql += vbCrLf + " ,[RECEIPT " & bothWeight(0) & "]"
                    strSql += vbCrLf + " ,[RECEIPT " & bothWeight(1) & "]"
                    strSql += vbCrLf + " ,[RECEIPT STNWT],[RECEIPT DIAWT],[RECEIPT PREWT]"
                    strSql += vbCrLf + " ,[PUR " & bothWeight(0) & "]"
                    strSql += vbCrLf + " ,[PUR " & bothWeight(1) & "]"
                    strSql += vbCrLf + " ,[PUR STNWT],[PUR DIAWT],[PUR PREWT]"
                    strSql += vbCrLf + " ,[ISSUE " & bothWeight(0) & "]"
                    strSql += vbCrLf + " ,[ISSUE " & bothWeight(1) & "]"
                    strSql += vbCrLf + " ,[ISSUE STNWT],[ISSUE DIAWT],[ISSUE PREWT]"
                Else
                    strSql += vbCrLf + " ,[RECEIPT " & Weight & "]"
                    strSql += vbCrLf + " ,[RECEIPT STNWT],[RECEIPT DIAWT],[RECEIPT PREWT]"
                    strSql += vbCrLf + " ,[PUR " & Weight & "]"
                    strSql += vbCrLf + " ,[PUR STNWT],[PUR DIAWT],[PUR PREWT]"
                    strSql += vbCrLf + " ,[ISSUE " & Weight & "]"
                    strSql += vbCrLf + " ,[ISSUE STNWT],[ISSUE DIAWT],[ISSUE PREWT]"
                End If
                strSql += vbCrLf + " ,[RECEIPT PCS]"
                strSql += vbCrLf + " ,[RECEIPT STNPCS],[RECEIPT DIAPCS],[RECEIPT PREPCS]"
                strSql += vbCrLf + " ,[PUR PCS]"
                strSql += vbCrLf + " ,[PUR STNPCS],[PUR DIAPCS],[PUR PREPCS]"

                strSql += vbCrLf + " ,[ISSUE PCS]"
                strSql += vbCrLf + " ,[ISSUE STNPCS],[ISSUE DIAPCS],[ISSUE PREPCS]"
                strSql += vbCrLf + " ,RESULT,COLHEAD)"
                strSql += vbCrLf + "   SELECT METAL,GS,CATNAME,ITEMNAME,'" & _MaxDate.ToString("yyyy-MM-dd") & "',' '+ITEMNAME+' TOT'"
                If rdbBothwt.Checked Then
                    strSql += vbCrLf + " ,SUM([RECEIPT " & bothWeight(0) & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT " & bothWeight(1) & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT STNWT]),SUM([RECEIPT DIAWT]),SUM([RECEIPT PREWT])"
                    strSql += vbCrLf + " ,SUM([PUR " & bothWeight(0) & "])"
                    strSql += vbCrLf + " ,SUM([PUR " & bothWeight(1) & "])"
                    strSql += vbCrLf + " ,SUM([PUR STNWT]),SUM([PUR DIAWT]),SUM([PUR PREWT])"
                    strSql += vbCrLf + " ,SUM([ISSUE " & bothWeight(0) & "])"
                    strSql += vbCrLf + " ,SUM([ISSUE " & bothWeight(1) & "])"
                    strSql += vbCrLf + " ,SUM([ISSUE STNWT]),SUM([ISSUE DIAWT]),SUM([ISSUE PREWT])"
                Else
                    strSql += vbCrLf + " ,SUM([RECEIPT " & Weight & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT STNWT]),SUM([RECEIPT DIAWT]),SUM([RECEIPT PREWT])"
                    strSql += vbCrLf + " ,SUM([PUR " & Weight & "])"
                    strSql += vbCrLf + " ,SUM([PUR STNWT]),SUM([PUR DIAWT]),SUM([PUR PREWT])"
                    strSql += vbCrLf + " ,SUM([ISSUE " & Weight & "])"
                    strSql += vbCrLf + " ,SUM([ISSUE STNWT]),SUM([ISSUE DIAWT]),SUM([ISSUE PREWT])"
                End If
                strSql += vbCrLf + " ,SUM([RECEIPT PCS])"
                strSql += vbCrLf + " ,SUM([RECEIPT STNPCS]),SUM([RECEIPT DIAPCS]),SUM([RECEIPT PREPCS])"
                strSql += vbCrLf + " ,SUM([PUR PCS])"
                strSql += vbCrLf + " ,SUM([PUR STNPCS]),SUM([PUR DIAPCS]),SUM([PUR PREPCS])"
                strSql += vbCrLf + " ,SUM([ISSUE PCS])"
                strSql += vbCrLf + " ,SUM([ISSUE STNPCS]),SUM([ISSUE DIAPCS]),SUM([ISSUE PREPCS])"
                strSql += vbCrLf + " ,3,'S1'COLHEAD"
                strSql += vbCrLf + " FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 AND TRANDATE IS NOT NULL GROUP BY METAL,GS,CATNAME,ITEMNAME"
            End If
            strSql += vbCrLf + "   END"
        ElseIf chkCategorywise.Checked = True Then
            strSql = " IF (SELECT COUNT(*) FROM TEMP" & systemId & "GSSTOCK)>0"
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,PARTICULAR,RESULT,COLHEAD)"
            strSql += vbCrLf + " SELECT DISTINCT METAL,GS,' '+GS,0 RESULT,'T'COLHEAD FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 "
            strSql += vbCrLf + " AND (ISNULL(METAL,'') <> '' or ISNULL(GS,'') <> '')"
            If rbtTranno.Checked Then
                strSql += vbCrLf + "   INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,TRANDATE,CATNAME,PARTICULAR"
                If rdbBothwt.Checked Then
                    strSql += vbCrLf + " ,[RECEIPT " & bothWeight(0) & "]"
                    strSql += vbCrLf + " ,[RECEIPT " & bothWeight(1) & "]"
                    strSql += vbCrLf + " ,[RECEIPT STNWT],[RECEIPT DIAWT],[RECEIPT PREWT]"
                    strSql += vbCrLf + " ,[PUR " & bothWeight(0) & "]"
                    strSql += vbCrLf + " ,[PUR " & bothWeight(1) & "]"
                    strSql += vbCrLf + " ,[PUR STNWT],[PUR DIAWT],[PUR PREWT]"
                    strSql += vbCrLf + " ,[ISSUE " & bothWeight(0) & "]"
                    strSql += vbCrLf + " ,[ISSUE " & bothWeight(1) & "]"
                    strSql += vbCrLf + " ,[ISSUE STNWT],[ISSUE DIAWT],[ISSUE PREWT]"
                Else
                    strSql += vbCrLf + " ,[RECEIPT " & Weight & "]"
                    strSql += vbCrLf + " ,[RECEIPT STNWT],[RECEIPT DIAWT],[RECEIPT PREWT]"
                    strSql += vbCrLf + " ,[PUR " & Weight & "]"
                    strSql += vbCrLf + " ,[PUR STNWT],[PUR DIAWT],[PUR PREWT]"
                    strSql += vbCrLf + " ,[ISSUE " & Weight & "]"
                    strSql += vbCrLf + " ,[ISSUE STNWT],[ISSUE DIAWT],[ISSUE PREWT]"
                End If
                strSql += vbCrLf + " ,[RECEIPT PCS]"
                strSql += vbCrLf + " ,[RECEIPT STNPCS],[RECEIPT DIAPCS],[RECEIPT PREPCS]"
                strSql += vbCrLf + " ,[PUR PCS]"
                strSql += vbCrLf + " ,[PUR STNPCS],[PUR DIAPCS],[PUR PREPCS]"
                strSql += vbCrLf + " ,[ISSUE PCS]"
                strSql += vbCrLf + " ,[ISSUE STNPCS],[ISSUE DIAPCS],[ISSUE PREPCS]"

                strSql += vbCrLf + " ,RESULT,COLHEAD)"
                strSql += vbCrLf + "   SELECT METAL,GS,TRANDATE,CATNAME,'   '+'SUB TOTAL'"
                If rdbBothwt.Checked Then
                    strSql += vbCrLf + " ,SUM([RECEIPT " & bothWeight(0) & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT " & bothWeight(1) & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT STNWT]),SUM([RECEIPT DIAWT]),SUM([RECEIPT PREWT])"
                    strSql += vbCrLf + " ,SUM([PUR " & bothWeight(0) & "])"
                    strSql += vbCrLf + " ,SUM([PUR " & bothWeight(1) & "])"
                    strSql += vbCrLf + " ,SUM([PUR STNWT]),SUM([PUR DIAWT]),SUM([PUR PREWT])"
                    strSql += vbCrLf + " ,SUM([ISSUE " & bothWeight(0) & "])"
                    strSql += vbCrLf + " ,SUM([ISSUE " & bothWeight(1) & "])"
                    strSql += vbCrLf + " ,SUM([ISSUE STNWT]),SUM([ISSUE DIAWT]),SUM([ISSUE PREWT])"
                    'strSql += vbCrLf + " ,SUM([CLOSING " & bothWeight(0) & "])"
                    'strSql += vbCrLf + " ,SUM([CLOSING " & bothWeight(1) & "])"
                Else
                    strSql += vbCrLf + " ,SUM([RECEIPT " & Weight & "])"
                    strSql += vbCrLf + " ,SUM([RECEIPT STNWT]),SUM([RECEIPT DIAWT]),SUM([RECEIPT PREWT])"
                    strSql += vbCrLf + " ,SUM([PUR " & Weight & "])"
                    strSql += vbCrLf + " ,SUM([PUR STNWT]),SUM([PUR DIAWT]),SUM([PUR PREWT])"
                    strSql += vbCrLf + " ,SUM([ISSUE " & Weight & "])" ',SUM([CLOSING " & Weight & "])"
                    strSql += vbCrLf + " ,SUM([ISSUE STNWT]),SUM([ISSUE DIAWT]),SUM([ISSUE PREWT])"
                End If
                strSql += vbCrLf + " ,SUM([RECEIPT PCS])"
                strSql += vbCrLf + " ,SUM([RECEIPT STNPCS]),SUM([RECEIPT DIAPCS]),SUM([RECEIPT PREPCS])"
                strSql += vbCrLf + " ,SUM([PUR PCS])"
                strSql += vbCrLf + " ,SUM([PUR STNPCS]),SUM([PUR DIAPCS]),SUM([PUR PREPCS])"
                strSql += vbCrLf + " ,SUM([ISSUE PCS])"
                strSql += vbCrLf + " ,SUM([ISSUE STNPCS]),SUM([ISSUE DIAPCS]),SUM([ISSUE PREPCS])"
                strSql += vbCrLf + " ,4,'S2'COLHEAD"
                strSql += vbCrLf + "   FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 AND TRANDATE IS NOT NULL GROUP BY CATNAME,METAL,GS,TRANDATE"
            End If
            strSql += vbCrLf + "  "
            strSql += vbCrLf + "   INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,CATNAME,PARTICULAR,RESULT,COLHEAD)"
            strSql += vbCrLf + "   SELECT DISTINCT METAL,GS,CATNAME,CATNAME,0 RESULT,'T'COLHEAD FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1"
            strSql += vbCrLf + "   INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,CATNAME,PARTICULAR,TRANDATE"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ,[RECEIPT " & bothWeight(0) & "]"
                strSql += vbCrLf + " ,[RECEIPT " & bothWeight(1) & "]"
                strSql += vbCrLf + " ,[RECEIPT STNWT],[RECEIPT DIAWT],[RECEIPT PREWT]"
                strSql += vbCrLf + " ,[PUR " & bothWeight(0) & "]"
                strSql += vbCrLf + " ,[PUR " & bothWeight(1) & "]"
                strSql += vbCrLf + " ,[PUR STNWT],[PUR DIAWT],[PUR PREWT]"
                strSql += vbCrLf + " ,[ISSUE " & bothWeight(0) & "]"
                strSql += vbCrLf + " ,[ISSUE " & bothWeight(1) & "]"
                strSql += vbCrLf + " ,[ISSUE STNWT],[ISSUE DIAWT],[ISSUE PREWT]"
            Else
                strSql += vbCrLf + " ,[RECEIPT " & Weight & "]"
                strSql += vbCrLf + " ,[RECEIPT STNWT],[RECEIPT DIAWT],[RECEIPT PREWT]"
                strSql += vbCrLf + " ,[PUR " & Weight & "]"
                strSql += vbCrLf + " ,[PUR STNWT],[PUR DIAWT],[PUR PREWT]"
                strSql += vbCrLf + " ,[ISSUE " & Weight & "]"
                strSql += vbCrLf + " ,[ISSUE STNWT],[ISSUE DIAWT],[ISSUE PREWT]"
            End If
            strSql += vbCrLf + " ,[RECEIPT PCS]"
            strSql += vbCrLf + " ,[RECEIPT STNPCS],[RECEIPT DIAPCS],[RECEIPT PREPCS]"
            strSql += vbCrLf + " ,[PUR PCS]"
            strSql += vbCrLf + " ,[PUR STNPCS],[PUR DIAPCS],[PUR PREPCS]"
            strSql += vbCrLf + " ,[ISSUE PCS]"
            strSql += vbCrLf + " ,[ISSUE STNPCS],[ISSUE DIAPCS],[ISSUE PREPCS]"
            strSql += vbCrLf + " ,RESULT,COLHEAD)"
            strSql += vbCrLf + "   SELECT METAL,GS,CATNAME,' '+CATNAME+' TOT','" & _MaxDate.ToString("yyyy-MM-dd") & "'" 'ITEMNAME,'" & _MaxDate.ToString("yyyy-MM-dd") & "',' '+ITEMNAME+' TOT'"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ,SUM([RECEIPT " & bothWeight(0) & "])"
                strSql += vbCrLf + " ,SUM([RECEIPT " & bothWeight(1) & "])"
                strSql += vbCrLf + " ,SUM([RECEIPT STNWT]),SUM([RECEIPT DIAWT]),SUM([RECEIPT PREWT])"
                strSql += vbCrLf + " ,SUM([PUR " & bothWeight(0) & "])"
                strSql += vbCrLf + " ,SUM([PUR " & bothWeight(1) & "])"
                strSql += vbCrLf + " ,SUM([PUR STNWT]),SUM([PUR DIAWT]),SUM([PUR PREWT])"
                strSql += vbCrLf + " ,SUM([ISSUE " & bothWeight(0) & "])"
                strSql += vbCrLf + " ,SUM([ISSUE " & bothWeight(1) & "])"
                strSql += vbCrLf + " ,SUM([ISSUE STNWT]),SUM([ISSUE DIAWT]),SUM([ISSUE PREWT])"
                'strSql += vbCrLf + " ,SUM([CLOSING " & bothWeight(0) & "])"
                'strSql += vbCrLf + " ,SUM([CLOSING " & bothWeight(1) & "])"
            Else
                strSql += vbCrLf + " ,SUM([RECEIPT " & Weight & "])"
                strSql += vbCrLf + " ,SUM([RECEIPT STNWT]),SUM([RECEIPT DIAWT]),SUM([RECEIPT PREWT])"
                strSql += vbCrLf + " ,SUM([PUR " & Weight & "])"
                strSql += vbCrLf + " ,SUM([PUR STNWT]),SUM([PUR DIAWT]),SUM([PUR PREWT])"
                strSql += vbCrLf + " ,SUM([ISSUE " & Weight & "])" ',SUM([CLOSING " & Weight & "])"
                strSql += vbCrLf + " ,SUM([ISSUE STNWT]),SUM([ISSUE DIAWT]),SUM([ISSUE PREWT])"
            End If

            strSql += vbCrLf + " ,SUM([RECEIPT PCS])"
            strSql += vbCrLf + " ,SUM([RECEIPT STNPCS]),SUM([RECEIPT DIAPCS]),SUM([RECEIPT PREPCS])"
            strSql += vbCrLf + " ,SUM([PUR PCS])"
            strSql += vbCrLf + " ,SUM([PUR STNPCS]),SUM([PUR DIAPCS]),SUM([PUR PREPCS])"
            strSql += vbCrLf + " ,SUM([ISSUE PCS])"
            strSql += vbCrLf + " ,SUM([ISSUE STNPCS]),SUM([ISSUE DIAPCS]),SUM([ISSUE PREPCS])"
            strSql += vbCrLf + " ,3,'S1'COLHEAD"
            strSql += vbCrLf + "   FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1 AND TRANDATE IS NOT NULL GROUP BY METAL,GS,CATNAME"
            strSql += vbCrLf + "   END"
        End If
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        strSql = "  IF (SELECT COUNT(*) FROM TEMP" & systemId & "GSSTOCK)>0"
        strSql += vbCrLf + "   BEGIN"
        strSql += vbCrLf + "   INSERT INTO TEMP" & systemId & "GSSTOCK(METAL,GS,CATNAME,PARTICULAR,TRANDATE"
        If rdbBothwt.Checked Then
            strSql += vbCrLf + " ,[RECEIPT " & bothWeight(0) & "]"
            strSql += vbCrLf + " ,[RECEIPT " & bothWeight(1) & "]"
            strSql += vbCrLf + " ,[RECEIPT STNWT],[RECEIPT DIAWT],[RECEIPT PREWT]"
            strSql += vbCrLf + " ,[PUR " & bothWeight(0) & "]"
            strSql += vbCrLf + " ,[PUR " & bothWeight(1) & "]"
            strSql += vbCrLf + " ,[PUR STNWT],[PUR DIAWT],[PUR PREWT]"
            strSql += vbCrLf + " ,[ISSUE " & bothWeight(0) & "]"
            strSql += vbCrLf + " ,[ISSUE " & bothWeight(1) & "]"
            strSql += vbCrLf + " ,[ISSUE STNWT],[ISSUE DIAWT],[ISSUE PREWT]"
        Else
            strSql += vbCrLf + " ,[RECEIPT " & Weight & "]"
            strSql += vbCrLf + " ,[RECEIPT STNWT],[RECEIPT DIAWT],[RECEIPT PREWT]"
            strSql += vbCrLf + " ,[PUR " & Weight & "]"
            strSql += vbCrLf + " ,[PUR STNWT],[PUR DIAWT],[PUR PREWT]"
            strSql += vbCrLf + " ,[ISSUE " & Weight & "]"
            strSql += vbCrLf + " ,[ISSUE STNWT],[ISSUE DIAWT],[ISSUE PREWT]"
        End If
        strSql += vbCrLf + " ,[RECEIPT PCS]"
        strSql += vbCrLf + " ,[RECEIPT STNPCS],[RECEIPT DIAPCS],[RECEIPT PREPCS]"
        strSql += vbCrLf + " ,[PUR PCS]"
        strSql += vbCrLf + " ,[PUR STNPCS],[PUR DIAPCS],[PUR PREPCS]"
        strSql += vbCrLf + " ,[ISSUE PCS]"
        strSql += vbCrLf + " ,[ISSUE STNPCS],[ISSUE DIAPCS],[ISSUE PREPCS]"
        strSql += vbCrLf + " ,RESULT,COLHEAD)"
        strSql += vbCrLf + "   SELECT METAL,GS,''CATNAME,'YEARLY TOTAL','" & _MaxDate.ToString("yyyy-MM-dd") & "'" 'ITEMNAME,'" & _MaxDate.ToString("yyyy-MM-dd") & "',' '+ITEMNAME+' TOT'"
        If rdbBothwt.Checked Then
            strSql += vbCrLf + " ,SUM([RECEIPT " & bothWeight(0) & "])"
            strSql += vbCrLf + " ,SUM([RECEIPT " & bothWeight(1) & "])"
            strSql += vbCrLf + " ,SUM([RECEIPT STNWT]),SUM([RECEIPT DIAWT]),SUM([RECEIPT PREWT])"
            strSql += vbCrLf + " ,SUM([PUR " & bothWeight(0) & "])"
            strSql += vbCrLf + " ,SUM([PUR " & bothWeight(1) & "])"
            strSql += vbCrLf + " ,SUM([PUR STNWT]),SUM([PUR DIAWT]),SUM([PUR PREWT])"
            strSql += vbCrLf + " ,SUM([ISSUE " & bothWeight(0) & "])"
            strSql += vbCrLf + " ,SUM([ISSUE " & bothWeight(1) & "])"
            strSql += vbCrLf + " ,SUM([ISSUE STNWT]),SUM([ISSUE DIAWT]),SUM([ISSUE PREWT])"
            'strSql += vbCrLf + " ,SUM([CLOSING " & bothWeight(0) & "])"
            'strSql += vbCrLf + " ,SUM([CLOSING " & bothWeight(1) & "])"
        Else
            strSql += vbCrLf + " ,SUM([RECEIPT " & Weight & "])"
            strSql += vbCrLf + " ,SUM([RECEIPT STNWT]),SUM([RECEIPT DIAWT]),SUM([RECEIPT PREWT])"
            strSql += vbCrLf + " ,SUM([PUR " & Weight & "])"
            strSql += vbCrLf + " ,SUM([PUR STNWT]),SUM([PUR DIAWT]),SUM([PUR PREWT])"
            strSql += vbCrLf + " ,SUM([ISSUE " & Weight & "])" ',SUM([CLOSING " & Weight & "])"
            strSql += vbCrLf + " ,SUM([ISSUE STNWT]),SUM([ISSUE DIAWT]),SUM([ISSUE PREWT])"
        End If

        strSql += vbCrLf + " ,SUM([RECEIPT PCS])"
        strSql += vbCrLf + " ,SUM([RECEIPT STNPCS]),SUM([RECEIPT DIAPCS]),SUM([RECEIPT PREPCS])"
        strSql += vbCrLf + " ,SUM([PUR PCS])"
        strSql += vbCrLf + " ,SUM([PUR STNPCS]),SUM([PUR DIAPCS]),SUM([PUR PREPCS])"
        strSql += vbCrLf + " ,SUM([ISSUE PCS])"
        strSql += vbCrLf + " ,SUM([ISSUE STNPCS]),SUM([ISSUE DIAPCS]),SUM([ISSUE PREPCS])"
        strSql += vbCrLf + " ,3,'S1'COLHEAD"
        strSql += vbCrLf + "   FROM TEMP" & systemId & "GSSTOCK WHERE RESULT = 1  GROUP BY METAL,GS"
        strSql += vbCrLf + "   END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " UPDATE TEMP" & systemId & "GSSTOCK SET PARTICULAR = '   '+'OPENING..' WHERE RESULT = 1 AND TRANDATE IS NULL"
        'strSql += vbCrLf + " UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT PCS] = NULL,[ISSUE PCS] = NULL  WHERE RESULT = 1 AND TRANDATE IS NULL"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET PARTICULAR = '   '+ACNAME WHERE RESULT = 1 AND TRANDATE IS NOT NULL"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT PCS] = NULL WHERE [RECEIPT PCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [PUR PCS] = NULL WHERE [PUR PCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [ISSUE PCS] = NULL WHERE [ISSUE PCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [CLOSING PCS] = NULL WHERE [CLOSING PCS] = 0"

        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT STNPCS] = NULL WHERE [RECEIPT STNPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [PUR STNPCS] = NULL WHERE [PUR STNPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [ISSUE STNPCS] = NULL WHERE [ISSUE STNPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [CLOSING STNPCS] = NULL WHERE [CLOSING STNPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT DIAPCS] = NULL WHERE [RECEIPT DIAPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [PUR DIAPCS] = NULL WHERE [PUR DIAPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [ISSUE DIAPCS] = NULL WHERE [ISSUE DIAPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [CLOSING DIAPCS] = NULL WHERE [CLOSING DIAPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT PREPCS] = NULL WHERE [RECEIPT PREPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [PUR PREPCS] = NULL WHERE [PUR PREPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [ISSUE PREPCS] = NULL WHERE [ISSUE PREPCS] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [CLOSING PREPCS] = NULL WHERE [CLOSING PREPCS] = 0"

        If rdbBothwt.Checked Then
            strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT " & bothWeight(0) & "] = NULL WHERE [RECEIPT " & bothWeight(0) & "] = 0"
            strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT " & bothWeight(1) & "] = NULL WHERE [RECEIPT " & bothWeight(1) & "] = 0"
            strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [PUR " & bothWeight(0) & "] = NULL WHERE [PUR " & bothWeight(0) & "] = 0"
            strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [PUR " & bothWeight(1) & "] = NULL WHERE [PUR " & bothWeight(1) & "] = 0"
            strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [ISSUE " & bothWeight(0) & "] = NULL WHERE [ISSUE " & bothWeight(0) & "] = 0"
            strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [ISSUE " & bothWeight(1) & "] = NULL WHERE [ISSUE " & bothWeight(1) & "] = 0"
            strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [CLOSING " & bothWeight(0) & "] = NULL WHERE [CLOSING " & bothWeight(0) & "] = 0"
            strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [CLOSING " & bothWeight(1) & "] = NULL WHERE [CLOSING " & bothWeight(1) & "] = 0"
        Else
            strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT " & Weight & "] = NULL WHERE [RECEIPT " & Weight & "] = 0"
            strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [PUR " & Weight & "] = NULL WHERE [PUR " & Weight & "] = 0"
            strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [ISSUE " & Weight & "] = NULL WHERE [ISSUE " & Weight & "] = 0"
            strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [CLOSING " & Weight & "] = NULL WHERE [CLOSING " & Weight & "] = 0"
        End If
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT STNWT] = NULL WHERE [RECEIPT STNWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [PUR STNWT] = NULL WHERE [PUR STNWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [ISSUE STNWT] = NULL WHERE [ISSUE STNWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [CLOSING STNWT] = NULL WHERE [CLOSING STNWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT DIAWT] = NULL WHERE [RECEIPT DIAWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [PUR DIAWT] = NULL WHERE [PUR DIAWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [ISSUE DIAWT] = NULL WHERE [ISSUE DIAWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [CLOSING DIAWT] = NULL WHERE [CLOSING DIAWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [RECEIPT PREWT] = NULL WHERE [RECEIPT PREWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [PUR PREWT] = NULL WHERE [PUR PREWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [ISSUE PREWT] = NULL WHERE [ISSUE PREWT] = 0"
        strSql += vbCrLf + "  UPDATE TEMP" & systemId & "GSSTOCK SET [CLOSING PREWT] = NULL WHERE [CLOSING PREWT] = 0"

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
            strSql = " SELECT PARTICULAR,TRANNO,TTRANDATE,CONVERT(VARCHAR(50),[RECEIPT PCS])AS [RECEIPT PCS]"
            If rdbBothwt.Checked Then
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[RECEIPT " & bothWeight(0) & "])AS [RECEIPT " & bothWeight(0) & "],CONVERT(VARCHAR(50),[RECEIPT " & bothWeight(1) & "])AS [RECEIPT " & bothWeight(1) & "]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[RECEIPT STNPCS])AS [RECEIPT STNPCS],CONVERT(VARCHAR(50),[RECEIPT DIAPCS])AS [RECEIPT DIAPCS],CONVERT(VARCHAR(50),[RECEIPT PREPCS])AS [RECEIPT PREPCS]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[RECEIPT STNWT])AS [RECEIPT STNWT],CONVERT(VARCHAR(50),[RECEIPT DIAWT])AS [RECEIPT DIAWT],CONVERT(VARCHAR(50),[RECEIPT PREWT])AS [RECEIPT PREWT]"

                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[PUR " & bothWeight(0) & "])AS [PUR " & bothWeight(0) & "],CONVERT(VARCHAR(50),[PUR " & bothWeight(1) & "])AS [PUR " & bothWeight(1) & "]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[PUR STNPCS])AS [PUR STNPCS],CONVERT(VARCHAR(50),[PUR DIAPCS])AS [PUR DIAPCS],CONVERT(VARCHAR(50),[PUR PREPCS])AS [PUR PREPCS]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[PUR STNWT])AS [PUR STNWT],CONVERT(VARCHAR(50),[PUR DIAWT])AS [PUR DIAWT],CONVERT(VARCHAR(50),[PUR PREWT])AS [PUR PREWT]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[ISSUE PCS])AS [ISSUE PCS],CONVERT(VARCHAR(50),[ISSUE " & bothWeight(0) & "])AS [ISSUE " & bothWeight(0) & "],CONVERT(VARCHAR(50),[ISSUE " & bothWeight(1) & "]) AS [ISSUE " & bothWeight(1) & "]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[ISSUE STNPCS])AS [ISSUE STNPCS],CONVERT(VARCHAR(50),[ISSUE DIAPCS])AS [ISSUE DIAPCS],CONVERT(VARCHAR(50),[ISSUE PREPCS])AS [ISSUE PREPCS]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[ISSUE STNWT])AS [ISSUE STNWT],CONVERT(VARCHAR(50),[ISSUE DIAWT])AS [ISSUE DIAWT],CONVERT(VARCHAR(50),[ISSUE PREWT])AS [ISSUE PREWT]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[CLOSING PCS])AS [CLOSING PCS],[CLOSING " & bothWeight(0) & "],[CLOSING " & bothWeight(1) & "]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[CLOSING STNPCS])AS [CLOSING STNPCS],CONVERT(VARCHAR(50),[CLOSING DIAPCS])AS [CLOSING DIAPCS],CONVERT(VARCHAR(50),[CLOSING PREPCS])AS [ISSUE PREPCS]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[CLOSING STNWT])AS [CLOSING STNWT],CONVERT(VARCHAR(50),[CLOSING DIAWT])AS [CLOSING DIAWT],CONVERT(VARCHAR(50),[CLOSING PREWT])AS [ISSUE PREWT]"
            Else
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[PUR " & Weight & "])AS [PUR " & Weight & "]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[PUR STNPCS])AS [PUR STNPCS],CONVERT(VARCHAR(50),[PUR DIAPCS])AS [PUR DIAPCS],CONVERT(VARCHAR(50),[PUR PREPCS])AS [PUR PREPCS]"
                strSql += vbCrLf + " ,CONVERT(VARCHAR(50),[PUR STNWT])AS [PUR STNWT],CONVERT(VARCHAR(50),[PUR DIAWT])AS [PUR DIAWT],CONVERT(VARCHAR(50),[PUR PREWT])AS [PUR PREWT]"

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
            strSql += vbCrLf + " FROM TEMP" & systemId & "GSSTOCK "
            strSql += vbCrLf + "  ORDER BY METAL,GS,CATNAME,TRANDATE,RESULT"
        ElseIf chkCategorywise.Checked = False Then
            strSql = " SELECT * FROM TEMP" & systemId & "GSSTOCK "
            strSql += vbCrLf + "  ORDER BY METAL,GS,CATNAME,ITEMNAME,TRANDATE,RESULT"
        End If

        If chkOrderbyTranno.Checked And chkOrderbyTranno.Visible Then
            strSql += vbCrLf + " ,TRANNO"
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
            RunStnPcs += Val(Ro.Item("RECEIPT STNPCS").ToString) + Val(Ro.Item("PUR STNPCS").ToString) - Val(Ro.Item("ISSUE STNPCS").ToString)
            RunDiaPcs += Val(Ro.Item("RECEIPT DIAPCS").ToString) + Val(Ro.Item("PUR DIAPCS").ToString) - Val(Ro.Item("ISSUE DIAPCS").ToString)
            RunPrePcs += Val(Ro.Item("RECEIPT PREPCS").ToString) + Val(Ro.Item("PUR PREPCS").ToString) - Val(Ro.Item("ISSUE PREPCS").ToString)
            If rdbBothwt.Checked Then
                RunWt += Val(Ro.Item("RECEIPT " & bothWeight(0) & "").ToString) + Val(Ro.Item("PUR " & bothWeight(0) & "").ToString) - Val(Ro.Item("ISSUE " & bothWeight(0) & "").ToString)

                RunWt1 += Val(Ro.Item("RECEIPT " & bothWeight(1) & "").ToString) + Val(Ro.Item("PUR " & bothWeight(1) & "").ToString) - Val(Ro.Item("ISSUE " & bothWeight(1) & "").ToString)

            Else
                RunWt += Val(Ro.Item("RECEIPT " & Weight & "").ToString) + Val(Ro.Item("PUR " & Weight & "").ToString) - Val(Ro.Item("ISSUE " & Weight & "").ToString)

            End If
            RunStnWt += Val(Ro.Item("RECEIPT STNWT").ToString) + Val(Ro.Item("PUR STNWT").ToString) - Val(Ro.Item("ISSUE STNWT").ToString)
            RunDiaWt += Val(Ro.Item("RECEIPT DIAWT").ToString) + Val(Ro.Item("PUR DIAWT").ToString) - Val(Ro.Item("ISSUE DIAWT").ToString)
            RunPreWt += Val(Ro.Item("RECEIPT PREWT").ToString) + Val(Ro.Item("PUR PREWT").ToString) - Val(Ro.Item("ISSUE PREWT").ToString)
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

                    Ro.Item("PUR " & bothWeight(0) & "") = DBNull.Value
                    Ro.Item("PUR " & bothWeight(1) & "") = DBNull.Value
                    Ro.Item("PUR STNWT") = DBNull.Value
                    Ro.Item("PUR DIAWT") = DBNull.Value
                    Ro.Item("PUR PREWT") = DBNull.Value
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
                    Ro.Item("PUR " & Weight & "") = DBNull.Value
                    Ro.Item("PUR STNWT") = DBNull.Value
                    Ro.Item("PUR DIAWT") = DBNull.Value
                    Ro.Item("PUR PREWT") = DBNull.Value
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
        dtGrid.Columns("PARTICULAR").SetOrdinal(2)

        GridView.DataSource = dtGrid
        GridView.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        GridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Dim TitleName As String = ""
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
        lblTitleHead.Text = TitleName.Trim
        TitleName = ""
        TitleName = vbCrLf + "THE KERALA VALUE ADDED TAX RULES 2005 "
        TitleName += vbCrLf + "FORM NO 14A [SEE SUB RULES 58(13)]"
        TitleName += vbCrLf + "DAILY STOCK ACCOUNT OF ORNAMENTS"
        If chkMetalSelectAll.Checked = False Then
            TitleName += vbCrLf + "Name Of Ornaments :" + chkMetal.ToString + " JEWELLERY"
        End If
        lblTitle.Text = TitleName.Trim

        GridViewHead.Visible = True

        DataGridView_Trandate(GridView)
        FillGridGroupStyle_KeyNoWise(GridView)
        GridViewHeaderCreator(GridViewHead, GridView)

        TabControl1.SelectedTab = TabPage2
        dtpFrom.Select()
        If chkCategorywise.Checked = True Then
            For i As Integer = 0 To GridView.Rows.Count - 1
                If GridView.Rows(i).Cells("PARTICULAR").Value.ToString <> "" Then
                    If GridView.Rows(i).Cells("PARTICULAR").Value = "   OPENING.." Then
                        If rdbBothwt.Checked Then
                            GridView.Rows(i).Cells("RECEIPT " & bothWeight(0)).Value = String.Empty
                            GridView.Rows(i).Cells("PURCHASE " & bothWeight(0)).Value = String.Empty
                            GridView.Rows(i).Cells("ISSUE " & bothWeight(0)).Value = String.Empty
                            GridView.Rows(i).Cells("RECEIPT " & bothWeight(1)).Value = String.Empty
                            GridView.Rows(i).Cells("PURCHASE " & bothWeight(1)).Value = String.Empty
                            GridView.Rows(i).Cells("ISSUE " & bothWeight(1)).Value = String.Empty
                            GridView.Rows(i).Cells("RECEIPT PCS").Value = String.Empty
                            GridView.Rows(i).Cells("PUR PCS").Value = String.Empty
                            GridView.Rows(i).Cells("ISSUE PCS").Value = String.Empty
                        Else
                            GridView.Rows(i).Cells("RECEIPT " & Weight).Value = String.Empty
                            GridView.Rows(i).Cells("PURCHASE " & Weight).Value = String.Empty
                            GridView.Rows(i).Cells("ISSUE " & Weight).Value = String.Empty
                            GridView.Rows(i).Cells("RECEIPT PCS").Value = String.Empty
                            GridView.Rows(i).Cells("PUR PCS").Value = String.Empty
                            GridView.Rows(i).Cells("ISSUE PCS").Value = String.Empty
                        End If
                    End If
                End If
                Dim cloPcs, cloGrswt, cloNetwt, cloWt As Decimal
                If GridView.Rows(i).Cells("COLHEAD").Value.ToString <> "" Then
                    If GridView.Rows(i).Cells("COLHEAD").Value = "S1" Then
                        If rbtTranno.Checked Then
                            If GridView.Rows(i - 2).Cells("CLOSING PCS").Value.ToString <> "" Then
                                cloPcs = GridView.Rows(i - 2).Cells("CLOSING PCS").Value
                                GridView.Rows(i).Cells("CLOSING PCS").Value = cloPcs
                            End If
                            If rdbBothwt.Checked Then
                                If GridView.Rows(i - 2).Cells("CLOSING " & bothWeight(0)).Value.ToString <> "" Then
                                    cloGrswt = GridView.Rows(i - 2).Cells("CLOSING " & bothWeight(0)).Value
                                    GridView.Rows(i).Cells("CLOSING " & bothWeight(0)).Value = cloGrswt
                                End If
                                If GridView.Rows(i - 2).Cells("CLOSING " & bothWeight(1)).Value.ToString <> "" Then
                                    cloNetwt = GridView.Rows(i - 2).Cells("CLOSING " & bothWeight(1)).Value
                                    GridView.Rows(i).Cells("CLOSING " & bothWeight(1)).Value = cloNetwt
                                End If
                            Else
                                If GridView.Rows(i - 2).Cells("CLOSING " & Weight).Value.ToString <> "" Then
                                    cloWt = GridView.Rows(i - 2).Cells("CLOSING " & Weight).Value
                                    GridView.Rows(i).Cells("CLOSING " & Weight).Value = cloWt
                                End If
                            End If
                        ElseIf rbtTrandate.Checked Then
                            If GridView.Rows(i - 1).Cells("CLOSING PCS").Value.ToString <> "" Then
                                cloPcs = GridView.Rows(i - 1).Cells("CLOSING PCS").Value
                                GridView.Rows(i).Cells("CLOSING PCS").Value = cloPcs
                            End If
                            If rdbBothwt.Checked Then
                                If GridView.Rows(i - 1).Cells("CLOSING " & bothWeight(0)).Value.ToString <> "" Then
                                    cloGrswt = GridView.Rows(i - 1).Cells("CLOSING " & bothWeight(0)).Value
                                    GridView.Rows(i).Cells("CLOSING " & bothWeight(0)).Value = cloGrswt
                                End If
                                If GridView.Rows(i - 1).Cells("CLOSING " & bothWeight(1)).Value.ToString <> "" Then
                                    cloNetwt = GridView.Rows(i - 1).Cells("CLOSING " & bothWeight(1)).Value
                                    GridView.Rows(i).Cells("CLOSING " & bothWeight(1)).Value = cloNetwt
                                End If
                            Else
                                If GridView.Rows(i - 1).Cells("CLOSING " & Weight).Value.ToString <> "" Then
                                    cloWt = GridView.Rows(i - 1).Cells("CLOSING " & Weight).Value
                                    GridView.Rows(i).Cells("CLOSING " & Weight).Value = cloWt
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
        If rdbBothwt.Checked Then
            Weight = "GRSWT,NETWT"
        End If
        If rdbBothwt.Checked Then
            bothWeight = Weight.Split(",")
        End If

        Dim dtHead As New DataTable
        strSql = "SELECT ''[TTRANDATE],''[TRANNO],''[PARTICULAR]"
        If rdbBothwt.Checked Then
            strSql += ",''[RECEIPT PCS~RECEIPT " & bothWeight(0) & "~RECEIPT " & bothWeight(1) & "~RECEIPT STNPCS~RECEIPT DIAPCS~RECEIPT PREPCS~RECEIPT STNWT~RECEIPT DIAWT~RECEIPT PREWT]"
            strSql += ",''[PUR PCS~PUR " & bothWeight(0) & "~PUR " & bothWeight(1) & "~PUR STNPCS~PUR DIAPCS~PUR PREPCS~PUR STNWT~PUR DIAWT~PUR PREWT]"
            strSql += ",''[T]"
            strSql += ",''[ISSUE PCS~ISSUE " & bothWeight(0) & "~ISSUE " & bothWeight(1) & "~ISSUE STNPCS~ISSUE DIAPCS~ISSUE PREPCS~ISSUE STNWT~ISSUE DIAWT~ISSUE PREWT]"
            strSql += ",''[CLOSING PCS~CLOSING " & bothWeight(0) & "~CLOSING " & bothWeight(1) & "~CLOSING STNPCS~CLOSING DIAPCS~CLOSING PREPCS~CLOSING STNWT~CLOSING DIAWT~CLOSING PREWT]"
        Else
            strSql += ",''[RECEIPT PCS~RECEIPT " & Weight & "~RECEIPT STNPCS~RECEIPT DIAPCS~RECEIPT PREPCS~RECEIPT STNWT~RECEIPT DIAWT~RECEIPT PREWT]"
            strSql += ",''[PUR PCS~PUR " & Weight & "~PUR STNPCS~PUR DIAPCS~PUR PREPCS~PUR STNWT~PUR DIAWT~PUR PREWT]"
            strSql += ",''[T]"
            strSql += ",''[ISSUE PCS~ISSUE " & Weight & "~ISSUE STNPCS~ISSUE DIAPCS~ISSUE PREPCS~ISSUE STNWT~ISSUE DIAWT~ISSUE PREWT]"
            strSql += ",''[CLOSING PCS~CLOSING " & Weight & "~CLOSING STNPCS~CLOSING DIAPCS~CLOSING PREPCS~CLOSING STNWT~CLOSING DIAWT~CLOSING PREWT]"
        End If
        strSql += ",''SCROLL"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtHead)
        gridviewHead.DataSource = dtHead
        gridviewHead.Columns("T").Width = 0
        gridviewHead.Columns("TTRANDATE").Width = gridview.Columns("TTRANDATE").Width
        gridviewHead.Columns("TTRANDATE").HeaderText = ""
        gridviewHead.Columns("TTRANDATE").Width = gridview.Columns("TTRANDATE").Width
        gridviewHead.Columns("TTRANDATE").Visible = True
        gridviewHead.Columns("TRANNO").HeaderText = ""
        gridviewHead.Columns("TRANNO").Width = IIf(gridview.Columns("TRANNO").Visible, gridview.Columns("TRANNO").Width, 0)
        gridviewHead.Columns("TRANNO").Visible = gridview.Columns("TRANNO").Visible
        gridviewHead.Columns("PARTICULAR").HeaderText = ""
        gridviewHead.Columns("PARTICULAR").Width = gridview.Columns("PARTICULAR").Width
        If rdbBothwt.Checked Then
            gridviewHead.Columns("RECEIPT PCS~RECEIPT " & bothWeight(0) & "~RECEIPT " & bothWeight(1) & "~RECEIPT STNPCS~RECEIPT DIAPCS~RECEIPT PREPCS~RECEIPT STNWT~RECEIPT DIAWT~RECEIPT PREWT").HeaderText = "RECEIPT"
            gridviewHead.Columns("RECEIPT PCS~RECEIPT " & bothWeight(0) & "~RECEIPT " & bothWeight(1) & "~RECEIPT STNPCS~RECEIPT DIAPCS~RECEIPT PREPCS~RECEIPT STNWT~RECEIPT DIAWT~RECEIPT PREWT").Width = _
            IIf(gridview.Columns("RECEIPT PCS").Visible, gridview.Columns("RECEIPT PCS").Width, 0) _
            + gridview.Columns("RECEIPT " & bothWeight(0) & "").Width + gridview.Columns("RECEIPT " & bothWeight(1) & "").Width _
            + IIf(gridview.Columns("RECEIPT STNPCS").Visible, gridview.Columns("RECEIPT STNPCS").Width, 0) + IIf(gridview.Columns("RECEIPT STNWT").Visible, gridview.Columns("RECEIPT STNWT").Width, 0) _
            + IIf(gridview.Columns("RECEIPT DIAPCS").Visible, gridview.Columns("RECEIPT DIAPCS").Width, 0) + IIf(gridview.Columns("RECEIPT DIAWT").Visible, gridview.Columns("RECEIPT DIAWT").Width, 0) _
            + IIf(gridview.Columns("RECEIPT PREPCS").Visible, gridview.Columns("RECEIPT PREPCS").Width, 0) + IIf(gridview.Columns("RECEIPT PREWT").Visible, gridview.Columns("RECEIPT PREWT").Width, 0)

            gridviewHead.Columns("PUR PCS~PUR " & bothWeight(0) & "~PUR " & bothWeight(1) & "~PUR STNPCS~PUR DIAPCS~PUR PREPCS~PUR STNWT~PUR DIAWT~PUR PREWT").HeaderText = "PURCHASE"
            gridviewHead.Columns("PUR PCS~PUR " & bothWeight(0) & "~PUR " & bothWeight(1) & "~PUR STNPCS~PUR DIAPCS~PUR PREPCS~PUR STNWT~PUR DIAWT~PUR PREWT").Width = _
            IIf(gridview.Columns("PUR PCS").Visible, gridview.Columns("PUR PCS").Width, 0) _
            + gridview.Columns("PUR " & bothWeight(0) & "").Width + gridview.Columns("PUR " & bothWeight(1) & "").Width _
            + IIf(gridview.Columns("PUR STNPCS").Visible, gridview.Columns("PUR STNPCS").Width, 0) + IIf(gridview.Columns("PUR STNWT").Visible, gridview.Columns("PUR STNWT").Width, 0) _
            + IIf(gridview.Columns("PUR DIAPCS").Visible, gridview.Columns("PUR DIAPCS").Width, 0) + IIf(gridview.Columns("PUR DIAWT").Visible, gridview.Columns("PUR DIAWT").Width, 0) _
            + IIf(gridview.Columns("PUR PREPCS").Visible, gridview.Columns("PUR PREPCS").Width, 0) + IIf(gridview.Columns("PUR PREWT").Visible, gridview.Columns("PUR PREWT").Width, 0)

            gridviewHead.Columns("ISSUE PCS~ISSUE " & bothWeight(0) & "~ISSUE " & bothWeight(1) & "~ISSUE STNPCS~ISSUE DIAPCS~ISSUE PREPCS~ISSUE STNWT~ISSUE DIAWT~ISSUE PREWT").HeaderText = "ISSUE"
            gridviewHead.Columns("ISSUE PCS~ISSUE " & bothWeight(0) & "~ISSUE " & bothWeight(1) & "~ISSUE STNPCS~ISSUE DIAPCS~ISSUE PREPCS~ISSUE STNWT~ISSUE DIAWT~ISSUE PREWT").Width = _
            IIf(gridview.Columns("ISSUE PCS").Visible, gridview.Columns("ISSUE PCS").Width, 0) _
            + gridview.Columns("ISSUE " & bothWeight(0) & "").Width + gridview.Columns("ISSUE " & bothWeight(1) & "").Width _
            + IIf(gridview.Columns("ISSUE STNPCS").Visible, gridview.Columns("ISSUE STNPCS").Width, 0) + IIf(gridview.Columns("ISSUE STNWT").Visible, gridview.Columns("ISSUE STNWT").Width, 0) _
            + IIf(gridview.Columns("ISSUE DIAPCS").Visible, gridview.Columns("ISSUE DIAPCS").Width, 0) + IIf(gridview.Columns("ISSUE DIAWT").Visible, gridview.Columns("ISSUE DIAWT").Width, 0) _
            + IIf(gridview.Columns("ISSUE PREPCS").Visible, gridview.Columns("ISSUE PREPCS").Width, 0) + IIf(gridview.Columns("ISSUE PREWT").Visible, gridview.Columns("ISSUE PREWT").Width, 0)

            gridviewHead.Columns("CLOSING PCS~CLOSING " & bothWeight(0) & "~CLOSING " & bothWeight(1) & "~CLOSING STNPCS~CLOSING DIAPCS~CLOSING PREPCS~CLOSING STNWT~CLOSING DIAWT~CLOSING PREWT").HeaderText = "CLOSING"
            gridviewHead.Columns("CLOSING PCS~CLOSING " & bothWeight(0) & "~CLOSING " & bothWeight(1) & "~CLOSING STNPCS~CLOSING DIAPCS~CLOSING PREPCS~CLOSING STNWT~CLOSING DIAWT~CLOSING PREWT").Width = _
            IIf(gridview.Columns("CLOSING PCS").Visible, gridview.Columns("CLOSING PCS").Width, 0) _
            + gridview.Columns("CLOSING " & bothWeight(0) & "").Width + gridview.Columns("CLOSING " & bothWeight(1) & "").Width _
            + IIf(gridview.Columns("CLOSING STNPCS").Visible, gridview.Columns("CLOSING STNPCS").Width, 0) + IIf(gridview.Columns("CLOSING STNWT").Visible, gridview.Columns("CLOSING STNWT").Width, 0) _
            + IIf(gridview.Columns("CLOSING DIAPCS").Visible, gridview.Columns("CLOSING DIAPCS").Width, 0) + IIf(gridview.Columns("CLOSING DIAWT").Visible, gridview.Columns("CLOSING DIAWT").Width, 0) _
            + IIf(gridview.Columns("CLOSING PREPCS").Visible, gridview.Columns("CLOSING PREPCS").Width, 0) + IIf(gridview.Columns("CLOSING PREWT").Visible, gridview.Columns("CLOSING PREWT").Width, 0)
        Else
            gridviewHead.Columns("RECEIPT PCS~RECEIPT " & Weight & "~RECEIPT STNPCS~RECEIPT DIAPCS~RECEIPT PREPCS~RECEIPT STNWT~RECEIPT DIAWT~RECEIPT PREWT").HeaderText = "RECEIPT"
            gridviewHead.Columns("RECEIPT PCS~RECEIPT " & Weight & "~RECEIPT STNPCS~RECEIPT DIAPCS~RECEIPT PREPCS~RECEIPT STNWT~RECEIPT DIAWT~RECEIPT PREWT").Width = _
            IIf(gridview.Columns("RECEIPT PCS").Visible, gridview.Columns("RECEIPT PCS").Width, 0) + gridview.Columns("RECEIPT " & Weight & "").Width _
            + IIf(gridview.Columns("RECEIPT STNPCS").Visible, gridview.Columns("RECEIPT STNPCS").Width, 0) + IIf(gridview.Columns("RECEIPT STNWT").Visible, gridview.Columns("RECEIPT STNWT").Width, 0) _
            + IIf(gridview.Columns("RECEIPT DIAPCS").Visible, gridview.Columns("RECEIPT DIAPCS").Width, 0) + IIf(gridview.Columns("RECEIPT DIAWT").Visible, gridview.Columns("RECEIPT DIAWT").Width, 0) _
            + IIf(gridview.Columns("RECEIPT PREPCS").Visible, gridview.Columns("RECEIPT PREPCS").Width, 0) + IIf(gridview.Columns("RECEIPT PREWT").Visible, gridview.Columns("RECEIPT PREWT").Width, 0)

            gridviewHead.Columns("PUR PCS~PUR " & Weight & "~PUR STNPCS~PUR DIAPCS~PUR PREPCS~PUR STNWT~PUR DIAWT~PUR PREWT").HeaderText = "PURCHASE"
            gridviewHead.Columns("PUR PCS~PUR " & Weight & "~PUR STNPCS~PUR DIAPCS~PUR PREPCS~PUR STNWT~PUR DIAWT~PUR PREWT").Width = _
            IIf(gridview.Columns("PUR PCS").Visible, gridview.Columns("PUR PCS").Width, 0) + gridview.Columns("PUR " & Weight & "").Width _
            + IIf(gridview.Columns("PUR STNPCS").Visible, gridview.Columns("PUR STNPCS").Width, 0) + IIf(gridview.Columns("PUR STNWT").Visible, gridview.Columns("PUR STNWT").Width, 0) _
            + IIf(gridview.Columns("PUR DIAPCS").Visible, gridview.Columns("PUR DIAPCS").Width, 0) + IIf(gridview.Columns("PUR DIAWT").Visible, gridview.Columns("PUR DIAWT").Width, 0) _
            + IIf(gridview.Columns("PUR PREPCS").Visible, gridview.Columns("PUR PREPCS").Width, 0) + IIf(gridview.Columns("PUR PREWT").Visible, gridview.Columns("PUR PREWT").Width, 0)

            gridviewHead.Columns("ISSUE PCS~ISSUE " & Weight & "~ISSUE STNPCS~ISSUE DIAPCS~ISSUE PREPCS~ISSUE STNWT~ISSUE DIAWT~ISSUE PREWT").HeaderText = "ISSUE"
            gridviewHead.Columns("ISSUE PCS~ISSUE " & Weight & "~ISSUE STNPCS~ISSUE DIAPCS~ISSUE PREPCS~ISSUE STNWT~ISSUE DIAWT~ISSUE PREWT").Width = _
            IIf(gridview.Columns("ISSUE PCS").Visible, gridview.Columns("ISSUE PCS").Width, 0) + gridview.Columns("ISSUE " & Weight & "").Width _
            + IIf(gridview.Columns("ISSUE STNPCS").Visible, gridview.Columns("ISSUE STNPCS").Width, 0) + IIf(gridview.Columns("ISSUE STNWT").Visible, gridview.Columns("ISSUE STNWT").Width, 0) _
            + IIf(gridview.Columns("ISSUE DIAPCS").Visible, gridview.Columns("ISSUE DIAPCS").Width, 0) + IIf(gridview.Columns("ISSUE DIAWT").Visible, gridview.Columns("ISSUE DIAWT").Width, 0) _
            + IIf(gridview.Columns("ISSUE PREPCS").Visible, gridview.Columns("ISSUE PREPCS").Width, 0) + IIf(gridview.Columns("ISSUE PREWT").Visible, gridview.Columns("ISSUE PREWT").Width, 0)

            gridviewHead.Columns("CLOSING PCS~CLOSING " & Weight & "~CLOSING STNPCS~CLOSING DIAPCS~CLOSING PREPCS~CLOSING STNWT~CLOSING DIAWT~CLOSING PREWT").HeaderText = "CLOSING"
            gridviewHead.Columns("CLOSING PCS~CLOSING " & Weight & "~CLOSING STNPCS~CLOSING DIAPCS~CLOSING PREPCS~CLOSING STNWT~CLOSING DIAWT~CLOSING PREWT").Width = _
            IIf(gridview.Columns("CLOSING PCS").Visible, gridview.Columns("CLOSING PCS").Width, 0) + gridview.Columns("CLOSING " & Weight & "").Width _
            + IIf(gridview.Columns("CLOSING STNPCS").Visible, gridview.Columns("CLOSING STNPCS").Width, 0) + IIf(gridview.Columns("CLOSING STNWT").Visible, gridview.Columns("CLOSING STNWT").Width, 0) _
            + IIf(gridview.Columns("CLOSING DIAPCS").Visible, gridview.Columns("CLOSING DIAPCS").Width, 0) + IIf(gridview.Columns("CLOSING DIAWT").Visible, gridview.Columns("CLOSING DIAWT").Width, 0) _
            + IIf(gridview.Columns("CLOSING PREPCS").Visible, gridview.Columns("CLOSING PREPCS").Width, 0) + IIf(gridview.Columns("CLOSING PREWT").Visible, gridview.Columns("CLOSING PREWT").Width, 0)
        End If

        With gridviewHead
            gridview.Focus()
            Dim colWid As Integer = 0
            For cnt As Integer = 0 To gridview.ColumnCount - 1
                If gridview.Columns(cnt).Visible Then colWid += gridview.Columns(cnt).Width
            Next
            If colWid >= gridview.Width Then
                .Columns("SCROLL").Visible = CType(gridview.Controls(0), HScrollBar).Visible
                .Columns("SCROLL").Width = CType(gridview.Controls(1), VScrollBar).Width
            Else
                .Columns("SCROLL").Visible = False
            End If
        End With

        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        gridviewHead.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        'SetGridHeadColWidth(gridviewHead)
    End Sub


    Private Sub frmMetalOrnamentDetailedStockView_New_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TabControl1.ItemSize = New System.Drawing.Size(1, 1)
        Me.TabControl1.Region = New Region(New RectangleF(Me.TabPage1.Left, Me.TabPage1.Top, Me.TabPage1.Width, Me.TabPage1.Height))

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
            FillCheckedListBox(strSql, chkLstCostCentre)
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
        btnNew_Click(Me, New EventArgs)
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        rdbBothwt.Checked = True
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        ' chkGs11.Checked = True
        ' chkGs12.Checked = True
        ' chkOthers.Checked = True
        'rbtTrandate.Checked = True
        ' cmbCategoryGroup.Text = "ALL"
        dtpFrom.Select()
        Prop_Gets()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub frmMetalOrnamentDetailedStockView_New_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        ElseIf e.KeyChar = Chr(Keys.Escape) Then
            If TabControl1.SelectedTab.Name = TabPage2.Name Then
                TabControl1.SelectedTab = TabPage1
                btnSearch.Enabled = True
            End If
            dtpFrom.Focus()
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
                strSql += " AND ISNULL(GS11,'') = 'Y'"
                strSql += StrCatGroup
            End If
            If chkGs12.Checked Then
                If strSql <> "" Then strSql += " UNION "
                strSql += " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
                strSql += " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalNames & "))"
                strSql += " AND ISNULL(GS12,'') = 'Y'"
                strSql += StrCatGroup
            End If
            If chkOthers.Checked Then
                If strSql <> "" Then strSql += " UNION "
                strSql += " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
                strSql += " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalNames & "))"
                strSql += " AND ISNULL(GS11,'') = '' AND ISNULL(GS12,'') = ''"
                strSql += StrCatGroup
            End If
            If rbtPcs.Checked Then
                strSql = " SELECT CATNAME FROM " & cnAdminDb & "..CATEGORY "
                strSql += " WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalNames & "))"
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
        Weight = IIf(rbtGrsWeight.Checked, "Grswt", "Netwt")
        If rdbBothwt.Checked Then
            Weight = "Grswt,Netwt"
        End If
        If rdbBothwt.Checked Then
            bothWeight = Weight.Split(",")
        End If
        With dgv
            For cnt As Integer = 1 To dgv.ColumnCount - 1
                dgv.Columns(cnt).Visible = False
            Next
            .Columns("TTRANDATE").Visible = True
            .Columns("TRANNO").Visible = rbtTranno.Checked
            .Columns("PARTICULAR").Visible = True
            .Columns("RECEIPT PCS").Visible = rbtPcs.Checked Or rbtBoth.Checked
            .Columns("PUR PCS").Visible = rbtPcs.Checked Or rbtBoth.Checked
            .Columns("ISSUE PCS").Visible = rbtPcs.Checked Or rbtBoth.Checked
            .Columns("CLOSING PCS").Visible = rbtPcs.Checked Or rbtBoth.Checked

            .Columns("RECEIPT STNPCS").Visible = (rbtPcs.Checked Or rbtBoth.Checked) And ChkStone.Checked
            .Columns("RECEIPT DIAPCS").Visible = (rbtPcs.Checked Or rbtBoth.Checked) And ChkDia.Checked
            .Columns("RECEIPT PREPCS").Visible = (rbtPcs.Checked Or rbtBoth.Checked) And ChkPresious.Checked
            .Columns("PUR STNPCS").Visible = (rbtPcs.Checked Or rbtBoth.Checked) And ChkStone.Checked
            .Columns("PUR DIAPCS").Visible = (rbtPcs.Checked Or rbtBoth.Checked) And ChkDia.Checked
            .Columns("PUR PREPCS").Visible = (rbtPcs.Checked Or rbtBoth.Checked) And ChkPresious.Checked

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

                .Columns("PUR " & bothWeight(0) & "").Visible = rbtWeight.Checked Or rbtBoth.Checked
                .Columns("PUR " & bothWeight(1) & "").Visible = rbtWeight.Checked Or rbtBoth.Checked
                .Columns("PUR STNWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkStone.Checked
                .Columns("PUR DIAWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkDia.Checked
                .Columns("PUR PREWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkPresious.Checked

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

                .Columns("PUR " & Weight & "").Visible = rbtWeight.Checked Or rbtBoth.Checked
                .Columns("PUR STNWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkStone.Checked
                .Columns("PUR DIAWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkDia.Checked
                .Columns("PUR PREWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkPresious.Checked

                .Columns("ISSUE " & Weight & "").Visible = rbtWeight.Checked Or rbtBoth.Checked
                .Columns("ISSUE STNWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkStone.Checked
                .Columns("ISSUE DIAWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkDia.Checked
                .Columns("ISSUE PREWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkPresious.Checked
                .Columns("CLOSING " & Weight & "").Visible = rbtWeight.Checked Or rbtBoth.Checked
                .Columns("CLOSING STNWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkStone.Checked
                .Columns("CLOSING DIAWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkDia.Checked
                .Columns("CLOSING PREWT").Visible = (rbtWeight.Checked Or rbtBoth.Checked) And ChkPresious.Checked
            End If

            .Columns("PARTICULAR").Width = 300
            .Columns("PARTICULAR").HeaderText = "Particular"
            .Columns("TRANNO").Width = 70
            .Columns("TRANNO").HeaderText = "Tranno"
            .Columns("TTRANDATE").Width = 90
            .Columns("TTRANDATE").HeaderText = "Trandate"
            .Columns("RECEIPT PCS").Width = 70
            .Columns("ISSUE PCS").Width = 70
            .Columns("CLOSING PCS").Width = 70
            If rdbBothwt.Checked Then
                .Columns("RECEIPT " & bothWeight(0) & "").Width = 100
                .Columns("RECEIPT " & bothWeight(1) & "").Width = 100
                .Columns("PUR " & bothWeight(0) & "").Width = 100
                .Columns("PUR " & bothWeight(1) & "").Width = 100
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
            .Columns("RECEIPT PCS").HeaderText = "Pcs"
            .Columns("PUR PCS").HeaderText = "Pcs"
            .Columns("ISSUE PCS").HeaderText = "Pcs"
            .Columns("CLOSING PCS").HeaderText = "Pcs"
            .Columns("RECEIPT STNPCS").HeaderText = "StnPcs"
            .Columns("RECEIPT DIAPCS").HeaderText = "DiaPcs"
            .Columns("RECEIPT PREPCS").HeaderText = "PrePcs"
            .Columns("PUR STNPCS").HeaderText = "StnPcs"
            .Columns("PUR DIAPCS").HeaderText = "DiaPcs"
            .Columns("PUR PREPCS").HeaderText = "PrePcs"
            .Columns("ISSUE PCS").HeaderText = "Pcs"
            .Columns("ISSUE STNPCS").HeaderText = "StnPcs"
            .Columns("ISSUE DIAPCS").HeaderText = "DiaPcs"
            .Columns("ISSUE PREPCS").HeaderText = "PrePcs"
            .Columns("CLOSING PCS").HeaderText = "Pcs"
            .Columns("CLOSING STNPCS").HeaderText = "StnPcs"
            .Columns("CLOSING DIAPCS").HeaderText = "DiaPcs"
            .Columns("CLOSING PREPCS").HeaderText = "PrePcs"

            If rdbBothwt.Checked Then
                '.Columns("RECEIPT " & bothWeight(0) & "").HeaderText = bothWeight(0)
                '.Columns("RECEIPT " & bothWeight(1) & "").HeaderText = bothWeight(1)
                '.Columns("PUR " & bothWeight(0) & "").HeaderText = bothWeight(0)
                '.Columns("PUR " & bothWeight(1) & "").HeaderText = bothWeight(1)
                '.Columns("ISSUE " & bothWeight(0) & "").HeaderText = bothWeight(0)
                '.Columns("ISSUE " & bothWeight(1) & "").HeaderText = bothWeight(1)
                '.Columns("CLOSING " & bothWeight(0) & "").HeaderText = bothWeight(0)
                '.Columns("CLOSING " & bothWeight(1) & "").HeaderText = bothWeight(1)
                .Columns("RECEIPT " & bothWeight(0) & "").HeaderText = bothWeight(0) & " Of Ornaments"
                .Columns("RECEIPT " & bothWeight(1) & "").HeaderText = bothWeight(1) & " excluding Stones,Gems & Pearls"
                .Columns("PUR " & bothWeight(0) & "").HeaderText = bothWeight(0) & " Of Ornaments"
                .Columns("PUR " & bothWeight(1) & "").HeaderText = bothWeight(1) & " excluding Stones,Gems & Pearls"
                .Columns("ISSUE " & bothWeight(0) & "").HeaderText = bothWeight(0) & " Of Ornaments"
                .Columns("ISSUE " & bothWeight(1) & "").HeaderText = bothWeight(1) & " excluding Stones,Gems & Pearls"
                .Columns("CLOSING " & bothWeight(0) & "").HeaderText = bothWeight(0) & " Of Ornaments"
                .Columns("CLOSING " & bothWeight(1) & "").HeaderText = bothWeight(1) & " excluding Stones,Gems & Pearls"

            Else
                .Columns("RECEIPT " & Weight & "").HeaderText = Weight & " Of Ornaments"
                .Columns("PUR " & Weight & "").HeaderText = Weight & " Of Ornaments"
                .Columns("ISSUE " & Weight & "").HeaderText = Weight & " Of Ornaments"
                .Columns("CLOSING " & Weight & "").HeaderText = Weight & " Of Ornaments"
            End If

            .Columns("RECEIPT STNWT").HeaderText = "StnWt"
            .Columns("RECEIPT DIAWT").HeaderText = "DiaWt"
            .Columns("RECEIPT PREWT").HeaderText = "PreWt"
            .Columns("PUR STNWT").HeaderText = "StnWt"
            .Columns("PUR DIAWT").HeaderText = "DiaWt"
            .Columns("PUR PREWT").HeaderText = "PreWt"
            .Columns("ISSUE STNWT").HeaderText = "StnWt"
            .Columns("ISSUE DIAWT").HeaderText = "DiaWt"
            .Columns("ISSUE PREWT").HeaderText = "PreWt"
            .Columns("CLOSING STNWT").HeaderText = "StnWt"
            .Columns("CLOSING DIAWT").HeaderText = "DiaWt"
            .Columns("CLOSING PREWT").HeaderText = "PreWt"
            .Columns("T").Width = 1


            .Columns("RECEIPT PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("PUR PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("ISSUE PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CLOSING PCS").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            If rdbBothwt.Checked Then
                .Columns("RECEIPT " & bothWeight(0) & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("RECEIPT " & bothWeight(1) & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("PUR " & bothWeight(0) & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("PUR " & bothWeight(1) & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("ISSUE " & bothWeight(0) & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("ISSUE " & bothWeight(1) & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CLOSING " & bothWeight(0) & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CLOSING " & bothWeight(1) & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            Else
                .Columns("RECEIPT " & Weight & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("PUR " & Weight & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("ISSUE " & Weight & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns("CLOSING " & Weight & "").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            End If

            FormatGridColumns(dgv, False, False, , False)
            FillGridGroupStyle_KeyNoWise(dgv)
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
        Dim obj As New frmMetalOrnamentDetailedStockView_New_Properties
        obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
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
        SetSettingsObj(obj, Me.Name, GetType(frmMetalOrnamentDetailedStockView_New_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmMetalOrnamentDetailedStockView_New_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmMetalOrnamentDetailedStockView_New_Properties))
        chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
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

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Excel) Then Exit Sub
        If GridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, cnCompanyName, lblTitleHead.Text & vbCrLf & lblTitle.Text, GridView, BrightPosting.GExport.GExportType.Export, GridViewHead)
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        If Not BrighttechPack.Methods.GetRights(_DtUserRights, Me.Name, BrighttechPack.Methods.RightMode.Print) Then Exit Sub
        If GridView.Rows.Count > 0 Then
            BrightPosting.GExport.Post(Me.Name, cnCompanyName, lblTitleHead.Text & vbCrLf & lblTitle.Text, GridView, BrightPosting.GExport.GExportType.Print, GridViewHead)
        End If
    End Sub

    Private Sub BtnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnBack.Click
        If TabControl1.SelectedTab.Name = TabPage2.Name Then
            TabControl1.SelectedTab = TabPage1
            btnSearch.Enabled = True
        End If
    End Sub

    Private Sub ResizeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResizeToolStripMenuItem.Click
        If GridView.RowCount > 0 Then
            If ResizeToolStripMenuItem.Checked Then
                GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.AllCells
                GridView.Invalidate()
                For Each dgvCol As DataGridViewColumn In GridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            Else
                For Each dgvCol As DataGridViewColumn In GridView.Columns
                    dgvCol.Width = dgvCol.Width
                Next
                GridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
            End If
            'gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.None
        End If
    End Sub

    Private Sub GridView_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles GridView.ColumnWidthChanged
        If FormReSize Then
            Dim wid As Double = Nothing
            For cnt As Integer = 0 To GridView.ColumnCount - 1
                If GridView.Columns(cnt).Visible Then
                    wid += GridView.Columns(cnt).Width
                End If
            Next
            wid += 10
            If CType(GridView.Controls(1), VScrollBar).Visible Then
                wid += 20
            End If
            wid += 20
            If wid > ScreenWid Then
                wid = ScreenWid
            End If
        End If
        If FormReLocation Then SetCenterLocation(Me)
        If GridViewHead.ColumnCount > 0 Then
            funcColWidth()
        End If
    End Sub

    Function funcColWidth() As Integer
        Dim bothWeight As String()
        Weight = IIf(rbtGrsWeight.Checked, "GRSWT", "NETWT")
        If rdbBothwt.Checked Then
            Weight = "GRSWT,NETWT"
        End If
        If rdbBothwt.Checked Then
            bothWeight = Weight.Split(",")
        End If
        GridView.Columns("TTRANDATE").Width = 80
        With GridViewHead
            If GridViewHead.DataSource Is Nothing Then Exit Function
            .Columns("SCROLL").HeaderText = ""
            .Columns("PARTICULAR").Width = GridView.Columns("PARTICULAR").Width

            .Columns("TTRANDATE").HeaderText = ""
            .Columns("TTRANDATE").Width = GridView.Columns("TTRANDATE").Width
            .Columns("TTRANDATE").Visible = True
            .Columns("TRANNO").HeaderText = ""
            .Columns("TRANNO").Width = IIf(GridView.Columns("TRANNO").Visible, GridView.Columns("TRANNO").Width, 0)
            .Columns("TRANNO").Visible = GridView.Columns("TRANNO").Visible
            .Columns("PARTICULAR").HeaderText = ""
            .Columns("PARTICULAR").Width = GridView.Columns("PARTICULAR").Width
            If rdbBothwt.Checked Then
                .Columns("RECEIPT PCS~RECEIPT " & bothWeight(0) & "~RECEIPT " & bothWeight(1) & "~RECEIPT STNPCS~RECEIPT DIAPCS~RECEIPT PREPCS~RECEIPT STNWT~RECEIPT DIAWT~RECEIPT PREWT").HeaderText = "RECEIPT"
                .Columns("RECEIPT PCS~RECEIPT " & bothWeight(0) & "~RECEIPT " & bothWeight(1) & "~RECEIPT STNPCS~RECEIPT DIAPCS~RECEIPT PREPCS~RECEIPT STNWT~RECEIPT DIAWT~RECEIPT PREWT").Width = _
                IIf(GridView.Columns("RECEIPT PCS").Visible, GridView.Columns("RECEIPT PCS").Width, 0) _
                + GridView.Columns("RECEIPT " & bothWeight(0) & "").Width + GridView.Columns("RECEIPT " & bothWeight(1) & "").Width _
                + IIf(GridView.Columns("RECEIPT STNPCS").Visible, GridView.Columns("RECEIPT STNPCS").Width, 0) + IIf(GridView.Columns("RECEIPT STNWT").Visible, GridView.Columns("RECEIPT STNWT").Width, 0) _
                + IIf(GridView.Columns("RECEIPT DIAPCS").Visible, GridView.Columns("RECEIPT DIAPCS").Width, 0) + IIf(GridView.Columns("RECEIPT DIAWT").Visible, GridView.Columns("RECEIPT DIAWT").Width, 0) _
                + IIf(GridView.Columns("RECEIPT PREPCS").Visible, GridView.Columns("RECEIPT PREPCS").Width, 0) + IIf(GridView.Columns("RECEIPT PREWT").Visible, GridView.Columns("RECEIPT PREWT").Width, 0)

                .Columns("PUR PCS~PUR " & bothWeight(0) & "~PUR " & bothWeight(1) & "~PUR STNPCS~PUR DIAPCS~PUR PREPCS~PUR STNWT~PUR DIAWT").HeaderText = "PURCHASE"
                .Columns("PUR PCS~PUR " & bothWeight(0) & "~PUR " & bothWeight(1) & "~PUR STNPCS~PUR DIAPCS~PUR PREPCS~PUR STNWT~PUR DIAWT").Width = _
                IIf(GridView.Columns("PUR PCS").Visible, GridView.Columns("PUR PCS").Width, 0) _
                + GridView.Columns("PUR " & bothWeight(0) & "").Width + GridView.Columns("PUR " & bothWeight(1) & "").Width _
                + IIf(GridView.Columns("PUR STNPCS").Visible, GridView.Columns("PUR STNPCS").Width, 0) + IIf(GridView.Columns("PUR STNWT").Visible, GridView.Columns("PUR STNWT").Width, 0) _
                + IIf(GridView.Columns("PUR DIAPCS").Visible, GridView.Columns("PUR DIAPCS").Width, 0) + IIf(GridView.Columns("PUR DIAWT").Visible, GridView.Columns("PUR DIAWT").Width, 0) _
                + IIf(GridView.Columns("PUR PREPCS").Visible, GridView.Columns("PUR PREPCS").Width, 0) + IIf(GridView.Columns("PUR PREWT").Visible, GridView.Columns("PUR PREWT").Width, 0)

                .Columns("ISSUE PCS~ISSUE " & bothWeight(0) & "~ISSUE " & bothWeight(1) & "~ISSUE STNPCS~ISSUE DIAPCS~ISSUE PREPCS~ISSUE STNWT~ISSUE DIAWT~ISSUE PREWT").HeaderText = "ISSUE"
                .Columns("ISSUE PCS~ISSUE " & bothWeight(0) & "~ISSUE " & bothWeight(1) & "~ISSUE STNPCS~ISSUE DIAPCS~ISSUE PREPCS~ISSUE STNWT~ISSUE DIAWT~ISSUE PREWT").Width = _
                IIf(GridView.Columns("ISSUE PCS").Visible, GridView.Columns("ISSUE PCS").Width, 0) _
                + GridView.Columns("ISSUE " & bothWeight(0) & "").Width + GridView.Columns("ISSUE " & bothWeight(1) & "").Width _
                + IIf(GridView.Columns("ISSUE STNPCS").Visible, GridView.Columns("ISSUE STNPCS").Width, 0) + IIf(GridView.Columns("ISSUE STNWT").Visible, GridView.Columns("ISSUE STNWT").Width, 0) _
                + IIf(GridView.Columns("ISSUE DIAPCS").Visible, GridView.Columns("ISSUE DIAPCS").Width, 0) + IIf(GridView.Columns("ISSUE DIAWT").Visible, GridView.Columns("ISSUE DIAWT").Width, 0) _
                + IIf(GridView.Columns("ISSUE PREPCS").Visible, GridView.Columns("ISSUE PREPCS").Width, 0) + IIf(GridView.Columns("ISSUE PREWT").Visible, GridView.Columns("ISSUE PREWT").Width, 0)

                .Columns("CLOSING PCS~CLOSING " & bothWeight(0) & "~CLOSING " & bothWeight(1) & "~CLOSING STNPCS~CLOSING DIAPCS~CLOSING PREPCS~CLOSING STNWT~CLOSING DIAWT~CLOSING PREWT").HeaderText = "CLOSING"
                .Columns("CLOSING PCS~CLOSING " & bothWeight(0) & "~CLOSING " & bothWeight(1) & "~CLOSING STNPCS~CLOSING DIAPCS~CLOSING PREPCS~CLOSING STNWT~CLOSING DIAWT~CLOSING PREWT").Width = _
                IIf(GridView.Columns("CLOSING PCS").Visible, GridView.Columns("CLOSING PCS").Width, 0) _
                + GridView.Columns("CLOSING " & bothWeight(0) & "").Width + GridView.Columns("CLOSING " & bothWeight(1) & "").Width _
                + IIf(GridView.Columns("CLOSING STNPCS").Visible, GridView.Columns("CLOSING STNPCS").Width, 0) + IIf(GridView.Columns("CLOSING STNWT").Visible, GridView.Columns("CLOSING STNWT").Width, 0) _
                + IIf(GridView.Columns("CLOSING DIAPCS").Visible, GridView.Columns("CLOSING DIAPCS").Width, 0) + IIf(GridView.Columns("CLOSING DIAWT").Visible, GridView.Columns("CLOSING DIAWT").Width, 0) _
                + IIf(GridView.Columns("CLOSING PREPCS").Visible, GridView.Columns("CLOSING PREPCS").Width, 0) + IIf(GridView.Columns("CLOSING PREWT").Visible, GridView.Columns("CLOSING PREWT").Width, 0)
            Else
                .Columns("RECEIPT PCS~RECEIPT " & Weight & "~RECEIPT STNPCS~RECEIPT DIAPCS~RECEIPT PREPCS~RECEIPT STNWT~RECEIPT DIAWT~RECEIPT PREWT").HeaderText = "RECEIPT"
                .Columns("RECEIPT PCS~RECEIPT " & Weight & "~RECEIPT STNPCS~RECEIPT DIAPCS~RECEIPT PREPCS~RECEIPT STNWT~RECEIPT DIAWT~RECEIPT PREWT").Width = _
                IIf(GridView.Columns("RECEIPT PCS").Visible, GridView.Columns("RECEIPT PCS").Width, 0) + GridView.Columns("RECEIPT " & Weight & "").Width _
                + IIf(GridView.Columns("RECEIPT STNPCS").Visible, GridView.Columns("RECEIPT STNPCS").Width, 0) + IIf(GridView.Columns("RECEIPT STNWT").Visible, GridView.Columns("RECEIPT STNWT").Width, 0) _
                + IIf(GridView.Columns("RECEIPT DIAPCS").Visible, GridView.Columns("RECEIPT DIAPCS").Width, 0) + IIf(GridView.Columns("RECEIPT DIAWT").Visible, GridView.Columns("RECEIPT DIAWT").Width, 0) _
                + IIf(GridView.Columns("RECEIPT PREPCS").Visible, GridView.Columns("RECEIPT PREPCS").Width, 0) + IIf(GridView.Columns("RECEIPT PREWT").Visible, GridView.Columns("RECEIPT PREWT").Width, 0)

                .Columns("PUR PCS~PUR " & Weight & "~PUR STNPCS~PUR DIAPCS~PUR PREPCS~PUR STNWT~PUR DIAWT~PUR PREWT").HeaderText = "PURCHASE"
                .Columns("PUR PCS~PUR " & Weight & "~PUR STNPCS~PUR DIAPCS~PUR PREPCS~PUR STNWT~PUR DIAWT~PUR PREWT").Width = _
                IIf(GridView.Columns("PUR PCS").Visible, GridView.Columns("PUR PCS").Width, 0) + GridView.Columns("PUR " & Weight & "").Width _
                + IIf(GridView.Columns("PUR STNPCS").Visible, GridView.Columns("PUR STNPCS").Width, 0) + IIf(GridView.Columns("PUR STNWT").Visible, GridView.Columns("PUR STNWT").Width, 0) _
                + IIf(GridView.Columns("PUR DIAPCS").Visible, GridView.Columns("PUR DIAPCS").Width, 0) + IIf(GridView.Columns("PUR DIAWT").Visible, GridView.Columns("PUR DIAWT").Width, 0) _
                + IIf(GridView.Columns("PUR PREPCS").Visible, GridView.Columns("PUR PREPCS").Width, 0) + IIf(GridView.Columns("PUR PREWT").Visible, GridView.Columns("PUR PREWT").Width, 0)

                .Columns("ISSUE PCS~ISSUE " & Weight & "~ISSUE STNPCS~ISSUE DIAPCS~ISSUE PREPCS~ISSUE STNWT~ISSUE DIAWT~ISSUE PREWT").HeaderText = "ISSUE"
                .Columns("ISSUE PCS~ISSUE " & Weight & "~ISSUE STNPCS~ISSUE DIAPCS~ISSUE PREPCS~ISSUE STNWT~ISSUE DIAWT~ISSUE PREWT").Width = _
                IIf(GridView.Columns("ISSUE PCS").Visible, GridView.Columns("ISSUE PCS").Width, 0) + GridView.Columns("ISSUE " & Weight & "").Width _
                + IIf(GridView.Columns("ISSUE STNPCS").Visible, GridView.Columns("ISSUE STNPCS").Width, 0) + IIf(GridView.Columns("ISSUE STNWT").Visible, GridView.Columns("ISSUE STNWT").Width, 0) _
                + IIf(GridView.Columns("ISSUE DIAPCS").Visible, GridView.Columns("ISSUE DIAPCS").Width, 0) + IIf(GridView.Columns("ISSUE DIAWT").Visible, GridView.Columns("ISSUE DIAWT").Width, 0) _
                + IIf(GridView.Columns("ISSUE PREPCS").Visible, GridView.Columns("ISSUE PREPCS").Width, 0) + IIf(GridView.Columns("ISSUE PREWT").Visible, GridView.Columns("ISSUE PREWT").Width, 0)

                .Columns("CLOSING PCS~CLOSING " & Weight & "~CLOSING STNPCS~CLOSING DIAPCS~CLOSING PREPCS~CLOSING STNWT~CLOSING DIAWT~CLOSING PREWT").HeaderText = "CLOSING"
                .Columns("CLOSING PCS~CLOSING " & Weight & "~CLOSING STNPCS~CLOSING DIAPCS~CLOSING PREPCS~CLOSING STNWT~CLOSING DIAWT~CLOSING PREWT").Width = _
                IIf(GridView.Columns("CLOSING PCS").Visible, GridView.Columns("CLOSING PCS").Width, 0) + GridView.Columns("CLOSING " & Weight & "").Width _
                + IIf(GridView.Columns("CLOSING STNPCS").Visible, GridView.Columns("CLOSING STNPCS").Width, 0) + IIf(GridView.Columns("CLOSING STNWT").Visible, GridView.Columns("CLOSING STNWT").Width, 0) _
                + IIf(GridView.Columns("CLOSING DIAPCS").Visible, GridView.Columns("CLOSING DIAPCS").Width, 0) + IIf(GridView.Columns("CLOSING DIAWT").Visible, GridView.Columns("CLOSING DIAWT").Width, 0) _
                + IIf(GridView.Columns("CLOSING PREPCS").Visible, GridView.Columns("CLOSING PREPCS").Width, 0) + IIf(GridView.Columns("CLOSING PREWT").Visible, GridView.Columns("CLOSING PREWT").Width, 0)
            End If


            .Columns("SCROLL").Visible = CType(GridView.Controls(0), HScrollBar).Visible
            .Columns("SCROLL").Width = CType(GridView.Controls(1), VScrollBar).Width
        End With
    End Function

    Private Sub GridView_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles GridView.Scroll
        If GridViewHead Is Nothing Then Exit Sub
        If Not GridViewHead.Columns.Count > 0 Then Exit Sub
        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
            GridViewHead.HorizontalScrollingOffset = e.NewValue
        End If
        Try
            If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then
                GridViewHead.HorizontalScrollingOffset = e.NewValue
                GridViewHead.Columns("SCROLL").Visible = CType(GridView.Controls(0), HScrollBar).Visible
                GridViewHead.Columns("SCROLL").Width = CType(GridView.Controls(1), VScrollBar).Width
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub
End Class
Public Class frmMetalOrnamentDetailedStockView_New_Properties
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

End Class
